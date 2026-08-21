using System.Drawing;
using System.Drawing.Drawing2D;

namespace NEXTFramework.UI.Common;

/// <summary>
/// MDI 상단 툴바용 카드형 아이콘 버튼. 둥근 사각 배지 배경 안에 아이콘을 직접 그리고,
/// 그 아래 텍스트를 표시한다. DevExpress SimpleButton은 이런 "배지형" 룩을 낼 수 없어서
/// Control을 상속받아 OnPaint에서 전부 직접 그리는 방식으로 구현했다.
///
/// 사용 예:
///   var btn = new IconBadgeButton
///   {
///       Text = "저장",
///       IconPainter = ToolbarIconPainters.Save,
///       BadgeColor = Color.FromArgb(41, 121, 255),
///       FilledBadge = true   // 배지를 꽉 채우고 아이콘은 흰색으로 (강조용)
///   };
///   btn.Click += ...;
/// </summary>
public class IconBadgeButton : Control
{
    /// <summary>실제 아이콘 모양을 그리는 델리게이트 - ToolbarIconPainters의 정적 메서드를 그대로 연결</summary>
    public Action<Graphics, Rectangle, Color, Color>? IconPainter { get; set; }

    /// <summary>배지(둥근 사각형) 배경색 - 옅은 파스텔톤 추천</summary>
    public Color BadgeColor { get; set; } = Color.FromArgb(241, 243, 245);

    /// <summary>아이콘 윤곽선 색</summary>
    public Color OutlineColor { get; set; } = Color.FromArgb(55, 55, 55);

    /// <summary>아이콘 포인트(강조) 색</summary>
    public Color AccentColor { get; set; } = Color.FromArgb(41, 121, 255);

    /// <summary>true면 배지를 진하게 채우고 아이콘은 흰색으로 그림 (저장처럼 강조하고 싶은 버튼용)</summary>
    public bool FilledBadge { get; set; }

    private bool _hover;

    public IconBadgeButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        Cursor = Cursors.Hand;
        Size = new Size(66, 58);
        BackColor = Color.Transparent;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Color.White);

        var badgeSize = Math.Min(Width - 8, 40);
        var badgeRect = new Rectangle((Width - badgeSize) / 2, 2, badgeSize, badgeSize);

        var bg = _hover ? ControlPaint.Dark(BadgeColor, 0.04f) : BadgeColor;
        using (var path = RoundedRect(badgeRect, 10))
        using (var brush = new SolidBrush(bg))
        {
            g.FillPath(brush, path);
        }

        var iconOutline = FilledBadge ? Color.White : OutlineColor;
        var iconAccent = FilledBadge ? Color.White : AccentColor;
        var iconRect = Rectangle.Inflate(badgeRect, -9, -9);
        IconPainter?.Invoke(g, iconRect, iconOutline, iconAccent);

        var textRect = new Rectangle(0, badgeRect.Bottom + 3, Width, 16);
        using var textBrush = new SolidBrush(Color.FromArgb(75, 75, 75));
        using var font = new Font("Segoe UI", 7.7f);
        var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(Text, font, textBrush, textRect, format);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        var d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
