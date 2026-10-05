-- 기타출고현황(frmEtcOutList) 조회 프로시저 + 메뉴 (2026-10-03, WYNLAB_DEV 전용)
-- Q: 기타출고 헤더 목록(출고일자/번호/출고유형/진행상태/품번·품명 조건), Q1: 한 출고의 품목. 사업장(@p_acc_id)이 첫 파라미터(화면 표준).
-- 메뉴는 재고관리 그룹(79)의 기타출고등록(50) 다음(60). 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_MA_ETCOUTLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준) */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,                /* Q1 - 한 출고의 품목 */
    @p_out_no VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번 / 품명 */
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_trans_type VARCHAR(10) = NULL,       /* 출고유형 */
    @p_stat_cd VARCHAR(10) = NULL,          /* MA0012 진행상태 */
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
            SELECT m.out_id, m.out_no, m.out_date, m.trans_type, m.stat_cd, d.dept_nm, e.emp_nm, m.cfm_dt,
                   (SELECT COUNT(*) FROM TMAETCOUTD x WHERE x.out_id = m.out_id) AS line_cnt,
                   (SELECT ISNULL(SUM(x.out_qty), 0) FROM TMAETCOUTD x WHERE x.out_id = m.out_id) AS total_qty,
                   m.remark
            FROM TMAETCOUTM m
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_out_no, '') = '' OR m.out_no LIKE '%' + @p_out_no + '%')
              AND (ISNULL(@p_date_from, '') = '' OR m.out_date >= @p_date_from)
              AND (ISNULL(@p_date_to, '') = '' OR m.out_date <= @p_date_to)
              AND (ISNULL(@p_trans_type, '') = '' OR m.trans_type = @p_trans_type)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_keyword, '') = ''
                   OR EXISTS (SELECT 1 FROM TMAETCOUTD x JOIN TBAITEM i ON i.item_id = x.item_id
                              WHERE x.out_id = m.out_id
                                AND (i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')))
            ORDER BY m.out_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT dt.out_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, dt.out_qty, dt.lot_no,
                   w.wh_nm, l.loc_nm, dt.stock_yn, dt.src_no, dt.remark
            FROM TMAETCOUTD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.out_id = @p_out_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

DECLARE @grp BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE MENU_NM = N'재고관리' AND MENU_TYPE = 'GROUP' AND UPPER_MENU_ID = 10);
INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT N'기타출고현황', @grp, 3, 'FORM', 'MA', 'frmEtcOutList', 'USP_MA_', 60, 'Y', 'wynlab', GETDATE()
WHERE @grp IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSMMENU WHERE SCREEN_CLASS_NM = 'frmEtcOutList');
GO
