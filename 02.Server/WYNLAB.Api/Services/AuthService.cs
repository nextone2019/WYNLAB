using WYNLAB.Api.Repositories;
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
    private readonly IMenuRepository _menuRepo;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUserRepository userRepo, IMenuRepository menuRepo, IJwtTokenService jwtTokenService)
    {
        _userRepo = userRepo;
        _menuRepo = menuRepo;
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

        // 인증 성공 - 메뉴권한 병합 (세션조회 프로시저에서 이미 받아온 그룹목록 재사용)
        var isAdmin = user.IsAdminYn == "Y";
        var menus = await _menuRepo.GetAllActiveMenusAsync();
        var menuDtos = await BuildMenuDtosAsync(user.UserId, isAdmin, session.GroupCodes, menus);

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
                PositionNm = user.PositionNm,
                IsAdminYn = isAdmin
            },
            Menus = menuDtos
        };
    }

    private async Task<List<MenuDto>> BuildMenuDtosAsync(string userId, bool isAdmin, List<string> groupCodes, List<MenuRow> menus)
    {
        // 시스템관리자는 권한테이블 조회 없이 전체 메뉴 풀권한 부여
        if (isAdmin)
        {
            return menus.Select(m => new MenuDto
            {
                MenuCd = m.MenuCd,
                MenuNm = m.MenuNm,
                UpperMenuCd = m.UpperMenuCd,
                MenuLevel = m.MenuLevel,
                MenuType = m.MenuType,
                FormClassNm = m.FormClassNm,
                IconNm = m.IconNm,
                SortOrder = m.SortOrder,
                ViewYn = true,
                InsertYn = true,
                UpdateYn = true,
                DeleteYn = true,
                ExcelYn = true
            }).ToList();
        }

        var authRows = await _menuRepo.GetMenuAuthRowsAsync(userId, groupCodes);
        return MenuPermissionMerger.Merge(menus, authRows);
    }

    private static LoginResponse Fail(string message) => new() { Success = false, Message = message };
}
