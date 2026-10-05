-- 출하 LOT 선택 팝업: 출하구분(W 자사창고/D 외주처 직송)에 맞는 창고의 재고만 (2026-10-01) - 243의 프로시저에 조건/파라미터 추가

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
                l.lot_id, s.lot_no, s.item_id, i.item_no, i.item_nm, s.unit_cd, s.wh_id, w.wh_nm, s.stock_qty,
                wm.wo_no, ISNULL(w.wh_type, '') AS wh_type
            FROM TMASTOCK s
                JOIN TBAITEM i ON i.item_id = s.item_id
                JOIN TBAWH w ON w.wh_id = s.wh_id
                LEFT JOIN TPRLOT l ON l.acc_id = s.acc_id AND l.item_id = s.item_id AND l.lot_no = s.lot_no
                LEFT JOIN TPRWOM wm ON wm.wo_id = l.wo_id
            WHERE s.stock_qty > 0 AND s.loc_id = 0 AND ISNULL(w.wh_type, '') <> 'TR'
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
