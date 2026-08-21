/* =========================================================
   사용자그룹관리 화면을 시스템운영관리(SM) 메뉴 하위에 등록
   ========================================================= */

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER) VALUES
('SM_USERGRP', N'사용자그룹관리', 'SM', 2, 'FORM',
 'WYNLAB.Modules.System.UserGroupListForm, WYNLAB.Modules.System',
 NULL, 15);

-- 시스템관리자 그룹에 조회/등록/수정/삭제 전체 권한 부여 (다른 관리 메뉴와 동일한 패턴)
INSERT INTO TSMMENUAUTH (MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, UPDATE_YN, DELETE_YN) VALUES
('SM_USERGRP', 'GRP', 'ADMIN', 'Y', 'Y', 'Y', 'Y');
