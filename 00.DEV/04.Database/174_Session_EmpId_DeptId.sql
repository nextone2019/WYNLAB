-- 로그인 세션에 EmpId/DeptId 추가(2026-09-22) - 지금까지는 EmpNo/DeptNm(표시용 문자열)만
-- 내려줬는데, 구매요청등록처럼 신규 진입 시 담당자/부서 FK를 세션값으로 바로 채워야 하는
-- 화면이 생겨서 원본 ID도 같이 내려준다. TSMUSER.ACC_ID를 세션에 추가했을 때(114번)와 같은 이유.
CREATE OR ALTER PROCEDURE SSP_WYNLAB_GetSession
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                U.USER_ID       AS UserId,
                U.USER_NM       AS UserNm,
                E.emp_no        AS EmpNo,
                U.EMP_ID        AS EmpId,
                D.DEPT_NM       AS DeptNm,
                E.DEPT_ID       AS DeptId,
                U.PASSWORD_HASH AS PasswordHash,
                U.EMAIL         AS Email,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn,
                U.ACC_ID        AS AccId,
                A.ACC_NM        AS AccNm
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            LEFT JOIN TBAACC A ON A.ACC_ID = U.ACC_ID
            WHERE U.USER_ID = @p_user_id;

            SELECT USER_GRP_CD
            FROM TSMUSERGRPMAP
            WHERE USER_ID = @p_user_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
