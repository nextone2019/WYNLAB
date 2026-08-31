using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

public interface IMenuManageRepository
{
    Task<List<MenuManageRow>> GetAllAsync();
    Task<bool> ExistsAsync(string menuCd);
    Task<ProcResult> CreateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, string?[] authNm);
    Task<ProcResult> UpdateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, bool useYn, string?[] authNm);
    Task<ProcResult> SetUseYnAsync(string menuCd, bool useYn);
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
        var result = await conn.QueryAsync<MenuManageRow>("USP_SM_MENU_Q",
            new { p_work_type = "Q" }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<bool> ExistsAsync(string menuCd)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_MENU_Q_1",
            new { p_work_type = "Q", p_menu_cd = menuCd }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task<ProcResult> CreateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, string?[] authNm)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_menu_cd", menuCd);
        p.Add("p_menu_nm", menuNm);
        p.Add("p_upper_menu_cd", string.IsNullOrWhiteSpace(upperMenuCd) ? null : upperMenuCd);
        p.Add("p_menu_level", menuLevel);
        p.Add("p_menu_type", menuType);
        p.Add("p_form_class_nm", formClassNm);
        p.Add("p_icon_nm", iconNm);
        p.Add("p_sort_order", sortOrder);
        AddAuthNmParams(p, authNm);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENU_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> UpdateAsync(string menuCd, string menuNm, string? upperMenuCd, int menuLevel,
        string menuType, string? formClassNm, string? iconNm, int sortOrder, bool useYn, string?[] authNm)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_menu_cd", menuCd);
        p.Add("p_menu_nm", menuNm);
        p.Add("p_upper_menu_cd", string.IsNullOrWhiteSpace(upperMenuCd) ? null : upperMenuCd);
        p.Add("p_menu_level", menuLevel);
        p.Add("p_menu_type", menuType);
        p.Add("p_form_class_nm", formClassNm);
        p.Add("p_icon_nm", iconNm);
        p.Add("p_sort_order", sortOrder);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        AddAuthNmParams(p, authNm);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENU_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    /// <summary>authNm[0..9] -> p_auth01_nm..p_auth10_nm. 배열이 10칸보다 짧으면 나머지는 NULL.</summary>
    private static void AddAuthNmParams(DynamicParameters p, string?[] authNm)
    {
        for (var i = 0; i < 10; i++)
            p.Add($"p_auth{(i + 1):00}_nm", i < authNm.Length ? authNm[i] : null);
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 하위 메뉴 참조무결성 보존을 위한 표준 삭제 방식(work_type='D')</summary>
    public async Task<ProcResult> SetUseYnAsync(string menuCd, bool useYn)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_menu_cd", menuCd);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENU_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
