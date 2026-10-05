-- 부가세유형 콤보(L_CM0004)에 세율(rel_cd1) 컬럼 추가 (2026-10-05)
--  frmCust/frmAcc/frmSo/frmPo가 "부가세유형을 고르면 부가세율이 그 유형의 관리항목1(rel_cd1)로 바뀌는" 동작을 쓰는데
--  LookUp 쿼리가 minor_cd/minor_nm만 돌려줘서 rel_cd1을 읽을 수 없었다. width=0으로 등록해 팝업에는 안 보이고 데이터 컬럼으로만 남긴다.
-- 적용 대상: WYNLAB_DEV, FADU 양쪽 DB.

UPDATE sysLookupM
   SET query_txt = N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm, ' + CHAR(13) + CHAR(10) + N'            rel_cd1 ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''CM0004''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
 WHERE lookup_key = 'L_CM0004';

IF NOT EXISTS (SELECT 1 FROM sysLookupC WHERE lookup_key = 'L_CM0004' AND column_nm = 'rel_cd1')
    INSERT INTO sysLookupC (lookup_key, column_nm, caption, sort, width, visible_yn)
    VALUES ('L_CM0004', 'rel_cd1', N'세율', 3, 0, 'Y');
GO
