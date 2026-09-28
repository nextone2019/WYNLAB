-- 홈화면 결재 위젯 개선(2026-09-29):
-- 1) 기안함(Q6) 목록에 "이 문서를 지금 승인해야 할 사람"(cur_appr_emp_nm)을 추가한다 - Q1과 같은
--    "다음 대기 결재라인" 판정(앞선 결재자가 전부 승인해야 내 차례)을 재사용해서 OUTER APPLY로
--    문서당 1명만 뽑는다. 승인완료/반려 등 더 이상 대기 라인이 없으면 NULL(화면에서 "-"로 표시).
-- 2) 구분(doc_type) 코드를 화면에서 이름으로 보여주기 위한 LookUp 'L_AP0002'를 등록한다
--    (TSMMINOR AP0002, sysLookupM.source_type='Q' - major_cd가 고정값이라 파라미터가 필요 없다).
--
-- 라이브 정의를 통째로 다시 쓰지 않고, Q6 SELECT 블록만 REPLACE로 바꾼다(180/208번과 같은 이유 -
-- 그 사이 다른 손을 안 탔는지 CHARINDEX로 확인 후에만 실행, 이미 반영되어 있으면 스킵).

DECLARE @def NVARCHAR(MAX) = OBJECT_DEFINITION(OBJECT_ID('dbo.USP_AP_APPR_Q'));
DECLARE @old NVARCHAR(MAX) = N'            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   a.end_yn, a.rtn_yn, a.app_stat_cd AS stat_cd
            FROM TAPDOC a
            WHERE a.emp_id = @my_emp_id2
            ORDER BY a.app_id DESC;';
DECLARE @new NVARCHAR(MAX) = N'            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no, a.form_id,
                   a.end_yn, a.rtn_yn, a.app_stat_cd AS stat_cd, ca.emp_nm AS cur_appr_emp_nm
            FROM TAPDOC a
            OUTER APPLY (
                SELECT TOP 1 e.emp_nm
                FROM TAPDOCPATH p
                LEFT JOIN TBAEMP e ON e.EMP_ID = p.emp_id
                WHERE p.app_id = a.app_id AND p.path_type = ''A'' AND p.stat_cd = ''N''
                  AND NOT EXISTS (
                      SELECT 1 FROM TAPDOCPATH q
                      WHERE q.app_id = p.app_id AND q.path_type = ''A'' AND q.sort < p.sort AND q.stat_cd <> ''Y''
                  )
                ORDER BY p.sort
            ) ca
            WHERE a.emp_id = @my_emp_id2
            ORDER BY a.app_id DESC;';

IF @def IS NOT NULL AND CHARINDEX(@old, @def) > 0
BEGIN
    SET @def = REPLACE(@def, @old, @new);
    DECLARE @pos INT = PATINDEX('%CREATE%PROCEDURE%', @def);
    SET @def = STUFF(@def, @pos, LEN(N'CREATE'), N'ALTER');
    EXEC (@def);
    PRINT N'USP_AP_APPR_Q: Q6 cur_appr_emp_nm 추가 완료';
END
ELSE
    PRINT N'USP_AP_APPR_Q: 변경할 부분이 없음(이미 반영됐거나 정의가 달라짐 - 확인 필요)';
GO

IF NOT EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'L_AP0002')
BEGIN
    INSERT INTO sysLookupM (lookup_key, source_type, query_txt, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt)
    VALUES (
        'L_AP0002', 'Q',
        N'SELECT minor_cd, minor_nm FROM TSMMINOR WHERE major_cd = ''AP0002'' AND use_yn = ''Y'' ORDER BY sort',
        N'문서유형(전자결재)', 'minor_cd', 'minor_nm', 'Y', N'홈화면 결재 위젯 구분 배지 + 향후 문서유형 콤보용',
        'SYSTEM', GETDATE()
    );
    PRINT N'L_AP0002 LookUp 등록 완료';
END
ELSE
    PRINT N'L_AP0002 이미 등록되어 있음';
GO
