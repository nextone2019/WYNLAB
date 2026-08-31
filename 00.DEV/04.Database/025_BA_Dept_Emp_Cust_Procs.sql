/* =========================================================
   BA(기준정보) 모듈 - frmDept/frmEmp/frmCust 세 화면의 _Q/_S 프로시저 첫 완성분.
   USP_SM_MINORCODE_Q/_S(007/028) 형식을 그대로 따른다:
   - 모든 입력 파라미터는 @p_ + snake_case(테이블 컬럼명과 동일 철자)
   - 표준 출력 5개(@GeneratedCode/@ReturnCode/@ReturnMsg/@ErrorCode/@ErrorMsg)는
     항상 이 순서, PascalCase 그대로
   - 첫 파라미터는 항상 @p_work_type - 조회는 'Q', 저장은 신규='N'/수정='U'/삭제='D'
   - 전부 TRY/CATCH로 감싸서 SQL 예외가 나면 @ErrorCode/@ErrorMsg로 화면까지 전달됨
   - @p_user_id/@p_client_pc는 화면이 안 보낸다 - 서버(ProcData.SaveAsync)가 로그인
     세션 기준으로 채운다.

   [acc_cd 관련 - 2026-08-28] TBADEPT/TBAEMP에 있는 acc_cd(회사코드로 추정, TBADEPT에서는
   PK 일부)는 아직 로그인 세션에 회사 개념이 없어서(TSMUSER 미완성, 추후 정리 예정) 지금은
   프로시저 기본값('0001') 하나로 고정한다. 세션에 회사코드가 생기면 그때 @p_user_id처럼
   서버가 자동으로 채우도록 바꾸면 된다 - 화면(frmDept/frmEmp)은 이 파라미터를 아예 안 보내도
   되니 그 전환 시점에 화면 쪽은 안 건드려도 된다.

   TBAEMP의 photo/photo_file_nm/photo_path(사진 업로드용)는 화면에 아직 업로드 UI가 없어서
   이번 첫 완성분에서는 뺐다 - 필요해지면 별도 프로시저(또는 이 프로시저에 파라미터 추가)로
   처리할 것.
   ========================================================= */

-- ===================== BA_DEPT (부서등록, frmDept) =====================

CREATE OR ALTER PROCEDURE USP_BA_DEPT_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_dept_cd VARCHAR(20) = NULL,
    @p_dept_nm VARCHAR(50) = NULL,
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
                acc_cd, dept_cd, dept_nm, par_dept_cd, dept_type, remark
            FROM TBADEPT
            WHERE (@p_dept_cd IS NULL OR dept_cd LIKE '%' + @p_dept_cd + '%')
              AND (@p_dept_nm IS NULL OR dept_nm LIKE '%' + @p_dept_nm + '%')
            ORDER BY dept_cd;
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

CREATE OR ALTER PROCEDURE USP_BA_DEPT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환 */
    @p_dept_cd VARCHAR(20),
    @p_dept_nm VARCHAR(50) = NULL,
    @p_par_dept_cd VARCHAR(20) = NULL,
    @p_dept_type VARCHAR(10) = NULL,
    @p_remark NVARCHAR(2000) = NULL,
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
            INSERT INTO TBADEPT (
                acc_cd, dept_cd, dept_nm, par_dept_cd, dept_type, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_dept_cd, @p_dept_nm, @p_par_dept_cd, @p_dept_type, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBADEPT SET
                dept_nm = @p_dept_nm, par_dept_cd = @p_par_dept_cd, dept_type = @p_dept_type,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_cd = @p_acc_cd AND dept_cd = @p_dept_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBADEPT WHERE acc_cd = @p_acc_cd AND dept_cd = @p_dept_cd;
        END

        SET @GeneratedCode = @p_dept_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ===================== BA_EMP (사원등록, frmEmp) =====================

CREATE OR ALTER PROCEDURE USP_BA_EMP_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no VARCHAR(20) = NULL,
    @p_emp_nm VARCHAR(100) = NULL,
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
                acc_cd, emp_no, emp_nm, emp_nm_eng, dept_cd, ent_date, grp_ent_date,
                job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email,
                nat_cd, zip_code, addr1, addr2, holi_yn, dilig_yn, pay_yn
            FROM TBAEMP
            WHERE (@p_emp_no IS NULL OR emp_no LIKE '%' + @p_emp_no + '%')
              AND (@p_emp_nm IS NULL OR emp_nm LIKE '%' + @p_emp_nm + '%')
            ORDER BY emp_no;
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

CREATE OR ALTER PROCEDURE USP_BA_EMP_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환 */
    @p_emp_no VARCHAR(20),
    @p_emp_nm VARCHAR(100) = NULL,
    @p_emp_nm_eng VARCHAR(100) = NULL,
    @p_dept_cd VARCHAR(20) = NULL,
    @p_ent_date VARCHAR(8) = NULL,
    @p_grp_ent_date VARCHAR(8) = NULL,
    @p_job_grade VARCHAR(10) = NULL,
    @p_job_type VARCHAR(10) = NULL,
    @p_ret_yn VARCHAR(2) = 'N',
    @p_ret_date VARCHAR(8) = NULL,
    @p_sex_cd VARCHAR(1) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_hp_tel VARCHAR(30) = NULL,
    @p_email NVARCHAR(50) = NULL,
    @p_nat_cd VARCHAR(10) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(300) = NULL,
    @p_addr2 NVARCHAR(300) = NULL,
    @p_holi_yn VARCHAR(1) = 'N',
    @p_dilig_yn VARCHAR(1) = NULL,
    @p_pay_yn VARCHAR(1) = NULL,
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
            INSERT INTO TBAEMP (
                acc_cd, emp_no, emp_nm, emp_nm_eng, dept_cd, ent_date, grp_ent_date,
                job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email,
                nat_cd, zip_code, addr1, addr2, holi_yn, dilig_yn, pay_yn,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_emp_no, @p_emp_nm, @p_emp_nm_eng, @p_dept_cd, @p_ent_date, @p_grp_ent_date,
                @p_job_grade, @p_job_type, @p_ret_yn, @p_ret_date, @p_sex_cd, @p_tel, @p_hp_tel, @p_email,
                @p_nat_cd, @p_zip_code, @p_addr1, @p_addr2, @p_holi_yn, @p_dilig_yn, @p_pay_yn,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAEMP SET
                emp_nm = @p_emp_nm, emp_nm_eng = @p_emp_nm_eng, dept_cd = @p_dept_cd,
                ent_date = @p_ent_date, grp_ent_date = @p_grp_ent_date,
                job_grade = @p_job_grade, job_type = @p_job_type, ret_yn = @p_ret_yn, ret_date = @p_ret_date,
                sex_cd = @p_sex_cd, tel = @p_tel, hp_tel = @p_hp_tel, email = @p_email, nat_cd = @p_nat_cd,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                holi_yn = @p_holi_yn, dilig_yn = @p_dilig_yn, pay_yn = @p_pay_yn,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE emp_no = @p_emp_no;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAEMP WHERE emp_no = @p_emp_no;
        END

        SET @GeneratedCode = @p_emp_no;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ===================== BA_CUST (거래처등록, frmCust) =====================

CREATE OR ALTER PROCEDURE USP_BA_CUST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_cd VARCHAR(20) = NULL,
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
                cust_cd, cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2,
                homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate,
                remark, stat_cd, emp_no
            FROM TBACUST
            WHERE (@p_cust_cd IS NULL OR cust_cd LIKE '%' + @p_cust_cd + '%')
              AND (@p_cust_nm IS NULL OR cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY cust_cd;
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
    @p_cust_cd VARCHAR(20),
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
                cust_cd, cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2,
                homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate,
                remark, stat_cd, emp_no, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_cust_cd, @p_cust_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_zip_code, @p_addr1, @p_addr2,
                @p_homepage, @p_email, @p_fax, @p_biz_kind, @p_biz_type, @p_trans_open_date, @p_vat_type, @p_vat_rate,
                @p_remark, @p_stat_cd, @p_emp_no, @p_user_id, GETDATE(), @p_client_pc
            );
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
            WHERE cust_cd = @p_cust_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUST WHERE cust_cd = @p_cust_cd;
        END

        SET @GeneratedCode = @p_cust_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
