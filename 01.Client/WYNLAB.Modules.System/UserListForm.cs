using DevExpress.XtraEditors;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;

namespace WYNLAB.Modules.System;

/// <summary>
/// 사용자관리 화면. TSMMENU.FORM_CLASS_NM 에 이 클래스의 어셈블리 정규화 이름이 등록되어
/// ShellForm에서 리플렉션으로 동적 오픈된다.
///
/// 화면 자체에는 버튼이 없다 - Shell 상단 공통 툴바(조회/입력/삭제/출력)가 이 화면이 활성화된 상태에서
/// QueryAsync/NewAsync/DeleteAsync/PrintAsync(BaseGridForm/BaseForm 상속)를 호출하는 구조.
/// </summary>
public class UserListForm : BaseGridForm
{
    private List<UserListItemDto> _currentList = new();

    public UserListForm()
    {
        Text = "사용자관리";
        MenuCd = "SM_USER"; // TSMMENU 등록 코드와 일치해야 권한이 정상 반영됨

        MainGridView.OptionsBehavior.Editable = false; // 그리드 직접편집 금지, 팝업으로만 수정
        MainGridView.DoubleClick += async (s, e) => await OpenEditPopupAsync();

        // 화면이 열리자마자 목록을 바로 보여주는 게 사용성이 좋음 (Shell 툴바 "조회" 안 눌러도 되도록)
        Load += async (s, e) => await QueryAsync();
    }

    public override async Task QueryAsync()
    {
        _currentList = await ApiClient.GetAsync<List<UserListItemDto>>("api/users") ?? new();
        MainGrid.DataSource = _currentList;
    }

    public override async Task NewAsync()
    {
        using var form = new UserEditForm(); // 신규모드
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }

    public override async Task DeleteAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserListItemDto;
        if (selected == null)
        {
            XtraMessageBox.Show("삭제할 사용자를 선택해주세요.", "안내");
            return;
        }

        var confirm = XtraMessageBox.Show($"'{selected.UserNm}({selected.UserId})' 사용자를 사용중지 처리하시겠습니까?",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/users/{selected.UserId}");
        await QueryAsync();
    }

    /// <summary>더블클릭으로 수정 팝업 오픈 - Shell 툴바가 아닌 그리드 자체 동작</summary>
    private async Task OpenEditPopupAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserListItemDto;
        if (selected == null) return;

        using var form = new UserEditForm(selected);
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryAsync();
        }
    }
}
