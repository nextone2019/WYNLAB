/* =========================================================
   로그인 처리(AuthService/UserRepository)에서 쓰는 나머지 쓰기 프로시저.
   조회(USP_SM_GetUserSession)는 이미 003에서 프로시저로 만들어져 있었음 - 그대로 유지.
   ========================================================= */

CREATE OR ALTER PROCEDURE USP_SM_LOGIN_S
    @UserId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TSMUSER
    SET LAST_LOGIN_DT = GETDATE(), PWD_FAIL_CNT = 0
    WHERE USER_ID = @UserId;
END
GO

CREATE OR ALTER PROCEDURE USP_SM_LOGIN_S_1
    @UserId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TSMUSER
    SET PWD_FAIL_CNT = PWD_FAIL_CNT + 1
    WHERE USER_ID = @UserId;
END
GO

CREATE OR ALTER PROCEDURE USP_SM_LOGIN_S_2
    @UserId VARCHAR(20),
    @ClientIp VARCHAR(50),
    @ClientVersion VARCHAR(100),
    @ResultCd VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO TSMLOGINHIST (USER_ID, CLIENT_IP, CLIENT_VERSION, RESULT_CD)
    VALUES (@UserId, @ClientIp, @ClientVersion, @ResultCd);
END
GO
