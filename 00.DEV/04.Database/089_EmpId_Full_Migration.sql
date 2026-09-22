/* ---------- TBACUST/TBAITEM/TSMUSER: emp_no -> EMP_ID 완전 전환 ----------
   088에서는 TSMUSER에 EMP_ID를 "추가"만 하고 EMP_NO는 그대로 뒀는데, 사장님이 세 테이블
   모두 emp_no 대신 EMP_ID를 쓰도록 완전히 바꿔달라고 함. TBACUST/TBAITEM은 둘 다 0건이라
   단순 컬럼 교체, TSMUSER는 EMP_ID가 087에서 이미 백필돼있으니 EMP_NO만 마저 드롭한다.
   TBAEMP.emp_no 자체는 그대로 유지(사장님 결정: PK 아닌 UNIQUE 컬럼으로 유지) - 화면에
   사번을 보여줘야 할 때는 TBAEMP를 EMP_ID로 조인해서 emp_no를 읽어오면 된다. */

ALTER TABLE TBACUST ADD EMP_ID BIGINT NULL;
GO
ALTER TABLE TBACUST DROP COLUMN emp_no;
GO

ALTER TABLE TBAITEM ADD EMP_ID BIGINT NULL;
GO
ALTER TABLE TBAITEM DROP COLUMN emp_no;
GO

ALTER TABLE TSMUSER DROP COLUMN EMP_NO;
GO
