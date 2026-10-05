using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Controllers;
using WYNLAB.Api.Repositories;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 결재승인 엔진(USP_AP_APPR_Q/USP_AP_APPR_S, USP_AP_ROUTE_S) 호출 전담. 결재는 구매요청/명함신청서
/// 등 서로 다른 모듈의 여러 화면이 공통으로 호출해야 하는데, 범용 데이터 통로(api/data/*,
/// DataController)는 호출한 화면 "자기 메뉴"에 등록된 PROC_PREFIX로만 허용하므로
/// (GENERIC_DATA_API.md의 열려있는 결정 항목 그대로) 다른 모듈 화면은 USP_AP_APPR_*를 거기로
/// 부를 수 없다.
///
/// LookupsController/ComboLookupsController와 같은 전례를 따라 - 로그인만 확인하고 메뉴권한/
/// PROC_PREFIX 체크 없이 IGenericDataRepository를 직접 호출한다. 실행 가능한 프로시저 이름은
/// 화면이 아니라 이 컨트롤러가 고정해서 부르므로(요청에 프로시저명이 실려오지 않음) 화이트리스트
/// 문제 자체가 없다.
///
/// 기안자/승인자 등 "누가 처리하는지"는 전부 CurrentUserId(로그인 세션)로 프로시저가 직접
/// TSMUSER->TBAEMP를 타고 해석한다 - 클라이언트가 자기 신원을 속일 수 없다. 결재라인에 "누구를
/// 추가할지"(대상자 선택)는 라우팅 대상 선택일 뿐이라 클라이언트가 보낸 사번을 그대로 쓴다.
/// </summary>
[ApiController]
[Route("api/approvals")]
[Authorize]
public class ApprovalsController : ControllerBase
{
    private readonly IGenericDataRepository _repo;

    public ApprovalsController(IGenericDataRepository repo) => _repo = repo;

    /// <summary>결재상신 - TAPDOC 헤더 + 기안자 본인(sort=1, 즉시 승인완료) 라인만 만든다. 실제
    /// 결재라인/수신라인 구성은 이 응답의 GeneratedCode(app_id)로 AddPath를 반복 호출해서 채운다.</summary>
    [HttpPost("submit")]
    public async Task<ActionResult<ApiResult>> Submit([FromBody] ApprovalSubmitRequest request)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "N",
            ["p_doc_type"] = request.DocType,
            ["p_doc_id"] = request.DocId.ToString(),
            ["p_doc_no"] = request.DocNo,
            ["p_app_title"] = request.Title,
            ["p_app_text"] = request.Text,
            ["p_form_id"] = request.FormId,
        };
        return await SaveAsync("USP_AP_APPR_S", p);
    }

    [HttpPost("add-path")]
    public async Task<ActionResult<ApiResult>> AddPath([FromBody] ApprovalAddPathRequest request)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "ADDPATH",
            ["p_app_id"] = request.AppId.ToString(),
            ["p_target_emp_no"] = request.TargetEmpNo,
            ["p_path_type"] = request.PathType,
        };
        return await SaveAsync("USP_AP_APPR_S", p);
    }

    /// <summary>상신 마무리 - 결재/수신라인 추가(add-path)를 모두 끝낸 뒤 한 번 부른다. 결재자가 없어 이미 완료인 문서는 여기서 최종승인 후처리(문서 확정 등)를 실행한다.</summary>
    [HttpPost("finalize")]
    public Task<ActionResult<ApiResult>> Finalize([FromBody] ApprovalActionRequest request) => ActionAsync("FIN", request);

    [HttpPost("approve")]
    public Task<ActionResult<ApiResult>> Approve([FromBody] ApprovalActionRequest request) => ActionAsync("A", request);

    [HttpPost("reject")]
    public Task<ActionResult<ApiResult>> Reject([FromBody] ApprovalActionRequest request) => ActionAsync("R", request);

    [HttpPost("cancel-approve")]
    public Task<ActionResult<ApiResult>> CancelApprove([FromBody] ApprovalActionRequest request) => ActionAsync("C", request);

    [HttpPost("acknowledge")]
    public Task<ActionResult<ApiResult>> Acknowledge([FromBody] ApprovalActionRequest request) => ActionAsync("F", request);

    private async Task<ActionResult<ApiResult>> ActionAsync(string workType, ApprovalActionRequest request)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = workType,
            ["p_app_id"] = request.AppId.ToString(),
            ["p_opinion"] = request.Opinion,
        };
        return await SaveAsync("USP_AP_APPR_S", p);
    }

    /// <summary>결재이력(헤더 + 결재/수신라인 전부) - 업무화면이 자기 문서의 결재상태를 보여줄 때 사용.</summary>
    [HttpGet("history")]
    public async Task<ActionResult<ApprovalHistoryResponse>> History([FromQuery] string docType, [FromQuery] long docId)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_doc_type"] = docType,
            ["p_doc_id"] = docId.ToString(),
        };

        var data = await _repo.QueryAsync("USP_AP_APPR_Q", p);
        var response = new ApprovalHistoryResponse();

        if (data.Tables.Count > 0)
            response.Headers = data.Tables[0].Rows.Select(MapHeader).ToList();
        if (data.Tables.Count > 1)
            response.Paths = data.Tables[1].Rows.Select(MapPath).ToList();

        return Ok(response);
    }

    /// <summary>홈화면 대시보드 위젯용 - 로그인 사용자의 기안문서(Q6)/승인대상문서(Q1)를 한 번에
    /// 돌려준다. HomeForm은 메뉴가 아니라서 범용 통로(api/data/*)의 PROC_PREFIX 게이트를 못 타는데
    /// (그 메뉴가 없으니 등록된 PROC_PREFIX 자체가 없음), 이 컨트롤러는 애초에 로그인만 확인하고
    /// 실행할 프로시저를 컨트롤러가 고정해서 부르므로(클래스 설명 참고) 문제가 안 된다. 이 사번도
    /// 클라이언트가 보낸 값이 아니라 CurrentUserId로 서버가 직접 해석한다(남의 결재현황을 못 봄).</summary>
    [HttpGet("my-dashboard")]
    public async Task<ActionResult<ApprovalDashboardResponse>> MyDashboard()
    {
        var empNo = await ResolveMyEmpNoAsync();
        if (string.IsNullOrEmpty(empNo)) return Ok(new ApprovalDashboardResponse());

        var pending = await _repo.QueryAsync("USP_AP_APPR_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q1", ["p_emp_no"] = empNo });
        var drafted = await _repo.QueryAsync("USP_AP_APPR_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q6", ["p_emp_no"] = empNo });

        return Ok(new ApprovalDashboardResponse
        {
            Pending = (pending.Tables.FirstOrDefault()?.Rows ?? new()).Select(MapDashboardItem).ToList(),
            Drafted = (drafted.Tables.FirstOrDefault()?.Rows ?? new()).Select(MapDashboardItem).ToList(),
        });
    }

    /// <summary>홈 "결재 리스트" 한 칸(기안함/결재함 x 미결재/반려/결재완료)의 목록.
    /// box: "drafted"=기안함(Q6), "inbox"=결재함. stat: "P"=미결재, "R"=반려, "E"=결재완료.
    /// 결재함의 미결재는 "내 차례인 건"이라 Q1, 반려/완료는 "내가 결재·수신라인에 포함된 문서"라 Q7이다.
    /// 사번은 my-dashboard와 같은 이유로 서버가 직접 해석한다.</summary>
    [HttpGet("my-list")]
    public async Task<ActionResult<List<ApprovalDashboardItemDto>>> MyList(
        [FromQuery] string box, [FromQuery] string stat, [FromQuery] long? accId = null,
        [FromQuery] string? docType = null, [FromQuery] string? dateFrom = null, [FromQuery] string? dateTo = null,
        [FromQuery] string? reqEmpNo = null, [FromQuery] string? title = null)
    {
        var empNo = await ResolveMyEmpNoAsync();
        if (string.IsNullOrEmpty(empNo)) return Ok(new List<ApprovalDashboardItemDto>());

        var workType = box == "drafted" ? "Q6" : stat == "P" ? "Q1" : "Q7";
        var data = await _repo.QueryAsync("USP_AP_APPR_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = accId?.ToString(),
            ["p_work_type"] = workType,
            ["p_emp_no"] = empNo,
            ["p_doc_type"] = docType,
            ["p_date_from"] = dateFrom,
            ["p_date_to"] = dateTo,
            ["p_req_emp_no"] = reqEmpNo,
            ["p_title"] = title,
            ["p_stat_cd"] = stat,
        });
        return Ok((data.Tables.FirstOrDefault()?.Rows ?? new()).Select(MapDashboardItem).ToList());
    }

    /// <summary>기안서 작성 탭의 바로가기 타일 - 문서유형(AP0002) 중 문서등록 화면(rel_cd1)이 지정된 것만.</summary>
    [HttpGet("doc-types")]
    public async Task<ActionResult<List<ApprovalDocTypeDto>>> DocTypes()
    {
        var result = await _repo.QueryRawAsync(
            "SELECT minor_cd, minor_nm, rel_cd1, rel_cd2 FROM TSMMINOR WHERE major_cd = 'AP0002' AND use_yn = 'Y' AND ISNULL(rel_cd1, '') <> '' ORDER BY sort, minor_cd",
            new Dictionary<string, string?>());
        return Ok((result.Tables.FirstOrDefault()?.Rows ?? new()).Select(r => new ApprovalDocTypeDto
        {
            DocType = r["minor_cd"]?.ToString() ?? string.Empty,
            DocTypeNm = r["minor_nm"]?.ToString() ?? string.Empty,
            FormId = r["rel_cd1"]?.ToString() ?? string.Empty,
            Category = r["rel_cd2"]?.ToString() ?? string.Empty,
        }).ToList());
    }

    private async Task<string?> ResolveMyEmpNoAsync()
    {
        var result = await _repo.QueryRawAsync(
            "SELECT e.emp_no FROM TSMUSER u JOIN TBAEMP e ON e.EMP_ID = u.EMP_ID WHERE u.USER_ID = @p_user_id",
            new Dictionary<string, string?> { ["p_user_id"] = CurrentUserId });
        return result.Tables.FirstOrDefault()?.Rows.FirstOrDefault()?["emp_no"]?.ToString();
    }

    private static ApprovalDashboardItemDto MapDashboardItem(Dictionary<string, object?> row) => new()
    {
        AppId = Convert.ToInt64(row["app_id"]),
        AppNo = row["app_no"]?.ToString() ?? string.Empty,
        AppDate = row["app_date"]?.ToString() ?? string.Empty,
        AppTitle = row["app_title"]?.ToString() ?? string.Empty,
        DocType = row["doc_type"]?.ToString() ?? string.Empty,
        DocId = Convert.ToInt64(row["doc_id"]),
        DocNo = row["doc_no"]?.ToString() ?? string.Empty,
        FormId = row["form_id"]?.ToString(),
        ReqEmpNm = row.TryGetValue("req_emp_nm", out var v) ? v?.ToString() : null,
        StatCd = row.TryGetValue("stat_cd", out var sc) ? sc?.ToString() : null,
        CurApprEmpNm = row.TryGetValue("cur_appr_emp_nm", out var ca) ? ca?.ToString() : null,
        ReqDt = row.TryGetValue("req_dt", out var rd) ? rd as DateTime? : null,
        LastApprEmpNm = row.TryGetValue("last_appr_emp_nm", out var la) ? la?.ToString() : null,
        PathType = row.TryGetValue("path_type", out var pt) ? pt?.ToString() : null,
    };

    [HttpGet("dept-tree")]
    public async Task<ActionResult<List<DeptTreeItemDto>>> DeptTree()
    {
        var data = await _repo.QueryAsync("USP_AP_APPR_Q", new Dictionary<string, string?> { ["p_work_type"] = "Q2" });
        var rows = data.Tables.Count > 0 ? data.Tables[0].Rows : new List<Dictionary<string, object?>>();
        return Ok(rows.Select(r => new DeptTreeItemDto
        {
            DeptId = Convert.ToInt64(r["dept_id"]),
            DeptNm = r["dept_nm"]?.ToString() ?? string.Empty,
            ParDeptId = r["par_dept_id"] is { } v && v != DBNull.Value ? Convert.ToInt64(v) : null,
            DeptType = r["dept_type"]?.ToString(),
        }).ToList());
    }

    [HttpGet("employees")]
    public async Task<ActionResult<List<ApprovalEmpItemDto>>> Employees([FromQuery] long? deptId = null)
    {
        var p = new Dictionary<string, string?> { ["p_work_type"] = "Q3", ["p_dept_id"] = deptId?.ToString() };
        var data = await _repo.QueryAsync("USP_AP_APPR_Q", p);
        var rows = data.Tables.Count > 0 ? data.Tables[0].Rows : new List<Dictionary<string, object?>>();
        return Ok(rows.Select(r => new ApprovalEmpItemDto
        {
            EmpId = Convert.ToInt64(r["EMP_ID"]),
            EmpNo = r["emp_no"]?.ToString() ?? string.Empty,
            EmpNm = r["emp_nm"]?.ToString() ?? string.Empty,
            JobGrade = r["job_grade"]?.ToString(),
            DeptId = Convert.ToInt64(r["DEPT_ID"]),
        }).ToList());
    }

    [HttpGet("routes")]
    public async Task<ActionResult<List<ApprovalRouteItemDto>>> Routes()
    {
        var p = new Dictionary<string, string?> { ["p_work_type"] = "Q4", ["p_user_id"] = CurrentUserId };
        var data = await _repo.QueryAsync("USP_AP_APPR_Q", p);
        var rows = data.Tables.Count > 0 ? data.Tables[0].Rows : new List<Dictionary<string, object?>>();
        return Ok(rows.Select(r => new ApprovalRouteItemDto
        {
            RouteId = Convert.ToInt64(r["route_id"]),
            RouteNm = r["route_nm"]?.ToString() ?? string.Empty,
        }).ToList());
    }

    [HttpGet("routes/{routeId}")]
    public async Task<ActionResult<List<ApprovalRoutePathItemDto>>> RouteDetail(long routeId)
    {
        var p = new Dictionary<string, string?> { ["p_work_type"] = "Q5", ["p_route_id"] = routeId.ToString() };
        var data = await _repo.QueryAsync("USP_AP_APPR_Q", p);
        var rows = data.Tables.Count > 0 ? data.Tables[0].Rows : new List<Dictionary<string, object?>>();
        return Ok(rows.Select(r => new ApprovalRoutePathItemDto
        {
            RouteId = Convert.ToInt64(r["route_id"]),
            Sort = Convert.ToInt32(r["sort"]),
            EmpId = Convert.ToInt64(r["emp_id"]),
            EmpNo = r["emp_no"]?.ToString(),
            EmpNm = r["emp_nm"]?.ToString(),
            PathType = r["path_type"]?.ToString() ?? string.Empty,
        }).ToList());
    }

    [HttpPost("routes")]
    public async Task<ActionResult<ApiResult>> SaveRoute([FromBody] ApprovalRouteSaveRequest request)
    {
        var p = new Dictionary<string, string?> { ["p_work_type"] = "N", ["p_route_nm"] = request.RouteNm };
        return await SaveAsync("USP_AP_ROUTE_S", p);
    }

    [HttpPost("routes/add-detail")]
    public async Task<ActionResult<ApiResult>> AddRouteDetail([FromBody] ApprovalRouteAddDetailRequest request)
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "ADDDETAIL",
            ["p_route_id"] = request.RouteId.ToString(),
            ["p_target_emp_no"] = request.TargetEmpNo,
            ["p_path_type"] = request.PathType,
        };
        return await SaveAsync("USP_AP_ROUTE_S", p);
    }

    [HttpDelete("routes/{routeId}")]
    public async Task<ActionResult<ApiResult>> DeleteRoute(long routeId)
    {
        var p = new Dictionary<string, string?> { ["p_work_type"] = "D", ["p_route_id"] = routeId.ToString() };
        return await SaveAsync("USP_AP_ROUTE_S", p);
    }

    private async Task<ActionResult<ApiResult>> SaveAsync(string procName, Dictionary<string, string?> p)
    {
        var result = await _repo.SaveAsync(procName, p, CurrentUserId, ClientPc);
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage, ErrorCode = result.ErrorCode });

        return Ok(new ApiResult { Success = true, GeneratedCode = result.GeneratedCode });
    }

    private static ApprovalHeaderDto MapHeader(Dictionary<string, object?> row) => new()
    {
        AppId = Convert.ToInt64(row["app_id"]),
        AppNo = row["app_no"]?.ToString() ?? string.Empty,
        AppDate = row["app_date"]?.ToString() ?? string.Empty,
        AppTitle = row["app_title"]?.ToString() ?? string.Empty,
        AppText = row["app_text"]?.ToString() ?? string.Empty,
        DocType = row["doc_type"]?.ToString() ?? string.Empty,
        DocId = Convert.ToInt64(row["doc_id"]),
        DocNo = row["doc_no"]?.ToString() ?? string.Empty,
        EndYn = row["end_yn"]?.ToString() ?? string.Empty,
        EndDt = row["end_dt"] as DateTime?,
        RtnYn = row["rtn_yn"]?.ToString() ?? string.Empty,
        RtnDt = row["rtn_dt"] as DateTime?,
        StatCd = row["stat_cd"]?.ToString(),
        ReqEmpNm = row["emp_nm"]?.ToString(),
    };

    private static ApprovalPathDto MapPath(Dictionary<string, object?> row) => new()
    {
        AppId = Convert.ToInt64(row["app_id"]),
        Serl = Convert.ToInt32(row["serl"]),
        Sort = Convert.ToInt32(row["sort"]),
        PathType = row["path_type"]?.ToString() ?? string.Empty,
        EmpId = Convert.ToInt64(row["emp_id"]),
        EmpNo = row["emp_no"]?.ToString(),
        EmpNm = row["emp_nm"]?.ToString(),
        StatCd = row["stat_cd"]?.ToString() ?? string.Empty,
        AppDt = row["app_dt"] as DateTime?,
        Remark = row["remark"]?.ToString(),
    };

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    private string? ClientPc => ClientPcInfo.Build(HttpContext);
}
