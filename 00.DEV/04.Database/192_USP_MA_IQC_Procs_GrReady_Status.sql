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
            WHERE (@p_iqc_id IS NULL OR iqc_id = @p_iqc_id)
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
-- 2) USP_MA_IQC_S - 헤더 N/U/D
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_iqc_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_iqc_date VARCHAR(8) = NULL,
    @p_cust_id BIGINT = NULL,
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
            DECLARE @cur_stat VARCHAR(10), @cur_cust BIGINT;
            SELECT @cur_stat = stat_cd, @cur_cust = cust_id FROM TMAIQCM WHERE iqc_id = @p_iqc_id;

            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 문서를 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 수입검사는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_cust_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'거래처를 입력하세요.'; RETURN;
            END

            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAIQCM', 'iqc_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMAIQCM (
                acc_id, iqc_no, iqc_date, cust_id, dept_id, emp_id, stat_cd, cfm_yn, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_iqc_date, @p_cust_id, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_iqc_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF @p_cust_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'거래처를 입력하세요.'; RETURN;
            END
            IF @p_cust_id <> @cur_cust AND EXISTS (SELECT 1 FROM TMAIQCD WHERE iqc_id = @p_iqc_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'품목이 있는 수입검사는 거래처를 바꿀 수 없습니다. 품목을 지운 뒤 바꾸세요.'; RETURN;
            END

            UPDATE TMAIQCM SET
                acc_id = @p_acc_id,
                iqc_date = @p_iqc_date,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE iqc_id = @p_iqc_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAIQCD WHERE iqc_id = @p_iqc_id;
            DELETE FROM TMAIQCM WHERE iqc_id = @p_iqc_id;
        END

        SET @GeneratedCode = CAST(@p_iqc_id AS VARCHAR(20));
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
-- 3) USP_MA_IQC_S_1 - 라인 N/U/D. 원천(납품 라인)과 품목/LOT는 바꿀 수 없다.
--    검증: 합격+특채+불합격 = 검사수량, 검사수량 <= 납품 미검사 잔량(확정된 검사만 차감), 불합격이 있으면 사유/처분 필수,
--    특채가 있으면 사유(remark) 필수.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQC_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_iqc_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_insp_qty NUMERIC(18,4) = NULL,
    @p_sample_qty NUMERIC(18,4) = NULL,
    @p_pass_qty NUMERIC(18,4) = NULL,
    @p_conc_qty NUMERIC(18,4) = NULL,
    @p_fail_qty NUMERIC(18,4) = NULL,
    @p_fail_reason_cd VARCHAR(10) = NULL,
    @p_fail_action_cd VARCHAR(10) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_serl INT = NULL,
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
        DECLARE @hdr_stat VARCHAR(10), @hdr_cust BIGINT;
        SELECT @hdr_stat = stat_cd, @hdr_cust = cust_id FROM TMAIQCM WHERE iqc_id = @p_iqc_id;

        IF @hdr_stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 문서를 찾을 수 없습니다.'; RETURN;
        END
        IF @hdr_stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 수입검사는 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
        END

        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT;
        IF @p_work_type = 'N'
        BEGIN
            SET @src_type = @p_src_type; SET @src_id = @p_src_id; SET @src_serl = @p_src_serl;
        END
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl
            FROM TMAIQCD WHERE iqc_id = @p_iqc_id AND serl = @p_serl;

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 품목을 찾을 수 없습니다.'; RETURN;
            END
        END

        DECLARE @pass NUMERIC(18,4) = ISNULL(@p_pass_qty, 0), @conc NUMERIC(18,4) = ISNULL(@p_conc_qty, 0), @fail NUMERIC(18,4) = ISNULL(@p_fail_qty, 0);
        DECLARE @dl_item BIGINT, @dl_unit VARCHAR(10), @dl_lot NVARCHAR(50), @dl_wh BIGINT, @dl_loc BIGINT, @dl_no VARCHAR(20);

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_insp_qty IS NULL OR @p_insp_qty <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사수량은 0보다 커야 합니다.'; RETURN;
            END
            IF @pass < 0 OR @conc < 0 OR @fail < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'합격/특채/불합격 수량은 음수일 수 없습니다.'; RETURN;
            END
            IF @pass + @conc + @fail <> @p_insp_qty
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'합격+특채+불합격(' + CAST(CAST(@pass + @conc + @fail AS FLOAT) AS NVARCHAR(30)) + N')이 검사수량('
                               + CAST(CAST(@p_insp_qty AS FLOAT) AS NVARCHAR(30)) + N')과 다릅니다.';
                RETURN;
            END
            IF @fail > 0 AND (ISNULL(@p_fail_reason_cd, '') = '' OR ISNULL(@p_fail_action_cd, '') = '')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'불합격 수량이 있으면 불량유형과 처분(반품/폐기)을 입력해야 합니다.'; RETURN;
            END
            IF @fail > 0 AND NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0008' AND minor_cd = @p_fail_action_cd AND use_yn = 'Y')
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'선택할 수 없는 불합격 처분입니다.'; RETURN;
            END
            IF @conc > 0 AND ISNULL(LTRIM(RTRIM(@p_remark)), N'') = N''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'특채 수량이 있으면 비고에 특채 사유를 입력해야 합니다.'; RETURN;
            END
            IF ISNULL(@src_type, '') <> 'DELV' OR @src_id IS NULL OR @src_serl IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 품목은 검사대기 불러오기로만 추가할 수 있습니다.'; RETURN;
            END

            DECLARE @dl_qty NUMERIC(18,4), @dl_next NUMERIC(18,4), @dl_qc VARCHAR(1), @dl_stop VARCHAR(1), @dm_stat VARCHAR(10), @dm_cust BIGINT;
            SELECT @dl_qty = dl.delv_qty, @dl_next = ISNULL(dl.next_qty, 0), @dl_qc = dl.qc_yn, @dl_stop = ISNULL(dl.stop_yn, 'N'),
                   @dm_stat = dm.stat_cd, @dm_cust = dm.cust_id, @dl_item = dl.item_id, @dl_unit = dl.unit_cd, @dl_lot = dl.lot_no,
                   @dl_wh = dl.wh_id, @dl_loc = dl.loc_id, @dl_no = dm.delv_no
            FROM TMADELVD dl JOIN TMADELVM dm ON dm.delv_id = dl.delv_id
            WHERE dl.delv_id = @src_id AND dl.serl = @src_serl;

            IF @dl_qty IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 납품 품목을 찾을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@dm_stat, '') <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 납품만 검사할 수 있습니다. (' + @dl_no + N')'; RETURN;
            END
            IF ISNULL(@dl_qc, 'N') <> 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사대상이 아닌(검사 면제) 납품 품목입니다. (' + @dl_no + N')'; RETURN;
            END
            IF @dl_stop = 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'마감된 납품 품목입니다. (' + @dl_no + N')'; RETURN;
            END
            IF @dm_cust <> @hdr_cust
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사 거래처와 납품 거래처가 다릅니다. (' + @dl_no + N')'; RETURN;
            END
            IF @p_insp_qty > @dl_qty - @dl_next
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'검사수량이 미검사 잔량(' + CAST(CAST(@dl_qty - @dl_next AS FLOAT) AS NVARCHAR(30)) + N')을 초과했습니다. (' + @dl_no + N')';
                RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAIQCD WHERE iqc_id = @p_iqc_id;

            INSERT INTO TMAIQCD (
                iqc_id, serl, acc_id, iqc_no, item_id, unit_cd, lot_no,
                insp_qty, sample_qty, pass_qty, conc_qty, fail_qty, fail_reason_cd, fail_action_cd, next_qty,
                wh_id, loc_id, src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.iqc_id, @nextSerl, m.acc_id, m.iqc_no, @dl_item, @dl_unit, @dl_lot,
                   @p_insp_qty, ISNULL(@p_sample_qty, @p_insp_qty), @pass, @conc, @fail,
                   CASE WHEN @fail > 0 THEN @p_fail_reason_cd END, CASE WHEN @fail > 0 THEN @p_fail_action_cd END, 0,
                   ISNULL(@p_wh_id, @dl_wh), ISNULL(@p_loc_id, @dl_loc), 'DELV', @src_id, @dl_no, @src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAIQCM m WHERE m.iqc_id = @p_iqc_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAIQCD SET
                insp_qty = @p_insp_qty,
                sample_qty = ISNULL(@p_sample_qty, @p_insp_qty),
                pass_qty = @pass,
                conc_qty = @conc,
                fail_qty = @fail,
                fail_reason_cd = CASE WHEN @fail > 0 THEN @p_fail_reason_cd END,
                fail_action_cd = CASE WHEN @fail > 0 THEN @p_fail_action_cd END,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE iqc_id = @p_iqc_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAIQCD WHERE iqc_id = @p_iqc_id AND serl = @p_serl;
        END

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
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
-- 4) USP_MA_IQC_C_S - 확정(C) / 확정취소(CC)
--    확정: 납품 라인(UPDLOCK)을 다시 검증(확정 납품/검사대상/거래처/누적 미검사 잔량)한 뒤 stat_cd='C',
--          납품 라인 next_qty와 발주 라인 next_qty(불합격 반품분 복원)를 재계산.
--    확정취소: 입고가 진행된 라인(next_qty>0)이 있으면 불가.
--    입고방식이 자동일 때 합격+특채분 자동 입고 호출은 입고 프로시저가 완성된 뒤(P4) 여기 연결한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQC_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_iqc_id BIGINT = NULL,
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
        DECLARE @stat VARCHAR(10), @cust BIGINT;
        SELECT @stat = stat_cd, @cust = cust_id FROM TMAIQCM WHERE iqc_id = @p_iqc_id;

        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 문서를 찾을 수 없습니다.'; RETURN;
        END

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 확정된 수입검사입니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TMAIQCD WHERE iqc_id = @p_iqc_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사 품목이 없어 확정할 수 없습니다.'; RETURN;
            END

            BEGIN TRAN;

            DECLARE @chk TABLE (delv_id BIGINT, serl INT, delv_no VARCHAR(20), this_qty NUMERIC(18,4), delv_qty NUMERIC(18,4),
                                delv_next NUMERIC(18,4), qc_yn VARCHAR(1), stop_yn VARCHAR(1), stat_cd VARCHAR(10), dm_cust BIGINT,
                                po_id BIGINT, po_serl INT);
            INSERT INTO @chk (delv_id, serl, delv_no, this_qty, delv_qty, delv_next, qc_yn, stop_yn, stat_cd, dm_cust, po_id, po_serl)
            SELECT dl.delv_id, dl.serl, dm.delv_no, SUM(iq.insp_qty), MAX(dl.delv_qty), MAX(ISNULL(dl.next_qty, 0)),
                   MAX(ISNULL(dl.qc_yn, 'N')), MAX(ISNULL(dl.stop_yn, 'N')), MAX(dm.stat_cd), MAX(dm.cust_id),
                   MAX(CASE WHEN dl.src_type = 'PO' THEN dl.src_id END), MAX(CASE WHEN dl.src_type = 'PO' THEN dl.src_serl END)
            FROM TMAIQCD iq
                JOIN TMADELVD dl WITH (UPDLOCK, ROWLOCK) ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                JOIN TMADELVM dm ON dm.delv_id = dl.delv_id
            WHERE iq.iqc_id = @p_iqc_id
            GROUP BY dl.delv_id, dl.serl, dm.delv_no;

            IF (SELECT COUNT(*) FROM TMAIQCD WHERE iqc_id = @p_iqc_id) <> (SELECT COUNT(*) FROM TMAIQCD iq JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl WHERE iq.iqc_id = @p_iqc_id)
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 납품 품목을 찾을 수 없는 검사 품목이 있습니다.'; RETURN;
            END

            DECLARE @bad_no VARCHAR(20);
            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE ISNULL(stat_cd, '') <> 'C';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정되지 않은(또는 확정취소된) 납품이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE qc_yn <> 'Y' OR stop_yn = 'Y';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사대상이 아니거나 마감된 납품 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE dm_cust <> @cust;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사 거래처와 납품 거래처가 다른 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = delv_no FROM @chk WHERE this_qty > delv_qty - delv_next;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'검사수량이 납품 미검사 잔량을 초과한 품목이 있습니다. (' + @bad_no + N') 다른 검사가 먼저 확정되었을 수 있습니다.'; RETURN;
            END

            UPDATE TMAIQCM SET
                stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE iqc_id = @p_iqc_id;

            DECLARE @d BIGINT, @ds INT, @p BIGINT, @ps INT;
            DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT delv_id, serl, po_id, po_serl FROM @chk;
            OPEN c;
            FETCH NEXT FROM c INTO @d, @ds, @p, @ps;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'DELV', @d, @ds;
                IF @p IS NOT NULL EXEC USP_MA_NEXTQTY_R 'PO', @p, @ps;
                FETCH NEXT FROM c INTO @d, @ds, @p, @ps;
            END
            CLOSE c;
            DEALLOCATE c;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 수입검사만 확정취소할 수 있습니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TMAIQCD WHERE iqc_id = @p_iqc_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'입고가 진행된 수입검사는 확정취소할 수 없습니다.'; RETURN;
            END

            BEGIN TRAN;

            UPDATE TMAIQCM SET
                stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE iqc_id = @p_iqc_id;

            DECLARE @d2 BIGINT, @ds2 INT, @p2 BIGINT, @ps2 INT;
            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
                SELECT DISTINCT dl.delv_id, dl.serl, CASE WHEN dl.src_type = 'PO' THEN dl.src_id END, CASE WHEN dl.src_type = 'PO' THEN dl.src_serl END
                FROM TMAIQCD iq JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                WHERE iq.iqc_id = @p_iqc_id;
            OPEN c2;
            FETCH NEXT FROM c2 INTO @d2, @ds2, @p2, @ps2;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'DELV', @d2, @ds2;
                IF @p2 IS NOT NULL EXEC USP_MA_NEXTQTY_R 'PO', @p2, @ps2;
                FETCH NEXT FROM c2 INTO @d2, @ds2, @p2, @ps2;
            END
            CLOSE c2;
            DEALLOCATE c2;

            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_iqc_id AS VARCHAR(20));
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
-- 5) USP_MA_IQCLIST_Q - 수입검사현황. Q: 헤더 목록, Q1: 한 검사의 라인
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_IQCLIST_Q
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
            WHERE (@p_iqc_no IS NULL OR @p_iqc_no = '' OR m.iqc_no LIKE '%' + @p_iqc_no + '%')
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
-- 6) USP_MA_DELVIQCPICK_Q - "검사대기 불러오기" 팝업. 확정 납품 중 검사대상(qc_yn='Y')이고 마감 아니며
--    미검사 잔량(delv_qty - next_qty)이 있는 라인.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELVIQCPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 납품번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명/규격 */
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
                m.delv_id, d.serl AS delv_serl, m.delv_no, m.delv_date, m.vendor_doc_no,
                m.cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd, d.lot_no,
                d.delv_qty, ISNULL(d.next_qty, 0) AS next_qty, (d.delv_qty - ISNULL(d.next_qty, 0)) AS wait_qty,
                DATEDIFF(DAY, TRY_CONVERT(DATE, m.delv_date, 112), GETDATE()) AS wait_days,
                d.src_no AS po_no, d.wh_id, w.wh_nm, d.loc_id, l.loc_nm, d.remark
            FROM TMADELVD d
                JOIN TMADELVM m ON m.delv_id = d.delv_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = d.loc_id
            WHERE m.stat_cd = 'C'
              AND ISNULL(d.qc_yn, 'N') = 'Y'
              AND ISNULL(d.stop_yn, 'N') <> 'Y'
              AND d.delv_qty - ISNULL(d.next_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.delv_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.delv_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.delv_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR m.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR i.item_no LIKE '%' + @p_keyword + '%'
                   OR i.item_nm LIKE '%' + @p_keyword + '%'
                   OR i.item_spec LIKE '%' + @p_keyword + '%')
            ORDER BY m.delv_date, m.delv_no, d.serl;
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
-- 7) VMA_GR_READY - 입고대기 뷰. 입고(사장님 설계)가 여기서 불러간다(설계서 7장 인계 계약).
--    두 집합을 UNION ALL:
--      * 검사대상: 확정된 수입검사 라인. src_type='IQC', ready_qty = 합격+특채
--      * 검사면제: 확정된 납품 라인 중 qc_yn='N'. src_type='DELV', ready_qty = 납품수량
--    next_qty는 입고가 갱신(원천 라인 next_qty = 확정된 입고수량 합), remain_qty = ready_qty - next_qty.
--    po_id/po_no/po_serl은 발주까지의 추적 키 - 매입 단가는 입고 라인이 복사하지 말고 이 키로 TMAPOD에서 조회한다.
--    stock_yn은 발주 라인 스냅샷(재고 갱신 여부 - 수불은 항상 생성, 재고 갱신은 Y일 때만).
-- ============================================================
CREATE OR ALTER VIEW dbo.VMA_GR_READY
AS
SELECT
    im.acc_id, 'IQC' AS src_type, iq.iqc_id AS src_id, im.iqc_no AS src_no, iq.serl AS src_serl,
    im.cust_id, c.cust_nm, iq.item_id, i.item_no, i.item_nm, i.item_spec, iq.unit_cd, iq.lot_no, iq.wh_id, iq.loc_id,
    (iq.pass_qty + iq.conc_qty) AS ready_qty,
    ISNULL(iq.next_qty, 0) AS next_qty,
    (iq.pass_qty + iq.conc_qty) - ISNULL(iq.next_qty, 0) AS remain_qty,
    dl.src_id AS po_id, dl.src_no AS po_no, dl.src_serl AS po_serl,
    ISNULL(pd.stock_yn, 'N') AS stock_yn,
    im.cfm_dt AS ready_date
FROM TMAIQCD iq
    JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
    JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
    LEFT JOIN TMAPOD pd ON pd.po_id = dl.src_id AND pd.serl = dl.src_serl AND dl.src_type = 'PO'
    LEFT JOIN TBAITEM i ON i.item_id = iq.item_id
    LEFT JOIN TBACUST c ON c.cust_id = im.cust_id
UNION ALL
SELECT
    dm.acc_id, 'DELV' AS src_type, dl.delv_id AS src_id, dm.delv_no AS src_no, dl.serl AS src_serl,
    dm.cust_id, c.cust_nm, dl.item_id, i.item_no, i.item_nm, i.item_spec, dl.unit_cd, dl.lot_no, dl.wh_id, dl.loc_id,
    dl.delv_qty AS ready_qty,
    ISNULL(dl.next_qty, 0) AS next_qty,
    dl.delv_qty - ISNULL(dl.next_qty, 0) AS remain_qty,
    dl.src_id AS po_id, dl.src_no AS po_no, dl.src_serl AS po_serl,
    ISNULL(pd.stock_yn, 'N') AS stock_yn,
    dm.cfm_dt AS ready_date
FROM TMADELVD dl
    JOIN TMADELVM dm ON dm.delv_id = dl.delv_id AND dm.cfm_yn = 'Y'
    LEFT JOIN TMAPOD pd ON pd.po_id = dl.src_id AND pd.serl = dl.src_serl AND dl.src_type = 'PO'
    LEFT JOIN TBAITEM i ON i.item_id = dl.item_id
    LEFT JOIN TBACUST c ON c.cust_id = dm.cust_id
WHERE ISNULL(dl.qc_yn, 'N') = 'N';
GO

-- ============================================================
-- 8) USP_MA_POSTATUS_Q - 구매진행현황. 승인 완료된 발주 라인 단위로 납품/검사/입고대기 진행을 한 줄에 보여준다.
--    p_status_type: ''=전체, UNDELV=미납품(잔량 있음+마감 아님), LATE=납기지연, IQCWAIT=검사대기,
--                   FAIL=불합격 발생, GRREADY=입고대기
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POSTATUS_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_status_type VARCHAR(20) = NULL,
    @p_po_no VARCHAR(20) = NULL,
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
            DECLARE @today DATE = CAST(GETDATE() AS DATE);

            WITH base AS (
                SELECT
                    m.po_id, d.serl AS po_serl, m.po_no, m.po_date, m.cust_id, c.cust_nm,
                    d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                    d.qty AS po_qty,
                    ISNULL((SELECT SUM(dl.delv_qty) FROM TMADELVD dl JOIN TMADELVM dm ON dm.delv_id = dl.delv_id AND dm.cfm_yn = 'Y'
                            WHERE dl.src_type = 'PO' AND dl.src_id = d.po_id AND dl.src_serl = d.serl), 0) AS delv_qty,
                    ISNULL(d.next_qty, 0) AS net_delv_qty,
                    d.qty - ISNULL(d.next_qty, 0) AS remain_qty,
                    ISNULL((SELECT SUM(dl.delv_qty - ISNULL(dl.next_qty, 0)) FROM TMADELVD dl JOIN TMADELVM dm ON dm.delv_id = dl.delv_id AND dm.cfm_yn = 'Y'
                            WHERE dl.src_type = 'PO' AND dl.src_id = d.po_id AND dl.src_serl = d.serl AND ISNULL(dl.qc_yn, 'N') = 'Y'), 0) AS iqc_wait_qty,
                    ISNULL((SELECT SUM(iq.fail_qty) FROM TMAIQCD iq JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
                            JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                            WHERE dl.src_type = 'PO' AND dl.src_id = d.po_id AND dl.src_serl = d.serl AND iq.fail_action_cd = 'RET'), 0) AS fail_ret_qty,
                    ISNULL((SELECT SUM(iq.fail_qty) FROM TMAIQCD iq JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
                            JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                            WHERE dl.src_type = 'PO' AND dl.src_id = d.po_id AND dl.src_serl = d.serl AND iq.fail_action_cd = 'SCRAP'), 0) AS fail_scrap_qty,
                    ISNULL((SELECT SUM(v.remain_qty) FROM dbo.VMA_GR_READY v WHERE v.po_id = d.po_id AND v.po_serl = d.serl), 0) AS gr_ready_qty,
                    d.delv_date, ISNULL(d.stop_yn, 'N') AS stop_yn, d.qc_yn
                FROM TMAPOD d
                    JOIN TMAPOM m ON m.po_id = d.po_id
                    LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                WHERE m.stat_cd = 'C'
                  AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
                  AND (@p_po_no IS NULL OR @p_po_no = '' OR m.po_no LIKE '%' + @p_po_no + '%')
                  AND (@p_date_from IS NULL OR @p_date_from = '' OR m.po_date >= @p_date_from)
                  AND (@p_date_to IS NULL OR @p_date_to = '' OR m.po_date <= @p_date_to)
                  AND (@p_keyword IS NULL OR @p_keyword = ''
                       OR c.cust_nm LIKE '%' + @p_keyword + '%'
                       OR i.item_no LIKE '%' + @p_keyword + '%'
                       OR i.item_nm LIKE '%' + @p_keyword + '%')
            ),
            calc AS (
                SELECT b.*,
                    CASE WHEN b.remain_qty > 0 AND b.stop_yn = 'N' AND TRY_CONVERT(DATE, b.delv_date, 112) IS NOT NULL
                              AND TRY_CONVERT(DATE, b.delv_date, 112) < @today
                         THEN DATEDIFF(DAY, TRY_CONVERT(DATE, b.delv_date, 112), @today) ELSE 0 END AS late_days
                FROM base b
            )
            SELECT *
            FROM calc
            WHERE ISNULL(@p_status_type, '') = ''
               OR (@p_status_type = 'UNDELV' AND remain_qty > 0 AND stop_yn = 'N')
               OR (@p_status_type = 'LATE' AND late_days > 0)
               OR (@p_status_type = 'IQCWAIT' AND iqc_wait_qty > 0)
               OR (@p_status_type = 'FAIL' AND fail_ret_qty + fail_scrap_qty > 0)
               OR (@p_status_type = 'GRREADY' AND gr_ready_qty > 0)
            ORDER BY po_date DESC, po_no DESC, po_serl;
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
