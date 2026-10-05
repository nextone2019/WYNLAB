using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 실사차이분석 - 입력완료/확정된 실사의 라인별 차이 현황(조회전용 단일 그리드). 그룹 패널에 열 머리를 끌어다 놓으면 실사/창고/품목/조정사유별로
/// 묶어서 볼 수 있다. 실사번호를 더블클릭하면 frmStockCount(재고실사등록)가 그 건으로 열린다.
/// </summary>
public partial class frmStockCountDiff : BaseForm
{
    private DataTable _list = new();

    public frmStockCountDiff()
    {
        InitializeComponent();

        Text = "실사차이분석";
        Controls.Add(BuildScreenHeader());

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        // 기본 조회기간 = 이번 달 1일 ~ 오늘, 차이 있는 라인만
        var today = DateTime.Today;
        dteSearchFrom.YyyyMmDd = new DateTime(today.Year, today.Month, 1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = today.ToString("yyyyMMdd");
        chkDiffOnly.Checked = true;

        txtSearchWhNm.MapField("wh_id", txtSearchWhId);

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.AddColumnSummary(colCntNo, DevExpress.Data.SummaryItemType.Count, "{0:#,##0}건");
        gvw1.RowCellStyle += Gvw1_RowCellStyle;
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenStockCountAsync, "재고실사등록 열기");

        Load += async (s, e) => await QueryClick();
    }

    private static decimal Dec(object? value) => value == null || value == DBNull.Value ? 0 : decimal.TryParse(value.ToString(), out var d) ? d : 0;

    /// <summary>차이는 부호별 색(+ 파랑 / - 빨강), 변동이 있던 라인은 변동 칸을 주황으로.</summary>
    private void Gvw1_RowCellStyle(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    {
        if (e.Column == colDiffQty)
        {
            var d = gvw1.GetRowCellValue(e.RowHandle, colDiffQty);
            if (d == null || d == DBNull.Value) return;
            if (Dec(d) > 0) e.Appearance.ForeColor = System.Drawing.Color.FromArgb(0, 90, 200);
            else if (Dec(d) < 0) e.Appearance.ForeColor = System.Drawing.Color.FromArgb(200, 40, 40);
        }
        else if (e.Column == colMoveQty)
        {
            var m = gvw1.GetRowCellValue(e.RowHandle, colMoveQty);
            if (m != null && m != DBNull.Value && Dec(m) != 0) e.Appearance.ForeColor = System.Drawing.Color.FromArgb(220, 110, 0);
        }
    }

    public override async Task QueryClick()
    {
        _list = await QueryAsync("USP_MA_CNTDIFF_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
            ["p_cnt_no"] = txtSearchCntNo.Text,
            ["p_wh_id"] = string.IsNullOrWhiteSpace(txtSearchWhId.Text) ? null : txtSearchWhId.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_adj_reason"] = cboSearchReason.EditValue?.ToString(),
            ["p_stat_cd"] = chkCfmOnly.Checked ? "C" : null,
            ["p_diff_yn"] = chkDiffOnly.Checked ? "Y" : "N",
        });
        grd1.DataSource = _list;
    }

    private async Task OpenStockCountAsync()
    {
        if (gvw1.FocusedColumn != colCntNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var cntId = view.Row["cnt_id"]?.ToString();
        if (string.IsNullOrEmpty(cntId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmStockCount");
        var form = new frmStockCount { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(cntId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
