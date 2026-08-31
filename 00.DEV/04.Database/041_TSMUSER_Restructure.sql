-- TSMUSER 재구성(사장님 지시, 2026-08-31): POSITION_NM/EMAIL/MOBILE_NO/IS_ADMIN_YN 제거,
-- DEVELOPER_YN/DEL_YN/LAST_PWD_CHANGE_DATE/UPT_USER_ID/UPT_DT 추가.
--
-- DEVELOPER_YN은 앞으로 "시스템관리자"를 판단하는 조건으로 쓴다(USER_TYPE='A'와는 별개 -
-- USER_TYPE은 일반 메뉴권한 우회용 관리자, DEVELOPER_YN은 SYS 모듈/개발자 전용 도구
-- 접근용이라는 두 축으로 분리). 사용자등록 화면(frmUserAuth)에서 이 값을 고칠 수 있는 UI를
-- 아예 안 만든다(체크박스는 보이되 Enabled=false) - DB에서 직접 UPDATE해야만 바뀌는,
-- "UI 편집 경로가 아예 없는" 값으로 설계(project_wynlab_popup_lookup_framework 메모리의
-- SYS 모듈 접근제어 논의 참고).
--
-- 기존 IS_ADMIN_YN='Y'였던 사용자는 DEVELOPER_YN='Y'로 이관해서(이미 관리자였던 사람이
-- 갑자기 개발자 도구 접근을 잃지 않게) 마이그레이션한다.

EXEC sp_rename 'TSMUSER', 'TSMUSER_OLD_20260831';
GO
EXEC sp_rename 'PK_TSMUSER', 'PK_TSMUSER_OLD_20260831';
GO

CREATE TABLE [dbo].[TSMUSER](
	[USER_ID] [varchar](20) NOT NULL,
	[USER_NM] [nvarchar](50) NOT NULL,
	[PASSWORD_HASH] [varchar](200) NOT NULL,
	[DEPT_CD] [varchar](20) NULL,
	[EMP_NO] [varchar](20) NULL,

	[USER_TYPE] [char](1) NOT NULL,
	[DEVELOPER_YN] [char](1) NOT NULL,
	[LAST_LOGIN_DT] [datetime] NULL,
	[PWD_FAIL_CNT] [int] NOT NULL,
	[LAST_PWD_CHANGE_DATE] [datetime] NULL,

	[USE_YN] [char](1) NOT NULL,
	[DEL_YN] [char](1) NOT NULL,
	[REG_USER_ID] [varchar](20) NULL,
	[REG_DT] [datetime] NOT NULL,
	[UPT_USER_ID] [varchar](20) NULL,
	[UPT_DT] [datetime] NOT NULL,


 CONSTRAINT [PK_TSMUSER] PRIMARY KEY CLUSTERED
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO TSMUSER (
    USER_ID, USER_NM, PASSWORD_HASH, DEPT_CD, EMP_NO,
    USER_TYPE, DEVELOPER_YN, LAST_LOGIN_DT, PWD_FAIL_CNT, LAST_PWD_CHANGE_DATE,
    USE_YN, DEL_YN, REG_USER_ID, REG_DT, UPT_USER_ID, UPT_DT
)
SELECT
    USER_ID, USER_NM, PASSWORD_HASH, DEPT_CD, EMP_NO,
    USER_TYPE, CASE WHEN IS_ADMIN_YN = 'Y' THEN 'Y' ELSE 'N' END, LAST_LOGIN_DT, PWD_FAIL_CNT, NULL,
    USE_YN, 'N', REG_USER_ID, REG_DT, NULL, REG_DT
FROM TSMUSER_OLD_20260831;
GO

-- 기존 데이터는 TSMUSER_OLD_20260831에 그대로 남아있다(안전을 위해 안 지움) - 확인 끝나면
-- 나중에 DROP TABLE TSMUSER_OLD_20260831로 직접 정리하면 된다.

/* ---------- SSP_WYNLAB_GetSession: POSITION_NM/EMAIL/MOBILE_NO/IS_ADMIN_YN 제거, DEVELOPER_YN 추가 ---------- */
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
                U.PASSWORD_HASH AS PasswordHash,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
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

/* ---------- USP_SM_USERAUTH_Q: POSITION_NM/EMAIL/MOBILE_NO/IS_ADMIN_YN 제거, DEVELOPER_YN 추가 ---------- */
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
                        U.DEPT_CD AS DeptCd,
                        D.DEPT_NM AS DeptNm,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
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

/* ---------- USP_SM_USERAUTH_S: POSITION_NM/EMAIL/MOBILE_NO/IS_ADMIN_YN 파라미터 제거.
   DEVELOPER_YN은 파라미터 자체가 없다 - 이 프로시저로는 절대 못 바꾼다(신규는 항상 'N',
   수정은 아예 언급 안 함 = DB에서 직접 UPDATE해야만 바뀜). USER_TYPE도 이 화면엔 그걸 정할
   UI가 없어서(예전에도 없었음) 신규는 'U' 고정, 수정에서는 건드리지 않는다(예전 코드는
   isAdminYn 체크박스 값으로 매번 재계산해서 매 수정마다 USER_TYPE이 'U'로 조용히
   되돌아가는 버그가 있었는데, is_admin_yn 자체가 없어지면서 자연히 없어짐). ---------- */
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_dept_cd VARCHAR(20) = NULL,
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
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, DEPT_CD, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_no, @p_dept_cd, 'U', 'N', 'Y', 'N', 0, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_NO = @p_emp_no, DEPT_CD = @p_dept_cd, USE_YN = @p_use_yn,
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
