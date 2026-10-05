-- 영업(SA) 출고(구 출하) 개편 (2026-10-05): 출고 품목은 확정된 거래명세서 라인에서 불러온다.
--  - USP_SA_GIINVCPICK_Q 신규(명세서 라인 선택). 기존 USP_SA_GISOPICK_Q(수주 라인 선택)는 삭제.
--  - 출고 가능 수량 = 명세서수량 - 확정 출고누계(TSAINVCD.gi_qty) - 다른 미확정 출고가 잡은 수량. 수주 next_qty는 이제 거래명세서 확정에서만 갱신한다.
--  - 출고확정/취소는 재고 차감/복원(USP_PR_TRANS_POST/REVERSE) + 명세서 라인 gi_qty 재계산. LOT/창고/외주처 직송 규칙은 그대로.
--  - 수주현황 품목상세(USP_SA_SOLIST_Q Q1): 명세서누계(확정+미확정)/출고누계/매출누계 표시.
-- 기존 프로시저의 최신 정의(출하 통제 249/250)를 기준으로 수정했다.

-- ============================================================
-- USP_SA_GI_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
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
    @p_invc_no VARCHAR(20) = NULL,
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
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_gi_id IS NULL OR gi_id = @p_gi_id)
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
                d.gi_id, d.serl, d.acc_id, d.gi_no, d.invc_id, iv.invc_no, d.invc_serl, d.so_id, so.so_no, d.so_serl, d.item_id, i.item_no, i.item_nm, d.unit_cd,
                d.qty, d.price, d.amt, d.remark,
                id.qty AS invc_qty, ISNULL(id.gi_qty, 0) AS invc_gi_qty,
                id.qty - ISNULL(id.gi_qty, 0) - ISNULL((SELECT SUM(g2.qty) FROM TSAGID g2 JOIN TSAGIM m2 ON m2.gi_id = g2.gi_id WHERE m2.stat_cd = '0' AND g2.invc_id = d.invc_id AND g2.invc_serl = d.invc_serl AND g2.gi_id <> d.gi_id), 0) AS invc_remain_qty
            FROM TSAGID d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                LEFT JOIN TSAINVCM iv ON iv.invc_id = d.invc_id
                LEFT JOIN TSAINVCD id ON id.invc_id = d.invc_id AND id.serl = d.invc_serl
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
                m.gi_id, m.gi_no, m.gi_date, c.cust_nm, t.invc_list AS invc_no, t.so_list AS so_no, t.kind_nm AS ship_kind_nm, m.ship_date, m.carrier, m.bl_no, m.stat_cd,
                t.line_cnt, t.total_qty, t.lot_list
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                OUTER APPLY (SELECT COUNT(*) AS line_cnt, SUM(d.qty) AS total_qty,
                                    (SELECT STRING_AGG(x.lot_no, ', ') WITHIN GROUP (ORDER BY x.serl, x.lot_serl) FROM TSAGIL x WHERE x.gi_id = m.gi_id AND ISNULL(x.lot_no, N'') <> N'') AS lot_list,
                                    (SELECT STRING_AGG(z.so_no, ', ') FROM (SELECT DISTINCT so2.so_no FROM TSAGID x2 JOIN TSASOM so2 ON so2.so_id = x2.so_id WHERE x2.gi_id = m.gi_id) z) AS so_list,
                                    (SELECT STRING_AGG(z.invc_no, ', ') FROM (SELECT DISTINCT iv2.invc_no FROM TSAGID x5 JOIN TSAINVCM iv2 ON iv2.invc_id = x5.invc_id WHERE x5.gi_id = m.gi_id) z) AS invc_list,
                                    (SELECT STRING_AGG(z.nm, ', ') FROM (SELECT DISTINCT CASE x4.ship_kind WHEN 'D' THEN N'외주처 직송' ELSE N'자사창고' END AS nm FROM TSAGIL x4 WHERE x4.gi_id = m.gi_id) z) AS kind_nm
                             FROM TSAGID d WHERE d.gi_id = m.gi_id) t
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_fr_date, '') = '' OR m.gi_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.gi_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_gi_no, '') = '' OR m.gi_no LIKE '%' + @p_gi_no + '%')
              AND (ISNULL(@p_so_no, '') = '' OR EXISTS (SELECT 1 FROM TSAGID x3 JOIN TSASOM so3 ON so3.so_id = x3.so_id WHERE x3.gi_id = m.gi_id AND so3.so_no LIKE '%' + @p_so_no + '%'))
              AND (ISNULL(@p_invc_no, '') = '' OR EXISTS (SELECT 1 FROM TSAGID x6 JOIN TSAINVCM iv3 ON iv3.invc_id = x6.invc_id WHERE x6.gi_id = m.gi_id AND iv3.invc_no LIKE '%' + @p_invc_no + '%'))
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

-- ============================================================
-- USP_SA_GI_S - 헤더 N/U/D (고객 변경 검사는 명세서 고객 기준)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_gi_date VARCHAR(8) = NULL,
    @p_cust_id BIGINT = NULL,
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
            IF ISNULL(@p_gi_date, '') = '' THROW 50001, N'출고일자를 입력하세요.', 1;
            IF @p_cust_id IS NULL THROW 50001, N'고객을 선택하세요.', 1;
            IF NOT EXISTS (SELECT 1 FROM TBACUST WHERE cust_id = @p_cust_id) THROW 50001, N'고객을 찾을 수 없습니다.', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSAGIM', 'gi_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TSAGIM (acc_id, gi_no, gi_date, cust_id, ship_date, carrier, bl_no, dest, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_gi_date, @p_cust_id, @p_ship_date, @p_carrier, @p_bl_no, @p_dest, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_gi_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
            IF @stat IS NULL THROW 50001, N'출고 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 출고는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;
            -- 라인(명세서 품목)이 이미 있으면 고객을 바꿀 수 없다(한 출고 = 한 고객)
            IF EXISTS (SELECT 1 FROM TSAGID d JOIN TSAINVCM iv ON iv.invc_id = d.invc_id WHERE d.gi_id = @p_gi_id AND iv.cust_id <> @p_cust_id)
                THROW 50001, N'이미 다른 고객의 명세서 품목이 담겨 있어 고객을 바꿀 수 없습니다. 라인을 지운 뒤 바꾸세요.', 1;

            UPDATE TSAGIM SET gi_date = @p_gi_date, cust_id = @p_cust_id, ship_date = @p_ship_date,
                carrier = @p_carrier, bl_no = @p_bl_no, dest = @p_dest, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            SELECT @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
            IF @stat IS NULL THROW 50001, N'출고 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 출고는 삭제할 수 없습니다. 먼저 확정취소하세요.', 1;
            DELETE FROM TSAGIL WHERE gi_id = @p_gi_id;
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

-- ============================================================
-- USP_SA_GI_S_1 - 출고 품목(명세서 라인 1건) N/U/D. N: 확정 명세서 라인을 가져온다(같은 고객). U: 비고만. D: LOT 상세도 같이 삭제.
--   p_serl 을 주면 그 번호로 만든다(화면이 품목 행과 LOT 행을 같이 저장하려고 번호를 미리 매긴다), 비면 최대값+1.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_invc_id BIGINT = NULL,
    @p_invc_serl INT = NULL,
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
        IF @stat IS NULL THROW 50001, N'출고 문서를 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 출고는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @item BIGINT, @unit VARCHAR(10), @price NUMERIC(18,4), @so_id BIGINT, @so_serl INT;
            SELECT @item = item_id, @unit = unit_cd, @price = price, @so_id = so_id, @so_serl = so_serl FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_invc_serl;
            IF @item IS NULL THROW 50001, N'명세서 라인을 찾을 수 없습니다.', 1;
            IF ISNULL((SELECT stat_cd FROM TSAINVCM WHERE invc_id = @p_invc_id), '') <> 'C' THROW 50001, N'확정된 거래명세서만 출고에 담을 수 있습니다.', 1;
            IF ISNULL((SELECT cust_id FROM TSAINVCM WHERE invc_id = @p_invc_id), -1) <> ISNULL(@cust, -2)
                THROW 50001, N'이 출고의 고객과 거래명세서의 고객이 다릅니다. 한 출고에는 같은 고객의 명세서만 담을 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND invc_id = @p_invc_id AND invc_serl = @p_invc_serl)
                THROW 50001, N'이미 이 출고에 담긴 명세서 품목입니다. LOT를 여러 개 출고하려면 그 품목 행에 LOT를 추가하세요.', 1;

            IF @p_serl IS NULL SET @p_serl = (SELECT ISNULL(MAX(serl), 0) + 1 FROM TSAGID WHERE gi_id = @p_gi_id);
            ELSE IF EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl) THROW 50001, N'출고 품목 번호가 중복입니다.', 1;

            INSERT INTO TSAGID (gi_id, serl, acc_id, gi_no, so_id, so_serl, invc_id, invc_serl, item_id, unit_cd, qty, price, amt, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_gi_id, @p_serl, @acc, @gi_no, @so_id, @so_serl, @p_invc_id, @p_invc_serl, @item, @unit, 0, @price, 0, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl) THROW 50001, N'출고 품목을 찾을 수 없습니다.', 1;
            UPDATE TSAGID SET remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE gi_id = @p_gi_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl;
            DELETE FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl;
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

-- ============================================================
-- USP_SA_GI_S_2 - 출고 LOT 행 N/U/D. 출고 통제: 가용재고(현재고 - 다른 미확정 출고가 잡은 같은 LOT/창고) / 명세서 잔량(명세서수량 - 확정 출고누계 - 다른 미확정 출고) 초과 거부.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,                     /* 출고 품목 번호 */
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
        DECLARE @acc BIGINT, @stat VARCHAR(10), @item BIGINT, @inv_id BIGINT, @inv_serl INT;
        SELECT @acc = m.acc_id, @stat = m.stat_cd, @item = d.item_id, @inv_id = d.invc_id, @inv_serl = d.invc_serl
        FROM TSAGIM m JOIN TSAGID d ON d.gi_id = m.gi_id AND d.serl = @p_serl WHERE m.gi_id = @p_gi_id;
        IF @stat IS NULL THROW 50001, N'출고 품목을 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 출고는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_qty, 0) <= 0 THROW 50001, N'출고수량은 0보다 커야 합니다.', 1;
            IF @p_wh_id IS NULL THROW 50001, N'출고 창고를 선택하세요. (출고 LOT 선택)', 1;
            IF ISNULL(@p_ship_kind, '') NOT IN ('W', 'D') THROW 50001, N'출고구분(자사창고/외주처)이 없습니다.', 1;
            DECLARE @whtype VARCHAR(10) = (SELECT wh_type FROM TBAWH WHERE wh_id = @p_wh_id);
            IF (@p_ship_kind = 'D' AND ISNULL(@whtype, '') <> 'OS') OR (@p_ship_kind = 'W' AND ISNULL(@whtype, '') = 'OS')
                THROW 50001, N'출고구분과 창고 종류가 맞지 않습니다. (외주처 직송은 외주처 창고, 자사창고 출고는 자사 창고)', 1;

            -- 같은 LOT/창고를 다른 미확정 출고(또는 이 출고의 다른 행)가 이미 잡은 수량을 뺀 가용재고보다 많이 출고할 수 없다(재고 차감은 확정 때지만 배정은 저장 때부터 잡는다).
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
                        + N' - 다른 미확정 출고가 잡은 수량 ' + CAST(CAST(@reserved AS FLOAT) AS NVARCHAR(30)) + N' = 가용 ' + CAST(CAST(@stock - @reserved AS FLOAT) AS NVARCHAR(30))
                        + N' < 요청 ' + CAST(CAST(@p_qty AS FLOAT) AS NVARCHAR(30));
                    THROW 50001, @m2, 1;
                END
            END

            -- 이 명세서 라인의 출고누계(확정) + 다른 미확정 출고 품목이 잡은 수량 + 이 품목의 LOT 합계가 명세서수량을 넘으면 거부
            DECLARE @iv_qty NUMERIC(18,4), @iv_gi NUMERIC(18,4);
            SELECT @iv_qty = qty, @iv_gi = ISNULL(gi_qty, 0) FROM TSAINVCD WHERE invc_id = @inv_id AND serl = @inv_serl;
            DECLARE @others NUMERIC(18,4) = (SELECT ISNULL(SUM(qty), 0) FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND (@p_work_type = 'N' OR lot_serl <> @p_lot_serl));
            DECLARE @iv_reserved NUMERIC(18,4) = ISNULL((
                SELECT SUM(d.qty) FROM TSAGID d JOIN TSAGIM m ON m.gi_id = d.gi_id
                WHERE m.stat_cd = '0' AND d.invc_id = @inv_id AND d.invc_serl = @inv_serl AND NOT (d.gi_id = @p_gi_id AND d.serl = @p_serl)), 0);
            IF @iv_gi + @iv_reserved + @others + @p_qty > @iv_qty
            BEGIN
                DECLARE @m3 NVARCHAR(400) = N'명세서 출고 잔량을 넘습니다. 명세서수량 ' + CAST(CAST(@iv_qty AS FLOAT) AS NVARCHAR(30)) + N' - 출고확정 ' + CAST(CAST(@iv_gi AS FLOAT) AS NVARCHAR(30))
                    + N' - 다른 미확정 출고 ' + CAST(CAST(@iv_reserved AS FLOAT) AS NVARCHAR(30)) + N' = 잔량 ' + CAST(CAST(@iv_qty - @iv_gi - @iv_reserved AS FLOAT) AS NVARCHAR(30))
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
            IF NOT EXISTS (SELECT 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl) THROW 50001, N'출고 LOT 행을 찾을 수 없습니다.', 1;
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

-- ============================================================
-- USP_SA_GI_C_S - C 확정(LOT 행별 재고 차감 + 명세서 출고누계) / CC 확정취소(재고 복원)
-- ============================================================
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
        IF @stat IS NULL THROW 50001, N'출고 문서를 찾을 수 없습니다.', 1;

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 출고만 확정할 수 있습니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id) THROW 50001, N'출고할 명세서 품목을 추가한 뒤 확정하세요.', 1;

            DECLARE @miss INT = (SELECT TOP 1 d.serl FROM TSAGID d WHERE d.gi_id = @p_gi_id AND NOT EXISTS (SELECT 1 FROM TSAGIL l WHERE l.gi_id = d.gi_id AND l.serl = d.serl) ORDER BY d.serl);
            IF @miss IS NOT NULL
            BEGIN
                DECLARE @m1 NVARCHAR(200) = N'출고 품목 ' + CAST(@miss AS NVARCHAR(10)) + N'번에 출고 LOT가 지정되지 않았습니다.';
                THROW 50001, @m1, 1;
            END
            IF EXISTS (SELECT 1 FROM TSAGID d JOIN TSAINVCM iv ON iv.invc_id = d.invc_id WHERE d.gi_id = @p_gi_id AND iv.stat_cd <> 'C')
                THROW 50001, N'확정되지 않은 거래명세서가 포함되어 있습니다.', 1;

            BEGIN TRAN;

            DECLARE @serl INT, @lot_serl INT, @sserl INT, @item BIGINT, @wh BIGINT, @lot NVARCHAR(50), @qty NUMERIC(18,4), @rmk NVARCHAR(3000), @tid BIGINT;
            DECLARE c CURSOR LOCAL FAST_FORWARD FOR
                SELECT serl, lot_serl, item_id, wh_id, lot_no, qty, remark FROM TSAGIL WHERE gi_id = @p_gi_id ORDER BY serl, lot_serl;
            OPEN c;
            FETCH NEXT FROM c INTO @serl, @lot_serl, @item, @wh, @lot, @qty, @rmk;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                -- 재고가 모자라면 여기서 현재고 부족으로 거부된다(마이너스 재고 불허). 수불 원천 순번 = 품목번호*1000 + LOT번호
                SET @sserl = @serl * 1000 + @lot_serl; -- EXEC 인자에는 식을 쓸 수 없어서 미리 계산
                EXEC USP_PR_TRANS_POST @acc, 'O', 'SA_OUT', @item, @wh, @lot, @qty, 'GI', @p_gi_id, @no, @sserl, NULL, @rmk, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
                FETCH NEXT FROM c INTO @serl, @lot_serl, @item, @wh, @lot, @qty, @rmk;
            END
            CLOSE c; DEALLOCATE c;

            UPDATE TSAGIM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정된 출고만 확정취소할 수 있습니다.', 1;

            BEGIN TRAN;
            EXEC USP_PR_TRANS_REVERSE 'GI', @p_gi_id, 0, 2000000000, N'출고 확정취소', @p_user_id, @p_client_pc;

            UPDATE TSAGIM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE gi_id = @p_gi_id;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        -- 명세서 라인의 출고누계 재계산(확정된 출고 품목 합계) - 명세서수량을 넘으면 거부
        UPDATE d SET gi_qty = (SELECT ISNULL(SUM(g.qty), 0) FROM TSAGID g JOIN TSAGIM m ON m.gi_id = g.gi_id AND m.stat_cd = 'C' WHERE g.invc_id = d.invc_id AND g.invc_serl = d.serl),
                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        FROM TSAINVCD d WHERE EXISTS (SELECT 1 FROM TSAGID x WHERE x.gi_id = @p_gi_id AND x.invc_id = d.invc_id AND x.invc_serl = d.serl);

        IF EXISTS (SELECT 1 FROM TSAINVCD d WHERE EXISTS (SELECT 1 FROM TSAGID x WHERE x.gi_id = @p_gi_id AND x.invc_id = d.invc_id AND x.invc_serl = d.serl) AND d.gi_qty > d.qty)
            THROW 50001, N'명세서수량을 초과하여 출고할 수 없습니다. (이미 확정된 다른 출고를 포함한 합계가 명세서수량보다 큽니다)', 1;

        COMMIT TRAN;
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

-- ============================================================
-- USP_SA_GIINVCPICK_Q - 출고 "명세서 품목 불러오기" 팝업(popPick 규약). 확정 명세서, 출고 잔량(확정 출고 + 다른 미확정 출고 차감) > 0
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GIINVCPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 명세서번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_gi_id BIGINT = NULL,                 /* 지금 편집 중인 출고(이 출고가 잡은 수량은 '다른 미확정 출고'에서 제외) */
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
                m.invc_id, d.serl AS invc_serl, m.invc_no, m.invc_date, d.so_id, d.so_serl, d.so_no, m.cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, d.unit_cd, d.qty, d.gi_qty, ISNULL(rs.reserved_qty, 0) AS reserved_qty,
                d.qty - d.gi_qty - ISNULL(rs.reserved_qty, 0) AS remain_qty,
                d.price, ISNULL(sd.delv_date, so.delv_date) AS delv_date
            FROM TSAINVCM m
                JOIN TSAINVCD d ON d.invc_id = m.invc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
                OUTER APPLY (SELECT SUM(g.qty) AS reserved_qty FROM TSAGID g JOIN TSAGIM gm ON gm.gi_id = g.gi_id
                             WHERE gm.stat_cd = '0' AND g.invc_id = d.invc_id AND g.invc_serl = d.serl AND (@p_gi_id IS NULL OR g.gi_id <> @p_gi_id)) rs
            WHERE m.stat_cd = 'C'
              AND d.qty - d.gi_qty - ISNULL(rs.reserved_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.invc_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.invc_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.invc_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR m.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = '' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.invc_date DESC, m.invc_no, d.serl;
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

IF OBJECT_ID('USP_SA_GISOPICK_Q') IS NOT NULL DROP PROCEDURE USP_SA_GISOPICK_Q;
GO

-- ============================================================
-- USP_SA_SOLIST_Q - 품목상세(Q1): 명세서누계(확정+미확정=next_qty, 미확정=reg_qty) / 출고누계 / 매출누계
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SOLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_so_no VARCHAR(20) = NULL,
    @p_so_title NVARCHAR(1000) = NULL,
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
                m.so_id, m.so_no, m.so_date, m.so_title,
                m.stat_cd,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TSASOM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND 1 = 1
              AND (@p_so_no IS NULL OR m.so_no LIKE '%' + @p_so_no + '%')
              AND (@p_so_title IS NULL OR m.so_title LIKE '%' + @p_so_title + '%')
            ORDER BY m.so_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.so_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, (ISNULL(dt.next_qty, 0) + ISNULL(rg.reg_qty, 0)) AS next_qty, ISNULL(rg.reg_qty, 0) AS reg_qty,
                (dt.qty - ISNULL(dt.next_qty, 0) - ISNULL(rg.reg_qty, 0)) AS remain_qty, dt.unit_cd,
                ISNULL(fl.gi_qty, 0) AS gi_qty, ISNULL(fl.bill_qty, 0) AS bill_qty,
                dt.price, dt.total_amt,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TSASOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                OUTER APPLY (SELECT SUM(x.qty) AS reg_qty FROM TSAINVCD x JOIN TSAINVCM xm ON xm.invc_id = x.invc_id
                             WHERE xm.stat_cd = '0' AND x.so_id = dt.so_id AND x.so_serl = dt.serl) rg
                OUTER APPLY (SELECT SUM(x.gi_qty) AS gi_qty, SUM(x.bill_qty) AS bill_qty FROM TSAINVCD x
                             WHERE x.so_id = dt.so_id AND x.so_serl = dt.serl) fl
            WHERE dt.so_id = @p_so_id
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
