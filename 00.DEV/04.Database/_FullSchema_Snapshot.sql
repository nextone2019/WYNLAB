-- 새 서버(빈 DB)에 처음부터 전체 스키마를 한 번에 만들기 위한 통짜 스냅샷 - 001~136번
-- 마이그레이션 파일을 순서대로 실행한 것과 최종 결과가 같다(이 파일 자체는 그 대체 경로일
-- 뿐, 마이그레이션 파일들을 지우거나 대신하는 게 아니다 - 앞으로도 새 변경은 계속 다음 번호
-- 마이그레이션 파일로 추가한다).
--
-- 2026-09-13에 개발 PC의 실제 DB(SQL Server 2022)에서 SMO(Scripter, WithDependencies=true)로
-- 직접 뽑아냈다 - 테이블 47개 + 프로시저/뷰/함수 77개, 참조 관계(FK) 순서까지 자동으로
-- 맞춰져 있다. 빈 DB에 실제로 적용해서 오류 0건 확인함(WYNLAB_INSTALLTEST로 검증).
--
-- USP_BA_ITEMLIST_Q/USP_BA_ITEMUNIT_Q/USP_BA_ITEMUNIT_S는 일부러 뺐다 - 예전 TBAITEM/
-- TBAACC 컬럼명 변경(item_cd/acc_cd/wh_cd/loc_cd 등) 이후로 갱신이 안 된 죽은 프로시저라
-- 개발 DB에서도 이미 실행 불가능한 상태다(원본 정의를 그대로 CREATE만 해도 "열 이름이
-- 유효하지 않습니다" 오류가 남 - 실제로 확인함). 필요해지면 그때 새 마이그레이션 파일로
-- 제대로 고쳐서 추가하면 된다.
--
-- 데이터는 전혀 포함하지 않는다(구조만) - 최초 로그인용 admin 계정은 136번 마이그레이션이
-- 별도로 만든다.
--
-- 다시 뽑아야 할 때(스키마가 많이 바뀐 뒤 등)는 이 파일을 직접 고치지 말고 새로 통째로
-- 교체한다(부분 수정 금지 - SMO가 매번 전체를 다시 뽑아야 의존성 순서가 보장된다).
--
-- 사용법(운영 서버, sqlcmd):
--   1) CREATE DATABASE WYNLAB;
--   2) sqlcmd -S <서버> -U <계정> -P <비밀번호> -d WYNLAB -C -f 65001 -i _FullSchema_Snapshot.sql
--   3) 136_Seed_Admin_User.sql만 이어서 실행(001~135는 이 파일에 이미 반영됨)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMUSERGRP](
	[USER_GRP_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[USER_GRP_NM] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DESCRIPTION] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[USE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[SORT_ORDER] [int] NOT NULL,
	[REG_DT] [datetime] NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TSMUSERGRP] PRIMARY KEY CLUSTERED 
(
	[USER_GRP_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMUSERGRIDLAYOUT](
	[USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[GRID_KEY] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[LAYOUT_XML] [nvarchar](max) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[UPT_DT] [datetime] NOT NULL,
	[MENU_ID] [bigint] NOT NULL,
 CONSTRAINT [PK_TSMUSERGRIDLAYOUT] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC,
	[MENU_ID] ASC,
	[GRID_KEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMUSER_OLD_20260831](
	[USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[USER_NM] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[EMP_NO] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[PASSWORD_HASH] [varchar](200) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DEPT_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[POSITION_NM] [nvarchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[EMAIL] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[MOBILE_NO] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[USE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[IS_ADMIN_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[LAST_LOGIN_DT] [datetime] NULL,
	[PWD_FAIL_CNT] [int] NOT NULL,
	[REG_DT] [datetime] NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[USER_TYPE] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
 CONSTRAINT [PK_TSMUSER_OLD_20260831] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMUSER](
	[USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[USER_NM] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[PASSWORD_HASH] [varchar](200) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[USER_TYPE] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DEVELOPER_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[LAST_LOGIN_DT] [datetime] NULL,
	[PWD_FAIL_CNT] [int] NOT NULL,
	[LAST_PWD_CHANGE_DATE] [datetime] NULL,
	[USE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DEL_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[REG_DT] [datetime] NOT NULL,
	[UPT_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[UPT_DT] [datetime] NOT NULL,
	[EMAIL] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[MUST_CHANGE_PWD_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[EMP_ID] [bigint] NULL,
	[ACC_ID] [bigint] NULL,
 CONSTRAINT [PK_TSMUSER] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMSITECONFIG](
	[config_id] [int] NOT NULL,
	[company_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[login_bg_image] [varbinary](max) NULL,
	[login_bg_mime] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[logo_image] [varbinary](max) NULL,
	[logo_mime] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[favicon_image] [varbinary](max) NULL,
	[favicon_mime] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[smtp_host] [varchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[smtp_port] [int] NULL,
	[smtp_username] [varchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[smtp_from_address] [varchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[smtp_from_display_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[file_block_extensions] [nvarchar](500) COLLATE Korean_Wansung_CI_AS NULL,
	[file_max_size_mb] [int] NULL,
	[pwd_expire_days] [int] NULL,
	[pwd_lock_threshold] [int] NULL,
	[pwd_reset_code_valid_min] [int] NULL,
	[pwd_min_length] [int] NULL,
	[pwd_require_upper_lower] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[pwd_require_digit] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[pwd_require_special] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[init_pwd_policy] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[force_change_on_first_login] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[required_field_back_color] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[grid_header_back_color] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[grid_focused_row_back_color] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[brand_color] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[tree_group_back_color] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[divider_color] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[idle_timeout_minutes] [int] NULL,
 CONSTRAINT [PK_TSMSITECONFIG] PRIMARY KEY CLUSTERED 
(
	[config_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMSHORTCUTDEFAULT](
	[action_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[action_nm] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[key_combo] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[sort_order] [int] NOT NULL,
 CONSTRAINT [PK_TSMSHORTCUTDEFAULT] PRIMARY KEY CLUSTERED 
(
	[action_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMPWDRESETTOKEN](
	[TOKEN_ID] [int] IDENTITY(1,1) NOT NULL,
	[USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[CODE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[EXPIRE_DT] [datetime] NOT NULL,
	[USED_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REQ_DT] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TOKEN_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMINOR](
	[major_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[minor_cd] [varchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[minor_nm] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[sort] [int] NULL,
	[sys_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[use_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd1] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd2] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd3] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd4] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd5] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd6] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd7] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd8] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd9] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd10] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[major_cd] ASC,
	[minor_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMENUAUTH_OLD_STRCD_20260904](
	[MENU_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH_TARGET_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH_TARGET_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[VIEW_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[INSERT_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DELETE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[SAVE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[PRINT_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[EXCEL_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH01] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH02] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH03] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH04] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH05] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH06] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH07] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH08] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH09] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH10] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[REG_DT] [datetime] NOT NULL,
	[UPT_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[UPT_DT] [datetime] NOT NULL,
 CONSTRAINT [PK_TSMMENUAUTH_OLD_STRCD_20260904] PRIMARY KEY CLUSTERED 
(
	[MENU_CD] ASC,
	[AUTH_TARGET_TYPE] ASC,
	[AUTH_TARGET_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMENUAUTH_OLD_20260831](
	[MENU_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH_TARGET_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH_TARGET_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[VIEW_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[INSERT_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[UPDATE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DELETE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[EXCEL_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
 CONSTRAINT [PK_TSMMENUAUTH_OLD_20260831] PRIMARY KEY CLUSTERED 
(
	[MENU_CD] ASC,
	[AUTH_TARGET_TYPE] ASC,
	[AUTH_TARGET_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMENUAUTH](
	[MENU_ID] [bigint] NOT NULL,
	[AUTH_TARGET_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH_TARGET_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[VIEW_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[INSERT_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[DELETE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[SAVE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[PRINT_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[EXCEL_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH01] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH02] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH03] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH04] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH05] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH06] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH07] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH08] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH09] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH10] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[REG_DT] [datetime] NOT NULL,
	[UPT_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[UPT_DT] [datetime] NOT NULL,
 CONSTRAINT [PK_TSMMENUAUTH] PRIMARY KEY CLUSTERED 
(
	[MENU_ID] ASC,
	[AUTH_TARGET_TYPE] ASC,
	[AUTH_TARGET_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMENU_OLD_STRCD_20260904](
	[MENU_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[MENU_NM] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[UPPER_MENU_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[MENU_LEVEL] [int] NOT NULL,
	[MENU_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[FORM_CLASS_NM] [varchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[ICON_NM] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[SORT_ORDER] [int] NOT NULL,
	[USE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_DT] [datetime] NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[PROC_PREFIX] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH01_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH02_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH03_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH04_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH05_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH06_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH07_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH08_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH09_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH10_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TSMMENU_OLD_STRCD_20260904] PRIMARY KEY CLUSTERED 
(
	[MENU_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tsmmenu_bak](
	[MENU_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[MENU_NM] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[UPPER_MENU_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[MENU_LEVEL] [int] NOT NULL,
	[MENU_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[FORM_CLASS_NM] [varchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[ICON_NM] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[SORT_ORDER] [int] NOT NULL,
	[USE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_DT] [datetime] NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[PROC_PREFIX] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH01_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH02_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH03_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH04_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH05_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH06_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH07_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH08_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH09_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH10_NM] [nvarchar](20) COLLATE Korean_Wansung_CI_AS NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMENU](
	[MENU_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MENU_NM] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[UPPER_MENU_ID] [bigint] NULL,
	[MENU_LEVEL] [int] NOT NULL,
	[MENU_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[MODULE] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[SCREEN_CLASS_NM] [varchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[ICON_NM] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[PROC_PREFIX] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[SORT_ORDER] [int] NOT NULL,
	[USE_YN] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[AUTH01_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH02_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH03_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH04_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH05_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH06_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH07_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH08_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH09_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[AUTH10_NM] [nvarchar](40) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TSMMENU] PRIMARY KEY CLUSTERED 
(
	[MENU_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMMAJOR](
	[major_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[major_nm] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[sys_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[rel_cd1] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title1] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type1] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd2] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title2] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type2] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd3] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title3] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type3] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd4] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title4] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type4] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd5] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title5] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type5] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd6] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title6] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type6] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd7] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title7] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type7] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd8] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title8] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type8] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd9] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title9] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type9] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd10] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_title10] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[rel_cd_type10] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[major_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMLOGINHIST](
	[LOGIN_HIST_SEQ] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[LOGIN_DT] [datetime] NOT NULL,
	[LOGOUT_DT] [datetime] NULL,
	[CLIENT_IP] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[CLIENT_VERSION] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[RESULT_CD] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
 CONSTRAINT [PK_TSMLOGINHIST] PRIMARY KEY CLUSTERED 
(
	[LOGIN_HIST_SEQ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMFILEHIST](
	[hist_id] [bigint] IDENTITY(1,1) NOT NULL,
	[file_id] [bigint] NOT NULL,
	[doc_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[doc_id] [bigint] NOT NULL,
	[doc_no] [varchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[file_nm] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[down_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[down_dt] [datetime] NOT NULL,
	[down_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[down_ip] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TSMFILEHIST] PRIMARY KEY CLUSTERED 
(
	[hist_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMFILE](
	[file_id] [bigint] IDENTITY(1,1) NOT NULL,
	[doc_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[doc_id] [bigint] NOT NULL,
	[doc_no] [varchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[doc_serl] [int] NOT NULL,
	[serl] [int] NOT NULL,
	[file_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[file_nm] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[file_size] [bigint] NULL,
	[mime_type] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[file_path] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[storage_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[form_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[fail_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[retry_cnt] [int] NULL,
 CONSTRAINT [PK_TSMFILE] PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMAUTOKEYHIST](
	[table_name] [varchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[base_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[yyyy] [varchar](4) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[mm] [varchar](2) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[dd] [varchar](2) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[seq] [int] NULL,
	[new_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[table_name] ASC,
	[base_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMAUTOKEY](
	[table_name] [varchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[table_desc] [nvarchar](500) COLLATE Korean_Wansung_CI_AS NULL,
	[pre_fix] [varchar](5) COLLATE Korean_Wansung_CI_AS NULL,
	[key_col] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[date_col] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[date_type] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[seq_len] [int] NULL,
	[reg_user_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[table_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[THRNAMECARDREQ](
	[req_id] [bigint] IDENTITY(1,1) NOT NULL,
	[req_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[dept_id] [bigint] NOT NULL,
	[emp_id] [bigint] NOT NULL,
	[job_grade] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[name_kor] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[name_eng] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[dept_kor] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[dept_eng] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[mobile] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[email] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[stat_cd] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[app_id] [bigint] NULL,
	[app_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_THRNAMECARDREQ] PRIMARY KEY CLUSTERED 
(
	[req_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBAITEMUNIT](
	[item_id] [bigint] NOT NULL,
	[fr_unit_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[fr_qty] [numeric](19, 5) NULL,
	[to_unit_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[to_qty] [numeric](19, 5) NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[acc_id] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[item_id] ASC,
	[fr_unit_cd] ASC,
	[to_unit_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBAITEMGRP](
	[grp_id] [bigint] IDENTITY(1,1) NOT NULL,
	[grp_nm] [nvarchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[grp_lvl] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[par_grp_id] [bigint] NOT NULL,
	[remark] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[acc_id] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[grp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBAITEM](
	[item_id] [bigint] IDENTITY(1,1) NOT NULL,
	[item_no] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[item_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[item_spec] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[unit_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[po_unit_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[wh_id] [bigint] NULL,
	[loc_id] [bigint] NULL,
	[safe_qty] [numeric](18, 5) NULL,
	[asset_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[out_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[stock_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[lot_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[po_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[po_qc_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[sale_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[prod_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[prod_qc_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[stat_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[grp1_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[grp2_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[grp3_id] [bigint] NULL,
	[grp4_id] [bigint] NULL,
	[po_acnt_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[sale_acnt_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[photo] [image] NULL,
	[photo_file_nm] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[photo_path] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[dept_id] [bigint] NULL,
	[cust_id] [bigint] NULL,
	[emp_id] [bigint] NULL,
	[acc_id] [bigint] NULL,
 CONSTRAINT [PK_TBAITEM] PRIMARY KEY CLUSTERED 
(
	[item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBAEMP](
	[emp_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[emp_nm] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[emp_nm_eng] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[ent_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[grp_ent_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[job_grade] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[job_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[ret_yn] [varchar](2) COLLATE Korean_Wansung_CI_AS NULL,
	[ret_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[sex_cd] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[tel] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[hp_tel] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[email] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[nat_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[zip_code] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[addr1] [nvarchar](300) COLLATE Korean_Wansung_CI_AS NULL,
	[addr2] [nvarchar](300) COLLATE Korean_Wansung_CI_AS NULL,
	[photo] [image] NULL,
	[photo_file_nm] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[photo_path] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[holi_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[dilig_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[pay_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[DEPT_ID] [bigint] NULL,
	[EMP_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[acc_id] [bigint] NULL,
 CONSTRAINT [PK_TBAEMP] PRIMARY KEY CLUSTERED 
(
	[EMP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_TBAEMP_EMPNO] UNIQUE NONCLUSTERED 
(
	[emp_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBADEPT_OLD_STRCD_20260906](
	[acc_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[dept_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[dept_nm] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[par_dept_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[dept_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](2000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBADEPT_OLD_STRCD_20260906] PRIMARY KEY CLUSTERED 
(
	[acc_cd] ASC,
	[dept_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBADEPT](
	[DEPT_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[dept_nm] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[PAR_DEPT_ID] [bigint] NULL,
	[dept_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[remark] [nvarchar](4000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[acc_id] [bigint] NULL,
 CONSTRAINT [PK_TBADEPT] PRIMARY KEY CLUSTERED 
(
	[DEPT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUSTTYPE](
	[cust_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[cust_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[cust_cd] ASC,
	[cust_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUSTPRSN_OLD_STRCD_20260906](
	[cust_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[serl] [int] NOT NULL,
	[prsn_nm] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[grade] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[tel1] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[tel2] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[fax] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[email] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBACUSTPRSN_OLD_STRCD_20260906] PRIMARY KEY CLUSTERED 
(
	[cust_cd] ASC,
	[serl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUSTPRSN](
	[CUST_ID] [bigint] NOT NULL,
	[serl] [int] NOT NULL,
	[prsn_nm] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[grade] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[tel1] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[tel2] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[fax] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[email] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBACUSTPRSN] PRIMARY KEY CLUSTERED 
(
	[CUST_ID] ASC,
	[serl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUSTACNT_OLD_STRCD_20260906](
	[cust_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[serl] [int] NOT NULL,
	[bank_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[acnt_no] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[remark] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBACUSTACNT_OLD_STRCD_20260906] PRIMARY KEY CLUSTERED 
(
	[cust_cd] ASC,
	[serl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUSTACNT](
	[CUST_ID] [bigint] NOT NULL,
	[serl] [int] NOT NULL,
	[bank_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[acnt_no] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[remark] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBACUSTACNT] PRIMARY KEY CLUSTERED 
(
	[CUST_ID] ASC,
	[serl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUST_OLD_STRCD_20260906](
	[cust_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[cust_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[biz_no] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[tel] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[cur_cd] [varchar](3) COLLATE Korean_Wansung_CI_AS NULL,
	[owner_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[zip_code] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[addr1] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[addr2] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[homepage] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[email] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[fax] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[biz_kind] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[biz_type] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[trans_open_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[vat_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[vat_rate] [numeric](19, 2) NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[stat_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[emp_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBACUST_OLD_STRCD_20260906] PRIMARY KEY CLUSTERED 
(
	[cust_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBACUST](
	[CUST_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[cust_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[biz_no] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[tel] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[cur_cd] [varchar](3) COLLATE Korean_Wansung_CI_AS NULL,
	[owner_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[zip_code] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[addr1] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[addr2] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[homepage] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[email] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[fax] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[biz_kind] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[biz_type] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[trans_open_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[vat_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[vat_rate] [numeric](19, 2) NULL,
	[remark] [nvarchar](3000) COLLATE Korean_Wansung_CI_AS NULL,
	[stat_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[EMP_ID] [bigint] NULL,
 CONSTRAINT [PK_TBACUST] PRIMARY KEY CLUSTERED 
(
	[CUST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBAACC](
	[ACC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ACC_NM] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[BIZ_NO] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[TEL] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[CUR_CD] [varchar](3) COLLATE Korean_Wansung_CI_AS NULL,
	[OWNER_NM] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[OWNER_NM_ENG] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[ZIP_CODE] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
	[ADDR1] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[ADDR2] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[ADDR1_ENG] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[ADDR2_ENG] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[HOMEPAGE] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[EMAIL] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[FAX] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[BIZ_KIND] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[BIZ_TYPE] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[OPEN_DATE] [varchar](8) COLLATE Korean_Wansung_CI_AS NULL,
	[VAT_TYPE] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[VAT_RATE] [numeric](19, 2) NULL,
	[LOGO] [image] NULL,
	[LOGO_FILE_NM] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[LOGO_PATH] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[STAMP] [image] NULL,
	[STAMP_FILE_NM] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[STAMP_PATH] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[REG_USER_ID] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_DT] [datetime] NULL,
	[REG_PC] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
	[UPT_USER_ID] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[UPT_DT] [datetime] NULL,
	[UPT_PC] [nvarchar](400) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TBAACC] PRIMARY KEY CLUSTERED 
(
	[ACC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAPROUTEDETAIL](
	[route_id] [bigint] NOT NULL,
	[sort] [int] NOT NULL,
	[emp_id] [bigint] NOT NULL,
	[path_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
 CONSTRAINT [PK_TAPROUTEDETAIL] PRIMARY KEY CLUSTERED 
(
	[route_id] ASC,
	[sort] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAPROUTE](
	[route_id] [bigint] IDENTITY(1,1) NOT NULL,
	[emp_id] [bigint] NOT NULL,
	[route_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[use_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TAPROUTE] PRIMARY KEY CLUSTERED 
(
	[route_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAPDOCPATH](
	[app_id] [bigint] NOT NULL,
	[serl] [int] NOT NULL,
	[app_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[doc_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[doc_id] [bigint] NOT NULL,
	[doc_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[sort] [int] NOT NULL,
	[path_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[emp_id] [bigint] NOT NULL,
	[stat_cd] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[app_dt] [datetime] NULL,
	[remark] [nvarchar](1000) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TAPDOCPATH] PRIMARY KEY CLUSTERED 
(
	[app_id] ASC,
	[serl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAPDOC](
	[app_id] [bigint] IDENTITY(1,1) NOT NULL,
	[app_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[app_date] [varchar](8) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[app_title] [nvarchar](500) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[app_text] [nvarchar](4000) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[form_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[doc_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[doc_id] [bigint] NOT NULL,
	[doc_no] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[acc_id] [bigint] NOT NULL,
	[dept_id] [bigint] NOT NULL,
	[emp_id] [bigint] NOT NULL,
	[end_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[end_emp_id] [bigint] NULL,
	[end_dt] [datetime] NULL,
	[rtn_yn] [varchar](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[rtn_emp_id] [bigint] NULL,
	[rtn_dt] [datetime] NULL,
	[stat_cd] [varchar](10) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[reg_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_user_id] [varchar](30) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[upt_pc] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TAPDOC] PRIMARY KEY CLUSTERED 
(
	[app_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysLookupM](
	[lookup_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[proc_nm] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[lookup_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[value_field] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[display_field] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[use_yn] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[remark] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
	[source_type] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[query_txt] [nvarchar](max) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_sysLookupM] PRIMARY KEY CLUSTERED 
(
	[lookup_key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysPopUpM](
	[popup_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[proc_nm] [varchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[popup_nm] [nvarchar](100) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[hierarchical_yn] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[key_field] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[parent_field] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[display_field] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[popup_width] [int] NOT NULL,
	[popup_height] [int] NOT NULL,
	[use_yn] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[remark] [nvarchar](200) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
 CONSTRAINT [PK_sysPopUpM] PRIMARY KEY CLUSTERED 
(
	[popup_key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMUSERSHORTCUT](
	[user_id] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[action_cd] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[key_combo] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[reg_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[reg_dt] [datetime] NOT NULL,
	[upt_user_id] [varchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[upt_dt] [datetime] NULL,
 CONSTRAINT [PK_TSMUSERSHORTCUT] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[action_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSMUSERGRPMAP](
	[USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[USER_GRP_CD] [varchar](20) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[REG_DT] [datetime] NOT NULL,
	[REG_USER_ID] [varchar](20) COLLATE Korean_Wansung_CI_AS NULL,
 CONSTRAINT [PK_TSMUSERGRPMAP] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC,
	[USER_GRP_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysPopUpD](
	[popup_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[column_nm] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[caption] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[control_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[lookup_proc_nm] [varchar](100) COLLATE Korean_Wansung_CI_AS NULL,
	[sort] [int] NOT NULL,
	[width] [int] NOT NULL,
	[visible_yn] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
 CONSTRAINT [PK_sysPopUpD] PRIMARY KEY CLUSTERED 
(
	[popup_key] ASC,
	[column_nm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysLookupP](
	[lookup_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[param_nm] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[caption] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[sort] [int] NOT NULL,
 CONSTRAINT [PK_sysLookupP] PRIMARY KEY CLUSTERED 
(
	[lookup_key] ASC,
	[param_nm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysLookupC](
	[lookup_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[column_nm] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[caption] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[sort] [int] NOT NULL,
	[width] [int] NOT NULL,
	[visible_yn] [char](1) COLLATE Korean_Wansung_CI_AS NOT NULL,
 CONSTRAINT [PK_sysLookupC] PRIMARY KEY CLUSTERED 
(
	[lookup_key] ASC,
	[column_nm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysPopUpS](
	[popup_key] [varchar](30) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[param_nm] [varchar](50) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[caption] [nvarchar](50) COLLATE Korean_Wansung_CI_AS NULL,
	[control_type] [varchar](10) COLLATE Korean_Wansung_CI_AS NOT NULL,
	[sort] [int] NOT NULL,
	[width] [int] NOT NULL,
 CONSTRAINT [PK_sysPopUpS] PRIMARY KEY CLUSTERED 
(
	[popup_key] ASC,
	[param_nm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TSMUSERGRP] ADD  DEFAULT ('Y') FOR [USE_YN]
GO
ALTER TABLE [dbo].[TSMUSERGRP] ADD  DEFAULT ((0)) FOR [SORT_ORDER]
GO
ALTER TABLE [dbo].[TSMUSERGRP] ADD  DEFAULT (getdate()) FOR [REG_DT]
GO
ALTER TABLE [dbo].[TSMUSER_OLD_20260831] ADD  DEFAULT ('Y') FOR [USE_YN]
GO
ALTER TABLE [dbo].[TSMUSER_OLD_20260831] ADD  DEFAULT ('N') FOR [IS_ADMIN_YN]
GO
ALTER TABLE [dbo].[TSMUSER_OLD_20260831] ADD  DEFAULT ((0)) FOR [PWD_FAIL_CNT]
GO
ALTER TABLE [dbo].[TSMUSER_OLD_20260831] ADD  DEFAULT (getdate()) FOR [REG_DT]
GO
ALTER TABLE [dbo].[TSMUSER_OLD_20260831] ADD  DEFAULT ('U') FOR [USER_TYPE]
GO
ALTER TABLE [dbo].[TSMUSER] ADD  CONSTRAINT [DF_TSMUSER_MUSTCHANGEPWD]  DEFAULT ('N') FOR [MUST_CHANGE_PWD_YN]
GO
ALTER TABLE [dbo].[TSMPWDRESETTOKEN] ADD  DEFAULT ('N') FOR [USED_YN]
GO
ALTER TABLE [dbo].[TSMPWDRESETTOKEN] ADD  DEFAULT (getdate()) FOR [REQ_DT]
GO
ALTER TABLE [dbo].[TSMMENUAUTH_OLD_20260831] ADD  DEFAULT ('Y') FOR [VIEW_YN]
GO
ALTER TABLE [dbo].[TSMMENUAUTH_OLD_20260831] ADD  DEFAULT ('N') FOR [INSERT_YN]
GO
ALTER TABLE [dbo].[TSMMENUAUTH_OLD_20260831] ADD  DEFAULT ('N') FOR [UPDATE_YN]
GO
ALTER TABLE [dbo].[TSMMENUAUTH_OLD_20260831] ADD  DEFAULT ('N') FOR [DELETE_YN]
GO
ALTER TABLE [dbo].[TSMMENUAUTH_OLD_20260831] ADD  DEFAULT ('Y') FOR [EXCEL_YN]
GO
ALTER TABLE [dbo].[TSMMENU_OLD_STRCD_20260904] ADD  DEFAULT ((1)) FOR [MENU_LEVEL]
GO
ALTER TABLE [dbo].[TSMMENU_OLD_STRCD_20260904] ADD  DEFAULT ('FORM') FOR [MENU_TYPE]
GO
ALTER TABLE [dbo].[TSMMENU_OLD_STRCD_20260904] ADD  DEFAULT ((0)) FOR [SORT_ORDER]
GO
ALTER TABLE [dbo].[TSMMENU_OLD_STRCD_20260904] ADD  DEFAULT ('Y') FOR [USE_YN]
GO
ALTER TABLE [dbo].[TSMMENU_OLD_STRCD_20260904] ADD  DEFAULT (getdate()) FOR [REG_DT]
GO
ALTER TABLE [dbo].[TSMMENU] ADD  DEFAULT ((1)) FOR [MENU_LEVEL]
GO
ALTER TABLE [dbo].[TSMMENU] ADD  DEFAULT ('FORM') FOR [MENU_TYPE]
GO
ALTER TABLE [dbo].[TSMMENU] ADD  DEFAULT ((0)) FOR [SORT_ORDER]
GO
ALTER TABLE [dbo].[TSMMENU] ADD  DEFAULT ('Y') FOR [USE_YN]
GO
ALTER TABLE [dbo].[TSMMAJOR] ADD  DEFAULT ('N') FOR [sys_yn]
GO
ALTER TABLE [dbo].[TSMLOGINHIST] ADD  DEFAULT (getdate()) FOR [LOGIN_DT]
GO
ALTER TABLE [dbo].[THRNAMECARDREQ] ADD  DEFAULT ('0') FOR [stat_cd]
GO
ALTER TABLE [dbo].[TBAEMP] ADD  DEFAULT ('N') FOR [holi_yn]
GO
ALTER TABLE [dbo].[TAPROUTE] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[TAPDOCPATH] ADD  DEFAULT ('N') FOR [stat_cd]
GO
ALTER TABLE [dbo].[TAPDOC] ADD  DEFAULT ('N') FOR [end_yn]
GO
ALTER TABLE [dbo].[TAPDOC] ADD  DEFAULT ('N') FOR [rtn_yn]
GO
ALTER TABLE [dbo].[sysLookupM] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[sysLookupM] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[sysLookupM] ADD  CONSTRAINT [DF_sysLookupM_SourceType]  DEFAULT ('P') FOR [source_type]
GO
ALTER TABLE [dbo].[sysPopUpM] ADD  DEFAULT ('N') FOR [hierarchical_yn]
GO
ALTER TABLE [dbo].[sysPopUpM] ADD  DEFAULT ((700)) FOR [popup_width]
GO
ALTER TABLE [dbo].[sysPopUpM] ADD  DEFAULT ((500)) FOR [popup_height]
GO
ALTER TABLE [dbo].[sysPopUpM] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[sysPopUpM] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[TSMUSERGRPMAP] ADD  DEFAULT (getdate()) FOR [REG_DT]
GO
ALTER TABLE [dbo].[sysPopUpD] ADD  DEFAULT ('TEXT') FOR [control_type]
GO
ALTER TABLE [dbo].[sysPopUpD] ADD  DEFAULT ((0)) FOR [sort]
GO
ALTER TABLE [dbo].[sysPopUpD] ADD  DEFAULT ((100)) FOR [width]
GO
ALTER TABLE [dbo].[sysPopUpD] ADD  DEFAULT ('Y') FOR [visible_yn]
GO
ALTER TABLE [dbo].[sysLookupP] ADD  DEFAULT ((0)) FOR [sort]
GO
ALTER TABLE [dbo].[sysLookupC] ADD  DEFAULT ((0)) FOR [sort]
GO
ALTER TABLE [dbo].[sysLookupC] ADD  DEFAULT ((100)) FOR [width]
GO
ALTER TABLE [dbo].[sysLookupC] ADD  DEFAULT ('Y') FOR [visible_yn]
GO
ALTER TABLE [dbo].[sysPopUpS] ADD  DEFAULT ('TEXT') FOR [control_type]
GO
ALTER TABLE [dbo].[sysPopUpS] ADD  DEFAULT ((0)) FOR [sort]
GO
ALTER TABLE [dbo].[sysPopUpS] ADD  DEFAULT ((120)) FOR [width]
GO
ALTER TABLE [dbo].[TSMUSERGRPMAP]  WITH CHECK ADD  CONSTRAINT [FK_TSMUSERGRPMAP_GRP] FOREIGN KEY([USER_GRP_CD])
REFERENCES [dbo].[TSMUSERGRP] ([USER_GRP_CD])
GO
ALTER TABLE [dbo].[TSMUSERGRPMAP] CHECK CONSTRAINT [FK_TSMUSERGRPMAP_GRP]
GO
ALTER TABLE [dbo].[sysPopUpD]  WITH CHECK ADD  CONSTRAINT [FK_sysPopUpD_sysPopUpM] FOREIGN KEY([popup_key])
REFERENCES [dbo].[sysPopUpM] ([popup_key])
GO
ALTER TABLE [dbo].[sysPopUpD] CHECK CONSTRAINT [FK_sysPopUpD_sysPopUpM]
GO
ALTER TABLE [dbo].[sysLookupP]  WITH CHECK ADD  CONSTRAINT [FK_sysLookupP_sysLookupM] FOREIGN KEY([lookup_key])
REFERENCES [dbo].[sysLookupM] ([lookup_key])
GO
ALTER TABLE [dbo].[sysLookupP] CHECK CONSTRAINT [FK_sysLookupP_sysLookupM]
GO
ALTER TABLE [dbo].[sysLookupC]  WITH CHECK ADD  CONSTRAINT [FK_sysLookupC_sysLookupM] FOREIGN KEY([lookup_key])
REFERENCES [dbo].[sysLookupM] ([lookup_key])
GO
ALTER TABLE [dbo].[sysLookupC] CHECK CONSTRAINT [FK_sysLookupC_sysLookupM]
GO
ALTER TABLE [dbo].[sysPopUpS]  WITH CHECK ADD  CONSTRAINT [FK_sysPopUpS_sysPopUpM] FOREIGN KEY([popup_key])
REFERENCES [dbo].[sysPopUpM] ([popup_key])
GO
ALTER TABLE [dbo].[sysPopUpS] CHECK CONSTRAINT [FK_sysPopUpS_sysPopUpM]
GO
ALTER TABLE [dbo].[TSMSITECONFIG]  WITH CHECK ADD  CONSTRAINT [CK_TSMSITECONFIG_SingleRow] CHECK  (([config_id]=(1)))
GO
ALTER TABLE [dbo].[TSMSITECONFIG] CHECK CONSTRAINT [CK_TSMSITECONFIG_SingleRow]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- ==================== 4) 프로시저 수정 ====================

CREATE   PROCEDURE SSP_CBO_ACC_Q
AS
BEGIN
    SET NOCOUNT ON;

    SELECT acc_id, acc_nm
    FROM TBAACC
    WHERE 1 = 1;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- SSP_CBO_CODE_Q의 파라미터 이름을 @p_major_code로 통일한다. 로컬 개발DB에는 @p_code로,
-- 운영DB에는 @p_where로 서로 다르게 올라가 있었다(둘 다 실제로 겪음 - 로컬은 @p_code로 확인,
-- 운영은 API가 @p_code를 보냈는데 "매개 변수 '@p_where'이(가) 필요하지만 제공되지 않았습니다"
-- SqlException으로 확인됨). 이 프로시저는 LookUpEditWyn.ProcName/Where를 통해 화면 어디서든
-- 공용으로 부르는 콤보 조회라(WYNLAB.BaseForm.ControlDataSources 참고), 파라미터 이름이
-- 환경마다 다르면 그때그때 API 쪽 파라미터 이름도 맞춰 바꿔야 해서 혼란스럽다 - 값의 실제
-- 의미(대분류코드)를 그대로 드러내는 이름 하나로 정리한다.
--exec ssp_cbo_code_q 'HR0001'
CREATE   PROCEDURE [dbo].[SSP_CBO_CODE_Q]
    @p_major_cd VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT minor_cd, minor_nm
    FROM TSMMINOR
    WHERE major_cd = @p_major_cd
      AND use_yn = 'Y'
    ORDER BY sort;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE   PROCEDURE [dbo].[SSP_CBO_ITEM_GRP_Q]
    @p_grp_lvl VARCHAR(20), 
    @p_par_grp_id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT grp_id, grp_nm, grp_lvl, par_grp_id
    FROM TBAITEMGRP
    WHERE grp_lvl = @p_grp_lvl
    AND     (@p_par_grp_id IS NULL OR par_grp_id LIKE @p_par_grp_id +'%')
    order by grp_lvl, grp_nm 
      
END



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- SSP_CBO_MINOR_Q의 파라미터 이름을 @p_major_code로 통일한다. 로컬 개발DB에는 @p_code로,
-- 운영DB에는 @p_where로 서로 다르게 올라가 있었다(둘 다 실제로 겪음 - 로컬은 @p_code로 확인,
-- 운영은 API가 @p_code를 보냈는데 "매개 변수 '@p_where'이(가) 필요하지만 제공되지 않았습니다"
-- SqlException으로 확인됨). 이 프로시저는 LookUpEditWyn.ProcName/Where를 통해 화면 어디서든
-- 공용으로 부르는 콤보 조회라(WYNLAB.BaseForm.ControlDataSources 참고), 파라미터 이름이
-- 환경마다 다르면 그때그때 API 쪽 파라미터 이름도 맞춰 바꿔야 해서 혼란스럽다 - 값의 실제
-- 의미(대분류코드)를 그대로 드러내는 이름 하나로 정리한다.
--exec SSP_CBO_MINOR_Q 'HR0001'
CREATE   PROCEDURE [dbo].[SSP_CBO_MINOR_Q]
    @p_major_cd VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT minor_cd, minor_nm
    FROM TSMMINOR
    WHERE major_cd = @p_major_cd
      AND use_yn = 'Y'
    ORDER BY sort;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* =========================================================
   080(TBADEPT DEPT_CD->DEPT_ID)에 딸린 프로시저/팝업 메타데이터 재작성 - 073(MenuId_Procs)과
   같은 패턴. dept_cd를 참조하던 모든 프로시저에서 조인/파라미터/출력 컬럼을 dept_id 기준으로
   바꾼다. "사용자가 부서코드를 알 필요 없다"는 전제라, 화면에 코드를 보여주거나 코드로
   검색하던 자리는 전부 이름(dept_nm) 기준으로 대체하거나 그냥 없앤다(SSP_WYNLAB_GetSession/
   USP_SM_USERAUTH_Q의 DeptCd 출력 컬럼 자체를 삭제 - DeptNm만 남김, 아무 화면도 DeptCd를
   실제로 안 쓰고 있었다).
   ========================================================= */

-- ============================================================
-- 1) SSP_POP_DEPT_Q - P_DEPT 팝업 소스. 검색은 이름으로만(코드 검색 제거).
-- ============================================================
CREATE   PROCEDURE [dbo].[SSP_POP_DEPT_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DEPT_ID, dept_nm, PAR_DEPT_ID, dept_type, remark
    FROM TBADEPT
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR dept_nm LIKE '%' + @p_keyword + '%')
    ORDER BY DEPT_ID;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[SSP_POP_EMP_Q]
    @p_code        VARCHAR(100) = NULL,
    @p_dept_nm     NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  a.EMP_ID,
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date
    FROM   TBAEMP as a
                JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
    WHERE  (@p_code IS NULL OR @p_code = ''
                OR a.emp_no LIKE '%' + @p_code + '%'
                OR a.emp_nm LIKE '%' + @p_code + '%')
    AND    (@p_dept_nm IS NULL OR @p_dept_nm = '' OR b.dept_nm LIKE '%' + @p_dept_nm + '%')
    ORDER BY a.emp_no;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* =========================================================
   072(TSMMENU MENU_CD->MENU_ID)에 딸린 프로시저 13개 재작성 - SSP_POP_MENU_Q,
   USP_SM_MENU_Q/Q_1/Q_2/S/S_1, USP_SM_MENUAUTH_Q/Q_1/Q_2/S_1/S_2, USP_SM_GRIDLAYOUT_Q/S.

   USP_SM_MENU_Q_1은 예전엔 "이 코드가 이미 있는지"(신규등록 중복확인)였는데, MENU_ID가
   IDENTITY라 더는 그 확인이 필요없다 - 대신 범용적인 "이 ID가 실제로 있는지" 확인으로 바꿨다
   (수정/삭제 전 존재 확인 등에 재사용 가능).

   USP_SM_MENU_S는 이제 TSMMENU에 표준 감사컬럼(reg_user_id 등)이 생겨서 @p_user_id/
   @p_client_pc를 새로 받는다 - 서버 쪽(MenuManageRepository/MenusController)에서 이 값을
   채워 넘기도록 다음 단계(서버)에서 고쳐야 한다.
   ========================================================= */

CREATE   PROCEDURE [dbo].[SSP_POP_MENU_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MENU_ID AS menu_id, MENU_NM AS menu_nm, UPPER_MENU_ID AS upper_menu_id, MENU_TYPE AS menu_type
    FROM TSMMENU
    WHERE USE_YN = 'Y'
      AND (@p_keyword IS NULL OR @p_keyword = '' OR MENU_NM LIKE '%' + @p_keyword + '%')
    ORDER BY MENU_NM;
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ---------- LOGIN (011 원본을 020이 표준출력 붙여 재정의한 것 기준) ---------- */

/* 로그인 성공 처리 - update 성격이라 work_type='U' */
CREATE   PROCEDURE [dbo].[SSP_SYS_LOGIN_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER
            SET LAST_LOGIN_DT = GETDATE(), PWD_FAIL_CNT = 0
            WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* 로그인 실패 카운트 증가 - update 성격이라 work_type='U' */
CREATE   PROCEDURE [dbo].[SSP_SYS_LOGIN_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER
            SET PWD_FAIL_CNT = PWD_FAIL_CNT + 1
            WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* 로그인 이력 적재 - insert라 work_type='N' */
CREATE   PROCEDURE [dbo].[SSP_SYS_LOGIN_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    @p_client_ip VARCHAR(50),
    @p_client_version VARCHAR(100),
    @p_result_cd VARCHAR(10),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMLOGINHIST (USER_ID, CLIENT_IP, CLIENT_VERSION, RESULT_CD)
            VALUES (@p_user_id, @p_client_ip, @p_client_version, @p_result_cd);
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- 065_Lookup_Columns.sql가 USP_SYS_LOOKUP_Q/USP_SYS_LOOKUP_S를 CREATE OR ALTER할 때, 064_Lookup_
-- Query_Source.sql이 이미 추가해둔 source_type/proc_nm(nullable)/query_txt 처리를 빠뜨린 채(더
-- 오래된 버전 본문을 베이스로 써서) 덮어써버렸다 - "쿼리로 만든 LookUp이 소스유형/쿼리문을 안
-- 보여준다"는 회귀 버그(2026-09-03 실제 발견, "실수임을 인정 - 065를 쓸 때 064의 변경을 놓침").
-- sysLookupM 테이블 자체(source_type/query_txt 컬럼과 데이터)는 전혀 안 건드려서 데이터 손실은
-- 없다 - 프로시져 SELECT/파라미터 목록에서만 빠졌던 것. 이 파일이 065의 실수를 되돌리고
-- 065에서 새로 추가한 것(sysLookupC 관련 Q2 분기, D 분기의 sysLookupC 삭제)은 그대로 유지한다.
-- USP_SYS_LOOKUP_S_1/USP_SYS_LOOKUP_S_2는 이 회귀와 무관해서 다시 안 건드린다.

CREATE   PROCEDURE [dbo].[SSP_SYS_LOOKUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30) = NULL,   /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'/'Q2'일 때는 정확히 일치하는 LookUp키 */
    @p_lookup_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT lookup_key, source_type, proc_nm, query_txt, lookup_nm, value_field, display_field, use_yn, remark
            FROM sysLookupM
            WHERE (@p_lookup_key IS NULL OR lookup_key LIKE '%' + @p_lookup_key + '%')
              AND (@p_lookup_nm IS NULL OR lookup_nm LIKE '%' + @p_lookup_nm + '%')
            ORDER BY lookup_key;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT lookup_key, param_nm, caption, sort
            FROM sysLookupP
            WHERE lookup_key = @p_lookup_key
            ORDER BY sort;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            SELECT lookup_key, column_nm, caption, sort, width, visible_yn
            FROM sysLookupC
            WHERE lookup_key = @p_lookup_key
            ORDER BY sort;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[SSP_SYS_LOOKUP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_source_type CHAR(1) = 'P',
    @p_proc_nm VARCHAR(100) = NULL,
    @p_query_txt NVARCHAR(MAX) = NULL,
    @p_lookup_nm NVARCHAR(100) = NULL,
    @p_value_field VARCHAR(50) = NULL,
    @p_display_field VARCHAR(50) = NULL,
    @p_use_yn VARCHAR(1) = 'Y',
    @p_remark NVARCHAR(200) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysLookupM (
                lookup_key, source_type, proc_nm, query_txt, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt
            )
            VALUES (
                @p_lookup_key, @p_source_type, @p_proc_nm, @p_query_txt, @p_lookup_nm, @p_value_field, @p_display_field, @p_use_yn, @p_remark, @p_user_id, GETDATE()
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupM SET
                source_type = @p_source_type, proc_nm = @p_proc_nm, query_txt = @p_query_txt,
                lookup_nm = @p_lookup_nm, value_field = @p_value_field,
                display_field = @p_display_field, use_yn = @p_use_yn, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHERE lookup_key = @p_lookup_key;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupC WHERE lookup_key = @p_lookup_key;
            DELETE FROM sysLookupP WHERE lookup_key = @p_lookup_key;
            DELETE FROM sysLookupM WHERE lookup_key = @p_lookup_key;
        END

        SET @GeneratedCode = @p_lookup_key;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[SSP_SYS_LOOKUP_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_param_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_sort INT = 0,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysLookupP (lookup_key, param_nm, caption, sort)
            VALUES (@p_lookup_key, @p_param_nm, @p_caption, @p_sort);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupP SET caption = @p_caption, sort = @p_sort
            WHERE lookup_key = @p_lookup_key AND param_nm = @p_param_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupP WHERE lookup_key = @p_lookup_key AND param_nm = @p_param_nm;
        END

        SET @GeneratedCode = @p_param_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- USP_SYS_LOOKUP_S_1(sysLookupP, 파라미터)과 같은 구조로 sysLookupC(컬럼) 저장/삭제를 처리한다.
CREATE   PROCEDURE [dbo].[SSP_SYS_LOOKUP_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lookup_key VARCHAR(30),
    @p_column_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_sort INT = 0,
    @p_width INT = 100,
    @p_visible_yn VARCHAR(1) = 'Y',
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysLookupC (lookup_key, column_nm, caption, sort, width, visible_yn)
            VALUES (@p_lookup_key, @p_column_nm, @p_caption, @p_sort, @p_width, @p_visible_yn);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysLookupC SET caption = @p_caption, sort = @p_sort, width = @p_width, visible_yn = @p_visible_yn
            WHERE lookup_key = @p_lookup_key AND column_nm = @p_column_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysLookupC WHERE lookup_key = @p_lookup_key AND column_nm = @p_column_nm;
        END

        SET @GeneratedCode = @p_column_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[SSP_SYS_POPUP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30) = NULL,    /* work_type='Q'일 때는 검색조건(부분일치), 'Q1'/'Q2'일 때는 정확히 일치하는 팝업키 */
    @p_popup_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                   display_field, popup_width, popup_height, use_yn, remark
            FROM sysPopUpM
            WHERE (@p_popup_key IS NULL OR popup_key LIKE '%' + @p_popup_key + '%')
              AND (@p_popup_nm IS NULL OR popup_nm LIKE '%' + @p_popup_nm + '%')
            ORDER BY popup_key;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn
            FROM sysPopUpD
            WHERE popup_key = @p_popup_key
            ORDER BY sort;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            SELECT popup_key, param_nm, caption, control_type, sort, width
            FROM sysPopUpS
            WHERE popup_key = @p_popup_key
            ORDER BY sort;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[SSP_SYS_POPUP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_proc_nm VARCHAR(100) = NULL,
    @p_popup_nm NVARCHAR(100) = NULL,
    @p_hierarchical_yn VARCHAR(1) = 'N',
    @p_key_field VARCHAR(50) = NULL,
    @p_parent_field VARCHAR(50) = NULL,
    @p_display_field VARCHAR(50) = NULL,
    @p_popup_width INT = 700,
    @p_popup_height INT = 500,
    @p_use_yn VARCHAR(1) = 'Y',
    @p_remark NVARCHAR(200) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysPopUpM (
                popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                display_field, popup_width, popup_height, use_yn, remark, reg_user_id, reg_dt
            )
            VALUES (
                @p_popup_key, @p_proc_nm, @p_popup_nm, @p_hierarchical_yn, @p_key_field, @p_parent_field,
                @p_display_field, @p_popup_width, @p_popup_height, @p_use_yn, @p_remark, @p_user_id, GETDATE()
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpM SET
                proc_nm = @p_proc_nm, popup_nm = @p_popup_nm, hierarchical_yn = @p_hierarchical_yn,
                key_field = @p_key_field, parent_field = @p_parent_field, display_field = @p_display_field,
                popup_width = @p_popup_width, popup_height = @p_popup_height, use_yn = @p_use_yn,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHERE popup_key = @p_popup_key;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpD WHERE popup_key = @p_popup_key;
            DELETE FROM sysPopUpM WHERE popup_key = @p_popup_key;
        END

        SET @GeneratedCode = @p_popup_key;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- 컬럼 설정 그리드(grd2)의 행 하나 - frmSysPopup.cs가 Added/Modified/Deleted 행마다 이걸 한
-- 번씩 부른다(USP_SM_MINORCODE_S_1과 같은 구조). column_nm이 키라 'U' 분기도 WHERE에서만 쓴다.
CREATE   PROCEDURE [dbo].[SSP_SYS_POPUP_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_column_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_control_type VARCHAR(10) = 'TEXT',
    @p_lookup_proc_nm VARCHAR(100) = NULL,
    @p_sort INT = 0,
    @p_width INT = 100,
    @p_visible_yn VARCHAR(1) = 'Y',
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
            VALUES (@p_popup_key, @p_column_nm, @p_caption, @p_control_type, @p_lookup_proc_nm, @p_sort, @p_width, @p_visible_yn);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpD SET
                caption = @p_caption, control_type = @p_control_type, lookup_proc_nm = @p_lookup_proc_nm,
                sort = @p_sort, width = @p_width, visible_yn = @p_visible_yn
            WHERE popup_key = @p_popup_key AND column_nm = @p_column_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpD WHERE popup_key = @p_popup_key AND column_nm = @p_column_nm;
        END

        SET @GeneratedCode = @p_column_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- 조회조건 그리드(grd3)의 행 하나 - USP_SYS_POPUP_S_1(컬럼설정)과 완전히 같은 구조,
-- param_nm이 키라 'U' 분기도 WHERE에서만 쓴다.
CREATE   PROCEDURE [dbo].[SSP_SYS_POPUP_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_popup_key VARCHAR(30),
    @p_param_nm VARCHAR(50),
    @p_caption NVARCHAR(50) = NULL,
    @p_control_type VARCHAR(10) = 'TEXT',
    @p_sort INT = 0,
    @p_width INT = 120,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width)
            VALUES (@p_popup_key, @p_param_nm, @p_caption, @p_control_type, @p_sort, @p_width);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE sysPopUpS SET
                caption = @p_caption, control_type = @p_control_type, sort = @p_sort, width = @p_width
            WHERE popup_key = @p_popup_key AND param_nm = @p_param_nm;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM sysPopUpS WHERE popup_key = @p_popup_key AND param_nm = @p_param_nm;
        END

        SET @GeneratedCode = @p_param_nm;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE SSP_WYNLAB_GetSession
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                U.USER_ID       AS UserId,
                U.USER_NM       AS UserNm,
                E.emp_no        AS EmpNo,
                D.DEPT_NM       AS DeptNm,
                U.PASSWORD_HASH AS PasswordHash,
                U.EMAIL         AS Email,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn,
                U.ACC_ID        AS AccId,
                A.ACC_NM        AS AccNm
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            LEFT JOIN TBAACC A ON A.ACC_ID = U.ACC_ID
            WHERE U.USER_ID = @p_user_id;

            SELECT USER_GRP_CD
            FROM TSMUSERGRPMAP
            WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- 전자결재 화면의 부서트리+사원목록(treeDept+grdEmp)을 부서/사원 통합 조직도 트리(treeEmp)
-- 하나로 합치기 위한 선행 작업. 통합 트리는 부서 목록(Q2)과 사원 전체 목록을 한 번에 받아
-- 클라이언트에서 "D"+dept_id / "E"+emp_id 키로 합쳐 그린다 - 이때 각 사원이 어느 부서 밑에
-- 붙는지 알아야 하므로 Q3 결과에 DEPT_ID가 반드시 있어야 한다(기존엔 없었음).
--
-- 기존 Q3(부서별 사원목록)은 @p_dept_id가 항상 필수였다(트리가 부서 선택 시점에 그 부서만
-- 조회). 조직도 화면은 처음 열릴 때 전체 사원을 한 번에 받아야 하므로,
-- @p_dept_id가 NULL이면 "전체 사원"을 의미하도록 WHERE절만 바꾼다(파라미터 자체는 이미
-- 프로시저 선언에서 NULL 기본값이었음 - 이 분기 안에서 그 의미를 실제로 살린 적이 없었을 뿐).
CREATE   PROCEDURE USP_AP_APPR_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_route_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q' -- 특정 문서 1건의 결재이력(헤더 요약본 + 전 결재경로, 결재+수신 구분 없이)
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.app_text, a.doc_type, a.doc_id,
                   a.doc_no, a.form_id, a.end_yn, a.end_dt, a.rtn_yn, a.rtn_dt, a.stat_cd, a.emp_id, e.emp_nm
            FROM TAPDOC a
            LEFT JOIN TBAEMP e ON e.EMP_ID = a.emp_id
            WHERE a.doc_type = @p_doc_type AND a.doc_id = @p_doc_id
            ORDER BY a.app_id DESC;

            SELECT p.app_id, p.serl, p.sort, p.path_type, p.emp_id, ep.emp_no, ep.emp_nm, p.stat_cd, p.app_dt, p.remark
            FROM TAPDOCPATH p
            LEFT JOIN TBAEMP ep ON ep.EMP_ID = p.emp_id
            WHERE p.doc_type = @p_doc_type AND p.doc_id = @p_doc_id
            ORDER BY p.app_id DESC, p.path_type, p.sort;
        END
        ELSE IF @p_work_type = 'Q1' -- 결재함: 로그인 사용자가 지금 처리해야 함(자기 차례임) 목록
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   p.serl, p.sort, p.path_type, e2.emp_nm AS req_emp_nm
            FROM TAPDOCPATH p
            JOIN TAPDOC a ON a.app_id = p.app_id
            JOIN TBAEMP e ON e.emp_no = @p_emp_no
            LEFT JOIN TBAEMP e2 ON e2.EMP_ID = a.emp_id
            WHERE p.emp_id = e.EMP_ID AND p.path_type = 'A' AND p.stat_cd = 'N'
              AND a.end_yn = 'N' AND a.rtn_yn = 'N'
              AND NOT EXISTS (
                  SELECT 1 FROM TAPDOCPATH q
                  WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort < p.sort AND q.stat_cd <> 'Y'
              )
            ORDER BY a.app_id;
        END
        ELSE IF @p_work_type = 'Q2' -- 부서트리(전체)
        BEGIN
            SELECT dept_id, dept_nm, par_dept_id, dept_type
            FROM TBADEPT
            ORDER BY dept_id;
        END
        ELSE IF @p_work_type = 'Q3' -- 사원목록(@p_dept_id 지정 시 그 부서만, NULL이면 전체 - 조직도 트리용)
        BEGIN
            SELECT EMP_ID, emp_no, emp_nm, job_grade, DEPT_ID
            FROM TBAEMP
            WHERE @p_dept_id IS NULL OR DEPT_ID = @p_dept_id
            ORDER BY emp_no;
        END
        ELSE IF @p_work_type = 'Q4' -- 내 결재경로 목록
        BEGIN
            DECLARE @my_emp_id BIGINT;
            SELECT @my_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            SELECT route_id, route_nm
            FROM TAPROUTE
            WHERE emp_id = @my_emp_id AND use_yn = 'Y'
            ORDER BY route_nm;
        END
        ELSE IF @p_work_type = 'Q5' -- 결재경로 상세
        BEGIN
            SELECT d.route_id, d.sort, d.emp_id, e.emp_no, e.emp_nm, d.path_type
            FROM TAPROUTEDETAIL d
            LEFT JOIN TBAEMP e ON e.EMP_ID = d.emp_id
            WHERE d.route_id = @p_route_id
            ORDER BY d.path_type, d.sort;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_AP_APPR_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_app_id BIGINT = NULL,
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_app_title NVARCHAR(500) = NULL,
    @p_app_text NVARCHAR(4000) = NULL,
    @p_form_id VARCHAR(30) = NULL,
    @p_target_emp_no VARCHAR(20) = NULL,
    @p_path_type VARCHAR(10) = NULL,
    @p_opinion NVARCHAR(1000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N' -- 결재상신: TAPDOC 헤더 + TAPDOCPATH sort=1(기안자 본인, 즉시 승인완료)
        BEGIN
            DECLARE @req_dept_id BIGINT, @req_emp_id BIGINT, @req_acc_id BIGINT;
            SELECT @req_emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
            SELECT @req_dept_id = DEPT_ID, @req_acc_id = acc_id FROM TBAEMP WHERE EMP_ID = @req_emp_id;

            INSERT INTO TAPDOC (
                app_no, app_date, app_title, app_text, form_id, doc_type, doc_id, doc_no,
                acc_id, dept_id, emp_id, end_yn, rtn_yn, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', CONVERT(VARCHAR(8), GETDATE(), 112), @p_app_title, @p_app_text, @p_form_id, @p_doc_type, @p_doc_id, @p_doc_no,
                @req_acc_id, @req_dept_id, @req_emp_id, 'N', 'N', 'P',
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_app_id = SCOPE_IDENTITY();
            UPDATE TAPDOC
               SET app_no = 'AP' + RIGHT('00000000' + CAST(@p_app_id AS VARCHAR(8)), 8)
             WHERE app_id = @p_app_id;

            -- 기안자 본인 = 결재라인 sort=1, 상신 자체가 승인완료 데이터.
            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd, app_dt,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, 1, app_no, @p_doc_type, @p_doc_id, @p_doc_no, 1, 'A', @req_emp_id, 'Y', GETDATE(),
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            IF @p_doc_type = 'NAMECARD'
                UPDATE THRNAMECARDREQ
                   SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
                 WHERE req_id = @p_doc_id;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'ADDPATH' -- 결재라인('A') 또는 수신라인('F')에 한 명 추가
        BEGIN
            DECLARE @target_emp_id BIGINT;
            SELECT @target_emp_id = EMP_ID FROM TBAEMP WHERE emp_no = @p_target_emp_no;
            IF @target_emp_id IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 사원 정보를 찾을 수 없습니다.';
                RETURN;
            END

            DECLARE @nextSerl INT, @nextSort INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TAPDOCPATH WHERE app_id = @p_app_id;
            IF @p_path_type = 'A'
                SELECT @nextSort = ISNULL(MAX(sort), 0) + 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'A';
            ELSE
                SET @nextSort = 0;

            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, @nextSerl, app_no, doc_type, doc_id, doc_no, @nextSort, @p_path_type, @target_emp_id, 'N',
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'A' -- 승인 (결재라인, 순서 게이트 적용)
        BEGIN
            DECLARE @apprv_emp_id BIGINT;
            SELECT @apprv_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND path_type = 'A' AND stat_cd = 'N')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @apprv_emp_id AND p.path_type = 'A' AND p.stat_cd = 'N'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort < p.sort AND q.stat_cd <> 'Y')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'아직 앞 순번 결재가 완료되지 않았습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'Y', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND path_type = 'A' AND stat_cd = 'N';

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND path_type = 'A' AND stat_cd = 'N')
            BEGIN
                UPDATE TAPDOC
                   SET end_yn = 'Y', end_emp_id = @apprv_emp_id, end_dt = GETDATE(), stat_cd = 'Y',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;

                DECLARE @fin_doc_type VARCHAR(10), @fin_doc_id BIGINT;
                SELECT @fin_doc_type = doc_type, @fin_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                IF @fin_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET stat_cd = 'C' WHERE req_id = @fin_doc_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'R' -- 반려 (자기 차례일 때만, 승인과 같은 게이트)
        BEGIN
            DECLARE @rej_emp_id BIGINT;
            SELECT @rej_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND path_type = 'A' AND stat_cd = 'N')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @rej_emp_id AND p.path_type = 'A' AND p.stat_cd = 'N'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort < p.sort AND q.stat_cd <> 'Y')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'아직 앞 순번 결재가 완료되지 않았습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'R', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND path_type = 'A' AND stat_cd = 'N';

            UPDATE TAPDOC
               SET rtn_yn = 'Y', rtn_emp_id = @rej_emp_id, rtn_dt = GETDATE(), stat_cd = 'R',
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id;

            -- 반려는 THRNAMECARDREQ.stat_cd(진행상태)를 건드리지 않는다 - 123번과 동일.
            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'C' -- 승인취소: 방금 내가 한 승인을 되돌림(뒷사람이 이미 승인했으면 불가)
        BEGIN
            DECLARE @undo_emp_id BIGINT;
            SELECT @undo_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'A' AND stat_cd = 'Y')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'취소할 승인 내역을 찾을 수 없습니다.';
                RETURN;
            END

            IF EXISTS (
                SELECT 1 FROM TAPDOCPATH p
                WHERE p.app_id = @p_app_id AND p.emp_id = @undo_emp_id AND p.path_type = 'A' AND p.stat_cd = 'Y'
                  AND EXISTS (SELECT 1 FROM TAPDOCPATH q WHERE q.app_id = p.app_id AND q.path_type = 'A' AND q.sort > p.sort AND q.stat_cd = 'Y')
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이후 순번이 이미 승인하여 취소할 수 없습니다.';
                RETURN;
            END

            UPDATE TAPDOCPATH
               SET stat_cd = 'N', app_dt = NULL, remark = NULL,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @undo_emp_id AND path_type = 'A' AND stat_cd = 'Y';

            IF EXISTS (SELECT 1 FROM TAPDOC WHERE app_id = @p_app_id AND end_yn = 'Y')
            BEGIN
                UPDATE TAPDOC
                   SET end_yn = 'N', end_emp_id = NULL, end_dt = NULL, stat_cd = 'P',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;

                DECLARE @undo_doc_type VARCHAR(10), @undo_doc_id BIGINT;
                SELECT @undo_doc_type = doc_type, @undo_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                IF @undo_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET stat_cd = '0' WHERE req_id = @undo_doc_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'F' -- 수신확인 (게이트 없음, 결재 흐름에 영향 없음)
        BEGIN
            DECLARE @ack_emp_id BIGINT;
            SELECT @ack_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            UPDATE TAPDOCPATH
               SET stat_cd = 'Y', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @ack_emp_id AND path_type = 'F' AND stat_cd = 'N';

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'확인할 수신 건을 찾을 수 없습니다.';
                RETURN;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ---------- USP_AP_ROUTE_S: 개인별 저장된 결재경로 관리 ---------- */
CREATE   PROCEDURE USP_AP_ROUTE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_route_nm NVARCHAR(100) = NULL,
    @p_target_emp_no VARCHAR(20) = NULL,
    @p_path_type VARCHAR(10) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N' -- 경로 헤더 신규
        BEGIN
            DECLARE @owner_emp_id BIGINT;
            SELECT @owner_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            INSERT INTO TAPROUTE (emp_id, route_nm, use_yn, reg_user_id, reg_dt, reg_pc)
            VALUES (@owner_emp_id, @p_route_nm, 'Y', @p_user_id, GETDATE(), @p_client_pc);

            SET @p_route_id = SCOPE_IDENTITY();
            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'ADDDETAIL' -- 경로 상세 한 명 추가
        BEGIN
            DECLARE @target_emp_id BIGINT;
            SELECT @target_emp_id = EMP_ID FROM TBAEMP WHERE emp_no = @p_target_emp_no;
            IF @target_emp_id IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 사원 정보를 찾을 수 없습니다.';
                RETURN;
            END

            DECLARE @nextSort INT;
            SELECT @nextSort = ISNULL(MAX(sort), 0) + 1 FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;

            INSERT INTO TAPROUTEDETAIL (route_id, sort, emp_id, path_type)
            VALUES (@p_route_id, @nextSort, @target_emp_id, @p_path_type);

            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D' -- 경로 삭제(소유자 본인만)
        BEGIN
            DECLARE @del_emp_id BIGINT;
            SELECT @del_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            DELETE FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;
            DELETE FROM TAPROUTE WHERE route_id = @p_route_id AND emp_id = @del_emp_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- frmAcc.Designer.cs의 panData에 남은 4개 컬럼(biz_kind/biz_type/vat_type/vat_rate) 컨트롤을
-- 추가했다(사장님 작업, 128번 마이그레이션 코멘트에서 "컨트롤이 생기면 같이 추가"라고 남겨둔
-- 부분) - 이 파일이 그 DB 배관 마무리다. TBAACC 컬럼 자체는 이미 있었다(32개 컬럼에 포함).

CREATE   PROCEDURE USP_BA_ACC_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_acc_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT acc_id, acc_nm, biz_no, tel, cur_cd, owner_nm, owner_nm_eng,
                   zip_code, addr1, addr2, addr1_eng, addr2_eng,
                   homepage, email, fax, open_date, biz_kind, biz_type, vat_type, vat_rate,
                   logo, stamp
            FROM TBAACC
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_acc_nm IS NULL OR acc_nm LIKE '%' + @p_acc_nm + '%')
            ORDER BY acc_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE USP_BA_ACC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_acc_nm NVARCHAR(100) = NULL,
    @p_biz_no VARCHAR(30) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_cur_cd VARCHAR(3) = NULL,
    @p_owner_nm NVARCHAR(100) = NULL,
    @p_owner_nm_eng NVARCHAR(100) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(1000) = NULL,
    @p_addr2 NVARCHAR(1000) = NULL,
    @p_addr1_eng NVARCHAR(1000) = NULL,
    @p_addr2_eng NVARCHAR(1000) = NULL,
    @p_homepage NVARCHAR(200) = NULL,
    @p_email NVARCHAR(100) = NULL,
    @p_fax VARCHAR(30) = NULL,
    @p_open_date VARCHAR(8) = NULL,
    @p_biz_kind NVARCHAR(200) = NULL,
    @p_biz_type NVARCHAR(200) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(19, 2) = NULL,
    @p_logo NVARCHAR(MAX) = NULL,      -- Base64 인코딩된 이미지 - NULL/빈 문자열이면 기존 값을 그대로 둔다
    @p_stamp NVARCHAR(MAX) = NULL,     -- 위와 동일
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        DECLARE @logo_bin VARBINARY(MAX) = NULL;
        DECLARE @stamp_bin VARBINARY(MAX) = NULL;
        IF @p_logo IS NOT NULL AND LEN(@p_logo) > 0
            SET @logo_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_logo"))', 'VARBINARY(MAX)');
        IF @p_stamp IS NOT NULL AND LEN(@p_stamp) > 0
            SET @stamp_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_stamp"))', 'VARBINARY(MAX)');

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAACC (
                acc_nm, biz_no, tel, cur_cd, owner_nm, owner_nm_eng,
                zip_code, addr1, addr2, addr1_eng, addr2_eng,
                homepage, email, fax, open_date, biz_kind, biz_type, vat_type, vat_rate,
                logo, stamp,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_owner_nm_eng,
                @p_zip_code, @p_addr1, @p_addr2, @p_addr1_eng, @p_addr2_eng,
                @p_homepage, @p_email, @p_fax, @p_open_date, @p_biz_kind, @p_biz_type, @p_vat_type, @p_vat_rate,
                @logo_bin, @stamp_bin,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_acc_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAACC SET
                acc_nm = @p_acc_nm, biz_no = @p_biz_no, tel = @p_tel, cur_cd = @p_cur_cd,
                owner_nm = @p_owner_nm, owner_nm_eng = @p_owner_nm_eng,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                addr1_eng = @p_addr1_eng, addr2_eng = @p_addr2_eng,
                homepage = @p_homepage, email = @p_email, fax = @p_fax, open_date = @p_open_date,
                biz_kind = @p_biz_kind, biz_type = @p_biz_type, vat_type = @p_vat_type, vat_rate = @p_vat_rate,
                logo = ISNULL(@logo_bin, logo), stamp = ISNULL(@stamp_bin, stamp),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @p_acc_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAACC WHERE acc_id = @p_acc_id;
        END

        SET @GeneratedCode = CAST(@p_acc_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* ---------- emp_no -> EMP_ID 완전 전환(089)에 맞춰 관련 프로시저 전부 수정 ----------
   TBACUST/TBAITEM의 목록/리스트 조회는 TBAEMP를 EMP_ID로 조인해서 emp_nm을 같이 보여준다
   (읽기전용 그리드라 이름으로 보여주는 게 낫다 - DEPT_ID/frmItemList 때와 같은 논리).
   상세입력 필드(txtDetailEmpNo/txtEmpNo)는 여전히 원시 ID를 입력받는 텍스트박스 그대로 둔다
   (팝업 작업은 사장님이 frmCust/frmItem에 직접 하겠다고 함 - 여긴 DB 배관만 정리).
   USP_SM_USERAUTH_S는 이제 EMP_ID를 그대로 받아 저장한다(더는 emp_no로 TBAEMP를 찾아
   내부에서 변환할 필요 없음 - 화면이 팝업에서 고른 EMP_ID를 직접 넘겨준다). */

CREATE   PROCEDURE [dbo].[USP_BA_CUST_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                a.CUST_ID, a.cust_nm, a.biz_no, a.tel, a.cur_cd, a.owner_nm, a.zip_code, a.addr1, a.addr2,
                a.homepage, a.email, a.fax, a.biz_kind, a.biz_type, a.trans_open_date, a.vat_type, a.vat_rate,
                a.remark, a.stat_cd, a.EMP_ID, b.emp_nm
            FROM TBACUST a
            LEFT JOIN TBAEMP b ON a.EMP_ID = b.EMP_ID
            WHERE  1 =1 
                --AND (@p_cust_id IS NULL OR a.CUST_ID = @p_cust_id)
              AND (@p_cust_nm IS NULL OR a.cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY a.CUST_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT CUST_ID, serl, prsn_nm, grade, tel1, tel2, fax, email
            FROM TBACUSTPRSN
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;

            SELECT CUST_ID, serl, bank_cd, acnt_no, remark
            FROM TBACUSTACNT
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_BA_CUST_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
    @p_biz_no VARCHAR(30) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_cur_cd VARCHAR(3) = NULL,
    @p_owner_nm NVARCHAR(100) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(1000) = NULL,
    @p_addr2 NVARCHAR(1000) = NULL,
    @p_homepage NVARCHAR(200) = NULL,
    @p_email NVARCHAR(100) = NULL,
    @p_fax VARCHAR(30) = NULL,
    @p_biz_kind NVARCHAR(200) = NULL,
    @p_biz_type NVARCHAR(200) = NULL,
    @p_trans_open_date VARCHAR(8) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(19, 2) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_emp_id BIGINT = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBACUST (
                cust_nm, biz_no, tel, cur_cd, owner_nm, zip_code, addr1, addr2,
                homepage, email, fax, biz_kind, biz_type, trans_open_date, vat_type, vat_rate,
                remark, stat_cd, EMP_ID, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_cust_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_zip_code, @p_addr1, @p_addr2,
                @p_homepage, @p_email, @p_fax, @p_biz_kind, @p_biz_type, @p_trans_open_date, @p_vat_type, @p_vat_rate,
                @p_remark, @p_stat_cd, @p_emp_id, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_cust_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUST SET
                cust_nm = @p_cust_nm, biz_no = @p_biz_no, tel = @p_tel, cur_cd = @p_cur_cd,
                owner_nm = @p_owner_nm, zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                homepage = @p_homepage, email = @p_email, fax = @p_fax, biz_kind = @p_biz_kind,
                biz_type = @p_biz_type, trans_open_date = @p_trans_open_date, vat_type = @p_vat_type,
                vat_rate = @p_vat_rate, remark = @p_remark, stat_cd = @p_stat_cd, EMP_ID = @p_emp_id,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE CUST_ID = @p_cust_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUST WHERE CUST_ID = @p_cust_id;
        END

        SET @GeneratedCode = CAST(@p_cust_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_BA_CUST_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_serl INT = NULL,
    @p_prsn_nm NVARCHAR(100) = NULL,
    @p_grade NVARCHAR(100) = NULL,
    @p_tel1 VARCHAR(30) = NULL,
    @p_tel2 VARCHAR(30) = NULL,
    @p_fax VARCHAR(30) = NULL,
    @p_email VARCHAR(30) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TBACUSTPRSN WHERE CUST_ID = @p_cust_id;

            INSERT INTO TBACUSTPRSN (CUST_ID, serl, prsn_nm, grade, tel1, tel2, fax, email, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_cust_id, @p_serl, @p_prsn_nm, @p_grade, @p_tel1, @p_tel2, @p_fax, @p_email, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUSTPRSN SET
                prsn_nm = @p_prsn_nm, grade = @p_grade, tel1 = @p_tel1, tel2 = @p_tel2,
                fax = @p_fax, email = @p_email, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUSTPRSN WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_BA_CUST_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_serl INT = NULL,
    @p_bank_cd VARCHAR(20) = NULL,
    @p_acnt_no VARCHAR(50) = NULL,
    @p_remark NVARCHAR(400) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TBACUSTACNT WHERE CUST_ID = @p_cust_id;

            INSERT INTO TBACUSTACNT (CUST_ID, serl, bank_cd, acnt_no, remark, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_cust_id, @p_serl, @p_bank_cd, @p_acnt_no, @p_remark, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBACUSTACNT SET
                bank_cd = @p_bank_cd, acnt_no = @p_acnt_no, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBACUSTACNT WHERE CUST_ID = @p_cust_id AND serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_DEPT_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_dept_id BIGINT = NULL,		/* Q1일 때만 사용 - 정확히 일치하는 부서 */
    @p_dept_nm VARCHAR(50) = NULL,    
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                        a.acc_id, 
                        a.dept_id, 
                        a.dept_nm, 
                        a.par_dept_id, 
                        b.dept_nm as par_dept_nm, 
                        a.dept_type, 
                        a.remark
            FROM TBADEPT as a
                        LEFT OUTER JOIN TBADEPT as b on a.par_dept_id = b.dept_id
            WHERE (@p_dept_nm IS NULL OR a.dept_nm LIKE '%' + @p_dept_nm + '%')
            ORDER BY a.DEPT_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                emp_no, emp_nm, emp_nm_eng, job_grade, job_type, tel, hp_tel, email
            FROM TBAEMP
            WHERE DEPT_ID = @p_dept_id
            ORDER BY emp_no;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_DEPT_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 사업장 생기면 서버가 채우도록 전환 */
    @p_dept_id BIGINT = NULL,		/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_dept_nm VARCHAR(50) = NULL,
    @p_par_dept_id BIGINT = NULL,
    @p_dept_type VARCHAR(10) = NULL,
    @p_remark NVARCHAR(2000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBADEPT (
                acc_id, dept_nm, PAR_DEPT_ID, dept_type, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_dept_nm, @p_par_dept_id, @p_dept_type, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBADEPT SET
                dept_nm = @p_dept_nm, PAR_DEPT_ID = @p_par_dept_id, dept_type = @p_dept_type,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE DEPT_ID = @p_dept_id;
            SET @GeneratedCode = CAST(@p_dept_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBADEPT WHERE DEPT_ID = @p_dept_id;
            SET @GeneratedCode = CAST(@p_dept_id AS VARCHAR(20));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- USP_BA_EMP_Q 부서 검색 조건 수정 (2026-09-09)
--
-- DEPT_ID가 080 마이그레이션에서 VARCHAR 코드 -> BIGINT ID로 바뀌었는데, 이 프로시저의 WHERE절은
-- 옛날 방식 그대로 "a.DEPT_ID LIKE @p_dept_id + '%'"(부분일치 코드검색)를 쓰고 있었다. @p_dept_id가
-- BIGINT라서 '%' 문자열을 bigint로 변환하려다 항상 변환 오류가 나고, 그 오류가 TRY/CATCH에 먹혀
-- 결과 없이 빈 그리드만 나온다(조건을 비워도 항상 실패 - 실제로 겪음, 2026-09-09).
--
-- 다른 화면(081_TBADEPT_Procs.sql 등)처럼 정확일치(=)로 통일한다 - DEPT_ID는 이제 사람이 입력하는
-- 코드가 아니라 부서선택 팝업이 돌려주는 ID값이므로 부분일치가 애초에 의미가 없다.

CREATE   PROCEDURE [dbo].[USP_BA_EMP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no   VARCHAR(20) = NULL,
    @p_dept_id  BIGINT = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                a.EMP_ID,
                a.acc_id,
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                a.dept_id,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date,
                a.job_grade,
                a.job_type,
                a.ret_yn,
                a.ret_date,
                a.sex_cd,
                a.tel,
                a.hp_tel,
                a.email,
                a.nat_cd,
                a.zip_code,
                a.addr1,
                a.addr2,
                a.holi_yn,
                a.dilig_yn,
                a.pay_yn,
                a.photo
            FROM        TBAEMP as  a
                            JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
            WHERE       1 = 1
            AND         (@p_dept_id IS NULL OR a.DEPT_ID = @p_dept_id)
            ORDER BY emp_no;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[USP_BA_EMP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_id BIGINT = NULL,	/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 연결된 계정을 여기서 채우도록 전환 */
    @p_emp_no VARCHAR(20),
    @p_emp_nm VARCHAR(100) = NULL,
    @p_emp_nm_eng VARCHAR(100) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_ent_date VARCHAR(8) = NULL,
    @p_grp_ent_date VARCHAR(8) = NULL,
    @p_job_grade VARCHAR(10) = NULL,
    @p_job_type VARCHAR(10) = NULL,
    @p_ret_yn VARCHAR(2) = 'N',
    @p_ret_date VARCHAR(8) = NULL,
    @p_sex_cd VARCHAR(1) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_hp_tel VARCHAR(30) = NULL,
    @p_email NVARCHAR(50) = NULL,
    @p_nat_cd VARCHAR(10) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(300) = NULL,
    @p_addr2 NVARCHAR(300) = NULL,
    @p_holi_yn VARCHAR(1) = 'N',
    @p_dilig_yn VARCHAR(1) = NULL,
    @p_pay_yn VARCHAR(1) = NULL,
    @p_photo NVARCHAR(MAX) = NULL,	/* Base64 인코딩된 이미지 - NULL/빈 문자열이면 사진을 지우지 않고 그대로 둠 */
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        -- 다른 필드들과 같은 원칙(화면이 현재 상태를 매번 통째로 다시 보낸다) - 클라이언트가
        -- 사진을 안 건드렸으면 로드했던 값을 그대로 다시 보내고, "사진 지우기"를 누르면 빈
        -- 문자열/NULL을 보내 실제로 지운다. U에서도 항상 photo 컬럼을 덮어쓴다.
        DECLARE @photo_bin VARBINARY(MAX) = NULL;
        IF @p_photo IS NOT NULL AND LEN(@p_photo) > 0
            SET @photo_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_photo"))', 'VARBINARY(MAX)');

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAEMP (
                acc_id, emp_no, emp_nm, emp_nm_eng, DEPT_ID, ent_date, grp_ent_date,
                job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email,
                nat_cd, zip_code, addr1, addr2, holi_yn, dilig_yn, pay_yn, photo,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_emp_no, @p_emp_nm, @p_emp_nm_eng, @p_dept_id, @p_ent_date, @p_grp_ent_date,
                @p_job_grade, @p_job_type, @p_ret_yn, @p_ret_date, @p_sex_cd, @p_tel, @p_hp_tel, @p_email,
                @p_nat_cd, @p_zip_code, @p_addr1, @p_addr2, @p_holi_yn, @p_dilig_yn, @p_pay_yn, @photo_bin,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAEMP SET
                emp_no = @p_emp_no, emp_nm = @p_emp_nm, emp_nm_eng = @p_emp_nm_eng, DEPT_ID = @p_dept_id,
                ent_date = @p_ent_date, grp_ent_date = @p_grp_ent_date,
                job_grade = @p_job_grade, job_type = @p_job_type, ret_yn = @p_ret_yn, ret_date = @p_ret_date,
                sex_cd = @p_sex_cd, tel = @p_tel, hp_tel = @p_hp_tel, email = @p_email, nat_cd = @p_nat_cd,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                holi_yn = @p_holi_yn, dilig_yn = @p_dilig_yn, pay_yn = @p_pay_yn, photo = @photo_bin,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE EMP_ID = @p_emp_id;
            SET @GeneratedCode = CAST(@p_emp_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAEMP WHERE EMP_ID = @p_emp_id;
            SET @GeneratedCode = CAST(@p_emp_id AS VARCHAR(20));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_EMPLIST_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no   VARCHAR(20) = NULL,
    @p_dept_nm   NVARCHAR(50) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                a.acc_id,
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                a.DEPT_ID,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date,
                a.job_grade,
                a.job_type,
                a.ret_yn,
                a.ret_date,
                a.sex_cd,
                a.tel,
                a.hp_tel,
                a.email,
                a.nat_cd,
                a.zip_code,
                a.addr1,
                a.addr2,
                a.holi_yn,
                a.dilig_yn,
                a.pay_yn,
                a.photo
            FROM        TBAEMP as  a
                            JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
            WHERE       1 = 1
            AND         (@p_dept_nm IS NULL OR @p_dept_nm = '' OR b.dept_nm LIKE '%' + @p_dept_nm + '%')
            AND         ((@p_emp_no IS NULL OR emp_no LIKE '%' + @p_emp_no + '%')
                            OR
                          (@p_emp_no IS NULL OR emp_nm LIKE '%' + @p_emp_no + '%'))
            ORDER BY emp_no;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_ITEM_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    @p_item_no VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                a.acc_id,
                a.item_id,
                a.item_no,
                a.item_nm,
                a.item_spec,
                a.unit_cd,
                a.po_unit_cd,
                a.wh_id,
                a.loc_id,
                a.safe_qty,
                a.dept_id,
                b.dept_nm,
                a.emp_id,
                c.emp_no,
                c.emp_nm,
                a.prod_yn,
                a.cust_id,
                d.cust_nm,
                a.asset_type,
                a.out_type,
                a.po_qc_yn,
                a.prod_qc_yn,
                a.lot_yn,
                a.stock_yn,
                a.po_yn,
                a.sale_yn,
                a.stat_cd,
                a.grp1_id,
                a.grp2_id,
                a.grp3_id,
                a.grp4_id,
                a.po_acnt_cd,
                a.sale_acnt_cd,
                a.remark
            FROM        TBAITEM as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
                        LEFT OUTER JOIN TBACUST as d on a.cust_id = d.cust_id
            WHERE       1 = 1
   --         AND         (@p_item_id IS NULL OR a.item_id = @p_item_id)
            AND         (@p_item_no IS NULL OR a.item_no LIKE '%' + @p_item_no + '%')
            AND         (@p_item_nm IS NULL OR a.item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY a.item_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_ITEM_Q_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                acc_id, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, remark
            FROM TBAITEMUNIT
            WHERE item_id = @p_item_id
            ORDER BY item_id, fr_unit_cd, to_unit_cd;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_ITEM_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_item_no NVARCHAR(200) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    @p_item_spec NVARCHAR(100) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_po_unit_cd VARCHAR(10) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_safe_qty NUMERIC(18, 5) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_prod_yn VARCHAR(1) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_asset_type VARCHAR(10) = NULL,
    @p_out_type VARCHAR(10) = NULL,
    @p_po_qc_yn VARCHAR(1) = NULL,
    @p_prod_qc_yn VARCHAR(1) = NULL,
    @p_lot_yn VARCHAR(1) = NULL,
    @p_stock_yn VARCHAR(1) = NULL,
    @p_po_yn VARCHAR(1) = NULL,
    @p_sale_yn VARCHAR(1) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_grp1_id BIGINT = NULL,
    @p_grp2_id BIGINT = NULL,
    @p_grp3_id BIGINT = NULL,
    @p_grp4_id BIGINT = NULL,
    @p_po_acnt_cd VARCHAR(20) = NULL,
    @p_sale_acnt_cd VARCHAR(20) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAITEM (
                        acc_id,             item_no,            item_nm,                item_spec, 
                        unit_cd,            po_unit_cd,         wh_id, loc_id,
                safe_qty, DEPT_ID, EMP_ID, prod_yn, CUST_ID, asset_type, out_type,
                po_qc_yn, prod_qc_yn, lot_yn, stock_yn, po_yn,  sale_yn, 
                stat_cd, grp1_id, grp2_id, grp3_id, grp4_id,
                po_acnt_cd, sale_acnt_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_item_no, @p_item_nm, @p_item_spec, @p_unit_cd, @p_po_unit_cd, @p_wh_id, @p_loc_id,
                @p_safe_qty, @p_dept_id, @p_emp_id, @p_prod_yn, @p_cust_id, @p_asset_type, @p_out_type,
                @p_po_qc_yn, @p_prod_qc_yn, @p_lot_yn, @p_stock_yn, @p_po_yn, @p_sale_yn, 
                @p_stat_cd, @p_grp1_id, @p_grp2_id, @p_grp3_id, @p_grp4_id,
                @p_po_acnt_cd, @p_sale_acnt_cd, @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_item_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEM SET
                item_no = @p_item_no, item_nm = @p_item_nm, item_spec = @p_item_spec,
                unit_cd = @p_unit_cd, po_unit_cd = @p_po_unit_cd, wh_id = @p_wh_id, loc_id = @p_loc_id,
                safe_qty = @p_safe_qty, DEPT_ID = @p_dept_id,
                EMP_ID = COALESCE(@p_emp_id, EMP_ID),
                prod_yn = COALESCE(@p_prod_yn, prod_yn),
                CUST_ID = COALESCE(@p_cust_id, CUST_ID),
                asset_type = @p_asset_type, out_type = @p_out_type,
                po_qc_yn = @p_po_qc_yn, 
                prod_qc_yn = @p_prod_qc_yn, 
                lot_yn = @p_lot_yn, 
                stock_yn = @p_stock_yn,
                po_yn = @p_po_yn, 
                sale_yn = @p_sale_yn, 
                stat_cd = @p_stat_cd, 
                grp1_id = @p_grp1_id, 
                grp2_id = @p_grp2_id,
                grp3_id = @p_grp3_id, 
                grp4_id = @p_grp4_id,
                po_acnt_cd = @p_po_acnt_cd, 
                sale_acnt_cd = @p_sale_acnt_cd, 
                remark = @p_remark,
                upt_user_id = @p_user_id, 
                upt_dt = GETDATE(), 
                upt_pc = @p_client_pc
            WHERE item_id = @p_item_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEM WHERE item_id = @p_item_id;
        END

        IF @p_work_type IN ('N', 'U') AND @p_unit_cd IS NOT NULL AND @p_po_unit_cd IS NOT NULL
            AND @p_unit_cd <> @p_po_unit_cd
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM TBAITEMUNIT
                WHERE item_id = @p_item_id AND fr_unit_cd = @p_unit_cd AND to_unit_cd = @p_po_unit_cd
            )
            BEGIN
                INSERT INTO TBAITEMUNIT (acc_id, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, reg_user_id, reg_dt, reg_pc)
                VALUES (@p_acc_id, @p_item_id, @p_unit_cd, 1, @p_po_unit_cd, 1, @p_user_id, GETDATE(), @p_client_pc);
            END
        END

        SET @GeneratedCode = CAST(@p_item_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_ITEM_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT,
    @p_fr_unit_cd VARCHAR(10),
    @p_fr_qty NUMERIC(19, 5) = NULL,
    @p_to_unit_cd VARCHAR(10),
    @p_to_qty NUMERIC(19, 5) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAITEMUNIT (
                acc_id, item_id, fr_unit_cd, fr_qty, to_unit_cd, to_qty, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_item_id, @p_fr_unit_cd, @p_fr_qty, @p_to_unit_cd, @p_to_qty, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMUNIT SET
                fr_qty = @p_fr_qty, to_qty = @p_to_qty, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE item_id = @p_item_id AND fr_unit_cd = @p_fr_unit_cd AND to_unit_cd = @p_to_unit_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEMUNIT
            WHERE item_id = @p_item_id AND fr_unit_cd = @p_fr_unit_cd AND to_unit_cd = @p_to_unit_cd;
        END

        SET @GeneratedCode = CAST(@p_item_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- USP_BA_ITEMGRP_Q/_S 전면 재작성 - 이 두 프로시저는 이번 rename과 무관하게 이미 026 마이그레이션
-- 시절의 옛 컬럼명(item_class_cd/item_class_nm/par_item_class_cd)을 그대로 참조하고 있어서,
-- TBAITEMGRP가 grp_id/grp_nm/par_grp_id로 재설계된 뒤로 계속 깨져 있던 상태였다(CREATE OR ALTER
-- 자체가 "잘못된 열 이름"으로 실패 - 실제로 이번 마이그레이션 적용 중 확인함). 아직 frmItemGrp
-- 화면이 없어 호출하는 곳이 없으므로(BA 모듈 어디에도 참조 없음, 확인함) 동작을 바꿔도 위험이
-- 없다 - 구조가 거의 동일한 USP_BA_DEPT_Q/_S(자기참조 계층형 테이블, par_dept_id/dept_nm 패턴)를
-- 그대로 본떠 지금의 실제 TBAITEMGRP 스키마(grp_id/grp_nm/grp_lvl/par_grp_id/remark/acc_id)에
-- 맞춰 새로 짰다.
CREATE   PROCEDURE [dbo].[USP_BA_ITEMGRP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_grp_id BIGINT = NULL,
    @p_grp_nm NVARCHAR(30) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                        a.acc_id,
                        a.grp_id,
                        a.grp_nm,
                        a.grp_lvl,
                        a.par_grp_id,
                        b.grp_nm as par_grp_nm,
                        a.remark
            FROM TBAITEMGRP as a
                        LEFT OUTER JOIN TBAITEMGRP as b on a.par_grp_id = b.grp_id
            WHERE 1 = 1
            --AND (@p_grp_id IS NULL OR a.grp_id = @p_grp_id)
              AND (@p_grp_nm IS NULL OR a.grp_nm LIKE '%' + @p_grp_nm + '%')
            ORDER BY a.grp_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_BA_ITEMGRP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 사업장 생기면 서버가 채우도록 전환 */
    @p_grp_id BIGINT = NULL,		/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_grp_nm NVARCHAR(30) = NULL,
    @p_grp_lvl VARCHAR(10) = NULL,
    @p_par_grp_id BIGINT = NULL,
    @p_remark NVARCHAR(1000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAITEMGRP (
                acc_id, grp_nm, grp_lvl, par_grp_id, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_grp_nm, @p_grp_lvl, @p_par_grp_id, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMGRP SET
                acc_id = @p_acc_id, 
                grp_nm = @p_grp_nm, grp_lvl = @p_grp_lvl, par_grp_id = @p_par_grp_id,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE grp_id = @p_grp_id;
            SET @GeneratedCode = CAST(@p_grp_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEMGRP WHERE grp_id = @p_grp_id;
            SET @GeneratedCode = CAST(@p_grp_id AS VARCHAR(20));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ---------- USP_HR_NAMECARD_Q: 명함신청서 목록조회 ---------- */
CREATE   PROCEDURE USP_HR_NAMECARD_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_req_no VARCHAR(20) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            -- 결재상태(appr_stat_cd)는 TAPDOC을 LEFT JOIN해서 그때그때 계산(Pull) - THRNAMECARDREQ
            -- 자체에는 중복 저장하지 않는다. stat_cd(진행상태)만 최종승인 시 USP_AP_APPR_S가 직접 갱신.
            SELECT
                n.req_id, n.req_no, n.dept_id, d.dept_nm, n.emp_id, e.emp_no, e.emp_nm,
                n.job_grade, n.name_kor, n.name_eng, n.dept_kor, n.dept_eng, n.mobile, n.email,
                n.stat_cd, n.app_id, n.app_no, n.remark, n.reg_dt,
                a.stat_cd AS appr_stat_cd
            FROM THRNAMECARDREQ n
            LEFT JOIN TBAEMP e ON e.EMP_ID = n.emp_id
            LEFT JOIN TBADEPT d ON d.DEPT_ID = n.dept_id
            LEFT JOIN TAPDOC a ON a.app_id = n.app_id
            WHERE (@p_emp_no IS NULL OR e.emp_no = @p_emp_no)
              AND (@p_req_id IS NULL OR n.req_id = @p_req_id)
              AND (@p_req_no IS NULL OR n.req_no LIKE '%' + @p_req_no + '%')
            ORDER BY n.req_id DESC;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* ---------- USP_HR_NAMECARD_S: 명함신청서 등록/수정/삭제 ---------- */
CREATE   PROCEDURE USP_HR_NAMECARD_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_job_grade VARCHAR(100) = NULL,
    @p_name_kor VARCHAR(50) = NULL,
    @p_name_eng VARCHAR(50) = NULL,
    @p_dept_kor VARCHAR(50) = NULL,
    @p_dept_eng VARCHAR(50) = NULL,
    @p_mobile NVARCHAR(50) = NULL,
    @p_email NVARCHAR(50) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        -- dept_id/emp_id는 화면이 몰라도 되게(Session에 DeptId가 없어서 EmpNo/DeptNm만 노출 -
        -- 화면 하나 때문에 공용 세션을 확장하는 대신) @p_user_id(로그인 세션, 클라이언트가 못
        -- 속임 - GenericDataRepository.SaveAsync가 항상 서버에서 채움)로 TSMUSER->TBAEMP를
        -- 타고 서버가 직접 채운다.
        DECLARE @dept_id BIGINT, @emp_id BIGINT;
        SELECT @emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
        SELECT @dept_id = DEPT_ID FROM TBAEMP WHERE EMP_ID = @emp_id;

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO THRNAMECARDREQ (
                req_no, dept_id, emp_id, job_grade, name_kor, name_eng, dept_kor, dept_eng,
                mobile, email, stat_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', @dept_id, @emp_id, @p_job_grade, @p_name_kor, @p_name_eng, @p_dept_kor, @p_dept_eng,
                @p_mobile, @p_email, '0', @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_req_id = SCOPE_IDENTITY();
            UPDATE THRNAMECARDREQ
               SET req_no = 'NC' + RIGHT('00000000' + CAST(@p_req_id AS VARCHAR(8)), 8)
             WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE THRNAMECARDREQ SET
                job_grade = @p_job_grade, name_kor = @p_name_kor, name_eng = @p_name_eng,
                dept_kor = @p_dept_kor, dept_eng = @p_dept_eng, mobile = @p_mobile, email = @p_email,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM THRNAMECARDREQ WHERE req_id = @p_req_id;
        END

        SET @GeneratedCode = CAST(@p_req_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* ---------- TSMFILE/TSMFILEHIST 프로시저: 파일업로드 공통팝업(frmFileUpload) ----------
   USP_SM_FILE_Q(Q) - doc_type+doc_id(+doc_serl)로 첨부파일 목록 조회. TSMUSER를 조인해서
   등록자명(RegUserNm)까지 같이 내려준다(그리드에 RegUserNm 컬럼이 이미 있음).
   USP_SM_FILE_S - N(신규 등록)/U(재시도 후 갱신)/D(삭제). 실제 파일 바이트를 디스크(또는
   NAS UNC 경로)에 쓰고 지우는 일은 서버(C# FileStorageService)가 하고, 이 프로시저는
   TSMFILE 메타데이터 행만 다룬다.
   USP_SM_FILEHIST_S - N(다운로드 이력 한 줄 적재)만 지원 - 성공한 다운로드마다 호출된다. */

CREATE   PROCEDURE USP_SM_FILE_Q
    @p_work_type varchar(10),
    @p_doc_type varchar(10) = NULL,
    @p_doc_id bigint = NULL,
    @p_doc_serl int = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_work_type = 'Q'
    BEGIN
        SELECT
            f.file_id AS FileId,
            f.doc_type AS DocType,
            f.doc_id AS DocId,
            f.doc_no AS DocNo,
            f.doc_serl AS DocSerl,
            f.serl AS Serl,
            f.file_type AS FileType,
            f.file_nm AS FileNm,
            f.file_size AS FileSize,
            f.mime_type AS MimeType,
            f.file_path AS FilePath,
            f.storage_type AS StorageType,
            f.form_id AS FormId,
            f.remark AS Remark,
            f.reg_user_id AS RegUserId,
            u.USER_NM AS RegUserNm,
            f.fail_yn AS FailYn,
            f.retry_cnt AS RetryCnt
        FROM TSMFILE f
        LEFT JOIN TSMUSER u ON u.USER_ID = f.reg_user_id
        WHERE f.doc_type = @p_doc_type
          AND f.doc_id = @p_doc_id
          AND (@p_doc_serl IS NULL OR f.doc_serl = @p_doc_serl)
        ORDER BY f.serl;
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* ---------- TSMFILE/TSMFILEHIST: form_id 기록 + reg_pc/upt_pc를 실제 PC명으로 ----------
   1) reg_pc/upt_pc가 로컬 테스트에서 전부 "::1"(루프백)로만 찍혀서 무의미해 보인다는 지적
      (2026-09-06) - 지금까지 이 값은 서버가 HttpContext.Connection.RemoteIpAddress로 채워왔는데,
      클라이언트/서버가 같은 PC면 항상 루프백 주소만 보인다(다른 PC끼리는 실제 IP가 찍히니
      틀린 동작은 아니었지만, "PC"라는 컬럼명에는 IP보다 컴퓨터 이름이 더 맞는다). 이제
      WinForms 클라이언트가 자기 Environment.MachineName을 X-Client-Pc 헤더로 보내고,
      FilesController가 그 값을 우선 쓰도록 바꿨다(코드 쪽 변경, 이 마이그레이션은 그걸 받을
      파라미터만 추가).
   2) form_id는 지금까지 아무도 안 채워서 항상 NULL이었다 - 어느 화면에서 업로드했는지
      남기기 위해 USP_SM_FILE_S에 @p_form_id를 추가한다(frmFileUpload가 호출한 화면의 클래스명을
      자동으로 채움).
   3) TSMFILEHIST.down_pc도 마찬가지로 지금까지 비어있었다 - down_ip(실제 IP)는 그대로 두고
      down_pc(컴퓨터 이름)도 같이 남기도록 USP_SM_FILEHIST_S에 @p_down_pc를 추가한다. */

CREATE   PROCEDURE USP_SM_FILE_S
    @p_work_type varchar(10),
    @p_file_id bigint = NULL,
    @p_doc_type varchar(10) = NULL,
    @p_doc_id bigint = NULL,
    @p_doc_no varchar(100) = NULL,
    @p_doc_serl int = NULL,
    @p_file_type varchar(10) = NULL,
    @p_file_nm nvarchar(200) = NULL,
    @p_file_size bigint = NULL,
    @p_mime_type varchar(100) = NULL,
    @p_file_path nvarchar(200) = NULL,
    @p_storage_type varchar(10) = NULL,
    @p_form_id varchar(50) = NULL,
    @p_remark nvarchar(100) = NULL,
    @p_fail_yn varchar(1) = NULL,
    @p_user_id varchar(30),
    @p_client_pc nvarchar(200) = NULL,
    @GeneratedCode varchar(50) OUTPUT,
    @ReturnCode int OUTPUT,
    @ReturnMsg varchar(200) OUTPUT,
    @ErrorCode int OUTPUT,
    @ErrorMsg varchar(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = NULL;
    SET @ErrorCode = 0; SET @ErrorMsg = NULL;
    SET @GeneratedCode = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl int = ISNULL(
                (SELECT MAX(serl) FROM TSMFILE WHERE doc_type = @p_doc_type AND doc_id = @p_doc_id AND doc_serl = @p_doc_serl),
                0) + 1;

            INSERT INTO TSMFILE (
                doc_type, doc_id, doc_no, doc_serl, serl, file_type, file_nm, file_size, mime_type,
                file_path, storage_type, form_id, remark, fail_yn, retry_cnt, reg_user_id, reg_dt, reg_pc)
            VALUES (
                @p_doc_type, @p_doc_id, @p_doc_no, @p_doc_serl, @nextSerl, @p_file_type, @p_file_nm, @p_file_size, @p_mime_type,
                @p_file_path, @p_storage_type, @p_form_id, @p_remark, @p_fail_yn, 0, @p_user_id, GETDATE(), @p_client_pc);

            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS varchar(50));
        END
        ELSE IF @p_work_type = 'U' -- 스테이징 경로 저장(init 직후) / 완료 처리(complete) 공용 -
                                    -- retry_cnt는 여기서 안 건드린다('R' 참고)
        BEGIN
            UPDATE TSMFILE
            SET file_path = @p_file_path,
                file_size = @p_file_size,
                mime_type = @p_mime_type,
                storage_type = @p_storage_type,
                fail_yn = @p_fail_yn,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE file_id = @p_file_id;
        END
        ELSE IF @p_work_type = 'R' -- 재시도 버튼을 실제로 눌렀을 때만(FilesController /mark-retry)
        BEGIN
            UPDATE TSMFILE
            SET retry_cnt = ISNULL(retry_cnt, 0) + 1,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE file_id = @p_file_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMFILE WHERE file_id = @p_file_id;
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_FILEHIST_S
    @p_work_type varchar(10),
    @p_file_id bigint,
    @p_doc_type varchar(10),
    @p_doc_id bigint,
    @p_doc_no varchar(100),
    @p_file_nm nvarchar(200) = NULL,
    @p_down_user_id varchar(30),
    @p_down_pc nvarchar(200) = NULL,
    @p_down_ip varchar(50) = NULL,
    @ReturnCode int OUTPUT,
    @ReturnMsg varchar(200) OUTPUT,
    @ErrorCode int OUTPUT,
    @ErrorMsg varchar(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = NULL;
    SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMFILEHIST (file_id, doc_type, doc_id, doc_no, file_nm, down_user_id, down_dt, down_pc, down_ip)
            VALUES (@p_file_id, @p_doc_type, @p_doc_id, @p_doc_no, @p_file_nm, @p_down_user_id, GETDATE(), @p_down_pc, @p_down_ip);
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_id BIGINT = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT GRID_KEY AS GridKey, LAYOUT_XML AS LayoutXml
            FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_id BIGINT = NULL,
    @p_grid_key VARCHAR(50) = NULL,
    @p_layout_xml NVARCHAR(MAX) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            IF EXISTS (SELECT 1 FROM TSMUSERGRIDLAYOUT WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id AND GRID_KEY = @p_grid_key)
                UPDATE TSMUSERGRIDLAYOUT
                SET LAYOUT_XML = @p_layout_xml, UPT_DT = GETDATE()
                WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id AND GRID_KEY = @p_grid_key;
            ELSE
                INSERT INTO TSMUSERGRIDLAYOUT (USER_ID, MENU_ID, GRID_KEY, LAYOUT_XML, UPT_DT)
                VALUES (@p_user_id, @p_menu_id, @p_grid_key, @p_layout_xml, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_ID = @p_menu_id AND GRID_KEY = @p_grid_key;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- TSMMENU.PROC_PREFIX(004_Add_Menu_ProcPrefix.sql에서 컬럼만 추가됐던 것)를 메뉴관리 CRUD
-- 경로(USP_SM_MENU_Q/USP_SM_MENU_S, api/menus, frmMenu 화면) 전체에 실제로 연결한다.
--
-- 문제: PROC_PREFIX는 지금까지 이 마이그레이션 스크립트들이 손으로 쓴 INSERT문에서만 채워졌고,
-- 메뉴관리 화면(frmMenu)이나 AI Builder의 메뉴 즉시등록(RegisterMenuAsync, api/menus POST)은
-- 이 컬럼 자체를 몰랐다(USP_SM_MENU_S에 파라미터가 아예 없었음) - 그래서 그 경로로 만들어진
-- 메뉴는 전부 PROC_PREFIX가 NULL로 남고, api/data/*(범용 데이터 통로) 저장/조회 시 "이 메뉴는
-- 범용 데이터 통로를 사용하도록 설정되어 있지 않습니다"로 막힌다(DataController.ValidateAsync
-- ②단계 - PROC_PREFIX가 비어있으면 무조건 거부). AI Builder로 만든 화면은 전부 이 경로를 타므로
-- frmCust뿐 아니라 앞으로 생성되는 모든 화면이 동일하게 겪는 문제였다(2026-09-04 실제 발견).
--
-- 조치: USP_SM_MENU_Q/S에 PROC_PREFIX를 정식 컬럼/파라미터로 추가하고, 이미 잘못 등록된
-- frmCust 메뉴(MENU_ID=49)도 여기서 같이 바로잡는다. C# 쪽(DTO/Repository/Controller/
-- frmAIBuilder/frmMenu)은 별도 코드 커밋에서 함께 수정한다.

CREATE   PROCEDURE USP_SM_MENU_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT MENU_ID AS MenuId, MENU_NM AS MenuNm, UPPER_MENU_ID AS UpperMenuId,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, MODULE AS Module, SCREEN_CLASS_NM AS ScreenClassNm,
                   ICON_NM AS IconNm, PROC_PREFIX AS ProcPrefix, SORT_ORDER AS SortOrder, USE_YN AS UseYn,
                   AUTH01_NM AS Auth01Nm, AUTH02_NM AS Auth02Nm, AUTH03_NM AS Auth03Nm, AUTH04_NM AS Auth04Nm, AUTH05_NM AS Auth05Nm,
                   AUTH06_NM AS Auth06Nm, AUTH07_NM AS Auth07Nm, AUTH08_NM AS Auth08Nm, AUTH09_NM AS Auth09Nm, AUTH10_NM AS Auth10Nm
            FROM TSMMENU
            ORDER BY UPPER_MENU_ID, SORT_ORDER;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENU_Q_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT COUNT(1) FROM TSMMENU WHERE MENU_ID = @p_menu_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENU_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT MENU_ID AS MenuId, MENU_NM AS MenuNm, UPPER_MENU_ID AS UpperMenuId,
                   MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, MODULE AS Module, SCREEN_CLASS_NM AS ScreenClassNm,
                   ICON_NM AS IconNm, SORT_ORDER AS SortOrder
            FROM TSMMENU
            WHERE USE_YN = 'Y'
            ORDER BY SORT_ORDER;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENU_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT = NULL,          -- U일 때만 필수(수정 대상 지정) - N은 IDENTITY로 자동 채움
    @p_menu_nm NVARCHAR(100),
    @p_upper_menu_id BIGINT = NULL,
    @p_menu_level INT = 1,
    @p_menu_type VARCHAR(10) = 'FORM',
    @p_module VARCHAR(20) = NULL,
    @p_screen_class_nm VARCHAR(200) = NULL,
    @p_icon_nm VARCHAR(50) = NULL,
    @p_proc_prefix VARCHAR(100) = NULL,
    @p_sort_order INT = 0,
    @p_use_yn CHAR(1) = 'Y',
    @p_auth01_nm NVARCHAR(20) = NULL,
    @p_auth02_nm NVARCHAR(20) = NULL,
    @p_auth03_nm NVARCHAR(20) = NULL,
    @p_auth04_nm NVARCHAR(20) = NULL,
    @p_auth05_nm NVARCHAR(20) = NULL,
    @p_auth06_nm NVARCHAR(20) = NULL,
    @p_auth07_nm NVARCHAR(20) = NULL,
    @p_auth08_nm NVARCHAR(20) = NULL,
    @p_auth09_nm NVARCHAR(20) = NULL,
    @p_auth10_nm NVARCHAR(20) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMMENU (
                MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, ICON_NM, PROC_PREFIX, SORT_ORDER, USE_YN,
                AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_menu_nm, @p_upper_menu_id, @p_menu_level, @p_menu_type, @p_module, @p_screen_class_nm, @p_icon_nm, @p_proc_prefix, @p_sort_order, 'Y',
                @p_auth01_nm, @p_auth02_nm, @p_auth03_nm, @p_auth04_nm, @p_auth05_nm, @p_auth06_nm, @p_auth07_nm, @p_auth08_nm, @p_auth09_nm, @p_auth10_nm,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(50));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMENU SET
                MENU_NM = @p_menu_nm, UPPER_MENU_ID = @p_upper_menu_id, MENU_LEVEL = @p_menu_level,
                MENU_TYPE = @p_menu_type, MODULE = @p_module, SCREEN_CLASS_NM = @p_screen_class_nm, ICON_NM = @p_icon_nm,
                PROC_PREFIX = @p_proc_prefix, SORT_ORDER = @p_sort_order, USE_YN = @p_use_yn,
                AUTH01_NM = @p_auth01_nm, AUTH02_NM = @p_auth02_nm, AUTH03_NM = @p_auth03_nm, AUTH04_NM = @p_auth04_nm, AUTH05_NM = @p_auth05_nm,
                AUTH06_NM = @p_auth06_nm, AUTH07_NM = @p_auth07_nm, AUTH08_NM = @p_auth08_nm, AUTH09_NM = @p_auth09_nm, AUTH10_NM = @p_auth10_nm,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE MENU_ID = @p_menu_id;

            SET @GeneratedCode = CAST(@p_menu_id AS VARCHAR(50));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENU_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_use_yn CHAR(1),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'D'
        BEGIN
            UPDATE TSMMENU SET USE_YN = @p_use_yn WHERE MENU_ID = @p_menu_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENUAUTH_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    @p_group_codes NVARCHAR(MAX) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT MENU_ID AS MenuId, AUTH_TARGET_TYPE AS AuthTargetType, AUTH_TARGET_CD AS AuthTargetCd,
                   VIEW_YN AS ViewYn, INSERT_YN AS InsertYn, SAVE_YN AS UpdateYn,
                   DELETE_YN AS DeleteYn, PRINT_YN AS PrintYn, EXCEL_YN AS ExcelYn,
                   AUTH01 AS Auth01, AUTH02 AS Auth02, AUTH03 AS Auth03, AUTH04 AS Auth04, AUTH05 AS Auth05,
                   AUTH06 AS Auth06, AUTH07 AS Auth07, AUTH08 AS Auth08, AUTH09 AS Auth09, AUTH10 AS Auth10
            FROM TSMMENUAUTH
            WHERE (AUTH_TARGET_TYPE = 'USER' AND AUTH_TARGET_CD = @p_user_id)
               OR (AUTH_TARGET_TYPE = 'GRP' AND @p_group_codes IS NOT NULL
                   AND AUTH_TARGET_CD IN (SELECT value FROM STRING_SPLIT(@p_group_codes, ',')));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENUAUTH_Q_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_target_type VARCHAR(10),
    @p_target_cd VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT M.MENU_ID AS MenuId, M.MENU_NM AS MenuNm, M.UPPER_MENU_ID AS UpperMenuId,
                   M.MENU_TYPE AS MenuType, M.SORT_ORDER AS SortOrder,
                   ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                   ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                   ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                   ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                   ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                   ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                   ISNULL(A.AUTH10, 'N') AS Auth10,
                   M.AUTH01_NM AS Auth01Nm, M.AUTH02_NM AS Auth02Nm, M.AUTH03_NM AS Auth03Nm, M.AUTH04_NM AS Auth04Nm, M.AUTH05_NM AS Auth05Nm,
                   M.AUTH06_NM AS Auth06Nm, M.AUTH07_NM AS Auth07Nm, M.AUTH08_NM AS Auth08Nm, M.AUTH09_NM AS Auth09Nm, M.AUTH10_NM AS Auth10Nm
            FROM TSMMENU M
            LEFT JOIN TSMMENUAUTH A
                ON A.MENU_ID = M.MENU_ID AND A.AUTH_TARGET_TYPE = @p_target_type AND A.AUTH_TARGET_CD = @p_target_cd
            WHERE M.USE_YN = 'Y'
            ORDER BY M.SORT_ORDER;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENUAUTH_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_target_type VARCHAR(10),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            IF @p_target_type = 'USER'
            BEGIN
                SELECT U.USER_ID AS TargetCd, U.USER_NM AS TargetNm, D.DEPT_NM AS SubNm,
                       ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                       ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                       ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                       ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                       ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                       ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                       ISNULL(A.AUTH10, 'N') AS Auth10
                FROM TSMUSER U
                LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
                LEFT JOIN TSMMENUAUTH A ON A.MENU_ID = @p_menu_id AND A.AUTH_TARGET_TYPE = 'USER' AND A.AUTH_TARGET_CD = U.USER_ID
                WHERE U.USE_YN = 'Y'
                ORDER BY U.USER_ID;
            END
            ELSE IF @p_target_type = 'GRP'
            BEGIN
                SELECT G.USER_GRP_CD AS TargetCd, G.USER_GRP_NM AS TargetNm,
                       CAST((SELECT COUNT(1) FROM TSMUSERGRPMAP M WHERE M.USER_GRP_CD = G.USER_GRP_CD) AS NVARCHAR(10)) + N'명' AS SubNm,
                       ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                       ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                       ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                       ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                       ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                       ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                       ISNULL(A.AUTH10, 'N') AS Auth10
                FROM TSMUSERGRP G
                LEFT JOIN TSMMENUAUTH A ON A.MENU_ID = @p_menu_id AND A.AUTH_TARGET_TYPE = 'GRP' AND A.AUTH_TARGET_CD = G.USER_GRP_CD
                WHERE G.USE_YN = 'Y'
                ORDER BY G.USER_GRP_CD;
            END
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENUAUTH_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_target_type VARCHAR(10),
    @p_target_cd VARCHAR(20),
    @p_items_json NVARCHAR(MAX),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            DELETE FROM TSMMENUAUTH WHERE AUTH_TARGET_TYPE = @p_target_type AND AUTH_TARGET_CD = @p_target_cd;

            INSERT INTO TSMMENUAUTH (
                MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, SAVE_YN, DELETE_YN, PRINT_YN, EXCEL_YN,
                AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_DT, UPT_DT
            )
            SELECT J.MenuId, @p_target_type, @p_target_cd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.PrintYn, J.ExcelYn,
                   J.Auth01, J.Auth02, J.Auth03, J.Auth04, J.Auth05, J.Auth06, J.Auth07, J.Auth08, J.Auth09, J.Auth10,
                   GETDATE(), GETDATE()
            FROM OPENJSON(@p_items_json)
            WITH (
                MenuId   BIGINT      '$.MenuId',
                ViewYn   CHAR(1)     '$.ViewYn',
                InsertYn CHAR(1)     '$.InsertYn',
                UpdateYn CHAR(1)     '$.UpdateYn',
                DeleteYn CHAR(1)     '$.DeleteYn',
                PrintYn  CHAR(1)     '$.PrintYn',
                ExcelYn  CHAR(1)     '$.ExcelYn',
                Auth01   CHAR(1)     '$.Auth01',
                Auth02   CHAR(1)     '$.Auth02',
                Auth03   CHAR(1)     '$.Auth03',
                Auth04   CHAR(1)     '$.Auth04',
                Auth05   CHAR(1)     '$.Auth05',
                Auth06   CHAR(1)     '$.Auth06',
                Auth07   CHAR(1)     '$.Auth07',
                Auth08   CHAR(1)     '$.Auth08',
                Auth09   CHAR(1)     '$.Auth09',
                Auth10   CHAR(1)     '$.Auth10'
            ) J
            WHERE J.ViewYn = 'Y' OR J.InsertYn = 'Y' OR J.UpdateYn = 'Y' OR J.DeleteYn = 'Y' OR J.PrintYn = 'Y' OR J.ExcelYn = 'Y'
               OR J.Auth01 = 'Y' OR J.Auth02 = 'Y' OR J.Auth03 = 'Y' OR J.Auth04 = 'Y' OR J.Auth05 = 'Y'
               OR J.Auth06 = 'Y' OR J.Auth07 = 'Y' OR J.Auth08 = 'Y' OR J.Auth09 = 'Y' OR J.Auth10 = 'Y';
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_MENUAUTH_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_target_type VARCHAR(10),
    @p_items_json NVARCHAR(MAX),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            DELETE FROM TSMMENUAUTH WHERE MENU_ID = @p_menu_id AND AUTH_TARGET_TYPE = @p_target_type;

            INSERT INTO TSMMENUAUTH (
                MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, SAVE_YN, DELETE_YN, PRINT_YN, EXCEL_YN,
                AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_DT, UPT_DT
            )
            SELECT @p_menu_id, @p_target_type, J.TargetCd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.PrintYn, J.ExcelYn,
                   J.Auth01, J.Auth02, J.Auth03, J.Auth04, J.Auth05, J.Auth06, J.Auth07, J.Auth08, J.Auth09, J.Auth10,
                   GETDATE(), GETDATE()
            FROM OPENJSON(@p_items_json)
            WITH (
                TargetCd VARCHAR(20) '$.TargetCd',
                ViewYn   CHAR(1)     '$.ViewYn',
                InsertYn CHAR(1)     '$.InsertYn',
                UpdateYn CHAR(1)     '$.UpdateYn',
                DeleteYn CHAR(1)     '$.DeleteYn',
                PrintYn  CHAR(1)     '$.PrintYn',
                ExcelYn  CHAR(1)     '$.ExcelYn',
                Auth01   CHAR(1)     '$.Auth01',
                Auth02   CHAR(1)     '$.Auth02',
                Auth03   CHAR(1)     '$.Auth03',
                Auth04   CHAR(1)     '$.Auth04',
                Auth05   CHAR(1)     '$.Auth05',
                Auth06   CHAR(1)     '$.Auth06',
                Auth07   CHAR(1)     '$.Auth07',
                Auth08   CHAR(1)     '$.Auth08',
                Auth09   CHAR(1)     '$.Auth09',
                Auth10   CHAR(1)     '$.Auth10'
            ) J
            WHERE J.ViewYn = 'Y' OR J.InsertYn = 'Y' OR J.UpdateYn = 'Y' OR J.DeleteYn = 'Y' OR J.PrintYn = 'Y' OR J.ExcelYn = 'Y'
               OR J.Auth01 = 'Y' OR J.Auth02 = 'Y' OR J.Auth03 = 'Y' OR J.Auth04 = 'Y' OR J.Auth05 = 'Y'
               OR J.Auth06 = 'Y' OR J.Auth07 = 'Y' OR J.Auth08 = 'Y' OR J.Auth09 = 'Y' OR J.Auth10 = 'Y';
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* =========================================================
   기초코드등록(CODE) 모듈에서 확정한 새 프로시저 표준의 첫 완성판.
   - 모든 입력 파라미터는 @p_ + snake_case(테이블 컬럼명과 동일 철자)
   - 표준 출력 5종(@GeneratedCode/@ReturnCode/@ReturnMsg/@ErrorCode/@ErrorMsg)은
     접두사 없이 PascalCase 그대로 유지
   - 첫 파라미터는 항상 @p_work_type - 조회는 Q/Q1/Q2..., 저장은 신규='N'/수정='U'/삭제='D'
   - 본문은 항상 TRY/CATCH로 감싸서 SQL 예외가 나도 @ErrorCode/@ErrorMsg로 화면까지 전달되게 함
     (조회 프로시저도 예외- 전에는 저장류만 TRY/CATCH였는데 이제 전부 통일)

   USP_SM_CODE_Q/USP_SM_CODE_S 둘 다 여기서 재정의. USP_SM_CODE_S_1(소분류 그리드 저장)은
   아직 이 표준으로 안 바꿨음 - 028의 snake_case 버전 그대로 유지 중.
   ========================================================= */

CREATE   PROCEDURE [dbo].[USP_SM_MINORCODE_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_major_cd VARCHAR(20) = NULL,		/*WORK_TYPE = 'Q' 일때 'Q1'일때 같은 변수 사용 */
    @p_major_nm NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                major_cd, major_nm, sys_yn,
                rel_cd1, rel_title1, rel_cd_type1,
                rel_cd2, rel_title2, rel_cd_type2,
                rel_cd3, rel_title3, rel_cd_type3,
                rel_cd4, rel_title4, rel_cd_type4,
                rel_cd5, rel_title5, rel_cd_type5,
                rel_cd6, rel_title6, rel_cd_type6,
                rel_cd7, rel_title7, rel_cd_type7,
                rel_cd8, rel_title8, rel_cd_type8,
                rel_cd9, rel_title9, rel_cd_type9,
                rel_cd10, rel_title10, rel_cd_type10,
                remark
            FROM TSMMAJOR
            WHERE (@p_major_cd IS NULL OR major_cd LIKE '%' + @p_major_cd + '%')
              AND (@p_major_nm IS NULL OR major_nm LIKE '%' + @p_major_nm + '%')
            ORDER BY major_cd;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                major_cd, minor_cd, minor_nm, sort,
                sys_yn, use_yn,
                rel_cd1, rel_cd2, rel_cd3, rel_cd4,
                rel_cd5, rel_cd6, rel_cd7, rel_cd8,
                rel_cd9, rel_cd10, remark
            FROM TSMMINOR
            WHERE major_cd = @p_major_cd
            ORDER BY sort;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- USP_SM_MINORCODE_S의 'D'(삭제) 분기가 TSMMAJOR만 지우고 TSMMINOR는 그대로 둬서, 소분류가
-- 있는 대분류를 삭제하면 소분류가 고아 레코드로 남는 문제가 있었다. 대분류 삭제 시 그 소속
-- 소분류를 먼저 지우도록 고친다(사용자가 운영 DB에 직접 적용한 것을 로컬 개발 DB와 저장소
-- 기록에도 반영).
--
-- FK 없이 소분류를 대분류코드로만 연결하고 있어서(TSMMINOR.major_cd, 참조 제약 없음) DB가
-- 강제하지 않으므로, 애플리케이션(이 프로시저)이 순서를 보장해야 한다 - 소분류 먼저, 대분류
-- 나중.

CREATE   PROCEDURE [dbo].[USP_SM_MINORCODE_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_major_cd VARCHAR(20),
    @p_major_nm NVARCHAR(200) = NULL,
    @p_sys_yn VARCHAR(1) = 'N',
    @p_rel_cd1 VARCHAR(50) = NULL,
    @p_rel_title1 VARCHAR(50) = NULL,
    @p_rel_cd_type1 VARCHAR(10) = NULL,
    @p_rel_cd2 VARCHAR(50) = NULL,
    @p_rel_title2 VARCHAR(50) = NULL,
    @p_rel_cd_type2 VARCHAR(10) = NULL,
    @p_rel_cd3 VARCHAR(50) = NULL,
    @p_rel_title3 VARCHAR(50) = NULL,
    @p_rel_cd_type3 VARCHAR(10) = NULL,
    @p_rel_cd4 VARCHAR(50) = NULL,
    @p_rel_title4 VARCHAR(50) = NULL,
    @p_rel_cd_type4 VARCHAR(10) = NULL,
    @p_rel_cd5 VARCHAR(50) = NULL,
    @p_rel_title5 VARCHAR(50) = NULL,
    @p_rel_cd_type5 VARCHAR(10) = NULL,
    @p_rel_cd6 VARCHAR(50) = NULL,
    @p_rel_title6 VARCHAR(50) = NULL,
    @p_rel_cd_type6 VARCHAR(10) = NULL,
    @p_rel_cd7 VARCHAR(50) = NULL,
    @p_rel_title7 VARCHAR(50) = NULL,
    @p_rel_cd_type7 VARCHAR(10) = NULL,
    @p_rel_cd8 VARCHAR(50) = NULL,
    @p_rel_title8 VARCHAR(50) = NULL,
    @p_rel_cd_type8 VARCHAR(10) = NULL,
    @p_rel_cd9 VARCHAR(50) = NULL,
    @p_rel_title9 VARCHAR(50) = NULL,
    @p_rel_cd_type9 VARCHAR(10) = NULL,
    @p_rel_cd10 VARCHAR(50) = NULL,
    @p_rel_title10 VARCHAR(50) = NULL,
    @p_rel_cd_type10 VARCHAR(10) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMMAJOR (
                major_cd, major_nm, sys_yn,
                rel_cd1, rel_title1, rel_cd_type1, rel_cd2, rel_title2, rel_cd_type2,
                rel_cd3, rel_title3, rel_cd_type3, rel_cd4, rel_title4, rel_cd_type4,
                rel_cd5, rel_title5, rel_cd_type5, rel_cd6, rel_title6, rel_cd_type6,
                rel_cd7, rel_title7, rel_cd_type7, rel_cd8, rel_title8, rel_cd_type8,
                rel_cd9, rel_title9, rel_cd_type9, rel_cd10, rel_title10, rel_cd_type10,
                remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_major_cd, @p_major_nm, @p_sys_yn,
                @p_rel_cd1, @p_rel_title1, @p_rel_cd_type1, @p_rel_cd2, @p_rel_title2, @p_rel_cd_type2,
                @p_rel_cd3, @p_rel_title3, @p_rel_cd_type3, @p_rel_cd4, @p_rel_title4, @p_rel_cd_type4,
                @p_rel_cd5, @p_rel_title5, @p_rel_cd_type5, @p_rel_cd6, @p_rel_title6, @p_rel_cd_type6,
                @p_rel_cd7, @p_rel_title7, @p_rel_cd_type7, @p_rel_cd8, @p_rel_title8, @p_rel_cd_type8,
                @p_rel_cd9, @p_rel_title9, @p_rel_cd_type9, @p_rel_cd10, @p_rel_title10, @p_rel_cd_type10,
                @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMAJOR SET
                major_nm = @p_major_nm, sys_yn = @p_sys_yn,
                rel_cd1 = @p_rel_cd1, rel_title1 = @p_rel_title1, rel_cd_type1 = @p_rel_cd_type1,
                rel_cd2 = @p_rel_cd2, rel_title2 = @p_rel_title2, rel_cd_type2 = @p_rel_cd_type2,
                rel_cd3 = @p_rel_cd3, rel_title3 = @p_rel_title3, rel_cd_type3 = @p_rel_cd_type3,
                rel_cd4 = @p_rel_cd4, rel_title4 = @p_rel_title4, rel_cd_type4 = @p_rel_cd_type4,
                rel_cd5 = @p_rel_cd5, rel_title5 = @p_rel_title5, rel_cd_type5 = @p_rel_cd_type5,
                rel_cd6 = @p_rel_cd6, rel_title6 = @p_rel_title6, rel_cd_type6 = @p_rel_cd_type6,
                rel_cd7 = @p_rel_cd7, rel_title7 = @p_rel_title7, rel_cd_type7 = @p_rel_cd_type7,
                rel_cd8 = @p_rel_cd8, rel_title8 = @p_rel_title8, rel_cd_type8 = @p_rel_cd_type8,
                rel_cd9 = @p_rel_cd9, rel_title9 = @p_rel_title9, rel_cd_type9 = @p_rel_cd_type9,
                rel_cd10 = @p_rel_cd10, rel_title10 = @p_rel_title10, rel_cd_type10 = @p_rel_cd_type10,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE major_cd = @p_major_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            --MinorCode먼저 삭제
            DELETE FROM TSMMINOR WHERE major_cd = @p_major_cd;
            --MajorCode 삭제
            DELETE FROM TSMMAJOR WHERE major_cd = @p_major_cd;
        END

        SET @GeneratedCode = @p_major_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- USP_SM_MINORCODE_S_1을 007(그리드 JSON 일괄 처리)에서 USP_SM_MINORCODE_S와 완전히 같은
-- 형태(단일 레코드, @p_work_type 분기, 파라미터 하나하나 나열)로 다시 바꾼다. 유사한 다른
-- 사이트의 P_FSMMINOR_S1 프로시저를 참고했다 - major_cd+minor_cd가 키인 것도 동일하다.
--
-- [007과 달라지는 점] 007은 그리드 전체를 JSON으로 한 번에 받아 프로시저 안에서 행 단위로
-- 나눠 처리했다. 이번엔 그 반대로, 프로시저는 레코드 하나만 다루고 화면(frmMinorCode.cs)이
-- 그리드에서 바뀐 행 수만큼 이 프로시저를 순서대로 호출한다 - USP_SM_MINORCODE_S(대분류)가
-- 항상 레코드 하나만 다루는 것과 완전히 같은 모양을 소분류에도 그대로 적용한 것.
--
-- [minor_cd(키)는 수정 대상이 아니다] major_cd가 한 번 등록되면 안 바뀌는 키로 다뤄지는 것과
-- 같은 이유로, 이 프로시저의 'U' 분기도 minor_cd를 WHERE에서만 쓰고 SET하지 않는다. 그리드에서
-- 코드 자체를 고친 경우는 화면이 "원래 코드 삭제 + 새 코드 등록"(D 호출 + N 호출)으로 표현한다.
--
-- [트랜잭션 범위가 좁아지는 점을 알아둘 것] 007은 그리드 전체가 한 트랜잭션이라 중간에 하나가
-- 실패해도 전부 롤백됐다. 이 방식은 호출마다 독립 트랜잭션이라, 그리드에서 여러 행이 바뀌었을
-- 때 일부는 반영되고 일부만 실패할 수 있다. 화면은 실패한 지점에서 멈추고 어떤 행인지 알려준다.

CREATE   PROCEDURE [dbo].[USP_SM_MINORCODE_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_major_cd VARCHAR(20),
    @p_minor_cd VARCHAR(100),
    @p_minor_nm NVARCHAR(200) = NULL,
    @p_sort INT = 0,
    @p_sys_yn VARCHAR(1) = 'N',
    @p_use_yn VARCHAR(1) = 'Y',
    @p_rel_cd1 VARCHAR(50) = NULL,
    @p_rel_cd2 VARCHAR(50) = NULL,
    @p_rel_cd3 VARCHAR(50) = NULL,
    @p_rel_cd4 VARCHAR(50) = NULL,
    @p_rel_cd5 VARCHAR(50) = NULL,
    @p_rel_cd6 VARCHAR(50) = NULL,
    @p_rel_cd7 VARCHAR(50) = NULL,
    @p_rel_cd8 VARCHAR(50) = NULL,
    @p_rel_cd9 VARCHAR(50) = NULL,
    @p_rel_cd10 VARCHAR(50) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMMINOR (
                major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn,
                rel_cd1, rel_cd2, rel_cd3, rel_cd4, rel_cd5,
                rel_cd6, rel_cd7, rel_cd8, rel_cd9, rel_cd10,
                remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_major_cd, @p_minor_cd, @p_minor_nm, @p_sort, @p_sys_yn, @p_use_yn,
                @p_rel_cd1, @p_rel_cd2, @p_rel_cd3, @p_rel_cd4, @p_rel_cd5,
                @p_rel_cd6, @p_rel_cd7, @p_rel_cd8, @p_rel_cd9, @p_rel_cd10,
                @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMINOR SET
                minor_nm = @p_minor_nm, sort = @p_sort, sys_yn = @p_sys_yn, use_yn = @p_use_yn,
                rel_cd1 = @p_rel_cd1, rel_cd2 = @p_rel_cd2, rel_cd3 = @p_rel_cd3,
                rel_cd4 = @p_rel_cd4, rel_cd5 = @p_rel_cd5, rel_cd6 = @p_rel_cd6,
                rel_cd7 = @p_rel_cd7, rel_cd8 = @p_rel_cd8, rel_cd9 = @p_rel_cd9, rel_cd10 = @p_rel_cd10,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE major_cd = @p_major_cd AND minor_cd = @p_minor_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMMINOR WHERE major_cd = @p_major_cd AND minor_cd = @p_minor_cd;
        END

        SET @GeneratedCode = @p_minor_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- N: 인증코드 발급 - 이 사용자의 기존 미사용 코드는 전부 무효화(USED_YN='Y')하고 새로 하나
-- 넣는다(재요청 시 이전 코드로는 더 이상 통과 못 하게). 코드 자체는 C#에서 난수로 만들어
-- 파라미터로 받는다(SQL 쪽 난수 생성 대신 - 훨씬 간단하고 테스트하기 쉬움).
--
-- C: 인증코드 확인 + 비밀번호 교체를 한 트랜잭션으로 - 확인 따로, 교체 따로 하면 그 사이에
-- 같은 코드로 두 번 시도하는 경쟁 상태가 생길 수 있다. 유효한 코드가 없으면(없음/만료/이미
-- 사용됨을 구분하지 않고) 전부 같은 실패 메시지 - 공격자에게 어떤 조건이 틀렸는지 알려주지
-- 않기 위함(계정 존재 여부를 캐는 수단이 되는 것도 같은 이유로 막음).
CREATE   PROCEDURE [dbo].[USP_SM_PWDRESET_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_code VARCHAR(10) = NULL,
    @p_expire_dt DATETIME = NULL,
    @p_new_password_hash VARCHAR(200) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            UPDATE TSMPWDRESETTOKEN SET USED_YN = 'Y' WHERE USER_ID = @p_user_id AND USED_YN = 'N';

            INSERT INTO TSMPWDRESETTOKEN (USER_ID, CODE, EXPIRE_DT)
            VALUES (@p_user_id, @p_code, @p_expire_dt);
        END
        ELSE IF @p_work_type = 'C'
        BEGIN
            BEGIN TRANSACTION;

            DECLARE @tokenId INT;
            SELECT TOP 1 @tokenId = TOKEN_ID
            FROM TSMPWDRESETTOKEN WITH (UPDLOCK, SERIALIZABLE)
            WHERE USER_ID = @p_user_id AND CODE = @p_code AND USED_YN = 'N' AND EXPIRE_DT > GETDATE();

            IF @tokenId IS NULL
            BEGIN
                ROLLBACK TRANSACTION;
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'인증코드가 올바르지 않거나 만료되었습니다.';
                RETURN;
            END

            UPDATE TSMPWDRESETTOKEN SET USED_YN = 'Y' WHERE TOKEN_ID = @tokenId;

            UPDATE TSMUSER
            SET PASSWORD_HASH = @p_new_password_hash,
                MUST_CHANGE_PWD_YN = 'N',
                LAST_PWD_CHANGE_DATE = GETDATE(),
                PWD_FAIL_CNT = 0
            WHERE USER_ID = @p_user_id;

            COMMIT TRANSACTION;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_SHORTCUT_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            -- 사용자 재정의가 있으면 그 값을, 없으면 기본값을 그대로 내려준다. custom_yn은
            -- 화면이 "초기화" 버튼을 그 행에만 켤지 판단하는 용도.
            SELECT
                d.action_cd,
                d.action_nm,
                COALESCE(u.key_combo, d.key_combo) AS key_combo,
                CASE WHEN u.key_combo IS NULL THEN 'N' ELSE 'Y' END AS custom_yn,
                d.sort_order
            FROM TSMSHORTCUTDEFAULT d
            LEFT JOIN TSMUSERSHORTCUT u
                ON u.action_cd = d.action_cd
               AND u.user_id = @p_user_id
            ORDER BY d.sort_order;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- USP_SM_SHORTCUT_S 수정 - @p_client_pc 파라미터 누락 수정.
--
-- 범용 데이터 통로(api/data/save)의 GenericDataRepository.SaveAsync는 어떤 저장 프로시저를
-- 부르든 항상 p_user_id와 p_client_pc 두 개를 같이 넘긴다(클라이언트가 보낸 값을 믿지 않고
-- 서버가 직접 채움 - GenericDataRepository.cs 주석 참고). 014에서 만든 USP_SM_SHORTCUT_S는
-- @p_user_id만 선언하고 @p_client_pc를 빠뜨려서, 이 화면에서 저장/초기화를 누르면 매번
-- "Procedure or function USP_SM_SHORTCUT_S has too many arguments specified" SQL 오류로
-- 실패한다(프로시저가 모르는 파라미터를 서버가 보내므로). USP_SM_MINORCODE_S_1 등 범용
-- 통로를 쓰는 다른 모든 저장 프로시저는 이미 이 파라미터를 갖고 있다 - 값 자체는 감사(audit)
-- 용도라 이 프로시저 안에서는 안 써도, 시그니처에는 반드시 있어야 한다.

CREATE   PROCEDURE [dbo].[USP_SM_SHORTCUT_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_action_cd VARCHAR(20),
    @p_key_combo VARCHAR(30) = NULL,
    @p_user_id VARCHAR(20),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            IF EXISTS (
                SELECT 1
                FROM TSMSHORTCUTDEFAULT d
                LEFT JOIN TSMUSERSHORTCUT u
                    ON u.action_cd = d.action_cd
                   AND u.user_id = @p_user_id
                WHERE d.action_cd <> @p_action_cd
                  AND COALESCE(u.key_combo, d.key_combo) = @p_key_combo
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 다른 동작에 사용 중인 단축키입니다.';
                RETURN;
            END

            MERGE TSMUSERSHORTCUT AS target
            USING (SELECT @p_user_id AS user_id, @p_action_cd AS action_cd) AS src
                ON target.user_id = src.user_id AND target.action_cd = src.action_cd
            WHEN MATCHED THEN
                UPDATE SET key_combo = @p_key_combo, upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHEN NOT MATCHED THEN
                INSERT (user_id, action_cd, key_combo, reg_user_id, reg_dt)
                VALUES (@p_user_id, @p_action_cd, @p_key_combo, @p_user_id, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMUSERSHORTCUT WHERE user_id = @p_user_id AND action_cd = @p_action_cd;
        END

        SET @GeneratedCode = @p_action_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_SITECONFIG_Q
    @p_work_type varchar(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_work_type = 'Q'
    BEGIN
        SELECT
            company_nm AS CompanyNm,
            smtp_host AS SmtpHost,
            smtp_port AS SmtpPort,
            smtp_username AS SmtpUsername,
            smtp_from_address AS SmtpFromAddress,
            smtp_from_display_nm AS SmtpFromDisplayNm,
            file_block_extensions AS FileBlockExtensions,
            file_max_size_mb AS FileMaxSizeMb,
            pwd_expire_days AS PwdExpireDays,
            pwd_lock_threshold AS PwdLockThreshold,
            pwd_reset_code_valid_min AS PwdResetCodeValidMin,
            pwd_min_length AS PwdMinLength,
            CASE WHEN pwd_require_upper_lower = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PwdRequireUpperLower,
            CASE WHEN pwd_require_digit = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PwdRequireDigit,
            CASE WHEN pwd_require_special = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PwdRequireSpecial,
            init_pwd_policy AS InitPwdPolicy,
            CASE WHEN force_change_on_first_login = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS ForceChangeOnFirstLogin,
            idle_timeout_minutes AS IdleTimeoutMinutes,
            required_field_back_color AS RequiredFieldBackColor,
            grid_header_back_color AS GridHeaderBackColor,
            grid_focused_row_back_color AS GridFocusedRowBackColor,
            brand_color AS BrandColor,
            tree_group_back_color AS TreeGroupBackColor,
            divider_color AS DividerColor
        FROM TSMSITECONFIG
        WHERE config_id = 1;
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_SITECONFIG_S
    @p_work_type varchar(10),
    @p_company_nm nvarchar(100) = NULL,
    @p_smtp_host varchar(200) = NULL,
    @p_smtp_port int = NULL,
    @p_smtp_username varchar(200) = NULL,
    @p_smtp_from_address varchar(200) = NULL,
    @p_smtp_from_display_nm nvarchar(100) = NULL,
    @p_file_block_extensions nvarchar(500) = NULL,
    @p_file_max_size_mb int = NULL,
    @p_pwd_expire_days int = NULL,
    @p_pwd_lock_threshold int = NULL,
    @p_pwd_reset_code_valid_min int = NULL,
    @p_pwd_min_length int = NULL,
    @p_pwd_require_upper_lower varchar(1) = NULL,
    @p_pwd_require_digit varchar(1) = NULL,
    @p_pwd_require_special varchar(1) = NULL,
    @p_init_pwd_policy varchar(20) = NULL,
    @p_force_change_on_first_login varchar(1) = NULL,
    @p_idle_timeout_minutes int = NULL,
    @p_required_field_back_color varchar(10) = NULL,
    @p_grid_header_back_color varchar(10) = NULL,
    @p_grid_focused_row_back_color varchar(10) = NULL,
    @p_brand_color varchar(10) = NULL,
    @p_tree_group_back_color varchar(10) = NULL,
    @p_divider_color varchar(10) = NULL,
    @p_user_id varchar(30),
    @p_client_pc nvarchar(200) = NULL,
    @ReturnCode int OUTPUT,
    @ReturnMsg varchar(200) OUTPUT,
    @ErrorCode int OUTPUT,
    @ErrorMsg varchar(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = NULL;
    SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMSITECONFIG
            SET company_nm = @p_company_nm,
                smtp_host = @p_smtp_host,
                smtp_port = @p_smtp_port,
                smtp_username = @p_smtp_username,
                smtp_from_address = @p_smtp_from_address,
                smtp_from_display_nm = @p_smtp_from_display_nm,
                file_block_extensions = @p_file_block_extensions,
                file_max_size_mb = @p_file_max_size_mb,
                pwd_expire_days = @p_pwd_expire_days,
                pwd_lock_threshold = @p_pwd_lock_threshold,
                pwd_reset_code_valid_min = @p_pwd_reset_code_valid_min,
                pwd_min_length = @p_pwd_min_length,
                pwd_require_upper_lower = @p_pwd_require_upper_lower,
                pwd_require_digit = @p_pwd_require_digit,
                pwd_require_special = @p_pwd_require_special,
                init_pwd_policy = @p_init_pwd_policy,
                force_change_on_first_login = @p_force_change_on_first_login,
                idle_timeout_minutes = @p_idle_timeout_minutes,
                required_field_back_color = @p_required_field_back_color,
                grid_header_back_color = @p_grid_header_back_color,
                grid_focused_row_back_color = @p_grid_focused_row_back_color,
                brand_color = @p_brand_color,
                tree_group_back_color = @p_tree_group_back_color,
                divider_color = @p_divider_color,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE config_id = 1;
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_user_nm NVARCHAR(50) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT  U.USER_ID AS UserId,
                        U.USER_NM AS UserNm,
                        U.EMP_ID AS EmpId,
                        E.emp_no AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        D.DEPT_NM AS DeptNm,
                        U.ACC_ID AS AccId,
                        A.ACC_NM AS AccNm,
                        U.EMAIL AS Email,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                        LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
                        LEFT JOIN TBAACC A ON A.ACC_ID = U.ACC_ID
            WHERE (@p_user_id IS NULL OR U.USER_ID LIKE '%' + @p_user_id + '%')
              AND (@p_user_nm IS NULL OR U.USER_NM LIKE '%' + @p_user_nm + '%')
            ORDER BY U.REG_DT DESC;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_Q_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT COUNT(1) FROM TSMUSER WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_Q_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT G.USER_GRP_CD AS UserGrpCd, G.USER_GRP_NM AS UserGrpNm,
                   CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
            FROM TSMUSERGRP G
            LEFT JOIN TSMUSERGRPMAP M ON M.USER_GRP_CD = G.USER_GRP_CD AND M.USER_ID = @p_user_id
            WHERE G.USE_YN = 'Y'
            ORDER BY G.SORT_ORDER;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_S]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @p_user_nm NVARCHAR(50),
    @p_password_hash VARCHAR(200) = NULL,
    @p_emp_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_email NVARCHAR(200) = NULL,
    @p_use_yn CHAR(1) = 'Y',
    @p_user_type VARCHAR(10) = 'U',
    @p_must_change_pwd_yn CHAR(1) = 'N',
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMUSER
                (USER_ID, USER_NM, PASSWORD_HASH, EMP_ID, ACC_ID, EMAIL, USER_TYPE, DEVELOPER_YN, USE_YN, DEL_YN, PWD_FAIL_CNT, MUST_CHANGE_PWD_YN, REG_DT, UPT_DT)
            VALUES (@p_user_id, @p_user_nm, @p_password_hash, @p_emp_id, @p_acc_id, @p_email, @p_user_type, 'N', 'Y', 'N', 0, @p_must_change_pwd_yn, GETDATE(), GETDATE());
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER SET
                USER_NM = @p_user_nm, EMP_ID = @p_emp_id, ACC_ID = @p_acc_id, EMAIL = @p_email, USE_YN = @p_use_yn, USER_TYPE = @p_user_type,
                UPT_DT = GETDATE()
            WHERE USER_ID = @p_user_id;
        END

        SET @GeneratedCode = @p_user_id;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* 물리삭제 대신 USE_YN='N' 처리(사용중지) - 의미상 삭제 동작이라 work_type='D' */
CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    @p_use_yn CHAR(1),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'D'
        BEGIN
            UPDATE TSMUSER SET USE_YN = @p_use_yn WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* 그룹소속 전체 치환(지우고 다시 넣음) - update 성격이라 work_type='U' */
CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_S_2]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20),
    @p_user_grp_cds NVARCHAR(MAX) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            DELETE FROM TSMUSERGRPMAP WHERE USER_ID = @p_user_id;

            IF @p_user_grp_cds IS NOT NULL AND LEN(@p_user_grp_cds) > 0
            BEGIN
                INSERT INTO TSMUSERGRPMAP (USER_ID, USER_GRP_CD)
                SELECT @p_user_id, value FROM STRING_SPLIT(@p_user_grp_cds, ',');
            END
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- 로그인된 상태에서 비밀번호를 바꾼다(강제변경 다이얼로그 등) - 현재 비밀번호 확인은 C#
-- (AuthService, BCrypt.Verify)에서 이미 끝내고 새 해시만 넘겨받는다.
CREATE   PROCEDURE [dbo].[USP_SM_USERAUTH_S_3]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_password_hash VARCHAR(200) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSER
            SET PASSWORD_HASH = @p_password_hash,
                MUST_CHANGE_PWD_YN = 'N',
                LAST_PWD_CHANGE_DATE = GETDATE(),
                PWD_FAIL_CNT = 0
            WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ---------- USERGRP (010 Q류 + 020 S류) ---------- */

CREATE   PROCEDURE USP_SM_USERGRP_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_nm NVARCHAR(100) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT G.USER_GRP_CD AS UserGrpCd, G.USER_GRP_NM AS UserGrpNm, G.DESCRIPTION AS Description,
                   G.SORT_ORDER AS SortOrder, G.USE_YN AS UseYn,
                   (SELECT COUNT(1) FROM TSMUSERGRPMAP M WHERE M.USER_GRP_CD = G.USER_GRP_CD) AS MemberCount
            FROM TSMUSERGRP G
            WHERE (@p_user_grp_nm IS NULL OR G.USER_GRP_NM LIKE '%' + @p_user_grp_nm + '%')
            ORDER BY G.SORT_ORDER;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_USERGRP_Q_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT COUNT(1) FROM TSMUSERGRP WHERE USER_GRP_CD = @p_user_grp_cd;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_USERGRP_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, D.DEPT_NM AS DeptNm,
                   CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            LEFT JOIN TSMUSERGRPMAP M ON M.USER_ID = U.USER_ID AND M.USER_GRP_CD = @p_user_grp_cd
            WHERE U.USE_YN = 'Y'
            ORDER BY U.USER_NM;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE USP_SM_USERGRP_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
    @p_user_grp_nm NVARCHAR(100),
    @p_description NVARCHAR(200) = NULL,
    @p_sort_order INT = 0,
    @p_use_yn CHAR(1) = 'Y',
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMUSERGRP (USER_GRP_CD, USER_GRP_NM, DESCRIPTION, SORT_ORDER, USE_YN)
            VALUES (@p_user_grp_cd, @p_user_grp_nm, @p_description, @p_sort_order, 'Y');
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMUSERGRP SET
                USER_GRP_NM = @p_user_grp_nm, DESCRIPTION = @p_description, SORT_ORDER = @p_sort_order, USE_YN = @p_use_yn
            WHERE USER_GRP_CD = @p_user_grp_cd;
        END

        SET @GeneratedCode = @p_user_grp_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* 사용중지(물리삭제 대신) - work_type='D' */
CREATE   PROCEDURE USP_SM_USERGRP_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
    @p_use_yn CHAR(1),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'D'
        BEGIN
            UPDATE TSMUSERGRP SET USE_YN = @p_use_yn WHERE USER_GRP_CD = @p_user_grp_cd;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* 소속 사용자 전체 치환(지우고 다시 넣음) - work_type='U' */
CREATE   PROCEDURE USP_SM_USERGRP_S_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
    @p_user_ids NVARCHAR(MAX) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            DELETE FROM TSMUSERGRPMAP WHERE USER_GRP_CD = @p_user_grp_cd;

            IF @p_user_ids IS NOT NULL AND LEN(@p_user_ids) > 0
            BEGIN
                INSERT INTO TSMUSERGRPMAP (USER_GRP_CD, USER_ID)
                SELECT @p_user_grp_cd, value FROM STRING_SPLIT(@p_user_ids, ',');
            END
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END

GO
