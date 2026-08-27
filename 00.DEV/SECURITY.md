# WYN LAB 보안 설계 메모

개발자(1인)를 위한 참고 문서. "어떤 걸 이미 해뒀고, 새 필드/화면을 만들 때 뭘 지켜야 하는지"를
정리해둔다. 2026-08-22 보안점검 후 정리 - 이후 바뀌면 이 문서도 같이 갱신할 것.

## 1. 지금 되어 있는 것

- **비밀번호**: BCrypt 해시(`BCrypt.Net.BCrypt.HashPassword`/`Verify`). 절대 평문 저장 금지.
- **SQL Injection**: 모든 DB 접근은 Dapper + 스토어드 프로시저(파라미터 바인딩)만 사용. 문자열
  조합으로 SQL 만들지 말 것.
- **인증**: 로그인 성공 시 JWT(60분) 발급, `Authorization: Bearer` 헤더로 전송. 클라이언트는
  메모리에만 들고 있고 디스크에 저장하지 않는다(`SessionManager`).
- **서버측 권한 체크**: `[Authorize]`는 "로그인했는가"만 확인한다. 실제 등록/수정/삭제/조회
  권한은 `[RequireMenuPermission(menuCd, MenuAction.Insert 등)]` 액션 필터로 요청마다
  TSMMENUAUTH 기준 병합 권한(`MenuPermissionService`)을 서버에서 다시 검증한다 - 클라이언트의
  버튼 비활성화(`CanInsert` 등)는 UX일 뿐, 실제 방어선은 이 필터다.
  **새 컨트롤러/액션을 추가할 때마다 이 필터를 반드시 붙일 것** (참고:
  `02.Server/WYNLAB.Api/Authorization/RequireMenuPermissionAttribute.cs`,
  적용 예시는 `UsersController`/`UserGroupsController`/`MenusController`/`MenuAuthController`).
- **시크릿**: DB 접속문자열/JWT 서명키는 `appsettings.*.json`에 두지 않는다.
  - 로컬 개발: `dotnet user-secrets set "Jwt:SecretKey" "..."` 등으로 관리(git 대상 아님).
  - 운영: `appsettings.Production.json`은 값을 두지 않고, 배포 서버에 환경변수로 주입한다
    (`Jwt__SecretKey`, `ConnectionStrings__WynlabDb` - 콜론 자리를 `__`로).

## 2. 아직 안 된 것 (백로그)

- **HTTPS**: 운영서버 API가 현재 평문 HTTP로 서비스되고 있다(`192.168.160.10:8090`, KT 외부포트가
  아직 안 열려있어서 지금은 내부망 IP로만 서비스 중).
  로그인 자격증명·JWT·업무데이터가 패킷상에서 평문으로 오간다. 서버에 TLS 인증서를 설치하고,
  다음을 순서대로 바꿔야 실제로 적용된다:
  1. 서버(Kestrel 또는 IIS)에 인증서 바인딩
  2. `01.Client/WYNLAB.Shell/appsettings.Dev.json` / `appsettings.Prod.json`의 `ApiBaseUrl`을
     `https://`로 변경
  3. `01.Client/WYNLAB.Shell/Properties/PublishProfiles/Dev.pubxml` / `Prod.pubxml`의
     `PublishUrl`/`InstallUrl`도 `https://`로 변경
  - `app.UseHsts()`는 이미 켜뒀으니(Program.cs), 인증서만 설치되면 바로 효과가 생긴다.
- **DB 비밀번호 로테이션**: git 히스토리에 예전 커밋으로 평문 비밀번호가 이미 노출되어 있다.
  값 자체는 이번에 안 바꿨음(로그인 계정 비밀번호 변경은 DB 관리 작업이라 별도로 진행 필요) -
  여유 될 때 SSMS 등에서 실제 로그인 비밀번호를 바꾸고 `user-secrets`/서버 환경변수도 같이 갱신할 것.
- **Refresh Token**: 로그인 응답에 발급은 되지만(`AuthService`), 이걸 받아서 갱신해주는 API
  엔드포인트가 없다 - 60분 지나면 재로그인해야 함. 보안 이슈는 아니고 UX 정리 대상.

## 3. 향후 민감정보(PII) 필드 추가 시 원칙

지금 스키마엔 주민번호·계좌번호 같은 고민감 정보가 없다(TSMUSER에 이메일/휴대폰만 평문으로
있음 - 이 둘은 저민감도로 분류하고 지금 당장 암호화하진 않는다). **주민번호, 계좌번호, 카드번호,
급여 등 필드가 새로 생기면 아래 원칙을 따른다**:

### 저장
- 앱 레이어에서 AES-256으로 암호화한 뒤 저장한다(컬럼 자체는 `VARBINARY`/암호문 문자열).
  SQL Server Always Encrypted도 대안이지만, 이 프로젝트 규모(솔로 개발, Dapper 기반)에서는
  앱 레이어 암호화가 구현/운영 부담이 더 적다.
- 암호화 키는 **appsettings.json에 절대 넣지 않는다** - 위 "시크릿" 절과 동일하게
  `user-secrets`(개발)/환경변수(운영)로 관리. 키 로테이션이 필요해질 걸 대비해서 키 버전 필드를
  같이 두는 것도 고려(예: `ENCRYPTED_VALUE`, `KEY_VERSION` 컬럼 쌍).
- 검색/조회 조건으로 써야 하는 값(예: 주민번호 뒷자리로 조회)은 원문을 암호화하는 것과 별개로,
  검색용 해시 컬럼(`HMAC-SHA256` 등, salt 고정)을 추가로 둘 것 - 암호화된 값 그대로는 SQL
  `WHERE`로 검색할 수 없다.

### 화면 표시
- 목록/조회 화면 기본값은 **마스킹**(예: 주민번호 뒷자리 `●●●●●●●`, 계좌번호 중간 자리 마스킹).
  전체 값이 필요한 화면(예: 실제 이체 처리)에서만, 그것도 필요한 순간에만 복호화해서 보여준다.
- 그리드에 절대 평문으로 깔아두지 않는다 - `GridColumn`에 마스킹된 문자열을 바인딩하고, 원본은
  버튼 클릭 등 명시적 동작에서만 별도 API로 조회.

### 로깅
- 지금은 별도 로깅 프레임워크가 없어서 당장 위험은 없지만, 나중에 Serilog 등을 붙일 때 요청/응답
  바디를 통째로 로깅하는 미들웨어는 절대 켜지 말 것(또는 민감필드 자동 마스킹 필터를 반드시
  같이 넣을 것). 비밀번호/토큰/PII가 로그 파일에 평문으로 남는 게 제일 흔한 사고 유형이다.

### 전송
- 위 HTTPS 항목이 선행되어야 한다 - PII는 암호화해서 저장해도, 조회 API 응답 자체가 평문 HTTP로
  나가면 네트워크 구간에서 그대로 노출된다.
