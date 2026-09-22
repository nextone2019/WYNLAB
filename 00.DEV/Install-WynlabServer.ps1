# 운영 서버(이 스크립트가 실행되는 바로 이 머신) 최초 설치 - 한 번 실행으로 SERVER_SETUP.md의
# 1~5번(폴더/방화벽/IIS 사이트+앱풀/MIME 타입/가상 디렉터리)을 전부 처리하고, 이미 준비된 배포
# 산출물(Api 게시물 + CoreAssembly + Modules + ClickOnce)을 제자리에 복사한 다음, web.config에
# DB/JWT(+선택적으로 SMTP) 값을 심어준다.
#
# **반드시 이 서버 자신의 콘솔(RDP)에서, 관리자 권한 PowerShell로 실행한다** - 원격에서 이
# 스크립트를 대신 돌려줄 수 없다(로컬 PC 개발 도구는 이 서버에 접속할 방법이 없음).
#
# 전제 조건 - 이 스크립트와 같은 폴더(또는 -PackageRoot로 지정한 폴더)에 아래 4개 폴더가
# 이미 있어야 한다(Build-ServerPackage.ps1이 개발 PC에서 만들어서 이 폴더 그대로 서버로
# 복사해주는 결과물 - USB/외장하드 등으로 옮기면 된다):
#   <PackageRoot>\Api\           (WYNLAB.Api  dotnet publish 결과물)
#   <PackageRoot>\ClickOnce\     (WYNLAB.exe  ClickOnce 게시물)
#   <PackageRoot>\CoreAssembly\  (BaseForm/Controls/Shared/Popup + manifest.json)
#   <PackageRoot>\Modules\       (화면 모듈 dll + manifest.json)
#
# 여러 번 실행해도 안전하다(idempotent) - 이미 있는 폴더/공유/방화벽규칙/사이트는 건너뛰고,
# 파일은 robocopy /MIR로 항상 최신 산출물과 똑같이 맞춘다. 재배포(업데이트) 때도 그대로 다시
# 실행하면 된다 - 단, web.config 시크릿은 -DbConnectionString/-JwtSecretKey를 다시 주지
# 않으면 기존 값을 그대로 둔다(아래 참고).
#
# 이 서버는 공인 IP(외부 인터넷에 노출)라서 SERVER_SETUP.md 원본(사내망 전용 서버 기준)과
# 달리 **SMB(445) 공유는 만들지 않는다** - SMB를 인터넷에 열어두는 것은 랜섬웨어의 대표적인
# 침투 경로라 절대 하면 안 된다(SERVER_SETUP.md 5-1번의 같은 판단, 다만 그 문서의 원래
# 서버는 사내망 전용이라 공유를 같이 만들었던 것뿐). Modules/CoreAssembly/Assets는 전부
# ClickOnce 사이트(8091) 밑의 HTTP 가상 디렉터리로만 노출한다 - 그래서 appsettings.Prod.json도
# 이번 서버(221.150.162.198) 기준으로 전부 http:// 경로로 맞춰뒀다(UNC 아님).
#
# 사용법 (서버 콘솔, 관리자 PowerShell):
#   .\Install-WynlabServer.ps1
#     -DbConnectionString "Server=tcp:localhost,1433;Database=WYNLAB;User Id=wynlab;Password=...;TrustServerCertificate=True;" `
#     -JwtSecretKey "<64자 이상 임의의 긴 문자열>"
#
#   SMTP(비밀번호 찾기 메일 발송)까지 같이 넣으려면 -Smtp* 파라미터를 추가로 준다(선택).
#   DB/JWT 값을 안 주고 실행하면 그 부분만 건너뛰고, web.config는 나중에 손으로 채워도 된다
#   (SERVER_SETUP.md 6-2 참고) - 인프라(폴더/사이트/방화벽 등) 설정은 그대로 진행된다.

param(
    [string]$PackageRoot = $PSScriptRoot,
    [string]$InstallRoot = "D:\WYNLAB",
    [int]$ApiPort = 8090,
    [int]$ClickOncePort = 8091,

    [string]$DbConnectionString,
    [string]$JwtSecretKey,

    [string]$SmtpHost,
    [int]$SmtpPort = 587,
    [string]$SmtpUserName,
    [string]$SmtpPassword,
    [string]$SmtpFromAddress,
    [string]$SmtpFromDisplayName = "WYNLAB"
)

$ErrorActionPreference = "Stop"

function Write-Step([string]$text) { Write-Host "=== $text ===" -ForegroundColor Cyan }
function Write-Ok([string]$text) { Write-Host "  $text" -ForegroundColor Green }
function Write-Skip([string]$text) { Write-Host "  (건너뜀: $text)" -ForegroundColor DarkGray }

# Get-WebConfigurationProperty로 사전 확인해도, 서버 전역(applicationHost.config)에 이미
# 등록된 MIME 타입은 사이트 단위 조회에서 "있음"으로 안 잡힐 때가 있다(상속 병합 방식의
# 한계) - 그래서 사전 확인 대신 그냥 추가를 시도하고, "중복 키" 오류만 조용히 건너뛴다
# (SERVER_SETUP.md 5번 "이미 등록되어 있다는 오류가 나면 무시해도 된다"와 같은 판단).
# 그 외의 진짜 오류(권한 부족 등)는 그대로 위로 던져서 스크립트가 멈추게 한다.
function Add-MimeTypeSafe([string]$pspath, [string]$ext, [string]$mimeType) {
    try {
        Add-WebConfigurationProperty -PSPath $pspath -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension = $ext; mimeType = $mimeType }
        Write-Ok "등록: $ext"
    }
    catch {
        if ($_.Exception.Message -match "고유한 키|already exists|Cannot add duplicate") {
            Write-Skip "$ext (이미 등록되어 있음 - 서버 전역 기본값일 수 있음)"
        }
        else {
            throw
        }
    }
}

$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)) {
    throw "관리자 권한 PowerShell에서 실행해주세요(우클릭 -> 관리자 권한으로 실행)."
}

$modules = "AP", "BA", "MA", "PR", "SA", "SM", "SYS"

Write-Step "1) 폴더 구조 생성"
$dirsToCreate = @("$InstallRoot\Api", "$InstallRoot\ClickOnce", "$InstallRoot\CoreAssembly", "$InstallRoot\Assets")
$dirsToCreate += $modules | ForEach-Object { "$InstallRoot\Modules\$_" }
foreach ($dir in $dirsToCreate) {
    if (Test-Path $dir) { Write-Skip "$dir (이미 있음)" }
    else { New-Item -ItemType Directory -Force -Path $dir | Out-Null; Write-Ok "생성: $dir" }
}

Write-Step "2) 방화벽 규칙 ($ApiPort / $ClickOncePort) - SMB(445)는 만들지 않음(공인 IP라 보안상 제외)"
foreach ($rule in @(
    @{ Name = "WYNLAB API ($ApiPort)"; Port = $ApiPort },
    @{ Name = "WYNLAB ClickOnce ($ClickOncePort)"; Port = $ClickOncePort }
)) {
    if (Get-NetFirewallRule -DisplayName $rule.Name -ErrorAction SilentlyContinue) {
        Write-Skip "$($rule.Name) (이미 있음)"
    }
    else {
        New-NetFirewallRule -DisplayName $rule.Name -Direction Inbound -LocalPort $rule.Port -Protocol TCP -Action Allow | Out-Null
        Write-Ok "생성: $($rule.Name)"
    }
}

Write-Step "3) IIS 앱풀 + 사이트"
Import-Module WebAdministration

if (Get-WebAppPoolState -Name "WYNLAB-Api" -ErrorAction SilentlyContinue) {
    Write-Skip "앱풀 WYNLAB-Api (이미 있음)"
}
else {
    New-WebAppPool -Name "WYNLAB-Api" | Out-Null
    # "관리되는 코드 없음" 필수 - ASP.NET Core는 자체 프로세스로 동작하고 IIS는 리버스 프록시 역할만 한다.
    Set-ItemProperty "IIS:\AppPools\WYNLAB-Api" -Name managedRuntimeVersion -Value ""
    Write-Ok "생성: 앱풀 WYNLAB-Api (관리되는 코드 없음)"
}
if (Get-Website -Name "WYNLAB-Api" -ErrorAction SilentlyContinue) {
    Write-Skip "사이트 WYNLAB-Api (이미 있음)"
}
else {
    New-Website -Name "WYNLAB-Api" -PhysicalPath "$InstallRoot\Api" -ApplicationPool "WYNLAB-Api" -Port $ApiPort | Out-Null
    Write-Ok "생성: 사이트 WYNLAB-Api (포트 $ApiPort)"
}

if (Get-WebAppPoolState -Name "WYNLAB-ClickOnce" -ErrorAction SilentlyContinue) {
    Write-Skip "앱풀 WYNLAB-ClickOnce (이미 있음)"
}
else {
    New-WebAppPool -Name "WYNLAB-ClickOnce" | Out-Null
    Write-Ok "생성: 앱풀 WYNLAB-ClickOnce"
}
if (Get-Website -Name "WYNLAB-ClickOnce" -ErrorAction SilentlyContinue) {
    Write-Skip "사이트 WYNLAB-ClickOnce (이미 있음)"
}
else {
    New-Website -Name "WYNLAB-ClickOnce" -PhysicalPath "$InstallRoot\ClickOnce" -ApplicationPool "WYNLAB-ClickOnce" -Port $ClickOncePort | Out-Null
    Write-Ok "생성: 사이트 WYNLAB-ClickOnce (포트 $ClickOncePort)"
}

Write-Step "4) ClickOnce MIME 타입 등록 (.application / .manifest / .deploy)"
foreach ($mime in @(
    @{ Ext = ".application"; Type = "application/x-ms-application" },
    @{ Ext = ".manifest"; Type = "application/x-ms-manifest" },
    @{ Ext = ".deploy"; Type = "application/octet-stream" }
)) {
    Add-MimeTypeSafe -pspath "IIS:\Sites\WYNLAB-ClickOnce" -ext $mime.Ext -mimeType $mime.Type
}

Write-Step "5) Modules/CoreAssembly/Assets 가상 디렉터리 + .dll/.pdb MIME 타입"
foreach ($vdir in @(
    @{ Name = "Modules"; Path = "$InstallRoot\Modules" },
    @{ Name = "CoreAssembly"; Path = "$InstallRoot\CoreAssembly" },
    @{ Name = "Assets"; Path = "$InstallRoot\Assets" }
)) {
    if (Get-WebVirtualDirectory -Site "WYNLAB-ClickOnce" -Name $vdir.Name -ErrorAction SilentlyContinue) {
        Write-Skip "가상 디렉터리 $($vdir.Name) (이미 있음)"
    }
    else {
        New-WebVirtualDirectory -Site "WYNLAB-ClickOnce" -Name $vdir.Name -PhysicalPath $vdir.Path | Out-Null
        Write-Ok "생성: 가상 디렉터리 $($vdir.Name)"
    }
}
foreach ($vdirName in "Modules", "CoreAssembly") {
    foreach ($ext in ".dll", ".pdb") {
        Add-MimeTypeSafe -pspath "IIS:\Sites\WYNLAB-ClickOnce\$vdirName" -ext $ext -mimeType "application/octet-stream"
    }
}

Write-Step "6) 배포 산출물 복사 (Package -> $InstallRoot)"
foreach ($part in "Api", "ClickOnce", "CoreAssembly", "Modules") {
    $src = Join-Path $PackageRoot $part
    if (-not (Test-Path $src)) {
        Write-Skip "$part (패키지에 없음 - $src) - 이 부분은 나중에 따로 배포하세요"
        continue
    }
    robocopy $src "$InstallRoot\$part" /MIR /NFL /NDL /NJH /NJS | Out-Null
    if ($LASTEXITCODE -ge 8) { throw "$part 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
    $global:LASTEXITCODE = 0
    Write-Ok "복사 완료: $part"
}

if ($DbConnectionString -or $JwtSecretKey -or $SmtpHost) {
    Write-Step "7) web.config에 DB/JWT/SMTP 값 반영"
    $webConfigPath = "$InstallRoot\Api\web.config"
    if (-not (Test-Path $webConfigPath)) {
        Write-Skip "web.config를 못 찾음($webConfigPath) - Api가 아직 복사 안 됐으면 6번을 먼저 확인하세요"
    }
    else {
        [xml]$webConfig = Get-Content $webConfigPath
        $envVars = $webConfig.configuration.location.'system.webServer'.aspNetCore.environmentVariables.environmentVariable
        foreach ($ev in $envVars) {
            switch ($ev.name) {
                "ConnectionStrings__WynlabDb" { if ($DbConnectionString) { $ev.value = $DbConnectionString } }
                "Jwt__SecretKey" { if ($JwtSecretKey) { $ev.value = $JwtSecretKey } }
                "Smtp__Host" { if ($SmtpHost) { $ev.value = $SmtpHost } }
                "Smtp__Port" { if ($SmtpHost) { $ev.value = [string]$SmtpPort } }
                "Smtp__UserName" { if ($SmtpHost) { $ev.value = $SmtpUserName } }
                "Smtp__Password" { if ($SmtpHost) { $ev.value = $SmtpPassword } }
                "Smtp__FromAddress" { if ($SmtpHost) { $ev.value = $SmtpFromAddress } }
                "Smtp__FromDisplayName" { if ($SmtpHost) { $ev.value = $SmtpFromDisplayName } }
            }
        }
        $webConfig.Save($webConfigPath)
        Write-Ok "web.config 갱신 완료"
        Restart-WebAppPool -Name "WYNLAB-Api"
        Write-Ok "앱풀 WYNLAB-Api 재시작"
    }
}
else {
    Write-Step "7) web.config DB/JWT/SMTP"
    Write-Skip "-DbConnectionString/-JwtSecretKey를 안 주셔서 건너뜀 - SERVER_SETUP.md 6-2 참고해서 나중에 직접 채워주세요"
}

Write-Host ""
Write-Host "=== 설치 완료 - 확인 체크리스트 ===" -ForegroundColor Green
$hostForCheck = "221.150.162.198"
Write-Host "  [ ] http://$($hostForCheck):$ApiPort/                                -> API 응답(404여도 연결만 되면 OK)"
Write-Host "  [ ] http://$($hostForCheck):$ClickOncePort/CoreAssembly/manifest.json  -> JSON 보임"
Write-Host "  [ ] http://$($hostForCheck):$ClickOncePort/Modules/manifest.json       -> JSON 보임"
Write-Host "  [ ] http://$($hostForCheck):$ClickOncePort/WYNLAB.application          -> ClickOnce 설치 시작"
