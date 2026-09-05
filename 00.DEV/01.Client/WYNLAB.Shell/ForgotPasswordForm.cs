using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// 비밀번호 찾기 - 1단계(아이디 입력 -> 이메일로 인증코드 발송 요청)와 2단계(코드+새비밀번호
/// 확인)를 한 다이얼로그에서 처리한다. 성공하면 그 자리에서 로그인까지 끝나서(Result에
/// LoginResponse가 담김), LoginForm은 일반 로그인 성공과 같은 방식으로 ShellForm을 연다.
/// </summary>
public partial class ForgotPasswordForm : XtraForm
{
    private readonly TextEdit txtUserId = new();
    private readonly SimpleButton btnRequestCode = new() { Text = "인증코드 받기" };

    private readonly LabelControl lblCode = new() { Text = "인증코드" };
    private readonly TextEdit txtCode = new();
    private readonly LabelControl lblNewPassword = new() { Text = "새 비밀번호" };
    private readonly TextEdit txtNewPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly LabelControl lblConfirmPassword = new() { Text = "새 비밀번호 확인" };
    private readonly TextEdit txtConfirmPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly SimpleButton btnConfirm = new() { Text = "확인" };

    /// <summary>성공하면 여기 로그인된 응답이 담긴다 - LoginForm이 이 값으로 그대로 ShellForm을 연다.</summary>
    public LoginResponse? Result { get; private set; }

    private const int FormX = 24;
    private const int FieldWidth = 312;

    public ForgotPasswordForm()
    {
        Text = "비밀번호 찾기";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(360, 160);

        BuildStep1();
        ShowStep2(false);
    }

    private void BuildStep1()
    {
        var y = 20;

        AddFieldLabel("아이디", FormX, ref y);
        txtUserId.Font = AppFonts.Body;
        txtUserId.Location = new Point(FormX, y);
        txtUserId.Size = new Size(FieldWidth, 26);
        Controls.Add(txtUserId);
        y += 38;

        btnRequestCode.Location = new Point(FormX, y);
        btnRequestCode.Size = new Size(FieldWidth, 32);
        btnRequestCode.Click += BtnRequestCode_Click;
        Controls.Add(btnRequestCode);

        // 2단계 컨트롤은 인증코드 발송 성공 뒤에 같은 자리 아래로 이어붙인다.
        var y2 = y + 42;
        AddControlAt(lblCode, FormX, ref y2, isLabel: true);
        txtCode.Font = AppFonts.Body;
        txtCode.Location = new Point(FormX, y2);
        txtCode.Size = new Size(FieldWidth, 26);
        Controls.Add(txtCode);
        y2 += 38;

        AddControlAt(lblNewPassword, FormX, ref y2, isLabel: true);
        txtNewPassword.Font = AppFonts.Body;
        txtNewPassword.Location = new Point(FormX, y2);
        txtNewPassword.Size = new Size(FieldWidth, 26);
        Controls.Add(txtNewPassword);
        y2 += 38;

        AddControlAt(lblConfirmPassword, FormX, ref y2, isLabel: true);
        txtConfirmPassword.Font = AppFonts.Body;
        txtConfirmPassword.Location = new Point(FormX, y2);
        txtConfirmPassword.Size = new Size(FieldWidth, 26);
        Controls.Add(txtConfirmPassword);
        y2 += 42;

        btnConfirm.Location = new Point(FormX, y2);
        btnConfirm.Size = new Size(FieldWidth, 32);
        btnConfirm.Click += BtnConfirm_Click;
        Controls.Add(btnConfirm);
    }

    private void AddFieldLabel(string text, int x, ref int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 16) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        Controls.Add(lbl);
        y += 20;
    }

    private void AddControlAt(LabelControl lbl, int x, ref int y, bool isLabel)
    {
        lbl.Location = new Point(x, y);
        lbl.AutoSizeMode = LabelAutoSizeMode.None;
        lbl.Size = new Size(200, 16);
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        Controls.Add(lbl);
        y += 20;
    }

    /// <summary>1단계(아이디+코드받기)만 보이던 상태에서 2단계(코드+새비밀번호) 컨트롤을
    /// 보이거나 숨긴다 - 창 높이도 같이 늘어난다.</summary>
    private void ShowStep2(bool visible)
    {
        lblCode.Visible = visible;
        txtCode.Visible = visible;
        lblNewPassword.Visible = visible;
        txtNewPassword.Visible = visible;
        lblConfirmPassword.Visible = visible;
        txtConfirmPassword.Visible = visible;
        btnConfirm.Visible = visible;

        ClientSize = new Size(360, visible ? 380 : 160);
    }

    private async void BtnRequestCode_Click(object? sender, EventArgs e)
    {
        var userId = txtUserId.Text.Trim();
        if (string.IsNullOrWhiteSpace(userId))
        {
            AppMessageBox.Show("아이디를 입력해주세요.", "확인");
            return;
        }

        btnRequestCode.Enabled = false;
        try
        {
            var result = await ApiClient.PostAsync<PasswordResetRequestDto, ApiResult>("api/auth/reset-request",
                new PasswordResetRequestDto { UserId = userId });

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "요청이 실패했습니다. 아이디, 등록된 이메일정보 등을 확인 하십시오.", "안내");
                return; // 1단계에 그대로 남는다 - 아이디를 고쳐서 다시 시도할 수 있게
            }

            AppMessageBox.Show(result.Message ?? "정상적으로 요청이 접수되었습니다.", "안내");
            ShowStep2(true);
            txtCode.Focus();
        }
        finally
        {
            btnRequestCode.Enabled = true;
        }
    }

    private async void BtnConfirm_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCode.Text))
        {
            AppMessageBox.Show("인증코드를 입력해주세요.", "확인");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtNewPassword.Text) || txtNewPassword.Text != txtConfirmPassword.Text)
        {
            AppMessageBox.Show("새 비밀번호가 비어있거나 확인값과 다릅니다.", "확인");
            return;
        }

        btnConfirm.Enabled = false;
        try
        {
            var response = await ApiClient.PostAsync<PasswordResetConfirmDto, LoginResponse>("api/auth/reset-confirm",
                new PasswordResetConfirmDto { UserId = txtUserId.Text.Trim(), Code = txtCode.Text.Trim(), NewPassword = txtNewPassword.Text });

            if (response == null || !response.Success)
            {
                AppMessageBox.Show(response?.Message ?? "인증코드가 올바르지 않거나 만료되었습니다.", "확인 실패");
                return;
            }

            Result = response;
            DialogResult = DialogResult.OK;
            Close();
        }
        finally
        {
            btnConfirm.Enabled = true;
        }
    }
}
