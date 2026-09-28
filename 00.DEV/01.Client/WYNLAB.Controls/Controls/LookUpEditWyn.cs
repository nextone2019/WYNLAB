using System.ComponentModel;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace WYNLAB.Base.Controls;

/// <summary>"코드/명" 한 쌍 - CodeLookupProvider가 돌려주는 항목 하나. 클래스 프로퍼티라
/// 실제 리플렉션 바인딩(LookUpEditWyn.Properties.ValueMember 등)이 가능하다(익명 튜플은
/// 이름이 컴파일러 메타데이터일 뿐이라 바인딩에 안 먹힘).
///
/// Row는 결과셋의 전체 컬럼 원본값(값필드/표시필드 외 나머지) - sysLookupC에 컬럼을 설정한
/// LookUp만 채워진다(2026-09-03). DevExpress LookUpColumnInfo는 바인딩된 객체의 "진짜
/// 프로퍼티 이름"을 리플렉션으로 찾기 때문에, LookUp마다 다른 임의 컬럼명(remark 등)은 이
/// 고정 클래스에 프로퍼티로 못 만든다 - 그래서 컬럼 구성이 있는 LookUp은 List&lt;CodeLookupItem&gt;
/// 대신 DataTable로 바꿔서 바인딩한다(BuildColumnDataSource 참고), Row는 그 변환에만 쓰인다.</summary>
public class CodeLookupItem
{
    public string Value { get; set; } = string.Empty;
    public string Display { get; set; } = string.Empty;
    public Dictionary<string, string?>? Row { get; set; }
}

/// <summary>sysLookupC 한 행(컬럼 구성) - ComboLookupResult.Columns가 비어있으면 예전 그대로
/// 값필드/표시필드 2컬럼 고정으로 그린다(하위호환).</summary>
public class ComboColumnDef
{
    public string ColumnNm { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int Width { get; set; } = 100;
}

/// <summary>ComboLookupProvider.Fetch의 반환값 - 항목 목록 + 컬럼 구성(설정 없으면 빈 리스트).</summary>
public class ComboLookupResult
{
    public List<CodeLookupItem> Items { get; set; } = new();
    public List<ComboColumnDef> Columns { get; set; } = new();
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
    public static Func<string, Dictionary<string, string?>, Task<ComboLookupResult>>? Fetch { get; set; }
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
    private List<CodeLookupItem> _loadedItems = new();

    /// <summary>지금까지 LookupKey로 불러온 목록의 첫 실제 항목(0번은 항상 빈 값이라 제외) 값 -
    /// 아직 로드 전이면 null. "신규입력 시 이 콤보를 목록 1번째로 기본값 세팅" 같은 요구에 쓴다
    /// (예: frmPo 발주구분). 비동기 재조회 없이 이미 불러와 있는 목록을 그대로 읽는다.</summary>
    public string? FirstItemValue => _loadedItems.Count > 1 ? _loadedItems[1].Value : null;

    public LookUpEditWyn()
    {
        Properties.NullText = string.Empty;
        // 타이핑한 글자가 포함된 항목만 팝업 목록에 남긴다(2026-09-09 요청) - 기존엔 자동완성
        // (일치하는 첫 항목으로 이동만 함)만 되고 나머지 항목은 그대로 다 보여서, 목록이 길면
        // 원하는 값을 찾기 어려웠다.
        Properties.PopupFilterMode = PopupFilterMode.Contains;
    }

    /// <summary>base.EditValue는 "선택 안 함" 상태에서 null을 돌려준다 - 저장 코드마다 매번
    /// `EditValue?.ToString() ?? string.Empty`처럼 `?.`를 챙겨야 하는 게 실수하기 쉽다는
    /// 이유로(2026-09-02, 자산구분 콤보에서 `.ToString()`만 쓰다가 NullReferenceException 날
    /// 뻔한 사례) 항상 빈 문자열을 대신 돌려주도록 오버라이드한다. EditValue = null로 값을
    /// 지우는 건 그대로 되고(내부 저장값 자체는 그대로 null), 읽을 때만 문자열로 보정된다 -
    /// 그래서 호출부는 이제 `EditValue.ToString()`만 써도 안전하다.
    ///
    /// [AllowNull]로 "set엔 null 허용, get은 항상 non-null"이라는 이 비대칭을 컴파일러에
    /// 알리고 싶었지만, net48엔 System.Diagnostics.CodeAnalysis.AllowNullAttribute가 없다
    /// (.NET Core 3.0+/.NET Standard 2.1부터 추가됨 - 폴리필 없이는 못 씀). 그래서 값을
    /// 지우려고 `EditValue = null`을 쓰는 호출부는 `null!`로 명시적으로 경고를 끈다
    /// (frmMinorCode.cs 등 - "이 자리는 원래 null 허용" 표시).</summary>
    public override object EditValue
    {
        get => base.EditValue ?? string.Empty;
        set => base.EditValue = value;
    }

    /// <summary>LookUpEdit도 ButtonEdit과 같은 RepositoryItemButtonEdit 계열이라, PopupLookupEditWyn.
    /// EnsureButton과 똑같은 DevExpress 버그를 겪는다 - 생성자에서는 기본 콤보 버튼(역삼각형)이
    /// Properties.Buttons에 있지만, InitializeComponent의 BeginInit/EndInit 구간을 지나면서
    /// (디자이너 코드로 직접 등록하지 않은 버튼이라) 비워져서 실제로 그려질 때는 버튼이 안 보인다
    /// (실제로 겪음 - 품목등록 기본단위/구매단위, 2026-09-02). 원인 자체를 막는 대신, 실제로
    /// 그려지기 직전(OnHandleCreated)에 없으면 다시 채워 넣는다.</summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (Properties.Buttons.Count == 0)
        {
            Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        }
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

    /// <summary>SetParam과 같지만 목록이 실제로 다시 채워질 때까지 기다린다 - 연쇄(부모-자식)
    /// LookUp에서 기존 레코드를 불러와 자식의 EditValue를 이어서 설정해야 할 때 쓴다. SetParam은
    /// fire-and-forget이라, 부모 값에 맞는 목록이 아직 안 채워진 상태에서 자식 EditValue를
    /// 설정하면 표시텍스트가 안 붙을 수 있다(품목그룹 1~4단 연쇄, 2026-09-15).</summary>
    public async Task SetParamAsync(string paramNm, string? value)
    {
        _lookupParams[paramNm] = value;
        await LoadFromLookupKeyAsync();
    }

    private async Task LoadFromLookupKeyAsync()
    {
        var lookupKey = _lookupKey;
        if (string.IsNullOrEmpty(lookupKey) || ComboLookupProvider.Fetch == null) return;

        try
        {
            var paramsSnapshot = new Dictionary<string, string?>(_lookupParams);
            var result = await ComboLookupProvider.Fetch(lookupKey!, paramsSnapshot);
            if (lookupKey != _lookupKey) return; // 응답 오는 사이 LookupKey가 또 바뀌었으면 버림

            var items = result.Items;
            items.Insert(0, new CodeLookupItem()); // ProcName/Where 경로와 같은 이유 - 빈 값으로 되돌릴 수 있게
            _loadedItems = items;

            var multiColumn = ComboLookupColumnBuilder.Build(items, result.Columns);
            if (multiColumn != null)
            {
                Properties.Columns.Clear();
                foreach (var col in multiColumn.Value.Columns) Properties.Columns.Add(col);
                Properties.ValueMember = nameof(CodeLookupItem.Value);
                Properties.DisplayMember = nameof(CodeLookupItem.Display);
                Properties.DataSource = multiColumn.Value.Table;
                Properties.PopupWidth = 260;
                Properties.AutoSearchColumnIndex = 1;
                Properties.ShowFooter = false;
            }
            else
            {
                BindCodeList(items, nameof(CodeLookupItem.Value), nameof(CodeLookupItem.Display));
            }
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
                this.ClearRequired();
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

    /// <summary>
    /// 그리드 컬럼(GridColumn.ColumnEdit)에 LookupKey 기반 LookUp을 붙일 때 쓴다. LookUpEditWyn
    /// 자신은 Control(패널의 독립 입력창)이라 그리드 컬럼 자리엔 못 들어간다 - DevExpress
    /// 그리드는 컬럼 편집기로 RepositoryItem(찍어내기용 "틀")을 받으므로, 이 메서드가 그 틀
    /// (RepositoryItemLookUpEdit)을 LookupKey로 채워서 돌려준다. 반환값은 호출부가
    /// grid.RepositoryItems.Add(item) 한 뒤 column.ColumnEdit = item으로 붙여야 한다(둘 다
    /// 빠뜨리면 안 됨 - RepositoryItems에 안 넣으면 그리드가 소유권을 안 가져가서 Dispose 시점이
    /// 꼬인다). 목록을 한 번만 가져오는 스냅샷이다 - LookUpEditWyn.LookupKey처럼 값이 바뀔 때마다
    /// 다시 불러오는 살아있는 바인딩이 아니라서, LookUp 정의 자체가 자주 안 바뀌는(=관리자가
    /// LookUp관리에서 가끔만 고치는) 코드성 목록에 적합하다.</summary>
    public static async Task<RepositoryItemLookUpEdit> CreateGridRepositoryItemAsync(string lookupKey)
    {
        var item = new RepositoryItemLookUpEdit { NullText = string.Empty };

        if (ComboLookupProvider.Fetch == null) return item;

        try
        {
            var result = await ComboLookupProvider.Fetch(lookupKey, new Dictionary<string, string?>());
            var items = result.Items;
            items.Insert(0, new CodeLookupItem()); // 값 지우기(빈 값으로 되돌리기) 가능하게 - BindCodeList와 같은 이유

            var multiColumn = ComboLookupColumnBuilder.Build(items, result.Columns);
            if (multiColumn != null)
            {
                foreach (var col in multiColumn.Value.Columns) item.Columns.Add(col);
                item.ValueMember = nameof(CodeLookupItem.Value);
                item.DisplayMember = nameof(CodeLookupItem.Display);
                item.DataSource = multiColumn.Value.Table;
            }
            else
            {
                item.Columns.Add(new LookUpColumnInfo(nameof(CodeLookupItem.Value), "코드", 80));
                item.Columns.Add(new LookUpColumnInfo(nameof(CodeLookupItem.Display), "명칭"));
                item.ValueMember = nameof(CodeLookupItem.Value);
                item.DisplayMember = nameof(CodeLookupItem.Display);
                item.DataSource = items;
            }
            item.PopupWidth = 260;
            item.AutoSearchColumnIndex = 1;
            item.ShowFooter = false;
        }
        catch
        {
            // 목록 하나 못 불러온다고 화면 전체가 죽으면 안 됨 - LoadFromLookupKeyAsync와 같은 이유.
        }

        return item;
    }
}

/// <summary>sysLookupC 컬럼 구성이 있는 LookUp을 실제로 팝업에 그리는 공용 변환 로직 -
/// LookUpEditWyn(패널 컨트롤)과 LookUpColumnEdit(그리드 컬럼 편집기) 둘 다 여기를 쓴다.
/// List&lt;CodeLookupItem&gt;은 고정 프로퍼티(Value/Display)만 가진 클래스라 LookUp마다 다른
/// 임의 컬럼명(remark 등)을 리플렉션으로 못 찾는다 - 그래서 컬럼 구성이 있으면 그 실제 컬럼명을
/// 그대로 가진 DataTable로 바꿔서 LookUpColumnInfo가 직접 참조할 수 있게 한다.</summary>
internal static class ComboLookupColumnBuilder
{
    /// <summary>columnDefs가 비어있으면(sysLookupC 설정 안 한 LookUp, 기존 전부 해당) null을
    /// 돌려줘서 호출부가 예전 그대로(BindCodeList, List&lt;CodeLookupItem&gt; 그대로)를 쓰게 한다.</summary>
    public static (DataTable Table, List<LookUpColumnInfo> Columns)? Build(List<CodeLookupItem> items, List<ComboColumnDef> columnDefs)
    {
        if (columnDefs.Count == 0) return null;

        var table = new DataTable();
        table.Columns.Add(nameof(CodeLookupItem.Value), typeof(string));
        table.Columns.Add(nameof(CodeLookupItem.Display), typeof(string));
        foreach (var def in columnDefs)
        {
            if (!table.Columns.Contains(def.ColumnNm))
                table.Columns.Add(def.ColumnNm, typeof(string));
        }

        foreach (var item in items)
        {
            var row = table.NewRow();
            row[nameof(CodeLookupItem.Value)] = item.Value;
            row[nameof(CodeLookupItem.Display)] = item.Display;
            if (item.Row != null)
            {
                foreach (var def in columnDefs)
                {
                    if (item.Row.TryGetValue(def.ColumnNm, out var v))
                        row[def.ColumnNm] = (object?)v ?? DBNull.Value;
                }
            }
            table.Rows.Add(row);
        }

        // Width=0으로 "숨김" 의도를 표현해도 DevExpress는 컬럼 자체는 Visible인 채로 폭만 0에
        // 가깝게 그려서 얇은 선(스크롤바/그리드 경계선)이 남아 보인다(실제로 겪음 - Module
        // LookUp 팝업 왼쪽 여백). Width<=0이면 Visible=false로 컬럼 자체를 빼서 완전히 숨긴다.
        var columns = columnDefs
            .Select(d =>
            {
                var col = new LookUpColumnInfo(d.ColumnNm, d.Caption ?? d.ColumnNm, d.Width);
                if (d.Width <= 0) col.Visible = false;
                return col;
            })
            .ToList();
        return (table, columns);
    }
}
