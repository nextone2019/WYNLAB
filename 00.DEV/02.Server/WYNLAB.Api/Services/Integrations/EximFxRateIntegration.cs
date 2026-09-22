using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Services.Integrations;

/// <summary>
/// 한국수출입은행 환율 Open API(exchangeJSON) 연동 - 외부 API 연동 프레임워크(144번 마이그레이션)의
/// 첫 파일럿(2026-09-15). 인증키는 은행 홈페이지에서 설치 현장마다 무료 발급받아 frmApiIntegration
/// 화면에 등록한다(현장마다 자기 키를 쓰는 패턴 - project_wynlab_zipcode_api_backlog와 동일).
/// 영업일 11시경 갱신되는 일별 데이터라 하루 한 번(TSMAPIDEF.run_time)이면 충분하다.
///
/// 저장 대상은 TBAEXCRATE(BA 모듈, 사용자가 직접 설계/생성 - 2026-09-15) - 처음엔 이 클래스가
/// 만든 TSMEXRATE(프레임워크 예시용 임시 테이블)에 넣었지만, 실제 업무에서 쓸 진짜 테이블은
/// BA 담당자가 따로 설계했으므로 그쪽으로 갈아탔다(145번 마이그레이션에서 TSMEXRATE는 제거).
/// 날짜 컬럼은 yyyyMMdd 관례(project_wynlab_yyyymmdd_date_convention)에 따라 VARCHAR(8).
/// </summary>
public class EximFxRateIntegration : IExternalApiIntegration
{
    public string ApiCd => "EXIM_FX";

    private readonly IHttpClientFactory _httpFactory;
    private readonly IDapperContext _db;

    public EximFxRateIntegration(IHttpClientFactory httpFactory, IDapperContext db)
    {
        _httpFactory = httpFactory;
        _db = db;
    }

    public async Task<IntegrationRunResult> RunAsync(ApiIntegrationDef def)
    {
        if (string.IsNullOrWhiteSpace(def.AuthKey))
            return new IntegrationRunResult { Success = false, Message = "인증키가 등록되어 있지 않습니다." };

        var today = DateTime.Now.ToString("yyyyMMdd");
        var url = $"{def.BaseUrl}?authkey={Uri.EscapeDataString(def.AuthKey)}&searchdate={today}&data=AP01";

        var client = _httpFactory.CreateClient();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var rates = JsonSerializer.Deserialize<List<EximRateItem>>(json) ?? new();

        // result는 문자열이 아니라 숫자로 온다(실제 응답 확인 후 수정, 2026-09-15 - "The JSON value
        // could not be converted to System.String" 오류로 발견). 1=정상, 2=DATA코드 오류,
        // 3=인증코드 오류, 4=일별 제한횟수 마감 - 은행 문서 기준. 주말/공휴일 등 휴장일에는 빈
        // 배열이 오므로 그건 "실패"가 아니라 "오늘은 데이터가 없다"로 취급한다(휴장일마다 관리자가
        // 실패 알림을 받으면 소음이 됨) - 반면 응답은 있는데 전부 result!=1이면 인증키/요청 자체의
        // 오류이므로 실패로 남긴다.
        if (rates.Count == 0)
            return new IntegrationRunResult { Success = true, Message = "오늘자 환율 데이터가 없습니다(휴장일 등).", RowCount = 0 };

        var validRates = rates.Where(r => r.Result == 1).ToList();
        if (validRates.Count == 0)
        {
            var errMsg = rates[0].Result switch
            {
                2 => "DATA코드 오류입니다.",
                3 => "인증키가 올바르지 않습니다.",
                4 => "일별 조회 제한 횟수를 초과했습니다.",
                _ => $"알 수 없는 오류입니다(result={rates[0].Result}).",
            };
            return new IntegrationRunResult { Success = false, Message = errMsg };
        }

        using var conn = _db.CreateConnection();

        // 화면의 "환율정보수신" 확인창이 "이미 등록된 환율정보가 있다면 초기화 후 재수신됩니다"라고
        // 안내하는 대로, 그날치를 UPSERT가 아니라 통째로 지우고 새로 넣는다(2026-09-15 - 처음엔
        // MERGE로 UPDATE했는데, 그러면 이미 있던 행의 등록일시(reg_dt)가 그대로 남아 "수신했는데
        // 등록일시가 안 바뀐다"는 지적을 받았다 - 재수신 = 그 시점에 새로 등록된 것으로 취급).
        await conn.ExecuteAsync("DELETE FROM TBAEXCRATE WHERE base_date = @today", new { today });

        foreach (var r in validRates)
        {
            var (curCd, unitAmt) = ParseCurUnit(r.CurUnit);
            if (curCd == null) continue; // cur_unit이 아예 안 오는 비정상 행은 건너뛴다

            await conn.ExecuteAsync(
                @"INSERT INTO TBAEXCRATE (base_date, cur_cd, cur_nm, ttb, tts, unit_amt, exc_rate, reg_user_id, reg_dt, reg_pc)
                  VALUES (@base_date, @cur_cd, @cur_nm, @ttb, @tts, @unit_amt, @exc_rate, @sysUser, GETDATE(), @sysPc);",
                new
                {
                    base_date = today,
                    cur_cd = curCd,
                    cur_nm = r.CurNm,
                    ttb = ParseDecimal(r.Ttb),
                    tts = ParseDecimal(r.Tts),
                    unit_amt = unitAmt,
                    exc_rate = ParseDecimal(r.DealBasR),
                    sysUser = "SYSTEM",
                    sysPc = Environment.MachineName,
                });
        }

        return new IntegrationRunResult { Success = true, Message = $"{validRates.Count}건 갱신", RowCount = validRates.Count };
    }

    // 수출입은행 API는 숫자를 "1,320.50"처럼 콤마 포함 문자열로 준다 - decimal.Parse 전에 콤마 제거 필요.
    private static decimal? ParseDecimal(string? s) =>
        string.IsNullOrWhiteSpace(s) ? null : decimal.TryParse(s.Replace(",", ""), out var v) ? v : null;

    // cur_unit은 "USD"처럼 통화코드만 오거나, 100단위 고시 통화는 "JPY(100)"처럼 단위가 괄호로
    // 붙어 온다 - TBAEXCRATE는 cur_cd/unit_amt를 별도 컬럼으로 나눠 갖고 있어서 여기서 분리한다.
    private static readonly Regex CurUnitPattern = new(@"^([A-Za-z]+)(?:\((\d+)\))?$", RegexOptions.Compiled);

    private static (string? CurCd, decimal UnitAmt) ParseCurUnit(string? curUnit)
    {
        if (string.IsNullOrWhiteSpace(curUnit)) return (null, 1);
        var m = CurUnitPattern.Match(curUnit.Trim());
        if (!m.Success) return (curUnit.Trim(), 1);
        var unitAmt = m.Groups[2].Success ? decimal.Parse(m.Groups[2].Value) : 1;
        return (m.Groups[1].Value, unitAmt);
    }

    private class EximRateItem
    {
        [JsonPropertyName("result")] public int Result { get; set; }
        [JsonPropertyName("cur_unit")] public string? CurUnit { get; set; }
        [JsonPropertyName("cur_nm")] public string? CurNm { get; set; }
        [JsonPropertyName("ttb")] public string? Ttb { get; set; }
        [JsonPropertyName("tts")] public string? Tts { get; set; }
        [JsonPropertyName("deal_bas_r")] public string? DealBasR { get; set; }
    }
}
