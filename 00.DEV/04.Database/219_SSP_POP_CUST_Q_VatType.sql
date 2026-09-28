-- 구매발주등록(frmPo) 거래처 팝업(P_CUST)에서 부가세유형/부가세율을 안 가져오는 문제 (2026-09-28).
-- SSP_POP_CUST_Q가 애초에 TBACUST.vat_type/vat_rate를 SELECT하지 않아서, 화면이 MapField로
-- 아무리 받아도 팝업 결과 행 자체에 그 컬럼이 없었다. 화면쪽(frmPo.cs MapField 추가)과 세트.

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_CUST_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CUST_ID, cust_nm, biz_no, owner_nm, tel, vat_type, vat_rate
    FROM TBACUST
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR cust_nm LIKE '%' + @p_keyword + '%')
    ORDER BY CUST_ID;
END
GO
