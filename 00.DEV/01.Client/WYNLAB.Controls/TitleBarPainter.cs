using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WYNLAB.Base.Controls;

namespace WYNLAB.Base;

/// <summary>
/// PanelWyn/GridControlWyn이 공유하는 "아이콘 + 제목(+구분선)" 타이틀바 그리기 로직.
/// 화면 최상단 타이틀("기초코드 등록" + 아래 구분선)과 카드/그리드 헤더("대분류코드 리스트" 같은
/// 섹션 제목)가 사실 같은 그림(아이콘+텍스트, 구분선 유무만 다름)이라 하나로 통일했다.
/// GridViewWynBehavior처럼 여러 Wyn 컨트롤이 상속 관계가 아니라서 공용 static 헬퍼로 뒀다.
/// </summary>
internal static class TitleBarPainter
{
    /// <summary>타이틀바가 차지하는 높이(px) - 아이콘/텍스트 한 줄 + 구분선 두께 포함.</summary>
    public const int Height = 30;

    private const int IconSize = 15;
    private const int Gap = 6;
    private const int DividerThickness = 2;

    public static void Paint(Graphics g, int width, string title, SectionHeaderIcon icon, bool showDivider, Color backColor)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var backBrush = new SolidBrush(backColor))
        {
            g.FillRectangle(backBrush, 0, 0, width, Height);
        }

        var textAreaHeight = showDivider ? Height - DividerThickness : Height;
        var painter = icon switch
        {
            SectionHeaderIcon.Folder => (Action<Graphics, Rectangle, Color>)SectionIconPainters.Folder,
            SectionHeaderIcon.Document => SectionIconPainters.Document,
            _ => SectionIconPainters.Grid
        };

        var iconRect = new Rectangle(0, (textAreaHeight - IconSize) / 2, IconSize, IconSize);
        painter(g, iconRect, UiTheme.SectionHeaderIconColor);

        var textRect = new Rectangle(iconRect.Right + Gap, 0, Math.Max(0, width - iconRect.Right - Gap), textAreaHeight);
        TextRenderer.DrawText(g, title, AppFonts.BodyBold, textRect, UiTheme.SectionHeaderTextColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        if (showDivider)
        {
            using var lineBrush = new SolidBrush(UiTheme.DividerColor);
            g.FillRectangle(lineBrush, 0, Height - DividerThickness, width, DividerThickness);
        }
    }
}
