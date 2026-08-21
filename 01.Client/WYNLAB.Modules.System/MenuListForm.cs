using DevExpress.XtraEditors;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;

namespace WYNLAB.Modules.System;

/// <summary>
/// 메뉴관리 화면. TSMMENU를 직접 관리한다 - 여기서 등록한 메뉴가 ShellForm 좌측
/// Accordion에 그대로 반영된다 (로그인 시 새로 조회하므로, 반영하려면 재로그인 필요).
/// </summary>
public class MenuListForm : BaseGridForm
{
    private List<MenuListItemDto> _currentList = new();

    public MenuListForm()
    {
        Text = "메뉴관리";
        MenuCd = "SM_MENU";

        MainGridView.OptionsBehavior.Editable = false;
        MainGridView.DoubleClick += async (s, e) => await OpenEditPopupAsync();

        Load += async (s, e) => await QueryAsync();
    }

    public override async Task QueryAsync()
    {
        _currentList = await ApiClient.GetAsync<List<MenuListItemDto>>("api/menus") ?? new();
        MainGrid.DataSource = _currentList;
    }

    public override async Task NewAsync()
    {
        using var form = new MenuEditForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }

    public override async Task DeleteAsync()
    {
        var selected = MainGridView.GetFocusedRow() as MenuListItemDto;
        if (selected == null)
        {
            AppMessageBox.Show("삭제할 메뉴를 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"'{selected.MenuNm}({selected.MenuCd})' 메뉴를 사용중지 처리하시겠습니까?\n하위 메뉴가 있다면 좌측 메뉴트리에서 같이 사라집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/menus/{selected.MenuCd}");
        await QueryAsync();
    }

    private async Task OpenEditPopupAsync()
    {
        var selected = MainGridView.GetFocusedRow() as MenuListItemDto;
        if (selected == null) return;

        using var form = new MenuEditForm(selected);
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }
}
