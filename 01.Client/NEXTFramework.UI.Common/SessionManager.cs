using NEXTFramework.Shared.Dtos;

namespace NEXTFramework.UI.Common;

/// <summary>
/// 로그인한 사용자 정보/토큰/메뉴권한을 앱 전역에서 참조하는 싱글턴.
///
/// UI.Common에 위치하는 이유: BaseForm과 모든 업무모듈 DLL(Modules.Sales 등)이
/// UI.Common을 참조하므로, 여기 두어야 어느 화면에서든 세션값을 꺼내쓸 수 있다.
/// (Shell 프로젝트에만 두면 모듈 DLL에서 접근 불가 - 참조 방향이 반대가 되어버림)
///
/// UserInfo의 각 필드(UserId, EmpNo, DeptCd 등)는 로그인시 서버의
/// USP_SM_GetUserSession 프로시저 조회 결과를 그대로 담고 있다.
/// </summary>
public sealed class SessionManager
{
    private static readonly Lazy<SessionManager> _instance = new(() => new SessionManager());
    public static SessionManager Current => _instance.Value;

    public UserInfoDto? UserInfo { get; private set; }
    public string? AccessToken { get; private set; }
    public List<MenuDto> Menus { get; private set; } = new();

    public bool IsSignedIn => UserInfo != null;

    private SessionManager() { }

    public void SignIn(LoginResponse response)
    {
        UserInfo = response.UserInfo;
        AccessToken = response.AccessToken;
        Menus = response.Menus;
    }

    public MenuDto? GetMenuAuth(string menuCd) =>
        Menus.FirstOrDefault(m => m.MenuCd == menuCd);

    public void SignOut()
    {
        UserInfo = null;
        AccessToken = null;
        Menus.Clear();
    }
}
