-- 홈화면 "결재 리스트"(기안함/결재함/기안서 작성) 통합 화면용 (2026-10-03)
--
-- 1) USP_AP_APPR_Q
--    - 첫 파라미터 @p_acc_id 추가(화면 표준 - 사업장 조건), 필터 파라미터 추가:
--      @p_date_from/@p_date_to(상신일 yyyyMMdd), @p_title(LIKE), @p_req_emp_no(기안자 사번),
--      @p_stat_cd('P'=진행중/미결재 0·1, 'R'=반려, 'E'=승인완료; 비우면 전체). 전부 NULL이면 예전과 동일.
--    - Q1(결재함-미결재) / Q6(기안함) 결과에 컬럼 추가: req_dt(상신일시), stat_cd, cur_appr_emp_nm(결재대기자),
--      last_appr_emp_nm(최종승인자) - 기존 호출자(frmApprInbox, HomeForm)는 추가 컬럼을 무시하므로 영향 없음.
--    - Q7 신설(결재함-반려/결재완료): 내가 결재라인(C) 또는 수신라인(R)에 들어있는 문서(내가 기안한 문서 제외)를
--      @p_stat_cd로 거른다. path_type 컬럼이 내 역할(C=결재/R=수신).
-- 2) TSMMINOR(AP0002 문서유형)의 rel_cd1 = 문서등록 화면("{MODULE}.{화면클래스명}"), rel_cd2 = 기안서 작성 화면의
--    분류 칩 이름 - 기안서 작성 탭이 이 값으로 바로가기 타일을 만든다(rel_cd1이 비어 있으면 타일 없음).
--
-- WYNLAB_DEV / FADU 양쪽에 같은 내용으로 적용(적용 전 두 DB의 USP_AP_APPR_Q 정의가 동일함을 확인함).
-- CREATE OR ALTER / 조건부 UPDATE라 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_AP_APPR_Q
    @p_acc_id BIGINT = NULL,           /* 사업장 - 화면 조회조건(표준), 비우면 전체. Q1/Q6/Q7만 사용 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_route_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_date_from VARCHAR(8) = NULL,    /* Q1/Q6/Q7 - 상신일 시작(yyyyMMdd) */
    @p_date_to VARCHAR(8) = NULL,      /* Q1/Q6/Q7 - 상신일 끝(yyyyMMdd) */
    @p_title NVARCHAR(200) = NULL,     /* Q1/Q6/Q7 - 제목 포함 검색 */
    @p_req_emp_no VARCHAR(20) = NULL,  /* Q1/Q6/Q7 - 기안자 사번 */
    @p_stat_cd VARCHAR(1) = NULL,      /* Q6/Q7 - P=진행중(0,1) / R=반려 / E=승인완료 */
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
        IF @p_work_type = 'Q' -- 특정 문서 1건의 전체이력(헤더 + 라인, path_type 그대로 반환)
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.app_text, a.doc_type, a.doc_id,
                   a.doc_no, a.form_id, a.end_yn, a.end_dt, a.rtn_yn, a.rtn_dt, a.app_stat_cd AS stat_cd, a.emp_id, e.emp_nm
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
        ELSE IF @p_work_type = 'Q1' -- 결재함(미결재): 로그인 사용자가 지금 처리해야 할(자기 차례인) 대기건
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.reg_dt AS req_dt, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   p.serl, p.sort, p.path_type, re.emp_nm AS req_emp_nm, a.app_stat_cd AS stat_cd,
                   ca.emp_nm AS cur_appr_emp_nm, la.emp_nm AS last_appr_emp_nm
            FROM TAPDOCPATH p
            JOIN TAPDOC a ON a.app_id = p.app_id
            JOIN TBAEMP e ON e.emp_no = @p_emp_no
            LEFT JOIN TBAEMP re ON re.EMP_ID = a.emp_id
            OUTER APPLY (
                SELECT TOP 1 x.emp_nm
                FROM TAPDOCPATH c
                LEFT JOIN TBAEMP x ON x.EMP_ID = c.emp_id
                WHERE c.app_id = a.app_id AND c.path_type = 'C' AND c.stat_cd = '0'
                  AND NOT EXISTS (
                      SELECT 1 FROM TAPDOCPATH q
                      WHERE q.app_id = c.app_id AND q.path_type = 'C' AND q.sort < c.sort AND q.stat_cd <> 'E'
                  )
                ORDER BY c.sort
            ) ca
            OUTER APPLY (
                SELECT TOP 1 x.emp_nm
                FROM TAPDOCPATH c
                LEFT JOIN TBAEMP x ON x.EMP_ID = c.emp_id
                WHERE c.app_id = a.app_id AND c.path_type = 'C'
                ORDER BY c.sort DESC
            ) la
            WHERE p.emp_id = e.EMP_ID AND p.path_type = 'C' AND p.stat_cd = '0'
              AND a.end_yn = 'N' AND a.rtn_yn = 'N'
              AND NOT EXISTS (
                  SELECT 1 FROM TAPDOCPATH q
                  WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort < p.sort AND q.stat_cd <> 'E'
              )
              AND (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND (ISNULL(@p_doc_type, '') = '' OR a.doc_type = @p_doc_type)
              AND (ISNULL(@p_date_from, '') = '' OR a.app_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR a.app_date <= @p_date_to)
              AND (ISNULL(@p_title, '') = '' OR a.app_title LIKE '%' + @p_title + '%')
              AND (ISNULL(@p_req_emp_no, '') = '' OR re.emp_no = @p_req_emp_no)
            ORDER BY a.app_id DESC;
        END
        ELSE IF @p_work_type = 'Q2' -- 부서트리(전체)
        BEGIN
            SELECT dept_id, dept_nm, par_dept_id, dept_type
            FROM TBADEPT
            ORDER BY dept_id;
        END
        ELSE IF @p_work_type = 'Q3' -- 부서별 사원목록
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
        ELSE IF @p_work_type = 'Q6' -- 기안함: 로그인 사용자가 상신한 문서(@p_stat_cd로 미결재/반려/완료 구분)
        BEGIN
            DECLARE @my_emp_id2 BIGINT;
            SELECT @my_emp_id2 = EMP_ID FROM TBAEMP WHERE emp_no = @p_emp_no;

            SELECT a.app_id, a.app_no, a.app_date, a.reg_dt AS req_dt, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   a.end_yn, a.rtn_yn, a.app_stat_cd AS stat_cd, re.emp_nm AS req_emp_nm,
                   ca.emp_nm AS cur_appr_emp_nm, la.emp_nm AS last_appr_emp_nm
            FROM TAPDOC a
            LEFT JOIN TBAEMP re ON re.EMP_ID = a.emp_id
            OUTER APPLY (
                SELECT TOP 1 e.emp_nm
                FROM TAPDOCPATH p
                LEFT JOIN TBAEMP e ON e.EMP_ID = p.emp_id
                WHERE p.app_id = a.app_id AND p.path_type = 'C' AND p.stat_cd = '0'
                  AND NOT EXISTS (
                      SELECT 1 FROM TAPDOCPATH q
                      WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort < p.sort AND q.stat_cd <> 'E'
                  )
                ORDER BY p.sort
            ) ca
            OUTER APPLY (
                SELECT TOP 1 x.emp_nm
                FROM TAPDOCPATH c
                LEFT JOIN TBAEMP x ON x.EMP_ID = c.emp_id
                WHERE c.app_id = a.app_id AND c.path_type = 'C'
                ORDER BY c.sort DESC
            ) la
            WHERE a.emp_id = @my_emp_id2
              AND (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND (ISNULL(@p_doc_type, '') = '' OR a.doc_type = @p_doc_type)
              AND (ISNULL(@p_date_from, '') = '' OR a.app_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR a.app_date <= @p_date_to)
              AND (ISNULL(@p_title, '') = '' OR a.app_title LIKE '%' + @p_title + '%')
              AND (ISNULL(@p_req_emp_no, '') = '' OR re.emp_no = @p_req_emp_no)
              AND (ISNULL(@p_stat_cd, '') = ''
                   OR (@p_stat_cd = 'P' AND a.app_stat_cd IN ('0', '1'))
                   OR a.app_stat_cd = @p_stat_cd)
            ORDER BY a.app_id DESC;
        END
        ELSE IF @p_work_type = 'Q7' -- 결재함(반려/결재완료): 내가 결재라인(C)/수신라인(R)에 포함된 문서(내가 기안한 건 제외)
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.reg_dt AS req_dt, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   mp.path_type, a.end_yn, a.rtn_yn, a.app_stat_cd AS stat_cd, re.emp_nm AS req_emp_nm,
                   ca.emp_nm AS cur_appr_emp_nm, la.emp_nm AS last_appr_emp_nm
            FROM TAPDOC a
            JOIN TBAEMP e ON e.emp_no = @p_emp_no
            LEFT JOIN TBAEMP re ON re.EMP_ID = a.emp_id
            CROSS APPLY (
                SELECT TOP 1 p.path_type
                FROM TAPDOCPATH p
                WHERE p.app_id = a.app_id AND p.emp_id = e.EMP_ID AND p.path_type IN ('C', 'R')
                ORDER BY CASE p.path_type WHEN 'C' THEN 0 ELSE 1 END
            ) mp
            OUTER APPLY (
                SELECT TOP 1 x.emp_nm
                FROM TAPDOCPATH c
                LEFT JOIN TBAEMP x ON x.EMP_ID = c.emp_id
                WHERE c.app_id = a.app_id AND c.path_type = 'C' AND c.stat_cd = '0'
                  AND NOT EXISTS (
                      SELECT 1 FROM TAPDOCPATH q
                      WHERE q.app_id = c.app_id AND q.path_type = 'C' AND q.sort < c.sort AND q.stat_cd <> 'E'
                  )
                ORDER BY c.sort
            ) ca
            OUTER APPLY (
                SELECT TOP 1 x.emp_nm
                FROM TAPDOCPATH c
                LEFT JOIN TBAEMP x ON x.EMP_ID = c.emp_id
                WHERE c.app_id = a.app_id AND c.path_type = 'C'
                ORDER BY c.sort DESC
            ) la
            WHERE a.emp_id <> e.EMP_ID
              AND (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND (ISNULL(@p_doc_type, '') = '' OR a.doc_type = @p_doc_type)
              AND (ISNULL(@p_date_from, '') = '' OR a.app_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR a.app_date <= @p_date_to)
              AND (ISNULL(@p_title, '') = '' OR a.app_title LIKE '%' + @p_title + '%')
              AND (ISNULL(@p_req_emp_no, '') = '' OR re.emp_no = @p_req_emp_no)
              AND (ISNULL(@p_stat_cd, '') = ''
                   OR (@p_stat_cd = 'P' AND a.app_stat_cd IN ('0', '1'))
                   OR a.app_stat_cd = @p_stat_cd)
            ORDER BY a.app_id DESC;
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

/* ---------- 문서유형 -> 문서등록 화면 / 기안서 작성 분류 ---------- */
UPDATE TSMMINOR SET rel_cd1 = 'AP.frmNameCardReq', rel_cd2 = N'인사/총무' WHERE major_cd = 'AP0002' AND minor_cd = 'NAMECARD' AND rel_cd1 IS NULL;
UPDATE TSMMINOR SET rel_cd1 = 'MA.frmPoReq',      rel_cd2 = N'구매'      WHERE major_cd = 'AP0002' AND minor_cd = 'POREQ'    AND rel_cd1 IS NULL;
UPDATE TSMMINOR SET rel_cd1 = 'MA.frmPo',         rel_cd2 = N'구매'      WHERE major_cd = 'AP0002' AND minor_cd = 'PO'       AND rel_cd1 IS NULL;
UPDATE TSMMINOR SET rel_cd1 = 'SA.frmSo',         rel_cd2 = N'영업'      WHERE major_cd = 'AP0002' AND minor_cd = 'SO'       AND rel_cd1 IS NULL;
GO
