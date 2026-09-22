# 로컬 개발 PC의 "두 번째 서비스"(NEXTONE DB) 배포 스크립트 - Deploy-Local.ps1의 API만 다시
# 게시하는 버전이다. 화면 DLL(Modules)/프레임워크(CoreAssembly)/ClickOnce는 DB와 무관하게 첫
# 번째 서비스(D:\WYNLAB_SVC)의 것과 완전히 동일한 바이너리라 다시 만들지 않고 그대로 재사용한다
# (Setup-LocalIIS2.ps1 주석 참고) - 이 스크립트는 D:\WYNLAB_SVC2\Api만 건드린다.
#
# 사용법(리포지토리 루트에서): .\Deploy-Local2.ps1
# 사전조건: Setup-LocalIIS2.ps1(관리자 권한)로 WYNLAB_SVC2-Api 사이트가 이미 만들어져 있어야 한다.

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$svcRoot = "D:\WYNLAB_SVC2"

# NEXTONE은 오늘(2026-09-06) WYNLAB DB를 백업/복원해서 만든 사본이다 - 스키마/데이터는 복원
# 시점 기준으로 WYNLAB과 동일하고, 이후로는 서로 독립적으로 바뀐다(서비스 간 전환 테스트 목적).
$localConnStr = "Server=tcp:localhost,15434;Database=NEXTONE;User Id=wynlab;Password=@nextone.com12!@;TrustServerCertificate=True;"
$localJwtKey = "8QkiVHgACqcQWl1KZpiumQ5Xnc99e2lVr20uokT1yqbJKYrZqCY6gt5xQdULz"

Write-Host "=== WYNLAB.Api 게시 -> $svcRoot\Api (NEXTONE DB) ===" -ForegroundColor Cyan
Push-Location "$root\02.Server\WYNLAB.Api"
try {
    dotnet publish -c Release -o "$svcRoot\Api"
    if ($LASTEXITCODE -ne 0) { throw "Api 게시 실패" }
}
finally {
    Pop-Location
}

Write-Host "  web.config를 로컬 NEXTONE DB(Development)용 값으로 덮어쓰는 중..."
$webConfigPath = "$svcRoot\Api\web.config"
[xml]$webConfig = Get-Content $webConfigPath
foreach ($ev in $webConfig.configuration.location.'system.webServer'.aspNetCore.environmentVariables.environmentVariable) {
    switch ($ev.name) {
        "ASPNETCORE_ENVIRONMENT"      { $ev.value = "Development" }
        "ConnectionStrings__WynlabDb" { $ev.value = $localConnStr }
        "Jwt__SecretKey"              { $ev.value = $localJwtKey }
    }
}
$webConfig.Save($webConfigPath)

Write-Host "=== 완료 ===" -ForegroundColor Green
Write-Host "  http://localhost:8092/  (Api - NEXTONE DB)"
Write-Host "  Modules/CoreAssembly/Assets는 http://localhost:8091/ 것을 그대로 재사용합니다."
