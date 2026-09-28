-- USP_MA_DELV_S_1을 새 프로시저 형식으로 정리(2026-09-26) - 동작은 그대로, 형식만 바꾼다.
--  * 프로시저 안에서 쓰는 지역 변수(DECLARE)는 전부 AS 바로 아래, 최상단 BEGIN 위쪽에 모은다.
--  * 지역 변수 이름에는 @v_ 접두사를 붙인다(입력 파라미터는 @p_, 출력 5종은 PascalCase 그대로).
--  * 초기값이 있던 DECLARE(@pct/@allowed)는 선언만 위로 올리고 값 대입(SET)은 원래 자리에 둔다 - 실행 시점/조건이 바뀌지 않게.
-- 기준 정의는 라이브 DB(2026-09-26 SSMS 스크립트)이다.

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
    ---------------------------------------------------------------------------------------------------
    -- 지역 변수 (@v_)
    ---------------------------------------------------------------------------------------------------
    DECLARE @v_hdr_stat VARCHAR(10);
    DECLARE @v_hdr_cust BIGINT;
    DECLARE @v_src_type VARCHAR(10);
    DECLARE @v_src_id BIGINT;
    DECLARE @v_src_serl INT;
    DECLARE @v_po_qty NUMERIC(18,4);
    DECLARE @v_po_next NUMERIC(18,4);
    DECLARE @v_po_stop VARCHAR(1);
    DECLARE @v_po_stat VARCHAR(10);
    DECLARE @v_po_cust BIGINT;
    DECLARE @v_po_item BIGINT;
    DECLARE @v_po_unit VARCHAR(10);
    DECLARE @v_po_qc VARCHAR(1);
    DECLARE @v_po_wh BIGINT;
    DECLARE @v_po_loc BIGINT;
    DECLARE @v_po_no VARCHAR(20);
    DECLARE @v_pct NUMERIC(9,4);
    DECLARE @v_allowed NUMERIC(18,4);
    DECLARE @v_next_serl INT;
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        SELECT @v_hdr_stat = stat_cd, @v_hdr_cust = cust_id FROM TMADELVM WHERE delv_id = @p_delv_id;

        IF @v_hdr_stat IS NULL
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'납품 문서를 찾을 수 없습니다.'; RETURN;
        END
        IF @v_hdr_stat <> '0'
        BEGIN
            SET @ReturnCode = -1; SET @ReturnMsg = N'확정된 납품은 수정하거나 삭제할 수 없습니다. 먼저 확정취소하세요.'; RETURN;
        END

        IF @p_work_type = 'N'
        BEGIN
            SET @v_src_type = @p_src_type; SET @v_src_id = @p_src_id; SET @v_src_serl = @p_src_serl;
        END
        ELSE
        BEGIN
            SELECT @v_src_type = src_type, @v_src_id = src_id, @v_src_serl = src_serl
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
            IF ISNULL(@v_src_type, '') <> 'PO' OR @v_src_id IS NULL OR @v_src_serl IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 품목은 발주 불러오기로만 추가할 수 있습니다.'; RETURN;
            END

            SELECT @v_po_qty = d.qty,
                   @v_po_next = ISNULL(d.next_qty, 0),
                   @v_po_stop = ISNULL(d.stop_yn, 'N'),
                   @v_po_stat = m.stat_cd,
                   @v_po_cust = m.cust_id, @v_po_item = d.item_id, @v_po_unit = d.unit_cd, @v_po_qc = d.qc_yn,
                   @v_po_wh = d.wh_id, @v_po_loc = d.loc_id, @v_po_no = m.po_no
            FROM TMAPOD d JOIN TMAPOM m ON m.po_id = d.po_id
            WHERE d.po_id = @v_src_id AND d.serl = @v_src_serl;

            IF @v_po_qty IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'원천 발주 품목을 찾을 수 없습니다.'; RETURN;
            END
            IF ISNULL(@v_po_stat, '') <> 'C'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'승인 완료된 발주만 납품받을 수 있습니다. (' + @v_po_no + N')'; RETURN;
            END
            IF @v_po_stop = 'Y'
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'마감된 발주 품목은 납품받을 수 없습니다. (' + @v_po_no + N')'; RETURN;
            END
            IF @v_po_cust <> @v_hdr_cust
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'납품 거래처와 발주 거래처가 다릅니다. (' + @v_po_no + N')'; RETURN;
            END

            -- 초과 납품 허용: 발주수량 * (1 + 허용율/100) - 이미 확정된 납품 순량
            SET @v_pct = ISNULL(TRY_CAST(dbo.FSM_PROCCONFIG('MA.OVER_DELV_PCT') AS NUMERIC(9,4)), 0);
            SET @v_allowed = @v_po_qty * (1 + @v_pct / 100.0) - @v_po_next;
            IF @p_delv_qty > @v_allowed
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'납품수량이 발주 잔량(' + CAST(CAST(CASE WHEN @v_allowed < 0 THEN 0 ELSE @v_allowed END AS FLOAT) AS NVARCHAR(30))
                               + N')을 초과했습니다. (' + @v_po_no + N')';
                RETURN;
            END

            -- LOT 관리 품목은 LOT번호 필수
            IF EXISTS (SELECT 1 FROM TBAITEM WHERE item_id = @v_po_item AND lot_yn = 'Y') AND ISNULL(LTRIM(RTRIM(@p_lot_no)), N'') = N''
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'LOT 관리 품목은 LOT번호를 입력해야 합니다. (' + @v_po_no + N')'; RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            SELECT @v_next_serl = ISNULL(MAX(serl), 0) + 1 FROM TMADELVD WHERE delv_id = @p_delv_id;

            INSERT INTO TMADELVD (
                delv_id, serl, acc_id, delv_no, item_id, unit_cd, delv_qty, next_qty, lot_no, qc_yn,
                wh_id, loc_id, src_type, src_id, src_no, src_serl, stop_yn, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            SELECT m.delv_id, @v_next_serl, m.acc_id, m.delv_no, @v_po_item, @v_po_unit, @p_delv_qty, 0, @p_lot_no, ISNULL(@v_po_qc, 'N'),
                   ISNULL(@p_wh_id, @v_po_wh), ISNULL(@p_loc_id, @v_po_loc), 'PO', @v_src_id, @v_po_no, @v_src_serl, 'N', @p_remark,
                   @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            FROM TMADELVM m WHERE m.delv_id = @p_delv_id;

            SET @p_serl = @v_next_serl;
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
