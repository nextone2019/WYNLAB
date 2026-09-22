/* ---------- TBACUST/TBACUSTPRSN/TBACUSTACNT: CUST_CD(varchar) -> CUST_ID(bigint identity) 전환 -----------
   TBADEPT(080)와 동일한 패턴. 세 테이블 모두 현재 0건이라 데이터 이관 없이 스키마만 바꾼다.
   TBAITEM.cust_cd도 CUST_ID로 교체한다(0건이라 단순 컬럼 교체).
   TBACUSTTYPE.cust_cd는 어떤 프로시저/화면에서도 참조하지 않는 미사용 테이블이라 이번 마이그레이션
   대상에서 제외한다(건드릴 이유가 없음). */

-- ===================== 1) TBACUST =====================
DECLARE @pkCust NVARCHAR(200) = (SELECT kc.name FROM sys.key_constraints kc WHERE kc.parent_object_id = OBJECT_ID('TBACUST') AND kc.type = 'PK');
EXEC sp_rename @pkCust, 'PK_TBACUST_OLD_STRCD_20260906';
EXEC sp_rename 'TBACUST', 'TBACUST_OLD_STRCD_20260906';
GO

CREATE TABLE TBACUST (
    CUST_ID bigint IDENTITY(1,1) NOT NULL,
    cust_nm nvarchar(100) NULL,
    biz_no varchar(30) NULL,
    tel varchar(30) NULL,
    cur_cd varchar(3) NULL,
    owner_nm nvarchar(100) NULL,
    zip_code varchar(20) NULL,
    addr1 nvarchar(1000) NULL,
    addr2 nvarchar(1000) NULL,
    homepage nvarchar(200) NULL,
    email nvarchar(100) NULL,
    fax varchar(30) NULL,
    biz_kind nvarchar(200) NULL,
    biz_type nvarchar(200) NULL,
    trans_open_date varchar(8) NULL,
    vat_type varchar(10) NULL,
    vat_rate numeric(19, 2) NULL,
    remark nvarchar(3000) NULL,
    stat_cd varchar(10) NULL,
    reg_user_id varchar(50) NULL,
    reg_dt datetime NULL,
    reg_pc nvarchar(200) NULL,
    upt_user_id varchar(50) NULL,
    upt_dt datetime NULL,
    upt_pc nvarchar(200) NULL,
    emp_no varchar(20) NULL,
    CONSTRAINT PK_TBACUST PRIMARY KEY (CUST_ID)
);
GO

-- ===================== 2) TBACUSTPRSN (담당자정보) =====================
DECLARE @pkPrsn NVARCHAR(200) = (SELECT kc.name FROM sys.key_constraints kc WHERE kc.parent_object_id = OBJECT_ID('TBACUSTPRSN') AND kc.type = 'PK');
EXEC sp_rename @pkPrsn, 'PK_TBACUSTPRSN_OLD_STRCD_20260906';
EXEC sp_rename 'TBACUSTPRSN', 'TBACUSTPRSN_OLD_STRCD_20260906';
GO

CREATE TABLE TBACUSTPRSN (
    CUST_ID bigint NOT NULL,
    serl int NOT NULL,
    prsn_nm nvarchar(50) NOT NULL,
    grade nvarchar(50) NULL,
    tel1 varchar(30) NULL,
    tel2 varchar(30) NULL,
    fax varchar(30) NULL,
    email varchar(30) NULL,
    reg_user_id varchar(50) NOT NULL,
    reg_dt datetime NULL,
    reg_pc nvarchar(200) NULL,
    upt_user_id varchar(50) NULL,
    upt_dt datetime NULL,
    upt_pc nvarchar(200) NULL,
    CONSTRAINT PK_TBACUSTPRSN PRIMARY KEY (CUST_ID, serl)
);
GO

-- ===================== 3) TBACUSTACNT (계좌정보) =====================
DECLARE @pkAcnt NVARCHAR(200) = (SELECT kc.name FROM sys.key_constraints kc WHERE kc.parent_object_id = OBJECT_ID('TBACUSTACNT') AND kc.type = 'PK');
EXEC sp_rename @pkAcnt, 'PK_TBACUSTACNT_OLD_STRCD_20260906';
EXEC sp_rename 'TBACUSTACNT', 'TBACUSTACNT_OLD_STRCD_20260906';
GO

CREATE TABLE TBACUSTACNT (
    CUST_ID bigint NOT NULL,
    serl int NOT NULL,
    bank_cd varchar(20) NOT NULL,
    acnt_no varchar(50) NOT NULL,
    remark nvarchar(200) NULL,
    reg_user_id varchar(50) NOT NULL,
    reg_dt datetime NULL,
    reg_pc nvarchar(200) NULL,
    upt_user_id varchar(50) NULL,
    upt_dt datetime NULL,
    upt_pc nvarchar(200) NULL,
    CONSTRAINT PK_TBACUSTACNT PRIMARY KEY (CUST_ID, serl)
);
GO

-- ===================== 4) TBAITEM.cust_cd -> CUST_ID (0건이라 단순 컬럼 교체) =====================
ALTER TABLE TBAITEM ADD CUST_ID bigint NULL;
GO
ALTER TABLE TBAITEM DROP COLUMN cust_cd;
GO
