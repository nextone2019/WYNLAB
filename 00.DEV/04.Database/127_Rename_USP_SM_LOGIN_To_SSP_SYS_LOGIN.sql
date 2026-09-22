-- 프로시저 네이밍 정리(사장님 지시, 2026-09-11) - 로그인 처리(UserRepository)가 쓰는
-- USP_SM_LOGIN_S/_S_1/_S_2 3개를 SSP_SYS_LOGIN_S/_S_1/_S_2로 개명한다. 원래 CREATE 마이그레이션이
-- 04.Database에 없다(126번과 마찬가지로 DB에 직접 만들어져 있던 것) - 이 파일이 그 개명 이력의
-- 시작이다. 메뉴 기반 PROC_PREFIX 화이트리스트(TSMMENU)와는 무관 - 이 3개는 화면이 아니라
-- AuthService/UserRepository가 서버 내부에서만 직접 호출한다(로그인 성공/실패 카운트 증가/
-- 로그인이력 INSERT).

EXEC sp_rename 'dbo.USP_SM_LOGIN_S', 'SSP_SYS_LOGIN_S', 'OBJECT';
EXEC sp_rename 'dbo.USP_SM_LOGIN_S_1', 'SSP_SYS_LOGIN_S_1', 'OBJECT';
EXEC sp_rename 'dbo.USP_SM_LOGIN_S_2', 'SSP_SYS_LOGIN_S_2', 'OBJECT';
GO
