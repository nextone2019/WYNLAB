-- 재고실사 대상확정: 대상 재고가 0건이어도 통과 (2026-10-04, WYNLAB_DEV 전용). 271의 USP_MA_CNT_SNAP_CORE 수정판.
-- 현재고에 데이터가 없는 창고/품목도 실사(신규 발견 재고 등록)를 할 수 있어야 한다. 이전에는 '대상 재고가 없습니다'로 대상확정이 막혀 시작조차 못 했다.
-- 라인이 0건인 채로 실사시작 후 '라인추가'로 입력하고, 입력완료는 라인 1건 이상을 요구한다. 여러 번 실행해도 안전하다.

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
    -- 대상 재고(라인)가 0건이어도 대상확정은 통과한다 - 빈 창고/새 품목은 실사중 '라인추가'로 넣는다(입력완료는 라인이 1건 이상 있어야 한다).

    UPDATE TMACNTM SET stat_cd = '1', freeze_mode = 'B', snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO
