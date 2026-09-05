namespace WYNLAB.SYS;

/// <summary>템플릿 종류 - 전부 panHeader(검색조건)는 공통이고 본문 구조만 다르다.
/// - SingleGrid: grd1 하나(인라인 편집, 저장버튼이 변경된 행을 일괄 저장) - 가장 단순한 목록화면.
/// - MasterSubGrid: grd1(마스터, 조회전용) + grd2(grd1 선택행에 따라 재조회) - 그리드 2개,
///   상세입력폼은 없음(frmDept.cs 패턴).
/// - MasterFormSubGrid: grd1(마스터, 조회전용 목록) + panData(선택행 상세편집폼) + grd2(하위
///   인라인편집 그리드) - 기초코드등록(frmMinorCode) 패턴, 화면 하나에 목록/상세/하위목록이
///   전부 있는 가장 완전한 형태.
/// - MasterFormTabGrid: MasterFormSubGrid와 같은 grd1+panData 구조에, 하위 그리드가 1개가
///   아니라 탭으로 묶인 2개(grd2/grd3)다. grd2/grd3는 조회전용이 아니라 편집 가능하고 각자
///   자기 저장프로시저로 저장된다(거래처+담당자+계좌 같은 1:N:N 구조, frmCust 패턴).</summary>
public enum TemplateKind
{
    SingleGrid,
    MasterSubGrid,
    MasterFormSubGrid,
    MasterFormTabGrid
}

/// <summary>AI Builder가 생성할 화면 하나를 기술하는 값 - describe-proc 결과를 사람이
/// 화면에서 다듬은 뒤 이 형태로 모아서 ScreenTemplateGenerator에 넘긴다.
///
/// 컬럼/파라미터는 템플릿 종류와 무관하게 전부 QuerySources/SaveActions(아래) 하나의 모델로만
/// 표현한다(2026-09-04 통합 - 예전엔 템플릿마다 QueryProc/MasterColumns/SaveParamColumnMap 같은
/// 별도 필드를 썼는데, 결국 "그리드/폼 몇 개, 저장 몇 번"만 다를 뿐 같은 모델이라 하나로
/// 합쳤다). 생성기는 TargetSlot/SourceSlot("grd1"/"grd2"/"grd3"/"panData")으로 필요한 걸 찾는다.</summary>
public class ScreenGenSpec
{
    public string Module { get; set; } = "SM";
    public string ScreenClassNm { get; set; } = string.Empty; // "frmXxx"
    public string MenuCaption { get; set; } = string.Empty;
    public long? UpperMenuId { get; set; }
    public string ProcPrefix { get; set; } = string.Empty;

    public TemplateKind Kind { get; set; } = TemplateKind.SingleGrid;

    // 아래 세 필드는 AI Builder 화면 상단 입력칸 그대로 - Describe Query/Save 버튼이 이 값으로
    // QuerySources/SaveActions를 자동으로 채우는 데 쓰고, 생성 코드의 __QUERY_PROC__/
    // __SAVE_PROC__/__DETAIL_WORK_TYPE__ 토큰 치환에도 그대로 쓰인다(ApplyTokens 참고).
    public string QueryProc { get; set; } = string.Empty;
    public string? DetailWorkType { get; set; } // 기본 "Q1"
    public string? SaveProc { get; set; }

    /// <summary>조회프로시저의 파라미터(p_ 뗀 이름, p_work_type 제외) - panHeader 검색조건
    /// 텍스트박스로 하나씩 만들어진다.</summary>
    public List<string> QueryParams { get; set; } = new();

    /// <summary>마스터 그리드에서 선택된 행의 이 컬럼 값을 서브 조회의 유일한 필터 파라미터로
    /// 넘긴다(frmDept.cs 패턴) - grd1 컬럼 중 하나여야 한다.</summary>
    public string? MasterKeyColumn { get; set; }

    /// <summary>MasterKeyColumn 값을 서브 조회 시 어느 파라미터(p_ 뗀 이름)로 보낼지 - 같은
    /// 프로시저를 Q/Q1으로 나눠 쓰므로 파라미터 목록 자체는 Q/Q1 공통이다.</summary>
    public string? DetailKeyParam { get; set; }

    public List<string> SearchParams() => QueryParams;

    /// <summary>조회 - 프로시저/워크타입 조합마다 레코드셋을 1개 이상 반환할 수 있고, 그 각각을
    /// ResultSetBindings로 화면의 슬롯(그리드/폼 컨트롤, 예: "grd1"/"grd2"/"grd3")에 바인딩한다.
    /// 순서대로 실행되지는 않는다(전부 조회 시점에 독립적으로 호출) - 슬롯 이름이 실제로 그
    /// 템플릿에 존재하는지는 생성기가 검증한다.</summary>
    public List<QuerySource> QuerySources { get; set; } = new();

    /// <summary>저장 - Save 클릭 시 이 순서대로 실행한다(Header를 먼저 두면 그 결과 키를 뒤이은
    /// Detail의 KeyParam으로 넘길 수 있다). 템플릿에 따라 비어있을 수 있다(MasterSubGrid는
    /// 조회전용이라 Save Proc을 안 채우면 SaveActions가 비고, 그러면 "조회전용"으로 생성된다).</summary>
    public List<SaveAction> SaveActions { get; set; } = new();
}

/// <summary>조회프로시저 1번 호출(하나의 work_type 분기) - 레코드셋을 1개 이상 반환할 수 있고,
/// 그 각각을 ResultSetBindings로 화면의 슬롯(그리드/폼 컨트롤 필드명, 예: "grd1"/"panData")에
/// 바인딩한다. 슬롯 이름이 실제로 그 템플릿에 존재하는지는 생성기가 검증한다.</summary>
public class QuerySource
{
    public string ProcName { get; set; } = string.Empty;
    public string WorkType { get; set; } = "Q";
    public List<ResultSetBinding> ResultSetBindings { get; set; } = new();
}

/// <summary>QuerySource가 반환하는 레코드셋 중 하나 - 순서(Index, describe-proc-multi가 매긴
/// 0-based 순번)와 그걸 받을 컨트롤(TargetSlot), 그 레코드셋의 컬럼 목록.</summary>
public class ResultSetBinding
{
    public int ResultSetIndex { get; set; }
    public string TargetSlot { get; set; } = string.Empty;
    public List<ColumnSpec> Columns { get; set; } = new();
}

/// <summary>Header = 슬롯(폼) 값을 1번만 저장(신규/수정은 화면의 편집모드로 판단).
/// Detail = 슬롯(그리드)의 변경된 행마다 반복 저장(RowState 기준 N/U/D - SingleGrid의
/// 기존 저장 루프와 동일한 방식).</summary>
public enum SaveActionScope { Header, Detail }

/// <summary>저장프로시저 1번 호출 - 소스 컨트롤(SourceSlot) 하나의 값만 파라미터로 쓴다.
/// 화면 하나에 헤더+명세처럼 저장 대상이 여러 컨트롤에 걸쳐 있으면 SaveAction을 여러 개 둔다.</summary>
public class SaveAction
{
    public string ProcName { get; set; } = string.Empty;
    public SaveActionScope Scope { get; set; } = SaveActionScope.Detail;
    public string SourceSlot { get; set; } = "grd1";

    public List<string> SaveParams { get; set; } = new();
    public Dictionary<string, string> SaveParamColumnMap { get; set; } = new();

    /// <summary>Detail scope에서 상위(Header) SaveAction이 만든 키 값을 넘겨 받을 파라미터명
    /// (p_ 뗀 이름) - 이 SaveAction보다 앞선 Header SaveAction의 결과(ProcResult.GeneratedCode
    /// 또는 화면의 편집중 키)를 그대로 채운다. 필요 없으면 null.</summary>
    public string? KeyParam { get; set; }

    public List<string> UnmappedSaveParams() => SaveParams.Where(p => !SaveParamColumnMap.ContainsKey(p)).ToList();
}

public class ColumnSpec
{
    public string Name { get; set; } = string.Empty;      // 실제 DB 컬럼명(FieldName)
    public string SqlType { get; set; } = "nvarchar";
    public string Caption { get; set; } = string.Empty;
    public bool IncludeInGrid { get; set; } = true;
    public bool IsKey { get; set; }

    /// <summary>TEXT/CHECK/NUMBER/COMBO 중 하나 - COMBO면 LookupKey로 지정된 LookUp에 연결된다.</summary>
    public string ControlKind { get; set; } = "TEXT";

    /// <summary>ControlKind가 COMBO일 때만 쓰인다 - sysLookupM.lookup_key(frmSysLookup에서 등록한
    /// LookUp 키). 그리드 컬럼은 LookUpColumnEdit, panData 컨트롤은 LookUpEditWyn으로 이 값에
    /// 연결된다.</summary>
    public string? LookupKey { get; set; }
}
