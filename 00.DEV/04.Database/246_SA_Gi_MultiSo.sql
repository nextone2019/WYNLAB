-- 출하를 "한 고객 · 여러 수주" 구조로 변경 (2026-10-01): 헤더는 고객(+선적정보), 라인이 수주 라인을 각각 가진다(TSAGID.so_id/so_serl).
-- TSAGIM.so_id/so_no 컬럼은 더 이상 쓰지 않는다(호환용으로 남김, NULL). 같은 고객의 수주끼리만 한 출하에 담는다.

CREATE OR ALTER PROCEDURE USP_SA_GI_Q
    @p_work_type VARCHAR(50),               /* Q 단건 / L 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_gi_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_so_no VARCHAR(20) = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
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
        BEGIN
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = gi_id FROM TSAGIM
            WHERE (@p_gi_id IS NULL OR gi_id = @p_gi_id)
              AND (@p_gi_id IS NOT NULL OR @p_gi_no IS NULL OR gi_no LIKE '%' + @p_gi_no + '%')
            ORDER BY gi_id DESC;

            SELECT
                m.gi_id, m.acc_id, m.gi_no, m.gi_date, m.cust_id, c.cust_nm, m.so_id, m.so_no, m.ship_kind, m.ship_date,
                m.carrier, m.bl_no, m.dest, m.dept_id, dp.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT dp ON dp.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.gi_id = @match_id;

            SELECT
                d.gi_id, d.serl, d.acc_id, d.gi_no, d.so_id, d.so_serl, d.item_id, i.item_no, i.item_nm, d.unit_cd,
                so.so_no, d.lot_id, d.lot_no, d.wh_id, w.wh_nm, d.qty, d.price, d.amt, d.remark,
                sd.qty AS so_qty, ISNULL(sd.next_qty, 0) AS so_next_qty, sd.qty - ISNULL(sd.next_qty, 0) AS so_remain_qty,
                ISNULL(st.stock_qty, 0) AS stock_qty
            FROM TSAGID d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
                LEFT JOIN TMASTOCK st ON st.acc_id = d.acc_id AND st.item_id = d.item_id AND st.wh_id = d.wh_id AND st.loc_id = 0 AND st.lot_no = ISNULL(d.lot_no, N'')
            WHERE d.gi_id = @match_id
            ORDER BY d.serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.gi_id, m.gi_no, m.gi_date, c.cust_nm, t.so_list AS so_no, m.ship_kind, m.ship_date, m.carrier, m.bl_no, m.stat_cd,
                t.line_cnt, t.total_qty, t.lot_list
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                OUTER APPLY (SELECT COUNT(*) AS line_cnt, SUM(d.qty) AS total_qty,
                                    (SELECT STRING_AGG(x.lot_no, ', ') WITHIN GROUP (ORDER BY x.serl) FROM TSAGID x WHERE x.gi_id = m.gi_id AND ISNULL(x.lot_no, N'') <> N'') AS lot_list,
                                    (SELECT STRING_AGG(z.so_no, ', ') FROM (SELECT DISTINCT so2.so_no FROM TSAGID x2 JOIN TSASOM so2 ON so2.so_id = x2.so_id WHERE x2.gi_id = m.gi_id) z) AS so_list
                             FROM TSAGID d WHERE d.gi_id = m.gi_id) t
            WHERE (ISNULL(@p_fr_date, '') = '' OR m.gi_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.gi_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_gi_no, '') = '' OR m.gi_no LIKE '%' + @p_gi_no + '%')
              AND (ISNULL(@p_so_no, '') = '' OR EXISTS (SELECT 1 FROM TSAGID x3 JOIN TSASOM so3 ON so3.so_id = x3.so_id WHERE x3.gi_id = m.gi_id AND so3.so_no LIKE '%' + @p_so_no + '%'))
              AND (ISNULL(@p_cust_nm, N'') = N'' OR c.cust_nm LIKE N'%' + @p_cust_nm + N'%')
              AND (ISNULL(@p_lot_no, N'') = N'' OR EXISTS (SELECT 1 FROM TSAGID x WHERE x.gi_id = m.gi_id AND x.lot_no LIKE N'%' + @p_lot_no + N'%'))
            ORDER BY m.gi_date DESC, m.gi_id DESC;
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

CREATE OR ALTER PROCEDURE USP_SA_GI_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_gi_date VARCHAR(8) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_ship_kind VARCHAR(1) = NULL,
    @p_ship_date VARCHAR(8) = NULL,
    @p_carrier NVARCHAR(100) = NULL,
    @p_bl_no NVARCHAR(100) = NULL,
    @p_dest NVARCHAR(200) = NULL,
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
        DECLARE @stat VARCHAR(10);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_gi_date, '') = '' THROW 50001, N'출하일자를 입력하세요.', 1;
            IF @p_cust_id IS NULL THROW 50001, N'고객을 선택하세요.', 1;
            IF NOT EXISTS (SELECT 1 FROM TBACUST WHERE cust_id = @p_cust_id) THROW 50001, N'고객을 찾을 수 없습니다.', 1;
            IF ISNULL(@p_ship_kind, '') NOT IN ('W', 'D') THROW 50001, N'출하구분을 선택하세요.', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSAGIM', 'gi_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TSAGIM (acc_id, gi_no, gi_date, cust_id, ship_kind, ship_date, carrier, bl_no, dest, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_gi_date, @p_cust_id, @p_ship_kind, @p_ship_date, @p_carrier, @p_bl_no, @p_dest, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_gi_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
            IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 출하는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;
            -- 라인(수주 품목)이 이미 있으면 고객을 바꿀 수 없다(한 출하 = 한 고객)
            IF EXISTS (SELECT 1 FROM TSAGID d JOIN TSASOM so ON so.so_id = d.so_id WHERE d.gi_id = @p_gi_id AND so.cust_id <> @p_cust_id)
                THROW 50001, N'이미 다른 고객의 수주 품목이 담겨 있어 고객을 바꿀 수 없습니다. 라인을 지운 뒤 바꾸세요.', 1;

            UPDATE TSAGIM SET gi_date = @p_gi_date, cust_id = @p_cust_id, ship_kind = @p_ship_kind, ship_date = @p_ship_date,
                carrier = @p_carrier, bl_no = @p_bl_no, dest = @p_dest, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            SELECT @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
            IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 출하는 삭제할 수 없습니다. 먼저 확정취소하세요.', 1;
            DELETE FROM TSAGID WHERE gi_id = @p_gi_id;
            DELETE FROM TSAGIM WHERE gi_id = @p_gi_id;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_gi_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_SA_GI_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_so_id BIGINT = NULL,                 /* N 전용: 이 라인의 수주(한 출하에 여러 수주 가능) */
    @p_so_serl INT = NULL,
    @p_lot_id BIGINT = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
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
        DECLARE @acc BIGINT, @gi_no VARCHAR(20), @cust BIGINT, @stat VARCHAR(10);
        SELECT @acc = acc_id, @gi_no = gi_no, @cust = cust_id, @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
        IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 출하는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_qty, 0) <= 0 THROW 50001, N'출하수량은 0보다 커야 합니다.', 1;
            IF @p_wh_id IS NULL THROW 50001, N'출하 창고를 선택하세요. (출하 LOT 불러오기)', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @item BIGINT, @unit VARCHAR(10), @price NUMERIC(18,4), @so_qty NUMERIC(18,4);
            SELECT @item = item_id, @unit = unit_cd, @price = price, @so_qty = qty FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_so_serl;
            IF @item IS NULL THROW 50001, N'수주 라인을 찾을 수 없습니다.', 1;
            IF ISNULL((SELECT cust_id FROM TSASOM WHERE so_id = @p_so_id), -1) <> ISNULL(@cust, -2) THROW 50001, N'이 출하의 고객과 수주의 고객이 다릅니다. 한 출하에는 같은 고객의 수주만 담을 수 있습니다.', 1;
            IF @p_qty > @so_qty THROW 50001, N'출하수량이 수주수량을 넘을 수 없습니다.', 1;

            DECLARE @new_serl INT = (SELECT ISNULL(MAX(serl), 0) + 1 FROM TSAGID WHERE gi_id = @p_gi_id);
            INSERT INTO TSAGID (gi_id, serl, acc_id, gi_no, so_id, so_serl, item_id, unit_cd, lot_id, lot_no, wh_id, qty, price, amt, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_gi_id, @new_serl, @acc, @gi_no, @p_so_id, @p_so_serl, @item, @unit, @p_lot_id, @p_lot_no, @p_wh_id, @p_qty, @price, ROUND(@p_qty * ISNULL(@price, 0), 4), @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_serl = @new_serl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl) THROW 50001, N'출하 라인을 찾을 수 없습니다.', 1;
            UPDATE TSAGID SET lot_id = @p_lot_id, lot_no = @p_lot_no, wh_id = @p_wh_id, qty = @p_qty, amt = ROUND(@p_qty * ISNULL(price, 0), 4), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl;
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_gi_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_SA_GI_C_S
    @p_work_type VARCHAR(50),               /* C / CC */
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
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
        DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @stat VARCHAR(10);
        SELECT @acc = acc_id, @no = gi_no, @date = gi_date, @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
        IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 출하만 확정할 수 있습니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id) THROW 50001, N'출하할 품목(LOT)을 추가한 뒤 확정하세요.', 1;

            BEGIN TRAN;

            DECLARE @serl INT, @item BIGINT, @wh BIGINT, @lot NVARCHAR(50), @qty NUMERIC(18,4), @rmk NVARCHAR(3000), @tid BIGINT;
            DECLARE c CURSOR LOCAL FAST_FORWARD FOR
                SELECT serl, item_id, wh_id, lot_no, qty, remark FROM TSAGID WHERE gi_id = @p_gi_id ORDER BY serl;
            OPEN c;
            FETCH NEXT FROM c INTO @serl, @item, @wh, @lot, @qty, @rmk;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                -- 재고가 모자라면 여기서 현재고 부족으로 거부된다(마이너스 재고 불허).
                EXEC USP_PR_TRANS_POST @acc, 'O', 'SA_OUT', @item, @wh, @lot, @qty, 'GI', @p_gi_id, @no, @serl, NULL, @rmk, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                FETCH NEXT FROM c INTO @serl, @item, @wh, @lot, @qty, @rmk;
            END
            CLOSE c; DEALLOCATE c;

            UPDATE TSAGIM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id;

            -- 수주 라인의 출하누계 재계산(확정된 출하 합계) - 수주수량을 넘으면 거부
            UPDATE s SET next_qty = (SELECT ISNULL(SUM(d.qty), 0) FROM TSAGID d JOIN TSAGIM m ON m.gi_id = d.gi_id AND m.stat_cd = 'C' WHERE d.so_id = s.so_id AND d.so_serl = s.serl),
                         upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            FROM TSASOD s WHERE EXISTS (SELECT 1 FROM TSAGID g WHERE g.gi_id = @p_gi_id AND g.so_id = s.so_id AND g.so_serl = s.serl);

            IF EXISTS (SELECT 1 FROM TSASOD s WHERE EXISTS (SELECT 1 FROM TSAGID g WHERE g.gi_id = @p_gi_id AND g.so_id = s.so_id AND g.so_serl = s.serl) AND s.next_qty > s.qty)
                THROW 50001, N'수주수량을 초과하여 출하할 수 없습니다. (이미 확정된 다른 출하를 포함한 합계가 수주수량보다 큽니다)', 1;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정된 출하만 확정취소할 수 있습니다.', 1;

            BEGIN TRAN;
            EXEC USP_PR_TRANS_REVERSE 'GI', @p_gi_id, 0, 999999, N'출하 확정취소', @p_user_id, @p_client_pc;

            UPDATE TSAGIM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id;

            UPDATE s SET next_qty = (SELECT ISNULL(SUM(d.qty), 0) FROM TSAGID d JOIN TSAGIM m ON m.gi_id = d.gi_id AND m.stat_cd = 'C' WHERE d.so_id = s.so_id AND d.so_serl = s.serl),
                         upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            FROM TSASOD s WHERE EXISTS (SELECT 1 FROM TSAGID g WHERE g.gi_id = @p_gi_id AND g.so_id = s.so_id AND g.so_serl = s.serl);
            COMMIT TRAN;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_gi_id AS VARCHAR(20));
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
