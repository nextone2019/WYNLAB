namespace WYNLAB.Shared.Dtos;

/// <summary>
/// LookUp(콤보) 프레임워크(sysLookupM/sysLookupP) DTO들 - 팝업 프레임워크와 같은 발상을
/// 콤보박스에 적용한 것. 화면에서는 LookUpEditWyn.LookupKey로 이름만 참조하면 되고, 실제
/// 프로시져/파라미터/값필드/표시필드는 여기 정의된 대로 서버가 알아서 실행/매핑한다.
/// 설계 배경은 프로젝트 메모리(project_wynlab_popup_lookup_framework) 참고.
/// </summary>
public class LookupDefinitionDto
{
    public string LookupKey { get; set; } = string.Empty;
    public string ProcNm { get; set; } = string.Empty;
    public string LookupNm { get; set; } = string.Empty;
    public string ValueField { get; set; } = string.Empty;
    public string DisplayField { get; set; } = string.Empty;
    public List<LookupParamDto> Params { get; set; } = new();
}

/// <summary>LookUp 프로시져가 받는 파라미터 하나 - 화면에 입력창으로 그려지는 게 아니라(팝업의
/// 조회조건과 다름), 그 LookUp을 쓰는 화면 코드가 LookUpEditWyn.SetParam()으로 값을 채워준다.
/// Caption은 "이 파라미터가 뭘 의미하는지" 관리 화면에서 참고하는 설명 문구일 뿐이다.</summary>
public class LookupParamDto
{
    public string ParamNm { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int Sort { get; set; }
}

/// <summary>LookUp 조회 결과 한 항목 - Value/Display는 항상 채워지는 "값(코드) + 표시값(명칭)"
/// 한 쌍(기존 하위호환용, sysLookupC 설정이 없는 LookUp도 그대로 동작). Row는 결과셋의 전체
/// 컬럼 원본값 - sysLookupC에 컬럼을 여러 개 설정해서 팝업에 값필드/표시필드 이상을 보여줄 때만
/// 실제로 쓰인다(LookUpEditWyn.LoadFromLookupKeyAsync 참고).</summary>
public class LookupItemDto
{
    public string Value { get; set; } = string.Empty;
    public string Display { get; set; } = string.Empty;
    public Dictionary<string, string?> Row { get; set; } = new();
}

/// <summary>sysLookupC 한 행 - LookUp 팝업에 값필드/표시필드 외에 추가로 보여줄 컬럼 구성.
/// 이 목록이 비어있으면(설정 안 한 LookUp) 예전 그대로 값필드/표시필드 2컬럼 고정으로 동작한다 -
/// 완전히 하위호환(2026-09-03, 038_Lookup_Framework.sql 당시엔 일부러 안 뒀던 것을 팝업 프레임워크
/// (sysPopUpD)만큼 유연하게 확장).</summary>
public class LookupColumnDefDto
{
    public string ColumnNm { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int Sort { get; set; }
    public int Width { get; set; } = 100;
}

/// <summary>api/combo-lookups/{key}/items 응답 - 컬럼 구성(sysLookupC, 없으면 빈 리스트)과 실제
/// 항목을 한 번에 내려준다. 컬럼 구성은 조회할 때마다 안 바뀌지만 그렇다고 별도 API 왕복을 늘리지
/// 않으려고(팝업 프레임워크의 /definition + /search 두 호출과 다르게) 항목 조회에 얹어 보낸다.</summary>
public class LookupItemsResultDto
{
    public List<LookupColumnDefDto> Columns { get; set; } = new();
    public List<LookupItemDto> Items { get; set; } = new();
}
