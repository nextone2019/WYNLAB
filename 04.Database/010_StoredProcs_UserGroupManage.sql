/* =========================================================
   사용자그룹관리(SM_USERGRP) 화면 전용 프로시저
   ========================================================= */

CREATE OR ALTER PROCEDURE USP_SM_USERGRP_Q
    @UserGrpNm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT G.USER_GRP_CD AS UserGrpCd, G.USER_GRP_NM AS UserGrpNm, G.DESCRIPTION AS Description,
           G.SORT_ORDER AS SortOrder, G.USE_YN AS UseYn,
           (SELECT COUNT(1) FROM TSMUSERGRPMAP M WHERE M.USER_GRP_CD = G.USER_GRP_CD) AS MemberCount
    FROM TSMUSERGRP G
    WHERE (@UserGrpNm IS NULL OR G.USER_GRP_NM LIKE '%' + @UserGrpNm + '%')
    ORDER BY G.SORT_ORDER;
END
GO

CREATE OR ALTER PROCEDURE USP_SM_USERGRP_Q_1
    @UserGrpCd VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(1) FROM TSMUSERGRP WHERE USER_GRP_CD = @UserGrpCd;
END
GO

/* 이 그룹의 소속 배정 화면용 - 전체 사용자(사용중인) + 소속여부 */
CREATE OR ALTER PROCEDURE USP_SM_USERGRP_Q_2
    @UserGrpCd VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, D.DEPT_NM AS DeptNm,
           CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
    FROM TSMUSER U
    LEFT JOIN TBADEPT D ON D.DEPT_CD = U.DEPT_CD
    LEFT JOIN TSMUSERGRPMAP M ON M.USER_ID = U.USER_ID AND M.USER_GRP_CD = @UserGrpCd
    WHERE U.USE_YN = 'Y'
    ORDER BY U.USER_NM;
END
GO

/* 신규등록/수정 겸용 - @Mode = 'C'(신규) / 'U'(수정) */
CREATE OR ALTER PROCEDURE USP_SM_USERGRP_S
    @Mode CHAR(1),
    @UserGrpCd VARCHAR(20),
    @UserGrpNm NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @SortOrder INT = 0,
    @UseYn CHAR(1) = 'Y'
AS
BEGIN
    SET NOCOUNT ON;

    IF @Mode = 'C'
    BEGIN
        INSERT INTO TSMUSERGRP (USER_GRP_CD, USER_GRP_NM, DESCRIPTION, SORT_ORDER, USE_YN)
        VALUES (@UserGrpCd, @UserGrpNm, @Description, @SortOrder, 'Y');
    END
    ELSE
    BEGIN
        UPDATE TSMUSERGRP SET
            USER_GRP_NM = @UserGrpNm, DESCRIPTION = @Description, SORT_ORDER = @SortOrder, USE_YN = @UseYn
        WHERE USER_GRP_CD = @UserGrpCd;
    END
END
GO

CREATE OR ALTER PROCEDURE USP_SM_USERGRP_S_1
    @UserGrpCd VARCHAR(20),
    @UseYn CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TSMUSERGRP SET USE_YN = @UseYn WHERE USER_GRP_CD = @UserGrpCd;
END
GO

/* 이 그룹의 소속 사용자를 체크된 목록으로 치환 - @UserIds는 콤마구분 문자열 */
CREATE OR ALTER PROCEDURE USP_SM_USERGRP_S_2
    @UserGrpCd VARCHAR(20),
    @UserIds NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM TSMUSERGRPMAP WHERE USER_GRP_CD = @UserGrpCd;

    IF @UserIds IS NOT NULL AND LEN(@UserIds) > 0
    BEGIN
        INSERT INTO TSMUSERGRPMAP (USER_GRP_CD, USER_ID)
        SELECT @UserGrpCd, value FROM STRING_SPLIT(@UserIds, ',');
    END
END
GO
