-- 구매요청(TMAPOREQM/TMAPOREQD)/구매발주(TMAPOM/TMAPOD) 테이블 - 사장님이 라이브 DB에 직접
-- 생성해뒀던 것을 마이그레이션 파일로 정식 기록한다(2026-09-22). TMAPOREQD.wh_id/loc_id는
-- 원래 varchar(20)였는데 TBAWH/TBALOC 실제 키(bigint)와 안 맞아 이 작업 중 bigint로 고쳤다 -
-- 이 파일은 "이미 반영된 라이브 상태"를 기록하는 것이라 CREATE TABLE 자체는 멱등 가드만 건다.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAPOREQM')
BEGIN
    CREATE TABLE TMAPOREQM (
        req_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id        BIGINT         NOT NULL,
        req_no        VARCHAR(20)    NOT NULL,
        req_date      VARCHAR(8)     NULL,
        req_title     NVARCHAR(200)  NULL,
        stat_cd       VARCHAR(10)    NULL,
        po_type       VARCHAR(10)    NULL,
        cust_id       BIGINT         NULL,
        dept_id       BIGINT         NULL,
        emp_id        BIGINT         NULL,
        pjt_id        BIGINT         NULL,
        cur_cd        VARCHAR(20)    NULL,
        cfm_yn        VARCHAR(1)     NULL,
        cfm_dt        DATETIME       NULL,
        cfm_user_id   VARCHAR(30)    NULL,
        stop_yn       VARCHAR(1)     NULL,
        stop_dt       DATETIME       NULL,
        stop_user_id  VARCHAR(30)    NULL,
        app_id        BIGINT         NULL,
        app_no        VARCHAR(20)    NULL,
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAPOREQM PRIMARY KEY CLUSTERED (req_id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAPOREQD')
BEGIN
    CREATE TABLE TMAPOREQD (
        req_id        BIGINT         NOT NULL,
        serl          INT            NOT NULL,
        acc_id        BIGINT         NOT NULL,
        req_no        VARCHAR(20)    NOT NULL,
        item_id       BIGINT         NULL,
        qty           NUMERIC(18,4)  NULL,
        next_qty      NUMERIC(18,4)  NULL,
        unit_cd       VARCHAR(10)    NULL,
        cfm_yn        VARCHAR(1)     NULL,
        stop_yn       VARCHAR(1)     NULL,
        cust_id       BIGINT         NULL,
        delv_date     VARCHAR(8)     NULL,
        wh_id         BIGINT         NULL,
        loc_id        BIGINT         NULL,
        src_type      VARCHAR(10)    NULL,
        src_id        BIGINT         NULL,
        src_no        VARCHAR(20)    NULL,
        src_serl      INT            NULL,
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAPOREQD PRIMARY KEY CLUSTERED (req_id, serl)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAPOM')
BEGIN
    CREATE TABLE TMAPOM (
        po_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id        BIGINT         NOT NULL,
        po_no         VARCHAR(20)    NOT NULL,
        po_date       VARCHAR(8)     NULL,
        cfm_yn        VARCHAR(1)     NULL,
        cfm_dt        DATETIME       NULL,
        cmf_user_id   VARCHAR(30)    NULL,
        stop_yn       VARCHAR(1)     NULL,
        stop_dt       DATETIME       NULL,
        stop_user_id  VARCHAR(30)    NULL,
        stat_cd       VARCHAR(10)    NULL,
        dept_id       BIGINT         NULL,
        emp_id        BIGINT         NULL,
        po_type       VARCHAR(10)    NULL,
        po_title      NVARCHAR(1000) NULL,
        pjt_id        BIGINT         NULL,
        cust_id       BIGINT         NULL,
        cur_cd        VARCHAR(10)    NULL,
        exc_rate      NUMERIC(18,4)  NULL,
        delv_date     VARCHAR(8)     NULL,
        vat_type      VARCHAR(10)    NULL,
        vat_rate      NUMERIC(9,4)   NULL,
        app_id        BIGINT         NULL,
        app_no        VARCHAR(20)    NULL,
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAPOM PRIMARY KEY CLUSTERED (po_id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAPOD')
BEGIN
    CREATE TABLE TMAPOD (
        po_id           BIGINT         NOT NULL,
        serl            INT            NOT NULL,
        acc_id          BIGINT         NOT NULL,
        po_no           VARCHAR(20)    NOT NULL,
        po_type         VARCHAR(10)    NOT NULL,
        item_id         BIGINT         NOT NULL,
        unit_cd         VARCHAR(10)    NULL,
        qty             NUMERIC(18,4)  NULL,
        next_qty        NUMERIC(18,4)  NULL,
        price           NUMERIC(18,4)  NULL,
        amt             NUMERIC(18,4)  NULL,
        vat             NUMERIC(18,4)  NULL,
        total_amt       NUMERIC(18,4)  NULL,
        kor_price       NUMERIC(18,4)  NULL,
        kor_amt         NUMERIC(18,4)  NULL,
        kor_vat         NUMERIC(18,4)  NULL,
        kor_total_amt   NUMERIC(18,4)  NULL,
        vat_type        VARCHAR(10)    NULL,
        vat_rate        NUMERIC(9,4)   NULL,
        delv_date       VARCHAR(8)     NULL,
        wh_id           BIGINT         NULL,
        loc_id          BIGINT         NULL,
        qc_yn           VARCHAR(1)     NULL,
        stock_yn        VARCHAR(1)     NULL,
        stock_unit_cd   VARCHAR(10)    NULL,
        stock_unit_qty  NUMERIC(18,4)  NULL,
        pjt_id          BIGINT         NULL,
        src_type        VARCHAR(10)    NULL,
        src_id          BIGINT         NULL,
        src_no          VARCHAR(20)    NULL,
        src_serl        INT            NULL,
        stop_yn         VARCHAR(1)     NOT NULL DEFAULT ('N'),
        stop_emp_no     VARCHAR(20)    NULL,
        stop_remark     NVARCHAR(1000) NULL,
        remark          NVARCHAR(3000) NULL,
        reg_user_id     VARCHAR(30)    NULL,
        reg_dt          DATETIME       NULL,
        reg_pc          NVARCHAR(200)  NULL,
        upt_user_id     VARCHAR(30)    NULL,
        upt_dt          DATETIME       NULL,
        upt_pc          NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAPOD PRIMARY KEY CLUSTERED (po_id, serl)
    );
END
GO
