namespace WYNLAB.Shared.Dtos;

/// <summary>메뉴 목록/상세 조회용 - 로그인 세션(MenuDto)과 별개, 관리화면 전용</summary>
public class MenuListItemDto
{
    public long MenuId { get; set; }
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? Module { get; set; }
    public string? ScreenClassNm { get; set; }
    public string? IconNm { get; set; }

    /// <summary>범용 데이터 통로(api/data/*)가 이 메뉴에서 호출을 허용할 프로시저 접두사
    /// (DataController.ValidateAsync ② 단계) - 비어있으면 이 메뉴로는 api/data/* 저장/조회 자체가
    /// 전부 거부된다("이 메뉴는 범용 데이터 통로를 사용하도록 설정되어 있지 않습니다"). FORM
    /// 타입 메뉴는 사실상 필수.</summary>
    public string? ProcPrefix { get; set; }

    public int SortOrder { get; set; }
    public bool UseYn { get; set; }

    /// <summary>TSMMENU.AUTH01_NM~10_NM - 이 메뉴에서 AUTH01~10을 어떤 의미로 쓸지 정의하는
    /// 캡션(사장님 지시, 2026-08-31). 사용자권한관리(frmUserAuth)에서 이 메뉴를 클릭하면 우측
    /// AUTH01~10 패널에 그대로 표시된다. 인덱스 0=AUTH01_NM ... 9=AUTH10_NM.</summary>
    public string?[] AuthNm { get; set; } = new string?[10];
}

/// <summary>MENU_ID는 이제 IDENTITY라 화면에서 직접 안 넣는다 - 등록 후 서버가 채번해서
/// ApiResult.GeneratedCode로 돌려준다.</summary>
public class MenuCreateRequest
{
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public int MenuLevel { get; set; } = 1;
    public string MenuType { get; set; } = "FORM";
    public string? Module { get; set; }
    public string? ScreenClassNm { get; set; }
    public string? IconNm { get; set; }
    public string? ProcPrefix { get; set; }
    public int SortOrder { get; set; }
    public string?[] AuthNm { get; set; } = new string?[10];
}

/// <summary>MENU_ID는 PK라 수정 불가 - 나머지 항목만 변경 가능(어느 행인지는 URL의 menuId로 지정)</summary>
public class MenuUpdateRequest
{
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public int MenuLevel { get; set; }
    public string MenuType { get; set; } = "FORM";
    public string? Module { get; set; }
    public string? ScreenClassNm { get; set; }
    public string? IconNm { get; set; }
    public string? ProcPrefix { get; set; }
    public int SortOrder { get; set; }
    public bool UseYn { get; set; }
    public string?[] AuthNm { get; set; } = new string?[10];
}
