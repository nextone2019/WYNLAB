-- 구매발주등록(frmPoOrder, Master-One Sheet) + 구매발주현황(frmPoOrderList, Master-SubGrid)
-- 프로시저 (2026-09-22) - 170번(구매요청)과 완전히 같은 구조, 대상 테이블만 TMAPOM/TMAPOD.
-- 품목 상세에 단가/금액/부가세 계산 컬럼이 추가된다(발주는 확정 가격이 필요, 요청 단계엔 없음).

-- ============================================================
-- 1) USP_MA_PO_Q - frmPoOrder 전용.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_po_no VARCHAR(20) = NULL,
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
            DECLARE @match_po_id BIGINT;
            SELECT TOP 1 @match_po_id = po_id
            FROM TMAPOM
            WHERE (@p_po_id IS NULL OR po_id = @p_po_id)
              AND (@p_po_id IS NOT NULL OR @p_po_no IS NULL OR po_no LIKE '%' + @p_po_no + '%')
            ORDER BY po_id DESC;

            -- 0) 헤더
            SELECT
                m.po_id, m.acc_id, a.ACC_NM,
                m.po_no, m.po_date,
                m.stat_cd, m.po_type, m.po_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.delv_date, m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cmf_user_id,
                m.app_id, m.app_no, t.stat_cd AS appr_stat_cd,
                m.remark
            FROM TMAPOM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.po_id = @match_po_id;

            -- 1) 품목 상세
            SELECT
                dt.po_id, dt.serl, dt.acc_id, dt.po_no, dt.po_type,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.qc_yn, dt.stock_yn, dt.stock_unit_cd, dt.stock_unit_qty,
                dt.pjt_id, dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.stop_yn, dt.stop_emp_no, dt.stop_remark, dt.remark
            FROM TMAPOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.po_id = @match_po_id
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

-- ============================================================
-- 2) USP_MA_PO_S - 헤더(TMAPOM) N/U/D. po_no 자동발번(PO+yymmdd+3자리), 결재상신된 건 삭제 금지.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_po_date VARCHAR(8) = NULL,
    @p_po_type VARCHAR(10) = NULL,
    @p_po_title NVARCHAR(1000) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(10) = NULL,
    @p_exc_rate NUMERIC(18,4) = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            -- 채번을 공용 프로시저로 위임(2026-09-22, 자체 날짜+순번 계산 대신) - 173번
            -- 마이그레이션에서 TMAPOM/po_no에 prefix='PO' 설정을 미리 심어둔다.
            DECLARE @new_po_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAPOM', 'po_no', @p_acc_id, @new_po_no OUTPUT;

            INSERT INTO TMAPOM (
                acc_id, po_no, po_date, stat_cd, po_type, po_title,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate,
                delv_date, vat_type, vat_rate, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @new_po_no, @p_po_date, '0', @p_po_type, @p_po_title,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate,
                @p_delv_date, @p_vat_type, @p_vat_rate, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_po_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOM SET
                acc_id = @p_acc_id,
                po_date = @p_po_date,
                po_type = @p_po_type,
                po_title = @p_po_title,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                exc_rate = @p_exc_rate,
                delv_date = @p_delv_date,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE po_id = @p_po_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TMAPOM WHERE po_id = @p_po_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 구매발주는 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TMAPOD WHERE po_id = @p_po_id;
            DELETE FROM TMAPOM WHERE po_id = @p_po_id;
        END

        SET @GeneratedCode = CAST(@p_po_id AS VARCHAR(20));
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
-- 3) USP_MA_PO_S_1 - 품목(TMAPOD) 행별 N/U/D. amt/vat/total_amt는 화면에서 계산해 그대로
--    넘겨받는다(프로시저는 저장만 - USP_BA_ITEM류와 같이 계산로직을 서버에 안 둔다).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,
    @p_price NUMERIC(18,4) = NULL,
    @p_amt NUMERIC(18,4) = NULL,
    @p_vat NUMERIC(18,4) = NULL,
    @p_total_amt NUMERIC(18,4) = NULL,
    @p_kor_price NUMERIC(18,4) = NULL,
    @p_kor_amt NUMERIC(18,4) = NULL,
    @p_kor_vat NUMERIC(18,4) = NULL,
    @p_kor_total_amt NUMERIC(18,4) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(9,4) = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_qc_yn VARCHAR(1) = NULL,
    @p_stock_yn VARCHAR(1) = NULL,
    @p_stock_unit_cd VARCHAR(10) = NULL,
    @p_stock_unit_qty NUMERIC(18,4) = NULL,
    @p_pjt_id BIGINT = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAPOD WHERE po_id = @p_po_id;

            INSERT INTO TMAPOD (
                po_id, serl, acc_id, po_no, po_type, item_id, unit_cd, qty, next_qty,
                price, amt, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                vat_type, vat_rate, delv_date, wh_id, loc_id,
                qc_yn, stock_yn, stock_unit_cd, stock_unit_qty, pjt_id,
                src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT m.po_id, @nextSerl, m.acc_id, m.po_no, m.po_type, @p_item_id, @p_unit_cd, @p_qty, @p_next_qty,
                   @p_price, @p_amt, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
                   @p_vat_type, @p_vat_rate, @p_delv_date, @p_wh_id, @p_loc_id,
                   @p_qc_yn, @p_stock_yn, @p_stock_unit_cd, @p_stock_unit_qty, @p_pjt_id,
                   @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TMAPOM m WHERE m.po_id = @p_po_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOD SET
                item_id = @p_item_id,
                unit_cd = @p_unit_cd,
                qty = @p_qty,
                next_qty = @p_next_qty,
                price = @p_price,
                amt = @p_amt,
                vat = @p_vat,
                total_amt = @p_total_amt,
                kor_price = @p_kor_price,
                kor_amt = @p_kor_amt,
                kor_vat = @p_kor_vat,
                kor_total_amt = @p_kor_total_amt,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                delv_date = @p_delv_date,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                qc_yn = @p_qc_yn,
                stock_yn = @p_stock_yn,
                stock_unit_cd = @p_stock_unit_cd,
                stock_unit_qty = @p_stock_unit_qty,
                pjt_id = @p_pjt_id,
                src_type = @p_src_type,
                src_id = @p_src_id,
                src_no = @p_src_no,
                src_serl = @p_src_serl,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE po_id = @p_po_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAPOD WHERE po_id = @p_po_id AND serl = @p_serl;
        END

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

-- ============================================================
-- 4) USP_MA_POLIST_Q - frmPoOrderList(현황) 전용. Q=목록(grd1), Q1=선택건 품목상세(grd2).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_po_no VARCHAR(20) = NULL,
    @p_po_title NVARCHAR(1000) = NULL,
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
                m.po_id, m.po_no, m.po_date, m.po_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.stat_cd AS appr_stat_cd
            FROM TMAPOM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE 1 = 1
              AND (@p_po_no IS NULL OR m.po_no LIKE '%' + @p_po_no + '%')
              AND (@p_po_title IS NULL OR m.po_title LIKE '%' + @p_po_title + '%')
            ORDER BY m.po_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.po_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.amt, dt.total_amt,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TMAPOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
            WHERE dt.po_id = @p_po_id
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
