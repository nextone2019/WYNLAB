-- LookUp(콤보) 프레임워크 - 팝업 프레임워크(sysPopUpM/D/S)와 같은 발상을 콤보박스에 적용한다.
-- 지금까지는 새 콤보/LookUp 프로시져(SSP_CBO_*)를 만들 때마다 WYNLAB.BaseForm.ControlDataSources에
-- 프로시져 이름별로 Providers 등록 코드를 한 줄씩 추가해야 했다(코드 배포가 필요함). 이제는
-- sysLookupM에 "LookUp 이름"(lookup_key) 하나만 등록하면, 화면에서는 그 이름만 참조해서 쓸 수
-- 있다(LookUpEditWyn.LookupKey) - 코드 변경/배포 없이 새 콤보 추가 가능.
--
-- 프로시져마다 파라미터명/개수가 다 다를 수 있다(사장님 결정 - 팝업 조회조건과 같은 전제)라서
-- sysLookupP에 파라미터 목록을 별도로 둔다. 팝업의 조회조건(sysPopUpS)과 달리 이 파라미터는
-- 화면에 입력창으로 그려지는 게 아니라, 그 LookUp을 쓰는 화면의 코드가 LookUpEditWyn.SetParam()으로
-- 직접 값을 채워주는 것이라(예: 상위코드로 필터링) control_type/width가 필요 없다 - caption은
-- 관리 화면에서 "이 파라미터가 뭘 의미하는지" 사람이 참고하는 설명 문구일 뿐이다.
--
-- 컬럼(팝업의 sysPopUpD 같은 것)은 없다 - LookUp은 결과셋 전체를 보여주는 게 아니라 "코드값
-- 하나(value_field) + 표시값 하나(display_field)"만 뽑아 쓰기 때문이다.

CREATE TABLE sysLookupM (
    lookup_key    VARCHAR(30)   NOT NULL,
    proc_nm       VARCHAR(100)  NOT NULL,
    lookup_nm     NVARCHAR(100) NOT NULL,
    value_field   VARCHAR(50)   NOT NULL,               -- 결과셋 중 실제 값(코드)으로 쓸 컬럼명
    display_field VARCHAR(50)   NOT NULL,                -- 결과셋 중 화면에 보여줄 컬럼명(명칭)
    use_yn        CHAR(1)       NOT NULL DEFAULT 'Y',
    remark        NVARCHAR(200) NULL,
    reg_user_id   VARCHAR(50)   NULL,
    reg_dt        DATETIME      NULL DEFAULT GETDATE(),
    upt_user_id   VARCHAR(50)   NULL,
    upt_dt        DATETIME      NULL,
    CONSTRAINT PK_sysLookupM PRIMARY KEY (lookup_key)
);
GO

CREATE TABLE sysLookupP (
    lookup_key VARCHAR(30)  NOT NULL,
    param_nm   VARCHAR(50)  NOT NULL,                    -- 실제 프로시져 파라미터명(앞의 '@' 뗀 것)
    caption    NVARCHAR(50) NULL,                         -- 이 파라미터가 뭘 의미하는지(관리자 참고용 설명)
    sort       INT          NOT NULL DEFAULT 0,
    CONSTRAINT PK_sysLookupP PRIMARY KEY (lookup_key, param_nm),
    CONSTRAINT FK_sysLookupP_sysLookupM FOREIGN KEY (lookup_key) REFERENCES sysLookupM(lookup_key)
);
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30) = NULL,   /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'일 때는 정확히 일치하는 LookUp키 */
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
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

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

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_LOOKUP_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_param_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_sort INT = 0,
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
            INSERT INTO sysLookupP (lookup_key, param_nm, caption, sort)
            VALUES (@p_lookup_key, @p_param_nm, @p_caption, @p_sort);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupP SET caption = @p_caption, sort = @p_sort
            WHERE lookup_key = @p_lookup_key AND param_nm = @p_param_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupP WHERE lookup_key = @p_lookup_key AND param_nm = @p_param_nm;
        END

        SET @GeneratedCode = @p_param_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 파일럿: SSP_CBO_CODE_Q(소분류코드 조회)를 새 프레임워크로 등록. frmMinorCode의 관계코드유형
-- 콤보 10개는 여전히 예전 방식(LookUpEditWyn.ProcName/Where, ControlDataSources 하드코딩
-- 등록)을 그대로 쓴다 - 이 시딩은 "SSP_CBO_CODE_Q 기준으로 만들어달라"는 요청에 대한 동작
-- 예시일 뿐이고, 기존 화면을 새 방식으로 옮기는 건 별도 작업이다.
INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark)
VALUES ('MINOR_CODE', 'SSP_CBO_CODE_Q', N'소분류코드', 'minor_cd', 'minor_nm', 'Y', N'대분류코드로 소분류 목록 조회');
GO

INSERT INTO sysLookupP (lookup_key, param_nm, caption, sort)
VALUES ('MINOR_CODE', 'p_major_code', N'대분류코드', 1);
GO

-- SYS 모듈 메뉴 등록(팝업관리와 같은 그룹)
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN, PROC_PREFIX)
SELECT 'SYS_LOOKUP', N'LookUp관리', 'SYS', 2, 'FORM', 'WYNLAB.SYS.frmSysLookup, WYNLAB.SYS', NULL, 20, 'Y', 'USP_SYS_LOOKUP_'
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_CD = 'SYS_LOOKUP');
GO
