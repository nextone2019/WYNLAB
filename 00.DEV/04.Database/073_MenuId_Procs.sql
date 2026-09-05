/* =========================================================
   072(TSMMENU MENU_CD->MENU_ID)에 딸린 프로시저 13개 재작성 - SSP_POP_MENU_Q,
   USP_SM_MENU_Q/Q_1/Q_2/S/S_1, USP_SM_MENUAUTH_Q/Q_1/Q_2/S_1/S_2, USP_SM_GRIDLAYOUT_Q/S.

   USP_SM_MENU_Q_1은 예전엔 "이 코드가 이미 있는지"(신규등록 중복확인)였는데, MENU_ID가
   IDENTITY라 더는 그 확인이 필요없다 - 대신 범용적인 "이 ID가 실제로 있는지" 확인으로 바꿨다
   (수정/삭제 전 존재 확인 등에 재사용 가능).

   USP_SM_MENU_S는 이제 TSMMENU에 표준 감사컬럼(reg_user_id 등)이 생겨서 @p_user_id/
   @p_client_pc를 새로 받는다 - 서버 쪽(MenuManageRepository/MenusController)에서 이 값을
   채워 넘기도록 다음 단계(서버)에서 고쳐야 한다.
   ========================================================= */

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_MENU_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MENU_ID AS menu_id, MENU_NM AS menu_nm, UPPER_MENU_ID AS upper_menu_id, MENU_TYPE AS menu_type
    FROM TSMMENU
    WHERE USE_YN = 'Y'
      AND (@p_keyword IS NULL OR @p_keyword = '' OR MENU_NM LIKE '%' + @p_keyword + '%')
    ORDER BY MENU_NM;
END
GO

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
            SELECT MENU_ID AS MenuId, MENU_NM AS MenuNm, UPPER_MENU_ID AS UpperMenuId,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, MODULE AS Module, SCREEN_CLASS_NM AS ScreenClassNm,
                   ICON_NM AS IconNm, SORT_ORDER AS SortOrder, USE_YN AS UseYn,
                   AUTH01_NM AS Auth01Nm, AUTH02_NM AS Auth02Nm, AUTH03_NM AS Auth03Nm, AUTH04_NM AS Auth04Nm, AUTH05_NM AS Auth05Nm,
                   AUTH06_NM AS Auth06Nm, AUTH07_NM AS Auth07Nm, AUTH08_NM AS Auth08Nm, AUTH09_NM AS Auth09Nm, AUTH10_NM AS Auth10Nm
            FROM TSMMENU
            ORDER BY UPPER_MENU_ID, SORT_ORDER;
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

CREATE OR ALTER PROCEDURE USP_SM_MENU_Q_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
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
            SELECT COUNT(1) FROM TSMMENU WHERE MENU_ID = @p_menu_id;
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

CREATE OR ALTER PROCEDURE USP_SM_MENU_Q_2
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
            SELECT MENU_ID AS MenuId, MENU_NM AS MenuNm, UPPER_MENU_ID AS UpperMenuId,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, MODULE AS Module, SCREEN_CLASS_NM AS ScreenClassNm,
                   ICON_NM AS IconNm, SORT_ORDER AS SortOrder
            FROM TSMMENU
            WHERE USE_YN = 'Y'
            ORDER BY SORT_ORDER;
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

CREATE OR ALTER PROCEDURE USP_SM_MENU_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT = NULL,          -- U일 때만 필수(수정 대상 지정) - N은 IDENTITY가 자동 채번
    @p_menu_nm NVARCHAR(100),
    @p_upper_menu_id BIGINT = NULL,
    @p_menu_level INT = 1,
    @p_menu_type VARCHAR(10) = 'FORM',
    @p_module VARCHAR(20) = NULL,
    @p_screen_class_nm VARCHAR(200) = NULL,
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
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
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
            INSERT INTO TSMMENU (
                MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN,
                AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_menu_nm, @p_upper_menu_id, @p_menu_level, @p_menu_type, @p_module, @p_screen_class_nm, @p_icon_nm, @p_sort_order, 'Y',
                @p_auth01_nm, @p_auth02_nm, @p_auth03_nm, @p_auth04_nm, @p_auth05_nm, @p_auth06_nm, @p_auth07_nm, @p_auth08_nm, @p_auth09_nm, @p_auth10_nm,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(50));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMENU SET
                MENU_NM = @p_menu_nm, UPPER_MENU_ID = @p_upper_menu_id, MENU_LEVEL = @p_menu_level,
                MENU_TYPE = @p_menu_type, MODULE = @p_module, SCREEN_CLASS_NM = @p_screen_class_nm, ICON_NM = @p_icon_nm,
                SORT_ORDER = @p_sort_order, USE_YN = @p_use_yn,
                AUTH01_NM = @p_auth01_nm, AUTH02_NM = @p_auth02_nm, AUTH03_NM = @p_auth03_nm, AUTH04_NM = @p_auth04_nm, AUTH05_NM = @p_auth05_nm,
                AUTH06_NM = @p_auth06_nm, AUTH07_NM = @p_auth07_nm, AUTH08_NM = @p_auth08_nm, AUTH09_NM = @p_auth09_nm, AUTH10_NM = @p_auth10_nm,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE MENU_ID = @p_menu_id;

            SET @GeneratedCode = CAST(@p_menu_id AS VARCHAR(50));
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

CREATE OR ALTER PROCEDURE USP_SM_MENU_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_use_yn CHAR(1),
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
        IF @p_work_type = 'D'
        BEGIN
            UPDATE TSMMENU SET USE_YN = @p_use_yn WHERE MENU_ID = @p_menu_id;
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
            SELECT MENU_ID AS MenuId, AUTH_TARGET_TYPE AS AuthTargetType, AUTH_TARGET_CD AS AuthTargetCd,
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
            SELECT M.MENU_ID AS MenuId, M.MENU_NM AS MenuNm, M.UPPER_MENU_ID AS UpperMenuId,
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
                ON A.MENU_ID = M.MENU_ID AND A.AUTH_TARGET_TYPE = @p_target_type AND A.AUTH_TARGET_CD = @p_target_cd
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
                LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
                LEFT JOIN TBADEPT D ON D.DEPT_CD = E.DEPT_CD
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
                MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, SAVE_YN, DELETE_YN, PRINT_YN, EXCEL_YN,
                AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_DT, UPT_DT
            )
            SELECT J.MenuId, @p_target_type, @p_target_cd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.PrintYn, J.ExcelYn,
                   J.Auth01, J.Auth02, J.Auth03, J.Auth04, J.Auth05, J.Auth06, J.Auth07, J.Auth08, J.Auth09, J.Auth10,
                   GETDATE(), GETDATE()
            FROM OPENJSON(@p_items_json)
            WITH (
                MenuId   BIGINT      '$.MenuId',
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

CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_target_type VARCHAR(10),
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
            DELETE FROM TSMMENUAUTH WHERE MENU_ID = @p_menu_id AND AUTH_TARGET_TYPE = @p_target_type;

            INSERT INTO TSMMENUAUTH (
                MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, SAVE_YN, DELETE_YN, PRINT_YN, EXCEL_YN,
                AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_DT, UPT_DT
            )
            SELECT @p_menu_id, @p_target_type, J.TargetCd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.PrintYn, J.ExcelYn,
                   J.Auth01, J.Auth02, J.Auth03, J.Auth04, J.Auth05, J.Auth06, J.Auth07, J.Auth08, J.Auth09, J.Auth10,
                   GETDATE(), GETDATE()
            FROM OPENJSON(@p_items_json)
            WITH (
                TargetCd VARCHAR(20) '$.TargetCd',
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

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_id BIGINT = NULL,
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
            SELECT GRID_KEY AS GridKey, LAYOUT_XML AS LayoutXml
            FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id;
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

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_id BIGINT = NULL,
    @p_grid_key VARCHAR(50) = NULL,
    @p_layout_xml NVARCHAR(MAX) = NULL,
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
            IF EXISTS (SELECT 1 FROM TSMUSERGRIDLAYOUT WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id AND GRID_KEY = @p_grid_key)
                UPDATE TSMUSERGRIDLAYOUT
                SET LAYOUT_XML = @p_layout_xml, UPT_DT = GETDATE()
                WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id AND GRID_KEY = @p_grid_key;
            ELSE
                INSERT INTO TSMUSERGRIDLAYOUT (USER_ID, MENU_ID, GRID_KEY, LAYOUT_XML, UPT_DT)
                VALUES (@p_user_id, @p_menu_id, @p_grid_key, @p_layout_xml, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id AND GRID_KEY = @p_grid_key;
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
