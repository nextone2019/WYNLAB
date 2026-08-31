-- 품목 관련 메뉴를 그룹으로 정리한다. 기존 BA_ITEM(품목등록, frmItem)은 BA 바로 밑
-- 단독 메뉴였는데, 그 밑에 품목현황(신규, frmItemList)을 형제로 추가하려면 그룹이 필요하다.
--
-- 그룹 코드를 BA_ITEM으로 쓰면 기존 BA_ITEM(frmItem)과 코드가 겹치므로, SM 모듈의 그룹코드
-- 관례(SM_USRG/SM_MENUG/SM_CODEG - 주제어+G)를 언더스코어로 구분해 가져와 BA_ITEM_G로 한다.
-- 자식 메뉴 코드는 그대로 두면(BA_ITEM, BA_ITEM_LIST) menu_cd 마지막 조각을 PascalCase로
-- 바꾸기만 해도 폼 클래스 이름이 나온다(BA_ITEM -> frmItem, BA_ITEM_LIST -> frmItemList) -
-- 이 코드베이스 전체가 쓰는 관례(예: SM_MINOR_CODE -> frmMinorCode).

-- 1) 그룹 신설 - BA 바로 밑, 기존 BA_ORG(조직관리, SORT_ORDER=10)보다 뒤에 오도록 20.
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, SORT_ORDER, USE_YN, REG_DT)
VALUES ('BA_ITEM_G', N'품목관리', 'BA', 2, 'GROUP', 20, 'Y', GETDATE());
GO

-- 2) 기존 BA_ITEM(품목등록)을 새 그룹 밑으로 옮긴다 - 화면/프로시저는 전혀 안 건드린다.
-- MENU_LEVEL은 BA_ITEM_G도 레벨2라 3 그대로 안 바뀐다. 새 그룹의 첫 자식이라 SORT_ORDER를 10으로.
UPDATE TSMMENU
SET UPPER_MENU_CD = 'BA_ITEM_G', SORT_ORDER = 10
WHERE MENU_CD = 'BA_ITEM';
GO

-- 3) 품목현황(신규) - 범용 데이터 통로를 쓰므로 PROC_PREFIX만 맞춰두면 서버 코드 없이 동작한다
-- (GENERIC_DATA_API.md). USP_BA_ITEM_LIST_* 프로시저와 frmItemList 화면은 별도로 만든다.
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER, USE_YN, REG_DT, PROC_PREFIX)
VALUES ('BA_ITEM_LIST', N'품목현황', 'BA_ITEM_G', 3, 'FORM', 'WYNLAB.BA.frmItemList, WYNLAB.BA', 20, 'Y', GETDATE(), 'USP_BA_ITEM_LIST_');
GO
