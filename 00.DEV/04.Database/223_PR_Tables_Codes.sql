-- 생산관리(PR) 모듈 테이블/공통코드/채번/콤보 (2026-09-29). 반도체 후공정 전면 외주 업체 시연용(P0).
--
-- 핵심 개념
--  * 공정마다 품목/단위가 바뀐다(웨이퍼 장 -> Good Die 개 -> 패키지 개). 그래서 라우팅(TPRROUTED)의 행 하나가 "투입품목 -> 산출품목"
--    1:1 변환(공정 단위 BOM)이고, 다음 공정 투입량은 고정 소요량이 아니라 직전 공정의 "확정 실적 수량"이다.
--  * 작업지시(TPRWOM) 1건 = 웨이퍼 LOT 1개의 공정 체인. 공정행(TPRWOD)이 곧 외주발주(외주처/단가/납기).
--  * 재고는 신규 원장 없이 기존 수불(TMATRANS)/현재고(TMASTOCK)를 쓴다. 외주처별 가상창고(TBAWH.cust_id로 외주처 연결)와
--    IN-TRANSIT 창고를 만들어 위치만 옮긴다. 소유는 항상 자사.
--      실적 확정 : 투입 LOT PR_OUT + 산출 LOT PR_IN (같은 외주처 창고)  <- 백플러시. 수량은 실측값.
--      이전 출발 : MV_OUT(출발 외주처 창고) -> MV_IN(IN-TRANSIT) / 도착: MV_OUT(IN-TRANSIT) -> MV_IN(도착 외주처 창고, 도착수량)
--      차이 잔량은 IN-TRANSIT에 남고 귀책 지정 후 ADJ_OUT(손실) 또는 재입고.
--    수불의 src_type은 RS(공정실적)/XF(외주이전), src_id/src_no/src_serl은 각 문서.
--  * LOT는 TPRLOT(메타)/TPRLOTREL(부모->자식 계보, 분할/병합). LOT 잔량의 진실은 TMASTOCK(item_id, lot_no).
--  * 마스터-디테일 테이블은 <베이스>M / <베이스>D 로 이름 짓는다.
--  * 시연용으로 뺀 것(P1): 결재, 외주정산, 선적(Billing) 시점 매입 인식, 수량차이 허용오차, 외주단가 마스터.

-- ============================================================
-- 1) 테이블
-- ============================================================

-- 라우팅 마스터: 완제품(또는 제품군)별 공정 체인 정의
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRROUTEM')
BEGIN
    CREATE TABLE TPRROUTEM (
        route_id       BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        route_cd       VARCHAR(20)    NOT NULL,
        route_nm       NVARCHAR(100)  NOT NULL,
        item_id        BIGINT         NULL,                 -- 최종 완제품
        use_yn         VARCHAR(1)     NOT NULL CONSTRAINT DF_TPRROUTEM_use DEFAULT ('Y'),
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRROUTEM PRIMARY KEY CLUSTERED (route_id)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRROUTEM_cd ON TPRROUTEM (acc_id, route_cd);
END
GO

-- 라우팅 디테일: 공정 순번별 처리 외주처, 투입품목 -> 산출품목
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRROUTED')
BEGIN
    CREATE TABLE TPRROUTED (
        route_id       BIGINT         NOT NULL,
        serl           INT            NOT NULL,             -- 공정 순번
        acc_id         BIGINT         NOT NULL,
        proc_cd        VARCHAR(10)    NOT NULL,             -- 공정코드 TBAPROC.proc_cd
        cust_id        BIGINT         NULL,                 -- 기본 외주처
        in_item_id     BIGINT         NOT NULL,             -- 투입품목
        out_item_id    BIGINT         NOT NULL,             -- 산출품목
        in_unit_cd     VARCHAR(10)    NULL,
        out_unit_cd    VARCHAR(10)    NULL,
        split_qty      NUMERIC(18,4)  NULL,                 -- 산출 LOT 분할 수량(예: Packaging 5000). NULL이면 분할 안 함
        price_unit_cd  VARCHAR(10)    NULL,                 -- 정산단위(웨이퍼 단가면 장, 개당이면 EA)
        price          NUMERIC(18,4)  NULL,                 -- 기본 가공단가(작업지시 생성 시 복사)
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRROUTED PRIMARY KEY CLUSTERED (route_id, serl)
    );
END
GO

-- 작업지시 마스터: 웨이퍼 LOT 1개의 공정 체인. 라우팅은 공정행(TPRWOD)으로 스냅샷 복사한다.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRWOM')
BEGIN
    CREATE TABLE TPRWOM (
        wo_id          BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        wo_no          VARCHAR(20)    NOT NULL,
        wo_date        VARCHAR(8)     NULL,
        route_id       BIGINT         NOT NULL,
        item_id        BIGINT         NULL,                 -- 최종 완제품
        start_lot_no   NVARCHAR(50)   NOT NULL,             -- 시작 LOT(웨이퍼 LOT 번호)
        start_qty      NUMERIC(18,4)  NOT NULL,             -- 시작 수량(웨이퍼 장수)
        so_id          BIGINT         NULL,                 -- 수주 연계(선택)
        so_no          VARCHAR(20)    NULL,
        so_serl        INT            NULL,
        delv_date      VARCHAR(8)     NULL,
        stat_cd        VARCHAR(10)    NULL,                 -- PR0001
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRWOM PRIMARY KEY CLUSTERED (wo_id),
        CONSTRAINT CK_TPRWOM_qty CHECK (start_qty > 0)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRWOM_no ON TPRWOM (acc_id, wo_no);
    CREATE NONCLUSTERED INDEX IX_TPRWOM_so ON TPRWOM (so_id, so_serl);
END
GO

-- 작업지시 디테일: 공정행 = 외주발주. in/good/bad_qty는 확정 실적 누계(캐시, 진실은 TPRRSLTM).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRWOD')
BEGIN
    CREATE TABLE TPRWOD (
        wo_id          BIGINT         NOT NULL,
        serl           INT            NOT NULL,             -- 공정 순번(라우팅과 같은 번호)
        acc_id         BIGINT         NOT NULL,
        wo_no          VARCHAR(20)    NOT NULL,
        proc_cd        VARCHAR(10)    NOT NULL,             -- TBAPROC.proc_cd
        cust_id        BIGINT         NULL,                 -- 외주처
        wh_id          BIGINT         NULL,                 -- 외주처 창고(그 공정의 재고 위치)
        in_item_id     BIGINT         NOT NULL,
        out_item_id    BIGINT         NOT NULL,
        in_unit_cd     VARCHAR(10)    NULL,
        out_unit_cd    VARCHAR(10)    NULL,
        split_qty      NUMERIC(18,4)  NULL,
        price_unit_cd  VARCHAR(10)    NULL,
        price          NUMERIC(18,4)  NULL,                 -- 가공단가
        due_date       VARCHAR(8)     NULL,                 -- 납기
        stat_cd        VARCHAR(10)    NULL,                 -- PR0002
        in_qty         NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TPRWOD_in DEFAULT (0),     -- 투입 누계(입력단위)
        good_qty       NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TPRWOD_good DEFAULT (0),   -- 양품 누계(산출단위)
        bad_qty        NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TPRWOD_bad DEFAULT (0),    -- 불량 누계(산출단위)
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRWOD PRIMARY KEY CLUSTERED (wo_id, serl)
    );
END
GO

-- LOT 마스터(출처 메타). 잔량은 TMASTOCK가 진실.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRLOT')
BEGIN
    CREATE TABLE TPRLOT (
        lot_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        lot_no         NVARCHAR(50)   NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        init_qty       NUMERIC(18,4)  NOT NULL,             -- 생성 수량
        wo_id          BIGINT         NULL,                 -- 이 LOT가 만들어진 작업지시/공정
        wo_serl        INT            NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRLOT PRIMARY KEY CLUSTERED (lot_id)
    );
    -- TMASTOCK 키(item_id, lot_no)와 맞춘다
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRLOT_key ON TPRLOT (acc_id, item_id, lot_no);
    CREATE NONCLUSTERED INDEX IX_TPRLOT_wo ON TPRLOT (wo_id, wo_serl);
END
GO

-- LOT 계보: 부모 -> 자식(분할은 1:N, 병합은 N:1). 계보 트리 조회용.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRLOTREL')
BEGIN
    CREATE TABLE TPRLOTREL (
        parent_lot_id  BIGINT         NOT NULL,
        child_lot_id   BIGINT         NOT NULL,
        acc_id         BIGINT         NOT NULL,
        rslt_id        BIGINT         NULL,                 -- 이 관계를 만든 공정 실적(TPRRSLTM)
        qty            NUMERIC(18,4)  NULL,                 -- 부모에서 자식으로 넘어간 수량(자식 단위)
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRLOTREL PRIMARY KEY CLUSTERED (parent_lot_id, child_lot_id)
    );
    CREATE NONCLUSTERED INDEX IX_TPRLOTREL_child ON TPRLOTREL (child_lot_id);
END
GO

-- 공정 실적 마스터: 한 투입 LOT의 공정 결과. in_qty는 투입단위, good/bad는 산출단위.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRRSLTM')
BEGIN
    CREATE TABLE TPRRSLTM (
        rslt_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        rslt_no        VARCHAR(20)    NOT NULL,
        rslt_date      VARCHAR(8)     NULL,
        wo_id          BIGINT         NOT NULL,
        wo_no          VARCHAR(20)    NOT NULL,
        wo_serl        INT            NOT NULL,             -- 공정 순번
        proc_cd        VARCHAR(10)    NOT NULL,
        cust_id        BIGINT         NULL,                 -- 외주처
        wh_id          BIGINT         NULL,                 -- 외주처 창고
        in_lot_id      BIGINT         NOT NULL,             -- 투입 LOT
        in_qty         NUMERIC(18,4)  NOT NULL,             -- 투입(소진) 수량, 투입단위
        good_qty       NUMERIC(18,4)  NOT NULL,             -- 양품(산출단위) - 다음 공정 투입 가능량
        bad_qty        NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TPRRSLTM_bad DEFAULT (0),  -- 불량(산출단위)
        yield_rate     NUMERIC(9,4)   NULL,                 -- 수율(%) - 확정 시 계산해 저장
        src_file_nm    NVARCHAR(400)  NULL,                 -- 외주처가 FTP로 보낸 원본 엑셀 파일명(엑셀 업로드로 입력한 경우)
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
        CONSTRAINT PK_TPRRSLTM PRIMARY KEY CLUSTERED (rslt_id),
        CONSTRAINT CK_TPRRSLTM_qty CHECK (in_qty > 0 AND good_qty >= 0 AND bad_qty >= 0)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRRSLTM_no ON TPRRSLTM (acc_id, rslt_no);
    CREATE NONCLUSTERED INDEX IX_TPRRSLTM_wo ON TPRRSLTM (wo_id, wo_serl);
    CREATE NONCLUSTERED INDEX IX_TPRRSLTM_lot ON TPRRSLTM (in_lot_id);
END
GO

-- 공정 실적 디테일: EDS 웨이퍼별 합/부 판정(EDS 공정 실적에만 사용). 마스터 good/bad = 이 합계.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRRSLTD')
BEGIN
    CREATE TABLE TPRRSLTD (
        rslt_id        BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        wafer_no       VARCHAR(20)    NOT NULL,             -- 웨이퍼 번호(LOT 내 1~25)
        good_qty       NUMERIC(18,4)  NOT NULL,             -- Good Die 수
        bad_qty        NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TPRRSLTD_bad DEFAULT (0),  -- Bad Die 수
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRRSLTD PRIMARY KEY CLUSTERED (rslt_id, serl),
        CONSTRAINT CK_TPRRSLTD_qty CHECK (good_qty >= 0 AND bad_qty >= 0)
    );
END
GO

-- 외주 이전 마스터: 외주처 간(또는 자사 경유/역이전) 이동 문서. 출발 확인 -> 도착 확인 -> (차이 있으면) 정리.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRXFERM')
BEGIN
    CREATE TABLE TPRXFERM (
        xfer_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        xfer_no        VARCHAR(20)    NOT NULL,
        xfer_date      VARCHAR(8)     NULL,
        xfer_kind      VARCHAR(1)     NOT NULL CONSTRAINT DF_TPRXFERM_kind DEFAULT ('N'),  -- N 정방향 / R 역이전(반품/재작업)
        wo_id          BIGINT         NOT NULL,
        wo_no          VARCHAR(20)    NOT NULL,
        from_serl      INT            NULL,                 -- 출발 공정 순번
        to_serl        INT            NULL,                 -- 도착 공정 순번(출하면 NULL)
        from_cust_id   BIGINT         NULL,
        from_wh_id     BIGINT         NOT NULL,
        to_cust_id     BIGINT         NULL,
        to_wh_id       BIGINT         NOT NULL,
        trans_wh_id    BIGINT         NOT NULL,             -- IN-TRANSIT 창고
        stat_cd        VARCHAR(10)    NULL,                 -- PR0003
        out_dt         DATETIME       NULL,                 -- 출발 확인
        out_user_id    VARCHAR(30)    NULL,
        in_dt          DATETIME       NULL,                 -- 도착 확인
        in_user_id    VARCHAR(30)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRXFERM PRIMARY KEY CLUSTERED (xfer_id),
        CONSTRAINT CK_TPRXFERM_kind CHECK (xfer_kind IN ('N', 'R'))
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TPRXFERM_no ON TPRXFERM (acc_id, xfer_no);
    CREATE NONCLUSTERED INDEX IX_TPRXFERM_wo ON TPRXFERM (wo_id, from_serl);
END
GO

-- 외주 이전 디테일: LOT별 출발/도착 수량과 차이 정리. diff_qty = out_qty - in_qty(도착 확인 때 계산).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TPRXFERD')
BEGIN
    CREATE TABLE TPRXFERD (
        xfer_id        BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        xfer_no        VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        lot_id         BIGINT         NOT NULL,
        lot_no         NVARCHAR(50)   NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        out_qty        NUMERIC(18,4)  NOT NULL,             -- 출발(지시) 수량
        in_qty         NUMERIC(18,4)  NULL,                 -- 도착 확인 수량(확인 전 NULL)
        diff_qty       NUMERIC(18,4)  NULL,                 -- out_qty - in_qty (도착 확인 후)
        diff_resp_cd   VARCHAR(10)    NULL,                 -- 귀책 PR0005
        diff_act_cd    VARCHAR(10)    NULL,                 -- 처리 PR0007 (LOSS 손실 / RTN 재입고)
        diff_dt        DATETIME       NULL,
        diff_user_id   VARCHAR(30)    NULL,
        diff_remark    NVARCHAR(1000) NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TPRXFERD PRIMARY KEY CLUSTERED (xfer_id, serl),
        CONSTRAINT CK_TPRXFERD_qty CHECK (out_qty > 0 AND (in_qty IS NULL OR (in_qty >= 0 AND in_qty <= out_qty)))
    );
    CREATE NONCLUSTERED INDEX IX_TPRXFERD_lot ON TPRXFERD (lot_id);
END
GO

-- ============================================================
-- 2) TBAWH.cust_id - 외주처 창고가 어느 외주처 소속인지(외주처 재고 조회/이전 창고 기본값). NULL = 자사 창고.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TBAWH') AND name = 'cust_id')
    ALTER TABLE TBAWH ADD cust_id BIGINT NULL;
GO

-- ============================================================
-- 3) 공통코드 PR0001~PR0007
-- ============================================================
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
SELECT v.cd, v.nm, 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('PR0001', N'작업지시상태'), ('PR0002', N'공정진행상태'), ('PR0003', N'외주이전상태'), ('PR0004', N'공정코드'),
             ('PR0005', N'이전차이귀책'), ('PR0006', N'공정실적상태'), ('PR0007', N'이전차이처리')) v(cd, nm)
WHERE NOT EXISTS (SELECT 1 FROM TSMMAJOR m WHERE m.major_cd = v.cd);
GO

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT v.major, v.minor, v.nm, v.sort, 'Y', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES
    ('PR0001', '0', N'계획', 1), ('PR0001', '1', N'진행', 2), ('PR0001', 'E', N'완료', 3), ('PR0001', 'X', N'중단', 4),
    ('PR0002', '0', N'대기', 1), ('PR0002', '1', N'진행', 2), ('PR0002', 'E', N'완료', 3),
    ('PR0003', '0', N'지시', 1), ('PR0003', '1', N'이동중', 2), ('PR0003', '2', N'도착(차이대기)', 3), ('PR0003', 'E', N'완료', 4), ('PR0003', 'X', N'취소', 5),
    ('PR0004', 'BUMP', N'Bumping', 1), ('PR0004', 'EDS', N'EDS', 2), ('PR0004', 'PKG', N'Packaging', 3), ('PR0004', 'FT', N'Final Test', 4),
    ('PR0005', 'SRC', N'출발처', 1), ('PR0005', 'DST', N'도착처', 2), ('PR0005', 'TRN', N'운송', 3), ('PR0005', 'ETC', N'기타', 4),
    ('PR0006', '0', N'작성', 1), ('PR0006', 'C', N'확정', 2),
    ('PR0007', 'LOSS', N'손실', 1), ('PR0007', 'RTN', N'재입고', 2)
) v(major, minor, nm, sort)
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR n WHERE n.major_cd = v.major AND n.minor_cd = v.minor);
GO

-- 수불 원천구분(MA0004)에 공정실적/외주이전 추가
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'MA0004', v.minor, v.nm, v.sort, 'Y', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('RS', N'공정실적', 6), ('XF', N'외주이전', 7)) v(minor, nm, sort)
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR n WHERE n.major_cd = 'MA0004' AND n.minor_cd = v.minor);
GO

-- ============================================================
-- 4) 콤보 L_PR0001~L_PR0007
-- ============================================================
DECLARE @keys TABLE (k VARCHAR(30), nm NVARCHAR(100), major VARCHAR(20));
INSERT INTO @keys VALUES
    ('L_PR0001', N'작업지시상태',   'PR0001'),
    ('L_PR0002', N'공정진행상태',   'PR0002'),
    ('L_PR0003', N'외주이전상태',   'PR0003'),
    ('L_PR0004', N'공정코드',       'PR0004'),
    ('L_PR0005', N'이전차이귀책',   'PR0005'),
    ('L_PR0006', N'공정실적상태',   'PR0006'),
    ('L_PR0007', N'이전차이처리',   'PR0007');

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT k.k, NULL, k.nm, 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''' + k.major + N'''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
FROM @keys k
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = k.k);
GO

-- ============================================================
-- 5) 채번: 작업지시 WO / 공정실적 RS / 외주이전 XF
-- ============================================================
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
SELECT v.t, v.d, v.p, v.c, 'YYMM', 4, 'SYSTEM', GETDATE()
FROM (VALUES ('TPRWOM', N'작업지시마스터', 'WO', 'wo_no'),
             ('TPRRSLTM', N'공정실적마스터', 'RS', 'rslt_no'),
             ('TPRXFERM', N'외주이전마스터', 'XF', 'xfer_no')) v(t, d, p, c)
WHERE NOT EXISTS (SELECT 1 FROM TSMAutoKey a WHERE a.table_name = v.t);
GO
