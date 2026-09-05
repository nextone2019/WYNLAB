namespace WYNLAB.Shared.Dtos;

/// <summary>권한부여관리 화면 - 메뉴 1건 + 이 대상(사용자 또는 그룹)에게 직접 걸린 권한</summary>
public class MenuAuthItemDto
{
    public long MenuId { get; set; }
    public string MenuNm { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
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

/// <summary>메뉴기준 권한관리(frmMenuAuth) 화면 전용 - MenuAuthItemDto의 반대 축. 메뉴 1건을
/// 고정하고, 사용자(또는 사용자그룹) 1명이 그 메뉴에 대해 가진 권한을 담는다(그리드 1행 = 대상
/// 1명). TargetNm/SubNm은 화면 표시 전용(저장 시 무시) - 사용자 탭은 이름/부서명, 사용자그룹
/// 탭은 그룹명/인원수를 담는다.</summary>
public class MenuAuthByMenuItemDto
{
    public string TargetCd { get; set; } = string.Empty;
    public string TargetNm { get; set; } = string.Empty;
    public string? SubNm { get; set; }
    public bool ViewYn { get; set; }
    public bool InsertYn { get; set; }
    public bool UpdateYn { get; set; }
    public bool DeleteYn { get; set; }
    public bool PrintYn { get; set; }
    public bool ExcelYn { get; set; }
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
}

/// <summary>메뉴기준 권한관리 저장 요청 - 선택된 메뉴 1건 + 대상유형(USER/GRP) 기준으로 그
/// 조합의 TSMMENUAUTH 행을 전체치환한다(USP_SM_MENUAUTH_S_1과 같은 방식, 축만 메뉴 하나로 고정).</summary>
public class SaveMenuAuthByMenuRequest
{
    public long MenuId { get; set; }
    public string TargetType { get; set; } = "USER";
    public List<MenuAuthByMenuItemDto> Items { get; set; } = new();
}
