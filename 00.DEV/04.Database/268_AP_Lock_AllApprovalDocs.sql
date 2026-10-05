-- 268: 결재를 쓰는 모든 문서의 저장 프로시저에 "결재상신 이후 수정/삭제 불가" 잠금 추가(2026-10-04) - 구매요청/발주/수주/명함신청/기타출고요청.
--   dbo.FN_AP_IS_LOCKED(app_id): 결재상신(0)/진행중(1)/승인완료(E)면 잠금, 반려(R)나 결재 없음/상신취소는 수정 가능. 헤더 프로시저는 수정(U)/삭제(D), 품목 프로시저는 등록/수정/삭제 모두 막는다.
--   (기타입고/기타출고/기초재고/구매단가는 266/267에서 이미 적용). 라이브 프로시저 정의에서 만들었다. 여러 번 실행해도 안전(이미 잠금 줄이 있으면 건너뜀).

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
    @p_exc_rate NUMERIC(18,6) = NULL,
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
        IF @p_work_type IN ('U', 'D') AND dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAPOREQM WHERE req_id = @p_req_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_req_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TMAPOREQM', 'req_no', @p_acc_id, @new_req_no OUTPUT;

            INSERT INTO TMAPOREQM (
                acc_id, req_no, req_date, req_title, stat_cd, po_type,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @new_req_no, @p_req_date, @p_req_title, '0', @p_po_type,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate, @p_remark,
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
                exc_rate = @p_exc_rate,
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

CREATE OR ALTER PROCEDURE USP_MA_POREQ_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,   /* 무시됨 - 서버가 USP_MA_NEXTQTY_R로 계산 */
    @p_unit_cd VARCHAR(10) = NULL,
    @p_price NUMERIC(18,6) = NULL,
    @p_amt NUMERIC(18,6) = NULL,
    @p_vat_rate NUMERIC(18,6) = NULL,
    @p_vat NUMERIC(18,6) = NULL,
    @p_total_amt NUMERIC(18,6) = NULL,
    @p_kor_price NUMERIC(18,6) = NULL,
    @p_kor_amt NUMERIC(18,6) = NULL,
    @p_kor_vat NUMERIC(18,6) = NULL,
    @p_kor_total_amt NUMERIC(18,6) = NULL,
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
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAPOREQM WHERE req_id = @p_req_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

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
                price, amt, vat_rate, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                cust_id, delv_date, wh_id, loc_id, src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.req_id, @nextSerl, m.acc_id, m.req_no, @p_item_id, @p_qty, 0, @p_unit_cd,
                   @p_price, @p_amt, @p_vat_rate, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
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
                price = @p_price,
                amt = @p_amt,
                vat_rate = @p_vat_rate,
                vat = @p_vat,
                total_amt = @p_total_amt,
                kor_price = @p_kor_price,
                kor_amt = @p_kor_amt,
                kor_vat = @p_kor_vat,
                kor_total_amt = @p_kor_total_amt,
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


        -- 헤더(TMAPOREQM)의 금액 합계를 품목(TMAPOREQD) 합계로 매번 다시 맞춘다 - 등록/수정/삭제 어느 쪽이든 이 프로시저를 거치므로
        -- 헤더 금액이 품목과 어긋날 일이 없다(품목이 하나도 안 남으면 0).
        UPDATE m SET
            amt = ISNULL(s.amt, 0),
            vat = ISNULL(s.vat, 0),
            total_amt = ISNULL(s.total_amt, 0)
        FROM TMAPOREQM m
            OUTER APPLY (
                SELECT SUM(d.amt) AS amt, SUM(d.vat) AS vat, SUM(d.total_amt) AS total_amt
                FROM TMAPOREQD d WHERE d.req_id = m.req_id
            ) s
        WHERE m.req_id = @p_req_id;

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
        IF @p_work_type IN ('U', 'D') AND dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAPOM WHERE po_id = @p_po_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

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
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAPOM WHERE po_id = @p_po_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

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
-- USP_SA_SO_S - 헤더(TSASOM) N/U/D. 결재상신된 건 삭제 금지(PO와 동일).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SO_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_so_date VARCHAR(8) = NULL,
    @p_so_title NVARCHAR(1000) = NULL,
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
        IF @p_work_type IN ('U', 'D') AND dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TSASOM WHERE so_id = @p_so_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @new_so_no VARCHAR(20);
            EXEC SSP_SYS_GetAutoKey 'TSASOM', 'so_no', @p_acc_id, @new_so_no OUTPUT;

            INSERT INTO TSASOM (
                acc_id, so_no, so_date, stat_cd, so_title,
                cust_id, dept_id, emp_id, pjt_id, cur_cd, exc_rate,
                delv_date, vat_type, vat_rate, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @new_so_no, @p_so_date, '0', @p_so_title,
                @p_cust_id, @p_dept_id, @p_emp_id, @p_pjt_id, @p_cur_cd, @p_exc_rate,
                @p_delv_date, @p_vat_type, @p_vat_rate, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_so_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSASOM SET
                acc_id = @p_acc_id,
                so_date = @p_so_date,
                so_title = @p_so_title,
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
            WHERE so_id = @p_so_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            IF EXISTS (SELECT 1 FROM TSASOM WHERE so_id = @p_so_id AND app_no IS NOT NULL AND app_no <> '')
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 결재상신된 수주는 삭제할 수 없습니다.';
                RETURN;
            END

            DELETE FROM TSASOD WHERE so_id = @p_so_id;
            DELETE FROM TSASOM WHERE so_id = @p_so_id;
        END

        SET @GeneratedCode = CAST(@p_so_id AS VARCHAR(20));
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
-- USP_SA_SO_S_1 - 품목(TSASOD) 행별 N/U/D. 견적에서 불러온 행(src_type='QT')은 저장할 때마다
-- 원본 견적 라인(TSAQTD)의 next_qty를 이 수주에 걸린 수량 합계로 다시 계산한다(구매요청의
-- USP_MA_NEXTQTY_R와 같은 목적이지만, 지금은 QT->SO 관계 하나뿐이라 별도 범용 프로시저 없이
-- 이 프로시저 안에서 바로 계산한다 - 관계가 하나 더 늘면 그때 공용화한다).
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_SO_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_next_qty NUMERIC(18,4) = NULL,
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
    DECLARE @v_src_type VARCHAR(10), @v_src_id BIGINT, @v_src_serl INT;
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TSASOM WHERE so_id = @p_so_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @nextSerl INT;
            SELECT @nextSerl = ISNULL(MAX(serl), 0) + 1 FROM TSASOD WHERE so_id = @p_so_id;

            INSERT INTO TSASOD (
                so_id, serl, acc_id, so_no, item_id, unit_cd, qty, next_qty,
                price, amt, vat, total_amt, kor_price, kor_amt, kor_vat, kor_total_amt,
                vat_type, vat_rate, delv_date, wh_id, loc_id,
                stock_yn, stock_unit_cd, stock_unit_qty, pjt_id,
                src_type, src_id, src_no, src_serl, remark,
                reg_user_id, reg_dt, reg_pc
            )
            SELECT m.so_id, @nextSerl, m.acc_id, m.so_no, @p_item_id, @p_unit_cd, @p_qty, @p_next_qty,
                   @p_price, @p_amt, @p_vat, @p_total_amt, @p_kor_price, @p_kor_amt, @p_kor_vat, @p_kor_total_amt,
                   @p_vat_type, @p_vat_rate, @p_delv_date, @p_wh_id, @p_loc_id,
                   @p_stock_yn, @p_stock_unit_cd, @p_stock_unit_qty, @p_pjt_id,
                   @p_src_type, @p_src_id, @p_src_no, @p_src_serl, @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc
            FROM TSASOM m WHERE m.so_id = @p_so_id;

            SET @p_serl = @nextSerl;
            SET @v_src_type = @p_src_type; SET @v_src_id = @p_src_id; SET @v_src_serl = @p_src_serl;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            SELECT @v_src_type = src_type, @v_src_id = src_id, @v_src_serl = src_serl
            FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_serl;

            UPDATE TSASOD SET
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
                stock_yn = @p_stock_yn,
                stock_unit_cd = @p_stock_unit_cd,
                stock_unit_qty = @p_stock_unit_qty,
                pjt_id = @p_pjt_id,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE so_id = @p_so_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            SELECT @v_src_type = src_type, @v_src_id = src_id, @v_src_serl = src_serl
            FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_serl;

            DELETE FROM TSASOD WHERE so_id = @p_so_id AND serl = @p_serl;
        END

        -- 견적에서 불러온 라인이면 원본 견적 라인의 next_qty를 다시 계산(이 견적라인을 참조하는
        -- 모든 수주라인의 qty 합계) - 삭제/수정 후에도 항상 최신 값으로 맞춘다.
        IF @v_src_type = 'QT' AND @v_src_id IS NOT NULL
        BEGIN
            UPDATE TSAQTD
               SET next_qty = (
                   SELECT ISNULL(SUM(d.qty), 0) FROM TSASOD d
                   WHERE d.src_type = 'QT' AND d.src_id = @v_src_id AND d.src_serl = @v_src_serl
               )
             WHERE qt_id = @v_src_id AND serl = @v_src_serl;
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

/* ---------- USP_HR_NAMECARD_S: 명함신청서 등록/수정/삭제 ---------- */
CREATE OR ALTER PROCEDURE USP_HR_NAMECARD_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_job_grade VARCHAR(100) = NULL,
    @p_name_kor VARCHAR(50) = NULL,
    @p_name_eng VARCHAR(50) = NULL,
    @p_dept_kor VARCHAR(50) = NULL,
    @p_dept_eng VARCHAR(50) = NULL,
    @p_mobile NVARCHAR(50) = NULL,
    @p_email NVARCHAR(50) = NULL,
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
        IF @p_work_type IN ('U', 'D') AND dbo.FN_AP_IS_LOCKED((SELECT app_id FROM THRNAMECARDREQ WHERE req_id = @p_req_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        -- dept_id/emp_id는 화면이 몰라도 되게(Session에 DeptId가 없어서 EmpNo/DeptNm만 노출 -
        -- 화면 하나 때문에 공용 세션을 확장하는 대신) @p_user_id(로그인 세션, 클라이언트가 못
        -- 속임 - GenericDataRepository.SaveAsync가 항상 서버에서 채움)로 TSMUSER->TBAEMP를
        -- 타고 서버가 직접 채운다.
        DECLARE @dept_id BIGINT, @emp_id BIGINT;
        SELECT @emp_id = u.EMP_ID FROM TSMUSER u WHERE u.USER_ID = @p_user_id;
        SELECT @dept_id = DEPT_ID FROM TBAEMP WHERE EMP_ID = @emp_id;

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO THRNAMECARDREQ (
                req_no, dept_id, emp_id, job_grade, name_kor, name_eng, dept_kor, dept_eng,
                mobile, email, stat_cd, remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                '', @dept_id, @emp_id, @p_job_grade, @p_name_kor, @p_name_eng, @p_dept_kor, @p_dept_eng,
                @p_mobile, @p_email, '0', @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_req_id = SCOPE_IDENTITY();
            UPDATE THRNAMECARDREQ
               SET req_no = 'NC' + RIGHT('00000000' + CAST(@p_req_id AS VARCHAR(8)), 8)
             WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE THRNAMECARDREQ SET
                job_grade = @p_job_grade, name_kor = @p_name_kor, name_eng = @p_name_eng,
                dept_kor = @p_dept_kor, dept_eng = @p_dept_eng, mobile = @p_mobile, email = @p_email,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM THRNAMECARDREQ WHERE req_id = @p_req_id;
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
        IF @p_work_type IN ('U', 'D') AND dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAETCREQM WHERE req_id = @p_req_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

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

CREATE OR ALTER PROCEDURE USP_MA_ETCREQ_S_1
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_req_id BIGINT = NULL,
    @p_serl INT = NULL,
    @p_item_id BIGINT = NULL,
    @p_qty NUMERIC(18,4) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_wh_id BIGINT = NULL,
    @p_loc_id BIGINT = NULL,
    @p_lot_no NVARCHAR(50) = NULL,
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
        IF dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAETCREQM WHERE req_id = @p_req_id)) = 1
        BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 문서는 수정하거나 삭제할 수 없습니다.'; RETURN; END

        DECLARE @stat VARCHAR(10), @appr VARCHAR(10);
        SELECT @stat = m.stat_cd, @appr = t.app_stat_cd
        FROM TMAETCREQM m LEFT JOIN TAPDOC t ON t.app_id = m.app_id WHERE m.req_id = @p_req_id;

        IF @stat IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'기타출고요청을 찾을 수 없습니다.'; RETURN; END
        IF @stat = 'C' BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'승인완료된 기타출고요청은 수정할 수 없습니다.'; RETURN; END
        IF @appr IN ('0', '1') BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'결재가 진행 중인 기타출고요청은 수정할 수 없습니다.'; RETURN; END

        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_item_id IS NULL BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'품목을 입력하세요.'; RETURN; END
            IF @p_qty IS NULL OR @p_qty <= 0 BEGIN SET @ReturnCode = -1; SET @ReturnMsg = N'요청수량은 0보다 커야 합니다.'; RETURN; END
        END

        IF @p_work_type = 'N'
        BEGIN
            DECLARE @next INT;
            SELECT @next = ISNULL(MAX(serl), 0) + 1 FROM TMAETCREQD WHERE req_id = @p_req_id;

            INSERT INTO TMAETCREQD (req_id, serl, acc_id, req_no, item_id, unit_cd, qty, next_qty, wh_id, loc_id, lot_no, cfm_yn, stop_yn, remark,
                                    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
            SELECT m.req_id, @next, m.acc_id, m.req_no, @p_item_id, ISNULL(@p_unit_cd, i.unit_cd), @p_qty, 0, @p_wh_id, @p_loc_id, @p_lot_no, 'N', 'N', @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMAETCREQM m LEFT JOIN TBAITEM i ON i.item_id = @p_item_id
            WHERE m.req_id = @p_req_id;
            SET @p_serl = @next;
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAETCREQD SET
                item_id = @p_item_id, unit_cd = @p_unit_cd, qty = @p_qty, wh_id = @p_wh_id, loc_id = @p_loc_id, lot_no = @p_lot_no, remark = @p_remark,
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE req_id = @p_req_id AND serl = @p_serl;
        END
        ELSE IF @p_work_type = 'D'
            DELETE FROM TMAETCREQD WHERE req_id = @p_req_id AND serl = @p_serl;

        SET @GeneratedCode = CAST(@p_serl AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1; SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER(); SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

