/* ---------- USP_SM_USERAUTH_S: panData의 cboUserType(L_SM0002)이 화면에 있는데도 저장 파라미터가
   아예 없어서 신규는 항상 하드코딩된 'U'로만 생성되고 수정 때도 절대 안 바뀌던 문제 수정.
   (2026-09-06 실제 발견 - "panData에 UserType이 바인딩이 되지 않아") ---------- */

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_id BIGINT = NULL,
    @p_email NVARCHAR(200) = NULL,
    @p_use_yn CHAR(1) = 'Y',
    @p_user_type VARCHAR(10) = 'U',
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMUSER
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_ID, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_id, @p_email, @p_user_type, 'N', 'Y', 'N', 0, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_ID = @p_emp_id, EMAIL = @p_email, USE_YN = @p_use_yn, USER_TYPE = @p_user_type,
                UPT_DT = GETDATE()
            WHERE USER_ID = @p_user_id;
        END

        SET @GeneratedCode = @p_user_id;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
