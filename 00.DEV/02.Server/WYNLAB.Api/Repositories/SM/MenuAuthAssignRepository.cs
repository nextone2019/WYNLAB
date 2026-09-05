using System.Data;
using System.Text.Json;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

public interface IMenuAuthAssignRepository
{
    Task<List<MenuAuthAssignRow>> GetAuthAsync(string targetType, string targetCd);
    Task<ProcResult> SaveAuthAsync(string targetType, string targetCd, List<MenuAuthAssignItem> items);
    Task<List<MenuAuthByMenuRow>> GetAuthByMenuAsync(long menuId, string targetType);
    Task<ProcResult> SaveAuthByMenuAsync(long menuId, string targetType, List<MenuAuthByMenuItem> items);
}

public class MenuAuthAssignRow
{
    public long MenuId { get; set; }
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public string MenuType { get; set; } = "FORM";
    public int SortOrder { get; set; }
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string PrintYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
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
    public string? Auth01Nm { get; set; }
    public string? Auth02Nm { get; set; }
    public string? Auth03Nm { get; set; }
    public string? Auth04Nm { get; set; }
    public string? Auth05Nm { get; set; }
    public string? Auth06Nm { get; set; }
    public string? Auth07Nm { get; set; }
    public string? Auth08Nm { get; set; }
    public string? Auth09Nm { get; set; }
    public string? Auth10Nm { get; set; }
}

/// <summary>메뉴기준 권한관리(frmMenuAuth) 조회용 - MenuAuthAssignRow의 반대 축(메뉴 1건 고정,
/// 대상 여러 명). TargetNm/SubNm은 표시 전용(사용자=이름/부서명, 그룹=그룹명/인원수).</summary>
public class MenuAuthByMenuRow
{
    public string TargetCd { get; set; } = string.Empty;
    public string TargetNm { get; set; } = string.Empty;
    public string? SubNm { get; set; }
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string PrintYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
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

/// <summary>OPENJSON으로 USP_SM_MENUAUTH_S_2에 넘길 저장용 행 - Y/N 문자열로 직렬화(SQL측
/// CHAR(1)과 맞춤). TargetCd만 MenuAuthAssignItem과 다르고 나머지 권한 필드는 동일 구조.</summary>
public class MenuAuthByMenuItem
{
    public string TargetCd { get; set; } = string.Empty;
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string PrintYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
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

/// <summary>OPENJSON으로 프로시저에 넘길 저장용 행 - Y/N 문자열로 직렬화(SQL측 CHAR(1)과 맞춤).
/// MenuId는 USP_SM_MENUAUTH_S_1의 OPENJSON WITH절이 "$.MenuId"로 읽으므로 이름이 일치해야 한다.</summary>
public class MenuAuthAssignItem
{
    public long MenuId { get; set; }
    public string ViewYn { get; set; } = "N";
    public string InsertYn { get; set; } = "N";
    public string UpdateYn { get; set; } = "N";
    public string DeleteYn { get; set; } = "N";
    public string PrintYn { get; set; } = "N";
    public string ExcelYn { get; set; } = "N";
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
            new { p_work_type = "Q", p_target_type = targetType, p_target_cd = targetCd }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>권한 전체 치환 - work_type='U'</summary>
    public async Task<ProcResult> SaveAuthAsync(string targetType, string targetCd, List<MenuAuthAssignItem> items)
    {
        var itemsJson = JsonSerializer.Serialize(items);

        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_target_type", targetType);
        p.Add("p_target_cd", targetCd);
        p.Add("p_items_json", itemsJson);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENUAUTH_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    /// <summary>메뉴기준 권한관리(frmMenuAuth) 조회 - 메뉴 1건 + 대상유형(USER/GRP) 기준으로
    /// 전체 대상 목록과 각자의 권한을 한 번에 가져온다.</summary>
    public async Task<List<MenuAuthByMenuRow>> GetAuthByMenuAsync(long menuId, string targetType)
    {
        using var conn = _context.CreateConnection();
        var result = await conn.QueryAsync<MenuAuthByMenuRow>("USP_SM_MENUAUTH_Q_2",
            new { p_work_type = "Q", p_menu_id = menuId, p_target_type = targetType }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>메뉴 1건 + 대상유형 기준 권한 전체 치환 - work_type='U'(USP_SM_MENUAUTH_S_1과 같은 방식)</summary>
    public async Task<ProcResult> SaveAuthByMenuAsync(long menuId, string targetType, List<MenuAuthByMenuItem> items)
    {
        var itemsJson = JsonSerializer.Serialize(items);

        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_menu_id", menuId);
        p.Add("p_target_type", targetType);
        p.Add("p_items_json", itemsJson);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENUAUTH_S_2", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
