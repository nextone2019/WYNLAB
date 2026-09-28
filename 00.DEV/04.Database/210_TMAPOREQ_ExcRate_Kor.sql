-- 구매요청에 환율/원화(KOR_) 컬럼 추가(2026-09-25) - 발주(TMAPOM/TMAPOD)와 같은 구조.
--  * 헤더 TMAPOREQM.exc_rate(환율, 기본 1) - 통화가 KRW가 아니면 사용자가 입력한다.
--  * 품목 TMAPOREQD.kor_price/kor_amt/kor_vat/kor_total_amt(원화단가/공급가액/부가세/합계) - 화면이 외화 값 x 환율로 계산해서 보내고 서버는 저장만 한다.
-- 기존 데이터는 환율 1, 원화 = 기존 금액으로 채운다. 조회(USP_MA_POREQ_Q)/헤더 저장(USP_MA_POREQ_S)/품목 저장(USP_MA_POREQ_S_1)에 반영.
-- 각 프로시저는 라이브 정의에 해당 컬럼/파라미터만 더했다(207의 Q/S_1, USP_MA_POREQ_S 라이브 정의 기준).

IF COL_LENGTH('TMAPOREQM', 'exc_rate') IS NULL ALTER TABLE TMAPOREQM ADD exc_rate NUMERIC(18,6) NULL;
IF COL_LENGTH('TMAPOREQD', 'kor_price') IS NULL ALTER TABLE TMAPOREQD ADD kor_price NUMERIC(18,6) NULL;
IF COL_LENGTH('TMAPOREQD', 'kor_amt') IS NULL ALTER TABLE TMAPOREQD ADD kor_amt NUMERIC(18,6) NULL;
IF COL_LENGTH('TMAPOREQD', 'kor_vat') IS NULL ALTER TABLE TMAPOREQD ADD kor_vat NUMERIC(18,6) NULL;
IF COL_LENGTH('TMAPOREQD', 'kor_total_amt') IS NULL ALTER TABLE TMAPOREQD ADD kor_total_amt NUMERIC(18,6) NULL;
GO

UPDATE TMAPOREQM SET exc_rate = 1 WHERE exc_rate IS NULL;
UPDATE TMAPOREQD SET kor_price = price, kor_amt = amt, kor_vat = vat, kor_total_amt = total_amt
 WHERE kor_price IS NULL AND kor_amt IS NULL AND kor_vat IS NULL AND kor_total_amt IS NULL;
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
                m.pjt_id, m.cur_cd, m.exc_rate,
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
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
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
    @p_kor_price NUMERIC(18,6) = NULL,
    @p_kor_amt NUMERIC(18,6) = NULL,
    @p_kor_vat NUMERIC(18,6) = NULL,
    @p_kor_total_amt NUMERIC(18,6) = NULL,
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
                price, amt, vat_rate, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                cust_id, delv_date, wh_id, loc_id, src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.req_id, @nextSerl, m.acc_id, m.req_no, @p_item_id, @p_qty, 0, @p_unit_cd,
                   @p_price, @p_amt, @p_vat_rate, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
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
                kor_price = @p_kor_price,
                kor_amt = @p_kor_amt,
                kor_vat = @p_kor_vat,
                kor_total_amt = @p_kor_total_amt,
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

CREATE OR ALTER PROCEDURE USP_MA_POREQ_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_req_date VARCHAR(8) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
    @p_po_type VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(20) = NULL,
    @p_exc_rate NUMERIC(18,6) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_req_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAPOREQM', 'req_no', @p_acc_id, @new_req_no OUTPUT;

            INSERT INTO TMAPOREQM (
                acc_id, req_no, req_date, req_title, stat_cd, po_type,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_req_no, @p_req_date, @p_req_title, '0', @p_po_type,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_req_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOREQM SET
                acc_id = @p_acc_id,
                req_date = @p_req_date,
                req_title = @p_req_title,
                po_type = @p_po_type,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                exc_rate = @p_exc_rate,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TMAPOREQM WHERE req_id = @p_req_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 구매요청은 삭제할 수 없습니다.';
                RETURN;
            END

            IF EXISTS (SELECT 1 FROM TMAPOREQD WHERE req_id = @p_req_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'발주에 사용된 품목이 있는 구매요청은 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TMAPOREQD WHERE req_id = @p_req_id;
            DELETE FROM TMAPOREQM WHERE req_id = @p_req_id;
        END

        SET @GeneratedCode = CAST(@p_req_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
