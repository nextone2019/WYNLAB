using System.ComponentModel;
using System.Text.Json;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 클라이언트 배포판(appsettings.json)에서 설정을 읽어온다.
/// DB 연결정보는 이 파일에 절대 포함하지 않는다 - DB 접근은 전부 WYNLAB.Api가 담당.
///
/// v2: 개발/운영 서버 정보를 둘 다 파일 안에 갖고 있어서, 로그인된 상태에서도
/// 툴바에서 실시간으로 서버를 전환할 수 있다 (SwitchEnvironment 참고).
/// DefaultEnvironment는 프로그램을 처음 켰을 때 어느 서버로 시작할지만 결정한다.
/// </summary>
public static class AppConfig
{
    private static readonly Lazy<ClientConfig> _config = new(Load);
    private static readonly Lazy<List<UserSiteEntry>> _userSites = new(LoadUserSites);
    private static string? _currentEnvironment;

    private static readonly string UserSitesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "sites.json");

    /// <summary>마지막으로 로그인 화면에서 선택했던 서비스 이름 하나만 저장하는 파일 - sites.json과
    /// 형식을 섞지 않으려고 따로 둔다(2026-09-14, "다음 로그인 때 마지막 접속 서비스가 선택돼
    /// 있어야 한다"는 요청으로 추가). 이 값 자체는 CurrentEnvironment 하나뿐이라 JSON으로 감쌀
    /// 필요 없이 이름만 그대로 텍스트로 저장한다.</summary>
    private static readonly string LastEnvironmentPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "last-environment.txt");

    public static string CurrentEnvironment => _currentEnvironment ??= LoadLastUsedEnvironmentOrDefault();

    /// <summary>appsettings.json에 내장된 기본 서비스(일반 사용자용 Prod/Test 등) + 사용자가
    /// 이 PC에서 직접 추가한 서비스(개발자가 여러 고객사 서버를 오갈 때 씀, AddSite 참고)를
    /// 합쳐서 보여준다. 후자는 재배포 없이 실행 중에 자유롭게 추가/삭제할 수 있다.
    ///
    /// 내장 서비스를 OverrideBuiltin으로 직접 수정한 경우, 그 원래 이름은 목록에서 빠진다(자기
    /// 자신을 오버라이드한 사용자 서비스가 그 자리를 대신하므로 - 2026-09-13, "개발자 혼자 쓰는
    /// PC라 회사 공통이라는 구분이 의미 없다, Development/Production도 자유롭게 고쳐 쓸 수
    /// 있어야 한다"는 요청으로 도입). 이름을 바꿔서 오버라이드했다면(예: Production -> A사)
    /// 원래 이름은 완전히 안 보이고 새 이름만 보인다 - 그 오버라이드 항목을 삭제하면
    /// OverridesBuiltin 덕분에 원래 이름이 자동으로 다시 나타난다(RemoveSite 참고).</summary>
    public static IReadOnlyList<string> AvailableEnvironments
    {
        get
        {
            var overridden = _userSites.Value
                .Where(s => s.OverridesBuiltin != null)
                .Select(s => s.OverridesBuiltin!)
                .ToHashSet();

            return _config.Value.Environments.Keys
                .Where(k => !overridden.Contains(k))
                .Concat(_userSites.Value.Select(s => s.Name))
                .ToList();
        }
    }

    /// <summary>사용자가 이 PC에서 직접 추가한 서비스 목록(appsettings.json 내장분 제외) -
    /// 서비스 관리 화면에서 목록 표시/삭제용으로 쓴다.</summary>
    public static IReadOnlyList<UserSiteEntry> UserSites => _userSites.Value;

    /// <summary>내장 서비스(appsettings.json)가 아니라 사용자가 추가한 서비스인지 여부 -
    /// 내장 서비스는 삭제 못 하게 막을 때 이 값으로 구분한다.</summary>
    public static bool IsUserSite(string name) => _userSites.Value.Any(s => s.Name == name);

    public static string ApiBaseUrl => ResolveCurrent().ApiBaseUrl;

    /// <summary>
    /// 화면별로 쪼개진 DLL(WYNLAB.Screens.*)들이 놓여있는 폴더 - 시작 시 ModuleLoader가
    /// 이 폴더를 스캔해서 전부 로드한다. 환경별로 다른 경로를 가질 수 있다(운영은 보통
    /// 서버의 공유폴더/배포경로, 개발은 각 화면 프로젝트의 빌드 후 자동복사 대상 폴더).
    /// appsettings.json에 상대경로(예: "Modules")로 적으면 exe 폴더 기준으로 풀어준다 -
    /// 개발 PC마다 저장소 경로가 다를 수 있어서 절대경로를 하드코딩하지 않기 위함.
    /// http(s) 주소를 적으면 HTTP 배포 모드로 동작한다(ModuleLoader/HttpFileSync 설명 참고).
    /// </summary>
    public static string ModulesPath
    {
        get
        {
            var modulesPath = ResolveCurrent().ModulesPath;
            if (string.IsNullOrWhiteSpace(modulesPath)) return string.Empty;

            // URL은 경로 보정 대상이 아니다. Path.IsPathRooted("http://...")는 false를 돌려주기
            // 때문에, 이 검사가 없으면 URL이 exe 폴더 경로와 합쳐져 "C:\app\http://서버/Modules"
            // 같은 쓰레기 값이 된다.
            if (HttpFileSync.IsHttpUrl(modulesPath)) return modulesPath;

            return Path.IsPathRooted(modulesPath)
                ? modulesPath
                : Path.Combine(AppContext.BaseDirectory, modulesPath);
        }
    }

    /// <summary>
    /// WYNLAB.BaseForm.dll/WYNLAB.Shared.dll(프레임워크 자체) 배포 폴더 - Program.cs 맨 앞의
    /// CoreAssemblyUpdater.EnsureUpToDate()가 실제로 참조하는 값은 이 프로퍼티가 아니라
    /// appsettings.json을 직접 다시 읽은 것이다(그 시점엔 이 dll 자체가 아직 최신인지 모르는
    /// 상태라 AppConfig를 쓸 수 없음 - WYNLAB.Bootstrap 프로젝트의 주석 참고). 여기 있는 건
    /// 그 이후(이미 최신 상태 확정된 후) 화면에서 참고용으로 보여주고 싶을 때 쓰는 용도.
    /// </summary>
    public static string CoreAssemblyPath => ResolveCurrent().CoreAssemblyPath;

    /// <summary>툴바 아이콘/로고/배경 이미지가 있는 서버 공유폴더 - AssetSyncer가 앱 시작 시
    /// 이 폴더를 읽어서 %LocalAppData%\WYNLAB\Assets\(IconAssetProvider.AssetsFolder)로
    /// 복사해둔다. 관리자는 여기(서버) 한 곳에만 이미지를 올려두면 되고, 각 PC에 일일이
    /// 파일을 옮길 필요가 없다.</summary>
    public static string AssetsPath => ResolveCurrent().AssetsPath;

    public static string ToolbarColor => _config.Value.ToolbarColor;

    public static UiThemeConfig Theme => _config.Value.Theme;

    public static bool IsDevelopment => CurrentEnvironment.Equals("Development", StringComparison.OrdinalIgnoreCase);

    /// <summary>서버 전환시 발생 - ShellForm이 이 이벤트를 구독해서 UI를 갱신할 수 있음</summary>
    public static event Action? EnvironmentChanged;

    /// <summary>서비스 목록이 추가/삭제되었을 때 발생 - 로그인 화면의 드롭다운이 이 이벤트를
    /// 구독해서 목록을 다시 그린다.</summary>
    public static event Action? SitesChanged;

    /// <summary>
    /// 런타임에 다른 서버로 전환. 호출 즉시 ApiClient의 접속주소도 같이 바뀌고,
    /// 기존 인증토큰은 무효화된다(새 서버 기준으로 재로그인 필요 - ShellForm에서 처리).
    /// </summary>
    public static void SwitchEnvironment(string environmentName)
    {
        if (!_config.Value.Environments.ContainsKey(environmentName) && !IsUserSite(environmentName))
            throw new ArgumentException($"'{environmentName}' 환경은 등록되어 있지 않습니다.");

        _currentEnvironment = environmentName;
        SaveLastUsedEnvironment(environmentName);
        ApiClient.Reconfigure(ApiBaseUrl);
        EnvironmentChanged?.Invoke();
    }

    /// <summary>last-environment.txt에 저장된 이름을 읽어온다 - 아직 없거나(최초 실행), 저장된
    /// 이름이 그 사이 삭제/이름변경돼서 더 이상 존재하지 않으면 DefaultEnvironment로 조용히
    /// 대체한다(파일이 깨져 있어도 앱 시작 자체는 막지 않는다, LoadUserSites와 같은 방침).</summary>
    private static string LoadLastUsedEnvironmentOrDefault()
    {
        try
        {
            if (File.Exists(LastEnvironmentPath))
            {
                var saved = File.ReadAllText(LastEnvironmentPath).Trim();
                if (!string.IsNullOrEmpty(saved) && AvailableEnvironments.Contains(saved))
                    return saved;
            }
        }
        catch { }
        return _config.Value.DefaultEnvironment;
    }

    private static void SaveLastUsedEnvironment(string name)
    {
        try
        {
            var dir = Path.GetDirectoryName(LastEnvironmentPath)!;
            Directory.CreateDirectory(dir);
            File.WriteAllText(LastEnvironmentPath, name);
        }
        catch { }
    }

    /// <summary>
    /// 개발자가 새 고객사/서버를 이 PC에 추가한다 - appsettings.json을 안 건드리므로 재배포
    /// 없이 바로 쓸 수 있다. 이름은 내장 서비스/기존 사용자 서비스와 안 겹쳐야 한다.
    /// </summary>
    public static void AddSite(string name, string apiBaseUrl, string modulesPath = "Modules", string coreAssemblyPath = "", string assetsPath = "")
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("이름을 입력해주세요.");
        if (string.IsNullOrWhiteSpace(apiBaseUrl)) throw new ArgumentException("API 주소를 입력해주세요.");
        if (_config.Value.Environments.ContainsKey(name) || IsUserSite(name))
            throw new ArgumentException($"'{name}'은(는) 이미 등록되어 있습니다.");

        _userSites.Value.Add(new UserSiteEntry
        {
            Name = name,
            ApiBaseUrl = apiBaseUrl,
            ModulesPath = modulesPath,
            CoreAssemblyPath = coreAssemblyPath,
            AssetsPath = assetsPath
        });
        SaveUserSites();
        SitesChanged?.Invoke();
    }

    /// <summary>appsettings.json 내장 서비스를 이 PC에서만 직접 덮어쓴다(값뿐 아니라 이름도
    /// 바꿀 수 있다 - 예: Production -> A사). 회사 전체가 appsettings.json을 공유하던 "여러
    /// 사용자" 전제와 달리, 개발자 1명이 여러 고객사(A사/B사...) 서버를 오가며 쓰는 이 PC에서는
    /// Development/Production 같은 내장 이름도 자유롭게 재정의할 수 있어야 한다는 요청으로
    /// 추가함(2026-09-13). 결과물은 평범한 UserSiteEntry라서, 이후 재수정은 UpdateSite가 그대로
    /// 처리한다 - OverridesBuiltin만 원본 이름을 계속 기억해서 나중에 삭제하면 그 원본이 다시
    /// 나타나게(AvailableEnvironments 참고) 해준다.</summary>
    public static void OverrideBuiltin(string builtinName, string newName, string apiBaseUrl, string modulesPath, string coreAssemblyPath, string assetsPath)
    {
        if (!_config.Value.Environments.ContainsKey(builtinName))
            throw new ArgumentException($"'{builtinName}'은(는) 내장 서비스가 아닙니다.");
        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("이름을 입력해주세요.");
        if (string.IsNullOrWhiteSpace(apiBaseUrl)) throw new ArgumentException("API 주소를 입력해주세요.");
        if (newName != builtinName && (_config.Value.Environments.ContainsKey(newName) || IsUserSite(newName)))
            throw new ArgumentException($"'{newName}'은(는) 이미 등록되어 있습니다.");

        var wasCurrent = CurrentEnvironment == builtinName;

        _userSites.Value.Add(new UserSiteEntry
        {
            Name = newName,
            ApiBaseUrl = apiBaseUrl,
            ModulesPath = modulesPath,
            CoreAssemblyPath = coreAssemblyPath,
            AssetsPath = assetsPath,
            OverridesBuiltin = builtinName
        });
        SaveUserSites();

        if (wasCurrent) _currentEnvironment = newName;
        SitesChanged?.Invoke();
    }

    /// <summary>사용자가 추가/오버라이드한 서비스를 삭제한다 - 아직 한 번도 안 건드린 순수 내장
    /// 서비스는 여기서 못 찾으므로 조용히 무시된다. 지운 항목이 OverrideBuiltin으로 만들어진
    /// 것이었다면, 그 원래 내장 이름은 별도 조치 없이도 AvailableEnvironments에 자동으로 다시
    /// 나타난다(오버라이드 항목 자체가 없어졌으므로) - "덮어쓰기를 지우면 원래 기본값으로
    /// 돌아간다"는 직관과 맞아떨어진다.</summary>
    public static void RemoveSite(string name)
    {
        if (_userSites.Value.RemoveAll(s => s.Name == name) == 0) return;
        SaveUserSites();
        SitesChanged?.Invoke();

        // 지금 접속 중인 환경이 방금 지워졌다면(사용자가 자기가 접속한 서비스를 스스로
        // 삭제한 경우) CurrentEnvironment가 더 이상 존재하지 않는 이름을 가리키게 되어
        // ResolveCurrent()가 다음 접근에서 예외를 던진다 - 기본 환경으로 되돌린다.
        if (_currentEnvironment == name) _currentEnvironment = _config.Value.DefaultEnvironment;
    }

    /// <summary>사용자가 추가/오버라이드한 서비스의 접속 정보를 수정한다(이름 변경 포함) - 아직
    /// appsettings.json 내장 상태 그대로인 항목은 여기서 못 찾으므로 조용히 무시된다(그런
    /// 경우는 OverrideBuiltin을 먼저 써야 함 - SiteManagerForm이 IsUserSite로 둘을 구분해서
    /// 알아서 호출한다). 이름을 바꾸는 경우, 그 새 이름이 다른 항목과 겹치지 않는지도
    /// 확인한다(자기 자신과 겹치는 건 당연히 허용).</summary>
    public static void UpdateSite(string oldName, string newName, string apiBaseUrl, string modulesPath, string coreAssemblyPath, string assetsPath)
    {
        var site = _userSites.Value.FirstOrDefault(s => s.Name == oldName);
        if (site == null) return;

        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("이름을 입력해주세요.");
        if (string.IsNullOrWhiteSpace(apiBaseUrl)) throw new ArgumentException("API 주소를 입력해주세요.");
        if (newName != oldName && (_config.Value.Environments.ContainsKey(newName) || IsUserSite(newName)))
            throw new ArgumentException($"'{newName}'은(는) 이미 등록되어 있습니다.");

        var wasCurrent = CurrentEnvironment == oldName;

        site.Name = newName;
        site.ApiBaseUrl = apiBaseUrl;
        site.ModulesPath = modulesPath;
        site.CoreAssemblyPath = coreAssemblyPath;
        site.AssetsPath = assetsPath;
        SaveUserSites();

        // 지금 접속 중인 서비스 자신을 수정한 경우, 새 이름으로 계속 그 서비스를 가리키게
        // 한다(이름이 안 바뀌었으면 이 대입은 그냥 같은 값을 다시 쓰는 것뿐이라 무해함).
        if (wasCurrent) _currentEnvironment = newName;

        SitesChanged?.Invoke();
    }

    /// <summary>appsettings.json 내장 목록 -> 사용자 추가 목록 순으로 찾아서 현재 선택된 서비스의
    /// 접속 정보를 돌려준다.</summary>
    private static (string ApiBaseUrl, string ModulesPath, string CoreAssemblyPath, string AssetsPath) ResolveCurrent()
    {
        var info = GetSiteInfo(CurrentEnvironment);
        if (info != null) return info.Value;

        throw new InvalidOperationException($"'{CurrentEnvironment}' 환경 설정을 찾을 수 없습니다.");
    }

    /// <summary>지금 접속 중인 환경인지와 무관하게, 이름 하나(내장이든 사용자 추가든)로 접속
    /// 정보를 조회한다 - SiteManagerForm이 "회사 공통" 항목을 클릭했을 때도 값을 보여주려고
    /// 추가했다(예전엔 ResolveCurrent가 CurrentEnvironment 전용이라 "지금 안 쓰는 회사 공통
    /// 항목의 값을 미리 들여다보기"가 불가능했다 - 2026-09-13). 사용자 서비스를 내장 목록보다
    /// 먼저 확인한다 - OverrideBuiltin으로 만든 항목(이름이 안 바뀐 경우 내장과 이름이 같음)이
    /// 내장 원본이 아니라 사용자가 고친 값으로 조회되어야 하므로.</summary>
    public static (string ApiBaseUrl, string ModulesPath, string CoreAssemblyPath, string AssetsPath)? GetSiteInfo(string name)
    {
        var site = _userSites.Value.FirstOrDefault(s => s.Name == name);
        if (site != null)
            return (site.ApiBaseUrl, site.ModulesPath, site.CoreAssemblyPath, site.AssetsPath);

        if (_config.Value.Environments.TryGetValue(name, out var env))
            return (env.ApiBaseUrl, env.ModulesPath, env.CoreAssemblyPath, env.AssetsPath);

        return null;
    }

    private static List<UserSiteEntry> LoadUserSites()
    {
        try
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return new();
            if (!File.Exists(UserSitesPath)) return new();

            var json = File.ReadAllText(UserSitesPath);
            return JsonSerializer.Deserialize<List<UserSiteEntry>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch
        {
            // 파일이 깨져 있어도 앱 시작 자체는 막지 않는다 - 그냥 사용자 추가 목록이 비어있는
            // 걸로 취급(내장 서비스는 정상적으로 그대로 쓸 수 있음).
            return new();
        }
    }

    private static void SaveUserSites()
    {
        var dir = Path.GetDirectoryName(UserSitesPath)!;
        Directory.CreateDirectory(dir);
        var json = JsonSerializer.Serialize(_userSites.Value, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(UserSitesPath, json);
    }

    private static ClientConfig Load()
    {
        // VS 디자이너(design-time 서로게이트 프로세스)의 실행 폴더에는 appsettings.json이 없는 게
        // 정상이다. WYNLAB.Controls(TextEditWyn 등)는 이제 AppConfig를 아예 모르니 그쪽에서 이
        // 문제가 생길 일은 구조적으로 없어졌지만, BaseForm 파생 화면이 생성자에서 AppConfig를
        // 직접 건드리는 경우는 여전히 있을 수 있어 방어적으로 남겨둔다. 런타임에는 절대 안 타는
        // 분기이므로 기본값(디자이너 미리보기용)만 돌려주고 넘어간다.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return new ClientConfig();
        }

        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                "appsettings.json을 찾을 수 없습니다. 배포판에 설정파일이 누락되었을 수 있습니다.", path);
        }

        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<ClientConfig>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("appsettings.json 파싱에 실패했습니다.");

        ApplyTheme(config.Theme);
        ApplyBrandDerivedTheme(config.ToolbarColor);
        return config;
    }

    /// <summary>ToolbarColor(appsettings.json의 회사별 브랜드색)에서 파생되는 색을 UiTheme에
    /// 심어둔다. ShellForm의 MDI 문서탭 활성 배경색(NavDarkBg를 흰색 쪽으로 75% 섞은 값)과
    /// 정확히 같은 공식을 여기서도 계산해서 UiTheme.TabActiveBackColor에 넣으면,
    /// TabControlWyn(화면 안쪽 탭)이 그 값을 그대로 읽어써서 MDI 탭과 항상 같은 색을 유지한다 -
    /// 화면마다 따로 계산했다가 공식이 어긋나 색이 미묘하게 달라지는 걸 막기 위함(실제로
    /// 겪음 - 처음엔 TabControlWyn에 고정 흰색을 박아뒀다가 MDI 탭과 색이 달라 보였다).</summary>
    private static void ApplyBrandDerivedTheme(string toolbarColorHex)
    {
        var accent = ColorHelper.FromHex(toolbarColorHex);
        var navDarkBg = ColorHelper.Mix(accent, Color.Black, 0.45f);
        UiTheme.TabActiveBackColor = ColorHelper.Mix(navDarkBg, Color.White, 0.75f);
    }

    /// <summary>appsettings.json의 Theme 섹션 값을 WYNLAB.Controls의 UiTheme(기본값만 있는 색상
    /// 저장소)로 밀어넣는다 - Controls 쪽은 AppConfig를 모르므로, 이 방향(BaseForm -> Controls)으로만
    /// 값이 흐른다. 앱 시작 시(AppConfig가 처음 쓰일 때) 한 번만 실행된다.</summary>
    private static void ApplyTheme(UiThemeConfig theme)
    {
        UiTheme.RequiredFieldBackColor = ColorHelper.FromHex(theme.RequiredFieldBackColor);
        UiTheme.RequiredFieldForeColor = ColorHelper.FromHex(theme.RequiredFieldForeColor);
        UiTheme.TreeGroupBackColor = ColorHelper.FromHex(theme.TreeGroupBackColor);
        UiTheme.TreeGroupForeColor = ColorHelper.FromHex(theme.TreeGroupForeColor);
        UiTheme.TreeLeafBackColor = ColorHelper.FromHex(theme.TreeLeafBackColor);
        UiTheme.TreeLeafForeColor = ColorHelper.FromHex(theme.TreeLeafForeColor);
        UiTheme.TreeModuleBackColor = ColorHelper.FromHex(theme.TreeModuleBackColor);
        UiTheme.DividerColor = ColorHelper.FromHex(theme.DividerColor);
        UiTheme.CardBackColor = ColorHelper.FromHex(theme.CardBackColor);
        UiTheme.CardBorderColor = ColorHelper.FromHex(theme.CardBorderColor);
        UiTheme.SectionHeaderIconColor = ColorHelper.FromHex(theme.SectionHeaderIconColor);
        UiTheme.SectionHeaderTextColor = ColorHelper.FromHex(theme.SectionHeaderTextColor);
        UiTheme.GridHeaderBackColor = ColorHelper.FromHex(theme.GridHeaderBackColor);
        UiTheme.GridHeaderForeColor = ColorHelper.FromHex(theme.GridHeaderForeColor);
        UiTheme.GridFocusedRowBackColor = ColorHelper.FromHex(theme.GridFocusedRowBackColor);
        UiTheme.SplitterBackColor = ColorHelper.FromHex(theme.SplitterBackColor);
    }

    /// <summary>frmSiteConfig(사이트환경설정 > 비밀번호 정책 > 세션)에서 관리자가 지정한
    /// 자리비움 잠금화면 시간(분) - NULL/0이면 잠금 기능 비활성. ShellForm의 유휴감지 타이머가
    /// 이 값을 읽는다. ApplySiteConfigTheme과 같은 자리(로그인 직후 1회)에서 채워진다.</summary>
    public static int? IdleTimeoutMinutes { get; set; }

    /// <summary>frmSiteConfig(사이트환경설정 > 색상)에서 개발자가 배포 시 지정한 값을
    /// appsettings.json의 Theme 위에 덮어쓴다 - DB 값이 appsettings.json보다 우선한다.
    /// 필드가 NULL(미지정)이면 appsettings.json 값을 그대로 둔다. 로그인 성공 직후
    /// (Program.cs, ShellForm 생성 전)에 한 번 호출된다 - SiteThemeSync.ApplyFromServer 참고.</summary>
    public static void ApplySiteConfigTheme(RuntimeSiteConfigDto dto)
    {
        IdleTimeoutMinutes = dto.IdleTimeoutMinutes;
        // net48의 string.IsNullOrWhiteSpace엔 [NotNullWhen(false)]가 없어(.NET Core+ 전용) 아래
        // 가드로 이미 null이 아님을 확인했는데도 컴파일러가 못 알아채고 매번 CS8604로 경고한다 -
        // 전부 안전하다.
        if (!string.IsNullOrWhiteSpace(dto.RequiredFieldBackColor)) UiTheme.RequiredFieldBackColor = ColorHelper.FromHex(dto.RequiredFieldBackColor!);
        if (!string.IsNullOrWhiteSpace(dto.GridHeaderBackColor)) UiTheme.GridHeaderBackColor = ColorHelper.FromHex(dto.GridHeaderBackColor!);
        if (!string.IsNullOrWhiteSpace(dto.GridFocusedRowBackColor)) UiTheme.GridFocusedRowBackColor = ColorHelper.FromHex(dto.GridFocusedRowBackColor!);
        if (!string.IsNullOrWhiteSpace(dto.TreeGroupBackColor)) UiTheme.TreeGroupBackColor = ColorHelper.FromHex(dto.TreeGroupBackColor!);
        if (!string.IsNullOrWhiteSpace(dto.DividerColor)) UiTheme.DividerColor = ColorHelper.FromHex(dto.DividerColor!);
        if (!string.IsNullOrWhiteSpace(dto.BrandColor)) ApplyBrandDerivedTheme(dto.BrandColor!);
    }

    private class ClientConfig
    {
        public Dictionary<string, EnvironmentEntry> Environments { get; set; } = new();
        public string DefaultEnvironment { get; set; } = "Production";
        public string ToolbarColor { get; set; } = "#1B2A3D";
        public UiThemeConfig Theme { get; set; } = new();
        public string AppVersion { get; set; } = string.Empty;
    }

    private class EnvironmentEntry
    {
        public string ApiBaseUrl { get; set; } = string.Empty;
        public string ModulesPath { get; set; } = string.Empty;
        public string CoreAssemblyPath { get; set; } = string.Empty;
        public string AssetsPath { get; set; } = string.Empty;
    }
}

/// <summary>
/// 사용자가 이 PC에서 직접 추가한 서비스 하나 - %LocalAppData%\WYNLAB\sites.json에
/// 목록으로 저장된다. appsettings.json 내장 서비스(EnvironmentEntry)와 필드는 같지만,
/// 딕셔너리 키가 아니라 목록 항목이라 이름을 자기 프로퍼티로 갖고 있어야 한다.
/// </summary>
public class UserSiteEntry
{
    public string Name { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public string ModulesPath { get; set; } = "Modules";
    public string CoreAssemblyPath { get; set; } = string.Empty;
    public string AssetsPath { get; set; } = string.Empty;

    /// <summary>이 항목이 appsettings.json 내장 서비스를 OverrideBuiltin으로 덮어써서 만들어진
    /// 경우, 그 원래 내장 이름(예: "Production") - Name 자체를 다른 이름으로 바꿔 저장해도
    /// (예: "A사") 이 값은 원본을 계속 가리킨다. 이 항목을 삭제하면 그 원래 내장 이름이
    /// AvailableEnvironments에 다시 나타난다. 순수하게 새로 추가한 서비스는 null.</summary>
    public string? OverridesBuiltin { get; set; }
}

/// <summary>
/// 필수입력 표시 등 화면 전반의 시각적 규칙을 담는 설정. appsettings.json의 "Theme" 섹션과 1:1 매핑.
/// 회사별로 이 값들만 바꾸면 전체 화면의 필수입력 스타일이 일괄로 바뀐다.
/// </summary>
public class UiThemeConfig
{
    /// <summary>필수입력 컨트롤(TextEdit, DateEdit, SpinEdit 등)의 배경색</summary>
    public string RequiredFieldBackColor { get; set; } = "#FFF9DB";

    /// <summary>필수입력 컨트롤의 글자색</summary>
    public string RequiredFieldForeColor { get; set; } = "#000000";

    /// <summary>
    /// 트리 화면(메뉴관리 등)에서 "그룹(폴더, 클릭해도 화면이 안 열림)" 행의 배경/글자색 -
    /// 화면(leaf)보다 차분하게 낮춰서 "구획 라벨"이라는 느낌을 준다. 트리를 쓰는 화면이 이후
    /// 늘어나도 전부 이 색을 그대로 재사용해서 톤앤매너를 통일한다.
    /// </summary>
    public string TreeGroupBackColor { get; set; } = "#F2F3F5";
    public string TreeGroupForeColor { get; set; } = "#5A5D64";

    /// <summary>트리 화면에서 실제로 클릭해서 이동 가능한 화면(leaf) 행의 배경/글자색</summary>
    public string TreeLeafBackColor { get; set; } = "#FFFFFF";
    public string TreeLeafForeColor { get; set; } = "#373737";

    /// <summary>메뉴 트리 최상위(MENU_LEVEL=1, "모듈") 행의 배경색 - TreeGroupBackColor보다
    /// 한 단계 더 진하게 해서 중간 폴더와 구분한다.</summary>
    public string TreeModuleBackColor { get; set; } = "#C0C0C0";

    /// <summary>화면명 아래 구분선(PanelWyn.Style = TitleDivider) 색상</summary>
    public string DividerColor { get; set; } = "#E4E5E8";

    /// <summary>그리드/패널을 감싸는 카드(PanelWyn.Style = Card) 배경/테두리 색상</summary>
    public string CardBackColor { get; set; } = "#FFFFFF";
    public string CardBorderColor { get; set; } = "#E1E1E1";

    /// <summary>SectionHeaderWyn(그리드/패널 상단 아이콘+제목) 아이콘/글자 색상</summary>
    public string SectionHeaderIconColor { get; set; } = "#5A5D64";
    public string SectionHeaderTextColor { get; set; } = "#3C3C3C";

    /// <summary>그리드 컬럼헤더 배경/글자색</summary>
    public string GridHeaderBackColor { get; set; } = "#F7F8FA";
    public string GridHeaderForeColor { get; set; } = "#565B62";

    /// <summary>GridViewWyn.HighlightFocusedRow 켰을 때 포커스된 행의 배경색</summary>
    public string GridFocusedRowBackColor { get; set; } = "#FDF3E1";

    /// <summary>스플리터 바(SplitterWyn) 배경색 - 기본은 눈에 안 띄게 카드 배경과 같은 흰색</summary>
    public string SplitterBackColor { get; set; } = "#FFFFFF";
}
