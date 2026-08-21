using System.Windows.Forms;

namespace WYNLAB.Shell;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // .NET Framework용 WinForms 초기화 (net6+의 ApplicationConfiguration.Initialize() 대체)
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // 로그인 성공 시에만 ShellForm(MDI 메인) 기동
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            Application.Run(new ShellForm());
        }
    }
}
