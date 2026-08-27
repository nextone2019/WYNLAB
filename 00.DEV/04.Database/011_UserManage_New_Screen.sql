-- 사용자관리 화면을 기초코드등록(frmMinorCode)과 같은 디자인 패턴(좌측 리스트 조회전용 +
-- 우측 상세패널 저장)으로 새로 짠 frmUserManage로 교체한다. 기존 UserListForm(목록) +
-- UserEditForm(수정 팝업, 그룹배정 포함) 파일은 지우지 않고 그대로 남겨뒀다 - 검토 후
-- 마음에 안 들면 이 UPDATE를 되돌리기만 하면 즉시 원복된다.
--
-- 서버(api/users, api/users/{id}/groups, api/menu-auth)는 전혀 안 건드렸다 - 기존
-- UserEditForm/PermissionAssignForm이 쓰던 API를 그대로 재사용한다.

UPDATE TSMMENU
   SET FORM_CLASS_NM = 'WYNLAB.SM.USER.frmUserManage, WYNLAB.SM.USER'
 WHERE MENU_CD = 'SM_USER'
   AND FORM_CLASS_NM <> 'WYNLAB.SM.USER.frmUserManage, WYNLAB.SM.USER';
GO

SELECT MENU_CD, FORM_CLASS_NM FROM TSMMENU WHERE MENU_CD = 'SM_USER';
GO
