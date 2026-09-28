-- 팝업 전용 검색패널 지원(2026-09-25) - 조회조건에 룩업/여러 줄/조건 간 연동이 필요한 팝업은
-- sysPopUpS 정의만으로 자동 생성하기가 너무 어려워져서, 그런 팝업만 VS 디자이너로 만든 UserControl
-- (WYNLAB.Base.PopupSearchPanelBase 상속)을 검색조건 영역으로 붙일 수 있게 한다. 그리드 결과
-- 컬럼은 여전히 sysPopUpD 정의대로 자동 생성.
--
-- search_panel_class: 그 UserControl의 전체 이름(예: WYNLAB.BA.pnlItemSearch). NULL이면 지금처럼
-- sysPopUpS 기반 자동 생성. 컨트롤 Name = 프로시저 파라미터명(p_xxx)이라 따로 연결하는 매핑 테이블은 없다.
--
-- SSP_SYS_POPUP_Q/SSP_SYS_POPUP_S는 sp_rename으로 이름만 바뀐 프로시저라 OBJECT_DEFINITION 텍스트
-- 안의 CREATE PROCEDURE 이름은 옛 이름(USP_)이다 - 반드시 실제 객체명(SSP_)으로 CREATE OR ALTER
-- (181 파일 설명 참고). 현재 정의는 배포 직전 실제 DB에서 다시 읽어 그 위에 이번 변경만 얹었다.
--
-- 같이 고친 것: SSP_SYS_POPUP_S의 'D'는 sysPopUpD만 지우고 sysPopUpS(조회조건)는 안 지워서
-- FK_sysPopUpS_sysPopUpM 때문에 조회조건이 있는 팝업은 삭제가 실패했다. 그리고 INSERT에도
-- upt_user_id/upt_dt를 같이 채운다(upt_* 컨벤션).

ALTER TABLE sysPopUpM ADD search_panel_class VARCHAR(200) NULL;
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
            SELECT popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no
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

CREATE OR ALTER PROCEDURE [dbo].[SSP_SYS_POPUP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_proc_nm VARCHAR(100) = NULL,
    @p_popup_nm NVARCHAR(100) = NULL,
    @p_hierarchical_yn VARCHAR(1) = 'N',
    @p_key_field VARCHAR(50) = NULL,
    @p_parent_field VARCHAR(50) = NULL,
    @p_display_field VARCHAR(50) = NULL,
    @p_popup_width INT = 700,
    @p_popup_height INT = 500,
    @p_use_yn VARCHAR(1) = 'Y',
    @p_remark NVARCHAR(200) = NULL,
    @p_search_panel_class VARCHAR(200) = NULL,
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
            INSERT INTO sysPopUpM (
                popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                display_field, popup_width, popup_height, use_yn, remark, search_panel_class,
                reg_user_id, reg_dt, upt_user_id, upt_dt
            )
            VALUES (
                @p_popup_key, @p_proc_nm, @p_popup_nm, @p_hierarchical_yn, @p_key_field, @p_parent_field,
                @p_display_field, @p_popup_width, @p_popup_height, @p_use_yn, @p_remark, @p_search_panel_class,
                @p_user_id, GETDATE(), @p_user_id, GETDATE()
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpM SET
                proc_nm = @p_proc_nm, popup_nm = @p_popup_nm, hierarchical_yn = @p_hierarchical_yn,
                key_field = @p_key_field, parent_field = @p_parent_field, display_field = @p_display_field,
                popup_width = @p_popup_width, popup_height = @p_popup_height, use_yn = @p_use_yn,
                remark = @p_remark, search_panel_class = @p_search_panel_class,
                upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHERE popup_key = @p_popup_key;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpS WHERE popup_key = @p_popup_key;
            DELETE FROM sysPopUpD WHERE popup_key = @p_popup_key;
            DELETE FROM sysPopUpM WHERE popup_key = @p_popup_key;
        END

        SET @GeneratedCode = @p_popup_key;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 품목 팝업 프로시저에 자산구분 조건 추가(선택 파라미터라 기존 호출은 그대로 동작) - 전용 검색패널
-- 샘플(BA pnlItemSearch)의 룩업 조건이 실제로 걸리게 하려는 것.
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_ITEM_Q]
    @p_keyword VARCHAR(100) = NULL,
    @p_asset_type VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.item_id, a.item_no, a.item_nm, a.item_spec, a.asset_type,
           a.unit_cd, a.po_unit_cd,
           a.wh_id, w.wh_nm, a.loc_id, l.loc_nm
    FROM TBAITEM a
        LEFT JOIN TBAWH w ON w.wh_id = a.wh_id
        LEFT JOIN TBALOC l ON l.loc_id = a.loc_id
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR a.item_no LIKE '%' + @p_keyword + '%'
           OR a.item_nm LIKE '%' + @p_keyword + '%')
      AND (@p_asset_type IS NULL OR @p_asset_type = '' OR a.asset_type = @p_asset_type)
    ORDER BY a.item_id;
END
GO

-- 샘플 팝업 P_ITEMPNL - 기존 P_ITEM의 정의(결과 컬럼 등)를 그대로 복사하고 검색패널만 전용으로.
-- P_ITEM 자체는 건드리지 않는다.
IF NOT EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_ITEMPNL')
   AND EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_ITEM')
BEGIN
    INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                           display_field, popup_width, popup_height, use_yn, remark, search_panel_class,
                           reg_user_id, reg_dt, upt_user_id, upt_dt)
    SELECT 'P_ITEMPNL', proc_nm, popup_nm + N' (전용검색패널 샘플)', hierarchical_yn, key_field, parent_field,
           display_field, popup_width, popup_height, use_yn, N'전용 검색패널 샘플 - BA.pnlItemSearch',
           'WYNLAB.BA.pnlItemSearch', 'system', GETDATE(), 'system', GETDATE()
    FROM sysPopUpM WHERE popup_key = 'P_ITEM';

    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
    SELECT 'P_ITEMPNL', column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn
    FROM sysPopUpD WHERE popup_key = 'P_ITEM';
END
GO
