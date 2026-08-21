using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IMenuManageRepository
{
    Task<List<MenuManageRow>> GetAllAsync();
    Task<bool> ExistsAsync(string menuCd);
    Task CreateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder);
    Task UpdateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, bool useYn);
    Task SetUseYnAsync(string menuCd, bool useYn);
}

/// <summary>DB 조회 전용 - 관리화면 목록 표시용</summary>
public class MenuManageRow
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? FormClassNm { get; set; }
    public string? IconNm { get; set; }
    public int SortOrder { get; set; }
    public string UseYn { get; set; } = "Y";
}

public class MenuManageRepository : IMenuManageRepository
{
    private readonly IDapperContext _context;

    public MenuManageRepository(IDapperContext context) => _context = context;

    public async Task<List<MenuManageRow>> GetAllAsync()
    {
        const string sql = @"
            SELECT MENU_CD AS MenuCd, MENU_NM AS MenuNm, UPPER_MENU_CD AS UpperMenuCd,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, FORM_CLASS_NM AS FormClassNm,
                   ICON_NM AS IconNm, SORT_ORDER AS SortOrder, USE_YN AS UseYn
            FROM TSMMENU
            ORDER BY UPPER_MENU_CD, SORT_ORDER";

        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuManageRow>(sql);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string menuCd)
    {
        const string sql = "SELECT COUNT(1) FROM TSMMENU WHERE MENU_CD = @MenuCd";
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(sql, new { MenuCd = menuCd });
        return count > 0;
    }

    public async Task CreateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder)
    {
        const string sql = @"
            INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN)
            VALUES (@MenuCd, @MenuNm, @UpperMenuCd, @MenuLevel, @MenuType, @FormClassNm, @IconNm, @SortOrder, 'Y')";

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new
        {
            MenuCd = menuCd,
            MenuNm = menuNm,
            UpperMenuCd = string.IsNullOrWhiteSpace(upperMenuCd) ? null : upperMenuCd,
            MenuLevel = menuLevel,
            MenuType = menuType,
            FormClassNm = formClassNm,
            IconNm = iconNm,
            SortOrder = sortOrder
        });
    }

    public async Task UpdateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, bool useYn)
    {
        const string sql = @"
            UPDATE TSMMENU SET
                MENU_NM = @MenuNm, UPPER_MENU_CD = @UpperMenuCd, MENU_LEVEL = @MenuLevel,
                MENU_TYPE = @MenuType, FORM_CLASS_NM = @FormClassNm, ICON_NM = @IconNm,
                SORT_ORDER = @SortOrder, USE_YN = @UseYn
            WHERE MENU_CD = @MenuCd";

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new
        {
            MenuCd = menuCd,
            MenuNm = menuNm,
            UpperMenuCd = string.IsNullOrWhiteSpace(upperMenuCd) ? null : upperMenuCd,
            MenuLevel = menuLevel,
            MenuType = menuType,
            FormClassNm = formClassNm,
            IconNm = iconNm,
            SortOrder = sortOrder,
            UseYn = useYn ? "Y" : "N"
        });
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 하위 메뉴 참조무결성 보존을 위한 표준 삭제 방식</summary>
    public async Task SetUseYnAsync(string menuCd, bool useYn)
    {
        const string sql = "UPDATE TSMMENU SET USE_YN = @UseYn WHERE MENU_CD = @MenuCd";
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(sql, new { MenuCd = menuCd, UseYn = useYn ? "Y" : "N" });
    }
}
