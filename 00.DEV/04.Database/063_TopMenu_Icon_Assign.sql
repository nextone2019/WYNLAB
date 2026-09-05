-- 기준정보관리(BA)와 Developer Tool(SYS)의 ICON_NM이 둘 다 NULL이라 좌측 최상위 메뉴에서
-- 같은 기본(Folder) 아이콘으로 보여 구분이 안 됐다(2026-09-02 지적). 각각 다른 아이콘으로 배정.
-- ICON_NM 값은 WYNLAB.Shell/ShellForm.cs의 TopMenuIcons 딕셔너리 키와 맞춰야 한다.
UPDATE TSMMENU SET ICON_NM = 'box' WHERE MENU_CD = 'BA';
UPDATE TSMMENU SET ICON_NM = 'code' WHERE MENU_CD = 'SYS';
