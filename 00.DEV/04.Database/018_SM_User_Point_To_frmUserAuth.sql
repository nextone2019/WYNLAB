-- 사용자관리 메뉴(SM_USER)가 열던 화면을 frmUserManage -> frmUserAuth로 교체한다.
-- frmUserManage 파일은 지우지 않고 BACK_frmUserManage 폴더에 그대로 남겨뒀다(참고용, 검토 후
-- 나중에 정리하기로 함 - 011/012 마이그레이션과 같은 패턴). 되돌리려면 이 UPDATE만 반대로
-- 돌리면 즉시 원복된다.

UPDATE TSMMENU
   SET FORM_CLASS_NM = 'WYNLAB.SM.frmUserAuth, WYNLAB.SM'
 WHERE MENU_CD = 'SM_USER'
   AND FORM_CLASS_NM <> 'WYNLAB.SM.frmUserAuth, WYNLAB.SM';
GO

SELECT MENU_CD, FORM_CLASS_NM FROM TSMMENU WHERE MENU_CD = 'SM_USER';
GO
