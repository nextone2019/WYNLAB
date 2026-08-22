# 원격 서버(115.23.220.115) 세팅 가이드

목표: 로컬 PC에서 API를 직접 켜서 테스트할 필요 없이, 서버(`115.23.220.115`)에서 개발(Dev)과
운영(Prod)이 각각 항상 떠 있는 구조로 만든다. 클라이언트(`WYNLAB.exe`)만 실행하면 되고, API는
IIS가 알아서 계속 띄워둔다.

이 문서는 제가 직접 그 서버에 접속할 수 없어서, 서버 콘솔(RDP)에서 직접 실행할 명령과 설정을
순서대로 정리한 것이다. 막히는 단계 번호를 말해주면 그 부분만 다시 봐줄 수 있다.

기존에 이 포트(8090~8093)를 쓰던 리브랜딩 이전 사이트(`nextfw-api`, `nextfw-deploy`)는 이미
삭제하셨다고 했으니, 아래 내용대로 새로 만들면 된다.

---

## 0. 최종 그림

운영(Prod)과 개발(Dev)을 **공유 루트 자체를 분리**하는 구조로 설계했다:

```
\\115.23.220.115\WYNLAB          <- 운영
\\115.23.220.115\WYNLAB_TEST     <- 개발/테스트
```

서버 폴더 구조와 포트:

```
D:\WYNLAB\                    <- 공유 이름 "WYNLAB"
  Api\        -> IIS 사이트 "WYNLAB-Api",        포트 8091   (WYNLAB.Api, Release 게시물)
  ClickOnce\  -> IIS 사이트 "WYNLAB-ClickOnce",   포트 8093   (WYNLAB.Shell ClickOnce, Release)
  Modules\    -> \\115.23.220.115\WYNLAB\Modules로 노출
    SM\  BA\  SA\  PR\  MA\

D:\WYNLAB_TEST\                <- 공유 이름 "WYNLAB_TEST"
  Api\        -> IIS 사이트 "WYNLAB_TEST-Api",      포트 8090   (WYNLAB.Api, Debug 게시물)
  ClickOnce\  -> IIS 사이트 "WYNLAB_TEST-ClickOnce", 포트 8092   (WYNLAB.Shell ClickOnce, Debug)
  Modules\    -> \\115.23.220.115\WYNLAB_TEST\Modules로 노출
    SM\  BA\  SA\  PR\  MA\
```

클라이언트 설정(`appsettings.Dev.json`/`Prod.json`)은 이미 이 구조를 가리키도록 되어 있다:
- Production 환경 → `ApiBaseUrl: http://115.23.220.115:8091/`, `ModulesPath: \\115.23.220.115\WYNLAB\Modules`
- Development 환경 → `ApiBaseUrl: http://115.23.220.115:8090/`, `ModulesPath: \\115.23.220.115\WYNLAB_TEST\Modules`

ClickOnce 게시 URL(`Dev.pubxml`/`Prod.pubxml`)도 각각 `:8092`/`:8093`으로 이미 맞춰져 있다.

---

## 1. .NET 8.0 Hosting Bundle 확인 (중요 - 이게 없으면 API가 IIS에서 절대 안 뜬다)

IIS는 이미 설치되어 있는 걸 확인했지만, ASP.NET Core 앱을 IIS 안에서 직접 돌리려면 이 번들이
별도로 필요하다(그냥 .NET 8 런타임만 설치해서는 안 됨). 먼저 이미 있는지 확인:

```powershell
Test-Path "C:\Program Files\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll"
```

`True`면 2번으로 건너뛰면 된다. `False`면:

1. 브라우저로 https://dotnet.microsoft.com/download/dotnet/8.0 접속
2. "ASP.NET Core Runtime 8.0.x" 항목의 **"Hosting Bundle"**(Windows용 인스톨러) 다운로드 - SDK나
   일반 Runtime이 아니라 반드시 Hosting Bundle이어야 한다.
3. 설치 후 IIS 재시작:
   ```powershell
   net stop was /y
   net start w3svc
   ```
4. 다시 위 `Test-Path`로 `True` 나오는지 확인.

---

## 2. 폴더 만들기 (양쪽 다)

```powershell
New-Item -ItemType Directory -Force -Path `
  "D:\WYNLAB\Api", "D:\WYNLAB\ClickOnce", `
  "D:\WYNLAB\Modules\SM", "D:\WYNLAB\Modules\BA", "D:\WYNLAB\Modules\SA", "D:\WYNLAB\Modules\PR", "D:\WYNLAB\Modules\MA", `
  "D:\WYNLAB_TEST\Api", "D:\WYNLAB_TEST\ClickOnce", `
  "D:\WYNLAB_TEST\Modules\SM", "D:\WYNLAB_TEST\Modules\BA", "D:\WYNLAB_TEST\Modules\SA", "D:\WYNLAB_TEST\Modules\PR", "D:\WYNLAB_TEST\Modules\MA"
```

---

## 3. 네트워크 공유 (양쪽 다)

클라이언트(각 직원 PC)가 `\\115.23.220.115\WYNLAB\Modules\...` 경로로 화면 DLL을 읽어가야
하므로, 두 루트를 각각 공유한다.

```powershell
New-SmbShare -Name "WYNLAB"      -Path "D:\WYNLAB"      -FullAccess "Administrators" -ReadAccess "Everyone"
New-SmbShare -Name "WYNLAB_TEST" -Path "D:\WYNLAB_TEST"  -FullAccess "Administrators" -ReadAccess "Everyone"
```

- 사내망이 도메인 환경이면 `-ReadAccess "Everyone"` 대신 `"Domain Users"` 등으로 좁히는 걸 권장.
- 배포(=DLL 갱신)는 서버에 직접 로그인해서 파일을 복사하는 방식으로 할 거라 쓰기 권한은
  Administrators만 있으면 된다.

확인: 다른 PC에서 탐색기 주소창에 `\\115.23.220.115\WYNLAB\Modules`, `\\115.23.220.115\WYNLAB_TEST\Modules`
둘 다 입력해서 폴더가 보이는지 체크.

---

## 4. 방화벽 규칙 (양쪽 다)

```powershell
New-NetFirewallRule -DisplayName "WYNLAB_TEST API (8090)"       -Direction Inbound -LocalPort 8090 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB API (8091)"             -Direction Inbound -LocalPort 8091 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB_TEST ClickOnce (8092)"  -Direction Inbound -LocalPort 8092 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB ClickOnce (8093)"       -Direction Inbound -LocalPort 8093 -Protocol TCP -Action Allow
```

(파일공유용 SMB 445 포트는 보통 이미 열려있지만, 안 열려있으면
`New-NetFirewallRule -DisplayName "SMB" -Direction Inbound -LocalPort 445 -Protocol TCP -Action Allow`.
회사 자체 방화벽 장비(라우터/UTM 등)가 별도로 있다면 그쪽에서도 이 4개 포트를 열어줘야 사내 다른
PC에서 접속 가능하다.)

시작 전에 한 번 더 확인하고 싶으면:
```powershell
Get-NetTCPConnection -State Listen | Where-Object { $_.LocalPort -in 8090,8091,8092,8093 }
```
아무 결과도 안 나오면(빈 화면) 안전하게 비어있는 포트다.

---

## 5. IIS 앱풀 + 사이트 4개

```powershell
Import-Module WebAdministration

# --- Api 앱풀 2개 - "관리되는 코드 없음" 필수 (ASP.NET Core는 자체 프로세스로 동작, IIS는 리버스 프록시 역할) ---
New-WebAppPool -Name "WYNLAB-Api"
Set-ItemProperty "IIS:\AppPools\WYNLAB-Api" -Name managedRuntimeVersion -Value ""

New-WebAppPool -Name "WYNLAB_TEST-Api"
Set-ItemProperty "IIS:\AppPools\WYNLAB_TEST-Api" -Name managedRuntimeVersion -Value ""

# --- ClickOnce 앱풀 2개 - 정적 파일만 서빙하므로 기본값 그대로 ---
New-WebAppPool -Name "WYNLAB-ClickOnce"
New-WebAppPool -Name "WYNLAB_TEST-ClickOnce"

# --- 사이트 4개 ---
New-Website -Name "WYNLAB-Api"            -PhysicalPath "D:\WYNLAB\Api"             -ApplicationPool "WYNLAB-Api"            -Port 8091
New-Website -Name "WYNLAB_TEST-Api"       -PhysicalPath "D:\WYNLAB_TEST\Api"        -ApplicationPool "WYNLAB_TEST-Api"       -Port 8090
New-Website -Name "WYNLAB-ClickOnce"      -PhysicalPath "D:\WYNLAB\ClickOnce"       -ApplicationPool "WYNLAB-ClickOnce"      -Port 8093
New-Website -Name "WYNLAB_TEST-ClickOnce" -PhysicalPath "D:\WYNLAB_TEST\ClickOnce"  -ApplicationPool "WYNLAB_TEST-ClickOnce" -Port 8092
```

**주의**: 이 4개 사이트는 아직 폴더가 비어있어서 지금 브라우저로 접속하면 403/404가 뜨는 게
정상이다. 7번에서 실제로 게시물을 넣은 다음에 확인하면 된다.

GUI로 하고 싶으면: IIS 관리자 실행 → 왼쪽 트리에서 "사이트" 우클릭 → "웹 사이트 추가" →
사이트 이름/물리 경로/포트를 위 표대로 입력. Api 사이트 2개는 애플리케이션 풀 "고급 설정"에서
".NET CLR 버전"을 "관리 코드 없음"으로 지정(ClickOnce 사이트 2개는 기본값 둬도 됨).

---

## 6. ClickOnce MIME 타입 등록 (빠뜨리기 쉬운 부분)

ClickOnce로 게시하면 `.application`, `.manifest`, `.deploy` 확장자 파일이 생기는데, IIS가
기본적으로 이 확장자를 모른다. 등록하지 않으면 사용자가 설치 링크를 클릭해도 다운로드가 깨진다.

```powershell
foreach ($site in @("WYNLAB-ClickOnce", "WYNLAB_TEST-ClickOnce")) {
    Add-WebConfigurationProperty -PSPath "IIS:\Sites\$site" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".application"; mimeType="application/x-ms-application"}
    Add-WebConfigurationProperty -PSPath "IIS:\Sites\$site" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".manifest"; mimeType="application/x-ms-manifest"}
    Add-WebConfigurationProperty -PSPath "IIS:\Sites\$site" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".deploy"; mimeType="application/octet-stream"}
}
```

이미 등록되어 있다는 오류가 나면 무시해도 된다(중복 등록 시도일 뿐).

---

## 7. 첫 배포

로컬 개발 PC(지금 이 리포가 있는 PC)에서 게시(publish)한 다음, 그 결과물을 서버의 해당 폴더로
복사하는 흐름이다.

### 7-1. WYNLAB.Api 게시 (양쪽 다)

```bash
cd "D:\01. SOURCE\00. WYNLAB\02.Server\WYNLAB.Api"
dotnet publish -c Release -o "publish-prod"
dotnet publish -c Debug   -o "publish-dev"
```

`publish-prod` 폴더 전체 내용물을 서버의 `D:\WYNLAB\Api\`로, `publish-dev` 전체를
`D:\WYNLAB_TEST\Api\`로 복사한다.

### 7-2. 시크릿을 사이트별 web.config에 등록 (중요)

`appsettings.Production.json`/`Development.json`엔 실제 DB 비밀번호·JWT 키를 넣지 않기로
했었다 - 대신 게시 폴더 안에 자동 생성된 `web.config`에 사이트별로 다른 값을 직접 넣는다(운영/
개발 앱풀이 같은 서버에 같이 떠 있어서, 서버 전체에 걸리는 시스템 환경변수를 쓰면 두 사이트가
같은 값을 공유해버려 안 된다).

`D:\WYNLAB\Api\web.config` (운영) 열어서 `<aspNetCore>` 안의 `<environmentVariables>`를:
```xml
<aspNetCore processPath="dotnet" arguments=".\WYNLAB.Api.dll" ... hostingModel="inprocess">
  <environmentVariables>
    <environmentVariable name="Jwt__SecretKey" value="<운영용_JWT_키>" />
    <environmentVariable name="ConnectionStrings__WynlabDb" value="Server=tcp:<운영DB서버>,1433;Database=<운영DB명>;User Id=<계정>;Password=<비밀번호>;TrustServerCertificate=True;" />
  </environmentVariables>
</aspNetCore>
```

`D:\WYNLAB_TEST\Api\web.config`(개발)도 같은 방식으로, 개발용 DB 접속정보를 넣는다(JWT 키는
운영과 다른 값을 써도 되고 같이 써도 무방 - 보안 민감도 낮은 개발서버라 편한 대로).

수정 후 해당 앱풀 재시작:
```powershell
Restart-WebAppPool -Name "WYNLAB-Api"
Restart-WebAppPool -Name "WYNLAB_TEST-Api"
```

운영 JWT 키는 이전에 새로 생성해둔 값을 쓰면 된다(대화에서 이미 전달함).

확인: 브라우저로 `http://115.23.220.115:8091/`(운영), `http://115.23.220.115:8090/swagger`(개발
- Development 환경이라 Swagger가 켜져 있음, `Program.cs` 참고) 접속해서 응답 오는지 확인.

### 7-3. 화면(모듈) DLL 배포

각 화면 프로젝트를 빌드하면(예: `99.SOURCE\SM\USER\WYNLAB.SM.USER.sln`), 그 결과
DLL(+pdb)을 서버의 해당 모듈 폴더로 옮긴다. 예:

```
99.SOURCE\SM\USER\WYNLAB.SM.USER\bin\Release\net48\WYNLAB.SM.USER.dll
  -> D:\WYNLAB\Modules\SM\WYNLAB.SM.USER.dll        (운영)
  -> D:\WYNLAB_TEST\Modules\SM\WYNLAB.SM.USER.dll   (개발, 테스트용으로 먼저 여기 배포하고 확인 후 운영에 반영하는 흐름 추천)
```

같은 방식으로 `USERGROUP`/`MENU`/`USERAUTH`도 각각 `Modules\SM\`에 넣으면 된다. (전부 SM
모듈이라 지금은 SM 폴더 하나만 쓰지만, BA/SA/PR/MA 화면이 생기면 그 모듈 폴더에 넣으면 됨 -
`ModuleLoader`가 하위 폴더까지 재귀적으로 스캔하도록 이미 고쳐뒀다.)

### 7-4. 클라이언트(WYNLAB.exe) ClickOnce 게시

Visual Studio에서 `WYNLAB.Shell` 프로젝트 우클릭 → "게시(Publish)" → 게시 프로필로 `Dev` 또는
`Prod` 선택 → 게시. 그러면 VS가 `PublishUrl`(`http://115.23.220.115:8092/` 또는 `:8093/`)로
직접 업로드를 시도하는데, 이건 그 URL이 실제로 쓰기 가능한 게시 지점으로 열려있어야 동작한다
(IIS의 웹 배포 게시 또는 FTP) - 5번에서 만든 사이트는 "정적 파일 서빙"만 되는 평범한 사이트라
VS가 직접 업로드하지 못할 수 있다. 그 경우엔:

1. 게시 프로필을 "폴더" 방식으로 로컬에 먼저 게시(`dotnet publish` 또는 VS의 폴더 프로필)
2. 그 결과물(`.application`, `.exe.manifest`, `Application Files\` 폴더 등)을 서버의
   `D:\WYNLAB\ClickOnce\`(또는 `D:\WYNLAB_TEST\ClickOnce\`)로 통째로 복사

둘 중 뭐가 더 편할지, 그리고 VS가 직접 업로드하는 방식을 쓰고 싶으면(웹 배포/FTP 세팅 추가
필요) 여기서 막히면 바로 물어봐줘 - 상황 보고 같이 정하면 된다.

---

## 8. 검증 체크리스트

- [ ] `http://115.23.220.115:8091/`, `:8090/` 접속 시 API가 응답(404여도 괜찮음 - "연결 자체"가 되는지가 핵심. 개발서버는 `:8090/swagger`로도 확인)
- [ ] `\\115.23.220.115\WYNLAB\Modules\SM\`, `\\115.23.220.115\WYNLAB_TEST\Modules\SM\` 둘 다 다른 PC 탐색기에서 보임
- [ ] 서버에서 `iisreset` 후에도 4개 사이트가 다시 정상 기동(자동 시작)
- [ ] 화면 DLL을 `Modules\SM\`에 넣고 클라이언트로 로그인(개발/운영 환경 각각) → 메뉴 클릭 시 화면이 뜸
- [ ] ClickOnce 설치 URL(`:8092`/`:8093`)로 브라우저 접속 시 설치 페이지가 뜸

막히는 단계가 있으면 그 번호(예: "5번에서 New-Website 실행했는데 이런 에러 났어")로 알려주면
바로 짚어줄게.

---

## 참고: 나중에 SSL/HTTPS

지금은 HTTP로 먼저 구성한다. SSL은 [SECURITY.md](SECURITY.md)의 "HTTPS" 항목에 정리된 백로그
항목이니, 실제 서비스 오픈 전에 그쪽을 보고 진행하면 된다.
