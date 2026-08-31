-- 개인별 그리드 레이아웃(컬럼 순서/숨김/폭) 저장 기능. 정렬/그룹핑/필터는 저장 대상에서
-- 제외한다(클라이언트 GridViewWynBehavior.OptionsLayout 설정으로 막음, 서버는 그냥 텍스트를
-- 그대로 저장/조회만 한다). SM 계열 시스템 테이블이라 컬럼명은 TSMMENU/TSMUSER처럼 대문자.
--
-- 키는 (USER_ID, MENU_CD, GRID_KEY) - GRID_KEY는 화면 안의 그리드 컨트롤 이름(gvw1, gvw2 등)을
-- 그대로 쓴다(BaseForm.ApplyGridLayoutsAsync 참고) - 화면 하나에 그리드가 여러 개 있어도 자동
-- 구분된다.
CREATE TABLE TSMUSERGRIDLAYOUT (
    USER_ID     VARCHAR(20)     NOT NULL,
    MENU_CD     VARCHAR(20)     NOT NULL,
    GRID_KEY    VARCHAR(50)     NOT NULL,
    LAYOUT_XML  NVARCHAR(MAX)   NOT NULL,
    UPT_DT      DATETIME        NOT NULL,
    CONSTRAINT PK_TSMUSERGRIDLAYOUT PRIMARY KEY (USER_ID, MENU_CD, GRID_KEY)
);
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_cd VARCHAR(20) = NULL,
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
            SELECT GRID_KEY, LAYOUT_XML
            FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_CD = @p_menu_cd;
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

-- N(저장 - 있으면 갱신, 없으면 신규) / D(삭제 - "레이아웃초기화" 시 저장된 값 자체를 지움).
-- 클라이언트가 이 그리드에 저장된 값이 있었는지 미리 알 필요 없게, N은 항상 upsert로 처리한다.
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_cd VARCHAR(20) = NULL,
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
            IF EXISTS (SELECT 1 FROM TSMUSERGRIDLAYOUT WHERE USER_ID = @p_user_id AND MENU_CD = @p_menu_cd AND GRID_KEY = @p_grid_key)
                UPDATE TSMUSERGRIDLAYOUT
                SET LAYOUT_XML = @p_layout_xml, UPT_DT = GETDATE()
                WHERE USER_ID = @p_user_id AND MENU_CD = @p_menu_cd AND GRID_KEY = @p_grid_key;
            ELSE
                INSERT INTO TSMUSERGRIDLAYOUT (USER_ID, MENU_CD, GRID_KEY, LAYOUT_XML, UPT_DT)
                VALUES (@p_user_id, @p_menu_cd, @p_grid_key, @p_layout_xml, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_CD = @p_menu_cd AND GRID_KEY = @p_grid_key;
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
