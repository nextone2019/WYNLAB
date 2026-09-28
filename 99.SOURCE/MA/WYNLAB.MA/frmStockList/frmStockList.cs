using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 재고현황 - 현재고(TMASTOCK)를 품목/창고/위치/LOT별로 보여주는 조회 전용 화면(USP_MA_STOCK_Q). 현재고는 입고/출고 확정 때 수불(TMATRANS)과
/// 같은 트랜잭션에서 갱신되는 캐시이고, 재고관리 품목(재고=Y)만 여기 나타난다. 재고 0인 행은 기본으로 숨긴다.
/// </summary>
public partial class frmStockList : BaseForm
{
    private DataTable _list = new();

    public frmStockList()
    {
        InitializeComponent();

        Text = "재고현황";

        Controls.Add(BuildScreenHeader());

        cboSearchZero.SelectedIndex = 0;

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_wh_keyword"] = txtSearchWh.Text,
            ["p_lot_no"] = txtSearchLot.Text,
            ["p_zero_yn"] = cboSearchZero.EditValue?.ToString() == "포함" ? "Y" : "N",
        };
        _list = await QueryAsync("USP_MA_STOCK_Q", p);
        grd1.DataSource = _list;
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
