using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WYNLAB.Base;

/// <summary>
/// 상단 툴바 전용 아이콘 버튼(2026-09 리디자인, IconBadgeButton을 대체). IconBadgeButton은
/// 작은 "배지"를 버튼 안쪽에 inset으로 두는 방식이라, 배지 채움색을 헤더 배경과 거의 구분 안
/// 되는 톤으로 눌러뒀더니 "칩처럼 안 보이고 밋밋하다"는 피드백이 반복됐다(실제로 겪음 - 흰
/// 헤더 위에서 버튼 자체가 거의 안 보였다). 이 컨트롤은 컨트롤 경계 전체(약간의 여백만 두고)가
/// 곧 칩이고, 채움색도 브랜드색에서 눈에 띄게 섞은 톤을 기본으로 쓴다 - ShellForm이 생성
/// 시점에 ChipBackColor/ChipHoverColor/ChipPressedColor를 브랜드 파생색으로 채워준다(다른
/// 브랜드 파생 색들과 같은 계산 패턴, ShellForm 헤더 참고).
///
/// 아이콘은 항상 IconImage(SvgIcons로 그린 벡터, 단일 색)만 그린다 - IconBadgeButton처럼
/// 서버 Assets의 PNG로 갈아끼우는 경로는 없다(툴바 아이콘은 회사별 커스터마이즈 대상이
/// 아니라는 판단 - SvgIcons의 ToolbarXxx 상수 설명 참고). 텍스트 캡션도 없음(Text는
/// 툴팁으로만 쓰임, IconBadgeButton과 같은 관례).
/// </summary>
public class IconChipButton : Control
{
    /// <summary>SvgIcons.Load(...)로 미리 그려둔 벡터 아이콘.</summary>
    public Image? IconImage { get; set; }

    /// <summary>칩 안에서 아이콘이 차지할 여백.</summary>
    public int IconInset { get; set; } = 15;

    public int CornerRadius { get; set; } = 10;

    public Color ChipBackColor { get; set; } = Color.FromArgb(238, 240, 244);
    public Color ChipHoverColor { get; set; } = Color.FromArgb(224, 228, 235);
    public Color ChipPressedColor { get; set; } = Color.FromArgb(210, 215, 224);
    private static readonly Color DisabledChipColor = Color.FromArgb(244, 245, 247);

    private bool _hover;
    private bool _pressed;

    public IconChipButton()
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
        g.Clear(Parent?.BackColor ?? Color.White);

        // 컨트롤 경계에서 2px만 남기고 거의 전체를 칩으로 채운다 - IconBadgeButton은 버튼
        // 안쪽에 작게 뜬 배지였는데, 그 여백 자체가 "칩이 흐릿하다"는 인상의 원인 중
        // 하나였다(실제로 겪음). 버튼 사이 간격은 ShellForm의 x 증가폭이 이미 담당하므로
        // 여기서 더 줄일 필요 없다.
        var chipRect = new Rectangle(2, 2, Width - 4, Height - 4);
        using var path = RoundedRect(chipRect, CornerRadius);

        var fill = !Enabled ? DisabledChipColor : _pressed ? ChipPressedColor : _hover ? ChipHoverColor : ChipBackColor;
        using (var brush = new SolidBrush(fill))
        {
            g.FillPath(brush, path);
        }

        if (IconImage != null)
        {
            // chipRect는 정사각형이 아니다(버튼이 64x56이라 칩도 60x52) - Inflate로 그냥
            // 깎으면 정사각형인 SVG 원본이 가로세로 비율이 다른 사각형에 맞춰 늘어나 그려져서
            // 아이콘이 찌그러져 보인다(실제로 겪음). 짧은 변 기준으로 정사각형 아이콘 영역을
            // 계산해서 칩 한가운데 놓는다 - SvgIcons.Load가 항상 정사각형(size x size)으로
            // 그려주므로 그려지는 쪽도 반드시 정사각형이어야 비율이 안 깨진다.
            var iconSize = Math.Min(chipRect.Width, chipRect.Height) - IconInset * 2;
            var iconRect = new Rectangle(
                chipRect.X + (chipRect.Width - iconSize) / 2,
                chipRect.Y + (chipRect.Height - iconSize) / 2,
                iconSize, iconSize);
            if (!Enabled)
            {
                // IconBadgeButton의 비활성 자동 페이드와 같은 방식(그 클래스 설명 참고) -
                // 알파만 낮춰서 눌러진 느낌을 낸다.
                using var attributes = new ImageAttributes();
                var fadeMatrix = new ColorMatrix { Matrix33 = 0.35f };
                attributes.SetColorMatrix(fadeMatrix);
                g.DrawImage(IconImage, iconRect, 0, 0, IconImage.Width, IconImage.Height, GraphicsUnit.Pixel, attributes);
            }
            else
            {
                g.DrawImage(IconImage, iconRect);
            }
        }
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
