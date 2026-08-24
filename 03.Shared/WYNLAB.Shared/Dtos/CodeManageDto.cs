namespace WYNLAB.Shared.Dtos;

/// <summary>
/// 대분류코드 목록/상세 조회용 - TSMMAJOR 1:1. 프로퍼티명은 DB 컬럼명(snake_case) 그대로 쓴다 -
/// SQL 컬럼 <-> DTO 프로퍼티 <-> 화면 컨트롤 이름(txtmajor_nm 등)이 전부 한 가지 표기로 이어지게
/// 하기로 한 컨벤션(2026-08-23 결정, 기초코드등록부터 적용 - 이전 화면들은 PascalCase 유지).
/// </summary>
public class MajorListItemDto
{
    public string major_cd { get; set; } = string.Empty;
    public string? major_nm { get; set; }
    // VARCHAR(1) 'Y'/'N' 컬럼 - bool로 선언하면 Dapper가 "N"을 bool로 자동변환 못 해서
    // QueryAsync가 FormatException으로 죽는다(실제로 겪음). 다른 DTO(UserManageRow.UseYn 등)도
    // 전부 이 이유로 string을 쓴다 - 여기만 빠뜨렸었음.
    public string sys_yn { get; set; } = "N";
    public string? rel_cd1 { get; set; }
    public string? rel_title1 { get; set; }
    public string? rel_cd_type1 { get; set; }
    public string? rel_cd2 { get; set; }
    public string? rel_title2 { get; set; }
    public string? rel_cd_type2 { get; set; }
    public string? rel_cd3 { get; set; }
    public string? rel_title3 { get; set; }
    public string? rel_cd_type3 { get; set; }
    public string? rel_cd4 { get; set; }
    public string? rel_title4 { get; set; }
    public string? rel_cd_type4 { get; set; }
    public string? rel_cd5 { get; set; }
    public string? rel_title5 { get; set; }
    public string? rel_cd_type5 { get; set; }
    public string? rel_cd6 { get; set; }
    public string? rel_title6 { get; set; }
    public string? rel_cd_type6 { get; set; }
    public string? rel_cd7 { get; set; }
    public string? rel_title7 { get; set; }
    public string? rel_cd_type7 { get; set; }
    public string? rel_cd8 { get; set; }
    public string? rel_title8 { get; set; }
    public string? rel_cd_type8 { get; set; }
    public string? rel_cd9 { get; set; }
    public string? rel_title9 { get; set; }
    public string? rel_cd_type9 { get; set; }
    public string? rel_cd10 { get; set; }
    public string? rel_title10 { get; set; }
    public string? rel_cd_type10 { get; set; }
    public string? remark { get; set; }
}

/// <summary>대분류코드 신규등록/수정 공통 필드 - major_cd는 등록시엔 본문, 수정시엔 라우트로 받음</summary>
public class MajorSaveRequest
{
    public string major_cd { get; set; } = string.Empty;
    public string? major_nm { get; set; }
    public bool sys_yn { get; set; }
    public string? rel_cd1 { get; set; }
    public string? rel_title1 { get; set; }
    public string? rel_cd_type1 { get; set; }
    public string? rel_cd2 { get; set; }
    public string? rel_title2 { get; set; }
    public string? rel_cd_type2 { get; set; }
    public string? rel_cd3 { get; set; }
    public string? rel_title3 { get; set; }
    public string? rel_cd_type3 { get; set; }
    public string? rel_cd4 { get; set; }
    public string? rel_title4 { get; set; }
    public string? rel_cd_type4 { get; set; }
    public string? rel_cd5 { get; set; }
    public string? rel_title5 { get; set; }
    public string? rel_cd_type5 { get; set; }
    public string? rel_cd6 { get; set; }
    public string? rel_title6 { get; set; }
    public string? rel_cd_type6 { get; set; }
    public string? rel_cd7 { get; set; }
    public string? rel_title7 { get; set; }
    public string? rel_cd_type7 { get; set; }
    public string? rel_cd8 { get; set; }
    public string? rel_title8 { get; set; }
    public string? rel_cd_type8 { get; set; }
    public string? rel_cd9 { get; set; }
    public string? rel_title9 { get; set; }
    public string? rel_cd_type9 { get; set; }
    public string? rel_cd10 { get; set; }
    public string? rel_title10 { get; set; }
    public string? rel_cd_type10 { get; set; }
    public string? remark { get; set; }
}

/// <summary>소분류코드 목록/저장 겸용 - TSMMINOR 1:1 (전체 치환 저장이라 조회/저장에 같은 모양 재사용)</summary>
public class MinorItemDto
{
    public string minor_cd { get; set; } = string.Empty;
    public string? minor_nm { get; set; }
    public int sort { get; set; }
    // VARCHAR(1) 'Y'/'N' 컬럼 - MajorListItemDto.sys_yn과 같은 이유로 string (Dapper 조회 시
    // bool 자동변환 실패 + JSON 저장 시에도 true/false 문자열이 그대로 들어가 OPENJSON의
    // VARCHAR(1) 컬럼에 잘려 들어가는 문제가 있었음).
    public string sys_yn { get; set; } = "N";
    public string use_yn { get; set; } = "Y";
    public string? rel_cd1 { get; set; }
    public string? rel_cd2 { get; set; }
    public string? rel_cd3 { get; set; }
    public string? rel_cd4 { get; set; }
    public string? rel_cd5 { get; set; }
    public string? rel_cd6 { get; set; }
    public string? rel_cd7 { get; set; }
    public string? rel_cd8 { get; set; }
    public string? rel_cd9 { get; set; }
    public string? rel_cd10 { get; set; }
    public string? remark { get; set; }
}

/// <summary>소분류 그리드 전체 치환 저장 요청 - Items는 특정 컬럼을 가리키는 게 아니라 목록
/// 컨테이너라서 기존 관례(PascalCase)를 그대로 유지한다.</summary>
public class SaveMinorsRequest
{
    public List<MinorItemDto> Items { get; set; } = new();
}

/// <summary>범용 코드 LookUp 항목(USP_SM_CODE_LOOKUP_Q 결과) - LookUpEditWyn.MajorCd가 이 DTO로
/// 응답을 받아서 WYNLAB.Controls의 CodeLookupItem(Value/Display)으로 매핑한다.</summary>
public class CodeLookupItemDto
{
    public string minor_cd { get; set; } = string.Empty;
    public string? minor_nm { get; set; }
}

/// <summary>대분류 조회 응답 - 리스트 + (선택된 대분류의) 소분류 리스트를 한 번에.
/// Majors/Minors도 컬럼이 아니라 컨테이너라 PascalCase 유지.</summary>
public class CodeQueryResponse
{
    public List<MajorListItemDto> Majors { get; set; } = new();
    public List<MinorItemDto> Minors { get; set; } = new();
}
