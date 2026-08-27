using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// 스플래시/로딩 화면에서 쓰는 회전 스피너. 외부 gif/이미지 없이 직접 그린다(이 프로젝트
/// 전반의 방침 - IconBadgeButton, MenuIconPainters 등과 같은 방식). 타이머로 각도를 돌리며
/// 짧은 호(arc) 하나를 계속 다시 그린다.
/// </summary>
public class SpinnerControl : Control
{
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 25 };
    private float _angle;

    public Color SpinnerColor { get; set; } = Color.FromArgb(41, 121, 255);
    public float StrokeWidth { get; set; } = 3f;

    public SpinnerControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Size = new Size(32, 32);
        _timer.Tick += (s, e) => { _angle = (_angle + 9f) % 360f; Invalidate(); };
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        _timer.Start();
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        _timer.Stop();
        base.OnHandleDestroyed(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Color.White);

        var rect = Rectangle.Inflate(ClientRectangle, -(int)StrokeWidth, -(int)StrokeWidth);
        if (rect.Width <= 0 || rect.Height <= 0) return;

        // 옅은 배경 원 위에 짙은 호 하나를 얹어서, 도는 중임이 멈춰있을 때도 자연스럽게 보이게 함
        using (var trackPen = new Pen(ColorHelper.Adjust(SpinnerColor, 150), StrokeWidth) { StartCap = LineCap.Round, EndCap = LineCap.Round })
        {
            g.DrawEllipse(trackPen, rect);
        }
        using (var pen = new Pen(SpinnerColor, StrokeWidth) { StartCap = LineCap.Round, EndCap = LineCap.Round })
        {
            g.DrawArc(pen, rect, _angle, 100);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
