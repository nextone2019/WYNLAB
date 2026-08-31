namespace WYNLAB.Shared.Dtos;

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
    public bool UpdateYn { get; set; } // TSMMENUAUTH.SAVE_YN - C# 이름은 그대로 유지(하위호환)
    public bool DeleteYn { get; set; }
    public bool PrintYn { get; set; } = true;
    public bool ExcelYn { get; set; } = true;

    /// <summary>TSMMENUAUTH.AUTH01~10 - 조회/입력/저장/출력/엑셀 이외에 화면마다 추가로
    /// 분리해야 하는 권한이 생겼을 때 쓰는 예비 슬롯(사장님 지시, 2026-08-31). 인덱스
    /// 0=AUTH01 ... 9=AUTH10. 화면 개발자는 BaseForm.Auth[]로 그대로 쓰면 된다.</summary>
    public bool[] Auth { get; set; } = new bool[10];
}
