-- TBAACC의 키를 acc_cd(VARCHAR PK, 사람이 직접 입력)에서 acc_id(BIGINT IDENTITY)로 바꾼다 -
-- TBAITEM이 이미 item_cd->item_id로 바뀐 것과 같은 방향. acc_cd를 참조하는 모든 테이블
-- (TBADEPT/TBAEMP/TBAITEM/TBAITEMGRP/TBAITEMUNIT)도 acc_id로 같이 바꾸고, 그 컬럼을 쓰는
-- 프로시저 전부를 맞춘다. 개발 데이터가 TBAACC 1행/acc_cd='01' 하나뿐이라 위험 부담 없이
-- ADD(새 컬럼) -> UPDATE(조인으로 채우기) -> DROP(옛 컬럼) 순서로 안전하게 옮긴다.
--
-- USP_BA_ITEMUNIT_Q/S는 건드리지 않는다 - 이미 존재하지 않는 컬럼(TBAITEMUNIT.item_cd, 실제로는
-- item_id로 바뀐 지 오래)을 참조하는 죽은 프로시저였다(호출하면 어차피 실패) - USP_BA_ITEM_Q_1/
-- USP_BA_ITEM_S_1이 item_id 기준으로 이미 올바르게 대체한 버전으로 보인다.

-- ==================== 1) 자식 테이블에 acc_id 추가 + TBAACC.acc_cd로 조인해서 채우기 ====================
-- (TBAACC.acc_cd를 아직 지우기 전에 먼저 해야 한다 - 조인 기준이 없어지면 못 채운다)

ALTER TABLE TBAACC ADD ACC_ID BIGINT IDENTITY(1,1);
GO

ALTER TABLE TBADEPT ADD acc_id BIGINT NULL;
GO
UPDATE d SET d.acc_id = a.ACC_ID FROM TBADEPT d JOIN TBAACC a ON d.acc_cd = a.ACC_CD;
GO

ALTER TABLE TBAEMP ADD acc_id BIGINT NULL;
GO
UPDATE e SET e.acc_id = a.ACC_ID FROM TBAEMP e JOIN TBAACC a ON e.acc_cd = a.ACC_CD;
GO

ALTER TABLE TBAITEM ADD acc_id BIGINT NULL;
GO
UPDATE i SET i.acc_id = a.ACC_ID FROM TBAITEM i JOIN TBAACC a ON i.acc_cd = a.ACC_CD;
GO

ALTER TABLE TBAITEMGRP ADD acc_id BIGINT NULL;
GO
UPDATE g SET g.acc_id = a.ACC_ID FROM TBAITEMGRP g JOIN TBAACC a ON g.acc_cd = a.ACC_CD;
GO

ALTER TABLE TBAITEMUNIT ADD acc_id BIGINT NULL;
GO
UPDATE u SET u.acc_id = a.ACC_ID FROM TBAITEMUNIT u JOIN TBAACC a ON u.acc_cd = a.ACC_CD;
GO

-- ==================== 2) 옛 acc_cd 컬럼 제거 ====================

ALTER TABLE TBADEPT DROP COLUMN acc_cd;
GO
ALTER TABLE TBAEMP DROP COLUMN acc_cd;
GO
ALTER TABLE TBAITEM DROP COLUMN acc_cd;
GO
ALTER TABLE TBAITEMGRP DROP COLUMN acc_cd;
GO
ALTER TABLE TBAITEMUNIT DROP COLUMN acc_cd;
GO

ALTER TABLE TBAACC DROP CONSTRAINT PK_TBAACC;
GO
ALTER TABLE TBAACC DROP COLUMN ACC_CD;
GO
ALTER TABLE TBAACC ADD CONSTRAINT PK_TBAACC PRIMARY KEY (ACC_ID);
GO

-- ==================== 3) L_ACC LookUp 등록/수정 (사업장 콤보 - value=acc_id, display=acc_nm) ====================
-- 로컬 개발DB에는 이미 L_ACC가 등록돼 있었다(source_type='P', proc_nm='SSP_CBO_ACC_Q'로 - 쿼리텍스트가
-- 아니라 위에서 고친 프로시저를 그대로 호출하는 방식) - value_field만 옛 acc_cd를 가리키고 있어서
-- 그것만 고친다. 혹시 L_ACC 자체가 없는 환경(운영 등)이면 같은 방식(프로시저 기반)으로 새로 만든다.

IF EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'L_ACC')
BEGIN
    UPDATE sysLookupM SET value_field = 'acc_id' WHERE lookup_key = 'L_ACC';
END
ELSE
BEGIN
    INSERT INTO sysLookupM (lookup_key, source_type, proc_nm, query_txt, lookup_nm, value_field, display_field, use_yn, remark)
    VALUES ('L_ACC', 'P', 'SSP_CBO_ACC_Q', NULL, N'사업장', 'acc_id', 'acc_nm', 'Y', N'frmItem 등 acc_id 콤보용(사업장)');
END
GO

-- ==================== 4) 프로시저 수정 ====================

CREATE OR ALTER PROCEDURE SSP_CBO_ACC_Q
AS
BEGIN
    SET NOCOUNT ON;

    SELECT acc_id, acc_nm
    FROM TBAACC
    WHERE 1 = 1;
END
GO

CREATE OR ALTER PROCEDURE USP_BA_ACC_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_acc_nm NVARCHAR(100) = NULL,
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
            SELECT acc_id, acc_nm
            FROM TBAACC
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_acc_nm IS NULL OR acc_nm LIKE '%' + @p_acc_nm + '%')
            ORDER BY acc_id;
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

CREATE OR ALTER PROCEDURE USP_BA_ACC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_acc_nm NVARCHAR(100) = NULL,
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
            INSERT INTO TBAACC (acc_nm, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_acc_nm, @p_user_id, GETDATE(), @p_client_pc);

            SET @p_acc_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAACC SET
                acc_nm = @p_acc_nm, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @p_acc_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAACC WHERE acc_id = @p_acc_id;
        END

        SET @GeneratedCode = CAST(@p_acc_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_DEPT_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_dept_nm VARCHAR(50) = NULL,
    @p_dept_id BIGINT = NULL,		/* Q1일 때만 사용 - 정확히 일치하는 부서 */
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
                acc_id, DEPT_ID, dept_nm, PAR_DEPT_ID, dept_type, remark
            FROM TBADEPT
            WHERE (@p_dept_nm IS NULL OR dept_nm LIKE '%' + @p_dept_nm + '%')
            ORDER BY DEPT_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                emp_no, emp_nm, emp_nm_eng, job_grade, job_type, tel, hp_tel, email
            FROM TBAEMP
            WHERE DEPT_ID = @p_dept_id
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_DEPT_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 사업장 생기면 서버가 채우도록 전환 */
    @p_dept_id BIGINT = NULL,		/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_dept_nm VARCHAR(50) = NULL,
    @p_par_dept_id BIGINT = NULL,
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
                acc_id, dept_nm, PAR_DEPT_ID, dept_type, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_dept_nm, @p_par_dept_id, @p_dept_type, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBADEPT SET
                dept_nm = @p_dept_nm, PAR_DEPT_ID = @p_par_dept_id, dept_type = @p_dept_type,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE DEPT_ID = @p_dept_id;
            SET @GeneratedCode = CAST(@p_dept_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBADEPT WHERE DEPT_ID = @p_dept_id;
            SET @GeneratedCode = CAST(@p_dept_id AS VARCHAR(20));
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no   VARCHAR(20) = NULL,
    @p_dept_id  BIGINT = NULL,
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
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                a.DEPT_ID,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date,
                a.job_grade,
                a.job_type,
                a.ret_yn,
                a.ret_date,
                a.sex_cd,
                a.tel,
                a.hp_tel,
                a.email,
                a.nat_cd,
                a.zip_code,
                a.addr1,
                a.addr2,
                a.holi_yn,
                a.dilig_yn,
                a.pay_yn,
                a.photo
            FROM        TBAEMP as  a
                            JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
            WHERE       1 = 1
            AND         (@p_dept_id IS NULL OR a.DEPT_ID = @p_dept_id)
            AND         ((@p_emp_no IS NULL OR emp_no LIKE '%' + @p_emp_no + '%')
                            OR
                          (@p_emp_no IS NULL OR emp_nm LIKE '%' + @p_emp_no + '%'))
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 사업장 생기면 거기서 채우도록 전환 */
    @p_emp_no VARCHAR(20),
    @p_emp_nm VARCHAR(100) = NULL,
    @p_emp_nm_eng VARCHAR(100) = NULL,
    @p_dept_id BIGINT = NULL,
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
    @p_photo NVARCHAR(MAX) = NULL,	/* Base64 인코딩된 이미지 - NULL/빈문자열이면 사진 없음으로 저장 */
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
        DECLARE @photo_bin VARBINARY(MAX) = NULL;
        IF @p_photo IS NOT NULL AND LEN(@p_photo) > 0
            SET @photo_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_photo"))', 'VARBINARY(MAX)');

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAEMP (
                acc_id, emp_no, emp_nm, emp_nm_eng, DEPT_ID, ent_date, grp_ent_date,
                job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email,
                nat_cd, zip_code, addr1, addr2, holi_yn, dilig_yn, pay_yn, photo,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_emp_no, @p_emp_nm, @p_emp_nm_eng, @p_dept_id, @p_ent_date, @p_grp_ent_date,
                @p_job_grade, @p_job_type, @p_ret_yn, @p_ret_date, @p_sex_cd, @p_tel, @p_hp_tel, @p_email,
                @p_nat_cd, @p_zip_code, @p_addr1, @p_addr2, @p_holi_yn, @p_dilig_yn, @p_pay_yn, @photo_bin,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAEMP SET
                emp_nm = @p_emp_nm, emp_nm_eng = @p_emp_nm_eng, DEPT_ID = @p_dept_id,
                ent_date = @p_ent_date, grp_ent_date = @p_grp_ent_date,
                job_grade = @p_job_grade, job_type = @p_job_type, ret_yn = @p_ret_yn, ret_date = @p_ret_date,
                sex_cd = @p_sex_cd, tel = @p_tel, hp_tel = @p_hp_tel, email = @p_email, nat_cd = @p_nat_cd,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                holi_yn = @p_holi_yn, dilig_yn = @p_dilig_yn, pay_yn = @p_pay_yn, photo = @photo_bin,
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMPLIST_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no   VARCHAR(20) = NULL,
    @p_dept_nm   NVARCHAR(50) = NULL,
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
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                a.DEPT_ID,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date,
                a.job_grade,
                a.job_type,
                a.ret_yn,
                a.ret_date,
                a.sex_cd,
                a.tel,
                a.hp_tel,
                a.email,
                a.nat_cd,
                a.zip_code,
                a.addr1,
                a.addr2,
                a.holi_yn,
                a.dilig_yn,
                a.pay_yn,
                a.photo
            FROM        TBAEMP as  a
                            JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
            WHERE       1 = 1
            AND         (@p_dept_nm IS NULL OR @p_dept_nm = '' OR b.dept_nm LIKE '%' + @p_dept_nm + '%')
            AND         ((@p_emp_no IS NULL OR emp_no LIKE '%' + @p_emp_no + '%')
                            OR
                          (@p_emp_no IS NULL OR emp_nm LIKE '%' + @p_emp_no + '%'))
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
                acc_id, item_lvl, item_class_cd, item_class_nm, par_item_class_cd, remark
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
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 사업장 생기면 서버가 채우도록 전환 */
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
                acc_id, item_lvl, item_class_cd, item_class_nm, par_item_class_cd, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_item_lvl, @p_item_class_cd, @p_item_class_nm, @p_par_item_class_cd, @p_remark,
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
                acc_id, item_no, item_nm, item_spec, unit_cd, po_unit_cd, wh_id, loc_id,
                safe_qty, DEPT_ID, EMP_ID, prod_yn, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn, po_price, sale_yn, sale_price,
                stat_cd, item_grp1, item_grp2, item_grp3, item_grp4,
                po_acnt_cd, sale_acnt_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_id, @p_loc_id,
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_Q_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
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
                acc_id, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, remark
            FROM TBAITEMUNIT
            WHERE item_id = @p_item_id
            ORDER BY item_id, fr_unit_cd, to_unit_cd;
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

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT,
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
                acc_id, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_item_id, @p_fr_unit_cd, @p_fr_qty, @p_to_unit_cd, @p_to_qty, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMUNIT SET
                fr_qty = @p_fr_qty, to_qty = @p_to_qty, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE item_id = @p_item_id AND fr_unit_cd = @p_fr_unit_cd AND to_unit_cd = @p_to_unit_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEMUNIT
            WHERE item_id = @p_item_id AND fr_unit_cd = @p_fr_unit_cd AND to_unit_cd = @p_to_unit_cd;
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
