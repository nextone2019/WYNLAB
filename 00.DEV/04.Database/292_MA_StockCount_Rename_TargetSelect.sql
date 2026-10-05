-- 재고실사 '대상확정' 명칭을 '대상선별'로 변경 (2026-10-05, WYNLAB_DEV 전용)
--  - 상태 코드 MA0014의 '1' 명칭, 프로시저/함수 안의 안내·오류 문구/주석만 바뀐다(로직 변경 없음). 대상선별 버튼 = 스냅샷 + 곧바로 실사중(289).
--  - 프로시저는 라이브 정의(OBJECT_DEFINITION)에서 문구만 치환해 CREATE OR ALTER 한다. 여러 번 실행해도 안전하다.
UPDATE TSMMINOR SET minor_nm = N'대상선별' WHERE major_cd = 'MA0014' AND minor_cd = '1' AND minor_nm = N'대상확정';
GO
-- 재고실사 프로시저 (2026-10-04, WYNLAB_DEV 전용). 테이블/코드는 268번, 설계는 Document\재고실사_설계서.md
--
--   FN_MA_CNT_MOVE         한 재고(품목/창고/위치/LOT)의 스냅샷 이후 ~ 기준일까지 확정 수불 순증감(변동). 이 실사 자신이 만든 조정 수불은 제외.
--   USP_MA_CNT_Q           조회(Q: 헤더 + 대상창고 + 라인 3개 결과셋) - 블라인드 실사는 입력완료 전 장부수량/변동/차이를 가린다
--   USP_MA_CNTLIST_Q       현황(Q: 목록, Q1: 한 실사의 라인)
--   USP_MA_CNT_S           헤더 저장(N/U/D) - 작성(0) 상태에서만 수정/삭제
--   USP_MA_CNT_W_S         대상 창고/조건 저장(N/U/D) - 작성(0) 상태에서만, 이동중(TR) 창고 불가
--   USP_MA_CNT_S_1         라인 저장(N/U/D) - 상태별 허용 범위가 다르다(아래)
--   USP_MA_CNT_SNAP_CORE   대상선별(0->1): 그 시점 TMASTOCK을 라인으로 스냅샷
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
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'작성 상태의 재고실사만 수정하거나 삭제할 수 있습니다. (대상선별 이후는 실사 취소로 정리하세요)'; RETURN; END
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
-- 라인추가 - 다른 진행 중 실사에 있는 품목/LOT 거부 (273의 USP_MA_CNT_S_1에 검사 한 단락 추가)
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
        IF @stat = '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'대상선별 후에 라인을 다룰 수 있습니다.'; RETURN; END
        IF @stat IN ('C', 'X') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정/취소된 재고실사는 수정할 수 없습니다.'; RETURN; END
        IF @stat = '1' AND @p_work_type <> 'D' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'대상선별 상태에서는 라인 삭제만 가능합니다. 실사를 시작한 뒤 수량을 입력하세요.'; RETURN; END
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

            -- 다른 진행 중 실사(대상선별/실사중/입력완료)에 이미 있는 재고는 담을 수 없다 - 같은 재고를 두 실사가 조정하면 이중 반영된다.
            DECLARE @other_no VARCHAR(20);
            SELECT TOP 1 @other_no = xm.cnt_no
            FROM TMACNTD x JOIN TMACNTM xm ON xm.cnt_id = x.cnt_id
            WHERE xm.cnt_id <> @p_cnt_id AND xm.stat_cd IN ('1', '2', '3')
              AND x.item_id = @p_item_id AND x.wh_id = @hdr_wh AND x.loc_id = 0 AND x.lot_no = @p_lot_no;
            IF @other_no IS NOT NULL
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 진행 중인 재고실사(' + @other_no + N')에 포함된 품목/LOT입니다. 그 실사를 확정하거나 취소한 뒤 추가하세요.'; RETURN; END

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
-- 재고실사: 대상선별(1)과 실사시작(2)을 한 단계로 합친다 (2026-10-05, WYNLAB_DEV)
--  - USP_MA_CNT_SNAP_CORE(= 화면 [실사시작] 버튼의 SNAP): 스냅샷을 뜨면 곧바로 stat_cd='2'(실사중)로 간다. 라인 추가/삭제/수량 입력이 바로 열린다.
--  - 기존에 '1'(대상선별)에 머물던 문서는 '2'(실사중)로 옮긴다. 상태 '1'은 더 이상 생기지 않는다(START 액션은 그대로 두되 대상 상태가 없다).
--  - 래퍼(USP_MA_CNT_C_S)의 제외 안내 문구만 '실사시작'으로 바꾼다.
-- 273의 USP_MA_CNT_SNAP_CORE(= 라이브 정의)에서 UPDATE 한 곳과 메시지만 바뀜. 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_MA_CNT_SNAP_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    @o_skipped INT = 0 OUTPUT,                  /* 다른 진행 중 실사에 이미 있어 제외한 재고 행 수 */
    @o_skip_docs NVARCHAR(100) = NULL OUTPUT    /* 그 실사번호들(최대 3개) */
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @wh BIGINT;
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @wh = wh_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 대상선별할 수 있습니다.', 1;
    IF @wh IS NULL THROW 50001, N'실사할 창고를 먼저 선택해 저장하세요.', 1;

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);
    -- 수불 통제 방식(시스템설정 MA.CNT_FREEZE): A 완전 동결 / B 변동 보정(기본). 이 실사가 시작되는 시점의 값을 문서에 고정한다.
    DECLARE @freeze VARCHAR(1) = CASE WHEN dbo.FSM_PROCCONFIG('MA.CNT_FREEZE') = 'A' THEN 'A' ELSE 'B' END;

    -- 다른 진행 중 실사에 이미 있는 재고 행(제외 대상)
    DECLARE @skip TABLE (item_id BIGINT, lot_no NVARCHAR(50), loc_id BIGINT, cnt_no VARCHAR(20));
    INSERT INTO @skip (item_id, lot_no, loc_id, cnt_no)
    SELECT s.item_id, s.lot_no, s.loc_id, xm.cnt_no
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id AND i.stock_yn = 'Y'
        JOIN TMACNTD x ON x.item_id = s.item_id AND x.wh_id = s.wh_id AND x.loc_id = s.loc_id AND x.lot_no = s.lot_no
        JOIN TMACNTM xm ON xm.cnt_id = x.cnt_id AND xm.cnt_id <> @p_cnt_id AND xm.stat_cd IN ('1', '2', '3')
    WHERE s.acc_id = @acc AND s.wh_id = @wh;

    SELECT @o_skipped = COUNT(*) FROM (SELECT DISTINCT item_id, lot_no, loc_id FROM @skip) k;
    SELECT @o_skip_docs = STRING_AGG(d.cnt_no, ', ') FROM (SELECT DISTINCT TOP 3 cnt_no FROM @skip ORDER BY cnt_no) d;

    INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, recnt_yn, add_yn, stock_yn,
                         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @p_cnt_id, ROW_NUMBER() OVER (ORDER BY i.item_no, s.lot_no), @acc, @no, s.item_id, ISNULL(s.unit_cd, i.unit_cd), s.wh_id, s.loc_id, s.lot_no,
           s.stock_qty - dbo.FN_MA_CNT_MOVE(s.acc_id, s.item_id, s.wh_id, s.loc_id, s.lot_no, @snap, '99991231', @p_cnt_id),
           'N', 'N', 'Y', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id
    WHERE s.acc_id = @acc AND s.wh_id = @wh AND i.stock_yn = 'Y'
      AND NOT EXISTS (SELECT 1 FROM @skip k WHERE k.item_id = s.item_id AND k.lot_no = s.lot_no AND k.loc_id = s.loc_id);

    -- 대상 재고(라인)가 0건이어도 시작은 통과한다 - 빈 창고/새 품목은 '라인추가'로 넣는다(입력완료는 라인이 1건 이상 있어야 한다).
    UPDATE TMACNTM SET stat_cd = '2', freeze_mode = @freeze, snap_trans_id = @snap, snap_dt = GETDATE(),
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
        IF @stat <> '1' THROW 50001, N'대상선별된 재고실사만 실사를 시작할 수 있습니다.', 1;
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
        IF @stat NOT IN ('1', '2', '3') THROW 50001, N'대상선별~입력완료 상태의 재고실사만 취소할 수 있습니다. (작성 상태는 삭제, 확정은 확정취소 후 취소)', 1;
        UPDATE TMACNTM SET stat_cd = 'X', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE cnt_id = @p_cnt_id;
    END
    ELSE THROW 50001, N'알 수 없는 처리입니다.', 1;

    COMMIT TRAN;
END
GO
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
        IF @p_work_type = 'SNAP'
        BEGIN
            DECLARE @skipped INT = 0, @skip_docs NVARCHAR(100);
            EXEC USP_MA_CNT_SNAP_CORE @p_cnt_id, @p_user_id, @p_client_pc, @skipped OUTPUT, @skip_docs OUTPUT;
            IF @skipped > 0
                SET @ReturnMsg = LEFT(N'대상선별 완료. 다른 진행 중 실사(' + ISNULL(@skip_docs, N'') + N')에 이미 포함된 재고 ' + CAST(@skipped AS NVARCHAR(10)) + N'건은 제외했습니다.', 200);
        END
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
