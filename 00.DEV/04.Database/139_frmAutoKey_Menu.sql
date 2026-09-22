-- AI Builder가 자동 생성 - frmAutoKey 화면 메뉴 등록.
-- 적용 전 검토 필요(다른 마이그레이션과 동일한 규칙) - 자동 실행되지 않는다.

IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'SM' AND SCREEN_CLASS_NM = 'frmAutoKey')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'자동채번등록', 30, 3, 'FORM', 'SM', 'frmAutoKey', 'USP_SM_AUTOKEY_', 10, 'Y', SUSER_SNAME(), GETDATE());
END
GO
