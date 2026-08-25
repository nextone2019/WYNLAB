using System.Drawing;

namespace WYNLAB.Base;

/// <summary>
/// 툴바 아이콘/배경/로고를 코드로 그리는 대신 파일로 바꿀 수 있게 하는 로더.
///
/// 이미지는 ClickOnce 설치 폴더(AppContext.BaseDirectory) 안이 아니라
/// %LocalAppData%\WYNLAB\Assets\ 에 둔다 - ClickOnce는 배포할 때마다 버전별로 새 폴더에
/// 통째로 새로 설치하므로(예: ...wynl..tion_..._d1358f4467631d50\), 설치 폴더 안에 뒀다면
/// 관리자가 넣어둔 커스텀 이미지가 앱을 업데이트할 때마다 사라진다. LocalAppData는 업데이트와
/// 무관하게 그대로 남는다 - sites.json(AppConfig.AddSite)이나 core-assembly-update.log
/// (CoreAssemblyUpdater)도 이미 같은 이유로 여기 두고 있다.
///
/// 명명 규칙(전부 PNG, 소문자):
///   {이름}.png           기본(Normal) 상태 - 필수
///   {이름}_hover.png     마우스 오버 - 선택, 없으면 기본으로 대체
///   {이름}_pressed.png   눌림 - 선택, 없으면 기본으로 대체
///   {이름}_disabled.png  비활성화 - 선택, 없으면 기본으로 대체
/// 상태별 이미지가 없어도 배지 배경/오버레이(IconBadgeButton)가 여전히 호버/눌림/비활성
/// 피드백을 표현하므로, 관리자가 {이름}.png 하나만 넣어도 정상적으로 보인다.
///
/// 파일이 아예 없으면(폴더가 비어있거나 그 아이콘만 없으면) null을 돌려주고, 호출하는 쪽이
/// 예전처럼 코드로 직접 그리는 방식(ToolbarIconPainters 등)으로 폴백한다 - 그래서 이 폴더를
/// 통째로 비워도 앱이 깨지지 않는다.
/// </summary>
public static class IconAssetProvider
{
    public static readonly string AssetsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "Assets");

    private static readonly Dictionary<string, Image?> Cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>fileNameWithoutExtension(예: "query_hover")에 해당하는 이미지를 찾아 돌려준다.
    /// 없거나 손상됐으면 null. 한 번 찾은 결과(성공/실패 모두)는 캐시해서 매 Paint마다
    /// 디스크를 다시 뒤지지 않는다 - ClearCache()로 관리자가 이미지를 교체한 뒤 다시 읽게 할 수 있다.</summary>
    public static Image? GetImage(string fileNameWithoutExtension)
    {
        if (Cache.TryGetValue(fileNameWithoutExtension, out var cached)) return cached;

        Image? image = null;
        try
        {
            var path = Path.Combine(AssetsFolder, fileNameWithoutExtension + ".png");
            if (File.Exists(path))
            {
                using var fs = File.OpenRead(path);
                using var loaded = Image.FromStream(fs);
                // 원본 스트림/파일 핸들과 완전히 분리된 사본을 만든다 - Image.FromStream이 돌려주는
                // 객체는 내부적으로 스트림이 열려있어야 하는 경우가 있어서, using이 끝나 스트림이
                // 닫히고 나면 이미지가 깨질 수 있다.
                image = new Bitmap(loaded);
            }
        }
        catch
        {
            // 손상된 파일 등은 조용히 무시하고 폴백(코드 렌더링)으로 넘어간다.
        }

        Cache[fileNameWithoutExtension] = image;
        return image;
    }

    /// <summary>관리자가 실행 중에 이미지를 교체했을 때 다시 읽게 하고 싶으면 호출(현재는
    /// 앱을 재시작하면 자연히 새로 읽으므로 필수는 아니고, 나중에 "이미지 다시 불러오기"
    /// 같은 기능을 만들 때 쓰라고 남겨둔 것).</summary>
    public static void ClearCache()
    {
        foreach (var img in Cache.Values) img?.Dispose();
        Cache.Clear();
    }
}
