-- 환율조회 화면(frmExcRate) 전용 조회 프로시저 + 메뉴 등록 (2026-09-15)
-- TBAEXCRATE(사용자가 직접 설계/생성)의 최신 고시일자 스냅샷을 보여준다. base_date를 안 주면
-- 가장 최근 데이터를 자동으로 보여주고, 필요하면 나중에 화면에서 날짜 검색조건을 추가할 수
-- 있도록 프로시저에는 @p_base_date/@p_cur_cd를 미리 열어둔다(기본값 NULL - GENERIC_DATA_API.md
-- "기본값을 주는 게 중요하다"와 같은 이유).

CREATE OR ALTER PROCEDURE USP_BA_EXCRATE_Q
    @p_work_type VARCHAR(50),
    @p_base_date VARCHAR(8) = NULL,
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
            DECLARE @target_date VARCHAR(8) = @p_base_date;
            IF @target_date IS NULL OR @target_date = ''
                SELECT @target_date = MAX(base_date) FROM TBAEXCRATE;

            SELECT base_date, cur_cd, cur_nm, ttb, tts, unit_amt, exc_rate
            FROM TBAEXCRATE
            WHERE base_date = @target_date
              AND (@p_cur_cd IS NULL OR cur_cd = @p_cur_cd)
            ORDER BY cur_cd;
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

IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'BA' AND SCREEN_CLASS_NM = 'frmExcRate')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'환율조회', 1, 3, 'FORM', 'BA', 'frmExcRate', 'USP_BA_EXCRATE_', 20, 'Y', SUSER_SNAME(), GETDATE());
END
GO
