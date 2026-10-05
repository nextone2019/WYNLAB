-- 영업(SA) 매출(세금계산서) 프로시저 (2026-10-05)
--  USP_SA_BILL_Q(Q 단건/L 라인목록) / USP_SA_BILL_S(N/U/D 헤더) / USP_SA_BILL_S_1(N/U/D 라인) / USP_SA_BILL_C_S(C/CC) / USP_SA_BILLINVCPICK_Q(명세서 라인 불러오기)
--  확정(C)된 거래명세서 라인에서 불러오고 부분매출 가능. 단가/세율은 명세서 값을 그대로 쓰고(수정 불가) 금액은 서버가 계산.
--  확정하면 명세서 라인 bill_qty = 확정 매출 수량 합계(명세서 수량 초과 시 거부). 출고 여부와 무관(출고 상태는 조회에서만 보여준다).

-- ============================================================
-- USP_SA_BILL_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_BILL_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),               /* Q 단건 / L 라인 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_bill_id BIGINT = NULL,
    @p_bill_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_invc_no VARCHAR(20) = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
    @p_keyword NVARCHAR(100) = NULL,
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
            SELECT TOP 1 @match_id = bill_id FROM TSABILLM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_bill_id IS NULL OR bill_id = @p_bill_id)
              AND (@p_bill_id IS NOT NULL OR @p_bill_no IS NULL OR bill_no LIKE '%' + @p_bill_no + '%')
            ORDER BY bill_id DESC;

            SELECT
                m.bill_id, m.acc_id, m.bill_no, m.bill_date, m.cust_id, c.cust_nm,
                m.tax_inv_no, m.tax_inv_date,
                m.dept_id, dp.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TSABILLM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT dp ON dp.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.bill_id = @match_id;

            SELECT
                d.bill_id, d.serl, d.acc_id, d.bill_no, d.invc_id, iv.invc_no, d.invc_serl, d.so_id, so.so_no, d.so_serl,
                d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                d.qty, d.price, d.amt, d.vat_type, d.vat_rate, d.vat, d.total_amt, d.cur_cd, d.exc_rate,
                d.kor_price, d.kor_amt, d.kor_vat, d.kor_total_amt, d.remark,
                id.qty AS invc_qty, id.gi_qty,
                id.qty - id.bill_qty - ISNULL(rs.reserved_qty, 0) AS invc_remain_qty,
                CASE WHEN id.gi_qty <= 0 THEN N'미출고' WHEN id.gi_qty >= id.qty THEN N'출고완료' ELSE N'부분출고' END AS gi_status
            FROM TSABILLD d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSAINVCM iv ON iv.invc_id = d.invc_id
                LEFT JOIN TSAINVCD id ON id.invc_id = d.invc_id AND id.serl = d.invc_serl
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                OUTER APPLY (SELECT SUM(x.qty) AS reserved_qty FROM TSABILLD x JOIN TSABILLM xm ON xm.bill_id = x.bill_id
                             WHERE xm.stat_cd = '0' AND x.invc_id = d.invc_id AND x.invc_serl = d.invc_serl AND x.bill_id <> d.bill_id) rs
            WHERE d.bill_id = @match_id
            ORDER BY d.serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.bill_id, m.bill_no, m.bill_date, m.cust_id, c.cust_nm, m.stat_cd, m.tax_inv_no, m.tax_inv_date,
                d.serl, iv.invc_no, d.invc_serl, so.so_no, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                d.qty, d.price, d.amt, d.vat, d.total_amt, d.cur_cd,
                CASE WHEN id.qty IS NULL THEN NULL WHEN id.gi_qty <= 0 THEN N'미출고' WHEN id.gi_qty >= id.qty THEN N'출고완료' ELSE N'부분출고' END AS gi_status,
                d.remark
            FROM TSABILLM m
                LEFT JOIN TSABILLD d ON d.bill_id = m.bill_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSAINVCM iv ON iv.invc_id = d.invc_id
                LEFT JOIN TSAINVCD id ON id.invc_id = d.invc_id AND id.serl = d.invc_serl
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_fr_date, '') = '' OR m.bill_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.bill_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_bill_no, '') = '' OR m.bill_no LIKE '%' + @p_bill_no + '%')
              AND (ISNULL(@p_invc_no, '') = '' OR iv.invc_no LIKE '%' + @p_invc_no + '%')
              AND (ISNULL(@p_cust_nm, N'') = N'' OR c.cust_nm LIKE N'%' + @p_cust_nm + N'%')
              AND (ISNULL(@p_keyword, N'') = N'' OR i.item_no LIKE N'%' + @p_keyword + N'%' OR i.item_nm LIKE N'%' + @p_keyword + N'%')
            ORDER BY m.bill_date DESC, m.bill_id DESC, d.serl;
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
-- USP_SA_BILL_S - 헤더 N/U/D
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_BILL_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_bill_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_bill_date VARCHAR(8) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_tax_inv_no NVARCHAR(50) = NULL,
    @p_tax_inv_date VARCHAR(8) = NULL,
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
            IF ISNULL(@p_bill_date, '') = '' THROW 50001, N'매출일자를 입력하세요.', 1;
            IF @p_cust_id IS NULL THROW 50001, N'고객을 선택하세요.', 1;
            IF NOT EXISTS (SELECT 1 FROM TBACUST WHERE cust_id = @p_cust_id) THROW 50001, N'고객을 찾을 수 없습니다.', 1;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSABILLM', 'bill_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TSABILLM (acc_id, bill_no, bill_date, cust_id, dept_id, emp_id, tax_inv_no, tax_inv_date, stat_cd, cfm_yn, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_bill_date, @p_cust_id, @p_dept_id, @p_emp_id, @p_tax_inv_no, @p_tax_inv_date, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_bill_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @stat = stat_cd FROM TSABILLM WHERE bill_id = @p_bill_id;
            IF @stat IS NULL THROW 50001, N'매출 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 매출은 수정할 수 없습니다. 먼저 확정취소하세요.', 1;
            IF EXISTS (SELECT 1 FROM TSABILLD d JOIN TSAINVCM iv ON iv.invc_id = d.invc_id WHERE d.bill_id = @p_bill_id AND iv.cust_id <> @p_cust_id)
                THROW 50001, N'이미 다른 고객의 명세서 품목이 담겨 있어 고객을 바꿀 수 없습니다. 라인을 지운 뒤 바꾸세요.', 1;

            UPDATE TSABILLM SET bill_date = @p_bill_date, cust_id = @p_cust_id, tax_inv_no = @p_tax_inv_no, tax_inv_date = @p_tax_inv_date,
                dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bill_id = @p_bill_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            SELECT @stat = stat_cd FROM TSABILLM WHERE bill_id = @p_bill_id;
            IF @stat IS NULL THROW 50001, N'매출 문서를 찾을 수 없습니다.', 1;
            IF @stat <> '0' THROW 50001, N'확정된 매출은 삭제할 수 없습니다. 먼저 확정취소하세요.', 1;
            DELETE FROM TSABILLD WHERE bill_id = @p_bill_id;
            DELETE FROM TSABILLM WHERE bill_id = @p_bill_id;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_bill_id AS VARCHAR(20));
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
-- USP_SA_BILL_S_1 - 라인 N/U/D. N: 확정 명세서 라인을 가져온다(같은 고객). N/U: 수량(부분매출)/비고. 단가/세율은 명세서 값.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_BILL_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_bill_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_invc_id BIGINT = NULL,
    @p_invc_serl INT = NULL,
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
        DECLARE @acc BIGINT, @bill_no VARCHAR(20), @cust BIGINT, @stat VARCHAR(10);
        SELECT @acc = acc_id, @bill_no = bill_no, @cust = cust_id, @stat = stat_cd FROM TSABILLM WHERE bill_id = @p_bill_id;
        IF @stat IS NULL THROW 50001, N'매출 문서를 찾을 수 없습니다.', 1;
        IF @stat <> '0' THROW 50001, N'확정된 매출은 수정할 수 없습니다. 먼저 확정취소하세요.', 1;

        IF @p_work_type = 'N'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_invc_serl) THROW 50001, N'명세서 라인을 찾을 수 없습니다.', 1;
            IF ISNULL((SELECT stat_cd FROM TSAINVCM WHERE invc_id = @p_invc_id), '') <> 'C' THROW 50001, N'확정된 거래명세서만 매출에 담을 수 있습니다.', 1;
            IF ISNULL((SELECT cust_id FROM TSAINVCM WHERE invc_id = @p_invc_id), -1) <> ISNULL(@cust, -2)
                THROW 50001, N'이 매출의 고객과 거래명세서의 고객이 다릅니다. 한 매출에는 같은 고객의 명세서만 담을 수 있습니다.', 1;
            IF EXISTS (SELECT 1 FROM TSABILLD WHERE bill_id = @p_bill_id AND invc_id = @p_invc_id AND invc_serl = @p_invc_serl)
                THROW 50001, N'이미 이 매출에 담긴 명세서 품목입니다.', 1;

            IF @p_serl IS NULL SET @p_serl = (SELECT ISNULL(MAX(serl), 0) + 1 FROM TSABILLD WHERE bill_id = @p_bill_id);
            ELSE IF EXISTS (SELECT 1 FROM TSABILLD WHERE bill_id = @p_bill_id AND serl = @p_serl) THROW 50001, N'매출 순번이 중복입니다.', 1;

            INSERT INTO TSABILLD (bill_id, serl, acc_id, bill_no, invc_id, invc_serl, so_id, so_serl, item_id, unit_cd, qty, cur_cd, exc_rate, price, vat_type, vat_rate, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT @p_bill_id, @p_serl, @acc, @bill_no, d.invc_id, d.serl, d.so_id, d.so_serl, d.item_id, d.unit_cd, ISNULL(@p_qty, 0), d.cur_cd, d.exc_rate, d.price, d.vat_type, d.vat_rate, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TSAINVCD d WHERE d.invc_id = @p_invc_id AND d.serl = @p_invc_serl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSABILLD WHERE bill_id = @p_bill_id AND serl = @p_serl) THROW 50001, N'매출 라인을 찾을 수 없습니다.', 1;
            SELECT @p_invc_id = invc_id, @p_invc_serl = invc_serl FROM TSABILLD WHERE bill_id = @p_bill_id AND serl = @p_serl;
            UPDATE TSABILLD SET qty = ISNULL(@p_qty, qty), remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bill_id = @p_bill_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TSABILLD WHERE bill_id = @p_bill_id AND serl = @p_serl;
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            DECLARE @cur_qty NUMERIC(18,4) = (SELECT qty FROM TSABILLD WHERE bill_id = @p_bill_id AND serl = @p_serl);
            IF @cur_qty <= 0 THROW 50001, N'매출 수량은 0보다 커야 합니다.', 1;

            -- 명세서 라인 잔량 = 명세서수량 - 확정 매출누계(bill_qty) - 다른 미확정 매출이 잡은 수량
            DECLARE @iv_qty NUMERIC(18,4), @iv_bill NUMERIC(18,4);
            SELECT @iv_qty = qty, @iv_bill = ISNULL(bill_qty, 0) FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_invc_serl;
            DECLARE @reserved NUMERIC(18,4) = ISNULL((
                SELECT SUM(x.qty) FROM TSABILLD x JOIN TSABILLM xm ON xm.bill_id = x.bill_id
                WHERE xm.stat_cd = '0' AND x.invc_id = @p_invc_id AND x.invc_serl = @p_invc_serl AND x.bill_id <> @p_bill_id), 0);
            IF @iv_bill + @reserved + @cur_qty > @iv_qty
            BEGIN
                DECLARE @m1 NVARCHAR(400) = N'명세서 잔량을 넘습니다. 명세서수량 ' + CAST(CAST(@iv_qty AS FLOAT) AS NVARCHAR(30)) + N' - 매출확정 ' + CAST(CAST(@iv_bill AS FLOAT) AS NVARCHAR(30))
                    + N' - 다른 미확정 매출 ' + CAST(CAST(@reserved AS FLOAT) AS NVARCHAR(30)) + N' = 잔량 ' + CAST(CAST(@iv_qty - @iv_bill - @reserved AS FLOAT) AS NVARCHAR(30))
                    + N' < 요청 ' + CAST(CAST(@cur_qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @m1, 1;
            END

            UPDATE d SET amt = a.amt, vat = a.vat, total_amt = a.amt + a.vat,
                kor_price = ROUND(ISNULL(d.price, 0) * a.er, 4), kor_amt = ROUND(a.amt * a.er, 4),
                kor_vat = ROUND(a.vat * a.er, 4), kor_total_amt = ROUND((a.amt + a.vat) * a.er, 4)
            FROM TSABILLD d
                CROSS APPLY (SELECT ROUND(d.qty * ISNULL(d.price, 0), 4) AS amt,
                                    ROUND(ROUND(d.qty * ISNULL(d.price, 0), 4) * ISNULL(d.vat_rate, 0) / 100, 4) AS vat,
                                    CASE WHEN ISNULL(d.exc_rate, 0) = 0 THEN 1 ELSE d.exc_rate END AS er) a
            WHERE d.bill_id = @p_bill_id AND d.serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_bill_id AS VARCHAR(20));
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
-- USP_SA_BILL_C_S - C 확정(명세서 bill_qty 재계산) / CC 확정취소
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_BILL_C_S
    @p_work_type VARCHAR(50),               /* C / CC */
    ---------------------------------------------------------------------------------------------------
    @p_bill_id BIGINT = NULL,
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
        SELECT @stat = stat_cd FROM TSABILLM WHERE bill_id = @p_bill_id;
        IF @stat IS NULL THROW 50001, N'매출 문서를 찾을 수 없습니다.', 1;

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 매출만 확정할 수 있습니다.', 1;
            IF NOT EXISTS (SELECT 1 FROM TSABILLD WHERE bill_id = @p_bill_id) THROW 50001, N'명세서 품목을 추가한 뒤 확정하세요.', 1;
            IF EXISTS (SELECT 1 FROM TSABILLD d JOIN TSAINVCM iv ON iv.invc_id = d.invc_id WHERE d.bill_id = @p_bill_id AND iv.stat_cd <> 'C')
                THROW 50001, N'확정되지 않은 거래명세서가 포함되어 있습니다.', 1;

            BEGIN TRAN;
            UPDATE TSABILLM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bill_id = @p_bill_id;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정된 매출만 확정취소할 수 있습니다.', 1;

            BEGIN TRAN;
            UPDATE TSABILLM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bill_id = @p_bill_id;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        UPDATE d SET bill_qty = (SELECT ISNULL(SUM(x.qty), 0) FROM TSABILLD x JOIN TSABILLM xm ON xm.bill_id = x.bill_id AND xm.stat_cd = 'C' WHERE x.invc_id = d.invc_id AND x.invc_serl = d.serl),
                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        FROM TSAINVCD d WHERE EXISTS (SELECT 1 FROM TSABILLD b WHERE b.bill_id = @p_bill_id AND b.invc_id = d.invc_id AND b.invc_serl = d.serl);

        IF EXISTS (SELECT 1 FROM TSAINVCD d WHERE EXISTS (SELECT 1 FROM TSABILLD b WHERE b.bill_id = @p_bill_id AND b.invc_id = d.invc_id AND b.invc_serl = d.serl) AND d.bill_qty > d.qty)
            THROW 50001, N'명세서수량을 초과하여 매출을 확정할 수 없습니다. (이미 확정된 다른 매출을 포함한 합계가 명세서수량보다 큽니다)', 1;

        COMMIT TRAN;
        SET @GeneratedCode = CAST(@p_bill_id AS VARCHAR(20));
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
-- USP_SA_BILLINVCPICK_Q - 매출 "명세서 품목 불러오기" 팝업(popPick 규약). 확정 명세서, 잔량(매출확정 + 다른 미확정 매출 차감) > 0. 출고 상태는 참고용 컬럼.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_BILLINVCPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 명세서번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_bill_id BIGINT = NULL,               /* 지금 편집 중인 매출 */
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
                m.invc_id, d.serl AS invc_serl, m.invc_no, m.invc_date, m.cust_id, c.cust_nm, d.so_no,
                d.item_id, i.item_no, i.item_nm, d.unit_cd, d.qty, d.bill_qty, ISNULL(rs.reserved_qty, 0) AS reserved_qty,
                d.qty - d.bill_qty - ISNULL(rs.reserved_qty, 0) AS remain_qty,
                d.price, d.vat_rate, d.cur_cd,
                CASE WHEN d.gi_qty <= 0 THEN N'미출고' WHEN d.gi_qty >= d.qty THEN N'출고완료' ELSE N'부분출고' END AS gi_status
            FROM TSAINVCM m
                JOIN TSAINVCD d ON d.invc_id = m.invc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                OUTER APPLY (SELECT SUM(x.qty) AS reserved_qty FROM TSABILLD x JOIN TSABILLM xm ON xm.bill_id = x.bill_id
                             WHERE xm.stat_cd = '0' AND x.invc_id = d.invc_id AND x.invc_serl = d.serl AND (@p_bill_id IS NULL OR x.bill_id <> @p_bill_id)) rs
            WHERE m.stat_cd = 'C'
              AND d.qty - d.bill_qty - ISNULL(rs.reserved_qty, 0) > 0
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
