/* ---------- CUST_CD -> CUST_ID 전환(084)에 맞춰 USP_BA_CUST_Q/_S/_S_1/_S_2 및 USP_BA_ITEM_Q/_S 수정 ----------
   P_CUST 팝업은 만들지 않는다(사장님이 frmCust/frmItem에 조건항목을 별도로 추가하겠다고 함) -
   여기서는 DB/프로시저 계층만 CUST_ID 기준으로 정리한다. */

CREATE OR ALTER PROCEDURE USP_BA_CUST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
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
                CUST_ID, cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2,
                homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate,
                remark, stat_cd, emp_no
            FROM TBACUST
            WHERE (@p_cust_id IS NULL OR CUST_ID = @p_cust_id)
              AND (@p_cust_nm IS NULL OR cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY CUST_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT CUST_ID, serl, prsn_nm, grade, tel1, tel2, fax, email
            FROM TBACUSTPRSN
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;

            SELECT CUST_ID, serl, bank_cd, acnt_no, remark
            FROM TBACUSTACNT
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;
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

CREATE OR ALTER PROCEDURE USP_BA_CUST_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT = NULL,	/* N(신규)일 때는 안 씀 - IDENTITY라 서버가 자동 채번 */
    @p_cust_nm NVARCHAR(100) = NULL,
    @p_biz_no VARCHAR(30) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_cur_cd VARCHAR(3) = NULL,
    @p_owner_nm NVARCHAR(100) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(1000) = NULL,
    @p_addr2 NVARCHAR(1000) = NULL,
    @p_homepage NVARCHAR(200) = NULL,
    @p_email NVARCHAR(100) = NULL,
    @p_fax VARCHAR(30) = NULL,
    @p_biz_kind NVARCHAR(200) = NULL,
    @p_biz_type NVARCHAR(200) = NULL,
    @p_trans_open_date VARCHAR(8) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(19, 2) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_emp_no VARCHAR(20) = NULL,
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
            INSERT INTO TBACUST (
                cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2,
                homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate,
                remark, stat_cd, emp_no, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_cust_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_zip_code, @p_addr1, @p_addr2,
                @p_homepage, @p_email, @p_fax, @p_biz_kind, @p_biz_type, @p_trans_open_date, @p_vat_type, @p_vat_rate,
                @p_remark, @p_stat_cd, @p_emp_no, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_cust_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUST SET
                cust_nm = @p_cust_nm, biz_no = @p_biz_no, tel = @p_tel, cur_cd = @p_cur_cd,
                owner_nm = @p_owner_nm, zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                homepage = @p_homepage, email = @p_email, fax = @p_fax, biz_kind = @p_biz_kind,
                biz_type = @p_biz_type, trans_open_date = @p_trans_open_date, vat_type = @p_vat_type,
                vat_rate = @p_vat_rate, remark = @p_remark, stat_cd = @p_stat_cd, emp_no = @p_emp_no,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE CUST_ID = @p_cust_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUST WHERE CUST_ID = @p_cust_id;
        END

        SET @GeneratedCode = CAST(@p_cust_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_BA_CUST_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_serl INT = NULL,
    @p_prsn_nm NVARCHAR(100) = NULL,
    @p_grade NVARCHAR(100) = NULL,
    @p_tel1 VARCHAR(30) = NULL,
    @p_tel2 VARCHAR(30) = NULL,
    @p_fax VARCHAR(30) = NULL,
    @p_email VARCHAR(30) = NULL,
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
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TBACUSTPRSN WHERE CUST_ID = @p_cust_id;

            INSERT INTO TBACUSTPRSN (CUST_ID, serl, prsn_nm, grade, tel1, tel2, fax, email, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_cust_id, @p_serl, @p_prsn_nm, @p_grade, @p_tel1, @p_tel2, @p_fax, @p_email, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUSTPRSN SET
                prsn_nm = @p_prsn_nm, grade = @p_grade, tel1 = @p_tel1, tel2 = @p_tel2,
                fax = @p_fax, email = @p_email, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUSTPRSN WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
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

CREATE OR ALTER PROCEDURE USP_BA_CUST_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_serl INT = NULL,
    @p_bank_cd VARCHAR(20) = NULL,
    @p_acnt_no VARCHAR(50) = NULL,
    @p_remark NVARCHAR(400) = NULL,
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
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TBACUSTACNT WHERE CUST_ID = @p_cust_id;

            INSERT INTO TBACUSTACNT (CUST_ID, serl, bank_cd, acnt_no, remark, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_cust_id, @p_serl, @p_bank_cd, @p_acnt_no, @p_remark, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUSTACNT SET
                bank_cd = @p_bank_cd, acnt_no = @p_acnt_no, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUSTACNT WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
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

-- ===================== USP_BA_ITEM_Q/_S: cust_cd -> CUST_ID =====================

CREATE OR ALTER PROCEDURE USP_BA_ITEM_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_cd VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
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
                acc_cd, item_id, item_cd, item_no, item_nm, item_spec, unit_cd, po_unit_cd, wh_cd, loc_cd,
                safe_qty, DEPT_ID, emp_no, prod_yn, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn, po_price, sale_yn, sale_price,
                stat_cd, item_class1, item_class2, item_class3, item_class4,
                po_acnt_cd, sale_acnt_cd, remark
            FROM TBAITEM
            WHERE (@p_item_cd IS NULL OR item_cd LIKE '%' + @p_item_cd + '%')
              AND (@p_item_nm IS NULL OR item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY item_cd;
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

CREATE OR ALTER PROCEDURE USP_BA_ITEM_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',
    @p_item_id BIGINT = NULL,
    @p_item_cd VARCHAR(100) = NULL,
    @p_item_no NVARCHAR(200) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    @p_item_spec NVARCHAR(100) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_po_unit_cd VARCHAR(10) = NULL,
    @p_wh_cd VARCHAR(20) = NULL,
    @p_loc_cd VARCHAR(20) = NULL,
    @p_safe_qty NUMERIC(18, 5) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_prod_yn VARCHAR(1) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_asset_type VARCHAR(10) = NULL,
    @p_out_type VARCHAR(10) = NULL,
    @p_po_qc_yn VARCHAR(1) = NULL,
    @p_prod_qc_yn VARCHAR(1) = NULL,
    @p_lot_yn VARCHAR(1) = NULL,
    @p_stock_yn VARCHAR(1) = NULL,
    @p_po_yn VARCHAR(1) = NULL,
    @p_po_price NUMERIC(18, 5) = NULL,
    @p_sale_yn VARCHAR(1) = NULL,
    @p_sale_price NUMERIC(18, 5) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_item_class1 VARCHAR(20) = NULL,
    @p_item_class2 VARCHAR(20) = NULL,
    @p_item_class3 VARCHAR(20) = NULL,
    @p_item_class4 VARCHAR(20) = NULL,
    @p_po_acnt_cd VARCHAR(20) = NULL,
    @p_sale_acnt_cd VARCHAR(20) = NULL,
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
            INSERT INTO TBAITEM (
                acc_cd, item_cd, item_no, item_nm, item_spec, unit_cd, po_unit_cd, wh_cd, loc_cd,
                safe_qty, DEPT_ID, emp_no, prod_yn, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn, po_price, sale_yn, sale_price,
                stat_cd, item_class1, item_class2, item_class3, item_class4,
                po_acnt_cd, sale_acnt_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_item_cd, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_cd, @p_loc_cd,
                @p_safe_qty, @p_dept_id, @p_emp_no, @p_prod_yn, @p_cust_id, @p_asset_type, @p_out_type,
                @p_po_qc_yn, @p_prod_qc_yn, @p_lot_yn, @p_stock_yn, @p_po_yn, @p_po_price, @p_sale_yn, @p_sale_price,
                @p_stat_cd, @p_item_class1, @p_item_class2, @p_item_class3, @p_item_class4,
                @p_po_acnt_cd, @p_sale_acnt_cd, @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_item_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_cd = @p_item_cd, item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_cd = @p_wh_cd, loc_cd = @p_loc_cd,
                safe_qty = @p_safe_qty, DEPT_ID = @p_dept_id, emp_no = @p_emp_no, prod_yn = @p_prod_yn,
                CUST_ID = @p_cust_id, asset_type = @p_asset_type, out_type = @p_out_type,
                po_qc_yn = @p_po_qc_yn, prod_qc_yn = @p_prod_qc_yn, lot_yn = @p_lot_yn, stock_yn = @p_stock_yn,
                po_yn = @p_po_yn, po_price = @p_po_price, sale_yn = @p_sale_yn, sale_price = @p_sale_price,
                stat_cd = @p_stat_cd, item_class1 = @p_item_class1, item_class2 = @p_item_class2,
                item_class3 = @p_item_class3, item_class4 = @p_item_class4,
                po_acnt_cd = @p_po_acnt_cd, sale_acnt_cd = @p_sale_acnt_cd, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE item_id = @p_item_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEM WHERE item_id = @p_item_id;
        END

        IF @p_work_type IN ('N', 'U') AND @p_unit_cd IS NOT NULL AND @p_po_unit_cd IS NOT NULL
           AND @p_unit_cd <> @p_po_unit_cd
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM TBAITEMUNIT
                WHERE item_cd = @p_item_id AND fr_unit_cd = @p_unit_cd AND to_unit_cd = @p_po_unit_cd
            )
            BEGIN
                INSERT INTO TBAITEMUNIT (acc_cd, item_cd, fr_unit_cd, fr_qty, to_unit_cd, to_qty, reg_user_id, reg_dt, reg_pc)
                VALUES (@p_acc_cd, @p_item_id, @p_unit_cd, 1, @p_po_unit_cd, 1, @p_user_id, GETDATE(), @p_client_pc);
            END
        END

        SET @GeneratedCode = CAST(@p_item_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
