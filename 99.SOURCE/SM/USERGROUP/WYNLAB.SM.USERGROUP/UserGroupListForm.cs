using DevExpress.XtraGrid.Views.Base;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;

namespace WYNLAB.SM.USERGROUP;

/// <summary>
/// 사용자그룹관리 화면. 좌측에 등록된 그룹 전체를 리스트로 보여주고, 우측에서 바로 등록/수정한다
/// (기존엔 더블클릭으로 별도 팝업(UserGroupEditForm)을 띄우는 구조였는데, 팝업을 열고 닫는 과정
/// 자체가 번거롭고 메뉴관리(MenuListForm) 화면과 조작 방식이 달라 헷갈린다는 피드백에 따라
/// 메뉴관리와 같은 "리스트 클릭 -> 옆에서 바로 편집 -> 저장" 구조로 통일했다. 팝업이 사라졌으므로
/// UserGroupEditForm은 더 이상 쓰지 않는다).
///
/// 소속 사용자 배정(TSMUSERGRPMAP)은 그룹코드(PK)가 있어야 의미가 있어서, 신규모드에서는 우측
/// 하단의 배정 그리드 자체를 숨긴다(EnterNewMode) - 신규 저장에 성공하면 그 자리에서 바로
/// 수정모드로 전환되면서(EnterEditMode) 배정 그리드가 나타난다(팝업을 닫았다 다시 여는 예전
/// 흐름 없이 한 화면 안에서 이어짐).
/// </summary>
public partial class UserGroupListForm : BaseForm
{
    private List<UserGroupListItemDto> _groups = new();
    private List<UserGroupMemberDto> _members = new();
    private string? _editingUserGrpCd; // null이면 신규모드

    public UserGroupListForm()
    {
        InitializeComponent();

        MenuCd = "SM_USERGRP"; // TSMMENU 등록 코드와 일치해야 권한이 정상 반영됨
        Controls.Add(BuildScreenHeader()); // 검색패널/스플리터보다 나중에 추가해야 맨 위를 차지한다

        PositionFooterButtons();
        EnterNewMode();
    }

    private async void UserGroupListForm_Load(object? sender, EventArgs e) => await QueryClick();

    private async void txtSearchGrpNm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.Handled = true;
        await QueryClick();
    }

    private async void btnSearch_Click(object? sender, EventArgs e) => await QueryClick();

    private void formFooterPanel_Resize(object? sender, EventArgs e) => PositionFooterButtons();

    private void PositionFooterButtons()
    {
        btnCancelEdit.Location = new Point(formFooterPanel.Width - btnCancelEdit.Width - 16, 12);
        btnSaveInline.Location = new Point(btnCancelEdit.Left - btnSaveInline.Width - 8, 12);
    }

    private async void btnSaveInline_Click(object? sender, EventArgs e) => await SaveClick();

    private void btnCancelEdit_Click(object? sender, EventArgs e)
    {
        if (_editingUserGrpCd == null) EnterNewMode();
        else
        {
            var group = _groups.FirstOrDefault(g => g.UserGrpCd == _editingUserGrpCd);
            if (group != null) EnterEditMode(group); else EnterNewMode();
        }
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is UserGroupListItemDto group) EnterEditMode(group);
    }

    public override async Task QueryClick()
    {
        var query = $"api/user-groups?userGrpNm={Uri.EscapeDataString(txtSearchGrpNm.Text.Trim())}";
        _groups = await ApiClient.GetAsync<List<UserGroupListItemDto>>(query) ?? new();
        grd1.DataSource = _groups;

        // 수정 중이던 그룹이 검색결과에서 빠졌으면(삭제됐거나 검색조건에 안 맞음) 신규모드로 복귀
        if (_editingUserGrpCd != null && _groups.All(g => g.UserGrpCd != _editingUserGrpCd))
        {
            EnterNewMode();
        }
    }

    public override Task NewClick()
    {
        EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (_editingUserGrpCd == null)
        {
            AppMessageBox.Show("삭제할 그룹을 왼쪽 리스트에서 선택해주세요.", "안내");
            return;
        }

        var group = _groups.FirstOrDefault(g => g.UserGrpCd == _editingUserGrpCd);
        var confirm = AppMessageBox.Show(
            $"'{group?.UserGrpNm}({_editingUserGrpCd})' 그룹을 사용중지 처리하시겠습니까?\n소속된 사용자 {group?.MemberCount ?? 0}명의 그룹 권한 합산에서 이 그룹이 빠집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/user-groups/{_editingUserGrpCd}");
        await QueryClick();
        Toast.Show("사용중지 처리되었습니다.");
    }

    private void EnterNewMode()
    {
        _editingUserGrpCd = null;
        _members = new();

        txtUserGrpCd.Text = string.Empty;
        txtUserGrpCd.Enabled = true;
        txtUserGrpNm.Text = string.Empty;
        txtDescription.Text = string.Empty;
        spnSortOrder.Value = 0;
        chkUseYn.Checked = true;
        chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)

        sectionFormHeader.Text = "신규 사용자그룹 등록";
        memberSectionPanel.Visible = false; // 그룹코드가 아직 없어 배정 그리드는 저장 후에나 의미가 있음
        grd2.DataSource = null;

        txtUserGrpNm.Focus();
    }

    private void EnterEditMode(UserGroupListItemDto group)
    {
        var isSameGroup = _editingUserGrpCd == group.UserGrpCd;
        _editingUserGrpCd = group.UserGrpCd;

        txtUserGrpCd.Text = group.UserGrpCd;
        txtUserGrpCd.Enabled = false; // 그룹코드는 PK라 수정 불가
        txtUserGrpNm.Text = group.UserGrpNm;
        txtDescription.Text = group.Description;
        spnSortOrder.Value = group.SortOrder;
        chkUseYn.Checked = group.UseYn;
        chkUseYn.Enabled = true;

        sectionFormHeader.Text = "사용자그룹 정보 수정";
        memberSectionPanel.Visible = true;

        // grd1.DataSource를 다시 세팅할 때마다(QueryClick 안) 포커스 행이 새로 잡히면서
        // FocusedRowObjectChanged가 또 발생한다 - 같은 그룹인데 배정 그리드를 또 불러오는
        // 불필요한 재조회를 막는다(CodeListForm의 EnterEditMode와 같은 이유).
        if (!isSameGroup)
        {
            _ = LoadMembers(group.UserGrpCd);
        }
    }

    private async Task LoadMembers(string userGrpCd)
    {
        _members = await ApiClient.GetAsync<List<UserGroupMemberDto>>($"api/user-groups/{userGrpCd}/members") ?? new();
        grd2.DataSource = _members;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtUserGrpCd.Text) || string.IsNullOrWhiteSpace(txtUserGrpNm.Text))
        {
            AppMessageBox.Show("그룹코드와 그룹명은 필수입니다.", "확인");
            return;
        }

        btnSaveInline.Enabled = false;
        try
        {
            ApiResult? result;
            var wasNew = _editingUserGrpCd == null;
            var savedUserGrpCd = wasNew ? txtUserGrpCd.Text : _editingUserGrpCd!;

            if (wasNew)
            {
                var req = new UserGroupCreateRequest
                {
                    UserGrpCd = txtUserGrpCd.Text,
                    UserGrpNm = txtUserGrpNm.Text,
                    Description = txtDescription.Text,
                    SortOrder = (int)spnSortOrder.Value
                };
                result = await ApiClient.PostAsync<UserGroupCreateRequest, ApiResult>("api/user-groups", req);
            }
            else
            {
                var req = new UserGroupUpdateRequest
                {
                    UserGrpNm = txtUserGrpNm.Text,
                    Description = txtDescription.Text,
                    SortOrder = (int)spnSortOrder.Value,
                    UseYn = chkUseYn.Checked
                };
                result = await ApiClient.PutAsync<UserGroupUpdateRequest, ApiResult>($"api/user-groups/{_editingUserGrpCd}", req);
            }

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }

            // 소속 사용자 배정은 수정모드(=그룹이 이미 존재)일 때만 같이 저장한다 - 신규모드는
            // 배정 그리드 자체가 숨겨져 있어(EnterNewMode) _members가 비어있다.
            if (!wasNew)
            {
                gvw2.CloseEditor();
                gvw2.UpdateCurrentRow();
                var checkedUserIds = _members.Where(m => m.IsMember).Select(m => m.UserId).ToList();
                var memberResult = await ApiClient.PutAsync<UpdateGroupMembersRequest, ApiResult>(
                    $"api/user-groups/{savedUserGrpCd}/members", new UpdateGroupMembersRequest { UserIds = checkedUserIds });

                if (memberResult == null || !memberResult.Success)
                {
                    AppMessageBox.Show(memberResult?.Message ?? "소속 배정 저장에 실패했습니다.", "저장 실패");
                    return;
                }
            }

            await QueryClick();

            // 리스트 인덱스가 아니라 값으로 찾는다 - gvw1에 정렬/필터가 걸려있으면 인덱스 순서가
            // _groups 리스트 순서와 어긋날 수 있어서(LocateByValue는 그 어떤 정렬 상태에서도 정확함).
            var savedRowHandle = gvw1.LocateByValue("UserGrpCd", savedUserGrpCd);
            if (savedRowHandle >= 0) gvw1.FocusedRowHandle = savedRowHandle;

            Toast.Show(wasNew ? "그룹이 등록되었습니다." : "수정되었습니다.");
        }
        finally
        {
            btnSaveInline.Enabled = true;
        }
    }
}
