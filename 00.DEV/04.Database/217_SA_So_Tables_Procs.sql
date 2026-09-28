-- 영업관리 P2 - 수주(TSASOM/TSASOD) 테이블 + 프로시저 + 결재연동(2026-09-28).
-- TMAPOM/TMAPOD(167/171)과 완전히 같은 구조(qc_yn만 뺌 - 수주/출하에는 검사 단계가 없다는 정책).
-- 결재(전자결재)는 수주 확정 때만 필요 - 기존 USP_AP_APPR_DOC 디스패처에 'SO' doc_type만 추가한다
-- (209번 마이그레이션 패턴 그대로, USP_AP_APPR_S 본체는 손대지 않음).

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSASOM')
BEGIN
    CREATE TABLE TSASOM (
        so_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id        BIGINT         NOT NULL,
        so_no         VARCHAR(20)    NOT NULL,
        so_date       VARCHAR(8)     NULL,
        cfm_yn        VARCHAR(1)     NULL,
        cfm_dt        DATETIME       NULL,
        cfm_user_id   VARCHAR(30)    NULL,
        stop_yn       VARCHAR(1)     NULL,
        stop_dt       DATETIME       NULL,
        stop_user_id  VARCHAR(30)    NULL,
        stat_cd       VARCHAR(10)    NULL,
        dept_id       BIGINT         NULL,
        emp_id        BIGINT         NULL,
        so_title      NVARCHAR(1000) NULL,
        pjt_id        BIGINT         NULL,
        cust_id       BIGINT         NULL,
        cur_cd        VARCHAR(10)    NULL,
        exc_rate      NUMERIC(18,4)  NULL,
        delv_date     VARCHAR(8)     NULL,
        vat_type      VARCHAR(10)    NULL,
        vat_rate      NUMERIC(9,4)   NULL,
        app_id        BIGINT         NULL,
        app_no        VARCHAR(20)    NULL,
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSASOM PRIMARY KEY CLUSTERED (so_id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSASOD')
BEGIN
    CREATE TABLE TSASOD (
        so_id           BIGINT         NOT NULL,
        serl            INT            NOT NULL,
        acc_id          BIGINT         NOT NULL,
        so_no           VARCHAR(20)    NOT NULL,
        item_id         BIGINT         NOT NULL,
        unit_cd         VARCHAR(10)    NULL,
        qty             NUMERIC(18,4)  NULL,
        next_qty        NUMERIC(18,4)  NULL,               -- 출하로 전환된 수량(출하 확정 시 재계산)
        price           NUMERIC(18,4)  NULL,
        amt             NUMERIC(18,4)  NULL,
        vat             NUMERIC(18,4)  NULL,
        total_amt       NUMERIC(18,4)  NULL,
        kor_price       NUMERIC(18,4)  NULL,
        kor_amt         NUMERIC(18,4)  NULL,
        kor_vat         NUMERIC(18,4)  NULL,
        kor_total_amt   NUMERIC(18,4)  NULL,
        vat_type        VARCHAR(10)    NULL,
        vat_rate        NUMERIC(9,4)   NULL,
        delv_date       VARCHAR(8)     NULL,
        wh_id           BIGINT         NULL,
        loc_id          BIGINT         NULL,
        stock_yn        VARCHAR(1)     NULL,                -- 재고관리 대상 여부(TBAITEM.stock_yn 스냅샷)
        stock_unit_cd   VARCHAR(10)    NULL,
        stock_unit_qty  NUMERIC(18,4)  NULL,
        pjt_id          BIGINT         NULL,
        src_type        VARCHAR(10)    NULL,                -- 'QT'(견적)
        src_id          BIGINT         NULL,
        src_no          VARCHAR(20)    NULL,
        src_serl        INT            NULL,
        stop_yn         VARCHAR(1)     NOT NULL DEFAULT ('N'),
        stop_emp_no     VARCHAR(20)    NULL,
        stop_remark     NVARCHAR(1000) NULL,
        remark          NVARCHAR(3000) NULL,
        reg_user_id     VARCHAR(30)    NULL,
        reg_dt          DATETIME       NULL,
        reg_pc          NVARCHAR(200)  NULL,
        upt_user_id     VARCHAR(30)    NULL,
        upt_dt          DATETIME       NULL,
        upt_pc          NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSASOD PRIMARY KEY CLUSTERED (so_id, serl)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TSASOM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TSASOM', N'수주마스터', 'SO', 'so_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 결재문서유형 AP0002에 'SO' 추가 (168번 마이그레이션과 같은 패턴)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'AP0002' AND minor_cd = 'SO')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('AP0002', 'SO', N'수주서', 4, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- USP_AP_APPR_S_SO - USP_AP_APPR_DOC 디스패처가 doc_type='SO'일 때 호출(USP_AP_APPR_S_PO와 완전히 동일한 모양,
-- TMAPOM/po_id 대신 TSASOM/so_id). USP_AP_APPR_S 본체는 건드리지 않는다([[project_wynlab_approval_doctype_dispatch]]).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_AP_APPR_S_SO
    @p_event VARCHAR(20), @p_doc_id BIGINT, @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL, @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @p_event = 'SUBMIT'
        UPDATE TSASOM SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
         WHERE so_id = @p_doc_id;
    ELSE IF @p_event = 'APPROVE_END'
        UPDATE TSASOM SET stat_cd = 'C' WHERE so_id = @p_doc_id;
    ELSE IF @p_event = 'UNDO_END'
        UPDATE TSASOM SET stat_cd = '0' WHERE so_id = @p_doc_id;
    ELSE IF @p_event = 'RESET'
        UPDATE TSASOM SET app_id = NULL, app_no = NULL, stat_cd = '0' WHERE so_id = @p_doc_id;
END
GO

-- ============================================================
-- USP_SA_SO_Q - frmSo 전용.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SO_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_so_no VARCHAR(20) = NULL,
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
            DECLARE @match_so_id BIGINT;
            SELECT TOP 1 @match_so_id = so_id
            FROM TSASOM
            WHERE (@p_so_id IS NULL OR so_id = @p_so_id)
              AND (@p_so_id IS NOT NULL OR @p_so_no IS NULL OR so_no LIKE '%' + @p_so_no + '%')
            ORDER BY so_id DESC;

            SELECT
                m.so_id, m.acc_id, a.ACC_NM,
                m.so_no, m.so_date,
                m.stat_cd, m.so_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.delv_date, m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.remark
            FROM TSASOM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.so_id = @match_so_id;

            SELECT
                dt.so_id, dt.serl, dt.acc_id, dt.so_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.stock_yn, dt.stock_unit_cd, dt.stock_unit_qty,
                dt.pjt_id, dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.stop_yn, dt.stop_emp_no, dt.stop_remark, dt.remark
            FROM TSASOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.so_id = @match_so_id
            ORDER BY dt.serl;
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
-- USP_SA_SO_S - 헤더(TSASOM) N/U/D. 결재상신된 건 삭제 금지(PO와 동일).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SO_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_so_date VARCHAR(8) = NULL,
    @p_so_title NVARCHAR(1000) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(10) = NULL,
    @p_exc_rate NUMERIC(18,4) = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(9,4) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_so_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSASOM', 'so_no', @p_acc_id, @new_so_no OUTPUT;

            INSERT INTO TSASOM (
                acc_id, so_no, so_date, stat_cd, so_title,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate,
                delv_date, vat_type, vat_rate, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @new_so_no, @p_so_date, '0', @p_so_title,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate,
                @p_delv_date, @p_vat_type, @p_vat_rate, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_so_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSASOM SET
                acc_id = @p_acc_id,
                so_date = @p_so_date,
                so_title = @p_so_title,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                exc_rate = @p_exc_rate,
                delv_date = @p_delv_date,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE so_id = @p_so_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TSASOM WHERE so_id = @p_so_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 수주는 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TSASOD WHERE so_id = @p_so_id;
            DELETE FROM TSASOM WHERE so_id = @p_so_id;
        END

        SET @GeneratedCode = CAST(@p_so_id AS VARCHAR(20));
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
-- USP_SA_SO_S_1 - 품목(TSASOD) 행별 N/U/D. 견적에서 불러온 행(src_type='QT')은 저장할 때마다
-- 원본 견적 라인(TSAQTD)의 next_qty를 이 수주에 걸린 수량 합계로 다시 계산한다(구매요청의
-- USP_MA_NEXTQTY_R와 같은 목적이지만, 지금은 QT->SO 관계 하나뿐이라 별도 범용 프로시저 없이
-- 이 프로시저 안에서 바로 계산한다 - 관계가 하나 더 늘면 그때 공용화한다).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SO_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,
    @p_price NUMERIC(18,4) = NULL,
    @p_amt NUMERIC(18,4) = NULL,
    @p_vat NUMERIC(18,4) = NULL,
    @p_total_amt NUMERIC(18,4) = NULL,
    @p_kor_price NUMERIC(18,4) = NULL,
    @p_kor_amt NUMERIC(18,4) = NULL,
    @p_kor_vat NUMERIC(18,4) = NULL,
    @p_kor_total_amt NUMERIC(18,4) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(9,4) = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_stock_yn VARCHAR(1) = NULL,
    @p_stock_unit_cd VARCHAR(10) = NULL,
    @p_stock_unit_qty NUMERIC(18,4) = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_no VARCHAR(20) = NULL,
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
    DECLARE @v_src_type VARCHAR(10), @v_src_id BIGINT, @v_src_serl INT;
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TSASOD WHERE so_id = @p_so_id;

            INSERT INTO TSASOD (
                so_id, serl, acc_id, so_no, item_id, unit_cd, qty, next_qty,
                price, amt, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                vat_type, vat_rate, delv_date, wh_id, loc_id,
                stock_yn, stock_unit_cd, stock_unit_qty, pjt_id,
                src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT m.so_id, @nextSerl, m.acc_id, m.so_no, @p_item_id, @p_unit_cd, @p_qty, @p_next_qty,
                   @p_price, @p_amt, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
                   @p_vat_type, @p_vat_rate, @p_delv_date, @p_wh_id, @p_loc_id,
                   @p_stock_yn, @p_stock_unit_cd, @p_stock_unit_qty, @p_pjt_id,
                   @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TSASOM m WHERE m.so_id = @p_so_id;

            SET @p_serl = @nextSerl;
            SET @v_src_type = @p_src_type; SET @v_src_id = @p_src_id; SET @v_src_serl = @p_src_serl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @v_src_type = src_type, @v_src_id = src_id, @v_src_serl = src_serl
            FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_serl;

            UPDATE TSASOD SET
                item_id = @p_item_id,
                unit_cd = @p_unit_cd,
                qty = @p_qty,
                price = @p_price,
                amt = @p_amt,
                vat = @p_vat,
                total_amt = @p_total_amt,
                kor_price = @p_kor_price,
                kor_amt = @p_kor_amt,
                kor_vat = @p_kor_vat,
                kor_total_amt = @p_kor_total_amt,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                delv_date = @p_delv_date,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                stock_yn = @p_stock_yn,
                stock_unit_cd = @p_stock_unit_cd,
                stock_unit_qty = @p_stock_unit_qty,
                pjt_id = @p_pjt_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE so_id = @p_so_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            SELECT @v_src_type = src_type, @v_src_id = src_id, @v_src_serl = src_serl
            FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_serl;

            DELETE FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_serl;
        END

        -- 견적에서 불러온 라인이면 원본 견적 라인의 next_qty를 다시 계산(이 견적라인을 참조하는
        -- 모든 수주라인의 qty 합계) - 삭제/수정 후에도 항상 최신 값으로 맞춘다.
        IF @v_src_type = 'QT' AND @v_src_id IS NOT NULL
        BEGIN
            UPDATE TSAQTD
               SET next_qty = (
                   SELECT ISNULL(SUM(d.qty), 0) FROM TSASOD d
                   WHERE d.src_type = 'QT' AND d.src_id = @v_src_id AND d.src_serl = @v_src_serl
               )
             WHERE qt_id = @v_src_id AND serl = @v_src_serl;
        END

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
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
-- USP_SA_QTPICK_Q - 수주등록의 "견적 불러오기" 팝업(popQtPick) 전용. 확정된(cfm_yn='Y') 견적 중
-- 잔량(qty - next_qty > 0)이 남은 라인만 보여준다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_QTPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_qt_no VARCHAR(20) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,
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
                m.qt_id, m.qt_no, m.qt_date, m.qt_title,
                m.cust_id, c.cust_nm,
                d.serl AS qt_serl, d.item_id, i.item_no, i.item_nm, i.item_spec,
                d.unit_cd, d.qty, d.next_qty, (d.qty - ISNULL(d.next_qty, 0)) AS remain_qty,
                d.delv_date
            FROM TSAQTM m
                JOIN TSAQTD d ON d.qt_id = m.qt_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE m.cfm_yn = 'Y'
              AND (d.qty - ISNULL(d.next_qty, 0)) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR m.qt_date >= @p_date_from)
              AND (@p_date_to IS NULL OR m.qt_date <= @p_date_to)
              AND (@p_qt_no IS NULL OR @p_qt_no = N'' OR m.qt_no LIKE '%' + @p_qt_no + '%')
              AND (@p_cust_id IS NULL OR m.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = N''
                   OR i.item_no LIKE N'%' + @p_keyword + N'%'
                   OR i.item_nm LIKE N'%' + @p_keyword + N'%'
                   OR i.item_spec LIKE N'%' + @p_keyword + N'%')
            ORDER BY m.qt_id DESC, d.serl;
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

-- 메뉴 골격의 수주등록(MENU_ID=20)/수주현황(21) 자리를 채운다 - 수주등록만 이번에 만든다(현황은 나중).
UPDATE TSMMENU
SET MODULE = 'SA', SCREEN_CLASS_NM = 'frmSo', PROC_PREFIX = 'USP_SA_',
    upt_user_id = SUSER_SNAME(), upt_dt = GETDATE()
WHERE MENU_ID = 20 AND MENU_NM = N'수주등록';
GO
