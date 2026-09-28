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

    // Default는 평소엔 배경이 없다가(투명) 호버할 때만 채워진다. 호버/눌림 배경은 원래 Slate
    // 100/200 불투명색(흰 배경 시절 값)이었는데, 어두운 남색 헤더 위에서는 거의 흰색 플래시처럼
    // 튀어 보였다("마우스오버 효과가 너무 밝아" 지적, 2026-09-23) - 불투명 색 대신 낮은 알파의
    // 흰색을 얹는 방식으로 바꿨다. 실제 헤더색이 사이트 테마(_accentColor)에 따라 바뀌는데도
    // (ShellForm.NavDarkBg/HeaderBg), 알파 오버레이는 그 밑에 깔린 색이 뭐든 "그 색보다 살짝
    // 밝게"만 만들어서 헤더색을 몰라도 항상 자연스럽다. DefaultTextColor는 원래 진한
    // 슬레이트(51,65,85, 흰 배경용)였는데, 어두운 헤더 위에서 거의 안 보이고 오히려
    // DisabledTextColor(밝은 회색)가 더 잘 보이는 역전 현상이 있었다("비활성일 때 폰트가 더 잘
    // 보여" 지적, 2026-09-23) - 활성=흰색, 비활성=어두운 회색 톤으로 뒤집었다
    // (ShellForm.ToolbarIconColor도 아이콘 쪽에 맞춰 같이 변경).
    public Color DefaultHoverBg { get; set; } = Color.FromArgb(22, 255, 255, 255);
    public Color DefaultPressedBg { get; set; } = Color.FromArgb(38, 255, 255, 255);
    public Color DefaultTextColor { get; set; } = Color.White;

    public Color DangerHoverBg { get; set; } = Color.FromArgb(18, 220, 38, 38);
    public Color DangerPressedBg { get; set; } = Color.FromArgb(32, 220, 38, 38);
    public Color DangerTextColor { get; set; } = Color.FromArgb(220, 38, 38);
    public Color DangerBorderColor { get; set; } = Color.FromArgb(254, 202, 202);

    // Primary(저장)는 항상 꽉 채운 강조색 - 호버/눌림은 그 색을 살짝 어둡게/밝게.
    public Color PrimaryBackColor { get; set; } = Color.FromArgb(37, 99, 235);
    public Color PrimaryHoverBg { get; set; } = Color.FromArgb(29, 78, 216);
    public Color PrimaryPressedBg { get; set; } = Color.FromArgb(23, 63, 175);
    public Color PrimaryTextColor { get; set; } = Color.White;

    // Accent(조회) - 평소엔 배경 없이 아이콘/글자 색(파랑)만으로 강조하고(2026-09-22), 호버/눌림도
    // Default와 같은 원리의 낮은 알파 흰색 오버레이를 쓴다 - 예전 불투명 옅은 파랑(219,234,254)은
    // 어두운 헤더 위에서 너무 밝게 튀었다.
    public Color AccentBackColor { get; set; } = Color.FromArgb(239, 246, 255);
    public Color AccentHoverBg { get; set; } = Color.FromArgb(22, 255, 255, 255);
    public Color AccentPressedBg { get; set; } = Color.FromArgb(38, 255, 255, 255);
    public Color AccentBorderColor { get; set; } = Color.FromArgb(191, 219, 254);
    public Color AccentTextColor { get; set; } = Color.FromArgb(29, 78, 216);

    // 테두리 - Default는 배경이 없을 때(평소)도 옅은 테두리 하나는 항상 그려서 버튼 경계가
    // 보이게 한다("보더가 없어 보인다"는 지적, 2026-09-17). 호버 시엔 더 짙은 톤으로 바뀌어
    // 눌림 가능 상태를 드러낸다(2026-09-22). Primary/Accent/Danger는 배경 자체가 있어서
    // 각자의 톤에 맞는 테두리를 따로 둔다.
    //
    // 원래 이 톤(Slate 300/400)은 흰 배경 위에서 쓰려고 잡은 값이라, 어두운 남색 ShellForm
    // 헤더 위에서는 거의 흰 선처럼 두드러져 보였다("보더 흰색이 너무 진하고 두꺼워" 지적,
    // 2026-09-23) - 헤더 톤에 맞춰 더 어둡게(Slate 600/500) 낮췄다. 두께도 1.4 -> 1로 줄였다.
    public Color BorderColor { get; set; } = Color.FromArgb(71, 85, 105);
    public Color HoverBorderColor { get; set; } = Color.FromArgb(100, 116, 139);
    public Color PrimaryBorderColor { get; set; } = Color.FromArgb(23, 63, 175);
    private const float BorderWidth = 1f;

    // 활성(White)과 뚜렷이 구분되는 어두운 회색 톤 - 밝은 회색(180,182,186)은 오히려 흰색 텍스트보다
    // 눈에 더 띄어서 "꺼짐" 인상을 못 줬다(2026-09-23 지적).
    private static readonly Color DisabledTextColor = Color.FromArgb(110, 118, 130);

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
        // g.Clear(Parent.BackColor)로 배경을 "추측"해서 지우면 안 된다(ButtonWyn.OnPaint의
        // 같은 경고 참고) - 이 버튼이 얹히는 ShellForm 헤더는 DevExpress 스킨이 실제로 그리는
        // 어두운 남색이라, Panel.BackColor 속성 자체는 그 색과 무관하게 기본값(밝은 회색/흰색)을
        // 돌려준다. 그 틀린 값으로 지우면 Fill이 불투명한 상태(Accent 배경 등)에서는 그 위에
        // 덮여 안 보이다가, Fill이 투명해지는 비활성(Disabled) 상태에서만 지워둔 밝은 사각형이
        // 그대로 드러나 "꺼진 버튼에만 흰 배경 박스"로 보였다(2026-09-22 실제 발견). 생성자의
        // BackColor=Transparent + SupportsTransparentBackColor 조합이면 WinForms가 OnPaint 전에
        // 부모의 실제 렌더링 결과를 이미 이 버퍼에 그려놔서 별도로 지울 필요가 없다.

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
            using var pen = new Pen(borderColor, BorderWidth) { Alignment = PenAlignment.Inset };
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
    /// 아이콘 틴트)를 계산한다. ShellForm 툴바(어두운 헤더)에 얹히는 버튼들이라 평소엔 전부
    /// 배경을 안 채운다(투명, 헤더 배경이 그대로 비침) - 아이콘/글자 색만으로 의미(조회=블루,
    /// 삭제/로그아웃=빨강, 나머지=슬레이트)를 구분하고, 호버할 때만 옅게 채워 눌림 가능함을
    /// 알린다("버튼 배경색을 제거해달라" 요청, 2026-09-22 - Accent도 항상 채워져 있던 것을
    /// Default/Danger와 같은 방식으로 통일). Primary(저장 강조용으로 남겨둔 변형)만 예외로
    /// 평소에도 꽉 채운 배경을 유지한다.</summary>
    private (Color Fill, Color Border, Color Text, Color IconTint) ResolveColors()
    {
        if (!Enabled) return (Color.Transparent, BorderColor, DisabledTextColor, DisabledTextColor);

        return Variant switch
        {
            IconChipVariant.Primary => (
                _pressed ? PrimaryPressedBg : _hover ? PrimaryHoverBg : PrimaryBackColor,
                PrimaryBorderColor, PrimaryTextColor, PrimaryTextColor),
            IconChipVariant.Accent => (
                _pressed ? AccentPressedBg : _hover ? AccentHoverBg : Color.Transparent,
                _hover || _pressed ? AccentBorderColor : BorderColor, AccentTextColor, AccentTextColor),
            IconChipVariant.Danger => (
                _pressed ? DangerPressedBg : _hover ? DangerHoverBg : Color.Transparent,
                _hover || _pressed ? DangerBorderColor : BorderColor, DangerTextColor, DangerTextColor),
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
            // 0.35 -> 0.6: 어두운 헤더 위에서 35% 알파는 아이콘이 거의 안 보일 정도로 흐렸다
            // ("비활성일 때 너무 흐려서 안 보여" 지적, 2026-09-23) - 여전히 활성 아이콘보다는
            // 확실히 흐리되, 형체는 알아볼 수 있는 선으로 올렸다.
            using var attributes = new ImageAttributes();
            var fadeMatrix = new ColorMatrix { Matrix33 = 0.6f };
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
