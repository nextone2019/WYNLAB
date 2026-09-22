# 로컬 개발 PC에 "두 번째 서비스"(NEXTONE DB를 바라보는 별도 API)를 세팅하는 스크립트 -
# 관리자 권한 PowerShell에서 실행해야 한다(Setup-LocalIIS.ps1과 같은 이유 - IIS 사이트 생성은
# elevation 없이는 막힌다).
#
# 화면 DLL(Modules)/프레임워크(CoreAssembly)/ClickOnce 설치파일은 DB와 무관하게 완전히 동일한
# 바이너리라서 첫 번째 서비스(포트 8091)의 것을 그대로 재사용한다 - 새 정적 사이트를 또 만들
# 필요가 없다. 이 스크립트가 새로 만드는 건 API 사이트 하나뿐이다:
#   D:\WYNLAB_SVC2\Api\ -> IIS 사이트 "WYNLAB_SVC2-Api", 포트 8092 (NEXTONE DB를 바라봄)
#
# 클라이언트에서 이 서비스로 전환하려면 appsettings.Dev.json의 Environments에 새 항목을
# 추가하면 된다(Deploy-Local2.ps1이 이 사이트에 실제 게시물을 채워 넣는다) - 예:
#   "NEXTONE": {
#     "ApiBaseUrl": "http://localhost:8092/",
#     "ModulesPath": "http://localhost:8091/Modules",       <- 8091(첫 서비스) 그대로 재사용
#     "CoreAssemblyPath": "http://localhost:8091/CoreAssembly",
#     "AssetsPath": "http://localhost:8091/Assets"
#   }
#
# 사전조건: Setup-LocalIIS.ps1을 이미 한 번 실행해서 .NET 8 Hosting Bundle이 확인된 상태여야 한다
# (이 스크립트는 그 확인을 반복하지 않는다).

$ErrorActionPreference = "Stop"

function Test-Admin {
    $principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}
if (-not (Test-Admin)) {
    throw "관리자 권한으로 실행해야 합니다. PowerShell을 관리자 권한으로 다시 열고 이 스크립트를 실행하세요."
}

Write-Host "=== 1) 폴더 확인 ===" -ForegroundColor Cyan
$svcRoot = "D:\WYNLAB_SVC2"
New-Item -ItemType Directory -Force -Path "$svcRoot\Api" | Out-Null

Write-Host "=== 2) IIS 앱풀 + 사이트 ===" -ForegroundColor Cyan
Import-Module WebAdministration

if (-not (Test-Path "IIS:\AppPools\WYNLAB_SVC2-Api")) {
    New-WebAppPool -Name "WYNLAB_SVC2-Api" | Out-Null
}
Set-ItemProperty "IIS:\AppPools\WYNLAB_SVC2-Api" -Name managedRuntimeVersion -Value ""
if (-not (Test-Path "IIS:\Sites\WYNLAB_SVC2-Api")) {
    New-Website -Name "WYNLAB_SVC2-Api" -PhysicalPath "$svcRoot\Api" -ApplicationPool "WYNLAB_SVC2-Api" -Port 8092 | Out-Null
}

Write-Host "=== 3) IIS 재시작 (새 사이트 인식) ===" -ForegroundColor Cyan
iisreset | Out-Null

Write-Host "=== 완료 ===" -ForegroundColor Green
Write-Host "  다음: Deploy-Local2.ps1을 실행해서 D:\WYNLAB_SVC2\Api에 실제 빌드 결과물을 채워 넣으세요."
Write-Host "  확인: http://localhost:8092/  (Api, 404가 떠도 연결 자체는 되는지가 핵심)"
