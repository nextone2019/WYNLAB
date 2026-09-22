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
#   .\Deploy-Local.ps1 -Rollback      # 배포 없이, 가장 최근 백업으로 그대로 복원만 한다
#
# 사전조건: Setup-LocalIIS.ps1(관리자 권한)로 IIS 사이트가 이미 만들어져 있어야 한다.
#
# 2026-09-15 - "배포가 전부 수동이라 위험하다"는 지적으로 안전장치 3개를 추가했다:
#   1) 배포 전 지금 떠 있는 것(Api/CoreAssembly/Modules[/ClickOnce])을 통째로 백업(최근 5개
#      보관) - 뭔가 잘못되면 -Rollback 한 줄로 바로 되돌릴 수 있다.
#   2) 배포 후 http://localhost:8090/health를 두드려서 실제로 뜨는지 확인 - 응답이 없으면
#      성공이라고 말하지 않고 바로 알려준다(이번 세션에서 겪은 ClickOnce/리소스 임베드 사고들처럼
#      "배포는 끝났다는데 실제로는 안 되는" 상태를 조용히 넘기지 않기 위함).
#   3) 모든 배포 시도(성공/실패)를 _deploy-history.log에 한 줄로 남긴다 - "언제 뭘 올렸었는지"를
#      나중에 되짚어볼 수 있게.

param(
    [switch]$IncludeShell,
    [switch]$Rollback
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$svcRoot = "D:\WYNLAB_SVC"
$backupRoot = "$svcRoot\_backups"
$historyLog = "$svcRoot\_deploy-history.log"
$maxBackups = 5

function Write-DeployHistory([string]$status, [string]$detail) {
    try {
        $commit = (git -C $root rev-parse --short HEAD 2>$null)
        if (-not $commit) { $commit = "(unknown)" }
        $line = "{0}`t{1}`t{2}`t{3}`t{4}" -f (Get-Date -Format "yyyy-MM-dd HH:mm:ss"), $env:USERNAME, $commit, $status, $detail
        Add-Content -Path $historyLog -Value $line -Encoding UTF8
    }
    catch {
        # 이력 기록 실패가 배포 자체를 막으면 안 된다 - 조용히 넘어간다.
    }
}

# ==================== -Rollback: 배포 없이 최근 백업만 복원 ====================
if ($Rollback) {
    if (-not (Test-Path $backupRoot)) { throw "백업이 없습니다 - 아직 한 번도 안전배포로 올린 적이 없는 것 같습니다." }

    $latestBackup = Get-ChildItem $backupRoot -Directory | Sort-Object Name -Descending | Select-Object -First 1
    if (-not $latestBackup) { throw "백업이 없습니다." }

    Write-Host "=== 롤백: $($latestBackup.Name) 로 복원 ===" -ForegroundColor Yellow
    foreach ($part in @("Api", "CoreAssembly", "Modules", "ClickOnce")) {
        $src = Join-Path $latestBackup.FullName $part
        if (Test-Path $src) {
            Write-Host "  복원 중: $part"
            robocopy $src "$svcRoot\$part" /MIR /NFL /NDL /NJH /NJS | Out-Null
            if ($LASTEXITCODE -ge 8) { throw "$part 복원 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
            $global:LASTEXITCODE = 0
        }
    }

    Write-DeployHistory "ROLLBACK" "restored from $($latestBackup.Name)"
    Write-Host "=== 롤백 완료 - IIS 사이트를 재시작해야 반영됩니다(앱풀 재시작 또는 PC 재부팅) ===" -ForegroundColor Green
    exit 0
}

# ==================== 배포 전 백업 ====================
# CoreAssembly/Modules는 파일이 많고 커서(DevExpress dll들) 백업 자체가 몇 초~몇십 초 걸릴 수
# 있지만, 이번 세션에서 겪은 사고들(며칠째 조용히 안 반영되던 ClickOnce, app.manifest 한 줄
# 때문에 매번 설치가 깨지던 문제)을 생각하면 이 정도 시간은 안전과 바꿀 만하다.
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$backupDir = "$backupRoot\$timestamp"
Write-Host "=== 0) 배포 전 백업 -> $backupDir ===" -ForegroundColor DarkCyan
foreach ($part in @("Api", "CoreAssembly", "Modules") + $(if ($IncludeShell) { @("ClickOnce") } else { @() })) {
    $src = "$svcRoot\$part"
    if (Test-Path $src) {
        robocopy $src "$backupDir\$part" /MIR /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "$part 백업 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
        $global:LASTEXITCODE = 0
    }
}
# 오래된 백업은 지운다(최근 5개만 보관) - 매번 늘어나기만 하면 결국 디스크를 다 먹는다.
$oldBackups = Get-ChildItem $backupRoot -Directory -ErrorAction SilentlyContinue |
    Sort-Object Name -Descending | Select-Object -Skip $maxBackups
foreach ($old in $oldBackups) { Remove-Item $old.FullName -Recurse -Force }

try {

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

# IIS가 Api를 이미 띄워둔 상태면 w3wp.exe가 WYNLAB.Api.dll을 물고 있어서 publish의 파일복사가
# 잠금에 막혀 실패한다(2026-09-15 여러 번 실제로 겪음 - MSB3027). 관리자 권한으로 실행 중이면
# 여기서 앱풀을 잠깐 내렸다 올려서 이 문제를 원천 차단한다 - 관리자 권한이 아니면(Import-Module
# 자체가 실패) 조용히 건너뛰고 예전처럼 동작한다(그럴 땐 배포 전 앱풀을 직접 내려둘 것).
$appPoolName = "WYNLAB_SVC-Api"
$stoppedAppPool = $false
try {
    Import-Module WebAdministration -ErrorAction Stop
    if ((Get-WebAppPoolState -Name $appPoolName).Value -eq "Started") {
        Write-Host "  앱풀($appPoolName) 잠깐 정지 중..."
        Stop-WebAppPool -Name $appPoolName
        Start-Sleep -Seconds 2
        $stoppedAppPool = $true
    }
}
catch {
    Write-Host "  (관리자 권한이 아니라 앱풀 자동 정지를 건너뜁니다 - 잠겨 있으면 직접 내려주세요)" -ForegroundColor DarkYellow
}

Push-Location "$root\02.Server\WYNLAB.Api"
try {
    dotnet publish -c Release -o "$svcRoot\Api"
    if ($LASTEXITCODE -ne 0) { throw "Api 게시 실패" }
}
finally {
    Pop-Location
    if ($stoppedAppPool) {
        Write-Host "  앱풀($appPoolName) 다시 시작 중..."
        Start-WebAppPool -Name $appPoolName
    }
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

$shellVersion = $null
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
        $shellVersion = "1.0.$build.$revision"

        & $msbuild WYNLAB.Shell.csproj /t:Publish /p:PublishProfile=DevLocal /p:Configuration=Debug `
            /p:ApplicationVersion=$shellVersion /p:UpdateRequired=true /p:MinimumRequiredVersion=$shellVersion
        if ($LASTEXITCODE -ne 0) { throw "ClickOnce 게시 실패" }

        robocopy "bin\Debug\net48\publish" "$svcRoot\ClickOnce" /MIR /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "ClickOnce 파일 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
        $global:LASTEXITCODE = 0

        # WYNLAB.application로 직접 안 들어가도 되게 설치 링크 안내 페이지를 같이 넣는다 -
        # MSBuild ClickOnce 게시 대상이 아니라서 위 robocopy /MIR가 안 건드림(Deploy-Package.ps1의
        # 같은 이유의 같은 조치 참고) - 이게 없으면 /MIR가 이전에 손으로 넣어둔 index.html까지
        # 지워버려서 http://localhost:8091/ 루트 접속이 403.14로 막힌다(2026-09-13 실제로 겪음).
        Copy-Item "index.html" "$svcRoot\ClickOnce\" -Force

        Write-Host "  게시 버전: $shellVersion"
    }
    finally {
        Pop-Location
    }
}

# ==================== 배포 후 헬스체크 ====================
# "배포는 성공했다는데 실제로는 안 뜨는" 상태를 조용히 넘기지 않으려고 실제로 두드려본다.
# IIS가 새 dll을 물고 다시 뜨는 데 약간 시간이 걸릴 수 있어 몇 번 재시도한다.
Write-Host "=== 4) 헬스체크 ===" -ForegroundColor Cyan
$healthy = $false
for ($i = 0; $i -lt 5; $i++) {
    try {
        $resp = Invoke-WebRequest -Uri "http://localhost:8090/health" -UseBasicParsing -TimeoutSec 5
        if ($resp.StatusCode -eq 200) { $healthy = $true; break }
    }
    catch { }
    Start-Sleep -Seconds 2
}

if (-not $healthy) {
    Write-DeployHistory "FAIL" "health check 실패 (IncludeShell=$IncludeShell)"
    Write-Host "=== 배포는 끝났지만 http://localhost:8090/health 응답이 없습니다! ===" -ForegroundColor Red
    Write-Host "  IIS 앱풀 상태를 확인하거나, 문제가 있으면 '.\Deploy-Local.ps1 -Rollback'으로 되돌리세요." -ForegroundColor Red
    exit 1
}

Write-DeployHistory "OK" "IncludeShell=$IncludeShell shellVersion=$shellVersion"
Write-Host "=== 완료 (헬스체크 정상) ===" -ForegroundColor Green
Write-Host "  http://localhost:8090/  (Api)"
Write-Host "  http://localhost:8091/  (ClickOnce/Modules/CoreAssembly/Assets)"

}
catch {
    Write-DeployHistory "FAIL" "$($_.Exception.Message)"
    Write-Host "=== 배포 실패: $($_.Exception.Message) ===" -ForegroundColor Red
    Write-Host "  방금 만든 백업($backupDir)에서 '.\Deploy-Local.ps1 -Rollback'으로 되돌릴 수 있습니다." -ForegroundColor Yellow
    throw
}
