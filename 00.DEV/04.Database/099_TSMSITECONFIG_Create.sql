/* ---------- TSMSITECONFIG 신설: 사이트(설치)별 환경설정 ----------
   회사당 딱 한 줄만 존재하는 설정 레코드 - 브랜딩/메일발신/첨부파일정책/비밀번호정책/색상값을
   한 테이블에 모았다(2026-09-06 설계). 개발자 전용 화면(frmSiteConfig, Developer Tool 메뉴
   안)에서만 편집한다 - 일반 사용자/관리자는 못 건드린다.

   이미지(로그인 배경/로고/파비콘)는 별도 파일서버 없이 varbinary(max)로 DB에 직접 담는다 -
   회사당 1건이라 용량 부담이 없고, WinForms 클라이언트가 API로 바이트를 그대로 받아쓰면 되므로
   파일 경로 동기화 문제가 아예 생기지 않는다.

   SMTP 비밀번호는 의도적으로 여기 없다 - DB연결문자열/JWT SecretKey와 같은 이유로 서버
   환경변수(Smtp__Password)로만 관리한다(이 화면에 두면 보안 원칙이 새는 구멍이 됨). */

CREATE TABLE TSMSITECONFIG (
    config_id int NOT NULL, -- 항상 1(단일 행) - 코드에서 강제

    -- 브랜딩
    company_nm nvarchar(100) NULL,
    login_bg_image varbinary(max) NULL,
    login_bg_mime varchar(50) NULL,
    logo_image varbinary(max) NULL,
    logo_mime varchar(50) NULL,
    favicon_image varbinary(max) NULL,
    favicon_mime varchar(50) NULL,

    -- 메일 발신(비밀번호 제외 - 서버 환경변수로 관리)
    smtp_host varchar(200) NULL,
    smtp_port int NULL,
    smtp_username varchar(200) NULL,
    smtp_from_address varchar(200) NULL,
    smtp_from_display_nm nvarchar(100) NULL,

    -- 첨부파일 정책
    file_block_extensions nvarchar(500) NULL,
    file_max_size_mb int NULL,

    -- 비밀번호 정책
    pwd_expire_days int NULL,
    pwd_lock_threshold int NULL,
    pwd_reset_code_valid_min int NULL,
    pwd_min_length int NULL,
    pwd_require_upper_lower varchar(1) NULL,
    pwd_require_digit varchar(1) NULL,
    pwd_require_special varchar(1) NULL,
    init_pwd_policy varchar(20) NULL, -- 'USER_ID' | 'RANDOM'
    force_change_on_first_login varchar(1) NULL,

    -- 색상값(배포 시 개발자가 1회 지정 - DevExpress 스킨 자체와는 별개, UiTheme 오버라이드값)
    required_field_back_color varchar(10) NULL,
    grid_header_back_color varchar(10) NULL,
    grid_focused_row_back_color varchar(10) NULL,
    brand_color varchar(10) NULL,
    tree_group_back_color varchar(10) NULL,
    divider_color varchar(10) NULL,

    upt_user_id varchar(30) NULL,
    upt_dt datetime NULL,
    upt_pc nvarchar(200) NULL,

    CONSTRAINT PK_TSMSITECONFIG PRIMARY KEY (config_id)
);
GO

-- 단일 행 강제 - config_id는 항상 1만 허용
ALTER TABLE TSMSITECONFIG ADD CONSTRAINT CK_TSMSITECONFIG_SingleRow CHECK (config_id = 1);
GO

INSERT INTO TSMSITECONFIG (
    config_id, company_nm,
    smtp_host, smtp_port, smtp_username, smtp_from_address, smtp_from_display_nm,
    file_block_extensions, file_max_size_mb,
    pwd_expire_days, pwd_lock_threshold, pwd_reset_code_valid_min, pwd_min_length,
    pwd_require_upper_lower, pwd_require_digit, pwd_require_special,
    init_pwd_policy, force_change_on_first_login,
    required_field_back_color, grid_header_back_color, grid_focused_row_back_color,
    brand_color, tree_group_back_color, divider_color
) VALUES (
    1, N'WYNLAB',
    NULL, 587, NULL, NULL, N'WYNLAB',
    'exe,bat,cmd,msi,dll', 2048,
    90, 5, 20, 8,
    'Y', 'Y', 'N',
    'USER_ID', 'Y',
    '#FFF9DB', '#F7F8FA', '#FDF3E1',
    '#1B2A3D', '#F2F3F5', '#E4E5E8'
);
GO

/* Developer Tool 메뉴(MENU_ID=39) 바로 아래 리프로 등록 - 팝업/룩업관리처럼 그룹을 하나 더
   만들지 않고 바로 FORM으로 둔다(AI Builder와 같은 레벨, MENU_LEVEL=2). PROC_PREFIX는 안 씀 -
   범용 데이터 채널이 아니라 전용 SiteConfigController를 쓰기 때문(FilesController와 같은 이유). */
INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
VALUES (N'사이트 환경설정', 39, 2, 'FORM', 'SM', 'frmSiteConfig', NULL, 40, 'Y', 'admin', GETDATE());
GO
