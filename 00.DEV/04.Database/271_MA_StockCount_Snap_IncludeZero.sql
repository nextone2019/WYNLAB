-- 재고실사 대상확정에 재고수량 0인 라인도 포함 (2026-10-04, WYNLAB_DEV 전용). 269의 USP_MA_CNT_SNAP_CORE 수정판.
-- 장부 재고가 0이어도 TMASTOCK에 행이 남아 있으면(소진된 LOT 등) 실물이 있을 수 있어서 실사로 확인해야 한다(오입고/미등록 입고 등).
-- 이전에는 stock_qty <> 0 이거나 품번을 지정한 조건만 대상이 됐다. 이제 대상 창고(+품목그룹/품번 조건)에 해당하는 재고 행은 0/음수 포함 전부 라인이 된다.
-- 아예 재고 행이 없는 품목은 라인으로 나올 수 없으므로 실사중 '라인추가'로 넣는다. 여러 번 실행해도 안전하다.

-- ============================================================
-- 6) 대상확정 - 그 시점의 TMASTOCK을 라인으로 스냅샷 (0 -> 1)
--    장부수량 = 지금 재고 - (스냅샷 기준점 이후 변동) : 기준점 조회와 재고 조회 사이에 다른 수불이 확정돼도 이중 반영되지 않게 한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_SNAP_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @cdate VARCHAR(8), @msg NVARCHAR(2048);
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 대상확정할 수 있습니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TMACNTW WHERE cnt_id = @p_cnt_id) THROW 50001, N'대상 창고를 먼저 입력하세요.', 1;

    -- 같은 창고는 실사를 하나만 진행한다(대상확정~입력완료 사이). 방치된 실사는 취소로 창고를 푼다.
    DECLARE @busy NVARCHAR(100);
    SELECT TOP 1 @busy = m.cnt_no + N' / ' + ISNULL(h.wh_nm, N'')
    FROM TMACNTM m
        JOIN TMACNTW w ON w.cnt_id = m.cnt_id
        JOIN TMACNTW me ON me.cnt_id = @p_cnt_id AND me.wh_id = w.wh_id
        LEFT JOIN TBAWH h ON h.wh_id = w.wh_id
    WHERE m.cnt_id <> @p_cnt_id AND m.acc_id = @acc AND m.stat_cd IN ('1', '2', '3');
    IF @busy IS NOT NULL
    BEGIN
        SET @msg = N'같은 창고에 진행 중인 재고실사가 있습니다. (' + @busy + N') 먼저 확정하거나 취소하세요.';
        THROW 50001, @msg, 1;
    END

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);

    INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, recnt_yn, add_yn, stock_yn,
                         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @p_cnt_id, ROW_NUMBER() OVER (ORDER BY i.item_no, s.wh_id, s.lot_no), @acc, @no, s.item_id, ISNULL(s.unit_cd, i.unit_cd), s.wh_id, s.loc_id, s.lot_no,
           s.stock_qty - dbo.FN_MA_CNT_MOVE(s.acc_id, s.item_id, s.wh_id, s.loc_id, s.lot_no, @snap, '99991231', @p_cnt_id),
           'N', 'N', 'Y', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id
    WHERE s.acc_id = @acc AND i.stock_yn = 'Y'
      AND EXISTS (SELECT 1 FROM TMACNTW w
                  WHERE w.cnt_id = @p_cnt_id AND w.wh_id = s.wh_id
                    AND (w.grp_id IS NULL OR i.grp1_id = w.grp_id OR i.grp2_id = w.grp_id OR i.grp3_id = w.grp_id OR i.grp4_id = w.grp_id)
                    AND (w.item_id IS NULL OR w.item_id = s.item_id));

    IF NOT EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id)
        THROW 50001, N'대상 재고가 없습니다. 대상 창고/조건을 확인하세요.', 1;

    UPDATE TMACNTM SET stat_cd = '1', freeze_mode = 'B', snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO
