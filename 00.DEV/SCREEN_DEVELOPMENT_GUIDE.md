# 화면 개발 가이드 - 기초코드등록(CODE) 화면 기준

목표: 조회/저장 버튼을 눌렀을 때 실제로 어떤 파일들을 거쳐서 어느 프로시저가 호출되는지,
변수가 어떻게 전달되는지 끝까지 추적한다. 나중에 **조회조건을 추가**하거나 **저장 프로시저의
파라미터를 바꿔야** 할 때, 이 문서에 나온 4개 레이어를 순서대로 고치면 된다.

기준 화면: `기초코드등록`(TSMMAJOR 대분류 / TSMMINOR 소분류)

---

## 0. 전체 구조 - 4개 레이어

```
[1] 클라이언트 화면(WinForms)
      CodeListForm.cs / CodeListForm.Designer.cs
      99.SOURCE\SM\CODE\WYNLAB.SM.CODE\
        │  ApiClient.GetAsync<T>(url) / PostAsync / PutAsync
        ▼
[2] HTTP 클라이언트 공용 래퍼
      ApiClient.cs
      01.Client\WYNLAB.BaseForm\ApiClient.cs
        │  HTTP GET/POST/PUT (JSON)
        ▼
[3] API 컨트롤러 (ASP.NET Core)
      CodesController.cs
      02.Server\WYNLAB.Api\Controllers\CodesController.cs
        │  _repo.GetAsync(...) / _repo.SaveMajorAsync(...)
        ▼
[4] 리포지토리 (Dapper)
      CodeManageRepository.cs
      02.Server\WYNLAB.Api\Repositories\CodeManageRepository.cs
        │  conn.QueryAsync<T>("USP_SM_CODE_Q", new {...}, CommandType.StoredProcedure)
        ▼
[DB] 저장 프로시저
      USP_SM_CODE_Q / USP_SM_CODE_S / USP_SM_CODE_S_1
```

**조회조건을 추가하거나 저장 파라미터를 바꾸려면 이 4곳(화면 → Repository → Controller는
그대로 두고 실제로는 화면/Repository/프로시저 3곳, Controller는 대부분 손댈 일 없음)을
전부 같이 고쳐야 한다** - 하나만 고치면 컴파일은 되지만 값이 조용히 안 넘어가거나(NULL),
아예 컴파일 에러가 난다.

---

## 1. 조회(Query) 흐름 끝까지 추적

### 1-1. 화면: `CodeListForm.QueryClick()`

```csharp
// 99.SOURCE\SM\CODE\WYNLAB.SM.CODE\CodeListForm.cs
public override async Task QueryClick()
{
    var keyword = txtminor_cd_q.Text.Trim();
    var query = $"api/codes?majorCd={Uri.EscapeDataString(keyword)}&majorNm={Uri.EscapeDataString(keyword)}&selectedMajorCd={Uri.EscapeDataString(_editingMajorCd ?? string.Empty)}";
    var result = await ApiClient.GetAsync<CodeQueryResponse>(query) ?? new();
    _majors = result.Majors;
    _minors = new BindingList<MinorItemDto>(result.Minors);
    grd1.DataSource = _majors;
    grd2.DataSource = _minors;
    ...
}
```

- 화면 위 검색창(`txtminor_cd_q`) 값 하나를 대분류코드/대분류명 두 조건에 동시에 넣어서 쿼리스트링을
  직접 만든다 (`?majorCd=...&majorNm=...&selectedMajorCd=...`).
- `BaseForm.QueryClick()`을 override한 것 - Shell 상단 툴바의 "조회" 버튼을 누르면
  Shell이 현재 활성 화면의 이 메서드를 그대로 호출한다(`ShellForm.BuildToolbar()` 참고,
  이 부분은 화면마다 손댈 필요 없음).

### 1-2. HTTP 래퍼: `ApiClient.GetAsync<T>`

```csharp
// 01.Client\WYNLAB.BaseForm\ApiClient.cs
public static async Task<TResponse?> GetAsync<TResponse>(string url)
{
    var response = await _http.GetAsync(url);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<TResponse>();
}
```
그냥 GET 날리고 JSON을 `TResponse`(여기선 `CodeQueryResponse`)로 역직렬화하는 범용 헬퍼.
**이 파일은 화면마다 고칠 일이 없다** - 모든 화면이 공유.

### 1-3. 컨트롤러: `CodesController.Get`

```csharp
// 02.Server\WYNLAB.Api\Controllers\CodesController.cs
[HttpGet]
[RequireMenuPermission("SM_CODE_BASE", MenuAction.View)]
public async Task<ActionResult<CodeQueryResponse>> Get(
    [FromQuery] string? majorCd, [FromQuery] string? majorNm, [FromQuery] string? selectedMajorCd)
{
    var result = await _repo.GetAsync(majorCd, majorNm, selectedMajorCd);
    return Ok(result);
}
```
- `[FromQuery]` 파라미터 이름(`majorCd`, `majorNm`, `selectedMajorCd`)이 화면에서 만든
  쿼리스트링 키 이름과 **대소문자 무관하게 자동 매칭**된다(ASP.NET Core 모델바인딩).
  **새 조회조건을 추가하려면 여기에 파라미터를 하나 추가**해야 한다.
- `[RequireMenuPermission("SM_CODE_BASE", MenuAction.View)]` - 이 화면의 메뉴코드로
  권한을 한 번 더 검증(클라이언트에서 버튼을 막아놔도 API를 직접 호출하는 우회를 막음).
  화면코드가 다르면 이 문자열도 그 화면의 `MenuCd`로 바꿔야 한다.

### 1-4. 리포지토리: `CodeManageRepository.GetAsync`

```csharp
// 02.Server\WYNLAB.Api\Repositories\CodeManageRepository.cs
public async Task<CodeQueryResponse> GetAsync(string? majorCd, string? majorNm, string? selectedMajorCd)
{
    using var conn = _context.CreateConnection();

    var majors = (await conn.QueryAsync<MajorListItemDto>("USP_SM_CODE_Q",
        new
        {
            p_work_type = "Q",
            p_major_cd = string.IsNullOrWhiteSpace(majorCd) ? null : majorCd,
            p_major_nm = string.IsNullOrWhiteSpace(majorNm) ? null : majorNm
        },
        commandType: CommandType.StoredProcedure)).ToList();
    ...
}
```
**여기가 실제로 프로시저를 호출하는 지점이다.** 핵심 규칙:

- 첫 번째 인자 `"USP_SM_CODE_Q"` = 호출할 프로시저 이름(문자열, 그대로).
- 두 번째 인자 `new { p_work_type = "Q", p_major_cd = ..., p_major_nm = ... }` = **익명 객체의
  프로퍼티 이름이 그대로 프로시저의 `@` 파라미터 이름이 된다** (Dapper가 자동 매핑,
  `@`는 안 붙임 - `p_major_cd` → `@p_major_cd`).
- `p_work_type = "Q"` - 이 프로시저 안에서 `IF @p_work_type = 'Q'` 분기를 타게 하는 값
  (아래 1-5 참고). 화면 하나가 여러 조회 패턴(예: 대분류 조회='Q', 소분류만 조회='Q1')을
  프로시저 하나로 처리할 때 이 값으로 분기한다.
- `string.IsNullOrWhiteSpace(majorCd) ? null : majorCd` - 빈 문자열이면 반드시 `null`로
  바꿔서 넘긴다. 프로시저 쪽 WHERE절이 `@p_major_cd IS NULL OR ...` 패턴이라, 빈 문자열
  `""`을 그대로 넘기면 "조건 없음"이 아니라 "빈 문자열과 일치"로 해석돼서 검색결과가
  0건이 되는 실수가 흔하다.
- `commandType: CommandType.StoredProcedure` - 이게 없으면 Dapper가 SQL 텍스트로
  오인해서 실행 자체가 실패한다. **절대 빠뜨리면 안 됨.**

### 1-5. 프로시저: `USP_SM_CODE_Q`

```sql
CREATE PROCEDURE [dbo].[USP_SM_CODE_Q]
    @p_work_type VARCHAR(50),
    @p_major_cd VARCHAR(20) = NULL,
    @p_major_nm NVARCHAR(200) = NULL,
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;
    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT major_cd, major_nm, sys_yn, rel_cd1, rel_title1, rel_cd_type1, ... , remark
            FROM TSMMAJOR
            WHERE (@p_major_cd IS NULL OR major_cd LIKE '%' + @p_major_cd + '%')
              AND (@p_major_nm IS NULL OR major_nm LIKE '%' + @p_major_nm + '%')
            ORDER BY major_cd;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, rel_cd1..rel_cd10, remark
            FROM TSMMINOR
            WHERE major_cd = @p_major_cd
            ORDER BY sort;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
```
- `SELECT`문의 **컬럼명이 그대로 C# DTO(`MajorListItemDto`)의 프로퍼티명과 매칭**된다
  (Dapper가 리플렉션으로 자동 매핑, 대소문자 무관). 컬럼을 추가하면 DTO에도 같은 이름의
  프로퍼티를 추가해야 값이 실제로 화면까지 온다 - 프로시저만 고치고 DTO를 안 고치면
  그 값은 조용히 버려진다(에러 안 남).
- `@p_work_type`으로 `'Q'`(대분류 조회)/`'Q1'`(소분류 조회) 두 가지를 한 프로시저 안에서
  분기 처리한다. Repository의 `GetMinorsAsync`가 `p_work_type = "Q1"`으로 같은 프로시저를
  다시 호출하는 걸 보면 이 패턴이 더 명확하다.

---

## 2. 저장(Save) 흐름 끝까지 추적 - 대분류(단건) 저장

### 2-1. 화면: `CodeListForm.SaveClick()`

```csharp
// CodeListForm.cs
public override async Task SaveClick()
{
    ...
    var request = new MajorSaveRequest
    {
        major_cd = txtmajor_cd.Text,
        major_nm = txtmajor_nm.Text,
        sys_yn = false,
        remark = string.Empty,
        rel_cd1 = codes[0].Text, rel_title1 = titles[0].Text, rel_cd_type1 = (string?)types[0].EditValue,
        ... // rel_cd2~10 반복
    };

    var wasNew = _editingMajorCd == null;
    var result = wasNew
        ? await ApiClient.PostAsync<MajorSaveRequest, ApiResult>("api/codes", request)
        : await ApiClient.PutAsync<MajorSaveRequest, ApiResult>($"api/codes/{_editingMajorCd}", request);
    ...
}
```
- **DTO(`MajorSaveRequest`)에 화면 컨트롤 값을 그대로 채워서** 요청 본문(JSON body)으로 보낸다.
  조회(GET)는 쿼리스트링이었지만, 저장(POST/PUT)은 **요청 본문(JSON)**으로 보낸다는 게 차이.
- 신규(`POST api/codes`)/수정(`PUT api/codes/{majorCd}`)을 `_editingMajorCd`가 null인지로
  구분해서 다른 HTTP 메서드를 쓴다.

### 2-2. 컨트롤러: `CodesController.Create` / `Update`

```csharp
[HttpPost]
[RequireMenuPermission("SM_CODE_BASE", MenuAction.Insert)]
public async Task<ActionResult<ApiResult>> Create([FromBody] MajorSaveRequest request)
{
    var result = await _repo.SaveMajorAsync("N", request, CurrentUserId, ClientIp);
    ...
}

[HttpPut("{majorCd}")]
[RequireMenuPermission("SM_CODE_BASE", MenuAction.Update)]
public async Task<ActionResult<ApiResult>> Update(string majorCd, [FromBody] MajorSaveRequest request)
{
    request.major_cd = majorCd;
    var result = await _repo.SaveMajorAsync("U", request, CurrentUserId, ClientIp);
    ...
}
```
- `[FromBody]`가 JSON 본문 전체를 `MajorSaveRequest`로 역직렬화 - **DTO에 없는 필드는
  요청에 넣어도 여기서 무시된다.** 새 입력 필드를 추가하려면 `MajorSaveRequest`
  (`03.Shared\WYNLAB.Shared\Dtos\CodeManageDto.cs`)에 프로퍼티를 먼저 추가해야 한다.
- `SaveMajorAsync`의 첫 인자 `"N"`/`"U"` - 이게 프로시저의 `@p_work_type`으로 그대로 들어가서
  신규(INSERT)/수정(UPDATE) 분기를 결정한다(아래 2-4).

### 2-3. 리포지토리: `CodeManageRepository.SaveMajorAsync`

```csharp
public async Task<ProcResult> SaveMajorAsync(string workType, MajorSaveRequest request, string userId, string? clientPc)
{
    using var conn = _context.CreateConnection();
    var p = new DynamicParameters();
    p.Add("p_work_type", workType);
    p.Add("p_major_cd", request.major_cd);
    p.Add("p_major_nm", request.major_nm);
    p.Add("p_sys_yn", request.sys_yn ? "Y" : "N");
    for (var i = 1; i <= 10; i++)
    {
        p.Add($"p_rel_cd{i}", GetProp(request, $"rel_cd{i}"));
        p.Add($"p_rel_title{i}", GetProp(request, $"rel_title{i}"));
        p.Add($"p_rel_cd_type{i}", GetProp(request, $"rel_cd_type{i}"));
    }
    p.Add("p_remark", request.remark);
    p.Add("p_user_id", userId);
    p.Add("p_client_pc", clientPc);
    p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

    await conn.ExecuteAsync("USP_SM_CODE_S", p, commandType: CommandType.StoredProcedure);

    return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
}
```
여기가 조회(1-4)와 다른 두 가지 패턴:

1. **`DynamicParameters`를 직접 만들어서 `p.Add("파라미터명", 값)`으로 하나씩 채운다**
   (조회는 익명객체 `new {...}` 한 번으로 끝났지만, 저장은 출력 파라미터가 있어서
   `DynamicParameters`를 써야 함). `rel_cd1~10`처럼 번호 붙는 반복 필드는 `GetProp()`
   헬퍼(리플렉션으로 `MajorSaveRequest`의 `rel_cd3` 같은 프로퍼티 값을 이름으로 꺼냄)로
   루프를 돌려서 30줄 반복을 피한다.
2. **`p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true)`** - 이 화면
   전용 코드가 아니라 **모든 저장류 프로시저가 공유하는 표준 출력 5종**을 등록하는
   공용 헬퍼(`02.Server\WYNLAB.Api\Data\ProcResult.cs`):
   `GeneratedCode, ReturnCode, ReturnMsg, ErrorCode, ErrorMsg` (전부 OUTPUT 파라미터).
   `pascalCase: true`는 CODE 모듈처럼 새 프로시저 표준(접두사 없는 PascalCase:
   `@GeneratedCode`)을 쓰는 경우, `false`는 예전 프로시저(USER/MENU 등, `@return_code`
   snake_case)를 쓰는 경우 - **어느 쪽인지는 실제 DB 프로시저의 OUTPUT 파라미터 이름과
   반드시 일치시켜야 한다.** 안 맞으면 값이 안 채워지고 `ReturnCode`가 항상 0(성공)으로
   보여서, 실제로는 실패했는데 화면엔 "저장되었습니다"가 뜨는 조용한 버그가 난다.
3. 실행 후 `p.ReadStandardOutputs(...)`로 그 5개 OUTPUT 값을 꺼내서 `ProcResult`(성공여부/
   메시지/에러코드/생성된코드)로 반환 - 컨트롤러가 이걸 그대로 화면에 JSON으로 돌려준다.

### 2-4. 프로시저: `USP_SM_CODE_S`

```sql
CREATE PROCEDURE [dbo].[USP_SM_CODE_S]
    @p_work_type VARCHAR(50),
    @p_major_cd VARCHAR(20),
    @p_major_nm NVARCHAR(200) = NULL,
    @p_sys_yn VARCHAR(1) = 'N',
    @p_rel_cd1 VARCHAR(50) = NULL, @p_rel_title1 VARCHAR(50) = NULL, @p_rel_cd_type1 VARCHAR(10) = NULL,
    ... -- rel_cd2~10 반복
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;
    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, rel_cd1, ..., remark, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_major_cd, @p_major_nm, @p_sys_yn, @p_rel_cd1, ..., @p_remark, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMAJOR SET major_nm = @p_major_nm, sys_yn = @p_sys_yn, rel_cd1 = @p_rel_cd1, ...
            WHERE major_cd = @p_major_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMMAJOR WHERE major_cd = @p_major_cd;
        END
        SET @GeneratedCode = @p_major_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
```
- `@p_work_type`으로 `'N'`(INSERT)/`'U'`(UPDATE)/`'D'`(DELETE, CODE 화면은 안 쓰지만
  프로시저 표준상 자리는 있음) 분기 - Repository가 넘긴 `"N"`/`"U"` 문자열이 여기로 들어옴.
- **모든 파라미터가 `@p_` 접두사 + snake_case** - Repository의 `p.Add("p_xxx", ...)`
  이름과 정확히 일치해야 한다. 하나라도 이름이 다르면 그 파라미터는 프로시저 쪽
  기본값(`= NULL` 등)으로 조용히 들어가고, 잘못 넘겼다는 에러조차 안 난다(가장 흔한 실수).

---

## 3. 소분류 그리드 저장 - "전체 치환 + JSON" 패턴 (주의해서 볼 것)

`grd2`(소분류)는 한 건씩이 아니라 **그리드 전체를 JSON으로 직렬화해서 한 번에 보내고,
프로시저 안에서 DELETE 후 전체 INSERT**하는 다른 패턴을 쓴다.

```csharp
// CodeManageRepository.cs
public async Task<ProcResult> SaveMinorsAsync(string majorCd, List<MinorItemDto> items, string userId, string? clientPc)
{
    var itemsJson = System.Text.Json.JsonSerializer.Serialize(items);

    var p = new DynamicParameters();
    p.Add("major_cd", majorCd);
    p.Add("items_json", itemsJson);
    ...
    await conn.ExecuteAsync("USP_SM_CODE_S_1", p, commandType: CommandType.StoredProcedure);
    return p.ReadStandardOutputs(); // pascalCase 지정 안 함 = snake_case(return_code 등) 프로시저
}
```

```sql
CREATE PROCEDURE USP_SM_CODE_S_1
    @major_cd VARCHAR(20), @items_json NVARCHAR(MAX), @user_id VARCHAR(50), ...
AS
BEGIN
    DELETE FROM TSMMINOR WHERE major_cd = @major_cd;
    INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, ...)
    SELECT @major_cd, J.MinorCd, J.MinorNm, J.Sort, ...
    FROM OPENJSON(@items_json)
    WITH (
        MinorCd  VARCHAR(100)  '$.MinorCd',
        MinorNm  NVARCHAR(200) '$.MinorNm',
        Sort     INT           '$.Sort',
        ...
    ) J;
END
```

**여기가 이 패턴에서 가장 실수하기 쉬운 지점이다**: `OPENJSON ... WITH (...)`의 `'$.MinorCd'`
경로 문자열은 **`JsonSerializer.Serialize(items)`가 실제로 만드는 JSON의 키 이름과
글자 하나까지 정확히 일치**해야 한다. `MinorItemDto`의 프로퍼티는

```csharp
// 03.Shared\WYNLAB.Shared\Dtos\CodeManageDto.cs
public class MinorItemDto
{
    public string minor_cd { get; set; } = ...
    public string? minor_nm { get; set; }
    public int sort { get; set; }
    ...
}
```
전부 **snake_case**로 선언돼 있어서, `JsonSerializer.Serialize`(별도 NamingPolicy 지정 없음)는
JSON을 `{"minor_cd": "...", "minor_nm": "...", "sort": 1, ...}` 처럼 **소문자 snake_case
그대로** 만든다. 그런데 지금 로컬 개발 DB에 있는 `USP_SM_CODE_S_1`의 OPENJSON 경로는
`'$.MinorCd'`, `'$.MinorNm'`, `'$.Sort'`처럼 **PascalCase**로 되어 있다 - 즉 **지금 이 상태로는
경로가 하나도 안 맞아서 소분류 그리드를 저장해도 전부 NULL로 들어간다.** OPENJSON은 경로가
안 맞아도 에러를 안 내고 조용히 NULL을 반환하는 게 이 문제를 알아채기 어렵게 만드는 원인.

(이전에 이 정확한 문제를 겪고 프로덕션 쪽은 손으로 고치신 걸로 아는데, 이 로컬 개발 DB
사본에는 그 수정이 아직 안 들어간 것 같습니다 - 직접 확인해보시고 필요하면 `OPENJSON WITH`의
경로를 소문자 snake_case(`'$.minor_cd'` 등)로 맞추시면 됩니다. 제가 먼저 손대지는 않았습니다.)

---

## 4. 실전 예제 1 - 조회조건 추가하기

**예: 대분류 검색에 "사용여부(sys_yn)" 필터를 추가하고 싶다.**

1. **화면** (`CodeListForm.cs`) - 체크박스/콤보 컨트롤을 만들고, `QueryClick()`의
   쿼리스트링에 값을 추가:
   ```csharp
   var query = $"api/codes?majorCd=...&majorNm=...&sysYn={Uri.EscapeDataString(cboSysYn.EditValue?.ToString() ?? "")}";
   ```
2. **컨트롤러** (`CodesController.Get`) - 파라미터 추가:
   ```csharp
   public async Task<ActionResult<CodeQueryResponse>> Get(
       [FromQuery] string? majorCd, [FromQuery] string? majorNm,
       [FromQuery] string? selectedMajorCd, [FromQuery] string? sysYn)
   {
       var result = await _repo.GetAsync(majorCd, majorNm, selectedMajorCd, sysYn);
       ...
   }
   ```
3. **리포지토리 인터페이스+구현** (`ICodeManageRepository`/`CodeManageRepository.GetAsync`) -
   시그니처에 `string? sysYn` 추가, 프로시저 호출부에 파라미터 추가:
   ```csharp
   new { p_work_type = "Q", p_major_cd = ..., p_major_nm = ...,
         p_sys_yn = string.IsNullOrWhiteSpace(sysYn) ? null : sysYn }
   ```
4. **프로시저** (`USP_SM_CODE_Q`) - 파라미터 선언 + WHERE절 추가:
   ```sql
   @p_sys_yn VARCHAR(1) = NULL,
   ...
   WHERE (@p_major_cd IS NULL OR major_cd LIKE '%' + @p_major_cd + '%')
     AND (@p_major_nm IS NULL OR major_nm LIKE '%' + @p_major_nm + '%')
     AND (@p_sys_yn IS NULL OR sys_yn = @p_sys_yn)
   ```
   `04.Database\` 밑에 새 번호로 마이그레이션 파일을 만들어서 로컬 개발 DB에 먼저
   실행(`sqlcmd -f 65001`)하고, 운영 DB는 나중에 같은 스크립트로 반영.

**4곳 중 하나라도 빠뜨리면**: 화면만 고치면 새 파라미터가 그냥 무시되고(컨트롤러가
안 받으니까), 프로시저만 고치면 항상 `NULL`이 넘어와서(아무도 안 채우니까) 필터가
동작 안 하는 것처럼 보인다.

---

## 5. 실전 예제 2 - 저장 프로시저에 파라미터 추가/변경하기

**예: 대분류 저장 시 "비고2(remark2)" 필드를 추가하고 싶다.**

1. **DTO** (`03.Shared\WYNLAB.Shared\Dtos\CodeManageDto.cs`) - `MajorSaveRequest`와
   (조회 결과도 같이 보여줄 거면) `MajorListItemDto`에 프로퍼티 추가:
   ```csharp
   public string? remark2 { get; set; }
   ```
2. **화면** (`CodeListForm.cs`/`.Designer.cs`) - 입력 컨트롤 추가하고, `SaveClick()`의
   `MajorSaveRequest` 생성부에 값 채우기: `remark2 = txtremark2.Text`.
   조회해서 보여줄 거면 `EnterEditMode()`에도 반영.
3. **리포지토리** (`CodeManageRepository.SaveMajorAsync`) - `DynamicParameters`에 한 줄 추가:
   ```csharp
   p.Add("p_remark2", request.remark2);
   ```
4. **프로시저** (`USP_SM_CODE_S`) - 파라미터 선언 + INSERT/UPDATE 양쪽에 반영:
   ```sql
   @p_remark2 NVARCHAR(500) = NULL,
   ...
   -- INSERT
   INSERT INTO TSMMAJOR (..., remark2) VALUES (..., @p_remark2);
   -- UPDATE
   UPDATE TSMMAJOR SET ..., remark2 = @p_remark2 WHERE major_cd = @p_major_cd;
   ```
   테이블에 실제 컬럼(`TSMMAJOR.remark2`)이 없으면 당연히 먼저 `ALTER TABLE`로
   컬럼부터 추가해야 한다 - 이것도 `04.Database\`에 새 마이그레이션 파일로.

**표준 출력 5종(`ReturnCode` 등)은 절대 건드릴 필요 없음** - `p.AddStandardOutputs(...)`가
공용으로 처리하므로, 새 파라미터를 추가할 때 저 5개와 헷갈리지 않게 항상 **업무 파라미터를
먼저, 표준 출력 5종은 맨 마지막**에 두는 지금 구조를 그대로 따르면 된다.

---

## 6. 자주 하는 실수 체크리스트

- [ ] Repository에서 익명객체/`DynamicParameters`의 파라미터 이름이 프로시저의 `@` 파라미터
      이름과 **정확히** 일치하는가 (오타 나면 조용히 무시됨, 에러 안 남)
- [ ] 빈 문자열(`""`)을 조회조건으로 넘길 때 `null`로 바꿔서 넘겼는가 (안 그러면 "빈 값과
      일치"로 해석되어 결과 0건)
- [ ] `commandType: CommandType.StoredProcedure`를 빠뜨리지 않았는가
- [ ] `AddStandardOutputs(pascalCase: ...)`가 실제 프로시저의 OUTPUT 파라미터 이름
      (PascalCase `@GeneratedCode` vs snake_case `@generated_code`)과 맞는가
- [ ] JSON 직렬화 기반(OPENJSON) 저장을 쓸 때, DTO 프로퍼티 이름 casing과 `OPENJSON WITH`의
      경로 문자열 casing이 정확히 일치하는가 (3번 항목 참고 - 제일 찾기 어려운 버그 유형)
- [ ] 새 컬럼/파라미터를 추가했으면 DTO에도 같은 이름의 프로퍼티를 추가했는가 (프로시저
      SELECT에만 있고 DTO에 없으면 그 값은 화면까지 안 옴)

---

## 7. 새 화면 하나를 처음부터 만드는 전체 절차

DB → 서버 → 클라이언트 → 메뉴등록 → 배포 순서. 예시로 "설비관리(가상의 새 화면, 메뉴코드
`SM_EQUIP`, 테이블 `TSMEQUIP`)"를 만든다고 가정한다.

### 7-1. DB - 테이블 + 프로시저

`04.Database\`에 새 번호로 마이그레이션 파일 하나 생성(기존 파일은 절대 수정하지 않고
항상 새 파일 - `[DB migration workflow]` 관례). 안에 넣을 내용:

1. `CREATE TABLE TSMEQUIP (...)` (컬럼명은 전부 snake_case)
2. 저장 프로시저 3종(CODE 화면과 동일한 이름 규칙):
   - `USP_SM_EQUIP_Q` - `@p_work_type='Q'`(목록조회) 분기, 필요하면 `'Q1'`(상세/서브목록)도
   - `USP_SM_EQUIP_S` - `@p_work_type='N'/'U'/'D'` 분기(신규/수정/삭제), 표준 출력 5종
     (`@GeneratedCode/@ReturnCode/@ReturnMsg/@ErrorCode/@ErrorMsg`) 포함
3. `sqlcmd -S localhost,15434 -d WYNLAB -U wynlab -P "..." -f 65001 -i "04.Database\0XX_....sql"`로
   로컬 개발 DB에 먼저 실행해서 검증. 운영 DB는 검증 끝난 뒤 같은 파일로 별도 실행.

### 7-2. 공유 DTO (`03.Shared\WYNLAB.Shared\Dtos\EquipManageDto.cs`)

`CodeManageDto.cs`를 참고해서 프로시저 SELECT 컬럼/저장 파라미터와 **이름이 정확히 같은**
프로퍼티로 DTO 3~4개를 만든다: 목록조회용(`EquipListItemDto`), 저장요청용
(`EquipSaveRequest`), 필요하면 조회응답 래퍼(`EquipQueryResponse`).

### 7-3. 서버 - Repository (`02.Server\WYNLAB.Api\Repositories\EquipManageRepository.cs`)

`CodeManageRepository.cs`를 그대로 복사해서 이름/프로시저명/DTO만 바꾸는 게 제일 빠르다.
인터페이스(`IEquipManageRepository`)도 같은 파일에 같이 선언(이 프로젝트 관례).

### 7-4. 서버 - Controller (`02.Server\WYNLAB.Api\Controllers\EquipController.cs`)

`CodesController.cs` 복사해서:
```csharp
[Route("api/equip")]
public class EquipController : ControllerBase
{
    [HttpGet]
    [RequireMenuPermission("SM_EQUIP", MenuAction.View)]   // ← 메뉴코드는 7-6에서 정할 값과 반드시 같아야 함
    ...
}
```

### 7-5. 서버 - DI 등록 (`02.Server\WYNLAB.Api\Program.cs`)

**이거 하나 빠뜨리면 서버가 실행은 되는데 이 API만 500 에러가 난다** - 컨트롤러가
`IEquipManageRepository`를 생성자로 받는데, 등록을 안 해두면 ASP.NET Core가 인스턴스를
못 만든다.
```csharp
builder.Services.AddScoped<IEquipManageRepository, EquipManageRepository>();
```
등록 후 `dotnet build "02.Server\WYNLAB.Api\WYNLAB.Api.csproj"`로 빌드 확인, 로컬에서
`dotnet run`으로 띄우고 Swagger(`https://localhost:.../swagger`)에서 새 엔드포인트가
보이는지, 직접 호출해보고 응답이 오는지 확인하고 다음 단계로 넘어가는 게 안전하다.

### 7-6. 클라이언트 - 새 프로젝트 만들기

**직접 새 프로젝트를 만들지 말고 템플릿을 복사한다** - `99.SOURCE\SM\TEMPLATE\WYNLAB.SM.TEMPLATE\`
가 이미 이 용도로 준비되어 있다(DevExpress 참조, `System.Resources.Extensions`,
`CopyToModulesFolder` 빌드 타겟까지 전부 세팅됨 - 이 설정들을 손으로 다시 만들면 하나씩
빠뜨리기 쉽다).

1. `99.SOURCE\SM\TEMPLATE\` 폴더 전체를 `99.SOURCE\SM\EQUIP\`로 복사 (bin/obj 폴더는
   지우고 복사해도 됨 - 빌드하면 다시 생김)
2. 안의 `WYNLAB.SM.TEMPLATE.csproj` → `WYNLAB.SM.EQUIP.csproj`로 파일명 변경,
   `WYNLAB.SM.TEMPLATE\` 폴더 자체도 `WYNLAB.SM.EQUIP\`로 이름 변경
   (`99.SOURCE\SM\EQUIP\WYNLAB.SM.EQUIP\WYNLAB.SM.EQUIP.csproj` 형태가 되도록 -
   기존 CODE/MENU/USER 화면들과 폴더 구조를 맞춰야 `Deploy-Package.ps1`이 자동으로 찾는다)
3. `.csproj` 안의 `<RootNamespace>WYNLAB.SM.TEMPLATE</RootNamespace>` →
   `WYNLAB.SM.EQUIP`로 수정
4. `TemplateForm.cs`/`TemplateForm.Designer.cs`/`TemplateForm.resx` → `EquipListForm.cs`
   등으로 파일명 변경, 안의 `namespace WYNLAB.SM.TEMPLATE;` → `WYNLAB.SM.EQUIP;`,
   `class TemplateForm` → `class EquipListForm`으로 전부 바꾸기(이름 하나만 빠뜨려도
   컴파일 에러)
5. **`.sln` 파일 생성** - 템플릿 폴더엔 `.sln`이 없다. 리포지토리 루트에서:
   ```powershell
   Set-Location "99.SOURCE\SM\EQUIP"
   dotnet new sln -n WYNLAB.SM.EQUIP
   dotnet sln add "WYNLAB.SM.EQUIP\WYNLAB.SM.EQUIP.csproj"
   ```
   (기존 화면들의 `.sln`이 `WYNLAB.BaseForm.csproj`/`WYNLAB.Shared.csproj`까지 같이
   물고 있는 건 아니다 - `.csproj`의 `<ProjectReference>`가 알아서 그 두 프로젝트도
   같이 빌드하게 만든다.)

### 7-7. 클라이언트 - 화면 UI 작성

두 가지 검증된 구조 중 이 화면 성격에 맞는 쪽을 고른다:

- **CODE 화면 방식**(그리드 2개: 대분류 리스트 + 소분류 인라인편집 그리드) - 상세 데이터가
  그리드로 여러 건인 화면(예: 헤더+명세 구조)에 적합
- **MENU/USERGROUP 화면 방식**(좌측 리스트 + 우측 입력폼, 한 건씩 저장) - 단건 마스터
  데이터를 등록/수정하는 화면에 적합, 최근에 만든 화면들이 다 이 구조

어느 쪽이든 `BaseForm`을 상속하고 `QueryClick/NewClick/DeleteClick/SaveClick`을 override,
`ApiClient.GetAsync/PostAsync/PutAsync`로 7-4에서 만든 API를 호출하도록 작성한다
(1~2장에서 설명한 4단계 흐름 그대로).

### 7-8. 메뉴 등록 - SQL 아님, 메뉴관리 화면에서 직접

`TSMMENU`에 직접 INSERT 안 해도 된다 - **메뉴관리 화면(사이드바 "메뉴관리") 자체가
TSMMENU 등록 UI다.** 로그인 후 메뉴관리 열어서 "입력" → 아래 값 채우고 저장:

| 필드 | 값 예시 | 설명 |
|---|---|---|
| 메뉴코드 | `SM_EQUIP` | 7-4의 `RequireMenuPermission`, 화면의 `MenuCd`와 반드시 동일 |
| 메뉴명 | `설비관리` | 사이드바에 표시될 이름 |
| 상위메뉴코드 | (트리에서 선택) | 어느 그룹 밑에 넣을지 |
| 메뉴유형 | `FORM` | 화면이면 FORM, 그룹(폴더)이면 GROUP |
| **화면 클래스명** | `WYNLAB.SM.EQUIP.EquipListForm, WYNLAB.SM.EQUIP` | **`"네임스페이스.클래스명, 어셈블리명"`** 형식 - 콤마 뒤 어셈블리명은 csproj 파일명(확장자 제외)과 같아야 함 |
| 정렬순서 | `10` | 같은 그룹 내 표시 순서 |

저장 시 화면 클래스명이 실제로 존재하는지 자동 검증한다(`MenuListForm.SaveClick` 안의
`Type.GetType` 체크 - 이때 이미 클라이언트가 그 dll을 빌드해서 `Modules\` 폴더에 올려둔
상태여야 통과한다, 7-9 참고).

### 7-9. 로컬에서 빌드 + 실제 열어보기

```powershell
dotnet build "99.SOURCE\SM\EQUIP\WYNLAB.SM.EQUIP.sln" -c Debug
```
템플릿에 이미 들어있는 `CopyToModulesFolder` 빌드 타겟이 빌드 후 자동으로
`01.Client\WYNLAB.Shell\bin\Debug\net48\Modules\`에 dll/pdb를 복사해준다 - **손으로 옮길
필요 없음.** Shell을 다시 실행(F5 또는 exe 재실행)하고 로그인 → 사이드바에서 방금 등록한
메뉴를 더블클릭해서 화면이 뜨는지, 조회/저장이 실제로 되는지 확인한다.

admin 계정(`USER_TYPE='A'`)으로 테스트하면 `TSMMENUAUTH`에 권한을 아직 안 넣어도 메뉴가
보이고 모든 액션이 허용된다(관리자 우회) - 일반 계정으로 테스트하려면 권한등록 화면에서
`SM_EQUIP`에 대한 조회/입력/수정/삭제 권한을 그 계정(또는 소속 그룹)에 먼저 부여해야 한다.

### 7-10. 서버/클라이언트 배포

- **서버(API)**: `USP_SM_EQUIP_*` 프로시저를 운영 DB에도 반영(7-1) + API 재배포
  (`dotnet publish` → `D:\WYNLAB\Api\` 덮어쓰기 → 앱풀 재시작)
- **클라이언트(새 화면 모듈)**: `Deploy-Package.ps1`을 실행하면 **`99.SOURCE\SM\` 밑의
  모든 하위폴더에서 `.sln`을 자동으로 찾아서 빌드**하므로(`EQUIP\WYNLAB.SM.EQUIP.sln`도
  자동 포함됨 - 스크립트 수정 불필요), `_deploy\Modules\SM\WYNLAB.SM.EQUIP.dll`이
  생성된다. 그걸 서버 `D:\WYNLAB\Modules\SM\`에 붙여넣으면 끝 - ClickOnce 재게시는
  필요 없다(화면 모듈 dll은 애초에 ClickOnce 패키지에 안 들어가고 `ModuleLoader`가
  실행 중에 네트워크 공유 폴더에서 읽어온다).

### 7-11. 다음에 이 화면을 다시 손볼 때

1~6장에서 설명한 조회조건 추가/저장 파라미터 추가 절차를 그대로 `EquipListForm.cs` /
`EquipManageRepository.cs` / `USP_SM_EQUIP_Q`,`USP_SM_EQUIP_S`에 적용하면 된다 - 화면이
몇 개가 늘어나도 이 4단계 구조와 절차는 항상 동일하다.
