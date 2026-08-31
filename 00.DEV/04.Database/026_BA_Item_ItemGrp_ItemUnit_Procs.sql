/* =========================================================
   BA(기준정보) 모듈 - 025(부서/사원/거래처)에 이어 품목 쪽 세 테이블의 _Q/_S 프로시저.
   USP_SM_MINORCODE_Q/_S 형식 그대로(025 상단 주석 참고).

   TBAITEMUNIT은 아직 화면이 없다(BA_ITEMGRP 같은 메뉴 연결 없이 프로시저만 먼저 만든다) -
   나중에 품목단위환산 화면이 생기면 그때 메뉴/화면을 붙이면 된다.

   TBAITEM의 photo/photo_file_nm/photo_path(사진)는 025의 TBAEMP와 같은 이유로 뺐다 - 업로드
   UI가 생기면 그때 추가.
   ========================================================= */

-- ===================== BA_ITEM (품목등록, frmItem) =====================

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
                acc_cd, item_cd, item_no, item_nm, item_spec, unit_cd, po_unit_cd, wh_cd, loc_cd,
                safe_qty, dept_cd, emp_no, prod_yn, cust_cd, asset_type, out_type,
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
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환(025 참고) */
    @p_item_cd VARCHAR(100),
    @p_item_no NVARCHAR(200) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    @p_item_spec NVARCHAR(100) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_po_unit_cd VARCHAR(10) = NULL,
    @p_wh_cd VARCHAR(20) = NULL,
    @p_loc_cd VARCHAR(20) = NULL,
    @p_safe_qty NUMERIC(18, 5) = NULL,
    @p_dept_cd VARCHAR(20) = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_prod_yn VARCHAR(1) = NULL,
    @p_cust_cd VARCHAR(20) = NULL,
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
                safe_qty, dept_cd, emp_no, prod_yn, cust_cd, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn, po_price, sale_yn, sale_price,
                stat_cd, item_class1, item_class2, item_class3, item_class4,
                po_acnt_cd, sale_acnt_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_item_cd, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_cd, @p_loc_cd,
                @p_safe_qty, @p_dept_cd, @p_emp_no, @p_prod_yn, @p_cust_cd, @p_asset_type, @p_out_type,
                @p_po_qc_yn, @p_prod_qc_yn, @p_lot_yn, @p_stock_yn, @p_po_yn, @p_po_price, @p_sale_yn, @p_sale_price,
                @p_stat_cd, @p_item_class1, @p_item_class2, @p_item_class3, @p_item_class4,
                @p_po_acnt_cd, @p_sale_acnt_cd, @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_cd = @p_wh_cd, loc_cd = @p_loc_cd,
                safe_qty = @p_safe_qty, dept_cd = @p_dept_cd, emp_no = @p_emp_no, prod_yn = @p_prod_yn,
                cust_cd = @p_cust_cd, asset_type = @p_asset_type, out_type = @p_out_type,
                po_qc_yn = @p_po_qc_yn, prod_qc_yn = @p_prod_qc_yn, lot_yn = @p_lot_yn, stock_yn = @p_stock_yn,
                po_yn = @p_po_yn, po_price = @p_po_price, sale_yn = @p_sale_yn, sale_price = @p_sale_price,
                stat_cd = @p_stat_cd, item_class1 = @p_item_class1, item_class2 = @p_item_class2,
                item_class3 = @p_item_class3, item_class4 = @p_item_class4,
                po_acnt_cd = @p_po_acnt_cd, sale_acnt_cd = @p_sale_acnt_cd, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE item_cd = @p_item_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEM WHERE item_cd = @p_item_cd;
        END

        SET @GeneratedCode = @p_item_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ===================== BA_ITEMGRP (품목그룹등록, frmItemGrp) =====================

CREATE OR ALTER PROCEDURE USP_BA_ITEMGRP_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_class_cd VARCHAR(10) = NULL,
    @p_item_class_nm NVARCHAR(30) = NULL,
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
                acc_cd, item_lvl, item_class_cd, item_class_nm, par_item_class_cd, remark
            FROM TBAITEMGRP
            WHERE (@p_item_class_cd IS NULL OR item_class_cd LIKE '%' + @p_item_class_cd + '%')
              AND (@p_item_class_nm IS NULL OR item_class_nm LIKE '%' + @p_item_class_nm + '%')
            ORDER BY item_class_cd;
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

CREATE OR ALTER PROCEDURE USP_BA_ITEMGRP_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환(025 참고) */
    @p_item_lvl VARCHAR(10),
    @p_item_class_cd VARCHAR(10),
    @p_item_class_nm NVARCHAR(30) = NULL,
    @p_par_item_class_cd VARCHAR(10) = NULL,
    @p_remark NVARCHAR(1000) = NULL,
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
            INSERT INTO TBAITEMGRP (
                acc_cd, item_lvl, item_class_cd, item_class_nm, par_item_class_cd, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_item_lvl, @p_item_class_cd, @p_item_class_nm, @p_par_item_class_cd, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMGRP SET
                item_lvl = @p_item_lvl, item_class_nm = @p_item_class_nm,
                par_item_class_cd = @p_par_item_class_cd, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE item_class_cd = @p_item_class_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEMGRP WHERE item_class_cd = @p_item_class_cd;
        END

        SET @GeneratedCode = @p_item_class_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ===================== BA_ITEMUNIT (품목단위환산, 화면 아직 없음) =====================
-- TBAITEMUNIT.item_cd는 TBAITEM.item_cd(varchar)와 타입이 다른 bigint다 - 그대로 따른다
-- (내부 품목ID로 추정, 검증/확인은 화면을 붙일 때 다시 볼 것).

CREATE OR ALTER PROCEDURE USP_BA_ITEMUNIT_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_cd BIGINT = NULL,
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
                acc_cd, item_cd, fr_unit_cd, fr_qty, to_unit_cd, to_qty, remark
            FROM TBAITEMUNIT
            WHERE @p_item_cd IS NULL OR item_cd = @p_item_cd
            ORDER BY item_cd, fr_unit_cd, to_unit_cd;
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

CREATE OR ALTER PROCEDURE USP_BA_ITEMUNIT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환(025 참고) */
    @p_item_cd BIGINT,
    @p_fr_unit_cd VARCHAR(10),
    @p_fr_qty NUMERIC(19, 5) = NULL,
    @p_to_unit_cd VARCHAR(10),
    @p_to_qty NUMERIC(19, 5) = NULL,
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
            INSERT INTO TBAITEMUNIT (
                acc_cd, item_cd, fr_unit_cd, fr_qty, to_unit_cd, to_qty, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_item_cd, @p_fr_unit_cd, @p_fr_qty, @p_to_unit_cd, @p_to_qty, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMUNIT SET
                fr_qty = @p_fr_qty, to_qty = @p_to_qty, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE item_cd = @p_item_cd AND fr_unit_cd = @p_fr_unit_cd AND to_unit_cd = @p_to_unit_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEMUNIT
            WHERE item_cd = @p_item_cd AND fr_unit_cd = @p_fr_unit_cd AND to_unit_cd = @p_to_unit_cd;
        END

        SET @GeneratedCode = CAST(@p_item_cd AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
