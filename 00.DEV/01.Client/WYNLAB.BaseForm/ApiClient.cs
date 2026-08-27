using System.Net.Http;
using System.Net.Http.Json;

namespace WYNLAB.Base;

/// <summary>
/// WYNLAB.Api 호출 전담 클라이언트.
/// UI.Common에 위치 - Shell뿐 아니라 모든 업무모듈(Modules.Sales, Modules.System 등)에서
/// 화면 개발시 이 클래스 하나로 조회/등록/수정/삭제 API를 호출한다.
/// </summary>
public static class ApiClient
{
    private static HttpClient _http = CreateClient();

    private static HttpClient CreateClient() => new() { BaseAddress = new Uri(AppConfig.ApiBaseUrl) };

    /// <summary>
    /// 서버 전환(AppConfig.SwitchEnvironment)시 호출됨 - 접속주소를 새로 설정하고
    /// 인증토큰은 초기화한다 (예전 서버 토큰은 새 서버에서 무효하므로 재로그인 필요).
    /// </summary>
    public static void Reconfigure(string newBaseUrl)
    {
        _http = new HttpClient { BaseAddress = new Uri(newBaseUrl) };
    }

    /// <summary>목록/단건 조회 - 화면의 "조회" 버튼에서 사용</summary>
    public static async Task<TResponse?> GetAsync<TResponse>(string url)
    {
        var response = await _http.GetAsync(url);
        var text = await ReadBodyOrThrowAsync(response, url);
        return Deserialize<TResponse>(text);
    }

    /// <summary>등록/로그인 등 - 화면의 "저장(신규)" 버튼에서 사용</summary>
    public static async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PostAsJsonAsync(url, body);
        // 401(로그인 실패)/403(권한 없음)도 body는 파싱해서 그대로 반환한다(Success=false,
        // Message 포함 패턴 지원) - 그래서 여기서는 ReadBodyOrThrowAsync(성공 아니면 무조건
        // throw)를 안 쓰고, 본문을 직접 읽어 상태 코드와 무관하게 역직렬화를 시도한다.
        var text = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(text))
            throw new HttpRequestException($"서버 오류(HTTP {(int)response.StatusCode} {response.StatusCode}) - 응답 본문이 비어있습니다. {url}");

        return Deserialize<TResponse>(text);
    }

    /// <summary>수정 - 화면의 "저장(수정)" 버튼에서 사용</summary>
    public static async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PutAsJsonAsync(url, body);
        var text = await ReadBodyOrThrowAsync(response, url);
        return Deserialize<TResponse>(text);
    }

    /// <summary>삭제(대부분 소프트삭제 - USE_YN='N' 처리) - 화면의 "삭제" 버튼에서 사용</summary>
    public static async Task DeleteAsync(string url)
    {
        var response = await _http.DeleteAsync(url);
        await ReadBodyOrThrowAsync(response, url);
    }

    /// <summary>응답 본문(보통 ApiResult)까지 받아오는 삭제 - 저장프로시저가 "참조 중이라 삭제
    /// 불가" 같은 사유를 ReturnMsg로 돌려주는 화면에서 쓴다. 위의 반환값 없는 버전으로는 그
    /// 사유를 화면에 보여줄 방법이 없다.</summary>
    public static async Task<TResponse?> DeleteAsync<TResponse>(string url)
    {
        var response = await _http.DeleteAsync(url);
        var text = await ReadBodyOrThrowAsync(response, url);
        return Deserialize<TResponse>(text);
    }

    public static void SetAuthToken(string accessToken) =>
        _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

    /// <summary>
    /// 응답 본문을 문자열로 한 번만 읽어서(스트림은 한 번만 읽을 수 있으므로), 실패 상태 코드면
    /// 본문 내용까지 담은 예외를 던진다. 실패했는데 EnsureSuccessStatusCode()만 쓰면 "응답 상태
    /// 코드가 성공을 나타내지 않습니다. 500(Internal Server Error)."처럼 상태 코드만 보이고
    /// 진짜 원인(서버 예외 메시지)은 사라진다(실제로 겪음 - frmUserAuth 개발 중 원인 불명의
    /// 500 여러 번). Program.cs가 개발 환경에서만 UseExceptionHandler로 예외 타입/메시지를
    /// JSON 본문에 내려주므로, 여기서 그 본문을 그대로 메시지에 포함시켜 화면(ShellForm의
    /// [액션명] 처리 중 오류가 발생했습니다 다이얼로그)까지 전달한다.
    /// </summary>
    private static async Task<string> ReadBodyOrThrowAsync(HttpResponseMessage response, string url)
    {
        var text = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var detail = string.IsNullOrWhiteSpace(text) ? "(응답 본문 없음)" : text;
            throw new HttpRequestException($"서버 오류(HTTP {(int)response.StatusCode} {response.StatusCode}) - {url}\n{detail}");
        }
        return text;
    }

    private static TResponse? Deserialize<TResponse>(string text) =>
        System.Text.Json.JsonSerializer.Deserialize<TResponse>(text,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
}
