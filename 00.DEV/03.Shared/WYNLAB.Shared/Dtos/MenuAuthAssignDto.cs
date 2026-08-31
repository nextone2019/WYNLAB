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
    public bool PrintYn { get; set; }
    public bool ExcelYn { get; set; }

    /// <summary>AUTH01~10 권한값 - frmUserAuth의 tree1/tree2에 조회/입력/저장/삭제와 같은 방식의
    /// 체크 컬럼(FieldName="Auth01".."Auth10")으로 바인딩된다. 개별 이름 프로퍼티인 이유는
    /// TreeList의 FieldName 바인딩이 배열 인덱스(Auth[0] 등)를 지원하지 않기 때문 - ViewYn 등
    /// 나머지 권한 컬럼과 동일한 패턴을 그대로 따른 것.</summary>
    public bool Auth01 { get; set; }
    public bool Auth02 { get; set; }
    public bool Auth03 { get; set; }
    public bool Auth04 { get; set; }
    public bool Auth05 { get; set; }
    public bool Auth06 { get; set; }
    public bool Auth07 { get; set; }
    public bool Auth08 { get; set; }
    public bool Auth09 { get; set; }
    public bool Auth10 { get; set; }

    /// <summary>TSMMENU.AUTH01_NM~10_NM - 메뉴등록(frmMenu)에서 정의한 이 메뉴의 AUTH01~10
    /// 캡션(조회 전용, 저장 시 무시됨 - 캡션은 frmMenu에서만 바꾼다). 비어있으면 화면에서
    /// "Auth01"처럼 기본 표기로 대체한다.</summary>
    public string?[] AuthNm { get; set; } = new string?[10];
}

/// <summary>권한 저장 요청 - TargetType은 "USER" 또는 "GRP"</summary>
public class SaveMenuAuthRequest
{
    public string TargetType { get; set; } = "USER";
    public string TargetCd { get; set; } = string.Empty;
    public List<MenuAuthItemDto> Items { get; set; } = new();
}
