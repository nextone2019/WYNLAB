using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.AP;

/// <summary>
/// 결재함 화면 - 결재승인 프로세스 1차 테스트용. 로그인 사용자가 결재자로 지정된 대기건
/// (USP_AP_APPR_Q work_type='Q1')을 목록으로 보여주고, 선택한 건을 승인/반려 처리한다
/// (USP_AP_APPR_S work_type='A'/'R').
///
/// 이 화면은 자기 메뉴(PROC_PREFIX=USP_AP_APPR_)로 결재엔진 프로시저를 직접 호출한다 - 범용
/// 데이터 통로(BaseForm.QueryAsync/SaveAsync)로 충분하다. 명함신청서(frmNameCardReq)처럼 "다른
/// 메뉴에서 결재엔진을 호출"하는 경우에만 ApprovalClient(전용 컨트롤러)가 필요하다.
/// </summary>
public partial class frmApprInbox : BaseForm
{
    private DataTable _list = new();

    public frmApprInbox()
    {
        InitializeComponent();

        Text = "결재함";

        gvw1.Role = GridRoleWyn.Query;

        var btnApprove = new ButtonWyn
        {
            Text = "승인",
            Location = new System.Drawing.Point(610, 60),
            Size = new System.Drawing.Size(90, 26),
        };
        btnApprove.Click += async (s, e) => await SafeExecuteAsync(() => ProcessAsync("A"), "승인");
        panBottom.Controls.Add(btnApprove);

        var btnReject = new ButtonWyn
        {
            Text = "반려",
            Location = new System.Drawing.Point(710, 60),
            Size = new System.Drawing.Size(90, 26),
        };
        btnReject.Click += async (s, e) => await SafeExecuteAsync(() => ProcessAsync("R"), "반려");
        panBottom.Controls.Add(btnReject);

        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenOriginalDocumentAsync, "원본문서열기");

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        _list = await QueryAsync("USP_AP_APPR_Q", new
        {
            p_work_type = "Q1",
            p_emp_no = Session.EmpNo
        });
        grd1.DataSource = _list;
        memoOpinion.Text = string.Empty;
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;

    /// <summary>승인/반려 공통 처리 - gvw1에서 고른 건의 app_id에 대해 USP_AP_APPR_S를 부른다.
    /// 승인/반려 모두 "그 결재건에서 내가 처리할 차례인 대기 라인"이 있어야만 성공한다(프로시저의
    /// @@ROWCOUNT=0 가드) - 그래서 화면에서 별도로 상태를 체크하지 않아도 안전하다.</summary>
    private async Task ProcessAsync(string workType)
    {
        if (gvw1.GetFocusedRow() is not DataRowView view)
        {
            AppMessageBox.Show("처리할 건을 먼저 선택해주세요.", "안내");
            return;
        }

        var appId = Convert.ToInt64(view.Row["app_id"]);
        var label = workType == "A" ? "승인" : "반려";

        var confirm = AppMessageBox.Show(
            $"선택하신 결재건을 {label} 처리하시겠습니까?\n\n[{view.Row["app_title"]}]",
            $"{label} 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        var result = await SaveAsync("USP_AP_APPR_S", new
        {
            p_work_type = workType,
            p_app_id = appId,
            p_opinion = memoOpinion.Text
        });

        if (!result.Success)
        {
            AppMessageBox.Show(result.Message ?? $"{label} 처리에 실패했습니다.", $"{label} 실패");
            return;
        }

        await QueryClick();
        Toast.Show($"{label} 처리되었습니다.");
    }

    /// <summary>결재 대기건을 더블클릭하면 실제로 뭘 승인하는지 확인할 수 있게 원본 업무화면을 그
    /// 건에 바로 포커스해서 연다 - 전자결재 화면은 결재 액션만 담당하고 문서 내용은 항상 원본
    /// 화면의 몫이다(공용 결재화면이 문서유형마다 다른 상세내용을 그릴 수는 없으므로).
    ///
    /// TAPDOC.form_id("{MODULE}.{화면클래스명}", 예: "AP.frmNameCardReq")를 ShellForm.OpenMenu와
    /// 같은 방식(ModuleLoader.EnsureLoaded로 그 모듈 dll을 불러온 뒤 Assembly.GetType)으로 풀어서
    /// 새 인스턴스를 열고, BaseForm.FocusRecordAsync(doc_id)로 그 건에 바로 포커스시킨다. 메뉴권한은
    /// SessionManager.Current.Menus에서 같은 MODULE+화면클래스명을 가진 메뉴를 찾아 그 MenuId를
    /// 그대로 넘겨준다(권한 없는 화면이면 그 메뉴가 목록에 없어 MenuId=0으로 열림 - BaseForm이
    /// 그 상태를 이미 안전하게 처리한다).</summary>
    private async Task OpenOriginalDocumentAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var formId = Convert.ToString(view.Row["form_id"]);
        if (string.IsNullOrWhiteSpace(formId))
        {
            AppMessageBox.Show("연결된 원본 화면 정보가 없습니다.", "안내");
            return;
        }

        var parts = formId.Split('.');
        if (parts.Length != 2)
        {
            AppMessageBox.Show($"원본 화면 정보 형식이 올바르지 않습니다: {formId}", "오류");
            return;
        }
        var module = parts[0];
        var className = parts[1];

        var assembly = ModuleLoader.EnsureLoaded($"WYNLAB.{module}");
        var formType = assembly?.GetType($"WYNLAB.{module}.{className}");
        if (formType == null || Activator.CreateInstance(formType) is not BaseForm form)
        {
            AppMessageBox.Show($"화면을 찾을 수 없습니다: {formId}", "오류");
            return;
        }

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == module && m.ScreenClassNm == className);
        form.MenuId = menu?.MenuId ?? 0;
        form.MdiParent = MdiParent;
        form.Show();

        var docId = Convert.ToString(view.Row["doc_id"]) ?? string.Empty;
        await form.FocusRecordAsync(docId);
    }
}
