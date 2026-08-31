-- USP_BA_DEPT_Q에 work_type='Q1' 분기 추가 - 부서등록 화면(frmDept) 왼쪽이 트리로 바뀌면서,
-- 선택한 부서에 소속된 사원 목록(grd2)을 이 프로시저 하나로 같이 처리한다(USP_SM_MINORCODE_Q의
-- Q/Q1 분리와 같은 패턴). @p_dept_cd는 Q일 때는 검색어(LIKE), Q1일 때는 정확히 일치하는
-- 부서코드로 의미가 다르다 - 화면이 어느 쪽으로 부르는지에 따라 같은 파라미터를 다르게 쓴다.

CREATE OR ALTER PROCEDURE USP_BA_DEPT_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_dept_cd VARCHAR(20) = NULL,		/* Q일 때 검색어(LIKE), Q1일 때 정확히 일치하는 부서코드 */
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
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                emp_no, emp_nm, emp_nm_eng, job_grade, job_type, tel, hp_tel, email
            FROM TBAEMP
            WHERE dept_cd = @p_dept_cd
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
