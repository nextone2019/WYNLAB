// @AI_BUILDER:BEGIN FILE_HEADER
// AI Builder 마스터-상세폼-서브그리드 템플릿 원본 - 이 파일 자체는 실행되지 않는다(어떤
// 메뉴에도 등록돼 있지 않음). VS에서 열어 로직 골격을 확인/조정하는 용도.
// @AI_BUILDER:END FILE_HEADER
using System.Data;
using WYNLAB.Base;
using WYNLAB.Base.Controls;

namespace WYNLAB.TEMPLATE;

public partial class TplMasterFormSubGrid : BaseForm
{
    private DataTable _list = new();
    private DataTable _detail = new();
    private string? _editingKey; // null이면 신규모드

    public TplMasterFormSubGrid()
    {
        InitializeComponent();

        Text = "__MENU_CAPTION__";
        MenuCd = "__MENU_CD__";

        Controls.Add(BuildScreenHeader());

        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw1.FocusedRowObjectChanged += async (s, e) => await OnMasterSelectedAsync();

        gvw2.Role = GridRoleWyn.Query; // 하위그리드는 조회전용으로 생성됨 - 편집/저장이 필요하면 직접 추가
        gvw2.HighlightFocusedRow = true;

      

        EnterNewMode();
        Load += async (s, e) => await QueryClick();
    }

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
        _detail = _detail.Clone();
        grd2.DataSource = _detail;
    }

    /// <summary>선택된(또는 방금 저장한) 마스터 행의 하위 목록을 조회한다.</summary>
    private async Task LoadDetailAsync()
    {
        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = "__DETAIL_WORK_TYPE__",
            ["p___DETAIL_KEY_PARAM__"] = _editingKey,
        };
        _detail = await QueryAsync("__QUERY_PROC__", p);
        grd2.DataSource = _detail;
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrEmpty("__SAVE_PROC__"))
        {
            AppMessageBox.Show("이 화면은 조회전용으로 생성됐습니다 - 저장프로시저가 지정되지 않았습니다.", "저장 불가");
            return;
        }

        var p = new Dictionary<string, string?>
        {
            ["p_work_type"] = _editingKey == null ? "N" : "U",
            // @AI_BUILDER:BEGIN DETAIL_FORM_SAVE_PARAMS
            //["p_sample1"] = txtDetailSample1.Text,
            // @AI_BUILDER:END DETAIL_FORM_SAVE_PARAMS
        };

        var result = await SaveAsync("__SAVE_PROC__", p);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    public override async Task DeleteClick()
    {
        if (_editingKey == null) return;

        if (string.IsNullOrEmpty("__SAVE_PROC__"))
        {
            AppMessageBox.Show("저장프로시저가 지정되지 않았습니다.", "삭제 불가");
            return;
        }

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
