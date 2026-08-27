-- BA_CUST/BA_ITEM 코드 단순화(BA_DEPT/BA_EMP와 같은 규칙)를 계속 가로막던 문제 - 그 코드를
-- 이미 상위 GROUP(폴더) 메뉴가 쓰고 있었다(TSMMENU PK라 폼이 같은 코드를 못 씀,
-- 020/022 마이그레이션 커밋 메시지 참고). 사용자가 그룹 자체를 없애고 화면만 남기는 쪽으로
-- 결정 - BA 밑에 화면들을 폴더 없이 바로 평평하게 둔다.
--
-- 아직 화면이 없던 형제 메뉴(BA_CUST_BULK 거래처 일괄등록, BA_CUST_STAT 거래처현황)도
-- 이번에 같이 지운다(사용자 확인) - 나중에 필요하면 새로 만들면 된다.
--
-- TSMMENUAUTH에 이 코드들을 참조하는 행이 하나도 없어서(직접 확인함) FK 제약에 안 걸리므로
-- 021과 달리 NOCHECK 없이 바로 진행한다.

-- 1) 실제 화면이 있는 폼들을 BA 밑으로 바로 옮긴다(그룹 없이 평평하게).
UPDATE TSMMENU SET UPPER_MENU_CD = 'BA' WHERE MENU_CD IN ('BA_CUST_REG', 'BA_ITEM_REG', 'BA_ITEM_GRP');

-- 2) 아직 화면 없는 형제 메뉴는 삭제.
DELETE FROM TSMMENU WHERE MENU_CD IN ('BA_CUST_BULK', 'BA_CUST_STAT');

-- 3) 이제 자식이 없는 옛 GROUP(폴더) 메뉴를 지운다.
DELETE FROM TSMMENU WHERE MENU_CD IN ('BA_CUST', 'BA_ITEM');

-- 4) 화면 폼의 코드를 원하던 단순한 이름으로 바꾼다 - 이제 그 이름을 쓰던 그룹이 없으니
--    PK 충돌이 안 난다.
UPDATE TSMMENU SET MENU_CD = 'BA_CUST' WHERE MENU_CD = 'BA_CUST_REG';
UPDATE TSMMENU SET MENU_CD = 'BA_ITEM' WHERE MENU_CD = 'BA_ITEM_REG';
UPDATE TSMMENU SET MENU_CD = 'BA_ITEMGRP' WHERE MENU_CD = 'BA_ITEM_GRP';
GO

SELECT MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_TYPE, FORM_CLASS_NM FROM TSMMENU WHERE UPPER_MENU_CD = 'BA' ORDER BY SORT_ORDER;
GO
