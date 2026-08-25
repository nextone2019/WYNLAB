using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WYNLAB.Base;

namespace WYNLAB.Shell;

internal static class Program
{
    /// <summary>
    /// 여기엔 CoreAssemblyUpdater 호출 외에 아무것도 두면 안 된다. WYNLAB.BaseForm/Shared/Controls의
    /// 타입을 "실행"하지 않는 것만으로는 부족하고, 이 메서드 본문에 "언급"조차 하면 안 된다 -
    /// .NET은 메서드의 첫 줄을 실행하기 전에 그 메서드 전체를 JIT 컴파일하면서 본문에 등장하는
    /// 모든 타입을 해석하고, 그 과정에서 해당 어셈블리를 프로세스에 로드해버리기 때문이다.
    ///
    /// 예전엔 Run()의 내용이 전부 이 Main() 안에 들어있었는데, 그래서 EnsureUpToDate()가
    /// 호출되기도 전에 (Main을 JIT하는 시점에) WYNLAB.BaseForm.dll이 이미 메모리에 매핑됐고,
    /// 매핑된 파일은 File.Delete가 UnauthorizedAccessException으로 실패한다(사용 중을 뜻하는
    /// IOException이 아니라서 CoreAssemblyUpdater의 IsFileLocked 사전 체크로도 안 걸러졌다).
    /// 결과적으로 BaseForm.dll이 며칠 동안 한 번도 갱신되지 못했다(2026-08-24~25 실제로 겪음).
    /// 그래서 실제 기동 로직을 별도 메서드로 분리하고, JIT이 그걸 Main으로 인라인해서 같은
    /// 문제를 되살리지 못하도록 NoInlining을 명시한다.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        WYNLAB.Bootstrap.CoreAssemblyUpdater.EnsureUpToDate();
        Run();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Run()
    {
        // DevExpress 스킨을 앱 전체(메시지박스, 버튼, 탭, 폼 캡션 등)에 적용.
        // EnableFormSkins()가 없으면 내부 컨트롤만 스킨되고 폼 자체 타이틀바는 계속
        // 기본 Windows 스타일로 남아서, 지금 겪은 "메시지박스 타이틀/본문이 안 나뉘어 보임"
        // 문제가 그대로 남는다. 반드시 Application.EnableVisualStyles()보다 먼저 호출.
        DevExpress.UserSkins.BonusSkins.Register();
        DevExpress.Skins.SkinManager.EnableFormSkins();
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(ShellForm.DefaultSkin);

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
            // 로그인 화면 자체는 이 이미지들을 안 쓰므로, 느리거나 접속 안 되는 서버 때문에
            // 로그인 창이 늦게 뜨는 일이 없도록 로그인 성공 후(ShellForm의 툴바/로고를
            // 그리기 직전)에 동기화한다.
            AssetSyncer.SyncFromServer();

            var shell = new ShellForm();
            shell.Shown += (s, e) => loginForm.Splash?.Close();
            Application.Run(shell);
        }
    }
}
