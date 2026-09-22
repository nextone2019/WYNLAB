using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.SM;

/// <summary>
/// 사이드바 "마이 메뉴(My Menu)" - 사용자가 즐겨찾기한 메뉴 목록(TSMUSERFAVORITEMENU),
/// 각 건은 메뉴ID + 사용자가 붙인 폴더 이름(선택). GridLayoutRepository와 같은 이유로 범용
/// 데이터 통로(api/data/*) 대신 전용 컨트롤러를 쓴다 - 특정 화면 소유 데이터가 아니라
/// 로그인한 사용자가 Shell에서 다루는 개인 설정이기 때문이다.
/// </summary>
public interface IFavoriteMenuRepository
{
    Task<List<FavoriteMenuDto>> GetAsync(string userId);
    Task<ProcResult> AddAsync(string userId, long menuId);
    Task<ProcResult> RemoveAsync(string userId, long menuId);
    Task ReorderAsync(string userId, List<long> orderedMenuIds);
    Task<ProcResult> SetFolderAsync(string userId, long menuId, string? folder);
}

public class FavoriteMenuRepository : IFavoriteMenuRepository
{
    private readonly IDapperContext _context;

    public FavoriteMenuRepository(IDapperContext context) => _context = context;

    public async Task<List<FavoriteMenuDto>> GetAsync(string userId)
    {
        using var conn = _context.CreateConnection();
        var rows = await conn.QueryAsync(
            "USP_SM_FAVORITEMENU_Q",
            new { p_work_type = "Q", p_user_id = userId },
            commandType: CommandType.StoredProcedure);

        return rows.Select(r => new FavoriteMenuDto { MenuId = r.MENU_ID, Folder = r.FOLDER_NM }).ToList();
    }

    public async Task<ProcResult> AddAsync(string userId, long menuId)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_user_id", userId);
        p.Add("p_menu_id", menuId);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FAVORITEMENU_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<ProcResult> RemoveAsync(string userId, long menuId)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_user_id", userId);
        p.Add("p_menu_id", menuId);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FAVORITEMENU_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    /// <summary>즐겨찾기 전체를 새 순서로 다시 매긴다("상위로/하위로 이동") - 목록이 몇 건 안
    /// 되는(개인 즐겨찾기라 많아야 수십 건) 화면이라, 바뀐 두 행만 정교하게 스왑하는 대신
    /// 클라이언트가 넘겨준 새 순서 전체를 1부터 다시 매기는 단순한 방식으로 처리한다.</summary>
    public async Task ReorderAsync(string userId, List<long> orderedMenuIds)
    {
        using var conn = _context.CreateConnection();
        for (var i = 0; i < orderedMenuIds.Count; i++)
        {
            var p = new DynamicParameters();
            p.Add("p_work_type", "O");
            p.Add("p_user_id", userId);
            p.Add("p_menu_id", orderedMenuIds[i]);
            p.Add("p_sort_order", i + 1);
            p.AddStandardOutputs(pascalCase: true);

            await conn.ExecuteAsync("USP_SM_FAVORITEMENU_S", p, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task<ProcResult> SetFolderAsync(string userId, long menuId, string? folder)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "F");
        p.Add("p_user_id", userId);
        p.Add("p_menu_id", menuId);
        p.Add("p_folder_nm", folder);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FAVORITEMENU_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
