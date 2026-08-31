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

/// <summary>LookUp 조회 결과 한 항목 - "값(코드) + 표시값(명칭)" 한 쌍. 서버가 sysLookupM의
/// value_field/display_field를 이용해 실제 결과셋에서 이 모양으로 뽑아준다.</summary>
public class LookupItemDto
{
    public string Value { get; set; } = string.Empty;
    public string Display { get; set; } = string.Empty;
}
