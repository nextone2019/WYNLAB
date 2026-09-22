using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.SM;

/// <summary>
/// TSMSITECONFIG(회사당 1건) 조회/저장 - frmSiteConfig(개발자 전용, Developer Tool 메뉴) 전용.
/// 이미지 3종(로그인배경/로고/파비콘)은 스칼라 값과 분리해서 여기서 직접 파라미터화된 SQL로
/// 다룬다 - 컬럼명을 프로시저 파라미터로 넘길 수 없고, 셋 다 단순 단일 컬럼 갱신이라
/// 프로시저 3개를 따로 만들 실익이 없다(USP_SM_SITECONFIG_S는 스칼라 값 전용).
/// </summary>
public interface ISiteConfigRepository
{
    Task<SiteConfigDto?> GetAsync();
    Task<ProcResult> SaveAsync(SiteConfigDto dto, string userId, string? clientPc);

    Task<(byte[] Bytes, string? Mime)?> GetImageAsync(string column, string mimeColumn);
    Task SaveImageAsync(string column, string mimeColumn, byte[] bytes, string mime);
}

public class SiteConfigRepository : ISiteConfigRepository
{
    private readonly IDapperContext _context;

    public SiteConfigRepository(IDapperContext context) => _context = context;

    public async Task<SiteConfigDto?> GetAsync()
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<SiteConfigDto>("USP_SM_SITECONFIG_Q",
            new { p_work_type = "Q" }, commandType: CommandType.StoredProcedure);
    }

    public async Task<ProcResult> SaveAsync(SiteConfigDto dto, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_company_nm", dto.CompanyNm);
        p.Add("p_smtp_host", dto.SmtpHost);
        p.Add("p_smtp_port", dto.SmtpPort);
        p.Add("p_smtp_username", dto.SmtpUsername);
        p.Add("p_smtp_from_address", dto.SmtpFromAddress);
        p.Add("p_smtp_from_display_nm", dto.SmtpFromDisplayNm);
        p.Add("p_file_block_extensions", dto.FileBlockExtensions);
        p.Add("p_file_max_size_mb", dto.FileMaxSizeMb);
        p.Add("p_pwd_expire_days", dto.PwdExpireDays);
        p.Add("p_pwd_lock_threshold", dto.PwdLockThreshold);
        p.Add("p_pwd_reset_code_valid_min", dto.PwdResetCodeValidMin);
        p.Add("p_pwd_min_length", dto.PwdMinLength);
        p.Add("p_pwd_require_upper_lower", dto.PwdRequireUpperLower ? "Y" : "N");
        p.Add("p_pwd_require_digit", dto.PwdRequireDigit ? "Y" : "N");
        p.Add("p_pwd_require_special", dto.PwdRequireSpecial ? "Y" : "N");
        p.Add("p_init_pwd_policy", dto.InitPwdPolicy);
        p.Add("p_force_change_on_first_login", dto.ForceChangeOnFirstLogin ? "Y" : "N");
        p.Add("p_idle_timeout_minutes", dto.IdleTimeoutMinutes);
        p.Add("p_required_field_back_color", dto.RequiredFieldBackColor);
        p.Add("p_grid_header_back_color", dto.GridHeaderBackColor);
        p.Add("p_grid_focused_row_back_color", dto.GridFocusedRowBackColor);
        p.Add("p_brand_color", dto.BrandColor);
        p.Add("p_tree_group_back_color", dto.TreeGroupBackColor);
        p.Add("p_divider_color", dto.DividerColor);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_SITECONFIG_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    /// <summary>column/mimeColumn은 호출측(컨트롤러)이 고정된 화이트리스트 문자열 상수로만
    /// 넘긴다(login_bg_image/logo_image/favicon_image) - 사용자 입력이 컬럼명으로 들어갈 일이
    /// 없어 SQL 텍스트 조합이어도 인젝션 여지가 없다.</summary>
    public async Task<(byte[] Bytes, string? Mime)?> GetImageAsync(string column, string mimeColumn)
    {
        using var conn = _context.CreateConnection();
        var row = await conn.QueryFirstOrDefaultAsync(
            $"SELECT {column} AS Bytes, {mimeColumn} AS Mime FROM TSMSITECONFIG WHERE config_id = 1");
        // dynamic(row)의 멤버 접근은 null 흐름분석이 하나의 || 조건 안에서는 잘 못 따라와서
        // (row == null || row.Bytes == null) 형태로 합쳐두면 CS8602가 오탐으로 뜬다 - 따로 나눈다.
        if (row == null) return null;
        if (row.Bytes == null) return null;
        return ((byte[])row.Bytes, (string?)row.Mime);
    }

    public async Task SaveImageAsync(string column, string mimeColumn, byte[] bytes, string mime)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(
            $"UPDATE TSMSITECONFIG SET {column} = @bytes, {mimeColumn} = @mime WHERE config_id = 1",
            new { bytes, mime });
    }
}
