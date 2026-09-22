-- AI Builder(frmAIBuilder)의 Control 컬럼 LookUp(L_SYS_CTRLKIND, 103_AIBuilder_ControlKind_Lookup.sql)에
-- DTE(DateEdit)/POP(PopupLookUp)를 추가한다 - ScreenTemplateGenerator.cs가 2026-09-07에 이 두
-- ControlKind를 실제로 처리하도록 확장됐다(NUMBER는 SpinEditWyn, DTE는 DateEditWyn, POP는
-- PopupLookupEditWyn으로 생성).
UPDATE sysLookupM
SET query_txt = N'SELECT ''TEXT'' AS ctrl_kind UNION ALL SELECT ''CHECK'' UNION ALL SELECT ''NUMBER'' UNION ALL SELECT ''COMBO'' UNION ALL SELECT ''DTE'' UNION ALL SELECT ''POP'''
WHERE lookup_key = 'L_SYS_CTRLKIND';
GO
