using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Screens.MenuManage;

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

    // 트리 폭은 사용자가 스플리터로 조절할 수 있게 하되(SplitContainerControl), 우측 입력
    // 필드들은 FixedControlWidth로 각자 고정폭이라 스플리터를 옮기거나 MDI 창을 리사이즈해도
    // 필드 자체의 크기/배치는 흔들리지 않는다 - 늘어나는/줄어드는 건 여백뿐이다.
    private readonly SplitContainerControl splitContainer = new() { Dock = DockStyle.Fill };
    private readonly Panel leftPanel = new() { Dock = DockStyle.Fill };
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

    // 화면 타이틀의 기본 폴더 아이콘 대신 "계층 구조를 관리한다"는 의미가 더 명확한 트리 아이콘을 사용.
    protected override Action<Graphics, Rectangle, Color> ScreenIconPainter => MenuIconPainters.MenuTree;

    public MenuListForm()
    {
        Text = "메뉴관리";
        MenuCd = "SM_MENU";

        cboMenuType.Properties.Items.AddRange(new[] { "GROUP", "FORM" });
        cboMenuType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

        BuildLeftPanel();
        BuildRightPanel();
        splitContainer.Panel1.Controls.Add(leftPanel);
        splitContainer.Panel2.Controls.Add(rightPanel);
        splitContainer.Panel1.MinSize = 220;
        splitContainer.Panel2.MinSize = 500; // 우측 필드 중 화면 클래스명(420px)이 가장 넓어서 그보다 여유있게
        splitContainer.SplitterPosition = 280;

        // Dock 추가 순서: Fill(splitContainer) 먼저, Top(타이틀바)은 나중에 추가해야 맨 위를 차지한다
        Controls.Add(splitContainer);
        Controls.Add(BuildScreenHeader());

        EnterNewMode(null);
        Load += async (s, e) => await QueryAsync();
    }

    private void BuildLeftPanel()
    {
        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.FromArgb(225, 225, 225) };

        // 버그 수정: Dock을 실제로 지정한 적이 없어서 트리가 기본 크기(작은 박스)로만 떠 있었다.
        menuTree.Dock = DockStyle.Fill;
        menuTree.KeyFieldName = "MenuCd";
        menuTree.ParentFieldName = "UpperMenuCd";
        menuTree.OptionsBehavior.Editable = false;
        menuTree.OptionsView.ShowIndicator = false;
        menuTree.OptionsView.ShowHorzLines = false;
        menuTree.OptionsView.ShowVertLines = false;
        // 컬럼 하나만 쓰고 패널 폭에 맞춰 자동으로 늘어나게 해서, 컬럼이 패널보다 넓어져
        // 처음부터(내용이 적을 때도) 가로 스크롤바가 생기는 일이 없도록 한다.
        menuTree.OptionsView.AutoWidth = true;
        menuTree.RowHeight = 26; // 기본 행높이는 다소 빡빡해 보여서 살짝 여유를 줌
        menuTree.Appearance.Row.Font = AppFonts.Body;
        colMenuNm.Visible = true;
        colMenuNm.VisibleIndex = 0;
        menuTree.Columns.Add(colMenuNm);
        menuTree.OptionsView.ShowColumns = false; // 컬럼이 하나뿐이라 헤더 자체가 불필요
        menuTree.FocusedNodeChanged += (s, e) => OnTreeSelectionChanged();

        // 그룹(폴더, 클릭해도 화면이 안 열림) 행과 실제 화면(leaf) 행을 배경/글자색으로 구분한다.
        // 색은 UiTheme(=appsettings.json Theme 섹션)에서 가져와서, 트리를 쓰는 다른 화면이
        // 늘어나도 전부 같은 톤을 재사용하게 한다.
        menuTree.NodeCellStyle += (s, e) =>
        {
            var menuType = e.Node.GetValue("MenuType") as string;
            var isGroup = menuType == "GROUP";
            e.Appearance.BackColor = isGroup ? UiTheme.TreeGroupBackColor : UiTheme.TreeLeafBackColor;
            e.Appearance.ForeColor = isGroup ? UiTheme.TreeGroupForeColor : UiTheme.TreeLeafForeColor;
            e.Appearance.Font = isGroup ? AppFonts.BodyBold : AppFonts.Body;
            e.Appearance.Options.UseBackColor = true;
            e.Appearance.Options.UseForeColor = true;
            e.Appearance.Options.UseFont = true;
        };

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

        // 입력 컨트롤 폰트를 앱 공통 스케일(AppFonts.Body)로 통일 - 지정하지 않으면
        // DevExpress 기본 폰트(Tahoma 8.25pt)로 표시되어 화면마다 크기가 들쭉날쭉해 보였다.
        foreach (var edit in new BaseEdit[] { txtMenuCd, txtMenuNm, txtUpperMenuCd, spnMenuLevel, cboMenuType, txtFormClassNm, txtIconNm, spnSortOrder, chkUseYn })
        {
            edit.Properties.Appearance.Font = AppFonts.Body;
            edit.Properties.Appearance.Options.UseFont = true;
        }

        // 각 필드에 입력될 값의 실제 길이를 고려한 고정폭 - LayoutControl 기본 동작(그룹 폭까지
        // 무조건 늘어남)을 끄고, 짧은 코드/콤보는 짧게, 긴 클래스명은 넉넉하게 잡는다.
        // 창 크기가 바뀌어도 이 폭은 그대로 유지된다(FixedControlWidth 참고).
        var groupBasic = root.AddGroup("기본정보").StyleAsSection();
        groupBasic.AddItem("메뉴코드", txtMenuCd).MarkRequired().FixedControlWidth(160);
        groupBasic.AddItem("메뉴명", txtMenuNm).MarkRequired().FixedControlWidth(220);
        groupBasic.AddItem("상위메뉴코드", txtUpperMenuCd).FixedControlWidth(160);
        groupBasic.AddItem("메뉴레벨", spnMenuLevel).FixedControlWidth(70);
        groupBasic.AddItem("메뉴유형", cboMenuType).MarkRequired().FixedControlWidth(130);

        var groupAdvanced = root.AddGroup("연결정보").StyleAsSection();
        groupAdvanced.AddItem("화면 클래스명", txtFormClassNm).FixedControlWidth(420);
        groupAdvanced.AddItem("아이콘명", txtIconNm).FixedControlWidth(150);
        groupAdvanced.AddItem("정렬순서", spnSortOrder).FixedControlWidth(70);

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
