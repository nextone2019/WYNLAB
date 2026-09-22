-- 그리드 컬럼의 DisplayFormat은 실제 DateTime 타입 값에만 적용된다. 하지만 api/data/query는
-- 결과를 JSON으로 내려주고, 클라이언트(ProcData.ToDataTable)는 모든 컬럼을 object로 받아서
-- JSON의 datetime 값은 그냥 문자열(밀리초 포함 "2026-09-15T13:01:01.54")로 남는다 - 그래서
-- 그리드에 지정한 포맷이 먹지 않고 원본 문자열이 그대로 보였다(2026-09-15). base_date(VARCHAR
-- 원본 + DateColumnEdit)처럼 별도 편집기를 만드는 대신, 이 컬럼은 조회 전용이라 더 간단하게
-- 프로시저에서 이미 "yyyy-MM-dd HH:mm:ss" 형태의 문자열로 변환해서 내려준다.

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
            SELECT base_date, cur_cd, cur_nm, ttb, tts, unit_amt, exc_rate,
                   CONVERT(varchar(19), reg_dt, 120) AS reg_dt
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
