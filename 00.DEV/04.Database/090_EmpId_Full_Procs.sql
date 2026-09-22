/* ---------- emp_no -> EMP_ID 완전 전환(089)에 맞춰 관련 프로시저 전부 수정 ----------
   TBACUST/TBAITEM의 목록/리스트 조회는 TBAEMP를 EMP_ID로 조인해서 emp_nm을 같이 보여준다
   (읽기전용 그리드라 이름으로 보여주는 게 낫다 - DEPT_ID/frmItemList 때와 같은 논리).
   상세입력 필드(txtDetailEmpNo/txtEmpNo)는 여전히 원시 ID를 입력받는 텍스트박스 그대로 둔다
   (팝업 작업은 사장님이 frmCust/frmItem에 직접 하겠다고 함 - 여긴 DB 배관만 정리).
   USP_SM_USERAUTH_S는 이제 EMP_ID를 그대로 받아 저장한다(더는 emp_no로 TBAEMP를 찾아
   내부에서 변환할 필요 없음 - 화면이 팝업에서 고른 EMP_ID를 직접 넘겨준다). */

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
                a.CUST_ID, a.cust_nm, a.biz_no, a.tel, a.cur_cd, a.owner_nm, a.zip_code, a.addr1, a.addr2,
                a.homepage, a.email, a.fax, a.biz_kind, a.biz_type, a.trans_open_date, a.vat_type, a.vat_rate,
                a.remark, a.stat_cd, a.EMP_ID, b.emp_nm
            FROM TBACUST a
            LEFT JOIN TBAEMP b ON a.EMP_ID = b.EMP_ID
            WHERE (@p_cust_id IS NULL OR a.CUST_ID = @p_cust_id)
              AND (@p_cust_nm IS NULL OR a.cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY a.CUST_ID;
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
    @p_cust_id BIGINT = NULL,
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
    @p_emp_id BIGINT = NULL,
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
                remark, stat_cd, EMP_ID, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_cust_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_zip_code, @p_addr1, @p_addr2,
                @p_homepage, @p_email, @p_fax, @p_biz_kind, @p_biz_type, @p_trans_open_date, @p_vat_type, @p_vat_rate,
                @p_remark, @p_stat_cd, @p_emp_id, @p_user_id, GETDATE(), @p_client_pc
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
                vat_rate = @p_vat_rate, remark = @p_remark, stat_cd = @p_stat_cd, EMP_ID = @p_emp_id,
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
                safe_qty, DEPT_ID, EMP_ID, prod_yn, CUST_ID, asset_type, out_type,
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
                safe_qty, DEPT_ID, EMP_ID, prod_yn, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn, po_price, sale_yn, sale_price,
                stat_cd, item_class1, item_class2, item_class3, item_class4,
                po_acnt_cd, sale_acnt_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_item_cd, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_cd, @p_loc_cd,
                @p_safe_qty, @p_dept_id, @p_emp_id, @p_prod_yn, @p_cust_id, @p_asset_type, @p_out_type,
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
                safe_qty = @p_safe_qty, DEPT_ID = @p_dept_id, EMP_ID = @p_emp_id, prod_yn = @p_prod_yn,
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMLIST_Q]
    @p_work_type VARCHAR(50),
    @p_item_cd VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
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
                a.item_id, a.item_cd, a.item_nm, a.item_spec, a.unit_cd, a.wh_cd, a.loc_cd, a.safe_qty,
                a.DEPT_ID, b.dept_nm, a.EMP_ID, c.emp_nm, a.stock_yn, a.sale_yn, a.sale_price, a.po_yn, a.po_price, a.stat_cd, a.remark
            FROM TBAITEM a
            LEFT JOIN TBADEPT b ON a.DEPT_ID = b.DEPT_ID
            LEFT JOIN TBAEMP c ON a.EMP_ID = c.EMP_ID
            WHERE (@p_item_cd IS NULL OR a.item_cd LIKE '%' + @p_item_cd + '%')
              AND (@p_item_nm IS NULL OR a.item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY a.item_cd;
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

-- ===================== SM: USP_SM_USERAUTH_S가 EMP_ID를 직접 받아 저장 =====================

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_id BIGINT = NULL,
    @p_email NVARCHAR(200) = NULL,
    @p_use_yn CHAR(1) = 'Y',
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
            INSERT INTO TSMUSER
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_ID, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_id, @p_email, 'U', 'N', 'Y', 'N', 0, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_ID = @p_emp_id, EMAIL = @p_email, USE_YN = @p_use_yn,
                UPT_DT = GETDATE()
            WHERE USER_ID = @p_user_id;
        END

        SET @GeneratedCode = @p_user_id;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_user_nm NVARCHAR(50) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
            SELECT  U.USER_ID AS UserId,
                        U.USER_NM AS UserNm,
                        U.EMP_ID AS EmpId,
                        E.emp_no AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        D.DEPT_NM AS DeptNm,
                        U.EMAIL AS Email,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                        LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            WHERE (@p_user_id IS NULL OR U.USER_ID LIKE '%' + @p_user_id + '%')
              AND (@p_user_nm IS NULL OR U.USER_NM LIKE '%' + @p_user_nm + '%')
            ORDER BY U.REG_DT DESC;
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

CREATE OR ALTER PROCEDURE SSP_WYNLAB_GetSession
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
                U.USER_ID       AS UserId,
                U.USER_NM       AS UserNm,
                E.emp_no        AS EmpNo,
                D.DEPT_NM       AS DeptNm,
                U.PASSWORD_HASH AS PasswordHash,
                U.EMAIL         AS Email,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            WHERE U.USER_ID = @p_user_id;

            SELECT USER_GRP_CD
            FROM TSMUSERGRPMAP
            WHERE USER_ID = @p_user_id;
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

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_EMP_Q]
    @p_code        VARCHAR(100) = NULL,
    @p_dept_nm     NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  a.EMP_ID,
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date
    FROM   TBAEMP as a
                JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
    WHERE  (@p_code IS NULL OR @p_code = ''
                OR a.emp_no LIKE '%' + @p_code + '%'
                OR a.emp_nm LIKE '%' + @p_code + '%')
    AND    (@p_dept_nm IS NULL OR @p_dept_nm = '' OR b.dept_nm LIKE '%' + @p_dept_nm + '%')
    ORDER BY a.emp_no;
END
GO

-- ===================== P_EMP 팝업 메타데이터: key_field를 EMP_ID로, 새로 추가된 EMP_ID 출력컬럼은 숨김 =====================
UPDATE sysPopUpM SET key_field = 'EMP_ID' WHERE popup_key = 'P_EMP';

IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_EMP' AND column_nm = 'EMP_ID')
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, visible_yn)
    VALUES ('P_EMP', 'EMP_ID', N'사원ID', 'N');
GO
