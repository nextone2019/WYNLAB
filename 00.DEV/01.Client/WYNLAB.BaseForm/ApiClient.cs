using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WYNLAB.Base;

/// <summary>
/// WYNLAB.Api 호출 전담 클라이언트.
/// UI.Common에 위치 - Shell뿐 아니라 모든 업무모듈(Modules.Sales, Modules.System 등)에서
/// 화면 개발시 이 클래스 하나로 조회/등록/수정/삭제 API를 호출한다.
/// </summary>
public static class ApiClient
{
    private static HttpClient _http = CreateClient();

    private static HttpClient CreateClient()
    {
        // SessionRefreshHandler가 401을 가로채 캐시된 자격증명으로 조용히 재로그인+재시도한다
        // (2026-09-09 요청 - "사용중에는 원시 에러메시지가 안 보이게") - Get/Post/Put/Delete 등
        // 모든 verb가 이 하나의 HttpClient를 공유하므로 여기 한 곳에서만 꽂으면 전체에 적용된다.
        var client = new HttpClient(new SessionRefreshHandler { InnerHandler = new HttpClientHandler() })
            { BaseAddress = new Uri(AppConfig.ApiBaseUrl) };
        // 서버가 reg_pc/upt_pc 등에 접속 IP 대신 쓸 수 있게 이 PC의 컴퓨터 이름을 매 요청에
        // 실어 보낸다(2026-09-06 - 클라이언트/서버가 같은 PC일 때 IP만으로는 전부 "::1"로
        // 찍혀 구분이 안 됐음). ClientPcInfo가 모든 컨트롤러 공통으로 읽는다(2026-09-11).
        client.DefaultRequestHeaders.Add("X-Client-Pc", Environment.MachineName);
        // 클라이언트/서버가 같은 PC(로컬 개발)일 땐 서버가 보는 접속 IP가 루프백(::1)이라
        // reg_pc/upt_pc에 실제 사설망 IP(예: 192.168.x.x)가 안 남는다(2026-09-11 실사용 발견 -
        // "IP정보가 반영 안된것 같아", NEXTONE | 192.168.120.1처럼 남길 원함) - 그래서 이 PC의
        // 실제 사설망 IP를 클라이언트가 직접 찾아 같이 실어 보내고, 서버는 이 헤더가 있으면
        // RemoteIpAddress보다 이 값을 우선한다(ClientPcInfo.Build 참고). PC명과 마찬가지로
        // 클라이언트가 보고하는 값이라 신뢰하지 않는 보안 판단(로그인 등)에는 쓰지 않는다 - 어디까지나
        // 감사이력(reg_pc/upt_pc) 참고용.
        var localIp = GetLocalIPv4();
        if (!string.IsNullOrEmpty(localIp))
            client.DefaultRequestHeaders.Add("X-Client-Ip", localIp);
        return client;
    }

    /// <summary>이 PC가 LAN에서 실제로 쓰는 사설망 IPv4 주소를 찾는다 - NetworkInterface를 돌며
    /// 사용 중(Up)이고 루프백/터널이 아닌 어댑터의 첫 IPv4 유니캐스트 주소를 쓴다. 여러 어댑터가
    /// 있어도(VPN, 가상 NIC 등) 실제 통신에 쓰는 어댑터를 우선하도록 OperationalStatus로 거른다.
    /// 못 찾으면 null - 이때 서버는 RemoteIpAddress(접속 IP)로 되돌아간다.</summary>
    private static string? GetLocalIPv4()
    {
        try
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus != OperationalStatus.Up) continue;
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel) continue;

                foreach (var addr in nic.GetIPProperties().UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        return addr.Address.ToString();
                }
            }
        }
        catch
        {
            // 감사이력 보조 정보일 뿐이므로 실패해도 저장 자체를 막지 않는다 - null 반환시
            // 서버가 RemoteIpAddress로 되돌아간다.
        }
        return null;
    }

    /// <summary>
    /// 서버 전환(AppConfig.SwitchEnvironment)시 호출됨 - 접속주소를 새로 설정하고
    /// 인증토큰은 초기화한다 (예전 서버 토큰은 새 서버에서 무효하므로 재로그인 필요).
    /// </summary>
    public static void Reconfigure(string newBaseUrl)
    {
        _http = new HttpClient(new SessionRefreshHandler { InnerHandler = new HttpClientHandler() })
            { BaseAddress = new Uri(newBaseUrl) };
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

    /// <summary>파일 청크 하나를 본문 그대로(멀티파트 아님) 올린다 - 첨부파일 공통팝업
    /// (popFileUpload)의 청크 업로드 전용. 응답 본문은 안 쓰므로(성공/실패만 중요) 실패 시
    /// 예외만 던진다 - 호출측이 청크별로 재시도 루프를 도는 데 이 예외를 쓴다.</summary>
    public static async Task UploadChunkAsync(string url, byte[] buffer, int count)
    {
        using var content = new ByteArrayContent(buffer, 0, count);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        var response = await _http.PostAsync(url, content);
        await ReadBodyOrThrowAsync(response, url);
    }

    /// <summary>파일 다운로드(바이너리) - 첨부파일 공통팝업 전용. 서버가 Content-Disposition에
    /// 담아 보낸 파일명을 함께 돌려준다(FilesController.Download의 File(bytes, contentType,
    /// fileNm) 호출이 자동으로 채워줌).</summary>
    public static async Task<(byte[] Bytes, string? FileName)> DownloadAsync(string url)
    {
        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync();
            var detail = string.IsNullOrWhiteSpace(text) ? "(응답 본문 없음)" : text;
            throw new HttpRequestException($"서버 오류(HTTP {(int)response.StatusCode} {response.StatusCode}) - {url}\n{detail}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
            ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"');
        return (bytes, fileName);
    }

    /// <summary>파일 하나를 멀티파트로 PUT - 서버 액션이 IFormFile로 받는 업로드 전용
    /// (frmSiteConfig의 로그인배경/로고/파비콘 등, 청크 없이 한 번에 보내는 작은 파일용).
    /// UploadChunkAsync(본문 그대로/멀티파트 아님, 청크 업로드 전용)와는 용도가 다르다 -
    /// 여기는 서버가 [FromForm] 없이 그냥 IFormFile file 파라미터로 받는 액션에 맞춘다.</summary>
    public static async Task<TResponse?> PutFileAsync<TResponse>(string url, byte[] bytes, string fileName)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(bytes);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await _http.PutAsync(url, content);
        var text = await ReadBodyOrThrowAsync(response, url);
        return Deserialize<TResponse>(text);
    }

    public static void SetAuthToken(string accessToken) =>
        _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

    /// <summary>강제 비밀번호 변경 흐름에서, 제한된(mustChangePwd=Y) 토큰을 붙인 채로 재로그인을
    /// 시도하면 로그인 자체가 [AllowAnonymous]라도 이 토큰이 그대로 딸려가서
    /// MustChangePasswordFilter에 막힌다(실제로 겪음 - "비밀번호를 먼저 변경해야 합니다"가
    /// 재로그인 시도에서도 뜸). 재로그인 직전에 이걸 호출해서 완전히 익명 상태로 되돌린다.</summary>
    public static void ClearAuthToken() =>
        _http.DefaultRequestHeaders.Authorization = null;

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
