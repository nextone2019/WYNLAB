using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

public interface IScreenBuilderRepository
{
    Task<DescribeProcResultDto> DescribeUspProcAsync(string procName, string? workType);

    /// <summary>describe-proc과 달리 레코드셋을 전부(2개 이상도) 읽는다 - 실제로 실행하고
    /// 끝나면 무조건 롤백한다. AI Builder가 "이 프로시저가 결과셋을 몇 개, 어떤 컬럼으로
    /// 반환하는지" 미리 보고 각 레코드셋을 화면의 어느 컨트롤(그리드/폼)에 바인딩할지 고르는 데 쓴다.</summary>
    Task<DescribeProcMultiResultDto> DescribeUspProcMultiAsync(string procName, string workType);
}

/// <summary>
/// AI Builder(화면개발 자동화, SYS_AI_BUILDER) 전용 - PopupLookupRepository.DescribeProcAsync와
/// 같은 기법(sys.parameters로 파라미터 구조만 읽음)을 USP_* 프로시저에 맞게 일반화한 것.
///
/// 결과셋 구조는 sys.dm_exec_describe_first_result_set(정적 분석)가 아니라
/// DescribeUspProcMultiAsync(실제 실행 후 롤백)를 그대로 재사용한다 - 정적 분석은 IF/ELSE
/// 분기마다 SELECT 개수/모양이 다른 프로시저(예: USP_BA_CUST_Q의 Q=레코드셋 1개, Q1=2개)를
/// 정적으로 못 풀어내고 name/system_type_name이 둘 다 NULL인 행 하나만 돌려준다(2026-09-04
/// 실제 발견 - Master Columns 미리보기가 항상 빈 상태로 남던 원인). 실제 실행은 항상 롤백되므로
/// "구조만 살짝 들여다본다"는 전제는 그대로 유지된다.
/// </summary>
public class ScreenBuilderRepository : IScreenBuilderRepository
{
    private readonly IDapperContext _context;

    public ScreenBuilderRepository(IDapperContext context) => _context = context;

    public async Task<DescribeProcResultDto> DescribeUspProcAsync(string procName, string? workType)
    {
        // workType이 없으면(저장프로시저 파라미터만 볼 때) 결과셋 자체를 안 읽는다 - N/U/D 분기는
        // 보통 SELECT 없이 OUTPUT 파라미터만 채우므로 describe할 결과셋이 없는 게 정상.
        if (!string.IsNullOrWhiteSpace(workType))
        {
            var multi = await DescribeUspProcMultiAsync(procName, workType);
            return new DescribeProcResultDto
            {
                Columns = multi.ResultSets.FirstOrDefault()?.Columns ?? new List<ProcColumnInfoDto>(),
                Params = multi.Params
            };
        }

        using var conn = _context.CreateConnection();
        var paramList = await LoadParamsAsync(conn, procName);
        return new DescribeProcResultDto { Columns = new List<ProcColumnInfoDto>(), Params = ToParamDtos(paramList) };
    }

    public async Task<DescribeProcMultiResultDto> DescribeUspProcMultiAsync(string procName, string workType)
    {
        using var conn = (SqlConnection)_context.CreateConnection();
        await conn.OpenAsync();

        var paramList = await LoadParamsAsync(conn, procName);
        var paramDtos = ToParamDtos(paramList);

        using var tran = conn.BeginTransaction();
        var resultSets = new List<ProcResultSetInfoDto>();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tran;
            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var p in paramList)
            {
                var isWorkType = string.Equals(p.name, "@p_work_type", StringComparison.OrdinalIgnoreCase);
                cmd.Parameters.Add(new SqlParameter(p.name, (object?)(isWorkType ? workType : null) ?? DBNull.Value));
            }

            using var reader = await cmd.ExecuteReaderAsync();
            var idx = 0;
            do
            {
                var columns = new List<ProcColumnInfoDto>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var sqlType = reader.GetDataTypeName(i);
                    columns.Add(new ProcColumnInfoDto
                    {
                        ColumnNm = reader.GetName(i),
                        SqlType = sqlType,
                        SuggestedControlType = sqlType.StartsWith("date", StringComparison.OrdinalIgnoreCase) ? "DATE" : "TEXT"
                    });
                }
                resultSets.Add(new ProcResultSetInfoDto { Index = idx++, Columns = columns });
            } while (await reader.NextResultAsync());
        }
        finally
        {
            // 성공/실패 무관하게 항상 롤백 - "구조만 살짝 들여다본다"는 이 API의 전제를
            // 실행 뒤에도 지키기 위함(Q 분기가 혹시라도 쓰기를 하더라도 되돌린다).
            try { tran.Rollback(); } catch { /* 연결이 이미 끊겼으면 롤백 자체가 무의미 - 무시 */ }
        }

        return new DescribeProcMultiResultDto { Params = paramDtos, ResultSets = resultSets };
    }

    private static async Task<List<(string name, string type_name)>> LoadParamsAsync(IDbConnection conn, string procName)
    {
        var rows = await conn.QueryAsync<(string name, string type_name)>(
            @"SELECT p.name, TYPE_NAME(p.user_type_id) AS type_name
              FROM sys.parameters p
              JOIN sys.objects o ON o.object_id = p.object_id
              WHERE o.name = @procName AND p.is_output = 0
              ORDER BY p.parameter_id",
            new { procName });
        return rows.ToList();
    }

    /// <summary>@p_xxx -> xxx. AI Builder(ScreenTemplateGenerator)가 생성 코드에서 파라미터명 앞에
    /// 항상 "p_"를 직접 붙이므로(예: $"p_{unmapped}") 여기서 "p_"까지 미리 떼어내지 않으면
    /// "p_p_xxx"로 중복 생성된다(2026-09-04 실제 발견 - 이미 생성된 frmItemGrp.cs 등에도 이
    /// 버그가 있었다). work_type/user_id/client_pc 제외 검사도 "p_" 없는 이름과 비교하므로
    /// 마찬가지로 이걸 떼어내야 정상 동작한다.</summary>
    private static List<ProcParamInfoDto> ToParamDtos(List<(string name, string type_name)> paramList) =>
        paramList.Select(p => new ProcParamInfoDto
        {
            ParamNm = StripPPrefix(p.name.TrimStart('@')),
            SqlType = p.type_name,
            SuggestedControlType = p.type_name.StartsWith("date", StringComparison.OrdinalIgnoreCase) ? "DATE" : "TEXT"
        }).ToList();

    private static string StripPPrefix(string name) =>
        name.StartsWith("p_", StringComparison.OrdinalIgnoreCase) ? name.Substring(2) : name;
}
