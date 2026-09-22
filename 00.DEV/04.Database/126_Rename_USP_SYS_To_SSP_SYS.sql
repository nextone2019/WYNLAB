-- 프로시저 네이밍 정리(사장님 지시, 2026-09-10) - LookUp/팝업 "관리" 프로시저(sysLookupM/P/C,
-- sysPopupM/D/S를 다루는 USP_SYS_LOOKUP_*/USP_SYS_POPUP_* 8개)를 그 관리 대상인 "콘텐츠"
-- 프로시저(SSP_POP_*/SSP_CBO_*)와 같은 SSP_ 접두사로 통일한다. sp_rename은 프로시저 본문 텍스트는
-- 안 바꾸므로(sys.sql_modules에는 원래 CREATE 문 그대로 남음 - 실행에는 지장 없음), 이후 이 8개를
-- CREATE OR ALTER할 일이 생기면 새 이름(SSP_SYS_*)으로 작성해야 한다.

EXEC sp_rename 'dbo.USP_SYS_LOOKUP_Q', 'SSP_SYS_LOOKUP_Q', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_LOOKUP_S', 'SSP_SYS_LOOKUP_S', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_LOOKUP_S_1', 'SSP_SYS_LOOKUP_S_1', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_LOOKUP_S_2', 'SSP_SYS_LOOKUP_S_2', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_POPUP_Q', 'SSP_SYS_POPUP_Q', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_POPUP_S', 'SSP_SYS_POPUP_S', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_POPUP_S_1', 'SSP_SYS_POPUP_S_1', 'OBJECT';
EXEC sp_rename 'dbo.USP_SYS_POPUP_S_2', 'SSP_SYS_POPUP_S_2', 'OBJECT';
GO

-- 범용 데이터 통로(api/data/*)가 화면(SYS_LOOKUP/SYS_POPUP) 메뉴의 TSMMENU.PROC_PREFIX와 요청
-- 프로시저명이 StartsWith로 맞는지 검사하므로(DataController), 프로시저 이름을 바꿨으면 이것도
-- 같이 바꿔야 frmSysLookup/frmSysPopup의 조회/저장이 계속 통과한다. 036/038 마이그레이션 원문은
-- MENU_CD로 INSERT했지만 그 뒤 스키마가 MENU_ID(identity)+MODULE+SCREEN_CLASS_NM로 바뀌어서
-- MENU_CD 컬럼 자체가 이제 없다(라이브 DB로 직접 확인, 옛 마이그레이션 파일을 그대로 믿지 않음) -
-- 그래서 SCREEN_CLASS_NM으로 찾는다.
UPDATE TSMMENU SET PROC_PREFIX = 'SSP_SYS_LOOKUP_' WHERE MODULE = 'SYS' AND SCREEN_CLASS_NM = 'frmSysLookup';
UPDATE TSMMENU SET PROC_PREFIX = 'SSP_SYS_POPUP_' WHERE MODULE = 'SYS' AND SCREEN_CLASS_NM = 'frmSysPopup';
GO
