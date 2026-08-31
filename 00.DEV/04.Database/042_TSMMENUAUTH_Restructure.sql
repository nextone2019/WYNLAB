-- TSMMENUAUTH 재구성(사장님 지시, 2026-08-31): PRINT_YN, AUTH01~AUTH10, REG/UPT 감사컬럼 추가.
--
-- AUTH01~10은 조회/입력/저장/출력/엑셀 이외에 화면마다 추가로 분리해야 할 권한이 생겼을 때
-- 쓰는 예비 슬롯이다 - 지금은 전부 기본값 'N'으로 시작하고 아직 의미가 정해진 게 없다.
--
-- C# 쪽 이름은 안 바꾼다 - UPDATE_YN 컬럼은 그대로 두고(C#의 UpdateYn/CanUpdate/MenuAction.Update가
-- 이미 "저장" 권한을 뜻하는 걸로 10곳 넘게 쓰이고 있어서, 전부 다시 이름 붙이는 대신 의미만
-- "저장"으로 그대로 유지) PRINT_YN/AUTH01~10만 새로 추가해서 노출한다
-- (BaseForm.CanPrint/Auth[] 참고, WYNLAB.BaseForm/BaseForm.cs).

EXEC sp_rename 'TSMMENUAUTH', 'TSMMENUAUTH_OLD_20260831';
GO
EXEC sp_rename 'PK_TSMMENUAUTH', 'PK_TSMMENUAUTH_OLD_20260831';
GO

CREATE TABLE [dbo].[TSMMENUAUTH](
	[MENU_CD] [varchar](20) NOT NULL,
	[AUTH_TARGET_TYPE] [varchar](10) NOT NULL,
	[AUTH_TARGET_CD] [varchar](20) NOT NULL,

	[VIEW_YN] [char](1) NOT NULL,
	[INSERT_YN] [char](1) NOT NULL,
	[DELETE_YN] [char](1) NOT NULL,
	[SAVE_YN] [char](1) NOT NULL,
	[PRINT_YN] [char](1) NOT NULL,
	[EXCEL_YN] [char](1) NOT NULL,

	[AUTH01] [char](1) NOT NULL,
	[AUTH02] [char](1) NOT NULL,
	[AUTH03] [char](1) NOT NULL,
	[AUTH04] [char](1) NOT NULL,
	[AUTH05] [char](1) NOT NULL,
	[AUTH06] [char](1) NOT NULL,
	[AUTH07] [char](1) NOT NULL,
	[AUTH08] [char](1) NOT NULL,
	[AUTH09] [char](1) NOT NULL,
	[AUTH10] [char](1) NOT NULL,

	[REG_USER_ID] [varchar](20) NULL,
	[REG_DT] [datetime] NOT NULL,
	[UPT_USER_ID] [varchar](20) NULL,
	[UPT_DT] [datetime] NOT NULL,


 CONSTRAINT [PK_TSMMENUAUTH] PRIMARY KEY CLUSTERED
(
	[MENU_CD] ASC,
	[AUTH_TARGET_TYPE] ASC,
	[AUTH_TARGET_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PRINT_YN은 예전엔 존재하지 않던 권한이라(출력 자체가 지금까지 권한으로 안 걸려 있었음)
-- VIEW_YN과 같은 값으로 이관한다 - 조회 가능했던 대상은 그대로 출력도 가능한 것으로 취급해서
-- 마이그레이션으로 인해 갑자기 기능이 막히는 걸 피한다. AUTH01~10은 전부 신규라 'N'.
INSERT INTO TSMMENUAUTH (
    MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD,
    VIEW_YN, INSERT_YN, DELETE_YN, SAVE_YN, PRINT_YN, EXCEL_YN,
    AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10,
    REG_USER_ID, REG_DT, UPT_USER_ID, UPT_DT
)
SELECT
    MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD,
    VIEW_YN, INSERT_YN, DELETE_YN, UPDATE_YN, VIEW_YN, EXCEL_YN,
    'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N',
    NULL, GETDATE(), NULL, GETDATE()
FROM TSMMENUAUTH_OLD_20260831;
GO

-- 기존 데이터는 TSMMENUAUTH_OLD_20260831에 그대로 남아있다(안전을 위해 안 지움) - 확인 끝나면
-- 나중에 DROP TABLE TSMMENUAUTH_OLD_20260831로 직접 정리하면 된다.

/* ---------- USP_SM_MENUAUTH_Q: 로그인시 권한병합용 원본 조회 - PRINT_YN/AUTH01~10 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    @p_group_codes NVARCHAR(MAX) = NULL,
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
            SELECT MENU_CD AS MenuCd, AUTH_TARGET_TYPE AS AuthTargetType, AUTH_TARGET_CD AS AuthTargetCd,
                   VIEW_YN AS ViewYn, INSERT_YN AS InsertYn, SAVE_YN AS UpdateYn,
                   DELETE_YN AS DeleteYn, PRINT_YN AS PrintYn, EXCEL_YN AS ExcelYn,
                   AUTH01 AS Auth01, AUTH02 AS Auth02, AUTH03 AS Auth03, AUTH04 AS Auth04, AUTH05 AS Auth05,
                   AUTH06 AS Auth06, AUTH07 AS Auth07, AUTH08 AS Auth08, AUTH09 AS Auth09, AUTH10 AS Auth10
            FROM TSMMENUAUTH
            WHERE (AUTH_TARGET_TYPE = 'USER' AND AUTH_TARGET_CD = @p_user_id)
               OR (AUTH_TARGET_TYPE = 'GRP' AND @p_group_codes IS NOT NULL
                   AND AUTH_TARGET_CD IN (SELECT value FROM STRING_SPLIT(@p_group_codes, ',')));
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

/* ---------- USP_SM_MENUAUTH_Q_1: 권한부여관리 화면 조회 - PRINT_YN/AUTH01~10 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_target_type VARCHAR(10),
    @p_target_cd VARCHAR(20),
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
            SELECT M.MENU_CD AS MenuCd, M.MENU_NM AS MenuNm, M.UPPER_MENU_CD AS UpperMenuCd,
                   M.MENU_TYPE AS MenuType, M.SORT_ORDER AS SortOrder,
                   ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                   ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                   ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                   ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                   ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                   ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                   ISNULL(A.AUTH10, 'N') AS Auth10
            FROM TSMMENU M
            LEFT JOIN TSMMENUAUTH A
                ON A.MENU_CD = M.MENU_CD AND A.AUTH_TARGET_TYPE = @p_target_type AND A.AUTH_TARGET_CD = @p_target_cd
            WHERE M.USE_YN = 'Y'
            ORDER BY M.SORT_ORDER;
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

/* ---------- USP_SM_MENUAUTH_S_1: 권한부여관리 화면 저장 - PRINT_YN/AUTH01~10 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_target_type VARCHAR(10),
    @p_target_cd VARCHAR(20),
    @p_items_json NVARCHAR(MAX),
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
        IF @p_work_type = 'U'
        BEGIN
            DELETE FROM TSMMENUAUTH WHERE AUTH_TARGET_TYPE = @p_target_type AND AUTH_TARGET_CD = @p_target_cd;

            INSERT INTO TSMMENUAUTH (
                MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, SAVE_YN, DELETE_YN, PRINT_YN, EXCEL_YN,
                AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_DT, UPT_DT
            )
            SELECT J.MenuCd, @p_target_type, @p_target_cd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.PrintYn, J.ExcelYn,
                   J.Auth01, J.Auth02, J.Auth03, J.Auth04, J.Auth05, J.Auth06, J.Auth07, J.Auth08, J.Auth09, J.Auth10,
                   GETDATE(), GETDATE()
            FROM OPENJSON(@p_items_json)
            WITH (
                MenuCd   VARCHAR(20) '$.MenuCd',
                ViewYn   CHAR(1)     '$.ViewYn',
                InsertYn CHAR(1)     '$.InsertYn',
                UpdateYn CHAR(1)     '$.UpdateYn',
                DeleteYn CHAR(1)     '$.DeleteYn',
                PrintYn  CHAR(1)     '$.PrintYn',
                ExcelYn  CHAR(1)     '$.ExcelYn',
                Auth01   CHAR(1)     '$.Auth01',
                Auth02   CHAR(1)     '$.Auth02',
                Auth03   CHAR(1)     '$.Auth03',
                Auth04   CHAR(1)     '$.Auth04',
                Auth05   CHAR(1)     '$.Auth05',
                Auth06   CHAR(1)     '$.Auth06',
                Auth07   CHAR(1)     '$.Auth07',
                Auth08   CHAR(1)     '$.Auth08',
                Auth09   CHAR(1)     '$.Auth09',
                Auth10   CHAR(1)     '$.Auth10'
            ) J
            WHERE J.ViewYn = 'Y' OR J.InsertYn = 'Y' OR J.UpdateYn = 'Y' OR J.DeleteYn = 'Y' OR J.PrintYn = 'Y' OR J.ExcelYn = 'Y'
               OR J.Auth01 = 'Y' OR J.Auth02 = 'Y' OR J.Auth03 = 'Y' OR J.Auth04 = 'Y' OR J.Auth05 = 'Y'
               OR J.Auth06 = 'Y' OR J.Auth07 = 'Y' OR J.Auth08 = 'Y' OR J.Auth09 = 'Y' OR J.Auth10 = 'Y';
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
