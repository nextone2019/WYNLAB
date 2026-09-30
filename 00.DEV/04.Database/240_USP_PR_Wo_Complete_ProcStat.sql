-- 작업지시 완료(E) 시 공정별 진행상태도 '완료'로, 완료취소(EC) 시 되돌림 (공정 상태 E가 정의만 있고 어디서도 설정되지 않던 문제) (2026-09-30)
-- 226/227의 USP_PR_WO_C_S 라이브 정의에 공정행 상태 갱신 2줄을 추가했다.

-- ============================================================
-- 4) USP_PR_WO_C_S - 상태 처리
--    C  확정   : 계획 -> 확정 (이때부터 실적/이전 등록 가능. 공정마다 외주처/창고가 있어야 함)
--    CC 확정취소: 확정 -> 계획 (실적/이전이 아직 없을 때만)
--    X  중단   : 확정/진행 -> 중단 (실적/이전 등록 불가)
--    XC 중단해제: 중단 -> 실적이 있으면 진행, 없으면 확정
--    E  완료   : 진행 -> 완료. 끝나지 않은 이전(작성/이동중/차이대기)이 있으면 거부
--    EC 완료취소: 완료 -> 진행
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_C_S
    @p_work_type VARCHAR(50),               /* C / CC / X / XC / E / EC */
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_user_id VARCHAR(50),
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        DECLARE @stat VARCHAR(10), @has_rslt BIT = 0;
        SELECT @stat = stat_cd FROM TPRWOM WHERE wo_id = @p_wo_id;
        IF @stat IS NULL THROW 50001, N'작업지시를 찾을 수 없습니다.', 1;
        IF EXISTS (SELECT 1 FROM TPRWOD WHERE wo_id = @p_wo_id AND in_qty > 0) SET @has_rslt = 1;

        DECLARE @new_stat VARCHAR(10), @bad_serl INT;
        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'계획 상태의 작업지시만 확정할 수 있습니다.', 1;
            -- 확정하면 실적/이전을 등록할 수 있게 되므로, 공정마다 외주처와 창고가 정해져 있어야 한다.
            SELECT TOP 1 @bad_serl = serl FROM TPRWOD WHERE wo_id = @p_wo_id AND (cust_id IS NULL OR wh_id IS NULL) ORDER BY serl;
            IF @bad_serl IS NOT NULL
            BEGIN
                DECLARE @m NVARCHAR(200) = N'공정 ' + CAST(@bad_serl AS NVARCHAR(10)) + N'번의 외주처/창고가 지정되지 않아 확정할 수 없습니다.';
                THROW 50001, @m, 1;
            END
            SET @new_stat = 'C';
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정 상태(실적 등록 전)의 작업지시만 확정취소할 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TPRXFERM WHERE wo_id = @p_wo_id) THROW 50001, N'외주 이전이 등록된 작업지시는 확정취소할 수 없습니다.', 1;
            SET @new_stat = '0';
        END
        ELSE IF @p_work_type = 'X'
        BEGIN
            IF @stat NOT IN ('C', '1') THROW 50001, N'확정 또는 진행 상태의 작업지시만 중단할 수 있습니다.', 1;
            SET @new_stat = 'X';
        END
        ELSE IF @p_work_type = 'XC'
        BEGIN
            IF @stat <> 'X' THROW 50001, N'중단된 작업지시만 중단 해제할 수 있습니다.', 1;
            SET @new_stat = CASE WHEN @has_rslt = 1 THEN '1' ELSE 'C' END;
        END
        ELSE IF @p_work_type = 'E'
        BEGIN
            IF @stat <> '1' THROW 50001, N'진행 상태의 작업지시만 완료할 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TPRXFERM WHERE wo_id = @p_wo_id AND stat_cd IN ('0', '1', '2'))
                THROW 50001, N'끝나지 않은 외주 이전(작성/이동중/차이대기)이 있어 완료할 수 없습니다.', 1;
            SET @new_stat = 'E';
        END
        ELSE IF @p_work_type = 'EC'
        BEGIN
            IF @stat <> 'E' THROW 50001, N'완료된 작업지시만 완료 취소할 수 있습니다.', 1;
            SET @new_stat = '1';
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        UPDATE TPRWOM SET stat_cd = @new_stat, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE wo_id = @p_wo_id;

        IF @p_work_type = 'E'  -- 완료: 진행 중이던 공정도 완료
            UPDATE TPRWOD SET stat_cd = 'E', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE wo_id = @p_wo_id AND stat_cd = '1';
        ELSE IF @p_work_type = 'EC'  -- 완료취소: 실적이 있는 공정은 진행, 없으면 대기
            UPDATE TPRWOD SET stat_cd = CASE WHEN in_qty > 0 THEN '1' ELSE '0' END, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE wo_id = @p_wo_id AND stat_cd = 'E';

        SET @GeneratedCode = CAST(@p_wo_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
