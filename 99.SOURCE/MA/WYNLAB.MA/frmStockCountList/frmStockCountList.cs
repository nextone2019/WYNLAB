using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 재고실사현황 - Master-SubGrid 조회 화면(위 grd1 실사 목록, 아래 grd2 선택한 실사의 라인). 조회전용이고 실사번호를 더블클릭하면
/// frmStockCount(재고실사등록)가 그 건으로 열린다. frmEtcOutList와 같은 구조.
/// </summary>
public partial class frmStockCountList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmStockCountList()
    {
        InitializeComponent();

        Text = "재고실사현황";
        Controls.Add(BuildScreenHeader());

        // 조회조건 사업장 - 화면 표준: 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        // 기본 조회기간 = 이번 달 1일 ~ 오늘
        var today = DateTime.Today;
        dteSearchFrom.YyyyMmDd = new DateTime(today.Year, today.Month, 1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "재고실사 라인 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenStockCountAsync, "재고실사등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        _list = await QueryAsync("USP_MA_CNTLIST_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_cnt_no"] = txtSearchCntNo.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
            ["p_cnt_type"] = cboSearchCntType.EditValue?.ToString(),
            ["p_stat_cd"] = cboSearchStatCd.EditValue?.ToString(),
        });
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2를 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_CNTLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_cnt_id"] = view.Row["cnt_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
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
