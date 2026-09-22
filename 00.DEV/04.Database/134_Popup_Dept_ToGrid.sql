-- 부서팝업(P_DEPT)을 트리에서 그리드로 변경(2026-09-12 요청 - "사원정보팝업처럼 그리드
-- 형태로"). sysPopUpM.hierarchical_yn만 popPopUp.cs(WYNLAB.Popup)의 트리/그리드 분기를
-- 결정하고(popPopUp.cs 308행 등 참고), parent_field는 hierarchical_yn='N'일 때 아예 안 읽으므로
-- 굳이 안 지워도 동작에는 지장 없지만, P_EMP(NULL)와 형태를 맞춰 헷갈리지 않게 같이 비운다.
-- SSP_POP_DEPT_Q/sysPopUpD(표시 컬럼 dept_nm/dept_type)는 이미 그리드로 써도 그대로 맞는
-- 컬럼을 반환하므로 프로시저/컬럼정의는 손대지 않는다.

UPDATE sysPopUpM
SET hierarchical_yn = 'N',
    parent_field = NULL
WHERE popup_key = 'P_DEPT';
GO
