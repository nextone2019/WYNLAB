using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using NEXTFramework.Shared.Dtos;
using NEXTFramework.UI.Common;
using System.Drawing;

namespace NEXTFramework.Modules.System;

/// <summary>
/// 메뉴 신규등록/수정 팝업. UserEditForm과 동일한 LayoutControl + MarkRequired() 패턴을
/// 그대로 재사용 - 앞으로 만들 모든 등록/수정 팝업이 이 두 화면의 구조를 표준으로 따라가면 된다.
/// </summary>
public class MenuEditForm : XtraForm
{
    private readonly bool _isEditMode;
    private readonly string? _originalMenuCd;

    private readonly TextEdit txtMenuCd = new();
    private readonly TextEdit txtMenuNm = new();
    private readonly TextEdit txtUpperMenuCd = new();
    private readonly SpinEdit spnMenuLevel = new() { Properties = { MinValue = 1, MaxValue = 5 } };
    private readonly ComboBoxEdit cboMenuType = new();
    private readonly TextEdit txtFormClassNm = new();
    private readonly TextEdit txtIconNm = new();
    private readonly SpinEdit spnSortOrder = new() { Properties = { MinValue = 0, MaxValue = 9999 } };
    private readonly CheckEdit chkUseYn = new() { Text = "사용" };

    private readonly LayoutControl layoutControl = new() { Dock = DockStyle.Fill, Padding = new Padding(12) };
    private readonly Panel headerPanel = new() { Dock = DockStyle.Top, Height = 46 };
    private readonly Panel footerPanel = new() { Dock = DockStyle.Bottom, Height = 56 };
    private readonly SimpleButton btnSave = new() { Text = "저장" };
    private readonly SimpleButton btnCancel = new() { Text = "취소", DialogResult = DialogResult.Cancel };

    /// <summary>신규등록 모드</summary>
    public MenuEditForm() : this(null) { }

    /// <summary>수정모드</summary>
    public MenuEditForm(MenuListItemDto? existing)
    {
        _isEditMode = existing != null;
        _originalMenuCd = existing?.MenuCd;

        Text = _isEditMode ? "메뉴 수정" : "메뉴 등록";
        StartPosition = FormStartPosition.CenterParent;
        Width = 480;
        Height = 560;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        cboMenuType.Properties.Items.AddRange(new[] { "GROUP", "FORM" });
        cboMenuType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

        BuildLayout();
        BuildHeader();
        BuildFooter();

        if (_isEditMode && existing != null)
        {
            txtMenuCd.Text = existing.MenuCd;
            txtMenuCd.Enabled = false; // 메뉴코드는 PK라 수정 불가
            txtMenuNm.Text = existing.MenuNm;
            txtUpperMenuCd.Text = existing.UpperMenuCd;
            spnMenuLevel.Value = existing.MenuLevel;
            cboMenuType.SelectedItem = existing.MenuType;
            txtFormClassNm.Text = existing.FormClassNm;
            txtIconNm.Text = existing.IconNm;
            spnSortOrder.Value = existing.SortOrder;
            chkUseYn.Checked = existing.UseYn;
        }
        else
        {
            cboMenuType.SelectedItem = "FORM";
            spnMenuLevel.Value = 2;
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨
        }
    }

    private void BuildHeader()
    {
        headerPanel.BackColor = Color.FromArgb(245, 246, 248);
        var titleLabel = new LabelControl
        {
            Text = _isEditMode ? "메뉴 정보 수정" : "신규 메뉴 등록",
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

        var groupBasic = root.AddGroup("기본정보");
        groupBasic.AddItem("메뉴코드", txtMenuCd).MarkRequired();
        groupBasic.AddItem("메뉴명", txtMenuNm).MarkRequired();
        groupBasic.AddItem("상위메뉴코드", txtUpperMenuCd);
        groupBasic.AddItem("메뉴레벨", spnMenuLevel);
        groupBasic.AddItem("메뉴유형", cboMenuType).MarkRequired();

        var groupAdvanced = root.AddGroup("연결정보");
        groupAdvanced.AddItem("화면 클래스명", txtFormClassNm);
        groupAdvanced.AddItem("아이콘명", txtIconNm);
        groupAdvanced.AddItem("정렬순서", spnSortOrder);

        var groupStatus = root.AddGroup("상태");
        var itemUseYn = groupStatus.AddItem(string.Empty, chkUseYn);
        itemUseYn.TextVisible = false;

        layoutControl.EndUpdate();

        if (!_isEditMode) txtMenuCd.MarkRequired();
        txtMenuNm.MarkRequired();
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
        if (string.IsNullOrWhiteSpace(txtMenuCd.Text) || string.IsNullOrWhiteSpace(txtMenuNm.Text))
        {
            XtraMessageBox.Show("메뉴코드와 메뉴명은 필수입니다.", "확인");
            return;
        }

        btnSave.Enabled = false;
        try
        {
            ApiResult? result;

            if (_isEditMode)
            {
                var req = new MenuUpdateRequest
                {
                    MenuNm = txtMenuNm.Text,
                    UpperMenuCd = txtUpperMenuCd.Text,
                    MenuLevel = (int)spnMenuLevel.Value,
                    MenuType = (string)cboMenuType.SelectedItem,
                    FormClassNm = txtFormClassNm.Text,
                    IconNm = txtIconNm.Text,
                    SortOrder = (int)spnSortOrder.Value,
                    UseYn = chkUseYn.Checked
                };
                result = await ApiClient.PutAsync<MenuUpdateRequest, ApiResult>($"api/menus/{_originalMenuCd}", req);
            }
            else
            {
                var req = new MenuCreateRequest
                {
                    MenuCd = txtMenuCd.Text,
                    MenuNm = txtMenuNm.Text,
                    UpperMenuCd = txtUpperMenuCd.Text,
                    MenuLevel = (int)spnMenuLevel.Value,
                    MenuType = (string)cboMenuType.SelectedItem,
                    FormClassNm = txtFormClassNm.Text,
                    IconNm = txtIconNm.Text,
                    SortOrder = (int)spnSortOrder.Value
                };
                result = await ApiClient.PostAsync<MenuCreateRequest, ApiResult>("api/menus", req);
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
