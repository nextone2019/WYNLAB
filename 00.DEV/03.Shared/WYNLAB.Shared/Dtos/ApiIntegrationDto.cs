namespace WYNLAB.Shared.Dtos;

public class ApiIntegrationDto
{
    public string ApiCd { get; set; } = string.Empty;
    public string ApiNm { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public bool HasAuthKey { get; set; }
    public string? RunTime { get; set; }
    public string EnabledYn { get; set; } = "Y";
    public DateTime? LastRunDt { get; set; }
    public string? LastRunResultCd { get; set; }
    public string? LastRunMsg { get; set; }
    public string? Description { get; set; }
}

public class SaveApiIntegrationRequest
{
    public string ApiNm { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>비어있으면 기존 인증키를 그대로 유지한다 - 화면이 저장된 키를 다시 보여줄 수
    /// 없으므로(마스킹), "바꿀 때만 입력"하는 방식으로만 갱신한다.</summary>
    public string? NewAuthKey { get; set; }

    public string? RunTime { get; set; }
    public string EnabledYn { get; set; } = "Y";
    public string? Description { get; set; }
}

public class ApiIntegrationLogDto
{
    public long LogId { get; set; }
    public string ApiCd { get; set; } = string.Empty;
    public DateTime RunDt { get; set; }
    public string ResultCd { get; set; } = string.Empty;
    public string? Message { get; set; }
    public int? ElapsedMs { get; set; }
    public int? RowCnt { get; set; }
}
