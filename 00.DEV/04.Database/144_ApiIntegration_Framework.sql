/* ---------- 외부 API 연동 프레임워크 신설 (2026-09-15) ----------
   현장 적용 시 필요한 외부 API 연동(환율/우편번호/문자발송 등)이 계속 늘어날 걸 대비해,
   "정의(DB, 화면에서 편집)"와 "동작(연동별 C# 클래스)"을 분리한다 - sysPopUpM/sysLookupM처럼
   DB에 정의를 두고 화면에서 편집하는 프레임워크지만, 필드 매핑까지 화면에서 정의하는 완전
   노코드 엔진은 만들지 않는다(연동 개수가 많지 않고 응답 구조/저장할 업무데이터가 연동마다
   달라 오히려 과한 추상화가 됨 - AI Builder가 화면 뼈대는 생성해도 업무 SQL은 직접 짜는 것과
   같은 판단).

   실행은 서버의 Windows 작업 스케줄러가 api/system/api-integrations/run-due를 주기적으로
   두드리는 방식(IIS 앱풀이 유휴 상태로 재활용되면 내부 타이머 방식은 실행이 씹힐 수 있어서
   배제 - 2026-09-15 논의). 인증키(auth_key_enc)는 평문 저장하지 않고 Windows DPAPI(서버
   머신 종속)로 암호화한다 - ApiKeyProtector.cs 참고. */

CREATE TABLE TSMAPIDEF (
    api_cd varchar(30) NOT NULL,           -- 예: 'EXIM_FX'
    api_nm nvarchar(100) NOT NULL,
    base_url varchar(300) NOT NULL,
    auth_key_enc varbinary(max) NULL,      -- DPAPI 암호화된 인증키 - 화면에는 등록여부만 표시(마스킹)
    run_time varchar(5) NULL,              -- 'HH:mm' - NULL이면 자동실행 없이 수동실행만
    enabled_yn varchar(1) NOT NULL DEFAULT 'Y',
    last_run_dt datetime NULL,
    last_run_result_cd varchar(10) NULL,   -- 'OK' | 'FAIL'
    last_run_msg nvarchar(500) NULL,
    description nvarchar(500) NULL,

    upt_user_id varchar(30) NULL,
    upt_dt datetime NULL,
    upt_pc nvarchar(200) NULL,

    CONSTRAINT PK_TSMAPIDEF PRIMARY KEY (api_cd)
);
GO

CREATE TABLE TSMAPILOG (
    log_id bigint IDENTITY(1,1) NOT NULL,
    api_cd varchar(30) NOT NULL,
    run_dt datetime NOT NULL DEFAULT GETDATE(),
    result_cd varchar(10) NOT NULL,        -- 'OK' | 'FAIL'
    message nvarchar(1000) NULL,
    elapsed_ms int NULL,
    row_cnt int NULL,

    CONSTRAINT PK_TSMAPILOG PRIMARY KEY (log_id)
);
GO
CREATE INDEX IX_TSMAPILOG_ApiCd_RunDt ON TSMAPILOG (api_cd, run_dt DESC);
GO

-- 환율 연동이 실제로 저장할 업무데이터(연동 프레임워크와 별개 - 다른 화면에서 조회/참조용).
-- 날짜 컬럼은 yyyyMMdd 관례(project_wynlab_yyyymmdd_date_convention)에 따라 VARCHAR(8).
CREATE TABLE TSMEXRATE (
    base_date varchar(8) NOT NULL,         -- 고시일자 yyyyMMdd
    cur_unit varchar(10) NOT NULL,         -- 통화코드 (예: USD, JPY(100))
    cur_nm nvarchar(50) NULL,
    ttb decimal(18,4) NULL,                -- 전신환매입율
    tts decimal(18,4) NULL,                -- 전신환매도율
    deal_bas_r decimal(18,4) NULL,         -- 매매기준율
    bkpr decimal(18,4) NULL,               -- 장부가격
    upt_dt datetime NULL,

    CONSTRAINT PK_TSMEXRATE PRIMARY KEY (base_date, cur_unit)
);
GO

INSERT INTO TSMAPIDEF (api_cd, api_nm, base_url, run_time, enabled_yn, description)
VALUES (
    'EXIM_FX', N'수출입은행 환율',
    'https://oapi.koreaexim.go.kr/site/program/financial/exchangeJSON',
    '09:30', 'N',
    N'한국수출입은행 일별 환율 고시(영업일 11시 전후 갱신) - 인증키는 은행 홈페이지에서 현장마다 발급받아 화면에서 등록. 등록 전까지는 사용안함(N) 상태.'
);
GO

/* Developer Tool(MENU_ID) 바로 아래 FORM으로 등록 - frmSiteConfig(099번)와 같은 레벨/구조.
   PROC_PREFIX는 안 씀 - 범용 데이터채널이 아니라 전용 ApiIntegrationController를 쓰기 때문
   (SiteConfig/FilesController와 같은 이유). */
DECLARE @devToolMenuId BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE MENU_NM = N'Developer Tool' AND UPPER_MENU_ID IS NULL);

IF @devToolMenuId IS NULL
BEGIN
    PRINT 'Developer Tool 메뉴를 찾을 수 없어 건너뜁니다.';
    RETURN;
END

IF EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SYS' AND SCREEN_CLASS_NM = 'frmApiIntegration')
BEGIN
    PRINT 'frmApiIntegration 메뉴가 이미 있어 건너뜁니다.';
    RETURN;
END

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
VALUES (N'API 연동관리', @devToolMenuId, 2, 'FORM', 'SYS', 'frmApiIntegration', NULL, 60, 'Y', 'admin', GETDATE());
GO
