using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// 사용자가 이 PC에서 접속할 서비스(서버)를 직접 추가/수정/삭제하는 화면. appsettings.json에
/// 내장된 서비스(예: Development/Production)도 값뿐 아니라 이름까지 자유롭게 고쳐 쓸 수 있다
/// (AppConfig.OverrideBuiltin 참고) - 이 앱을 개발자 1명이 여러 고객사(A사/B사...) 서버를 오가며
/// 쓰는 경우, appsettings.json에 박힌 기본 이름들도 이 PC에서만큼은 그 고객사 이름으로 자유롭게
/// 바꿔 쓸 수 있어야 한다는 요청으로 "회사 공통이라 수정 불가" 제한을 없앴다(2026-09-13). 내장
/// 서비스를 고치면 AppConfig.AddSite/UpdateSite/RemoveSite로 관리하는 "이 PC에만 저장된" 목록에
/// 평범한 사용자 서비스로 편입되고(원본 이름은 목록에서 빠짐), 그 항목을 삭제하면 원본이 다시
/// 나타난다 - 재배포 없이 사용자가 원하는 만큼 서비스를 등록해두고 로그인 화면에서 바로 골라
/// 쓸 수 있다.
///
/// 접속 정보 입력은 API 주소 + "정적 리소스 서버 주소" 2가지만 받는다 - Modules/CoreAssembly/
/// Assets는 SERVER_SETUP.md의 고정 배치(ClickOnce 사이트 하나 밑에 가상 디렉터리 3개, 예:
/// http://host:8091/Modules, /CoreAssembly, /Assets)를 그대로 따르므로, 정적 리소스 서버
/// 주소(예: http://host:8091) 하나만 받아서 세 경로를 자동으로 만든다(CombineStaticPath 참고).
/// UNC 공유폴더 방식(\\서버\WYNLAB)도 같은 방식으로 하위 폴더만 다르게 붙는 구조라 그대로
/// 지원된다. AppConfig.AddSite/UpdateSite 자체는 여전히 세 경로를 따로 받는 시그니처를 유지한다
/// (sites.json 형식이 appsettings.json의 Environment 항목과 계속 같은 모양이어야 하므로) -
/// 이 화면(입력 UI)만 사용자 입력을 1개로 줄이고 내부에서 조합해서 넘긴다.
/// </summary>
public class SiteManagerForm : XtraForm
{
    private const int HeaderHeight = 36;

    private readonly PanelControl headerPanel = new();
    private readonly LabelControl lblCaption = new() { Text = "서비스 관리" };
    private readonly CloseGlyph btnClose = new();
    private readonly GridControlWyn grid = new();
    private readonly LabelControl lblSectionTitle = new() { Text = "새 서비스 추가" };
    private readonly LabelControl lblStaticBase = new();
    private readonly TextEditWyn txtName = new();
    private readonly TextEditWyn txtApiBaseUrl = new();
    private readonly TextEditWyn txtStaticBase = new();
    private readonly SimpleButton btnSave = new() { Text = "추가" };
    private readonly SimpleButton btnCancelEdit = new() { Text = "취소" };
    private readonly SimpleButton btnDelete = new() { Text = "선택 항목 삭제" };
    private readonly SimpleButton btnSelect = new() { Text = "선택" };
    private readonly SimpleButton btnCloseFooter = new() { Text = "닫기" };

    private Point _dragStart;
    private bool _dragging;

    /// <summary>사용자가 "선택"(또는 그리드 더블클릭)으로 로그인 화면에 적용할 서비스를 골랐을 때
    /// 그 이름 - LoginForm이 ShowDialog 반환값이 OK일 때 이 값을 읽어 드롭다운에 반영한다.
    /// null이면 그냥 관리만 하다가 닫은 것(추가/수정/삭제만 했거나 X로 닫음).</summary>
    public string? SelectedSiteName { get; private set; }

    /// <summary>지금 입력폼이 어느 서비스를 수정하는 중인지 - null이면 "새로 추가" 모드.
    /// 이름 자체도 바꿀 수 있어야 해서(UpdateSite/OverrideBuiltin이 이름 변경을 지원) 원래
    /// 이름을 따로 들고 있어야 한다(txtName.Text는 사용자가 바꿔버릴 수 있으므로).</summary>
    private string? _editingSiteName;

    /// <summary>_editingSiteName이 가리키는 서비스가 아직 한 번도 오버라이드 안 된 순수 내장
    /// 서비스인지 - true면 저장 시 UpdateSite가 아니라 OverrideBuiltin을 호출해야 한다(AppConfig가
    /// 그 이름을 UserSites에서 못 찾아 UpdateSite가 조용히 무시해버리므로).</summary>
    private bool _editingIsBuiltin;

    private class SiteRow
    {
        public string Name { get; set; } = string.Empty;
        public string ApiBaseUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public SiteManagerForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        Size = new Size(480, 420);
        ShowInTaskbar = false;

        BuildHeader();
        BuildBody();

        Controls.Add(BuildBodyPanel());
        Controls.Add(headerPanel);

        RefreshGrid();

        // 그리드는 데이터를 바인딩하는 순간 자동으로 첫 행에 포커스를 준다 - 그런데 이건
        // "포커스 변경"이 아니라 "최초 포커스"라서 FocusedRowObjectChanged가 안 불린다(이미
        // 포커스된 그 행을 나중에 다시 클릭해도 마찬가지 - "바뀐" 게 없어서 이벤트가 안 뜬다).
        // 그래서 화면을 열자마자 첫 행(보통 Development)의 접속정보가 안 보이는 버그가 있었다
        // (2026-09-13, feedback_query_refocus_after_save와 같은 종류의 함정) - 여기서 직접 한
        // 번 불러서 그 초기 포커스 상태를 반영한다.
        OnGridFocusChanged();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(210, 212, 217));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    private void BuildHeader()
    {
        var brandColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = HeaderHeight;
        headerPanel.Appearance.BackColor = brandColor;
        headerPanel.Appearance.Options.UseBackColor = true;
        headerPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        lblCaption.Location = new Point(14, 9);
        lblCaption.AutoSizeMode = LabelAutoSizeMode.None;
        lblCaption.Appearance.ForeColor = Color.White;
        lblCaption.Appearance.Font = AppFonts.BodyBold;

        btnClose.Size = new Size(24, 24);
        btnClose.HeaderColor = brandColor;
        btnClose.Cursor = Cursors.Hand;
        btnClose.Click += (s, e) => Close();

        headerPanel.Controls.Add(lblCaption);
        headerPanel.Controls.Add(btnClose);

        // 캡션/닫기 위치는 headerPanel의 "실제" 폭이 정해진 뒤에 잡는다 - 생성자 시점의 Width는
        // 아직 폼 기준이라 어긋난다(AppMessageBox.LayoutHeader와 같은 이유/같은 해법).
        headerPanel.Resize += (s, e) => LayoutHeader();
        LayoutHeader();

        EnableDrag(headerPanel);
        EnableDrag(lblCaption);
    }

    private void LayoutHeader()
    {
        var w = headerPanel.ClientSize.Width;
        if (w <= 0) return;

        btnClose.Location = new Point(w - btnClose.Width - 8, (HeaderHeight - btnClose.Height) / 2);
        lblCaption.Size = new Size(Math.Max(0, btnClose.Left - 14 - 8), 18);
    }

    private void BuildBody()
    {
        // GridControlWyn은 이제 View를 자동으로 안 만든다(디자이너 컬럼 저장 문제 때문에 뺐음 -
        // GridControlWyn.cs 주석 참고). 이 화면은 디자이너가 아니라 코드로 직접 만드니 여기서
        // 그냥 하나 만들어서 연결해준다.
        grid.MainView = new GridViewWyn();
        grid.View!.OptionsView.ShowGroupPanel = false;
        grid.View.OptionsBehavior.Editable = false;
        grid.View.EmptyText = "추가된 서비스가 없습니다.";
        grid.View.FocusedRowObjectChanged += (s, e) => OnGridFocusChanged();
        // 더블클릭으로도 "선택"과 같은 동작(로그인 화면에 바로 적용) - 더블클릭 시점엔 마우스
        // 다운으로 포커스 행이 이미 그 행으로 옮겨진 뒤라 GetFocusedRow()를 그대로 믿을 수 있다.
        grid.View.DoubleClick += (s, e) => SelectFocusedRow();
    }

    private Control BuildBodyPanel()
    {
        var body = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16) };

        grid.Location = new Point(16, 16);
        grid.Size = new Size(448, 170);
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        lblSectionTitle.Location = new Point(16, 200);
        lblSectionTitle.AutoSizeMode = LabelAutoSizeMode.None;
        lblSectionTitle.Size = new Size(300, 16);
        lblSectionTitle.Appearance.ForeColor = Color.FromArgb(140, 140, 140);
        lblSectionTitle.Appearance.Font = AppFonts.Caption;

        const int leftX = 16;
        const int rightX = 244;
        const int fieldWidth = 220;
        var y = 224;

        var lblName = FieldLabel("이름", leftX, y);
        txtName.Location = new Point(leftX, y + 20);
        txtName.Size = new Size(fieldWidth, 24);
        txtName.Font = AppFonts.Body;

        var lblUrl = FieldLabel("API 주소", rightX, y);
        txtApiBaseUrl.Location = new Point(rightX, y + 20);
        txtApiBaseUrl.Size = new Size(fieldWidth, 24);
        txtApiBaseUrl.Font = AppFonts.Body;
        txtApiBaseUrl.Properties.NullText = "http://host:8090/";
        y += 56;

        lblStaticBase.Text = "정적 리소스 서버 주소 (Modules/CoreAssembly/Assets)";
        lblStaticBase.Location = new Point(leftX, y);
        lblStaticBase.AutoSizeMode = LabelAutoSizeMode.None;
        lblStaticBase.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lblStaticBase.Appearance.Font = AppFonts.Caption;
        lblStaticBase.Size = new Size(fieldWidth * 2 - 8, 16);
        txtStaticBase.Location = new Point(leftX, y + 20);
        txtStaticBase.Size = new Size(fieldWidth * 2 - 8, 24);
        txtStaticBase.Font = AppFonts.Body;
        txtStaticBase.Properties.NullText = "http://host:8091 또는 \\\\서버\\WYNLAB";
        y += 56;

        btnSave.Location = new Point(leftX, y);
        btnSave.Size = new Size(90, 28);
        btnSave.Click += BtnSave_Click;

        btnCancelEdit.Location = new Point(leftX + 96, y);
        btnCancelEdit.Size = new Size(70, 28);
        btnCancelEdit.Click += (s, e) => SetEditingSite(null);

        btnDelete.Location = new Point(leftX + 172, y);
        btnDelete.Size = new Size(120, 28);
        btnDelete.Click += BtnDelete_Click;

        btnSelect.Location = new Point(leftX + 296, y);
        btnSelect.Size = new Size(52, 28);
        btnSelect.Click += (s, e) => SelectFocusedRow();

        btnCloseFooter.Location = new Point(rightX + 124, y);
        btnCloseFooter.Size = new Size(96, 28);
        btnCloseFooter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCloseFooter.Click += (s, e) => Close();

        body.Controls.Add(grid);
        body.Controls.Add(lblSectionTitle);
        body.Controls.Add(lblName);
        body.Controls.Add(txtName);
        body.Controls.Add(lblUrl);
        body.Controls.Add(txtApiBaseUrl);
        body.Controls.Add(lblStaticBase);
        body.Controls.Add(txtStaticBase);
        body.Controls.Add(btnSave);
        body.Controls.Add(btnCancelEdit);
        body.Controls.Add(btnDelete);
        body.Controls.Add(btnSelect);
        body.Controls.Add(btnCloseFooter);

        // 정적 리소스 필드/닫기 버튼은 "폼이 480px일 것"이라는 가정으로 잡은 고정폭(432 등) -
        // 실제 렌더링 폭(DPI 배율 등으로 달라질 수 있음)에 안 맞으면 오른쪽이 잘려 보인다
        // (닫기 버튼이 통째로 화면 밖으로 밀려난 사례 실제 확인). LayoutHeader와 같은 이유로
        // body의 "실제" ClientSize를 기준으로 다시 계산한다.
        body.Resize += (s, e) => LayoutBody(body, leftX);
        LayoutBody(body, leftX);

        return body;
    }

    private void LayoutBody(Panel body, int leftX)
    {
        var w = body.ClientSize.Width;
        if (w <= 0) return;

        const int margin = 16;
        var fieldsWidth = Math.Max(0, w - leftX - margin);

        grid.Width = fieldsWidth;
        lblStaticBase.Width = fieldsWidth;
        txtStaticBase.Width = fieldsWidth;

        btnCloseFooter.Location = new Point(Math.Max(leftX, w - margin - btnCloseFooter.Width), btnCloseFooter.Top);
    }

    private static LabelControl FieldLabel(string text, int x, int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 16) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        return lbl;
    }

    private void RefreshGrid()
    {
        // 회사 공통(내장) 서비스도 실제 접속 주소를 그대로 보여준다 - 예전엔 "(회사 공통)"이라는
        // 문구로 가려뒀었는데, 그게 바로 "도대체 어디 접속하고 있는건지 알수가 없다"(2026-09-13)
        // 지적의 원인이었다. GetSiteInfo는 내장/사용자 서비스 모두를 같은 방식으로 조회하므로
        // 여기서도 구분 없이 그대로 쓴다.
        var rows = AppConfig.AvailableEnvironments
            .Select(name =>
            {
                var userSite = AppConfig.UserSites.FirstOrDefault(s => s.Name == name);
                var type = userSite == null ? "기본 제공"
                    : userSite.OverridesBuiltin != null ? "수정함"
                    : "내가 추가함";
                return new SiteRow
                {
                    Name = name,
                    ApiBaseUrl = AppConfig.GetSiteInfo(name)?.ApiBaseUrl ?? string.Empty,
                    Type = type
                };
            })
            .ToList();

        grid.DataSource = rows;
    }

    /// <summary>그리드에서 서비스를 고르면 그 값을 입력폼에 그대로 불러와서 바로 수정할 수 있게
    /// 한다("수정" 버튼 없이 선택 자체가 곧 편집 시작 - 클릭 한 번을 아낀다). 내장 서비스든
    /// 사용자 서비스든 구분 없이 전부 편집 가능(2026-09-13 - 내장 서비스도 자유롭게 고쳐 쓸 수
    /// 있어야 한다는 요청); 단, "삭제"는 사용자 서비스(오버라이드 포함)에만 허용한다 - 아직
    /// 한 번도 안 건드린 순수 내장 서비스는 지울 데이터 자체가 없다.</summary>
    private void OnGridFocusChanged()
    {
        var focused = GetFocusedRow();
        btnDelete.Enabled = focused != null && AppConfig.IsUserSite(focused.Name);
        btnSelect.Enabled = focused != null;

        if (focused == null)
        {
            SetEditingSite(null);
            return;
        }

        if (AppConfig.IsUserSite(focused.Name))
        {
            var site = AppConfig.UserSites.First(s => s.Name == focused.Name);
            SetEditingSite(site, isBuiltin: false);
            return;
        }

        // 아직 한 번도 오버라이드 안 된 순수 내장(appsettings.json) 항목 - GetSiteInfo로 값을
        // 가져와 폼에 채우고 그대로 편집 가능하게 연다. 저장하면 OverrideBuiltin이 호출된다
        // (BtnSave_Click 참고).
        var info = AppConfig.GetSiteInfo(focused.Name);
        if (info == null) { SetEditingSite(null); return; }

        var (apiBaseUrl, modulesPath, coreAssemblyPath, assetsPath) = info.Value;
        SetEditingSite(new UserSiteEntry
        {
            Name = focused.Name,
            ApiBaseUrl = apiBaseUrl,
            ModulesPath = modulesPath,
            CoreAssemblyPath = coreAssemblyPath,
            AssetsPath = assetsPath
        }, isBuiltin: true);
    }

    /// <summary>site가 null이면 "새로 추가" 모드. site가 있으면 그 서비스를 수정하는 모드
    /// ("저장") - isBuiltin은 저장 시 AppConfig.UpdateSite를 부를지 OverrideBuiltin을 부를지만
    /// 결정하고(BtnSave_Click 참고), 화면 표시 자체는 두 경우 동일하게 편집 가능한 입력폼이다.</summary>
    private void SetEditingSite(UserSiteEntry? site, bool isBuiltin = false)
    {
        _editingSiteName = site?.Name;
        _editingIsBuiltin = isBuiltin;

        if (site == null)
        {
            lblSectionTitle.Text = "새 서비스 추가";
            txtName.Text = string.Empty;
            txtApiBaseUrl.Text = string.Empty;
            txtStaticBase.Text = string.Empty;
            btnSave.Text = "추가";
            btnCancelEdit.Visible = false;
            return;
        }

        lblSectionTitle.Text = isBuiltin ? $"'{site.Name}' 수정 중 (기본 제공 서비스)" : $"'{site.Name}' 수정 중";
        txtName.Text = site.Name;
        txtApiBaseUrl.Text = site.ApiBaseUrl;
        txtStaticBase.Text = ExtractStaticBase(site);
        btnSave.Text = "저장";
        btnCancelEdit.Visible = true;
    }

    /// <summary>Modules/CoreAssembly/Assets 중 저장되어 있는 값 하나를 골라, 그 끝의 하위
    /// 폴더 이름("/Modules" 등)을 떼어내서 사용자가 원래 입력했을 "정적 리소스 서버 주소"를
    /// 되살린다. 세 값이 서로 다른 서버를 가리키게(사람이 sites.json을 직접 고치는 등) 되어
    /// 있어도, 이 화면에서 다시 저장하면 셋 다 같은 값으로 맞춰진다 - 그런 특수한 상황을
    /// 지원하려는 화면이 아니므로 이 정도 단순화는 의도적이다.</summary>
    private static string ExtractStaticBase(UserSiteEntry site)
    {
        var path = !string.IsNullOrWhiteSpace(site.ModulesPath) ? site.ModulesPath
            : !string.IsNullOrWhiteSpace(site.CoreAssemblyPath) ? site.CoreAssemblyPath
            : site.AssetsPath;
        if (string.IsNullOrWhiteSpace(path)) return string.Empty;

        foreach (var subfolder in new[] { "Modules", "CoreAssembly", "Assets" })
        {
            foreach (var separator in new[] { "/", "\\" })
            {
                var suffix = separator + subfolder;
                if (path.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    return path.Substring(0, path.Length - suffix.Length);
            }
        }
        return path; // 알아본 하위 폴더 이름이 아니면(직접 편집 등) 그대로 보여준다
    }

    /// <summary>"정적 리소스 서버 주소" 하나에 하위 폴더 이름을 붙여 실제 경로를 만든다.
    /// http(s) 주소면 "/"로, UNC 공유폴더(\\서버\...)면 "\"로 잇는다 - HttpFileSync.IsHttpUrl은
    /// WYNLAB.BaseForm 내부 전용(internal)이라 여기서 직접 못 쓰므로 같은 판정을 그대로
    /// 다시 구현한다(판정 기준 자체가 한 줄짜리라 중복이 문제 될 정도는 아님).</summary>
    private static string CombineStaticPath(string staticBase, string subfolder)
    {
        if (string.IsNullOrWhiteSpace(staticBase)) return string.Empty;

        var isHttp = staticBase.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                     staticBase.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        var separator = isHttp ? "/" : "\\";
        return staticBase.TrimEnd('/', '\\') + separator + subfolder;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var name = txtName.Text.Trim();
        var apiBaseUrl = txtApiBaseUrl.Text.Trim();
        var staticBase = txtStaticBase.Text.Trim();
        var modulesPath = CombineStaticPath(staticBase, "Modules");
        var coreAssemblyPath = CombineStaticPath(staticBase, "CoreAssembly");
        var assetsPath = CombineStaticPath(staticBase, "Assets");

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(apiBaseUrl))
        {
            AppMessageBox.Show("이름과 API 주소를 입력해주세요.", "확인");
            return;
        }

        try
        {
            if (_editingSiteName == null)
                AppConfig.AddSite(name, apiBaseUrl, modulesPath, coreAssemblyPath, assetsPath);
            else if (_editingIsBuiltin)
                AppConfig.OverrideBuiltin(_editingSiteName, name, apiBaseUrl, modulesPath, coreAssemblyPath, assetsPath);
            else
                AppConfig.UpdateSite(_editingSiteName, name, apiBaseUrl, modulesPath, coreAssemblyPath, assetsPath);
        }
        catch (ArgumentException ex)
        {
            AppMessageBox.Show(ex.Message, "확인");
            return;
        }

        SetEditingSite(null);
        RefreshGrid();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (grid.View!.GetFocusedRow() is not SiteRow row || !AppConfig.IsUserSite(row.Name)) return;

        if (AppMessageBox.Show($"'{row.Name}' 서비스를 삭제할까요?", "확인", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        AppConfig.RemoveSite(row.Name);
        SetEditingSite(null);
        RefreshGrid();
    }

    /// <summary>DataSource를 막 새로 세팅한 직후에는 "포커스된 행"이라는 내부 상태가 아직 그
    /// 행을 못 따라온 시점이 있어서(자동 첫 행 포커스는 즉시가 아니라 비동기적으로 반영됨 -
    /// GetFocusedRow()가 null을 돌려줌) GetFocusedRow()만 믿으면 화면을 막 열었을 때 아무 것도
    /// 안 보이는 버그가 있었다(2026-09-13, RefreshGrid 직후 호출에서 실제로 재현/확인함).
    /// GetRow(0)으로 실제 첫 데이터 행을 직접 읽어 대비한다 - 이건 DataSource 자체에서 바로
    /// 읽으므로 포커스 타이밍과 무관하게 항상 최신이다.</summary>
    private SiteRow? GetFocusedRow() =>
        grid.View!.GetFocusedRow() as SiteRow
            ?? (grid.View.RowCount > 0 ? grid.View.GetRow(0) as SiteRow : null);

    /// <summary>"선택" 버튼 또는 그리드 더블클릭 - 내장/사용자 서비스 구분 없이 지금 포커스된
    /// 행을 로그인 화면에 바로 적용하고 창을 닫는다. LoginForm이 DialogResult.OK +
    /// SelectedSiteName을 보고 cboEnvironment에 반영한다(2026-09-14 - "항목선택 기능이 없다"는
    /// 지적으로 추가).</summary>
    private void SelectFocusedRow()
    {
        var focused = GetFocusedRow();
        if (focused == null) return;

        SelectedSiteName = focused.Name;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void EnableDrag(Control control)
    {
        control.MouseDown += (s, e) => { _dragging = true; _dragStart = e.Location; };
        control.MouseMove += (s, e) =>
        {
            if (!_dragging) return;
            Location = new Point(Location.X + e.X - _dragStart.X, Location.Y + e.Y - _dragStart.Y);
        };
        control.MouseUp += (s, e) => { _dragging = false; };
    }

    /// <summary>헤더 닫기 버튼 - 예전엔 LabelControl에 유니코드 "✕" 글자를 넣었는데, 폰트에 따라
    /// 글자가 상하좌우로 살짝씩 잘려 보이는 문제가 있었다("이미지가 잘린 것처럼 보인다"는
    /// 지적, 2026-09-16). AppMessageBox.CloseGlyph와 같은 방식으로 직접 X를 그려서 항상 또렷하게
    /// 중앙에 오도록 한다.</summary>
    private class CloseGlyph : Control
    {
        private bool _hover;

        public Color HeaderColor { get; set; } = Color.Transparent;

        public CloseGlyph()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                      ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(HeaderColor);

            if (_hover)
            {
                using var hoverBrush = new SolidBrush(Color.FromArgb(38, 255, 255, 255));
                g.FillRectangle(hoverBrush, 0, 0, Width, Height);
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            const int inset = 8;
            using var pen = new Pen(Color.FromArgb(_hover ? 255 : 210, 255, 255, 255), 1.4f);
            g.DrawLine(pen, inset, inset, Width - inset - 1, Height - inset - 1);
            g.DrawLine(pen, Width - inset - 1, inset, inset, Height - inset - 1);
        }
    }
}
