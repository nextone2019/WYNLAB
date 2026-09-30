-- 공정코드를 기초코드(PR0004)에서 빼고 별도 마스터 테이블 TBAPROC로 분리 (2026-09-29, 사용자 지시).
-- 공정은 업체마다 늘어날 수 있고(검사, 세척 등) 정산단위/외주여부 같은 속성을 붙일 수 있어 코드 테이블 대신 마스터로 둔다.
--  * TPRROUTED/TPRWOD/TPRRSLTM.proc_cd는 그대로 TBAPROC.proc_cd 값을 저장한다(컬럼 변경 없음).
--  * 223번이 만든 PR0004(TSMMAJOR/TSMMINOR)와 콤보 L_PR0004는 삭제하고, 콤보는 TBAPROC를 조회하는 L_PRPROC로 대체한다.
--    (PR0004 번호는 비워 둔다. 나머지 PR0005~7은 이미 쓰이고 있어 재번호하지 않는다.)

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TBAPROC')
BEGIN
    CREATE TABLE TBAPROC (
        acc_id         BIGINT         NOT NULL,
        proc_cd        VARCHAR(10)    NOT NULL,
        proc_nm        NVARCHAR(50)   NOT NULL,
        sort           INT            NOT NULL CONSTRAINT DF_TBAPROC_sort DEFAULT (0),
        use_yn         VARCHAR(1)     NOT NULL CONSTRAINT DF_TBAPROC_use DEFAULT ('Y'),
        remark         NVARCHAR(3000) NULL,
        reg_user_id    VARCHAR(30)    NULL,
        reg_dt         DATETIME       NULL,
        reg_pc         NVARCHAR(200)  NULL,
        upt_user_id    VARCHAR(30)    NULL,
        upt_dt         DATETIME       NULL,
        upt_pc         NVARCHAR(200)  NULL,
        CONSTRAINT PK_TBAPROC PRIMARY KEY CLUSTERED (acc_id, proc_cd),
        CONSTRAINT CK_TBAPROC_use CHECK (use_yn IN ('Y', 'N'))
    );
END
GO

-- 기존 PR0004 코드값을 옮긴다(이미 있으면 건너뜀). 옮긴 뒤 코드 테이블에서 삭제.
INSERT INTO TBAPROC (acc_id, proc_cd, proc_nm, sort, use_yn, reg_user_id, reg_dt)
SELECT 1, v.cd, v.nm, v.sort, 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('BUMP', N'Bumping', 1), ('EDS', N'EDS', 2), ('PKG', N'Packaging', 3), ('FT', N'Final Test', 4)) v(cd, nm, sort)
WHERE NOT EXISTS (SELECT 1 FROM TBAPROC p WHERE p.acc_id = 1 AND p.proc_cd = v.cd);
GO

DELETE FROM TSMMINOR WHERE major_cd = 'PR0004';
DELETE FROM TSMMAJOR WHERE major_cd = 'PR0004';
DELETE FROM sysLookupM WHERE lookup_key = 'L_PR0004';
GO

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_PRPROC', NULL, N'공정', 'proc_cd', 'proc_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   proc_cd, ' + CHAR(13) + CHAR(10) + N'            proc_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TBAPROC' + CHAR(13) + CHAR(10) + N'WHERE use_yn = ''Y''' + CHAR(13) + CHAR(10)
       + N'Order by sort, proc_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_PRPROC');
GO
