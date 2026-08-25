# 로컬 개발 PC에서 실행 - 서버에 배포해야 할 파일들을 전부 Release로 새로 빌드해서
# _deploy\ 폴더 하나에 서버 구조 그대로 모아준다. 여기저기 bin\Release\... 경로를 손으로
# 뒤져서 파일을 하나씩 옮기다 보면 실수로 오래된 파일(다른 프로젝트가 복사해둔 예전 사본)을
# 집어가기 쉬운데(실제로 겪음 - Controls/Shared 크기가 안 맞아서 몇 번 헤맴), 그 문제를
# 원천적으로 없애기 위한 스크립트다. 이 스크립트가 나온 뒤로는 개별 bin 폴더를 직접 뒤지지
# 말고 항상 _deploy\ 결과물만 서버로 옮기면 된다.
#
# 사용법(로컬 PC, 리포지토리 루트에서):
#   .\Deploy-Package.ps1                 # CoreAssembly + 화면(SM) 모듈만 (평소 배포, 빠름)
#   .\Deploy-Package.ps1 -IncludeShell    # 위에 더해 WYNLAB.exe ClickOnce까지 새로 게시(느림,
#                                          # Shell 자체 코드나 화면 프로젝트 개수가 바뀌었을 때만)
#
# 끝나면 _deploy\ 폴더를 통째로 서버 D:\WYNLAB\ 밑에 같은 이름의 하위폴더로 덮어쓰면 된다
# (_deploy\CoreAssembly\* -> D:\WYNLAB\CoreAssembly\, _deploy\Modules\* -> D:\WYNLAB\Modules\,
#  _deploy\ClickOnce\*(있으면) -> D:\WYNLAB\ClickOnce\). 그 다음 서버에서 CoreAssembly
# 폴더로 가서 Generate-CoreAssemblyManifest.ps1을 실행해야 클라이언트가 변경을 인식한다
# (이 스크립트가 자동으로 안 해주는 이유: manifest는 서버에 실제로 파일이 도착한 뒤에
# 생성해야 의미가 있어서, 로컬 스크립트가 미리 만들어봐야 소용없다).

param(
    [switch]$IncludeShell
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$deploy = Join-Path $root "_deploy"

Write-Host "=== 1) 이전 스테이징 결과 정리 ===" -ForegroundColor Cyan
Remove-Item -Recurse -Force $deploy -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path "$deploy\CoreAssembly" | Out-Null
New-Item -ItemType Directory -Force -Path "$deploy\Modules\SM" | Out-Null

function Build-Release([string]$ProjectOrSolution) {
    Write-Host "  빌드: $ProjectOrSolution"
    dotnet build $ProjectOrSolution -c Release | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "빌드 실패: $ProjectOrSolution" }
}

Write-Host "=== 2) CoreAssembly (BaseForm/Shared/Controls) ===" -ForegroundColor Cyan
Build-Release "$root\01.Client\WYNLAB.BaseForm\WYNLAB.BaseForm.csproj"
Build-Release "$root\01.Client\WYNLAB.Controls\WYNLAB.Controls.csproj"
Build-Release "$root\03.Shared\WYNLAB.Shared\WYNLAB.Shared.csproj"

Copy-Item "$root\01.Client\WYNLAB.BaseForm\bin\Release\net48\WYNLAB.BaseForm.dll" "$deploy\CoreAssembly\" -Force
Copy-Item "$root\01.Client\WYNLAB.BaseForm\bin\Release\net48\WYNLAB.BaseForm.pdb" "$deploy\CoreAssembly\" -Force -ErrorAction SilentlyContinue
Copy-Item "$root\01.Client\WYNLAB.Controls\bin\Release\net48\WYNLAB.Controls.dll" "$deploy\CoreAssembly\" -Force
Copy-Item "$root\03.Shared\WYNLAB.Shared\bin\Release\netstandard2.0\WYNLAB.Shared.dll" "$deploy\CoreAssembly\" -Force

# manifest.json 생성 스크립트도 같이 넣어둔다 - 서버에서 매번 따로 안 챙겨도 되게.
# 실행은 여전히 서버에서 사람이 해야 한다(그 시점 CoreAssembly 폴더의 실제 파일 기준으로
# 해시를 계산해야 의미가 있어서, 로컬에서 미리 실행해봐야 소용없다 - 위 설명 참고).
Copy-Item "$root\01.Client\WYNLAB.Bootstrap\Generate-CoreAssemblyManifest.ps1" "$deploy\CoreAssembly\" -Force

Write-Host "=== 3) 화면(SM) 모듈 ===" -ForegroundColor Cyan
$smSolutions = Get-ChildItem "$root\99.SOURCE\SM" -Directory | ForEach-Object {
    Get-ChildItem $_.FullName -Filter "*.sln" | Select-Object -First 1
} | Where-Object { $_ -ne $null }

foreach ($sln in $smSolutions) {
    Build-Release $sln.FullName
    $moduleName = $sln.BaseName # 예: WYNLAB.SM.CODE
    $binDir = Get-ChildItem "$($sln.DirectoryName)\$moduleName\bin\Release\net48" -Filter "$moduleName.dll" -ErrorAction SilentlyContinue
    if (-not $binDir) {
        Write-Warning "$moduleName.dll 을 못 찾았습니다 - 건너뜀"
        continue
    }
    $srcDir = $binDir.DirectoryName
    Copy-Item "$srcDir\$moduleName.dll" "$deploy\Modules\SM\" -Force
    Copy-Item "$srcDir\$moduleName.pdb" "$deploy\Modules\SM\" -Force -ErrorAction SilentlyContinue

    # 이 화면이 SvgIcon 등 preserialized 리소스를 쓰면 같은 폴더에 System.Resources.Extensions.dll이
    # 같이 생기는데, 이건 화면별 폴더가 아니라 Modules\ 바로 밑에 한 벌만 두면 된다(SERVER_SETUP.md 참고).
    $resExt = Join-Path $srcDir "System.Resources.Extensions.dll"
    if (Test-Path $resExt) {
        Copy-Item $resExt "$deploy\Modules\" -Force
    }
}

if ($IncludeShell) {
    Write-Host "=== 4) WYNLAB.exe ClickOnce 게시 (시간이 좀 걸립니다) ===" -ForegroundColor Cyan
    $msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"
    if (-not (Test-Path $msbuild)) { throw "MSBuild.exe를 찾을 수 없습니다: $msbuild" }

    $shellDir = "$root\01.Client\WYNLAB.Shell"
    Push-Location $shellDir
    try {
        # 이전 게시 결과물을 반드시 먼저 지운다 - 안 지우면 증분빌드가 매니페스트 재생성을 건너뛴다
        Remove-Item -Recurse -Force "bin\Release\net48\publish" -ErrorAction SilentlyContinue
        Remove-Item -Recurse -Force "bin\Release\net48\app.publish" -ErrorAction SilentlyContinue
        Remove-Item -Force "obj\Release\net48\WYNLAB.exe.manifest" -ErrorAction SilentlyContinue
        Remove-Item -Force "obj\Release\net48\WYNLAB.application" -ErrorAction SilentlyContinue

        # 매번 새 버전 번호를 명시적으로 준다 - 안 주면 항상 1.0.0.0으로 게시되어 이미 설치된
        # 클라이언트가 "업데이트 없음"으로 판단하고 예전 버전을 계속 실행한다
        $build = [int]((Get-Date) - (Get-Date "2024-01-01")).TotalDays
        $revision = [int](Get-Date).TimeOfDay.TotalMinutes
        & $msbuild WYNLAB.Shell.csproj /t:Publish /p:PublishProfile=ProdLocal /p:Configuration=Release /p:ApplicationVersion="1.0.$build.$revision"
        if ($LASTEXITCODE -ne 0) { throw "ClickOnce 게시 실패" }

        # Copy-Item -Recurse는 소스가 "폴더\*" 와일드카드일 때 "Application Files"처럼
        # 이름에 공백이 든 하위 폴더 하나를 통째로 한 단계 접어버리는 경우가 있었다(실제로
        # 겪음 - 그 폴더 자체가 아니라 그 안의 WYNLAB_1_0_966_680\이 곧바로 ClickOnce\
        # 밑에 복사되어, "Application Files\" 계층 자체가 사라졌다). ClickOnce 매니페스트는
        # 이 폴더명을 그대로("Application Files/버전/...") URL에 박아두므로, 계층이 하나라도
        # 어긋나면 클라이언트가 설치 시점에 404로 죽는다. robocopy는 이런 폴더명 문제 없이
        # 트리 구조를 있는 그대로 복사한다.
        robocopy "bin\Release\net48\publish" "$deploy\ClickOnce" /E /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "ClickOnce 파일 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }
        Write-Host "  게시 버전: 1.0.$build.$revision"
    }
    finally {
        Pop-Location
    }
}

Write-Host ""
Write-Host "=== 완료 - _deploy 폴더 내용 ===" -ForegroundColor Green
Get-ChildItem $deploy -Recurse -File | ForEach-Object {
    $rel = $_.FullName.Substring($deploy.Length + 1)
    Write-Host ("  {0,-55} {1,10:N0} bytes" -f $rel, $_.Length)
}

Write-Host ""
Write-Host "다음 단계 (서버에서):" -ForegroundColor Yellow
$step = 1
Write-Host "  $step. _deploy\CoreAssembly\*  ->  D:\WYNLAB\CoreAssembly\"; $step++
Write-Host "  $step. _deploy\Modules\*       ->  D:\WYNLAB\Modules\"; $step++
if ($IncludeShell) {
    Write-Host "  $step. _deploy\ClickOnce\*     ->  D:\WYNLAB\ClickOnce\"; $step++
}
Write-Host "  $step. Set-Location D:\WYNLAB\CoreAssembly; .\Generate-CoreAssemblyManifest.ps1"; $step++
Write-Host "  $step. 실행 중인 WYNLAB.exe를 전부 종료 후 재실행"
