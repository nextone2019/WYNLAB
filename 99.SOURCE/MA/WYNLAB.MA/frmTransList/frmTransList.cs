using System.Data;
using System.Drawing;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 수불현황 - 수불 원장(TMATRANS)을 일자/품목/창고/구분별로 보여주는 조회 전용 화면(USP_MA_TRANSLIST_Q). 증감은 입고 +, 출고 -이고
/// 취소(역거래)는 반대 방향 행으로 남으며 역거래=Y로 표시된다. 출고 행과 역거래 행은 색으로 구분한다.
/// </summary>
public partial class frmTransList : BaseForm
{
    private DataTable _list = new();

    public frmTransList()
    {
        InitializeComponent();

        Text = "수불현황";

        Controls.Add(BuildScreenHeader());

        cboSearchKind.SelectedIndex = 0;
        dteSearchFrom.YyyyMmDd = DateTime.Today.AddMonths(-1).ToString("yyyyMMdd");
        dteSearchTo.YyyyMmDd = DateTime.Today.ToString("yyyyMMdd");

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowCellStyle += Gvw1_RowCellStyle;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var kind = cboSearchKind.EditValue?.ToString();
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_wh_keyword"] = txtSearchWh.Text,
            ["p_trans_kind"] = kind == "입고" ? "I" : kind == "출고" ? "O" : string.Empty,
        };
        _list = await QueryAsync("USP_MA_TRANSLIST_Q", p);
        grd1.DataSource = _list;
    }

    /// <summary>증감 칸은 입고=파랑/출고=빨강, 역거래 행의 구분/유형은 회색 이탤릭 느낌으로 눈에 띄지 않게 처리한다.</summary>
    private void Gvw1_RowCellStyle(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    {
        if (e.RowHandle < 0) return;

        if (e.Column == colSignedQty)
        {
            var kind = gvw1.GetRowCellValue(e.RowHandle, colTransKind)?.ToString();
            e.Appearance.ForeColor = kind == "I" ? Color.FromArgb(21, 101, 192) : Color.FromArgb(198, 40, 40);
        }
        else if (gvw1.GetRowCellValue(e.RowHandle, colReversalYn)?.ToString() == "Y")
        {
            e.Appearance.ForeColor = Color.FromArgb(120, 124, 132);
        }
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
