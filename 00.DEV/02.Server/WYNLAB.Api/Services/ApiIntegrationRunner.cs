using System.Diagnostics;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Api.Services.Integrations;

namespace WYNLAB.Api.Services;

/// <summary>
/// api_cd로 알맞은 IExternalApiIntegration 구현체를 찾아 실행하고, 성공/실패와 무관하게 항상
/// TSMAPIDEF.last_run_*과 TSMAPILOG에 결과를 남긴다(2026-09-15) - 컨트롤러(수동 실행)와
/// run-due(작업 스케줄러 자동 실행) 양쪽에서 이 하나만 호출하면 된다.
/// </summary>
public interface IApiIntegrationRunner
{
    Task<IntegrationRunResult> RunAsync(string apiCd);
    Task<List<string>> RunDueAsync();
}

public class ApiIntegrationRunner : IApiIntegrationRunner
{
    private readonly IApiIntegrationRepository _repo;
    private readonly IEnumerable<IExternalApiIntegration> _integrations;
    private readonly ILogger<ApiIntegrationRunner> _logger;

    public ApiIntegrationRunner(IApiIntegrationRepository repo, IEnumerable<IExternalApiIntegration> integrations, ILogger<ApiIntegrationRunner> logger)
    {
        _repo = repo;
        _integrations = integrations;
        _logger = logger;
    }

    public async Task<IntegrationRunResult> RunAsync(string apiCd)
    {
        var sw = Stopwatch.StartNew();

        var integration = _integrations.FirstOrDefault(i => i.ApiCd == apiCd);
        if (integration == null)
        {
            var msg = $"'{apiCd}'에 대한 연동 구현이 등록되어 있지 않습니다.";
            await _repo.UpdateRunResultAsync(apiCd, "FAIL", msg);
            await _repo.InsertLogAsync(apiCd, "FAIL", msg, 0, null);
            return new IntegrationRunResult { Success = false, Message = msg };
        }

        var def = await _repo.GetRunDefAsync(apiCd);
        if (def == null)
        {
            var msg = "연동 정의를 찾을 수 없습니다.";
            return new IntegrationRunResult { Success = false, Message = msg };
        }

        IntegrationRunResult result;
        try
        {
            var authKey = def.AuthKeyEnc == null ? null : ApiKeyProtector.Unprotect(def.AuthKeyEnc);
            result = await integration.RunAsync(new ApiIntegrationDef(apiCd, def.BaseUrl, authKey));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ApiIntegration] {ApiCd} 실행 실패", apiCd);
            result = new IntegrationRunResult { Success = false, Message = ex.Message };
        }

        sw.Stop();
        var resultCd = result.Success ? "OK" : "FAIL";
        await _repo.UpdateRunResultAsync(apiCd, resultCd, result.Message);
        await _repo.InsertLogAsync(apiCd, resultCd, result.Message, (int)sw.ElapsedMilliseconds, result.RowCount);
        return result;
    }

    public async Task<List<string>> RunDueAsync()
    {
        var due = await _repo.GetDueApiCdsAsync();
        foreach (var apiCd in due)
            await RunAsync(apiCd);
        return due;
    }
}
