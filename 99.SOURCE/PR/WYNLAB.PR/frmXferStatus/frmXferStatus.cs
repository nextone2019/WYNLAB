using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 외주이전현황 - Master-SubGrid 조회 화면(위 grd1 외주이전 목록, 아래 grd2 선택한 이전의 LOT별 출발/도착/차이 처리).
/// 조회전용이고 이전번호를 더블클릭하면 frmXfer, 작업지시번호를 더블클릭하면 frmWo가 그 건으로 열린다.
/// </summary>
public partial class frmXferStatus : BaseForm
{
    private DataTable _list = new();

    public frmXferStatus()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "외주이전현황";

        Controls.Add(BuildScreenHeader());

        dteSearchFrom.YyyyMmDd = DateTime.Today.AddMonths(-1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "외주이전 상세 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenLinkAsync, "화면 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        _list = await QueryAsync("USP_PR_XFERSTAT_Q", new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "L",
            ["p_fr_date"] = dteSearchFrom.YyyyMmDd,
            ["p_to_date"] = dteSearchTo.YyyyMmDd,
            ["p_stat_cd"] = cboSearchStat.EditValue?.ToString(),
            ["p_xfer_no"] = txtSearchXferNo.Text,
            ["p_wo_no"] = txtSearchWoNo.Text,
            ["p_lot_no"] = txtSearchLotNo.Text,
        });
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2를 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view)
        {
            grd2.DataSource = null;
            return;
        }

        var tables = await QueryMultiAsync("USP_PR_XFER_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_xfer_id"] = view.Row["xfer_id"]?.ToString(),
        });
        grd2.DataSource = tables.Count > 1 ? tables[1] : null;
    }

    private async Task OpenLinkAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        if (gvw1.FocusedColumn == col1XferNo)
        {
            var id = view.Row["xfer_id"]?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "PR" && m.ScreenClassNm == "frmXfer");
            var form = new frmXfer { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
            form.Show();
            await form.FocusRecordAsync(id);
        }
        else if (gvw1.FocusedColumn == col1WoNo)
        {
            var id = view.Row["wo_id"]?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "PR" && m.ScreenClassNm == "frmWo");
            var form = new frmWo { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
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
