-- 웨이퍼 입고(frmRcv) 지원 (2026-09-29): TSMC 등 공급처에서 웨이퍼가 자사 창고를 거치지 않고 첫 공정 외주처(Amkor)로 직송되어 도착한 것을 등록한다.
--  * 입고 문서 TPRRCV(단일 테이블): 작업지시(시작 LOT) 기준으로 그 작업지시의 첫 공정 외주처 창고에 입고. 부분 입고(여러 번 나눠 도착) 가능,
--    누계는 작업지시 시작수량을 넘을 수 없다.
--  * 확정하면 수불(PU_IN, src_type='WR')이 생겨 시작 LOT 재고가 첫 공정 외주처 창고에 생기고, 공정실적의 "실적 대기 LOT 불러오기"에 나온다.
--    확정취소는 역거래로 되돌리며, 그 LOT를 이미 공정실적에서 썼으면(재고 부족) 서버가 막는다.
--  * 작성(0)/확정(C) 상태는 공정실적과 같은 코드 PR0006. 채번 RV. 메뉴는 생산관리 > 생산실행 > 웨이퍼입고.
--  * 선적(Billing) 시점 매입 인식은 아직 없다(P1) - 나중에 이 문서에 선적 정보를 붙이는 방식으로 확장한다.

-- ============================================================
-- 1) 테이블 / 코드 / 채번
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRRCV')
BEGIN
    CREATE TABLE TPRRCV (
        rcv_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        rcv_no         VARCHAR(20)    NOT NULL,
        rcv_date       VARCHAR(8)     NULL,
        wo_id          BIGINT         NOT NULL,
        wo_no          VARCHAR(20)    NOT NULL,
        lot_id         BIGINT         NOT NULL,             -- 시작 LOT(TPRLOT, wo_serl=0)
        lot_no         NVARCHAR(50)   NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        qty            NUMERIC(18,4)  NOT NULL,             -- 입고수량(웨이퍼 장수)
        wh_id          BIGINT         NOT NULL,             -- 입고 창고 = 첫 공정 외주처 창고
        sup_cust_id    BIGINT         NULL,                 -- 공급처(TSMC 등, 선택)
        stat_cd        VARCHAR(10)    NULL,                 -- PR0006 (0 작성, C 확정)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(30)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRRCV PRIMARY KEY CLUSTERED (rcv_id),
        CONSTRAINT CK_TPRRCV_qty CHECK (qty > 0)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRRCV_no ON TPRRCV (acc_id, rcv_no);
    CREATE NONCLUSTERED INDEX IX_TPRRCV_wo ON TPRRCV (wo_id);
END
GO

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'MA0004', 'WR', N'웨이퍼입고', 8, 'Y', 'Y', 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'WR');

INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
SELECT 'TPRRCV', N'웨이퍼입고', 'RV', 'rcv_no', 'YYMM', 4, 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TPRRCV');
GO

-- ============================================================
-- 2) USP_PR_RCV_Q - L: 목록(좌측 그리드) / Q: 한 건
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCV_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rcv_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* L: 입고번호/작업지시번호/LOT */
    @p_acc_id BIGINT = NULL,
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
        IF @p_work_type = 'L'
        BEGIN
            SELECT r.rcv_id, r.rcv_no, r.rcv_date, r.wo_no, r.lot_no, r.qty, r.unit_cd, r.stat_cd
            FROM TPRRCV r
            WHERE (@p_acc_id IS NULL OR r.acc_id = @p_acc_id)
              AND (@p_keyword IS NULL OR @p_keyword = N''
                   OR r.rcv_no LIKE '%' + @p_keyword + '%' OR r.wo_no LIKE '%' + @p_keyword + '%' OR r.lot_no LIKE '%' + @p_keyword + '%')
            ORDER BY r.rcv_id DESC;
        END
        ELSE IF @p_work_type = 'Q'
        BEGIN
            SELECT r.rcv_id, r.acc_id, r.rcv_no, r.rcv_date, r.wo_id, r.wo_no, r.lot_id, r.lot_no, r.item_id, i.item_no, i.item_nm, r.unit_cd, r.qty,
                   r.wh_id, w.wh_nm, r.sup_cust_id, c.cust_nm AS sup_cust_nm, r.stat_cd, r.cfm_dt, r.cfm_user_id, r.remark,
                   m.start_qty,
                   ISNULL((SELECT SUM(x.qty) FROM TPRRCV x WHERE x.wo_id = r.wo_id AND x.rcv_id <> r.rcv_id), 0) AS other_qty
            FROM TPRRCV r
                JOIN TPRWOM m ON m.wo_id = r.wo_id
                LEFT JOIN TBAITEM i ON i.item_id = r.item_id
                LEFT JOIN TBAWH w ON w.wh_id = r.wh_id
                LEFT JOIN TBACUST c ON c.cust_id = r.sup_cust_id
            WHERE r.rcv_id = @p_rcv_id;
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
-- 3) USP_PR_RCV_S - N/U/D (작성 상태에서만). N은 작업지시에서 시작 LOT/품목/입고 창고(첫 공정 외주처 창고)를 가져온다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCV_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rcv_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_rcv_date VARCHAR(8) = NULL,
    @p_wo_id BIGINT = NULL,                 /* N 전용 */
    @p_qty NUMERIC(18,4) = NULL,
    @p_sup_cust_id BIGINT = NULL,
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
            DECLARE @cur_stat VARCHAR(10);
            SELECT @cur_stat = stat_cd, @p_wo_id = wo_id FROM TPRRCV WHERE rcv_id = @p_rcv_id;
            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'웨이퍼 입고 문서를 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 입고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            DECLARE @wo_no VARCHAR(20), @wo_stat VARCHAR(10), @start_qty NUMERIC(18,4), @lot_id BIGINT, @lot_no NVARCHAR(50), @item BIGINT, @unit VARCHAR(10), @wh BIGINT;
            SELECT @wo_no = m.wo_no, @wo_stat = m.stat_cd, @start_qty = m.start_qty,
                   @lot_id = l.lot_id, @lot_no = l.lot_no, @item = l.item_id, @unit = l.unit_cd,
                   @wh = (SELECT d.wh_id FROM TPRWOD d WHERE d.wo_id = m.wo_id AND d.serl = (SELECT MIN(serl) FROM TPRWOD WHERE wo_id = m.wo_id))
            FROM TPRWOM m JOIN TPRLOT l ON l.wo_id = m.wo_id AND l.wo_serl = 0
            WHERE m.wo_id = @p_wo_id;

            IF @wo_no IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시를 선택하세요.'; RETURN;
            END
            IF @wo_stat IN ('E', 'X')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'완료되었거나 중단된 작업지시에는 입고할 수 없습니다.'; RETURN;
            END
            IF @wh IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시의 첫 공정에 외주처 창고가 지정되지 않았습니다. 작업지시에서 먼저 지정하세요.'; RETURN;
            END
            IF ISNULL(@p_qty, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고수량은 0보다 커야 합니다.'; RETURN;
            END
            IF ISNULL((SELECT SUM(qty) FROM TPRRCV WHERE wo_id = @p_wo_id AND (@p_work_type = 'N' OR rcv_id <> @p_rcv_id)), 0) + @p_qty > @start_qty
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고 누계가 작업지시 시작수량(' + CAST(CAST(@start_qty AS FLOAT) AS NVARCHAR(30)) + N')을 넘을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TPRRCV', 'rcv_no', @p_acc_id, @new_no OUTPUT;
            INSERT INTO TPRRCV (acc_id, rcv_no, rcv_date, wo_id, wo_no, lot_id, lot_no, item_id, unit_cd, qty, wh_id, sup_cust_id, stat_cd, cfm_yn, remark,
                                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_rcv_date, @p_wo_id, @wo_no, @lot_id, @lot_no, @item, @unit, @p_qty, @wh, @p_sup_cust_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_rcv_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 작업지시/LOT/창고는 바꿀 수 없다(다른 작업지시로 옮기려면 삭제 후 새로 등록).
            UPDATE TPRRCV SET rcv_date = @p_rcv_date, qty = @p_qty, sup_cust_id = @p_sup_cust_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rcv_id = @p_rcv_id;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRRCV WHERE rcv_id = @p_rcv_id;

        SET @GeneratedCode = CAST(@p_rcv_id AS VARCHAR(20));
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
-- 4) USP_PR_RCV_C_S - C 입고 확정(수불 PU_IN 생성) / CC 확정취소(역거래)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCV_C_S
    @p_work_type VARCHAR(50),               /* C / CC */
    ---------------------------------------------------------------------------------------------------
    @p_rcv_id BIGINT = NULL,
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
        DECLARE @acc BIGINT, @no VARCHAR(20), @date VARCHAR(8), @wo BIGINT, @lot_no NVARCHAR(50), @item BIGINT, @qty NUMERIC(18,4), @wh BIGINT, @stat VARCHAR(10), @rmk NVARCHAR(3000);
        SELECT @acc = acc_id, @no = rcv_no, @date = rcv_date, @wo = wo_id, @lot_no = lot_no, @item = item_id, @qty = qty, @wh = wh_id, @stat = stat_cd, @rmk = remark
        FROM TPRRCV WHERE rcv_id = @p_rcv_id;
        IF @stat IS NULL THROW 50001, N'웨이퍼 입고 문서를 찾을 수 없습니다.', 1;

        DECLARE @tid BIGINT;
        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0' THROW 50001, N'작성 상태의 입고만 확정할 수 있습니다.', 1;

            DECLARE @wo_stat VARCHAR(10), @start_qty NUMERIC(18,4);
            SELECT @wo_stat = stat_cd, @start_qty = start_qty FROM TPRWOM WHERE wo_id = @wo;
            IF @wo_stat IN ('E', 'X') THROW 50001, N'완료되었거나 중단된 작업지시에는 입고할 수 없습니다.', 1;
            IF ISNULL((SELECT SUM(qty) FROM TPRRCV WHERE wo_id = @wo AND rcv_id <> @p_rcv_id AND stat_cd = 'C'), 0) + @qty > @start_qty
                THROW 50001, N'확정된 입고 누계가 작업지시 시작수량을 넘어 확정할 수 없습니다.', 1;

            BEGIN TRAN;
            EXEC USP_PR_TRANS_POST @acc, 'I', 'PU_IN', @item, @wh, @lot_no, @qty, 'WR', @p_rcv_id, @no, 1, NULL, @rmk, @p_user_id, @p_client_pc, @date, @tid OUTPUT;
            UPDATE TPRRCV SET stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rcv_id = @p_rcv_id;
            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C' THROW 50001, N'확정된 입고만 확정취소할 수 있습니다.', 1;
            BEGIN TRAN;
            -- 그 LOT를 이미 공정실적에서 소진했으면 현재고 부족으로 여기서 거부된다.
            EXEC USP_PR_TRANS_REVERSE 'WR', @p_rcv_id, 0, 999999, N'웨이퍼 입고 확정취소', @p_user_id, @p_client_pc;
            UPDATE TPRRCV SET stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rcv_id = @p_rcv_id;
            COMMIT TRAN;
        END
        ELSE THROW 50001, N'처리 구분이 올바르지 않습니다.', 1;

        SET @GeneratedCode = CAST(@p_rcv_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 5) USP_PR_RCVPICK_Q - "작업지시 불러오기" 팝업(popPick 공통 파라미터 규약). 아직 시작수량만큼 입고되지 않은 계획/확정/진행 작업지시.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RCVPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 작업지시번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 시작 LOT/품번/품명 */
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
            SELECT m.wo_id, m.wo_no, m.wo_date, m.start_lot_no, m.start_qty, l.item_id, i.item_no, i.item_nm, l.unit_cd,
                   d.cust_id, c.cust_nm, d.wh_id, w.wh_nm,
                   ISNULL(r.rcv_qty, 0) AS rcv_qty, m.start_qty - ISNULL(r.rcv_qty, 0) AS remain_qty
            FROM TPRWOM m
                JOIN TPRLOT l ON l.wo_id = m.wo_id AND l.wo_serl = 0
                JOIN TPRWOD d ON d.wo_id = m.wo_id AND d.serl = (SELECT MIN(x.serl) FROM TPRWOD x WHERE x.wo_id = m.wo_id)
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TBACUST c ON c.cust_id = d.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                OUTER APPLY (SELECT SUM(x.qty) AS rcv_qty FROM TPRRCV x WHERE x.wo_id = m.wo_id) r
            WHERE m.stat_cd IN ('0', 'C', '1')
              AND m.start_qty - ISNULL(r.rcv_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.wo_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.wo_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.wo_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR d.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR m.start_lot_no LIKE '%' + @p_keyword + '%' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.wo_no;
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
-- 6) 메뉴: 생산관리 > 생산실행 > 웨이퍼입고 (작업지시 다음)
-- ============================================================
DECLARE @grp BIGINT = (SELECT TOP 1 MENU_ID FROM TSMMENU WHERE UPPER_MENU_ID = 14 AND MENU_LEVEL = 2 AND MENU_TYPE = 'GROUP' AND MENU_NM = N'생산실행');
IF @grp IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'PR' AND SCREEN_CLASS_NM = 'frmRcv')
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'웨이퍼입고', @grp, 3, 'FORM', 'PR', 'frmRcv', 'USP_PR_', 15, 'Y', SUSER_SNAME(), GETDATE());
GO
