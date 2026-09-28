using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>한쪽 변에만 얇은 구분선을 그릴 때 사용 - Style=None일 때만 의미가 있다.</summary>
public enum PanelEdge
{
    None,
    Top,
    Bottom,
    Left,
    Right
}

/// <summary>
/// 화면 공통 레이아웃에서 반복되는 패널 역할을 이름 접두사 같은 규칙이 아니라 타입 있는 속성
/// 하나로 고른다 - 오타로 스타일이 조용히 안 먹는 문제를 원천 차단한다.
/// </summary>
public enum PanelWynStyle
{
    /// <summary>기본 DevExpress PanelControl 그대로(+ EdgeLine 옵션 사용 가능)</summary>
    None,

    /// <summary>화면명 아래 얇은 구분선. Dock=Top + Height를 2~3px 정도로 얇게 주고 쓴다.</summary>
    TitleDivider,

    /// <summary>그리드/패널을 감싸는 둥근 모서리 카드(배경+테두리를 실제로 둥글게 그림).</summary>
    Card
}

/// <summary>
/// DevExpress PanelControl 기반 - 순정 WinForms Panel 대신 이걸 기본으로 쓰면 스킨과 어울리는
/// 배경/테두리가 자동 적용된다. Style 속성으로 화면 공통 레이아웃에서 반복되는 역할(구분선/카드)을
/// 색상·모서리까지 한 번에 적용할 수 있다 - 색상은 전부 UiTheme(appsettings.json Theme 섹션)에서
/// 가져오므로 회사별로 배포판 설정만 바꾸면 전체 화면이 일괄로 바뀐다.
/// </summary>
[ToolboxItem(true)]
public class PanelWyn : PanelControl
{
    private PanelWynStyle _style = PanelWynStyle.None;
    private PanelEdge _edge = PanelEdge.None;
    private Color _edgeColor = Color.FromArgb(228, 229, 232);
    private int _edgeThickness = 1;
    private int _cardCornerRadius = 10;

    private bool _showTitleBar;
    private string _titleText = string.Empty;
    private SectionHeaderIcon _titleIcon = SectionHeaderIcon.Grid;
    private bool _showTitleDivider = true;

    public PanelWyn()
    {
        BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
    }

    /// <summary>PanelControl.ShouldSerializeBorderStyle()이 이 값과 비교해서 같으면 디자이너가
    /// InitializeComponent에 BorderStyle을 아예 안 써버린다 - 기본 DevExpress 기준값은
    /// BorderStyles.Default인데 PanelWyn은 생성자에서 NoBorder로 초기화하므로, 기준값도
    /// NoBorder로 맞춰야 "Default를 직접 선택"한 값이 재빌드 후에도 유지된다(안 맞으면
    /// Default 선택 시 "이미 기본값"으로 오인되어 저장을 생략하고, 재빌드 시 생성자의
    /// NoBorder로 되돌아가 버린다 - 실제로 겪은 버그).</summary>
    protected override DevExpress.XtraEditors.Controls.BorderStyles DefaultBorderStyle =>
        DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

    /// <summary>DevExpress PanelControl은 자기 자신의 배경을 순정 WinForms .BackColor가 아니라
    /// Appearance.BackColor(+UseBackColor)로 그린다 - .BackColor만 설정하면 값은 저장되지만
    /// (Style=Card인 자식이 Parent.BackColor로 읽어가는 용도로는 충분) 패널 자기 자신은 계속
    /// 스킨 기본색으로 보인다. 매번 두 속성을 같이 챙기지 않아도 되도록 여기서 자동 동기화한다
    /// (2026-09-22 캔버스 배경이 하나도 안 보이던 문제의 원인).</summary>
    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        Appearance.BackColor = BackColor;
        Appearance.Options.UseBackColor = true;
    }

    [Category("WYNLAB")]
    [Description("화면 공통 레이아웃 역할(구분선/카드)을 한 번에 적용합니다.")]
    [DefaultValue(PanelWynStyle.None)]
    public PanelWynStyle Style
    {
        get => _style;
        set { _style = value; UpdateContentPadding(); Invalidate(); }
    }

    /// <summary>패널 상단에 아이콘+제목을 표시할지 여부 - 화면 최상단 타이틀("기초코드 등록")과
    /// 카드/그리드 섹션 헤더("대분류코드 리스트") 둘 다 이걸로 만든다(구분선 유무만 다르게 쓰면 됨).
    /// 켜면 자식 컨트롤이 겹치지 않도록 Padding.Top이 자동으로 밀린다.</summary>
    [Category("WYNLAB")]
    [Description("패널 상단에 아이콘+제목을 표시합니다. 화면 최상단 타이틀이나 카드/그리드 섹션 헤더로 사용.")]
    [DefaultValue(false)]
    public bool ShowTitleBar
    {
        get => _showTitleBar;
        set { _showTitleBar = value; UpdateContentPadding(); Invalidate(); }
    }

    [Category("WYNLAB")]
    [Description("타이틀바에 표시할 제목 텍스트.")]
    [DefaultValue("")]
    public string TitleText
    {
        get => _titleText;
        set { _titleText = value; Invalidate(); }
    }

    [Category("WYNLAB")]
    [Description("타이틀 앞에 표시할 아이콘 종류.")]
    [DefaultValue(SectionHeaderIcon.Grid)]
    public SectionHeaderIcon TitleIcon
    {
        get => _titleIcon;
        set { _titleIcon = value; Invalidate(); }
    }

    /// <summary>타이틀 아래 구분선 표시 여부 - 화면 최상단 타이틀은 true(선으로 구획),
    /// 카드 위 섹션 헤더는 보통 false(카드 테두리가 이미 구획 역할을 하므로)로 쓴다.</summary>
    [Category("WYNLAB")]
    [Description("타이틀 아래 구분선 표시 여부. 화면 최상단 타이틀=true, 카드 섹션 헤더=false 권장.")]
    [DefaultValue(true)]
    public bool ShowTitleDivider
    {
        get => _showTitleDivider;
        set { _showTitleDivider = value; Invalidate(); }
    }

    [Category("WYNLAB")]
    [Description("Style=Card일 때 모서리 둥근 정도(px).")]
    [DefaultValue(10)]
    public int CardCornerRadius
    {
        get => _cardCornerRadius;
        set { _cardCornerRadius = value; Invalidate(); }
    }

    [Category("WYNLAB")]
    [Description("Style=None일 때, 패널의 한쪽 변에만 얇은 구분선을 그립니다.")]
    [DefaultValue(PanelEdge.None)]
    public PanelEdge EdgeLine
    {
        get => _edge;
        set { _edge = value; Invalidate(); }
    }

    [Category("WYNLAB")]
    [Description("EdgeLine 구분선 색상.")]
    public Color EdgeLineColor
    {
        get => _edgeColor;
        set { _edgeColor = value; Invalidate(); }
    }

    [Category("WYNLAB")]
    [Description("EdgeLine 구분선 두께(px).")]
    [DefaultValue(1)]
    public int EdgeLineThickness
    {
        get => _edgeThickness;
        set { _edgeThickness = Math.Max(1, value); Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        switch (_style)
        {
            case PanelWynStyle.TitleDivider:
                PaintTitleDivider(e);
                return;
            case PanelWynStyle.Card:
                PaintCard(e);
                PaintTitleBar(e, Parent?.BackColor ?? UiTheme.CardBackColor);
                return;
            default:
                base.OnPaint(e);
                PaintEdgeLine(e);
                PaintTitleBar(e, BackColor);
                return;
        }
    }

    /// <summary>ShowTitleBar가 켜져 있으면 상단에 아이콘+제목(+구분선)을 그린다 - Style=Card일 땐
    /// 카드 바깥(부모 배경색)에, 그 외엔 패널 자기 배경색 위에 그린다.</summary>
    private void PaintTitleBar(PaintEventArgs e, Color backColor)
    {
        if (!_showTitleBar) return;
        TitleBarPainter.Paint(e.Graphics, Width, _titleText, _titleIcon, _showTitleDivider, backColor);
    }

    /// <summary>ShowTitleBar/Style이 바뀔 때마다 자식 컨트롤이 타이틀바나 카드 테두리에
    /// 겹치지 않도록 Padding을 다시 계산한다.</summary>
    private void UpdateContentPadding()
    {
        var top = _showTitleBar ? TitleBarPainter.Height : 0;
        var side = _style == PanelWynStyle.Card ? 8 : 0;
        var bottom = _style == PanelWynStyle.Card ? 8 : 0;
        if (_style == PanelWynStyle.Card) top += 4;
        Padding = new Padding(side, top, side, bottom);
    }

    private void PaintTitleDivider(PaintEventArgs e)
    {
        using var brush = new SolidBrush(UiTheme.DividerColor);
        e.Graphics.FillRectangle(brush, ClientRectangle);
    }

    private void PaintCard(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        // 배경을 먼저 부모 색으로 지워야 라운드 바깥 네 귀퉁이(사각 클라이언트 영역 중 둥근
        // 테두리 밖의 삼각 자투리)가 패널 배경색으로 남아 어긋나 보이는 걸 막을 수 있다.
        g.Clear(Parent?.BackColor ?? UiTheme.CardBackColor);

        var top = _showTitleBar ? TitleBarPainter.Height : 0;
        var rect = new Rectangle(0, top, Width - 1, Height - top - 1);
        using var path = RoundedRect(rect, _cardCornerRadius);
        using var fillBrush = new SolidBrush(UiTheme.CardBackColor);
        g.FillPath(fillBrush, path);
        using var pen = new Pen(UiTheme.CardBorderColor, 1f);
        g.DrawPath(pen, path);
    }

    private void PaintEdgeLine(PaintEventArgs e)
    {
        if (_edge == PanelEdge.None) return;

        using var brush = new SolidBrush(_edgeColor);
        var rect = _edge switch
        {
            PanelEdge.Top => new Rectangle(0, 0, Width, _edgeThickness),
            PanelEdge.Bottom => new Rectangle(0, Height - _edgeThickness, Width, _edgeThickness),
            PanelEdge.Left => new Rectangle(0, 0, _edgeThickness, Height),
            PanelEdge.Right => new Rectangle(Width - _edgeThickness, 0, _edgeThickness, Height),
            _ => Rectangle.Empty
        };
        e.Graphics.FillRectangle(brush, rect);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        if (radius <= 0) { path.AddRectangle(bounds); return path; } // 라운드 0 = 각진 카드(AddArc는 지름 0이면 예외)
        var d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
