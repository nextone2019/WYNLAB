using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// 좌측 메뉴(AccordionControl) 최상위 항목용 단색 라인아이콘. ToolbarIconPainters와 같은 방식으로
/// 24x24 가상 좌표계에 그리고 실제 크기로 자동 스케일링한다. 메뉴는 다크 사이드바 배경 위에
/// 단색(흰색 계열) 하나로만 그려서 절제된 느낌을 준다 - 그래서 accent 파라미터는 쓰지 않는다.
/// </summary>
public static class MenuIconPainters
{
    public static void Settings(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawEllipse(pen, 7, 7, 10, 10);
        gg.DrawEllipse(pen, 10.3f, 10.3f, 3.4f, 3.4f);

        var center = new PointF(12, 12);
        for (var i = 0; i < 8; i++)
        {
            var angle = Math.PI / 4 * i;
            var inner = new PointF(center.X + (float)(9.5 * Math.Cos(angle)), center.Y + (float)(9.5 * Math.Sin(angle)));
            var outer = new PointF(center.X + (float)(11.5 * Math.Cos(angle)), center.Y + (float)(11.5 * Math.Sin(angle)));
            gg.DrawLine(pen, inner, outer);
        }
    });

    public static void Cart(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[]
        {
            new PointF(3.5f, 4), new PointF(6, 4), new PointF(9, 15), new PointF(19, 15), new PointF(21, 6.5f), new PointF(7.3f, 6.5f)
        });
        using var brush = new SolidBrush(color);
        gg.FillEllipse(brush, 9, 17, 3, 3);
        gg.FillEllipse(brush, 16.5f, 17, 3, 3);
    });

    public static void Tools(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.7f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLine(pen, 6, 18, 15, 9);

        // 렌치 머리 부분 - 열린 반원 두 개
        gg.DrawArc(pen, 3, 15, 6, 6, 30, 200);
        gg.DrawArc(pen, 14, 4, 6, 6, 210, 200);
    });

    public static void Folder(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[]
        {
            new PointF(3.5f, 7), new PointF(9, 7), new PointF(10.5f, 5), new PointF(15, 5),
            new PointF(15, 7.5f), new PointF(20.5f, 7.5f), new PointF(20.5f, 18), new PointF(3.5f, 18), new PointF(3.5f, 7)
        });
    });

    /// <summary>작은 채워진 점 - 메뉴트리에서 "그룹(폴더)"이 아니라 실제로 클릭 가능한 화면임을
    /// 표시하는 불릿 마커. 들여쓰기를 깊게 하지 않고도 그룹과 화면을 구분하기 위한 용도.</summary>
    public static void Dot(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var brush = new SolidBrush(color);
        gg.FillEllipse(brush, 9.5f, 9.5f, 5, 5);
    });

    /// <summary>채워진 5각 별 - 사이드바 "마이 메뉴(즐겨찾기)" 그룹 아이콘용(2026-09-21).</summary>
    public static void Star(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var brush = new SolidBrush(color);
        const float cx = 12f, cy = 12.5f, outerR = 9.5f, innerR = 4f;
        var pts = new PointF[10];
        for (var i = 0; i < 10; i++)
        {
            var angle = -Math.PI / 2 + i * Math.PI / 5;
            var r = i % 2 == 0 ? outerR : innerR;
            pts[i] = new PointF(cx + (float)(r * Math.Cos(angle)), cy + (float)(r * Math.Sin(angle)));
        }
        gg.FillPolygon(brush, pts);
    });

    /// <summary>메뉴(트리) 관리 화면 타이틀용 아이콘 - 세로 줄기에서 가지 3개가 뻗어나가는 형태로
    /// "계층 구조를 관리한다"는 의미를 폴더 아이콘보다 명확하게 전달한다.</summary>
    public static void MenuTree(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLine(pen, 5, 4, 5, 20);
        gg.DrawLine(pen, 5, 7, 10, 7);
        gg.DrawLine(pen, 5, 13, 10, 13);
        gg.DrawLine(pen, 5, 19, 10, 19);

        using var brush = new SolidBrush(color);
        gg.FillRectangle(brush, 10, 5.3f, 8, 3.4f);
        gg.FillRectangle(brush, 10, 11.3f, 8, 3.4f);
        gg.FillRectangle(brush, 10, 17.3f, 8, 3.4f);
    });

    /// <summary>24x24 가상 좌표계 -> 실제 rect 크기로 자동 스케일링해서 그려주는 헬퍼</summary>
    private static void DrawScaled(Graphics g, Rectangle rect, Action<Graphics> draw)
    {
        var state = g.Save();
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TranslateTransform(rect.X, rect.Y);
        var scale = Math.Min(rect.Width, rect.Height) / 24f;
        g.ScaleTransform(scale, scale);
        draw(g);
        g.Restore(state);
    }

    /// <summary>지정한 색으로 아이콘을 정사각형 비트맵에 래스터화 - AccordionControlElement.ImageOptions.Image에 바로 대입 가능</summary>
    public static Bitmap Render(Action<Graphics, Rectangle, Color> painter, int size, Color color)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        painter(g, new Rectangle(0, 0, size, size), color);
        return bmp;
    }
}
