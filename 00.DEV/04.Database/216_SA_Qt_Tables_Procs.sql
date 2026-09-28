-- 영업관리 P1 - 견적(TSAQTM/TSAQTD) 테이블 + 프로시저(2026-09-28).
-- TMAPOM/TMAPOD(167/171)과 같은 Master-One Sheet 구조지만 견적은:
--   * 결재 없음(app_id/app_no 없음) - 수주 확정 때만 결재.
--   * 검사/재고 관련 컬럼 없음(qc_yn/stock_yn/wh_id/loc_id) - 아직 창고에 닿는 단계가 아님.
--   * 원천(src_type) 없음 - 견적이 영업 프로세스의 첫 문서.
--   * cfm_yn(견적확정)로 잠근다 - 확정된 견적만 수주에서 불러올 수 있음(popPick 대상, 172번 식).
--   * next_qty(수주로 전환된 수량)는 나중에 수주 저장 프로시저가 재계산한다(USP_MA_NEXTQTY_R와 같은 패턴,
--     지금은 컬럼만 만들어두고 재계산 프로시저는 수주 단계에서 추가).

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSAQTM')
BEGIN
    CREATE TABLE TSAQTM (
        qt_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id        BIGINT         NOT NULL,
        qt_no         VARCHAR(20)    NOT NULL,
        qt_date       VARCHAR(8)     NULL,
        valid_date    VARCHAR(8)     NULL,                  -- 견적 유효기한
        cfm_yn        VARCHAR(1)     NULL,
        cfm_dt        DATETIME       NULL,
        cfm_user_id   VARCHAR(30)    NULL,
        stat_cd       VARCHAR(10)    NULL,
        qt_title      NVARCHAR(1000) NULL,
        cust_id       BIGINT         NULL,
        dept_id       BIGINT         NULL,
        emp_id        BIGINT         NULL,
        pjt_id        BIGINT         NULL,
        cur_cd        VARCHAR(10)    NULL,
        exc_rate      NUMERIC(18,4)  NULL,
        vat_type      VARCHAR(10)    NULL,
        vat_rate      NUMERIC(9,4)   NULL,
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAQTM PRIMARY KEY CLUSTERED (qt_id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSAQTD')
BEGIN
    CREATE TABLE TSAQTD (
        qt_id           BIGINT         NOT NULL,
        serl            INT            NOT NULL,
        acc_id          BIGINT         NOT NULL,
        qt_no           VARCHAR(20)    NOT NULL,
        item_id         BIGINT         NOT NULL,
        unit_cd         VARCHAR(10)    NULL,
        qty             NUMERIC(18,4)  NULL,
        next_qty        NUMERIC(18,4)  NULL,                -- 수주로 전환된 수량(수주 저장 시 재계산)
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
        delv_date       VARCHAR(8)     NULL,                -- 희망납기
        pjt_id          BIGINT         NULL,
        remark          NVARCHAR(3000) NULL,
        reg_user_id     VARCHAR(30)    NULL,
        reg_dt          DATETIME       NULL,
        reg_pc          NVARCHAR(200)  NULL,
        upt_user_id     VARCHAR(30)    NULL,
        upt_dt          DATETIME       NULL,
        upt_pc          NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAQTD PRIMARY KEY CLUSTERED (qt_id, serl)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TSAQTM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TSAQTM', N'견적마스터', 'QT', 'qt_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO

-- ============================================================
-- USP_SA_QT_Q - frmQt 전용.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_QT_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_qt_id BIGINT = NULL,
    @p_qt_no VARCHAR(20) = NULL,
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
            DECLARE @match_qt_id BIGINT;
            SELECT TOP 1 @match_qt_id = qt_id
            FROM TSAQTM
            WHERE (@p_qt_id IS NULL OR qt_id = @p_qt_id)
              AND (@p_qt_id IS NOT NULL OR @p_qt_no IS NULL OR qt_no LIKE '%' + @p_qt_no + '%')
            ORDER BY qt_id DESC;

            SELECT
                m.qt_id, m.acc_id, a.ACC_NM,
                m.qt_no, m.qt_date, m.valid_date,
                m.stat_cd, m.qt_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.remark
            FROM TSAQTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.qt_id = @match_qt_id;

            SELECT
                dt.qt_id, dt.serl, dt.acc_id, dt.qt_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.pjt_id, dt.remark
            FROM TSAQTD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
            WHERE dt.qt_id = @match_qt_id
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
-- USP_SA_QT_S - 헤더(TSAQTM) N/U/D + 확정/확정취소(C/CC). 확정된 견적은 수정/삭제 금지.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_QT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_qt_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_qt_date VARCHAR(8) = NULL,
    @p_valid_date VARCHAR(8) = NULL,
    @p_qt_title NVARCHAR(1000) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(10) = NULL,
    @p_exc_rate NUMERIC(18,4) = NULL,
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
        IF @p_work_type IN ('N', 'U', 'D')
           AND EXISTS (SELECT 1 FROM TSAQTM WHERE qt_id = @p_qt_id AND cfm_yn = 'Y')
        BEGIN
            SET @ReturnCode = -1;
            SET @ReturnMsg = N'확정된 견적은 수정/삭제할 수 없습니다. 먼저 확정을 취소해주세요.';
            RETURN;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_qt_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSAQTM', 'qt_no', @p_acc_id, @new_qt_no OUTPUT;

            INSERT INTO TSAQTM (
                acc_id, qt_no, qt_date, valid_date, stat_cd, qt_title,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate,
                vat_type, vat_rate, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @new_qt_no, @p_qt_date, @p_valid_date, '0', @p_qt_title,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate,
                @p_vat_type, @p_vat_rate, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_qt_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSAQTM SET
                acc_id = @p_acc_id,
                qt_date = @p_qt_date,
                valid_date = @p_valid_date,
                qt_title = @p_qt_title,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                exc_rate = @p_exc_rate,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE qt_id = @p_qt_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSAQTD WHERE qt_id = @p_qt_id;
            DELETE FROM TSAQTM WHERE qt_id = @p_qt_id;
        END
        ELSE IF @p_work_type = 'C'
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSAQTD WHERE qt_id = @p_qt_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'품목이 없는 견적은 확정할 수 없습니다.'; RETURN;
            END
            UPDATE TSAQTM SET cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, stat_cd = 'C'
            WHERE qt_id = @p_qt_id;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF EXISTS (SELECT 1 FROM TSAQTD WHERE qt_id = @p_qt_id AND next_qty > 0)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 수주로 진행된 품목이 있어 확정을 취소할 수 없습니다.'; RETURN;
            END
            UPDATE TSAQTM SET cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, stat_cd = '0'
            WHERE qt_id = @p_qt_id;
        END

        SET @GeneratedCode = CAST(@p_qt_id AS VARCHAR(20));
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
-- USP_SA_QT_S_1 - 품목(TSAQTD) 행별 N/U/D. amt/vat/total_amt는 화면에서 계산해 그대로 저장.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_QT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_qt_id BIGINT = NULL,
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
    @p_pjt_id BIGINT = NULL,
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
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TSAQTD WHERE qt_id = @p_qt_id;

            INSERT INTO TSAQTD (
                qt_id, serl, acc_id, qt_no, item_id, unit_cd, qty, next_qty,
                price, amt, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                vat_type, vat_rate, delv_date, pjt_id, remark,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT m.qt_id, @nextSerl, m.acc_id, m.qt_no, @p_item_id, @p_unit_cd, @p_qty, @p_next_qty,
                   @p_price, @p_amt, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
                   @p_vat_type, @p_vat_rate, @p_delv_date, @p_pjt_id, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TSAQTM m WHERE m.qt_id = @p_qt_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSAQTD SET
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
                pjt_id = @p_pjt_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE qt_id = @p_qt_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSAQTD WHERE qt_id = @p_qt_id AND serl = @p_serl;
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

-- 메뉴 골격은 아직 견적 항목이 없으므로 영업기준관리(17) 하위에 새로 추가한다.
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SA' AND SCREEN_CLASS_NM = 'frmQt')
INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
VALUES (N'견적등록', 17, 3, 'FORM', 'SA', 'frmQt', 'USP_SA_', 5, 'Y', SUSER_SNAME(), GETDATE());
GO
