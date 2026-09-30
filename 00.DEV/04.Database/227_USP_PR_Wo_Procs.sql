-- 생산관리(PR) 작업지시 프로시저 (2026-09-29). 226번(실적/이전)이 쓰는 작업지시(TPRWOM/TPRWOD)와 시작 LOT를 만든다.
--
--  작업지시 1건 = 웨이퍼 LOT 1개의 공정 체인. 저장(N)할 때
--    1) 라우팅(TPRROUTED)을 공정행(TPRWOD)으로 스냅샷 복사한다 - 이후 라우팅을 고쳐도 이미 낸 작업지시는 그대로.
--       공정행의 창고(wh_id)는 그 외주처의 외주창고(TBAWH.cust_id 연결, wh_type='OS')로 채운다.
--    2) 시작 LOT는 웨이퍼입고(TPRRCV, 234/235번)로 이미 입고된 미배정 웨이퍼 LOT를 골라 이 작업지시에 배정한다(TPRLOT.wo_id/wo_serl=0).
--       웨이퍼가 작업지시보다 먼저 도착하는 실제 순서를 따른다. 시작수량 = 그 LOT의 첫 공정 외주처 창고 재고 전량. 삭제하면 배정만 풀린다.
--  수정(U)은 일자/수주 연계/납기/담당/비고만. 시작 LOT/라우팅은 못 바꾼다(잘못 만들었으면 삭제 후 재작성).
--  삭제(D)는 실적/이전이 하나도 없을 때만.
--
--  USP_PR_WO_Q     Q: 헤더(0)+공정행(1)+LOT(2) / L: 목록(기간/상태/번호)
--  USP_PR_WO_S     헤더 N/U/D
--  USP_PR_WO_S_1   공정행 U(외주처/창고/단가/납기/분할수량/비고)
--  USP_PR_WO_C_S   상태: C 확정 / CC 확정취소 / X 중단 / XC 중단해제 / E 완료 / EC 완료취소  (계획 0 -> 확정 C -> 진행 1(첫 실적) -> 완료 E)

-- ============================================================
-- 1) USP_PR_WO_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_Q
    @p_work_type VARCHAR(50),               /* Q 단건 / L 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,          /* 시작 LOT 검색 */
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
            SELECT TOP 1 @match_id = wo_id FROM TPRWOM
            WHERE (@p_wo_id IS NULL OR wo_id = @p_wo_id)
              AND (@p_wo_id IS NOT NULL OR @p_wo_no IS NULL OR wo_no LIKE '%' + @p_wo_no + '%')
            ORDER BY wo_id DESC;

            SELECT
                m.wo_id, m.acc_id, m.wo_no, m.wo_date, m.route_id, r.route_cd, r.route_nm,
                m.item_id, i.item_no, i.item_nm, m.start_lot_no, m.start_qty,
                m.so_id, m.so_no, m.so_serl, m.delv_date, m.stat_cd,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.remark
            FROM TPRWOM m
                LEFT JOIN TPRROUTEM r ON r.route_id = m.route_id
                LEFT JOIN TBAITEM i ON i.item_id = m.item_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.wo_id = @match_id;

            SELECT
                dt.wo_id, dt.serl, dt.acc_id, dt.wo_no, dt.proc_cd, p.proc_nm,
                dt.cust_id, c.cust_nm, dt.wh_id, w.wh_nm,
                dt.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, dt.in_unit_cd,
                dt.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, dt.out_unit_cd,
                dt.split_qty, dt.price_unit_cd, dt.price, dt.due_date, dt.stat_cd,
                dt.in_qty, dt.good_qty, dt.bad_qty,
                CASE WHEN dt.in_qty > 0 AND ISNULL(dt.in_unit_cd, '') = ISNULL(dt.out_unit_cd, '') THEN dt.good_qty * 100.0 / dt.in_qty
                     WHEN dt.good_qty + dt.bad_qty > 0 THEN dt.good_qty * 100.0 / (dt.good_qty + dt.bad_qty) ELSE NULL END AS yield_rate,
                dt.remark
            FROM TPRWOD dt
                LEFT JOIN TBAPROC p ON p.acc_id = dt.acc_id AND p.proc_cd = dt.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = dt.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBAITEM ii ON ii.item_id = dt.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = dt.out_item_id
            WHERE dt.wo_id = @match_id
            ORDER BY dt.serl;

            -- 이 작업지시의 LOT와 현재 위치별 재고
            SELECT l.lot_id, l.lot_no, l.wo_serl, l.item_id, i.item_no, i.item_nm, l.unit_cd, l.init_qty,
                   s.wh_id, w.wh_nm, s.stock_qty
            FROM TPRLOT l
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.loc_id = 0 AND s.stock_qty <> 0
                LEFT JOIN TBAWH w ON w.wh_id = s.wh_id
            WHERE l.wo_id = @match_id
            ORDER BY l.wo_serl, l.lot_no;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.wo_id, m.wo_no, m.wo_date, m.route_nm, m.item_nm, m.start_lot_no, m.start_qty, m.so_no, m.delv_date, m.stat_cd,
                -- 진행 공정: 실적이 있는 마지막 공정 순번/이름과 그 공정 양품 누계
                pr.serl AS cur_serl, pr.proc_nm AS cur_proc_nm, pr.good_qty AS cur_good_qty, pr.out_unit_cd AS cur_unit_cd
            FROM (
                SELECT wm.*, r.route_nm, i.item_nm
                FROM TPRWOM wm
                    LEFT JOIN TPRROUTEM r ON r.route_id = wm.route_id
                    LEFT JOIN TBAITEM i ON i.item_id = wm.item_id
            ) m
                OUTER APPLY (
                    SELECT TOP 1 d.serl, p.proc_nm, d.good_qty, d.out_unit_cd
                    FROM TPRWOD d LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                    WHERE d.wo_id = m.wo_id AND d.in_qty > 0 ORDER BY d.serl DESC
                ) pr
            WHERE (@p_fr_date IS NULL OR m.wo_date >= @p_fr_date)
              AND (@p_to_date IS NULL OR m.wo_date <= @p_to_date)
              AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
              AND (@p_wo_no IS NULL OR m.wo_no LIKE '%' + @p_wo_no + '%')
              AND (@p_lot_no IS NULL OR m.start_lot_no LIKE '%' + @p_lot_no + '%')
            ORDER BY m.wo_id DESC;
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
-- 2) USP_PR_WO_S - 헤더 N/U/D
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_wo_date VARCHAR(8) = NULL,
    @p_route_id BIGINT = NULL,              /* N 전용 */
    @p_start_lot_no NVARCHAR(50) = NULL,    /* (사용 안 함 - 호환용) 시작 LOT는 @p_start_lot_id로 고른다 */
    @p_start_qty NUMERIC(18,4) = NULL,      /* (사용 안 함 - 호환용) 시작수량은 LOT의 첫 공정 창고 재고 전량 */
    @p_start_lot_id BIGINT = NULL,          /* N 전용: 웨이퍼입고(TPRRCV)로 입고된 미배정 웨이퍼 LOT(TPRLOT) */
    @p_so_id BIGINT = NULL,
    @p_so_serl INT = NULL,
    @p_delv_date VARCHAR(8) = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type IN ('U', 'D')
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TPRWOM WHERE wo_id = @p_wo_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시를 찾을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U') AND @p_so_id IS NOT NULL
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSASOM WHERE so_id = @p_so_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'연계할 수주를 찾을 수 없습니다.'; RETURN;
            END
            IF @p_so_serl IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_so_serl)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'연계할 수주 품목을 찾을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @route_item BIGINT, @route_use VARCHAR(1);
            SELECT @route_item = item_id, @route_use = use_yn FROM TPRROUTEM WHERE route_id = @p_route_id;
            IF @route_use IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅을 선택하세요.'; RETURN;
            END
            IF @route_use <> 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사용하지 않는 라우팅입니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TPRROUTED WHERE route_id = @p_route_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정이 없는 라우팅입니다.'; RETURN;
            END
            IF ISNULL(@p_start_lot_id, 0) = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고된 웨이퍼 LOT를 선택하세요. (먼저 웨이퍼입고 화면에서 입고를 확정해야 합니다)'; RETURN;
            END

            -- 시작 LOT = 웨이퍼입고로 들어온 미배정 LOT. 품목이 라우팅 첫 공정 투입품목이어야 하고, 첫 공정 외주처 창고에 재고가 있어야 한다.
            -- 시작수량은 그 창고 재고 전량(한 작업지시 = 웨이퍼 LOT 하나 통째로).
            DECLARE @first_serl INT, @first_item BIGINT, @first_unit VARCHAR(10), @first_wh BIGINT, @lot_no NVARCHAR(50), @lot_item BIGINT, @lot_wo BIGINT, @stk NUMERIC(18,4);
            SELECT TOP 1 @first_serl = d.serl, @first_item = d.in_item_id, @first_unit = d.in_unit_cd,
                   @first_wh = (SELECT TOP 1 w.wh_id FROM TBAWH w WHERE w.acc_id = @p_acc_id AND w.cust_id = d.cust_id AND w.wh_type = 'OS' ORDER BY w.wh_id)
            FROM TPRROUTED d WHERE d.route_id = @p_route_id ORDER BY d.serl;

            SELECT @lot_no = lot_no, @lot_item = item_id, @lot_wo = wo_id FROM TPRLOT WHERE lot_id = @p_start_lot_id AND acc_id = @p_acc_id;
            IF @lot_no IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'선택한 웨이퍼 LOT를 찾을 수 없습니다.'; RETURN;
            END
            IF @lot_wo IS NOT NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 다른 작업지시에 배정된 LOT입니다.'; RETURN;
            END
            IF @lot_item <> @first_item
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'LOT의 품목이 이 라우팅 첫 공정의 투입품목과 다릅니다.'; RETURN;
            END

            IF @first_wh IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅 첫 공정의 외주처에 연결된 외주창고가 없습니다.'; RETURN;
            END
            SELECT @stk = stock_qty FROM TMASTOCK WHERE acc_id = @p_acc_id AND item_id = @lot_item AND wh_id = @first_wh AND loc_id = 0 AND lot_no = @lot_no;
            IF ISNULL(@stk, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'웨이퍼 LOT ' + @lot_no + N'의 재고가 첫 공정 외주처 창고에 없습니다. 웨이퍼입고 화면에서 그 창고로 입고했는지 확인하세요.'; RETURN;
            END
            SET @p_start_qty = @stk;
            SET @p_start_lot_no = @lot_no;

            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TPRWOM', 'wo_no', @p_acc_id, @new_no OUTPUT;

            BEGIN TRAN;

            INSERT INTO TPRWOM (
                acc_id, wo_no, wo_date, route_id, item_id, start_lot_no, start_qty, so_id, so_no, so_serl, delv_date, stat_cd, dept_id, emp_id, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_wo_date, @p_route_id, @route_item, @p_start_lot_no, @p_start_qty,
                @p_so_id, (SELECT so_no FROM TSASOM WHERE so_id = @p_so_id), @p_so_serl, @p_delv_date, '0', @p_dept_id, @p_emp_id, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );
            SET @p_wo_id = SCOPE_IDENTITY();

            INSERT INTO TPRWOD (
                wo_id, serl, acc_id, wo_no, proc_cd, cust_id, wh_id, in_item_id, out_item_id, in_unit_cd, out_unit_cd, split_qty, price_unit_cd, price, stat_cd,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT @p_wo_id, d.serl, @p_acc_id, @new_no, d.proc_cd, d.cust_id,
                   (SELECT TOP 1 w.wh_id FROM TBAWH w WHERE w.acc_id = @p_acc_id AND w.cust_id = d.cust_id AND w.wh_type = 'OS' ORDER BY w.wh_id),
                   d.in_item_id, d.out_item_id, d.in_unit_cd, d.out_unit_cd, d.split_qty, d.price_unit_cd, d.price, '0',
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TPRROUTED d WHERE d.route_id = @p_route_id;

            -- 입고된 LOT를 이 작업지시의 시작 LOT로 배정한다(LOT 자체는 웨이퍼입고 때 이미 만들어져 있다).
            UPDATE TPRLOT SET wo_id = @p_wo_id, wo_serl = 0, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE lot_id = @p_start_lot_id;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRWOM SET
                wo_date = @p_wo_date,
                so_id = @p_so_id, so_no = (SELECT so_no FROM TSASOM WHERE so_id = @p_so_id), so_serl = @p_so_serl,
                delv_date = @p_delv_date, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE wo_id = @p_wo_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TPRRSLTM WHERE wo_id = @p_wo_id) OR EXISTS (SELECT 1 FROM TPRXFERM WHERE wo_id = @p_wo_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'실적이나 이전이 등록된 작업지시는 삭제할 수 없습니다. 중단 처리하세요.'; RETURN;
            END
            BEGIN TRAN;
            -- 시작 LOT는 웨이퍼입고로 만들어진 것이라 지우지 않고 배정만 푼다(다른 작업지시에 다시 쓸 수 있게). 입고 기록도 재고도 없는 옛 LOT(고아)만 지운다.
            UPDATE TPRLOT SET wo_id = NULL, wo_serl = NULL WHERE wo_id = @p_wo_id AND wo_serl = 0;
            DELETE l FROM TPRLOT l
            WHERE l.wo_id IS NULL AND l.lot_no = (SELECT start_lot_no FROM TPRWOM WHERE wo_id = @p_wo_id)
              AND NOT EXISTS (SELECT 1 FROM TPRRCV r WHERE r.acc_id = l.acc_id AND r.item_id = l.item_id AND r.lot_no = l.lot_no)
              AND NOT EXISTS (SELECT 1 FROM TMASTOCK s WHERE s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.stock_qty <> 0);
            DELETE FROM TPRWOD WHERE wo_id = @p_wo_id;
            DELETE FROM TPRWOM WHERE wo_id = @p_wo_id;
            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_wo_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 3) USP_PR_WO_S_1 - 공정행 U. 외주처/창고는 그 공정에 실적/이전이 없을 때만 바꾼다. 외주처만 바꾸고 창고를 비우면 그 외주처의 외주창고로 채운다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_S_1
    @p_work_type VARCHAR(50),               /* U */
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_cust_id BIGINT = NULL,
    @p_wh_id BIGINT = NULL,
    @p_split_qty NUMERIC(18,4) = NULL,
    @p_price_unit_cd VARCHAR(10) = NULL,
    @p_price NUMERIC(18,4) = NULL,
    @p_due_date VARCHAR(8) = NULL,
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
        DECLARE @acc BIGINT, @old_cust BIGINT, @old_wh BIGINT, @wo_stat VARCHAR(10);
        SELECT @acc = d.acc_id, @old_cust = d.cust_id, @old_wh = d.wh_id, @wo_stat = m.stat_cd
        FROM TPRWOD d JOIN TPRWOM m ON m.wo_id = d.wo_id WHERE d.wo_id = @p_wo_id AND d.serl = @p_serl;

        IF @acc IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시 공정을 찾을 수 없습니다.'; RETURN;
        END
        IF @wo_stat IN ('E', 'X')
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'완료되었거나 중단된 작업지시는 수정할 수 없습니다.'; RETURN;
        END

        IF @p_cust_id IS NOT NULL AND @p_wh_id IS NULL
            SELECT TOP 1 @p_wh_id = wh_id FROM TBAWH WHERE acc_id = @acc AND cust_id = @p_cust_id AND wh_type = 'OS' ORDER BY wh_id;

        IF (ISNULL(@p_cust_id, 0) <> ISNULL(@old_cust, 0) OR ISNULL(@p_wh_id, 0) <> ISNULL(@old_wh, 0))
           AND (EXISTS (SELECT 1 FROM TPRRSLTM WHERE wo_id = @p_wo_id AND wo_serl = @p_serl)
                OR EXISTS (SELECT 1 FROM TPRXFERM WHERE wo_id = @p_wo_id AND (from_serl = @p_serl OR to_serl = @p_serl)))
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'실적이나 이전이 등록된 공정은 외주처/창고를 바꿀 수 없습니다.'; RETURN;
        END
        IF @p_wh_id IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TBAWH WHERE wh_id = @p_wh_id)
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'창고를 찾을 수 없습니다.'; RETURN;
        END
        IF ISNULL(@p_split_qty, 1) <= 0
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'분할 수량은 0보다 커야 합니다.'; RETURN;
        END

        UPDATE TPRWOD SET
            cust_id = @p_cust_id, wh_id = @p_wh_id, split_qty = @p_split_qty, price_unit_cd = @p_price_unit_cd, price = @p_price,
            due_date = @p_due_date, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        WHERE wo_id = @p_wo_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_wo_id AS VARCHAR(20));
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
