using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// SectionHeaderWyn(그리드/패널 상단 아이콘+제목)용 단색 라인아이콘. WYNLAB.BaseForm의
/// MenuIconPainters(좌측 다크 사이드바용)와 그리는 방식은 같지만, WYNLAB.Controls는 BaseForm을
/// 참조할 수 없어서(반대 방향 참조만 허용) 별도로 둔다 - 24x24 가상 좌표계에 그리고 실제
/// 크기로 자동 스케일링한다.
/// </summary>
public static class SectionIconPainters
{
    public static void Folder(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[]
        {
            new PointF(3.5f, 7), new PointF(9, 7), new PointF(10.5f, 5), new PointF(15, 5),
            new PointF(15, 7.5f), new PointF(20.5f, 7.5f), new PointF(20.5f, 18), new PointF(3.5f, 18), new PointF(3.5f, 7)
        });
    });

    /// <summary>2x2 격자 - 목록/표 형태 데이터를 의미. 상세입력 폼/그리드 헤더에 주로 사용.</summary>
    public static void Grid(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { LineJoin = LineJoin.Round };
        gg.DrawRectangle(pen, 3.5f, 3.5f, 17, 17);
        gg.DrawLine(pen, 3.5f, 12, 20.5f, 12);
        gg.DrawLine(pen, 12, 3.5f, 12, 20.5f);
    });

    /// <summary>문서 한 장 - 상세입력/등록 폼 헤더에 사용.</summary>
    public static void Document(Graphics g, Rectangle rect, Color color) => DrawScaled(g, rect, gg =>
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        gg.DrawLines(pen, new[]
        {
            new PointF(6, 3.5f), new PointF(14, 3.5f), new PointF(18, 7.5f), new PointF(18, 20.5f), new PointF(6, 20.5f), new PointF(6, 3.5f)
        });
        gg.DrawLine(pen, 14, 3.5f, 14, 7.5f);
        gg.DrawLine(pen, 18, 7.5f, 14, 7.5f);
        gg.DrawLine(pen, 9, 12, 15, 12);
        gg.DrawLine(pen, 9, 16, 15, 16);
    });

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
}
