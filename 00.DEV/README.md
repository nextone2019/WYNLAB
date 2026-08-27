# WYN LAB — 7단계 (MDI 셸 디자인 개선 + 서버 실시간 전환)

## 이번 변경
- **시안 A(다크 헤더 + 세그먼트 툴바) 적용** — `ShellForm.cs` 전면 재구성
- **툴바 색상 회사별 커스터마이징** — `appsettings.json`의 `ToolbarColor` 값 하나로 전체 헤더 배색 결정. `ColorHelper`가 배경 밝기를 계산해서 아이콘/글자색(흰색/짙은회색)을 자동 선택하므로 어떤 색을 넣어도 항상 잘 보임
- **로고 클릭 시 좌측 메뉴 토글** — 로고 영역 클릭할 때마다 Accordion 메뉴 표시/숨김
- **로그인 후에도 서버(개발/운영) 실시간 전환 가능** — 툴바 우측 콤보박스. 전환 시 확인창 → 열린 화면 전부 닫힘 → 재로그인 필요 (다른 서버는 다른 DB라 세션이 무효화되기 때문)
  - `appsettings.json` 구조 변경: `Environments`(dev/prod 주소 둘 다 포함) + `DefaultEnvironment`(시작 시 기본값)로 재구성
- **사용자등록 화면(`UserEditForm`) UI 개선** — `LayoutControl`로 계정정보/인적사항/연락처/권한 4개 그룹으로 정리, 헤더/푸터 바 추가, 저장버튼 강조색 적용

## appsettings.json 새 구조
```json
{
  "Environments": {
    "Development": { "ApiBaseUrl": "http://서버주소:8090/" },
    "Production": { "ApiBaseUrl": "http://서버주소:8091/" }
  },
  "DefaultEnvironment": "Development",
  "ToolbarColor": "#1B2A3D",
  "AppVersion": "1.0.0"
}
```
회사별 배포판을 만들 때 `ToolbarColor`만 바꾸면 툴바 색이 전부 바뀝니다.

## 이전 단계 기록

## 이번 변경 - 화면별 개별 버튼 → Shell 상단 공통 툴바
- **`ShellForm` 상단에 조회/입력/저장/삭제/출력 5개 아이콘 툴바 추가** — 화면마다 버튼을 따로 만들지 않음
- 아이콘은 이미지 파일 없이 **코드(GDI+)로 직접 그림** (`ToolbarIcons.cs`) — 배포시 파일 누락 위험 없음
- 클릭하면 **현재 활성 MDI 자식폼(`ActiveMdiChild`)**의 `QueryAsync`/`NewAsync`/`SaveAsync`/`DeleteAsync`/`PrintAsync`(전부 `BaseForm`에 정의된 가상 메서드)를 호출
- `BaseGridForm`에서 개별 툴바(버튼 5개)는 제거 — 그리드 골격만 남김
- `UserListForm`도 이 체계에 맞춰 재정리 (화면 열리면 자동 조회, 더블클릭은 그리드 자체 동작으로 유지)

## 새 화면 만들 때 패턴 (중요 - 앞으로 계속 이렇게 감)
```csharp
public class XxxListForm : BaseGridForm
{
    public XxxListForm()
    {
        Text = "화면명";
        MenuCd = "메뉴코드";
        Load += async (s, e) => await QueryAsync();
    }

    public override async Task QueryAsync() { /* 조회 API 호출 + 그리드 바인딩 */ }
    public override async Task NewAsync()   { /* 등록 팝업 오픈 */ }
    public override async Task DeleteAsync(){ /* 삭제 API 호출 */ }
    // SaveAsync, PrintAsync은 필요한 경우만 override (PrintAsync 기본값은 엑셀출력)
}
```

## 이전 단계 기록

## 구조 정리 (이번에 중요하게 바뀐 것)
- **`ApiClient`, `AppConfig`를 `Shell` → `UI.Common`으로 이동** — 이제 어떤 업무모듈(DLL)에서도 API를 호출할 수 있음
- **`BaseGridForm`에 표준 CRUD 툴바 추가** — 조회/신규/수정/삭제/엑셀 버튼 + `OnSearchClickAsync` 등 4개 가상 메서드만 override하면 목록형 화면 하나가 완성되는 패턴 확립
- **첫 업무모듈 프로젝트 `WYNLAB.Modules.System` 생성** — 사용자관리 화면(`UserListForm`, `UserEditForm`)이 여기 위치. 앞으로 모듈(영업관리, 생산관리 등)이 늘어날 때마다 이런 프로젝트를 하나씩 추가하면 됨

## 사용자관리 화면
- `UserListForm` (`BaseGridForm` 상속) — 목록조회, 더블클릭으로 수정팝업, 삭제(소프트삭제: USE_YN='N')
- `UserEditForm` — 신규등록/수정 겸용 팝업
- 서버: `UsersController` (`GET/POST/PUT/DELETE /api/users`) — **[Authorize] 필수**, 첫 인증 필요 API
- `004_UserManage_Menu.sql` — `TSMMENU`에 화면 등록 (FORM_CLASS_NM은 어셈블리 정규화 이름으로 작성해야 함)

## 지금 바로 하실 것 (사장님)
1. `004_UserManage_Menu.sql`을 개발 DB에 실행
2. 압축본 받아서 개발 PC 프로젝트 갱신 (새 프로젝트 `WYNLAB.Modules.System`이 추가됐어요 — **솔루션을 완전히 닫았다가 다시 열어야** 인식됩니다)
3. Shell을 다시 게시 → 서버에 배포 → 로그인 후 좌측 메뉴에서 "사용자관리" 클릭

## DevExpress 참조 관련 안내
- 새 프로젝트를 만들 때마다 **csproj 파일에 DevExpress 참조(HintPath)를 미리 넣어서 드립니다.** Visual Studio에서 직접 "참조 추가"를 다시 하실 필요 없습니다.
- 단, 참조 경로가 `C:\Program Files (x86)\DevExpress 20.2\Components\Bin\Framework\` 로 고정되어 있어서, **이 경로가 다른 PC/서버에서는 다를 경우에만** 수동 조정이 필요합니다. 지금까지는 사장님 PC 기준으로 확인된 경로라 문제없이 인식될 겁니다.해서 확인

## 이전 단계 기록

## ⚠️ 중요: 클라이언트 타겟 프레임워크 변경
DevExpress 20.2는 .NET 6/7/8을 지원하지 않아(공식 지원은 v23.1부터), 클라이언트를 **.NET Framework 4.8**로 전환했습니다.

| 프로젝트 | 타겟 프레임워크 | 비고 |
|---|---|---|
| `WYNLAB.Shell` | `net48` | .NET Framework 4.8 필요 |
| `WYNLAB.UI.Common` | `net48` | 동일 |
| `WYNLAB.Shared` | `netstandard2.0` | net48 클라이언트 / net8.0 서버 양쪽에서 참조 가능한 공통 타겟 |
| `WYNLAB.Api` | `net8.0` | **변경 없음** - DevExpress와 무관한 서버 프로젝트라 최신 .NET 유지 |

### 코드 변경 사항
- `Program.cs` : `ApplicationConfiguration.Initialize()`(net6+ 전용) → `Application.EnableVisualStyles()` + `Application.SetCompatibleTextRenderingDefault(false)`(classic 방식)로 교체
- `Shell.csproj` : `System.Text.Json` NuGet 패키지 추가 (.NET Framework엔 기본 내장 안 되어 있음)
- DevExpress 참조 방식 안내를 PackageReference 주석에서 **"로컬 설치 기반 참조"** 안내로 변경 (20.2는 NuGet 배포가 아님 - Visual Studio에서 참조 추가 시 DevExpress 항목에서 직접 선택)
- `ApplicationIcon` 설정은 실제 아이콘 파일(`app.ico`) 없으면 빌드 에러가 나서 일단 제거 — 준비되면 다시 추가

### DevExpress 20.2 참조 시 필요한 어셈블리 (Visual Studio > 참조 추가 > DevExpress v20.2 XX)
- `DevExpress.Data.v20.2`, `DevExpress.Utils.v20.2` (공통)
- `DevExpress.XtraEditors.v20.2` (Shell 로그인폼, UI.Common 공통)
- `DevExpress.XtraGrid.v20.2` (UI.Common - BaseGridForm)
- `DevExpress.XtraBars.v20.2`
- `DevExpress.XtraNavBar.v20.2` (AccordionControl)
- `DevExpress.XtraTabbedMdi.v20.2` (MDI 탭매니저)
- `DevExpress.XtraLayout.v20.2`

## 세션 구조 (신규)
- **`UI.Common/SessionManager.cs`** — 로그인 세션 싱글턴. `Shell`이 아닌 `UI.Common`에 위치해서 모든 업무모듈 DLL에서 접근 가능
- **`UI.Common/BaseForm.cs`** — `CurrentUserId`, `CurrentUserNm`, `CurrentEmpNo`, `CurrentDeptCd`, `CurrentDeptNm`, `CurrentPositionNm`, `CurrentIsAdmin` 속성 제공. 모든 업무화면에서 `this.CurrentUserId`처럼 바로 사용
- **`04.Database/003_Session_Procedure.sql`** — `SSP_WYNLAB_GetSession` 프로시저. 로그인 시 사용자 기본정보(결과셋1) + 소속그룹 목록(결과셋2)을 한 번에 반환
- 서버(`AuthService`)는 이제 개별 쿼리 대신 이 프로시저 하나만 호출해서 세션 데이터를 구성

## 포함된 것

### 클라이언트
- `01.Client/WYNLAB.Shell` : 실행 프로젝트 (로그인 → MDI 셸)
  - `Program.cs` / `LoginForm.cs`(실제 API 호출로 연동됨) / `ApiClient.cs`(HttpClient 래퍼)
  - `SessionManager.cs` : 로그인 세션/메뉴권한 캐시 (싱글턴)
  - `ShellForm.cs` : MDI 메인폼, Accordion 메뉴를 메뉴권한(MenuDto) 기준으로 동적 생성
- `01.Client/WYNLAB.UI.Common` : `BaseForm.cs`, `BaseGridForm.cs` (공통 베이스)

### 서버
- `02.Server/WYNLAB.Api` : ASP.NET Core Web API (.NET 8)
  - `Controllers/AuthController.cs` : `POST /api/auth/login`
  - `Services/AuthService.cs` : 로그인 인증 + 관리자 전체권한 처리 오케스트레이션
  - `Services/MenuPermissionMerger.cs` : **그룹(OR 합산) + 개인권한(우선 덮어쓰기)** 병합 로직
  - `Services/JwtTokenService.cs` : JWT 발급
  - `Repositories/UserRepository.cs`, `MenuRepository.cs` : Dapper 기반 조회
  - `Data/DapperContext.cs` : DB 커넥션 팩토리
  - `appsettings.json` : 연결문자열/JWT 시크릿 (배포 전 반드시 값 교체 필요)

### 공용
- `03.Shared/WYNLAB.Shared` : `MenuDto.cs`, `AuthDto.cs`

### DB
- `04.Database/001_Base_Tables.sql` : `TBADEPT`, `TSMUSER`(EMP_NO 포함), `TSMUSERGRP`, `TSMUSERGRPMAP`, `TSMMENU`, `TSMMENUAUTH`, `TSMLOGINHIST`
- `04.Database/002_Seed_Admin_User.sql` : 테스트 관리자 계정 (`admin` / `1234`)
- `04.Database/003_Session_Procedure.sql` : `SSP_WYNLAB_GetSession` 세션조회 프로시저

## 권한 병합 규칙 (중요)
1. 사용자가 속한 **모든 그룹의 권한을 OR로 합산** — 하나의 그룹에서라도 Y면 최종 Y
2. 그 메뉴에 **개인(USER) 권한 행이 있으면 그룹 합산 결과를 완전히 덮어씀** (예외 부여/회수용)
3. `IS_ADMIN_YN = 'Y'` 인 사용자는 권한테이블 조회 없이 전체 메뉴 풀권한

## 개발서버 / 운영서버 분리 운영

### 클라이언트 (WinForms Shell)
- `appsettings.Dev.json` / `appsettings.Prod.json` 두 파일을 분리 관리
- 빌드 구성(Debug/Release)에 따라 `csproj`가 자동으로 알맞은 파일을 `appsettings.json` 이름으로 출력폴더에 복사
- ClickOnce 게시도 `Properties/PublishProfiles/Dev.pubxml` / `Prod.pubxml` 로 분리 — Visual Studio 게시 화면에서 프로필만 선택하면 됨
- Dev 빌드로 로그인하면 셸 타이틀바에 `[개발서버]` + 경고 문구가 표시되어, 운영 데이터로 착각하는 사고를 방지

### 서버 (WYNLAB.Api)
- ASP.NET Core 표준 방식 그대로 사용: `appsettings.json`(공통, 민감정보 없음) + `appsettings.Development.json` / `appsettings.Production.json`(환경별 DB 연결정보·JWT 시크릿)
- 실행 시 `ASPNETCORE_ENVIRONMENT` 환경변수 값에 따라 자동 병합됨 (커스텀 코드 불필요)
- **IIS 설정**: 개발용 API와 운영용 API를 **서로 다른 IIS 사이트(또는 앱풀)**로 분리해서 배포하고, 각 사이트의 `web.config`에 아래처럼 환경변수를 지정
  ```xml
  <aspNetCore ...>
    <environmentVariables>
      <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Development" />
    </environmentVariables>
  </aspNetCore>
  ```
  (운영 사이트는 `value="Production"`)
- 즉 코드/DLL은 완전히 동일한 걸 배포하고, IIS 사이트별 환경변수 값만 다르게 주는 방식 — 개발서버에 배포한 파일을 그대로 운영에도 복사해서 쓸 수 있어 배포 실수 위험이 적음

## 트러블슈팅 기록 (실제로 겪었던 문제 + 해결)

- **DB 연결문자열에 `Server=localhost`만 쓰면 Named Pipes 프로토콜을 먼저 시도하다 실패할 수 있음**
  → `Server=tcp:localhost,1433`처럼 **`tcp:`와 포트를 명시**하면 TCP로 바로 접속해서 해결됨.
  같은 서버에서 다른 DB(WYNLAB 등)에 붙는 API를 새로 만들 때도 이 패턴을 기본으로 사용할 것.
- appsettings 수정 후에는 **반드시 해당 IIS 사이트(또는 앱풀)를 재시작**해야 반영됨.
  `Restart-WebAppPool -Name "사이트명-pool"` 이 `iisreset`(서버 전체 재시작)보다 안전 — 다른 사이트(MES, BARO 등)에 영향 없음.
- Swagger가 안 뜨면 `web.config`의 `<aspNetCore>` 태그 안에 `ASPNETCORE_ENVIRONMENT=Development` 환경변수가 제대로 들어갔는지 먼저 확인.

## 지금 바로 하실 것 (사장님)
1. `001_Base_Tables.sql` → `002_Seed_Admin_User.sql` → `003_Session_Procedure.sql` 순서로 **개발 DB**에 먼저 실행 (운영 DB는 검증 후 별도 실행)
2. 개발서버/운영서버 IIS 주소, DB 서버 주소를 알려주시면 아래 파일들의 `[플레이스홀더]`를 실제 값으로 채워드림
   - `Shell/appsettings.Dev.json`, `Shell/appsettings.Prod.json` (API 주소)
   - `Api/appsettings.Development.json`, `Api/appsettings.Production.json` (DB 연결정보)
   - `Shell/Properties/PublishProfiles/Dev.pubxml`, `Prod.pubxml` (ClickOnce 게시경로)
3. DevExpress 정확한 버전 확인 → 서브스크립션 만료일 확인
4. Visual Studio 2022 + .NET 8 SDK 설치 여부 확인

## 다음 제가 할 것
- DevExpress 실제 버전 확정되면 Shell/UI.Common의 `PackageReference` 채워넣기
- 첫 업무화면(수주관리 등) 1개를 `BaseGridForm` 상속받아 템플릿으로 완성
- ClickOnce 배포 설정 + IIS 가상디렉터리 구성 가이드

## 참고
- API는 로컬 개발 기준 `https://localhost:5001` 로 하드코딩되어 있습니다 (`ApiClient.cs`). 배포 단계에서 설정파일로 분리 예정입니다.
- `Program.cs`(Shell)의 클라이언트 참조는 DevExpress 부분이 TODO 상태라 그대로는 빌드되지 않습니다. 개발 PC에서 DevExpress 참조 추가 필요.
- 테스트 계정 `admin/1234`는 개발 편의용이며, 운영 반영 전 반드시 비밀번호를 변경해야 합니다.
