namespace WYNLAB.Shared.Dtos;

/// <summary>
/// MENU 테이블 1:1 매핑 DTO.
/// Shell의 좌측 AccordionControl을 이 목록으로 동적 구성한다.
/// </summary>
public class MenuDto
{
    public long MenuId { get; set; }
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM"; // FORM / GROUP

    /// <summary>리플렉션으로 화면을 로딩할 때 쓴다 - Module="SM"/ScreenClassNm="frmMenu"를
    /// "WYNLAB.{Module}.{ScreenClassNm}, WYNLAB.{Module}" 형태로 조합해서 Type.GetType()에 넘긴다
    /// (ShellForm.OpenMenuForm 참고). 예전엔 이 조합된 문자열 자체를 FORM_CLASS_NM 한 컬럼에
    /// 저장했는데, 두 값이 사실 독립적인 정보(모듈/클래스명)라 분리했다.</summary>
    public string? Module { get; set; }
    public string? ScreenClassNm { get; set; }
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
