# 팝업(코드+명 검색) 프레임워크 개발 가이드

목표: 화면에 "코드/명 검색 팝업"(부서/품목/거래처...)을 붙이고 싶을 때, **화면 클래스를 새로
만들지 않고** 팝업관리 화면에서 정의만 등록해서 바로 쓰는 방법을 정리한다. 컨트롤 하나로
팝업을 여는 방법(코드+명 2개 필드 페어)과, 팝업 결과 행의 여러 컬럼을 화면의 여러 컨트롤에
한 번에 채워 넣는 방법(멀티필드 모드) 둘 다 다룬다.

관련 문서: 화면 자체를 처음부터 만드는 절차는 [`SCREEN_DEVELOPMENT_GUIDE.md`](SCREEN_DEVELOPMENT_GUIDE.md),
범용 데이터 통로(`api/data/*`)는 [`GENERIC_DATA_API.md`](GENERIC_DATA_API.md) 참고.

---

## 0. 전체 구조

```
[DB]  sysPopUpM(팝업 정의) / sysPopUpD(그리드 컬럼) / sysPopUpS(조회조건)
      SSP_POP_{ENTITY}_Q  ← 팝업이 실제로 조회하는 프로시저(엔티티마다 하나)
        │
        ▼
[서버] Controllers/Framework/LookupsController        - 런타임 조회(정의/검색), 메뉴권한 없음
       Controllers/Framework/PopupAdminController      - "컬럼생성"용 introspection, 메뉴권한 필요
       Repositories/Framework/PopupLookupRepository
        │  api/lookups/{key}/definition (GET)
        │  api/lookups/{key}/search     (POST, 조회조건 딕셔너리)
        ▼
[클라이언트] WYNLAB.BaseForm/PopupLookupForm.cs   - 정의를 읽어 팝업창을 스스로 그리는 공용 폼
             WYNLAB.BaseForm/ControlDataSources.cs - PopupLookupProvider에 실제 구현 연결(앱 시작 시 1회)
             WYNLAB.Controls/Controls/PopupLookupEditWyn.cs - 화면에 놓는 컨트롤(버튼+텍스트)
        │
        ▼
[화면]  frmXxx.Designer.cs - PopupLookupEditWyn 배치, LookupKey/NameControl 또는 MatchField/MapField 지정
```

**엔티티별로 새로 만드는 것은 DB 3줄(팝업 정의 + 조회조건)과 프로시저 하나(`SSP_POP_*_Q`)뿐이다.**
서버 컨트롤러/리포지토리, `PopupLookupForm`(팝업창 자체), `PopupLookupEditWyn`(컨트롤)은 전부
공용이라 손댈 필요가 없다.

---

## 1. 팝업 등록하기 (팝업관리 화면)

메뉴: **SYS > 팝업관리**(`frmSysPopup`, `99.SOURCE/SYS/WYNLAB.SYS/frmSysPopup/`).

### 1-1. 조회 전용 프로시저부터 만든다

```sql
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_DEPT_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT dept_cd, dept_nm, par_dept_cd, dept_type, remark
    FROM TBADEPT
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR dept_cd LIKE '%' + @p_keyword + '%'
           OR dept_nm LIKE '%' + @p_keyword + '%')
    ORDER BY dept_cd;
END
```

규칙:

- 이름은 **`SSP_POP_{ENTITY}_Q`**(예: `SSP_POP_DEPT_Q`, `SSP_POP_ITEM_Q`) - CRUD용
  `USP_{모듈}_*`와 완전히 별개다. 메뉴별 `PROC_PREFIX` 화이트리스트에 안 걸리는 별도 공용
  API(`api/lookups/*`)로 노출되므로, 이 프로시저는 **어느 화면에서든** 호출 가능해야 한다는
  전제로 만든다(`work_type` 분기 없이 조회 전용 하나로 끝낸다).
- **파라미터는 몇 개든, 이름이 뭐든 상관없다.** 예전엔 모든 팝업이 `@p_keyword` 하나만
  받는다고 가정했지만 지금은 그 제약이 없다 - 파라미터가 0개(조회조건 없이 전체 목록)여도
  되고, `@p_dept_cd`/`@p_use_yn`처럼 여러 개여도 된다. "컬럼생성" 버튼(1-3)이 실제 파라미터
  목록을 프로시저에서 읽어와서 `sysPopUpS`에 채워준다.
- `SELECT`하는 컬럼 중 최소 2개(값으로 쓸 컬럼, 표시할 컬럼)가 있어야 하고, 나머지는
  멀티필드 모드(3장)에서 다른 컨트롤에 매핑할 때 쓸 수 있다 - **화면에 그리드로 안 보여주는
  컬럼도 매핑에는 쓸 수 있으니, 나중에 쓸 만한 컬럼은 미리 SELECT 목록에 넣어두는 게 좋다.**

### 1-2. 팝업관리에서 마스터 등록

팝업관리 좌측 목록에서 "입력" 후 상세 패널에 채운다:

| 필드 | 예시 | 설명 |
|---|---|---|
| 팝업키 | `P_DEPT` | **`P_`로 시작**(사장님 확정 네이밍 규칙, 2026-08-31). `PopupLookupEditWyn.LookupKey`에 그대로 쓰는 값 |
| 팝업명 | `부서 조회` | 팝업창 제목으로 그대로 뜸 |
| 프로시저명 | `SSP_POP_DEPT_Q` | 1-1에서 만든 프로시저 |
| 값필드 | `dept_cd` | 결과 행 중 "코드"로 쓸 컬럼명 |
| 표시필드 | `dept_nm` | 결과 행 중 "명칭"으로 쓸 컬럼명 |
| 계층형여부 | 트리(부서처럼 상하위가 있으면) / 그리드(대부분) | 트리면 키필드/부모필드도 지정 |
| 팝업너비/높이 | 700 / 500 | 기본값, 팝업마다 재정의 가능 |

### 1-3. "컬럼생성"으로 컬럼/조회조건 자동 채우기

프로시저명을 넣은 뒤 **"컬럼생성" 버튼**을 누르면:

- 프로시저를 **실행하지 않고** `sys.dm_exec_describe_first_result_set`/`sys.parameters`로
  구조만 읽어서, 그리드에 보여줄 컬럼 목록(`sysPopUpD`)과 조회조건(파라미터) 목록
  (`sysPopUpS`)을 자동으로 채워 넣는다.
- **이미 있는 행(캡션/타입 등을 손으로 고친 것)은 안 건드린다** - 프로시저에 컬럼/파라미터를
  나중에 추가하고 이 버튼을 다시 눌러도 기존 설정이 안 지워진다. 그래서 프로시저를 고칠
  때마다 이 버튼을 다시 눌러주면 된다.
- 그 후 각 컬럼의 **캡션**(그리드 헤더 표시 텍스트), 각 조회조건의 **라벨**(검색창 입력창
  앞에 뜨는 문구, 예: "부서코드/명")은 자동으로 안 정해지니 직접 채운다.

### 1-4. 조회조건(sysPopUpS)이 하는 일

팝업 상단 검색창은 **조회조건 개수만큼 라벨+입력창이 나열**된다(고정된 검색창 하나가
아니다). 조회조건이 하나도 없으면 검색줄 자체가 안 보인다(전체 목록만 뜨는 팝업이 됨).
DATE 타입으로 지정하면 입력창이 DateEdit으로 뜬다.

---

## 2. 화면에 붙이기 - 모드① 코드+명 페어 (가장 단순한 경우)

컨트롤 하나로 "코드값"을 받고, 짝이 되는 "명칭" 텍스트박스를 자동으로 채워주기만 하면
될 때 쓴다.

### 2-1. 컨트롤 배치

Designer.cs에서 코드 필드를 `PopupLookupEditWyn`으로 바꾸고 명칭 필드는 평범한
`TextEditWyn`으로 둔다:

```csharp
this.txtParDeptCd = new WYNLAB.Base.Controls.PopupLookupEditWyn();
this.txtParDeptNm = new WYNLAB.Base.Controls.TextEditWyn();
...
this.txtParDeptCd.LookupKey = "P_DEPT";
this.txtParDeptCd.NameControl = this.txtParDeptNm;
```

- `LookupKey` - 1장에서 등록한 팝업키.
- `NameControl` - 선택 결과의 "명칭"을 채워 넣을 컨트롤.

### 2-2. 동작

- "..." 버튼을 누르면 `P_DEPT` 팝업이 뜬다. 행을 고르면(더블클릭 또는 "선택" 버튼)
  `txtParDeptCd.Text` = 값필드(`dept_cd`), `txtParDeptNm.Text` = 표시필드(`dept_nm`).
- `.Text`로 읽고 쓰는 기존 화면 코드는 그대로 - `EnterEditMode`/`SaveClick`에서
  `txtParDeptCd.Text`를 읽는 부분을 안 고쳐도 된다.
- `txtParDeptNm`을 지우면(사용자가 명칭 텍스트박스를 직접 비우면) `txtParDeptCd`도
  같이 비워진다(선택 해제 의도로 간주).

이 모드는 코드로 따로 할 일이 없다 - Designer.cs 두 줄이 끝이다.

---

## 3. 화면에 붙이기 - 모드② 멀티필드 (팝업 결과 행 전체를 여러 컨트롤에 매핑)

팝업 결과가 코드+명 2개가 아니라 **임의 개수의 컬럼**을 여러 컨트롤에 각각 채워 넣고
싶을 때 쓴다. 예: 부서 팝업에서 `dept_cd, dept_nm, par_dept_cd, par_dept_nm` 4개를 각각
`txtdept_cd, txtdept_nm, txtpar_dept_cd, txtpar_dept_nm`에 채우고 싶은 경우.

이 모드는 **"명칭" 필드 자신이 팝업을 여는 컨트롤**이 된다(모드①처럼 별도 코드 필드에
버튼이 붙는 게 아니다) - 사용자가 이름을 직접 타이핑하고 다른 곳으로 포커스를 옮기면,
그 값으로 조용히 검색해서 정확히 하나만 일치하면 자동으로 채우고, 아니면 그 값으로
미리 검색된 팝업이 떠서 고르게 한다.

### 3-1. 컨트롤 배치

트리거가 될 필드(보통 "명칭")를 `PopupLookupEditWyn`으로 만든다:

```csharp
this.txtdept_nm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
```

### 3-2. 코드에서 매핑 등록 (생성자에서, `InitializeComponent()` 뒤)

`Dictionary` 매핑이라 디자이너 속성창으로는 못 하고 코드로만 가능하다:

```csharp
public frmDept()
{
    InitializeComponent();

    // 이 컨트롤 자신의 타이핑 값 = 팝업 결과의 dept_nm 컬럼
    txtdept_nm.LookupKey = "P_DEPT";
    txtdept_nm.MatchField = "dept_nm";

    // 나머지 컬럼들을 원하는 컨트롤에 매핑(자기 자신은 등록 안 해도 됨)
    txtdept_nm.MapField("dept_cd", txtdept_cd);
    txtdept_nm.MapField("par_dept_cd", txtpar_dept_cd);
    txtdept_nm.MapField("par_dept_nm", txtpar_dept_nm);

    ...
}
```

- `LookupKey` - 모드①과 동일, 팝업키.
- `MatchField` - 이 컨트롤 자신에 타이핑된 값이 팝업 결과의 **어느 컬럼**과 같은 의미인지
  (여기서는 `dept_nm`). 이게 있어야 3-3의 자동조회/자동팝업이 켜진다.
- `MapField(컬럼명, 대상컨트롤)` - 필요한 만큼 여러 번 호출. 대상 컨트롤은
  `.Text`를 가진 아무 `Control`이나 가능(`TextEditWyn`, `CheckBoxWyn` 등).

> **매핑에 쓰려는 컬럼(예: `par_dept_nm`)이 실제로 `SSP_POP_DEPT_Q`의 `SELECT` 목록에
> 있어야 한다.** 그리드에 안 보이게(`visible_yn='N'`) 해둔 컬럼도 매핑엔 쓸 수 있지만,
> 프로시저가 애초에 그 컬럼을 안 돌려주면 매핑 대상 자체가 없다 - 1-1의 SELECT 목록부터
> 확인할 것.

### 3-3. 동작 흐름

1. 사용자가 `txtdept_nm`에 "영업"이라고 타이핑하고 다른 컨트롤로 포커스를 옮긴다(Leave).
2. `txtdept_nm`은 `P_DEPT`의 조회조건 전부에 "영업"을 넣어 조용히 검색한다(팝업 UI는
   아직 안 뜸).
3. 검색 결과 중 `dept_nm`이 정확히 "영업"과 일치하는 행이:
   - **정확히 1개** → 팝업을 띄우지 않고 그 행으로 `dept_cd`/`dept_nm`/`par_dept_cd`/
     `par_dept_nm` 4개 필드를 조용히 채운다.
   - **0개 또는 여러 개**(예: "영업1팀"/"영업2팀"/"영업3팀"이 다 걸림) → `P_DEPT` 팝업이
     **"영업"으로 이미 검색된 상태**로 뜬다. 사용자가 그중 하나를 고르면 그 행으로 4개
     필드를 채운다.
4. `txtdept_nm`을 지우면 `MapField`로 등록된 나머지 필드(`dept_cd`/`par_dept_cd`/
   `par_dept_nm`)도 같이 지워진다.
5. "..." 버튼은 그대로 남아있어서, 타이핑 없이 수동으로 팝업을 열어 고를 수도 있다.

### 3-4. 모드①/②를 같은 화면에서 같이 쓸 수 있는가

된다 - `PopupLookupEditWyn`은 컨트롤 인스턴스마다 독립적으로 모드가 정해진다.
`MatchField`/`MapField`를 안 쓴 컨트롤은 모드①(코드+명 페어, 버튼으로만 동작)로
그대로 동작한다. 예를 들어 상위부서는 모드①(`txtParDeptCd`+`NameControl`), 자기
부서 검색은 모드②(`txtdept_nm`+`MatchField`+`MapField`)로 한 화면에 섞어 써도 된다.

---

## 4. 실전 예제 - 부서 팝업을 부서등록 화면에 4필드 매핑

이미 있는 필드 구성(`txtDeptCd`/`txtDeptNm`/`txtParDeptCd`/`txtParDeptNm`)을 기준으로,
만약 "부서명을 타이핑해서 그 부서의 정보 전체(자기 코드/명 + 상위부서 코드/명)를
채우고 싶다"면:

```csharp
// frmDept.Designer.cs - txtDeptNm을 PopupLookupEditWyn으로 바꾼다
this.txtDeptNm = new WYNLAB.Base.Controls.PopupLookupEditWyn();
```

```csharp
// frmDept.cs 생성자
txtDeptNm.LookupKey = "P_DEPT";
txtDeptNm.MatchField = "dept_nm";
txtDeptNm.MapField("dept_cd", txtDeptCd);
txtDeptNm.MapField("par_dept_cd", txtParDeptCd);
txtDeptNm.MapField("par_dept_nm", txtParDeptNm);
```

전제조건(1-1 참고): `SSP_POP_DEPT_Q`가 `par_dept_nm`(상위부서 명칭)까지 돌려줘야 한다 -
지금 프로시저는 `par_dept_cd`까지만 있고 `par_dept_nm`은 없으므로(상위부서명은 자기
조인으로 구해야 함), 이 예제를 실제로 쓰려면 프로시저에 자기조인을 추가하고 팝업관리에서
"컬럼생성"을 다시 눌러 `sysPopUpS`도 갱신해야 한다.

---

## 5. 자주 하는 실수 체크리스트

- [ ] 팝업키가 `P_`로 시작하는가(네이밍 규칙)
- [ ] `SSP_POP_*_Q`가 `work_type` 분기 없이 조회 전용 하나로만 되어 있는가(CRUD 프로시저와
      혼동 금지)
- [ ] 매핑(`MapField`)에 쓰려는 컬럼이 실제로 프로시저 `SELECT` 목록에 있는가
- [ ] `MatchField`를 지정 안 하고 `MapField`만 등록하면 멀티필드 모드가 안 켜진다(Leave
      시 자동조회/자동팝업이 동작 안 함) - 둘 다 있어야 한다
- [ ] 프로시저에 컬럼/파라미터를 추가한 뒤 팝업관리에서 "컬럼생성"을 다시 안 눌러서
      `sysPopUpD`/`sysPopUpS`가 옛날 상태로 남아있지 않은가
- [ ] 조회조건(`sysPopUpS`)의 라벨을 안 채워서 검색창에 라벨 없이 입력창만 뜨고 있지 않은가
- [ ] 화면 코드에서 하드코딩한 팝업키 문자열(`LookupKey = "..."`)이 팝업관리에 등록된
      실제 `popup_key`와 정확히 일치하는가(팝업키를 나중에 바꾸면 화면 코드도 같이 바꿔야 함)

---

## 6. 참고 - 콤보(LookUp)는 다른 프레임워크

이 문서는 **팝업(코드+명 검색창)** 얘기다. 콤보박스(`LookUpEditWyn`)는 비슷한 발상이지만
별도 프레임워크(`sysLookupM`/`sysLookupP`, `L_` 접두어, LookUp관리 화면 `frmSysLookup`)로
분리되어 있다 - 결과 화면(팝업창 vs 드롭다운)과 트리거 방식이 달라서 컨트롤도 다르다
(`PopupLookupEditWyn` vs `LookUpEditWyn.LookupKey`). 콤보 쪽은 아직 이 문서에서 안 다룬다.

---

## 7. 관련 파일 위치

| 역할 | 경로 |
|---|---|
| DB 테이블/프로시저 | `00.DEV/04.Database/033_Popup_Framework_Tables_And_DeptProc.sql`, `035_Sys_Popup_Manage_Procs.sql`, `037_Popup_Search_Conditions.sql` |
| 팝업관리 화면 | `99.SOURCE/SYS/WYNLAB.SYS/frmSysPopup/` |
| 런타임 팝업창 | `00.DEV/01.Client/WYNLAB.BaseForm/PopupLookupForm.cs` |
| 컨트롤 | `00.DEV/01.Client/WYNLAB.Controls/Controls/PopupLookupEditWyn.cs` |
| 프로바이더 등록 | `00.DEV/01.Client/WYNLAB.BaseForm/ControlDataSources.cs` |
| 서버 컨트롤러/리포지토리 | `00.DEV/02.Server/WYNLAB.Api/Controllers/Framework/`, `Repositories/Framework/PopupLookupRepository.cs` |
| 사용 예 | `99.SOURCE/BA/WYNLAB.BA/frmDept/` |
