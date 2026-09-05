/* =========================================================
   BA_CUST(거래처등록) 재설계 - AI Builder의 "레코드셋 여러 개 + 저장프로시저 여러 개" 케이스를
   실제 화면으로 검증하기 위한 첫 사례.

   구조:
   - TBACUST(거래처마스터) - grd1(목록) + panData(상세폼, grd1 선택행을 그대로 복사해서 채움 -
     별도 조회 없음. AI Builder MasterFormGrid 템플릿과 동일한 방식이라 레코드셋을 따로 안 만듦)
   - TBACUSTPRSN(담당자정보) / TBACUSTACNT(계좌정보) - grd2/grd3(탭), 둘 다 USP_BA_CUST_Q의
     같은 호출(@p_work_type='Q1') 안에서 SELECT 두 개로 순서대로 반환 - 프로시저 하나가
     레코드셋을 2개 반환하는 케이스.

   저장은 대상 테이블별로 프로시저를 나눴다(화면 하나가 저장프로시저 3개를 쓰는 케이스):
   - USP_BA_CUST_S    : panData -> TBACUST (기존 프로시저, 대상 테이블 안 바뀜)
   - USP_BA_CUST_S_1  : grd2    -> TBACUSTPRSN (신규)
   - USP_BA_CUST_S_2  : grd3    -> TBACUSTACNT (신규)

   TBACUSTPRSN/TBACUSTACNT의 PK는 (cust_cd, serl) - serl은 IDENTITY가 아니라(거래처별로 1부터
   증가하는 일련번호) 저장프로시저가 신규(N) 시 MAX(serl)+1로 직접 채운다.

   기존 USP_BA_CUST_Q/USP_BA_CUST_S는 025_BA_Dept_Emp_Cust_Procs.sql에서 만든 걸 여기서
   CREATE OR ALTER로 교체한다 - 025는 그대로 두고(이미 적용된 스크립트는 수정하지 않는다는 규칙),
   최신 정의는 이 파일이 기준이다.
   ========================================================= */

-- ===================== BA_CUST (거래처마스터, grd1+panData) =====================

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
            -- 레코드셋 1개 - grd1이 이 결과를 받고, panData는 grd1에서 선택된 행 값을 그대로
            -- 복사해서 채운다(화면이 별도로 다시 조회하지 않음).
            SELECT
                cust_cd, cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2,
                homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate,
                remark, stat_cd, emp_no
            FROM TBACUST
            WHERE (@p_cust_cd IS NULL OR cust_cd LIKE '%' + @p_cust_cd + '%')
              AND (@p_cust_nm IS NULL OR cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY cust_cd;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            -- 레코드셋 2개 - grd2(담당자), grd3(계좌) 순서대로. @p_cust_cd는 grd1에서 선택된
            -- 거래처코드(필수)로 넘어온다.
            SELECT cust_cd, serl, prsn_nm, grade, tel1, tel2, fax, email
            FROM TBACUSTPRSN
            WHERE cust_cd = @p_cust_cd
            ORDER BY serl;

            SELECT cust_cd, serl, bank_cd, acnt_no, remark
            FROM TBACUSTACNT
            WHERE cust_cd = @p_cust_cd
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

-- ===================== BA_CUST_PRSN (담당자정보, grd2) =====================

CREATE OR ALTER PROCEDURE USP_BA_CUST_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_cd VARCHAR(20),
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
            -- serl은 IDENTITY가 아니라 거래처(cust_cd)별로 1부터 증가하는 일련번호 - 신규일 때 여기서 채운다.
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TBACUSTPRSN WHERE cust_cd = @p_cust_cd;

            INSERT INTO TBACUSTPRSN (cust_cd, serl, prsn_nm, grade, tel1, tel2, fax, email, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_cust_cd, @p_serl, @p_prsn_nm, @p_grade, @p_tel1, @p_tel2, @p_fax, @p_email, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUSTPRSN SET
                prsn_nm = @p_prsn_nm, grade = @p_grade, tel1 = @p_tel1, tel2 = @p_tel2,
                fax = @p_fax, email = @p_email, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cust_cd = @p_cust_cd AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUSTPRSN WHERE cust_cd = @p_cust_cd AND serl = @p_serl;
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

-- ===================== BA_CUST_ACNT (계좌정보, grd3) =====================

CREATE OR ALTER PROCEDURE USP_BA_CUST_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_cd VARCHAR(20),
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
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TBACUSTACNT WHERE cust_cd = @p_cust_cd;

            INSERT INTO TBACUSTACNT (cust_cd, serl, bank_cd, acnt_no, remark, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_cust_cd, @p_serl, @p_bank_cd, @p_acnt_no, @p_remark, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUSTACNT SET
                bank_cd = @p_bank_cd, acnt_no = @p_acnt_no, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cust_cd = @p_cust_cd AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUSTACNT WHERE cust_cd = @p_cust_cd AND serl = @p_serl;
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
