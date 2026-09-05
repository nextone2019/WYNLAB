-- 비밀번호 변경 절차(사장님 지시, 2026-09-01/02 설계) 인프라.
--
-- MUST_CHANGE_PWD_YN 플래그 하나로 "만료돼서 강제변경" / "관리자가 초기화" 두 트리거를 다
-- 처리한다(둘 다 결과는 같음 - 다음 로그인 때 새 비밀번호를 강제로 설정해야 함). 지금은 분실
-- 시나리오(이메일 인증코드로 셀프 초기화)만 구현한다 - 만료 판정(며칠 지나면 만료)은 하드코딩
-- 안 하기로 했고(사장님 지시, 시스템환경설정 화면에서 나중에 관리) 관리자 초기화 화면도 아직
-- 없다 - 둘 다 이 플래그/메커니즘을 그대로 재사용해서 나중에 이어붙이면 된다.
ALTER TABLE TSMUSER ADD EMAIL NVARCHAR(200) NULL;
GO

ALTER TABLE TSMUSER ADD MUST_CHANGE_PWD_YN CHAR(1) NOT NULL CONSTRAINT DF_TSMUSER_MUSTCHANGEPWD DEFAULT 'N';
GO

-- 이메일로 보낸 6자리 인증코드 - 사용자당 여러 건 쌓일 수 있어(재요청) IDENTITY PK를 쓴다.
-- 유효시간은 서버(AuthService)가 발급 시점에 계산해서 EXPIRE_DT에 그대로 박아넣는다.
CREATE TABLE TSMPWDRESETTOKEN (
    TOKEN_ID    INT IDENTITY(1,1)   NOT NULL PRIMARY KEY,
    USER_ID     VARCHAR(20)         NOT NULL,
    CODE        VARCHAR(10)         NOT NULL,
    EXPIRE_DT   DATETIME            NOT NULL,
    USED_YN     CHAR(1)             NOT NULL DEFAULT 'N',
    REQ_DT      DATETIME            NOT NULL DEFAULT GETDATE()
);
GO

-- N: 인증코드 발급 - 이 사용자의 기존 미사용 코드는 전부 무효화(USED_YN='Y')하고 새로 하나
-- 넣는다(재요청 시 이전 코드로는 더 이상 통과 못 하게). 코드 자체는 C#에서 난수로 만들어
-- 파라미터로 받는다(SQL 쪽 난수 생성 대신 - 훨씬 간단하고 테스트하기 쉬움).
--
-- C: 인증코드 확인 + 비밀번호 교체를 한 트랜잭션으로 - 확인 따로, 교체 따로 하면 그 사이에
-- 같은 코드로 두 번 시도하는 경쟁 상태가 생길 수 있다. 유효한 코드가 없으면(없음/만료/이미
-- 사용됨을 구분하지 않고) 전부 같은 실패 메시지 - 공격자에게 어떤 조건이 틀렸는지 알려주지
-- 않기 위함(계정 존재 여부를 캐는 수단이 되는 것도 같은 이유로 막음).
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_PWDRESET_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_code VARCHAR(10) = NULL,
    @p_expire_dt DATETIME = NULL,
    @p_new_password_hash VARCHAR(200) = NULL,
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
            UPDATE TSMPWDRESETTOKEN SET USED_YN = 'Y' WHERE USER_ID = @p_user_id AND USED_YN = 'N';

            INSERT INTO TSMPWDRESETTOKEN (USER_ID, CODE, EXPIRE_DT)
            VALUES (@p_user_id, @p_code, @p_expire_dt);
        END
        ELSE IF @p_work_type = 'C'
        BEGIN
            BEGIN TRANSACTION;

            DECLARE @tokenId INT;
            SELECT TOP 1 @tokenId = TOKEN_ID
            FROM TSMPWDRESETTOKEN WITH (UPDLOCK, SERIALIZABLE)
            WHERE USER_ID = @p_user_id AND CODE = @p_code AND USED_YN = 'N' AND EXPIRE_DT > GETDATE();

            IF @tokenId IS NULL
            BEGIN
                ROLLBACK TRANSACTION;
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'인증코드가 올바르지 않거나 만료되었습니다.';
                RETURN;
            END

            UPDATE TSMPWDRESETTOKEN SET USED_YN = 'Y' WHERE TOKEN_ID = @tokenId;

            UPDATE TSMUSER
            SET PASSWORD_HASH = @p_new_password_hash,
                MUST_CHANGE_PWD_YN = 'N',
                LAST_PWD_CHANGE_DATE = GETDATE(),
                PWD_FAIL_CNT = 0
            WHERE USER_ID = @p_user_id;

            COMMIT TRANSACTION;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 로그인 시 MUST_CHANGE_PWD_YN을 같이 받아와야 AuthService가 강제변경 여부를 판단할 수 있다.
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
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn
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

-- 사용자권한관리(frmUserAuth) 목록/저장에 이메일을 추가한다.
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
                        E.DEPT_CD AS DeptCd,
                        D.DEPT_NM AS DeptNm,
                        U.EMAIL AS Email,
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMUSER
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_no, @p_email, 'U', 'N', 'Y', 'N', 0, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_NO = @p_emp_no, EMAIL = @p_email, USE_YN = @p_use_yn,
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

-- 로그인된 상태에서 비밀번호를 바꾼다(강제변경 다이얼로그 등) - 현재 비밀번호 확인은 C#
-- (AuthService, BCrypt.Verify)에서 이미 끝내고 새 해시만 넘겨받는다.
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S_3]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_password_hash VARCHAR(200) = NULL,
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
        IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER
            SET PASSWORD_HASH = @p_password_hash,
                MUST_CHANGE_PWD_YN = 'N',
                LAST_PWD_CHANGE_DATE = GETDATE(),
                PWD_FAIL_CNT = 0
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
