using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.SM;

public interface IShortcutRepository
{
    /// <summary>
    /// USP_SM_SHORTCUT_Q 호출 - 로그인 사용자의 유효 단축키(기본값+재정의 병합) 7건을 가져온다.
    /// 로그인 응답(LoginResponse.Shortcuts)에 얹어 내려주기 위한 용도라, UserRepository.GetSessionAsync와
    /// 마찬가지로 범용 데이터 통로(api/data/*)의 메뉴권한 검사 없이 직접 프로시저를 부른다 -
    /// 단축키는 특정 메뉴의 조회권한과 무관하게 항상 세션에 있어야 하는 값이기 때문이다.
    /// </summary>
    Task<List<ShortcutDto>> GetEffectiveShortcutsAsync(string userId);
}

public class ShortcutRepository : IShortcutRepository
{
    private readonly IDapperContext _context;

    public ShortcutRepository(IDapperContext context) => _context = context;

    public async Task<List<ShortcutDto>> GetEffectiveShortcutsAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var rows = await conn.QueryAsync(
            "USP_SM_SHORTCUT_Q",
            new { p_work_type = "Q", p_user_id = userId },
            commandType: CommandType.StoredProcedure);

        return rows.Select(r => new ShortcutDto
        {
            ActionCd = r.action_cd,
            ActionNm = r.action_nm,
            KeyCombo = r.key_combo,
            CustomYn = r.custom_yn == "Y",
            SortOrder = r.sort_order
        }).ToList();
    }
}
