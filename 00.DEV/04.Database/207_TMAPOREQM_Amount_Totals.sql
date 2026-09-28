-- 구매요청 헤더(TMAPOREQM)에 금액 합계 컬럼 추가(2026-09-25) - 품목(TMAPOREQD)의 amt/vat/total_amt 합계를 품목 저장(USP_MA_POREQ_S_1)마다 다시 계산해서 넣는다.
-- 기존 데이터는 여기서 한 번 채운다. 조회(USP_MA_POREQ_Q) 헤더 결과에도 amt/vat/total_amt를 함께 내려준다. 라이브 정의(206)에 해당 부분만 더했다.

IF COL_LENGTH('TMAPOREQM', 'amt') IS NULL ALTER TABLE TMAPOREQM ADD amt NUMERIC(18,6) NULL;
IF COL_LENGTH('TMAPOREQM', 'vat') IS NULL ALTER TABLE TMAPOREQM ADD vat NUMERIC(18,6) NULL;
IF COL_LENGTH('TMAPOREQM', 'total_amt') IS NULL ALTER TABLE TMAPOREQM ADD total_amt NUMERIC(18,6) NULL;
GO

UPDATE m SET
    amt = ISNULL(s.amt, 0),
    vat = ISNULL(s.vat, 0),
    total_amt = ISNULL(s.total_amt, 0)
FROM TMAPOREQM m
    OUTER APPLY (
        SELECT SUM(d.amt) AS amt, SUM(d.vat) AS vat, SUM(d.total_amt) AS total_amt
        FROM TMAPOREQD d WHERE d.req_id = m.req_id
    ) s;
GO

CREATE OR ALTER PROCEDURE USP_MA_POREQ_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_req_no VARCHAR(20) = NULL,
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
            DECLARE @match_req_id BIGINT;
            SELECT TOP 1 @match_req_id = req_id
            FROM TMAPOREQM
            WHERE (@p_req_id IS NULL OR req_id = @p_req_id)
              AND (@p_req_id IS NOT NULL OR @p_req_no IS NULL OR req_no LIKE '%' + @p_req_no + '%')
            ORDER BY req_id DESC;

            -- 0) 헤더
            SELECT
                m.req_id, m.acc_id, a.ACC_NM,
                m.req_no, m.req_date, m.req_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.amt, m.vat, m.total_amt,
                m.remark
            FROM TMAPOREQM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.req_id = @match_req_id;

            -- 1) 품목 행
            SELECT
                dt.req_id, dt.serl, dt.acc_id, dt.req_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.amt, dt.vat_rate, dt.vat, dt.total_amt,
                dt.cfm_yn, dt.stop_yn,
                dt.cust_id, c2.cust_nm,
                dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.remark
            FROM TMAPOREQD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBACUST c2 ON c2.cust_id = dt.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.req_id = @match_req_id
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

CREATE OR ALTER PROCEDURE USP_MA_POREQ_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,   /* 무시됨 - 서버가 USP_MA_NEXTQTY_R로 계산 */
    @p_unit_cd VARCHAR(10) = NULL,
    @p_price NUMERIC(18,6) = NULL,
    @p_amt NUMERIC(18,6) = NULL,
    @p_vat_rate NUMERIC(18,6) = NULL,
    @p_vat NUMERIC(18,6) = NULL,
    @p_total_amt NUMERIC(18,6) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_no VARCHAR(20) = NULL,
    @p_src_serl INT = NULL,
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
        IF @p_work_type IN ('U', 'D')
           AND EXISTS (SELECT 1 FROM TMAPOREQD WHERE req_id = @p_req_id AND serl = @p_serl AND ISNULL(next_qty, 0) > 0)
        BEGIN
            SET @ReturnCode = -1;
            SET @ReturnMsg = N'발주에 사용된 구매요청 품목은 수정하거나 삭제할 수 없습니다.';
            RETURN;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAPOREQD WHERE req_id = @p_req_id;

            INSERT INTO TMAPOREQD (
                req_id, serl, acc_id, req_no, item_id, qty, next_qty, unit_cd,
                price, amt, vat_rate, vat, total_amt,
                cust_id, delv_date, wh_id, loc_id, src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.req_id, @nextSerl, m.acc_id, m.req_no, @p_item_id, @p_qty, 0, @p_unit_cd,
                   @p_price, @p_amt, @p_vat_rate, @p_vat, @p_total_amt,
                   @p_cust_id, @p_delv_date, @p_wh_id, @p_loc_id, @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAPOREQM m WHERE m.req_id = @p_req_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOREQD SET
                item_id = @p_item_id,
                qty = @p_qty,
                unit_cd = @p_unit_cd,
                price = @p_price,
                amt = @p_amt,
                vat_rate = @p_vat_rate,
                vat = @p_vat,
                total_amt = @p_total_amt,
                cust_id = @p_cust_id,
                delv_date = @p_delv_date,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                src_type = @p_src_type,
                src_id = @p_src_id,
                src_no = @p_src_no,
                src_serl = @p_src_serl,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE req_id = @p_req_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAPOREQD WHERE req_id = @p_req_id AND serl = @p_serl;
        END


        -- 헤더(TMAPOREQM)의 금액 합계를 품목(TMAPOREQD) 합계로 매번 다시 맞춘다 - 등록/수정/삭제 어느 쪽이든 이 프로시저를 거치므로
        -- 헤더 금액이 품목과 어긋날 일이 없다(품목이 하나도 안 남으면 0).
        UPDATE m SET
            amt = ISNULL(s.amt, 0),
            vat = ISNULL(s.vat, 0),
            total_amt = ISNULL(s.total_amt, 0)
        FROM TMAPOREQM m
            OUTER APPLY (
                SELECT SUM(d.amt) AS amt, SUM(d.vat) AS vat, SUM(d.total_amt) AS total_amt
                FROM TMAPOREQD d WHERE d.req_id = m.req_id
            ) s
        WHERE m.req_id = @p_req_id;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
