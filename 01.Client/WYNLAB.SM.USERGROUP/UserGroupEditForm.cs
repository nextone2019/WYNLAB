using DevExpress.XtraEditors;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.SM.USERGROUP;

/// <summary>
/// 사용자그룹 신규등록/수정 팝업. UserEditForm/MenuEditForm과 같은 LayoutControl 패턴을 쓰되,
/// 수정모드에서는 그 아래에 "소속 사용자 배정" 그리드를 추가로 둔다 - 전체 사용자를 체크그리드로
/// 보여주고, 체크된 사용자 목록을 저장 시점에 한 번에 치환(PUT .../members) 한다.
/// 신규등록 모드에서는 그룹코드(PK)가 아직 없어 배정 그리드를 숨긴다(컨트롤 자체는 항상 만들어두고
/// Visible만 토글) - 먼저 그룹을 만들고 나서 다시 들어와 배정하는 흐름.
/// </summary>
public partial class UserGroupEditForm : XtraForm
{
    private readonly bool _isEditMode;
    private readonly string? _originalUserGrpCd;
    private List<UserGroupMemberDto> _members = new();

    /// <summary>신규등록 모드</summary>
    public UserGroupEditForm() : this(null) { }

    /// <summary>수정모드</summary>
    public UserGroupEditForm(UserGroupListItemDto? existing)
    {
        _isEditMode = existing != null;
        _originalUserGrpCd = existing?.UserGrpCd;

        InitializeComponent();

        Text = _isEditMode ? "사용자그룹 수정" : "사용자그룹 등록";
        Height = _isEditMode ? 640 : 420;
        titleLabel.Text = _isEditMode ? "사용자그룹 정보 수정" : "신규 사용자그룹 등록";
        memberGroupPanel.Visible = _isEditMode;

        if (!_isEditMode) txtUserGrpCd.MarkRequired();

        if (_isEditMode && existing != null)
        {
            txtUserGrpCd.Text = existing.UserGrpCd;
            txtUserGrpCd.Enabled = false; // 그룹코드는 PK라 수정 불가
            txtUserGrpNm.Text = existing.UserGrpNm;
            txtDescription.Text = existing.Description;
            spnSortOrder.Value = existing.SortOrder;
            chkUseYn.Checked = existing.UseYn;
        }
        else
        {
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨
        }

        PositionFooterButtons();
    }

    private async void UserGroupEditForm_Load(object? sender, EventArgs e)
    {
        if (!_isEditMode) return;
        await LoadMembersAsync();
    }

    private async Task LoadMembersAsync()
    {
        _members = await ApiClient.GetAsync<List<UserGroupMemberDto>>($"api/user-groups/{_originalUserGrpCd}/members") ?? new();
        memberGrid.DataSource = _members;
    }

    private void footerPanel_Resize(object? sender, EventArgs e) => PositionFooterButtons();

    private void PositionFooterButtons()
    {
        btnCancel.Location = new Point(footerPanel.Width - btnCancel.Width - 16, 12);
        btnSave.Location = new Point(btnCancel.Left - btnSave.Width - 8, 12);
    }

    private async void btnSave_Click(object? sender, EventArgs e) => await SaveAsync();

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(txtUserGrpCd.Text) || string.IsNullOrWhiteSpace(txtUserGrpNm.Text))
        {
            AppMessageBox.Show("그룹코드와 그룹명은 필수입니다.", "확인");
            return;
        }

        btnSave.Enabled = false;
        try
        {
            ApiResult? result;

            if (_isEditMode)
            {
                var req = new UserGroupUpdateRequest
                {
                    UserGrpNm = txtUserGrpNm.Text,
                    Description = txtDescription.Text,
                    SortOrder = (int)spnSortOrder.Value,
                    UseYn = chkUseYn.Checked
                };
                result = await ApiClient.PutAsync<UserGroupUpdateRequest, ApiResult>($"api/user-groups/{_originalUserGrpCd}", req);

                if (result != null && result.Success)
                {
                    memberGridView.CloseEditor();
                    memberGridView.UpdateCurrentRow();
                    var checkedUserIds = _members.Where(m => m.IsMember).Select(m => m.UserId).ToList();
                    var memberResult = await ApiClient.PutAsync<UpdateGroupMembersRequest, ApiResult>(
                        $"api/user-groups/{_originalUserGrpCd}/members", new UpdateGroupMembersRequest { UserIds = checkedUserIds });

                    if (memberResult == null || !memberResult.Success)
                    {
                        AppMessageBox.Show(memberResult?.Message ?? "소속 배정 저장에 실패했습니다.", "저장 실패");
                        return;
                    }
                }
            }
            else
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

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }
}
