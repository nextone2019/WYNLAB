namespace WYNLAB.Shared.Dtos;

/// <summary>
/// 결재상신 요청 - 어느 업무화면(구매요청/명함신청서 등)에서 호출하든 같은 모양이다. 결재엔진
/// (USP_AP_APPR_S)은 doc_type/doc_id로만 원본 문서를 식별하므로, 화면은 자기 문서의 PK와
/// 제목/내용만 넘기면 된다. 결재라인(누가 승인/수신하는지)은 상신으로 만들어진 app_id를 받은
/// 뒤 <see cref="ApprovalAddPathRequest"/>로 한 명씩 추가한다 - 기안자 본인(sort=1, 즉시
/// 승인완료)은 서버가 로그인 세션으로 자동으로 채운다.
/// </summary>
public class ApprovalSubmitRequest
{
    /// <summary>TSMMINOR(AP0002) 문서유형코드 - 예: "NAMECARD".</summary>
    public string DocType { get; set; } = string.Empty;
    public long DocId { get; set; }
    public string DocNo { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    /// <summary>"{TSMMENU.MODULE}.{TSMMENU.SCREEN_CLASS_NM}"(예: "GW.frmNameCardReq") - 결재함이
    /// 대기건을 더블클릭했을 때 이 값으로 원본 업무화면을 찾아 그 건에 포커스해서 연다
    /// (popApp.ShowAsync가 owner 폼의 타입에서 직접 뽑아 채운다).</summary>
    public string? FormId { get; set; }
}

/// <summary>결재라인('A', 순서 게이트 적용) 또는 수신라인('F', 게이트 없음)에 한 명 추가.</summary>
public class ApprovalAddPathRequest
{
    public long AppId { get; set; }
    public string TargetEmpNo { get; set; } = string.Empty;
    /// <summary>"A"=결재(순서대로 승인 필요), "F"=수신(참조, 순서 무관 단순 확인).</summary>
    public string PathType { get; set; } = "A";
}

/// <summary>승인/반려/승인취소/수신확인 공통 요청 - 대상 결재건(app_id)과 의견만 있으면 된다.
/// 실제로 처리할 행(로그인 사용자 소유의 대기중인 라인)은 서버가 @p_user_id로 직접 찾는다.</summary>
public class ApprovalActionRequest
{
    public long AppId { get; set; }
    public string? Opinion { get; set; }
}

public class DeptTreeItemDto
{
    public long DeptId { get; set; }
    public string DeptNm { get; set; } = string.Empty;
    public long? ParDeptId { get; set; }
    public string? DeptType { get; set; }
}

public class ApprovalEmpItemDto
{
    public long EmpId { get; set; }
    public string EmpNo { get; set; } = string.Empty;
    public string EmpNm { get; set; } = string.Empty;
    public string? JobGrade { get; set; }
    public long DeptId { get; set; }
}

public class ApprovalRouteItemDto
{
    public long RouteId { get; set; }
    public string RouteNm { get; set; } = string.Empty;
}

public class ApprovalRoutePathItemDto
{
    public long RouteId { get; set; }
    public int Sort { get; set; }
    public long EmpId { get; set; }
    public string? EmpNo { get; set; }
    public string? EmpNm { get; set; }
    public string PathType { get; set; } = string.Empty;
}

public class ApprovalRouteSaveRequest
{
    public string RouteNm { get; set; } = string.Empty;
}

public class ApprovalRouteAddDetailRequest
{
    public long RouteId { get; set; }
    public string TargetEmpNo { get; set; } = string.Empty;
    public string PathType { get; set; } = "A";
}

/// <summary>결재 헤더 1건 - 명함신청서 등 업무화면이 "내 문서의 결재상태"를 보여줄 때 쓴다.</summary>
public class ApprovalHeaderDto
{
    public long AppId { get; set; }
    public string AppNo { get; set; } = string.Empty;
    public string AppDate { get; set; } = string.Empty;
    public string AppTitle { get; set; } = string.Empty;
    public string AppText { get; set; } = string.Empty;
    public string DocType { get; set; } = string.Empty;
    public long DocId { get; set; }
    public string DocNo { get; set; } = string.Empty;
    public string EndYn { get; set; } = string.Empty;
    public DateTime? EndDt { get; set; }
    public string RtnYn { get; set; } = string.Empty;
    public DateTime? RtnDt { get; set; }
    public string? StatCd { get; set; }
    public string? ReqEmpNm { get; set; }
}

/// <summary>결재경로(TAPDOCPATH) 상세 1행 - 결재이력 그리드 바인딩용.</summary>
public class ApprovalPathDto
{
    public long AppId { get; set; }
    public int Serl { get; set; }
    public int Sort { get; set; }
    public string PathType { get; set; } = string.Empty;
    public long EmpId { get; set; }
    public string? EmpNo { get; set; }
    public string? EmpNm { get; set; }
    public string StatCd { get; set; } = string.Empty;
    public DateTime? AppDt { get; set; }
    public string? Remark { get; set; }
}

public class ApprovalHistoryResponse
{
    public List<ApprovalHeaderDto> Headers { get; set; } = new();
    public List<ApprovalPathDto> Paths { get; set; } = new();
}

/// <summary>홈화면 대시보드 위젯용 - 로그인 사용자의 기안문서/승인대상문서를 한 번에 담는다
/// (ApprovalsController.MyDashboard). ApprovalHeaderDto보다 가벼운 이유는 Q1/Q6이 app_text
/// 같은 무거운 본문 컬럼을 안 돌려주기 때문(목록에는 필요 없음).</summary>
public class ApprovalDashboardResponse
{
    /// <summary>승인대상문서 - 로그인 사용자가 지금 처리할 차례인 결재건(USP_AP_APPR_Q Q1).</summary>
    public List<ApprovalDashboardItemDto> Pending { get; set; } = new();
    /// <summary>기안문서 - 로그인 사용자가 상신한 결재건(USP_AP_APPR_Q Q6).</summary>
    public List<ApprovalDashboardItemDto> Drafted { get; set; } = new();
}

public class ApprovalDashboardItemDto
{
    public long AppId { get; set; }
    public string AppNo { get; set; } = string.Empty;
    public string AppDate { get; set; } = string.Empty;
    public string AppTitle { get; set; } = string.Empty;
    public string DocType { get; set; } = string.Empty;
    public long DocId { get; set; }
    public string? FormId { get; set; }
    /// <summary>Pending 목록에서만 값이 있다(기안자 이름) - Drafted 목록은 로그인 사용자 본인이라 불필요.</summary>
    public string? ReqEmpNm { get; set; }
    /// <summary>Drafted 목록에서만 값이 있다(TAPDOC.app_stat_cd: 0=결재상신/1=진행중/E=승인완료/R=반려) -
    /// Pending 목록은 정의상 전부 "내 차례 대기중"이라 불필요.</summary>
    public string? StatCd { get; set; }
    /// <summary>Drafted 목록에서만 값이 있다 - 이 문서를 지금 승인해야 할 차례인 사람 이름
    /// (승인완료/반려 등 더 이상 대기 라인이 없으면 NULL). Pending 목록은 로그인 사용자 본인이
    /// 항상 그 대상이라 불필요.</summary>
    public string? CurApprEmpNm { get; set; }
}
