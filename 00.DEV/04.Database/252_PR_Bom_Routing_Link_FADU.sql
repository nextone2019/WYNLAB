-- 252: BOM(자재명세) + 제품별 라우팅 연결 + 작업지시 자재소요 스냅샷 (2026-10-03, FADU DB 전용).
--  기존: 라우팅 공정행이 투입/산출 품목을 직접 들고 있었다(공정 단위 1:1). 변경:
--   TPRBOMM/D      품목별 BOM. 상위 품목(item_id) 1개를 만드는 데 드는 구성품 목록. 구성품 구분(PR0009): MAIN 주원료(공정 투입 LOT) / RAW 원자재 / SUB 부자재 / CON 소모품.
--                  MAIN은 BOM당 정확히 1개(작업지시/LOT 흐름이 추적하는 투입 LOT), 나머지는 몇 개든 가능(소요량 관리).
--   TPRITEMROUTE   제품(item) <-> 라우팅 연결. 제품마다 1개 이상, 같은 라우팅을 여러 제품이 공유 가능. default_yn='Y'가 기본 라우팅(제품당 1개).
--   TPRROUTED      산출품목을 고르면 투입품목은 BOM 주원료로 자동 결정/검증한다(컬럼은 그대로 유지 - 결과를 담는 캐시 역할).
--   TPRWOMAT       작업지시 저장 시 각 공정 산출품목의 BOM을 복사해 두는 자재소요 스냅샷(이후 BOM을 고쳐도 이미 낸 작업지시는 그대로).
--  작업지시는 제품 -> 그 제품에 연결된 라우팅을 고른다. 공정행(TPRWOD)/실적/이전/LOT 계보는 바뀌지 않는다.
--  ※ 이 마이그레이션은 FADU DB에만 적용한다(WYNLAB_DEV 미적용).

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRBOMM')
BEGIN
    CREATE TABLE TPRBOMM (
        bom_id       BIGINT IDENTITY(1,1) NOT NULL,
        acc_id       BIGINT         NOT NULL,
        item_id      BIGINT         NOT NULL,                 -- 상위(만들어지는) 품목
        use_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TPRBOMM_use DEFAULT ('Y'),
        remark       NVARCHAR(3000) NULL,
        reg_user_id  VARCHAR(30)    NULL,
        reg_dt       DATETIME       NULL,
        reg_pc       NVARCHAR(200)  NULL,
        upt_user_id  VARCHAR(30)    NULL,
        upt_dt       DATETIME       NULL,
        upt_pc       NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRBOMM PRIMARY KEY CLUSTERED (bom_id)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRBOMM_item ON TPRBOMM (acc_id, item_id);
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRBOMD')
BEGIN
    CREATE TABLE TPRBOMD (
        bom_id        BIGINT         NOT NULL,
        serl          INT            NOT NULL,
        acc_id        BIGINT         NOT NULL,
        comp_type     VARCHAR(10)    NOT NULL,                -- PR0009 MAIN/RAW/SUB/CON
        comp_item_id  BIGINT         NOT NULL,                -- 구성품 품목
        qty_per       NUMERIC(18,6)  NOT NULL,                -- 상위 품목 1단위를 만드는 데 드는 수량
        unit_cd       VARCHAR(10)    NULL,
        loss_rate     NUMERIC(9,4)   NOT NULL CONSTRAINT DF_TPRBOMD_loss DEFAULT (0),   -- 손실율(%)
        proc_cd       VARCHAR(10)    NULL,                    -- 투입/소모 공정(비우면 상위 품목을 만드는 공정 전체)
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRBOMD PRIMARY KEY CLUSTERED (bom_id, serl),
        CONSTRAINT CK_TPRBOMD_qty CHECK (qty_per > 0)
    );
    CREATE NONCLUSTERED INDEX IX_TPRBOMD_comp ON TPRBOMD (comp_item_id);
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRITEMROUTE')
BEGIN
    CREATE TABLE TPRITEMROUTE (
        acc_id       BIGINT         NOT NULL,
        item_id      BIGINT         NOT NULL,                 -- 제품
        route_id     BIGINT         NOT NULL,
        default_yn   VARCHAR(1)     NOT NULL CONSTRAINT DF_TPRITEMROUTE_def DEFAULT ('N'),
        reg_user_id  VARCHAR(30)    NULL,
        reg_dt       DATETIME       NULL,
        reg_pc       NVARCHAR(200)  NULL,
        upt_user_id  VARCHAR(30)    NULL,
        upt_dt       DATETIME       NULL,
        upt_pc       NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRITEMROUTE PRIMARY KEY CLUSTERED (item_id, route_id)
    );
    CREATE NONCLUSTERED INDEX IX_TPRITEMROUTE_route ON TPRITEMROUTE (route_id);
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRWOMAT')
BEGIN
    CREATE TABLE TPRWOMAT (
        wo_id        BIGINT         NOT NULL,
        serl         INT            NOT NULL,                 -- 공정 순번(TPRWOD.serl)
        mat_serl     INT            NOT NULL,
        acc_id       BIGINT         NOT NULL,
        comp_type    VARCHAR(10)    NOT NULL,
        item_id      BIGINT         NOT NULL,
        unit_cd      VARCHAR(10)    NULL,
        qty_per      NUMERIC(18,6)  NOT NULL,
        loss_rate    NUMERIC(9,4)   NOT NULL CONSTRAINT DF_TPRWOMAT_loss DEFAULT (0),
        remark       NVARCHAR(3000) NULL,
        reg_user_id  VARCHAR(30)    NULL,
        reg_dt       DATETIME       NULL,
        reg_pc       NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRWOMAT PRIMARY KEY CLUSTERED (wo_id, serl, mat_serl)
    );
END
GO

-- 공통코드 PR0009 구성품구분 + 콤보
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'PR0009')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('PR0009', N'BOM구성품구분', 'Y', 'SYSTEM', GETDATE());
GO
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'PR0009', v.cd, v.nm, v.sort, 'Y', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('MAIN', N'주원료', 1), ('RAW', N'원자재', 2), ('SUB', N'부자재', 3), ('CON', N'소모품', 4)) v(cd, nm, sort)
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR n WHERE n.major_cd = 'PR0009' AND n.minor_cd = v.cd);
GO
INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_PR0009', NULL, N'BOM구성품구분', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''PR0009''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_PR0009');
GO

-- 제품별 라우팅 콤보(작업지시): 제품을 주면 그 제품에 연결된 라우팅(기본 라우팅이 맨 위), 비우면 사용 중인 전체 라우팅
CREATE OR ALTER PROCEDURE SSP_CBO_PRROUTE_Q
    @p_item_id BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.route_id, m.route_nm
    FROM TPRROUTEM m
    WHERE m.use_yn = 'Y'
      AND (ISNULL(@p_item_id, 0) = 0 OR EXISTS (SELECT 1 FROM TPRITEMROUTE ir WHERE ir.route_id = m.route_id AND ir.item_id = @p_item_id))
    ORDER BY CASE WHEN EXISTS (SELECT 1 FROM TPRITEMROUTE ir WHERE ir.route_id = m.route_id AND ir.item_id = @p_item_id AND ir.default_yn = 'Y') THEN 0 ELSE 1 END,
             m.route_cd;
END
GO
IF NOT EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'L_PRROUTE_ITEM')
BEGIN
    INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
    VALUES ('L_PRROUTE_ITEM', 'SSP_CBO_PRROUTE_Q', N'제품별 라우팅', 'route_id', 'route_nm', 'Y', N'p_item_id로 제품에 연결된 라우팅만', 'admin', GETDATE(), 'P', NULL);
    INSERT INTO sysLookupP (lookup_key, param_nm, caption, sort) VALUES ('L_PRROUTE_ITEM', 'p_item_id', N'제품', 1);
END
GO

-- 메뉴: 생산기준관리(15) 밑에 BOM관리(라우팅관리 앞)
INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT N'BOM관리', 15, 3, 'FORM', 'PR', 'frmBom', 'USP_PR_', 15, 'Y', SUSER_SNAME(), GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.MODULE = 'PR' AND m.SCREEN_CLASS_NM = 'frmBom');
GO

-- 데이터 이전: 기존 라우팅 공정행의 (투입 -> 산출) 쌍으로 BOM(주원료) 생성, 라우팅의 완제품은 제품-라우팅 연결(기본)로 옮김
INSERT INTO TPRBOMM (acc_id, item_id, use_yn, reg_user_id, reg_dt, upt_user_id, upt_dt)
SELECT DISTINCT d.acc_id, d.out_item_id, 'Y', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE()
FROM TPRROUTED d
WHERE NOT EXISTS (SELECT 1 FROM TPRBOMM b WHERE b.acc_id = d.acc_id AND b.item_id = d.out_item_id);
GO
INSERT INTO TPRBOMD (bom_id, serl, acc_id, comp_type, comp_item_id, qty_per, unit_cd, loss_rate, reg_user_id, reg_dt, upt_user_id, upt_dt)
SELECT b.bom_id, 1, b.acc_id, 'MAIN', x.in_item_id, 1, x.in_unit_cd, 0, 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE()
FROM TPRBOMM b
    CROSS APPLY (SELECT TOP 1 d.in_item_id, d.in_unit_cd FROM TPRROUTED d WHERE d.acc_id = b.acc_id AND d.out_item_id = b.item_id ORDER BY d.route_id, d.serl) x
WHERE NOT EXISTS (SELECT 1 FROM TPRBOMD bd WHERE bd.bom_id = b.bom_id);
GO
INSERT INTO TPRITEMROUTE (acc_id, item_id, route_id, default_yn, reg_user_id, reg_dt, upt_user_id, upt_dt)
SELECT r.acc_id, r.item_id, r.route_id, 'Y', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE()
FROM TPRROUTEM r
WHERE r.item_id IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TPRITEMROUTE x WHERE x.item_id = r.item_id AND x.route_id = r.route_id);
GO
-- 이미 만들어진 작업지시도 자재소요를 BOM으로 채운다(지금은 주원료만 있으므로 자재 변화는 없음)
INSERT INTO TPRWOMAT (wo_id, serl, mat_serl, acc_id, comp_type, item_id, unit_cd, qty_per, loss_rate, remark, reg_user_id, reg_dt)
SELECT wd.wo_id, wd.serl, bd.serl, wd.acc_id, bd.comp_type, bd.comp_item_id, bd.unit_cd, bd.qty_per, bd.loss_rate, bd.remark, 'SYSTEM', GETDATE()
FROM TPRWOD wd
    JOIN TPRBOMM b ON b.acc_id = wd.acc_id AND b.item_id = wd.out_item_id
    JOIN TPRBOMD bd ON bd.bom_id = b.bom_id
WHERE NOT EXISTS (SELECT 1 FROM TPRWOMAT x WHERE x.wo_id = wd.wo_id AND x.serl = wd.serl AND x.mat_serl = bd.serl);
GO

-- ============================================================
-- USP_PR_BOM_Q - L: BOM 목록(좌측 그리드) / Q: 한 건(헤더 0 + 구성품 1)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_BOM_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_bom_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* L: 품번/품명 */
    @p_use_yn VARCHAR(1) = NULL,
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
            SELECT m.bom_id, m.acc_id, m.item_id, i.item_no, i.item_nm, m.use_yn,
                   (SELECT COUNT(*) FROM TPRBOMD d WHERE d.bom_id = m.bom_id) AS comp_cnt
            FROM TPRBOMM m LEFT JOIN TBAITEM i ON i.item_id = m.item_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_keyword IS NULL OR @p_keyword = N'' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
              AND (@p_use_yn IS NULL OR @p_use_yn = '' OR m.use_yn = @p_use_yn)
            ORDER BY i.item_no;
        END
        ELSE IF @p_work_type = 'Q'
        BEGIN
            SELECT m.bom_id, m.acc_id, m.item_id, i.item_no, i.item_nm, m.use_yn, m.remark,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRROUTED rd WHERE rd.acc_id = m.acc_id AND rd.out_item_id = m.item_id) THEN 'Y' ELSE 'N' END AS used_yn
            FROM TPRBOMM m LEFT JOIN TBAITEM i ON i.item_id = m.item_id
            WHERE m.bom_id = @p_bom_id;

            SELECT d.bom_id, d.serl, d.acc_id, d.comp_type, d.comp_item_id, ci.item_no AS comp_item_no, ci.item_nm AS comp_item_nm,
                   d.qty_per, d.unit_cd, d.loss_rate, d.proc_cd, p.proc_nm, d.remark
            FROM TPRBOMD d
                LEFT JOIN TBAITEM ci ON ci.item_id = d.comp_item_id
                LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
            WHERE d.bom_id = @p_bom_id
            ORDER BY d.serl;
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
-- USP_PR_BOM_S - 헤더 N/U/D. 품목 1개당 BOM 1개. 라우팅이 산출품목으로 쓰는 BOM은 삭제 불가(사용여부를 끈다).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_BOM_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_bom_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_use_yn VARCHAR(1) = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type IN ('U', 'D') AND NOT EXISTS (SELECT 1 FROM TPRBOMM WHERE bom_id = @p_bom_id)
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'BOM을 찾을 수 없습니다.'; RETURN;
        END
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_item_id, 0) = 0 OR NOT EXISTS (SELECT 1 FROM TBAITEM WHERE item_id = @p_item_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'상위 품목을 선택하세요.'; RETURN;
            END
            IF ISNULL(@p_use_yn, 'Y') NOT IN ('Y', 'N')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사용여부가 올바르지 않습니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TPRBOMM WHERE acc_id = @p_acc_id AND item_id = @p_item_id AND (@p_work_type = 'N' OR bom_id <> @p_bom_id))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 BOM이 등록된 품목입니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TPRBOMM (acc_id, item_id, use_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @p_item_id, ISNULL(@p_use_yn, 'Y'), @p_remark, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_bom_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            DECLARE @old_item BIGINT, @old_acc BIGINT;
            SELECT @old_item = item_id, @old_acc = acc_id FROM TPRBOMM WHERE bom_id = @p_bom_id;
            IF @old_item <> @p_item_id AND EXISTS (SELECT 1 FROM TPRROUTED WHERE acc_id = @old_acc AND out_item_id = @old_item)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅에서 쓰는 BOM은 상위 품목을 바꿀 수 없습니다.'; RETURN;
            END
            UPDATE TPRBOMM SET item_id = @p_item_id, use_yn = ISNULL(@p_use_yn, 'Y'), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bom_id = @p_bom_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TPRBOMM b JOIN TPRROUTED rd ON rd.acc_id = b.acc_id AND rd.out_item_id = b.item_id WHERE b.bom_id = @p_bom_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅에서 산출품목으로 쓰는 BOM은 삭제할 수 없습니다. 사용여부를 N으로 바꾸세요.'; RETURN;
            END
            BEGIN TRAN;
            DELETE FROM TPRBOMD WHERE bom_id = @p_bom_id;
            DELETE FROM TPRBOMM WHERE bom_id = @p_bom_id;
            COMMIT TRAN;
        END
        SET @GeneratedCode = CAST(@p_bom_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- USP_PR_BOM_S_1 - 구성품 N/U/D. MAIN(주원료)은 BOM당 1개. 구성품이 자기 자신이거나 순환(구성품의 BOM을 따라가면 상위 품목이 다시 나옴)이면 거부.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_BOM_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_bom_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_comp_type VARCHAR(10) = NULL,
    @p_comp_item_id BIGINT = NULL,
    @p_qty_per NUMERIC(18,6) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_loss_rate NUMERIC(9,4) = NULL,
    @p_proc_cd VARCHAR(10) = NULL,
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
        DECLARE @acc BIGINT, @parent BIGINT;
        SELECT @acc = acc_id, @parent = item_id FROM TPRBOMM WHERE bom_id = @p_bom_id;
        IF @acc IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'BOM을 찾을 수 없습니다. 먼저 헤더를 저장하세요.'; RETURN;
        END

        DECLARE @used_in_route BIT = CASE WHEN EXISTS (SELECT 1 FROM TPRROUTED WHERE acc_id = @acc AND out_item_id = @parent) THEN 1 ELSE 0 END;

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_comp_type, '') NOT IN ('MAIN', 'RAW', 'SUB', 'CON')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'구성품 구분을 선택하세요.'; RETURN;
            END
            IF ISNULL(@p_comp_item_id, 0) = 0 OR NOT EXISTS (SELECT 1 FROM TBAITEM WHERE item_id = @p_comp_item_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'구성품 품목을 선택하세요.'; RETURN;
            END
            IF @p_comp_item_id = @parent
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'상위 품목 자신을 구성품으로 넣을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@p_qty_per, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'소요수량은 0보다 커야 합니다.'; RETURN;
            END
            IF ISNULL(@p_loss_rate, 0) < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'손실율은 0 이상이어야 합니다.'; RETURN;
            END
            IF ISNULL(@p_proc_cd, '') <> '' AND NOT EXISTS (SELECT 1 FROM TBAPROC WHERE acc_id = @acc AND proc_cd = @p_proc_cd)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정마스터에 없는 공정입니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TPRBOMD WHERE bom_id = @p_bom_id AND comp_item_id = @p_comp_item_id AND (@p_work_type = 'N' OR serl <> @p_serl))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'같은 품목이 이미 구성품에 있습니다.'; RETURN;
            END
            IF @p_comp_type = 'MAIN' AND EXISTS (SELECT 1 FROM TPRBOMD WHERE bom_id = @p_bom_id AND comp_type = 'MAIN' AND (@p_work_type = 'N' OR serl <> @p_serl))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'주원료는 BOM당 1개만 둘 수 있습니다.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            -- 순환 검사: 구성품에서 시작해 각 구성품의 BOM을 따라 내려가다 상위 품목이 나오면 순환
            DECLARE @cyc INT = 0;
            ;WITH walk AS (
                SELECT @p_comp_item_id AS item_id, 0 AS lvl
                UNION ALL
                SELECT d.comp_item_id, w.lvl + 1
                FROM walk w
                    JOIN TPRBOMM m ON m.acc_id = @acc AND m.item_id = w.item_id
                    JOIN TPRBOMD d ON d.bom_id = m.bom_id
                WHERE w.lvl < 20
            )
            SELECT @cyc = COUNT(*) FROM walk WHERE item_id = @parent;
            IF @cyc > 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'순환 구조입니다. 구성품의 BOM을 따라가면 상위 품목이 다시 나옵니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_serl IS NULL SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TPRBOMD WHERE bom_id = @p_bom_id;
            IF EXISTS (SELECT 1 FROM TPRBOMD WHERE bom_id = @p_bom_id AND serl = @p_serl)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 있는 순번입니다. (' + CAST(@p_serl AS NVARCHAR(10)) + N')'; RETURN;
            END
            IF @p_unit_cd IS NULL SELECT @p_unit_cd = unit_cd FROM TBAITEM WHERE item_id = @p_comp_item_id;
            INSERT INTO TPRBOMD (bom_id, serl, acc_id, comp_type, comp_item_id, qty_per, unit_cd, loss_rate, proc_cd, remark,
                                 reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_bom_id, @p_serl, @acc, @p_comp_type, @p_comp_item_id, @p_qty_per, @p_unit_cd, ISNULL(@p_loss_rate, 0), NULLIF(@p_proc_cd, ''), @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            DECLARE @old_type VARCHAR(10), @old_comp BIGINT;
            SELECT @old_type = comp_type, @old_comp = comp_item_id FROM TPRBOMD WHERE bom_id = @p_bom_id AND serl = @p_serl;
            IF @old_type IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수정할 구성품 행을 찾을 수 없습니다.'; RETURN;
            END
            IF @old_type = 'MAIN' AND @used_in_route = 1 AND (@p_comp_type <> 'MAIN' OR @p_comp_item_id <> @old_comp)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅에서 쓰는 BOM의 주원료는 바꿀 수 없습니다. (라우팅 공정의 투입품목이 주원료입니다)'; RETURN;
            END
            IF @p_unit_cd IS NULL SELECT @p_unit_cd = unit_cd FROM TBAITEM WHERE item_id = @p_comp_item_id;
            UPDATE TPRBOMD SET comp_type = @p_comp_type, comp_item_id = @p_comp_item_id, qty_per = @p_qty_per, unit_cd = @p_unit_cd,
                loss_rate = ISNULL(@p_loss_rate, 0), proc_cd = NULLIF(@p_proc_cd, ''), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE bom_id = @p_bom_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF @used_in_route = 1 AND EXISTS (SELECT 1 FROM TPRBOMD WHERE bom_id = @p_bom_id AND serl = @p_serl AND comp_type = 'MAIN')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅에서 쓰는 BOM의 주원료는 삭제할 수 없습니다.'; RETURN;
            END
            DELETE FROM TPRBOMD WHERE bom_id = @p_bom_id AND serl = @p_serl;
        END
        SET @GeneratedCode = CAST(@p_bom_id AS VARCHAR(20));
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
-- USP_PR_ROUTE_S_2 - 라우팅의 적용 제품(TPRITEMROUTE) N/D. default_yn='Y'는 제품당 1개(다른 라우팅의 기본은 자동으로 푼다).
--   작업지시가 이미 (제품, 라우팅) 조합으로 만들어졌으면 연결을 지울 수 없다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_S_2
    @p_work_type VARCHAR(50),               /* N 추가/수정 / D 삭제 */
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_default_yn VARCHAR(1) = NULL,
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
        DECLARE @acc BIGINT;
        SELECT @acc = acc_id FROM TPRROUTEM WHERE route_id = @p_route_id;
        IF @acc IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅을 찾을 수 없습니다. 먼저 헤더를 저장하세요.'; RETURN;
        END
        IF ISNULL(@p_item_id, 0) = 0 OR NOT EXISTS (SELECT 1 FROM TBAITEM WHERE item_id = @p_item_id)
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'적용 제품을 선택하세요.'; RETURN;
        END

        IF @p_work_type = 'N'
        BEGIN
            IF ISNULL(@p_default_yn, 'N') NOT IN ('Y', 'N')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'기본 라우팅 값이 올바르지 않습니다.'; RETURN;
            END
            BEGIN TRAN;
            IF @p_default_yn = 'Y'
                UPDATE TPRITEMROUTE SET default_yn = 'N', upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                WHERE item_id = @p_item_id AND route_id <> @p_route_id AND default_yn = 'Y';
            IF EXISTS (SELECT 1 FROM TPRITEMROUTE WHERE item_id = @p_item_id AND route_id = @p_route_id)
                UPDATE TPRITEMROUTE SET default_yn = ISNULL(@p_default_yn, 'N'), upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                WHERE item_id = @p_item_id AND route_id = @p_route_id;
            ELSE
                INSERT INTO TPRITEMROUTE (acc_id, item_id, route_id, default_yn, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                VALUES (@acc, @p_item_id, @p_route_id, ISNULL(@p_default_yn, 'N'), @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            -- 제품의 라우팅이 이것 하나뿐이면 자동으로 기본
            IF NOT EXISTS (SELECT 1 FROM TPRITEMROUTE WHERE item_id = @p_item_id AND default_yn = 'Y')
                UPDATE TPRITEMROUTE SET default_yn = 'Y' WHERE item_id = @p_item_id AND route_id = (SELECT TOP 1 route_id FROM TPRITEMROUTE WHERE item_id = @p_item_id ORDER BY route_id);
            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TPRWOM WHERE item_id = @p_item_id AND route_id = @p_route_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이 제품과 라우팅 조합으로 작업지시가 만들어져 있어 연결을 지울 수 없습니다.'; RETURN;
            END
            DELETE FROM TPRITEMROUTE WHERE item_id = @p_item_id AND route_id = @p_route_id;
        END
        SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 라우팅 조회: 적용 제품(TPRITEMROUTE) 결과셋 추가
-- ============================================================
-- 라우팅관리(frmRoute)를 기초코드등록처럼 "좌측 목록 그리드 + 우측 입력/공정 그리드"로 바꾸면서 USP_PR_ROUTE_Q에 목록 조회(L)를 추가한다 (2026-09-29).
--   L: 라우팅 목록(좌측 그리드) - p_keyword가 라우팅코드/명에 포함되는 것, 비우면 전체. p_use_yn/p_acc_id는 선택 필터.
--   Q: 기존 그대로 - 한 건(헤더 + 공정행).
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_route_cd VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_use_yn VARCHAR(1) = NULL,
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
            SELECT m.route_id, m.acc_id, m.route_cd, m.route_nm, m.item_id,
                   (SELECT STRING_AGG(i2.item_nm, N', ') FROM TPRITEMROUTE ir JOIN TBAITEM i2 ON i2.item_id = ir.item_id WHERE ir.route_id = m.route_id) AS item_nm,
                   m.use_yn,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRWOM w WHERE w.route_id = m.route_id) THEN 'Y' ELSE 'N' END AS used_yn
            FROM TPRROUTEM m
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_keyword IS NULL OR @p_keyword = N'' OR m.route_cd LIKE '%' + @p_keyword + '%' OR m.route_nm LIKE '%' + @p_keyword + '%')
              AND (@p_use_yn IS NULL OR @p_use_yn = '' OR m.use_yn = @p_use_yn)
            ORDER BY m.route_cd;
        END
        ELSE IF @p_work_type = 'Q'
        BEGIN
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = route_id FROM TPRROUTEM
            WHERE (@p_route_id IS NULL OR route_id = @p_route_id)
              AND (@p_route_id IS NOT NULL OR @p_route_cd IS NULL OR route_cd LIKE '%' + @p_route_cd + '%')
            ORDER BY route_id DESC;

            SELECT m.route_id, m.acc_id, m.route_cd, m.route_nm, m.item_id, i.item_no, i.item_nm, m.use_yn, m.remark,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRWOM w WHERE w.route_id = m.route_id) THEN 'Y' ELSE 'N' END AS used_yn
            FROM TPRROUTEM m LEFT JOIN TBAITEM i ON i.item_id = m.item_id
            WHERE m.route_id = @match_id;

            SELECT d.route_id, d.serl, d.acc_id, d.proc_cd, p.proc_nm, d.cust_id, c.cust_nm,
                   d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd,
                   d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd,
                   d.split_qty, d.price_unit_cd, d.price, d.remark
            FROM TPRROUTED d
                LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = d.cust_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE d.route_id = @match_id
            ORDER BY d.serl;

            SELECT ir.route_id, ir.item_id, i.item_no, i.item_nm, ir.default_yn
            FROM TPRITEMROUTE ir LEFT JOIN TBAITEM i ON i.item_id = ir.item_id
            WHERE ir.route_id = @match_id
            ORDER BY i.item_no;
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
-- 라우팅 공정행: 산출품목의 BOM 주원료가 투입품목(자동/검증)
-- ============================================================
-- ============================================================
-- 5) USP_PR_ROUTE_S_1 - 공정행 N/U/D. 순번(serl)은 라우팅 안에서 유일(비우면 마지막+1). 공정코드는 공정마스터에 있어야 하고 투입/산출 품목은 필수.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_proc_cd VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_in_item_id BIGINT = NULL,
    @p_out_item_id BIGINT = NULL,
    @p_in_unit_cd VARCHAR(10) = NULL,
    @p_out_unit_cd VARCHAR(10) = NULL,
    @p_split_qty NUMERIC(18,4) = NULL,
    @p_price_unit_cd VARCHAR(10) = NULL,
    @p_price NUMERIC(18,4) = NULL,
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
        DECLARE @acc BIGINT;
        SELECT @acc = acc_id FROM TPRROUTEM WHERE route_id = @p_route_id;
        IF @acc IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅을 찾을 수 없습니다. 먼저 헤더를 저장하세요.'; RETURN;
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_proc_cd, '') = '' OR NOT EXISTS (SELECT 1 FROM TBAPROC WHERE acc_id = @acc AND proc_cd = @p_proc_cd)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정을 선택하세요. (공정마스터에 등록된 공정만 쓸 수 있습니다)'; RETURN;
            END
            IF @p_out_item_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'산출품목을 선택하세요.'; RETURN;
            END
            DECLARE @bom_main BIGINT, @bom_unit VARCHAR(10);
            SELECT @bom_main = d.comp_item_id, @bom_unit = d.unit_cd
            FROM TPRBOMM b JOIN TPRBOMD d ON d.bom_id = b.bom_id AND d.comp_type = 'MAIN'
            WHERE b.acc_id = @acc AND b.item_id = @p_out_item_id AND b.use_yn = 'Y';
            IF @bom_main IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'산출품목의 BOM(주원료 포함)이 없습니다. BOM관리에서 먼저 등록하세요.'; RETURN;
            END
            IF @p_in_item_id IS NOT NULL AND @p_in_item_id <> @bom_main
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'투입품목이 산출품목 BOM의 주원료와 다릅니다. 비워 두면 BOM에서 자동으로 채워집니다.'; RETURN;
            END
            SET @p_in_item_id = @bom_main;
            IF @p_in_unit_cd IS NULL SET @p_in_unit_cd = ISNULL(@bom_unit, (SELECT unit_cd FROM TBAITEM WHERE item_id = @bom_main));
            IF @p_out_unit_cd IS NULL SET @p_out_unit_cd = (SELECT unit_cd FROM TBAITEM WHERE item_id = @p_out_item_id);
            IF @p_split_qty IS NOT NULL AND @p_split_qty <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'분할수량은 0보다 커야 합니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_serl IS NULL SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TPRROUTED WHERE route_id = @p_route_id;
            IF EXISTS (SELECT 1 FROM TPRROUTED WHERE route_id = @p_route_id AND serl = @p_serl)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 있는 공정 순번입니다. (' + CAST(@p_serl AS NVARCHAR(10)) + N')'; RETURN;
            END
            INSERT INTO TPRROUTED (route_id, serl, acc_id, proc_cd, cust_id, in_item_id, out_item_id, in_unit_cd, out_unit_cd, split_qty, price_unit_cd, price, remark,
                                   reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_route_id, @p_serl, @acc, @p_proc_cd, @p_cust_id, @p_in_item_id, @p_out_item_id, @p_in_unit_cd, @p_out_unit_cd, @p_split_qty, @p_price_unit_cd, @p_price, @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRROUTED SET proc_cd = @p_proc_cd, cust_id = @p_cust_id, in_item_id = @p_in_item_id, out_item_id = @p_out_item_id,
                in_unit_cd = @p_in_unit_cd, out_unit_cd = @p_out_unit_cd, split_qty = @p_split_qty, price_unit_cd = @p_price_unit_cd, price = @p_price, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE route_id = @p_route_id AND serl = @p_serl;
            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수정할 공정 행을 찾을 수 없습니다.'; RETURN;
            END
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRROUTED WHERE route_id = @p_route_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
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
-- 작업지시 조회: 자재소요(TPRWOMAT) 결과셋 추가. 소요수량 = (공정 투입수량 / 주원료 소요수량) x 구성품 소요수량 x (1+손실율)
--   공정 투입수량 = 그 공정 실적 투입 누계, 아직 실적이 없으면 첫 공정은 시작수량(다음 공정은 실적이 쌓이면 계산)
-- ============================================================
-- 생산관리(PR) 작업지시 프로시저 (2026-09-29). 226번(실적/이전)이 쓰는 작업지시(TPRWOM/TPRWOD)와 시작 LOT를 만든다.
--
--  작업지시 1건 = 웨이퍼 LOT 1개의 공정 체인. 저장(N)할 때
--    1) 라우팅(TPRROUTED)을 공정행(TPRWOD)으로 스냅샷 복사한다 - 이후 라우팅을 고쳐도 이미 낸 작업지시는 그대로.
--       공정행의 창고(wh_id)는 그 외주처의 외주창고(TBAWH.cust_id 연결, wh_type='OS')로 채운다.
--    2) 시작 LOT는 웨이퍼입고(TPRRCV, 234/235번)로 이미 입고된 미배정 웨이퍼 LOT를 골라 이 작업지시에 배정한다(TPRLOT.wo_id/wo_serl=0).
--       웨이퍼가 작업지시보다 먼저 도착하는 실제 순서를 따른다. 시작수량 = 그 LOT의 첫 공정 외주처 창고 재고 전량. 삭제하면 배정만 풀린다.
--  수정(U)은 일자/수주 연계/납기/담당/비고만. 시작 LOT/라우팅은 못 바꾼다(잘못 만들었으면 삭제 후 재작성).
--  삭제(D)는 실적/이전이 하나도 없을 때만.
--
--  USP_PR_WO_Q     Q: 헤더(0)+공정행(1)+LOT(2) / L: 목록(기간/상태/번호)
--  USP_PR_WO_S     헤더 N/U/D
--  USP_PR_WO_S_1   공정행 U(외주처/창고/단가/납기/분할수량/비고)
--  USP_PR_WO_C_S   상태: C 확정 / CC 확정취소 / X 중단 / XC 중단해제 / E 완료 / EC 완료취소  (계획 0 -> 확정 C -> 진행 1(첫 실적) -> 완료 E)

-- ============================================================
-- 1) USP_PR_WO_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_Q
    @p_work_type VARCHAR(50),               /* Q 단건 / L 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,          /* 시작 LOT 검색 */
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
            SELECT TOP 1 @match_id = wo_id FROM TPRWOM
            WHERE (@p_wo_id IS NULL OR wo_id = @p_wo_id)
              AND (@p_wo_id IS NOT NULL OR @p_wo_no IS NULL OR wo_no LIKE '%' + @p_wo_no + '%')
            ORDER BY wo_id DESC;

            SELECT
                m.wo_id, m.acc_id, m.wo_no, m.wo_date, m.route_id, r.route_cd, r.route_nm,
                m.item_id, i.item_no, i.item_nm, m.start_lot_no, m.start_qty,
                m.so_id, m.so_no, m.so_serl, m.delv_date, m.stat_cd,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.remark
            FROM TPRWOM m
                LEFT JOIN TPRROUTEM r ON r.route_id = m.route_id
                LEFT JOIN TBAITEM i ON i.item_id = m.item_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.wo_id = @match_id;

            SELECT
                dt.wo_id, dt.serl, dt.acc_id, dt.wo_no, dt.proc_cd, p.proc_nm,
                dt.cust_id, c.cust_nm, dt.wh_id, w.wh_nm,
                dt.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, dt.in_unit_cd,
                dt.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, dt.out_unit_cd,
                dt.split_qty, dt.price_unit_cd, dt.price, dt.due_date, dt.stat_cd,
                dt.in_qty, dt.good_qty, dt.bad_qty,
                CASE WHEN dt.in_qty > 0 AND ISNULL(dt.in_unit_cd, '') = ISNULL(dt.out_unit_cd, '') THEN dt.good_qty * 100.0 / dt.in_qty
                     WHEN dt.good_qty + dt.bad_qty > 0 THEN dt.good_qty * 100.0 / (dt.good_qty + dt.bad_qty) ELSE NULL END AS yield_rate,
                dt.remark
            FROM TPRWOD dt
                LEFT JOIN TBAPROC p ON p.acc_id = dt.acc_id AND p.proc_cd = dt.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = dt.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBAITEM ii ON ii.item_id = dt.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = dt.out_item_id
            WHERE dt.wo_id = @match_id
            ORDER BY dt.serl;

            -- 이 작업지시의 LOT와 현재 위치별 재고
            SELECT l.lot_id, l.lot_no, l.wo_serl, l.item_id, i.item_no, i.item_nm, l.unit_cd, l.init_qty,
                   s.wh_id, w.wh_nm, s.stock_qty
            FROM TPRLOT l
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.loc_id = 0 AND s.stock_qty <> 0
                LEFT JOIN TBAWH w ON w.wh_id = s.wh_id
            WHERE l.wo_id = @match_id
            ORDER BY l.wo_serl, l.lot_no;

            -- 자재소요
            SELECT mt.wo_id, mt.serl, p.proc_nm, mt.mat_serl, mt.comp_type, mt.item_id, i.item_no, i.item_nm, mt.unit_cd, mt.qty_per, mt.loss_rate,
                   CASE WHEN x.base_qty IS NULL OR ISNULL(mn.qty_per, 0) = 0 THEN NULL
                        ELSE x.base_qty / mn.qty_per * mt.qty_per * (1 + mt.loss_rate / 100.0) END AS req_qty,
                   mt.remark
            FROM TPRWOMAT mt
                JOIN TPRWOD wd ON wd.wo_id = mt.wo_id AND wd.serl = mt.serl
                JOIN TPRWOM wm ON wm.wo_id = mt.wo_id
                LEFT JOIN TPRWOMAT mn ON mn.wo_id = mt.wo_id AND mn.serl = mt.serl AND mn.comp_type = 'MAIN'
                LEFT JOIN TBAPROC p ON p.acc_id = wd.acc_id AND p.proc_cd = wd.proc_cd
                LEFT JOIN TBAITEM i ON i.item_id = mt.item_id
                CROSS APPLY (SELECT COALESCE(NULLIF(wd.in_qty, 0),
                                             CASE WHEN wd.serl = (SELECT MIN(serl) FROM TPRWOD WHERE wo_id = mt.wo_id) THEN wm.start_qty END) AS base_qty) x
            WHERE mt.wo_id = @match_id
            ORDER BY mt.serl, mt.mat_serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.wo_id, m.wo_no, m.wo_date, m.route_nm, m.item_nm, m.start_lot_no, m.start_qty, m.so_no, m.delv_date, m.stat_cd,
                -- 진행 공정: 실적이 있는 마지막 공정 순번/이름과 그 공정 양품 누계
                pr.serl AS cur_serl, pr.proc_nm AS cur_proc_nm, pr.good_qty AS cur_good_qty, pr.out_unit_cd AS cur_unit_cd
            FROM (
                SELECT wm.*, r.route_nm, i.item_nm
                FROM TPRWOM wm
                    LEFT JOIN TPRROUTEM r ON r.route_id = wm.route_id
                    LEFT JOIN TBAITEM i ON i.item_id = wm.item_id
            ) m
                OUTER APPLY (
                    SELECT TOP 1 d.serl, p.proc_nm, d.good_qty, d.out_unit_cd
                    FROM TPRWOD d LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                    WHERE d.wo_id = m.wo_id AND d.in_qty > 0 ORDER BY d.serl DESC
                ) pr
            WHERE (@p_fr_date IS NULL OR m.wo_date >= @p_fr_date)
              AND (@p_to_date IS NULL OR m.wo_date <= @p_to_date)
              AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
              AND (@p_wo_no IS NULL OR m.wo_no LIKE '%' + @p_wo_no + '%')
              AND (@p_lot_no IS NULL OR m.start_lot_no LIKE '%' + @p_lot_no + '%')
            ORDER BY m.wo_id DESC;
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
-- 작업지시 저장: 제품(p_item_id) 선택 -> 연결된 라우팅 검증 + 자재소요(BOM) 스냅샷
-- ============================================================
-- ============================================================
-- 2) USP_PR_WO_S - 헤더 N/U/D
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_wo_date VARCHAR(8) = NULL,
    @p_route_id BIGINT = NULL,              /* N 전용 */
    @p_item_id BIGINT = NULL,               /* N 전용: 제품(라우팅은 이 제품에 연결된 것만) */
    @p_start_lot_no NVARCHAR(50) = NULL,    /* (사용 안 함 - 호환용) 시작 LOT는 @p_start_lot_id로 고른다 */
    @p_start_qty NUMERIC(18,4) = NULL,      /* (사용 안 함 - 호환용) 시작수량은 LOT의 첫 공정 창고 재고 전량 */
    @p_start_lot_id BIGINT = NULL,          /* N 전용: 웨이퍼입고(TPRRCV)로 입고된 미배정 웨이퍼 LOT(TPRLOT) */
    @p_so_id BIGINT = NULL,
    @p_so_serl INT = NULL,
    @p_delv_date VARCHAR(8) = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type IN ('U', 'D')
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TPRWOM WHERE wo_id = @p_wo_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시를 찾을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U') AND @p_so_id IS NOT NULL
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TSASOM WHERE so_id = @p_so_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'연계할 수주를 찾을 수 없습니다.'; RETURN;
            END
            IF @p_so_serl IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_so_serl)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'연계할 수주 품목을 찾을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @route_item BIGINT, @route_use VARCHAR(1);
            SELECT @route_item = item_id, @route_use = use_yn FROM TPRROUTEM WHERE route_id = @p_route_id;
            IF @route_use IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅을 선택하세요.'; RETURN;
            END
            IF @route_use <> 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사용하지 않는 라우팅입니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TPRROUTED WHERE route_id = @p_route_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정이 없는 라우팅입니다.'; RETURN;
            END
            IF ISNULL(@p_item_id, 0) = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'제품을 선택하세요.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TPRITEMROUTE WHERE acc_id = @p_acc_id AND item_id = @p_item_id AND route_id = @p_route_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'선택한 제품에 연결된 라우팅이 아닙니다. 라우팅관리의 적용 제품에서 연결하세요.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TPRROUTED WHERE route_id = @p_route_id AND serl = (SELECT MAX(serl) FROM TPRROUTED WHERE route_id = @p_route_id) AND out_item_id = @p_item_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅의 마지막 공정 산출품목이 선택한 제품과 다릅니다.'; RETURN;
            END
            IF ISNULL(@p_start_lot_id, 0) = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고된 웨이퍼 LOT를 선택하세요. (먼저 웨이퍼입고 화면에서 입고를 확정해야 합니다)'; RETURN;
            END

            -- 시작 LOT = 웨이퍼입고로 들어온 미배정 LOT. 품목이 라우팅 첫 공정 투입품목이어야 하고, 첫 공정 외주처 창고에 재고가 있어야 한다.
            -- 시작수량은 그 창고 재고 전량(한 작업지시 = 웨이퍼 LOT 하나 통째로).
            DECLARE @first_serl INT, @first_item BIGINT, @first_unit VARCHAR(10), @first_wh BIGINT, @lot_no NVARCHAR(50), @lot_item BIGINT, @lot_wo BIGINT, @stk NUMERIC(18,4);
            SELECT TOP 1 @first_serl = d.serl, @first_item = d.in_item_id, @first_unit = d.in_unit_cd,
                   @first_wh = (SELECT TOP 1 w.wh_id FROM TBAWH w WHERE w.acc_id = @p_acc_id AND w.cust_id = d.cust_id AND w.wh_type = 'OS' ORDER BY w.wh_id)
            FROM TPRROUTED d WHERE d.route_id = @p_route_id ORDER BY d.serl;

            SELECT @lot_no = lot_no, @lot_item = item_id, @lot_wo = wo_id FROM TPRLOT WHERE lot_id = @p_start_lot_id AND acc_id = @p_acc_id;
            IF @lot_no IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'선택한 웨이퍼 LOT를 찾을 수 없습니다.'; RETURN;
            END
            IF @lot_wo IS NOT NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 다른 작업지시에 배정된 LOT입니다.'; RETURN;
            END
            IF @lot_item <> @first_item
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'LOT의 품목이 이 라우팅 첫 공정의 투입품목과 다릅니다.'; RETURN;
            END

            IF @first_wh IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅 첫 공정의 외주처에 연결된 외주창고가 없습니다.'; RETURN;
            END
            SELECT @stk = stock_qty FROM TMASTOCK WHERE acc_id = @p_acc_id AND item_id = @lot_item AND wh_id = @first_wh AND loc_id = 0 AND lot_no = @lot_no;
            IF ISNULL(@stk, 0) <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'웨이퍼 LOT ' + @lot_no + N'의 재고가 첫 공정 외주처 창고에 없습니다. 웨이퍼입고 화면에서 그 창고로 입고했는지 확인하세요.'; RETURN;
            END
            SET @p_start_qty = @stk;
            SET @p_start_lot_no = @lot_no;

            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TPRWOM', 'wo_no', @p_acc_id, @new_no OUTPUT;

            BEGIN TRAN;

            INSERT INTO TPRWOM (
                acc_id, wo_no, wo_date, route_id, item_id, start_lot_no, start_qty, so_id, so_no, so_serl, delv_date, stat_cd, dept_id, emp_id, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_wo_date, @p_route_id, @p_item_id, @p_start_lot_no, @p_start_qty,
                @p_so_id, (SELECT so_no FROM TSASOM WHERE so_id = @p_so_id), @p_so_serl, @p_delv_date, '0', @p_dept_id, @p_emp_id, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );
            SET @p_wo_id = SCOPE_IDENTITY();

            INSERT INTO TPRWOD (
                wo_id, serl, acc_id, wo_no, proc_cd, cust_id, wh_id, in_item_id, out_item_id, in_unit_cd, out_unit_cd, split_qty, price_unit_cd, price, stat_cd,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT @p_wo_id, d.serl, @p_acc_id, @new_no, d.proc_cd, d.cust_id,
                   (SELECT TOP 1 w.wh_id FROM TBAWH w WHERE w.acc_id = @p_acc_id AND w.cust_id = d.cust_id AND w.wh_type = 'OS' ORDER BY w.wh_id),
                   d.in_item_id, d.out_item_id, d.in_unit_cd, d.out_unit_cd, d.split_qty, d.price_unit_cd, d.price, '0',
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TPRROUTED d WHERE d.route_id = @p_route_id;

            -- 자재소요 스냅샷: 각 공정 산출품목의 BOM(주원료/원자재/부자재/소모품)을 복사한다. 이후 BOM을 고쳐도 이미 낸 작업지시는 그대로.
            INSERT INTO TPRWOMAT (wo_id, serl, mat_serl, acc_id, comp_type, item_id, unit_cd, qty_per, loss_rate, remark, reg_user_id, reg_dt, reg_pc)
            SELECT @p_wo_id, wd.serl, bd.serl, @p_acc_id, bd.comp_type, bd.comp_item_id, bd.unit_cd, bd.qty_per, bd.loss_rate, bd.remark, @p_user_id, GETDATE(), @p_client_pc
            FROM TPRWOD wd
                JOIN TPRBOMM b ON b.acc_id = wd.acc_id AND b.item_id = wd.out_item_id
                JOIN TPRBOMD bd ON bd.bom_id = b.bom_id AND (bd.proc_cd IS NULL OR bd.proc_cd = wd.proc_cd)
            WHERE wd.wo_id = @p_wo_id;

            -- 입고된 LOT를 이 작업지시의 시작 LOT로 배정한다(LOT 자체는 웨이퍼입고 때 이미 만들어져 있다).
            UPDATE TPRLOT SET wo_id = @p_wo_id, wo_serl = 0, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc WHERE lot_id = @p_start_lot_id;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRWOM SET
                wo_date = @p_wo_date,
                so_id = @p_so_id, so_no = (SELECT so_no FROM TSASOM WHERE so_id = @p_so_id), so_serl = @p_so_serl,
                delv_date = @p_delv_date, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE wo_id = @p_wo_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TPRRSLTM WHERE wo_id = @p_wo_id) OR EXISTS (SELECT 1 FROM TPRXFERM WHERE wo_id = @p_wo_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'실적이나 이전이 등록된 작업지시는 삭제할 수 없습니다. 중단 처리하세요.'; RETURN;
            END
            BEGIN TRAN;
            -- 시작 LOT는 웨이퍼입고로 만들어진 것이라 지우지 않고 배정만 푼다(다른 작업지시에 다시 쓸 수 있게). 입고 기록도 재고도 없는 옛 LOT(고아)만 지운다.
            UPDATE TPRLOT SET wo_id = NULL, wo_serl = NULL WHERE wo_id = @p_wo_id AND wo_serl = 0;
            DELETE l FROM TPRLOT l
            WHERE l.wo_id IS NULL AND l.lot_no = (SELECT start_lot_no FROM TPRWOM WHERE wo_id = @p_wo_id)
              AND NOT EXISTS (SELECT 1 FROM TPRRCV r WHERE r.acc_id = l.acc_id AND r.item_id = l.item_id AND r.lot_no = l.lot_no)
              AND NOT EXISTS (SELECT 1 FROM TMASTOCK s WHERE s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.stock_qty <> 0);
            DELETE FROM TPRWOMAT WHERE wo_id = @p_wo_id;
            DELETE FROM TPRWOD WHERE wo_id = @p_wo_id;
            DELETE FROM TPRWOM WHERE wo_id = @p_wo_id;
            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_wo_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 입고 LOT 선택: p_route_id를 주면 그 라우팅 첫 공정 투입품목의 LOT만
-- ============================================================
-- ============================================================
-- 5) USP_PR_WOLOTPICK_Q - 작업지시의 "입고 LOT 불러오기" 팝업(popPick 공통 파라미터 규약).
--    웨이퍼입고로 들어와 아직 어떤 작업지시에도 배정되지 않았고 재고가 남아 있는 LOT. 재고 있는 창고별로 한 행.
--    p_date_from/to는 LOT 등록일 기준, p_doc_no/p_keyword는 LOT번호/품번/품명.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WOLOTPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* LOT 번호 */
    @p_cust_id BIGINT = NULL,
    @p_route_id BIGINT = NULL,              /* 라우팅 첫 공정 투입품목의 LOT만 */
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명 */
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
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.wh_id, w.wh_nm, s.stock_qty,
                   CONVERT(VARCHAR(8), l.reg_dt, 112) AS lot_date,
                   (SELECT TOP 1 c.cust_nm FROM TPRRCV r JOIN TBACUST c ON c.cust_id = r.sup_cust_id
                    WHERE r.acc_id = l.acc_id AND r.item_id = l.item_id AND r.lot_no = l.lot_no AND r.sup_cust_id IS NOT NULL ORDER BY r.rcv_id DESC) AS sup_cust_nm
            FROM TPRLOT l
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.loc_id = 0 AND s.stock_qty > 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TBAWH w ON w.wh_id = s.wh_id
            WHERE l.wo_id IS NULL
              AND (ISNULL(@p_route_id, 0) = 0 OR l.item_id = (SELECT TOP 1 rd.in_item_id FROM TPRROUTED rd WHERE rd.route_id = @p_route_id ORDER BY rd.serl))
              AND (@p_acc_id IS NULL OR l.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR CONVERT(VARCHAR(8), l.reg_dt, 112) >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR CONVERT(VARCHAR(8), l.reg_dt, 112) <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR l.lot_no LIKE '%' + @p_doc_no + '%')
              AND (@p_keyword IS NULL OR @p_keyword = '' OR i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY l.lot_no, w.wh_nm;
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
