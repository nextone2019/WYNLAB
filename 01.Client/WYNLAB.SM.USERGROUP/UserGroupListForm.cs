using WYNLAB.Shared.Dtos;
using WYNLAB.Base;

namespace WYNLAB.SM.USERGROUP;

/// <summary>
/// 사용자그룹관리 화면. TSMUSERGRP를 관리하고, 더블클릭으로 들어가는 수정 팝업(UserGroupEditForm)
/// 안에서 그룹 소속 사용자 배정(TSMUSERGRPMAP)까지 같이 처리한다.
/// UserListForm과 동일하게 검색조건(그룹명) 패널을 상단에 둔다.
/// </summary>
public partial class UserGroupListForm : BaseGridForm
{
    private List<UserGroupListItemDto> _currentList = new();

    public UserGroupListForm()
    {
        InitializeComponent();

        MenuCd = "SM_USERGRP"; // TSMMENU 등록 코드와 일치해야 권한이 정상 반영됨
        Controls.Add(BuildScreenHeader()); // 검색패널보다 나중에 추가해야 맨 위를 차지한다
    }

    private async void UserGroupListForm_Load(object? sender, EventArgs e) => await QueryClick();

    private async void MainGridView_DoubleClick(object? sender, EventArgs e) => await OpenEditPopupAsync();

    private async void txtSearchGrpNm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.Handled = true;
        await QueryClick();
    }

    private async void btnSearch_Click(object? sender, EventArgs e) => await QueryClick();

    public override async Task QueryClick()
    {
        var query = $"api/user-groups?userGrpNm={Uri.EscapeDataString(txtSearchGrpNm.Text.Trim())}";
        _currentList = await ApiClient.GetAsync<List<UserGroupListItemDto>>(query) ?? new();
        MainGrid.DataSource = _currentList;
    }

    public override async Task NewClick()
    {
        using var form = new UserGroupEditForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryClick();
        }
    }

    public override async Task DeleteClick()
    {
        var selected = MainGridView.GetFocusedRow() as UserGroupListItemDto;
        if (selected == null)
        {
            AppMessageBox.Show("삭제할 그룹을 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show(
            $"'{selected.UserGrpNm}({selected.UserGrpCd})' 그룹을 사용중지 처리하시겠습니까?\n소속된 사용자 {selected.MemberCount}명의 그룹 권한 합산에서 이 그룹이 빠집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/user-groups/{selected.UserGrpCd}");
        await QueryClick();
    }

    private async Task OpenEditPopupAsync()
    {
        var selected = MainGridView.GetFocusedRow() as UserGroupListItemDto;
        if (selected == null) return;

        using var form = new UserGroupEditForm(selected);
        if (form.ShowDialog() == DialogResult.OK)
        {
            await QueryClick();
        }
    }
}
