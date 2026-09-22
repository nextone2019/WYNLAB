-- TSMAUTOKEY(자동채번 설정 - 테이블별 접두어/키컬럼/날짜컬럼/일련번호 자릿수)와
-- TSMAUTOKEYHIST(테이블+기준일자별로 현재까지 발급된 일련번호/마지막 채번값) 조회+저장
-- 프로시저. USP_SM_MINORCODE_Q/S(USP_SM_CODE_Q/S)와 같은 패턴 - Q는 마스터(TSMAUTOKEY)
-- 목록, Q_1은 그 아래 이력(TSMAUTOKEYHIST) 조회. 저장은 TSMAUTOKEY만 다룬다(TSMAUTOKEYHIST는
-- 실제 채번 로직이 직접 관리하는 값이라 이 화면에서 사람이 손으로 고치는 대상이 아님).

CREATE PROCEDURE USP_SM_AUTOKEY_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_table_name VARCHAR(100) = NULL,		/* WORK_TYPE = 'Q'일 때 검색어(LIKE), 'Q_1'일 때 정확히 일치하는 부모 키 */
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
                table_name, table_desc, pre_fix, key_col, date_col, date_type, seq_len,
                reg_user_id, reg_dt, upt_user_id, upt_dt
            FROM TSMAUTOKEY
            WHERE (@p_table_name IS NULL OR table_name LIKE '%' + @p_table_name + '%')
            ORDER BY table_name;
        END
        ELSE IF @p_work_type = 'Q_1'
        BEGIN
            SELECT
                table_name, base_date, yyyy, mm, dd, seq, new_key,
                reg_user_id, reg_dt, upt_user_id, upt_dt
            FROM TSMAUTOKEYHIST
            WHERE table_name = @p_table_name
            ORDER BY base_date DESC;
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

CREATE PROCEDURE USP_SM_AUTOKEY_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_table_name VARCHAR(100),
    @p_table_desc NVARCHAR(500) = NULL,
    @p_pre_fix VARCHAR(5) = NULL,
    @p_key_col VARCHAR(30) = NULL,
    @p_date_col VARCHAR(30) = NULL,
    @p_date_type VARCHAR(8) = NULL,
    @p_seq_len INT = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMAUTOKEY (
                table_name, table_desc, pre_fix, key_col, date_col, date_type, seq_len,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_table_name, @p_table_desc, @p_pre_fix, @p_key_col, @p_date_col, @p_date_type, @p_seq_len,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMAUTOKEY SET
                table_desc = @p_table_desc, pre_fix = @p_pre_fix, key_col = @p_key_col,
                date_col = @p_date_col, date_type = @p_date_type, seq_len = @p_seq_len,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE table_name = @p_table_name;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMAUTOKEY WHERE table_name = @p_table_name;
        END

        SET @GeneratedCode = @p_table_name;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
