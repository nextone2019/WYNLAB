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

    /// <summary>지금 열리는 팝업의 프로시저에 "조회조건 화면 밖에서" 더 넘길 값(예: 구매요청등록이 팝업을 열 때 그 화면
    /// 헤더의 거래처/요청일자). 팝업을 여는 쪽(PopupLookupColumnEdit.ConditionProvider)이 열기 직전에 채우고 닫힌 뒤 비운다 -
    /// 팝업은 앱 전체에서 한 번에 하나만 뜨므로(popPopUp._isShowing) 전역 하나로 충분하다. 팝업 엔진은 조회조건에 같은 키가
    /// 비어 있을 때만 이 값을 넣고, 검색패널에 같은 이름의 컨트롤이 있으면 초기값으로도 채운다.</summary>
    public static Dictionary<string, string?>? ExtraConditions { get; set; }
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
    private string? _valueOnEnter;

    public PopupLookupEditWyn()
    {
        EnsureButton();
        Properties.NullText = string.Empty;
        // "..." 버튼 클릭은 async 람다를 그대로 discard(fire-and-forget)해서 호출한다 - 만약
        // OpenPopupAsync 내부(또는 그 안에서 부르는 popPopUp.ShowAsync)가 await 이후 구간에서
        // 예외를 던지면, 아무도 그 Task를 기다리지 않으므로 예외가 UI에 전혀 드러나지 않고
        // 조용히 사라진다 - 사용자에게는 "버튼을 눌러도 아무 반응이 없다"로만 보인다(2026-09-23
        // 실제 발견 - "그래도 팝업 안떠"). 그 자체를 고치는 대신, 최소한 그 실패가 눈에 보이게
        // try/catch로 감싼다 - 이러면 다음엔 진짜 원인이 메시지로 드러난다.
        ButtonClick += async (s, e) =>
        {
            try
            {
                await OpenPopupAsync(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"팝업을 여는 중 오류가 발생했습니다.\n{ex.Message}", "오류");
            }
        };
        // 포커스를 얻은 시점의 값을 기억해뒀다가 Leave에서 실제로 바뀌었는지 비교한다(아래
        // OnLeaveAsync 설명 참고) - 사용자가 아무것도 안 고쳤는데도(이미 이름이 겹치는 부서라
        // 원래부터 모호했던 값 등) 그냥 포커스만 지나가도 매번 팝업이 뜨고, 그걸 닫으면 값이
        // 지워지는 오작동을 막기 위함(2026-09-12 실제 발견).
        Enter += (s, e) => _valueOnEnter = Text;
        Leave += async (s, e) => await OnLeaveAsync();
        EditValueChanged += (s, e) => { if (Text.Length == 0) ClearMappedFields(); };
    }

    private bool _required;

    /// <summary>TextEditWyn/MemoEditWyn 등과 같은 목적 - "필수입력" 표시를 속성창 체크박스
    /// 하나로 켤 수 있게 한다. ButtonEdit도 DevExpress BaseEdit 계열이라
    /// RequiredFieldExtensions.MarkRequired&lt;T&gt;()를 그대로 재사용할 수 있다.</summary>
    [Category("WYNLAB")]
    [Description("필수 입력 항목이면 배경색으로 강조 표시합니다.")]
    [DefaultValue(false)]
    public bool Required
    {
        get => _required;
        set
        {
            _required = value;
            ApplyRequiredStyle();
        }
    }

    private void ApplyRequiredStyle()
    {
        if (_required)
        {
            this.MarkRequired();
        }
        else
        {
            this.ClearRequired();
        }
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

    private string? _lookupKey;

    /// <summary>어느 팝업을 열지(sysPopUpM.popup_key), 예: "DEPT". 그리드 컬럼 편집기로 쓰일 때는
    /// (PopupLookupColumnEdit.LookupKey, 그 클래스 설명 참고) 이 컨트롤 자신에 값을 직접 지정한
    /// 적이 없으면 Properties(그리드가 공유해주는 리포지토리 아이템)의 LookupKey로 폴백한다 -
    /// PopupLookupColumnEdit은 이 프로퍼티처럼 살아있는 컨트롤 상태가 아니라 RepositoryItem
    /// 하나에 값을 들고 있어서, 셀 편집이 시작될 때마다 새로 만들어지는 이 컨트롤 인스턴스가
    /// 자동으로 물려받지 못하기 때문(2026-09-07).</summary>
    [Category("WYNLAB")]
    [Description("어느 팝업을 열지 지정합니다(sysPopUpM.popup_key). 예: DEPT")]
    [DefaultValue(null)]
    public string? LookupKey
    {
        get => _lookupKey ?? (Properties as PopupLookupColumnEdit)?.LookupKey;
        set => _lookupKey = value;
    }

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

    /// <summary>이 컨트롤만 비우고, MapField로 연결된 필드는 건드리지 않는다. 일반 Text=""는
    /// EditValueChanged -> ClearMappedFields로 매핑필드도 같이 지운다(사용자가 직접 지운 경우엔
    /// 그게 맞는 동작) - 그런데 호출부가 EnterNewMode처럼 매핑필드에 이미 다른 기본값을 정해둔
    /// 경우엔 그 매핑필드 clear가 자기 자신을 다시 덮어써서 기본값이 사라지는 문제가 있었다
    /// (frmPo 신규입력 시 거래처를 지우면 부가세율 기본값 "10"이 같이 지워짐, 2026-09-28).</summary>
    public void ClearSelf()
    {
        _syncingPair = true;
        try { Text = string.Empty; }
        finally { _syncingPair = false; }
    }

    private void NameControl_TextChanged(object? sender, EventArgs e)
    {
        if (_syncingPair || _nameControl == null || Text.Length == 0) return;

        // .Text가 아니라 EditValue로 "진짜 비었는지" 확인한다 - NameControl에 NullText(플레이스홀더)가
        // 설정되어 있으면 .Text가 빈 문자열 대신 그 플레이스홀더 문구를 돌려줄 수 있다(같은 함정을
        // popPopUp 검색창에서도 겪었다 - popPopUp.SearchAsync 주석 참고).
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
    /// 아무 일도 안 한다 - 기존 화면 동작에 영향 없음.
    ///
    /// 포커스를 얻었을 때의 값(_valueOnEnter)과 지금 값이 같으면(=사용자가 실제로 아무것도
    /// 안 고침) 아예 검색도 팝업도 건너뛴다 - 이름이 겹치는 부서처럼 원래부터 모호한 값이
    /// 이미 들어있는 필드를 그냥 탭으로 지나가기만 해도 매번 팝업이 뜨고, 그걸 닫으면 멀쩡한
    /// 값이 지워지는 오작동을 막기 위함(2026-09-12 실제 발견 - "부서명에 생산이라고 치면...
    /// 팝업에서 선택 안 하고 닫으면... 부서정보가 없어져버려").</summary>
    private async Task OnLeaveAsync()
    {
        if (string.IsNullOrEmpty(MatchField) || _fieldMap.Count == 0) return;

        var typed = Text?.Trim();
        if (string.Equals(typed, _valueOnEnter?.Trim(), StringComparison.OrdinalIgnoreCase)) return;
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
        // 진단용(2026-09-23) - "..." 버튼을 눌러도 예외 메시지조차 안 뜬다는 보고가 있어서,
        // 이 두 조기 return이 실제로 원인인지부터 확인한다. 둘 다 정상이면 원인이 다른 곳
        // (ButtonClick 자체가 아예 안 불림)이라는 뜻이므로 다음 조사 방향이 갈린다.
        if (PopupLookupProvider.OpenPopup == null)
        {
            MessageBox.Show("[진단] PopupLookupProvider.OpenPopup이 등록되어 있지 않습니다.", "진단");
            return;
        }
        if (string.IsNullOrEmpty(LookupKey))
        {
            MessageBox.Show($"[진단] LookupKey가 비어 있습니다. (_lookupKey={_lookupKey ?? "null"}, Properties={Properties?.GetType().Name ?? "null"})", "진단");
            return;
        }

        var result = await PopupLookupProvider.OpenPopup(LookupKey!, this, initialKeyword);
        if (result == null)
        {
            // initialKeyword가 있다 = OnLeaveAsync가 방금 타이핑된(실제로 바뀐) 값을 못 좁혀서
            // 자동으로 띄운 팝업이라는 뜻 - 그 값은 어느 행과도 확정되지 않았으므로 취소 시
            // 비워서 "선택 안 됨"을 명확히 한다(매핑된 필드도 EditValueChanged->ClearMappedFields로
            // 같이 비워짐). initialKeyword가 없다 = "..." 버튼으로 사용자가 직접 연 것이라 그냥
            // 둘러보다 취소했을 수 있으니 기존 값을 그대로 둔다.
            if (initialKeyword != null) Text = string.Empty;
            return;
        }

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
                    var value = result.Row.TryGetValue(kv.Key, out var v) ? v : null;
                    SetFieldValue(kv.Value, value);
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
                SetFieldValue(control, null);
            }
        }
        finally
        {
            _syncingPair = false;
        }
    }

    /// <summary>콤보(LookUpEdit 계열, 예: cboVatType)는 .Text로 선택 항목이 안 바뀐다(DevExpress는
    /// EditValue로 골라야 한다) - 대상이 LookUpEdit이면 EditValue를, 아니면(일반 텍스트박스) 기존대로
    /// .Text를 쓴다.</summary>
    private static void SetFieldValue(Control control, string? value)
    {
        if (control is DevExpress.XtraEditors.LookUpEdit lookupEdit)
            lookupEdit.EditValue = string.IsNullOrEmpty(value) ? null : value;
        else
            control.Text = value ?? string.Empty;
    }
}
