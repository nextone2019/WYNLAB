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
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    /// <summary>등록/로그인 등 - 화면의 "저장(신규)" 버튼에서 사용</summary>
    public static async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PostAsJsonAsync(url, body);
        // 401(로그인 실패) 등도 body는 파싱해서 그대로 반환 (Success=false, Message 포함 패턴 지원)
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    /// <summary>수정 - 화면의 "저장(수정)" 버튼에서 사용</summary>
    public static async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PutAsJsonAsync(url, body);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    /// <summary>삭제(대부분 소프트삭제 - USE_YN='N' 처리) - 화면의 "삭제" 버튼에서 사용</summary>
    public static async Task DeleteAsync(string url)
    {
        var response = await _http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>응답 본문(보통 ApiResult)까지 받아오는 삭제 - 저장프로시저가 "참조 중이라 삭제
    /// 불가" 같은 사유를 ReturnMsg로 돌려주는 화면에서 쓴다. 위의 반환값 없는 버전으로는 그
    /// 사유를 화면에 보여줄 방법이 없다.</summary>
    public static async Task<TResponse?> DeleteAsync<TResponse>(string url)
    {
        var response = await _http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public static void SetAuthToken(string accessToken) =>
        _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
}
