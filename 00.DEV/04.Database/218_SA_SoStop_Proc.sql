-- 영업관리 P2 보충 - 수주 라인 마감/마감취소(USP_SA_SOSTOP_S). USP_MA_POSTOP_S(189)와 완전히 같은 구조,
-- TMAPOD/po_id 대신 TSASOD/so_id, 승인상태 체크 컬럼은 TSASOM.stat_cd='C'(수주승인완료).

CREATE OR ALTER PROCEDURE USP_SA_SOSTOP_S
    @p_work_type VARCHAR(50),               /* 'U' */
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_stop_yn VARCHAR(1) = 'Y',            /* Y=마감, N=마감취소 */
    @p_stop_remark NVARCHAR(1000) = NULL,
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
        IF @p_work_type = 'U'
        BEGIN
            DECLARE @qty NUMERIC(18,4), @next_qty NUMERIC(18,4), @stat_cd VARCHAR(10);
            SELECT @qty = d.qty, @next_qty = ISNULL(d.next_qty, 0), @stat_cd = m.stat_cd
            FROM TSASOD d JOIN TSASOM m ON m.so_id = d.so_id
            WHERE d.so_id = @p_so_id AND d.serl = @p_serl;

            IF @qty IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수주 품목을 찾을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@stat_cd, '') <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료된 수주의 품목만 마감할 수 있습니다.'; RETURN;
            END

            IF @p_stop_yn = 'Y'
            BEGIN
                IF @p_stop_remark IS NULL OR LTRIM(RTRIM(@p_stop_remark)) = N''
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = N'마감 사유를 입력하세요.'; RETURN;
                END
                IF @qty - @next_qty <= 0
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = N'잔량이 없는 수주 품목은 마감할 필요가 없습니다.'; RETURN;
                END

                DECLARE @emp_no VARCHAR(20);
                SELECT @emp_no = e.emp_no
                FROM TSMUSER u JOIN TBAEMP e ON e.emp_id = u.emp_id
                WHERE u.user_id = @p_user_id;

                UPDATE TSASOD SET
                    stop_yn = 'Y',
                    stop_emp_no = @emp_no,
                    stop_remark = @p_stop_remark,
                    upt_user_id = @p_user_id,
                    upt_dt = GETDATE(),
                    upt_pc = @p_client_pc
                WHERE so_id = @p_so_id AND serl = @p_serl;
            END
            ELSE
            BEGIN
                UPDATE TSASOD SET
                    stop_yn = 'N',
                    stop_emp_no = NULL,
                    stop_remark = NULL,
                    upt_user_id = @p_user_id,
                    upt_dt = GETDATE(),
                    upt_pc = @p_client_pc
                WHERE so_id = @p_so_id AND serl = @p_serl;
            END

            SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
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
