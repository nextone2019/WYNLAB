-- 모듈 개명(사장님 지시, 2026-09-11) - "그룹웨어"로 시작했던 WYNLAB.GW 모듈을 결재 프로세스
-- 전용으로 범위를 좁히면서 WYNLAB.AP로 이름을 바꾼다(그룹웨어 전반을 다 만들 계획은 없음).
-- 어셈블리/네임스페이스는 99.SOURCE\GW\WYNLAB.GW -> 99.SOURCE\AP\WYNLAB.AP로 이미 바꿨고, 이
-- 파일은 그에 맞춰 DB 쪽(TSMMENU.MODULE, 최상위 메뉴그룹명/순서, 기존 테스트 결재건의
-- TAPDOC.form_id)을 맞춘다.

-- 1) 메뉴의 MODULE 값 - ShellForm.OpenMenu가 "WYNLAB.{MODULE}.{SCREEN_CLASS_NM}, WYNLAB.{MODULE}"로
--    조립하므로, 어셈블리 이름이 바뀌면 이것도 같이 바뀌어야 메뉴가 열린다.
UPDATE TSMMENU SET MODULE = 'AP' WHERE MODULE = 'GW';

-- 2) 최상위 메뉴그룹 "그룹웨어" -> "결재관리"로 이름 변경 + 시스템운영관리(SORT_ORDER=10)와
--    기준정보관리(SORT_ORDER=15) 사이로 순서 이동.
UPDATE TSMMENU SET MENU_NM = N'결재관리', SORT_ORDER = 12
WHERE MENU_LEVEL = 1 AND MENU_TYPE = 'GROUP' AND MENU_NM = N'그룹웨어';

-- 3) 이미 상신해둔 테스트 결재건(TAPDOC.form_id='GW.frmNameCardReq', 2건)도 같이 맞춘다 -
--    안 바꾸면 결재함에서 그 건을 더블클릭했을 때 "GW" 모듈을 찾다가 원본 문서로 못 돌아간다
--    (frmApprInbox.OpenOriginalDocumentAsync 참고).
UPDATE TAPDOC SET form_id = 'AP.frmNameCardReq' WHERE form_id = 'GW.frmNameCardReq';
GO
