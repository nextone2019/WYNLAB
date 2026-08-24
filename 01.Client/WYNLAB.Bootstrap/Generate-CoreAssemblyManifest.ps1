# WYNLAB.BaseForm.dll/WYNLAB.Shared.dll/WYNLAB.Controls.dll을 서버의 CoreAssembly 공유폴더에 배포한 뒤
# 이 스크립트를 그 폴더 위치에서 실행하면 manifest.json을 자동 생성한다.
# 사람이 버전 번호를 손으로 적지 않는 이유: 값을 바꾸는 걸 깜빡하면 클라이언트가 조용히
# 예전 버전을 계속 쓰게 되는 문제가 있어서(과거 DB명 오타로 겪은 것과 같은 유형의 실수) -
# 파일 해시를 자동 계산하면 그 실수 자체가 불가능해진다.
#
# 사용법:
#   Set-Location "D:\WYNLAB\CoreAssembly"
#   & "그 스크립트가 있는 경로\Generate-CoreAssemblyManifest.ps1"

param(
    [string]$TargetDir = (Get-Location).Path
)

$trackedFiles = @("WYNLAB.BaseForm.dll", "WYNLAB.Shared.dll", "WYNLAB.Controls.dll")

$files = foreach ($name in $trackedFiles) {
    $path = Join-Path $TargetDir $name
    if (-not (Test-Path $path)) {
        Write-Warning "$name 이(가) $TargetDir 에 없습니다 - 건너뜁니다."
        continue
    }
    $hash = (Get-FileHash -Path $path -Algorithm SHA256).Hash
    $size = (Get-Item $path).Length
    [PSCustomObject]@{ fileName = $name; sha256 = $hash; size = $size }
}

$manifest = [PSCustomObject]@{ files = @($files) }
$manifestPath = Join-Path $TargetDir "manifest.json"
$manifest | ConvertTo-Json -Depth 3 | Out-File -FilePath $manifestPath -Encoding utf8

Write-Host "manifest.json 생성 완료: $manifestPath"
$files | Format-Table fileName, sha256, size -AutoSize
