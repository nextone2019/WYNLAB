-- SYS 모듈 최상위 메뉴 그룹 + 팝업관리(frmSysPopup) 화면 메뉴 등록.
-- SYS는 BA/MA/SA/PR과 나란한 최상위 그룹이지만, 성격이 "업무 데이터"가 아니라 "잘못 건드리면
-- 시스템 전체에 영향 주는 설정"이라 사장님이 의도적으로 분리를 요청함 - 접근제어를 메뉴권한만으로
-- 끝내지 않기로 한 결정은 project_wynlab_popup_lookup_framework 메모리 참고(아직 확정 전).

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN, PROC_PREFIX)
VALUES ('SYS', N'시스템설정', NULL, 1, 'GROUP', NULL, NULL, 90, 'Y', NULL);

INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN, PROC_PREFIX)
VALUES ('SYS_POPUP', N'팝업관리', 'SYS', 2, 'FORM', 'WYNLAB.SYS.frmSysPopup, WYNLAB.SYS', NULL, 10, 'Y', 'USP_SYS_POPUP_');
GO
