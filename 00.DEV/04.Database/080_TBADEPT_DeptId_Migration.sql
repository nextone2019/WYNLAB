/* =========================================================
   TBADEPT 재설계 - DEPT_CD(VARCHAR, 사람이 직접 입력) -> DEPT_ID(BIGINT IDENTITY).
   072(TSMMENU MENU_CD->MENU_ID)와 완전히 같은 이유/패턴 - 부서코드는 사용자가 알아야 할
   필요가 없는 순수 내부 식별자라는 판단(2026-09-06, 사용자 직접 결정).

   같이 바뀌는 것:
   - TBAEMP.dept_cd(VARCHAR) -> TBAEMP.dept_id(BIGINT) - 사번(emp_no)은 이 마이그레이션과
     무관하게 그대로 유지(별도 084 파일에서 EMP_ID를 "추가"만 함, emp_no는 안 없어짐).
   - TBAITEM.dept_cd(VARCHAR) -> TBAITEM.dept_id(BIGINT) - 담당부서 컬럼, 조인/팝업 없이
     단순 값 컬럼이었지만 타입은 맞춰야 한다.
   - 관련 프로시저 전부는 081_TBADEPT_Procs.sql에서 재작성(SSP_POP_DEPT_Q, USP_BA_DEPT_Q/S,
     USP_BA_EMP_Q, SSP_WYNLAB_GetSession, USP_SM_USERAUTH_Q, USP_SM_USERGRP_Q_2,
     USP_SM_MENUAUTH_Q_2).

   기존 데이터는 지우지 않고 *_OLD_STRCD_20260906로 이름만 바꿔 보존한다(072/073과 같은 관례) -
   확인 끝나면 나중에 수동으로 DROP TABLE 하면 된다.
   ========================================================= */

-- ============================================================
-- 0) 기존 테이블 이름 보존 + PK 제약조건 이름도 같이 비켜준다
--    (TBADEPT의 PK 제약조건은 자동생성 이름이라 sys.key_constraints에서 미리 확인한 실제 이름 사용)
-- ============================================================
DECLARE @pkName NVARCHAR(200) = (
    SELECT kc.name FROM sys.key_constraints kc
    WHERE kc.parent_object_id = OBJECT_ID('TBADEPT') AND kc.type = 'PK'
);
EXEC sp_rename 'TBADEPT', 'TBADEPT_OLD_STRCD_20260906';
EXEC sp_rename @pkName, 'PK_TBADEPT_OLD_STRCD_20260906';
GO

-- ============================================================
-- 1) 새 TBADEPT 생성 (OLD_DEPT_CD는 마이그레이션 중간에만 쓰는 임시 컬럼 - 맨 끝에 DROP)
-- ============================================================
CREATE TABLE [dbo].[TBADEPT](
    [DEPT_ID] [bigint] IDENTITY(1,1) NOT NULL,
    [acc_cd] [varchar](10) NOT NULL,
    [dept_nm] [varchar](50) NULL,
    [PAR_DEPT_ID] [bigint] NULL,
    [dept_type] [varchar](10) NULL,
    [remark] [nvarchar](4000) NULL,
    [reg_user_id] [varchar](50) NOT NULL,
    [reg_dt] [datetime] NULL,
    [reg_pc] [nvarchar](400) NULL,
    [upt_user_id] [varchar](50) NULL,
    [upt_dt] [datetime] NULL,
    [upt_pc] [nvarchar](400) NULL,
    [OLD_DEPT_CD] [varchar](20) NULL,
 CONSTRAINT [PK_TBADEPT] PRIMARY KEY CLUSTERED ([DEPT_ID] ASC)
) ON [PRIMARY]
GO

-- ============================================================
-- 2) 데이터 이관
-- ============================================================
INSERT INTO TBADEPT (
    acc_cd, dept_nm, dept_type, remark,
    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc, OLD_DEPT_CD
)
SELECT
    acc_cd, dept_nm, dept_type, remark,
    ISNULL(reg_user_id, 'system'), reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc, dept_cd
FROM TBADEPT_OLD_STRCD_20260906;
GO

-- PAR_DEPT_ID 해소 - 옛 par_dept_cd를 새 DEPT_ID로 치환
UPDATE T
SET PAR_DEPT_ID = P.DEPT_ID
FROM TBADEPT T
JOIN TBADEPT_OLD_STRCD_20260906 O ON O.dept_cd = T.OLD_DEPT_CD
JOIN TBADEPT P ON P.OLD_DEPT_CD = O.par_dept_cd
WHERE O.par_dept_cd IS NOT NULL;
GO

-- ============================================================
-- 3) TBAEMP.dept_cd -> dept_id
-- ============================================================
ALTER TABLE TBAEMP ADD [DEPT_ID] [bigint] NULL;
GO
UPDATE E
SET DEPT_ID = D.DEPT_ID
FROM TBAEMP E
JOIN TBADEPT D ON D.OLD_DEPT_CD = E.dept_cd;
GO
ALTER TABLE TBAEMP DROP COLUMN dept_cd;
GO

-- ============================================================
-- 4) TBAITEM.dept_cd -> dept_id (조인/팝업 없이 값만 들고 있던 컬럼 - 타입만 맞춘다)
-- ============================================================
ALTER TABLE TBAITEM ADD [DEPT_ID] [bigint] NULL;
GO
UPDATE I
SET DEPT_ID = D.DEPT_ID
FROM TBAITEM I
JOIN TBADEPT D ON D.OLD_DEPT_CD = I.dept_cd;
GO
ALTER TABLE TBAITEM DROP COLUMN dept_cd;
GO

-- ============================================================
-- 5) 임시 컬럼 정리
-- ============================================================
ALTER TABLE TBADEPT DROP COLUMN OLD_DEPT_CD;
GO
