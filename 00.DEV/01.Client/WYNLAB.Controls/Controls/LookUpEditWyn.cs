using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace WYNLAB.Base.Controls;

/// <summary>"코드/명" 한 쌍 - CodeLookupProvider가 돌려주는 항목 하나. 클래스 프로퍼티라
/// 실제 리플렉션 바인딩(LookUpEditWyn.Properties.ValueMember 등)이 가능하다(익명 튜플은
/// 이름이 컴파일러 메타데이터일 뿐이라 바인딩에 안 먹힘).</summary>
public class CodeLookupItem
{
    public string Value { get; set; } = string.Empty;
    public string Display { get; set; } = string.Empty;
}

/// <summary>
/// LookUpEditWyn.ProcName/Where 조합이 값을 가져올 때 실제로 호출하는 함수들의 등록소.
/// WYNLAB.Controls는 ApiClient/AppConfig를 모르므로(반대 방향 참조 금지 컨벤션), 앱 시작 시
/// WYNLAB.BaseForm 쪽에서 딱 한 번 필요한 프로시져들을 등록해준다(WYNLAB.Shell.Program.cs 참고).
///
/// 키는 실제 저장프로시져 이름(SSP_CBO_* 콤보/LookUp용, SSP_POP_*는 팝업용 - USP_{모듈명}인
/// 업무 프로세스 프로시져와 구분하는 명명규칙)과 1:1로 맞춘다. Where 값은 그 프로시져의
/// 파라미터 하나에 그대로 바인딩되는 "값"일 뿐이라, 클라이언트가 어떤 문자열을 넣어도 서버에서
/// 실행되는 SQL 구조 자체는 못 바꾼다(각 프로시져의 WHERE 절은 서버 코드에 고정되어 있음) -
/// 그래서 등록되지 않은 프로시져 이름은 조용히 무시된다(화이트리스트 방식, 임의 실행 불가).
/// </summary>
public static class CodeLookupProvider
{
    public static Dictionary<string, Func<string, Task<IEnumerable<CodeLookupItem>>>> Providers { get; } = new();
}

/// <summary>
/// LookUpEditWyn.LookupKey가 값을 가져올 때 실제로 호출하는 단일 함수의 등록소 - CodeLookupProvider와
/// 달리 프로시져별로 한 줄씩 등록하지 않는다. LookUp 이름(sysLookupM.lookup_key) + 파라미터
/// 딕셔너리를 받아 서버(api/combo-lookups/{key}/items)에 그대로 넘기면, 어떤 프로시져를 실행할지
/// /파라미터가 몇 개인지는 서버가 sysLookupM/P를 보고 알아서 처리한다 - 그래서 새 LookUp을
/// 추가해도(frmSysLookup에서 등록) 이 프로바이더 자체는 코드 변경이 필요 없다.
/// WYNLAB.Controls는 ApiClient를 모르므로(반대 방향 참조 금지 컨벤션), 앱 시작 시 WYNLAB.BaseForm
/// 쪽에서 딱 한 번 등록해준다(ControlDataSources 참고).
/// </summary>
public static class ComboLookupProvider
{
    public static Func<string, Dictionary<string, string?>, Task<IEnumerable<CodeLookupItem>>>? Fetch { get; set; }
}

/// <summary>
/// DevExpress LookUpEdit 기반 - WYNLAB 화면 전반에서 반복되는 "코드+명 2열 팝업" 패턴
/// (대분류/부서/담당자/상태코드 등)을 BindCodeList() 한 줄로 구성할 수 있게 한다. 지금까지는
/// 화면마다 Properties.Columns를 직접 채웠는데, 컬럼 순서/폭/검색컬럼 지정을 깜빡하기 쉬워서
/// 화면마다 팝업 모양이 조금씩 달랐다 - 여기서 한 번만 정의해서 통일한다.
///
/// ProcName + Where를 지정하면(디자이너 속성창에서든 코드에서든) 그 프로시져(SSP_CBO_*)를 자동
/// 호출해서 BindCodeList까지 알아서 해준다 - 둘 다 지정되어야 조회가 실행된다. 등록 안 된
/// ProcName이면 아무 일도 안 일어난다(CodeLookupProvider.Providers 참고). 다른 조회가
/// 필요하면 프로시져를 새로 만들고 Providers에 한 줄 등록하면 이 속성으로 바로 쓸 수 있다.
/// </summary>
[ToolboxItem(true)]
public class LookUpEditWyn : LookUpEdit
{
    private bool _required;
    private string? _procName;
    private string? _where;
    private string? _lookupKey;
    private readonly Dictionary<string, string?> _lookupParams = new();

    public LookUpEditWyn()
    {
        Properties.NullText = string.Empty;
    }

    /// <summary>ProcName/Where(프로시져 이름을 직접 지정하는 옛 방식)와 별개인, sysLookupM에
    /// 등록해둔 LookUp 이름만으로 쓰는 새 방식 - 실제 프로시져/파라미터/값필드/표시필드는 서버가
    /// sysLookupM/P를 보고 알아서 처리한다. 이 값만 있으면 조회가 실행된다(파라미터가 필요 없는
    /// LookUp도 있을 수 있어서 - SetParam 호출은 선택). 파라미터가 필요하면 SetParam으로 채운다.</summary>
    [Category("WYNLAB")]
    [Description("sysLookupM에 등록해둔 LookUp 이름. ProcName/Where 대신 이것만 지정하면 됩니다.")]
    [DefaultValue(null)]
    public string? LookupKey
    {
        get => _lookupKey;
        set { _lookupKey = value; _ = LoadFromLookupKeyAsync(); }
    }

    /// <summary>LookupKey가 가리키는 LookUp이 받는 파라미터 값을 채운다(파라미터가 없는 LookUp이면
    /// 호출할 필요 없음). 예: cboMinorCd.SetParam("p_major_code", "CM0001").</summary>
    public void SetParam(string paramNm, string? value)
    {
        _lookupParams[paramNm] = value;
        _ = LoadFromLookupKeyAsync();
    }

    private async Task LoadFromLookupKeyAsync()
    {
        var lookupKey = _lookupKey;
        if (string.IsNullOrEmpty(lookupKey) || ComboLookupProvider.Fetch == null) return;

        try
        {
            var paramsSnapshot = new Dictionary<string, string?>(_lookupParams);
            var items = (await ComboLookupProvider.Fetch(lookupKey!, paramsSnapshot)).ToList();
            if (lookupKey != _lookupKey) return; // 응답 오는 사이 LookupKey가 또 바뀌었으면 버림

            items.Insert(0, new CodeLookupItem()); // ProcName/Where 경로와 같은 이유 - 빈 값으로 되돌릴 수 있게
            BindCodeList(items, nameof(CodeLookupItem.Value), nameof(CodeLookupItem.Display));
        }
        catch
        {
            // 목록 하나 못 불러온다고 화면 전체가 죽으면 안 됨 - ProcName/Where 경로와 같은 이유.
        }
    }

    /// <summary>호출할 콤보/LookUp 전용 프로시져 이름(예: "SSP_CBO_CODE_Q"). Where와 함께
    /// 지정해야 조회가 실행된다.</summary>
    [Category("WYNLAB")]
    [Description("호출할 콤보/LookUp 전용 프로시져 이름(예: SSP_CBO_CODE_Q). Where와 함께 지정해야 조회됩니다.")]
    [DefaultValue(null)]
    public string? ProcName
    {
        get => _procName;
        set { _procName = value; _ = LoadAsync(); }
    }

    /// <summary>ProcName이 가리키는 프로시져의 파라미터로 그대로 바인딩되는 값(예: "CD0001").
    /// SQL 텍스트가 아니라 값 하나다.</summary>
    [Category("WYNLAB")]
    [Description("ProcName 프로시져에 파라미터 값으로 전달됩니다(SQL 텍스트 아님).")]
    [DefaultValue(null)]
    public string? Where
    {
        get => _where;
        set { _where = value; _ = LoadAsync(); }
    }

    private async Task LoadAsync()
    {
        var (procName, where) = (_procName, _where);
        if (string.IsNullOrEmpty(procName) || string.IsNullOrEmpty(where)) return;
        if (!CodeLookupProvider.Providers.TryGetValue(procName!, out var fetch)) return;

        try
        {
            var items = (await fetch(where!)).ToList();
            if (procName != _procName || where != _where) return; // 응답 오는 사이 값이 또 바뀌었으면 버림

            // 지우기(X) 버튼 대신 빈 값 자체를 목록의 선택 가능한 항목 하나로 넣는다 - 이게
            // 없으면 한 번 값을 고른 뒤에는 빈 값으로 되돌릴 방법이 없다(실제로 겪음 -
            // frmMinorCode 관리항목 구분 콤보를 잘못 선택했는데 지울 수 없었음. Delete 버튼을
            // 붙여봤지만 화면에 안 보여서 이 방식으로 바꿈). ProcName/Where를 쓰는 모든
            // 콤보에 공통 적용된다.
            items.Insert(0, new CodeLookupItem());
            BindCodeList(items, nameof(CodeLookupItem.Value), nameof(CodeLookupItem.Display));
        }
        catch
        {
            // 목록 하나 못 불러온다고 화면 전체가 죽으면 안 됨 - 조용히 무시(빈 팝업으로 남음).
        }
    }

    [Category("WYNLAB")]
    [Description("필수입력 스타일(연노랑 배경)을 적용합니다.")]
    [DefaultValue(false)]
    public bool Required
    {
        get => _required;
        set
        {
            _required = value;
            if (value)
            {
                this.MarkRequired();
            }
            else
            {
                Properties.Appearance.Options.UseBackColor = false;
                Properties.Appearance.Options.UseForeColor = false;
            }
        }
    }

    /// <summary>
    /// "코드+명" 2열 팝업으로 데이터소스를 구성한다. 검색은 명칭(표시값) 컬럼 기준으로 자동 적용된다.
    /// </summary>
    /// <param name="dataSource">바인딩할 목록(List/DataTable 등)</param>
    /// <param name="valueMember">실제 값으로 쓸 필드명(코드)</param>
    /// <param name="displayMember">화면에 표시할 필드명(명칭)</param>
    /// <param name="valueCaption">코드 컬럼 헤더 캡션</param>
    /// <param name="displayCaption">명칭 컬럼 헤더 캡션</param>
    /// <param name="popupWidth">팝업 폭(px)</param>
    public void BindCodeList(object dataSource, string valueMember, string displayMember,
        string valueCaption = "코드", string displayCaption = "명칭", int popupWidth = 260)
    {
        Properties.Columns.Clear();
        Properties.Columns.Add(new LookUpColumnInfo(valueMember, valueCaption, 80));
        Properties.Columns.Add(new LookUpColumnInfo(displayMember, displayCaption));

        Properties.ValueMember = valueMember;
        Properties.DisplayMember = displayMember;
        Properties.DataSource = dataSource;

        Properties.PopupWidth = popupWidth;
        Properties.AutoSearchColumnIndex = 1; // 명칭(표시값) 기준으로 타이핑 검색
        Properties.ShowFooter = false;
    }
}
