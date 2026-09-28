-- frmUserAuth(사용자권한관리) grd1(사용자 LIST)에 사업장(ACC_ID)/마지막 비밀번호 변경일자를
-- 노출하기 위한 조회 프로시저 확장 (2026-09-22). ACC_ID/ACC_NM은 이미 SELECT되고 있었고
-- panData(cboAccCd, L_ACC)에도 이미 있었다 - 그리드에만 컬럼이 빠져 있었다. LAST_PWD_CHANGE_DATE도
-- TSMUSER에 이미 있고 USP_SM_USERAUTH_S_3/USP_SM_PWDRESET_S(work_type='C') 둘 다 비밀번호를 바꿀
-- 때마다 이미 GETDATE()로 채우고 있었다 - 조회 프로시저가 그 컬럼을 안 돌려주고 있었을 뿐이다.
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
                        U.EMP_ID AS EmpId,
                        E.emp_no AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        D.DEPT_NM AS DeptNm,
                        U.ACC_ID AS AccId,
                        A.ACC_NM AS AccNm,
                        U.EMAIL AS Email,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt,
                        U.LAST_PWD_CHANGE_DATE AS LastPwdChangeDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_ID = U.EMP_ID
                        LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
                        LEFT JOIN TBAACC A ON A.ACC_ID = U.ACC_ID
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
