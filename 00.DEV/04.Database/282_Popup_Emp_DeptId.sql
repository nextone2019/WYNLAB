-- 282: 사원 팝업(P_EMP)이 소속 부서 ID(dept_id)도 돌려준다 (2026-10-04). 화면이 담당자를 고르면 부서코드/부서명도 같이 채우고(PopupLookupEditWyn.LinkDept),
--   담당자 팝업을 열 때는 화면의 부서명을 p_dept_nm 조건으로 넘긴다. dept_id는 팝업 그리드에는 숨김(visible_yn=N).

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
                a.DEPT_ID AS dept_id,
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
IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_EMP' AND column_nm = 'dept_id')
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
    VALUES ('P_EMP', 'dept_id', N'부서ID', 'TEXT', NULL, 4, 100, 'N');
GO
