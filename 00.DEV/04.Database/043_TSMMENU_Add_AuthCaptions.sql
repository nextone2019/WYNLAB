-- TSMMENU에 AUTH01_NM~AUTH10_NM(추가권한 캡션) 컬럼 추가.
--
-- 메뉴등록(frmMenu)에서 화면별로 한 번만 캡션을 정의해두면, 사용자권한관리(frmUserAuth)의
-- 권한부여 트리에서 그 메뉴를 클릭했을 때 우측 AUTH01~10 패널에 그 캡션이 그대로 표시된다.
-- 캡션을 안 채운 슬롯은 화면에서 그냥 "Auth01"처럼 기본 표기로 대체(값 자체는 그대로 저장/조회됨).

ALTER TABLE TSMMENU ADD
    AUTH01_NM NVARCHAR(20) NULL,
    AUTH02_NM NVARCHAR(20) NULL,
    AUTH03_NM NVARCHAR(20) NULL,
    AUTH04_NM NVARCHAR(20) NULL,
    AUTH05_NM NVARCHAR(20) NULL,
    AUTH06_NM NVARCHAR(20) NULL,
    AUTH07_NM NVARCHAR(20) NULL,
    AUTH08_NM NVARCHAR(20) NULL,
    AUTH09_NM NVARCHAR(20) NULL,
    AUTH10_NM NVARCHAR(20) NULL;
GO

/* ---------- USP_SM_MENU_Q: AUTH01_NM~10_NM 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENU_Q
    @p_work_type VARCHAR(50),
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
            SELECT MENU_CD AS MenuCd, MENU_NM AS MenuNm, UPPER_MENU_CD AS UpperMenuCd,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, FORM_CLASS_NM AS FormClassNm,
                   ICON_NM AS IconNm, SORT_ORDER AS SortOrder, USE_YN AS UseYn,
                   AUTH01_NM AS Auth01Nm, AUTH02_NM AS Auth02Nm, AUTH03_NM AS Auth03Nm, AUTH04_NM AS Auth04Nm, AUTH05_NM AS Auth05Nm,
                   AUTH06_NM AS Auth06Nm, AUTH07_NM AS Auth07Nm, AUTH08_NM AS Auth08Nm, AUTH09_NM AS Auth09Nm, AUTH10_NM AS Auth10Nm
            FROM TSMMENU
            ORDER BY UPPER_MENU_CD, SORT_ORDER;
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

/* ---------- USP_SM_MENU_S: AUTH01_NM~10_NM 저장 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENU_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_cd VARCHAR(20),
    @p_menu_nm NVARCHAR(100),
    @p_upper_menu_cd VARCHAR(20) = NULL,
    @p_menu_level INT = 1,
    @p_menu_type VARCHAR(10) = 'FORM',
    @p_form_class_nm VARCHAR(200) = NULL,
    @p_icon_nm VARCHAR(50) = NULL,
    @p_sort_order INT = 0,
    @p_use_yn CHAR(1) = 'Y',
    @p_auth01_nm NVARCHAR(20) = NULL,
    @p_auth02_nm NVARCHAR(20) = NULL,
    @p_auth03_nm NVARCHAR(20) = NULL,
    @p_auth04_nm NVARCHAR(20) = NULL,
    @p_auth05_nm NVARCHAR(20) = NULL,
    @p_auth06_nm NVARCHAR(20) = NULL,
    @p_auth07_nm NVARCHAR(20) = NULL,
    @p_auth08_nm NVARCHAR(20) = NULL,
    @p_auth09_nm NVARCHAR(20) = NULL,
    @p_auth10_nm NVARCHAR(20) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN,
                                  AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM)
            VALUES (@p_menu_cd, @p_menu_nm, @p_upper_menu_cd, @p_menu_level, @p_menu_type, @p_form_class_nm, @p_icon_nm, @p_sort_order, 'Y',
                    @p_auth01_nm, @p_auth02_nm, @p_auth03_nm, @p_auth04_nm, @p_auth05_nm, @p_auth06_nm, @p_auth07_nm, @p_auth08_nm, @p_auth09_nm, @p_auth10_nm);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMENU SET
                MENU_NM = @p_menu_nm, UPPER_MENU_CD = @p_upper_menu_cd, MENU_LEVEL = @p_menu_level,
                MENU_TYPE = @p_menu_type, FORM_CLASS_NM = @p_form_class_nm, ICON_NM = @p_icon_nm,
                SORT_ORDER = @p_sort_order, USE_YN = @p_use_yn,
                AUTH01_NM = @p_auth01_nm, AUTH02_NM = @p_auth02_nm, AUTH03_NM = @p_auth03_nm, AUTH04_NM = @p_auth04_nm, AUTH05_NM = @p_auth05_nm,
                AUTH06_NM = @p_auth06_nm, AUTH07_NM = @p_auth07_nm, AUTH08_NM = @p_auth08_nm, AUTH09_NM = @p_auth09_nm, AUTH10_NM = @p_auth10_nm
            WHERE MENU_CD = @p_menu_cd;
        END

        SET @GeneratedCode = @p_menu_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- USP_SM_MENUAUTH_Q_1: AUTH01_NM~10_NM(캡션) JOIN 추가 ---------- */
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
                   ISNULL(A.AUTH10, 'N') AS Auth10,
                   M.AUTH01_NM AS Auth01Nm, M.AUTH02_NM AS Auth02Nm, M.AUTH03_NM AS Auth03Nm, M.AUTH04_NM AS Auth04Nm, M.AUTH05_NM AS Auth05Nm,
                   M.AUTH06_NM AS Auth06Nm, M.AUTH07_NM AS Auth07Nm, M.AUTH08_NM AS Auth08Nm, M.AUTH09_NM AS Auth09Nm, M.AUTH10_NM AS Auth10Nm
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
