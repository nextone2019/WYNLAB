using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NEXTFramework.Api.Services;

public interface IJwtTokenService
{
    string CreateAccessToken(string userId, bool isAdmin);
    string CreateRefreshToken();
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config) => _config = config;

    public string CreateAccessToken(string userId, bool isAdmin)
    {
        var jwtSection = _config.GetSection("Jwt");
        var secretKey = jwtSection["SecretKey"]!;
        var expireMinutes = int.Parse(jwtSection["AccessTokenExpireMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new("isAdmin", isAdmin ? "Y" : "N")
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
