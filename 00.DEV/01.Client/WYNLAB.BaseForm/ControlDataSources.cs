using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// WYNLAB.Controls가 노출한 데이터 조회 훅(LookUpEditWyn.ProcName/Where)에 실제 구현(ApiClient 호출)을
/// 연결한다. WYNLAB.Controls는 ApiClient/WYNLAB.Shared를 몰라도 되게(디자인타임 안전성 유지) 만든
/// 구조라, 이 방향(BaseForm -> Controls)으로만 값을 채워준다 - AppConfig.ApplyTheme()와 같은 패턴.
/// Program.cs의 Main()에서 앱 시작 시 딱 한 번 호출하면 된다.
///
/// 새 콤보/LookUp 프로시져(SSP_CBO_*)를 추가할 때마다 여기 Providers에 한 줄씩 등록한다 -
/// 등록된 프로시져 이름만 LookUpEditWyn.ProcName에서 실제로 호출 가능하다(화이트리스트).
/// </summary>
public static class ControlDataSources
{
    public static void Initialize()
    {
        CodeLookupProvider.Providers["SSP_CBO_CODE_Q"] = async where =>
        {
            var items = await ApiClient.GetAsync<List<CodeLookupItemDto>>(
                $"api/codes/lookup?where={Uri.EscapeDataString(where)}") ?? new();
            return items.Select(i => new CodeLookupItem { Value = i.minor_cd, Display = i.minor_nm ?? string.Empty });
        };
    }
}
