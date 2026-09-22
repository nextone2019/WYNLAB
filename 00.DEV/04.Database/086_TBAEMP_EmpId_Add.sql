/* ---------- TBAEMP: EMP_ID(bigint identity) 추가, PK를 emp_no -> EMP_ID로 이관, emp_no는
   UNIQUE 제약으로 유지(사장님 결정: "EMP_NO는 유지, PK 아님") ----------
   DEPT_ID/CUST_ID와 달리 emp_no는 화면/다른 테이블(TSMUSER.EMP_NO, TBACUST.emp_no,
   TBAITEM.emp_no)에서 계속 그대로 쓴다 - UNIQUE 제약만 있으면 기존 프로시저/화면은 전혀
   안 고쳐도 된다(모두 emp_no로 조회/저장하며, emp_no는 여전히 유일함). 그래서 이 마이그레이션은
   순수 DB 스키마 변경만 하고 프로시저/클라이언트는 건드리지 않는다. */

DECLARE @pkEmp NVARCHAR(200) = (SELECT kc.name FROM sys.key_constraints kc WHERE kc.parent_object_id = OBJECT_ID('TBAEMP') AND kc.type = 'PK');
EXEC sp_rename @pkEmp, 'PK_TBAEMP_OLD_EMPNO_20260906';
GO

ALTER TABLE TBAEMP ADD EMP_ID BIGINT IDENTITY(1,1) NOT NULL;
GO

ALTER TABLE TBAEMP DROP CONSTRAINT PK_TBAEMP_OLD_EMPNO_20260906;
GO

ALTER TABLE TBAEMP ADD CONSTRAINT PK_TBAEMP PRIMARY KEY (EMP_ID);
GO

ALTER TABLE TBAEMP ADD CONSTRAINT UQ_TBAEMP_EMPNO UNIQUE (emp_no);
GO
