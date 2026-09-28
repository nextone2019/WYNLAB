-- 구매 프로세스 P0 - 기존 요청/발주 보완(2026-09-25). 아래 프로시저는 모두 라이브 DB의 현재 정의를 기준으로
-- 다시 쓴 것이다(170/171번 파일과 라이브 정의가 이미 어긋나 있음 - 173/180번 등에서 고쳐진 채번/결재 컬럼).
--
--  1) next_qty를 화면이 보내는 값이 아니라 하위 문서(이번엔 발주 라인)의 수량 합으로 서버가 다시 계산한다.
--     요청 라인 next_qty = 그 요청 라인을 원천(src_type='POREQ')으로 가진 발주 라인 수량 합.
--     증감(+/-)이 아니라 합계 재계산이라 저장/삭제/재저장을 반복해도 어긋나지 않는다.
--  2) 요청 -> 발주 불러오기(USP_MA_POREQPICK_Q)와 발주 라인 저장 시 원천 검증(승인 완료된 요청,
--     같은 품목, 요청 잔량 이내).
--  3) 발주 라인 qc_yn/stock_yn을 품목(TBAITEM.po_qc_yn/stock_yn)에서 기본으로 채운다 - 지금까지
--     화면이 이 값을 보내지 않아 항상 NULL이었다.
--  4) 발주 라인 마감/마감취소(USP_MA_POSTOP_S) - TMAPOD.stop_yn/stop_emp_no/stop_remark는 있었지만
--     처리 프로시저가 없었다.
--  * 새로 쓰는 INSERT에는 reg_*와 함께 upt_*도 세팅한다(feedback: INSERT에도 upt 세팅).

-- ============================================================
-- 1) USP_MA_NEXTQTY_R - next_qty 재계산 공용. 지금은 요청(POREQ)만 - 납품/검사 단계가 생기면 대상을
--    이 프로시저에 추가한다. 다른 프로시저가 내부에서 부르는 용도라 출력 파라미터는 없다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_NEXTQTY_R
    @p_target VARCHAR(10),          /* 'POREQ' = 구매요청 라인 */
    @p_id BIGINT,                   /* POREQ: req_id */
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
END
GO

-- ============================================================
-- 2) USP_MA_POREQ_S - 헤더. 라이브 정의 + 삭제 시 발주에 사용된 요청 차단 + INSERT upt_* 세팅.
--    (승인취소로 결재가 초기화된 요청이라도 발주 라인이 붙어 있으면 지울 수 없다.)
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQ_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_req_date VARCHAR(8) = NULL,
    @p_req_title NVARCHAR(200) = NULL,
    @p_po_type VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(20) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_req_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAPOREQM', 'req_no', @p_acc_id, @new_req_no OUTPUT;

            INSERT INTO TMAPOREQM (
                acc_id, req_no, req_date, req_title, stat_cd, po_type,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_req_no, @p_req_date, @p_req_title, '0', @p_po_type,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_req_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOREQM SET
                acc_id = @p_acc_id,
                req_date = @p_req_date,
                req_title = @p_req_title,
                po_type = @p_po_type,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TMAPOREQM WHERE req_id = @p_req_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 구매요청은 삭제할 수 없습니다.';
                RETURN;
            END

            IF EXISTS (SELECT 1 FROM TMAPOREQD WHERE req_id = @p_req_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'발주에 사용된 품목이 있는 구매요청은 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TMAPOREQD WHERE req_id = @p_req_id;
            DELETE FROM TMAPOREQM WHERE req_id = @p_req_id;
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

-- ============================================================
-- 3) USP_MA_POREQ_S_1 - 요청 라인. next_qty는 더 이상 화면 값을 받지 않는다(@p_next_qty는 예전 화면
--    dll이 보내도 오류가 나지 않게 파라미터만 남기고 무시). 발주에 사용된 라인(next_qty>0)은
--    수정/삭제 불가.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQ_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,   /* 무시됨 - 서버가 USP_MA_NEXTQTY_R로 계산 */
    @p_unit_cd VARCHAR(10) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_no VARCHAR(20) = NULL,
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
        IF @p_work_type IN ('U', 'D')
           AND EXISTS (SELECT 1 FROM TMAPOREQD WHERE req_id = @p_req_id AND serl = @p_serl AND ISNULL(next_qty, 0) > 0)
        BEGIN
            SET @ReturnCode = -1;
            SET @ReturnMsg = N'발주에 사용된 구매요청 품목은 수정하거나 삭제할 수 없습니다.';
            RETURN;
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAPOREQD WHERE req_id = @p_req_id;

            INSERT INTO TMAPOREQD (
                req_id, serl, acc_id, req_no, item_id, qty, next_qty, unit_cd,
                cust_id, delv_date, wh_id, loc_id, src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.req_id, @nextSerl, m.acc_id, m.req_no, @p_item_id, @p_qty, 0, @p_unit_cd,
                   @p_cust_id, @p_delv_date, @p_wh_id, @p_loc_id, @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAPOREQM m WHERE m.req_id = @p_req_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOREQD SET
                item_id = @p_item_id,
                qty = @p_qty,
                unit_cd = @p_unit_cd,
                cust_id = @p_cust_id,
                delv_date = @p_delv_date,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                src_type = @p_src_type,
                src_id = @p_src_id,
                src_no = @p_src_no,
                src_serl = @p_src_serl,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE req_id = @p_req_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAPOREQD WHERE req_id = @p_req_id AND serl = @p_serl;
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
-- 4) USP_MA_PO_S - 헤더. 라이브 정의 + 삭제 시 원천 요청 잔량 복원(next_qty 재계산) + 납품이 붙은
--    발주 삭제 차단 + INSERT upt_* 세팅.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_po_date VARCHAR(8) = NULL,
    @p_po_type VARCHAR(10) = NULL,
    @p_po_title NVARCHAR(1000) = NULL,
    @p_cust_id BIGINT = NULL,
    @p_dept_id BIGINT = NULL,
    @p_emp_id BIGINT = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_cur_cd VARCHAR(10) = NULL,
    @p_exc_rate NUMERIC(18,4) = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(9,4) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_po_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAPOM', 'po_no', @p_acc_id, @new_po_no OUTPUT;

            INSERT INTO TMAPOM (
                acc_id, po_no, po_date, stat_cd, po_type, po_title,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate,
                delv_date, vat_type, vat_rate, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_po_no, @p_po_date, '0', @p_po_type, @p_po_title,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate,
                @p_delv_date, @p_vat_type, @p_vat_rate, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_po_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOM SET
                acc_id = @p_acc_id,
                po_date = @p_po_date,
                po_type = @p_po_type,
                po_title = @p_po_title,
                cust_id = @p_cust_id,
                dept_id = @p_dept_id,
                emp_id = @p_emp_id,
                pjt_id = @p_pjt_id,
                cur_cd = @p_cur_cd,
                exc_rate = @p_exc_rate,
                delv_date = @p_delv_date,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE po_id = @p_po_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TMAPOM WHERE po_id = @p_po_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 구매발주는 삭제할 수 없습니다.';
                RETURN;
            END

            IF EXISTS (SELECT 1 FROM TMAPOD WHERE po_id = @p_po_id AND ISNULL(next_qty, 0) > 0)
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'납품이 등록된 품목이 있는 구매발주는 삭제할 수 없습니다.';
                RETURN;
            END

            -- 지우기 전에 이 발주가 잡고 있던 요청 라인을 기억해 두었다가, 삭제 뒤 잔량을 복원한다.
            DECLARE @refs TABLE (src_id BIGINT, src_serl INT);
            INSERT INTO @refs (src_id, src_serl)
            SELECT DISTINCT src_id, src_serl FROM TMAPOD
            WHERE po_id = @p_po_id AND src_type = 'POREQ' AND src_id IS NOT NULL;

            DELETE FROM TMAPOD WHERE po_id = @p_po_id;
            DELETE FROM TMAPOM WHERE po_id = @p_po_id;

            DECLARE @r_id BIGINT, @r_serl INT;
            DECLARE ref_cur CURSOR LOCAL FAST_FORWARD FOR SELECT src_id, src_serl FROM @refs;
            OPEN ref_cur;
            FETCH NEXT FROM ref_cur INTO @r_id, @r_serl;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC USP_MA_NEXTQTY_R 'POREQ', @r_id, @r_serl;
                FETCH NEXT FROM ref_cur INTO @r_id, @r_serl;
            END
            CLOSE ref_cur;
            DEALLOCATE ref_cur;
        END

        SET @GeneratedCode = CAST(@p_po_id AS VARCHAR(20));
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
-- 5) USP_MA_PO_S_1 - 발주 라인.
--    * next_qty는 화면 값을 무시(@p_next_qty는 파라미터만 유지). 수량이 이미 납품된 수량(next_qty) 미만으로
--      줄거나, 납품이 붙은 라인을 지우는 것은 막는다(납품 단계가 생기기 전에는 항상 0이라 영향 없음).
--    * 원천이 구매요청(src_type='POREQ')이면 라인 등록/수정 때마다 승인 완료 여부, 품목 일치, 요청 라인
--      마감 여부, 요청 잔량을 검증하고, 저장/삭제 뒤 요청 라인 next_qty를 다시 계산한다.
--    * 원천(src_*)과 품목은 라인을 만든 뒤 바꿀 수 없다(수정 때 화면이 보낸 src_* 값은 무시).
--    * qc_yn/stock_yn을 화면이 안 보내면 품목의 po_qc_yn/stock_yn으로 채운다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_PO_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,   /* 무시됨 - 서버가 계산 */
    @p_price NUMERIC(18,4) = NULL,
    @p_amt NUMERIC(18,4) = NULL,
    @p_vat NUMERIC(18,4) = NULL,
    @p_total_amt NUMERIC(18,4) = NULL,
    @p_kor_price NUMERIC(18,4) = NULL,
    @p_kor_amt NUMERIC(18,4) = NULL,
    @p_kor_vat NUMERIC(18,4) = NULL,
    @p_kor_total_amt NUMERIC(18,4) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(9,4) = NULL,
    @p_delv_date VARCHAR(8) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_qc_yn VARCHAR(1) = NULL,
    @p_stock_yn VARCHAR(1) = NULL,
    @p_stock_unit_cd VARCHAR(10) = NULL,
    @p_stock_unit_qty NUMERIC(18,4) = NULL,
    @p_pjt_id BIGINT = NULL,
    @p_src_type VARCHAR(10) = NULL,
    @p_src_id BIGINT = NULL,
    @p_src_no VARCHAR(20) = NULL,
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
        -- 원천/기존 상태: 신규는 화면이 준 값, 수정/삭제는 이미 저장된 값(원천은 바꿀 수 없다).
        DECLARE @src_type VARCHAR(10), @src_id BIGINT, @src_serl INT, @cur_item_id BIGINT, @cur_next_qty NUMERIC(18,4),
                @cur_qc_yn VARCHAR(1), @cur_stock_yn VARCHAR(1);

        IF @p_work_type = 'N'
        BEGIN
            SET @src_type = @p_src_type; SET @src_id = @p_src_id; SET @src_serl = @p_src_serl;
        END
        ELSE
        BEGIN
            SELECT @src_type = src_type, @src_id = src_id, @src_serl = src_serl, @cur_item_id = item_id,
                   @cur_next_qty = ISNULL(next_qty, 0), @cur_qc_yn = qc_yn, @cur_stock_yn = stock_yn
            FROM TMAPOD WHERE po_id = @p_po_id AND serl = @p_serl;
        END

        -- 납품이 붙은 라인 보호
        IF @p_work_type = 'D' AND ISNULL(@cur_next_qty, 0) > 0
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'납품이 등록된 발주 품목은 삭제할 수 없습니다.'; RETURN;
        END
        IF @p_work_type = 'U' AND @p_qty < ISNULL(@cur_next_qty, 0)
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'이미 납품된 수량(' + CAST(CAST(@cur_next_qty AS FLOAT) AS NVARCHAR(30)) + N')보다 적게 수정할 수 없습니다.'; RETURN;
        END

        -- 구매요청 원천 검증(신규/수정)
        IF @p_work_type IN ('N', 'U') AND @src_type = 'POREQ'
        BEGIN
            DECLARE @req_qty NUMERIC(18,4), @req_stat VARCHAR(10), @req_item BIGINT, @req_stop VARCHAR(1), @used NUMERIC(18,4), @self_serl INT;

            SELECT @req_qty = d.qty, @req_stat = m.stat_cd, @req_item = d.item_id, @req_stop = ISNULL(d.stop_yn, 'N')
            FROM TMAPOREQD d JOIN TMAPOREQM m ON m.req_id = d.req_id
            WHERE d.req_id = @src_id AND d.serl = @src_serl;

            IF @req_qty IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 구매요청 품목을 찾을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@req_stat, '') <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료된 구매요청만 발주할 수 있습니다.'; RETURN;
            END
            IF @req_stop = 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'마감된 구매요청 품목은 발주할 수 없습니다.'; RETURN;
            END
            IF @req_item <> @p_item_id
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'구매요청에서 불러온 발주 품목은 품목을 바꿀 수 없습니다.'; RETURN;
            END

            -- 수정이면 자기 자신은 사용량에서 뺀다(신규는 자기 행이 아직 없다)
            SET @self_serl = ISNULL(@p_serl, -1);
            SELECT @used = ISNULL(SUM(qty), 0) FROM TMAPOD
            WHERE src_type = 'POREQ' AND src_id = @src_id AND src_serl = @src_serl
              AND NOT (po_id = @p_po_id AND serl = @self_serl);

            IF @used + ISNULL(@p_qty, 0) > @req_qty
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'발주수량이 구매요청 잔량(' + CAST(CAST(@req_qty - @used AS FLOAT) AS NVARCHAR(30)) + N')을 초과했습니다.';
                RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT, @item_qc VARCHAR(1), @item_stock VARCHAR(1);
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TMAPOD WHERE po_id = @p_po_id;
            SELECT @item_qc = po_qc_yn, @item_stock = stock_yn FROM TBAITEM WHERE item_id = @p_item_id;

            INSERT INTO TMAPOD (
                po_id, serl, acc_id, po_no, po_type, item_id, unit_cd, qty, next_qty,
                price, amt, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                vat_type, vat_rate, delv_date, wh_id, loc_id,
                qc_yn, stock_yn, stock_unit_cd, stock_unit_qty, pjt_id,
                src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.po_id, @nextSerl, m.acc_id, m.po_no, m.po_type, @p_item_id, @p_unit_cd, @p_qty, 0,
                   @p_price, @p_amt, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
                   @p_vat_type, @p_vat_rate, @p_delv_date, @p_wh_id, @p_loc_id,
                   ISNULL(@p_qc_yn, ISNULL(@item_qc, 'N')), ISNULL(@p_stock_yn, ISNULL(@item_stock, 'N')),
                   @p_stock_unit_cd, @p_stock_unit_qty, @p_pjt_id,
                   @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAPOM m WHERE m.po_id = @p_po_id;

            SET @p_serl = @nextSerl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            -- 기존 값이 NULL인 옛 라인(qc_yn/stock_yn을 화면이 안 보내던 시절)도 품목 값으로 채워 넣는다.
            DECLARE @u_item_qc VARCHAR(1), @u_item_stock VARCHAR(1);
            SELECT @u_item_qc = po_qc_yn, @u_item_stock = stock_yn FROM TBAITEM WHERE item_id = @p_item_id;

            UPDATE TMAPOD SET
                item_id = @p_item_id,
                unit_cd = @p_unit_cd,
                qty = @p_qty,
                price = @p_price,
                amt = @p_amt,
                vat = @p_vat,
                total_amt = @p_total_amt,
                kor_price = @p_kor_price,
                kor_amt = @p_kor_amt,
                kor_vat = @p_kor_vat,
                kor_total_amt = @p_kor_total_amt,
                vat_type = @p_vat_type,
                vat_rate = @p_vat_rate,
                delv_date = @p_delv_date,
                wh_id = @p_wh_id,
                loc_id = @p_loc_id,
                qc_yn = ISNULL(@p_qc_yn, ISNULL(@cur_qc_yn, ISNULL(@u_item_qc, 'N'))),
                stock_yn = ISNULL(@p_stock_yn, ISNULL(@cur_stock_yn, ISNULL(@u_item_stock, 'N'))),
                stock_unit_cd = @p_stock_unit_cd,
                stock_unit_qty = @p_stock_unit_qty,
                pjt_id = @p_pjt_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE po_id = @p_po_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAPOD WHERE po_id = @p_po_id AND serl = @p_serl;
        END

        IF @src_type = 'POREQ' AND @src_id IS NOT NULL
            EXEC USP_MA_NEXTQTY_R 'POREQ', @src_id, @src_serl;

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
-- 6) USP_MA_POSTOP_S - 발주 라인 마감/마감취소. 잔량이 남아도 그 라인을 종결하는 처리(납품 불러오기에서
--    제외된다). 승인 완료(stat_cd='C')된 발주의 라인만 대상이고, 마감에는 사유가 필요하며 잔량이 없는
--    라인은 마감할 필요가 없어 거부한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POSTOP_S
    @p_work_type VARCHAR(50),               /* 'U' */
    ---------------------------------------------------------------------------------------------------
    @p_po_id BIGINT = NULL,
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
            FROM TMAPOD d JOIN TMAPOM m ON m.po_id = d.po_id
            WHERE d.po_id = @p_po_id AND d.serl = @p_serl;

            IF @qty IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'발주 품목을 찾을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@stat_cd, '') <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료된 발주의 품목만 마감할 수 있습니다.'; RETURN;
            END

            IF @p_stop_yn = 'Y'
            BEGIN
                IF @p_stop_remark IS NULL OR LTRIM(RTRIM(@p_stop_remark)) = N''
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = N'마감 사유를 입력하세요.'; RETURN;
                END
                IF @qty - @next_qty <= 0
                BEGIN
                    SET @ReturnCode = -1; SET @ReturnMsg = N'잔량이 없는 발주 품목은 마감할 필요가 없습니다.'; RETURN;
                END

                DECLARE @emp_no VARCHAR(20);
                SELECT @emp_no = e.emp_no
                FROM TSMUSER u JOIN TBAEMP e ON e.emp_id = u.emp_id
                WHERE u.user_id = @p_user_id;

                UPDATE TMAPOD SET
                    stop_yn = 'Y',
                    stop_emp_no = @emp_no,
                    stop_remark = @p_stop_remark,
                    upt_user_id = @p_user_id,
                    upt_dt = GETDATE(),
                    upt_pc = @p_client_pc
                WHERE po_id = @p_po_id AND serl = @p_serl;
            END
            ELSE
            BEGIN
                UPDATE TMAPOD SET
                    stop_yn = 'N',
                    stop_emp_no = NULL,
                    stop_remark = NULL,
                    upt_user_id = @p_user_id,
                    upt_dt = GETDATE(),
                    upt_pc = @p_client_pc
                WHERE po_id = @p_po_id AND serl = @p_serl;
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

-- ============================================================
-- 7) USP_MA_POREQPICK_Q - frmPo "요청 불러오기" 팝업용. 승인 완료(stat_cd='C')된 요청 중 마감 아니고
--    잔량(qty - next_qty)이 남은 라인. cust_id는 요청 라인 거래처가 있으면 그것, 없으면 요청 헤더 거래처.
--    이 프로시저는 이름이 USP_MA_로 시작해서 frmPo 메뉴의 프로시저 화이트리스트를 그대로 통과한다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POREQPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_req_no VARCHAR(20) = NULL,
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
                m.req_id, d.serl AS req_serl, m.req_no, m.req_date, m.req_title,
                m.po_type, m.cur_cd, m.pjt_id,
                ISNULL(d.cust_id, m.cust_id) AS cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, i.item_spec, d.unit_cd,
                d.qty, ISNULL(d.next_qty, 0) AS next_qty, (d.qty - ISNULL(d.next_qty, 0)) AS remain_qty,
                d.delv_date, d.wh_id, w.wh_nm, d.loc_id, l.loc_nm,
                d.remark
            FROM TMAPOREQD d
                JOIN TMAPOREQM m ON m.req_id = d.req_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
                LEFT JOIN TBACUST c ON c.cust_id = ISNULL(d.cust_id, m.cust_id)
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TBALOC l ON l.loc_id = d.loc_id
            WHERE m.stat_cd = 'C'
              AND ISNULL(m.stop_yn, 'N') <> 'Y'
              AND ISNULL(d.stop_yn, 'N') <> 'Y'
              AND d.qty - ISNULL(d.next_qty, 0) > 0
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.req_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.req_date <= @p_date_to)
              AND (@p_req_no IS NULL OR @p_req_no = '' OR m.req_no LIKE '%' + @p_req_no + '%')
              AND (@p_cust_id IS NULL OR ISNULL(d.cust_id, m.cust_id) = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR i.item_no LIKE '%' + @p_keyword + '%'
                   OR i.item_nm LIKE '%' + @p_keyword + '%'
                   OR i.item_spec LIKE '%' + @p_keyword + '%')
            ORDER BY m.req_date DESC, m.req_no DESC, d.serl;
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
