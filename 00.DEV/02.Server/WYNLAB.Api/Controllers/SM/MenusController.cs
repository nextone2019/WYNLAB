using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Controllers;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 메뉴관리 화면(TSMMENU)용 CRUD API. 로그인 필수(JWT).
/// 각 액션은 [RequireMenuPermission("SM", "MENU.frmMenu", ...)]로 TSMMENUAUTH 기준 권한을 서버에서도 검증한다.
/// </summary>
[ApiController]
[Route("api/menus")]
[Authorize]
public class MenusController : ControllerBase
{
    private readonly IMenuManageRepository _repo;

    public MenusController(IMenuManageRepository repo) => _repo = repo;

    [HttpGet]
    [RequireMenuPermission("SM", "MENU.frmMenu", MenuAction.View)]
    public async Task<ActionResult<List<MenuListItemDto>>> GetAll()
    {
        var rows = await _repo.GetAllAsync();
        return Ok(rows.Select(MapToDto).ToList());
    }

    [HttpPost]
    [RequireMenuPermission("SM", "MENU.frmMenu", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] MenuCreateRequest request)
    {
        var result = await _repo.CreateAsync(request.MenuNm, request.UpperMenuId, request.MenuLevel,
            request.MenuType, request.Module, request.ScreenClassNm, request.IconNm, request.ProcPrefix, request.SortOrder, request.AuthNm,
            CurrentUserId, ClientPc);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    [HttpPut("{menuId}")]
    [RequireMenuPermission("SM", "MENU.frmMenu", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(long menuId, [FromBody] MenuUpdateRequest request)
    {
        if (!await _repo.ExistsAsync(menuId))
            return Ok(new ApiResult { Success = false, Message = "존재하지 않는 메뉴입니다." });

        var result = await _repo.UpdateAsync(menuId, request.MenuNm, request.UpperMenuId, request.MenuLevel,
            request.MenuType, request.Module, request.ScreenClassNm, request.IconNm, request.ProcPrefix, request.SortOrder, request.UseYn, request.AuthNm,
            CurrentUserId, ClientPc);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    [HttpDelete("{menuId}")]
    [RequireMenuPermission("SM", "MENU.frmMenu", MenuAction.Delete)]
    public async Task<ActionResult<ApiResult>> Delete(long menuId)
    {
        var result = await _repo.SetUseYnAsync(menuId, useYn: false, CurrentUserId, ClientPc);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        return Ok(new ApiResult { Success = true });
    }

    private static MenuListItemDto MapToDto(MenuManageRow row) => new()
    {
        MenuId = row.MenuId,
        MenuNm = row.MenuNm,
        UpperMenuId = row.UpperMenuId,
        MenuLevel = row.MenuLevel,
        MenuType = row.MenuType,
        Module = row.Module,
        ScreenClassNm = row.ScreenClassNm,
        IconNm = row.IconNm,
        ProcPrefix = row.ProcPrefix,
        SortOrder = row.SortOrder,
        UseYn = row.UseYn == "Y",
        AuthNm = new[]
        {
            row.Auth01Nm, row.Auth02Nm, row.Auth03Nm, row.Auth04Nm, row.Auth05Nm,
            row.Auth06Nm, row.Auth07Nm, row.Auth08Nm, row.Auth09Nm, row.Auth10Nm
        }
    };

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    private string? ClientPc => ClientPcInfo.Build(HttpContext);
}
