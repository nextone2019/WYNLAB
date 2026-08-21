/* =========================================================
   화면 프로젝트/DLL을 WYNLAB.{모듈코드}.{화면코드} 규칙으로 재명명한 데 따른
   FORM_CLASS_NM 재갱신 (016번 스크립트에서 WYNLAB.Screens.* 로 한 번 바꾼 걸 다시 갱신).
   ========================================================= */

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.SM.USER.UserListForm, WYNLAB.SM.USER'
    WHERE MENU_CD = 'SM_USER';

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.SM.USERGROUP.UserGroupListForm, WYNLAB.SM.USERGROUP'
    WHERE MENU_CD = 'SM_USERGRP';

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.SM.MENU.MenuListForm, WYNLAB.SM.MENU'
    WHERE MENU_CD = 'SM_MENU';

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.SM.USERAUTH.PermissionAssignForm, WYNLAB.SM.USERAUTH'
    WHERE MENU_CD = 'SM_AUTH';
