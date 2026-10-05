using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

/// <summary>
/// 거래명세서현황 - 명세서 라인 단위 조회전용 목록. 명세서번호를 더블클릭하면 frmInvc가 그 건으로 열린다(다른 현황 화면과 같은 규칙).
/// 출고누계/매출누계와 출고/매출 상태(미/부분/완료)를 함께 보여준다.
/// </summary>
public partial class frmInvcList : BaseForm
{
    public frmInvcList()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "거래명세서현황";

        Controls.Add(BuildScreenHeader());

        dteSearchFrom.YyyyMmDd = DateTime.Today.AddMonths(-1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenLinkAsync, "화면 열기");

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        grd1.DataSource = await QueryAsync("USP_SA_INVC_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "L",
            ["p_fr_date"] = dteSearchFrom.YyyyMmDd,
            ["p_to_date"] = dteSearchTo.YyyyMmDd,
            ["p_stat_cd"] = cboSearchStat.EditValue?.ToString(),
            ["p_invc_no"] = txtSearchInvcNo.Text,
            ["p_so_no"] = txtSearchSoNo.Text,
            ["p_cust_nm"] = txtSearchCustNm.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
        });
    }

    private async Task OpenLinkAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        if (gvw1.FocusedColumn == col1InvcNo)
        {
            var id = view.Row["invc_id"]?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "SA" && m.ScreenClassNm == "frmInvc");
            var form = new frmInvc { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
            form.Show();
            await form.FocusRecordAsync(id);
        }
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
