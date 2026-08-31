-- DEPT 팝업 정의(sysPopUpM/D) 시드 데이터 - 아직 "팝업관리" 관리 화면이 없어서(다음 단계 작업)
-- 파일럿 검증 목적으로 직접 등록한다. 화면이 생기면 이 데이터를 화면에서 그대로 다시 만들 수
-- 있어야 하고(구조 검증용), 나중에 화면으로 등록한 값과 겹치지 않도록 이 스크립트는 재실행해도
-- 안전하게 덮어쓰도록 MERGE 대신 DELETE+INSERT로 둔다.

DELETE FROM sysPopUpD WHERE popup_key = 'DEPT';
DELETE FROM sysPopUpM WHERE popup_key = 'DEPT';

INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field, display_field, popup_width, popup_height, use_yn)
VALUES ('DEPT', 'SSP_POP_DEPT_Q', N'부서 조회', 'Y', 'dept_cd', 'par_dept_cd', 'dept_nm', 700, 500, 'Y');

INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, sort, width, visible_yn) VALUES
('DEPT', 'dept_cd',  N'부서코드', 'TEXT', 1, 100, 'Y'),
('DEPT', 'dept_nm',  N'부서명',   'TEXT', 2, 200, 'Y'),
('DEPT', 'dept_type', N'부서유형', 'TEXT', 3, 100, 'Y'),
('DEPT', 'par_dept_cd', N'상위부서코드', 'TEXT', 4, 100, 'N'); -- 트리 부모 연결용, 화면엔 안 보임
GO
