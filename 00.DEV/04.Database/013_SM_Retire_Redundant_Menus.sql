-- frmUserManage(사용자등록, SM_USER)가 이미 그룹배정+권한부여를 자체 탭으로 전부 구현하므로,
-- 별도 화면(UserGroupListForm/PermissionAssignForm - 방금 소스 자체를 삭제함)을 가리키던
-- 사용자그룹관리/권한부여관리 메뉴는 더 이상 필요 없다. 사용자관리(SM_USRG) 그룹 아래엔
-- 이제 사용자등록(SM_USER) 하나만 남는다(메뉴관리처럼 그룹 밑에 리프 하나만 있는 구성은
-- 기존에도 있던 패턴 - SM_MENUG 그룹 아래 SM_MENU 하나뿐인 것과 동일).
--
-- TSMMENUAUTH(메뉴별 권한)에 이 두 MENU_CD를 참조하는 행이 없음을 미리 확인했다(0건) -
-- 별도 정리 없이 바로 삭제해도 고아 데이터가 남지 않는다.

DELETE FROM TSMMENU WHERE MENU_CD IN ('SM_USERGRP', 'SM_AUTH');
GO

SELECT MENU_CD, MENU_NM, UPPER_MENU_CD FROM TSMMENU WHERE UPPER_MENU_CD = 'SM_USRG' ORDER BY SORT_ORDER;
GO
