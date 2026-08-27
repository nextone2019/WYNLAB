-- 네이밍 통일 - BA_ORG_DEPT/BA_ORG_EMP를 다른 모듈 규칙(모듈코드_화면명, 예: SM_USER)과 맞춰
-- BA_DEPT/BA_EMP로 단순화한다.
--
-- BA_CUST_REG(거래처등록)는 이번에 BA_CUST로 못 바꾼다 - BA_CUST는 이미 "거래처관리"
-- GROUP 메뉴가 쓰고 있는 코드라(TSMMENU PK 충돌) 그대로 두었다. 다른 이름으로 정하면
-- 별도 마이그레이션으로 처리할 것.
--
-- MENU_CD는 FK 제약(FK_TSMMENUAUTH_MENU: TSMMENUAUTH.MENU_CD -> TSMMENU.MENU_CD)이 걸려있어서,
-- TSMMENU(부모)를 먼저 바꾸면 TSMMENUAUTH(자식)의 남은 참조가 끊기고, TSMMENUAUTH를 먼저
-- 바꾸면 TSMMENU에 새 코드가 아직 없어서 막힌다(둘 다 실제로 겪음, 처음 시도가 그래서 전부
-- 실패하고 롤백됐다) - 한 트랜잭션 안에서 제약을 잠깐 꺼뒀다가 다시 켜서 검증한다.

BEGIN TRAN;

ALTER TABLE TSMMENUAUTH NOCHECK CONSTRAINT FK_TSMMENUAUTH_MENU;

UPDATE TSMMENUAUTH SET MENU_CD = 'BA_DEPT' WHERE MENU_CD = 'BA_ORG_DEPT';
UPDATE TSMMENUAUTH SET MENU_CD = 'BA_EMP' WHERE MENU_CD = 'BA_ORG_EMP';

UPDATE TSMMENU SET MENU_CD = 'BA_DEPT' WHERE MENU_CD = 'BA_ORG_DEPT';
UPDATE TSMMENU SET MENU_CD = 'BA_EMP' WHERE MENU_CD = 'BA_ORG_EMP';

-- WITH CHECK로 다시 켜야 이후에도 제약이 실제로 검증된다(그냥 CHECK CONSTRAINT만 하면
-- 켜지기만 하고 기존 데이터는 재검증을 안 해서, 위에서 잠깐 꺼둔 사이에 실수로 끊어진
-- 참조가 있어도 조용히 넘어간다).
ALTER TABLE TSMMENUAUTH WITH CHECK CHECK CONSTRAINT FK_TSMMENUAUTH_MENU;

COMMIT TRAN;
GO

SELECT MENU_CD, MENU_NM, UPPER_MENU_CD, FORM_CLASS_NM FROM TSMMENU WHERE MENU_CD IN ('BA_DEPT', 'BA_EMP', 'BA_CUST_REG');
GO
