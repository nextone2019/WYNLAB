/* =========================================================
   권한부여관리(SM_AUTH) 화면 전용 프로시저 - 특정 대상(USER 1명 또는 GRP 1개)의
   메뉴별 권한을 조회/저장한다. MENUAUTH 모듈의 확장 조회/저장이라 _Q_1/_S_1로 명명
   (_Q, _S_2는 012에서 로그인 병합용으로 이미 사용중).
   ========================================================= */

/* 사용중(USE_YN='Y')인 전체 메뉴 + 이 대상에게 직접 걸린 권한(없으면 전부 N) */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q_1
    @TargetType VARCHAR(10),
    @TargetCd VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT M.MENU_CD AS MenuCd, M.MENU_NM AS MenuNm, M.UPPER_MENU_CD AS UpperMenuCd,
           M.MENU_TYPE AS MenuType, M.SORT_ORDER AS SortOrder,
           ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
           ISNULL(A.UPDATE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
           ISNULL(A.EXCEL_YN, 'N') AS ExcelYn
    FROM TSMMENU M
    LEFT JOIN TSMMENUAUTH A
        ON A.MENU_CD = M.MENU_CD AND A.AUTH_TARGET_TYPE = @TargetType AND A.AUTH_TARGET_CD = @TargetCd
    WHERE M.USE_YN = 'Y'
    ORDER BY M.SORT_ORDER;
END
GO

/* 이 대상의 권한을 화면에서 넘어온 목록으로 치환. @ItemsJson 예:
   [{"MenuCd":"SA_ORDER","ViewYn":"Y","InsertYn":"Y","UpdateYn":"N","DeleteYn":"N","ExcelYn":"Y"}, ...]
   다섯 항목이 전부 'N'인 행은 저장할 필요가 없는 행(=권한없음)이라 자동으로 걸러진다. */
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_S_1
    @TargetType VARCHAR(10),
    @TargetCd VARCHAR(20),
    @ItemsJson NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM TSMMENUAUTH WHERE AUTH_TARGET_TYPE = @TargetType AND AUTH_TARGET_CD = @TargetCd;

    INSERT INTO TSMMENUAUTH (MENU_CD, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, UPDATE_YN, DELETE_YN, EXCEL_YN)
    SELECT J.MenuCd, @TargetType, @TargetCd, J.ViewYn, J.InsertYn, J.UpdateYn, J.DeleteYn, J.ExcelYn
    FROM OPENJSON(@ItemsJson)
    WITH (
        MenuCd   VARCHAR(20) '$.MenuCd',
        ViewYn   CHAR(1)     '$.ViewYn',
        InsertYn CHAR(1)     '$.InsertYn',
        UpdateYn CHAR(1)     '$.UpdateYn',
        DeleteYn CHAR(1)     '$.DeleteYn',
        ExcelYn  CHAR(1)     '$.ExcelYn'
    ) J
    WHERE J.ViewYn = 'Y' OR J.InsertYn = 'Y' OR J.UpdateYn = 'Y' OR J.DeleteYn = 'Y' OR J.ExcelYn = 'Y';
END
GO
