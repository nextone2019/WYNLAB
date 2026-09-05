using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using System.Drawing;

namespace WYNLAB.SM.MENU;

/// <summary>
/// 메뉴관리 화면. 좌측에 등록된 메뉴 전체를 트리로 보여주고, 우측에서 바로 등록/수정한다
/// (별도 팝업 없음 - UserEditForm/UserGroupEditForm과 달리 이 화면은 계층 구조 자체가
/// 핵심이라 트리를 보면서 바로 옆에서 편집하는 게 훨씬 직관적).
///
/// 신규 등록 규칙:
/// - Shell 툴바 "입력"(NewClick) -> 좌측 트리에서 현재 선택된 메뉴가 있으면 그 메뉴의
///   "하위 메뉴"로 신규 등록 (상위메뉴ID/메뉴레벨 자동 세팅, 직접 입력 불가)
/// - 우측 상단 "최상위 메뉴로 등록" 버튼 -> 트리 선택과 무관하게 항상 최상위(모듈)로 신규 등록
///
/// MENU_ID는 BIGINT IDENTITY라 더는 사람이 입력하지 않는다(서버가 채번) - 메뉴코드 자기입력
/// 필드는 없어졌고, 대신 등록된 메뉴의 ID를 읽기전용으로만 보여준다.
/// </summary>
public class frmMenu : BaseForm
{
    private List<MenuListItemDto> _menus = new();

    private long? _editingMenuId;   // null이면 신규모드, 값이 있으면 그 메뉴 수정모드
    private long? _lastSelectedId;  // 트리에서 마지막으로 선택된 메뉴 (신규 시 상위메뉴 후보)

    // 트리 폭은 사용자가 스플리터로 조절할 수 있게 하되(SplitContainerControl), 우측 입력
    // 필드들은 FixedControlWidth로 각자 고정폭이라 스플리터를 옮기거나 MDI 창을 리사이즈해도
    // 필드 자체의 크기/배치는 흔들리지 않는다 - 늘어나는/줄어드는 건 여백뿐이다.
    private readonly SplitContainerControl splitContainer = new() { Dock = DockStyle.Fill };
    private readonly Panel leftPanel = new() { Dock = DockStyle.Fill };
    private readonly Panel rightPanel = new() { Dock = DockStyle.Fill };
    private readonly TreeList menuTree = new();
    private readonly TreeListColumn colMenuId = new() { FieldName = "MenuId", Caption = "메뉴ID" };
    private readonly TreeListColumn colMenuNm = new() { FieldName = "MenuNm", Caption = "메뉴명" };

    private readonly LabelControl lblFormTitle = new();
    private readonly LabelControl lblFormHint = new();
    private readonly SimpleButton btnNewTop = new() { Text = "최상위 메뉴로 등록" };

    // 메뉴ID는 서버 채번(IDENTITY) - 항상 읽기전용, 신규모드에선 저장 전까지 비어있다.
    private readonly TextEdit txtMenuId = new() { Properties = { ReadOnly = true } };
    private readonly TextEdit txtMenuNm = new();
    private readonly TextEdit txtUpperMenuId = new() { Properties = { ReadOnly = true } };

    // 상위메뉴를 이름으로 찾아서 고르는 팝업(P_MENU, 메뉴 자기참조 트리) - 고르면 MapField가
    // 위 txtUpperMenuId를 같이 채운다. 트리 클릭으로 자동 세팅되던 상위메뉴를 이제 이 필드로도
    // 바꿀 수 있으므로, txtUpperMenuId.EditValueChanged에서 메뉴레벨을 다시 계산해야 한다
    // (아래 생성자 참고) - 안 그러면 신규모드에서 트리로 잡힌 레벨이 팝업으로 부모를 바꾼
    // 뒤에도 안 바뀌어서 저장 시 실제 부모 레벨과 안 맞는 값이 들어간다.
    private readonly PopupLookupEditWyn txtUpperMenuNm = new();
    private readonly SpinEdit spnMenuLevel = new() { Properties = { MinValue = 1, MaxValue = 5, ReadOnly = true } };
    private readonly ComboBoxEdit cboMenuType = new();
    private readonly TextEdit txtModule = new();
    private readonly TextEdit txtScreenClassNm = new();
    private readonly TextEdit txtIconNm = new();

    // 범용 데이터 통로(api/data/*)가 이 메뉴에서 호출을 허용할 프로시저 접두사 - 비어있으면
    // 그 메뉴로는 api/data/* 저장/조회가 전부 거부된다(DataController.ValidateAsync ②단계).
    // 지금까지는 이 값을 UI에서 아예 편집할 방법이 없어서 마이그레이션 스크립트로만 채워졌었다
    // (2026-09-04 실제 발견 - AI Builder가 즉시등록한 메뉴는 전부 이게 비어서 저장이 막혔음).
    private readonly TextEdit txtProcPrefix = new();
    private readonly SpinEdit spnSortOrder = new() { Properties = { MinValue = 0, MaxValue = 9999 } };
    private readonly CheckEdit chkUseYn = new() { Text = "사용" };

    // AUTH01~10 캡션 - 이 메뉴에서 BaseForm.Auth[0..9]를 어떤 의미로 쓸지 여기서 한 번만 정의해두면
    // 사용자권한관리(frmUserAuth)에서 이 메뉴를 클릭했을 때 그대로 표시된다. 비워두면 그 화면에서
    // "Auth01"처럼 기본 표기로 대체됨 - 그래서 여기도 전부 선택 입력(필수 아님)이다.
    private readonly TextEdit[] txtAuthNm = Enumerable.Range(0, 10).Select(_ => new TextEdit()).ToArray();

    private readonly LayoutControl layoutControl = new() { Padding = new Padding(16) };
    private readonly Panel formFooterPanel = new() { Dock = DockStyle.Bottom, Height = 56 };
    private readonly SimpleButton btnSaveInline = new() { Text = "저장" };
    private readonly SimpleButton btnCancelEdit = new() { Text = "취소" };
    private readonly SimpleButton btnCopy = new() { Text = "복사" };

    // 화면 타이틀의 기본 폴더 아이콘 대신 "계층 구조를 관리한다"는 의미가 더 명확한 트리 아이콘을 사용.
    protected override Action<Graphics, Rectangle, Color> ScreenIconPainter => MenuIconPainters.MenuTree;

    public frmMenu()
    {
        Text = "메뉴관리";

        cboMenuType.Properties.Items.AddRange(new[] { "GROUP", "FORM" });
        cboMenuType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

        // 상위메뉴 팝업(멀티필드 모드) - MatchField가 이 컨트롤 자신이 대표하는 팝업 결과
        // 컬럼(menu_nm)을 가리키고, MapField가 나머지 컬럼(menu_id)을 txtUpperMenuId에 채운다.
        txtUpperMenuNm.LookupKey = "P_MENU";
        txtUpperMenuNm.MatchField = "menu_nm";
        txtUpperMenuNm.MapField("menu_id", txtUpperMenuId);

        // 상위메뉴ID가 어떤 경로로 바뀌든(트리 클릭 기반 신규모드 진입, 또는 방금 추가한
        // 팝업으로 직접 변경) 메뉴레벨을 항상 "그 부모의 레벨 + 1"로 다시 맞춘다 - 팝업으로
        // 부모를 바꿨는데 예전 레벨이 그대로 남으면 저장 시 실제 계층과 안 맞는 레벨이 들어간다.
        txtUpperMenuId.EditValueChanged += (s, e) =>
        {
            var parent = long.TryParse(txtUpperMenuId.Text, out var parentId)
                ? _menus.FirstOrDefault(m => m.MenuId == parentId)
                : null;
            spnMenuLevel.Value = (parent?.MenuLevel ?? 0) + 1;
        };

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. TSMMENU는 다른 BA 테이블과
        // 달리 컬럼명이 대문자라 그대로 맞춘다. txtUpperMenuNm은 PopupLookupEditWyn이라
        // "Popup : P_MENU"도 자동으로 같이 붙는다.
        txtMenuId.Tag = new BindingFieldTag("MENU_ID");
        txtMenuNm.Tag = new BindingFieldTag("MENU_NM");
        txtUpperMenuId.Tag = new BindingFieldTag("UPPER_MENU_ID");
        txtUpperMenuNm.Tag = new BindingFieldTag("UPPER_MENU_ID");
        spnMenuLevel.Tag = new BindingFieldTag("MENU_LEVEL");
        cboMenuType.Tag = new BindingFieldTag("MENU_TYPE");
        txtModule.Tag = new BindingFieldTag("MODULE");
        txtScreenClassNm.Tag = new BindingFieldTag("SCREEN_CLASS_NM");
        txtIconNm.Tag = new BindingFieldTag("ICON_NM");
        txtProcPrefix.Tag = new BindingFieldTag("PROC_PREFIX");
        spnSortOrder.Tag = new BindingFieldTag("SORT_ORDER");
        chkUseYn.Tag = new BindingFieldTag("USE_YN");
        for (var i = 0; i < txtAuthNm.Length; i++)
            txtAuthNm[i].Tag = new BindingFieldTag($"AUTH{(i + 1):00}_NM");

        BuildLeftPanel();
        BuildRightPanel();
        splitContainer.Panel1.Controls.Add(leftPanel);
        splitContainer.Panel2.Controls.Add(rightPanel);
        // 트리 컬럼 폭 합(메뉴ID 70 + 메뉴명 170 = 240)에 계층 들여쓰기(레벨이 깊어질수록
        // 늘어남)까지 감안해서 여유를 둔다.
        splitContainer.Panel1.MinSize = 260;
        splitContainer.Panel2.MinSize = 500; // 우측 필드 중 화면 클래스명(300px)이 가장 넓어서 그보다 여유있게
        splitContainer.SplitterPosition = 340;

        // Dock 추가 순서: Fill(splitContainer) 먼저, Top(타이틀바)은 나중에 추가해야 맨 위를 차지한다
        Controls.Add(splitContainer);
        Controls.Add(BuildScreenHeader());

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync) - 이 화면은 panData 대신 layoutControl에
        // 편집 컨트롤을 직접 담으므로 그걸 넘긴다. menuTree는 편집 불가(OptionsBehavior.Editable
        // = false)라 별도로 걸 것이 없다.
        TrackDirty(layoutControl);

        EnterNewMode(null);
        Load += async (s, e) => await QueryClick();
    }

    private void BuildLeftPanel()
    {
        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.FromArgb(225, 225, 225) };

        // 버그 수정: Dock을 실제로 지정한 적이 없어서 트리가 기본 크기(작은 박스)로만 떠 있었다.
        menuTree.Dock = DockStyle.Fill;
        menuTree.KeyFieldName = "MenuId";
        menuTree.ParentFieldName = "UpperMenuId";
        menuTree.OptionsBehavior.Editable = false;
        menuTree.OptionsView.ShowIndicator = false;
        menuTree.OptionsView.ShowHorzLines = false;
        menuTree.OptionsView.ShowVertLines = false;
        menuTree.RowHeight = 26; // 기본 행높이는 다소 빡빡해 보여서 살짝 여유를 줌
        menuTree.Appearance.Row.Font = AppFonts.Body;
        colMenuId.Visible = true;
        colMenuId.VisibleIndex = 0;
        colMenuId.Width = 70;
        colMenuNm.Visible = true;
        colMenuNm.VisibleIndex = 1;
        colMenuNm.Width = 170;
        menuTree.Columns.AddRange(new[] { colMenuId, colMenuNm });
        menuTree.OptionsView.ShowColumns = true; // 컬럼이 둘이라 헤더로 구분해준다(메뉴ID/메뉴명)
        // 두 컬럼 폭 합이 패널보다 좁아도(또는 스플리터로 더 늘려도) 남는/모자란 폭을 컬럼들이
        // 나눠 채우게 해서 가로 스크롤바가 생기지 않게 한다 - 컬럼이 하나였을 때도 같은 이유로 켰었다.
        menuTree.OptionsView.AutoWidth = true;
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
        foreach (var edit in new BaseEdit[] { txtMenuId, txtMenuNm, txtUpperMenuId, txtUpperMenuNm, spnMenuLevel, cboMenuType, txtModule, txtScreenClassNm, txtIconNm, txtProcPrefix, spnSortOrder, chkUseYn }.Concat(txtAuthNm))
        {
            edit.Properties.Appearance.Font = AppFonts.Body;
            edit.Properties.Appearance.Options.UseFont = true;
        }

        // 각 필드에 입력될 값의 실제 길이를 고려한 고정폭 - LayoutControl 기본 동작(그룹 폭까지
        // 무조건 늘어남)을 끄고, 짧은 코드/콤보는 짧게, 긴 클래스명은 넉넉하게 잡는다.
        // 창 크기가 바뀌어도 이 폭은 그대로 유지된다(FixedControlWidth 참고).
        var groupBasic = root.AddGroup("기본정보").StyleAsSection();
        groupBasic.AddItem("메뉴ID", txtMenuId).FixedControlWidth(100);
        groupBasic.AddItem("메뉴명", txtMenuNm).MarkRequired().FixedControlWidth(220);
        groupBasic.AddItem("상위메뉴ID", txtUpperMenuId).FixedControlWidth(100);
        groupBasic.AddItem("상위메뉴명", txtUpperMenuNm).FixedControlWidth(220);
        groupBasic.AddItem("메뉴레벨", spnMenuLevel).FixedControlWidth(70);
        groupBasic.AddItem("메뉴유형", cboMenuType).MarkRequired().FixedControlWidth(130);

        var groupAdvanced = root.AddGroup("연결정보").StyleAsSection();
        groupAdvanced.AddItem("모듈", txtModule).FixedControlWidth(120);
        groupAdvanced.AddItem("화면 클래스명", txtScreenClassNm).FixedControlWidth(300);
        groupAdvanced.AddItem("아이콘명", txtIconNm).FixedControlWidth(150);
        groupAdvanced.AddItem("Proc Prefix", txtProcPrefix).FixedControlWidth(220);
        groupAdvanced.AddItem("정렬순서", spnSortOrder).FixedControlWidth(70);

        var groupStatus = root.AddGroup("상태").StyleAsSection();
        var itemUseYn = groupStatus.AddItem(string.Empty, chkUseYn);
        itemUseYn.TextVisible = false;

        // AUTH01~10 캡션 - 사용자권한관리(frmUserAuth)의 AUTH01~10 패널에 그대로 표시될 텍스트.
        var groupAuthNm = root.AddGroup("추가권한 캡션(AUTH01~10)").StyleAsSection();
        for (var i = 0; i < txtAuthNm.Length; i++)
            groupAuthNm.AddItem($"AUTH{(i + 1):00}", txtAuthNm[i]).FixedControlWidth(160);

        layoutControl.EndUpdate();
        // 필드 수가 많아 세로로 길어질 수 있어 우측 패널 자체를 스크롤 가능하게 한다.
        rightPanel.AutoScroll = true;
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
        btnSaveInline.Click += async (s, e) => await SaveClick();

        btnCancelEdit.Size = new Size(90, 32);
        btnCancelEdit.Click += (s, e) => EnterNewMode(_lastSelectedId);

        // 복사(Save As) - 지금 선택된 메뉴가 있을 때만 의미가 있다(EnterEditMode/EnterNewMode에서
        // Enabled를 맞춘다). 저장은 하지 않고 신규입력 상태(메뉴ID만 비움)로만 만들어둔다 -
        // 나머지 값은 전부 그대로 복사돼 있으니 저장 누르면 새 ID로 채번된다.
        btnCopy.Size = new Size(90, 32);
        btnCopy.Enabled = false;
        btnCopy.Click += (s, e) => EnterCopyMode();

        formFooterPanel.Resize += (s, e) => PositionFooterButtons();
        formFooterPanel.Controls.Add(btnSaveInline);
        formFooterPanel.Controls.Add(btnCancelEdit);
        formFooterPanel.Controls.Add(btnCopy);
        rightPanel.Controls.Add(formFooterPanel);
        PositionFooterButtons();
    }

    private void PositionFooterButtons()
    {
        btnCancelEdit.Location = new Point(formFooterPanel.Width - btnCancelEdit.Width - 16, 12);
        btnSaveInline.Location = new Point(btnCancelEdit.Left - btnSaveInline.Width - 8, 12);
        btnCopy.Location = new Point(btnSaveInline.Left - btnCopy.Width - 8, 12);
    }

    public override async Task QueryClick()
    {
        _menus = await ApiClient.GetAsync<List<MenuListItemDto>>("api/menus") ?? new();
        menuTree.DataSource = _menus;
        menuTree.ExpandAll();

        // 편집 중이던 메뉴가 여전히 존재하면 선택 유지, 없어졌으면(삭제됨) 신규모드로 복귀
        if (_editingMenuId != null && _menus.All(m => m.MenuId != _editingMenuId))
        {
            EnterNewMode(null);
        }
    }

    public override Task NewClick()
    {
        EnterNewMode(_lastSelectedId);
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (_editingMenuId == null)
        {
            AppMessageBox.Show("삭제할 메뉴를 왼쪽 트리에서 선택해주세요.", "안내");
            return;
        }

        var menu = _menus.FirstOrDefault(m => m.MenuId == _editingMenuId);
        var confirm = AppMessageBox.Show(
            $"'{menu?.MenuNm}(ID:{_editingMenuId})' 메뉴를 사용중지 처리하시겠습니까?\n하위 메뉴가 있다면 좌측 메뉴트리에서 같이 사라집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/menus/{_editingMenuId}");
        await QueryClick();
        Toast.Show("사용중지 처리되었습니다.");
    }

    private void OnTreeSelectionChanged()
    {
        var node = menuTree.FocusedNode;
        if (node == null) return;

        var menuId = Convert.ToInt64(node.GetValue("MenuId"));
        var menu = _menus.FirstOrDefault(m => m.MenuId == menuId);
        if (menu == null) return;

        _lastSelectedId = menuId;
        EnterEditMode(menu);
    }

    /// <summary>
    /// 신규모드 진입. parentId가 있으면 그 메뉴의 하위로, forceTop이면(또는 parentId가 null이면)
    /// 트리 선택과 무관하게 최상위(모듈)로 등록되도록 상위메뉴ID/메뉴레벨을 자동 세팅한다.
    /// </summary>
    /// <summary>편집 컨트롤 값을 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안 그러면
    /// 코드가 값을 채우는 것뿐인데 TrackDirty(layoutControl)가 "사용자가 고쳤다"로 오인해서,
    /// 트리 선택/신규모드 진입 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode(long? parentId, bool forceTop = false)
    {
        var newParentId = forceTop ? null : parentId;
        var parent = newParentId != null ? _menus.FirstOrDefault(m => m.MenuId == newParentId) : null;

        SuppressDirtyTracking(() =>
        {
            _editingMenuId = null;

            txtMenuId.Text = string.Empty;
            txtMenuNm.Text = string.Empty;
            txtUpperMenuId.Text = parent?.MenuId.ToString() ?? string.Empty;
            txtUpperMenuNm.Text = parent?.MenuNm ?? string.Empty;
            spnMenuLevel.Value = (parent?.MenuLevel ?? 0) + 1;
            cboMenuType.SelectedItem = "FORM";
            txtModule.Text = string.Empty;
            txtScreenClassNm.Text = string.Empty;
            txtIconNm.Text = string.Empty;
            txtProcPrefix.Text = string.Empty;
            spnSortOrder.Value = 0;
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)
            foreach (var t in txtAuthNm) t.Text = string.Empty;
        });

        lblFormTitle.Text = parent != null ? "신규 메뉴 등록 (하위 메뉴)" : "신규 메뉴 등록 (최상위 모듈)";
        lblFormHint.Text = parent != null
            ? $"'{parent.MenuNm}' 메뉴의 하위로 등록됩니다."
            : "최상위 메뉴(모듈)로 등록됩니다. 좌측 트리 최상단에 새 가지가 생깁니다.";

        btnCopy.Enabled = false;
        txtMenuNm.Focus();
    }

    /// <summary>지금 우측 패널에 표시된 값(선택된 메뉴 한 건)을 그대로 둔 채 메뉴ID만 비우고
    /// 신규입력 상태로 전환한다 - 저장은 여기서 하지 않는다(사용자가 직접 저장 버튼을 눌러야
    /// 새 ID로 실제 등록됨). 비슷한 메뉴를 여러 개 등록할 때 매번 전체 필드를 다시 채울 필요
    /// 없이 몇 군데만 바꿔서 저장하면 되도록 하기 위함(사장님 요청).</summary>
    private void EnterCopyMode()
    {
        if (_editingMenuId == null) return; // btnCopy가 이 상태에선 비활성화라 보통 여기 안 옴

        var sourceId = _editingMenuId;
        var sourceNm = txtMenuNm.Text;

        SuppressDirtyTracking(() =>
        {
            _editingMenuId = null;
            txtMenuId.Text = string.Empty;
            // 메뉴명/상위메뉴/레벨/유형/모듈/화면클래스명/아이콘/정렬순서/AUTH01~10은 그대로 복사 -
            // 여기서 일부러 안 지운다.
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)
        });

        lblFormTitle.Text = "메뉴 복사 등록";
        lblFormHint.Text = $"'{sourceNm}(ID:{sourceId})'의 값을 그대로 복사했습니다. 확인 후 저장하면 새 ID로 등록됩니다.";

        btnCopy.Enabled = false;
        txtMenuNm.Focus();
    }

    private void EnterEditMode(MenuListItemDto menu)
    {
        var parent = menu.UpperMenuId != null ? _menus.FirstOrDefault(m => m.MenuId == menu.UpperMenuId) : null;

        SuppressDirtyTracking(() =>
        {
            _editingMenuId = menu.MenuId;

            txtMenuId.Text = menu.MenuId.ToString();
            txtMenuNm.Text = menu.MenuNm;
            txtUpperMenuId.Text = menu.UpperMenuId?.ToString() ?? string.Empty;
            txtUpperMenuNm.Text = parent?.MenuNm ?? string.Empty;
            spnMenuLevel.Value = menu.MenuLevel;
            cboMenuType.SelectedItem = menu.MenuType;
            txtModule.Text = menu.Module ?? string.Empty;
            txtScreenClassNm.Text = menu.ScreenClassNm ?? string.Empty;
            txtIconNm.Text = menu.IconNm;
            txtProcPrefix.Text = menu.ProcPrefix ?? string.Empty;
            spnSortOrder.Value = menu.SortOrder;
            chkUseYn.Checked = menu.UseYn;
            chkUseYn.Enabled = true;
            for (var i = 0; i < txtAuthNm.Length; i++)
                txtAuthNm[i].Text = i < menu.AuthNm.Length ? (menu.AuthNm[i] ?? string.Empty) : string.Empty;
        });

        lblFormTitle.Text = "메뉴 수정";
        lblFormHint.Text = parent != null ? $"상위 메뉴: {parent.MenuNm}" : "최상위 메뉴(모듈)입니다.";

        btnCopy.Enabled = true;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtMenuNm.Text))
        {
            AppMessageBox.Show("메뉴명은 필수입니다.", "확인");
            return;
        }

        // 모듈/화면 클래스명은 "WYNLAB.{Module}.{ScreenClassNm}, WYNLAB.{Module}" 형태로 조합되는
        // 손입력 텍스트라 오타가 나도 저장 시점엔 아무 에러가 안 나고, 나중에 그 메뉴를 클릭했을
        // 때서야 화면을 못 찾는다는 에러로 드러난다 - 저장 전에 미리 Type.GetType으로 실제
        // 존재하는 클래스인지 확인해서 막는다(ShellForm.OpenMenuForm과 같은 조합 방식).
        // Shell.exe가 시작할 때 ModuleLoader.LoadAll이 Modules\ 폴더의 모든 화면 dll을 이미
        // 로드해뒀고 AssemblyResolve 훅도 걸어놔서(ModuleLoader.cs 참고), 여기서 Type.GetType을
        // 호출하면 ShellForm이 실제 메뉴 클릭 시 찾는 것과 완전히 같은 경로로 찾아본다.
        var module = txtModule.Text.Trim();
        var screenClassNm = txtScreenClassNm.Text.Trim();
        if ((string)cboMenuType.SelectedItem == "FORM"
            && !string.IsNullOrWhiteSpace(module) && !string.IsNullOrWhiteSpace(screenClassNm)
            && Type.GetType($"WYNLAB.{module}.{screenClassNm}, WYNLAB.{module}") == null)
        {
            AppMessageBox.Show(
                $"화면 클래스를 찾을 수 없습니다: WYNLAB.{module}.{screenClassNm}, WYNLAB.{module}\n" +
                "모듈/화면 클래스명이 맞는지, 해당 화면 모듈(dll)이 배포되어 있는지 확인해주세요.",
                "확인");
            return;
        }

        btnSaveInline.Enabled = false;
        try
        {
            ApiResult? result;
            long savedMenuId;
            var wasNew = _editingMenuId == null;
            var authNm = txtAuthNm.Select(t => string.IsNullOrWhiteSpace(t.Text) ? null : t.Text).ToArray();
            var upperMenuId = long.TryParse(txtUpperMenuId.Text, out var parsedUpperId) ? parsedUpperId : (long?)null;

            if (_editingMenuId != null)
            {
                savedMenuId = _editingMenuId.Value;
                var req = new MenuUpdateRequest
                {
                    MenuNm = txtMenuNm.Text,
                    UpperMenuId = upperMenuId,
                    MenuLevel = (int)spnMenuLevel.Value,
                    MenuType = (string)cboMenuType.SelectedItem,
                    Module = module,
                    ScreenClassNm = screenClassNm,
                    IconNm = txtIconNm.Text,
                    ProcPrefix = txtProcPrefix.Text.Trim(),
                    SortOrder = (int)spnSortOrder.Value,
                    UseYn = chkUseYn.Checked,
                    AuthNm = authNm
                };
                result = await ApiClient.PutAsync<MenuUpdateRequest, ApiResult>($"api/menus/{_editingMenuId}", req);
            }
            else
            {
                var req = new MenuCreateRequest
                {
                    MenuNm = txtMenuNm.Text,
                    UpperMenuId = upperMenuId,
                    MenuLevel = (int)spnMenuLevel.Value,
                    MenuType = (string)cboMenuType.SelectedItem,
                    Module = module,
                    ScreenClassNm = screenClassNm,
                    IconNm = txtIconNm.Text,
                    ProcPrefix = txtProcPrefix.Text.Trim(),
                    SortOrder = (int)spnSortOrder.Value,
                    AuthNm = authNm
                };
                result = await ApiClient.PostAsync<MenuCreateRequest, ApiResult>("api/menus", req);
                savedMenuId = result != null && result.Success && long.TryParse(result.GeneratedCode, out var newId) ? newId : 0;
            }

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }

            await QueryClick();

            var savedNode = menuTree.FindNodeByKeyID(savedMenuId);
            if (savedNode != null) menuTree.FocusedNode = savedNode;

            Toast.Show(wasNew ? "메뉴가 등록되었습니다." : "수정되었습니다.");
        }
        finally
        {
            btnSaveInline.Enabled = true;
        }
    }
}
