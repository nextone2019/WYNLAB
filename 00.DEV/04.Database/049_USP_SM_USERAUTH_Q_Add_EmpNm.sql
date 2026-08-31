-- USP_SM_USERAUTH_Q에 EMP_NM(사원명) 추가 - TBAEMP는 이미 JOIN되어 있었는데(DEPT_CD/DEPT_NM용)
-- EMP_NM만 SELECT 목록에 빠져 있었다. frmUserAuth가 이 값을 안 받으니 사용자 목록에서 행을
-- 바꿔도 사원명(txtEmpNm) 표시가 갱신될 방법이 없어 "모든 row를 클릭해도 사원명이 항상 같은
-- 값"으로 보이는 증상의 원인이었다(화면 쪽 EnterEditModeAsync가 안 채우는 문제와 같이 발생).

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_user_nm NVARCHAR(50) = NULL,
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
            SELECT  U.USER_ID AS UserId,
                        U.USER_NM AS UserNm,
                        U.EMP_NO AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        E.DEPT_CD AS DeptCd,
                        D.DEPT_NM AS DeptNm,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
                        LEFT JOIN TBADEPT D ON D.DEPT_CD = E.DEPT_CD
            WHERE (@p_user_id IS NULL OR U.USER_ID LIKE '%' + @p_user_id + '%')
              AND (@p_user_nm IS NULL OR U.USER_NM LIKE '%' + @p_user_nm + '%')
            ORDER BY U.REG_DT DESC;
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
