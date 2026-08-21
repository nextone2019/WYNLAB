using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEXTFramework.Api.Services;
using NEXTFramework.Shared.Dtos;

namespace NEXTFramework.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>
    /// WinForms Shell의 LoginForm이 호출하는 로그인 엔드포인트.
    /// 성공 시 JWT + 병합된 메뉴권한(MenuDto 목록)을 함께 내려준다.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _authService.LoginAsync(request, clientIp);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }
}
