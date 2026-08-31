-- TSMUSER.DEPT_CD 컬럼 삭제(사장님 지시, 2026-08-31) - 부서 정보를 TSMUSER에 직접 저장하지
-- 않고, EMP_NO로 TBAEMP를 조인해서 그때그때 부서코드/부서명을 얻어온다. TSMUSER는 이제
-- EMP_NO만 남고, DEPT_CD/DEPT_NM은 화면 조회용 조인 컬럼일 뿐 저장 대상이 아니다.
--
-- C#은 UserRow/UserManageRow/UserInfoDto/UserListItemDto의 DeptCd/DeptNm 프로퍼티를 그대로
-- 둔다(구조는 안 바꿈) - 값을 채우는 SQL만 "U.DEPT_CD" -> "E.DEPT_CD"(TBAEMP 조인)로 바뀐다.
-- 대신 저장(등록/수정) 경로에서는 DeptCd를 더 이상 입력받지 않는다 - EMP_NO만 저장하면 부서는
-- 자동으로 따라온다(UserCreateRequest/UserUpdateRequest에서 DeptCd 제거, USP_SM_USERAUTH_S의
-- @p_dept_cd 파라미터 제거).

ALTER TABLE TSMUSER DROP COLUMN DEPT_CD;
GO

/* ---------- SSP_WYNLAB_GetSession: DEPT_CD/DEPT_NM을 TBAEMP 경유로 조회 ---------- */
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
                E.DEPT_CD       AS DeptCd,
                D.DEPT_NM       AS DeptNm,
                U.PASSWORD_HASH AS PasswordHash,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
            LEFT JOIN TBADEPT D ON D.DEPT_CD = E.DEPT_CD
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

/* ---------- USP_SM_USERAUTH_Q: DEPT_CD/DEPT_NM을 TBAEMP 경유로 조회 ---------- */
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
                        E.DEPT_CD AS DeptCd,
                        D.DEPT_NM AS DeptNm,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
                        LEFT JOIN TBADEPT D ON D.DEPT_CD = E.DEPT_CD
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

/* ---------- USP_SM_USERAUTH_S: @p_dept_cd 파라미터/DEPT_CD 컬럼 제거(더 이상 직접 저장 안 함) ---------- */
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_no VARCHAR(20) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMUSER
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_no, 'U', 'N', 'Y', 'N', 0, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_NO = @p_emp_no, USE_YN = @p_use_yn,
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

/* ---------- USP_SM_USERGRP_Q_2: DEPT_NM을 TBAEMP 경유로 조회 ---------- */
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
            LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
            LEFT JOIN TBADEPT D ON D.DEPT_CD = E.DEPT_CD
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
