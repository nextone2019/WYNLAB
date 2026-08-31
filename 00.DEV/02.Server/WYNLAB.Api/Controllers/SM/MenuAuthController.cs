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
/// 다른 사람의 권한 자체를 바꾸는 화면이라, [RequireMenuPermission("SM_USER", ...)]로 서버에서도
/// 반드시 검증한다 - 여기가 뚫리면 로그인한 사용자가 스스로에게 전체관리자 권한을 부여할 수 있다.
/// 메뉴코드가 SM_USER인 이유: 이 API는 frmUserManage의 권한부여 탭이 쓰는데, 전용 메뉴였던
/// SM_AUTH는 SM 모듈 정리 때(013_SM_Retire_Redundant_Menus.sql) TSMMENU에서 삭제됐다 -
/// 그때 이 컨트롤러의 권한체크 참조를 SM_USER로 같이 옮겼어야 했는데 빠뜨려서, 이후 admin을
/// 포함한 모든 사용자가 이 API에서 항상 403을 받는 상태로 남아 있었다(2026-08-27 발견/수정).
/// </summary>
[ApiController]
[Route("api/menu-auth")]
[Authorize]
public class MenuAuthController : ControllerBase
{
    private readonly IMenuAuthAssignRepository _repo;

    public MenuAuthController(IMenuAuthAssignRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM_USER", MenuAction.View)]
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
            PrintYn = r.PrintYn == "Y",
            ExcelYn = r.ExcelYn == "Y",
            Auth01 = r.Auth01 == "Y", Auth02 = r.Auth02 == "Y", Auth03 = r.Auth03 == "Y", Auth04 = r.Auth04 == "Y", Auth05 = r.Auth05 == "Y",
            Auth06 = r.Auth06 == "Y", Auth07 = r.Auth07 == "Y", Auth08 = r.Auth08 == "Y", Auth09 = r.Auth09 == "Y", Auth10 = r.Auth10 == "Y",
            AuthNm = new[]
            {
                r.Auth01Nm, r.Auth02Nm, r.Auth03Nm, r.Auth04Nm, r.Auth05Nm,
                r.Auth06Nm, r.Auth07Nm, r.Auth08Nm, r.Auth09Nm, r.Auth10Nm
            }
        }).ToList());
    }

    [HttpPut]
    [RequireMenuPermission("SM_USER", MenuAction.Update)]
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
            PrintYn = i.PrintYn ? "Y" : "N",
            ExcelYn = i.ExcelYn ? "Y" : "N",
            Auth01 = i.Auth01 ? "Y" : "N", Auth02 = i.Auth02 ? "Y" : "N", Auth03 = i.Auth03 ? "Y" : "N", Auth04 = i.Auth04 ? "Y" : "N", Auth05 = i.Auth05 ? "Y" : "N",
            Auth06 = i.Auth06 ? "Y" : "N", Auth07 = i.Auth07 ? "Y" : "N", Auth08 = i.Auth08 ? "Y" : "N", Auth09 = i.Auth09 ? "Y" : "N", Auth10 = i.Auth10 ? "Y" : "N"
        }).ToList();

        var result = await _repo.SaveAuthAsync(request.TargetType, request.TargetCd, items);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }
}
