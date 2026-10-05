-- 기타수불유형 기준 정리 (2026-10-03, WYNLAB_DEV 전용)
-- 기타입고/기타출고에서 고를 수 있는 수불유형 = 기초코드 MA0011 중 기타수불여부(rel_cd2)='Y'인 것(입고/출고는 rel_cd1 I/O로 구분).
--  - 콤보 L_MA0011_O(출고유형) 조건에 rel_cd2='Y' 추가, 새 콤보 L_MA0011_I(입고유형) 추가
--  - 기타출고요청/기타출고 저장 프로시저의 출고유형 검증도 같은 기준으로(예전엔 출고 계열이면 아무거나 허용)
-- 사용자가 MA0011에서 기타수불여부 체크를 바꾸면 콤보와 검증이 그대로 따라간다. 여러 번 실행해도 안전하다.

UPDATE sysLookupM SET query_txt =
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''MA0011''' + CHAR(13) + CHAR(10)
       + N'AND     rel_cd1 = ''O''' + CHAR(13) + CHAR(10) + N'AND     rel_cd2 = ''Y''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm',
       lookup_nm = N'출고유형(기타수불 출고계열)'
WHERE lookup_key = 'L_MA0011_O';
GO

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_MA0011_I', NULL, N'입고유형(기타수불 입고계열)', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''MA0011''' + CHAR(13) + CHAR(10)
       + N'AND     rel_cd1 = ''I''' + CHAR(13) + CHAR(10) + N'AND     rel_cd2 = ''Y''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_MA0011_I');
GO
CREATE OR ALTER PROCEDURE USP_MA_ETCREQ_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_req_date VARCHAR(8) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
    @p_out_reason VARCHAR(10) = NULL,
    @p_trans_type VARCHAR(10) = NULL,       /* 출고유형 - MA0011 중 출고 계열(rel_cd1='O'), 비우면 ETC_OUT */
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10), @app_no VARCHAR(20), @appr VARCHAR(10);
            SELECT @stat = m.stat_cd, @app_no = m.app_no, @appr = t.app_stat_cd
            FROM TMAETCREQM m LEFT JOIN TAPDOC t ON t.app_id = m.app_id WHERE m.req_id = @p_req_id;

            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고요청을 찾을 수 없습니다.'; RETURN; END
            IF @stat = 'C' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청은 수정하거나 삭제할 수 없습니다.'; RETURN; END
            IF @appr IN ('0', '1') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재가 진행 중인 기타출고요청은 수정하거나 삭제할 수 없습니다.'; RETURN; END
            IF @p_work_type = 'D' AND ISNULL(@app_no, '') <> ''
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'이미 결재상신된 기타출고요청은 삭제할 수 없습니다.'; RETURN; END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'ETC_OUT';
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'O' AND rel_cd2 = 'Y' AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고유형이 올바르지 않습니다(기초코드 MA0011에서 기타수불여부가 체크된 출고 계열만 선택할 수 있습니다).'; RETURN; END
        END
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCREQM', 'req_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCREQM (acc_id, req_no, req_date, req_title, out_reason, trans_type, dept_id, emp_id, stat_cd, cfm_yn, stop_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_req_date, @p_req_title, @p_out_reason, @p_trans_type, @p_dept_id, @p_emp_id, '0', 'N', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_req_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCREQM SET
                acc_id = @p_acc_id, req_date = @p_req_date, req_title = @p_req_title, out_reason = @p_out_reason, trans_type = @p_trans_type,
                dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAETCREQD WHERE req_id = @p_req_id;
            DELETE FROM TMAETCREQM WHERE req_id = @p_req_id;
        END

        SET @GeneratedCode = CAST(@p_req_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE USP_MA_ETCOUT_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_out_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_out_date VARCHAR(8) = NULL,
    @p_out_reason VARCHAR(10) = NULL,
    @p_trans_type VARCHAR(10) = NULL,       /* 출고유형 - MA0011 중 출고 계열(rel_cd1='O'), 비우면 ETC_OUT */
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
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
        IF @p_work_type IN ('U', 'D')
        BEGIN
            DECLARE @stat VARCHAR(10);
            SELECT @stat = stat_cd FROM TMAETCOUTM WHERE out_id = @p_out_id;
            IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고 문서를 찾을 수 없습니다.'; RETURN; END
            IF @stat <> '0' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 기타출고는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN; END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_trans_type, '') = '' SET @p_trans_type = 'ETC_OUT';
            IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0011' AND minor_cd = @p_trans_type AND rel_cd1 = 'O' AND rel_cd2 = 'Y' AND use_yn = 'Y')
            BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'출고유형이 올바르지 않습니다(기초코드 MA0011에서 기타수불여부가 체크된 출고 계열만 선택할 수 있습니다).'; RETURN; END
        END
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAETCOUTM', 'out_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAETCOUTM (acc_id, out_no, out_date, out_reason, trans_type, dept_id, emp_id, stat_cd, cfm_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_acc_id, @new_no, @p_out_date, @p_out_reason, @p_trans_type, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            SET @p_out_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCOUTM SET
                acc_id = @p_acc_id, out_date = @p_out_date, out_reason = @p_out_reason, trans_type = @p_trans_type, dept_id = @p_dept_id, emp_id = @p_emp_id, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE out_id = @p_out_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAETCOUTD WHERE out_id = @p_out_id;
            DELETE FROM TMAETCOUTM WHERE out_id = @p_out_id;
        END

        SET @GeneratedCode = CAST(@p_out_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
