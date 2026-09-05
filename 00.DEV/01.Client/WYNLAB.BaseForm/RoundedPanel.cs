using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// 둥근 모서리 배경을 가진 컨테이너 패널. 툴바 버튼 묶음처럼 "이 영역은 하나의 그룹"이라는
/// 시각적 카드 느낌을 주고 싶을 때 사용한다. 자식 컨트롤(IconBadgeButton 등)은 이 패널을
/// Parent로 두면, 자신의 배경 지우기(Parent.BackColor 참조)에서 이 패널의 색을 자동으로
/// 물려받아 이질감 없이 어울린다.
/// </summary>
public class RoundedPanel : Panel
{
    public int CornerRadius { get; set; } = 14;

    public RoundedPanel()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.SupportsTransparentBackColor, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Color.White);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRect(rect, CornerRadius);
        using var brush = new SolidBrush(BackColor);
        g.FillPath(brush, path);

        base.OnPaint(e);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        // 지름(d)이 실제 사각형의 폭/높이보다 크면 네 모서리 호가 서로 겹치거나 bounds
        // 바깥으로 넘어가서 찌그러진 모양이 그려진다 - 폭이 아주 좁은 사용처(사이드바
        // 세로 강조바 등, CornerRadius = 폭/2로 "완전한 캡슐"을 노린 경우)에서 실제로 겪었다.
        // 짧은 변 기준으로 지름을 clamp해서 항상 bounds 안에 들어가는 호만 그리게 한다.
        var d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
