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
        using var form = new AppMessageBoxForm(text, caption, buttons, icon);
        return form.ShowDialog();
    }
}

internal class AppMessageBoxForm : XtraForm
{
    private const int HeaderHeight = 36;
    private const int FooterHeight = 46;
    private const int BodyHeight = 72;

    private readonly PanelControl headerPanel = new();
    private readonly CircleBadge headerBadge = new();
    private readonly LabelControl lblHeaderGlyph = new();
    private readonly LabelControl lblCaption = new();
    private readonly LabelControl lblClose = new() { Text = "✕" };
    private readonly Panel bodyPanel = new() { Dock = DockStyle.Fill, BackColor = Color.White };
    private readonly LabelControl lblMessage = new();
    private readonly Panel footerPanel = new() { Dock = DockStyle.Bottom, Height = FooterHeight };

    private Point _dragStart;
    private bool _dragging;

    public AppMessageBoxForm(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        Width = 360;
        Height = HeaderHeight + BodyHeight + FooterHeight;
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

    /// <summary>
    /// 다이얼로그 전체 테두리를 얇게 그려서 하나의 박스로 보이게 한다. 이게 없으면 헤더/본문/
    /// 푸터가 각자 다른 배경색 패널일 뿐이라 경계가 흐려지고, 특히 푸터(거의 흰색)가 바탕화면과
    /// 구분이 잘 안 돼서 버튼이 메시지박스 밖에 붕 떠 있는 것처럼 보이는 문제가 있었다.
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(210, 212, 217));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
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
        lblCaption.Size = new Size(Width - textX - 34, 18);
        lblCaption.Appearance.ForeColor = Color.White;
        lblCaption.Appearance.Font = AppFonts.BodyBold;

        lblClose.Location = new Point(Width - 32, 8);
        lblClose.AutoSizeMode = LabelAutoSizeMode.None;
        lblClose.Size = new Size(20, 20);
        lblClose.Appearance.ForeColor = Color.FromArgb(230, 255, 255, 255);
        lblClose.Appearance.Font = AppFonts.Caption;
        lblClose.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblClose.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        lblClose.Cursor = Cursors.Hand;
        lblClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        headerPanel.Controls.Add(lblCaption);
        headerPanel.Controls.Add(lblClose);

        EnableDrag(headerPanel);
        EnableDrag(lblCaption);
    }

    private void BuildBody(string text)
    {
        lblMessage.Text = text;
        lblMessage.Location = new Point(20, 0);
        lblMessage.AutoSizeMode = LabelAutoSizeMode.None;
        lblMessage.Size = new Size(Width - 40, BodyHeight);
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
        footerPanel.BackColor = Color.FromArgb(248, 249, 250);
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 231, 234) };
        footerPanel.Controls.Add(topBorder);

        var specs = GetButtonSpecs(buttons);
        const int btnWidth = 76;
        const int btnHeight = 28;
        const int btnGap = 10;
        var totalWidth = specs.Length * btnWidth + (specs.Length - 1) * btnGap;
        var x = (Width - totalWidth) / 2;
        var y = (FooterHeight - 1 - btnHeight) / 2 + 1;

        SimpleButton? firstButton = null;
        foreach (var (label, result) in specs)
        {
            var btn = new SimpleButton { Text = label, Size = new Size(btnWidth, btnHeight), DialogResult = result };
            btn.Appearance.Font = AppFonts.Body;
            btn.Location = new Point(x, y);
            footerPanel.Controls.Add(btn);
            firstButton ??= btn;
            x += btnWidth + btnGap;
        }

        if (firstButton != null) AcceptButton = firstButton;
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

    /// <summary>헤더 안의 작은 원형 아이콘 배지 - 사각 Panel로는 참고화면의 동그란 느낌이 안 나서 직접 그림</summary>
    private class CircleBadge : Panel
    {
        public CircleBadge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? Color.Transparent);
            using var brush = new SolidBrush(BackColor);
            g.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            base.OnPaint(e);
        }
    }
}
