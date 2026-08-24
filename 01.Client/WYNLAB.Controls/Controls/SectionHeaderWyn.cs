using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Controls;
using DevExpress.Utils.Svg;

namespace WYNLAB.Base.Controls;

public enum SectionHeaderIcon
{
    Folder,
    Grid,
    Document
}

/// <summary>
/// 그리드/패널 상단에 "아이콘 + 제목"으로 이 영역이 뭔지 짧게 알려주는 헤더 스트립.
/// DevExpress를 상속하지 않은 순정 Control이라 가볍고, 아이콘은 두 가지 방식을 지원한다:
/// - Icon: 이미지 파일 없이 코드로 직접 그리는 기본 아이콘(간단하지만 퀄리티는 낮음)
/// - SvgIcon: DevExpress 내장 이미지 라이브러리(수천 개 전문 아이콘)에서 고른 실제 이미지.
///   지정하면 Icon보다 우선 적용된다. 속성창에서 "..." 누르면 DevExpress 이미지 갤러리가 뜬다.
/// </summary>
[ToolboxItem(true)]
public class SectionHeaderWyn : Control
{
    private const int IconSize = 16;
    private const int Gap = 6;

    private SectionHeaderIcon _icon = SectionHeaderIcon.Grid;
    private SvgImage? _svgIcon;
    private Image? _renderedSvgIcon;

    public SectionHeaderWyn()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Height = 24;
        Font = AppFonts.BodyBold;
        BackColor = Color.White;
    }

    [Category("WYNLAB")]
    [Description("코드로 직접 그리는 기본 아이콘 - SvgIcon을 지정하면 이 값은 무시됩니다.")]
    [DefaultValue(SectionHeaderIcon.Grid)]
    public SectionHeaderIcon Icon
    {
        get => _icon;
        set { _icon = value; Invalidate(); }
    }

    /// <summary>DevExpress 이미지 라이브러리에서 고른 실제 아이콘. 속성창에서 이 속성의 "..." 버튼을
    /// 누르면 DevExpress 내장 이미지 갤러리(수천 개)가 뜬다 - 지정하면 Icon보다 우선 적용된다.</summary>
    [Category("WYNLAB")]
    [Description("DevExpress 이미지 라이브러리에서 고른 아이콘. 지정하면 Icon보다 우선 적용됩니다.")]
    [Editor("DevExpress.Utils.Design.SvgImageEditor, DevExpress.Design.v21.2, Version=21.2.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a", typeof(UITypeEditor))]
    public SvgImage? SvgIcon
    {
        get => _svgIcon;
        set
        {
            _svgIcon = value;
            _renderedSvgIcon?.Dispose();
            _renderedSvgIcon = null;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var iconRect = new Rectangle(0, (Height - IconSize) / 2, IconSize, IconSize);

        if (_svgIcon != null)
        {
            _renderedSvgIcon ??= ImageHelper.CreateImageFromSvgImage(_svgIcon, iconRect.Size, UserLookAndFeel.Default);
            e.Graphics.DrawImage(_renderedSvgIcon, iconRect);
        }
        else
        {
            var painter = _icon switch
            {
                SectionHeaderIcon.Folder => (Action<Graphics, Rectangle, Color>)SectionIconPainters.Folder,
                SectionHeaderIcon.Document => SectionIconPainters.Document,
                _ => SectionIconPainters.Grid
            };
            painter(e.Graphics, iconRect, UiTheme.SectionHeaderIconColor);
        }

        var textRect = new Rectangle(iconRect.Right + Gap, 0, Math.Max(0, Width - iconRect.Right - Gap), Height);
        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, UiTheme.SectionHeaderTextColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _renderedSvgIcon?.Dispose();
        }
        base.Dispose(disposing);
    }
}
