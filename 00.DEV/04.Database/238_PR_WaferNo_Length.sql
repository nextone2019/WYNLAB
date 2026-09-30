-- 웨이퍼 번호를 '<시작 LOT>-NN'로 정의하면서 wafer_no 길이 확장(VARCHAR(20) -> 60, 프로시저 파라미터도 동일 - 넘치면 조용히 잘리므로) (2026-09-30)
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TPRRSLTD' AND COLUMN_NAME='wafer_no' AND CHARACTER_MAXIMUM_LENGTH < 60)
    ALTER TABLE TPRRSLTD ALTER COLUMN wafer_no VARCHAR(60) NOT NULL;
GO

CREATE OR ALTER PROCEDURE USP_PR_RSLT_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_rslt_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_wafer_no VARCHAR(60) = NULL,
    @p_good_qty NUMERIC(18,4) = NULL,
    @p_bad_qty NUMERIC(18,4) = NULL,
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
        DECLARE @stat VARCHAR(10), @acc BIGINT;
        SELECT @stat = stat_cd, @acc = acc_id FROM TPRRSLTM WHERE rslt_id = @p_rslt_id;
        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'공정 실적을 찾을 수 없습니다.'; RETURN;
        END
        IF @stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 실적은 수정할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF ISNULL(@p_wafer_no, '') = ''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'웨이퍼 번호를 입력하세요.'; RETURN;
            END
            IF ISNULL(@p_good_qty, 0) < 0 OR ISNULL(@p_bad_qty, 0) < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'Good/Bad 수량은 음수일 수 없습니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id AND wafer_no = @p_wafer_no AND (@p_work_type = 'N' OR serl <> @p_serl))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 등록된 웨이퍼 번호입니다.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            SELECT @p_serl = ISNULL(MAX(serl), 0) + 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id;
            INSERT INTO TPRRSLTD (rslt_id, serl, acc_id, wafer_no, good_qty, bad_qty, remark, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            VALUES (@p_rslt_id, @p_serl, @acc, @p_wafer_no, ISNULL(@p_good_qty, 0), ISNULL(@p_bad_qty, 0), @p_remark,
                    @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
            UPDATE TPRRSLTD SET wafer_no = @p_wafer_no, good_qty = ISNULL(@p_good_qty, 0), bad_qty = ISNULL(@p_bad_qty, 0), remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rslt_id = @p_rslt_id AND serl = @p_serl;
        ELSE IF @p_work_type = 'D'
            DELETE FROM TPRRSLTD WHERE rslt_id = @p_rslt_id AND serl = @p_serl;
        ELSE IF @p_work_type = 'DA'
            DELETE FROM TPRRSLTD WHERE rslt_id = @p_rslt_id;

        IF EXISTS (SELECT 1 FROM TPRRSLTD WHERE rslt_id = @p_rslt_id)
            UPDATE TPRRSLTM SET
                good_qty = (SELECT SUM(good_qty) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id),
                bad_qty = (SELECT SUM(bad_qty) FROM TPRRSLTD WHERE rslt_id = @p_rslt_id),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE rslt_id = @p_rslt_id;

        SET @GeneratedCode = CAST(@p_rslt_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
