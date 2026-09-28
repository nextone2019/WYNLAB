-- 프로세스 설정(TSMPROCCONFIG) - 업무 처리 방식/허용 기준처럼 프로세스 동작을 바꾸는 값을 키-값으로
-- 관리한다(2026-09-25). 회사정보/메일/비밀번호정책을 담는 TSMSITECONFIG(단일 행, 값마다 컬럼)와는
-- 별개다 - 프로세스 정책은 모듈이 늘수록 계속 늘어나서 값 하나마다 컬럼+프로시저+DTO+화면을 고치는
-- 방식으로는 감당이 안 된다. 새 설정은 이 테이블에 행 하나를 INSERT하는 마이그레이션으로 끝난다.
--
--  * 설정 "정의"(config_key/타입/범위/기본값/설명)는 개발자가 마이그레이션으로 넣고, 관리자는 화면에서
--    "값"(config_value)만 바꾼다. config_value가 NULL이면 default_value를 쓴다.
--  * ENUM 타입은 enum_major_cd 공통코드의 사용(use_yn='Y') 항목만 선택지로 보인다 - 아직 준비 안 된
--    선택지는 use_yn='N'으로 시드해 두면 화면에 나타나지 않고, 준비되면 use_yn만 바꿔 연다.
--  * 서버 프로시저는 FSM_PROCCONFIG(config_key)로 값을 읽는다. 값이 바뀌어도 이미 확정된 문서는
--    소급하지 않는다(값은 프로시저가 확정하는 순간에 읽는다).
--  * 값이 바뀔 때마다 TSMPROCCONFIGHIST에 이전/이후 값을 남긴다(처리 경로가 바뀌는 값이라 추적 필요).

-- ============================================================
-- 1) 테이블
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMPROCCONFIG')
BEGIN
    CREATE TABLE TSMPROCCONFIG (
        config_key      VARCHAR(50)    NOT NULL,                    -- 모듈.이름 (예: MA.GR_MODE)
        module_cd       VARCHAR(10)    NOT NULL,                    -- MA 구매, SA 판매, PR 생산, SM 공통
        group_nm        NVARCHAR(50)   NULL,                        -- 화면 묶음 이름
        config_nm       NVARCHAR(100)  NOT NULL,                    -- 설정 이름
        data_type       VARCHAR(10)    NOT NULL,                    -- YN / ENUM / INT / DEC / TEXT
        enum_major_cd   VARCHAR(20)    NULL,                        -- ENUM일 때 선택지를 가져올 공통코드 대분류
        min_value       NUMERIC(18,4)  NULL,                        -- INT/DEC 하한
        max_value       NUMERIC(18,4)  NULL,                        -- INT/DEC 상한
        default_value   NVARCHAR(200)  NOT NULL,                    -- 기본값
        config_value    NVARCHAR(200)  NULL,                        -- 현재 값(NULL이면 기본값 사용)
        description     NVARCHAR(500)  NULL,                        -- 바꾸면 무엇이 달라지는지 / 적용 시점
        sort            INT            NOT NULL CONSTRAINT DF_TSMPROCCONFIG_sort DEFAULT 0,
        use_yn          VARCHAR(1)     NOT NULL CONSTRAINT DF_TSMPROCCONFIG_use_yn DEFAULT 'Y',
        reg_user_id     VARCHAR(50)    NULL,
        reg_dt          DATETIME       NULL,
        reg_pc          NVARCHAR(200)  NULL,
        upt_user_id     VARCHAR(50)    NULL,
        upt_dt          DATETIME       NULL,
        upt_pc          NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSMPROCCONFIG PRIMARY KEY CLUSTERED (config_key),
        CONSTRAINT CK_TSMPROCCONFIG_type CHECK (data_type IN ('YN', 'ENUM', 'INT', 'DEC', 'TEXT'))
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMPROCCONFIGHIST')
BEGIN
    CREATE TABLE TSMPROCCONFIGHIST (
        hist_id         BIGINT IDENTITY(1,1) NOT NULL,
        config_key      VARCHAR(50)    NOT NULL,
        old_value       NVARCHAR(200)  NULL,
        new_value       NVARCHAR(200)  NULL,
        chg_user_id     VARCHAR(50)    NULL,
        chg_dt          DATETIME       NOT NULL CONSTRAINT DF_TSMPROCCONFIGHIST_chg_dt DEFAULT GETDATE(),
        chg_pc          NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSMPROCCONFIGHIST PRIMARY KEY CLUSTERED (hist_id)
    );

    CREATE NONCLUSTERED INDEX IX_TSMPROCCONFIGHIST_key ON TSMPROCCONFIGHIST (config_key, hist_id DESC);
END
GO

-- ============================================================
-- 2) 값 읽기 함수 - 다른 프로시저가 FSM_PROCCONFIG('MA.GR_MODE')처럼 부른다.
--    사용안함(use_yn='N') 설정은 저장된 값이 있어도 기본값을 돌려준다.
-- ============================================================
CREATE OR ALTER FUNCTION dbo.FSM_PROCCONFIG (@config_key VARCHAR(50))
RETURNS NVARCHAR(200)
AS
BEGIN
    RETURN (
        SELECT CASE WHEN use_yn = 'Y' THEN ISNULL(config_value, default_value) ELSE default_value END
        FROM TSMPROCCONFIG
        WHERE config_key = @config_key
    );
END
GO

-- ============================================================
-- 3) 공통코드 MA0009(구매 입고방식) - 자동(A)은 입고 프로시저가 완성되기 전까지 사용안함으로 시드한다
--    (선택지에 안 나타남). 입고 연결이 끝나면 use_yn만 'Y'로 바꾸면 화면에 열린다.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0009')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('MA0009', N'구매 입고방식', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0009' AND minor_cd = 'M')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0009', 'M', N'수동', 1, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0009' AND minor_cd = 'A')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0009', 'A', N'자동', 2, 'Y', 'N', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 4) 초기 설정 2건
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMPROCCONFIG WHERE config_key = 'MA.GR_MODE')
INSERT INTO TSMPROCCONFIG (
    config_key, module_cd, group_nm, config_nm, data_type, enum_major_cd,
    min_value, max_value, default_value, description, sort, use_yn,
    reg_user_id, reg_dt, upt_user_id, upt_dt
)
VALUES (
    'MA.GR_MODE', 'MA', N'납품/입고', N'구매 입고방식', 'ENUM', 'MA0009',
    NULL, NULL, 'M',
    N'자동이면 무검사품은 납품 확정 시, 검사품은 검사 확정 시 입고가 자동 처리됩니다. 변경 이후 확정되는 문서부터 적용되고, 이미 입고대기인 건은 그대로 입고 화면에서 처리합니다.',
    10, 'Y', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE()
);
GO

IF NOT EXISTS (SELECT 1 FROM TSMPROCCONFIG WHERE config_key = 'MA.OVER_DELV_PCT')
INSERT INTO TSMPROCCONFIG (
    config_key, module_cd, group_nm, config_nm, data_type, enum_major_cd,
    min_value, max_value, default_value, description, sort, use_yn,
    reg_user_id, reg_dt, upt_user_id, upt_dt
)
VALUES (
    'MA.OVER_DELV_PCT', 'MA', N'납품/입고', N'초과 납품 허용율(%)', 'DEC', NULL,
    0, 100, '0',
    N'발주 잔량 대비 이 비율까지 초과 납품을 받습니다. 0이면 초과 납품 불가입니다. 변경 이후 저장/확정하는 납품부터 적용됩니다.',
    20, 'Y', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE()
);
GO

-- ============================================================
-- 5) USP_SM_PROCCONFIG_Q - Q: 설정 목록(모듈/키워드 조건), Q1: 한 설정의 변경 이력,
--    Q2: ENUM 선택지(사용 Y 항목만) - 화면이 행마다 콤보 선택지를 만들 때 쓴다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_PROCCONFIG_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_module_cd VARCHAR(10) = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_config_key VARCHAR(50) = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_work_type = 'Q'
    BEGIN
        SELECT
            c.config_key,
            c.module_cd,
            CASE c.module_cd WHEN 'MA' THEN N'구매' WHEN 'SA' THEN N'판매' WHEN 'PR' THEN N'생산' WHEN 'SM' THEN N'공통' ELSE c.module_cd END AS module_nm,
            c.group_nm,
            c.config_nm,
            c.data_type,
            c.enum_major_cd,
            c.min_value,
            c.max_value,
            c.default_value,
            c.config_value,
            ISNULL(c.config_value, c.default_value) AS cur_value,
            c.description,
            c.sort,
            c.use_yn,
            CASE WHEN c.config_value IS NULL OR c.config_value = c.default_value THEN 'Y' ELSE 'N' END AS default_yn,
            c.upt_user_id,
            c.upt_dt
        FROM TSMPROCCONFIG c
        WHERE c.use_yn = 'Y'
          AND (@p_module_cd IS NULL OR @p_module_cd = '' OR c.module_cd = @p_module_cd)
          AND (@p_keyword IS NULL OR @p_keyword = ''
               OR c.config_key LIKE '%' + @p_keyword + '%'
               OR c.config_nm LIKE '%' + @p_keyword + '%')
        ORDER BY c.module_cd, c.group_nm, c.sort, c.config_key;
    END
    ELSE IF @p_work_type = 'Q1'
    BEGIN
        SELECT
            h.hist_id,
            h.config_key,
            h.old_value,
            h.new_value,
            h.chg_user_id,
            h.chg_dt,
            h.chg_pc
        FROM TSMPROCCONFIGHIST h
        WHERE h.config_key = @p_config_key
        ORDER BY h.hist_id DESC;
    END
    ELSE IF @p_work_type = 'Q2'
    BEGIN
        SELECT DISTINCT
            m.major_cd,
            m.minor_cd,
            m.minor_nm,
            m.sort
        FROM TSMMINOR m
        JOIN TSMPROCCONFIG c ON c.enum_major_cd = m.major_cd AND c.use_yn = 'Y'
        WHERE m.use_yn = 'Y'
        ORDER BY m.major_cd, m.sort, m.minor_cd;
    END
END
GO

-- ============================================================
-- 6) USP_SM_PROCCONFIG_S - U: 값 수정(빈 값이면 기본값으로 되돌림). 타입/범위/ENUM 선택지를 서버가
--    다시 검증하고, 값이 실제로 바뀔 때만 같은 트랜잭션에서 이력을 남긴다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_PROCCONFIG_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_config_key VARCHAR(50) = NULL,
    @p_config_value NVARCHAR(200) = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            DECLARE @data_type VARCHAR(10), @enum_major_cd VARCHAR(20), @min_value NUMERIC(18,4), @max_value NUMERIC(18,4),
                    @default_value NVARCHAR(200), @old_value NVARCHAR(200), @config_nm NVARCHAR(100);

            SELECT @data_type = data_type, @enum_major_cd = enum_major_cd, @min_value = min_value, @max_value = max_value,
                   @default_value = default_value, @old_value = config_value, @config_nm = config_nm
            FROM TSMPROCCONFIG
            WHERE config_key = @p_config_key AND use_yn = 'Y';

            IF @data_type IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'존재하지 않거나 사용하지 않는 설정입니다.';
                RETURN;
            END

            -- 빈 값 = 기본값으로 되돌리기(config_value를 NULL로 저장)
            IF @p_config_value = N'' SET @p_config_value = NULL;

            IF @p_config_value IS NOT NULL
            BEGIN
                IF @data_type = 'YN' AND @p_config_value NOT IN (N'Y', N'N')
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = @config_nm + N': Y 또는 N만 입력할 수 있습니다.'; RETURN;
                END

                IF @data_type = 'ENUM' AND NOT EXISTS (
                    SELECT 1 FROM TSMMINOR WHERE major_cd = @enum_major_cd AND minor_cd = @p_config_value AND use_yn = 'Y')
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = @config_nm + N': 선택할 수 없는 값입니다.'; RETURN;
                END

                IF @data_type = 'INT' AND (TRY_CAST(@p_config_value AS BIGINT) IS NULL
                        OR TRY_CAST(@p_config_value AS NUMERIC(18,4)) <> TRY_CAST(@p_config_value AS BIGINT))
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = @config_nm + N': 정수만 입력할 수 있습니다.'; RETURN;
                END

                IF @data_type = 'DEC' AND TRY_CAST(@p_config_value AS NUMERIC(18,4)) IS NULL
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = @config_nm + N': 숫자만 입력할 수 있습니다.'; RETURN;
                END

                IF @data_type IN ('INT', 'DEC')
                BEGIN
                    DECLARE @num NUMERIC(18,4) = TRY_CAST(@p_config_value AS NUMERIC(18,4));
                    IF (@min_value IS NOT NULL AND @num < @min_value) OR (@max_value IS NOT NULL AND @num > @max_value)
                    BEGIN
                        SET @ReturnCode = -1;
                        SET @ReturnMsg = @config_nm + N': 허용 범위(' + ISNULL(CAST(CAST(@min_value AS FLOAT) AS NVARCHAR(30)), N'') + N' ~ '
                                       + ISNULL(CAST(CAST(@max_value AS FLOAT) AS NVARCHAR(30)), N'') + N')를 벗어났습니다.';
                        RETURN;
                    END
                END
            END

            -- 실제로 값이 바뀔 때만 수정 + 이력(효과값 기준 비교: 기본값과 같은 값을 저장해도 변경으로 보지 않는다)
            IF ISNULL(@p_config_value, @default_value) <> ISNULL(@old_value, @default_value)
            BEGIN
                BEGIN TRAN;

                UPDATE TSMPROCCONFIG SET
                    config_value = @p_config_value,
                    upt_user_id = @p_user_id,
                    upt_dt = GETDATE(),
                    upt_pc = @p_client_pc
                WHERE config_key = @p_config_key;

                INSERT INTO TSMPROCCONFIGHIST (config_key, old_value, new_value, chg_user_id, chg_dt, chg_pc)
                VALUES (@p_config_key, ISNULL(@old_value, @default_value), ISNULL(@p_config_value, @default_value),
                        @p_user_id, GETDATE(), @p_client_pc);

                COMMIT TRAN;
            END
            ELSE IF ISNULL(@p_config_value, N'') <> ISNULL(@old_value, N'')
            BEGIN
                -- 효과값은 같은데 저장 표현만 다른 경우(기본값 명시 <-> NULL): 이력 없이 표현만 맞춘다
                UPDATE TSMPROCCONFIG SET config_value = @p_config_value WHERE config_key = @p_config_key;
            END

            SET @GeneratedCode = LEFT(@p_config_key, 20);
        END
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 7) 메뉴 등록 - 시스템운영 > Configuration(44) 밑, frmSiteConfig 다음. 기본 권한은 부여하지 않는다
--    (메뉴권한 화면에서 관리자 그룹에 부여).
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SM' AND SCREEN_CLASS_NM = 'frmProcConfig')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'프로세스설정', 44, 3, 'FORM', 'SM', 'frmProcConfig', 'USP_SM_PROCCONFIG_', 20, 'Y', SUSER_SNAME(), GETDATE());
END
GO
