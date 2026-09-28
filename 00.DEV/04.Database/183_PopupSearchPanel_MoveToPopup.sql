-- 전용 검색패널 샘플을 BA 모듈에서 WYNLAB.Popup 프로젝트로 옮김(2026-09-25, "WYNLAB.BA가 아니라
-- WYNLAB.Popup에 추가해줘") - 클래스 전체 이름이 WYNLAB.BA.pnlItemSearch에서 WYNLAB.Popup.pnlItemSearch로
-- 바뀌었다. WYNLAB.Popup은 CoreAssembly라 항상 로드돼 있어서, 패널이 든 모듈이 안 열려 있어 클래스를
-- 못 찾는 문제도 없어진다.
UPDATE sysPopUpM
   SET search_panel_class = 'WYNLAB.Popup.pnlItemSearch',
       remark = N'전용 검색패널 샘플 - Popup.pnlItemSearch'
 WHERE popup_key = 'P_ITEMPNL' AND search_panel_class = 'WYNLAB.BA.pnlItemSearch';
GO
