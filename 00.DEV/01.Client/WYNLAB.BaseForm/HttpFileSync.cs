using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace WYNLAB.Base;

/// <summary>
/// 서버의 파일 배포 위치가 http(s) 주소일 때, manifest.json을 기준으로 로컬 캐시 폴더를
/// 최신 상태로 맞춘다. ModuleLoader(화면 dll)와 AssetSyncer(아이콘 이미지)가 공유한다.
///
/// 왜 필요한가: 예전엔 배포 파일을 전부 UNC 공유폴더(\\서버\WYNLAB\Modules 등)에서 직접
/// 읽었는데, 그러면 클라이언트가 서버와 SMB(445 포트)로 통신할 수 있어야만 한다. SMB는
/// 사내 LAN 전용으로 설계된 프로토콜이라 인터넷 너머로 열어두는 게 위험하고, 고객사
/// 방화벽에서도 거의 허용되지 않는다. HTTP로 받으면 이미 열려있는 ClickOnce 포트 하나로
/// 전부 해결되고, 어떤 네트워크에서든 동작한다.
///
/// [설계 요점] 여기서 하는 일은 "로컬 캐시 폴더를 서버와 똑같이 맞추는 것"까지다. 실제로
/// dll을 로드하거나 이미지를 읽는 코드는 그 캐시 폴더를 평범한 로컬 폴더로 취급하면 되므로,
/// UNC 시절에 쓰던 로직(ModuleLoader의 재로드 판정 등)을 한 줄도 안 바꾸고 그대로 쓸 수 있다.
///
/// [실패 처리] 네트워크가 안 되면 조용히 넘어가고 캐시에 이미 있는 파일을 계속 쓴다 -
/// CoreAssemblyUpdater와 같은 원칙(최신성보다 가용성 우선)이다. 다만 파일 하나가 실패해도
/// 나머지는 계속 받는다 - 하나의 실패가 전체를 취소시켜서 정상 파일까지 며칠간 반영이
/// 안 됐던 사고를 CoreAssemblyUpdater에서 이미 겪었다(2026-08-25).
/// </summary>
internal static class HttpFileSync
{
    /// <summary>
    /// "이 경로의 파일은 (그때의 수정시각/크기 상태에서) 해시가 이 값이었다"는 확인 결과.
    /// 메뉴를 열 때마다 동기화가 돌기 때문에, 이게 없으면 매번 캐시의 모든 파일을 SHA256으로
    /// 다시 계산하게 된다 - 화면이 수백 개로 늘어나면 메뉴 여는 순간마다 수십 MB를 해싱하는
    /// 셈이라 체감될 만큼 느려진다. 파일이 그대로면(수정시각+크기 동일) 다시 계산하지 않는다.
    /// </summary>
    private static readonly Dictionary<string, (DateTime WriteTimeUtc, long Length, string Sha256)> VerifiedCache =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>설정값이 UNC/로컬 경로가 아니라 http(s) 주소인지 - 이 판정 하나로 두 방식이
    /// 갈린다. 덕분에 appsettings.json의 경로만 되돌리면 즉시 예전 방식으로 복구된다.</summary>
    public static bool IsHttpUrl(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        (value!.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
         value.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// baseUrl의 manifest.json을 읽어서 cacheDir을 최신으로 맞춘다. 실제로 받은 파일이
    /// 하나라도 있으면 true(호출하는 쪽이 캐시 무효화 같은 후처리를 할 수 있게).
    /// </summary>
    public static bool SyncToCache(string baseUrl, string cacheDir)
    {
        try
        {
            var manifest = DownloadManifest(baseUrl);
            if (manifest?.Files == null || manifest.Files.Count == 0) return false;

            Directory.CreateDirectory(cacheDir);

            var changed = false;
            foreach (var file in manifest.Files)
            {
                if (string.IsNullOrWhiteSpace(file.FileName) || string.IsNullOrWhiteSpace(file.Sha256)) continue;

                try
                {
                    if (DownloadIfChanged(baseUrl, cacheDir, file)) changed = true;
                }
                catch (Exception ex)
                {
                    // 파일 하나가 실패해도 나머지는 계속 받는다(클래스 설명 참고).
                    Log($"{file.FileName} 받기 실패(나머지는 계속 진행): {ex.Message}");
                }
            }

            return changed;
        }
        catch (Exception ex)
        {
            // 서버에 아예 못 붙는 경우 등 - 캐시에 있는 걸로 계속 진행한다.
            Log($"동기화 실패({baseUrl}): {ex.Message}");
            return false;
        }
    }

    private static bool DownloadIfChanged(string baseUrl, string cacheDir, ManifestFile file)
    {
        // manifest의 fileName은 슬래시로 구분된 상대경로다(예: "SM/WYNLAB.SM.MENU.dll") -
        // 배포 폴더를 모듈별 하위폴더로 나눠 쓰는 구조를 그대로 표현하기 위함.
        var localPath = Path.Combine(cacheDir, file.FileName.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(localPath) && LocalHash(localPath) == file.Sha256) return false;

        var localDir = Path.GetDirectoryName(localPath);
        if (!string.IsNullOrEmpty(localDir)) Directory.CreateDirectory(localDir!);

        // 임시 파일로 받고 해시를 검증한 뒤에야 제자리에 넣는다 - 받다가 끊기거나 손상되면
        // 기존에 잘 쓰던 캐시 파일을 건드리지 않기 위함(CoreAssemblyUpdater와 같은 안전장치).
        var tempPath = localPath + ".new";
        using (var client = CreateClient())
        {
            client.DownloadFile(CombineUrl(baseUrl, file.FileName), tempPath);
        }

        if (ComputeSha256(tempPath) != file.Sha256)
        {
            File.Delete(tempPath);
            throw new InvalidOperationException("해시 불일치");
        }

        if (File.Exists(localPath)) File.Delete(localPath);
        File.Move(tempPath, localPath);
        Remember(localPath, file.Sha256);
        Log($"{file.FileName} 갱신됨");
        return true;
    }

    /// <summary>로컬 파일의 해시 - 직전에 확인해둔 뒤로 파일이 그대로면(수정시각+크기 동일)
    /// 다시 계산하지 않고 기억해둔 값을 쓴다(VerifiedCache 설명 참고).</summary>
    private static string LocalHash(string localPath)
    {
        var info = new FileInfo(localPath);
        if (VerifiedCache.TryGetValue(localPath, out var known) &&
            known.WriteTimeUtc == info.LastWriteTimeUtc && known.Length == info.Length)
        {
            return known.Sha256;
        }

        var sha = ComputeSha256(localPath);
        VerifiedCache[localPath] = (info.LastWriteTimeUtc, info.Length, sha);
        return sha;
    }

    private static void Remember(string localPath, string sha256)
    {
        var info = new FileInfo(localPath);
        VerifiedCache[localPath] = (info.LastWriteTimeUtc, info.Length, sha256);
    }

    private static Manifest? DownloadManifest(string baseUrl)
    {
        using var client = CreateClient();
        var json = client.DownloadString(CombineUrl(baseUrl, "manifest.json"));

        // 파일로 읽을 땐 .NET이 BOM을 알아서 걸러주지만 HTTP로 받으면 문자열 맨 앞에 U+FEFF가
        // 그대로 남아서 System.Text.Json이 "invalid start of a value"로 실패한다. 매니페스트를
        // 만드는 쪽(Generate-Manifest.ps1)에서도 BOM을 안 붙이지만, 다른 도구로 만든 파일이
        // 섞여 들어와도 죽지 않도록 여기서도 떼어낸다.
        return JsonSerializer.Deserialize<Manifest>(json.TrimStart('﻿'),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    /// <summary>
    /// HttpClient가 아니라 WebClient를 쓰는 이유: 이 동기화는 UI 스레드에서(메뉴를 여는
    /// 순간) 동기적으로 실행되는데, HttpClient는 비동기 전용이라 .Result/.GetAwaiter()로
    /// 기다리면 WinForms의 SynchronizationContext와 얽혀 교착이 날 수 있다. WebClient는
    /// 동기 메서드가 정식으로 제공되어 그 위험이 없다.
    ///
    /// 캐시 무효화: 프록시나 IIS가 예전 응답을 돌려주면 "분명 새로 배포했는데 반영이 안 되는"
    /// 최악의 증상이 되므로 캐시를 명시적으로 끈다. 해시 검증이 있어서 잘못된 파일을 쓰지는
    /// 않지만, 계속 옛날 파일만 받아오면 갱신 자체가 영영 안 된다.
    /// </summary>
    private static WebClient CreateClient()
    {
        var client = new WebClient { CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore) };
        client.Headers.Add("Cache-Control", "no-cache");
        return client;
    }

    private static string CombineUrl(string baseUrl, string relativePath) =>
        baseUrl.TrimEnd('/') + "/" + relativePath.TrimStart('/');

    private static string ComputeSha256(string path)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(path);
        // .NET Framework 4.8엔 Convert.ToHexString이 없어서(그건 .NET 5+ API) BitConverter로 변환
        return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "");
    }

    /// <summary>최선노력 로그 - 실패해도 절대 위로 예외를 던지지 않는다. 배포 파일이 왜
    /// 반영이 안 되는지를 원격지 PC에서 추적하려면 이런 기록이 반드시 필요하다는 걸
    /// CoreAssemblyUpdater에서 이미 경험했다(그 로그가 없었으면 원인 파악이 불가능했다).</summary>
    private static void Log(string message)
    {
        try
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB");
            Directory.CreateDirectory(dir);
            File.AppendAllText(Path.Combine(dir, "file-sync.log"), $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }
        catch
        {
        }
    }

    private class Manifest
    {
        public List<ManifestFile> Files { get; set; } = new();
    }

    private class ManifestFile
    {
        public string FileName { get; set; } = string.Empty;
        public string Sha256 { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}
