using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.PR;

/// <summary>
/// 공정관리 - 공정 마스터(TBAPROC) 싱글그리드. 공정코드(BUMP/EDS/PKG/FT 등)와 공정명/정렬/사용여부를 그리드에서 바로 추가/수정/삭제한다.
/// 공정코드는 라우팅(TPRROUTED)/작업지시(TPRWOD)/실적이 코드값을 그대로 저장하므로 신규 등록 때만 입력하고, 쓰이는 공정은 삭제 대신 사용여부를 끈다
/// (서버가 삭제를 막는다). 사용여부=N인 공정은 라우팅 공정 콤보(L_PRPROC)에서 빠진다.
/// </summary>
public partial class frmProc : BaseForm
{
    private DataTable _list = new();

    public frmProc()
    {
        InitializeComponent();

        Text = "공정관리";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => AddRow();
        gvw1.RowDelete += (s, e) => DeleteRow();
        gvw1.InitNewRow += Gvw1_InitNewRow;
        gvw1.ShowingEditor += Gvw1_ShowingEditor;
        gvw1.CellValueChanged += Gvw1_CellValueChanged;

        btnAddRow1.Click += (s, e) => AddRow();
        btnDeletRow1.Click += (s, e) => DeleteRow();

        Load += async (s, e) => await SafeExecuteAsync(QueryClick, "공정 조회");
    }

    private void AddRow() => gvw1.AddNewRow();

    private void DeleteRow()
    {
        try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
        catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
    }

    /// <summary>새 행 기본값 - 사업장은 세션 값, 사용여부 Y, 정렬은 마지막+10.</summary>
    private void Gvw1_InitNewRow(object? sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
    {
        var max = 0m;
        foreach (DataRow r in _list.Rows)
            if (r.RowState != DataRowState.Deleted && decimal.TryParse(r["sort"]?.ToString(), out var s) && s > max) max = s;

        gvw1.SetRowCellValue(e.RowHandle, colAccId, Session.AccId);
        gvw1.SetRowCellValue(e.RowHandle, colUseYn, "Y");
        gvw1.SetRowCellValue(e.RowHandle, colSort, max + 10);
        gvw1.SetRowCellValue(e.RowHandle, colUsedYn, "N");
    }

    /// <summary>공정코드는 새 행에서만 입력할 수 있다(저장된 행은 코드를 못 바꾼다).</summary>
    private void Gvw1_ShowingEditor(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (gvw1.FocusedColumn != colProcCd) return;
        var row = gvw1.GetFocusedDataRow();
        if (row != null && row.RowState != DataRowState.Added && row.RowState != DataRowState.Detached) e.Cancel = true;
    }

    private void Gvw1_CellValueChanged(object? sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
    {
        if (e.Column == colSort && e.Value is string text)
            gvw1.SetRowCellValue(e.RowHandle, e.Column, decimal.TryParse(text.Replace(",", ""), out var n) ? n : 0m);
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            ["p_acc_id"] = Session.AccId?.ToString(),
            ["p_keyword"] = txtSearchKeyword.Text,
        };
        _list = await QueryAsync("USP_PR_PROC_Q", p);
        TrackDirty(_list);
        grd1.DataSource = _list;
    }

    public override async Task SaveClick()
    {
        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        // 삭제 -> 수정 -> 추가 순서(같은 코드를 지우고 다시 만드는 경우 대비)
        var rows = _list.Rows.Cast<DataRow>()
            .Where(r => r.RowState != DataRowState.Unchanged)
            .OrderBy(r => r.RowState == DataRowState.Deleted ? 0 : r.RowState == DataRowState.Modified ? 1 : 2)
            .ToList();

        foreach (var row in rows)
        {
            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p_acc_id"] = ProcData.Str(row, "acc_id", version),
                ["p_proc_cd"] = ProcData.Str(row, "proc_cd", version),
                ["p_proc_nm"] = ProcData.Str(row, "proc_nm", version),
                ["p_sort"] = ProcData.Str(row, "sort", version),
                ["p_use_yn"] = ProcData.Str(row, "use_yn", version),
                ["p_remark"] = ProcData.Str(row, "remark", version),
            };
            var result = await SaveAsync("USP_PR_PROC_S", p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                await QueryClick(); // 이미 저장된 행까지 반영해서 화면을 실제 상태에 맞춘다
                return;
            }
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    public override Task NewRowClick()
    {
        AddRow();
        return Task.CompletedTask;
    }

    public override Task DeleteRowClick()
    {
        DeleteRow();
        return Task.CompletedTask;
    }
}
