-- AI Builder(frmAIBuilder)의 Query Sources/컬럼 미리보기 그리드 "Control" 컬럼을 하드코딩된
-- RepositoryItemComboBox(cboEditControlKind) 대신 LookUpColumnEdit(LookupKey)로 바꾸기 위한
-- 고정값 LookUp. 코드=표시값이 같은 4개(TEXT/CHECK/NUMBER/COMBO)뿐이라 프로시저 없이
-- 쿼리소스(source_type='Q', 064_Lookup_Query_Source.sql)로 등록한다.
INSERT INTO sysLookupM (lookup_key, source_type, proc_nm, query_txt, lookup_nm, value_field, display_field, use_yn, remark)
SELECT 'L_SYS_CTRLKIND', 'Q', NULL,
    N'SELECT ''TEXT'' AS ctrl_kind UNION ALL SELECT ''CHECK'' UNION ALL SELECT ''NUMBER'' UNION ALL SELECT ''COMBO''',
    N'AI Builder 컨트롤종류', 'ctrl_kind', 'ctrl_kind', 'Y', N'AI Builder Query Sources 미리보기 그리드의 Control 컬럼용'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'L_SYS_CTRLKIND');
GO
