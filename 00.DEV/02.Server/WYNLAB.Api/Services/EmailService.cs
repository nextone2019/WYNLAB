using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WYNLAB.Api.Services;

public interface IEmailService
{
    Task SendAsync(string toAddress, string subject, string body);
}

/// <summary>
/// MailKit으로 SMTP 발송한다. 설정(SmtpSettings)은 appsettings/user-secrets/web.config에서만
/// 오고 코드에는 값이 없다 - SmtpSettings.cs 설명 참고.
/// </summary>
public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public EmailService(IOptions<SmtpSettings> settings) => _settings = settings.Value;

    public async Task SendAsync(string toAddress, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromDisplayName, _settings.FromAddress));
        message.To.Add(MailboxAddress.Parse(toAddress));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.UserName, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
