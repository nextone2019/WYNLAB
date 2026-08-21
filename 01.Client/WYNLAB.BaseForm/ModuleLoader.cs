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
/// 않는다 - 그래서 앱 시작 시 이 클래스가 지정된 폴더의 모든 DLL을 Assembly.LoadFrom으로
/// 먼저 로드해둬야 Type.GetType이 정상 동작한다.
///
/// 화면 DLL 하나가 깨져 있어도(예: 배포 중 손상) 앱 전체가 죽지 않도록, 개별 DLL 로드
/// 실패는 무시한다 - 그 DLL에 들어있던 메뉴만 "화면을 찾을 수 없습니다"로 처리된다.
///
/// Assembly.LoadFrom으로 로드한 어셈블리는 CLR의 "LoadFrom 컨텍스트"라는 별도 바인딩
/// 컨텍스트에 들어간다. Type.GetType(assemblyQualifiedName)이 어셈블리를 이름만으로
/// 찾을 때는 기본 로드 컨텍스트만 뒤지기 때문에, LoadFrom 컨텍스트에 있는 어셈블리는
/// 못 찾고 FileNotFoundException을 삼켜서 그냥 null을 돌려준다. AssemblyResolve
/// 이벤트를 걸어서 이미 로드해둔 어셈블리를 직접 돌려줘야 Type.GetType이 찾을 수 있다.
/// </summary>
public static class ModuleLoader
{
    private static readonly Dictionary<string, Assembly> LoadedAssemblies = new(System.StringComparer.OrdinalIgnoreCase);
    private static bool _resolverAttached;

    public static void LoadAll(string? folderPath)
    {
        if (!_resolverAttached)
        {
            System.AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            _resolverAttached = true;
        }

        if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) return;

        // 하위 폴더까지 재귀 탐색 - 배포 폴더를 SM/BA/SA/PR/MA 등 모듈별 하위 폴더로
        // 나눠서 관리하는 구조(99.SOURCE\{모듈}\{화면}\ 소스 구조와 대응)를 그대로 지원한다.
        foreach (var dllPath in Directory.GetFiles(folderPath, "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                LoadedAssemblies[assembly.GetName().Name] = assembly;
            }
            catch
            {
                // 화면 DLL 하나가 로드에 실패해도 나머지 화면/앱 시작 자체는 계속 진행한다.
            }
        }
    }

    private static Assembly? OnAssemblyResolve(object sender, System.ResolveEventArgs args)
    {
        var simpleName = new AssemblyName(args.Name).Name;
        return LoadedAssemblies.TryGetValue(simpleName, out var assembly) ? assembly : null;
    }
}
