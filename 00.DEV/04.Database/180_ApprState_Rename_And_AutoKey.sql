-- 전자결재 개선 일괄 반영(2026-09-22, 사장님 지시 다건):
-- 1) 결재번호(app_no) 채번을 구매요청/발주번호와 동일하게 SSP_SYS_GetAutoKey로 통일.
-- 2) 승인취소(work_type='C')를 기안자 본인이 누르면 결재 자체를 완전히 초기화(TAPDOCPATH/
--    TAPDOC 삭제 + 원본 문서 app_id/app_no까지 NULL) - 중간 결재자의 승인취소는 기존처럼
--    자기 단계만 되돌린다.
-- 3) doc_type 'PO_REQ' -> 'POREQ'로 변경(밑줄 제거).
-- 4) TAPDOC.stat_cd -> app_stat_cd로 개명 + AP0001 표준 코드(0/1/E/R)로 재정의:
--    0=결재상신(제출은 됐지만 상신자 외 아무도 처리 안 함), 1=결재진행중(상신자 외 누군가
--    한 명이라도 승인함, 아직 전원 완료는 아님), E=승인완료(전원 승인 - 상신자만 있는
--    결재라인이면 상신 즉시 여기 해당), R=반려. 매 결재라인 변경(상신/결재자추가/승인/
--    승인취소)마다 다시 계산해서 항상 최신 상태를 반영한다.

-- ============================================================
-- 0) doc_type 데이터 정리: PO_REQ -> POREQ
-- ============================================================
UPDATE TSMMINOR SET minor_cd = 'POREQ' WHERE major_cd = 'AP0002' AND minor_cd = 'PO_REQ';
UPDATE TAPDOC SET doc_type = 'POREQ' WHERE doc_type = 'PO_REQ';
UPDATE TAPDOCPATH SET doc_type = 'POREQ' WHERE doc_type = 'PO_REQ';
GO

-- ============================================================
-- 1) TAPDOC.stat_cd -> app_stat_cd 개명 + 기존 데이터 재계산
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TAPDOC') AND name = 'stat_cd')
BEGIN
    EXEC sp_rename 'TAPDOC.stat_cd', 'app_stat_cd', 'COLUMN';
END
GO

UPDATE d
   SET app_stat_cd = CASE
       WHEN d.rtn_yn = 'Y' THEN 'R'
       WHEN NOT EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = d.app_id AND p.path_type = 'A' AND p.stat_cd = 'N') THEN 'E'
       WHEN EXISTS (SELECT 1 FROM TAPDOCPATH p WHERE p.app_id = d.app_id AND p.path_type = 'A' AND p.stat_cd = 'Y' AND p.sort > 1) THEN '1'
       ELSE '0'
   END
FROM TAPDOC d;
GO

-- ============================================================
-- 2) TAPDOC/app_no 자동채번 등록 - 구매요청(PR)/구매발주(PO)와 같은 프리픽스+YYMM+4자리 형식.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TAPDOC')
BEGIN
    INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
    VALUES ('TAPDOC', N'전자결재', 'AP', 'app_no', 'YYMM', 4, 'SYSTEM', GETDATE());
END
GO

-- ============================================================
-- 3) USP_AP_APPR_S
-- ============================================================
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

            IF @p_doc_type = 'NAMECARD'
                UPDATE THRNAMECARDREQ
                   SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
                 WHERE req_id = @p_doc_id;
            ELSE IF @p_doc_type = 'POREQ'
                UPDATE TMAPOREQM
                   SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
                 WHERE req_id = @p_doc_id;
            ELSE IF @p_doc_type = 'PO'
                UPDATE TMAPOM
                   SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
                 WHERE po_id = @p_doc_id;

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

                IF @fin_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET stat_cd = 'C' WHERE req_id = @fin_doc_id;
                ELSE IF @fin_doc_type = 'POREQ'
                    UPDATE TMAPOREQM SET stat_cd = 'C' WHERE req_id = @fin_doc_id;
                ELSE IF @fin_doc_type = 'PO'
                    UPDATE TMAPOM SET stat_cd = 'C' WHERE po_id = @fin_doc_id;
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

                IF @reset_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE req_id = @reset_doc_id;
                ELSE IF @reset_doc_type = 'POREQ'
                    UPDATE TMAPOREQM SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE req_id = @reset_doc_id;
                ELSE IF @reset_doc_type = 'PO'
                    UPDATE TMAPOM SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE po_id = @reset_doc_id;
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

                    IF @undo_doc_type = 'NAMECARD'
                        UPDATE THRNAMECARDREQ SET stat_cd = '0' WHERE req_id = @undo_doc_id;
                    ELSE IF @undo_doc_type = 'POREQ'
                        UPDATE TMAPOREQM SET stat_cd = '0' WHERE req_id = @undo_doc_id;
                    ELSE IF @undo_doc_type = 'PO'
                        UPDATE TMAPOM SET stat_cd = '0' WHERE po_id = @undo_doc_id;
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
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 4) USP_AP_APPR_Q - a.stat_cd -> a.app_stat_cd(별칭은 그대로 stat_cd 유지, C# DTO 안 건드림)
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
                   a.doc_no, a.form_id, a.end_yn, a.end_dt, a.rtn_yn, a.app_stat_cd AS stat_cd, a.emp_id, e.emp_nm
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
                   a.end_yn, a.rtn_yn, a.app_stat_cd AS stat_cd
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
-- 5) USP_MA_POREQ_Q - t.stat_cd -> t.app_stat_cd (별칭 appr_stat_cd는 그대로)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQ_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_req_no VARCHAR(20) = NULL,
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
            DECLARE @match_req_id BIGINT;
            SELECT TOP 1 @match_req_id = req_id
            FROM TMAPOREQM
            WHERE (@p_req_id IS NULL OR req_id = @p_req_id)
              AND (@p_req_id IS NOT NULL OR @p_req_no IS NULL OR req_no LIKE '%' + @p_req_no + '%')
            ORDER BY req_id DESC;

            -- 0) 헤더
            SELECT
                m.req_id, m.acc_id, a.ACC_NM,
                m.req_no, m.req_date, m.req_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.remark
            FROM TMAPOREQM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.req_id = @match_req_id;

            -- 1) 품목 상세
            SELECT
                dt.req_id, dt.serl, dt.acc_id, dt.req_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.cfm_yn, dt.stop_yn,
                dt.cust_id, c2.cust_nm,
                dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.remark
            FROM TMAPOREQD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBACUST c2 ON c2.cust_id = dt.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.req_id = @match_req_id
            ORDER BY dt.serl;
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
-- 6) USP_MA_POREQLIST_Q - t.stat_cd -> t.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_req_no VARCHAR(20) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
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
                m.req_id, m.req_no, m.req_date, m.req_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TMAPOREQM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE 1 = 1
              AND (@p_req_no IS NULL OR m.req_no LIKE '%' + @p_req_no + '%')
              AND (@p_req_title IS NULL OR m.req_title LIKE '%' + @p_req_title + '%')
            ORDER BY m.req_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.req_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TMAPOREQD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
            WHERE dt.req_id = @p_req_id
            ORDER BY dt.serl;
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
-- 7) USP_MA_PO_Q - t.stat_cd -> t.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_po_no VARCHAR(20) = NULL,
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
            DECLARE @match_po_id BIGINT;
            SELECT TOP 1 @match_po_id = po_id
            FROM TMAPOM
            WHERE (@p_po_id IS NULL OR po_id = @p_po_id)
              AND (@p_po_id IS NOT NULL OR @p_po_no IS NULL OR po_no LIKE '%' + @p_po_no + '%')
            ORDER BY po_id DESC;

            -- 0) 헤더
            SELECT
                m.po_id, m.acc_id, a.ACC_NM,
                m.po_no, m.po_date,
                m.stat_cd, m.po_type, m.po_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.delv_date, m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cmf_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.remark
            FROM TMAPOM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.po_id = @match_po_id;

            -- 1) 품목 상세
            SELECT
                dt.po_id, dt.serl, dt.acc_id, dt.po_no, dt.po_type,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.qc_yn, dt.stock_yn, dt.stock_unit_cd, dt.stock_unit_qty,
                dt.pjt_id, dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.stop_yn, dt.stop_emp_no, dt.stop_remark, dt.remark
            FROM TMAPOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.po_id = @match_po_id
            ORDER BY dt.serl;
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
-- 8) USP_MA_POLIST_Q - t.stat_cd -> t.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_po_no VARCHAR(20) = NULL,
    @p_po_title NVARCHAR(1000) = NULL,
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
                m.po_id, m.po_no, m.po_date, m.po_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TMAPOM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE 1 = 1
              AND (@p_po_no IS NULL OR m.po_no LIKE '%' + @p_po_no + '%')
              AND (@p_po_title IS NULL OR m.po_title LIKE '%' + @p_po_title + '%')
            ORDER BY m.po_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.po_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.amt, dt.total_amt,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TMAPOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
            WHERE dt.po_id = @p_po_id
            ORDER BY dt.serl;
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
-- 9) USP_HR_NAMECARD_Q - a.stat_cd -> a.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_HR_NAMECARD_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_req_no VARCHAR(20) = NULL,
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
            -- 결재상태(appr_stat_cd)는 TAPDOC을 LEFT JOIN해서 그때그때 계산(Pull) - THRNAMECARDREQ
            -- 자체에는 중복 저장하지 않는다. stat_cd(진행상태)만 최종승인 시 USP_AP_APPR_S가 직접 갱신.
            SELECT
                n.req_id, n.req_no, n.dept_id, d.dept_nm, n.emp_id, e.emp_no, e.emp_nm,
                n.job_grade, n.name_kor, n.name_eng, n.dept_kor, n.dept_eng, n.mobile, n.email,
                n.stat_cd, n.app_id, n.app_no, n.remark, n.reg_dt,
                a.app_stat_cd AS appr_stat_cd
            FROM THRNAMECARDREQ n
            LEFT JOIN TBAEMP e ON e.EMP_ID = n.emp_id
            LEFT JOIN TBADEPT d ON d.DEPT_ID = n.dept_id
            LEFT JOIN TAPDOC a ON a.app_id = n.app_id
            WHERE (@p_emp_no IS NULL OR e.emp_no = @p_emp_no)
              AND (@p_req_id IS NULL OR n.req_id = @p_req_id)
              AND (@p_req_no IS NULL OR n.req_no LIKE '%' + @p_req_no + '%')
            ORDER BY n.req_id DESC;
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
