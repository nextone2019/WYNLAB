-- 매출/거래명세서 마스터에 통화/환율/부가세구분/합계 추가 (2026-10-05, WYNLAB_DEV 전용)
--  요구: 매출 마스터에 화폐단위/환율 등의 정보가 있어야 한다(설계서 TB_MES_SA_SHPM_MST/출하마스터에는 CUR_CD/EXC_RT/VAT_TYPE/VAT_RT 정의).
--  결정(사용자): 거래명세서는 수주 환율을 그대로 유지, 매출은 매출일 기준 환율.
--  1) TSAINVCM / TSABILLM: cur_cd, exc_rate, vat_type, vat_rate, amt, vat, total_amt, kor_amt, kor_vat, kor_total_amt 추가(없던 컬럼만).
--  2) FN_SA_EXCRATE(통화, 일자): 환율정보(TBAEXCRATE)에서 일자 이전 가장 가까운 기준일의 환율(exc_rate / unit_amt = 1단위당 원화). KRW/빈 통화는 1, 등록된 환율이 없으면 NULL.
--  3) USP_SA_INVC_HDR_SYNC / USP_SA_BILL_HDR_SYNC: 라인 기준으로 헤더 통화/부가세/환율/합계를 맞추는 내부 프로시저.
--     - 거래명세서: 첫 라인의 통화/환율(수주 값)/부가세구분을 헤더에 올린다. 라인이 모두 지워지면 헤더 값도 비운다.
--     - 매출: 첫 라인의 통화/부가세구분을 헤더에 올리고, 환율은 매출일 기준(FN_SA_EXCRATE)으로 정한 뒤 모든 라인의 환율/원화금액을 그 환율로 다시 계산한다(KRW는 1).
--       직접 입력한 환율(@p_exc_rate)이 있으면 그 값, 매출일을 바꾸면 새 매출일 기준으로 다시 정한다. 외화인데 환율정보에 등록된 환율이 없으면 저장을 거부한다.
--  4) 라인 저장(USP_SA_INVC_S_1 / USP_SA_BILL_S_1)이 위 동기화를 호출하고, 통화/환율(명세서)/부가세구분이 다른 품목은 한 문서에 못 담게 막는다.
--  5) 조회(USP_SA_INVC_Q / USP_SA_BILL_Q)의 헤더 결과에 새 컬럼을 추가, USP_SA_BILL_S에 @p_exc_rate 추가.
--  *** 새 조회 컬럼/파라미터는 화면(SA 모듈)과 같이 배포한다. 여러 번 실행해도 안전하다. ***

IF COL_LENGTH('TSAINVCM', 'cur_cd') IS NULL
    ALTER TABLE TSAINVCM ADD cur_cd VARCHAR(10) NULL, exc_rate NUMERIC(18,4) NULL, vat_type VARCHAR(10) NULL, vat_rate NUMERIC(9,4) NULL,
        amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSAINVCM_amt DEFAULT 0, vat NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSAINVCM_vat DEFAULT 0,
        total_amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSAINVCM_total DEFAULT 0, kor_amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSAINVCM_kor_amt DEFAULT 0,
        kor_vat NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSAINVCM_kor_vat DEFAULT 0, kor_total_amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSAINVCM_kor_total DEFAULT 0;
GO
IF COL_LENGTH('TSABILLM', 'cur_cd') IS NULL
    ALTER TABLE TSABILLM ADD cur_cd VARCHAR(10) NULL, exc_rate NUMERIC(18,4) NULL, vat_type VARCHAR(10) NULL, vat_rate NUMERIC(9,4) NULL,
        amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSABILLM_amt DEFAULT 0, vat NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSABILLM_vat DEFAULT 0,
        total_amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSABILLM_total DEFAULT 0, kor_amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSABILLM_kor_amt DEFAULT 0,
        kor_vat NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSABILLM_kor_vat DEFAULT 0, kor_total_amt NUMERIC(18,4) NOT NULL CONSTRAINT DF_TSABILLM_kor_total DEFAULT 0;
GO

CREATE OR ALTER FUNCTION dbo.FN_SA_EXCRATE (@cur_cd VARCHAR(10), @date VARCHAR(8))
RETURNS NUMERIC(18,4)
AS
BEGIN
    IF ISNULL(@cur_cd, '') IN ('', 'KRW') RETURN 1;
    RETURN (SELECT TOP 1 CAST(exc_rate / ISNULL(NULLIF(unit_amt, 0), 1) AS NUMERIC(18,4))
            FROM TBAEXCRATE
            WHERE cur_cd = @cur_cd AND base_date <= ISNULL(NULLIF(@date, ''), '99991231') AND ISNULL(exc_rate, 0) > 0
            ORDER BY base_date DESC);
END
GO

CREATE OR ALTER PROCEDURE USP_SA_INVC_HDR_SYNC
    @p_invc_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @has BIT = 0, @cur VARCHAR(10), @exc NUMERIC(18,4), @vt VARCHAR(10), @vr NUMERIC(9,4);
    SELECT TOP 1 @has = 1, @cur = cur_cd, @exc = exc_rate, @vt = vat_type, @vr = vat_rate FROM TSAINVCD WHERE invc_id = @p_invc_id ORDER BY serl;

    UPDATE m SET cur_cd = CASE WHEN @has = 1 THEN @cur END,
                 exc_rate = CASE WHEN @has = 1 THEN ISNULL(NULLIF(@exc, 0), 1) END,
                 vat_type = CASE WHEN @has = 1 THEN @vt END,
                 vat_rate = CASE WHEN @has = 1 THEN @vr END,
                 amt = t.amt, vat = t.vat, total_amt = t.total_amt, kor_amt = t.kor_amt, kor_vat = t.kor_vat, kor_total_amt = t.kor_total_amt,
                 upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    FROM TSAINVCM m
        CROSS APPLY (SELECT ISNULL(SUM(amt), 0) amt, ISNULL(SUM(vat), 0) vat, ISNULL(SUM(total_amt), 0) total_amt,
                            ISNULL(SUM(kor_amt), 0) kor_amt, ISNULL(SUM(kor_vat), 0) kor_vat, ISNULL(SUM(kor_total_amt), 0) kor_total_amt
                     FROM TSAINVCD WHERE invc_id = m.invc_id) t
    WHERE m.invc_id = @p_invc_id;
END
GO

CREATE OR ALTER PROCEDURE USP_SA_BILL_HDR_SYNC
    @p_bill_id BIGINT,
    @p_exc_override NUMERIC(18,4) = NULL,    /* 사용자가 직접 입력한 환율(외화일 때만 적용) */
    @p_relookup BIT = 0,                      /* 1: 매출일 기준 환율을 다시 가져온다(매출일을 바꿨을 때) */
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @has BIT = 0, @cur VARCHAR(10), @vt VARCHAR(10), @vr NUMERIC(9,4), @date VARCHAR(8), @old_exc NUMERIC(18,4), @rate NUMERIC(18,4);
    SELECT @date = bill_date, @old_exc = exc_rate FROM TSABILLM WHERE bill_id = @p_bill_id;
    SELECT TOP 1 @has = 1, @cur = cur_cd, @vt = vat_type, @vr = vat_rate FROM TSABILLD WHERE bill_id = @p_bill_id ORDER BY serl;

    IF @has = 1
    BEGIN
        IF ISNULL(@cur, 'KRW') = 'KRW' SET @rate = 1;
        ELSE IF ISNULL(@p_exc_override, 0) > 0 SET @rate = @p_exc_override;
        ELSE IF @p_relookup = 1 OR ISNULL(@old_exc, 0) = 0
        BEGIN
            SET @rate = dbo.FN_SA_EXCRATE(@cur, @date);
            IF @rate IS NULL
            BEGIN
                DECLARE @m NVARCHAR(300) = @cur + N' 통화의 환율이 없습니다. 환율정보에 매출일(' + ISNULL(@date, N'') + N') 이전 환율을 등록한 뒤 다시 저장하세요.';
                THROW 50001, @m, 1;
            END
        END
        ELSE SET @rate = @old_exc;

        UPDATE d SET exc_rate = @rate, kor_price = ROUND(ISNULL(d.price, 0) * @rate, 4), kor_amt = ROUND(ISNULL(d.amt, 0) * @rate, 4),
                     kor_vat = ROUND(ISNULL(d.vat, 0) * @rate, 4), kor_total_amt = ROUND((ISNULL(d.amt, 0) + ISNULL(d.vat, 0)) * @rate, 4)
        FROM TSABILLD d WHERE d.bill_id = @p_bill_id;
    END

    UPDATE m SET cur_cd = CASE WHEN @has = 1 THEN @cur END, exc_rate = CASE WHEN @has = 1 THEN @rate END,
                 vat_type = CASE WHEN @has = 1 THEN @vt END, vat_rate = CASE WHEN @has = 1 THEN @vr END,
                 amt = t.amt, vat = t.vat, total_amt = t.total_amt, kor_amt = t.kor_amt, kor_vat = t.kor_vat, kor_total_amt = t.kor_total_amt,
                 upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    FROM TSABILLM m
        CROSS APPLY (SELECT ISNULL(SUM(amt), 0) amt, ISNULL(SUM(vat), 0) vat, ISNULL(SUM(total_amt), 0) total_amt,
                            ISNULL(SUM(kor_amt), 0) kor_amt, ISNULL(SUM(kor_vat), 0) kor_vat, ISNULL(SUM(kor_total_amt), 0) kor_total_amt
                     FROM TSABILLD WHERE bill_id = m.bill_id) t
    WHERE m.bill_id = @p_bill_id;
END
GO
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
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark,
                m.cur_cd, m.exc_rate, m.vat_type, m.vat_rate, m.amt, m.vat, m.total_amt, m.kor_amt, m.kor_vat, m.kor_total_amt
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
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark,
                m.cur_cd, m.exc_rate, m.vat_type, m.vat_rate, m.amt, m.vat, m.total_amt, m.kor_amt, m.kor_vat, m.kor_total_amt
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

            -- 한 거래명세서는 통화/환율(수주 환율 그대로)/부가세구분이 하나다 - 이미 담긴 품목이 있으면 같은 값의 수주 품목만 담을 수 있다.
            DECLARE @h_cur VARCHAR(10), @h_exc NUMERIC(18,4), @h_vt VARCHAR(10);
            SELECT TOP 1 @h_cur = cur_cd, @h_exc = exc_rate, @h_vt = vat_type FROM TSAINVCD WHERE invc_id = @p_invc_id ORDER BY serl;
            IF @@ROWCOUNT > 0
            BEGIN
                IF ISNULL(@h_cur, 'KRW') <> ISNULL(@cur, 'KRW')
                    THROW 50001, N'통화가 다른 수주 품목은 같은 거래명세서에 담을 수 없습니다. (거래명세서 통화와 수주 통화가 다릅니다)', 1;
                IF ISNULL(NULLIF(@h_exc, 0), 1) <> ISNULL(NULLIF(@exc, 0), 1)
                    THROW 50001, N'수주 환율이 다른 수주 품목은 같은 거래명세서에 담을 수 없습니다. 환율이 같은 수주끼리 발행하세요.', 1;
                IF ISNULL(@h_vt, '') <> ISNULL(@vtype, '')
                    THROW 50001, N'부가세 구분이 다른 수주 품목은 같은 거래명세서에 담을 수 없습니다.', 1;
            END

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

        -- 헤더 통화/환율/부가세와 합계를 라인 기준으로 맞춘다.
        EXEC USP_SA_INVC_HDR_SYNC @p_invc_id, @p_user_id, @p_client_pc;

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

            -- 한 매출(세금계산서)은 통화와 부가세구분이 하나다(환율은 매출일 기준으로 헤더에서 정한다).
            DECLARE @h_cur VARCHAR(10), @h_vt VARCHAR(10), @n_cur VARCHAR(10), @n_vt VARCHAR(10);
            SELECT TOP 1 @h_cur = cur_cd, @h_vt = vat_type FROM TSABILLD WHERE bill_id = @p_bill_id ORDER BY serl;
            IF @@ROWCOUNT > 0
            BEGIN
                SELECT @n_cur = cur_cd, @n_vt = vat_type FROM TSAINVCD WHERE invc_id = @p_invc_id AND serl = @p_invc_serl;
                IF ISNULL(@h_cur, 'KRW') <> ISNULL(@n_cur, 'KRW')
                    THROW 50001, N'통화가 다른 명세서 품목은 같은 매출에 담을 수 없습니다. 통화별로 매출을 나누어 등록하세요.', 1;
                IF ISNULL(@h_vt, '') <> ISNULL(@n_vt, '')
                    THROW 50001, N'부가세 구분이 다른 명세서 품목은 같은 매출에 담을 수 없습니다.', 1;
            END

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

        -- 헤더 통화/부가세, 매출일 기준 환율, 합계를 라인 기준으로 맞춘다(라인 환율/원화금액도 이 환율로 다시 계산).
        EXEC USP_SA_BILL_HDR_SYNC @p_bill_id, NULL, 0, @p_user_id, @p_client_pc;

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
    @p_exc_rate NUMERIC(18,4) = NULL,        /* 외화 매출의 환율을 직접 고칠 때만(비우면 유지, 매출일을 바꾸면 새 매출일 환율로 자동) */
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
        DECLARE @stat VARCHAR(10), @old_date VARCHAR(8), @relook BIT = 0;

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

            SELECT @old_date = bill_date FROM TSABILLM WHERE bill_id = @p_bill_id;
            IF ISNULL(@old_date, '') <> @p_bill_date SET @relook = 1;

            UPDATE TSABILLM SET bill_date = @p_bill_date, cust_id = @p_cust_id, tax_inv_no = @p_tax_inv_no, tax_inv_date = @p_tax_inv_date,
                dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bill_id = @p_bill_id;

            -- 매출일을 바꿨으면 새 매출일 기준 환율로, 환율을 직접 입력했으면 그 값으로 라인/합계를 다시 계산한다.
            EXEC USP_SA_BILL_HDR_SYNC @p_bill_id, @p_exc_rate, @relook, @p_user_id, @p_client_pc;
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
