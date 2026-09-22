-- 결재승인 프로세스 1차 테스트: TAPDOC(결재 HEADER)/TAPDOCPATH(결재 DETAIL) + THRNAMECARDREQ(명함신청서,
-- 테스트용 업무문서) 생성. 사용자가 이미 라이브 DB에 3개 테이블을 직접 만들어뒀으나(마이그레이션
-- 파일 없음, 전부 0건) acc_cd/dept_cd/emp_no가 TBAACC/TBADEPT/TBAEMP의 현재 ID 체계(ACC_ID/
-- DEPT_ID/EMP_ID)와 어긋나 있어 이 파일에서 DROP 후 acc_id/dept_id/emp_id로 교정해서 다시 만든다
-- (3개 테이블 모두 0건이라 데이터 손실 없음).
--
-- 결재 액션(상신/승인/반려)은 문서유형(doc_type)마다 다른 화면(다른 메뉴/PROC_PREFIX)에서 호출돼야
-- 하므로 범용 데이터 통로(api/data/*, DataController)가 아니라 전용 컨트롤러(ApprovalsController,
-- 메뉴/PROC_PREFIX 체크 없이 로그인만 확인)를 통해서만 호출한다 - Popup/LookUp 프레임워크
-- (LookupsController/ComboLookupsController)와 같은 전례.
--
-- 결재상태 반영은 Pull 방식이 기본이지만(THRNAMECARDREQ 조회시 TAPDOC을 JOIN해서 보여줌),
-- 원본 문서 자신의 진행상태(stat_cd)만은 최종승인 시 USP_AP_APPR_S가 직접 UPDATE한다(사장님 지시:
-- 최초저장 '0' -> 결재 최종승인 시 'C'). 문서유형이 늘어나면 USP_AP_APPR_S의 doc_type 분기에
-- 그 문서유형의 UPDATE 한 줄을 추가하면 된다 - 지금은 NAMECARD 하나뿐이라 동적 SQL/메타테이블
-- 없이 단순 IF 분기로 처리한다(과설계 방지).

IF OBJECT_ID('TAPDOCPATH') IS NOT NULL DROP TABLE TAPDOCPATH;
IF OBJECT_ID('TAPDOC') IS NOT NULL DROP TABLE TAPDOC;
IF OBJECT_ID('THRNAMECARDREQ') IS NOT NULL DROP TABLE THRNAMECARDREQ;
GO

/* ---------- TAPDOC: 결재 HEADER ---------- */
CREATE TABLE TAPDOC (
    app_id       BIGINT IDENTITY(1,1) NOT NULL,
    app_no       VARCHAR(20)    NOT NULL,
    app_date     VARCHAR(8)     NOT NULL,
    app_title    NVARCHAR(500)  NOT NULL,
    app_text     NVARCHAR(4000) NOT NULL,
    form_id      VARCHAR(30)    NULL,
    doc_type     VARCHAR(10)    NOT NULL,
    doc_id       BIGINT         NOT NULL,
    doc_no       VARCHAR(20)    NOT NULL,
    acc_id       BIGINT         NOT NULL,
    dept_id      BIGINT         NOT NULL,
    emp_id       BIGINT         NOT NULL,
    end_yn       VARCHAR(1)     NOT NULL DEFAULT ('N'),
    end_emp_id   BIGINT         NULL,
    end_dt       DATETIME       NULL,
    rtn_yn       VARCHAR(1)     NOT NULL DEFAULT ('N'),
    rtn_emp_id   BIGINT         NULL,
    rtn_dt       DATETIME       NULL,
    stat_cd      VARCHAR(10)    NULL,
    reg_user_id  VARCHAR(30)    NULL,
    reg_dt       DATETIME       NULL,
    reg_pc       NVARCHAR(200)  NULL,
    upt_user_id  VARCHAR(30)    NULL,
    upt_dt       DATETIME       NULL,
    upt_pc       NVARCHAR(200)  NULL,
    CONSTRAINT PK_TAPDOC PRIMARY KEY CLUSTERED (app_id)
);
GO

/* ---------- TAPDOCPATH: 결재 DETAIL(결재경로/승인자) ---------- */
CREATE TABLE TAPDOCPATH (
    app_id       BIGINT         NOT NULL,
    serl         INT            NOT NULL,
    app_no       VARCHAR(20)    NOT NULL,
    doc_type     VARCHAR(10)    NOT NULL,
    doc_id       BIGINT         NOT NULL,
    doc_no       VARCHAR(20)    NOT NULL,
    sort         INT            NOT NULL,
    path_type    VARCHAR(10)    NOT NULL,
    emp_id       BIGINT         NOT NULL,
    stat_cd      VARCHAR(1)     NOT NULL DEFAULT ('N'),
    app_dt       DATETIME       NULL,
    remark       NVARCHAR(1000) NULL,
    reg_user_id  VARCHAR(30)    NULL,
    reg_dt       DATETIME       NULL,
    reg_pc       NVARCHAR(200)  NULL,
    upt_user_id  VARCHAR(30)    NULL,
    upt_dt       DATETIME       NULL,
    upt_pc       NVARCHAR(200)  NULL,
    CONSTRAINT PK_TAPDOCPATH PRIMARY KEY CLUSTERED (app_id, serl)
);
GO

/* ---------- THRNAMECARDREQ: 명함신청서(테스트용 업무문서) ---------- */
CREATE TABLE THRNAMECARDREQ (
    req_id       BIGINT IDENTITY(1,1) NOT NULL,
    req_no       VARCHAR(20)    NOT NULL,
    dept_id      BIGINT         NOT NULL,
    emp_id       BIGINT         NOT NULL,
    job_grade    VARCHAR(100)   NULL,
    name_kor     VARCHAR(50)    NULL,
    name_eng     VARCHAR(50)    NULL,
    dept_kor     VARCHAR(50)    NULL,
    dept_eng     VARCHAR(50)    NULL,
    mobile       NVARCHAR(50)   NULL,
    email        NVARCHAR(50)   NULL,
    stat_cd      VARCHAR(1)     NOT NULL DEFAULT ('0'), -- 0=신청, C=결재 최종승인 확정
    app_id       BIGINT         NULL,
    app_no       VARCHAR(20)    NULL,
    remark       NVARCHAR(3000) NULL,
    reg_user_id  VARCHAR(50)    NULL,
    reg_dt       DATETIME       NULL,
    reg_pc       NVARCHAR(50)   NULL,
    upt_user_id  VARCHAR(50)    NULL,
    upt_dt       DATETIME       NULL,
    upt_pc       NVARCHAR(50)   NULL,
    CONSTRAINT PK_THRNAMECARDREQ PRIMARY KEY CLUSTERED (req_id)
);
GO

/* ---------- 소분류코드 시드 ---------- */
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('AP0001', N'결재상태', 'Y', 'SYSTEM', GETDATE());

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES
    ('AP0001', 'P', N'진행중', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('AP0001', 'Y', N'승인완료', 2, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('AP0001', 'R', N'반려', 3, 'Y', 'Y', 'SYSTEM', GETDATE());

INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('AP0002', N'결재문서유형', 'Y', 'SYSTEM', GETDATE());

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('AP0002', 'NAMECARD', N'명함신청서', 1, 'Y', 'Y', 'SYSTEM', GETDATE());

INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('GW0001', N'명함신청 진행상태', 'Y', 'SYSTEM', GETDATE());

INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES
    ('GW0001', '0', N'신청', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('GW0001', 'C', N'확정', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

/* ---------- USP_HR_NAMECARD_Q: 명함신청서 목록조회 ---------- */
CREATE OR ALTER PROCEDURE USP_HR_NAMECARD_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    @p_req_no VARCHAR(20) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            -- 결재상태(appr_stat_cd)는 TAPDOC을 LEFT JOIN해서 그때그때 계산(Pull) - THRNAMECARDREQ
            -- 자체에는 중복 저장하지 않는다. stat_cd(진행상태)만 최종승인 시 USP_AP_APPR_S가 직접 갱신.
            SELECT
                n.req_id, n.req_no, n.dept_id, d.dept_nm, n.emp_id, e.emp_no, e.emp_nm,
                n.job_grade, n.name_kor, n.name_eng, n.dept_kor, n.dept_eng, n.mobile, n.email,
                n.stat_cd, n.app_id, n.app_no, n.remark, n.reg_dt,
                a.stat_cd AS appr_stat_cd
            FROM THRNAMECARDREQ n
            LEFT JOIN TBAEMP e ON e.EMP_ID = n.emp_id
            LEFT JOIN TBADEPT d ON d.DEPT_ID = n.dept_id
            LEFT JOIN TAPDOC a ON a.app_id = n.app_id
            WHERE (@p_emp_no IS NULL OR e.emp_no = @p_emp_no)
              AND (@p_req_id IS NULL OR n.req_id = @p_req_id)
              AND (@p_req_no IS NULL OR n.req_no LIKE '%' + @p_req_no + '%')
            ORDER BY n.req_id DESC;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- USP_HR_NAMECARD_S: 명함신청서 등록/수정/삭제 ---------- */
CREATE OR ALTER PROCEDURE USP_HR_NAMECARD_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_job_grade VARCHAR(100) = NULL,
    @p_name_kor VARCHAR(50) = NULL,
    @p_name_eng VARCHAR(50) = NULL,
    @p_dept_kor VARCHAR(50) = NULL,
    @p_dept_eng VARCHAR(50) = NULL,
    @p_mobile NVARCHAR(50) = NULL,
    @p_email NVARCHAR(50) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        -- dept_id/emp_id는 화면이 몰라도 되게(Session에 DeptId가 없어서 EmpNo/DeptNm만 노출 -
        -- 화면 하나 때문에 공용 세션을 확장하는 대신) @p_user_id(로그인 세션, 클라이언트가 못
        -- 속임 - GenericDataRepository.SaveAsync가 항상 서버에서 채움)로 TSMUSER->TBAEMP를
        -- 타고 서버가 직접 채운다.
        DECLARE @dept_id BIGINT, @emp_id BIGINT;
        SELECT @emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
        SELECT @dept_id = DEPT_ID FROM TBAEMP WHERE EMP_ID = @emp_id;

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO THRNAMECARDREQ (
                req_no, dept_id, emp_id, job_grade, name_kor, name_eng, dept_kor, dept_eng,
                mobile, email, stat_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', @dept_id, @emp_id, @p_job_grade, @p_name_kor, @p_name_eng, @p_dept_kor, @p_dept_eng,
                @p_mobile, @p_email, '0', @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_req_id = SCOPE_IDENTITY();
            UPDATE THRNAMECARDREQ
               SET req_no = 'NC' + RIGHT('00000000' + CAST(@p_req_id AS VARCHAR(8)), 8)
             WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE THRNAMECARDREQ SET
                job_grade = @p_job_grade, name_kor = @p_name_kor, name_eng = @p_name_eng,
                dept_kor = @p_dept_kor, dept_eng = @p_dept_eng, mobile = @p_mobile, email = @p_email,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM THRNAMECARDREQ WHERE req_id = @p_req_id;
        END

        SET @GeneratedCode = CAST(@p_req_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- USP_AP_APPR_Q: 결재 조회(이력/결재함) ---------- */
CREATE OR ALTER PROCEDURE USP_AP_APPR_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_emp_no VARCHAR(20) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q' -- 특정 문서 1건의 결재이력(헤더 결과셋 + 상세 결과셋)
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.app_text, a.doc_type, a.doc_id,
                   a.doc_no, a.end_yn, a.end_dt, a.rtn_yn, a.rtn_dt, a.stat_cd, a.emp_id, e.emp_nm
            FROM TAPDOC a
            LEFT JOIN TBAEMP e ON e.EMP_ID = a.emp_id
            WHERE a.doc_type = @p_doc_type AND a.doc_id = @p_doc_id
            ORDER BY a.app_id DESC;

            SELECT p.app_id, p.serl, p.sort, p.path_type, p.emp_id, ep.emp_nm, p.stat_cd, p.app_dt, p.remark
            FROM TAPDOCPATH p
            LEFT JOIN TBAEMP ep ON ep.EMP_ID = p.emp_id
            WHERE p.doc_type = @p_doc_type AND p.doc_id = @p_doc_id
            ORDER BY p.app_id DESC, p.sort;
        END
        ELSE IF @p_work_type = 'Q1' -- 결재함: 로그인 사용자가 처리해야 할 대기건
        BEGIN
            SELECT a.app_id, a.app_no, a.app_date, a.app_title, a.doc_type, a.doc_id, a.doc_no,
                   p.serl, p.sort, p.path_type, e2.emp_nm AS req_emp_nm
            FROM TAPDOCPATH p
            JOIN TAPDOC a ON a.app_id = p.app_id
            JOIN TBAEMP e ON e.emp_no = @p_emp_no
            LEFT JOIN TBAEMP e2 ON e2.EMP_ID = a.emp_id
            WHERE p.emp_id = e.EMP_ID AND p.stat_cd = 'N' AND a.end_yn = 'N' AND a.rtn_yn = 'N'
            ORDER BY a.app_id;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- USP_AP_APPR_S: 결재상신/승인/반려 ---------- */
CREATE OR ALTER PROCEDURE USP_AP_APPR_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_app_id BIGINT = NULL,
    @p_doc_type VARCHAR(10) = NULL,
    @p_doc_id BIGINT = NULL,
    @p_doc_no VARCHAR(20) = NULL,
    @p_app_title NVARCHAR(500) = NULL,
    @p_app_text NVARCHAR(4000) = NULL,
    @p_approver_emp_no VARCHAR(20) = NULL,
    @p_opinion NVARCHAR(1000) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'N' -- 결재상신: TAPDOC 헤더 1건 + TAPDOCPATH 1건(sort=1, v1은 승인자 1명 고정)
        BEGIN
            -- 기안자(emp_id)는 @p_user_id(로그인 세션, 서버가 항상 채움)로 TSMUSER->TBAEMP를
            -- 타고 서버가 직접 채운다 - 클라이언트가 "누가 상신했는지"를 속일 수 없게.
            DECLARE @req_dept_id BIGINT, @req_emp_id BIGINT, @req_acc_id BIGINT, @apprvr_emp_id BIGINT;
            SELECT @req_emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
            SELECT @req_dept_id = DEPT_ID, @req_acc_id = acc_id FROM TBAEMP WHERE EMP_ID = @req_emp_id;

            -- 승인자(누구에게 결재를 넘길지)는 기안자가 화면에서 고른 대상이라 클라이언트가 보내도
            -- 문제 없다(신원 주장이 아니라 라우팅 대상 선택) - P_EMP 팝업에서 고른 emp_no.
            SELECT @apprvr_emp_id = EMP_ID FROM TBAEMP WHERE emp_no = @p_approver_emp_no;

            IF @apprvr_emp_id IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'승인자 사원 정보를 찾을 수 없습니다.';
                RETURN;
            END

            INSERT INTO TAPDOC (
                app_no, app_date, app_title, app_text, doc_type, doc_id, doc_no,
                acc_id, dept_id, emp_id, end_yn, rtn_yn, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', CONVERT(VARCHAR(8), GETDATE(), 112), @p_app_title, @p_app_text, @p_doc_type, @p_doc_id, @p_doc_no,
                @req_acc_id, @req_dept_id, @req_emp_id, 'N', 'N', 'P',
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_app_id = SCOPE_IDENTITY();
            UPDATE TAPDOC
               SET app_no = 'AP' + RIGHT('00000000' + CAST(@p_app_id AS VARCHAR(8)), 8)
             WHERE app_id = @p_app_id;

            INSERT INTO TAPDOCPATH (
                app_id, serl, app_no, doc_type, doc_id, doc_no, sort, path_type, emp_id, stat_cd,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT @p_app_id, 1, app_no, @p_doc_type, @p_doc_id, @p_doc_no, 1, 'A', @apprvr_emp_id, 'N',
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TAPDOC WHERE app_id = @p_app_id;

            -- 원본 문서에 결재번호 되채움 - 지금은 명함신청서(NAMECARD) 하나뿐이라 단순 IF 분기로
            -- 처리한다(문서유형이 늘면 여기에 분기를 추가).
            IF @p_doc_type = 'NAMECARD'
                UPDATE THRNAMECARDREQ
                   SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id)
                 WHERE req_id = @p_doc_id;

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'A' -- 승인
        BEGIN
            DECLARE @apprv_emp_id BIGINT;
            SELECT @apprv_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            UPDATE TAPDOCPATH
               SET stat_cd = 'Y', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @apprv_emp_id AND stat_cd = 'N';

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            -- v1은 승인자 1명 고정이라 이 승인이 곧 최종승인이다(다음 대기 결재자가 남아있으면
            -- 최종처리를 건너뛰게 만든 것 - 나중에 다단계 결재선으로 확장해도 그대로 맞는다).
            IF NOT EXISTS (SELECT 1 FROM TAPDOCPATH WHERE app_id = @p_app_id AND stat_cd = 'N')
            BEGIN
                UPDATE TAPDOC
                   SET end_yn = 'Y', end_emp_id = @apprv_emp_id, end_dt = GETDATE(), stat_cd = 'Y',
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
                 WHERE app_id = @p_app_id;

                DECLARE @fin_doc_type VARCHAR(10), @fin_doc_id BIGINT;
                SELECT @fin_doc_type = doc_type, @fin_doc_id = doc_id FROM TAPDOC WHERE app_id = @p_app_id;

                IF @fin_doc_type = 'NAMECARD'
                    UPDATE THRNAMECARDREQ SET stat_cd = 'C' WHERE req_id = @fin_doc_id;
            END

            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'R' -- 반려
        BEGIN
            DECLARE @rej_emp_id BIGINT;
            SELECT @rej_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

            UPDATE TAPDOCPATH
               SET stat_cd = 'R', app_dt = GETDATE(), remark = @p_opinion,
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id AND emp_id = @rej_emp_id AND stat_cd = 'N';

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'처리할 결재 대기 건을 찾을 수 없습니다.';
                RETURN;
            END

            UPDATE TAPDOC
               SET rtn_yn = 'Y', rtn_emp_id = @rej_emp_id, rtn_dt = GETDATE(), stat_cd = 'R',
                   upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE app_id = @p_app_id;

            -- 반려는 THRNAMECARDREQ.stat_cd(진행상태)를 건드리지 않는다(사장님 지시는 최종승인 시
            -- 'C' 전환만 명시 - 반려는 결재상태(TAPDOC, Pull 조회)로만 보여주고 원본 문서 상태는
            -- 그대로 둬서 기안자가 수정 후 재상신할 수 있게 한다).
            SET @GeneratedCode = CAST(@p_app_id AS VARCHAR(20));
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- 메뉴등록: 그룹웨어(GW) 새 최상위 그룹 + 명함신청서/결재함 화면 ---------- */
DECLARE @grpTop BIGINT, @grpSub BIGINT;

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SORT_ORDER, USE_YN, REG_USER_ID, REG_DT)
VALUES (N'그룹웨어', NULL, 1, 'GROUP', NULL, 70, 'Y', 'SYSTEM', GETDATE());
SET @grpTop = SCOPE_IDENTITY();

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SORT_ORDER, USE_YN, REG_USER_ID, REG_DT)
VALUES (N'결재/신청관리', @grpTop, 2, 'GROUP', NULL, 10, 'Y', 'SYSTEM', GETDATE());
SET @grpSub = SCOPE_IDENTITY();

-- SCREEN_CLASS_NM은 클래스명만(네임스페이스/어셈블리 없이) - ShellForm.OpenMenu가
-- $"WYNLAB.{MODULE}.{SCREEN_CLASS_NM}, WYNLAB.{MODULE}" 형태로 직접 조합한다(072번
-- 마이그레이션 이후 규약, 045_TBAACC_Create.sql 주석의 예전 FORM_CLASS_NM 한 컬럼 방식과 다름 -
-- 처음에 그 예전 방식으로 잘못 넣어서 메뉴가 안 열리는 걸 직접 겪고 고쳤다).
INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, REG_USER_ID, REG_DT)
VALUES (N'명함신청서', @grpSub, 3, 'FORM', 'GW', 'frmNameCardReq', 'USP_HR_NAMECARD_', 10, 'Y', 'SYSTEM', GETDATE());

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, REG_USER_ID, REG_DT)
VALUES (N'결재함', @grpSub, 3, 'FORM', 'GW', 'frmApprInbox', 'USP_AP_APPR_', 20, 'Y', 'SYSTEM', GETDATE());
GO
