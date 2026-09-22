using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WYNLAB.Api.Controllers;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Api.Services;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Api.Controllers.Framework;

/// <summary>
/// 파일업로드 공통팝업(popFileUpload) 전용 API - doc_type/doc_id로 호출측(거래처등록 등)을
/// 특정할 뿐, 그 화면의 메뉴권한/PROC_PREFIX에는 얽매이지 않는다(LookupsController와 같은 이유 -
/// 한 메뉴 권한으로는 다른 화면의 doc_type을 못 열게 되므로 [RequireMenuPermission]을 안 건다).
/// 로그인만 되어 있으면 어느 화면에서든 첨부파일을 열고 쓸 수 있다.
/// </summary>
[ApiController]
[Route("api/files")]
[Authorize]
public class FilesController : ControllerBase
{
    /// <summary>청크 하나의 크기 - IIS(system.webServer/requestFiltering)와 ASP.NET Core
    /// (IISServerOptions.MaxRequestBodySize) 기본 요청 크기 제한(약 28.6MB~30MB)보다 한참
    /// 작게 잡아서, web.config를 전혀 안 건드려도 파일이 아무리 커도 요청 하나하나는 항상
    /// 그 제한 밑에 있게 만든다(2026-09-06 지시 - "청크로 쪼개면 용량 제한 없지 않아?").</summary>
    private const int ChunkSizeBytes = 4 * 1024 * 1024; // 4MB

    /// <summary>업로드 가능한 파일 전체 크기 상한 - IIS 제한과는 무관한 별개의 정책값(디스크를
    /// 무한정 채우는 것을 막는 안전장치). 필요해지면 시스템설정 화면으로 옮긴다.</summary>
    private const long MaxFileSizeBytes = 2L * 1024 * 1024 * 1024; // 2GB

    private readonly IFileRepository _repo;
    private readonly IFileStorageService _storage;
    private readonly ISiteConfigRepository _siteConfig;

    public FilesController(IFileRepository repo, IFileStorageService storage, ISiteConfigRepository siteConfig)
    {
        _repo = repo;
        _storage = storage;
        _siteConfig = siteConfig;
    }

    [HttpGet]
    public async Task<ActionResult<List<FileListItemDto>>> Get([FromQuery] string docType, [FromQuery] long docId, [FromQuery] int? docSerl)
    {
        return Ok(await _repo.GetListAsync(docType, docId, docSerl));
    }

    /// <summary>업로드 시작 - 메타데이터만 등록하고(TSMFILE 행 fail_yn='Y'로 "진행중" 상태 생성),
    /// 실제 바이트는 아직 한 바이트도 안 받는다. 클라이언트는 이 응답의 FileId로 이후
    /// {fileId}/chunk를 ChunkSize만큼씩 나눠 순서대로 보낸 뒤 {fileId}/complete를 부른다.</summary>
    [HttpPost("init")]
    public async Task<ActionResult<FileInitUploadResponse>> InitUpload([FromBody] FileInitUploadRequest request)
    {
        if (request.FileSize <= 0)
            return Ok(new FileInitUploadResponse { Success = false, Message = "파일이 비어있습니다." });

        // TSMSITECONFIG에 값이 지정돼 있으면 그걸 우선한다 - 미지정(NULL)이면 위 MaxFileSizeBytes
        // 상수로 폴백(2026-09-06, frmSiteConfig 첨부파일정책 연동).
        var siteConfig = await _siteConfig.GetAsync();
        var maxSizeBytes = siteConfig?.FileMaxSizeMb is int mb && mb > 0 ? (long)mb * 1024 * 1024 : MaxFileSizeBytes;
        if (request.FileSize > maxSizeBytes)
            return Ok(new FileInitUploadResponse { Success = false, Message = $"파일 크기가 너무 큽니다(최대 {maxSizeBytes / 1024 / 1024}MB)." });

        var ext = Path.GetExtension(request.FileNm)?.TrimStart('.').ToLowerInvariant();
        var blocked = siteConfig?.FileBlockExtensions?
            .Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim().TrimStart('.').ToLowerInvariant());
        if (!string.IsNullOrEmpty(ext) && blocked != null && blocked.Contains(ext))
            return Ok(new FileInitUploadResponse { Success = false, Message = $"허용되지 않는 파일 형식입니다(.{ext})." });

        var insertResult = await _repo.InsertAsync(new FileMetaInput
        {
            DocType = request.DocType,
            DocId = request.DocId,
            DocNo = request.DocNo,
            DocSerl = request.DocSerl,
            FileType = request.FileType,
            FileNm = request.FileNm,
            FileSize = request.FileSize,
            MimeType = request.MimeType,
            FilePath = string.Empty,
            StorageType = _storage.StorageType,
            FormId = request.FormId,
            Remark = request.Remark,
            FailYn = "Y", // complete가 성공적으로 끝나기 전까지는 "진행중/미완료"로 취급
        }, CurrentUserId, ClientPcInfo.Build(HttpContext));

        if (!insertResult.IsSuccess || !long.TryParse(insertResult.GeneratedCode, out var fileId))
            return Ok(new FileInitUploadResponse { Success = false, Message = insertResult.FailMessage ?? "업로드 시작에 실패했습니다." });

        // file_id를 알아야 스테이징 파일명("{fileId}_{fileNm}.part")을 정할 수 있어서, insert
        // 직후 한 번 더 갱신한다(같은 이유로 아래 chunk/complete도 UpdateRetryAsync를 재사용함 -
        // "재시도"든 "최초 완료"든 결국 file_path/file_size/fail_yn을 갱신한다는 점은 같다).
        var stagingPath = _storage.GetStagingPath(request.DocType, request.DocId, request.DocSerl, fileId, request.FileNm);
        await _repo.UpdateRetryAsync(fileId, stagingPath, 0, request.MimeType, _storage.StorageType, "Y", CurrentUserId, ClientPcInfo.Build(HttpContext));

        return Ok(new FileInitUploadResponse { Success = true, FileId = fileId, ChunkSize = ChunkSizeBytes });
    }

    /// <summary>이어올리기(resume) 판단용 - 지금까지 서버가 실제로 받아 디스크에 쓴 바이트 수.
    /// 새 업로드든 실패 후 재시도든, 청크를 보내기 전에 항상 먼저 이 값을 확인해서 이미 받은
    /// 만큼은 다시 안 보낸다.</summary>
    [HttpGet("{fileId}/staged-bytes")]
    public async Task<ActionResult<StagedBytesResponse>> GetStagedBytes(long fileId)
    {
        var meta = await _repo.GetByIdAsync(fileId);
        if (meta == null || string.IsNullOrEmpty(meta.FilePath))
            return Ok(new StagedBytesResponse { Bytes = 0 });

        return Ok(new StagedBytesResponse { Bytes = _storage.GetStagedBytes(meta.FilePath) });
    }

    /// <summary>청크 하나를 스테이징 파일 끝에 이어붙인다. 본문을 그대로(멀티파트 아님) 받는다 -
    /// 청크가 수십~수백 번 오갈 수 있어 멀티파트 파싱 오버헤드를 안 지운다.</summary>
    [HttpPost("{fileId}/chunk")]
    public async Task<IActionResult> UploadChunk(long fileId)
    {
        var meta = await _repo.GetByIdAsync(fileId);
        if (meta == null || string.IsNullOrEmpty(meta.FilePath)) return NotFound();

        using var stream = new MemoryStream();
        await Request.Body.CopyToAsync(stream);
        await _storage.AppendChunkAsync(meta.FilePath, stream.GetBuffer(), (int)stream.Length);

        return Ok();
    }

    /// <summary>모든 청크를 다 보낸 뒤 호출 - 스테이징 파일(.part)을 정식 파일로 바꾸고
    /// fail_yn을 'N'으로 돌린다. 이 호출 전까지는 목록에 "실패/진행중"으로 보인다.</summary>
    [HttpPost("{fileId}/complete")]
    public async Task<ActionResult<FileUploadResponse>> CompleteUpload(long fileId)
    {
        var meta = await _repo.GetByIdAsync(fileId);
        if (meta == null || string.IsNullOrEmpty(meta.FilePath))
            return Ok(new FileUploadResponse { Success = false, Message = "업로드 대상을 찾을 수 없습니다." });

        string finalPath;
        long fileSize;
        try
        {
            (finalPath, fileSize) = _storage.CompleteStaged(meta.FilePath);
        }
        catch (Exception ex)
        {
            return Ok(new FileUploadResponse { Success = false, Message = $"파일 완료 처리 중 오류가 발생했습니다.\n{ex.Message}" });
        }

        await _repo.UpdateRetryAsync(fileId, finalPath, fileSize, meta.MimeType, _storage.StorageType, "N", CurrentUserId, ClientPcInfo.Build(HttpContext));

        var saved = await _repo.GetByIdAsync(fileId);
        return Ok(new FileUploadResponse { Success = true, File = saved });
    }

    /// <summary>사용자가 "재시도" 버튼을 눌러 실제로 재업로드를 시작할 때 호출 - retry_cnt만
    /// +1 한다. init/complete가 공유하는 UpdateRetryAsync('U' 분기)는 최초 업로드에서도 매번
    /// 불리기 때문에 거기서 retry_cnt를 올리면 실패한 적 없는 업로드도 retry_cnt가 찍히는
    /// 버그가 있었다(2026-09-06 수정) - 그래서 "진짜 재시도"만 이 별도 엔드포인트로 기록한다.</summary>
    [HttpPost("{fileId}/mark-retry")]
    public async Task<ActionResult<ApiResult>> MarkRetry(long fileId)
    {
        var result = await _repo.MarkRetryAsync(fileId, CurrentUserId, ClientPcInfo.Build(HttpContext));
        return Ok(new ApiResult { Success = result.IsSuccess, Message = result.FailMessage });
    }

    /// <summary>다운로드 - 성공적으로 바이트를 돌려줄 때마다 TSMFILEHIST에 한 줄 남긴다(보안 감사
    /// 목적, 2026-09-06 지시). 파일을 못 찾거나 물리 파일이 없으면 이력도 안 남긴다 - "실제로
    /// 받아간 적"이 아니기 때문.</summary>
    [HttpGet("{fileId}/download")]
    public async Task<IActionResult> Download(long fileId)
    {
        var meta = await _repo.GetByIdAsync(fileId);
        if (meta == null || string.IsNullOrEmpty(meta.FilePath))
            return NotFound();

        byte[] bytes;
        try
        {
            bytes = await _storage.ReadAsync(meta.FilePath);
        }
        catch (IOException)
        {
            return NotFound();
        }

        await _repo.InsertHistAsync(meta.FileId, meta.DocType, meta.DocId, meta.DocNo, meta.FileNm, CurrentUserId, ClientPc, ClientIp);

        var contentType = string.IsNullOrEmpty(meta.MimeType) ? "application/octet-stream" : meta.MimeType;
        return File(bytes, contentType, meta.FileNm ?? $"file_{fileId}");
    }

    [HttpDelete("{fileId}")]
    public async Task<ActionResult<ApiResult>> Delete(long fileId)
    {
        var meta = await _repo.GetByIdAsync(fileId);
        if (meta == null) return Ok(new ApiResult { Success = true }); // 이미 없으면 삭제 목적은 달성된 것

        var result = await _repo.DeleteAsync(fileId, CurrentUserId, ClientPcInfo.Build(HttpContext));
        if (!result.IsSuccess)
            return Ok(new ApiResult { Success = false, Message = result.FailMessage });

        _storage.TryDelete(meta.FilePath);
        return Ok(new ApiResult { Success = true });
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    private string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();

    /// <summary>TSMFILEHIST(다운로드 이력)의 down_pc 컬럼 전용 - 그 테이블은 down_pc/down_ip가
    /// 이미 따로 있는 컬럼이라(InsertHistAsync 참고) PC명만 담는다(IP는 down_ip에 별도로
    /// 넘긴다). 나머지 reg_pc/upt_pc(단일 컬럼)는 전부 ClientPcInfo.Build로 "PC명 | IP"를
    /// 합쳐서 남긴다 - 이 둘을 섞으면 down_pc에 IP가 중복으로 끼어 들어간다.</summary>
    private string? ClientPc
    {
        get
        {
            string? header = HttpContext.Request.Headers["X-Client-Pc"];
            return string.IsNullOrEmpty(header) ? ClientIp : header;
        }
    }
}
