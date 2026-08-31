-- TBAEMP.PHOTO(사원사진등록) - 실행해보니 이 컬럼은 이미 존재했다(레거시 IMAGE 타입, 원래
-- 테이블 설계에 예비로 들어있던 것으로 보임) - 그래서 ALTER TABLE은 생략한다. IMAGE는
-- Microsoft가 신규 설계엔 VARBINARY(MAX)를 권장하지만(레거시/사용 중단 예정 타입), byte[]
-- 읽기/쓰기 자체는 VARBINARY(MAX)와 동일하게 동작하므로 굳이 타입을 바꾸지 않고 그대로 쓴다.
--
-- 클라이언트는 Base64 문자열로 주고받는다:
--   조회: byte[] 컬럼값을 System.Text.Json이 응답 JSON에서 자동으로 Base64 문자열로 직렬화
--         해준다 - 서버 코드가 따로 인코딩할 필요 없음(ProcData.ToDataTable이 그대로 문자열로
--         받는다).
--   저장: 클라이언트가 PictureEditWyn.ImageBytes를 Convert.ToBase64String으로 인코딩해서
--         p_photo(NVARCHAR(MAX)) 파라미터로 보내고, 여기서 XML 기반 base64Binary 변환으로
--         VARBINARY(MAX)로 되돌린다(SQL Server에 base64 디코딩 내장 함수가 따로 없어서 이
--         방식이 표준적으로 쓰인다) - VARBINARY(MAX) -> IMAGE는 암시적 변환된다.

/* ---------- USP_BA_EMP_Q: photo 컬럼 추가 ---------- */
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no   VARCHAR(20) = NULL,
    @p_dept_cd   VARCHAR(20) = NULL,
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
                            a.emp_no,
                            a.emp_nm,
                            a.emp_nm_eng,
                            a.dept_cd,
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
                            JOIN TBADEPT as b on a.dept_cd = b.dept_cd
            WHERE       1 = 1
            AND         (@p_dept_cd IS NULL OR @p_dept_cd = '' OR a.dept_cd LIKE @p_dept_cd + '%')
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

/* ---------- USP_BA_EMP_S: @p_photo(Base64) 파라미터 추가 ---------- */
CREATE OR ALTER PROCEDURE USP_BA_EMP_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 거기서 채우도록 전환 */
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
    @p_photo NVARCHAR(MAX) = NULL,	/* Base64 인코딩된 이미지 - NULL/빈문자열이면 사진 유지, 지우려면 별도 처리 없이 빈 문자열 전송 시 NULL로 저장 */
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
                acc_cd, emp_no, emp_nm, emp_nm_eng, dept_cd, ent_date, grp_ent_date,
                job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email,
                nat_cd, zip_code, addr1, addr2, holi_yn, dilig_yn, pay_yn, photo,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_emp_no, @p_emp_nm, @p_emp_nm_eng, @p_dept_cd, @p_ent_date, @p_grp_ent_date,
                @p_job_grade, @p_job_type, @p_ret_yn, @p_ret_date, @p_sex_cd, @p_tel, @p_hp_tel, @p_email,
                @p_nat_cd, @p_zip_code, @p_addr1, @p_addr2, @p_holi_yn, @p_dilig_yn, @p_pay_yn, @photo_bin,
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
