namespace WYNLAB.Shared.Dtos;

/// <summary>
/// 팝업 프레임워크(sysPopUpM/sysPopUpD) DTO들. 화면 어디서든 코드/명 필드 하나에 "..." 버튼으로
/// 붙는 검색 팝업(부서/품목/거래처...)을 정의/사용하기 위한 것들이다. 설계 배경은 프로젝트
/// 메모리(project_wynlab_popup_lookup_framework) 참고.
/// </summary>
public class PopupDefinitionDto
{
    public string PopupKey { get; set; } = string.Empty;
    public string ProcNm { get; set; } = string.Empty;
    public string PopupNm { get; set; } = string.Empty;
    public bool HierarchicalYn { get; set; }
    public string KeyField { get; set; } = string.Empty;
    public string? ParentField { get; set; }
    public string DisplayField { get; set; } = string.Empty;
    public int PopupWidth { get; set; } = 700;
    public int PopupHeight { get; set; } = 500;
    public List<PopupColumnDto> Columns { get; set; } = new();
    public List<PopupSearchFieldDto> SearchFields { get; set; } = new();
}

/// <summary>팝업 그리드/트리의 컬럼 하나. ControlType: TEXT/DATE/LOOKUP/CHECK.</summary>
public class PopupColumnDto
{
    public string ColumnNm { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string ControlType { get; set; } = "TEXT";
    public string? LookupProcNm { get; set; }
    public int Sort { get; set; }
    public int Width { get; set; } = 100;
    public bool VisibleYn { get; set; } = true;
}

/// <summary>팝업 상단 검색창의 조회조건 하나 - 프로시저마다 파라미터명/개수가 전부 다르므로
/// (모든 팝업이 @p_keyword 하나로 통일된다는 가정을 버림) 팝업별로 sysPopUpS에 저장해둔다.
/// ParamNm은 실제 프로시저 파라미터명에서 앞의 '@'를 뗀 것(GenericDataRepository에 넘길 때
/// 그대로 딕셔너리 키로 쓴다 - column_nm이 '@' 없이 저장되는 것과 같은 컨벤션).</summary>
public class PopupSearchFieldDto
{
    public string ParamNm { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string ControlType { get; set; } = "TEXT";
    public int Sort { get; set; }
    public int Width { get; set; } = 100;
}

/// <summary>"컬럼생성" 버튼 - 프로시저를 실행하지 않고 결과셋 구조만 읽어온 것(sys.dm_exec_describe_first_result_set).</summary>
public class ProcColumnInfoDto
{
    public string ColumnNm { get; set; } = string.Empty;
    public string SqlType { get; set; } = string.Empty;

    /// <summary>SQL 타입으로 추정한 기본 컨트롤타입(date/datetime류 -> DATE, 그 외 -> TEXT) -
    /// 관리 화면에서 그대로 저장해도 되고, 필요하면 사람이 LOOKUP 등으로 바꾸면 된다.</summary>
    public string SuggestedControlType { get; set; } = "TEXT";
}

/// <summary>"컬럼생성" 버튼 - 프로시저의 입력 파라미터 구조(sys.parameters). 결과셋 컬럼과 별개로
/// 조회조건(sysPopUpS) 그리드를 채우는 데 쓴다.</summary>
public class ProcParamInfoDto
{
    public string ParamNm { get; set; } = string.Empty;
    public string SqlType { get; set; } = string.Empty;
    public string SuggestedControlType { get; set; } = "TEXT";
}

/// <summary>"컬럼생성" 버튼 한 번 클릭으로 결과셋 컬럼 + 입력 파라미터를 한 번에 받아온다
/// (프로시저 하나를 introspect하는 김에 왕복 한 번으로 끝내기 위함).</summary>
public class DescribeProcResultDto
{
    public List<ProcColumnInfoDto> Columns { get; set; } = new();
    public List<ProcParamInfoDto> Params { get; set; } = new();
}
