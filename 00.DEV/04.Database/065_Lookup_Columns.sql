-- LookUp 팝업이 지금까지 값필드/표시필드 2컬럼 고정이었는데(038_Lookup_Framework.sql 주석 참고 -
-- "LookUp은 결과셋 전체를 보여주는 게 아니라 코드값+표시값만 뽑아 쓴다"고 명시적으로 안 두기로
-- 했던 것), 팝업(sysPopUpD)처럼 조회된 컬럼 중 원하는 만큼 골라서 폭까지 지정할 수 있게
-- 확장한다(사장님 지시, 2026-09-03) - "쿼리에서 정의된 모든 컬럼을 보여줄 수 있어야 한다".
--
-- sysPopUpD와 달리 control_type/lookup_proc_nm은 없다 - LookUp 팝업은 항상 읽기전용 텍스트
-- 표시라 팝업의 그리드 편집기(LOOKUP 중첩 등) 개념이 필요 없다.
--
-- 이 테이블에 행이 하나도 없는 LookUp은 예전 그대로(값필드+표시필드 2컬럼 고정)로 동작한다 -
-- 완전히 하위호환. 행을 추가하는 순간부터 그 컬럼 구성을 그대로 쓴다(LookupRepository.
-- GetItemsAsync/LookUpEditWyn.LoadFromLookupKeyAsync 참고).

CREATE TABLE sysLookupC (
    lookup_key VARCHAR(30)  NOT NULL,
    column_nm  VARCHAR(50)  NOT NULL,               -- LookUp 소스(프로시져/쿼리) 결과셋의 실제 컬럼명
    caption    NVARCHAR(50) NULL,                    -- 팝업 헤더 표시 텍스트(NULL이면 column_nm 그대로)
    sort       INT          NOT NULL DEFAULT 0,
    width      INT          NOT NULL DEFAULT 100,     -- 컬럼 폭(px)
    visible_yn CHAR(1)      NOT NULL DEFAULT 'Y',
    CONSTRAINT PK_sysLookupC PRIMARY KEY (lookup_key, column_nm),
    CONSTRAINT FK_sysLookupC_sysLookupM FOREIGN KEY (lookup_key) REFERENCES sysLookupM(lookup_key)
);
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30) = NULL,   /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'/'Q2'일 때는 정확히 일치하는 LookUp키 */
    @p_lookup_nm NVARCHAR(100) = NULL,
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
            SELECT lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark
            FROM sysLookupM
            WHERE (@p_lookup_key IS NULL OR lookup_key LIKE '%' + @p_lookup_key + '%')
              AND (@p_lookup_nm IS NULL OR lookup_nm LIKE '%' + @p_lookup_nm + '%')
            ORDER BY lookup_key;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT lookup_key, param_nm, caption, sort
            FROM sysLookupP
            WHERE lookup_key = @p_lookup_key
            ORDER BY sort;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            SELECT lookup_key, column_nm, caption, sort, width, visible_yn
            FROM sysLookupC
            WHERE lookup_key = @p_lookup_key
            ORDER BY sort;
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

-- 기존 USP_SYS_LOOKUP_S의 'D' 분기에 sysLookupC 삭제만 추가 - 나머지는 그대로.
CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_proc_nm VARCHAR(100) = NULL,
    @p_lookup_nm NVARCHAR(100) = NULL,
    @p_value_field VARCHAR(50) = NULL,
    @p_display_field VARCHAR(50) = NULL,
    @p_use_yn VARCHAR(1) = 'Y',
    @p_remark NVARCHAR(200) = NULL,
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
            INSERT INTO sysLookupM (
                lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt
            )
            VALUES (
                @p_lookup_key, @p_proc_nm, @p_lookup_nm, @p_value_field, @p_display_field, @p_use_yn, @p_remark, @p_user_id, GETDATE()
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupM SET
                proc_nm = @p_proc_nm, lookup_nm = @p_lookup_nm, value_field = @p_value_field,
                display_field = @p_display_field, use_yn = @p_use_yn, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHERE lookup_key = @p_lookup_key;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupC WHERE lookup_key = @p_lookup_key;
            DELETE FROM sysLookupP WHERE lookup_key = @p_lookup_key;
            DELETE FROM sysLookupM WHERE lookup_key = @p_lookup_key;
        END

        SET @GeneratedCode = @p_lookup_key;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- USP_SYS_LOOKUP_S_1(sysLookupP, 파라미터)과 같은 구조로 sysLookupC(컬럼) 저장/삭제를 처리한다.
CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_column_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_sort INT = 0,
    @p_width INT = 100,
    @p_visible_yn VARCHAR(1) = 'Y',
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
            INSERT INTO sysLookupC (lookup_key, column_nm, caption, sort, width, visible_yn)
            VALUES (@p_lookup_key, @p_column_nm, @p_caption, @p_sort, @p_width, @p_visible_yn);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupC SET caption = @p_caption, sort = @p_sort, width = @p_width, visible_yn = @p_visible_yn
            WHERE lookup_key = @p_lookup_key AND column_nm = @p_column_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupC WHERE lookup_key = @p_lookup_key AND column_nm = @p_column_nm;
        END

        SET @GeneratedCode = @p_column_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
