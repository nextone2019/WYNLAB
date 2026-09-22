using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 완전히 직접 그리는(커스텀 페인트) 보조 액션 버튼 - "파라미터생성"/"복사"/"실행결과 미리보기"
/// 같은 텍스트 라벨 버튼과, 행추가/행삭제 같은 아이콘 전용 버튼 둘 다에 쓴다.
///
/// 원래는 DevExpress SimpleButton을 상속해서 Appearance 색만 바꾸는 방식이었는데(2026-09-02
/// 1차), 거기에 모서리를 둥글게 하려고 Control.Region으로 잘라내봤더니 WinForms Region은
/// anti-aliasing이 아예 안 돼서 모서리가 계단져 보이는 문제가 있었다(실제로 겪음 - "둥근게
/// 아니라 끊어져 보여"). Region 클리핑은 근본적으로 부드럽게 안 된다는 걸 확인하고, IconBadgeButton
/// 과 같은 방식(OnPaint에서 SmoothingMode.AntiAlias로 직접 그림)으로 완전히 바꿨다(2026-09-02 2차).
///
/// DevExpress SimpleButton을 더 이상 안 쓰므로, 그동안 화면들이 쓰던 SimpleButton 전용 API
/// (Appearance.BackColor/BorderColor, ImageOptions.Image, Options.UseXxx)는 없다 - 대신 이
/// 클래스 자체의 평범한 속성(FillColor/BorderColor/Image)을 쓴다. ToolTip만은 예외로 SimpleButton과
/// 똑같이 "문자열 대입 = 툴팁 등록" 형태를 유지했다(내부적으로 공유 ToolTip 컴포넌트에 위임) -
/// 화면 코드 쪽 변경을 최소화하기 위해서다.
/// </summary>
[ToolboxItem(true)]
public class ButtonWyn : Control
{
    private static readonly ToolTip SharedToolTip = new();

    // 2026-09-04 사장님 지시로 원복 - 연한 파란색 배경 채움 디자인이 맘에 안 든다고 하셔서,
    // 옛날(SimpleButton 그대로 쓰던 시절) 흰 배경 + 검은 글자의 평범한 모양으로 되돌리고,
    // 텍스트박스 등 다른 입력 컨트롤과 구분되도록 테두리만 파란 톤으로 남겼다(둥근 모서리는
    // 그대로 유지 - 그 부분은 마음에 든다고 하셨음).
    private static readonly Color DefaultFill = Color.White;
    private static readonly Color DefaultHover = Color.FromArgb(240, 245, 255);
    private static readonly Color DefaultPressed = Color.FromArgb(225, 235, 253);
    private static readonly Color DefaultBorder = Color.FromArgb(74, 134, 232);
    private static readonly Color DefaultText = Color.FromArgb(51, 51, 51);
    private static readonly Color DisabledFill = Color.FromArgb(240, 241, 243);
    private static readonly Color DisabledBorder = Color.FromArgb(223, 224, 227);

    private const int CornerRadius = 4;

    private bool _hover;
    private bool _pressed;
    private string? _toolTip;

    public ButtonWyn()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.SupportsTransparentBackColor, true);
        // BackColor를 Transparent로 명시해야 SupportsTransparentBackColor가 실제로 켜진다 -
        // 이게 빠져있으면 WinForms가 "부모를 그대로 물려서 배경을 그린다"는 정식 경로를 안 타고,
        // 대신 OnPaint의 g.Clear(Parent.BackColor)가 항상 실행돼야 하는데 그 값 자체가 진짜
        // 배경색과 다를 수 있다(부모가 PanelWyn=DevExpress PanelControl이면 실제 렌더링은 스킨
        // 엔진이 하고 .BackColor 속성은 그거랑 무관해서 항상 틀렸다 - 2026-09-04 실제 발견,
        // 버튼 모서리에 그림자처럼 보이던 원인).
        BackColor = Color.Transparent;
        Cursor = Cursors.Hand;
        Size = new Size(100, 23);
        ForeColor = DefaultText;
        FillColor = DefaultFill;
        HoverColor = DefaultHover;
        PressedColor = DefaultPressed;
        BorderColor = DefaultBorder;
    }

    /// <summary>평상시 배경색. 행추가(초록)/행삭제(빨강)처럼 버튼마다 다른 색을 쓰려면 이 값을
    /// 지정한다(기본값은 파란 액센트).</summary>
    [Category("WYNLAB")]
    public Color FillColor { get; set; }

    /// <summary>마우스를 올렸을 때 배경색.</summary>
    [Category("WYNLAB")]
    public Color HoverColor { get; set; }

    /// <summary>누르고 있는 동안 배경색.</summary>
    [Category("WYNLAB")]
    public Color PressedColor { get; set; }

    /// <summary>테두리색.</summary>
    [Category("WYNLAB")]
    public Color BorderColor { get; set; }

    /// <summary>아이콘 전용 버튼(행추가/행삭제 등)용 - 지정하면 Text 대신 이 이미지를 가운데에
    /// 그린다.</summary>
    [Category("WYNLAB")]
    public Image? Image { get; set; }

    /// <summary>DevExpress SimpleButton.ToolTip과 같은 방식(문자열 대입)으로 쓸 수 있게 한
    /// 호환용 속성 - 내부적으로 공유 ToolTip 컴포넌트에 등록한다.</summary>
    [Category("WYNLAB")]
    public string? ToolTip
    {
        get => _toolTip;
        set
        {
            _toolTip = value;
            SharedToolTip.SetToolTip(this, value);
        }
    }

    protected override void OnMouseEnter(System.EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(System.EventArgs e)
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

    protected override void OnEnabledChanged(System.EventArgs e)
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
        // 배경은 여기서 안 그린다 - BackColor=Transparent + SupportsTransparentBackColor 조합이면
        // WinForms가 OnPaint 전에 부모를 실제로 그린 결과를 이미 이 버퍼에 그려놨다(부모가 어떤
        // 색이든, DevExpress 스킨 컨트롤이든 상관없이 항상 맞다) - 직접 g.Clear로 부모색을
        // "추측"해서 지우면 그 추측이 틀렸을 때만 보이는 그림자 같은 색 불일치가 생긴다.

        var fill = !Enabled ? DisabledFill : _pressed ? PressedColor : _hover ? HoverColor : FillColor;
        var border = !Enabled ? DisabledBorder : BorderColor;

        // (0,0,Width-1,Height-1)로 두면 원점(0,0)에서 시작하는 사각형이라 좌/상단은 컨트롤
        // 경계에 딱 붙지만 우/하단은 1px 못 미친 지점(Width-1, Height-1)에서 끝난다 - 그 1px
        // 틈은 채움도 테두리도 안 그리는 미착색 영역으로 남아서, 부모(PanelWyn, 순백색이 아닌
        // DevExpress 스킨 배경)가 그대로 비쳐 보인다(2026-09-06 실제 발견 - "우측/하단만 진하게
        // 보인다"는 증상의 진짜 원인. Inset 정렬 자체는 문제가 아니었다). 네 변 다 동일하게
        // 컨트롤 경계까지 꽉 채우도록 Width/Height 그대로 쓴다 - Inset 정렬 펜은 경로 안쪽에만
        // 그려지는 게 보장되므로 클리핑 방지용 여백이 애초에 필요 없다.
        var rect = new Rectangle(0, 0, Width, Height);
        using (var path = RoundedRect(rect, CornerRadius))
        {
            using (var brush = new SolidBrush(fill)) g.FillPath(brush, path);
            using (var pen = new Pen(border) { Alignment = PenAlignment.Inset }) g.DrawPath(pen, path);
        }

        if (Image != null)
        {
            var imgRect = new Rectangle((Width - Image.Width) / 2, (Height - Image.Height) / 2, Image.Width, Image.Height);
            g.DrawImage(Image, imgRect);
        }
        else if (!string.IsNullOrEmpty(Text))
        {
            var textColor = Enabled ? ForeColor : Color.FromArgb(160, 162, 166);
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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
