namespace WYNLAB.Shared;

/// <summary>
/// TSMMENU.ICON_NM(짧은 키) - 실제 DevExpress 내장 SVG 아이콘 리소스 이름 매핑. 현재는 최상위
/// (모듈) 메뉴의 사이드바 아이콘에만 쓰인다(WYNLAB.Shell.ShellForm.BuildAccordionMenu 참고).
///
/// 메뉴등록 화면(frmMenu, WYNLAB.SM)의 아이콘 선택 룩업도 같은 목록을 그대로 쓴다(2026-09-16
/// 요청 - "아이콘명을 실제 아이콘을 룩업에서 선택") - 화면에서 고를 수 있는 아이콘과 실제로
/// 사이드바에 그려지는 아이콘이 항상 일치하도록, 목록을 여기 한 곳에만 두고 양쪽(WYNLAB.Shell,
/// WYNLAB.SM)이 같이 참조한다. 이 프로젝트(WYNLAB.Shared)는 서버/클라이언트 양쪽에서 참조되는
/// netstandard2.0이라 DevExpress를 전혀 모른다 - 그래서 순수 문자열 데이터로만 갖고 있는다.
/// SvgResourceName 값은 DevExpress.Images.ImageResourceCache.Default.GetSvgImage(...)에 그대로
/// 넘길 수 있는 리소스 이름이다(DevExpress를 참조하는 프로젝트라면 어디서든 동일하게 동작 -
/// WYNLAB.Shell/SvgIcons.cs 참고).
///
/// 새 아이콘을 추가하고 싶으면 이 목록에 한 줄만 추가하면 된다 - ShellForm과 frmMenu 둘 다
/// 자동으로 반영된다.
/// </summary>
public static class MenuIconCatalog
{
    public static readonly IReadOnlyList<MenuIconEntry> Entries = new List<MenuIconEntry>
    {
        new MenuIconEntry("settings", "설정", "svgimages/icon%20builder/actions_settings.svg"),
        new MenuIconEntry("shoppingcart", "장바구니", "svgimages/icon%20builder/shopping_shoppingcart.svg"),
        new MenuIconEntry("tools", "도구/DB", "svgimages/icon%20builder/actions_database.svg"),
        new MenuIconEntry("user", "사용자", "svgimages/icon%20builder/actions_user.svg"),
        new MenuIconEntry("security", "보안", "svgimages/icon%20builder/security_security.svg"),
        new MenuIconEntry("box", "박스/재고", "svgimages/icon%20builder/shopping_box.svg"),
        new MenuIconEntry("money", "회계/money", "svgimages/icon%20builder/business_money.svg"),
        new MenuIconEntry("code", "코드/개발도구", "svgimages/xaf/action_showscript.svg"),
        new MenuIconEntry("home", "홈/대시보드", "svgimages/icon%20builder/actions_home.svg"),
        new MenuIconEntry("calendar", "일정/캘린더", "svgimages/icon%20builder/actions_calendar.svg"),
        new MenuIconEntry("clock", "시간/근태", "svgimages/icon%20builder/actions_clock.svg"),
        new MenuIconEntry("bell", "알림", "svgimages/icon%20builder/actions_bell.svg"),
        new MenuIconEntry("mail", "메일/쪽지", "svgimages/icon%20builder/actions_envelopeclose.svg"),
        new MenuIconEntry("approval", "승인/결재", "svgimages/icon%20builder/actions_checkcircled.svg"),
        new MenuIconEntry("folder", "문서/폴더", "svgimages/icon%20builder/actions_folderopen.svg"),
        new MenuIconEntry("report", "보고서", "svgimages/icon%20builder/business_report.svg"),
        new MenuIconEntry("chart", "통계/차트", "svgimages/icon%20builder/business_piechart.svg"),
        new MenuIconEntry("briefcase", "업무/기획", "svgimages/icon%20builder/business_briefcase.svg"),
        new MenuIconEntry("bank", "은행/재무", "svgimages/icon%20builder/business_bank.svg"),
        new MenuIconEntry("hr", "인사/사원", "svgimages/icon%20builder/business_businessman.svg"),
        new MenuIconEntry("delivery", "배송/물류", "svgimages/icon%20builder/shopping_delivery.svg"),
        new MenuIconEntry("store", "매장/영업점", "svgimages/icon%20builder/shopping_store.svg"),
        new MenuIconEntry("world", "글로벌/전사", "svgimages/icon%20builder/business_world.svg"),
    };
}

// netstandard2.0(서버/클라이언트 공용)엔 record의 init 접근자가 요구하는
// System.Runtime.CompilerServices.IsExternalInit이 없어서 record 대신 평범한 불변 클래스로 둔다.
public sealed class MenuIconEntry
{
    public MenuIconEntry(string key, string displayNm, string svgResourceName)
    {
        Key = key;
        DisplayNm = displayNm;
        SvgResourceName = svgResourceName;
    }

    public string Key { get; }
    public string DisplayNm { get; }
    public string SvgResourceName { get; }
}
