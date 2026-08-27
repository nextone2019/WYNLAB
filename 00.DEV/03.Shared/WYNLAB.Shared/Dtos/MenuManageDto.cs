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
}
