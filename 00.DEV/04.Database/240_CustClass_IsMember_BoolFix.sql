-- USP_BA_CUST_S_3 수정 - 화면에서 실제로 넘어오는 @p_is_member 값이 "1"/"0"이 아니라
-- "True"/"False"였다(그리드 체크박스 컬럼이 is_member를 bool로 바인딩해서 ProcData.Str이
-- Convert.ToString(bool)의 결과인 "True"/"False"를 그대로 보냄 - 2026-09-30 실제 발견,
-- EXEC 캡처로 확인: "@p_is_member = N'False'"). 두 표현 다 받아들이게 방어적으로 고친다.
CREATE OR ALTER PROCEDURE USP_BA_CUST_S_3
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_class_cd VARCHAR(20) = NULL,
    @p_is_member VARCHAR(10) = NULL,
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
        IF @p_work_type = 'U'
        BEGIN
            IF @p_is_member IN ('1', 'True', 'true')
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM TBACUSTCLASS WHERE CUST_ID = @p_cust_id AND class_cd = @p_class_cd)
                    INSERT INTO TBACUSTCLASS (CUST_ID, class_cd, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                    VALUES (@p_cust_id, @p_class_cd, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            END
            ELSE
                DELETE FROM TBACUSTCLASS WHERE CUST_ID = @p_cust_id AND class_cd = @p_class_cd;
        END

        SET @GeneratedCode = @p_class_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
