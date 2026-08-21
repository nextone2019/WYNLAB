using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers;

/// <summary>
/// 권한부여관리 화면(TSMMENUAUTH) 전용 API - 사용자 1명 또는 그룹 1개를 대상으로
/// 메뉴별 조회/등록/수정/삭제/엑셀 권한을 직접 조회/저장한다.
/// 로그인시 자동 병합되는 MenusController와는 별개 (그쪽은 여러 대상을 합산한 "결과", 이쪽은
/// 특정 대상 1건에 실제로 걸려있는 "원본 설정").
/// </summary>
[ApiController]
[Route("api/menu-auth")]
[Authorize]
public class MenuAuthController : ControllerBase
{
    private readonly IMenuAuthAssignRepository _repo;

    public MenuAuthController(IMenuAuthAssignRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<List<MenuAuthItemDto>>> GetAuth([FromQuery] string targetType, [FromQuery] string targetCd)
    {
        if (targetType != "USER" && targetType != "GRP")
            return BadRequest("targetType은 USER 또는 GRP여야 합니다.");

        var rows = await _repo.GetAuthAsync(targetType, targetCd);
        return Ok(rows.Select(r => new MenuAuthItemDto
        {
            MenuCd = r.MenuCd,
            MenuNm = r.MenuNm,
            UpperMenuCd = r.UpperMenuCd,
            MenuType = r.MenuType,
            SortOrder = r.SortOrder,
            ViewYn = r.ViewYn == "Y",
            InsertYn = r.InsertYn == "Y",
            UpdateYn = r.UpdateYn == "Y",
            DeleteYn = r.DeleteYn == "Y",
            ExcelYn = r.ExcelYn == "Y"
        }).ToList());
    }

    [HttpPut]
    public async Task<ActionResult<ApiResult>> SaveAuth([FromBody] SaveMenuAuthRequest request)
    {
        if (request.TargetType != "USER" && request.TargetType != "GRP")
            return Ok(new ApiResult { Success = false, Message = "TargetType은 USER 또는 GRP여야 합니다." });

        var items = request.Items.Select(i => new MenuAuthAssignItem
        {
            MenuCd = i.MenuCd,
            ViewYn = i.ViewYn ? "Y" : "N",
            InsertYn = i.InsertYn ? "Y" : "N",
            UpdateYn = i.UpdateYn ? "Y" : "N",
            DeleteYn = i.DeleteYn ? "Y" : "N",
            ExcelYn = i.ExcelYn ? "Y" : "N"
        }).ToList();

        await _repo.SaveAuthAsync(request.TargetType, request.TargetCd, items);
        return Ok(new ApiResult { Success = true });
    }
}
