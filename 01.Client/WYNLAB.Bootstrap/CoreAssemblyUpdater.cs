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

            var manifestPath = Path.Combine(coreAssemblyPath, "manifest.json");
            if (!File.Exists(manifestPath)) { Log($"건너뜀: manifest.json을 못 찾음 ({manifestPath})"); return; }

            var manifest = JsonSerializer.Deserialize<CoreAssemblyManifest>(
                File.ReadAllText(manifestPath),
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

            foreach (var file in filesToUpdate)
            {
                UpdateFile(coreAssemblyPath, appDir, file);
            }

            Log($"업데이트 완료: {string.Join(", ", filesToUpdate.Select(f => f.FileName))}");
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
        var sourcePath = Path.Combine(serverDir, file.FileName);
        var targetPath = Path.Combine(appDir, file.FileName);
        var tempPath = targetPath + ".new";

        File.Copy(sourcePath, tempPath, overwrite: true);

        if (ComputeSha256(tempPath) != file.Sha256)
        {
            File.Delete(tempPath);
            throw new InvalidOperationException($"{file.FileName} 다운로드 검증 실패(해시 불일치)");
        }

        File.Delete(targetPath);
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
