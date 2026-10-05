# 로컬 개발 PC에 "FADU 서비스"를 세팅하는 스크립트 - 관리자 권한 PowerShell에서 실행해야 한다(Setup-LocalIIS.ps1과 같은 이유).
# WYNLAB_DEV 서비스(8090/8091, D:\WYNLAB_SVC)와 완전히 분리된 독립 서비스다 - API도, 화면 DLL/핵심 어셈블리/에셋도 따로 둔다.
#   D:\WYNLAB_SVC_FADU\Api\          -> IIS 사이트 "WYNLAB_SVC_FADU-Api",    포트 8093 (FADU DB)
#   D:\WYNLAB_SVC_FADU\Modules\      \
#   D:\WYNLAB_SVC_FADU\CoreAssembly\  } IIS 사이트 "WYNLAB_SVC_FADU-Static", 포트 8094 (가상 디렉터리 3개, .dll/.pdb MIME)
#   D:\WYNLAB_SVC_FADU\Assets\       /
# 클라이언트(로그인 화면 서비스관리)의 FADU 서비스는 API http://localhost:8093/, 정적 리소스 http://localhost:8094 를 본다.
# 사전조건: Setup-LocalIIS.ps1을 이미 한 번 실행해서 .NET 8 Hosting Bundle이 확인된 상태여야 한다. 여러 번 실행해도 안전하다.

$ErrorActionPreference = "Stop"

function Test-Admin {
    $principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}
if (-not (Test-Admin)) {
    throw "관리자 권한으로 실행해야 합니다. PowerShell을 관리자 권한으로 다시 열고 이 스크립트를 실행하세요."
}

Write-Host "=== 1) 폴더 확인 ===" -ForegroundColor Cyan
$svcRoot = "D:\WYNLAB_SVC_FADU"
New-Item -ItemType Directory -Force -Path "$svcRoot\Api", "$svcRoot\Static", "$svcRoot\Modules", "$svcRoot\CoreAssembly", "$svcRoot\Assets" | Out-Null

Write-Host "=== 2) IIS 앱풀 + API 사이트 ===" -ForegroundColor Cyan
Import-Module WebAdministration

if (-not (Test-Path "IIS:\AppPools\WYNLAB_SVC_FADU-Api")) { New-WebAppPool -Name "WYNLAB_SVC_FADU-Api" | Out-Null }
Set-ItemProperty "IIS:\AppPools\WYNLAB_SVC_FADU-Api" -Name managedRuntimeVersion -Value ""
if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC_FADU-Api")) {
    New-Website -Name "WYNLAB_SVC_FADU-Api" -PhysicalPath "$svcRoot\Api" -ApplicationPool "WYNLAB_SVC_FADU-Api" -Port 8093 | Out-Null
}

Write-Host "=== 3) IIS 앱풀 + 정적 리소스 사이트 (Modules/CoreAssembly/Assets) ===" -ForegroundColor Cyan
if (-not (Test-Path "IIS:\AppPools\WYNLAB_SVC_FADU-Static")) { New-WebAppPool -Name "WYNLAB_SVC_FADU-Static" | Out-Null }
if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC_FADU-Static")) {
    New-Website -Name "WYNLAB_SVC_FADU-Static" -PhysicalPath "$svcRoot\Static"-ApplicationPool "WYNLAB_SVC_FADU-Static" -Port 8094 | Out-Null
}
foreach ($vdir in "Modules", "CoreAssembly", "Assets") {
    if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC_FADU-Static\$vdir")) {
        New-WebVirtualDirectory -Site "WYNLAB_SVC_FADU-Static" -Name $vdir -PhysicalPath "$svcRoot\$vdir" | Out-Null
    }
}
foreach ($vdir in "Modules", "CoreAssembly") {
    foreach ($ext in ".dll", ".pdb") {
        try {
            Add-WebConfigurationProperty -PSPath "IIS:\Sites\WYNLAB_SVC_FADU-Static\$vdir" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=$ext; mimeType="application/octet-stream"}
        } catch {
            Write-Host "  ($vdir$ext 이미 등록됨 - 무시)" -ForegroundColor DarkGray
        }
    }
}

Write-Host "=== 4) IIS 재시작 (새 사이트 인식) ===" -ForegroundColor Cyan
iisreset | Out-Null

Write-Host "=== 완료 ===" -ForegroundColor Green
Write-Host "  API:    http://localhost:8093/health"
Write-Host "  정적:   http://localhost:8094/Modules/manifest.json"
Write-Host "  이후 FADU 화면/핵심 DLL 갱신은 Deploy-Local-Fadu.ps1 -Static 으로 배포합니다."
