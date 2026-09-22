using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories;
using WYNLAB.Api.Services;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers;

/// <summary>
/// 화면별 Controller를 만들지 않고 저장프로시저를 실행해주는 범용 통로.
/// 설계 배경은 저장소 루트의 GENERIC_DATA_API.md 참고.
///
/// 이 컨트롤러가 하는 일은 "실행해도 되는 요청인지" 판단하는 것뿐이고, 실제 실행은
/// GenericDataRepository가 한다. 판단은 세 가지다:
///   ① 로그인 사용자가 그 메뉴에 대해 그 동작을 할 권한이 있는가 (TSMMENUAUTH)
///   ② 요청한 프로시저가 그 메뉴에 등록된 것인가 (TSMMENU.PROC_PREFIX)
///   ③ 넘길 파라미터가 "p_"로 시작하는가 (Repository에서 걸러냄)
///
/// 화면별 컨트롤러(MinorCodeController 등)는 그대로 둔다 - 화면을 하나씩 옮기는 동안 두 방식이
/// 공존해야 하기 때문이다.
/// </summary>
[ApiController]
[Route("api/data")]
[Authorize]
public class DataController : ControllerBase
{
    private readonly IGenericDataRepository _repo;
    private readonly IMenuPermissionService _permissions;

    public DataController(IGenericDataRepository repo, IMenuPermissionService permissions)
    {
        _repo = repo;
        _permissions = permissions;
    }

    /// <summary>조회 - 조회권한(ViewYn)만 확인한다.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<DataQueryResponse>> Query([FromBody] DataRequest request)
    {
        var denied = await ValidateAsync(request, MenuAction.View);
        if (denied != null) return denied;

        return Ok(await _repo.QueryAsync(request.ProcName, request.Params));
    }

    /// <summary>
    /// 저장/삭제 - p_work_type 값에 따라 필요한 권한이 달라진다. 화면이 무엇을 하려는지는
    /// 그 값에만 드러나므로, 여기서 읽어서 등록/수정/삭제 권한 중 맞는 것을 확인한다.
    /// </summary>
    [HttpPost("save")]
    public async Task<ActionResult<ApiResult>> Save([FromBody] DataRequest request)
    {
        var action = ResolveSaveAction(request);
        var denied = await ValidateAsync(request, action);
        if (denied != null) return denied;

        var result = await _repo.SaveAsync(request.ProcName, request.Params, CurrentUserId, ClientPc);

        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage, ErrorCode = result.ErrorCode });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    /// <summary>
    /// p_work_type -> 필요한 권한. 값이 없거나 모르는 값이면 가장 강한 권한(삭제)을 요구한다 -
    /// 판단이 안 서는 요청을 통과시키는 것보다 막는 쪽이 안전하다.
    /// 그리드 전체 치환 저장(USP_*_S_1)처럼 work_type을 안 쓰는 프로시저는 수정으로 취급한다.
    /// </summary>
    private static MenuAction ResolveSaveAction(DataRequest request)
    {
        if (!request.Params.TryGetValue("p_work_type", out var workType) || string.IsNullOrWhiteSpace(workType))
            return MenuAction.Update;

        return workType.Trim().ToUpperInvariant() switch
        {
            "N" => MenuAction.Insert,
            "U" => MenuAction.Update,
            "D" => MenuAction.Delete,
            _ => MenuAction.Delete
        };
    }

    /// <summary>
    /// 통과시켜도 되는 요청인지 확인하고, 안 되면 그 이유를 담은 응답을 돌려준다(null이면 통과).
    /// </summary>
    private async Task<ActionResult?> ValidateAsync(DataRequest request, MenuAction action)
    {
        if (request.MenuId <= 0 || string.IsNullOrWhiteSpace(request.ProcName))
            return BadRequest(new ApiResult { Success = false, Message = "메뉴ID와 프로시저명은 필수입니다." });

        if (string.IsNullOrEmpty(CurrentUserId)) return Unauthorized();

        // ① 메뉴 권한 - RequireMenuPermissionAttribute가 하던 검사와 같되, 메뉴ID가 요청에
        //    담겨 오므로 런타임에 확인한다.
        var permission = await _permissions.GetEffectivePermissionAsync(CurrentUserId, request.MenuId);
        var allowed = permission != null && action switch
        {
            MenuAction.View => permission.ViewYn,
            MenuAction.Insert => permission.InsertYn,
            MenuAction.Update => permission.UpdateYn,
            MenuAction.Delete => permission.DeleteYn,
            MenuAction.Excel => permission.ExcelYn,
            _ => false
        };

        if (!allowed)
            return Forbid(new ApiResult { Success = false, Message = "이 작업에 대한 권한이 없습니다." });

        // ② 프로시저 화이트리스트 - 그 메뉴에 등록된 접두사로 시작하는 프로시저만 실행할 수 있다.
        //    이게 없으면 권한 있는 메뉴 하나만 가지고 아무 프로시저나 부를 수 있게 된다.
        var prefix = await _repo.GetProcPrefixAsync(request.MenuId);
        if (string.IsNullOrWhiteSpace(prefix))
            return Forbid(new ApiResult { Success = false, Message = "이 메뉴는 범용 데이터 통로를 사용하도록 설정되어 있지 않습니다." });

        if (!request.ProcName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            return Forbid(new ApiResult { Success = false, Message = "이 메뉴에서 사용할 수 없는 프로시저입니다." });

        return null;
    }

    /// <summary>403은 본문을 못 담는 기본 Forbid()와 달리, 화면에 사유를 보여줄 수 있게 본문을 함께 돌려준다.</summary>
    private static ObjectResult Forbid(ApiResult body) =>
        new(body) { StatusCode = StatusCodes.Status403Forbidden };

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    private string? ClientPc => ClientPcInfo.Build(HttpContext);
}
