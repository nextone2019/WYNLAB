-- 기타출고요청/기타출고에 출고유형(trans_type) 추가 (2026-10-03, WYNLAB_DEV 전용)
--
-- 기타출고요청 헤더에 출고유형(MA0011 수불유형 중 출고 계열 rel_cd1='O')을 둔다. 비우면 ETC_OUT(기타출고). 기타출고(TMAETCOUTM)에도
-- 같은 컬럼을 두어, 요청에서 불러올 때 요청의 출고유형이 따라오고 확정 때 수불(TMATRANS)의 수불유형으로 쓰인다(예전엔 항상 ETC_OUT).
-- 기존 출고사유(out_reason, MA0013)는 보류 상태라 화면/조회에서는 빼고 컬럼만 남겨 둔다(기타출고요청 불러오기 팝업의 "출고사유" 열은 "출고유형"으로 바뀐다).
-- 콤보 L_MA0011_O = 출고 계열 수불유형만. 사용자가 MA0011을 정리해도(rel_cd1='O' 유지) 그대로 동작한다.
-- 여러 번 실행해도 안전하다.

IF COL_LENGTH('TMAETCREQM', 'trans_type') IS NULL ALTER TABLE TMAETCREQM ADD trans_type VARCHAR(10) NULL;   -- 출고유형 MA0011(출고 계열)
GO
IF COL_LENGTH('TMAETCOUTM', 'trans_type') IS NULL ALTER TABLE TMAETCOUTM ADD trans_type VARCHAR(10) NULL;   -- 출고유형(수불유형) MA0011(출고 계열)
GO
UPDATE TMAETCREQM SET trans_type = 'ETC_OUT' WHERE trans_type IS NULL;
UPDATE TMAETCOUTM SET trans_type = 'ETC_OUT' WHERE trans_type IS NULL;
GO

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_MA0011_O', NULL, N'출고유형(출고계열 수불유형)', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''MA0011''' + CHAR(13) + CHAR(10)
       + N'AND     rel_cd1 = ''O''' + CHAR(13) + CHAR(10) + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_MA0011_O');
GO

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

            SELECT m.req_id, m.acc_id, a.ACC_NM, m.req_no, m.req_date, m.req_title, m.out_reason, m.trans_type, tt.minor_nm AS trans_type_nm,
                   m.stat_cd, m.dept_id, d.dept_nm, m.emp_id, e.emp_nm,
                   m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.stop_yn,
                   m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMAETCREQM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
                LEFT JOIN TSMMINOR tt ON tt.major_cd = 'MA0011' AND tt.minor_cd = m.trans_type
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
    @p_trans_type VARCHAR(10) = NULL,       /* 출고유형 - MA0011 중 출고 계열(rel_cd1='O'), 비우면 ETC_OUT */
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

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'ETC_OUT';
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'O' AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고유형이 올바르지 않습니다(수불유형 중 출고 계열만 선택할 수 있습니다).'; RETURN; END
        END
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCREQM', 'req_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCREQM (acc_id, req_no, req_date, req_title, out_reason, trans_type, dept_id, emp_id, stat_cd, cfm_yn, stop_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_req_date, @p_req_title, @p_out_reason, @p_trans_type, @p_dept_id, @p_emp_id, '0', 'N', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_req_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCREQM SET
                acc_id = @p_acc_id, req_date = @p_req_date, req_title = @p_req_title, out_reason = @p_out_reason, trans_type = @p_trans_type,
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
                   m.trans_type, tt.minor_nm AS trans_type_nm,
                   dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, dt.lot_no,
                   dt.qty AS req_qty, ISNULL(dt.next_qty, 0) AS next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                   dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, ISNULL(i.stock_yn, 'N') AS stock_yn
            FROM TMAETCREQD dt
                JOIN TMAETCREQM m ON m.req_id = dt.req_id
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TSMMINOR tt ON tt.major_cd = 'MA0011' AND tt.minor_cd = m.trans_type
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

            SELECT m.out_id, m.acc_id, a.ACC_NM, m.out_no, m.out_date, m.out_reason, m.trans_type, tt.minor_nm AS trans_type_nm, m.stat_cd,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TMAETCOUTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TSMMINOR tt ON tt.major_cd = 'MA0011' AND tt.minor_cd = m.trans_type
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
    @p_trans_type VARCHAR(10) = NULL,       /* 출고유형 - MA0011 중 출고 계열(rel_cd1='O'), 비우면 ETC_OUT */
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

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'ETC_OUT';
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'O' AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고유형이 올바르지 않습니다(수불유형 중 출고 계열만 선택할 수 있습니다).'; RETURN; END
        END
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCOUTM', 'out_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCOUTM (acc_id, out_no, out_date, out_reason, trans_type, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_out_date, @p_out_reason, @p_trans_type, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_out_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTM SET
                acc_id = @p_acc_id, out_date = @p_out_date, out_reason = @p_out_reason, trans_type = @p_trans_type, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
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

            -- 헤더 출고유형이 기본값(비어 있거나 ETC_OUT)이고 요청에서 불러왔으면 요청의 출고유형으로 채운다
            IF @src_type = 'ETCREQ'
                UPDATE h SET trans_type = r.trans_type FROM TMAETCOUTM h, TMAETCREQM r
                WHERE h.out_id = @p_out_id AND r.req_id = @src_id AND ISNULL(h.trans_type, '') IN ('', 'ETC_OUT') AND ISNULL(r.trans_type, '') <> '';
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

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @out_no VARCHAR(20), @acc BIGINT, @out_date VARCHAR(8), @type VARCHAR(10);
    SELECT @stat = stat_cd, @out_no = out_no, @acc = acc_id, @out_date = out_date, @type = ISNULL(NULLIF(trans_type, ''), 'ETC_OUT') FROM TMAETCOUTM WHERE out_id = @p_out_id;

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
        VALUES (@acc, ISNULL(@out_date, CONVERT(VARCHAR(8), GETDATE(), 112)), 'O', @type, @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
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

    DECLARE @stat VARCHAR(10), @out_no VARCHAR(20), @acc BIGINT, @type VARCHAR(10);
    SELECT @stat = stat_cd, @out_no = out_no, @acc = acc_id, @type = ISNULL(NULLIF(trans_type, ''), 'ETC_OUT') FROM TMAETCOUTM WHERE out_id = @p_out_id;
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
        VALUES (@acc, CONVERT(VARCHAR(8), GETDATE(), 112), 'I', @type, @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
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

