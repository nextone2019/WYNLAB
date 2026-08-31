-- P_EMP 팝업(sysPopUpM) 설정 버그 수정 - key_field/display_field가 둘 다 빈 문자열로 등록돼
-- 있었다(P_DEPT는 dept_cd/dept_nm로 정상 등록됨과 비교해서 확인). PopupLookupForm.Accept()가
-- 선택된 행에서 이 두 컬럼명으로 code/display 값을 꺼내는데, key_field가 빈 문자열이면 그
-- 이름의 컬럼이 있을 리 없어 code가 항상 비어서 "선택할 행이 없다"로 판단해 조용히 아무 일도
-- 안 하고 리턴한다 - 더블클릭도 선택 버튼도 반응이 없던 증상의 원인.

UPDATE sysPopUpM
SET key_field = 'emp_no', display_field = 'emp_nm'
WHERE popup_key = 'P_EMP';
GO
