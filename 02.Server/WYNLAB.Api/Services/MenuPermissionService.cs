using WYNLAB.Api.Repositories;
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
    /// 사용자가 없거나 사용중지 상태거나 그 메뉴 자체가 없으면 null(권한 없음으로 취급).
    /// </summary>
    Task<MenuDto?> GetEffectivePermissionAsync(string userId, string menuCd);
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
        var session = await _userRepo.GetSessionAsync(userId);
        var user = session.User;
        if (user == null || user.UseYn != "Y") return new List<MenuDto>();

        var menus = await _menuRepo.GetAllActiveMenusAsync();
        var isAdmin = user.IsAdminYn == "Y";

        if (isAdmin)
        {
            return menus.Select(m => ToDto(m, allowAll: true)).ToList();
        }

        var authRows = await _menuRepo.GetMenuAuthRowsAsync(userId, session.GroupCodes);
        return MenuPermissionMerger.Merge(menus, authRows);
    }

    public async Task<MenuDto?> GetEffectivePermissionAsync(string userId, string menuCd)
    {
        var permissions = await GetEffectivePermissionsAsync(userId);
        return permissions.FirstOrDefault(m => m.MenuCd == menuCd);
    }

    private static MenuDto ToDto(MenuRow m, bool allowAll) => new()
    {
        MenuCd = m.MenuCd,
        MenuNm = m.MenuNm,
        UpperMenuCd = m.UpperMenuCd,
        MenuLevel = m.MenuLevel,
        MenuType = m.MenuType,
        FormClassNm = m.FormClassNm,
        IconNm = m.IconNm,
        SortOrder = m.SortOrder,
        ViewYn = allowAll,
        InsertYn = allowAll,
        UpdateYn = allowAll,
        DeleteYn = allowAll,
        ExcelYn = allowAll
    };
}
