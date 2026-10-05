-- 281: USP_BA_ITEM_S(U) 담당자/구매처를 빈 값으로도 저장(해제)할 수 있게 한다 (2026-10-04, 품목일괄수정 요청).
--   기존에는 EMP_ID/CUST_ID가 COALESCE(@p_x, 기존값)이라 비워도 이전 값이 남았다. 호출 화면(frmItem, frmItemMod)은 항상 두 값을 보내므로(비면 NULL) 그대로 대입해도 안전하다.

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_item_no NVARCHAR(200) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    @p_item_spec NVARCHAR(100) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_po_unit_cd VARCHAR(10) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_safe_qty NUMERIC(18, 5) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_cust_id BIGINT = NULL,
    @p_asset_type VARCHAR(10) = NULL,
    @p_out_type VARCHAR(10) = NULL,
    @p_po_qc_yn VARCHAR(1) = NULL,
    @p_prod_qc_yn VARCHAR(1) = NULL,
    @p_lot_yn VARCHAR(1) = NULL,
    @p_stock_yn VARCHAR(1) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_grp1_id BIGINT = NULL,
    @p_grp2_id BIGINT = NULL,
    @p_grp3_id BIGINT = NULL,
    @p_grp4_id BIGINT = NULL,
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
                        acc_id,             item_no,            item_nm,                item_spec,
                        unit_cd,            po_unit_cd,         wh_id, loc_id,
                safe_qty, DEPT_ID, EMP_ID, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn,
                stat_cd, grp1_id, grp2_id, grp3_id, grp4_id,
                remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_id, @p_loc_id,
                @p_safe_qty, @p_dept_id, @p_emp_id, @p_cust_id, @p_asset_type, @p_out_type,
                @p_po_qc_yn, @p_prod_qc_yn, @p_lot_yn, @p_stock_yn,
                @p_stat_cd, @p_grp1_id, @p_grp2_id, @p_grp3_id, @p_grp4_id,
                @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_item_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_id = @p_wh_id, loc_id = @p_loc_id,
                safe_qty = @p_safe_qty, DEPT_ID = @p_dept_id,
                EMP_ID = @p_emp_id,
                CUST_ID = @p_cust_id,
                asset_type = @p_asset_type, out_type = @p_out_type,
                po_qc_yn = @p_po_qc_yn,
                prod_qc_yn = @p_prod_qc_yn,
                lot_yn = @p_lot_yn,
                stock_yn = @p_stock_yn,
                stat_cd = @p_stat_cd,
                grp1_id = @p_grp1_id,
                grp2_id = @p_grp2_id,
                grp3_id = @p_grp3_id,
                grp4_id = @p_grp4_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE item_id = @p_item_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEM WHERE item_id = @p_item_id;
        END

        IF @p_work_type IN ('N', 'U') AND  ISNULL(@p_unit_cd,'') <> ISNULL(@p_po_unit_cd,'') AND (ISNULL(@p_unit_cd,'') >'' AND ISNULL(@p_po_unit_cd,'') > '')
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM TBAITEMUNIT
                WHERE item_id = @p_item_id AND fr_unit_cd = @p_unit_cd AND to_unit_cd = @p_po_unit_cd
            )
            BEGIN
                INSERT INTO TBAITEMUNIT (acc_id, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, reg_user_id, reg_dt, reg_pc)
                VALUES (@p_acc_id, @p_item_id, @p_unit_cd, 1, @p_po_unit_cd, 1, @p_user_id, GETDATE(), @p_client_pc);
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
