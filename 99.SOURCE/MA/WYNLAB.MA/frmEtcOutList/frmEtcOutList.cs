using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 기타출고현황 - Master-SubGrid 조회 화면(위 grd1 기타출고 목록, 아래 grd2 선택한 출고의 품목). 조회전용이고 출고번호를 더블클릭하면
/// frmEtcOut(기타출고등록)이 그 건으로 열린다. frmGrList와 같은 구조.
/// </summary>
public partial class frmEtcOutList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmEtcOutList()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        // 기본 조회기간 = 이번 달 1일 ~ 오늘
        var today = DateTime.Today;
        dteSearchFrom.YyyyMmDd = new DateTime(today.Year, today.Month, 1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = today.ToString("yyyyMMdd");

        Text = "기타출고현황";

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.AddColumnSummary(colTotalQty, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.####}");
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "기타출고 품목 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenEtcOutAsync, "기타출고등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;
        gvw2.AddColumnSummary(colDetOutQty, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.####}");

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        _list = await QueryAsync("USP_MA_ETCOUTLIST_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_out_no"] = txtSearchOutNo.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
            ["p_trans_type"] = cboSearchTransType.EditValue?.ToString(),
            ["p_stat_cd"] = cboSearchStatCd.EditValue?.ToString(),
        });
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2를 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_ETCOUTLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_out_id"] = view.Row["out_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    private async Task OpenEtcOutAsync()
    {
        if (gvw1.FocusedColumn != colOutNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var outId = view.Row["out_id"]?.ToString();
        if (string.IsNullOrEmpty(outId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmEtcOut");
        var form = new frmEtcOut { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(outId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
