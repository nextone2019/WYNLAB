using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Api.Services;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

/// <summary>연동 실행에 필요한 최소 정보 - 암호화된 키를 그대로 담아서 넘긴다(복호화는
/// ApiIntegrationRunner가 한다, 리포지토리는 DPAPI를 모른다).</summary>
public record ApiIntegrationRunDef(string ApiCd, string BaseUrl, byte[]? AuthKeyEnc);

public interface IApiIntegrationRepository
{
    Task<List<ApiIntegrationDto>> GetListAsync();
    Task<ApiIntegrationDto?> GetAsync(string apiCd);
    Task<ApiIntegrationRunDef?> GetRunDefAsync(string apiCd);
    Task<List<string>> GetDueApiCdsAsync();
    Task SaveAsync(string apiCd, SaveApiIntegrationRequest request, string userId, string pc);
    Task UpdateRunResultAsync(string apiCd, string resultCd, string message);
    Task InsertLogAsync(string apiCd, string resultCd, string message, int elapsedMs, int? rowCnt);
    Task<List<ApiIntegrationLogDto>> GetLogsAsync(string apiCd, int limit);
}

public class ApiIntegrationRepository : IApiIntegrationRepository
{
    private readonly IDapperContext _context;
    public ApiIntegrationRepository(IDapperContext context) => _context = context;

    private const string SelectListSql = @"
        SELECT api_cd AS ApiCd, api_nm AS ApiNm, base_url AS BaseUrl,
               CASE WHEN auth_key_enc IS NULL THEN 0 ELSE 1 END AS HasAuthKey,
               run_time AS RunTime, enabled_yn AS EnabledYn,
               last_run_dt AS LastRunDt, last_run_result_cd AS LastRunResultCd, last_run_msg AS LastRunMsg,
               description AS Description
        FROM TSMAPIDEF";

    public async Task<List<ApiIntegrationDto>> GetListAsync()
    {
        using var conn = _context.CreateConnection();
        var rows = await conn.QueryAsync<ApiIntegrationDto>($"{SelectListSql} ORDER BY api_cd");
        return rows.ToList();
    }

    public async Task<ApiIntegrationDto?> GetAsync(string apiCd)
    {
        using var conn = _context.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<ApiIntegrationDto>(
            $"{SelectListSql} WHERE api_cd = @apiCd", new { apiCd });
    }

    public async Task<ApiIntegrationRunDef?> GetRunDefAsync(string apiCd)
    {
        using var conn = _context.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<ApiIntegrationRunDef>(
            "SELECT api_cd AS ApiCd, base_url AS BaseUrl, auth_key_enc AS AuthKeyEnc FROM TSMAPIDEF WHERE api_cd = @apiCd",
            new { apiCd });
    }

    public async Task<List<string>> GetDueApiCdsAsync()
    {
        using var conn = _context.CreateConnection();
        // Windows 작업 스케줄러가 이 결과를 얻기 위해 run-due 엔드포인트를 주기적으로(예: 15분마다)
        // 두드리는 구조라, 정확히 run_time 그 순간이 아니라 "그 시각을 지난 뒤 첫 폴링"에 실행된다.
        var rows = await conn.QueryAsync<string>(
            @"SELECT api_cd FROM TSMAPIDEF
              WHERE enabled_yn = 'Y' AND run_time IS NOT NULL
                AND run_time <= CONVERT(varchar(5), GETDATE(), 108)
                AND (last_run_dt IS NULL OR CAST(last_run_dt AS date) < CAST(GETDATE() AS date))");
        return rows.ToList();
    }

    public async Task SaveAsync(string apiCd, SaveApiIntegrationRequest request, string userId, string pc)
    {
        using var conn = _context.CreateConnection();

        if (!string.IsNullOrWhiteSpace(request.NewAuthKey))
        {
            var encrypted = ApiKeyProtector.Protect(request.NewAuthKey);
            await conn.ExecuteAsync(
                @"UPDATE TSMAPIDEF SET api_nm=@ApiNm, base_url=@BaseUrl, auth_key_enc=@encrypted,
                     run_time=@RunTime, enabled_yn=@EnabledYn, description=@Description,
                     upt_user_id=@userId, upt_dt=GETDATE(), upt_pc=@pc
                  WHERE api_cd=@apiCd",
                new { request.ApiNm, request.BaseUrl, encrypted, request.RunTime, request.EnabledYn, request.Description, userId, pc, apiCd });
        }
        else
        {
            await conn.ExecuteAsync(
                @"UPDATE TSMAPIDEF SET api_nm=@ApiNm, base_url=@BaseUrl,
                     run_time=@RunTime, enabled_yn=@EnabledYn, description=@Description,
                     upt_user_id=@userId, upt_dt=GETDATE(), upt_pc=@pc
                  WHERE api_cd=@apiCd",
                new { request.ApiNm, request.BaseUrl, request.RunTime, request.EnabledYn, request.Description, userId, pc, apiCd });
        }
    }

    public async Task UpdateRunResultAsync(string apiCd, string resultCd, string message)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE TSMAPIDEF SET last_run_dt=GETDATE(), last_run_result_cd=@resultCd, last_run_msg=@message WHERE api_cd=@apiCd",
            new { apiCd, resultCd, message });
    }

    public async Task InsertLogAsync(string apiCd, string resultCd, string message, int elapsedMs, int? rowCnt)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(
            @"INSERT INTO TSMAPILOG (api_cd, run_dt, result_cd, message, elapsed_ms, row_cnt)
              VALUES (@apiCd, GETDATE(), @resultCd, @message, @elapsedMs, @rowCnt)",
            new { apiCd, resultCd, message, elapsedMs, rowCnt });
    }

    public async Task<List<ApiIntegrationLogDto>> GetLogsAsync(string apiCd, int limit)
    {
        using var conn = _context.CreateConnection();
        var rows = await conn.QueryAsync<ApiIntegrationLogDto>(
            @"SELECT TOP (@limit) log_id AS LogId, api_cd AS ApiCd, run_dt AS RunDt,
                     result_cd AS ResultCd, message AS Message, elapsed_ms AS ElapsedMs, row_cnt AS RowCnt
              FROM TSMAPILOG WHERE api_cd = @apiCd ORDER BY run_dt DESC",
            new { apiCd, limit });
        return rows.ToList();
    }
}
