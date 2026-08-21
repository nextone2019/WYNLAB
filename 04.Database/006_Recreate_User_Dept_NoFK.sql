/* =========================================================
   TBADEPT / TSMUSER 재생성 - FOREIGN KEY 제약 전부 제거
   (개발 초기 단계에서 부서코드/사용자그룹 미입력 등으로 INSERT가 자꾸 막히는
   문제 해결용. TSMUSERGRPMAP -> TSMUSER 참조 FK도 복구하지 않고 그대로 제거함)

   주의: 기존 TSMUSER 데이터(admin 포함 테스트 계정들)는 전부 삭제되고
         admin 계정만 새로 다시 만들어집니다. 재실행하셔도 안전하도록
         IF EXISTS 체크를 넣어뒀습니다.
   ========================================================= */

-- 0. TSMUSERGRPMAP -> TSMUSER 참조 FK 제거 (복구하지 않음 - TSMUSERGRPMAP도 이제 FK 없이 자유롭게 저장됨)
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TSMUSERGRPMAP_USER')
    ALTER TABLE TSMUSERGRPMAP DROP CONSTRAINT FK_TSMUSERGRPMAP_USER;

-- 1. 기존 테이블 제거
IF OBJECT_ID('TSMUSER', 'U') IS NOT NULL DROP TABLE TSMUSER;
IF OBJECT_ID('TBADEPT', 'U') IS NOT NULL DROP TABLE TBADEPT;

/* -----------------------------------------------------------
   2. TBADEPT - 부서 (기준정보 모듈) - FK 없음
   ----------------------------------------------------------- */
CREATE TABLE TBADEPT
(
    DEPT_CD         VARCHAR(20)     NOT NULL,
    DEPT_NM         NVARCHAR(100)   NOT NULL,
    UPPER_DEPT_CD   VARCHAR(20)     NULL,
    USE_YN          CHAR(1)         NOT NULL DEFAULT 'Y',
    SORT_ORDER      INT             NOT NULL DEFAULT 0,
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TBADEPT PRIMARY KEY (DEPT_CD)
);

/* -----------------------------------------------------------
   3. TSMUSER - 사용자 (시스템운영관리 모듈) - DEPT_CD FK 없음
   ----------------------------------------------------------- */
CREATE TABLE TSMUSER
(
    USER_ID         VARCHAR(20)     NOT NULL,
    USER_NM         NVARCHAR(50)    NOT NULL,
    EMP_NO          VARCHAR(20)     NULL,
    PASSWORD_HASH   VARCHAR(200)    NOT NULL,
    DEPT_CD         VARCHAR(20)     NULL,       -- TBADEPT 참조하지만 FK 제약은 없음 (앱단에서 검증)
    POSITION_NM     NVARCHAR(30)    NULL,
    EMAIL           VARCHAR(100)    NULL,
    MOBILE_NO       VARCHAR(20)     NULL,
    USE_YN          CHAR(1)         NOT NULL DEFAULT 'Y',
    IS_ADMIN_YN     CHAR(1)         NOT NULL DEFAULT 'N',
    LAST_LOGIN_DT   DATETIME        NULL,
    PWD_FAIL_CNT    INT             NOT NULL DEFAULT 0,
    REG_DT          DATETIME        NOT NULL DEFAULT GETDATE(),
    REG_USER_ID     VARCHAR(20)     NULL,
    CONSTRAINT PK_TSMUSER PRIMARY KEY (USER_ID)
);

/* -----------------------------------------------------------
   4. admin 계정 재삽입 (비밀번호: 1234, BCrypt 해시)
   ----------------------------------------------------------- */
INSERT INTO TSMUSER (USER_ID, USER_NM, PASSWORD_HASH, IS_ADMIN_YN, USE_YN) VALUES
('admin', N'시스템관리자', '$2b$12$.nz4wt9UXNm7TKG8mtNJFO575P8rpqc4PPWZ.jsuLvROzcyuxCaXG', 'Y', 'Y');
