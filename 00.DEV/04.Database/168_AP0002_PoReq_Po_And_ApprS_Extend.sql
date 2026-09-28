-- 구매요청/구매발주 화면에 전자결재(TAP) 연동 추가 (2026-09-22). 명함신청서(NAMECARD)가
-- 이미 쓰고 있는 배관(USP_AP_APPR_S/TAPDOC/TAPDOCPATH/popApp)을 그대로 재사용 - 문서유형만
-- 늘리면 된다(123/124번 마이그레이션 주석에 "문서유형이 늘면 여기에 분기 추가"라고 이미
-- 명시돼 있음).

-- ============================================================
-- 1) AP0002(결재문서유형)에 PO_REQ/PO 추가
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'AP0002' AND minor_cd = 'PO_REQ')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('AP0002', 'PO_REQ', N'구매요청서', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'AP0002' AND minor_cd = 'PO')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('AP0002', 'PO', N'구매발주서', 3, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 2) 구매요청/구매발주 자체 진행상태 코드 - GW0001(명함신청서 진행상태: 0=신청/C=확정)과
--    같은 방식. AP0001(결재상태 자체)과는 별개 개념이다 - stat_cd는 업무상태, 결재상태는
--    TAPDOC을 JOIN해서 화면에 얹는다(USP_HR_NAMECARD_Q 패턴).
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0001')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('MA0001', N'구매요청 진행상태', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0001')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES
    ('MA0001', '0', N'작성', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('MA0001', 'C', N'승인완료', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0002')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('MA0002', N'구매발주 진행상태', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0002')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES
    ('MA0002', '0', N'작성', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('MA0002', 'C', N'승인완료', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 3) USP_AP_APPR_S 확장 - 상신(N)/최종승인(A)/승인취소(C) 세 분기에 PO_REQ(TMAPOREQM,
--    키 req_id)/PO(TMAPOM, 키 po_id) 케이스 추가. NAMECARD 분기는 그대로 두고 옆에
--    ELSE IF로만 덧붙인다 - 기존 결재 동작(명함신청서)엔 영향 없음. 반려(R)는 NAMECARD와
--    동일하게 원본 stat_cd를 건드리지 않는다(123번 주석과 같은 원칙).
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

            INSERT INTO TAPDOC (
                app_no, app_date, app_title, app_text, form_id, doc_type, doc_id, doc_no,
                acc_id, dept_id, emp_id, end_yn, rtn_yn, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', CONVERT(VARCHAR(8), GETDATE(), 112), @p_app_title, @p_app_text, @p_form_id, @p_doc_type, @p_doc_id, @p_doc_no,
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
            ELSE IF @p_doc_type = 'PO_REQ'
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
                ELSE IF @fin_doc_type = 'PO_REQ'
                    UPDATE TMAPOREQM SET stat_cd = 'C' WHERE req_id = @fin_doc_id;
                ELSE IF @fin_doc_type = 'PO'
                    UPDATE TMAPOM SET stat_cd = 'C' WHERE po_id = @fin_doc_id;
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

            -- 반려는 원본 stat_cd(진행상태)를 건드리지 않는다 - NAMECARD/PO_REQ/PO 전부 동일.
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
                ELSE IF @undo_doc_type = 'PO_REQ'
                    UPDATE TMAPOREQM SET stat_cd = '0' WHERE req_id = @undo_doc_id;
                ELSE IF @undo_doc_type = 'PO'
                    UPDATE TMAPOM SET stat_cd = '0' WHERE po_id = @undo_doc_id;
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
