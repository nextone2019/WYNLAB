-- 전용 검색패널의 컨트롤과 프로시저 파라미터를 팝업관리에서 명시적으로 연결(2026-09-25, "팝업등록에서
-- 프로시저의 검색조건과 pnlItemSearch의 컨트롤과 연결하는 작업이 필요하지 않을까?").
--
-- control_nm: 그 파라미터(param_nm)의 값을 읽어올 검색패널 컨트롤의 Name. 비워두면 예전 규칙대로
-- "컨트롤 Name = 파라미터명"인 컨트롤에서 읽는다. 지정하면 컨트롤 이름이 파라미터명과 달라도 되고,
-- 같은 패널을 다른 프로시저에 재사용할 수도 있다. search_panel_class가 없는 팝업(자동 생성 검색창)
-- 에서는 쓰이지 않는다.
--
-- SSP_SYS_POPUP_Q / SSP_SYS_POPUP_S_2는 실제 DB의 현재 정의(181/182 반영본)를 읽어 그 위에 이번
-- 변경만 얹었다(migration_regression_check 컨벤션).

ALTER TABLE sysPopUpS ADD control_nm VARCHAR(50) NULL;
GO

CREATE OR ALTER PROCEDURE [dbo].[SSP_SYS_POPUP_Q]
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
                   display_field, popup_width, popup_height, use_yn, remark, search_panel_class
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
            SELECT popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no, control_nm
            FROM sysPopUpS
            WHERE popup_key = @p_popup_key
            ORDER BY row_no, sort;
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

CREATE OR ALTER PROCEDURE [dbo].[SSP_SYS_POPUP_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_param_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_control_type VARCHAR(10) = 'TEXT',
    @p_sort INT = 0,
    @p_width INT = 120,
    @p_lookup_key VARCHAR(30) = NULL,
    @p_row_no INT = 1,
    @p_control_nm VARCHAR(50) = NULL,
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
            INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no, control_nm)
            VALUES (@p_popup_key, @p_param_nm, @p_caption, @p_control_type, @p_sort, @p_width, @p_lookup_key, @p_row_no, @p_control_nm);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpS SET
                caption = @p_caption, control_type = @p_control_type, sort = @p_sort, width = @p_width,
                lookup_key = @p_lookup_key, row_no = @p_row_no, control_nm = @p_control_nm
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

-- 샘플 팝업 P_ITEMPNL의 조회조건(파라미터)을 등록하고 패널 컨트롤과 연결해 둔다 - 팝업관리에서 이 연결을
-- 그대로 볼 수 있게 하는 시연용(pnlItemSearch의 컨트롤 이름과 파라미터명이 우연히 같아서 같은 이름으로
-- 연결됨 - 이름이 달라도 control_nm만 바꾸면 된다).
IF EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_ITEMPNL')
   AND NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_ITEMPNL')
BEGIN
    INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no, control_nm)
    VALUES ('P_ITEMPNL', 'p_keyword',    N'품번/품명', 'TEXT',   1, 180, NULL,       1, 'p_keyword'),
           ('P_ITEMPNL', 'p_asset_type', N'자산구분',  'LOOKUP', 2, 140, 'L_CM0002', 1, 'p_asset_type');
END
GO
