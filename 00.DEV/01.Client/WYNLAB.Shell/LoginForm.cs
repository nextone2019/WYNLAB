using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using System.Drawing;
using System.Net.Http;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 화면.
/// - 타이틀바 없음(FormBorderStyle=None) - 시스템 X버튼이 아예 사라지고, 우측 상단의
///   커스텀 닫기 라벨(lblClose)로만 닫을 수 있음
/// - 좌측: 브랜드 영역(그라데이션 배경 + 로고 + 문구), 우측: 로그인 입력폼
/// - 서비스 선택(Development / Live) 콤보 - 선택 즉시 AppConfig.SwitchEnvironment 호출해서
///   로그인 요청이 실제로 어느 서버로 갈지 그 자리에서 바뀜
/// - 타이틀바가 없어서 창을 못 옮기니, 좌측 브랜드 영역을 드래그하면 창이 이동하도록 처리
/// </summary>
public partial class LoginForm : XtraForm
{
    private static readonly Color BrandColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

    // 플레인 WinForms Panel을 씀 - RoundedPanel(배지)이 Parent.BackColor를 그대로 참조해서
    // 배경을 지우는데, DevExpress PanelControl은 실제 렌더링에 Appearance.BackColor를 쓰고
    // 베이스 .BackColor는 반영되지 않아 배지 모서리 색이 어긋나 보이는 문제가 있었다.
    private readonly Panel leftPanel = new();
    private readonly TextEdit txtUserId = new();
    private readonly TextEdit txtPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly ComboBoxEdit cboEnvironment = new();
    private readonly LabelControl lnkManageSites = new() { Text = "서비스 관리" };
    private readonly SimpleButton btnLogin = new() { Text = "로그인" };
    private readonly LabelControl lblClose = new() { Text = "✕" };

    private readonly Dictionary<string, string> _envLabels = new()
    {
        ["Development"] = "Development",
        ["Production"] = "Live (Production)"
    };

    private Point _dragStart;
    private bool _dragging;

    /// <summary>로그인 성공 시 이 창을 닫기 직전에 띄운 스플래시 - Program.cs가 ShellForm을
    /// 다 띄운 뒤에 닫아준다. LoginForm 자신의 자식이 아니라 독립된 Form이어야 한다(클래스
    /// 상단 주석 참고 - 모달 대화상자 도중에 Hide()하면 ShowDialog()가 바로 반환돼버림).</summary>
    public SplashForm? Splash { get; private set; }

    public LoginForm()
    {
        Text = "WYN LAB 로그인";
        FormBorderStyle = FormBorderStyle.None;
        ControlBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(640, 420);
        BackColor = Color.White;

        BuildLeftPanel();
        BuildCloseButton();
        BuildRightPanel();

        Controls.Add(leftPanel);
    }

    /// <summary>
    /// 다이얼로그 전체 테두리를 얇게 그려서 배경과 경계를 분명하게 함
    /// (AppMessageBox/SplashForm과 같은 패턴 - 이 앱의 모든 커스텀 다이얼로그가 공유하는 규칙).
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(225, 226, 230));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    /// <summary>
    /// 좌측 브랜드 영역 - 단색 배경(예전엔 그라데이션이었는데, 플랫한 단색이 더 절제되고
    /// 고급스러워 보인다는 방향으로 정리) + 둥근 배지 로고 + 자간을 띄운 태그라인.
    /// 드래그로 창 이동 가능.
    /// </summary>
    private void BuildLeftPanel()
    {
        leftPanel.Dock = DockStyle.Left;
        leftPanel.Width = 240;
        leftPanel.BackColor = BrandColor;

        var badge = new Panel
        {
            BackColor = BrandColor,
            Size = new Size(44, 44),
            Location = new Point(32, 140)
        };
        badge.Paint += (s, e) => LogoPainter.Draw(e.Graphics, badge.ClientRectangle, darkBackground: true);

        var titleLabel = new LabelControl
        {
            Text = "WYN LAB",
            Location = new Point(32, 196),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(180, 40)
        };
        titleLabel.Appearance.ForeColor = Color.White;
        titleLabel.Appearance.Font = AppFonts.Display;

        // 배지-타이틀 밑에 얇은 포인트 선 하나 - 텍스트 블록에 정돈된 매듭을 지어주는 용도
        var accentLine = new Panel
        {
            Location = new Point(32, 244),
            Size = new Size(36, 2),
            BackColor = Color.FromArgb(140, 255, 255, 255)
        };

        // 글자 사이를 띄운 "킥커" 스타일 태그라인 - WinForms엔 자간(letter-spacing) 속성이
        // 없어서, 글자 사이에 공백을 직접 넣는 방식으로 흉내낸다. 작은 대문자 캡션이
        // 큰 타이틀 밑에서 절제된 브랜드 문구처럼 보이게 하는 흔한 로그인 화면 기법.
        var taglineLabel = new LabelControl
        {
            Text = "W H A T   Y O U   N E E D",
            Location = new Point(32, 256),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(200, 18)
        };
        taglineLabel.Appearance.ForeColor = Color.FromArgb(200, 255, 255, 255);
        taglineLabel.Appearance.Font = AppFonts.Caption;

        var versionLabel = new LabelControl
        {
            Text = $"v{DateTime.Now:yyyy.MM.dd}",
            Location = new Point(32, 380),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(180, 18)
        };
        versionLabel.Appearance.ForeColor = Color.FromArgb(150, 255, 255, 255);
        versionLabel.Appearance.Font = AppFonts.Caption;

        leftPanel.Controls.Add(badge);
        leftPanel.Controls.Add(titleLabel);
        leftPanel.Controls.Add(accentLine);
        leftPanel.Controls.Add(taglineLabel);
        leftPanel.Controls.Add(versionLabel);

        EnableDrag(leftPanel);
        EnableDrag(titleLabel);
        EnableDrag(taglineLabel);
    }

    /// <summary>우측 상단 커스텀 닫기 버튼 - 타이틀바가 없으므로 이게 유일한 닫기 수단</summary>
    private void BuildCloseButton()
    {
        lblClose.Location = new Point(Width - 44, 14);
        lblClose.AutoSizeMode = LabelAutoSizeMode.None;
        lblClose.Size = new Size(28, 28);
        lblClose.Appearance.ForeColor = Color.FromArgb(150, 150, 150);
        lblClose.Appearance.Font = AppFonts.SubHeading;
        lblClose.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblClose.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        lblClose.Cursor = Cursors.Hand;
        lblClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Controls.Add(lblClose);
        lblClose.BringToFront();
    }

    /// <summary>우측 로그인 입력 영역</summary>
    private void BuildRightPanel()
    {
        const int formX = 272;
        const int fieldWidth = 320;
        int y = 78;

        var welcomeLabel = new LabelControl
        {
            Text = "로그인",
            Location = new Point(formX, y),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(fieldWidth, 30)
        };
        welcomeLabel.Appearance.Font = AppFonts.Heading;
        Controls.Add(welcomeLabel);
        y += 40;

        AddFieldLabel("아이디", formX, ref y);
        txtUserId.Font = AppFonts.Body;
        txtUserId.Location = new Point(formX, y);
        txtUserId.Size = new Size(fieldWidth, 26);
        Controls.Add(txtUserId);
        y += 40;

        AddFieldLabel("비밀번호", formX, ref y);
        txtPassword.Font = AppFonts.Body;
        txtPassword.Location = new Point(formX, y);
        txtPassword.Size = new Size(fieldWidth, 26);
        Controls.Add(txtPassword);
        y += 40;

        AddFieldLabel("접속 서비스", formX, ref y);
        RefreshEnvironmentItems();
        cboEnvironment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboEnvironment.Font = AppFonts.Body;
        cboEnvironment.Location = new Point(formX, y);
        cboEnvironment.Size = new Size(fieldWidth - 84, 26);
        cboEnvironment.SelectedIndexChanged += (s, e) =>
        {
            if (cboEnvironment.SelectedItem == null) return;
            var key = GetEnvKey((string)cboEnvironment.SelectedItem);
            AppConfig.SwitchEnvironment(key);
        };
        Controls.Add(cboEnvironment);

        // 개발자가 이 PC에서 접속할 서비스를 직접 추가/삭제하는 화면으로 - appsettings.json에
        // 내장 안 된 고객사 서버를 재배포 없이 늘려갈 때 씀(AppConfig.AddSite 참고).
        lnkManageSites.Location = new Point(formX + fieldWidth - 78, y + 5);
        lnkManageSites.AutoSizeMode = LabelAutoSizeMode.None;
        lnkManageSites.Size = new Size(78, 16);
        lnkManageSites.Appearance.ForeColor = BrandColor;
        lnkManageSites.Appearance.Font = AppFonts.Caption;
        lnkManageSites.Cursor = Cursors.Hand;
        lnkManageSites.Click += (s, e) =>
        {
            using var siteManager = new SiteManagerForm();
            siteManager.ShowDialog(this);
            RefreshEnvironmentItems();
        };
        Controls.Add(lnkManageSites);
        y += 46;

        btnLogin.Location = new Point(formX, y);
        btnLogin.Size = new Size(fieldWidth, 38);
        btnLogin.Appearance.BackColor = BrandColor;
        btnLogin.Appearance.ForeColor = Color.White;
        btnLogin.Appearance.Font = AppFonts.BodyBold;
        btnLogin.Appearance.Options.UseBackColor = true;
        btnLogin.Appearance.Options.UseForeColor = true;
        btnLogin.Click += BtnLogin_Click;
        Controls.Add(btnLogin);

        AcceptButton = btnLogin;
    }

    private void AddFieldLabel(string text, int x, ref int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 16) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        Controls.Add(lbl);
        y += 20;
    }

    private string GetEnvLabel(string env) => _envLabels.TryGetValue(env, out var label) ? label : env;
    private string GetEnvKey(string label) => _envLabels.FirstOrDefault(kv => kv.Value == label).Key ?? label;

    /// <summary>서비스 관리 화면에서 추가/삭제하고 돌아왔을 때 드롭다운 목록을 다시 채운다 -
    /// 지금 선택되어 있던 서비스가 삭제됐으면 AppConfig.CurrentEnvironment(기본값)로 되돌린다.</summary>
    private void RefreshEnvironmentItems()
    {
        var previouslySelectedKey = cboEnvironment.SelectedItem != null
            ? GetEnvKey((string)cboEnvironment.SelectedItem)
            : AppConfig.CurrentEnvironment;

        cboEnvironment.Properties.Items.Clear();
        cboEnvironment.Properties.Items.AddRange(AppConfig.AvailableEnvironments.Select(GetEnvLabel).ToArray());

        var selectedKey = AppConfig.AvailableEnvironments.Contains(previouslySelectedKey)
            ? previouslySelectedKey
            : AppConfig.CurrentEnvironment;
        cboEnvironment.SelectedItem = GetEnvLabel(selectedKey);
    }

    /// <summary>타이틀바가 없어서 창을 못 옮기므로, 지정한 컨트롤을 드래그하면 창이 같이 움직이도록 처리</summary>
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

    private async void BtnLogin_Click(object? sender, EventArgs e)
    {
        var request = new LoginRequest
        {
            UserId = txtUserId.Text.Trim(),
            Password = txtPassword.Text,
            ClientVersion = Application.ProductVersion
        };

        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Password))
        {
            AppMessageBox.Show("아이디와 비밀번호를 입력해주세요.", "확인");
            return;
        }

        btnLogin.Enabled = false;
        try
        {
            LoginResponse? response;
            try
            {
                response = await ApiClient.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
            }
            catch (HttpRequestException ex)
            {
                AppMessageBox.Show($"서버에 연결할 수 없습니다. 네트워크 상태를 확인해주세요.\n{ex.Message}", "연결 오류");
                return;
            }

            if (response == null || !response.Success)
            {
                AppMessageBox.Show(response?.Message ?? "로그인에 실패했습니다.", "로그인 실패");
                return;
            }

            SessionManager.Current.SignIn(response);
            ApiClient.SetAuthToken(response.AccessToken!);

            // 로그인창을 닫기 직전에 스플래시를 띄워서, 닫히는 순간 바로 스플래시가 이어받게 한다.
            Splash = new SplashForm();
            Splash.Show();
            Splash.Refresh();

            DialogResult = DialogResult.OK;
            Close();
        }
        finally
        {
            btnLogin.Enabled = true;
        }
    }
}
