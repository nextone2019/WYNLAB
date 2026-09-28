-- 구매단가(TMAPOPRICE) - 품목별 / 거래처별 / 기간별 구매단가 관리(2026-09-25).
--
-- 적용 규칙:
--  * cust_id가 NULL이면 "전체 거래처 공통 단가", 값이 있으면 그 거래처에만 적용되는 단가.
--    발주 시 단가를 찾을 때는 (해당 거래처 단가) -> (공통 단가) 순으로 우선한다(조회 프로시저에서 처리).
--  * 적용기간은 start_date ~ end_date(VARCHAR(8), yyyyMMdd, 양끝 포함). 종료일이 정해지지 않은 단가는
--    NULL이 아니라 '99991231'로 둔다 - BETWEEN/비교 조건과 겹침 검사가 단순해진다.
--  * 같은 (사업장, 품목, 거래처(또는 공통), 시작일)은 한 건만 - 유니크 인덱스. SQL Server 유니크 인덱스는
--    NULL끼리도 같은 값으로 취급하므로 공통 단가(cust_id NULL)도 같은 규칙으로 막힌다.
--  * 기간이 겹치는 단가(같은 품목/거래처 범위에서 시작일이 다르지만 기간이 겹침)는 인덱스로 막을 수
--    없어 저장 프로시저에서 검증한다.
--
-- 다른 구매 테이블(TMAPOM/TMAPOD)과 같은 규칙: PK는 IDENTITY, 사업장(acc_id) 보유, 등록/수정 이력
-- (reg_*/upt_*) 컬럼, FK 제약은 두지 않는다.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAPOPRICE')
BEGIN
    CREATE TABLE TMAPOPRICE (
        price_id      BIGINT IDENTITY(1,1) NOT NULL,
        acc_id        BIGINT         NOT NULL,                       -- 사업장
        item_id       BIGINT         NOT NULL,                       -- 품목(TBAITEM.item_id)
        cust_id       BIGINT         NULL,                           -- 거래처(TBACUST.cust_id). NULL = 전체 거래처 공통
        start_date    VARCHAR(8)     NOT NULL,                       -- 적용 시작일(yyyyMMdd)
        end_date      VARCHAR(8)     NOT NULL CONSTRAINT DF_TMAPOPRICE_end_date DEFAULT '99991231', -- 적용 종료일(yyyyMMdd)
        cur_cd        VARCHAR(10)    NULL,                           -- 통화(L_CM0003)
        unit_cd       VARCHAR(10)    NULL,                           -- 단가 기준 단위(발주단위)
        price         NUMERIC(18,4)  NOT NULL,                       -- 구매단가
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAPOPRICE PRIMARY KEY CLUSTERED (price_id),
        CONSTRAINT CK_TMAPOPRICE_period CHECK (start_date <= end_date)
    );

    -- 같은 품목/거래처(또는 공통)/시작일 중복 방지
    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAPOPRICE_key
        ON TMAPOPRICE (acc_id, item_id, cust_id, start_date);

    -- 발주 시 단가 조회용: 품목 + 기준일(start_date/end_date)로 찾고 거래처 여부를 같이 본다
    CREATE NONCLUSTERED INDEX IX_TMAPOPRICE_lookup
        ON TMAPOPRICE (acc_id, item_id, start_date, end_date)
        INCLUDE (cust_id, price, cur_cd, unit_cd);
END
GO
