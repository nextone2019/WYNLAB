-- 사이드바 "마이 메뉴(My Menu)" - 사용자가 직접 즐겨찾기한 메뉴 목록(2026-09-21).
-- TSMUSERSHORTCUT/TSMUSERGRIDLAYOUT과 같은 성격의 "개인별 선택" 테이블이라 같은 관례를
-- 따른다 - 기본값 테이블이 따로 없고(대상 자체가 사용자의 자유 선택), 행의 존재 자체가
-- "즐겨찾기됨"을 뜻하므로 소프트삭제 컬럼 없이 그냥 DELETE로 해제한다. FK도 두 선례와
-- 동일하게 안 건다(참조 무결성은 proc/app 로직에서만 관리하는 이 DB의 관례).
CREATE TABLE TSMUSERFAVORITEMENU (
    USER_ID VARCHAR(20) NOT NULL,
    MENU_ID BIGINT      NOT NULL,
    REG_DT  DATETIME    NOT NULL,
    CONSTRAINT PK_TSMUSERFAVORITEMENU PRIMARY KEY (USER_ID, MENU_ID)
);
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_FAVORITEMENU_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
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
            SELECT MENU_ID
            FROM TSMUSERFAVORITEMENU
            WHERE USER_ID = @p_user_id
            ORDER BY REG_DT;
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

-- N(추가 - 이미 있으면 그대로 둠, idempotent) / D(제거 - idempotent).
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_FAVORITEMENU_S]
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
        IF @p_work_type = 'N'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSMUSERFAVORITEMENU WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id)
                INSERT INTO TSMUSERFAVORITEMENU (USER_ID, MENU_ID, REG_DT)
                VALUES (@p_user_id, @p_menu_id, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERFAVORITEMENU
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
