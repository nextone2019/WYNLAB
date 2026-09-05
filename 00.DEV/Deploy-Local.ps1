# 로컬 개발 PC 전용 배포 스크립트 - D:\WYNLAB_SVC(이 PC의 로컬 IIS 루트)로 Api/CoreAssembly/
# 화면(SM) 모듈을 새로 올린다. Deploy-Package.ps1(서버 배포용, _deploy\ 로 스테이징)과 로직을
# 중복하지 않도록, CoreAssembly/Modules는 Deploy-Package.ps1을 그대로 호출해서 만든 _deploy\
# 결과물을 복사만 한다.
#
# 평소 화면 개발(F5 디버그)에는 이 스크립트 자체가 필요 없다 - Api는 IIS가 항상 띄워두고,
# 화면 DLL은 Visual Studio가 bin\Debug에 바로 만들어주기 때문. 이 스크립트는 "IIS가 물고 있는
# Api를 최신 코드로 갈아끼우고 싶을 때", "화면 DLL을 Modules\로 옮겨서 배포 경로 자체를
# 검증하고 싶을 때" 쓴다.
#
# 사용법(리포지토리 루트에서):
#   .\Deploy-Local.ps1                # Api + CoreAssembly + 화면(SM) 모듈만 (평소, 빠름)
#   .\Deploy-Local.ps1 -IncludeShell  # 위에 더해 ClickOnce(D:\WYNLAB_SVC\ClickOnce)까지 새로 게시
#                                      # (ClickOnce 설치/자동업데이트 흐름 자체를 로컬에서 검증하고
#                                      # 싶을 때만 - 평소 개발에는 필요 없음)
#
# 사전조건: Setup-LocalIIS.ps1(관리자 권한)로 IIS 사이트가 이미 만들어져 있어야 한다.

param(
    [switch]$IncludeShell
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$svcRoot = "D:\WYNLAB_SVC"

# user-secrets(dotnet user-secrets, WYNLAB.Api UserSecretsId=803575cc-...)에 등록된 것과 같은
# 값이다 - IIS로 띄우는 Api는 user-secrets를 못 읽으므로(그건 dotnet run 전용) web.config의
# environmentVariables로 같은 값을 넣어줘야 한다.
$localConnStr = "Server=tcp:localhost,15434;Database=WYNLAB;User Id=wynlab;Password=@nextone.com12!@;TrustServerCertificate=True;"
$localJwtKey = "8QkiVHgACqcQWl1KZpiumQ5Xnc99e2lVr20uokT1yqbJKYrZqCY6gt5xQdULz"

# SMTP는 여기 스크립트(git 추적됨)에 직접 못 박아넣는다 - 개인 Gmail 계정 비밀번호라 DB
# 비밀번호/JWT키보다 훨씬 민감하다. 대신 $svcRoot(D:\WYNLAB_SVC, 저장소 바깥이라 git과 무관)에
# 있는 로컬 전용 파일에서 읽는다 - 파일이 없으면 그냥 건너뛴다(SMTP 값 없이도 나머지 배포는
# 정상 진행되어야 함, 메일 발송 기능만 안 될 뿐).
$smtpLocalFile = "$svcRoot\smtp-local.json"
$smtp = $null
if (Test-Path $smtpLocalFile) {
    $smtp = Get-Content $smtpLocalFile -Raw | ConvertFrom-Json
}

Write-Host "=== 1) WYNLAB.Api 게시 -> $svcRoot\Api ===" -ForegroundColor Cyan
Push-Location "$root\02.Server\WYNLAB.Api"
try {
    dotnet publish -c Release -o "$svcRoot\Api"
    if ($LASTEXITCODE -ne 0) { throw "Api 게시 실패" }
}
finally {
    Pop-Location
}

# dotnet publish는 프로젝트 안의 web.config(운영 서버용 비밀값이 담긴 gitignore 파일)를 그대로
# 재사용해서 게시 결과물에 복사한다 - 로컬 IIS가 실수로 운영 DB에 붙는 사고를 막기 위해 반드시
# 로컬 값으로 덮어쓴다(SERVER_SETUP.md 6-2와 같은 이유, 대상만 로컬).
Write-Host "  web.config를 로컬 DB(Development)용 값으로 덮어쓰는 중..."
$webConfigPath = "$svcRoot\Api\web.config"
[xml]$webConfig = Get-Content $webConfigPath
foreach ($ev in $webConfig.configuration.location.'system.webServer'.aspNetCore.environmentVariables.environmentVariable) {
    switch ($ev.name) {
        "ASPNETCORE_ENVIRONMENT"      { $ev.value = "Development" }
        "ConnectionStrings__WynlabDb" { $ev.value = $localConnStr }
        "Jwt__SecretKey"              { $ev.value = $localJwtKey }
        "Smtp__Host"           { if ($smtp) { $ev.value = $smtp.Host } }
        "Smtp__Port"           { if ($smtp) { $ev.value = [string]$smtp.Port } }
        "Smtp__UserName"       { if ($smtp) { $ev.value = $smtp.UserName } }
        "Smtp__Password"       { if ($smtp) { $ev.value = $smtp.Password } }
        "Smtp__FromAddress"    { if ($smtp) { $ev.value = $smtp.FromAddress } }
        "Smtp__FromDisplayName" { if ($smtp) { $ev.value = $smtp.FromDisplayName } }
    }
}
$webConfig.Save($webConfigPath)

Write-Host "=== 2) CoreAssembly + 화면(SM) 모듈 (Deploy-Package.ps1 재사용) ===" -ForegroundColor Cyan
& "$root\Deploy-Package.ps1"
if ($LASTEXITCODE -ne 0) { throw "Deploy-Package.ps1 실패" }

$deploy = "$root\_deploy"
robocopy "$deploy\CoreAssembly" "$svcRoot\CoreAssembly" /MIR /NFL /NDL /NJH /NJS | Out-Null
if ($LASTEXITCODE -ge 8) { throw "CoreAssembly 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
robocopy "$deploy\Modules" "$svcRoot\Modules" /MIR /NFL /NDL /NJH /NJS | Out-Null
if ($LASTEXITCODE -ge 8) { throw "Modules 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
$global:LASTEXITCODE = 0
Write-Host "  $svcRoot\CoreAssembly, $svcRoot\Modules 갱신 완료"

if ($IncludeShell) {
    Write-Host "=== 3) WYNLAB.exe ClickOnce 게시 -> $svcRoot\ClickOnce ===" -ForegroundColor Cyan
    $msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"
    if (-not (Test-Path $msbuild)) { throw "MSBuild.exe를 찾을 수 없습니다: $msbuild" }

    $shellDir = "$root\01.Client\WYNLAB.Shell"
    Push-Location $shellDir
    try {
        # Debug로 게시한다(DevLocal.pubxml 주석 참고) - Release는 appsettings.Prod.json을
        # 물어서 이 PC의 D:\WYNLAB_SVC가 아니라 회사 서버(192.168.160.10)를 바라보는 셸이
        # 나온다. 지금은 로컬 Api/CoreAssembly/Modules만 떠 있으므로 반드시 Debug여야
        # appsettings.Dev.json(진짜 localhost:8090/8091)이 게시물에 들어간다.
        Remove-Item -Recurse -Force "bin\Debug\net48\publish" -ErrorAction SilentlyContinue
        Remove-Item -Recurse -Force "bin\Debug\net48\app.publish" -ErrorAction SilentlyContinue
        Remove-Item -Force "obj\Debug\net48\WYNLAB.exe.manifest" -ErrorAction SilentlyContinue
        Remove-Item -Force "obj\Debug\net48\WYNLAB.application" -ErrorAction SilentlyContinue

        # Deploy-Package.ps1의 같은 계산과 동일한 이유(그 스크립트 주석 참고) - build/revision을
        # 같은 시계(기준일부터 총 분)에서 나눠 만들어 버전 역행이 구조적으로 불가능하게 한다.
        $totalMinutes = [int64]((Get-Date) - (Get-Date "2024-01-01")).TotalMinutes
        $build = [int]($totalMinutes / 1440)
        $revision = [int]($totalMinutes % 65536)
        $version = "1.0.$build.$revision"

        & $msbuild WYNLAB.Shell.csproj /t:Publish /p:PublishProfile=DevLocal /p:Configuration=Debug `
            /p:ApplicationVersion=$version /p:UpdateRequired=true /p:MinimumRequiredVersion=$version
        if ($LASTEXITCODE -ne 0) { throw "ClickOnce 게시 실패" }

        robocopy "bin\Debug\net48\publish" "$svcRoot\ClickOnce" /MIR /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "ClickOnce 파일 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
        $global:LASTEXITCODE = 0
        Write-Host "  게시 버전: $version"
    }
    finally {
        Pop-Location
    }
}

Write-Host "=== 완료 ===" -ForegroundColor Green
Write-Host "  http://localhost:8090/  (Api)"
Write-Host "  http://localhost:8091/  (ClickOnce/Modules/CoreAssembly/Assets)"
