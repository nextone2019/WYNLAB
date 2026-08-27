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
/// 코드로 직접 그리던 방식(예전 ToolbarIconPainters)은 버튼마다 톤/색이 들쭉날쭉해지는 문제가
/// 있어서 완전히 폐기했다 - 이제 모든 툴바 버튼은 반드시 이 폴더에 대응하는 png를 갖는다
/// (원본은 WYNLAB.Shell/DefaultAssets, 배포 시 서버 Assets 폴더로 동기화됨). 파일이 없으면
/// null을 돌려주고 IconBadgeButton은 그 자리에 아이콘 없이 배지만 그린다.
/// </summary>
public static class IconAssetProvider
{
    public static readonly string AssetsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "Assets");

    private static readonly Dictionary<string, Image?> Cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>fileNameWithoutExtension(예: "query_hover")에 해당하는 이미지를 찾아 돌려준다.
    /// 없거나 손상됐으면 null. 성공한 결과만 캐시해서 매 Paint마다 디스크를 다시 뒤지지 않는다 -
    /// ClearCache()로 관리자가 이미지를 교체한 뒤 다시 읽게 할 수 있다.
    ///
    /// [중요] 실패(null)는 캐시하지 않는다 - 예전엔 실패도 캐시했는데, 배포 직후 백신
    /// (AhnLab Safe Transaction Service 등)이 방금 쓰인 png를 스캔하느라 아주 짧게 파일을
    /// 잠그는 순간과 겹치면 IOException으로 실패하고, 그 결과(null)가 프로세스 종료까지
    /// 영구히 캐시되어 다음 Paint부터는 파일이 멀쩡해도 계속 빈 배지만 보였다(2026-08-27
    /// 아이콘 코드-드로잉 제거 직후 실제로 겪음 - 그 전엔 실패하면 코드 렌더링으로 조용히
    /// 폴백해서 증상이 "가끔 빨간 아이콘"으로만 보였을 뿐, 이 캐시 버그 자체는 이미 있었다).
    /// 실패를 캐시 안 해도 성공하는 보통의 경우엔 비용이 없고, 실패하는 드문 경우에만 다음
    /// Paint에서 다시 시도한다.</summary>
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
            // 손상된 파일/일시적 잠금 등은 조용히 무시하고 이번엔 아이콘 없이 넘어간다 -
            // 캐시하지 않으므로 다음 Paint(예: 마우스 오버)에서 다시 시도된다.
            return null;
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
