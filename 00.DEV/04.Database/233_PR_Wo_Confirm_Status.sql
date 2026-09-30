-- 작업지시 확정 단계 추가 (2026-09-29): 계획(0) -> 확정(C) -> 진행(1, 첫 실적 확정 시 자동) -> 완료(E) / 중단(X).
--  * 확정 전에는 공정실적/외주이전을 등록할 수 없고, 실적 대기 LOT / 이전 대상 LOT 불러오기에도 나오지 않는다(228번 pick 프로시저가 C/1만 본다).
--  * 상태 처리는 USP_PR_WO_C_S(227번)의 C/CC, 실적/이전 차단은 226번 프로시저들이 한다. 이 파일은 공통코드 PR0001에 '확정'을 넣고 정렬만 바로잡는다.
--  * 이미 만들어 둔 계획(0) 작업지시는 작업지시 화면에서 확정 버튼을 눌러야 실적을 등록할 수 있다.
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'PR0001' AND minor_cd = 'C')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('PR0001', 'C', N'확정', 2, 'Y', 'Y', 'SYSTEM', GETDATE());

UPDATE TSMMINOR SET sort = CASE minor_cd WHEN '0' THEN 1 WHEN 'C' THEN 2 WHEN '1' THEN 3 WHEN 'E' THEN 4 WHEN 'X' THEN 5 ELSE sort END
WHERE major_cd = 'PR0001';
GO
