-- 그리드에 등록일시(reg_dt) 컬럼이 추가되면서 SELECT 목록에도 포함시킨다(2026-09-15).

CREATE OR ALTER PROCEDURE USP_BA_EXCRATE_Q
    @p_work_type VARCHAR(50),
    @p_base_date_from VARCHAR(8),
    @p_base_date_to VARCHAR(8),
    @p_cur_cd VARCHAR(10) = NULL,
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
            SELECT base_date, cur_cd, cur_nm, ttb, tts, unit_amt, exc_rate, reg_dt
            FROM TBAEXCRATE
            WHERE base_date BETWEEN @p_base_date_from AND @p_base_date_to
              AND (@p_cur_cd IS NULL OR cur_cd = @p_cur_cd)
            ORDER BY base_date DESC, cur_cd;
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
