-- 영업(SA) 거래명세서 프로시저 (2026-10-05)
--  USP_SA_INVC_Q(Q 단건/L 라인목록) / USP_SA_INVC_S(N/U/D 헤더) / USP_SA_INVC_S_1(N/U/D 라인) / USP_SA_INVC_C_S(C/CC) / USP_SA_INVCSOPICK_Q(수주 라인 불러오기)
--  수주 라인 1건 = 명세서 라인 1행. 수량/단가/부가세율 수정 가능(금액은 서버가 계산). 확정하면 수주 라인 next_qty = 확정 명세서 수량 합계.
--  다른 미확정 명세서가 잡은 수량도 잔량에서 뺀다(출하 통제 방식과 동일).

-- ============================================================
-- USP_SA_INVC_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_INVC_Q
    @p_acc_id BIGINT = NULL,                /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),               /* Q 단건 / L 라인 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_invc_id BIGINT = NULL,
    @p_invc_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_so_no VARCHAR(20) = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = invc_id FROM TSAINVCM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_invc_id IS NULL OR invc_id = @p_invc_id)
              AND (@p_invc_id IS NOT NULL OR @p_invc_no IS NULL OR invc_no LIKE '%' + @p_invc_no + '%')
            ORDER BY invc_id DESC;

            SELECT
                m.invc_id, m.acc_id, m.invc_no, m.invc_date, m.cust_id, c.cust_nm,
                m.dept_id, dp.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TSAINVCM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT dp ON dp.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.invc_id = @match_id;

            SELECT
                d.invc_id, d.serl, d.acc_id, d.invc_no, d.so_id, d.so_no, d.so_serl,
                d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                d.qty, d.price, d.amt, d.vat_type, d.vat_rate, d.vat, d.total_amt, d.cur_cd, d.exc_rate,
                d.kor_price, d.kor_amt, d.kor_vat, d.kor_total_amt,
                d.gi_qty, d.bill_qty, d.remark,
                sd.qty AS so_qty,
                sd.qty - ISNULL(sd.next_qty, 0) - ISNULL(rs.reserved_qty, 0) AS so_remain_qty
            FROM TSAINVCD d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
                OUTER APPLY (SELECT SUM(x.qty) AS reserved_qty FROM TSAINVCD x JOIN TSAINVCM xm ON xm.invc_id = x.invc_id
                             WHERE xm.stat_cd = '0' AND x.so_id = d.so_id AND x.so_serl = d.so_serl AND x.invc_id <> d.invc_id) rs
            WHERE d.invc_id = @match_id
            ORDER BY d.serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.invc_id, m.invc_no, m.invc_date, m.cust_id, c.cust_nm, m.stat_cd,
                d.serl, d.so_no, d.so_serl, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                d.qty, d.price, d.amt, d.vat, d.total_amt, d.cur_cd,
                d.gi_qty, d.bill_qty,
                CASE WHEN d.qty IS NULL THEN NULL WHEN d.gi_qty <= 0 THEN N'미출고' WHEN d.gi_qty >= d.qty THEN N'출고완료' ELSE N'부분출고' END AS gi_status,
                CASE WHEN d.qty IS NULL THEN NULL WHEN d.bill_qty <= 0 THEN N'미매출' WHEN d.bill_qty >= d.qty THEN N'매출완료' ELSE N'부분매출' END AS bill_status,
                d.remark
            FROM TSAINVCM m
                LEFT JOIN TSAINVCD d ON d.invc_id = m.invc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_fr_date, '') = '' OR m.invc_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.invc_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_invc_no, '') = '' OR m.invc_no LIKE '%' + @p_invc_no + '%')
              AND (ISNULL(@p_so_no, '') = '' OR d.so_no LIKE '%' + @p_so_no + '%')
              AND (ISNULL(@p_cust_nm, N'') = N'' OR c.cust_nm LIKE N'%' + @p_cust_nm + N'%')
              AND (ISNULL(@p_keyword, N'') = N'' OR i.item_no LIKE N'%' + @p_keyword + N'%' OR i.item_nm LIKE N'%' + @p_keyword + N'%')
            ORDER BY m.invc_date DESC, m.invc_id DESC, d.serl;
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
-- USP_SA_INVC_S - 헤더 N/U/D
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_INVC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_invc_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_invc_date VARCHAR(8) = NULL,
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
        DECLARE @stat VARCHAR(10);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_invc_date, '') = '' THROW 50001, N'명세서일자를 입력하세요.', 1;
            IF @p_cust_id IS NULL THROW 50001, N'고객을 선택하세요.', 1;
            IF NOT EXISTS (SELECT 1 FROM TBACUST WHERE cust_id = @p_cust_id) THROW 50001, N'고객을 찾을 수 없습니다.', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSAINVCM', 'invc_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TSAINVCM (acc_id, invc_no, invc_date, cust_id, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_invc_date, @p_cust_id, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_invc_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @stat = stat_cd FROM TSAINVCM WHERE invc_id = @p_invc_id;
            IF @stat IS NULL THROW 50001, N'거래명세서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 거래명세서는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;
            IF EXISTS (SELECT 1 FROM TSAINVCD d JOIN TSASOM so ON so.so_id = d.so_id WHERE d.invc_id = @p_invc_id AND so.cust_id <> @p_cust_id)
                THROW 50001, N'이미 다른 고객의 수주 품목이 담겨 있어 고객을 바꿀 수 없습니다. 라인을 지운 뒤 바꾸세요.', 1;

            UPDATE TSAINVCM SET invc_date = @p_invc_date, cust_id = @p_cust_id, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE invc_id = @p_invc_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            SELECT @stat = stat_cd FROM TSAINVCM WHERE invc_id = @p_invc_id;
            IF @stat IS NULL THROW 50001, N'거래명세서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 거래명세서는 삭제할 수 없습니다. 먼저 확정취소하세요.', 1;
            DELETE FROM TSAINVCD WHERE invc_id = @p_invc_id;
            DELETE FROM TSAINVCM WHERE invc_id = @p_invc_id;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_invc_id AS VARCHAR(20));
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
-- USP_SA_INVC_S_1 - 라인 N/U/D. N: 수주 라인을 가져온다(같은 고객, 확정(결재완료) 수주만). N/U: 수량/단가/부가세율/비고. 금액은 서버가 계산.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_INVC_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_invc_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_so_id BIGINT = NULL,
    @p_so_serl INT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_price NUMERIC(18,4) = NULL,
    @p_vat_rate NUMERIC(9,4) = NULL,
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
        DECLARE @acc BIGINT, @invc_no VARCHAR(20), @cust BIGINT, @stat VARCHAR(10);
        SELECT @acc = acc_id, @invc_no = invc_no, @cust = cust_id, @stat = stat_cd FROM TSAINVCM WHERE invc_id = @p_invc_id;
        IF @stat IS NULL THROW 50001, N'거래명세서를 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 거래명세서는 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @item BIGINT, @unit VARCHAR(10), @vtype VARCHAR(10), @vrate NUMERIC(9,4), @so_no VARCHAR(20), @cur VARCHAR(10), @exc NUMERIC(18,4);
            SELECT @item = d.item_id, @unit = d.unit_cd, @vtype = ISNULL(d.vat_type, m.vat_type), @vrate = ISNULL(d.vat_rate, m.vat_rate), @so_no = d.so_no, @cur = m.cur_cd, @exc = m.exc_rate
            FROM TSASOD d JOIN TSASOM m ON m.so_id = d.so_id WHERE d.so_id = @p_so_id AND d.serl = @p_so_serl;
            IF @item IS NULL THROW 50001, N'수주 라인을 찾을 수 없습니다.', 1;
            IF ISNULL((SELECT stat_cd FROM TSASOM WHERE so_id = @p_so_id), '') <> 'C' THROW 50001, N'확정(결재완료)된 수주만 거래명세서에 담을 수 있습니다.', 1;
            IF ISNULL((SELECT cust_id FROM TSASOM WHERE so_id = @p_so_id), -1) <> ISNULL(@cust, -2)
                THROW 50001, N'이 거래명세서의 고객과 수주의 고객이 다릅니다. 한 거래명세서에는 같은 고객의 수주만 담을 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TSAINVCD WHERE invc_id = @p_invc_id AND so_id = @p_so_id AND so_serl = @p_so_serl)
                THROW 50001, N'이미 이 거래명세서에 담긴 수주 품목입니다.', 1;

            IF @p_serl IS NULL SET @p_serl = (SELECT ISNULL(MAX(serl), 0) + 1 FROM TSAINVCD WHERE invc_id = @p_invc_id);
            ELSE IF EXISTS (SELECT 1 FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_serl) THROW 50001, N'명세서 순번이 중복입니다.', 1;
            IF @p_vat_rate IS NOT NULL SET @vrate = @p_vat_rate;

            INSERT INTO TSAINVCD (invc_id, serl, acc_id, invc_no, so_id, so_no, so_serl, item_id, unit_cd, qty, cur_cd, exc_rate, price, vat_type, vat_rate, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_invc_id, @p_serl, @acc, @invc_no, @p_so_id, @so_no, @p_so_serl, @item, @unit, ISNULL(@p_qty, 0), @cur, @exc, @p_price, @vtype, @vrate, @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_serl) THROW 50001, N'명세서 라인을 찾을 수 없습니다.', 1;
            SELECT @p_so_id = so_id, @p_so_serl = so_serl FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_serl;
            UPDATE TSAINVCD SET qty = ISNULL(@p_qty, qty), price = @p_price, vat_rate = ISNULL(@p_vat_rate, vat_rate), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE invc_id = @p_invc_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_serl;
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            DECLARE @cur_qty NUMERIC(18,4) = (SELECT qty FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_serl);
            IF @cur_qty <= 0 THROW 50001, N'명세서 수량은 0보다 커야 합니다.', 1;

            -- 수주 잔량 = 수주수량 - 확정 명세서 누계(next_qty) - 다른 미확정 명세서가 잡은 수량
            DECLARE @so_qty NUMERIC(18,4), @so_next NUMERIC(18,4);
            SELECT @so_qty = qty, @so_next = ISNULL(next_qty, 0) FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_so_serl;
            DECLARE @reserved NUMERIC(18,4) = ISNULL((
                SELECT SUM(x.qty) FROM TSAINVCD x JOIN TSAINVCM xm ON xm.invc_id = x.invc_id
                WHERE xm.stat_cd = '0' AND x.so_id = @p_so_id AND x.so_serl = @p_so_serl AND x.invc_id <> @p_invc_id), 0);
            IF @so_next + @reserved + @cur_qty > @so_qty
            BEGIN
                DECLARE @m1 NVARCHAR(400) = N'수주 잔량을 넘습니다. 수주수량 ' + CAST(CAST(@so_qty AS FLOAT) AS NVARCHAR(30)) + N' - 확정 명세서 ' + CAST(CAST(@so_next AS FLOAT) AS NVARCHAR(30))
                    + N' - 다른 미확정 명세서 ' + CAST(CAST(@reserved AS FLOAT) AS NVARCHAR(30)) + N' = 잔량 ' + CAST(CAST(@so_qty - @so_next - @reserved AS FLOAT) AS NVARCHAR(30))
                    + N' < 요청 ' + CAST(CAST(@cur_qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @m1, 1;
            END

            -- 금액 계산(서버 기준): 공급가 = 수량*단가, 부가세 = 공급가*세율, 원화 = 각 금액*환율
            UPDATE d SET amt = a.amt, vat = a.vat, total_amt = a.amt + a.vat,
                kor_price = ROUND(ISNULL(d.price, 0) * a.er, 4), kor_amt = ROUND(a.amt * a.er, 4),
                kor_vat = ROUND(a.vat * a.er, 4), kor_total_amt = ROUND((a.amt + a.vat) * a.er, 4)
            FROM TSAINVCD d
                CROSS APPLY (SELECT ROUND(d.qty * ISNULL(d.price, 0), 4) AS amt,
                                    ROUND(ROUND(d.qty * ISNULL(d.price, 0), 4) * ISNULL(d.vat_rate, 0) / 100, 4) AS vat,
                                    CASE WHEN ISNULL(d.exc_rate, 0) = 0 THEN 1 ELSE d.exc_rate END AS er) a
            WHERE d.invc_id = @p_invc_id AND d.serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_invc_id AS VARCHAR(20));
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
-- USP_SA_INVC_C_S - C 확정(수주 next_qty 재계산) / CC 확정취소(출고/매출이 걸려 있으면 거부)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_INVC_C_S
    @p_work_type VARCHAR(50),               /* C / CC */
    ---------------------------------------------------------------------------------------------------
    @p_invc_id BIGINT = NULL,
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
        DECLARE @stat VARCHAR(10);
        SELECT @stat = stat_cd FROM TSAINVCM WHERE invc_id = @p_invc_id;
        IF @stat IS NULL THROW 50001, N'거래명세서를 찾을 수 없습니다.', 1;

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 거래명세서만 확정할 수 있습니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TSAINVCD WHERE invc_id = @p_invc_id) THROW 50001, N'수주 품목을 추가한 뒤 확정하세요.', 1;
            IF EXISTS (SELECT 1 FROM TSAINVCD d JOIN TSASOM so ON so.so_id = d.so_id WHERE d.invc_id = @p_invc_id AND (ISNULL(so.stat_cd, '') <> 'C' OR ISNULL(so.stop_yn, 'N') = 'Y'))
                THROW 50001, N'확정되지 않았거나 마감된 수주가 포함되어 있습니다.', 1;

            BEGIN TRAN;
            UPDATE TSAINVCM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE invc_id = @p_invc_id;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정된 거래명세서만 확정취소할 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TSAGID WHERE invc_id = @p_invc_id) THROW 50001, N'이 거래명세서로 출고가 등록되어 있어 확정취소할 수 없습니다. 출고를 먼저 삭제하세요.', 1;
            IF EXISTS (SELECT 1 FROM TSABILLD WHERE invc_id = @p_invc_id) THROW 50001, N'이 거래명세서로 매출이 등록되어 있어 확정취소할 수 없습니다. 매출을 먼저 삭제하세요.', 1;

            BEGIN TRAN;
            UPDATE TSAINVCM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE invc_id = @p_invc_id;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        -- 수주 라인의 명세서 누계 재계산(확정 명세서 수량 합계) - 수주수량을 넘으면 거부
        UPDATE s SET next_qty = (SELECT ISNULL(SUM(d.qty), 0) FROM TSAINVCD d JOIN TSAINVCM m ON m.invc_id = d.invc_id AND m.stat_cd = 'C' WHERE d.so_id = s.so_id AND d.so_serl = s.serl),
                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        FROM TSASOD s WHERE EXISTS (SELECT 1 FROM TSAINVCD g WHERE g.invc_id = @p_invc_id AND g.so_id = s.so_id AND g.so_serl = s.serl);

        IF EXISTS (SELECT 1 FROM TSASOD s WHERE EXISTS (SELECT 1 FROM TSAINVCD g WHERE g.invc_id = @p_invc_id AND g.so_id = s.so_id AND g.so_serl = s.serl) AND s.next_qty > s.qty)
            THROW 50001, N'수주수량을 초과하여 거래명세서를 확정할 수 없습니다. (이미 확정된 다른 명세서를 포함한 합계가 수주수량보다 큽니다)', 1;

        COMMIT TRAN;
        SET @GeneratedCode = CAST(@p_invc_id AS VARCHAR(20));
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
-- USP_SA_INVCSOPICK_Q - 거래명세서 "수주 품목 불러오기" 팝업(popPick 규약). 확정 수주, 마감 제외, 잔량(확정 명세서 + 다른 미확정 명세서 차감) > 0
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_INVCSOPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 수주번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명 */
    @p_invc_id BIGINT = NULL,               /* 지금 편집 중인 명세서(이 명세서가 잡은 수량은 '다른 미확정'에서 제외) */
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
                d.item_id, i.item_no, i.item_nm, d.unit_cd, d.qty, ISNULL(d.next_qty, 0) AS next_qty, ISNULL(rs.reserved_qty, 0) AS reserved_qty,
                d.qty - ISNULL(d.next_qty, 0) - ISNULL(rs.reserved_qty, 0) AS remain_qty,
                d.price, ISNULL(d.vat_rate, m.vat_rate) AS vat_rate, m.cur_cd, ISNULL(d.delv_date, m.delv_date) AS delv_date
            FROM TSASOM m
                JOIN TSASOD d ON d.so_id = m.so_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                OUTER APPLY (SELECT SUM(x.qty) AS reserved_qty FROM TSAINVCD x JOIN TSAINVCM xm ON xm.invc_id = x.invc_id
                             WHERE xm.stat_cd = '0' AND x.so_id = d.so_id AND x.so_serl = d.serl AND (@p_invc_id IS NULL OR x.invc_id <> @p_invc_id)) rs
            WHERE m.stat_cd = 'C' AND ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y'
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
