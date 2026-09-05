-- 메뉴기준 권한관리(frmMenuAuth) 신규 화면 - frmUserAuth(대상 1명 고르면 메뉴트리에 권한을
-- 매김)의 반대 축: 메뉴 1건을 고르면 전체 사용자/사용자그룹과 그들의 권한을 그리드로 보여주고
-- 부여한다. 같은 TSMMENUAUTH 테이블을 조회하는 축만 바꾼 것이라 USP_SM_MENUAUTH_Q_1/S_1
-- (042/043 마이그레이션)과 짝이 되는 _Q_2/_S_2를 새로 추가한다.

/* ---------- USP_SM_MENUAUTH_Q_2: 메뉴기준 권한관리 조회 - 메뉴 1건 + 대상유형별 전체 대상 목록 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_cd VARCHAR(20),
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
                LEFT JOIN TSMMENUAUTH A ON A.MENU_CD = @p_menu_cd AND A.AUTH_TARGET_TYPE = 'USER' AND A.AUTH_TARGET_CD = U.USER_ID
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
                LEFT JOIN TSMMENUAUTH A ON A.MENU_CD = @p_menu_cd AND A.AUTH_TARGET_TYPE = 'GRP' AND A.AUTH_TARGET_CD = G.USER_GRP_CD
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

/* ---------- USP_SM_MENUAUTH_S_2: 메뉴기준 권한관리 저장 - 메뉴 1건 + 대상유형 기준 전체치환 ---------- */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_cd VARCHAR(20),
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
            DELETE FROM TSMMENUAUTH WHERE MENU_CD = @p_menu_cd AND AUTH_TARGET_TYPE = @p_target_type;

            INSERT INTO TSMMENUAUTH (
                MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, SAVE_YN, DELETE_YN, PRINT_YN, EXCEL_YN,
                AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_DT, UPT_DT
            )
            SELECT @p_menu_cd, @p_target_type, J.TargetCd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.PrintYn, J.ExcelYn,
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

-- 화면 자체를 메뉴트리에 등록(SM_USR_G 산하, 사용자등록(SM_USER, sort20)과 단축키설정
-- (SM_SHORTCUT, sort30) 사이).
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_CD = 'SM_MENU_AUTH')
BEGIN
    INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER, USE_YN, REG_DT)
    VALUES ('SM_MENU_AUTH', N'메뉴별권한관리', 'SM_USR_G', 3, 'FORM', 'WYNLAB.SM.frmMenuAuth, WYNLAB.SM', 25, 'Y', GETDATE());
END
GO
