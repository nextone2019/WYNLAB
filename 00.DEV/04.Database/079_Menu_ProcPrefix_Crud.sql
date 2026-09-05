-- TSMMENU.PROC_PREFIX(004_Add_Menu_ProcPrefix.sql에서 컬럼만 추가됐던 것)를 메뉴관리 CRUD
-- 경로(USP_SM_MENU_Q/USP_SM_MENU_S, api/menus, frmMenu 화면) 전체에 실제로 연결한다.
--
-- 문제: PROC_PREFIX는 지금까지 이 마이그레이션 스크립트들이 손으로 쓴 INSERT문에서만 채워졌고,
-- 메뉴관리 화면(frmMenu)이나 AI Builder의 메뉴 즉시등록(RegisterMenuAsync, api/menus POST)은
-- 이 컬럼 자체를 몰랐다(USP_SM_MENU_S에 파라미터가 아예 없었음) - 그래서 그 경로로 만들어진
-- 메뉴는 전부 PROC_PREFIX가 NULL로 남고, api/data/*(범용 데이터 통로) 저장/조회 시 "이 메뉴는
-- 범용 데이터 통로를 사용하도록 설정되어 있지 않습니다"로 막힌다(DataController.ValidateAsync
-- ②단계 - PROC_PREFIX가 비어있으면 무조건 거부). AI Builder로 만든 화면은 전부 이 경로를 타므로
-- frmCust뿐 아니라 앞으로 생성되는 모든 화면이 동일하게 겪는 문제였다(2026-09-04 실제 발견).
--
-- 조치: USP_SM_MENU_Q/S에 PROC_PREFIX를 정식 컬럼/파라미터로 추가하고, 이미 잘못 등록된
-- frmCust 메뉴(MENU_ID=49)도 여기서 같이 바로잡는다. C# 쪽(DTO/Repository/Controller/
-- frmAIBuilder/frmMenu)은 별도 코드 커밋에서 함께 수정한다.

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
                   ICON_NM AS IconNm, PROC_PREFIX AS ProcPrefix, SORT_ORDER AS SortOrder, USE_YN AS UseYn,
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

CREATE OR ALTER PROCEDURE USP_SM_MENU_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT = NULL,          -- U일 때만 필수(수정 대상 지정) - N은 IDENTITY로 자동 채움
    @p_menu_nm NVARCHAR(100),
    @p_upper_menu_id BIGINT = NULL,
    @p_menu_level INT = 1,
    @p_menu_type VARCHAR(10) = 'FORM',
    @p_module VARCHAR(20) = NULL,
    @p_screen_class_nm VARCHAR(200) = NULL,
    @p_icon_nm VARCHAR(50) = NULL,
    @p_proc_prefix VARCHAR(100) = NULL,
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
                MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, ICON_NM, PROC_PREFIX, SORT_ORDER, USE_YN,
                AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_menu_nm, @p_upper_menu_id, @p_menu_level, @p_menu_type, @p_module, @p_screen_class_nm, @p_icon_nm, @p_proc_prefix, @p_sort_order, 'Y',
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
                PROC_PREFIX = @p_proc_prefix, SORT_ORDER = @p_sort_order, USE_YN = @p_use_yn,
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

-- 이미 PROC_PREFIX 없이 등록된 frmCust 메뉴를 바로잡는다(AI Builder가 즉시등록한 실제 값,
-- 078_frmCust_Menu.sql의 INSERT문과 동일한 값 - 그 파일은 IF NOT EXISTS라 이미 있는 이 행에는
-- 적용되지 않았었다).
UPDATE TSMMENU SET PROC_PREFIX = 'USP_BA_CUST_'
WHERE MODULE = 'BA' AND SCREEN_CLASS_NM = 'frmCust' AND PROC_PREFIX IS NULL;
GO
