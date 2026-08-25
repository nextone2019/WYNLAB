namespace WYNLAB.Base;

/// <summary>
/// 서버 공유폴더(AppConfig.AssetsPath)의 툴바 아이콘/로고/배경 이미지를
/// %LocalAppData%\WYNLAB\Assets\(IconAssetProvider.AssetsFolder)로 동기화한다.
///
/// CoreAssemblyUpdater(BaseForm/Shared/Controls dll)와 목적은 비슷하지만 훨씬 단순하다 -
/// 그쪽은 "이미 프로세스에 로드된 dll은 교체 불가"라는 제약 때문에 Main() 맨 첫 줄에서,
/// 파일 잠금까지 신경 쓰며 실행해야 했다. 이미지는 dll처럼 로드되어 잠기는 게 아니라
/// IconAssetProvider가 매번 파일을 읽어 Bitmap 사본을 만들 뿐이라, 그런 제약이 없다 -
/// 그래서 시점도 자유롭고(로그인 전, AppConfig를 쓸 수 있게 된 후 아무 때나) 실패해도
/// 그냥 "서버에 새 이미지 없음"과 똑같이 취급하면 된다(로컬에 이미 있으면 그걸 계속 쓰고,
/// 아예 없으면 IconBadgeButton이 코드로 그리는 것으로 자동 폴백).
/// </summary>
public static class AssetSyncer
{
    /// <summary>Program.cs에서 로그인 창을 띄우기 전에 한 번 호출한다. 관리자가 서버
    /// AssetsPath 폴더에 이미지를 새로 올려두면, 사용자는 그냥 평소처럼 실행하는 것만으로
    /// 자동으로 최신 이미지를 받는다 - 폴더 위치를 몰라도, 파일을 옮길 줄 몰라도 된다.</summary>
    public static void SyncFromServer()
    {
        try
        {
            var serverPath = AppConfig.AssetsPath;

            // http(s) 주소면 manifest.json 기반으로 받아온다(HttpFileSync 설명 참고).
            // UNC 공유폴더 방식은 아래 기존 경로 그대로 - 설정값만 바꾸면 양쪽을 오갈 수 있다.
            if (HttpFileSync.IsHttpUrl(serverPath))
            {
                if (HttpFileSync.SyncToCache(serverPath, IconAssetProvider.AssetsFolder)) IconAssetProvider.ClearCache();
                return;
            }

            if (string.IsNullOrWhiteSpace(serverPath) || !Directory.Exists(serverPath)) return;

            Directory.CreateDirectory(IconAssetProvider.AssetsFolder);

            var changed = false;
            foreach (var serverFile in Directory.GetFiles(serverPath, "*.png"))
            {
                var localFile = Path.Combine(IconAssetProvider.AssetsFolder, Path.GetFileName(serverFile));
                if (!NeedsCopy(serverFile, localFile)) continue;

                // 임시 파일명으로 받고 나서 바꿔치기 - 받는 도중 문제가 생겨도 기존 로컬
                // 이미지가 깨지지 않는다(CoreAssemblyUpdater와 같은 안전장치).
                var tempFile = localFile + ".new";
                File.Copy(serverFile, tempFile, overwrite: true);
                File.Delete(localFile);
                File.Move(tempFile, localFile);
                changed = true;
            }

            if (changed) IconAssetProvider.ClearCache();
        }
        catch
        {
            // 서버 공유폴더에 접근 못 해도(네트워크 문제 등) 무시하고 계속 진행한다 - 이미
            // 로컬에 캐시된 이미지를 그대로 쓰거나, 하나도 없으면 코드 렌더링으로 폴백된다.
        }
    }

    /// <summary>크기나 최종수정시각이 다르면 새로 받는다 - 매번 SHA256을 계산하는 것보다
    /// 훨씬 빠르고, 아이콘 이미지 파일 정도 규모에서는 이 정도로 충분하다.</summary>
    private static bool NeedsCopy(string serverFile, string localFile)
    {
        if (!File.Exists(localFile)) return true;

        var serverInfo = new FileInfo(serverFile);
        var localInfo = new FileInfo(localFile);
        return serverInfo.Length != localInfo.Length || serverInfo.LastWriteTimeUtc > localInfo.LastWriteTimeUtc;
    }
}
