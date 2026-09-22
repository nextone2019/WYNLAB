-- frmExcRate에 기간별 조회조건(ymdFrDate~ymdToDate)이 추가되면서, 최신 1일 스냅샷만 보여주던
-- USP_BA_EXCRATE_Q(146번)를 기간 범위 조회로 바꾼다(2026-09-15). 두 날짜 모두 화면에서 필수
-- 입력이라 프로시저에서도 필수로 받는다(기본값 없음 - NULL이 오면 그건 화면 검증을 우회한
-- 비정상 호출이므로 굳이 완화하지 않는다).

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
            SELECT base_date, cur_cd, cur_nm, ttb, tts, unit_amt, exc_rate
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
