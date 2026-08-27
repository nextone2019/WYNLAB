# 배포 폴더 하나를 통째로 훑어서 manifest.json을 만든다. 클라이언트(HttpFileSync,
# CoreAssemblyUpdater)는 이 파일의 해시 목록만 보고 "무엇이 바뀌었는지"를 판단한다.
#
# 하위 폴더까지 재귀적으로 포함하고, 경로는 슬래시(/)로 구분된 상대경로로 기록한다
# (예: "SM/WYNLAB.SM.frmMinorCode.dll") - HTTP URL에 그대로 이어붙일 수 있어야 하기 때문.
#
# 사람이 버전 번호를 손으로 적지 않는 이유: 값을 바꾸는 걸 깜빡하면 클라이언트가 조용히
# 예전 버전을 계속 쓰게 된다. 파일 해시를 자동 계산하면 그 실수 자체가 불가능해진다.
#
# 사용법:
#   .\Generate-Manifest.ps1 -TargetDir "D:\01. SOURCE\00. WYNLAB\_deploy\Modules"
#   .\Generate-Manifest.ps1 -TargetDir "D:\WYNLAB\Assets"      # 서버에서 이미지 추가한 뒤

param(
    [string]$TargetDir = (Get-Location).Path,
    [switch]$Quiet
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $TargetDir)) { throw "폴더를 찾을 수 없습니다: $TargetDir" }

$rootPath = (Resolve-Path $TargetDir).Path

# manifest.json 자기 자신과 배포용 스크립트는 목록에서 뺀다 - 클라이언트가 받아갈 대상이 아니다.
$files = Get-ChildItem $rootPath -Recurse -File |
    Where-Object { $_.Name -ne "manifest.json" -and $_.Extension -ne ".ps1" } |
    ForEach-Object {
        $relative = $_.FullName.Substring($rootPath.Length).TrimStart('\', '/').Replace('\', '/')
        [PSCustomObject]@{
            fileName = $relative
            sha256   = (Get-FileHash -Path $_.FullName -Algorithm SHA256).Hash
            size     = $_.Length
        }
    }

$manifest = [PSCustomObject]@{ files = @($files) }
$manifestPath = Join-Path $rootPath "manifest.json"

# BOM 없이 저장해야 한다. Windows PowerShell 5.1의 `Out-File -Encoding utf8`은 UTF-8 BOM을
# 붙이는데, 파일로 읽을 땐 .NET이 BOM을 알아서 걸러주지만 HTTP로 받으면 문자열 맨 앞에
# U+FEFF가 그대로 남아서 System.Text.Json이 "invalid start of a value"로 파싱에 실패한다.
# (클라이언트 쪽에서도 방어적으로 BOM을 떼어내지만, 애초에 안 붙이는 게 맞다.)
$json = $manifest | ConvertTo-Json -Depth 3
[System.IO.File]::WriteAllText($manifestPath, $json, (New-Object System.Text.UTF8Encoding($false)))

if (-not $Quiet) {
    Write-Host "manifest.json 생성 완료: $manifestPath ($(@($files).Count)개 파일)"
    $files | Format-Table fileName, sha256, size -AutoSize
}
