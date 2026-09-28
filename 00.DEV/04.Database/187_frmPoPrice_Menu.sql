-- 구매단가등록(frmPoPrice) 메뉴 등록 - 자재관리(10) > 자재기준관리(11) 밑(2026-09-25).
-- 품목/거래처/기간별 구매단가는 발주 때 참조하는 기준정보라서 구매발주관리(12)가 아니라 기준관리 그룹에 둔다.
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'MA' AND SCREEN_CLASS_NM = 'frmPoPrice')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'구매단가등록', 11, 3, 'FORM', 'MA', 'frmPoPrice', 'USP_MA_', 10, 'Y', SUSER_SNAME(), GETDATE());
END
GO
