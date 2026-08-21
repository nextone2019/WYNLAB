using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Modules.System;

/// <summary>
/// 메뉴관리 화면. 좌측에 등록된 메뉴 전체를 트리로 보여주고, 우측에서 바로 등록/수정한다
/// (별도 팝업 없음 - UserEditForm/UserGroupEditForm과 달리 이 화면은 계층 구조 자체가
/// 핵심이라 트리를 보면서 바로 옆에서 편집하는 게 훨씬 직관적).
///
/// 신규 등록 규칙:
/// - Shell 툴바 "입력"(NewAsync) -> 좌측 트리에서 현재 선택된 메뉴가 있으면 그 메뉴의
///   "하위 메뉴"로 신규 등록 (상위메뉴코드/메뉴레벨 자동 세팅, 직접 입력 불가)
/// - 우측 상단 "최상위 메뉴로 등록" 버튼 -> 트리 선택과 무관하게 항상 최상위(모듈)로 신규 등록
/// </summary>
public class MenuListForm : BaseForm
{
    private List<MenuListItemDto> _menus = new();

    private string? _editingMenuCd;   // null이면 신규모드, 값이 있으면 그 메뉴 수정모드
    private string? _lastSelectedCd;  // 트리에서 마지막으로 선택된 메뉴 (신규 시 상위메뉴 후보)
    private string? _newParentCd;     // 신규모드일 때 실제 적용될 상위메뉴코드 (null = 최상위)

    private readonly Panel leftPanel = new() { Dock = DockStyle.Left, Width = 300 };
    private readonly Panel rightPanel = new() { Dock = DockStyle.Fill };
    private readonly TreeList menuTree = new();
    private readonly TreeListColumn colMenuNm = new() { FieldName = "MenuNm", Caption = "메뉴명" };

    private readonly LabelControl lblFormTitle = new();
    private readonly LabelControl lblFormHint = new();
    private readonly SimpleButton btnNewTop = new() { Text = "최상위 메뉴로 등록" };

    private readonly TextEdit txtMenuCd = new();
    private readonly TextEdit txtMenuNm = new();
    private readonly TextEdit txtUpperMenuCd = new() { Properties = { ReadOnly = true } };
    private readonly SpinEdit spnMenuLevel = new() { Properties = { MinValue = 1, MaxValue = 5, ReadOnly = true } };
    private readonly ComboBoxEdit cboMenuType = new();
    private readonly TextEdit txtFormClassNm = new();
    private readonly TextEdit txtIconNm = new();
    private readonly SpinEdit spnSortOrder = new() { Properties = { MinValue = 0, MaxValue = 9999 } };
    private readonly CheckEdit chkUseYn = new() { Text = "사용" };

    private readonly LayoutControl layoutControl = new() { Padding = new Padding(16) };
    private readonly Panel formFooterPanel = new() { Dock = DockStyle.Bottom, Height = 56 };
    private readonly SimpleButton btnSaveInline = new() { Text = "저장" };
    private readonly SimpleButton btnCancelEdit = new() { Text = "취소" };

    public MenuListForm()
    {
        Text = "메뉴관리";
        MenuCd = "SM_MENU";

        cboMenuType.Properties.Items.AddRange(new[] { "GROUP", "FORM" });
        cboMenuType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

        BuildLeftPanel();
        BuildRightPanel();
        Controls.Add(rightPanel);
        Controls.Add(leftPanel);

        EnterNewMode(null);
        Load += async (s, e) => await QueryAsync();
    }

    private void BuildLeftPanel()
    {
        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.FromArgb(225, 225, 225) };

        menuTree.KeyFieldName = "MenuCd";
        menuTree.ParentFieldName = "UpperMenuCd";
        menuTree.OptionsBehavior.Editable = false;
        menuTree.OptionsView.ShowIndicator = false;
        menuTree.OptionsView.ShowHorzLines = false;
        menuTree.OptionsView.ShowVertLines = false;
        // 컬럼 하나만 쓰고 패널 폭에 맞춰 자동으로 늘어나게 해서, 컬럼이 패널보다 넓어져
        // 처음부터(내용이 적을 때도) 가로 스크롤바가 생기는 일이 없도록 한다.
        menuTree.OptionsView.AutoWidth = true;
        colMenuNm.Visible = true;
        colMenuNm.VisibleIndex = 0;
        menuTree.Columns.Add(colMenuNm);
        menuTree.OptionsView.ShowColumns = false; // 컬럼이 하나뿐이라 헤더 자체가 불필요
        menuTree.FocusedNodeChanged += (s, e) => OnTreeSelectionChanged();

        leftPanel.Controls.Add(menuTree); // Fill 먼저
        leftPanel.Controls.Add(divider);
    }

    private void BuildRightPanel()
    {
        var headerPanel = new Panel { Dock = DockStyle.Top, Height = 56, Padding = new Padding(16, 10, 16, 0) };
        lblFormTitle.Font = AppFonts.BodyBold;
        lblFormTitle.Location = new Point(0, 2);
        lblFormTitle.AutoSizeMode = LabelAutoSizeMode.None;
        lblFormTitle.Size = new Size(400, 20);
        headerPanel.Controls.Add(lblFormTitle);

        lblFormHint.Font = AppFonts.Caption;
        lblFormHint.Appearance.ForeColor = Color.FromArgb(140, 140, 140);
        lblFormHint.Location = new Point(0, 22);
        lblFormHint.AutoSizeMode = LabelAutoSizeMode.None;
        lblFormHint.Size = new Size(400, 18);
        headerPanel.Controls.Add(lblFormHint);

        btnNewTop.Size = new Size(140, 28);
        btnNewTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNewTop.Click += (s, e) => EnterNewMode(null, forceTop: true);
        headerPanel.Controls.Add(btnNewTop);
        headerPanel.Resize += (s, e) => btnNewTop.Location = new Point(headerPanel.Width - btnNewTop.Width - 16, 12);

        BuildLayout();

        // Dock 추가 순서: Fill(layoutControl) 먼저, Top(headerPanel)은 나중에 추가해야
        // 맨 위 가장자리를 정상적으로 차지한다.
        rightPanel.Controls.Add(layoutControl);
        rightPanel.Controls.Add(headerPanel);
        BuildFooter();
    }

    private void BuildLayout()
    {
        layoutControl.Dock = DockStyle.Fill;
        layoutControl.BeginUpdate();

        var root = layoutControl.Root;
        root.TextVisible = false;
        root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);

        var groupBasic = root.AddGroup("기본정보").StyleAsSection();
        groupBasic.AddItem("메뉴코드", txtMenuCd).MarkRequired();
        groupBasic.AddItem("메뉴명", txtMenuNm).MarkRequired();
        groupBasic.AddItem("상위메뉴코드", txtUpperMenuCd);
        groupBasic.AddItem("메뉴레벨", spnMenuLevel);
        groupBasic.AddItem("메뉴유형", cboMenuType).MarkRequired();

        var groupAdvanced = root.AddGroup("연결정보").StyleAsSection();
        groupAdvanced.AddItem("화면 클래스명", txtFormClassNm);
        groupAdvanced.AddItem("아이콘명", txtIconNm);
        groupAdvanced.AddItem("정렬순서", spnSortOrder);

        var groupStatus = root.AddGroup("상태").StyleAsSection();
        var itemUseYn = groupStatus.AddItem(string.Empty, chkUseYn);
        itemUseYn.TextVisible = false;

        layoutControl.EndUpdate();
        txtMenuNm.MarkRequired();
    }

    private void BuildFooter()
    {
        formFooterPanel.BackColor = Color.FromArgb(245, 246, 248);
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 231, 234) };
        formFooterPanel.Controls.Add(topBorder);

        btnSaveInline.Size = new Size(90, 32);
        btnSaveInline.Appearance.BackColor = Color.FromArgb(37, 122, 201);
        btnSaveInline.Appearance.ForeColor = Color.White;
        btnSaveInline.Appearance.Options.UseBackColor = true;
        btnSaveInline.Appearance.Options.UseForeColor = true;
        btnSaveInline.Click += async (s, e) => await SaveAsync();

        btnCancelEdit.Size = new Size(90, 32);
        btnCancelEdit.Click += (s, e) => EnterNewMode(_lastSelectedCd);

        formFooterPanel.Resize += (s, e) => PositionFooterButtons();
        formFooterPanel.Controls.Add(btnSaveInline);
        formFooterPanel.Controls.Add(btnCancelEdit);
        rightPanel.Controls.Add(formFooterPanel);
        PositionFooterButtons();
    }

    private void PositionFooterButtons()
    {
        btnCancelEdit.Location = new Point(formFooterPanel.Width - btnCancelEdit.Width - 16, 12);
        btnSaveInline.Location = new Point(btnCancelEdit.Left - btnSaveInline.Width - 8, 12);
    }

    public override async Task QueryAsync()
    {
        _menus = await ApiClient.GetAsync<List<MenuListItemDto>>("api/menus") ?? new();
        menuTree.DataSource = _menus;
        menuTree.ExpandAll();

        // 편집 중이던 메뉴가 여전히 존재하면 선택 유지, 없어졌으면(삭제됨) 신규모드로 복귀
        if (_editingMenuCd != null && _menus.All(m => m.MenuCd != _editingMenuCd))
        {
            EnterNewMode(null);
        }
    }

    public override Task NewAsync()
    {
        EnterNewMode(_lastSelectedCd);
        return Task.CompletedTask;
    }

    public override async Task DeleteAsync()
    {
        if (_editingMenuCd == null)
        {
            AppMessageBox.Show("삭제할 메뉴를 왼쪽 트리에서 선택해주세요.", "안내");
            return;
        }

        var menu = _menus.FirstOrDefault(m => m.MenuCd == _editingMenuCd);
        var confirm = AppMessageBox.Show(
            $"'{menu?.MenuNm}({_editingMenuCd})' 메뉴를 사용중지 처리하시겠습니까?\n하위 메뉴가 있다면 좌측 메뉴트리에서 같이 사라집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/menus/{_editingMenuCd}");
        await QueryAsync();
    }

    private void OnTreeSelectionChanged()
    {
        var node = menuTree.FocusedNode;
        if (node == null) return;

        var menuCd = (string)node.GetValue("MenuCd");
        var menu = _menus.FirstOrDefault(m => m.MenuCd == menuCd);
        if (menu == null) return;

        _lastSelectedCd = menuCd;
        EnterEditMode(menu);
    }

    /// <summary>
    /// 신규모드 진입. parentCd가 있으면 그 메뉴의 하위로, forceTop이면(또는 parentCd가 null이면)
    /// 트리 선택과 무관하게 최상위(모듈)로 등록되도록 상위메뉴코드/메뉴레벨을 자동 세팅한다.
    /// </summary>
    private void EnterNewMode(string? parentCd, bool forceTop = false)
    {
        _editingMenuCd = null;
        _newParentCd = forceTop ? null : parentCd;

        var parent = _newParentCd != null ? _menus.FirstOrDefault(m => m.MenuCd == _newParentCd) : null;

        txtMenuCd.Text = string.Empty;
        txtMenuCd.Enabled = true;
        txtMenuNm.Text = string.Empty;
        txtUpperMenuCd.Text = parent?.MenuCd ?? string.Empty;
        spnMenuLevel.Value = (parent?.MenuLevel ?? 0) + 1;
        cboMenuType.SelectedItem = "FORM";
        txtFormClassNm.Text = string.Empty;
        txtIconNm.Text = string.Empty;
        spnSortOrder.Value = 0;
        chkUseYn.Checked = true;
        chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)

        lblFormTitle.Text = parent != null ? "신규 메뉴 등록 (하위 메뉴)" : "신규 메뉴 등록 (최상위 모듈)";
        lblFormHint.Text = parent != null
            ? $"'{parent.MenuNm}' 메뉴의 하위로 등록됩니다."
            : "최상위 메뉴(모듈)로 등록됩니다. 좌측 트리 최상단에 새 가지가 생깁니다.";

        txtMenuNm.Focus();
    }

    private void EnterEditMode(MenuListItemDto menu)
    {
        _editingMenuCd = menu.MenuCd;
        _newParentCd = null;

        var parent = menu.UpperMenuCd != null ? _menus.FirstOrDefault(m => m.MenuCd == menu.UpperMenuCd) : null;

        txtMenuCd.Text = menu.MenuCd;
        txtMenuCd.Enabled = false; // 메뉴코드는 PK라 수정 불가
        txtMenuNm.Text = menu.MenuNm;
        txtUpperMenuCd.Text = menu.UpperMenuCd ?? string.Empty;
        spnMenuLevel.Value = menu.MenuLevel;
        cboMenuType.SelectedItem = menu.MenuType;
        txtFormClassNm.Text = menu.FormClassNm;
        txtIconNm.Text = menu.IconNm;
        spnSortOrder.Value = menu.SortOrder;
        chkUseYn.Checked = menu.UseYn;
        chkUseYn.Enabled = true;

        lblFormTitle.Text = "메뉴 수정";
        lblFormHint.Text = parent != null ? $"상위 메뉴: {parent.MenuNm}" : "최상위 메뉴(모듈)입니다.";
    }

    public override async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(txtMenuCd.Text) || string.IsNullOrWhiteSpace(txtMenuNm.Text))
        {
            AppMessageBox.Show("메뉴코드와 메뉴명은 필수입니다.", "확인");
            return;
        }

        btnSaveInline.Enabled = false;
        try
        {
            ApiResult? result;
            string savedMenuCd;

            if (_editingMenuCd != null)
            {
                savedMenuCd = _editingMenuCd;
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
                result = await ApiClient.PutAsync<MenuUpdateRequest, ApiResult>($"api/menus/{_editingMenuCd}", req);
            }
            else
            {
                savedMenuCd = txtMenuCd.Text;
                var req = new MenuCreateRequest
                {
                    MenuCd = txtMenuCd.Text,
                    MenuNm = txtMenuNm.Text,
                    UpperMenuCd = _newParentCd,
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
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }

            await QueryAsync();

            var savedNode = menuTree.FindNodeByKeyID(savedMenuCd);
            if (savedNode != null) menuTree.FocusedNode = savedNode;
        }
        finally
        {
            btnSaveInline.Enabled = true;
        }
    }
}
