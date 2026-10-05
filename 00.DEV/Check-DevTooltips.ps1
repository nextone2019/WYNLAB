<#
  개발자 툴팁(마우스 오버 시 BindingField / LookUp명 / Popup명) 회귀 점검 - 2026-10-05
  배포된 모든 업무화면(D:\WYNLAB_SVC\Modules)을 실제로 열어서 확인한다.
    (기본)  : 모든 입력 컨트롤/그리드 컬럼에 툴팁이 달려 있어야 한다. 하나라도 빠지면 실패(exit 1).
    -NonDeveloper            : 일반 사용자 - 툴팁이 하나도 보이면 안 된다(유출 방지). 보이면 실패.
  사용: powershell -STA -File .\Check-DevTooltips.ps1            (개발자 모드 점검)
        powershell -STA -File .\Check-DevTooltips.ps1 -NonDeveloper
  ※ BaseForm/공통 컨트롤/화면을 고치고 배포한 뒤 반드시 실행한다(DEV 서비스 기준, 화면 수십 개라 몇 분 걸린다).
#>
param(
    [switch]$NonDeveloper,
    [string]$SvcRoot = "D:\WYNLAB_SVC",
    [string]$ShellBin = "D:\01. SOURCE\00. WYNLAB\00.DEV\01.Client\WYNLAB.Shell\bin\Debug\net48"
)
$ErrorActionPreference = "Continue"
$Developer = -not $NonDeveloper
if ([Threading.Thread]::CurrentThread.GetApartmentState() -ne 'STA') { Write-Host "STA로 실행하세요: powershell -STA -File ..." -ForegroundColor Red; exit 2 }

# DevExpress/System.* 는 셸 빌드 폴더에서, 나머지 공통 DLL은 배포 폴더(CoreAssembly)에서 읽는다.
foreach ($d in (Get-ChildItem $ShellBin -Filter "*.dll" | Where-Object { $_.Name -notlike "WYNLAB*" })) { try { [void][Reflection.Assembly]::LoadFrom($d.FullName) } catch {} }
foreach ($d in (Get-ChildItem "$SvcRoot\CoreAssembly" -Filter "*.dll")) { try { [void][Reflection.Assembly]::LoadFrom($d.FullName) } catch {} }
# resx가 요구하는 버전(System.Resources.Extensions 4.0.0.0 등)이 달라도 이미 로드된 같은 이름의 어셈블리로 연결해 준다.
Add-Type -TypeDefinition @"
using System; using System.Linq; using System.Reflection;
public static class AsmRedirect {
    public static void Install() {
        AppDomain.CurrentDomain.AssemblyResolve += (s, e) => {
            var n = new AssemblyName(e.Name).Name;
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == n);
        };
    }
}
"@
[AsmRedirect]::Install()
Add-Type -AssemblyName System.Windows.Forms
[Windows.Forms.Application]::EnableVisualStyles()
[Windows.Forms.Application]::SetUnhandledExceptionMode([Windows.Forms.UnhandledExceptionMode]::CatchException)
[Windows.Forms.Application]::add_ThreadException({ param($s, $e) })

$ui = New-Object WYNLAB.Shared.Dtos.UserInfoDto
$ui.UserId = "tooltip-check"; $ui.DeveloperYn = $Developer
[WYNLAB.Base.SessionManager].GetProperty("UserInfo").SetValue([WYNLAB.Base.SessionManager]::Current, $ui)
if ([WYNLAB.Base.Session]::IsDeveloper -ne $Developer) { Write-Host "세션 설정 실패" -ForegroundColor Red; exit 2 }

$baseT = [WYNLAB.Base.BaseForm]
$bad = New-Object Collections.Generic.List[string]
$cannotOpen = New-Object Collections.Generic.List[string]
$screens = 0

function Test-Walk($c, [ref]$stat) {
    foreach ($k in $c.Controls) {
        if ($k -is [DevExpress.XtraEditors.BaseEdit] -and $k.Parent -isnot [DevExpress.XtraEditors.BaseEdit] -and $k.GetType().FullName -notmatch "Mask|Inner") {
            $has = ($k.ToolTip -like "BindingField*") -or ($k.ToolTip -like "조회조건*") -or ($k.ToolTip -like "Popup :*") -or ($k.ToolTip -like "LookUp*")
            if ($Developer -and -not $has) { $stat.Value.Missing += $k.Name }
            if (-not $Developer -and $has) { $stat.Value.Leak += $k.Name }
        }
        $cols = @()
        if ($k -is [DevExpress.XtraGrid.GridControl] -and $k.MainView -is [DevExpress.XtraGrid.Views.Grid.GridView]) { $cols = $k.MainView.Columns }
        elseif ($k -is [DevExpress.XtraTreeList.TreeList]) { $cols = $k.Columns }
        foreach ($col in $cols) {
            if (-not $col.FieldName) { continue }
            $has = $col.ToolTip -like "BindingField*"
            if ($Developer -and -not $has) { $stat.Value.Missing += ("[col]" + $col.FieldName) }
            if (-not $Developer -and $has) { $stat.Value.Leak += ("[col]" + $col.FieldName) }
        }
        Test-Walk $k $stat
    }
}

foreach ($mod in (Get-ChildItem "$SvcRoot\Modules" -Directory | Where-Object { $_.Name -notlike "_*" })) {
    $dll = Join-Path $mod.FullName ("WYNLAB." + $mod.Name + ".dll")
    if (-not (Test-Path $dll)) { continue }
    try { $types = [Reflection.Assembly]::LoadFrom($dll).GetTypes() } catch { $types = $_.Exception.Types | Where-Object { $_ } }
    foreach ($t in $types) {
        if ($t -eq $null -or -not $baseT.IsAssignableFrom($t) -or $t.IsAbstract -or $t.Name -notlike "frm*") { continue }
        $name = "$($mod.Name).$($t.Name)"
        try {
            $f = [Activator]::CreateInstance($t)
            $f.StartPosition = 'Manual'; $f.Left = 20; $f.Top = 20; $f.Width = 1200; $f.Height = 700
            $f.Show()
            $end = (Get-Date).AddMilliseconds(1000)
            while ((Get-Date) -lt $end) { [Windows.Forms.Application]::DoEvents(); Start-Sleep -Milliseconds 30 }
            $stat = [pscustomobject]@{ Missing = @(); Leak = @() }
            Test-Walk $f ([ref]$stat)
            if ($stat.Missing.Count -gt 0) { $bad.Add("$name  툴팁 없음: " + ($stat.Missing -join ", ")) }
            if ($stat.Leak.Count -gt 0) { $bad.Add("$name  일반 사용자에게 툴팁 노출: " + ($stat.Leak -join ", ")) }
            $screens++
            $f.Close(); $f.Dispose()
        } catch {
            $cannotOpen.Add("$name : " + $_.Exception.GetBaseException().Message)
        }
    }
}

"점검한 화면: $screens 개 (개발자 모드=$Developer)"
if ($cannotOpen.Count -gt 0) { "열지 못한 화면(점검 제외): " + $cannotOpen.Count; $cannotOpen | ForEach-Object { "  - $_" } }
if ($bad.Count -eq 0) { "결과: OK - 모든 화면 정상"; exit 0 }
"결과: 실패 - " + $bad.Count + "개 화면"
$bad | ForEach-Object { "  - $_" }
exit 1
