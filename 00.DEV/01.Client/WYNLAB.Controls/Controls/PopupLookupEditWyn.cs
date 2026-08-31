using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace WYNLAB.Base.Controls;

/// <summary>팝업에서 사용자가 고른 한 행 - Code/Display는 예전 코드+명 페어 방식(NameControl)을
/// 위한 하위호환용이고, Row는 그 행의 전체 컬럼값(멀티필드 모드/MapField가 쓴다).</summary>
public class PopupLookupResult
{
    public string Code { get; set; } = string.Empty;
    public string Display { get; set; } = string.Empty;
    public Dictionary<string, string?> Row { get; set; } = new();
}

/// <summary>
/// PopupLookupEditWyn이 실제 팝업창을 여는 방법을 등록해두는 곳. WYNLAB.Controls는 ApiClient를
/// 모르므로(반대 방향 참조 금지 컨벤션 - LookUpEditWyn.CodeLookupProvider와 같은 이유) 실제 구현은
/// WYNLAB.BaseForm이 앱 시작 시 한 번 채워준다(ControlDataSources.Initialize 참고).
/// </summary>
public static class PopupLookupProvider
{
    /// <summary>버튼 클릭(또는 SearchExact로 못 좁혀졌을 때)으로 팝업 UI를 띄운다. initialKeyword를
    /// 주면 팝업의 조회조건 입력창들을 그 값으로 미리 채우고 뜨자마자 그 값으로 자동 조회한다.</summary>
    public static Func<string, Control, string?, Task<PopupLookupResult?>>? OpenPopup { get; set; }

    /// <summary>MatchField 모드에서 포커스가 빠져나갈 때 부르는 "조용한" 검색 - 팝업 UI 없이
    /// 결과 행들만 돌려준다. 그 popup_key에 정의된 조회조건 전부에 keyword를 넣어 검색한다 -
    /// 조건이 몇 개인지/이름이 뭔지는 이 컨트롤이 몰라도 된다(WYNLAB.BaseForm이 정의를 읽어서 처리).</summary>
    public static Func<string, string, Task<List<PopupLookupResult>>>? SearchExact { get; set; }
}

/// <summary>
/// 코드값 텍스트박스 + "..." 버튼(DevExpress ButtonEdit 기반) - 클릭하면 LookupKey에 등록된
/// 팝업(sysPopUpM.popup_key)이 뜨고, 선택한 행의 값을 화면에 채워 넣는다. 두 가지 모드가 있다:
///
/// ① 코드+명 페어 모드(기존) - NameControl만 지정. 이 컨트롤 자신 = 코드값, NameControl = 명칭.
///    버튼을 눌러야만 팝업이 뜬다(frmDept의 상위부서코드가 이 모드).
/// ② 멀티필드 모드(신규) - MatchField + MapField를 같이 씀. 팝업 결과 행이 코드/명 2개가 아니라
///    임의 개수의 컬럼을 가질 수 있고(예: dept_cd/dept_nm/par_dept_cd/par_dept_nm), 그걸 각각
///    다른 컨트롤에 매핑해서 채워 넣는다. 그리고 이 컨트롤(=MatchField가 가리키는 컬럼, 보통
///    "명칭") 자체에 사용자가 직접 타이핑하고 포커스를 벗어나면(Leave) 그 값으로 자동 조회해서
///    ① 정확히 일치하는 행이 하나면 조용히 채우고 ② 아니면 그 값으로 미리 검색된 팝업을 띄워
///    사용자가 고르게 한다. 이 컨트롤을 지우면(빈 값) 매핑된 필드가 전부 같이 지워진다.
///
/// 사용법(모드①): 기존 코드 텍스트박스(TextEditWyn) 자리에 이 컨트롤을 놓고 LookupKey와
/// NameControl만 지정. 사용법(모드②): LookupKey + MatchField(예: "dept_nm") 지정하고
/// MapField(컬럼명, 대상컨트롤)를 필요한 만큼 호출(자기 자신 포함 안 해도 됨 - MatchField로 이미
/// 자기 자신은 채워짐).
/// </summary>
[ToolboxItem(true)]
public class PopupLookupEditWyn : ButtonEdit
{
    private Control? _nameControl;
    private bool _syncingPair;
    private readonly Dictionary<string, Control> _fieldMap = new();

    public PopupLookupEditWyn()
    {
        EnsureButton();
        Properties.NullText = string.Empty;
        ButtonClick += (s, e) => _ = OpenPopupAsync(null);
        Leave += async (s, e) => await OnLeaveAsync();
        EditValueChanged += (s, e) => { if (Text.Length == 0) ClearMappedFields(); };
    }

    private string? _toolTipText;

    /// <summary>생성자에서 한 번 넣어도 InitializeComponent의 BeginInit/EndInit 구간을 지나면서
    /// Properties.Buttons가 다시 비워지는 걸 실제로 확인했다(진단 로그로 검증: 생성자 직후엔
    /// Count=1, HandleCreated 시점엔 Count=0 - DevExpress RepositoryItemButtonEdit.EndInit이
    /// 디자이너 코드로 등록되지 않은 버튼을 기본 상태로 되돌리는 것으로 보임). 원인 자체를 막는
    /// 대신, 실제로 그려지기 직전(OnHandleCreated)에 없으면 다시 채워 넣어 항상 버튼이 보이게
    /// 한다. 버튼이 다시 만들어질 때마다 ToolTip도 같이 다시 입혀야 한다(아래 ToolTip 프로퍼티
    /// 참고) - 버튼이 새로 생기면 그 버튼엔 우리가 마지막으로 지정한 툴팁이 없는 빈 상태다.</summary>
    private void EnsureButton()
    {
        if (Properties.Buttons.Count == 0)
        {
            // Caption을 명시적으로 줘서, 스킨에 따라 생략표(...) 아이콘 자체가 흐릿하거나 안 보이는
            // 경우에도 텍스트로는 항상 버튼이 있다는 게 드러나게 한다.
            Properties.Buttons.Add(new EditorButton(ButtonPredefines.Ellipsis, "...")
            {
                Width = 24
            });
        }

        ApplyButtonToolTip();
    }

    private void ApplyButtonToolTip()
    {
        if (Properties.Buttons.Count > 0) Properties.Buttons[0].ToolTip = _toolTipText ?? string.Empty;
    }

    /// <summary>base(BaseControl.ToolTip)는 텍스트 입력 영역에만 적용된다 - "..." 버튼은
    /// 별도의 EditorButton.ToolTip을 따로 보기 때문에, 버튼 위에 마우스를 올리면 DevExpress가
    /// 이 프로퍼티를 무시하고 버튼 캡션("...")을 대신 툴팁으로 보여준다(실제로 겪음 - 텍스트
    /// 영역엔 BindingField 툴팁이 뜨는데 버튼 위에서는 "..."만 떴음). 그래서 이 프로퍼티를
    /// 가려서(new) 버튼에도 같은 텍스트를 같이 넣어준다 - 컨트롤 어디에 마우스를 올려도 같은
    /// 툴팁이 뜨게.</summary>
    public new string? ToolTip
    {
        get => _toolTipText;
        set
        {
            _toolTipText = value;
            base.ToolTip = value ?? string.Empty;
            ApplyButtonToolTip();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        EnsureButton();
    }

    /// <summary>어느 팝업을 열지(sysPopUpM.popup_key), 예: "DEPT".</summary>
    [Category("WYNLAB")]
    [Description("어느 팝업을 열지 지정합니다(sysPopUpM.popup_key). 예: DEPT")]
    [DefaultValue(null)]
    public string? LookupKey { get; set; }

    /// <summary>같은 화면의 "명칭" 컨트롤 - 팝업에서 선택하면 여기에도 자동으로 채워진다.
    /// 이 컨트롤의 값을 사용자가 지우면(선택 해제 의도) 코드값(이 컨트롤 자신)도 같이 비운다.
    /// 모드①(코드+명 페어) 전용 - 모드②(MapField)를 쓰면 이건 지정 안 해도 된다.</summary>
    [Category("WYNLAB")]
    [Description("팝업 선택 시 명칭을 채워 넣을 컨트롤(코드+명 페어 모드 전용). 이 컨트롤을 비우면 코드값도 같이 비워집니다.")]
    [DefaultValue(null)]
    public Control? NameControl
    {
        get => _nameControl;
        set
        {
            if (_nameControl != null) _nameControl.TextChanged -= NameControl_TextChanged;
            _nameControl = value;
            if (_nameControl != null) _nameControl.TextChanged += NameControl_TextChanged;
        }
    }

    /// <summary>멀티필드 모드 전용 - 이 컨트롤 자신에 타이핑된 값이 팝업 결과 행의 어느 컬럼과
    /// 같은지(예: "dept_nm"). 지정해야 Leave 시 자동조회/자동팝업이 동작한다.</summary>
    [Category("WYNLAB")]
    [Description("멀티필드 모드 전용 - 이 컨트롤이 대표하는 팝업 결과 컬럼명(예: dept_nm). MapField와 함께 사용.")]
    [DefaultValue(null)]
    public string? MatchField { get; set; }

    /// <summary>멀티필드 모드 전용 - 팝업 결과 행의 컬럼 하나를 지정한 컨트롤에 채워 넣도록
    /// 등록한다. 필요한 만큼 여러 번 호출. 자기 자신(MatchField가 가리키는 값)은 자동으로
    /// 채워지므로 따로 등록할 필요 없다.</summary>
    public void MapField(string resultColumn, Control target) => _fieldMap[resultColumn] = target;

    private void NameControl_TextChanged(object? sender, EventArgs e)
    {
        if (_syncingPair || _nameControl == null || Text.Length == 0) return;

        // .Text가 아니라 EditValue로 "진짜 비었는지" 확인한다 - NameControl에 NullText(플레이스홀더)가
        // 설정되어 있으면 .Text가 빈 문자열 대신 그 플레이스홀더 문구를 돌려줄 수 있다(같은 함정을
        // PopupLookupForm 검색창에서도 겪었다 - PopupLookupForm.SearchAsync 주석 참고).
        var nameIsEmpty = _nameControl is BaseEdit nameEdit
            ? string.IsNullOrEmpty(nameEdit.EditValue as string)
            : _nameControl.Text.Length == 0;

        if (nameIsEmpty)
        {
            _syncingPair = true;
            try { Text = string.Empty; }
            finally { _syncingPair = false; }
        }
    }

    /// <summary>멀티필드 모드에서 포커스가 벗어날 때: 지금 타이핑된 값으로 조용히 조회해서
    /// 정확히 하나만 일치하면 그대로 채우고, 아니면(0개거나 여러 개) 그 값을 미리 채운 상태의
    /// 팝업을 띄워 고르게 한다. 모드①(NameControl만 쓰는 화면)에서는 MatchField가 없으니
    /// 아무 일도 안 한다 - 기존 화면 동작에 영향 없음.</summary>
    private async Task OnLeaveAsync()
    {
        if (string.IsNullOrEmpty(MatchField) || _fieldMap.Count == 0) return;

        var typed = Text?.Trim();
        if (string.IsNullOrEmpty(typed) || string.IsNullOrEmpty(LookupKey) || PopupLookupProvider.SearchExact == null) return;

        List<PopupLookupResult> candidates;
        try
        {
            candidates = await PopupLookupProvider.SearchExact(LookupKey!, typed!);
        }
        catch
        {
            return; // 목록 하나 못 불러온다고 화면이 죽으면 안 됨 - 다른 팝업 경로와 같은 원칙.
        }

        var exact = candidates.Where(c =>
            c.Row.TryGetValue(MatchField!, out var v) && string.Equals(v, typed, StringComparison.OrdinalIgnoreCase)).ToList();

        if (exact.Count == 1)
        {
            ApplyResult(exact[0]);
            return;
        }

        await OpenPopupAsync(typed);
    }

    private async Task OpenPopupAsync(string? initialKeyword)
    {
        if (string.IsNullOrEmpty(LookupKey) || PopupLookupProvider.OpenPopup == null) return;

        var result = await PopupLookupProvider.OpenPopup(LookupKey!, this, initialKeyword);
        if (result == null) return; // 취소

        ApplyResult(result);
    }

    private void ApplyResult(PopupLookupResult result)
    {
        _syncingPair = true;
        try
        {
            if (_fieldMap.Count > 0 && !string.IsNullOrEmpty(MatchField))
            {
                Text = result.Row.TryGetValue(MatchField!, out var self) ? (self ?? string.Empty) : result.Display;
                foreach (var kv in _fieldMap)
                {
                    if (ReferenceEquals(kv.Value, this)) continue;
                    kv.Value.Text = result.Row.TryGetValue(kv.Key, out var v) ? (v ?? string.Empty) : string.Empty;
                }
            }
            else
            {
                // 모드①(코드+명 페어) - MatchField/MapField를 안 쓰는 화면(frmDept 상위부서 등)은 그대로 동작.
                Text = result.Code;
                if (_nameControl != null) _nameControl.Text = result.Display;
            }
        }
        finally
        {
            _syncingPair = false;
        }
    }

    private void ClearMappedFields()
    {
        if (_syncingPair || _fieldMap.Count == 0) return;

        _syncingPair = true;
        try
        {
            foreach (var control in _fieldMap.Values)
            {
                if (ReferenceEquals(control, this)) continue;
                control.Text = string.Empty;
            }
        }
        finally
        {
            _syncingPair = false;
        }
    }
}
