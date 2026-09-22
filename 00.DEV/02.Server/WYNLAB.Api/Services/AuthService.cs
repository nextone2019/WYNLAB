using System.Security.Cryptography;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp);

    /// <summary>비밀번호 찾기 1단계 - 계정이 없거나 이메일이 등록 안 되어 있으면 실패로 알려준다
    /// (2026-09-02 변경: 원래는 계정 존재 여부를 캐는 수단이 되지 않도록 항상 같은 메시지만
    /// 돌려줬는데, 아이디 오타를 실제로 낸 사용자가 오지도 않을 메일을 기다리게 되는 문제 때문에
    /// 명확한 성공/실패 구분으로 바꿨다 - 자세한 사유는 구현부 XML 주석 참고).</summary>
    Task<ApiResult> RequestPasswordResetAsync(string userId);

    /// <summary>비밀번호 찾기 2단계 - 성공하면 그대로 로그인까지 처리해서 LoginResponse를 돌려준다.</summary>
    Task<LoginResponse> ConfirmPasswordResetAsync(string userId, string code, string newPassword, string clientIp);

    /// <summary>로그인된 상태에서 비밀번호 변경(강제변경 다이얼로그 등) - 현재 비밀번호를 다시
    /// 검증한다(이미 로그인했다는 사실만으로 바꾸게 두지 않음).</summary>
    Task<ApiResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
}

public class AuthService : IAuthService
{
    private const int DefaultMaxPwdFailCount = 5; // TSMSITECONFIG.pwd_lock_threshold가 NULL일 때 폴백
    private const int DefaultResetCodeExpireMinutes = 15; // TSMSITECONFIG.pwd_reset_code_valid_min이 NULL일 때 폴백

    private readonly IUserRepository _userRepo;
    private readonly IUserManageRepository _userManageRepo;
    private readonly IPwdResetRepository _pwdResetRepo;
    private readonly IEmailService _emailService;
    private readonly IMenuPermissionService _menuPermissionService;
    private readonly IShortcutRepository _shortcutRepo;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ISiteConfigRepository _siteConfig;

    public AuthService(
        IUserRepository userRepo,
        IUserManageRepository userManageRepo,
        IPwdResetRepository pwdResetRepo,
        IEmailService emailService,
        IMenuPermissionService menuPermissionService,
        IShortcutRepository shortcutRepo,
        IJwtTokenService jwtTokenService,
        ISiteConfigRepository siteConfig)
    {
        _userRepo = userRepo;
        _userManageRepo = userManageRepo;
        _pwdResetRepo = pwdResetRepo;
        _emailService = emailService;
        _menuPermissionService = menuPermissionService;
        _shortcutRepo = shortcutRepo;
        _jwtTokenService = jwtTokenService;
        _siteConfig = siteConfig;
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

        var config = await _siteConfig.GetAsync();
        var maxPwdFailCount = config?.PwdLockThreshold ?? DefaultMaxPwdFailCount;
        if (user.PwdFailCnt >= maxPwdFailCount)
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

        var isAdmin = user.UserType == "A";
        var mustChangePwd = user.MustChangePwdYn == "Y";

        await _userRepo.UpdateLoginSuccessAsync(user.UserId);
        await _userRepo.InsertLoginHistAsync(user.UserId, clientIp, request.ClientVersion, "SUCCESS");

        var userInfo = new UserInfoDto
        {
            UserId = user.UserId,
            UserNm = user.UserNm,
            EmpNo = user.EmpNo,
            DeptNm = user.DeptNm,
            DeveloperYn = user.DeveloperYn == "Y",
            UserType = user.UserType,
            AccId = user.AccId,
            AccNm = user.AccNm
        };

        // 비밀번호를 먼저 바꿔야 하는 계정은 메뉴권한/바로가기를 조회할 필요가 없다 - 어차피
        // MustChangePasswordFilter가 이 토큰으로는 change-password 말고 아무것도 못 부르게 막는다.
        if (mustChangePwd)
        {
            return new LoginResponse
            {
                Success = true,
                RequirePasswordChange = true,
                AccessToken = _jwtTokenService.CreateAccessToken(user.UserId, isAdmin, mustChangePwd: true),
                RefreshToken = _jwtTokenService.CreateRefreshToken(),
                UserInfo = userInfo
            };
        }

        var menuDtos = await _menuPermissionService.GetEffectivePermissionsAsync(user.UserId);
        var shortcuts = await _shortcutRepo.GetEffectiveShortcutsAsync(user.UserId);

        return new LoginResponse
        {
            Success = true,
            AccessToken = _jwtTokenService.CreateAccessToken(user.UserId, isAdmin),
            RefreshToken = _jwtTokenService.CreateRefreshToken(),
            UserInfo = userInfo,
            Menus = menuDtos,
            Shortcuts = shortcuts
        };
    }

    /// <summary>
    /// 계정 존재 여부/이메일 등록 여부에 따라 성공/실패를 다르게 알려준다(사장님 지시,
    /// 2026-09-02) - 원래는 "계정 존재 여부를 캐는 수단이 되지 않도록" 항상 같은 메시지만
    /// 보여주려고 했었는데, 그러면 아이디를 잘못 입력한 진짜 사용자도 "접수되었습니다"만 보고
    /// 오지도 않을 메일을 기다리게 되는 문제가 있었다. 사내 전용 시스템이라 계정 목록이
    /// 노출되는 위험보다 이 UX 문제가 더 크다고 판단해서, 명확한 성공/실패 구분으로 바꿨다
    /// (아이디 존재 여부가 노출되는 트레이드오프는 사장님이 인지하고 승인함).
    /// </summary>
    public async Task<ApiResult> RequestPasswordResetAsync(string userId)
    {
        var session = await _userRepo.GetSessionAsync(userId);
        var user = session.User;

        if (user == null || string.IsNullOrWhiteSpace(user.Email))
            return new ApiResult { Success = false, Message = "아이디를 찾을 수 없거나 등록된 이메일이 없습니다. 아이디를 확인하거나 관리자에게 문의하세요." };

        var config = await _siteConfig.GetAsync();
        var expireMinutes = config?.PwdResetCodeValidMin ?? DefaultResetCodeExpireMinutes;
        var code = GenerateCode();
        var expireAt = DateTime.Now.AddMinutes(expireMinutes); // GETDATE()와 같은 기준(서버 로컬시간)으로 비교되므로 UtcNow를 쓰면 안 됨
        await _pwdResetRepo.IssueCodeAsync(user.UserId, code, expireAt);

        try
        {
            await _emailService.SendAsync(user.Email!, "[WYNLAB] 비밀번호 재설정 인증코드",
                $"인증코드: {code}\n\n{expireMinutes}분 안에 로그인 화면에서 입력해주세요.\n본인이 요청하지 않았다면 이 메일을 무시하셔도 됩니다.");
        }
        catch (Exception ex)
        {
            // "아이디/이메일이 틀렸다"는 메시지와 분명히 구분한다 - 여기 온 시점엔 계정은 이미
            // 정상 확인됐고 메일 발송(SMTP 연결/인증/전송)만 실패한 것이므로, 사용자가 자기 아이디를
            // 의심하며 헤매지 않게 한다(2026-09-10 - "인증코드받기 누르고 죽어버렸어"로 시작된 조사,
            // 원인은 EmailService에 타임아웃이 없어 최대 100초간 멈춘 것처럼 보였던 것). 예외
            // 상세는 사용자에게 노출하지 않는다(이 엔드포인트는 [AllowAnonymous] - SMTP 서버/설정
            // 정보가 새어나가면 안 됨) - 서버 콘솔에만 남긴다(구조화 로거 도입 전까지의 임시 방편).
            Console.Error.WriteLine($"[RequestPasswordResetAsync] 메일 발송 실패 (userId={userId}): {ex}");
            return new ApiResult { Success = false, Message = "메일 발송에 실패했습니다. 잠시 후 다시 시도하시거나 관리자에게 문의하세요." };
        }

        return new ApiResult { Success = true, Message = "정상적으로 요청이 접수되었습니다." };
    }

    public async Task<LoginResponse> ConfirmPasswordResetAsync(string userId, string code, string newPassword, string clientIp)
    {
        var policyError = await ValidatePasswordPolicyAsync(newPassword);
        if (policyError != null) return Fail(policyError);

        var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        var result = await _pwdResetRepo.ConsumeCodeAsync(userId, code, newHash);

        if (!result.IsSuccess)
            return Fail(result.FailMessage ?? "인증코드가 올바르지 않거나 만료되었습니다.");

        // 새 비밀번호가 이미 반영됐으니 그대로 로그인까지 처리해서 사용자가 한 번 더 입력할 필요 없게 한다.
        return await LoginAsync(new LoginRequest { UserId = userId, Password = newPassword, ClientVersion = string.Empty }, clientIp);
    }

    public async Task<ApiResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var session = await _userRepo.GetSessionAsync(userId);
        var user = session.User;
        if (user == null) return new ApiResult { Success = false, Message = "사용자를 찾을 수 없습니다." };

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            return new ApiResult { Success = false, Message = "현재 비밀번호가 올바르지 않습니다." };

        var policyError = await ValidatePasswordPolicyAsync(newPassword);
        if (policyError != null) return new ApiResult { Success = false, Message = policyError };

        var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        var result = await _userManageRepo.ChangePasswordAsync(userId, newHash);

        return result.IsSuccess
            ? new ApiResult { Success = true }
            : new ApiResult { Success = false, Message = result.FailMessage };
    }

    /// <summary>TSMSITECONFIG.pwd_min_length/pwd_require_* (frmSiteConfig 비밀번호정책 탭) 기준
    /// 새 비밀번호를 검증한다 - 값이 전부 NULL(미설정)이면 통과시킨다(정책을 안 정한 설치는
    /// 예전처럼 아무 제약도 없음). ChangePasswordAsync/ConfirmPasswordResetAsync 둘 다 새
    /// 비밀번호를 실제로 반영하기 "전"에 호출해야 한다.</summary>
    private async Task<string?> ValidatePasswordPolicyAsync(string newPassword)
    {
        var config = await _siteConfig.GetAsync();
        if (config == null) return null;

        if (config.PwdMinLength is int minLength && newPassword.Length < minLength)
            return $"비밀번호는 최소 {minLength}자 이상이어야 합니다.";

        if (config.PwdRequireUpperLower && !(newPassword.Any(char.IsUpper) && newPassword.Any(char.IsLower)))
            return "비밀번호는 영문 대/소문자를 모두 포함해야 합니다.";

        if (config.PwdRequireDigit && !newPassword.Any(char.IsDigit))
            return "비밀번호는 숫자를 포함해야 합니다.";

        if (config.PwdRequireSpecial && !newPassword.Any(c => !char.IsLetterOrDigit(c)))
            return "비밀번호는 특수문자를 포함해야 합니다.";

        return null;
    }

    /// <summary>암호학적으로 안전한 난수로 6자리 코드를 만든다(System.Random 아님 - 이건 보안에
    /// 쓰이는 값이라 예측 가능하면 안 된다).</summary>
    private static string GenerateCode() => RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

    private static LoginResponse Fail(string message) => new() { Success = false, Message = message };
}
