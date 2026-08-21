using Dapper;
using NEXTFramework.Api.Data;

namespace NEXTFramework.Api.Repositories;

public class MenuRow
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? FormClassNm { get; set; }
    public string? IconNm { get; set; }
    public int SortOrder { get; set; }
}

public class MenuAuthRow
{
    public string MenuCd { get; set; } = string.Empty;
    public string AuthTargetType { get; set; } = string.Empty; // USER / GRP
    public string AuthTargetCd { get; set; } = string.Empty;
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
}

public interface IMenuRepository
{
    Task<List<MenuRow>> GetAllActiveMenusAsync();

    /// <summary>사용자가 속한 그룹들(GRP) + 본인(USER)에 걸린 권한행을 한 번에 조회</summary>
    Task<List<MenuAuthRow>> GetMenuAuthRowsAsync(string userId, List<string> groupCodes);
}

public class MenuRepository : IMenuRepository
{
    private readonly IDapperContext _context;

    public MenuRepository(IDapperContext context) => _context = context;

    public async Task<List<MenuRow>> GetAllActiveMenusAsync()
    {
        const string sql = @"
            SELECT MENU_CD AS MenuCd, MENU_NM AS MenuNm, UPPER_MENU_CD AS UpperMenuCd,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, FORM_CLASS_NM AS FormClassNm,
                   ICON_NM AS IconNm, SORT_ORDER AS SortOrder
            FROM TSMMENU
            WHERE USE_YN = 'Y'
            ORDER BY SORT_ORDER";

        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuRow>(sql);
        return result.ToList();
    }

    public async Task<List<MenuAuthRow>> GetMenuAuthRowsAsync(string userId, List<string> groupCodes)
    {
        // GRP 권한(사용자가 속한 모든 그룹) + USER 권한(본인) 을 한 번에 조회
        // 그룹 목록이 비어있으면 GRP 조건은 자동으로 매치되는 행이 없음(정상)
        const string sql = @"
            SELECT MENU_CD AS MenuCd, AUTH_TARGET_TYPE AS AuthTargetType, AUTH_TARGET_CD AS AuthTargetCd,
                   VIEW_YN AS ViewYn, INSERT_YN AS InsertYn, UPDATE_YN AS UpdateYn,
                   DELETE_YN AS DeleteYn, EXCEL_YN AS ExcelYn
            FROM TSMMENUAUTH
            WHERE (AUTH_TARGET_TYPE = 'USER' AND AUTH_TARGET_CD = @UserId)
               OR (AUTH_TARGET_TYPE = 'GRP'  AND AUTH_TARGET_CD IN @GroupCodes)";

        using var conn = _context.CreateConnection();
        // IN 절에 빈 리스트가 들어가면 Dapper가 오류를 내므로 더미값 처리
        var safeGroupCodes = groupCodes.Count > 0 ? groupCodes : new List<string> { "__NONE__" };
        var result = await conn.QueryAsync<MenuAuthRow>(sql, new { UserId = userId, GroupCodes = safeGroupCodes });
        return result.ToList();
    }
}
