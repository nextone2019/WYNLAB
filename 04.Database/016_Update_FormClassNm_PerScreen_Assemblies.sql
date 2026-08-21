/* =========================================================
   화면별 프로젝트/DLL 분리에 따른 FORM_CLASS_NM 갱신.

   기존엔 6개 화면이 전부 WYNLAB.Modules.System 프로젝트(DLL) 하나에 있었는데,
   이제 화면(메뉴)당 프로젝트 하나로 쪼갰다. TSMMENU.FORM_CLASS_NM은
   "Namespace.TypeName, AssemblyName" 형식의 어셈블리 정규화 이름이라, 네임스페이스와
   어셈블리명이 둘 다 바뀐 지금 값들을 갱신해야 ShellForm.OpenMenuForm의 Type.GetType이
   다시 찾을 수 있다(런타임 로더는 WYNLAB.UI.Common.ModuleLoader, appsettings.json의
   ModulesPath 폴더를 앱 시작 시 스캔해서 로드).
   ========================================================= */

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.Screens.UserManage.UserListForm, WYNLAB.Screens.UserManage'
    WHERE MENU_CD = 'SM_USER';

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.Screens.UserGroupManage.UserGroupListForm, WYNLAB.Screens.UserGroupManage'
    WHERE MENU_CD = 'SM_USERGRP';

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.Screens.MenuManage.MenuListForm, WYNLAB.Screens.MenuManage'
    WHERE MENU_CD = 'SM_MENU';

UPDATE TSMMENU SET FORM_CLASS_NM =
    'WYNLAB.Screens.PermissionAssign.PermissionAssignForm, WYNLAB.Screens.PermissionAssign'
    WHERE MENU_CD = 'SM_AUTH';
