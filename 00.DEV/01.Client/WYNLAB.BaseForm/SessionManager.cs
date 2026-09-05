using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 로그인한 사용자 정보/토큰/메뉴권한을 앱 전역에서 참조하는 싱글턴.
///
/// UI.Common에 위치하는 이유: BaseForm과 모든 업무모듈 DLL(Modules.Sales 등)이
/// UI.Common을 참조하므로, 여기 두어야 어느 화면에서든 세션값을 꺼내쓸 수 있다.
/// (Shell 프로젝트에만 두면 모듈 DLL에서 접근 불가 - 참조 방향이 반대가 되어버림)
///
/// UserInfo의 각 필드(UserId, EmpNo, DeptCd 등)는 로그인시 서버의
/// SSP_WYNLAB_GetSession 프로시저 조회 결과를 그대로 담고 있다.
/// </summary>
public sealed class SessionManager
{
    private static readonly Lazy<SessionManager> _instance = new(() => new SessionManager());
    public static SessionManager Current => _instance.Value;

    public UserInfoDto? UserInfo { get; private set; }
    public string? AccessToken { get; private set; }
    public List<MenuDto> Menus { get; private set; } = new();

    /// <summary>로그인 사용자의 유효 단축키(기본값+재정의 병합) - ShellForm.ProcessCmdKey가
    /// 눌린 키를 ShortcutKeys.ToText로 바꿔 여기 KeyCombo와 비교해서 어떤 툴바 액션을 부를지 찾는다.</summary>
    public List<ShortcutDto> Shortcuts { get; private set; } = new();

    /// <summary>이번 세션에서 로그인한 시각(클라이언트 로컬시간) - 셸 사이드바에 표시용</summary>
    public DateTime? SignInTime { get; private set; }

    public bool IsSignedIn => UserInfo != null;

    /// <summary>최근 열어본 화면(최신순, 최대 RecentMenusMaxCount개) - 홈 대시보드 "최근 사용" 카드용.
    /// 로그인 세션 안에서만 유지되고 서버엔 저장하지 않는다(가벼운 UX 편의 기능).</summary>
    public IReadOnlyList<MenuDto> RecentMenus => _recentMenus;
    private readonly List<MenuDto> _recentMenus = new();
    private const int RecentMenusMaxCount = 8;

    private SessionManager() { }

    public void SignIn(LoginResponse response)
    {
        UserInfo = response.UserInfo;
        AccessToken = response.AccessToken;
        Menus = response.Menus;
        Shortcuts = response.Shortcuts;
        SignInTime = DateTime.Now;
    }

    /// <summary>단축키 설정화면(frmShortcut)이 저장/초기화 성공 후 세션 캐시를 다시 맞추려고 부른다 -
    /// 재로그인 없이도 그 자리에서 바로 새 단축키가 전역에 반영된다.</summary>
    public void ReplaceShortcuts(List<ShortcutDto> shortcuts) => Shortcuts = shortcuts;

    public MenuDto? GetMenuAuth(long menuId) =>
        Menus.FirstOrDefault(m => m.MenuId == menuId);

    /// <summary>화면을 열 때마다 호출 - 이미 목록에 있으면 맨 앞으로 이동(MRU), 없으면 앞에 추가하고 초과분은 버림</summary>
    public void AddRecentMenu(MenuDto menu)
    {
        _recentMenus.RemoveAll(m => m.MenuId == menu.MenuId);
        _recentMenus.Insert(0, menu);
        if (_recentMenus.Count > RecentMenusMaxCount)
            _recentMenus.RemoveRange(RecentMenusMaxCount, _recentMenus.Count - RecentMenusMaxCount);
    }

    public void SignOut()
    {
        UserInfo = null;
        AccessToken = null;
        Menus.Clear();
        Shortcuts.Clear();
        _recentMenus.Clear();
        SignInTime = null;
    }
}
