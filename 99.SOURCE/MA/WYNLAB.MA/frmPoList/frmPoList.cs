using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매발주현황 - frmPoReqList와 완전히 같은 Master-SubGrid 구조(grd1 목록 위/grd2 품목상세
/// 아래, 둘 다 조회전용). 발주번호 컬럼을 더블클릭하면 frmPo를 그 건으로 열어 조회한다.
/// </summary>
public partial class frmPoList : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public frmPoList()
    {
        InitializeComponent();

        Text = "구매발주현황";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await LoadDetailAsync();
        gvw1.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenPoAsync, "구매발주등록 열기");

        gvw2.Role = GridRoleWyn.Query;
        // 합계금액 합계를 그리드 하단(Footer)에 상시 표시(2026-09-26 요청).
        gvw2.AddColumnSummary(colDetTotalAmt, DevExpress.Data.SummaryItemType.Sum, "{0:#,##0.##}");
        gvw2.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_po_no"] = txtSearchPoNo.Text,
            ["p_po_title"] = txtSearchPoTitle.Text,
        };
        _list = await QueryAsync("USP_MA_POLIST_Q", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>grd1 선택행이 바뀔 때만 grd2(품목 상세)를 다시 조회한다 - MAIN/SUB 그리드 관례대로,
    /// 포커스 행이 없으면 SUB도 반드시 비운다.</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        _detail = await QueryAsync("USP_MA_POLIST_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q1",
            ["p_po_id"] = view.Row["po_id"]?.ToString(),
        });
        grd2.DataSource = _detail;
    }

    /// <summary>발주번호 더블클릭 - 같은 모듈(MA) 안이라 리플렉션 없이 바로 새 인스턴스를 열고
    /// FocusRecordAsync(po_id)로 그 건에 포커스시킨다(frmPoReqList.OpenReqAsync와 같은 원리).</summary>
    private async Task OpenPoAsync()
    {
        if (gvw1.FocusedColumn != colPoNo) return;
        if (gvw1.GetFocusedRow() is not DataRowView view) return;

        var poId = view.Row["po_id"]?.ToString();
        if (string.IsNullOrEmpty(poId)) return;

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "MA" && m.ScreenClassNm == "frmPo");
        var form = new frmPo { MenuId = menu?.MenuId ?? 0, MdiParent = MdiParent };
        form.Show();
        await form.FocusRecordAsync(poId);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
