using System.Windows.Forms;
using WYNLAB.Base;

namespace WYNLAB.Shell;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // 반드시 가장 먼저 호출 - WYNLAB.Base/WYNLAB.Shared 타입(AppConfig 등)을 이 시점까지
        // 단 하나도 건드리면 안 된다. 이미 프로세스에 로드된 dll은 파일째로 교체할 수 없어서,
        // "로드되기 전에 최신인지 확인하고 필요하면 교체"하는 순서 자체가 이 기능의 핵심이다.
        // 자세한 이유는 WYNLAB.Bootstrap.csproj 주석 참고.
        WYNLAB.Bootstrap.CoreAssemblyUpdater.EnsureUpToDate();

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

        // 화면별로 쪼개진 DLL(WYNLAB.SM.*)이 있는 폴더 위치를 기억해둔다 - 실제 로드는 여기서
        // 미리 다 하지 않고, ShellForm.OpenMenuForm이 메뉴를 처음 열 때마다(EnsureLoaded)
        // 그때그때 한다. 배포 직후에도 재로그인 없이 최신 화면을 받게 하려는 목적 - 자세한
        // 이유는 ModuleLoader 클래스 설명 참고.
        ModuleLoader.Initialize(AppConfig.ModulesPath);

        // LookUpEditWyn.MajorCd 같은 WYNLAB.Controls의 데이터 조회 훅에 실제 구현(ApiClient)을
        // 연결 - 화면 모듈이 로드되어 컨트롤이 생성되기 전에 반드시 먼저 해둬야 한다.
        ControlDataSources.Initialize();

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
