using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 프로시저 빌더(frmProcBuilder, SYS_PROC_BUILDER) 전용 - ScreenBuilderController(AI Builder)와
/// 별개 화면이라 컨트롤러도 분리했다(00.DEV/MODULE_ARCHITECTURE.md의 "별도 도구, 공용 유틸리티만
/// 공유" 판단과 같은 결). 테이블 하나의 컬럼 구조만 읽어온다 - 아무것도 실행하지 않는다.
/// </summary>
[ApiController]
[Route("api/proc-builder")]
[Authorize]
public class ProcBuilderController : ControllerBase
{
    private readonly IProcBuilderRepository _repo;
    private readonly ILogger<ProcBuilderController> _logger;

    public ProcBuilderController(IProcBuilderRepository repo, ILogger<ProcBuilderController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet("describe-table")]
    [RequireMenuPermission("SYS", "frmProcBuilder", MenuAction.View)]
    public async Task<ActionResult<DescribeTableResultDto>> DescribeTable([FromQuery] string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            return BadRequest(new ApiResult { Success = false, Message = "테이블 이름을 입력해주세요." });

        var result = await _repo.DescribeTableAsync(tableName);
        if (result.Columns.Count == 0)
            return NotFound(new ApiResult { Success = false, Message = $"'{tableName}' 테이블을 찾을 수 없습니다." });

        return Ok(result);
    }

    /// <summary>frmProcBuilder가 미리보기로 조립한 SQL 텍스트를 그대로 실행해서 DB에 프로시저를
    /// 만든다/바꾼다(2026-09-14 - "생성 버튼을 누르면 실제로 반영돼야 한다"는 요청으로 추가).
    /// 임의 SQL 실행 엔드포인트가 되면 안 되므로, CREATE [OR ALTER] PROCEDURE로 시작하는지만
    /// 확인한다 - 그 외 문장(DROP/DELETE 등)이 앞에 오면 거부한다. 화면이 프로시저를 만드는
    /// 도구인 이상 이 정도 실행 권한은 View보다 강한 Insert로 게이트한다.</summary>
    [HttpPost("execute")]
    [RequireMenuPermission("SYS", "frmProcBuilder", MenuAction.Insert)]
    public async Task<ActionResult<ApiResult>> Execute([FromBody] ExecuteProcRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Sql))
            return BadRequest(new ApiResult { Success = false, Message = "실행할 SQL이 없습니다." });

        var trimmed = request.Sql.TrimStart();
        if (!trimmed.StartsWith("CREATE PROCEDURE", StringComparison.OrdinalIgnoreCase) &&
            !trimmed.StartsWith("CREATE OR ALTER PROCEDURE", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ApiResult { Success = false, Message = "CREATE OR ALTER PROCEDURE로 시작하는 텍스트만 실행할 수 있습니다." });

        // 이 엔드포인트는 DB 스키마(프로시저)를 직접 바꾸는 유일한 API라, 누가 언제 무엇을
        // 실행했는지는 반드시 로그 파일에 남겨야 한다(2026-09-15) - 성공/실패 둘 다 남긴다.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "(unknown)";

        try
        {
            await _repo.ExecuteSqlAsync(request.Sql);
            _logger.LogWarning("[ProcBuilder] {UserId}가 프로시저를 생성/변경했습니다.\n{Sql}", userId, request.Sql);
            return Ok(new ApiResult { Success = true, Message = "프로시저가 생성/변경되었습니다." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ProcBuilder] {UserId}의 프로시저 실행이 실패했습니다.\n{Sql}", userId, request.Sql);
            return Ok(new ApiResult { Success = false, Message = $"실행 중 오류가 발생했습니다.\n{ex.Message}" });
        }
    }
}
