-- 기타출고요청 / 기타출고 / 기초재고 프로시저 + 전자결재 연결(ETCREQ) (2026-10-03, WYNLAB_DEV 전용)
-- 테이블/코드/채번은 258번. 구조와 규칙은 입고(198/GR 프로시저)와 같다.
--
--   기타출고요청  USP_MA_ETCREQ_Q / _S / _S_1 / USP_MA_ETCREQPICK_Q(기타출고 "요청 불러오기" 팝업) + USP_AP_APPR_S_ETCREQ(결재 후처리)
--   기타출고      USP_MA_ETCOUT_Q / _S / _S_1 / USP_MA_ETCOUT_C_S(확정 C / 확정취소 CC) + _CONFIRM_CORE / _CANCEL_CORE
--   기초재고      USP_MA_OPEN_Q / _S / _S_1 / USP_MA_OPEN_C_S + _CONFIRM_CORE / _CANCEL_CORE
--
-- 확정 = 수불(TMATRANS) 생성 + (라인 stock_yn='Y'만) 현재고(TMASTOCK) 반영. 취소 = 역거래 수불 추가(org_trans_id) + 재고 되돌림(재고 부족하면 불가).
-- 내부(CORE) 프로시저는 오류를 THROW 50001로 올리고 화면용 래퍼(_C_S)가 그 문구를 돌려준다.
-- 기타출고는 요청 없이도 직접 등록할 수 있다(라인 src_type이 NULL). 요청을 불러온 라인은 승인완료된 요청의 잔량 이내여야 한다.

-- ============================================================
-- 1) 기타출고요청
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_ETCREQ_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준) */
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = req_id
            FROM TMAETCREQM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_req_id IS NULL OR req_id = @p_req_id)
              AND (@p_req_id IS NOT NULL OR @p_req_no IS NULL OR req_no LIKE '%' + @p_req_no + '%')
            ORDER BY req_id DESC;

            SELECT m.req_id, m.acc_id, a.ACC_NM, m.req_no, m.req_date, m.req_title, m.out_reason,
                   m.stat_cd, m.dept_id, d.dept_nm, m.emp_id, e.emp_nm,
                   m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.stop_yn,
                   m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMAETCREQM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.req_id = @match_id;

            SELECT dt.req_id, dt.serl, dt.acc_id, dt.req_no, dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                   dt.qty, ISNULL(dt.next_qty, 0) AS next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                   dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.lot_no, dt.cfm_yn, dt.stop_yn, dt.remark
            FROM TMAETCREQD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.req_id = @match_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCREQ_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_req_date VARCHAR(8) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
    @p_out_reason VARCHAR(10) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10), @app_no VARCHAR(20), @appr VARCHAR(10);
            SELECT @stat = m.stat_cd, @app_no = m.app_no, @appr = t.app_stat_cd
            FROM TMAETCREQM m LEFT JOIN TAPDOC t ON t.app_id = m.app_id WHERE m.req_id = @p_req_id;

            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고요청을 찾을 수 없습니다.'; RETURN; END
            IF @stat = 'C' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청은 수정하거나 삭제할 수 없습니다.'; RETURN; END
            IF @appr IN ('0', '1') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재가 진행 중인 기타출고요청은 수정하거나 삭제할 수 없습니다.'; RETURN; END
            IF @p_work_type = 'D' AND ISNULL(@app_no, '') <> ''
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 결재상신된 기타출고요청은 삭제할 수 없습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCREQM', 'req_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCREQM (acc_id, req_no, req_date, req_title, out_reason, dept_id, emp_id, stat_cd, cfm_yn, stop_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_req_date, @p_req_title, @p_out_reason, @p_dept_id, @p_emp_id, '0', 'N', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_req_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCREQM SET
                acc_id = @p_acc_id, req_date = @p_req_date, req_title = @p_req_title, out_reason = @p_out_reason,
                dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAETCREQD WHERE req_id = @p_req_id;
            DELETE FROM TMAETCREQM WHERE req_id = @p_req_id;
        END

        SET @GeneratedCode = CAST(@p_req_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCREQ_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        DECLARE @stat VARCHAR(10), @appr VARCHAR(10);
        SELECT @stat = m.stat_cd, @appr = t.app_stat_cd
        FROM TMAETCREQM m LEFT JOIN TAPDOC t ON t.app_id = m.app_id WHERE m.req_id = @p_req_id;

        IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고요청을 찾을 수 없습니다.'; RETURN; END
        IF @stat = 'C' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청은 수정할 수 없습니다.'; RETURN; END
        IF @appr IN ('0', '1') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재가 진행 중인 기타출고요청은 수정할 수 없습니다.'; RETURN; END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @p_qty IS NULL OR @p_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'요청수량은 0보다 커야 합니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAETCREQD WHERE req_id = @p_req_id;

            INSERT INTO TMAETCREQD (req_id, serl, acc_id, req_no, item_id, unit_cd, qty, next_qty, wh_id, loc_id, lot_no, cfm_yn, stop_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.req_id, @next, m.acc_id, m.req_no, @p_item_id, ISNULL(@p_unit_cd, i.unit_cd), @p_qty, 0, @p_wh_id, @p_loc_id, @p_lot_no, 'N', 'N', @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAETCREQM m LEFT JOIN TBAITEM i ON i.item_id = @p_item_id
            WHERE m.req_id = @p_req_id;
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCREQD SET
                item_id = @p_item_id, unit_cd = @p_unit_cd, qty = @p_qty, wh_id = @p_wh_id, loc_id = @p_loc_id, lot_no = @p_lot_no, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAETCREQD WHERE req_id = @p_req_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 기타출고 "요청 불러오기" 팝업 - 승인완료(확정)된 요청 중 아직 출고할 잔량이 남은 라인
CREATE OR ALTER PROCEDURE USP_MA_ETCREQPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_cust_id BIGINT = NULL,               /* 팝업 공통 파라미터 규약상 받기만 한다(사용 안 함) */
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명/규격 */
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
            SELECT 'ETCREQ' AS src_type, dt.req_id AS src_id, dt.serl AS src_serl, dt.req_no AS src_no, m.req_date, m.req_title,
                   m.out_reason, r.minor_nm AS out_reason_nm,
                   dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, dt.lot_no,
                   dt.qty AS req_qty, ISNULL(dt.next_qty, 0) AS next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                   dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, ISNULL(i.stock_yn, 'N') AS stock_yn
            FROM TMAETCREQD dt
                JOIN TMAETCREQM m ON m.req_id = dt.req_id
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TSMMINOR r ON r.major_cd = 'MA0013' AND r.minor_cd = m.out_reason
            WHERE m.stat_cd = 'C' AND ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(dt.stop_yn, 'N') <> 'Y'
              AND dt.qty - ISNULL(dt.next_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_date_from, '') = '' OR m.req_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR m.req_date <= @p_date_to)
              AND (ISNULL(@p_doc_no, '') = '' OR dt.req_no LIKE '%' + @p_doc_no + '%')
              AND (ISNULL(@p_keyword, '') = '' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%' OR i.item_spec LIKE '%' + @p_keyword + '%')
            ORDER BY m.req_date DESC, dt.req_id DESC, dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 전자결재 후처리 - USP_AP_APPR_DOC가 doc_type='ETCREQ'로 찾아 부른다. 최종승인 = 확정(stat_cd C, cfm_yn Y).
CREATE OR ALTER PROCEDURE USP_AP_APPR_S_ETCREQ
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_event = 'SUBMIT'
    BEGIN
        UPDATE TMAETCREQM SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id) WHERE req_id = @p_doc_id;

        -- 결재자가 본인뿐이면 상신 즉시 확정
        IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C' AND stat_cd <> 'E')
            SET @p_event = 'APPROVE_END';
    END

    IF @p_event = 'APPROVE_END'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM TMAETCREQD WHERE req_id = @p_doc_id)
            THROW 50001, N'요청 품목이 없어 승인할 수 없습니다.', 1;

        UPDATE TMAETCREQM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id WHERE req_id = @p_doc_id;
        UPDATE TMAETCREQD SET cfm_yn = 'Y' WHERE req_id = @p_doc_id;
    END
    ELSE IF @p_event IN ('UNDO_END', 'RESET')
    BEGIN
        IF EXISTS (SELECT 1 FROM TMAETCREQD WHERE req_id = @p_doc_id AND ISNULL(next_qty, 0) > 0)
            THROW 50001, N'이미 기타출고가 진행된 요청은 승인을 취소할 수 없습니다.', 1;

        UPDATE TMAETCREQM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL WHERE req_id = @p_doc_id;
        UPDATE TMAETCREQD SET cfm_yn = 'N' WHERE req_id = @p_doc_id;
        IF @p_event = 'RESET' UPDATE TMAETCREQM SET app_id = NULL, app_no = NULL WHERE req_id = @p_doc_id;
    END
    -- 'REJECT'(반려)는 문서 상태를 바꾸지 않는다(결재 쪽 상태가 반려로 표시되고 요청자가 고쳐서 다시 상신).
END
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'AP0002' AND minor_cd = 'ETCREQ')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, rel_cd1, rel_cd2, reg_user_id, reg_dt)
VALUES ('AP0002', 'ETCREQ', N'기타출고요청서', (SELECT ISNULL(MAX(sort), 0) + 1 FROM TSMMINOR WHERE major_cd = 'AP0002'), 'N', 'Y', 'MA.frmEtcOutReq', N'자재', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 2) 기타출고
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_out_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = out_id
            FROM TMAETCOUTM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_out_id IS NULL OR out_id = @p_out_id)
              AND (@p_out_id IS NOT NULL OR @p_out_no IS NULL OR out_no LIKE '%' + @p_out_no + '%')
            ORDER BY out_id DESC;

            SELECT m.out_id, m.acc_id, a.ACC_NM, m.out_no, m.out_date, m.out_reason, m.stat_cd,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TMAETCOUTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.out_id = @match_id;

            SELECT dt.out_id, dt.serl, dt.acc_id, dt.out_no, dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                   dt.out_qty, dt.lot_no, dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn,
                   s.stock_qty,
                   dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                   rd.qty AS req_qty, (rd.qty - ISNULL(rd.next_qty, 0)) AS req_remain_qty,
                   dt.trans_id, dt.remark
            FROM TMAETCOUTD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TMASTOCK s ON s.acc_id = dt.acc_id AND s.item_id = dt.item_id AND s.wh_id = dt.wh_id
                                    AND s.loc_id = ISNULL(dt.loc_id, 0) AND s.lot_no = ISNULL(dt.lot_no, N'')
                LEFT JOIN TMAETCREQD rd ON dt.src_type = 'ETCREQ' AND rd.req_id = dt.src_id AND rd.serl = dt.src_serl
            WHERE dt.out_id = @match_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_out_date VARCHAR(8) = NULL,
    @p_out_reason VARCHAR(10) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10);
            SELECT @stat = stat_cd FROM TMAETCOUTM WHERE out_id = @p_out_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고 문서를 찾을 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타출고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCOUTM', 'out_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCOUTM (acc_id, out_no, out_date, out_reason, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_out_date, @p_out_reason, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_out_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTM SET
                acc_id = @p_acc_id, out_date = @p_out_date, out_reason = @p_out_reason, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE out_id = @p_out_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAETCOUTD WHERE out_id = @p_out_id;
            DELETE FROM TMAETCOUTM WHERE out_id = @p_out_id;
        END

        SET @GeneratedCode = CAST(@p_out_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 라인: 요청 불러오기 라인(src_type='ETCREQ')은 품목/단위/원천 고정, 출고수량은 요청 잔량 이내. 직접 라인(src_type NULL)은 품목을 직접 고른다.
CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_out_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_serl INT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        DECLARE @hdr_stat VARCHAR(10), @hdr_acc BIGINT;
        SELECT @hdr_stat = stat_cd, @hdr_acc = acc_id FROM TMAETCOUTM WHERE out_id = @p_out_id;
        IF @hdr_stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고 문서를 찾을 수 없습니다.'; RETURN; END
        IF @hdr_stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타출고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END

        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT;
        IF @p_work_type = 'N'
            SELECT @src_type = NULLIF(@p_src_type, ''), @src_id = @p_src_id, @src_serl = @p_src_serl;
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl FROM TMAETCOUTD WHERE out_id = @p_out_id AND serl = @p_serl;
            IF @@ROWCOUNT = 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고 품목을 찾을 수 없습니다.'; RETURN; END
        END

        DECLARE @item BIGINT = @p_item_id, @unit VARCHAR(10), @stock VARCHAR(1), @src_no VARCHAR(20), @remain NUMERIC(18,4);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_out_qty IS NULL OR @p_out_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고수량은 0보다 커야 합니다.'; RETURN; END

            IF @src_type = 'ETCREQ'
            BEGIN
                SELECT @item = d.item_id, @unit = d.unit_cd, @src_no = d.req_no, @remain = d.qty - ISNULL(d.next_qty, 0)
                FROM TMAETCREQD d JOIN TMAETCREQM m ON m.req_id = d.req_id
                WHERE d.req_id = @src_id AND d.serl = @src_serl AND m.stat_cd = 'C' AND m.acc_id = @hdr_acc
                  AND ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y';
                IF @item IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청 품목을 찾을 수 없습니다.'; RETURN; END
                IF @p_out_qty > @remain
                BEGIN
                    SET @ReturnCode = -1;
                    SET @ReturnMsg = N'출고수량이 요청 잔량(' + CAST(CAST(@remain AS FLOAT) AS NVARCHAR(30)) + N')을 초과했습니다. (' + @src_no + N')';
                    RETURN;
                END
            END
            ELSE IF @src_type IS NOT NULL
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'올바르지 않은 원천입니다.'; RETURN; END

            IF @item IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            SELECT @stock = ISNULL(stock_yn, 'N'), @unit = ISNULL(@unit, unit_cd) FROM TBAITEM WHERE item_id = @item;
            IF @stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAETCOUTD WHERE out_id = @p_out_id;

            INSERT INTO TMAETCOUTD (out_id, serl, acc_id, out_no, item_id, unit_cd, out_qty, lot_no, wh_id, loc_id, stock_yn,
                                    src_type, src_id, src_no, src_serl, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.out_id, @next, m.acc_id, m.out_no, @item, @unit, @p_out_qty, @p_lot_no, @p_wh_id, @p_loc_id, @stock,
                   @src_type, @src_id, @src_no, @src_serl, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAETCOUTM m WHERE m.out_id = @p_out_id;

            -- 헤더 출고사유가 비어 있고 요청에서 불러왔으면 요청의 출고사유로 채운다
            IF @src_type = 'ETCREQ'
                UPDATE h SET out_reason = r.out_reason FROM TMAETCOUTM h, TMAETCREQM r
                WHERE h.out_id = @p_out_id AND r.req_id = @src_id AND ISNULL(h.out_reason, '') = '';
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTD SET
                item_id = @item, unit_cd = @unit, out_qty = @p_out_qty, lot_no = @p_lot_no, wh_id = @p_wh_id, loc_id = @p_loc_id, stock_yn = @stock, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE out_id = @p_out_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAETCOUTD WHERE out_id = @p_out_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_CONFIRM_CORE
    @p_out_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @out_no VARCHAR(20), @acc BIGINT, @out_date VARCHAR(8);
    SELECT @stat = stat_cd, @out_no = out_no, @acc = acc_id, @out_date = out_date FROM TMAETCOUTM WHERE out_id = @p_out_id;

    IF @stat IS NULL THROW 50001, N'기타출고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'이미 확정된 기타출고입니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TMAETCOUTD WHERE out_id = @p_out_id) THROW 50001, N'출고 품목이 없어 확정할 수 없습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAETCOUTD WHERE out_id = @p_out_id AND wh_id IS NULL) THROW 50001, N'출고 창고를 입력하지 않은 품목이 있습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAETCOUTD d JOIN TBAITEM i ON i.item_id = d.item_id
               WHERE d.out_id = @p_out_id AND i.lot_yn = 'Y' AND ISNULL(d.lot_no, N'') = N'')
        THROW 50001, N'LOT 관리 품목의 LOT를 입력하지 않은 품목이 있습니다.', 1;

    BEGIN TRAN;

    -- 원천 요청 라인을 잠그고 잔량을 다시 검증한다(같은 요청 라인을 여러 줄이 쓰면 합산).
    DECLARE @lock INT;
    SELECT @lock = 1 FROM TMAETCREQD r WITH (UPDLOCK, ROWLOCK)
    JOIN TMAETCOUTD g ON g.src_type = 'ETCREQ' AND g.src_id = r.req_id AND g.src_serl = r.serl WHERE g.out_id = @p_out_id;

    DECLARE @bad VARCHAR(20);
    SELECT TOP 1 @bad = t.src_no
    FROM (SELECT src_id, src_serl, MAX(src_no) AS src_no, SUM(out_qty) AS this_qty FROM TMAETCOUTD
          WHERE out_id = @p_out_id AND src_type = 'ETCREQ' GROUP BY src_id, src_serl) t
        LEFT JOIN TMAETCREQD r ON r.req_id = t.src_id AND r.serl = t.src_serl
        LEFT JOIN TMAETCREQM m ON m.req_id = r.req_id
    WHERE r.req_id IS NULL OR m.stat_cd <> 'C' OR ISNULL(m.stop_yn, 'N') = 'Y' OR ISNULL(r.stop_yn, 'N') = 'Y'
       OR t.this_qty > r.qty - ISNULL(r.next_qty, 0);
    IF @bad IS NOT NULL
    BEGIN
        SET @msg = N'기타출고요청의 출고 가능 수량을 초과했거나 마감/취소된 요청 품목이 있습니다. (' + @bad + N')';
        THROW 50001, @msg, 1;
    END

    -- 재고 검증: 재고관리 품목은 (품목,창고,위치,LOT)별 합계가 현재고 이내여야 한다.
    DECLARE @short NVARCHAR(200);
    SELECT TOP 1 @short = ISNULL(i.item_nm, CAST(t.item_id AS NVARCHAR(20))) + N' (현재고 ' + CAST(CAST(ISNULL(s.stock_qty, 0) AS FLOAT) AS NVARCHAR(30))
                          + N' < 출고 ' + CAST(CAST(t.q AS FLOAT) AS NVARCHAR(30)) + N')'
    FROM (SELECT item_id, wh_id, ISNULL(loc_id, 0) AS loc_id, ISNULL(lot_no, N'') AS lot_no, SUM(out_qty) AS q
          FROM TMAETCOUTD WHERE out_id = @p_out_id AND stock_yn = 'Y' GROUP BY item_id, wh_id, ISNULL(loc_id, 0), ISNULL(lot_no, N'')) t
        LEFT JOIN TMASTOCK s WITH (UPDLOCK, HOLDLOCK) ON s.acc_id = @acc AND s.item_id = t.item_id AND s.wh_id = t.wh_id AND s.loc_id = t.loc_id AND s.lot_no = t.lot_no
        LEFT JOIN TBAITEM i ON i.item_id = t.item_id
    WHERE ISNULL(s.stock_qty, 0) < t.q;
    IF @short IS NOT NULL
    BEGIN
        SET @msg = N'현재고가 부족합니다. ' + @short;
        THROW 50001, @msg, 1;
    END

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @tid BIGINT;
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, out_qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn FROM TMAETCOUTD WHERE out_id = @p_out_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, ISNULL(@out_date, CONVERT(VARCHAR(8), GETDATE(), 112)), 'O', 'ETC_OUT', @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
                'ETCOUT', @p_out_id, @out_no, @serl, NULL, NULL, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        UPDATE TMAETCOUTD SET trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE out_id = @p_out_id AND serl = @serl;

        IF @stock = 'Y'
            UPDATE TMASTOCK SET stock_qty = stock_qty - @qty, trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;

        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAETCOUTM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
                          upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE out_id = @p_out_id;

    -- 원천 요청 라인 next_qty = 확정된 기타출고 수량 합
    UPDATE r SET next_qty = ISNULL((SELECT SUM(o.out_qty) FROM TMAETCOUTD o JOIN TMAETCOUTM h ON h.out_id = o.out_id
                                    WHERE o.src_type = 'ETCREQ' AND o.src_id = r.req_id AND o.src_serl = r.serl AND h.stat_cd = 'C'), 0)
    FROM TMAETCREQD r
    WHERE EXISTS (SELECT 1 FROM TMAETCOUTD g WHERE g.out_id = @p_out_id AND g.src_type = 'ETCREQ' AND g.src_id = r.req_id AND g.src_serl = r.serl);

    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_CANCEL_CORE
    @p_out_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @out_no VARCHAR(20), @acc BIGINT;
    SELECT @stat = stat_cd, @out_no = out_no, @acc = acc_id FROM TMAETCOUTM WHERE out_id = @p_out_id;
    IF @stat IS NULL THROW 50001, N'기타출고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> 'C' THROW 50001, N'확정된 기타출고만 확정취소할 수 있습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @orig BIGINT, @tid BIGINT;
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, out_qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn, trans_id FROM TMAETCOUTD WHERE out_id = @p_out_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, CONVERT(VARCHAR(8), GETDATE(), 112), 'I', 'ETC_OUT', @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
                'ETCOUT', @p_out_id, @out_no, @serl, @orig, N'기타출고 확정취소', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        IF @stock = 'Y'
        BEGIN
            UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET stock_qty = stock_qty + @qty, trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF @@ROWCOUNT = 0
                INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @item, @wh, @loc, @lot, @unit, @qty, @tid, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END

        UPDATE TMAETCOUTD SET trans_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE out_id = @p_out_id AND serl = @serl;
        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAETCOUTM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE out_id = @p_out_id;

    UPDATE r SET next_qty = ISNULL((SELECT SUM(o.out_qty) FROM TMAETCOUTD o JOIN TMAETCOUTM h ON h.out_id = o.out_id
                                    WHERE o.src_type = 'ETCREQ' AND o.src_id = r.req_id AND o.src_serl = r.serl AND h.stat_cd = 'C'), 0)
    FROM TMAETCREQD r
    WHERE EXISTS (SELECT 1 FROM TMAETCOUTD g WHERE g.out_id = @p_out_id AND g.src_type = 'ETCREQ' AND g.src_id = r.req_id AND g.src_serl = r.serl);

    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'C' EXEC USP_MA_ETCOUT_CONFIRM_CORE @p_out_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC' EXEC USP_MA_ETCOUT_CANCEL_CORE @p_out_id, @p_user_id, @p_client_pc;
        SET @GeneratedCode = CAST(@p_out_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 3) 기초재고
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_OPEN_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
    @p_open_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = open_id
            FROM TMAOPENM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_open_id IS NULL OR open_id = @p_open_id)
              AND (@p_open_id IS NOT NULL OR @p_open_no IS NULL OR open_no LIKE '%' + @p_open_no + '%')
            ORDER BY open_id DESC;

            SELECT m.open_id, m.acc_id, a.ACC_NM, m.open_no, m.open_date, m.stat_cd,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TMAOPENM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.open_id = @match_id;

            SELECT dt.open_id, dt.serl, dt.acc_id, dt.open_no, dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                   dt.qty, dt.lot_no, dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn, dt.trans_id, dt.remark
            FROM TMAOPEND dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.open_id = @match_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_open_date VARCHAR(8) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10);
            SELECT @stat = stat_cd FROM TMAOPENM WHERE open_id = @p_open_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기초재고 문서를 찾을 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기초재고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAOPENM', 'open_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAOPENM (acc_id, open_no, open_date, dept_id, emp_id, stat_cd, cfm_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_open_date, @p_dept_id, @p_emp_id, '0', 'N', @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_open_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAOPENM SET acc_id = @p_acc_id, open_date = @p_open_date, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE open_id = @p_open_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAOPEND WHERE open_id = @p_open_id;
            DELETE FROM TMAOPENM WHERE open_id = @p_open_id;
        END

        SET @GeneratedCode = CAST(@p_open_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        DECLARE @hdr_stat VARCHAR(10);
        SELECT @hdr_stat = stat_cd FROM TMAOPENM WHERE open_id = @p_open_id;
        IF @hdr_stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기초재고 문서를 찾을 수 없습니다.'; RETURN; END
        IF @hdr_stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기초재고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END

        DECLARE @unit VARCHAR(10), @stock VARCHAR(1);
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @p_qty IS NULL OR @p_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기초수량은 0보다 커야 합니다.'; RETURN; END
            SELECT @unit = unit_cd, @stock = ISNULL(stock_yn, 'N') FROM TBAITEM WHERE item_id = @p_item_id;
            IF @stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAOPEND WHERE open_id = @p_open_id;

            INSERT INTO TMAOPEND (open_id, serl, acc_id, open_no, item_id, unit_cd, qty, lot_no, wh_id, loc_id, stock_yn, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.open_id, @next, m.acc_id, m.open_no, @p_item_id, @unit, @p_qty, @p_lot_no, @p_wh_id, @p_loc_id, @stock, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAOPENM m WHERE m.open_id = @p_open_id;
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAOPEND SET item_id = @p_item_id, unit_cd = @unit, qty = @p_qty, lot_no = @p_lot_no, wh_id = @p_wh_id, loc_id = @p_loc_id,
                                stock_yn = @stock, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE open_id = @p_open_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAOPEND WHERE open_id = @p_open_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_CONFIRM_CORE
    @p_open_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @open_no VARCHAR(20), @acc BIGINT, @open_date VARCHAR(8);
    SELECT @stat = stat_cd, @open_no = open_no, @acc = acc_id, @open_date = open_date FROM TMAOPENM WHERE open_id = @p_open_id;

    IF @stat IS NULL THROW 50001, N'기초재고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'이미 확정된 기초재고입니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TMAOPEND WHERE open_id = @p_open_id) THROW 50001, N'기초재고 품목이 없어 확정할 수 없습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAOPEND WHERE open_id = @p_open_id AND wh_id IS NULL) THROW 50001, N'창고를 입력하지 않은 품목이 있습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAOPEND d JOIN TBAITEM i ON i.item_id = d.item_id
               WHERE d.open_id = @p_open_id AND i.lot_yn = 'Y' AND ISNULL(d.lot_no, N'') = N'')
        THROW 50001, N'LOT 관리 품목의 LOT를 입력하지 않은 품목이 있습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @tid BIGINT;
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn FROM TMAOPEND WHERE open_id = @p_open_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, ISNULL(@open_date, CONVERT(VARCHAR(8), GETDATE(), 112)), 'I', 'OPEN_IN', @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
                'OPEN', @p_open_id, @open_no, @serl, NULL, NULL, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        UPDATE TMAOPEND SET trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE open_id = @p_open_id AND serl = @serl;

        IF @stock = 'Y'
        BEGIN
            UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET stock_qty = stock_qty + @qty, trans_id = @tid, unit_cd = ISNULL(@unit, unit_cd),
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF @@ROWCOUNT = 0
                INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @item, @wh, @loc, @lot, @unit, @qty, @tid, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END

        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAOPENM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE open_id = @p_open_id;

    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_CANCEL_CORE
    @p_open_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @open_no VARCHAR(20), @acc BIGINT;
    SELECT @stat = stat_cd, @open_no = open_no, @acc = acc_id FROM TMAOPENM WHERE open_id = @p_open_id;
    IF @stat IS NULL THROW 50001, N'기초재고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> 'C' THROW 50001, N'확정된 기초재고만 확정취소할 수 있습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @orig BIGINT,
            @tid BIGINT, @cur NUMERIC(18,4), @item_nm NVARCHAR(200);
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn, trans_id FROM TMAOPEND WHERE open_id = @p_open_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @stock = 'Y'
        BEGIN
            SELECT @cur = stock_qty FROM TMASTOCK WITH (UPDLOCK, HOLDLOCK) WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF ISNULL(@cur, 0) < @qty
            BEGIN
                SELECT @item_nm = item_nm FROM TBAITEM WHERE item_id = @item;
                SET @msg = N'현재고가 부족해 기초재고를 취소할 수 없습니다(이미 출고/이동된 재고). 품목: ' + ISNULL(@item_nm, CAST(@item AS NVARCHAR(20)))
                         + N', 현재고 ' + CAST(CAST(ISNULL(@cur, 0) AS FLOAT) AS NVARCHAR(30)) + N' < 취소수량 ' + CAST(CAST(@qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @msg, 1;
            END
        END

        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, CONVERT(VARCHAR(8), GETDATE(), 112), 'O', 'OPEN_IN', @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
                'OPEN', @p_open_id, @open_no, @serl, @orig, N'기초재고 확정취소', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        IF @stock = 'Y'
            UPDATE TMASTOCK SET stock_qty = stock_qty - @qty, trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;

        UPDATE TMAOPEND SET trans_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE open_id = @p_open_id AND serl = @serl;
        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAOPENM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE open_id = @p_open_id;

    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'C' EXEC USP_MA_OPEN_CONFIRM_CORE @p_open_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC' EXEC USP_MA_OPEN_CANCEL_CORE @p_open_id, @p_user_id, @p_client_pc;
        SET @GeneratedCode = CAST(@p_open_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
