-- AI Builder(frmAIBuilder)의 Control 컬럼 LookUp(L_SYS_CTRLKIND)에 MEMO(여러 줄 입력, MemoEditWyn)를
-- 추가한다 - ScreenTemplateGenerator.cs가 이 ControlKind를 panData/검색조건 필드에서
-- MemoEditWyn으로 생성하도록 확장됐다(104_..._AddDteAndPop.sql과 같은 패턴).
UPDATE sysLookupM
SET query_txt = N'SELECT ''TEXT'' AS ctrl_kind UNION ALL SELECT ''CHECK'' UNION ALL SELECT ''NUMBER'' UNION ALL SELECT ''COMBO'' UNION ALL SELECT ''DTE'' UNION ALL SELECT ''POP'' UNION ALL SELECT ''MEMO'''
WHERE lookup_key = 'L_SYS_CTRLKIND';
GO
