-- TBAITEM에서 prod_yn/po_yn/sale_yn/po_acnt_cd/sale_acnt_cd 컬럼이 삭제됨(2026-09-15, 사장님
-- 확인 - "지운거야. 테이블도 수정할 예정이야"). USP_BA_ITEM_Q/S가 여전히 이 죽은 컬럼들을
-- 참조하고 있어서(SELECT/INSERT/UPDATE 전부) 지금 상태로는 조회/저장이 스키마 오류로 깨진다 -
-- 둘 다 그 컬럼들을 완전히 빼고 다시 만든다. frmItem.cs/Designer.cs 쪽 컨트롤도 같이 정리함
-- (매입계정/매출계정 입력칸+그리드컬럼 제거, 생산여부/발주대상/판매대상은 이미 화면에서
-- 빠져 있었음).

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    @p_item_no VARCHAR(100) = NULL,
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
                a.acc_id,
                a.item_id,
                a.item_no,
                a.item_nm,
                a.item_spec,
                a.unit_cd,
                a.po_unit_cd,
                a.wh_id,
                a.loc_id,
                a.safe_qty,
                a.dept_id,
                b.dept_nm,
                a.emp_id,
                c.emp_no,
                c.emp_nm,
                a.cust_id,
                d.cust_nm,
                a.asset_type,
                a.out_type,
                a.po_qc_yn,
                a.prod_qc_yn,
                a.lot_yn,
                a.stock_yn,
                a.stat_cd,
                a.grp1_id,
                a.grp2_id,
                a.grp3_id,
                a.grp4_id,
                a.remark
            FROM        TBAITEM as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
                        LEFT OUTER JOIN TBACUST as d on a.cust_id = d.cust_id
            WHERE       1 = 1
   --         AND         (@p_item_id IS NULL OR a.item_id = @p_item_id)
            AND         (@p_item_no IS NULL OR a.item_no LIKE '%' + @p_item_no + '%')
            AND         (@p_item_nm IS NULL OR a.item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY a.item_id;
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
                remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_id, @p_loc_id,
                @p_safe_qty, @p_dept_id, @p_emp_id, @p_cust_id, @p_asset_type, @p_out_type,
                @p_po_qc_yn, @p_prod_qc_yn, @p_lot_yn, @p_stock_yn,
                @p_stat_cd, @p_grp1_id, @p_grp2_id, @p_grp3_id, @p_grp4_id,
                @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_item_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_id = @p_wh_id, loc_id = @p_loc_id,
                safe_qty = @p_safe_qty, DEPT_ID = @p_dept_id,
                EMP_ID = COALESCE(@p_emp_id, EMP_ID),
                CUST_ID = COALESCE(@p_cust_id, CUST_ID),
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

        IF @p_work_type IN ('N', 'U') AND @p_unit_cd IS NOT NULL AND @p_po_unit_cd IS NOT NULL
            AND @p_unit_cd <> @p_po_unit_cd
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
