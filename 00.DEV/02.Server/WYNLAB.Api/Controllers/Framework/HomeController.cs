using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Repositories;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 홈화면(HomeForm) 대시보드 위젯 전담. ApprovalsController와 같은 이유(그 클래스 설명 참고) -
/// 홈화면은 실제 메뉴가 아니라서(ShellForm이 MenuId를 안 채워줌) 등록된 PROC_PREFIX 자체가 없어,
/// 범용 통로(api/data/*, DataController)를 못 탄다. 그래서 로그인만 확인하고 실행할 프로시저는
/// 이 컨트롤러가 고정해서 부른다(요청에 프로시저명이 실려오지 않으므로 화이트리스트 문제 없음).
/// </summary>
[ApiController]
[Route("api/home")]
[Authorize]
public class HomeController : ControllerBase
{
    private readonly IGenericDataRepository _repo;

    public HomeController(IGenericDataRepository repo) => _repo = repo;

    /// <summary>공지사항 - 게시중(use_yn='Y')인 것만, 중요공지 먼저 최신순으로 상위 N건
    /// (USP_SM_BOARD_Q). topN 기본값 5.</summary>
    [HttpGet("notices")]
    public async Task<ActionResult<List<HomeNoticeItemDto>>> Notices([FromQuery] int topN = 5)
    {
        var data = await _repo.QueryAsync("USP_SM_BOARD_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_use_yn"] = "Y",
            ["p_top_n"] = topN.ToString(),
        });

        var rows = data.Tables.FirstOrDefault()?.Rows ?? new List<Dictionary<string, object?>>();
        return Ok(rows.Select(r => new HomeNoticeItemDto
        {
            BoardId = Convert.ToInt64(r["board_id"]),
            Title = r["title"]?.ToString() ?? string.Empty,
            ImportantYn = r["important_yn"]?.ToString() ?? "N",
            RegDt = r["reg_dt"] as DateTime?,
        }).ToList());
    }

    /// <summary>오늘 일정 - 로그인 사용자 본인 것만(USP_SM_SCHEDULE_Q, 오늘 날짜가 시작~종료일
    /// 구간에 걸치는 일정). 사번은 클라이언트가 보낸 값을 믿지 않고 CurrentUserId로 서버가 직접
    /// 해석한다(ApprovalsController.MyDashboard와 같은 이유).</summary>
    [HttpGet("today-schedule")]
    public async Task<ActionResult<List<HomeScheduleItemDto>>> TodaySchedule()
    {
        var empNo = await ResolveMyEmpNoAsync();
        if (string.IsNullOrEmpty(empNo)) return Ok(new List<HomeScheduleItemDto>());

        var today = DateTime.Now.ToString("yyyyMMdd");
        var data = await _repo.QueryAsync("USP_SM_SCHEDULE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_emp_no"] = empNo,
            ["p_date_from"] = today,
            ["p_date_to"] = today,
        });

        var rows = data.Tables.FirstOrDefault()?.Rows ?? new List<Dictionary<string, object?>>();
        return Ok(rows.Select(r => new HomeScheduleItemDto
        {
            ScheduleId = Convert.ToInt64(r["schedule_id"]),
            Title = r["title"]?.ToString() ?? string.Empty,
            StartDt = r["start_dt"]?.ToString() ?? string.Empty,
            EndDt = r["end_dt"]?.ToString(),
            StartTm = r["start_tm"]?.ToString(),
            EndTm = r["end_tm"]?.ToString(),
            ColorCd = r["color_cd"]?.ToString(),
        }).ToList());
    }

    /// <summary>읽지 않은 쪽지 - 로그인 사용자가 받은 것 중 read_yn='N'인 것만(USP_SM_MESSAGE_Q,
    /// box_type='받음'으로 서버에서 한 번 더 걸러서 내가 보낸 쪽지는 절대 안 섞이게 한다).</summary>
    [HttpGet("unread-messages")]
    public async Task<ActionResult<List<HomeMessageItemDto>>> UnreadMessages()
    {
        var empNo = await ResolveMyEmpNoAsync();
        if (string.IsNullOrEmpty(empNo)) return Ok(new List<HomeMessageItemDto>());

        var data = await _repo.QueryAsync("USP_SM_MESSAGE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_emp_no"] = empNo,
        });

        var rows = data.Tables.FirstOrDefault()?.Rows ?? new List<Dictionary<string, object?>>();
        return Ok(rows
            .Where(r => r["box_type"]?.ToString() == "받음" && r["read_yn"]?.ToString() != "Y")
            .Select(r => new HomeMessageItemDto
            {
                MsgId = Convert.ToInt64(r["msg_id"]),
                Title = r["title"]?.ToString() ?? string.Empty,
                FromEmpNm = r["from_emp_nm"]?.ToString() ?? string.Empty,
                RegDt = r["reg_dt"] as DateTime?,
            }).ToList());
    }

    /// <summary>ApprovalsController.ResolveMyEmpNoAsync와 같은 방식(QueryRawAsync로 TSMUSER->TBAEMP
    /// 조회) - CurrentUserId로 서버가 직접 사번을 알아낸다(클라이언트가 남의 사번을 못 보냄).</summary>
    private async Task<string?> ResolveMyEmpNoAsync()
    {
        var result = await _repo.QueryRawAsync(
            "SELECT e.emp_no FROM TSMUSER u JOIN TBAEMP e ON e.EMP_ID = u.EMP_ID WHERE u.USER_ID = @p_user_id",
            new Dictionary<string, string?> { ["p_user_id"] = CurrentUserId });
        return result.Tables.FirstOrDefault()?.Rows.FirstOrDefault()?["emp_no"]?.ToString();
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}
