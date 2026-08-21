namespace NEXTFramework.Shared.Dtos;

/// <summary>
/// MENU 테이블 1:1 매핑 DTO.
/// Shell의 좌측 AccordionControl을 이 목록으로 동적 구성한다.
/// </summary>
public class MenuDto
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM"; // FORM / GROUP
    public string? FormClassNm { get; set; }        // 리플렉션으로 로딩할 화면 클래스 풀네임
    public string? IconNm { get; set; }
    public int SortOrder { get; set; }

    // 로그인 사용자 기준으로 서버가 병합해서 내려주는 권한 정보
    public bool ViewYn { get; set; } = true;
    public bool InsertYn { get; set; }
    public bool UpdateYn { get; set; }
    public bool DeleteYn { get; set; }
    public bool ExcelYn { get; set; } = true;
}
