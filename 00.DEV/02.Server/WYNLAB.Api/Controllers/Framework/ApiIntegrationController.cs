using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Api.Services;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 외부 API 연동 정의/실행/이력 관리(frmApiIntegration, 2026-09-15). 정의(TSMAPIDEF)는 여기서
/// CRUD하고, 실제 fetch/parse/저장 로직은 연동별 IExternalApiIntegration 구현체(EximFxRateIntegration
/// 등)가 따로 갖는다 - 새 연동을 추가해도 이 컨트롤러는 손댈 필요가 없다(구현체 + DB row만 추가).
///
/// run-due는 사람이 아니라 서버의 Windows 작업 스케줄러가 주기적으로(예: 15분마다) 두드리는
/// 용도라 로그인 세션이 없다 - JWT 대신 고정 내부키(Internal:ServiceKey, web.config 환경변수 -
/// DB연결문자열과 같은 자리)로만 게이트한다(2026-09-15, IIS 앱풀 유휴재활용 문제로 내부 타이머
/// 대신 이 방식을 선택).
/// </summary>
[ApiController]
[Route("api/system/api-integrations")]
public class ApiIntegrationController : ControllerBase
{
    private readonly IApiIntegrationRepository _repo;
    private readonly IApiIntegrationRunner _runner;
    private readonly IConfiguration _config;

    public ApiIntegrationController(IApiIntegrationRepository repo, IApiIntegrationRunner runner, IConfiguration config)
    {
        _repo = repo;
        _runner = runner;
        _config = config;
    }

    [HttpGet]
    [Authorize]
    [RequireMenuPermission("SYS", "frmApiIntegration", MenuAction.View)]
    public async Task<ActionResult<List<ApiIntegrationDto>>> GetList() => Ok(await _repo.GetListAsync());

    [HttpGet("{apiCd}/logs")]
    [Authorize]
    [RequireMenuPermission("SYS", "frmApiIntegration", MenuAction.View)]
    public async Task<ActionResult<List<ApiIntegrationLogDto>>> GetLogs(string apiCd, [FromQuery] int limit = 50)
        => Ok(await _repo.GetLogsAsync(apiCd, limit));

    [HttpPut("{apiCd}")]
    [Authorize]
    [RequireMenuPermission("SYS", "frmApiIntegration", MenuAction.Update)]
    public async Task<ActionResult<ApiResult>> Save(string apiCd, [FromBody] SaveApiIntegrationRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "(unknown)";
        var pc = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        await _repo.SaveAsync(apiCd, request, userId, pc);
        return Ok(new ApiResult { Success = true, Message = "저장되었습니다." });
    }

    // 연동관리 화면(개발자 전용) 말고도 그 연동을 실제로 쓰는 업무화면(frmExcRate의 "환율정보수신"
    // 버튼 등)에서 바로 재실행할 수 있어야 한다 - 그 화면들은 SYS.frmApiIntegration 메뉴 권한이
    // 없는 일반 사용자도 쓰므로, 여기만 메뉴 게이트 없이 로그인 여부만 확인한다(2026-09-15).
    // 실행 대상은 이미 정의된 연동(api_cd)뿐이고 그 구현(URL/파싱/저장 로직)은 서버 코드로 고정돼
    // 있어 사용자가 임의 동작을 시킬 수 없다 - ProcBuilder.Execute(임의 SQL)와는 위험도가 다르다.
    [HttpPost("{apiCd}/run")]
    [Authorize]
    public async Task<ActionResult<ApiResult>> Run(string apiCd)
    {
        var result = await _runner.RunAsync(apiCd);
        return Ok(new ApiResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("run-due")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResult>> RunDue([FromHeader(Name = "X-Internal-Key")] string? internalKey)
    {
        var expected = _config["Internal:ServiceKey"];
        if (string.IsNullOrEmpty(expected) || internalKey != expected)
            return Unauthorized();

        var ran = await _runner.RunDueAsync();
        return Ok(new ApiResult { Success = true, Message = ran.Count == 0 ? "실행할 연동이 없습니다." : $"{string.Join(", ", ran)} 실행됨" });
    }
}
