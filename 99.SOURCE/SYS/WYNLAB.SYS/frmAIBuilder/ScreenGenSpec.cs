namespace WYNLAB.SYS;

/// <summary>템플릿 종류 - 전부 panHeader(검색조건)는 공통이고 본문 구조만 다르다.
/// - SingleGrid: grd1 하나(인라인 편집, 저장버튼이 변경된 행을 일괄 저장) - 가장 단순한 목록화면.
/// - MasterSubGrid: grd1(마스터, 조회전용) + grd2(grd1 선택행에 따라 재조회) - 그리드 2개,
///   상세입력폼은 없음(frmDept.cs 패턴).
/// - MasterFormSubGrid: grd1(마스터, 조회전용 목록) + panData(선택행 상세편집폼) + 탭으로 묶인
///   하위 인라인편집 그리드 1~2개(grd2/grd3, 둘 다 편집 가능하고 각자 자기 저장프로시저로
///   저장된다) - 기초코드등록/거래처등록(frmMinorCode/frmCust) 패턴, 화면 하나에 목록/상세/
///   하위목록이 전부 있는 가장 완전한 형태. grd3는 선택사항이라 하위그리드가 1개뿐인 화면도
///   이걸로 만든다(2026-09-08 - 예전엔 grd2 하나만 조회전용으로 만드는 "MasterFormSubGrid"와
///   grd2/grd3 둘 다 편집 가능한 "MasterFormTabGrid"가 별도 템플릿이었는데, 후자가 전자를 완전히
///   포함하는 상위호환이라 하나로 합쳤다 - grd3/grd2저장을 안 쓰면 생성 후 그 탭/저장액션만 지우면
///   예전 MasterFormSubGrid와 동일해진다).
/// - MasterOneSheet: grd1(마스터 목록 그리드) 자체가 없다(2026-09-09 재설계 - "grd1이 없는
///   모습"). panHeader(검색조건)에 키를 입력하고 툴바 조회를 누르면 그 문서 1건이 곧바로
///   panData(문서 자체)에 채워진다 - grd1에서 행을 고르는 중간 단계가 없다. 하위 그리드
///   (grd2~grd5, 탭)는 MasterFormSubGrid와 달리 전부 선택사항이다(0~4개, 예전엔 하위그리드가
///   아예 없었는데 지금은 있어도/없어도 된다). QuerySources 쪽에서는 여전히 "grd1"을 문서
///   헤더 레코드셋의 슬롯 이름으로 쓰지만(도구 내부 규약, 실제 그리드는 없음), grd1~grd5가
///   전부 같은 조회프로시저(한 번의 QueryMultiAsync)의 서로 다른 레코드셋이어야 한다 - grd1
///   선택에 따라 별도 프로시저를 다시 부르는 중간 단계 자체가 없기 때문.
/// - TreeMasterSubGrid: MasterFormSubGrid와 완전히 같은 구조인데 grd1(평범한 목록 그리드) 대신
///   tree1(TreeListWyn, 자기참조 계층 데이터)이 마스터다(2026-09-08 추가) - 부서/메뉴처럼 상위-
///   하위 구조를 가진 목록이 마스터인 화면용. MasterParentColumn(아래)이 이 템플릿에서만 쓰인다.</summary>
public enum TemplateKind
{
    SingleGrid,
    MasterSubGrid,
    MasterFormSubGrid,
    MasterOneSheet,
    TreeMasterSubGrid
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

    /// <summary>panHeader(검색조건)에 노출할 컨트롤 목록 - Name/ControlKind/LookupKey는 그리드/
    /// panData 컬럼과 같은 어휘(ColumnSpec)를 그대로 쓴다. 기본은 조회프로시저 자체 파라미터가
    /// 1:1로 채워지지만(ParamName="p_"+파라미터명), 그 프로시저의 실제 파라미터가 아닌 화면표시
    /// 전용 컨트롤도 자유롭게 추가할 수 있다 - 그런 항목은 ParamName을 비워둔다(예: 검색조건에
    /// dept_cd를 쓰면서 화면엔 dept_nm도 같이 보여주고 싶을 때, dept_cd는 ParamName="p_dept_cd",
    /// dept_nm은 ParamName=""로 추가한다. 2026-09-08).</summary>
    public List<ColumnSpec> SearchFields { get; set; } = new();

    /// <summary>마스터 그리드에서 선택된 행의 이 컬럼 값을 서브 조회의 유일한 필터 파라미터로
    /// 넘긴다(frmDept.cs 패턴) - grd1 컬럼 중 하나여야 한다.</summary>
    public string? MasterKeyColumn { get; set; }

    /// <summary>Kind가 TreeMasterSubGrid일 때만 쓰인다 - 마스터 트리(tree1)의 자기참조 상위키
    /// 컬럼명(예: 부서 테이블의 UPPER_DEPT_ID). MasterKeyColumn과 짝을 이뤄 tree1.KeyFieldName/
    /// ParentFieldName에 그대로 들어간다 - 이 컬럼도 grd1(마스터) 컬럼 목록에 실제로 있어야 한다.</summary>
    public string? MasterParentColumn { get; set; }

    /// <summary>MasterKeyColumn 값을 서브 조회 시 어느 파라미터(p_ 뗀 이름)로 보낼지 - 같은
    /// 프로시저를 Q/Q1으로 나눠 쓰므로 파라미터 목록 자체는 Q/Q1 공통이다.</summary>
    public string? DetailKeyParam { get; set; }

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

    /// <summary>그리드(grd1~5)/panData/검색조건 컨트롤에 공통으로 적용된다(2026-09-09 요청).
    /// true면 생성된 컨트롤에 BaseEdit 계열은 RequiredFieldExtensions.MarkRequired()(배경색 강조),
    /// 그리드 컬럼은 GridColumn.MarkRequired()(빈 셀만 강조)가 적용된다 - 두 확장메서드 모두 기존
    /// hand-written 화면들이 이미 쓰던 것을 그대로 재사용한다. 예외 두 가지: (1) TreeListColumn
    /// (tree1)에는 대응하는 확장메서드가 없어 트리 컬럼은 적용하지 않는다(의도적 범위 제한).
    /// (2) 그리드 컬럼은 그 그리드의 Role이 Query(조회전용, 편집 불가)면 이 플래그를 무시한다
    /// (2026-09-09, 사장님 지적 - "roll이 query인 컨트롤에는 필수입력 여부를 적용할 필요가 없어":
    /// 편집이 안 되는 그리드에 강조를 표시해봐야 사용자가 채울 방법이 없다) - panData/검색조건은
    /// 항상 편집 가능하므로 이 예외가 없다.</summary>
    public bool Required { get; set; }

    /// <summary>TEXT/CHECK/NUMBER/COMBO 중 하나 - COMBO면 LookupKey로 지정된 LookUp에 연결된다.</summary>
    public string ControlKind { get; set; } = "TEXT";

    /// <summary>ControlKind가 COMBO일 때만 쓰인다 - sysLookupM.lookup_key(frmSysLookup에서 등록한
    /// LookUp 키). 그리드 컬럼은 LookUpColumnEdit, panData 컨트롤은 LookUpEditWyn으로 이 값에
    /// 연결된다.</summary>
    public string? LookupKey { get; set; }

    /// <summary>ScreenGenSpec.SearchFields에서만 쓰인다(그리드/panData 컬럼은 안 씀) - 이 검색조건
    /// 컨트롤이 조회프로시저의 실제 파라미터일 때 그 파라미터 키를 "p_" 접두어까지 포함해서 그대로
    /// 담는다(예: "p_dept_cd"). 비어있으면 조회 시 이 컨트롤의 값을 안 보낸다 - 화면표시 전용
    /// 컨트롤(예: dept_cd 옆의 dept_nm)이라는 뜻이다.</summary>
    public string? ParamName { get; set; }

    /// <summary>ScreenGenSpec.SearchFields에서만 쓰인다(그리드/panData 컬럼은 안 씀, 2026-09-09) -
    /// false면 panHeader에 라벨+컨트롤이 그대로 생성되지만 Visible=false로 만들어진다(코드에서 값을
    /// 계속 읽고 쓸 수는 있어야 하는 화면 로직상 필요조건이라, ParamName처럼 아예 안 만들지는 않고
    /// 숨기기만 한다). 조회 시 QUERY_PARAMS에 값을 보내는지 여부(ParamName)와는 완전히 별개다.</summary>
    public bool Visible { get; set; } = true;
}
