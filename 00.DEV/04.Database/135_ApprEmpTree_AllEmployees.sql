-- 전자결재 화면의 부서트리+사원목록(treeDept+grdEmp)을 부서/사원 통합 조직도 트리(treeEmp)
-- 하나로 합치기 위한 선행 작업. 통합 트리는 부서 목록(Q2)과 사원 전체 목록을 한 번에 받아
-- 클라이언트에서 "D"+dept_id / "E"+emp_id 키로 합쳐 그린다 - 이때 각 사원이 어느 부서 밑에
-- 붙는지 알아야 하므로 Q3 결과에 DEPT_ID가 반드시 있어야 한다(기존엔 없었음).
--
-- 기존 Q3(부서별 사원목록)은 @p_dept_id가 항상 필수였다(트리가 부서 선택 시점에 그 부서만
-- 조회). 조직도 화면은 처음 열릴 때 전체 사원을 한 번에 받아야 하므로,
-- @p_dept_id가 NULL이면 "전체 사원"을 의미하도록 WHERE절만 바꾼다(파라미터 자체는 이미
-- 프로시저 선언에서 NULL 기본값이었음 - 이 분기 안에서 그 의미를 실제로 살린 적이 없었을 뿐).
CREATE OR ALTER PROCEDURE USP_AP_APPR_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_route_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
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
        IF @p_work_type = 'Q' -- 특정 문서 1건의 결재이력(헤더 요약본 + 전 결재경로, 결재+수신 구분 없이)
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.app_text, a.doc_type, a.doc_id,
                   a.doc_no, a.form_id, a.end_yn, a.end_dt, a.rtn_yn, a.rtn_dt, a.stat_cd, a.emp_id, e.emp_nm
            FROM TAPDOC a
            LEFT JOIN TBAEMP e ON e.EMP_ID = a.emp_id
            WHERE a.doc_type = @p_doc_type AND a.doc_id = @p_doc_id
            ORDER BY a.app_id DESC;

            SELECT p.app_id, p.serl, p.sort, p.path_type, p.emp_id, ep.emp_no, ep.emp_nm, p.stat_cd, p.app_dt, p.remark
            FROM TAPDOCPATH p
            LEFT JOIN TBAEMP ep ON ep.EMP_ID = p.emp_id
            WHERE p.doc_type = @p_doc_type AND p.doc_id = @p_doc_id
            ORDER BY p.app_id DESC, p.path_type, p.sort;
        END
        ELSE IF @p_work_type = 'Q1' -- 결재함: 로그인 사용자가 지금 처리해야 함(자기 차례임) 목록
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   p.serl, p.sort, p.path_type, e2.emp_nm AS req_emp_nm
            FROM TAPDOCPATH p
            JOIN TAPDOC a ON a.app_id = p.app_id
            JOIN TBAEMP e ON e.emp_no = @p_emp_no
            LEFT JOIN TBAEMP e2 ON e2.EMP_ID = a.emp_id
            WHERE p.emp_id = e.EMP_ID AND p.path_type = 'A' AND p.stat_cd = 'N'
              AND a.end_yn = 'N' AND a.rtn_yn = 'N'
              AND NOT EXISTS (
                  SELECT 1 FROM TAPDOCPATH q
                  WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort < p.sort AND q.stat_cd <> 'Y'
              )
            ORDER BY a.app_id;
        END
        ELSE IF @p_work_type = 'Q2' -- 부서트리(전체)
        BEGIN
            SELECT dept_id, dept_nm, par_dept_id, dept_type
            FROM TBADEPT
            ORDER BY dept_id;
        END
        ELSE IF @p_work_type = 'Q3' -- 사원목록(@p_dept_id 지정 시 그 부서만, NULL이면 전체 - 조직도 트리용)
        BEGIN
            SELECT EMP_ID, emp_no, emp_nm, job_grade, DEPT_ID
            FROM TBAEMP
            WHERE @p_dept_id IS NULL OR DEPT_ID = @p_dept_id
            ORDER BY emp_no;
        END
        ELSE IF @p_work_type = 'Q4' -- 내 결재경로 목록
        BEGIN
            DECLARE @my_emp_id BIGINT;
            SELECT @my_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            SELECT route_id, route_nm
            FROM TAPROUTE
            WHERE emp_id = @my_emp_id AND use_yn = 'Y'
            ORDER BY route_nm;
        END
        ELSE IF @p_work_type = 'Q5' -- 결재경로 상세
        BEGIN
            SELECT d.route_id, d.sort, d.emp_id, e.emp_no, e.emp_nm, d.path_type
            FROM TAPROUTEDETAIL d
            LEFT JOIN TBAEMP e ON e.EMP_ID = d.emp_id
            WHERE d.route_id = @p_route_id
            ORDER BY d.path_type, d.sort;
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
