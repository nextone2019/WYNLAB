using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 작업지시현황 - Master-SubGrid 조회 화면(위 grd1 작업지시 목록, 가운데 grd2 선택한 작업지시의 공정별 외주 진행, 아래 grd3 그 작업지시의 LOT와
/// 현재 재고 위치). 조회전용이고 작업지시번호를 더블클릭하면 frmWo가 그 건으로 열린다. 진행 공정/양품 누계로 "지금 어느 외주처에서 어디까지
/// 왔는지", LOT 현황으로 "이 LOT가 지금 어느 창고(외주처/이동중)에 있는지"를 한눈에 본다.
/// </summary>
public partial class frmWoStatus : BaseForm
{
    private DataTable _list = new();

    public frmWoStatus()
    {
        InitializeComponent();

        Text = "작업지시현황";

        Controls.Add(BuildScreenHeader());

        dteSearchFrom.YyyyMmDd = DateTime.Today.AddMonths(-1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "작업지시 상세 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenWoAsync, "작업지시 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;
        gvw3.Role = GridRoleWyn.Query;
        gvw3.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "L",
            ["p_fr_date"] = dteSearchFrom.YyyyMmDd,
            ["p_to_date"] = dteSearchTo.YyyyMmDd,
            ["p_stat_cd"] = cboSearchStat.EditValue?.ToString(),
            ["p_wo_no"] = txtSearchWoNo.Text,
            ["p_lot_no"] = txtSearchLotNo.Text,
        };
        _list = await QueryAsync("USP_PR_WO_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
        grd3.DataSource = null;
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2/grd3을 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view)
        {
            grd2.DataSource = null;
            grd3.DataSource = null;
            return;
        }

        var tables = await QueryMultiAsync("USP_PR_WO_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_wo_id"] = view.Row["wo_id"]?.ToString(),
        });
        grd2.DataSource = tables.Count > 1 ? tables[1] : null;
        grd3.DataSource = tables.Count > 2 ? tables[2] : null;
    }

    private async Task OpenWoAsync()
    {
        if (gvw1.FocusedColumn != colWoNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var woId = view.Row["wo_id"]?.ToString();
        if (string.IsNullOrEmpty(woId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "PR" && m.ScreenClassNm == "frmWo");
        var form = new frmWo { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(woId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
