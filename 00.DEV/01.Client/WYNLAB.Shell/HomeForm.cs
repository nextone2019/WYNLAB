using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Popup;
using WYNLAB.Shared.Dtos;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 직후 항상 MDI에 열려있는 홈 화면. 사용자가 탭을 닫을 수 없다
/// (OnFormClosing에서 앱 종료가 아닌 경우 취소).
///
/// 2026-09-22 사장님이 준 목업 이미지대로 레이아웃을 다시 짰다 - 기존 통계카드4장/빠른메뉴 줄을
/// 없애고, 상단(내 정보 카드 + 오늘의 일정), 미결재 문서함, 사내 공지사항 3블록으로 재구성.
/// "공지사항/결재/일정 실데이터를 활용해서"가 지시라 목업에 있던 항목 중 실제 DB에 없는 값
/// (접속일시/접속IP, 공지 카테고리 배지, 결재문서 상태 다양성)은 지어내지 않고 뺐다 - 승인대상
/// 문서(Q1)는 정의상 전부 "결재대기" 상태라 상태배지는 고정 텍스트다. 쪽지함 위젯도 목업에 없고
/// 지시에도 없어서 이번에 뺐다(unread-messages 호출 자체를 안 함).
/// </summary>
public partial class HomeForm : BaseForm
{
    private static readonly Color CardBorder = Color.FromArgb(222, 224, 229);
    private static readonly Color CardBg = Color.White;
    private static readonly Color TextPrimary = Color.FromArgb(35, 35, 38);
    private static readonly Color TextSecondary = Color.FromArgb(130, 132, 138);
    private static readonly Color TextMuted = Color.FromArgb(170, 172, 178);
    private static readonly Color AccentBlue = Color.FromArgb(79, 142, 247);
    private static readonly Color BadgeBg = Color.FromArgb(240, 242, 245);
    private static readonly Color PendingBg = Color.FromArgb(255, 236, 236);
    private static readonly Color PendingText = Color.FromArgb(191, 62, 62);
    // 목록 줄무늬(zebra) 배경 - 그룹웨어 포털 느낌을 위해 한 줄씩 아주 옅게 번갈아 칠한다.
    private static readonly Color ZebraBg = Color.FromArgb(249, 250, 252);

    // 색상/크기 중 화면에 보이는 고정 값(페이지 배경, 카드 간격, 섹션 높이 등)은 HomeForm.Designer.cs에 직접 들어 있다.
    // 아래 색상은 런타임에 동적으로 만드는 행(공지/일정 행, 배지, 기안서 작성 타일)이 쓴다.

    // frmSchedule.ColorPalette와 같은 값 - Shell은 화면 모듈(SM)을 참조하지 않으므로 여기 따로
    // 둔다(작은 값이라 중복이 참조보다 싸다). id는 DB에 저장된 COLOR_CD 그대로.
    private static readonly Dictionary<string, Color> SchedulePalette = new()
    {
        ["1"] = Color.FromArgb(0x4F, 0x8E, 0xF7),
        ["2"] = Color.FromArgb(0xF7, 0x66, 0x66),
        ["3"] = Color.FromArgb(0x4C, 0xAF, 0x7D),
        ["4"] = Color.FromArgb(0xF5, 0xA7, 0x42),
        ["5"] = Color.FromArgb(0x9B, 0x6F, 0xD6),
        ["6"] = Color.FromArgb(0x8C, 0x92, 0x9B),
        ["7"] = Color.FromArgb(0x3F, 0xC1, 0xC9),
        ["8"] = Color.FromArgb(0xF2, 0x7F, 0xB0),
    };

    private List<HomeNoticeItemDto> _notices = new();
    // KPI "전자결재 대기" 건수 전용(목록 자체는 결재 리스트 그리드가 따로 조회한다).
    private List<ApprovalDashboardItemDto> _pendingApprovals = new();
    private List<HomeScheduleItemDto> _todaySchedule = new();
    // 구분 배지용 문서유형 코드->이름(L_AP0002 LookUp, api/combo-lookups) - 목록이 작고 거의 안
    // 바뀌어서 최초 1회만 불러와 캐시한다(RefreshDashboardAsync 참고).
    private Dictionary<string, string> _docTypeNames = new();

    // 디자이너 구성이 끝나기 전에 발생하는 컨트롤 이벤트(탭 선택 변경 등)를 무시하기 위한 플래그.
    private bool _homeReady;

    public HomeForm()
    {
        InitializeComponent();

        // 디자이너(HomeForm.Designer.cs)가 만든 고정 컨트롤에 런타임 값(아이콘 이미지/사용자 정보/기본 검색조건)을 채운다.
        InitializeHome();
        InitializeApprovalCenter();
        _homeReady = true;
    }

    /// <summary>디자이너에서 정할 수 없는 런타임 값만 여기서 채운다 - 오늘 날짜 문구, 처음엔 안 보이는 기안서 작성 화면.</summary>
    private void InitializeHome()
    {
        lblScheduleDate.Text = DateTime.Now.ToString("MM/dd (ddd)", System.Globalization.CultureInfo.GetCultureInfo("ko-KR"));
        panComposeView.Visible = false;
    }

    private async void HomeForm_Load(object? sender, EventArgs e) => await RefreshDashboardAsync();

    /// <summary>섹션 제목 앞 색상 점 - 점 색은 디자이너에서 정한 패널의 ForeColor를 그대로 쓴다.</summary>
    private void SectionDot_Paint(object? sender, PaintEventArgs e)
    {
        var dot = (Control)sender!;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var brush = new SolidBrush(dot.ForeColor);
        e.Graphics.FillEllipse(brush, 0, 0, 7, 7);
    }

    /// <summary>표 테두리 1px - 공지 표 프레임이 쓴다(결재 검색조건 패널은 panSearch_Paint).</summary>
    private void BorderFrame_Paint(object? sender, PaintEventArgs e)
    {
        var frame = (Control)sender!;
        using var pen = new Pen(CardBorder);
        e.Graphics.DrawRectangle(pen, 0, 0, frame.Width - 1, frame.Height - 1);
    }

    /// <summary>빠른 실행 타일/글자 클릭 - Tag="모듈|화면클래스명"(디자이너에서 지정).</summary>
    private void QuickLaunch_Click(object? sender, EventArgs e)
    {
        if (((Control)sender!).Tag is not string tag) return;
        var parts = tag.Split('|');
        if (parts.Length == 2) OpenScreen(parts[0], parts[1]);
    }

    private void lblNoticeAdd_Click(object? sender, EventArgs e) => OpenScreen("SM", "frmBoard");

    /// <summary>공지사항/전자결재(승인대상문서)/오늘 일정 실데이터를 받아와 이미 그려진 카드
    /// 내용을 채워 넣는다 - 하나가 실패해도(네트워크 순간 끊김 등) 서로 영향 없게 각각 try/catch로
    /// 감싼다. HomeForm은 사용자가 닫을 수 없는 화면이라 여기서 예외가 새어나가면 안 된다.
    ///
    /// public인 이유 - 최초 로드(Load 이벤트) 때만 부르면, 공지사항을 새로 등록하고 홈 탭으로
    /// 돌아와도 화면이 그대로라 "새로고침이 없다"는 지적을 받았다. ShellForm이 MDI 탭을 홈으로
    /// 전환할 때마다(MdiChildActivate) 이 메서드를 다시 불러서, 다른 화면에서 뭔가 바꾸고
    /// 돌아오면 항상 최신 상태로 보이게 한다.</summary>
    public async Task RefreshDashboardAsync()
    {
        try { _notices = await ApiClient.GetAsync<List<HomeNoticeItemDto>>("api/home/notices?topN=5") ?? new(); }
        catch { _notices = new(); }
        RenderNotices();

        if (_docTypeNames.Count == 0)
        {
            try
            {
                var lookup = await ApiClient.PostAsync<Dictionary<string, string?>, LookupItemsResultDto>("api/combo-lookups/L_AP0002/items", new());
                _docTypeNames = lookup?.Items.ToDictionary(i => i.Value, i => i.Display) ?? new();
            }
            catch { /* 못 불러와도 원래 코드값을 그대로 보여주면 되므로 화면이 죽으면 안 됨 */ }
        }

        try
        {
            var dash = await ApiClient.GetAsync<ApprovalDashboardResponse>("api/approvals/my-dashboard");
            _pendingApprovals = dash?.Pending ?? new();
        }
        catch { _pendingApprovals = new(); }

        // 결재 리스트(기안함/결재함 그리드)와 기안서 작성 타일 - 현재 검색조건/탭 그대로 다시 조회한다.
        await LoadDocTypesAsync();
        await QueryApprovalAsync(silent: true);

        try { _todaySchedule = await ApiClient.GetAsync<List<HomeScheduleItemDto>>("api/home/today-schedule") ?? new(); }
        catch { _todaySchedule = new(); }
        RenderSchedule();
    }

    /// <summary>_todaySchedule로 panScheduleList를 다시 그린다. 시간대(start_tm/end_tm)가
    /// 있으면 "HH:mm-HH:mm"로, 없으면(종일 일정) "종일"로 표시하고, color_cd를 SchedulePalette로
    /// 매핑한 점 하나를 앞에 찍는다(frmSchedule의 라벨 색과 같은 팔레트).</summary>
    private void RenderSchedule()
    {
        panScheduleList.SuspendLayout();
        panScheduleList.Controls.Clear();

        if (_todaySchedule.Count == 0)
        {
            panScheduleList.Controls.Add(BuildEmptyRow("오늘 등록된 일정이 없습니다."));
        }
        else
        {
            foreach (var s in Enumerable.Reverse(_todaySchedule))
            {
                var timeLabel = FormatHhMm(s.StartTm) is { Length: > 0 } st
                    ? (FormatHhMm(s.EndTm) is { Length: > 0 } et ? $"{st}-{et}" : st)
                    : "종일";
                var color = s.ColorCd is not null && SchedulePalette.TryGetValue(s.ColorCd, out var c) ? c : SchedulePalette["1"];
                panScheduleList.Controls.Add(BuildScheduleRow(color, timeLabel, s.Title));
            }
        }

        panScheduleList.ResumeLayout();
    }

    private static string? FormatHhMm(string? hhmm) =>
        hhmm is { Length: 4 } ? $"{hhmm.Substring(0, 2)}:{hhmm.Substring(2, 2)}" : null;

    private Panel BuildScheduleRow(Color dotColor, string time, string title)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 30 };

        var dot = new Panel { Location = new Point(2, 11), Size = new Size(8, 8) };
        dot.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(dotColor);
            e.Graphics.FillEllipse(brush, 0, 0, 7, 7);
        };

        var lblTime = new LabelControl { Text = time, Location = new Point(16, 6), Size = new Size(56, 18), AutoSizeMode = LabelAutoSizeMode.None };
        lblTime.Appearance.Font = AppFonts.Body;
        lblTime.Appearance.ForeColor = TextSecondary;

        var lblTitle = new LabelControl { Text = title, Location = new Point(74, 5), Size = new Size(row.Width - 78, 20), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        row.Controls.Add(dot);
        row.Controls.Add(lblTime);
        row.Controls.Add(lblTitle);
        return row;
    }

    // 공지 목록 컬럼 폭 - 좌우 끝에는 RowSideMargin만큼 여백을 둬서 표 테두리에 글자가 붙지 않게 한다.
    private const int ColTypeWidth = 90;
    private const int ColApproverWidth = 90;
    private const int RowSideMargin = 12;

    /// <summary>행/머리글 양 끝에 같은 배경색의 고정폭 여백 패널을 붙인다 - Dock=Left/Right 스택은 나중에
    /// 추가한 컨트롤이 바깥쪽이므로 항상 맨 마지막에 호출한다. row.Padding을 쓰지 않는 이유는 행 구분선
    /// (Dock=Top/Bottom 1px 패널)까지 안쪽으로 줄어들어 선이 끊겨 보이기 때문이다.</summary>
    private static void AddRowSideMargins(Panel row, Color back)
    {
        row.Controls.Add(new Panel { Dock = DockStyle.Left, Width = RowSideMargin, BackColor = back });
        row.Controls.Add(new Panel { Dock = DockStyle.Right, Width = RowSideMargin, BackColor = back });
    }

    private string ResolveDocTypeName(string docType) =>
        _docTypeNames.TryGetValue(docType, out var name) ? name : docType;

    private const int ColNoticeDateWidth = 150;
    // 공지 행 높이 - 결재 리스트 그리드 행의 화면상 높이(gvwApproval.RowHeight 25 + 행 구분선/여백 = 28)와 같게 맞춘다.
    private const int NoticeRowHeight = 28;

    /// <summary>_notices(최대 5건, 서버가 이미 중요공지 우선/최신순으로 정렬해서 줌)로
    /// panNoticeList를 다시 그린다. Dock=Top 스택은 나중에 추가한 컨트롤이 위로 가므로
    /// 화면에 보일 순서의 역순으로 추가한다.</summary>
    private void RenderNotices()
    {
        panNoticeList.SuspendLayout();
        panNoticeList.Controls.Clear();

        if (_notices.Count == 0)
        {
            panNoticeList.Controls.Add(BuildEmptyRow("등록된 공지사항이 없습니다."));
        }
        else
        {
            // BuildApprovalRow와 같은 이유로 뒤에서부터 추가하되 zebra는 화면 표시 순서(=원래
            // _notices 순서) 기준 인덱스로 매긴다.
            for (var i = _notices.Count - 1; i >= 0; i--)
                panNoticeList.Controls.Add(BuildNoticeRow(_notices[i], i));
        }

        panNoticeList.ResumeLayout();
    }

    private Panel BuildNoticeRow(HomeNoticeItemDto n, int displayIndex)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = NoticeRowHeight, BackColor = displayIndex % 2 == 1 ? ZebraBg : CardBg };
        var divider = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = CardBorder };

        // 작성일시 "yyyy-MM-dd HH:mm" / 작성자 - 머리글(BuildNoticeColumnHeaderRow)과 같은 폭, 왼쪽 정렬.
        var lblDate = new LabelControl { Text = n.RegDt?.ToString("yyyy-MM-dd HH:mm") ?? string.Empty, Dock = DockStyle.Right, Width = ColNoticeDateWidth, Padding = new Padding(0, 6, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblDate.Appearance.Font = AppFonts.Body;
        lblDate.Appearance.ForeColor = TextMuted;

        var lblAuthor = new LabelControl { Text = n.EmpNm, Dock = DockStyle.Right, Width = ColApproverWidth, Padding = new Padding(0, 6, 8, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblAuthor.Appearance.Font = AppFonts.Body;
        lblAuthor.Appearance.ForeColor = TextSecondary;

        // "중요" 배지를 Dock=Left로 직접 붙이면 Panel이 부모(row, 33px)의 전체 높이로 늘어나
        // 버려서(Dock은 세로 축도 꽉 채운다 - Margin은 일반 Panel 레이아웃에서 무시됨) 배지
        // Paint가 그 늘어난 높이로 알약을 그려 텍스트 줄과 크기가 안 맞아 보였다("중요 표시가
        // 라인에 사이즈도 안 맞아" 지적, 2026-10-02) - 다른 배지들(구분/상태)과 같은 방식으로
        // 고정폭 호스트 안에 Location으로 세로 중앙 정렬해서 배지 자체 크기(Height=22)를 지킨다.
        // 중요하지 않은 공지도 칸 폭은 똑같이 차지해야 제목 시작 위치가 모든 행에서 같다.
        var badgeHost = new Panel { Dock = DockStyle.Left, Width = ColTypeWidth, BackColor = row.BackColor };
        if (n.ImportantYn == "Y")
        {
            var importantBadge = BuildPillBadge("중요", PendingBg, PendingText);
            importantBadge.Location = new Point(0, (row.Height - 1 - importantBadge.Height) / 2);
            badgeHost.Controls.Add(importantBadge);
        }

        var lblTitle = new LabelControl { Text = n.Title, Dock = DockStyle.Fill, Padding = new Padding(0, 6, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        row.Controls.Add(lblTitle);
        row.Controls.Add(lblAuthor);
        row.Controls.Add(lblDate);
        row.Controls.Add(badgeHost);
        AddRowSideMargins(row, row.BackColor);
        row.Controls.Add(divider);
        return row;
    }

    /// <summary>빠른메뉴 타일(예전 BuildQuickMenuTile)과 같은 방식 - 모듈/화면클래스명으로
    /// SessionManager의 권한필터된 메뉴목록에서 찾아 연다. 권한이 없거나 메뉴 자체가 없으면
    /// 조용히 안내만 띄운다.</summary>
    private void OpenScreen(string module, string screenClassNm)
    {
        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == module && m.ScreenClassNm == screenClassNm);
        if (menu == null)
        {
            AppMessageBox.Show("연결된 메뉴가 없거나 접근 권한이 없습니다.", "안내");
            return;
        }
        (MdiParent as ShellForm)?.OpenMenuById(menu.MenuId);
    }

    /// <summary>문서번호 클릭 - frmApprInbox.OpenOriginalDocumentAsync와 완전히 같은 방식으로
    /// item.FormId("{MODULE}.{화면클래스명}")를 풀어서 원본 업무화면을 그 건에 포커스해서 연다.
    /// 홈화면은 결재 액션이 없으므로 여기서 열고 나면, 사용자가 그 화면의 전자결재 버튼을 직접
    /// 눌러 결재를 진행한다.</summary>
    private async Task OpenOriginalDocumentAsync(ApprovalDashboardItemDto item)
    {
        if (string.IsNullOrWhiteSpace(item.FormId))
        {
            AppMessageBox.Show("연결된 원본 화면 정보가 없습니다.", "안내");
            return;
        }

        var parts = item.FormId.Split('.');
        if (parts.Length != 2)
        {
            AppMessageBox.Show($"원본 화면 정보 형식이 올바르지 않습니다: {item.FormId}", "오류");
            return;
        }
        var module = parts[0];
        var className = parts[1];

        var assembly = ModuleLoader.EnsureLoaded($"WYNLAB.{module}");
        var formType = assembly?.GetType($"WYNLAB.{module}.{className}");
        if (formType == null || Activator.CreateInstance(formType) is not BaseForm form)
        {
            AppMessageBox.Show($"화면을 찾을 수 없습니다: {item.FormId}", "오류");
            return;
        }

        var menu = SessionManager.Current.Menus.FirstOrDefault(m => m.Module == module && m.ScreenClassNm == className);
        form.MenuId = menu?.MenuId ?? 0;
        form.MdiParent = MdiParent;
        form.Show();

        await form.FocusRecordAsync(item.DocId.ToString());
    }

    /// <summary>결재번호 클릭 - 원본 업무화면을 거치지 않고 전자결재 팝업(popApp)을 바로 띄운다.
    /// 이미 상신된 건(app_id 있음)이라 popApp.ShowAsync가 서버 이력을 조회해서 곧바로 처리모드
    /// (승인/반려)로 연다 - title/text 인자는 아직 상신 전(작성모드)일 때만 쓰이므로 여기선
    /// 의미가 없다(그런 상황 자체가 안 생김 - 홈 목록은 전부 이미 상신된 건).</summary>
    private async Task OpenApprovalPopupAsync(ApprovalDashboardItemDto item)
    {
        var changed = await popApp.ShowAsync(item.DocType, item.DocId, item.DocNo, item.AppTitle, string.Empty, this);
        if (changed) await RefreshDashboardAsync();
    }

    private Panel BuildEmptyRow(string message)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 33 };
        var lbl = new LabelControl { Text = message, Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lbl.Appearance.Font = AppFonts.Body;
        lbl.Appearance.ForeColor = TextMuted;
        row.Controls.Add(lbl);
        return row;
    }

    /// <summary>작은 알약(pill) 모양 배지 - 구분/상태/중요 표시에 공통으로 쓴다. 텍스트 폭에 맞춰
    /// AutoSize로 크기를 잡고 Paint에서 배경만 둥글게 채운다.</summary>
    private Panel BuildPillBadge(string text, Color bg, Color fg)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(10, 3), AutoSize = true };
        lbl.Appearance.Font = AppFonts.Body;
        lbl.Appearance.ForeColor = fg;

        var badge = new Panel { Height = 22, AutoSize = false };
        badge.Controls.Add(lbl);
        badge.SizeChanged += (s, e) => lbl.Location = new Point(10, (badge.Height - lbl.Height) / 2);
        badge.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(bg);
            var rect = new Rectangle(0, 0, badge.Width - 1, badge.Height - 1);
            using var path = new GraphicsPath();
            var d = badge.Height;
            path.AddArc(rect.X, rect.Y, d, d, 90, 180);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 180);
            path.CloseFigure();
            e.Graphics.FillPath(brush, path);
        };

        // 텍스트 폭 + 좌우 여백(10+10)으로 배지 폭을 정한다 - AutoSize 대신 직접 계산하는 이유는
        // 위 Paint가 Width를 참조하는데 AutoSize 타이밍과 얽히면 첫 렌더에서 0폭으로 그려지는
        // 경우가 있어서(생성자 시점에 한 번 고정폭을 준다).
        using (var g = badge.CreateGraphics())
        {
            var size = g.MeasureString(text, AppFonts.Body);
            badge.Width = (int)size.Width + 20;
        }

        return badge;
    }

    /// <summary>둥근 모서리 사각형 경로 - 탭 알약/배지 등 이 파일 여러 곳에서 공유하는 모양.</summary>
    private static GraphicsPath RoundedRectPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        var d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>
    /// 사용자가 탭의 X 버튼을 눌러도 닫히지 않도록 막는다.
    /// 전체 프로그램이 종료되거나(MDI 부모가 닫힐 때), 서버 전환으로 강제 정리될 때만 허용.
    /// </summary>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.MdiFormClosing)
        {
            e.Cancel = true;
            return;
        }
        base.OnFormClosing(e);
    }
}
