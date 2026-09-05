using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Services;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

// 클래스 레벨에 [AllowAnonymous]를 두지 않는다 - ASP.NET Core는 AllowAnonymous가 어디에라도
// 있으면 같은 액션의 [Authorize]를 통째로 무시한다(클래스든 액션이든 상관없이 우선한다). 그래서
// 로그인/비밀번호찾기처럼 실제로 익명이어야 하는 액션에만 개별로 [AllowAnonymous]를 붙이고,
// ChangePassword는 [Authorize]가 실제로 걸리도록 클래스에는 아무것도 안 붙인다 - 여기서
// 한 번 잘못 만들 뻔했다(클래스에 AllowAnonymous를 두면 ChangePassword가 인증 없이 열림).
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>
    /// WinForms Shell의 LoginForm이 호출하는 로그인 엔드포인트.
    /// 성공 시 JWT + 병합된 메뉴권한(MenuDto 목록)을 함께 내려준다. 자격증명은 맞았지만
    /// 비밀번호를 먼저 바꿔야 하는 계정(RequirePasswordChange=true)이면 메뉴/바로가기 없이
    /// 제한된 토큰만 내려간다 - LoginForm은 이 경우 강제 변경 다이얼로그부터 띄워야 한다.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _authService.LoginAsync(request, clientIp);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }

    /// <summary>비밀번호 찾기 1단계 - 아이디만 받는다. 계정이 있든 없든, 이메일이 등록돼
    /// 있든 아니든 항상 같은 응답이다(AuthService 설명 참고 - 계정 존재 여부 노출 방지).</summary>
    [HttpPost("reset-request")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResult>> ResetRequest([FromBody] PasswordResetRequestDto request)
    {
        var result = await _authService.RequestPasswordResetAsync(request.UserId);
        return Ok(result);
    }

    /// <summary>비밀번호 찾기 2단계 - 인증코드 확인 + 새 비밀번호 설정. 성공하면 그대로
    /// 로그인까지 되어 LoginResponse가 돌아온다(다시 로그인 화면에서 입력할 필요 없음).</summary>
    [HttpPost("reset-confirm")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> ResetConfirm([FromBody] PasswordResetConfirmDto request)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _authService.ConfirmPasswordResetAsync(request.UserId, request.Code, request.NewPassword, clientIp);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }

    /// <summary>
    /// 로그인된 상태에서 비밀번호 변경 - 만료/초기화로 인한 강제변경 다이얼로그가 쓴다.
    /// [Authorize] 필요(로그인은 돼 있어야 함)하지만, mustChangePwd=Y인 제한된 토큰으로도
    /// 호출 가능해야 하므로 [AllowWhilePasswordChangeRequired]로 MustChangePasswordFilter의
    /// 전역 차단에서 이 액션만 예외로 둔다.
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    [AllowWhilePasswordChangeRequired]
    public async Task<ActionResult<ApiResult>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
        return Ok(result);
    }
}
