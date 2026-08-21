/* =========================================================
   USP_SM_GetUserSession
   로그인시 세션에 담을 사용자 정보를 한 번에 조회하는 프로시저.

   결과셋 1개 : 사용자 기본정보 (부서명 조인 포함)
   결과셋 2개 : 사용자가 속한 모든 사용자그룹 코드 목록 (다대다)

   호출 위치 : WYNLAB.Api > UserRepository.GetSessionAsync()
   ========================================================= */
CREATE OR ALTER PROCEDURE USP_SM_GetUserSession
    @USER_ID VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- 결과셋 1: 사용자 기본정보 (세션의 뼈대가 되는 값들)
    -- 컬럼 별칭은 WYNLAB.Api의 UserRow(C#) 프로퍼티명과 1:1로 맞춤 (Dapper 자동매핑용)
    SELECT
        U.USER_ID       AS UserId,
        U.USER_NM       AS UserNm,
        U.EMP_NO        AS EmpNo,
        U.DEPT_CD       AS DeptCd,
        D.DEPT_NM       AS DeptNm,
        U.POSITION_NM   AS PositionNm,
        U.EMAIL         AS Email,
        U.MOBILE_NO     AS MobileNo,
        U.PASSWORD_HASH AS PasswordHash,  -- 인증(BCrypt 검증)에만 사용, 세션에는 절대 담지 않음
        U.USE_YN        AS UseYn,
        U.IS_ADMIN_YN   AS IsAdminYn,
        U.PWD_FAIL_CNT  AS PwdFailCnt
    FROM TSMUSER U
    LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
    WHERE U.USER_ID = @USER_ID;

    -- 결과셋 2: 소속 사용자그룹 목록 (메뉴권한 병합에 사용)
    SELECT USER_GRP_CD
    FROM TSMUSERGRPMAP
    WHERE USER_ID = @USER_ID;
END
GO
