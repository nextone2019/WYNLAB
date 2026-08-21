/* =========================================================
   WYN LAB 기본 테이블 - 명명규칙 반영판
   대상: MSSQL Server

   [테이블 명명규칙]
   T + 모듈코드(2자리) + 엔티티명
   BA : 기준정보 (Base/기초 데이터)
   SM : 시스템운영관리 (System Management)
   SA : 영업관리 (Sales)
   PR : 생산관리 (Production)
   ... 이후 모듈 추가시 동일 규칙으로 확장 (PU:구매, QM:품질 등)
   ========================================================= */

/* -----------------------------------------------------------
   1. TBADEPT - 부서 (기준정보 모듈)
   ----------------------------------------------------------- */
CREATE TABLE TBADEPT
(
    DEPT_CD         VARCHAR(20)     NOT NULL,   -- 부서코드 (PK)
    DEPT_NM         NVARCHAR(100)   NOT NULL,   -- 부서명
    UPPER_DEPT_CD   VARCHAR(20)     NULL,       -- 상위부서코드 (자기참조)
    USE_YN          CHAR(1)         NOT NULL DEFAULT 'Y',
    SORT_ORDER      INT             NOT NULL DEFAULT 0,
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TBADEPT PRIMARY KEY (DEPT_CD)
);

/* -----------------------------------------------------------
   2. TSMUSERGRP - 사용자그룹 (시스템운영관리 모듈)
   같은 그룹에 속한 사용자는 TSMMENUAUTH에서 그룹단위로 일괄 권한 부여받음
   ----------------------------------------------------------- */
CREATE TABLE TSMUSERGRP
(
    USER_GRP_CD     VARCHAR(20)     NOT NULL,   -- 사용자그룹코드 (PK), 예: SALES_MGR, PROD_WORKER
    USER_GRP_NM     NVARCHAR(100)   NOT NULL,   -- 그룹명 (예: 영업팀장, 생산현장직)
    DESCRIPTION     NVARCHAR(200)   NULL,
    USE_YN          CHAR(1)         NOT NULL DEFAULT 'Y',
    SORT_ORDER      INT             NOT NULL DEFAULT 0,
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TSMUSERGRP PRIMARY KEY (USER_GRP_CD)
);

/* -----------------------------------------------------------
   3. TSMUSER - 사용자 (시스템운영관리 모듈)
   ----------------------------------------------------------- */
CREATE TABLE TSMUSER
(
    USER_ID         VARCHAR(20)     NOT NULL,   -- 로그인 ID (PK)
    USER_NM         NVARCHAR(50)    NOT NULL,   -- 사용자명(로그인 계정명)
    EMP_NO          VARCHAR(20)     NULL,       -- 사번. 향후 인사마스터(TBAEMP) 연동 예정, 현재는 단순 컬럼으로 보관
    PASSWORD_HASH   VARCHAR(200)    NOT NULL,   -- 해시된 비밀번호 (평문 저장 금지)
    DEPT_CD         VARCHAR(20)     NULL,       -- 소속부서 (TBADEPT)
    POSITION_NM     NVARCHAR(30)    NULL,       -- 직급
    EMAIL           VARCHAR(100)    NULL,
    MOBILE_NO       VARCHAR(20)     NULL,
    USE_YN          CHAR(1)         NOT NULL DEFAULT 'Y',
    IS_ADMIN_YN     CHAR(1)         NOT NULL DEFAULT 'N',  -- 시스템 관리자 여부
    LAST_LOGIN_DT   DATETIME        NULL,
    PWD_FAIL_CNT    INT             NOT NULL DEFAULT 0,    -- 비밀번호 실패 횟수(잠금 처리용)
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TSMUSER PRIMARY KEY (USER_ID),
    CONSTRAINT FK_TSMUSER_DEPT FOREIGN KEY (DEPT_CD) REFERENCES TBADEPT(DEPT_CD)
);

/* -----------------------------------------------------------
   3-1. TSMUSERGRPMAP - 사용자-그룹 매핑 (다대다)
   한 사용자가 여러 그룹에 속할 수 있음. 권한 조회시 이 매핑을 통해
   사용자가 속한 모든 그룹의 TSMMENUAUTH(GRP)를 OR로 합산 적용한다.
   ----------------------------------------------------------- */
CREATE TABLE TSMUSERGRPMAP
(
    USER_ID         VARCHAR(20)     NOT NULL,
    USER_GRP_CD     VARCHAR(20)     NOT NULL,
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TSMUSERGRPMAP PRIMARY KEY (USER_ID, USER_GRP_CD),
    CONSTRAINT FK_TSMUSERGRPMAP_USER FOREIGN KEY (USER_ID) REFERENCES TSMUSER(USER_ID),
    CONSTRAINT FK_TSMUSERGRPMAP_GRP FOREIGN KEY (USER_GRP_CD) REFERENCES TSMUSERGRP(USER_GRP_CD)
);

/* -----------------------------------------------------------
   4. TSMMENU - 메뉴 (트리형, 좌측 Accordion에 그대로 매핑)
   ----------------------------------------------------------- */
CREATE TABLE TSMMENU
(
    MENU_CD         VARCHAR(20)     NOT NULL,   -- 메뉴코드 (PK), 예: SA, SA_ORDER
    MENU_NM         NVARCHAR(100)   NOT NULL,   -- 메뉴명 (화면 표시)
    UPPER_MENU_CD   VARCHAR(20)     NULL,       -- 상위메뉴코드 (NULL이면 최상위=대분류)
    MENU_LEVEL      INT             NOT NULL DEFAULT 1,  -- 1:대분류 2:소분류 3:상세화면
    MENU_TYPE       VARCHAR(10)     NOT NULL DEFAULT 'FORM', -- FORM(실제화면) / GROUP(그룹only)
    FORM_CLASS_NM   VARCHAR(200)    NULL,       -- 실행할 WinForms 클래스 풀네임 (리플렉션 동적로딩)
    ICON_NM         VARCHAR(50)     NULL,       -- Accordion 아이콘명
    SORT_ORDER      INT             NOT NULL DEFAULT 0,
    USE_YN          CHAR(1)         NOT NULL DEFAULT 'Y',
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TSMMENU PRIMARY KEY (MENU_CD)
);

/* -----------------------------------------------------------
   5. TSMMENUAUTH - 메뉴별 권한
   AUTH_TARGET_TYPE = USER(개인) / GRP(사용자그룹) 로 이원화.

   [권한 병합 규칙 - API 조회 로직에서 구현]
   1) 사용자가 속한 모든 그룹(TSMUSERGRPMAP)의 GRP 권한을 OR로 합산
      예: A그룹은 등록권한 없음, B그룹은 등록권한 있음 -> 최종 등록 가능
   2) 그 위에 USER(개인) 권한 행이 있으면 개인권한이 그룹 합산결과를 덮어씀
      (개인에게 예외적으로 추가/제한 권한을 줄 때 사용)
   ----------------------------------------------------------- */
CREATE TABLE TSMMENUAUTH
(
    MENU_CD         VARCHAR(20)     NOT NULL,
    AUTH_TARGET_TYPE VARCHAR(10)    NOT NULL,   -- USER / GRP
    AUTH_TARGET_CD  VARCHAR(20)     NOT NULL,   -- USER_ID 또는 USER_GRP_CD
    VIEW_YN         CHAR(1)         NOT NULL DEFAULT 'Y',
    INSERT_YN       CHAR(1)         NOT NULL DEFAULT 'N',
    UPDATE_YN       CHAR(1)         NOT NULL DEFAULT 'N',
    DELETE_YN       CHAR(1)         NOT NULL DEFAULT 'N',
    EXCEL_YN        CHAR(1)         NOT NULL DEFAULT 'Y',  -- 엑셀 export 허용 여부
    CONSTRAINT PK_TSMMENUAUTH PRIMARY KEY (MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD),
    CONSTRAINT FK_TSMMENUAUTH_MENU FOREIGN KEY (MENU_CD) REFERENCES TSMMENU(MENU_CD)
);

/* -----------------------------------------------------------
   6. TSMLOGINHIST - 로그인/작업 이력 (감사로그)
   ----------------------------------------------------------- */
CREATE TABLE TSMLOGINHIST
(
    LOGIN_HIST_SEQ  BIGINT IDENTITY(1,1) NOT NULL,
    USER_ID         VARCHAR(20)     NOT NULL,
    LOGIN_DT        DATETIME        NOT NULL DEFAULT GETDATE(),
    LOGOUT_DT       DATETIME        NULL,
    CLIENT_IP       VARCHAR(50)     NULL,
    CLIENT_VERSION  VARCHAR(50)     NULL,       -- ClickOnce 배포버전 (버전별 이슈 추적용)
    RESULT_CD       VARCHAR(10)     NOT NULL,   -- SUCCESS / FAIL_PWD / FAIL_LOCK 등
    CONSTRAINT PK_TSMLOGINHIST PRIMARY KEY (LOGIN_HIST_SEQ)
);

/* -----------------------------------------------------------
   샘플 데이터
   ----------------------------------------------------------- */
INSERT INTO TSMUSERGRP (USER_GRP_CD, USER_GRP_NM, DESCRIPTION, SORT_ORDER) VALUES
('ADMIN',     N'시스템관리자',   N'전체 메뉴 관리권한',           10),
('SALES_MGR', N'영업관리자',     N'영업관리 모듈 등록/수정/삭제',  20),
('SALES_STF', N'영업담당',       N'영업관리 모듈 조회+등록',       30),
('PROD_STF',  N'생산담당',       N'생산관리 모듈 조회+등록',       40);

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, ICON_NM, SORT_ORDER) VALUES
('SM', N'시스템운영관리', NULL, 1, 'GROUP', 'settings',      10),
('SA', N'영업관리',       NULL, 1, 'GROUP', 'shoppingcart',  20),
('PR', N'생산관리',       NULL, 1, 'GROUP', 'tools',         30);

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, ICON_NM, SORT_ORDER) VALUES
('SA_EST',   N'견적관리', 'SA', 2, 'GROUP', NULL, 10),
('SA_ORDER', N'수주관리', 'SA', 2, 'FORM',  NULL, 20),
('SA_SHIP',  N'출고관리', 'SA', 2, 'GROUP', NULL, 30);

-- 영업관리자 그룹 전체에 수주관리 등록/수정 권한 일괄 부여 예시
INSERT INTO TSMMENUAUTH (MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, UPDATE_YN, DELETE_YN) VALUES
('SA_ORDER', 'GRP', 'SALES_MGR', 'Y', 'Y', 'Y', 'Y'),
('SA_ORDER', 'GRP', 'SALES_STF', 'Y', 'Y', 'N', 'N');

-- 한 사용자가 여러 그룹에 속하는 예시: 홍길동은 영업담당이면서 동시에 생산담당 그룹에도 소속
-- (예: 겸직, TF 참여 등으로 여러 모듈 권한을 동시에 필요로 하는 경우)
-- INSERT INTO TSMUSERGRPMAP (USER_ID, USER_GRP_CD) VALUES
-- ('hong123', 'SALES_STF'),
-- ('hong123', 'PROD_STF');
