-- 구매 프로세스 P1/P2 - 납품(DELV)과 수입검사(IQC) 테이블/공통코드/채번/콤보(2026-09-25).
--
-- 흐름: 발주(TMAPOD) -> 납품(TMADELVD) -> [검사대상이면] 수입검사(TMAIQCD) -> 입고대기(VMA_GR_READY, 입고는 별도 설계).
-- 문서 사이는 기존 발주 테이블과 같은 src_type/src_id/src_serl(원천 추적) + next_qty(다음 단계 처리량) 규칙으로 잇는다.
-- 기존 구매 테이블(TMAPOM/TMAPOD)과 같은 규칙: PK는 IDENTITY, 사업장(acc_id) 보유, 등록/수정 이력 컬럼,
-- FK 제약 없음, 날짜는 VARCHAR(8) yyyyMMdd. 금액 컬럼은 두지 않는다 - 단가/금액은 발주 라인에만 있고 매입 단계에서
-- src 체인으로 조회한다.

-- ============================================================
-- 1) 납품 헤더/라인
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMADELVM')
BEGIN
    CREATE TABLE TMADELVM (
        delv_id        BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        delv_no        VARCHAR(20)    NOT NULL,
        delv_date      VARCHAR(8)     NULL,                 -- 납품(접수)일
        cust_id        BIGINT         NOT NULL,             -- 협력사. 라인의 발주 협력사와 같아야 한다
        vendor_doc_no  NVARCHAR(50)   NULL,                 -- 협력사 납품서/거래명세서 번호
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,                 -- 접수 담당
        stat_cd        VARCHAR(10)    NULL,                 -- MA0005 (0 작성, C 확정)
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
        CONSTRAINT PK_TMADELVM PRIMARY KEY CLUSTERED (delv_id)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMADELVM_no ON TMADELVM (acc_id, delv_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMADELVD')
BEGIN
    CREATE TABLE TMADELVD (
        delv_id        BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        delv_no        VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        delv_qty       NUMERIC(18,4)  NOT NULL,             -- 납품수량
        next_qty       NUMERIC(18,4)  NULL,                 -- 다음 단계 처리량: 검사대상=확정된 검사수량 합 / 무검사=입고수량 합(입고 설계가 갱신)
        lot_no         NVARCHAR(50)   NULL,                 -- TBAITEM.lot_yn='Y' 품목은 필수
        qc_yn          VARCHAR(1)     NULL,                 -- 검사대상 여부(불러올 때 발주 라인 값을 복사, 이후 마스터가 바뀌어도 이 납품은 유지)
        wh_id          BIGINT         NULL,                 -- 입고 예정 창고/위치(발주 값이 기본)
        loc_id         BIGINT         NULL,
        src_type       VARCHAR(10)    NULL,                 -- PO
        src_id         BIGINT         NULL,                 -- po_id
        src_no         VARCHAR(20)    NULL,                 -- po_no
        src_serl       INT            NULL,                 -- 발주 serl
        stop_yn        VARCHAR(1)     NOT NULL CONSTRAINT DF_TMADELVD_stop_yn DEFAULT ('N'),
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMADELVD PRIMARY KEY CLUSTERED (delv_id, serl)
    );

    -- 발주 라인 next_qty 재계산(원천 기준 합계) 용
    CREATE NONCLUSTERED INDEX IX_TMADELVD_src ON TMADELVD (src_type, src_id, src_serl) INCLUDE (delv_qty);
END
GO

-- ============================================================
-- 2) 수입검사 헤더/라인
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAIQCM')
BEGIN
    CREATE TABLE TMAIQCM (
        iqc_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        iqc_no         VARCHAR(20)    NOT NULL,
        iqc_date       VARCHAR(8)     NULL,                 -- 검사일
        cust_id        BIGINT         NOT NULL,             -- 협력사(납품에서 복사)
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,                 -- 검사 담당
        stat_cd        VARCHAR(10)    NULL,                 -- MA0006 (0 작성, C 확정)
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
        CONSTRAINT PK_TMAIQCM PRIMARY KEY CLUSTERED (iqc_id)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMAIQCM_no ON TMAIQCM (acc_id, iqc_no);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMAIQCD')
BEGIN
    CREATE TABLE TMAIQCD (
        iqc_id           BIGINT         NOT NULL,
        serl             INT            NOT NULL,
        acc_id           BIGINT         NOT NULL,
        iqc_no           VARCHAR(20)    NOT NULL,
        item_id          BIGINT         NOT NULL,
        unit_cd          VARCHAR(10)    NULL,
        lot_no           NVARCHAR(50)   NULL,
        insp_qty         NUMERIC(18,4)  NOT NULL,             -- 판정 대상 수량(납품 미검사 잔량 이하)
        sample_qty       NUMERIC(18,4)  NULL,                 -- 실제 검사한 표본 수(전수검사면 insp_qty와 같음, 참고용)
        pass_qty         NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TMAIQCD_pass DEFAULT (0),   -- 합격
        conc_qty         NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TMAIQCD_conc DEFAULT (0),   -- 특채(조건부 합격, 입고 가능)
        fail_qty         NUMERIC(18,4)  NOT NULL CONSTRAINT DF_TMAIQCD_fail DEFAULT (0),   -- 불합격
        fail_reason_cd   VARCHAR(10)    NULL,                 -- MA0007 불량유형
        fail_action_cd   VARCHAR(10)    NULL,                 -- MA0008 불합격 처분 (RET 반품 / SCRAP 폐기)
        next_qty         NUMERIC(18,4)  NULL,                 -- 입고 처리 수량 합(입고 설계가 갱신)
        wh_id            BIGINT         NULL,
        loc_id           BIGINT         NULL,
        src_type         VARCHAR(10)    NULL,                 -- DELV
        src_id           BIGINT         NULL,                 -- delv_id
        src_no           VARCHAR(20)    NULL,                 -- delv_no
        src_serl         INT            NULL,                 -- 납품 serl
        remark           NVARCHAR(3000) NULL,                 -- 특채 사유(conc_qty>0이면 필수)
        reg_user_id      VARCHAR(30)    NULL,
        reg_dt           DATETIME       NULL,
        reg_pc           NVARCHAR(200)  NULL,
        upt_user_id      VARCHAR(30)    NULL,
        upt_dt           DATETIME       NULL,
        upt_pc           NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMAIQCD PRIMARY KEY CLUSTERED (iqc_id, serl),
        CONSTRAINT CK_TMAIQCD_sum CHECK (pass_qty + conc_qty + fail_qty = insp_qty)
    );

    CREATE NONCLUSTERED INDEX IX_TMAIQCD_src ON TMAIQCD (src_type, src_id, src_serl) INCLUDE (insp_qty, fail_qty, fail_action_cd);
END
GO

-- ============================================================
-- 3) 공통코드 - MA0004(원천구분)에 DELV/IQC 추가(PO/POREQ는 이미 있음), MA0005~MA0008 신규
--    (MA0009=구매 입고방식은 188번에서 이미 등록)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'DELV')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0004', 'DELV', N'납품', 3, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'IQC')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0004', 'IQC', N'수입검사', 4, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0005')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0005', N'납품 진행상태', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0006')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0006', N'수입검사 진행상태', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0007')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0007', N'불량유형', 'N', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0008')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0008', N'불합격 처분', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0005')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0005', '0', N'작성', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0005', 'C', N'확정', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0006')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0006', '0', N'작성', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0006', 'C', N'확정', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
-- 불량유형은 초기값이며 코드관리 화면에서 편집한다(sys_yn='N').
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0007')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0007', 'DIM',  N'치수불량', 1, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0007', 'APR',  N'외관불량', 2, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0007', 'SHORT', N'수량부족', 3, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0007', 'PERF', N'성능불량', 4, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0007', 'PACK', N'포장불량', 5, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0007', 'ETC',  N'기타',     9, 'N', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0008')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0008', 'RET',   N'반품(재납품 대상)', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0008', 'SCRAP', N'폐기',              2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 4) 콤보(LookUp) - 기존 L_MA0001~4와 같은 방식(sysLookupM, 공통코드 사용 Y 항목)
-- ============================================================
DECLARE @keys TABLE (k VARCHAR(30), nm NVARCHAR(100), major VARCHAR(20));
INSERT INTO @keys VALUES
    ('L_MA0005', N'납품진행상태', 'MA0005'),
    ('L_MA0006', N'수입검사진행상태', 'MA0006'),
    ('L_MA0007', N'불량유형', 'MA0007'),
    ('L_MA0008', N'불합격처분', 'MA0008');

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT k.k, NULL, k.nm, 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''' + k.major + N'''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
FROM @keys k
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = k.k);
GO

-- ============================================================
-- 5) 채번 - 납품(DV)/수입검사(IQ). SSP_SYS_GetAutoKey는 설정이 없으면 접두사 없이 자동 등록해버리므로
--    원하는 접두사를 먼저 심어둔다(173번과 같은 방식).
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMADELVM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMADELVM', N'납품마스터', 'DV', 'delv_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAIQCM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAIQCM', N'수입검사마스터', 'IQ', 'iqc_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 6) 초과 납품 허용율 설명 보정 - 발주 "수량" 기준으로 구현한다(누적 납품이 발주수량*(1+허용율)까지).
-- ============================================================
UPDATE TSMPROCCONFIG
SET description = N'발주수량 대비 이 비율까지 초과 납품을 받습니다(예: 5이면 발주 100개에 105개까지). 0이면 초과 납품 불가입니다. 변경 이후 저장/확정하는 납품부터 적용됩니다.'
WHERE config_key = 'MA.OVER_DELV_PCT';
GO
