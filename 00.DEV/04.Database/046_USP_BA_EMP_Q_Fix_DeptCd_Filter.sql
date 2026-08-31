-- USP_BA_EMP_Q 버그 수정: @p_dept_cd가 NULL일 때 "a.dept_cd LIKE @p_dept_cd + '%'" 조건이
-- NULL과의 LIKE 비교라 항상 거짓이 되어(SQL의 NULL 연결은 NULL), 부서를 필터링하지 않고
-- 전체 조회하려는 게 정상 케이스인데도 결과가 0건으로 나왔다(사원등록 화면 소스 수정 중 발견).
-- emp_no/emp_nm 조건처럼 "@p_dept_cd IS NULL 이면 조건 자체를 건너뛴다"로 통일.

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
                            a.pay_yn
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
