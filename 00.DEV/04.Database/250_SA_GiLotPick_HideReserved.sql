-- 출하 LOT 선택 팝업: 다른 미확정 출하가 이미 모두 잡은 LOT(가용재고 0 이하)는 목록에서 뺀다 (2026-10-01) - 249의 프로시저에서 조건만 변경

CREATE OR ALTER PROCEDURE USP_SA_GILOTPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_item_id BIGINT = NULL,
    @p_gi_id BIGINT = NULL,                 /* 지금 편집 중인 출하(이 출하가 잡은 수량은 '다른 미확정 출하'에서 제외) */
    @p_ship_kind VARCHAR(1) = NULL,         /* W 자사창고 출하 -> 외주처(OS) 창고 제외 / D 외주처 직송 -> 외주처 창고만. 비면 전체 */
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
                l.lot_id, s.lot_no, s.item_id, i.item_no, i.item_nm, s.unit_cd, s.wh_id, w.wh_nm, s.stock_qty, ISNULL(rs.reserved_qty, 0) AS reserved_qty, s.stock_qty - ISNULL(rs.reserved_qty, 0) AS avail_qty, rs.reserved_docs,
                wm.wo_no, ISNULL(w.wh_type, '') AS wh_type,
                CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN 'D' ELSE 'W' END AS ship_kind,
                CASE WHEN ISNULL(w.wh_type, '') = 'OS' THEN N'외주처' ELSE N'자사창고' END AS ship_kind_nm
            FROM TMASTOCK s
                JOIN TBAITEM i ON i.item_id = s.item_id
                JOIN TBAWH w ON w.wh_id = s.wh_id
                LEFT JOIN TPRLOT l ON l.acc_id = s.acc_id AND l.item_id = s.item_id AND l.lot_no = s.lot_no
                OUTER APPLY (SELECT SUM(x.qty) AS reserved_qty, STRING_AGG(xm.gi_no, ', ') AS reserved_docs
                             FROM TSAGIL x JOIN TSAGIM xm ON xm.gi_id = x.gi_id
                             WHERE xm.stat_cd = '0' AND x.acc_id = s.acc_id AND x.item_id = s.item_id AND x.wh_id = s.wh_id AND ISNULL(x.lot_no, N'') = s.lot_no
                               AND (@p_gi_id IS NULL OR x.gi_id <> @p_gi_id)) rs
                LEFT JOIN TPRWOM wm ON wm.wo_id = l.wo_id
            WHERE s.stock_qty - ISNULL(rs.reserved_qty, 0) > 0 AND s.loc_id = 0 AND ISNULL(w.wh_type, '') <> 'TR'
              AND (@p_acc_id IS NULL OR s.acc_id = @p_acc_id)
              AND (@p_item_id IS NULL OR s.item_id = @p_item_id)
              AND (ISNULL(@p_ship_kind, '') = ''
                   OR (@p_ship_kind = 'D' AND ISNULL(w.wh_type, '') = 'OS')
                   OR (@p_ship_kind = 'W' AND ISNULL(w.wh_type, '') <> 'OS'))
              AND (@p_keyword IS NULL OR @p_keyword = '' OR s.lot_no LIKE '%' + @p_keyword + '%' OR w.wh_nm LIKE '%' + @p_keyword + '%')
            ORDER BY s.lot_no, w.wh_nm;
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
