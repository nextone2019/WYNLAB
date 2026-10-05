using System.Data;
using Dapper;
using WYNLAB.Api.Data;

namespace WYNLAB.Api.Repositories.SM;

public interface IMenuManageRepository
{
    Task<List<MenuManageRow>> GetAllAsync();
    Task<bool> ExistsAsync(long menuId);
    Task<ProcResult> CreateAsync(string menuNm, long? upperMenuId, int menuLevel,
        string menuType, string? module, string? screenClassNm, string? iconNm, string? procPrefix, int sortOrder, string?[] authNm,
        string userId, string? clientPc);
    Task<ProcResult> UpdateAsync(long menuId, string menuNm, long? upperMenuId, int menuLevel,
        string menuType, string? module, string? screenClassNm, string? iconNm, string? procPrefix, int sortOrder, bool useYn, string?[] authNm,
        string userId, string? clientPc);
    Task<ProcResult> SetUseYnAsync(long menuId, bool useYn, string userId, string? clientPc);

    /// <summary>이 메뉴에 설정된 화면 기능 전체(사용 안 함으로 꺼둔 것 포함) - 메뉴등록 화면의 "화면 기능" 영역 표시용.</summary>
    Task<List<WYNLAB.Shared.Dtos.MenuFeatureDto>> GetFeaturesAsync(long menuId);

    /// <summary>화면 기능 설정 저장 - 목록의 각 기능을 USP_SM_MENUFEATURE_S(SET)로 한 건씩 저장한다. 첫 실패에서 멈추고 그 결과를 돌려준다.</summary>
    Task<ProcResult> SaveFeaturesAsync(long menuId, IEnumerable<WYNLAB.Shared.Dtos.MenuFeatureDto> features, string userId, string? clientPc);
}

/// <summary>DB 조회 전용 - 관리화면 목록 표시용</summary>
public class MenuManageRow
{
    public long MenuId { get; set; }
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? Module { get; set; }
    public string? ScreenClassNm { get; set; }
    public string? IconNm { get; set; }
    public string? ProcPrefix { get; set; }
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
        // USP_SM_MENU_Q가 PROC_PREFIX AS ProcPrefix를 SELECT하므로(079 마이그레이션) Dapper가
        // MenuManageRow.ProcPrefix에 그대로 매핑한다 - 별도 코드 필요 없음.
        var result = await conn.QueryAsync<MenuManageRow>("USP_SM_MENU_Q",
            new { p_work_type = "Q" }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    /// <summary>USP_SM_MENU_Q_1은 예전엔 "신규등록 시 이 코드가 이미 있는지" 중복확인이었는데,
    /// MENU_ID가 IDENTITY라 이제 그럴 일이 없다 - 수정/삭제 전 "이 ID가 실제로 있는지" 확인
    /// 용도로 재사용한다.</summary>
    public async Task<bool> ExistsAsync(long menuId)
    {
        using var conn = _context.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>("USP_SM_MENU_Q_1",
            new { p_work_type = "Q", p_menu_id = menuId }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task<ProcResult> CreateAsync(string menuNm, long? upperMenuId, int menuLevel,
        string menuType, string? module, string? screenClassNm, string? iconNm, string? procPrefix, int sortOrder, string?[] authNm,
        string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_menu_nm", menuNm);
        p.Add("p_upper_menu_id", upperMenuId);
        p.Add("p_menu_level", menuLevel);
        p.Add("p_menu_type", menuType);
        p.Add("p_module", module);
        p.Add("p_screen_class_nm", screenClassNm);
        p.Add("p_icon_nm", iconNm);
        p.Add("p_proc_prefix", procPrefix);
        p.Add("p_sort_order", sortOrder);
        AddAuthNmParams(p, authNm);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENU_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> UpdateAsync(long menuId, string menuNm, long? upperMenuId, int menuLevel,
        string menuType, string? module, string? screenClassNm, string? iconNm, string? procPrefix, int sortOrder, bool useYn, string?[] authNm,
        string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_menu_id", menuId);
        p.Add("p_menu_nm", menuNm);
        p.Add("p_upper_menu_id", upperMenuId);
        p.Add("p_menu_level", menuLevel);
        p.Add("p_menu_type", menuType);
        p.Add("p_module", module);
        p.Add("p_screen_class_nm", screenClassNm);
        p.Add("p_icon_nm", iconNm);
        p.Add("p_proc_prefix", procPrefix);
        p.Add("p_sort_order", sortOrder);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        AddAuthNmParams(p, authNm);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
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

    public async Task<List<WYNLAB.Shared.Dtos.MenuFeatureDto>> GetFeaturesAsync(long menuId)
    {
        using var conn = _context.CreateConnection();
        // Dapper는 snake_case 컬럼을 PascalCase 속성에 자동 매핑하지 않으므로 별칭을 단다.
        var rows = await conn.QueryAsync<(string FeatureCd, string UseYn, string? OptionVal)>(
            "SELECT feature_cd AS FeatureCd, use_yn AS UseYn, option_val AS OptionVal FROM TSMMENUFEATURE WHERE menu_id = @menuId ORDER BY feature_cd",
            new { menuId });
        return rows.Select(r => new WYNLAB.Shared.Dtos.MenuFeatureDto { FeatureCd = r.FeatureCd, UseYn = r.UseYn == "Y", OptionVal = r.OptionVal }).ToList();
    }

    public async Task<ProcResult> SaveFeaturesAsync(long menuId, IEnumerable<WYNLAB.Shared.Dtos.MenuFeatureDto> features, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        ProcResult last = new();
        foreach (var f in features)
        {
            var p = new DynamicParameters();
            p.Add("p_work_type", "SET");
            p.Add("p_menu_id", menuId);
            p.Add("p_feature_cd", f.FeatureCd);
            p.Add("p_use_yn", f.UseYn ? "Y" : "N");
            p.Add("p_option_val", f.OptionVal);
            p.Add("p_user_id", userId);
            p.Add("p_client_pc", clientPc);
            p.AddStandardOutputs(pascalCase: true);

            await conn.ExecuteAsync("USP_SM_MENUFEATURE_S", p, commandType: CommandType.StoredProcedure);
            last = p.ReadStandardOutputs(pascalCase: true);
            if (!last.IsSuccess) return last;
        }
        return last;
    }

    /// <summary>물리삭제 대신 USE_YN='N' 처리 - 하위 메뉴 참조무결성 보존을 위한 표준 삭제 방식(work_type='D')</summary>
    public async Task<ProcResult> SetUseYnAsync(long menuId, bool useYn, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_menu_id", menuId);
        p.Add("p_use_yn", useYn ? "Y" : "N");
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_MENU_S_1", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
