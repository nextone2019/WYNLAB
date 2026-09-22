-- frmEMP(사원등록) 저장/조회 버그 수정 (2026-09-09)
--
-- USP_BA_EMP_Q가 EMP_ID(진짜 PK, bigint IDENTITY)를 SELECT 목록에서 아예 빼먹고 있었다 - AI
-- Builder에서 Master Key Col.을 "emp_id"로 지정했지만 실제 쿼리 결과에는 없는 컬럼이라, 화면에서
-- 행을 하나라도 선택하면 row["emp_id"]를 읽는 순간(OnMasterSelectedAsync) DataException으로
-- 죽는다.
--
-- USP_BA_EMP_S도 @p_emp_id 파라미터 자체가 없이 emp_no로만 UPDATE/DELETE를 걸고 있었다 -
-- 화면(frmEMP.cs)은 DeleteClick에서 p_emp_id를 그대로 보내는데 프로시저에 그런 파라미터가 없어
-- 삭제를 누르면 SqlException이 난다. TBAACC/TBADEPT 등 다른 화면들처럼 emp_id(bigint IDENTITY)를
-- 진짜 키로 쓰도록 통일한다.
--
-- 두 프로시저 모두 로컬 개발 DB의 현재(라이브) 정의를 그대로 가져와서 필요한 부분만 고쳤다.

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
                a.EMP_ID,
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
    @p_emp_id BIGINT = NULL,	/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 연결된 계정을 여기서 채우도록 전환 */
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
    @p_photo NVARCHAR(MAX) = NULL,	/* Base64 인코딩된 이미지 - NULL/빈 문자열이면 사진을 지우지 않고 그대로 둠 */
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
        -- 다른 필드들과 같은 원칙(화면이 현재 상태를 매번 통째로 다시 보낸다) - 클라이언트가
        -- 사진을 안 건드렸으면 로드했던 값을 그대로 다시 보내고, "사진 지우기"를 누르면 빈
        -- 문자열/NULL을 보내 실제로 지운다. U에서도 항상 photo 컬럼을 덮어쓴다.
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
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAEMP SET
                emp_no = @p_emp_no, emp_nm = @p_emp_nm, emp_nm_eng = @p_emp_nm_eng, DEPT_ID = @p_dept_id,
                ent_date = @p_ent_date, grp_ent_date = @p_grp_ent_date,
                job_grade = @p_job_grade, job_type = @p_job_type, ret_yn = @p_ret_yn, ret_date = @p_ret_date,
                sex_cd = @p_sex_cd, tel = @p_tel, hp_tel = @p_hp_tel, email = @p_email, nat_cd = @p_nat_cd,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                holi_yn = @p_holi_yn, dilig_yn = @p_dilig_yn, pay_yn = @p_pay_yn, photo = @photo_bin,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE EMP_ID = @p_emp_id;
            SET @GeneratedCode = CAST(@p_emp_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAEMP WHERE EMP_ID = @p_emp_id;
            SET @GeneratedCode = CAST(@p_emp_id AS VARCHAR(20));
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
