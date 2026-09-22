/* ---------- TSMUSER-TBAEMP 조인을 EMP_NO 대신 EMP_ID로 교체 ----------
   USP_SM_USERAUTH_S가 저장(등록/수정) 시 @p_emp_no로 TBAEMP를 찾아 EMP_ID를 같이 채운다 -
   화면(frmUserAuth)은 여전히 사번만 입력하면 되고 하나도 안 고쳐도 된다. 나머지 4개 조회
   프로시저는 조인 조건만 E.EMP_NO=U.EMP_NO -> E.EMP_ID=U.EMP_ID로 바꾼다(출력 컬럼은 전부
   그대로라 영향 없음). */

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_email NVARCHAR(200) = NULL,
    @p_use_yn CHAR(1) = 'Y',
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
        DECLARE @emp_id BIGINT = (SELECT EMP_ID FROM TBAEMP WHERE emp_no = @p_emp_no);

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMUSER
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, EMP_ID, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_no, @emp_id, @p_email, 'U', 'N', 'Y', 'N', 0, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_NO = @p_emp_no, EMP_ID = @emp_id, EMAIL = @p_email, USE_YN = @p_use_yn,
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
                U.EMP_NO        AS EmpNo,
                D.DEPT_NM       AS DeptNm,
                U.PASSWORD_HASH AS PasswordHash,
                U.EMAIL         AS Email,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
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
                        U.EMP_NO AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        D.DEPT_NM AS DeptNm,
                        U.EMAIL AS Email,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                        LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
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

CREATE OR ALTER PROCEDURE USP_SM_USERGRP_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
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
        IF @p_work_type = 'Q'
        BEGIN
            SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, D.DEPT_NM AS DeptNm,
                   CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            LEFT JOIN TSMUSERGRPMAP M ON M.USER_ID = U.USER_ID AND M.USER_GRP_CD = @p_user_grp_cd
            WHERE U.USE_YN = 'Y'
            ORDER BY U.USER_NM;
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

CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_target_type VARCHAR(10),
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
        IF @p_work_type = 'Q'
        BEGIN
            IF @p_target_type = 'USER'
            BEGIN
                SELECT U.USER_ID AS TargetCd, U.USER_NM AS TargetNm, D.DEPT_NM AS SubNm,
                       ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                       ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                       ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                       ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                       ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                       ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                       ISNULL(A.AUTH10, 'N') AS Auth10
                FROM TSMUSER U
                LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
                LEFT JOIN TSMMENUAUTH A ON A.MENU_ID = @p_menu_id AND A.AUTH_TARGET_TYPE = 'USER' AND A.AUTH_TARGET_CD = U.USER_ID
                WHERE U.USE_YN = 'Y'
                ORDER BY U.USER_ID;
            END
            ELSE IF @p_target_type = 'GRP'
            BEGIN
                SELECT G.USER_GRP_CD AS TargetCd, G.USER_GRP_NM AS TargetNm,
                       CAST((SELECT COUNT(1) FROM TSMUSERGRPMAP M WHERE M.USER_GRP_CD = G.USER_GRP_CD) AS NVARCHAR(10)) + N'명' AS SubNm,
                       ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                       ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                       ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                       ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                       ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                       ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                       ISNULL(A.AUTH10, 'N') AS Auth10
                FROM TSMUSERGRP G
                LEFT JOIN TSMMENUAUTH A ON A.MENU_ID = @p_menu_id AND A.AUTH_TARGET_TYPE = 'GRP' AND A.AUTH_TARGET_CD = G.USER_GRP_CD
                WHERE G.USE_YN = 'Y'
                ORDER BY G.USER_GRP_CD;
            END
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
