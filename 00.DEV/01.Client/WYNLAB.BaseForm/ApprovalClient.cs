using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 결재승인 엔진(api/approvals/*) 호출 전담 - ApiClient와 같은 자리에, 결재를 연동하려는 모든
/// 업무화면(명함신청서 등)이 이 클래스 하나로 상신/처리/이력조회/부서트리/결재경로를 부른다.
///
/// api/data/query·save(범용 데이터 통로)가 아니라 별도 엔드포인트인 이유: 결재 프로시저는 화면
/// 자신의 메뉴(PROC_PREFIX)가 아니라 항상 다른 메뉴(구매요청/명함신청서 등)에서 호출돼야 하는데,
/// DataController는 호출한 화면 자신의 메뉴에 등록된 PROC_PREFIX로만 허용한다 - 결재는 Popup/LookUp
/// 조회와 같은 성격의 "로그인만 하면 어느 화면에서든 호출 가능해야 하는 공용 기능"이라 별도 통로를
/// 쓴다(ApprovalsController 주석 참고).
/// </summary>
public static class ApprovalClient
{
    /// <summary>결재상신 - TAPDOC 헤더 + 기안자 본인(sort=1, 즉시 승인완료) 라인만 만든다. 결재라인/
    /// 수신라인 구성은 반환된 GeneratedCode(app_id)로 AddPathAsync를 반복 호출해서 채운다.</summary>
    public static Task<ApiResult?> SubmitAsync(ApprovalSubmitRequest request) =>
        ApiClient.PostAsync<ApprovalSubmitRequest, ApiResult>("api/approvals/submit", request);

    public static Task<ApiResult?> AddPathAsync(ApprovalAddPathRequest request) =>
        ApiClient.PostAsync<ApprovalAddPathRequest, ApiResult>("api/approvals/add-path", request);

    public static Task<ApiResult?> ApproveAsync(long appId, string? opinion) =>
        ApiClient.PostAsync<ApprovalActionRequest, ApiResult>("api/approvals/approve", new ApprovalActionRequest { AppId = appId, Opinion = opinion });

    public static Task<ApiResult?> RejectAsync(long appId, string? opinion) =>
        ApiClient.PostAsync<ApprovalActionRequest, ApiResult>("api/approvals/reject", new ApprovalActionRequest { AppId = appId, Opinion = opinion });

    public static Task<ApiResult?> CancelApproveAsync(long appId) =>
        ApiClient.PostAsync<ApprovalActionRequest, ApiResult>("api/approvals/cancel-approve", new ApprovalActionRequest { AppId = appId });

    public static Task<ApiResult?> AcknowledgeAsync(long appId, string? opinion) =>
        ApiClient.PostAsync<ApprovalActionRequest, ApiResult>("api/approvals/acknowledge", new ApprovalActionRequest { AppId = appId, Opinion = opinion });

    /// <summary>특정 문서 1건의 결재이력(헤더 + 결재/수신라인 전부) - 업무화면/전자결재 화면이
    /// 결재상태를 보여줄 때 사용.</summary>
    public static Task<ApprovalHistoryResponse?> GetHistoryAsync(string docType, long docId) =>
        ApiClient.GetAsync<ApprovalHistoryResponse>($"api/approvals/history?docType={Uri.EscapeDataString(docType)}&docId={docId}");

    public static Task<List<DeptTreeItemDto>?> GetDeptTreeAsync() =>
        ApiClient.GetAsync<List<DeptTreeItemDto>>("api/approvals/dept-tree");

    /// <summary>deptId를 생략하면 전체 사원(조직도 트리용) - 지정하면 그 부서 소속만.</summary>
    public static Task<List<ApprovalEmpItemDto>?> GetEmployeesAsync(long? deptId = null) =>
        ApiClient.GetAsync<List<ApprovalEmpItemDto>>(deptId is { } id ? $"api/approvals/employees?deptId={id}" : "api/approvals/employees");

    public static Task<List<ApprovalRouteItemDto>?> GetRoutesAsync() =>
        ApiClient.GetAsync<List<ApprovalRouteItemDto>>("api/approvals/routes");

    public static Task<List<ApprovalRoutePathItemDto>?> GetRouteDetailAsync(long routeId) =>
        ApiClient.GetAsync<List<ApprovalRoutePathItemDto>>($"api/approvals/routes/{routeId}");

    public static Task<ApiResult?> SaveRouteAsync(string routeNm) =>
        ApiClient.PostAsync<ApprovalRouteSaveRequest, ApiResult>("api/approvals/routes", new ApprovalRouteSaveRequest { RouteNm = routeNm });

    public static Task<ApiResult?> AddRouteDetailAsync(long routeId, string targetEmpNo, string pathType) =>
        ApiClient.PostAsync<ApprovalRouteAddDetailRequest, ApiResult>("api/approvals/routes/add-detail",
            new ApprovalRouteAddDetailRequest { RouteId = routeId, TargetEmpNo = targetEmpNo, PathType = pathType });

    public static Task<ApiResult?> DeleteRouteAsync(long routeId) =>
        ApiClient.DeleteAsync<ApiResult>($"api/approvals/routes/{routeId}");
}
