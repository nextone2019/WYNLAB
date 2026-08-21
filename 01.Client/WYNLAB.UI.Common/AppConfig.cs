using System.Text.Json;

namespace WYNLAB.UI.Common;

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
    private static string? _currentEnvironment;

    public static string CurrentEnvironment => _currentEnvironment ??= _config.Value.DefaultEnvironment;

    public static IReadOnlyList<string> AvailableEnvironments => _config.Value.Environments.Keys.ToList();

    public static string ApiBaseUrl =>
        _config.Value.Environments.TryGetValue(CurrentEnvironment, out var env)
            ? env.ApiBaseUrl
            : throw new InvalidOperationException($"'{CurrentEnvironment}' 환경 설정이 appsettings.json에 없습니다.");

    /// <summary>
    /// 화면별로 쪼개진 DLL(WYNLAB.Screens.*)들이 놓여있는 폴더 - 시작 시 ModuleLoader가
    /// 이 폴더를 스캔해서 전부 로드한다. 환경별로 다른 경로를 가질 수 있다(운영은 보통
    /// 서버의 공유폴더/배포경로, 개발은 각 화면 프로젝트의 빌드 후 자동복사 대상 폴더).
    /// appsettings.json에 상대경로(예: "Modules")로 적으면 exe 폴더 기준으로 풀어준다 -
    /// 개발 PC마다 저장소 경로가 다를 수 있어서 절대경로를 하드코딩하지 않기 위함.
    /// </summary>
    public static string ModulesPath
    {
        get
        {
            if (!_config.Value.Environments.TryGetValue(CurrentEnvironment, out var env) ||
                string.IsNullOrWhiteSpace(env.ModulesPath))
            {
                return string.Empty;
            }

            return Path.IsPathRooted(env.ModulesPath)
                ? env.ModulesPath
                : Path.Combine(AppContext.BaseDirectory, env.ModulesPath);
        }
    }

    public static string ToolbarColor => _config.Value.ToolbarColor;

    public static UiThemeConfig Theme => _config.Value.Theme;

    public static bool IsDevelopment => CurrentEnvironment.Equals("Development", StringComparison.OrdinalIgnoreCase);

    /// <summary>서버 전환시 발생 - ShellForm이 이 이벤트를 구독해서 UI를 갱신할 수 있음</summary>
    public static event Action? EnvironmentChanged;

    /// <summary>
    /// 런타임에 다른 서버로 전환. 호출 즉시 ApiClient의 접속주소도 같이 바뀌고,
    /// 기존 인증토큰은 무효화된다(새 서버 기준으로 재로그인 필요 - ShellForm에서 처리).
    /// </summary>
    public static void SwitchEnvironment(string environmentName)
    {
        if (!_config.Value.Environments.ContainsKey(environmentName))
            throw new ArgumentException($"'{environmentName}' 환경은 appsettings.json에 정의되어 있지 않습니다.");

        _currentEnvironment = environmentName;
        ApiClient.Reconfigure(ApiBaseUrl);
        EnvironmentChanged?.Invoke();
    }

    private static ClientConfig Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                "appsettings.json을 찾을 수 없습니다. 배포판에 설정파일이 누락되었을 수 있습니다.", path);
        }

        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<ClientConfig>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return config ?? throw new InvalidOperationException("appsettings.json 파싱에 실패했습니다.");
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
    }
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
}
