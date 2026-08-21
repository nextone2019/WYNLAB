# NEXTFramework — IIS 서버 환경 구축 가이드

대상 서버: `115.23.220.115` (DB + IIS 동일 서버)
이 문서는 서버에 **원격데스크톱(RDP)으로 접속한 상태**에서, PowerShell을 **관리자 권한**으로 열고 진행하는 걸 기준으로 작성했습니다.

---

## 0. 사전 확인

- Windows Server 2019 또는 2022 기준으로 작성했습니다. 버전이 다르면 일부 메뉴 위치만 다를 수 있습니다.
- IIS가 아직 설치되어 있지 않다면 먼저 설치합니다.

```powershell
Install-WindowsFeature -Name Web-Server -IncludeManagementTools
```

---

## 1. ASP.NET Core Hosting Bundle 설치

API(`NEXTFramework.Api`)는 IIS 안에서 ASP.NET Core로 동작하기 때문에 별도 런타임이 필요합니다.

1. 아래 링크에서 **".NET 8.0 Hosting Bundle"** 다운로드 (Windows 서버용)
   https://dotnet.microsoft.com/en-us/download/dotnet/8.0
   → "Hosting Bundle" 항목을 찾아 다운로드 (SDK나 Runtime이 아니라 **Hosting Bundle**이어야 합니다)
2. 다운로드한 설치파일 실행 → 기본값으로 설치
3. 설치 후 IIS 재시작

```powershell
net stop was /y
net start w3svc
```

---

## 2. 배포 전용 폴더 준비 (소스코드와 완전히 분리)

```powershell
New-Item -ItemType Directory -Path "C:\inetpub\nextfw-api-dev"    -Force
New-Item -ItemType Directory -Path "C:\inetpub\nextfw-api"        -Force
New-Item -ItemType Directory -Path "C:\inetpub\nextfw-deploy-dev" -Force
New-Item -ItemType Directory -Path "C:\inetpub\nextfw-deploy"     -Force
```

이 4개 폴더에는 나중에 Visual Studio에서 **게시(Publish)한 결과물**만 들어갑니다. 소스코드(csproj, cs 파일)는 여기 두지 않습니다.

---

## 3. IIS 앱풀 4개 생성 (API 2개는 반드시 "관리되는 코드 없음")

```powershell
Import-Module WebAdministration

# API용 앱풀 - No Managed Code 필수 (ASP.NET Core는 자체 프로세스로 동작, IIS는 리버스 프록시 역할만)
New-WebAppPool -Name "nextfw-api-dev-pool"
Set-ItemProperty IIS:\AppPools\nextfw-api-dev-pool -Name managedRuntimeVersion -Value ""

New-WebAppPool -Name "nextfw-api-pool"
Set-ItemProperty IIS:\AppPools\nextfw-api-pool -Name managedRuntimeVersion -Value ""

# ClickOnce 배포용 앱풀 - 정적 파일만 서빙하므로 기본값 그대로
New-WebAppPool -Name "nextfw-deploy-dev-pool"
New-WebAppPool -Name "nextfw-deploy-pool"
```

---

## 4. IIS 사이트 4개 생성

기존 시스템이 8081, 8082, 8083 포트를 사용 중이라 확인하여, 겹치지 않는 포트로 배정했습니다.

```powershell
New-Website -Name "nextfw-api-dev" `
  -PhysicalPath "C:\inetpub\nextfw-api-dev" `
  -ApplicationPool "nextfw-api-dev-pool" `
  -Port 8090

New-Website -Name "nextfw-api" `
  -PhysicalPath "C:\inetpub\nextfw-api" `
  -ApplicationPool "nextfw-api-pool" `
  -Port 8091

New-Website -Name "nextfw-deploy-dev" `
  -PhysicalPath "C:\inetpub\nextfw-deploy-dev" `
  -ApplicationPool "nextfw-deploy-dev-pool" `
  -Port 8092

New-Website -Name "nextfw-deploy" `
  -PhysicalPath "C:\inetpub\nextfw-deploy" `
  -ApplicationPool "nextfw-deploy-pool" `
  -Port 8093
```

> 사이트 생성 전에 한번 더 확인하고 싶으시면 아래로 한번 더 체크해보세요.
> ```powershell
> Get-NetTCPConnection -State Listen | Where-Object { $_.LocalPort -in 8090,8091,8092,8093 }
> ```
> 아무 결과도 안 나오면(빈 화면) 안전하게 비어있는 포트입니다.

---

## 5. ClickOnce 배포용 MIME 타입 등록

ClickOnce로 게시하면 `.application`, `.deploy` 확장자 파일이 생기는데, IIS가 기본적으로 이 확장자를 모릅니다. 아래를 실행하지 않으면 사용자가 설치 링크를 클릭해도 다운로드가 깨집니다.

```powershell
# nextfw-deploy-dev, nextfw-deploy 두 사이트 모두에 등록
foreach ($site in @("nextfw-deploy-dev", "nextfw-deploy")) {
    Add-WebConfigurationProperty -PSPath "IIS:\Sites\$site" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".application"; mimeType="application/x-ms-application"}
    Add-WebConfigurationProperty -PSPath "IIS:\Sites\$site" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".manifest"; mimeType="application/x-ms-manifest"}
    Add-WebConfigurationProperty -PSPath "IIS:\Sites\$site" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".deploy"; mimeType="application/octet-stream"}
}
```

이미 등록되어 있다는 오류가 나면 무시하셔도 됩니다 (중복 등록 시도일 뿐입니다).

---

## 6. SSL 인증서 (지금은 건너뛰어도 됩니다 - 추후 진행)

지금 단계에서는 **HTTP로 먼저 구성**하고, 실제 서비스 오픈 전에 SSL을 추가하는 걸 추천드립니다. HTTP와 HTTPS를 같은 포트에 동시에 바인딩하면 충돌이 나기 때문에, SSL을 붙일 때는 포트를 새로 하나 더 열어서 진행하는 게 안전합니다.

나중에 필요해지면 이런 식으로 진행합니다 (참고용, 지금 실행 안 하셔도 됩니다).

```powershell
# 자체서명 인증서 생성 (5년 유효)
$cert = New-SelfSignedCertificate -DnsName "115.23.220.115", "nextfw.internal" `
  -CertStoreLocation "cert:\LocalMachine\My" `
  -NotAfter (Get-Date).AddYears(5)

# 예: 운영 API에 SSL 전용 포트(8443)를 새로 추가하고 싶다면
New-WebBinding -Name "nextfw-api" -Protocol https -Port 8443 -SslFlags 0
(Get-WebBinding -Name "nextfw-api" -Protocol https -Port 8443).AddSslCertificate($cert.Thumbprint, "my")
```

---

## 7. 방화벽 인바운드 규칙 오픈

```powershell
New-NetFirewallRule -DisplayName "NEXTFW API Dev"    -Direction Inbound -LocalPort 8090 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "NEXTFW API Prod"   -Direction Inbound -LocalPort 8091 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "NEXTFW Deploy Dev" -Direction Inbound -LocalPort 8092 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "NEXTFW Deploy Prod"-Direction Inbound -LocalPort 8093 -Protocol TCP -Action Allow
```

회사 자체 방화벽 장비(라우터/UTM 등)가 별도로 있다면, 그쪽에서도 해당 포트를 열어주셔야 사내 다른 PC에서 접속이 가능합니다.

---

## 8. 여기까지 되면 확인하는 방법

브라우저에서 `http://115.23.220.115:8091/` (운영 API 포트)로 접속했을 때, 아직 아무것도 게시 안 한 상태라 **IIS 기본 오류 페이지나 403/404**가 나오면 정상입니다. 이후 Visual Studio에서 API 프로젝트를 게시하고 나면 정상 응답이 옵니다.

## 다음 단계

이 서버 세팅이 끝나면:
1. Api 프로젝트를 `nextfw-api-dev` 폴더로 게시(Publish)
2. 브라우저에서 `http://115.23.220.115:8090/swagger` 접속 → Swagger 페이지가 뜨면 성공
3. 이후 클라이언트(.NET Framework 4.8) 게시 → 로그인 테스트
