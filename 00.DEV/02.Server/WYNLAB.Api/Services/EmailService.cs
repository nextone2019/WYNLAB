using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using WYNLAB.Api.Repositories.SM;

namespace WYNLAB.Api.Services;

public interface IEmailService
{
    Task SendAsync(string toAddress, string subject, string body);
}

/// <summary>
/// MailKit으로 SMTP 발송한다. 비밀번호는 appsettings/user-secrets/web.config에서만 오고
/// 코드에는 값이 없다(SmtpSettings.cs 설명 참고) - 그 외 Host/Port/UserName/발신주소/발신표시명은
/// TSMSITECONFIG(frmSiteConfig 메일발신 탭)에 값이 있으면 그걸 우선하고, 없으면(NULL)
/// appsettings의 SmtpSettings로 폴백한다(2026-09-06 연동). 비밀번호까지 DB로 옮기지 않는 건
/// frmSiteConfig 설계 당시 이미 결정된 사항 - DB연결문자열/JWT시크릿과 같은 급의 값이라 서버
/// 환경변수로만 관리한다.
/// </summary>
public class EmailService : IEmailService
{
    /// <summary>MailKit SmtpClient의 기본 타임아웃(약 100초)을 그대로 두면, 네트워크가 막혀서
    /// 연결 자체가 안 될 때 클라이언트(비밀번호 찾기 화면)가 최대 100초 동안 "멈춘 것처럼"
    /// 보인다(2026-09-10 실제 겪음 - "인증코드받기 누르고 죽어버렸어"). 서버가 이미 3중 재시도
    /// 없이 한 번만 시도하므로, 짧게 잡아서 빨리 실패하고 명확한 오류로 돌려주는 쪽이 낫다.</summary>
    private const int ConnectTimeoutMs = 15000;

    private readonly SmtpSettings _settings;
    private readonly ISiteConfigRepository _siteConfig;

    public EmailService(IOptions<SmtpSettings> settings, ISiteConfigRepository siteConfig)
    {
        _settings = settings.Value;
        _siteConfig = siteConfig;
    }

    public async Task SendAsync(string toAddress, string subject, string body)
    {
        var config = await _siteConfig.GetAsync();

        var host = string.IsNullOrWhiteSpace(config?.SmtpHost) ? _settings.Host : config.SmtpHost;
        var port = config?.SmtpPort ?? _settings.Port;
        var userName = string.IsNullOrWhiteSpace(config?.SmtpUsername) ? _settings.UserName : config.SmtpUsername;
        var fromAddress = string.IsNullOrWhiteSpace(config?.SmtpFromAddress) ? _settings.FromAddress : config.SmtpFromAddress;
        var fromDisplayName = string.IsNullOrWhiteSpace(config?.SmtpFromDisplayNm) ? _settings.FromDisplayName : config.SmtpFromDisplayNm;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromDisplayName, fromAddress));
        message.To.Add(MailboxAddress.Parse(toAddress));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient { Timeout = ConnectTimeoutMs };
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(userName, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
