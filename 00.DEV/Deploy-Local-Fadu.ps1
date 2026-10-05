# 로컬 개발 PC의 "FADU 서비스"(FADU DB) 배포 스크립트 - Deploy-Local.ps1의 API만 다시
# 게시하는 버전이다. 화면 DLL(Modules)/프레임워크(CoreAssembly)/ClickOnce는 DB와 무관하게 첫
# 번째 서비스(D:\WYNLAB_SVC)의 것과 완전히 동일한 바이너리라 다시 만들지 않고 그대로 재사용한다
# (Setup-LocalIIS-Fadu.ps1 주석 참고) - 이 스크립트는 D:\WYNLAB_SVC_FADU\Api만 건드린다.
#
# 사용법(리포지토리 루트에서): .\Deploy-Local-Fadu.ps1
# 사전조건: Setup-LocalIIS-Fadu.ps1(관리자 권한)로 WYNLAB_SVC_FADU-Api 사이트가 이미 만들어져 있어야 한다.

param([switch]$Static)   # -Static: WYNLAB_DEV 쪽 Modules/CoreAssembly/Assets를 FADU로 복사(API 게시는 생략)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$svcRoot = "D:\WYNLAB_SVC_FADU"

# FADU 사이트 DB(FADU)를 바라보는 별도 API 서비스. WYNLAB_DEV와 JWT 키를 따로 써서 서로의 로그인 토큰이 통하지 않는다.
$localConnStr = "Server=tcp:localhost,15434;Database=FADU;User Id=wynlab;Password=@nextone.com12!@;TrustServerCertificate=True;"
$localJwtKey = "bw9feVQsYMMxsl3TfrP6XzEjZuIbxUJvxVzN2xWUycgK3344IKjdOsGnxZzgmyG7"

if ($Static) {
    foreach ($n in "Modules", "CoreAssembly", "Assets") { robocopy "D:\WYNLAB_SVC\$n" "$svcRoot\$n" /E /XD _backups /NFL /NDL /NJH /NJS /NP | Out-Null }
    Write-Host "정적 리소스(Modules/CoreAssembly/Assets) 복사 완료 -> $svcRoot" -ForegroundColor Green
    return
}

Write-Host "=== WYNLAB.Api 게시 -> $svcRoot\Api (FADU DB) ===" -ForegroundColor Cyan
Push-Location "$root\02.Server\WYNLAB.Api"
try {
    dotnet publish -c Release -o "$svcRoot\Api"
    if ($LASTEXITCODE -ne 0) { throw "Api 게시 실패" }
}
finally {
    Pop-Location
}

Write-Host "  web.config를 로컬 FADU DB(Development)용 값으로 덮어쓰는 중..."
$webConfigPath = "$svcRoot\Api\web.config"
[xml]$webConfig = Get-Content $webConfigPath
foreach ($ev in $webConfig.configuration.location.'system.webServer'.aspNetCore.environmentVariables.environmentVariable) {
    switch ($ev.name) {
        "ASPNETCORE_ENVIRONMENT"      { $ev.value = "Development" }
        "ConnectionStrings__WynlabDb" { $ev.value = $localConnStr }
        "Jwt__SecretKey"              { $ev.value = $localJwtKey }
        "Internal__ServiceKey"        { $ev.value = "cG1h+ZCdb0VpRrDzts1ZISCEtEmk9pl1De+vnOVHQEE=" }
    }
}
$webConfig.Save($webConfigPath)

Write-Host "=== 완료 ===" -ForegroundColor Green
Write-Host "  http://localhost:8093/  (Api - FADU DB)"
Write-Host "  정적 리소스는 http://localhost:8094/ (D:\WYNLAB_SVC_FADU\Modules 등, -Static으로 갱신)"
