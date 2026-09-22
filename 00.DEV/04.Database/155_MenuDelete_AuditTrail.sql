-- 메뉴관리(frmMenu) 삭제(=소프트삭제, USE_YN='N') 시 upt_user_id/upt_dt가 안 남던 것 수정
-- (2026-09-16 - 삭제가 실제로 동작하는지 확인하다가 발견). MenusController.Delete ->
-- MenuManageRepository.SetUseYnAsync -> USP_SM_MENU_S_1이 지금까지 @p_menu_id/@p_use_yn만
-- 받고 누가/언제 바꿨는지는 전혀 안 남기고 있었다 - 다른 화면들의 표준 감사컬럼 관례와 다르다.

CREATE OR ALTER PROCEDURE USP_SM_MENU_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_use_yn CHAR(1),
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'D'
        BEGIN
            UPDATE TSMMENU SET
                USE_YN = @p_use_yn,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE MENU_ID = @p_menu_id;
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
