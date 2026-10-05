using System.ComponentModel;
using System.Drawing;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Popup;

/// <summary>화면이 공통 버튼 패널(FeatureBarWyn)에 알려줘야 하는 "지금 열려 있는 문서" 정보.</summary>
public class FeatureContext
{
    /// <summary>저장된 문서의 PK(예: in_id) - 아직 저장 전(신규)이면 null. 결재/첨부는 저장된 문서에만 쓸 수 있다.</summary>
    public long? DocId { get; set; }
    public string DocNo { get; set; } = string.Empty;
    /// <summary>전자결재 제목/본문 기본값(상신 전 작성 화면에 미리 채워짐).</summary>
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    /// <summary>저장하지 않은 변경이 있는지 - 있으면 결재/첨부를 열기 전에 저장하게 한다.</summary>
    public bool HasUnsavedChanges { get; set; }
    /// <summary>이 문서에 연결된 결재번호(TAPDOC.app_no)와 진행상태(AP0001: 0 결재상신/1 진행중/E 승인완료/R 반려) - 화면 조회 결과에서 채운다.</summary>
    public string? AppNo { get; set; }
    public string? ApprStatCd { get; set; }
}

/// <summary>FeatureBarWyn을 올린 화면이 구현한다 - 문서 정보를 돌려주고, 기능 사용 후(결재 상태 변경 등) 다시 조회하게 한다.</summary>
public interface IFeatureHost
{
    FeatureContext GetFeatureContext();

    /// <summary>기능을 쓰고 난 뒤 호출된다(featureCd = SM0012 코드). 결재 상태가 바뀌었으면 화면이 문서를 다시 조회하면 된다.</summary>
    void OnFeatureChanged(string featureCd);
}

/// <summary>
/// 화면 공통 "기능 버튼 패널" - 메뉴등록(TSMMENUFEATURE)에서 이 화면에 켜둔 기능(전자결재/첨부파일...)의 버튼만 보여준다(2026-10-03).
/// 구매요청등록의 전자결재 버튼이 있던 자리(헤더 위 얇은 띠)에 이 컨트롤 하나를 올리면 되고, 켜진 기능이 없으면 패널 자체가 숨겨진다.
///
/// 쓰는 법: 디자이너에서 Dock=Top으로 올리고, 화면(BaseForm)이 IFeatureHost를 구현한다. 문서를 조회/신규 전환할 때마다 UpdateState()를 불러
/// 결재 상태 표시를 갱신하고, 결재가 걸린 문서의 편집 잠금은 ApprovalLocked로 판단한다(상신 후 수정 불가 - 반려는 다시 수정 가능).
/// 기능을 늘리려면 아래 Registry에 한 줄(코드/버튼 글자/동작)을 추가하고 공통코드 SM0012에 같은 코드를 등록하면 된다.
/// 디자인 타임에는 모든 기능 버튼을 보여줘서 디자이너에서 배치를 볼 수 있다.
/// </summary>
[ToolboxItem(true)]
public class FeatureBarWyn : PanelWyn
{
    private sealed class FeatureDef
    {
        public string Code { get; }
        public string Caption { get; }
        public int Width { get; }
        public string Tip { get; }
        public Func<FeatureBarWyn, Task> Execute { get; }

        public FeatureDef(string code, string caption, int width, string tip, Func<FeatureBarWyn, Task> execute)
        {
            Code = code; Caption = caption; Width = width; Tip = tip; Execute = execute;
        }
    }

    // 새 화면 기능은 여기 한 줄 + 공통코드 SM0012 한 줄. 표시 순서 = 이 순서.
    private static readonly FeatureDef[] Registry =
    {
        new("APPROVAL", "전자결재", 100, "전자결재 팝업을 엽니다(상신/승인/반려).", bar => bar.OpenApprovalAsync()),
        new("FILE", "첨부파일", 100, "첨부파일 팝업을 엽니다.", bar => bar.OpenFileAsync()),
    };

    private readonly Dictionary<string, ButtonWyn> _buttons = new();
    private readonly Dictionary<string, MenuFeatureDto> _enabled = new();
    private readonly LabelControlLite _lblState = new();

    /// <summary>메뉴 기능을 읽어서 버튼을 만들고 난 뒤(화면이 열린 직후) 발생 - 화면은 여기서 잠금/버튼 상태를 다시 맞춘다
    /// (생성자 시점엔 아직 MenuId가 안 채워져 기능 목록을 알 수 없다).</summary>
    public event EventHandler? FeaturesLoaded;

    public FeatureBarWyn()
    {
        Dock = DockStyle.Top;
        Height = 33;
        Padding = new Padding(5, 0, 0, 2);
        Appearance.BackColor = Color.White;
        Appearance.Options.UseBackColor = true;

        _lblState.Visible = false;
        Controls.Add(_lblState);
    }

    // ------------------------------------------------------------------ 기능 목록

    public bool HasFeature(string featureCd) => _enabled.ContainsKey(featureCd);
    public string? FeatureOption(string featureCd) => _enabled.TryGetValue(featureCd, out var f) ? f.OptionVal : null;

    public bool ApprovalEnabled => HasFeature("APPROVAL");

    /// <summary>결재가 걸린 문서가 지금 수정 불가 상태인지 - 결재를 쓰는 메뉴에서 상신된(진행/승인완료) 문서. 반려(R)나 아직 상신 전은 false.
    /// 서버(FN_AP_IS_LOCKED)도 같은 기준으로 저장을 거부한다.</summary>
    public bool ApprovalLocked => ApprovalEnabled && GetHost()?.GetFeatureContext().ApprStatCd is "0" or "1" or "E";

    private IFeatureHost? GetHost() => FindForm() as IFeatureHost;

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            // 디자이너: 모든 기능 버튼을 보여준다(배치 확인용).
            foreach (var def in Registry) EnsureButton(def);
            LayoutButtons();
            return;
        }
        // 폼의 MenuId가 채워진 뒤(보통 Show 직전)에 읽는다 - 핸들이 만들어지는 시점은 Show 이후다.
        BeginInvoke(new Action(LoadFeatures));
    }

    private void LoadFeatures()
    {
        _enabled.Clear();
        if (FindForm() is BaseForm form)
        {
            var menu = SessionManager.Current.GetMenuAuth(form.MenuId);
            foreach (var f in menu?.Features ?? new List<MenuFeatureDto>())
                if (f.UseYn && Registry.Any(r => r.Code == f.FeatureCd)) _enabled[f.FeatureCd] = f;
        }

        foreach (var def in Registry)
        {
            if (_enabled.ContainsKey(def.Code)) EnsureButton(def);
            else if (_buttons.TryGetValue(def.Code, out var old)) { Controls.Remove(old); old.Dispose(); _buttons.Remove(def.Code); }
        }
        LayoutButtons();
        Visible = _enabled.Count > 0;   // 켜진 기능이 없으면 패널 자체를 숨겨 자리를 차지하지 않게 한다.
        UpdateState();
        FeaturesLoaded?.Invoke(this, EventArgs.Empty);
    }

    private void EnsureButton(FeatureDef def)
    {
        if (_buttons.ContainsKey(def.Code)) return;
        var btn = new ButtonWyn { Text = def.Caption, Size = new Size(def.Width, 24), ToolTip = def.Tip, Anchor = AnchorStyles.Top | AnchorStyles.Left };
        btn.Click += async (s, e) =>
        {
            try { await def.Execute(this); }
            catch (Exception ex) { AppMessageBox.Show($"{def.Caption} 처리 중 오류가 발생했습니다.\n{ex.Message}", "오류"); }
        };
        _buttons[def.Code] = btn;
        Controls.Add(btn);
    }

    private void LayoutButtons()
    {
        var x = 6;
        foreach (var def in Registry)
        {
            if (!_buttons.TryGetValue(def.Code, out var btn)) continue;
            btn.Location = new Point(x, 4);
            x += def.Width + 6;
        }
        _lblState.Location = new Point(x + 6, 9);
    }

    // ------------------------------------------------------------------ 상태 표시

    /// <summary>화면이 문서를 조회/신규 전환할 때마다 부른다 - 결재 상태("결재: AP... · 승인중")를 갱신한다.</summary>
    public void UpdateState()
    {
        if (DesignMode) return;
        var ctx = GetHost()?.GetFeatureContext();
        if (!ApprovalEnabled || ctx == null || string.IsNullOrEmpty(ctx.AppNo))
        {
            _lblState.Visible = false;
            return;
        }

        var stat = ctx.ApprStatCd switch { "0" => "결재상신", "1" => "승인중", "E" => "승인완료", "R" => "반려", _ => string.Empty };
        _lblState.Text = $"결재: {ctx.AppNo}" + (stat.Length > 0 ? $"  ·  {stat}" : string.Empty);
        _lblState.ForeColor = ctx.ApprStatCd == "R" ? Color.FromArgb(191, 62, 62) : ctx.ApprStatCd == "E" ? Color.FromArgb(56, 142, 60) : Color.FromArgb(79, 142, 247);
        _lblState.Visible = true;
    }

    // ------------------------------------------------------------------ 기능 동작

    private FeatureContext? RequireSavedDocument(string what)
    {
        var host = GetHost();
        if (host == null) return null;
        var ctx = host.GetFeatureContext();
        if (ctx.DocId == null)
        {
            AppMessageBox.Show($"먼저 저장한 뒤 {what}을(를) 열어주세요.", "안내");
            return null;
        }
        if (ctx.HasUnsavedChanges)
        {
            AppMessageBox.Show($"저장하지 않은 변경이 있습니다. 먼저 저장한 뒤 {what}을(를) 열어주세요.", "안내");
            return null;
        }
        return ctx;
    }

    private async Task OpenApprovalAsync()
    {
        var ctx = RequireSavedDocument("전자결재");
        if (ctx == null) return;
        var docType = FeatureOption("APPROVAL");
        if (string.IsNullOrWhiteSpace(docType))
        {
            AppMessageBox.Show("결재 문서유형이 지정되지 않았습니다. 메뉴등록 화면의 화면 기능에서 지정해주세요.", "안내");
            return;
        }

        var changed = await popApp.ShowAsync(docType, ctx.DocId!.Value, ctx.DocNo, ctx.Title, ctx.Text, FindForm()!);
        if (changed) GetHost()?.OnFeatureChanged("APPROVAL");
    }

    private Task OpenFileAsync()
    {
        var ctx = RequireSavedDocument("첨부파일");
        if (ctx == null) return Task.CompletedTask;

        // 첨부 구분(doc_type)은 메뉴 옵션, 비우면 화면 클래스명(폴더명으로 쓰이므로 영문/숫자).
        var docType = FeatureOption("FILE") is { Length: > 0 } opt ? opt : FindForm()!.GetType().Name;
        popFileUpload.ShowAsync(docType, ctx.DocId!.Value, ctx.DocNo, 0, this);
        GetHost()?.OnFeatureChanged("FILE");
        return Task.CompletedTask;
    }

    // ------------------------------------------------------------------ 상태 라벨(가벼운 라벨)

    /// <summary>DevExpress LabelControl 대신 쓰는 최소한의 라벨 - 텍스트/색만 필요하고 폰트는 앱 표준(AppFonts.Body)이면 된다.</summary>
    private sealed class LabelControlLite : System.Windows.Forms.Label
    {
        public LabelControlLite()
        {
            AutoSize = true;
            Font = AppFonts.Body;
            BackColor = Color.Transparent;
        }
    }
}
