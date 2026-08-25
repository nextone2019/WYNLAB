using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 권한부여관리 화면(TSMMENUAUTH) 전용 API - 사용자 1명 또는 그룹 1개를 대상으로
/// 메뉴별 조회/등록/수정/삭제/엑셀 권한을 직접 조회/저장한다.
/// 로그인시 자동 병합되는 MenusController와는 별개 (그쪽은 여러 대상을 합산한 "결과", 이쪽은
/// 특정 대상 1건에 실제로 걸려있는 "원본 설정").
/// 다른 사람의 권한 자체를 바꾸는 화면이라, [RequireMenuPermission("SM_AUTH", ...)]로 서버에서도
/// 반드시 검증한다 - 여기가 뚫리면 로그인한 사용자가 스스로에게 전체관리자 권한을 부여할 수 있다.
/// </summary>
[ApiController]
[Route("api/menu-auth")]
[Authorize]
public class MenuAuthController : ControllerBase
{
    private readonly IMenuAuthAssignRepository _repo;

    public MenuAuthController(IMenuAuthAssignRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM_AUTH", MenuAction.View)]
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
    [RequireMenuPermission("SM_AUTH", MenuAction.Update)]
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

        var result = await _repo.SaveAuthAsync(request.TargetType, request.TargetCd, items);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }
}
