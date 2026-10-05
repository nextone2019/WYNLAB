using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories;

public interface IGenericDataRepository
{
    /// <summary>메뉴에 등록된 프로시저 접두사(TSMMENU.PROC_PREFIX). 없으면 null - 그 메뉴는
    /// 범용 통로를 쓰지 않는다는 뜻이다.</summary>
    Task<string?> GetProcPrefixAsync(long menuId);

    Task<DataQueryResponse> QueryAsync(string procName, Dictionary<string, string?> parameters);

    /// <summary>QueryAsync와 동일하지만 프로시저가 아니라 원본 SQL 텍스트를 그대로 실행한다 -
    /// LookUp관리(sysLookupM.source_type='Q')처럼 관리자가 프로시저 없이 쿼리문 자체를 등록해둔
    /// 경우에 쓴다. sqlText는 관리자가 저장해둔 값(같은 신뢰 수준의 텍스트, 프로시저 이름과
    /// 동급)이고, parameters는 QueryAsync와 똑같이 SqlParameter로만 바인딩되므로 값 자체가
    /// SQL로 해석될 걱정은 없다.</summary>
    Task<DataQueryResponse> QueryRawAsync(string sqlText, Dictionary<string, string?> parameters);

    Task<ProcResult> SaveAsync(string procName, Dictionary<string, string?> parameters, string userId, string? clientPc);
}

/// <summary>
/// 화면별 Repository를 만들지 않고 프로시저를 그대로 실행해주는 범용 통로.
/// 설계 배경과 보안 모델은 저장소 루트의 GENERIC_DATA_API.md 참고.
///
/// 이 클래스는 "무엇을 실행할지"를 판단하지 않는다 - 실행 가능 여부(권한/화이트리스트)는
/// DataController가 먼저 확인하고, 여기는 확인이 끝난 요청만 받아서 실행한다.
/// </summary>
public class GenericDataRepository : IGenericDataRepository
{
    private readonly IDapperContext _context;

    public GenericDataRepository(IDapperContext context) => _context = context;

    public async Task<string?> GetProcPrefixAsync(long menuId)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<string?>(
            "SELECT PROC_PREFIX FROM TSMMENU WHERE MENU_ID = @menuId", new { menuId });
    }

    public Task<DataQueryResponse> QueryAsync(string procName, Dictionary<string, string?> parameters) =>
        ExecuteQueryAsync(procName, CommandType.StoredProcedure, parameters);

    public Task<DataQueryResponse> QueryRawAsync(string sqlText, Dictionary<string, string?> parameters) =>
        ExecuteQueryAsync(sqlText, CommandType.Text, parameters);

    /// <summary>QueryAsync/QueryRawAsync가 공유하는 실행부 - 프로시저 이름이냐 SQL 텍스트냐(CommandType)
    /// 차이만 있고 나머지는 완전히 같다.
    ///
    /// Dapper의 QueryMultipleAsync/GridReader.ReadAsync는 결과셋이 0건이면 컬럼 이름 자체를
    /// 알 방법이 없다(동적 행을 하나도 안 만드므로) - 소분류가 하나도 없는 대분류를 조회하면
    /// Tables가 컬럼 정보 없는 빈 결과가 되어, 그리드가 스키마 없는 DataTable에 바인딩된다.
    /// 그 상태에서 새 행을 추가해 값을 입력해도 실제로는 어느 컬럼에도 붙지 않아 포커스를
    /// 옮기면 그대로 사라졌다(실제로 겪음 - frmMinorCode 소분류 그리드). SqlDataReader를 직접
    /// 써서 FieldCount/GetName으로 행 개수와 무관하게 항상 컬럼 스키마를 얻는다.</summary>
    private async Task<DataQueryResponse> ExecuteQueryAsync(string commandText, CommandType commandType, Dictionary<string, string?> parameters)
    {
        using var conn = (SqlConnection)_context.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = commandText;
        cmd.CommandType = commandType;

        // 조회 조건은 비어있으면 "조건 없음"(NULL)이 맞다 - 숫자형 필터를 공백으로 두면 0으로
        // 걸러서 결과가 0건이 되는 게 더 이상하다(SaveAsync와 반대 이유 - numericBlankAsZero: false).
        var paramTypes = await GetParamTypesAsync(conn, commandText);
        foreach (var kv in parameters.Where(kv => kv.Key.StartsWith("p_", StringComparison.OrdinalIgnoreCase)))
        {
            var value = NormalizeBlankValue(kv.Key, kv.Value, paramTypes, numericBlankAsZero: false);
            cmd.Parameters.AddWithValue(kv.Key, (object?)value ?? DBNull.Value);
        }

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        var response = new DataQueryResponse();
        do
        {
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            var rows = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>();
                for (var i = 0; i < columns.Count; i++)
                    row[columns[i]] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                rows.Add(row);
            }
            response.Tables.Add(new DataTableResult { Columns = columns, Rows = rows });
        } while (await reader.NextResultAsync());

        return response;
    }

    public async Task<ProcResult> SaveAsync(string procName, Dictionary<string, string?> parameters, string userId, string? clientPc)
    {
        using var conn = (SqlConnection)_context.CreateConnection();

        var paramTypes = await GetParamTypesAsync(conn, procName);
        var p = BuildParameters(parameters, paramTypes);

        // 사용자/PC는 화면이 보내는 값을 믿지 않고 서버가 직접 채운다 - 클라이언트가 남의 아이디로
        // 등록이력을 남길 수 있으면 안 된다. 화면이 같은 이름을 보내도 여기서 덮어쓴다.
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);

        // 새 프로시저 표준(@p_work_type 계열)은 표준 출력 이름이 PascalCase다 - ProcResult의
        // AddStandardOutputs 주석 참고. 범용 통로는 이 새 표준을 쓰는 프로시저만 대상으로 한다.
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync(procName, p, commandType: CommandType.StoredProcedure);

        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    /// <summary>
    /// 화면이 보낸 파라미터를 Dapper 파라미터로 바꾼다. "p_"로 시작하는 것만 통과시키는 이유는
    /// 표준 출력 파라미터(@ReturnCode 등)를 클라이언트가 덮어쓰지 못하게 하기 위함이다 -
    /// 그걸 허용하면 실패한 저장을 성공으로 보이게 만들 수 있다.
    ///
    /// 값이 SQL로 해석될 걱정은 없다. 프로시저 이름과 파라미터 이름은 서버가 검증하고, 값은
    /// SQL 텍스트가 아니라 파라미터 값으로 전달되기 때문이다(SSP_CBO_CODE_Q의 where 파라미터와
    /// 같은 원리 - MinorCodeManageRepository.GetLookupAsync 주석 참고).
    /// </summary>
    private static DynamicParameters BuildParameters(Dictionary<string, string?> parameters, Dictionary<string, string> paramTypes)
    {
        var p = new DynamicParameters();

        foreach (var kv in parameters)
        {
            if (!kv.Key.StartsWith("p_", StringComparison.OrdinalIgnoreCase)) continue;
            // 저장 값이 비어있으면 숫자형(NUMERIC/DECIMAL류)은 0으로, ID/날짜 같은 나머지
            // 비문자 타입은 NULL로 채운다(NormalizeBlankValue 주석 참고 - numericBlankAsZero: true).
            p.Add(kv.Key, NormalizeBlankValue(kv.Key, kv.Value, paramTypes, numericBlankAsZero: true));
        }

        return p;
    }

    /// <summary>프로시저의 실제 입력 파라미터 타입(sys.parameters/sys.types)을 한 번 조회해서
    /// {파라미터명(@ 뗀 것) -> SQL 타입이름} 맵으로 돌려준다. 원본 SQL 텍스트(QueryRawAsync)를
    /// 넘기면 OBJECT_ID가 못 찾아 빈 맵이 되고, 그러면 NormalizeBlankValue는 아무것도 안 바꾼다 -
    /// 프로시저 경로에서만 이 보정이 적용된다(일부러, 안전한 쪽으로).</summary>
    private static async Task<Dictionary<string, string>> GetParamTypesAsync(SqlConnection conn, string procName)
    {
        var rows = await conn.QueryAsync<(string Name, string TypeName)>(
            @"SELECT p.name AS Name, t.name AS TypeName
              FROM sys.parameters p
              JOIN sys.types t ON t.user_type_id = p.user_type_id
              WHERE p.object_id = OBJECT_ID(@procName)",
            new { procName });

        return rows.ToDictionary(r => r.Name.TrimStart('@'), r => r.TypeName, StringComparer.OrdinalIgnoreCase);
    }

    private static readonly HashSet<string> ZeroDefaultTypes = new(StringComparer.OrdinalIgnoreCase)
        { "numeric", "decimal", "float", "real", "money", "smallmoney" };

    private static readonly HashSet<string> NullDefaultNonStringTypes = new(StringComparer.OrdinalIgnoreCase)
        { "int", "bigint", "smallint", "tinyint", "bit", "datetime", "datetime2", "smalldatetime", "date", "uniqueidentifier" };

    /// <summary>
    /// 화면이 빈 문자열("")을 보내면, 프로시저 파라미터가 실제로 문자열이 아닌 타입일 때 SQL이
    /// ''를 그 타입으로 못 바꿔서 그대로 죽는다(예: NUMERIC(9,4) 자리에 빈 문자열 - "데이터 형식
    /// nvarchar을(를) numeric(으)로 변환하는 중 오류" - 2026-10-01 수주등록 부가세율 공백 저장
    /// 실제로 겪음). 범용 통로(ProcData/GenericDataRepository)를 쓰는 모든 화면이 공통으로 겪을 수
    /// 있는 문제라 화면마다 NullIfEmpty를 안 챙겨도 여기서 한 번에 막는다:
    ///   - 숫자형(NUMERIC/DECIMAL/FLOAT/REAL/MONEY) 이고 저장(numericBlankAsZero=true)이면 "0" -
    ///     부가세율/단가/수량 같은 값이 비어있으면 0이 맞다는 사장님 판단(2026-10-01).
    ///   - 그 외 비문자 타입(INT/BIGINT 등 ID, DATETIME, BIT)은 NULL - 거래처ID 같은 FK를
    ///     비워뒀다고 0(존재할 수 있는 다른 행의 키)으로 채우면 더 위험하다.
    ///   - 조회/검색 조건(numericBlankAsZero=false)에서는 숫자형도 NULL - 금액 조건을 비웠다고
    ///     "0으로 필터링"하면 결과가 0건이 되는 게 더 이상하다, "조건 없음"이 맞다.
    ///   - 문자열 타입(VARCHAR/NVARCHAR 등)과 타입을 못 찾은 파라미터(원본 SQL 텍스트 등)는
    ///     손대지 않는다 - 지금까지의 동작 그대로.
    /// </summary>
    private static string? NormalizeBlankValue(string paramName, string? value, Dictionary<string, string> paramTypes, bool numericBlankAsZero)
    {
        if (!string.IsNullOrWhiteSpace(value)) return value;
        if (!paramTypes.TryGetValue(paramName, out var typeName)) return value;

        if (ZeroDefaultTypes.Contains(typeName)) return numericBlankAsZero ? "0" : null;
        if (NullDefaultNonStringTypes.Contains(typeName)) return null;
        return value;
    }
}
