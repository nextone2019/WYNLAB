# 개발 PC 전용 - 운영 서버(221.150.162.198)로 옮길 배포 묶음 하나를 만든다. 이 폴더를
# 통째로(USB/외장하드 등으로) 서버에 복사한 다음, 그 안의 Install-WynlabServer.ps1을 서버에서
# 관리자 권한으로 실행하면 설치가 끝난다.
#
# 결과물: 00.DEV\_serverpackage\
#   Api\            (WYNLAB.Api  dotnet publish 결과물 - Release, 이 PC의 로컬 개발 DB
#                     정보가 안 섞이도록 Deploy-Local.ps1과 달리 web.config를 덮어쓰지 않는다)
#   ClickOnce\      (WYNLAB.exe  ClickOnce 게시물, InstallUrl=http://221.150.162.198:8091/)
#   CoreAssembly\   (BaseForm/Controls/Shared/Popup + manifest.json)
#   Modules\        (화면 모듈(AP/BA/MA/PR/SA/SM/SYS) + manifest.json)
#   Install-WynlabServer.ps1 / Generate-Manifest.ps1  (서버에서 실행할 스크립트도 같이 담아준다)
#
# 사용법(리포지토리 루트\00.DEV에서):
#   .\Build-ServerPackage.ps1
#
# DB 접속정보/JWT 키 같은 시크릿은 이 스크립트가 전혀 다루지 않는다 - 서버에서
# Install-WynlabServer.ps1을 실행할 때 파라미터로 그 자리에서 직접 넣는다(SERVER_SETUP.md 6-2와
# 같은 이유 - 저장소/패키지 어디에도 실제 비밀번호가 남지 않게 하기 위함).

param(
    [switch]$SkipShell
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$package = Join-Path $root "_serverpackage"

Write-Host "=== 1) 이전 패키지 정리 ===" -ForegroundColor Cyan
Remove-Item -Recurse -Force $package -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $package | Out-Null

Write-Host "=== 2) WYNLAB.Api 게시 (Release) ===" -ForegroundColor Cyan
# Deploy-Local.ps1과 달리 web.config를 로컬 DB 값으로 덮어쓰지 않는다 - 이 패키지는 실제
# 운영 서버로 갈 물건이라, DB/JWT 값은 서버에서 Install-WynlabServer.ps1이 직접 채운다.
Push-Location "$root\02.Server\WYNLAB.Api"
try {
    Remove-Item -Recurse -Force "$package\Api" -ErrorAction SilentlyContinue
    dotnet publish -c Release -o "$package\Api"
    if ($LASTEXITCODE -ne 0) { throw "Api 게시 실패" }
}
finally {
    Pop-Location
}
Write-Host "  완료: $package\Api"

Write-Host "=== 3) CoreAssembly + Modules (+ClickOnce) - Deploy-Package.ps1 재사용 ===" -ForegroundColor Cyan
if ($SkipShell) {
    & "$root\Deploy-Package.ps1"
}
else {
    & "$root\Deploy-Package.ps1" -IncludeShell
}
if ($LASTEXITCODE -ne 0) { throw "Deploy-Package.ps1 실패" }

Copy-Item "$root\_deploy\CoreAssembly" "$package\CoreAssembly" -Recurse -Force
Copy-Item "$root\_deploy\Modules" "$package\Modules" -Recurse -Force
if (-not $SkipShell) {
    if (-not (Test-Path "$root\_deploy\ClickOnce")) { throw "ClickOnce 게시물을 못 찾음 - Deploy-Package.ps1 -IncludeShell이 실패했을 수 있습니다" }
    Copy-Item "$root\_deploy\ClickOnce" "$package\ClickOnce" -Recurse -Force
}
else {
    Write-Host "  (-SkipShell: ClickOnce는 이번 패키지에 포함하지 않음 - 이미 서버에 최신 버전이 있을 때만 사용)" -ForegroundColor DarkGray
}

Write-Host "=== 4) 서버에서 실행할 스크립트 동봉 ===" -ForegroundColor Cyan
Copy-Item "$root\Install-WynlabServer.ps1" "$package\" -Force
Copy-Item "$root\Generate-Manifest.ps1" "$package\" -Force
Write-Host "  완료"

Write-Host ""
Write-Host "=== 패키지 완성: $package ===" -ForegroundColor Green
Get-ChildItem $package -Recurse -File | Measure-Object -Property Length -Sum | ForEach-Object {
    Write-Host ("  파일 {0}개, 총 {1:N1} MB" -f $_.Count, ($_.Sum / 1MB))
}
Write-Host ""
Write-Host "다음 단계:" -ForegroundColor Yellow
Write-Host "  1. '$package' 폴더 전체를 USB/외장하드 등으로 옮겨서 서버(221.150.162.198)에 복사"
Write-Host "  2. 서버 콘솔(RDP)에서 관리자 권한 PowerShell로 그 폴더에 들어가 실행:"
Write-Host '     .\Install-WynlabServer.ps1 -DbConnectionString "..." -JwtSecretKey "..."'
Write-Host "     (SERVER_SETUP.md 6-2 참고 - DB 접속정보/JWT 키는 여기서 직접 입력)"
