-- 생산관리(PR) 공정실적 / 외주이전 프로시저 (2026-09-29). 223~225번 테이블 위에서 동작한다.
--
--  USP_PR_TRANS_POST     수불 1건 기록 + 현재고 반영(출고는 현재고 부족이면 거부). 실적/이전이 공통으로 호출(내부용).
--  USP_PR_TRANS_REVERSE  문서의 수불을 역거래로 되돌림(내부용). 원 거래를 지우지 않고 반대 방향 행을 추가(org_trans_id).
--  USP_PR_RSLT_Q / _S / _S_1 / _CONFIRM_CORE / _CANCEL_CORE / _C_S   공정 실적(조회 / 헤더 / EDS 웨이퍼 라인 / 확정 / 취소 / 화면 래퍼)
--  USP_PR_XFER_Q / _S / _S_1 / _C_S / _DIFF_S                       외주 이전(조회 / 헤더 / LOT 라인 / 출발·도착 확인 / 차이 처리)
--
-- 수불 src_type/src_id/src_no/src_serl 규칙
--   RS(공정실적): src_serl 1 = 투입 LOT 소진(PR_OUT), 101~ = 산출 LOT 입고(PR_IN, LOT 순번).
--   XF(외주이전): 라인 serl = s 일 때  출발 s*10+1(MV_OUT 출발창고)/s*10+2(MV_IN 이동중)
--                                       도착 1000+s*10+1(MV_OUT 이동중)/+2(MV_IN 도착창고)
--                                       차이 2000+s*10+1/+2. 범위(0~999 / 1000~1999 / 2000~2999)로 단계별 취소한다.
--
-- LOT 이름 규칙(산출 LOT): <뿌리>-<공정코드>[<투입 LOT 끝 숫자>][<분할 순번 2자리>]
--   뿌리 = 작업지시의 시작 LOT 번호(웨이퍼 LOT 번호에 '-'가 있어도 됨). 예) W123 -BUMP-> W123-BUMP -EDS-> W123-EDS -PKG(5000개씩)-> W123-PKG01~05 -FT-> W123-FT01~05
--   같은 산출 LOT가 이미 있으면(같은 투입 LOT를 나눠서 실적 등록) 그 LOT에 수량을 더한다.
--   ponytail: 분할 순번은 2자리(99개 초과 분할 시 자릿수가 늘어 정렬이 깨짐). 필요해지면 자릿수를 늘린다.

-- ============================================================
-- 1) USP_PR_TRANS_POST - 수불 1건 + 현재고(내부용). 위치(loc_id)는 쓰지 않는다(0).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_TRANS_POST
    @p_acc_id BIGINT,
    @p_kind VARCHAR(1),                     /* I 입고 / O 출고 */
    @p_type VARCHAR(10),                    /* 수불유형 MA0011 */
    @p_item_id BIGINT,
    @p_wh_id BIGINT,
    @p_lot_no NVARCHAR(50),
    @p_qty NUMERIC(18,4),
    @p_src_type VARCHAR(10),
    @p_src_id BIGINT,
    @p_src_no VARCHAR(20),
    @p_src_serl INT,
    @p_org_trans_id BIGINT,
    @p_remark NVARCHAR(3000),
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200),
    @p_trans_date VARCHAR(8),
    @o_trans_id BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stock VARCHAR(1), @unit VARCHAR(10), @nm NVARCHAR(200), @cur NUMERIC(18,4), @msg NVARCHAR(2048);
    SELECT @stock = ISNULL(stock_yn, 'N'), @unit = unit_cd, @nm = item_nm FROM TBAITEM WHERE item_id = @p_item_id;

    IF @nm IS NULL THROW 50001, N'수불 품목을 찾을 수 없습니다.', 1;
    IF ISNULL(@p_qty, 0) <= 0 THROW 50001, N'수불 수량은 0보다 커야 합니다.', 1;
    IF @p_wh_id IS NULL THROW 50001, N'수불 창고가 지정되지 않았습니다.', 1;

    IF @p_kind = 'O' AND @stock = 'Y'
    BEGIN
        SELECT @cur = stock_qty FROM TMASTOCK WITH (UPDLOCK, HOLDLOCK)
        WHERE acc_id = @p_acc_id AND item_id = @p_item_id AND wh_id = @p_wh_id AND loc_id = 0 AND lot_no = ISNULL(@p_lot_no, N'');

        IF ISNULL(@cur, 0) < @p_qty
        BEGIN
            SET @msg = N'현재고가 부족합니다. 품목: ' + @nm + N', LOT: ' + ISNULL(@p_lot_no, N'')
                     + N', 현재고 ' + CAST(CAST(ISNULL(@cur, 0) AS FLOAT) AS NVARCHAR(30)) + N' < 필요수량 ' + CAST(CAST(@p_qty AS FLOAT) AS NVARCHAR(30));
            THROW 50001, @msg, 1;
        END
    END

    INSERT INTO TMATRANS (
        acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn,
        src_type, src_id, src_no, src_serl, org_trans_id, remark,
        reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
    )
    VALUES (
        @p_acc_id, ISNULL(@p_trans_date, CONVERT(VARCHAR(8), GETDATE(), 112)), @p_kind, @p_type, @p_item_id, @p_wh_id, 0, ISNULL(@p_lot_no, N''), @unit, @p_qty, @stock,
        @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_org_trans_id, @p_remark,
        @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    );
    SET @o_trans_id = SCOPE_IDENTITY();

    IF @stock = 'Y'
    BEGIN
        UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET
            stock_qty = stock_qty + CASE WHEN @p_kind = 'I' THEN @p_qty ELSE -@p_qty END,
            trans_id = @o_trans_id, unit_cd = ISNULL(@unit, unit_cd),
            upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        WHERE acc_id = @p_acc_id AND item_id = @p_item_id AND wh_id = @p_wh_id AND loc_id = 0 AND lot_no = ISNULL(@p_lot_no, N'');

        IF @@ROWCOUNT = 0
            INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @p_item_id, @p_wh_id, 0, ISNULL(@p_lot_no, N''), @unit, @p_qty, @o_trans_id, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
    END
END
GO

-- ============================================================
-- 2) USP_PR_TRANS_REVERSE - 문서(src_type/src_id)의 src_serl 범위 수불 중 아직 되돌리지 않은 것을 역거래로 되돌린다(내부용).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_TRANS_REVERSE
    @p_src_type VARCHAR(10),
    @p_src_id BIGINT,
    @p_serl_min INT,
    @p_serl_max INT,
    @p_remark NVARCHAR(3000),
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @tid BIGINT, @acc BIGINT, @kind VARCHAR(1), @type VARCHAR(10), @item BIGINT, @wh BIGINT, @lot NVARCHAR(50), @qty NUMERIC(18,4),
            @sno VARCHAR(20), @sserl INT, @new_tid BIGINT, @rkind VARCHAR(1);
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT t.trans_id, t.acc_id, t.trans_kind, t.trans_type, t.item_id, t.wh_id, t.lot_no, t.qty, t.src_no, t.src_serl
        FROM TMATRANS t
        WHERE t.src_type = @p_src_type AND t.src_id = @p_src_id AND t.src_serl BETWEEN @p_serl_min AND @p_serl_max
          AND t.org_trans_id IS NULL
          AND NOT EXISTS (SELECT 1 FROM TMATRANS r WHERE r.org_trans_id = t.trans_id)
        ORDER BY t.trans_id DESC;
    OPEN c;
    FETCH NEXT FROM c INTO @tid, @acc, @kind, @type, @item, @wh, @lot, @qty, @sno, @sserl;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @rkind = CASE WHEN @kind = 'I' THEN 'O' ELSE 'I' END;
        EXEC USP_PR_TRANS_POST @acc, @rkind, @type, @item, @wh, @lot, @qty,
                               @p_src_type, @p_src_id, @sno, @sserl, @tid, @p_remark, @p_user_id, @p_client_pc, NULL, @new_tid OUTPUT;
        FETCH NEXT FROM c INTO @tid, @acc, @kind, @type, @item, @wh, @lot, @qty, @sno, @sserl;
    END
    CLOSE c;
    DEALLOCATE c;
END
GO

-- ============================================================
-- 3) USP_PR_RSLT_Q - Q: 헤더(0) + EDS 웨이퍼 라인(1) + 산출 LOT(2) / LOT: 투입 가능 LOT 목록(작업지시 공정 기준)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLT_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
    @p_rslt_no VARCHAR(20) = NULL,
    @p_wo_id BIGINT = NULL,                 /* LOT 조회용 */
    @p_wo_serl INT = NULL,
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
            SELECT TOP 1 @match_id = rslt_id FROM TPRRSLTM
            WHERE (@p_rslt_id IS NULL OR rslt_id = @p_rslt_id)
              AND (@p_rslt_id IS NOT NULL OR @p_rslt_no IS NULL OR rslt_no LIKE '%' + @p_rslt_no + '%')
            ORDER BY rslt_id DESC;

            SELECT
                m.rslt_id, m.acc_id, m.rslt_no, m.rslt_date,
                m.wo_id, m.wo_no, m.wo_serl, m.proc_cd, p.proc_nm,
                m.cust_id, c.cust_nm, m.wh_id, w.wh_nm,
                m.in_lot_id, l.lot_no AS in_lot_no, d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd,
                d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd, d.split_qty,
                m.in_qty, m.good_qty, m.bad_qty, m.yield_rate, m.src_file_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TPRRSLTM m
                LEFT JOIN TPRWOD d ON d.wo_id = m.wo_id AND d.serl = m.wo_serl
                LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = m.wh_id
                LEFT JOIN TPRLOT l ON l.lot_id = m.in_lot_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE m.rslt_id = @match_id;

            SELECT rslt_id, serl, acc_id, wafer_no, good_qty, bad_qty, good_qty + bad_qty AS gross_qty, remark
            FROM TPRRSLTD WHERE rslt_id = @match_id ORDER BY serl;

            SELECT r.parent_lot_id, r.child_lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, r.qty
            FROM TPRLOTREL r
                JOIN TPRLOT l ON l.lot_id = r.child_lot_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE r.rslt_id = @match_id ORDER BY l.lot_no;
        END
        ELSE IF @p_work_type = 'LOT'
        BEGIN
            -- 그 공정의 외주처 창고에 재고가 있는 투입품목 LOT(이 작업지시 소속)
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.stock_qty
            FROM TPRWOD d
                JOIN TPRLOT l ON l.wo_id = d.wo_id AND l.item_id = d.in_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = d.wh_id AND s.loc_id = 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE d.wo_id = @p_wo_id AND d.serl = @p_wo_serl AND s.stock_qty > 0
            ORDER BY l.lot_no;
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
-- 4) USP_PR_RSLT_S - 헤더 N/U/D (작성 상태에서만). 웨이퍼 판정 라인이 있으면 양품/불량은 라인 합계가 우선한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_rslt_date VARCHAR(8) = NULL,
    @p_wo_id BIGINT = NULL,
    @p_wo_serl INT = NULL,
    @p_in_lot_id BIGINT = NULL,
    @p_in_qty NUMERIC(18,4) = NULL,
    @p_good_qty NUMERIC(18,4) = NULL,
    @p_bad_qty NUMERIC(18,4) = NULL,
    @p_src_file_nm NVARCHAR(400) = NULL,
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
            SELECT @cur_stat = stat_cd, @p_wo_id = wo_id, @p_wo_serl = wo_serl FROM TPRRSLTM WHERE rslt_id = @p_rslt_id;

            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정 실적을 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 실적은 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            DECLARE @wo_stat VARCHAR(10), @wo_no VARCHAR(20), @proc VARCHAR(10), @cust BIGINT, @wh BIGINT, @in_item BIGINT, @in_unit VARCHAR(10), @out_unit VARCHAR(10);
            SELECT @wo_stat = m.stat_cd, @wo_no = m.wo_no, @proc = d.proc_cd, @cust = d.cust_id, @wh = d.wh_id,
                   @in_item = d.in_item_id, @in_unit = d.in_unit_cd, @out_unit = d.out_unit_cd
            FROM TPRWOM m JOIN TPRWOD d ON d.wo_id = m.wo_id
            WHERE m.wo_id = @p_wo_id AND d.serl = @p_wo_serl;

            IF @proc IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시의 공정을 찾을 수 없습니다.'; RETURN;
            END
            IF @wo_stat IN ('E', 'X')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'완료되었거나 중단된 작업지시에는 실적을 등록할 수 없습니다.'; RETURN;
            END
            IF ISNULL(@wo_stat, '0') = '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시가 아직 확정되지 않았습니다. 작업지시 화면에서 먼저 확정하세요.'; RETURN;
            END
            IF @wh IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시 공정에 외주처 창고가 지정되지 않았습니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TPRLOT WHERE lot_id = @p_in_lot_id AND wo_id = @p_wo_id AND item_id = @in_item)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'투입 LOT가 이 작업지시의 투입품목 LOT가 아닙니다.'; RETURN;
            END

            SET @p_good_qty = ISNULL(@p_good_qty, 0);
            SET @p_bad_qty = ISNULL(@p_bad_qty, 0);
            IF ISNULL(@p_in_qty, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'투입수량은 0보다 커야 합니다.'; RETURN;
            END
            IF @p_good_qty < 0 OR @p_bad_qty < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'양품/불량 수량은 음수일 수 없습니다.'; RETURN;
            END
            IF ISNULL(@in_unit, '') = ISNULL(@out_unit, '') AND @p_good_qty + @p_bad_qty > @p_in_qty
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'양품+불량 수량이 투입수량을 넘을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TPRRSLTM', 'rslt_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TPRRSLTM (
                acc_id, rslt_no, rslt_date, wo_id, wo_no, wo_serl, proc_cd, cust_id, wh_id, in_lot_id, in_qty, good_qty, bad_qty,
                src_file_nm, stat_cd, cfm_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_rslt_date, @p_wo_id, @wo_no, @p_wo_serl, @proc, @cust, @wh, @p_in_lot_id, @p_in_qty, @p_good_qty, @p_bad_qty,
                @p_src_file_nm, '0', 'N', @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );
            SET @p_rslt_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRRSLTM SET
                rslt_date = @p_rslt_date, in_lot_id = @p_in_lot_id, in_qty = @p_in_qty, good_qty = @p_good_qty, bad_qty = @p_bad_qty,
                src_file_nm = @p_src_file_nm, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rslt_id = @p_rslt_id;

            -- 웨이퍼 판정 라인이 있으면 합계가 우선
            IF EXISTS (SELECT 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id)
                UPDATE TPRRSLTM SET
                    good_qty = (SELECT SUM(good_qty) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id),
                    bad_qty = (SELECT SUM(bad_qty) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id)
                WHERE rslt_id = @p_rslt_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TPRRSLTD WHERE rslt_id = @p_rslt_id;
            DELETE FROM TPRRSLTM WHERE rslt_id = @p_rslt_id;
        END

        SET @GeneratedCode = CAST(@p_rslt_id AS VARCHAR(20));
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
-- 5) USP_PR_RSLT_S_1 - EDS 웨이퍼 판정 라인 N/U/D/DA(전체삭제 - 엑셀 다시 읽기용). 변경 후 헤더 양품/불량을 라인 합계로 맞춘다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_wafer_no VARCHAR(20) = NULL,
    @p_good_qty NUMERIC(18,4) = NULL,
    @p_bad_qty NUMERIC(18,4) = NULL,
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
        DECLARE @stat VARCHAR(10), @acc BIGINT;
        SELECT @stat = stat_cd, @acc = acc_id FROM TPRRSLTM WHERE rslt_id = @p_rslt_id;
        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'공정 실적을 찾을 수 없습니다.'; RETURN;
        END
        IF @stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 실적은 수정할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_wafer_no, '') = ''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'웨이퍼 번호를 입력하세요.'; RETURN;
            END
            IF ISNULL(@p_good_qty, 0) < 0 OR ISNULL(@p_bad_qty, 0) < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'Good/Bad 수량은 음수일 수 없습니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id AND wafer_no = @p_wafer_no AND (@p_work_type = 'N' OR serl <> @p_serl))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 등록된 웨이퍼 번호입니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id;
            INSERT INTO TPRRSLTD (rslt_id, serl, acc_id, wafer_no, good_qty, bad_qty, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_rslt_id, @p_serl, @acc, @p_wafer_no, ISNULL(@p_good_qty, 0), ISNULL(@p_bad_qty, 0), @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
            UPDATE TPRRSLTD SET wafer_no = @p_wafer_no, good_qty = ISNULL(@p_good_qty, 0), bad_qty = ISNULL(@p_bad_qty, 0), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rslt_id = @p_rslt_id AND serl = @p_serl;
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRRSLTD WHERE rslt_id = @p_rslt_id AND serl = @p_serl;
        ELSE IF @p_work_type = 'DA'
            DELETE FROM TPRRSLTD WHERE rslt_id = @p_rslt_id;

        IF EXISTS (SELECT 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id)
            UPDATE TPRRSLTM SET
                good_qty = (SELECT SUM(good_qty) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id),
                bad_qty = (SELECT SUM(bad_qty) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rslt_id = @p_rslt_id;

        SET @GeneratedCode = CAST(@p_rslt_id AS VARCHAR(20));
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
-- 6) USP_PR_RSLT_CONFIRM_CORE - 실적 확정(내부용). 백플러시: 투입 LOT를 in_qty만큼 소진(PR_OUT)하고 양품을 산출 LOT로 입고(PR_IN).
--    산출 LOT는 split_qty가 있으면 그 수량씩 분할. 불량은 소진만 되고 재고로 남지 않는다(손실).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLT_CONFIRM_CORE
    @p_rslt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @wo BIGINT, @serl INT, @proc VARCHAR(10), @wh BIGINT, @lot_id BIGINT,
            @in_qty NUMERIC(18,4), @good NUMERIC(18,4), @bad NUMERIC(18,4), @stat VARCHAR(10);
    SELECT @acc = acc_id, @no = rslt_no, @date = rslt_date, @wo = wo_id, @serl = wo_serl, @proc = proc_cd, @wh = wh_id, @lot_id = in_lot_id,
           @in_qty = in_qty, @good = good_qty, @bad = bad_qty, @stat = stat_cd
    FROM TPRRSLTM WHERE rslt_id = @p_rslt_id;

    IF @stat IS NULL THROW 50001, N'공정 실적을 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'이미 확정된 실적입니다.', 1;

    DECLARE @in_item BIGINT, @out_item BIGINT, @in_unit VARCHAR(10), @out_unit VARCHAR(10), @split NUMERIC(18,4), @wo_stat VARCHAR(10);
    SELECT @in_item = d.in_item_id, @out_item = d.out_item_id, @in_unit = d.in_unit_cd, @out_unit = d.out_unit_cd, @split = d.split_qty, @wo_stat = m.stat_cd
    FROM TPRWOD d JOIN TPRWOM m ON m.wo_id = d.wo_id WHERE d.wo_id = @wo AND d.serl = @serl;

    IF @in_item IS NULL THROW 50001, N'작업지시 공정을 찾을 수 없습니다.', 1;
    IF @wo_stat IN ('E', 'X') THROW 50001, N'완료되었거나 중단된 작업지시입니다.', 1;
    IF ISNULL(@wo_stat, '0') = '0' THROW 50001, N'작업지시가 아직 확정되지 않았습니다. 작업지시 화면에서 먼저 확정하세요.', 1;
    IF @wh IS NULL THROW 50001, N'외주처 창고가 지정되지 않았습니다.', 1;

    DECLARE @wd_cnt INT, @wd_good NUMERIC(18,4), @wd_bad NUMERIC(18,4);
    SELECT @wd_cnt = COUNT(*), @wd_good = ISNULL(SUM(good_qty), 0), @wd_bad = ISNULL(SUM(bad_qty), 0) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id;
    IF @wd_cnt > 0 AND (@wd_good <> @good OR @wd_bad <> @bad)
        THROW 50001, N'웨이퍼별 판정 합계와 양품/불량 수량이 다릅니다.', 1;

    DECLARE @in_lot NVARCHAR(50);
    SELECT @in_lot = lot_no FROM TPRLOT WHERE lot_id = @lot_id AND wo_id = @wo AND item_id = @in_item;
    IF @in_lot IS NULL THROW 50001, N'투입 LOT를 찾을 수 없습니다.', 1;

    -- 산출 LOT 이름 재료: 뿌리 = 작업지시의 시작 LOT 번호(웨이퍼 LOT 번호 자체에 '-'가 있어도 안전), 그리고 투입 LOT가 뿌리에서 파생된
    -- LOT면 그 끝 세그먼트의 끝 숫자(분할 순번 이어받기). 시작 LOT 자체를 투입하면 숫자 없음.
    DECLARE @root NVARCHAR(50) = (SELECT start_lot_no FROM TPRWOM WHERE wo_id = @wo);
    DECLARE @dig NVARCHAR(50) = N'';
    IF LEFT(@in_lot, LEN(@root) + 1) = @root + N'-'
    BEGIN
        DECLARE @last NVARCHAR(50) = SUBSTRING(@in_lot, LEN(@root) + 2, 50), @pos INT;
        WHILE CHARINDEX(N'-', @last) > 0 SET @last = SUBSTRING(@last, CHARINDEX(N'-', @last) + 1, 50);
        SET @pos = LEN(@last);
        WHILE @pos > 0 AND SUBSTRING(@last, @pos, 1) LIKE N'[0-9]' SET @pos = @pos - 1;
        SET @dig = SUBSTRING(@last, @pos + 1, 50);
    END

    BEGIN TRAN;

    DECLARE @tid BIGINT;
    EXEC USP_PR_TRANS_POST @acc, 'O', 'PR_OUT', @in_item, @wh, @in_lot, @in_qty, 'RS', @p_rslt_id, @no, 1, NULL, NULL, @p_user_id, @p_client_pc, @date, @tid OUTPUT;

    IF @good > 0
    BEGIN
        DECLARE @n INT = CASE WHEN ISNULL(@split, 0) > 0 THEN CAST(CEILING(@good / @split) AS INT) ELSE 1 END,
                @i INT = 1, @left NUMERIC(18,4) = @good, @q NUMERIC(18,4), @out_lot NVARCHAR(50), @out_id BIGINT, @ss INT;
        WHILE @i <= @n
        BEGIN
            SET @q = CASE WHEN ISNULL(@split, 0) > 0 AND @left > @split THEN @split ELSE @left END;
            SET @out_lot = @root + N'-' + @proc + @dig + CASE WHEN ISNULL(@split, 0) > 0 THEN RIGHT(N'00' + CAST(@i AS NVARCHAR(3)), 2) ELSE N'' END;

            SET @out_id = NULL;
            SELECT @out_id = lot_id FROM TPRLOT WHERE acc_id = @acc AND item_id = @out_item AND lot_no = @out_lot;
            IF @out_id IS NULL
            BEGIN
                INSERT INTO TPRLOT (acc_id, lot_no, item_id, unit_cd, init_qty, wo_id, wo_serl, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @out_lot, @out_item, @out_unit, @q, @wo, @serl, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
                SET @out_id = SCOPE_IDENTITY();
            END
            ELSE
                UPDATE TPRLOT SET init_qty = init_qty + @q, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE lot_id = @out_id;

            IF EXISTS (SELECT 1 FROM TPRLOTREL WHERE parent_lot_id = @lot_id AND child_lot_id = @out_id)
                UPDATE TPRLOTREL SET qty = qty + @q WHERE parent_lot_id = @lot_id AND child_lot_id = @out_id;
            ELSE
                INSERT INTO TPRLOTREL (parent_lot_id, child_lot_id, acc_id, rslt_id, qty, reg_user_id, reg_dt, reg_pc)
                VALUES (@lot_id, @out_id, @acc, @p_rslt_id, @q, @p_user_id, GETDATE(), @p_client_pc);

            SET @ss = @i + 100;
            EXEC USP_PR_TRANS_POST @acc, 'I', 'PR_IN', @out_item, @wh, @out_lot, @q, 'RS', @p_rslt_id, @no, @ss, NULL, NULL, @p_user_id, @p_client_pc, @date, @tid OUTPUT;

            SET @left = @left - @q;
            SET @i = @i + 1;
        END
    END

    -- 수율: 투입/산출 단위가 같으면 양품/투입, 다르면(웨이퍼 -> Die) 양품/(양품+불량)
    UPDATE TPRRSLTM SET
        yield_rate = CASE WHEN ISNULL(@in_unit, '') = ISNULL(@out_unit, '') THEN @good * 100.0 / @in_qty
                          WHEN @good + @bad > 0 THEN @good * 100.0 / (@good + @bad) ELSE NULL END,
        stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
        upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE rslt_id = @p_rslt_id;

    UPDATE TPRWOD SET in_qty = in_qty + @in_qty, good_qty = good_qty + @good, bad_qty = bad_qty + @bad,
        stat_cd = CASE WHEN stat_cd = 'E' THEN stat_cd ELSE '1' END, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE wo_id = @wo AND serl = @serl;
    UPDATE TPRWOM SET stat_cd = '1', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE wo_id = @wo AND stat_cd = 'C'; -- 확정 -> 진행(첫 실적)

    COMMIT TRAN;
END
GO

-- ============================================================
-- 7) USP_PR_RSLT_CANCEL_CORE - 실적 확정취소(내부용). 수불을 역거래로 되돌린다. 산출 LOT가 이미 다른 공정/이전에 쓰여
--    재고가 모자라면 거부(현재고 부족 메시지).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLT_CANCEL_CORE
    @p_rslt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @wo BIGINT, @serl INT, @in_qty NUMERIC(18,4), @good NUMERIC(18,4), @bad NUMERIC(18,4), @stat VARCHAR(10);
    SELECT @wo = wo_id, @serl = wo_serl, @in_qty = in_qty, @good = good_qty, @bad = bad_qty, @stat = stat_cd FROM TPRRSLTM WHERE rslt_id = @p_rslt_id;

    IF @stat IS NULL THROW 50001, N'공정 실적을 찾을 수 없습니다.', 1;
    IF @stat <> 'C' THROW 50001, N'확정된 실적만 확정취소할 수 있습니다.', 1;

    BEGIN TRAN;

    EXEC USP_PR_TRANS_REVERSE 'RS', @p_rslt_id, 0, 999999, N'공정 실적 확정취소', @p_user_id, @p_client_pc;

    -- 산출 LOT 수량 되돌리고, 이 실적으로만 만들어졌던 LOT는 삭제
    UPDATE l SET init_qty = l.init_qty - r.qty
    FROM TPRLOT l JOIN TPRLOTREL r ON r.child_lot_id = l.lot_id WHERE r.rslt_id = @p_rslt_id;
    DECLARE @kids TABLE (lot_id BIGINT);
    INSERT INTO @kids SELECT child_lot_id FROM TPRLOTREL WHERE rslt_id = @p_rslt_id;
    DELETE FROM TPRLOTREL WHERE rslt_id = @p_rslt_id;
    DELETE FROM TPRLOT WHERE lot_id IN (SELECT lot_id FROM @kids) AND init_qty <= 0;

    UPDATE TPRRSLTM SET yield_rate = NULL, stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
        upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE rslt_id = @p_rslt_id;

    UPDATE TPRWOD SET in_qty = in_qty - @in_qty, good_qty = good_qty - @good, bad_qty = bad_qty - @bad,
        upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE wo_id = @wo AND serl = @serl;
    UPDATE TPRWOD SET stat_cd = '0' WHERE wo_id = @wo AND serl = @serl AND in_qty <= 0 AND stat_cd = '1';
    IF NOT EXISTS (SELECT 1 FROM TPRWOD WHERE wo_id = @wo AND in_qty > 0)
        UPDATE TPRWOM SET stat_cd = 'C' WHERE wo_id = @wo AND stat_cd = '1'; -- 실적이 모두 취소되면 진행 -> 확정으로 되돌린다

    COMMIT TRAN;
END
GO

-- ============================================================
-- 8) USP_PR_RSLT_C_S - 화면용 확정(C)/확정취소(CC) 래퍼
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLT_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
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
            EXEC USP_PR_RSLT_CONFIRM_CORE @p_rslt_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC'
            EXEC USP_PR_RSLT_CANCEL_CORE @p_rslt_id, @p_user_id, @p_client_pc;

        SET @GeneratedCode = CAST(@p_rslt_id AS VARCHAR(20));
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
-- 9) USP_PR_XFER_Q - Q: 헤더(0) + 라인(1) / LOT: 이전 가능 LOT 목록(출발 공정 창고에 재고가 있는 이 작업지시의 LOT)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_XFER_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_xfer_id BIGINT = NULL,
    @p_xfer_no VARCHAR(20) = NULL,
    @p_wo_id BIGINT = NULL,                 /* LOT 조회용 */
    @p_from_serl INT = NULL,
    @p_to_serl INT = NULL,
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
            SELECT TOP 1 @match_id = xfer_id FROM TPRXFERM
            WHERE (@p_xfer_id IS NULL OR xfer_id = @p_xfer_id)
              AND (@p_xfer_id IS NOT NULL OR @p_xfer_no IS NULL OR xfer_no LIKE '%' + @p_xfer_no + '%')
            ORDER BY xfer_id DESC;

            SELECT
                m.xfer_id, m.acc_id, m.xfer_no, m.xfer_date, m.xfer_kind, m.wo_id, m.wo_no, m.from_serl, m.to_serl,
                m.from_cust_id, fc.cust_nm AS from_cust_nm, m.from_wh_id, fw.wh_nm AS from_wh_nm,
                m.to_cust_id, tc.cust_nm AS to_cust_nm, m.to_wh_id, tw.wh_nm AS to_wh_nm, m.trans_wh_id,
                m.stat_cd, m.out_dt, m.out_user_id, m.in_dt, m.in_user_id, m.remark
            FROM TPRXFERM m
                LEFT JOIN TBACUST fc ON fc.cust_id = m.from_cust_id
                LEFT JOIN TBACUST tc ON tc.cust_id = m.to_cust_id
                LEFT JOIN TBAWH fw ON fw.wh_id = m.from_wh_id
                LEFT JOIN TBAWH tw ON tw.wh_id = m.to_wh_id
            WHERE m.xfer_id = @match_id;

            SELECT
                d.xfer_id, d.serl, d.acc_id, d.xfer_no, d.item_id, i.item_no, i.item_nm, d.lot_id, d.lot_no, d.unit_cd,
                d.out_qty, d.in_qty, d.diff_qty, d.diff_resp_cd, d.diff_act_cd, d.diff_dt, d.diff_user_id, d.diff_remark, d.remark
            FROM TPRXFERD d LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE d.xfer_id = @match_id ORDER BY d.serl;
        END
        ELSE IF @p_work_type = 'LOT'
        BEGIN
            -- 이전 대상 품목 = 두 공정 중 앞선 공정의 산출품목. 위치 = 출발 공정의 외주처 창고.
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.stock_qty
            FROM TPRWOD f
                JOIN TPRWOD lo ON lo.wo_id = f.wo_id AND lo.serl = CASE WHEN @p_to_serl IS NULL OR @p_from_serl < @p_to_serl THEN @p_from_serl ELSE @p_to_serl END
                JOIN TPRLOT l ON l.wo_id = f.wo_id AND l.item_id = lo.out_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = f.wh_id AND s.loc_id = 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE f.wo_id = @p_wo_id AND f.serl = @p_from_serl AND s.stock_qty > 0
            ORDER BY l.lot_no;
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
-- 10) USP_PR_XFER_S - 헤더 N/U/D (작성 상태에서만). 창고/외주처는 작업지시 공정에서 가져온다.
--     N(정방향)은 도착 공정 > 출발 공정, R(역이전: 반품/재작업)은 도착 공정 < 출발 공정. 도착 공정이 없으면(자사 반입) to_wh_id를 직접 지정.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_XFER_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_xfer_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_xfer_date VARCHAR(8) = NULL,
    @p_xfer_kind VARCHAR(1) = NULL,         /* N 정방향(기본) / R 역이전 */
    @p_wo_id BIGINT = NULL,
    @p_from_serl INT = NULL,
    @p_to_serl INT = NULL,
    @p_to_wh_id BIGINT = NULL,              /* 도착 공정이 없을 때만 */
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
            DECLARE @cur_stat VARCHAR(10), @cur_to_serl INT;
            SELECT @cur_stat = stat_cd, @cur_to_serl = to_serl FROM TPRXFERM WHERE xfer_id = @p_xfer_id;
            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'외주 이전 문서를 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'출발 확인된 이전은 수정하거나 삭제할 수 없습니다. 먼저 출발 취소하세요.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            SET @p_xfer_kind = ISNULL(NULLIF(@p_xfer_kind, ''), 'N');
            IF @p_xfer_kind NOT IN ('N', 'R')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이전 구분이 올바르지 않습니다.'; RETURN;
            END

            DECLARE @wo_no VARCHAR(20), @wo_stat VARCHAR(10);
            SELECT @wo_no = wo_no, @wo_stat = stat_cd FROM TPRWOM WHERE wo_id = @p_wo_id;
            IF @wo_no IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시를 찾을 수 없습니다.'; RETURN;
            END
            IF @wo_stat IN ('E', 'X')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'완료되었거나 중단된 작업지시입니다.'; RETURN;
            END
            IF ISNULL(@wo_stat, '0') = '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시가 아직 확정되지 않았습니다. 작업지시 화면에서 먼저 확정하세요.'; RETURN;
            END

            DECLARE @from_wh BIGINT, @from_cust BIGINT, @to_wh BIGINT, @to_cust BIGINT, @tr_wh BIGINT;
            SELECT @from_wh = wh_id, @from_cust = cust_id FROM TPRWOD WHERE wo_id = @p_wo_id AND serl = @p_from_serl;
            IF @from_wh IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'출발 공정을 찾을 수 없거나 외주처 창고가 지정되지 않았습니다.'; RETURN;
            END

            IF @p_to_serl IS NOT NULL
            BEGIN
                SELECT @to_wh = wh_id, @to_cust = cust_id FROM TPRWOD WHERE wo_id = @p_wo_id AND serl = @p_to_serl;
                IF @to_wh IS NULL
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = N'도착 공정을 찾을 수 없거나 외주처 창고가 지정되지 않았습니다.'; RETURN;
                END
                IF (@p_xfer_kind = 'N' AND @p_to_serl <= @p_from_serl) OR (@p_xfer_kind = 'R' AND @p_to_serl >= @p_from_serl)
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = CASE WHEN @p_xfer_kind = 'N' THEN N'정방향 이전은 도착 공정이 출발 공정보다 뒤여야 합니다.' ELSE N'역이전은 도착 공정이 출발 공정보다 앞이어야 합니다.' END; RETURN;
                END
            END
            ELSE
            BEGIN
                SET @to_wh = @p_to_wh_id;
                IF @to_wh IS NULL OR NOT EXISTS (SELECT 1 FROM TBAWH WHERE wh_id = @to_wh)
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = N'도착 공정이 없으면 도착 창고를 지정해야 합니다.'; RETURN;
                END
            END

            SELECT TOP 1 @tr_wh = wh_id FROM TBAWH WHERE acc_id = @p_acc_id AND wh_type = 'TR' ORDER BY wh_id;
            IF @tr_wh IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이동중 재고 창고(창고유형 TR)가 없습니다. 창고를 먼저 등록하세요.'; RETURN;
            END

            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TPRXFERM', 'xfer_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TPRXFERM (
                acc_id, xfer_no, xfer_date, xfer_kind, wo_id, wo_no, from_serl, to_serl, from_cust_id, from_wh_id, to_cust_id, to_wh_id, trans_wh_id,
                stat_cd, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_xfer_date, @p_xfer_kind, @p_wo_id, @wo_no, @p_from_serl, @p_to_serl, @from_cust, @from_wh, @to_cust, @to_wh, @tr_wh,
                '0', @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );
            SET @p_xfer_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 공정/방향은 바꿀 수 없다(라인 검증 기준). 날짜/비고, 도착 공정이 없는 이전이면 도착 창고만 바꾼다.
            UPDATE TPRXFERM SET
                xfer_date = @p_xfer_date,
                to_wh_id = CASE WHEN @cur_to_serl IS NULL AND @p_to_wh_id IS NOT NULL THEN @p_to_wh_id ELSE to_wh_id END,
                remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE xfer_id = @p_xfer_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TPRXFERD WHERE xfer_id = @p_xfer_id;
            DELETE FROM TPRXFERM WHERE xfer_id = @p_xfer_id;
        END

        SET @GeneratedCode = CAST(@p_xfer_id AS VARCHAR(20));
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
-- 11) USP_PR_XFER_S_1 - 라인 N/U/D (작성 상태) + I(도착 수량 입력: 이동중 상태에서, 0 <= 도착 <= 출발)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_XFER_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_xfer_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_lot_id BIGINT = NULL,
    @p_out_qty NUMERIC(18,4) = NULL,
    @p_in_qty NUMERIC(18,4) = NULL,         /* I 전용 */
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
        DECLARE @stat VARCHAR(10), @acc BIGINT, @xno VARCHAR(20), @wo BIGINT, @from_serl INT, @to_serl INT;
        SELECT @stat = stat_cd, @acc = acc_id, @xno = xfer_no, @wo = wo_id, @from_serl = from_serl, @to_serl = to_serl FROM TPRXFERM WHERE xfer_id = @p_xfer_id;
        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'외주 이전 문서를 찾을 수 없습니다.'; RETURN;
        END

        IF @p_work_type = 'I'
        BEGIN
            IF @stat <> '1'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'도착 수량은 이동중(출발 확인된) 이전에서만 입력할 수 있습니다.'; RETURN;
            END
            DECLARE @o NUMERIC(18,4);
            SELECT @o = out_qty FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND serl = @p_serl;
            IF @o IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이전 라인을 찾을 수 없습니다.'; RETURN;
            END
            IF @p_in_qty IS NOT NULL AND (@p_in_qty < 0 OR @p_in_qty > @o)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'도착 수량은 0 이상, 출발 수량 이하여야 합니다.'; RETURN;
            END
            UPDATE TPRXFERD SET in_qty = @p_in_qty, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE xfer_id = @p_xfer_id AND serl = @p_serl;
            SET @GeneratedCode = CAST(@p_xfer_id AS VARCHAR(20));
            RETURN;
        END

        IF @stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'출발 확인된 이전은 수정할 수 없습니다. 먼저 출발 취소하세요.'; RETURN;
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_out_qty, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'출발 수량은 0보다 커야 합니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @lot_no NVARCHAR(50), @item BIGINT, @unit VARCHAR(10), @need_item BIGINT;
            -- 이전 대상 품목 = 두 공정 중 앞선 공정의 산출품목
            SELECT @need_item = out_item_id FROM TPRWOD
            WHERE wo_id = @wo AND serl = CASE WHEN @to_serl IS NULL OR @from_serl < @to_serl THEN @from_serl ELSE @to_serl END;
            SELECT @lot_no = l.lot_no, @item = l.item_id, @unit = l.unit_cd FROM TPRLOT l WHERE l.lot_id = @p_lot_id AND l.wo_id = @wo;

            IF @lot_no IS NULL OR @item <> @need_item
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이 이전에서 옮길 수 있는 품목의 LOT가 아닙니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND lot_id = @p_lot_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 추가된 LOT입니다.'; RETURN;
            END

            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TPRXFERD WHERE xfer_id = @p_xfer_id;
            INSERT INTO TPRXFERD (xfer_id, serl, acc_id, xfer_no, item_id, lot_id, lot_no, unit_cd, out_qty, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_xfer_id, @p_serl, @acc, @xno, @item, @p_lot_id, @lot_no, @unit, @p_out_qty, @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
            UPDATE TPRXFERD SET out_qty = @p_out_qty, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE xfer_id = @p_xfer_id AND serl = @p_serl;
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_xfer_id AS VARCHAR(20));
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
-- 12) USP_PR_XFER_C_S - 화면용 단계 처리
--     OUT   출발 확인: 출발 창고 -> 이동중 (stat 0 -> 1)
--     OUTCC 출발 취소: 이동중 -> 출발 창고로 원복 (stat 1 -> 0, 도착 확인 전에만)
--     IN    도착 확인: 이동중 -> 도착 창고, 도착 수량 미입력 라인은 전량 도착으로 본다. 차이가 있으면 stat 2, 없으면 E
--     INCC  도착 취소: 도착 창고 -> 이동중으로 원복 (stat 2/E -> 1, 차이 처리가 시작되지 않았을 때만)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_XFER_C_S
    @p_work_type VARCHAR(50),               /* OUT / OUTCC / IN / INCC */
    ---------------------------------------------------------------------------------------------------
    @p_xfer_id BIGINT = NULL,
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
        DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @stat VARCHAR(10), @fwh BIGINT, @twh BIGINT, @trwh BIGINT;
        SELECT @acc = acc_id, @no = xfer_no, @date = xfer_date, @stat = stat_cd, @fwh = from_wh_id, @twh = to_wh_id, @trwh = trans_wh_id
        FROM TPRXFERM WHERE xfer_id = @p_xfer_id;
        IF @stat IS NULL THROW 50001, N'외주 이전 문서를 찾을 수 없습니다.', 1;

        DECLARE @serl INT, @item BIGINT, @lot NVARCHAR(50), @out NUMERIC(18,4), @in NUMERIC(18,4), @tid BIGINT, @sa INT, @sb INT;

        IF @p_work_type = 'OUT'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 이전만 출발 확인할 수 있습니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TPRXFERD WHERE xfer_id = @p_xfer_id) THROW 50001, N'이전할 LOT가 없어 출발 확인할 수 없습니다.', 1;

            BEGIN TRAN;
            DECLARE c1 CURSOR LOCAL FAST_FORWARD FOR SELECT serl, item_id, lot_no, out_qty FROM TPRXFERD WHERE xfer_id = @p_xfer_id ORDER BY serl;
            OPEN c1; FETCH NEXT FROM c1 INTO @serl, @item, @lot, @out;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @sa = @serl * 10 + 1; SET @sb = @serl * 10 + 2;
                EXEC USP_PR_TRANS_POST @acc, 'O', 'MV_OUT', @item, @fwh, @lot, @out, 'XF', @p_xfer_id, @no, @sa, NULL, NULL, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                EXEC USP_PR_TRANS_POST @acc, 'I', 'MV_IN', @item, @trwh, @lot, @out, 'XF', @p_xfer_id, @no, @sb, NULL, NULL, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                FETCH NEXT FROM c1 INTO @serl, @item, @lot, @out;
            END
            CLOSE c1; DEALLOCATE c1;
            UPDATE TPRXFERM SET stat_cd = '1', out_dt = GETDATE(), out_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE xfer_id = @p_xfer_id;
            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'OUTCC'
        BEGIN
            IF @stat <> '1' THROW 50001, N'이동중 상태의 이전만 출발 취소할 수 있습니다. (도착 확인된 이전은 먼저 도착 취소)', 1;
            BEGIN TRAN;
            EXEC USP_PR_TRANS_REVERSE 'XF', @p_xfer_id, 0, 999, N'외주 이전 출발 취소', @p_user_id, @p_client_pc;
            UPDATE TPRXFERD SET in_qty = NULL, diff_qty = NULL WHERE xfer_id = @p_xfer_id;
            UPDATE TPRXFERM SET stat_cd = '0', out_dt = NULL, out_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE xfer_id = @p_xfer_id;
            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'IN'
        BEGIN
            IF @stat <> '1' THROW 50001, N'이동중 상태의 이전만 도착 확인할 수 있습니다.', 1;

            BEGIN TRAN;
            UPDATE TPRXFERD SET in_qty = out_qty WHERE xfer_id = @p_xfer_id AND in_qty IS NULL;
            UPDATE TPRXFERD SET diff_qty = out_qty - in_qty, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE xfer_id = @p_xfer_id;

            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR SELECT serl, item_id, lot_no, in_qty FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND in_qty > 0 ORDER BY serl;
            OPEN c2; FETCH NEXT FROM c2 INTO @serl, @item, @lot, @in;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @sa = 1000 + @serl * 10 + 1; SET @sb = 1000 + @serl * 10 + 2;
                EXEC USP_PR_TRANS_POST @acc, 'O', 'MV_OUT', @item, @trwh, @lot, @in, 'XF', @p_xfer_id, @no, @sa, NULL, NULL, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                EXEC USP_PR_TRANS_POST @acc, 'I', 'MV_IN', @item, @twh, @lot, @in, 'XF', @p_xfer_id, @no, @sb, NULL, NULL, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                FETCH NEXT FROM c2 INTO @serl, @item, @lot, @in;
            END
            CLOSE c2; DEALLOCATE c2;

            UPDATE TPRXFERM SET
                stat_cd = CASE WHEN EXISTS (SELECT 1 FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND diff_qty > 0) THEN '2' ELSE 'E' END,
                in_dt = GETDATE(), in_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE xfer_id = @p_xfer_id;
            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'INCC'
        BEGIN
            IF @stat NOT IN ('2', 'E') THROW 50001, N'도착 확인된 이전만 도착 취소할 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND diff_act_cd IS NOT NULL)
                THROW 50001, N'차이 처리가 진행된 이전은 도착 취소할 수 없습니다. 차이 처리를 먼저 취소하세요.', 1;
            BEGIN TRAN;
            EXEC USP_PR_TRANS_REVERSE 'XF', @p_xfer_id, 1000, 1999, N'외주 이전 도착 취소', @p_user_id, @p_client_pc;
            UPDATE TPRXFERD SET in_qty = NULL, diff_qty = NULL WHERE xfer_id = @p_xfer_id;
            UPDATE TPRXFERM SET stat_cd = '1', in_dt = NULL, in_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE xfer_id = @p_xfer_id;
            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_xfer_id AS VARCHAR(20));
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
-- 13) USP_PR_XFER_DIFF_S - 수량 차이 처리(이전 라인 단위). 차이 수량은 이동중 창고에 남아 있다.
--     work_type P: 처리. act_cd LOSS = 손실(ADJ_OUT, 이동중에서 차감) / RTN = 재입고(이동중 -> 도착 창고). resp_cd = 귀책(PR0005).
--     work_type CC: 그 라인의 차이 처리 취소. 모든 차이 라인이 처리되면 이전이 완료(E), 취소하면 다시 차이대기(2).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_XFER_DIFF_S
    @p_work_type VARCHAR(50),               /* P 처리 / CC 처리 취소 */
    ---------------------------------------------------------------------------------------------------
    @p_xfer_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_resp_cd VARCHAR(10) = NULL,
    @p_act_cd VARCHAR(10) = NULL,
    @p_remark NVARCHAR(1000) = NULL,
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
        DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @stat VARCHAR(10), @twh BIGINT, @trwh BIGINT;
        SELECT @acc = acc_id, @no = xfer_no, @date = xfer_date, @stat = stat_cd, @twh = to_wh_id, @trwh = trans_wh_id FROM TPRXFERM WHERE xfer_id = @p_xfer_id;
        IF @stat IS NULL THROW 50001, N'외주 이전 문서를 찾을 수 없습니다.', 1;
        IF @stat NOT IN ('2', 'E') THROW 50001, N'도착 확인된 이전만 차이를 처리할 수 있습니다.', 1;

        DECLARE @item BIGINT, @lot NVARCHAR(50), @diff NUMERIC(18,4), @act VARCHAR(10), @tid BIGINT, @sa INT = 2000 + ISNULL(@p_serl, 0) * 10 + 1, @sb INT = 2000 + ISNULL(@p_serl, 0) * 10 + 2;
        SELECT @item = item_id, @lot = lot_no, @diff = diff_qty, @act = diff_act_cd FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND serl = @p_serl;
        IF @item IS NULL THROW 50001, N'이전 라인을 찾을 수 없습니다.', 1;

        BEGIN TRAN;
        IF @p_work_type = 'P'
        BEGIN
            IF ISNULL(@diff, 0) <= 0 THROW 50001, N'차이 수량이 없는 라인입니다.', 1;
            IF @act IS NOT NULL THROW 50001, N'이미 차이 처리된 라인입니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'PR0005' AND minor_cd = @p_resp_cd AND use_yn = 'Y') THROW 50001, N'귀책을 선택하세요.', 1;
            IF @p_act_cd NOT IN ('LOSS', 'RTN') THROW 50001, N'처리 방법(손실/재입고)을 선택하세요.', 1;

            IF @p_act_cd = 'LOSS'
                EXEC USP_PR_TRANS_POST @acc, 'O', 'ADJ_OUT', @item, @trwh, @lot, @diff, 'XF', @p_xfer_id, @no, @sa, NULL, N'이전 차이 손실', @p_user_id, @p_client_pc, @date, @tid OUTPUT;
            ELSE
            BEGIN
                EXEC USP_PR_TRANS_POST @acc, 'O', 'MV_OUT', @item, @trwh, @lot, @diff, 'XF', @p_xfer_id, @no, @sa, NULL, N'이전 차이 재입고', @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                EXEC USP_PR_TRANS_POST @acc, 'I', 'MV_IN', @item, @twh, @lot, @diff, 'XF', @p_xfer_id, @no, @sb, NULL, N'이전 차이 재입고', @p_user_id, @p_client_pc, @date, @tid OUTPUT;
            END

            UPDATE TPRXFERD SET diff_resp_cd = @p_resp_cd, diff_act_cd = @p_act_cd, diff_dt = GETDATE(), diff_user_id = @p_user_id, diff_remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE xfer_id = @p_xfer_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @act IS NULL THROW 50001, N'차이 처리된 라인이 아닙니다.', 1;
            EXEC USP_PR_TRANS_REVERSE 'XF', @p_xfer_id, 2000, 2999, N'이전 차이 처리 취소', @p_user_id, @p_client_pc;
            UPDATE TPRXFERD SET diff_resp_cd = NULL, diff_act_cd = NULL, diff_dt = NULL, diff_user_id = NULL, diff_remark = NULL,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE xfer_id = @p_xfer_id AND serl = @p_serl;
        END

        UPDATE TPRXFERM SET
            stat_cd = CASE WHEN EXISTS (SELECT 1 FROM TPRXFERD WHERE xfer_id = @p_xfer_id AND diff_qty > 0 AND diff_act_cd IS NULL) THEN '2' ELSE 'E' END,
            upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        WHERE xfer_id = @p_xfer_id;
        COMMIT TRAN;

        SET @GeneratedCode = CAST(@p_xfer_id AS VARCHAR(20));
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
