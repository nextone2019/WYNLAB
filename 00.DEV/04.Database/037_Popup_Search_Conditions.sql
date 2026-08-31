-- 팝업 조회조건(sysPopUpS) - 팝업 상단 검색창의 입력 필드 하나하나를 정의한다. 처음엔 모든
-- SSP_POP_*_Q가 "@p_keyword" 파라미터 하나만 받는다고 가정했는데, 그 규칙을 강제하지 않기로
-- 결정됨(사장님) - 프로시저마다 파라미터명/개수가 전부 다를 수 있다. 그래서 sysPopUpM에 라벨
-- 하나를 두는 대신, "컬럼생성"이 결과셋 컬럼(sysPopUpD)과 함께 프로시저의 입력 파라미터도
-- 읽어와서 이 테이블에 채워 넣고, 캡션(라벨 문구)/컨트롤타입/순서/폭은 관리자가 여기서 고친다.

CREATE TABLE sysPopUpS (
    popup_key    VARCHAR(30)   NOT NULL,
    param_nm     VARCHAR(50)   NOT NULL,               -- 실제 프로시저 파라미터명(앞의 '@' 뗀 것, 예: p_dept_cd)
    caption      NVARCHAR(50)  NULL,                    -- 검색창 라벨 텍스트(NULL이면 param_nm 그대로)
    control_type VARCHAR(10)   NOT NULL DEFAULT 'TEXT',  -- TEXT/DATE
    sort         INT           NOT NULL DEFAULT 0,
    width        INT           NOT NULL DEFAULT 120,     -- 입력창 폭(px)
    CONSTRAINT PK_sysPopUpS PRIMARY KEY (popup_key, param_nm),
    CONSTRAINT FK_sysPopUpS_sysPopUpM FOREIGN KEY (popup_key) REFERENCES sysPopUpM(popup_key)
);
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_POPUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30) = NULL,    /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'/'Q2'일 때는 정확히 일치하는 팝업키 */
    @p_popup_nm NVARCHAR(100) = NULL,
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
            SELECT popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                   display_field, popup_width, popup_height, use_yn, remark
            FROM sysPopUpM
            WHERE (@p_popup_key IS NULL OR popup_key LIKE '%' + @p_popup_key + '%')
              AND (@p_popup_nm IS NULL OR popup_nm LIKE '%' + @p_popup_nm + '%')
            ORDER BY popup_key;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn
            FROM sysPopUpD
            WHERE popup_key = @p_popup_key
            ORDER BY sort;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            SELECT popup_key, param_nm, caption, control_type, sort, width
            FROM sysPopUpS
            WHERE popup_key = @p_popup_key
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

-- 조회조건 그리드(grd3)의 행 하나 - USP_SYS_POPUP_S_1(컬럼설정)과 완전히 같은 구조,
-- param_nm이 키라 'U' 분기도 WHERE에서만 쓴다.
CREATE OR ALTER PROCEDURE [dbo].[USP_SYS_POPUP_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_param_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_control_type VARCHAR(10) = 'TEXT',
    @p_sort INT = 0,
    @p_width INT = 120,
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
            INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width)
            VALUES (@p_popup_key, @p_param_nm, @p_caption, @p_control_type, @p_sort, @p_width);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpS SET
                caption = @p_caption, control_type = @p_control_type, sort = @p_sort, width = @p_width
            WHERE popup_key = @p_popup_key AND param_nm = @p_param_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpS WHERE popup_key = @p_popup_key AND param_nm = @p_param_nm;
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

-- 기존 DEPT 팝업은 034_Popup_Dept_Seed.sql로 직접 넣은 것(frmSysPopup으로 만든 게 아님)이라
-- 조회조건도 여기서 한 번 채워준다. SSP_POP_DEPT_Q의 실제 파라미터는 @p_keyword 하나뿐.
INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width)
VALUES ('DEPT', 'p_keyword', N'부서코드/명', 'TEXT', 1, 180);
GO
