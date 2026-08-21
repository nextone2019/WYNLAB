using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// IconBadgeButton 안에 그려질 아이콘 모양을 정의. 24x24 가상 좌표계 기준으로 그리고,
/// 실제 버튼 크기에 맞춰 자동으로 스케일링된다. outline은 기본 윤곽선 색, accent는 포인트 색.
/// </summary>
public static class ToolbarIconPainters
{
    public static void Home(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(outline, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[] { new PointF(4, 11), new PointF(12, 4), new PointF(20, 11) });
        gg.DrawLines(pen, new[]
        {
            new PointF(6, 10), new PointF(6, 20), new PointF(11, 20), new PointF(11, 14),
            new PointF(13, 14), new PointF(13, 20), new PointF(18, 20), new PointF(18, 10)
        });
        using var accentBrush = new SolidBrush(accent);
        gg.FillRectangle(accentBrush, 14.5f, 5f, 2.4f, 4f);
    });

    public static void Query(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(outline, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        gg.DrawEllipse(pen, 4, 4, 12, 12);
        using var accentPen = new Pen(accent, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        gg.DrawLine(accentPen, 14.3f, 14.3f, 20.5f, 20.5f);
    });

    public static void New(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(outline, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[] { new PointF(5, 3), new PointF(14, 3), new PointF(18, 7), new PointF(18, 17), new PointF(5, 17), new PointF(5, 3) });
        gg.DrawLines(pen, new[] { new PointF(14, 3), new PointF(14, 7), new PointF(18, 7) });
        gg.DrawLine(pen, 8, 13, 13, 13);

        using var accentBrush = new SolidBrush(accent);
        gg.FillEllipse(accentBrush, 13.5f, 13.5f, 8, 8);
        using var whitePen = new Pen(Color.White, 1.4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        gg.DrawLine(whitePen, 17.5f, 15.7f, 17.5f, 19.3f);
        gg.DrawLine(whitePen, 15.7f, 17.5f, 19.3f, 17.5f);
    });

    public static void Save(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(outline, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[] { new PointF(4, 4), new PointF(16, 4), new PointF(20, 8), new PointF(20, 20), new PointF(4, 20), new PointF(4, 4) });
        gg.DrawLines(pen, new[] { new PointF(7, 4), new PointF(7, 9), new PointF(14, 9), new PointF(14, 4) });

        using var accentBrush = new SolidBrush(accent);
        gg.FillRectangle(accentBrush, 8, 13, 8, 5);
    });

    public static void Delete(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var accentBrush = new SolidBrush(accent);
        gg.FillPolygon(accentBrush, new[]
        {
            new PointF(9, 3.5f), new PointF(15, 3.5f), new PointF(16, 5.5f),
            new PointF(20, 5.5f), new PointF(20, 7.5f), new PointF(4, 7.5f),
            new PointF(4, 5.5f), new PointF(8, 5.5f)
        });

        using var pen = new Pen(outline, 1.7f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[] { new PointF(6.5f, 8), new PointF(7.5f, 21), new PointF(16.5f, 21), new PointF(17.5f, 8) });
        gg.DrawLine(pen, 10, 11, 10, 18);
        gg.DrawLine(pen, 14, 11, 14, 18);
    });

    public static void Print(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        // 예전 버전은 본체(y9~18)와 아래쪽 출력용지함(y17~22) 사각형이 1 유닛 겹쳐서
        // 작은 크기로 그릴 때 그 자리에 선이 뭉개져("찌그러져") 보이는 문제가 있었다.
        // 겹치지 않게 다시 그림: 위쪽 용지는 열린 선으로, 본체/출력함은 서로 안 닿게 간격을 둠.
        using var pen = new Pen(outline, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };

        // 위에서 본체로 들어가는 용지
        gg.DrawLines(pen, new[] { new PointF(8, 9), new PointF(8, 4), new PointF(16, 4), new PointF(16, 9) });

        // 프린터 본체
        gg.DrawRectangle(pen, 4, 9, 16, 6.5f);

        // 본체 상태표시등
        using var dotBrush = new SolidBrush(outline);
        gg.FillEllipse(dotBrush, 14.7f, 11.2f, 2, 2);

        // 출력 용지함 (강조색 채움, 본체와 겹치지 않도록 살짝 띄움)
        using var accentBrush = new SolidBrush(accent);
        gg.FillRectangle(accentBrush, 7.5f, 17, 9, 5);
    });

    public static void RowAdd(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(outline, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawRectangle(pen, 3.5f, 4, 17, 15);
        gg.DrawLine(pen, 3.5f, 9, 20.5f, 9);
        gg.DrawLine(pen, 3.5f, 14, 20.5f, 14);

        // New 아이콘과 동일한 자리에 같은 스타일의 "+" 배지로 통일감을 준다
        using var accentBrush = new SolidBrush(accent);
        gg.FillEllipse(accentBrush, 13.5f, 13.5f, 8, 8);
        using var whitePen = new Pen(Color.White, 1.4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        gg.DrawLine(whitePen, 17.5f, 15.7f, 17.5f, 19.3f);
        gg.DrawLine(whitePen, 15.7f, 17.5f, 19.3f, 17.5f);
    });

    public static void RowDelete(Graphics g, Rectangle rect, Color outline, Color accent) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(outline, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawRectangle(pen, 3.5f, 4, 17, 15);
        gg.DrawLine(pen, 3.5f, 9, 20.5f, 9);
        gg.DrawLine(pen, 3.5f, 14, 20.5f, 14);

        // RowAdd와 같은 자리, "-" 배지만 다르게 - 삭제 대상 행이 아니라 "행을 없앤다"는
        // 동작 자체를 뜻하므로 Delete(휴지통) 아이콘과 달리 배지 하나로만 표현한다.
        using var accentBrush = new SolidBrush(accent);
        gg.FillEllipse(accentBrush, 13.5f, 13.5f, 8, 8);
        using var whitePen = new Pen(Color.White, 1.4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        gg.DrawLine(whitePen, 15.7f, 17.5f, 19.3f, 17.5f);
    });

    /// <summary>24x24 가상 좌표계 -> 실제 rect 크기로 자동 스케일링해서 그려주는 헬퍼</summary>
    private static void DrawScaled(Graphics g, Rectangle rect, Action<Graphics> draw)
    {
        var state = g.Save();
        g.TranslateTransform(rect.X, rect.Y);
        var scale = Math.Min(rect.Width, rect.Height) / 24f;
        g.ScaleTransform(scale, scale);
        draw(g);
        g.Restore(state);
    }
}
