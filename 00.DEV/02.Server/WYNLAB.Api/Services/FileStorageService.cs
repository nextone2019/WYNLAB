namespace WYNLAB.Api.Services;

/// <summary>
/// 첨부파일 실제 바이트를 어디(로컬 디스크 vs NAS)에 쓸지는 여기 한 곳에서만 안다 - FilesController/
/// FileRepository는 경로 문자열과 StorageType 라벨만 받아서 TSMFILE에 그대로 저장할 뿐이다.
///
/// NAS도 결국 서버 프로세스 입장에서는 UNC 경로(\\nas-server\share\...)일 뿐이라 File.IO
/// 호출 자체는 로컬 디스크와 코드가 완전히 같다(HTTP file distribution과 같은 "경로 문자열
/// 하나로 배포 방식을 바꾼다"는 설계 원칙 - project_wynlab_http_distribution 메모리 참고).
/// 그래서 이 클래스는 로컬/NAS를 분기하는 코드가 따로 없고, appsettings의 FileStorage:BasePath를
/// 어디로 잡느냐(로컬 폴더 경로 or UNC 경로)만 회사별로 다르면 된다. FileStorage:StorageType은
/// TSMFILE.storage_type에 그대로 찍히는 라벨일 뿐, 동작을 바꾸지 않는다.
///
/// 업로드는 청크 방식만 지원한다(2026-09-06 - "청크로 쪼개면 용량 제한 없지 않아?" 지시).
/// 파일 하나를 한 번의 HTTP 요청으로 통째로 받으면 IIS(system.webServer/requestFiltering)와
/// ASP.NET Core(IISServerOptions.MaxRequestBodySize)가 기본으로 약 28.6MB~30MB에서 요청
/// 자체를 끊어버린다 - web.config를 건드리는 대신, 청크 하나(ChunkSizeBytes, FilesController
/// 참고)가 그 기본값보다 훨씬 작게 여러 번 나눠 보내는 쪽을 택했다. 그래서 여기 있는 메서드도
/// "통째로 저장"이 아니라 "이어붙이기(Append) + 완료 시 정식 이름으로 바꾸기" 조합이다.
/// </summary>
public interface IFileStorageService
{
    /// <summary>StorageType 라벨(appsettings FileStorage:StorageType) - TSMFILE.storage_type에 기록용</summary>
    string StorageType { get; }

    /// <summary>doc_type/doc_id/doc_serl별 폴더 아래 "{fileId}_{fileNm}.part" 임시 경로를
    /// 계산한다(폴더가 없으면 만든다) - 아직 바이트를 쓰지 않은 채로 TSMFILE.file_path에
    /// 미리 저장해둘 값이 필요해서 청크 저장과 분리된 메서드로 뺐다.</summary>
    string GetStagingPath(string docType, long docId, int docSerl, long fileId, string fileNm);

    /// <summary>청크 한 조각을 스테이징 파일 끝에 이어붙인다. 클라이언트가 청크를 순서대로
    /// 보내고 응답을 기다린 뒤 다음 청크를 보내는 것을 전제로 한다(동시에 여러 청크를 병렬로
    /// 보내면 순서가 뒤섞일 수 있음 - 이 화면은 그렇게 안 함).</summary>
    Task AppendChunkAsync(string stagingPath, byte[] buffer, int count);

    /// <summary>지금까지 실제로 디스크에 쓰인 바이트 수 - 이어올리기(resume) 판단용. 파일이
    /// 아직 없으면(청크를 하나도 못 받음) 0.</summary>
    long GetStagedBytes(string stagingPath);

    /// <summary>모든 청크를 다 받은 뒤 ".part" 접미사를 떼어 정식 파일로 만들고, 그 최종
    /// 경로와 실제 바이트 수를 돌려준다.</summary>
    (string FinalPath, long FileSize) CompleteStaged(string stagingPath);

    Task<byte[]> ReadAsync(string filePath);

    /// <summary>파일이 이미 없어도(예: 수동 정리) 예외 없이 조용히 넘어간다 - 삭제는 메타데이터
    /// 행 삭제가 우선이고, 물리 파일 정리는 "가능하면" 하는 보조 동작이기 때문. 완료 전
    /// 스테이징(.part) 상태에서 삭제될 수도 있어 두 경로 다 지운다.</summary>
    void TryDelete(string? filePath);
}

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public FileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"]
            ?? throw new InvalidOperationException("FileStorage:BasePath 설정이 없습니다.");
        StorageType = configuration["FileStorage:StorageType"] ?? "LOCAL";
    }

    public string StorageType { get; }

    public string GetStagingPath(string docType, long docId, int docSerl, long fileId, string fileNm)
    {
        var dir = Path.Combine(_basePath, docType, docId.ToString(), docSerl.ToString());
        Directory.CreateDirectory(dir);

        var safeFileNm = string.Join("_", fileNm.Split(Path.GetInvalidFileNameChars()));
        return Path.Combine(dir, $"{fileId}_{safeFileNm}.part");
    }

    public async Task AppendChunkAsync(string stagingPath, byte[] buffer, int count)
    {
        await using var fs = new FileStream(stagingPath, FileMode.Append, FileAccess.Write, FileShare.None);
        await fs.WriteAsync(buffer, 0, count);
    }

    public long GetStagedBytes(string stagingPath) => File.Exists(stagingPath) ? new FileInfo(stagingPath).Length : 0;

    public (string FinalPath, long FileSize) CompleteStaged(string stagingPath)
    {
        var finalPath = stagingPath.EndsWith(".part", StringComparison.OrdinalIgnoreCase)
            ? stagingPath[..^".part".Length]
            : stagingPath;

        File.Move(stagingPath, finalPath, overwrite: true);
        return (finalPath, new FileInfo(finalPath).Length);
    }

    public Task<byte[]> ReadAsync(string filePath) => File.ReadAllBytesAsync(filePath);

    public void TryDelete(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        try
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            var stagingPath = filePath + ".part";
            if (File.Exists(stagingPath)) File.Delete(stagingPath);
        }
        catch { /* 물리 파일 정리는 최선 노력일 뿐 - 실패해도 메타데이터 삭제는 이미 끝난 뒤다 */ }
    }
}
