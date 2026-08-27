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
#  _deploy\ClickOnce\*(있으면) -> D:\WYNLAB\ClickOnce\). manifest.json은 이 스크립트가 여기서
# 미리 만들어서 파일들과 같이 넣어주므로, 서버에서 따로 실행할 스크립트는 없다.
#
# [예전 방식과 달라진 점] 전에는 파일을 서버에 복사한 다음 서버에서 매니페스트 생성
# 스크립트를 사람이 실행했다. 그러면 "파일 복사"와 "매니페스트 생성"이 별개 단계라 그 사이가
# 어긋날 수 있었고, 실제로 클라이언트 로그에 '해시 불일치'로 갱신이 실패하는 일이 반복됐다
# (2026-08-24~25). 이제는 파일과 매니페스트를 한 벌로 묶어서 옮기므로 그 어긋남 자체가 없다.

param(
    [switch]$IncludeShell
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$deploy = Join-Path $root "_deploy"

# 2026-08-27 폴더 구조 변경: 리포 루트(이 스크립트가 있는 곳)가 00.DEV\로 옮겨갔지만,
# 99.SOURCE(화면 프로젝트)는 그 이동에서 빠지고 옛 위치(00.DEV의 부모 폴더)에 그대로 남았다.
# 그래서 99.SOURCE만 $root가 아니라 그 부모 기준으로 찾는다 - 나중에 99.SOURCE도 00.DEV
# 밑으로 옮기게 되면 이 한 줄만 "$root"로 되돌리면 된다.
$legacySourceRoot = Split-Path $root -Parent

Write-Host "=== 1) 이전 스테이징 결과 정리 ===" -ForegroundColor Cyan

# 이번에 다시 만들 폴더만 지운다. 예전엔 _deploy를 통째로 지웠는데, 그러면 -IncludeShell로
# ClickOnce까지 만들어둔 뒤에 평소 배포(-IncludeShell 없이)를 한 번 더 돌리는 순간 애써 만든
# ClickOnce 게시물이 아무 말 없이 사라졌다(실제로 겪음 - 서버에 올리려고 보니 폴더가 비어있음).
Remove-Item -Recurse -Force "$deploy\CoreAssembly" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "$deploy\Modules" -ErrorAction SilentlyContinue
# ClickOnce 폴더는 여기서 지우지 않는다 - 게시 단계에서 robocopy /MIR로 원본과 똑같이
# 맞추므로 잔해가 알아서 정리된다(그 주석 참고).

New-Item -ItemType Directory -Force -Path "$deploy\CoreAssembly" | Out-Null
New-Item -ItemType Directory -Force -Path "$deploy\Modules" | Out-Null

if (-not $IncludeShell -and (Test-Path "$deploy\ClickOnce")) {
    Write-Host "  (이전에 만든 _deploy\ClickOnce는 그대로 둡니다 - 다시 만들려면 -IncludeShell)" -ForegroundColor DarkGray
}

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

# 서버 Assets 폴더(관리자가 아이콘 이미지를 직접 올리는 곳)는 이 스크립트가 다루지 않으므로,
# 그쪽 manifest.json을 만들 수 있도록 생성 스크립트를 같이 넣어둔다.
Copy-Item "$root\Generate-Manifest.ps1" "$deploy\" -Force

Write-Host "=== 3) 화면 모듈 (99.SOURCE\{모듈코드}\, 예: SM/BA) ===" -ForegroundColor Cyan
# 99.SOURCE 바로 밑 폴더 하나하나가 모듈(SM/BA/...) - 배포 파일은 Modules\{모듈코드}\에 모듈별로
# 나눠 담는다(ModuleLoader의 재귀 탐색이 이 하위폴더 구조를 그대로 지원 - 99.SOURCE 소스 구조와
# 대응된다). 모듈 폴더 밑의 TEMPLATE\WYNLAB.{모듈}.TEMPLATE(새 화면 복사용 원본, 예:
# SM\TEMPLATE\WYNLAB.SM.TEMPLATE)는 sln이 없어서 아래 스캔에 애초에 안 걸린다 - 따로 건너뛸
# 필요가 없다.
$moduleDirs = Get-ChildItem "$legacySourceRoot\99.SOURCE" -Directory

foreach ($moduleDir in $moduleDirs) {
    $moduleCd = $moduleDir.Name # 예: SM, BA

    # 두 가지 sln 배치가 당분간 공존한다 - 아직 화면 단위인 예전 화면들(99.SOURCE\{모듈}\{화면}\{화면}.sln)과
    # 모듈 단위로 전환한 것(99.SOURCE\{모듈}\WYNLAB.{모듈}.sln, 서브모듈은 그 안의 폴더로만 구분). 그래서
    # 99.SOURCE\{모듈} 바로 밑과 그 한 단계 아래 폴더 양쪽에서 sln을 찾는다 - 화면이 전부 모듈 단위로
    # 옮겨지면 아래쪽(하위 폴더 스캔)은 자연히 안 찾아지고 위쪽만 남는다.
    $solutions = @(Get-ChildItem $moduleDir.FullName -Filter "*.sln") +
        (Get-ChildItem $moduleDir.FullName -Directory | ForEach-Object {
            Get-ChildItem $_.FullName -Filter "*.sln" -ErrorAction SilentlyContinue | Select-Object -First 1
        }) | Where-Object { $_ -ne $null }

    if ($solutions.Count -eq 0) { continue }

    New-Item -ItemType Directory -Force -Path "$deploy\Modules\$moduleCd" | Out-Null

    foreach ($sln in $solutions) {
        Build-Release $sln.FullName
        $moduleName = $sln.BaseName # 예: WYNLAB.SM.CODE, WYNLAB.BA
        $binDir = Get-ChildItem "$($sln.DirectoryName)\$moduleName\bin\Release\net48" -Filter "$moduleName.dll" -ErrorAction SilentlyContinue
        if (-not $binDir) {
            Write-Warning "$moduleName.dll 을 못 찾았습니다 - 건너뜀"
            continue
        }
        $srcDir = $binDir.DirectoryName
        Copy-Item "$srcDir\$moduleName.dll" "$deploy\Modules\$moduleCd\" -Force
        Copy-Item "$srcDir\$moduleName.pdb" "$deploy\Modules\$moduleCd\" -Force -ErrorAction SilentlyContinue

        # 이 화면이 SvgIcon 등 preserialized 리소스를 쓰면 같은 폴더에 System.Resources.Extensions.dll이
        # 같이 생기는데, 이건 화면별 폴더가 아니라 Modules\ 바로 밑에 한 벌만 두면 된다(SERVER_SETUP.md 참고).
        $resExt = Join-Path $srcDir "System.Resources.Extensions.dll"
        if (Test-Path $resExt) {
            Copy-Item $resExt "$deploy\Modules\" -Force
        }
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
        # 클라이언트가 "업데이트 없음"으로 판단하고 예전 버전을 계속 실행한다.
        #
        # revision을 "자정부터 몇 분 지났는지"(TimeOfDay.TotalMinutes)로 구했었는데, 이건 매일
        # 자정마다 0으로 리셋된다 - build(기준일 2024-01-01부터의 날짜 수)의 하루 경계가 정확히
        # 로컬 자정과 일치한다는 보장이 없어서(실제로 겪음 - 2026-08-27 00시대에 build는 그대로
        # 969인데 revision만 931 -> 6으로 떨어져서 versions이 역행했다), build는 그대로인데
        # revision만 리셋되면 전체 버전이 거꾸로 간다. ClickOnce는 MinimumRequiredVersion보다
        # 낮은 버전은 배포 자체를 거부해서("응용 프로그램의 현재 최소 필요 버전보다 낮은 버전의
        # 배포를 활성화할 수 없습니다"), 한 번 이렇게 역행하면 이미 그 버전을 받은 클라이언트는
        # 그 뒤로 계속 막힌다.
        #
        # 고쳐서 build/revision을 "기준일부터 지난 총 분"이라는 같은 값에서 나눠서 만든다
        # (build=몫, revision=나머지 % 65536(UInt16 최대값)) - 두 값이 서로 다른 기준(자정 vs
        # 임의의 하루 경계)으로 따로 도는 게 아니라 항상 같은 시계에서 나오므로 이런 어긋남 자체가
        # 구조적으로 생길 수 없다. revision이 65536(약 45일)마다 한 번 줄어들 수는 있지만, 그
        # 사이 build가 최소 40번 이상 늘어나 있으므로(하루 최대 1번+) 전체 버전은 여전히
        # 역행하지 않는다.
        $totalMinutes = [int64]((Get-Date) - (Get-Date "2024-01-01")).TotalMinutes
        $build = [int]($totalMinutes / 1440)
        $revision = [int]($totalMinutes % 65536)
        $version = "1.0.$build.$revision"

        # MinimumRequiredVersion을 이번 게시 버전과 똑같이 줘서 "필수 업데이트"로 만든다.
        # 이게 없으면 클라이언트 실행 시 "새 버전을 사용할 수 있습니다. 지금 다운로드
        # 하시겠습니까? [확인] [건너뛰기]" 창이 떠서 사용자가 건너뛸 수 있는데, 그러면
        # 사람마다 다른 버전을 쓰게 되고 "저는 그 오류 안 나는데요" 같은 상황이 생긴다.
        # 업무용 사내 앱은 항상 최신이어야 하므로 선택지를 주지 않는다(묻지 않고 받은 뒤 재시작).
        #
        # 이 두 값을 pubxml에 박지 않고 여기서 넘기는 이유: MinimumRequiredVersion에는
        # ApplicationVersion처럼 와일드카드(1.0.0.*)를 쓸 수 없어서 매 게시마다 실제 값이
        # 필요하고, pubxml에 UpdateRequired만 켜두면 이 값 없이 수동 게시했을 때 게시가
        # 실패한다 - 두 값을 항상 짝으로 넘기는 이 스크립트에서만 켜는 게 안전하다.
        & $msbuild WYNLAB.Shell.csproj /t:Publish /p:PublishProfile=ProdLocal /p:Configuration=Release `
            /p:ApplicationVersion=$version /p:UpdateRequired=true /p:MinimumRequiredVersion=$version
        if ($LASTEXITCODE -ne 0) { throw "ClickOnce 게시 실패" }

        # Copy-Item -Recurse는 소스가 "폴더\*" 와일드카드일 때 "Application Files"처럼
        # 이름에 공백이 든 하위 폴더 하나를 통째로 한 단계 접어버리는 경우가 있었다(실제로
        # 겪음 - 그 폴더 자체가 아니라 그 안의 WYNLAB_1_0_966_680\이 곧바로 ClickOnce\
        # 밑에 복사되어, "Application Files\" 계층 자체가 사라졌다). ClickOnce 매니페스트는
        # 이 폴더명을 그대로("Application Files/버전/...") URL에 박아두므로, 계층이 하나라도
        # 어긋나면 클라이언트가 설치 시점에 404로 죽는다. robocopy는 이런 폴더명 문제 없이
        # 트리 구조를 있는 그대로 복사한다.
        # /MIR = 대상을 원본과 "정확히 똑같이" 맞춘다(원본에 없는 건 대상에서 지움).
        # /E로 덮어쓰기만 하면 예전 게시 버전 폴더(Application Files\WYNLAB_1_0_x_y\)가 계속
        # 쌓인다. 미리 Remove-Item으로 지워봐도, 삭제 도중 파일 하나가 잠겨 있으면(백신 검사 등)
        # -ErrorAction SilentlyContinue가 그 실패를 삼켜서 반쯤 지워진 폴더가 조용히 남는다
        # (실제로 겪음 - 파일 하나만 남은 예전 버전 폴더가 배포물에 섞여 들어갔다). /MIR는
        # 복사와 정리를 한 번에 해서 그런 어중간한 상태 자체가 생기지 않는다.
        robocopy "bin\Release\net48\publish" "$deploy\ClickOnce" /MIR /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw "ClickOnce 파일 복사 실패 (robocopy 종료 코드 $LASTEXITCODE)" }

        # robocopy는 성공해도 0이 아닌 코드를 남긴다(1 = 파일을 복사함, 3 = 복사+스킵 등. 8 미만은
        # 전부 성공). 그런데 PowerShell은 스크립트가 끝날 때 $LASTEXITCODE를 그대로 자기 종료
        # 코드로 물려주기 때문에, 이걸 안 지우면 배포가 멀쩡히 끝났는데도 "종료 코드 1 = 실패"로
        # 보인다(실제로 겪음). 여기까지 왔다는 건 이미 성공이므로 0으로 되돌린다.
        $global:LASTEXITCODE = 0
        Write-Host "  게시 버전: $version (필수 업데이트 - 클라이언트가 건너뛸 수 없음)"
    }
    finally {
        Pop-Location
    }

    # ClickOnce 게시는 위에서 VS의 MSBuild.exe로 도는데, 2)에서 CoreAssembly를 만들 때 쓴
    # dotnet build와는 서로 다른 컴파일러 인스턴스라 같은 소스라도 산출물 바이트가 달라진다
    # (기능은 동일하지만 해시가 다름). 그대로 두면 ClickOnce 패키지 안의 BaseForm.dll과
    # CoreAssembly\BaseForm.dll이 영원히 다른 파일이 되어, 클라이언트가 설치 직후 매번 굳이
    # 다시 받아가고 무엇보다 "왜 해시가 다르지?"로 사람을 헷갈리게 한다(실제로 겪음).
    # 방금 게시가 만든 산출물로 CoreAssembly를 덮어써서 두 벌을 같은 파일로 맞춘다.
    # (manifest.json은 이 다음 단계에서 생성되므로 여기서 바꿔도 항상 최신 해시가 반영된다.)
    Write-Host "  CoreAssembly를 게시 산출물과 동일하게 맞추는 중..."
    Copy-Item "$shellDir\bin\Release\net48\WYNLAB.BaseForm.dll" "$deploy\CoreAssembly\" -Force
    Copy-Item "$shellDir\bin\Release\net48\WYNLAB.Controls.dll" "$deploy\CoreAssembly\" -Force
    Copy-Item "$shellDir\bin\Release\net48\WYNLAB.Shared.dll" "$deploy\CoreAssembly\" -Force
    Copy-Item "$shellDir\bin\Release\net48\WYNLAB.BaseForm.pdb" "$deploy\CoreAssembly\" -Force -ErrorAction SilentlyContinue
}

Write-Host "=== 5) manifest.json 생성 ===" -ForegroundColor Cyan
& "$root\Generate-Manifest.ps1" -TargetDir "$deploy\CoreAssembly" -Quiet
Write-Host "  CoreAssembly\manifest.json"
& "$root\Generate-Manifest.ps1" -TargetDir "$deploy\Modules" -Quiet
Write-Host "  Modules\manifest.json"

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
Write-Host "  $step. 실행 중인 WYNLAB.exe를 전부 종료 후 재실행"
Write-Host ""
Write-Host "  manifest.json은 위 폴더에 이미 포함되어 있습니다 - 서버에서 따로 실행할 스크립트 없음" -ForegroundColor DarkGray
