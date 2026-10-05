-- L_YN(예/아니오) 드롭다운 컬럼 구성 (2026-10-05, WYNLAB_DEV 전용): 명칭을 앞, 코드를 뒤로.
-- 컬럼 구성(sysLookupC)이 없는 룩업은 기본값(코드 | 명칭)으로 그려진다 - 이 룩업만 순서를 바꾼다. 여러 번 실행해도 안전하다.
DELETE FROM sysLookupC WHERE lookup_key = 'L_YN';
INSERT INTO sysLookupC (lookup_key, column_nm, caption, sort, width, visible_yn)
VALUES ('L_YN', 'yn_nm', N'명칭', 1, 90, 'Y'),
       ('L_YN', 'yn_cd', N'코드', 2, 50, 'Y');
GO
