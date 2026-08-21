using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Modules.System;

/// <summary>
/// 사용자 신규등록/수정 팝업.
/// LayoutControl로 "계정정보 / 인적사항 / 연락처 / 권한" 4개 그룹으로 항목을 묶어서
/// 라벨-입력창 정렬이 자동으로 맞춰지도록 구성했다 (기존의 Location 수동배치 방식보다
/// 훨씬 정돈된 결과가 나오고, 항목이 늘어나도 레이아웃이 안 깨진다).
/// 수정모드에서는 하단에 "소속 그룹 배정" 체크그리드를 추가로 둔다 - UserGroupEditForm의
/// 소속 사용자 배정 그리드와 반대 방향(그룹 입장이 아닌 사용자 입장)으로 같은 TSMUSERGRPMAP을 편집.
/// </summary>
public class UserEditForm : XtraForm
{
    private readonly bool _isEditMode;
    private readonly string? _originalUserId;

    private readonly TextEdit txtUserId = new();
    private readonly TextEdit txtUserNm = new();
    private readonly TextEdit txtPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly TextEdit txtEmpNo = new();
    private readonly TextEdit txtDeptCd = new();
    private readonly TextEdit txtPositionNm = new();
    private readonly TextEdit txtEmail = new();
    private readonly TextEdit txtMobileNo = new();
    private readonly CheckEdit chkIsAdmin = new() { Text = "시스템관리자 권한 부여" };
    private readonly CheckEdit chkUseYn = new() { Text = "사용" };

    private readonly LayoutControl layoutControl = new() { Dock = DockStyle.Fill, Padding = new Padding(12) };
    private readonly Panel headerPanel = new() { Dock = DockStyle.Top, Height = 46 };
    private readonly Panel footerPanel = new() { Dock = DockStyle.Bottom, Height = 56 };
    private readonly SimpleButton btnSave = new() { Text = "저장" };
    private readonly SimpleButton btnCancel = new() { Text = "취소", DialogResult = DialogResult.Cancel };

    private readonly GridControl groupGrid = new();
    private readonly GridView groupGridView = new();
    private List<UserGroupAssignDto> _groups = new();

    /// <summary>신규등록 모드</summary>
    public UserEditForm() : this(null) { }

    /// <summary>수정모드 - 기존 데이터로 폼을 채운다</summary>
    public UserEditForm(UserListItemDto? existing)
    {
        _isEditMode = existing != null;
        _originalUserId = existing?.UserId;

        Text = _isEditMode ? "사용자 수정" : "사용자 등록";
        StartPosition = FormStartPosition.CenterParent;
        Width = 480;
        Height = _isEditMode ? 700 : 580;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        BuildLayout();
        if (_isEditMode) BuildGroupGrid();
        BuildHeader();
        BuildFooter();

        if (_isEditMode && existing != null)
        {
            txtUserId.Text = existing.UserId;
            txtUserId.Enabled = false; // 아이디는 수정 불가
            txtPassword.Enabled = false; // 비밀번호 변경은 별도 기능으로 분리 예정
            txtUserNm.Text = existing.UserNm;
            txtEmpNo.Text = existing.EmpNo;
            txtDeptCd.Text = existing.DeptCd;
            txtPositionNm.Text = existing.PositionNm;
            txtEmail.Text = existing.Email;
            txtMobileNo.Text = existing.MobileNo;
            chkIsAdmin.Checked = existing.IsAdminYn;
            chkUseYn.Checked = existing.UseYn;

            Load += async (s, e) => await LoadGroupsAsync();
        }
        else
        {
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)
        }
    }

    /// <summary>이 사용자가 속할 그룹 배정 그리드 - LayoutControl(Dock=Fill) 아래쪽에 자리잡는다</summary>
    private void BuildGroupGrid()
    {
        var groupPanel = new Panel { Dock = DockStyle.Bottom, Height = 220 };

        var caption = new LabelControl
        {
            Text = "소속 그룹 배정 (체크 후 저장)",
            Dock = DockStyle.Top,
            Height = 24,
            Padding = new Padding(12, 6, 0, 0)
        };
        caption.Appearance.Font = AppFonts.BodyBold;
        caption.Appearance.ForeColor = Color.FromArgb(70, 70, 70);

        groupGrid.MainView = groupGridView;
        groupGrid.Dock = DockStyle.Fill;
        groupGridView.OptionsView.ShowGroupPanel = false;
        groupGridView.OptionsBehavior.Editable = true;

        groupPanel.Controls.Add(groupGrid);
        groupPanel.Controls.Add(caption);
        Controls.Add(groupPanel);
    }

    private async Task LoadGroupsAsync()
    {
        _groups = await ApiClient.GetAsync<List<UserGroupAssignDto>>($"api/users/{_originalUserId}/groups") ?? new();
        groupGrid.DataSource = _groups;

        groupGridView.Columns["UserGrpCd"].Caption = "그룹코드";
        groupGridView.Columns["UserGrpNm"].Caption = "그룹명";
        groupGridView.Columns["IsMember"].Caption = "소속";
        groupGridView.Columns["IsMember"].Width = 50;
        groupGridView.Columns["UserGrpCd"].OptionsColumn.AllowEdit = false;
        groupGridView.Columns["UserGrpNm"].OptionsColumn.AllowEdit = false;
    }

    private void BuildHeader()
    {
        headerPanel.BackColor = Color.FromArgb(245, 246, 248);
        var titleLabel = new LabelControl
        {
            Text = _isEditMode ? "사용자 정보 수정" : "신규 사용자 등록",
            Location = new Point(16, 12),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(300, 22)
        };
        titleLabel.Appearance.Font = new Font("Segoe UI", 11, FontStyle.Bold);
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

        var groupAccount = root.AddGroup("계정정보");
        groupAccount.AddItem("아이디", txtUserId).MarkRequired();
        var pwItem = groupAccount.AddItem("비밀번호", txtPassword);
        if (!_isEditMode) pwItem.MarkRequired();

        var groupPersonal = root.AddGroup("인적사항");
        groupPersonal.AddItem("이름", txtUserNm).MarkRequired();
        groupPersonal.AddItem("사번", txtEmpNo);
        groupPersonal.AddItem("부서코드", txtDeptCd);
        groupPersonal.AddItem("직급", txtPositionNm);

        if (!_isEditMode)
        {
            txtUserId.MarkRequired();
            txtUserNm.MarkRequired();
            txtPassword.MarkRequired();
        }
        else
        {
            txtUserNm.MarkRequired();
        }

        var groupContact = root.AddGroup("연락처");
        groupContact.AddItem("이메일", txtEmail);
        groupContact.AddItem("휴대폰", txtMobileNo);

        var groupAuth = root.AddGroup("권한");
        var itemAdmin = groupAuth.AddItem(string.Empty, chkIsAdmin);
        itemAdmin.TextVisible = false;
        var itemUseYn = groupAuth.AddItem(string.Empty, chkUseYn);
        itemUseYn.TextVisible = false;

        layoutControl.EndUpdate();
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
        if (string.IsNullOrWhiteSpace(txtUserId.Text) || string.IsNullOrWhiteSpace(txtUserNm.Text))
        {
            XtraMessageBox.Show("아이디와 이름은 필수입니다.", "확인");
            return;
        }
        if (!_isEditMode && string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            XtraMessageBox.Show("비밀번호는 필수입니다.", "확인");
            return;
        }

        btnSave.Enabled = false;
        try
        {
            ApiResult? result;

            if (_isEditMode)
            {
                var req = new UserUpdateRequest
                {
                    UserNm = txtUserNm.Text,
                    EmpNo = txtEmpNo.Text,
                    DeptCd = txtDeptCd.Text,
                    PositionNm = txtPositionNm.Text,
                    Email = txtEmail.Text,
                    MobileNo = txtMobileNo.Text,
                    UseYn = chkUseYn.Checked,
                    IsAdminYn = chkIsAdmin.Checked
                };
                result = await ApiClient.PutAsync<UserUpdateRequest, ApiResult>($"api/users/{_originalUserId}", req);

                if (result != null && result.Success)
                {
                    groupGridView.CloseEditor();
                    groupGridView.UpdateCurrentRow();
                    var checkedGrpCds = _groups.Where(g => g.IsMember).Select(g => g.UserGrpCd).ToList();
                    var groupResult = await ApiClient.PutAsync<UpdateUserGroupsRequest, ApiResult>(
                        $"api/users/{_originalUserId}/groups", new UpdateUserGroupsRequest { UserGrpCds = checkedGrpCds });

                    if (groupResult == null || !groupResult.Success)
                    {
                        XtraMessageBox.Show(groupResult?.Message ?? "소속 그룹 저장에 실패했습니다.", "저장 실패");
                        return;
                    }
                }
            }
            else
            {
                var req = new UserCreateRequest
                {
                    UserId = txtUserId.Text,
                    UserNm = txtUserNm.Text,
                    Password = txtPassword.Text,
                    EmpNo = txtEmpNo.Text,
                    DeptCd = txtDeptCd.Text,
                    PositionNm = txtPositionNm.Text,
                    Email = txtEmail.Text,
                    MobileNo = txtMobileNo.Text,
                    IsAdminYn = chkIsAdmin.Checked
                };
                result = await ApiClient.PostAsync<UserCreateRequest, ApiResult>("api/users", req);
            }

            if (result == null || !result.Success)
            {
                XtraMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
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
