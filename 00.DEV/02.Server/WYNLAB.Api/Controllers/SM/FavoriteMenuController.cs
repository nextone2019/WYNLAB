using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 사이드바 "마이 메뉴" 즐겨찾기 - GridLayoutController와 같은 이유로 [Authorize]만 걸고
/// [RequireMenuPermission]은 안 건다(특정 화면의 업무 데이터가 아니라 로그인 사용자 개인 설정).
/// </summary>
[ApiController]
[Route("api/favorite-menu")]
[Authorize]
public class FavoriteMenuController : ControllerBase
{
    private readonly IFavoriteMenuRepository _repo;

    public FavoriteMenuController(IFavoriteMenuRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<List<FavoriteMenuDto>>> Get() => Ok(await _repo.GetAsync(CurrentUserId));

    // ":long" 라우트 제약을 붙여서(2026-09-21 순서변경 추가하며) "/reorder"(문자열 리터럴)가
    // 이 {menuId} 자리에 잘못 매치되는 일을 원천적으로 막는다.
    [HttpPut("{menuId:long}")]
    public async Task<ActionResult<ApiResult>> Add(long menuId)
    {
        var result = await _repo.AddAsync(CurrentUserId, menuId);
        if (!result.IsSuccess) return Ok(new ApiResult { Success = false, Message = result.FailMessage });
        return Ok(new ApiResult { Success = true });
    }

    [HttpDelete("{menuId:long}")]
    public async Task<ActionResult<ApiResult>> Remove(long menuId)
    {
        var result = await _repo.RemoveAsync(CurrentUserId, menuId);
        if (!result.IsSuccess) return Ok(new ApiResult { Success = false, Message = result.FailMessage });
        return Ok(new ApiResult { Success = true });
    }

    /// <summary>"상위로/하위로 이동" - 클라이언트가 스왑까지 끝낸 전체 순서(menuId 배열)를
    /// 그대로 보내면, 서버는 그 순서대로 SORT_ORDER를 1부터 다시 매긴다.</summary>
    [HttpPut("reorder")]
    public async Task<ActionResult<ApiResult>> Reorder([FromBody] List<long> orderedMenuIds)
    {
        await _repo.ReorderAsync(CurrentUserId, orderedMenuIds);
        return Ok(new ApiResult { Success = true });
    }

    /// <summary>마이 메뉴 안에서 이 즐겨찾기를 어느 폴더로 묶을지 지정한다("핵심 업무",
    /// "자재 출납" 같은 사용자 임의 이름표, 2026-09-21) - 빈 문자열/null이면 폴더 해제
    /// (마이 메뉴 바로 아래 평평하게 표시).</summary>
    [HttpPut("{menuId:long}/folder")]
    public async Task<ActionResult<ApiResult>> SetFolder(long menuId, [FromBody] string? folder)
    {
        var result = await _repo.SetFolderAsync(CurrentUserId, menuId, folder);
        if (!result.IsSuccess) return Ok(new ApiResult { Success = false, Message = result.FailMessage });
        return Ok(new ApiResult { Success = true });
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}
