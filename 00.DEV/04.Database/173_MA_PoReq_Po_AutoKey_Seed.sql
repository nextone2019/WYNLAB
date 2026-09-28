-- 구매요청/구매발주 번호 채번을 자체 로직(날짜+순번 직접 계산) 대신 공용 SSP_SYS_GetAutoKey로
-- 바꿨다(2026-09-22, 사장님 지시) - USP_MA_POREQ_S/USP_MA_PO_S 수정은 170/171번 파일 자체를
-- 고쳤다(재실행하면 이 파일과 함께 최신 정의로 맞춰짐). SSP_SYS_GetAutoKey는 해당 table_name의
-- TSMAutoKey 설정이 없으면 첫 호출 때 prefix=''/date_type='YYMM'으로 자동 등록해버리므로,
-- 원하는 접두사(PR/PO)가 붙도록 먼저 설정을 심어둔다.
IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAPOREQM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAPOREQM', N'구매요청마스터', 'PR', 'req_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMAutoKey WHERE table_name = 'TMAPOM')
INSERT INTO TSMAutoKey (table_name, table_desc, pre_fix, key_col, date_type, seq_len, reg_user_id, reg_dt)
VALUES ('TMAPOM', N'구매발주마스터', 'PO', 'po_no', 'YYMM', 4, 'SYSTEM', GETDATE());
GO
