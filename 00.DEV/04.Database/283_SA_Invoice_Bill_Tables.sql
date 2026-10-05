-- 영업(SA) 프로세스 개편 (2026-10-05): 수주 -> 거래명세서 -> 출고(등록/확정=수불) , 거래명세서 -> 매출(세금계산서)
--  TSAINVCM/D 거래명세서  : 고객 기준 서류. 수주 라인 1건당 1행, 단가 수정 가능. 실제 출고와 무관하게 발행 가능.
--                           확정 -> 수주 라인 next_qty(=확정 명세서 수량 합계) 갱신. 출고확정 -> gi_qty, 매출확정 -> bill_qty (서버가 합계로 재계산).
--  TSABILLM/D 매출(계산서) : 고객 기준. 명세서 라인에서 불러와 부분매출 가능(월합계 계산서 = 같은 고객의 여러 명세서 라인).
--  TSAGID                 : invc_id/invc_serl 추가 - 출고 품목은 이제 명세서 라인에서 불러온다.
--  기존 출하 데이터(확정 0건, 재고수불 0건)는 모두 삭제한다 (사용자 요청 2026-10-05).
-- 적용 대상: WYNLAB_DEV, FADU 양쪽 DB.

-- ===== 기존 출하 데이터 삭제 (확정된 출하나 수불이 있으면 중단) =====
IF EXISTS (SELECT 1 FROM TSAGIM WHERE stat_cd = 'C') OR EXISTS (SELECT 1 FROM TMATRANS WHERE src_type = 'GI')
    RAISERROR(N'확정된 출하 또는 출하 수불이 있어 삭제를 중단합니다. 먼저 확정취소하세요.', 16, 1);
ELSE
BEGIN
    DELETE FROM TSAGIL;
    DELETE FROM TSAGID;
    DELETE FROM TSAGIM;
END
GO

-- ===== 거래명세서 =====
IF OBJECT_ID('TSAINVCM') IS NULL
BEGIN
    CREATE TABLE TSAINVCM (
        invc_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        invc_no        VARCHAR(20)    NOT NULL,
        invc_date      VARCHAR(8)     NULL,
        cust_id        BIGINT         NULL,
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        stat_cd        VARCHAR(10)    NULL,                 -- 0 작성, C 확정 (L_MA0002)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(50)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAINVCM PRIMARY KEY CLUSTERED (invc_id)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TSAINVCM_no ON TSAINVCM (acc_id, invc_no);
    CREATE NONCLUSTERED INDEX IX_TSAINVCM_cust ON TSAINVCM (cust_id, invc_date);
END
GO

IF OBJECT_ID('TSAINVCD') IS NULL
BEGIN
    CREATE TABLE TSAINVCD (
        invc_id        BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        invc_no        VARCHAR(20)    NULL,
        so_id          BIGINT         NOT NULL,
        so_no          VARCHAR(20)    NULL,
        so_serl        INT            NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        qty            NUMERIC(18,4)  NOT NULL,
        gi_qty         NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TSAINVCD_gi DEFAULT 0,     -- 출고(확정) 누계
        bill_qty       NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TSAINVCD_bill DEFAULT 0,   -- 매출(확정) 누계
        cur_cd         VARCHAR(10)    NULL,
        exc_rate       NUMERIC(18,4)  NULL,
        price          NUMERIC(18,4)  NULL,                 -- 수정 가능(수주 단가로 시작)
        amt            NUMERIC(18,4)  NULL,
        vat_type       VARCHAR(10)    NULL,
        vat_rate       NUMERIC(9,4)   NULL,
        vat            NUMERIC(18,4)  NULL,
        total_amt      NUMERIC(18,4)  NULL,
        kor_price      NUMERIC(18,4)  NULL,
        kor_amt        NUMERIC(18,4)  NULL,
        kor_vat        NUMERIC(18,4)  NULL,
        kor_total_amt  NUMERIC(18,4)  NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAINVCD PRIMARY KEY CLUSTERED (invc_id, serl),
        CONSTRAINT CK_TSAINVCD_qty CHECK (qty > 0)
    );
    CREATE NONCLUSTERED INDEX IX_TSAINVCD_so ON TSAINVCD (so_id, so_serl) INCLUDE (qty);
END
GO

-- ===== 매출(계산서) =====
IF OBJECT_ID('TSABILLM') IS NULL
BEGIN
    CREATE TABLE TSABILLM (
        bill_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        bill_no        VARCHAR(20)    NOT NULL,
        bill_date      VARCHAR(8)     NULL,
        cust_id        BIGINT         NULL,
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        tax_inv_no     NVARCHAR(50)   NULL,                 -- 세금계산서 번호
        tax_inv_date   VARCHAR(8)     NULL,                 -- 세금계산서 발행일
        stat_cd        VARCHAR(10)    NULL,                 -- 0 작성, C 확정
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(50)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSABILLM PRIMARY KEY CLUSTERED (bill_id)
    );
    CREATE UNIQUE NONCLUSTERED INDEX UX_TSABILLM_no ON TSABILLM (acc_id, bill_no);
    CREATE NONCLUSTERED INDEX IX_TSABILLM_cust ON TSABILLM (cust_id, bill_date);
END
GO

IF OBJECT_ID('TSABILLD') IS NULL
BEGIN
    CREATE TABLE TSABILLD (
        bill_id        BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        bill_no        VARCHAR(20)    NULL,
        invc_id        BIGINT         NOT NULL,
        invc_serl      INT            NOT NULL,
        so_id          BIGINT         NULL,
        so_serl        INT            NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        qty            NUMERIC(18,4)  NOT NULL,             -- 부분매출 가능(명세서 수량 이하)
        cur_cd         VARCHAR(10)    NULL,
        exc_rate       NUMERIC(18,4)  NULL,
        price          NUMERIC(18,4)  NULL,                 -- 명세서 단가 스냅샷
        amt            NUMERIC(18,4)  NULL,
        vat_type       VARCHAR(10)    NULL,
        vat_rate       NUMERIC(9,4)   NULL,
        vat            NUMERIC(18,4)  NULL,
        total_amt      NUMERIC(18,4)  NULL,
        kor_price      NUMERIC(18,4)  NULL,
        kor_amt        NUMERIC(18,4)  NULL,
        kor_vat        NUMERIC(18,4)  NULL,
        kor_total_amt  NUMERIC(18,4)  NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(50)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(50)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSABILLD PRIMARY KEY CLUSTERED (bill_id, serl),
        CONSTRAINT CK_TSABILLD_qty CHECK (qty > 0)
    );
    CREATE NONCLUSTERED INDEX IX_TSABILLD_invc ON TSABILLD (invc_id, invc_serl) INCLUDE (qty);
END
GO

-- 출고 품목 -> 명세서 라인 연결
IF COL_LENGTH('TSAGID', 'invc_id') IS NULL
BEGIN
    ALTER TABLE TSAGID ADD invc_id BIGINT NULL, invc_serl INT NULL;
    CREATE NONCLUSTERED INDEX IX_TSAGID_invc ON TSAGID (invc_id, invc_serl);
END
GO

-- 채번 IV(명세서) / BL(매출)
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
SELECT 'TSAINVCM', N'거래명세서', 'IV', 'invc_no', 'YYMM', 4, 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TSAINVCM');
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
SELECT 'TSABILLM', N'매출', 'BL', 'bill_no', 'YYMM', 4, 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TSABILLM');
GO
