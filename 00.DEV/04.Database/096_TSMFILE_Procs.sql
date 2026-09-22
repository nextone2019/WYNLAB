/* ---------- TSMFILE/TSMFILEHIST 프로시저: 파일업로드 공통팝업(frmFileUpload) ----------
   USP_SM_FILE_Q(Q) - doc_type+doc_id(+doc_serl)로 첨부파일 목록 조회. TSMUSER를 조인해서
   등록자명(RegUserNm)까지 같이 내려준다(그리드에 RegUserNm 컬럼이 이미 있음).
   USP_SM_FILE_S - N(신규 등록)/U(재시도 후 갱신)/D(삭제). 실제 파일 바이트를 디스크(또는
   NAS UNC 경로)에 쓰고 지우는 일은 서버(C# FileStorageService)가 하고, 이 프로시저는
   TSMFILE 메타데이터 행만 다룬다.
   USP_SM_FILEHIST_S - N(다운로드 이력 한 줄 적재)만 지원 - 성공한 다운로드마다 호출된다. */

CREATE OR ALTER PROCEDURE USP_SM_FILE_Q
    @p_work_type varchar(10),
    @p_doc_type varchar(10) = NULL,
    @p_doc_id bigint = NULL,
    @p_doc_serl int = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_work_type = 'Q'
    BEGIN
        SELECT
            f.file_id AS FileId,
            f.doc_type AS DocType,
            f.doc_id AS DocId,
            f.doc_no AS DocNo,
            f.doc_serl AS DocSerl,
            f.serl AS Serl,
            f.file_type AS FileType,
            f.file_nm AS FileNm,
            f.file_size AS FileSize,
            f.mime_type AS MimeType,
            f.file_path AS FilePath,
            f.storage_type AS StorageType,
            f.form_id AS FormId,
            f.remark AS Remark,
            f.reg_user_id AS RegUserId,
            u.USER_NM AS RegUserNm,
            f.fail_yn AS FailYn,
            f.retry_cnt AS RetryCnt
        FROM TSMFILE f
        LEFT JOIN TSMUSER u ON u.USER_ID = f.reg_user_id
        WHERE f.doc_type = @p_doc_type
          AND f.doc_id = @p_doc_id
          AND (@p_doc_serl IS NULL OR f.doc_serl = @p_doc_serl)
        ORDER BY f.serl;
    END
END
GO

CREATE OR ALTER PROCEDURE USP_SM_FILE_S
    @p_work_type varchar(10),
    @p_file_id bigint = NULL,
    @p_doc_type varchar(10) = NULL,
    @p_doc_id bigint = NULL,
    @p_doc_no varchar(100) = NULL,
    @p_doc_serl int = NULL,
    @p_file_type varchar(10) = NULL,
    @p_file_nm nvarchar(200) = NULL,
    @p_file_size bigint = NULL,
    @p_mime_type varchar(100) = NULL,
    @p_file_path nvarchar(200) = NULL,
    @p_storage_type varchar(10) = NULL,
    @p_remark nvarchar(100) = NULL,
    @p_fail_yn varchar(1) = NULL,
    @p_user_id varchar(30),
    @p_client_pc nvarchar(200) = NULL,
    @GeneratedCode varchar(50) OUTPUT,
    @ReturnCode int OUTPUT,
    @ReturnMsg varchar(200) OUTPUT,
    @ErrorCode int OUTPUT,
    @ErrorMsg varchar(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = NULL;
    SET @ErrorCode = 0; SET @ErrorMsg = NULL;
    SET @GeneratedCode = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl int = ISNULL(
                (SELECT MAX(serl) FROM TSMFILE WHERE doc_type = @p_doc_type AND doc_id = @p_doc_id AND doc_serl = @p_doc_serl),
                0) + 1;

            INSERT INTO TSMFILE (
                doc_type, doc_id, doc_no, doc_serl, serl, file_type, file_nm, file_size, mime_type,
                file_path, storage_type, remark, fail_yn, retry_cnt, reg_user_id, reg_dt, reg_pc)
            VALUES (
                @p_doc_type, @p_doc_id, @p_doc_no, @p_doc_serl, @nextSerl, @p_file_type, @p_file_nm, @p_file_size, @p_mime_type,
                @p_file_path, @p_storage_type, @p_remark, @p_fail_yn, 0, @p_user_id, GETDATE(), @p_client_pc);

            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS varchar(50));
        END
        ELSE IF @p_work_type = 'U' -- 재시도: 물리 파일을 다시 쓴 뒤 메타데이터 갱신
        BEGIN
            UPDATE TSMFILE
            SET file_path = @p_file_path,
                file_size = @p_file_size,
                mime_type = @p_mime_type,
                storage_type = @p_storage_type,
                fail_yn = @p_fail_yn,
                retry_cnt = ISNULL(retry_cnt, 0) + 1,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE file_id = @p_file_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMFILE WHERE file_id = @p_file_id;
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_SM_FILEHIST_S
    @p_work_type varchar(10),
    @p_file_id bigint,
    @p_doc_type varchar(10),
    @p_doc_id bigint,
    @p_doc_no varchar(100),
    @p_file_nm nvarchar(200) = NULL,
    @p_down_user_id varchar(30),
    @p_down_ip varchar(50) = NULL,
    @ReturnCode int OUTPUT,
    @ReturnMsg varchar(200) OUTPUT,
    @ErrorCode int OUTPUT,
    @ErrorMsg varchar(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = NULL;
    SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMFILEHIST (file_id, doc_type, doc_id, doc_no, file_nm, down_user_id, down_dt, down_ip)
            VALUES (@p_file_id, @p_doc_type, @p_doc_id, @p_doc_no, @p_file_nm, @p_down_user_id, GETDATE(), @p_down_ip);
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
