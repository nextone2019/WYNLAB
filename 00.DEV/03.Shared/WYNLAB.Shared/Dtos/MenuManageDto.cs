namespace WYNLAB.Shared.Dtos;

/// <summary>메뉴 목록/상세 조회용 - 로그인 세션(MenuDto)과 별개, 관리화면 전용</summary>
public class MenuListItemDto
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? FormClassNm { get; set; }
    public string? IconNm { get; set; }
    public int SortOrder { get; set; }
    public bool UseYn { get; set; }

    /// <summary>TSMMENU.AUTH01_NM~10_NM - 이 메뉴에서 AUTH01~10을 어떤 의미로 쓸지 정의하는
    /// 캡션(사장님 지시, 2026-08-31). 사용자권한관리(frmUserAuth)에서 이 메뉴를 클릭하면 우측
    /// AUTH01~10 패널에 그대로 표시된다. 인덱스 0=AUTH01_NM ... 9=AUTH10_NM.</summary>
    public string?[] AuthNm { get; set; } = new string?[10];
}

public class MenuCreateRequest
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public int MenuLevel { get; set; } = 1;
    public string MenuType { get; set; } = "FORM";
    public string? FormClassNm { get; set; }
    public string? IconNm { get; set; }
    public int SortOrder { get; set; }
    public string?[] AuthNm { get; set; } = new string?[10];
}

/// <summary>MENU_CD는 PK라 수정 불가 - 나머지 항목만 변경 가능</summary>
public class MenuUpdateRequest
{
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? FormClassNm { get; set; }
    public string? IconNm { get; set; }
    public int SortOrder { get; set; }
    public bool UseYn { get; set; }
    public string?[] AuthNm { get; set; } = new string?[10];
}
