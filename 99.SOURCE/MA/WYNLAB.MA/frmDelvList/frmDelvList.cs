using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 납품현황 - Master-SubGrid 조회 화면(위 grd1 납품 목록, 아래 grd2 선택한 납품의 품목과 검사상태). 둘 다 조회전용이고
/// 등록/수정/확정은 항상 frmDelv에서만 한다. 납품번호를 더블클릭하면 frmDelv가 그 건으로 열린다(frmPoReqList와 같은 방식).
/// </summary>
public partial class frmDelvList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmDelvList()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "납품현황";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadDetailAsync, "납품 품목 조회");
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenDelvAsync, "납품등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_delv_no"] = txtSearchDelvNo.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
        };
        _list = await QueryAsync("USP_MA_DELVLIST_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2를 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_DELVLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_delv_id"] = view.Row["delv_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    private async Task OpenDelvAsync()
    {
        if (gvw1.FocusedColumn != colDelvNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var delvId = view.Row["delv_id"]?.ToString();
        if (string.IsNullOrEmpty(delvId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmDelv");
        var form = new frmDelv { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(delvId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
