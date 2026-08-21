/* =========================================================
   로그인시 메뉴권한 병합(AuthService)에서 쓰는 조회 프로시저
   ========================================================= */

/* 로그인 후 좌측 메뉴 구성용 - 사용중(USE_YN='Y')인 메뉴 전체. MENU 모듈의 확장 조회라 _Q_2로 명명 */
CREATE OR ALTER PROCEDURE USP_SM_MENU_Q_2
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MENU_CD AS MenuCd, MENU_NM AS MenuNm, UPPER_MENU_CD AS UpperMenuCd,
           MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, FORM_CLASS_NM AS FormClassNm,
           ICON_NM AS IconNm, SORT_ORDER AS SortOrder
    FROM TSMMENU
    WHERE USE_YN = 'Y'
    ORDER BY SORT_ORDER;
END
GO

/* 사용자 본인(USER) + 소속그룹 전체(GRP)에 걸린 권한행을 한 번에 조회 - MenuPermissionMerger가 OR 합산 */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q
    @UserId VARCHAR(20),
    @GroupCodes NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MENU_CD AS MenuCd, AUTH_TARGET_TYPE AS AuthTargetType, AUTH_TARGET_CD AS AuthTargetCd,
           VIEW_YN AS ViewYn, INSERT_YN AS InsertYn, UPDATE_YN AS UpdateYn,
           DELETE_YN AS DeleteYn, EXCEL_YN AS ExcelYn
    FROM TSMMENUAUTH
    WHERE (AUTH_TARGET_TYPE = 'USER' AND AUTH_TARGET_CD = @UserId)
       OR (AUTH_TARGET_TYPE = 'GRP' AND @GroupCodes IS NOT NULL
           AND AUTH_TARGET_CD IN (SELECT value FROM STRING_SPLIT(@GroupCodes, ',')));
END
GO
