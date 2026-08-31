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

        // PopupLookupEditWyn("..." 버튼)이 실제로 팝업을 여는 방법 - PopupLookupForm이
        // sysPopUpM/sysPopUpD 정의를 읽어서 스스로 그린다(엔티티별 폼 클래스 없음).
        PopupLookupProvider.OpenPopup = PopupLookupForm.ShowAsync;

        // PopupLookupEditWyn의 멀티필드 모드가 Leave 시 부르는 "조용한" 검색 - 팝업 UI 없이
        // 결과 행들만 돌려준다. 그 popup_key에 정의된 조회조건(sysPopUpS) 전부에 keyword를
        // 그대로 넣어 검색한다 - 어느 조건이 몇 개인지는 컨트롤이 몰라도 되게, 여기서 정의를
        // 먼저 읽어서 처리한다(같은 원칙: PopupLookupForm.BuildSearchPanel의 initialKeyword 전파).
        PopupLookupProvider.SearchExact = async (popupKey, keyword) =>
        {
            var def = await ApiClient.GetAsync<PopupDefinitionDto>($"api/lookups/{Uri.EscapeDataString(popupKey)}/definition");
            if (def == null) return new List<PopupLookupResult>();

            var conditions = def.SearchFields.ToDictionary(f => f.ParamNm, f => (string?)keyword);
            var response = await ApiClient.PostAsync<Dictionary<string, string?>, DataQueryResponse>(
                $"api/lookups/{Uri.EscapeDataString(popupKey)}/search", conditions);

            if (response == null || response.Tables.Count == 0) return new List<PopupLookupResult>();

            return response.Tables[0].Rows.Select(row =>
            {
                var rowStrings = row.ToDictionary(kv => kv.Key, kv => (string?)Convert.ToString(kv.Value));
                rowStrings.TryGetValue(def.KeyField, out var code);
                rowStrings.TryGetValue(def.DisplayField, out var display);
                return new PopupLookupResult { Code = code ?? string.Empty, Display = display ?? string.Empty, Row = rowStrings };
            }).ToList();
        };

        // LookUpEditWyn.LookupKey가 값을 가져오는 방법 - CodeLookupProvider.Providers처럼
        // 프로시져별로 등록하지 않는다. LookUp 이름 + 파라미터를 그대로 서버에 넘기면 서버가
        // sysLookupM/P를 보고 어떤 프로시져를 어떤 파라미터로 실행할지 알아서 처리한다(frmSysLookup
        // 참고) - 그래서 새 LookUp을 추가해도 이 등록 자체는 코드 변경이 필요 없다.
        ComboLookupProvider.Fetch = async (lookupKey, parameters) =>
        {
            var items = await ApiClient.PostAsync<Dictionary<string, string?>, List<LookupItemDto>>(
                $"api/combo-lookups/{Uri.EscapeDataString(lookupKey)}/items", parameters) ?? new();
            return items.Select(i => new CodeLookupItem { Value = i.Value, Display = i.Display });
        };
    }
}
