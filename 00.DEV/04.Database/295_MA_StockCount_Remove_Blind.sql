-- 재고실사 블라인드 기능 제거 (2026-10-05, WYNLAB_DEV 전용)
--  - 블라인드(입력완료 전 장부수량/변동/차이 숨김) 옵션을 없앤다: TMACNTM.blind_yn 컬럼 삭제, USP_MA_CNT_S의 @p_blind_yn 파라미터 삭제,
--    USP_MA_CNT_Q / USP_MA_CNTLIST_Q의 마스킹(@mask)과 blind_yn 출력 삭제. 장부수량/변동/차이는 항상 그대로 내려간다.
--  - 프로시저는 라이브 정의에서 해당 부분만 빼서 CREATE OR ALTER 한다. 컬럼은 프로시저를 바꾼 뒤에 지운다(기본값 제약 DF_TMACNTM_blind, 체크 제약 CK_TMACNTM_blind 포함).
--  *** 새 프로시저(@p_blind_yn 없음)는 예전 화면(chkBlind가 p_blind_yn을 보냄)과 맞지 않는다 - 화면(MA 모듈)을 같이 배포하고 클라이언트를 다시 시작할 것. ***
--  여러 번 실행해도 안전하다.
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

            DECLARE @stat VARCHAR(10), @snap BIGINT, @cdate VARCHAR(8);
            SELECT @stat = stat_cd, @snap = snap_trans_id, @cdate = cnt_date FROM TMACNTM WHERE cnt_id = @match_id;

            SELECT m.cnt_id, m.acc_id, a.ACC_NM, m.cnt_no, m.cnt_title, m.cnt_type, m.cnt_date, m.freeze_mode,
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
                   d.book_qty,
                   d.cnt_qty, d.recnt_yn, d.recnt_qty, d.fin_qty,
                   mv.move_qty,
                   CASE WHEN @stat IN ('3', 'C') AND d.diff_qty IS NOT NULL THEN d.diff_qty
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
            SELECT m.cnt_id, m.cnt_no, m.cnt_title, m.cnt_type, m.cnt_date, m.stat_cd,
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
            SELECT dt.cnt_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, w.wh_nm, dt.lot_no,
                   dt.book_qty,
                   dt.fin_qty,
                   dt.move_qty,
                   dt.diff_qty,
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

            INSERT INTO TMACNTM (acc_id, cnt_no, wh_id, cnt_title, cnt_type, cnt_date, tol_qty, tol_rate, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                 reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_wh_id, @p_cnt_title, @p_cnt_type, @p_cnt_date, @p_tol_qty, @p_tol_rate, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_cnt_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMACNTM SET
                acc_id = @p_acc_id, wh_id = @p_wh_id, cnt_title = @p_cnt_title, cnt_type = @p_cnt_type, cnt_date = @p_cnt_date,
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

IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_TMACNTM_blind' AND parent_object_id = OBJECT_ID('TMACNTM'))
    ALTER TABLE TMACNTM DROP CONSTRAINT CK_TMACNTM_blind;
GO
IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_TMACNTM_blind' AND parent_object_id = OBJECT_ID('TMACNTM'))
    ALTER TABLE TMACNTM DROP CONSTRAINT DF_TMACNTM_blind;
GO
IF COL_LENGTH('TMACNTM', 'blind_yn') IS NOT NULL
    ALTER TABLE TMACNTM DROP COLUMN blind_yn;
GO