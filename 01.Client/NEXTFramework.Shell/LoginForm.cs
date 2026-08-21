using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using NEXTFramework.Shared.Dtos;
using NEXTFramework.UI.Common;
using System.Drawing;
using System.Net.Http;

namespace NEXTFramework.Shell;

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

    private readonly PanelControl leftPanel = new();
    private readonly TextEdit txtUserId = new();
    private readonly TextEdit txtPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly ComboBoxEdit cboEnvironment = new();
    private readonly SimpleButton btnLogin = new() { Text = "로그인" };
    private readonly LabelControl lblClose = new() { Text = "✕" };

    private readonly Dictionary<string, string> _envLabels = new()
    {
        ["Development"] = "Development",
        ["Production"] = "Live (Production)"
    };

    private Point _dragStart;
    private bool _dragging;

    public LoginForm()
    {
        Text = "NEXTFramework 로그인";
        FormBorderStyle = FormBorderStyle.None;
        ControlBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(860, 500);
        BackColor = Color.White;

        BuildLeftPanel();
        BuildCloseButton();
        BuildRightPanel();

        Controls.Add(leftPanel);
    }

    /// <summary>좌측 브랜드 영역 - 그라데이션 배경 + 로고 + 문구. 드래그로 창 이동 가능.</summary>
    private void BuildLeftPanel()
    {
        leftPanel.Dock = DockStyle.Left;
        leftPanel.Width = 360;
        leftPanel.Appearance.BackColor = BrandColor;
        leftPanel.Appearance.BackColor2 = ColorHelper.Adjust(BrandColor, -30);
        leftPanel.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
        leftPanel.Appearance.Options.UseBackColor = true;
        leftPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        var badge = new Panel { BackColor = Color.White, Size = new Size(56, 56), Location = new Point(48, 90) };
        var badgeLabel = new LabelControl { Text = "N", Dock = DockStyle.Fill, AutoSizeMode = LabelAutoSizeMode.None };
        badgeLabel.Appearance.ForeColor = BrandColor;
        badgeLabel.Appearance.Font = new Font("Segoe UI", 22, FontStyle.Bold);
        badgeLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        badgeLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        badge.Controls.Add(badgeLabel);

        var titleLabel = new LabelControl
        {
            Text = "NEXTFramework",
            Location = new Point(48, 168),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(280, 40)
        };
        titleLabel.Appearance.ForeColor = Color.White;
        titleLabel.Appearance.Font = new Font("Segoe UI", 20, FontStyle.Bold);

        var taglineLabel = new LabelControl
        {
            Text = "업무의 모든 흐름을\n하나의 프레임워크 안에서.",
            Location = new Point(48, 214),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(280, 60)
        };
        taglineLabel.Appearance.ForeColor = Color.FromArgb(230, 255, 255, 255);
        taglineLabel.Appearance.Font = new Font("Segoe UI", 11);

        var versionLabel = new LabelControl
        {
            Text = $"v{DateTime.Now:yyyy.MM.dd}",
            Location = new Point(48, 430),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(200, 20)
        };
        versionLabel.Appearance.ForeColor = Color.FromArgb(160, 255, 255, 255);
        versionLabel.Appearance.Font = new Font("Segoe UI", 8.5f);

        leftPanel.Controls.Add(badge);
        leftPanel.Controls.Add(titleLabel);
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
        lblClose.Appearance.Font = new Font("Segoe UI", 11);
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
        const int formX = 440;
        int y = 130;

        var welcomeLabel = new LabelControl
        {
            Text = "로그인",
            Location = new Point(formX, y),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(300, 34)
        };
        welcomeLabel.Appearance.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        Controls.Add(welcomeLabel);
        y += 56;

        AddFieldLabel("아이디", formX, ref y);
        txtUserId.Location = new Point(formX, y);
        txtUserId.Size = new Size(320, 28);
        Controls.Add(txtUserId);
        y += 44;

        AddFieldLabel("비밀번호", formX, ref y);
        txtPassword.Location = new Point(formX, y);
        txtPassword.Size = new Size(320, 28);
        Controls.Add(txtPassword);
        y += 44;

        AddFieldLabel("접속 서비스", formX, ref y);
        cboEnvironment.Properties.Items.AddRange(AppConfig.AvailableEnvironments.Select(GetEnvLabel).ToArray());
        cboEnvironment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboEnvironment.SelectedItem = GetEnvLabel(AppConfig.CurrentEnvironment);
        cboEnvironment.Location = new Point(formX, y);
        cboEnvironment.Size = new Size(320, 28);
        cboEnvironment.SelectedIndexChanged += (s, e) =>
        {
            var key = GetEnvKey((string)cboEnvironment.SelectedItem!);
            AppConfig.SwitchEnvironment(key);
        };
        Controls.Add(cboEnvironment);
        y += 56;

        btnLogin.Location = new Point(formX, y);
        btnLogin.Size = new Size(320, 40);
        btnLogin.Appearance.BackColor = BrandColor;
        btnLogin.Appearance.ForeColor = Color.White;
        btnLogin.Appearance.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btnLogin.Appearance.Options.UseBackColor = true;
        btnLogin.Appearance.Options.UseForeColor = true;
        btnLogin.Click += BtnLogin_Click;
        Controls.Add(btnLogin);

        AcceptButton = btnLogin;
    }

    private void AddFieldLabel(string text, int x, ref int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 18) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = new Font("Segoe UI", 8.5f);
        Controls.Add(lbl);
        y += 22;
    }

    private string GetEnvLabel(string env) => _envLabels.TryGetValue(env, out var label) ? label : env;
    private string GetEnvKey(string label) => _envLabels.FirstOrDefault(kv => kv.Value == label).Key ?? label;

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
            XtraMessageBox.Show("아이디와 비밀번호를 입력해주세요.", "확인");
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
                XtraMessageBox.Show($"서버에 연결할 수 없습니다. 네트워크 상태를 확인해주세요.\n{ex.Message}", "연결 오류");
                return;
            }

            if (response == null || !response.Success)
            {
                XtraMessageBox.Show(response?.Message ?? "로그인에 실패했습니다.", "로그인 실패");
                return;
            }

            SessionManager.Current.SignIn(response);
            ApiClient.SetAuthToken(response.AccessToken!);
            DialogResult = DialogResult.OK;
            Close();
        }
        finally
        {
            btnLogin.Enabled = true;
        }
    }
}
