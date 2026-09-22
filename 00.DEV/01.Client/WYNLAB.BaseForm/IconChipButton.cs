using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WYNLAB.Base;

/// <summary>버튼의 "성격" - Primary(저장처럼 항상 꽉 채운 강조), Accent(조회처럼 항상 옅게
/// 틴트된 배경으로 강조), Danger(삭제/로그아웃처럼 항상 빨간 글자/아이콘), Default(나머지 전부,
/// 평소엔 배경 없이 호버할 때만 살짝 채워짐).</summary>
public enum IconChipVariant
{
    Default,
    Primary,
    Accent,
    Danger
}

/// <summary>
/// 상단 툴바 전용 아이콘+라벨+단축키 버튼(2026-09-17 리디자인, 아이콘 전용이던 이전 버전을
/// 대체). 스티치로 그린 목업(아이콘+라벨+단축키가 한 줄에, 저장만 꽉 채운 파란 버튼, 삭제는
/// 빨간 글자)에 맞춰 완전히 다시 그렸다 - 이전엔 아이콘만 있고 라벨은 툴팁으로만 썼는데,
/// 이번엔 라벨/단축키를 버튼 안에 실제로 그린다.
///
/// 폭은 내용(아이콘+라벨+단축키)에 맞춰 자동으로 계산된다(UpdateSize) - 목업처럼 버튼마다
/// 텍스트 길이가 달라서 고정폭을 쓰면 "조회"처럼 짧은 라벨은 여백이 붕 뜨고 "행삭제"처럼
/// 긴 라벨은 잘린다.
/// </summary>
public class IconChipButton : Control
{
    private Image? _iconImage;
    private string? _shortcutText;
    private IconChipVariant _variant = IconChipVariant.Default;

    public Image? IconImage
    {
        get => _iconImage;
        set { _iconImage = value; UpdateSize(); }
    }

    /// <summary>"Ctrl+S"처럼 라벨 옆에 옅게 붙는 단축키 힌트. null/빈 문자열이면 안 그린다.</summary>
    public string? ShortcutText
    {
        get => _shortcutText;
        set { _shortcutText = value; UpdateSize(); }
    }

    public IconChipVariant Variant
    {
        get => _variant;
        set { _variant = value; Invalidate(); }
    }

    // 7 -> 3: "모바일 알약(Pill) 형태 금지, 데스크톱 규격 2~3px 샤프한 사각 라운드"
    // 요청(2026-09-22) - 이전엔 Height(30) 대비 반경이 커서 알약처럼 보였다.
    public int CornerRadius { get; set; } = 3;

    // Default는 평소엔 배경이 없다가(투명) 호버할 때만 채워진다. 색상은 엔터프라이즈 툴바
    // 가이드(2026-09-22) 수치를 그대로 옮겼다: 평소 흰 배경/슬레이트 테두리, 호버 Slate 100,
    // 눌림 Slate 200 - 이전엔 검정 반투명 오버레이였는데, 흰 배경 위에서 "흐리멍텅"하다는
    // 지적이 반복돼 고정 슬레이트 톤으로 바꿨다.
    public Color DefaultHoverBg { get; set; } = Color.FromArgb(241, 245, 249);
    public Color DefaultPressedBg { get; set; } = Color.FromArgb(226, 232, 240);
    public Color DefaultTextColor { get; set; } = Color.FromArgb(51, 65, 85);

    public Color DangerHoverBg { get; set; } = Color.FromArgb(18, 220, 38, 38);
    public Color DangerPressedBg { get; set; } = Color.FromArgb(32, 220, 38, 38);
    public Color DangerTextColor { get; set; } = Color.FromArgb(220, 38, 38);
    public Color DangerBorderColor { get; set; } = Color.FromArgb(254, 202, 202);

    // Primary(저장)는 항상 꽉 채운 강조색 - 호버/눌림은 그 색을 살짝 어둡게/밝게.
    public Color PrimaryBackColor { get; set; } = Color.FromArgb(37, 99, 235);
    public Color PrimaryHoverBg { get; set; } = Color.FromArgb(29, 78, 216);
    public Color PrimaryPressedBg { get; set; } = Color.FromArgb(23, 63, 175);
    public Color PrimaryTextColor { get; set; } = Color.White;

    // Accent(조회) - 항상 옅은 블루 틴트 배경으로 강조. 가장 먼저/자주 쓰는 동작이라는 걸
    // 색으로 드러낸다(2026-09-22 요청).
    public Color AccentBackColor { get; set; } = Color.FromArgb(239, 246, 255);
    public Color AccentHoverBg { get; set; } = Color.FromArgb(219, 234, 254);
    public Color AccentPressedBg { get; set; } = Color.FromArgb(191, 219, 254);
    public Color AccentBorderColor { get; set; } = Color.FromArgb(191, 219, 254);
    public Color AccentTextColor { get; set; } = Color.FromArgb(29, 78, 216);

    // 테두리 - Default는 배경이 없을 때(평소)도 옅은 테두리 하나는 항상 그려서 버튼 경계가
    // 보이게 한다("보더가 없어 보인다"는 지적, 2026-09-17). 호버 시엔 더 짙은 톤으로 바뀌어
    // 눌림 가능 상태를 드러낸다(2026-09-22). Primary/Accent/Danger는 배경 자체가 있어서
    // 각자의 톤에 맞는 테두리를 따로 둔다.
    public Color BorderColor { get; set; } = Color.FromArgb(203, 213, 225);
    public Color HoverBorderColor { get; set; } = Color.FromArgb(148, 163, 184);
    public Color PrimaryBorderColor { get; set; } = Color.FromArgb(23, 63, 175);

    private static readonly Color DisabledTextColor = Color.FromArgb(180, 182, 186);

    private const int IconSize = 16;
    private const int PaddingLeft = 12;
    private const int PaddingRight = 12;
    private const int IconLabelGap = 6;
    private const int LabelShortcutGap = 6;

    private bool _hover;
    private bool _pressed;

    public IconChipButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.SupportsTransparentBackColor, true);
        Cursor = Cursors.Hand;
        BackColor = Color.Transparent;
        // 34 -> 30: "툴바 아이콘 높이를 조금 줄여줘"(2026-09-21). ShellForm.ToolbarButtonY도
        // 같이 맞춰야 헤더 안에서 세로 가운데 정렬이 유지된다.
        Height = 30;
        // 호출부가 Font를 따로 안 건드리면 예전과 같은 크기(Body, 9pt)로 보이게 하는 기본값 -
        // 상태바 SQL LOG 칩처럼 더 작은 글씨가 필요한 자리만 인스턴스별로 Font를 바꿔 쓴다
        // (2026-09-17 요청, "폰트크기 줄이고").
        Font = AppFonts.Body;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        UpdateSize();
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        UpdateSize();
    }

    /// <summary>텍스트/단축키 없이 아이콘만 있을 때(예: 상태바 SQL LOG 버튼) 좌우로 똑같이
    /// 두는 여백 - 라벨용 PaddingLeft/Right(10/12, 비대칭)를 그대로 쓰면 아이콘이 왼쪽으로
    /// 치우쳐 보인다("아이콘이 정중앙에 위치하게" 지적, 2026-09-17).</summary>
    private const int IconOnlyPadding = 9;

    /// <summary>아이콘+라벨+단축키를 전부 그렸을 때 필요한 최소 폭으로 Width를 다시 계산한다.
    /// 목업처럼 버튼마다 텍스트 길이가 다른 걸 그대로 반영하기 위함(클래스 설명 참고).</summary>
    private void UpdateSize()
    {
        if (string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(ShortcutText) && IconImage != null)
        {
            Width = IconSize + IconOnlyPadding * 2;
            Invalidate();
            return;
        }

        var labelSize = string.IsNullOrEmpty(Text) ? Size.Empty : TextRenderer.MeasureText(Text, Font);
        var shortcutSize = string.IsNullOrEmpty(ShortcutText) ? Size.Empty : TextRenderer.MeasureText(ShortcutText, AppFonts.Caption);

        var width = PaddingLeft;
        if (IconImage != null) width += IconSize + IconLabelGap;
        width += labelSize.Width;
        if (!string.IsNullOrEmpty(ShortcutText)) width += LabelShortcutGap + shortcutSize.Width;
        width += PaddingRight;

        Width = Math.Max(width, IconSize + PaddingLeft + PaddingRight);
        Invalidate();
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

        var chipRect = new Rectangle(1, 1, Width - 2, Height - 2);
        using var path = RoundedRect(chipRect, CornerRadius);

        var (fill, borderColor, textColor, iconTint) = ResolveColors();
        if (fill.A > 0)
        {
            using var brush = new SolidBrush(fill);
            g.FillPath(brush, path);
        }
        if (Enabled)
        {
            // SmoothingMode.None으로 테두리만 그려봤는데(2026-09-21), 둥근 모서리 구간이
            // 매끄러운 곡선이 아니라 계단져 "깨진" 것처럼 보였다("보더가 깨지고 이상해" 지적) -
            // AntiAlias로 되돌린다. 얇은 선이 살짝 번져 보이는 정도는 색과 두께로 커버한다.
            using var pen = new Pen(borderColor, 1.4f) { Alignment = PenAlignment.Inset };
            g.DrawPath(pen, path);
        }

        var isIconOnly = string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(ShortcutText) && IconImage != null;
        var x = isIconOnly ? (chipRect.Width - IconSize) / 2 : chipRect.X + PaddingLeft;
        var centerY = chipRect.Y + chipRect.Height / 2;

        if (IconImage != null)
        {
            var iconRect = new Rectangle(x, centerY - IconSize / 2, IconSize, IconSize);
            DrawTintedIcon(g, IconImage, iconRect, iconTint, Enabled);
            x += IconSize + IconLabelGap;
        }

        if (!string.IsNullOrEmpty(Text))
        {
            var labelSize = TextRenderer.MeasureText(Text, Font);
            var labelRect = new Rectangle(x, centerY - labelSize.Height / 2, labelSize.Width, labelSize.Height);
            TextRenderer.DrawText(g, Text, Font, labelRect, textColor,
                TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter);
            x += labelSize.Width;
        }

        if (!string.IsNullOrEmpty(ShortcutText))
        {
            x += LabelShortcutGap;
            var shortcutColor = !Enabled ? DisabledTextColor : Color.FromArgb(Variant == IconChipVariant.Primary ? 210 : 150, textColor);
            var shortcutSize = TextRenderer.MeasureText(ShortcutText, AppFonts.Caption);
            var shortcutRect = new Rectangle(x, centerY - shortcutSize.Height / 2, shortcutSize.Width, shortcutSize.Height);
            TextRenderer.DrawText(g, ShortcutText, AppFonts.Caption, shortcutRect, shortcutColor,
                TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter);
        }
    }

    /// <summary>변형(Variant)+상태(hover/pressed/disabled)에 따른 (배경색, 테두리색, 글자색,
    /// 아이콘 틴트)를 계산한다. Primary/Accent는 평소에도 배경이 채워져 있고, Default/Danger는
    /// 호버해야 옅게 보인다.</summary>
    private (Color Fill, Color Border, Color Text, Color IconTint) ResolveColors()
    {
        if (!Enabled) return (Color.Transparent, BorderColor, DisabledTextColor, DisabledTextColor);

        return Variant switch
        {
            IconChipVariant.Primary => (
                _pressed ? PrimaryPressedBg : _hover ? PrimaryHoverBg : PrimaryBackColor,
                PrimaryBorderColor, PrimaryTextColor, PrimaryTextColor),
            IconChipVariant.Accent => (
                _pressed ? AccentPressedBg : _hover ? AccentHoverBg : AccentBackColor,
                AccentBorderColor, AccentTextColor, AccentTextColor),
            IconChipVariant.Danger => (
                _pressed ? DangerPressedBg : _hover ? DangerHoverBg : Color.Transparent,
                DangerBorderColor, DangerTextColor, DangerTextColor),
            _ => (
                _pressed ? DefaultPressedBg : _hover ? DefaultHoverBg : Color.Transparent,
                _hover || _pressed ? HoverBorderColor : BorderColor, DefaultTextColor, DefaultTextColor)
        };
    }

    /// <summary>IconImage는 SvgIcons.Load가 이미 특정 색(currentColor 치환)으로 구워낸 비트맵이라,
    /// 변형(Primary=흰색/Danger=빨강)마다 다른 색이 필요하면 ShellForm이 애초에 그 색으로 다시
    /// Load해서 넘겨준다(NewChipButton 참고) - 여기서는 비활성 상태의 알파 페이드만 처리한다.</summary>
    private static void DrawTintedIcon(Graphics g, Image icon, Rectangle rect, Color tint, bool enabled)
    {
        if (!enabled)
        {
            using var attributes = new ImageAttributes();
            var fadeMatrix = new ColorMatrix { Matrix33 = 0.35f };
            attributes.SetColorMatrix(fadeMatrix);
            g.DrawImage(icon, rect, 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, attributes);
        }
        else
        {
            g.DrawImage(icon, rect);
        }
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        var d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
