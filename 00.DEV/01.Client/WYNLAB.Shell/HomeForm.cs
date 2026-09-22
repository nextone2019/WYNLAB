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

    private const int UserCardHeight = 104;
    private const int SectionGap = 16;
    private const int ApprovalSectionHeight = 250;
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
    private List<HomeScheduleItemDto> _todaySchedule = new();

    private Panel _noticeListHost = null!;
    private Panel _approvalListHost = null!;
    private Panel _scheduleListHost = null!;
    private LabelControl _approvalCountLabel = null!;

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

        try
        {
            var dash = await ApiClient.GetAsync<ApprovalDashboardResponse>("api/approvals/my-dashboard");
            _pendingApprovals = dash?.Pending ?? new();
        }
        catch { _pendingApprovals = new(); }
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

        var lblTitle = new LabelControl { Text = "오늘의 일정", Dock = DockStyle.Top, Height = 24, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

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

        var headerRow = new Panel { Dock = DockStyle.Top, Height = 26 };

        var lblTitle = new LabelControl { Text = "미결재 문서함", Location = new Point(0, 2), AutoSize = true };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

        _approvalCountLabel = new LabelControl { Location = new Point(lblTitle.Right + 8, 4), AutoSize = true };
        _approvalCountLabel.Appearance.Font = AppFonts.Caption;
        _approvalCountLabel.Appearance.ForeColor = PendingText;

        var lblGoInbox = new LabelControl { Text = "결재함 바로가기 >", Dock = DockStyle.Right, Width = 120, AutoSizeMode = LabelAutoSizeMode.None, Cursor = Cursors.Hand };
        lblGoInbox.Appearance.Font = AppFonts.Caption;
        lblGoInbox.Appearance.ForeColor = AccentBlue;
        lblGoInbox.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        lblGoInbox.Click += (s, e) => OpenScreen("AP", "frmApprInbox");

        headerRow.Controls.Add(lblTitle);
        headerRow.Controls.Add(_approvalCountLabel);
        headerRow.Controls.Add(lblGoInbox);

        var colHeaderRow = BuildApprovalColumnHeaderRow();

        _approvalListHost = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 4, 0, 0) };

        card.Controls.Add(_approvalListHost);
        card.Controls.Add(colHeaderRow);
        card.Controls.Add(headerRow);
        return card;
    }

    /// <summary>미결재 문서함 목록 위 컬럼 제목 줄(구분/기안제목/기안자/기안일시/상태) - 아래
    /// BuildApprovalRow와 같은 Dock 순서(안쪽->바깥쪽: 기안자/기안일시/상태)로 맞춰야 컬럼이
    /// 세로로 정렬된다.</summary>
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
        var lblReq = Head("기안자"); lblReq.Dock = DockStyle.Right; lblReq.Width = 90; lblReq.AutoSizeMode = LabelAutoSizeMode.None;
        var lblType = Head("구분"); lblType.Dock = DockStyle.Left; lblType.Width = 90; lblType.AutoSizeMode = LabelAutoSizeMode.None;
        var lblSubject = Head("기안제목"); lblSubject.Dock = DockStyle.Fill; lblSubject.AutoSizeMode = LabelAutoSizeMode.None;

        row.Controls.Add(lblSubject);
        row.Controls.Add(lblReq);
        row.Controls.Add(lblDate);
        row.Controls.Add(lblStatus);
        row.Controls.Add(lblType);
        row.Controls.Add(divider);
        return row;
    }

    /// <summary>_pendingApprovals(승인대상문서 - 내가 지금 처리할 차례인 결재건, USP_AP_APPR_Q
    /// Q1)로 _approvalListHost를 다시 그린다. Q1은 정의상(WHERE p.stat_cd='N') 전부 "아직 내
    /// 결재 전" 상태만 걸리므로 상태 배지는 매 행 "결재대기" 고정이다 - 목업처럼 "진행중"을 섞고
    /// 싶으면 기안문서(Q6, 내가 상신한 것) stat_cd를 따로 매핑하는 별도 위젯이 필요하다.</summary>
    private void RenderApprovalList()
    {
        _approvalCountLabel.Text = $"{_pendingApprovals.Count}건 대기";

        _approvalListHost.SuspendLayout();
        _approvalListHost.Controls.Clear();

        if (_pendingApprovals.Count == 0)
        {
            _approvalListHost.Controls.Add(BuildEmptyRow("결재 대기중인 문서가 없습니다."));
        }
        else
        {
            foreach (var item in Enumerable.Reverse(_pendingApprovals))
                _approvalListHost.Controls.Add(BuildApprovalRow(item));
        }

        _approvalListHost.ResumeLayout();
    }

    private Panel BuildApprovalRow(ApprovalDashboardItemDto item)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 38 };
        var divider = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = CardBorder };

        // 상태/기안일시/기안자 칸은 위 BuildApprovalColumnHeaderRow와 같은 폭(90)으로 맞춰서
        // 컬럼이 세로로 정렬되게 한다. 상태는 목업처럼 짙은 배지 + 가운데 정렬.
        var statusHost = new Panel { Dock = DockStyle.Right, Width = 90 };
        var statusBadge = BuildPillBadge("결재대기", StatusWaitingBg, StatusWaitingText);
        statusBadge.Location = new Point((statusHost.Width - statusBadge.Width) / 2, (row.Height - 1 - statusBadge.Height) / 2);
        statusHost.Controls.Add(statusBadge);

        var lblDate = new LabelControl { Text = item.AppDate, Dock = DockStyle.Right, Width = 90, Padding = new Padding(0, 11, 8, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblDate.Appearance.Font = AppFonts.Caption;
        lblDate.Appearance.ForeColor = TextMuted;

        var lblReq = new LabelControl { Text = item.ReqEmpNm ?? string.Empty, Dock = DockStyle.Right, Width = 90, Padding = new Padding(0, 11, 8, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblReq.Appearance.Font = AppFonts.Body;
        lblReq.Appearance.ForeColor = TextSecondary;

        var typeHost = new Panel { Dock = DockStyle.Left, Width = 90 };
        var typeBadge = BuildPillBadge(item.DocType, BadgeBg, TextSecondary);
        typeBadge.Location = new Point(0, (row.Height - 1 - typeBadge.Height) / 2);
        typeHost.Controls.Add(typeBadge);

        var lblTitle = new LabelControl { Text = item.AppTitle, Dock = DockStyle.Fill, Padding = new Padding(0, 11, 0, 0), AutoSizeMode = LabelAutoSizeMode.None, Cursor = Cursors.Hand };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;
        lblTitle.Click += (s, e) => OpenScreen("AP", "frmApprInbox");

        // Dock=Right 스택은 나중에 추가한 컨트롤이 바깥쪽(오른쪽 끝) 우선권을 가진다 - 화면에
        // 보일 순서(안쪽->바깥쪽: 기안자/기안일시/상태)대로 추가한다.
        row.Controls.Add(lblReq);
        row.Controls.Add(lblDate);
        row.Controls.Add(statusHost);
        row.Controls.Add(lblTitle);
        row.Controls.Add(typeHost);
        row.Controls.Add(divider);
        return row;
    }

    private Panel BuildNoticeSection()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = NoticeSectionHeight, Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var headerRow = new Panel { Dock = DockStyle.Top, Height = 26 };
        var lblTitle = new LabelControl { Text = "사내 공지사항", Location = new Point(0, 2), AutoSize = true };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

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
            foreach (var n in Enumerable.Reverse(_notices))
                _noticeListHost.Controls.Add(BuildNoticeRow(n));
        }

        _noticeListHost.ResumeLayout();
    }

    private Panel BuildNoticeRow(HomeNoticeItemDto n)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 33 };
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
