-- 팝업 조회조건(sysPopUpS)에 LOOKUP 컨트롤타입 + 여러 줄 배치 지원 추가(2026-09-23 요청 -
-- "조회조건부에 컨트롤유형이 TEXT, DATE밖에 없어. LookUp도 필요할 것 같은데... 그리고 조회
-- 조건을 한줄로 밖에 표현이 안되는데 이것도 조정할 수 있는 방법이 필요하겠어").
--
-- lookup_key: LOOKUP 타입일 때 어떤 콤보 목록을 쓸지(sysLookupM.lookup_key) - sysPopUpD의
-- lookup_proc_nm(프로시저 직접 지정)과는 다른 계열이다. 조회조건은 "이미 있는 코드/명 콤보에서
-- 하나 골라서 필터링"하는 용도가 자연스러워서, LookUpEditWyn이 이미 쓰는 "LookupKey"(별도
-- 프레임워크 sysLookupM/P)를 그대로 재사용한다 - 사용자가 직접 "LookUpKey도 필요하겠지"라고
-- 짚은 그 이름 그대로.
--
-- row_no: 조회조건이 많아지면 한 줄에 다 못 넣는 팝업이 생긴다(현재 popPopUp.BuildSearchPanel은
-- 고정 한 줄) - 몇 번째 줄에 놓을지 관리자가 여기서 지정하면 런타임이 그 줄 수만큼 패널 높이를
-- 늘려서 그린다. 기존 팝업은 전부 기본값 1(한 줄)로 지금과 동일하게 보인다.
--
-- SSP_SYS_POPUP_Q/SSP_SYS_POPUP_S_2는 sp_rename으로 이름만 USP_->SSP_로 바뀐 프로시저라
-- (OBJECT_DEFINITION 안의 CREATE PROCEDURE 텍스트는 여전히 옛 이름 USP_SYS_POPUP_*를 담고
-- 있음 - sp_rename은 그 텍스트를 안 고친다) 반드시 실제 객체명(SSP_SYS_POPUP_*)으로 CREATE OR
-- ALTER해야 한다 - USP_ 이름으로 하면 별도의 새 프로시저가 생기고 실제 쓰이는 SSP_ 쪽은 그대로
-- 남는다(migration_regression_check 컨벤션 - 배포 전 실제 서버에서 OBJECT_DEFINITION으로 현재
-- 정의를 다시 확인했음).

ALTER TABLE sysPopUpS ADD
    lookup_key VARCHAR(30) NULL,
    row_no     INT         NOT NULL DEFAULT 1;
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
            INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no)
            VALUES (@p_popup_key, @p_param_nm, @p_caption, @p_control_type, @p_sort, @p_width, @p_lookup_key, @p_row_no);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpS SET
                caption = @p_caption, control_type = @p_control_type, sort = @p_sort, width = @p_width,
                lookup_key = @p_lookup_key, row_no = @p_row_no
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
