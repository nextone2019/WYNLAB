-- LookUp관리(frmSysLookup)에서 프로시저 없이 단순 쿼리문만으로도 LookUp을 만들 수 있게 확장.
-- source_type='P'(프로시저, 기존 방식 그대로) / 'Q'(쿼리 - query_txt를 그대로 실행). proc_nm은
-- source_type='Q'일 때는 안 쓰므로 NULL을 허용하도록 완화한다.

ALTER TABLE sysLookupM ADD source_type CHAR(1) NOT NULL CONSTRAINT DF_sysLookupM_SourceType DEFAULT 'P';
ALTER TABLE sysLookupM ADD query_txt NVARCHAR(MAX) NULL;
GO

ALTER TABLE sysLookupM ALTER COLUMN proc_nm VARCHAR(100) NULL;
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30) = NULL,   /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'일 때는 정확히 일치하는 LookUp키 */
    @p_lookup_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT lookup_key, source_type, proc_nm, query_txt, lookup_nm, value_field, display_field, use_yn, remark
            FROM sysLookupM
            WHERE (@p_lookup_key IS NULL OR lookup_key LIKE '%' + @p_lookup_key + '%')
              AND (@p_lookup_nm IS NULL OR lookup_nm LIKE '%' + @p_lookup_nm + '%')
            ORDER BY lookup_key;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT lookup_key, param_nm, caption, sort
            FROM sysLookupP
            WHERE lookup_key = @p_lookup_key
            ORDER BY sort;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_source_type CHAR(1) = 'P',
    @p_proc_nm VARCHAR(100) = NULL,
    @p_query_txt NVARCHAR(MAX) = NULL,
    @p_lookup_nm NVARCHAR(100) = NULL,
    @p_value_field VARCHAR(50) = NULL,
    @p_display_field VARCHAR(50) = NULL,
    @p_use_yn VARCHAR(1) = 'Y',
    @p_remark NVARCHAR(200) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysLookupM (
                lookup_key, source_type, proc_nm, query_txt, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt
            )
            VALUES (
                @p_lookup_key, @p_source_type, @p_proc_nm, @p_query_txt, @p_lookup_nm, @p_value_field, @p_display_field, @p_use_yn, @p_remark, @p_user_id, GETDATE()
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupM SET
                source_type = @p_source_type, proc_nm = @p_proc_nm, query_txt = @p_query_txt,
                lookup_nm = @p_lookup_nm, value_field = @p_value_field,
                display_field = @p_display_field, use_yn = @p_use_yn, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHERE lookup_key = @p_lookup_key;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupP WHERE lookup_key = @p_lookup_key;
            DELETE FROM sysLookupM WHERE lookup_key = @p_lookup_key;
        END

        SET @GeneratedCode = @p_lookup_key;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
