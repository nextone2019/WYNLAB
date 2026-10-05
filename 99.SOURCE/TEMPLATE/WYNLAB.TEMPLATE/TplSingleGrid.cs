// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 싱글그리드 템플릿 원본 - 실제 업무로직은 생성기가 "// @AI_BUILDER:BEGIN ~ END"
// 구간과 "__토큰__" 표시만 갈아끼우고, 나머지(QueryClick/SaveClick의 전체 흐름, 행추가/삭제
// 처리 등)는 그대로 복제한다. 이 파일 자체는 실행되지 않는다(TplSingleGrid는 어떤 메뉴에도
// 등록돼 있지 않음) - VS에서 열어 로직 골격을 확인/조정하는 용도.
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplSingleGrid : BaseForm
{
    private DataTable _list = new();

    public TplSingleGrid()
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
    }

    public override async Task SaveClick()
    {
        // 저장프로시저가 지정 안 된 조회전용 화면이면 여기서 막는다 - 코드생성 시점에 이 메서드
        // 자체를 없애는 대신 실행시점 가드로 처리해서, 나중에 저장프로시저를 붙여도 이 메서드
        // 골격을 그대로 재사용할 수 있게 한다.
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
