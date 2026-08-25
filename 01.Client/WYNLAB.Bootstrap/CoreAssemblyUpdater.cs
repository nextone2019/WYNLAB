using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Forms;

namespace WYNLAB.Bootstrap;

/// <summary>
/// 프레임워크 본체(WYNLAB.BaseForm.dll/WYNLAB.Shared.dll/WYNLAB.Controls.dll)가 실행 중 프로세스에 로드되기
/// 전에, 서버(CoreAssemblyPath 공유폴더)의 최신 버전과 비교해서 다르면 교체한다.
/// 화면 모듈(WYNLAB.SM.*.dll)은 Shell이 리플렉션으로만 알기 때문에 핫스왑이 쉬웠지만,
/// 프레임워크 본체는 Shell.exe와 모든 화면이 컴파일타임에 직접 참조하고 있어서 "이미 로드된
/// 뒤에는 교체 불가" 문제가 있다 - 그래서 Program.cs의 Main() 맨 첫 줄, WYNLAB.Base 타입을
/// 하나도 건드리기 전 시점에 이 메서드를 호출해야 의미가 있다.
///
/// 실패 처리 원칙: 네트워크 문제 등으로 갱신 확인 자체가 안 되면 조용히 넘어간다(예외를 밖으로
/// 던지지 않음) - "최신인지 확인 못 했다"고 로그인 자체를 막으면 서버 공유폴더 접근이 일시적으로
/// 안 되는 것만으로 전체 업무가 멈춰버린다. 최신성보다 가용성이 우선이다.
/// </summary>
public static class CoreAssemblyUpdater
{
    private static readonly string[] TrackedFiles = { "WYNLAB.BaseForm.dll", "WYNLAB.Shared.dll", "WYNLAB.Controls.dll" };

    public static void EnsureUpToDate()
    {
        try
        {
            var coreAssemblyPath = ReadCoreAssemblyPath();
            if (string.IsNullOrWhiteSpace(coreAssemblyPath)) { Log("건너뜀: appsettings.json에 CoreAssemblyPath가 없음"); return; }

            var manifestJson = ReadManifestJson(coreAssemblyPath);
            if (manifestJson == null) return; // 사유는 ReadManifestJson이 이미 로그에 남긴다

            var manifest = JsonSerializer.Deserialize<CoreAssemblyManifest>(
                manifestJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (manifest?.Files == null) { Log("건너뜀: manifest.json 파싱 결과가 비어있음"); return; }

            var appDir = AppContext.BaseDirectory;
            var filesToUpdate = new List<CoreAssemblyManifestFile>();

            foreach (var file in manifest.Files)
            {
                if (Array.IndexOf(TrackedFiles, file.FileName) < 0) continue; // 매니페스트에 다른 파일이 섞여 있어도 여긴 프레임워크 dll만 취급

                var localPath = Path.Combine(appDir, file.FileName);
                if (!File.Exists(localPath) || ComputeSha256(localPath) != file.Sha256)
                {
                    filesToUpdate.Add(file);
                }
            }

            if (filesToUpdate.Count == 0) { Log("이미 최신 상태"); return; }

            // 로드되기 전에 교체해야 하므로(클래스 설명 참고) 이 시점에 파일이 잠겨있다면
            // 그건 "다른 이미 실행 중인 WYNLAB 인스턴스"가 들고 있다는 뜻뿐이다.
            foreach (var file in filesToUpdate)
            {
                var localPath = Path.Combine(appDir, file.FileName);
                if (File.Exists(localPath) && IsFileLocked(localPath))
                {
                    Log($"중단: {file.FileName}이(가) 잠겨있어 업데이트 필요 안내 후 종료");
                    MessageBox.Show(
                        "프로그램 업데이트가 있습니다.\n실행 중인 다른 WYN LAB 창을 모두 닫은 후 다시 실행해주세요.",
                        "업데이트 필요", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(0);
                    return;
                }
            }

            // 파일 하나씩 독립적으로 시도한다 - 예전엔 하나가 실패(throw)하면 그 예외가 밖의
            // catch까지 올라가서 나머지 파일 업데이트까지 전부 취소됐다. 실제로 BaseForm.dll
            // 하나만 계속 실패하는 바람에, 서버에 정상적으로 올라간 Shared.dll조차 하루 넘게
            // 클라이언트에 반영이 안 되는 사고로 이어졌다(2026-08-25 실제로 겪음) - 이제는
            // 각 파일을 따로 시도해서, 문제 있는 파일 하나 때문에 나머지가 발목 잡히지 않는다.
            var updated = new List<string>();
            var failed = new List<string>();
            foreach (var file in filesToUpdate)
            {
                try
                {
                    UpdateFile(coreAssemblyPath, appDir, file);
                    updated.Add(file.FileName);
                }
                catch (Exception ex)
                {
                    failed.Add(file.FileName);
                    Log($"{file.FileName} 갱신 실패(다른 파일은 계속 진행): {ex}");
                }
            }

            if (updated.Count > 0) Log($"업데이트 완료: {string.Join(", ", updated)}");
            if (failed.Count > 0) Log($"업데이트 실패로 예전 버전 유지: {string.Join(", ", failed)}");
        }
        catch (Exception ex)
        {
            // 갱신 확인/적용 실패는 무시하고 로컬에 있는 버전으로 계속 진행한다(클래스 설명
            // 참고) - 하지만 "왜" 실패했는지조차 안 남기면, 클라이언트가 예전 dll을 계속
            // 들고 있는 걸 나중에 우연히 발견하기 전까진 아무도 모른다(실제로 겪음 - 원인
            // 파악에 몇 차례의 수동 파일 비교가 필요했다). 로그 자체도 실패할 수 있으니
            // 이중으로 삼킨다 - 이 메서드는 어떤 경우에도 예외를 밖으로 던지면 안 된다.
            Log($"갱신 실패: {ex}");
        }
    }

    /// <summary>
    /// 배포 위치가 http(s) 주소면 HTTP로, 아니면 예전처럼 공유폴더에서 manifest.json을 읽는다.
    /// 못 읽으면 사유를 로그에 남기고 null - 호출하는 쪽은 그냥 조용히 넘어간다.
    ///
    /// HTTP 전환 이유는 WYNLAB.BaseForm의 HttpFileSync 클래스 설명 참고(SMB/445 포트를 인터넷에
    /// 열지 않기 위함). 이 프로젝트는 BaseForm을 참조할 수 없다는 제약 때문에(클래스 설명 참고)
    /// 그쪽 코드를 재사용하지 못하고 여기에 최소한으로 다시 구현한다 - ReadCoreAssemblyPath가
    /// AppConfig의 파싱을 중복 구현하는 것과 같은 이유의, 같은 종류의 불가피한 중복이다.
    /// </summary>
    private static string? ReadManifestJson(string coreAssemblyPath)
    {
        try
        {
            if (IsHttpUrl(coreAssemblyPath))
            {
                using var client = CreateWebClient();

                // 파일로 읽을 땐 .NET이 BOM을 알아서 걸러주지만 HTTP로 받으면 문자열 맨 앞에
                // U+FEFF가 남아서 System.Text.Json이 파싱에 실패한다 - 떼어낸다.
                return client.DownloadString(CombineUrl(coreAssemblyPath, "manifest.json")).TrimStart('﻿');
            }

            var manifestPath = Path.Combine(coreAssemblyPath, "manifest.json");
            if (!File.Exists(manifestPath)) { Log($"건너뜀: manifest.json을 못 찾음 ({manifestPath})"); return null; }

            return File.ReadAllText(manifestPath);
        }
        catch (Exception ex)
        {
            Log($"건너뜀: manifest.json을 읽지 못함({coreAssemblyPath}): {ex.Message}");
            return null;
        }
    }

    private static bool IsHttpUrl(string value) =>
        value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

    private static string CombineUrl(string baseUrl, string relativePath) =>
        baseUrl.TrimEnd('/') + "/" + relativePath.TrimStart('/');

    /// <summary>HttpClient가 아니라 WebClient를 쓰는 이유와 캐시를 끄는 이유는
    /// WYNLAB.BaseForm의 HttpFileSync.CreateClient 주석 참고(같은 판단).</summary>
    private static System.Net.WebClient CreateWebClient()
    {
        var client = new System.Net.WebClient
        {
            CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
        };
        client.Headers.Add("Cache-Control", "no-cache");
        return client;
    }

    /// <summary>최선노력 로그 - 실패해도(디스크 접근 불가 등) 절대 위로 예외를 던지지 않는다.
    /// 매 실행마다 새로 쓰지 않고 이어 붙여서, 문제가 간헐적으로 재발할 때 이전 시도 기록도
    /// 같이 보인다(오늘 겪은 것처럼 "이번엔 됐는데 다음번엔 왜 또 안 되지" 같은 패턴 파악용).</summary>
    private static void Log(string message)
    {
        try
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB");
            Directory.CreateDirectory(dir);
            File.AppendAllText(Path.Combine(dir, "core-assembly-update.log"), $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }
        catch
        {
        }
    }

    /// <summary>
    /// AppConfig(WYNLAB.BaseForm 소속)를 쓸 수 없는 시점이라 appsettings.json을 직접 다시 읽는다 -
    /// AppConfig.cs의 파싱 로직과 사실상 중복이지만, 이 프로젝트가 BaseForm을 참조하지 않는다는
    /// 제약을 지키기 위한 불가피한 중복이다.
    /// </summary>
    private static string ReadCoreAssemblyPath()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path)) return string.Empty;

        var config = JsonSerializer.Deserialize<MiniConfig>(
            File.ReadAllText(path),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (config?.Environments == null) return string.Empty;

        var envName = config.DefaultEnvironment ?? string.Empty;
        if (!config.Environments.TryGetValue(envName, out var env)) return string.Empty;

        return string.IsNullOrWhiteSpace(env.CoreAssemblyPath) ? string.Empty : env.CoreAssemblyPath;
    }

    /// <summary>다른 WYNLAB 프로세스가 이 dll을 로드해서 "쓰기"를 막고 있는지만 확인한다.
    /// FileShare.None(그 어떤 동시 접근도 불허)으로 열면 백신 실시간 검사 같은 무해한 동시 읽기
    /// 조차 "잠김"으로 오판해서 실제로는 아무도 안 물고 있는데 이 메시지가 계속 뜨는 문제가
    /// 있었다(실제로 겪음) - FileShare.Read로 완화해서 "다른 프로세스의 읽기"는 허용하고
    /// 진짜 문제가 되는 쓰기 충돌만 잡는다.</summary>
    private static bool IsFileLocked(string path)
    {
        try
        {
            using var _ = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return false;
        }
        catch (IOException)
        {
            return true;
        }
    }

    /// <summary>임시 파일명으로 받고 해시를 검증한 다음에야 실제 파일명으로 바꾼다 - 받다가
    /// 끊기거나 손상되면 기존에 잘 동작하던 로컬 파일을 절대 건드리지 않기 위함.</summary>
    private static void UpdateFile(string serverDir, string appDir, CoreAssemblyManifestFile file)
    {
        var targetPath = Path.Combine(appDir, file.FileName);
        var tempPath = targetPath + ".new";

        if (IsHttpUrl(serverDir))
        {
            using var client = CreateWebClient();
            client.DownloadFile(CombineUrl(serverDir, file.FileName), tempPath);
        }
        else
        {
            File.Copy(Path.Combine(serverDir, file.FileName), tempPath, overwrite: true);
        }

        if (ComputeSha256(tempPath) != file.Sha256)
        {
            File.Delete(tempPath);
            throw new InvalidOperationException($"{file.FileName} 다운로드 검증 실패(해시 불일치)");
        }

        ReplaceTarget(targetPath, tempPath, file.FileName);
    }

    /// <summary>
    /// 검증이 끝난 새 파일로 기존 파일을 교체한다. 보통은 삭제 후 이동이면 되지만, 그 dll이
    /// 이미 이 프로세스에 로드(이미지로 매핑)돼 있으면 File.Delete가 UnauthorizedAccessException으로
    /// 실패한다 - 사용 중을 뜻하는 IOException이 아니라서 IsFileLocked 사전 체크로도 못 거른다.
    ///
    /// 다행히 Windows는 "매핑된 파일 삭제"는 막아도 "이름 변경"은 허용하므로, 삭제가 거부되면
    /// 기존 파일을 .old로 밀어내고 그 자리에 새 파일을 넣는다. 이렇게 하면 지금 실행 중인
    /// 프로세스는 이미 메모리에 올라간 예전 코드로 계속 돌지만(그건 어차피 못 바꿈), 최소한
    /// 디스크에는 최신 파일이 자리잡아서 "다음 실행부터는" 정상 반영된다 - 예전엔 이 경우
    /// 영영 갱신이 안 돼서 며칠씩 예전 버전에 머물렀다(2026-08-24~25 실제로 겪음).
    ///
    /// 애초에 로드되기 전에 교체하는 게 정석이고 그건 Program.Main의 구조로 보장한다(Program.cs
    /// 주석 참고) - 여기는 그게 어떤 이유로든 깨졌을 때를 위한 안전망이다.
    /// </summary>
    private static void ReplaceTarget(string targetPath, string tempPath, string fileName)
    {
        try
        {
            File.Delete(targetPath);
        }
        catch (UnauthorizedAccessException)
        {
            var oldPath = targetPath + ".old";
            try { File.Delete(oldPath); } catch { } // 지난번에 밀어둔 게 남아있으면 정리(실패해도 무시)

            File.Move(targetPath, oldPath);
            Log($"{fileName}이(가) 이미 로드돼 있어 삭제 대신 .old로 밀어냄 - 다음 실행부터 반영됨");
        }

        File.Move(tempPath, targetPath);
    }

    private static string ComputeSha256(string path)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(path);
        // .NET Framework 4.8엔 Convert.ToHexString이 없어서(그건 .NET 5+ API) BitConverter로 변환
        return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "");
    }

    private class MiniConfig
    {
        public Dictionary<string, MiniEnvironmentEntry>? Environments { get; set; }
        public string? DefaultEnvironment { get; set; }
    }

    private class MiniEnvironmentEntry
    {
        public string CoreAssemblyPath { get; set; } = string.Empty;
    }
}

public class CoreAssemblyManifest
{
    public List<CoreAssemblyManifestFile> Files { get; set; } = new();
}

public class CoreAssemblyManifestFile
{
    public string FileName { get; set; } = string.Empty;
    public string Sha256 { get; set; } = string.Empty;
    public long Size { get; set; }
}
