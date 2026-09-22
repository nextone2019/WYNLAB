-- 새 운영 서버처럼 TSMUSER가 완전히 비어있는 DB에 최초 로그인용 관리자 계정을 만든다.
-- 로그인 화면은 TSMUSER 행이 하나도 없으면 그 자체로는 막지 않지만, 아무도 로그인할 수 없어
-- 그 뒤 화면(사용자권한관리 등)으로 계정을 추가할 방법이 없다 - 그래서 이 씨앗 계정이 필요하다.
--
-- 이미 admin 계정이 있으면(재실행/기존 DB) 아무것도 하지 않는다 - 여러 번 실행해도 안전하다.
IF EXISTS (SELECT 1 FROM TSMUSER WHERE USER_ID = 'admin')
BEGIN
    PRINT 'admin 계정이 이미 있어 건너뜁니다.';
    RETURN;
END

-- TSMUSER.ACC_ID/EMP_ID는 FK 제약이 없는 단순 참조 컬럼이지만, 로그인 후 화면들이 실제로
-- 그 값으로 TBAACC/TBAEMP를 조인해서 사업장명/사원명을 보여주므로, 최소한의 자리표시자(placeholder)
-- 행을 같이 만들어둔다 - 실제 회사 정보는 로그인한 뒤 시스템환경설정(frmSiteConfig)/사원등록
-- 화면에서 직접 채우면 된다.
-- ACC_ID/EMP_ID 둘 다 IDENTITY 컬럼이라 명시적으로 1을 넣으려면 IDENTITY_INSERT를
-- 켜야 한다(안 켜면 "IDENTITY_INSERT가 OFF로 설정되면..." 오류로 실패 - 로컬에서
-- 직접 재현해서 확인함).
IF NOT EXISTS (SELECT 1 FROM TBAACC WHERE ACC_ID = 1)
BEGIN
    SET IDENTITY_INSERT TBAACC ON;
    INSERT INTO TBAACC (ACC_ID, ACC_NM, REG_USER_ID, REG_DT)
    VALUES (1, N'회사명 미설정', 'admin', GETDATE());
    SET IDENTITY_INSERT TBAACC OFF;
END

IF NOT EXISTS (SELECT 1 FROM TBAEMP WHERE EMP_ID = 1)
BEGIN
    SET IDENTITY_INSERT TBAEMP ON;
    INSERT INTO TBAEMP (EMP_ID, emp_no, emp_nm, holi_yn, acc_id, reg_user_id, reg_dt)
    VALUES (1, 'admin', N'관리자', 'N', 1, 'admin', GETDATE());
    SET IDENTITY_INSERT TBAEMP OFF;
END

-- 비밀번호는 BCrypt.Net-Next(WYNLAB.Api가 실제 로그인 검증에 쓰는 것과 동일한 라이브러리/버전)로
-- 미리 해시해서 넣어둔다 - SQL Server엔 BCrypt 내장 함수가 없어서 여기서 직접 계산할 수 없다.
-- 초기 비밀번호는 "Admin!2026"이고, MUST_CHANGE_PWD_YN='Y'로 최초 로그인 시 반드시 새
-- 비밀번호로 바꾸도록 강제한다(AuthService.cs 참고) - 이 파일이 git에 그대로 남으므로,
-- 로그인하자마자 바로 비밀번호를 바꾸는 것을 전제로 한 임시 값이다.
INSERT INTO TSMUSER (
    USER_ID, USER_NM, PASSWORD_HASH, USER_TYPE, DEVELOPER_YN,
    PWD_FAIL_CNT, USE_YN, DEL_YN, REG_USER_ID, REG_DT, UPT_USER_ID, UPT_DT,
    MUST_CHANGE_PWD_YN, EMP_ID, ACC_ID
)
VALUES (
    'admin', N'관리자', '$2a$11$kJoexn0V35qgRzxkeBaS8uXR0GP1hKfjWmrFJH2vKp1Jr.89eEiaq', 'A', 'Y',
    0, 'Y', 'N', 'admin', GETDATE(), 'admin', GETDATE(),
    'Y', 1, 1
);

PRINT 'admin 계정 생성 완료 - 초기 비밀번호: Admin!2026 (최초 로그인 시 변경 강제됨)';
