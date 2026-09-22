-- 160: 일정관리(TSMSCHEDULE)에서 비고(REMARK) 컬럼 제거 - 화면에서도 뺌
-- 적용 전 검토 필요 - 자동 실행되지 않는다.

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TSMSCHEDULE') AND name = 'REMARK')
    ALTER TABLE TSMSCHEDULE DROP COLUMN REMARK;
GO

-- ============================================================
-- USP_SM_SCHEDULE_Q - 159의 살아있는 정의에서 remark만 뺌
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_SCHEDULE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no VARCHAR(20) = NULL,     -- 로그인 사용자 사번(Session.EmpNo) - 본인 일정만 보임
    @p_title VARCHAR(200) = NULL,
    @p_date_from VARCHAR(8) = NULL,   -- 이 날짜가 START_DT~END_DT 구간에 걸치는 일정만(둘 다 NULL이면 전체)
    @p_date_to VARCHAR(8) = NULL,
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
            SELECT s.schedule_id, s.acc_id, s.title, s.content, s.start_dt, s.end_dt,
                   s.all_day_yn, s.start_tm, s.end_tm, s.color_cd, s.reg_dt
            FROM TSMSCHEDULE s
            JOIN TBAEMP e ON e.EMP_ID = s.emp_id
            WHERE e.emp_no = @p_emp_no
              AND (@p_title IS NULL OR @p_title = '' OR s.title LIKE '%' + @p_title + '%')
              AND (
                    (@p_date_from IS NULL AND @p_date_to IS NULL)
                    OR (s.start_dt <= ISNULL(@p_date_to, @p_date_from) AND ISNULL(s.end_dt, s.start_dt) >= @p_date_from)
                  )
            ORDER BY s.start_dt DESC;
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

-- ============================================================
-- USP_SM_SCHEDULE_S - 159의 살아있는 정의에서 remark만 뺌
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_SCHEDULE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_schedule_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_title NVARCHAR(200) = NULL,
    @p_content NVARCHAR(MAX) = NULL,
    @p_start_dt VARCHAR(8) = NULL,
    @p_end_dt VARCHAR(8) = NULL,
    @p_all_day_yn VARCHAR(1) = NULL,
    @p_start_tm VARCHAR(4) = NULL,
    @p_end_tm VARCHAR(4) = NULL,
    @p_color_cd VARCHAR(10) = NULL,
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
        DECLARE @my_emp_id BIGINT;
        SELECT @my_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMSCHEDULE (
                acc_id, emp_id, title, content, start_dt, end_dt,
                all_day_yn, start_tm, end_tm, color_cd,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @my_emp_id, @p_title, @p_content, @p_start_dt, @p_end_dt,
                ISNULL(@p_all_day_yn, 'Y'), @p_start_tm, @p_end_tm, @p_color_cd,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMSCHEDULE SET
                acc_id = @p_acc_id, title = @p_title, content = @p_content,
                start_dt = @p_start_dt, end_dt = @p_end_dt,
                all_day_yn = ISNULL(@p_all_day_yn, 'Y'), start_tm = @p_start_tm, end_tm = @p_end_tm,
                color_cd = @p_color_cd,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE schedule_id = @p_schedule_id;
            SET @GeneratedCode = CAST(@p_schedule_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMSCHEDULE WHERE schedule_id = @p_schedule_id;
            SET @GeneratedCode = CAST(@p_schedule_id AS VARCHAR(20));
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
