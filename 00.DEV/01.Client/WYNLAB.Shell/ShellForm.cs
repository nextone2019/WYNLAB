using DevExpress.XtraBars.Navigation;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
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

    // 모던 리프레시(2026-08-27): 예전엔 헤더 전체를 브랜드색으로 짙게 물들였는데(Mix 0.35),
    // "브랜드색 배경 + 색색의 배지 버튼"이 화면 전체를 시끄럽게 만든다는 피드백으로 헤더를
    // 거의 흰색에 가까운 중립 면으로 바꿨다 - 브랜드색은 이제 로고 배지 하나에만 진하게 쓰고
    // (아래 BuildLogo), 나머지는 전부 중립 회색조로 눌러서 "어디에 힘을 줄지"를 명확히 했다.
    // 0.94 정도로 아주 살짝만 브랜드색을 섞어두는 이유는 완전한 회백색보다 이 앱만의 톤이라는
    // 인상을 은은하게 남기기 위함 - 육안으로는 거의 안 보이지만 다른 브랜드색으로 바꾸면
    // 헤더 톤도 같이 미세하게 따라간다.
    private Color HeaderBg => ColorHelper.Mix(_accentColor, Color.White, 0.94f);
    private Color HeaderDividerColor => ColorHelper.Adjust(HeaderBg, -22);
    // 아이콘 배지: 예전엔 헤더와 거의 같은 톤으로 눌러서 튀지 않게 했는데, 헤더 자체가 이제
    // 거의 흰색이라 그 방식 그대로면 배지가 안 보이게 된다. 살짝 어둡게 눌러 중립 회색 "칩"으로
    // 만들어서 클릭 가능한 영역임이 눌러보지 않아도 눈에 들어오게 한다.
    private Color IconBadgeBg => ColorHelper.Adjust(HeaderBg, -8);
    private static readonly Color SidebarBg = Color.FromArgb(245, 246, 248);
    private static readonly Color DividerColor = Color.FromArgb(225, 225, 225);
    // 헤더 위 일반 텍스트/아이콘 색 - 예전엔 헤더가 짙어서 흰 글자를 썼는데, 이제 헤더가
    // 밝아졌으니 짙은 중립색으로 뒤집는다.
    private static readonly Color HeaderText = Color.FromArgb(28, 30, 34);
    private static readonly Color HeaderTextMuted = Color.FromArgb(120, 123, 130);
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

    /// <summary>사이드바 맨 위 사용자정보 띠의 배경. 위(타이틀바)도 아래(메뉴트리)도 짙은 색이라
    /// 여기만 옅게 둬서 구분되게 한다. 고정색이 아니라 브랜드색을 아주 옅게 섞은 값이라,
    /// 회사별로 ToolbarColor가 바뀌면 이 띠도 같은 계열로 따라간다.</summary>
    private Color SidebarUserBg => ColorHelper.Mix(_accentColor, Color.White, 0.93f);

    private Color NavDarkBg => ColorHelper.Mix(_accentColor, Color.Black, 0.45f);
    // 밝기만 올리는 Adjust는 이미 어두운 남색(NavDarkBg)에 회색을 끼얹은 것처럼 보여 "밝은 파랑"으로
    // 안 읽힌다는 피드백 - 채도 있는 파랑(NavAccentBlue) 쪽으로 섞어야 눈에 띄게 파래진다.
    private static readonly Color NavAccentBlue = Color.FromArgb(43, 130, 255);
    private Color NavHoverBg => ColorHelper.Mix(NavDarkBg, NavAccentBlue, 0.55f);
    private Color NavPressedBg => ColorHelper.Mix(NavDarkBg, NavAccentBlue, 0.8f);
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

    /// <summary>헤더의 화면 검색 콤보 - 타이핑하면 목록이 걸러지고, 고르면 그 화면이 열린다.
    /// 예전엔 별도 검색창(QuickMenuSearchForm)을 Ctrl+K로 띄웠는데, 이 콤보가 사이드바에
    /// 항상 보이는 자리로 옮겨오면서 그 창과 전용 단축키 모두 없앴다.</summary>
    private readonly LookUpEdit cboMenuSearch = new();
    private readonly Panel sidebarPanel;
    private readonly Panel statusBar;
    // 접힌 상태에서 accordionMenu 대신 보여주는 아이콘 전용 레일 - 최상위 메뉴당 아이콘 버튼 1개.
    // 클릭하면 사이드바를 다시 펼친다(하위 메뉴까지 좁은 폭에 다 담기는 어려워, 펼침으로 위임).
    private readonly Panel sidebarIconRail = new() { Dock = DockStyle.Fill, Visible = false };
    private Panel? _sidebarToolPanel;
    /// <summary>사이드바를 접었을 때 같이 숨기는 사용자정보 영역(사용자명 + 접속시각 두 줄).
    /// 라벨 하나가 아니라 둘을 담은 그릇을 들고 있어야 두 줄이 함께 사라진다.</summary>
    private Control? _lblUserInline;

    /// <summary>홈 버튼을 담은 영역 - 사이드바가 접히면 Dock을 Right에서 Fill로 바꿔
    /// 좁아진 폭 한가운데로 보낸다(ToggleSidebarCollapsed 참고).</summary>
    private Panel? _homeArea;
    // 사이드바 맨 위 여백 - 오른쪽 MDI 탭 줄과 높이를 맞춰서, 그 아래(사용자정보)와
    // 탭 줄 아래(문서 내용)가 같은 Y좌표에서 시작하도록 CustomDrawTabHeader에서 실측해 맞춘다.
    /// <summary>사이드바 맨 위, 로그인 사용자/접속시각을 두 줄로 보여주는 띠.
    /// 배경색과 위/아래 흰 경계선은 ConfigureSidebarTopGap에서 입힌다.</summary>
    private readonly Panel sidebarTopGap = new() { Dock = DockStyle.Top, Height = SidebarTopGapMinHeight };

    /// <summary>사용자명/접속시각 두 줄 + 위아래 경계선 1px씩이 눌리지 않고 들어가는 최소 높이.
    /// 이 띠는 원래 오른쪽 탭 줄 높이를 그대로 따라가지만(양쪽 시작 Y를 맞추려고), 한 줄이던
    /// 시절 기준이라 두 줄에는 모자란다 - 그래서 이 값보다는 작아지지 않게 한다.</summary>
    private const int SidebarTopGapMinHeight = 46;
    private readonly LabelControl lblStatusMessage = new();
    private readonly LabelControl lblStatusRight = new();
    private readonly ComboBoxEdit cboEnvironment = new();
    private readonly ComboBoxEdit cboSkin = new();
    // 순정 WinForms ToolTip은 OS 시스템 폰트(Segoe UI)로 그려져서 앱 나머지가 다 쓰는
    // Malgun Gothic과 섞이면 툴팁만 눈에 띄게 어긋나 보인다 - DevExpress 컨트롤러를 쓰면
    // 스킨/폰트를 앱과 맞출 수 있다. SetToolTip(Control, text) API가 동일해서 호출부는 그대로 둔다.
    private readonly DevExpress.Utils.ToolTipController toolbarToolTip = new();

    // SQL/API 활동 로그 뷰어(관리자 전용) - null이면 관리자가 아니라서 아예 안 만들어진 상태.
    private Panel? _sqlLogPanel;
    private SimpleButton _btnSqlLogStartStop = null!;
    private LabelControl _lblSqlLogCount = null!;
    private MemoEdit _memoSqlLogDetail = null!;

    /// <summary>
    /// 제품 기본 테마. Program.cs가 앱 시작 시 이 값으로 스킨을 지정하고, 아래 테마 콤보의
    /// 초기 선택값도 결국 이 값이 된다 - 두 군데에 문자열을 따로 적어두면 한쪽만 바꾸고
    /// 다른 쪽을 깜빡하기 쉬워서 한 곳에서만 정의한다.
    /// </summary>
    public const string DefaultSkin = "WXI";

    /// <summary>
    /// 사용자가 고를 수 있는 테마 목록. DevExpress에는 스킨이 수십 개 있어 전부 나열하면
    /// 오래되거나 브랜드와 안 어울리는 것도 섞여 들어가므로, 최근/모던한 스킨만 선별했다.
    /// </summary>
    private static readonly string[] AvailableSkins =
    {
        DefaultSkin,
        "Office 2019 Colorful", "Office 2019 Black", "Office 2019 Dark Gray", "Office 2019 White",
        "Visual Studio 2019 Blue", "Visual Studio 2019 Dark", "WXI", "Basic"
    };

    private readonly AccordionControl accordionMenu = new() { Dock = DockStyle.Fill };
    private readonly XtraTabbedMdiManager tabbedMdiManager = new();
    // 탭 우클릭 시 닫기/다른 탭 모두 닫기/모두 닫기 메뉴 - PermissionAssignForm의 copyMenu와
    // 같은 방식으로 클릭할 때마다 Items를 비우고 다시 채운다(대상이 매번 다른 탭이라서).
    private readonly ContextMenuStrip tabContextMenu = new();
    private HomeForm? _homeForm;

    // 예전엔 헤더 툴바 맨 앞에 있었는데, 메뉴트리 바로 위(사이드바 상단 여백)로 옮기고
    // 크기도 작게 줄였다 - 다른 업무 액션들과 성격이 달라서(화면 전환이지 데이터 액션이 아님)
    // 메뉴트리와 더 가까운 자리가 자연스럽다는 피드백.
    // IconInset을 기본값(9, 헤더 54x48 버튼 기준)보다 줄여야 이 작은 크기에서도 아이콘이
    // 실제로 보인다 - 처음엔 기본값 그대로 썼다가 아이콘이 점처럼 작아져 거의 안 보였다.
    // 배치는 ConfigureSidebarTopGap 참고(Dock=Right로 직접 붙이면 세로로 늘어난다).
    private readonly IconBadgeButton homeButton = new()
    {
        Text = "홈",
        Size = new Size(34, 34),
        IconInset = 6
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

        toolbarToolTip.Appearance.Font = AppFonts.Caption;
        toolbarToolTip.Appearance.Options.UseFont = true;

        // 가로: 현재 헤더 툴바(조회/입력/삭제/행추가/행삭제/저장/출력 7개를 세 묶음으로 띄워
        // 한 줄에 배치)가 겹치지 않고 다 보이는 최소폭. 홈 버튼은 사이드바 메뉴트리 위로 옮겨서
        // 헤더에서 빠졌다.
        // 세로도 업무화면이 너무 눌리지 않도록 최소값을 둠.
        MinimumSize = new Size(920, 650);

        tabbedMdiManager.MdiParent = this;
        // MDI 탭 헤더에 X(닫기) 버튼 표시 + 탭 영역 맨 오른쪽에 "현재 탭 닫기" 버튼도 같이
        // 표시(InAllTabPagesAndTabControlHeader) - Home 탭은 BaseForm/HomeForm.OnFormClosing에서
        // 이미 닫기를 막고 있어서, 두 버튼 다 눌러도 실제로는 안 닫힌다.
        tabbedMdiManager.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
        ConfigureTabAppearance();

        headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = HeaderBg };
        // 관리자가 IconAssetProvider.AssetsFolder에 toolbar_background.png를 넣어두면 그 위에
        // 그려서 배경을 이미지로 바꿀 수 있게 한다 - 없으면(기본 상태) 그냥 BackColor(HeaderBg)
        // 그대로 보인다. Dock=Fill처럼 헤더 전체 크기에 맞춰 늘려 그린다.
        headerPanel.Paint += (s, e) =>
        {
            var bg = IconAssetProvider.GetImage("toolbar_background");
            if (bg != null) e.Graphics.DrawImage(bg, headerPanel.ClientRectangle);
        };
        logoPanel = new Panel { BackColor = HeaderBg, Cursor = Cursors.Hand, Width = 212, Dock = DockStyle.Left };
        sidebarPanel = new Panel { Dock = DockStyle.Left, Width = 212, BackColor = NavDarkBg };
        statusBar = new Panel { Dock = DockStyle.Bottom, Height = 26, BackColor = SidebarBg };

        BuildLogo();
        BuildToolbar();
        BuildSidebar();
        BuildAccordionMenu();
        BuildStatusBar();

        headerPanel.Controls.Add(logoPanel);

        var headerBottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = HeaderDividerColor };
        headerPanel.Controls.Add(headerBottomBorder);

        // 관리자만 SQL 로그 패널을 만든다(BuildSqlLogPanel 클래스 설명 참고) - statusBar보다
        // 먼저 Controls에 들어가야(BuildSqlLogPanel 내부에서 처리) 패널이 상태바 위쪽에 온다.
        if (Session.UserType == "A") BuildSqlLogPanel();

        // 순서 중요: 상단바를 먼저 추가해야 전체 폭을 차지하고, 그 아래에 좌측 메뉴가 배치됨
        // statusBar는 Bottom이라 순서 무관하게 항상 최하단에 고정됨
        Controls.Add(statusBar);
        Controls.Add(sidebarPanel);
        Controls.Add(headerPanel);

        AppConfig.EnvironmentChanged += RefreshTitle;

        FormClosing += ShellForm_FormClosing;

        // 활성 업무화면이 바뀔 때마다(다른 탭 클릭, 화면 열기/닫기 등) 툴바 아이콘의
        // 활성/비활성을 그 화면의 권한으로 다시 계산한다.
        MdiChildActivate += (s, e) => UpdateToolbarPermissions();

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

        // 열린 탭이 많아지면 원하는 화면을 눈으로 찾기 힘들다는 피드백 - DevExpress 내장
        // "문서 선택기"를 켜서 Ctrl+Tab으로 열린 화면 갤러리를 띄울 수 있게 한다(파워유저용
        // 보조 단축키). 단, 이 버전은 클릭할 수 있는 버튼을 화면에 그려주지 않고 키보드로만
        // 열리는 방식이라(리플렉션으로 XtraTabbedMdiManager 전체 멤버를 확인했지만 버튼을
        // 노출하는 프로퍼티나 팝업을 직접 여는 메서드가 없음 - 실제로 겪음, 처음엔 버튼이
        // 저절로 생기는 줄 알았다), 눈에 보이는 진입점은 아래 BuildToolbar의 "탭 목록" 버튼 +
        // ShowTabListPopup(직접 구현한 검색 팝업)이 맡는다.
        tabbedMdiManager.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;

        // 탭 우클릭 -> 닫기/다른 탭 모두 닫기/모두 닫기. XtraTabbedMdiManager는 이 세 가지를
        // 기본 제공하지 않아서(HeaderButtons enum엔 Prev/Next/Close만 있음) MouseUp에서 직접
        // 히트테스트해서 만든다.
        tabbedMdiManager.MouseUp += TabbedMdiManager_MouseUp;

        // XtraTabbedMdiManager엔 탭 텍스트 좌우 여백을 조절하는 속성이 없어서(리플렉션으로
        // 전체 속성을 확인했지만 Padding/Indent/최소너비 같은 훅이 전혀 없었다), 탭이 텍스트
        // 길이에 딱 맞게 폭을 계산하는 걸 역으로 이용한다 - 탭에 표시되는 문자열 앞뒤에 공백을
        // 붙이면 그만큼 탭 자체가 넓어지면서 자연스럽게 여백처럼 보인다. form.Text(창 제목,
        // BuildScreenHeader의 화면 타이틀)는 그대로 두고, MDI 탭 전용 Text만 별도로 바꾼다.
        tabbedMdiManager.PageAdded += (s, e) =>
        {
            e.Page.Text = $"  {e.Page.Text}  ";

            // 새 탭은 기본적으로 열려있는 탭들 맨 오른쪽에 추가되는데, 그러면 로그인 직후
            // 맨 먼저 열리는 홈 탭이 항상 맨 앞에 고정되어 자리만 차지한다는 피드백 - 새로
            // 여는 탭이 항상 맨 왼쪽(0번)에 오도록 바꿔서, 방금 연 화면이 제일 눈에 잘 띄는
            // 자리에 오고 홈 탭은 다른 탭이 열릴 때마다 자연스럽게 뒤로 밀려난다.
            var pages = tabbedMdiManager.Pages;
            var index = pages.IndexOf(e.Page);
            if (index > 0)
            {
                pages.Remove(e.Page);
                pages.Insert(0, e.Page);
            }
        };
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
        // 사용자/접속시각을 두 줄로 넣으면서부터는 탭 줄 높이만으로는 모자랄 수 있어서,
        // 최소 높이를 보장한다(SidebarTopGapMinHeight 설명 참고).
        var rowHeight = Math.Max(e.TabHeaderRowInfo.Bounds.Height, SidebarTopGapMinHeight);
        if (sidebarTopGap.Height != rowHeight)
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
    /// 탭 우클릭 시 그 탭을 대상으로 닫기/다른 탭 모두 닫기/모두 닫기 메뉴를 띄운다.
    /// CalcHitInfo는 매니저가 넘겨준 e.Location(내부 탭 컨트롤 기준 좌표)을 그대로 받는
    /// GridView.CalcHitInfo(e.Location)와 같은 관례라, 별도 좌표변환 없이 바로 쓴다.
    /// </summary>
    private void TabbedMdiManager_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        var hit = tabbedMdiManager.CalcHitInfo(e.Location);
        if (!hit.IsValid || hit.Page is not XtraMdiTabPage page || page.MdiChild is not { } clickedChild) return;

        tabContextMenu.Items.Clear();
        tabContextMenu.Items.Add("닫기", null, (s, ev) => clickedChild.Close());
        tabContextMenu.Items.Add("다른 탭 모두 닫기", null, (s, ev) => CloseAllMdiChildren(except: clickedChild));
        tabContextMenu.Items.Add("모두 닫기", null, (s, ev) => CloseAllMdiChildren(except: null));
        tabContextMenu.Show(Cursor.Position);
    }

    /// <summary>홈 탭은 건드리지 않는다 - 어차피 HomeForm.OnFormClosing이 자체적으로 닫기를
    /// 막고 있지만(개별 X버튼과 동일하게), 막힐 걸 알면서 Close()를 호출하지 않도록 여기서도
    /// 먼저 걸러낸다. MdiChildren은 호출할 때마다 새 배열을 돌려주므로(WinForms 자체 동작)
    /// Close() 중 컬렉션이 바뀌어도 이 foreach 자체는 안전하다.</summary>
    private void CloseAllMdiChildren(Form? except)
    {
        foreach (var child in MdiChildren)
        {
            if (child is HomeForm) continue;
            if (except != null && child == except) continue;
            child.Close();
        }
    }

    /// <summary>"탭 목록" 버튼 클릭 -> 열려있는 화면을 검색해서 바로 이동할 수 있는 작은 팝업을
    /// anchor(버튼) 바로 아래에 띄운다. DevExpress 내장 문서 선택기는 버튼이 없어 Ctrl+Tab으로만
    /// 열리므로(ConfigureTabAppearance 주석 참고), 클릭으로 여는 진입점은 직접 만든다.</summary>
    private void ShowTabListPopup(Control anchor)
    {
        var pages = tabbedMdiManager.Pages.OfType<XtraMdiTabPage>().Where(p => p.MdiChild != null).ToList();
        if (pages.Count == 0) return;

        var popup = new TabListPopup(pages);
        var loc = anchor.PointToScreen(new Point(anchor.Width - popup.Width, anchor.Height + 2));
        popup.Location = loc;
        popup.Show(this);
    }

    /// <summary>
    /// 검색창 + 목록 하나짜리 borderless 팝업. 포커스를 잃으면(Deactivate) 스스로 닫혀서
    /// 클릭 한 번으로 골라 이동하거나, 바깥을 클릭해서 그냥 닫을 수 있다(콤보박스 드롭다운과
    /// 같은 동작 관례).
    /// </summary>
    private sealed class TabListPopup : XtraForm
    {
        private sealed class Entry
        {
            public string Title { get; }
            public XtraMdiTabPage Page { get; }
            public Entry(string title, XtraMdiTabPage page) { Title = title; Page = page; }
            public override string ToString() => Title;
        }

        private readonly TextEdit txtSearch = new();
        private readonly ListBoxControl list = new();
        private readonly List<Entry> _allEntries;

        public TabListPopup(List<XtraMdiTabPage> pages)
        {
            // 탭 텍스트는 ConfigureTabAppearance에서 앞뒤에 공백을 붙여 여백처럼 쓰고 있어서
            // (PageAdded 참고) 검색/표시 둘 다에서 그 공백은 잘라내고 보여준다.
            _allEntries = pages.Select(p => new Entry(p.Text.Trim(), p)).ToList();

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Size = new Size(300, 340);
            Padding = new Padding(1); // 아래 BackColor가 이 1px만큼 테두리처럼 비쳐 보이게

            BackColor = Color.FromArgb(210, 212, 216);

            var body = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Controls.Add(body);

            txtSearch.Dock = DockStyle.Top;
            txtSearch.Height = 30;
            txtSearch.Properties.NullValuePrompt = "화면 검색";
            txtSearch.TextChanged += (s, e) => Refill(txtSearch.Text.Trim());

            list.Dock = DockStyle.Fill;
            list.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            list.DoubleClick += (s, e) => ActivateSelected();

            body.Controls.Add(list);
            body.Controls.Add(txtSearch);

            Refill(string.Empty);

            KeyPreview = true;
            KeyDown += TabListPopup_KeyDown;
            txtSearch.KeyDown += TabListPopup_KeyDown;
            list.KeyDown += TabListPopup_KeyDown;

            Deactivate += (s, e) => Close();
            Shown += (s, e) => txtSearch.Focus();
        }

        private void Refill(string keyword)
        {
            list.Items.Clear();
            var matches = keyword.Length == 0
                ? _allEntries
                : _allEntries.Where(en => en.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            foreach (var entry in matches) list.Items.Add(entry);
            if (list.Items.Count > 0) list.SelectedIndex = 0;
        }

        private void TabListPopup_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    ActivateSelected();
                    e.Handled = true;
                    break;
                case Keys.Down:
                    MoveSelection(1);
                    e.Handled = true;
                    break;
                case Keys.Up:
                    MoveSelection(-1);
                    e.Handled = true;
                    break;
            }
        }

        private void MoveSelection(int delta)
        {
            if (list.Items.Count == 0) return;
            list.SelectedIndex = Math.Max(0, Math.Min(list.Items.Count - 1, list.SelectedIndex + delta));
        }

        private void ActivateSelected()
        {
            if (list.SelectedItem is Entry entry && entry.Page.MdiChild != null)
            {
                entry.Page.MdiChild.Activate();
            }
            Close();
        }
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

    /// <summary>
    /// SQL/API 활동 로그 뷰어(관리자 전용, TSMUSER.USER_TYPE='A') - 이 클라이언트가 api/data/query,
    /// api/data/save로 보낸 요청 내역(ApiCallLog.Entries)을 시간순으로 보여준다. 실제 SQL 텍스트가
    /// 아니라 프로시저명+파라미터인 이유는 ApiCallLog 클래스 설명 참고.
    ///
    /// MDI 탭 영역과 하단 상태바 사이에 Dock=Bottom으로 깔린다. 관리자가 아니면 이 메서드 자체를
    /// 호출하지 않아서 _sqlLogPanel이 null로 남고, 헤더의 토글 버튼도 안 만들어진다(BuildHeaderRight
    /// 참고) - 다른 사용자에게는 버튼도 패널도 존재 자체가 안 보인다.
    ///
    /// "시작/정지"(ApiCallLog.IsCapturing)와 "패널 표시/숨김"은 서로 다른 상태다 - 패널을 닫아도
    /// 시작 상태면 계속 쌓이고, 다시 패널을 열면 그동안 쌓인 내역이 그대로 보인다(요구사항).
    /// </summary>
    private void BuildSqlLogPanel()
    {
        _sqlLogPanel = new Panel { Dock = DockStyle.Bottom, Height = 240, Visible = false, BackColor = Color.White };
        var splitter = new Splitter { Dock = DockStyle.Bottom, Height = 4, BackColor = DividerColor };

        var toolbar = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = SidebarBg };
        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Left, AutoSize = true, FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false, Padding = new Padding(8, 3, 8, 3)
        };

        _btnSqlLogStartStop = new SimpleButton { Text = "시작", Width = 64, Height = 26 };
        _btnSqlLogStartStop.Click += (s, e) => ToggleSqlLogCapturing();

        var btnClear = new SimpleButton { Text = "지우기", Width = 64, Height = 26 };
        btnClear.Click += (s, e) => ApiCallLog.Clear();

        var btnCopy = new SimpleButton { Text = "SQL 복사", Width = 74, Height = 26 };
        btnCopy.Click += (s, e) =>
        {
            if (_memoSqlLogDetail.Text.Length > 0) Clipboard.SetText(_memoSqlLogDetail.Text);
        };

        _lblSqlLogCount = new LabelControl { AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(120, 26) };
        _lblSqlLogCount.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        RefreshSqlLogCount();

        flow.Controls.Add(_btnSqlLogStartStop);
        flow.Controls.Add(btnClear);
        flow.Controls.Add(btnCopy);
        flow.Controls.Add(_lblSqlLogCount);
        toolbar.Controls.Add(flow);

        // 그리드가 선택된 행의 실행 가능한 EXEC 문 전체를 보여주는 하단 메모(읽기전용, 복사만
        // 가능) - 그리드 셀은 한 줄이라 파라미터 많은 프로시저는 다 안 보인다(실제로 겪음).
        _memoSqlLogDetail = new MemoEdit { Dock = DockStyle.Bottom, Height = 90, ReadOnly = true };
        _memoSqlLogDetail.Properties.Appearance.Font = new Font("Consolas", 9f);
        _memoSqlLogDetail.Properties.ScrollBars = ScrollBars.Vertical;

        var grid = BuildSqlLogGrid();

        _sqlLogPanel.Controls.Add(grid);
        _sqlLogPanel.Controls.Add(_memoSqlLogDetail);
        _sqlLogPanel.Controls.Add(toolbar);

        ApiCallLog.Entries.ListChanged += (s, e) => RefreshSqlLogCount();

        // Dock=Bottom 추가 순서(이 파일 전체 규칙): 나중에 추가한 쪽이 진짜 바닥 가장자리에
        // 더 가깝게 붙는다. splitter를 _sqlLogPanel보다 먼저 추가해야 splitter가 MDI 영역과
        // _sqlLogPanel 사이(패널 위쪽)에 오고, _sqlLogPanel이 진짜 바닥을 차지한다 - 순서가
        // 반대면 splitter가 패널 아래(상태바 쪽)로 밀려서 드래그해도 패널 높이가 안 바뀌는
        // 것처럼 보인다(실제로 겪음 - 스플리터가 SQL 로그 패널 아래에 있었다).
        Controls.Add(splitter);
        Controls.Add(_sqlLogPanel);
    }

    private GridControlWyn BuildSqlLogGrid()
    {
        var grid = new GridControlWyn { Dock = DockStyle.Fill, DataSource = ApiCallLog.Entries };
        var view = new GridViewWyn { HighlightFocusedRow = true };
        view.OptionsBehavior.Editable = false;
        view.OptionsView.ShowGroupPanel = false;
        grid.ViewCollection.Add(view);
        grid.MainView = view;

        AddSqlLogColumn(view, nameof(ApiCallLogEntry.Timestamp), "시각", 90);
        view.Columns[nameof(ApiCallLogEntry.Timestamp)].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        view.Columns[nameof(ApiCallLogEntry.Timestamp)].DisplayFormat.FormatString = "HH:mm:ss.fff";

        AddSqlLogColumn(view, nameof(ApiCallLogEntry.MenuCd), "메뉴", 90);
        // 프로시저명/파라미터를 컬럼 두 개로 나누지 않고, SSMS에 바로 붙여넣어 실행할 수 있는
        // EXEC 문 한 줄로 합쳐서 보여준다 - 줄바꿈까지 포함한 전체 형태는 행을 선택했을 때
        // 하단 메모(복사 가능)에 표시된다.
        AddSqlLogColumn(view, nameof(ApiCallLogEntry.SqlPreview), "SQL", 550);
        AddSqlLogColumn(view, nameof(ApiCallLogEntry.Success), "성공", 55);
        AddSqlLogColumn(view, nameof(ApiCallLogEntry.DurationMs), "소요(ms)", 70);
        AddSqlLogColumn(view, nameof(ApiCallLogEntry.Message), "메시지", 250);

        view.FocusedRowObjectChanged += (s, e) =>
            _memoSqlLogDetail.Text = e.Row is ApiCallLogEntry entry ? entry.ToSqlText() : string.Empty;

        return grid;
    }

    private static void AddSqlLogColumn(GridViewWyn view, string fieldName, string caption, int width)
    {
        var column = view.Columns.AddField(fieldName);
        column.Caption = caption;
        column.Width = width;
        column.Visible = true;
    }

    private void ToggleSqlLogCapturing()
    {
        ApiCallLog.IsCapturing = !ApiCallLog.IsCapturing;
        _btnSqlLogStartStop.Text = ApiCallLog.IsCapturing ? "정지" : "시작";
    }

    private void RefreshSqlLogCount() => _lblSqlLogCount.Text = $"{ApiCallLog.Entries.Count}건";

    /// <summary>좌측 사이드바 - 다크 테마. 우측 구분선으로 MDI(흰색) 영역과 시각적으로 분리.
    /// 맨 위 흰 여백(sidebarTopGap)엔 로그인 사용자/시간, 맨 아래엔 화면검색/서버선택 콤보
    /// (원래 헤더 우측에 있었는데 창을 좁히면 밀려 보인다는 피드백으로 이동 - BuildSidebarToolPanel
    /// 참고), 가운데는 메뉴 아코디언.</summary>
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
        sidebarPanel.Controls.Add(sidebarTopGap);
        sidebarPanel.Controls.Add(_sidebarToolPanel);
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
        // 위(타이틀바)도 아래(메뉴트리)도 짙은 색이라, 이 띠만 옅은 톤으로 두고 위아래에
        // 1px 흰 선을 넣어 경계를 만든다 - 선을 짙게 넣으면 어두운 면이 세 겹으로 겹쳐
        // 답답해 보이고, 아예 없으면 띠가 어디서 시작해 어디서 끝나는지 흐려진다.
        sidebarTopGap.BackColor = SidebarUserBg;

        // 두 줄의 위계를 뚜렷하게 - 이름은 한 단계 큰 굵은 글씨(SubHeading)로 먼저 읽히게 하고,
        // 접속시각은 작고 흐린 보조정보(Caption)로 낮춘다. 같은 크기·같은 색으로 두면 어느 쪽이
        // 중요한지 알 수 없어 덩어리로만 보인다. 왼쪽 여백은 이니셜 배지(avatarHost)가 대신 맡는다.

        var lblUserName = new LabelControl
        {
            Dock = DockStyle.Top,
            Height = 19,
            AutoSizeMode = LabelAutoSizeMode.None,
            Padding = new Padding(0, 0, 8, 0)
        };
        lblUserName.Appearance.ForeColor = Color.FromArgb(38, 41, 46);
        lblUserName.Appearance.Font = AppFonts.SubHeading;
        lblUserName.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;

        var lblSignInTime = new LabelControl
        {
            Dock = DockStyle.Top,
            Height = 16,
            AutoSizeMode = LabelAutoSizeMode.None,
            Padding = new Padding(0, 0, 8, 0)
        };
        lblSignInTime.Appearance.ForeColor = Color.FromArgb(138, 143, 150);
        lblSignInTime.Appearance.Font = AppFonts.Caption;
        lblSignInTime.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;

        // 이름 한 글자를 담은 원형 배지 대신, 얇은 세로선 하나만 둔다 - 동그라미+글자 조합은
        // "아바타"처럼 보여 과했다는 피드백. 세로선은 폭이 좁아 왼쪽 정렬된 느낌을 주면서도
        // "여기부터 사용자 정보 영역"이라는 경계 역할은 그대로 한다.
        const int barWidth = 4;
        var avatarBadge = new RoundedPanel
        {
            CornerRadius = barWidth / 2,
            BackColor = ColorHelper.Mix(_accentColor, Color.Black, 0.08f),
            Size = new Size(barWidth, lblUserName.Height + lblSignInTime.Height)
        };

        void Refresh()
        {
            var user = SessionManager.Current.UserInfo;
            if (user == null)
            {
                lblUserName.Text = string.Empty;
                lblSignInTime.Text = string.Empty;
                return;
            }
            lblUserName.Text = user.UserNm;
            // "접속"을 앞에 두면 매번 같은 글자가 먼저 읽혀 시각이 늦게 눈에 들어온다 -
            // 실제로 보는 값(시각)을 앞세우고 라벨은 뒤에 작게 붙인다.
            lblSignInTime.Text = SessionManager.Current.SignInTime is { } t ? $"{t:yyyy-MM-dd HH:mm} 접속" : string.Empty;
        }
        Refresh();
        AppConfig.EnvironmentChanged += Refresh;

        // 배지 기본색(241,243,245)은 옅은 배경(sidebarTopGap)과 거의 구분이 안 돼서, 브랜드색을
        // 섞은 톤으로 바꿔 이 배경 위에서도 "누를 수 있는 것"으로 보이게 한다. 띠 자체가 이미
        // 브랜드색을 옅게 섞은 색이라(SidebarUserBg = 0.93), 배지는 그보다 진해야 떠 보인다.
        homeButton.BadgeColor = ColorHelper.Mix(_accentColor, Color.White, 0.78f);
        homeButton.IconImage = SvgIcons.Load(SvgIcons.Home, homeButton.Width - homeButton.IconInset * 2, _accentColor);
        toolbarToolTip.SetToolTip(homeButton, "홈");
        homeButton.Click += (s, e) => OpenHomeForm();

        // 버튼/배지를 직접 Dock=Right·Left로 붙이면 안 된다 - Dock은 세로를 띠 높이만큼
        // 늘려버려서 정사각형 배지가 세로로 길쭉해진다. 대신 TableLayoutPanel 안에
        // Anchor=None으로 넣어두면, 셀 크기가 바뀔 때마다(=sidebarTopGap 높이가 바뀔 때마다)
        // 레이아웃 엔진이 알아서 다시 정가운데로 맞춰준다 - Resize 이벤트를 직접 구독해서
        // 수동으로 좌표를 계산하던 예전 방식은 첫 측정값이 우연히 초기값과 같으면 재정렬이
        // 평생 한 번도 안 불리는 문제가 있었다(실제로 겪음 - 사용자명/접속시각이 계속 위쪽에
        // 붙어있었다). TableLayoutPanel은 그 클래스의 버그를 구조적으로 없앤다.
        var homeArea = new TableLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = homeButton.Width + 16,
            ColumnCount = 1,
            RowCount = 1,
            BackColor = SidebarUserBg
        };
        _homeArea = homeArea; // 사이드바를 접으면 Dock=Fill로 바꿔 좁은 폭 한가운데로 보낸다
        homeArea.Controls.Add(homeButton);
        homeButton.Anchor = AnchorStyles.None;

        var avatarHost = new TableLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 30,
            ColumnCount = 1,
            RowCount = 1,
            BackColor = SidebarUserBg
        };
        avatarHost.Controls.Add(avatarBadge);
        avatarBadge.Anchor = AnchorStyles.None;

        // 텍스트 두 줄을 세로 가운데로 모으는 그릇 - Percent(50)/AutoSize/Percent(50) 3행으로
        // 나누면, 가운데 행(고정 높이의 두 줄 묶음)이 항상 위아래 여백을 똑같이 나눠 갖는다.
        // 이 역시 TableLayoutPanel 자체의 레이아웃 계산이라 Resize 이벤트가 필요 없다.
        var textArea = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = SidebarUserBg };
        textArea.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
        textArea.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textArea.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
        textArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        var textStack = new Panel
        {
            Dock = DockStyle.Fill,
            // 두 라벨을 각각 위/아래로 붙여둔 덕에(VAlignment Bottom/Top) 합친 높이가 곧
            // 두 줄이 자연스럽게 붙은 높이가 된다 - AutoSize 행이 이 고정 Height를 그대로
            // 측정해서 쓴다.
            Height = lblUserName.Height + lblSignInTime.Height,
            BackColor = SidebarUserBg
        };
        textStack.Controls.Add(lblSignInTime);
        textStack.Controls.Add(lblUserName); // Dock=Top은 나중에 추가한 쪽이 위로 온다
        textArea.Controls.Add(textStack, 0, 1);

        // 사이드바를 접을 때 이니셜 배지와 텍스트 두 줄이 함께 사라지도록 한 그릇에 담는다.
        var identityWrap = new Panel { Dock = DockStyle.Fill, BackColor = SidebarUserBg };
        _lblUserInline = identityWrap;
        identityWrap.Controls.Add(textArea);
        identityWrap.Controls.Add(avatarHost);

        // Dock 추가 순서 중요(이 파일 전체에 반복되는 규칙): Fill(identityWrap) 먼저,
        // 가장자리에 붙는 컨트롤(홈 버튼 영역/경계선)은 나중에 추가해야 제자리를 차지한다.
        sidebarTopGap.Controls.Add(identityWrap);
        sidebarTopGap.Controls.Add(homeArea);
        sidebarTopGap.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.White });
        sidebarTopGap.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.White });
    }

    /// <summary>
    /// 사이드바 맨 아래 콤보 두 줄(화면검색/서버) - 원래 헤더 우측에 가로로 나란히 있었는데,
    /// 창을 좁히면 헤더의 다른 요소(로고/툴바 아이콘)와 부딪혀 밀려 보인다는 피드백으로
    /// 여기로 옮겼다. 사이드바는 폭이 고정(212px)이라 창 너비와 무관하게 항상 같은 자리를
    /// 차지해서, 창을 아무리 좁혀도 겹치거나 밀리는 일이 없다.
    ///
    /// 테마선택 콤보(cboSkin)는 기능은 계속 살려두되(제품 기본 스킨 기준으로 화면별 색을
    /// 맞춰가는 중이라 아직 노출은 안 함 - cboSkin.Enabled=false) 이 자리엔 넣지 않는다.
    /// 나중에 다시 보여주려면 BuildSidebarComboRow("테마", cboSkin)로 줄을 하나 더 만들어
    /// toolPanel.Controls.Add하면 된다.
    /// </summary>
    private Panel BuildSidebarToolPanel()
    {
        // 화면검색 + 서버, 두 줄(각 46px) + 위아래 패딩(10+10).
        var toolPanel = new Panel { Dock = DockStyle.Bottom, Height = 46 * 2 + 20, BackColor = NavDarkBg, Padding = new Padding(14, 10, 14, 10) };
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = NavDivider };

        // 테마 선택 - 고르는 즉시 UserLookAndFeel이 전역으로 바뀌면서 이미 열려있는 화면들까지
        // 포함해 앱 전체(메시지박스, 버튼, 탭, 그리드...)에 실시간으로 반영된다. 자리는 안
        // 만들지만 기능은 그대로 셋업해둔다(위 요약 참고).
        cboSkin.Properties.Items.AddRange(AvailableSkins);
        cboSkin.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboSkin.SelectedItem = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName;
        cboSkin.SelectedIndexChanged += (s, e) =>
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle((string)cboSkin.SelectedItem!);
        cboSkin.Enabled = false;

        BuildEnvironmentCombo();
        var envRow = BuildSidebarComboRow("서버", cboEnvironment);

        BuildMenuSearchCombo();
        var searchRow = BuildSidebarComboRow("화면검색", cboMenuSearch);

        // Dock=Top 쌓기 순서(이 파일 전체 규칙): 나중에 추가한 게 위로 온다 - 화면검색을
        // 맨 위, 서버를 그 아래에 둔다.
        toolPanel.Controls.Add(envRow);
        toolPanel.Controls.Add(searchRow);
        toolPanel.Controls.Add(topBorder);

        return toolPanel;
    }

    /// <summary>다크 배경 위 "라벨 + 콤보" 한 줄. 콤보 자체도 사이드바 톤에 맞게 어둡게 스타일링.
    /// BaseEdit로 받는 이유는 cboEnvironment(ComboBoxEdit)뿐 아니라 cboMenuSearch(LookUpEdit)도
    /// 이 메서드를 같이 쓰기 때문 - Properties.Appearance는 둘 다 RepositoryItem에서 물려받은
    /// 공통 멤버라 타입을 좁히지 않아도 그대로 쓸 수 있다.</summary>
    private Panel BuildSidebarComboRow(string label, BaseEdit combo)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 46 };

        var lbl = new LabelControl { Text = label, Dock = DockStyle.Top, Height = 16 };
        lbl.Appearance.ForeColor = NavTextMuted;
        lbl.Appearance.Font = AppFonts.Caption;
        // 스킨(WXI)이 기본 폰트를 깔고 있어서 Options.UseFont 없이 Appearance.Font만 지정하면
        // 스킨 기본값에 밀릴 수 있다(툴팁에서 겪은 것과 같은 문제 - ShellForm 생성자의
        // toolbarToolTip 주석 참고).
        lbl.Appearance.Options.UseFont = true;

        combo.Dock = DockStyle.Top;
        combo.Font = AppFonts.Body;
        combo.Properties.Appearance.BackColor = NavHoverBg;
        combo.Properties.Appearance.ForeColor = NavText;
        combo.Properties.Appearance.Font = AppFonts.Body;
        combo.Properties.Appearance.Options.UseBackColor = true;
        combo.Properties.Appearance.Options.UseForeColor = true;
        combo.Properties.Appearance.Options.UseFont = true;
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

        // 배지 뒤에 별도 사각형 배경을 두지 않고, 헤더 배경색 위에 큐브 마크를 직접 그린다 -
        // LogoPainter 참고(로그인/스플래시와 좌표를 공유). 관리자가 IconAssetProvider.AssetsFolder에
        // logo.png를 넣어두면 큐브 대신 그 이미지를 그린다 - 회사별 로고를 파일 하나 교체만으로
        // 반영할 수 있게.
        var badge = new Panel
        {
            BackColor = HeaderBg,
            Size = new Size(30, 30),
            Location = new Point(16, 15)
        };
        var customLogo = IconAssetProvider.GetImage("logo");
        if (customLogo != null)
        {
            badge.Paint += (s, e) => e.Graphics.DrawImage(customLogo, badge.ClientRectangle);
        }
        else
        {
            badge.Paint += (s, e) => LogoPainter.Draw(e.Graphics, badge.ClientRectangle, darkBackground: false);
        }

        var nameLabel = new LabelControl
        {
            Text = "WYN LAB",
            Location = new Point(56, 20),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(150, 22)
        };
        nameLabel.Appearance.ForeColor = HeaderText;
        nameLabel.Appearance.Font = AppFonts.SubHeading;

        logoPanel.Controls.Add(badge);
        logoPanel.Controls.Add(nameLabel);

        void ToggleMenu(object? s, EventArgs e) => ToggleSidebarCollapsed();
        logoPanel.Click += ToggleMenu;
        badge.Click += ToggleMenu;
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
        if (_lblUserInline != null)
            _lblUserInline.Visible = !_sidebarCollapsed;
        if (_sidebarToolPanel != null)
            _sidebarToolPanel.Visible = !_sidebarCollapsed;

        // 접히면 사용자정보 두 줄이 사라져 띠 전체가 홈 버튼 차지가 된다. 이때도 Dock=Right로
        // 두면 버튼이 오른쪽에 치우쳐 좁은 폭에서 눈에 띄게 삐뚤어 보이므로, Fill로 바꿔서
        // 남은 폭 한가운데에 오게 한다(가운데 정렬 계산은 homeArea의 Resize 핸들러가 이미 한다).
        if (_homeArea != null)
            _homeArea.Dock = _sidebarCollapsed ? DockStyle.Fill : DockStyle.Right;
    }

    /// <summary>
    /// 조회/입력/삭제/행추가/행삭제/저장/출력을 카드로 묶지 않고 각각 독립된 배지 버튼으로
    /// 헤더에 나란히 배치한다. 예전엔 조회/입력/저장을 하나의 카드로 묶고 저장만 강조색을
    /// 꽉 채워 표시했는데, 그룹핑 자체가 산만하다는 피드백에 따라 전부 개별 버튼으로 풀고
    /// 저장도 다른 아이콘과 같은 스타일(연한 배지 + 강조색 아이콘)로 통일했다.
    /// 배지 배경은 순백색 대신 헤더색을 살짝 섞은 연한 톤(IconBadgeBg)을 써서 튀어 보이지
    /// 않게 했다. "성격" 구분은 이제 아이콘 색으로만 표현한다(삭제/행삭제=빨강, 나머지=브랜드 강조색).
    /// 홈은 화면 전환용이라 별도로 사이드바 메뉴트리 위(ConfigureSidebarTopGap)로 옮겼다.
    ///
    /// 남은 7개는 구분선(|) 없이 한 줄로 두되, 성격이 다른 지점에만 여백(GroupGap)을 줘서
    /// 세 묶음으로 읽히게 한다 - [조회 입력 삭제](레코드 단위) / [행추가 행삭제](그리드 행 단위)
    /// / [저장 출력](마무리). 처음엔 7개를 완전히 붙여놨는데 행 단위 액션과 레코드 단위 액션이
    /// 구분이 안 된다는 피드백이 있었다. 선을 긋는 대신 여백만 준 건 툴바가 조각나 보이지
    /// 않게 하기 위함.
    /// 클릭하면 현재 활성화된 MDI 자식폼(ActiveMdiChild)의 표준 액션(BaseForm.QueryClick 등)을 호출한다.
    /// </summary>
    private void BuildToolbar()
    {
        var badgeBg = IconBadgeBg;

        var x = 228;
        btnQuery = AddIconBadgeButton(headerPanel, ref x, 6, "조회", "query", badgeBg, f => f.QueryClick());
        btnNew = AddIconBadgeButton(headerPanel, ref x, 6, "입력", "new", badgeBg, f => f.NewClick());
        btnDelete = AddIconBadgeButton(headerPanel, ref x, 6, "삭제", "delete", badgeBg, f => f.DeleteClick());
        x += GroupGap;
        btnRowAdd = AddIconBadgeButton(headerPanel, ref x, 6, "행추가", "rowadd", badgeBg, f => f.NewRowClick());
        btnRowDelete = AddIconBadgeButton(headerPanel, ref x, 6, "행삭제", "rowdelete", badgeBg, f => f.DeleteRowClick());
        x += GroupGap;
        btnSave = AddIconBadgeButton(headerPanel, ref x, 6, "저장", "save", badgeBg, f => f.SaveClick());
        btnPrint = AddIconBadgeButton(headerPanel, ref x, 6, "출력", "print", badgeBg, f => f.PrintClick());

        // 사용자별 단축키(TSMSHORTCUTDEFAULT/TSMUSERSHORTCUT)가 ACTION_CD로 가리키는 액션 -
        // ProcessCmdKey가 눌린 키를 SessionManager.Current.Shortcuts에서 찾아 ACTION_CD를 얻으면
        // 여기서 같은 버튼/델리게이트를 그대로 재사용한다(클릭한 것과 완전히 동일하게 동작).
        _toolbarActions["QUERY"] = (btnQuery, "조회", f => f.QueryClick());
        _toolbarActions["NEW"] = (btnNew, "입력", f => f.NewClick());
        _toolbarActions["DELETE"] = (btnDelete, "삭제", f => f.DeleteClick());
        _toolbarActions["ROWADD"] = (btnRowAdd, "행추가", f => f.NewRowClick());
        _toolbarActions["ROWDELETE"] = (btnRowDelete, "행삭제", f => f.DeleteRowClick());
        _toolbarActions["SAVE"] = (btnSave, "저장", f => f.SaveClick());
        _toolbarActions["PRINT"] = (btnPrint, "출력", f => f.PrintClick());

        // 시스템 성격 버튼(SQL로그/로그아웃) - 업무 액션(조회~출력)과는 종류가 아예 달라서(데이터
        // 조작이 아니라 시스템/세션 동작), 여백만 있는 다른 소그룹들과 달리 여기는 실선으로
        // 확실히 갈라준다. 헤더 맨 오른쪽 끝(예전 자리)이 아니라 왼쪽 버튼 무리 가까이로
        // 모아달라는 요청 - 화면검색/서비스전환 콤보만 우측에 남긴다(BuildHeaderRight 참고).
        x += GroupGap;
        AddDivider(ref x);
        x += GroupGap - 12;
        var btnTabList = AddPlainIconButton(headerPanel, ref x, 6, "탭 목록", "tablist", badgeBg, () => { });
        btnTabList.Click += (s, e) => ShowTabListPopup(btnTabList);
        if (Session.UserType == "A")
        {
            AddPlainIconButton(headerPanel, ref x, 6, "SQL로그", "sqllog", badgeBg,
                () => { if (_sqlLogPanel != null) _sqlLogPanel.Visible = !_sqlLogPanel.Visible; });
        }
        AddPlainIconButton(headerPanel, ref x, 6, "로그아웃", "logout", badgeBg, OnLogoutClick);
    }

    /// <summary>AddIconBadgeButton과 같은 시각 스타일이지만, 활성 MDI 자식(BaseForm)과 무관하게
    /// 항상 동작해야 하는 버튼(SQL로그 토글, 로그아웃)용 - BaseForm 액션 델리게이트 대신 단순
    /// Action만 받는다.</summary>
    private IconBadgeButton AddPlainIconButton(Control container, ref int x, int y, string text, string iconName,
        Color badgeColor, Action onClick)
    {
        var btn = new IconBadgeButton
        {
            Text = text,
            IconName = iconName,
            BadgeColor = badgeColor,
            Location = new Point(x, y),
            Size = ButtonSize
        };
        toolbarToolTip.SetToolTip(btn, text);
        btn.Click += (s, e) => onClick();
        container.Controls.Add(btn);
        x += btn.Width;
        return btn;
    }

    // 화면(MDI 자식)이 바뀔 때마다 UpdateToolbarPermissions()가 이 참조들의 Enabled를
    // 그 화면의 BaseForm.CanInsert/CanUpdate/CanDelete로 다시 계산해서 켜고 끈다.
    // AddIconBadgeButton 내부 지역변수였던 것을 필드로 승격 - 나중에 다시 손댈 수 있어야 해서.
    private IconBadgeButton btnQuery = null!;
    private IconBadgeButton btnNew = null!;
    private IconBadgeButton btnDelete = null!;
    private IconBadgeButton btnRowAdd = null!;
    private IconBadgeButton btnRowDelete = null!;
    private IconBadgeButton btnSave = null!;
    private IconBadgeButton btnPrint = null!;

    /// <summary>ACTION_CD -> (버튼, 표시용 라벨, 실행 델리게이트). BuildToolbar에서 채워지고
    /// ProcessCmdKey의 단축키 디스패치가 읽는다.</summary>
    private readonly Dictionary<string, (IconBadgeButton Button, string Label, Func<BaseForm, Task> Action)> _toolbarActions = new();

    private static readonly Size ButtonSize = new(54, 48);

    /// <summary>툴바 아이콘 묶음 사이 여백(BuildHeaderToolbar 주석의 3개 묶음 참고).
    /// 구분선을 긋지 않고 여백만으로 나누는 방식이라, 너무 넓으면 툴바가 흩어져 보이고
    /// 너무 좁으면 나눈 티가 안 난다 - 버튼 폭(54)의 1/4 정도가 적당했다.</summary>
    private const int GroupGap = 14;

    /// <summary>
    /// action 파라미터는 BaseForm을 받지만, "홈" 버튼처럼 활성화면과 무관하게 항상 동작해야 하는
    /// 경우도 있어서, 실제로는 델리게이트 내부에서 ActiveMdiChild를 쓸지 말지 자유롭게 결정한다.
    /// (홈 버튼은 activeForm 인자를 무시하고 항상 OpenHomeForm()만 호출)
    /// container: 이 버튼을 실제로 담을 컨트롤(headerPanel 직접 또는 AddToolbarGroup으로 만든 카드).
    /// x/y는 container 기준 로컬 좌표. 반환값은 UpdateToolbarPermissions()에서 Enabled를
    /// 다시 계산할 수 있도록 호출측(BuildToolbar)이 필드에 보관해두기 위함.
    /// </summary>
    private IconBadgeButton AddIconBadgeButton(Control container, ref int x, int y, string text, string iconName,
        Color badgeColor, Func<BaseForm, Task> action)
    {
        var btn = new IconBadgeButton
        {
            Text = text,
            IconName = iconName,
            BadgeColor = badgeColor,
            Location = new Point(x, y),
            Size = ButtonSize
        };
        toolbarToolTip.SetToolTip(btn, text);

        btn.Click += async (s, e) => await InvokeToolbarActionAsync(text, action);

        container.Controls.Add(btn);
        x += btn.Width;
        return btn;
    }

    /// <summary>버튼 클릭과 단축키(ProcessCmdKey) 둘 다 여기로 모아서 같은 방식(활성화면 확인/
    /// 진행중 표시/오류 메시지)으로 BaseForm 표준 액션을 실행한다 - 예전엔 이 로직이
    /// AddIconBadgeButton의 Click 람다 안에만 있어서 단축키는 재사용할 수 없었다.</summary>
    private async Task InvokeToolbarActionAsync(string label, Func<BaseForm, Task> action)
    {
        // 홈 버튼처럼 활성화면이 없어도 동작해야 하는 경우를 위해 null 허용 폼으로 처리
        var activeForm = ActiveMdiChild as BaseForm;
        if (activeForm == null && label != "홈")
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
            AppMessageBox.Show($"[{label}] 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            activeForm?.HideBusy();
        }
    }

    /// <summary>
    /// 활성 MDI 자식(현재 열려있는 업무화면)이 바뀔 때마다 호출되어 7개 툴바 아이콘의
    /// Enabled를 그 화면의 권한(BaseForm.CanInsert/CanUpdate/CanDelete)에 맞춰 다시 계산한다.
    /// 조회/출력은 별도 권한 플래그가 없다 - ViewYn은 이미 "이 메뉴를 열 수 있는지" 자체를
    /// 가리자원, 화면이 열려 있다는 것 자체가 조회 권한이 있다는 뜻이라 항상 켜둔다(출력도 동일
    /// 취급 - PrintYn이라는 필드 자체가 없음). 저장은 신규/수정 두 흐름을 다 섬기므로
    /// CanInsert 또는 CanUpdate 둘 중 하나만 있어도 켠다.
    /// 활성 업무화면이 없는 경우(홈 화면이거나 열린 화면이 하나도 없을 때)는 조회/출력만 남기고
    /// 나머지 5개는 전부 끈다 - 대상 데이터가 없는 상태에서 입력/삭제/저장을 누르게 둘 이유가 없다.
    /// </summary>
    private void UpdateToolbarPermissions()
    {
        var activeForm = ActiveMdiChild as BaseForm;
        var hasTarget = activeForm != null;

        btnQuery.Enabled = true;
        btnPrint.Enabled = true;

        var canInsert = hasTarget && activeForm!.CanInsert;
        var canUpdate = hasTarget && activeForm!.CanUpdate;
        var canDelete = hasTarget && activeForm!.CanDelete;

        btnNew.Enabled = canInsert;
        btnRowAdd.Enabled = canInsert;
        btnDelete.Enabled = canDelete;
        btnRowDelete.Enabled = canDelete;
        btnSave.Enabled = canInsert || canUpdate;
    }

    private void AddDivider(ref int x)
    {
        var divider = new Panel { Location = new Point(x, 14), Size = new Size(1, 32), BackColor = HeaderDividerColor };
        headerPanel.Controls.Add(divider);
        x += 12;
    }

    /// <summary>헤더 우측의 서비스(환경) 선택 콤보 - 값을 바꾸면 OnEnvironmentComboChanged가
    /// 재로그인 흐름을 진행한다(로그아웃 버튼과 같은 SignOutAndShowLogin을 공유).</summary>
    private void BuildEnvironmentCombo()
    {
        cboEnvironment.Size = new Size(120, 26);
        cboEnvironment.Font = AppFonts.Body;
        cboEnvironment.Properties.Appearance.Font = AppFonts.Body;
        cboEnvironment.Properties.Items.AddRange(AppConfig.AvailableEnvironments.Select(GetEnvLabel).ToArray());
        cboEnvironment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboEnvironment.SelectedItem = GetEnvLabel(AppConfig.CurrentEnvironment);
        cboEnvironment.SelectedIndexChanged += OnEnvironmentComboChanged;
    }

    /// <summary>
    /// 화면검색 콤보. 타이핑하면 목록이 걸러지고(SearchMode.AutoFilter), 화면을 고르면 바로 열고
    /// 다시 비워서 연속 검색이 가능하게 한다. 열 수 있는 화면(GROUP이 아니고 FormClassNm이
    /// 있으며 조회권한이 있는 것)만 담는다 - OpenQuickMenuSearch와 같은 기준이다.
    ///
    /// 목록은 로그인 후에야 채워지므로 여기선 모양만 잡고, 실제 데이터는 RefreshMenuSearchItems가
    /// 메뉴를 만들 때 넣는다(서버를 바꿔 재로그인하면 메뉴가 통째로 달라지므로 그때도 다시 채운다).
    /// </summary>
    private void BuildMenuSearchCombo()
    {
        cboMenuSearch.Size = new Size(210, 26);
        cboMenuSearch.Properties.NullText = "화면 검색";
        cboMenuSearch.Properties.ShowHeader = false;
        cboMenuSearch.Properties.ShowFooter = false;
        cboMenuSearch.Properties.DisplayMember = nameof(MenuDto.MenuNm);
        cboMenuSearch.Properties.ValueMember = nameof(MenuDto.MenuCd);

        // PopulateColumns()를 쓰면 MenuDto의 모든 프로퍼티가 열로 깔려서(MenuCd/FormClassNm 등)
        // 드롭다운이 표처럼 보인다(실제로 겪음). 보여줄 열 하나만 직접 정의한다.
        cboMenuSearch.Properties.Columns.Clear();
        cboMenuSearch.Properties.Columns.Add(
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(nameof(MenuDto.MenuNm)));

        // 타이핑한 글자로 목록을 걸러준다 - AutoSearchColumnIndex는 "몇 번째 열을 기준으로
        // 찾을지"라, 화면명 한 열만 두고 0번으로 지정한다.
        //
        // TextEditStyle을 반드시 Standard로 열어줘야 한다. LookUpEdit의 기본값은
        // DisableTextEditor라서 글자를 아예 못 치고(실제로 겪음), 그러면 AutoFilter도 걸릴
        // 입력 자체가 없어 그냥 목록만 뜨는 콤보가 된다.
        cboMenuSearch.Properties.TextEditStyle = TextEditStyles.Standard;
        cboMenuSearch.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        cboMenuSearch.Properties.AutoSearchColumnIndex = 0;

        // 이름 중간에 있는 글자로도 찾히게 한다 - "코드"라고 치면 "기초코드등록"이 나와야지,
        // 앞글자부터 정확히 맞춰 쳐야 하면 화면명을 외우고 있어야만 쓸 수 있다.
        cboMenuSearch.Properties.PopupFilterMode = PopupFilterMode.Contains;

        // 글자를 치기 시작하면 그때 목록이 뜬다(걸러진 상태로). 포커스만 갔을 때 전체 목록이
        // 쏟아지면 "검색"이 아니라 그냥 긴 목록이 되어버려서, 먼저 치게 두고 결과만 보여준다.
        cboMenuSearch.Properties.ImmediatePopup = true;
        cboMenuSearch.Properties.Appearance.Font = AppFonts.Body;

        // EditValueChanged가 아니라 CloseUp을 쓴다 - EditValueChanged는 타이핑 도중 AutoFilter가
        // 후보를 좁혀가면서 EditValue를 스스로 건드릴 때도 fire해서, 실제로 고르지 않았는데도
        // 화면이 열려버렸다(실제로 겪음 - 에디팅 중간에 "화면을 여시겠습니까" 메시지가 뜸).
        // CloseUp은 팝업이 닫힐 때만 fire하고, AcceptValue로 "진짜 골라서 닫힘(Enter/클릭)"과
        // "그냥 닫힘(Esc, 포커스 이탈)"을 구분해준다 - 후자는 무시한다.
        cboMenuSearch.CloseUp += (s, e) =>
        {
            if (!e.AcceptValue) return;
            if (e.Value is not string menuCd || string.IsNullOrWhiteSpace(menuCd)) return;

            // 여기서 곧바로 화면을 열면 콤보의 팝업이 닫히는 중에 모달/포커스가 얽힌다.
            // 한 박자 뒤로 미뤄서 콤보가 자기 일을 끝낸 다음에 열도록 한다.
            BeginInvoke(new Action(() =>
            {
                // ImmediatePopup=true라 Text를 빈 문자열로 바꾸는 것 자체가 "타이핑"으로 잡혀
                // 팝업이 또 잠깐 열렸다 닫힌다 - 선택 직후 화면이 열리는 순간과 겹쳐서 깜빡임으로
                // 보였다(실제로 겪음). 다음 검색을 위해 비우는 동안만 ImmediatePopup을 꺼서
                // 이 재오픈 자체가 안 생기게 막는다.
                cboMenuSearch.Properties.ImmediatePopup = false;
                try
                {
                    // 다음 검색을 위해 비운다. EditValue만 지우면 AutoFilter로 쳐넣은 글자가
                    // 입력칸에 남아, 다시 검색할 때 지우고 시작해야 한다.
                    cboMenuSearch.EditValue = null;
                    cboMenuSearch.Text = string.Empty;
                    cboMenuSearch.ClosePopup();
                }
                finally
                {
                    cboMenuSearch.Properties.ImmediatePopup = true;
                }

                OpenMenuByCode(menuCd);
            }));
        };
    }

    /// <summary>검색 콤보의 목록을 현재 로그인 사용자가 열 수 있는 화면들로 채운다.</summary>
    private void RefreshMenuSearchItems()
    {
        cboMenuSearch.Properties.DataSource = SessionManager.Current.Menus
            .Where(m => m.ViewYn && m.MenuType != "GROUP" && !string.IsNullOrWhiteSpace(m.FormClassNm))
            .OrderBy(m => m.MenuNm)
            .ToList();
        cboMenuSearch.EditValue = null;
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

        SignOutAndShowLogin(() => AppConfig.SwitchEnvironment(selectedKey));
    }

    /// <summary>로그아웃 버튼 - 확인창에서 "예"를 누르면 SignOutAndShowLogin으로 넘어간다
    /// (서버 전환과 같은 흐름을 공유, 전환할 서버가 없다는 점만 다르다).</summary>
    private void OnLogoutClick()
    {
        var confirm = AppMessageBox.Show("로그아웃 하시겠습니까?", "로그아웃", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        SignOutAndShowLogin();
    }

    /// <summary>
    /// 현재 세션을 정리하고 로그인창을 다시 띄운다. 셸(ShellForm) 자체는 닫지 않고 숨겼다가
    /// 재로그인 성공 시 다시 보여준다 - Program.cs의 Application.Run(shell)이 계속 도는 채로
    /// 재사용되므로 Program.cs를 건드릴 필요가 없다. 재로그인을 취소하면 프로그램을 종료한다
    /// (예전 세션으로 되돌릴 방법이 없으므로).
    ///
    /// beforeShowingLogin: 서버 전환(OnEnvironmentComboChanged)처럼 SignOut 직후, 메뉴를 비우기
    /// 전에 한 단계 더 필요한 경우에만 쓴다(AppConfig.SwitchEnvironment) - 로그아웃 버튼은 그냥
    /// null로 부른다.
    /// </summary>
    private void SignOutAndShowLogin(Action? beforeShowingLogin = null)
    {
        foreach (Form child in MdiChildren.ToArray())
        {
            child.Close();
        }

        SessionManager.Current.SignOut();
        beforeShowingLogin?.Invoke();
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
    /// <summary>
    /// TSMMENU.ICON_NM 값 -> DevExpress SVG 아이콘 이름. 여기 없는 값이 오면 폴더 아이콘으로
    /// 떨어진다(SvgIcons.Folder) - 메뉴를 새로 만들 때 아이콘 이름을 안 정해도 메뉴는 정상으로 뜬다.
    /// 모듈이 늘어나면 이 표에 한 줄만 추가하면 되고, 쓸 수 있는 아이콘 목록은 SvgIcons 참고.
    /// </summary>
    private static readonly Dictionary<string, string> TopMenuIcons = new(StringComparer.OrdinalIgnoreCase)
    {
        ["settings"] = SvgIcons.Settings,
        ["shoppingcart"] = SvgIcons.ShoppingCart,
        ["tools"] = SvgIcons.Database,
        ["user"] = SvgIcons.User,
        ["security"] = SvgIcons.Security,
        ["box"] = SvgIcons.Box,
        ["money"] = SvgIcons.Money,
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

            var iconName = (top.IconNm != null && TopMenuIcons.TryGetValue(top.IconNm, out var n)) ? n : SvgIcons.Folder;
            group.ImageOptions.Image = SvgIcons.Load(iconName, MenuTopIconSize, NavText);
            AddSidebarRailButton(top.MenuNm, iconName, ref railY);

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

            ApplyGroupPressedAppearance(group, AppFonts.SubHeading);

            AddChildMenus(group, menus, top.MenuCd);
            accordionMenu.Elements.Add(group);
        }

        // 헤더의 화면검색 콤보도 같은 메뉴 목록을 쓰므로 여기서 같이 채운다 - 서버를 바꿔
        // 재로그인하면 이 메서드가 다시 도니까 콤보 목록도 자동으로 새 메뉴로 갈린다.
        RefreshMenuSearchItems();
    }

    /// <summary>
    /// 접힌 사이드바(아이콘 레일)에 최상위 메뉴 하나당 아이콘 버튼 하나를 세로로 쌓아 배치.
    /// 하위 메뉴까지 좁은 폭에 담기는 어려워, 클릭하면 그냥 사이드바를 펼치는 것으로 위임한다.
    /// </summary>
    private void AddSidebarRailButton(string tooltipText, string iconName, ref int y)
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
            Image = SvgIcons.Load(iconName, MenuTopIconSize + 4, NavText),
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

                ApplyGroupPressedAppearance(element, AppFonts.BodyBold);
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

    /// <summary>
    /// 그룹(펼치기/접기만 하는 항목)의 "눌린 상태" 외형. 호버 상태와 똑같이 맞춘다 - 누르는
    /// 동안에도 마우스는 그 위에 있으니 호버와 같아야 자연스럽고, 무엇보다 아무 색이 튀지 않는다.
    ///
    /// 이걸 지정하지 않으면 DevExpress가 스킨 기본 눌림 외형(밝은 회색/흰색)을 쓰기 때문에,
    /// 어두운 사이드바에서 그룹을 클릭하는 순간 배경이 흰색으로 번쩍했다가 돌아온다(실제로 겪음).
    /// 화면(Item) 항목은 이미 Pressed를 지정하고 있었는데 그룹만 빠져 있었다.
    /// </summary>
    private void ApplyGroupPressedAppearance(AccordionControlElement element, Font font)
    {
        element.Appearance.Pressed.BackColor = NavHoverBg;
        element.Appearance.Pressed.ForeColor = Color.White;
        element.Appearance.Pressed.Font = font;
        element.Appearance.Pressed.Options.UseBackColor = true;
        element.Appearance.Pressed.Options.UseForeColor = true;
        element.Appearance.Pressed.Options.UseFont = true;
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
    /// 사용자별 단축키(TSMSHORTCUTDEFAULT/TSMUSERSHORTCUT, SessionManager.Current.Shortcuts) 디스패치.
    /// MDI 자식(그리드 등)에 포커스가 있어도 잡히도록 ProcessCmdKey에서 가로챈다(KeyDown은
    /// 포커스를 가진 자식 컨트롤이 먼저 소비해버릴 수 있음).
    ///
    /// 눌린 키를 ShortcutKeys.ToText로 서버와 같은 문자열 형식("Ctrl+Q")으로 바꿔 유효 단축키
    /// 목록에서 찾고, 매치되면 그 ACTION_CD로 BuildToolbar가 등록해둔 것과 완전히 같은 버튼/
    /// 델리게이트를 부른다(InvokeToolbarActionAsync 참고) - 버튼을 직접 클릭한 것과 동작이
    /// 같아야 하므로, 그 버튼이 지금 비활성(권한 없음 등)이면 단축키도 조용히 무시한다.
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        var comboText = ShortcutKeys.ToText(keyData);
        var shortcut = SessionManager.Current.Shortcuts.FirstOrDefault(s => s.KeyCombo == comboText);
        if (shortcut != null && _toolbarActions.TryGetValue(shortcut.ActionCd, out var entry) && entry.Button.Enabled)
        {
            _ = InvokeToolbarActionAsync(entry.Label, entry.Action);
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
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
        if (formType == null)
        {
            AppMessageBox.Show($"화면을 찾을 수 없습니다: {menu.FormClassNm}", "오류");
            return;
        }

        BaseForm form;
        try
        {
            if (Activator.CreateInstance(formType) is not BaseForm created)
            {
                AppMessageBox.Show($"화면을 찾을 수 없습니다: {menu.FormClassNm}", "오류");
                return;
            }
            form = created;
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            // Activator.CreateInstance는 생성자가 던진 진짜 예외를 TargetInvocationException으로
            // 감싼다 - 그대로 놔두면 화면마다 "호출 대상이 예외를 Throw했습니다"라는 의미 없는
            // 메시지만 보여서 원인을 전혀 알 수 없다(실제로 겪음 - frmUserAuth 첫 오픈 때).
            // InnerException(진짜 원인)을 그대로 보여준다.
            var real = ex.InnerException ?? ex;
            AppMessageBox.Show($"화면을 여는 중 오류가 발생했습니다: {menu.FormClassNm}\n\n{real.GetType().Name}: {real.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        form.Name = menu.MenuCd;
        form.MenuCd = menu.MenuCd;
        form.MdiParent = this;
        form.Show();

        SessionManager.Current.AddRecentMenu(menu);
    }
}
