-- 기타출고 현재고 통제 (2026-10-03, WYNLAB_DEV 전용)
--  1) USP_MA_STOCKQTY_Q  - (사업장, 품목, 창고, 위치, LOT) 한 재고의 현재고 + 품목의 재고관리 여부(stock_yn) 1건. 기타출고등록 화면이 품목/창고/LOT가 바뀔 때와 저장 직전에 부른다.
--  2) USP_MA_ETCOUT_S_1  - 라인 저장(N/U) 때 재고관리 품목은 창고를 필수로 하고, 현재고(+같은 문서의 같은 재고를 쓰는 다른 라인)보다 많이 출고하면 저장 거부.
--     확정(CONFIRM_CORE)의 재고 검증은 그대로 둔다(마지막 안전장치 - 저장 뒤 다른 출고가 먼저 재고를 쓴 경우).
-- 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_MA_STOCKQTY_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
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
        IF @p_work_type = 'Q'
            SELECT ISNULL(SUM(s.stock_qty), 0) AS stock_qty,
                   ISNULL((SELECT i.stock_yn FROM TBAITEM i WHERE i.item_id = @p_item_id), 'N') AS stock_yn   /* 재고관리 품목 여부 - 직접 입력 라인의 재고반영 여부를 화면이 알 수 있게 */
            FROM TMASTOCK s
            WHERE (@p_acc_id IS NULL OR s.acc_id = @p_acc_id)
              AND s.item_id = @p_item_id AND s.wh_id = @p_wh_id
              AND s.loc_id = ISNULL(@p_loc_id, 0) AND s.lot_no = ISNULL(@p_lot_no, N'');
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_out_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_serl INT = NULL,
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
        DECLARE @hdr_stat VARCHAR(10), @hdr_acc BIGINT;
        SELECT @hdr_stat = stat_cd, @hdr_acc = acc_id FROM TMAETCOUTM WHERE out_id = @p_out_id;
        IF @hdr_stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고 문서를 찾을 수 없습니다.'; RETURN; END
        IF @hdr_stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타출고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END

        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT;
        IF @p_work_type = 'N'
            SELECT @src_type = NULLIF(@p_src_type, ''), @src_id = @p_src_id, @src_serl = @p_src_serl;
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl FROM TMAETCOUTD WHERE out_id = @p_out_id AND serl = @p_serl;
            IF @@ROWCOUNT = 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고 품목을 찾을 수 없습니다.'; RETURN; END
        END

        DECLARE @item BIGINT = @p_item_id, @unit VARCHAR(10), @stock VARCHAR(1), @src_no VARCHAR(20), @remain NUMERIC(18,4);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_out_qty IS NULL OR @p_out_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고수량은 0보다 커야 합니다.'; RETURN; END

            IF @src_type = 'ETCREQ'
            BEGIN
                SELECT @item = d.item_id, @unit = d.unit_cd, @src_no = d.req_no, @remain = d.qty - ISNULL(d.next_qty, 0)
                FROM TMAETCREQD d JOIN TMAETCREQM m ON m.req_id = d.req_id
                WHERE d.req_id = @src_id AND d.serl = @src_serl AND m.stat_cd = 'C' AND m.acc_id = @hdr_acc
                  AND ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y';
                IF @item IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청 품목을 찾을 수 없습니다.'; RETURN; END
                IF @p_out_qty > @remain
                BEGIN
                    SET @ReturnCode = -1;
                    SET @ReturnMsg = N'출고수량이 요청 잔량(' + CAST(CAST(@remain AS FLOAT) AS NVARCHAR(30)) + N')을 초과했습니다. (' + @src_no + N')';
                    RETURN;
                END
            END
            ELSE IF @src_type IS NOT NULL
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'올바르지 않은 원천입니다.'; RETURN; END

            IF @item IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            SELECT @stock = ISNULL(stock_yn, 'N'), @unit = ISNULL(@unit, unit_cd) FROM TBAITEM WHERE item_id = @item;
            IF @stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END

            -- 재고관리 품목은 저장 때 현재고를 확인해서 부족하면 저장을 거부한다(같은 출고 문서의 다른 라인이 같은 재고(품목/창고/위치/LOT)를 쓰는 수량까지 합산).
            IF @stock = 'Y'
            BEGIN
                IF @p_wh_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고관리 품목은 출고 창고를 입력해야 현재고를 확인할 수 있습니다.'; RETURN; END

                DECLARE @cur_stock NUMERIC(18,4), @others NUMERIC(18,4);
                SELECT @cur_stock = ISNULL(SUM(stock_qty), 0) FROM TMASTOCK
                WHERE acc_id = @hdr_acc AND item_id = @item AND wh_id = @p_wh_id AND loc_id = ISNULL(@p_loc_id, 0) AND lot_no = ISNULL(@p_lot_no, N'');
                SELECT @others = ISNULL(SUM(out_qty), 0) FROM TMAETCOUTD
                WHERE out_id = @p_out_id AND item_id = @item AND wh_id = @p_wh_id AND ISNULL(loc_id, 0) = ISNULL(@p_loc_id, 0)
                  AND ISNULL(lot_no, N'') = ISNULL(@p_lot_no, N'') AND stock_yn = 'Y' AND serl <> ISNULL(@p_serl, 0);

                IF @p_out_qty + @others > @cur_stock
                BEGIN
                    SET @ReturnCode = -1;
                    SET @ReturnMsg = N'현재고가 부족합니다. 현재고 ' + CAST(CAST(@cur_stock AS FLOAT) AS NVARCHAR(30))
                                   + N' < 출고수량 ' + CAST(CAST(@p_out_qty + @others AS FLOAT) AS NVARCHAR(30)) + N' (같은 재고의 다른 품목 포함)';
                    RETURN;
                END
            END        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAETCOUTD WHERE out_id = @p_out_id;

            INSERT INTO TMAETCOUTD (out_id, serl, acc_id, out_no, item_id, unit_cd, out_qty, lot_no, wh_id, loc_id, stock_yn,
                                    src_type, src_id, src_no, src_serl, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.out_id, @next, m.acc_id, m.out_no, @item, @unit, @p_out_qty, @p_lot_no, @p_wh_id, @p_loc_id, @stock,
                   @src_type, @src_id, @src_no, @src_serl, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAETCOUTM m WHERE m.out_id = @p_out_id;

            -- 헤더 출고유형이 기본값(비어 있거나 ETC_OUT)이고 요청에서 불러왔으면 요청의 출고유형으로 채운다
            IF @src_type = 'ETCREQ'
                UPDATE h SET trans_type = r.trans_type FROM TMAETCOUTM h, TMAETCREQM r
                WHERE h.out_id = @p_out_id AND r.req_id = @src_id AND ISNULL(h.trans_type, '') IN ('', 'ETC_OUT') AND ISNULL(r.trans_type, '') <> '';
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTD SET
                item_id = @item, unit_cd = @unit, out_qty = @p_out_qty, lot_no = @p_lot_no, wh_id = @p_wh_id, loc_id = @p_loc_id, stock_yn = @stock, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE out_id = @p_out_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAETCOUTD WHERE out_id = @p_out_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
