# 원격 서버(115.23.220.115) 세팅 가이드

목표: 로컬 PC에서 API를 직접 켜서 테스트할 필요 없이, 서버(`115.23.220.115`)에서 항상 떠 있는
구조로 만든다. 클라이언트(`WYNLAB.exe`)만 실행하면 되고, API는 IIS가 알아서 계속 띄워둔다.

이 문서는 제가 직접 그 서버에 접속할 수 없어서, 서버 콘솔(RDP)에서 직접 실행할 명령과 설정을
순서대로 정리한 것이다. 막히는 단계 번호를 말해주면 그 부분만 다시 봐줄 수 있다.

지금은 **운영(WYNLAB)만** 만든다. 개발/테스트 환경(`WYNLAB_TEST`)은 나중에 필요해지면 같은
방식으로 추가하면 되고, 지금은 손대지 않는다.

.NET 8 Hosting Bundle은 이미 설치되어 있는 걸 확인했다(8.0.30, IIS에 정상 등록됨) - 그 단계는
건너뛴다.

---

## 0. 최종 그림

```
\\115.23.220.115\WYNLAB          <- 운영 공유 루트

D:\WYNLAB\
  Api\        -> IIS 사이트 "WYNLAB-Api",      포트 8090   (WYNLAB.Api, Release 게시물)
  ClickOnce\  -> IIS 사이트 "WYNLAB-ClickOnce", 포트 8091   (WYNLAB.Shell ClickOnce, Release)
  Modules\    -> \\115.23.220.115\WYNLAB\Modules로 노출
    SM\  BA\  SA\  PR\  MA\             <- 화면 DLL을 모듈별로 여기 배포
```

포트는 8090(API)부터 순차로: **API=8090, ClickOnce=8091**. 나중에 개발환경을 추가하게 되면
이어서 8092(API)/8093(ClickOnce)를 쓰면 된다(지금은 안 만듦).

클라이언트 설정(`appsettings.Dev.json`/`Prod.json`)과 게시 프로필(`Prod.pubxml`)은 이미 이
포트에 맞춰뒀다:
- Production 환경 → `ApiBaseUrl: http://115.23.220.115:8090/`, `ModulesPath: \\115.23.220.115\WYNLAB\Modules`
- ClickOnce 게시 URL(`Prod.pubxml`) → `http://115.23.220.115:8091/`

---

## 1. 폴더 만들기

```powershell
New-Item -ItemType Directory -Force -Path `
  "D:\WYNLAB\Api", "D:\WYNLAB\ClickOnce", `
  "D:\WYNLAB\Modules\SM", "D:\WYNLAB\Modules\BA", "D:\WYNLAB\Modules\SA", "D:\WYNLAB\Modules\PR", "D:\WYNLAB\Modules\MA"
```

---

## 2. 네트워크 공유

클라이언트(각 직원 PC)가 `\\115.23.220.115\WYNLAB\Modules\...` 경로로 화면 DLL을 읽어가야
하므로, `D:\WYNLAB`을 `WYNLAB`이라는 이름으로 공유한다.

```powershell
New-SmbShare -Name "WYNLAB" -Path "D:\WYNLAB" -FullAccess "Administrators" -ReadAccess "Everyone"
```

- 사내망이 도메인 환경이면 `-ReadAccess "Everyone"` 대신 `"Domain Users"` 등으로 좁히는 걸 권장.
- 배포(=DLL 갱신)는 서버에 직접 로그인해서 파일을 복사하는 방식으로 할 거라 쓰기 권한은
  Administrators만 있으면 된다.

확인: 다른 PC에서 탐색기 주소창에 `\\115.23.220.115\WYNLAB\Modules` 입력해서 폴더가 보이는지 체크.

---

## 3. 방화벽 규칙

```powershell
New-NetFirewallRule -DisplayName "WYNLAB API (8090)"      -Direction Inbound -LocalPort 8090 -Protocol TCP -Action Allow
New-NetFirewallRule -DisplayName "WYNLAB ClickOnce (8091)" -Direction Inbound -LocalPort 8091 -Protocol TCP -Action Allow
```

(파일공유용 SMB 445 포트는 보통 이미 열려있지만, 안 열려있으면
`New-NetFirewallRule -DisplayName "SMB" -Direction Inbound -LocalPort 445 -Protocol TCP -Action Allow`.
회사 자체 방화벽 장비(라우터/UTM 등)가 별도로 있다면 그쪽에서도 이 포트를 열어줘야 사내 다른
PC에서 접속 가능하다.)

시작 전에 한 번 더 확인하고 싶으면:
```powershell
Get-NetTCPConnection -State Listen | Where-Object { $_.LocalPort -in 8090,8091 }
```
아무 결과도 안 나오면(빈 화면) 안전하게 비어있는 포트다.

---

## 4. IIS 앱풀 + 사이트

```powershell
Import-Module WebAdministration

# --- API - "관리되는 코드 없음" 필수 (ASP.NET Core는 자체 프로세스로 동작, IIS는 리버스 프록시 역할) ---
New-WebAppPool -Name "WYNLAB-Api"
Set-ItemProperty "IIS:\AppPools\WYNLAB-Api" -Name managedRuntimeVersion -Value ""
New-Website -Name "WYNLAB-Api" -PhysicalPath "D:\WYNLAB\Api" -ApplicationPool "WYNLAB-Api" -Port 8090

# --- ClickOnce - 정적 파일만 서빙하므로 기본값 그대로 ---
New-WebAppPool -Name "WYNLAB-ClickOnce"
New-Website -Name "WYNLAB-ClickOnce" -PhysicalPath "D:\WYNLAB\ClickOnce" -ApplicationPool "WYNLAB-ClickOnce" -Port 8091
```

**주의**: 이 2개 사이트는 아직 폴더가 비어있어서 지금 브라우저로 접속하면 403/404가 뜨는 게
정상이다. 6번에서 실제로 게시물을 넣은 다음에 확인하면 된다.

GUI로 하고 싶으면: IIS 관리자 실행 → 왼쪽 트리에서 "사이트" 우클릭 → "웹 사이트 추가" →
사이트 이름/물리 경로/포트를 위 표대로 입력. API 사이트는 애플리케이션 풀 "고급 설정"에서
".NET CLR 버전"을 "관리 코드 없음"으로 지정(ClickOnce 사이트는 기본값 둬도 됨).

---

## 5. ClickOnce MIME 타입 등록 (빠뜨리기 쉬운 부분)

ClickOnce로 게시하면 `.application`, `.manifest`, `.deploy` 확장자 파일이 생기는데, IIS가
기본적으로 이 확장자를 모른다. 등록하지 않으면 사용자가 설치 링크를 클릭해도 다운로드가 깨진다.

```powershell
Add-WebConfigurationProperty -PSPath "IIS:\Sites\WYNLAB-ClickOnce" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".application"; mimeType="application/x-ms-application"}
Add-WebConfigurationProperty -PSPath "IIS:\Sites\WYNLAB-ClickOnce" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".manifest"; mimeType="application/x-ms-manifest"}
Add-WebConfigurationProperty -PSPath "IIS:\Sites\WYNLAB-ClickOnce" -Filter "system.webServer/staticContent" -Name "." -Value @{fileExtension=".deploy"; mimeType="application/octet-stream"}
```

이미 등록되어 있다는 오류가 나면 무시해도 된다(중복 등록 시도일 뿐).

---

## 6. 첫 배포

로컬 개발 PC(지금 이 리포가 있는 PC)에서 게시(publish)한 다음, 그 결과물을 서버의 해당 폴더로
복사하는 흐름이다.

### 6-1. WYNLAB.Api 게시

```bash
cd "D:\01. SOURCE\00. WYNLAB\02.Server\WYNLAB.Api"
dotnet publish -c Release -o "publish-prod"
```

`publish-prod` 폴더 전체 내용물을 서버의 `D:\WYNLAB\Api\`로 복사한다.

### 6-2. 시크릿을 web.config에 등록 (중요)

`appsettings.Production.json`엔 실제 DB 비밀번호·JWT 키를 넣지 않기로 했었다 - 대신 게시 폴더
안에 자동 생성된 `D:\WYNLAB\Api\web.config`를 열어서 `<aspNetCore>` 안의
`<environmentVariables>`에 직접 넣는다:

```xml
<aspNetCore processPath="dotnet" arguments=".\WYNLAB.Api.dll" ... hostingModel="inprocess">
  <environmentVariables>
    <environmentVariable name="Jwt__SecretKey" value="<운영용_JWT_키>" />
    <environmentVariable name="ConnectionStrings__WynlabDb" value="Server=tcp:<운영DB서버>,1433;Database=<운영DB명>;User Id=<계정>;Password=<비밀번호>;TrustServerCertificate=True;" />
  </environmentVariables>
</aspNetCore>
```

운영 JWT 키는 이전에 새로 생성해둔 값을 쓰면 된다(대화에서 이미 전달함).

수정 후 앱풀 재시작:
```powershell
Restart-WebAppPool -Name "WYNLAB-Api"
```

확인: 브라우저로 `http://115.23.220.115:8090/` 접속해서 응답 오는지 확인(Production 환경이라
Swagger는 꺼져있는 게 정상 - `Program.cs` 참고).

### 6-3. 화면(모듈) DLL 배포

각 화면 프로젝트를 Release로 빌드하면(예: `99.SOURCE\SM\USER\WYNLAB.SM.USER.sln`), 그 결과
DLL(+pdb)을 서버의 해당 모듈 폴더로 옮긴다. 예:

```
99.SOURCE\SM\USER\WYNLAB.SM.USER\bin\Release\net48\WYNLAB.SM.USER.dll
  -> D:\WYNLAB\Modules\SM\WYNLAB.SM.USER.dll
```

같은 방식으로 `USERGROUP`/`MENU`/`USERAUTH`도 각각 `Modules\SM\`에 넣으면 된다. (전부 SM
모듈이라 지금은 SM 폴더 하나만 쓰지만, BA/SA/PR/MA 화면이 생기면 그 모듈 폴더에 넣으면 됨 -
`ModuleLoader`가 하위 폴더까지 재귀적으로 스캔하도록 이미 고쳐뒀다.)

### 6-4. 클라이언트(WYNLAB.exe) ClickOnce 게시

Visual Studio에서 `WYNLAB.Shell` 프로젝트 우클릭 → "게시(Publish)" → `Prod` 프로필 선택 →
게시. VS가 `PublishUrl`(`http://115.23.220.115:8091/`)로 직접 업로드를 시도하는데, 이건 그
URL이 실제로 쓰기 가능한 게시 지점으로 열려있어야 동작한다(IIS의 웹 배포 게시 또는 FTP) - 4번에서
만든 사이트는 "정적 파일 서빙"만 되는 평범한 사이트라 VS가 직접 업로드하지 못할 수 있다. 그
경우엔:

1. 게시 프로필을 "폴더" 방식으로 로컬에 먼저 게시(`dotnet publish` 또는 VS의 폴더 프로필)
2. 그 결과물(`.application`, `.exe.manifest`, `Application Files\` 폴더 등)을 서버의
   `D:\WYNLAB\ClickOnce\`로 통째로 복사

둘 중 뭐가 더 편할지, 그리고 VS가 직접 업로드하는 방식을 쓰고 싶으면(웹 배포/FTP 세팅 추가
필요) 여기서 막히면 바로 물어봐줘 - 상황 보고 같이 정하면 된다.

---

## 7. 검증 체크리스트

- [ ] `http://115.23.220.115:8090/` 접속 시 API가 응답(404여도 괜찮음 - "연결 자체"가 되는지가 핵심)
- [ ] `\\115.23.220.115\WYNLAB\Modules\SM\` 폴더가 다른 PC 탐색기에서 보임
- [ ] 서버에서 `iisreset` 후에도 2개 사이트가 다시 정상 기동(자동 시작)
- [ ] 화면 DLL을 `Modules\SM\`에 넣고 클라이언트(운영 환경)로 로그인 → 메뉴 클릭 시 화면이 뜸
- [ ] ClickOnce 설치 URL(`:8091`)로 브라우저 접속 시 설치 페이지가 뜸

막히는 단계가 있으면 그 번호(예: "4번에서 New-Website 실행했는데 이런 에러 났어")로 알려주면
바로 짚어줄게.

---

## 참고

- **SSL/HTTPS**: 지금은 HTTP로 먼저 구성한다. [SECURITY.md](SECURITY.md)의 "HTTPS" 항목에
  정리된 백로그니, 실제 서비스 오픈 전에 그쪽을 보고 진행하면 된다.
- **개발/테스트 환경(WYNLAB_TEST)**: 나중에 필요해지면 위 1~6번을 그대로 반복하되 경로/사이트
  이름/포트만 `WYNLAB_TEST`/`8092`(API)/`8093`(ClickOnce)로 바꾸면 된다. 클라이언트의
  `Development` 환경 항목이 이미 그 경로(`\\115.23.220.115\WYNLAB_TEST\Modules`,
  `:8092`)를 가리키도록 세팅되어 있어서, 서버에 실제로 만들고 나면 클라이언트에서 환경 전환만
  하면 바로 붙는다.
