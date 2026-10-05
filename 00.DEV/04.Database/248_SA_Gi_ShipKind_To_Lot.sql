-- 출하구분(자사창고 출하/외주처 직송)을 헤더에서 LOT 상세(TSAGIL.ship_kind)로 이동 (2026-10-01).
--  한 출하 안에서 LOT마다 자사 창고/외주처 창고가 섞일 수 있게 하고, 출하 LOT 선택 팝업에서 자사/외주처를 골라 조회한다.
--  서버는 LOT 행의 출하구분과 창고 종류(TBAWH.wh_type OS)가 맞는지 검증한다. 기존 헤더 값은 LOT 행으로 복사 후 헤더 컬럼을 지운다.

IF COL_LENGTH('TSAGIL', 'ship_kind') IS NULL ALTER TABLE TSAGIL ADD ship_kind VARCHAR(1) NULL;
GO
IF COL_LENGTH('TSAGIM', 'ship_kind') IS NOT NULL
BEGIN
    EXEC('UPDATE l SET ship_kind = ISNULL(m.ship_kind, CASE WHEN ISNULL(w.wh_type, '''') = ''OS'' THEN ''D'' ELSE ''W'' END) FROM TSAGIL l JOIN TSAGIM m ON m.gi_id = l.gi_id LEFT JOIN TBAWH w ON w.wh_id = l.wh_id WHERE l.ship_kind IS NULL');
END
ELSE
    UPDATE l SET ship_kind = CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN 'D' ELSE 'W' END FROM TSAGIL l LEFT JOIN TBAWH w ON w.wh_id = l.wh_id WHERE l.ship_kind IS NULL;
GO

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
                sd.qty AS so_qty, ISNULL(sd.next_qty, 0) AS so_next_qty, sd.qty - ISNULL(sd.next_qty, 0) AS so_remain_qty
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
            IF ISNULL(@p_gi_date, '') = '' THROW 50001, N'출하일자를 입력하세요.', 1;
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
            IF @stat IS NULL THROW 50001, N'출하 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 출하는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;
            -- 라인(수주 품목)이 이미 있으면 고객을 바꿀 수 없다(한 출하 = 한 고객)
            IF EXISTS (SELECT 1 FROM TSAGID d JOIN TSASOM so ON so.so_id = d.so_id WHERE d.gi_id = @p_gi_id AND so.cust_id <> @p_cust_id)
                THROW 50001, N'이미 다른 고객의 수주 품목이 담겨 있어 고객을 바꿀 수 없습니다. 라인을 지운 뒤 바꾸세요.', 1;

            UPDATE TSAGIM SET gi_date = @p_gi_date, cust_id = @p_cust_id, ship_date = @p_ship_date,
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

            -- 이 출하 품목의 LOT 합계(이 행 제외 + 새 수량)가 수주수량을 넘으면 거부
            DECLARE @so_qty NUMERIC(18,4) = (SELECT qty FROM TSASOD WHERE so_id = @so_id AND serl = @so_serl);
            DECLARE @others NUMERIC(18,4) = (SELECT ISNULL(SUM(qty), 0) FROM TSAGIL WHERE gi_id = @p_gi_id AND serl = @p_serl AND (@p_work_type = 'N' OR lot_serl <> @p_lot_serl));
            IF @others + @p_qty > @so_qty THROW 50001, N'출하수량의 합계가 수주수량을 넘을 수 없습니다.', 1;
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
                l.lot_id, s.lot_no, s.item_id, i.item_no, i.item_nm, s.unit_cd, s.wh_id, w.wh_nm, s.stock_qty,
                wm.wo_no, ISNULL(w.wh_type, '') AS wh_type,
                CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN 'D' ELSE 'W' END AS ship_kind,
                CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN N'외주처' ELSE N'자사창고' END AS ship_kind_nm
            FROM TMASTOCK s
                JOIN TBAITEM i ON i.item_id = s.item_id
                JOIN TBAWH w ON w.wh_id = s.wh_id
                LEFT JOIN TPRLOT l ON l.acc_id = s.acc_id AND l.item_id = s.item_id AND l.lot_no = s.lot_no
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

IF COL_LENGTH('TSAGIM', 'ship_kind') IS NOT NULL ALTER TABLE TSAGIM DROP COLUMN ship_kind;
GO
