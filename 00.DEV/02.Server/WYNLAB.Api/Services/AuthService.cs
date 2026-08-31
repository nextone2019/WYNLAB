using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp);
}

public class AuthService : IAuthService
{
    private const int MaxPwdFailCount = 5; // 5회 실패시 잠금 처리 (안내 메시지만, 실제 잠금해제는 관리자 화면에서)

    private readonly IUserRepository _userRepo;
    private readonly IMenuPermissionService _menuPermissionService;
    private readonly IShortcutRepository _shortcutRepo;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUserRepository userRepo, IMenuPermissionService menuPermissionService, IShortcutRepository shortcutRepo, IJwtTokenService jwtTokenService)
    {
        _userRepo = userRepo;
        _menuPermissionService = menuPermissionService;
        _shortcutRepo = shortcutRepo;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp)
    {
        var session = await _userRepo.GetSessionAsync(request.UserId);
        var user = session.User;

        if (user == null)
        {
            await _userRepo.InsertLoginHistAsync(request.UserId, clientIp, request.ClientVersion, "FAIL_NOID");
            return Fail("아이디 또는 비밀번호가 올바르지 않습니다.");
        }

        if (user.UseYn != "Y")
        {
            await _userRepo.InsertLoginHistAsync(user.UserId, clientIp, request.ClientVersion, "FAIL_DISABLED");
            return Fail("사용이 정지된 계정입니다. 관리자에게 문의해주세요.");
        }

        if (user.PwdFailCnt >= MaxPwdFailCount)
        {
            await _userRepo.InsertLoginHistAsync(user.UserId, clientIp, request.ClientVersion, "FAIL_LOCK");
            return Fail("비밀번호 실패 횟수 초과로 잠긴 계정입니다. 관리자에게 문의해주세요.");
        }

        var passwordOk = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordOk)
        {
            await _userRepo.IncreasePwdFailCountAsync(user.UserId);
            await _userRepo.InsertLoginHistAsync(user.UserId, clientIp, request.ClientVersion, "FAIL_PWD");
            return Fail("아이디 또는 비밀번호가 올바르지 않습니다.");
        }

        // 인증 성공 - 메뉴권한 병합 (MenuPermissionService가 API 액션 권한체크와 동일한 로직을 씀)
        var isAdmin = user.UserType == "A";
        var menuDtos = await _menuPermissionService.GetEffectivePermissionsAsync(user.UserId);
        var shortcuts = await _shortcutRepo.GetEffectiveShortcutsAsync(user.UserId);

        await _userRepo.UpdateLoginSuccessAsync(user.UserId);
        await _userRepo.InsertLoginHistAsync(user.UserId, clientIp, request.ClientVersion, "SUCCESS");

        return new LoginResponse
        {
            Success = true,
            AccessToken = _jwtTokenService.CreateAccessToken(user.UserId, isAdmin),
            RefreshToken = _jwtTokenService.CreateRefreshToken(),
            UserInfo = new UserInfoDto
            {
                UserId = user.UserId,
                UserNm = user.UserNm,
                EmpNo = user.EmpNo,
                DeptCd = user.DeptCd,
                DeptNm = user.DeptNm,
                DeveloperYn = user.DeveloperYn == "Y",
                UserType = user.UserType
            },
            Menus = menuDtos,
            Shortcuts = shortcuts
        };
    }

    private static LoginResponse Fail(string message) => new() { Success = false, Message = message };
}
