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

    /// <summary>fileNameWithoutExtension(예: "query_hover")에 해당하는 이미지를 폴더에서 찾아
    /// 그대로 돌려준다. 없거나 못 읽으면 null - 호출하는 쪽(IconBadgeButton)은 그 경우 그냥
    /// 아이콘 없이 배지만 그린다, 특별 취급 없음. 실패는 왜 실패했는지 file-sync.log에
    /// 한 줄 남긴다(2026-08-28, 배포된 png인데 계속 안 보이는 문제의 원인을 못 좁혀서 추가함 -
    /// 다음에 또 재발하면 이 로그로 파일이 없는 건지 읽다가 예외가 난 건지 바로 알 수 있다).
    /// 성공한 결과만 캐시하고, 실패는 캐시하지 않아 다음 Paint에서 다시 시도된다.</summary>
    public static Image? GetImage(string fileNameWithoutExtension)
    {
        if (Cache.TryGetValue(fileNameWithoutExtension, out var cached))
        {
            Log($"{fileNameWithoutExtension} 캐시 적중 -> {(cached == null ? "null" : "이미지 있음")}");
            return cached;
        }

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
                Log($"{fileNameWithoutExtension} 새로 읽음 성공 (경로: {path}, {image.Width}x{image.Height})");
            }
            else
            {
                Log($"{fileNameWithoutExtension}.png 없음 (경로: {path})");
            }
        }
        catch (Exception ex)
        {
            Log($"{fileNameWithoutExtension}.png 읽기 실패: {ex}");
            return null;
        }

        Cache[fileNameWithoutExtension] = image;
        return image;
    }

    /// <summary>최선노력 로그 - HttpFileSync.Log와 같은 파일(file-sync.log)에 남긴다.</summary>
    private static void Log(string message)
    {
        try
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB");
            Directory.CreateDirectory(dir);
            File.AppendAllText(Path.Combine(dir, "file-sync.log"), $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] IconAssetProvider: {message}{Environment.NewLine}");
        }
        catch
        {
        }
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
