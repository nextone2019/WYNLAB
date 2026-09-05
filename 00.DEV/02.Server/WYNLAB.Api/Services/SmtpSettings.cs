namespace WYNLAB.Api.Services;

/// <summary>
/// appsettings.json의 "Smtp" 섹션에 바인딩된다. 회사마다 메일서버(회사 메일서버/Gmail/기타
/// 웹메일)가 다르므로 코드에 값을 박아넣지 않는다 - 개발PC는 dotnet user-secrets, 실제 서버는
/// web.config의 environmentVariables(Deploy-Local.ps1이 DB연결문자열/JWT키를 넣는 것과 같은
/// 자리)에 그 회사 값을 넣으면 코드 수정 없이 그대로 동작한다.
/// </summary>
public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromDisplayName { get; set; } = "WYNLAB";
}
