-- AI Builder가 자동 생성 - frmItem333 화면 메뉴 등록.
-- 적용 전 검토 필요(다른 마이그레이션과 동일한 규칙) - 자동 실행되지 않는다.

IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'BA' AND SCREEN_CLASS_NM = 'frmItem333')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'품목정보등록', 9, 3, 'FORM', 'BA', 'frmItem333', 'USP_BA_', 10, 'Y', SUSER_SNAME(), GETDATE());
END
GO
