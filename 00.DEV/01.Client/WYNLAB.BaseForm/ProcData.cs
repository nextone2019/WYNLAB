using System.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 범용 데이터 통로(api/data/*)로 저장프로시저를 부르는 헬퍼. 설계 배경은 저장소 루트의
/// GENERIC_DATA_API.md 참고.
///
/// 화면은 BaseForm의 QueryAsync/SaveAsync를 쓰면 되고(메뉴ID가 자동으로 채워짐), 이 클래스를
/// 직접 부를 일은 BaseForm을 상속하지 않는 곳(팝업 등)뿐이다.
/// </summary>
public static class ProcData
{
    /// <summary>
    /// 조회 프로시저를 실행하고 첫 번째 결과셋을 DataTable로 돌려준다.
    /// 결과셋이 여럿인 프로시저는 QueryMultiAsync를 쓴다.
    /// </summary>
    public static async Task<DataTable> QueryAsync(long menuId, string procName, object? parameters = null)
    {
        var tables = await QueryMultiAsync(menuId, procName, parameters);
        return tables.Count > 0 ? tables[0] : new DataTable();
    }

    /// <summary>결과셋을 여러 개 돌려주는 프로시저용(예: 대분류+소분류를 한 번에 조회).</summary>
    public static async Task<List<DataTable>> QueryMultiAsync(long menuId, string procName, object? parameters = null)
    {
        var callParams = ToParams(parameters);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var response = await ApiClient.PostAsync<DataRequest, DataQueryResponse>("api/data/query", new DataRequest
            {
                MenuId = menuId,
                ProcName = procName,
                Params = callParams
            });

            ApiCallLog.Record(menuId.ToString(), procName, callParams, success: true, message: null, stopwatch.ElapsedMilliseconds);
            return (response?.Tables ?? new()).Select(ToDataTable).ToList();
        }
        catch (Exception ex)
        {
            ApiCallLog.Record(menuId.ToString(), procName, callParams, success: false, message: ex.Message, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// 저장/삭제 프로시저를 실행한다. 어떤 권한이 필요한지는 서버가 p_work_type 값을 보고
    /// 판단하므로(N/U/D), 화면은 그 값을 파라미터에 담아 보내기만 하면 된다.
    /// p_user_id/p_client_pc는 서버가 직접 채우므로 화면에서 보낼 필요가 없다.
    /// </summary>
    public static async Task<ApiResult> SaveAsync(long menuId, string procName, object? parameters = null)
    {
        var callParams = ToParams(parameters);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var result = await ApiClient.PostAsync<DataRequest, ApiResult>("api/data/save", new DataRequest
            {
                MenuId = menuId,
                ProcName = procName,
                Params = callParams
            }) ?? new ApiResult { Success = false, Message = "서버 응답을 받지 못했습니다." };

            ApiCallLog.Record(menuId.ToString(), procName, callParams, result.Success, result.Message, stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            ApiCallLog.Record(menuId.ToString(), procName, callParams, success: false, message: ex.Message, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// DataRow의 한 컬럼 값을 지정한 버전(Current/Original)으로 안전하게 문자열로 읽는다.
    /// 없는 컬럼이거나 NULL이면 C# null(빈 문자열 아님).
    ///
    /// 버전을 반드시 명시해야 하는 이유: DataRow의 값 없는 인덱서(row[컬럼])는
    /// DataRowVersion.Default를 쓰는데, Deleted 상태 행에서 이걸 읽으면 "Current가 없다"는
    /// 이유로 DeletedRowInaccessibleException이 난다(실제로 확인함 - Original로도 안 넘어가고
    /// 그냥 예외가 난다). 그리드에서 지운 행의 원래 키 값을 읽어 서버에 삭제 요청을 보내야 하는
    /// 행 단위 저장 화면에서는 이 버전 인자를 빼먹으면 곧바로 예외로 죽는다.
    ///
    /// NULL을 빈 문자열("")이 아니라 null로 돌려주는 이유(2026-09-07 변경 - 원래는 ""였음):
    /// 이 결과가 그대로 SaveAsync의 파라미터 딕셔너리 값(p_xxx)으로 들어가는데, 서버가 문자열
    /// 값을 항상 NVARCHAR로 SqlParameter에 바인딩한다(GenericDataRepository.BuildParameters -
    /// 대상 프로시저 파라미터의 실제 SQL 타입을 조회하지 않음). 그 결과 컬럼이 NULL인 채로 "" 를
    /// 보내면, 저장프로시저의 BIGINT/DATETIME 파라미터에 ""를 바인딩하려다 프로시저 본문(BEGIN
    /// TRY)에 들어가기도 전에 "nvarchar을(를) numeric(으)로 변환하는 중 오류" SqlException으로
    /// 그대로 죽는다(2026-09-07 실제 발견 - frmItem333의 dept_id를 비워두고 저장). null이면
    /// Dapper가 DBNull로 보내서 그 파라미터의 기본값(NULL)이 그대로 적용된다. 호출부는 이미 전부
    /// Dictionary&lt;string, string?&gt;에 담으므로(널 허용) 이 반환 타입 변경으로 깨지는 곳은 없다.
    /// </summary>
    public static string? Str(DataRow row, string columnName, DataRowVersion version)
    {
        if (!row.Table.Columns.Contains(columnName)) return null;

        var value = row[columnName, version];
        return value == DBNull.Value ? null : Convert.ToString(value);
    }

    /// <summary>
    /// 익명 객체(new { p_work_type = "Q", p_major_cd = "CM0001" })를 파라미터 딕셔너리로 바꾼다.
    /// 화면 코드에서 딕셔너리를 직접 만들면 중괄호가 겹쳐 읽기 나빠서, 익명 객체를 받는다.
    ///
    /// 관리항목1~10처럼 파라미터 이름을 반복문으로 만들어야 하는 화면(frmMinorCode.SaveClick 등)은
    /// 익명 객체로 표현할 수 없어 Dictionary&lt;string, string?&gt;를 직접 만들어 넘긴다 - 이 경우는
    /// 이미 원하는 모양이므로 반사(reflection) 없이 그대로 돌려준다. 반사 경로로 잘못 흘려보내면
    /// Dictionary의 인덱서 프로퍼티(this[key])까지 GetProperties()에 잡혀서, 인덱스 없이
    /// GetValue()를 부르다 TargetParameterCountException("매개 변수의 개수가 일치하지 않습니다")이
    /// 난다(실제로 겪음 - 대분류 저장 시 저장 실패).
    ///
    /// 값은 전부 문자열로 보낸다 - 숫자/날짜 변환은 프로시저 파라미터 타입이 알아서 하고,
    /// JSON에서 타입을 섞으면 오히려 어긋날 여지가 생긴다(DataRequest.Params 주석 참고).
    /// bool은 'Y'/'N'으로 바꾼다 - DB의 VARCHAR(1) 사용여부 컬럼 규약에 맞추기 위함이다.
    /// </summary>
    private static Dictionary<string, string?> ToParams(object? parameters)
    {
        if (parameters is Dictionary<string, string?> dict) return new Dictionary<string, string?>(dict);

        var result = new Dictionary<string, string?>();
        if (parameters == null) return result;

        foreach (var prop in parameters.GetType().GetProperties())
        {
            var value = prop.GetValue(parameters);
            result[prop.Name] = value switch
            {
                null => null,
                bool b => b ? "Y" : "N",
                DateTime d => d.ToString("yyyy-MM-dd HH:mm:ss"),
                _ => value.ToString()
            };
        }

        return result;
    }

    /// <summary>
    /// 서버가 돌려준 결과셋을 DataTable로 바꾼다. 그리드가 바인딩할 수 있어야 하고, 컬럼이
    /// 디자이너에 FieldName 문자열로 잡혀 있으므로 컬럼 이름만 맞으면 그대로 표시된다.
    ///
    /// 컬럼은 반드시 Rows가 아니라 Columns(서버가 행 개수와 무관하게 항상 채워줌)에서 만든다 -
    /// 행이 0건일 때 Rows에서 컬럼 이름을 유추하면(예전 방식) 스키마 없는 DataTable이 되어,
    /// 그리드에 새 행을 추가해 입력해도 어느 필드에도 안 붙어 포커스를 옮기면 그대로
    /// 사라지는 문제가 있었다(실제로 겪음 - 소분류가 없는 대분류를 조회한 뒤 소분류 그리드에
    /// 입력). 컬럼 타입은 전부 object로 두는 이유: 행마다 타입이 다르거나 NULL이어도 그리드가
    /// 표시할 때 알아서 처리하기 때문이다.
    ///
    /// 끝에서 AcceptChanges()를 반드시 호출한다 - 안 하면 방금 서버에서 막 받아온 행들도
    /// DataRowState.Added로 잡혀서, 그리드에서 아무것도 안 건드려도 저장 시 "전부 신규 행"으로
    /// 오인해 이미 있는 데이터까지 다시 INSERT하려 든다(실제로 겪음 - 조회 직후 저장하면 PK
    /// 중복 오류가 났다). AcceptChanges 이후에야 방금 로드한 행은 Unchanged, 그 다음 사용자가
    /// 그리드에서 실제로 편집/추가/삭제한 행만 Modified/Added/Deleted로 구분된다.
    /// </summary>
    /// <summary>popPopUp처럼 BaseForm을 상속하지 않아 menuId 기반 QueryAsync를 못 쓰는
    /// 곳(api/lookups/* 같은 별도 엔드포인트를 직접 호출)도 이 변환 로직만은 그대로 재사용할 수
    /// 있도록 internal로 연다 - JsonElement 언래핑을 빠뜨리면 그리드 정렬/검색이 조용히 깨진다
    /// (아래 UnwrapJsonValue 설명 참고).</summary>
    internal static DataTable ToDataTable(DataTableResult source)
    {
        var table = new DataTable();
        foreach (var columnName in source.Columns)
            table.Columns.Add(columnName, typeof(object));

        foreach (var row in source.Rows)
        {
            var dataRow = table.NewRow();
            foreach (var kv in row)
            {
                if (!table.Columns.Contains(kv.Key)) continue;
                dataRow[kv.Key] = UnwrapJsonValue(kv.Value);
            }
            table.Rows.Add(dataRow);
        }

        table.AcceptChanges();
        return table;
    }

    /// <summary>
    /// object 타입 컬럼에 담긴 값을 실제 값으로 풀어준다. System.Text.Json은 object로 선언된
    /// 자리(Dictionary&lt;string, object?&gt;의 값 등)를 역직렬화할 때 원시 타입(string/long/bool 등)이
    /// 아니라 JsonElement로 박스에 담아서 돌려준다 - 그대로 DataTable 셀에 넣으면 겉보기엔
    /// 멀쩡히 표시되지만(ToString이 값처럼 보이는 텍스트를 내놓으므로), 그 값으로 하는 다른
    /// 작업은 전부 깨진다: 헤더 클릭 정렬이 반응 없음(JsonElement가 IComparable을 구현 안 해서
    /// 비교 자체가 안 됨), GridView.LocateByValue가 항상 못 찾음(JsonElement.Equals(string)이
    /// 절대 true가 안 됨 - frmMinorCode 포커스 복원에서 실제로 겪음) 등. 컬럼 타입을 여전히
    /// object로 두는 이유(ToDataTable 설명 참고)는 그대로 두되, 그 안에 들어가는 실제 값만
    /// 진짜 CLR 원시 타입으로 풀어서 넣는다 - 이러면 그리드가 하는 모든 값 기반 동작(정렬,
    /// 검색, 합계, LocateByValue)이 예전처럼 정상 동작한다.
    /// </summary>
    private static object UnwrapJsonValue(object? value)
    {
        if (value is not System.Text.Json.JsonElement element) return value ?? DBNull.Value;

        return element.ValueKind switch
        {
            System.Text.Json.JsonValueKind.Null or System.Text.Json.JsonValueKind.Undefined => DBNull.Value,
            System.Text.Json.JsonValueKind.String => (object?)element.GetString() ?? DBNull.Value,
            System.Text.Json.JsonValueKind.True => true,
            System.Text.Json.JsonValueKind.False => false,
            System.Text.Json.JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            _ => element.GetRawText()
        };
    }
}
