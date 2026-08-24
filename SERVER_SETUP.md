# 원격 서버(192.168.160.10) 세팅 가이드

목표: 로컬 PC에서 API를 직접 켜서 테스트할 필요 없이, 서버(`192.168.160.10`)에서 항상 떠 있는
구조로 만든다. 클라이언트(`WYNLAB.exe`)만 실행하면 되고, API는 IIS가 알아서 계속 띄워둔다.

이 문서는 제가 직접 그 서버에 접속할 수 없어서, 서버 콘솔(RDP)에서 직접 실행할 명령과 설정을
순서대로 정리한 것이다. 막히는 단계 번호를 말해주면 그 부분만 다시 봐줄 수 있다.

지금은 **운영(WYNLAB)만** 만든다. 개발/테스트 환경(`WYNLAB_TEST`)은 나중에 필요해지면 같은
방식으로 추가하면 되고, 지금은 손대지 않는다.

.NET 8 Hosting Bundle은 이미 설치되어 있는 걸 확인했다(8.0.30, IIS에 정상 등록됨) - 그 단계는
건너뛴다.

**IP 관련**: KT 회선에서 외부 포트가 열려있지 않아서, 외부 IP(`115.23.220.115`)로는 당장 접속
테스트가 안 된다. 그래서 우선 서버 내부 IP(`192.168.160.10`)를 기준으로 진행하고, 클라이언트도
서버 자체(또는 같은 내부망의 다른 PC)에 설치해서 테스트한다. 외부 접속(KT 포트포워딩)은 별도로
해결되면 그때 외부 IP로 다시 바꾸면 된다 - 클라이언트 설정(`appsettings.*.json`, `*.pubxml`)은
이미 내부 IP로 맞춰뒀다.

---

## 0. 최종 그림

```
\\192.168.160.10\WYNLAB          <- 운영 공유 루트

D:\WYNLAB\
  Api\        -> IIS 사이트 "WYNLAB-Api",      포트 8090   (WYNLAB.Api, Release 게시물)
  ClickOnce\  -> IIS 사이트 "WYNLAB-ClickOnce", 포트 8091   (WYNLAB.Shell ClickOnce, Release)
  Modules\    -> \\192.168.160.10\WYNLAB\Modules로 노출
    SM\  BA\  SA\  PR\  MA\             <- 화면 DLL을 모듈별로 여기 배포
  CoreAssembly\ -> \\192.168.160.10\WYNLAB\CoreAssembly로 노출
    WYNLAB.BaseForm.dll, WYNLAB.Shared.dll, WYNLAB.Controls.dll, manifest.json  <- 6-5번 참고
```

포트는 8090(API)부터 순차로: **API=8090, ClickOnce=8091**. 나중에 개발환경을 추가하게 되면
이어서 8092(API)/8093(ClickOnce)를 쓰면 된다(지금은 안 만듦).

클라이언트 설정(`appsettings.Dev.json`/`Prod.json`)과 게시 프로필(`Prod.pubxml`)은 이미 이
포트에 맞춰뒀다:
- Production 환경 → `ApiBaseUrl: http://192.168.160.10:8090/`, `ModulesPath: \\192.168.160.10\WYNLAB\Modules`
- ClickOnce 게시 URL(`Prod.pubxml`) → `http://192.168.160.10:8091/`

---

## 1. 폴더 만들기

```powershell
New-Item -ItemType Directory -Force -Path `
  "D:\WYNLAB\Api", "D:\WYNLAB\ClickOnce", `
  "D:\WYNLAB\Modules\SM", "D:\WYNLAB\Modules\BA", "D:\WYNLAB\Modules\SA", "D:\WYNLAB\Modules\PR", "D:\WYNLAB\Modules\MA"
```

---

## 2. 네트워크 공유

클라이언트(각 직원 PC)가 `\\192.168.160.10\WYNLAB\Modules\...` 경로로 화면 DLL을 읽어가야
하므로, `D:\WYNLAB`을 `WYNLAB`이라는 이름으로 공유한다.

```powershell
New-SmbShare -Name "WYNLAB" -Path "D:\WYNLAB" -FullAccess "Administrators" -ReadAccess "Everyone"
```

- 사내망이 도메인 환경이면 `-ReadAccess "Everyone"` 대신 `"Domain Users"` 등으로 좁히는 걸 권장.
- 배포(=DLL 갱신)는 서버에 직접 로그인해서 파일을 복사하는 방식으로 할 거라 쓰기 권한은
  Administrators만 있으면 된다.

확인: 다른 PC에서 탐색기 주소창에 `\\192.168.160.10\WYNLAB\Modules` 입력해서 폴더가 보이는지 체크.

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

확인: 브라우저로 `http://192.168.160.10:8090/` 접속해서 응답 오는지 확인(Production 환경이라
Swagger는 꺼져있는 게 정상 - `Program.cs` 참고).

**중요 - 모듈 DLL이 WYNLAB.Shared에 새 타입을 추가했다면 Shell도 같이 재게시해야 한다**:
Shell(ClickOnce)은 `WYNLAB.Shared.dll`/`WYNLAB.BaseForm.dll`/`WYNLAB.Controls.dll`을 자기 패키지 안에 번들해서
배포한다. 화면 모듈(`WYNLAB.SM.*.dll`)이 `WYNLAB.Shared`에 새로 추가된 DTO 등을 참조하는데
Shell의 ClickOnce 패키지 안 `WYNLAB.Shared.dll`이 그보다 예전 버전이면, 두 어셈블리 이름·버전이
동일해서(`WYNLAB.Shared, Version=1.0.0.0`) .NET이 "이미 로드된 그 어셈블리"로 취급해버리고 -
그 안엔 새 타입이 없으니 화면을 열 때 `System.TypeLoadException`이 난다(실제로 겪음). 그래서
새 화면이 `WYNLAB.Shared`에 새 DTO를 추가했다면, 모듈 DLL만 `Modules\`에 올리는 걸로 끝내지 말고
**6-4의 Shell 재게시도 항상 같이** 해야 한다.

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

**화면이 SvgIcon처럼 문자열 아닌 리소스를 쓰면 `System.Resources.Extensions.dll`도 필요하다**:
해당 화면의 `bin\Release\net48\`에 이 dll이 같이 생기는데(csproj의
`GenerateResourceUsePreserializedResources` 옵션 때문), 이게 없으면 그 화면을 열 때
`FileNotFoundException`으로 죽는다(실제로 겪음 - 기초코드등록/`WYNLAB.SM.CODE` 최초 배포 때
이 dll을 안 챙겨서 발생). Shell.exe는 이 dll을 몰라서(참조 안 함) CoreAssembly 대상은 아니고,
마이크로소프트가 배포하는 고정 버전 패키지라 자주 안 바뀌므로 화면마다 매번 챙기지 않아도
되게 **`Modules\SM\`이 아니라 `Modules\` 바로 밑에 한 벌만** 두면 된다(재귀 스캔 대상이라
이 위치도 정상 로드됨) - 앞으로 SvgIcon 쓰는 화면이 늘어도 이 파일 하나로 전부 해결된다.

```
D:\WYNLAB\Modules\System.Resources.Extensions.dll   <- 화면 폴더가 아니라 여기 한 번만
```

### 6-4. 클라이언트(WYNLAB.exe) ClickOnce 게시

**VS의 "게시(Publish)" 메뉴로는 안 된다** - `WYNLAB.Shell`이 SDK 스타일 .NET Framework
프로젝트라서, VS의 새 게시 화면(우클릭 → 게시 → "+ 새 프로필")에는 ClickOnce 옵션 자체가
없다(.NET 5+ 프로젝트에만 지원됨, "ClickOnce 게시" 개별 구성요소가 설치되어 있어도 마찬가지).
대신 `MSBuild.exe`로 직접 게시해야 한다.

**중요 - 반드시 아래 순서 그대로(그냥 `/t:Publish`만 실행하면 클라이언트가 업데이트를
인식 못 함, 아래 "왜 이렇게 해야 하는가" 참고):**

```powershell
$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"
Set-Location "D:\01. SOURCE\00. WYNLAB\01.Client\WYNLAB.Shell"

# 1) 이전 게시 결과물을 반드시 먼저 지운다 - 안 지우면 2)에서 아무것도 안 바뀐 것처럼 보인다
Remove-Item -Recurse -Force "bin\Release\net48\publish" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "bin\Release\net48\app.publish" -ErrorAction SilentlyContinue
Remove-Item -Force "obj\Release\net48\WYNLAB.exe.manifest" -ErrorAction SilentlyContinue
Remove-Item -Force "obj\Release\net48\WYNLAB.application" -ErrorAction SilentlyContinue

# 2) 매번 새 버전 번호를 명시적으로 준다 - 안 주면 항상 1.0.0.0으로 게시되어 이미 설치된
#    클라이언트가 "업데이트 없음"으로 판단하고 예전 버전을 계속 실행한다
$build = [int]((Get-Date) - (Get-Date "2024-01-01")).TotalDays
$revision = [int](Get-Date).TimeOfDay.TotalMinutes
& $msbuild WYNLAB.Shell.csproj /t:Publish /p:PublishProfile=ProdLocal /p:Configuration=Release /p:ApplicationVersion="1.0.$build.$revision"
```

결과물은 `01.Client\WYNLAB.Shell\bin\Release\net48\publish\`에 나온다
(`WYNLAB.application` + `Application Files\` + `WYNLAB.exe`. 바로 옆에 생기는
`app.publish\`는 일반 파일복사 결과물이라 ClickOnce와 무관 - 무시). 이 `publish\` 폴더 안
내용물 전체를 서버의 `D:\WYNLAB\ClickOnce\`로 복사하면 된다.

**왜 이렇게 해야 하는가(실제로 겪은 문제)**:
- `GenerateApplicationManifest`/`GenerateDeploymentManifest`는 MSBuild의 증분빌드
  대상이라, 이전에 한 번이라도 게시해서 `bin\Release\net48\publish\`나
  `obj\Release\net48\WYNLAB.application`이 이미 존재하면 "출력이 이미 최신"이라고 판단해서
  **아무 것도 바뀐 게 없어도, 소스 코드를 아무리 고쳐도 매니페스트 자체를 다시 안 만들고
  건너뛴다.** 실제로 코드를 여러 군데 고친 뒤 그냥 `/t:Publish`만 다시 돌렸더니 서버에 새로
  복사해도 클라이언트에 아무 변화가 없었던 사례가 있었다 - 원인이 바로 이 증분빌드 스킵이었다.
  그래서 매번 게시 전에 이전 결과물을 지우는 게 필수다.
- `pubxml`의 `<ApplicationVersion>1.0.0.*</ApplicationVersion>`에 있는 `*`는 VS의 게시
  마법사(GUI)에서만 자동으로 리비전 번호로 치환된다 - CLI로 `MSBuild.exe`를 직접 호출하면
  `*`가 그냥 무시되고 리비전이 계속 0으로 고정되어 **매번 정확히 같은 버전(`1.0.0.0`)**으로
  게시된다. 이미 그 버전이 설치되어 있는 클라이언트는 서버 파일이 실제로 바뀌었어도 "버전이
  같으니 업데이트 없음"이라고 판단해서 예전 그대로 실행한다 - 이게 "재배포했는데 화면이 하나도
  안 바뀐 것 같다"는 증상의 실제 원인이었다. 그래서 `/p:ApplicationVersion`을 매번 명시적으로
  다른 값으로 줘야 한다(위 스크립트의 `$build`/`$revision` 계산 - Build는 날짜 기준이라
  거의 매일 커지고, Revision은 그날 자정부터의 분(分)이라 하루 안에서도 게시할 때마다 커진다).
- `ProdLocal.pubxml`에 `PublishProtocol=ClickOnce`/`GenerateManifests`/`SignManifests`를
  명시적으로 넣어둔 것도 핵심이다 - 이게 없으면 CLI에서 조용히 일반 파일복사 게시로 빠져버려서
  `.application` 매니페스트 자체가 안 생긴다. `WYNLAB.application` 안의 업데이트 확인 주소는
  `InstallUrl`(`http://192.168.160.10:8091/`) 그대로 정확히 반영된다 - 로컬에 게시해도
  클라이언트는 정상적으로 서버로 업데이트를 확인하러 간다.

게시 후 확인: `Get-Content "bin\Release\net48\publish\WYNLAB.application" | Select-String 'version='`
로 방금 계산한 `$build`/`$revision`값이 실제로 들어갔는지 확인하고 복사하면 안전하다.

**`publish.htm`(설치 안내 페이지)은 생성되지 않는다** - VS 게시 마법사가 만들어주는 건데 CLI
게시에는 없다. 그래서 브라우저로 접속할 때 루트 주소(`http://192.168.160.10:8091/`)가 아니라
**`http://192.168.160.10:8091/WYNLAB.application`**로 직접 접속해야 설치가 시작된다(또는
5번처럼 `WYNLAB.application`을 IIS 기본 문서로 등록해두면 루트 주소로도 됨).

### 6-5. 프레임워크(WYNLAB.BaseForm.dll/WYNLAB.Shared.dll/WYNLAB.Controls.dll) 배포 - ClickOnce 재게시 없이

`WYNLAB.BaseForm.dll`/`WYNLAB.Shared.dll`/`WYNLAB.Controls.dll`은 이제 ClickOnce 안에 번들되는 것과 별개로,
`CoreAssembly\` 공유폴더에도 배포해두면 클라이언트가 **로그인 시(정확히는 실행 시작 시)
자동으로 버전을 비교해서 다르면 스스로 받아간다** - Shell.exe를 ClickOnce로 매번 재게시하지
않아도 된다. 원리와 왜 이렇게 만들었는지는 `01.Client\WYNLAB.Bootstrap\WYNLAB.Bootstrap.csproj`
주석 참고(요약: 화면 모듈처럼 리플렉션 로드가 아니라 컴파일타임에 직접 참조되는 dll이라, 로드되기
"전"에 교체해야 해서 별도의 최소 프로젝트로 분리함).

```powershell
# 1) 프레임워크만 새로 빌드
cd "D:\01. SOURCE\00. WYNLAB\01.Client\WYNLAB.BaseForm"
dotnet build -c Release

# 2) 결과물을 서버 CoreAssembly 폴더에 복사
#    (bin\Release\net48\WYNLAB.BaseForm.dll, 그리고 같은 방식으로 WYNLAB.Shared/WYNLAB.Controls도)
```
`D:\WYNLAB\CoreAssembly\`에 `WYNLAB.BaseForm.dll`/`WYNLAB.Shared.dll`/`WYNLAB.Controls.dll`을 복사해 넣은 다음,
**반드시** 매니페스트를 다시 생성해야 클라이언트가 변경을 인식한다(사람이 버전을 손으로 안 적고
파일 해시를 자동 계산 - 깜빡할 일이 없게):

```powershell
# 서버 콘솔(RDP)에서 D:\WYNLAB\CoreAssembly 폴더를 대상으로 직접 실행.
# 스크립트 자체는 저장소의 01.Client\WYNLAB.Bootstrap\Generate-CoreAssemblyManifest.ps1를
# 서버로 복사해두고 쓰면 된다(-TargetDir로 대상 폴더만 넘기면 됨).
.\Generate-CoreAssemblyManifest.ps1 -TargetDir "D:\WYNLAB\CoreAssembly"
```
`manifest.json`이 새로 생기면 끝 - 사용자는 다음 실행 때 자동으로 새 버전을 받는다. 실행 중인
다른 WYNLAB 창이 있으면(파일이 잠겨서 교체 불가) 안내 메시지가 뜨고 프로그램이 종료되니, 모든
창을 닫고 다시 실행하라고 안내하면 된다.

**최초 1회는 `WYNLAB.BaseForm.dll`/`WYNLAB.Shared.dll`/`WYNLAB.Controls.dll`이 이미 ClickOnce 안에 들어있으므로
`CoreAssembly\` 폴더가 비어있어도(또는 `manifest.json`이 없어도) 문제없이 조용히 건너뛴다** -
이 자동갱신 구조를 준비 없이 그냥 배포해도 기존 클라이언트가 깨지지 않는다.

---

## 7. 검증 체크리스트

- [ ] `http://192.168.160.10:8090/` 접속 시 API가 응답(404여도 괜찮음 - "연결 자체"가 되는지가 핵심)
- [ ] `\\192.168.160.10\WYNLAB\Modules\SM\` 폴더가 다른 PC 탐색기에서 보임
- [ ] 서버에서 `iisreset` 후에도 2개 사이트가 다시 정상 기동(자동 시작)
- [ ] 화면 DLL을 `Modules\SM\`에 넣고 클라이언트(운영 환경)로 로그인 → 메뉴 클릭 시 화면이 뜸
- [ ] ClickOnce 설치 URL(`http://192.168.160.10:8091/WYNLAB.application`)로 브라우저 접속 시 설치가 시작됨

막히는 단계가 있으면 그 번호(예: "4번에서 New-Website 실행했는데 이런 에러 났어")로 알려주면
바로 짚어줄게.

---

## 참고

- **SSL/HTTPS**: 지금은 HTTP로 먼저 구성한다. [SECURITY.md](SECURITY.md)의 "HTTPS" 항목에
  정리된 백로그니, 실제 서비스 오픈 전에 그쪽을 보고 진행하면 된다.
- **개발/테스트 환경(WYNLAB_TEST)**: 나중에 필요해지면 위 1~6번을 그대로 반복하되 경로/사이트
  이름/포트만 `WYNLAB_TEST`/`8092`(API)/`8093`(ClickOnce)로 바꾸면 된다. 클라이언트의
  `Development` 환경 항목이 이미 그 경로(`\\192.168.160.10\WYNLAB_TEST\Modules`,
  `:8092`)를 가리키도록 세팅되어 있어서, 서버에 실제로 만들고 나면 클라이언트에서 환경 전환만
  하면 바로 붙는다.
