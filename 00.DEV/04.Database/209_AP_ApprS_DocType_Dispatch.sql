-- 전자결재 후처리를 문서종류(doc_type)별 프로시저로 분기(2026-09-25).
--
-- 지금까지 USP_AP_APPR_S 하나가 "IF doc_type = 'NAMECARD' ... ELSE IF 'POREQ' ... ELSE IF 'PO'"를 네 군데(상신/최종승인/기안자취소/완료해제)에
-- 반복해서 들고 있었다 - 문서종류가 늘 때마다 그 프로시저를 열어 네 군데를 고쳐야 했다. 이제는 문서종류별로 프로시저를 하나씩 둔다:
--     USP_AP_APPR_S_NAMECARD / USP_AP_APPR_S_POREQ / USP_AP_APPR_S_PO  (새 문서종류는 USP_AP_APPR_S_{doc_type} 하나만 만들면 됨)
-- USP_AP_APPR_S는 결재 흐름(TAPDOC/TAPDOCPATH)만 처리하고, 상태가 바뀔 때 USP_AP_APPR_DOC을 부른다. USP_AP_APPR_DOC이 doc_type으로 위 프로시저를
-- 찾아 실행한다(프로시저가 없는 doc_type이면 아무것도 안 한다 - 결재 자체는 그대로 동작).
--
-- 이벤트(@p_event): SUBMIT(상신) / APPROVE_END(최종 승인 완료) / REJECT(반려) / UNDO_END(완료 후 승인취소) / RESET(기안자 취소로 문서 초기화)
-- 문서별 프로시저가 THROW 50001, N'사유'로 올린 오류는 화면에 그 문구로 보인다(USP_AP_APPR_S의 CATCH가 그대로 돌려준다).
-- USP_AP_APPR_S는 180번 정의(라이브와 같음을 확인)에서 문서종류 IF 사슬 네 개를 이 호출로 바꾸고 반려 훅 호출과 오류 문구 전달만 더했다.

CREATE OR ALTER PROCEDURE USP_AP_APPR_DOC
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_type VARCHAR(10),
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- doc_type으로 USP_AP_APPR_S_{doc_type}을 찾아 실행한다. 프로시저 이름을 만들어 쓰므로 영문/숫자/밑줄이 아닌 값은 거른다.
    IF @p_doc_type IS NULL OR @p_doc_type = '' OR @p_doc_type LIKE '%[^A-Za-z0-9_]%' RETURN;

    DECLARE @proc SYSNAME = N'USP_AP_APPR_S_' + @p_doc_type;
    IF OBJECT_ID(@proc, N'P') IS NULL RETURN;

    EXEC @proc @p_event = @p_event, @p_doc_id = @p_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;
END
GO

CREATE OR ALTER PROCEDURE USP_AP_APPR_S_NAMECARD
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 명함신청(THRNAMECARDREQ) 결재 이벤트 후처리 - USP_AP_APPR_S가 USP_AP_APPR_DOC을 거쳐 부른다.
    -- 이 문서만의 규칙(승인 시 다른 테이블 갱신, 반려 시 처리, 검증 등)은 여기에 넣는다. 결재 자체(TAPDOC/TAPDOCPATH)는
    -- USP_AP_APPR_S가 이미 처리한 뒤다. 거절해야 하면 THROW 50001, N'사유' 로 올리면 화면에 그 문구가 그대로 보인다.
    IF @p_event = 'SUBMIT'          -- 결재상신: 문서에 결재번호 연결
        UPDATE THRNAMECARDREQ
           SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
         WHERE req_id = @p_doc_id;
    ELSE IF @p_event = 'APPROVE_END' -- 최종 승인 완료: 진행상태 C(확정)
        UPDATE THRNAMECARDREQ SET stat_cd = 'C' WHERE req_id = @p_doc_id;
    ELSE IF @p_event = 'UNDO_END'    -- 승인 완료 후 승인 취소로 완료가 풀림: 진행상태 0(작성)
        UPDATE THRNAMECARDREQ SET stat_cd = '0' WHERE req_id = @p_doc_id;
    ELSE IF @p_event = 'RESET'       -- 기안자가 결재를 취소해 결재 문서 자체가 삭제됨: 연결 해제 + 진행상태 0
        UPDATE THRNAMECARDREQ SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE req_id = @p_doc_id;
    -- 'REJECT'(반려)는 지금 명함신청에서 하는 일이 없다(문서 진행상태는 그대로) - 필요해지면 여기에 추가.
END
GO

CREATE OR ALTER PROCEDURE USP_AP_APPR_S_POREQ
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 구매요청(TMAPOREQM) 결재 이벤트 후처리 - USP_AP_APPR_S가 USP_AP_APPR_DOC을 거쳐 부른다.
    -- 이 문서만의 규칙(승인 시 다른 테이블 갱신, 반려 시 처리, 검증 등)은 여기에 넣는다. 결재 자체(TAPDOC/TAPDOCPATH)는
    -- USP_AP_APPR_S가 이미 처리한 뒤다. 거절해야 하면 THROW 50001, N'사유' 로 올리면 화면에 그 문구가 그대로 보인다.
    IF @p_event = 'SUBMIT'          -- 결재상신: 문서에 결재번호 연결
        UPDATE TMAPOREQM
           SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
         WHERE req_id = @p_doc_id;
    ELSE IF @p_event = 'APPROVE_END' -- 최종 승인 완료: 진행상태 C(확정)
        UPDATE TMAPOREQM SET stat_cd = 'C' WHERE req_id = @p_doc_id;
    ELSE IF @p_event = 'UNDO_END'    -- 승인 완료 후 승인 취소로 완료가 풀림: 진행상태 0(작성)
        UPDATE TMAPOREQM SET stat_cd = '0' WHERE req_id = @p_doc_id;
    ELSE IF @p_event = 'RESET'       -- 기안자가 결재를 취소해 결재 문서 자체가 삭제됨: 연결 해제 + 진행상태 0
        UPDATE TMAPOREQM SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE req_id = @p_doc_id;
    -- 'REJECT'(반려)는 지금 구매요청에서 하는 일이 없다(문서 진행상태는 그대로) - 필요해지면 여기에 추가.
END
GO

CREATE OR ALTER PROCEDURE USP_AP_APPR_S_PO
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 구매발주(TMAPOM) 결재 이벤트 후처리 - USP_AP_APPR_S가 USP_AP_APPR_DOC을 거쳐 부른다.
    -- 이 문서만의 규칙(승인 시 다른 테이블 갱신, 반려 시 처리, 검증 등)은 여기에 넣는다. 결재 자체(TAPDOC/TAPDOCPATH)는
    -- USP_AP_APPR_S가 이미 처리한 뒤다. 거절해야 하면 THROW 50001, N'사유' 로 올리면 화면에 그 문구가 그대로 보인다.
    IF @p_event = 'SUBMIT'          -- 결재상신: 문서에 결재번호 연결
        UPDATE TMAPOM
           SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
         WHERE po_id = @p_doc_id;
    ELSE IF @p_event = 'APPROVE_END' -- 최종 승인 완료: 진행상태 C(확정)
        UPDATE TMAPOM SET stat_cd = 'C' WHERE po_id = @p_doc_id;
    ELSE IF @p_event = 'UNDO_END'    -- 승인 완료 후 승인 취소로 완료가 풀림: 진행상태 0(작성)
        UPDATE TMAPOM SET stat_cd = '0' WHERE po_id = @p_doc_id;
    ELSE IF @p_event = 'RESET'       -- 기안자가 결재를 취소해 결재 문서 자체가 삭제됨: 연결 해제 + 진행상태 0
        UPDATE TMAPOM SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE po_id = @p_doc_id;
    -- 'REJECT'(반려)는 지금 구매발주에서 하는 일이 없다(문서 진행상태는 그대로) - 필요해지면 여기에 추가.
END
GO

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
            DECLARE @req_dept_id BIGINT, @req_emp_id BIGINT, @req_acc_id BIGINT;
            SELECT @req_emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
            SELECT @req_dept_id = DEPT_ID, @req_acc_id = acc_id FROM TBAEMP WHERE EMP_ID = @req_emp_id;

            -- 채번을 구매요청/발주번호와 같은 공용 프로시저로 위임(2026-09-22) - 자체
            -- 'AP'+SCOPE_IDENTITY() 8자리 순번 방식 대신.
            DECLARE @new_app_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TAPDOC', 'app_no', @req_acc_id, @new_app_no OUTPUT;

            -- app_stat_cd는 일단 '0'으로 넣는다 - 결재라인에 상신자 외 아무도 없으면(팝업에서
            -- 결재자를 한 명도 안 추가했으면) 바로 아래 재계산에서 즉시 'E'로 바뀐다(2026-09-22
            -- 요청: "상신자만 포함한 상태에서 상신하면 결재완료").
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

            -- 기안자 본인 = 결재라인 sort=1, 상신 자체가 승인완료 데이터.
            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd, app_dt,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, 1, app_no, @p_doc_type, @p_doc_id, @p_doc_no, 1, 'A', @req_emp_id, 'Y', GETDATE(),
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            -- 지금 시점엔 결재라인에 상신자(sort=1)뿐이라 대기 중인 결재자가 없다 - 그대로면
            -- 곧바로 'E'. 팝업(popApp.SubmitAsync)이 이 뒤에 ADDPATH를 호출해 실제 결재자를
            -- 추가하면, 그 안에서 다시 계산되어 '0'으로 내려간다.
            UPDATE TAPDOC
               SET app_stat_cd = CASE
                   WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'A' AND p.stat_cd = 'N') THEN 'E'
                   ELSE '0'
               END
             WHERE app_id = @p_app_id;

            -- 문서종류별 후처리는 USP_AP_APPR_S_{doc_type} 프로시저가 한다(USP_AP_APPR_DOC이 doc_type으로 골라 부른다)
            EXEC USP_AP_APPR_DOC @p_event = 'SUBMIT', @p_doc_type = @p_doc_type, @p_doc_id = @p_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;

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

            -- 결재라인('A')에 새로 추가됐다면 대기자가 생긴 것 - app_stat_cd를 다시 계산한다
            -- (예: 상신 직후 'E'였다가 결재자가 추가되면 '0'으로 내려감). 수신라인('F') 추가는
            -- 결재 진행상태와 무관하므로 재계산해도 결과가 그대로다.
            UPDATE TAPDOC
               SET app_stat_cd = CASE
                   WHEN rtn_yn = 'Y' THEN 'R'
                   WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'A' AND p.stat_cd = 'N') THEN 'E'
                   WHEN EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'A' AND p.stat_cd = 'Y' AND p.sort > 1) THEN '1'
                   ELSE '0'
               END
             WHERE app_id = @p_app_id;

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
                -- 전원 승인 완료.
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
                -- 상신자 이후 누군가(이 승인자) 한 명이라도 처리했지만 아직 전원 완료는 아님.
                UPDATE TAPDOC
                   SET app_stat_cd = '1',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;
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
               SET rtn_yn = 'Y', rtn_emp_id = @rej_emp_id, rtn_dt = GETDATE(), app_stat_cd = 'R',
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id;

            -- 반려는 원본 stat_cd(업무 진행상태)를 건드리지 않는다 - NAMECARD/POREQ/PO 전부 동일.
            DECLARE @rej_doc_type VARCHAR(10), @rej_doc_id BIGINT;
            SELECT @rej_doc_type = doc_type, @rej_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;
            EXEC USP_AP_APPR_DOC @p_event = 'REJECT', @p_doc_type = @rej_doc_type, @p_doc_id = @rej_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'C' -- 승인취소
        BEGIN
            DECLARE @undo_emp_id BIGINT;
            SELECT @undo_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'A' AND stat_cd = 'Y')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'취소할 승인 내역을 찾을 수 없습니다.';
                RETURN;
            END

            -- 이후 순번이 이미 승인했으면 취소 불가 - 기안자 본인의 전체 초기화든 중간 결재자의
            -- 자기 단계 취소든 공통으로 적용되는 안전장치(다른 사람이 이미 승인한 걸 뒤엎지 못함).
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

            DECLARE @drafter_emp_id BIGINT;
            SELECT @drafter_emp_id = emp_id FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'A' AND sort = 1;

            IF @undo_emp_id = @drafter_emp_id
            BEGIN
                -- 기안자(상신자) 본인의 승인취소 = 결재상신 자체를 취소한다(2026-09-22 요청) -
                -- 자기 단계만 되돌리는 게 아니라 결재라인/문서를 통째로 지우고, 원본 문서의
                -- app_id/app_no까지 비워서 상신 이전 상태로 완전히 되돌린다.
                DECLARE @reset_doc_type VARCHAR(10), @reset_doc_id BIGINT;
                SELECT @reset_doc_type = doc_type, @reset_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                DELETE FROM TAPDOCPATH WHERE app_id = @p_app_id;
                DELETE FROM TAPDOC WHERE app_id = @p_app_id;

                EXEC USP_AP_APPR_DOC @p_event = 'RESET', @p_doc_type = @reset_doc_type, @p_doc_id = @reset_doc_id, @p_app_id = @p_app_id, @p_user_id = @p_user_id, @p_client_pc = @p_client_pc;
            END
            ELSE
            BEGIN
                -- 중간 결재자 본인의 승인취소 = 자기 결재 단계만 되돌린다(기존 동작 그대로).
                UPDATE TAPDOCPATH
                   SET stat_cd = 'N', app_dt = NULL, remark = NULL,
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'A' AND stat_cd = 'Y';

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
                       WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'A' AND p.stat_cd = 'N') THEN 'E'
                       WHEN EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = @p_app_id AND p.path_type = 'A' AND p.stat_cd = 'Y' AND p.sort > 1) THEN '1'
                       ELSE '0'
                   END
                 WHERE app_id = @p_app_id;
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
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
