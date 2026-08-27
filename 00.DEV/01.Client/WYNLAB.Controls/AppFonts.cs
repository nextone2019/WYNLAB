using System.Drawing;

namespace WYNLAB.Base;

/// <summary>
/// 앱 전체 공통 타이포그래피 스케일. 화면마다 제각각 크기로 new Font(...)를 흩어놓으면
/// (7.7, 8.25, 8.5, 9.5, 10, 11, 14, 16, 20, 22pt 등) 화면 간 통일감이 없어 "깨져" 보이므로,
/// 반드시 이 클래스의 상수만 사용한다.
///
/// 폰트 종류: 맑은 고딕(Malgun Gothic) - Windows Vista 이후 기본 한글 UI 폰트이자
/// Segoe UI/Tahoma와 짝을 맞춰 디자인된 폰트라 영문/한글이 섞여도 크기·굵기가 자연스럽게
/// 맞는다. Tahoma/Segoe UI는 한글 글리프가 아예 없어서 Windows가 한글만 다른 폰트로
/// 자동 대체해 그리는데, 그 대체 폰트가 원래 폰트보다 작아 보이는 문제(영문은 정상 크기,
/// 한글만 작게 보임)가 있어서 바꿨다.
/// 크기 기준(Body): 9pt. 예전 기본 스킨("Office 2019 Colorful")의 기본 폰트를 실제로 띄워서
/// 측정한 값(Tahoma 9pt)에 맞춘 것이고, 기본 스킨이 바뀐 뒤에도(현재는 ShellForm.DefaultSkin)
/// 이미 모든 화면이 이 크기를 기준으로 맞춰져 있어서 그대로 유지한다.
/// </summary>
public static class AppFonts
{
    private const string Family = "Malgun Gothic";

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

}
