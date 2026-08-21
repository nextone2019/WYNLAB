using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// MDI 상단 툴바용 카드형 아이콘 버튼. 둥근 사각 배지 배경 안에 아이콘만 직접 그린다
/// (아래 텍스트 캡션은 없음 - 대신 Text 속성값을 툴팁으로 노출해서 필요할 때만 보이게 함).
/// DevExpress SimpleButton은 이런 "배지형" 룩을 낼 수 없어서 Control을 상속받아
/// OnPaint에서 전부 직접 그리는 방식으로 구현했다.
///
/// 상태 표현(Normal/Hover/Pressed/Disabled)은 배지 색과 무관하게 항상 같은 세기로 보이도록
/// 고정 알파의 검정 오버레이를 얹는 방식으로 통일했다 - 배지 자체 색(중립회색/파랑틴트/
/// 강조색 채움 등)마다 밝기가 다 달라서, 그 색 기준 상대적으로 어둡게 하는 방식(예전 버전의
/// ControlPaint.Dark(4%))으로는 버튼마다 호버 느낌의 세기가 제각각으로 보였다.
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
    private static readonly Color DisabledBadgeColor = Color.FromArgb(240, 241, 243);
    private static readonly Color DisabledIconColor = Color.FromArgb(195, 197, 201);

    /// <summary>실제 아이콘 모양을 그리는 델리게이트 - ToolbarIconPainters의 정적 메서드를 그대로 연결</summary>
    public Action<Graphics, Rectangle, Color, Color>? IconPainter { get; set; }

    /// <summary>
    /// 배지 안에서 아이콘이 차지할 여백. 기본값 9는 헤더 툴바 버튼(54x48) 기준으로 잡은 값이라,
    /// 그보다 훨씬 작게 쓰는 버튼(예: 사이드바의 작은 홈 버튼)에서 그대로 두면 아이콘이 몇
    /// 픽셀짜리 점처럼 보여 거의 안 보이게 된다 - 그런 경우 이 값을 줄여서 호출한다.
    /// </summary>
    public int IconInset { get; set; } = 9;

    /// <summary>배지(둥근 사각형) 배경색 - 옅은 파스텔톤 추천</summary>
    public Color BadgeColor { get; set; } = Color.FromArgb(241, 243, 245);

    /// <summary>아이콘 윤곽선 색</summary>
    public Color OutlineColor { get; set; } = Color.FromArgb(55, 55, 55);

    /// <summary>아이콘 포인트(강조) 색</summary>
    public Color AccentColor { get; set; } = Color.FromArgb(41, 121, 255);

    /// <summary>true면 배지를 진하게 채우고 아이콘은 흰색으로 그림 (저장처럼 강조하고 싶은 버튼용)</summary>
    public bool FilledBadge { get; set; }

    private bool _hover;
    private bool _pressed;

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
        _pressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _pressed = true;
            Invalidate();
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        _pressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        _hover = false;
        _pressed = false;
        Invalidate();
        base.OnEnabledChanged(e);
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

        using var path = RoundedRect(badgeRect, 10);

        var badgeColor = Enabled ? BadgeColor : DisabledBadgeColor;
        using (var brush = new SolidBrush(badgeColor))
        {
            g.FillPath(brush, path);
        }

        if (Enabled && _pressed)
        {
            using var overlay = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
            g.FillPath(overlay, path);
        }
        else if (Enabled && _hover)
        {
            using var overlay = new SolidBrush(Color.FromArgb(14, 0, 0, 0));
            g.FillPath(overlay, path);
        }

        var iconOutline = !Enabled ? DisabledIconColor : (FilledBadge ? Color.White : OutlineColor);
        var iconAccent = !Enabled ? DisabledIconColor : (FilledBadge ? Color.White : AccentColor);
        var iconRect = Rectangle.Inflate(badgeRect, -IconInset, -IconInset);
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
