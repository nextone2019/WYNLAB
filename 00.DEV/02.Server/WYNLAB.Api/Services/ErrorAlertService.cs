using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace WYNLAB.Api.Services;

/// <summary>
/// 처리 안 된 예외(500)가 나면 관리자에게 이메일로 알린다 - appsettings의 Alerts:AdminEmail이
/// 비어있으면 조용히 아무것도 안 한다(로그 파일만으로 충분한 경우, 또는 아직 이메일 설정을
/// 안 해둔 개발 환경에서 매번 예외를 던져도 메일이 안 나가야 하므로).
///
/// 같은 예외 타입이 짧은 시간에 반복되면(예: DB 연결 끊김으로 수십 건이 동시에 터짐) 그만큼
/// 메일이 쏟아지면 알림이 아니라 스팸이 된다 - 예외 타입별로 15분에 한 번만 보내도록 막는다
/// (2026-09-15, "로깅/모니터링을 만들어달라"는 요청으로 추가. ELK/Grafana 같은 별도 인프라 없이
/// 이미 있는 SMTP(비밀번호 재설정 메일과 같은 경로)를 그대로 재사용한다).
/// </summary>
public interface IErrorAlertService
{
    Task NotifyAsync(Exception ex, string requestPath);
}

public class ErrorAlertService : IErrorAlertService
{
    private static readonly ConcurrentDictionary<string, DateTime> _lastSentAt = new();
    private static readonly TimeSpan ThrottleWindow = TimeSpan.FromMinutes(15);

    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;
    private readonly ILogger<ErrorAlertService> _logger;

    public ErrorAlertService(IEmailService emailService, IConfiguration config, ILogger<ErrorAlertService> logger)
    {
        _emailService = emailService;
        _config = config;
        _logger = logger;
    }

    public async Task NotifyAsync(Exception ex, string requestPath)
    {
        var adminEmail = _config["Alerts:AdminEmail"];
        if (string.IsNullOrWhiteSpace(adminEmail)) return;

        var throttleKey = ex.GetType().FullName ?? "Unknown";
        var now = DateTime.UtcNow;
        if (_lastSentAt.TryGetValue(throttleKey, out var last) && now - last < ThrottleWindow)
            return; // 같은 종류의 예외가 최근에도 이미 알려졌다 - 조용히 넘어간다(로그 파일에는 계속 남음).

        _lastSentAt[throttleKey] = now;

        try
        {
            var body = $"""
                운영 서버에서 처리되지 않은 예외가 발생했습니다.

                시간: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                요청 경로: {requestPath}
                예외 종류: {ex.GetType().FullName}
                메시지: {ex.Message}

                (같은 종류의 예외는 15분에 한 번만 메일로 알립니다 - 전체 스택트레이스는 서버의
                로그 파일(logs\wynlab-*.log)에서 확인해주세요.)
                """;
            await _emailService.SendAsync(adminEmail, "[WYNLAB] 서버 오류 알림", body);
        }
        catch (Exception mailEx)
        {
            // 알림 메일 발송 실패가 원래 예외 처리 흐름을 막으면 안 된다 - 로그에만 남기고 삼킨다.
            _logger.LogWarning(mailEx, "오류 알림 메일 발송에 실패했습니다.");
        }
    }
}
