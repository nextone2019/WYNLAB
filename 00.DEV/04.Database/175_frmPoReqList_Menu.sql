-- 구매요청현황(frmPoReqList) 메뉴 등록 - 구매요청등록과 같은 그룹(자재관리(10) > 구매요청관리(13)) 밑.
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'MA' AND SCREEN_CLASS_NM = 'frmPoReqList')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'구매요청현황', 13, 3, 'FORM', 'MA', 'frmPoReqList', 'USP_MA_', 20, 'Y', SUSER_SNAME(), GETDATE());
END
GO
