-- 창고(TBAWH)/위치(TBALOC) 마스터 화면(frmWh) + 품목등록(frmItem)의 wh_id/loc_id 팝업 연동
-- (2026-09-15). 두 테이블은 사장님이 직접 생성 - wh_id/loc_id 둘 다 BIGINT IDENTITY, TBALOC.wh_id가
-- TBAWH.wh_id에 종속(위치는 항상 어느 창고 소속). frmItem은 원래 wh_id/loc_id를 SpinEditWyn(순수
-- 숫자입력)으로 받고 있었는데, 이름으로 찾아 선택할 수 있게 PopupLookupEditWyn(멀티필드 모드,
-- MatchField+MapField)로 바꾼다 - 그러려면 이 두 테이블에 대한 팝업(P_WH/P_LOC)과 frmItem에서
-- 이름까지 같이 조회해줄 조인이 필요하다.

-- ============================================================
-- 1) SSP_POP_WH_Q / SSP_POP_LOC_Q - P_WH/P_LOC 팝업 소스. 검색은 이름으로만(SSP_POP_DEPT_Q와
--    같은 패턴).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_WH_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT wh_id, wh_nm, wh_type
    FROM TBAWH
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR wh_nm LIKE '%' + @p_keyword + '%')
    ORDER BY wh_id;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_LOC_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT loc_id, loc_nm, loc_type, wh_id
    FROM TBALOC
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR loc_nm LIKE '%' + @p_keyword + '%')
    ORDER BY loc_id;
END
GO

-- ============================================================
-- 2) 팝업 메타데이터 등록 (sysPopUpM/D/S) - P_DEPT/P_EMP와 같은 형태.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_WH')
INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field, display_field, popup_width, popup_height, use_yn, reg_user_id, reg_dt)
VALUES ('P_WH', 'SSP_POP_WH_Q', N'창고 조회', 'N', 'wh_id', NULL, 'wh_nm', 700, 500, 'Y', SUSER_SNAME(), GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_LOC')
INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field, display_field, popup_width, popup_height, use_yn, reg_user_id, reg_dt)
VALUES ('P_LOC', 'SSP_POP_LOC_Q', N'위치 조회', 'N', 'loc_id', NULL, 'loc_nm', 700, 500, 'Y', SUSER_SNAME(), GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_WH')
BEGIN
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, sort, width, visible_yn) VALUES
    ('P_WH', 'wh_id',   N'창고ID', 'TEXT', 1, 100, 'N'),
    ('P_WH', 'wh_nm',   N'창고명', 'TEXT', 2, 200, 'Y'),
    ('P_WH', 'wh_type', N'창고유형', 'TEXT', 3, 100, 'Y');
END
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_LOC')
BEGIN
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, sort, width, visible_yn) VALUES
    ('P_LOC', 'loc_id',   N'위치ID', 'TEXT', 1, 100, 'N'),
    ('P_LOC', 'loc_nm',   N'위치명', 'TEXT', 2, 200, 'Y'),
    ('P_LOC', 'loc_type', N'위치유형', 'TEXT', 3, 100, 'Y'),
    ('P_LOC', 'wh_id',    N'창고ID', 'TEXT', 4, 100, 'N');
END
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_WH')
INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width) VALUES ('P_WH', 'p_keyword', N'창고명', 'TEXT', 1, 180);
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_LOC')
INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width) VALUES ('P_LOC', 'p_keyword', N'위치명', 'TEXT', 1, 180);
GO

-- ============================================================
-- 3) USP_BA_WH_Q - frmWh 마스터(창고) 목록 + Q1(선택된 창고의 위치 목록, grd2용).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_WH_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_wh_id BIGINT = NULL,
    @p_wh_nm NVARCHAR(200) = NULL,
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
                a.wh_id, a.wh_nm, a.acc_id, a.wh_type,
                a.dept_id, b.dept_nm,
                a.emp_id, c.emp_nm
            FROM        TBAWH as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
            WHERE       1 = 1
            AND         (@p_wh_nm IS NULL OR a.wh_nm LIKE '%' + @p_wh_nm + '%')
            ORDER BY a.wh_id;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT loc_id, loc_nm, loc_type, wh_id
            FROM TBALOC
            WHERE wh_id = @p_wh_id
            ORDER BY loc_id;
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
-- 4) USP_BA_WH_S - 창고(헤더) N/U/D. wh_id는 IDENTITY라 N에서 SCOPE_IDENTITY()로 돌려준다.
--    위치가 하나라도 있으면 삭제를 막는다(frmDept의 상위부서 삭제 제약과 같은 원칙).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_WH_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_wh_id BIGINT = NULL,
    @p_wh_nm NVARCHAR(200) = NULL,
    @p_acc_id BIGINT = NULL,
    @p_wh_type VARCHAR(10) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
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
            INSERT INTO TBAWH (wh_nm, acc_id, wh_type, dept_id, emp_id, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_wh_nm, @p_acc_id, @p_wh_type, @p_dept_id, @p_emp_id, @p_user_id, GETDATE(), @p_client_pc);

            SET @p_wh_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAWH SET
                wh_nm = @p_wh_nm,
                acc_id = @p_acc_id,
                wh_type = @p_wh_type,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE wh_id = @p_wh_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TBALOC WHERE wh_id = @p_wh_id)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이 창고에 등록된 위치가 있어 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TBAWH WHERE wh_id = @p_wh_id;
        END

        SET @GeneratedCode = CAST(@p_wh_id AS VARCHAR(20));
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
-- 5) USP_BA_WH_LOC_S - 위치(grd2) 행별 N/U/D(frmMinorCode의 USP_SM_MINORCODE_S_1과 같은
--    행단위 저장 방식). 프로시저 이름이 USP_BA_WH_로 시작해야 frmWh 메뉴의 PROC_PREFIX
--    화이트리스트를 지난다.
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_WH_LOC_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_loc_nm NVARCHAR(200) = NULL,
    @p_loc_type VARCHAR(10) = NULL,
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
            INSERT INTO TBALOC (loc_nm, loc_type, acc_id, wh_id, reg_user_id, reg_dt, reg_pc)
            SELECT @p_loc_nm, @p_loc_type, a.acc_id, @p_wh_id, @p_user_id, GETDATE(), @p_client_pc
            FROM TBAWH a WHERE a.wh_id = @p_wh_id;

            SET @p_loc_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBALOC SET
                loc_nm = @p_loc_nm,
                loc_type = @p_loc_type,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE loc_id = @p_loc_id AND wh_id = @p_wh_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBALOC WHERE loc_id = @p_loc_id AND wh_id = @p_wh_id;
        END

        SET @GeneratedCode = CAST(@p_loc_id AS VARCHAR(20));
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
-- 6) USP_BA_ITEM_Q에 wh_nm/loc_nm 조인 추가 - frmItem의 popDetailWhNm/popDetailLocNm이 조회
--    시점에 이름을 보여주려면 필요하다(migration 150에서 재작성한 최신 정의를 그대로 가져와
--    조인 2개 + SELECT 컬럼 2개만 추가).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    @p_item_no VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
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
                a.acc_id,
                a.item_id,
                a.item_no,
                a.item_nm,
                a.item_spec,
                a.unit_cd,
                a.po_unit_cd,
                a.wh_id,
                e.wh_nm,
                a.loc_id,
                f.loc_nm,
                a.safe_qty,
                a.dept_id,
                b.dept_nm,
                a.emp_id,
                c.emp_no,
                c.emp_nm,
                a.cust_id,
                d.cust_nm,
                a.asset_type,
                a.out_type,
                a.po_qc_yn,
                a.prod_qc_yn,
                a.lot_yn,
                a.stock_yn,
                a.stat_cd,
                a.grp1_id,
                a.grp2_id,
                a.grp3_id,
                a.grp4_id,
                a.remark
            FROM        TBAITEM as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
                        LEFT OUTER JOIN TBACUST as d on a.cust_id = d.cust_id
                        LEFT OUTER JOIN TBAWH as e on a.wh_id = e.wh_id
                        LEFT OUTER JOIN TBALOC as f on a.loc_id = f.loc_id
            WHERE       1 = 1
   --         AND         (@p_item_id IS NULL OR a.item_id = @p_item_id)
            AND         (@p_item_no IS NULL OR a.item_no LIKE '%' + @p_item_no + '%')
            AND         (@p_item_nm IS NULL OR a.item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY a.item_id;
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
-- 7) 메뉴 등록 - 품목관리 그룹(UPPER_MENU_ID=9, frmItem/frmItemGrp/frmItemList와 같은 그룹) 아래.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'BA' AND SCREEN_CLASS_NM = 'frmWh')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'창고/위치등록', 9, 3, 'FORM', 'BA', 'frmWh', 'USP_BA_WH_', 30, 'Y', SUSER_SNAME(), GETDATE());
END
GO
