-- TMATRANS.stock_upd_yn -> stock_yn (2026-09-25, 사장님 지시). 발주 라인(TMAPOD.stock_yn)/품목(TBAITEM.stock_yn)과 같은 이름으로 맞춘다.
-- 의미는 그대로: 이 수불이 현재고(TMASTOCK)를 갱신하는지 여부의 스냅샷 - 재고관리 품목이면 Y.
-- 컬럼을 참조하는 CHECK/기본값 제약이 있으면 sp_rename이 거부되므로(오류 15336) 제약을 내렸다가 새 이름으로 다시 만든다.
-- 인덱스(IX_TMATRANS_item)는 SQL Server가 컬럼 이름 변경을 자동으로 따라간다. 다시 실행해도 무해하다.

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'stock_upd_yn')
BEGIN
    IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'CK_TMATRANS_upd')
        ALTER TABLE TMATRANS DROP CONSTRAINT CK_TMATRANS_upd;
    IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'DF_TMATRANS_upd')
        ALTER TABLE TMATRANS DROP CONSTRAINT DF_TMATRANS_upd;

    EXEC sp_rename 'TMATRANS.stock_upd_yn', 'stock_yn', 'COLUMN';
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TMATRANS') AND name = 'stock_yn')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'DF_TMATRANS_stock')
        EXEC(N'ALTER TABLE TMATRANS ADD CONSTRAINT DF_TMATRANS_stock DEFAULT (''Y'') FOR stock_yn');
    IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('TMATRANS') AND name = 'CK_TMATRANS_stock')
        EXEC(N'ALTER TABLE TMATRANS ADD CONSTRAINT CK_TMATRANS_stock CHECK (stock_yn IN (''Y'', ''N''))');
END
GO
