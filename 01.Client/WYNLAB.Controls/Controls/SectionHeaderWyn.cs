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

/// <summary>SectionHeaderWyn 안에서 아이콘과 제목을 세로 어디에 붙일지.</summary>
public enum SectionHeaderContentAlign
{
    Top,
    Middle,
    Bottom
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

    /// <summary>Bottom 정렬일 때 아래 테두리에 완전히 딱 붙지 않도록 남기는 여백. 0으로 두면
    /// 헤더 바로 아래에 오는 그리드/카드 경계선과 글자가 붙어버려 답답해 보인다.</summary>
    private const int BottomInset = 2;

    private SectionHeaderIcon _icon = SectionHeaderIcon.Grid;
    private SectionHeaderContentAlign _contentAlign = SectionHeaderContentAlign.Bottom;
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

    /// <summary>아이콘과 제목을 세로 어디에 붙일지. 기본은 Bottom - 이 헤더는 바로 아래 오는
    /// 그리드/패널을 가리키는 라벨이라, 아래쪽에 붙어야 무엇을 설명하는지가 시각적으로 이어진다.
    /// 아이콘과 글자가 같은 바닥선을 공유하도록 그려서 둘의 높이가 달라도 어긋나 보이지 않는다.</summary>
    [Category("WYNLAB")]
    [Description("아이콘과 제목의 세로 정렬 위치.")]
    [DefaultValue(SectionHeaderContentAlign.Bottom)]
    public SectionHeaderContentAlign ContentAlign
    {
        get => _contentAlign;
        set { _contentAlign = value; Invalidate(); }
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

    /// <summary>UserPaint 컨트롤은 Text가 바뀌어도 자동으로 다시 그려지지 않는다(네이티브
    /// 텍스트 렌더링을 안 쓰므로) - 사용자그룹관리처럼 런타임에 제목을 바꿔가며 재사용하는
    /// 화면(신규/수정 모드 전환)이 생기면서 필요해졌다. 지금까지는 전부 디자이너에서 한 번만
    /// 설정하고 끝이라 드러나지 않았던 부분.</summary>
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var iconRect = new Rectangle(0, IconTop(), IconSize, IconSize);

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

        // 글자도 아이콘과 같은 영역을 기준으로 정렬해야 둘의 바닥선이 맞는다 - Bottom일 때
        // 글자 상자만 컨트롤 전체 높이(Height)를 쓰면 아이콘보다 BottomInset만큼 더 내려간다.
        var textRect = new Rectangle(
            iconRect.Right + Gap,
            0,
            Math.Max(0, Width - iconRect.Right - Gap),
            _contentAlign == SectionHeaderContentAlign.Bottom ? Math.Max(0, Height - BottomInset) : Height);

        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, UiTheme.SectionHeaderTextColor,
            TextVerticalFlag() | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    private int IconTop() => _contentAlign switch
    {
        SectionHeaderContentAlign.Top => 0,
        SectionHeaderContentAlign.Bottom => Math.Max(0, Height - IconSize - BottomInset),
        _ => (Height - IconSize) / 2
    };

    private TextFormatFlags TextVerticalFlag() => _contentAlign switch
    {
        SectionHeaderContentAlign.Top => TextFormatFlags.Top,
        SectionHeaderContentAlign.Bottom => TextFormatFlags.Bottom,
        _ => TextFormatFlags.VerticalCenter
    };

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _renderedSvgIcon?.Dispose();
        }
        base.Dispose(disposing);
    }
}
