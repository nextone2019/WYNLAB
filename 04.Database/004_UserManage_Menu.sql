/* =========================================================
   사용자관리 화면을 시스템운영관리(SM) 메뉴 하위에 등록
   FORM_CLASS_NM은 어셈블리 정규화 이름(클래스명, 어셈블리명) 형태로 작성해야
   ShellForm의 Type.GetType()이 다른 프로젝트(DLL)의 클래스도 찾을 수 있음
   ========================================================= */

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER) VALUES
('SM_USER', N'사용자관리', 'SM', 2, 'FORM',
 'NEXTFramework.Modules.System.UserListForm, NEXTFramework.Modules.System',
 NULL, 10);
