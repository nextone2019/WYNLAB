using System.Data;
using Dapper;
using WYNLAB.Api.Data;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Repositories.Framework;

/// <summary>
/// TSMFILE/TSMFILEHIST 메타데이터 CRUD. 실제 파일 바이트 저장/삭제는 FileStorageService가
/// 하고, 여기는 USP_SM_FILE_*/USP_SM_FILEHIST_S 호출만 담당한다(GenericDataRepository와 달리
/// 여러 화면이 공유하는 프레임워크 기능이라 전용 Repository/Controller를 둔다 - Popup/Lookup
/// 프레임워크와 같은 이유, project_wynlab_popup_lookup_framework 참고).
/// </summary>
public interface IFileRepository
{
    Task<List<FileListItemDto>> GetListAsync(string docType, long docId, int? docSerl);

    /// <summary>신규 등록. 성공하면 GeneratedCode에 새 file_id가 담겨 돌아온다.</summary>
    Task<ProcResult> InsertAsync(FileMetaInput meta, string userId, string? clientPc);

    /// <summary>스테이징 경로 저장(init 직후)/완료 처리(complete) 공용 메타데이터 갱신 - retry_cnt는
    /// 안 건드린다(진짜 재시도 여부는 MarkRetryAsync가 따로 기록함).</summary>
    Task<ProcResult> UpdateRetryAsync(long fileId, string filePath, long fileSize, string? mimeType, string storageType, string failYn, string userId, string? clientPc);

    /// <summary>사용자가 "재시도" 버튼을 실제로 눌렀을 때만 호출 - retry_cnt만 +1 한다.</summary>
    Task<ProcResult> MarkRetryAsync(long fileId, string userId, string? clientPc);

    Task<ProcResult> DeleteAsync(long fileId, string userId, string? clientPc);

    Task<FileListItemDto?> GetByIdAsync(long fileId);

    Task<ProcResult> InsertHistAsync(long fileId, string docType, long docId, string docNo, string? fileNm, string downUserId, string? downPc, string? downIp);
}

/// <summary>USP_SM_FILE_S 'N'(신규)/'U'(재시도) 호출에 필요한 필드 묶음 - 두 경로가 대부분의
/// 컬럼을 공유해서 파라미터 나열을 반복하지 않기 위한 입력 모델.</summary>
public class FileMetaInput
{
    public string DocType { get; set; } = string.Empty;
    public long DocId { get; set; }
    public string DocNo { get; set; } = string.Empty;
    public int DocSerl { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string FileNm { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? MimeType { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string StorageType { get; set; } = string.Empty;
    public string? FormId { get; set; }
    public string? Remark { get; set; }
    public string FailYn { get; set; } = "N";
}

public class FileRepository : IFileRepository
{
    private readonly IDapperContext _context;

    public FileRepository(IDapperContext context) => _context = context;

    public async Task<List<FileListItemDto>> GetListAsync(string docType, long docId, int? docSerl)
    {
        using var conn = _context.CreateConnection();
        var items = await conn.QueryAsync<FileListItemDto>("USP_SM_FILE_Q",
            new { p_work_type = "Q", p_doc_type = docType, p_doc_id = docId, p_doc_serl = docSerl },
            commandType: CommandType.StoredProcedure);
        return items.ToList();
    }

    public async Task<FileListItemDto?> GetByIdAsync(long fileId)
    {
        // 단건 조회 전용 프로시저를 따로 두지 않고, doc_type/doc_id 없이도 걸리게 doc_type='%'로
        // 필터를 건너뛸 수는 없으므로(프로시저가 doc_type/doc_id를 필수로 매칭) 여기서는 직접
        // TSMFILE을 한 번 더 읽는다 - 다운로드/삭제/재시도 전에 "이 file_id가 실제로 있는지 +
        // 물리 경로가 뭔지"를 확인하는 용도라 프로시저 화이트리스트 정책과는 무관한 내부 조회다.
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<FileListItemDto>(
            @"SELECT f.file_id AS FileId, f.doc_type AS DocType, f.doc_id AS DocId, f.doc_no AS DocNo,
                     f.doc_serl AS DocSerl, f.serl AS Serl, f.file_type AS FileType, f.file_nm AS FileNm,
                     f.file_size AS FileSize, f.mime_type AS MimeType, f.file_path AS FilePath,
                     f.storage_type AS StorageType, f.form_id AS FormId, f.remark AS Remark,
                     f.reg_user_id AS RegUserId, f.fail_yn AS FailYn, f.retry_cnt AS RetryCnt
              FROM TSMFILE f WHERE f.file_id = @fileId",
            new { fileId });
    }

    public async Task<ProcResult> InsertAsync(FileMetaInput meta, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_doc_type", meta.DocType);
        p.Add("p_doc_id", meta.DocId);
        p.Add("p_doc_no", meta.DocNo);
        p.Add("p_doc_serl", meta.DocSerl);
        p.Add("p_file_type", meta.FileType);
        p.Add("p_file_nm", meta.FileNm);
        p.Add("p_file_size", meta.FileSize);
        p.Add("p_mime_type", meta.MimeType);
        p.Add("p_file_path", meta.FilePath);
        p.Add("p_storage_type", meta.StorageType);
        p.Add("p_form_id", meta.FormId);
        p.Add("p_remark", meta.Remark);
        p.Add("p_fail_yn", meta.FailYn);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FILE_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> UpdateRetryAsync(long fileId, string filePath, long fileSize, string? mimeType, string storageType, string failYn, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "U");
        p.Add("p_file_id", fileId);
        p.Add("p_file_path", filePath);
        p.Add("p_file_size", fileSize);
        p.Add("p_mime_type", mimeType);
        p.Add("p_storage_type", storageType);
        p.Add("p_fail_yn", failYn);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FILE_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> MarkRetryAsync(long fileId, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "R");
        p.Add("p_file_id", fileId);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FILE_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> DeleteAsync(long fileId, string userId, string? clientPc)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "D");
        p.Add("p_file_id", fileId);
        p.Add("p_user_id", userId);
        p.Add("p_client_pc", clientPc);
        p.AddStandardOutputs(withGeneratedCode: true, pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FILE_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(withGeneratedCode: true, pascalCase: true);
    }

    public async Task<ProcResult> InsertHistAsync(long fileId, string docType, long docId, string docNo, string? fileNm, string downUserId, string? downPc, string? downIp)
    {
        using var conn = _context.CreateConnection();
        var p = new DynamicParameters();
        p.Add("p_work_type", "N");
        p.Add("p_file_id", fileId);
        p.Add("p_doc_type", docType);
        p.Add("p_doc_id", docId);
        p.Add("p_doc_no", docNo);
        p.Add("p_file_nm", fileNm);
        p.Add("p_down_user_id", downUserId);
        p.Add("p_down_pc", downPc);
        p.Add("p_down_ip", downIp);
        p.AddStandardOutputs(pascalCase: true);

        await conn.ExecuteAsync("USP_SM_FILEHIST_S", p, commandType: CommandType.StoredProcedure);
        return p.ReadStandardOutputs(pascalCase: true);
    }
}
