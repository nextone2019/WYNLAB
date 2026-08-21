namespace WYNLAB.Shared.Dtos;

/// <summary>권한부여관리 화면 - 메뉴 1건 + 이 대상(사용자 또는 그룹)에게 직접 걸린 권한</summary>
public class MenuAuthItemDto
{
    public string MenuCd { get; set; } = string.Empty;
    public string MenuNm { get; set; } = string.Empty;
    public string? UpperMenuCd { get; set; }
    public string MenuType { get; set; } = "FORM"; // FORM / GROUP
    public int SortOrder { get; set; }
    public bool ViewYn { get; set; }
    public bool InsertYn { get; set; }
    public bool UpdateYn { get; set; }
    public bool DeleteYn { get; set; }
    public bool ExcelYn { get; set; }
}

/// <summary>권한 저장 요청 - TargetType은 "USER" 또는 "GRP"</summary>
public class SaveMenuAuthRequest
{
    public string TargetType { get; set; } = "USER";
    public string TargetCd { get; set; } = string.Empty;
    public List<MenuAuthItemDto> Items { get; set; } = new();
}
