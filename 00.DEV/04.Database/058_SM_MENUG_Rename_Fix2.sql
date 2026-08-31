-- 057도 실패했다 - FK_TSMMENUAUTH_MENU가 TSMMENUAUTH_OLD_20260831.MENU_CD -> TSMMENU.MENU_CD를
-- 거는 FK라(백업 테이블이 자식), 부모(TSMMENU)를 먼저 바꾸면 자식이 옛 값을 참조 중이라 막히고
-- 자식을 먼저 바꾸면 부모에 새 값이 아직 없어서 막힌다(닭과 달걀). TSMMENUAUTH_OLD_20260831은
-- 042 마이그레이션이 TSMMENUAUTH 재구성 전 스냅샷으로 남겨둔 죽은 백업 테이블이라(실사용 코드
-- 어디서도 안 씀) 앞으로도 TSMMENU 변경을 막을 이유가 없다 - FK 자체를 정리한다.

ALTER TABLE dbo.TSMMENUAUTH_OLD_20260831 DROP CONSTRAINT FK_TSMMENUAUTH_MENU;
GO

UPDATE TSMMENU SET MENU_CD = 'SM_MENU_G' WHERE MENU_CD = 'SM_MENUG';
GO

-- 백업 테이블도 참고용으로 값을 맞춰둔다(필수는 아니지만 기록 일관성을 위해).
UPDATE dbo.TSMMENUAUTH_OLD_20260831 SET MENU_CD = 'SM_MENU_G' WHERE MENU_CD = 'SM_MENUG';
GO
