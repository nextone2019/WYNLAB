// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-서브그리드 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤 메뉴에도
// 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도.
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterSubGrid : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();

    public TplMasterSubGrid()
    {
        InitializeComponent();
        // 검색조건 기본값(화면 표준 - 첫 번째 검색조건 사업장은 로그인 사업장이 기본값, 2026-10-03)
        // @AI_BUILDER:BEGIN SEARCH_DEFAULTS
        // @AI_BUILDER:END SEARCH_DEFAULTS

        Text = "__MENU_CAPTION__";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Edit;
        gvw1.HighlightFocusedRow = true;
        gvw1.RowAdd += (s, e) => gvw1.AddNewRow();
        gvw1.RowDelete += (s, e) =>
        {
            try { if (gvw1.GetFocusedRow() is DataRowView view) view.Row.Delete(); }
            catch (Exception ex) { AppMessageBox.Show(ex.Message, "삭제 실패"); }
        };
        gvw1.FocusedRowObjectChanged += async (s, e) => await LoadDetailAsync();

        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        btnQuery.Click += async (s, e) => await QueryClick();
        btnSave.Click += async (s, e) => await SaveClick();

        Load += async (s, e) => await QueryClick();
    }

    public override async Task QueryClick()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "Q",
            // @AI_BUILDER:BEGIN QUERY_PARAMS
            ["p_sample1"] = txtSample1.Text,
            // @AI_BUILDER:END QUERY_PARAMS
        };
        _list = await QueryAsync("__QUERY_PROC__", p);
        grd1.DataSource = _list;
        grd2.DataSource = null;
    }

    /// <summary>grd1 선택행이 바뀔 때만 grd2를 다시 조회한다(frmDept.cs 패턴).</summary>
    private async Task LoadDetailAsync()
    {
        if (gvw1.GetFocusedRow() is not DataRowView view) { grd2.DataSource = null; return; }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "__DETAIL_WORK_TYPE__",
            ["p___DETAIL_KEY_PARAM__"] = view.Row["__MASTER_KEY_COLUMN__"]?.ToString(),
        };
        _detail = await QueryAsync("__QUERY_PROC__", p);
        grd2.DataSource = _detail;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrEmpty("__SAVE_PROC__"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        gvw1.CloseEditor();
        gvw1.UpdateCurrentRow();

        foreach (DataRow row in _list.Rows.Cast<DataRow>().ToList())
        {
            if (row.RowState == DataRowState.Unchanged) continue;

            var workType = row.RowState == DataRowState.Deleted ? "D" : row.RowState == DataRowState.Added ? "N" : "U";
            var version = row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

            var p = new Dictionary<string, string?>
            {
                ["p_work_type"] = workType,
                // @AI_BUILDER:BEGIN SAVE_PARAMS
                ["p_sample1"] = row["sample1", version]?.ToString(),
                // @AI_BUILDER:END SAVE_PARAMS
            };

            var result = await SaveAsync("__SAVE_PROC__", p);
            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }
}
