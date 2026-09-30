-- 공정실적 웨이퍼 번호를 '작업지시 시작 LOT-NN'로 정의하기 위해 시작 LOT 번호를 실적 대기 LOT 팝업/실적 조회 헤더에 추가 (2026-09-30)

CREATE OR ALTER PROCEDURE USP_PR_RSLTREADYPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 작업지시번호 */
    @p_cust_id BIGINT = NULL,               /* 외주처 */
    @p_keyword NVARCHAR(100) = NULL,        /* LOT/품번/품명 */
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
                m.wo_id, m.wo_no, m.wo_date, m.start_lot_no, d.serl AS wo_serl, d.proc_cd, p.proc_nm,
                d.cust_id, c.cust_nm, d.wh_id, w.wh_nm,
                l.lot_id AS in_lot_id, l.lot_no AS in_lot_no,
                d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd, s.stock_qty AS in_qty,
                d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd, d.split_qty
            FROM TPRWOM m
                JOIN TPRWOD d ON d.wo_id = m.wo_id
                JOIN TPRLOT l ON l.wo_id = d.wo_id AND l.item_id = d.in_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = d.wh_id AND s.loc_id = 0 AND s.stock_qty > 0
                LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = d.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE m.stat_cd IN ('C', '1')
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.wo_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.wo_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.wo_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR d.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR l.lot_no LIKE '%' + @p_keyword + '%'
                   OR ii.item_no LIKE '%' + @p_keyword + '%'
                   OR ii.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.wo_no, d.serl, l.lot_no;
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

CREATE OR ALTER PROCEDURE USP_PR_RSLT_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
    @p_rslt_no VARCHAR(20) = NULL,
    @p_wo_id BIGINT = NULL,                 /* LOT 조회용 */
    @p_wo_serl INT = NULL,
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
            SELECT TOP 1 @match_id = rslt_id FROM TPRRSLTM
            WHERE (@p_rslt_id IS NULL OR rslt_id = @p_rslt_id)
              AND (@p_rslt_id IS NOT NULL OR @p_rslt_no IS NULL OR rslt_no LIKE '%' + @p_rslt_no + '%')
            ORDER BY rslt_id DESC;

            SELECT
                m.rslt_id, m.acc_id, m.rslt_no, m.rslt_date,
                m.wo_id, m.wo_no, wm.start_lot_no, m.wo_serl, m.proc_cd, p.proc_nm,
                m.cust_id, c.cust_nm, m.wh_id, w.wh_nm,
                m.in_lot_id, l.lot_no AS in_lot_no, d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd,
                d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd, d.split_qty,
                m.in_qty, m.good_qty, m.bad_qty, m.yield_rate, m.src_file_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TPRRSLTM m
                LEFT JOIN TPRWOM wm ON wm.wo_id = m.wo_id
                LEFT JOIN TPRWOD d ON d.wo_id = m.wo_id AND d.serl = m.wo_serl
                LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = m.wh_id
                LEFT JOIN TPRLOT l ON l.lot_id = m.in_lot_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE m.rslt_id = @match_id;

            SELECT rslt_id, serl, acc_id, wafer_no, good_qty, bad_qty, good_qty + bad_qty AS gross_qty, remark
            FROM TPRRSLTD WHERE rslt_id = @match_id ORDER BY serl;

            SELECT r.parent_lot_id, r.child_lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, r.qty
            FROM TPRLOTREL r
                JOIN TPRLOT l ON l.lot_id = r.child_lot_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE r.rslt_id = @match_id ORDER BY l.lot_no;
        END
        ELSE IF @p_work_type = 'LOT'
        BEGIN
            -- 그 공정의 외주처 창고에 재고가 있는 투입품목 LOT(이 작업지시 소속)
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.stock_qty
            FROM TPRWOD d
                JOIN TPRLOT l ON l.wo_id = d.wo_id AND l.item_id = d.in_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = d.wh_id AND s.loc_id = 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE d.wo_id = @p_wo_id AND d.serl = @p_wo_serl AND s.stock_qty > 0
            ORDER BY l.lot_no;
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
