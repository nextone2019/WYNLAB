using DevExpress.XtraEditors;
using System.Drawing;
using System.Windows.Forms;

namespace WYNLAB.UI.Common;

/// <summary>
/// 앱 전체 공통 메시지박스. DevExpress 스킨을 켜도 XtraMessageBox의 타이틀바는 여전히
/// 밋밋한 Windows 기본 스타일로 남아있어서(캡션 영역과 본문이 시각적으로 구분이 안 됨),
/// LoginForm과 같은 방식(FormBorderStyle=None + 직접 그린 컬러 헤더)으로 아예 새로 만들었다.
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
    private readonly PanelControl headerPanel = new();
    private readonly LabelControl lblCaption = new();
    private readonly LabelControl lblClose = new() { Text = "✕" };
    private readonly Panel badge = new();
    private readonly LabelControl lblBadgeGlyph = new();
    private readonly LabelControl lblMessage = new();
    private readonly Panel footerPanel = new() { Dock = DockStyle.Bottom, Height = 60 };

    private Point _dragStart;
    private bool _dragging;

    public AppMessageBoxForm(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        Width = 420;
        ShowInTaskbar = false;

        var (accentColor, glyph) = GetIconStyle(icon);

        BuildHeader(caption, accentColor);
        BuildBody(text, accentColor, glyph, icon);
        BuildFooter(buttons, accentColor);

        ResizeToFitMessage();
    }

    private static (Color accent, string glyph) GetIconStyle(MessageBoxIcon icon) => icon switch
    {
        MessageBoxIcon.Error => (Color.FromArgb(192, 57, 43), "!"),
        MessageBoxIcon.Warning => (Color.FromArgb(217, 141, 39), "!"),
        MessageBoxIcon.Question => (Color.FromArgb(41, 121, 255), "?"),
        MessageBoxIcon.Information => (Color.FromArgb(41, 121, 255), "i"),
        _ => (Color.FromArgb(90, 96, 105), "")
    };

    private void BuildHeader(string caption, Color accentColor)
    {
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 44;
        headerPanel.Appearance.BackColor = accentColor;
        headerPanel.Appearance.Options.UseBackColor = true;
        headerPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        lblCaption.Text = caption;
        lblCaption.Location = new Point(18, 12);
        lblCaption.AutoSizeMode = LabelAutoSizeMode.None;
        lblCaption.Size = new Size(340, 22);
        lblCaption.Appearance.ForeColor = Color.White;
        lblCaption.Appearance.Font = AppFonts.BodyBold;

        lblClose.Location = new Point(Width - 38, 10);
        lblClose.AutoSizeMode = LabelAutoSizeMode.None;
        lblClose.Size = new Size(24, 24);
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

        Controls.Add(headerPanel);
    }

    private void BuildBody(string text, Color accentColor, string glyph, MessageBoxIcon icon)
    {
        if (icon != MessageBoxIcon.None)
        {
            badge.Size = new Size(40, 40);
            badge.Location = new Point(20, 26);
            badge.BackColor = accentColor;
            lblBadgeGlyph.Text = glyph;
            lblBadgeGlyph.Dock = DockStyle.Fill;
            lblBadgeGlyph.AutoSizeMode = LabelAutoSizeMode.None;
            lblBadgeGlyph.Appearance.ForeColor = Color.White;
            lblBadgeGlyph.Appearance.Font = AppFonts.Heading;
            lblBadgeGlyph.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            lblBadgeGlyph.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            badge.Controls.Add(lblBadgeGlyph);
            Controls.Add(badge);
        }

        var textX = icon != MessageBoxIcon.None ? 74 : 20;
        lblMessage.Text = text;
        lblMessage.Location = new Point(textX, 26);
        lblMessage.AutoSizeMode = LabelAutoSizeMode.None;
        lblMessage.Size = new Size(Width - textX - 20, 20);
        lblMessage.Appearance.ForeColor = Color.FromArgb(55, 55, 55);
        lblMessage.Appearance.Font = AppFonts.Body;
        lblMessage.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        Controls.Add(lblMessage);
    }

    private void BuildFooter(MessageBoxButtons buttons, Color accentColor)
    {
        footerPanel.BackColor = Color.FromArgb(248, 249, 250);
        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 231, 234) };
        footerPanel.Controls.Add(topBorder);

        var specs = GetButtonSpecs(buttons);
        var x = Width - 16;
        SimpleButton? firstButton = null;

        for (var i = specs.Length - 1; i >= 0; i--)
        {
            var (label, result, primary) = specs[i];
            var btn = new SimpleButton { Text = label, Size = new Size(88, 34), DialogResult = result };
            if (primary)
            {
                btn.Appearance.BackColor = accentColor;
                btn.Appearance.ForeColor = Color.White;
                btn.Appearance.Options.UseBackColor = true;
                btn.Appearance.Options.UseForeColor = true;
            }
            btn.Appearance.Font = AppFonts.BodyBold;
            x -= btn.Width;
            btn.Location = new Point(x, 13);
            x -= 8;
            footerPanel.Controls.Add(btn);
            firstButton ??= btn; // 가장 오른쪽(주 버튼)부터 역순으로 만들어지므로 첫 생성 = AcceptButton 후보
        }

        if (firstButton != null) AcceptButton = firstButton;
        CancelButton = new SimpleButton { DialogResult = DialogResult.Cancel, Visible = false };
        Controls.Add(footerPanel);
    }

    /// <summary>(버튼텍스트, DialogResult, 강조여부) - 배열의 마지막 항목이 화면상 가장 오른쪽(주 액션)</summary>
    private static (string, DialogResult, bool)[] GetButtonSpecs(MessageBoxButtons buttons) => buttons switch
    {
        MessageBoxButtons.OKCancel => new[] { ("취소", DialogResult.Cancel, false), ("확인", DialogResult.OK, true) },
        MessageBoxButtons.YesNo => new[] { ("아니요", DialogResult.No, false), ("예", DialogResult.Yes, true) },
        MessageBoxButtons.YesNoCancel => new[] { ("취소", DialogResult.Cancel, false), ("아니요", DialogResult.No, false), ("예", DialogResult.Yes, true) },
        _ => new[] { ("확인", DialogResult.OK, true) }
    };

    private void ResizeToFitMessage()
    {
        var textX = badge.Parent != null ? 74 : 20;
        var maxWidth = Width - textX - 20;
        var measured = TextRenderer.MeasureText(lblMessage.Text, new Font("Segoe UI", 9.5f),
            new Size(maxWidth, 0), TextFormatFlags.WordBreak);

        var textHeight = Math.Max(20, measured.Height);
        lblMessage.Size = new Size(maxWidth, textHeight);

        var bodyHeight = Math.Max(textHeight, 40) + 52; // 위아래 여백 포함
        Height = headerPanel.Height + bodyHeight + footerPanel.Height;
    }

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
}
