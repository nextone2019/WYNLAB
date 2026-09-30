-- 웨이퍼입고를 작업지시에서 독립시킨다 (2026-09-29): 웨이퍼는 작업지시와 무관하게 먼저 도착하므로 입고를 품목/LOT/수량/창고로 직접 등록하고,
-- 작업지시는 나중에 그 입고된 미배정 LOT를 골라 배정한다(227번 USP_PR_WO_S의 @p_start_lot_id).
--  * TPRRCV: wo_id/wo_no/lot_id를 NULL 허용으로 바꾼다(작업지시 배정은 TPRLOT.wo_id로 표현). 입고 확정 때 TPRLOT(미배정 LOT)을 만들고/키운다.
--  * 같은 품목+LOT번호로 나눠 도착해도 되고(부분 입고), 이미 작업지시에 배정된 LOT에는 더 입고할 수 없다.
--  * 확정취소: 작업지시에 배정된 LOT는 거부(작업지시 삭제로 배정을 먼저 푼다). 그 LOT를 실적에서 썼으면 재고 부족으로 거부(기존 규칙).
--  * 입고 화면의 작업지시 불러오기(USP_PR_RCVPICK_Q)는 폐지하고, 작업지시 화면의 "입고 LOT 불러오기" 팝업(USP_PR_WOLOTPICK_Q)을 새로 만든다.

-- ============================================================
-- 1) 테이블 변경
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TPRRCV') AND name = 'wo_id' AND is_nullable = 0)
    ALTER TABLE TPRRCV ALTER COLUMN wo_id BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TPRRCV') AND name = 'wo_no' AND is_nullable = 0)
    ALTER TABLE TPRRCV ALTER COLUMN wo_no VARCHAR(20) NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TPRRCV') AND name = 'lot_id' AND is_nullable = 0)
    ALTER TABLE TPRRCV ALTER COLUMN lot_id BIGINT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('TPRRCV') AND name = 'IX_TPRRCV_lot')
    CREATE NONCLUSTERED INDEX IX_TPRRCV_lot ON TPRRCV (acc_id, item_id, lot_no);
GO

-- 미배정 LOT 조회용(작업지시 LOT 선택)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('TPRLOT') AND name = 'IX_TPRLOT_free')
    CREATE NONCLUSTERED INDEX IX_TPRLOT_free ON TPRLOT (acc_id, wo_id) INCLUDE (lot_no, item_id);
GO

DROP PROCEDURE IF EXISTS USP_PR_RCVPICK_Q;
GO

-- ============================================================
-- 2) USP_PR_RCV_Q - L: 목록 / Q: 한 건 (배정된 작업지시 표시)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCV_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rcv_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* L: 입고번호/LOT/품목 */
    @p_acc_id BIGINT = NULL,
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
        IF @p_work_type = 'L'
        BEGIN
            SELECT r.rcv_id, r.rcv_no, r.rcv_date, r.lot_no, i.item_nm, r.qty, r.unit_cd, w.wh_nm, r.stat_cd
            FROM TPRRCV r
                LEFT JOIN TBAITEM i ON i.item_id = r.item_id
                LEFT JOIN TBAWH w ON w.wh_id = r.wh_id
            WHERE (@p_acc_id IS NULL OR r.acc_id = @p_acc_id)
              AND (@p_keyword IS NULL OR @p_keyword = N''
                   OR r.rcv_no LIKE '%' + @p_keyword + '%' OR r.lot_no LIKE '%' + @p_keyword + '%'
                   OR i.item_nm LIKE '%' + @p_keyword + '%' OR i.item_no LIKE '%' + @p_keyword + '%')
            ORDER BY r.rcv_id DESC;
        END
        ELSE IF @p_work_type = 'Q'
        BEGIN
            SELECT r.rcv_id, r.acc_id, r.rcv_no, r.rcv_date, r.item_id, i.item_no, i.item_nm, r.lot_no, r.unit_cd, r.qty,
                   r.wh_id, w.wh_nm, r.sup_cust_id, c.cust_nm AS sup_cust_nm, r.stat_cd, r.cfm_dt, r.cfm_user_id, r.remark,
                   lo.wo_id AS lot_wo_id, wm.wo_no AS lot_wo_no
            FROM TPRRCV r
                LEFT JOIN TBAITEM i ON i.item_id = r.item_id
                LEFT JOIN TBAWH w ON w.wh_id = r.wh_id
                LEFT JOIN TBACUST c ON c.cust_id = r.sup_cust_id
                LEFT JOIN TPRLOT lo ON lo.acc_id = r.acc_id AND lo.item_id = r.item_id AND lo.lot_no = r.lot_no
                LEFT JOIN TPRWOM wm ON wm.wo_id = lo.wo_id
            WHERE r.rcv_id = @p_rcv_id;
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

-- ============================================================
-- 3) USP_PR_RCV_S - N/U/D (작성 상태에서만). 품목/LOT번호/입고창고/수량을 직접 입력.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCV_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rcv_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_rcv_date VARCHAR(8) = NULL,
    @p_item_id BIGINT = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_sup_cust_id BIGINT = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @cur_stat VARCHAR(10);
            SELECT @cur_stat = stat_cd FROM TPRRCV WHERE rcv_id = @p_rcv_id;
            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'웨이퍼 입고 문서를 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 입고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            DECLARE @unit VARCHAR(10);
            SELECT @unit = unit_cd FROM TBAITEM WHERE item_id = @p_item_id;
            IF @p_item_id IS NULL OR NOT EXISTS (SELECT 1 FROM TBAITEM WHERE item_id = @p_item_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고할 품목(웨이퍼)을 선택하세요.'; RETURN;
            END
            IF ISNULL(LTRIM(RTRIM(@p_lot_no)), N'') = N''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'LOT 번호를 입력하세요. (공급처가 부여한 웨이퍼 LOT 번호)'; RETURN;
            END
            IF @p_wh_id IS NULL OR NOT EXISTS (SELECT 1 FROM TBAWH WHERE wh_id = @p_wh_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 창고를 선택하세요. (첫 공정 외주처 창고)'; RETURN;
            END
            IF ISNULL(@p_qty, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고수량은 0보다 커야 합니다.'; RETURN;
            END
            SET @p_lot_no = LTRIM(RTRIM(@p_lot_no));
            IF EXISTS (SELECT 1 FROM TPRLOT WHERE acc_id = @p_acc_id AND item_id = @p_item_id AND lot_no = @p_lot_no AND wo_id IS NOT NULL)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 작업지시에 배정된 LOT입니다. 같은 LOT 번호로 더 입고할 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TPRRCV', 'rcv_no', @p_acc_id, @new_no OUTPUT;
            INSERT INTO TPRRCV (acc_id, rcv_no, rcv_date, item_id, lot_no, unit_cd, qty, wh_id, sup_cust_id, stat_cd, cfm_yn, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_rcv_date, @p_item_id, @p_lot_no, @unit, @p_qty, @p_wh_id, @p_sup_cust_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_rcv_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRRCV SET rcv_date = @p_rcv_date, item_id = @p_item_id, lot_no = @p_lot_no, unit_cd = @unit, qty = @p_qty, wh_id = @p_wh_id,
                sup_cust_id = @p_sup_cust_id, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rcv_id = @p_rcv_id;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRRCV WHERE rcv_id = @p_rcv_id;

        SET @GeneratedCode = CAST(@p_rcv_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 4) USP_PR_RCV_C_S - C 입고 확정(수불 PU_IN + 미배정 LOT 생성/증가) / CC 확정취소
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCV_C_S
    @p_work_type VARCHAR(50),               /* C / CC */
    ---------------------------------------------------------------------------------------------------
    @p_rcv_id BIGINT = NULL,
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
        DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @lot_no NVARCHAR(50), @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @wh BIGINT,
                @stat VARCHAR(10), @rmk NVARCHAR(3000);
        SELECT @acc = acc_id, @no = rcv_no, @date = rcv_date, @lot_no = lot_no, @item = item_id, @unit = unit_cd, @qty = qty, @wh = wh_id, @stat = stat_cd, @rmk = remark
        FROM TPRRCV WHERE rcv_id = @p_rcv_id;
        IF @stat IS NULL THROW 50001, N'웨이퍼 입고 문서를 찾을 수 없습니다.', 1;

        DECLARE @tid BIGINT, @lot_id BIGINT, @lot_wo BIGINT;
        SELECT @lot_id = lot_id, @lot_wo = wo_id FROM TPRLOT WHERE acc_id = @acc AND item_id = @item AND lot_no = @lot_no;

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 입고만 확정할 수 있습니다.', 1;
            IF @lot_wo IS NOT NULL THROW 50001, N'이미 작업지시에 배정된 LOT입니다. 같은 LOT 번호로 더 입고할 수 없습니다.', 1;

            -- 이 LOT에 이미 재고가 있으면(부분 입고) 수량을 더하고, 재고가 하나도 없는 기존 LOT 행(옛 방식으로 만든 고아)이면 입고수량으로 맞춘다.
            DECLARE @had_stock BIT = CASE WHEN EXISTS (SELECT 1 FROM TMASTOCK WHERE acc_id = @acc AND item_id = @item AND lot_no = @lot_no AND stock_qty <> 0) THEN 1 ELSE 0 END;

            BEGIN TRAN;
            EXEC USP_PR_TRANS_POST @acc, 'I', 'PU_IN', @item, @wh, @lot_no, @qty, 'WR', @p_rcv_id, @no, 1, NULL, @rmk, @p_user_id, @p_client_pc, @date, @tid OUTPUT;

            IF @lot_id IS NULL
            BEGIN
                INSERT INTO TPRLOT (acc_id, lot_no, item_id, unit_cd, init_qty, wo_id, wo_serl, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @lot_no, @item, @unit, @qty, NULL, NULL, N'웨이퍼 입고 ' + @no, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
                SET @lot_id = SCOPE_IDENTITY();
            END
            ELSE
                UPDATE TPRLOT SET init_qty = CASE WHEN @had_stock = 1 THEN init_qty + @qty ELSE @qty END, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE lot_id = @lot_id;

            UPDATE TPRRCV SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, lot_id = @lot_id,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rcv_id = @p_rcv_id;
            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정된 입고만 확정취소할 수 있습니다.', 1;
            IF @lot_wo IS NOT NULL THROW 50001, N'작업지시에 배정된 LOT입니다. 작업지시를 삭제해 배정을 푼 뒤 확정취소하세요.', 1;

            BEGIN TRAN;
            -- 그 LOT를 이미 소진/이동했으면 현재고 부족으로 여기서 거부된다.
            EXEC USP_PR_TRANS_REVERSE 'WR', @p_rcv_id, 0, 999999, N'웨이퍼 입고 확정취소', @p_user_id, @p_client_pc;
            IF @lot_id IS NOT NULL
            BEGIN
                UPDATE TPRLOT SET init_qty = init_qty - @qty, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE lot_id = @lot_id;
                DELETE FROM TPRLOT WHERE lot_id = @lot_id AND wo_id IS NULL AND init_qty <= 0;
            END
            UPDATE TPRRCV SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, lot_id = NULL,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rcv_id = @p_rcv_id;
            COMMIT TRAN;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_rcv_id AS VARCHAR(20));
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

-- ============================================================
-- 5) USP_PR_WOLOTPICK_Q - 작업지시의 "입고 LOT 불러오기" 팝업(popPick 공통 파라미터 규약).
--    웨이퍼입고로 들어와 아직 어떤 작업지시에도 배정되지 않았고 재고가 남아 있는 LOT. 재고 있는 창고별로 한 행.
--    p_date_from/to는 LOT 등록일 기준, p_doc_no/p_keyword는 LOT번호/품번/품명.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WOLOTPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* LOT 번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명 */
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
        BEGIN
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.wh_id, w.wh_nm, s.stock_qty,
                   CONVERT(VARCHAR(8), l.reg_dt, 112) AS lot_date,
                   (SELECT TOP 1 c.cust_nm FROM TPRRCV r JOIN TBACUST c ON c.cust_id = r.sup_cust_id
                    WHERE r.acc_id = l.acc_id AND r.item_id = l.item_id AND r.lot_no = l.lot_no AND r.sup_cust_id IS NOT NULL ORDER BY r.rcv_id DESC) AS sup_cust_nm
            FROM TPRLOT l
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.loc_id = 0 AND s.stock_qty > 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TBAWH w ON w.wh_id = s.wh_id
            WHERE l.wo_id IS NULL
              AND (@p_acc_id IS NULL OR l.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR CONVERT(VARCHAR(8), l.reg_dt, 112) >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR CONVERT(VARCHAR(8), l.reg_dt, 112) <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR l.lot_no LIKE '%' + @p_doc_no + '%')
              AND (@p_keyword IS NULL OR @p_keyword = '' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY l.lot_no, w.wh_nm;
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
