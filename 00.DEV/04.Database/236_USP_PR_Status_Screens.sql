-- 생산관리(PR) 공정실적현황 / 외주이전현황 조회 화면용 목록 프로시저 + 메뉴 (2026-09-30)
-- 상세(산출 LOT/웨이퍼 판정/이전 LOT)는 기존 USP_PR_RSLT_Q / USP_PR_XFER_Q 'Q'를 그대로 쓴다.

CREATE OR ALTER PROCEDURE USP_PR_RSLTSTAT_Q
    @p_work_type VARCHAR(50) = 'L',
    ---------------------------------------------------------------------------------------------------
    @p_fr_date VARCHAR(8) = NULL,
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_proc_cd VARCHAR(20) = NULL,
    @p_rslt_no VARCHAR(20) = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,          /* 투입 LOT 또는 산출 LOT */
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
        SELECT
            m.rslt_id, m.rslt_no, m.rslt_date, m.wo_id, m.wo_no, m.wo_serl, m.proc_cd, p.proc_nm,
            c.cust_nm, l.lot_no AS in_lot_no, ii.item_nm AS in_item_nm, d.out_unit_cd,
            m.in_qty, m.good_qty, m.bad_qty, m.yield_rate,
            (SELECT STRING_AGG(ol.lot_no, ', ') WITHIN GROUP (ORDER BY ol.lot_no)
             FROM TPRLOTREL r JOIN TPRLOT ol ON ol.lot_id = r.child_lot_id WHERE r.rslt_id = m.rslt_id) AS out_lots,
            m.stat_cd, CONVERT(VARCHAR(16), m.cfm_dt, 120) AS cfm_dt, m.src_file_nm
        FROM TPRRSLTM m
            LEFT JOIN TPRWOD d ON d.wo_id = m.wo_id AND d.serl = m.wo_serl
            LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
            LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
            LEFT JOIN TPRLOT l ON l.lot_id = m.in_lot_id
            LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
        WHERE (@p_fr_date IS NULL OR @p_fr_date = '' OR m.rslt_date >= @p_fr_date)
          AND (@p_to_date IS NULL OR @p_to_date = '' OR m.rslt_date <= @p_to_date)
          AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
          AND (@p_proc_cd IS NULL OR @p_proc_cd = '' OR m.proc_cd = @p_proc_cd)
          AND (@p_rslt_no IS NULL OR @p_rslt_no = '' OR m.rslt_no LIKE '%' + @p_rslt_no + '%')
          AND (@p_wo_no IS NULL OR @p_wo_no = '' OR m.wo_no LIKE '%' + @p_wo_no + '%')
          AND (@p_lot_no IS NULL OR @p_lot_no = '' OR l.lot_no LIKE '%' + @p_lot_no + '%'
               OR EXISTS (SELECT 1 FROM TPRLOTREL r JOIN TPRLOT ol ON ol.lot_id = r.child_lot_id
                          WHERE r.rslt_id = m.rslt_id AND ol.lot_no LIKE '%' + @p_lot_no + '%'))
        ORDER BY m.rslt_date DESC, m.rslt_id DESC;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_PR_XFERSTAT_Q
    @p_work_type VARCHAR(50) = 'L',
    ---------------------------------------------------------------------------------------------------
    @p_fr_date VARCHAR(8) = NULL,
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_xfer_no VARCHAR(20) = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
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
        SELECT
            m.xfer_id, m.xfer_no, m.xfer_date, m.wo_id, m.wo_no,
            fp.proc_nm AS from_proc_nm, fc.cust_nm AS from_cust_nm, tp.proc_nm AS to_proc_nm, tc.cust_nm AS to_cust_nm,
            t.lot_cnt, t.out_qty, t.in_qty, t.diff_qty,
            m.stat_cd, CONVERT(VARCHAR(16), m.out_dt, 120) AS out_dt, CONVERT(VARCHAR(16), m.in_dt, 120) AS in_dt
        FROM TPRXFERM m
            LEFT JOIN TPRWOD fd ON fd.wo_id = m.wo_id AND fd.serl = m.from_serl
            LEFT JOIN TPRWOD td ON td.wo_id = m.wo_id AND td.serl = m.to_serl
            LEFT JOIN TBAPROC fp ON fp.acc_id = m.acc_id AND fp.proc_cd = fd.proc_cd
            LEFT JOIN TBAPROC tp ON tp.acc_id = m.acc_id AND tp.proc_cd = td.proc_cd
            LEFT JOIN TBACUST fc ON fc.cust_id = m.from_cust_id
            LEFT JOIN TBACUST tc ON tc.cust_id = m.to_cust_id
            OUTER APPLY (SELECT COUNT(*) AS lot_cnt, SUM(x.out_qty) AS out_qty, SUM(x.in_qty) AS in_qty, SUM(x.diff_qty) AS diff_qty
                         FROM TPRXFERD x WHERE x.xfer_id = m.xfer_id) t
        WHERE (@p_fr_date IS NULL OR @p_fr_date = '' OR m.xfer_date >= @p_fr_date)
          AND (@p_to_date IS NULL OR @p_to_date = '' OR m.xfer_date <= @p_to_date)
          AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
          AND (@p_xfer_no IS NULL OR @p_xfer_no = '' OR m.xfer_no LIKE '%' + @p_xfer_no + '%')
          AND (@p_wo_no IS NULL OR @p_wo_no = '' OR m.wo_no LIKE '%' + @p_wo_no + '%')
          AND (@p_lot_no IS NULL OR @p_lot_no = '' OR EXISTS (SELECT 1 FROM TPRXFERD x WHERE x.xfer_id = m.xfer_id AND x.lot_no LIKE '%' + @p_lot_no + '%'))
        ORDER BY m.xfer_date DESC, m.xfer_id DESC;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 메뉴: 생산실행 그룹 아래 공정실적현황(50), 외주이전현황(60)
DECLARE @grp BIGINT = (SELECT TOP 1 MENU_ID FROM TSMMENU WHERE UPPER_MENU_ID = 14 AND MENU_LEVEL = 2 AND MENU_TYPE = 'GROUP' AND MENU_NM = N'생산실행');
DECLARE @screens TABLE (nm NVARCHAR(50), cls VARCHAR(50), sort INT);
INSERT INTO @screens VALUES (N'공정실적현황', 'frmRsltStatus', 50), (N'외주이전현황', 'frmXferStatus', 60);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT s.nm, @grp, 3, 'FORM', 'PR', s.cls, 'USP_PR_', s.sort, 'Y', SUSER_SNAME(), GETDATE()
FROM @screens s
WHERE @grp IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.MODULE = 'PR' AND m.SCREEN_CLASS_NM = s.cls);
GO
