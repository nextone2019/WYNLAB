-- 재고실사: 다른 진행 중 실사에 이미 들어 있는 재고(품목/창고/위치/LOT)는 이 실사에 담지 못하게 막는다 (2026-10-04, WYNLAB_DEV 전용).
-- 진행 중 = 대상확정(1)/실사중(2)/입력완료(3). 확정(C)/취소(X)되거나 라인을 삭제하면 풀린다. (문서 단위로 같은 창고/같은 날짜 실사를 막지는 않는다)
--  - 대상확정(SNAP): 다른 진행 중 실사에 있는 재고 행은 라인으로 만들지 않고 건수/실사번호를 호출자에게 돌려준다(래퍼가 메시지로 안내).
--  - 라인추가(S_1 'N'): 다른 진행 중 실사에 있는 품목/LOT면 거부.
-- 이유: 같은 재고를 두 실사가 각각 조정하면 이중 조정(차이 중복 반영)이 된다. 여러 번 실행해도 안전하다.

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
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 대상확정할 수 있습니다.', 1;
    IF @wh IS NULL THROW 50001, N'실사할 창고를 먼저 선택해 저장하세요.', 1;

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);

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

    -- 대상 재고(라인)가 0건이어도 대상확정은 통과한다 - 빈 창고/새 품목은 실사중 '라인추가'로 넣는다(입력완료는 라인이 1건 이상 있어야 한다).
    UPDATE TMACNTM SET stat_cd = '1', freeze_mode = 'B', snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO

-- 라인추가 - 다른 진행 중 실사에 있는 품목/LOT 거부 (273의 USP_MA_CNT_S_1에 검사 한 단락 추가)
CREATE OR ALTER PROCEDURE USP_MA_CNT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,               /* N(계획 외 추가)에서만 */
    @p_lot_no NVARCHAR(50) = NULL,          /* N에서만 */
    @p_cnt_qty NUMERIC(18,4) = NULL,        /* 실사수량 - 재실사 지정 라인이면 재실사 수량으로 들어간다. NULL이면 수량은 그대로 */
    @p_adj_reason VARCHAR(10) = NULL,       /* MA0016 */
    @p_remark NVARCHAR(3000) = NULL,
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
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        DECLARE @stat VARCHAR(10), @app BIGINT, @acc BIGINT, @no VARCHAR(20), @snap BIGINT, @cdate VARCHAR(8), @hdr_wh BIGINT;
        SELECT @stat = stat_cd, @app = app_id, @acc = acc_id, @no = cnt_no, @snap = snap_trans_id, @cdate = cnt_date, @hdr_wh = wh_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;

        IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고실사를 찾을 수 없습니다.'; RETURN; END
        IF dbo.FN_AP_IS_LOCKED(@app) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END
        IF @stat = '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'대상확정 후에 라인을 다룰 수 있습니다.'; RETURN; END
        IF @stat IN ('C', 'X') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정/취소된 재고실사는 수정할 수 없습니다.'; RETURN; END
        IF @stat = '1' AND @p_work_type <> 'D' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'대상확정 상태에서는 라인 삭제만 가능합니다. 실사를 시작한 뒤 수량을 입력하세요.'; RETURN; END
        IF @stat = '3' AND @p_work_type <> 'U' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'입력완료 상태에서는 조정사유/비고만 수정할 수 있습니다. (수량 수정은 재실사 지정 또는 입력완료 취소)'; RETURN; END

        IF @p_work_type IN ('U', 'D') AND NOT EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id AND serl = @p_serl)
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사 라인을 찾을 수 없습니다.'; RETURN; END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_cnt_qty < 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사수량은 0 이상이어야 합니다.'; RETURN; END
            IF ISNULL(@p_adj_reason, '') <> '' AND NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0016' AND minor_cd = @p_adj_reason AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'조정사유가 올바르지 않습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @hdr_wh IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사 헤더에 창고가 없습니다. 창고를 지정해 저장하세요.'; RETURN; END

            DECLARE @item_stock VARCHAR(1), @item_lot VARCHAR(1), @unit VARCHAR(10);
            SELECT @item_stock = ISNULL(stock_yn, 'N'), @item_lot = ISNULL(lot_yn, 'N'), @unit = unit_cd FROM TBAITEM WHERE item_id = @p_item_id;
            IF @item_stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
            IF @item_stock <> 'Y' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고관리 품목만 실사할 수 있습니다.'; RETURN; END
            IF @item_lot = 'Y' AND ISNULL(@p_lot_no, N'') = N'' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'LOT 관리 품목은 LOT를 입력하세요.'; RETURN; END
            IF @item_lot <> 'Y' SET @p_lot_no = N'';
            SET @p_lot_no = ISNULL(@p_lot_no, N'');

            IF EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id AND item_id = @p_item_id AND wh_id = @hdr_wh AND loc_id = 0 AND lot_no = @p_lot_no)
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 이 실사에 있는 품목/LOT입니다.'; RETURN; END

            -- 다른 진행 중 실사(대상확정/실사중/입력완료)에 이미 있는 재고는 담을 수 없다 - 같은 재고를 두 실사가 조정하면 이중 반영된다.
            DECLARE @other_no VARCHAR(20);
            SELECT TOP 1 @other_no = xm.cnt_no
            FROM TMACNTD x JOIN TMACNTM xm ON xm.cnt_id = x.cnt_id
            WHERE xm.cnt_id <> @p_cnt_id AND xm.stat_cd IN ('1', '2', '3')
              AND x.item_id = @p_item_id AND x.wh_id = @hdr_wh AND x.loc_id = 0 AND x.lot_no = @p_lot_no;
            IF @other_no IS NOT NULL
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 진행 중인 재고실사(' + @other_no + N')에 포함된 품목/LOT입니다. 그 실사를 확정하거나 취소한 뒤 추가하세요.'; RETURN; END

            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id;

            -- 장부수량 = 지금 재고 - 스냅샷 이후 변동(스냅샷 시점 수량. 스냅샷 이후 생긴 LOT면 0)
            DECLARE @book NUMERIC(18,4) = ISNULL((SELECT stock_qty FROM TMASTOCK WHERE acc_id = @acc AND item_id = @p_item_id AND wh_id = @hdr_wh AND loc_id = 0 AND lot_no = @p_lot_no), 0)
                                          - dbo.FN_MA_CNT_MOVE(@acc, @p_item_id, @hdr_wh, 0, @p_lot_no, @snap, '99991231', @p_cnt_id);

            INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, cnt_qty, recnt_yn, fin_qty, add_yn, adj_reason, stock_yn,
                                 cnt_user_id, cnt_dt, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_cnt_id, @p_serl, @acc, @no, @p_item_id, @unit, @hdr_wh, 0, @p_lot_no, @book, @p_cnt_qty, 'N', @p_cnt_qty, 'Y', NULLIF(@p_adj_reason, ''), 'Y',
                    CASE WHEN @p_cnt_qty IS NULL THEN NULL ELSE @p_user_id END, CASE WHEN @p_cnt_qty IS NULL THEN NULL ELSE GETDATE() END,
                    @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 상태 2에서만 수량을 쓴다(상태 3은 사유/비고만). 재실사 지정 라인이면 재실사 수량 칸에 들어간다.
            DECLARE @write_qty BIT = CASE WHEN @stat = '2' AND @p_cnt_qty IS NOT NULL THEN 1 ELSE 0 END;

            UPDATE TMACNTD SET
                cnt_qty   = CASE WHEN @write_qty = 1 AND recnt_yn = 'N' THEN @p_cnt_qty ELSE cnt_qty END,
                recnt_qty = CASE WHEN @write_qty = 1 AND recnt_yn = 'Y' THEN @p_cnt_qty ELSE recnt_qty END,
                fin_qty   = CASE WHEN @write_qty = 1 THEN @p_cnt_qty ELSE fin_qty END,
                cnt_user_id = CASE WHEN @write_qty = 1 THEN @p_user_id ELSE cnt_user_id END,
                cnt_dt      = CASE WHEN @write_qty = 1 THEN GETDATE() ELSE cnt_dt END,
                adj_reason = NULLIF(@p_adj_reason, ''), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cnt_id = @p_cnt_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMACNTD WHERE cnt_id = @p_cnt_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 화면용 래퍼 - SNAP이 제외한 재고가 있으면 성공 메시지에 건수/실사번호를 담는다(화면이 안내창으로 보여준다)
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
                SET @ReturnMsg = LEFT(N'대상확정 완료. 다른 진행 중 실사(' + ISNULL(@skip_docs, N'') + N')에 이미 포함된 재고 ' + CAST(@skipped AS NVARCHAR(10)) + N'건은 제외했습니다.', 200);
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
