-- 화면 기능(전자결재/첨부파일...) 메뉴별 설정 + 결재 상신 후 수정 잠금 (2026-10-03, WYNLAB_DEV 전용)
--
-- 1) TSMMENUFEATURE(menu_id, feature_cd, use_yn, option_val) - 메뉴등록 화면에서 "이 화면은 전자결재/첨부파일을 쓴다"를 지정한다.
--    feature_cd는 공통코드 SM0012(화면기능): APPROVAL(옵션 = 결재 문서유형 doc_type, AP0002), FILE(옵션 = 첨부 doc_type, 비우면 화면 클래스명).
--    공통 버튼 패널(FeatureBarWyn)이 로그인 때 내려온 메뉴 정보의 기능 목록을 보고 켜진 기능의 버튼만 보여준다.
--    USP_SM_MENUFEATURE_Q = 기능 조회(메뉴 지정 또는 전체), USP_SM_MENUFEATURE_S = 한 기능 저장(upsert).
-- 2) dbo.FN_AP_IS_LOCKED(app_id) - 결재가 상신돼서(진행상태 0/1) 진행 중이거나 승인완료(E)면 1. 반려(R)나 결재 없음은 0(수정 가능).
--    결재를 쓰는 문서의 저장 프로시저(S/S_1)가 이 함수로 "결재상신 이후는 수정/삭제 불가"를 서버에서도 지킨다.
-- 3) 기타입고/기타출고/기초재고를 결재 대상으로 쓸 수 있게: 헤더에 app_id/app_no(기타입고는 이미 있음), 조회(Q)에 app_no/appr_stat_cd,
--    저장 프로시저에 잠금 검증, 결재 문서유형 ETCIN/ETCOUT/OPENSTK(AP0002)과 후처리 프로시저(USP_AP_APPR_S_{doc_type}).
--    후처리 = 상신 시 문서에 결재번호 연결, 최종승인 시 해당 문서 확정(수불/재고 반영), 승인취소 시 확정취소, 기안자 취소 시 연결 해제.
--    확정을 어떻게 할지는 문서유형별 후처리 프로시저가 정한다(프레임워크는 관여하지 않음). 기안서 작성 타일(rel_cd1)은 비워 둔다.
-- 여러 번 실행해도 안전하다.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMMENUFEATURE')
CREATE TABLE TSMMENUFEATURE (
    menu_id      BIGINT        NOT NULL,
    feature_cd   VARCHAR(20)   NOT NULL,           -- 공통코드 SM0012
    use_yn       VARCHAR(1)    NOT NULL CONSTRAINT DF_TSMMENUFEATURE_use DEFAULT ('Y'),
    option_val   VARCHAR(100)  NULL,               -- 기능별 옵션(APPROVAL=doc_type, FILE=첨부 doc_type)
    reg_user_id  VARCHAR(30)   NULL,
    reg_dt       DATETIME      NULL,
    reg_pc       NVARCHAR(200) NULL,
    upt_user_id  VARCHAR(30)   NULL,
    upt_dt       DATETIME      NULL,
    upt_pc       NVARCHAR(200) NULL,
    CONSTRAINT PK_TSMMENUFEATURE PRIMARY KEY CLUSTERED (menu_id, feature_cd)
);
GO

IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'SM0012')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('SM0012', N'화면기능', 'Y', 'SYSTEM', GETDATE());
GO
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'SM0012')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('SM0012', 'APPROVAL', N'전자결재', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('SM0012', 'FILE',     N'첨부파일', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

CREATE OR ALTER PROCEDURE USP_SM_MENUFEATURE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT = NULL,              /* 비우면 전체 메뉴(로그인 때 한 번에 내려줄 때) */
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
            SELECT f.menu_id, f.feature_cd, f.use_yn, f.option_val
            FROM TSMMENUFEATURE f
            WHERE (@p_menu_id IS NULL OR f.menu_id = @p_menu_id)
            ORDER BY f.menu_id, f.feature_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_SM_MENUFEATURE_S
    @p_work_type VARCHAR(50),              /* SET = 한 기능의 사용여부/옵션 저장(없으면 추가) */
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT = NULL,
    @p_feature_cd VARCHAR(20) = NULL,
    @p_use_yn VARCHAR(1) = NULL,
    @p_option_val VARCHAR(100) = NULL,
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
        IF @p_work_type = 'SET'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_ID = @p_menu_id)
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'메뉴를 찾을 수 없습니다.'; RETURN; END
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'SM0012' AND minor_cd = @p_feature_cd AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'알 수 없는 화면 기능입니다.'; RETURN; END

            SET @p_use_yn = CASE WHEN @p_use_yn = 'Y' THEN 'Y' ELSE 'N' END;
            SET @p_option_val = NULLIF(LTRIM(RTRIM(@p_option_val)), '');
            -- 옵션은 폴더명/프로시저명 조합에 쓰이므로 영문/숫자/밑줄만 허용
            IF @p_option_val IS NOT NULL AND @p_option_val LIKE '%[^A-Za-z0-9_]%'
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'옵션 값은 영문/숫자/밑줄만 쓸 수 있습니다.'; RETURN; END
            IF @p_feature_cd = 'APPROVAL' AND @p_use_yn = 'Y' AND @p_option_val IS NULL
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'전자결재를 사용하려면 결재 문서유형(doc_type)을 지정해야 합니다.'; RETURN; END

            UPDATE TSMMENUFEATURE SET use_yn = @p_use_yn, option_val = @p_option_val,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE menu_id = @p_menu_id AND feature_cd = @p_feature_cd;
            IF @@ROWCOUNT = 0
                INSERT INTO TSMMENUFEATURE (menu_id, feature_cd, use_yn, option_val, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@p_menu_id, @p_feature_cd, @p_use_yn, @p_option_val, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER FUNCTION dbo.FN_AP_IS_LOCKED (@app_id BIGINT)
RETURNS BIT
AS
BEGIN
    RETURN CASE WHEN @app_id IS NOT NULL
                 AND EXISTS (SELECT 1 FROM TAPDOC WHERE app_id = @app_id AND app_stat_cd IN ('0', '1', 'E'))
                THEN 1 ELSE 0 END;
END
GO

IF COL_LENGTH('TMAOPENM', 'app_id') IS NULL ALTER TABLE TMAOPENM ADD app_id BIGINT NULL, app_no VARCHAR(20) NULL;
GO
IF COL_LENGTH('TMAETCOUTM', 'app_id') IS NULL ALTER TABLE TMAETCOUTM ADD app_id BIGINT NULL, app_no VARCHAR(20) NULL;
GO
IF COL_LENGTH('TMAETCINM', 'app_id') IS NULL ALTER TABLE TMAETCINM ADD app_id BIGINT NULL, app_no VARCHAR(20) NULL;
GO
CREATE OR ALTER PROCEDURE USP_MA_ETCIN_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_in_id BIGINT = NULL,
    @p_in_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = in_id
            FROM TMAETCINM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_in_id IS NULL OR in_id = @p_in_id)
              AND (@p_in_id IS NOT NULL OR @p_in_no IS NULL OR in_no LIKE '%' + @p_in_no + '%')
            ORDER BY in_id DESC;

            SELECT m.in_id, m.acc_id, a.ACC_NM, m.in_no, m.in_date, m.trans_type, tt.minor_nm AS trans_type_nm, m.stat_cd,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMAETCINM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
                LEFT JOIN TSMMINOR tt ON tt.major_cd = 'MA0011' AND tt.minor_cd = m.trans_type
            WHERE m.in_id = @match_id;

            SELECT dt.in_id, dt.serl, dt.acc_id, dt.in_no, dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                   dt.in_qty, dt.lot_no, dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn, dt.trans_id, dt.remark
            FROM TMAETCIND dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.in_id = @match_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCIN_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_in_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_in_date VARCHAR(8) = NULL,
    @p_trans_type VARCHAR(10) = NULL,       /* 입고유형 - MA0011 중 기타수불(rel_cd2='Y') 입고 계열(rel_cd1='I'), 비우면 ETC_IN */
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10);
            SELECT @stat = stat_cd FROM TMAETCINM WHERE in_id = @p_in_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타입고 문서를 찾을 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타입고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
            IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAETCINM WHERE in_id = @p_in_id)) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'ETC_IN';
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'I' AND rel_cd2 = 'Y' AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'입고유형이 올바르지 않습니다(기초코드 MA0011에서 기타수불여부가 체크된 입고 계열만 선택할 수 있습니다).'; RETURN; END
        END
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCINM', 'in_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCINM (acc_id, in_no, in_date, trans_type, dept_id, emp_id, stat_cd, cfm_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_in_date, @p_trans_type, @p_dept_id, @p_emp_id, '0', 'N', @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_in_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCINM SET acc_id = @p_acc_id, in_date = @p_in_date, trans_type = @p_trans_type, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE in_id = @p_in_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAETCIND WHERE in_id = @p_in_id;
            DELETE FROM TMAETCINM WHERE in_id = @p_in_id;
        END

        SET @GeneratedCode = CAST(@p_in_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCIN_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_in_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_in_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        DECLARE @hdr_stat VARCHAR(10);
        SELECT @hdr_stat = stat_cd FROM TMAETCINM WHERE in_id = @p_in_id;
        IF @hdr_stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타입고 문서를 찾을 수 없습니다.'; RETURN; END
        IF @hdr_stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타입고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAETCINM WHERE in_id = @p_in_id)) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        DECLARE @unit VARCHAR(10), @stock VARCHAR(1);
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @p_in_qty IS NULL OR @p_in_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'입고수량은 0보다 커야 합니다.'; RETURN; END
            SELECT @unit = unit_cd, @stock = ISNULL(stock_yn, 'N') FROM TBAITEM WHERE item_id = @p_item_id;
            IF @stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAETCIND WHERE in_id = @p_in_id;

            INSERT INTO TMAETCIND (in_id, serl, acc_id, in_no, item_id, unit_cd, in_qty, lot_no, wh_id, loc_id, stock_yn, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.in_id, @next, m.acc_id, m.in_no, @p_item_id, @unit, @p_in_qty, @p_lot_no, @p_wh_id, @p_loc_id, @stock, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAETCINM m WHERE m.in_id = @p_in_id;
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCIND SET item_id = @p_item_id, unit_cd = @unit, in_qty = @p_in_qty, lot_no = @p_lot_no, wh_id = @p_wh_id, loc_id = @p_loc_id,
                                stock_yn = @stock, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE in_id = @p_in_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAETCIND WHERE in_id = @p_in_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_out_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = out_id
            FROM TMAETCOUTM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_out_id IS NULL OR out_id = @p_out_id)
              AND (@p_out_id IS NOT NULL OR @p_out_no IS NULL OR out_no LIKE '%' + @p_out_no + '%')
            ORDER BY out_id DESC;

            SELECT m.out_id, m.acc_id, a.ACC_NM, m.out_no, m.out_date, m.out_reason, m.trans_type, tt.minor_nm AS trans_type_nm, m.stat_cd,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMAETCOUTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
                LEFT JOIN TSMMINOR tt ON tt.major_cd = 'MA0011' AND tt.minor_cd = m.trans_type
            WHERE m.out_id = @match_id;

            SELECT dt.out_id, dt.serl, dt.acc_id, dt.out_no, dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                   dt.out_qty, dt.lot_no, dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn,
                   s.stock_qty,
                   dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                   rd.qty AS req_qty, (rd.qty - ISNULL(rd.next_qty, 0)) AS req_remain_qty,
                   dt.trans_id, dt.remark
            FROM TMAETCOUTD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TMASTOCK s ON s.acc_id = dt.acc_id AND s.item_id = dt.item_id AND s.wh_id = dt.wh_id
                                    AND s.loc_id = ISNULL(dt.loc_id, 0) AND s.lot_no = ISNULL(dt.lot_no, N'')
                LEFT JOIN TMAETCREQD rd ON dt.src_type = 'ETCREQ' AND rd.req_id = dt.src_id AND rd.serl = dt.src_serl
            WHERE dt.out_id = @match_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_out_date VARCHAR(8) = NULL,
    @p_out_reason VARCHAR(10) = NULL,
    @p_trans_type VARCHAR(10) = NULL,       /* 출고유형 - MA0011 중 출고 계열(rel_cd1='O'), 비우면 ETC_OUT */
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10);
            SELECT @stat = stat_cd FROM TMAETCOUTM WHERE out_id = @p_out_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고 문서를 찾을 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타출고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
            IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAETCOUTM WHERE out_id = @p_out_id)) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'ETC_OUT';
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'O' AND rel_cd2 = 'Y' AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고유형이 올바르지 않습니다(기초코드 MA0011에서 기타수불여부가 체크된 출고 계열만 선택할 수 있습니다).'; RETURN; END
        END
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCOUTM', 'out_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCOUTM (acc_id, out_no, out_date, out_reason, trans_type, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_out_date, @p_out_reason, @p_trans_type, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_out_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTM SET
                acc_id = @p_acc_id, out_date = @p_out_date, out_reason = @p_out_reason, trans_type = @p_trans_type, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE out_id = @p_out_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAETCOUTD WHERE out_id = @p_out_id;
            DELETE FROM TMAETCOUTM WHERE out_id = @p_out_id;
        END

        SET @GeneratedCode = CAST(@p_out_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_out_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_serl INT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        DECLARE @hdr_stat VARCHAR(10), @hdr_acc BIGINT;
        SELECT @hdr_stat = stat_cd, @hdr_acc = acc_id FROM TMAETCOUTM WHERE out_id = @p_out_id;
        IF @hdr_stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고 문서를 찾을 수 없습니다.'; RETURN; END
        IF @hdr_stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타출고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAETCOUTM WHERE out_id = @p_out_id)) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT;
        IF @p_work_type = 'N'
            SELECT @src_type = NULLIF(@p_src_type, ''), @src_id = @p_src_id, @src_serl = @p_src_serl;
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl FROM TMAETCOUTD WHERE out_id = @p_out_id AND serl = @p_serl;
            IF @@ROWCOUNT = 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고 품목을 찾을 수 없습니다.'; RETURN; END
        END

        DECLARE @item BIGINT = @p_item_id, @unit VARCHAR(10), @stock VARCHAR(1), @src_no VARCHAR(20), @remain NUMERIC(18,4);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_out_qty IS NULL OR @p_out_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고수량은 0보다 커야 합니다.'; RETURN; END

            IF @src_type = 'ETCREQ'
            BEGIN
                SELECT @item = d.item_id, @unit = d.unit_cd, @src_no = d.req_no, @remain = d.qty - ISNULL(d.next_qty, 0)
                FROM TMAETCREQD d JOIN TMAETCREQM m ON m.req_id = d.req_id
                WHERE d.req_id = @src_id AND d.serl = @src_serl AND m.stat_cd = 'C' AND m.acc_id = @hdr_acc
                  AND ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y';
                IF @item IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청 품목을 찾을 수 없습니다.'; RETURN; END
                IF @p_out_qty > @remain
                BEGIN
                    SET @ReturnCode = -1;
                    SET @ReturnMsg = N'출고수량이 요청 잔량(' + CAST(CAST(@remain AS FLOAT) AS NVARCHAR(30)) + N')을 초과했습니다. (' + @src_no + N')';
                    RETURN;
                END
            END
            ELSE IF @src_type IS NOT NULL
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'올바르지 않은 원천입니다.'; RETURN; END

            IF @item IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            SELECT @stock = ISNULL(stock_yn, 'N'), @unit = ISNULL(@unit, unit_cd) FROM TBAITEM WHERE item_id = @item;
            IF @stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END

            -- 재고관리 품목은 저장 때 현재고를 확인해서 부족하면 저장을 거부한다(같은 출고 문서의 다른 라인이 같은 재고(품목/창고/위치/LOT)를 쓰는 수량까지 합산).
            IF @stock = 'Y'
            BEGIN
                IF @p_wh_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'재고관리 품목은 출고 창고를 입력해야 현재고를 확인할 수 있습니다.'; RETURN; END

                DECLARE @cur_stock NUMERIC(18,4), @others NUMERIC(18,4);
                SELECT @cur_stock = ISNULL(SUM(stock_qty), 0) FROM TMASTOCK
                WHERE acc_id = @hdr_acc AND item_id = @item AND wh_id = @p_wh_id AND loc_id = ISNULL(@p_loc_id, 0) AND lot_no = ISNULL(@p_lot_no, N'');
                SELECT @others = ISNULL(SUM(out_qty), 0) FROM TMAETCOUTD
                WHERE out_id = @p_out_id AND item_id = @item AND wh_id = @p_wh_id AND ISNULL(loc_id, 0) = ISNULL(@p_loc_id, 0)
                  AND ISNULL(lot_no, N'') = ISNULL(@p_lot_no, N'') AND stock_yn = 'Y' AND serl <> ISNULL(@p_serl, 0);

                IF @p_out_qty + @others > @cur_stock
                BEGIN
                    SET @ReturnCode = -1;
                    SET @ReturnMsg = N'현재고가 부족합니다. 현재고 ' + CAST(CAST(@cur_stock AS FLOAT) AS NVARCHAR(30))
                                   + N' < 출고수량 ' + CAST(CAST(@p_out_qty + @others AS FLOAT) AS NVARCHAR(30)) + N' (같은 재고의 다른 품목 포함)';
                    RETURN;
                END
            END        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAETCOUTD WHERE out_id = @p_out_id;

            INSERT INTO TMAETCOUTD (out_id, serl, acc_id, out_no, item_id, unit_cd, out_qty, lot_no, wh_id, loc_id, stock_yn,
                                    src_type, src_id, src_no, src_serl, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.out_id, @next, m.acc_id, m.out_no, @item, @unit, @p_out_qty, @p_lot_no, @p_wh_id, @p_loc_id, @stock,
                   @src_type, @src_id, @src_no, @src_serl, @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAETCOUTM m WHERE m.out_id = @p_out_id;

            -- 헤더 출고유형이 기본값(비어 있거나 ETC_OUT)이고 요청에서 불러왔으면 요청의 출고유형으로 채운다
            IF @src_type = 'ETCREQ'
                UPDATE h SET trans_type = r.trans_type FROM TMAETCOUTM h, TMAETCREQM r
                WHERE h.out_id = @p_out_id AND r.req_id = @src_id AND ISNULL(h.trans_type, '') IN ('', 'ETC_OUT') AND ISNULL(r.trans_type, '') <> '';
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTD SET
                item_id = @item, unit_cd = @unit, out_qty = @p_out_qty, lot_no = @p_lot_no, wh_id = @p_wh_id, loc_id = @p_loc_id, stock_yn = @stock, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE out_id = @p_out_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAETCOUTD WHERE out_id = @p_out_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_Q
    @p_acc_id BIGINT = NULL,
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
    @p_open_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = open_id
            FROM TMAOPENM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_open_id IS NULL OR open_id = @p_open_id)
              AND (@p_open_id IS NOT NULL OR @p_open_no IS NULL OR open_no LIKE '%' + @p_open_no + '%')
            ORDER BY open_id DESC;

            SELECT m.open_id, m.acc_id, a.ACC_NM, m.open_no, m.open_date, m.stat_cd,
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd, m.remark
            FROM TMAOPENM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.open_id = @match_id;

            SELECT dt.open_id, dt.serl, dt.acc_id, dt.open_no, dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                   dt.qty, dt.lot_no, dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn, dt.trans_id, dt.remark
            FROM TMAOPEND dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.open_id = @match_id
            ORDER BY dt.serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_open_date VARCHAR(8) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10);
            SELECT @stat = stat_cd FROM TMAOPENM WHERE open_id = @p_open_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기초재고 문서를 찾을 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기초재고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
            IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAOPENM WHERE open_id = @p_open_id)) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAOPENM', 'open_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAOPENM (acc_id, open_no, open_date, dept_id, emp_id, stat_cd, cfm_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_open_date, @p_dept_id, @p_emp_id, '0', 'N', @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_open_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAOPENM SET acc_id = @p_acc_id, open_date = @p_open_date, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE open_id = @p_open_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAOPEND WHERE open_id = @p_open_id;
            DELETE FROM TMAOPENM WHERE open_id = @p_open_id;
        END

        SET @GeneratedCode = CAST(@p_open_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_OPEN_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_open_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        DECLARE @hdr_stat VARCHAR(10);
        SELECT @hdr_stat = stat_cd FROM TMAOPENM WHERE open_id = @p_open_id;
        IF @hdr_stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기초재고 문서를 찾을 수 없습니다.'; RETURN; END
        IF @hdr_stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기초재고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAOPENM WHERE open_id = @p_open_id)) = 1 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        DECLARE @unit VARCHAR(10), @stock VARCHAR(1);
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @p_qty IS NULL OR @p_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기초수량은 0보다 커야 합니다.'; RETURN; END
            SELECT @unit = unit_cd, @stock = ISNULL(stock_yn, 'N') FROM TBAITEM WHERE item_id = @p_item_id;
            IF @stock IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 찾을 수 없습니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAOPEND WHERE open_id = @p_open_id;

            INSERT INTO TMAOPEND (open_id, serl, acc_id, open_no, item_id, unit_cd, qty, lot_no, wh_id, loc_id, stock_yn, remark,
                                  reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.open_id, @next, m.acc_id, m.open_no, @p_item_id, @unit, @p_qty, @p_lot_no, @p_wh_id, @p_loc_id, @stock, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAOPENM m WHERE m.open_id = @p_open_id;
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAOPEND SET item_id = @p_item_id, unit_cd = @unit, qty = @p_qty, lot_no = @p_lot_no, wh_id = @p_wh_id, loc_id = @p_loc_id,
                                stock_yn = @stock, remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE open_id = @p_open_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAOPEND WHERE open_id = @p_open_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 전자결재 후처리 - 기타입고(ETCIN). 상신: 결재번호 연결(결재자가 본인뿐이면 즉시 확정), 최종승인: 문서 확정(수불/재고 반영), 승인취소: 확정취소, 기안자 취소: 연결 해제.
CREATE OR ALTER PROCEDURE USP_AP_APPR_S_ETCIN
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_event = 'SUBMIT'
    BEGIN
        UPDATE TMAETCINM SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id) WHERE in_id = @p_doc_id;
        IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C' AND stat_cd <> 'E')
            SET @p_event = 'APPROVE_END';
    END

    IF @p_event = 'APPROVE_END'
    BEGIN
        -- 이미 확정된 문서(상신 즉시 확정 뒤에 최종승인 이벤트가 또 오는 경우)는 건너뛴다.
        IF EXISTS (SELECT 1 FROM TMAETCINM WHERE in_id = @p_doc_id AND stat_cd = '0')
            EXEC USP_MA_ETCIN_CONFIRM_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'UNDO_END'
    BEGIN
        IF EXISTS (SELECT 1 FROM TMAETCINM WHERE in_id = @p_doc_id AND stat_cd = 'C')
            EXEC USP_MA_ETCIN_CANCEL_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'RESET'
        UPDATE TMAETCINM SET app_id = NULL, app_no = NULL WHERE in_id = @p_doc_id;
    -- 'REJECT'(반려)는 문서 상태를 바꾸지 않는다(결재가 반려로 표시되고 기안자가 고쳐서 다시 상신).
END
GO

-- 전자결재 후처리 - 기타출고(ETCOUT). 상신: 결재번호 연결(결재자가 본인뿐이면 즉시 확정), 최종승인: 문서 확정(수불/재고 반영), 승인취소: 확정취소, 기안자 취소: 연결 해제.
CREATE OR ALTER PROCEDURE USP_AP_APPR_S_ETCOUT
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_event = 'SUBMIT'
    BEGIN
        UPDATE TMAETCOUTM SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id) WHERE out_id = @p_doc_id;
        IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C' AND stat_cd <> 'E')
            SET @p_event = 'APPROVE_END';
    END

    IF @p_event = 'APPROVE_END'
    BEGIN
        -- 이미 확정된 문서(상신 즉시 확정 뒤에 최종승인 이벤트가 또 오는 경우)는 건너뛴다.
        IF EXISTS (SELECT 1 FROM TMAETCOUTM WHERE out_id = @p_doc_id AND stat_cd = '0')
            EXEC USP_MA_ETCOUT_CONFIRM_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'UNDO_END'
    BEGIN
        IF EXISTS (SELECT 1 FROM TMAETCOUTM WHERE out_id = @p_doc_id AND stat_cd = 'C')
            EXEC USP_MA_ETCOUT_CANCEL_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'RESET'
        UPDATE TMAETCOUTM SET app_id = NULL, app_no = NULL WHERE out_id = @p_doc_id;
    -- 'REJECT'(반려)는 문서 상태를 바꾸지 않는다(결재가 반려로 표시되고 기안자가 고쳐서 다시 상신).
END
GO

-- 전자결재 후처리 - 기초재고(OPENSTK). 상신: 결재번호 연결(결재자가 본인뿐이면 즉시 확정), 최종승인: 문서 확정(수불/재고 반영), 승인취소: 확정취소, 기안자 취소: 연결 해제.
CREATE OR ALTER PROCEDURE USP_AP_APPR_S_OPENSTK
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_event = 'SUBMIT'
    BEGIN
        UPDATE TMAOPENM SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id) WHERE open_id = @p_doc_id;
        IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'C' AND stat_cd <> 'E')
            SET @p_event = 'APPROVE_END';
    END

    IF @p_event = 'APPROVE_END'
    BEGIN
        -- 이미 확정된 문서(상신 즉시 확정 뒤에 최종승인 이벤트가 또 오는 경우)는 건너뛴다.
        IF EXISTS (SELECT 1 FROM TMAOPENM WHERE open_id = @p_doc_id AND stat_cd = '0')
            EXEC USP_MA_OPEN_CONFIRM_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'UNDO_END'
    BEGIN
        IF EXISTS (SELECT 1 FROM TMAOPENM WHERE open_id = @p_doc_id AND stat_cd = 'C')
            EXEC USP_MA_OPEN_CANCEL_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'RESET'
        UPDATE TMAOPENM SET app_id = NULL, app_no = NULL WHERE open_id = @p_doc_id;
    -- 'REJECT'(반려)는 문서 상태를 바꾸지 않는다(결재가 반려로 표시되고 기안자가 고쳐서 다시 상신).
END
GO

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'AP0002', v.cd, v.nm, (SELECT ISNULL(MAX(sort), 0) FROM TSMMINOR WHERE major_cd = 'AP0002') + v.n, 'N', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('ETCIN', N'기타입고서', 1), ('ETCOUT', N'기타출고서', 2), ('OPENSTK', N'기초재고서', 3)) v(cd, nm, n)
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR x WHERE x.major_cd = 'AP0002' AND x.minor_cd = v.cd);
GO