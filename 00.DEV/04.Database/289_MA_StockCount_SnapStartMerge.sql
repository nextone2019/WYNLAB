-- 재고실사: 대상확정(1)과 실사시작(2)을 한 단계로 합친다 (2026-10-05, WYNLAB_DEV)
--  - USP_MA_CNT_SNAP_CORE(= 화면 [실사시작] 버튼의 SNAP): 스냅샷을 뜨면 곧바로 stat_cd='2'(실사중)로 간다. 라인 추가/삭제/수량 입력이 바로 열린다.
--  - 기존에 '1'(대상확정)에 머물던 문서는 '2'(실사중)로 옮긴다. 상태 '1'은 더 이상 생기지 않는다(START 액션은 그대로 두되 대상 상태가 없다).
--  - 래퍼(USP_MA_CNT_C_S)의 제외 안내 문구만 '실사시작'으로 바꾼다.
-- 273의 USP_MA_CNT_SNAP_CORE(= 라이브 정의)에서 UPDATE 한 곳과 메시지만 바뀜. 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_MA_CNT_SNAP_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    @o_skipped INT = 0 OUTPUT,                  /* 다른 진행 중 실사에 이미 있어 제외한 재고 행 수 */
    @o_skip_docs NVARCHAR(100) = NULL OUTPUT    /* 그 실사번호들(최대 3개) */
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @wh BIGINT;
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @wh = wh_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 실사를 시작할 수 있습니다.', 1;
    IF @wh IS NULL THROW 50001, N'실사할 창고를 먼저 선택해 저장하세요.', 1;

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);
    -- 수불 통제 방식(시스템설정 MA.CNT_FREEZE): A 완전 동결 / B 변동 보정(기본). 이 실사가 시작되는 시점의 값을 문서에 고정한다.
    DECLARE @freeze VARCHAR(1) = CASE WHEN dbo.FSM_PROCCONFIG('MA.CNT_FREEZE') = 'A' THEN 'A' ELSE 'B' END;

    -- 다른 진행 중 실사에 이미 있는 재고 행(제외 대상)
    DECLARE @skip TABLE (item_id BIGINT, lot_no NVARCHAR(50), loc_id BIGINT, cnt_no VARCHAR(20));
    INSERT INTO @skip (item_id, lot_no, loc_id, cnt_no)
    SELECT s.item_id, s.lot_no, s.loc_id, xm.cnt_no
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id AND i.stock_yn = 'Y'
        JOIN TMACNTD x ON x.item_id = s.item_id AND x.wh_id = s.wh_id AND x.loc_id = s.loc_id AND x.lot_no = s.lot_no
        JOIN TMACNTM xm ON xm.cnt_id = x.cnt_id AND xm.cnt_id <> @p_cnt_id AND xm.stat_cd IN ('1', '2', '3')
    WHERE s.acc_id = @acc AND s.wh_id = @wh;

    SELECT @o_skipped = COUNT(*) FROM (SELECT DISTINCT item_id, lot_no, loc_id FROM @skip) k;
    SELECT @o_skip_docs = STRING_AGG(d.cnt_no, ', ') FROM (SELECT DISTINCT TOP 3 cnt_no FROM @skip ORDER BY cnt_no) d;

    INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, recnt_yn, add_yn, stock_yn,
                         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @p_cnt_id, ROW_NUMBER() OVER (ORDER BY i.item_no, s.lot_no), @acc, @no, s.item_id, ISNULL(s.unit_cd, i.unit_cd), s.wh_id, s.loc_id, s.lot_no,
           s.stock_qty - dbo.FN_MA_CNT_MOVE(s.acc_id, s.item_id, s.wh_id, s.loc_id, s.lot_no, @snap, '99991231', @p_cnt_id),
           'N', 'N', 'Y', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id
    WHERE s.acc_id = @acc AND s.wh_id = @wh AND i.stock_yn = 'Y'
      AND NOT EXISTS (SELECT 1 FROM @skip k WHERE k.item_id = s.item_id AND k.lot_no = s.lot_no AND k.loc_id = s.loc_id);

    -- 대상 재고(라인)가 0건이어도 시작은 통과한다 - 빈 창고/새 품목은 '라인추가'로 넣는다(입력완료는 라인이 1건 이상 있어야 한다).
    UPDATE TMACNTM SET stat_cd = '2', freeze_mode = @freeze, snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO

-- 이미 '대상확정'에 머물던 문서는 실사중으로
UPDATE TMACNTM SET stat_cd = '2' WHERE stat_cd = '1';
GO

CREATE OR ALTER PROCEDURE USP_MA_CNT_C_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_serls VARCHAR(2000) = NULL,          /* RECNT - 재실사할 라인 순번(쉼표 구분) */
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
        IF @p_work_type = 'SNAP'
        BEGIN
            DECLARE @skipped INT = 0, @skip_docs NVARCHAR(100);
            EXEC USP_MA_CNT_SNAP_CORE @p_cnt_id, @p_user_id, @p_client_pc, @skipped OUTPUT, @skip_docs OUTPUT;
            IF @skipped > 0
                SET @ReturnMsg = LEFT(N'실사시작 완료. 다른 진행 중 실사(' + ISNULL(@skip_docs, N'') + N')에 이미 포함된 재고 ' + CAST(@skipped AS NVARCHAR(10)) + N'건은 제외했습니다.', 200);
        END
        ELSE IF @p_work_type IN ('START', 'DONE', 'UNDONE', 'RECNT', 'X') EXEC USP_MA_CNT_STATE_CORE @p_cnt_id, @p_work_type, @p_serls, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'C' EXEC USP_MA_CNT_CONFIRM_CORE @p_cnt_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC' EXEC USP_MA_CNT_CANCEL_CORE @p_cnt_id, @p_user_id, @p_client_pc;
        ELSE THROW 50001, N'알 수 없는 처리입니다.', 1;
        SET @GeneratedCode = CAST(@p_cnt_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
