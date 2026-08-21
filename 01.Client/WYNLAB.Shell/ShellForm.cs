using DevExpress.XtraBars.Navigation;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// MDI 메인 셸 (디자인: 화이트 리본 툴바 - Outlook/오피스 스타일).
///
/// 레이아웃 구조 (위→아래):
///  1. headerPanel  - 로고(클릭시 좌측메뉴 토글) + 조회/입력/저장 | 삭제 | 출력 툴바 + 서버전환 콤보 + 사용자정보
///  2. sidebarPanel - 좌측 메뉴(연한 회색 배경 + 우측 구분선으로 MDI 영역과 명확히 분리). 로고 클릭으로 표시/숨김
///  3. MDI 클라이언트 영역(흰색) - 업무화면들이 뜨는 공간
///
/// 헤더 배경은 흰색으로 고정하고, 회사별 커스터마이징은 로고 배지 색상 / 툴바 강조색에
/// appsettings.json의 ToolbarColor 값을 적용하는 방식으로 처리한다. 삭제 버튼은 항상 빨간색 고정
/// (브랜드 색과 무관하게 "위험한 동작"이라는 UX 관례를 지키기 위함).
/// </summary>
public class ShellForm : XtraForm
{
    private readonly Color _accentColor = ColorHelper.FromHex(AppConfig.ToolbarColor);
    private static readonly Color DangerColor = Color.FromArgb(192, 57, 43);
    private static readonly Color HeaderBg = Color.White;
    private static readonly Color SidebarBg = Color.FromArgb(245, 246, 248);
    private static readonly Color DividerColor = Color.FromArgb(225, 225, 225);

    private readonly Panel headerPanel;
    private readonly Panel logoPanel;
    private readonly Panel sidebarPanel;
    private readonly Panel statusBar;
    private readonly LabelControl lblStatusMessage = new();
    private readonly LabelControl lblStatusRight = new();
    private readonly ComboBoxEdit cboEnvironment = new();
    private readonly LabelControl lblUserInfo = new();

    private readonly AccordionControl accordionMenu = new() { Dock = DockStyle.Fill };
    private readonly XtraTabbedMdiManager tabbedMdiManager = new();
    private HomeForm? _homeForm;

    private readonly Dictionary<string, string> _envLabels = new()
    {
        ["Development"] = "개발서버",
        ["Production"] = "운영서버"
    };

    private bool _suppressEnvChange;

    public ShellForm()
    {
        IsMdiContainer = true;
        RefreshTitle();
        WindowState = FormWindowState.Maximized;
        BackColor = Color.White;

        tabbedMdiManager.MdiParent = this;

        headerPanel = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = HeaderBg };
        logoPanel = new Panel { BackColor = HeaderBg, Cursor = Cursors.Hand, Width = 212, Dock = DockStyle.Left };
        sidebarPanel = new Panel { Dock = DockStyle.Left, Width = 212, BackColor = SidebarBg };
        statusBar = new Panel { Dock = DockStyle.Bottom, Height = 26, BackColor = SidebarBg };

        BuildLogo();
        BuildToolbar();
        BuildUserArea();
        BuildSidebar();
        BuildAccordionMenu();
        BuildStatusBar();

        headerPanel.Controls.Add(logoPanel);

        var headerBottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = DividerColor };
        headerPanel.Controls.Add(headerBottomBorder);

        // 순서 중요: 상단바를 먼저 추가해야 전체 폭을 차지하고, 그 아래에 좌측 메뉴가 배치됨
        // statusBar는 Bottom이라 순서 무관하게 항상 최하단에 고정됨
        Controls.Add(statusBar);
        Controls.Add(sidebarPanel);
        Controls.Add(headerPanel);

        AppConfig.EnvironmentChanged += () => { RefreshUserInfoLabel(); RefreshTitle(); };
        RefreshUserInfoLabel();

        OpenHomeForm();
    }

    /// <summary>
    /// 로그인 직후 항상 열려있어야 하는 홈화면을 오픈. 이미 열려있으면 재사용(Activate만).
    /// HomeForm은 닫을 수 없게 만들어져 있어서, 정상적인 흐름에서는 이 메서드가 프로그램
    /// 실행 중 딱 한 번만 실제로 새로 생성한다.
    /// </summary>
    private void OpenHomeForm()
    {
        if (_homeForm != null && !_homeForm.IsDisposed)
        {
            _homeForm.Activate();
            return;
        }

        _homeForm = new HomeForm { MdiParent = this };
        _homeForm.Show();
    }

    private void RefreshTitle()
    {
        Text = AppConfig.IsDevelopment
            ? "WYN LAB [개발서버]  ※ 실제 데이터가 아닌 개발/테스트 서버입니다"
            : "WYN LAB";
    }

    /// <summary>
    /// 하단 상태바. BAROCRM 화면 하단처럼 상태메시지(좌) + 서비스/버전정보(우)를 보여준다.
    /// 다른 화면(BaseForm 상속)에서 ShellForm.SetStatusMessage(...)로 메시지를 띄울 수 있다.
    /// </summary>
    private void BuildStatusBar()
    {
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = DividerColor };
        statusBar.Controls.Add(topBorder);

        lblStatusMessage.Location = new Point(14, 5);
        lblStatusMessage.AutoSizeMode = LabelAutoSizeMode.None;
        lblStatusMessage.Size = new Size(500, 18);
        lblStatusMessage.Appearance.ForeColor = Color.FromArgb(90, 90, 90);
        lblStatusMessage.Appearance.Font = AppFonts.Caption;

        lblStatusRight.AutoSizeMode = LabelAutoSizeMode.None;
        lblStatusRight.Size = new Size(320, 18);
        lblStatusRight.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        lblStatusRight.Appearance.ForeColor = Color.FromArgb(120, 120, 120);
        lblStatusRight.Appearance.Font = AppFonts.Caption;
        RefreshStatusRight();

        statusBar.Resize += (s, e) => PositionStatusBar();
        statusBar.Controls.Add(lblStatusMessage);
        statusBar.Controls.Add(lblStatusRight);
        PositionStatusBar();

        AppConfig.EnvironmentChanged += RefreshStatusRight;
        Instance = this;
    }

    private void PositionStatusBar()
    {
        lblStatusRight.Location = new Point(statusBar.Width - lblStatusRight.Width - 14, 5);
    }

    private void RefreshStatusRight()
    {
        var envLabel = GetEnvLabel(AppConfig.CurrentEnvironment);
        lblStatusRight.Text = $"[Service : WYN LAB]  [{envLabel}]  v{DateTime.Now:yyyy.MM.dd}";
    }

    /// <summary>
    /// 다른 화면에서 하단 상태바에 메시지를 띄울 때 사용.
    /// 예: (ShellForm.Instance)?.SetStatusMessage("조회된 데이터가 없습니다.");
    /// </summary>
    public static ShellForm? Instance { get; private set; }

    public void SetStatusMessage(string message) => lblStatusMessage.Text = message;

    /// <summary>좌측 사이드바 - 우측에 명확한 구분선을 둬서 MDI(흰색) 영역과 시각적으로 분리</summary>
    private void BuildSidebar()
    {
        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = ColorHelper.Adjust(SidebarBg, -50) };
        sidebarPanel.Controls.Add(accordionMenu);
        sidebarPanel.Controls.Add(divider);
    }

    /// <summary>좌측 로고 - 클릭할 때마다 좌측 사이드바 전체가 보였다 숨겨졌다 함</summary>
    private void BuildLogo()
    {
        var separator = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = DividerColor };
        logoPanel.Controls.Add(separator);

        var badge = new Panel
        {
            BackColor = _accentColor,
            Size = new Size(28, 28),
            Location = new Point(16, 24)
        };
        var badgeLabel = new LabelControl
        {
            Text = "W",
            AutoSizeMode = LabelAutoSizeMode.None,
            Dock = DockStyle.Fill
        };
        badgeLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        badgeLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        badgeLabel.Appearance.ForeColor = Color.White;
        badgeLabel.Appearance.Font = AppFonts.LogoGlyphSmall;
        badge.Controls.Add(badgeLabel);

        var nameLabel = new LabelControl
        {
            Text = "WYN LAB",
            Location = new Point(52, 29),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(150, 22)
        };
        nameLabel.Appearance.ForeColor = Color.FromArgb(35, 35, 35);
        nameLabel.Appearance.Font = AppFonts.SubHeading;

        logoPanel.Controls.Add(badge);
        logoPanel.Controls.Add(nameLabel);

        void ToggleMenu(object? s, EventArgs e) => sidebarPanel.Visible = !sidebarPanel.Visible;
        logoPanel.Click += ToggleMenu;
        badge.Click += ToggleMenu;
        badgeLabel.Click += ToggleMenu;
        nameLabel.Click += ToggleMenu;
    }

    /// <summary>
    /// 홈은 상단바에 단독 배치하고, 조회/입력/저장 · 삭제 · 출력은 각각 둥근 배경 카드
    /// (RoundedPanel)로 묶어서 "그룹"임이 한눈에 보이도록 구성한다.
    /// 삭제 카드는 옅은 빨강, 출력 카드는 옅은 파랑 배경으로 성격을 색으로도 구분한다.
    /// 클릭하면 현재 활성화된 MDI 자식폼(ActiveMdiChild)의 표준 액션(BaseForm.QueryAsync 등)을 호출한다.
    /// </summary>
    private void BuildToolbar()
    {
        var iconAccent = Color.FromArgb(41, 121, 255);
        var neutralBadge = Color.FromArgb(241, 243, 245);
        var blueBadge = Color.FromArgb(232, 240, 254);
        var redBadge = Color.FromArgb(253, 236, 234);

        var x = 228;
        AddIconBadgeButton(headerPanel, ref x, 6, "홈", ToolbarIconPainters.Home, neutralBadge, iconAccent, false, f => { OpenHomeForm(); return Task.CompletedTask; });
        x += 6;
        AddDivider(ref x);

        // 조회 / 입력 / 저장 - 중립 회색 톤 카드로 한 그룹
        var queryGroup = AddToolbarGroup(ref x, 64 * 3, SidebarBg);
        var qx = 0;
        AddIconBadgeButton(queryGroup, ref qx, 2, "조회", ToolbarIconPainters.Query, neutralBadge, iconAccent, false, f => f.QueryAsync());
        AddIconBadgeButton(queryGroup, ref qx, 2, "입력", ToolbarIconPainters.New, blueBadge, iconAccent, false, f => f.NewAsync());
        AddIconBadgeButton(queryGroup, ref qx, 2, "저장", ToolbarIconPainters.Save, iconAccent, iconAccent, true, f => f.SaveAsync());

        // 삭제 - "위험한 동작"이라 카드 배경 자체를 옅은 빨강으로 강조
        var deleteGroup = AddToolbarGroup(ref x, 64, redBadge);
        var dx = 0;
        AddIconBadgeButton(deleteGroup, ref dx, 2, "삭제", ToolbarIconPainters.Delete, Color.White, DangerColor, false, f => f.DeleteAsync());

        // 출력 - 옅은 파랑 카드로 조회 그룹과는 다른 성격임을 표시
        var printGroup = AddToolbarGroup(ref x, 64, blueBadge);
        var px = 0;
        AddIconBadgeButton(printGroup, ref px, 2, "출력", ToolbarIconPainters.Print, Color.White, iconAccent, false, f => f.PrintAsync());
    }

    /// <summary>둥근 배경 카드를 만들어 headerPanel에 배치하고, 다음 그룹을 위한 x좌표를 진행시킨다.</summary>
    private RoundedPanel AddToolbarGroup(ref int x, int width, Color backColor)
    {
        var panel = new RoundedPanel
        {
            Location = new Point(x, 4),
            Size = new Size(width, 68),
            BackColor = backColor,
            CornerRadius = 14
        };
        headerPanel.Controls.Add(panel);
        x += width + 10;
        return panel;
    }

    /// <summary>
    /// action 파라미터는 BaseForm을 받지만, "홈" 버튼처럼 활성화면과 무관하게 항상 동작해야 하는
    /// 경우도 있어서, 실제로는 델리게이트 내부에서 ActiveMdiChild를 쓸지 말지 자유롭게 결정한다.
    /// (홈 버튼은 activeForm 인자를 무시하고 항상 OpenHomeForm()만 호출)
    /// container: 이 버튼을 실제로 담을 컨트롤(headerPanel 직접 또는 AddToolbarGroup으로 만든 카드).
    /// x/y는 container 기준 로컬 좌표.
    /// </summary>
    private void AddIconBadgeButton(Control container, ref int x, int y, string text, Action<Graphics, Rectangle, Color, Color> painter,
        Color badgeColor, Color accentColor, bool filled, Func<BaseForm, Task> action)
    {
        var btn = new IconBadgeButton
        {
            Text = text,
            IconPainter = painter,
            BadgeColor = badgeColor,
            AccentColor = accentColor,
            FilledBadge = filled,
            Location = new Point(x, y),
            Size = new Size(64, 64)
        };

        btn.Click += async (s, e) =>
        {
            // 홈 버튼처럼 활성화면이 없어도 동작해야 하는 경우를 위해 null 허용 폼으로 처리
            var activeForm = ActiveMdiChild as BaseForm;
            if (activeForm == null && text != "홈")
            {
                XtraMessageBox.Show("먼저 작업할 화면을 열어주세요.", "안내");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                await action(activeForm!);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"[{text}] 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        };

        container.Controls.Add(btn);
        x += btn.Width;
    }

    private void AddDivider(ref int x)
    {
        var divider = new Panel { Location = new Point(x, 21), Size = new Size(1, 34), BackColor = DividerColor };
        headerPanel.Controls.Add(divider);
        x += 12;
    }

    /// <summary>우측 - 서버전환 콤보 + 사용자정보</summary>
    private void BuildUserArea()
    {
        cboEnvironment.Properties.Items.AddRange(AppConfig.AvailableEnvironments.Select(GetEnvLabel).ToArray());
        cboEnvironment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboEnvironment.SelectedItem = GetEnvLabel(AppConfig.CurrentEnvironment);
        cboEnvironment.Font = AppFonts.Body;
        cboEnvironment.Size = new Size(112, 26);
        cboEnvironment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cboEnvironment.SelectedIndexChanged += OnEnvironmentComboChanged;

        lblUserInfo.AutoSizeMode = LabelAutoSizeMode.None;
        lblUserInfo.Size = new Size(160, 34);
        lblUserInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        lblUserInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        lblUserInfo.Appearance.ForeColor = Color.FromArgb(45, 45, 45);
        lblUserInfo.Appearance.Font = AppFonts.Body;
        lblUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;

        headerPanel.Resize += (s, e) => PositionUserArea();
        headerPanel.Controls.Add(cboEnvironment);
        headerPanel.Controls.Add(lblUserInfo);
        PositionUserArea();
    }

    private void PositionUserArea()
    {
        cboEnvironment.Location = new Point(headerPanel.Width - cboEnvironment.Width - 16, 25);
        lblUserInfo.Location = new Point(cboEnvironment.Left - lblUserInfo.Width - 12, 21);
    }

    private void RefreshUserInfoLabel()
    {
        var user = SessionManager.Current.UserInfo;
        lblUserInfo.Text = user == null
            ? string.Empty
            : $"{user.UserNm} {user.PositionNm}\n{user.DeptNm}";
    }

    private string GetEnvLabel(string env) => _envLabels.TryGetValue(env, out var label) ? label : env;

    private string GetEnvKey(string label) => _envLabels.FirstOrDefault(kv => kv.Value == label).Key ?? label;

    /// <summary>
    /// 서버 전환. 다른 서버는 다른 DB를 바라보므로, 현재 세션/화면은 전부 무효화하고
    /// 새 서버 기준으로 재로그인을 받아야 한다. 재로그인 취소시 프로그램을 종료한다
    /// (예전 서버 세션으로 되돌릴 방법이 없으므로).
    /// </summary>
    private void OnEnvironmentComboChanged(object? sender, EventArgs e)
    {
        if (_suppressEnvChange) return;

        var selectedKey = GetEnvKey((string)cboEnvironment.SelectedItem!);
        if (selectedKey == AppConfig.CurrentEnvironment) return;

        var confirm = XtraMessageBox.Show(
            $"'{GetEnvLabel(selectedKey)}'로 전환하면 현재 열려있는 화면이 모두 닫히고 다시 로그인해야 합니다.\n계속하시겠습니까?",
            "서버 전환", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
        {
            _suppressEnvChange = true;
            cboEnvironment.SelectedItem = GetEnvLabel(AppConfig.CurrentEnvironment);
            _suppressEnvChange = false;
            return;
        }

        foreach (Form child in MdiChildren.ToArray())
        {
            child.Close();
        }

        SessionManager.Current.SignOut();
        AppConfig.SwitchEnvironment(selectedKey);
        accordionMenu.Elements.Clear();

        Hide();
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            BuildAccordionMenu();
            RefreshUserInfoLabel();
            Show();
        }
        else
        {
            Application.Exit();
        }
    }

    /// <summary>
    /// SessionManager에 캐싱된 메뉴권한(MenuDto) 목록으로 Accordion 트리를 재귀 구성.
    /// MENU_TYPE = GROUP 이면 상위그룹, FORM이면 클릭 시 화면 오픈.
    /// </summary>
    private void BuildAccordionMenu()
    {
        var menus = SessionManager.Current.Menus.Where(m => m.ViewYn).ToList();
        var topMenus = menus.Where(m => m.UpperMenuCd == null).OrderBy(m => m.SortOrder);

        foreach (var top in topMenus)
        {
            var group = new AccordionControlElement
            {
                Text = top.MenuNm,
                Name = top.MenuCd,
                Style = ElementStyle.Group
            };

            // 최상위 모듈만 브랜드 색상 배경으로 강조 - 하위 메뉴와 한눈에 구분되도록.
            // 회사별 커스터마이징을 위해 헤더의 로고 배지와 동일한 _accentColor(ToolbarColor)를 재사용.
            group.Appearance.Normal.BackColor = _accentColor;
            group.Appearance.Normal.ForeColor = Color.White;
            group.Appearance.Normal.Font = AppFonts.SubHeading;
            group.Appearance.Normal.Options.UseBackColor = true;
            group.Appearance.Normal.Options.UseForeColor = true;
            group.Appearance.Normal.Options.UseFont = true;

            // 마우스 올렸을 때도 톤을 맞춰줌 (약간 밝게)
            group.Appearance.Hovered.BackColor = ColorHelper.Adjust(_accentColor, 20);
            group.Appearance.Hovered.ForeColor = Color.White;
            group.Appearance.Hovered.Font = AppFonts.SubHeading;
            group.Appearance.Hovered.Options.UseBackColor = true;
            group.Appearance.Hovered.Options.UseForeColor = true;
            group.Appearance.Hovered.Options.UseFont = true;

            AddChildMenus(group, menus, top.MenuCd);
            accordionMenu.Elements.Add(group);
        }
    }

    private void AddChildMenus(AccordionControlElement parent, List<MenuDto> allMenus, string upperMenuCd)
    {
        AddChildMenus(parent, allMenus, upperMenuCd, new HashSet<string> { upperMenuCd });
    }

    /// <summary>
    /// visited: 지금까지 내려온 조상 메뉴코드 목록. 메뉴관리 화면에서 상위메뉴코드를
    /// 순환되게(A→B→A) 잘못 입력해도, 이걸로 감지해서 무한재귀(StackOverflow로 인한
    /// 프로그램 다운)를 막는다. 순환이 감지되면 해당 하위 메뉴는 그냥 건너뛴다.
    /// </summary>
    private void AddChildMenus(AccordionControlElement parent, List<MenuDto> allMenus, string upperMenuCd, HashSet<string> visited)
    {
        var children = allMenus.Where(m => m.UpperMenuCd == upperMenuCd).OrderBy(m => m.SortOrder);

        foreach (var child in children)
        {
            if (!visited.Add(child.MenuCd))
            {
                // 이미 조상 경로에 있던 메뉴코드가 다시 나타남 = 순환 참조. 건너뛰고 계속 진행.
                continue;
            }

            var element = new AccordionControlElement
            {
                Text = child.MenuNm,
                Name = child.MenuCd,
                Style = child.MenuType == "GROUP" ? ElementStyle.Group : ElementStyle.Item
            };

            if (child.MenuType == "GROUP")
            {
                // 2단계 이하 그룹(소분류) - 최상위 그룹과 구분되도록 배경 없이 굵은 글씨만
                element.Appearance.Normal.ForeColor = Color.FromArgb(60, 60, 60);
                element.Appearance.Normal.Font = AppFonts.BodyBold;
                element.Appearance.Normal.Options.UseForeColor = true;
                element.Appearance.Normal.Options.UseFont = true;

                element.Appearance.Hovered.ForeColor = _accentColor;
                element.Appearance.Hovered.Options.UseForeColor = true;
            }
            else
            {
                // 실제 클릭 가능한 화면(FORM) - 은은한 강조색 호버로 클릭 가능함을 명확히 표시
                element.Appearance.Normal.ForeColor = Color.FromArgb(80, 80, 80);
                element.Appearance.Normal.Font = AppFonts.Body;
                element.Appearance.Normal.Options.UseForeColor = true;
                element.Appearance.Normal.Options.UseFont = true;

                element.Appearance.Hovered.BackColor = ColorHelper.Adjust(SidebarBg, -12);
                element.Appearance.Hovered.ForeColor = _accentColor;
                element.Appearance.Hovered.Font = AppFonts.BodyBold;
                element.Appearance.Hovered.Options.UseBackColor = true;
                element.Appearance.Hovered.Options.UseForeColor = true;
                element.Appearance.Hovered.Options.UseFont = true;

                element.Appearance.Pressed.BackColor = ColorHelper.Adjust(SidebarBg, -20);
                element.Appearance.Pressed.ForeColor = _accentColor;
                element.Appearance.Pressed.Options.UseBackColor = true;
                element.Appearance.Pressed.Options.UseForeColor = true;

                element.Click += (s, e) => OpenMenuForm(child);
            }

            AddChildMenus(element, allMenus, child.MenuCd, visited);
            parent.Elements.Add(element);

            visited.Remove(child.MenuCd); // 형제 메뉴 처리를 위해 이 가지에서만 빠져나오면 복원
        }
    }

    /// <summary>
    /// MENU.FORM_CLASS_NM (예: "WYNLAB.Modules.System.UserListForm, WYNLAB.Modules.System") 을
    /// 리플렉션으로 로딩해서 MDI 자식으로 오픈. 이미 열려있으면 해당 탭으로 포커스만 이동.
    /// </summary>
    private void OpenMenuForm(MenuDto menu)
    {
        if (string.IsNullOrWhiteSpace(menu.FormClassNm))
        {
            XtraMessageBox.Show("연결된 화면이 없습니다. (FORM_CLASS_NM 미설정)", "안내");
            return;
        }

        var existing = MdiChildren.FirstOrDefault(f => f.Name == menu.MenuCd);
        if (existing != null)
        {
            var confirm = XtraMessageBox.Show(
                $"'{menu.MenuNm}' 화면이 이미 열려있습니다.\n신규로 오픈하시겠습니까?",
                "화면 중복", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
            {
                existing.Activate();
                return;
            }
            // "예"를 선택하면 아래로 내려가서 새 인스턴스를 만든다 (기존 탭은 그대로 둠)
        }

        var formType = Type.GetType(menu.FormClassNm);
        if (formType == null || Activator.CreateInstance(formType) is not BaseForm form)
        {
            XtraMessageBox.Show($"화면을 찾을 수 없습니다: {menu.FormClassNm}", "오류");
            return;
        }

        form.Name = menu.MenuCd;
        form.MenuCd = menu.MenuCd;
        form.MdiParent = this;
        form.Show();
    }
}
