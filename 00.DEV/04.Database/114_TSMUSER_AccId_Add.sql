-- TSMUSER에 ACC_ID(TBAACC 참조, 사업장)를 추가한다 - BA 모듈 저장프로시저들(USP_BA_DEPT_S 등)에
-- 남아있는 "로그인 세션에 사업장 생기면 서버가 채우도록 전환" TODO가 기다리던 값이다.
-- 세션 조회(SSP_WYNLAB_GetSession)와 사용자관리 화면(USP_SM_USERAUTH_Q/S)에 같이 반영한다.

ALTER TABLE TSMUSER ADD ACC_ID BIGINT NULL;
GO

CREATE OR ALTER PROCEDURE SSP_WYNLAB_GetSession
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
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
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                U.USER_ID       AS UserId,
                U.USER_NM       AS UserNm,
                E.emp_no        AS EmpNo,
                D.DEPT_NM       AS DeptNm,
                U.PASSWORD_HASH AS PasswordHash,
                U.EMAIL         AS Email,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn,
                U.ACC_ID        AS AccId,
                A.ACC_NM        AS AccNm
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            LEFT JOIN TBAACC A ON A.ACC_ID = U.ACC_ID
            WHERE U.USER_ID = @p_user_id;

            SELECT USER_GRP_CD
            FROM TSMUSERGRPMAP
            WHERE USER_ID = @p_user_id;
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

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_user_nm NVARCHAR(50) = NULL,
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
        IF @p_work_type = 'Q'
        BEGIN
            SELECT  U.USER_ID AS UserId,
                        U.USER_NM AS UserNm,
                        U.EMP_ID AS EmpId,
                        E.emp_no AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        D.DEPT_NM AS DeptNm,
                        U.ACC_ID AS AccId,
                        A.ACC_NM AS AccNm,
                        U.EMAIL AS Email,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                        LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
                        LEFT JOIN TBAACC A ON A.ACC_ID = U.ACC_ID
            WHERE (@p_user_id IS NULL OR U.USER_ID LIKE '%' + @p_user_id + '%')
              AND (@p_user_nm IS NULL OR U.USER_NM LIKE '%' + @p_user_nm + '%')
            ORDER BY U.REG_DT DESC;
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

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
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
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_ID, ACC_ID, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, MUST_CHANGE_PWD_YN, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_id, @p_acc_id, @p_email, @p_user_type, 'N', 'Y', 'N', 0, @p_must_change_pwd_yn, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_ID = @p_emp_id, ACC_ID = @p_acc_id, EMAIL = @p_email, USE_YN = @p_use_yn, USER_TYPE = @p_user_type,
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
