-- 구매 프로세스 P4 - 입고(TMAGRM/D) 프로시저와 자동 입고(2026-09-25).
--
--   1) USP_MA_NEXTQTY_R            - next_qty 재계산 공용: POREQ/PO/DELV에 더해 IQC(검사 라인)와 무검사 DELV 라인이 확정된 입고수량 합을 갖게 한다
--   2) USP_MA_GR_Q / _S / _S_1     - 입고 조회 / 헤더 N,U,D / 라인 N,U,D (라인은 입고대기 불러오기로만)
--   3) USP_MA_GR_CONFIRM_CORE      - 확정 핵심(내부용): 원천 잠금+재검증 -> 수불 생성 -> stock_yn='Y'인 라인만 현재고 반영 -> 원천 next_qty 재계산
--   4) USP_MA_GR_CANCEL_CORE       - 확정취소 핵심(내부용): 역거래 수불 추가 -> 재고 차감(부족하면 취소 불가) -> next_qty 재계산
--   5) USP_MA_GR_C_S               - 화면용 확정(C)/확정취소(CC) 래퍼. 자동 생성 입고(auto_yn='Y')는 여기서 취소 못 한다
--   6) USP_MA_GR_AUTO_S / _CANCEL  - 입고방식(MA.GR_MODE)이 자동(A)일 때 납품/검사 확정이 부르는 자동 입고 생성/취소
--   7) USP_MA_GRLIST_Q             - 입고현황 Q/Q1
--   8) USP_MA_GRREADYPICK_Q        - frmGr "입고대기 불러오기" 팝업용(VMA_GR_READY의 remain>0)
--   9) USP_MA_STOCK_Q / USP_MA_TRANSLIST_Q - 재고현황 / 수불현황
--
-- 규칙(설계서 7장 인계 계약): 수불은 항상 생성하고, 현재고(TMASTOCK) 갱신은 라인 stock_yn='Y'일 때만 한다. 취소는 원 거래를 지우지 않고
-- 반대 방향(O)의 역거래를 추가한다(org_trans_id). 재고가 부족해서 되돌릴 수 없으면(이미 출고/이동됨) 확정취소가 거부된다.
-- 내부 프로시저는 오류를 THROW 50001(사용자에게 보여줄 메시지)로 올리고, 화면용 래퍼가 그 메시지를 ReturnMsg로 돌려준다.

-- ============================================================
-- 1) USP_MA_NEXTQTY_R
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_NEXTQTY_R
    @p_target VARCHAR(10),          /* 'POREQ' 구매요청 라인 / 'PO' 발주 라인 / 'DELV' 납품 라인 / 'IQC' 수입검사 라인 */
    @p_id BIGINT,                   /* POREQ: req_id, PO: po_id, DELV: delv_id, IQC: iqc_id */
    @p_serl INT = NULL              /* NULL이면 그 문서의 전체 라인 */
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_target = 'POREQ'
    BEGIN
        UPDATE d SET
            next_qty = ISNULL((
                SELECT SUM(po.qty)
                FROM TMAPOD po
                WHERE po.src_type = 'POREQ' AND po.src_id = d.req_id AND po.src_serl = d.serl
            ), 0)
        FROM TMAPOREQD d
        WHERE d.req_id = @p_id
          AND (@p_serl IS NULL OR d.serl = @p_serl);
    END
    ELSE IF @p_target = 'PO'
    BEGIN
        -- 확정된 납품수량 합 - 확정된 수입검사의 불합격 반품(RET) 수량 합(반품분은 납품으로 인정하지 않아 잔량이 복원된다)
        UPDATE p SET
            next_qty =
                ISNULL((SELECT SUM(dl.delv_qty)
                        FROM TMADELVD dl
                        JOIN TMADELVM dm ON dm.delv_id = dl.delv_id AND dm.cfm_yn = 'Y'
                        WHERE dl.src_type = 'PO' AND dl.src_id = p.po_id AND dl.src_serl = p.serl), 0)
              - ISNULL((SELECT SUM(iq.fail_qty)
                        FROM TMAIQCD iq
                        JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
                        JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                        WHERE dl.src_type = 'PO' AND dl.src_id = p.po_id AND dl.src_serl = p.serl
                          AND iq.fail_action_cd = 'RET'), 0)
        FROM TMAPOD p
        WHERE p.po_id = @p_id
          AND (@p_serl IS NULL OR p.serl = @p_serl);
    END
    ELSE IF @p_target = 'DELV'
    BEGIN
        -- 검사대상 라인: 확정된 수입검사의 검사수량 합
        UPDATE d SET
            next_qty = ISNULL((
                SELECT SUM(iq.insp_qty)
                FROM TMAIQCD iq
                JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
                WHERE iq.src_type = 'DELV' AND iq.src_id = d.delv_id AND iq.src_serl = d.serl
            ), 0)
        FROM TMADELVD d
        WHERE d.delv_id = @p_id
          AND (@p_serl IS NULL OR d.serl = @p_serl)
          AND ISNULL(d.qc_yn, 'N') = 'Y';

        -- 무검사 라인: 확정된 입고수량 합(다음 단계가 입고)
        UPDATE d SET
            next_qty = ISNULL((
                SELECT SUM(g.gr_qty)
                FROM TMAGRD g
                JOIN TMAGRM gm ON gm.gr_id = g.gr_id AND gm.cfm_yn = 'Y'
                WHERE g.src_type = 'DELV' AND g.src_id = d.delv_id AND g.src_serl = d.serl
            ), 0)
        FROM TMADELVD d
        WHERE d.delv_id = @p_id
          AND (@p_serl IS NULL OR d.serl = @p_serl)
          AND ISNULL(d.qc_yn, 'N') = 'N';
    END
    ELSE IF @p_target = 'IQC'
    BEGIN
        -- 검사 라인: 확정된 입고수량 합
        UPDATE q SET
            next_qty = ISNULL((
                SELECT SUM(g.gr_qty)
                FROM TMAGRD g
                JOIN TMAGRM gm ON gm.gr_id = g.gr_id AND gm.cfm_yn = 'Y'
                WHERE g.src_type = 'IQC' AND g.src_id = q.iqc_id AND g.src_serl = q.serl
            ), 0)
        FROM TMAIQCD q
        WHERE q.iqc_id = @p_id
          AND (@p_serl IS NULL OR q.serl = @p_serl);
    END
END
GO

-- ============================================================
-- 2) USP_MA_GR_Q - 헤더(0번) + 라인(1번). p_trans_type이 있으면 그 유형의 입고만(구매입고 화면=PU_IN)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
    @p_gr_no VARCHAR(20) = NULL,
    @p_trans_type VARCHAR(10) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = gr_id
            FROM TMAGRM
            WHERE (@p_gr_id IS NULL OR gr_id = @p_gr_id)
              AND (@p_gr_id IS NOT NULL OR @p_gr_no IS NULL OR gr_no LIKE '%' + @p_gr_no + '%')
              AND (@p_trans_type IS NULL OR @p_trans_type = '' OR trans_type = @p_trans_type)
            ORDER BY gr_id DESC;

            -- 0) 헤더
            SELECT
                m.gr_id, m.acc_id, a.ACC_NM,
                m.gr_no, m.gr_date, m.trans_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.auto_yn,
                m.remark
            FROM TMAGRM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.gr_id = @match_id;

            -- 1) 라인 (ready_qty/remain_qty는 입고대기 뷰 기준 - 미확정 작성본을 고칠 때 참고)
            SELECT
                dt.gr_id, dt.serl, dt.acc_id, dt.gr_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                dt.gr_qty, ISNULL(dt.next_qty, 0) AS next_qty, dt.lot_no,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.po_id, dt.po_no, dt.po_serl,
                v.ready_qty, v.remain_qty AS ready_remain_qty,
                dt.trans_id, dt.remark
            FROM TMAGRD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN dbo.VMA_GR_READY v ON v.src_type = dt.src_type AND v.src_id = dt.src_id AND v.src_serl = dt.src_serl
            WHERE dt.gr_id = @match_id
            ORDER BY dt.serl;
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
-- 3) USP_MA_GR_S - 헤더 N/U/D. 작성 상태에서만, 자동 생성 입고(auto_yn='Y')는 수정/삭제 불가.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_gr_date VARCHAR(8) = NULL,
    @p_trans_type VARCHAR(10) = NULL,       /* MA0011 중 입고 유형(rel_cd1='I'). 비우면 PU_IN */
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
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
            DECLARE @cur_stat VARCHAR(10), @cur_cust BIGINT, @cur_auto VARCHAR(1);
            SELECT @cur_stat = stat_cd, @cur_cust = cust_id, @cur_auto = auto_yn FROM TMAGRM WHERE gr_id = @p_gr_id;

            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 문서를 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_auto = 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'자동 생성된 입고는 수정하거나 삭제할 수 없습니다. 납품 또는 검사의 확정취소로만 취소됩니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 입고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_work_type = 'N' AND ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'PU_IN';
            IF @p_work_type = 'U' AND ISNULL(@p_trans_type, '') = '' SELECT @p_trans_type = trans_type FROM TMAGRM WHERE gr_id = @p_gr_id;

            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'I' AND use_yn = 'Y')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 수불유형이 올바르지 않습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAGRM', 'gr_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAGRM (
                acc_id, gr_no, gr_date, trans_type, cust_id, dept_id, emp_id, stat_cd, cfm_yn, auto_yn, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_gr_date, @p_trans_type, @p_cust_id, @p_dept_id, @p_emp_id, '0', 'N', 'N', @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_gr_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 라인이 있으면 거래처를 바꿀 수 없다(라인의 원천 거래처와 같아야 하므로)
            IF ISNULL(@p_cust_id, 0) <> ISNULL(@cur_cust, 0) AND EXISTS (SELECT 1 FROM TMAGRD WHERE gr_id = @p_gr_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'품목이 있는 입고는 거래처를 바꿀 수 없습니다. 품목을 지운 뒤 바꾸세요.'; RETURN;
            END

            UPDATE TMAGRM SET
                acc_id = @p_acc_id,
                gr_date = @p_gr_date,
                trans_type = @p_trans_type,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE gr_id = @p_gr_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAGRD WHERE gr_id = @p_gr_id;
            DELETE FROM TMAGRM WHERE gr_id = @p_gr_id;
        END

        SET @GeneratedCode = CAST(@p_gr_id AS VARCHAR(20));
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
-- 4) USP_MA_GR_S_1 - 라인 N/U/D. 라인은 입고대기(VMA_GR_READY)에서 "불러온" 것만 만들 수 있다. 품목/LOT/원천은 바꿀 수 없고
--    입고수량/창고/위치/비고만 바꾼다. 입고수량은 입고대기 잔량 이하(저장 때 한 번, 확정 때 잠그고 한 번 더 검증).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_gr_qty NUMERIC(18,4) = NULL,
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
        DECLARE @hdr_stat VARCHAR(10), @hdr_cust BIGINT, @hdr_auto VARCHAR(1), @hdr_acc BIGINT;
        SELECT @hdr_stat = stat_cd, @hdr_cust = cust_id, @hdr_auto = auto_yn, @hdr_acc = acc_id FROM TMAGRM WHERE gr_id = @p_gr_id;

        IF @hdr_stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'입고 문서를 찾을 수 없습니다.'; RETURN;
        END
        IF @hdr_auto = 'Y'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'자동 생성된 입고는 수정할 수 없습니다.'; RETURN;
        END
        IF @hdr_stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 입고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
        END

        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT;
        IF @p_work_type = 'N'
        BEGIN
            SET @src_type = @p_src_type; SET @src_id = @p_src_id; SET @src_serl = @p_src_serl;
        END
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl
            FROM TMAGRD WHERE gr_id = @p_gr_id AND serl = @p_serl;

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 품목을 찾을 수 없습니다.'; RETURN;
            END
        END

        DECLARE @v_item BIGINT, @v_unit VARCHAR(10), @v_lot NVARCHAR(50), @v_wh BIGINT, @v_loc BIGINT, @v_remain NUMERIC(18,4),
                @v_cust BIGINT, @v_stock VARCHAR(1), @v_po_id BIGINT, @v_po_no VARCHAR(20), @v_po_serl INT, @v_src_no VARCHAR(20);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_gr_qty IS NULL OR @p_gr_qty <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고수량은 0보다 커야 합니다.'; RETURN;
            END
            IF ISNULL(@src_type, '') NOT IN ('IQC', 'DELV') OR @src_id IS NULL OR @src_serl IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 품목은 입고대기 불러오기로만 추가할 수 있습니다.'; RETURN;
            END

            SELECT @v_item = item_id, @v_unit = unit_cd, @v_lot = lot_no, @v_wh = wh_id, @v_loc = loc_id, @v_remain = remain_qty,
                   @v_cust = cust_id, @v_stock = stock_yn, @v_po_id = po_id, @v_po_no = po_no, @v_po_serl = po_serl, @v_src_no = src_no
            FROM dbo.VMA_GR_READY
            WHERE src_type = @src_type AND src_id = @src_id AND src_serl = @src_serl AND acc_id = @hdr_acc;

            IF @v_item IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고대기 품목을 찾을 수 없습니다(확정된 납품/검사만 입고할 수 있습니다).'; RETURN;
            END
            IF @hdr_cust IS NOT NULL AND @v_cust <> @hdr_cust
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 거래처와 원천(납품/검사) 거래처가 다릅니다. (' + @v_src_no + N')'; RETURN;
            END
            IF @p_gr_qty > @v_remain
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'입고수량이 입고대기 잔량(' + CAST(CAST(@v_remain AS FLOAT) AS NVARCHAR(30)) + N')을 초과했습니다. (' + @v_src_no + N')';
                RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAGRD WHERE gr_id = @p_gr_id;

            INSERT INTO TMAGRD (
                gr_id, serl, acc_id, gr_no, item_id, unit_cd, gr_qty, next_qty, lot_no, wh_id, loc_id, stock_yn,
                src_type, src_id, src_no, src_serl, po_id, po_no, po_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.gr_id, @nextSerl, m.acc_id, m.gr_no, @v_item, @v_unit, @p_gr_qty, 0, @v_lot, ISNULL(@p_wh_id, @v_wh), ISNULL(@p_loc_id, @v_loc), ISNULL(@v_stock, 'N'),
                   @src_type, @src_id, @v_src_no, @src_serl, @v_po_id, @v_po_no, @v_po_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAGRM m WHERE m.gr_id = @p_gr_id;

            -- 헤더 거래처가 비어 있으면 첫 원천의 거래처로 채운다
            UPDATE TMAGRM SET cust_id = @v_cust WHERE gr_id = @p_gr_id AND cust_id IS NULL;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAGRD SET
                gr_qty = @p_gr_qty,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE gr_id = @p_gr_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAGRD WHERE gr_id = @p_gr_id AND serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
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
-- 5) USP_MA_GR_CONFIRM_CORE - 입고 확정 핵심(내부용, 오류는 THROW 50001)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_CONFIRM_CORE
    @p_gr_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @type VARCHAR(10), @cust BIGINT, @gr_no VARCHAR(20), @acc BIGINT, @gr_date VARCHAR(8);
    SELECT @stat = stat_cd, @type = trans_type, @cust = cust_id, @gr_no = gr_no, @acc = acc_id, @gr_date = gr_date
    FROM TMAGRM WHERE gr_id = @p_gr_id;

    IF @stat IS NULL THROW 50001, N'입고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'이미 확정된 입고입니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TMAGRD WHERE gr_id = @p_gr_id) THROW 50001, N'입고 품목이 없어 확정할 수 없습니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @type AND rel_cd1 = 'I' AND use_yn = 'Y')
        THROW 50001, N'입고 수불유형이 올바르지 않습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAGRD WHERE gr_id = @p_gr_id AND wh_id IS NULL)
        THROW 50001, N'입고 창고를 입력하지 않은 품목이 있습니다. (자동 입고라면 납품/검사 품목의 창고가 필요합니다)', 1;

    BEGIN TRAN;

    -- 원천(검사/납품 라인)을 잠그고 입고대기 잔량을 다시 검증한다. 같은 원천을 여러 줄이 쓰면 합산.
    DECLARE @lock INT;
    SELECT @lock = 1 FROM TMAIQCD q WITH (UPDLOCK, ROWLOCK)
    JOIN TMAGRD g ON g.src_type = 'IQC' AND g.src_id = q.iqc_id AND g.src_serl = q.serl WHERE g.gr_id = @p_gr_id;
    SELECT @lock = 1 FROM TMADELVD d WITH (UPDLOCK, ROWLOCK)
    JOIN TMAGRD g ON g.src_type = 'DELV' AND g.src_id = d.delv_id AND g.src_serl = d.serl WHERE g.gr_id = @p_gr_id;

    DECLARE @bad VARCHAR(20), @bad_kind INT;
    SELECT TOP 1 @bad = t.src_no, @bad_kind = CASE WHEN v.src_id IS NULL THEN 1 WHEN @cust IS NOT NULL AND v.cust_id <> @cust THEN 2 ELSE 3 END
    FROM (SELECT g.src_type, g.src_id, g.src_serl, MAX(g.src_no) AS src_no, SUM(g.gr_qty) AS this_qty
          FROM TMAGRD g WHERE g.gr_id = @p_gr_id GROUP BY g.src_type, g.src_id, g.src_serl) t
        LEFT JOIN dbo.VMA_GR_READY v ON v.src_type = t.src_type AND v.src_id = t.src_id AND v.src_serl = t.src_serl AND v.acc_id = @acc
    WHERE v.src_id IS NULL
       OR (@cust IS NOT NULL AND v.cust_id <> @cust)
       OR t.this_qty > v.remain_qty;
    IF @bad IS NOT NULL
    BEGIN
        SET @msg = CASE @bad_kind WHEN 1 THEN N'입고대기에서 찾을 수 없는(확정 취소되었거나 이미 처리된) 원천이 있습니다. (' + @bad + N')'
                                  WHEN 2 THEN N'입고 거래처와 원천 거래처가 다른 품목이 있습니다. (' + @bad + N')'
                                  ELSE N'입고수량이 입고대기 잔량을 초과한 품목이 있습니다. (' + @bad + N') 다른 입고가 먼저 확정되었을 수 있습니다.' END;
        THROW 50001, @msg, 1;
    END

    -- 라인별: 수불 생성 + (재고관리 품목만) 현재고 반영
    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @src_no VARCHAR(20), @tid BIGINT;
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, gr_qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn, src_no FROM TMAGRD WHERE gr_id = @p_gr_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @src_no;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO TMATRANS (
            acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
            src_type, src_id, src_no, src_serl, org_trans_id, remark,
            reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
        )
        VALUES (
            @acc, ISNULL(@gr_date, CONVERT(VARCHAR(8), GETDATE(), 112)), 'I', @type, @item, @wh, @loc, @lot, @unit, @qty, @stock, @cust,
            'GR', @p_gr_id, @gr_no, @serl, NULL, NULL,
            @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
        );
        SET @tid = SCOPE_IDENTITY();

        UPDATE TMAGRD SET trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE gr_id = @p_gr_id AND serl = @serl;

        IF @stock = 'Y'
        BEGIN
            UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET
                stock_qty = stock_qty + @qty, trans_id = @tid, unit_cd = ISNULL(@unit, unit_cd),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;

            IF @@ROWCOUNT = 0
                INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @item, @wh, @loc, @lot, @unit, @qty, @tid, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END

        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @src_no;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAGRM SET
        stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
        upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE gr_id = @p_gr_id;

    -- 원천 라인 next_qty 재계산
    DECLARE @s_type VARCHAR(10), @s_id BIGINT, @s_serl INT;
    DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
        SELECT DISTINCT src_type, src_id, src_serl FROM TMAGRD WHERE gr_id = @p_gr_id;
    OPEN c2;
    FETCH NEXT FROM c2 INTO @s_type, @s_id, @s_serl;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @s_type = 'IQC' EXEC USP_MA_NEXTQTY_R 'IQC', @s_id, @s_serl;
        ELSE IF @s_type = 'DELV' EXEC USP_MA_NEXTQTY_R 'DELV', @s_id, @s_serl;
        FETCH NEXT FROM c2 INTO @s_type, @s_id, @s_serl;
    END
    CLOSE c2;
    DEALLOCATE c2;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 6) USP_MA_GR_CANCEL_CORE - 입고 확정취소 핵심(내부용). 역거래(O) 수불을 추가하고 재고관리 품목은 현재고를 차감한다.
--    현재고가 모자라면(이미 출고/이동됨) 취소할 수 없다. 후속 처리(매입 등)된 라인이 있어도 불가.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_CANCEL_CORE
    @p_gr_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @type VARCHAR(10), @cust BIGINT, @gr_no VARCHAR(20), @acc BIGINT, @gr_date VARCHAR(8);
    SELECT @stat = stat_cd, @type = trans_type, @cust = cust_id, @gr_no = gr_no, @acc = acc_id, @gr_date = gr_date
    FROM TMAGRM WHERE gr_id = @p_gr_id;

    IF @stat IS NULL THROW 50001, N'입고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> 'C' THROW 50001, N'확정된 입고만 확정취소할 수 있습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAGRD WHERE gr_id = @p_gr_id AND ISNULL(next_qty, 0) > 0)
        THROW 50001, N'매입 등 후속 처리가 진행된 입고는 확정취소할 수 없습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1),
            @orig_tid BIGINT, @tid BIGINT, @cur NUMERIC(18,4), @item_nm NVARCHAR(200);
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, gr_qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn, trans_id FROM TMAGRD WHERE gr_id = @p_gr_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig_tid;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @stock = 'Y'
        BEGIN
            SELECT @cur = stock_qty FROM TMASTOCK WITH (UPDLOCK, HOLDLOCK)
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;

            IF ISNULL(@cur, 0) < @qty
            BEGIN
                SELECT @item_nm = item_nm FROM TBAITEM WHERE item_id = @item;
                SET @msg = N'현재고가 부족해 입고를 취소할 수 없습니다(이미 출고/이동된 재고). 품목: ' + ISNULL(@item_nm, CAST(@item AS NVARCHAR(20)))
                         + N', 현재고 ' + CAST(CAST(ISNULL(@cur, 0) AS FLOAT) AS NVARCHAR(30)) + N' < 취소수량 ' + CAST(CAST(@qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @msg, 1;
            END
        END

        INSERT INTO TMATRANS (
            acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
            src_type, src_id, src_no, src_serl, org_trans_id, remark,
            reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
        )
        VALUES (
            @acc, CONVERT(VARCHAR(8), GETDATE(), 112), 'O', @type, @item, @wh, @loc, @lot, @unit, @qty, @stock, @cust,
            'GR', @p_gr_id, @gr_no, @serl, @orig_tid, N'입고 확정취소',
            @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
        );
        SET @tid = SCOPE_IDENTITY();

        IF @stock = 'Y'
            UPDATE TMASTOCK SET
                stock_qty = stock_qty - @qty, trans_id = @tid,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;

        UPDATE TMAGRD SET trans_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE gr_id = @p_gr_id AND serl = @serl;

        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig_tid;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAGRM SET
        stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
        upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE gr_id = @p_gr_id;

    DECLARE @s_type VARCHAR(10), @s_id BIGINT, @s_serl INT;
    DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
        SELECT DISTINCT src_type, src_id, src_serl FROM TMAGRD WHERE gr_id = @p_gr_id;
    OPEN c2;
    FETCH NEXT FROM c2 INTO @s_type, @s_id, @s_serl;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @s_type = 'IQC' EXEC USP_MA_NEXTQTY_R 'IQC', @s_id, @s_serl;
        ELSE IF @s_type = 'DELV' EXEC USP_MA_NEXTQTY_R 'DELV', @s_id, @s_serl;
        FETCH NEXT FROM c2 INTO @s_type, @s_id, @s_serl;
    END
    CLOSE c2;
    DEALLOCATE c2;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 7) USP_MA_GR_C_S - 화면용 확정(C)/확정취소(CC) 래퍼
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
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
        IF @p_work_type = 'C'
            EXEC USP_MA_GR_CONFIRM_CORE @p_gr_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF EXISTS (SELECT 1 FROM TMAGRM WHERE gr_id = @p_gr_id AND auto_yn = 'Y')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'자동 생성된 입고는 납품 또는 검사의 확정취소로만 취소할 수 있습니다.'; RETURN;
            END
            EXEC USP_MA_GR_CANCEL_CORE @p_gr_id, @p_user_id, @p_client_pc;
        END

        SET @GeneratedCode = CAST(@p_gr_id AS VARCHAR(20));
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
-- 8) USP_MA_GR_AUTO_S - 자동 입고 생성+확정(내부용). 원천(납품 또는 검사) 문서의 입고대기 행을 전부 입고로 만든다.
--    검사면제 납품이면 그 납품의 무검사 라인, 검사면 그 검사의 합격+특채분. 호출한 납품/검사 확정 트랜잭션 안에서 실행된다.
--    창고가 없는 라인이 있으면 THROW 50001 - 호출한 확정 전체가 롤백된다(설계서 3장 자동 처리 규칙).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_AUTO_S
    @p_src_type VARCHAR(10),                /* DELV / IQC */
    @p_src_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @rows TABLE (rn INT IDENTITY(1,1), src_serl INT, src_no VARCHAR(20), acc_id BIGINT, cust_id BIGINT, item_id BIGINT, unit_cd VARCHAR(10),
                         lot_no NVARCHAR(50), wh_id BIGINT, loc_id BIGINT, remain_qty NUMERIC(18,4), stock_yn VARCHAR(1),
                         po_id BIGINT, po_no VARCHAR(20), po_serl INT, item_nm NVARCHAR(200));
    INSERT INTO @rows (src_serl, src_no, acc_id, cust_id, item_id, unit_cd, lot_no, wh_id, loc_id, remain_qty, stock_yn, po_id, po_no, po_serl, item_nm)
    SELECT v.src_serl, v.src_no, v.acc_id, v.cust_id, v.item_id, v.unit_cd, v.lot_no, v.wh_id, v.loc_id, v.remain_qty, v.stock_yn, v.po_id, v.po_no, v.po_serl, v.item_nm
    FROM dbo.VMA_GR_READY v
    WHERE v.src_type = @p_src_type AND v.src_id = @p_src_id AND v.remain_qty > 0
    ORDER BY v.src_serl;

    IF NOT EXISTS (SELECT 1 FROM @rows) RETURN;   -- 입고할 것이 없으면(불합격 전량 등) 조용히 끝

    DECLARE @msg NVARCHAR(2048), @bad_nm NVARCHAR(200);
    SELECT TOP 1 @bad_nm = item_nm FROM @rows WHERE wh_id IS NULL;
    IF @bad_nm IS NOT NULL
    BEGIN
        SET @msg = N'입고방식이 자동이면 납품/검사 품목에 입고 창고가 있어야 합니다. 창고를 입력한 뒤 확정하세요. (품목: ' + @bad_nm + N')';
        THROW 50001, @msg, 1;
    END

    DECLARE @acc BIGINT, @cust BIGINT, @dept BIGINT, @emp BIGINT, @gr_no VARCHAR(20), @gr_id BIGINT;
    SELECT TOP 1 @acc = acc_id, @cust = cust_id FROM @rows;
    IF @p_src_type = 'DELV' SELECT @dept = dept_id, @emp = emp_id FROM TMADELVM WHERE delv_id = @p_src_id;
    ELSE SELECT @dept = dept_id, @emp = emp_id FROM TMAIQCM WHERE iqc_id = @p_src_id;

    BEGIN TRAN;

    EXEC SSP_SYS_GetAutoKey 'TMAGRM', 'gr_no', @acc, @gr_no OUTPUT;
    INSERT INTO TMAGRM (acc_id, gr_no, gr_date, trans_type, cust_id, dept_id, emp_id, stat_cd, cfm_yn, auto_yn, remark,
                        reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    VALUES (@acc, @gr_no, CONVERT(VARCHAR(8), GETDATE(), 112), 'PU_IN', @cust, @dept, @emp, '0', 'N', 'Y', N'자동 입고',
            @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
    SET @gr_id = SCOPE_IDENTITY();

    INSERT INTO TMAGRD (gr_id, serl, acc_id, gr_no, item_id, unit_cd, gr_qty, next_qty, lot_no, wh_id, loc_id, stock_yn,
                        src_type, src_id, src_no, src_serl, po_id, po_no, po_serl,
                        reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @gr_id, r.rn, @acc, @gr_no, r.item_id, r.unit_cd, r.remain_qty, 0, r.lot_no, r.wh_id, r.loc_id, ISNULL(r.stock_yn, 'N'),
           @p_src_type, @p_src_id, r.src_no, r.src_serl, r.po_id, r.po_no, r.po_serl,
           @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM @rows r;

    EXEC USP_MA_GR_CONFIRM_CORE @gr_id, @p_user_id, @p_client_pc;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 9) USP_MA_GR_AUTO_CANCEL - 원천(납품/검사)이 확정취소될 때 그 원천으로 자동 생성된 입고를 먼저 취소하고 지운다(내부용).
--    입고 쪽 취소 조건(재고 부족 등)에 걸리면 THROW 50001 - 호출한 확정취소 전체가 롤백된다. 수불 원장에는 역거래가 남는다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_AUTO_CANCEL
    @p_src_type VARCHAR(10),                /* DELV / IQC */
    @p_src_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @gr_id BIGINT;
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT DISTINCT m.gr_id FROM TMAGRM m JOIN TMAGRD d ON d.gr_id = m.gr_id
        WHERE m.auto_yn = 'Y' AND d.src_type = @p_src_type AND d.src_id = @p_src_id;
    OPEN c;
    FETCH NEXT FROM c INTO @gr_id;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS (SELECT 1 FROM TMAGRM WHERE gr_id = @gr_id AND stat_cd = 'C')
            EXEC USP_MA_GR_CANCEL_CORE @gr_id, @p_user_id, @p_client_pc;

        DELETE FROM TMAGRD WHERE gr_id = @gr_id;
        DELETE FROM TMAGRM WHERE gr_id = @gr_id;
        FETCH NEXT FROM c INTO @gr_id;
    END
    CLOSE c;
    DEALLOCATE c;
END
GO

-- ============================================================
-- 10) USP_MA_GRLIST_Q - 입고현황. Q: 헤더 목록, Q1: 한 입고의 라인
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GRLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
    @p_gr_no VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 거래처명 / 품번 / 품명 */
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_trans_type VARCHAR(10) = NULL,
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
            SELECT
                m.gr_id, m.gr_no, m.gr_date, m.trans_type, m.stat_cd, m.auto_yn,
                m.cust_id, c.cust_nm, d.dept_nm, e.emp_nm, m.cfm_dt,
                (SELECT COUNT(*) FROM TMAGRD x WHERE x.gr_id = m.gr_id) AS line_cnt,
                (SELECT ISNULL(SUM(x.gr_qty), 0) FROM TMAGRD x WHERE x.gr_id = m.gr_id) AS total_qty,
                m.remark
            FROM TMAGRM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_gr_no IS NULL OR @p_gr_no = '' OR m.gr_no LIKE '%' + @p_gr_no + '%')
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.gr_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.gr_date <= @p_date_to)
              AND (@p_trans_type IS NULL OR @p_trans_type = '' OR m.trans_type = @p_trans_type)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR c.cust_nm LIKE '%' + @p_keyword + '%'
                   OR EXISTS (SELECT 1 FROM TMAGRD x JOIN TBAITEM i ON i.item_id = x.item_id
                              WHERE x.gr_id = m.gr_id
                                AND (i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')))
            ORDER BY m.gr_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.gr_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                dt.gr_qty, ISNULL(dt.next_qty, 0) AS next_qty, dt.lot_no, w.wh_nm, l.loc_nm, dt.stock_yn,
                dt.src_type, dt.src_no, dt.po_no, dt.remark
            FROM TMAGRD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.gr_id = @p_gr_id
            ORDER BY dt.serl;
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
-- 11) USP_MA_GRREADYPICK_Q - "입고대기 불러오기" 팝업. VMA_GR_READY 중 잔량이 있는 행(팝업 공통 파라미터 규약).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GRREADYPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 납품번호 또는 검사번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명/규격 */
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
            SELECT
                v.src_type, v.src_id, v.src_serl, v.src_no,
                CASE v.src_type WHEN 'IQC' THEN N'수입검사' ELSE N'납품(검사면제)' END AS src_nm,
                CONVERT(VARCHAR(8), v.ready_date, 112) AS ready_date,
                v.cust_id, v.cust_nm, v.item_id, v.item_no, v.item_nm, v.item_spec, v.unit_cd, v.lot_no,
                v.ready_qty, v.next_qty, v.remain_qty,
                v.wh_id, w.wh_nm, v.loc_id, l.loc_nm, v.stock_yn,
                v.po_id, v.po_no, v.po_serl
            FROM dbo.VMA_GR_READY v
                LEFT JOIN TBAWH w ON w.wh_id = v.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = v.loc_id
            WHERE v.remain_qty > 0
              AND (@p_acc_id IS NULL OR v.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR CONVERT(VARCHAR(8), v.ready_date, 112) >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR CONVERT(VARCHAR(8), v.ready_date, 112) <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR v.src_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR v.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR v.item_no LIKE '%' + @p_keyword + '%'
                   OR v.item_nm LIKE '%' + @p_keyword + '%'
                   OR v.item_spec LIKE '%' + @p_keyword + '%')
            ORDER BY v.ready_date, v.src_no, v.src_serl;
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
-- 12) USP_MA_STOCK_Q - 재고현황(TMASTOCK). p_zero_yn='Y'면 재고 0인 행도 보여준다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_STOCK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명/규격 */
    @p_wh_keyword NVARCHAR(100) = NULL,     /* 창고명 */
    @p_lot_no NVARCHAR(50) = NULL,
    @p_zero_yn VARCHAR(1) = 'N',
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
            SELECT
                s.acc_id, a.ACC_NM, s.item_id, i.item_no, i.item_nm, i.item_spec,
                s.wh_id, w.wh_nm, s.loc_id, l.loc_nm, s.lot_no, s.unit_cd, s.stock_qty,
                s.trans_id, s.upt_dt
            FROM TMASTOCK s
                LEFT JOIN TBAACC a ON a.ACC_ID = s.acc_id
                LEFT JOIN TBAITEM i ON i.item_id = s.item_id
                LEFT JOIN TBAWH w ON w.wh_id = s.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = s.loc_id
            WHERE (@p_acc_id IS NULL OR s.acc_id = @p_acc_id)
              AND (@p_zero_yn = 'Y' OR s.stock_qty <> 0)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%' OR i.item_spec LIKE '%' + @p_keyword + '%')
              AND (@p_wh_keyword IS NULL OR @p_wh_keyword = '' OR w.wh_nm LIKE '%' + @p_wh_keyword + '%')
              AND (@p_lot_no IS NULL OR @p_lot_no = '' OR s.lot_no LIKE '%' + @p_lot_no + '%')
            ORDER BY i.item_no, w.wh_nm, s.loc_id, s.lot_no;
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
-- 13) USP_MA_TRANSLIST_Q - 수불현황(TMATRANS 원장). signed_qty는 입고 +, 출고 -(역거래 포함).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_TRANSLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명/규격 */
    @p_wh_keyword NVARCHAR(100) = NULL,
    @p_trans_kind VARCHAR(1) = NULL,        /* I / O */
    @p_trans_type VARCHAR(10) = NULL,
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
            SELECT
                t.trans_id, t.trans_date, t.trans_kind, t.trans_type,
                i.item_id, i.item_no, i.item_nm, i.item_spec,
                w.wh_nm, l.loc_nm, t.lot_no, t.unit_cd, t.qty,
                CASE t.trans_kind WHEN 'I' THEN t.qty ELSE -t.qty END AS signed_qty,
                t.stock_yn, c.cust_nm, t.src_type, t.src_no, t.src_serl,
                CASE WHEN t.org_trans_id IS NOT NULL THEN 'Y' ELSE 'N' END AS reversal_yn, t.org_trans_id,
                t.remark, t.reg_user_id, t.reg_dt
            FROM TMATRANS t
                LEFT JOIN TBAITEM i ON i.item_id = t.item_id
                LEFT JOIN TBAWH w ON w.wh_id = t.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = t.loc_id
                LEFT JOIN TBACUST c ON c.cust_id = t.cust_id
            WHERE (@p_acc_id IS NULL OR t.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR t.trans_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR t.trans_date <= @p_date_to)
              AND (@p_trans_kind IS NULL OR @p_trans_kind = '' OR t.trans_kind = @p_trans_kind)
              AND (@p_trans_type IS NULL OR @p_trans_type = '' OR t.trans_type = @p_trans_type)
              AND (@p_wh_keyword IS NULL OR @p_wh_keyword = '' OR w.wh_nm LIKE '%' + @p_wh_keyword + '%')
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%' OR i.item_spec LIKE '%' + @p_keyword + '%')
            ORDER BY t.trans_id DESC;
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
-- 14) 납품/수입검사 확정 프로시저에 자동 입고 연결 (191/192번 정의에 자동 입고 호출과 오류 메시지 전달을 더한 것)
-- ============================================================
-- USP_MA_DELV_C_S - 확정(C) / 확정취소(CC)
--    확정: 라인이 1개 이상, 발주 라인(승인/마감/거래처/누적 수량)을 UPDLOCK으로 잠그고 다시 검증한 뒤 stat_cd='C',
--          발주 라인 next_qty 재계산.
--    확정취소: 검사 또는 입고가 진행된 라인(next_qty>0)이 있으면 불가. stat_cd='0'으로 되돌리고 재계산.
--    입고방식(MA.GR_MODE)이 자동(A)이면 확정 직후 검사면제 라인이 자동 입고되고, 확정취소는 그 자동 입고를 먼저 취소한다(P4, 199번).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELV_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_delv_id BIGINT = NULL,
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
        DECLARE @stat VARCHAR(10), @cust BIGINT;
        SELECT @stat = stat_cd, @cust = cust_id FROM TMADELVM WHERE delv_id = @p_delv_id;

        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'납품 문서를 찾을 수 없습니다.'; RETURN;
        END

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 확정된 납품입니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TMADELVD WHERE delv_id = @p_delv_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 품목이 없어 확정할 수 없습니다.'; RETURN;
            END

            BEGIN TRAN;

            -- 이 납품이 참조하는 발주 라인을 잠그고 그 상태를 한 번에 읽는다(같은 발주 라인을 여러 줄이 쓰면 합산)
            DECLARE @chk TABLE (po_id BIGINT, serl INT, po_no VARCHAR(20), this_qty NUMERIC(18,4), po_qty NUMERIC(18,4),
                                po_next NUMERIC(18,4), stop_yn VARCHAR(1), stat_cd VARCHAR(10), po_cust BIGINT);
            INSERT INTO @chk (po_id, serl, po_no, this_qty, po_qty, po_next, stop_yn, stat_cd, po_cust)
            SELECT pd.po_id, pd.serl, pm.po_no, SUM(dl.delv_qty), MAX(pd.qty), MAX(ISNULL(pd.next_qty, 0)),
                   MAX(ISNULL(pd.stop_yn, 'N')), MAX(pm.stat_cd), MAX(pm.cust_id)
            FROM TMADELVD dl
                JOIN TMAPOD pd WITH (UPDLOCK, ROWLOCK) ON pd.po_id = dl.src_id AND pd.serl = dl.src_serl
                JOIN TMAPOM pm ON pm.po_id = pd.po_id
            WHERE dl.delv_id = @p_delv_id AND dl.src_type = 'PO'
            GROUP BY pd.po_id, pd.serl, pm.po_no;

            IF (SELECT COUNT(*) FROM TMADELVD WHERE delv_id = @p_delv_id) <> (SELECT COUNT(*) FROM TMADELVD dl JOIN TMAPOD pd ON pd.po_id = dl.src_id AND pd.serl = dl.src_serl WHERE dl.delv_id = @p_delv_id AND dl.src_type = 'PO')
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 발주 품목을 찾을 수 없는 납품 품목이 있습니다.'; RETURN;
            END

            DECLARE @bad_no VARCHAR(20);
            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE ISNULL(stat_cd, '') <> 'C';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료되지 않은 발주가 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE stop_yn = 'Y';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'마감된 발주 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE po_cust <> @cust;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 거래처와 발주 거래처가 다른 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            DECLARE @pct NUMERIC(9,4) = ISNULL(TRY_CAST(dbo.FSM_PROCCONFIG('MA.OVER_DELV_PCT') AS NUMERIC(9,4)), 0);
            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE this_qty > po_qty * (1 + @pct / 100.0) - po_next;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품수량이 발주 잔량을 초과한 품목이 있습니다. (' + @bad_no + N') 다른 납품이 먼저 확정되었을 수 있습니다.'; RETURN;
            END

            UPDATE TMADELVM SET
                stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE delv_id = @p_delv_id;

            DECLARE @po BIGINT, @ps INT;
            DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT po_id, serl FROM @chk;
            OPEN c;
            FETCH NEXT FROM c INTO @po, @ps;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'PO', @po, @ps;
                FETCH NEXT FROM c INTO @po, @ps;
            END
            CLOSE c;
            DEALLOCATE c;

            -- 입고방식이 자동(A)이면 검사면제 라인을 바로 입고한다(창고가 없으면 THROW로 이 확정 전체가 롤백된다)
            IF ISNULL(dbo.FSM_PROCCONFIG('MA.GR_MODE'), 'M') = 'A'
                EXEC USP_MA_GR_AUTO_S 'DELV', @p_delv_id, @p_user_id, @p_client_pc;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 납품만 확정취소할 수 있습니다.'; RETURN;
            END
            BEGIN TRAN;

            -- 이 납품으로 자동 생성된 입고가 있으면 먼저 취소(수불에는 역거래가 남는다). 수동으로 만든 입고/검사가 있으면 남은 next_qty로 걸러진다.
            EXEC USP_MA_GR_AUTO_CANCEL 'DELV', @p_delv_id, @p_user_id, @p_client_pc;
            EXEC USP_MA_NEXTQTY_R 'DELV', @p_delv_id, NULL;

            IF EXISTS (SELECT 1 FROM TMADELVD WHERE delv_id = @p_delv_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 또는 입고가 진행된 납품은 확정취소할 수 없습니다.'; RETURN;
            END

            UPDATE TMADELVM SET
                stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE delv_id = @p_delv_id;

            DECLARE @po2 BIGINT, @ps2 INT;
            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
                SELECT DISTINCT src_id, src_serl FROM TMADELVD WHERE delv_id = @p_delv_id AND src_type = 'PO';
            OPEN c2;
            FETCH NEXT FROM c2 INTO @po2, @ps2;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'PO', @po2, @ps2;
                FETCH NEXT FROM c2 INTO @po2, @ps2;
            END
            CLOSE c2;
            DEALLOCATE c2;

            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_delv_id AS VARCHAR(20));
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


-- USP_MA_IQC_C_S - 확정(C) / 확정취소(CC)
--    확정: 납품 라인(UPDLOCK)을 다시 검증(확정 납품/검사대상/거래처/누적 미검사 잔량)한 뒤 stat_cd='C',
--          납품 라인 next_qty와 발주 라인 next_qty(불합격 반품분 복원)를 재계산.
--    확정취소: 입고가 진행된 라인(next_qty>0)이 있으면 불가.
--    입고방식(MA.GR_MODE)이 자동(A)이면 확정 직후 합격+특채분이 자동 입고되고, 확정취소는 그 자동 입고를 먼저 취소한다(P4, 199번).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQC_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_iqc_id BIGINT = NULL,
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
        DECLARE @stat VARCHAR(10), @cust BIGINT;
        SELECT @stat = stat_cd, @cust = cust_id FROM TMAIQCM WHERE iqc_id = @p_iqc_id;

        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 문서를 찾을 수 없습니다.'; RETURN;
        END

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 확정된 수입검사입니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TMAIQCD WHERE iqc_id = @p_iqc_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사 품목이 없어 확정할 수 없습니다.'; RETURN;
            END

            BEGIN TRAN;

            DECLARE @chk TABLE (delv_id BIGINT, serl INT, delv_no VARCHAR(20), this_qty NUMERIC(18,4), delv_qty NUMERIC(18,4),
                                delv_next NUMERIC(18,4), qc_yn VARCHAR(1), stop_yn VARCHAR(1), stat_cd VARCHAR(10), dm_cust BIGINT,
                                po_id BIGINT, po_serl INT);
            INSERT INTO @chk (delv_id, serl, delv_no, this_qty, delv_qty, delv_next, qc_yn, stop_yn, stat_cd, dm_cust, po_id, po_serl)
            SELECT dl.delv_id, dl.serl, dm.delv_no, SUM(iq.insp_qty), MAX(dl.delv_qty), MAX(ISNULL(dl.next_qty, 0)),
                   MAX(ISNULL(dl.qc_yn, 'N')), MAX(ISNULL(dl.stop_yn, 'N')), MAX(dm.stat_cd), MAX(dm.cust_id),
                   MAX(CASE WHEN dl.src_type = 'PO' THEN dl.src_id END), MAX(CASE WHEN dl.src_type = 'PO' THEN dl.src_serl END)
            FROM TMAIQCD iq
                JOIN TMADELVD dl WITH (UPDLOCK, ROWLOCK) ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                JOIN TMADELVM dm ON dm.delv_id = dl.delv_id
            WHERE iq.iqc_id = @p_iqc_id
            GROUP BY dl.delv_id, dl.serl, dm.delv_no;

            IF (SELECT COUNT(*) FROM TMAIQCD WHERE iqc_id = @p_iqc_id) <> (SELECT COUNT(*) FROM TMAIQCD iq JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl WHERE iq.iqc_id = @p_iqc_id)
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 납품 품목을 찾을 수 없는 검사 품목이 있습니다.'; RETURN;
            END

            DECLARE @bad_no VARCHAR(20);
            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE ISNULL(stat_cd, '') <> 'C';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정되지 않은(또는 확정취소된) 납품이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE qc_yn <> 'Y' OR stop_yn = 'Y';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사대상이 아니거나 마감된 납품 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE dm_cust <> @cust;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사 거래처와 납품 거래처가 다른 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE this_qty > delv_qty - delv_next;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사수량이 납품 미검사 잔량을 초과한 품목이 있습니다. (' + @bad_no + N') 다른 검사가 먼저 확정되었을 수 있습니다.'; RETURN;
            END

            UPDATE TMAIQCM SET
                stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE iqc_id = @p_iqc_id;

            DECLARE @d BIGINT, @ds INT, @p BIGINT, @ps INT;
            DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT delv_id, serl, po_id, po_serl FROM @chk;
            OPEN c;
            FETCH NEXT FROM c INTO @d, @ds, @p, @ps;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'DELV', @d, @ds;
                IF @p IS NOT NULL EXEC USP_MA_NEXTQTY_R 'PO', @p, @ps;
                FETCH NEXT FROM c INTO @d, @ds, @p, @ps;
            END
            CLOSE c;
            DEALLOCATE c;

            -- 입고방식이 자동(A)이면 합격+특채분을 바로 입고한다(창고가 없으면 THROW로 이 확정 전체가 롤백된다)
            IF ISNULL(dbo.FSM_PROCCONFIG('MA.GR_MODE'), 'M') = 'A'
                EXEC USP_MA_GR_AUTO_S 'IQC', @p_iqc_id, @p_user_id, @p_client_pc;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 수입검사만 확정취소할 수 있습니다.'; RETURN;
            END
            BEGIN TRAN;

            -- 이 검사로 자동 생성된 입고가 있으면 먼저 취소. 수동으로 만든 입고가 있으면 남은 next_qty로 걸러진다.
            EXEC USP_MA_GR_AUTO_CANCEL 'IQC', @p_iqc_id, @p_user_id, @p_client_pc;
            EXEC USP_MA_NEXTQTY_R 'IQC', @p_iqc_id, NULL;

            IF EXISTS (SELECT 1 FROM TMAIQCD WHERE iqc_id = @p_iqc_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고가 진행된 수입검사는 확정취소할 수 없습니다.'; RETURN;
            END

            UPDATE TMAIQCM SET
                stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE iqc_id = @p_iqc_id;

            DECLARE @d2 BIGINT, @ds2 INT, @p2 BIGINT, @ps2 INT;
            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
                SELECT DISTINCT dl.delv_id, dl.serl, CASE WHEN dl.src_type = 'PO' THEN dl.src_id END, CASE WHEN dl.src_type = 'PO' THEN dl.src_serl END
                FROM TMAIQCD iq JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                WHERE iq.iqc_id = @p_iqc_id;
            OPEN c2;
            FETCH NEXT FROM c2 INTO @d2, @ds2, @p2, @ps2;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'DELV', @d2, @ds2;
                IF @p2 IS NOT NULL EXEC USP_MA_NEXTQTY_R 'PO', @p2, @ps2;
                FETCH NEXT FROM c2 INTO @d2, @ds2, @p2, @ps2;
            END
            CLOSE c2;
            DEALLOCATE c2;

            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_iqc_id AS VARCHAR(20));
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

