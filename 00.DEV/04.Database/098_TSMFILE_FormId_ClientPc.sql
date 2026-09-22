/* ---------- TSMFILE/TSMFILEHIST: form_id 기록 + reg_pc/upt_pc를 실제 PC명으로 ----------
   1) reg_pc/upt_pc가 로컬 테스트에서 전부 "::1"(루프백)로만 찍혀서 무의미해 보인다는 지적
      (2026-09-06) - 지금까지 이 값은 서버가 HttpContext.Connection.RemoteIpAddress로 채워왔는데,
      클라이언트/서버가 같은 PC면 항상 루프백 주소만 보인다(다른 PC끼리는 실제 IP가 찍히니
      틀린 동작은 아니었지만, "PC"라는 컬럼명에는 IP보다 컴퓨터 이름이 더 맞는다). 이제
      WinForms 클라이언트가 자기 Environment.MachineName을 X-Client-Pc 헤더로 보내고,
      FilesController가 그 값을 우선 쓰도록 바꿨다(코드 쪽 변경, 이 마이그레이션은 그걸 받을
      파라미터만 추가).
   2) form_id는 지금까지 아무도 안 채워서 항상 NULL이었다 - 어느 화면에서 업로드했는지
      남기기 위해 USP_SM_FILE_S에 @p_form_id를 추가한다(frmFileUpload가 호출한 화면의 클래스명을
      자동으로 채움).
   3) TSMFILEHIST.down_pc도 마찬가지로 지금까지 비어있었다 - down_ip(실제 IP)는 그대로 두고
      down_pc(컴퓨터 이름)도 같이 남기도록 USP_SM_FILEHIST_S에 @p_down_pc를 추가한다. */

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
    @p_form_id varchar(50) = NULL,
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
                file_path, storage_type, form_id, remark, fail_yn, retry_cnt, reg_user_id, reg_dt, reg_pc)
            VALUES (
                @p_doc_type, @p_doc_id, @p_doc_no, @p_doc_serl, @nextSerl, @p_file_type, @p_file_nm, @p_file_size, @p_mime_type,
                @p_file_path, @p_storage_type, @p_form_id, @p_remark, @p_fail_yn, 0, @p_user_id, GETDATE(), @p_client_pc);

            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS varchar(50));
        END
        ELSE IF @p_work_type = 'U' -- 스테이징 경로 저장(init 직후) / 완료 처리(complete) 공용 -
                                    -- retry_cnt는 여기서 안 건드린다('R' 참고)
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

CREATE OR ALTER PROCEDURE USP_SM_FILEHIST_S
    @p_work_type varchar(10),
    @p_file_id bigint,
    @p_doc_type varchar(10),
    @p_doc_id bigint,
    @p_doc_no varchar(100),
    @p_file_nm nvarchar(200) = NULL,
    @p_down_user_id varchar(30),
    @p_down_pc nvarchar(200) = NULL,
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
            INSERT INTO TSMFILEHIST (file_id, doc_type, doc_id, doc_no, file_nm, down_user_id, down_dt, down_pc, down_ip)
            VALUES (@p_file_id, @p_doc_type, @p_doc_id, @p_doc_no, @p_file_nm, @p_down_user_id, GETDATE(), @p_down_pc, @p_down_ip);
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
