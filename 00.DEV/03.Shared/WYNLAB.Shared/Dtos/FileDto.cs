namespace WYNLAB.Shared.Dtos;

/// <summary>
/// TSMFILE 한 행 - popFileUpload(공통 첨부파일 팝업)의 grd1과 호출측 화면(frmCust의 grdFile 등)
/// 둘 다 이 DTO를 그대로 그리드에 바인딩한다. Query(USP_SM_FILE_Q)는 컬럼을 이미 이 이름
/// (PascalCase)으로 별칭 지어 내려주므로 Dapper가 별도 매핑 없이 그대로 채운다.
/// </summary>
public class FileListItemDto
{
    public long FileId { get; set; }
    public string DocType { get; set; } = string.Empty;
    public long DocId { get; set; }
    public string DocNo { get; set; } = string.Empty;
    public int DocSerl { get; set; }
    public int Serl { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string? FileNm { get; set; }
    public long? FileSize { get; set; }
    public string? MimeType { get; set; }
    public string? FilePath { get; set; }
    public string? StorageType { get; set; }
    public string? FormId { get; set; }
    public string? Remark { get; set; }
    public string? RegUserId { get; set; }
    public string? RegUserNm { get; set; }
    public string? FailYn { get; set; }
    public int? RetryCnt { get; set; }

    /// <summary>서버는 안 채운다 - popFileUpload 그리드의 체크박스 컬럼(다중선택) 상태를 담는
    /// 클라이언트 전용 필드. 조회할 때마다 새로 받는 객체라 항상 false로 시작한다.</summary>
    public bool IsChecked { get; set; }
}

/// <summary>업로드 완료 응답 - ApiResult(Success/Message)와 같은 모양에 결과 행(File)만 얹었다.
/// ApiClient.PostAsync는 성공/실패 상태코드와 무관하게 본문을 그대로 역직렬화하므로, 실패 응답도
/// 항상 이 모양으로 돌아와야 화면에서 Success/Message를 읽을 수 있다.</summary>
public class FileUploadResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public FileListItemDto? File { get; set; }
}

/// <summary>청크 업로드 시작(POST api/files/init) 요청 - 실제 파일 바이트는 안 담는다(이후
/// api/files/{fileId}/chunk로 나눠 보냄). IIS/ASP.NET Core 기본 요청 크기 제한(약 28.6MB~30MB)에
/// 걸리지 않으려고 업로드를 "메타데이터 등록 -> 청크 여러 개 -> 완료" 세 단계로 나눈 것이
///  design 배경(2026-09-06 지시 - "청크로 쪼개면 용량 제한 없지 않아?").</summary>
public class FileInitUploadRequest
{
    public string DocType { get; set; } = string.Empty;
    public long DocId { get; set; }
    public string DocNo { get; set; } = string.Empty;
    public int DocSerl { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public string FileNm { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? MimeType { get; set; }

    /// <summary>어느 화면에서 올렸는지(예: "frmCust") - popFileUpload.ShowAsync가 호출측 폼의
    /// 타입 이름으로 자동으로 채운다(2026-09-06 요청). TSMFILE.form_id에 그대로 저장된다.</summary>
    public string? FormId { get; set; }
}

public class FileInitUploadResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public long FileId { get; set; }

    /// <summary>서버가 정한 청크 크기(바이트) - 클라이언트가 이 크기로 파일을 나눠 보낸다.
    /// 서버가 정하는 이유: IIS 요청 크기 제한과 안 부딪히는 값을 서버 설정 한 곳에서만
    /// 관리하기 위함(클라이언트마다 값이 다르면 나중에 제한을 조정할 때 다 같이 바꿔야 함).</summary>
    public int ChunkSize { get; set; }
}

/// <summary>이어올리기(resume)용 - 이 파일에 대해 서버가 지금까지 실제로 받아 디스크에 쓴
/// 바이트 수. 신규 업로드든 재시도든 청크를 보내기 전에 먼저 이 값을 확인해서, 이미 받은
/// 만큼은 다시 안 보내고 그 이후부터 이어 보낸다.</summary>
public class StagedBytesResponse
{
    public long Bytes { get; set; }
}
