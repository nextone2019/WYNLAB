-- 구매요청등록(frmPoReq, Master-One Sheet) + 구매요청현황(frmPoReqList, Master-SubGrid) 프로시저
-- (2026-09-22). Q/S/S_1 3개는 frmPoReq(등록화면)용, POREQLIST_Q는 frmPoReqList(현황화면)용으로
-- 따로 둔다 - USP_BA_EMPLIST_Q가 USP_BA_EMP_Q와 별개인 것과 같은 이유(목록 조회조건과 상세
-- 단건조회 조건이 다르다).

-- ============================================================
-- 1) USP_MA_POREQ_Q - frmPoReq 전용. grd1이 없는 단일시트 화면이라 한 번의 호출로 헤더(0번
--    레코드셋)+품목(1번 레코드셋)을 같이 돌려준다. p_req_id(단건 강제조회, 저장 후 재조회/
--    현황화면 드릴다운용) 또는 p_req_no(사용자가 직접 입력해서 조회)로 매칭한다.
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
                m.app_id, m.app_no, t.stat_cd AS appr_stat_cd,
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
-- 2) USP_MA_POREQ_S - 헤더(TMAPOREQM) N/U/D. req_no는 신규 시 자동발번(PR+yymmdd+3자리,
--    USP_BA_WH_S류의 단순 SCOPE_IDENTITY 발번과 달리 업무번호 포맷이 필요해서 별도 계산).
--    이미 결재상신된 건(app_no 있음)은 삭제 금지 - THRNAMECARDREQ.frmNameCardReq.cs 관례와 동일.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQ_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_req_date VARCHAR(8) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
    @p_po_type VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(20) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            -- 채번을 공용 프로시저로 위임(2026-09-22, 자체 날짜+순번 계산 대신) - 173번
            -- 마이그레이션에서 TMAPOREQM/req_no에 prefix='PR' 설정을 미리 심어둔다.
            DECLARE @new_req_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAPOREQM', 'req_no', @p_acc_id, @new_req_no OUTPUT;

            INSERT INTO TMAPOREQM (
                acc_id, req_no, req_date, req_title, stat_cd, po_type,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @new_req_no, @p_req_date, @p_req_title, '0', @p_po_type,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_req_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOREQM SET
                acc_id = @p_acc_id,
                req_date = @p_req_date,
                req_title = @p_req_title,
                po_type = @p_po_type,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TMAPOREQM WHERE req_id = @p_req_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 구매요청은 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TMAPOREQD WHERE req_id = @p_req_id;
            DELETE FROM TMAPOREQM WHERE req_id = @p_req_id;
        END

        SET @GeneratedCode = CAST(@p_req_id AS VARCHAR(20));
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
-- 3) USP_MA_POREQ_S_1 - 품목(TMAPOREQD) 행별 N/U/D(frmMinorCode의 _S_1류와 같은 방식).
--    acc_id/req_no는 헤더에서 그대로 복사(비정규화 컬럼).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQ_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_no VARCHAR(20) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAPOREQD WHERE req_id = @p_req_id;

            INSERT INTO TMAPOREQD (
                req_id, serl, acc_id, req_no, item_id, qty, next_qty, unit_cd,
                cust_id, delv_date, wh_id, loc_id, src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT m.req_id, @nextSerl, m.acc_id, m.req_no, @p_item_id, @p_qty, @p_next_qty, @p_unit_cd,
                   @p_cust_id, @p_delv_date, @p_wh_id, @p_loc_id, @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TMAPOREQM m WHERE m.req_id = @p_req_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOREQD SET
                item_id = @p_item_id,
                qty = @p_qty,
                next_qty = @p_next_qty,
                unit_cd = @p_unit_cd,
                cust_id = @p_cust_id,
                delv_date = @p_delv_date,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                src_type = @p_src_type,
                src_id = @p_src_id,
                src_no = @p_src_no,
                src_serl = @p_src_serl,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE req_id = @p_req_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAPOREQD WHERE req_id = @p_req_id AND serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
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
-- 4) USP_MA_POREQLIST_Q - frmPoReqList(현황, Master-SubGrid) 전용. Q=검색조건에 맞는 헤더
--    목록(grd1), Q1=선택된 req_id의 품목 상세(grd2, 조회전용).
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
                m.app_id, m.app_no, t.stat_cd AS appr_stat_cd
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
