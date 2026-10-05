using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.SA;

/// <summary>
/// 수주현황 - frmPoList(구매발주현황)와 완전히 같은 Master-SubGrid 구조(grd1 목록 위/grd2 품목상세
/// 아래, 둘 다 조회전용). 수주번호 컬럼을 더블클릭하면 frmSo를 그 건으로 열어 조회한다.
/// </summary>
public partial class frmSoList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmSoList()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "수주현황";

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await LoadDetailAsync();
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenSoAsync, "수주등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        gvw2.AddColumnSummary(colDetTotalAmt, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.##}");
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_so_no"] = txtSearchSoNo.Text,
            ["p_so_title"] = txtSearchSoTitle.Text,
        };
        _list = await QueryAsync("USP_SA_SOLIST_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>grd1 선택행이 바뀔 때만 grd2(품목 상세)를 다시 조회한다 - MAIN/SUB 그리드 관례대로,
    /// 포커스 행이 없으면 SUB도 반드시 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_SA_SOLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_so_id"] = view.Row["so_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    /// <summary>수주번호 더블클릭 - 같은 모듈(SA) 안이라 리플렉션 없이 바로 새 인스턴스를 열고
    /// FocusRecordAsync(so_id)로 그 건에 포커스시킨다(frmPoList.OpenPoAsync와 같은 원리).</summary>
    private async Task OpenSoAsync()
    {
        if (gvw1.FocusedColumn != colSoNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var soId = view.Row["so_id"]?.ToString();
        if (string.IsNullOrEmpty(soId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "SA" && m.ScreenClassNm == "frmSo");
        var form = new frmSo { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(soId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
