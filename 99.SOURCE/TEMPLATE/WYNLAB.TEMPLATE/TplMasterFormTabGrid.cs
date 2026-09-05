// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-폼-탭그리드 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤 메뉴에도
// 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도.
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterFormTabGrid : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail1 = new();
    private DataTable _detail2 = new();
    private string? _editingKey; // null이면 신규모드

    public TplMasterFormTabGrid()
    {
        InitializeComponent();

        Text = "__MENU_CAPTION__";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await OnMasterSelectedAsync();

        // grd2/grd3는 MasterFormSubGrid의 grd2(조회전용)와 달리 편집 가능하다 - 각자 자기
        // 저장프로시저(__SAVE_PROC_1__/__SAVE_PROC_2__)로 저장되기 때문. Role=Edit이면 그리드
        // 자신의 EmbeddedNavigator에도 추가/삭제 버튼이 뜬다(탭당 하나씩, 독립 동작).
        gvw2.Role = GridRoleWyn.Edit;
        gvw2.HighlightFocusedRow = true;
        gvw2.RowAdd += (s, e) => gvw2.AddNewRow();
        gvw2.RowDelete += (s, e) =>
        {
            try { if (gvw2.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        gvw3.Role = GridRoleWyn.Edit;
        gvw3.HighlightFocusedRow = true;
        gvw3.RowAdd += (s, e) => gvw3.AddNewRow();
        gvw3.RowDelete += (s, e) =>
        {
            try { if (gvw3.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        // panelWyn1의 공용 추가/삭제 버튼 - 현재 활성 탭의 그리드에 적용(각 그리드 자체
        // EmbeddedNavigator와 별개로, 탭을 안 넘나들어도 되는 지름길).
        btnAddRow2.Click += (s, e) => ActiveDetailView().AddNewRow();
        btnDeletRow2.Click += (s, e) =>
        {
            try { if (ActiveDetailView().GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

    private GridViewWyn ActiveDetailView() => ReferenceEquals(tabDetailGrids.SelectedTabPage, tabDetail2) ? gvw3 : gvw2;

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            // @AI_BUILDER:BEGIN QUERY_PARAMS
            ["p_sample1"] = txtSearchQ.Text,
            // @AI_BUILDER:END QUERY_PARAMS
        };
        _list = await QueryAsync("__QUERY_PROC__", p);
        grd1.DataSource = _list;
        EnterNewMode();
    }

    private async Task OnMasterSelectedAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { EnterNewMode(); return; }

        var row = view.Row;
        _editingKey = row["__MASTER_KEY_COLUMN__"]?.ToString();
        // @AI_BUILDER:BEGIN DETAIL_FORM_ASSIGN
        //txtDetailSample1.Text = row["sample1"]?.ToString() ?? string.Empty;
        // @AI_BUILDER:END DETAIL_FORM_ASSIGN
        await LoadDetailAsync();
    }

    private void EnterNewMode()
    {
        _editingKey = null;
        // @AI_BUILDER:BEGIN DETAIL_FORM_CLEAR
        //txtDetailSample1.Text = string.Empty;
        // @AI_BUILDER:END DETAIL_FORM_CLEAR
        _detail1 = _detail1.Clone();
        _detail2 = _detail2.Clone();
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
    }

    /// <summary>선택된 마스터 행의 하위 목록 2개(grd2/grd3)를 한 번의 호출로 같이 조회한다 -
    /// __DETAIL_QUERY_PROC__가 work_type='__DETAIL_WORK_TYPE__'일 때 레코드셋을 2개(순서대로
    /// grd2용, grd3용) 반환하기 때문에 QueryMultiAsync를 쓴다(QueryAsync는 첫 레코드셋만 받음).</summary>
    private async Task LoadDetailAsync()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "__DETAIL_WORK_TYPE__",
            ["p___DETAIL_KEY_PARAM__"] = _editingKey,
        };
        var tables = await QueryMultiAsync("__DETAIL_QUERY_PROC__", p);
        _detail1 = tables.Count > 0 ? tables[0] : new DataTable();
        _detail2 = tables.Count > 1 ? tables[1] : new DataTable();
        grd2.DataSource = _detail1;
        grd3.DataSource = _detail2;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        // ---- 1) 헤더(panData -> __SAVE_PROC__) ----
        var headerParams = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            // @AI_BUILDER:BEGIN DETAIL_FORM_SAVE_PARAMS
            //["p_sample1"] = txtDetailSample1.Text,
            // @AI_BUILDER:END DETAIL_FORM_SAVE_PARAMS
        };

        var headerResult = await SaveAsync("__SAVE_PROC__", headerParams);
        if (headerResult == null || !headerResult.Success)
        {
            AppMessageBox.Show(headerResult?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var headerKey = _editingKey ?? headerResult.GeneratedCode;

        // ---- 2) 명세1(grd2 -> __SAVE_PROC_1__) ----
        var detail1Ok = await SaveDetailRowsAsync(gvw2, _detail1, "__SAVE_PROC_1__", headerKey, (row, version) => new Dictionary<string, string?>
        {
            // @AI_BUILDER:BEGIN DETAIL1_SAVE_PARAMS
            ["p_sample1"] = ProcData.Str(row, "sample1", version),
            // @AI_BUILDER:END DETAIL1_SAVE_PARAMS
        });
        if (!detail1Ok) return;

        // ---- 3) 명세2(grd3 -> __SAVE_PROC_2__) ----
        var detail2Ok = await SaveDetailRowsAsync(gvw3, _detail2, "__SAVE_PROC_2__", headerKey, (row, version) => new Dictionary<string, string?>
        {
            // @AI_BUILDER:BEGIN DETAIL2_SAVE_PARAMS
            ["p_sample1"] = ProcData.Str(row, "sample1", version),
            // @AI_BUILDER:END DETAIL2_SAVE_PARAMS
        });
        if (!detail2Ok) return;

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    /// <summary>grd2/grd3 공통 저장 루프 - 변경된 행마다 N/U/D로 나눠 저장한다(SingleGrid.SaveClick과
    /// 같은 RowState 판정 방식). extraParams는 화면마다 다른 컬럼->파라미터 매핑을 행 하나 기준으로
    /// 만들어주는 콜백(생성기가 컬럼 목록으로 채워넣음).</summary>
    private async Task<bool> SaveDetailRowsAsync(GridViewWyn gvw, DataTable table, string saveProc, string? masterKey,
        Func<DataRow, DataRowVersion, Dictionary<string, string?>> extraParams)
    {
        gvw.CloseEditor();
        gvw.UpdateCurrentRow();

        foreach (DataRow row in table.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            // Deleted 행에서 DataRowVersion.Current를 읽으면 DeletedRowInaccessibleException이 난다
            // (ProcData.Str 주석 참고) - 그래서 컬럼 매핑에도 이 버전을 그대로 넘겨준다.
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                ["p___MASTER_KEY_COLUMN__"] = masterKey,
            };
            foreach (var kv in extraParams(row, version)) p[kv.Key] = kv.Value;

            var result = await SaveAsync(saveProc, p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return false;
            }
        }
        return true;
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "D",
            ["p___MASTER_KEY_COLUMN__"] = _editingKey,
        };
        var result = await SaveAsync("__SAVE_PROC__", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "삭제에 실패했습니다.", "삭제 실패");
            return;
        }

        Toast.Show("삭제되었습니다.");
        await QueryClick();
    }
}
