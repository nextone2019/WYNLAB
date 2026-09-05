using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 개인별 그리드 레이아웃(컬럼 순서/숨김/폭) 저장/조회/초기화 - [Authorize]만 걸고
/// [RequireMenuPermission]은 안 건다. 이유: 이건 특정 화면의 업무 데이터(등록/수정/삭제
/// 권한이 걸려야 하는)가 아니라, 로그인한 사용자 개인의 "그 그리드를 어떻게 보고 싶은가"라서
/// 조회 권한만 있고 저장 권한은 없는 화면에서도 자기 컬럼 배치는 저장할 수 있어야 한다.
/// (범용 데이터 통로(api/data/*)로 만들었다가 이 이유로 못 쓰게 되어 이 전용 컨트롤러로
/// 분리했다 - GridLayoutRepository 클래스 설명 참고.)
/// </summary>
[ApiController]
[Route("api/grid-layout")]
[Authorize]
public class GridLayoutController : ControllerBase
{
    private readonly IGridLayoutRepository _repo;

    public GridLayoutController(IGridLayoutRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<List<GridLayoutItemDto>>> Get([FromQuery] long menuId) =>
        Ok(await _repo.GetAsync(CurrentUserId, menuId));

    [HttpPut]
    public async Task<ActionResult<ApiResult>> Save([FromBody] SaveGridLayoutRequest request)
    {
        var result = await _repo.SaveAsync(CurrentUserId, request.MenuId, request.GridKey, request.LayoutXml);
        if (!result.IsSuccess) return Ok(new ApiResult { Success = false, Message = result.FailMessage });
        return Ok(new ApiResult { Success = true });
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResult>> Delete([FromQuery] long menuId, [FromQuery] string gridKey)
    {
        var result = await _repo.DeleteAsync(CurrentUserId, menuId, gridKey);
        if (!result.IsSuccess) return Ok(new ApiResult { Success = false, Message = result.FailMessage });
        return Ok(new ApiResult { Success = true });
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}
