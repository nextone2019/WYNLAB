-- 기타출고요청 / 기타출고 / 기초재고 테이블 + 공통코드 + 채번 (2026-10-03, WYNLAB_DEV 전용)
--
-- 업무 흐름
--   1) 기타출고요청(TMAETCREQM/D): 작성 -> 전자결재 상신 -> 최종승인(= 확정, stat_cd 0 -> C, cfm_yn='Y').
--      구매요청(TMAPOREQM/D)과 같은 모양 - 결재 연결(app_id/app_no), 라인 단위 next_qty(= 기타출고 확정 누계)와 중단(stop_yn).
--   2) 기타출고(TMAETCOUTM/D): 승인완료된 기타출고요청 라인을 불러와(src_type='ETCREQ') 작성 -> 확정.
--      확정 = 수불(TMATRANS, trans_kind='O', trans_type='ETC_OUT') 생성 + (stock_yn='Y' 라인만) 현재고(TMASTOCK) 차감
--      + 원천 요청 라인 next_qty 재계산. (입고 확정 USP_MA_GR_CONFIRM_CORE의 출고 버전)
--   3) 기초재고(TMAOPENM/D): 작성 -> 확정. 확정 = 수불(trans_kind='I', trans_type='OPEN_IN', src_type='OPEN') 생성
--      + (stock_yn='Y' 라인만) 현재고 증가. 결재는 타지 않는다.
--
-- 규칙 - 테이블 구조는 입고(TMAGRM/TMAGRD, 198번)와 같은 관례를 따른다: 헤더 <base>M / 라인 <base>D, 헤더 PK는 IDENTITY,
-- 라인 PK는 (헤더id, serl), 문서번호는 사업장별 유니크(acc_id, xxx_no), 원천 추적은 src_type/src_id/src_no/src_serl,
-- 확정으로 만들어진 수불은 라인 trans_id에 보관(확정취소 때 역거래의 원 거래). 라인 stock_yn은 작성 시점 품목 마스터의
-- 재고관리 여부 스냅샷. 모든 테이블에 acc_id(사업장)가 있고 화면 조회조건 첫 번째는 사업장이다.
--
-- 이 파일은 테이블/코드/채번만 만든다 - 프로시저(USP_MA_ETCREQ_*, USP_MA_ETCOUT_*, USP_MA_OPEN_*), 전자결재 문서유형(AP0002 'ETCREQ')
-- 후처리 프로시저, 메뉴/화면은 다음 단계. 여러 번 실행해도 안전하다(IF NOT EXISTS).

-- ============================================================
-- 1) 기타출고요청 - TMAETCREQM / TMAETCREQD
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAETCREQM')
BEGIN
    CREATE TABLE TMAETCREQM (
        req_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        req_no         VARCHAR(20)    NOT NULL,
        req_date       VARCHAR(8)     NULL,                 -- 요청일
        req_title      NVARCHAR(200)  NULL,                 -- 요청 제목(결재 제목에도 쓴다)
        out_reason     VARCHAR(10)    NULL,                 -- 출고사유 MA0013
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,                 -- 요청자
        stat_cd        VARCHAR(10)    NULL,                 -- MA0001 (0 작성, C 승인완료=확정)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(30)    NULL,
        stop_yn        VARCHAR(1)     NULL,                 -- 잔량 마감(더 출고하지 않음)
        stop_dt        DATETIME       NULL,
        stop_user_id   VARCHAR(30)    NULL,
        app_id         BIGINT         NULL,                 -- 전자결재 연결(TAPDOC)
        app_no         VARCHAR(20)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAETCREQM PRIMARY KEY CLUSTERED (req_id)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAETCREQM_no ON TMAETCREQM (acc_id, req_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAETCREQD')
BEGIN
    CREATE TABLE TMAETCREQD (
        req_id         BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        req_no         VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        qty            NUMERIC(18,4)  NOT NULL,             -- 요청수량
        next_qty       NUMERIC(18,4)  NULL,                 -- 기타출고 확정 누계(다음 단계 처리량)
        wh_id          BIGINT         NULL,                 -- 출고 희망 창고/위치/LOT - 비워도 되고, 실제 출고(기타출고)에서 정한다
        loc_id         BIGINT         NULL,
        lot_no         NVARCHAR(50)   NULL,
        cfm_yn         VARCHAR(1)     NULL,                 -- 라인 확정(헤더 확정 때 같이 Y)
        stop_yn        VARCHAR(1)     NULL,                 -- 라인 잔량 마감
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAETCREQD PRIMARY KEY CLUSTERED (req_id, serl),
        CONSTRAINT CK_TMAETCREQD_qty CHECK (qty > 0)
    );

    CREATE NONCLUSTERED INDEX IX_TMAETCREQD_item ON TMAETCREQD (acc_id, item_id) INCLUDE (qty, next_qty);
END
GO

-- ============================================================
-- 2) 기타출고 - TMAETCOUTM / TMAETCOUTD
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAETCOUTM')
BEGIN
    CREATE TABLE TMAETCOUTM (
        out_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        out_no         VARCHAR(20)    NOT NULL,
        out_date       VARCHAR(8)     NULL,                 -- 출고일(수불일자)
        out_reason     VARCHAR(10)    NULL,                 -- 출고사유 MA0013 (요청에서 불러오면 요청의 값)
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,                 -- 출고 담당자
        stat_cd        VARCHAR(10)    NULL,                 -- MA0012 (0 작성, C 확정)
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
        CONSTRAINT PK_TMAETCOUTM PRIMARY KEY CLUSTERED (out_id)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAETCOUTM_no ON TMAETCOUTM (acc_id, out_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAETCOUTD')
BEGIN
    CREATE TABLE TMAETCOUTD (
        out_id         BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        out_no         VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        out_qty        NUMERIC(18,4)  NOT NULL,             -- 출고수량
        lot_no         NVARCHAR(50)   NULL,                 -- 출고할 재고의 LOT/창고/위치 (확정할 때 필수, 재고가 있어야 함)
        wh_id          BIGINT         NULL,
        loc_id         BIGINT         NULL,
        stock_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMAETCOUTD_stock DEFAULT ('N'),  -- 현재고 갱신 여부 스냅샷
        src_type       VARCHAR(10)    NULL,                 -- ETCREQ(기타출고요청 라인) - 직접 출고면 비움
        src_id         BIGINT         NULL,
        src_no         VARCHAR(20)    NULL,
        src_serl       INT            NULL,
        trans_id       BIGINT         NULL,                 -- 확정으로 만들어진 수불(TMATRANS)
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAETCOUTD PRIMARY KEY CLUSTERED (out_id, serl),
        CONSTRAINT CK_TMAETCOUTD_qty CHECK (out_qty > 0),
        CONSTRAINT CK_TMAETCOUTD_stock CHECK (stock_yn IN ('Y', 'N'))
    );

    -- 원천 요청 라인 next_qty 재계산용
    CREATE NONCLUSTERED INDEX IX_TMAETCOUTD_src ON TMAETCOUTD (src_type, src_id, src_serl) INCLUDE (out_qty);
END
GO

-- ============================================================
-- 3) 기초재고 - TMAOPENM / TMAOPEND
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAOPENM')
BEGIN
    CREATE TABLE TMAOPENM (
        open_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        open_no        VARCHAR(20)    NOT NULL,
        open_date      VARCHAR(8)     NULL,                 -- 기초재고 기준일(수불일자)
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,                 -- 등록 담당자
        stat_cd        VARCHAR(10)    NULL,                 -- MA0012 (0 작성, C 확정)
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
        CONSTRAINT PK_TMAOPENM PRIMARY KEY CLUSTERED (open_id)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAOPENM_no ON TMAOPENM (acc_id, open_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAOPEND')
BEGIN
    CREATE TABLE TMAOPEND (
        open_id        BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        open_no        VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        qty            NUMERIC(18,4)  NOT NULL,             -- 기초수량
        lot_no         NVARCHAR(50)   NULL,
        wh_id          BIGINT         NULL,                 -- 확정할 때 필수
        loc_id         BIGINT         NULL,
        stock_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMAOPEND_stock DEFAULT ('N'),    -- 현재고 갱신 여부 스냅샷
        trans_id       BIGINT         NULL,                 -- 확정으로 만들어진 수불(TMATRANS)
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAOPEND PRIMARY KEY CLUSTERED (open_id, serl),
        CONSTRAINT CK_TMAOPEND_qty CHECK (qty > 0),
        CONSTRAINT CK_TMAOPEND_stock CHECK (stock_yn IN ('Y', 'N'))
    );
END
GO

-- ============================================================
-- 4) 공통코드 - MA0013 기타출고사유(신규), MA0011 수불유형에 OPEN_IN(기초재고), MA0004 원천구분에 ETCREQ/ETCOUT/OPEN
--    (기타출고 ETC_OUT 은 MA0011에 이미 있다)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0013')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0013', N'기타출고사유', 'N', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0013')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0013', 'SCRAP',    N'폐기',     1, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0013', 'SAMPLE',   N'샘플출고', 2, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0013', 'INTERNAL', N'사내사용', 3, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0013', 'DEFECT',   N'불량처리', 4, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0013', 'ETC',      N'기타',     9, 'N', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = 'OPEN_IN')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, rel_cd1, reg_user_id, reg_dt)
VALUES ('MA0011', 'OPEN_IN', N'기초재고', (SELECT ISNULL(MAX(sort), 0) + 1 FROM TSMMINOR WHERE major_cd = 'MA0011'), 'Y', 'Y', 'I', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'ETCREQ')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'MA0004', v.cd, v.nm, (SELECT ISNULL(MAX(sort), 0) FROM TSMMINOR WHERE major_cd = 'MA0004') + v.n, 'Y', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('ETCREQ', N'기타출고요청', 1), ('ETCOUT', N'기타출고', 2), ('OPEN', N'기초재고', 3)) v(cd, nm, n);
GO

-- ============================================================
-- 5) 콤보 L_MA0013 / 채번 ER(기타출고요청) EO(기타출고) OB(기초재고)
-- ============================================================
INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_MA0013', NULL, N'기타출고사유', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''MA0013''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_MA0013');
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAETCREQM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAETCREQM', N'기타출고요청마스터', 'ER', 'req_no', 'YYMM', 4, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAETCOUTM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAETCOUTM', N'기타출고마스터', 'EO', 'out_no', 'YYMM', 4, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAOPENM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAOPENM', N'기초재고마스터', 'OB', 'open_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO
