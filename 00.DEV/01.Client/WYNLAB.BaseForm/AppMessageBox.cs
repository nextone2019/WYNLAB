using DevExpress.XtraEditors;
using System.Drawing;
using System.Windows.Forms;

namespace WYNLAB.Base;

/// <summary>
/// 앱 전체 공통 메시지박스. DevExpress 스킨을 켜도 XtraMessageBox의 타이틀바는 여전히
/// 밋밋한 Windows 기본 스타일로 남아있어서(캡션 영역과 본문이 시각적으로 구분이 안 됨),
/// LoginForm과 같은 방식(FormBorderStyle=None + 직접 그린 컬러 헤더)으로 아예 새로 만들었다.
/// 헤더 배경색은 아이콘 종류와 무관하게 항상 AppConfig.ToolbarColor(=상단 툴바와 같은 브랜드색)를
/// 쓴다 - 나중에 툴바 색이 회사별로 바뀌면 메시지박스 헤더도 같이 따라가도록 하기 위함.
/// 아이콘 종류(정보/경고/오류/질문)는 헤더 안의 작은 원형 배지로만 구분해서 표시한다.
/// 기존 XtraMessageBox.Show(...)와 같은 시그니처를 제공하므로 호출부만 바꿔치기하면 된다.
/// </summary>
public static class AppMessageBox
{
    public static DialogResult Show(string text, string caption)
        => Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        => Show(text, caption, buttons, MessageBoxIcon.Information);

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        // 사용자의 답을 기다리는 동안은 "작업 중"이 아니므로 busy 오버레이를 걷는다 -
        // 안 그러면 확인창 뒤 화면이 통째로 회색으로 덮인다(BaseForm.SuspendBusyForModal 참고).
        BaseForm.SuspendBusyForModal();
        try
        {
            using var form = new AppMessageBoxForm(text, caption, buttons, icon);
            return form.ShowDialog();
        }
        finally
        {
            BaseForm.ResumeBusyAfterModal();
        }
    }
}

internal class AppMessageBoxForm : XtraForm
{
    private const int HeaderHeight = 36;
    private const int FooterHeight = 46;
    private const int BodyHeight = 72;

    // 본문/버튼 영역을 순수 흰색(255,255,255)이 아니라 아주 살짝 톤 다운된 색으로 통일한다 -
    // 이 앱의 화면 대부분이 흰 배경이라, 메시지박스도 순백이면 뒤 화면과 거의 구분이 안 돼서
    // 대화상자가 떠 있다는 느낌이 약했다(실제 피드백). 예전엔 body=흰색/footer=회색으로
    // 서로 달라서 그 자체도 이질감이 있었는데, 이제 하나로 합쳤다.
    private static readonly Color SurfaceColor = Color.FromArgb(250, 250, 251);
    private static readonly Color BorderColor = Color.FromArgb(170, 174, 182);

    private readonly PanelControl headerPanel = new();
    private readonly CircleBadge headerBadge = new();
    private readonly LabelControl lblHeaderGlyph = new();
    private readonly LabelControl lblCaption = new();
    private readonly CloseGlyph btnClose = new();
    private readonly Panel bodyPanel = new() { Dock = DockStyle.Fill, BackColor = SurfaceColor };
    private readonly LabelControl lblMessage = new();
    private readonly Panel footerPanel = new() { Dock = DockStyle.Bottom, Height = FooterHeight };

    private Point _dragStart;
    private bool _dragging;

    public AppMessageBoxForm(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;

        // 테두리를 그리는 대신, 폼 배경을 테두리색으로 칠하고 자식 패널들을 1px 안쪽으로
        // 밀어넣는다. 그러면 사방에 남는 1px 띠가 곧 테두리가 된다.
        //
        // 처음엔 OnPaint에서 DrawRectangle로 그렸는데 두 번 실패했다. (1) Padding이 없으면
        // bodyPanel(Dock=Fill)/header(Top)/footer(Bottom)가 폼 영역을 한 픽셀도 남기지 않고
        // 덮어서 테두리가 그 아래 깔려 아예 안 보였고, (2) Padding을 준 뒤에도 그 1px 띠는
        // 폼이 다시 그려질 때만 갱신되는 영역이라 자식이 갱신될 때 남은 자국이 보였다.
        // 배경색으로 처리하면 그릴 필요 자체가 없어져서 두 문제 다 생기지 않는다.
        BackColor = BorderColor;
        Padding = new Padding(1);
        Width = 360 + Padding.Horizontal;
        Height = HeaderHeight + BodyHeight + FooterHeight + Padding.Vertical;
        ShowInTaskbar = false;

        var brandColor = ColorHelper.FromHex(AppConfig.ToolbarColor);
        var (badgeColor, glyph) = GetIconStyle(icon);

        BuildBody(text);
        BuildFooter(buttons);
        BuildHeader(caption, brandColor, badgeColor, glyph, icon);

        // Dock 추가 순서 중요(이 프로젝트 전반의 규칙): Fill(bodyPanel)을 먼저 추가하고
        // Top/Bottom은 나중에 추가해야 각자 자기 가장자리를 정상적으로 차지한다.
        Controls.Add(bodyPanel);
        Controls.Add(footerPanel);
        Controls.Add(headerPanel);
    }

    private static (Color badge, string glyph) GetIconStyle(MessageBoxIcon icon) => icon switch
    {
        MessageBoxIcon.Error => (Color.FromArgb(224, 92, 80), "!"),
        MessageBoxIcon.Warning => (Color.FromArgb(232, 170, 76), "!"),
        MessageBoxIcon.Question => (Color.FromArgb(94, 158, 255), "?"),
        MessageBoxIcon.Information => (Color.FromArgb(94, 158, 255), "i"),
        _ => (Color.Transparent, "")
    };

    private void BuildHeader(string caption, Color brandColor, Color badgeColor, string glyph, MessageBoxIcon icon)
    {
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = HeaderHeight;
        headerPanel.Appearance.BackColor = brandColor;
        headerPanel.Appearance.Options.UseBackColor = true;
        headerPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        var textX = 14;
        if (icon != MessageBoxIcon.None)
        {
            headerBadge.Size = new Size(18, 18);
            headerBadge.Location = new Point(12, 9);
            headerBadge.BackColor = badgeColor;
            headerBadge.HeaderColor = brandColor;
            lblHeaderGlyph.Text = glyph;
            lblHeaderGlyph.Dock = DockStyle.Fill;
            lblHeaderGlyph.AutoSizeMode = LabelAutoSizeMode.None;
            lblHeaderGlyph.Appearance.ForeColor = Color.White;
            lblHeaderGlyph.Appearance.Font = AppFonts.BodyBold;
            lblHeaderGlyph.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            lblHeaderGlyph.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            headerBadge.Controls.Add(lblHeaderGlyph);
            headerPanel.Controls.Add(headerBadge);
            textX = 36;
        }

        lblCaption.Text = caption;
        lblCaption.Location = new Point(textX, 9);
        lblCaption.AutoSizeMode = LabelAutoSizeMode.None;
        lblCaption.Appearance.ForeColor = Color.White;
        lblCaption.Appearance.Font = AppFonts.BodyBold;

        btnClose.Size = new Size(24, 24);
        btnClose.HeaderColor = brandColor;
        btnClose.Cursor = Cursors.Hand;
        btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        headerPanel.Controls.Add(lblCaption);
        headerPanel.Controls.Add(btnClose);

        // 캡션/닫기 위치는 headerPanel의 "실제" 폭이 정해진 뒤에 잡는다 - 생성자 시점의 Width는
        // 아직 폼 기준이라 Padding만큼 어긋난다.
        headerPanel.Resize += (s, e) => LayoutHeader(textX);
        LayoutHeader(textX);

        EnableDrag(headerPanel);
        EnableDrag(lblCaption);
    }

    private void LayoutHeader(int textX)
    {
        var w = headerPanel.ClientSize.Width;
        if (w <= 0) return;

        btnClose.Location = new Point(w - btnClose.Width - 8, (HeaderHeight - btnClose.Height) / 2);
        lblCaption.Size = new Size(Math.Max(0, btnClose.Left - textX - 8), 18);
    }

    private void BuildBody(string text)
    {
        lblMessage.Text = text;
        // 본문은 bodyPanel(Dock=Fill)에 꽉 채우고 가운데 정렬 - 좌우 20px만 남긴다.
        // 폼 Width가 아니라 Dock으로 잡아야 Padding이 붙어도 어긋나지 않는다.
        lblMessage.Dock = DockStyle.Fill;
        lblMessage.Padding = new Padding(20, 0, 20, 0);
        lblMessage.AutoSizeMode = LabelAutoSizeMode.None;
        lblMessage.Appearance.ForeColor = Color.FromArgb(55, 55, 55);
        lblMessage.Appearance.Font = AppFonts.Body;
        lblMessage.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        lblMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblMessage.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        bodyPanel.Controls.Add(lblMessage);
    }

    /// <summary>참고 화면(캡처)처럼 버튼을 전부 같은 톤(강조색 없음)으로, 가운데 정렬한다.</summary>
    private void BuildFooter(MessageBoxButtons buttons)
    {
        footerPanel.BackColor = SurfaceColor;
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = BorderColor };
        footerPanel.Controls.Add(topBorder);

        var specs = GetButtonSpecs(buttons);
        const int btnWidth = 76;
        const int btnHeight = 28;
        const int btnGap = 10;
        var y = (FooterHeight - 1 - btnHeight) / 2 + 1;

        var madeButtons = new List<SimpleButton>();
        foreach (var (label, result) in specs)
        {
            var btn = new SimpleButton { Text = label, Size = new Size(btnWidth, btnHeight), DialogResult = result };
            btn.Appearance.Font = AppFonts.Body;
            btn.Top = y;
            footerPanel.Controls.Add(btn);
            madeButtons.Add(btn);
        }

        // 가로 위치는 footerPanel의 실제 폭 기준으로 잡는다(헤더와 같은 이유 - LayoutHeader 주석 참고).
        void LayoutButtons()
        {
            var totalWidth = madeButtons.Count * btnWidth + (madeButtons.Count - 1) * btnGap;
            var x = (footerPanel.ClientSize.Width - totalWidth) / 2;
            foreach (var btn in madeButtons)
            {
                btn.Left = x;
                x += btnWidth + btnGap;
            }
        }
        footerPanel.Resize += (s, e) => LayoutButtons();
        LayoutButtons();

        if (madeButtons.Count > 0) AcceptButton = madeButtons[0];
        CancelButton = new SimpleButton { DialogResult = DialogResult.Cancel, Visible = false };
    }

    /// <summary>(버튼텍스트, DialogResult) - 캡처 참고화면처럼 강조버튼 구분 없이 전부 동일한 스타일</summary>
    private static (string, DialogResult)[] GetButtonSpecs(MessageBoxButtons buttons) => buttons switch
    {
        MessageBoxButtons.OKCancel => new[] { ("확인", DialogResult.OK), ("취소", DialogResult.Cancel) },
        MessageBoxButtons.YesNo => new[] { ("Yes", DialogResult.Yes), ("No", DialogResult.No) },
        MessageBoxButtons.YesNoCancel => new[] { ("Yes", DialogResult.Yes), ("No", DialogResult.No), ("취소", DialogResult.Cancel) },
        _ => new[] { ("확인", DialogResult.OK) }
    };

    private void EnableDrag(Control control)
    {
        control.MouseDown += (s, e) => { _dragging = true; _dragStart = e.Location; };
        control.MouseMove += (s, e) =>
        {
            if (!_dragging) return;
            Location = new Point(Location.X + e.X - _dragStart.X, Location.Y + e.Y - _dragStart.Y);
        };
        control.MouseUp += (s, e) => { _dragging = false; };
    }

    /// <summary>
    /// 헤더 오른쪽 닫기(X) 버튼. 예전엔 LabelControl에 "✕"(U+2715) 문자를 넣었는데, 맑은 고딕에
    /// 이 글자가 없어서 윈도우가 다른 폰트로 대체해 그리는 바람에 크기와 위치가 어긋나 잘린 것처럼
    /// 보였다(실제로 겪음). 폰트에 의존하지 않도록 선 두 개로 직접 그린다 - 어떤 PC에서든 같게 나온다.
    /// </summary>
    private class CloseGlyph : Control
    {
        private bool _hover;

        /// <summary>헤더 배경색. 부모(headerPanel)에서 알아낼 수 없어서 직접 받아야 한다 -
        /// headerPanel은 DevExpress PanelControl이라 실제로 보이는 색은 Appearance.BackColor에
        /// 있고, Control.BackColor는 손대지 않은 기본 회색을 돌려준다. 그 회색으로 배경을
        /// 지우는 바람에 헤더 오른쪽에 밝은 사각형 자국이 남았다(실제로 겪음).</summary>
        public Color HeaderColor { get; set; } = Color.Transparent;

        public CloseGlyph()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                      ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(HeaderColor);

            // 마우스를 올리면 살짝 밝은 사각형을 깔아 눌러지는 버튼임을 알린다(윈도우 창 닫기 버튼과 같은 감각).
            if (_hover)
            {
                using var hoverBrush = new SolidBrush(Color.FromArgb(38, 255, 255, 255));
                g.FillRectangle(hoverBrush, 0, 0, Width, Height);
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            const int inset = 8; // X가 버튼 한가운데에 적당한 크기로 오도록 하는 여백
            using var pen = new Pen(Color.FromArgb(_hover ? 255 : 210, 255, 255, 255), 1.4f);
            g.DrawLine(pen, inset, inset, Width - inset - 1, Height - inset - 1);
            g.DrawLine(pen, Width - inset - 1, inset, inset, Height - inset - 1);
        }
    }

    /// <summary>헤더 안의 작은 원형 아이콘 배지 - 사각 Panel로는 참고화면의 동그란 느낌이 안 나서 직접 그림</summary>
    private class CircleBadge : Panel
    {
        /// <summary>헤더 배경색 - 직접 받아야 하는 이유는 CloseGlyph.HeaderColor 주석 참고.</summary>
        public Color HeaderColor { get; set; } = Color.Transparent;

        public CircleBadge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(HeaderColor);
            using var brush = new SolidBrush(BackColor);
            g.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            base.OnPaint(e);
        }
    }
}
