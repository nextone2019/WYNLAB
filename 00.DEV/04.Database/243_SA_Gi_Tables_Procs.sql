-- 영업(SA) 출하(Goods Issue) - 테이블/프로시저/메뉴 (2026-10-01)
--  TSAGIM 출하 헤더 / TSAGID 출하 라인(LOT 단위). 수주(TSASOM/D) 기준, 확정 시 재고 차감(수불 SA_OUT, src_type GI) + 수주 라인 next_qty(출하누계) 갱신.
--  출하 창고는 자사 창고/외주처 창고 모두 가능(외주처에서 고객으로 직송 = ship_kind 'D'). 출하 전 검사/결재 없음.
--  USP_SA_GI_Q(Q/L) / USP_SA_GI_S(N/U/D) / USP_SA_GI_S_1(N/U/D 라인) / USP_SA_GI_C_S(C/CC) / USP_SA_GISOPICK_Q / USP_SA_GILOTPICK_Q
--  재고 차감/복원은 생산모듈에서 검증한 USP_PR_TRANS_POST / USP_PR_TRANS_REVERSE 를 공용으로 쓴다.

IF OBJECT_ID('TSAGIM') IS NULL
BEGIN
    CREATE TABLE TSAGIM (
        gi_id          BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        gi_no          VARCHAR(20)    NOT NULL,
        gi_date        VARCHAR(8)     NULL,
        cust_id        BIGINT         NULL,
        so_id          BIGINT         NULL,                 -- 기준 수주(한 출하 문서 = 수주 1건)
        so_no          VARCHAR(20)    NULL,
        ship_kind      VARCHAR(1)     NULL,                 -- W 자사창고 출하 / D 외주처 직송
        ship_date      VARCHAR(8)     NULL,                 -- 선적일(매출/정산 인식 기준일)
        carrier        NVARCHAR(100)  NULL,                 -- 운송사
        bl_no          NVARCHAR(100)  NULL,                 -- B/L, 송장 번호
        dest           NVARCHAR(200)  NULL,                 -- 도착지
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        stat_cd        VARCHAR(10)    NULL,                 -- 0 작성, C 확정 (PR0006과 같은 의미)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(50)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAGIM PRIMARY KEY CLUSTERED (gi_id)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TSAGIM_no ON TSAGIM (acc_id, gi_no);
    CREATE NONCLUSTERED INDEX IX_TSAGIM_so ON TSAGIM (so_id);
END
GO

IF OBJECT_ID('TSAGID') IS NULL
BEGIN
    CREATE TABLE TSAGID (
        gi_id          BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        gi_no          VARCHAR(20)    NULL,
        so_id          BIGINT         NULL,
        so_serl        INT            NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        lot_id         BIGINT         NULL,                 -- 생산 LOT(TPRLOT). LOT 재고를 안 쓰는 품목은 NULL
        lot_no         NVARCHAR(50)   NULL,
        wh_id          BIGINT         NOT NULL,             -- 출하 창고(자사/외주처)
        qty            NUMERIC(18,4)  NOT NULL,
        price          NUMERIC(18,4)  NULL,                 -- 수주 단가 스냅샷(매출 단계에서 사용)
        amt            NUMERIC(18,4)  NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAGID PRIMARY KEY CLUSTERED (gi_id, serl),
        CONSTRAINT CK_TSAGID_qty CHECK (qty > 0)
    );
    CREATE NONCLUSTERED INDEX IX_TSAGID_so ON TSAGID (so_id, so_serl);
    CREATE NONCLUSTERED INDEX IX_TSAGID_lot ON TSAGID (lot_id);
END
GO

-- 채번 GI, 수불 원천구분 GI(MA0004)
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
SELECT 'TSAGIM', N'출하', 'GI', 'gi_no', 'YYMM', 4, 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TSAGIM');

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'MA0004', 'GI', N'출하', 9, 'Y', 'Y', 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'GI');

-- 출하구분 SA0001 (W 자사창고 출하 / D 외주처 직송) + 콤보 L_SA0001
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'SA0001')
    INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
    VALUES ('SA0001', N'출하구분', 'Y', 'SYSTEM', GETDATE());
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT v.mj, v.cd, v.nm, v.srt, 'Y', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('SA0001', 'W', N'자사창고 출하', 1), ('SA0001', 'D', N'외주처 직송', 2)) v(mj, cd, nm, srt)
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR n WHERE n.major_cd = v.mj AND n.minor_cd = v.cd);

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_SA0001', NULL, N'출하구분', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''SA0001''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_SA0001');
GO

-- ============================================================
-- USP_SA_GI_Q - Q: 출하 1건(헤더 + 라인) / L: 목록
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
                d.lot_id, d.lot_no, d.wh_id, w.wh_nm, d.qty, d.price, d.amt, d.remark,
                sd.qty AS so_qty, ISNULL(sd.next_qty, 0) AS so_next_qty, sd.qty - ISNULL(sd.next_qty, 0) AS so_remain_qty,
                ISNULL(st.stock_qty, 0) AS stock_qty
            FROM TSAGID d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
                LEFT JOIN TMASTOCK st ON st.acc_id = d.acc_id AND st.item_id = d.item_id AND st.wh_id = d.wh_id AND st.loc_id = 0 AND st.lot_no = ISNULL(d.lot_no, N'')
            WHERE d.gi_id = @match_id
            ORDER BY d.serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.gi_id, m.gi_no, m.gi_date, c.cust_nm, m.so_id, m.so_no, m.ship_kind, m.ship_date, m.carrier, m.bl_no, m.stat_cd,
                t.line_cnt, t.total_qty, t.lot_list
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                OUTER APPLY (SELECT COUNT(*) AS line_cnt, SUM(d.qty) AS total_qty,
                                    (SELECT STRING_AGG(x.lot_no, ', ') WITHIN GROUP (ORDER BY x.serl) FROM TSAGID x WHERE x.gi_id = m.gi_id AND ISNULL(x.lot_no, N'') <> N'') AS lot_list
                             FROM TSAGID d WHERE d.gi_id = m.gi_id) t
            WHERE (ISNULL(@p_fr_date, '') = '' OR m.gi_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.gi_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_gi_no, '') = '' OR m.gi_no LIKE '%' + @p_gi_no + '%')
              AND (ISNULL(@p_so_no, '') = '' OR m.so_no LIKE '%' + @p_so_no + '%')
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

-- ============================================================
-- USP_SA_GI_S - 헤더 N/U/D. 작성 상태에서만 수정/삭제.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_gi_date VARCHAR(8) = NULL,
    @p_so_id BIGINT = NULL,
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
        DECLARE @stat VARCHAR(10), @so_no VARCHAR(20), @cust BIGINT;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_gi_date, '') = '' THROW 50001, N'출하일자를 입력하세요.', 1;
            IF @p_so_id IS NULL THROW 50001, N'수주를 선택하세요. (수주 불러오기)', 1;
            SELECT @so_no = so_no, @cust = cust_id FROM TSASOM WHERE so_id = @p_so_id;
            IF @so_no IS NULL THROW 50001, N'수주를 찾을 수 없습니다.', 1;
            IF ISNULL(@p_ship_kind, '') NOT IN ('W', 'D') THROW 50001, N'출하구분을 선택하세요.', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSAGIM', 'gi_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TSAGIM (acc_id, gi_no, gi_date, cust_id, so_id, so_no, ship_kind, ship_date, carrier, bl_no, dest, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_gi_date, @cust, @p_so_id, @so_no, @p_ship_kind, @p_ship_date, @p_carrier, @p_bl_no, @p_dest, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_gi_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
            IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 출하는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;
            -- 라인이 이미 있으면 수주를 바꿀 수 없다(라인은 수주 라인 기준)
            IF EXISTS (SELECT 1 FROM TSAGID WHERE gi_id = @p_gi_id AND so_id <> @p_so_id)
                THROW 50001, N'이미 다른 수주의 품목이 담겨 있어 수주를 바꿀 수 없습니다. 라인을 지운 뒤 바꾸세요.', 1;

            UPDATE TSAGIM SET gi_date = @p_gi_date, cust_id = @cust, so_id = @p_so_id, so_no = @so_no, ship_kind = @p_ship_kind, ship_date = @p_ship_date,
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

-- ============================================================
-- USP_SA_GI_S_1 - 라인 N/U/D (작성 상태의 출하에서만)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GI_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_serl INT = NULL,
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
        DECLARE @acc BIGINT, @gi_no VARCHAR(20), @so_id BIGINT, @stat VARCHAR(10);
        SELECT @acc = acc_id, @gi_no = gi_no, @so_id = so_id, @stat = stat_cd FROM TSAGIM WHERE gi_id = @p_gi_id;
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
            SELECT @item = item_id, @unit = unit_cd, @price = price, @so_qty = qty FROM TSASOD WHERE so_id = @so_id AND serl = @p_so_serl;
            IF @item IS NULL THROW 50001, N'이 출하의 수주에 없는 수주 라인입니다.', 1;
            IF @p_qty > @so_qty THROW 50001, N'출하수량이 수주수량을 넘을 수 없습니다.', 1;

            DECLARE @new_serl INT = (SELECT ISNULL(MAX(serl), 0) + 1 FROM TSAGID WHERE gi_id = @p_gi_id);
            INSERT INTO TSAGID (gi_id, serl, acc_id, gi_no, so_id, so_serl, item_id, unit_cd, lot_id, lot_no, wh_id, qty, price, amt, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_gi_id, @new_serl, @acc, @gi_no, @so_id, @p_so_serl, @item, @unit, @p_lot_id, @p_lot_no, @p_wh_id, @p_qty, @price, ROUND(@p_qty * ISNULL(@price, 0), 4), @p_remark,
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

-- ============================================================
-- USP_SA_GI_C_S - C 확정(재고 차감 + 수주 출하누계) / CC 확정취소(재고 복원)
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
        DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @stat VARCHAR(10), @so_id BIGINT;
        SELECT @acc = acc_id, @no = gi_no, @date = gi_date, @stat = stat_cd, @so_id = so_id FROM TSAGIM WHERE gi_id = @p_gi_id;
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
            FROM TSASOD s WHERE s.so_id = @so_id AND s.serl IN (SELECT so_serl FROM TSAGID WHERE gi_id = @p_gi_id);

            IF EXISTS (SELECT 1 FROM TSASOD s WHERE s.so_id = @so_id AND s.serl IN (SELECT so_serl FROM TSAGID WHERE gi_id = @p_gi_id) AND s.next_qty > s.qty)
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
            FROM TSASOD s WHERE s.so_id = @so_id AND s.serl IN (SELECT so_serl FROM TSAGID WHERE gi_id = @p_gi_id);
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

-- ============================================================
-- USP_SA_GISOPICK_Q - 출하등록의 "수주 불러오기" 팝업(popPick 규약): 출하 잔량이 남은 수주 라인
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_GISOPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 수주번호 */
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
            SELECT
                m.so_id, d.serl AS so_serl, m.so_no, m.so_date, m.cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, d.unit_cd, d.qty, ISNULL(d.next_qty, 0) AS next_qty, d.qty - ISNULL(d.next_qty, 0) AS remain_qty,
                d.price, ISNULL(d.delv_date, m.delv_date) AS delv_date
            FROM TSASOM m
                JOIN TSASOD d ON d.so_id = m.so_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y'
              AND d.qty - ISNULL(d.next_qty, 0) > 0
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

-- ============================================================
-- USP_SA_GILOTPICK_Q - "출하 LOT 불러오기" 팝업: 그 품목의 재고가 있는 LOT(창고별). 이동중 재고(wh_type TR)는 제외.
--   p_item_id 는 popPick 추가 파라미터. p_keyword = LOT번호/창고명.
-- ============================================================
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
                l.lot_id, s.lot_no, s.item_id, i.item_no, i.item_nm, s.unit_cd, s.wh_id, w.wh_nm, s.stock_qty,
                wm.wo_no, ISNULL(w.wh_type, '') AS wh_type
            FROM TMASTOCK s
                JOIN TBAITEM i ON i.item_id = s.item_id
                JOIN TBAWH w ON w.wh_id = s.wh_id
                LEFT JOIN TPRLOT l ON l.acc_id = s.acc_id AND l.item_id = s.item_id AND l.lot_no = s.lot_no
                LEFT JOIN TPRWOM wm ON wm.wo_id = l.wo_id
            WHERE s.stock_qty > 0 AND s.loc_id = 0 AND ISNULL(w.wh_type, '') <> 'TR'
              AND (@p_acc_id IS NULL OR s.acc_id = @p_acc_id)
              AND (@p_item_id IS NULL OR s.item_id = @p_item_id)
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

-- ============================================================
-- 메뉴: 영업관리(16) > 22 그룹을 '출하관리'로, 23 출하등록(frmGi), 출하현황(frmGiList)
-- ============================================================
UPDATE TSMMENU SET MENU_NM = N'출하관리' WHERE MENU_ID = 22 AND MENU_TYPE = 'GROUP';
UPDATE TSMMENU SET MODULE = 'SA', SCREEN_CLASS_NM = 'frmGi', PROC_PREFIX = 'USP_SA_', MENU_NM = N'출하등록', SORT_ORDER = 10
WHERE MENU_ID = 23 AND MENU_TYPE = 'FORM' AND SCREEN_CLASS_NM IS NULL;

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT N'출하현황', 22, 3, 'FORM', 'SA', 'frmGiList', 'USP_SA_', 20, 'Y', SUSER_SNAME(), GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SA' AND SCREEN_CLASS_NM = 'frmGiList');
GO
