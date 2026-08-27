using DevExpress.XtraEditors;
using WYNLAB.Base;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 성공 -> 메인 셸(ShellForm) 기동 사이를 이어주는 스플래시. 로그인창이 닫히자마자
/// 뜨고, ShellForm이 실제로 화면에 표시된 직후(Shown 이벤트)에 Program.cs에서 닫아준다.
/// LoginForm과 생명주기가 독립적인 별개의 Form이라 - LoginForm은 ShowDialog()로 뜬 모달이라
/// 도중에 Hide()하면 그 시점에 ShowDialog()가 바로 반환돼버려서(WinForms 동작), 이 스플래시를
/// LoginForm 내부가 아니라 별도 Form으로 분리해야 그 문제를 피할 수 있다.
/// </summary>
public class SplashForm : XtraForm
{
    public SplashForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        ShowInTaskbar = false;
        Size = new Size(280, 200);
        BackColor = Color.White;

        var accent = ColorHelper.FromHex(AppConfig.ToolbarColor);

        var badge = new Panel
        {
            BackColor = BackColor,
            Size = new Size(40, 40),
            Location = new Point((Width - 40) / 2, 34)
        };
        badge.Paint += (s, e) => LogoPainter.Draw(e.Graphics, badge.ClientRectangle, darkBackground: false);

        var spinner = new SpinnerControl
        {
            SpinnerColor = accent,
            Size = new Size(28, 28),
            Location = new Point((Width - 28) / 2, 96)
        };

        var lblText = new LabelControl
        {
            Text = "불러오는 중입니다...",
            Location = new Point(0, 140),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(Width, 20)
        };
        lblText.Appearance.ForeColor = Color.FromArgb(120, 120, 120);
        lblText.Appearance.Font = AppFonts.Caption;
        lblText.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

        Controls.Add(badge);
        Controls.Add(spinner);
        Controls.Add(lblText);
    }

    /// <summary>다이얼로그 전체 테두리를 얇게 그려서 배경과 경계를 분명하게 함(AppMessageBox와 같은 패턴)</summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(225, 226, 230));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}
