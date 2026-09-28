using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 수입검사현황 - Master-SubGrid 조회 화면(위 grd1 검사 목록과 합격/불합격 합계, 아래 grd2 선택한 검사의 품목별 판정).
/// 조회전용이고 검사번호를 더블클릭하면 frmIqc가 그 건으로 열린다.
/// </summary>
public partial class frmIqcList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmIqcList()
    {
        InitializeComponent();

        Text = "수입검사현황";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "검사 품목 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenIqcAsync, "수입검사등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_iqc_no"] = txtSearchIqcNo.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
        };
        _list = await QueryAsync("USP_MA_IQCLIST_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_IQCLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_iqc_id"] = view.Row["iqc_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    private async Task OpenIqcAsync()
    {
        if (gvw1.FocusedColumn != colIqcNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var iqcId = view.Row["iqc_id"]?.ToString();
        if (string.IsNullOrEmpty(iqcId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmIqc");
        var form = new frmIqc { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(iqcId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
