using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.SM;

/// <summary>
/// 개인별 그리드 레이아웃(TSMUSERGRIDLAYOUT) - USP_SM_GRIDLAYOUT_Q/_S로 위임한다.
/// GridLayoutController(api/grid-layout) 전용 - 범용 데이터 통로(DataController)를 안 쓰는
/// 이유는 그 통로가 화면마다 등록된 PROC_PREFIX로만 호출을 제한하는데, 이 기능은 특정 화면
/// 소유 데이터가 아니라 로그인한 사용자면 어느 화면에서든 써야 하기 때문이다.
/// </summary>
public interface IGridLayoutRepository
{
    Task<List<GridLayoutItemDto>> GetAsync(string userId, long menuId);
    Task<ProcResult> SaveAsync(string userId, long menuId, string gridKey, string layoutXml);
    Task<ProcResult> DeleteAsync(string userId, long menuId, string gridKey);
}

public class GridLayoutRepository : IGridLayoutRepository
{
    private readonly IDapperContext _context;

    public GridLayoutRepository(IDapperContext context) => _context = context;

    public async Task<List<GridLayoutItemDto>> GetAsync(string userId, long menuId)
    {
        using var conn = _context.CreateConnection();
        var rows = await conn.QueryAsync<GridLayoutItemDto>("USP_SM_GRIDLAYOUT_Q",
            new { p_work_type = "Q", p_user_id = userId, p_menu_id = menuId },
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public async Task<ProcResult> SaveAsync(string userId, long menuId, string gridKey, string layoutXml)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N"); // USP_SM_GRIDLAYOUT_S의 N은 있으면 갱신/없으면 신규(upsert)로 처리한다
        p.Add("p_user_id", userId);
        p.Add("p_menu_id", menuId);
        p.Add("p_grid_key", gridKey);
        p.Add("p_layout_xml", layoutXml);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_GRIDLAYOUT_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }

    public async Task<ProcResult> DeleteAsync(string userId, long menuId, string gridKey)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_user_id", userId);
        p.Add("p_menu_id", menuId);
        p.Add("p_grid_key", gridKey);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_GRIDLAYOUT_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
