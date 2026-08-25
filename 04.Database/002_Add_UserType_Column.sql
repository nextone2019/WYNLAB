/* =========================================================
   TSMUSER에 USER_TYPE(CHAR(1): A=관리자, U=일반사용자) 컬럼 추가.

   기존 IS_ADMIN_YN(Y/N)을 완전히 대체하는 게 목적이다 - 나중에 역할이 더 늘어나도
   (예: 감사자 등) 코드 값 하나만 늘리면 되고, 여러 곳에서 Y/N과 A/U 두 가지 판단
   기준이 동시에 존재해서 서로 어긋나는 사고를 막기 위함. IS_ADMIN_YN 컬럼 자체는
   안전하게 그대로 둔다(삭제 안 함) - 이후 어떤 서버/클라이언트 코드도 이 컬럼을
   더 이상 읽지 않지만, 혹시 몰라 데이터는 보존해둔다.

   USER_TYPE='A'인 사용자는 MenuPermissionService.GetEffectivePermissionsAsync에서
   TSMMENUAUTH를 아예 안 보고 모든 메뉴/모든 액션을 허용으로 내려준다(기존 IsAdminYn
   기준 관리자 우회 로직을 그대로 이어받음 - 로직 자체는 이미 있었고, 판단 기준 컬럼만
   바뀐다).
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TSMUSER') AND name = 'USER_TYPE')
BEGIN
    ALTER TABLE TSMUSER ADD USER_TYPE CHAR(1) NOT NULL DEFAULT 'U';
END
GO

UPDATE TSMUSER SET USER_TYPE = 'A' WHERE IS_ADMIN_YN = 'Y';
GO

/* ---------- SSP_WYNLAB_GetSession: SELECT 목록에 UserType 추가 ---------- */
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
                U.DEPT_CD       AS DeptCd,
                D.DEPT_NM       AS DeptNm,
                U.POSITION_NM   AS PositionNm,
                U.EMAIL         AS Email,
                U.MOBILE_NO     AS MobileNo,
                U.PASSWORD_HASH AS PasswordHash,
                U.USE_YN        AS UseYn,
                U.IS_ADMIN_YN   AS IsAdminYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt
            FROM TSMUSER U
            LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
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

/* ---------- USP_SM_USER_Q: SELECT 목록에 UserType 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_USER_Q
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
            SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, U.EMP_NO AS EmpNo,
                   U.DEPT_CD AS DeptCd, D.DEPT_NM AS DeptNm, U.POSITION_NM AS PositionNm,
                   U.EMAIL AS Email, U.MOBILE_NO AS MobileNo, U.USE_YN AS UseYn,
                   U.IS_ADMIN_YN AS IsAdminYn, U.USER_TYPE AS UserType, U.LAST_LOGIN_DT AS LastLoginDt
            FROM TSMUSER U
            LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
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

/* ---------- USP_SM_USER_S: @p_user_type 파라미터 추가(INSERT/UPDATE 둘 다) ---------- */
CREATE OR ALTER PROCEDURE USP_SM_USER_S
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_dept_cd VARCHAR(20) = NULL,
    @p_position_nm NVARCHAR(30) = NULL,
    @p_email VARCHAR(100) = NULL,
    @p_mobile_no VARCHAR(20) = NULL,
    @p_use_yn CHAR(1) = 'Y',
    @p_is_admin_yn CHAR(1) = 'N',
    @p_user_type CHAR(1) = 'U',
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
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, DEPT_CD, POSITION_NM, EMAIL, MOBILE_NO, IS_ADMIN_YN, USER_TYPE, USE_YN)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_no, @p_dept_cd, @p_position_nm, @p_email, @p_mobile_no, @p_is_admin_yn, @p_user_type, 'Y');
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_NO = @p_emp_no, DEPT_CD = @p_dept_cd, POSITION_NM = @p_position_nm,
                EMAIL = @p_email, MOBILE_NO = @p_mobile_no, USE_YN = @p_use_yn, IS_ADMIN_YN = @p_is_admin_yn,
                USER_TYPE = @p_user_type
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
