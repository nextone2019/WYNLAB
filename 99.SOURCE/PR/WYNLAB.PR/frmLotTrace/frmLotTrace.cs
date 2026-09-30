using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// LOT계보조회 - 조회전용 화면(위 grd1 검색한 LOT가 속한 계보 전체를 원 LOT(웨이퍼 입고 LOT)부터 산출 LOT까지 트리 모양으로, 아래 grd2 선택한 LOT의 이력:
/// 입고 / 공정 산출 / 공정 투입 / 외주이전). LOT번호나 작업지시번호를 넣어 조회하고, 이력의 문서번호를 더블클릭하면 해당 화면(frmRcv/frmRslt/frmXfer)이 열린다.
/// </summary>
public partial class frmLotTrace : BaseForm
{
    public frmLotTrace()
    {
        InitializeComponent();

        Text = "LOT계보조회";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await SafeExecuteAsync(LoadHistoryAsync, "LOT 이력 조회");
        gvw1.RowStyle += (s, e) =>
        {
            if (e.RowHandle >= 0 && gvw1.GetRow(e.RowHandle) is DataRowView v && v.Row.Table.Columns.Contains("hit") && Convert.ToInt32(v.Row["hit"]) == 1)
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold); // 검색한 LOT 강조
        };

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;
        gvw2.DoubleClick += async (s, e) => await SafeExecuteAsync(OpenDocAsync, "화면 열기");
    }

    public override async Task QueryClick()
    {
        if (string.IsNullOrWhiteSpace(txtSearchLotNo.Text) && string.IsNullOrWhiteSpace(txtSearchWoNo.Text))
        {
            AppMessageBox.Show("LOT번호 또는 작업지시번호를 입력하고 조회하세요.", "안내");
            return;
        }

        var list = await QueryAsync("USP_PR_LOTTRACE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "L",
            ["p_lot_no"] = txtSearchLotNo.Text.Trim(),
            ["p_wo_no"] = txtSearchWoNo.Text.Trim(),
        });
        grd1.DataSource = list;
        grd2.DataSource = null;
        if (list.Rows.Count == 0) Toast.Show("조회 결과가 없습니다.");
        else
        {
            // 검색한 LOT 행으로 포커스를 옮겨 그 이력이 바로 보이게 한다.
            var hit = list.Rows.Cast<DataRow>().FirstOrDefault(r => Convert.ToInt32(r["hit"]) == 1);
            var handle = hit == null ? 0 : gvw1.GetRowHandle(list.Rows.IndexOf(hit));
            if (handle >= 0) gvw1.FocusedRowHandle = handle;
        }
    }

    /// <summary>MAIN/SUB 그리드 관례 - grd1 선택행이 바뀔 때만 grd2를 다시 조회하고, 포커스 행이 없으면 SUB도 비운다.</summary>
    private async Task LoadHistoryAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view)
        {
            grd2.DataSource = null;
            return;
        }

        grd2.DataSource = await QueryAsync("USP_PR_LOTTRACE_Q", new Dictionary<string, string?>
        {
            ["p_work_type"] = "H",
            ["p_lot_id"] = view.Row["lot_id"]?.ToString(),
        });
    }

    private async Task OpenDocAsync()
    {
        if (gvw2.FocusedColumn != col2DocNo) return;
        if (gvw2.GetFocusedRow() is not DataRowView view) return;

        var id = view.Row["doc_id"]?.ToString();
        if (string.IsNullOrEmpty(id)) return;

        var kind = view.Row["doc_type"]?.ToString();
        var menuFor = (string cls) => SessionManager.Current.Menus.FirstOrDefault(m => m.Module == "PR" && m.ScreenClassNm == cls)?.MenuId ?? 0;
        BaseForm? form = kind switch
        {
            "RV" => new frmRcv { MenuId = menuFor("frmRcv"), MdiParent = MdiParent },
            "RS" => new frmRslt { MenuId = menuFor("frmRslt"), MdiParent = MdiParent },
            "XF" => new frmXfer { MenuId = menuFor("frmXfer"), MdiParent = MdiParent },
            _ => null,
        };
        if (form == null) return;
        form.Show();
        await form.FocusRecordAsync(id);
    }

    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;
    public override Task SaveClick() => Task.CompletedTask;
}
