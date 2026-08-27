-- 017 마이그레이션에서 USP_SM_USER_Q_2(그룹배정 목록 조회 - UserManageRepository.GetUserGroupsAsync)를
-- 이름 바꾸기 목록에서 빠뜨렸다. 코드는 이미 USP_SM_USERAUTH_Q_2를 부르도록 고쳐져 있어서(017과
-- 같이 배포됨), 이 프로시저만 옛 이름으로 남아있으면 "프로시저를 찾을 수 없음" SQL 오류로
-- api/users/{id}/groups 호출이 500으로 죽는다(frmUserAuth 첫 테스트에서 실제로 겪음).

EXEC sp_rename 'USP_SM_USER_Q_2', 'USP_SM_USERAUTH_Q_2';
GO

SELECT name FROM sys.objects WHERE type = 'P' AND name = 'USP_SM_USERAUTH_Q_2';
GO
