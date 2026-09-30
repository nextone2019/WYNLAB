using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 공정실적현황 - Master-SubGrid 조회 화면(위 grd1 공정실적 목록, 가운데 grd2 선택한 실적의 산출 LOT, 아래 grd3 그 실적의 웨이퍼별 판정).
/// 조회전용이고 실적번호를 더블클릭하면 frmRslt, 작업지시번호를 더블클릭하면 frmWo가 그 건으로 열린다.
/// </summary>
public partial class frmRsltStatus : BaseForm
{
    private DataTable _list = new();

    public frmRsltStatus()
    {
        InitializeComponent();

        Text = "공정실적현황";

        Controls.Add(BuildScreenHeader());

        dteSearchFrom.YyyyMmDd = DateTime.Today.AddMonths(-1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "공정실적 상세 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenLinkAsync, "화면 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;
        gvw3.Role = GridRoleWyn.Query;
        gvw3.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        _list = await QueryAsync("USP_PR_RSLTSTAT_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "L",
            ["p_fr_date"] = dteSearchFrom.YyyyMmDd,
            ["p_to_date"] = dteSearchTo.YyyyMmDd,
            ["p_stat_cd"] = cboSearchStat.EditValue?.ToString(),
            ["p_proc_cd"] = cboSearchProc.EditValue?.ToString(),
            ["p_rslt_no"] = txtSearchRsltNo.Text,
            ["p_wo_no"] = txtSearchWoNo.Text,
            ["p_lot_no"] = txtSearchLotNo.Text,
        });
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

        var tables = await QueryMultiAsync("USP_PR_RSLT_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_rslt_id"] = view.Row["rslt_id"]?.ToString(),
        });
        grd2.DataSource = tables.Count > 2 ? tables[2] : null; // 산출 LOT
        grd3.DataSource = tables.Count > 1 ? tables[1] : null; // 웨이퍼 판정
    }

    private async Task OpenLinkAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        if (gvw1.FocusedColumn == col1RsltNo)
        {
            var id = view.Row["rslt_id"]?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "PR" && m.ScreenClassNm == "frmRslt");
            var form = new frmRslt { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
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
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
