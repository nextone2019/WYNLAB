/* ---------- USP_SM_FILE_S: retry_cnt 오증가 버그 수정 ----------
   청크 업로드로 바꾸면서 'U'(work_type) 분기를 "재시도 갱신" 용도 하나로만 안 쓰고, init 직후
   스테이징 경로 저장 + complete 시 최종 경로/크기 반영까지 전부 재사용했다 - 그런데 'U' 분기가
   무조건 retry_cnt를 +1 해서, 실패 한 번 없이 성공한 최초 업로드도 retry_cnt가 2(스테이징
   저장 1회 + complete 1회)로 찍히는 버그가 있었다(2026-09-06 발견). retry_cnt 증가는 'U'에서
   떼어내 새 분기 'R'로 분리하고, 실제로 사용자가 "재시도" 버튼을 눌렀을 때만(FilesController의
   /mark-retry) 호출한다. */

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
        ELSE IF @p_work_type = 'U' -- 스테이징 경로 저장(init 직후) / 완료 처리(complete) 공용 -
                                    -- retry_cnt는 여기서 안 건드린다(아래 'R' 참고)
        BEGIN
            UPDATE TSMFILE
            SET file_path = @p_file_path,
                file_size = @p_file_size,
                mime_type = @p_mime_type,
                storage_type = @p_storage_type,
                fail_yn = @p_fail_yn,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE file_id = @p_file_id;
        END
        ELSE IF @p_work_type = 'R' -- 재시도 버튼을 실제로 눌렀을 때만(FilesController /mark-retry)
        BEGIN
            UPDATE TSMFILE
            SET retry_cnt = ISNULL(retry_cnt, 0) + 1,
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
