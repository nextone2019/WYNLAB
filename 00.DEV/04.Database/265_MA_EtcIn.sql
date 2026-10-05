-- 기타입고 (TMAETCINM/D) 테이블 + 프로시저 + 메뉴 (2026-10-03, WYNLAB_DEV 전용)
--  - 기타입고: 작성 -> 확정. 확정 = 수불(TMATRANS, 입고계열, 헤더의 입고유형)을 만들고 재고관리 품목만 현재고를 올린다. 확정취소는 역거래 + 재고 차감(재고 부족하면 불가).
--  - 입고유형 = MA0011 중 입고 계열(rel_cd1='I') & 기타수불여부(rel_cd2='Y'). 비우면 ETC_IN. 금액/단가는 다루지 않는다(재고금액평가에서 처리). 입고사유는 비고로 대체.
--  - 헤더의 app_id/app_no는 "메뉴에서 결재 사용을 지정"하는 공통 결재 연동(설계 검토 중)이 붙을 자리 - 지금은 쓰지 않는다.
--  - 구조/프로시저는 기초재고(TMAOPENM/D, USP_MA_OPEN_*, 259번)와 같고 입고유형만 더했다. 채번 EI. 여러 번 실행해도 안전하다.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAETCINM')
BEGIN
    CREATE TABLE TMAETCINM (
        in_id          BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        in_no          VARCHAR(20)    NOT NULL,
        in_date        VARCHAR(8)     NULL,                 -- 입고일(수불일자)
        trans_type     VARCHAR(10)    NULL,                 -- 입고유형 MA0011(입고 계열, 기타수불여부 Y)
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        stat_cd        VARCHAR(10)    NULL,                 -- MA0012 (0 작성, C 확정)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(30)    NULL,
        app_id         BIGINT         NULL,                 -- 전자결재 연결(TAPDOC) - 예약
        app_no         VARCHAR(20)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAETCINM PRIMARY KEY CLUSTERED (in_id)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAETCINM_no ON TMAETCINM (acc_id, in_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAETCIND')
BEGIN
    CREATE TABLE TMAETCIND (
        in_id          BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        in_no          VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        in_qty         NUMERIC(18,4)  NOT NULL,             -- 입고수량
        lot_no         NVARCHAR(50)   NULL,
        wh_id          BIGINT         NULL,                 -- 확정할 때 필수
        loc_id         BIGINT         NULL,
        stock_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMAETCIND_stock DEFAULT ('N'),   -- 현재고 갱신 여부 스냅샷
        trans_id       BIGINT         NULL,                 -- 확정으로 만들어진 수불(TMATRANS)
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAETCIND PRIMARY KEY CLUSTERED (in_id, serl),
        CONSTRAINT CK_TMAETCIND_qty CHECK (in_qty > 0),
        CONSTRAINT CK_TMAETCIND_stock CHECK (stock_yn IN ('Y', 'N'))
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAETCINM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAETCINM', N'기타입고마스터', 'EI', 'in_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'ETCIN')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0004', 'ETCIN', N'기타입고', (SELECT ISNULL(MAX(sort), 0) + 1 FROM TSMMINOR WHERE major_cd = 'MA0004'), 'Y', 'Y', 'SYSTEM', GETDATE());
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
                   m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TMAETCINM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
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

CREATE OR ALTER PROCEDURE USP_MA_ETCIN_CONFIRM_CORE
    @p_in_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @in_no VARCHAR(20), @acc BIGINT, @in_date VARCHAR(8), @type VARCHAR(10);
    SELECT @stat = stat_cd, @in_no = in_no, @acc = acc_id, @in_date = in_date, @type = ISNULL(NULLIF(trans_type, ''), 'ETC_IN') FROM TMAETCINM WHERE in_id = @p_in_id;

    IF @stat IS NULL THROW 50001, N'기타입고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'이미 확정된 기타입고입니다.', 1;
    IF NOT EXISTS (SELECT 1 FROM TMAETCIND WHERE in_id = @p_in_id) THROW 50001, N'기타입고 품목이 없어 확정할 수 없습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAETCIND WHERE in_id = @p_in_id AND wh_id IS NULL) THROW 50001, N'창고를 입력하지 않은 품목이 있습니다.', 1;
    IF EXISTS (SELECT 1 FROM TMAETCIND d JOIN TBAITEM i ON i.item_id = d.item_id
               WHERE d.in_id = @p_in_id AND i.lot_yn = 'Y' AND ISNULL(d.lot_no, N'') = N'')
        THROW 50001, N'LOT 관리 품목의 LOT를 입력하지 않은 품목이 있습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @tid BIGINT;
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, in_qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn FROM TMAETCIND WHERE in_id = @p_in_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, ISNULL(@in_date, CONVERT(VARCHAR(8), GETDATE(), 112)), 'I', @type, @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
                'ETCIN', @p_in_id, @in_no, @serl, NULL, NULL, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        UPDATE TMAETCIND SET trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE in_id = @p_in_id AND serl = @serl;

        IF @stock = 'Y'
        BEGIN
            UPDATE TMASTOCK WITH (UPDLOCK, HOLDLOCK) SET stock_qty = stock_qty + @qty, trans_id = @tid, unit_cd = ISNULL(@unit, unit_cd),
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF @@ROWCOUNT = 0
                INSERT INTO TMASTOCK (acc_id, item_id, wh_id, loc_id, lot_no, unit_cd, stock_qty, trans_id, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @item, @wh, @loc, @lot, @unit, @qty, @tid, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END

        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAETCINM SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE in_id = @p_in_id;

    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCIN_CANCEL_CORE
    @p_in_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @msg NVARCHAR(2048), @stat VARCHAR(10), @in_no VARCHAR(20), @acc BIGINT, @type VARCHAR(10);
    SELECT @stat = stat_cd, @in_no = in_no, @acc = acc_id, @type = ISNULL(NULLIF(trans_type, ''), 'ETC_IN') FROM TMAETCINM WHERE in_id = @p_in_id;
    IF @stat IS NULL THROW 50001, N'기타입고 문서를 찾을 수 없습니다.', 1;
    IF @stat <> 'C' THROW 50001, N'확정된 기타입고만 확정취소할 수 있습니다.', 1;

    BEGIN TRAN;

    DECLARE @serl INT, @item BIGINT, @unit VARCHAR(10), @qty NUMERIC(18,4), @lot NVARCHAR(50), @wh BIGINT, @loc BIGINT, @stock VARCHAR(1), @orig BIGINT,
            @tid BIGINT, @cur NUMERIC(18,4), @item_nm NVARCHAR(200);
    DECLARE c CURSOR LOCAL FAST_FORWARD FOR
        SELECT serl, item_id, unit_cd, in_qty, ISNULL(lot_no, N''), wh_id, ISNULL(loc_id, 0), stock_yn, trans_id FROM TMAETCIND WHERE in_id = @p_in_id ORDER BY serl;
    OPEN c;
    FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @stock = 'Y'
        BEGIN
            SELECT @cur = stock_qty FROM TMASTOCK WITH (UPDLOCK, HOLDLOCK) WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;
            IF ISNULL(@cur, 0) < @qty
            BEGIN
                SELECT @item_nm = item_nm FROM TBAITEM WHERE item_id = @item;
                SET @msg = N'현재고가 부족해 기타입고를 취소할 수 없습니다(이미 출고/이동된 재고). 품목: ' + ISNULL(@item_nm, CAST(@item AS NVARCHAR(20)))
                         + N', 현재고 ' + CAST(CAST(ISNULL(@cur, 0) AS FLOAT) AS NVARCHAR(30)) + N' < 취소수량 ' + CAST(CAST(@qty AS FLOAT) AS NVARCHAR(30));
                THROW 50001, @msg, 1;
            END
        END

        INSERT INTO TMATRANS (acc_id, trans_date, trans_kind, trans_type, item_id, wh_id, loc_id, lot_no, unit_cd, qty, stock_yn, cust_id,
                              src_type, src_id, src_no, src_serl, org_trans_id, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
        VALUES (@acc, CONVERT(VARCHAR(8), GETDATE(), 112), 'O', @type, @item, @wh, @loc, @lot, @unit, @qty, @stock, NULL,
                'ETCIN', @p_in_id, @in_no, @serl, @orig, N'기타입고 확정취소', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        SET @tid = SCOPE_IDENTITY();

        IF @stock = 'Y'
            UPDATE TMASTOCK SET stock_qty = stock_qty - @qty, trans_id = @tid, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @acc AND item_id = @item AND wh_id = @wh AND loc_id = @loc AND lot_no = @lot;

        UPDATE TMAETCIND SET trans_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE in_id = @p_in_id AND serl = @serl;
        FETCH NEXT FROM c INTO @serl, @item, @unit, @qty, @lot, @wh, @loc, @stock, @orig;
    END
    CLOSE c;
    DEALLOCATE c;

    UPDATE TMAETCINM SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE in_id = @p_in_id;

    COMMIT TRAN;
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCIN_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_in_id BIGINT = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'C' EXEC USP_MA_ETCIN_CONFIRM_CORE @p_in_id, @p_user_id, @p_client_pc;
        ELSE IF @p_work_type = 'CC' EXEC USP_MA_ETCIN_CANCEL_CORE @p_in_id, @p_user_id, @p_client_pc;
        SET @GeneratedCode = CAST(@p_in_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

DECLARE @grp BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE MENU_NM = N'재고관리' AND MENU_TYPE = 'GROUP' AND UPPER_MENU_ID = 10);
INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT N'기타입고등록', @grp, 3, 'FORM', 'MA', 'frmEtcIn', 'USP_MA_', 35, 'Y', 'wynlab', GETDATE()
WHERE @grp IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSMMENU WHERE SCREEN_CLASS_NM = 'frmEtcIn');
GO
