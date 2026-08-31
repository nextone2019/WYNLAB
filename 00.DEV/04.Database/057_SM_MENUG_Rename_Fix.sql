-- 056에서 SM_MENUG -> SM_MENU_G로 바꾸려던 UPDATE 하나만 FK_TSMMENUAUTH_MENU 충돌로 실패했다.
-- 원인: TSMMENUAUTH_OLD_20260831(042 마이그레이션이 TSMMENUAUTH 재구성 전에 남겨둔 백업
-- 테이블, 실사용 안 함)이 아직 TSMMENU.MENU_CD를 FK로 참조하고 있는데, 그 백업에 남아있는
-- 옛 SM_MENUG 행 하나가 막았다. 백업 테이블의 그 값도 같이 옮겨서 FK를 만족시킨 뒤 재시도한다.

UPDATE dbo.TSMMENUAUTH_OLD_20260831 SET MENU_CD = 'SM_MENU_G' WHERE MENU_CD = 'SM_MENUG';
GO

UPDATE TSMMENU SET MENU_CD = 'SM_MENU_G' WHERE MENU_CD = 'SM_MENUG';
GO
