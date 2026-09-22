using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 로그인 성공 직후 서버(TSMSITECONFIG, frmSiteConfig에서 개발자가 지정)의 색상값을 받아와
/// UiTheme에 덮어쓴다 - AssetSyncer(아이콘 이미지 동기화)와 같은 자리(Program.cs, ShellForm
/// 생성 직전)에서 호출되지만, 이쪽은 API 호출(GET api/site-config/runtime)이라 반드시 로그인
/// (ApiClient.SetAuthToken) 이후에만 불러야 한다는 점이 다르다.
/// </summary>
public static class SiteThemeSync
{
    /// <summary>서버에 연결 못 하거나 아직 설정이 없어도(NotFound 등) 무시하고 계속 진행한다 -
    /// appsettings.json Theme 값을 그대로 쓰는 것과 동일하게 취급하면 된다(AssetSyncer와 같은
    /// 원칙 - 이 동기화는 있으면 좋은 것이지, 실패했다고 로그인 자체를 막을 이유는 없다).</summary>
    public static void ApplyFromServer()
    {
        try
        {
            var dto = ApiClient.GetAsync<RuntimeSiteConfigDto>("api/site-config/runtime").GetAwaiter().GetResult();
            if (dto != null) AppConfig.ApplySiteConfigTheme(dto);
        }
        catch
        {
            // 무시 - appsettings.json 기본 테마로 계속 진행
        }
    }
}
