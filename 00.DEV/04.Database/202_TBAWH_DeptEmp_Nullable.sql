-- 창고등록(frmWh)의 담당부서/담당자는 필수가 아니다(Required=false) - 필수 여부는 화면 컨트롤의 Required 속성 하나로만
-- 정한다(BaseForm 필수입력 검사 규칙, 2026-09-25). 그런데 TBAWH.dept_id/emp_id가 NOT NULL이라 비우고 저장하면 DB 오류가 나서,
-- 화면이 임의로 "담당부서와 담당자를 입력하세요"를 띄우고 있었다. 컬럼을 NULL 허용으로 바꿔 두 규칙을 일치시킨다.
-- (조회/저장 프로시저는 LEFT JOIN + 파라미터 NULL 기본값이라 그대로 동작한다.)

ALTER TABLE TBAWH ALTER COLUMN dept_id BIGINT NULL;
GO
ALTER TABLE TBAWH ALTER COLUMN emp_id BIGINT NULL;
GO
