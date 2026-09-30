-- 공정마스터(frmProc) / 라우팅(frmRoute) 화면 지원 (2026-09-29): 프로시저 5개 + 메뉴 등록. 메뉴는 생산관리(14) > 생산기준관리(15) 밑, 접두사 USP_PR_.
--  USP_PR_PROC_Q / _S           TBAPROC 목록 조회 / 행별 N,U,D 저장(그리드 저장 방식). 삭제는 라우팅/작업지시/실적에서 쓰는 공정이면 막는다.
--  USP_PR_ROUTE_Q / _S / _S_1   TPRROUTEM(라우팅) 헤더 + TPRROUTED(공정 체인) 조회 / 헤더 N,U,D / 공정행 N,U,D.
--    라우팅을 고쳐도 이미 낸 작업지시는 영향이 없다(작업지시는 공정행을 복사해서 갖는다). 라우팅 삭제는 작업지시가 쓰는 라우팅이면 막는다.

-- ============================================================
-- 1) USP_PR_PROC_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_PROC_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_keyword NVARCHAR(50) = NULL,         /* 공정코드/공정명 */
    @p_use_yn VARCHAR(1) = NULL,
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
            SELECT p.acc_id, p.proc_cd, p.proc_nm, p.sort, p.use_yn, p.remark,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRROUTED d WHERE d.acc_id = p.acc_id AND d.proc_cd = p.proc_cd)
                          OR EXISTS (SELECT 1 FROM TPRWOD w WHERE w.acc_id = p.acc_id AND w.proc_cd = p.proc_cd)
                        THEN 'Y' ELSE 'N' END AS used_yn
            FROM TBAPROC p
            WHERE (@p_acc_id IS NULL OR p.acc_id = @p_acc_id)
              AND (@p_keyword IS NULL OR @p_keyword = '' OR p.proc_cd LIKE '%' + @p_keyword + '%' OR p.proc_nm LIKE '%' + @p_keyword + '%')
              AND (@p_use_yn IS NULL OR @p_use_yn = '' OR p.use_yn = @p_use_yn)
            ORDER BY p.sort, p.proc_cd;
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
-- 2) USP_PR_PROC_S - 행 단위 N/U/D. 공정코드는 신규 때만 정하고 수정 불가(라우팅/작업지시가 코드값을 저장하므로).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_PROC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_proc_cd VARCHAR(10) = NULL,
    @p_proc_nm NVARCHAR(50) = NULL,
    @p_sort INT = NULL,
    @p_use_yn VARCHAR(1) = NULL,
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
        IF ISNULL(@p_proc_cd, '') = ''
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'공정코드를 입력하세요.'; RETURN;
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_proc_nm, N'') = N''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정명을 입력하세요.'; RETURN;
            END
            IF ISNULL(@p_use_yn, 'Y') NOT IN ('Y', 'N')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사용여부가 올바르지 않습니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF EXISTS (SELECT 1 FROM TBAPROC WHERE acc_id = @p_acc_id AND proc_cd = @p_proc_cd)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 등록된 공정코드입니다. (' + @p_proc_cd + N')'; RETURN;
            END
            INSERT INTO TBAPROC (acc_id, proc_cd, proc_nm, sort, use_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @p_proc_cd, @p_proc_nm, ISNULL(@p_sort, 0), ISNULL(@p_use_yn, 'Y'), @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAPROC SET proc_nm = @p_proc_nm, sort = ISNULL(@p_sort, 0), use_yn = ISNULL(@p_use_yn, 'Y'), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @p_acc_id AND proc_cd = @p_proc_cd;
            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수정할 공정을 찾을 수 없습니다.'; RETURN;
            END
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TPRROUTED WHERE acc_id = @p_acc_id AND proc_cd = @p_proc_cd)
               OR EXISTS (SELECT 1 FROM TPRWOD WHERE acc_id = @p_acc_id AND proc_cd = @p_proc_cd)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅이나 작업지시에서 쓰는 공정은 삭제할 수 없습니다. 사용여부를 N으로 바꾸세요. (' + @p_proc_cd + N')'; RETURN;
            END
            DELETE FROM TBAPROC WHERE acc_id = @p_acc_id AND proc_cd = @p_proc_cd;
        END

        SET @GeneratedCode = @p_proc_cd;
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
-- 3) USP_PR_ROUTE_Q - 헤더(0) + 공정행(1)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_route_cd VARCHAR(20) = NULL,
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
            SELECT TOP 1 @match_id = route_id FROM TPRROUTEM
            WHERE (@p_route_id IS NULL OR route_id = @p_route_id)
              AND (@p_route_id IS NOT NULL OR @p_route_cd IS NULL OR route_cd LIKE '%' + @p_route_cd + '%')
            ORDER BY route_id DESC;

            SELECT m.route_id, m.acc_id, m.route_cd, m.route_nm, m.item_id, i.item_no, i.item_nm, m.use_yn, m.remark,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRWOM w WHERE w.route_id = m.route_id) THEN 'Y' ELSE 'N' END AS used_yn
            FROM TPRROUTEM m LEFT JOIN TBAITEM i ON i.item_id = m.item_id
            WHERE m.route_id = @match_id;

            SELECT d.route_id, d.serl, d.acc_id, d.proc_cd, p.proc_nm, d.cust_id, c.cust_nm,
                   d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd,
                   d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd,
                   d.split_qty, d.price_unit_cd, d.price, d.remark
            FROM TPRROUTED d
                LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = d.cust_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE d.route_id = @match_id
            ORDER BY d.serl;
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
-- 4) USP_PR_ROUTE_S - 헤더 N/U/D. 라우팅코드는 사업장 안에서 유일. 삭제는 작업지시가 쓰는 라우팅이면 거부.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_route_cd VARCHAR(20) = NULL,
    @p_route_nm NVARCHAR(100) = NULL,
    @p_item_id BIGINT = NULL,
    @p_use_yn VARCHAR(1) = NULL,
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
    SET XACT_ABORT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_route_cd, '') = ''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅코드를 입력하세요.'; RETURN;
            END
            IF ISNULL(@p_route_nm, N'') = N''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅명을 입력하세요.'; RETURN;
            END
            IF ISNULL(@p_use_yn, 'Y') NOT IN ('Y', 'N')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사용여부가 올바르지 않습니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TPRROUTEM WHERE acc_id = @p_acc_id AND route_cd = @p_route_cd AND (@p_work_type = 'N' OR route_id <> @p_route_id))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 등록된 라우팅코드입니다. (' + @p_route_cd + N')'; RETURN;
            END
        END

        IF @p_work_type IN ('U', 'D') AND NOT EXISTS (SELECT 1 FROM TPRROUTEM WHERE route_id = @p_route_id)
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅을 찾을 수 없습니다.'; RETURN;
        END

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TPRROUTEM (acc_id, route_cd, route_nm, item_id, use_yn, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @p_route_cd, @p_route_nm, @p_item_id, ISNULL(@p_use_yn, 'Y'), @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_route_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRROUTEM SET route_cd = @p_route_cd, route_nm = @p_route_nm, item_id = @p_item_id, use_yn = ISNULL(@p_use_yn, 'Y'), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE route_id = @p_route_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TPRWOM WHERE route_id = @p_route_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'작업지시에서 쓴 라우팅은 삭제할 수 없습니다. 사용여부를 N으로 바꾸세요.'; RETURN;
            END
            BEGIN TRAN;
            DELETE FROM TPRROUTED WHERE route_id = @p_route_id;
            DELETE FROM TPRROUTEM WHERE route_id = @p_route_id;
            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- ============================================================
-- 5) USP_PR_ROUTE_S_1 - 공정행 N/U/D. 순번(serl)은 라우팅 안에서 유일(비우면 마지막+1). 공정코드는 공정마스터에 있어야 하고 투입/산출 품목은 필수.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_proc_cd VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_in_item_id BIGINT = NULL,
    @p_out_item_id BIGINT = NULL,
    @p_in_unit_cd VARCHAR(10) = NULL,
    @p_out_unit_cd VARCHAR(10) = NULL,
    @p_split_qty NUMERIC(18,4) = NULL,
    @p_price_unit_cd VARCHAR(10) = NULL,
    @p_price NUMERIC(18,4) = NULL,
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
        DECLARE @acc BIGINT;
        SELECT @acc = acc_id FROM TPRROUTEM WHERE route_id = @p_route_id;
        IF @acc IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'라우팅을 찾을 수 없습니다. 먼저 헤더를 저장하세요.'; RETURN;
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_proc_cd, '') = '' OR NOT EXISTS (SELECT 1 FROM TBAPROC WHERE acc_id = @acc AND proc_cd = @p_proc_cd)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'공정을 선택하세요. (공정마스터에 등록된 공정만 쓸 수 있습니다)'; RETURN;
            END
            IF @p_in_item_id IS NULL OR @p_out_item_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'투입품목과 산출품목을 모두 선택하세요.'; RETURN;
            END
            IF @p_split_qty IS NOT NULL AND @p_split_qty <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'분할수량은 0보다 커야 합니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_serl IS NULL SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TPRROUTED WHERE route_id = @p_route_id;
            IF EXISTS (SELECT 1 FROM TPRROUTED WHERE route_id = @p_route_id AND serl = @p_serl)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 있는 공정 순번입니다. (' + CAST(@p_serl AS NVARCHAR(10)) + N')'; RETURN;
            END
            INSERT INTO TPRROUTED (route_id, serl, acc_id, proc_cd, cust_id, in_item_id, out_item_id, in_unit_cd, out_unit_cd, split_qty, price_unit_cd, price, remark,
                                   reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_route_id, @p_serl, @acc, @p_proc_cd, @p_cust_id, @p_in_item_id, @p_out_item_id, @p_in_unit_cd, @p_out_unit_cd, @p_split_qty, @p_price_unit_cd, @p_price, @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TPRROUTED SET proc_cd = @p_proc_cd, cust_id = @p_cust_id, in_item_id = @p_in_item_id, out_item_id = @p_out_item_id,
                in_unit_cd = @p_in_unit_cd, out_unit_cd = @p_out_unit_cd, split_qty = @p_split_qty, price_unit_cd = @p_price_unit_cd, price = @p_price, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE route_id = @p_route_id AND serl = @p_serl;
            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수정할 공정 행을 찾을 수 없습니다.'; RETURN;
            END
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRROUTED WHERE route_id = @p_route_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
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
-- 6) 메뉴: 생산관리(14) > 생산기준관리(15) > 공정관리/라우팅관리
-- ============================================================
DECLARE @screens TABLE (nm NVARCHAR(50), cls VARCHAR(50), sort INT);
INSERT INTO @screens VALUES (N'공정관리', 'frmProc', 10), (N'라우팅관리', 'frmRoute', 20);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT s.nm, 15, 3, 'FORM', 'PR', s.cls, 'USP_PR_', s.sort, 'Y', SUSER_SNAME(), GETDATE()
FROM @screens s
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.MODULE = 'PR' AND m.SCREEN_CLASS_NM = s.cls);
GO
