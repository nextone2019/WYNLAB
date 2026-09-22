-- 마이 메뉴(TSMUSERFAVORITEMENU) 사용자 임의 순서 지원(2026-09-21, "상위로/하위로 이동").
-- 기존엔 REG_DT(등록시각)순 고정이었는데, 사용자가 직접 순서를 바꿀 수 있어야 해서 컬럼을
-- 추가한다.
ALTER TABLE TSMUSERFAVORITEMENU ADD SORT_ORDER INT NOT NULL CONSTRAINT DF_TSMUSERFAVORITEMENU_SORT_ORDER DEFAULT 0;
GO

-- 기존 행은 REG_DT 순서 그대로 1,2,3...으로 초기값을 채운다(마이그레이션 시점 1회성).
;WITH ordered AS (
    SELECT USER_ID, MENU_ID, ROW_NUMBER() OVER (PARTITION BY USER_ID ORDER BY REG_DT) AS rn
    FROM TSMUSERFAVORITEMENU
)
UPDATE t SET SORT_ORDER = o.rn
FROM TSMUSERFAVORITEMENU t
JOIN ordered o ON o.USER_ID = t.USER_ID AND o.MENU_ID = t.MENU_ID;
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

-- N(추가 - 이미 있으면 그대로 둠, idempotent, SORT_ORDER는 현재 사용자 최대값+1) /
-- D(제거 - idempotent) / O(순서변경 - 지정한 메뉴 하나의 SORT_ORDER를 갱신, "상위로/하위로
-- 이동"은 클라이언트가 스왑된 두 항목 각각에 대해 이걸 두 번 부르는 대신, 바뀐 전체 순서를
-- 한 번에 다시 매긴다 - FavoriteMenuRepository.ReorderAsync 참고).
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_FAVORITEMENU_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_id BIGINT = NULL,
    @p_sort_order INT = NULL,
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
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
