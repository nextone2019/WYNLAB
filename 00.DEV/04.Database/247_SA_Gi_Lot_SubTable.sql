-- 출하를 3단계 구조로 변경 (2026-10-01): TSAGIM 출하(고객) > TSAGID 출하 품목(= 수주 라인 1건당 1행, 출하수량 = LOT 합계) > TSAGIL 출하 LOT 상세(LOT, 출하 창고, 수량).
--  예) 수주 20,000 / LOT 4개 -> 출하 품목 1행(수량 19,500) + LOT 상세 4행. 수주별 집계/진행관리는 품목 행 기준, 재고 차감과 LOT 계보는 LOT 행 기준.
--  기존(LOT가 TSAGID에 붙어 있던) 데이터는 같은 (출하, 수주 라인)끼리 묶어 품목 1행 + LOT 행들로 옮긴다.

IF OBJECT_ID('TSAGIL') IS NULL
BEGIN
    CREATE TABLE TSAGIL (
        gi_id          BIGINT         NOT NULL,
        serl           INT            NOT NULL,             -- 출하 품목(TSAGID.serl)
        lot_serl       INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        item_id        BIGINT         NOT NULL,
        lot_id         BIGINT         NULL,                 -- 생산 LOT(TPRLOT). LOT 재고를 안 쓰는 품목은 NULL
        lot_no         NVARCHAR(50)   NULL,
        wh_id          BIGINT         NOT NULL,             -- 출하 창고(자사/외주처)
        qty            NUMERIC(18,4)  NOT NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAGIL PRIMARY KEY CLUSTERED (gi_id, serl, lot_serl),
        CONSTRAINT CK_TSAGIL_qty CHECK (qty > 0)
    );
    CREATE NONCLUSTERED INDEX IX_TSAGIL_lot ON TSAGIL (lot_id);
END
GO

-- 기존 TSAGID(LOT가 붙은 구조) -> 새 구조로 이전
IF COL_LENGTH('TSAGID', 'lot_no') IS NOT NULL
BEGIN
    SELECT * INTO #old FROM TSAGID;

    DROP TABLE TSAGID;
    CREATE TABLE TSAGID (
        gi_id          BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        gi_no          VARCHAR(20)    NULL,
        so_id          BIGINT         NULL,
        so_serl        INT            NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        qty            NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TSAGID_qty DEFAULT 0,   -- 출하수량 = LOT 상세 합계(저장 시 서버가 맞춘다)
        price          NUMERIC(18,4)  NULL,
        amt            NUMERIC(18,4)  NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAGID PRIMARY KEY CLUSTERED (gi_id, serl),
        CONSTRAINT CK_TSAGID_qty CHECK (qty >= 0)
    );
    CREATE NONCLUSTERED INDEX IX_TSAGID_so ON TSAGID (so_id, so_serl);

    -- 같은 (출하, 수주 라인)끼리 한 품목 행으로
    ;WITH g AS (
        SELECT gi_id, so_id, so_serl, MIN(serl) AS first_serl FROM #old GROUP BY gi_id, so_id, so_serl
    ), n AS (
        SELECT gi_id, so_id, so_serl, ROW_NUMBER() OVER (PARTITION BY gi_id ORDER BY first_serl) AS new_serl FROM g
    )
    INSERT INTO TSAGID (gi_id, serl, acc_id, gi_no, so_id, so_serl, item_id, unit_cd, qty, price, amt, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT o.gi_id, n.new_serl, MIN(o.acc_id), MIN(o.gi_no), o.so_id, o.so_serl, MIN(o.item_id), MIN(o.unit_cd), SUM(o.qty), MIN(o.price),
           ROUND(SUM(o.qty) * ISNULL(MIN(o.price), 0), 4), NULL, MIN(o.reg_user_id), MIN(o.reg_dt), MIN(o.reg_pc), MIN(o.upt_user_id), MIN(o.upt_dt), MIN(o.upt_pc)
    FROM #old o JOIN n ON n.gi_id = o.gi_id AND n.so_id = o.so_id AND n.so_serl = o.so_serl
    GROUP BY o.gi_id, n.new_serl, o.so_id, o.so_serl;

    ;WITH g AS (
        SELECT gi_id, so_id, so_serl, MIN(serl) AS first_serl FROM #old GROUP BY gi_id, so_id, so_serl
    ), n AS (
        SELECT gi_id, so_id, so_serl, ROW_NUMBER() OVER (PARTITION BY gi_id ORDER BY first_serl) AS new_serl FROM g
    )
    INSERT INTO TSAGIL (gi_id, serl, lot_serl, acc_id, item_id, lot_id, lot_no, wh_id, qty, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT o.gi_id, n.new_serl, ROW_NUMBER() OVER (PARTITION BY o.gi_id, n.new_serl ORDER BY o.serl), o.acc_id, o.item_id, o.lot_id, o.lot_no, o.wh_id, o.qty, o.remark,
           o.reg_user_id, o.reg_dt, o.reg_pc, o.upt_user_id, o.upt_dt, o.upt_pc
    FROM #old o JOIN n ON n.gi_id = o.gi_id AND n.so_id = o.so_id AND n.so_serl = o.so_serl;

    DROP TABLE #old;
END
GO

-- ============================================================
-- USP_SA_GI_Q - Q: 출하 1건(헤더, 품목, LOT 상세 3개 결과셋) / L: 목록
-- ============================================================
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
                m.gi_id, m.acc_id, m.gi_no, m.gi_date, m.cust_id, c.cust_nm, m.ship_kind, m.ship_date,
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
                sd.qty AS so_qty, ISNULL(sd.next_qty, 0) AS so_next_qty, sd.qty - ISNULL(sd.next_qty, 0) AS so_remain_qty
            FROM TSAGID d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
            WHERE d.gi_id = @match_id
            ORDER BY d.serl;

            SELECT
                l.gi_id, l.serl, l.lot_serl, l.acc_id, l.item_id, l.lot_id, l.lot_no, l.wh_id, w.wh_nm, l.qty, l.remark,
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
                m.gi_id, m.gi_no, m.gi_date, c.cust_nm, t.so_list AS so_no, m.ship_kind, m.ship_date, m.carrier, m.bl_no, m.stat_cd,
                t.line_cnt, t.total_qty, t.lot_list
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                OUTER APPLY (SELECT COUNT(*) AS line_cnt, SUM(d.qty) AS total_qty,
                                    (SELECT STRING_AGG(x.lot_no, ', ') WITHIN GROUP (ORDER BY x.serl, x.lot_serl) FROM TSAGIL x WHERE x.gi_id = m.gi_id AND ISNULL(x.lot_no, N'') <> N'') AS lot_list,
                                    (SELECT STRING_AGG(z.so_no, ', ') FROM (SELECT DISTINCT so2.so_no FROM TSAGID x2 JOIN TSASOM so2 ON so2.so_id = x2.so_id WHERE x2.gi_id = m.gi_id) z) AS so_list
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

-- ============================================================
-- USP_SA_GI_S_1 - 출하 품목(수주 라인 1건) N/U/D. N: 수주 라인을 가져온다(같은 고객의 수주만). U: 비고만. D: LOT 상세도 같이 삭제.
--   p_serl 을 주면 그 번호로 만든다(화면이 품목 행과 LOT 행을 같이 저장하려고 번호를 미리 매긴다), 비면 최대값+1.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_so_id BIGINT = NULL,
    @p_so_serl INT = NULL,
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

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @item BIGINT, @unit VARCHAR(10), @price NUMERIC(18,4);
            SELECT @item = item_id, @unit = unit_cd, @price = price FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_so_serl;
            IF @item IS NULL THROW 50001, N'수주 라인을 찾을 수 없습니다.', 1;
            IF ISNULL((SELECT cust_id FROM TSASOM WHERE so_id = @p_so_id), -1) <> ISNULL(@cust, -2)
                THROW 50001, N'이 출하의 고객과 수주의 고객이 다릅니다. 한 출하에는 같은 고객의 수주만 담을 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND so_id = @p_so_id AND so_serl = @p_so_serl)
                THROW 50001, N'이미 이 출하에 담긴 수주 품목입니다. LOT를 여러 개 출하하려면 그 품목 행에 LOT를 추가하세요.', 1;

            IF @p_serl IS NULL SET @p_serl = (SELECT ISNULL(MAX(serl), 0) + 1 FROM TSAGID WHERE gi_id = @p_gi_id);
            ELSE IF EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl) THROW 50001, N'출하 품목 번호가 중복입니다.', 1;

            INSERT INTO TSAGID (gi_id, serl, acc_id, gi_no, so_id, so_serl, item_id, unit_cd, qty, price, amt, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_gi_id, @p_serl, @acc, @gi_no, @p_so_id, @p_so_serl, @item, @unit, 0, @price, 0, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND serl = @p_serl) THROW 50001, N'출하 품목을 찾을 수 없습니다.', 1;
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
-- USP_SA_GI_S_2 - 출하 LOT 상세 N/U/D. 저장할 때마다 품목 행의 출하수량/금액을 LOT 합계로 맞춘다. 합계가 수주수량을 넘으면 거부.
--   p_lot_serl 을 주면 그 번호로 만든다(S_1과 같은 이유), 비면 최대값+1.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,                     /* 출하 품목 번호 */
    @p_lot_serl INT = NULL,
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
        DECLARE @acc BIGINT, @stat VARCHAR(10), @item BIGINT, @so_id BIGINT, @so_serl INT, @price NUMERIC(18,4);
        SELECT @acc = m.acc_id, @stat = m.stat_cd, @item = d.item_id, @so_id = d.so_id, @so_serl = d.so_serl, @price = d.price
        FROM TSAGIM m JOIN TSAGID d ON d.gi_id = m.gi_id AND d.serl = @p_serl WHERE m.gi_id = @p_gi_id;
        IF @stat IS NULL THROW 50001, N'출하 품목을 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 출하는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_qty, 0) <= 0 THROW 50001, N'출하수량은 0보다 커야 합니다.', 1;
            IF @p_wh_id IS NULL THROW 50001, N'출하 창고를 선택하세요. (출하 LOT 선택)', 1;

            -- 이 출하 품목의 LOT 합계(이 행 제외 + 새 수량)가 수주수량을 넘으면 거부
            DECLARE @so_qty NUMERIC(18,4) = (SELECT qty FROM TSASOD WHERE so_id = @so_id AND serl = @so_serl);
            DECLARE @others NUMERIC(18,4) = (SELECT ISNULL(SUM(qty), 0) FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND (@p_work_type = 'N' OR lot_serl <> @p_lot_serl));
            IF @others + @p_qty > @so_qty THROW 50001, N'출하수량의 합계가 수주수량을 넘을 수 없습니다.', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_lot_serl IS NULL SET @p_lot_serl = (SELECT ISNULL(MAX(lot_serl), 0) + 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl);
            ELSE IF EXISTS (SELECT 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl) THROW 50001, N'LOT 번호가 중복입니다.', 1;

            INSERT INTO TSAGIL (gi_id, serl, lot_serl, acc_id, item_id, lot_id, lot_no, wh_id, qty, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_gi_id, @p_serl, @p_lot_serl, @acc, @item, @p_lot_id, @p_lot_no, @p_wh_id, @p_qty, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND lot_serl = @p_lot_serl) THROW 50001, N'출하 LOT 행을 찾을 수 없습니다.', 1;
            UPDATE TSAGIL SET lot_id = @p_lot_id, lot_no = @p_lot_no, wh_id = @p_wh_id, qty = @p_qty, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
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
-- USP_SA_GI_C_S - C 확정(LOT 행별 재고 차감 + 수주 출하누계) / CC 확정취소(재고 복원)
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
        IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 출하만 확정할 수 있습니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id) THROW 50001, N'출하할 수주 품목을 추가한 뒤 확정하세요.', 1;

            DECLARE @miss INT = (SELECT TOP 1 d.serl FROM TSAGID d WHERE d.gi_id = @p_gi_id AND NOT EXISTS (SELECT 1 FROM TSAGIL l WHERE l.gi_id = d.gi_id AND l.serl = d.serl) ORDER BY d.serl);
            IF @miss IS NOT NULL
            BEGIN
                DECLARE @m1 NVARCHAR(200) = N'출하 품목 ' + CAST(@miss AS NVARCHAR(10)) + N'번에 출하 LOT가 지정되지 않았습니다.';
                THROW 50001, @m1, 1;
            END

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

            -- 수주 라인의 출하누계 재계산(확정된 출하 품목 합계) - 수주수량을 넘으면 거부
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
            EXEC USP_PR_TRANS_REVERSE 'GI', @p_gi_id, 0, 2000000000, N'출하 확정취소', @p_user_id, @p_client_pc;

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

-- LOT계보조회 이력의 출하 이벤트를 LOT 상세(TSAGIL) 기준으로 (244의 프로시저에서 출하 블록만 교체)
CREATE OR ALTER PROCEDURE USP_PR_LOTTRACE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lot_no NVARCHAR(50) = NULL,          /* L 전용: LOT번호 일부 */
    @p_wo_no VARCHAR(20) = NULL,            /* L 전용: 작업지시번호 일부 */
    @p_lot_id BIGINT = NULL,                /* H 전용 */
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
            IF ISNULL(@p_lot_no, '') = '' AND ISNULL(@p_wo_no, '') = ''
            BEGIN
                -- 조건이 없으면 전체 계보를 끌어오지 않고 빈 결과를 돌려준다(LOT번호나 작업지시번호를 넣어야 함).
                SELECT CAST(NULL AS BIGINT) AS lot_id WHERE 1 = 0;
                RETURN;
            END

            ;WITH hit AS (
                SELECT l.lot_id FROM TPRLOT l LEFT JOIN TPRWOM w ON w.wo_id = l.wo_id
                WHERE (ISNULL(@p_lot_no, '') = '' OR l.lot_no LIKE '%' + @p_lot_no + '%')
                  AND (ISNULL(@p_wo_no, '') = '' OR w.wo_no LIKE '%' + @p_wo_no + '%')
            ),
            up AS (
                SELECT lot_id FROM hit
                UNION ALL
                SELECT r.parent_lot_id FROM up JOIN TPRLOTREL r ON r.child_lot_id = up.lot_id
            ),
            roots AS (
                SELECT DISTINCT u.lot_id FROM up u
                WHERE NOT EXISTS (SELECT 1 FROM TPRLOTREL r WHERE r.child_lot_id = u.lot_id)
            ),
            tree AS (
                SELECT l.lot_id, 0 AS depth, CAST(NULL AS BIGINT) AS rslt_id,
                       CAST(RIGHT('0000000000' + CAST(l.lot_id AS VARCHAR(10)), 10) AS VARCHAR(900)) AS path
                FROM TPRLOT l JOIN roots ro ON ro.lot_id = l.lot_id
                UNION ALL
                SELECT c.lot_id, t.depth + 1, r.rslt_id,
                       CAST(t.path + '.' + RIGHT('0000000000' + CAST(c.lot_id AS VARCHAR(10)), 10) AS VARCHAR(900))
                FROM tree t
                    JOIN TPRLOTREL r ON r.parent_lot_id = t.lot_id
                    JOIN TPRLOT c ON c.lot_id = r.child_lot_id
            )
            SELECT
                t.lot_id, t.depth,
                REPLICATE(N'    ', t.depth) + CASE WHEN t.depth > 0 THEN N'└ ' ELSE N'' END + l.lot_no AS lot_disp,
                l.lot_no, i.item_no, i.item_nm, l.unit_cd, l.init_qty,
                w.wo_no,
                CASE WHEN l.wo_serl IS NULL OR l.wo_serl = 0 THEN N'입고(시작 LOT)' ELSE p.proc_nm END AS gen_proc_nm,
                rs.rslt_no,
                st.wh_nm, st.stock_qty,
                CASE WHEN EXISTS (SELECT 1 FROM hit h WHERE h.lot_id = t.lot_id) THEN 1 ELSE 0 END AS hit
            FROM tree t
                JOIN TPRLOT l ON l.lot_id = t.lot_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TPRWOM w ON w.wo_id = l.wo_id
                LEFT JOIN TPRWOD d ON d.wo_id = l.wo_id AND d.serl = l.wo_serl
                LEFT JOIN TBAPROC p ON p.acc_id = l.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TPRRSLTM rs ON rs.rslt_id = t.rslt_id
                OUTER APPLY (SELECT TOP 1 wh.wh_nm, s.stock_qty
                             FROM TMASTOCK s JOIN TBAWH wh ON wh.wh_id = s.wh_id
                             WHERE s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.stock_qty > 0
                             ORDER BY s.stock_qty DESC) st
            ORDER BY t.path
            OPTION (MAXRECURSION 200);
        END
        ELSE IF @p_work_type = 'H'
        BEGIN
            SELECT evt_kind, doc_type, doc_id, doc_no, evt_date, descr, qty, stat_nm FROM (
                -- 웨이퍼 입고(이 LOT 번호로 들어온 입고 문서)
                SELECT N'입고' AS evt_kind, 'RV' AS doc_type, v.rcv_id AS doc_id, v.rcv_no AS doc_no, v.rcv_date AS evt_date,
                       N'웨이퍼 입고 → ' + ISNULL(wh.wh_nm, N'') AS descr, v.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = v.stat_cd) AS stat_nm
                FROM TPRLOT l JOIN TPRRCV v ON v.acc_id = l.acc_id AND v.item_id = l.item_id AND v.lot_no = l.lot_no
                    LEFT JOIN TBAWH wh ON wh.wh_id = v.wh_id
                WHERE l.lot_id = @p_lot_id
                UNION ALL
                -- 공정 산출: 이 LOT를 만든 실적
                SELECT N'공정 산출', 'RS', m.rslt_id, m.rslt_no, m.rslt_date,
                       ISNULL(p.proc_nm, m.proc_cd) + N' 산출 (' + ISNULL(c.cust_nm, N'') + N', 투입 LOT ' + ISNULL(pl.lot_no, N'') + N')', r.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = m.stat_cd)
                FROM TPRLOTREL r JOIN TPRRSLTM m ON m.rslt_id = r.rslt_id
                    LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                    LEFT JOIN TPRLOT pl ON pl.lot_id = r.parent_lot_id
                WHERE r.child_lot_id = @p_lot_id
                UNION ALL
                -- 공정 투입: 이 LOT를 투입한 실적
                SELECT N'공정 투입', 'RS', m.rslt_id, m.rslt_no, m.rslt_date,
                       ISNULL(p.proc_nm, m.proc_cd) + N' 투입 (' + ISNULL(c.cust_nm, N'') + N', 양품 ' + CONVERT(NVARCHAR(30), CAST(m.good_qty AS DECIMAL(18,4)), 1) + N')', m.in_qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = m.stat_cd)
                FROM TPRRSLTM m
                    LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                WHERE m.in_lot_id = @p_lot_id
                UNION ALL
                -- 외주이전
                SELECT N'외주이전', 'XF', x.xfer_id, x.xfer_no, x.xfer_date,
                       ISNULL(fc.cust_nm, N'') + N' → ' + ISNULL(tc.cust_nm, N'') + N' (도착 ' + CONVERT(NVARCHAR(30), CAST(ISNULL(d.in_qty, 0) AS DECIMAL(18,4)), 1) + N')', d.out_qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0003' AND minor_cd = x.stat_cd)
                FROM TPRXFERD d JOIN TPRXFERM x ON x.xfer_id = d.xfer_id
                    LEFT JOIN TBACUST fc ON fc.cust_id = x.from_cust_id
                    LEFT JOIN TBACUST tc ON tc.cust_id = x.to_cust_id
                WHERE d.lot_id = @p_lot_id
                UNION ALL
                -- 출하(영업 출하등록 TSAGIM/D/L) - 이 LOT가 고객에게 나간 이력(LOT 상세 행 기준)
                SELECT N'출하', 'GI', g.gi_id, g.gi_no, g.gi_date,
                       N'출하 → ' + ISNULL(c.cust_nm, N'') + N' (' + ISNULL(w.wh_nm, N'') + N', 수주 ' + ISNULL(so.so_no, N'') + N')', l.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = g.stat_cd)
                FROM TSAGIL l JOIN TSAGIM g ON g.gi_id = l.gi_id
                    JOIN TSAGID d ON d.gi_id = l.gi_id AND d.serl = l.serl
                    LEFT JOIN TSASOM so ON so.so_id = d.so_id
                    LEFT JOIN TBACUST c ON c.cust_id = g.cust_id
                    LEFT JOIN TBAWH w ON w.wh_id = l.wh_id
                WHERE l.lot_id = @p_lot_id
            ) e
            ORDER BY evt_date, doc_no;
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
