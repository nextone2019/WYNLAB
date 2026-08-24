using DevExpress.XtraBars.Navigation;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// MDI 메인 셸 (디자인: 브랜드 컬러 헤더 + 사이드바, 개별 아이콘 툴바).
///
/// 레이아웃 구조 (위→아래):
///  1. headerPanel  - 로고 + 홈/조회/입력/저장/삭제/출력 개별 아이콘 버튼. 사용자정보는 좌측 사이드바로 이동됨.
///  2. sidebarPanel - 좌측 메뉴(다크 배경 + 우측 구분선으로 MDI 영역과 명확히 분리). 로고 클릭으로 표시/숨김
///  3. MDI 클라이언트 영역(흰색) - 업무화면들이 뜨는 공간
///
/// 헤더와 사이드바는 둘 다 appsettings.json의 ToolbarColor(회사별 브랜드색)에서 파생된 같은 색
/// 계열이지만, 헤더는 흰색 쪽으로 살짝 섞은 밝은 톤, 사이드바는 검정 쪽으로 많이 섞은 짙은 톤을
/// 써서 "같은 브랜드지만 서로 다른 면"임이 구분되도록 한다(ColorHelper.Mix). 툴바 버튼은 카드로
/// 묶지 않고 전부 개별 흰 배지 버튼으로 나열하며, "성격" 구분은 아이콘 색으로만 표현한다
/// (삭제 아이콘은 브랜드 색과 무관하게 항상 빨간색 고정 - 위험한 동작이라는 UX 관례 유지).
/// </summary>
public class ShellForm : XtraForm
{
    private readonly Color _accentColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

    // 헤더와 사이드바가 "같은 브랜드 색 계열이지만 톤(명도)이 다른" 두 면으로 보이도록,
    // 둘 다 _accentColor에서 파생시킨다. 사이드바는 검정 쪽으로 많이 섞어 원래 의도(항상
    // 짙은 배경에 밝은 글자, 브랜드색이 밝아도 대비 보장)를 유지하고, 헤더는 흰색 쪽으로
    // 살짝만 섞어 사이드바보다 눈에 띄게 연한 톤으로 구분되게 한다.
    private Color HeaderBg => ColorHelper.Mix(_accentColor, Color.White, 0.35f);
    private Color HeaderDividerColor => ColorHelper.Adjust(HeaderBg, -22);
    // 툴바 아이콘 배지는 순백색이면 헤더 톤 위에서 너무 튀어 보여서, 헤더색을 살짝 섞은
    // 연한 톤으로 낮춘다 - 헤더와 같은 계열이라 훨씬 차분하게 어우러진다.
    private Color IconBadgeBg => ColorHelper.Mix(HeaderBg, Color.White, 0.55f);
    private static readonly Color DangerColor = Color.FromArgb(192, 57, 43);
    private static readonly Color SidebarBg = Color.FromArgb(245, 246, 248);
    private static readonly Color DividerColor = Color.FromArgb(225, 225, 225);
    // 툴바 아이콘과 활성 MDI 탭 표시줄이 공유하는 "인터랙션" 강조색(브랜드색과는 별개로 고정).
    private static readonly Color ActionAccent = Color.FromArgb(41, 121, 255);

    // MDI 문서 탭 색 - 비활성 탭은 눌러앉은 느낌의 연회색, 활성 탭은 배경색 하나로만 구분한다.
    // 순백색(255,255,255)이나 살짝만 섞은 오프화이트는 거의 흰색이나 마찬가지라는 피드백이 있어서,
    // 사이드바 색(NavDarkBg = 메뉴트리 색과 같은 계열)을 흰색 쪽으로 많이 섞은 옅은 톤으로
    // 바꿨다 - "메뉴트리와 같은 계열의 아주 연한 색"으로 육안에도 분명히 인지되게 한다.
    private static readonly Color TabInactiveBg = Color.FromArgb(232, 234, 238);
    private static readonly Color TabHotBg = Color.FromArgb(244, 245, 247);
    private Color TabActiveBg => ColorHelper.Mix(NavDarkBg, Color.White, 0.75f);
    private static readonly Color TabInactiveFg = Color.FromArgb(120, 122, 128);
    private static readonly Color TabActiveFg = Color.FromArgb(35, 35, 38);

    private Color NavDarkBg => ColorHelper.Mix(_accentColor, Color.Black, 0.45f);
    private Color NavHoverBg => ColorHelper.Adjust(NavDarkBg, 12);
    private Color NavPressedBg => ColorHelper.Adjust(NavDarkBg, 23);
    private static readonly Color NavText = Color.FromArgb(222, 224, 228);
    private static readonly Color NavTextMuted = Color.FromArgb(158, 161, 168);
    private Color NavDivider => ColorHelper.Adjust(NavDarkBg, 20);

    private const int MenuTopIconSize = 15;
    private const int MenuLeafDotSize = 9;
    private const int RailIconSize = 13;

    // 사이드바 접힘/펼침 폭. 예전엔 로고 클릭시 사이드바 자체가 Visible=false로 완전히
    // 사라졌는데(폭 0), 접혀도 최상위 메뉴 아이콘만 남는 "아이콘 레일"로 바꿔서 접힌 상태에서도
    // 길을 잃지 않고 계속 다른 모듈로 이동할 수 있게 한다(요즘 관리자 UI들의 표준 패턴).
    private const int SidebarExpandedWidth = 212;
    private const int SidebarCollapsedWidth = 60;
    private bool _sidebarCollapsed;

    private readonly Panel headerPanel;
    private readonly Panel logoPanel;
    private readonly Panel headerRightPanel = new() { Dock = DockStyle.Right, Width = 210 };
    private readonly LabelControl lblEnvBadge = new();
    private readonly Panel avatarBadge = new() { Size = new Size(30, 30) };
    private readonly LabelControl lblAvatarInitial = new();
    private readonly Panel sidebarPanel;
    private readonly Panel statusBar;
    // 접힌 상태에서 accordionMenu 대신 보여주는 아이콘 전용 레일 - 최상위 메뉴당 아이콘 버튼 1개.
    // 클릭하면 사이드바를 다시 펼친다(하위 메뉴까지 좁은 폭에 다 담기는 어려워, 펼침으로 위임).
    private readonly Panel sidebarIconRail = new() { Dock = DockStyle.Fill, Visible = false };
    private Panel? _sidebarToolPanel;
    private LabelControl? _lblUserInline;
    // 사이드바 맨 위 여백 - 오른쪽 MDI 탭 줄과 높이를 맞춰서, 그 아래(사용자정보)와
    // 탭 줄 아래(문서 내용)가 같은 Y좌표에서 시작하도록 CustomDrawTabHeader에서 실측해 맞춘다.
    private readonly Panel sidebarTopGap = new() { Dock = DockStyle.Top, Height = 30, BackColor = Color.White };
    private readonly LabelControl lblStatusMessage = new();
    private readonly LabelControl lblStatusRight = new();
    private readonly ComboBoxEdit cboEnvironment = new();
    private readonly ComboBoxEdit cboSkin = new();
    private readonly ToolTip toolbarToolTip = new();

    /// <summary>
    /// 사용자가 고를 수 있는 테마 목록. DevExpress에는 스킨이 수십 개 있어 전부 나열하면
    /// 오래되거나 브랜드와 안 어울리는 것도 섞여 들어가므로, 최근/모던한 스킨만 선별했다.
    /// </summary>
    private static readonly string[] AvailableSkins =
    {
        "Office 2019 Colorful", "Office 2019 Black", "Office 2019 Dark Gray", "Office 2019 White",
        "Visual Studio 2019 Blue", "Visual Studio 2019 Dark", "WXI", "Basic"
    };

    private readonly AccordionControl accordionMenu = new() { Dock = DockStyle.Fill };
    private readonly XtraTabbedMdiManager tabbedMdiManager = new();
    private HomeForm? _homeForm;

    // 예전엔 헤더 툴바 맨 앞에 있었는데, 메뉴트리 바로 위(사이드바 상단 여백)로 옮기고
    // 크기도 작게 줄였다 - 다른 업무 액션들과 성격이 달라서(화면 전환이지 데이터 액션이 아님)
    // 메뉴트리와 더 가까운 자리가 자연스럽다는 피드백.
    // IconInset을 기본값(9, 헤더 54x48 버튼 기준)보다 훨씬 줄여야 이 작은 크기에서도 아이콘이
    // 실제로 보인다 - 처음엔 기본값 그대로 썼다가 아이콘이 점처럼 작아져 거의 안 보였다.
    private readonly IconBadgeButton homeButton = new()
    {
        Text = "홈",
        IconPainter = ToolbarIconPainters.Home,
        Size = new Size(28, 28),
        IconInset = 5
    };

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

        // 가로: 현재 헤더 툴바(조회/입력/삭제/행추가/행삭제/저장/출력 7개, 구분선 없이 한 줄)가
        // 겹치지 않고 다 보이는 최소폭. 홈 버튼은 사이드바 메뉴트리 위로 옮겨서 헤더에서 빠졌다.
        // 세로도 업무화면이 너무 눌리지 않도록 최소값을 둠.
        MinimumSize = new Size(920, 650);

        tabbedMdiManager.MdiParent = this;
        // MDI 탭 헤더에 X(닫기) 버튼 표시 + 탭 영역 맨 오른쪽에 "현재 탭 닫기" 버튼도 같이
        // 표시(InAllTabPagesAndTabControlHeader) - Home 탭은 BaseForm/HomeForm.OnFormClosing에서
        // 이미 닫기를 막고 있어서, 두 버튼 다 눌러도 실제로는 안 닫힌다.
        tabbedMdiManager.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
        ConfigureTabAppearance();

        headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = HeaderBg };
        logoPanel = new Panel { BackColor = HeaderBg, Cursor = Cursors.Hand, Width = 212, Dock = DockStyle.Left };
        sidebarPanel = new Panel { Dock = DockStyle.Left, Width = 212, BackColor = NavDarkBg };
        statusBar = new Panel { Dock = DockStyle.Bottom, Height = 26, BackColor = SidebarBg };

        BuildLogo();
        BuildToolbar();
        BuildHeaderRight();
        BuildSidebar();
        BuildAccordionMenu();
        BuildStatusBar();

        headerPanel.Controls.Add(logoPanel);
        headerPanel.Controls.Add(headerRightPanel);

        var headerBottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = HeaderDividerColor };
        headerPanel.Controls.Add(headerBottomBorder);

        // 순서 중요: 상단바를 먼저 추가해야 전체 폭을 차지하고, 그 아래에 좌측 메뉴가 배치됨
        // statusBar는 Bottom이라 순서 무관하게 항상 최하단에 고정됨
        Controls.Add(statusBar);
        Controls.Add(sidebarPanel);
        Controls.Add(headerPanel);

        AppConfig.EnvironmentChanged += RefreshTitle;

        FormClosing += ShellForm_FormClosing;

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

    /// <summary>사용자가 셸의 X버튼으로 직접 닫으려 할 때만 확인 - 서버전환 취소 등 프로그램 내부에서
    /// Application.Exit()을 호출하는 경우는 이미 그 자리에서 확인을 거친 것이므로 재확인하지 않는다.</summary>
    private void ShellForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.UserClosing) return;

        var confirm = AppMessageBox.Show("모든 프로그램을 종료하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            e.Cancel = true;
        }
    }

    private void RefreshTitle()
    {
        Text = AppConfig.IsDevelopment
            ? "WYN LAB [개발서버]  ※ 실제 데이터가 아닌 개발/테스트 서버입니다"
            : "WYN LAB";
    }

    /// <summary>
    /// MDI 문서 탭 - 기본 XtraTabbedMdiManager는 각진 사각 탭에 활성/비활성 색 차이가 거의 없어서
    /// 산만해 보였다. AppearancePage로 상태별 색/폰트를 지정하고, CustomDrawTabHeader로 탭
    /// 배경을 직접 그려서(위쪽만 둥근 모서리) 좀 더 부드럽고 활성 탭이 눈에 띄게 만든다.
    /// </summary>
    private void ConfigureTabAppearance()
    {
        tabbedMdiManager.AppearancePage.Header.BackColor = TabInactiveBg;
        tabbedMdiManager.AppearancePage.Header.ForeColor = TabInactiveFg;
        tabbedMdiManager.AppearancePage.Header.Font = AppFonts.Body;
        tabbedMdiManager.AppearancePage.Header.Options.UseBackColor = true;
        tabbedMdiManager.AppearancePage.Header.Options.UseForeColor = true;
        tabbedMdiManager.AppearancePage.Header.Options.UseFont = true;

        tabbedMdiManager.AppearancePage.HeaderHotTracked.BackColor = TabHotBg;
        tabbedMdiManager.AppearancePage.HeaderHotTracked.ForeColor = TabActiveFg;
        tabbedMdiManager.AppearancePage.HeaderHotTracked.Options.UseBackColor = true;
        tabbedMdiManager.AppearancePage.HeaderHotTracked.Options.UseForeColor = true;

        tabbedMdiManager.AppearancePage.HeaderActive.BackColor = TabActiveBg;
        tabbedMdiManager.AppearancePage.HeaderActive.ForeColor = TabActiveFg;
        tabbedMdiManager.AppearancePage.HeaderActive.Font = AppFonts.Body;
        tabbedMdiManager.AppearancePage.HeaderActive.Options.UseBackColor = true;
        tabbedMdiManager.AppearancePage.HeaderActive.Options.UseForeColor = true;
        tabbedMdiManager.AppearancePage.HeaderActive.Options.UseFont = true;

        tabbedMdiManager.CustomDrawTabHeader += TabbedMdiManager_CustomDrawTabHeader;

        // XtraTabbedMdiManager엔 탭 텍스트 좌우 여백을 조절하는 속성이 없어서(리플렉션으로
        // 전체 속성을 확인했지만 Padding/Indent/최소너비 같은 훅이 전혀 없었다), 탭이 텍스트
        // 길이에 딱 맞게 폭을 계산하는 걸 역으로 이용한다 - 탭에 표시되는 문자열 앞뒤에 공백을
        // 붙이면 그만큼 탭 자체가 넓어지면서 자연스럽게 여백처럼 보인다. form.Text(창 제목,
        // BuildScreenHeader의 화면 타이틀)는 그대로 두고, MDI 탭 전용 Text만 별도로 바꾼다.
        tabbedMdiManager.PageAdded += (s, e) => e.Page.Text = $"  {e.Page.Text}  ";
    }

    /// <summary>
    /// 탭 배경을 직접 그리고(위쪽 모서리만 살짝 둥글게, 탭 사이는 간격을 둬서 서로 안 맞닿게),
    /// 텍스트/아이콘/닫기 버튼은 DevExpress 기본 로직(DefaultDraw*)에 그대로 맡긴다.
    /// 활성 탭 강조는 배경색 차이 하나로만 표현한다(처음엔 상단 강조색 바를 더했었는데,
    /// 과하다는 피드백을 받아 배경색만 남기고 단순화했다).
    /// </summary>
    private void TabbedMdiManager_CustomDrawTabHeader(object? sender, TabHeaderCustomDrawEventArgs e)
    {
        // 탭 줄의 실제 높이를 매번 측정해서 사이드바 상단 여백에 반영 - 폰트/스킨이 바뀌어도
        // 왼쪽(사용자정보)과 오른쪽(문서 내용) 시작 Y좌표가 계속 맞도록 자동으로 따라간다.
        var rowHeight = e.TabHeaderRowInfo.Bounds.Height;
        if (rowHeight > 0 && sidebarTopGap.Height != rowHeight)
        {
            sidebarTopGap.Height = rowHeight;
        }

        var info = e.TabHeaderInfo;
        var rect = e.Bounds;
        if (rect.Width <= 0 || rect.Height <= 0) return;

        rect.Inflate(-1, 0);
        rect.Y += 2;
        rect.Height -= 2;

        var isActive = info.IsActiveState;
        var isHot = info.IsHotState;
        var back = isActive ? TabActiveBg : (isHot ? TabHotBg : TabInactiveBg);

        var g = e.Graphics;
        var oldMode = g.SmoothingMode;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // 테두리 없이 배경색만 있으니 밋밋해 보인다는 피드백 - 배경색 기준으로 살짝 어둡게
        // 만든 얇은 테두리를 둘러서 탭 하나하나의 경계가 또렷하게 보이게 한다.
        using (var path = TopRoundedRect(rect, 3))
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillPath(brush, path);
            }
            using var pen = new Pen(ColorHelper.Adjust(back, -24));
            g.DrawPath(pen, path);
        }

        g.SmoothingMode = oldMode;

        e.DefaultDrawImage();
        e.DefaultDrawText();
        e.DefaultDrawButtons();
        e.Handled = true;
    }

    /// <summary>위쪽 두 모서리만 둥근 사각형 - 탭이 아래쪽 본문(MDI 영역)과 이어지는 느낌을 유지한다.</summary>
    private static System.Drawing.Drawing2D.GraphicsPath TopRoundedRect(Rectangle bounds, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        var d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddLine(bounds.Right, bounds.Y + radius, bounds.Right, bounds.Bottom);
        path.AddLine(bounds.Right, bounds.Bottom, bounds.X, bounds.Bottom);
        path.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Y + radius);
        path.CloseFigure();
        return path;
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

    /// <summary>좌측 사이드바 - 다크 테마. 우측 구분선으로 MDI(흰색) 영역과 시각적으로 분리.
    /// 맨 위 흰 여백(sidebarTopGap)엔 로그인 사용자/시간, 맨 아래엔 서비스선택/테마선택 콤보,
    /// 가운데는 메뉴 아코디언.</summary>
    private void BuildSidebar()
    {
        accordionMenu.BackColor = NavDarkBg;
        // 내용이 사이드바 높이보다 짧을 때도 스크롤바 트랙이 항상 보이던 것을 숨김.
        // (메뉴가 많아져서 실제로 넘치면 마우스 휠 스크롤 자체는 그대로 동작함)
        accordionMenu.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;

        // 그룹(Style=Group)은 DevExpress 기본 동작 그대로 한 번 클릭하면 펼침/접힘이 되고,
        // 화면(Style=Item)은 AddChildMenus에서 Click을 안 걸어뒀으므로 한 번 클릭으론 아무 일도
        // 안 일어난다 - 실수로 스치듯 클릭했다가 화면이 열리는 걸 막기 위해 더블클릭으로만
        // 열리게 한다. AccordionControlElement 자체엔 DoubleClick 이벤트가 없어서(Click만
        // 있음), 컨트롤 레벨 MouseDoubleClick + CalcHitInfo로 더블클릭 지점의 실제 엘리먼트를
        // 찾아낸다.
        accordionMenu.MouseDoubleClick += AccordionMenu_MouseDoubleClick;

        sidebarIconRail.BackColor = NavDarkBg;

        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = NavDivider };
        _sidebarToolPanel = BuildSidebarToolPanel();
        ConfigureSidebarTopGap();

        // Dock 추가 순서: Fill(accordionMenu/sidebarIconRail) 먼저, Top/Bottom은 나중에 추가해야
        // 각자 가장자리를 정상적으로 차지한다 (PermissionAssignForm에서 겪은 것과 같은 문제 방지).
        sidebarPanel.Controls.Add(accordionMenu);
        sidebarPanel.Controls.Add(sidebarIconRail);
        sidebarPanel.Controls.Add(divider);
        sidebarPanel.Controls.Add(_sidebarToolPanel);
        sidebarPanel.Controls.Add(sidebarTopGap);
    }

    /// <summary>
    /// 사이드바 맨 위 흰 여백(오른쪽 탭 줄과 높이를 맞춘 sidebarTopGap) 안에 로그인 사용자명 +
    /// 시각을 넣는다. 원래는 사이드바 안쪽에 별도의 어두운 패널로 표시했는데, 탭 줄과 높이를
    /// 맞추려고 추가한 위쪽 흰 여백이 비어있느니 차라리 그 자리에 넣는 게 낫다는 피드백으로 이동.
    /// 홈 버튼도 여기(오른쪽 끝, 메뉴트리 바로 위)에 작게 배치한다 - 예전엔 헤더 툴바 맨 앞에
    /// 있었는데, 다른 업무 액션들과 성격이 달라서 메뉴트리와 더 가까운 자리로 옮겼다.
    /// </summary>
    private void ConfigureSidebarTopGap()
    {
        var lblUserInline = new LabelControl
        {
            Dock = DockStyle.Fill,
            AutoSizeMode = LabelAutoSizeMode.None,
            Padding = new Padding(14, 0, 10, 0)
        };
        lblUserInline.Appearance.ForeColor = Color.FromArgb(55, 55, 55);
        lblUserInline.Appearance.Font = AppFonts.Body;
        lblUserInline.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        _lblUserInline = lblUserInline;

        void Refresh()
        {
            var user = SessionManager.Current.UserInfo;
            if (user == null)
            {
                lblUserInline.Text = string.Empty;
                return;
            }
            var time = SessionManager.Current.SignInTime is { } t ? $" [{t:yyyy-MM-dd HH:mm}]" : string.Empty;
            lblUserInline.Text = $"{user.UserNm}{time}";
        }
        Refresh();
        AppConfig.EnvironmentChanged += Refresh;

        // 배지 기본색(241,243,245)은 흰 배경(sidebarTopGap)과 거의 구분이 안 돼서, 브랜드색을
        // 살짝 섞은 톤으로 바꿔 흰 배경 위에서도 보이게 한다(헤더 툴바 IconBadgeBg와 같은 원리).
        homeButton.BadgeColor = ColorHelper.Mix(_accentColor, Color.White, 0.85f);
        homeButton.AccentColor = ActionAccent;
        homeButton.Dock = DockStyle.Right; // 절대좌표 계산 대신, 이 파일에서 이미 여러 번 검증된 Dock 방식 사용
        toolbarToolTip.SetToolTip(homeButton, "홈");
        homeButton.Click += (s, e) => OpenHomeForm();

        // Dock 추가 순서 중요(이 파일 전체에 반복되는 규칙): Fill(lblUserInline) 먼저,
        // 가장자리에 붙는 컨트롤(homeButton, Dock=Right)은 나중에 추가해야 제자리를 차지한다.
        sidebarTopGap.Controls.Add(lblUserInline);
        sidebarTopGap.Controls.Add(homeButton);
    }

    /// <summary>
    /// 서비스선택 + 테마선택 콤보를 다크 사이드바 톤에 맞춰 배치. 사이드바 맨 아래에 고정.
    /// 각 행을 독립된 패널로 분리해뒀기 때문에, 나중에 "메뉴 찾기" 검색창을 추가할 때도
    /// 이 패널들 사이에 같은 방식으로 한 행만 끼워넣으면 된다.
    /// </summary>
    private Panel BuildSidebarToolPanel()
    {
        var toolPanel = new Panel { Dock = DockStyle.Bottom, Height = 112, BackColor = NavDarkBg, Padding = new Padding(14, 10, 14, 10) };
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = NavDivider };

        var envRow = BuildSidebarComboRow("서비스", cboEnvironment);
        var skinRow = BuildSidebarComboRow("테마", cboSkin);

        cboEnvironment.Properties.Items.AddRange(AppConfig.AvailableEnvironments.Select(GetEnvLabel).ToArray());
        cboEnvironment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboEnvironment.SelectedItem = GetEnvLabel(AppConfig.CurrentEnvironment);
        cboEnvironment.SelectedIndexChanged += OnEnvironmentComboChanged;

        // 테마 선택 - 고르는 즉시 UserLookAndFeel이 전역으로 바뀌면서 이미 열려있는 화면들까지
        // 포함해 앱 전체(메시지박스, 버튼, 탭, 그리드...)에 실시간으로 반영된다.
        cboSkin.Properties.Items.AddRange(AvailableSkins);
        cboSkin.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboSkin.SelectedItem = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName;
        cboSkin.SelectedIndexChanged += (s, e) =>
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle((string)cboSkin.SelectedItem!);

        // Dock 추가 순서: skinRow(아래쪽 항목) 먼저, envRow(위쪽 항목) 나중에 -
        // 나중에 추가된 Top이 우선권을 가지므로 이렇게 해야 화면상 envRow가 위, skinRow가 아래로 온다.
        toolPanel.Controls.Add(skinRow);
        toolPanel.Controls.Add(envRow);
        toolPanel.Controls.Add(topBorder);

        return toolPanel;
    }

    /// <summary>다크 배경 위 "라벨 + 콤보" 한 줄. 콤보 자체도 사이드바 톤에 맞게 어둡게 스타일링.</summary>
    private Panel BuildSidebarComboRow(string label, ComboBoxEdit combo)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 46 };

        var lbl = new LabelControl { Text = label, Dock = DockStyle.Top, Height = 16 };
        lbl.Appearance.ForeColor = NavTextMuted;
        lbl.Appearance.Font = AppFonts.Caption;

        combo.Dock = DockStyle.Top;
        combo.Font = AppFonts.Body;
        combo.Properties.Appearance.BackColor = NavHoverBg;
        combo.Properties.Appearance.ForeColor = NavText;
        combo.Properties.Appearance.Options.UseBackColor = true;
        combo.Properties.Appearance.Options.UseForeColor = true;
        combo.Properties.Appearance.BorderColor = NavDivider;
        combo.Properties.Appearance.Options.UseBorderColor = true;

        // Dock 순서: 콤보 먼저, 라벨은 나중에(Top 우선권) -> 라벨이 위, 콤보가 아래로 배치
        row.Controls.Add(combo);
        row.Controls.Add(lbl);

        return row;
    }

    /// <summary>좌측 로고 - 클릭할 때마다 좌측 사이드바 전체가 보였다 숨겨졌다 함</summary>
    private void BuildLogo()
    {
        var separator = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = HeaderDividerColor };
        logoPanel.Controls.Add(separator);

        // 로고 영역 배경이 이제 헤더와 같은 브랜드색이라, 배지도 같은 색으로 채우면
        // 눈에 안 띄게 된다. 흰색 배지 위에 브랜드색 글자를 얹어 대비를 확보.
        var badge = new Panel
        {
            BackColor = Color.White,
            Size = new Size(28, 28),
            Location = new Point(16, 16)
        };
        var badgeLabel = new LabelControl
        {
            Text = "W",
            AutoSizeMode = LabelAutoSizeMode.None,
            Dock = DockStyle.Fill
        };
        badgeLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        badgeLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        badgeLabel.Appearance.ForeColor = _accentColor;
        badgeLabel.Appearance.Font = AppFonts.LogoGlyphSmall;
        badge.Controls.Add(badgeLabel);

        var nameLabel = new LabelControl
        {
            Text = "WYN LAB",
            Location = new Point(52, 19),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(150, 22)
        };
        nameLabel.Appearance.ForeColor = Color.White;
        nameLabel.Appearance.Font = AppFonts.SubHeading;

        logoPanel.Controls.Add(badge);
        logoPanel.Controls.Add(nameLabel);

        void ToggleMenu(object? s, EventArgs e) => ToggleSidebarCollapsed();
        logoPanel.Click += ToggleMenu;
        badge.Click += ToggleMenu;
        badgeLabel.Click += ToggleMenu;
        nameLabel.Click += ToggleMenu;
    }

    /// <summary>
    /// 사이드바 접힘/펼침 전환. 접힌 상태에선 폭만 줄이는 게 아니라 accordionMenu(트리) 대신
    /// sidebarIconRail(최상위 아이콘만)을 보여준다 - 하위 메뉴/서비스·테마 선택 콤보는 좁은
    /// 폭에 담기 어려워서 함께 숨기고, 사용자명 텍스트도 잘려 보이므로 같이 숨긴다.
    /// </summary>
    private void ToggleSidebarCollapsed()
    {
        _sidebarCollapsed = !_sidebarCollapsed;

        sidebarPanel.Width = _sidebarCollapsed ? SidebarCollapsedWidth : SidebarExpandedWidth;
        accordionMenu.Visible = !_sidebarCollapsed;
        sidebarIconRail.Visible = _sidebarCollapsed;
        if (_sidebarToolPanel != null)
            _sidebarToolPanel.Visible = !_sidebarCollapsed;
        if (_lblUserInline != null)
            _lblUserInline.Visible = !_sidebarCollapsed;
    }

    /// <summary>
    /// 조회/입력/삭제/행추가/행삭제/저장/출력을 카드로 묶지 않고 각각 독립된 배지 버튼으로
    /// 헤더에 나란히 배치한다. 예전엔 조회/입력/저장을 하나의 카드로 묶고 저장만 강조색을
    /// 꽉 채워 표시했는데, 그룹핑 자체가 산만하다는 피드백에 따라 전부 개별 버튼으로 풀고
    /// 저장도 다른 아이콘과 같은 스타일(연한 배지 + 강조색 아이콘)로 통일했다.
    /// 배지 배경은 순백색 대신 헤더색을 살짝 섞은 연한 톤(IconBadgeBg)을 써서 튀어 보이지
    /// 않게 했다. "성격" 구분은 이제 아이콘 색으로만 표현한다(삭제/행삭제=빨강, 나머지=브랜드 강조색).
    /// 구분선(|) 없이 전부 한 줄로 이어서 배치한다 - 홈은 화면 전환용이라 별도로 사이드바
    /// 메뉴트리 위(ConfigureSidebarTopGap)로 옮겼고, 남은 7개는 전부 한 화면 안에서 이어지는
    /// 데이터 액션이라 사이를 나눌 필요가 없다는 피드백에 따름.
    /// 클릭하면 현재 활성화된 MDI 자식폼(ActiveMdiChild)의 표준 액션(BaseForm.QueryClick 등)을 호출한다.
    /// </summary>
    private void BuildToolbar()
    {
        var iconAccent = ActionAccent;
        var badgeBg = IconBadgeBg;

        var x = 228;
        AddIconBadgeButton(headerPanel, ref x, 6, "조회", ToolbarIconPainters.Query, badgeBg, iconAccent, false, f => f.QueryClick());
        AddIconBadgeButton(headerPanel, ref x, 6, "입력", ToolbarIconPainters.New, badgeBg, iconAccent, false, f => f.NewClick());
        AddIconBadgeButton(headerPanel, ref x, 6, "삭제", ToolbarIconPainters.Delete, badgeBg, DangerColor, false, f => f.DeleteClick());
        AddIconBadgeButton(headerPanel, ref x, 6, "행추가", ToolbarIconPainters.RowAdd, badgeBg, iconAccent, false, f => f.NewRowClick());
        AddIconBadgeButton(headerPanel, ref x, 6, "행삭제", ToolbarIconPainters.RowDelete, badgeBg, DangerColor, false, f => f.DeleteRowClick());
        AddIconBadgeButton(headerPanel, ref x, 6, "저장", ToolbarIconPainters.Save, badgeBg, iconAccent, false, f => f.SaveClick());
        AddIconBadgeButton(headerPanel, ref x, 6, "출력", ToolbarIconPainters.Print, badgeBg, iconAccent, false, f => f.PrintClick());
    }

    private static readonly Size ButtonSize = new(54, 48);

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
            Size = ButtonSize
        };
        toolbarToolTip.SetToolTip(btn, text);

        btn.Click += async (s, e) =>
        {
            // 홈 버튼처럼 활성화면이 없어도 동작해야 하는 경우를 위해 null 허용 폼으로 처리
            var activeForm = ActiveMdiChild as BaseForm;
            if (activeForm == null && text != "홈")
            {
                AppMessageBox.Show("먼저 작업할 화면을 열어주세요.", "안내");
                return;
            }

            try
            {
                activeForm?.ShowBusy();
                await action(activeForm!);
            }
            catch (Exception ex)
            {
                AppMessageBox.Show($"[{text}] 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                activeForm?.HideBusy();
            }
        };

        container.Controls.Add(btn);
        x += btn.Width;
    }

    private void AddDivider(ref int x)
    {
        var divider = new Panel { Location = new Point(x, 14), Size = new Size(1, 32), BackColor = HeaderDividerColor };
        headerPanel.Controls.Add(divider);
        x += 12;
    }

    /// <summary>
    /// 헤더 우측 - 전역검색(Ctrl+K와 동일 동작) 아이콘, 현재 접속 환경(운영/개발) 표시,
    /// 로그인 사용자 아바타(이니셜)를 배치한다. 예전엔 헤더 우측이 통째로 비어있어서 밋밋해
    /// 보인다는 피드백에 따라 추가 - 환경/사용자명 자체는 이미 사이드바 하단/상단에도 있지만,
    /// 여기 배지는 화면 전환 없이 항상 눈에 들어오는 요약 정보 역할.
    /// Dock=Right는 나중에 추가한 컨트롤일수록 더 오른쪽 끝을 차지한다(이 파일 전체의 규칙) -
    /// 그래서 왼쪽부터 보이길 원하는 순서(검색/환경/아바타)의 역순으로 추가한다.
    /// </summary>
    private void BuildHeaderRight()
    {
        headerRightPanel.BackColor = HeaderBg;
        // Fill(스페이서)을 가장 먼저 둬서 나머지 빈 공간을 흡수 - headerRightPanel 자체 폭(210)과
        // 무관하게 아래 항목들이 항상 오른쪽 정렬로 보이게 한다.
        headerRightPanel.Controls.Add(new Panel { Dock = DockStyle.Fill });

        var searchButton = new IconBadgeButton
        {
            IconPainter = ToolbarIconPainters.Query,
            BadgeColor = IconBadgeBg,
            AccentColor = ActionAccent,
            Size = new Size(36, 36),
            IconInset = 8
        };
        toolbarToolTip.SetToolTip(searchButton, "화면 검색 (Ctrl+K)");
        searchButton.Click += (s, e) => OpenQuickMenuSearch();
        AddHeaderRightItem(searchButton, rightPadding: 10);

        lblEnvBadge.AutoSizeMode = LabelAutoSizeMode.None;
        lblEnvBadge.Size = new Size(72, 24);
        lblEnvBadge.Appearance.Font = AppFonts.Caption;
        lblEnvBadge.Appearance.ForeColor = Color.White;
        lblEnvBadge.Appearance.BackColor = Color.FromArgb(255, 255, 255, 40);
        lblEnvBadge.Appearance.Options.UseBackColor = true;
        lblEnvBadge.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblEnvBadge.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        void RefreshEnvBadge() => lblEnvBadge.Text = GetEnvLabel(AppConfig.CurrentEnvironment);
        RefreshEnvBadge();
        AppConfig.EnvironmentChanged += RefreshEnvBadge;
        AddHeaderRightItem(lblEnvBadge, rightPadding: 12);

        avatarBadge.BackColor = Color.White;
        // Panel은 기본적으로 사각형이라, 원형 아바타처럼 보이게 그리기 영역 자체를 원으로 잘라낸다
        using (var circlePath = new System.Drawing.Drawing2D.GraphicsPath())
        {
            circlePath.AddEllipse(0, 0, avatarBadge.Width, avatarBadge.Height);
            avatarBadge.Region = new Region(circlePath);
        }
        lblAvatarInitial.Dock = DockStyle.Fill;
        lblAvatarInitial.AutoSizeMode = LabelAutoSizeMode.None;
        lblAvatarInitial.Appearance.Font = AppFonts.SubHeading;
        lblAvatarInitial.Appearance.ForeColor = _accentColor;
        lblAvatarInitial.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblAvatarInitial.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        avatarBadge.Controls.Add(lblAvatarInitial);

        void RefreshAvatar()
        {
            var user = SessionManager.Current.UserInfo;
            lblAvatarInitial.Text = !string.IsNullOrEmpty(user?.UserNm) ? user!.UserNm.Substring(0, 1) : "?";
            var time = SessionManager.Current.SignInTime is { } t ? $" [{t:yyyy-MM-dd HH:mm} 로그인]" : string.Empty;
            toolbarToolTip.SetToolTip(avatarBadge, $"{user?.UserNm}{time}");
            toolbarToolTip.SetToolTip(lblAvatarInitial, $"{user?.UserNm}{time}");
        }
        RefreshAvatar();
        AppConfig.EnvironmentChanged += RefreshAvatar;
        AddHeaderRightItem(avatarBadge, rightPadding: 16);
    }

    /// <summary>
    /// 작은 컨트롤(검색버튼/배지/아바타)을 헤더 우측에 배치 - Dock=Right는 컨트롤 높이를
    /// 부모(headerPanel, Height=60) 전체로 늘려버려서 작은 원형 아바타 등이 찌그러지므로,
    /// Dock 대신 폭 고정 래퍼 패널을 만들고 그 안에서 세로 중앙 정렬만 좌표로 계산한다.
    /// headerPanel.Height는 실행 중 바뀌지 않아 한 번만 계산해도 안전하다.
    /// </summary>
    private void AddHeaderRightItem(Control control, int rightPadding)
    {
        var wrapper = new Panel { Dock = DockStyle.Right, Width = control.Width + rightPadding, BackColor = HeaderBg };
        control.Location = new Point(0, (headerPanel.Height - control.Height) / 2);
        wrapper.Controls.Add(control);
        headerRightPanel.Controls.Add(wrapper);
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

        var confirm = AppMessageBox.Show(
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
            Show();
            loginForm.Splash?.Close();
        }
        else
        {
            Application.Exit();
        }
    }

    /// <summary>ICON_NM(DB) -> 실제 라인아이콘 매핑. 매칭 안 되면 기본 폴더 아이콘.</summary>
    private static readonly Dictionary<string, Action<Graphics, Rectangle, Color>> TopMenuIcons = new(StringComparer.OrdinalIgnoreCase)
    {
        ["settings"] = MenuIconPainters.Settings,
        ["shoppingcart"] = MenuIconPainters.Cart,
        ["tools"] = MenuIconPainters.Tools,
    };

    /// <summary>
    /// SessionManager에 캐싱된 메뉴권한(MenuDto) 목록으로 Accordion 트리를 재귀 구성.
    /// MENU_TYPE = GROUP 이면 상위그룹, FORM이면 클릭 시 화면 오픈.
    /// </summary>
    private void BuildAccordionMenu()
    {
        var menus = SessionManager.Current.Menus.Where(m => m.ViewYn).ToList();
        var topMenus = menus.Where(m => m.UpperMenuCd == null).OrderBy(m => m.SortOrder);

        sidebarIconRail.Controls.Clear();
        var railY = 8;

        foreach (var top in topMenus)
        {
            var group = new AccordionControlElement
            {
                Text = top.MenuNm,
                Name = top.MenuCd,
                Style = ElementStyle.Group
            };

            var painter = (top.IconNm != null && TopMenuIcons.TryGetValue(top.IconNm, out var p)) ? p : MenuIconPainters.Folder;
            group.ImageOptions.Image = MenuIconPainters.Render(painter, MenuTopIconSize, NavText);
            AddSidebarRailButton(top.MenuNm, painter, ref railY);

            // 최상위 항목 - 다크 배경 위에 아이콘 + 굵은 밝은 글씨. 개별 배경색은 주지 않고
            // 사이드바 바탕색을 그대로 살려서 평평한 리스트처럼 보이게 한다.
            group.Appearance.Normal.BackColor = NavDarkBg;
            group.Appearance.Normal.ForeColor = NavText;
            group.Appearance.Normal.Font = AppFonts.SubHeading;
            group.Appearance.Normal.Options.UseBackColor = true;
            group.Appearance.Normal.Options.UseForeColor = true;
            group.Appearance.Normal.Options.UseFont = true;

            group.Appearance.Hovered.BackColor = NavHoverBg;
            group.Appearance.Hovered.ForeColor = Color.White;
            group.Appearance.Hovered.Font = AppFonts.SubHeading;
            group.Appearance.Hovered.Options.UseBackColor = true;
            group.Appearance.Hovered.Options.UseForeColor = true;
            group.Appearance.Hovered.Options.UseFont = true;

            AddChildMenus(group, menus, top.MenuCd);
            accordionMenu.Elements.Add(group);
        }
    }

    /// <summary>
    /// 접힌 사이드바(아이콘 레일)에 최상위 메뉴 하나당 아이콘 버튼 하나를 세로로 쌓아 배치.
    /// 하위 메뉴까지 좁은 폭에 담기는 어려워, 클릭하면 그냥 사이드바를 펼치는 것으로 위임한다.
    /// </summary>
    private void AddSidebarRailButton(string tooltipText, Action<Graphics, Rectangle, Color> painter, ref int y)
    {
        const int size = 40;
        var btn = new Panel
        {
            Size = new Size(size, size),
            Location = new Point((SidebarCollapsedWidth - size) / 2, y),
            BackColor = NavDarkBg,
            Cursor = Cursors.Hand
        };
        var pic = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.CenterImage,
            Image = MenuIconPainters.Render(painter, MenuTopIconSize + 4, NavText),
            Cursor = Cursors.Hand
        };
        btn.Controls.Add(pic);
        toolbarToolTip.SetToolTip(pic, tooltipText);

        void ExpandSidebar(object? s, EventArgs e)
        {
            if (_sidebarCollapsed) ToggleSidebarCollapsed();
        }
        void Hover(object? s, EventArgs e) => btn.BackColor = NavHoverBg;
        void Unhover(object? s, EventArgs e) => btn.BackColor = NavDarkBg;

        btn.Click += ExpandSidebar;
        pic.Click += ExpandSidebar;
        btn.MouseEnter += Hover;
        pic.MouseEnter += Hover;
        btn.MouseLeave += Unhover;
        pic.MouseLeave += Unhover;

        sidebarIconRail.Controls.Add(btn);
        y += size + 8;
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
                // 2단계 이하 그룹(소분류) - 클릭해서 화면이 열리는 게 아니라 펼치기만 하는
                // "구획 라벨"이라는 걸 보여주려고, 오히려 화면(leaf)보다 차분한(muted) 색으로
                // 낮춘다. 굵은 글씨는 유지해서 "헤더"라는 느낌은 남긴다.
                element.Appearance.Normal.BackColor = NavDarkBg;
                element.Appearance.Normal.ForeColor = NavTextMuted;
                element.Appearance.Normal.Font = AppFonts.BodyBold;
                element.Appearance.Normal.Options.UseBackColor = true;
                element.Appearance.Normal.Options.UseForeColor = true;
                element.Appearance.Normal.Options.UseFont = true;

                element.Appearance.Hovered.BackColor = NavHoverBg;
                element.Appearance.Hovered.ForeColor = Color.White;
                element.Appearance.Hovered.Font = AppFonts.BodyBold;
                element.Appearance.Hovered.Options.UseBackColor = true;
                element.Appearance.Hovered.Options.UseForeColor = true;
                element.Appearance.Hovered.Options.UseFont = true;
            }
            else
            {
                // 실제 클릭 가능한 화면(FORM) - 들여쓰기를 깊게 하지 않고도 그룹과 구분되도록
                // 작은 점 불릿(강조색)을 붙이고, 글자색도 그룹보다 밝게 해서 "여기가 실제
                // 이동 가능한 화면"이라는 게 한눈에 보이게 한다. 호버 시 살짝 밝아지는 배경으로
                // 클릭 가능함을 한 번 더 보강.
                element.ImageOptions.Image = MenuIconPainters.Render(MenuIconPainters.Dot, MenuLeafDotSize, ActionAccent);
                element.Appearance.Normal.BackColor = NavDarkBg;
                element.Appearance.Normal.ForeColor = NavText;
                element.Appearance.Normal.Font = AppFonts.Body;
                element.Appearance.Normal.Options.UseBackColor = true;
                element.Appearance.Normal.Options.UseForeColor = true;
                element.Appearance.Normal.Options.UseFont = true;

                element.Appearance.Hovered.BackColor = NavHoverBg;
                element.Appearance.Hovered.ForeColor = Color.White;
                element.Appearance.Hovered.Font = AppFonts.BodyBold;
                element.Appearance.Hovered.Options.UseBackColor = true;
                element.Appearance.Hovered.Options.UseForeColor = true;
                element.Appearance.Hovered.Options.UseFont = true;

                // 클릭(선택) 상태 - 폰트 크기/굵기/배경은 전부 Normal과 동일하게 유지하고
                // 글자색만 옅은 골드톤으로 바꿔서 "선택됨"을 표시한다. 예전엔 Font/UseFont를
                // 따로 안 줘서 Pressed일 때 스타일 안 먹은 기본(더 작아 보이는) 폰트로
                // 떨어지는 버그가 있었다 - Normal과 같은 AppFonts.Body를 명시해서 고침.
                element.Appearance.Pressed.BackColor = NavDarkBg;
                element.Appearance.Pressed.ForeColor = Color.FromArgb(255, 205, 86);
                element.Appearance.Pressed.Font = AppFonts.Body;
                element.Appearance.Pressed.Options.UseBackColor = true;
                element.Appearance.Pressed.Options.UseForeColor = true;
                element.Appearance.Pressed.Options.UseFont = true;
            }

            AddChildMenus(element, allMenus, child.MenuCd, visited);
            parent.Elements.Add(element);

            visited.Remove(child.MenuCd); // 형제 메뉴 처리를 위해 이 가지에서만 빠져나오면 복원
        }
    }

    /// <summary>더블클릭한 지점이 실제 화면(Item) 엘리먼트일 때만 그 메뉴를 연다 - 그룹
    /// 헤더를 더블클릭하면 DevExpress 기본 동작(펼침/접힘)만 두 번 일어날 뿐, 화면이 열리진
    /// 않는다. AccordionControlElement 자체엔 DoubleClick 이벤트가 없어서 컨트롤 레벨에서
    /// 좌표로 히트테스트한다.</summary>
    private void AccordionMenu_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        var hitInfo = accordionMenu.CalcHitInfo(e.Location);
        if (hitInfo.HitTest != AccordionControlHitTest.Item) return;

        var element = hitInfo.ItemInfo?.Element;
        if (element?.Name == null) return;

        var menu = SessionManager.Current.GetMenuAuth(element.Name);
        if (menu != null) OpenMenuForm(menu);
    }

    /// <summary>
    /// Ctrl+K - 전역 메뉴 빠른 검색 팔레트. MDI 자식(그리드 등)에 포커스가 있어도 잡히도록
    /// ProcessCmdKey에서 가로챈다(KeyDown은 포커스를 가진 자식 컨트롤이 먼저 소비해버릴 수 있음).
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.K))
        {
            OpenQuickMenuSearch();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void OpenQuickMenuSearch()
    {
        var leafMenus = SessionManager.Current.Menus
            .Where(m => m.ViewYn && m.MenuType != "GROUP" && !string.IsNullOrWhiteSpace(m.FormClassNm))
            .ToList();
        if (leafMenus.Count == 0) return;

        using var search = new QuickMenuSearchForm(leafMenus);
        if (search.ShowDialog(this) == DialogResult.OK && search.Result != null)
        {
            OpenMenuForm(search.Result);
        }
    }

    /// <summary>HomeForm 대시보드의 바로가기/최근사용 카드에서 호출 - 메뉴코드로 화면을 연다</summary>
    public void OpenMenuByCode(string menuCd)
    {
        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.MenuCd == menuCd);
        if (menu == null)
        {
            AppMessageBox.Show("연결된 메뉴를 찾을 수 없습니다.", "안내");
            return;
        }
        OpenMenuForm(menu);
    }

    /// <summary>
    /// MENU.FORM_CLASS_NM (예: "WYNLAB.Modules.System.UserListForm, WYNLAB.Modules.System") 을
    /// 리플렉션으로 로딩해서 MDI 자식으로 오픈. 이미 열려있으면 해당 탭으로 포커스만 이동.
    /// </summary>
    private void OpenMenuForm(MenuDto menu)
    {
        var formClassNm = menu.FormClassNm;
        if (string.IsNullOrWhiteSpace(formClassNm))
        {
            AppMessageBox.Show("연결된 화면이 없습니다. (FORM_CLASS_NM 미설정)", "안내");
            return;
        }

        var existing = MdiChildren.FirstOrDefault(f => f.Name == menu.MenuCd);
        if (existing != null)
        {
            var confirm = AppMessageBox.Show(
                $"'{menu.MenuNm}' 화면이 이미 열려있습니다.\n신규로 오픈하시겠습니까?",
                "화면 중복", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.No)
            {
                existing.Activate();
                return;
            }
            // "예"를 선택하면 아래로 내려가서 새 인스턴스를 만든다 (기존 탭은 그대로 둠)
        }

        // 메뉴를 열 때마다 Modules 폴더의 dll이 그 사이 바뀌었는지 확인하고, 바뀌었으면 최신
        // 코드로 다시 로드한다(ModuleLoader 클래스 설명 참고) - 재로그인 없이도 배포 직후
        // 바로 반영된다. 이미 열려있던 기존 인스턴스는 자기가 만들어질 때의 코드로 계속
        // 동작하고, 이 메뉴를 "새로" 여는 순간부터만 최신 코드가 적용된다.
        //
        // 반드시 EnsureLoaded가 돌려준 Assembly 객체에서 직접 GetType(타입명)을 호출해야
        // 한다 - Type.GetType(전체문자열)을 쓰면 CLR이 "WYNLAB.SM.CODE" 같은 어셈블리
        // 단순 이름의 첫 해석 결과를 내부적으로 캐싱해버려서, 두 번째 열 때부터는
        // EnsureLoaded로 최신 dll을 새로 읽어와도 무시되고 계속 예전 화면이 뜬다(실제로
        // 겪은 버그 - ModuleLoader 클래스 설명 참고).
        var nameParts = formClassNm.Split(',');
        var typeName = nameParts[0].Trim();
        var assembly = nameParts.Length > 1 ? ModuleLoader.EnsureLoaded(nameParts[1].Trim()) : null;

        var formType = assembly?.GetType(typeName);
        if (formType == null || Activator.CreateInstance(formType) is not BaseForm form)
        {
            AppMessageBox.Show($"화면을 찾을 수 없습니다: {menu.FormClassNm}", "오류");
            return;
        }

        form.Name = menu.MenuCd;
        form.MenuCd = menu.MenuCd;
        form.MdiParent = this;
        form.Show();

        SessionManager.Current.AddRecentMenu(menu);
    }
}
