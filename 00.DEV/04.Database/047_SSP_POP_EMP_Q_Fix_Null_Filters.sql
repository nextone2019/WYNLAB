-- SSP_POP_EMP_Q(P_EMP 팝업) 버그 수정 - 검색조건을 하나도 안 입력한 채 팝업이 뜨면(자동조회)
-- 전 파라미터가 NULL인데, 기존 WHERE절이 NULL을 그냥 문자열 연결/비교에 흘려보내서
-- (LIKE NULL, "@p_dept_cd = NULL") 결과가 항상 0건이었다 - "사원정보 팝업에 아무 데이터도
-- 안 나온다"는 증상의 원인. emp_no/emp_nm 조건에 IS NULL 가드를 추가하고, "@p_dept_cd = NULL"
-- (SQL에서 항상 거짓)을 "@p_dept_cd IS NULL"로 고치고, 등록만 되어있고 실제로는 안 쓰이던
-- @p_dept_nm 조건도 채워 넣는다(sysPopUpS에 "부서명" 검색조건으로 이미 등록되어 있었음).

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_EMP_Q]
    @p_code             VARCHAR(100) = NULL,
    @p_dept_cd          VARCHAR(100) = NULL,
    @p_dept_nm      NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                a.dept_cd,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date
    FROM   TBAEMP as a
                JOIN TBADEPT as b on a.dept_cd = b.dept_cd
    WHERE  (@p_code IS NULL OR @p_code = ''
                OR a.emp_no LIKE '%' + @p_code + '%'
                OR a.emp_nm LIKE '%' + @p_code + '%')
    AND    (@p_dept_cd IS NULL OR @p_dept_cd = '' OR a.dept_cd LIKE @p_dept_cd + '%')
    AND    (@p_dept_nm IS NULL OR @p_dept_nm = '' OR b.dept_nm LIKE '%' + @p_dept_nm + '%')
    ORDER BY a.dept_cd;
END
GO
