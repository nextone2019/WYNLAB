-- 로그인 세션에 사원명(EmpNm) 추가(2026-09-26) - 구매요청/발주/납품/검사/입고 화면이 신규 진입 때 담당자를 세션값으로 채우는데,
-- 담당자 ID는 세션의 사원(EmpId)이지만 이름은 사용자 이름(UserNm)을 넣고 있어서 저장 후 다시 조회하면 사원 이름으로 바뀌어 보였다.
-- 같은 원본(TBAEMP)에서 사원명도 내려준다. 라이브 정의(2026-09-26)에 emp_nm 컬럼 하나만 더했다.
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
                E.emp_nm        AS EmpNm,
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
