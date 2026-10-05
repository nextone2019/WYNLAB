-- 재고실사 프로시저 (2026-10-04, WYNLAB_DEV 전용). 테이블/코드는 268번, 설계는 Document\재고실사_설계서.md
--
--   FN_MA_CNT_MOVE         한 재고(품목/창고/위치/LOT)의 스냅샷 이후 ~ 기준일까지 확정 수불 순증감(변동). 이 실사 자신이 만든 조정 수불은 제외.
--   USP_MA_CNT_Q           조회(Q: 헤더 + 대상창고 + 라인 3개 결과셋) - 블라인드 실사는 입력완료 전 장부수량/변동/차이를 가린다
--   USP_MA_CNTLIST_Q       현황(Q: 목록, Q1: 한 실사의 라인)
--   USP_MA_CNT_S           헤더 저장(N/U/D) - 작성(0) 상태에서만 수정/삭제
--   USP_MA_CNT_W_S         대상 창고/조건 저장(N/U/D) - 작성(0) 상태에서만, 이동중(TR) 창고 불가
--   USP_MA_CNT_S_1         라인 저장(N/U/D) - 상태별 허용 범위가 다르다(아래)
--   USP_MA_CNT_SNAP_CORE   대상확정(0->1): 그 시점 TMASTOCK을 라인으로 스냅샷
--   USP_MA_CNT_STATE_CORE  START(1->2) / DONE(2->3) / UNDONE(3->2) / RECNT(3->2, 선택 라인 재실사) / X(취소)
--   USP_MA_CNT_CONFIRM_CORE / _CANCEL_CORE   확정(조정 수불 생성) / 확정취소(역거래)
--   USP_MA_CNT_C_S         화면용 래퍼: SNAP / START / DONE / UNDONE / RECNT / C / CC / X
--
-- 라인 저장 허용 범위: 상태 0 불가 / 1 삭제만 / 2 수량 입력·계획 외 추가·삭제·사유·비고 / 3 사유·비고만 / C, X 불가. 결재 상신(FN_AP_IS_LOCKED)이면 전부 불가.
-- 핵심 규칙: 선별된 라인은 삭제하지 않는 한 전부 실사수량(0 유효)을 입력해야 입력완료할 수 있고, 전부 확정에 반영된다. 장부와 같으면 차이 0(수불 미생성).
-- 변동 보정(방식 B): diff = 최종실사수량 - (스냅샷수량 + 스냅샷 이후 기준일까지 확정 수불 순증감).
-- 내부(CORE) 프로시저는 오류를 THROW 50001로 올리고, 화면용 래퍼(_C_S)가 그 문구를 돌려준다.

-- ============================================================
-- 0) 변동 계산 함수
-- ============================================================
CREATE OR ALTER FUNCTION dbo.FN_MA_CNT_MOVE (
    @acc_id BIGINT, @item_id BIGINT, @wh_id BIGINT, @loc_id BIGINT, @lot_no NVARCHAR(50),
    @snap_trans_id BIGINT, @cnt_date VARCHAR(8), @cnt_id BIGINT
)
RETURNS NUMERIC(18,4)
AS
BEGIN
    RETURN ISNULL((
        SELECT SUM(CASE WHEN t.trans_kind = 'I' THEN t.qty ELSE -t.qty END)
        FROM TMATRANS t
        WHERE t.acc_id = @acc_id AND t.item_id = @item_id AND t.wh_id = @wh_id
          AND t.loc_id = ISNULL(@loc_id, 0) AND t.lot_no = ISNULL(@lot_no, N'')
          AND t.stock_yn = 'Y'
          AND t.trans_id > ISNULL(@snap_trans_id, 9223372036854775807)
          AND t.trans_date <= ISNULL(@cnt_date, '99991231')
          AND NOT (t.src_type = 'STKCNT' AND t.src_id = @cnt_id)
    ), 0);
END
GO

-- ============================================================
-- 1) 조회 - 헤더 / 대상창고 / 라인
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
                   m.snap_trans_id, m.snap_dt, m.tol_qty, m.tol_rate,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                   m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMACNTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.cnt_id = @match_id;

            SELECT w.cnt_id, w.serl, w.wh_id, h.wh_nm, w.grp_id, w.item_id, i.item_no, i.item_nm
            FROM TMACNTW w
                LEFT JOIN TBAWH h ON h.wh_id = w.wh_id
                LEFT JOIN TBAITEM i ON i.item_id = w.item_id
            WHERE w.cnt_id = @match_id
            ORDER BY w.serl;

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
-- 2) 현황 - Q 목록 / Q1 한 실사의 라인
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
                   (SELECT STRING_AGG(x.wh_nm, N', ') FROM (SELECT DISTINCT h.wh_nm FROM TMACNTW cw JOIN TBAWH h ON h.wh_id = cw.wh_id WHERE cw.cnt_id = m.cnt_id) x) AS wh_nms,
                   (SELECT COUNT(*) FROM TMACNTD x WHERE x.cnt_id = m.cnt_id) AS line_cnt,
                   (SELECT COUNT(*) FROM TMACNTD x WHERE x.cnt_id = m.cnt_id AND x.fin_qty IS NOT NULL) AS entered_cnt,
                   CASE WHEN m.stat_cd IN ('3', 'C') THEN (SELECT COUNT(*) FROM TMACNTD x WHERE x.cnt_id = m.cnt_id AND ISNULL(x.diff_qty, 0) <> 0) END AS diff_cnt,
                   d.dept_nm, e.emp_nm, m.cfm_dt, m.app_no, m.remark
            FROM TMACNTM m
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_cnt_no, '') = '' OR m.cnt_no LIKE '%' + @p_cnt_no + '%')
              AND (ISNULL(@p_date_from, '') = '' OR m.cnt_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR m.cnt_date <= @p_date_to)
              AND (ISNULL(@p_cnt_type, '') = '' OR m.cnt_type = @p_cnt_type)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (@p_wh_id IS NULL OR EXISTS (SELECT 1 FROM TMACNTW cw WHERE cw.cnt_id = m.cnt_id AND cw.wh_id = @p_wh_id))
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
-- 3) 헤더 저장
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
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
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMACNTM', 'cnt_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMACNTM (acc_id, cnt_no, cnt_title, cnt_type, cnt_date, blind_yn, tol_qty, tol_rate, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                 reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_cnt_title, @p_cnt_type, @p_cnt_date, ISNULL(@p_blind_yn, 'N'), @p_tol_qty, @p_tol_rate, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_cnt_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMACNTM SET
                acc_id = @p_acc_id, cnt_title = @p_cnt_title, cnt_type = @p_cnt_type, cnt_date = @p_cnt_date, blind_yn = ISNULL(@p_blind_yn, 'N'),
                tol_qty = @p_tol_qty, tol_rate = @p_tol_rate, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cnt_id = @p_cnt_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMACNTW WHERE cnt_id = @p_cnt_id;
            DELETE FROM TMACNTM WHERE cnt_id = @p_cnt_id;
        END

        SET @GeneratedCode = CAST(@p_cnt_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 4) 대상 창고/조건 저장 (작성 상태에서만)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_W_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_wh_id BIGINT = NULL,
    @p_grp_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
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
        DECLARE @stat VARCHAR(10), @acc BIGINT, @app BIGINT;
        SELECT @stat = stat_cd, @acc = acc_id, @app = app_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;
        IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고실사를 찾을 수 없습니다.'; RETURN; END
        IF dbo.FN_AP_IS_LOCKED(@app) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정할 수 없습니다.'; RETURN; END
        IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'작성 상태에서만 대상 창고를 바꿀 수 있습니다.'; RETURN; END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            DECLARE @whtype VARCHAR(10), @whacc BIGINT;
            SELECT @whtype = ISNULL(wh_type, ''), @whacc = acc_id FROM TBAWH WHERE wh_id = @p_wh_id;
            IF @whacc IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'창고를 선택하세요.'; RETURN; END
            IF @whacc <> @acc BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'다른 사업장의 창고는 선택할 수 없습니다.'; RETURN; END
            IF @whtype = 'TR' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이동중 재고 창고는 실사 대상이 아닙니다. (이동 차이는 외주이전 차이처리에서 다룹니다)'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TMACNTW WHERE cnt_id = @p_cnt_id;
            INSERT INTO TMACNTW (cnt_id, serl, wh_id, grp_id, item_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_cnt_id, @p_serl, @p_wh_id, @p_grp_id, @p_item_id, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
            UPDATE TMACNTW SET wh_id = @p_wh_id, grp_id = @p_grp_id, item_id = @p_item_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE cnt_id = @p_cnt_id AND serl = @p_serl;
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMACNTW WHERE cnt_id = @p_cnt_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 5) 라인 저장 - 상태별 허용 범위(맨 위 머리말 참고)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,               /* N(계획 외 추가)에서만 */
    @p_wh_id BIGINT = NULL,                 /* N에서만 */
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
        DECLARE @stat VARCHAR(10), @app BIGINT, @acc BIGINT, @no VARCHAR(20), @snap BIGINT, @cdate VARCHAR(8);
        SELECT @stat = stat_cd, @app = app_id, @acc = acc_id, @no = cnt_no, @snap = snap_trans_id, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @p_cnt_id;

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
            IF @p_item_id IS NULL OR @p_wh_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목과 창고를 입력하세요.'; RETURN; END

            DECLARE @item_stock VARCHAR(1), @item_lot VARCHAR(1), @unit VARCHAR(10), @whtype VARCHAR(10), @whacc BIGINT;
            SELECT @item_stock = ISNULL(stock_yn, 'N'), @item_lot = ISNULL(lot_yn, 'N'), @unit = unit_cd FROM TBAITEM WHERE item_id = @p_item_id;
            IF @item_stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
            IF @item_stock <> 'Y' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고관리 품목만 실사할 수 있습니다.'; RETURN; END
            IF @item_lot = 'Y' AND ISNULL(@p_lot_no, N'') = N'' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'LOT 관리 품목은 LOT를 입력하세요.'; RETURN; END
            IF @item_lot <> 'Y' SET @p_lot_no = N'';
            SET @p_lot_no = ISNULL(@p_lot_no, N'');

            SELECT @whtype = ISNULL(wh_type, ''), @whacc = acc_id FROM TBAWH WHERE wh_id = @p_wh_id;
            IF @whacc IS NULL OR @whacc <> @acc BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'창고가 올바르지 않습니다.'; RETURN; END
            IF @whtype = 'TR' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이동중 재고 창고는 실사 대상이 아닙니다.'; RETURN; END

            IF EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id AND item_id = @p_item_id AND wh_id = @p_wh_id AND loc_id = 0 AND lot_no = @p_lot_no)
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 이 실사에 있는 품목/창고/LOT입니다.'; RETURN; END

            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id;

            -- 장부수량 = 지금 재고 - 스냅샷 이후 변동(스냅샷 시점 수량. 스냅샷 이후 생긴 LOT면 0)
            DECLARE @book NUMERIC(18,4) = ISNULL((SELECT stock_qty FROM TMASTOCK WHERE acc_id = @acc AND item_id = @p_item_id AND wh_id = @p_wh_id AND loc_id = 0 AND lot_no = @p_lot_no), 0)
                                          - dbo.FN_MA_CNT_MOVE(@acc, @p_item_id, @p_wh_id, 0, @p_lot_no, @snap, '99991231', @p_cnt_id);

            INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, cnt_qty, recnt_yn, fin_qty, add_yn, adj_reason, stock_yn,
                                 cnt_user_id, cnt_dt, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_cnt_id, @p_serl, @acc, @no, @p_item_id, @unit, @p_wh_id, 0, @p_lot_no, @book, @p_cnt_qty, 'N', @p_cnt_qty, 'Y', NULLIF(@p_adj_reason, ''), 'Y',
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
-- 6) 대상확정 - 그 시점의 TMASTOCK을 라인으로 스냅샷 (0 -> 1)
--    장부수량 = 지금 재고 - (스냅샷 기준점 이후 변동) : 기준점 조회와 재고 조회 사이에 다른 수불이 확정돼도 이중 반영되지 않게 한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_SNAP_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @cdate VARCHAR(8), @msg NVARCHAR(2048);
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 대상확정할 수 있습니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TMACNTW WHERE cnt_id = @p_cnt_id) THROW 50001, N'대상 창고를 먼저 입력하세요.', 1;

    -- 같은 창고는 실사를 하나만 진행한다(대상확정~입력완료 사이). 방치된 실사는 취소로 창고를 푼다.
    DECLARE @busy NVARCHAR(100);
    SELECT TOP 1 @busy = m.cnt_no + N' / ' + ISNULL(h.wh_nm, N'')
    FROM TMACNTM m
        JOIN TMACNTW w ON w.cnt_id = m.cnt_id
        JOIN TMACNTW me ON me.cnt_id = @p_cnt_id AND me.wh_id = w.wh_id
        LEFT JOIN TBAWH h ON h.wh_id = w.wh_id
    WHERE m.cnt_id <> @p_cnt_id AND m.acc_id = @acc AND m.stat_cd IN ('1', '2', '3');
    IF @busy IS NOT NULL
    BEGIN
        SET @msg = N'같은 창고에 진행 중인 재고실사가 있습니다. (' + @busy + N') 먼저 확정하거나 취소하세요.';
        THROW 50001, @msg, 1;
    END

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);

    INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, recnt_yn, add_yn, stock_yn,
                         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @p_cnt_id, ROW_NUMBER() OVER (ORDER BY i.item_no, s.wh_id, s.lot_no), @acc, @no, s.item_id, ISNULL(s.unit_cd, i.unit_cd), s.wh_id, s.loc_id, s.lot_no,
           s.stock_qty - dbo.FN_MA_CNT_MOVE(s.acc_id, s.item_id, s.wh_id, s.loc_id, s.lot_no, @snap, '99991231', @p_cnt_id),
           'N', 'N', 'Y', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id
    WHERE s.acc_id = @acc AND i.stock_yn = 'Y'
      AND EXISTS (SELECT 1 FROM TMACNTW w
                  WHERE w.cnt_id = @p_cnt_id AND w.wh_id = s.wh_id
                    AND (w.grp_id IS NULL OR i.grp1_id = w.grp_id OR i.grp2_id = w.grp_id OR i.grp3_id = w.grp_id OR i.grp4_id = w.grp_id)
                    AND (w.item_id IS NULL OR w.item_id = s.item_id)
                    AND (s.stock_qty <> 0 OR w.item_id IS NOT NULL));

    IF NOT EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id)
        THROW 50001, N'대상 재고가 없습니다. 대상 창고/조건을 확인하세요.', 1;

    UPDATE TMACNTM SET stat_cd = '1', freeze_mode = 'B', snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 7) 상태 전이 - START / DONE / UNDONE / RECNT / X
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_STATE_CORE
    @p_cnt_id BIGINT,
    @p_action VARCHAR(10),                  /* START / DONE / UNDONE / RECNT / X */
    @p_serls VARCHAR(2000) = NULL,          /* RECNT - 재실사할 라인 순번(쉼표 구분) */
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @app BIGINT, @snap BIGINT, @cdate VARCHAR(8), @msg NVARCHAR(2048);
    SELECT @stat = stat_cd, @app = app_id, @snap = snap_trans_id, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @p_action IN ('UNDONE', 'RECNT', 'X') AND dbo.FN_AP_IS_LOCKED(@app) = 1
        THROW 50001, N'결재 상신된 재고실사는 처리할 수 없습니다. 먼저 결재를 취소하세요.', 1;

    BEGIN TRAN;

    IF @p_action = 'START'
    BEGIN
        IF @stat <> '1' THROW 50001, N'대상확정된 재고실사만 실사를 시작할 수 있습니다.', 1;
        UPDATE TMACNTM SET stat_cd = '2', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id;
    END
    ELSE IF @p_action = 'DONE'
    BEGIN
        IF @stat <> '2' THROW 50001, N'실사중인 재고실사만 입력완료할 수 있습니다.', 1;
        IF NOT EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id) THROW 50001, N'실사 라인이 없습니다.', 1;

        DECLARE @miss INT, @names NVARCHAR(300);
        SELECT @miss = COUNT(*) FROM TMACNTD WHERE cnt_id = @p_cnt_id AND fin_qty IS NULL;
        IF @miss > 0
        BEGIN
            SELECT @names = STRING_AGG(x.item_no, N', ') FROM (SELECT TOP 3 i.item_no FROM TMACNTD d JOIN TBAITEM i ON i.item_id = d.item_id
                                                              WHERE d.cnt_id = @p_cnt_id AND d.fin_qty IS NULL ORDER BY d.serl) x;
            SET @msg = N'실사수량이 입력되지 않은 라인이 ' + CAST(@miss AS NVARCHAR(10)) + N'건 있습니다. 모두 입력하거나(0 가능) 라인을 삭제하세요. (' + ISNULL(@names, N'') + CASE WHEN @miss > 3 THEN N' ...' ELSE N'' END + N')';
            THROW 50001, @msg, 1;
        END

        UPDATE d SET move_qty = mv.move_qty, diff_qty = d.fin_qty - (d.book_qty + mv.move_qty),
                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        FROM TMACNTD d
            CROSS APPLY (SELECT dbo.FN_MA_CNT_MOVE(d.acc_id, d.item_id, d.wh_id, d.loc_id, d.lot_no, @snap, @cdate, d.cnt_id) AS move_qty) mv
        WHERE d.cnt_id = @p_cnt_id;

        UPDATE TMACNTM SET stat_cd = '3', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id;
    END
    ELSE IF @p_action = 'UNDONE'
    BEGIN
        IF @stat <> '3' THROW 50001, N'입력완료된 재고실사만 입력완료를 취소할 수 있습니다.', 1;
        UPDATE TMACNTD SET move_qty = NULL, diff_qty = NULL WHERE cnt_id = @p_cnt_id;
        UPDATE TMACNTM SET stat_cd = '2', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id;
    END
    ELSE IF @p_action = 'RECNT'
    BEGIN
        IF @stat <> '3' THROW 50001, N'입력완료된 재고실사만 재실사를 지정할 수 있습니다.', 1;
        IF ISNULL(@p_serls, '') = '' THROW 50001, N'재실사할 라인을 선택하세요.', 1;

        UPDATE d SET recnt_yn = 'Y', recnt_qty = NULL, fin_qty = NULL, move_qty = NULL, diff_qty = NULL,
                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        FROM TMACNTD d
        WHERE d.cnt_id = @p_cnt_id AND d.serl IN (SELECT TRY_CAST(LTRIM(RTRIM(value)) AS INT) FROM STRING_SPLIT(@p_serls, ','));
        IF @@ROWCOUNT = 0 THROW 50001, N'선택한 라인을 찾을 수 없습니다.', 1;

        UPDATE TMACNTD SET move_qty = NULL, diff_qty = NULL WHERE cnt_id = @p_cnt_id;
        UPDATE TMACNTM SET stat_cd = '2', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id;
    END
    ELSE IF @p_action = 'X'
    BEGIN
        IF @stat NOT IN ('1', '2', '3') THROW 50001, N'대상확정~입력완료 상태의 재고실사만 취소할 수 있습니다. (작성 상태는 삭제, 확정은 확정취소 후 취소)', 1;
        UPDATE TMACNTM SET stat_cd = 'X', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id;
    END
    ELSE THROW 50001, N'알 수 없는 처리입니다.', 1;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 8) 확정 - 차이 라인마다 조정 수불 생성 + 현재고 반영 (3 -> C)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_CONFIRM_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @app BIGINT, @snap BIGINT, @cdate VARCHAR(8);
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @app = app_id, @snap = snap_trans_id, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '3' THROW 50001, N'입력완료된 재고실사만 확정할 수 있습니다.', 1;
    IF @app IS NOT NULL AND ISNULL((SELECT app_stat_cd FROM TAPDOC WHERE app_id = @app), '') <> 'E'
        THROW 50001, N'결재가 승인완료되어야 확정할 수 있습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMACNTD WHERE cnt_id = @p_cnt_id AND fin_qty IS NULL)
        THROW 50001, N'실사수량이 입력되지 않은 라인이 있어 확정할 수 없습니다.', 1;

    BEGIN TRAN;

    -- 입력완료 이후 ~ 지금까지 확정된 수불까지 반영해 차이를 다시 확정한다.
    UPDATE d SET move_qty = mv.move_qty, diff_qty = d.fin_qty - (d.book_qty + mv.move_qty)
    FROM TMACNTD d
        CROSS APPLY (SELECT dbo.FN_MA_CNT_MOVE(d.acc_id, d.item_id, d.wh_id, d.loc_id, d.lot_no, @snap, @cdate, d.cnt_id) AS move_qty) mv
    WHERE d.cnt_id = @p_cnt_id;

    DECLARE @noreason INT;
    SELECT @noreason = COUNT(*) FROM TMACNTD WHERE cnt_id = @p_cnt_id AND diff_qty <> 0 AND ISNULL(adj_reason, '') = '';
    IF @noreason > 0
    BEGIN
        SET @msg = N'차이가 있는 라인 ' + CAST(@noreason AS NVARCHAR(10)) + N'건의 조정사유를 입력하세요.';
        THROW 50001, @msg, 1;
    END

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @wh BIGINT, @loc BIGINT, @lot NVARCHAR(50), @diff NUMERIC(18,4), @qty NUMERIC(18,4),
            @kind VARCHAR(1), @type VARCHAR(10), @cur NUMERIC(18,4), @tid BIGINT, @nm NVARCHAR(200);
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, wh_id, loc_id, lot_no, diff_qty FROM TMACNTD
        WHERE cnt_id = @p_cnt_id AND diff_qty <> 0 AND stock_yn = 'Y' ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @wh, @loc, @lot, @diff;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @qty = ABS(@diff);
        SET @kind = CASE WHEN @diff > 0 THEN 'I' ELSE 'O' END;
        SET @type = CASE WHEN @diff > 0 THEN 'ADJ_IN' ELSE 'ADJ_OUT' END;

        IF @kind = 'O'
        BEGIN
            SELECT @cur = stock_qty FROM TMASTOCK WITH (UPDLOCK, HOLDLOCK)
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF ISNULL(@cur, 0) < @qty
            BEGIN
                SELECT @nm = item_nm FROM TBAITEM WHERE item_id = @item;
                SET @msg = N'현재고가 부족해 조정할 수 없습니다. 품목: ' + ISNULL(@nm, N'') + N', LOT: ' + @lot
                         + N', 현재고 ' + CAST(CAST(ISNULL(@cur, 0) AS FLOAT) AS NVARCHAR(30)) + N' < 감소 ' + CAST(CAST(@qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @msg, 1;
            END
        END

        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, @cdate, @kind, @type, @item, @wh, @loc, @lot, @unit, @qty, 'Y', NULL,
                'STKCNT', @p_cnt_id, @no, @serl, NULL, N'재고실사 조정', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        UPDATE TMACNTD SET trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id AND serl = @serl;

        UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET stock_qty = stock_qty + CASE WHEN @kind = 'I' THEN @qty ELSE -@qty END, trans_id = @tid,
                                                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
        IF @@ROWCOUNT = 0
            INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@acc, @item, @wh, @loc, @lot, @unit, @qty, @tid, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);

        FETCH NEXT FROM c INTO @serl, @item, @unit, @wh, @loc, @lot, @diff;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMACNTM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 9) 확정취소 - 조정 수불을 역거래로 되돌린다 (C -> 3). 되돌리면 재고가 모자라질 때(조정 입고분을 이미 썼을 때)는 불가.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_CANCEL_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20);
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no FROM TMACNTM WHERE cnt_id = @p_cnt_id;
    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> 'C' THROW 50001, N'확정된 재고실사만 확정취소할 수 있습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @wh BIGINT, @loc BIGINT, @lot NVARCHAR(50), @diff NUMERIC(18,4), @qty NUMERIC(18,4),
            @kind VARCHAR(1), @type VARCHAR(10), @orig BIGINT, @cur NUMERIC(18,4), @tid BIGINT, @nm NVARCHAR(200);
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, wh_id, loc_id, lot_no, diff_qty, trans_id FROM TMACNTD
        WHERE cnt_id = @p_cnt_id AND trans_id IS NOT NULL ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @wh, @loc, @lot, @diff, @orig;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @qty = ABS(@diff);
        -- 원 거래가 입고(ADJ_IN)였으면 취소는 출고, 출고(ADJ_OUT)였으면 입고
        SET @kind = CASE WHEN @diff > 0 THEN 'O' ELSE 'I' END;
        SET @type = CASE WHEN @diff > 0 THEN 'ADJ_IN' ELSE 'ADJ_OUT' END;

        IF @kind = 'O'
        BEGIN
            SELECT @cur = stock_qty FROM TMASTOCK WITH (UPDLOCK, HOLDLOCK)
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF ISNULL(@cur, 0) < @qty
            BEGIN
                SELECT @nm = item_nm FROM TBAITEM WHERE item_id = @item;
                SET @msg = N'확정취소할 수 없습니다. 조정으로 늘어난 재고를 이미 사용했습니다. 품목: ' + ISNULL(@nm, N'') + N', LOT: ' + @lot
                         + N', 현재고 ' + CAST(CAST(ISNULL(@cur, 0) AS FLOAT) AS NVARCHAR(30)) + N' < 되돌릴 수량 ' + CAST(CAST(@qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @msg, 1;
            END
        END

        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, CONVERT(VARCHAR(8), GETDATE(), 112), @kind, @type, @item, @wh, @loc, @lot, @unit, @qty, 'Y', NULL,
                'STKCNT', @p_cnt_id, @no, @serl, @orig, N'재고실사 확정취소', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET stock_qty = stock_qty + CASE WHEN @kind = 'I' THEN @qty ELSE -@qty END, trans_id = @tid,
                                                     upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
        IF @@ROWCOUNT = 0
            INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@acc, @item, @wh, @loc, @lot, @unit, @qty, @tid, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);

        UPDATE TMACNTD SET trans_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id AND serl = @serl;
        FETCH NEXT FROM c INTO @serl, @item, @unit, @wh, @loc, @lot, @diff, @orig;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMACNTM SET stat_cd = '3', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO

-- ============================================================
-- 10) 화면용 래퍼 - SNAP 대상확정 / START 실사시작 / DONE 입력완료 / UNDONE 입력완료취소 / RECNT 재실사지정 / C 확정 / CC 확정취소 / X 실사취소
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_CNT_C_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cnt_id BIGINT = NULL,
    @p_serls VARCHAR(2000) = NULL,          /* RECNT - 재실사할 라인 순번(쉼표 구분) */
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
        IF @p_work_type = 'SNAP' EXEC USP_MA_CNT_SNAP_CORE @p_cnt_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type IN ('START', 'DONE', 'UNDONE', 'RECNT', 'X') EXEC USP_MA_CNT_STATE_CORE @p_cnt_id, @p_work_type, @p_serls, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'C' EXEC USP_MA_CNT_CONFIRM_CORE @p_cnt_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC' EXEC USP_MA_CNT_CANCEL_CORE @p_cnt_id, @p_user_id, @p_client_pc;
        ELSE THROW 50001, N'알 수 없는 처리입니다.', 1;
        SET @GeneratedCode = CAST(@p_cnt_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
