using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.UI.Common;

/// <summary>
/// MDI 상단 툴바용 카드형 아이콘 버튼. 둥근 사각 배지 배경 안에 아이콘만 직접 그린다
/// (아래 텍스트 캡션은 없음 - 대신 Text 속성값을 툴팁으로 노출해서 필요할 때만 보이게 함).
/// DevExpress SimpleButton은 이런 "배지형" 룩을 낼 수 없어서 Control을 상속받아
/// OnPaint에서 전부 직접 그리는 방식으로 구현했다.
///
/// 사용 예:
///   var btn = new IconBadgeButton
///   {
///       Text = "저장",   // 툴팁 문구로 쓰임
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
                  ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.SupportsTransparentBackColor, true);
        Cursor = Cursors.Hand;
        Size = new Size(52, 48);
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
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        g.Clear(Parent?.BackColor ?? Color.White);

        var badgeSize = Math.Min(Math.Min(Width, Height) - 4, 44);
        var badgeRect = new Rectangle((Width - badgeSize) / 2, (Height - badgeSize) / 2, badgeSize, badgeSize);

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
