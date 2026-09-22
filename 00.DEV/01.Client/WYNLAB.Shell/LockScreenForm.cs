using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Shell;

/// <summary>
/// 자리비움 잠금화면 - ShellForm의 유휴감지 타이머(GetLastInputInfo 기반)가 설정된 시간
/// (AppConfig.IdleTimeoutMinutes, frmSiteConfig에서 관리자가 지정)을 넘기면 이 폼을 모달로
/// 띄운다(2026-09-09 요청 - "잠금화면이 뜨도록... 비번을 치면 다시 화면이 열리도록").
///
/// 로그인화면과 똑같이 api/auth/login을 그대로 재사용한다 - 아이디는 이미 알고 있으니(현재
/// 로그인된 사용자) 비밀번호만 다시 확인하면 되고, 성공하면 그 응답이 새 액세스 토큰까지
/// 같이 발급해주므로 잠금해제가 세션 갱신을 겸한다(SessionRefreshHandler의 "조용한 갱신"과는
/// 별개 경로 - 이쪽은 사용자가 직접 비번을 입력하는 화면이 이미 있으므로 그 자체가 UX).
///
/// WYNLAB(ShellForm) 창 크기만큼만 덮는 테두리 없는 창 - TopMost는 쓰지 않고 ShowDialog(owner:
/// ShellForm)의 Owner 관계만으로 항상 WYNLAB 위에 뜨게 한다(2026-09-09 - "wynlab만 막고
/// PC사용은 할수 있어야 해": 처음엔 TopMost+전체 화면으로 만들어서 다른 프로그램까지 다 가려버리는
/// 부작용이 있었음, Owner-모달 방식으로 바꿔 WYNLAB 창만 잠그도록 수정). 잠긴 동안 뒤에 있는
/// 업무화면/MDI 상태는 전혀 건드리지 않는다(닫거나 초기화하지 않음).
///
/// "다른 사용자로 로그인" 버튼은 DialogResult.Abort로 닫기만 한다 - 실제 로그아웃(전체 MDI
/// 자식 닫기 + LoginForm 재표시)은 ShellForm.SignOutAndShowLogin()이 하는데, 그걸 이 폼이
/// 열려있는 도중(ShowDialog가 아직 안 끝난 상태)에 부르면 모달이 중첩되어 꼬인다 - 그래서
/// 호출측(ShellForm)이 ShowDialog() 반환값을 보고 닫힌 "다음"에 처리한다.
/// </summary>
public class LockScreenForm : XtraForm
{
    private readonly Color _accentColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

    private readonly LabelControl lblTitle = new();
    private readonly LabelControl lblUserNm = new();
    private readonly LabelControl lblError = new() { Visible = false };
    private readonly TextEdit txtPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly SimpleButton btnUnlock = new() { Text = "잠금 해제" };
    private readonly SimpleButton btnLogout = new() { Text = "다른 사용자로 로그인" };
    private bool _busy;

    public LockScreenForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        var bounds = Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1920, 1080);
        Bounds = bounds;
        BackColor = ColorHelper.Mix(_accentColor, Color.Black, 0.55f);
        KeyPreview = true;

        var card = new PanelControl
        {
            Size = new Size(360, 260),
            Appearance = { BackColor = Color.White, Options = { UseBackColor = true } },
        };
        card.Location = new Point((bounds.Width - card.Width) / 2, (bounds.Height - card.Height) / 2);

        lblTitle.Text = "화면이 잠겼습니다";
        lblTitle.Font = AppFonts.Heading;
        lblTitle.Location = new Point(24, 24);
        lblTitle.AutoSize = true;

        lblUserNm.Font = AppFonts.Body;
        lblUserNm.ForeColor = Color.FromArgb(100, 103, 110);
        lblUserNm.Location = new Point(24, 58);
        lblUserNm.AutoSize = true;

        txtPassword.Font = AppFonts.Body;
        txtPassword.Location = new Point(24, 96);
        txtPassword.Size = new Size(312, 26);
        txtPassword.KeyDown += async (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            await UnlockAsync();
        };

        lblError.Font = AppFonts.Caption;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(24, 128);
        lblError.AutoSize = true;

        btnUnlock.Size = new Size(312, 32);
        btnUnlock.Location = new Point(24, 156);
        btnUnlock.Appearance.BackColor = _accentColor;
        btnUnlock.Appearance.ForeColor = Color.White;
        btnUnlock.Appearance.Options.UseBackColor = true;
        btnUnlock.Appearance.Options.UseForeColor = true;
        btnUnlock.Click += async (s, e) => await UnlockAsync();

        btnLogout.Size = new Size(312, 28);
        btnLogout.Location = new Point(24, 196);
        btnLogout.Click += (s, e) =>
        {
            DialogResult = DialogResult.Abort;
            Close();
        };

        card.Controls.Add(lblTitle);
        card.Controls.Add(lblUserNm);
        card.Controls.Add(txtPassword);
        card.Controls.Add(lblError);
        card.Controls.Add(btnUnlock);
        card.Controls.Add(btnLogout);
        Controls.Add(card);

        Load += (s, e) =>
        {
            // TopMost 없이 Owner(ShellForm)로만 모달 관계를 맺으므로, Windows가 이 창을 항상
            // WYNLAB 위에만 띄워준다 - 잠금 범위를 WYNLAB 창 크기로 맞춰서 다른 프로그램(PC의
            // 나머지 화면)은 잠금과 무관하게 그대로 쓸 수 있게 한다(2026-09-09 요청 - "wynlab만
            // 막고 PC사용은 할수 있어야 해").
            if (Owner != null) Bounds = Owner.Bounds;
            card.Location = new Point((Bounds.Width - card.Width) / 2, (Bounds.Height - card.Height) / 2);

            lblUserNm.Text = $"{Session.UserNm}({Session.UserId})님, 비밀번호를 다시 입력해주세요.";
            txtPassword.Text = string.Empty;
            txtPassword.Focus();
        };
    }

    private async Task UnlockAsync()
    {
        if (_busy) return;
        var password = txtPassword.Text;
        if (string.IsNullOrEmpty(password))
        {
            ShowError("비밀번호를 입력하세요.");
            return;
        }

        _busy = true;
        btnUnlock.Enabled = false;
        try
        {
            var userId = Session.UserId ?? string.Empty;
            var response = await ApiClient.PostAsync<LoginRequest, LoginResponse>("api/auth/login", new LoginRequest
            {
                UserId = userId,
                Password = password,
                ClientVersion = Application.ProductVersion
            });

            if (response is not { Success: true, AccessToken.Length: > 0 })
            {
                ShowError(response?.Message ?? "잠금해제에 실패했습니다.");
                txtPassword.SelectAll();
                txtPassword.Focus();
                return;
            }

            // MUST_CHANGE_PWD_YN='Y'로 발급된 제한된 토큰은 change-password 외 모든 API가
            // 막혀있다(LoginResponse.RequirePasswordChange 주석 참고) - 잠금화면 안에서 비밀번호
            // 변경까지 흉내내는 대신, 정상 로그인 경로(로그아웃 후 재로그인)로 안내한다.
            if (response.RequirePasswordChange)
            {
                ShowError("비밀번호 변경이 필요합니다. \"다른 사용자로 로그인\"으로 로그아웃 후 다시 로그인해주세요.");
                return;
            }

            SessionManager.Current.SignIn(response);
            SessionManager.Current.RememberCredentials(userId, password);
            ApiClient.SetAuthToken(response.AccessToken!);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"잠금해제 중 오류가 발생했습니다.\n{ex.Message}");
        }
        finally
        {
            _busy = false;
            btnUnlock.Enabled = true;
        }
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }
}
