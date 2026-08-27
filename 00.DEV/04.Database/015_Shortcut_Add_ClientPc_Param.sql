-- USP_SM_SHORTCUT_S 수정 - @p_client_pc 파라미터 누락 수정.
--
-- 범용 데이터 통로(api/data/save)의 GenericDataRepository.SaveAsync는 어떤 저장 프로시저를
-- 부르든 항상 p_user_id와 p_client_pc 두 개를 같이 넘긴다(클라이언트가 보낸 값을 믿지 않고
-- 서버가 직접 채움 - GenericDataRepository.cs 주석 참고). 014에서 만든 USP_SM_SHORTCUT_S는
-- @p_user_id만 선언하고 @p_client_pc를 빠뜨려서, 이 화면에서 저장/초기화를 누르면 매번
-- "Procedure or function USP_SM_SHORTCUT_S has too many arguments specified" SQL 오류로
-- 실패한다(프로시저가 모르는 파라미터를 서버가 보내므로). USP_SM_MINORCODE_S_1 등 범용
-- 통로를 쓰는 다른 모든 저장 프로시저는 이미 이 파라미터를 갖고 있다 - 값 자체는 감사(audit)
-- 용도라 이 프로시저 안에서는 안 써도, 시그니처에는 반드시 있어야 한다.

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_SHORTCUT_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_action_cd VARCHAR(20),
    @p_key_combo VARCHAR(30) = NULL,
    @p_user_id VARCHAR(20),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
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
            IF EXISTS (
                SELECT 1
                FROM TSMSHORTCUTDEFAULT d
                LEFT JOIN TSMUSERSHORTCUT u
                    ON u.action_cd = d.action_cd
                   AND u.user_id = @p_user_id
                WHERE d.action_cd <> @p_action_cd
                  AND COALESCE(u.key_combo, d.key_combo) = @p_key_combo
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 다른 동작에 사용 중인 단축키입니다.';
                RETURN;
            END

            MERGE TSMUSERSHORTCUT AS target
            USING (SELECT @p_user_id AS user_id, @p_action_cd AS action_cd) AS src
                ON target.user_id = src.user_id AND target.action_cd = src.action_cd
            WHEN MATCHED THEN
                UPDATE SET key_combo = @p_key_combo, upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHEN NOT MATCHED THEN
                INSERT (user_id, action_cd, key_combo, reg_user_id, reg_dt)
                VALUES (@p_user_id, @p_action_cd, @p_key_combo, @p_user_id, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERSHORTCUT WHERE user_id = @p_user_id AND action_cd = @p_action_cd;
        END

        SET @GeneratedCode = @p_action_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
