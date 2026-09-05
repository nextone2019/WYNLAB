using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// 강제 비밀번호 변경 다이얼로그 - 로그인 응답의 RequirePasswordChange=true일 때(만료/관리자
/// 초기화 등) LoginForm이 ShellForm을 열기 전에 반드시 먼저 띄운다. 취소하면 로그인 자체를
/// 취소한 것으로 처리한다(비밀번호를 안 바꾸고 앱에 들어갈 방법은 없음 - 서버의
/// MustChangePasswordFilter가 어차피 다른 API를 다 막는다).
/// </summary>
public partial class ChangePasswordForm : XtraForm
{
    private readonly TextEdit txtCurrent = new() { Properties = { PasswordChar = '*' } };
    private readonly TextEdit txtNew = new() { Properties = { PasswordChar = '*' } };
    private readonly TextEdit txtConfirm = new() { Properties = { PasswordChar = '*' } };
    private readonly SimpleButton btnSave = new() { Text = "변경" };
    private readonly SimpleButton btnCancel = new() { Text = "취소" };

    /// <summary>변경 성공(DialogResult.OK) 후 LoginForm이 이 값으로 다시 로그인한다.</summary>
    public string NewPassword => txtNew.Text;

    public ChangePasswordForm(string currentPasswordPrefill)
    {
        Text = "비밀번호 변경";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(360, 240);

        txtCurrent.Text = currentPasswordPrefill; // 방금 로그인 화면에서 입력한 값 그대로 - 다시 안 치게

        BuildLayout();
    }

    private void BuildLayout()
    {
        const int x = 24;
        const int fieldWidth = 312;
        var y = 20;

        var noticeLabel = new LabelControl
        {
            Text = "비밀번호를 변경해야 계속 진행할 수 있습니다.",
            Location = new Point(x, y),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(fieldWidth, 34)
        };
        noticeLabel.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        noticeLabel.Appearance.Font = AppFonts.Caption;
        Controls.Add(noticeLabel);
        y += 40;

        AddFieldLabel("현재 비밀번호", x, ref y);
        txtCurrent.Font = AppFonts.Body;
        txtCurrent.Location = new Point(x, y);
        txtCurrent.Size = new Size(fieldWidth, 26);
        Controls.Add(txtCurrent);
        y += 38;

        AddFieldLabel("새 비밀번호", x, ref y);
        txtNew.Font = AppFonts.Body;
        txtNew.Location = new Point(x, y);
        txtNew.Size = new Size(fieldWidth, 26);
        Controls.Add(txtNew);
        y += 38;

        AddFieldLabel("새 비밀번호 확인", x, ref y);
        txtConfirm.Font = AppFonts.Body;
        txtConfirm.Location = new Point(x, y);
        txtConfirm.Size = new Size(fieldWidth, 26);
        Controls.Add(txtConfirm);
        y += 42;

        btnCancel.Size = new Size(90, 32);
        btnCancel.Location = new Point(x + fieldWidth - btnCancel.Width, y);
        btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Controls.Add(btnCancel);

        btnSave.Size = new Size(90, 32);
        btnSave.Location = new Point(btnCancel.Left - btnSave.Width - 8, y);
        btnSave.Click += BtnSave_Click;
        Controls.Add(btnSave);

        AcceptButton = btnSave;
        CancelButton = btnCancel;
    }

    private void AddFieldLabel(string text, int x, ref int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 16) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        Controls.Add(lbl);
        y += 20;
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNew.Text) || txtNew.Text != txtConfirm.Text)
        {
            AppMessageBox.Show("새 비밀번호가 비어있거나 확인값과 다릅니다.", "확인");
            return;
        }

        btnSave.Enabled = false;
        try
        {
            var result = await ApiClient.PostAsync<ChangePasswordRequest, ApiResult>("api/auth/change-password",
                new ChangePasswordRequest { CurrentPassword = txtCurrent.Text, NewPassword = txtNew.Text });

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "비밀번호 변경에 실패했습니다.", "변경 실패");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }
}
