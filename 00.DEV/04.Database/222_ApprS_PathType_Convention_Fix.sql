-- 결재라인 코드 통일 + 실서버 드리프트 복구(2026-09-29).
--
-- 발견된 상황: USP_AP_APPR_S가 마이그레이션 파일(209번) 밖에서 직접 손을 타 있었다 -
-- 결재라인 path_type을 'A'->'C', 수신라인을 'F'->'R', 대기/완료 상태를 'N'/'Y'->'0'/'E'로
-- 바꿔서 AP0001 4단계 코드(0/1/E/R)와 값을 맞추려던 작업으로 보인다(USP_AP_APPR_S_POREQ에
-- "결재자가 나 혼자면 상신 즉시 확정" 같은 새 로직도 같이 추가돼 있어 의도적인 작업임을 알 수
-- 있다). 문제는 세 가지: 1) USP_AP_APPR_Q(조회)와 popApp.cs/frmApprRoute.cs(클라이언트)는
-- 그대로 옛 코드('A'/'N'/'Y')를 찾고 있어서 상신자 행이 DB엔 있는데 화면엔 하나도 안 보였다.
-- 2) @p_work_type 분기에서 '승인'과 '승인취소'가 둘 다 'C'로 겹쳐(원래 승인은 'A') 승인취소
-- 분기가 죽은 코드가 됐다. 3) 상신자 본인 행을 넣을 때 stat_cd를 '0'(대기)으로 넣어서
-- "상신자만 있으면 즉시 결재완료"(2026-09-22 확정 규칙, 180번 마이그레이션) 자체가 깨졌다.
--
-- 이 마이그레이션은 새 코드(C/R, 0/E)로 방향을 확정하고, USP_AP_APPR_Q/popApp.cs/
-- frmApprRoute.cs를 거기 맞추면서 위 세 가지 버그만 고친다. 문서종류별 후처리
-- 프로시저(USP_AP_APPR_S_NAMECARD/POREQ/PO)와 USP_AP_APPR_DOC은 이미 새 코드와 일관되게
-- 맞춰져 있어 손대지 않는다.

/* ---------- 1) USP_AP_APPR_S: 승인 분기 'C'->'A' 복원 + 상신자 자동승인 stat_cd 수정 ---------- */
CREATE OR ALTER PROCEDURE USP_AP_APPR_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_app_id BIGINT = NULL,
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_app_title NVARCHAR(500) = NULL,
    @p_app_text NVARCHAR(4000) = NULL,
    @p_form_id VARCHAR(30) = NULL,
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
            DECLARE @req_dept_id BIGINT, @req_emp_id BIGINT, @req_acc_id BIGINT, @new_app_no VARCHAR(20);
            SELECT @req_emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
            SELECT @req_dept_id = DEPT_ID, @req_acc_id = acc_id FROM TBAEMP WHERE EMP_ID = @req_emp_id;

            EXEC SSP_SYS_GetAutoKey 'TAPDOC', 'app_no', @req_acc_id, @new_app_no OUTPUT;

            -- app_stat_cd는 일단 '0'으로 넣는다 - 결재라인에 상신자 외 아무도 없으면 바로 아래
            -- 재계산에서 즉시 'E'로 바뀐다(2026-09-22 요청: "상신자만 포함한 상태에서 상신하면
            -- 결재완료").
            INSERT INTO TAPDOC (
                app_no, app_date, app_title, app_text, form_id, doc_type, doc_id, doc_no,
                acc_id, dept_id, emp_id, end_yn, rtn_yn, app_stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @new_app_no, CONVERT(VARCHAR(8), GETDATE(), 112), @p_app_title, @p_app_text, @p_form_id, @p_doc_type, @p_doc_id, @p_doc_no,
                @req_acc_id, @req_dept_id, @req_emp_id, 'N', 'N', '0',
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_app_id = SCOPE_IDENTITY();

            -- 기안자 본인 = 결재라인('C') sort=1, 상신 자체가 승인완료 데이터 - stat_cd를
            -- 'E'(완료)로 즉시 넣는다(여기가 '0'으로 잘못 들어가던 부분 - 버그 수정).
            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd, app_dt,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, 1, app_no, @p_doc_type, @p_doc_id, @p_doc_no, 1, 'C', @req_emp_id, 'E', GETDATE(),
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            -- 지금 시점엔 결재라인에 상신자(sort=1, 이미 'E')뿐이라 대기 중인 결재자가 없다 -
            -- 그대로면 곧바로 'E'. 팝업(popApp.SubmitAsync)이 이 뒤에 ADDPATH를 호출해 실제
            -- 결재자를 추가하면, 그 안에서 다시 계산되어 '0'으로 내려간다.
            UPDATE TAPDOC
               SET app_stat_cd = CASE
                   WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'C' AND p.stat_cd <> 'E') THEN 'E'
                   ELSE '0'
               END
             WHERE app_id = @p_app_id;

            EXEC USP_AP_APPR_DOC @p_event = 'SUBMIT', @p_doc_type = @p_doc_type, @p_doc_id = @p_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'ADDPATH' -- 결재라인('C') 또는 수신라인('R')에 한 명 추가
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
            IF @p_path_type = 'C'
                SELECT @nextSort = ISNULL(MAX(sort), 0) + 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C';
            ELSE
                SET @nextSort = 0;

            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, @nextSerl, app_no, doc_type, doc_id, doc_no, @nextSort, @p_path_type, @target_emp_id, '0',
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            UPDATE TAPDOC
               SET app_stat_cd = CASE
                   WHEN rtn_yn = 'Y' THEN 'R'
                   WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'C' AND p.stat_cd = '0') THEN 'E'
                   WHEN EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'C' AND p.stat_cd = 'E' AND p.sort > 1) THEN '1'
                   ELSE '0'
               END
             WHERE app_id = @p_app_id;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'A' -- 승인 (결재라인, 순서 게이트 적용) - 라이브에서 'C'로 잘못
                                    -- 바뀌어 승인취소('C')와 겹쳐있던 것을 원래 코드로 되돌림.
        BEGIN
            DECLARE @apprv_emp_id BIGINT;
            SELECT @apprv_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND path_type = 'C' AND stat_cd = '0')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @apprv_emp_id AND p.path_type = 'C' AND p.stat_cd = '0'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort < p.sort AND q.stat_cd <> 'E')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'아직 앞 순번 결재가 완료되지 않았습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'E', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND path_type = 'C' AND stat_cd = '0';

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C' AND stat_cd = '0')
            BEGIN
                UPDATE TAPDOC
                   SET end_yn = 'Y', end_emp_id = @apprv_emp_id, end_dt = GETDATE(), app_stat_cd = 'E',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;

                DECLARE @fin_doc_type VARCHAR(10), @fin_doc_id BIGINT;
                SELECT @fin_doc_type = doc_type, @fin_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                EXEC USP_AP_APPR_DOC @p_event = 'APPROVE_END', @p_doc_type = @fin_doc_type, @p_doc_id = @fin_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;
            END
            ELSE
            BEGIN
                UPDATE TAPDOC
                   SET app_stat_cd = '1', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'R' -- 반려 (자기 차례일 때만, 승인과 같은 게이트)
        BEGIN
            DECLARE @rej_emp_id BIGINT;
            SELECT @rej_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND path_type = 'C' AND stat_cd = '0')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @rej_emp_id AND p.path_type = 'C' AND p.stat_cd = '0'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort < p.sort AND q.stat_cd <> 'E')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'아직 앞 순번 결재가 완료되지 않았습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'R', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND path_type = 'C' AND stat_cd = '0';

            UPDATE TAPDOC
               SET rtn_yn = 'Y', rtn_emp_id = @rej_emp_id, rtn_dt = GETDATE(), app_stat_cd = 'R',
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id;

            DECLARE @rej_doc_type VARCHAR(10), @rej_doc_id BIGINT;
            SELECT @rej_doc_type = doc_type, @rej_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;
            EXEC USP_AP_APPR_DOC @p_event = 'REJECT', @p_doc_type = @rej_doc_type, @p_doc_id = @rej_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'C' -- 승인취소 (원래 코드 - 이제 승인('A')과 안 겹침)
        BEGIN
            DECLARE @undo_emp_id BIGINT;
            SELECT @undo_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'C' AND stat_cd = 'E')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'취소할 승인 내역을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @undo_emp_id AND p.path_type = 'C' AND p.stat_cd = 'E'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort > p.sort AND q.stat_cd = 'E')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이후 순번이 이미 승인하여 취소할 수 없습니다.';
                RETURN;
            END

            DECLARE @drafter_emp_id BIGINT;
            SELECT @drafter_emp_id = emp_id FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C' AND sort = 1;

            IF @undo_emp_id = @drafter_emp_id
            BEGIN
                -- 기안자(상신자) 본인의 승인취소 = 결재상신 자체를 취소한다(2026-09-22 요청) -
                -- 결재라인/문서를 통째로 지우고, 원본 문서의 app_id/app_no까지 비운다.
                DECLARE @reset_doc_type VARCHAR(10), @reset_doc_id BIGINT;
                SELECT @reset_doc_type = doc_type, @reset_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                DELETE FROM TAPDOCPATH WHERE app_id = @p_app_id;
                DELETE FROM TAPDOC WHERE app_id = @p_app_id;

                EXEC USP_AP_APPR_DOC @p_event = 'RESET', @p_doc_type = @reset_doc_type, @p_doc_id = @reset_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;
            END
            ELSE
            BEGIN
                UPDATE TAPDOCPATH
                   SET stat_cd = '0', app_dt = NULL, remark = NULL,
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'C' AND stat_cd = 'E';

                IF EXISTS (SELECT 1 FROM TAPDOC WHERE app_id = @p_app_id AND end_yn = 'Y')
                BEGIN
                    UPDATE TAPDOC
                       SET end_yn = 'N', end_emp_id = NULL, end_dt = NULL,
                           upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                     WHERE app_id = @p_app_id;

                    DECLARE @undo_doc_type VARCHAR(10), @undo_doc_id BIGINT;
                    SELECT @undo_doc_type = doc_type, @undo_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                    EXEC USP_AP_APPR_DOC @p_event = 'UNDO_END', @p_doc_type = @undo_doc_type, @p_doc_id = @undo_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;
                END

                UPDATE TAPDOC
                   SET app_stat_cd = CASE
                       WHEN rtn_yn = 'Y' THEN 'R'
                       WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'C' AND p.stat_cd = 'E') THEN 'E'
                       WHEN EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'C' AND p.stat_cd = 'E' AND p.sort > 1) THEN '1'
                       ELSE '0'
                   END
                 WHERE app_id = @p_app_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'F' -- 수신확인 (게이트 없음, 결재 흐름에 영향 없음) - 완료 코드도 'E'로 통일
        BEGIN
            DECLARE @ack_emp_id BIGINT;
            SELECT @ack_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            UPDATE TAPDOCPATH
               SET stat_cd = 'E', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @ack_emp_id AND path_type = 'R' AND stat_cd = '0';

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
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- 2) USP_AP_APPR_Q: Q1/Q6 필터를 새 코드('C'/'0'/'E')로 ---------- */
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
        ELSE IF @p_work_type = 'Q1' -- 결재함: 로그인 사용자가 지금 처리해야 할(자기 차례인) 대기건
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   p.serl, p.sort, p.path_type, e2.emp_nm AS req_emp_nm
            FROM TAPDOCPATH p
            JOIN TAPDOC a ON a.app_id = p.app_id
            JOIN TBAEMP e ON e.emp_no = @p_emp_no
            LEFT JOIN TBAEMP e2 ON e2.EMP_ID = a.emp_id
            WHERE p.emp_id = e.EMP_ID AND p.path_type = 'C' AND p.stat_cd = '0'
              AND a.end_yn = 'N' AND a.rtn_yn = 'N'
              AND NOT EXISTS (
                  SELECT 1 FROM TAPDOCPATH q
                  WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort < p.sort AND q.stat_cd <> 'E'
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
        ELSE IF @p_work_type = 'Q6' -- 기안문서: 로그인 사용자가 상신한 문서(홈화면 "기안문서" 탭용)
        BEGIN
            DECLARE @my_emp_id2 BIGINT;
            SELECT @my_emp_id2 = EMP_ID FROM TBAEMP WHERE emp_no = @p_emp_no;

            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   a.end_yn, a.rtn_yn, a.app_stat_cd AS stat_cd, ca.emp_nm AS cur_appr_emp_nm
            FROM TAPDOC a
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

/* ---------- 3) 데이터 보정: 버그 상태로 저장된 기존 제출 건 복구 ---------- */
-- 기안자 본인(sort=1)이 유일한 결재라인인데 아직 '0'(대기)로 남아있는 건 = 위 버그로 자동승인이
-- 안 걸린 건들. 지금 규칙대로 즉시 승인완료 처리하고, 문서별 확정 상태도 같이 맞춘다.
DECLARE @fixApps TABLE (app_id BIGINT PRIMARY KEY);
INSERT INTO @fixApps (app_id)
SELECT p.app_id
FROM TAPDOCPATH p
WHERE p.path_type = 'C' AND p.sort = 1 AND p.stat_cd = '0'
  AND NOT EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'C' AND q.sort > 1);

UPDATE p
   SET stat_cd = 'E'
FROM TAPDOCPATH p
WHERE p.app_id IN (SELECT app_id FROM @fixApps) AND p.path_type = 'C' AND p.sort = 1;

UPDATE d
   SET end_yn = 'Y', end_emp_id = d.emp_id, end_dt = GETDATE(), app_stat_cd = 'E'
FROM TAPDOC d
WHERE d.app_id IN (SELECT app_id FROM @fixApps);

UPDATE m
   SET m.stat_cd = 'C'
FROM TMAPOREQM m
JOIN TAPDOC d ON d.app_id = m.app_id
WHERE d.app_id IN (SELECT app_id FROM @fixApps) AND d.doc_type = 'POREQ';

UPDATE m
   SET m.stat_cd = 'C'
FROM TMAPOM m
JOIN TAPDOC d ON d.app_id = m.app_id
WHERE d.app_id IN (SELECT app_id FROM @fixApps) AND d.doc_type = 'PO';

UPDATE n
   SET n.stat_cd = 'C'
FROM THRNAMECARDREQ n
JOIN TAPDOC d ON d.app_id = n.app_id
WHERE d.app_id IN (SELECT app_id FROM @fixApps) AND d.doc_type = 'NAMECARD';
GO
