-- 프로시저명을 화면명과 일치시키는 명명 규칙 적용 - 이 사용자 조회/저장 프로시저 계열은
-- frmUserAuth 화면(탭1 "사용자별 권한관리")이 쓰므로 USP_SM_USER_* -> USP_SM_USERAUTH_*로
-- 이름을 맞춘다. 앞으로 새 화면을 만들 때도 그 화면이 주로 쓰는 프로시저는 화면명과 접두사를
-- 맞추기로 함(사장님 지시, 2026-08-27).
--
-- sp_rename으로 이름만 바꾼다 - 본문(SELECT/INSERT 등)은 그대로라 다시 짤 필요가 없다. 본문
-- 안의 "CREATE PROCEDURE USP_SM_USER_Q" 같은 헤더 텍스트는 sp_rename이 못 고치지만(주석처럼
-- 남는 것뿐, sys.objects의 실제 이름은 정상적으로 바뀜) 동작에는 영향이 없다.
--
-- 호출하는 쪽(UserManageRepository.cs)도 새 이름으로 같이 고쳐야 한다 - 이 마이그레이션과
-- 그 코드 수정은 반드시 같이 배포해야 한다.

EXEC sp_rename 'USP_SM_USER_Q', 'USP_SM_USERAUTH_Q';
GO
EXEC sp_rename 'USP_SM_USER_Q_1', 'USP_SM_USERAUTH_Q_1';
GO
EXEC sp_rename 'USP_SM_USER_S', 'USP_SM_USERAUTH_S';
GO
EXEC sp_rename 'USP_SM_USER_S_1', 'USP_SM_USERAUTH_S_1';
GO
EXEC sp_rename 'USP_SM_USER_S_2', 'USP_SM_USERAUTH_S_2';
GO

SELECT name FROM sys.objects WHERE type = 'P' AND name LIKE 'USP_SM_USERAUTH%' ORDER BY name;
GO
