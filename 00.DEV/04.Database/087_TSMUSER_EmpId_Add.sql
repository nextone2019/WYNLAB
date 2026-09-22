/* ---------- TSMUSER: TBAEMP를 EMP_NO로 조인하던 걸 EMP_ID로 바꾸기 위한 준비 ----------
   TSMUSER.EMP_NO는 그대로 둔다(화면에서 여전히 사번으로 입력/표시하고, USP_SM_USERAUTH_Q가
   U.EMP_NO를 그대로 출력하는 등 독자적인 값으로도 쓰이기 때문) - EMP_ID는 TBAEMP와의 조인
   전용으로 새로 추가하고, USP_SM_USERAUTH_S가 저장할 때마다 EMP_NO로 TBAEMP를 찾아 같이
   채워 넣는다(088 참고). 그래서 화면(frmUserAuth)은 하나도 안 고쳐도 된다 - 여전히 사번만
   입력하면 서버가 알아서 EMP_ID를 같이 채운다. */

ALTER TABLE TSMUSER ADD EMP_ID BIGINT NULL;
GO

UPDATE U
SET U.EMP_ID = E.EMP_ID
FROM TSMUSER U
JOIN TBAEMP E ON E.emp_no = U.EMP_NO
WHERE U.EMP_NO IS NOT NULL;
GO
