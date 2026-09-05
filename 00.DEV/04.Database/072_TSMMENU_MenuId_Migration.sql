/* =========================================================
   TSMMENU 재설계 - MENU_CD(VARCHAR, 사람이 직접 입력) -> MENU_ID(BIGINT IDENTITY).
   FORM_CLASS_NM("WYNLAB.SM.frmMenu, WYNLAB.SM" 한 컬럼) -> MODULE("SM") + SCREEN_CLASS_NM
   ("frmMenu") 두 컬럼으로 분리(시스템이 필요할 때 조합). 감사컬럼을 표준 6개로 교체.

   문자열 코드가 완전히 없어지면서 남는 유일한 문제 - 컴파일된 코드(서버의
   [RequireMenuPermission] 어트리뷰트 7곳)에 박아넣을 값이 필요한데, 그건 MODULE+
   SCREEN_CLASS_NM 조합("SM.frmMenu")을 그대로 쓰기로 했다(새 컬럼 추가 안 함) - 서버/클라이언트
   코드 수정은 이 파일 다음 단계.

   같이 바뀌는 것:
   - TSMMENUAUTH: MENU_CD(VARCHAR) -> MENU_ID(BIGINT)
   - TSMUSERGRIDLAYOUT: MENU_CD(VARCHAR) -> MENU_ID(BIGINT) (데이터 0건이라 단순 컬럼 교체)
   - 관련 프로시저 전부: SSP_POP_MENU_Q, USP_SM_MENU_Q/Q_1/Q_2/S/S_1,
     USP_SM_MENUAUTH_Q/Q_1/Q_2/S_1/S_2, USP_SM_GRIDLAYOUT_Q/S (다음 파일에서 처리)

   기존 데이터는 지우지 않고 *_OLD_STRCD_20260904로 이름만 바꿔 보존한다 - 확인 끝나면 나중에
   수동으로 DROP TABLE 하면 된다(043/067 때와 같은 관례).

   주의: sp_rename은 테이블 이름만 바꾸고 PK 제약조건 이름(PK_TSMMENU 등)은 그대로 남으므로,
   새 TSMMENU를 만들기 전에 그 제약조건도 먼저 이름을 바꿔줘야 한다(안 그러면 새 테이블 생성이
   "개체가 이미 있습니다" 오류로 막힌다 - 실제로 한 번 겪음).
   ========================================================= */

-- ============================================================
-- 0) 기존 테이블 이름 보존 + 그 PK 제약조건 이름도 같이 비켜준다
-- ============================================================
EXEC sp_rename 'TSMMENU', 'TSMMENU_OLD_STRCD_20260904';
EXEC sp_rename 'TSMMENUAUTH', 'TSMMENUAUTH_OLD_STRCD_20260904';
EXEC sp_rename 'PK_TSMMENU', 'PK_TSMMENU_OLD_STRCD_20260904';
EXEC sp_rename 'PK_TSMMENUAUTH', 'PK_TSMMENUAUTH_OLD_STRCD_20260904';
GO

-- ============================================================
-- 1) 새 TSMMENU 생성 (OLD_MENU_CD는 마이그레이션 중간에만 쓰는 임시 컬럼 - 맨 끝에 DROP)
-- ============================================================
CREATE TABLE [dbo].[TSMMENU](
	[MENU_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MENU_NM] [nvarchar](200) NOT NULL,
	[UPPER_MENU_ID] [bigint] NULL,
	[MENU_LEVEL] [int] NOT NULL,
	[MENU_TYPE] [varchar](10) NOT NULL,
	[MODULE] [varchar](20) NULL,
	[SCREEN_CLASS_NM] [varchar](200) NULL,
	[ICON_NM] [varchar](50) NULL,
	[PROC_PREFIX] [varchar](100) NULL,
	[SORT_ORDER] [int] NOT NULL,
	[USE_YN] [char](1) NOT NULL,
	[AUTH01_NM] [nvarchar](40) NULL,
	[AUTH02_NM] [nvarchar](40) NULL,
	[AUTH03_NM] [nvarchar](40) NULL,
	[AUTH04_NM] [nvarchar](40) NULL,
	[AUTH05_NM] [nvarchar](40) NULL,
	[AUTH06_NM] [nvarchar](40) NULL,
	[AUTH07_NM] [nvarchar](40) NULL,
	[AUTH08_NM] [nvarchar](40) NULL,
	[AUTH09_NM] [nvarchar](40) NULL,
	[AUTH10_NM] [nvarchar](40) NULL,
	[reg_user_id] [varchar](50) NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) NULL,
	[upt_user_id] [varchar](50) NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) NULL,
	[OLD_MENU_CD] [varchar](20) NULL,
 CONSTRAINT [PK_TSMMENU] PRIMARY KEY CLUSTERED ([MENU_ID] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TSMMENU] ADD DEFAULT ((1)) FOR [MENU_LEVEL]
GO
ALTER TABLE [dbo].[TSMMENU] ADD DEFAULT ('FORM') FOR [MENU_TYPE]
GO
ALTER TABLE [dbo].[TSMMENU] ADD DEFAULT ((0)) FOR [SORT_ORDER]
GO
ALTER TABLE [dbo].[TSMMENU] ADD DEFAULT ('Y') FOR [USE_YN]
GO

-- ============================================================
-- 2) 데이터 이관 - MODULE/SCREEN_CLASS_NM은 FORM_CLASS_NM("WYNLAB.{Module}.{나머지}, WYNLAB.{Module}")
--    에서 파싱한다. GROUP 행은 FORM_CLASS_NM이 NULL이라 그대로 NULL.
-- ============================================================
INSERT INTO TSMMENU (
    MENU_NM, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, ICON_NM, PROC_PREFIX, SORT_ORDER, USE_YN,
    AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc, OLD_MENU_CD
)
SELECT
    MENU_NM, MENU_LEVEL, MENU_TYPE,
    CASE WHEN FORM_CLASS_NM IS NULL OR LTRIM(RTRIM(FORM_CLASS_NM)) = '' OR CHARINDEX(',', FORM_CLASS_NM) = 0 THEN NULL
         ELSE LTRIM(RTRIM(SUBSTRING(FORM_CLASS_NM, CHARINDEX(',', FORM_CLASS_NM) + 1, 200))) END AS assembly_raw,
    NULL AS screen_class_nm_placeholder, -- 2단계 UPDATE에서 채움(MODULE 먼저 확정돼야 계산 가능)
    ICON_NM, PROC_PREFIX, SORT_ORDER, USE_YN,
    AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
    ISNULL(REG_USER_ID, 'system'), REG_DT, NULL, NULL, NULL, NULL,
    MENU_CD
FROM TSMMENU_OLD_STRCD_20260904;
GO

-- MODULE = "WYNLAB." 다음 부분(예: "WYNLAB.SM" -> "SM"). 지금 MODULE 컬럼엔 위에서 assembly_raw를
-- 임시로 넣어뒀으니 그걸 다시 다듬는다.
UPDATE TSMMENU
SET MODULE = CASE WHEN MODULE IS NULL THEN NULL
                   WHEN MODULE LIKE 'WYNLAB.%' THEN SUBSTRING(MODULE, 8, 50)
                   ELSE MODULE END;
GO

-- SCREEN_CLASS_NM = 원래 FORM_CLASS_NM의 콤마 앞부분("WYNLAB.SM.frmMenu")에서
-- "WYNLAB.{MODULE}." 접두사를 뗀 나머지("frmMenu", SM_MENU/SM_SHORTCUT처럼 중간에 네임스페이스가
-- 더 있으면 "MENU.frmMenu"도 그대로 유지 - Type.GetType()이 그 전체를 타입명으로 그대로 씀).
-- GROUP 행 상당수는 FORM_CLASS_NM이 NULL이 아니라 빈 문자열('')이었다(실제로 9건 확인) -
-- 콤마가 없으면 CHARINDEX가 0을 돌려줘서 SUBSTRING의 길이 인자가 음수(-1)가 되어 오류가 난다
-- (실제로 겪음). NULL/빈 문자열/콤마 없음을 전부 "값 없음"으로 취급하도록 방어한다.
UPDATE T
SET SCREEN_CLASS_NM = CASE
    WHEN O.FORM_CLASS_NM IS NULL OR LTRIM(RTRIM(O.FORM_CLASS_NM)) = '' THEN NULL
    WHEN CHARINDEX(',', O.FORM_CLASS_NM) = 0 THEN NULL
    ELSE LTRIM(RTRIM(SUBSTRING(O.FORM_CLASS_NM, 1, CHARINDEX(',', O.FORM_CLASS_NM) - 1)))
    END
FROM TSMMENU T
JOIN TSMMENU_OLD_STRCD_20260904 O ON O.MENU_CD = T.OLD_MENU_CD;
GO

UPDATE TSMMENU
SET SCREEN_CLASS_NM = SUBSTRING(SCREEN_CLASS_NM, LEN('WYNLAB.' + MODULE + '.') + 1, 200)
WHERE SCREEN_CLASS_NM IS NOT NULL AND MODULE IS NOT NULL
  AND SCREEN_CLASS_NM LIKE 'WYNLAB.' + MODULE + '.%';
GO

-- UPPER_MENU_ID 해소 - 옛 UPPER_MENU_CD를 새 MENU_ID로 치환
UPDATE T
SET UPPER_MENU_ID = P.MENU_ID
FROM TSMMENU T
JOIN TSMMENU_OLD_STRCD_20260904 O ON O.MENU_CD = T.OLD_MENU_CD
JOIN TSMMENU P ON P.OLD_MENU_CD = O.UPPER_MENU_CD
WHERE O.UPPER_MENU_CD IS NOT NULL;
GO

-- ============================================================
-- 3) TSMMENUAUTH 재생성 - MENU_CD -> MENU_ID
-- ============================================================
CREATE TABLE [dbo].[TSMMENUAUTH](
	[MENU_ID] [bigint] NOT NULL,
	[AUTH_TARGET_TYPE] [varchar](10) NOT NULL,
	[AUTH_TARGET_CD] [varchar](20) NOT NULL,
	[VIEW_YN] [char](1) NOT NULL,
	[INSERT_YN] [char](1) NOT NULL,
	[DELETE_YN] [char](1) NOT NULL,
	[SAVE_YN] [char](1) NOT NULL,
	[PRINT_YN] [char](1) NOT NULL,
	[EXCEL_YN] [char](1) NOT NULL,
	[AUTH01] [char](1) NOT NULL,
	[AUTH02] [char](1) NOT NULL,
	[AUTH03] [char](1) NOT NULL,
	[AUTH04] [char](1) NOT NULL,
	[AUTH05] [char](1) NOT NULL,
	[AUTH06] [char](1) NOT NULL,
	[AUTH07] [char](1) NOT NULL,
	[AUTH08] [char](1) NOT NULL,
	[AUTH09] [char](1) NOT NULL,
	[AUTH10] [char](1) NOT NULL,
	[REG_USER_ID] [varchar](20) NULL,
	[REG_DT] [datetime] NOT NULL,
	[UPT_USER_ID] [varchar](20) NULL,
	[UPT_DT] [datetime] NOT NULL,
 CONSTRAINT [PK_TSMMENUAUTH] PRIMARY KEY CLUSTERED ([MENU_ID] ASC, [AUTH_TARGET_TYPE] ASC, [AUTH_TARGET_CD] ASC)
) ON [PRIMARY]
GO

INSERT INTO TSMMENUAUTH (
    MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, DELETE_YN, SAVE_YN, PRINT_YN, EXCEL_YN,
    AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10,
    REG_USER_ID, REG_DT, UPT_USER_ID, UPT_DT
)
SELECT
    T.MENU_ID, O.AUTH_TARGET_TYPE, O.AUTH_TARGET_CD, O.VIEW_YN, O.INSERT_YN, O.DELETE_YN, O.SAVE_YN, O.PRINT_YN, O.EXCEL_YN,
    O.AUTH01, O.AUTH02, O.AUTH03, O.AUTH04, O.AUTH05, O.AUTH06, O.AUTH07, O.AUTH08, O.AUTH09, O.AUTH10,
    O.REG_USER_ID, O.REG_DT, O.UPT_USER_ID, O.UPT_DT
FROM TSMMENUAUTH_OLD_STRCD_20260904 O
JOIN TSMMENU T ON T.OLD_MENU_CD = O.MENU_CD;
GO

-- ============================================================
-- 4) TSMUSERGRIDLAYOUT - 데이터 0건이라 단순 컬럼 교체(PK가 MENU_CD를 물고 있어서 PK부터 재생성)
-- ============================================================
ALTER TABLE TSMUSERGRIDLAYOUT DROP CONSTRAINT PK_TSMUSERGRIDLAYOUT;
ALTER TABLE TSMUSERGRIDLAYOUT DROP COLUMN MENU_CD;
ALTER TABLE TSMUSERGRIDLAYOUT ADD MENU_ID BIGINT NOT NULL;
ALTER TABLE TSMUSERGRIDLAYOUT ADD CONSTRAINT PK_TSMUSERGRIDLAYOUT PRIMARY KEY CLUSTERED (USER_ID, MENU_ID, GRID_KEY);
GO

-- ============================================================
-- 5) 임시 컬럼 정리
-- ============================================================
ALTER TABLE TSMMENU DROP COLUMN OLD_MENU_CD;
GO
