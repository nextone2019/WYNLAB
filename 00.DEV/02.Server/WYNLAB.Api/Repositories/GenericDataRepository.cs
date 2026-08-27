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
    Task<string?> GetProcPrefixAsync(string menuCd);

    Task<DataQueryResponse> QueryAsync(string procName, Dictionary<string, string?> parameters);

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

    public async Task<string?> GetProcPrefixAsync(string menuCd)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<string?>(
            "SELECT PROC_PREFIX FROM TSMMENU WHERE MENU_CD = @menuCd", new { menuCd });
    }

    public async Task<DataQueryResponse> QueryAsync(string procName, Dictionary<string, string?> parameters)
    {
        // Dapper의 QueryMultipleAsync/GridReader.ReadAsync는 결과셋이 0건이면 컬럼 이름 자체를
        // 알 방법이 없다(동적 행을 하나도 안 만드므로) - 소분류가 하나도 없는 대분류를 조회하면
        // Tables가 컬럼 정보 없는 빈 결과가 되어, 그리드가 스키마 없는 DataTable에 바인딩된다.
        // 그 상태에서 새 행을 추가해 값을 입력해도 실제로는 어느 컬럼에도 붙지 않아 포커스를
        // 옮기면 그대로 사라졌다(실제로 겪음 - frmMinorCode 소분류 그리드). SqlDataReader를 직접
        // 써서 FieldCount/GetName으로 행 개수와 무관하게 항상 컬럼 스키마를 얻는다.
        using var conn = (SqlConnection)_context.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = procName;
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (var kv in parameters.Where(kv => kv.Key.StartsWith("p_", StringComparison.OrdinalIgnoreCase)))
            cmd.Parameters.AddWithValue(kv.Key, (object?)kv.Value ?? DBNull.Value);

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
        using var conn = _context.CreateConnection();

        var p = BuildParameters(parameters);

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
    private static DynamicParameters BuildParameters(Dictionary<string, string?> parameters)
    {
        var p = new DynamicParameters();

        foreach (var kv in parameters)
        {
            if (!kv.Key.StartsWith("p_", StringComparison.OrdinalIgnoreCase)) continue;
            p.Add(kv.Key, kv.Value);
        }

        return p;
    }
}
