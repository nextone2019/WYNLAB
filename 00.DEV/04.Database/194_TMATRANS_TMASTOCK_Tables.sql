-- 수불(TMATRANS) / 현재고(TMASTOCK) 테이블 (2026-09-25). 구매 입고뿐 아니라 판매/생산/재고조정까지 공용으로 쓴다.
--
-- 구조: 수불이 원장(진실), 현재고는 그 합계의 캐시. 입고/출고 프로시저가 확정/취소 트랜잭션 안에서 두 테이블을 같이 갱신한다.
--  * 수불은 항상 남기고, 현재고 갱신은 stock_upd_yn='Y'(재고관리 품목)일 때만 한다. stock_upd_yn은 수불 행에 스냅샷으로
--    저장해서, 나중에 품목 마스터(TBAITEM.stock_yn)가 바뀌어도 취소(역거래) 때 만들 때와 같은 방식으로 되돌린다.
--  * 취소는 원 거래를 지우지 않고 반대 부호의 역거래 행을 추가한다(org_trans_id가 원 거래를 가리킴) - 이력 보존.
--    역거래는 입고수량/출고수량이 원 거래와 뒤바뀐다.
--  * 문서와의 연결은 기존 문서들과 같은 src_type/src_id/src_no/src_serl. 구매 입고면 원천은 입고 문서 라인이고, 발주/검사까지의
--    추적은 그 입고 라인의 src 체인으로 한다.
--  * 수량 단위는 재고단위 기준(unit_cd). 단가/금액(재고평가)은 이번 구조에 넣지 않았다 - 필요해지면 컬럼 추가.
--
-- trans_type_cd(수불구분) 제안값 - 공통코드는 아직 만들지 않았다(정해지면 별도 마이그레이션):
--   PU_IN 구매입고 / PU_OUT 구매반품출고 / SA_OUT 판매출고 / SA_IN 판매반품입고 / PR_OUT 생산투입 / PR_IN 생산입고 /
--   MV_OUT 창고이동출고 / MV_IN 창고이동입고 / ADJ_IN 재고조정증가 / ADJ_OUT 재고조정감소
--
-- 현재고 키 = (acc_id, item_id, wh_id, loc_id, lot_no). 위치/LOT가 없는 재고도 한 행으로 잡히도록 loc_id는 0, lot_no는 ''
-- 를 "없음"으로 쓴다(NULL은 유니크/PK에서 같은 값으로 다루기 번거롭다). 수불에도 같은 값을 그대로 기록해서 키가 일치한다.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMATRANS')
BEGIN
    CREATE TABLE TMATRANS (
        trans_id       BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,                       -- 사업장
        trans_date     VARCHAR(8)     NOT NULL,                       -- 수불일자(yyyyMMdd)
        trans_type_cd  VARCHAR(10)    NOT NULL,                       -- 수불구분(위 제안값)
        item_id        BIGINT         NOT NULL,
        wh_id          BIGINT         NOT NULL,
        loc_id         BIGINT         NOT NULL CONSTRAINT DF_TMATRANS_loc DEFAULT (0),      -- 0 = 위치 없음
        lot_no         NVARCHAR(50)   NOT NULL CONSTRAINT DF_TMATRANS_lot DEFAULT (N''),    -- '' = LOT 없음
        unit_cd        VARCHAR(10)    NULL,                           -- 재고단위
        in_qty         NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TMATRANS_in DEFAULT (0),       -- 입고(증가)수량
        out_qty        NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TMATRANS_out DEFAULT (0),      -- 출고(감소)수량
        stock_upd_yn   VARCHAR(1)     NOT NULL CONSTRAINT DF_TMATRANS_upd DEFAULT ('Y'),    -- 현재고 갱신 여부(재고관리 품목만 Y) - 스냅샷
        cust_id        BIGINT         NULL,                           -- 거래처(구매=협력사, 판매=고객) - 조회/수불부용
        src_type       VARCHAR(10)    NULL,                           -- 원천 문서 종류(예: GR 입고, GI 출고)
        src_id         BIGINT         NULL,
        src_no         VARCHAR(20)    NULL,
        src_serl       INT            NULL,
        org_trans_id   BIGINT         NULL,                           -- 역거래(취소)면 원 거래의 trans_id, 일반 거래면 NULL
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMATRANS PRIMARY KEY CLUSTERED (trans_id),
        -- 한 행은 입고 또는 출고 중 하나만(둘 다 0인 빈 행 방지), 음수 수량 금지(역거래는 입고/출고를 뒤바꿔서 표현)
        CONSTRAINT CK_TMATRANS_qty CHECK ((in_qty > 0 AND out_qty = 0) OR (in_qty = 0 AND out_qty > 0)),
        CONSTRAINT CK_TMATRANS_upd CHECK (stock_upd_yn IN ('Y', 'N'))
    );

    -- 수불부(품목/창고/기간) 조회용
    CREATE NONCLUSTERED INDEX IX_TMATRANS_item ON TMATRANS (acc_id, item_id, wh_id, trans_date)
        INCLUDE (in_qty, out_qty, trans_type_cd, stock_upd_yn);
    -- 문서 기준 조회/취소(원천 문서로 수불 행 찾기)용
    CREATE NONCLUSTERED INDEX IX_TMATRANS_src ON TMATRANS (src_type, src_id, src_serl);
    -- 원 거래의 역거래 존재 확인용(필터 인덱스는 접속 SET 옵션에 따라 DML이 실패할 수 있어 일반 인덱스로 둔다)
    CREATE NONCLUSTERED INDEX IX_TMATRANS_org ON TMATRANS (org_trans_id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMASTOCK')
BEGIN
    CREATE TABLE TMASTOCK (
        acc_id         BIGINT         NOT NULL,
        item_id        BIGINT         NOT NULL,
        wh_id          BIGINT         NOT NULL,
        loc_id         BIGINT         NOT NULL CONSTRAINT DF_TMASTOCK_loc DEFAULT (0),      -- 0 = 위치 없음
        lot_no         NVARCHAR(50)   NOT NULL CONSTRAINT DF_TMASTOCK_lot DEFAULT (N''),    -- '' = LOT 없음
        unit_cd        VARCHAR(10)    NULL,                           -- 재고단위
        stock_qty      NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TMASTOCK_qty DEFAULT (0),      -- 현재고
        last_trans_id  BIGINT         NULL,                           -- 마지막으로 반영한 수불(정합성 점검용)
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMASTOCK PRIMARY KEY CLUSTERED (acc_id, item_id, wh_id, loc_id, lot_no)
    );

    -- 창고/위치별 재고 조회용(품목 기준은 PK가 이미 커버)
    CREATE NONCLUSTERED INDEX IX_TMASTOCK_wh ON TMASTOCK (acc_id, wh_id, loc_id) INCLUDE (item_id, stock_qty);
END
GO
