-- AI Builder(frmAIBuilder)의 Control 선택 컬럼이 하드코딩된 SQL 리터럴 LookUp(L_SYS_CTRLKIND,
-- 103/104/115 마이그레이션에서 UNION ALL SELECT로 박아둔 것)이 아니라, 기초코드(TSMMINOR,
-- frmMinorCode에서 관리자가 직접 UI로 관리 가능)에 이미 등록돼 있던 L_SM0005("CONTROL구분")를
-- 쓰도록 바꾼다(2026-09-09 요청).
--
-- 주의: ScreenTemplateGenerator.cs/frmAIBuilder.cs가 ControlKind 값을 "TEXT"/"CHECK"/"NUMBER"/
-- "COMBO"/"DTE"/"POP"/"MEMO" 문자열 그대로 비교(c.ControlKind == "CHECK" 등)하므로, L_SM0005의
-- minor_cd도 정확히 이 값이어야 한다 - 기존 TSMMINOR(SM0005)엔 CBO/CHK/NUM/TXT처럼 축약된
-- 코드로 등록돼 있어서 그대로 붙이면 컨트롤 종류 인식이 조용히 깨진다. 정확한 문자열로 맞추고,
-- L_SYS_CTRLKIND에는 있었지만 SM0005엔 없던 MEMO도 추가한다.
UPDATE TSMMINOR SET minor_cd = 'COMBO' WHERE major_cd = 'SM0005' AND minor_cd = 'CBO';
UPDATE TSMMINOR SET minor_cd = 'CHECK' WHERE major_cd = 'SM0005' AND minor_cd = 'CHK';
UPDATE TSMMINOR SET minor_cd = 'NUMBER' WHERE major_cd = 'SM0005' AND minor_cd = 'NUM';
UPDATE TSMMINOR SET minor_cd = 'TEXT' WHERE major_cd = 'SM0005' AND minor_cd = 'TXT';
-- DTE/POP은 이미 코드값이 일치하므로 그대로 둔다.

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'SM0005' AND minor_cd = 'MEMO')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, use_yn, reg_user_id, reg_dt)
VALUES ('SM0005', 'MEMO', N'MemoEdit(여러줄)', 7, 'Y', SUSER_SNAME(), GETDATE());

-- L_SYS_CTRLKIND는 이제 아무 화면도 참조하지 않으므로(frmAIBuilder.cs/Designer.cs도 이 마이그레이션과
-- 함께 L_SM0005로 바뀜) 정리한다.
DELETE FROM sysLookupC WHERE lookup_key = 'L_SYS_CTRLKIND';
DELETE FROM sysLookupM WHERE lookup_key = 'L_SYS_CTRLKIND';
GO
