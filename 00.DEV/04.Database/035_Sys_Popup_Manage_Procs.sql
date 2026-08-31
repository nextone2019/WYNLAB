-- 팝업관리 화면(frmSysPopup, SYS 모듈)용 CRUD 프로시저. sysPopUpM(마스터)/sysPopUpD(컬럼설정)
-- 둘 다 이 화면에서만 다루므로 USP_SM_MINORCODE_Q/_S/_S_1과 같은 마스터+상세 구조를 그대로
-- 따른다 - work_type Q(마스터 목록)/Q1(상세 목록)/N/U/D, 표준 5개 출력.

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_POPUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30) = NULL,    /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'일 때는 정확히 일치하는 팝업키 */
    @p_popup_nm NVARCHAR(100) = NULL,
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
            SELECT popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                   display_field, popup_width, popup_height, use_yn, remark
            FROM sysPopUpM
            WHERE (@p_popup_key IS NULL OR popup_key LIKE '%' + @p_popup_key + '%')
              AND (@p_popup_nm IS NULL OR popup_nm LIKE '%' + @p_popup_nm + '%')
            ORDER BY popup_key;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn
            FROM sysPopUpD
            WHERE popup_key = @p_popup_key
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

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_POPUP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_proc_nm VARCHAR(100) = NULL,
    @p_popup_nm NVARCHAR(100) = NULL,
    @p_hierarchical_yn VARCHAR(1) = 'N',
    @p_key_field VARCHAR(50) = NULL,
    @p_parent_field VARCHAR(50) = NULL,
    @p_display_field VARCHAR(50) = NULL,
    @p_popup_width INT = 700,
    @p_popup_height INT = 500,
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
            INSERT INTO sysPopUpM (
                popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                display_field, popup_width, popup_height, use_yn, remark, reg_user_id, reg_dt
            )
            VALUES (
                @p_popup_key, @p_proc_nm, @p_popup_nm, @p_hierarchical_yn, @p_key_field, @p_parent_field,
                @p_display_field, @p_popup_width, @p_popup_height, @p_use_yn, @p_remark, @p_user_id, GETDATE()
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpM SET
                proc_nm = @p_proc_nm, popup_nm = @p_popup_nm, hierarchical_yn = @p_hierarchical_yn,
                key_field = @p_key_field, parent_field = @p_parent_field, display_field = @p_display_field,
                popup_width = @p_popup_width, popup_height = @p_popup_height, use_yn = @p_use_yn,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHERE popup_key = @p_popup_key;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpD WHERE popup_key = @p_popup_key;
            DELETE FROM sysPopUpM WHERE popup_key = @p_popup_key;
        END

        SET @GeneratedCode = @p_popup_key;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 컬럼 설정 그리드(grd2)의 행 하나 - frmSysPopup.cs가 Added/Modified/Deleted 행마다 이걸 한
-- 번씩 부른다(USP_SM_MINORCODE_S_1과 같은 구조). column_nm이 키라 'U' 분기도 WHERE에서만 쓴다.
CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_POPUP_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_column_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_control_type VARCHAR(10) = 'TEXT',
    @p_lookup_proc_nm VARCHAR(100) = NULL,
    @p_sort INT = 0,
    @p_width INT = 100,
    @p_visible_yn VARCHAR(1) = 'Y',
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
            INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
            VALUES (@p_popup_key, @p_column_nm, @p_caption, @p_control_type, @p_lookup_proc_nm, @p_sort, @p_width, @p_visible_yn);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpD SET
                caption = @p_caption, control_type = @p_control_type, lookup_proc_nm = @p_lookup_proc_nm,
                sort = @p_sort, width = @p_width, visible_yn = @p_visible_yn
            WHERE popup_key = @p_popup_key AND column_nm = @p_column_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpD WHERE popup_key = @p_popup_key AND column_nm = @p_column_nm;
        END

        SET @GeneratedCode = @p_column_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
