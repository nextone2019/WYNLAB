-- 로그인 시 세션 정보(사용자 기본정보 + 소속그룹 목록)를 조회하던 프로시저명을
-- USP_SM_GetUserSession -> SSP_WYNLAB_GetSession 으로 변경.
-- 로직 자체는 그대로이고 이름만 바꾸는 거라 CREATE OR ALTER가 아니라 sp_rename을 쓴다.
-- API 쪽(UserRepository.GetSessionAsync)도 이 이름으로 같이 바꿔서 배포해야 한다 -
-- 순서가 어긋나면(DB만 먼저 바뀌거나 API만 먼저 배포되면) 그 사이 로그인이 실패한다.
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'USP_SM_GetUserSession')
BEGIN
    EXEC sp_rename 'USP_SM_GetUserSession', 'SSP_WYNLAB_GetSession';
END
