/* =========================================================
   권한부여관리 화면을 시스템운영관리(SM) 메뉴 하위에 등록
   ========================================================= */

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER) VALUES
('SM_AUTH', N'권한부여관리', 'SM', 2, 'FORM',
 'WYNLAB.Modules.System.PermissionAssignForm, WYNLAB.Modules.System',
 NULL, 30);
