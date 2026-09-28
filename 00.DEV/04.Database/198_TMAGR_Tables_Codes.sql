-- 입고(TMAGRM/TMAGRD) 테이블/공통코드/채번/콤보 (2026-09-25). 구매입고등록(frmGr)이 첫 사용처이고, 판매/생산 입고도
-- 같은 테이블을 쓸 수 있게 헤더에 수불유형(trans_type, MA0011)을 둔다(구매입고=PU_IN).
--
-- 입고 확정 = 수불(TMATRANS) 생성 + (stock_yn='Y'인 라인만) 현재고(TMASTOCK) 반영 + 원천 라인(검사 또는 납품) next_qty 재계산.
-- 원천은 VMA_GR_READY(입고대기 뷰)의 행: src_type='IQC'(검사대상 - 합격+특채분) 또는 'DELV'(검사면제 납품분).
-- 발주까지의 추적 키(po_id/po_no/po_serl)는 매입 단계에서 TMAPOD 단가를 조회하는 데 쓰려고 라인이 보관한다(단가는 복사하지 않음).
-- 라인 stock_yn은 불러오는 시점의 발주 라인 값 스냅샷 - 나중에 품목 마스터가 바뀌어도 이 입고의 취소는 같은 방식으로 되돌린다.
-- auto_yn='Y'는 입고방식(MA.GR_MODE)이 자동일 때 납품/검사 확정이 만든 입고 - 입고 화면에서 단독 수정/삭제/확정취소를 못 하고
-- 원천(납품/검사)의 확정취소로만 취소된다.

-- ============================================================
-- 1) 테이블
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAGRM')
BEGIN
    CREATE TABLE TMAGRM (
        gr_id          BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        gr_no          VARCHAR(20)    NOT NULL,
        gr_date        VARCHAR(8)     NULL,                 -- 입고일(수불일자)
        trans_type     VARCHAR(10)    NOT NULL,             -- 수불유형 MA0011 (구매입고 PU_IN)
        cust_id        BIGINT         NULL,                 -- 거래처(구매=협력사)
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        stat_cd        VARCHAR(10)    NULL,                 -- MA0012 (0 작성, C 확정)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(30)    NULL,
        auto_yn        VARCHAR(1)     NOT NULL CONSTRAINT DF_TMAGRM_auto DEFAULT ('N'),   -- 자동 생성 입고 여부
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAGRM PRIMARY KEY CLUSTERED (gr_id)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAGRM_no ON TMAGRM (acc_id, gr_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAGRD')
BEGIN
    CREATE TABLE TMAGRD (
        gr_id          BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        gr_no          VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        gr_qty         NUMERIC(18,4)  NOT NULL,             -- 입고수량
        next_qty       NUMERIC(18,4)  NULL,                 -- 다음 단계(매입) 처리량 - 매입 설계 때 갱신
        lot_no         NVARCHAR(50)   NULL,
        wh_id          BIGINT         NULL,                 -- 확정할 때 필수
        loc_id         BIGINT         NULL,
        stock_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMAGRD_stock DEFAULT ('N'),  -- 현재고 갱신 여부 스냅샷
        src_type       VARCHAR(10)    NULL,                 -- IQC(수입검사 라인) / DELV(검사면제 납품 라인)
        src_id         BIGINT         NULL,
        src_no         VARCHAR(20)    NULL,
        src_serl       INT            NULL,
        po_id          BIGINT         NULL,                 -- 발주까지의 추적 키
        po_no          VARCHAR(20)    NULL,
        po_serl        INT            NULL,
        trans_id       BIGINT         NULL,                 -- 확정으로 만들어진 수불(TMATRANS) - 확정취소 때 역거래의 원 거래
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAGRD PRIMARY KEY CLUSTERED (gr_id, serl),
        CONSTRAINT CK_TMAGRD_qty CHECK (gr_qty > 0),
        CONSTRAINT CK_TMAGRD_stock CHECK (stock_yn IN ('Y', 'N'))
    );

    -- 원천 라인(검사/납품) next_qty 재계산용
    CREATE NONCLUSTERED INDEX IX_TMAGRD_src ON TMAGRD (src_type, src_id, src_serl) INCLUDE (gr_qty);
END
GO

-- ============================================================
-- 2) 공통코드 - MA0012 입고 진행상태, MA0004(원천구분)에 GR(입고) 추가
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0012')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0012', N'입고 진행상태', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0012')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0012', '0', N'작성', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0012', 'C', N'확정', 2, 'Y', 'Y', 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'GR')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0004', 'GR', N'입고', 5, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 3) 콤보 L_MA0012 / 채번 GR
-- ============================================================
INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_MA0012', NULL, N'입고진행상태', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''MA0012''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_MA0012');
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAGRM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAGRM', N'입고마스터', 'GR', 'gr_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO
