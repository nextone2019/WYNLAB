using DevExpress.XtraEditors;
using WYNLAB.Base;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 직후 항상 MDI에 열려있는 홈 화면. 사용자가 탭을 닫을 수 없다
/// (OnFormClosing에서 앱 종료가 아닌 경우 취소).
///
/// 일반적인 그룹웨어 홈 화면(요약 카드/공지사항/일정/결재대기/빠른메뉴) 목업을 그대로
/// 화면으로 옮긴 것 - 공지사항/일정/결재 기능은 아직 DB/API가 없어서 전부 고정 문구다.
/// 실제 기능이 생기면 그때 해당 섹션만 데이터 연동으로 바꾸면 된다.
/// </summary>
public class HomeForm : BaseForm
{
    private static readonly Color PageBg = Color.FromArgb(247, 248, 250);
    private static readonly Color CardBorder = Color.FromArgb(226, 228, 232);
    private static readonly Color CardBg = Color.White;
    private static readonly Color TextPrimary = Color.FromArgb(35, 35, 38);
    private static readonly Color TextSecondary = Color.FromArgb(130, 132, 138);
    private static readonly Color TextMuted = Color.FromArgb(170, 172, 178);
    private static readonly Color DangerText = Color.FromArgb(153, 60, 29);

    public HomeForm()
    {
        Text = "Home";
        Name = "__HOME__"; // ShellForm이 이 이름으로 기존 홈 탭을 찾아서 재사용/보호함
        BackColor = PageBg;

        var scrollHost = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = PageBg };
        var content = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 24), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };

        var mainRow = BuildMainRow();
        var statsRow = BuildStatsRow();
        var greeting = BuildGreeting();

        // Dock=Top 스택은 나중에 추가한 컨트롤이 위쪽 우선권을 가진다 - 화면 순서(위->아래:
        // 인사말/요약카드/본문2단)의 역순으로 추가한다.
        content.Controls.Add(mainRow);
        content.Controls.Add(statsRow);
        content.Controls.Add(greeting);

        scrollHost.Controls.Add(content);
        Controls.Add(scrollHost);
    }

    private Panel BuildGreeting()
    {
        var panel = new Panel { Dock = DockStyle.Top, Height = 52 };

        var lblGreeting = new LabelControl { Text = "안녕하세요, 홍길동님", Dock = DockStyle.Top, Height = 26, AutoSizeMode = LabelAutoSizeMode.None };
        lblGreeting.Appearance.Font = AppFonts.SubHeading;
        lblGreeting.Appearance.ForeColor = TextPrimary;

        var lblDetail = new LabelControl { Text = "영업관리팀  ·  대리  ·  2026-08-24 (월) 09:12", Dock = DockStyle.Top, Height = 20, AutoSizeMode = LabelAutoSizeMode.None };
        lblDetail.Appearance.Font = AppFonts.Body;
        lblDetail.Appearance.ForeColor = TextSecondary;

        panel.Controls.Add(lblDetail);
        panel.Controls.Add(lblGreeting);
        return panel;
    }

    private FlowLayoutPanel BuildStatsRow()
    {
        var row = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 78,
            Margin = new Padding(0, 20, 0, 0),
            WrapContents = false,
            FlowDirection = FlowDirection.LeftToRight
        };

        row.Controls.Add(BuildStatCard("오늘 일정", "3건", TextPrimary));
        row.Controls.Add(BuildStatCard("결재 대기", "5건", DangerText));
        row.Controls.Add(BuildStatCard("읽지 않은 쪽지", "2건", TextPrimary));
        row.Controls.Add(BuildStatCard("공지사항", "1건", TextPrimary));
        return row;
    }

    private const int StatCardWidth = 168;

    private Panel BuildStatCard(string title, string value, Color valueColor)
    {
        var card = new Panel { Size = new Size(StatCardWidth, 66), Margin = new Padding(0, 0, 12, 0), BackColor = CardBg };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 8);

        var lblTitle = new LabelControl { Text = title, Location = new Point(16, 12), Size = new Size(StatCardWidth - 32, 18), AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Caption;
        lblTitle.Appearance.ForeColor = TextSecondary;

        var lblValue = new LabelControl { Text = value, Location = new Point(15, 32), Size = new Size(StatCardWidth - 32, 24), AutoSizeMode = LabelAutoSizeMode.None };
        lblValue.Appearance.Font = AppFonts.Heading;
        lblValue.Appearance.ForeColor = valueColor;

        card.Controls.Add(lblTitle);
        card.Controls.Add(lblValue);
        return card;
    }

    private Panel BuildMainRow()
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 400, Margin = new Padding(0, 16, 0, 0) };

        var rightCol = new Panel { Dock = DockStyle.Right, Width = 240 };
        var pendingCard = BuildPendingApprovalCard();
        var scheduleCard = BuildScheduleCard();
        rightCol.Controls.Add(pendingCard);
        rightCol.Controls.Add(scheduleCard);

        var leftGap = new Panel { Dock = DockStyle.Right, Width = 16 };

        var leftCol = new Panel { Dock = DockStyle.Fill };
        var quickMenuCard = BuildQuickMenuSection();
        var noticeCard = BuildNoticeCard();
        leftCol.Controls.Add(quickMenuCard);
        leftCol.Controls.Add(noticeCard);

        row.Controls.Add(leftCol);
        row.Controls.Add(leftGap);
        row.Controls.Add(rightCol);
        return row;
    }

    private Panel BuildNoticeCard()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = 190, Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var lblTitle = new LabelControl { Text = "공지사항", Dock = DockStyle.Top, Height = 24, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

        var list = new Panel { Dock = DockStyle.Fill };
        list.Controls.Add(BuildNoticeRow("여름 휴가 일정 취합", "08-14"));
        list.Controls.Add(BuildNoticeRow("기초코드등록 화면 개편 안내", "08-20"));
        list.Controls.Add(BuildNoticeRow("8월 정기 시스템 점검 안내", "08-22"));

        card.Controls.Add(list);
        card.Controls.Add(lblTitle);
        return card;
    }

    private Panel BuildNoticeRow(string title, string date)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 33 };
        var divider = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = CardBorder };

        var lblTitle = new LabelControl { Text = title, Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        var lblDate = new LabelControl { Text = date, Dock = DockStyle.Right, Width = 56, Padding = new Padding(0, 8, 0, 0), AutoSizeMode = LabelAutoSizeMode.None };
        lblDate.Appearance.Font = AppFonts.Caption;
        lblDate.Appearance.ForeColor = TextMuted;
        lblDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        row.Controls.Add(lblTitle);
        row.Controls.Add(lblDate);
        row.Controls.Add(divider);
        return row;
    }

    private Panel BuildQuickMenuSection()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = 110, Margin = new Padding(0, 0, 0, 16), Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var lblTitle = new LabelControl { Text = "빠른 메뉴", Dock = DockStyle.Top, Height = 24, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, FlowDirection = FlowDirection.LeftToRight };
        flow.Controls.Add(BuildQuickMenuTile("사용자관리"));
        flow.Controls.Add(BuildQuickMenuTile("메뉴관리"));
        flow.Controls.Add(BuildQuickMenuTile("결재함"));
        flow.Controls.Add(BuildQuickMenuTile("일정관리"));

        card.Controls.Add(flow);
        card.Controls.Add(lblTitle);
        return card;
    }

    private Panel BuildQuickMenuTile(string text)
    {
        var tile = new Panel { Size = new Size(94, 56), Margin = new Padding(0, 4, 8, 0) };
        tile.Paint += (s, e) => DrawCardBorder(tile, e.Graphics, CardBorder, 8);

        var lbl = new LabelControl { Text = text, Dock = DockStyle.Fill, AutoSizeMode = LabelAutoSizeMode.None };
        lbl.Appearance.Font = AppFonts.Caption;
        lbl.Appearance.ForeColor = TextPrimary;
        lbl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lbl.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

        tile.Controls.Add(lbl);
        return tile;
    }

    private Panel BuildScheduleCard()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = 130, Margin = new Padding(0, 0, 0, 16), Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var lblTitle = new LabelControl { Text = "오늘 일정", Dock = DockStyle.Top, Height = 24, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

        var list = new Panel { Dock = DockStyle.Fill };
        list.Controls.Add(BuildScheduleRow("17:00", "월간 보고서 제출"));
        list.Controls.Add(BuildScheduleRow("14:00", "고객사 미팅"));
        list.Controls.Add(BuildScheduleRow("10:00", "주간 영업회의"));

        card.Controls.Add(list);
        card.Controls.Add(lblTitle);
        return card;
    }

    private Panel BuildScheduleRow(string time, string title)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 26 };

        var lblTime = new LabelControl { Text = time, Dock = DockStyle.Left, Width = 44, AutoSizeMode = LabelAutoSizeMode.None };
        lblTime.Appearance.Font = AppFonts.Body;
        lblTime.Appearance.ForeColor = TextSecondary;

        var lblTitle = new LabelControl { Text = title, Dock = DockStyle.Fill, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        row.Controls.Add(lblTitle);
        row.Controls.Add(lblTime);
        return row;
    }

    private Panel BuildPendingApprovalCard()
    {
        var card = new Panel { Dock = DockStyle.Top, Height = 100, Padding = new Padding(16) };
        card.Paint += (s, e) => DrawCardBorder(card, e.Graphics, CardBorder, 10);

        var lblTitle = new LabelControl { Text = "결재 대기", Dock = DockStyle.Top, Height = 24, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = TextPrimary;

        var list = new Panel { Dock = DockStyle.Fill };
        list.Controls.Add(BuildApprovalRow("구매 요청서", "이지은"));
        list.Controls.Add(BuildApprovalRow("휴가 신청서", "박민수"));

        card.Controls.Add(list);
        card.Controls.Add(lblTitle);
        return card;
    }

    private Panel BuildApprovalRow(string title, string requester)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 26 };

        var lblTitle = new LabelControl { Text = title, Dock = DockStyle.Fill, AutoSizeMode = LabelAutoSizeMode.None };
        lblTitle.Appearance.Font = AppFonts.Body;
        lblTitle.Appearance.ForeColor = TextPrimary;

        var lblRequester = new LabelControl { Text = requester, Dock = DockStyle.Right, Width = 56, AutoSizeMode = LabelAutoSizeMode.None };
        lblRequester.Appearance.Font = AppFonts.Caption;
        lblRequester.Appearance.ForeColor = TextMuted;
        lblRequester.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

        row.Controls.Add(lblTitle);
        row.Controls.Add(lblRequester);
        return row;
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
