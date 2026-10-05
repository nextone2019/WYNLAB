-- 결재경로 관리 화면(frmApprRoute) 추가. TAPROUTE/TAPROUTEDETAIL·USP_AP_ROUTE_S는 124번에서
-- 이미 만들어져 있었다(popApp 안의 결재경로 저장 위젯용) - 새 테이블을 또 만들지 않고 그대로
-- 쓴다. 다만 지금까지 이 경로들을 "목록으로 관리"하는 화면이 없었고(이름변경/삭제 UI 자체가
-- 없음), 상세(결재라인/수신라인) 전체를 한 번에 재구성하는 기능도 없었다(ADDDETAIL로 한 명씩
-- 추가만 가능) - 이번에 프로시저를 확장한다.
--
-- 동시에 ADDDETAIL/D가 @p_route_id 소유자 확인 없이 동작하던 구멍도 막는다(로그인만 하면 다른
-- 사람의 route_id를 알아내 그 사람 결재경로에 라인을 추가하거나 지울 수 있었음).

-- frmApprRoute 자기 메뉴(PROC_PREFIX=USP_AP_ROUTE_)로 직접 부르는 조회 프로시저 - frmApprInbox와
-- 같은 원칙("자기 메뉴면 QueryAsync/SaveAsync로 충분, ApprovalClient는 다른 메뉴에서 부를 때만").
-- 소유자 식별은 QUERY 쪽이라 SaveAsync처럼 서버가 세션에서 강제하지 않는다(GenericDataRepository의
-- query 엔드포인트는 CurrentUserId를 넘기지 않음 - frmApprInbox.QueryClick이 p_emp_no를 직접
-- 넘기는 것과 같은 기존 관례) - 그래서 Session.EmpId를 클라이언트가 그대로 넘긴다.
CREATE OR ALTER PROCEDURE USP_AP_ROUTE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
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
        IF @p_work_type = 'Q' -- 내 결재경로 목록(grd1)
        BEGIN
            SELECT route_id, route_nm
            FROM TAPROUTE
            WHERE emp_id = @p_emp_id AND use_yn = 'Y'
            ORDER BY route_nm;
        END
        ELSE IF @p_work_type = 'Q1' -- 선택된 경로의 상세(결재라인/수신라인)
        BEGIN
            SELECT d.route_id, d.sort, d.emp_id, e.emp_no, e.emp_nm, d.path_type
            FROM TAPROUTEDETAIL d
            LEFT JOIN TBAEMP e ON e.EMP_ID = d.emp_id
            WHERE d.route_id = @p_route_id
            ORDER BY d.path_type, d.sort;
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

CREATE OR ALTER PROCEDURE USP_AP_ROUTE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_route_nm NVARCHAR(100) = NULL,
    @p_target_emp_no VARCHAR(20) = NULL,
    @p_path_type VARCHAR(10) = NULL,
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
        DECLARE @owner_emp_id BIGINT;
        SELECT @owner_emp_id = EMP_ID FROM TSMUSER WHERE USER_ID = @p_user_id;

        IF @p_work_type = 'N' -- 경로 헤더 신규
        BEGIN
            INSERT INTO TAPROUTE (emp_id, route_nm, use_yn, reg_user_id, reg_dt, reg_pc)
            VALUES (@owner_emp_id, @p_route_nm, 'Y', @p_user_id, GETDATE(), @p_client_pc);

            SET @p_route_id = SCOPE_IDENTITY();
            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U' -- 경로 이름 변경(소유자 본인만) - frmApprRoute 신규 추가
        BEGIN
            UPDATE TAPROUTE
               SET route_nm = @p_route_nm, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
             WHERE route_id = @p_route_id AND emp_id = @owner_emp_id;

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'변경할 결재경로를 찾을 수 없습니다.';
                RETURN;
            END

            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'ADDDETAIL' -- 경로 상세 한 명 추가(소유자 본인 확인 추가)
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TAPROUTE WHERE route_id = @p_route_id AND emp_id = @owner_emp_id)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 결재경로를 찾을 수 없습니다.';
                RETURN;
            END

            DECLARE @target_emp_id BIGINT;
            SELECT @target_emp_id = EMP_ID FROM TBAEMP WHERE emp_no = @p_target_emp_no;
            IF @target_emp_id IS NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 사원 정보를 찾을 수 없습니다.';
                RETURN;
            END

            DECLARE @nextSort INT;
            SELECT @nextSort = ISNULL(MAX(sort), 0) + 1 FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;

            INSERT INTO TAPROUTEDETAIL (route_id, sort, emp_id, path_type)
            VALUES (@p_route_id, @nextSort, @target_emp_id, @p_path_type);

            SET @GeneratedCode = CAST(@p_route_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'CLEARDETAIL' -- 경로 상세 전체 삭제(소유자 본인만) - frmApprRoute
                                              -- 신규 추가. 저장화면이 전체 재삽입 전에 먼저 부른다.
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TAPROUTE WHERE route_id = @p_route_id AND emp_id = @owner_emp_id)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 결재경로를 찾을 수 없습니다.';
                RETURN;
            END

            DELETE FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;
        END
        ELSE IF @p_work_type = 'D' -- 경로 삭제(소유자 확인 위치를 앞으로 당김 - 기존엔 TAPROUTEDETAIL을
                                    -- 소유자 확인 없이 먼저 지웠음)
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TAPROUTE WHERE route_id = @p_route_id AND emp_id = @owner_emp_id)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'대상 결재경로를 찾을 수 없습니다.';
                RETURN;
            END

            DELETE FROM TAPROUTEDETAIL WHERE route_id = @p_route_id;
            DELETE FROM TAPROUTE WHERE route_id = @p_route_id AND emp_id = @owner_emp_id;
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

-- 메뉴 등록: 결재함(frmApprInbox)과 같은 그룹 밑에 "결재경로관리" 추가.
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'AP' AND SCREEN_CLASS_NM = 'frmApprRoute')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    SELECT N'결재경로관리', UPPER_MENU_ID, MENU_LEVEL, 'FORM', 'AP', 'frmApprRoute', 'USP_AP_ROUTE_', 30, 'Y', SUSER_SNAME(), GETDATE()
    FROM TSMMENU WHERE MODULE = 'AP' AND SCREEN_CLASS_NM = 'frmApprInbox';
END
GO
