using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Services;

/// <summary>
/// 로그인 사용자 기준으로 메뉴별 최종(병합) 권한을 계산하는 공용 서비스.
///
/// 로그인시(AuthService)엔 화면 전체 목록에 대한 권한이 필요하고, API 액션 하나하나를
/// 서버에서 지킬 때(RequireMenuPermissionAttribute)는 메뉴 1개에 대한 권한만 필요해서
/// 계산 로직(관리자 전체허용 + MenuPermissionMerger 합산)을 이 서비스 하나로 통일했다 -
/// 두 곳에서 각자 구현하면 나중에 한쪽만 고치는 실수가 생기기 쉽다.
/// </summary>
public interface IMenuPermissionService
{
    /// <summary>로그인시 클라이언트에 내려줄 전체 메뉴 권한 목록</summary>
    Task<List<MenuDto>> GetEffectivePermissionsAsync(string userId);

    /// <summary>
    /// API 액션 하나가 특정 메뉴에 대해 특정 권한(조회/등록/수정/삭제/엑셀)이 있는지 확인할 때 사용.
    /// 범용 데이터 통로(api/data/*)처럼 화면이 자기 MenuId를 요청에 실어 보내는 경우에 쓴다.
    /// 사용자가 없거나 사용중지 상태거나 그 메뉴 자체가 없으면 null(권한 없음으로 취급).
    /// </summary>
    Task<MenuDto?> GetEffectivePermissionAsync(string userId, long menuId);

    /// <summary>
    /// [RequireMenuPermission(module, screenClassNm, action)] 전용 - MENU_ID는 환경(DB)마다
    /// IDENTITY로 다르게 채번되어 컴파일된 코드에 상수로 박아넣을 수 없다(로컬DB의 5번이 운영DB
    /// 에선 다른 메뉴일 수 있음). Module+ScreenClassNm(예: "SM"+"frmMenu")은 실제 화면 클래스
    /// 이름 그대로라 환경이 바뀌어도 항상 같은 값이라 이걸로 대신 찾는다.
    /// </summary>
    Task<MenuDto?> GetEffectivePermissionByKeyAsync(string userId, string module, string screenClassNm);
}

public class MenuPermissionService : IMenuPermissionService
{
    private readonly IUserRepository _userRepo;
    private readonly IMenuRepository _menuRepo;

    public MenuPermissionService(IUserRepository userRepo, IMenuRepository menuRepo)
    {
        _userRepo = userRepo;
        _menuRepo = menuRepo;
    }

    public async Task<List<MenuDto>> GetEffectivePermissionsAsync(string userId)
    {
        var menus = await GetPermissionsCoreAsync(userId);
        if (menus.Count == 0) return menus;

        // 메뉴에 켜진 화면 기능(전자결재/첨부파일...)은 사용자 권한과 무관한 메뉴 설정이라 권한 계산 뒤에 한 번에 붙인다.
        var features = (await _menuRepo.GetActiveFeaturesAsync()).ToLookup(f => f.MenuId);
        foreach (var menu in menus)
            menu.Features = features[menu.MenuId].Select(f => new MenuFeatureDto { FeatureCd = f.FeatureCd, UseYn = true, OptionVal = f.OptionVal }).ToList();
        return menus;
    }

    private async Task<List<MenuDto>> GetPermissionsCoreAsync(string userId)
    {
        var session = await _userRepo.GetSessionAsync(userId);
        var user = session.User;
        if (user == null || user.UseYn != "Y") return new List<MenuDto>();

        var menus = await _menuRepo.GetAllActiveMenusAsync();
        var isAdmin = user.UserType == "A";

        if (isAdmin)
        {
            return menus.Select(m => ToDto(m, allowAll: true)).ToList();
        }

        var authRows = await _menuRepo.GetMenuAuthRowsAsync(userId, session.GroupCodes);
        return MenuPermissionMerger.Merge(menus, authRows);
    }

    public async Task<MenuDto?> GetEffectivePermissionAsync(string userId, long menuId)
    {
        var permissions = await GetEffectivePermissionsAsync(userId);
        return permissions.FirstOrDefault(m => m.MenuId == menuId);
    }

    public async Task<MenuDto?> GetEffectivePermissionByKeyAsync(string userId, string module, string screenClassNm)
    {
        var permissions = await GetEffectivePermissionsAsync(userId);
        return permissions.FirstOrDefault(m =>
            string.Equals(m.Module, module, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(m.ScreenClassNm, screenClassNm, StringComparison.OrdinalIgnoreCase));
    }

    private static MenuDto ToDto(MenuRow m, bool allowAll) => new()
    {
        MenuId = m.MenuId,
        MenuNm = m.MenuNm,
        UpperMenuId = m.UpperMenuId,
        MenuLevel = m.MenuLevel,
        MenuType = m.MenuType,
        Module = m.Module,
        ScreenClassNm = m.ScreenClassNm,
        IconNm = m.IconNm,
        SortOrder = m.SortOrder,
        ViewYn = allowAll,
        InsertYn = allowAll,
        UpdateYn = allowAll,
        DeleteYn = allowAll,
        PrintYn = allowAll,
        ExcelYn = allowAll,
        Auth = Enumerable.Repeat(allowAll, 10).ToArray()
    };
}
