-- LookUp키 네이밍 규칙(P_/L_ 접두어, 사장님 지시) 확정 이전에 만들어진 MINOR_CODE를
-- L_MINOR_CODE로 맞춘다. lookup_key는 sysLookupM의 PK이자 sysLookupP의 FK라서, 039번
-- 마이그레이션(팝업키 P_ 접두어 정리)과 같은 방식으로 안전하게 옮긴다 - 새 키로 부모 행을
-- 먼저 복제 -> 자식 행들을 새 키로 옮김 -> 옛 부모 행 삭제.

IF EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'MINOR_CODE')
   AND NOT EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'L_MINOR_CODE')
BEGIN
    INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt)
    SELECT 'L_MINOR_CODE', proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt
    FROM sysLookupM WHERE lookup_key = 'MINOR_CODE';

    UPDATE sysLookupP SET lookup_key = 'L_MINOR_CODE' WHERE lookup_key = 'MINOR_CODE';

    DELETE FROM sysLookupM WHERE lookup_key = 'MINOR_CODE';
END
GO
