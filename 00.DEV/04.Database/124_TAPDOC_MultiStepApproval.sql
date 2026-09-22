-- 결재승인 프로세스 2차: 공용 "전자결재" 화면 지원.
-- 1차(123번)는 결재자 1명 고정이었다. 이제 TAPDOCPATH.sort를 실제 승인 순서로 쓰고, path_type으로
-- 결재(A, 순서 게이트 적용)와 수신/참조(F, 게이트 없음 - 단순 확인)를 구분한다. 결재라인의 맨 앞
-- (sort=1)은 상신자 본인이며, 상신(N) 자체가 그 사람의 승인완료 데이터로 즉시 들어간다(사장님 지시).
--
-- 승인자 식별은 여전히 @p_user_id(로그인 세션, GenericDataRepository.SaveAsync가 항상 서버에서
-- 채움)로 TSMUSER->TBAEMP를 타고 서버가 직접 해석한다(123번과 동일 원칙, 클라이언트가 못 속임).
-- 결재라인에 추가하는 "대상자"(@p_target_emp_no)는 신원 주장이 아니라 라우팅 대상 선택이라
-- 클라이언트가 보내도 안전하다(123번 @p_approver_emp_no와 같은 논리).

/* ---------- TAPROUTE/TAPROUTEDETAIL: 개인별 저장된 결재경로 ---------- */
IF OBJECT_ID('TAPROUTEDETAIL') IS NOT NULL DROP TABLE TAPROUTEDETAIL;
IF OBJECT_ID('TAPROUTE') IS NOT NULL DROP TABLE TAPROUTE;
GO

CREATE TABLE TAPROUTE (
    route_id     BIGINT IDENTITY(1,1) NOT NULL,
    emp_id       BIGINT         NOT NULL,
    route_nm     NVARCHAR(100)  NOT NULL,
    use_yn       VARCHAR(1)     NOT NULL DEFAULT ('Y'),
    reg_user_id  VARCHAR(30)    NULL,
    reg_dt       DATETIME       NULL,
    reg_pc       NVARCHAR(200)  NULL,
    upt_user_id  VARCHAR(30)    NULL,
    upt_dt       DATETIME       NULL,
    upt_pc       NVARCHAR(200)  NULL,
    CONSTRAINT PK_TAPROUTE PRIMARY KEY CLUSTERED (route_id)
);
GO

CREATE TABLE TAPROUTEDETAIL (
    route_id     BIGINT       NOT NULL,
    sort         INT          NOT NULL,
    emp_id       BIGINT       NOT NULL,
    path_type    VARCHAR(10)  NOT NULL,
    CONSTRAINT PK_TAPROUTEDETAIL PRIMARY KEY CLUSTERED (route_id, sort)
);
GO

/* ---------- USP_AP_APPR_Q: 부서트리/부서별사원/결재경로 조회 추가 ---------- */
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
        IF @p_work_type = 'Q' -- 특정 문서 1건의 결재이력(헤더 결과셋 + 상세 결과셋, 결재+수신 전부)
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.app_text, a.doc_type, a.doc_id,
                   a.doc_no, a.end_yn, a.end_dt, a.rtn_yn, a.rtn_dt, a.stat_cd, a.emp_id, e.emp_nm
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
        ELSE IF @p_work_type = 'Q1' -- 결재함: 로그인 사용자가 지금 처리해야 할(자기 차례인) 대기건
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no,
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
        ELSE IF @p_work_type = 'Q3' -- 부서별 사원목록
        BEGIN
            SELECT EMP_ID, emp_no, emp_nm, job_grade
            FROM TBAEMP
            WHERE DEPT_ID = @p_dept_id
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

/* ---------- USP_AP_APPR_S: 상신/라인추가/승인/반려/승인취소/수신확인 ---------- */
CREATE OR ALTER PROCEDURE USP_AP_APPR_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_app_id BIGINT = NULL,
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_app_title NVARCHAR(500) = NULL,
    @p_app_text NVARCHAR(4000) = NULL,
    @p_target_emp_no VARCHAR(20) = NULL,
    @p_path_type VARCHAR(10) = NULL,
    @p_opinion NVARCHAR(1000) = NULL,
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
        IF @p_work_type = 'N' -- 결재상신: TAPDOC 헤더 + TAPDOCPATH sort=1(기안자 본인, 즉시 승인완료)
        BEGIN
            DECLARE @req_dept_id BIGINT, @req_emp_id BIGINT, @req_acc_id BIGINT;
            SELECT @req_emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
            SELECT @req_dept_id = DEPT_ID, @req_acc_id = acc_id FROM TBAEMP WHERE EMP_ID = @req_emp_id;

            INSERT INTO TAPDOC (
                app_no, app_date, app_title, app_text, doc_type, doc_id, doc_no,
                acc_id, dept_id, emp_id, end_yn, rtn_yn, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', CONVERT(VARCHAR(8), GETDATE(), 112), @p_app_title, @p_app_text, @p_doc_type, @p_doc_id, @p_doc_no,
                @req_acc_id, @req_dept_id, @req_emp_id, 'N', 'N', 'P',
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_app_id = SCOPE_IDENTITY();
            UPDATE TAPDOC
               SET app_no = 'AP' + RIGHT('00000000' + CAST(@p_app_id AS VARCHAR(8)), 8)
             WHERE app_id = @p_app_id;

            -- 기안자 본인 = 결재라인 sort=1, 상신 자체가 승인완료 데이터.
            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd, app_dt,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, 1, app_no, @p_doc_type, @p_doc_id, @p_doc_no, 1, 'A', @req_emp_id, 'Y', GETDATE(),
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            IF @p_doc_type = 'NAMECARD'
                UPDATE THRNAMECARDREQ
                   SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
                 WHERE req_id = @p_doc_id;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'ADDPATH' -- 결재라인('A') 또는 수신라인('F')에 한 명 추가
        BEGIN
            DECLARE @target_emp_id BIGINT;
            SELECT @target_emp_id = EMP_ID FROM TBAEMP WHERE emp_no = @p_target_emp_no;
            IF @target_emp_id IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 사원 정보를 찾을 수 없습니다.';
                RETURN;
            END

            DECLARE @nextSerl INT, @nextSort INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TAPDOCPATH WHERE app_id = @p_app_id;
            IF @p_path_type = 'A'
                SELECT @nextSort = ISNULL(MAX(sort), 0) + 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'A';
            ELSE
                SET @nextSort = 0;

            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, @nextSerl, app_no, doc_type, doc_id, doc_no, @nextSort, @p_path_type, @target_emp_id, 'N',
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'A' -- 승인 (결재라인, 순서 게이트 적용)
        BEGIN
            DECLARE @apprv_emp_id BIGINT;
            SELECT @apprv_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND path_type = 'A' AND stat_cd = 'N')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @apprv_emp_id AND p.path_type = 'A' AND p.stat_cd = 'N'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort < p.sort AND q.stat_cd <> 'Y')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'아직 앞 순번 결재가 완료되지 않았습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'Y', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND path_type = 'A' AND stat_cd = 'N';

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'A' AND stat_cd = 'N')
            BEGIN
                UPDATE TAPDOC
                   SET end_yn = 'Y', end_emp_id = @apprv_emp_id, end_dt = GETDATE(), stat_cd = 'Y',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;

                DECLARE @fin_doc_type VARCHAR(10), @fin_doc_id BIGINT;
                SELECT @fin_doc_type = doc_type, @fin_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                IF @fin_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET stat_cd = 'C' WHERE req_id = @fin_doc_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'R' -- 반려 (자기 차례일 때만, 승인과 같은 게이트)
        BEGIN
            DECLARE @rej_emp_id BIGINT;
            SELECT @rej_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND path_type = 'A' AND stat_cd = 'N')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @rej_emp_id AND p.path_type = 'A' AND p.stat_cd = 'N'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort < p.sort AND q.stat_cd <> 'Y')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'아직 앞 순번 결재가 완료되지 않았습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'R', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND path_type = 'A' AND stat_cd = 'N';

            UPDATE TAPDOC
               SET rtn_yn = 'Y', rtn_emp_id = @rej_emp_id, rtn_dt = GETDATE(), stat_cd = 'R',
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id;

            -- 반려는 THRNAMECARDREQ.stat_cd(진행상태)를 건드리지 않는다 - 123번과 동일.
            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'C' -- 승인취소: 방금 내가 한 승인을 되돌림(뒷사람이 이미 승인했으면 불가)
        BEGIN
            DECLARE @undo_emp_id BIGINT;
            SELECT @undo_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'A' AND stat_cd = 'Y')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'취소할 승인 내역을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @undo_emp_id AND p.path_type = 'A' AND p.stat_cd = 'Y'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort > p.sort AND q.stat_cd = 'Y')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이후 순번이 이미 승인하여 취소할 수 없습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'N', app_dt = NULL, remark = NULL,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'A' AND stat_cd = 'Y';

            IF EXISTS (SELECT 1 FROM TAPDOC WHERE app_id = @p_app_id AND end_yn = 'Y')
            BEGIN
                UPDATE TAPDOC
                   SET end_yn = 'N', end_emp_id = NULL, end_dt = NULL, stat_cd = 'P',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;

                DECLARE @undo_doc_type VARCHAR(10), @undo_doc_id BIGINT;
                SELECT @undo_doc_type = doc_type, @undo_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                IF @undo_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET stat_cd = '0' WHERE req_id = @undo_doc_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'F' -- 수신확인 (게이트 없음, 결재 흐름에 영향 없음)
        BEGIN
            DECLARE @ack_emp_id BIGINT;
            SELECT @ack_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            UPDATE TAPDOCPATH
               SET stat_cd = 'Y', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @ack_emp_id AND path_type = 'F' AND stat_cd = 'N';

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'확인할 수신 건을 찾을 수 없습니다.';
                RETURN;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
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

/* ---------- USP_AP_ROUTE_S: 개인별 저장된 결재경로 관리 ---------- */
CREATE OR ALTER PROCEDURE USP_AP_ROUTE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_route_nm NVARCHAR(100) = NULL,
    @p_target_emp_no VARCHAR(20) = NULL,
    @p_path_type VARCHAR(10) = NULL,
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
        IF @p_work_type = 'N' -- 경로 헤더 신규
        BEGIN
            DECLARE @owner_emp_id BIGINT;
            SELECT @owner_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            INSERT INTO TAPROUTE (emp_id, route_nm, use_yn, reg_user_id, reg_dt, reg_pc)
            VALUES (@owner_emp_id, @p_route_nm, 'Y', @p_user_id, GETDATE(), @p_client_pc);

            SET @p_route_id = SCOPE_IDENTITY();
            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'ADDDETAIL' -- 경로 상세 한 명 추가
        BEGIN
            DECLARE @target_emp_id BIGINT;
            SELECT @target_emp_id = EMP_ID FROM TBAEMP WHERE emp_no = @p_target_emp_no;
            IF @target_emp_id IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 사원 정보를 찾을 수 없습니다.';
                RETURN;
            END

            DECLARE @nextSort INT;
            SELECT @nextSort = ISNULL(MAX(sort), 0) + 1 FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;

            INSERT INTO TAPROUTEDETAIL (route_id, sort, emp_id, path_type)
            VALUES (@p_route_id, @nextSort, @target_emp_id, @p_path_type);

            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D' -- 경로 삭제(소유자 본인만)
        BEGIN
            DECLARE @del_emp_id BIGINT;
            SELECT @del_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            DELETE FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;
            DELETE FROM TAPROUTE WHERE route_id = @p_route_id AND emp_id = @del_emp_id;
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
