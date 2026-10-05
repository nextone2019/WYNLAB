-- 조회 프로시저에 사업장(@p_acc_id) 조건 추가 (2026-10-03)
--
-- 화면 표준: 모든 조회조건의 첫 번째는 사업장(LookUp L_ACC, Required, 기본값=로그인 사업장)이고,
-- acc_id 컬럼이 있는 데이터를 조회하는 프로시저는 첫 번째 파라미터로 @p_acc_id BIGINT를 받는다.
-- (기존에 이미 @p_acc_id가 있던 USP_MA_STOCK_Q / USP_MA_TRANSLIST_Q / USP_MA_POSTATUS_Q / USP_PR_PROC_Q /
--  USP_PR_RCV_Q / USP_PR_ROUTE_Q 등과 같은 WHERE (@p_acc_id IS NULL OR 별칭.acc_id = @p_acc_id) 형식)
--
-- 기본값 NULL = 전체(조건 없음)라서 아직 p_acc_id를 안 보내는 호출(결재함 더블클릭 포커스, 팝업 등)은
-- 그대로 동작한다. 조회(Q/L) 분기의 메인 WHERE만 바뀌었고 Q1/Q2/H 등 하위 분기는 그대로다.
-- CREATE OR ALTER라 여러 번 실행해도 안전하다. WYNLAB_DEV / FADU 양쪽에 같은 내용으로 적용한다.
-- (USP_PR_WO_Q는 두 DB 정의가 달라 255/256번으로 따로 둔다.)
-- ============================================================
-- USP_BA_DEPT_Q
-- ============================================================

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_DEPT_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_dept_id BIGINT = NULL,		/* Q1일 때만 사용 - 정확히 일치하는 부서 */
    @p_dept_nm VARCHAR(50) = NULL,    
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
                        a.acc_id, 
                        a.dept_id, 
                        a.dept_nm, 
                        a.par_dept_id, 
                        b.dept_nm as par_dept_nm, 
                        a.dept_type, 
                        a.remark
            FROM TBADEPT as a
                        LEFT OUTER JOIN TBADEPT as b on a.par_dept_id = b.dept_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND (@p_dept_nm IS NULL OR a.dept_nm LIKE '%' + @p_dept_nm + '%')
            ORDER BY a.DEPT_nm;
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
-- USP_BA_EMP_Q
-- ============================================================
-- USP_BA_EMP_Q 부서 검색 조건 수정 (2026-09-09)
--
-- DEPT_ID가 080 마이그레이션에서 VARCHAR 코드 -> BIGINT ID로 바뀌었는데, 이 프로시저의 WHERE절은
-- 옛날 방식 그대로 "a.DEPT_ID LIKE @p_dept_id + '%'"(부분일치 코드검색)를 쓰고 있었다. @p_dept_id가
-- BIGINT라서 '%' 문자열을 bigint로 변환하려다 항상 변환 오류가 나고, 그 오류가 TRY/CATCH에 먹혀
-- 결과 없이 빈 그리드만 나온다(조건을 비워도 항상 실패 - 실제로 겪음, 2026-09-09).
--
-- 다른 화면(081_TBADEPT_Procs.sql 등)처럼 정확일치(=)로 통일한다 - DEPT_ID는 이제 사람이 입력하는
-- 코드가 아니라 부서선택 팝업이 돌려주는 ID값이므로 부분일치가 애초에 의미가 없다.

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_EMP_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
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
                a.EMP_ID,
                a.acc_id,
                a.emp_no,
                a.emp_nm,
                a.emp_nm_eng,
                a.dept_id,
                b.dept_nm,
                a.ent_date,
                a.grp_ent_date,
                a.job_grade,
                a.job_type,
                ISNULL(a.ret_yn,'N') as ret_yn, 
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
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND       1 = 1
            AND         (@p_dept_id IS NULL OR a.DEPT_ID = @p_dept_id)
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
-- USP_BA_EMPLIST_Q
-- ============================================================
-- frmEmpList 검색조건(dept_id, emp_nm) 반영.
-- 직전 버전(사장님이 직접 수정)은 @p_dept_id/@p_emp_nm를 파라미터로만 추가하고 정작 WHERE절에서
-- 안 써서 검색이 동작하지 않았고, 기본값(= NULL)도 없어서 화면이 재배포되기 전(구버전 dll)까지는
-- "매개변수를 제공하지 않았습니다" 오류가 났다. 둘 다 여기서 같이 고친다.

CREATE OR ALTER PROCEDURE USP_BA_EMPLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    @p_dept_id BIGINT = NULL,
    @p_emp_nm VARCHAR(30) = NULL,
    ---------------------------------------------------------------------------------------------------
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
            SELECT  a.emp_no,
                    a.emp_nm,
                    a.emp_nm_eng,
                    a.ent_date,
                    a.grp_ent_date,
                    a.job_grade,
                    a.job_type,
                    a.ret_yn,
                    a.ret_date,
                    a.sex_cd,
                    a.tel,
                    a.email,
                    a.EMP_ID,
                    a.dept_id,
                    b.dept_nm
            FROM    TBAEMP AS a
                    JOIN TBADEPT AS b ON a.dept_id = b.dept_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND   (@p_dept_id IS NULL OR a.dept_id = @p_dept_id)
                AND (@p_emp_nm IS NULL OR a.emp_nm LIKE '%' + @p_emp_nm + '%')
            ORDER BY EMP_ID;
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
-- USP_BA_ITEMGRP_Q
-- ============================================================

-- USP_BA_ITEMGRP_Q/_S 전면 재작성 - 이 두 프로시저는 이번 rename과 무관하게 이미 026 마이그레이션
-- 시절의 옛 컬럼명(item_class_cd/item_class_nm/par_item_class_cd)을 그대로 참조하고 있어서,
-- TBAITEMGRP가 grp_id/grp_nm/par_grp_id로 재설계된 뒤로 계속 깨져 있던 상태였다(CREATE OR ALTER
-- 자체가 "잘못된 열 이름"으로 실패 - 실제로 이번 마이그레이션 적용 중 확인함). 아직 frmItemGrp
-- 화면이 없어 호출하는 곳이 없으므로(BA 모듈 어디에도 참조 없음, 확인함) 동작을 바꿔도 위험이
-- 없다 - 구조가 거의 동일한 USP_BA_DEPT_Q/_S(자기참조 계층형 테이블, par_dept_id/dept_nm 패턴)를
-- 그대로 본떠 지금의 실제 TBAITEMGRP 스키마(grp_id/grp_nm/grp_lvl/par_grp_id/remark/acc_id)에
-- 맞춰 새로 짰다.
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMGRP_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    --@p_grp_id BIGINT = NULL,
    @p_grp_nm NVARCHAR(30) = NULL,
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
                        a.acc_id,
                        a.grp_id,
                        a.grp_nm,
                        a.grp_lvl,
                        a.par_grp_id,
                        b.grp_nm as par_grp_nm,
                        a.remark
            FROM TBAITEMGRP as a
                        LEFT OUTER JOIN TBAITEMGRP as b on a.par_grp_id = b.grp_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND 1 = 1
            --AND (@p_grp_id IS NULL OR a.grp_id = @p_grp_id)
              AND (@p_grp_nm IS NULL OR a.grp_nm LIKE '%' + @p_grp_nm + '%')
            ORDER BY a.grp_id;
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
-- USP_BA_ITEMLIST_Q
-- ============================================================

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMLIST_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
	---------------------------------------------------------------------------------------------------------
    @p_item_no VARCHAR(100) = NULL,
    --@p_item_nm NVARCHAR(100) = NULL,
	--@p_item_spec NVARCHAR(100) = NULL,
	@p_stat_cd	 VARCHAR(10) = NULL,
	@p_asset_type	 VARCHAR(10) = NULL,
	---------------------------------------------------------------------------------------------------------
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
            SELECT	a.item_id, 
						a.item_no, 
						a.item_nm, 
						a.item_spec, 
						a.asset_type, 
						a.unit_cd, 
						a.wh_id, 
						d.wh_nm, 
						a.loc_id, 
						e.loc_nm, 
						a.safe_qty,
						a.dept_id, 
						b.dept_nm, 
						a.emp_id, 
						c.emp_nm, 
						a.stock_yn, 
						a.stat_cd, 
						a.remark
            FROM	TBAITEM a
						LEFT JOIN TBADEPT b ON a.DEPT_ID = b.DEPT_ID
						LEFT JOIN TBAEMP c ON a.EMP_ID = c.EMP_ID
						LEFT OUTER JOIN TBAWH as d on d.wh_id = a.wh_id
						LEFT OUTER JOIN TBALOC as e on e.loc_id = a.loc_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND ((@p_item_no IS NULL OR a.item_no LIKE '%' + @p_item_no + '%')
							OR  (@p_item_no IS NULL OR a.item_nm LIKE '%' + @p_item_no + '%')
							OR  (@p_item_no IS NULL OR a.item_spec LIKE '%' + @p_item_no + '%') )
			  AND (@p_stat_cd IS NULL OR @p_stat_cd  ='' OR  a.stat_cd LIKE @p_stat_cd +'%')
			  AND (@p_asset_type IS NULL OR @p_asset_type='' OR  a.asset_type LIKE @p_asset_type +'%')
            ORDER BY a.item_no;
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
-- USP_BA_WH_Q
-- ============================================================

-- ============================================================
-- 3) USP_BA_WH_Q - frmWh 마스터(창고) 목록 + Q1(선택된 창고의 위치 목록, grd2용).
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_WH_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_wh_id BIGINT = NULL,
    @p_wh_nm NVARCHAR(200) = NULL,
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
                a.wh_id, a.wh_nm, a.acc_id, a.wh_type,
                a.dept_id, b.dept_nm,
                a.emp_id, c.emp_nm
            FROM        TBAWH as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND       1 = 1
            AND         (@p_wh_nm IS NULL OR a.wh_nm LIKE '%' + @p_wh_nm + '%')
            ORDER BY a.wh_id;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT loc_id, loc_nm, loc_type, wh_id
            FROM TBALOC
            WHERE wh_id = @p_wh_id
            ORDER BY loc_id;
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
-- USP_BA_ITEM_Q
-- ============================================================
-- 품목일괄등록(frmItemMulti) 지원 - 2026-09-16.
-- 화면 자체는 새 저장 프로시저 없이 기존 USP_BA_ITEM_S(N)를 행마다 그대로 재사용한다(TplSingleGrid
-- 패턴과 동일 - "여러 줄 입력 후 한 번에 저장"일 뿐 저장 자체는 frmItem의 신규 등록과 완전히 같은
-- 동작이어야 하므로 프로시저를 분리하지 않는다). 창고/위치/담당부서/담당자/구매처는 팝업
-- 프레임워크(api/lookups/*), 품목그룹1~4는 콤보 프레임워크(api/combo-lookups/*, L_ITEM_GRP)를
-- 그대로 재사용해서 이름->ID를 화면(클라이언트)에서 직접 조회/검증한다 - 둘 다 메뉴의
-- PROC_PREFIX 화이트리스트에 안 걸리는(sysPopUpM/sysLookupM 메타데이터 기반) 별도 통로라 이
-- 화면이 USP_BA_WH_Q 등을 직접 호출할 필요가 없다.
--
-- 여기서 새로 필요한 것은 딱 하나: "붙여넣은/업로드한 품목코드가 이미 DB에 있는지" 한 번에
-- 확인하는 것 - 기존 USP_BA_ITEM_Q의 Q(단건 LIKE 검색)로는 여러 개를 정확히 비교할 수 없어서
-- work_type='Q2'를 추가한다(콤마로 구분된 품목코드 목록 -> STRING_SPLIT으로 정확히 일치하는
-- 것만 돌려줌).

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    @p_item_no VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    @p_item_no_list NVARCHAR(MAX) = NULL,
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
                a.acc_id,
                a.item_id,
                a.item_no,
                a.item_nm,
                a.item_spec,
                a.unit_cd,
                a.po_unit_cd,
                a.wh_id,
                e.wh_nm,
                a.loc_id,
                f.loc_nm,
                a.safe_qty,
                a.dept_id,
                b.dept_nm,
                a.emp_id,
                c.emp_no,
                c.emp_nm,
                a.cust_id,
                d.cust_nm,
                a.asset_type,
                a.out_type,
                a.po_qc_yn,
                a.prod_qc_yn,
                a.lot_yn,
                a.stock_yn,
                a.stat_cd,
                a.grp1_id,
                a.grp2_id,
                a.grp3_id,
                a.grp4_id,
                a.remark
            FROM        TBAITEM as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
                        LEFT OUTER JOIN TBACUST as d on a.cust_id = d.cust_id
                        LEFT OUTER JOIN TBAWH as e on a.wh_id = e.wh_id
                        LEFT OUTER JOIN TBALOC as f on a.loc_id = f.loc_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND       1 = 1
   --         AND         (@p_item_id IS NULL OR a.item_id = @p_item_id)
            AND         (@p_item_no IS NULL OR a.item_no LIKE '%' + @p_item_no + '%')
            AND         (@p_item_nm IS NULL OR a.item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY a.item_id;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            -- frmItemMulti(품목일괄등록)의 붙여넣기/업로드 검증용 - 콤마로 구분된 품목코드 목록
            -- 중 이미 TBAITEM에 존재하는 것만(정확히 일치) 돌려준다.
            SELECT a.item_no, a.item_nm
            FROM TBAITEM a
            WHERE a.item_no IN (SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@p_item_no_list, ','));
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
-- USP_MA_DELV_Q
-- ============================================================

-- ============================================================
-- 2) USP_MA_DELV_Q - 헤더(0번) + 라인(1번)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELV_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_delv_id BIGINT = NULL,
    @p_delv_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = delv_id
            FROM TMADELVM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_delv_id IS NULL OR delv_id = @p_delv_id)
              AND (@p_delv_id IS NOT NULL OR @p_delv_no IS NULL OR delv_no LIKE '%' + @p_delv_no + '%')
            ORDER BY delv_id DESC;

            -- 0) 헤더
            SELECT
                m.delv_id, m.acc_id, a.ACC_NM,
                m.delv_no, m.delv_date,
                m.cust_id, c.cust_nm, m.vendor_doc_no,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.remark
            FROM TMADELVM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.delv_id = @match_id;

            -- 1) 라인
            SELECT
                dt.delv_id, dt.serl, dt.acc_id, dt.delv_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                dt.delv_qty, ISNULL(dt.next_qty, 0) AS next_qty, dt.lot_no, dt.qc_yn,
                CASE WHEN ISNULL(dt.qc_yn, 'N') = 'N' THEN N'면제'
                     WHEN ISNULL(dt.next_qty, 0) = 0 THEN N'검사대기'
                     WHEN ISNULL(dt.next_qty, 0) < dt.delv_qty THEN N'검사중'
                     ELSE N'검사완료' END AS qc_stat_nm,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                pd.qty AS po_qty, ISNULL(pd.next_qty, 0) AS po_next_qty, (pd.qty - ISNULL(pd.next_qty, 0)) AS po_remain_qty,
                pd.delv_date AS po_delv_date,
                dt.stop_yn, dt.remark
            FROM TMADELVD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TMAPOD pd ON pd.po_id = dt.src_id AND pd.serl = dt.src_serl
            WHERE dt.delv_id = @match_id
            ORDER BY dt.serl;
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
-- USP_MA_DELVLIST_Q
-- ============================================================

-- ============================================================
-- 6) USP_MA_DELVLIST_Q - 납품현황. Q: 헤더 목록(조건 검색), Q1: 한 납품의 라인(검사상태 포함)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELVLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_delv_id BIGINT = NULL,
    @p_delv_no VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 거래처명 / 품번 / 품명 */
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
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
                m.delv_id, m.delv_no, m.delv_date, m.stat_cd,
                m.cust_id, c.cust_nm, m.vendor_doc_no,
                d.dept_nm, e.emp_nm, m.cfm_dt,
                (SELECT COUNT(*) FROM TMADELVD x WHERE x.delv_id = m.delv_id) AS line_cnt,
                m.remark
            FROM TMADELVM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_delv_no IS NULL OR @p_delv_no = '' OR m.delv_no LIKE '%' + @p_delv_no + '%')
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.delv_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.delv_date <= @p_date_to)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR c.cust_nm LIKE '%' + @p_keyword + '%'
                   OR EXISTS (SELECT 1 FROM TMADELVD x JOIN TBAITEM i ON i.item_id = x.item_id
                              WHERE x.delv_id = m.delv_id
                                AND (i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')))
            ORDER BY m.delv_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.delv_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                dt.delv_qty, ISNULL(dt.next_qty, 0) AS next_qty, dt.lot_no, dt.qc_yn,
                CASE WHEN ISNULL(dt.qc_yn, 'N') = 'N' THEN N'면제'
                     WHEN ISNULL(dt.next_qty, 0) = 0 THEN N'검사대기'
                     WHEN ISNULL(dt.next_qty, 0) < dt.delv_qty THEN N'검사중'
                     ELSE N'검사완료' END AS qc_stat_nm,
                dt.src_no AS po_no, dt.src_serl AS po_serl, w.wh_nm, l.loc_nm, dt.remark
            FROM TMADELVD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.delv_id = @p_delv_id
            ORDER BY dt.serl;
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
-- USP_MA_GR_Q
-- ============================================================

-- ============================================================
-- 2) USP_MA_GR_Q - 헤더(0번) + 라인(1번). p_trans_type이 있으면 그 유형의 입고만(구매입고 화면=PU_IN)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GR_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
    @p_gr_no VARCHAR(20) = NULL,
    @p_trans_type VARCHAR(10) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = gr_id
            FROM TMAGRM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_gr_id IS NULL OR gr_id = @p_gr_id)
              AND (@p_gr_id IS NOT NULL OR @p_gr_no IS NULL OR gr_no LIKE '%' + @p_gr_no + '%')
              AND (@p_trans_type IS NULL OR @p_trans_type = '' OR trans_type = @p_trans_type)
            ORDER BY gr_id DESC;

            -- 0) 헤더
            SELECT
                m.gr_id, m.acc_id, a.ACC_NM,
                m.gr_no, m.gr_date, m.trans_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.auto_yn,
                m.remark
            FROM TMAGRM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.gr_id = @match_id;

            -- 1) 라인 (ready_qty/remain_qty는 입고대기 뷰 기준 - 미확정 작성본을 고칠 때 참고)
            SELECT
                dt.gr_id, dt.serl, dt.acc_id, dt.gr_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                dt.gr_qty, ISNULL(dt.next_qty, 0) AS next_qty, dt.lot_no,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm, dt.stock_yn,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.po_id, dt.po_no, dt.po_serl,
                v.ready_qty, v.remain_qty AS ready_remain_qty,
                dt.trans_id, dt.remark
            FROM TMAGRD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN dbo.VMA_GR_READY v ON v.src_type = dt.src_type AND v.src_id = dt.src_id AND v.src_serl = dt.src_serl
            WHERE dt.gr_id = @match_id
            ORDER BY dt.serl;
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
-- USP_MA_GRLIST_Q
-- ============================================================

-- ============================================================
-- 10) USP_MA_GRLIST_Q - 입고현황. Q: 헤더 목록, Q1: 한 입고의 라인
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_GRLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_gr_id BIGINT = NULL,
    @p_gr_no VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 거래처명 / 품번 / 품명 */
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_trans_type VARCHAR(10) = NULL,
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
                m.gr_id, m.gr_no, m.gr_date, m.trans_type, m.stat_cd, m.auto_yn,
                m.cust_id, c.cust_nm, d.dept_nm, e.emp_nm, m.cfm_dt,
                (SELECT COUNT(*) FROM TMAGRD x WHERE x.gr_id = m.gr_id) AS line_cnt,
                (SELECT ISNULL(SUM(x.gr_qty), 0) FROM TMAGRD x WHERE x.gr_id = m.gr_id) AS total_qty,
                m.remark
            FROM TMAGRM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_gr_no IS NULL OR @p_gr_no = '' OR m.gr_no LIKE '%' + @p_gr_no + '%')
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.gr_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.gr_date <= @p_date_to)
              AND (@p_trans_type IS NULL OR @p_trans_type = '' OR m.trans_type = @p_trans_type)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR c.cust_nm LIKE '%' + @p_keyword + '%'
                   OR EXISTS (SELECT 1 FROM TMAGRD x JOIN TBAITEM i ON i.item_id = x.item_id
                              WHERE x.gr_id = m.gr_id
                                AND (i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')))
            ORDER BY m.gr_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.gr_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd,
                dt.gr_qty, ISNULL(dt.next_qty, 0) AS next_qty, dt.lot_no, w.wh_nm, l.loc_nm, dt.stock_yn,
                dt.src_type, dt.src_no, dt.po_no, dt.remark
            FROM TMAGRD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.gr_id = @p_gr_id
            ORDER BY dt.serl;
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
-- USP_MA_IQC_Q
-- ============================================================
-- 구매 프로세스 P2/P3 - 수입검사(TMAIQCM/D) 프로시저, 입고대기 뷰, 구매진행현황(2026-09-25).
--
--  1) USP_MA_IQC_Q            - 수입검사 헤더 + 라인(0/1번 결과셋) - frmIqc
--  2) USP_MA_IQC_S            - 헤더 N/U/D (작성 상태에서만)
--  3) USP_MA_IQC_S_1          - 라인 N/U/D (납품 라인에서 불러온 것만) - 합격+특채+불합격=검사수량, 특채 사유, 불합격 사유/처분 검증
--  4) USP_MA_IQC_C_S          - 확정(C)/확정취소(CC) - 납품 라인 next_qty와 발주 라인 next_qty(반품분 복원) 재계산
--  5) USP_MA_IQCLIST_Q        - 수입검사현황 Q/Q1 - frmIqcList
--  6) USP_MA_DELVIQCPICK_Q    - frmIqc "검사대기 불러오기" 팝업용 - 확정 납품 중 검사대상이고 미검사 잔량이 있는 라인
--  7) VMA_GR_READY            - 입고대기 뷰(입고 설계와의 인계 지점, 설계서 7장)
--  8) USP_MA_POSTATUS_Q       - 구매진행현황 - frmPoStatus

-- ============================================================
-- 1) USP_MA_IQC_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQC_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_iqc_id BIGINT = NULL,
    @p_iqc_no VARCHAR(20) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = iqc_id
            FROM TMAIQCM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_iqc_id IS NULL OR iqc_id = @p_iqc_id)
              AND (@p_iqc_id IS NOT NULL OR @p_iqc_no IS NULL OR iqc_no LIKE '%' + @p_iqc_no + '%')
            ORDER BY iqc_id DESC;

            -- 0) 헤더
            SELECT
                m.iqc_id, m.acc_id, a.ACC_NM,
                m.iqc_no, m.iqc_date,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.remark
            FROM TMAIQCM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.iqc_id = @match_id;

            -- 1) 라인
            SELECT
                dt.iqc_id, dt.serl, dt.acc_id, dt.iqc_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, dt.lot_no,
                dl.delv_qty, dt.insp_qty, dt.sample_qty, dt.pass_qty, dt.conc_qty, dt.fail_qty,
                dt.fail_reason_cd, dt.fail_action_cd, ISNULL(dt.next_qty, 0) AS next_qty,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dl.src_no AS po_no,
                dt.remark
            FROM TMAIQCD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TMADELVD dl ON dl.delv_id = dt.src_id AND dl.serl = dt.src_serl
            WHERE dt.iqc_id = @match_id
            ORDER BY dt.serl;
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
-- USP_MA_IQCLIST_Q
-- ============================================================

-- ============================================================
-- 5) USP_MA_IQCLIST_Q - 수입검사현황. Q: 헤더 목록, Q1: 한 검사의 라인
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQCLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_iqc_id BIGINT = NULL,
    @p_iqc_no VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 거래처명 / 품번 / 품명 */
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
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
                m.iqc_id, m.iqc_no, m.iqc_date, m.stat_cd,
                m.cust_id, c.cust_nm, d.dept_nm, e.emp_nm, m.cfm_dt,
                (SELECT COUNT(*) FROM TMAIQCD x WHERE x.iqc_id = m.iqc_id) AS line_cnt,
                (SELECT ISNULL(SUM(x.insp_qty), 0) FROM TMAIQCD x WHERE x.iqc_id = m.iqc_id) AS insp_qty,
                (SELECT ISNULL(SUM(x.pass_qty + x.conc_qty), 0) FROM TMAIQCD x WHERE x.iqc_id = m.iqc_id) AS ok_qty,
                (SELECT ISNULL(SUM(x.fail_qty), 0) FROM TMAIQCD x WHERE x.iqc_id = m.iqc_id) AS fail_qty,
                m.remark
            FROM TMAIQCM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_iqc_no IS NULL OR @p_iqc_no = '' OR m.iqc_no LIKE '%' + @p_iqc_no + '%')
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.iqc_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.iqc_date <= @p_date_to)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR c.cust_nm LIKE '%' + @p_keyword + '%'
                   OR EXISTS (SELECT 1 FROM TMAIQCD x JOIN TBAITEM i ON i.item_id = x.item_id
                              WHERE x.iqc_id = m.iqc_id
                                AND (i.item_no LIKE '%' + @p_keyword + '%' OR i.item_nm LIKE '%' + @p_keyword + '%')))
            ORDER BY m.iqc_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.iqc_id, dt.serl, i.item_no, i.item_nm, i.item_spec, dt.unit_cd, dt.lot_no,
                dt.insp_qty, dt.sample_qty, dt.pass_qty, dt.conc_qty, dt.fail_qty,
                CASE WHEN dt.insp_qty > 0 THEN CAST(ROUND((dt.pass_qty + dt.conc_qty) * 100.0 / dt.insp_qty, 1) AS NUMERIC(9,1)) END AS pass_rate,
                dt.fail_reason_cd, dt.fail_action_cd, ISNULL(dt.next_qty, 0) AS next_qty,
                dt.src_no AS delv_no, dl.src_no AS po_no, w.wh_nm, l.loc_nm, dt.remark
            FROM TMAIQCD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
                LEFT JOIN TMADELVD dl ON dl.delv_id = dt.src_id AND dl.serl = dt.src_serl
            WHERE dt.iqc_id = @p_iqc_id
            ORDER BY dt.serl;
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
-- USP_MA_PO_Q
-- ============================================================

-- ============================================================
-- 7) USP_MA_PO_Q - t.stat_cd -> t.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_po_no VARCHAR(20) = NULL,
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
            DECLARE @match_po_id BIGINT;
            SELECT TOP 1 @match_po_id = po_id
            FROM TMAPOM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_po_id IS NULL OR po_id = @p_po_id)
              AND (@p_po_id IS NOT NULL OR @p_po_no IS NULL OR po_no LIKE '%' + @p_po_no + '%')
            ORDER BY po_id DESC;

            -- 0) 헤더
            SELECT
                m.po_id, m.acc_id, a.ACC_NM,
                m.po_no, m.po_date,
                m.stat_cd, m.po_type, m.po_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.delv_date, m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cmf_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.remark
            FROM TMAPOM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.po_id = @match_po_id;

            -- 1) 품목 상세
            SELECT
                dt.po_id, dt.serl, dt.acc_id, dt.po_no, dt.po_type,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.qc_yn, dt.stock_yn, dt.stock_unit_cd, dt.stock_unit_qty,
                dt.pjt_id, dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.stop_yn, dt.stop_emp_no, dt.stop_remark, dt.remark
            FROM TMAPOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.po_id = @match_po_id
            ORDER BY dt.serl;
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
-- USP_MA_POLIST_Q
-- ============================================================

-- ============================================================
-- 8) USP_MA_POLIST_Q - t.stat_cd -> t.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_po_no VARCHAR(20) = NULL,
    @p_po_title NVARCHAR(1000) = NULL,
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
                m.po_id, m.po_no, m.po_date, m.po_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TMAPOM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND 1 = 1
              AND (@p_po_no IS NULL OR m.po_no LIKE '%' + @p_po_no + '%')
              AND (@p_po_title IS NULL OR m.po_title LIKE '%' + @p_po_title + '%')
            ORDER BY m.po_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.po_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.amt, dt.total_amt,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TMAPOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
            WHERE dt.po_id = @p_po_id
            ORDER BY dt.serl;
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
-- USP_MA_POREQ_Q
-- ============================================================

CREATE OR ALTER PROCEDURE USP_MA_POREQ_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
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
            DECLARE @match_req_id BIGINT;
            SELECT TOP 1 @match_req_id = req_id
            FROM TMAPOREQM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_req_id IS NULL OR req_id = @p_req_id)
              AND (@p_req_id IS NOT NULL OR @p_req_no IS NULL OR req_no LIKE '%' + @p_req_no + '%')
            ORDER BY req_id DESC;

            -- 0) 헤더
            SELECT
                m.req_id, m.acc_id, a.ACC_NM,
                m.req_no, m.req_date, m.req_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.amt, m.vat, m.total_amt,
                m.remark
            FROM TMAPOREQM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.req_id = @match_req_id;

            -- 1) 품목 행
            SELECT
                dt.req_id, dt.serl, dt.acc_id, dt.req_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.amt, dt.vat_rate, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.cfm_yn, dt.stop_yn,
                dt.cust_id, c2.cust_nm,
                dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.remark
            FROM TMAPOREQD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBACUST c2 ON c2.cust_id = dt.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.req_id = @match_req_id
            ORDER BY dt.serl;
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
-- USP_MA_POREQLIST_Q
-- ============================================================

-- ============================================================
-- 6) USP_MA_POREQLIST_Q - t.stat_cd -> t.app_stat_cd
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_req_no VARCHAR(20) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
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
                m.req_id, m.req_no, m.req_date, m.req_title,
                m.stat_cd, m.po_type,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TMAPOREQM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND 1 = 1
              AND (@p_req_no IS NULL OR m.req_no LIKE '%' + @p_req_no + '%')
              AND (@p_req_title IS NULL OR m.req_title LIKE '%' + @p_req_title + '%')
            ORDER BY m.req_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.req_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TMAPOREQD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
            WHERE dt.req_id = @p_req_id
            ORDER BY dt.serl;
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
-- USP_PR_LOTTRACE_Q
-- ============================================================

-- LOT계보조회 이력의 출하 이벤트를 LOT 상세(TSAGIL) 기준으로 (244의 프로시저에서 출하 블록만 교체)
CREATE OR ALTER PROCEDURE USP_PR_LOTTRACE_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lot_no NVARCHAR(50) = NULL,          /* L 전용: LOT번호 일부 */
    @p_wo_no VARCHAR(20) = NULL,            /* L 전용: 작업지시번호 일부 */
    @p_lot_id BIGINT = NULL,                /* H 전용 */
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
        IF @p_work_type = 'L'
        BEGIN
            IF ISNULL(@p_lot_no, '') = '' AND ISNULL(@p_wo_no, '') = ''
            BEGIN
                -- 조건이 없으면 전체 계보를 끌어오지 않고 빈 결과를 돌려준다(LOT번호나 작업지시번호를 넣어야 함).
                SELECT CAST(NULL AS BIGINT) AS lot_id WHERE 1 = 0;
                RETURN;
            END

            ;WITH hit AS (
                SELECT l.lot_id FROM TPRLOT l LEFT JOIN TPRWOM w ON w.wo_id = l.wo_id
                WHERE (@p_acc_id IS NULL OR l.acc_id = @p_acc_id)
                  AND (ISNULL(@p_lot_no, '') = '' OR l.lot_no LIKE '%' + @p_lot_no + '%')
                  AND (ISNULL(@p_wo_no, '') = '' OR w.wo_no LIKE '%' + @p_wo_no + '%')
            ),
            up AS (
                SELECT lot_id FROM hit
                UNION ALL
                SELECT r.parent_lot_id FROM up JOIN TPRLOTREL r ON r.child_lot_id = up.lot_id
            ),
            roots AS (
                SELECT DISTINCT u.lot_id FROM up u
                WHERE NOT EXISTS (SELECT 1 FROM TPRLOTREL r WHERE r.child_lot_id = u.lot_id)
            ),
            tree AS (
                SELECT l.lot_id, 0 AS depth, CAST(NULL AS BIGINT) AS rslt_id,
                       CAST(RIGHT('0000000000' + CAST(l.lot_id AS VARCHAR(10)), 10) AS VARCHAR(900)) AS path
                FROM TPRLOT l JOIN roots ro ON ro.lot_id = l.lot_id
                UNION ALL
                SELECT c.lot_id, t.depth + 1, r.rslt_id,
                       CAST(t.path + '.' + RIGHT('0000000000' + CAST(c.lot_id AS VARCHAR(10)), 10) AS VARCHAR(900))
                FROM tree t
                    JOIN TPRLOTREL r ON r.parent_lot_id = t.lot_id
                    JOIN TPRLOT c ON c.lot_id = r.child_lot_id
            )
            SELECT
                t.lot_id, t.depth,
                REPLICATE(N'    ', t.depth) + CASE WHEN t.depth > 0 THEN N'└ ' ELSE N'' END + l.lot_no AS lot_disp,
                l.lot_no, i.item_no, i.item_nm, l.unit_cd, l.init_qty,
                w.wo_no,
                CASE WHEN l.wo_serl IS NULL OR l.wo_serl = 0 THEN N'입고(시작 LOT)' ELSE p.proc_nm END AS gen_proc_nm,
                rs.rslt_no,
                st.wh_nm, st.stock_qty,
                CASE WHEN EXISTS (SELECT 1 FROM hit h WHERE h.lot_id = t.lot_id) THEN 1 ELSE 0 END AS hit
            FROM tree t
                JOIN TPRLOT l ON l.lot_id = t.lot_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TPRWOM w ON w.wo_id = l.wo_id
                LEFT JOIN TPRWOD d ON d.wo_id = l.wo_id AND d.serl = l.wo_serl
                LEFT JOIN TBAPROC p ON p.acc_id = l.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TPRRSLTM rs ON rs.rslt_id = t.rslt_id
                OUTER APPLY (SELECT TOP 1 wh.wh_nm, s.stock_qty
                             FROM TMASTOCK s JOIN TBAWH wh ON wh.wh_id = s.wh_id
                             WHERE s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.stock_qty > 0
                             ORDER BY s.stock_qty DESC) st
            ORDER BY t.path
            OPTION (MAXRECURSION 200);
        END
        ELSE IF @p_work_type = 'H'
        BEGIN
            SELECT evt_kind, doc_type, doc_id, doc_no, evt_date, descr, qty, stat_nm FROM (
                -- 웨이퍼 입고(이 LOT 번호로 들어온 입고 문서)
                SELECT N'입고' AS evt_kind, 'RV' AS doc_type, v.rcv_id AS doc_id, v.rcv_no AS doc_no, v.rcv_date AS evt_date,
                       N'웨이퍼 입고 → ' + ISNULL(wh.wh_nm, N'') AS descr, v.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = v.stat_cd) AS stat_nm
                FROM TPRLOT l JOIN TPRRCV v ON v.acc_id = l.acc_id AND v.item_id = l.item_id AND v.lot_no = l.lot_no
                    LEFT JOIN TBAWH wh ON wh.wh_id = v.wh_id
                WHERE l.lot_id = @p_lot_id
                UNION ALL
                -- 공정 산출: 이 LOT를 만든 실적
                SELECT N'공정 산출', 'RS', m.rslt_id, m.rslt_no, m.rslt_date,
                       ISNULL(p.proc_nm, m.proc_cd) + N' 산출 (' + ISNULL(c.cust_nm, N'') + N', 투입 LOT ' + ISNULL(pl.lot_no, N'') + N')', r.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = m.stat_cd)
                FROM TPRLOTREL r JOIN TPRRSLTM m ON m.rslt_id = r.rslt_id
                    LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                    LEFT JOIN TPRLOT pl ON pl.lot_id = r.parent_lot_id
                WHERE r.child_lot_id = @p_lot_id
                UNION ALL
                -- 공정 투입: 이 LOT를 투입한 실적
                SELECT N'공정 투입', 'RS', m.rslt_id, m.rslt_no, m.rslt_date,
                       ISNULL(p.proc_nm, m.proc_cd) + N' 투입 (' + ISNULL(c.cust_nm, N'') + N', 양품 ' + CONVERT(NVARCHAR(30), CAST(m.good_qty AS DECIMAL(18,4)), 1) + N')', m.in_qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = m.stat_cd)
                FROM TPRRSLTM m
                    LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                WHERE m.in_lot_id = @p_lot_id
                UNION ALL
                -- 외주이전
                SELECT N'외주이전', 'XF', x.xfer_id, x.xfer_no, x.xfer_date,
                       ISNULL(fc.cust_nm, N'') + N' → ' + ISNULL(tc.cust_nm, N'') + N' (도착 ' + CONVERT(NVARCHAR(30), CAST(ISNULL(d.in_qty, 0) AS DECIMAL(18,4)), 1) + N')', d.out_qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0003' AND minor_cd = x.stat_cd)
                FROM TPRXFERD d JOIN TPRXFERM x ON x.xfer_id = d.xfer_id
                    LEFT JOIN TBACUST fc ON fc.cust_id = x.from_cust_id
                    LEFT JOIN TBACUST tc ON tc.cust_id = x.to_cust_id
                WHERE d.lot_id = @p_lot_id
                UNION ALL
                -- 출하(영업 출하등록 TSAGIM/D/L) - 이 LOT가 고객에게 나간 이력(LOT 상세 행 기준)
                SELECT N'출하', 'GI', g.gi_id, g.gi_no, g.gi_date,
                       N'출하 → ' + ISNULL(c.cust_nm, N'') + N' (' + ISNULL(w.wh_nm, N'') + N', 수주 ' + ISNULL(so.so_no, N'') + N')', l.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = g.stat_cd)
                FROM TSAGIL l JOIN TSAGIM g ON g.gi_id = l.gi_id
                    JOIN TSAGID d ON d.gi_id = l.gi_id AND d.serl = l.serl
                    LEFT JOIN TSASOM so ON so.so_id = d.so_id
                    LEFT JOIN TBACUST c ON c.cust_id = g.cust_id
                    LEFT JOIN TBAWH w ON w.wh_id = l.wh_id
                WHERE l.lot_id = @p_lot_id
            ) e
            ORDER BY evt_date, doc_no;
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
-- USP_PR_RSLT_Q
-- ============================================================

CREATE OR ALTER PROCEDURE USP_PR_RSLT_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
    @p_rslt_no VARCHAR(20) = NULL,
    @p_wo_id BIGINT = NULL,                 /* LOT 조회용 */
    @p_wo_serl INT = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = rslt_id FROM TPRRSLTM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_rslt_id IS NULL OR rslt_id = @p_rslt_id)
              AND (@p_rslt_id IS NOT NULL OR @p_rslt_no IS NULL OR rslt_no LIKE '%' + @p_rslt_no + '%')
            ORDER BY rslt_id DESC;

            SELECT
                m.rslt_id, m.acc_id, m.rslt_no, m.rslt_date,
                m.wo_id, m.wo_no, wm.start_lot_no, m.wo_serl, m.proc_cd, p.proc_nm,
                m.cust_id, c.cust_nm, m.wh_id, w.wh_nm,
                m.in_lot_id, l.lot_no AS in_lot_no, d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd,
                d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd, d.split_qty,
                m.in_qty, m.good_qty, m.bad_qty, m.yield_rate, m.src_file_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TPRRSLTM m
                LEFT JOIN TPRWOM wm ON wm.wo_id = m.wo_id
                LEFT JOIN TPRWOD d ON d.wo_id = m.wo_id AND d.serl = m.wo_serl
                LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = m.wh_id
                LEFT JOIN TPRLOT l ON l.lot_id = m.in_lot_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE m.rslt_id = @match_id;

            SELECT rslt_id, serl, acc_id, wafer_no, good_qty, bad_qty, good_qty + bad_qty AS gross_qty, remark
            FROM TPRRSLTD WHERE rslt_id = @match_id ORDER BY serl;

            SELECT r.parent_lot_id, r.child_lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, r.qty
            FROM TPRLOTREL r
                JOIN TPRLOT l ON l.lot_id = r.child_lot_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE r.rslt_id = @match_id ORDER BY l.lot_no;
        END
        ELSE IF @p_work_type = 'LOT'
        BEGIN
            -- 그 공정의 외주처 창고에 재고가 있는 투입품목 LOT(이 작업지시 소속)
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.stock_qty
            FROM TPRWOD d
                JOIN TPRLOT l ON l.wo_id = d.wo_id AND l.item_id = d.in_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = d.wh_id AND s.loc_id = 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE d.wo_id = @p_wo_id AND d.serl = @p_wo_serl AND s.stock_qty > 0
            ORDER BY l.lot_no;
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
-- USP_PR_RSLTSTAT_Q
-- ============================================================
-- 생산관리(PR) 공정실적현황 / 외주이전현황 조회 화면용 목록 프로시저 + 메뉴 (2026-09-30)
-- 상세(산출 LOT/웨이퍼 판정/이전 LOT)는 기존 USP_PR_RSLT_Q / USP_PR_XFER_Q 'Q'를 그대로 쓴다.

CREATE OR ALTER PROCEDURE USP_PR_RSLTSTAT_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50) = 'L',
    ---------------------------------------------------------------------------------------------------
    @p_fr_date VARCHAR(8) = NULL,
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_proc_cd VARCHAR(20) = NULL,
    @p_rslt_no VARCHAR(20) = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,          /* 투입 LOT 또는 산출 LOT */
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
        SELECT
            m.rslt_id, m.rslt_no, m.rslt_date, m.wo_id, m.wo_no, m.wo_serl, m.proc_cd, p.proc_nm,
            c.cust_nm, l.lot_no AS in_lot_no, ii.item_nm AS in_item_nm, d.out_unit_cd,
            m.in_qty, m.good_qty, m.bad_qty, m.yield_rate,
            (SELECT STRING_AGG(ol.lot_no, ', ') WITHIN GROUP (ORDER BY ol.lot_no)
             FROM TPRLOTREL r JOIN TPRLOT ol ON ol.lot_id = r.child_lot_id WHERE r.rslt_id = m.rslt_id) AS out_lots,
            m.stat_cd, CONVERT(VARCHAR(16), m.cfm_dt, 120) AS cfm_dt, m.src_file_nm
        FROM TPRRSLTM m
            LEFT JOIN TPRWOD d ON d.wo_id = m.wo_id AND d.serl = m.wo_serl
            LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
            LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
            LEFT JOIN TPRLOT l ON l.lot_id = m.in_lot_id
            LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
        WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
          AND (@p_fr_date IS NULL OR @p_fr_date = '' OR m.rslt_date >= @p_fr_date)
          AND (@p_to_date IS NULL OR @p_to_date = '' OR m.rslt_date <= @p_to_date)
          AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
          AND (@p_proc_cd IS NULL OR @p_proc_cd = '' OR m.proc_cd = @p_proc_cd)
          AND (@p_rslt_no IS NULL OR @p_rslt_no = '' OR m.rslt_no LIKE '%' + @p_rslt_no + '%')
          AND (@p_wo_no IS NULL OR @p_wo_no = '' OR m.wo_no LIKE '%' + @p_wo_no + '%')
          AND (@p_lot_no IS NULL OR @p_lot_no = '' OR l.lot_no LIKE '%' + @p_lot_no + '%'
               OR EXISTS (SELECT 1 FROM TPRLOTREL r JOIN TPRLOT ol ON ol.lot_id = r.child_lot_id
                          WHERE r.rslt_id = m.rslt_id AND ol.lot_no LIKE '%' + @p_lot_no + '%'))
        ORDER BY m.rslt_date DESC, m.rslt_id DESC;
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
-- USP_PR_XFER_Q
-- ============================================================

-- ============================================================
-- 9) USP_PR_XFER_Q - Q: 헤더(0) + 라인(1) / LOT: 이전 가능 LOT 목록(출발 공정 창고에 재고가 있는 이 작업지시의 LOT)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_XFER_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_xfer_id BIGINT = NULL,
    @p_xfer_no VARCHAR(20) = NULL,
    @p_wo_id BIGINT = NULL,                 /* LOT 조회용 */
    @p_from_serl INT = NULL,
    @p_to_serl INT = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = xfer_id FROM TPRXFERM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_xfer_id IS NULL OR xfer_id = @p_xfer_id)
              AND (@p_xfer_id IS NOT NULL OR @p_xfer_no IS NULL OR xfer_no LIKE '%' + @p_xfer_no + '%')
            ORDER BY xfer_id DESC;

            SELECT
                m.xfer_id, m.acc_id, m.xfer_no, m.xfer_date, m.xfer_kind, m.wo_id, m.wo_no, m.from_serl, m.to_serl,
                m.from_cust_id, fc.cust_nm AS from_cust_nm, m.from_wh_id, fw.wh_nm AS from_wh_nm,
                m.to_cust_id, tc.cust_nm AS to_cust_nm, m.to_wh_id, tw.wh_nm AS to_wh_nm, m.trans_wh_id,
                m.stat_cd, m.out_dt, m.out_user_id, m.in_dt, m.in_user_id, m.remark
            FROM TPRXFERM m
                LEFT JOIN TBACUST fc ON fc.cust_id = m.from_cust_id
                LEFT JOIN TBACUST tc ON tc.cust_id = m.to_cust_id
                LEFT JOIN TBAWH fw ON fw.wh_id = m.from_wh_id
                LEFT JOIN TBAWH tw ON tw.wh_id = m.to_wh_id
            WHERE m.xfer_id = @match_id;

            SELECT
                d.xfer_id, d.serl, d.acc_id, d.xfer_no, d.item_id, i.item_no, i.item_nm, d.lot_id, d.lot_no, d.unit_cd,
                d.out_qty, d.in_qty, d.diff_qty, d.diff_resp_cd, d.diff_act_cd, d.diff_dt, d.diff_user_id, d.diff_remark, d.remark
            FROM TPRXFERD d LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE d.xfer_id = @match_id ORDER BY d.serl;
        END
        ELSE IF @p_work_type = 'LOT'
        BEGIN
            -- 이전 대상 품목 = 두 공정 중 앞선 공정의 산출품목. 위치 = 출발 공정의 외주처 창고.
            SELECT l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.stock_qty
            FROM TPRWOD f
                JOIN TPRWOD lo ON lo.wo_id = f.wo_id AND lo.serl = CASE WHEN @p_to_serl IS NULL OR @p_from_serl < @p_to_serl THEN @p_from_serl ELSE @p_to_serl END
                JOIN TPRLOT l ON l.wo_id = f.wo_id AND l.item_id = lo.out_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = f.wh_id AND s.loc_id = 0
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE f.wo_id = @p_wo_id AND f.serl = @p_from_serl AND s.stock_qty > 0
            ORDER BY l.lot_no;
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
-- USP_PR_XFERSTAT_Q
-- ============================================================

CREATE OR ALTER PROCEDURE USP_PR_XFERSTAT_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50) = 'L',
    ---------------------------------------------------------------------------------------------------
    @p_fr_date VARCHAR(8) = NULL,
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_xfer_no VARCHAR(20) = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
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
        SELECT
            m.xfer_id, m.xfer_no, m.xfer_date, m.wo_id, m.wo_no,
            fp.proc_nm AS from_proc_nm, fc.cust_nm AS from_cust_nm, tp.proc_nm AS to_proc_nm, tc.cust_nm AS to_cust_nm,
            t.lot_cnt, t.out_qty, t.in_qty, t.diff_qty,
            m.stat_cd, CONVERT(VARCHAR(16), m.out_dt, 120) AS out_dt, CONVERT(VARCHAR(16), m.in_dt, 120) AS in_dt
        FROM TPRXFERM m
            LEFT JOIN TPRWOD fd ON fd.wo_id = m.wo_id AND fd.serl = m.from_serl
            LEFT JOIN TPRWOD td ON td.wo_id = m.wo_id AND td.serl = m.to_serl
            LEFT JOIN TBAPROC fp ON fp.acc_id = m.acc_id AND fp.proc_cd = fd.proc_cd
            LEFT JOIN TBAPROC tp ON tp.acc_id = m.acc_id AND tp.proc_cd = td.proc_cd
            LEFT JOIN TBACUST fc ON fc.cust_id = m.from_cust_id
            LEFT JOIN TBACUST tc ON tc.cust_id = m.to_cust_id
            OUTER APPLY (SELECT COUNT(*) AS lot_cnt, SUM(x.out_qty) AS out_qty, SUM(x.in_qty) AS in_qty, SUM(x.diff_qty) AS diff_qty
                         FROM TPRXFERD x WHERE x.xfer_id = m.xfer_id) t
        WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
          AND (@p_fr_date IS NULL OR @p_fr_date = '' OR m.xfer_date >= @p_fr_date)
          AND (@p_to_date IS NULL OR @p_to_date = '' OR m.xfer_date <= @p_to_date)
          AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
          AND (@p_xfer_no IS NULL OR @p_xfer_no = '' OR m.xfer_no LIKE '%' + @p_xfer_no + '%')
          AND (@p_wo_no IS NULL OR @p_wo_no = '' OR m.wo_no LIKE '%' + @p_wo_no + '%')
          AND (@p_lot_no IS NULL OR @p_lot_no = '' OR EXISTS (SELECT 1 FROM TPRXFERD x WHERE x.xfer_id = m.xfer_id AND x.lot_no LIKE '%' + @p_lot_no + '%'))
        ORDER BY m.xfer_date DESC, m.xfer_id DESC;
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
-- USP_SA_GI_Q
-- ============================================================

-- 출하 품목의 "수주 잔량" 표시도 다른 미확정 출하가 잡은 수량을 뺀다(이 출하의 다른 품목 행 제외는 화면이 계산)
CREATE OR ALTER PROCEDURE USP_SA_GI_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),               /* Q 단건 / L 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_gi_id BIGINT = NULL,
    @p_gi_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_so_no VARCHAR(20) = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
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
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = gi_id FROM TSAGIM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_gi_id IS NULL OR gi_id = @p_gi_id)
              AND (@p_gi_id IS NOT NULL OR @p_gi_no IS NULL OR gi_no LIKE '%' + @p_gi_no + '%')
            ORDER BY gi_id DESC;

            SELECT
                m.gi_id, m.acc_id, m.gi_no, m.gi_date, m.cust_id, c.cust_nm, m.ship_date,
                m.carrier, m.bl_no, m.dest, m.dept_id, dp.dept_nm, m.emp_id, e.emp_nm,
                m.stat_cd, m.cfm_yn, m.cfm_dt, m.cfm_user_id, m.remark
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT dp ON dp.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.gi_id = @match_id;

            SELECT
                d.gi_id, d.serl, d.acc_id, d.gi_no, d.so_id, so.so_no, d.so_serl, d.item_id, i.item_no, i.item_nm, d.unit_cd,
                d.qty, d.price, d.amt, d.remark,
                sd.qty AS so_qty, ISNULL(sd.next_qty, 0) AS so_next_qty, sd.qty - ISNULL(sd.next_qty, 0) - ISNULL((SELECT SUM(g2.qty) FROM TSAGID g2 JOIN TSAGIM m2 ON m2.gi_id = g2.gi_id WHERE m2.stat_cd = '0' AND g2.so_id = d.so_id AND g2.so_serl = d.so_serl AND g2.gi_id <> d.gi_id), 0) AS so_remain_qty
            FROM TSAGID d
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TSASOM so ON so.so_id = d.so_id
                LEFT JOIN TSASOD sd ON sd.so_id = d.so_id AND sd.serl = d.so_serl
            WHERE d.gi_id = @match_id
            ORDER BY d.serl;

            SELECT
                l.gi_id, l.serl, l.lot_serl, l.acc_id, l.item_id, l.ship_kind, l.lot_id, l.lot_no, l.wh_id, w.wh_nm, l.qty, l.remark,
                ISNULL(st.stock_qty, 0) AS stock_qty
            FROM TSAGIL l
                LEFT JOIN TBAWH w ON w.wh_id = l.wh_id
                LEFT JOIN TMASTOCK st ON st.acc_id = l.acc_id AND st.item_id = l.item_id AND st.wh_id = l.wh_id AND st.loc_id = 0 AND st.lot_no = ISNULL(l.lot_no, N'')
            WHERE l.gi_id = @match_id
            ORDER BY l.serl, l.lot_serl;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.gi_id, m.gi_no, m.gi_date, c.cust_nm, t.so_list AS so_no, t.kind_nm AS ship_kind_nm, m.ship_date, m.carrier, m.bl_no, m.stat_cd,
                t.line_cnt, t.total_qty, t.lot_list
            FROM TSAGIM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                OUTER APPLY (SELECT COUNT(*) AS line_cnt, SUM(d.qty) AS total_qty,
                                    (SELECT STRING_AGG(x.lot_no, ', ') WITHIN GROUP (ORDER BY x.serl, x.lot_serl) FROM TSAGIL x WHERE x.gi_id = m.gi_id AND ISNULL(x.lot_no, N'') <> N'') AS lot_list,
                                    (SELECT STRING_AGG(z.so_no, ', ') FROM (SELECT DISTINCT so2.so_no FROM TSAGID x2 JOIN TSASOM so2 ON so2.so_id = x2.so_id WHERE x2.gi_id = m.gi_id) z) AS so_list,
                                    (SELECT STRING_AGG(z.nm, ', ') FROM (SELECT DISTINCT CASE x4.ship_kind WHEN 'D' THEN N'외주처 직송' ELSE N'자사창고' END AS nm FROM TSAGIL x4 WHERE x4.gi_id = m.gi_id) z) AS kind_nm
                             FROM TSAGID d WHERE d.gi_id = m.gi_id) t
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (ISNULL(@p_fr_date, '') = '' OR m.gi_date >= @p_fr_date)
              AND (ISNULL(@p_to_date, '') = '' OR m.gi_date <= @p_to_date)
              AND (ISNULL(@p_stat_cd, '') = '' OR m.stat_cd = @p_stat_cd)
              AND (ISNULL(@p_gi_no, '') = '' OR m.gi_no LIKE '%' + @p_gi_no + '%')
              AND (ISNULL(@p_so_no, '') = '' OR EXISTS (SELECT 1 FROM TSAGID x3 JOIN TSASOM so3 ON so3.so_id = x3.so_id WHERE x3.gi_id = m.gi_id AND so3.so_no LIKE '%' + @p_so_no + '%'))
              AND (ISNULL(@p_cust_nm, N'') = N'' OR c.cust_nm LIKE N'%' + @p_cust_nm + N'%')
              AND (ISNULL(@p_lot_no, N'') = N'' OR EXISTS (SELECT 1 FROM TSAGIL x WHERE x.gi_id = m.gi_id AND x.lot_no LIKE N'%' + @p_lot_no + N'%'))
            ORDER BY m.gi_date DESC, m.gi_id DESC;
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
-- USP_SA_QT_Q
-- ============================================================

-- ============================================================
-- USP_SA_QT_Q - frmQt 전용.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_QT_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_qt_id BIGINT = NULL,
    @p_qt_no VARCHAR(20) = NULL,
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
            DECLARE @match_qt_id BIGINT;
            SELECT TOP 1 @match_qt_id = qt_id
            FROM TSAQTM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_qt_id IS NULL OR qt_id = @p_qt_id)
              AND (@p_qt_id IS NOT NULL OR @p_qt_no IS NULL OR qt_no LIKE '%' + @p_qt_no + '%')
            ORDER BY qt_id DESC;

            SELECT
                m.qt_id, m.acc_id, a.ACC_NM,
                m.qt_no, m.qt_date, m.valid_date,
                m.stat_cd, m.qt_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.remark
            FROM TSAQTM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.qt_id = @match_qt_id;

            SELECT
                dt.qt_id, dt.serl, dt.acc_id, dt.qt_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.pjt_id, dt.remark
            FROM TSAQTD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
            WHERE dt.qt_id = @match_qt_id
            ORDER BY dt.serl;
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
-- USP_SA_SO_Q
-- ============================================================

-- ============================================================
-- USP_SA_SO_Q - frmSo 전용.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SO_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_so_no VARCHAR(20) = NULL,
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
            DECLARE @match_so_id BIGINT;
            SELECT TOP 1 @match_so_id = so_id
            FROM TSASOM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_so_id IS NULL OR so_id = @p_so_id)
              AND (@p_so_id IS NOT NULL OR @p_so_no IS NULL OR so_no LIKE '%' + @p_so_no + '%')
            ORDER BY so_id DESC;

            SELECT
                m.so_id, m.acc_id, a.ACC_NM,
                m.so_no, m.so_date,
                m.stat_cd, m.so_title,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.pjt_id, m.cur_cd, m.exc_rate,
                m.delv_date, m.vat_type, m.vat_rate,
                m.cfm_yn, m.cfm_dt, m.cfm_user_id,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd,
                m.remark
            FROM TSASOM m
                LEFT JOIN TBAACC a ON a.ACC_ID = m.acc_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE m.so_id = @match_so_id;

            SELECT
                dt.so_id, dt.serl, dt.acc_id, dt.so_no,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.unit_cd, dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty,
                dt.price, dt.amt, dt.vat, dt.total_amt,
                dt.kor_price, dt.kor_amt, dt.kor_vat, dt.kor_total_amt,
                dt.vat_type, dt.vat_rate, dt.delv_date,
                dt.wh_id, w.wh_nm, dt.loc_id, l.loc_nm,
                dt.stock_yn, dt.stock_unit_cd, dt.stock_unit_qty,
                dt.pjt_id, dt.src_type, dt.src_id, dt.src_no, dt.src_serl,
                dt.stop_yn, dt.stop_emp_no, dt.stop_remark, dt.remark
            FROM TSASOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = dt.loc_id
            WHERE dt.so_id = @match_so_id
            ORDER BY dt.serl;
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
-- USP_SA_SOLIST_Q
-- ============================================================
-- 수주현황 품목상세: 미확정 출하등록 수량(reg_qty) 표시, 잔량은 확정+미확정 차감
CREATE OR ALTER PROCEDURE USP_SA_SOLIST_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_so_no VARCHAR(20) = NULL,
    @p_so_title NVARCHAR(1000) = NULL,
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
                m.so_id, m.so_no, m.so_date, m.so_title,
                m.stat_cd,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TSASOM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND 1 = 1
              AND (@p_so_no IS NULL OR m.so_no LIKE '%' + @p_so_no + '%')
              AND (@p_so_title IS NULL OR m.so_title LIKE '%' + @p_so_title + '%')
            ORDER BY m.so_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.so_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, (ISNULL(dt.next_qty, 0) + ISNULL(rg.reg_qty, 0)) AS next_qty, ISNULL(rg.reg_qty, 0) AS reg_qty,
                (dt.qty - ISNULL(dt.next_qty, 0) - ISNULL(rg.reg_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.total_amt,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TSASOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                OUTER APPLY (SELECT SUM(gd.qty) AS reg_qty FROM TSAGID gd JOIN TSAGIM gm ON gm.gi_id = gd.gi_id
                             WHERE gm.stat_cd = '0' AND gd.so_id = dt.so_id AND gd.so_serl = dt.serl) rg
            WHERE dt.so_id = @p_so_id
            ORDER BY dt.serl;
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
-- USP_SM_BOARD_Q
-- ============================================================

-- ============================================================
-- 2) USP_SM_BOARD_Q - 목록 조회
--    p_top_n: NULL이면 전체, 값이 있으면 상위 N건만(홈화면 위젯이 씀 - 중요공지 먼저, 최신순)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SM_BOARD_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_title VARCHAR(200) = NULL,
    @p_use_yn VARCHAR(1) = NULL,
    @p_top_n INT = NULL,
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
            SELECT TOP (ISNULL(@p_top_n, 2147483647))
                a.board_id,
                a.acc_id,
                a.title,
                a.content,
                a.emp_id,
                e.emp_nm,
                a.important_yn,
                a.use_yn,
                a.reg_user_id,
                a.reg_dt,
                a.upt_user_id,
                a.upt_dt
            FROM TSMBOARD a
            LEFT JOIN TBAEMP e ON e.EMP_ID = a.emp_id
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND (@p_title IS NULL OR @p_title = '' OR a.title LIKE '%' + @p_title + '%')
              AND (@p_use_yn IS NULL OR @p_use_yn = '' OR a.use_yn = @p_use_yn)
            ORDER BY a.important_yn DESC, a.reg_dt DESC;
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

