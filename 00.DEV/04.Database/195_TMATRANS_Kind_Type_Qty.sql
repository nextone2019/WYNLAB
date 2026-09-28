-- 수불/현재고 컬럼 정리 (2026-09-25, 사장님 지시). 194번에서 만든 구조를 아래처럼 바꾼다:
--   1) TMASTOCK.last_trans_id -> trans_id (마지막으로 반영한 수불)
--   2) TMATRANS.in_qty/out_qty 두 컬럼 -> qty 한 컬럼(항상 양수). 방향은 trans_kind가 나타낸다.
--   3) TMATRANS.trans_type_cd -> trans_type(수불유형) + trans_kind(수불구분: I 입고 / O 출고) 추가
--
-- trans_type(수불유형) 제안값 - 공통코드는 아직 만들지 않았다(정해지면 별도 마이그레이션). 방향은 trans_kind가 따로 가지므로
-- 유형 이름에 방향이 들어 있어도 되고, 유형만으로도 방향을 알 수 있게 한다:
--   PU_IN 구매입고 / PU_OUT 구매반품출고 / ETC_IN 기타입고 / ETC_OUT 기타출고 / MV_IN 이동입고 / MV_OUT 이동출고 /
--   PR_IN 생산입고 / PR_OUT 생산투입 / SA_OUT 판매출고 / SA_IN 판매반품입고 / ADJ_IN 재고조정증가 / ADJ_OUT 재고조정감소
-- 취소(역거래)는 원 거래와 같은 trans_type에 trans_kind만 반대로 넣고 org_trans_id로 원 거래를 가리킨다.
--
-- 이미 데이터가 있어도 안전하게 변환하고(qty = 입고+출고 중 양수 쪽, trans_kind는 입고>0이면 I), 다시 실행해도 무해하다
-- (컬럼이 이미 바뀌었으면 건너뜀). 새 컬럼을 참조하는 문장은 EXEC로 감싸서 컴파일 시점 컬럼 검사를 피한다.

-- ============================================================
-- 1) TMASTOCK.last_trans_id -> trans_id
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMASTOCK') AND name = 'last_trans_id')
    EXEC sp_rename 'TMASTOCK.last_trans_id', 'trans_id', 'COLUMN';
GO

-- ============================================================
-- 2) TMATRANS: in_qty/out_qty -> qty + trans_kind
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'in_qty')
BEGIN
    -- 컬럼을 바꾸려면 이 컬럼을 포함하는 인덱스와 제약을 먼저 내려야 한다
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'IX_TMATRANS_item')
        DROP INDEX IX_TMATRANS_item ON TMATRANS;
    IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'CK_TMATRANS_qty')
        ALTER TABLE TMATRANS DROP CONSTRAINT CK_TMATRANS_qty;

    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'qty')
        ALTER TABLE TMATRANS ADD qty NUMERIC(18,4) NULL, trans_kind VARCHAR(1) NULL;

    EXEC(N'UPDATE TMATRANS SET qty = CASE WHEN in_qty > 0 THEN in_qty ELSE out_qty END,
                              trans_kind = CASE WHEN in_qty > 0 THEN ''I'' ELSE ''O'' END
           WHERE qty IS NULL');
    EXEC(N'ALTER TABLE TMATRANS ALTER COLUMN qty NUMERIC(18,4) NOT NULL');
    EXEC(N'ALTER TABLE TMATRANS ALTER COLUMN trans_kind VARCHAR(1) NOT NULL');

    IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'DF_TMATRANS_in')
        ALTER TABLE TMATRANS DROP CONSTRAINT DF_TMATRANS_in;
    IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'DF_TMATRANS_out')
        ALTER TABLE TMATRANS DROP CONSTRAINT DF_TMATRANS_out;
    ALTER TABLE TMATRANS DROP COLUMN in_qty, out_qty;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'qty')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'CK_TMATRANS_qty')
        EXEC(N'ALTER TABLE TMATRANS ADD CONSTRAINT CK_TMATRANS_qty CHECK (qty > 0)');            -- 수량은 항상 양수(방향은 trans_kind)
    IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'CK_TMATRANS_kind')
        EXEC(N'ALTER TABLE TMATRANS ADD CONSTRAINT CK_TMATRANS_kind CHECK (trans_kind IN (''I'', ''O''))');
END
GO

-- ============================================================
-- 3) TMATRANS.trans_type_cd -> trans_type
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'trans_type_cd')
    EXEC sp_rename 'TMATRANS.trans_type_cd', 'trans_type', 'COLUMN';
GO

-- 수불부 조회용 인덱스 재생성(컬럼 구성이 바뀌었으므로)
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'qty')
   AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'IX_TMATRANS_item')
    EXEC(N'CREATE NONCLUSTERED INDEX IX_TMATRANS_item ON TMATRANS (acc_id, item_id, wh_id, trans_date)
           INCLUDE (qty, trans_kind, trans_type, stock_upd_yn)');
GO

-- 컬럼 설명(스키마 조회/툴팁용)
IF NOT EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID('TMATRANS') AND minor_id = COLUMNPROPERTY(OBJECT_ID('TMATRANS'), 'trans_kind', 'ColumnId') AND name = 'MS_Description')
    EXEC sp_addextendedproperty 'MS_Description', N'수불구분: I 입고 / O 출고', 'SCHEMA', 'dbo', 'TABLE', 'TMATRANS', 'COLUMN', 'trans_kind';
IF NOT EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID('TMATRANS') AND minor_id = COLUMNPROPERTY(OBJECT_ID('TMATRANS'), 'trans_type', 'ColumnId') AND name = 'MS_Description')
    EXEC sp_addextendedproperty 'MS_Description', N'수불유형: 구매입고/기타입고/이동입고/이동출고/생산입고 등(제안값은 195번 파일 머리말)', 'SCHEMA', 'dbo', 'TABLE', 'TMATRANS', 'COLUMN', 'trans_type';
IF NOT EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID('TMATRANS') AND minor_id = COLUMNPROPERTY(OBJECT_ID('TMATRANS'), 'qty', 'ColumnId') AND name = 'MS_Description')
    EXEC sp_addextendedproperty 'MS_Description', N'수불수량(항상 양수, 방향은 trans_kind)', 'SCHEMA', 'dbo', 'TABLE', 'TMATRANS', 'COLUMN', 'qty';
GO
