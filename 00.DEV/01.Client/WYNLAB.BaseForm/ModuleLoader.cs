using System.Collections.Generic;
using System.Reflection;

namespace WYNLAB.Base;

/// <summary>
/// 화면(메뉴) 하나당 별도 프로젝트/DLL로 분리된 구조를 지원하는 런타임 로더.
///
/// ShellForm.OpenMenuForm은 TSMMENU.FORM_CLASS_NM("Namespace.Type, AssemblyName")을
/// Type.GetType(...)으로 찾는데, 이 메서드는 이미 AppDomain에 로드된 어셈블리에서만 찾는다.
/// 예전엔 Shell이 화면 프로젝트를 전부 ProjectReference해서 빌드 시 자동으로 같이
/// 로드됐지만, 화면별로 프로젝트를 쪼갠 뒤로는 Shell이 더 이상 그 프로젝트들을 참조하지
/// 않는다 - 그래서 메뉴를 열 때 이 클래스가 해당 DLL을 먼저 로드해둬야 Type.GetType이
/// 정상 동작한다.
///
/// 화면 DLL 하나가 깨져 있어도(예: 배포 중 손상) 앱 전체가 죽지 않도록, 개별 DLL 로드
/// 실패는 무시한다 - 그 DLL에 들어있던 메뉴만 "화면을 찾을 수 없습니다"로 처리된다.
///
/// [핵심] Assembly.LoadFrom(경로)이 아니라 File.ReadAllBytes + Assembly.Load(byte[])로
/// 로드한다. 이 두 방식은 결과만 비슷해 보이지 겪는 제약이 완전히 다르다:
///  - LoadFrom(경로)은 파일을 프로세스가 살아있는 내내 잠그고, 같은 이름의 어셈블리는
///    AppDomain에 "한 번만" 올라가서 이후 재로드 시도는 전부 기존 것을 그대로 돌려받는다
///    (교체 불가) - 그래서 예전엔 "이미 연 화면은 재로그인해야 새 버전을 받는다"는 한계가
///    있었다.
///  - Load(byte[])은 파일을 읽는 즉시 핸들을 놓아주고(배포 중 잠금 문제가 원천적으로 없음),
///    같은 이름이라도 부를 때마다 완전히 새로운 Assembly 객체를 만들어준다 - CLR이 이름
///    충돌을 신경 쓰지 않는다. 그래서 "메뉴를 열 때마다 파일이 바뀌었는지 확인하고,
///    바뀌었으면 다시 읽어서 최신 코드로 교체"가 실제로 가능하다. 이미 열려있던 예전
///    인스턴스는 자기가 만들어질 때 잡았던 Type을 계속 참조하므로 전혀 영향받지 않고
///    그대로 동작한다 - 다음에 "새로" 여는 순간부터만 최신 코드를 쓴다. AppDomain을
///    별도로 격리하지 않고도(= DevExpress Form을 도메인 경계 너머로 넘기는 위험 없이)
///    같은 결과를 얻는다. 단, 한 번 로드된 어셈블리 자체는 (옛 버전 포함) 프로세스가
///    끝날 때까지 메모리에 남는다 - 화면 dll 하나가 수십~수백KB 수준이라 재배포가
///    잦아도 무시할 만한 비용이다.
///
/// [중요 - 실제로 겪은 함정] 처음엔 ShellForm이 계속 Type.GetType(assemblyQualifiedName)으로
/// 타입을 찾게 하고, AssemblyResolve만 최신 어셈블리를 돌려주면 매번 새로 반영될 줄
/// 알았는데 실제로는 안 됐다 - 이유: CLR은 "어셈블리 단순 이름 하나"에 대한 바인딩 결과를
/// AppDomain 안에 캐싱한다. Type.GetType이 "WYNLAB.SM.CODE"를 맨 처음 찾을 때만
/// AssemblyResolve가 불리고, 그 이름이 한 번 해석되고 나면 이후 같은 이름에 대한
/// Type.GetType 호출은 CLR이 캐시를 그대로 돌려줄 뿐 AssemblyResolve를 다시 부르지
/// 않는다 - LoadedAssemblies 딕셔너리를 최신으로 갱신해봐야 아무 소용이 없었던 이유가
/// 이거였다. 그래서 호출하는 쪽(ShellForm)이 Type.GetType(전체문자열) 대신, EnsureLoaded가
/// 돌려주는 "그 시점의 실제 Assembly 객체"에서 직접 Assembly.GetType(타입명)을 불러야
/// 한다 - 이건 CLR의 이름 기반 캐시를 아예 거치지 않고 그 어셈블리 인스턴스 안에서만
/// 타입을 찾기 때문에, 매번 최신 코드로 새로 만든 Form을 받을 수 있다.
/// </summary>
public static class ModuleLoader
{
    private static readonly Dictionary<string, Assembly> LoadedAssemblies = new(System.StringComparer.OrdinalIgnoreCase);
    private static readonly Dictionary<string, System.DateTime> LoadedWriteTimesUtc = new(System.StringComparer.OrdinalIgnoreCase);
    private static bool _resolverAttached;

    /// <summary>실제로 dll을 읽어들이는 폴더. HTTP 모드에서는 서버가 아니라 로컬 캐시 폴더를 가리킨다.</summary>
    private static string? _folderPath;

    /// <summary>HTTP 모드일 때만 값이 있다(예: "http://서버:8091/Modules"). null이면 예전 방식(UNC/로컬 폴더).</summary>
    private static string? _serverUrl;

    /// <summary>
    /// 앱 시작 시 한 번만 호출 - 폴더 위치를 기억해두고 AssemblyResolve를 걸어둘 뿐,
    /// 이 시점엔 아무 DLL도 로드하지 않는다(로드는 EnsureLoaded가 메뉴를 열 때마다 한다).
    ///
    /// folderPathOrUrl이 http(s) 주소면 HTTP 배포 모드로 동작한다 - 서버에서 직접 읽는 대신
    /// 로컬 캐시 폴더(%LocalAppData%\WYNLAB\Modules)에 내려받아 두고 그 폴더를 읽는다.
    /// 이렇게 하면 아래 TryLoad의 로직(재귀 탐색, 변경 감지, Assembly.Load(byte[]) 재로드)이
    /// 두 방식에서 완전히 동일하게 동작한다 - HTTP 전환 때문에 그 까다로운 부분을 건드릴
    /// 필요가 없다는 게 이 설계의 핵심이다.
    /// </summary>
    public static void Initialize(string? folderPathOrUrl)
    {
        if (HttpFileSync.IsHttpUrl(folderPathOrUrl))
        {
            _serverUrl = folderPathOrUrl;
            _folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "Modules");
            Directory.CreateDirectory(_folderPath);
        }
        else
        {
            _serverUrl = null;
            _folderPath = folderPathOrUrl;
        }

        if (!_resolverAttached)
        {
            System.AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            _resolverAttached = true;
        }
    }

    /// <summary>
    /// assemblyName(단순 이름, 버전/컬처 없이)의 dll을 Modules 폴더에서 찾아 로드하고, 그
    /// 시점의 Assembly 객체를 돌려준다. 파일이 그 사이 바뀌었으면(=새로 배포됨) 다시 읽어서
    /// 최신 코드로 교체한 다음 그 새 Assembly를 돌려준다. 호출하는 쪽은 반드시 이 반환값에서
    /// 직접 GetType(타입명)을 호출해야 한다(Type.GetType(전체문자열)을 쓰면 안 됨 - 클래스
    /// 설명의 CLR 캐싱 함정 참고). ShellForm.OpenMenuForm이 메뉴를 열 때마다,
    /// TSMMENU.FORM_CLASS_NM에서 뽑아낸 어셈블리명으로 호출한다.
    /// </summary>
    public static Assembly? EnsureLoaded(string assemblyName)
    {
        // HTTP 모드면 여기서 캐시를 서버와 맞춘다. 메뉴를 열 때마다 확인하는 건 UNC 시절에
        // TryLoad가 파일 시각을 매번 확인하던 것과 같은 동작이다 - 재배포 직후 앱을 다시
        // 켜지 않아도 메뉴만 다시 열면 최신 화면이 뜨는 특성을 그대로 유지하기 위함.
        // (아래 OnAssemblyResolve는 일부러 동기화하지 않는다 - 그건 지금 로드 중인 화면의
        //  의존성을 찾는 호출이라, 방금 맞춘 캐시 상태를 그대로 써야 일관성이 맞다.)
        if (_serverUrl != null && _folderPath != null) HttpFileSync.SyncToCache(_serverUrl, _folderPath);

        return TryLoad(assemblyName);
    }

    /// <summary>
    /// 화면 dll이 요청한 이름을 못 찾을 때(기본 로드 컨텍스트/이미 등록된 LoadedAssemblies
    /// 둘 다에 없을 때) 마지막으로 호출된다 - 화면 코드가 직접 참조하는 자기 자신의 어셈블리뿐
    /// 아니라, 그 화면이 내부적으로 필요로 하는 부가 의존성(예: SvgIcon 같은 리소스를 쓰는
    /// 화면의 System.Resources.Extensions.dll)도 여기서 자동으로 찾아 로드된다 - 어떤 화면이
    /// 어떤 의존성을 쓰는지 이 클래스가 미리 알 필요가 없다. Modules 폴더 어딘가에 같은
    /// 이름의 dll이 있기만 하면 EnsureLoaded와 완전히 같은 방식(최신 여부 확인 후 필요하면
    /// 재로드)으로 처리된다.
    /// </summary>
    private static Assembly? OnAssemblyResolve(object sender, System.ResolveEventArgs args)
    {
        var simpleName = new AssemblyName(args.Name).Name;
        if (LoadedAssemblies.TryGetValue(simpleName, out var loaded)) return loaded;

        return TryLoad(simpleName);
    }

    private static Assembly? TryLoad(string assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName)) return null;
        if (string.IsNullOrWhiteSpace(_folderPath) || !Directory.Exists(_folderPath)) return null;

        // 하위 폴더까지 재귀 탐색 - 배포 폴더를 SM/BA/SA/PR/MA 등 모듈별 하위 폴더로 나눠서
        // 관리하는 구조(99.SOURCE\{모듈}\{화면}\ 소스 구조와 대응)와, 화면별 부가 의존성이
        // 공용 위치(Modules\ 바로 밑)에 한 벌만 있는 구조를 둘 다 그대로 지원한다.
        var dllPath = Directory.GetFiles(_folderPath, assemblyName + ".dll", SearchOption.AllDirectories).FirstOrDefault();
        if (dllPath == null) return null;

        var writeTimeUtc = File.GetLastWriteTimeUtc(dllPath);
        if (LoadedWriteTimesUtc.TryGetValue(assemblyName, out var loadedTimeUtc) && loadedTimeUtc == writeTimeUtc)
        {
            return LoadedAssemblies.TryGetValue(assemblyName, out var current) ? current : null;
        }

        try
        {
            var dllBytes = File.ReadAllBytes(dllPath); // 읽는 즉시 핸들을 놓아주므로 배포(덮어쓰기)를 막지 않는다

            var pdbPath = Path.ChangeExtension(dllPath, ".pdb");
            byte[]? pdbBytes = null;
            if (File.Exists(pdbPath))
            {
                try { pdbBytes = File.ReadAllBytes(pdbPath); }
                catch { /* pdb는 디버깅용일 뿐이라 없어도 dll 로드는 계속 진행 */ }
            }

            var assembly = pdbBytes != null ? Assembly.Load(dllBytes, pdbBytes) : Assembly.Load(dllBytes);

            LoadedAssemblies[assembly.GetName().Name] = assembly;
            LoadedWriteTimesUtc[assemblyName] = writeTimeUtc;
            return assembly;
        }
        catch
        {
            // 화면 DLL 하나가 로드에 실패해도(배포 중 손상 등) 앱 전체는 계속 진행한다 -
            // 이 메뉴만 "화면을 찾을 수 없습니다"로 처리된다.
            return null;
        }
    }
}
