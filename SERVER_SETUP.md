# 원격 서버(115.23.220.115) 세팅 가이드

목표: 로컬 PC에서 API를 직접 켜서 테스트할 필요 없이, 개발서버/운영서버 둘 다 `115.23.220.115`
한 대에서 항상 떠 있는 구조로 만든다. 클라이언트(`WYNLAB.exe`)만 실행하면 되고, API는 IIS가
알아서 계속 띄워둔다.

이 문서는 제가 직접 그 서버에 접속할 수 없어서, 그 서버 콘솔(또는 RDP)에서 직접 실행할 명령과
설정을 순서대로 정리한 것이다. 막히는 단계 번호를 말해주면 그 부분만 다시 봐줄 수 있다.

## 0. 최종 그림

```
D:\WYNLAB\                              <- 이 문서에서 만드는 루트
  Api\
    Dev\        -> IIS 사이트, 포트 8090  (WYNLAB.Api, Debug 게시물)
    Prod\       -> IIS 사이트, 포트 8091  (WYNLAB.Api, Release 게시물)
  ClickOnce\
    Dev\        -> IIS 사이트, 포트 8092  (WYNLAB.Shell ClickOnce, Debug)
    Prod\       -> IIS 사이트, 포트 8093  (WYNLAB.Shell ClickOnce, Release)
  Modules\      <- 공유 폴더(\\115.23.220.115\WYNLAB\Modules)로 열어둠
    Dev\
      SM\  BA\  SA\  PR\  MA\            <- 화면 DLL을 모듈별로 여기 배포
    Prod\
      SM\  BA\  SA\  PR\  MA\
```

포트 4개(8090~8093)는 지금 이미 `appsettings.Dev.json`/`appsettings.Prod.json`/ClickOnce
게시 프로필(`Dev.pubxml`/`Prod.pubxml`)에 박혀있는 값 그대로라 서버 쪽만 맞추면 클라이언트
코드는 안 고쳐도 된다. (`ModulesPath`는 `Modules\Dev`/`Modules\Prod`로 나누도록 방금
appsettings 두 파일을 고쳐뒀다 - 이 문서의 폴더 구조와 짝이 맞다.)

---

## 1. 사전 확인

RDP로 `115.23.220.115`에 접속한 뒤, PowerShell(관리자 권한)에서:

```powershell
# OS/버전 확인
Get-ComputerInfo | Select-Object WindowsProductName, OsVersion

# IIS가 이미 있는지
Get-WindowsFeature -Name Web-Server   # Windows Server인 경우
# 또는(일반 Windows 10/11이면)
Get-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole

# D 드라이브 존재/여유공간 확인
Get-PSDrive D
```

`Get-WindowsFeature`가 "명령을 찾을 수 없음"이면 Windows Server가 아니라 일반 Windows(10/11)
데스크톱판일 수 있다 - 그 경우 "제어판 > Windows 기능 켜기/끄기 > 인터넷 정보 서비스"로 켜야
한다. 어떤 쪽인지 알려주면 이후 단계를 그에 맞게 조정해줄 수 있다.

---

## 2. IIS + ASP.NET Core Hosting Bundle 설치

### 2-1. IIS 설치

**Windows Server인 경우:**
```powershell
Install-WindowsFeature -Name Web-Server -IncludeManagementTools
```

**Windows 10/11인 경우:**
```powershell
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole, IIS-WebServer, IIS-CommonHttpFeatures, IIS-HttpErrors, IIS-ApplicationDevelopment, IIS-NetFxExtensibility45, IIS-ISAPIExtensions, IIS-ISAPIFilter, IIS-ASPNET45 -All
```

설치 후 브라우저로 `http://localhost` 접속했을 때 IIS 기본 페이지("IIS Windows Server" 등)가
뜨면 성공.

### 2-2. .NET 8.0 Hosting Bundle 설치 (중요 - 이게 없으면 API가 IIS에서 절대 안 뜬다)

`WYNLAB.Api`는 ASP.NET Core 8이고 `AspNetCoreHostingModel=InProcess`로 되어 있어서, IIS가
.NET 앱을 직접 호스팅하려면 이 번들이 필요하다(그냥 .NET 8 런타임만 설치해서는 안 됨).

1. 브라우저로 https://dotnet.microsoft.com/download/dotnet/8.0 접속
2. "ASP.NET Core Runtime 8.0.x" 항목의 "Hosting Bundle" (Windows용 인스톨러) 다운로드
3. 설치 후 **IIS 재시작 필요**:
   ```powershell
   net stop was /y
   net start w3svc
   ```
4. 확인: `C:\Program Files\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll` 파일이 있으면 정상 설치된 것.

---

## 3. 폴더 만들기

```powershell
$root = "D:\WYNLAB"
New-Item -ItemType Directory -Force -Path `
  "$root\Api\Dev", "$root\Api\Prod", `
  "$root\ClickOnce\Dev", "$root\ClickOnce\Prod", `
  "$root\Modules\Dev\SM", "$root\Modules\Dev\BA", "$root\Modules\Dev\SA", "$root\Modules\Dev\PR", "$root\Modules\Dev\MA", `
  "$root\Modules\Prod\SM", "$root\Modules\Prod\BA", "$root\Modules\Prod\SA", "$root\Modules\Prod\PR", "$root\Modules\Prod\MA"
```

---

## 4. Modules 폴더 네트워크 공유

클라이언트(각 직원 PC)가 `\\115.23.220.115\WYNLAB\Modules\...` 경로로 화면 DLL을 읽어가야
하므로, `D:\WYNLAB` 자체를 `WYNLAB`이라는 이름으로 공유한다.

```powershell
New-SmbShare -Name "WYNLAB" -Path "D:\WYNLAB" -FullAccess "Administrators" -ReadAccess "Everyone"
```

- 사내망이 도메인 환경이면 `-ReadAccess "Everyone"` 대신 `"Domain Users"` 등으로 좁히는 걸 권장.
- 배포(=DLL 갱신)는 이 서버에 직접 로그인해서 파일을 복사하는 방식으로 할 거라 쓰기 권한은
  Administrators만 있으면 된다. 나중에 다른 배포 계정을 쓰게 되면 그 계정에 쓰기 권한을 추가.

확인: 다른 PC에서 탐색기 주소창에 `\\115.23.220.115\WYNLAB\Modules` 입력해서 폴더가 보이는지 체크.

---

## 5. 방화벽 규칙

```powershell
New-NetFirewallRule -DisplayName "WYNLAB API Dev (8090)"       -Direction Inbound -LocalPort 8090 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB API Prod (8091)"      -Direction Inbound -LocalPort 8091 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB ClickOnce Dev (8092)" -Direction Inbound -LocalPort 8092 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB ClickOnce Prod (8093)" -Direction Inbound -LocalPort 8093 -Protocol TCP -Action Allow
```

(파일공유용 SMB 445 포트는 보통 이미 열려있지만, 안 열려있으면 `New-NetFirewallRule -DisplayName "SMB" -Direction Inbound -LocalPort 445 -Protocol TCP -Action Allow`.)

---

## 6. IIS 사이트 4개 만들기

`WebAdministration` 모듈 사용(IIS 설치하면 자동으로 같이 깔림):

```powershell
Import-Module WebAdministration

# --- API (Dev) ---
New-WebAppPool -Name "WYNLAB-Api-Dev"
Set-ItemProperty "IIS:\AppPools\WYNLAB-Api-Dev" -Name managedRuntimeVersion -Value ""   # ASP.NET Core는 "관리되지 않음"으로 둬야 함
New-Website -Name "WYNLAB-Api-Dev" -PhysicalPath "D:\WYNLAB\Api\Dev" -ApplicationPool "WYNLAB-Api-Dev" -Port 8090

# --- API (Prod) ---
New-WebAppPool -Name "WYNLAB-Api-Prod"
Set-ItemProperty "IIS:\AppPools\WYNLAB-Api-Prod" -Name managedRuntimeVersion -Value ""
New-Website -Name "WYNLAB-Api-Prod" -PhysicalPath "D:\WYNLAB\Api\Prod" -ApplicationPool "WYNLAB-Api-Prod" -Port 8091

# --- ClickOnce (Dev) - 정적 파일만 서빙하면 되므로 기본 앱풀(관리 코드 버전 없음)로 충분 ---
New-WebAppPool -Name "WYNLAB-ClickOnce-Dev"
New-Website -Name "WYNLAB-ClickOnce-Dev" -PhysicalPath "D:\WYNLAB\ClickOnce\Dev" -ApplicationPool "WYNLAB-ClickOnce-Dev" -Port 8092

# --- ClickOnce (Prod) ---
New-WebAppPool -Name "WYNLAB-ClickOnce-Prod"
New-Website -Name "WYNLAB-ClickOnce-Prod" -PhysicalPath "D:\WYNLAB\ClickOnce\Prod" -ApplicationPool "WYNLAB-ClickOnce-Prod" -Port 8093
```

**주의**: 이 4개 사이트는 아직 폴더가 비어있어서 지금 브라우저로 접속하면 403/404가 뜨는 게
정상이다. 8번에서 실제로 게시물을 넣은 다음에 확인하면 된다.

GUI로 하고 싶으면: IIS 관리자 실행 → 왼쪽 트리에서 "사이트" 우클릭 → "웹 사이트 추가" →
사이트 이름/물리 경로/포트를 위 표대로 입력. 애플리케이션 풀은 "고급 설정"에서 새로 만들고
".NET CLR 버전"을 "관리 코드 없음"으로 지정(API 2개 사이트만 해당, ClickOnce 2개는 기본값 둬도 됨).

---

## 7. 시크릿을 서버 환경변수로 설정 (API 실행에 필요)

지난 작업에서 `appsettings.Production.json`엔 실제 DB 비밀번호/JWT 키를 넣지 않기로 했다 -
대신 서버 환경변수로 주입한다. 이 서버(115.23.220.115)에서 시스템 환경변수로 등록:

```powershell
[Environment]::SetEnvironmentVariable("Jwt__SecretKey", "<새로_발급한_운영_JWT_키>", "Machine")
[Environment]::SetEnvironmentVariable("ConnectionStrings__WynlabDb", "Server=tcp:<운영DB서버>,1433;Database=<운영DB명>;User Id=<계정>;Password=<비밀번호>;TrustServerCertificate=True;", "Machine")
```

- `Jwt__SecretKey` 값은 이전에 새로 생성해둔 값을 쓰면 된다(대화에서 이미 전달함).
- **등록 후 IIS 재시작 필요**(환경변수는 프로세스 시작 시점에 읽히므로): `iisreset`
- 확인: `iisreset` 후 `[Environment]::GetEnvironmentVariable("Jwt__SecretKey", "Machine")`로 값이 제대로 박혔는지 재확인.

개발서버(Dev API)는 appsettings.Development.json을 그대로 쓰거나(로컬 dotnet user-secrets는
이 서버엔 없으니), 이 서버에서 Dev용 값도 같은 방식으로 환경변수를 따로 등록해도 된다
(`ASPNETCORE_ENVIRONMENT=Development`로 실행되는 사이트에서는 `appsettings.Development.json` +
환경변수가 같이 적용된다). Dev는 보안 민감도가 낮으니 개발 편의상 appsettings에 값을 직접
넣어도 괜찮다 - 다만 그 파일을 git에 커밋하지 않도록 주의.

---

## 8. 첫 배포

로컬 개발 PC(지금 이 리포가 있는 PC)에서 게시(publish)한 �음, 그 결과물을 서버의 해당 폴더로
복사하는 흐름이다.

### 8-1. WYNLAB.Api 게시

```bash
cd "D:\01. SOURCE\00. WYNLAB\02.Server\WYNLAB.Api"
dotnet publish -c Release -o "publish-prod"
dotnet publish -c Debug -o "publish-dev"
```

`publish-prod` 폴더 전체를 서버의 `D:\WYNLAB\Api\Prod\`로, `publish-dev` 폴더 전체를
`D:\WYNLAB\Api\Dev\`로 복사(파일공유/RDP로 붙여넣기, 또는 나중엔 배포 스크립트로 자동화 가능).

복사 후 서버에서 해당 앱풀 재시작:
```powershell
Restart-WebAppPool -Name "WYNLAB-Api-Dev"
Restart-WebAppPool -Name "WYNLAB-Api-Prod"
```

확인: 서버에서 `curl http://localhost:8091/api/auth/login -Method Post -Body ...` 또는 그냥
브라우저로 Swagger가 있으면(`Development`일 때만 켜짐, `Program.cs` 참고) `http://115.23.220.115:8090/swagger` 접속해서 응답 오는지 확인.

### 8-2. 화면(모듈) DLL 배포

각 화면 프로젝트를 빌드하면(예: `99.SOURCE\SM\USER\WYNLAB.SM.USER.sln`) 이미 만들어둔
`CopyToModulesFolder` 빌드 후 타겟이 로컬의 `01.Client\WYNLAB.Shell\bin\...\Modules\`로
복사해준다 - 이건 **로컬 테스트용**이고, 서버 배포용으로는 그 빌드 결과 DLL(+pdb)을 직접
서버의 해당 모듈 폴더로 옮겨야 한다. 예:

```
99.SOURCE\SM\USER\WYNLAB.SM.USER\bin\Release\net48\WYNLAB.SM.USER.dll
  -> D:\WYNLAB\Modules\Prod\SM\WYNLAB.SM.USER.dll   (서버)
  -> D:\WYNLAB\Modules\Dev\SM\WYNLAB.SM.USER.dll    (테스트용으로 Dev에도)
```

같은 방식으로 `USERGROUP`/`MENU`/`USERAUTH`도 각각 `Modules\{Dev|Prod}\SM\`에 넣으면 된다.
(전부 SM 모듈이라 지금은 SM 폴더 하나만 쓰지만, BA/SA/PR/MA 화면이 생기면 그 모듈 폴더에 넣으면
됨 - `ModuleLoader`가 하위 폴더까지 재귀적으로 스캔하도록 이미 고쳐뒀다.)

### 8-3. 클라이언트(WYNLAB.exe) ClickOnce 게시

Visual Studio에서 `WYNLAB.Shell` 프로젝트 우클릭 → "게시(Publish)" → 게시 프로필로
`Dev`(또는 `Prod`) 선택 → 게시. 그러면 VS가 `PublishUrl`(`http://115.23.220.115:8092/` 또는
`:8093/`)로 직접 업로드를 시도하는데, 이건 그 URL이 실제로 쓰기 가능한 게시 지점으로 열려있어야
동작한다(IIS의 웹 배포 게시 또는 FTP) - 지금 6번에서 만든 사이트는 "정적 파일 서빙"만 되는
평범한 사이트라 VS가 직접 업로드하지 못할 수 있다. 그 경우엔:

1. 게시 프로필을 "폴더" 방식으로 로컬에 먼저 게시(`dotnet publish` 또는 VS의 폴더 프로필)
2. 그 결과물(`.application`, `.exe.manifest`, `Application Files\` 폴더 등)을 서버의
   `D:\WYNLAB\ClickOnce\Dev\`(또는 `Prod\`)로 통째로 복사

둘 중 뭐가 더 편할지, 그리고 VS가 직접 업로드하는 방식을 쓰고 싶으면(웹 배포/FTP 세팅 추가
필요) 여기서 막히면 바로 물어봐줘 - 상황 보고 같이 정하면 된다.

---

## 9. 검증 체크리스트

- [ ] `http://115.23.220.115:8090/` , `:8091/` 접속 시 IIS/API가 응답(404여도 괜찮음 - "연결 자체"가 되는지가 핵심. Swagger가 있는 Dev는 `/swagger`로 확인)
- [ ] `\\115.23.220.115\WYNLAB\Modules\Prod\SM\` 폴더가 다른 PC 탐색기에서 보임
- [ ] 서버에서 `iisreset` 후에도 4개 사이트가 다시 정상 기동(자동 시작)
- [ ] 화면 DLL을 `Modules\Dev\SM\`에 넣고 클라이언트(Dev 환경 선택)로 로그인 → 메뉴 클릭 시 화면이 뜸
- [ ] ClickOnce 설치 URL(`:8092`/`:8093`)로 브라우저 접속 시 설치 페이지가 뜸

막히는 단계가 있으면 그 번호(예: "6번에서 New-Website 실행했는데 이런 에러 났어")로 알려주면
바로 짚어줄게.
