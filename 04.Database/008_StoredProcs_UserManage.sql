/* =========================================================
   사용자관리(SM_USER) 화면 전용 프로시저
   명명규칙: USP_{모듈}_{폼}_{Q=조회/S=저장}[_순번]
   ========================================================= */

CREATE OR ALTER PROCEDURE USP_SM_USER_Q
    @UserId VARCHAR(20) = NULL,
    @UserNm NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, U.EMP_NO AS EmpNo,
           U.DEPT_CD AS DeptCd, D.DEPT_NM AS DeptNm, U.POSITION_NM AS PositionNm,
           U.EMAIL AS Email, U.MOBILE_NO AS MobileNo, U.USE_YN AS UseYn,
           U.IS_ADMIN_YN AS IsAdminYn, U.LAST_LOGIN_DT AS LastLoginDt
    FROM TSMUSER U
    LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
    WHERE (@UserId IS NULL OR U.USER_ID LIKE '%' + @UserId + '%')
      AND (@UserNm IS NULL OR U.USER_NM LIKE '%' + @UserNm + '%')
    ORDER BY U.REG_DT DESC;
END
GO

/* 아이디 중복확인용 - 신규등록 전 존재여부 체크 */
CREATE OR ALTER PROCEDURE USP_SM_USER_Q_1
    @UserId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(1) FROM TSMUSER WHERE USER_ID = @UserId;
END
GO

/* 신규등록/수정 겸용 - @Mode = 'C'(신규) / 'U'(수정) */
CREATE OR ALTER PROCEDURE USP_SM_USER_S
    @Mode CHAR(1),
    @UserId VARCHAR(20),
    @UserNm NVARCHAR(50),
    @PasswordHash VARCHAR(200) = NULL,
    @EmpNo VARCHAR(20) = NULL,
    @DeptCd VARCHAR(20) = NULL,
    @PositionNm NVARCHAR(30) = NULL,
    @Email VARCHAR(100) = NULL,
    @MobileNo VARCHAR(20) = NULL,
    @UseYn CHAR(1) = 'Y',
    @IsAdminYn CHAR(1) = 'N'
AS
BEGIN
    SET NOCOUNT ON;

    IF @Mode = 'C'
    BEGIN
        INSERT INTO TSMUSER (USER_ID, USER_NM, PASSWORD_HASH, EMP_NO, DEPT_CD, POSITION_NM, EMAIL, MOBILE_NO, IS_ADMIN_YN, USE_YN)
        VALUES (@UserId, @UserNm, @PasswordHash, @EmpNo, @DeptCd, @PositionNm, @Email, @MobileNo, @IsAdminYn, 'Y');
    END
    ELSE
    BEGIN
        UPDATE TSMUSER SET
            USER_NM = @UserNm, EMP_NO = @EmpNo, DEPT_CD = @DeptCd, POSITION_NM = @PositionNm,
            EMAIL = @Email, MOBILE_NO = @MobileNo, USE_YN = @UseYn, IS_ADMIN_YN = @IsAdminYn
        WHERE USER_ID = @UserId;
    END
END
GO

/* 사용중지 처리(물리삭제 대신) */
CREATE OR ALTER PROCEDURE USP_SM_USER_S_1
    @UserId VARCHAR(20),
    @UseYn CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TSMUSER SET USE_YN = @UseYn WHERE USER_ID = @UserId;
END
GO

/* 이 사용자가 속한 그룹 배정 화면용 - 전체 그룹 + 소속여부 */
CREATE OR ALTER PROCEDURE USP_SM_USER_Q_2
    @UserId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT G.USER_GRP_CD AS UserGrpCd, G.USER_GRP_NM AS UserGrpNm,
           CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
    FROM TSMUSERGRP G
    LEFT JOIN TSMUSERGRPMAP M ON M.USER_GRP_CD = G.USER_GRP_CD AND M.USER_ID = @UserId
    WHERE G.USE_YN = 'Y'
    ORDER BY G.SORT_ORDER;
END
GO

/* 이 사용자의 그룹소속을 체크된 목록으로 치환 - @UserGrpCds는 콤마구분 문자열 */
CREATE OR ALTER PROCEDURE USP_SM_USER_S_2
    @UserId VARCHAR(20),
    @UserGrpCds NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM TSMUSERGRPMAP WHERE USER_ID = @UserId;

    IF @UserGrpCds IS NOT NULL AND LEN(@UserGrpCds) > 0
    BEGIN
        INSERT INTO TSMUSERGRPMAP (USER_ID, USER_GRP_CD)
        SELECT @UserId, value FROM STRING_SPLIT(@UserGrpCds, ',');
    END
END
GO
