using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WYNLAB.Base;

/// <summary>
/// 공용 팝업(popPopUp)의 조회조건 영역을 자동 생성 대신 직접 디자인하고 싶을 때 상속하는 UserControl
/// (2026-09-25 - 조회조건에 룩업/여러 줄/조건 간 연동이 필요해지면 sysPopUpS 정의만으로는 너무
/// 어려워져서, 그런 팝업만 VS 디자이너로 만든 패널을 붙이는 방식). 그리드 결과는 그대로 sysPopUpD
/// 정의대로 자동 생성된다.
///
/// 사용법: 이 클래스를 상속한 UserControl(.cs + .Designer.cs)을 만들고, 그 클래스의 전체 이름(예:
/// WYNLAB.Popup.pnlItemSearch)을 팝업관리 화면의 "검색패널"에 넣는다. 조회조건 컨트롤(DevExpress
/// BaseEdit 계열)과 프로시저 파라미터의 연결은 두 가지 중 하나:
///  ① 팝업관리 조회조건 그리드의 "패널 컨트롤"(sysPopUpS.control_nm)에 컨트롤 Name을 지정 - 컨트롤
///     이름이 파라미터명과 달라도 되고, 같은 패널을 다른 프로시저에 재사용할 수도 있다.
///  ② 지정하지 않은 파라미터는 Name이 파라미터명(앞의 '@' 뗀 것, 예: p_asset_type)과 똑같은 컨트롤에서
///     읽는다(별도 연결 작업 없이 이름만 맞추면 됨).
/// 클래스는 이미 로드된 어셈블리에서 이름으로 찾는다 - WYNLAB.Popup(CoreAssembly)의 SearchPanels
/// 폴더에 두면 항상 찾을 수 있다. 못 찾거나 만들다 실패하면 기본(자동 생성) 검색창으로 열린다.
/// </summary>
public class PopupSearchPanelBase : UserControl
{
    public const string ParamPrefix = "p_";

    private readonly Dictionary<string, string> _paramToControl = new(StringComparer.OrdinalIgnoreCase);

    public PopupSearchPanelBase()
    {
        // 업무화면 조회조건 패널(BaseForm.ApplySearchPanelStyle의 panHeader)과 같은 배경색. 투명으로
        // 두면 VS 디자이너의 어두운 작업영역 위에서 패널 영역이 안 보이고, 런타임엔 흰 팝업 본문과
        // 검색영역이 구분되지 않는다(2026-09-25). 테두리는 호스트(PanelWyn)가 그린다.
        BackColor = Color.FromArgb(241, 245, 249);
    }

    /// <summary>클래스 전체 이름(예: "WYNLAB.Popup.pnlItemSearch")으로 이미 로드된 어셈블리에서 패널을
    /// 만든다. 엔진(WYNLAB.Popup)과 팝업관리 화면이 똑같이 쓴다. 이름이 비어있으면 null(에러 없음),
    /// 못 찾거나 만들다 실패하면 null + error에 사유.</summary>
    public static PopupSearchPanelBase? TryCreate(string? className, out string? error)
    {
        error = null;
        className = className?.Trim();
        if (string.IsNullOrEmpty(className)) return null;

        try
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type? type;
                try { type = assembly.GetType(className, throwOnError: false); }
                catch { continue; } // 동적/깨진 어셈블리는 건너뜀

                if (type != null && typeof(PopupSearchPanelBase).IsAssignableFrom(type))
                    return (PopupSearchPanelBase)Activator.CreateInstance(type)!;
            }

            error = $"검색패널 클래스({className})를 찾지 못했습니다. 이름과, 그 클래스가 든 모듈이 로드돼 있는지 확인해주세요.";
        }
        catch (Exception ex)
        {
            error = $"검색패널({className})을 만들지 못했습니다.\n{ex.Message}";
        }
        return null;
    }

    /// <summary>파라미터명 -> 컨트롤 Name 연결(sysPopUpS.control_nm). 엔진이 패널을 만든 직후 넘겨준다.</summary>
    public void SetMappings(IEnumerable<KeyValuePair<string, string>> paramToControl)
    {
        _paramToControl.Clear();
        foreach (var kv in paramToControl) _paramToControl[kv.Key] = kv.Value;
    }

    /// <summary>패널 안의 모든 편집 컨트롤 Name - 팝업관리 화면이 "패널 컨트롤" 콤보 목록으로 쓴다.</summary>
    public IReadOnlyList<string> GetEditorNames() =>
        AllEditors().Where(e => !string.IsNullOrEmpty(e.Name)).Select(e => e.Name).ToList();

    private List<BaseEdit> AllEditors()
    {
        var list = new List<BaseEdit>();
        Collect(this, list);
        return list.OrderBy(e => e.TabIndex).ThenBy(e => e.Top).ThenBy(e => e.Left).ToList();
    }

    private static void Collect(Control root, List<BaseEdit> list)
    {
        foreach (Control child in root.Controls)
        {
            if (child is BaseEdit edit) list.Add(edit);
            Collect(child, list);
        }
    }

    /// <summary>조회조건으로 쓰이는 컨트롤 - 연결(control_nm)로 지정됐거나, 연결 없이 이름이 p_로 시작.</summary>
    protected IEnumerable<BaseEdit> ParamEditors()
    {
        var mapped = new HashSet<string>(_paramToControl.Values, StringComparer.OrdinalIgnoreCase);
        return AllEditors().Where(e => mapped.Contains(e.Name) ||
            e.Name.StartsWith(ParamPrefix, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>프로시저에 넘길 "파라미터명 -> 입력값". 기본 구현: ① control_nm으로 연결된 파라미터는 그
    /// 컨트롤에서, ② 연결에 안 쓰인 p_ 컨트롤은 자기 이름을 파라미터명으로 - 값 변환이 특별한 컨트롤이
    /// 있으면 override. 연결된 컨트롤이 자기 이름으로도 한 번 더 넘어가지 않게(프로시저에 없는
    /// 파라미터라 오류) ②에서는 연결에 쓰인 컨트롤을 뺀다.</summary>
    public virtual Dictionary<string, string?> GetConditions()
    {
        var all = AllEditors();
        var result = new Dictionary<string, string?>();

        foreach (var pair in _paramToControl)
        {
            var edit = all.FirstOrDefault(e => string.Equals(e.Name, pair.Value, StringComparison.OrdinalIgnoreCase));
            if (edit != null) result[pair.Key] = ExtractValue(edit);
        }

        var mapped = new HashSet<string>(_paramToControl.Values, StringComparer.OrdinalIgnoreCase);
        foreach (var edit in all)
        {
            if (mapped.Contains(edit.Name)) continue;
            if (edit.Name.StartsWith(ParamPrefix, StringComparison.OrdinalIgnoreCase)) result[edit.Name] = ExtractValue(edit);
        }
        return result;
    }

    /// <summary>팝업을 여는 쪽이 이미 타이핑한 값(initialKeyword)이 있을 때 미리 채워 넣는다. 기본은
    /// 첫 번째 일반 텍스트 입력칸 하나에만(룩업/날짜/버튼형 제외) - 자동 생성 검색창과 같은 규칙.</summary>
    public virtual void SetInitialKeyword(string? keyword)
    {
        if (string.IsNullOrEmpty(keyword)) return;
        var target = ParamEditors().FirstOrDefault(e => e is TextEdit && e is not ButtonEdit);
        if (target != null) target.EditValue = keyword;
    }

    /// <summary>팝업을 여는 화면이 넘긴 추가 조건(PopupLookupProvider.ExtraConditions)을, 같은 이름(파라미터명 또는 연결된
    /// 컨트롤 Name)의 조회조건 컨트롤이 있고 아직 비어 있으면 초기값으로 채운다 - 날짜 칸은 yyyyMMdd/yyyy-MM-dd를 받는다.</summary>
    public virtual void ApplyExtraConditions(IDictionary<string, string?>? extra)
    {
        if (extra == null) return;

        var all = AllEditors();
        foreach (var kv in extra)
        {
            if (string.IsNullOrEmpty(kv.Value)) continue;

            var controlName = _paramToControl.TryGetValue(kv.Key, out var mapped) ? mapped : kv.Key;
            var edit = all.FirstOrDefault(e => string.Equals(e.Name, controlName, StringComparison.OrdinalIgnoreCase));
            if (edit == null || !(edit.EditValue == null || edit.EditValue is DBNull || (edit.EditValue is string s && s.Length == 0))) continue;

            if (edit is DateEdit)
            {
                var digits = new string(kv.Value.Where(char.IsDigit).ToArray());
                if (DateTime.TryParseExact(digits, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dt))
                    edit.EditValue = dt;
            }
            else edit.EditValue = kv.Value;
        }
    }

    /// <summary>엔진이 패널을 올린 뒤 부른다 - 조건 칸에서 Enter를 치면 조회하도록 연결.</summary>
    public void Initialize(Action onSearch)
    {
        foreach (var edit in ParamEditors())
        {
            edit.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.Handled = true;
                onSearch();
            };
        }
    }

    /// <summary>.Text가 아니라 EditValue를 쓴다 - NullText(플레이스홀더)가 .Text로 새어 나오는 것을 피한다.</summary>
    private static string? ExtractValue(BaseEdit edit) => edit.EditValue switch
    {
        null or DBNull => null,
        DateTime dt => dt.ToString("yyyy-MM-dd"),
        bool b => b ? "Y" : "N",
        var v => Convert.ToString(v)
    };
}
