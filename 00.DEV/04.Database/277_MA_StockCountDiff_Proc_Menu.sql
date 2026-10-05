-- 실사차이분석(frmStockCountDiff) 조회 프로시저 + 메뉴 (2026-10-04, WYNLAB_DEV 전용). 설계: Document\재고실사_설계서.md (P3)
-- USP_MA_CNTDIFF_Q 'Q': 실사 라인 단위 차이 현황 - 입력완료(3)/확정(C)된 실사의 라인(차이가 고정된 것)만 대상. 실사/창고/품목/사유별로 모아 보는 용도(화면의 그룹 패널).
--   조건: 기준일자 범위, 실사번호, 창고, 품번/품명, 조정사유, 진행상태(3 또는 C로 좁히기), 차이 있는 라인만(@p_diff_yn='Y').
--   블라인드 실사는 입력완료 전엔 차이가 없으므로(대상 아님) 마스킹이 필요 없다.
-- 메뉴는 재고관리 그룹(79)의 재고실사현황(80) 다음(90). 권한은 구매입고등록(frmGr)과 같은 대상에게 똑같이 준다. 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_MA_CNTDIFF_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준) */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_date_from VARCHAR(8) = NULL,         /* 실사 기준일자 범위 */
    @p_date_to VARCHAR(8) = NULL,
    @p_cnt_no VARCHAR(20) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번 / 품명 */
    @p_adj_reason VARCHAR(10) = NULL,       /* MA0016 조정사유 */
    @p_stat_cd VARCHAR(10) = NULL,          /* 3 입력완료 / C 확정 (비우면 둘 다) */
    @p_diff_yn VARCHAR(1) = NULL,           /* Y = 차이가 있는 라인만 */
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
            SELECT m.cnt_id, m.cnt_no, m.cnt_title, m.cnt_date, m.stat_cd, m.cfm_dt,
                   d.serl, w.wh_nm, i.item_no, i.item_nm, i.item_spec, d.unit_cd, d.lot_no,
                   d.book_qty, d.move_qty, d.fin_qty, d.diff_qty,
                   d.adj_reason, r.minor_nm AS adj_reason_nm, d.add_yn, d.trans_id, d.remark
            FROM TMACNTD d
                JOIN TMACNTM m ON m.cnt_id = d.cnt_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TSMMINOR r ON r.major_cd = 'MA0016' AND r.minor_cd = d.adj_reason
            WHERE m.stat_cd IN ('3', 'C')
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_date_from, '') = '' OR m.cnt_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR m.cnt_date <= @p_date_to)
              AND (ISNULL(@p_cnt_no, '') = '' OR m.cnt_no LIKE '%' + @p_cnt_no + '%')
              AND (@p_wh_id IS NULL OR d.wh_id = @p_wh_id)
              AND (ISNULL(@p_adj_reason, '') = '' OR d.adj_reason = @p_adj_reason)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_diff_yn, 'N') <> 'Y' OR ISNULL(d.diff_qty, 0) <> 0)
              AND (ISNULL(@p_keyword, '') = '' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.cnt_date DESC, m.cnt_id DESC, d.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

DECLARE @grp BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE MENU_NM = N'재고관리' AND MENU_TYPE = 'GROUP' AND UPPER_MENU_ID = 10);
DECLARE @tpl BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE SCREEN_CLASS_NM = 'frmGr');

DECLARE @items TABLE (cls VARCHAR(100), nm NVARCHAR(100), so INT);
INSERT INTO @items VALUES ('frmStockCountDiff', N'실사차이분석', 90);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT i.nm, @grp, 3, 'FORM', 'MA', i.cls, 'USP_MA_', i.so, 'Y', 'wynlab', GETDATE()
FROM @items i
WHERE @grp IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.SCREEN_CLASS_NM = i.cls);

INSERT INTO TSMMENUAUTH (MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, DELETE_YN, SAVE_YN, PRINT_YN, EXCEL_YN,
                         AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_USER_ID, REG_DT)
SELECT m.MENU_ID, a.AUTH_TARGET_TYPE, a.AUTH_TARGET_CD, a.VIEW_YN, a.INSERT_YN, a.DELETE_YN, a.SAVE_YN, a.PRINT_YN, a.EXCEL_YN,
       a.AUTH01, a.AUTH02, a.AUTH03, a.AUTH04, a.AUTH05, a.AUTH06, a.AUTH07, a.AUTH08, a.AUTH09, a.AUTH10, 'wynlab', GETDATE()
FROM TSMMENU m
JOIN @items i ON i.cls = m.SCREEN_CLASS_NM
JOIN TSMMENUAUTH a ON a.MENU_ID = @tpl
WHERE NOT EXISTS (SELECT 1 FROM TSMMENUAUTH x WHERE x.MENU_ID = m.MENU_ID AND x.AUTH_TARGET_TYPE = a.AUTH_TARGET_TYPE AND x.AUTH_TARGET_CD = a.AUTH_TARGET_CD);
GO
