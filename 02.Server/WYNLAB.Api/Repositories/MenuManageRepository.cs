using System.Data;
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

/// <summary>
/// 데이터 처리는 전부 저장프로시저(USP_SM_MENU_*)로 위임한다.
/// 명명규칙: USP_SM_MENU_Q(조회) / _Q_1(보조조회) / _S(저장) / _S_1(추가 저장동작)
/// </summary>
public class MenuManageRepository : IMenuManageRepository
{
    private readonly IDapperContext _context;

    public MenuManageRepository(IDapperContext context) => _context = context;

    public async Task<List<MenuManageRow>> GetAllAsync()
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuManageRow>("USP_SM_MENU_Q", commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string menuCd)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_MENU_Q_1",
            new { MenuCd = menuCd }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task CreateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_MENU_S", new
        {
            Mode = "C",
            MenuCd = menuCd,
            MenuNm = menuNm,
            UpperMenuCd = string.IsNullOrWhiteSpace(upperMenuCd) ? null : upperMenuCd,
            MenuLevel = menuLevel,
            MenuType = menuType,
            FormClassNm = formClassNm,
            IconNm = iconNm,
            SortOrder = sortOrder
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, bool useYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_MENU_S", new
        {
            Mode = "U",
            MenuCd = menuCd,
            MenuNm = menuNm,
            UpperMenuCd = string.IsNullOrWhiteSpace(upperMenuCd) ? null : upperMenuCd,
            MenuLevel = menuLevel,
            MenuType = menuType,
            FormClassNm = formClassNm,
            IconNm = iconNm,
            SortOrder = sortOrder,
            UseYn = useYn ? "Y" : "N"
        }, commandType: CommandType.StoredProcedure);
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 하위 메뉴 참조무결성 보존을 위한 표준 삭제 방식</summary>
    public async Task SetUseYnAsync(string menuCd, bool useYn)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_MENU_S_1",
            new { MenuCd = menuCd, UseYn = useYn ? "Y" : "N" }, commandType: CommandType.StoredProcedure);
    }
}
