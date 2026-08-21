/* =========================================================
   메뉴트리 구조 미리보기용 시딩 스크립트 (메뉴등록.xlsx 기준)

   사용자가 제공한 엑셀(대분류/중분류/소분류 3단 들여쓰기)을 그대로 옮기되,
   이미 개발되어 연결되어 있던 화면(사용자등록/사용자그룹등록/메뉴관리/권한부여관리)은
   기존 MENU_CD를 그대로 유지하고(TSMMENUAUTH 권한 부여 이력이 걸려있는 코드도 있어서
   코드를 바꾸면 그 권한이 끊어짐) 위치(UPPER_MENU_CD)/레벨/이름만 새 구조에 맞게 옮겼다.
   아직 개발 안 된 화면은 FORM_CLASS_NM을 NULL로 두고 MENU_TYPE='FORM'(리프)으로만 등록 -
   실제 앱에서 클릭하면 "연결된 화면이 없습니다" 안내만 뜨고 등록/삭제 자체는 문제없다.

   정리한 것:
   - '1111'(사용자관리, 빈 테스트용 그룹으로 보임, 하위 메뉴/권한 없음) 삭제
   - 'SA_EST'(견적관리, 001 스크립트의 샘플 데이터, 하위/권한 없음) 삭제
   - 'SA_ORDER'는 기존에 FORM(리프)이었는데 엑셀에서는 "수주관리"가 그룹이라 GROUP으로 전환
     (SALES_MGR/SALES_STF 그룹에 이미 걸려있던 권한은 MENU_CD를 그대로 써서 안 끊어짐)
   - 'SA_SHIP'(기존 "출고관리")은 엑셀의 "출하요청등록" 그룹에 대응하는 것으로 보고 이름만 변경

   참고: 엑셀 원본에서 "영업관리 > 출하요청등록(중분류) > 출하요청등록(소분류)"처럼
   중분류/소분류 이름이 똑같이 중복되어 있음 - 오타인지 의도한 것인지 확인 필요해서
   일단 원본 그대로 반영했다.
   ========================================================= */

-- 하위/권한 참조가 없는 걸 확인한 스트레이 데이터 정리
DELETE FROM TSMMENU WHERE MENU_CD = '1111';
DELETE FROM TSMMENU WHERE MENU_CD = 'SA_EST';

/* ---------------- 시스템운영관리(SM) 하위 재구성 ---------------- */

-- 새 중분류 그룹 3개
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('SM_USRG',  N'사용자관리', 'SM', 2, 'GROUP', NULL, 10),
('SM_MENUG', N'메뉴관리',   'SM', 2, 'GROUP', NULL, 20),
('SM_CODEG', N'코드관리',   'SM', 2, 'GROUP', NULL, 30);

-- 기존에 이미 개발/연결되어 있던 화면들을 새 그룹 하위(소분류)로 이동 + 엑셀 표기에 맞춰 이름 정리.
-- SM_USERGRP는 007_UserGroupManage_Menu.sql에서 등록하기로 되어 있었는데 이 DB엔 실제로
-- 없었다(스크립트가 적용 안 됐던 것으로 보임) - 있으면 옮기고, 없으면 새로 등록한다.
IF EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_CD = 'SM_USERGRP')
    UPDATE TSMMENU SET MENU_NM = N'사용자그룹등록', UPPER_MENU_CD = 'SM_USRG', MENU_LEVEL = 3, SORT_ORDER = 10 WHERE MENU_CD = 'SM_USERGRP';
ELSE
    INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
    ('SM_USERGRP', N'사용자그룹등록', 'SM_USRG', 3, 'FORM',
     'WYNLAB.Modules.System.UserGroupListForm, WYNLAB.Modules.System', 10);

UPDATE TSMMENU SET UPPER_MENU_CD = 'SM_USRG', MENU_LEVEL = 3, SORT_ORDER = 20 WHERE MENU_CD = 'SM_USER';
UPDATE TSMMENU SET MENU_NM = N'권한등록', UPPER_MENU_CD = 'SM_USRG', MENU_LEVEL = 3, SORT_ORDER = 30 WHERE MENU_CD = 'SM_AUTH';
UPDATE TSMMENU SET MENU_NM = N'메뉴등록', UPPER_MENU_CD = 'SM_MENUG', MENU_LEVEL = 3, SORT_ORDER = 10 WHERE MENU_CD = 'SM_MENU';

-- 코드관리 하위 - 아직 화면 없음
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('SM_CODE_BASE', N'기초코드등록', 'SM_CODEG', 3, 'FORM', NULL, 10),
('SM_CODE_AUTO', N'자동채번등록', 'SM_CODEG', 3, 'FORM', NULL, 20);

/* ---------------- 기준정보관리(BS) - 신규 대분류 ---------------- */

-- ICON_NM은 일부러 비워둠: ShellForm의 TopMenuIcons엔 settings(SM)/shoppingcart(SA)/tools(PR)만
-- 매핑되어 있어서, 없는 이름을 쓰면 기본 폴더 아이콘으로 대체됨 - PR과 아이콘이 겹치는 것보다 낫다.
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('BS', N'기준정보관리', NULL, 1, 'GROUP', NULL, 15);

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('BS_ORG',  N'조직관리',   'BS', 2, 'GROUP', NULL, 10),
('BS_ITEM', N'품목관리',   'BS', 2, 'GROUP', NULL, 20),
('BS_CUST', N'거래처관리', 'BS', 2, 'GROUP', NULL, 30);

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('BS_ORG_DEPT', N'부서등록', 'BS_ORG', 3, 'FORM', NULL, 10),
('BS_ORG_EMP',  N'사원등록', 'BS_ORG', 3, 'FORM', NULL, 20),

('BS_ITEM_GRP', N'품목그룹등록', 'BS_ITEM', 3, 'FORM', NULL, 10),
('BS_ITEM_REG', N'품목등록',     'BS_ITEM', 3, 'FORM', NULL, 20),

('BS_CUST_REG',  N'거래처등록',      'BS_CUST', 3, 'FORM', NULL, 10),
('BS_CUST_BULK', N'거래처 일괄등록', 'BS_CUST', 3, 'FORM', NULL, 20),
('BS_CUST_STAT', N'거래처현황',      'BS_CUST', 3, 'FORM', NULL, 30);

/* ---------------- 영업관리(SA) 하위 재구성 ---------------- */

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('SA_BASE', N'영업기준관리', 'SA', 2, 'GROUP', NULL, 10);

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('SA_BASE_PRICE', N'판매단가등록', 'SA_BASE', 3, 'FORM', NULL, 10);

-- 기존 SA_ORDER(예전엔 리프였던 "수주관리")를 그룹으로 전환 - 기존 권한(SALES_MGR/SALES_STF)은 유지됨
UPDATE TSMMENU SET MENU_TYPE = 'GROUP', SORT_ORDER = 20 WHERE MENU_CD = 'SA_ORDER';

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('SA_ORDER_REG',  N'수주등록', 'SA_ORDER', 3, 'FORM', NULL, 10),
('SA_ORDER_STAT', N'수주현황', 'SA_ORDER', 3, 'FORM', NULL, 20);

-- 기존 SA_SHIP("출고관리")을 엑셀의 "출하요청등록" 그룹으로 이름만 변경
UPDATE TSMMENU SET MENU_NM = N'출하요청등록', SORT_ORDER = 30 WHERE MENU_CD = 'SA_SHIP';

-- 엑셀 원본 그대로: 중분류와 소분류 이름이 동일("출하요청등록")
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER) VALUES
('SA_SHIP_REQ', N'출하요청등록', 'SA_SHIP', 3, 'FORM', NULL, 10),
('SA_SHIP_REG', N'출하등록',     'SA_SHIP', 3, 'FORM', NULL, 20);
