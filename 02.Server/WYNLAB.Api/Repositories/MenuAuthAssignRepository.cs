using System.Data;
using System.Text.Json;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories;

public interface IMenuAuthAssignRepository
{
    Task<List<MenuAuthAssignRow>> GetAuthAsync(string targetType, string targetCd);
    Task SaveAuthAsync(string targetType, string targetCd, List<MenuAuthAssignItem> items);
}

public class MenuAuthAssignRow
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public string MenuType { get; set; } = "FORM";
    public int SortOrder { get; set; }
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
}

/// <summary>OPENJSON으로 프로시저에 넘길 저장용 행 - Y/N 문자열로 직렬화(SQL측 CHAR(1)과 맞춤)</summary>
public class MenuAuthAssignItem
{
    public string MenuCd { get; set; } = string.Empty;
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
}

/// <summary>
/// 데이터 처리는 USP_SM_MENUAUTH_Q_1(조회)/_S_1(저장) 프로시저로 위임.
/// (USP_SM_MENUAUTH_Q/_S_2는 로그인시 권한병합용으로 별개 - AuthService/MenuRepository 참고)
/// </summary>
public class MenuAuthAssignRepository : IMenuAuthAssignRepository
{
    private readonly IDapperContext _context;

    public MenuAuthAssignRepository(IDapperContext context) => _context = context;

    public async Task<List<MenuAuthAssignRow>> GetAuthAsync(string targetType, string targetCd)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuAuthAssignRow>("USP_SM_MENUAUTH_Q_1",
            new { TargetType = targetType, TargetCd = targetCd }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task SaveAuthAsync(string targetType, string targetCd, List<MenuAuthAssignItem> items)
    {
        var itemsJson = JsonSerializer.Serialize(items);

        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("USP_SM_MENUAUTH_S_1",
            new { TargetType = targetType, TargetCd = targetCd, ItemsJson = itemsJson },
            commandType: CommandType.StoredProcedure);
    }
}
