using DevExpress.XtraBars.Navigation;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;

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
/// 써서 "같은 브랜드지만 서로 다른 면"임이 구분되도록 한다(ColorHelper.Mix). 툴바 버튼은
/// 아이콘+라벨+단축키를 한 줄에 표시하는 IconChipButton으로 나열한다(2026-09-17 리디자인) -
/// 저장만 Primary(꽉 찬 파란 배경), 삭제/로그아웃은 Danger(빨간 글자/아이콘), 나머지는 Default
/// (평소엔 배경 없이 호버할 때만 옅게 채워짐)로 구분한다(IconChipVariant).
/// </summary>
// 2026-09-17: ToolbarForm(DevExpress 공식 "제목줄에 merge" 클래스)을 시도했으나, 제목줄
// 배경색이 활성 스킨(WXI)에 눌려 BackColor/Controller.PaintStyleName/BarManager.BarBackColor
// 셋 다 효과가 없었다(전부 실제 배포로 확인) - DevExpress 공식 문서도 "스킨 요소 재염색은
// Skin Editor 영역"이라 코드로는 못 하는 게 맞았다. 그래서 스킨 시스템 자체를 안 쓰는 방식으로
// 바꿨다 - FormBorderStyle.None + 직접 그린 제목줄 Panel(titleBarPanel, BuildTitleBar 참고).
// 예전에 실패했던 "커스텀 캡션" 시도(RefreshTitle 위 주석)는 FormBorderStyle을 그대로 둔 채
// XtraForm 위에 WM_NCHITTEST를 얹어서 XtraForm 자신의 비클라이언트 처리와 충돌했던 것 -
// 이번엔 None으로 아예 꺼버려서 그 충돌 자체가 성립하지 않는다. 드래그 이동은 WM_NCHITTEST
// 가로채기 대신 훨씬 단순한 표준 트릭(ReleaseCapture+WM_NCLBUTTONDOWN, BuildTitleBar 참고)을
// 쓴다 - 많은 상용 WinForms 셸이 실제로 쓰는 방식.
public class ShellForm : XtraForm
{
    private readonly Color _accentColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

    // 2026-09-17엔 제목줄(titleBarPanel)만 짙은 남색이고 그 아래 헤더(툴바)는 순백색이었는데,
    // "툴바 아이콘 있는 패널의 배경색을 그 위쪽(제목줄) 배경색과 같은 색으로" 요청(2026-09-22)으로
    // NavDarkBg를 그대로 쓰도록 되돌렸다가, 완전히 같은 색이라 한 덩어리로 뭉쳐 보인다는
    // 지적(2026-09-22)에 SidebarBg와 같은 방식(Adjust)으로 밝혔다. 처음 +10은 여전히 구분이
    // 안 된다는 재지적(2026-09-22)으로 +28로 더 키웠다. 회사별 ToolbarColor가 바뀌어도 항상
    // "제목줄보다 한 톤 밝은 같은 계열"을 유지한다.
    private Color HeaderBg => ColorHelper.Adjust(NavDarkBg, 28);
    private Color HeaderDividerColor => ColorHelper.Adjust(HeaderBg, -22);
    private static readonly Color SqlLogToolbarBg = Color.FromArgb(245, 246, 248);
    private static readonly Color DividerColor = Color.FromArgb(225, 225, 225);
    // cboMenuSearch(검색창) 자신은 항상 흰 입력창(BuildMenuSearchCombo에서 명시적으로
    // Color.White)이라 헤더 배경과 무관하게 이 짙은 색 그대로 쓴다.
    private static readonly Color HeaderText = Color.FromArgb(28, 30, 34);
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
    // 밝기만 올리는 Adjust는 이미 어두운 남색(NavDarkBg)에 회색을 끼얹은 것처럼 보여 "밝은 파랑"으로
    // 안 읽힌다는 피드백 - 채도 있는 파랑(NavAccentBlue) 쪽으로 섞어야 눈에 띄게 파래진다.
    private static readonly Color NavAccentBlue = Color.FromArgb(43, 130, 255);

    /// <summary>사이드바(메뉴트리)+상태바 전용 배경 - 제목줄(titleBarPanel)은 계속 NavDarkBg를
    /// 그대로 쓰고, 그 옆/아래 영역만 같은 계열에서 살짝 밝혀서 구분되게 한다. 완전히 다른 색이
    /// 아니라 NavDarkBg에서 파생시켜서, 회사별 ToolbarColor가 바뀌어도 항상 "제목줄보다 한 톤
    /// 밝은 같은 계열"을 유지한다("제목줄과 메뉴트리가 같은 색이라 이상하다"는 지적, 2026-09-17).</summary>
    private Color SidebarBg => ColorHelper.Adjust(NavDarkBg, 10);

    /// <summary>메뉴트리(accordionMenu) 자체의 배경 - 참고 목업(2026-09-21, "탐색 창
    /// (Navigation)")이 다크가 아니라 밝은 오프화이트(#F8FAFC) 사이드바였다는 요청으로,
    /// 기존 다크 그레이 톤에서 라이트 테마로 전환했다. 상태바/제목줄(NavDarkBg/SidebarBg)은
    /// 계속 남색 계열 그대로 - 트리 영역만 밝은 톤이라 그 위·아래와 대비된다.</summary>
    private static readonly Color SidebarTreeBg = Color.FromArgb(248, 250, 252);

    private Color NavHoverBg => ColorHelper.Mix(SidebarTreeBg, NavAccentBlue, 0.12f);
    private static readonly Color NavText = Color.FromArgb(51, 65, 85);
    private static readonly Color NavTextMuted = Color.FromArgb(100, 116, 139);
    private Color NavDivider => Color.FromArgb(226, 232, 240);
    /// <summary>호버된 그룹/화면 항목의 글자색 - 라이트 테마 전환 전엔 다크 배경 위라 항상
    /// 흰색이었지만, 이제 배경(NavHoverBg)이 옅은 하늘색이라 흰 글자는 거의 안 보인다.</summary>
    private static readonly Color NavHoverFg = Color.FromArgb(15, 23, 42);

    private const int MenuTopIconSize = 15;
    // AccordionControlElement는 별도 Height 속성이 없고 행 높이가 아이콘/폰트 크기로 자동
    // 계산된다 - "마이메뉴/전체메뉴 높이를 조금 더 키워달라" 요청(2026-09-22)에 맞춰 이 둘의
    // 헤더 아이콘만 키운다(모듈 그룹/화면들은 MenuTopIconSize 그대로라 영향 없음).
    private const int MenuRootHeaderIconSize = 20;
    private const int MenuLeafDotSize = 9;
    private const int MenuSubGroupIconSize = 14;
    private const int RailIconSize = 13;

    // 사이드바(고정) 폭. 접기/펼치기는 폭을 줄이는 대신 sidebarPanel 자체를 통째로
    // 숨긴다(ToggleSidebarCollapsed 참고, 2026-09-21 요청 - "메뉴트리 숨기기 하면 트리 자체를
    // 숨기고 열린 폼이 전체 화면을 채우게").
    private const int SidebarExpandedWidth = 212;
    private bool _sidebarCollapsed;

    private readonly Panel headerPanel;

    /// <summary>탭 줄(XtraTabbedMdiManager가 그리는 영역) 오른쪽 끝에 겹쳐 띄우는 작은 아이콘
    /// 스트립 - "탭 목록"/"탭 전체 닫기"를 여기로 옮겼다(BuildTabStripButtons 참고). 탭 컨트롤
    /// 자체엔 커스텀 버튼을 넣을 자리가 없어서(HeaderButtons가 실제로는 Prev/Next/Close만 있는
    /// 열거형이라는 걸 리플렉션으로 확인함), ShellForm 위에 절대좌표로 별도 패널을 얹어
    /// 시각적으로만 같은 줄처럼 보이게 하는 방식이다.
    ///
    /// 배경은 흰색 -> 2026-09-21엔 "창 테두리(WindowBorderColor)와 같은 배경색으로" 요청으로
    /// 회색(Gainsboro)으로 바꿨었는데, 그 결과 탭이 없을 때(MDI 영역이 그냥 흰 배경) 이
    /// 스트립만 회색 박스처럼 도드라져 보였다("세 아이콘 뒤 배경을 나머지와 동일하게" 지적,
    /// 2026-09-22) - 다시 흰색으로 되돌린다. IconBadgeButton이 알아서 Parent.BackColor를
    /// 따라가므로 여기 색만 바꾸면 된다.</summary>
    private readonly Panel tabStripButtons = new() { BackColor = Color.White, Height = 30 };
    // 2026-09-17 요청 - "탭닫기 버튼 있는쪽에 홈버튼만 추가해줘"(탭목록/탭전체닫기 기능은 그대로).
    private readonly IconBadgeButton btnHomeStrip = new() { Size = new Size(28, 28), IconInset = 5 };
    private readonly IconBadgeButton btnTabListStrip = new() { Size = new Size(28, 28), IconInset = 5 };
    private readonly IconBadgeButton btnCloseAllStrip = new() { Size = new Size(28, 28), IconInset = 5 };

    /// <summary>헤더 맨 오른쪽의 화면 검색 콤보 - 타이핑하면 목록이 걸러지고, 고르면 그 화면이
    /// 열린다. 예전엔 별도 검색창(QuickMenuSearchForm)을 Ctrl+K로 띄웠었고, 그 다음엔 사이드바
    /// 맨 아래(BuildSidebarToolPanel)로 옮겼었는데, "리사이즈해도 항상 헤더 오른쪽 끝에 있어야
    /// 한다"는 요청(2026-09-03)으로 다시 헤더로 옮겼다(BuildHeaderRightCombos 참고) - 이번엔
    /// Dock=Right라 창을 좁혀도 항상 오른쪽 끝에 붙어있다.</summary>
    private readonly LookUpEdit cboMenuSearch = new();
    // 제목줄 SERVICE 알약처럼 둥근 필드로 보이게 하려고(2026-09-18 요청) 뒤에 까는 배경 -
    // DevExpress 에디터 자체는 각진 사각 배경만 그릴 수 있어서, 에디터 쪽 테두리는 없애고
    // (NoBorder) 이 패널이 그리는 둥근 배경 위에 얹는 방식으로 흉내 낸다. 검색창은 이제
    // headerPanel(툴바) 안, 조회 버튼 왼쪽에 있다(BuildHeaderSearchZone 참고).
    private Panel _cboMenuSearchPill = null!;
    private readonly Panel sidebarPanel;
    private readonly Panel statusBar;
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
    /// <summary>상태바 우측(lblStatusRight 옆)의 프로필러 토글 아이콘 - null이면 관리자가
    /// 아니라서 애초에 안 만들어졌다. PositionStatusBar가 리사이즈마다 lblStatusRight 기준
    /// 위치를 다시 계산한다.</summary>
    private IconChipButton? _sqlLogChip;
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
    // 사이드바 메뉴트리 우클릭 - "메뉴 새로고침"(재로그인 없이 새로 등록된 메뉴를 바로 반영,
    // 2026-09-16 요청) 하나뿐이라 tabContextMenu처럼 클릭마다 다시 채울 필요 없이 한 번만 구성한다.
    private readonly ContextMenuStrip sidebarContextMenu = new();
    private HomeForm? _homeForm;

    /// <summary>즐겨찾기 1건의 로컬 표현 - 서버 FavoriteMenuDto와 1:1. Folder는 사용자가
    /// "폴더 지정..."으로 붙인 임의 이름표(2026-09-21, "핵심 업무"/"자재 출납" 같은 것) - null/빈
    /// 문자열이면 마이 메뉴 바로 아래 평평하게 보인다.</summary>
    private sealed class FavoriteMenuEntry
    {
        public long MenuId;
        public string? Folder;
    }

    /// <summary>사이드바 "마이 메뉴" 즐겨찾기 - 로그인 사용자가 별표한 목록(2026-09-21).
    /// 표시 순서 그대로라 List(순서 있음)다 - HashSet이었다면 상위/하위로 이동(순서변경)을
    /// 표현할 수 없다. ReloadFavoriteMenusAsync가 서버(TSMUSERFAVORITEMENU, SORT_ORDER 순)와
    /// 동기화한다.</summary>
    private readonly List<FavoriteMenuEntry> _favorites = new();

    /// <summary>방금 만든 "마이 메뉴" 그룹 엘리먼트 - "마이 메뉴" 헤더 자신을 클릭했는지
    /// (ToggleMyMenuMode) 판단하는 데 쓴다.</summary>
    private AccordionControlElement? _favoritesGroupElement;

    /// <summary>BuildFavoriteMenuGroup이 만든 리프(화면) 엘리먼트 전체 - 우클릭한 항목이
    /// 마이메뉴 소속인지(그래서 "상위로/하위로 이동"/"폴더 지정..."을 보여줄지) 판단하는 데
    /// 쓴다. 폴더로 묶이면 리프가 "마이 메뉴" 그룹의 직계 자식이 아니라 폴더 그룹의 자식이
    /// 되므로(2단계 중첩), _favoritesGroupElement.Elements.Contains만으로는 더 이상 못
    /// 판별한다 - AccordionControlElement엔 Parent 프로퍼티가 없어서, 만들 때 직접 이 집합에
    /// 넣어두고 나중에 멤버십만 확인한다.</summary>
    private readonly HashSet<AccordionControlElement> _favoriteLeafElements = new();

    /// <summary>"마이 메뉴"와 "전체 메뉴"는 상호배타 - 하나가 열리면 다른 하나는 통째로
    /// 숨는다(2026-09-21 목업 요청). 기본값 false = 처음엔 전체 메뉴만 보이고 마이 메뉴는
    /// 닫힌 채(헤더만 보임) 시작한다. 마이 메뉴 헤더를 클릭(ToggleMyMenuMode)하면 뒤집힌다.</summary>
    private bool _myMenuExpanded;

    /// <summary>사이드바 메뉴검색(cboMenuSearch)에 타이핑 중인 실시간 필터 키워드 - 비어있으면
    /// 평소와 동일하게 전체가 보인다. RebuildAccordionTree가 매 키 입력마다 이 값 기준으로
    /// 마이메뉴/전체메뉴 양쪽을 다시 그린다(2026-09-21 요청 - "마이메뉴와 전체메뉴를 동시에
    /// 필터링하여 실시간 검색").</summary>
    private string _menuFilterText = string.Empty;

    // 자리비움 잠금화면(2026-09-09 요청) - 20초마다 OS 전체 유휴시간(GetLastInputInfo, 이 앱에
    // 포커스가 없어도 감지됨)을 확인해서 AppConfig.IdleTimeoutMinutes(frmSiteConfig에서 관리자가
    // 지정, 0/null이면 비활성)를 넘기면 LockScreenForm을 모달로 띄운다. _lockScreenShowing은
    // 잠금화면이 이미 떠 있는 동안 타이머가 중복으로 또 띄우지 못하게 막는 가드.
    private readonly System.Windows.Forms.Timer _idleCheckTimer = new() { Interval = 20_000 };
    private bool _lockScreenShowing;

    // "메뉴 접기/펼치기"와 "홈" 두 버튼의 자리를 서로 바꿨다(2026-09-03) - 이 필드는 원래
    // "홈" 버튼이었고, 헤더 툴바 맨 앞의 버튼은 원래 "메뉴 접기/펼치기"였다. 여기(사이드바
    // 상단 여백, 메뉴트리 바로 위)엔 이제 메뉴 접기/펼치기가 있다 - 클릭 핸들러/아이콘만
    // ConfigureSidebarTopGap에서 바꿔치기했고, 버튼 자체(크기/배치)는 그대로 재사용한다.
    // IconInset을 기본값(9, 헤더 54x48 버튼 기준)보다 줄여야 이 작은 크기에서도 아이콘이
    // 실제로 보인다 - 처음엔 기본값 그대로 썼다가 아이콘이 점처럼 작아져 거의 안 보였다.
    // 배치는 ConfigureSidebarTopGap 참고(Dock=Right로 직접 붙이면 세로로 늘어난다).
    private readonly IconBadgeButton sidebarToggleButton = new()
    {
        Text = "메뉴 접기/펼치기",
        // 34 -> 28 -> 24: 검색창과 나란히 놓으면서 딱 맞춰(28) 줄였더니 여백이 0이라 흰
        // 배경에서 아래쪽이 살짝 잘려 보였다("잘렸어" 지적, 2026-09-17) - 24로 한 번 더
        // 줄여서 ConfigureSidebarTopGap의 내용 높이(28) 안에 위아래 2px씩 여백이 남게 했다.
        Size = new Size(24, 24),
        IconInset = 4
    };

    private readonly Dictionary<string, string> _envLabels = new()
    {
        ["Development"] = "개발서버",
        ["Production"] = "운영서버"
    };

    private bool _suppressEnvChange;

    // 직접 그린 제목줄(BuildTitleBar) 구성요소 - RefreshTitleBarStatus가 다시 그릴 때 필요해서
    // 필드로 들고 있는다.
    private readonly Panel titleBarPanel = new() { Dock = DockStyle.Top, Height = 36 };
    // SERVICE 선택을 메뉴검색(cboMenuSearch)처럼 "제목 없는 룩업" 모양으로 그리기 위한 껍데기
    // (2026-09-17 요청) - LabelControl 하나로는 알약 배경+드롭다운 세모를 같이 못 그려서, 얇은
    // Panel(_titleBarEnvPill)에 배경/세모를 직접 그리고 그 안에 텍스트 라벨만 Dock=Fill로 얹는다.
    private Panel _titleBarEnvPill = null!;
    private readonly LabelControl _titleBarEnvLabel = new();
    private readonly LabelControl _titleBarUserLabel = new();
    private readonly ContextMenuStrip _envSwitchMenu = new();

    public ShellForm()
    {
        IsMdiContainer = true;
        RefreshTitle();
        // 제목줄을 직접 그리므로(BuildTitleBar) OS 기본 캡션은 꺼둔다 - 리사이즈 테두리는
        // 당장은 생략(회귀 위험 최소화, 클래스 선언부 주석 참고) - 앱이 항상 최대화로 뜨고
        // (WindowState=Maximized) 최대화 버튼으로도 복원 가능해서 실사용 영향은 적다.
        // ponytail: 가장자리 드래그 리사이즈 없음 - 필요해지면 WM_NCHITTEST 가장자리 판정 추가.
        FormBorderStyle = FormBorderStyle.None;
        WindowState = FormWindowState.Maximized;
        BackColor = Color.White;

        // FormBorderStyle.None이라 OS가 그려주던 얇은 창 테두리가 없다 - 복원(Normal) 상태로
        // 리사이즈하면 바탕화면과 경계가 전혀 안 보였다("MDI 배경이 바탕화면과 구분이 안
        // 된다" 지적, 2026-09-21). Form.Padding(1px)으로 만드는데, 절대좌표로 배치된
        // tabStripButtons가 이 Padding을 모른 채 ClientSize 기준으로 계산돼 있어서 처음엔
        // MDI 자식폼이 열렸을 때 탭 스트립 쪽에 테두리색이 비쳐 보이는 회귀가 있었다 -
        // RepositionTabStripButtons에서 Padding.Right를 반영해서 고쳤다.
        RefreshWindowBorder();
        Resize += (s, e) => RefreshWindowBorder();

        BuildTitleBar();

        toolbarToolTip.Appearance.Font = AppFonts.Caption;
        toolbarToolTip.Appearance.Options.UseFont = true;

        // 가로: 현재 헤더 툴바(조회/입력/삭제/행추가/행삭제/저장/출력 7개를 세 묶음으로 띄워
        // 한 줄에 배치)가 겹치지 않고 다 보이는 최소폭. 홈 버튼은 사이드바 메뉴트리 위로 옮겨서
        // 헤더에서 빠졌다.
        // 세로도 업무화면이 너무 눌리지 않도록 최소값을 둠.
        // 버튼을 54x48 -> 64x56으로 키우면서(BuildToolbar 주석 참고) 툴바 전체 폭도 같이
        // 늘어나, 겹치지 않는 최소폭도 그만큼 올렸다. 화면검색/서비스 콤보가 헤더 오른쪽으로
        // 옮겨오면서(BuildHeaderRightCombos, 2026-09-03) 그 폭(HeaderRightCombosWidth)만큼
        // 다시 늘렸다 - 안 늘리면 창을 좁힐 때 툴바 아이콘과 겹친다.
        MinimumSize = new Size(1020 + HeaderRightCombosWidth, 650);

        tabbedMdiManager.MdiParent = this;
        // MDI 탭 헤더에 X(닫기) 버튼 표시 + 탭 영역 맨 오른쪽에 "현재 탭 닫기" 버튼도 같이
        // 표시(InAllTabPagesAndTabControlHeader) - Home 탭은 BaseForm/HomeForm.OnFormClosing에서
        // 이미 닫기를 막고 있어서, 두 버튼 다 눌러도 실제로는 안 닫힌다.
        // (InAllTabPageHeaders만 쓰면 탭마다 X가 아예 안 뜨는 걸 확인해서(2026-09-02) 원복 -
        // 이 "현재 탭 닫기" 버튼과 tabStripButtons가 겹치는 문제는 NativeTabCloseButtonWidth만큼
        // tabStripButtons를 왼쪽으로 띄우는 방식으로 해결(2026-09-03, RepositionTabStripButtons
        // 참고) - DevExpress가 그 버튼 폭을 이벤트로 안 알려줘서 실측 대신 고정폭을 예약해뒀다.)
        tabbedMdiManager.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
        ConfigureTabAppearance();

        // 60 -> 52: "툴바 높이를 조금만 낮춰줘"(2026-09-17). ToolbarButtonY/SidebarTopGapMinHeight도
        // 같이 맞춰야 한다(각 상수 주석 참고).
        headerPanel = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = HeaderBg };
        // 관리자가 IconAssetProvider.AssetsFolder에 toolbar_background.png를 넣어두면 그 위에
        // 그려서 배경을 이미지로 바꿀 수 있게 한다 - 없으면(기본 상태) 그냥 BackColor(HeaderBg)
        // 그대로 보인다. Dock=Fill처럼 헤더 전체 크기에 맞춰 늘려 그린다.
        headerPanel.Paint += (s, e) =>
        {
            var bg = IconAssetProvider.GetImage("toolbar_background");
            if (bg != null) e.Graphics.DrawImage(bg, headerPanel.ClientRectangle);
        };
        sidebarPanel = new Panel { Dock = DockStyle.Left, Width = 212, BackColor = SidebarTreeBg };
        // 48 -> 42 -> 30까지 줄였더니 상태바가 너무 얇아져서 사이드바(메뉴트리)가 화면 아래
        // 끝까지 꽉 찬 것처럼 보였다("메뉴트리가 아래쪽까지 전체를 차지하고 있다"는 지적,
        // 2026-09-17) - "아래쪽 툴바를 전체 사이즈로 넓히고 그 위에 메뉴트리가 오도록"
        // 요청대로 이전에 "넓힌" 값이었던 48로 되돌렸다. 상태바가 Dock=Bottom(폼 전체
        // 너비)이라, 이 위에 얹힌 sidebarPanel(Dock=Left)의 실제 높이는 자동으로 그만큼
        // 줄어든다(Dock 레이아웃이 알아서 계산 - 별도 코드 불필요).
        // 높이는 30으로 되돌렸다("높이는 원래대로 줄여줘", 2026-09-17) - 문제는 높이가 아니라
        // 폭이었다(위 Controls.Add 순서 주석 참고), 폭만 고치고 높이는 이전(70%) 값 그대로 둔다.
        statusBar = new Panel { Dock = DockStyle.Bottom, Height = 30, BackColor = SidebarBg };

        BuildToolbar();
        BuildHeaderRightCombos();
        BuildSidebar();
        BuildAccordionMenu();
        BuildStatusBar();
        RefreshTitleBarStatus(); // 여기서 처음 불러야 한다 - SERVICE 알약이 이제 상태바 소속이라 그 전엔 없다.
        AppConfig.EnvironmentChanged += RefreshTitleBarStatus;
        BuildTabStripButtons();

        var headerBottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = HeaderDividerColor };
        headerPanel.Controls.Add(headerBottomBorder);

        // 관리자만 SQL 로그 패널을 만든다(BuildSqlLogPanel 클래스 설명 참고) - statusBar보다
        // 먼저 Controls에 들어가야(BuildSqlLogPanel 내부에서 처리) 패널이 상태바 위쪽에 온다.
        if (Session.UserType == "A") BuildSqlLogPanel();

        // 순서 중요 - Dock은 "나중에 추가한 쪽이 바깥쪽(먼저 차지)"이므로(이 파일 전체 규칙),
        // 원하는 최종 모양(제목줄이 맨 위 전체 폭 -> 그 아래 툴바도 전체 폭 -> 상태바가 맨
        // 아래 전체 폭 -> 그 사이에서 사이드바가 왼쪽만)을 만들려면 바깥쪽부터 titleBarPanel ->
        // statusBar -> headerPanel -> sidebarPanel 순으로 "차지"해야 한다 - 즉 추가는 그 역순
        // (sidebarPanel 먼저 ... titleBarPanel 마지막). 예전엔 headerPanel(툴바)이 sidebarPanel
        // 보다 먼저 차지해서 사이드바 오른쪽 영역에서만 그려졌는데(메뉴검색/트리닫기 버튼이
        // 사이드바 쪽에만 있던 시절) - "탐색창 없애고 툴바를 메뉴트리 영역까지 전체 폭으로
        // 확장해서 검색/닫기버튼도 그 안에 포함해달라"는 요청(2026-09-21)으로 순서를 뒤집어
        // headerPanel이 sidebarPanel보다 먼저(=바깥쪽에서) 전체 폭을 차지하게 했다.
        Controls.Add(sidebarPanel);
        Controls.Add(headerPanel);
        Controls.Add(statusBar);
        Controls.Add(titleBarPanel);
        // MDI 클라이언트 영역(탭 줄 포함) 위에 겹쳐 보이도록 맨 마지막에 추가 + BringToFront -
        // Z-order상 나중에 추가된 컨트롤이 위에 그려지는 WinForms 규칙을 그대로 이용한다.
        Controls.Add(tabStripButtons);
        tabStripButtons.BringToFront();
        Resize += (s, e) => RepositionTabStripButtons();
        RepositionTabStripButtons();

        AppConfig.EnvironmentChanged += RefreshTitle;

        FormClosing += ShellForm_FormClosing;

        // 활성 업무화면이 바뀔 때마다(다른 탭 클릭, 화면 열기/닫기 등) 툴바 아이콘의
        // 활성/비활성을 그 화면의 권한으로 다시 계산한다.
        MdiChildActivate += (s, e) => UpdateToolbarPermissions();

        // 홈 탭으로 돌아올 때마다 대시보드(공지사항/전자결재)를 다시 불러온다 - 최초 로드 한
        // 번뿐이면 다른 화면(공지사항등록 등)에서 뭔가 바꾸고 홈으로 돌아와도 화면이 그대로였다
        // (2026-09-17 지적 - "공지사항등록(게시체크) 후 홈화면에서 새로고침이 없네").
        MdiChildActivate += (s, e) => { if (ActiveMdiChild is HomeForm home) _ = home.RefreshDashboardAsync(); };

        OpenHomeForm();

        _ = ReloadFavoriteMenusAsync();

        _idleCheckTimer.Tick += (s, e) => CheckIdleLock();
        _idleCheckTimer.Start();
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

    private bool _exitConfirmed;

    /// <summary>사용자가 셸의 X버튼으로 직접 닫으려 할 때만 확인 - 서버전환 취소 등 프로그램 내부에서
    /// Application.Exit()을 호출하는 경우는 이미 그 자리에서 확인을 거친 것이므로 재확인하지 않는다.
    ///
    /// "종료하시겠습니까?"에 예를 누른 뒤, 실제로 Close()를 부르기 전에 열려있는 화면들을
    /// 전부 BaseForm.ConfirmCloseAsync로 미리 확인한다(CloseAllMdiChildren과 같은 패턴) - MDI
    /// 부모(이 폼)가 실제로 닫히기 시작하면 자식들에게 CloseReason.MdiFormClosing이 동기적으로
    /// 전파되는데, 그 시점에 자식이 비동기로 "잠깐만요" 하며 취소하면 부모 자신의 종료 판정이
    /// 그 자리에서 거부된 것으로 처리된다(BaseForm.BaseForm_FormClosing 주석 참고 - 실제로 겪음:
    /// 화면을 하나도 안 열어도 홈 탭 때문에 종료 확인이 두 번 떠야 실제로 닫혔다). 그래서 자식들
    /// 확인은 전부 여기서 미리 끝내두고, 실제 Close() 호출 시점엔 모든 자식이 이미 "닫혀도 됨"
    /// 상태이거나 사라진 뒤라 그 동기적 판정이 절대 걸리지 않는다.</summary>
    private async void ShellForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.UserClosing) return;
        if (_exitConfirmed) return;

        e.Cancel = true;

        var confirm = AppMessageBox.Show("모든 프로그램을 종료하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        foreach (var child in MdiChildren)
        {
            if (child is HomeForm) continue;
            if (child is BaseForm baseChild && !await baseChild.ConfirmCloseAsync()) return; // 저장 실패 - 종료 중단, 그 화면은 열어둔 채로 둔다
        }

        _exitConfirmed = true;
        Close();
    }

    /// <summary>창 제목표시줄(OS가 그리는 영역) - 개발서버 경고문구까지 전부 여기 한 줄로 담는다.
    /// [2026-09-02] 한 번은 "taskbar/Alt+Tab에서 잘려 보인다"는 이유로 경고문구를 빼서 화면 안
    /// 배너(envBanner)로 옮겼었는데, [2026-09-03] "제목줄+배너가 막대 두 개로 따로 놀아 보인다"는
    /// 지적으로 다시 여기 한 줄로 합치고 envBanner는 없앴다 - 커스텀 캡션(OS 제목줄을 직접 그려
    /// 하나로 잇는 방식)도 시도했지만 DevExpress XtraForm의 비클라이언트 처리와 충돌해 창 드래그
    /// 자체가 안 되는 회귀가 나서 포기했고(WndProc 관련 코드 전부 원복), 결국 "막대가 하나로
    /// 보여야 한다"는 요구를 OS 막대 자체를 없애는 대신 "막대를 하나만 쓴다"는 방식으로 만족시켰다 -
    /// 길어서 taskbar/Alt+Tab에서 잘리는 건 감수한다(사용자가 명시적으로 이 방향을 선택함).</summary>
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
    /// 텍스트/닫기 버튼은 DevExpress 기본 로직(DefaultDraw*)에 그대로 맡긴다. 아이콘은 그리지
    /// 않는다 - Form.Icon을 지정한 화면이 하나도 없어서 DevExpress 기본 아이콘(의미 없는
    /// 기본 폼 아이콘)만 나오던 걸 2026-09-07에 없앴다(탭 둥글게 만들 때 DefaultDrawImage를
    /// 같이 넣은 게 원인 - 8185c32).
    /// 활성 탭 강조는 배경색 차이 하나로만 표현한다(처음엔 상단 강조색 바를 더했었는데,
    /// 과하다는 피드백을 받아 배경색만 남기고 단순화했다).
    /// </summary>
    private void TabbedMdiManager_CustomDrawTabHeader(object? sender, TabHeaderCustomDrawEventArgs e)
    {
        // tabStripButtons(탭 줄 오른쪽 끝 아이콘 스트립)는 탭 줄의 실제 높이(패딩 없는
        // 원본값)를 그대로 따라간다 - 시각적으로 진짜 탭 줄과 같은 자리에 겹쳐야 하기 때문.
        // (예전엔 여기서 사이드바 상단 여백 높이도 같이 동기화했었는데, headerPanel이 사이드바
        // 위까지 전체 폭을 차지하는 구조로 바뀌면서 그 동기화 자체가 필요 없어졌다 - BuildSidebar 참고.)
        var tabRowHeight = e.TabHeaderRowInfo.Bounds.Height;
        if (tabRowHeight > 0 && tabStripButtons.Height != tabRowHeight)
        {
            tabStripButtons.Height = tabRowHeight;
            RepositionTabStripButtons();
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
    /// Close() 중 컬렉션이 바뀌어도 이 foreach 자체는 안전하다.
    ///
    /// 화면마다 저장 안 된 변경이 있으면 닫기 전에 물어야 한다(BaseForm.ConfirmCloseAsync) -
    /// 여기서 먼저 await로 확인받고 나서 Close()를 부르면, 그 Close()가 다시 FormClosing을
    /// 태워도 이미 확인된 상태라 재질문 없이 바로 닫힌다. 확인 메시지박스 자체가 동기적으로
    /// 화면을 막고 서 있어서(첫 await 지점 이전) 다음 화면으로 넘어가기 전에 사용자가 반드시
    /// 답해야 하므로, 여러 화면이 한꺼번에 물어보는 대신 탭 순서대로 하나씩 순차적으로 묻는다.
    ///
    /// "전체 닫기"(except==null - 탭 줄의 [탭 전체 닫기] 아이콘, 우클릭 메뉴의 "모두 닫기")는
    /// 실행 전에 한 번 더 확인한다(2026-09-16 요청) - 실수로 눌러서 여러 화면이 한꺼번에 닫히는
    /// 걸 막기 위함. "다른 탭 모두 닫기"(except!=null, 지금 보는 화면은 남기는 동작)는 대상이
    /// 다르므로 이 확인 대상이 아니다. 닫을 탭이 애초에 없으면(홈만 열려있음) 물을 필요도 없다.</summary>
    private async void CloseAllMdiChildren(Form? except)
    {
        if (except == null)
        {
            if (!MdiChildren.Any(c => c is not HomeForm)) return;

            var confirm = AppMessageBox.Show(
                "현재 열려있는 모든 탭을 닫으시겠습니까?",
                "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
        }

        foreach (var child in MdiChildren)
        {
            if (child is HomeForm) continue;
            if (except != null && child == except) continue;

            if (child is BaseForm baseChild && !await baseChild.ConfirmCloseAsync()) continue; // 저장 실패 - 이 화면은 열어둔 채 다음으로

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
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = NavDivider };
        statusBar.Controls.Add(topBorder);

        const int contentHeight = 22;
        var statusBarContentTop = topBorder.Height + (statusBar.Height - topBorder.Height - contentHeight) / 2;

        // SERVICE 선택 알약 - 제목줄 우측에 있던 걸 상태바 좌측(예전 SQL 로그 아이콘 자리)으로
        // 옮겼다(2026-09-21 요청 - "서비스선택 룩업을 현재 프로필러위치로 이동"). 실제 배경/
        // 폭 계산 로직은 ConfigureTitleBarEnvPill/ResizeTitleBarEnvPill 그대로 재사용한다.
        ConfigureTitleBarEnvPill();
        _titleBarEnvPill.Location = new Point(14, statusBarContentTop);
        _titleBarEnvPill.Height = contentHeight;
        statusBar.Controls.Add(_titleBarEnvPill);
        foreach (var envKey in AppConfig.AvailableEnvironments)
        {
            var label = GetEnvLabel(envKey);
            _envSwitchMenu.Items.Add(label, null, (s, e) => cboEnvironment.SelectedItem = label);
        }

        var messageX = _titleBarEnvPill.Right + 14;

        // SQL 로그(프로필러) 토글 칩(관리자 전용) - SERVICE 알약이 좌측 자리를 넘겨받으면서
        // 우측(lblStatusRight 옆)으로 옮겼다. 보더 없이 아이콘 자체만 보이도록
        // BorderColor를 투명으로 뺐다("버튼 보더 없애고 아이콘 자체만 클릭하도록" 요청,
        // 2026-09-21) - 클릭 가능 영역 자체는 그대로(칩 전체)라 동작은 이전과 같다.
        if (Session.UserType == "A")
        {
            _sqlLogChip = new IconChipButton
            {
                // NavText(어두운 슬레이트)는 라이트 사이드바 트리용 색이라 이 어두운 상태바
                // 배경 위에서는 거의 안 보였다 - lblStatusRight가 겪었던 것과 같은 문제
                // (line 833 주석 참고), 같은 해결책(거의-흰색 톤)으로 맞춘다.
                IconImage = SvgIcons.Load(SvgIcons.ToolbarSqlLog, 16, Color.FromArgb(245, 246, 248)),
                Height = contentHeight,
                DefaultTextColor = Color.FromArgb(245, 246, 248),
                BorderColor = Color.Transparent,
                DefaultHoverBg = Color.FromArgb(28, 255, 255, 255),
                DefaultPressedBg = Color.FromArgb(45, 255, 255, 255)
            };
            _sqlLogChip.Click += (s, e) => { if (_sqlLogPanel != null) _sqlLogPanel.Visible = !_sqlLogPanel.Visible; };
            toolbarToolTip.SetToolTip(_sqlLogChip, "SQL 로그");
            statusBar.Controls.Add(_sqlLogChip);
        }

        lblStatusMessage.Location = new Point(messageX, statusBarContentTop + (contentHeight - 18) / 2);
        lblStatusMessage.AutoSizeMode = LabelAutoSizeMode.None;
        lblStatusMessage.Size = new Size(500, 18);
        lblStatusMessage.Appearance.ForeColor = NavText;
        lblStatusMessage.Appearance.Font = AppFonts.Caption;

        lblStatusRight.AutoSizeMode = LabelAutoSizeMode.None;
        lblStatusRight.Size = new Size(320, 18);
        lblStatusRight.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        // NavTextMuted -> NavText -> 245,246,248(2026-09-21, "폰트가 더 안보여" - NavText로는
        // 여전히 부족했다): _titleBarUserLabel과 같은 거의-흰색 톤으로 확실히 밝게. SERVICE
        // 룩업 텍스트(_titleBarEnvLabel)도 같은 값을 쓰므로 이제 둘이 같은 톤이다.
        lblStatusRight.Appearance.ForeColor = Color.FromArgb(245, 246, 248);
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
        lblStatusRight.Location = new Point(statusBar.Width - lblStatusRight.Width - 14, lblStatusMessage.Top);
        if (_sqlLogChip != null)
            _sqlLogChip.Location = new Point(lblStatusRight.Left - _sqlLogChip.Width - 10, lblStatusMessage.Top);
    }

    private void RefreshStatusRight()
    {
        var envLabel = GetEnvLabel(AppConfig.CurrentEnvironment);
        lblStatusRight.Text = $"[Service : WYN LAB]  [{envLabel}]  v{DateTime.Now:yyyy.MM.dd}";
    }

    /// <summary>직접 그린 제목줄(titleBarPanel) - 흰 배경에 브랜드 마크, 사이드바 폭에 맞춘
    /// 위치에 접속정보(아이콘+라벨), 우측에 SERVICE 알약 + 최소화/최대화/닫기를 올린다. 드래그
    /// 이동은 WM_NCHITTEST를 가로채는 대신 표준 트릭(ReleaseCapture+WM_NCLBUTTONDOWN)을 쓴다 -
    /// Windows에게 "지금부터 진짜 캡션을 드래그하는 것처럼 처리해라"라고 위임하는 것이라 Aero
    /// Snap도 그대로 따라오고, XtraForm의 비클라이언트 처리와 충돌할 여지도 없다(클래스 선언부
    /// 주석 참고).</summary>
    private void BuildTitleBar()
    {
        // 흰 배경으로 바꿨다가(2026-09-17) "로고/사용자정보 있는 곳을 원래 색깔대로 돌려줘"
        // 요청으로 같은 날 다시 원래의 짙은 남색(NavDarkBg)으로 되돌렸다 - 그에 맞춰 아래
        // 색 관련 값들(브랜드 라벨, 사용자정보 라벨, 사용자 아이콘, 버튼 글리프, 호버색)도
        // 전부 밝은 톤으로 같이 되돌렸다.
        titleBarPanel.BackColor = NavDarkBg;

        var logo = new Panel { Location = new Point(14, 8), Size = new Size(20, 20) };
        logo.Paint += (s, e) => LogoPainter.Draw(e.Graphics, logo.ClientRectangle, darkBackground: true);

        var brandLabel = new LabelControl
        {
            Text = "WYN LAB",
            Location = new Point(42, 6),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(140, 24)
        };
        brandLabel.Appearance.ForeColor = Color.White;
        brandLabel.Appearance.Font = AppFonts.SubHeading;
        brandLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

        var btnClose = BuildTitleBarButton(DrawCloseGlyph, isClose: true);
        btnClose.Click += (s, e) => Close();

        _btnMaxRestore = BuildTitleBarButton(DrawMaxRestoreGlyph, isClose: false);
        _btnMaxRestore.Click += (s, e) => ToggleMaximizeRestore();

        var btnMin = BuildTitleBarButton(DrawMinimizeGlyph, isClose: false);
        btnMin.Click += (s, e) => WindowState = FormWindowState.Minimized;

        // 접속정보(아이콘+라벨) - 예전엔 우측 버튼 무리 안에서 Dock=Right로 오른쪽 끝에 붙어
        // 있었는데, "메뉴트리 끝선과 맞춰서 저 위치로"라는 요청(2026-09-17)으로 고정 좌표
        // (사이드바 폭=SidebarExpandedWidth과 같은 X)에 왼쪽 정렬로 옮겼다 - 사이드바 폭이
        // 고정값이라 이쪽도 창 크기와 무관하게 고정 좌표로 둔다(Dock 대신 Location).
        var loginIcon = new Panel { Location = new Point(SidebarExpandedWidth, 8), Size = new Size(20, 20) };
        loginIcon.Paint += (s, e) => DrawUserGlyph(e.Graphics, loginIcon.ClientRectangle);

        _titleBarUserLabel.Location = new Point(SidebarExpandedWidth + 26, 0);
        _titleBarUserLabel.Size = new Size(320, titleBarPanel.Height);
        _titleBarUserLabel.AutoSizeMode = LabelAutoSizeMode.None;
        _titleBarUserLabel.Appearance.ForeColor = Color.FromArgb(245, 246, 248);
        _titleBarUserLabel.Appearance.Font = AppFonts.Caption;
        _titleBarUserLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
        _titleBarUserLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

        titleBarPanel.Controls.Add(logo);
        titleBarPanel.Controls.Add(brandLabel);
        titleBarPanel.Controls.Add(loginIcon);
        titleBarPanel.Controls.Add(_titleBarUserLabel);
        // Dock=Right는 "먼저 추가한 쪽이 안쪽"이다(실제로 확인 - 처음엔 반대로 알고 있었다).
        // SERVICE 알약은 2026-09-21에 상태바로 옮겨서(ConfigureTitleBarEnvPill 설명 참고) 여기
        // 순서에서 빠졌다 - 최종 좌->우 순서는 이제 [최소화][최대화][닫기].
        titleBarPanel.Controls.Add(btnMin);
        titleBarPanel.Controls.Add(_btnMaxRestore);
        titleBarPanel.Controls.Add(btnClose);

        // ReleaseCapture+WM_NCLBUTTONDOWN(아래 MouseDown)은 Windows의 비클라이언트 드래그
        // 루프로 넘어가버려서 WinForms 자체의 더블클릭 타이머가 끝까지 못 돈다(실제로 겪음 -
        // MouseDoubleClick이 하나도 안 잡힘) - 그래서 더블클릭 판정은 직접 시간/위치를 재서
        // 드래그를 넘기기 "전"에 가로챈다(표준 회피책).
        titleBarPanel.MouseDown += (s, e) =>
        {
            if (e.Button != MouseButtons.Left) return;

            var now = DateTime.UtcNow;
            var isDoubleClick = (now - _titleBarLastClick) <= TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime)
                                 && (Math.Abs(e.X - _titleBarLastClickPos.X) <= SystemInformation.DoubleClickSize.Width)
                                 && (Math.Abs(e.Y - _titleBarLastClickPos.Y) <= SystemInformation.DoubleClickSize.Height);
            _titleBarLastClick = isDoubleClick ? DateTime.MinValue : now; // 세 번째 클릭이 또 더블클릭으로 안 잡히게 리셋
            _titleBarLastClickPos = e.Location;

            if (isDoubleClick)
            {
                ToggleMaximizeRestore();
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        };
        Resize += (s, e) => _btnMaxRestore.Invalidate();

        // RefreshTitleBarStatus 호출은 BuildStatusBar 이후로 미뤘다 - 그 메서드가 이제
        // SERVICE 알약(_titleBarEnvPill, 2026-09-21부터 상태바 소속)도 같이 갱신하는데,
        // 여기(BuildTitleBar) 시점엔 statusBar/그 알약이 아직 만들어지기 전이다.
    }

    private DateTime _titleBarLastClick = DateTime.MinValue;
    private Point _titleBarLastClickPos;

    // SERVICE 알약 안쪽 텍스트 좌/우 여백 - 오른쪽은 드롭다운 세모(EnvPillTriangleArea)가
    // 들어갈 자리라 왼쪽보다 넓게 둔다.
    private const int EnvPillPaddingLeft = 10;
    private const int EnvPillPaddingRight = 20;

    /// <summary>SERVICE 선택을 메뉴검색(cboMenuSearch)과 같은 "제목 없는 룩업" 모양으로
    /// 그린다(2026-09-17 요청 - "SERVICE라는 제목 없이 룩업처럼, 왼쪽 아이콘 없이 오른쪽
    /// 세모만"). 알약 배경+세모는 _titleBarEnvPill(Panel)의 Paint에서 직접 그리고, 그 안에
    /// _titleBarEnvLabel을 Dock=Fill로 얹어 텍스트만 담당하게 한다.</summary>
    private void ConfigureTitleBarEnvPill()
    {
        // 2026-09-21: 제목줄 우측에서 상태바 좌측(예전 SQL 로그 아이콘 자리)으로 옮겼다
        // ("서비스선택 룩업을 현재 프로필러위치로 이동" 요청) - Dock=Right 대신 호출부
        // (BuildStatusBar)가 Location/Height를 직접 정해준다. 배경 대비도 "글자 안보임,
        // 룩업 형태 보이도록 조금 연하게" 요청대로 이전(제목줄 배경+17, 거의 안 보이는
        // 수준)보다 훨씬 밝게 statusBar 배경과 섞는다.
        _titleBarEnvPill = new Panel { Cursor = Cursors.Hand };
        _titleBarEnvPill.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var pillRect = new Rectangle(0, 0, _titleBarEnvPill.Width, _titleBarEnvPill.Height);
            using (var path = RoundedRect(pillRect, 6))
            using (var fillBrush = new SolidBrush(ColorHelper.Mix(statusBar.BackColor, Color.White, 0.22f)))
                e.Graphics.FillPath(fillBrush, path);

            var cx = _titleBarEnvPill.Width - EnvPillPaddingRight / 2f;
            var cy = _titleBarEnvPill.Height / 2f;
            using var triangleBrush = new SolidBrush(NavTextMuted);
            e.Graphics.FillPolygon(triangleBrush, new[]
            {
                new PointF(cx - 4, cy - 2.5f), new PointF(cx + 4, cy - 2.5f), new PointF(cx, cy + 3.5f)
            });
        };
        _titleBarEnvPill.Click += (s, e) => _envSwitchMenu.Show(_titleBarEnvPill, new Point(0, _titleBarEnvPill.Height));

        _titleBarEnvLabel.Dock = DockStyle.Fill;
        _titleBarEnvLabel.AutoSizeMode = LabelAutoSizeMode.None;
        _titleBarEnvLabel.Padding = new Padding(EnvPillPaddingLeft, 0, EnvPillPaddingRight, 0);
        _titleBarEnvLabel.Appearance.ForeColor = Color.FromArgb(245, 246, 248);
        _titleBarEnvLabel.Appearance.Font = AppFonts.Caption;
        _titleBarEnvLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
        _titleBarEnvLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        _titleBarEnvLabel.Cursor = Cursors.Hand;
        _titleBarEnvLabel.Click += (s, e) => _envSwitchMenu.Show(_titleBarEnvPill, new Point(0, _titleBarEnvPill.Height));
        _titleBarEnvPill.Controls.Add(_titleBarEnvLabel);
    }

    /// <summary>고정폭 대신 실제 텍스트 길이에 맞춰 알약 폭을 다시 계산한다 - 고정폭이면 짧은
    /// 텍스트일 때 빈 공간이 남고, 긴 텍스트일 때 잘리는 문제가 둘 다 있었다(2026-09-17
    /// 지적: "날짜 시간이 잘렸어. 아이콘은 너무 떨어져 있고" - 같은 문제를 알약에도 그대로
    /// 적용해 예방한다).</summary>
    private void ResizeTitleBarEnvPill()
    {
        var textWidth = TextRenderer.MeasureText(_titleBarEnvLabel.Text, AppFonts.Caption).Width;
        // *1.5: "서비스 선택 부분을 width를 지금의 1.5배로 키워줘"(2026-09-17) - 늘어난 폭은
        // 텍스트와 오른쪽 세모 사이 빈 공간으로 흡수된다(세모는 항상 오른쪽 끝에 고정).
        _titleBarEnvPill.Width = (int)((textWidth + EnvPillPaddingLeft + EnvPillPaddingRight) * 1.5);
    }

    private Panel _btnMaxRestore = null!;
    private static readonly Color TitleBarGlyphColor = Color.FromArgb(220, 222, 226);

    private void ToggleMaximizeRestore()
    {
        WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        _btnMaxRestore.Invalidate();
    }

    // 복원(Normal) 상태에서만 보이는 창 테두리색 - 짙은 회색 -> Silver -> Gainsboro(2026-09-21,
    // "실버보다 더 밝은 색으로"). tabStripButtons(홈/탭목록/전체닫기 배경)도 같은 색을 쓴다.
    private static readonly Color WindowBorderColor = Color.Gainsboro;

    /// <summary>FormBorderStyle.None에는 OS가 그려주던 1px 창 테두리가 없다 - 최대화 상태에선
    /// 화면 가장자리와 맞닿아 안 보이지만, 복원(Normal) 상태로 리사이즈하면 바탕화면과 경계가
    /// 전혀 안 보였다. Form.Padding을 1px 주면 도킹된 자식들이 자동으로 그만큼 안쪽으로
    /// 밀려나고, 그 바깥쪽 1px 테두리 자리에 Form 자신의 BackColor(테두리색)가 비쳐 보인다 -
    /// 최대화일 땐 이 여백 자체가 필요 없어서 꺼둔다. 절대좌표로 배치된 tabStripButtons는
    /// 이 Padding을 스스로 반영해야 한다(RepositionTabStripButtons 참고) - 안 그러면 MDI
    /// 자식폼이 열렸을 때 탭 스트립 쪽에 테두리색이 비쳐 보인다.</summary>
    private void RefreshWindowBorder()
    {
        // 주의: 생성자 초반(headerPanel 등이 아직 안 만들어졌을 때)에도 불리므로, 여기서
        // RepositionTabStripButtons()를 직접 부르면 안 된다(NullReferenceException) - Resize
        // 이벤트가 이미 따로 그 메서드를 부르고 있고(같은 Resize 시점에 이 메서드도 불림),
        // 생성자 맨 마지막에도 한 번 더 명시적으로 부른다.
        var bordered = WindowState == FormWindowState.Normal;
        Padding = bordered ? new Padding(1) : new Padding(0);
        BackColor = bordered ? WindowBorderColor : Color.White;
    }


    /// <summary>최소화/최대화/닫기 아이콘을 폰트 글리프(✕, ❐ 등) 대신 직접 그린다 - 유니코드
    /// 기호는 폰트마다 지원 여부가 달라 깨지거나 흐리게 보일 수 있다(실제로 겪음, 2026-09-17) -
    /// 벡터로 직접 그리면 항상 또렷하다.</summary>
    private Panel BuildTitleBarButton(Action<Graphics, Rectangle> drawGlyph, bool isClose)
    {
        var btn = new Panel { Dock = DockStyle.Right, Width = 46, Cursor = Cursors.Hand, BackColor = titleBarPanel.BackColor };
        btn.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            drawGlyph(e.Graphics, btn.ClientRectangle);
        };

        var hoverColor = isClose ? Color.FromArgb(220, 38, 38) : ColorHelper.Adjust(titleBarPanel.BackColor, 25);
        btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
        btn.MouseLeave += (s, e) => btn.BackColor = titleBarPanel.BackColor;
        return btn;
    }

    /// <summary>사용자정보 앞에 붙는 작은 사람 아이콘 - 머리(원) + 어깨(반타원)만으로 표현한
    /// 최소한의 실루엣(다른 제목줄 아이콘들과 같은 벡터 직접 그리기 방식). 원형 테두리를 둘러
    /// 아바타 배지처럼 도드라지게 한다("눈에 띄게 해달라" 요청, 2026-09-21).</summary>
    private static void DrawUserGlyph(Graphics g, Rectangle bounds)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var circleRect = Rectangle.Inflate(bounds, -1, -1);
        using (var borderPen = new Pen(NavTextMuted, 1.2f))
            g.DrawEllipse(borderPen, circleRect);

        using var brush = new SolidBrush(NavText);
        var cx = bounds.Width / 2f;
        const float headR = 4f;
        var headCenterY = bounds.Height / 2f - 4f;
        g.FillEllipse(brush, cx - headR, headCenterY - headR, headR * 2, headR * 2);

        var shoulders = new RectangleF(cx - 7f, headCenterY + headR - 1f, 14f, 11f);
        using var path = new GraphicsPath();
        path.AddArc(shoulders, 180, 180);
        path.CloseFigure();
        g.FillPath(brush, path);
    }

    private static void DrawCloseGlyph(Graphics g, Rectangle bounds)
    {
        using var pen = new Pen(TitleBarGlyphColor, 1.4f);
        var cx = bounds.Width / 2;
        var cy = bounds.Height / 2;
        const int half = 5;
        g.DrawLine(pen, cx - half, cy - half, cx + half, cy + half);
        g.DrawLine(pen, cx - half, cy + half, cx + half, cy - half);
    }

    private static void DrawMinimizeGlyph(Graphics g, Rectangle bounds)
    {
        using var pen = new Pen(TitleBarGlyphColor, 1.4f);
        var cx = bounds.Width / 2;
        var cy = bounds.Height / 2;
        g.DrawLine(pen, cx - 5, cy, cx + 5, cy);
    }

    private void DrawMaxRestoreGlyph(Graphics g, Rectangle bounds)
    {
        using var pen = new Pen(TitleBarGlyphColor, 1.2f);
        var cx = bounds.Width / 2;
        var cy = bounds.Height / 2;
        if (WindowState == FormWindowState.Maximized)
        {
            const int size = 9;
            g.DrawRectangle(pen, cx - size / 2 + 2, cy - size / 2 - 2, size, size);
            g.DrawRectangle(pen, cx - size / 2 - 2, cy - size / 2 + 2, size, size);
        }
        else
        {
            const int size = 10;
            g.DrawRectangle(pen, cx - size / 2, cy - size / 2, size, size);
        }
    }

    /// <summary>제목줄 우측의 SERVICE/서버 상태 + 사용자명/접속시각. 로그인/로그아웃/서버 전환
    /// 시점마다(AppConfig.EnvironmentChanged 구독 + 로그인 직후 명시적 재호출, 2026-09-06 -
    /// 이벤트가 로그인보다 먼저 지나가버려 값이 최신이 아니었던 문제) 다시 불러야 한다.</summary>
    private void RefreshTitleBarStatus()
    {
        // "SERVICE" 제목 없이 값만 - 메뉴검색과 같은 룩업 모양이라 라벨 없이도 이게 서버
        // 선택 필드라는 게 드러난다(2026-09-17 요청).
        _titleBarEnvLabel.Text = GetEnvLabel(AppConfig.CurrentEnvironment);
        ResizeTitleBarEnvPill();

        var user = SessionManager.Current.UserInfo;
        // 형식 변경: "이름 접속 : yyyy-MM-dd"(~2026-09-16) -> "이름  LOGIN : yyyy-MM-dd HH:mm"
        // (2026-09-17) -> "이름[LOGIN : yyyy-MM-dd  HH:mm]"(2026-09-21, 대괄호로 묶어달라는 요청).
        _titleBarUserLabel.Text = user == null
            ? string.Empty
            : SessionManager.Current.SignInTime is { } t
                ? $"{user.UserNm}[LOGIN : {t:yyyy-MM-dd}  {t:HH:mm}]"
                : user.UserNm;
        // 고정폭(320)으로 두면 실제 글자보다 훨씬 넓은 자리를 차지해서, 텍스트가 끝난 뒤의
        // 빈 공간을 더블클릭해도 이 라벨(자식 컨트롤)이 클릭을 가로채 제목줄 자체의
        // 더블클릭-최대화가 씹혔다("저 부분만 더블클릭해도 최대화가 안 된다" 지적,
        // 2026-09-18) - 실제 글자 길이만큼만 폭을 줘서 빈 공간을 없앤다.
        _titleBarUserLabel.Width = TextRenderer.MeasureText(_titleBarUserLabel.Text, AppFonts.Caption).Width + 4;
    }

    [DllImport("user32.dll")] private static extern bool ReleaseCapture();
    [DllImport("user32.dll")] private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 2;
    private const int WM_GETMINMAXINFO = 0x0024;

    // FormBorderStyle.None으로 바꾸면서 OS가 기본으로 주던 "가장자리 드래그로 리사이즈"가
    // 같이 없어졌다 - 항상 최대화로 띄우고 최대화 버튼도 있어서 처음엔 미뤄뒀는데("가장자리
    // 드래그 리사이즈 없음", 기존 메모), 창을 복원(Normal) 상태로 쓸 때 리사이즈가 안 된다는
    // 요청(2026-09-18)으로 추가한다. WM_NCHITTEST에 직접 응답해서 "커서가 이 근처에 있으면
    // 이건 왼쪽/오른쪽/위/아래 가장자리다"라고 알려주면, 그 다음 드래그는 OS가 일반 창과
    // 똑같이 처리한다(제목줄 드래그 때 쓴 ReleaseCapture 트릭과 같은 원리 - 우리가 손대는 건
    // "이게 무슨 영역인지" 뿐, 실제 리사이즈 루프는 OS가 그대로 돈다).
    private const int WM_NCHITTEST = 0x0084;
    private const int HTLEFT = 10;
    private const int HTRIGHT = 11;
    private const int HTTOP = 12;
    private const int HTTOPLEFT = 13;
    private const int HTTOPRIGHT = 14;
    private const int HTBOTTOM = 15;
    private const int HTBOTTOMLEFT = 16;
    private const int HTBOTTOMRIGHT = 17;
    private const int ResizeBorderThickness = 6;

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    [StructLayout(LayoutKind.Sequential)]
    private struct MINMAXINFO
    {
        public POINT Reserved;
        public POINT MaxSize;
        public POINT MaxPosition;
        public POINT MinTrackSize;
        public POINT MaxTrackSize;
    }

    /// <summary>FormBorderStyle.None + WindowState.Maximized 조합의 흔한 버그(작업표시줄을
    /// 가리며 화면 밖으로 넘침) 방지 - 최대화 시 실제 작업영역(Screen.WorkingArea)만큼만 커지게
    /// WM_GETMINMAXINFO에 직접 값을 채워준다(테두리가 있는 일반 창은 OS가 알아서 해주지만,
    /// None은 그 처리가 빠져서 직접 해줘야 한다 - 이 조합에서 거의 표준으로 쓰이는 대응).</summary>
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_GETMINMAXINFO)
        {
            var mmi = Marshal.PtrToStructure<MINMAXINFO>(m.LParam);
            var screen = Screen.FromHandle(Handle);
            mmi.MaxPosition.X = screen.WorkingArea.Left - screen.Bounds.Left;
            mmi.MaxPosition.Y = screen.WorkingArea.Top - screen.Bounds.Top;
            mmi.MaxSize.X = screen.WorkingArea.Width;
            mmi.MaxSize.Y = screen.WorkingArea.Height;
            Marshal.StructureToPtr(mmi, m.LParam, true);
        }
        else if (m.Msg == WM_NCHITTEST && WindowState == FormWindowState.Normal)
        {
            base.WndProc(ref m);
            if ((int)m.Result != 1 /* HTCLIENT */) return; // 다른 컨트롤(예: 제목줄)이 이미 답을 정했으면 그대로 둔다.

            // LParam은 화면 좌표를 (x,y) 각각 16비트로 눌러 담은 값 - 음수 좌표(다중 모니터)도
            // 있을 수 있어 short로 캐스팅해서 부호를 살린다.
            var lp = m.LParam.ToInt32();
            var screenPoint = new Point(unchecked((short)(lp & 0xFFFF)), unchecked((short)((lp >> 16) & 0xFFFF)));
            var p = PointToClient(screenPoint);

            var onLeft = p.X <= ResizeBorderThickness;
            var onRight = p.X >= ClientSize.Width - ResizeBorderThickness;
            var onTop = p.Y <= ResizeBorderThickness;
            var onBottom = p.Y >= ClientSize.Height - ResizeBorderThickness;

            if (onTop && onLeft) m.Result = (IntPtr)HTTOPLEFT;
            else if (onTop && onRight) m.Result = (IntPtr)HTTOPRIGHT;
            else if (onBottom && onLeft) m.Result = (IntPtr)HTBOTTOMLEFT;
            else if (onBottom && onRight) m.Result = (IntPtr)HTBOTTOMRIGHT;
            else if (onLeft) m.Result = (IntPtr)HTLEFT;
            else if (onRight) m.Result = (IntPtr)HTRIGHT;
            else if (onTop) m.Result = (IntPtr)HTTOP;
            else if (onBottom) m.Result = (IntPtr)HTBOTTOM;
            return;
        }
        base.WndProc(ref m);
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

        var toolbar = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = SqlLogToolbarBg };
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

        AddSqlLogColumn(view, nameof(ApiCallLogEntry.MenuId), "메뉴", 90);
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

    /// <summary>좌측 사이드바 - 메뉴 아코디언(accordionMenu) 하나만 담는다. 우측 구분선으로
    /// MDI(흰색) 영역과 시각적으로 분리. 검색창/트리닫기 버튼은 이제 헤더 툴바 쪽에 있다
    /// (BuildHeaderSearchZone, 2026-09-21).</summary>
    private void BuildSidebar()
    {
        accordionMenu.BackColor = SidebarTreeBg;
        // 내용이 사이드바 높이보다 짧을 때도 스크롤바 트랙이 항상 보이던 것을 숨김.
        // (메뉴가 많아져서 실제로 넘치면 마우스 휠 스크롤 자체는 그대로 동작함)
        accordionMenu.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;
        // DevExpress 기본값(최상위 그룹 사이 여백)이 "마이 메뉴"(접힌 헤더 한 줄)와 "전체
        // 메뉴" 사이를 부자연스럽게 벌려놓았다("간격 없애줘" 지적, 2026-09-21) - 이제 이 둘이
        // 유일한 최상위 그룹이라 간격을 0으로 없애도 다른 곳엔 영향이 없다.
        accordionMenu.DistanceBetweenRootGroups = 0;
        // 기본 슬라이드 애니메이션이 "마이 메뉴"/"전체 메뉴"를 서로 바꿔 보여줄 때(둘 다
        // Elements.Clear() 후 완전히 다시 그리는데, 애니메이션은 옛 상태에서 새 상태로 부드럽게
        // "미끄러지는" 것처럼 보이려다 오히려 배경/테두리가 다시 그려지며 움직이는 것처럼
        // 보였다("클릭할 때마다 부자연스럽게 움직인다" 반복 지적, 2026-09-21 - BeginUpdate/
        // EndUpdate로 묶어도 애니메이션 자체는 그대로라 효과가 없었다) - 아예 꺼서 즉시 전환되게 한다.
        accordionMenu.AnimationType = DevExpress.XtraBars.Navigation.AnimationType.None;

        // 그룹(Style=Group)은 DevExpress 기본 동작 그대로 한 번 클릭하면 펼침/접힘이 되고,
        // 화면(Style=Item)은 AddChildMenus에서 Click을 안 걸어뒀으므로 한 번 클릭으론 아무 일도
        // 안 일어난다 - 실수로 스치듯 클릭했다가 화면이 열리는 걸 막기 위해 더블클릭으로만
        // 열리게 한다. AccordionControlElement 자체엔 DoubleClick 이벤트가 없어서(Click만
        // 있음), 컨트롤 레벨 MouseDoubleClick + CalcHitInfo로 더블클릭 지점의 실제 엘리먼트를
        // 찾아낸다.
        accordionMenu.MouseDoubleClick += AccordionMenu_MouseDoubleClick;

        // 우클릭 - 화면(Item) 위에서는 "즐겨찾기 추가/해제" + "메뉴 새로고침", 그 외에는
        // "메뉴 새로고침"만(ShowSidebarContextMenu 참고). "메뉴 새로고침"은 재로그인(아이디/비번
        // 재입력) 없이 방금 새로 등록한 메뉴를 그 자리에서 바로 보고 싶다는 요청(2026-09-16) -
        // api/auth/menus(로그인과 같은 권한 계산 로직)를 다시 불러와 세션 캐시만 바꿔치기하고
        // 트리를 다시 그린다.
        // 좌클릭 - "마이 메뉴"/"전체 메뉴" 헤더 자신을 클릭했을 때만 상호배타 토글
        // (ToggleMyMenuMode)을 반응시킨다(2026-09-21, 둘 다 이제 대칭인 Group이라 양쪽 다
        // 같은 토글을 켠다). 그 안의 하위 그룹(시스템운영관리 등)은 손대지 않는다 - DevExpress
        // 기본 동작이 이미 그 그룹들의 펼침/접힘을 알아서 처리하므로, 여기서 또 건드리면
        // 두 번 토글되는 꼴이 된다.
        accordionMenu.MouseUp += (s, e) =>
        {
            if (e.Button == MouseButtons.Right) { ShowSidebarContextMenu(e.Location); return; }
            if (e.Button != MouseButtons.Left) return;

            var hit = accordionMenu.CalcHitInfo(e.Location);
            if (hit.HitTest != AccordionControlHitTest.Item) return;
            if (hit.ItemInfo?.Element == _favoritesGroupElement || hit.ItemInfo?.Element == _systemMenuGroupElement)
                ToggleMyMenuMode();
        };

        // 하위 항목엔 세로 가이드라인을, 선택된 화면(leaf)엔 좌측 강조 바를 덧그린다 -
        // Appearance(BackColor/ForeColor)만으로는 표현할 수 없는 장식이라 커스텀 드로잉이 필요.
        accordionMenu.CustomDrawElement += AccordionMenu_CustomDrawElement;

        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = NavDivider };

        // 2026-09-21: 검색창/트리닫기 버튼을 담던 sidebarTopGap+sidebarNavTitleRow를 통째로
        // 없앴다("탐색창 없애고 툴바로 옮기고, 메뉴검색/트리닫기 버튼도 툴바에 포함해달라"는
        // 요청) - 둘 다 이제 headerPanel(툴바) 안에 있다(BuildHeaderSearchZone 참고). 그
        // 결과 사이드바 자신은 이제 트리(accordionMenu) 하나만 담는 훨씬 단순한 구조가 됐고,
        // headerPanel이 사이드바 위까지 전체 폭을 차지하므로 트리 상단이 자동으로 MDI 탭 줄과
        // 같은 Y에서 시작한다(예전엔 이 정렬을 맞추려고 높이를 실측해서 동기화하는 코드가
        // 따로 필요했다 - TabbedMdiManager_CustomDrawTabHeader 참고, 이제 필요 없어졌다).
        // (여기 있던 sidebarBottomMargin(하단 1px 줄) - HeaderBg가 흰색이던 시절엔 안 보이는
        // 경계선이었는데, HeaderBg가 남색으로 바뀌면서(2026-09-22) 상태바 SERVICE 알약 위에
        // 도드라지는 남색 줄로 보여 지적받았다("메뉴트리 하단 라인 삭제해달라") - 제거.)

        // Dock 추가 순서: Fill(accordionMenu) 먼저, Right는 나중에 추가해야 가장자리를
        // 정상적으로 차지한다 (PermissionAssignForm에서 겪은 것과 같은 문제 방지).
        sidebarPanel.Controls.Add(accordionMenu);
        sidebarPanel.Controls.Add(divider);
    }

    /// <summary>사이드바 우클릭 메뉴의 "전체 메뉴 모두 펼치기/접기" 핸들러 - "전체 메뉴" 그룹
    /// 안의 모듈/서브모듈들을 재귀적으로 펼치거나 접는다(2026-09-21, 예전엔 accordionHeaderRow의
    /// "Expand" 체크박스였는데, 그 장식 바 자체를 없애고 "전체 메뉴"를 진짜 아코디언 그룹으로
    /// 바꾸면서 우클릭 메뉴로 옮겼다). "마이 메뉴" 그룹은 건너뛴다 - 그건 이 항목이 아니라
    /// ToggleMyMenuMode(자기 헤더 클릭)로만 열고 닫는 별도 상태라서, 여기서 같이 건드리면
    /// "전체 메뉴만 펼치려던" 사용자 의도와 달리 마이메뉴까지 열려버린다.</summary>
    private void SetAccordionMenuExpanded(bool expanded)
    {
        void Recurse(AccordionControlElement element)
        {
            if (element == _favoritesGroupElement) return;
            if (element.Style == ElementStyle.Group) element.Expanded = expanded;
            foreach (AccordionControlElement child in element.Elements) Recurse(child);
        }

        foreach (AccordionControlElement element in accordionMenu.Elements) Recurse(element);
    }

    /// <summary>메뉴검색 콤보 + 사이드바 접기/펼치기 토글 버튼 - 원래 사이드바 맨 위에
    /// 있었는데 "탐색창 없애고 툴바로 옮겨달라"는 요청(2026-09-21)으로 헤더로 옮겨오는 중
    /// 이 둘을 실제로 담던 패널이 어디에도 Add되지 않은 채 방치돼 화면에서 통째로 사라졌었다
    /// ("검색창/토글 버튼이 사라졌다" 리포트, 2026-09-22). 다시 만들면서 "조회 버튼 왼쪽에
    /// 배치해달라"는 요청 그대로 BuildToolbar의 x축 레이아웃 흐름(AddToolbarButton과 같은
    /// 절대좌표 방식)에 자연스럽게 끼워 넣는다. 필드/색상 튜닝(둥근 필드 반경 5, 배경 톤 등)은
    /// 예전 사이드바 버전에서 이미 여러 차례 다듬어진 값을 그대로 재사용했다.
    /// container.Height 기준으로 세로 중앙 정렬하므로 헤더 높이가 바뀌어도 따로 손댈 게 없다.
    /// 반환값은 이 zone이 차지한 다음 x좌표 - BuildToolbar가 이어서 조회 버튼부터 배치한다.</summary>
    private int BuildHeaderSearchZone(Control container, int x)
    {
        BuildMenuSearchCombo();

        const int searchWidth = 180;
        const int searchHeight = 26;
        var pillLocation = new Point(x, (container.Height - searchHeight) / 2);
        var pillSize = new Size(searchWidth, searchHeight);

        _cboMenuSearchPill = new Panel { Location = pillLocation, Size = pillSize, BackColor = HeaderBg };
        _cboMenuSearchPill.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = RoundedRect(_cboMenuSearchPill.ClientRectangle, 5);
            // 헤더가 남색(HeaderBg=NavDarkBg)이라 Adjust(어둡게)로는 안 보인다 - 상태바
            // SERVICE 알약(ConfigureTitleBarEnvPill)과 같은 방식으로 흰색을 섞어 밝힌다.
            using (var brush = new SolidBrush(ColorHelper.Mix(HeaderBg, Color.White, 0.12f)))
                e.Graphics.FillPath(brush, path);
            using var pen = new Pen(ColorHelper.Mix(HeaderBg, Color.White, 0.3f));
            e.Graphics.DrawPath(pen, path);
        };

        // cboMenuSearch를 알약과 똑같은 크기로 겹쳐두면 에디터 자신이 그리는 사각 배경이
        // 알약 가장자리의 둥근 테두리 선을 덮어버린다 - 사방 2px씩 작게 둬서 테두리가 그
        // 바깥으로 드러나 보이게 한다.
        cboMenuSearch.Dock = DockStyle.None;
        cboMenuSearch.Location = new Point(pillLocation.X + 2, pillLocation.Y + 2);
        cboMenuSearch.Size = new Size(pillSize.Width - 4, pillSize.Height - 4);

        container.Controls.Add(_cboMenuSearchPill);
        container.Controls.Add(cboMenuSearch);
        cboMenuSearch.BringToFront(); // 실제 입력/클릭은 항상 이 컨트롤이 받아야 하므로 배경 패널보다 앞에 둔다.
        x += searchWidth + 8;

        sidebarToggleButton.BadgeColor = NavAccentBlue;
        sidebarToggleButton.CornerRadius = 12;
        RefreshSidebarToggleIcon();
        toolbarToolTip.SetToolTip(sidebarToggleButton, "메뉴트리 숨기기/펼치기");
        sidebarToggleButton.Click += (s, e) => ToggleSidebarCollapsed();
        sidebarToggleButton.Location = new Point(x, (container.Height - sidebarToggleButton.Height) / 2);
        container.Controls.Add(sidebarToggleButton);
        x += sidebarToggleButton.Width;

        x += GroupGap;
        AddDivider(ref x);
        x += GroupGap - 12;
        return x;
    }

    /// <summary>
    /// 헤더 맨 오른쪽 자리 - 원래는 서비스 선택 콤보(cboEnvironment, "SERVICE" 라벨)가 있었는데,
    /// 같은 기능이 제목줄 알약(_titleBarEnvPill)으로 다시 올라가면서 중복이 됐다 - "위쪽에
    /// 다시 만들었으니까 필요없어. 삭제해줘"(2026-09-17) 요청으로 화면에서는 뺐다. 다만
    /// cboEnvironment 컨트롤 자체(BuildEnvironmentCombo)는 계속 셋업해야 한다 - 제목줄 알약의
    /// 클릭 메뉴(_envSwitchMenu)가 실제 서버 전환 로직(OnEnvironmentComboChanged)을 이 콤보의
    /// SelectedIndexChanged에 위임하기 때문이다. 부모 없이도 SelectedIndexChanged는 정상
    /// 동작한다.
    ///
    /// 그 자리엔 대신 로그아웃 버튼을 옮겨왔다("로그아웃버튼을 프레임 오른쪽으로 이동시켜줘",
    /// 2026-09-17) - 원래 헤더 왼쪽 아이콘 줄 맨 끝에 있던 걸 여기로 옮겼다(BuildToolbar에서
    /// 뺐다).
    ///
    /// 테마선택 콤보(cboSkin)는 기능은 계속 살려두되(제품 기본 스킨 기준으로 화면별 색을
    /// 맞춰가는 중이라 아직 노출은 안 함 - cboSkin.Enabled=false) 화면엔 안 올린다.
    /// </summary>
    private void BuildHeaderRightCombos()
    {
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

        var logoutChip = NewChipButton("로그아웃", SvgIcons.ToolbarLogout, IconChipVariant.Danger, 0);
        logoutChip.Click += (s, e) => OnLogoutClick();

        // sidebarToggleButton과 같은 이유로 TableLayoutPanel+Anchor=None을 쓴다 - Dock=Right를
        // 칩에 직접 주면 세로로 헤더 높이만큼 늘어난다.
        var logoutArea = new TableLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = logoutChip.Width + 28,
            ColumnCount = 1,
            RowCount = 1,
            // 명시 안 하면 TableLayoutPanel 기본값(SystemColors.Control, 밝은 회색)이 그대로
            // 남아 헤더가 흰색이던 때는 안 티났는데, 헤더가 남색(HeaderBg)으로 바뀌면서
            // 로그아웃 버튼 뒤에 네모난 회색 얼룩처럼 보이게 됐다 - 헤더와 맞춘다.
            BackColor = HeaderBg
        };
        logoutArea.Controls.Add(logoutChip);
        logoutChip.Anchor = AnchorStyles.None;

        headerPanel.Controls.Add(logoutArea);
    }

    private const int HeaderRightCombosWidth = 230;

    /// <summary>
    /// 사이드바 접힘/펼침 전환 - "메뉴트리 숨기기 하면 트리 자체를 숨기고 열린 폼이 전체
    /// 화면을 채우게" 요청(2026-09-21)에 맞춰, 폭을 줄이는 대신 sidebarPanel을 통째로
    /// Visible=false 한다. 검색창/토글 버튼은 이제 sidebarPanel이 아니라 headerPanel(툴바)
    /// 안에 있어서 사이드바를 숨겨도 그대로 남아있다 - 다시 펼 방법이 항상 눈에 보인다.
    /// </summary>
    private void ToggleSidebarCollapsed()
    {
        _sidebarCollapsed = !_sidebarCollapsed;
        sidebarPanel.Visible = !_sidebarCollapsed;
        RefreshSidebarToggleIcon();
    }

    /// <summary>sidebarToggleButton은 "누르면 일어날 동작"을 보여준다(SvgIcons.MenuHide/MenuView
    /// 설명 참고) - 펼쳐진 상태면 접으라는 아이콘을, 접힌 상태면 펼치라는 아이콘을 보여준다.
    /// BuildHeaderSearchZone(최초 1회)과 ToggleSidebarCollapsed(전환할 때마다) 둘 다에서 부른다.</summary>
    private void RefreshSidebarToggleIcon()
    {
        // 접힌 상태면 펼치라는 뜻으로 오른쪽 화살표를, 펼쳐진 상태면 접으라는 뜻으로 왼쪽
        // 화살표를 보여준다(이전 MenuHide/MenuView와 같은 관례) - 2026-09-17 목업 요청으로
        // 사각 패널 아이콘 대신 배지 안 순수 화살표(ChevronLeft/Right)로 교체.
        var icon = _sidebarCollapsed ? SvgIcons.ChevronRight : SvgIcons.ChevronLeft;
        sidebarToggleButton.IconImage = SvgIcons.Load(icon, sidebarToggleButton.Width - sidebarToggleButton.IconInset * 2,
            Color.White);
    }

    /// <summary>탭 줄 오른쪽 끝 아이콘 스트립("탭 목록"/"탭 전체 닫기") 구성 - 클릭 동작은
    /// 원래 툴바 버튼과 완전히 동일(ShowTabListPopup/CloseAllMdiChildren)하게 그대로 재사용,
    /// 자리만 옮겼다. tabStripButtons 필드 설명 참고 - MDI 영역은 사이드바 폭과 무관하게 항상
    /// 창 오른쪽 끝까지 채우므로, X좌표는 ClientSize.Width 기준으로 계산하면 된다.</summary>
    private void BuildTabStripButtons()
    {
        // 배지(둥근 칩) 배경을 안 그리게 투명으로 둔다 - tabStripButtons 패널 배경(TabInactiveBg)
        // 위에 IconBadgeButton 기본 배지색(거의 흰색이지만 미세하게 다름)이 얹히면 아이콘마다
        // 옅은 사각 얼룩처럼 도드라져 보였다(실제로 지적받음, 2026-09-03) - 투명으로 두면 아이콘
        // 글리프만 보이고 호버/눌림 오버레이는 그대로 동작한다(IconBadgeButton.OnPaint 참고).
        btnHomeStrip.BadgeColor = Color.Transparent;
        btnTabListStrip.BadgeColor = Color.Transparent;
        btnCloseAllStrip.BadgeColor = Color.Transparent;

        btnHomeStrip.IconImage = SvgIcons.Load(SvgIcons.Home,
            btnHomeStrip.Width - btnHomeStrip.IconInset * 2, ToolbarIconColor);
        toolbarToolTip.SetToolTip(btnHomeStrip, "홈");
        btnHomeStrip.Click += (s, e) => OpenHomeForm();

        btnTabListStrip.IconImage = SvgIcons.Load(SvgIcons.ToolbarTabList,
            btnTabListStrip.Width - btnTabListStrip.IconInset * 2, ToolbarIconColor);
        toolbarToolTip.SetToolTip(btnTabListStrip, "탭 목록");
        btnTabListStrip.Click += (s, e) => ShowTabListPopup(btnTabListStrip);

        btnCloseAllStrip.IconImage = SvgIcons.Load(SvgIcons.ToolbarCloseAll,
            btnCloseAllStrip.Width - btnCloseAllStrip.IconInset * 2, ToolbarIconColor);
        toolbarToolTip.SetToolTip(btnCloseAllStrip, "탭 전체 닫기");
        btnCloseAllStrip.Click += (s, e) => CloseAllMdiChildren(except: null);

        tabStripButtons.Controls.Add(btnHomeStrip);
        tabStripButtons.Controls.Add(btnTabListStrip);
        tabStripButtons.Controls.Add(btnCloseAllStrip);
        tabStripButtons.Width = TabStripButtonsWidth;
    }

    // [홈][탭 목록][탭 전체 닫기][X] 네 아이콘 사이 간격을 전부 이 값 하나로 통일한다 - 예전엔
    // 여백/간격/X 앞 예약폭이 6·4·30으로 제각각이라 X 앞만 유독 벌어져 보였다(실제로 지적받음,
    // 2026-09-03). tabStripButtons 자체 폭에 "여백-버튼-간격-버튼-간격-버튼-여백"을 다 담고,
    // X는 그 마지막 여백 바로 뒤에서 시작하게 해서(NativeCloseButtonOwnWidth만 추가 예약,
    // 간격은 안 더함) 모든 간격이 전부 TabStripIconGap 하나로 맞춰지게 한다. 홈 버튼은
    // 2026-09-17 요청으로 추가(탭목록/탭전체닫기 기능은 그대로 두고 자리만 하나 늘림).
    private const int TabStripIconGap = 10;
    private const int TabStripButtonsWidth =
        TabStripIconGap + 28 + TabStripIconGap + 28 + TabStripIconGap + 28 + TabStripIconGap;

    /// <summary>DevExpress가 ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader로 탭
    /// 컨트롤 헤더 맨 끝에 그리는 "현재 탭 닫기" X 버튼 자체의 폭 추정치(간격 제외) - DevExpress가
    /// 그 버튼의 실제 폭을 이벤트로 알려주지 않아 정확히 실측할 방법이 없다. 스킨/폰트가 바뀌어
    /// 어긋나면 이 값만 조정하면 된다.</summary>
    private const int NativeCloseButtonOwnWidth = 20;

    /// <summary>tabStripButtons 안의 아이콘들을 창 오른쪽 끝에서 얼마나 더 떼어놓을지 - 그냥
    /// 붙이면 창 모서리(리사이즈 코너)에 바짝 붙어 답답해 보인다는 지적으로
    /// (2026-09-03) 여유를 뒀다. X 버튼은 DevExpress가 탭 컨트롤 헤더 자체 로직으로 그려서
    /// 우리가 직접 위치를 못 옮기지만, tabStripButtons가 그만큼 더 왼쪽에서 시작하니 결과적으로
    /// 아이콘들이 뭉쳐서 왼쪽으로 이동한 것처럼 보인다.</summary>
    private const int TabStripGroupRightMargin = 14;

    /// <summary>tabStripButtons를 창 오른쪽 끝(사이드바 폭과 무관, MDI 영역이 항상 거기까지
    /// 채움)·탭 줄 실측 높이에 맞춰 다시 배치한다 - Form Resize와 탭 줄 높이가 바뀔 때
    /// (TabbedMdiManager_CustomDrawTabHeader) 둘 다에서 불린다. DevExpress 자체 "현재 탭 닫기"
    /// X 버튼(NativeCloseButtonOwnWidth)이 이 스트립 오른쪽에 그려지도록 그만큼 왼쪽으로 띄워서,
    /// [홈][탭 목록][탭 전체 닫기][X] 네 아이콘이 겹치지 않고 동일한 간격(TabStripIconGap)으로
    /// 나란히 보이게 한다 - tabStripButtons 자신의 마지막 여백이 이미 X 앞 간격을 담당하므로,
    /// 여기서는 X 버튼 자체의 폭만 추가로 예약한다(간격을 두 번 더하지 않음). TabStripGroupRightMargin은
    /// 그 전체를 창 모서리에서 한 번 더 떼어놓는 여유값이다.</summary>
    private void RepositionTabStripButtons()
    {
        // ClientSize.Width는 Form.Padding을 반영 안 한 "전체" 폭이라, 복원(Normal) 상태의
        // 창 테두리(Padding, RefreshWindowBorder 참고)가 켜져 있으면 이 스트립이 그만큼 밖으로
        // 밀려나 테두리색이 탭 줄 쪽에 비쳐 보였다("메뉴 열었을 때 메뉴탭 쪽에 색깔이 보인다"
        // 지적, 2026-09-21) - Padding.Right만큼 안쪽으로 당겨서 맞춘다.
        tabStripButtons.Location = new Point(
            ClientSize.Width - tabStripButtons.Width - NativeCloseButtonOwnWidth - TabStripGroupRightMargin - Padding.Right,
            headerPanel.Bottom);

        var y = Math.Max(0, (tabStripButtons.Height - btnHomeStrip.Height) / 2);
        btnHomeStrip.Location = new Point(TabStripIconGap, y);
        btnTabListStrip.Location = new Point(btnHomeStrip.Right + TabStripIconGap, y);
        btnCloseAllStrip.Location = new Point(btnTabListStrip.Right + TabStripIconGap, y);
    }

    /// <summary>
    /// 조회/입력/삭제/행추가/행삭제/저장/출력을 카드로 묶지 않고 각각 독립된 배지 버튼으로
    /// 헤더에 나란히 배치한다. 홈은 화면 전환용이라 별도로 사이드바 메뉴트리 위
    /// (ConfigureSidebarTopGap)로 옮겼다.
    ///
    /// 남은 7개는 성격이 다른 지점마다 여백(GroupGap) + 구분선으로 세 묶음이 뚜렷하게 읽히게
    /// 한다 - [조회 입력 삭제](레코드 단위) / [행추가 행삭제](그리드 행 단위) / [저장 출력](마무리).
    /// 강조(IconChipVariant)는 조회(Accent)/삭제·로그아웃(Danger) 세 곳에만 준다(2026-09-22
    /// 엔터프라이즈 툴바 가이드 반영) - 나머지는 Default로, 평소엔 배경 없이 호버할 때만 옅게 채워진다.
    /// 클릭하면 현재 활성화된 MDI 자식폼(ActiveMdiChild)의 표준 액션(BaseForm.QueryClick 등)을 호출한다.
    /// </summary>
    private void BuildToolbar()
    {
        // 홈 버튼은 여기 있었는데, 탭 줄 오른쪽 아이콘 스트립(btnHomeStrip)에도 생기면서
        // 중복이라 뺐다(BuildTabStripButtons 참고, 2026-09-17 요청 - "홈버튼은 중복되니까
        // 제거"). 묶음도 재정리했다(같은 날 요청) - [조회 입력] / [행추가 행삭제] /
        // [삭제 저장] / [출력], 총 네 묶음.
        var x = 16;

        // 메뉴검색 + 사이드바 접기/펼치기 토글을 조회 버튼 왼쪽에 배치(2026-09-22 요청).
        x = BuildHeaderSearchZone(headerPanel, x);

        btnQuery = AddToolbarButton(headerPanel, ref x, "조회", SvgIcons.ToolbarSearch, f => f.RunQueryAsync(),
            IconChipVariant.Accent);
        btnNew = AddToolbarButton(headerPanel, ref x, "입력", SvgIcons.ToolbarNew, f => f.NewClick());
        x += GroupGap;
        AddDivider(ref x);
        x += GroupGap - 12;
        btnRowAdd = AddToolbarButton(headerPanel, ref x, "행추가", SvgIcons.ToolbarRowAdd, f => f.NewRowClick());
        btnRowDelete = AddToolbarButton(headerPanel, ref x, "행삭제", SvgIcons.ToolbarRowDelete, f => f.DeleteRowClick());
        x += GroupGap;
        AddDivider(ref x);
        x += GroupGap - 12;
        btnDelete = AddToolbarButton(headerPanel, ref x, "삭제", SvgIcons.ToolbarDelete, f => f.DeleteClick(),
            IconChipVariant.Danger);
        // Primary(꽉 찬 파란 배경) -> Default: "저장도 배경색을 다른 아이콘들과 통일해달라"
        // 요청(2026-09-21) - 나머지 버튼과 같은 평소엔 배경 없는 스타일로 되돌렸다.
        btnSave = AddToolbarButton(headerPanel, ref x, "저장", SvgIcons.ToolbarSave, f => f.RunSaveAsync());
        x += GroupGap;
        AddDivider(ref x);
        x += GroupGap - 12;
        btnPrint = AddToolbarButton(headerPanel, ref x, "출력", SvgIcons.ToolbarPrint, f => f.PrintClick());

        // 사용자별 단축키(TSMSHORTCUTDEFAULT/TSMUSERSHORTCUT)가 ACTION_CD로 가리키는 액션 -
        // ProcessCmdKey가 눌린 키를 SessionManager.Current.Shortcuts에서 찾아 ACTION_CD를 얻으면
        // 여기서 같은 버튼/델리게이트를 그대로 재사용한다(클릭한 것과 완전히 동일하게 동작).
        _toolbarActions["QUERY"] = (btnQuery, "조회", f => f.RunQueryAsync());
        _toolbarActions["NEW"] = (btnNew, "입력", f => f.NewClick());
        _toolbarActions["DELETE"] = (btnDelete, "삭제", f => f.DeleteClick());
        _toolbarActions["ROWADD"] = (btnRowAdd, "행추가", f => f.NewRowClick());
        _toolbarActions["ROWDELETE"] = (btnRowDelete, "행삭제", f => f.DeleteRowClick());
        _toolbarActions["SAVE"] = (btnSave, "저장", f => f.RunSaveAsync());
        _toolbarActions["PRINT"] = (btnPrint, "출력", f => f.PrintClick());
    }

    /// <summary>두 Add*ToolbarButton이 공유하는 생성 로직. "모든 툴바 아이콘의 배경색을 조회
    /// 버튼과 동일하게" 요청(2026-09-22)에 맞춰, 실제로 그려지는 Variant는 항상 Accent로
    /// 고정한다 - Accent의 배경/보더(옅은 블루 틴트, 항상 켜짐)가 모든 버튼에 그대로 공유되고,
    /// 대신 AccentTextColor만 버튼마다 덮어써서 원래 의미(조회=블루, 삭제/로그아웃=빨강,
    /// 나머지=슬레이트)를 글자/아이콘 색으로 유지한다. 호출부(AddToolbarButton)가 넘기는
    /// variant 파라미터는 이제 배경이 아니라 이 텍스트/아이콘 색만 고른다.</summary>
    private IconChipButton NewChipButton(string text, string svgIconPath, IconChipVariant variant, int x, Color? iconColorOverride = null)
    {
        var contentColor = iconColorOverride ?? variant switch
        {
            IconChipVariant.Accent => ToolbarQueryIconColor,
            IconChipVariant.Danger => Color.FromArgb(220, 38, 38),
            // ToolbarIconColor(슬레이트, 밝은 배경용)가 아니라 별도 흰색 상수를 쓴다 - 이 버튼들은
            // 이제 배경 없이 어두운 헤더가 그대로 비치는 위에 얹히므로(IconChipButton 배경 제거,
            // 2026-09-22) 슬레이트 톤은 거의 안 보였다(2026-09-23 지적).
            _ => ToolbarDefaultIconColor
        };
        var btn = new IconChipButton
        {
            Text = text,
            Variant = IconChipVariant.Accent,
            AccentTextColor = contentColor,
            IconImage = SvgIcons.Load(svgIconPath, ToolbarIconSize, contentColor),
            Location = new Point(x, ToolbarButtonY)
        };
        toolbarToolTip.SetToolTip(btn, text);
        return btn;
    }

    // 화면(MDI 자식)이 바뀔 때마다 UpdateToolbarPermissions()가 이 참조들의 Enabled를
    // 그 화면의 BaseForm.CanInsert/CanUpdate/CanDelete로 다시 계산해서 켜고 끈다.
    // AddToolbarButton 내부 지역변수였던 것을 필드로 승격 - 나중에 다시 손댈 수 있어야 해서.
    private IconChipButton btnQuery = null!;
    private IconChipButton btnNew = null!;
    private IconChipButton btnDelete = null!;
    private IconChipButton btnRowAdd = null!;
    private IconChipButton btnRowDelete = null!;
    private IconChipButton btnSave = null!;
    private IconChipButton btnPrint = null!;

    /// <summary>ACTION_CD -> (버튼, 표시용 라벨, 실행 델리게이트). BuildToolbar에서 채워지고
    /// ProcessCmdKey의 단축키 디스패치가 읽는다.</summary>
    private readonly Dictionary<string, (IconChipButton Button, string Label, Func<BaseForm, Task> Action)> _toolbarActions = new();

    // 목업(아이콘+라벨+단축키가 한 줄, 저장만 꽉 찬 파란 버튼) 방향으로 다시 그리면서
    // (2026-09-17) 아이콘도 26 -> 16으로, 버튼도 정사각 배지 대신 내용에 맞춘 가변폭으로
    // 바꿨다(IconChipButton.UpdateSize 참고) - 헤더(60px) 안에서 세로로 가운데 오도록 Y좌표만 고정.
    private const int ToolbarIconSize = 16;
    private const int ToolbarButtonY = 11; // (headerPanel.Height(52) - IconChipButton.Height(30)) / 2
    // ToolbarIconColor(슬레이트 #334155) - tabStripButtons(홈/탭목록/전체닫기, 밝은 Gainsboro
    // 배경) 전용으로 쓰던 색인데, "모든 툴바 아이콘 배경을 조회 버튼과 동일하게"(2026-09-22)
    // 요청으로 나머지 툴바 버튼들도 이제 옅은 블루 칩(Accent, 밝은 배경) 위에 올라가게 되면서
    // 다시 이 슬레이트 톤이 잘 어울리게 됐다 - NewChipButton도 그대로 재사용한다.
    private static readonly Color ToolbarIconColor = Color.FromArgb(51, 65, 85);
    // 조회/삭제/로그아웃 이외(입력/행추가/행삭제/저장/출력)의 기본 툴바 아이콘 색 - 어두운
    // 헤더 위에 배경 없이 얹히므로(2026-09-22) ToolbarIconColor(밝은 배경용 슬레이트)가 아니라
    // 흰색을 쓴다(2026-09-23, IconChipButton.DefaultTextColor와 짝 맞춤).
    private static readonly Color ToolbarDefaultIconColor = Color.White;
    private static readonly Color ToolbarQueryIconColor = Color.FromArgb(29, 78, 216);

    /// <summary>툴바 아이콘 묶음 사이 여백(BuildHeaderToolbar 주석의 3개 묶음 참고).
    /// 구분선을 긋지 않고 여백만으로 나누는 방식이라, 너무 넓으면 툴바가 흩어져 보이고
    /// 너무 좁으면 나눈 티가 안 난다 - 버튼 폭(54)의 1/4 정도가 적당했다.</summary>
    private const int GroupGap = 14;

    /// <summary>같은 묶음 안(예: 조회-입력) 버튼끼리의 간격 - 예전엔 이 간격이 0이라 서로
    /// 딱 붙어 보였다("사이가 너무 붙었어" 지적, 2026-09-17).</summary>
    private const int ButtonGap = 6;

    /// <summary>
    /// action 파라미터는 BaseForm을 받지만, "홈" 버튼처럼 활성화면과 무관하게 항상 동작해야 하는
    /// 경우도 있어서, 실제로는 델리게이트 내부에서 ActiveMdiChild를 쓸지 말지 자유롭게 결정한다.
    /// (홈 버튼은 activeForm 인자를 무시하고 항상 OpenHomeForm()만 호출)
    /// container: 이 버튼을 실제로 담을 컨트롤(headerPanel 직접 또는 AddToolbarGroup으로 만든 카드).
    /// x는 container 기준 로컬 좌표(y는 ToolbarButtonY로 고정). 반환값은 UpdateToolbarPermissions()에서
    /// Enabled를 다시 계산할 수 있도록 호출측(BuildToolbar)이 필드에 보관해두기 위함.
    /// </summary>
    private IconChipButton AddToolbarButton(Control container, ref int x, string text, string svgIconPath,
        Func<BaseForm, Task> action, IconChipVariant variant = IconChipVariant.Default, Color? iconColorOverride = null)
    {
        var btn = NewChipButton(text, svgIconPath, variant, x, iconColorOverride);
        btn.Click += async (s, e) => await InvokeToolbarActionAsync(text, action);
        container.Controls.Add(btn);
        x += btn.Width + ButtonGap;
        return btn;
    }

    /// <summary>버튼 클릭과 단축키(ProcessCmdKey) 둘 다 여기로 모아서 같은 방식(활성화면 확인/
    /// 진행중 표시/오류 메시지)으로 BaseForm 표준 액션을 실행한다 - 예전엔 이 로직이
    /// AddToolbarButton의 Click 람다 안에만 있어서 단축키는 재사용할 수 없었다.</summary>
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
    /// Enabled를 그 화면의 권한(BaseForm.CanInsert/CanUpdate/CanDelete/CanPrint)에 맞춰
    /// 다시 계산한다. 조회는 별도 권한 플래그가 없다 - ViewYn은 이미 "이 메뉴를 열 수 있는지"
    /// 자체를 가리키므로, 화면이 열려 있다는 것 자체가 조회 권한이 있다는 뜻이라 항상 켜둔다.
    /// 출력은 2026-08-31에 TSMMENUAUTH.PRINT_YN이 새로 생기면서 CanPrint로 실제 권한을 본다
    /// (그 전엔 필드 자체가 없어서 항상 켜뒀었음). 저장은 신규/수정 두 흐름을 다 섬기므로
    /// CanInsert 또는 CanUpdate 둘 중 하나만 있어도 켠다.
    /// 활성 업무화면이 없는 경우(홈 화면이거나 열린 화면이 하나도 없을 때)는 조회만 남기고
    /// 나머지 6개는 전부 끈다 - 대상 데이터가 없는 상태에서 입력/삭제/저장/출력을 누르게 둘 이유가 없다.
    /// </summary>
    private void UpdateToolbarPermissions()
    {
        var activeForm = ActiveMdiChild as BaseForm;
        var hasTarget = activeForm != null;

        btnQuery.Enabled = true;

        var canInsert = hasTarget && activeForm!.CanInsert;
        var canUpdate = hasTarget && activeForm!.CanUpdate;
        var canDelete = hasTarget && activeForm!.CanDelete;
        var canPrint = hasTarget && activeForm!.CanPrint;

        btnNew.Enabled = canInsert;
        btnRowAdd.Enabled = canInsert;
        btnDelete.Enabled = canDelete;
        btnRowDelete.Enabled = canDelete;
        btnSave.Enabled = canInsert || canUpdate;
        btnPrint.Enabled = canPrint;
    }

    // 버튼(30px)보다 짧은 16px 구분선 - "그룹 사이엔 높이 16px 연한 회색 구분선" 요청(2026-09-22).
    // Slate 300(#CBD5E1) - IconChipButton.BorderColor(평소 테두리)와 같은 톤이라 버튼 테두리의
    // 연장선처럼 자연스럽게 이어져 보인다.
    private static readonly Color ToolbarGroupDividerColor = Color.FromArgb(203, 213, 225);

    private void AddDivider(ref int x)
    {
        var dividerY = ToolbarButtonY + (30 - 16) / 2;
        var divider = new Panel { Location = new Point(x, dividerY), Size = new Size(1, 16), BackColor = ToolbarGroupDividerColor };
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

        RefreshEnvironmentStatusDot();
        AppConfig.EnvironmentChanged += RefreshEnvironmentStatusDot;
    }

    /// <summary>서버 선택 콤보 왼쪽 아이콘 - 원래는 개발/운영 색점(주황/초록)이었는데, MENU
    /// 행(cboMenuSearch)의 돋보기 아이콘과 모양이 안 맞는다는 지적(2026-09-06)으로 같은
    /// ButtonPredefines.Search로 통일했다. 이 콤보엔 검색 기능이 없으므로(재로그인만 트리거)
    /// ButtonClick을 안 붙여 눌러도 아무 동작이 없다 - cboMenuSearch의 돋보기도 클릭 자체엔
    /// 아무 기능이 없고 타이핑으로만 필터링되는 것과 동일한 상태(장식용). 메서드명은 이제
    /// "점"이 아니지만 호출부(AppConfig.EnvironmentChanged 등)와의 연결을 그대로 유지하기
    /// 위해 이름은 남겨둔다.</summary>
    private void RefreshEnvironmentStatusDot()
    {
        cboEnvironment.Properties.Buttons.Clear();
        cboEnvironment.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Search) { IsLeft = true });
        // Buttons.Clear()가 ComboBoxEdit 기본 드롭다운 화살표까지 같이 지워버려서(실제로 겪음 -
        // 왼쪽 아이콘만 있고 오른쪽 화살표가 없어짐), cboMenuSearch처럼 오른쪽에 선택 삼각형이
        // 보이도록 다시 추가한다(2026-09-06 요청).
        cboEnvironment.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
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
        cboMenuSearch.Properties.NullText = "메뉴 검색";
        cboMenuSearch.Properties.ShowHeader = false;
        cboMenuSearch.Properties.ShowFooter = false;

        // 검색줄이 다시 흰 배경(HeaderBg)으로 분리되면서(2026-09-17 요청) 밝은 배경용 색으로
        // 되돌렸다 - UseXxxColor를 같이 켜야 스킨이 이 값을 실제로 반영한다. 배경은 에디터
        // 2026-09-21: "콤보 배경은 흰색, 바깥(_cboMenuSearchPill) 배경은 그레이로" 요청으로
        // 다시 나눴다 - 에디터 자체는 흰색 입력창처럼, 2px 바깥 여백(안쪽으로 인셋된 부분,
        // ConfigureSidebarTopGap 참고)만 회색 테두리처럼 보이게 한다. 에디터 자체 테두리는
        // NoBorder로 꺼서 그 회색 링과 안 겹치게 한다.
        cboMenuSearch.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        cboMenuSearch.Properties.Appearance.BackColor = Color.White;
        cboMenuSearch.Properties.Appearance.ForeColor = HeaderText;
        cboMenuSearch.Properties.Appearance.Options.UseBackColor = true;
        cboMenuSearch.Properties.Appearance.Options.UseForeColor = true;
        cboMenuSearch.Properties.DisplayMember = nameof(MenuDto.MenuNm);
        cboMenuSearch.Properties.ValueMember = nameof(MenuDto.MenuId);

        // PopulateColumns()를 쓰면 MenuDto의 모든 프로퍼티가 열로 깔려서(MenuId/ScreenClassNm 등)
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

        // 왼쪽 돋보기 아이콘은 뺐다 - 제목줄 SERVICE 알약과 같은 디자인(왼쪽 아이콘 없이
        // 오른쪽 세모만)으로 맞춰달라는 요청(2026-09-17). LookUpEdit 자체의 드롭다운 버튼
        // (오른쪽 세모)은 Buttons 컬렉션과 별개로 항상 그려지므로 그것만 남는다.

        // EditValueChanged가 아니라 CloseUp을 쓴다 - EditValueChanged는 타이핑 도중 AutoFilter가
        // 후보를 좁혀가면서 EditValue를 스스로 건드릴 때도 fire해서, 실제로 고르지 않았는데도
        // 화면이 열려버렸다(실제로 겪음 - 에디팅 중간에 "화면을 여시겠습니까" 메시지가 뜸).
        // CloseUp은 팝업이 닫힐 때만 fire하고, AcceptValue로 "진짜 골라서 닫힘(Enter/클릭)"과
        // "그냥 닫힘(Esc, 포커스 이탈)"을 구분해준다 - 후자는 무시한다.
        cboMenuSearch.CloseUp += (s, e) =>
        {
            if (!e.AcceptValue) return;
            if (e.Value is not long menuId || menuId <= 0) return;

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

                OpenMenuById(menuId);
            }));
        };

        // 실시간 트리 필터(2026-09-21 요청 - "마이메뉴와 전체메뉴를 동시에 필터링하여 실시간
        // 검색"). RebuildAccordionTree만 다시 부르고 BuildAccordionMenu(전체)는 안 부른다 -
        // 그쪽이 부르는 RefreshMenuSearchItems가 EditValue를 null로 비워서, 타이핑 도중 부르면
        // 방금 친 글자가 지워진다.
        cboMenuSearch.TextChanged += (s, e) =>
        {
            _menuFilterText = cboMenuSearch.Text?.Trim() ?? string.Empty;
            RebuildAccordionTree();
        };
    }

    /// <summary>검색 콤보의 목록을 현재 로그인 사용자가 열 수 있는 화면들로 채운다.</summary>
    private void RefreshMenuSearchItems()
    {
        var hiddenMenuIds = Session.IsDeveloper ? new HashSet<long>() : GetDeveloperOnlyMenuIds(SessionManager.Current.Menus);
        cboMenuSearch.Properties.DataSource = SessionManager.Current.Menus
            .Where(m => m.ViewYn && m.MenuType != "GROUP" && !string.IsNullOrWhiteSpace(m.ScreenClassNm) && !hiddenMenuIds.Contains(m.MenuId))
            .OrderBy(m => m.MenuNm)
            .ToList();
        cboMenuSearch.EditValue = null;
    }

    /// <summary>"Developer Tool" 최상위 그룹(TSMMENU, MENU_NM='Developer Tool')과 그 하위 전체
    /// (AI Builder/Component관리 그룹 + 그 안의 frmAIBuilder/frmSysLookup/frmSysPopup 등 leaf 메뉴)의
    /// MenuId 집합을 구한다 - Session.IsDeveloper가 아닌 계정에게는 사이드바 트리와 화면검색
    /// 콤보 양쪽에서 전부 숨기기 위함(2026-09-06 요청). MenuId는 DB마다 IDENTITY라 값을 고정할
    /// 수 없어서 이름으로 최상위 그룹을 찾은 뒤 UpperMenuId를 따라 하위로 내려가며 모은다.</summary>
    private static HashSet<long> GetDeveloperOnlyMenuIds(List<MenuDto> allMenus)
    {
        var result = new HashSet<long>();
        var root = allMenus.FirstOrDefault(m => m.UpperMenuId == null && m.MenuNm == "Developer Tool");
        if (root == null) return result;

        var queue = new Queue<long>();
        queue.Enqueue(root.MenuId);
        result.Add(root.MenuId);
        while (queue.Count > 0)
        {
            var id = queue.Dequeue();
            foreach (var child in allMenus.Where(m => m.UpperMenuId == id))
            {
                if (result.Add(child.MenuId)) queue.Enqueue(child.MenuId);
            }
        }
        return result;
    }



    private string GetEnvLabel(string env) => _envLabels.TryGetValue(env, out var label) ? label : env;

    private string GetEnvKey(string label) => _envLabels.FirstOrDefault(kv => kv.Value == label).Key ?? label;

    /// <summary>
    /// 서버 전환. 다른 서버는 다른 DB를 바라보므로 현재 열려있는 화면은 전부 닫아야 하지만,
    /// 로그인한 아이디/비번이 새 서버에도 그대로 통하면(SessionManager에 기억해둔 값으로 조용히
    /// 재로그인 시도) 로그인창을 다시 띄우지 않고 바로 전환한다(2026-09-06 요청) - 같은
    /// 아이디/비번을 쓰는 서비스 간 이동을 테스트할 때 매번 다시 타이핑하는 번거로움을 없앤다.
    /// 아이디/비번이 다르거나(그 서비스엔 없는 계정 등) 서버 자체에 연결이 안 되면, 예전처럼
    /// 로그인창을 띄워서 사용자가 직접 그 서비스의 자격증명을 입력하게 한다.
    /// </summary>
    private async void OnEnvironmentComboChanged(object? sender, EventArgs e)
    {
        if (_suppressEnvChange) return;

        var selectedKey = GetEnvKey((string)cboEnvironment.SelectedItem!);
        if (selectedKey == AppConfig.CurrentEnvironment) return;

        var confirm = AppMessageBox.Show(
            $"'{GetEnvLabel(selectedKey)}'로 전환하면 현재 열려있는 화면이 모두 닫힙니다.\n계속하시겠습니까?",
            "서버 전환", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
        {
            _suppressEnvChange = true;
            cboEnvironment.SelectedItem = GetEnvLabel(AppConfig.CurrentEnvironment);
            _suppressEnvChange = false;
            return;
        }

        var userId = SessionManager.Current.CachedUserId;
        var password = SessionManager.Current.CachedPassword;
        if (userId != null && password != null && await TrySwitchWithoutLoginAsync(selectedKey, userId, password))
        {
            return;
        }

        // 조용한 재로그인이 안 됐다(자격증명 없음/실패) - AppConfig는 이미 새 서비스를 가리키고
        // 있을 수 있지만, SwitchEnvironment는 같은 값으로 다시 불러도 무해하므로 그대로 기존
        // 흐름(로그인창)에 맡긴다.
        SignOutAndShowLogin(() => AppConfig.SwitchEnvironment(selectedKey));
    }

    /// <summary>같은 아이디/비번으로 새 서비스에 조용히 재로그인해본다 - 성공하면 화면 전환까지
    /// 끝내고 true, 실패하면(계정이 그 서비스에 없거나 서버 연결 실패 등) 아무것도 바꾸지 않은
    /// 것처럼 false를 돌려줘서 호출부가 평소 로그인창 흐름으로 넘어가게 한다.</summary>
    private async Task<bool> TrySwitchWithoutLoginAsync(string targetEnv, string userId, string password)
    {
        AppConfig.SwitchEnvironment(targetEnv); // 이 뒤의 API 호출이 새 주소로 나가려면 먼저 바꿔야 한다.

        LoginResponse? response;
        try
        {
            response = await ApiClient.PostAsync<LoginRequest, LoginResponse>("api/auth/login",
                new LoginRequest { UserId = userId, Password = password, ClientVersion = Application.ProductVersion });
        }
        catch
        {
            response = null;
        }

        if (response is not { Success: true, RequirePasswordChange: false })
        {
            return false;
        }

        foreach (Form child in MdiChildren.ToArray())
        {
            child.Close();
        }

        SessionManager.Current.SignOut();
        SessionManager.Current.SignIn(response);
        SessionManager.Current.RememberCredentials(userId, password);
        ApiClient.SetAuthToken(response.AccessToken!);
        RefreshTitleBarStatus(); // EnvironmentChanged 이벤트가 이미 지나간 뒤라 여기서 직접 한 번 더

        await ReloadFavoriteMenusAsync(); // 즐겨찾기는 user_id 기준이라 서버(=DB) 전환 시 다시 받아야 한다.
        return true;
    }

    [DllImport("user32.dll")]
    private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    [StructLayout(LayoutKind.Sequential)]
    private struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    /// <summary>OS 전체의 "마지막 입력으로부터 지난 시간"(이 앱 창에 포커스가 있든 없든 감지됨,
    /// Windows 자체 화면잠금과 같은 방식) - GetLastInputInfo가 돌려주는 tick과 지금 tick의 차를
    /// uint 연산으로 뺀다(둘 다 Environment.TickCount 기준 uint 캐스팅 - int 오버플로/약 49.7일
    /// 주기 롤오버를 안전하게 넘긴다, GetLastInputInfo의 표준 사용법).</summary>
    private static TimeSpan GetSystemIdleTime()
    {
        var info = new LASTINPUTINFO { cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)) };
        if (!GetLastInputInfo(ref info)) return TimeSpan.Zero;
        var idleMs = unchecked((uint)Environment.TickCount - info.dwTime);
        return TimeSpan.FromMilliseconds(idleMs);
    }

    /// <summary>자리비움 잠금화면(2026-09-09 요청 - "시스템을 켜놓고 자리를 비우는경우... 일정
    /// 시간이 지나면 잠금화면이 뜨도록"). 20초 타이머(_idleCheckTimer)에서 호출된다. 로그인
    /// 전이거나(SessionManager.IsSignedIn) 설정값이 없으면(0/null) 아무것도 하지 않는다.
    /// LockScreenForm이 "다른 사용자로 로그인"으로 닫히면(DialogResult.Abort) 그 폼 자신은
    /// SignOutAndShowLogin을 모른 채 그냥 닫히기만 했으므로, 여기서(모달이 완전히 닫힌 뒤) 이어서
    /// 처리한다 - 모달 중첩을 피하기 위함(LockScreenForm 클래스 설명 참고).</summary>
    private void CheckIdleLock()
    {
        if (_lockScreenShowing) return;
        var minutes = AppConfig.IdleTimeoutMinutes;
        if (minutes is not > 0) return;
        if (!SessionManager.Current.IsSignedIn) return;
        if (GetSystemIdleTime().TotalMinutes < minutes.Value) return;

        _lockScreenShowing = true;
        try
        {
            using var lockScreen = new LockScreenForm();
            var result = lockScreen.ShowDialog(this);
            if (result == DialogResult.Abort) SignOutAndShowLogin();
        }
        finally
        {
            _lockScreenShowing = false;
        }
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
            // Program.cs의 최초 로그인 직후와 같은 자리(2026-09-09 실제 발견 - AppConfig.
            // IdleTimeoutMinutes를 채우는 SiteThemeSync가 여기서 안 불려서, 앱을 완전히
            // 재시작하지 않고 로그아웃/재로그인만 하면 자리비움 잠금시간 같은 사이트설정 값이
            // 새로 반영이 안 됐다 - 테마색/에셋도 같은 문제라 둘 다 여기서 다시 부른다).
            AssetSyncer.SyncFromServer();
            SiteThemeSync.ApplyFromServer();

            RefreshTitleBarStatus(); // 로그아웃 시 비워둔 이름/접속시각을 새 세션 값으로 다시 채운다.
            BuildAccordionMenu();
            _ = ReloadFavoriteMenusAsync(); // 재로그인한 사용자가 다를 수 있으므로 즐겨찾기도 다시 받는다.
            Show();
            loginForm.Splash?.Close();
        }
        else
        {
            Application.Exit();
        }
    }

    /// <summary>
    /// TSMMENU.ICON_NM 값 -> DevExpress SVG 아이콘 이름. 여기 없는 값이 오면 폴더 아이콘으로
    /// 떨어진다(SvgIcons.Folder) - 메뉴를 새로 만들 때 아이콘 이름을 안 정해도 메뉴는 정상으로 뜬다.
    /// 목록 자체는 WYNLAB.Shared.MenuIconCatalog에 있다(메뉴등록 화면의 아이콘 선택 룩업과 같은
    /// 목록을 공유해서 둘이 어긋나지 않게 하려고, 2026-09-16) - 여기서는 그 Key -> SvgResourceName만
    /// 다시 꺼내 쓴다.
    /// </summary>
    private static readonly Dictionary<string, string> TopMenuIcons = WYNLAB.Shared.MenuIconCatalog.Entries
        .ToDictionary(e => e.Key, e => e.SvgResourceName, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// SessionManager에 캐싱된 메뉴권한(MenuDto) 목록으로 Accordion 트리를 재귀 구성.
    /// MENU_TYPE = GROUP 이면 상위그룹, FORM이면 클릭 시 화면 오픈.
    /// </summary>
    private void BuildAccordionMenu()
    {
        RebuildAccordionTree();

        // 헤더의 화면검색 콤보도 같은 메뉴 목록을 쓰므로 여기서 같이 채운다 - 서버를 바꿔
        // 재로그인하면 이 메서드가 다시 도니까 콤보 목록도 자동으로 새 메뉴로 갈린다.
        // RebuildAccordionTree 쪽에는 안 넣는다 - 그쪽은 검색창에 타이핑할 때마다(매 키 입력)도
        // 다시 불리는데, cboMenuSearch.EditValue를 여기서 null로 비우기 때문에 타이핑 도중
        // 부르면 방금 친 글자가 지워진다(RefreshMenuSearchItems 설명 참고).
        RefreshMenuSearchItems();
    }

    /// <summary>
    /// SessionManager에 캐싱된 메뉴권한(MenuDto) 목록으로 Accordion 트리를 재귀 구성.
    /// MENU_TYPE = GROUP 이면 상위그룹, FORM이면 클릭 시 화면 오픈. _menuFilterText가 비어있지
    /// 않으면 자기 이름 또는 하위 어딘가가 일치하는 항목만 남기고, 일치하는 하위가 있는 그룹은
    /// 강제로 펼쳐서 보이게 한다(검색 결과가 접힌 채로 숨어있지 않도록) - cboMenuSearch의
    /// TextChanged가 키 입력마다 이 메서드만 다시 부른다(BuildAccordionMenu는 안 부름, 위 주석 참고).
    /// </summary>
    /// <summary>"전체 메뉴" 자신 - "마이 메뉴"와 대칭인 진짜 Group 엘리먼트(2026-09-21, 예전
    /// 장식 바 accordionHeaderRow를 대체). 우클릭한 항목이 이 헤더 자신인지(그래서
    /// ToggleMyMenuMode를 반응시킬지) 판단하는 데 쓴다.</summary>
    private AccordionControlElement? _systemMenuGroupElement;

    private void RebuildAccordionTree()
    {
        // BeginUpdate/EndUpdate로 Clear+재구성 전체를 한 번의 레이아웃/리페인트로 묶는다 -
        // 안 묶으면 Elements.Add()를 여러 번 호출하는 동안 컨트롤이 매번 다시 레이아웃을 계산해
        // 그리면서, 클릭할 때마다 배경/테두리가 다시 그려지며 살짝 움직이는 것처럼 보였다
        // ("마이메뉴/전체메뉴 클릭할 때마다 부자연스럽게 움직인다" 지적, 2026-09-21). Clear()도
        // 호출부마다 따로 부르던 걸 여기 한 곳으로 모았다(모든 호출부가 항상 Clear 직후 이
        // 메서드를 불렀으므로 중복이었다).
        accordionMenu.BeginUpdate();
        try
        {
        accordionMenu.Elements.Clear();
        // "Developer Tool"(AI Builder/Component관리) 최상위 그룹은 일반 사용자에게는 굳이 보일
        // 필요가 없다 - TSMUSER.DEVELOPER_YN='Y'인 계정에서만 보이게 한다(2026-09-06 요청).
        // 화면검색 콤보(RefreshMenuSearchItems)도 같은 기준으로 걸러야 사이드바에서만 숨고
        // 검색으로는 그대로 찾아지는 반쪽짜리 숨김이 안 된다 - GetDeveloperOnlyMenuIds 참고.
        var hiddenMenuIds = Session.IsDeveloper ? new HashSet<long>() : GetDeveloperOnlyMenuIds(SessionManager.Current.Menus);
        var menus = SessionManager.Current.Menus.Where(m => m.ViewYn && !hiddenMenuIds.Contains(m.MenuId)).ToList();
        var topMenus = menus.Where(m => m.UpperMenuId == null).OrderBy(m => m.SortOrder);
        var filtering = !string.IsNullOrEmpty(_menuFilterText);

        BuildFavoriteMenuGroup(menus); // 항상 트리 맨 위에 오도록 "전체 메뉴"보다 먼저 Elements에 추가한다.

        // "전체 메뉴" 자신 - 시스템운영관리/기준정보관리 등 실제 모듈들을 전부 그 하위로
        // 묶는 부모 그룹(2026-09-21 요청 - "탭모양으로 바꿨던 전체 메뉴를 없애고 모듈들을
        // 전체메뉴 하위로 묶어달라"). "마이 메뉴"와 마찬가지로 accordionMenu 안의 진짜 Group이라
        // DevExpress 기본 클릭-토글이 그대로 동작하고, 헤더 자체는 항상 보인다 - 마이메뉴와
        // 상호배타 상태(_myMenuExpanded)로 Expanded만 결정한다. 검색 중엔 매치를 숨기지
        // 않도록 강제로 편다.
        var systemMenuGroup = new AccordionControlElement
        {
            Text = "전체 메뉴",
            Name = "SYSTEM_MENU",
            Style = ElementStyle.Group,
            Expanded = filtering || !_myMenuExpanded
        };
        systemMenuGroup.ImageOptions.Image = SvgIcons.Load(SvgIcons.Folder, MenuRootHeaderIconSize, NavText);
        systemMenuGroup.Appearance.Normal.BackColor = SidebarTreeBg;
        systemMenuGroup.Appearance.Normal.ForeColor = NavText;
        // Bold 제거해보는 중(2026-09-21 요청 - "메뉴그룹 Bold가 지저분해 보인다").
        systemMenuGroup.Appearance.Normal.Font = AppFonts.Body;
        systemMenuGroup.Appearance.Normal.Options.UseBackColor = true;
        systemMenuGroup.Appearance.Normal.Options.UseForeColor = true;
        systemMenuGroup.Appearance.Normal.Options.UseFont = true;
        systemMenuGroup.Appearance.Hovered.BackColor = NavHoverBg;
        systemMenuGroup.Appearance.Hovered.ForeColor = NavHoverFg;
        systemMenuGroup.Appearance.Hovered.Font = AppFonts.Body;
        systemMenuGroup.Appearance.Hovered.Options.UseBackColor = true;
        systemMenuGroup.Appearance.Hovered.Options.UseForeColor = true;
        systemMenuGroup.Appearance.Hovered.Options.UseFont = true;
        ApplyGroupPressedAppearance(systemMenuGroup, AppFonts.Body);

        foreach (var top in topMenus)
        {
            var iconName = (top.IconNm != null && TopMenuIcons.TryGetValue(top.IconNm, out var n)) ? n : SvgIcons.Folder;

            var group = new AccordionControlElement
            {
                // 대문자로 올려서 하위 화면(leaf)과는 다른 "카테고리 라벨"이라는 느낌을 준다 -
                // 한글은 대소문자가 없어 영향이 없고, "Developer Tool"처럼 영문 메뉴명만 실제로
                // "DEVELOPER TOOL"로 바뀐다.
                Text = top.MenuNm.ToUpperInvariant(),
                Name = top.MenuId.ToString(),
                Style = ElementStyle.Group
            };
            group.ImageOptions.Image = SvgIcons.Load(iconName, MenuTopIconSize, NavText);

            // 최상위 항목(모듈) - 개별 배경색은 주지 않고 사이드바 바탕색을 그대로 살려서
            // 평평한 리스트처럼 보이게 한다. Bold는 "지저분해 보인다"는 지적(2026-09-21)으로
            // 빼고 일반 굵기로 시험한다 - 위계는 색으로만 표현.
            group.Appearance.Normal.BackColor = SidebarTreeBg;
            group.Appearance.Normal.ForeColor = NavText;
            group.Appearance.Normal.Font = AppFonts.Body;
            group.Appearance.Normal.Options.UseBackColor = true;
            group.Appearance.Normal.Options.UseForeColor = true;
            group.Appearance.Normal.Options.UseFont = true;

            group.Appearance.Hovered.BackColor = NavHoverBg;
            group.Appearance.Hovered.ForeColor = NavHoverFg;
            group.Appearance.Hovered.Font = AppFonts.Body;
            group.Appearance.Hovered.Options.UseBackColor = true;
            group.Appearance.Hovered.Options.UseForeColor = true;
            group.Appearance.Hovered.Options.UseFont = true;

            ApplyGroupPressedAppearance(group, AppFonts.Body);

            var hasMatchingDescendant = AddChildMenus(group, menus, top.MenuId);
            var selfMatches = !filtering || top.MenuNm.IndexOf(_menuFilterText, StringComparison.OrdinalIgnoreCase) >= 0;
            if (!filtering || selfMatches || hasMatchingDescendant)
            {
                if (filtering) group.Expanded = true;
                systemMenuGroup.Elements.Add(group);
            }
        }

        _systemMenuGroupElement = systemMenuGroup;
        accordionMenu.Elements.Add(systemMenuGroup);
        }
        finally
        {
            accordionMenu.EndUpdate();
        }
    }

    /// <summary>"마이 메뉴" 헤더 클릭 - 마이메뉴/전체메뉴 상호배타 상태를 뒤집고 트리를
    /// 통째로 다시 그린다(2026-09-21 목업 요청 - 한쪽이 열리면 다른 쪽은 완전히 숨음).</summary>
    private void ToggleMyMenuMode()
    {
        _myMenuExpanded = !_myMenuExpanded;
        RebuildAccordionTree();
    }

    /// <summary>사이드바 맨 위 "마이 메뉴(N)" 그룹 - 즐겨찾기한 화면을 나열한다. 폴더 이름이
    /// 붙은 항목(FavoriteMenuEntry.Folder)은 그 이름의 하위 그룹으로 묶고, 폴더가 없는
    /// 항목은 마이 메뉴 바로 아래 평평하게 보인다(2026-09-21 목업 - "핵심 업무"/"자재 출납"
    /// 같은 사용자 임의 폴더). BuildAccordionMenu가 넘겨준 menus는 이미 ViewYn/개발자전용
    /// 필터를 거친 목록이라, 삭제되었거나 권한이 없어진 즐겨찾기는 자동으로 빠진다. 즐겨찾기가
    /// 하나도 없으면 그룹 자체를 만들지 않는다(빈 섹션을 보여줄 이유가 없다).</summary>
    private void BuildFavoriteMenuGroup(List<MenuDto> menus)
    {
        _favoritesGroupElement = null;
        _favoriteLeafElements.Clear();

        // _favorites 순서(=서버 SORT_ORDER, MoveFavoriteMenu로 사용자가 바꾼 순서)를 그대로
        // 유지한다 - 정렬해버리면 상위로/하위로 이동이 화면에 반영 안 된 것처럼 보인다.
        var rows = _favorites
            .Select(f => (Entry: f, Menu: menus.FirstOrDefault(m => m.MenuId == f.MenuId)))
            .Where(x => x.Menu != null && x.Menu!.MenuType != "GROUP")
            .Where(x => string.IsNullOrEmpty(_menuFilterText) || x.Menu!.MenuNm.IndexOf(_menuFilterText, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();

        // 2026-09-21 목업 - "마이 메뉴"는 즐겨찾기가 하나도 없어도 "마이 메뉴 (0)" 헤더
        // 자체는 항상 보이는 고정 섹션이다(예전엔 비어있으면 통째로 숨겼는데, 그러면 이
        // 기능이 있는지조차 알기 어려웠다). 단, 검색 필터링 중에 일치하는 즐겨찾기가 없을
        // 때는 계속 숨긴다(검색 결과 없음은 굳이 빈 섹션으로 보여줄 이유가 없다).
        var filtering = !string.IsNullOrEmpty(_menuFilterText);
        if (filtering && rows.Count == 0) return;

        var group = new AccordionControlElement
        {
            Text = $"My Menu ({rows.Count})",
            Name = "FAVORITES",
            Style = ElementStyle.Group,
            // 2026-09-21 목업: 처음엔 닫혀 있고(전체 메뉴만 보임), 헤더 클릭(ToggleMyMenuMode)
            // 때만 열린다 - 예전엔 항상 true(항상 펼침)였다.
            Expanded = _myMenuExpanded
        };
        // 헤더 배경이 전체 메뉴와 같은 중립 슬레이트로 통일되면서(2026-09-21) 별 아이콘은
        // 브랜드 블루로 - 배경과 더 이상 안 묶여 있으니 즐겨찾기 강조색만 담당한다.
        group.ImageOptions.Image = MenuIconPainters.Render(MenuIconPainters.Star, MenuRootHeaderIconSize, NavAccentBlue);

        group.Appearance.Normal.BackColor = SidebarTreeBg;
        group.Appearance.Normal.ForeColor = NavText;
        group.Appearance.Normal.Font = AppFonts.Body;
        group.Appearance.Normal.Options.UseBackColor = true;
        group.Appearance.Normal.Options.UseForeColor = true;
        group.Appearance.Normal.Options.UseFont = true;

        group.Appearance.Hovered.BackColor = NavHoverBg;
        group.Appearance.Hovered.ForeColor = NavHoverFg;
        group.Appearance.Hovered.Font = AppFonts.Body;
        group.Appearance.Hovered.Options.UseBackColor = true;
        group.Appearance.Hovered.Options.UseForeColor = true;
        group.Appearance.Hovered.Options.UseFont = true;

        ApplyGroupPressedAppearance(group, AppFonts.Body);

        AccordionControlElement CreateLeafElement(MenuDto menu)
        {
            // 전체 메뉴 쪽과 같은 이유로 공백 두 칸을 붙여 들여쓴다(AddChildMenus 주석 참고).
            var element = new AccordionControlElement { Text = "  " + menu.MenuNm, Name = menu.MenuId.ToString(), Style = ElementStyle.Item };
            element.ImageOptions.Image = MenuIconPainters.Render(MenuIconPainters.Dot, MenuLeafDotSize, ActionAccent);

            element.Appearance.Normal.BackColor = SidebarTreeBg;
            element.Appearance.Normal.ForeColor = NavText;
            element.Appearance.Normal.Font = AppFonts.Body;
            element.Appearance.Normal.Options.UseBackColor = true;
            element.Appearance.Normal.Options.UseForeColor = true;
            element.Appearance.Normal.Options.UseFont = true;

            element.Appearance.Hovered.BackColor = NavHoverBg;
            element.Appearance.Hovered.ForeColor = NavHoverFg;
            element.Appearance.Hovered.Font = AppFonts.BodyBold;
            element.Appearance.Hovered.Options.UseBackColor = true;
            element.Appearance.Hovered.Options.UseForeColor = true;
            element.Appearance.Hovered.Options.UseFont = true;

            element.Appearance.Pressed.BackColor = ColorHelper.Mix(SidebarTreeBg, NavAccentBlue, 0.15f);
            element.Appearance.Pressed.ForeColor = NavAccentBlue;
            element.Appearance.Pressed.Font = AppFonts.BodyBold;
            element.Appearance.Pressed.Options.UseBackColor = true;
            element.Appearance.Pressed.Options.UseForeColor = true;
            element.Appearance.Pressed.Options.UseFont = true;

            _favoriteLeafElements.Add(element);
            return element;
        }

        // 폴더별로 묶는다(순서는 첫 등장 순서, 즉 전역 정렬 그대로). 폴더 없음(null/빈 문자열)은
        // 마이 메뉴 바로 아래 평평하게 넣는다.
        var folderGroups = rows.GroupBy(x => string.IsNullOrWhiteSpace(x.Entry.Folder) ? null : x.Entry.Folder);

        foreach (var folderGroup in folderGroups)
        {
            var parent = group;
            if (folderGroup.Key != null)
            {
                var folderElement = new AccordionControlElement { Text = folderGroup.Key, Style = ElementStyle.Group, Expanded = true };
                folderElement.ImageOptions.Image = SvgIcons.Load(SvgIcons.Folder, MenuSubGroupIconSize, NavTextMuted);
                folderElement.Appearance.Normal.BackColor = SidebarTreeBg;
                folderElement.Appearance.Normal.ForeColor = NavTextMuted;
                folderElement.Appearance.Normal.Font = AppFonts.Body;
                folderElement.Appearance.Normal.Options.UseBackColor = true;
                folderElement.Appearance.Normal.Options.UseForeColor = true;
                folderElement.Appearance.Normal.Options.UseFont = true;
                folderElement.Appearance.Hovered.BackColor = NavHoverBg;
                folderElement.Appearance.Hovered.ForeColor = NavHoverFg;
                folderElement.Appearance.Hovered.Font = AppFonts.Body;
                folderElement.Appearance.Hovered.Options.UseBackColor = true;
                folderElement.Appearance.Hovered.Options.UseForeColor = true;
                folderElement.Appearance.Hovered.Options.UseFont = true;
                ApplyGroupPressedAppearance(folderElement, AppFonts.Body);

                group.Elements.Add(folderElement);
                parent = folderElement;
            }

            foreach (var (_, menu) in folderGroup)
                parent.Elements.Add(CreateLeafElement(menu!));
        }

        _favoritesGroupElement = group;
        accordionMenu.Elements.Add(group);
    }

    /// <summary>서버(TSMUSERFAVORITEMENU)에서 현재 사용자의 즐겨찾기 목록(메뉴ID+폴더)을 다시
    /// 받아와 트리를 재구성한다 - 로그인/서버전환/로그아웃-재로그인 등 사용자 세션이 바뀌는
    /// 시점마다 불러야 한다(즐겨찾기는 user_id 기준). 실패해도(오프라인/API 오류) 즐겨찾기는
    /// 부가 기능일 뿐이라 전체 메뉴트리 자체는 정상 표시되어야 하므로 예외를 삼킨다.</summary>
    private async Task ReloadFavoriteMenusAsync()
    {
        try
        {
            var rows = await ApiClient.GetAsync<List<FavoriteMenuDto>>("api/favorite-menu");
            _favorites.Clear();
            if (rows != null)
                _favorites.AddRange(rows.Select(r => new FavoriteMenuEntry { MenuId = r.MenuId, Folder = r.Folder }));
        }
        catch
        {
            // 부가 기능 - 실패해도 트리는 그대로 표시한다.
        }

        BuildAccordionMenu();
    }

    private bool IsFavorite(long menuId) => _favorites.Any(f => f.MenuId == menuId);

    /// <summary>사이드바 우클릭 메뉴의 "즐겨찾기 추가/해제" 핸들러 - 낙관적으로 로컬 목록을 먼저
    /// 바꾸고 트리를 다시 그린 뒤(즉각 반응), 서버에도 반영한다. 실패하면 오류만 알리고 다음
    /// ReloadFavoriteMenusAsync(로그인 등) 때 서버 진짜 상태로 다시 맞춰진다.</summary>
    private async void ToggleFavoriteMenu(long menuId, bool addFavorite)
    {
        if (addFavorite) _favorites.Add(new FavoriteMenuEntry { MenuId = menuId });
        else _favorites.RemoveAll(f => f.MenuId == menuId);

        BuildAccordionMenu();

        try
        {
            if (addFavorite) await ApiClient.PutAsync<object, ApiResult>($"api/favorite-menu/{menuId}", new { });
            else await ApiClient.DeleteAsync<ApiResult>($"api/favorite-menu/{menuId}");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"즐겨찾기 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
    }

    /// <summary>"마이 메뉴" 안에서 항목 순서를 바꾼다(상위로/하위로 이동) - 같은 폴더에 속한
    /// 항목끼리만 스왑한다(다른 폴더 항목이 전역 리스트에서 사이에 끼어 있어도 건너뛴다).
    /// 낙관적으로 로컬 목록을 먼저 바꾸고 다시 그린 뒤, 새 전역 순서를 서버에 보내
    /// SORT_ORDER를 맞춘다. direction: -1(상위로) / +1(하위로). 같은 폴더의 맨 위/맨
    /// 아래에서는 조용히 무시한다.</summary>
    private async void MoveFavoriteMenu(long menuId, int direction)
    {
        var entry = _favorites.FirstOrDefault(f => f.MenuId == menuId);
        if (entry == null) return;

        var sameFolder = _favorites.Where(f => f.Folder == entry.Folder).ToList();
        var localIndex = sameFolder.IndexOf(entry);
        var newLocalIndex = localIndex + direction;
        if (newLocalIndex < 0 || newLocalIndex >= sameFolder.Count) return;

        var swapWith = sameFolder[newLocalIndex];
        var globalIndexA = _favorites.IndexOf(entry);
        var globalIndexB = _favorites.IndexOf(swapWith);
        (_favorites[globalIndexA], _favorites[globalIndexB]) = (_favorites[globalIndexB], _favorites[globalIndexA]);

        BuildAccordionMenu();

        try
        {
            await ApiClient.PutAsync<List<long>, ApiResult>("api/favorite-menu/reorder", _favorites.Select(f => f.MenuId).ToList());
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"즐겨찾기 순서 변경 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
    }

    /// <summary>"폴더 지정..." - 즐겨찾기 하나를 마이 메뉴 안의 이름 붙은 폴더로 옮기거나
    /// (같은 이름을 쓰는 다른 항목과 자동으로 묶인다), 빈 문자열이면 폴더에서 뺀다.</summary>
    private async void SetFavoriteFolder(long menuId, string? folder)
    {
        var entry = _favorites.FirstOrDefault(f => f.MenuId == menuId);
        if (entry == null) return;

        entry.Folder = string.IsNullOrWhiteSpace(folder) ? null : folder!.Trim();

        BuildAccordionMenu();

        try
        {
            await ApiClient.PutAsync<string?, ApiResult>($"api/favorite-menu/{menuId}/folder", entry.Folder);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"폴더 지정 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
    }

    /// <summary>사이드바 우클릭 메뉴 - 실제 화면(Item) 위에서 우클릭했으면 "즐겨찾기 추가/해제"를
    /// 맨 위에 얹고(마이 메뉴 소속이면 "상위로/하위로 이동"/"폴더 지정..."도 같이), 그 외
    /// (그룹/빈 공간)에서는 "메뉴 새로고침"만 보여준다. tabContextMenu와 같은 이유로 클릭할
    /// 때마다 Items를 비우고 다시 채운다(대상이 매번 다른 항목이라서).</summary>
    private void ShowSidebarContextMenu(Point location)
    {
        sidebarContextMenu.Items.Clear();

        var hit = accordionMenu.CalcHitInfo(location);
        if (hit.HitTest == AccordionControlHitTest.Item &&
            hit.ItemInfo?.Element is { Style: ElementStyle.Item } element &&
            element.Name != null && long.TryParse(element.Name, out var menuId))
        {
            var isFavorite = IsFavorite(menuId);
            sidebarContextMenu.Items.Add(isFavorite ? "즐겨찾기 해제" : "즐겨찾기 추가", null,
                (s, e) => ToggleFavoriteMenu(menuId, !isFavorite));

            if (_favoriteLeafElements.Contains(element))
            {
                sidebarContextMenu.Items.Add("상위로 이동", null, (s, e) => MoveFavoriteMenu(menuId, -1));
                sidebarContextMenu.Items.Add("하위로 이동", null, (s, e) => MoveFavoriteMenu(menuId, 1));
                sidebarContextMenu.Items.Add("폴더 지정...", null, (s, e) =>
                {
                    var current = _favorites.FirstOrDefault(f => f.MenuId == menuId)?.Folder ?? string.Empty;
                    var input = DevExpress.XtraEditors.XtraInputBox.Show(
                        "마이 메뉴 폴더 이름 (비워두면 폴더 없이 바로 아래 표시)", "폴더 지정", current);
                    SetFavoriteFolder(menuId, input);
                });
            }

            sidebarContextMenu.Items.Add(new ToolStripSeparator());
        }

        // "전체 메뉴 모두 펼치기/접기" - accordionHeaderRow(예전 장식 바)의 "Expand" 체크박스를
        // 대신한다(2026-09-21, 그 바 자체를 없애면서 자리를 잃었다).
        sidebarContextMenu.Items.Add("전체 메뉴 모두 펼치기", null, (s, e) => SetAccordionMenuExpanded(true));
        sidebarContextMenu.Items.Add("전체 메뉴 모두 접기", null, (s, e) => SetAccordionMenuExpanded(false));
        sidebarContextMenu.Items.Add(new ToolStripSeparator());
        sidebarContextMenu.Items.Add("메뉴 새로고침", null, async (s, e) => await RefreshMenusAsync());
        sidebarContextMenu.Show(accordionMenu, location);
    }

    /// <summary>사이드바 우클릭 "메뉴 새로고침" - api/auth/menus(로그인 때와 같은 권한 계산
    /// 로직, AuthController.GetMyMenus)를 다시 불러와 SessionManager 캐시를 바꿔치기한 뒤
    /// 트리를 다시 그린다. ReLoginWithSameCredentials(서버 전환 재로그인)가 쓰는
    /// "accordionMenu.Elements.Clear(); BuildAccordionMenu();" 조합과 같은 방식이다.</summary>
    private async Task RefreshMenusAsync()
    {
        List<MenuDto>? menus;
        try
        {
            menus = await ApiClient.GetAsync<List<MenuDto>>("api/auth/menus");
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"메뉴를 새로고침하는 중 오류가 발생했습니다.\n{ex.Message}", "새로고침 실패");
            return;
        }

        if (menus == null)
        {
            AppMessageBox.Show("메뉴를 새로고침하는 중 오류가 발생했습니다.", "새로고침 실패");
            return;
        }

        SessionManager.Current.ReplaceMenus(menus);
        BuildAccordionMenu();
        Toast.Show("메뉴를 새로고침했습니다.");
    }


    private bool AddChildMenus(AccordionControlElement parent, List<MenuDto> allMenus, long upperMenuId)
    {
        return AddChildMenus(parent, allMenus, upperMenuId, new HashSet<long> { upperMenuId });
    }

    /// <summary>
    /// visited: 지금까지 내려온 조상 메뉴ID 목록. 메뉴관리 화면에서 상위메뉴를
    /// 순환되게(A→B→A) 잘못 지정해도, 이걸로 감지해서 무한재귀(StackOverflow로 인한
    /// 프로그램 다운)를 막는다. 순환이 감지되면 해당 하위 메뉴는 그냥 건너뛴다.
    ///
    /// 반환값: 하나라도 자식을 parent.Elements에 추가했는지(2026-09-21, 검색 필터 추가하며
    /// 도입) - _menuFilterText가 있을 때 "이 그룹 자체는 검색어와 안 맞아도 하위에 일치하는
    /// 화면이 있으니 그룹은 남겨야 한다"를 호출부(RebuildAccordionTree)가 판단하는 데 쓴다.
    /// </summary>
    private bool AddChildMenus(AccordionControlElement parent, List<MenuDto> allMenus, long upperMenuId, HashSet<long> visited)
    {
        var children = allMenus.Where(m => m.UpperMenuId == upperMenuId).OrderBy(m => m.SortOrder);
        var filtering = !string.IsNullOrEmpty(_menuFilterText);
        var anyAdded = false;

        foreach (var child in children)
        {
            if (!visited.Add(child.MenuId))
            {
                // 이미 조상 경로에 있던 메뉴가 다시 나타남 = 순환 참조. 건너뛰고 계속 진행.
                continue;
            }

            // 화면(leaf)만 텍스트 앞에 공백 두 칸을 붙여 서브그룹보다 살짝 더 들여써 보이게
            // 한다("서브그룹과 폼 정보가 왼쪽정렬이 거의 같아 구분이 안 간다" 지적, 2026-09-21) -
            // 처음엔 Graphics.TranslateTransform으로 그리기 좌표 자체를 밀었는데, DevExpress
            // 내부 클리핑 계산과 안 맞아 스크롤할 때마다 아이콘이 생겼다 없어지는 버그가
            // 났다(실제로 겪음) - 그 대신 텍스트 자체에 공백을 넣는(MDI 탭 제목과 같은 방식,
            // ConfigureTabAppearance의 PageAdded 참고) 훨씬 안전한 방법으로 바꿨다.
            var element = new AccordionControlElement
            {
                Text = child.MenuType == "GROUP" ? child.MenuNm : "  " + child.MenuNm,
                Name = child.MenuId.ToString(),
                Style = child.MenuType == "GROUP" ? ElementStyle.Group : ElementStyle.Item
            };

            if (child.MenuType == "GROUP")
            {
                // 2단계 이하 그룹(소분류) - 클릭해서 화면이 열리는 게 아니라 펼치기만 하는
                // "구획 라벨"이라는 걸 보여주려고, 오히려 화면(leaf)보다 차분한(muted) 색으로
                // 낮춘다. 폴더 아이콘까지 붙여서 화면(leaf)의 점 불릿과 뚜렷이 구분되게 한다 -
                // 굵기/색만으로는 구분이 잘 안 간다는 지적으로 추가(2026-09-02). 최상위 모듈이
                // 아이콘 없을 때 쓰는 기본 폴더 아이콘(SvgIcons.Folder)과 같은 그림을 그대로
                // 재사용해서 "폴더=펼침"이라는 시각 언어를 트리 전체에서 통일했다. Bold는
                // "지저분해 보인다"는 지적(2026-09-21)으로 뺐다 - 이제 위계는 색(muted)과
                // 폴더 아이콘만으로 표현한다.
                element.ImageOptions.Image = SvgIcons.Load(SvgIcons.Folder, MenuSubGroupIconSize, NavTextMuted);
                element.Appearance.Normal.BackColor = SidebarTreeBg;
                element.Appearance.Normal.ForeColor = NavTextMuted;
                element.Appearance.Normal.Font = AppFonts.Body;
                element.Appearance.Normal.Options.UseBackColor = true;
                element.Appearance.Normal.Options.UseForeColor = true;
                element.Appearance.Normal.Options.UseFont = true;

                element.Appearance.Hovered.BackColor = NavHoverBg;
                element.Appearance.Hovered.ForeColor = NavHoverFg;
                element.Appearance.Hovered.Font = AppFonts.Body;
                element.Appearance.Hovered.Options.UseBackColor = true;
                element.Appearance.Hovered.Options.UseForeColor = true;
                element.Appearance.Hovered.Options.UseFont = true;

                ApplyGroupPressedAppearance(element, AppFonts.Body);
            }
            else
            {
                // 실제 클릭 가능한 화면(FORM) - 들여쓰기를 깊게 하지 않고도 그룹과 구분되도록
                // 작은 점 불릿(강조색)을 붙이고, 글자색도 그룹보다 밝게 해서 "여기가 실제
                // 이동 가능한 화면"이라는 게 한눈에 보이게 한다. 호버 시 살짝 밝아지는 배경으로
                // 클릭 가능함을 한 번 더 보강.
                //
                // 이미 "마이 메뉴"에 즐겨찾기된 화면은 점 대신 작은 별 아이콘 + 굵은 강조색
                // 글자로 표시한다(2026-09-21 목업 - 전체 메뉴를 훑다가 이미 즐겨찾기한 화면을
                // 우클릭으로 또 확인할 필요 없이 한눈에 알아보도록).
                var isFavoriteMenu = IsFavorite(child.MenuId);
                element.ImageOptions.Image = isFavoriteMenu
                    ? MenuIconPainters.Render(MenuIconPainters.Star, MenuLeafDotSize + 2, NavAccentBlue)
                    : MenuIconPainters.Render(MenuIconPainters.Dot, MenuLeafDotSize, ActionAccent);
                element.Appearance.Normal.BackColor = SidebarTreeBg;
                element.Appearance.Normal.ForeColor = isFavoriteMenu ? NavAccentBlue : NavText;
                element.Appearance.Normal.Font = isFavoriteMenu ? AppFonts.BodyBold : AppFonts.Body;
                element.Appearance.Normal.Options.UseBackColor = true;
                element.Appearance.Normal.Options.UseForeColor = true;
                element.Appearance.Normal.Options.UseFont = true;

                element.Appearance.Hovered.BackColor = NavHoverBg;
                element.Appearance.Hovered.ForeColor = NavHoverFg;
                element.Appearance.Hovered.Font = AppFonts.BodyBold;
                element.Appearance.Hovered.Options.UseBackColor = true;
                element.Appearance.Hovered.Options.UseForeColor = true;
                element.Appearance.Hovered.Options.UseFont = true;

                // 클릭(선택) 상태 - 실제로 칠해지는 배경은 AccordionMenu_CustomDrawElement가
                // 전체 폭이 아니라 좌우로 살짝 띄운 둥근 알약(pill) 모양으로 따로 그린다(참고
                // 목업의 "미결재 문서함"처럼, 2026-09-17) - 여기 BackColor는 그 pill 색과 같은
                // 값을 써서 DrawHeaderBackground를 안 거치는 다른 경로(예: 포커스 사각형 등
                // DevExpress 내부 로직)에서도 색이 어긋나지 않게 맞춰만 둔다. 글자는 흰색+굵게로
                // 올려서 "선택됨"이 한눈에 보이게 한다 - 안 주면 Pressed가 스타일 안 먹은
                // 기본(더 작아 보이는) 폰트로 떨어지는 버그가 있었다(실제로 겪음).
                element.Appearance.Pressed.BackColor = ColorHelper.Mix(SidebarTreeBg, NavAccentBlue, 0.15f);
                element.Appearance.Pressed.ForeColor = NavAccentBlue;
                element.Appearance.Pressed.Font = AppFonts.BodyBold;
                element.Appearance.Pressed.Options.UseBackColor = true;
                element.Appearance.Pressed.Options.UseForeColor = true;
                element.Appearance.Pressed.Options.UseFont = true;
            }

            var hasMatchingDescendant = AddChildMenus(element, allMenus, child.MenuId, visited);
            var selfMatches = !filtering || child.MenuNm.IndexOf(_menuFilterText, StringComparison.OrdinalIgnoreCase) >= 0;

            if (!filtering || selfMatches || hasMatchingDescendant)
            {
                if (filtering && child.MenuType == "GROUP") element.Expanded = true; // 검색 결과가 접힌 채로 숨지 않게
                parent.Elements.Add(element);
                anyAdded = true;
            }

            visited.Remove(child.MenuId); // 형제 메뉴 처리를 위해 이 가지에서만 빠져나오면 복원
        }

        return anyAdded;
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
        element.Appearance.Pressed.ForeColor = NavHoverFg;
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
        if (element?.Name == null || !long.TryParse(element.Name, out var menuId)) return;

        var menu = SessionManager.Current.GetMenuAuth(menuId);
        if (menu != null) OpenMenuForm(menu);
    }

    /// <summary>
    /// 트리 각 행을 그릴 때마다 호출된다. 아이콘/텍스트는 DevExpress 기본 로직(DrawImage/
    /// DrawText 등, TabbedMdiManager_CustomDrawTabHeader와 같은 패턴)에 그대로 맡기고, 배경과
    /// 장식은 우리가 직접 그린다:
    ///  1) 세로 가이드라인 - 1단계 이상(하위 항목)의 행 왼쪽에 짧은 세로선을 그어, 들여쓰기만으론
    ///     흐릿했던 상위-하위 소속 관계를 더 또렷하게 만든다. 레벨마다 픽셀을 직접 계산하지
    ///     않는 이유: DevExpress가 이미 레벨에 비례해 HeaderBounds.X를 들여써주므로, 그 X에서
    ///     고정폭(6px)만 안쪽으로 들어가면 레벨마다 자연스럽게 제자리에 그려진다.
    ///  2) 선택된 화면(leaf, Pressed 상태) - 예전엔 DrawHeaderBackground로 전체 폭을 칠하고
    ///     왼쪽에 3px 바만 덧그렸는데, 참고 목업(2026-09-17, "미결재 문서함")처럼 좌우로 살짝
    ///     띄운 둥근 알약(pill) 모양만 강조색으로 채우는 방식으로 바꿨다 - 먼저 바탕색으로
    ///     전체를 지운 뒤 그 위에 알약만 얹는다(DrawHeaderBackground는 항상 전체 폭 사각형이라
    ///     이 모양을 낼 수 없어서 안 쓴다). 선택되지 않은 행은 그대로 DrawHeaderBackground를 쓴다.
    /// e.Handled를 true로 안 두면 이 이벤트 다음에 컨트롤이 자기 기본 그리기를 또 실행해서
    /// 방금 그린 장식을 덮어써 버린다(탭 헤더 커스텀드로우에서와 같은 이유) - 그래서 배경/
    /// 이미지/텍스트/버튼까지 전부 우리가 직접 순서대로 그려주고 마지막에 Handled = true로 막는다.
    /// </summary>
    /// <summary>"마이 메뉴"/"전체 메뉴" 헤더 배경색 - 2026-09-21 목업 요청("배경색 및
    /// 보더라인을 줘서 구분되게"). 마이 메뉴는 별 아이콘과 어울리는 옅은 골드, 전체 메뉴는
    /// 중립 슬레이트로 서로 다른 톤을 준다.</summary>
    // 마이 메뉴만 옅은 골드로 구분했었는데, "전체메뉴, mymenu의 배경색을 현재 전체 메뉴와
    // 동일하게 맞춰달라"는 요청(2026-09-21)으로 둘 다 같은 슬레이트 톤을 쓰도록 통일했다.
    private static readonly Color SystemMenuHeaderBg = Color.FromArgb(241, 245, 249);
    private static readonly Color SystemMenuHeaderBorder = Color.FromArgb(203, 213, 225);
    private static readonly Color FavoritesHeaderBg = SystemMenuHeaderBg;
    private static readonly Color FavoritesHeaderBorder = SystemMenuHeaderBorder;

    private void AccordionMenu_CustomDrawElement(object? sender, CustomDrawElementEventArgs e)
    {
        var bounds = e.ObjectInfo.HeaderBounds;
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        // "마이 메뉴"/"전체 메뉴" 자신의 헤더 줄만 별도 배경+테두리선으로 구분한다 - 그 안의
        // 하위 그룹/화면들은 평소 로직(가이드라인/선택 알약)을 그대로 탄다.
        if (e.Element == _favoritesGroupElement || e.Element == _systemMenuGroupElement)
        {
            var isFavoritesHeader = e.Element == _favoritesGroupElement;
            var fill = isFavoritesHeader ? FavoritesHeaderBg : SystemMenuHeaderBg;
            var border = isFavoritesHeader ? FavoritesHeaderBorder : SystemMenuHeaderBorder;

            using (var brush = new SolidBrush(fill))
                e.Graphics.FillRectangle(brush, bounds);
            using (var pen = new Pen(border))
            {
                e.Graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                // 마이 메뉴가 접혀 있으면(DistanceBetweenRootGroups=0이라) 바로 아래 전체 메뉴
                // 헤더와 맞닿아서, 마이 메뉴의 아래쪽 선 + 전체 메뉴의 위쪽 선이 거의 같은
                // 자리에 겹쳐 그려져 테두리가 두 겹으로 두꺼워 보였다("마이메뉴 닫으면 보더가
                // 이중으로 보인다" 지적, 2026-09-21) - 마이 메뉴는 아래쪽 선을 안 그리고, 그
                // 경계는 항상 존재하는 전체 메뉴의 위쪽 선 하나로만 표현한다.
                if (!isFavoritesHeader)
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
            }

            e.DrawImage();
            e.DrawText();
            e.DrawExpandCollapseButton();
            e.DrawContextButtons();
            e.Handled = true;
            return;
        }

        var isSelectedLeaf = e.Element.Style == ElementStyle.Item &&
            (e.ObjectInfo.State & DevExpress.Utils.Drawing.ObjectState.Pressed) != 0;

        if (isSelectedLeaf)
        {
            using var baseBrush = new SolidBrush(SidebarTreeBg);
            e.Graphics.FillRectangle(baseBrush, bounds);
        }
        else
        {
            e.DrawHeaderBackground();
        }

        if (e.Element.Level >= 1)
        {
            var guideX = bounds.X + 6;
            using var guidePen = new Pen(NavDivider);
            e.Graphics.DrawLine(guidePen, guideX, bounds.Top, guideX, bounds.Bottom);
        }

        if (isSelectedLeaf)
        {
            var pillRect = new Rectangle(bounds.X + 4, bounds.Y + 3, bounds.Width - 8, bounds.Height - 6);
            using var pillPath = RoundedRect(pillRect, 8);
            using var pillBrush = new SolidBrush(ColorHelper.Mix(SidebarTreeBg, NavAccentBlue, 0.15f));
            e.Graphics.FillPath(pillBrush, pillPath);
        }

        e.DrawImage();
        e.DrawText();
        e.DrawExpandCollapseButton();
        e.DrawContextButtons();
        e.Handled = true;
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        var d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
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

    /// <summary>HomeForm 대시보드의 바로가기/최근사용 카드에서 호출 - 메뉴ID로 화면을 연다</summary>
    public void OpenMenuById(long menuId)
    {
        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.MenuId == menuId);
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
        // MODULE("SM")+SCREEN_CLASS_NM("frmMenu") -> 리플렉션이 필요로 하는
        // "Namespace.Class, Assembly" 형태로 조합한다(예전엔 FORM_CLASS_NM 한 컬럼에 이 조합된
        // 문자열 자체가 저장돼 있었다 - 두 값으로 나뉜 이유는 MenuDto.Module 주석 참고).
        var formClassNm = !string.IsNullOrWhiteSpace(menu.Module) && !string.IsNullOrWhiteSpace(menu.ScreenClassNm)
            ? $"WYNLAB.{menu.Module}.{menu.ScreenClassNm}, WYNLAB.{menu.Module}"
            : null;
        if (string.IsNullOrWhiteSpace(formClassNm))
        {
            AppMessageBox.Show("연결된 화면이 없습니다. (MODULE/SCREEN_CLASS_NM 미설정)", "안내");
            return;
        }

        var menuIdText = menu.MenuId.ToString();
        var existing = MdiChildren.FirstOrDefault(f => f.Name == menuIdText);
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
        // net48의 string.IsNullOrWhiteSpace엔 [NotNullWhen(false)]가 없어 위 가드(2161행)로
        // 이미 확인했는데도 컴파일러가 못 알아채고 경고한다 - 안전하다.
        var nameParts = formClassNm!.Split(',');
        var typeName = nameParts[0].Trim();
        var assembly = nameParts.Length > 1 ? ModuleLoader.EnsureLoaded(nameParts[1].Trim()) : null;

        var formType = assembly?.GetType(typeName);
        if (formType == null)
        {
            AppMessageBox.Show($"화면을 찾을 수 없습니다: {formClassNm}", "오류");
            return;
        }

        BaseForm form;
        try
        {
            if (Activator.CreateInstance(formType) is not BaseForm created)
            {
                AppMessageBox.Show($"화면을 찾을 수 없습니다: {formClassNm}", "오류");
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
            AppMessageBox.Show($"화면을 여는 중 오류가 발생했습니다: {formClassNm}\n\n{real.GetType().Name}: {real.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        form.Name = menuIdText;
        form.MenuId = menu.MenuId;
        form.MdiParent = this;
        form.Show();

        SessionManager.Current.AddRecentMenu(menu);
    }
}
