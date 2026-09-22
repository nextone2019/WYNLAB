-- 158: 일정관리(TSMSCHEDULE) + 쪽지함(TSMMESSAGE) 신설 + 홈화면 위젯 연동
-- 적용 전 검토 필요(다른 마이그레이션과 동일한 규칙) - 자동 실행되지 않는다.

-- ============================================================
-- 1) TSMSCHEDULE - 개인 일정(공유 캘린더 아님, 로그인 사용자 본인 것만 보임/관리됨)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMSCHEDULE')
BEGIN
    CREATE TABLE TSMSCHEDULE (
        SCHEDULE_ID     BIGINT IDENTITY(1,1) NOT NULL,
        ACC_ID          BIGINT NOT NULL,
        EMP_ID          BIGINT NULL,             -- 소유자(TBAEMP) - 저장프로시저가 p_user_id로 역매핑해서 채움
        TITLE           NVARCHAR(200) NOT NULL,
        CONTENT         NVARCHAR(MAX) NULL,
        START_DT        VARCHAR(8) NOT NULL,      -- yyyyMMdd(project_wynlab_yyyymmdd_date_convention)
        END_DT          VARCHAR(8) NULL,          -- NULL이면 하루짜리 일정(START_DT와 같은 날)
        REMARK          NVARCHAR(1000) NULL,
        REG_USER_ID     VARCHAR(30) NULL,
        REG_DT          DATETIME NULL,
        REG_PC          NVARCHAR(400) NULL,
        UPT_USER_ID     VARCHAR(30) NULL,
        UPT_DT          DATETIME NULL,
        UPT_PC          NVARCHAR(400) NULL,
        CONSTRAINT PK_TSMSCHEDULE PRIMARY KEY (SCHEDULE_ID)
    );
END
GO

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
            SELECT s.schedule_id, s.acc_id, s.title, s.content, s.start_dt, s.end_dt, s.remark, s.reg_dt
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

CREATE OR ALTER PROCEDURE USP_SM_SCHEDULE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_schedule_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_title NVARCHAR(200) = NULL,
    @p_content NVARCHAR(MAX) = NULL,
    @p_start_dt VARCHAR(8) = NULL,
    @p_end_dt VARCHAR(8) = NULL,
    @p_remark NVARCHAR(1000) = NULL,
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
            INSERT INTO TSMSCHEDULE (acc_id, emp_id, title, content, start_dt, end_dt, remark, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_acc_id, @my_emp_id, @p_title, @p_content, @p_start_dt, @p_end_dt, @p_remark, @p_user_id, GETDATE(), @p_client_pc);
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMSCHEDULE SET
                acc_id = @p_acc_id, title = @p_title, content = @p_content,
                start_dt = @p_start_dt, end_dt = @p_end_dt, remark = @p_remark,
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

IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SM' AND SCREEN_CLASS_NM = 'frmSchedule')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'일정관리', 25, 2, 'FORM', 'SM', 'frmSchedule', 'USP_SM_SCHEDULE_', 50, 'Y', SUSER_SNAME(), GETDATE());
END
GO

-- ============================================================
-- 2) TSMMESSAGE - 사내 쪽지함(받은/보낸 한 화면, box_type으로 구분 표시)
--    받는사람은 사번(TO_EMP_NO, 유일값이라 이름 대신 사용 - 동명이인 문제 회피)으로 직접 입력.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMMESSAGE')
BEGIN
    CREATE TABLE TSMMESSAGE (
        MSG_ID          BIGINT IDENTITY(1,1) NOT NULL,
        FROM_EMP_ID     BIGINT NOT NULL,          -- 발신자(TBAEMP) - 저장프로시저가 p_user_id로 역매핑해서 채움
        TO_EMP_NO       VARCHAR(20) NOT NULL,      -- 수신자 사번(TBAEMP.emp_no) - 저장 시 존재 여부 검증
        TITLE           NVARCHAR(200) NOT NULL,
        CONTENT         NVARCHAR(MAX) NULL,
        READ_YN         VARCHAR(1) NOT NULL CONSTRAINT DF_TSMMESSAGE_READ_YN DEFAULT('N'),
        REG_USER_ID     VARCHAR(30) NULL,
        REG_DT          DATETIME NULL,
        REG_PC          NVARCHAR(400) NULL,
        UPT_USER_ID     VARCHAR(30) NULL,
        UPT_DT          DATETIME NULL,
        UPT_PC          NVARCHAR(400) NULL,
        CONSTRAINT PK_TSMMESSAGE PRIMARY KEY (MSG_ID)
    );
END
GO

CREATE OR ALTER PROCEDURE USP_SM_MESSAGE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no VARCHAR(20) = NULL,     -- 로그인 사용자 사번(Session.EmpNo) - 이 사람이 보낸 것+받은 것만
    @p_title VARCHAR(200) = NULL,
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
            SELECT m.msg_id, m.title, m.content, m.read_yn, m.reg_dt,
                   fe.emp_no AS from_emp_no, fe.emp_nm AS from_emp_nm,
                   m.to_emp_no, te.emp_nm AS to_emp_nm,
                   CASE WHEN fe.emp_no = @p_emp_no THEN N'보냄' ELSE N'받음' END AS box_type
            FROM TSMMESSAGE m
            JOIN TBAEMP fe ON fe.EMP_ID = m.from_emp_id
            LEFT JOIN TBAEMP te ON te.emp_no = m.to_emp_no
            WHERE (fe.emp_no = @p_emp_no OR m.to_emp_no = @p_emp_no)
              AND (@p_title IS NULL OR @p_title = '' OR m.title LIKE '%' + @p_title + '%')
            ORDER BY m.reg_dt DESC;
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

CREATE OR ALTER PROCEDURE USP_SM_MESSAGE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_msg_id BIGINT = NULL,
    @p_to_emp_no VARCHAR(20) = NULL,
    @p_title NVARCHAR(200) = NULL,
    @p_content NVARCHAR(MAX) = NULL,
    @p_read_yn VARCHAR(1) = NULL,
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
            IF @p_to_emp_no IS NULL OR NOT EXISTS (SELECT 1 FROM TBAEMP WHERE emp_no = @p_to_emp_no)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'받는사람 사번을 찾을 수 없습니다: ' + ISNULL(@p_to_emp_no, '');
                RETURN;
            END

            INSERT INTO TSMMESSAGE (from_emp_id, to_emp_no, title, content, read_yn, reg_user_id, reg_dt, reg_pc)
            VALUES (@my_emp_id, @p_to_emp_no, @p_title, @p_content, 'N', @p_user_id, GETDATE(), @p_client_pc);
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 받은 쪽지의 "읽음" 체크(주 용도) - 수신자가 아니어도 화면상 막지는 않는다(발신자도
            -- 자기 쪽지 제목/내용을 고칠 수 있음, 간단한 사내 쪽지라 엄격한 잠금은 안 둠).
            UPDATE TSMMESSAGE SET
                to_emp_no = @p_to_emp_no, title = @p_title, content = @p_content,
                read_yn = ISNULL(@p_read_yn, read_yn),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE msg_id = @p_msg_id;
            SET @GeneratedCode = CAST(@p_msg_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMMESSAGE WHERE msg_id = @p_msg_id;
            SET @GeneratedCode = CAST(@p_msg_id AS VARCHAR(20));
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

IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SM' AND SCREEN_CLASS_NM = 'frmMessage')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'쪽지함', 25, 2, 'FORM', 'SM', 'frmMessage', 'USP_SM_MESSAGE_', 60, 'Y', SUSER_SNAME(), GETDATE());
END
GO
