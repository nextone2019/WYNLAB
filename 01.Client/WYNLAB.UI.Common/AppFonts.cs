using System.Drawing;

namespace WYNLAB.UI.Common;

/// <summary>
/// 앱 전체 공통 타이포그래피 스케일. 화면마다 제각각 크기로 new Font(...)를 흩어놓으면
/// (7.7, 8.25, 8.5, 9.5, 10, 11, 14, 16, 20, 22pt 등) 화면 간 통일감이 없어 "깨져" 보이므로,
/// 반드시 이 클래스의 상수만 사용한다.
///
/// 폰트 종류/기준 크기(Body) 근거: DevExpress "Office 2019 Colorful" 스킨(Program.cs에서
/// 앱 전체에 적용)의 기본 폰트를 실제로 띄워서 측정한 값 = Tahoma 9pt. 그리드 헤더/행처럼
/// 이 클래스로 스타일을 안 준 컨트롤은 전부 이 스킨 기본값을 그대로 쓰기 때문에, 명시적으로
/// 스타일을 주는 라벨/메뉴트리/탭 등도 같은 값(Tahoma 9pt)을 기준으로 맞춰야 서로 안 어긋난다.
/// </summary>
public static class AppFonts
{
    private const string Family = "Tahoma";

    /// <summary>로그인 화면 브랜드 타이틀 ("WYN LAB")</summary>
    public static readonly Font Display = new(Family, 26f, FontStyle.Bold);

    /// <summary>화면 큰 제목 - 로그인 "로그인" 문구, 홈 화면 타이틀 등</summary>
    public static readonly Font Heading = new(Family, 15f, FontStyle.Bold);

    /// <summary>섹션/그룹 헤더 - 좌측 메뉴 최상위 그룹, 헤더 로고명</summary>
    public static readonly Font SubHeading = new(Family, 10f, FontStyle.Bold);

    /// <summary>기본 본문 - 버튼, 입력값, 사용자정보, 콤보박스, 메뉴 하위 항목 등.
    /// 스킨 기본값(그리드 헤더/행 등)과 동일한 크기.</summary>
    public static readonly Font Body = new(Family, 9f, FontStyle.Regular);

    /// <summary>기본 본문 굵게 - 강조가 필요한 본문 텍스트</summary>
    public static readonly Font BodyBold = new(Family, 9f, FontStyle.Bold);

    /// <summary>보조 캡션 - 필드 라벨, 상태바, 툴바 아이콘 캡션, 버전 정보</summary>
    public static readonly Font Caption = new(Family, 8f, FontStyle.Regular);

    /// <summary>로고 배지 안 글자(W) - 큰 사이즈(로그인 화면 좌측 패널용)</summary>
    public static readonly Font LogoGlyphLarge = new(Family, 22f, FontStyle.Bold);

    /// <summary>로고 배지 안 글자(W) - 작은 사이즈(셸 상단 헤더용)</summary>
    public static readonly Font LogoGlyphSmall = new(Family, 11f, FontStyle.Bold);
}
