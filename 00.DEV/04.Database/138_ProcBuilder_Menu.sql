-- Developer Tool(MENU_ID=39) 산하에 "프로시저 빌더" 서브그룹과 그 아래 frmProcBuilder 화면을
-- 등록한다 - AI Builder(068_AI_Builder_Menu.sql)와 같은 2단계 구조(그룹->화면)를 그대로
-- 따른다(Developer Tool 직속 자식은 전부 GROUP이라는 기존 관례 - AI Builder/Component관리/
-- Configuration 확인함).
--
-- 068번은 그 시점의 옛 스키마(MENU_CD/UPPER_MENU_CD/FORM_CLASS_NM)로 쓰여서 그대로 못 베낀다 -
-- 지금은 MENU_ID(IDENTITY)/UPPER_MENU_ID/MODULE+SCREEN_CLASS_NM 구조다(041_TSMUSER_Restructure류
-- 리팩터링 이후). Developer Tool의 실제 MENU_ID(39)를 이름으로 찾아서 참조한다 - 환경마다
-- IDENTITY 채번값이 다를 수 있으므로 39를 그대로 박아넣지 않는다.

DECLARE @devToolMenuId BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE MENU_NM = N'Developer Tool' AND UPPER_MENU_ID IS NULL);
DECLARE @procBuilderGroupId BIGINT;

IF @devToolMenuId IS NULL
BEGIN
    PRINT 'Developer Tool 메뉴를 찾을 수 없어 건너뜁니다.';
    RETURN;
END

IF EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SYS' AND SCREEN_CLASS_NM = 'frmProcBuilder')
BEGIN
    PRINT 'frmProcBuilder 메뉴가 이미 있어 건너뜁니다.';
    RETURN;
END

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
VALUES (N'프로시저 빌더', @devToolMenuId, 2, N'GROUP', 50, N'Y', 'system', GETDATE());
SET @procBuilderGroupId = SCOPE_IDENTITY();

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
VALUES (N'프로시저 빌더', @procBuilderGroupId, 3, N'FORM', N'SYS', N'frmProcBuilder', 10, N'Y', 'system', GETDATE());
GO
