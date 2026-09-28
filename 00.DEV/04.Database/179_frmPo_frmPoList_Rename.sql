-- 화면명 변경: frmPoOrder -> frmPo, frmPoOrderList -> frmPoList (2026-09-22, 사장님 지시).
-- 프로시저는 이미 USP_MA_PO_*/USP_MA_POLIST_Q로 이 이름 기준에 맞춰 만들어놔서 변경할 게 없다 -
-- TSMMENU.SCREEN_CLASS_NM만 새 클래스명으로 맞춘다.
UPDATE TSMMENU SET SCREEN_CLASS_NM = 'frmPo' WHERE MODULE = 'MA' AND SCREEN_CLASS_NM = 'frmPoOrder';
UPDATE TSMMENU SET SCREEN_CLASS_NM = 'frmPoList' WHERE MODULE = 'MA' AND SCREEN_CLASS_NM = 'frmPoOrderList';
GO
