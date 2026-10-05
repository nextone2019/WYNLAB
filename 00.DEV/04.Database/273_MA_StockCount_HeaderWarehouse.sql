-- 재고실사: 대상 창고를 헤더 단일 창고(필수)로 단순화 (2026-10-04, WYNLAB_DEV 전용). 설계 변경은 Document\재고실사_설계서.md 맨 위 박스.
--  1) TMACNTM.wh_id 추가(기존 문서는 TMACNTW의 창고로 채움) - 프로시저가 저장 때 필수로 받는다(기존 문서 호환 때문에 컬럼 자체는 NULL 허용).
--  2) 대상 창고/조건 테이블 TMACNTW와 USP_MA_CNT_W_S 제거. 품목그룹/특정 품번으로 좁히는 일은 라인 그리드에서 행삭제로 한다.
--  3) 같은 창고에 진행 중인 실사가 있으면 대상확정을 막던 검사 제거(요청: 같은 날짜/창고 실사가 있어도 막지 않는다).
--  4) USP_MA_CNT_Q(결과셋 3개 -> 2개: 헤더+라인), USP_MA_CNTLIST_Q, USP_MA_CNT_S(wh_id 필수), USP_MA_CNT_S_1(라인추가 창고 = 헤더 창고), USP_MA_CNT_SNAP_CORE 수정.
-- 여러 번 실행해도 안전하다.

-- ============================================================
-- 1) 컬럼 추가 + 기존 데이터 이전 + TMACNTW/W_S 제거
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMACNTM') AND name = 'wh_id')
    ALTER TABLE TMACNTM ADD wh_id BIGINT NULL;
GO

IF OBJECT_ID('TMACNTW') IS NOT NULL
BEGIN
    EXEC(N'UPDATE m SET wh_id = (SELECT MIN(w.wh_id) FROM TMACNTW w WHERE w.cnt_id = m.cnt_id) FROM TMACNTM m WHERE m.wh_id IS NULL');
    EXEC(N'DROP TABLE TMACNTW');
END
-- 대상 창고 행이 없던 문서는 라인의 창고로 채운다(라인이 한 창고뿐일 때)
UPDATE m SET wh_id = (SELECT MIN(d.wh_id) FROM TMACNTD d WHERE d.cnt_id = m.cnt_id)
FROM TMACNTM m WHERE m.wh_id IS NULL AND EXISTS (SELECT 1 FROM TMACNTD d WHERE d.cnt_id = m.cnt_id);
GO

IF OBJECT_ID('USP_MA_CNT_W_S', 'P') IS NOT NULL DROP PROCEDURE USP_MA_CNT_W_S;
GO

-- ============================================================
-- 2) 조회 - 헤더 / 라인 (대상창고 결과셋 제거)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준) */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_cnt_no VARCHAR(20) = NULL,
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
            SELECT TOP 1 @match_id = cnt_id
            FROM TMACNTM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_cnt_id IS NULL OR cnt_id = @p_cnt_id)
              AND (@p_cnt_id IS NOT NULL OR @p_cnt_no IS NULL OR cnt_no LIKE '%' + @p_cnt_no + '%')
            ORDER BY cnt_id DESC;

            DECLARE @stat VARCHAR(10), @blind VARCHAR(1), @snap BIGINT, @cdate VARCHAR(8), @mask BIT;
            SELECT @stat = stat_cd, @blind = blind_yn, @snap = snap_trans_id, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @match_id;
            SET @mask = CASE WHEN @blind = 'Y' AND @stat IN ('0', '1', '2') THEN 1 ELSE 0 END;

            SELECT m.cnt_id, m.acc_id, a.ACC_NM, m.cnt_no, m.cnt_title, m.cnt_type, m.cnt_date, m.blind_yn, m.freeze_mode,
                   m.wh_id, h.wh_nm,
                   m.snap_trans_id, m.snap_dt, m.tol_qty, m.tol_rate,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                   m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMACNTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBAWH h ON h.wh_id = m.wh_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.cnt_id = @match_id;

            SELECT d.cnt_id, d.serl, d.acc_id, d.cnt_no, d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                   d.wh_id, w.wh_nm, d.loc_id, d.lot_no,
                   CASE WHEN @mask = 1 THEN NULL ELSE d.book_qty END AS book_qty,
                   d.cnt_qty, d.recnt_yn, d.recnt_qty, d.fin_qty,
                   CASE WHEN @mask = 1 THEN NULL ELSE mv.move_qty END AS move_qty,
                   CASE WHEN @mask = 1 THEN NULL
                        WHEN @stat IN ('3', 'C') AND d.diff_qty IS NOT NULL THEN d.diff_qty
                        WHEN d.fin_qty IS NULL THEN NULL
                        ELSE d.fin_qty - (d.book_qty + mv.move_qty) END AS diff_qty,
                   d.add_yn, d.adj_reason, d.stock_yn, d.cnt_user_id, d.cnt_dt, d.trans_id, d.remark
            FROM TMACNTD d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                CROSS APPLY (SELECT CASE WHEN @stat IN ('3', 'C') AND d.move_qty IS NOT NULL THEN d.move_qty
                                         ELSE dbo.FN_MA_CNT_MOVE(d.acc_id, d.item_id, d.wh_id, d.loc_id, d.lot_no, @snap, @cdate, d.cnt_id) END AS move_qty) mv
            WHERE d.cnt_id = @match_id
            ORDER BY d.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 3) 현황 - 창고는 헤더 값(컬럼 별칭 wh_nms 유지: 화면 컬럼 FieldName 그대로)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNTLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준) */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,                /* Q1 - 한 실사의 라인 */
    @p_cnt_no VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번 / 품명 */
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_cnt_type VARCHAR(10) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,          /* MA0014 진행상태 */
    @p_wh_id BIGINT = NULL,
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
            SELECT m.cnt_id, m.cnt_no, m.cnt_title, m.cnt_type, m.cnt_date, m.stat_cd, m.blind_yn,
                   h.wh_nm AS wh_nms,
                   (SELECT COUNT(*) FROM TMACNTD x WHERE x.cnt_id = m.cnt_id) AS line_cnt,
                   (SELECT COUNT(*) FROM TMACNTD x WHERE x.cnt_id = m.cnt_id AND x.fin_qty IS NOT NULL) AS entered_cnt,
                   CASE WHEN m.stat_cd IN ('3', 'C') THEN (SELECT COUNT(*) FROM TMACNTD x WHERE x.cnt_id = m.cnt_id AND ISNULL(x.diff_qty, 0) <> 0) END AS diff_cnt,
                   d.dept_nm, e.emp_nm, m.cfm_dt, m.app_no, m.remark
            FROM TMACNTM m
                LEFT JOIN TBAWH h ON h.wh_id = m.wh_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_cnt_no, '') = '' OR m.cnt_no LIKE '%' + @p_cnt_no + '%')
              AND (ISNULL(@p_date_from, '') = '' OR m.cnt_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR m.cnt_date <= @p_date_to)
              AND (ISNULL(@p_cnt_type, '') = '' OR m.cnt_type = @p_cnt_type)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (@p_wh_id IS NULL OR m.wh_id = @p_wh_id)
              AND (ISNULL(@p_keyword, '') = ''
                   OR EXISTS (SELECT 1 FROM TMACNTD x JOIN TBAITEM i ON i.item_id = x.item_id
                              WHERE x.cnt_id = m.cnt_id
                                AND (i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')))
            ORDER BY m.cnt_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            DECLARE @stat VARCHAR(10), @blind VARCHAR(1);
            SELECT @stat = stat_cd, @blind = blind_yn FROM TMACNTM WHERE cnt_id = @p_cnt_id;
            DECLARE @mask BIT = CASE WHEN @blind = 'Y' AND @stat IN ('0', '1', '2') THEN 1 ELSE 0 END;

            SELECT dt.cnt_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, w.wh_nm, dt.lot_no,
                   CASE WHEN @mask = 1 THEN NULL ELSE dt.book_qty END AS book_qty,
                   dt.fin_qty,
                   CASE WHEN @mask = 1 THEN NULL ELSE dt.move_qty END AS move_qty,
                   CASE WHEN @mask = 1 THEN NULL ELSE dt.diff_qty END AS diff_qty,
                   r.minor_nm AS adj_reason_nm, dt.add_yn, dt.remark
            FROM TMACNTD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TSMMINOR r ON r.major_cd = 'MA0016' AND r.minor_cd = dt.adj_reason
            WHERE dt.cnt_id = @p_cnt_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 4) 헤더 저장 - 창고(wh_id) 필수
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_wh_id BIGINT = NULL,
    @p_cnt_title NVARCHAR(200) = NULL,
    @p_cnt_type VARCHAR(10) = NULL,
    @p_cnt_date VARCHAR(8) = NULL,
    @p_blind_yn VARCHAR(1) = NULL,
    @p_tol_qty NUMERIC(18,4) = NULL,
    @p_tol_rate NUMERIC(9,4) = NULL,
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
            DECLARE @stat VARCHAR(10), @app BIGINT;
            SELECT @stat = stat_cd, @app = app_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고실사를 찾을 수 없습니다.'; RETURN; END
            IF dbo.FN_AP_IS_LOCKED(@app) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'작성 상태의 재고실사만 수정하거나 삭제할 수 있습니다. (대상확정 이후는 실사 취소로 정리하세요)'; RETURN; END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0015' AND minor_cd = @p_cnt_type AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사유형이 올바르지 않습니다.'; RETURN; END
            IF ISNULL(@p_cnt_date, '') = '' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사 기준일을 입력하세요.'; RETURN; END
            IF ISNULL(@p_blind_yn, 'N') NOT IN ('Y', 'N') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'블라인드 값이 올바르지 않습니다.'; RETURN; END

            DECLARE @whtype VARCHAR(10), @whacc BIGINT;
            SELECT @whtype = ISNULL(wh_type, ''), @whacc = acc_id FROM TBAWH WHERE wh_id = @p_wh_id;
            IF @whacc IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사할 창고를 선택하세요.'; RETURN; END
            IF @whacc <> @p_acc_id BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'다른 사업장의 창고는 선택할 수 없습니다.'; RETURN; END
            IF @whtype = 'TR' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이동중 재고 창고는 실사 대상이 아닙니다. (이동 차이는 외주이전 차이처리에서 다룹니다)'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMACNTM', 'cnt_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMACNTM (acc_id, cnt_no, wh_id, cnt_title, cnt_type, cnt_date, blind_yn, tol_qty, tol_rate, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                 reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_wh_id, @p_cnt_title, @p_cnt_type, @p_cnt_date, ISNULL(@p_blind_yn, 'N'), @p_tol_qty, @p_tol_rate, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_cnt_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMACNTM SET
                acc_id = @p_acc_id, wh_id = @p_wh_id, cnt_title = @p_cnt_title, cnt_type = @p_cnt_type, cnt_date = @p_cnt_date, blind_yn = ISNULL(@p_blind_yn, 'N'),
                tol_qty = @p_tol_qty, tol_rate = @p_tol_rate, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cnt_id = @p_cnt_id;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMACNTM WHERE cnt_id = @p_cnt_id;

        SET @GeneratedCode = CAST(@p_cnt_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 5) 라인 저장 - 계획 외 라인추가(N)의 창고는 헤더 창고로 고정 (@p_wh_id 파라미터 제거)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,               /* N(계획 외 추가)에서만 */
    @p_lot_no NVARCHAR(50) = NULL,          /* N에서만 */
    @p_cnt_qty NUMERIC(18,4) = NULL,        /* 실사수량 - 재실사 지정 라인이면 재실사 수량으로 들어간다. NULL이면 수량은 그대로 */
    @p_adj_reason VARCHAR(10) = NULL,       /* MA0016 */
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
        DECLARE @stat VARCHAR(10), @app BIGINT, @acc BIGINT, @no VARCHAR(20), @snap BIGINT, @cdate VARCHAR(8), @hdr_wh BIGINT;
        SELECT @stat = stat_cd, @app = app_id, @acc = acc_id, @no = cnt_no, @snap = snap_trans_id, @cdate = cnt_date, @hdr_wh = wh_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;

        IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고실사를 찾을 수 없습니다.'; RETURN; END
        IF dbo.FN_AP_IS_LOCKED(@app) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END
        IF @stat = '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'대상확정 후에 라인을 다룰 수 있습니다.'; RETURN; END
        IF @stat IN ('C', 'X') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정/취소된 재고실사는 수정할 수 없습니다.'; RETURN; END
        IF @stat = '1' AND @p_work_type <> 'D' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'대상확정 상태에서는 라인 삭제만 가능합니다. 실사를 시작한 뒤 수량을 입력하세요.'; RETURN; END
        IF @stat = '3' AND @p_work_type <> 'U' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'입력완료 상태에서는 조정사유/비고만 수정할 수 있습니다. (수량 수정은 재실사 지정 또는 입력완료 취소)'; RETURN; END

        IF @p_work_type IN ('U', 'D') AND NOT EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id AND serl = @p_serl)
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사 라인을 찾을 수 없습니다.'; RETURN; END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_cnt_qty < 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사수량은 0 이상이어야 합니다.'; RETURN; END
            IF ISNULL(@p_adj_reason, '') <> '' AND NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0016' AND minor_cd = @p_adj_reason AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'조정사유가 올바르지 않습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @hdr_wh IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'실사 헤더에 창고가 없습니다. 창고를 지정해 저장하세요.'; RETURN; END

            DECLARE @item_stock VARCHAR(1), @item_lot VARCHAR(1), @unit VARCHAR(10);
            SELECT @item_stock = ISNULL(stock_yn, 'N'), @item_lot = ISNULL(lot_yn, 'N'), @unit = unit_cd FROM TBAITEM WHERE item_id = @p_item_id;
            IF @item_stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
            IF @item_stock <> 'Y' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고관리 품목만 실사할 수 있습니다.'; RETURN; END
            IF @item_lot = 'Y' AND ISNULL(@p_lot_no, N'') = N'' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'LOT 관리 품목은 LOT를 입력하세요.'; RETURN; END
            IF @item_lot <> 'Y' SET @p_lot_no = N'';
            SET @p_lot_no = ISNULL(@p_lot_no, N'');

            IF EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id AND item_id = @p_item_id AND wh_id = @hdr_wh AND loc_id = 0 AND lot_no = @p_lot_no)
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 이 실사에 있는 품목/LOT입니다.'; RETURN; END

            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id;

            -- 장부수량 = 지금 재고 - 스냅샷 이후 변동(스냅샷 시점 수량. 스냅샷 이후 생긴 LOT면 0)
            DECLARE @book NUMERIC(18,4) = ISNULL((SELECT stock_qty FROM TMASTOCK WHERE acc_id = @acc AND item_id = @p_item_id AND wh_id = @hdr_wh AND loc_id = 0 AND lot_no = @p_lot_no), 0)
                                          - dbo.FN_MA_CNT_MOVE(@acc, @p_item_id, @hdr_wh, 0, @p_lot_no, @snap, '99991231', @p_cnt_id);

            INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, cnt_qty, recnt_yn, fin_qty, add_yn, adj_reason, stock_yn,
                                 cnt_user_id, cnt_dt, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_cnt_id, @p_serl, @acc, @no, @p_item_id, @unit, @hdr_wh, 0, @p_lot_no, @book, @p_cnt_qty, 'N', @p_cnt_qty, 'Y', NULLIF(@p_adj_reason, ''), 'Y',
                    CASE WHEN @p_cnt_qty IS NULL THEN NULL ELSE @p_user_id END, CASE WHEN @p_cnt_qty IS NULL THEN NULL ELSE GETDATE() END,
                    @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 상태 2에서만 수량을 쓴다(상태 3은 사유/비고만). 재실사 지정 라인이면 재실사 수량 칸에 들어간다.
            DECLARE @write_qty BIT = CASE WHEN @stat = '2' AND @p_cnt_qty IS NOT NULL THEN 1 ELSE 0 END;

            UPDATE TMACNTD SET
                cnt_qty   = CASE WHEN @write_qty = 1 AND recnt_yn = 'N' THEN @p_cnt_qty ELSE cnt_qty END,
                recnt_qty = CASE WHEN @write_qty = 1 AND recnt_yn = 'Y' THEN @p_cnt_qty ELSE recnt_qty END,
                fin_qty   = CASE WHEN @write_qty = 1 THEN @p_cnt_qty ELSE fin_qty END,
                cnt_user_id = CASE WHEN @write_qty = 1 THEN @p_user_id ELSE cnt_user_id END,
                cnt_dt      = CASE WHEN @write_qty = 1 THEN GETDATE() ELSE cnt_dt END,
                adj_reason = NULLIF(@p_adj_reason, ''), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cnt_id = @p_cnt_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMACNTD WHERE cnt_id = @p_cnt_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 6) 대상확정 - 헤더 창고의 재고(0/음수 행 포함)를 라인으로 스냅샷. 대상이 0건이어도 통과, 같은 창고의 다른 진행 실사가 있어도 막지 않는다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_SNAP_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @wh BIGINT;
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @wh = wh_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 대상확정할 수 있습니다.', 1;
    IF @wh IS NULL THROW 50001, N'실사할 창고를 먼저 선택해 저장하세요.', 1;

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);

    INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, recnt_yn, add_yn, stock_yn,
                         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @p_cnt_id, ROW_NUMBER() OVER (ORDER BY i.item_no, s.lot_no), @acc, @no, s.item_id, ISNULL(s.unit_cd, i.unit_cd), s.wh_id, s.loc_id, s.lot_no,
           s.stock_qty - dbo.FN_MA_CNT_MOVE(s.acc_id, s.item_id, s.wh_id, s.loc_id, s.lot_no, @snap, '99991231', @p_cnt_id),
           'N', 'N', 'Y', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id
    WHERE s.acc_id = @acc AND s.wh_id = @wh AND i.stock_yn = 'Y';

    -- 대상 재고(라인)가 0건이어도 대상확정은 통과한다 - 빈 창고/새 품목은 실사중 '라인추가'로 넣는다(입력완료는 라인이 1건 이상 있어야 한다).
    UPDATE TMACNTM SET stat_cd = '1', freeze_mode = 'B', snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO
