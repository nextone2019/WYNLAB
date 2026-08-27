using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 메뉴관리 화면(TSMMENU)용 CRUD API. 로그인 필수(JWT).
/// 각 액션은 [RequireMenuPermission("SM_MENU", ...)]로 TSMMENUAUTH 기준 권한을 서버에서도 검증한다.
/// </summary>
[ApiController]
[Route("api/menus")]
[Authorize]
public class MenusController : ControllerBase
{
    private readonly IMenuManageRepository _repo;

    public MenusController(IMenuManageRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM_MENU", MenuAction.View)]
    public async Task<ActionResult<List<MenuListItemDto>>> GetAll()
    {
        var rows = await _repo.GetAllAsync();
        return Ok(rows.Select(MapToDto).ToList());
    }

    [HttpPost]
    [RequireMenuPermission("SM_MENU", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] MenuCreateRequest request)
    {
        if (await _repo.ExistsAsync(request.MenuCd))
            return Ok(new ApiResult { Success = false, Message = "이미 존재하는 메뉴코드입니다." });

        var result = await _repo.CreateAsync(request.MenuCd, request.MenuNm, request.UpperMenuCd, request.MenuLevel,
            request.MenuType, request.FormClassNm, request.IconNm, request.SortOrder);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    [HttpPut("{menuCd}")]
    [RequireMenuPermission("SM_MENU", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(string menuCd, [FromBody] MenuUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(menuCd))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 메뉴입니다." });

        var result = await _repo.UpdateAsync(menuCd, request.MenuNm, request.UpperMenuCd, request.MenuLevel,
            request.MenuType, request.FormClassNm, request.IconNm, request.SortOrder, request.UseYn);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    [HttpDelete("{menuCd}")]
    [RequireMenuPermission("SM_MENU", MenuAction.Delete)]
    public async Task<ActionResult<ApiResult>> Delete(string menuCd)
    {
        var result = await _repo.SetUseYnAsync(menuCd, useYn: false);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    private static MenuListItemDto MapToDto(MenuManageRow row) => new()
    {
        MenuCd = row.MenuCd,
        MenuNm = row.MenuNm,
        UpperMenuCd = row.UpperMenuCd,
        MenuLevel = row.MenuLevel,
        MenuType = row.MenuType,
        FormClassNm = row.FormClassNm,
        IconNm = row.IconNm,
        SortOrder = row.SortOrder,
        UseYn = row.UseYn == "Y"
    };
}
