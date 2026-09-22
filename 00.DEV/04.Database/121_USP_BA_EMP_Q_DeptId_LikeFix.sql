-- USP_BA_EMP_Q 부서 검색 조건 수정 (2026-09-09)
--
-- DEPT_ID가 080 마이그레이션에서 VARCHAR 코드 -> BIGINT ID로 바뀌었는데, 이 프로시저의 WHERE절은
-- 옛날 방식 그대로 "a.DEPT_ID LIKE @p_dept_id + '%'"(부분일치 코드검색)를 쓰고 있었다. @p_dept_id가
-- BIGINT라서 '%' 문자열을 bigint로 변환하려다 항상 변환 오류가 나고, 그 오류가 TRY/CATCH에 먹혀
-- 결과 없이 빈 그리드만 나온다(조건을 비워도 항상 실패 - 실제로 겪음, 2026-09-09).
--
-- 다른 화면(081_TBADEPT_Procs.sql 등)처럼 정확일치(=)로 통일한다 - DEPT_ID는 이제 사람이 입력하는
-- 코드가 아니라 부서선택 팝업이 돌려주는 ID값이므로 부분일치가 애초에 의미가 없다.

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
