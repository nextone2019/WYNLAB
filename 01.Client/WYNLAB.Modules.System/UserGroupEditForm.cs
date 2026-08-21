using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Modules.System;

/// <summary>
/// 사용자그룹 신규등록/수정 팝업. UserEditForm/MenuEditForm과 같은 LayoutControl 패턴을 쓰되,
/// 수정모드에서는 그 아래에 "소속 사용자 배정" 그리드를 추가로 둔다 - 전체 사용자를 체크그리드로
/// 보여주고, 체크된 사용자 목록을 저장 시점에 한 번에 치환(PUT .../members) 한다.
/// 신규등록 모드에서는 그룹코드(PK)가 아직 없어 배정 그리드를 보여주지 않는다 -
/// 먼저 그룹을 만들고 나서 다시 들어와 배정하는 흐름.
/// </summary>
public class UserGroupEditForm : XtraForm
{
    private readonly bool _isEditMode;
    private readonly string? _originalUserGrpCd;

    private readonly TextEdit txtUserGrpCd = new();
    private readonly TextEdit txtUserGrpNm = new();
    private readonly MemoEdit txtDescription = new();
    private readonly SpinEdit spnSortOrder = new() { Properties = { MinValue = 0, MaxValue = 9999 } };
    private readonly CheckEdit chkUseYn = new() { Text = "사용" };

    private readonly LayoutControl layoutControl = new() { Dock = DockStyle.Fill, Padding = new Padding(12) };
    private readonly Panel headerPanel = new() { Dock = DockStyle.Top, Height = 46 };
    private readonly Panel footerPanel = new() { Dock = DockStyle.Bottom, Height = 56 };
    private readonly SimpleButton btnSave = new() { Text = "저장" };
    private readonly SimpleButton btnCancel = new() { Text = "취소", DialogResult = DialogResult.Cancel };

    private readonly GridControl memberGrid = new();
    private readonly GridView memberGridView = new();
    private List<UserGroupMemberDto> _members = new();

    /// <summary>신규등록 모드</summary>
    public UserGroupEditForm() : this(null) { }

    /// <summary>수정모드</summary>
    public UserGroupEditForm(UserGroupListItemDto? existing)
    {
        _isEditMode = existing != null;
        _originalUserGrpCd = existing?.UserGrpCd;

        Text = _isEditMode ? "사용자그룹 수정" : "사용자그룹 등록";
        StartPosition = FormStartPosition.CenterParent;
        Width = 560;
        Height = _isEditMode ? 640 : 420;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        BuildLayout();
        if (_isEditMode) BuildMemberGrid();
        BuildHeader();
        BuildFooter();

        if (_isEditMode && existing != null)
        {
            txtUserGrpCd.Text = existing.UserGrpCd;
            txtUserGrpCd.Enabled = false; // 그룹코드는 PK라 수정 불가
            txtUserGrpNm.Text = existing.UserGrpNm;
            txtDescription.Text = existing.Description;
            spnSortOrder.Value = existing.SortOrder;
            chkUseYn.Checked = existing.UseYn;

            Load += async (s, e) => await LoadMembersAsync();
        }
        else
        {
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨
        }
    }

    private void BuildHeader()
    {
        headerPanel.BackColor = Color.FromArgb(245, 246, 248);
        var titleLabel = new LabelControl
        {
            Text = _isEditMode ? "사용자그룹 정보 수정" : "신규 사용자그룹 등록",
            Location = new Point(16, 12),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(340, 22)
        };
        titleLabel.Appearance.Font = AppFonts.SubHeading;
        headerPanel.Controls.Add(titleLabel);
        Controls.Add(headerPanel);
    }

    private void BuildLayout()
    {
        Controls.Add(layoutControl);
        layoutControl.BeginUpdate();

        var root = layoutControl.Root;
        root.TextVisible = false;
        root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);

        var groupBasic = root.AddGroup("기본정보").StyleAsSection();
        groupBasic.AddItem("그룹코드", txtUserGrpCd).MarkRequired();
        groupBasic.AddItem("그룹명", txtUserGrpNm).MarkRequired();
        var descItem = groupBasic.AddItem("설명", txtDescription);
        descItem.Control.Height = 50;
        groupBasic.AddItem("정렬순서", spnSortOrder);

        var groupStatus = root.AddGroup("상태").StyleAsSection();
        var itemUseYn = groupStatus.AddItem(string.Empty, chkUseYn);
        itemUseYn.TextVisible = false;

        layoutControl.EndUpdate();

        if (!_isEditMode) txtUserGrpCd.MarkRequired();
        txtUserGrpNm.MarkRequired();
    }

    /// <summary>전체 사용자 + 소속여부(IsMember) 체크그리드. LayoutControl 영역 아래(Dock=Fill) 자리잡는다.
    /// LayoutControl이 먼저 Dock=Fill로 추가되어 있으므로, 이 패널을 나중에 Dock=Bottom으로 추가하면
    /// 위쪽엔 기본정보 폼, 아래쪽엔 배정그리드로 자연스럽게 나뉜다.</summary>
    private void BuildMemberGrid()
    {
        var groupPanel = new Panel { Dock = DockStyle.Bottom, Height = 260 };

        var caption = new LabelControl
        {
            Text = "소속 사용자 배정 (체크 후 저장)",
            Dock = DockStyle.Top,
            Height = 24,
            Padding = new Padding(12, 6, 0, 0)
        };
        caption.Appearance.Font = AppFonts.BodyBold;
        caption.Appearance.ForeColor = Color.FromArgb(70, 70, 70);

        memberGrid.MainView = memberGridView;
        memberGrid.Dock = DockStyle.Fill;
        memberGridView.OptionsView.ShowGroupPanel = false;
        memberGridView.OptionsView.ShowAutoFilterRow = true;
        memberGridView.OptionsBehavior.Editable = true;

        groupPanel.Controls.Add(memberGrid);
        groupPanel.Controls.Add(caption);
        Controls.Add(groupPanel);
    }

    private async Task LoadMembersAsync()
    {
        _members = await ApiClient.GetAsync<List<UserGroupMemberDto>>($"api/user-groups/{_originalUserGrpCd}/members") ?? new();
        memberGrid.DataSource = _members;

        memberGridView.Columns["UserId"].Caption = "아이디";
        memberGridView.Columns["UserNm"].Caption = "이름";
        memberGridView.Columns["DeptNm"].Caption = "부서";
        memberGridView.Columns["IsMember"].Caption = "소속";
        memberGridView.Columns["IsMember"].Width = 50;
        memberGridView.Columns["UserId"].OptionsColumn.AllowEdit = false;
        memberGridView.Columns["UserNm"].OptionsColumn.AllowEdit = false;
        memberGridView.Columns["DeptNm"].OptionsColumn.AllowEdit = false;
    }

    private void BuildFooter()
    {
        footerPanel.BackColor = Color.FromArgb(245, 246, 248);

        btnSave.Size = new Size(90, 32);
        btnSave.Appearance.BackColor = Color.FromArgb(37, 122, 201);
        btnSave.Appearance.ForeColor = Color.White;
        btnSave.Appearance.Options.UseBackColor = true;
        btnSave.Appearance.Options.UseForeColor = true;
        btnSave.Click += async (s, e) => await SaveAsync();

        btnCancel.Size = new Size(90, 32);

        footerPanel.Resize += (s, e) => PositionFooterButtons();
        footerPanel.Controls.Add(btnSave);
        footerPanel.Controls.Add(btnCancel);
        Controls.Add(footerPanel);

        CancelButton = btnCancel;
        PositionFooterButtons();
    }

    private void PositionFooterButtons()
    {
        btnCancel.Location = new Point(footerPanel.Width - btnCancel.Width - 16, 12);
        btnSave.Location = new Point(btnCancel.Left - btnSave.Width - 8, 12);
    }

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
