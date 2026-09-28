-- 수불 공통코드(2026-09-25). TMATRANS.trans_kind / trans_type 용.
--   MA0010 수불구분  : I 입고 / O 출고
--   MA0011 수불유형  : 구매입고, 기타입고, 이동입고, 이동출고, 생산입고 등. rel_cd1에 그 유형의 방향(I/O)을 넣어 두어서
--                      프로시저/콤보가 "입고 유형만" 같은 필터를 걸 수 있고, trans_kind와 trans_type이 어긋나는 입력을 막는 근거로 쓴다.
--                      취소(역거래)는 원 유형에 trans_kind만 반대로 넣으므로 이 방향과 trans_kind가 다를 수 있다 - 그 경우는 org_trans_id가 있는 행.
-- 프로시저가 유형 값을 직접 쓰므로 sys_yn='Y'(시스템 코드). 유형을 더 늘리려면 코드관리 화면에서 추가한다.
-- 콤보(L_MA0010/L_MA0011)와 함께, 기존에 빠져 있던 L_MA0002(발주 진행상태 - frmPo가 이미 쓰는 키)/L_MA0009(구매 입고방식)도 등록한다.

IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0010')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0010', N'수불구분', 'Y', 'SYSTEM', GETDATE());
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0011')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0011', N'수불유형', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0010')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0010', 'I', N'입고', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0010', 'O', N'출고', 2, 'Y', 'Y', 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, rel_cd1, reg_user_id, reg_dt)
VALUES ('MA0011', 'PU_IN',   N'구매입고',       1, 'Y', 'Y', 'I', 'SYSTEM', GETDATE()),
       ('MA0011', 'ETC_IN',  N'기타입고',       2, 'Y', 'Y', 'I', 'SYSTEM', GETDATE()),
       ('MA0011', 'MV_IN',   N'이동입고',       3, 'Y', 'Y', 'I', 'SYSTEM', GETDATE()),
       ('MA0011', 'PR_IN',   N'생산입고',       4, 'Y', 'Y', 'I', 'SYSTEM', GETDATE()),
       ('MA0011', 'SA_IN',   N'판매반품입고',   5, 'Y', 'Y', 'I', 'SYSTEM', GETDATE()),
       ('MA0011', 'ADJ_IN',  N'재고조정증가',   6, 'Y', 'Y', 'I', 'SYSTEM', GETDATE()),
       ('MA0011', 'PU_OUT',  N'구매반품출고',   11, 'Y', 'Y', 'O', 'SYSTEM', GETDATE()),
       ('MA0011', 'ETC_OUT', N'기타출고',       12, 'Y', 'Y', 'O', 'SYSTEM', GETDATE()),
       ('MA0011', 'MV_OUT',  N'이동출고',       13, 'Y', 'Y', 'O', 'SYSTEM', GETDATE()),
       ('MA0011', 'PR_OUT',  N'생산투입',       14, 'Y', 'Y', 'O', 'SYSTEM', GETDATE()),
       ('MA0011', 'SA_OUT',  N'판매출고',       15, 'Y', 'Y', 'O', 'SYSTEM', GETDATE()),
       ('MA0011', 'ADJ_OUT', N'재고조정감소',   16, 'Y', 'Y', 'O', 'SYSTEM', GETDATE());
GO

DECLARE @keys TABLE (k VARCHAR(30), nm NVARCHAR(100), major VARCHAR(20));
INSERT INTO @keys VALUES
    ('L_MA0002', N'발주진행상태', 'MA0002'),
    ('L_MA0009', N'구매입고방식', 'MA0009'),
    ('L_MA0010', N'수불구분',     'MA0010'),
    ('L_MA0011', N'수불유형',     'MA0011');

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT k.k, NULL, k.nm, 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''' + k.major + N'''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
FROM @keys k
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = k.k);
GO
