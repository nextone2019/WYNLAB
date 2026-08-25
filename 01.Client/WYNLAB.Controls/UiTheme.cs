using System.Drawing;

namespace WYNLAB.Base;

/// <summary>
/// 필수입력 표시 등 화면 전반의 시각적 규칙을 담는 색상 저장소. WYNLAB.Controls는 AppConfig를
/// 전혀 모르므로(appsettings.json도, 디자인타임 문제도 이쪽엔 없음), 여기 있는 건 그냥 기본값이
/// 박힌 setter 있는 static 속성일 뿐이다 - 실제 회사별 설정값(appsettings.json의 Theme 섹션)은
/// WYNLAB.BaseForm의 AppConfig.Load()가 앱 시작 시 한 번 여기로 밀어넣어준다.
/// </summary>
public static class UiTheme
{
    public static Color RequiredFieldBackColor { get; set; } = ColorHelper.FromHex("#FFF9DB");
    public static Color RequiredFieldForeColor { get; set; } = ColorHelper.FromHex("#000000");

    public static Color TreeGroupBackColor { get; set; } = ColorHelper.FromHex("#F2F3F5");
    public static Color TreeGroupForeColor { get; set; } = ColorHelper.FromHex("#5A5D64");
    public static Color TreeLeafBackColor { get; set; } = ColorHelper.FromHex("#FFFFFF");
    public static Color TreeLeafForeColor { get; set; } = ColorHelper.FromHex("#373737");

    /// <summary>화면명 아래 구분선(PanelWyn.Style = TitleDivider) 색상.</summary>
    public static Color DividerColor { get; set; } = ColorHelper.FromHex("#E4E5E8");

    /// <summary>그리드/패널을 감싸는 카드(PanelWyn.Style = Card) 배경/테두리 색상.</summary>
    public static Color CardBackColor { get; set; } = ColorHelper.FromHex("#FFFFFF");
    public static Color CardBorderColor { get; set; } = ColorHelper.FromHex("#E1E1E1");

    /// <summary>SectionHeaderWyn(그리드/패널 상단 아이콘+제목) 아이콘/글자 색상.</summary>
    public static Color SectionHeaderIconColor { get; set; } = ColorHelper.FromHex("#5A5D64");
    public static Color SectionHeaderTextColor { get; set; } = ColorHelper.FromHex("#3C3C3C");

    /// <summary>그리드 컬럼헤더 배경/글자색 - 지정 안 하면 DevExpress 스킨 기본값(Tahoma 계열
    /// 회색조)이 그대로 쓰여서 좌측 메뉴트리(AppFonts 적용)와 톤이 미묘하게 어긋나 보인다.</summary>
    public static Color GridHeaderBackColor { get; set; } = ColorHelper.FromHex("#F7F8FA");
    public static Color GridHeaderForeColor { get; set; } = ColorHelper.FromHex("#565B62");

    /// <summary>GridViewWyn.HighlightFocusedRow 켰을 때 포커스된 행의 배경색.</summary>
    public static Color GridFocusedRowBackColor { get; set; } = ColorHelper.FromHex("#FDF3E1");

    /// <summary>드래그로 폭을 조절하는 스플리터 바(SplitterWyn)의 배경색. 기본은 카드 배경과
    /// 같은 흰색이라 눈에 띄지 않는다 - 좌우 패널이 각자 카드 테두리를 갖고 있어서 스플리터까지
    /// 색을 입히면 경계선이 두 겹으로 보여 두껍고 부자연스럽다. 구분선을 굳이 보이게 하고 싶으면
    /// 이 값을 DividerColor(#E4E5E8)로 바꾸면 된다.</summary>
    public static Color SplitterBackColor { get; set; } = ColorHelper.FromHex("#FFFFFF");
}
