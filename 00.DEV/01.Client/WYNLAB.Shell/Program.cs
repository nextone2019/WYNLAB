using System.Runtime.CompilerServices;
using System.Threading;
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
    /// (System.Threading.Mutex/MessageBox는 BCL이라 이 제약과 무관 - WYNLAB 어셈블리가 아니다.)
    ///
    /// 예전엔 Run()의 내용이 전부 이 Main() 안에 들어있었는데, 그래서 EnsureUpToDate()가
    /// 호출되기도 전에 (Main을 JIT하는 시점에) WYNLAB.BaseForm.dll이 이미 메모리에 매핑됐고,
    /// 매핑된 파일은 File.Delete가 UnauthorizedAccessException으로 실패한다(사용 중을 뜻하는
    /// IOException이 아니라서 CoreAssemblyUpdater가 그런 경우를 rename으로 우회하는 것과는
    /// 별개 문제). 결과적으로 BaseForm.dll이 며칠 동안 한 번도 갱신되지 못했다(2026-08-24~25
    /// 실제로 겪음). 그래서 실제 기동 로직을 별도 메서드로 분리하고, JIT이 그걸 Main으로
    /// 인라인해서 같은 문제를 되살리지 못하도록 NoInlining을 명시한다.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // "두 인스턴스가 동시에 CoreAssembly 갱신 로직을 타면서 서로의 dll을 잠그는" 문제만
        // 직렬화한다(2026-09-06 발견). 예전엔 이 Mutex를 프로세스 종료까지 계속 들고 있어서
        // WYNLAB.exe 자체가 중복 실행이 아예 안 됐는데, 업무 특성상 여러 개를 동시에 띄워놓고
        // 쓰는 게 필수라는 요청에 따라(2026-09-10) 업데이트가 끝나는 즉시 놓도록 바꿨다 -
        // using 블록을 EnsureUpToDate() 호출만 감싸게 좁혀서, 그 이후(Run())부터는 다른
        // 인스턴스가 몇 개든 자유롭게 실행될 수 있다. 이름에 "Global\"을 안 붙였으므로 같은
        // 로그인 세션 안에서만 유효 - 이 앱은 단일 사용자 데스크톱용이라 그걸로 충분하고,
        // "Global\"은 별도 권한이 필요해질 수 있어 피한다.
        using (var updateMutex = new Mutex(initiallyOwned: true, "WYNLAB_CoreAssemblyUpdate", out var acquiredImmediately))
        {
            // 못 얻었어도(다른 인스턴스가 지금 막 갱신 중) 업데이트 자체를 건너뛰지 않는다 -
            // 잠깐(최대 30초) 순서만 양보하고, 그래도 안 풀리면 그냥 진행한다. 드물게 경합이
            // 나더라도 CoreAssemblyUpdater가 이미 잠긴 파일을 조용히 건너뛰도록 되어 있어
            // (2026-09-06 안정성 수정 참고) 앱이 죽지는 않는다.
            if (!acquiredImmediately) updateMutex.WaitOne(TimeSpan.FromSeconds(30));
            WYNLAB.Bootstrap.CoreAssemblyUpdater.EnsureUpToDate();
        }

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

        // DevExpress 컨트롤 전체의 기본 글꼴. 이걸 안 잡으면 DevExpress 자체 기본값인
        // Tahoma 9가 쓰여서, AppFonts(맑은 고딕)를 명시한 곳(메뉴트리/라벨 등)과 그렇지 않은
        // 곳(입력칸/콤보/그리드 등)의 글꼴이 서로 달라 보인다(실제로 겪음). 컨트롤마다 폰트를
        // 지정하는 대신 여기서 한 번에 바꾼다 - 새로 만드는 컨트롤도 자동으로 따라오고,
        // 우리가 감싸지 않은 DevExpress 컨트롤을 그대로 쓸 때도 통일된다.
        //
        // 반드시 어떤 컨트롤/폼도 만들어지기 전에 설정해야 한다 - 이미 만들어진 컨트롤은
        // 생성 시점의 글꼴을 들고 있어서 나중에 바꿔도 반영되지 않는다.
        DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = AppFonts.Body;
        DevExpress.XtraEditors.WindowsFormsSettings.DefaultMenuFont = AppFonts.Body;

        // .NET Framework용 WinForms 초기화 (net6+의 ApplicationConfiguration.Initialize() 대체)
        Application.EnableVisualStyles();
        WYNLAB.Base.Controls.ImeGuard.InstallGlobalFilter();   // 전각 키 입력을 반각으로 - 로그인/팝업 포함 앱 전체(ImeGuard 참고)
        Application.SetCompatibleTextRenderingDefault(false);

        // 전역 예외 안전망. `Load += async (s, e) => await QueryClick();`처럼 이벤트에 async
        // 람다를 직접 붙이는 게 이 코드베이스 전체의 표준 패턴인데(화면을 열자마자 자동조회),
        // 그 안에서 예외가 나면 어디서도 안 잡혀서 프로세스가 그대로 죽고 JIT 디버그 창이
        // 뜬다(실제로 겪음 - 서버가 오류를 냈을 뿐인데 앱 전체가 크래시됐다). 툴바 버튼
        // 클릭(ShellForm.AddIconBadgeButton)은 자체적으로 try/catch가 있어서 안전하지만, 폼이
        // 열리는 순간의 자동조회처럼 그 바깥에서 도는 코드는 이 보호막이 없다.
        //
        // Application.ThreadException이 WinForms 메시지 루프에서 도는 이벤트 핸들러의 처리되지
        // 않은 예외를 가로채는 표준 지점이다 - 이 한 줄로 지금 화면 5개와 앞으로 생길 모든
        // 화면이 같은 패턴을 써도 한꺼번에 보호된다. CatchException 모드를 명시해야 이 훅이
        // 실제로 동작한다(기본값은 환경에 따라 갈릴 수 있어 명시적으로 고정).
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) =>
            AppMessageBox.Show($"예상치 못한 오류가 발생했습니다.\n{e.Exception.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        // 화면별로 쪼개진 DLL(WYNLAB.SM.*)이 있는 폴더 위치를 기억해둔다 - 실제 로드는 여기서
        // 미리 다 하지 않고, ShellForm.OpenMenuForm이 메뉴를 처음 열 때마다(EnsureLoaded)
        // 그때그때 한다. 배포 직후에도 재로그인 없이 최신 화면을 받게 하려는 목적 - 자세한
        // 이유는 ModuleLoader 클래스 설명 참고.
        ModuleLoader.Initialize(AppConfig.ModulesPath);

        // LookUpEditWyn.MajorCd 같은 WYNLAB.Controls의 데이터 조회 훅에 실제 구현(ApiClient)을
        // 연결 - 화면 모듈이 로드되어 컨트롤이 생성되기 전에 반드시 먼저 해둬야 한다.
        ControlDataSources.Initialize();
        // PopupLookupEditWyn("..." 버튼)의 팝업 훅 연결 - popPopUp이 WYNLAB.Popup
        // 프로젝트에 있어서 ControlDataSources.Initialize()(BaseForm 소속)가 대신 못 한다
        // (순환참조 방지, WYNLAB.Popup/PopupWiring.cs 설명 참고).
        WYNLAB.Popup.PopupWiring.Initialize();

        // 로그인 성공 시에만 ShellForm(MDI 메인) 기동. 로그인창이 닫히면서 띄운 스플래시는
        // ShellForm이 실제로 화면에 그려진 직후(Shown)에 닫아서, 그 사이 빈 화면이 보이지 않게 한다.
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            // 로그인 화면 자체는 이 이미지들을 안 쓰므로, 느리거나 접속 안 되는 서버 때문에
            // 로그인 창이 늦게 뜨는 일이 없도록 로그인 성공 후(ShellForm의 툴바/로고를
            // 그리기 직전)에 동기화한다.
            AssetSyncer.SyncFromServer();

            // 사이트환경설정(frmSiteConfig) 색상값도 같은 자리에서 - ShellForm/화면들이 UiTheme를
            // 읽어 그리기 시작하기 전에 반드시 끝나 있어야 한다(SiteThemeSync 설명 참고).
            SiteThemeSync.ApplyFromServer();

            var shell = new ShellForm();
            shell.Shown += (s, e) => loginForm.Splash?.Close();
            Application.Run(shell);
        }
    }
}
