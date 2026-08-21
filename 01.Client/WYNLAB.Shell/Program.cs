using System.Windows.Forms;
using WYNLAB.Base;

namespace WYNLAB.Shell;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // DevExpress 스킨을 앱 전체(메시지박스, 버튼, 탭, 폼 캡션 등)에 적용.
        // EnableFormSkins()가 없으면 내부 컨트롤만 스킨되고 폼 자체 타이틀바는 계속
        // 기본 Windows 스타일로 남아서, 지금 겪은 "메시지박스 타이틀/본문이 안 나뉘어 보임"
        // 문제가 그대로 남는다. 반드시 Application.EnableVisualStyles()보다 먼저 호출.
        DevExpress.UserSkins.BonusSkins.Register();
        DevExpress.Skins.SkinManager.EnableFormSkins();
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");

        // .NET Framework용 WinForms 초기화 (net6+의 ApplicationConfiguration.Initialize() 대체)
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // 화면별로 쪼개진 DLL(WYNLAB.Screens.*)들을 미리 로드해둔다 - Shell은 이 프로젝트들을
        // 더 이상 컴파일 타임에 참조하지 않으므로, ShellForm.OpenMenuForm의 Type.GetType이
        // 찾을 수 있으려면 이 시점에 먼저 AppDomain에 올려둬야 한다.
        ModuleLoader.LoadAll(AppConfig.ModulesPath);

        // 로그인 성공 시에만 ShellForm(MDI 메인) 기동. 로그인창이 닫히면서 띄운 스플래시는
        // ShellForm이 실제로 화면에 그려진 직후(Shown)에 닫아서, 그 사이 빈 화면이 보이지 않게 한다.
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            var shell = new ShellForm();
            shell.Shown += (s, e) => loginForm.Splash?.Close();
            Application.Run(shell);
        }
    }
}
