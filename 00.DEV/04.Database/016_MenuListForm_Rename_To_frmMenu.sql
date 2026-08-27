-- 메뉴관리 화면 클래스명 변경: MenuListForm -> frmMenu (프로젝트 폴더/파일명을 화면명과
-- 일치시키는 정리 작업의 일환 - CODE->frmMinorCode, MENU->frmMenu, SHORTCUT->frmShortcut,
-- USER_BACK->frmUserManage). 네임스페이스(WYNLAB.SM.MENU)는 그대로 두고 클래스명만 바꿨다.

UPDATE TSMMENU
   SET FORM_CLASS_NM = 'WYNLAB.SM.MENU.frmMenu, WYNLAB.SM'
 WHERE MENU_CD = 'SM_MENU'
   AND FORM_CLASS_NM <> 'WYNLAB.SM.MENU.frmMenu, WYNLAB.SM';
GO

SELECT MENU_CD, FORM_CLASS_NM FROM TSMMENU WHERE MENU_CD = 'SM_MENU';
GO
