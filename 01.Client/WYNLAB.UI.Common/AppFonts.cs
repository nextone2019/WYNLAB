using System.Drawing;

namespace WYNLAB.UI.Common;

/// <summary>
/// 앱 전체 공통 타이포그래피 스케일. 화면마다 제각각 크기로 new Font(...)를 흩어놓으면
/// (7.7, 8.25, 8.5, 9.5, 10, 11, 14, 16, 20, 22pt 등) 화면 간 통일감이 없어 "깨져" 보이므로,
/// 반드시 이 클래스의 상수만 사용한다. 전부 Windows 기본 내장 폰트인 Segoe UI 기준.
/// </summary>
public static class AppFonts
{
    private const string Family = "Segoe UI";

    /// <summary>로그인 화면 브랜드 타이틀 ("WYN LAB")</summary>
    public static readonly Font Display = new(Family, 26f, FontStyle.Bold);

    /// <summary>화면 큰 제목 - 로그인 "로그인" 문구, 홈 화면 타이틀 등</summary>
    public static readonly Font Heading = new(Family, 17f, FontStyle.Bold);

    /// <summary>섹션/그룹 헤더 - 좌측 메뉴 최상위 그룹, 헤더 로고명</summary>
    public static readonly Font SubHeading = new(Family, 11f, FontStyle.Bold);

    /// <summary>기본 본문 - 버튼, 입력값, 사용자정보, 콤보박스, 메뉴 하위 항목 등</summary>
    public static readonly Font Body = new(Family, 9.5f, FontStyle.Regular);

    /// <summary>기본 본문 굵게 - 강조가 필요한 본문 텍스트</summary>
    public static readonly Font BodyBold = new(Family, 9.5f, FontStyle.Bold);

    /// <summary>보조 캡션 - 필드 라벨, 상태바, 툴바 아이콘 캡션, 버전 정보</summary>
    public static readonly Font Caption = new(Family, 8.5f, FontStyle.Regular);

    /// <summary>로고 배지 안 글자(W) - 큰 사이즈(로그인 화면 좌측 패널용)</summary>
    public static readonly Font LogoGlyphLarge = new(Family, 22f, FontStyle.Bold);

    /// <summary>로고 배지 안 글자(W) - 작은 사이즈(셸 상단 헤더용)</summary>
    public static readonly Font LogoGlyphSmall = new(Family, 11f, FontStyle.Bold);
}
