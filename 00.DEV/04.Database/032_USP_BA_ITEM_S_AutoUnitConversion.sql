-- 품목등록(frmItem) 저장 시, 기본단위(unit_cd)와 구매단위(po_unit_cd)가 다르게 등록되면
-- TBAITEMUNIT(품목단위환산)에 그 조합의 환산행을 자동으로 만들어준다. unit_cd를 fr_unit_cd
-- 기준으로 삼는다(요청 그대로). 수량은 일단 1:1로 넣어두고(비율을 알 방법이 없어 임시값),
-- 실제 환산비율은 grd2(TBAITEMUNIT 입력그리드)에서 사용자가 고친다 - 이미 같은 조합이
-- 있으면 다시 안 만든다(저장을 여러 번 눌러도 중복 안 생기게).

CREATE OR ALTER PROCEDURE USP_BA_ITEM_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환(025 참고) */
    @p_item_id BIGINT = NULL,	/* N(신규)일 때는 안 씀 - IDENTITY라 서버가 자동 채번 */
    @p_item_cd VARCHAR(100) = NULL,
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

            SET @p_item_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_cd = @p_item_cd, item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_cd = @p_wh_cd, loc_cd = @p_loc_cd,
                safe_qty = @p_safe_qty, dept_cd = @p_dept_cd, emp_no = @p_emp_no, prod_yn = @p_prod_yn,
                cust_cd = @p_cust_cd, asset_type = @p_asset_type, out_type = @p_out_type,
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

        -- 기본단위와 구매단위가 다르면 그 조합의 환산행을 자동으로 만든다(unit_cd가 기준=fr_unit_cd).
        -- 이미 있으면 건드리지 않는다 - 사용자가 grd2에서 고친 환산비율(fr_qty/to_qty)을
        -- 저장을 다시 누를 때마다 1:1로 되돌리면 안 되기 때문이다.
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
