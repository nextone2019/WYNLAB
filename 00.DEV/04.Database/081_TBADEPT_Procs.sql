/* =========================================================
   080(TBADEPT DEPT_CD->DEPT_ID)에 딸린 프로시저/팝업 메타데이터 재작성 - 073(MenuId_Procs)과
   같은 패턴. dept_cd를 참조하던 모든 프로시저에서 조인/파라미터/출력 컬럼을 dept_id 기준으로
   바꾼다. "사용자가 부서코드를 알 필요 없다"는 전제라, 화면에 코드를 보여주거나 코드로
   검색하던 자리는 전부 이름(dept_nm) 기준으로 대체하거나 그냥 없앤다(SSP_WYNLAB_GetSession/
   USP_SM_USERAUTH_Q의 DeptCd 출력 컬럼 자체를 삭제 - DeptNm만 남김, 아무 화면도 DeptCd를
   실제로 안 쓰고 있었다).
   ========================================================= */

-- ============================================================
-- 1) SSP_POP_DEPT_Q - P_DEPT 팝업 소스. 검색은 이름으로만(코드 검색 제거).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_DEPT_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DEPT_ID, dept_nm, PAR_DEPT_ID, dept_type, remark
    FROM TBADEPT
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR dept_nm LIKE '%' + @p_keyword + '%')
    ORDER BY DEPT_ID;
END
GO

-- ============================================================
-- 2) SSP_POP_EMP_Q - P_EMP 팝업 소스. dept_cd 조인/검색/출력을 dept_id로 교체
--    (emp_no 자체는 이 마이그레이션과 무관 - 084에서 EMP_ID 추가할 때도 그대로 유지됨).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_EMP_Q]
    @p_code        VARCHAR(100) = NULL,
    @p_dept_nm     NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date
    FROM   TBAEMP as a
                JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
    WHERE  (@p_code IS NULL OR @p_code = ''
                OR a.emp_no LIKE '%' + @p_code + '%'
                OR a.emp_nm LIKE '%' + @p_code + '%')
    AND    (@p_dept_nm IS NULL OR @p_dept_nm = '' OR b.dept_nm LIKE '%' + @p_dept_nm + '%')
    ORDER BY a.emp_no;
END
GO

-- ============================================================
-- 3) USP_BA_DEPT_Q - Q(부서 목록, 이름검색) / Q1(선택 부서 소속 사원, dept_id 정확일치)
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_DEPT_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_dept_nm VARCHAR(50) = NULL,
    @p_dept_id BIGINT = NULL,		/* Q1일 때만 사용 - 정확히 일치하는 부서 */
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
            SELECT
                acc_cd, DEPT_ID, dept_nm, PAR_DEPT_ID, dept_type, remark
            FROM TBADEPT
            WHERE (@p_dept_nm IS NULL OR dept_nm LIKE '%' + @p_dept_nm + '%')
            ORDER BY DEPT_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                emp_no, emp_nm, emp_nm_eng, job_grade, job_type, tel, hp_tel, email
            FROM TBAEMP
            WHERE DEPT_ID = @p_dept_id
            ORDER BY emp_no;
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

-- ============================================================
-- 4) USP_BA_DEPT_S - N/U/D를 dept_id 기준으로. N은 SCOPE_IDENTITY()로 새 ID를 GeneratedCode에 담는다.
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_DEPT_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 서버가 채우도록 전환 */
    @p_dept_id BIGINT = NULL,		/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_dept_nm VARCHAR(50) = NULL,
    @p_par_dept_id BIGINT = NULL,
    @p_dept_type VARCHAR(10) = NULL,
    @p_remark NVARCHAR(2000) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBADEPT (
                acc_cd, dept_nm, PAR_DEPT_ID, dept_type, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_dept_nm, @p_par_dept_id, @p_dept_type, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBADEPT SET
                dept_nm = @p_dept_nm, PAR_DEPT_ID = @p_par_dept_id, dept_type = @p_dept_type,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE DEPT_ID = @p_dept_id;
            SET @GeneratedCode = CAST(@p_dept_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBADEPT WHERE DEPT_ID = @p_dept_id;
            SET @GeneratedCode = CAST(@p_dept_id AS VARCHAR(20));
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

-- ============================================================
-- 5) USP_BA_EMP_Q - dept_cd 조인/필터를 dept_id로 교체 (emp_no 쪽은 084 전까지 그대로)
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_emp_no   VARCHAR(20) = NULL,
    @p_dept_id  BIGINT = NULL,
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
            SELECT
                            a.acc_cd,
                            a.emp_no,
                            a.emp_nm,
                            a.emp_nm_eng,
                            a.DEPT_ID,
                            b.dept_nm,
                            a.ent_date,
                            a.grp_ent_date,
                            a.job_grade,
                            a.job_type,
                            a.ret_yn,
                            a.ret_date,
                            a.sex_cd,
                            a.tel,
                            a.hp_tel,
                            a.email,
                            a.nat_cd,
                            a.zip_code,
                            a.addr1,
                            a.addr2,
                            a.holi_yn,
                            a.dilig_yn,
                            a.pay_yn,
                            a.photo
            FROM        TBAEMP as  a
                            JOIN TBADEPT as b on a.DEPT_ID = b.DEPT_ID
            WHERE       1 = 1
            AND         (@p_dept_id IS NULL OR a.DEPT_ID = @p_dept_id)
            AND         ((@p_emp_no IS NULL OR emp_no LIKE '%' + @p_emp_no + '%')
                            OR
                          (@p_emp_no IS NULL OR emp_nm LIKE '%' + @p_emp_no + '%'))
            ORDER BY emp_no;
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

-- ============================================================
-- 6) USP_BA_EMP_S - dept_cd 파라미터를 dept_id로 교체 (emp_no는 084 전까지 그대로 PK)
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMP_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(10) = '0001',	/* TODO: 로그인 세션에 회사코드 생기면 거기서 채우도록 전환 */
    @p_emp_no VARCHAR(20),
    @p_emp_nm VARCHAR(100) = NULL,
    @p_emp_nm_eng VARCHAR(100) = NULL,
    @p_dept_id BIGINT = NULL,
    @p_ent_date VARCHAR(8) = NULL,
    @p_grp_ent_date VARCHAR(8) = NULL,
    @p_job_grade VARCHAR(10) = NULL,
    @p_job_type VARCHAR(10) = NULL,
    @p_ret_yn VARCHAR(2) = 'N',
    @p_ret_date VARCHAR(8) = NULL,
    @p_sex_cd VARCHAR(1) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_hp_tel VARCHAR(30) = NULL,
    @p_email NVARCHAR(50) = NULL,
    @p_nat_cd VARCHAR(10) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(300) = NULL,
    @p_addr2 NVARCHAR(300) = NULL,
    @p_holi_yn VARCHAR(1) = 'N',
    @p_dilig_yn VARCHAR(1) = NULL,
    @p_pay_yn VARCHAR(1) = NULL,
    @p_photo NVARCHAR(MAX) = NULL,	/* Base64 인코딩된 이미지 - NULL/빈문자열이면 사진 없음으로 저장 */
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
        DECLARE @photo_bin VARBINARY(MAX) = NULL;
        IF @p_photo IS NOT NULL AND LEN(@p_photo) > 0
            SET @photo_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_photo"))', 'VARBINARY(MAX)');

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAEMP (
                acc_cd, emp_no, emp_nm, emp_nm_eng, DEPT_ID, ent_date, grp_ent_date,
                job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email,
                nat_cd, zip_code, addr1, addr2, holi_yn, dilig_yn, pay_yn, photo,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_cd, @p_emp_no, @p_emp_nm, @p_emp_nm_eng, @p_dept_id, @p_ent_date, @p_grp_ent_date,
                @p_job_grade, @p_job_type, @p_ret_yn, @p_ret_date, @p_sex_cd, @p_tel, @p_hp_tel, @p_email,
                @p_nat_cd, @p_zip_code, @p_addr1, @p_addr2, @p_holi_yn, @p_dilig_yn, @p_pay_yn, @photo_bin,
                @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAEMP SET
                emp_nm = @p_emp_nm, emp_nm_eng = @p_emp_nm_eng, DEPT_ID = @p_dept_id,
                ent_date = @p_ent_date, grp_ent_date = @p_grp_ent_date,
                job_grade = @p_job_grade, job_type = @p_job_type, ret_yn = @p_ret_yn, ret_date = @p_ret_date,
                sex_cd = @p_sex_cd, tel = @p_tel, hp_tel = @p_hp_tel, email = @p_email, nat_cd = @p_nat_cd,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                holi_yn = @p_holi_yn, dilig_yn = @p_dilig_yn, pay_yn = @p_pay_yn, photo = @photo_bin,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE emp_no = @p_emp_no;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAEMP WHERE emp_no = @p_emp_no;
        END

        SET @GeneratedCode = @p_emp_no;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 7) SSP_WYNLAB_GetSession - DeptCd 출력 제거(아무도 안 씀), 조인만 dept_id로 교체
-- ============================================================
CREATE OR ALTER PROCEDURE SSP_WYNLAB_GetSession
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20),
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
            SELECT
                U.USER_ID       AS UserId,
                U.USER_NM       AS UserNm,
                U.EMP_NO        AS EmpNo,
                D.DEPT_NM       AS DeptNm,
                U.PASSWORD_HASH AS PasswordHash,
                U.EMAIL         AS Email,
                U.USE_YN        AS UseYn,
                U.DEVELOPER_YN  AS DeveloperYn,
                U.USER_TYPE     AS UserType,
                U.PWD_FAIL_CNT  AS PwdFailCnt,
                U.MUST_CHANGE_PWD_YN AS MustChangePwdYn
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            WHERE U.USER_ID = @p_user_id;

            SELECT USER_GRP_CD
            FROM TSMUSERGRPMAP
            WHERE USER_ID = @p_user_id;
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

-- ============================================================
-- 8) USP_SM_USERAUTH_Q - DeptCd 출력 제거, 조인만 dept_id로 교체
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_USERAUTH_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_user_nm NVARCHAR(50) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
            SELECT  U.USER_ID AS UserId,
                        U.USER_NM AS UserNm,
                        U.EMP_NO AS EmpNo,
                        E.EMP_NM AS EmpNm,
                        D.DEPT_NM AS DeptNm,
                        U.EMAIL AS Email,
                        U.USE_YN AS UseYn,
                        U.DEVELOPER_YN AS DeveloperYn,
                        U.USER_TYPE AS UserType,
                        U.LAST_LOGIN_DT AS LastLoginDt
            FROM    TSMUSER U
                        LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
                        LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            WHERE (@p_user_id IS NULL OR U.USER_ID LIKE '%' + @p_user_id + '%')
              AND (@p_user_nm IS NULL OR U.USER_NM LIKE '%' + @p_user_nm + '%')
            ORDER BY U.REG_DT DESC;
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

-- ============================================================
-- 9) USP_SM_USERGRP_Q_2 - 조인만 dept_id로 교체(출력은 원래도 DeptNm뿐, 영향 없음)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_USERGRP_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_grp_cd VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
            SELECT U.USER_ID AS UserId, U.USER_NM AS UserNm, D.DEPT_NM AS DeptNm,
                   CASE WHEN M.USER_ID IS NULL THEN 0 ELSE 1 END AS IsMember
            FROM TSMUSER U
            LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
            LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
            LEFT JOIN TSMUSERGRPMAP M ON M.USER_ID = U.USER_ID AND M.USER_GRP_CD = @p_user_grp_cd
            WHERE U.USE_YN = 'Y'
            ORDER BY U.USER_NM;
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

-- ============================================================
-- 10) USP_SM_MENUAUTH_Q_2 - 조인만 dept_id로 교체(출력은 원래도 SubNm=DEPT_NM뿐, 영향 없음)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_MENUAUTH_Q_2
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_menu_id BIGINT,
    @p_target_type VARCHAR(10),
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
            IF @p_target_type = 'USER'
            BEGIN
                SELECT U.USER_ID AS TargetCd, U.USER_NM AS TargetNm, D.DEPT_NM AS SubNm,
                       ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                       ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                       ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                       ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                       ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                       ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                       ISNULL(A.AUTH10, 'N') AS Auth10
                FROM TSMUSER U
                LEFT JOIN TBAEMP E ON E.EMP_NO = U.EMP_NO
                LEFT JOIN TBADEPT D ON D.DEPT_ID = E.DEPT_ID
                LEFT JOIN TSMMENUAUTH A ON A.MENU_ID = @p_menu_id AND A.AUTH_TARGET_TYPE = 'USER' AND A.AUTH_TARGET_CD = U.USER_ID
                WHERE U.USE_YN = 'Y'
                ORDER BY U.USER_ID;
            END
            ELSE IF @p_target_type = 'GRP'
            BEGIN
                SELECT G.USER_GRP_CD AS TargetCd, G.USER_GRP_NM AS TargetNm,
                       CAST((SELECT COUNT(1) FROM TSMUSERGRPMAP M WHERE M.USER_GRP_CD = G.USER_GRP_CD) AS NVARCHAR(10)) + N'명' AS SubNm,
                       ISNULL(A.VIEW_YN, 'N') AS ViewYn, ISNULL(A.INSERT_YN, 'N') AS InsertYn,
                       ISNULL(A.SAVE_YN, 'N') AS UpdateYn, ISNULL(A.DELETE_YN, 'N') AS DeleteYn,
                       ISNULL(A.PRINT_YN, 'N') AS PrintYn, ISNULL(A.EXCEL_YN, 'N') AS ExcelYn,
                       ISNULL(A.AUTH01, 'N') AS Auth01, ISNULL(A.AUTH02, 'N') AS Auth02, ISNULL(A.AUTH03, 'N') AS Auth03,
                       ISNULL(A.AUTH04, 'N') AS Auth04, ISNULL(A.AUTH05, 'N') AS Auth05, ISNULL(A.AUTH06, 'N') AS Auth06,
                       ISNULL(A.AUTH07, 'N') AS Auth07, ISNULL(A.AUTH08, 'N') AS Auth08, ISNULL(A.AUTH09, 'N') AS Auth09,
                       ISNULL(A.AUTH10, 'N') AS Auth10
                FROM TSMUSERGRP G
                LEFT JOIN TSMMENUAUTH A ON A.MENU_ID = @p_menu_id AND A.AUTH_TARGET_TYPE = 'GRP' AND A.AUTH_TARGET_CD = G.USER_GRP_CD
                WHERE G.USE_YN = 'Y'
                ORDER BY G.USER_GRP_CD;
            END
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

-- ============================================================
-- 11) 팝업 메타데이터 - P_DEPT의 key_field/parent_field를 dept_id 기준으로,
--     P_DEPT/P_EMP의 표시/검색 목록에서 dept_cd 관련 행을 정리
-- ============================================================
UPDATE sysPopUpM SET key_field = 'DEPT_ID', parent_field = 'PAR_DEPT_ID' WHERE popup_key = 'P_DEPT';

UPDATE sysPopUpD SET column_nm = 'DEPT_ID', caption = N'부서ID', visible_yn = 'N'
WHERE popup_key = 'P_DEPT' AND column_nm = 'dept_cd';
UPDATE sysPopUpD SET column_nm = 'PAR_DEPT_ID'
WHERE popup_key = 'P_DEPT' AND column_nm = 'par_dept_cd';

DELETE FROM sysPopUpD WHERE popup_key = 'P_EMP' AND column_nm = 'dept_cd';

UPDATE sysPopUpS SET caption = N'부서명' WHERE popup_key = 'P_DEPT' AND param_nm = 'p_keyword';
DELETE FROM sysPopUpS WHERE popup_key = 'P_EMP' AND param_nm = 'p_dept_cd';
GO
