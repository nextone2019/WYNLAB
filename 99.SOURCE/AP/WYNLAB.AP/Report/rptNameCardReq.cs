namespace WYNLAB.AP.Report;

/// <summary>
/// 명함신청서 출력물 - frmNameCardReq에서 선택한 건 1개만 담은 단일행 DataTable을 DataSource로
/// 받는다(여러 행을 그대로 넣으면 Detail 밴드가 행 수만큼 반복 출력되므로, 호출부가 미리
/// 1행짜리 테이블로 걸러서 넘긴다 - frmNameCardReq.PreviewPrintAsync 참고). 컬럼명은
/// USP_HR_NAMECARD_Q(work_type='Q')가 돌려주는 그대로(req_no/reg_dt/dept_nm/emp_nm/app_no/
/// appr_stat_cd/name_kor/name_eng/dept_kor/dept_eng/job_grade/mobile/email/remark).
/// </summary>
public partial class rptNameCardReq : DevExpress.XtraReports.UI.XtraReport
{
    public rptNameCardReq()
    {
        InitializeComponent();
    }
}
