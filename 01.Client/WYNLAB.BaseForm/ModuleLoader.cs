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
/// </summary>
public static class ModuleLoader
{
    public static void LoadAll(string? folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) return;

        foreach (var dllPath in Directory.GetFiles(folderPath, "*.dll"))
        {
            try
            {
                Assembly.LoadFrom(dllPath);
            }
            catch
            {
                // 화면 DLL 하나가 로드에 실패해도 나머지 화면/앱 시작 자체는 계속 진행한다.
            }
        }
    }
}
