using System.Data;
using System.Drawing;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.MA;

/// <summary>
/// 구매진행현황 - 승인 완료된 발주 라인 단위로 납품/검사/입고대기 진행을 한 줄에 보여주는 조회 전용 화면(USP_MA_POSTATUS_Q).
/// 구분 콤보로 미납품/납기지연/검사대기/불합격발생/입고대기인 라인만 골라볼 수 있다. 납기지연(지연일&gt;0)과 검사대기/입고대기
/// 수량이 있는 칸은 색으로 눈에 띄게 표시한다. 입고대기 수량은 VMA_GR_READY(입고 설계와의 인계 뷰) 기준이다.
/// </summary>
public partial class frmPoStatus : BaseForm
{
    private static readonly Dictionary<string, string> StatusCodes = new()
    {
        ["전체"] = "", ["미납품"] = "UNDELV", ["납기지연"] = "LATE", ["검사대기"] = "IQCWAIT", ["불합격발생"] = "FAIL", ["입고대기"] = "GRREADY",
    };

    private DataTable _list = new();

    public frmPoStatus()
    {
        InitializeComponent();

        // 조회조건 사업장 - 화면 표준(2026-10-03): 항상 첫 번째, Required, 화면을 열면 로그인 사업장이 기본값.
        cboSearchAccId.EditValue = Session.AccId?.ToString() ?? string.Empty;
        cboSearchAccId.Tag = new BindingFieldTag("acc_id");

        Text = "구매진행현황";


        cboSearchType.SelectedIndex = 0;

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowCellStyle += Gvw1_RowCellStyle;

        cboSearchType.EditValueChanged += async (s, e) => await SafeExecuteAsync(QueryClick, "조회");

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var type = cboSearchType.EditValue?.ToString();
        var p = new Dictionary<string, string?>
        {
            ["p_acc_id"] = cboSearchAccId.EditValue?.ToString(),
            ["p_work_type"] = "Q",
            ["p_status_type"] = type != null && StatusCodes.TryGetValue(type, out var code) ? code : string.Empty,
            ["p_po_no"] = txtSearchPoNo.Text,
            ["p_keyword"] = txtSearchKeyword.Text,
            ["p_date_from"] = dteSearchFrom.YyyyMmDd,
            ["p_date_to"] = dteSearchTo.YyyyMmDd,
        };
        _list = await QueryAsync("USP_MA_POSTATUS_Q", p);
        grd1.DataSource = _list;
    }

    private static decimal Dec(object? value) => decimal.TryParse(value?.ToString(), out var d) ? d : 0;

    /// <summary>지연일/검사대기/불합격/입고대기가 있는 칸만 색으로 강조(값이 0인 칸은 그대로).</summary>
    private void Gvw1_RowCellStyle(object? sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
    {
        if (e.RowHandle < 0) return;

        if (e.Column == colLateDays && Dec(gvw1.GetRowCellValue(e.RowHandle, colLateDays)) > 0)
        {
            e.Appearance.ForeColor = Color.FromArgb(198, 40, 40);
            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
        }
        else if (e.Column == colIqcWaitQty && Dec(gvw1.GetRowCellValue(e.RowHandle, colIqcWaitQty)) > 0)
        {
            e.Appearance.ForeColor = Color.FromArgb(180, 100, 0);
        }
        else if ((e.Column == colFailRetQty || e.Column == colFailScrapQty) && Dec(gvw1.GetRowCellValue(e.RowHandle, e.Column)) > 0)
        {
            e.Appearance.ForeColor = Color.FromArgb(198, 40, 40);
        }
        else if (e.Column == colGrReadyQty && Dec(gvw1.GetRowCellValue(e.RowHandle, colGrReadyQty)) > 0)
        {
            e.Appearance.ForeColor = Color.FromArgb(21, 101, 192);
        }
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    protected override bool ConfirmDeleteByDefault => false; // 조회전용 - 삭제 기능 없음
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
