/* =========================================================
   메뉴관리(SM_MENU) 화면 전용 프로시저
   ========================================================= */

CREATE OR ALTER PROCEDURE USP_SM_MENU_Q
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MENU_CD AS MenuCd, MENU_NM AS MenuNm, UPPER_MENU_CD AS UpperMenuCd,
           MENU_LEVEL AS MenuLevel, MENU_TYPE AS MenuType, FORM_CLASS_NM AS FormClassNm,
           ICON_NM AS IconNm, SORT_ORDER AS SortOrder, USE_YN AS UseYn
    FROM TSMMENU
    ORDER BY UPPER_MENU_CD, SORT_ORDER;
END
GO

CREATE OR ALTER PROCEDURE USP_SM_MENU_Q_1
    @MenuCd VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(1) FROM TSMMENU WHERE MENU_CD = @MenuCd;
END
GO

/* 신규등록/수정 겸용 - @Mode = 'C'(신규) / 'U'(수정) */
CREATE OR ALTER PROCEDURE USP_SM_MENU_S
    @Mode CHAR(1),
    @MenuCd VARCHAR(20),
    @MenuNm NVARCHAR(100),
    @UpperMenuCd VARCHAR(20) = NULL,
    @MenuLevel INT = 1,
    @MenuType VARCHAR(10) = 'FORM',
    @FormClassNm VARCHAR(200) = NULL,
    @IconNm VARCHAR(50) = NULL,
    @SortOrder INT = 0,
    @UseYn CHAR(1) = 'Y'
AS
BEGIN
    SET NOCOUNT ON;

    IF @Mode = 'C'
    BEGIN
        INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN)
        VALUES (@MenuCd, @MenuNm, @UpperMenuCd, @MenuLevel, @MenuType, @FormClassNm, @IconNm, @SortOrder, 'Y');
    END
    ELSE
    BEGIN
        UPDATE TSMMENU SET
            MENU_NM = @MenuNm, UPPER_MENU_CD = @UpperMenuCd, MENU_LEVEL = @MenuLevel,
            MENU_TYPE = @MenuType, FORM_CLASS_NM = @FormClassNm, ICON_NM = @IconNm,
            SORT_ORDER = @SortOrder, USE_YN = @UseYn
        WHERE MENU_CD = @MenuCd;
    END
END
GO

CREATE OR ALTER PROCEDURE USP_SM_MENU_S_1
    @MenuCd VARCHAR(20),
    @UseYn CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TSMMENU SET USE_YN = @UseYn WHERE MENU_CD = @MenuCd;
END
GO
