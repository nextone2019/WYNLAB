using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매입고현황 - Master-SubGrid 조회 화면(위 grd1 입고 목록, 아래 grd2 선택한 입고의 품목). 조회전용이고 입고번호를 더블클릭하면
/// frmGr가 그 건으로 열린다. 구매입고(수불유형 PU_IN)만 보인다.
/// </summary>
public partial class frmGrList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmGrList()
    {
        InitializeComponent();

        Text = "구매입고현황";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "입고 품목 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenGrAsync, "구매입고등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_gr_no"] = txtSearchGrNo.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
            ["p_trans_type"] = "PU_IN",
        };
        _list = await QueryAsync("USP_MA_GRLIST_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2를 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_GRLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_gr_id"] = view.Row["gr_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    private async Task OpenGrAsync()
    {
        if (gvw1.FocusedColumn != colGrNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var grId = view.Row["gr_id"]?.ToString();
        if (string.IsNullOrEmpty(grId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmGr");
        var form = new frmGr { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(grId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
