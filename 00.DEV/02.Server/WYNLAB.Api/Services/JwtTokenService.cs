using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WYNLAB.Api.Services;

public interface IJwtTokenService
{
    /// <summary>mustChangePwd=true면 MustChangePasswordFilter가 change-password 호출을
    /// 제외한 모든 API를 이 토큰으로 막는다 - 비밀번호 강제변경 전엔 앱을 못 쓰게 하기 위함
    /// (만료로 인한 강제변경/이메일 초기화 직후 등, AuthService.LoginAsync 참고).</summary>
    string CreateAccessToken(string userId, bool isAdmin, bool mustChangePwd = false);
    string CreateRefreshToken();
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config) => _config = config;

    public string CreateAccessToken(string userId, bool isAdmin, bool mustChangePwd = false)
    {
        var jwtSection = _config.GetSection("Jwt");
        var secretKey = jwtSection["SecretKey"]!;
        var expireMinutes = int.Parse(jwtSection["AccessTokenExpireMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new("isAdmin", isAdmin ? "Y" : "N"),
            new("mustChangePwd", mustChangePwd ? "Y" : "N")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken() => Guid.NewGuid().ToString("N");
}
