using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

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
    public string UpdateYn { get; set; } = "N"; // TSMMENUAUTH.SAVE_YN - C# 이름은 그대로 유지(하위호환)
    public string DeleteYn { get; set; } = "N";
    public string PrintYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
    // TSMMENUAUTH.AUTH01~10 - 조회/입력/저장/출력/엑셀 이외에 화면마다 추가로 분리해야 하는
    // 권한이 생겼을 때 쓰는 예비 슬롯(사장님 지시, 2026-08-31). Dapper가 컬럼별로 매핑하도록
    // 10개를 각각 선언한다(배열은 자동매핑이 안 됨) - MenuPermissionMerger.ToAuthArray 참고.
    public string Auth01 { get; set; } = "N";
    public string Auth02 { get; set; } = "N";
    public string Auth03 { get; set; } = "N";
    public string Auth04 { get; set; } = "N";
    public string Auth05 { get; set; } = "N";
    public string Auth06 { get; set; } = "N";
    public string Auth07 { get; set; } = "N";
    public string Auth08 { get; set; } = "N";
    public string Auth09 { get; set; } = "N";
    public string Auth10 { get; set; } = "N";
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
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuRow>("USP_SM_MENU_Q_2",
            new { p_work_type = "Q" }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<List<MenuAuthRow>> GetMenuAuthRowsAsync(string userId, List<string> groupCodes)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuAuthRow>("USP_SM_MENUAUTH_Q",
            new { p_work_type = "Q", p_user_id = userId, p_group_codes = groupCodes.Count > 0 ? string.Join(",", groupCodes) : null },
            commandType: CommandType.StoredProcedure);
        return result.ToList();
    }
}
