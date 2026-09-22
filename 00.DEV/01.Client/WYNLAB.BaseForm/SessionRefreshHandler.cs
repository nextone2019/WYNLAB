using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 액세스 토큰이 만료돼서 401이 오면, 화면에 원시 에러("서버 오류(HTTP 401 Unauthorized)...")를
/// 보여주는 대신 캐시된 자격증명(SessionManager.CachedUserId/CachedPassword - 원래 "서버 전환 시
/// 조용히 재로그인"용으로 만들어진 것을 그대로 재사용, SessionManager.cs 참고)으로 백그라운드에서
/// 다시 로그인해서 새 토큰을 받고, 실패했던 원래 요청을 한 번 더 시도한다 - 성공하면 사용자는
/// 아무것도 못 느낀다(2026-09-09 요청 - "시스템 사용중에는 이런 메시지를 보지 않도록 내부적으로
/// 처리해줘"). ApiClient의 모든 verb(Get/Post/Put/Delete 등)가 공유하는 HttpClient 파이프라인에
/// 꽂혀서 한 곳에서만 처리한다 - 개별 verb 메서드는 손댈 필요 없음.
///
/// 캐시된 자격증명이 없거나(로그인 안 된 상태) 재로그인 자체도 실패하면(비번이 그새 바뀌었거나
/// 계정이 잠긴 등 드문 경우) 원래 401 응답을 그대로 돌려준다 - 기존 에러 처리 그대로 유지된다.
/// 이 드문 실패 케이스까지 잠금화면을 강제로 띄우는 건 이번 스코프 밖(백그라운드 스레드에서 모달
/// UI를 안전하게 띄우고 기다려야 해서 복잡도가 크게 늘어남) - ShellForm의 유휴감지 잠금화면은
/// 완전히 별개 경로다.
/// </summary>
public class SessionRefreshHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 요청 본문을 재시도 시에도 다시 읽을 수 있게 먼저 메모리에 올려둔다(스트림은 한 번만
        // 읽을 수 있어서, 첫 시도에서 이미 다 읽혀버리면 재시도 때 빈 본문이 나간다).
        if (request.Content != null)
            await request.Content.LoadIntoBufferAsync();

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        // 로그인 요청 자체가 401이면(비번 오타 등, PostAsync 주석 참고 - 로그인 실패도 401을 씀)
        // 재시도하지 않는다 - 무한 재귀/불필요한 왕복을 막는다.
        if (request.RequestUri?.AbsolutePath.TrimEnd('/').EndsWith("api/auth/login", StringComparison.OrdinalIgnoreCase) == true)
            return response;

        var userId = SessionManager.Current.CachedUserId;
        var password = SessionManager.Current.CachedPassword;
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password)) return response;

        LoginResponse? loginResult;
        try
        {
            using var loginRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(request.RequestUri!, "/api/auth/login"))
            {
                // net48의 string.IsNullOrEmpty엔 [NotNullWhen(false)]가 없어 위 가드로 이미
                // 확인했는데도 컴파일러가 못 알아채고 경고한다 - 안전하다.
                Content = JsonContent.Create(new LoginRequest
                {
                    UserId = userId!,
                    Password = password!,
                    ClientVersion = System.Windows.Forms.Application.ProductVersion
                })
            };
            using var loginResponse = await base.SendAsync(loginRequest, cancellationToken);
            if (!loginResponse.IsSuccessStatusCode) return response;

            loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
        }
        catch
        {
            return response; // 재로그인 자체가 실패(서버 무응답 등)하면 원래 401을 그대로 돌려준다.
        }

        if (loginResult is not { Success: true, AccessToken.Length: > 0 }) return response;

        SessionManager.Current.SignIn(loginResult);
        ApiClient.SetAuthToken(loginResult.AccessToken!);

        using var retryRequest = await CloneRequestAsync(request);
        retryRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.AccessToken);
        return await base.SendAsync(retryRequest, cancellationToken);
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri) { Version = original.Version };
        if (original.Content != null)
        {
            var bytes = await original.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(bytes);
            foreach (var header in original.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        foreach (var header in original.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        return clone;
    }
}
