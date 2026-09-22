using System.Drawing;
using System.Drawing.Drawing2D;

namespace WYNLAB.Base;

/// <summary>
/// WYN LAB 브랜드 마크(아이소메트릭 큐브 배지) - 로그인 화면 좌측 패널, 스플래시 화면,
/// 셸 툴바 로고 세 곳이 이 클래스 하나만 공유해서 그린다. 좌표는 목업(30x30 기준 SVG,
/// scratchpad/wynlab_mark_studies.html의 "E. 아이소메트릭 큐브" 시안)과 동일 비율이라,
/// 나중에 목업을 다시 손보면 이 좌표도 그대로 옮기면 세 곳이 같이 바뀐다.
/// </summary>
public static class LogoPainter
{
    private const float BaseSize = 30f;

    private static readonly PointF[] TopFace = { new(15, 3), new(25, 8.5f), new(15, 14), new(5, 8.5f) };
    private static readonly PointF[] LeftFace = { new(5, 8.5f), new(15, 14), new(15, 25), new(5, 19.5f) };
    private static readonly PointF[] RightFace = { new(15, 14), new(25, 8.5f), new(25, 19.5f), new(15, 25) };

    // 앞면 중앙에 살짝 눌린(embossed) 느낌을 내는 W - 어두운 획을 살짝 아래로 겹쳐 그린 뒤
    // 그 위에 흰 획을 덮는다.
    private static readonly PointF[] WShadowStroke = { new(11.1f, 13.6f), new(13.3f, 21.6f), new(15.6f, 15.6f), new(17.9f, 21.6f), new(20.1f, 13.6f) };
    private static readonly PointF[] WStroke = { new(10.5f, 13f), new(12.7f, 21f), new(15f, 15f), new(17.3f, 21f), new(19.5f, 13f) };

    /// <summary>
    /// rect 안에 큐브 배지를 그린다. darkBackground로 밑 그림자의 색·농도를 배경에 맞춰 고른다 -
    /// 어두운 배경(로그인 좌측 브랜드 패널)에선 검은 그림자, 밝은 배경(툴바 헤더/스플래시)에선
    /// 옅은 남색 그림자라야 배경에 파묻히지 않고 자연스럽다. rect는 정사각형을 가정한다.
    /// </summary>
    public static void Draw(Graphics g, Rectangle rect, bool darkBackground)
    {
        var savedState = g.Save();
        try
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var scale = rect.Width / BaseSize;
            g.TranslateTransform(rect.X, rect.Y);
            g.ScaleTransform(scale, scale);

            // 원래 좌표(TopFace 등)는 30x30 캔버스 안에서 가로 5~25(66.7%), 세로 3~28.7(85.7%)만
            // 채워서, 정사각형 아이콘(특히 작업표시줄/바로가기 .ico)에서 옆에 놓인 다른 앱
            // 아이콘들보다 눈에 띄게 작아 보인다는 지적(2026-09-06)이 있었다. 좌표 배열을 전부
            // 다시 재는 대신, 그리기 직전에 도형 중심(대략 15, 15.85) 기준으로 살짝 더 확대하는
            // 변환을 하나 더 끼워 넣는 방식으로 여백만 줄인다 - 세로는 이미 꽤 채워져 있어
            // 살짝만(1.05배), 가로는 부족한 만큼 더(1.16배) 키워서 전체적으로 고르게 채운다.
            const float pivotX = 15f, pivotY = 15.85f;
            const float fillScaleX = 1.16f, fillScaleY = 1.05f;
            g.TranslateTransform(pivotX, pivotY);
            g.ScaleTransform(fillScaleX, fillScaleY);
            g.TranslateTransform(-pivotX, -pivotY);

            var shadowColor = darkBackground ? Color.Black : ColorHelper.FromHex("1B2A3D");
            using (var shadowBrush = new SolidBrush(Color.FromArgb(darkBackground ? 56 : 31, shadowColor)))
                g.FillEllipse(shadowBrush, 7f, 25.9f, 16f, 2.8f);

            using (var topBrush = new LinearGradientBrush(new PointF(5, 3), new PointF(25, 14), ColorHelper.FromHex("CFE8FF"), ColorHelper.FromHex("6FA9FF")))
                g.FillPolygon(topBrush, TopFace);
            using (var leftBrush = new LinearGradientBrush(new PointF(5, 8.5f), new PointF(15, 25), ColorHelper.FromHex("3D7FEF"), ColorHelper.FromHex("1E4FB0")))
                g.FillPolygon(leftBrush, LeftFace);
            using (var rightBrush = new LinearGradientBrush(new PointF(15, 8.5f), new PointF(25, 25), ColorHelper.FromHex("2563EB"), ColorHelper.FromHex("123B8F")))
                g.FillPolygon(rightBrush, RightFace);

            using (var shadowPen = new Pen(Color.FromArgb(89, ColorHelper.FromHex("0B1E42")), 2.3f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                g.DrawLines(shadowPen, WShadowStroke);
            using (var pen = new Pen(Color.White, 2.3f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                g.DrawLines(pen, WStroke);
        }
        finally
        {
            g.Restore(savedState);
        }
    }
}
