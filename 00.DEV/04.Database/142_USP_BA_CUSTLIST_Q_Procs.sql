CREATE OR ALTER PROCEDURE USP_BA_CUSTLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
                CUST_ID, cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2, homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate, remark, stat_cd, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc, EMP_ID
            FROM tbacust
            ORDER BY CUST_ID;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
