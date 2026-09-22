namespace WYNLAB.Api.Services.Integrations;

public class IntegrationRunResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int RowCount { get; set; }
}

/// <summary>연동 실행 시점에 이미 복호화된 접속정보 - 구현체는 DPAPI를 몰라도 된다
/// (ApiIntegrationRunner가 TSMAPIDEF에서 읽어 복호화한 뒤 넘겨준다).</summary>
public record ApiIntegrationDef(string ApiCd, string BaseUrl, string? AuthKey);

/// <summary>
/// 연동 하나당 구현체 하나(예: EximFxRateIntegration). 새 연동을 추가할 때 이 인터페이스만
/// 구현하고 Program.cs에 한 줄(AddScoped) 등록하면 된다 - 정의 저장/실행이력/화면은 전부
/// ApiIntegrationController/Runner가 공통으로 처리하므로 컨트롤러나 화면을 새로 만들 필요가 없다.
/// </summary>
public interface IExternalApiIntegration
{
    /// <summary>TSMAPIDEF.api_cd와 정확히 일치해야 한다 - 이 값으로 런너가 구현체를 찾는다.</summary>
    string ApiCd { get; }

    Task<IntegrationRunResult> RunAsync(ApiIntegrationDef def);
}
