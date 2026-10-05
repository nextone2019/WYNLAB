-- 공통 출력물(WYNLAB.Report) 데이터 규약 (2026-10-05)
--  FN_RPT_ACC(acc_id)   : 공급자(우리 사업장) 한 행 - nm, biz_no, owner_nm, addr, biz_kind, biz_type, tel, fax, stamp, logo
--  FN_RPT_CUST(cust_id) : 거래처(고객/공급사) 한 행 - nm, biz_no, owner_nm, addr, biz_kind, biz_type, tel, fax
--  어떤 문서 출력 프로시저든 이 두 함수를 CROSS APPLY 하고 접두사(sup_/rcv_ ...)만 붙여 돌려주면
--  클라이언트 TradeParty.FromRow(row, "sup", "공급자")가 표준 5줄(등록번호/상호·성명/주소/업태·종목/전화·팩스)과 직인/로고를 만든다.
--  USP_SA_INVC_P : 거래명세서 출력 데이터(Q 결과 1: 헤더+공급자/공급받는자, 결과 2: 품목)

CREATE OR ALTER FUNCTION FN_RPT_ACC (@acc_id BIGINT)
RETURNS TABLE
AS RETURN
(
    SELECT a.ACC_NM AS nm, a.BIZ_NO AS biz_no, a.OWNER_NM AS owner_nm,
           LTRIM(RTRIM(ISNULL(a.ADDR1, N'') + N' ' + ISNULL(a.ADDR2, N''))) AS addr,
           a.BIZ_KIND AS biz_kind, a.BIZ_TYPE AS biz_type, a.TEL AS tel, a.FAX AS fax,
           a.STAMP AS stamp, a.LOGO AS logo
    FROM TBAACC a WHERE a.ACC_ID = @acc_id
);
GO

CREATE OR ALTER FUNCTION FN_RPT_CUST (@cust_id BIGINT)
RETURNS TABLE
AS RETURN
(
    SELECT c.cust_nm AS nm, c.biz_no AS biz_no, c.owner_nm AS owner_nm,
           LTRIM(RTRIM(ISNULL(c.addr1, N'') + N' ' + ISNULL(c.addr2, N''))) AS addr,
           c.biz_kind AS biz_kind, c.biz_type AS biz_type, c.tel AS tel, c.fax AS fax
    FROM TBACUST c WHERE c.cust_id = @cust_id
);
GO

CREATE OR ALTER PROCEDURE USP_SA_INVC_P
    @p_work_type VARCHAR(50),               /* P 출력 */
    ---------------------------------------------------------------------------------------------------
    @p_invc_id BIGINT = NULL,
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
        IF @p_work_type = 'P'
        BEGIN
            SELECT m.invc_id, m.invc_no, m.invc_date, m.stat_cd, m.remark, e.emp_nm,
                   s.nm AS sup_nm, s.biz_no AS sup_biz_no, s.owner_nm AS sup_owner_nm, s.addr AS sup_addr, s.biz_kind AS sup_biz_kind,
                   s.biz_type AS sup_biz_type, s.tel AS sup_tel, s.fax AS sup_fax, s.stamp AS sup_stamp, s.logo AS sup_logo,
                   r.nm AS rcv_nm, r.biz_no AS rcv_biz_no, r.owner_nm AS rcv_owner_nm, r.addr AS rcv_addr, r.biz_kind AS rcv_biz_kind,
                   r.biz_type AS rcv_biz_type, r.tel AS rcv_tel, r.fax AS rcv_fax
            FROM TSAINVCM m
                OUTER APPLY FN_RPT_ACC(m.acc_id) s
                OUTER APPLY FN_RPT_CUST(m.cust_id) r
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.invc_id = @p_invc_id;

            SELECT d.serl, d.so_no, i.item_no, i.item_nm, i.item_spec, d.unit_cd, d.qty, d.price, d.amt, d.vat, d.total_amt, d.cur_cd, d.remark
            FROM TSAINVCD d LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE d.invc_id = @p_invc_id
            ORDER BY d.serl;
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
