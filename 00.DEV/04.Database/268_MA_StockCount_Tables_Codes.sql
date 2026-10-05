-- 재고실사 테이블 + 공통코드 + 채번 + 콤보 (2026-10-04, WYNLAB_DEV 전용). 설계: Document\재고실사_설계서.md
--
-- 흐름: 작성(0) -> 대상확정(1, 그 시점 TMASTOCK을 라인으로 스냅샷) -> 실사중(2, 수량 입력) -> 입력완료(3, 차이 계산·고정) -> 확정(C) / 취소(X)
-- 확정 = 차이(diff_qty)가 있는 라인마다 수불(TMATRANS, ADJ_IN 'I' / ADJ_OUT 'O', src_type='STKCNT') 생성 + 현재고 반영.
-- 규칙 - 선별(스냅샷)된 라인은 삭제하지 않는 한 전부 실사수량을 입력해야 하고(0 유효, 빈칸 = 미입력) 전부 반영된다. 장부와 같으면 차이 0(수불 없음).
-- 구조는 기타출고(258)와 같은 관례: 헤더 <base>M / 라인 <base>D, 라인 PK (헤더id, serl), 문서번호 사업장별 유니크,
-- 확정으로 만든 수불은 라인 trans_id에 보관(확정취소 때 역거래의 원 거래). 여러 번 실행해도 안전하다.

-- ============================================================
-- 1) 실사 헤더 - TMACNTM
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMACNTM')
BEGIN
    CREATE TABLE TMACNTM (
        cnt_id         BIGINT IDENTITY(1,1) NOT NULL,
        acc_id         BIGINT         NOT NULL,
        cnt_no         VARCHAR(20)    NOT NULL,
        cnt_title      NVARCHAR(200)  NULL,                 -- 실사명(결재 제목에도 쓴다)
        cnt_type       VARCHAR(10)    NOT NULL,             -- MA0015 ALL 전수 / CYC 순환 / SPOT 지정
        cnt_date       VARCHAR(8)     NOT NULL,             -- 실사 기준일(조정 수불일자)
        blind_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMACNTM_blind DEFAULT ('N'),   -- 블라인드 카운트(입력완료 전 장부수량 숨김)
        freeze_mode    VARCHAR(1)     NULL,                 -- 대상확정 시점의 수불 통제 방식 스냅샷(현재 B 변동보정만 구현)
        snap_trans_id  BIGINT         NULL,                 -- 스냅샷 시점 TMATRANS 최대 trans_id(이후 수불 = 변동)
        snap_dt        DATETIME       NULL,
        tol_qty        NUMERIC(18,4)  NULL,                 -- 허용 오차(수량) - 표시용
        tol_rate       NUMERIC(9,4)   NULL,                 -- 허용 오차(%) - 표시용
        dept_id        BIGINT         NULL,
        emp_id         BIGINT         NULL,
        stat_cd        VARCHAR(10)    NOT NULL,             -- MA0014 (0 작성 / 1 대상확정 / 2 실사중 / 3 입력완료 / C 확정 / X 취소)
        cfm_yn         VARCHAR(1)     NULL,
        cfm_dt         DATETIME       NULL,
        cfm_user_id    VARCHAR(30)    NULL,
        app_id         BIGINT         NULL,                 -- 전자결재 연결(TAPDOC) - 결재 상신 시
        app_no         VARCHAR(20)    NULL,
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMACNTM PRIMARY KEY CLUSTERED (cnt_id),
        CONSTRAINT CK_TMACNTM_blind CHECK (blind_yn IN ('Y', 'N')),
        CONSTRAINT CK_TMACNTM_stat CHECK (stat_cd IN ('0', '1', '2', '3', 'C', 'X'))
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TMACNTM_no ON TMACNTM (acc_id, cnt_no);
END
GO

-- ============================================================
-- 2) 실사 대상 조건 - TMACNTW (대상확정 때 이 조건으로 TMASTOCK을 라인에 펼친다. 재현성용으로 남긴다)
--    wh_id 필수(이동중 TR 창고는 저장 프로시저가 거부), grp_id = 품목그룹(1~4단 어느 단이든), item_id = 특정 품목. 둘 다 비면 그 창고 전체.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMACNTW')
BEGIN
    CREATE TABLE TMACNTW (
        cnt_id         BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        wh_id          BIGINT         NOT NULL,
        grp_id         BIGINT         NULL,
        item_id        BIGINT         NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMACNTW PRIMARY KEY CLUSTERED (cnt_id, serl)
    );

    CREATE NONCLUSTERED INDEX IX_TMACNTW_wh ON TMACNTW (wh_id) INCLUDE (cnt_id);
END
GO

-- ============================================================
-- 3) 실사 라인 - TMACNTD
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TMACNTD')
BEGIN
    CREATE TABLE TMACNTD (
        cnt_id         BIGINT         NOT NULL,
        serl           INT            NOT NULL,
        acc_id         BIGINT         NOT NULL,
        cnt_no         VARCHAR(20)    NOT NULL,
        item_id        BIGINT         NOT NULL,
        unit_cd        VARCHAR(10)    NULL,
        wh_id          BIGINT         NOT NULL,
        loc_id         BIGINT         NOT NULL CONSTRAINT DF_TMACNTD_loc DEFAULT (0),      -- 0 = 위치 없음(TMASTOCK 키와 같은 값)
        lot_no         NVARCHAR(50)   NOT NULL CONSTRAINT DF_TMACNTD_lot DEFAULT (N''),    -- '' = LOT 없음
        book_qty       NUMERIC(18,4)  NOT NULL,             -- 스냅샷 시점 장부수량
        cnt_qty        NUMERIC(18,4)  NULL,                 -- 초도 실사수량 (NULL = 미입력, 0은 유효한 입력)
        recnt_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMACNTD_recnt DEFAULT ('N'),  -- 재실사 지정 - Y면 recnt_qty를 다시 입력해야 한다
        recnt_qty      NUMERIC(18,4)  NULL,                 -- 재실사 수량
        fin_qty        NUMERIC(18,4)  NULL,                 -- 최종 실사수량 = recnt_yn='Y'면 recnt_qty, 아니면 cnt_qty (저장 프로시저가 유지)
        move_qty       NUMERIC(18,4)  NULL,                 -- 스냅샷 이후 ~ 기준일까지 확정 수불 순증감(입력완료/확정 때 계산·고정)
        diff_qty       NUMERIC(18,4)  NULL,                 -- fin_qty - (book_qty + move_qty). +면 실물이 많음(ADJ_IN), -면 적음(ADJ_OUT)
        add_yn         VARCHAR(1)     NOT NULL CONSTRAINT DF_TMACNTD_add DEFAULT ('N'),    -- 실사 중 추가한 계획 외 라인
        adj_reason     VARCHAR(10)    NULL,                 -- MA0016 조정사유 (차이 ≠ 0이면 확정 전 필수)
        stock_yn       VARCHAR(1)     NOT NULL CONSTRAINT DF_TMACNTD_stock DEFAULT ('Y'),  -- 품목 재고관리 여부 스냅샷
        cnt_user_id    VARCHAR(30)    NULL,                 -- 수량 입력자/일시
        cnt_dt         DATETIME       NULL,
        trans_id       BIGINT         NULL,                 -- 확정으로 만든 조정 수불(TMATRANS)
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TMACNTD PRIMARY KEY CLUSTERED (cnt_id, serl),
        CONSTRAINT CK_TMACNTD_qty CHECK (ISNULL(cnt_qty, 0) >= 0 AND ISNULL(recnt_qty, 0) >= 0),
        CONSTRAINT CK_TMACNTD_recnt CHECK (recnt_yn IN ('Y', 'N')),
        CONSTRAINT CK_TMACNTD_add CHECK (add_yn IN ('Y', 'N')),
        CONSTRAINT CK_TMACNTD_stock CHECK (stock_yn IN ('Y', 'N'))
    );

    -- 같은 재고(품목/창고/위치/LOT)를 한 실사에서 두 줄로 세지 않게
    CREATE UNIQUE NONCLUSTERED INDEX UX_TMACNTD_key ON TMACNTD (cnt_id, item_id, wh_id, loc_id, lot_no);
    -- 품목별 실사 이력 조회용
    CREATE NONCLUSTERED INDEX IX_TMACNTD_item ON TMACNTD (acc_id, item_id) INCLUDE (cnt_id, diff_qty);
END
GO

-- ============================================================
-- 4) 공통코드 - MA0014 실사 진행상태 / MA0015 실사유형 / MA0016 조정사유, MA0004(원천구분)에 STKCNT
--    (수불유형 MA0011의 ADJ_IN/ADJ_OUT은 196번에 이미 있다 - 기타수불여부 N이라 기타입고/출고 콤보에는 안 나온다)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0014')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0014', N'재고실사 진행상태', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0015')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0015', N'재고실사 유형', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0016')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0016', N'재고실사 조정사유', 'N', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0014')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0014', '0', N'작성',     1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0014', '1', N'대상확정', 2, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0014', '2', N'실사중',   3, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0014', '3', N'입력완료', 4, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0014', 'C', N'확정',     5, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0014', 'X', N'취소',     6, 'Y', 'Y', 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0015')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0015', 'ALL',  N'전수실사',   1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0015', 'CYC',  N'순환실사',   2, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0015', 'SPOT', N'불시/지정실사', 3, 'Y', 'Y', 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0016')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0016', 'LOSS',   N'분실',     1, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0016', 'DAMAGE', N'파손',     2, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0016', 'MISIN',  N'오입고',   3, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0016', 'MISCNT', N'계수착오', 4, 'N', 'Y', 'SYSTEM', GETDATE()),
       ('MA0016', 'ETC',    N'기타',     9, 'N', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004' AND minor_cd = 'STKCNT')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0004', 'STKCNT', N'재고실사', (SELECT ISNULL(MAX(sort), 0) + 1 FROM TSMMINOR WHERE major_cd = 'MA0004'), 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 5) 콤보 L_MA0014 / L_MA0015 / L_MA0016, 채번 SC(재고실사)
-- ============================================================
DECLARE @keys TABLE (k VARCHAR(30), nm NVARCHAR(100), major VARCHAR(20));
INSERT INTO @keys VALUES
    ('L_MA0014', N'재고실사진행상태', 'MA0014'),
    ('L_MA0015', N'재고실사유형',     'MA0015'),
    ('L_MA0016', N'재고실사조정사유', 'MA0016');

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT k.k, NULL, k.nm, 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''' + k.major + N'''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
FROM @keys k
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = k.k);
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMACNTM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMACNTM', N'재고실사마스터', 'SC', 'cnt_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO
