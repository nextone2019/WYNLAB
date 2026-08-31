-- 품목현황(BA_ITEMLIST/frmItemList) 화면용 - 품목등록(USP_BA_ITEM_Q)과 달리 등록/수정이 없는
-- 순수 조회 전용 목록이라 프로시저도 Q 분기 하나만 있다. USP_BA_ITEM_Q와 같은 컬럼 목록을
-- 그대로 반환한다(GENERIC_DATA_API.md 범용 데이터 통로 - 컨트롤러/리포지토리 없음).

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMLIST_Q]
    @p_work_type VARCHAR(50),
    @p_item_cd VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
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
                item_id, item_cd, item_nm, item_spec, unit_cd, wh_cd, loc_cd, safe_qty,
                dept_cd, emp_no, stock_yn, sale_yn, sale_price, po_yn, po_price, stat_cd, remark
            FROM TBAITEM
            WHERE (@p_item_cd IS NULL OR item_cd LIKE '%' + @p_item_cd + '%')
              AND (@p_item_nm IS NULL OR item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY item_cd;
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
