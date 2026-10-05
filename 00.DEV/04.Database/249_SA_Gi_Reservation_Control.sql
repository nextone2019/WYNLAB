-- 출하 통제: 재고 차감(확정) 전이라도 미확정 출하가 잡은 LOT 수량/수주 잔량을 다른 출하가 또 잡지 못하게 한다 (2026-10-01).
--  - LOT 행 저장 시: 가용재고(현재고 - 다른 미확정 출하의 같은 LOT/창고 합계) 초과 거부, 수주 잔량(수주수량 - 확정 출하누계 - 다른 미확정 출하 품목) 초과 거부
--  - 수주 품목/출하 LOT 선택 팝업: 잔량/가용재고를 같은 기준으로 보여주고, 이미 다 잡힌 것은 제외/0으로 표시
--  확정 때는 기존처럼 실제 현재고와 수주수량으로 한 번 더 검사한다.

CREATE OR ALTER PROCEDURE USP_SA_GI_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,                     /* 출하 품목 번호 */
    @p_lot_serl INT = NULL,
    @p_lot_id BIGINT = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_ship_kind VARCHAR(1) = NULL,         /* W 자사창고 / D 외주처 직송 - 선택한 창고 종류와 맞아야 한다 */
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
        DECLARE @acc BIGINT, @stat VARCHAR(10), @item BIGINT, @so_id BIGINT, @so_serl INT, @price NUMERIC(18,4);
        SELECT @acc = m.acc_id, @stat = m.stat_cd, @item = d.item_id, @so_id = d.so_id, @so_serl = d.so_serl, @price = d.price
        FROM TSAGIM m JOIN TSAGID d ON d.gi_id = m.gi_id AND d.serl = @p_serl WHERE m.gi_id = @p_gi_id;
        IF @stat IS NULL THROW 50001, N'출하 품목을 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 출하는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_qty, 0) <= 0 THROW 50001, N'출하수량은 0보다 커야 합니다.', 1;
            IF @p_wh_id IS NULL THROW 50001, N'출하 창고를 선택하세요. (출하 LOT 선택)', 1;
            IF ISNULL(@p_ship_kind, '') NOT IN ('W', 'D') THROW 50001, N'출하구분(자사창고/외주처)이 없습니다.', 1;
            DECLARE @whtype VARCHAR(10) = (SELECT wh_type FROM TBAWH WHERE wh_id = @p_wh_id);
            IF (@p_ship_kind = 'D' AND ISNULL(@whtype, '') <> 'OS') OR (@p_ship_kind = 'W' AND ISNULL(@whtype, '') = 'OS')
                THROW 50001, N'출하구분과 창고 종류가 맞지 않습니다. (외주처 직송은 외주처 창고, 자사창고 출하는 자사 창고)', 1;

            -- 같은 LOT/창고를 다른 미확정 출하(또는 이 출하의 다른 행)가 이미 잡은 수량을 뺀 가용재고보다 많이 출하할 수 없다(재고 차감은 확정 때지만 배정은 저장 때부터 잡는다).
            IF ISNULL((SELECT stock_yn FROM TBAITEM WHERE item_id = @item), 'N') = 'Y'
            BEGIN
                DECLARE @stock NUMERIC(18,4) = ISNULL((SELECT SUM(stock_qty) FROM TMASTOCK WHERE acc_id = @acc AND item_id = @item AND wh_id = @p_wh_id AND loc_id = 0 AND lot_no = ISNULL(@p_lot_no, N'')), 0);
                DECLARE @reserved NUMERIC(18,4) = ISNULL((
                    SELECT SUM(l.qty) FROM TSAGIL l JOIN TSAGIM m ON m.gi_id = l.gi_id
                    WHERE m.stat_cd = '0' AND l.acc_id = @acc AND l.item_id = @item AND l.wh_id = @p_wh_id AND ISNULL(l.lot_no, N'') = ISNULL(@p_lot_no, N'')
                      AND NOT (l.gi_id = @p_gi_id AND l.serl = @p_serl AND l.lot_serl = ISNULL(@p_lot_serl, -1))), 0);
                IF @reserved + @p_qty > @stock
                BEGIN
                    DECLARE @m2 NVARCHAR(400) = N'가용재고가 부족합니다. LOT: ' + ISNULL(@p_lot_no, N'') + N', 현재고 ' + CAST(CAST(@stock AS FLOAT) AS NVARCHAR(30))
                        + N' - 다른 미확정 출하가 잡은 수량 ' + CAST(CAST(@reserved AS FLOAT) AS NVARCHAR(30)) + N' = 가용 ' + CAST(CAST(@stock - @reserved AS FLOAT) AS NVARCHAR(30))
                        + N' < 요청 ' + CAST(CAST(@p_qty AS FLOAT) AS NVARCHAR(30));
                    THROW 50001, @m2, 1;
                END
            END

            -- 이 수주 라인의 출하누계(확정) + 다른 미확정 출하 품목이 잡은 수량 + 이 품목의 LOT 합계가 수주수량을 넘으면 거부
            DECLARE @so_qty NUMERIC(18,4), @so_next NUMERIC(18,4);
            SELECT @so_qty = qty, @so_next = ISNULL(next_qty, 0) FROM TSASOD WHERE so_id = @so_id AND serl = @so_serl;
            DECLARE @others NUMERIC(18,4) = (SELECT ISNULL(SUM(qty), 0) FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND (@p_work_type = 'N' OR lot_serl <> @p_lot_serl));
            DECLARE @so_reserved NUMERIC(18,4) = ISNULL((
                SELECT SUM(d.qty) FROM TSAGID d JOIN TSAGIM m ON m.gi_id = d.gi_id
                WHERE m.stat_cd = '0' AND d.so_id = @so_id AND d.so_serl = @so_serl AND NOT (d.gi_id = @p_gi_id AND d.serl = @p_serl)), 0);
            IF @so_next + @so_reserved + @others + @p_qty > @so_qty
            BEGIN
                DECLARE @m3 NVARCHAR(400) = N'수주 출하 잔량을 넘습니다. 수주수량 ' + CAST(CAST(@so_qty AS FLOAT) AS NVARCHAR(30)) + N' - 출하확정 ' + CAST(CAST(@so_next AS FLOAT) AS NVARCHAR(30))
                    + N' - 다른 미확정 출하 ' + CAST(CAST(@so_reserved AS FLOAT) AS NVARCHAR(30)) + N' = 잔량 ' + CAST(CAST(@so_qty - @so_next - @so_reserved AS FLOAT) AS NVARCHAR(30))
                    + N' < 이 품목 합계 ' + CAST(CAST(@others + @p_qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @m3, 1;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_lot_serl IS NULL SET @p_lot_serl = (SELECT ISNULL(MAX(lot_serl), 0) + 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl);
            ELSE IF EXISTS (SELECT 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl) THROW 50001, N'LOT 번호가 중복입니다.', 1;

            INSERT INTO TSAGIL (gi_id, serl, lot_serl, acc_id, item_id, ship_kind, lot_id, lot_no, wh_id, qty, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_gi_id, @p_serl, @p_lot_serl, @acc, @item, @p_ship_kind, @p_lot_id, @p_lot_no, @p_wh_id, @p_qty, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl) THROW 50001, N'출하 LOT 행을 찾을 수 없습니다.', 1;
            UPDATE TSAGIL SET ship_kind = @p_ship_kind, lot_id = @p_lot_id, lot_no = @p_lot_no, wh_id = @p_wh_id, qty = @p_qty, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl;
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        -- 품목 행 수량/금액 = LOT 합계
        UPDATE TSAGID SET qty = (SELECT ISNULL(SUM(qty), 0) FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl),
            amt = ROUND((SELECT ISNULL(SUM(qty), 0) FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl) * ISNULL(price, 0), 4),
            upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        WHERE gi_id = @p_gi_id AND serl = @p_serl;

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

CREATE OR ALTER PROCEDURE USP_SA_GISOPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 수주번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명 */
    @p_gi_id BIGINT = NULL,                 /* 지금 편집 중인 출하(이 출하가 잡은 수량은 '다른 미확정 출하'에서 제외) */
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
                m.so_id, d.serl AS so_serl, m.so_no, m.so_date, m.cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, d.unit_cd, d.qty, ISNULL(d.next_qty, 0) AS next_qty, ISNULL(rs.reserved_qty, 0) AS reserved_qty, d.qty - ISNULL(d.next_qty, 0) - ISNULL(rs.reserved_qty, 0) AS remain_qty,
                d.price, ISNULL(d.delv_date, m.delv_date) AS delv_date
            FROM TSASOM m
                JOIN TSASOD d ON d.so_id = m.so_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                OUTER APPLY (SELECT SUM(g.qty) AS reserved_qty FROM TSAGID g JOIN TSAGIM gm ON gm.gi_id = g.gi_id
                             WHERE gm.stat_cd = '0' AND g.so_id = d.so_id AND g.so_serl = d.serl AND (@p_gi_id IS NULL OR g.gi_id <> @p_gi_id)) rs
            WHERE ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y'
              AND d.qty - ISNULL(d.next_qty, 0) - ISNULL(rs.reserved_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.so_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.so_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.so_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR m.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = '' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.so_date DESC, m.so_no, d.serl;
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

CREATE OR ALTER PROCEDURE USP_SA_GILOTPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_item_id BIGINT = NULL,
    @p_gi_id BIGINT = NULL,                 /* 지금 편집 중인 출하(이 출하가 잡은 수량은 '다른 미확정 출하'에서 제외) */
    @p_ship_kind VARCHAR(1) = NULL,         /* W 자사창고 출하 -> 외주처(OS) 창고 제외 / D 외주처 직송 -> 외주처 창고만. 비면 전체 */
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
                l.lot_id, s.lot_no, s.item_id, i.item_no, i.item_nm, s.unit_cd, s.wh_id, w.wh_nm, s.stock_qty, ISNULL(rs.reserved_qty, 0) AS reserved_qty, s.stock_qty - ISNULL(rs.reserved_qty, 0) AS avail_qty, rs.reserved_docs,
                wm.wo_no, ISNULL(w.wh_type, '') AS wh_type,
                CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN 'D' ELSE 'W' END AS ship_kind,
                CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN N'외주처' ELSE N'자사창고' END AS ship_kind_nm
            FROM TMASTOCK s
                JOIN TBAITEM i ON i.item_id = s.item_id
                JOIN TBAWH w ON w.wh_id = s.wh_id
                LEFT JOIN TPRLOT l ON l.acc_id = s.acc_id AND l.item_id = s.item_id AND l.lot_no = s.lot_no
                OUTER APPLY (SELECT SUM(x.qty) AS reserved_qty, STRING_AGG(xm.gi_no, ', ') AS reserved_docs
                             FROM TSAGIL x JOIN TSAGIM xm ON xm.gi_id = x.gi_id
                             WHERE xm.stat_cd = '0' AND x.acc_id = s.acc_id AND x.item_id = s.item_id AND x.wh_id = s.wh_id AND ISNULL(x.lot_no, N'') = s.lot_no
                               AND (@p_gi_id IS NULL OR x.gi_id <> @p_gi_id)) rs
                LEFT JOIN TPRWOM wm ON wm.wo_id = l.wo_id
            WHERE s.stock_qty > 0 AND s.loc_id = 0 AND ISNULL(w.wh_type, '') <> 'TR'
              AND (@p_acc_id IS NULL OR s.acc_id = @p_acc_id)
              AND (@p_item_id IS NULL OR s.item_id = @p_item_id)
              AND (ISNULL(@p_ship_kind, '') = ''
                   OR (@p_ship_kind = 'D' AND ISNULL(w.wh_type, '') = 'OS')
                   OR (@p_ship_kind = 'W' AND ISNULL(w.wh_type, '') <> 'OS'))
              AND (@p_keyword IS NULL OR @p_keyword = '' OR s.lot_no LIKE '%' + @p_keyword + '%' OR w.wh_nm LIKE '%' + @p_keyword + '%')
            ORDER BY s.lot_no, w.wh_nm;
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

-- 출하 품목의 "수주 잔량" 표시도 다른 미확정 출하가 잡은 수량을 뺀다(이 출하의 다른 품목 행 제외는 화면이 계산)
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
                m.gi_id, m.acc_id, m.gi_no, m.gi_date, m.cust_id, c.cust_nm, m.ship_date,
                m.carrier, m.bl_no, m.dest, m.dept_id, dp.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT dp ON dp.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.gi_id = @match_id;

            SELECT
                d.gi_id, d.serl, d.acc_id, d.gi_no, d.so_id, so.so_no, d.so_serl, d.item_id, i.item_no, i.item_nm, d.unit_cd,
                d.qty, d.price, d.amt, d.remark,
                sd.qty AS so_qty, ISNULL(sd.next_qty, 0) AS so_next_qty, sd.qty - ISNULL(sd.next_qty, 0) - ISNULL((SELECT SUM(g2.qty) FROM TSAGID g2 JOIN TSAGIM m2 ON m2.gi_id = g2.gi_id WHERE m2.stat_cd = '0' AND g2.so_id = d.so_id AND g2.so_serl = d.so_serl AND g2.gi_id <> d.gi_id), 0) AS so_remain_qty
            FROM TSAGID d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
            WHERE d.gi_id = @match_id
            ORDER BY d.serl;

            SELECT
                l.gi_id, l.serl, l.lot_serl, l.acc_id, l.item_id, l.ship_kind, l.lot_id, l.lot_no, l.wh_id, w.wh_nm, l.qty, l.remark,
                ISNULL(st.stock_qty, 0) AS stock_qty
            FROM TSAGIL l
                LEFT JOIN TBAWH w ON w.wh_id = l.wh_id
                LEFT JOIN TMASTOCK st ON st.acc_id = l.acc_id AND st.item_id = l.item_id AND st.wh_id = l.wh_id AND st.loc_id = 0 AND st.lot_no = ISNULL(l.lot_no, N'')
            WHERE l.gi_id = @match_id
            ORDER BY l.serl, l.lot_serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.gi_id, m.gi_no, m.gi_date, c.cust_nm, t.so_list AS so_no, t.kind_nm AS ship_kind_nm, m.ship_date, m.carrier, m.bl_no, m.stat_cd,
                t.line_cnt, t.total_qty, t.lot_list
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                OUTER APPLY (SELECT COUNT(*) AS line_cnt, SUM(d.qty) AS total_qty,
                                    (SELECT STRING_AGG(x.lot_no, ', ') WITHIN GROUP (ORDER BY x.serl, x.lot_serl) FROM TSAGIL x WHERE x.gi_id = m.gi_id AND ISNULL(x.lot_no, N'') <> N'') AS lot_list,
                                    (SELECT STRING_AGG(z.so_no, ', ') FROM (SELECT DISTINCT so2.so_no FROM TSAGID x2 JOIN TSASOM so2 ON so2.so_id = x2.so_id WHERE x2.gi_id = m.gi_id) z) AS so_list,
                                    (SELECT STRING_AGG(z.nm, ', ') FROM (SELECT DISTINCT CASE x4.ship_kind WHEN 'D' THEN N'외주처 직송' ELSE N'자사창고' END AS nm FROM TSAGIL x4 WHERE x4.gi_id = m.gi_id) z) AS kind_nm
                             FROM TSAGID d WHERE d.gi_id = m.gi_id) t
            WHERE (ISNULL(@p_fr_date, '') = '' OR m.gi_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.gi_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_gi_no, '') = '' OR m.gi_no LIKE '%' + @p_gi_no + '%')
              AND (ISNULL(@p_so_no, '') = '' OR EXISTS (SELECT 1 FROM TSAGID x3 JOIN TSASOM so3 ON so3.so_id = x3.so_id WHERE x3.gi_id = m.gi_id AND so3.so_no LIKE '%' + @p_so_no + '%'))
              AND (ISNULL(@p_cust_nm, N'') = N'' OR c.cust_nm LIKE N'%' + @p_cust_nm + N'%')
              AND (ISNULL(@p_lot_no, N'') = N'' OR EXISTS (SELECT 1 FROM TSAGIL x WHERE x.gi_id = m.gi_id AND x.lot_no LIKE N'%' + @p_lot_no + N'%'))
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
