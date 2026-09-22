-- TBAITEM 컬럼 변경(사장님이 테이블에서 직접 적용, 마이그레이션 파일로 남아있지 않음)을
-- USP_BA_ITEM_Q/USP_BA_ITEM_S에 반영한다:
--   - item_cd 컬럼 자체가 삭제됨(더 이상 존재하지 않음)
--   - wh_cd(VARCHAR) -> wh_id(BIGINT), loc_cd(VARCHAR) -> loc_id(BIGINT)
--   - item_class1~4 -> item_grp1~4 (이름만 변경, 타입/길이는 VARCHAR(20) 그대로)
-- 두 프로시저는 지금 테이블에 없는 컬럼(item_cd/wh_cd/loc_cd/item_class1~4)을 그대로 참조하고
-- 있어서 호출하면 바로 "잘못된 열 이름" 오류가 난다 - 화면 수정과 함께 반드시 같이 반영해야 한다.
--
-- 겸사겸사 USP_BA_ITEM_Q의 내부 불일치도 같이 정리한다: @p_item_id가 필수 파라미터로 추가돼
-- 있었는데 WHERE절에서 전혀 안 쓰이고(죽은 파라미터), ORDER BY는 여전히 없는 컬럼(item_cd)을
-- 가리키고 있었다 - @p_item_id를 선택 파라미터의 정확히 일치 검색으로 실제로 쓰게 하고,
-- ORDER BY는 item_id 기준으로 바꾼다.
--
-- emp_id/prod_yn/cust_id는 화면(frmItem)에서 입력 컨트롤을 뺐지만(2026-09-07) 컬럼 자체는
-- 테이블에 남아있다 - 그대로 두면 이 화면에서 저장할 때마다(U) 그 세 컬럼이 매번 NULL로
-- 덮어써진다(파라미터를 안 보내면 프로시저 기본값 NULL이 그대로 UPDATE에 쓰이므로). COALESCE로
-- "값이 안 오면 기존 값 유지"하도록 바꿔서, 이 화면이 그 컬럼들을 더는 건드리지 않게 한다.

CREATE OR ALTER PROCEDURE USP_BA_ITEM_Q
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
                a.acc_cd,
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
                a.prod_yn,
                a.cust_id,
                d.cust_nm,
                a.asset_type,
                a.out_type,
                a.po_qc_yn,
                a.prod_qc_yn,
                a.lot_yn,
                a.stock_yn,
                a.po_yn,
                a.po_price,
                a.sale_yn,
                a.sale_price,
                a.stat_cd,
                a.item_grp1,
                a.item_grp2,
                a.item_grp3,
                a.item_grp4,
                a.po_acnt_cd,
                a.sale_acnt_cd,
                a.remark
            FROM        TBAITEM as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
                        LEFT OUTER JOIN TBACUST as d on a.cust_id = d.cust_id
            WHERE       1 = 1
            AND         (@p_item_id IS NULL OR a.item_id = @p_item_id)
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

CREATE OR ALTER PROCEDURE USP_BA_ITEM_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',
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
    @p_item_grp1 VARCHAR(20) = NULL,
    @p_item_grp2 VARCHAR(20) = NULL,
    @p_item_grp3 VARCHAR(20) = NULL,
    @p_item_grp4 VARCHAR(20) = NULL,
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
                acc_cd, item_no, item_nm, item_spec, unit_cd, po_unit_cd, wh_id, loc_id,
                safe_qty, DEPT_ID, EMP_ID, prod_yn, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn, po_price, sale_yn, sale_price,
                stat_cd, item_grp1, item_grp2, item_grp3, item_grp4,
                po_acnt_cd, sale_acnt_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_id, @p_loc_id,
                @p_safe_qty, @p_dept_id, @p_emp_id, @p_prod_yn, @p_cust_id, @p_asset_type, @p_out_type,
                @p_po_qc_yn, @p_prod_qc_yn, @p_lot_yn, @p_stock_yn, @p_po_yn, @p_po_price, @p_sale_yn, @p_sale_price,
                @p_stat_cd, @p_item_grp1, @p_item_grp2, @p_item_grp3, @p_item_grp4,
                @p_po_acnt_cd, @p_sale_acnt_cd, @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_item_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_id = @p_wh_id, loc_id = @p_loc_id,
                safe_qty = @p_safe_qty, DEPT_ID = @p_dept_id,
                -- emp_id/prod_yn/cust_id는 frmItem에 입력 컨트롤이 없어져서(2026-09-07) 항상 NULL로
                -- 넘어온다 - COALESCE로 값이 안 오면 기존 값을 그대로 둔다(다른 화면/경로가 나중에
                -- 이 컬럼들을 관리하게 되더라도 이 화면이 매번 NULL로 지워버리지 않도록).
                EMP_ID = COALESCE(@p_emp_id, EMP_ID),
                prod_yn = COALESCE(@p_prod_yn, prod_yn),
                CUST_ID = COALESCE(@p_cust_id, CUST_ID),
                asset_type = @p_asset_type, out_type = @p_out_type,
                po_qc_yn = @p_po_qc_yn, prod_qc_yn = @p_prod_qc_yn, lot_yn = @p_lot_yn, stock_yn = @p_stock_yn,
                po_yn = @p_po_yn, po_price = @p_po_price, sale_yn = @p_sale_yn, sale_price = @p_sale_price,
                stat_cd = @p_stat_cd, item_grp1 = @p_item_grp1, item_grp2 = @p_item_grp2,
                item_grp3 = @p_item_grp3, item_grp4 = @p_item_grp4,
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
                WHERE item_id = @p_item_id AND fr_unit_cd = @p_unit_cd AND to_unit_cd = @p_po_unit_cd
            )
            BEGIN
                INSERT INTO TBAITEMUNIT (acc_cd, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, reg_user_id, reg_dt, reg_pc)
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
