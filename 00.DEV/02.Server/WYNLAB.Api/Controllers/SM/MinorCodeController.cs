using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.SM;

/// <summary>
/// 기초코드등록 화면(TSMMAJOR/TSMMINOR)용 API. 로그인 필수(JWT).
/// 각 액션은 [RequireMenuPermission("SM", "frmMinorCode", ...)]로 TSMMENUAUTH 기준 권한을 서버에서도 검증한다.
/// </summary>
[ApiController]
[Route("api/minor-codes")]
[Authorize]
public class MinorCodeController : ControllerBase
{
    private readonly IMinorCodeManageRepository _repo;

    public MinorCodeController(IMinorCodeManageRepository repo) => _repo = repo;

    /// <summary>대분류 리스트 + (selectedMajorCd로 넘어온) 소분류 리스트를 한 번에 조회</summary>
    [HttpGet]
    [RequireMenuPermission("SM", "frmMinorCode", MenuAction.View)]
    public async Task<ActionResult<MinorCodeQueryResponse>> Get(
        [FromQuery] string? majorCd, [FromQuery] string? majorNm, [FromQuery] string? selectedMajorCd)
    {
        var result = await _repo.GetAsync(majorCd, majorNm, selectedMajorCd);
        return Ok(result);
    }

    /// <summary>대분류 그리드(grd1) 포커스 행만 바뀌었을 때 쓰는 가벼운 조회 - 대분류
    /// 목록은 안 건드리고 이 대분류의 소분류 목록만 돌려준다.</summary>
    [HttpGet("{majorCd}/minors")]
    [RequireMenuPermission("SM", "frmMinorCode", MenuAction.View)]
    public async Task<ActionResult<List<MinorItemDto>>> GetMinors(string majorCd)
    {
        var result = await _repo.GetMinorsAsync(majorCd);
        return Ok(result);
    }

    /// <summary>범용 코드 LookUp(SSP_CBO_CODE_Q) - where 값(major_cd)만 넘기면 그 소분류 목록
    /// (코드+명)을 돌려준다. 화면별 권한이 아니라 로그인만 되어 있으면 누구나 쓸 수 있어야 하는
    /// 공용 조회라 [RequireMenuPermission]을 안 걸었다(LookUpEditWyn.Where가 화면 어디서든 호출함).
    /// 경로를 "/api/codes/lookup"으로 고정한다(컨트롤러 자체 라우트(api/minor-codes)를 안 따름) -
    /// 이 화면(frmMinorCode)만 쓰는 게 아니라 WYNLAB.BaseForm의 ControlDataSources.cs가 모든
    /// 화면의 LookUpEditWyn에서 공용으로 호출하는 주소라서, 이 화면 리네임과 무관하게 고정해야
    /// 한다 - 리네임하면서 라우트가 같이 옮겨져 전체 화면의 LookUp이 깨질 뻔했던 걸 여기서 막음.</summary>
    [HttpGet("/api/codes/lookup")]
    public async Task<ActionResult<List<CodeLookupItemDto>>> Lookup([FromQuery] string where)
    {
        var result = await _repo.GetLookupAsync(where);
        return Ok(result);
    }

    [HttpPost]
    [RequireMenuPermission("SM", "frmMinorCode", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Create([FromBody] MajorSaveRequest request)
    {
        var result = await _repo.SaveMajorAsync("N", request, CurrentUserId, ClientIp);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage, ErrorCode = result.ErrorCode });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    [HttpPut("{majorCd}")]
    [RequireMenuPermission("SM", "frmMinorCode", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Update(string majorCd, [FromBody] MajorSaveRequest request)
    {
        request.major_cd = majorCd;
        var result = await _repo.SaveMajorAsync("U", request, CurrentUserId, ClientIp);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage, ErrorCode = result.ErrorCode });

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>대분류 삭제. 실제 삭제 방식(소분류까지 같이 지울지, 소분류가 있으면 막을지)은
    /// USP_SM_MINORCODE_S의 'D' 분기가 정하며, 막는 경우 프로시저가 ReturnCode/ReturnMsg로
    /// 사유를 돌려주면 화면에 그대로 표시된다.</summary>
    [HttpDelete("{majorCd}")]
    [RequireMenuPermission("SM", "frmMinorCode", MenuAction.Delete)]
    public async Task<ActionResult<ApiResult>> Delete(string majorCd)
    {
        var result = await _repo.SaveMajorAsync("D", new MajorSaveRequest { major_cd = majorCd }, CurrentUserId, ClientIp);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage, ErrorCode = result.ErrorCode });

        return Ok(new ApiResult { Success = true });
    }

    /// <summary>이 대분류의 소분류 그리드를 화면에서 넘어온 목록으로 전체 치환</summary>
    [HttpPut("{majorCd}/minors")]
    [RequireMenuPermission("SM", "frmMinorCode", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> SaveMinors(string majorCd, [FromBody] SaveMinorsRequest request)
    {
        var result = await _repo.SaveMinorsAsync(majorCd, request.Items, CurrentUserId, ClientIp);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage, ErrorCode = result.ErrorCode });

        return Ok(new ApiResult { Success = true });
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    private string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();
}
