using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// AI Builder(화면개발 자동화, SYS_AI_BUILDER) 전용 - 조회/저장 프로시저 이름만 주면 실제로
/// 실행하지 않고 파라미터/결과셋 구조를 읽어온다. PopupAdminController.DescribeProc과 같은
/// 발상이지만 대상이 USP_*(업무 화면 프로시저)라 워크타입(Q/Q1 등) 분기까지 다룬다.
/// </summary>
[ApiController]
[Route("api/screen-builder")]
[Authorize]
public class ScreenBuilderController : ControllerBase
{
    private readonly IScreenBuilderRepository _repo;

    public ScreenBuilderController(IScreenBuilderRepository repo) => _repo = repo;

    public class DescribeProcRequest
    {
        public string ProcName { get; set; } = string.Empty;

        /// <summary>결과셋을 읽을 분기(Q/Q1 등) - 비워두면 파라미터만 읽고 결과셋은 스킵한다
        /// (저장프로시저처럼 SELECT가 없는 경우용).</summary>
        public string? WorkType { get; set; }
    }

    [HttpPost("describe-proc")]
    [RequireMenuPermission("SYS", "frmAIBuilder", MenuAction.View)]
    public async Task<ActionResult<DescribeProcResultDto>> DescribeProc([FromBody] DescribeProcRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProcName) || !request.ProcName.StartsWith("USP_", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "USP_로 시작하는 프로시저만 지정할 수 있습니다." });

        var result = await _repo.DescribeUspProcAsync(request.ProcName, request.WorkType);
        return Ok(result);
    }

    public class DescribeProcMultiRequest
    {
        public string ProcName { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
    }

    /// <summary>describe-proc과 달리 프로시저가 반환하는 레코드셋을 전부(2개 이상도) 읽는다 -
    /// 정적분석이 아니라 트랜잭션 안에서 실제로 실행 후 무조건 롤백한다(ScreenBuilderRepository
    /// 주석 참고). AI Builder에서 조회프로시저 하나가 레코드셋을 여러 개 반환할 때, 그 각각을
    /// 화면의 어느 그리드/폼에 바인딩할지 고르는 "결과셋 미리보기"용.</summary>
    [HttpPost("describe-proc-multi")]
    [RequireMenuPermission("SYS", "frmAIBuilder", MenuAction.View)]
    public async Task<ActionResult<DescribeProcMultiResultDto>> DescribeProcMulti([FromBody] DescribeProcMultiRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProcName) || !request.ProcName.StartsWith("USP_", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "USP_로 시작하는 프로시저만 지정할 수 있습니다." });
        if (string.IsNullOrWhiteSpace(request.WorkType))
            return BadRequest(new ApiResult { Success = false, Message = "WorkType이 필요합니다." });

        var result = await _repo.DescribeUspProcMultiAsync(request.ProcName, request.WorkType);
        return Ok(result);
    }
}
