-- 구매 프로세스 P1 - 납품(TMADELVM/D) 프로시저(2026-09-25).
--
--  1) USP_MA_NEXTQTY_R        - next_qty 재계산 공용. 189번의 POREQ에 PO(발주 라인)/DELV(납품 라인) 대상을 더한다.
--  2) USP_MA_DELV_Q           - 납품 헤더 + 라인(0/1번 결과셋) - frmDelv
--  3) USP_MA_DELV_S           - 헤더 N/U/D (작성 상태에서만)
--  4) USP_MA_DELV_S_1         - 라인 N/U/D (발주 라인에서 불러온 것만, 원천/품목은 못 바꿈)
--  5) USP_MA_DELV_C_S         - 확정(C)/확정취소(CC) - 확정하는 순간 발주 라인 next_qty가 다시 계산된다
--  6) USP_MA_DELVLIST_Q       - 납품현황 Q(헤더 목록)/Q1(라인) - frmDelvList
--  7) USP_MA_PODELVPICK_Q     - frmDelv "발주 불러오기" 팝업용 - 승인 완료 발주 중 납품 가능 잔량이 있는 라인
--
-- 수량 규칙(설계서 4장): 발주 라인 next_qty = 확정된 납품수량 합 - 확정된 수입검사의 불합격 반품(RET) 수량 합.
-- 초과 납품은 프로세스 설정 MA.OVER_DELV_PCT(기본 0)까지만 허용 - 누적 납품이 발주수량*(1+허용율/100)을 넘을 수 없다.
-- 저장 때 한 번(화면에서 바로 오류를 보이려고), 확정 때 한 번(발주 라인을 UPDLOCK으로 잠근 채) 검증한다.
-- 미확정 작성본은 잔량을 잠그지 않는다.

-- ============================================================
-- 1) USP_MA_NEXTQTY_R
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_NEXTQTY_R
    @p_target VARCHAR(10),          /* 'POREQ' 구매요청 라인 / 'PO' 발주 라인 / 'DELV' 납품 라인 */
    @p_id BIGINT,                   /* POREQ: req_id, PO: po_id, DELV: delv_id */
    @p_serl INT = NULL              /* NULL이면 그 문서의 전체 라인 */
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_target = 'POREQ'
    BEGIN
        UPDATE d SET
            next_qty = ISNULL((
                SELECT SUM(po.qty)
                FROM TMAPOD po
                WHERE po.src_type = 'POREQ' AND po.src_id = d.req_id AND po.src_serl = d.serl
            ), 0)
        FROM TMAPOREQD d
        WHERE d.req_id = @p_id
          AND (@p_serl IS NULL OR d.serl = @p_serl);
    END
    ELSE IF @p_target = 'PO'
    BEGIN
        -- 확정된 납품수량 합 - 확정된 수입검사의 불합격 반품(RET) 수량 합(반품분은 납품으로 인정하지 않아 잔량이 복원된다)
        UPDATE p SET
            next_qty =
                ISNULL((SELECT SUM(dl.delv_qty)
                        FROM TMADELVD dl
                        JOIN TMADELVM dm ON dm.delv_id = dl.delv_id AND dm.cfm_yn = 'Y'
                        WHERE dl.src_type = 'PO' AND dl.src_id = p.po_id AND dl.src_serl = p.serl), 0)
              - ISNULL((SELECT SUM(iq.fail_qty)
                        FROM TMAIQCD iq
                        JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
                        JOIN TMADELVD dl ON iq.src_type = 'DELV' AND dl.delv_id = iq.src_id AND dl.serl = iq.src_serl
                        WHERE dl.src_type = 'PO' AND dl.src_id = p.po_id AND dl.src_serl = p.serl
                          AND iq.fail_action_cd = 'RET'), 0)
        FROM TMAPOD p
        WHERE p.po_id = @p_id
          AND (@p_serl IS NULL OR p.serl = @p_serl);
    END
    ELSE IF @p_target = 'DELV'
    BEGIN
        -- 검사대상 라인만: 확정된 수입검사의 검사수량 합. 무검사 라인은 입고(별도 설계)가 next_qty를 갱신한다.
        UPDATE d SET
            next_qty = ISNULL((
                SELECT SUM(iq.insp_qty)
                FROM TMAIQCD iq
                JOIN TMAIQCM im ON im.iqc_id = iq.iqc_id AND im.cfm_yn = 'Y'
                WHERE iq.src_type = 'DELV' AND iq.src_id = d.delv_id AND iq.src_serl = d.serl
            ), 0)
        FROM TMADELVD d
        WHERE d.delv_id = @p_id
          AND (@p_serl IS NULL OR d.serl = @p_serl)
          AND ISNULL(d.qc_yn, 'N') = 'Y';
    END
END
GO

-- ============================================================
-- 2) USP_MA_DELV_Q - 헤더(0번) + 라인(1번)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELV_Q
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
            WHERE (@p_delv_id IS NULL OR delv_id = @p_delv_id)
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
-- 3) USP_MA_DELV_S - 헤더 N/U/D. 작성(stat_cd='0') 상태에서만 수정/삭제. 채번은 공용 SSP_SYS_GetAutoKey.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELV_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_delv_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_vendor_doc_no NVARCHAR(50) = NULL,
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
            SELECT @cur_stat = stat_cd, @cur_cust = cust_id FROM TMADELVM WHERE delv_id = @p_delv_id;

            IF @cur_stat IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 문서를 찾을 수 없습니다.'; RETURN;
            END
            IF @cur_stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 납품은 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            IF @p_cust_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'거래처를 입력하세요.'; RETURN;
            END

            DECLARE @new_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMADELVM', 'delv_no', @p_acc_id, @new_no OUTPUT;

            INSERT INTO TMADELVM (
                acc_id, delv_no, delv_date, cust_id, vendor_doc_no, dept_id, emp_id, stat_cd, cfm_yn, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_no, @p_delv_date, @p_cust_id, @p_vendor_doc_no, @p_dept_id, @p_emp_id, '0', 'N', @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_delv_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            IF @p_cust_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'거래처를 입력하세요.'; RETURN;
            END
            -- 라인이 이미 있으면 거래처를 바꿀 수 없다(라인의 발주 협력사와 같아야 하므로)
            IF @p_cust_id <> @cur_cust AND EXISTS (SELECT 1 FROM TMADELVD WHERE delv_id = @p_delv_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'품목이 있는 납품은 거래처를 바꿀 수 없습니다. 품목을 지운 뒤 바꾸세요.'; RETURN;
            END

            UPDATE TMADELVM SET
                acc_id = @p_acc_id,
                delv_date = @p_delv_date,
                cust_id = @p_cust_id,
                vendor_doc_no = @p_vendor_doc_no,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE delv_id = @p_delv_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMADELVD WHERE delv_id = @p_delv_id;
            DELETE FROM TMADELVM WHERE delv_id = @p_delv_id;
        END

        SET @GeneratedCode = CAST(@p_delv_id AS VARCHAR(20));
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
-- 4) USP_MA_DELV_S_1 - 라인 N/U/D. 라인은 발주 라인에서 "불러온" 것만 만들 수 있고(src_type='PO'),
--    품목/원천은 만든 뒤 바꿀 수 없다(수정 때 화면이 보낸 src_*는 무시). 품목/단위/검사여부/창고는 발주 라인에서 복사.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELV_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_delv_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_delv_qty NUMERIC(18,4) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
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
        SELECT @hdr_stat = stat_cd, @hdr_cust = cust_id FROM TMADELVM WHERE delv_id = @p_delv_id;

        IF @hdr_stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'납품 문서를 찾을 수 없습니다.'; RETURN;
        END
        IF @hdr_stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 납품은 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
        END

        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT;
        IF @p_work_type = 'N'
        BEGIN
            SET @src_type = @p_src_type; SET @src_id = @p_src_id; SET @src_serl = @p_src_serl;
        END
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl
            FROM TMADELVD WHERE delv_id = @p_delv_id AND serl = @p_serl;

            IF @@ROWCOUNT = 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 품목을 찾을 수 없습니다.'; RETURN;
            END
        END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_delv_qty IS NULL OR @p_delv_qty <= 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품수량은 0보다 커야 합니다.'; RETURN;
            END
            IF ISNULL(@src_type, '') <> 'PO' OR @src_id IS NULL OR @src_serl IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 품목은 발주 불러오기로만 추가할 수 있습니다.'; RETURN;
            END

            DECLARE @po_qty NUMERIC(18,4), @po_next NUMERIC(18,4), @po_stop VARCHAR(1), @po_stat VARCHAR(10), @po_cust BIGINT,
                    @po_item BIGINT, @po_unit VARCHAR(10), @po_qc VARCHAR(1), @po_wh BIGINT, @po_loc BIGINT, @po_no VARCHAR(20);

            SELECT @po_qty = d.qty, @po_next = ISNULL(d.next_qty, 0), @po_stop = ISNULL(d.stop_yn, 'N'), @po_stat = m.stat_cd,
                   @po_cust = m.cust_id, @po_item = d.item_id, @po_unit = d.unit_cd, @po_qc = d.qc_yn,
                   @po_wh = d.wh_id, @po_loc = d.loc_id, @po_no = m.po_no
            FROM TMAPOD d JOIN TMAPOM m ON m.po_id = d.po_id
            WHERE d.po_id = @src_id AND d.serl = @src_serl;

            IF @po_qty IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 발주 품목을 찾을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@po_stat, '') <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료된 발주만 납품받을 수 있습니다. (' + @po_no + N')'; RETURN;
            END
            IF @po_stop = 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'마감된 발주 품목은 납품받을 수 없습니다. (' + @po_no + N')'; RETURN;
            END
            IF @po_cust <> @hdr_cust
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 거래처와 발주 거래처가 다릅니다. (' + @po_no + N')'; RETURN;
            END

            -- 초과 납품 허용: 발주수량 * (1 + 허용율/100) - 이미 확정된 납품 순량
            DECLARE @pct NUMERIC(9,4) = ISNULL(TRY_CAST(dbo.FSM_PROCCONFIG('MA.OVER_DELV_PCT') AS NUMERIC(9,4)), 0);
            DECLARE @allowed NUMERIC(18,4) = @po_qty * (1 + @pct / 100.0) - @po_next;
            IF @p_delv_qty > @allowed
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'납품수량이 발주 잔량(' + CAST(CAST(CASE WHEN @allowed < 0 THEN 0 ELSE @allowed END AS FLOAT) AS NVARCHAR(30))
                               + N')을 초과했습니다. (' + @po_no + N')';
                RETURN;
            END

            -- LOT 관리 품목은 LOT번호 필수
            IF EXISTS (SELECT 1 FROM TBAITEM WHERE item_id = @po_item AND lot_yn = 'Y') AND ISNULL(LTRIM(RTRIM(@p_lot_no)), N'') = N''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'LOT 관리 품목은 LOT번호를 입력해야 합니다. (' + @po_no + N')'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMADELVD WHERE delv_id = @p_delv_id;

            INSERT INTO TMADELVD (
                delv_id, serl, acc_id, delv_no, item_id, unit_cd, delv_qty, next_qty, lot_no, qc_yn,
                wh_id, loc_id, src_type, src_id, src_no, src_serl, stop_yn, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.delv_id, @nextSerl, m.acc_id, m.delv_no, @po_item, @po_unit, @p_delv_qty, 0, @p_lot_no, ISNULL(@po_qc, 'N'),
                   ISNULL(@p_wh_id, @po_wh), ISNULL(@p_loc_id, @po_loc), 'PO', @src_id, @po_no, @src_serl, 'N', @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMADELVM m WHERE m.delv_id = @p_delv_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMADELVD SET
                delv_qty = @p_delv_qty,
                lot_no = @p_lot_no,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE delv_id = @p_delv_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMADELVD WHERE delv_id = @p_delv_id AND serl = @p_serl;
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
-- 5) USP_MA_DELV_C_S - 확정(C) / 확정취소(CC)
--    확정: 라인이 1개 이상, 발주 라인(승인/마감/거래처/누적 수량)을 UPDLOCK으로 잠그고 다시 검증한 뒤 stat_cd='C',
--          발주 라인 next_qty 재계산.
--    확정취소: 검사 또는 입고가 진행된 라인(next_qty>0)이 있으면 불가. stat_cd='0'으로 되돌리고 재계산.
--    입고방식이 자동일 때의 자동 입고 호출은 입고 프로시저가 완성된 뒤(P4) 여기 연결한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELV_C_S
    @p_work_type VARCHAR(50),               /* C 확정 / CC 확정취소 */
    ---------------------------------------------------------------------------------------------------
    @p_delv_id BIGINT = NULL,
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
        SELECT @stat = stat_cd, @cust = cust_id FROM TMADELVM WHERE delv_id = @p_delv_id;

        IF @stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'납품 문서를 찾을 수 없습니다.'; RETURN;
        END

        IF @p_work_type = 'C'
        BEGIN
            IF @stat <> '0'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'이미 확정된 납품입니다.'; RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM TMADELVD WHERE delv_id = @p_delv_id)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 품목이 없어 확정할 수 없습니다.'; RETURN;
            END

            BEGIN TRAN;

            -- 이 납품이 참조하는 발주 라인을 잠그고 그 상태를 한 번에 읽는다(같은 발주 라인을 여러 줄이 쓰면 합산)
            DECLARE @chk TABLE (po_id BIGINT, serl INT, po_no VARCHAR(20), this_qty NUMERIC(18,4), po_qty NUMERIC(18,4),
                                po_next NUMERIC(18,4), stop_yn VARCHAR(1), stat_cd VARCHAR(10), po_cust BIGINT);
            INSERT INTO @chk (po_id, serl, po_no, this_qty, po_qty, po_next, stop_yn, stat_cd, po_cust)
            SELECT pd.po_id, pd.serl, pm.po_no, SUM(dl.delv_qty), MAX(pd.qty), MAX(ISNULL(pd.next_qty, 0)),
                   MAX(ISNULL(pd.stop_yn, 'N')), MAX(pm.stat_cd), MAX(pm.cust_id)
            FROM TMADELVD dl
                JOIN TMAPOD pd WITH (UPDLOCK, ROWLOCK) ON pd.po_id = dl.src_id AND pd.serl = dl.src_serl
                JOIN TMAPOM pm ON pm.po_id = pd.po_id
            WHERE dl.delv_id = @p_delv_id AND dl.src_type = 'PO'
            GROUP BY pd.po_id, pd.serl, pm.po_no;

            IF (SELECT COUNT(*) FROM TMADELVD WHERE delv_id = @p_delv_id) <> (SELECT COUNT(*) FROM TMADELVD dl JOIN TMAPOD pd ON pd.po_id = dl.src_id AND pd.serl = dl.src_serl WHERE dl.delv_id = @p_delv_id AND dl.src_type = 'PO')
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 발주 품목을 찾을 수 없는 납품 품목이 있습니다.'; RETURN;
            END

            DECLARE @bad_no VARCHAR(20);
            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE ISNULL(stat_cd, '') <> 'C';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료되지 않은 발주가 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE stop_yn = 'Y';
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'마감된 발주 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE po_cust <> @cust;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 거래처와 발주 거래처가 다른 품목이 있습니다. (' + @bad_no + N')'; RETURN;
            END

            DECLARE @pct NUMERIC(9,4) = ISNULL(TRY_CAST(dbo.FSM_PROCCONFIG('MA.OVER_DELV_PCT') AS NUMERIC(9,4)), 0);
            SELECT TOP 1 @bad_no = po_no FROM @chk WHERE this_qty > po_qty * (1 + @pct / 100.0) - po_next;
            IF @bad_no IS NOT NULL
            BEGIN
                ROLLBACK TRAN;
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품수량이 발주 잔량을 초과한 품목이 있습니다. (' + @bad_no + N') 다른 납품이 먼저 확정되었을 수 있습니다.'; RETURN;
            END

            UPDATE TMADELVM SET
                stat_cd = 'C', cfm_yn = 'Y', cfm_dt = GETDATE(), cfm_user_id = @p_user_id,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE delv_id = @p_delv_id;

            DECLARE @po BIGINT, @ps INT;
            DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT po_id, serl FROM @chk;
            OPEN c;
            FETCH NEXT FROM c INTO @po, @ps;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'PO', @po, @ps;
                FETCH NEXT FROM c INTO @po, @ps;
            END
            CLOSE c;
            DEALLOCATE c;

            COMMIT TRAN;
        END
        ELSE IF @p_work_type = 'CC'
        BEGIN
            IF @stat <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 납품만 확정취소할 수 있습니다.'; RETURN;
            END
            IF EXISTS (SELECT 1 FROM TMADELVD WHERE delv_id = @p_delv_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'수입검사 또는 입고가 진행된 납품은 확정취소할 수 없습니다.'; RETURN;
            END

            BEGIN TRAN;

            UPDATE TMADELVM SET
                stat_cd = '0', cfm_yn = 'N', cfm_dt = NULL, cfm_user_id = NULL,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE delv_id = @p_delv_id;

            DECLARE @po2 BIGINT, @ps2 INT;
            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
                SELECT DISTINCT src_id, src_serl FROM TMADELVD WHERE delv_id = @p_delv_id AND src_type = 'PO';
            OPEN c2;
            FETCH NEXT FROM c2 INTO @po2, @ps2;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'PO', @po2, @ps2;
                FETCH NEXT FROM c2 INTO @po2, @ps2;
            END
            CLOSE c2;
            DEALLOCATE c2;

            COMMIT TRAN;
        END

        SET @GeneratedCode = CAST(@p_delv_id AS VARCHAR(20));
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
-- 6) USP_MA_DELVLIST_Q - 납품현황. Q: 헤더 목록(조건 검색), Q1: 한 납품의 라인(검사상태 포함)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_DELVLIST_Q
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
            WHERE (@p_delv_no IS NULL OR @p_delv_no = '' OR m.delv_no LIKE '%' + @p_delv_no + '%')
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
-- 7) USP_MA_PODELVPICK_Q - "발주 불러오기" 팝업. 승인 완료 발주 중 마감 아니고 납품 가능량(허용율 포함)이
--    남은 라인. 팝업 공통 파라미터 이름(p_doc_no/p_keyword 등)은 popPick 규약을 따른다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PODELVPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 발주번호 */
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
            DECLARE @pct NUMERIC(9,4) = ISNULL(TRY_CAST(dbo.FSM_PROCCONFIG('MA.OVER_DELV_PCT') AS NUMERIC(9,4)), 0);

            SELECT
                m.po_id, d.serl AS po_serl, m.po_no, m.po_date, m.po_title,
                m.cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                d.qty, ISNULL(d.next_qty, 0) AS next_qty, (d.qty - ISNULL(d.next_qty, 0)) AS remain_qty,
                CASE WHEN d.qty * (1 + @pct / 100.0) - ISNULL(d.next_qty, 0) < 0 THEN 0
                     ELSE d.qty * (1 + @pct / 100.0) - ISNULL(d.next_qty, 0) END AS allowed_qty,
                d.delv_date, d.qc_yn, d.wh_id, w.wh_nm, d.loc_id, l.loc_nm, d.remark
            FROM TMAPOD d
                JOIN TMAPOM m ON m.po_id = d.po_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = d.loc_id
            WHERE m.stat_cd = 'C'
              AND ISNULL(d.stop_yn, 'N') <> 'Y'
              AND d.qty * (1 + @pct / 100.0) - ISNULL(d.next_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.po_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.po_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.po_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR m.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR i.item_no LIKE '%' + @p_keyword + '%'
                   OR i.item_nm LIKE '%' + @p_keyword + '%'
                   OR i.item_spec LIKE '%' + @p_keyword + '%')
            ORDER BY m.po_date DESC, m.po_no DESC, d.serl;
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
