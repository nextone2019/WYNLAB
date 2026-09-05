namespace WYNLAB.Shared.Dtos;

/// <summary>
/// 범용 데이터 통로(api/data/query, api/data/save) 요청. 화면이 "어느 메뉴에서, 어떤 프로시저를,
/// 어떤 파라미터로" 부를지 담아 보낸다. 설계 배경은 저장소 루트의 GENERIC_DATA_API.md 참고.
///
/// 화면마다 Controller/Repository를 따로 만들지 않기 위한 구조다. 업무 로직이 전부 저장프로시저에
/// 있고 API는 값을 옮겨주는 통로일 뿐인데, 그 통로를 화면 수만큼 손으로 만들다 보니 검색조건 하나
/// 추가하는 요청에도 서버 배포가 따라왔다.
/// </summary>
public class DataRequest
{
    /// <summary>호출하는 화면의 메뉴ID(TSMMENU.MENU_ID). 서버가 이 값으로 두 가지를 확인한다 -
    /// ① 로그인 사용자가 이 메뉴에 대한 권한이 있는지 ② 요청한 프로시저가 이 메뉴에 등록된
    /// 것인지(TSMMENU.PROC_PREFIX). 화면에서 직접 채우지 않아도 BaseForm이 자기 MenuId로 채워준다.</summary>
    public long MenuId { get; set; }

    /// <summary>실행할 저장프로시저 이름(예: "USP_SM_MINORCODE_Q").
    /// 아무 프로시저나 부를 수 있으면 안 되므로, 서버가 MenuCd에 등록된 접두사로 시작하는지 검사한다.</summary>
    public string ProcName { get; set; } = string.Empty;

    /// <summary>프로시저에 넘길 파라미터. 키는 프로시저의 파라미터 이름 그대로(@는 빼고) 쓴다.
    /// 예: { "p_work_type": "Q", "p_major_cd": "CM0001" }
    ///
    /// 값이 문자열인 이유: 화면에서 오는 값은 결국 텍스트박스/콤보 값이고, 숫자·날짜 변환은
    /// 프로시저 파라미터 타입이 알아서 한다. JSON에서 타입을 섞으면 클라이언트마다 직렬화가
    /// 달라져 오히려 헷갈린다. null은 그대로 null로 전달된다(프로시저의 기본값이 적용되지 않고
    /// NULL이 들어가므로, 조건 무시는 프로시저에서 IS NULL로 처리한다).</summary>
    public Dictionary<string, string?> Params { get; set; } = new();
}

/// <summary>
/// 조회 응답. 프로시저가 결과셋을 여러 개 돌려주는 경우(대분류+소분류를 한 번에 주는 식)가 있어
/// 배열로 담는다. 화면은 보통 Tables[0]을 그리드에 바인딩한다.
///
/// 행을 DTO가 아니라 Dictionary로 돌려주는 이유: 화면마다 DTO를 만들지 않는 것이 이 구조의
/// 목적이기 때문이다. 그리드 컬럼이 디자이너에서 FieldName 문자열("minor_cd")로 잡혀 있어서
/// 타입 없는 행을 그대로 바인딩해도 지금과 똑같이 동작한다.
/// </summary>
public class DataQueryResponse
{
    public List<DataTableResult> Tables { get; set; } = new();
}

/// <summary>
/// 결과셋 하나. Columns를 Rows와 별도로 들고 있는 이유: 행이 0건이면 Rows에서 컬럼 이름을 유추할
/// 방법이 없다 - 예를 들어 소분류가 하나도 없는 대분류를 조회하면 Rows는 빈 배열인데, 컬럼 정보가
/// 없으면 클라이언트가 스키마 없는 DataTable을 그리드에 바인딩하게 되어 그 상태에서 새 행을 추가해
/// 입력해도 어느 필드에도 값이 안 붙어 포커스를 옮기면 그대로 사라진다(실제로 겪음). 그래서 서버가
/// 행 개수와 무관하게 항상 컬럼 이름을 같이 내려준다.
/// </summary>
public class DataTableResult
{
    public List<string> Columns { get; set; } = new();
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
}
