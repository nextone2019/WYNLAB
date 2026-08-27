# 로컬 개발 PC의 IIS를 처음 한 번 세팅하는 스크립트 - 관리자 권한 PowerShell에서 실행해야 한다
# (IIS 관리자 -> 마우스 오른쪽 -> "관리자 권한으로 실행"이 아니라, PowerShell 자체를 관리자
# 권한으로 켠 다음 이 파일을 실행). Windows 기능 활성화/IIS 사이트 생성은 관리자 권한 없이는
# 막힌다(elevation 오류).
#
# 서버 배포판(SERVER_SETUP.md)과 완전히 같은 구조를 이 PC에도 그대로 만든다 - 사이트 이름/포트만
# 로컬용으로 바꿨다:
#   D:\WYNLAB_SVC\Api\       -> IIS 사이트 "WYNLAB_SVC-Api",       포트 8090
#   D:\WYNLAB_SVC\ClickOnce\ -> IIS 사이트 "WYNLAB_SVC-ClickOnce", 포트 8091
#     (그 아래 가상 디렉터리로 Modules/CoreAssembly/Assets 연결 - HTTP로 서빙)
#
# 실행 전에 확인: .NET 8 Hosting Bundle이 설치되어 있어야 한다(이 스크립트가 설치하지는 않음 -
# 설치 프로그램 실행은 사용자가 직접 눈으로 보고 진행하는 게 안전해서 의도적으로 뺐다).
# 없으면 아래에서 받아서 설치:
#   https://dotnet.microsoft.com/en-us/download/dotnet/8.0
#   -> "Hosting Bundle" (ASP.NET Core Runtime 8.0.x - Windows Hosting Bundle) 다운로드/설치
#   -> 설치 후 IIS를 재시작해야 모듈이 인식된다(이 스크립트 마지막에서 재시작함).

$ErrorActionPreference = "Stop"

function Test-Admin {
    $principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}
if (-not (Test-Admin)) {
    throw "관리자 권한으로 실행해야 합니다. PowerShell을 관리자 권한으로 다시 열고 이 스크립트를 실행하세요."
}

Write-Host "=== 1) IIS 기능 활성화 (Windows 11) ===" -ForegroundColor Cyan
$features = @(
    "IIS-WebServerRole", "IIS-WebServer", "IIS-CommonHttpFeatures",
    "IIS-StaticContent", "IIS-DefaultDocument", "IIS-HttpErrors", "IIS-HttpLogging",
    "IIS-RequestFiltering", "IIS-ManagementConsole"
)
Enable-WindowsOptionalFeature -Online -FeatureName $features -All -NoRestart | Out-Null
Write-Host "  IIS 기능 활성화 완료 (재부팅은 필요 없음)"

Write-Host "=== 2) .NET Hosting Bundle 확인 ===" -ForegroundColor Cyan
$ancmPath = "C:\Program Files\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll"
if (-not (Test-Path $ancmPath)) {
    Write-Host "  .NET 8 Hosting Bundle이 설치되어 있지 않습니다." -ForegroundColor Yellow
    Write-Host "  https://dotnet.microsoft.com/en-us/download/dotnet/8.0 에서 'Hosting Bundle'을 받아 설치한 뒤" -ForegroundColor Yellow
    Write-Host "  이 스크립트를 다시 실행하세요." -ForegroundColor Yellow
    throw "Hosting Bundle 미설치로 중단"
}
Write-Host "  확인됨: $ancmPath"

Write-Host "=== 3) 폴더 확인 ===" -ForegroundColor Cyan
$svcRoot = "D:\WYNLAB_SVC"
New-Item -ItemType Directory -Force -Path `
    "$svcRoot\Api", "$svcRoot\ClickOnce", `
    "$svcRoot\Modules\SM", "$svcRoot\CoreAssembly", "$svcRoot\Assets" | Out-Null

Write-Host "=== 4) IIS 앱풀 + 사이트 ===" -ForegroundColor Cyan
Import-Module WebAdministration

# Get-IISAppPool/Get-Website는 IISAdministration 모듈 소속이라(WebAdministration과 다른
# 모듈), Import-Module WebAdministration만 한 상태에선 못 찾아서 "없다"고 잘못 판단하고
# New-WebAppPool을 또 시도해 "이미 존재합니다" 오류가 났다(실제로 겪음). IIS:\ PSDrive에
# Test-Path로 직접 물어보는 쪽이 WebAdministration 하나로 항상 일관되게 동작한다.
if (-not (Test-Path "IIS:\AppPools\WYNLAB_SVC-Api")) {
    New-WebAppPool -Name "WYNLAB_SVC-Api" | Out-Null
}
Set-ItemProperty "IIS:\AppPools\WYNLAB_SVC-Api" -Name managedRuntimeVersion -Value ""
if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC-Api")) {
    New-Website -Name "WYNLAB_SVC-Api" -PhysicalPath "$svcRoot\Api" -ApplicationPool "WYNLAB_SVC-Api" -Port 8090 | Out-Null
}

if (-not (Test-Path "IIS:\AppPools\WYNLAB_SVC-ClickOnce")) {
    New-WebAppPool -Name "WYNLAB_SVC-ClickOnce" | Out-Null
}
if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC-ClickOnce")) {
    New-Website -Name "WYNLAB_SVC-ClickOnce" -PhysicalPath "$svcRoot\ClickOnce" -ApplicationPool "WYNLAB_SVC-ClickOnce" -Port 8091 | Out-Null
}

Write-Host "=== 5) ClickOnce 확장자 MIME 타입 등록 ===" -ForegroundColor Cyan
$clickOnceMime = @{ ".application" = "application/x-ms-application"; ".manifest" = "application/x-ms-manifest"; ".deploy" = "application/octet-stream" }
foreach ($ext in $clickOnceMime.Keys) {
    try {
        Add-WebConfigurationProperty -PSPath "IIS:\Sites\WYNLAB_SVC-ClickOnce" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=$ext; mimeType=$clickOnceMime[$ext]}
    } catch {
        Write-Host "  ($ext 이미 등록됨 - 무시)" -ForegroundColor DarkGray
    }
}

Write-Host "=== 6) Modules/CoreAssembly/Assets 가상 디렉터리 + .dll/.pdb MIME 타입 ===" -ForegroundColor Cyan
foreach ($vdir in "Modules", "CoreAssembly", "Assets") {
    if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC-ClickOnce\$vdir")) {
        New-WebVirtualDirectory -Site "WYNLAB_SVC-ClickOnce" -Name $vdir -PhysicalPath "$svcRoot\$vdir" | Out-Null
    }
}
foreach ($vdir in "Modules", "CoreAssembly") {
    foreach ($ext in ".dll", ".pdb") {
        try {
            Add-WebConfigurationProperty -PSPath "IIS:\Sites\WYNLAB_SVC-ClickOnce\$vdir" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=$ext; mimeType="application/octet-stream"}
        } catch {
            Write-Host "  ($vdir$ext 이미 등록됨 - 무시)" -ForegroundColor DarkGray
        }
    }
}

Write-Host "=== 7) IIS 재시작 (Hosting Bundle 인식) ===" -ForegroundColor Cyan
iisreset | Out-Null

Write-Host "=== 완료 ===" -ForegroundColor Green
Write-Host "  다음: Deploy-Local.ps1을 실행해서 D:\WYNLAB_SVC\Api 등에 실제 빌드 결과물을 채워 넣으세요."
Write-Host "  확인: http://localhost:8090/  (Api, 404가 떠도 연결 자체는 되는지가 핵심)"
