-- frmEmpList 검색조건(dept_id, emp_nm) 반영.
-- 직전 버전(사장님이 직접 수정)은 @p_dept_id/@p_emp_nm를 파라미터로만 추가하고 정작 WHERE절에서
-- 안 써서 검색이 동작하지 않았고, 기본값(= NULL)도 없어서 화면이 재배포되기 전(구버전 dll)까지는
-- "매개변수를 제공하지 않았습니다" 오류가 났다. 둘 다 여기서 같이 고친다.

CREATE OR ALTER PROCEDURE USP_BA_EMPLIST_Q
    @p_work_type VARCHAR(50),
    @p_dept_id BIGINT = NULL,
    @p_emp_nm VARCHAR(30) = NULL,
    ---------------------------------------------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------
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
            SELECT  a.emp_no,
                    a.emp_nm,
                    a.emp_nm_eng,
                    a.ent_date,
                    a.grp_ent_date,
                    a.job_grade,
                    a.job_type,
                    a.ret_yn,
                    a.ret_date,
                    a.sex_cd,
                    a.tel,
                    a.email,
                    a.EMP_ID,
                    a.dept_id,
                    b.dept_nm
            FROM    TBAEMP AS a
                    JOIN TBADEPT AS b ON a.dept_id = b.dept_id
            WHERE   (@p_dept_id IS NULL OR a.dept_id = @p_dept_id)
                AND (@p_emp_nm IS NULL OR a.emp_nm LIKE '%' + @p_emp_nm + '%')
            ORDER BY EMP_ID;
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
