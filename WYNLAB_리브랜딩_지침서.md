# WYN LAB 리브랜딩 작업 지침서 (Claude Code용)

이 문서를 로컬 PC의 `NEXTFramework.sln`이 있는 폴더에 저장하고, Claude Code에서
"이 지침서대로 리브랜딩 작업 진행해줘"라고 요청하면 됩니다.

## 배경
- 기존 사내 프레임워크명 NEXTFramework → 제품명 **WYN LAB**으로 전환
- 캐치프레이즈: **"What You Need"**
- WYN = What You Need / LAB = Logic Application, Business
- 동시에 로컬 개발환경 전환 작업도 진행 중 (DB: 로컬 SQL Server Express, 인스턴스명 SQLEXPRESS)

## 변경 매핑표

| 항목 | 기존 | 변경 후 |
|---|---|---|
| 솔루션 파일 | `NEXTFramework.sln` | `WYNLAB.sln` |
| 프로젝트: Shell | `NEXTFramework.Shell` | `WYNLAB.Shell` |
| 프로젝트: UI.Common | `NEXTFramework.UI.Common` | `WYNLAB.UI.Common` |
| 프로젝트: Modules.System | `NEXTFramework.Modules.System` | `WYNLAB.Modules.System` |
| 프로젝트: Api | `NEXTFramework.Api` | `WYNLAB.Api` |
| 프로젝트: Shared | `NEXTFramework.Shared` | `WYNLAB.Shared` |
| 네임스페이스 루트 | `NEXTFramework.*` | `WYNLAB.*` |
| DB명 | `NEXTONE` | `WYNLAB` |
| DB 로그인 계정 | `nextone` | `wynlab` |
| DB 로그인 비밀번호 | `@nextone.com12!@` | `@wynlab.com12!@` (로컬 개발용 임시 제안, 원하면 다른 값으로) |
| API 배포 경로 (서버, 참고용) | `C:\inetpub\nextfw-api-dev` | `C:\inetpub\wynlab-api-dev` |
| Shell 배포 경로 (서버, 참고용) | `C:\NEXTFW-ClientTest` | `C:\WYNLAB-ClientTest` |
| 화면 표시 텍스트 (로그인 브랜드 패널, 타이틀바, About) | NEXTFramework / NEXTONE | WYN LAB |

## 바꾸지 않는 것 (그대로 유지)
- DB 테이블 접두어 (`TBADEPT`, `TSMUSER`, `TSMMENU` 등) — 모듈코드 기반 명명규칙이라 브랜드와 무관
- 포트 번호 (8090~8093), 서버 IP (`115.23.220.115`)
- `appsettings.json`의 키 구조 (`Environments`, `ToolbarColor`, `Theme` 등)
- 테스트 계정 `admin`/`1234` (사용자 계정이지 DB 로그인 계정이 아님)

## 로컬 DB 리네임 (이미 복원 완료된 NEXTONE DB를 재사용)
새로 만들지 말고, 이미 로컬에 복원해둔 DB를 이름만 바꿉니다. SSMS 새 쿼리창에서 실행:

```sql
ALTER DATABASE NEXTONE SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
ALTER DATABASE NEXTONE MODIFY NAME = WYNLAB;
ALTER DATABASE WYNLAB SET MULTI_USER;
GO

USE WYNLAB;
GO
CREATE LOGIN wynlab WITH PASSWORD = '@wynlab.com12!@', CHECK_POLICY = OFF;
CREATE USER wynlab FOR LOGIN wynlab;
ALTER ROLE db_owner ADD MEMBER wynlab;
GO
```

기존 `nextone` 로그인은 확인 후 필요 없으면 나중에 삭제해도 됩니다.

## 코드에서 놓치기 쉬운 파일 (Claude Code에게 꼭 확인시킬 것)
- `*.csproj` — `AssemblyName`, `RootNamespace`, `<Product>`, `<Company>`, `<AssemblyTitle>`
- `*.Designer.cs` — 폼 디자이너 파일도 네임스페이스 선언부가 있어 반드시 함께 변경
- `*.resx` — 리소스 파일의 root namespace 메타데이터
- `appsettings.Development.json` / `appsettings.json` — 연결문자열과 Environments 주소
- `LoginForm.cs` — 좌측 브랜드 그라데이션 패널 하드코딩 텍스트
- `ShellForm.cs` — 타이틀바 텍스트
- `04.Database` 스크립트 내 DB명이 하드코딩된 구문 (있다면)

## Claude Code에 그대로 붙여넣을 작업 요청

```
이 프로젝트(NEXTFramework)를 WYN LAB으로 리브랜딩하는 작업을 진행해줘.
같은 폴더의 WYNLAB_리브랜딩_지침서.md에 매핑표와 주의사항을 정리해뒀으니 그 기준으로 진행해줘.

순서:
1. 먼저 시작하기 전에 git 커밋을 하나 남겨줘 (git 저장소가 없으면 git init부터).
2. 프로젝트 전체에서 NEXTFramework, NEXTONE, nextone 이 등장하는 파일 목록을
   먼저 검색해서 나한테 보여줘 (실행 전 확인용).
3. 지침서의 매핑표에 따라 솔루션/프로젝트명, 네임스페이스, csproj 속성,
   Designer.cs, resx 를 빠짐없이 변경해줘.
4. appsettings.Development.json의 DB 연결문자열을 Database=WYNLAB,
   User Id=wynlab, Password=@wynlab.com12!@ 로 갱신해줘.
5. 화면에 하드코딩된 브랜드 텍스트(로그인 화면, 타이틀바, About)도
   WYN LAB / What You Need 로 바꿔줘.
6. 다 바꾼 뒤 전체 솔루션을 빌드해서 에러 없는지 확인해줘.
   에러 있으면 하나씩 같이 고치자.
```

## 참고
- 로컬 DB 리네임 SQL은 미리 실행해도 되고, Claude Code에게 "SSMS에서 이 SQL 실행해줘"라고
  요청은 못 시키니(원격 DB 도구 직접 조작은 Claude Code 범위 밖) 이 부분만 직접 SSMS에서 실행하세요.
