-- 157: 공지사항(TSMBOARD) 신설 + 홈화면 실데이터 연동(공지사항/전자결재 기안·승인대상)
-- 적용 전 검토 필요(다른 마이그레이션과 동일한 규칙) - 자동 실행되지 않는다.

-- ============================================================
-- 1) TSMBOARD - 공지사항
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMBOARD')
BEGIN
    CREATE TABLE TSMBOARD (
        BOARD_ID        BIGINT IDENTITY(1,1) NOT NULL,
        ACC_ID          BIGINT NOT NULL,
        TITLE           NVARCHAR(200) NOT NULL,
        CONTENT         NVARCHAR(MAX) NULL,
        EMP_ID          BIGINT NULL,             -- 작성자(TBAEMP) - 저장프로시저가 p_user_id로 역매핑해서 채움
        IMPORTANT_YN    VARCHAR(1) NOT NULL CONSTRAINT DF_TSMBOARD_IMPORTANT_YN DEFAULT('N'),
        USE_YN          VARCHAR(1) NOT NULL CONSTRAINT DF_TSMBOARD_USE_YN DEFAULT('Y'),
        REG_USER_ID     VARCHAR(30) NULL,
        REG_DT          DATETIME NULL,
        REG_PC          NVARCHAR(400) NULL,
        UPT_USER_ID     VARCHAR(30) NULL,
        UPT_DT          DATETIME NULL,
        UPT_PC          NVARCHAR(400) NULL,
        CONSTRAINT PK_TSMBOARD PRIMARY KEY (BOARD_ID)
    );
END
GO

-- ============================================================
-- 2) USP_SM_BOARD_Q - 목록 조회
--    p_top_n: NULL이면 전체, 값이 있으면 상위 N건만(홈화면 위젯이 씀 - 중요공지 먼저, 최신순)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_BOARD_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_title VARCHAR(200) = NULL,
    @p_use_yn VARCHAR(1) = NULL,
    @p_top_n INT = NULL,
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
            SELECT TOP (ISNULL(@p_top_n, 2147483647))
                a.board_id,
                a.acc_id,
                a.title,
                a.content,
                a.emp_id,
                e.emp_nm,
                a.important_yn,
                a.use_yn,
                a.reg_user_id,
                a.reg_dt,
                a.upt_user_id,
                a.upt_dt
            FROM TSMBOARD a
            LEFT JOIN TBAEMP e ON e.EMP_ID = a.emp_id
            WHERE (@p_title IS NULL OR @p_title = '' OR a.title LIKE '%' + @p_title + '%')
              AND (@p_use_yn IS NULL OR @p_use_yn = '' OR a.use_yn = @p_use_yn)
            ORDER BY a.important_yn DESC, a.reg_dt DESC;
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

-- ============================================================
-- 3) USP_SM_BOARD_S - 신규/수정/삭제
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_BOARD_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_board_id BIGINT = NULL,        -- U/D일 때 필수 - N일때는 안 씀(신규 채번)
    @p_acc_id BIGINT = NULL,
    @p_title NVARCHAR(200) = NULL,
    @p_content NVARCHAR(MAX) = NULL,
    @p_important_yn VARCHAR(1) = NULL,
    @p_use_yn VARCHAR(1) = NULL,
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
        DECLARE @my_emp_id BIGINT;
        SELECT @my_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMBOARD (
                acc_id, title, content, emp_id, important_yn, use_yn,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_title, @p_content, @my_emp_id, ISNULL(@p_important_yn, 'N'), ISNULL(@p_use_yn, 'Y'),
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMBOARD SET
                acc_id = @p_acc_id,
                title = @p_title,
                content = @p_content,
                important_yn = ISNULL(@p_important_yn, 'N'),
                use_yn = ISNULL(@p_use_yn, 'Y'),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE board_id = @p_board_id;
            SET @GeneratedCode = CAST(@p_board_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMBOARD WHERE board_id = @p_board_id;
            SET @GeneratedCode = CAST(@p_board_id AS VARCHAR(20));
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

-- ============================================================
-- 4) USP_AP_APPR_Q에 Q6(기안문서 - 내가 상신한 문서) 추가
--    기존 정의(Q/Q1/Q2/Q3/Q4/Q5)는 살아있는 정의를 그대로 유지 - 그 위에 Q6 분기만 더한다
--    (Migration regression check 컨벤션 - 실제 운영 중인 정의를 먼저 확인 후 CREATE OR ALTER).
-- ============================================================
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
        IF @p_work_type = 'Q' -- 특정 문서 1건의 전체이력(헤더 전체행 + 그 라인, 헤더+라인 순서 유지)
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
        ELSE IF @p_work_type = 'Q1' -- 결재대기: 로그인 사용자가 지금 처리해야 함(자기 차례) 목록
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
        ELSE IF @p_work_type = 'Q3' -- 사원목록(@p_dept_id 있으면 그 부서만, NULL이면 전체 - 사원트리용)
        BEGIN
            SELECT EMP_ID, emp_no, emp_nm, job_grade, DEPT_ID
            FROM TBAEMP
            WHERE @p_dept_id IS NULL OR DEPT_ID = @p_dept_id
            ORDER BY emp_no;
        END
        ELSE IF @p_work_type = 'Q4' -- 내 결재선 목록
        BEGIN
            DECLARE @my_emp_id BIGINT;
            SELECT @my_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            SELECT route_id, route_nm
            FROM TAPROUTE
            WHERE emp_id = @my_emp_id AND use_yn = 'Y'
            ORDER BY route_nm;
        END
        ELSE IF @p_work_type = 'Q5' -- 결재선 상세
        BEGIN
            SELECT d.route_id, d.sort, d.emp_id, e.emp_no, e.emp_nm, d.path_type
            FROM TAPROUTEDETAIL d
            LEFT JOIN TBAEMP e ON e.EMP_ID = d.emp_id
            WHERE d.route_id = @p_route_id
            ORDER BY d.path_type, d.sort;
        END
        ELSE IF @p_work_type = 'Q6' -- 기안문서: 로그인 사용자가 상신한 문서(홈화면 "기안문서" 탭용, 최근 순)
        BEGIN
            DECLARE @my_emp_id2 BIGINT;
            SELECT @my_emp_id2 = EMP_ID FROM TBAEMP WHERE emp_no = @p_emp_no;

            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   a.end_yn, a.rtn_yn, a.stat_cd
            FROM TAPDOC a
            WHERE a.emp_id = @my_emp_id2
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

-- ============================================================
-- 5) 메뉴 등록 - 공지사항등록(SM, 시스템운영관리 하위)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SM' AND SCREEN_CLASS_NM = 'frmBoard')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'공지사항등록', 25, 2, 'FORM', 'SM', 'frmBoard', 'USP_SM_BOARD_', 40, 'Y', SUSER_SNAME(), GETDATE());
END
GO
