-- 마이 메뉴 안에 사용자 임의 폴더로 묶어 보여주는 기능(2026-09-21 목업 - "핵심 업무",
-- "자재 출납" 같은 사용자 지정 그룹). 실제 시스템 메뉴 계층(TSMMENU)과는 무관한, 순전히
-- 사용자 개인이 즐겨찾기를 정리하려고 붙이는 이름표라 별도 폴더 테이블 없이 이 매핑
-- 테이블에 텍스트 컬럼 하나만 추가한다 - NULL/빈 문자열이면 폴더 없이 마이 메뉴 바로
-- 아래 평평하게 보인다.
ALTER TABLE TSMUSERFAVORITEMENU ADD FOLDER_NM NVARCHAR(50) NULL;
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
            SELECT MENU_ID, FOLDER_NM
            FROM TSMUSERFAVORITEMENU
            WHERE USER_ID = @p_user_id
            ORDER BY SORT_ORDER, REG_DT;
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

-- N(추가) / D(제거) / O(순서변경) / F(폴더 지정 - FOLDER_NM 갱신, 빈 문자열/NULL이면 폴더 해제).
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_FAVORITEMENU_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_id BIGINT = NULL,
    @p_sort_order INT = NULL,
    @p_folder_nm NVARCHAR(50) = NULL,
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
            BEGIN
                DECLARE @nextOrder INT = ISNULL((SELECT MAX(SORT_ORDER) FROM TSMUSERFAVORITEMENU WHERE USER_ID = @p_user_id), 0) + 1;
                INSERT INTO TSMUSERFAVORITEMENU (USER_ID, MENU_ID, REG_DT, SORT_ORDER)
                VALUES (@p_user_id, @p_menu_id, GETDATE(), @nextOrder);
            END
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERFAVORITEMENU
            WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id;
        END
        ELSE IF @p_work_type = 'O'
        BEGIN
            UPDATE TSMUSERFAVORITEMENU
            SET SORT_ORDER = @p_sort_order
            WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id;
        END
        ELSE IF @p_work_type = 'F'
        BEGIN
            UPDATE TSMUSERFAVORITEMENU
            SET FOLDER_NM = NULLIF(@p_folder_nm, '')
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
