/* ---------- USP_SM_USERAUTH_S: 신규 계정 생성 시 TSMSITECONFIG의 비밀번호정책
   (init_pwd_policy/force_change_on_first_login)을 반영할 수 있도록 must_change_pwd_yn
   파라미터 추가. 실제 초기 비밀번호 값 자체는(아이디와 동일 / 랜덤) 서버(UsersController)가
   결정해서 그냥 문자열로 넘겨주므로 여기선 해시만 그대로 저장 - 정책 판단 로직을 SQL에
   넣지 않는다. 기존 라이브 정의(2026-09-06, panData UserType 바인딩 문제 수정본)에서
   must_change_pwd_yn 인서트 한 줄만 추가한다. */

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_id BIGINT = NULL,
    @p_email NVARCHAR(200) = NULL,
    @p_use_yn CHAR(1) = 'Y',
    @p_user_type VARCHAR(10) = 'U',
    @p_must_change_pwd_yn CHAR(1) = 'N',
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
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_ID, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, MUST_CHANGE_PWD_YN, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_id, @p_email, @p_user_type, 'N', 'Y', 'N', 0, @p_must_change_pwd_yn, GETDATE(), GETDATE());
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
