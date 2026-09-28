using DevExpress.XtraEditors;
using WYNLAB.Base;
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
public class HomeForm : BaseForm
{
    private static readonly Color PageBg = Color.FromArgb(247, 248, 250);
    private static readonly Color CardBorder = Color.FromArgb(226, 228, 232);
    private static readonly Color CardBg = Color.White;
    private static readonly Color TextPrimary = Color.FromArgb(35, 35, 38);
    private static readonly Color TextSecondary = Color.FromArgb(130, 132, 138);
    private static readonly Color TextMuted = Color.FromArgb(170, 172, 178);
    private static readonly Color AccentBlue = Color.FromArgb(79, 142, 247);
    private static readonly Color BadgeBg = Color.FromArgb(240, 242, 245);
    private static readonly Color PendingBg = Color.FromArgb(255, 236, 236);
    private static readonly Color PendingText = Color.FromArgb(191, 62, 62);
    // "결재대기" 상태 배지 전용(목업은 이것만 짙은 배지, 나머지는 옅은 배지) - PendingBg/Text와
    // 별도로 둔다("중요" 공지 배지는 계속 옅은 빨강을 쓴다).
    private static readonly Color StatusWaitingBg = Color.FromArgb(31, 35, 44);
    private static readonly Color StatusWaitingText = Color.White;
    // 기안함 탭 전용 상태 배지 색(app_stat_cd: 1=진행중/E=승인완료/R=반려) - 반려는 "중요" 공지
    // 배지와 같은 옅은 빨강을 재사용한다.
    private static readonly Color StatusProgressBg = Color.FromArgb(232, 240, 254);
    private static readonly Color StatusDoneBg = Color.FromArgb(230, 247, 237);
    private static readonly Color StatusDoneText = Color.FromArgb(56, 142, 60);
    // 목록 줄무늬(zebra) 배경 - 그룹웨어 포털 느낌을 위해 한 줄씩 아주 옅게 번갈아 칠한다.
    private static readonly Color ZebraBg = Color.FromArgb(249, 250, 252);
    // 공지사항/오늘의 일정 섹션 제목 앞 작은 색상 점(포털 위젯 구분용 - 파랑/결재는 탭 자체가
    // 헤더라 점이 따로 필요 없다).
    private static readonly Color NoticeAccent = Color.FromArgb(245, 158, 66);
    private static readonly Color ScheduleAccent = Color.FromArgb(76, 175, 125);

    private const int UserCardHeight = 104;
    private const int SectionGap = 16;
    private const int ApprovalSectionHeight = 270; // 탭 스트립(20) 추가분만큼 기존 250에서 키움
    private const int NoticeSectionHeight = 190;

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
    private List<ApprovalDashboardItemDto> _pendingApprovals = new();
    private List<ApprovalDashboardItemDto> _draftedApprovals = new();
    private List<HomeScheduleItemDto> _todaySchedule = new();
    // 구분 배지용 문서유형 코드->이름(L_AP0002 LookUp, api/combo-lookups) - 목록이 작고 거의 안
    // 바뀌어서 최초 1회만 불러와 캐시한다(RefreshDashboardAsync 참고).
    private Dictionary<string, string> _docTypeNames = new();

    // false=결재함(내가 승인해야 할 문서/Pending), true=기안함(내가 기안한 문서/Drafted)
    private bool _showDrafted;

    private Panel _noticeListHost = null!;
    private Panel _approvalListHost = null!;
    private Panel _scheduleListHost = null!;
    private Panel _approvalTabStrip = null!;
    private LabelControl _approvalCountLabel = null!;
    private LabelControl _approverColHeader = null!;

    public HomeForm()
    {
        Text = "Home";
        Name = "__HOME__"; // ShellForm이 이 이름으로 기존 홈 탭을 찾아서 재사용/보호함
        BackColor = PageBg;

        var scrollHost = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = PageBg };
        var content = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 24), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };

        // 목업대로 "오늘의 일정"은 페이지 오른쪽에 세로로 길게 고정하고(Dock=Right, 전체 높이),
        // 나머지 3블록(내 정보/미결재 문서함/사내 공지사항)은 왼쪽에 세로로 쌓는다. 오른쪽 칼럼의
        // 높이는 Dock=Right가 부모(mainRow) 높이를 그대로 따라가므로, mainRow의 Height를 왼쪽
        // 3블록 높이 합(+간격)으로 명시해주면 저절로 왼쪽과 같은 높이로 맞춰진다.
        var mainRow = new Panel
        {
            Dock = DockStyle.Top,
            Height = UserCardHeight + SectionGap + ApprovalSectionHeight + SectionGap + NoticeSectionHeight,
        };

        var scheduleCard = BuildScheduleCard();
        scheduleCard.Dock = DockStyle.Fill;
        var rightCol = new Panel { Dock = DockStyle.Right, Width = 320 };
        rightCol.Controls.Add(scheduleCard);

        var gap = new Panel { Dock = DockStyle.Right, Width = SectionGap };

        var leftCol = new Panel { Dock = DockStyle.Fill };
        var userCard = BuildUserCard();
        userCard.Dock = DockStyle.Top;
        userCard.Height = UserCardHeight;
        var approvalSection = BuildApprovalSection();
        var noticeSection = BuildNoticeSection();
        var gap1 = new Panel { Dock = DockStyle.Top, Height = SectionGap };
        var gap2 = new Panel { Dock = DockStyle.Top, Height = SectionGap };
        // Dock=Top 스택은 나중에 추가한 컨트롤이 위쪽 우선권을 가진다 - 화면 순서(위->아래:
        // 내정보/간격/미결재문서함/간격/공지사항)의 역순으로 추가한다.
        leftCol.Controls.Add(noticeSection);
        leftCol.Controls.Add(gap2);
        leftCol.Controls.Add(approvalSection);
        leftCol.Controls.Add(gap1);
        leftCol.Controls.Add(userCard);

        mainRow.Controls.Add(leftCol);
        mainRow.Controls.Add(gap);
        mainRow.Controls.Add(rightCol);

        content.Controls.Add(mainRow);

        scrollHost.Controls.Add(content);
        Controls.Add(scrollHost);

        Load += async (s, e) => await RefreshDashboardAsync();
    }

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
            _draftedApprovals = dash?.Drafted ?? new();
        }
        catch { _pendingApprovals = new(); _draftedApprovals = new(); }
        RenderApprovalList();

        try { _todaySchedule = await ApiClient.GetAsync<List<HomeScheduleItemDto>>("api/home/today-schedule") ?? new(); }
        catch { _todaySchedule = new(); }
        RenderSchedule();
    }

    /// <summary>이름/부서/관리자여부/현재 시각은 로그인 시점에 이미 세션에 있는 값이라(Session.
    /// UserNm/DeptNm/IsAdmin, DateTime.Now) 실데이터로 채우는 데 새 API가 필요 없었다. 목업의
    /// "접속일시/접속IP"는 세션에 그 값 자체가 없어(로그인 감사로그를 아직 안 만듦) 지어내지
    /// 않고 현재 시각으로 대체했다.</summary>
    private Panel BuildUserCard()
    {
        var card = new Panel { Padding = new Padding(20, 18, 20, 18) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        const int avatarSize = 56;
        var avatar = new Panel { Location = new Point(20, 18), Size = new Size(avatarSize, avatarSize) };
        var initial = string.IsNullOrEmpty(Session.UserNm) ? "?" : Session.UserNm.Substring(0, 1);
        avatar.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(AccentBlue);
            e.Graphics.FillEllipse(brush, 0, 0, avatarSize - 1, avatarSize - 1);
            using var font = new Font(AppFonts.Heading.FontFamily, 16f, FontStyle.Bold);
            using var textBrush = new SolidBrush(Color.White);
            var size = e.Graphics.MeasureString(initial, font);
            e.Graphics.DrawString(initial, font, textBrush, (avatarSize - size.Width) / 2, (avatarSize - size.Height) / 2);
        };

        var infoLeft = avatar.Right + 16;

        var lblName = new LabelControl { Text = $"{Session.UserNm}님", Location = new Point(infoLeft, 20), AutoSize = true };
        lblName.Appearance.Font = AppFonts.SubHeading;
        lblName.Appearance.ForeColor = TextPrimary;

        var badgeRow = new FlowLayoutPanel { Location = new Point(infoLeft + lblName.Width + 12, 18), AutoSize = true, WrapContents = false };
        if (!string.IsNullOrEmpty(Session.DeptNm)) badgeRow.Controls.Add(BuildPillBadge(Session.DeptNm, BadgeBg, TextSecondary));
        if (Session.IsAdmin) badgeRow.Controls.Add(BuildPillBadge("최고관리자", Color.FromArgb(232, 240, 254), AccentBlue));

        var now = DateTime.Now;
        var lblDetail = new LabelControl
        {
            Text = now.ToString("yyyy-MM-dd (ddd) HH:mm", System.Globalization.CultureInfo.GetCultureInfo("ko-KR")),
            Location = new Point(infoLeft, 50),
            AutoSize = true,
        };
        lblDetail.Appearance.Font = AppFonts.Body;
        lblDetail.Appearance.ForeColor = TextSecondary;

        card.Controls.Add(avatar);
        card.Controls.Add(lblName);
        card.Controls.Add(badgeRow);
        card.Controls.Add(lblDetail);
        return card;
    }

    private Panel BuildScheduleCard()
    {
        var card = new Panel { Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var lblTitle = BuildSectionTitle("오늘의 일정", ScheduleAccent);

        var lblDate = new LabelControl
        {
            Text = DateTime.Now.ToString("MM/dd (ddd)", System.Globalization.CultureInfo.GetCultureInfo("ko-KR")),
            Dock = DockStyle.Right,
            AutoSizeMode = LabelAutoSizeMode.None,
            Width = 80,
        };
        lblDate.Appearance.Font = AppFonts.Caption;
        lblDate.Appearance.ForeColor = TextSecondary;
        lblDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        var titleRow = new Panel { Dock = DockStyle.Top, Height = 24 };
        titleRow.Controls.Add(lblTitle);
        titleRow.Controls.Add(lblDate);

        // 실데이터는 RefreshDashboardAsync -> RenderSchedule이 채운다(다른 카드와 같은 패턴).
        _scheduleListHost = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 0) };

        card.Controls.Add(_scheduleListHost);
        card.Controls.Add(titleRow);
        return card;
    }

    /// <summary>_todaySchedule로 _scheduleListHost를 다시 그린다. 시간대(start_tm/end_tm)가
    /// 있으면 "HH:mm-HH:mm"로, 없으면(종일 일정) "종일"로 표시하고, color_cd를 SchedulePalette로
    /// 매핑한 점 하나를 앞에 찍는다(frmSchedule의 라벨 색과 같은 팔레트).</summary>
    private void RenderSchedule()
    {
        _scheduleListHost.SuspendLayout();
        _scheduleListHost.Controls.Clear();

        if (_todaySchedule.Count == 0)
        {
            _scheduleListHost.Controls.Add(BuildEmptyRow("오늘 등록된 일정이 없습니다."));
        }
        else
        {
            foreach (var s in Enumerable.Reverse(_todaySchedule))
            {
                var timeLabel = FormatHhMm(s.StartTm) is { Length: > 0 } st
                    ? (FormatHhMm(s.EndTm) is { Length: > 0 } et ? $"{st}-{et}" : st)
                    : "종일";
                var color = s.ColorCd is not null && SchedulePalette.TryGetValue(s.ColorCd, out var c) ? c : SchedulePalette["1"];
                _scheduleListHost.Controls.Add(BuildScheduleRow(color, timeLabel, s.Title));
            }
        }

        _scheduleListHost.ResumeLayout();
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
        lblTime.Appearance.Font = AppFonts.Caption;
        lblTime.Appearance.ForeColor = TextSecondary;

        var lblTitle = new LabelControl { Text = title, Location = new Point(74, 5), Size = new Size(row.Width - 78, 20), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        row.Controls.Add(dot);
        row.Controls.Add(lblTime);
        row.Controls.Add(lblTitle);
        return row;
    }

    private Panel BuildApprovalSection()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = ApprovalSectionHeight, Margin = new Padding(0, 0, 0, 0), Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var tabStrip = BuildApprovalTabStrip();

        var metaRow = new Panel { Dock = DockStyle.Top, Height = 22, Margin = new Padding(0, 8, 0, 0) };

        _approvalCountLabel = new LabelControl { Location = new Point(0, 3), AutoSize = true };
        _approvalCountLabel.Appearance.Font = AppFonts.Caption;
        _approvalCountLabel.Appearance.ForeColor = PendingText;

        var lblGoInbox = new LabelControl { Text = "결재함 바로가기 >", Dock = DockStyle.Right, Width = 120, AutoSizeMode = LabelAutoSizeMode.None, Cursor = Cursors.Hand };
        lblGoInbox.Appearance.Font = AppFonts.Caption;
        lblGoInbox.Appearance.ForeColor = AccentBlue;
        lblGoInbox.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        lblGoInbox.Click += (s, e) => OpenScreen("AP", "frmApprInbox");

        metaRow.Controls.Add(_approvalCountLabel);
        metaRow.Controls.Add(lblGoInbox);

        var colHeaderRow = BuildApprovalColumnHeaderRow();

        _approvalListHost = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 4, 0, 0) };

        card.Controls.Add(_approvalListHost);
        card.Controls.Add(colHeaderRow);
        card.Controls.Add(metaRow);
        card.Controls.Add(tabStrip);
        return card;
    }

    private static readonly string[] ApprovalTabLabels = { "결재함", "기안함" };
    private const int ApprovalTabWidth = 68;
    private const int ApprovalTabGap = 20;

    /// <summary>웹 포털에서 흔한 밑줄(underline) 탭 - 카드 전체 폭을 채우는 대신 왼쪽에 내용 폭
    /// 만큼만 고정 크기로 둔다(처음엔 카드 절반씩 채우는 탭으로 만들었는데 너무 커 보인다는
    /// 피드백으로 교체, 2026-09-29). 활성 탭은 굵은 글씨 + 파란 밑줄, 비활성 탭은 옅은 회색
    /// 글씨만 - DevExpress XtraTab은 전체 화면용이라 이런 작은 위젯엔 과해서 직접 GDI+로 그린다.</summary>
    private Panel BuildApprovalTabStrip()
    {
        var strip = new Panel { Dock = DockStyle.Top, Height = 30, Cursor = Cursors.Hand };
        strip.Paint += (s, e) => DrawApprovalTabStrip(strip, e.Graphics);
        strip.MouseClick += (s, e) =>
        {
            for (var i = 0; i < ApprovalTabLabels.Length; i++)
            {
                if (ApprovalTabRect(i).Contains(e.X, e.Y)) { SwitchApprovalTab(i == 1); return; }
            }
        };
        _approvalTabStrip = strip;
        return strip;
    }

    private static Rectangle ApprovalTabRect(int index) =>
        new(index * (ApprovalTabWidth + ApprovalTabGap), 0, ApprovalTabWidth, 30);

    private void DrawApprovalTabStrip(Panel strip, Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var activeIndex = _showDrafted ? 1 : 0;

        for (var i = 0; i < ApprovalTabLabels.Length; i++)
        {
            var rect = ApprovalTabRect(i);
            var active = i == activeIndex;

            using var font = new Font(AppFonts.Body.FontFamily, AppFonts.Body.Size, active ? FontStyle.Bold : FontStyle.Regular);
            using var textBrush = new SolidBrush(active ? TextPrimary : TextSecondary);
            var text = ApprovalTabLabels[i];
            var size = g.MeasureString(text, font);
            g.DrawString(text, font, textBrush, rect.X + (rect.Width - size.Width) / 2, rect.Y + 3);

            if (active)
            {
                using var pen = new Pen(AccentBlue, 2f);
                g.DrawLine(pen, rect.X + 6, rect.Bottom - 2, rect.Right - 6, rect.Bottom - 2);
            }
        }

        using var dividerPen = new Pen(CardBorder);
        g.DrawLine(dividerPen, 0, strip.Height - 1, strip.Width, strip.Height - 1);
    }

    /// <summary>미결재 문서함 목록 위 컬럼 제목 줄(구분/기안제목/기안자 또는 승인대기/기안일시/상태) -
    /// 아래 BuildApprovalRow와 같은 Dock 순서(안쪽->바깥쪽: 기안자·승인대기/기안일시/상태)로
    /// 맞춰야 컬럼이 세로로 정렬된다. 4번째 컬럼(_approverColHeader)은 탭에 따라 문구가 바뀐다
    /// (SwitchApprovalTab 참고).</summary>
    private Panel BuildApprovalColumnHeaderRow()
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 28, Margin = new Padding(0, 10, 0, 0) };
        var divider = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = CardBorder };

        LabelControl Head(string text)
        {
            var lbl = new LabelControl { Text = text };
            lbl.Appearance.Font = AppFonts.Caption;
            lbl.Appearance.ForeColor = TextMuted;
            return lbl;
        }

        var lblStatus = Head("상태"); lblStatus.Dock = DockStyle.Right; lblStatus.Width = 90; lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
        lblStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        var lblDate = Head("기안일시"); lblDate.Dock = DockStyle.Right; lblDate.Width = 90; lblDate.AutoSizeMode = LabelAutoSizeMode.None;
        _approverColHeader = Head("기안자"); _approverColHeader.Dock = DockStyle.Right; _approverColHeader.Width = 90; _approverColHeader.AutoSizeMode = LabelAutoSizeMode.None;
        var lblType = Head("구분"); lblType.Dock = DockStyle.Left; lblType.Width = 90; lblType.AutoSizeMode = LabelAutoSizeMode.None;
        var lblSubject = Head("기안제목"); lblSubject.Dock = DockStyle.Fill; lblSubject.AutoSizeMode = LabelAutoSizeMode.None;

        row.Controls.Add(lblSubject);
        row.Controls.Add(_approverColHeader);
        row.Controls.Add(lblDate);
        row.Controls.Add(lblStatus);
        row.Controls.Add(lblType);
        row.Controls.Add(divider);
        return row;
    }

    private void SwitchApprovalTab(bool showDrafted)
    {
        if (_showDrafted == showDrafted) return;
        _showDrafted = showDrafted;
        _approvalTabStrip.Invalidate();
        _approverColHeader.Text = _showDrafted ? "승인대기" : "기안자";
        RenderApprovalList();
    }

    /// <summary>활성 탭(_showDrafted)에 따라 _pendingApprovals(결재함, Q1) 또는 _draftedApprovals
    /// (기안함, Q6)로 _approvalListHost를 다시 그린다.</summary>
    private void RenderApprovalList()
    {
        var list = _showDrafted ? _draftedApprovals : _pendingApprovals;
        _approvalCountLabel.Text = _showDrafted ? $"{list.Count}건" : $"{list.Count}건 대기";

        _approvalListHost.SuspendLayout();
        _approvalListHost.Controls.Clear();

        if (list.Count == 0)
        {
            _approvalListHost.Controls.Add(BuildEmptyRow(_showDrafted ? "기안한 문서가 없습니다." : "결재 대기중인 문서가 없습니다."));
        }
        else
        {
            // Dock=Top 스택은 나중에 추가한 컨트롤이 위로 가므로, list[0]이 맨 위에 오려면
            // 뒤에서부터(아래 자리부터) 추가해야 한다 - 그 삽입 순서와 별개로 zebra 줄무늬는
            // 화면에 보이는 순서(=list 순서) 기준으로 매겨야 해서 i를 그대로 넘긴다.
            for (var i = list.Count - 1; i >= 0; i--)
                _approvalListHost.Controls.Add(BuildApprovalRow(list[i], i));
        }

        _approvalListHost.ResumeLayout();
    }

    /// <summary>결재함(Q1) 행은 정의상 전부 "아직 내 결재 전"이라 상태 배지가 항상 "결재대기"
    /// 고정이다. 기안함(Q6) 행은 item.StatCd(TAPDOC.app_stat_cd: 0/1/E/R)로 실제 진행상태를
    /// 보여준다 - [[project_wynlab_approval_status_code_convention]]과 같은 코드값.</summary>
    private (string Text, Color Bg, Color Fg) GetStatusBadge(ApprovalDashboardItemDto item)
    {
        if (!_showDrafted) return ("결재대기", StatusWaitingBg, StatusWaitingText);
        return item.StatCd switch
        {
            "E" => ("승인완료", StatusDoneBg, StatusDoneText),
            "R" => ("반려", PendingBg, PendingText),
            "1" => ("진행중", StatusProgressBg, AccentBlue),
            _ => ("결재상신", StatusWaitingBg, StatusWaitingText),
        };
    }

    private Panel BuildApprovalRow(ApprovalDashboardItemDto item, int displayIndex)
    {
        var zebra = displayIndex % 2 == 1 ? ZebraBg : CardBg;
        var row = new Panel { Dock = DockStyle.Top, Height = 38, BackColor = zebra };
        var divider = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = CardBorder };

        // 상태/기안일시/기안자 칸은 위 BuildApprovalColumnHeaderRow와 같은 폭(90)으로 맞춰서
        // 컬럼이 세로로 정렬되게 한다. 상태는 목업처럼 짙은 배지 + 가운데 정렬. statusHost/typeHost는
        // 일반 Panel이라 BackColor를 안 맞추면 zebra 줄무늬가 이 두 구간만 하얗게 끊겨 보인다.
        var (statusText, statusBg, statusFg) = GetStatusBadge(item);
        var statusHost = new Panel { Dock = DockStyle.Right, Width = 90, BackColor = zebra };
        var statusBadge = BuildPillBadge(statusText, statusBg, statusFg);
        statusBadge.Location = new Point((statusHost.Width - statusBadge.Width) / 2, (row.Height - 1 - statusBadge.Height) / 2);
        statusHost.Controls.Add(statusBadge);

        var lblDate = new LabelControl { Text = item.AppDate, Dock = DockStyle.Right, Width = 90, Padding = new Padding(0, 11, 8, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblDate.Appearance.Font = AppFonts.Caption;
        lblDate.Appearance.ForeColor = TextMuted;

        // 결재함 탭은 기안자(누가 올렸는지), 기안함 탭은 승인대기(지금 누가 처리할 차례인지)를
        // 같은 자리에 보여준다 - BuildApprovalColumnHeaderRow의 _approverColHeader와 짝.
        var approverText = _showDrafted ? (item.CurApprEmpNm ?? "-") : (item.ReqEmpNm ?? string.Empty);
        var lblApprover = new LabelControl { Text = approverText, Dock = DockStyle.Right, Width = 90, Padding = new Padding(0, 11, 8, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblApprover.Appearance.Font = AppFonts.Body;
        lblApprover.Appearance.ForeColor = TextSecondary;

        var typeHost = new Panel { Dock = DockStyle.Left, Width = 90, BackColor = zebra };
        var typeBadge = BuildPillBadge(ResolveDocTypeName(item.DocType), BadgeBg, TextSecondary);
        typeBadge.Location = new Point(0, (row.Height - 1 - typeBadge.Height) / 2);
        typeHost.Controls.Add(typeBadge);

        var lblTitle = new LabelControl { Text = item.AppTitle, Dock = DockStyle.Fill, Padding = new Padding(0, 11, 0, 0), AutoSizeMode = LabelAutoSizeMode.None, Cursor = Cursors.Hand };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;
        lblTitle.Click += (s, e) => OpenScreen("AP", "frmApprInbox");

        // Dock=Right 스택은 나중에 추가한 컨트롤이 바깥쪽(오른쪽 끝) 우선권을 가진다 - 화면에
        // 보일 순서(안쪽->바깥쪽: 기안자·승인대기/기안일시/상태)대로 추가한다.
        row.Controls.Add(lblApprover);
        row.Controls.Add(lblDate);
        row.Controls.Add(statusHost);
        row.Controls.Add(lblTitle);
        row.Controls.Add(typeHost);
        row.Controls.Add(divider);
        return row;
    }

    private string ResolveDocTypeName(string docType) =>
        _docTypeNames.TryGetValue(docType, out var name) ? name : docType;

    private Panel BuildNoticeSection()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = NoticeSectionHeight, Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var headerRow = new Panel { Dock = DockStyle.Top, Height = 26 };
        var lblTitle = BuildSectionTitle("사내 공지사항", NoticeAccent);
        lblTitle.Location = new Point(0, 2);

        var lblAdd = new LabelControl { Text = "+", Dock = DockStyle.Right, Width = 24, AutoSizeMode = LabelAutoSizeMode.None, Cursor = Cursors.Hand };
        lblAdd.Appearance.Font = AppFonts.BodyBold;
        lblAdd.Appearance.ForeColor = TextSecondary;
        lblAdd.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        lblAdd.Click += (s, e) => OpenScreen("SM", "frmBoard");

        headerRow.Controls.Add(lblTitle);
        headerRow.Controls.Add(lblAdd);

        // 실데이터는 LoadDashboardDataAsync -> RenderNotices가 채운다 - 최초 렌더 시점(생성자)엔
        // 아직 서버 응답이 안 왔으므로 빈 채로 두고, 응답이 오면 이 컨테이너 안만 다시 그린다.
        _noticeListHost = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 0) };

        card.Controls.Add(_noticeListHost);
        card.Controls.Add(headerRow);
        return card;
    }

    /// <summary>_notices(최대 5건, 서버가 이미 중요공지 우선/최신순으로 정렬해서 줌)로
    /// _noticeListHost를 다시 그린다. Dock=Top 스택은 나중에 추가한 컨트롤이 위로 가므로
    /// 화면에 보일 순서의 역순으로 추가한다.</summary>
    private void RenderNotices()
    {
        _noticeListHost.SuspendLayout();
        _noticeListHost.Controls.Clear();

        if (_notices.Count == 0)
        {
            _noticeListHost.Controls.Add(BuildEmptyRow("등록된 공지사항이 없습니다."));
        }
        else
        {
            // BuildApprovalRow와 같은 이유로 뒤에서부터 추가하되 zebra는 화면 표시 순서(=원래
            // _notices 순서) 기준 인덱스로 매긴다.
            for (var i = _notices.Count - 1; i >= 0; i--)
                _noticeListHost.Controls.Add(BuildNoticeRow(_notices[i], i));
        }

        _noticeListHost.ResumeLayout();
    }

    private Panel BuildNoticeRow(HomeNoticeItemDto n, int displayIndex)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 33, BackColor = displayIndex % 2 == 1 ? ZebraBg : CardBg };
        var divider = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = CardBorder };

        var lblDate = new LabelControl { Text = n.RegDt?.ToString("MM-dd") ?? string.Empty, Dock = DockStyle.Right, Width = 56, Padding = new Padding(0, 8, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblDate.Appearance.Font = AppFonts.Caption;
        lblDate.Appearance.ForeColor = TextMuted;
        lblDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        Panel? importantBadge = null;
        if (n.ImportantYn == "Y")
        {
            importantBadge = BuildPillBadge("중요", PendingBg, PendingText);
            importantBadge.Dock = DockStyle.Left;
            importantBadge.Margin = new Padding(0, 6, 8, 0);
        }

        var lblTitle = new LabelControl { Text = n.Title, Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        row.Controls.Add(lblTitle);
        if (importantBadge != null) row.Controls.Add(importantBadge);
        row.Controls.Add(lblDate);
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

    private Panel BuildEmptyRow(string message)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 33 };
        var lbl = new LabelControl { Text = message, Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lbl.Appearance.Font = AppFonts.Body;
        lbl.Appearance.ForeColor = TextMuted;
        row.Controls.Add(lbl);
        return row;
    }

    /// <summary>섹션 제목 앞에 작은 색상 점을 붙인다 - 포털 대시보드에서 흔한, 위젯마다 다른 색
    /// 아이콘을 다는 관행을 색 점 하나로 가볍게 흉내낸다(진짜 SVG 아이콘은 이런 홈 위젯 제목엔
    /// 과하다). 결재 섹션은 탭 자체가 제목 역할이라 이 헬퍼를 안 쓴다.</summary>
    private Panel BuildSectionTitle(string text, Color accentColor)
    {
        var panel = new Panel { Height = 24 };
        var dot = new Panel { Size = new Size(8, 8), Location = new Point(0, 8) };
        dot.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(accentColor);
            e.Graphics.FillEllipse(brush, 0, 0, 7, 7);
        };

        var lbl = new LabelControl { Text = text, Location = new Point(14, 3), AutoSize = true };
        lbl.Appearance.Font = AppFonts.BodyBold;
        lbl.Appearance.ForeColor = TextPrimary;

        panel.Controls.Add(dot);
        panel.Controls.Add(lbl);

        using (var g = panel.CreateGraphics())
        {
            var size = g.MeasureString(text, AppFonts.BodyBold);
            panel.Width = 14 + (int)size.Width;
        }

        return panel;
    }

    /// <summary>작은 알약(pill) 모양 배지 - 구분/상태/중요 표시에 공통으로 쓴다. 텍스트 폭에 맞춰
    /// AutoSize로 크기를 잡고 Paint에서 배경만 둥글게 채운다.</summary>
    private Panel BuildPillBadge(string text, Color bg, Color fg)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(10, 3), AutoSize = true };
        lbl.Appearance.Font = AppFonts.Caption;
        lbl.Appearance.ForeColor = fg;

        var badge = new Panel { Height = 20, AutoSize = false };
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
        // 경우가 있어서(DrawCardBorder류와 같은 이유로 생성자 시점에 한 번 고정폭을 준다).
        using (var g = badge.CreateGraphics())
        {
            var size = g.MeasureString(text, AppFonts.Caption);
            badge.Width = (int)size.Width + 20;
        }

        return badge;
    }

    /// <summary>카드 테두리를 살짝 둥글게 직접 그린다(DevExpress Panel엔 기본 라운드 테두리가 없어서)</summary>
    private static void DrawCardBorder(Panel card, Graphics g, Color color, int radius)
    {
        var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = new GraphicsPath();
        var d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        using var pen = new Pen(color);
        g.DrawPath(pen, path);
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
