-- 구매단가등록(frmPoPrice) 프로시저(2026-09-25). 테이블은 185_TMAPOPRICE_Table.sql.
--
--  * USP_MA_POPRICE_Q  : 'Q' 목록 조회(화면용), 'Q1' 기준일에 실제로 적용되는 단가 1건(발주 화면 등에서
--                        재사용 - 거래처 지정 단가가 있으면 그것을, 없으면 공통 단가를 돌려준다).
--  * USP_MA_POPRICE_S  : 행 하나 N/U/D. 필수값/기간/겹침 검증을 서버가 최종 책임진다(화면도 미리 검사하지만
--                        붙여넣기/다른 사용자의 동시 입력까지는 화면이 못 막는다).

-- ============================================================
-- 1) USP_MA_POPRICE_Q
--    거래처를 조건으로 주면 "그 거래처 단가 + 전체 거래처 공통 단가"를 같이 보여준다 - 공통 단가도
--    그 거래처에 적용되는 단가라서 빼면 오히려 헷갈린다. 공통만/거래처지정만 보고 싶으면 p_cust_scope.
--    stat_nm(만료/예정/적용중)은 "오늘" 기준이고, 기준일(p_base_date)+p_valid_only='Y'이면 그 날짜에
--    유효한 단가만 남긴다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POPRICE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_cust_id BIGINT = NULL,
    @p_cust_scope VARCHAR(10) = NULL,      /* NULL/'ALL'=전체, 'COMMON'=공통단가만, 'CUST'=거래처지정만 */
    @p_base_date VARCHAR(8) = NULL,        /* 'Q': valid_only='Y'일 때의 기준일, 'Q1': 단가를 찾을 기준일 */
    @p_valid_only VARCHAR(1) = 'N',
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
        DECLARE @today VARCHAR(8) = CONVERT(VARCHAR(8), GETDATE(), 112);

        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                p.price_id, p.acc_id,
                p.item_id, i.item_no, i.item_nm, i.item_spec,
                p.cust_id, c.cust_nm,
                p.start_date, p.end_date,
                p.cur_cd, p.unit_cd, p.price, p.remark,
                CASE WHEN p.end_date < @today THEN N'만료'
                     WHEN p.start_date > @today THEN N'예정'
                     ELSE N'적용중' END AS stat_nm
            FROM TMAPOPRICE p
                LEFT JOIN TBAITEM i ON i.item_id = p.item_id
                LEFT JOIN TBACUST c ON c.cust_id = p.cust_id
            WHERE (@p_acc_id IS NULL OR p.acc_id = @p_acc_id)
              AND (@p_item_id IS NULL OR p.item_id = @p_item_id)
              AND (@p_cust_id IS NULL OR p.cust_id = @p_cust_id OR p.cust_id IS NULL)
              AND (@p_cust_scope IS NULL OR @p_cust_scope IN ('', 'ALL')
                   OR (@p_cust_scope = 'COMMON' AND p.cust_id IS NULL)
                   OR (@p_cust_scope = 'CUST' AND p.cust_id IS NOT NULL))
              AND (ISNULL(@p_valid_only, 'N') <> 'Y' OR @p_base_date IS NULL
                   OR (p.start_date <= @p_base_date AND p.end_date >= @p_base_date))
            ORDER BY i.item_no, CASE WHEN p.cust_id IS NULL THEN 0 ELSE 1 END, c.cust_nm, p.start_date DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            -- 기준일에 적용되는 단가 1건: 거래처 지정 단가가 있으면 그것, 없으면 공통 단가.
            SELECT TOP 1
                p.price_id, p.item_id, p.cust_id, p.start_date, p.end_date,
                p.cur_cd, p.unit_cd, p.price
            FROM TMAPOPRICE p
            WHERE p.acc_id = @p_acc_id
              AND p.item_id = @p_item_id
              AND p.start_date <= ISNULL(@p_base_date, @today)
              AND p.end_date >= ISNULL(@p_base_date, @today)
              AND (p.cust_id IS NULL OR p.cust_id = @p_cust_id)
            ORDER BY CASE WHEN p.cust_id IS NULL THEN 1 ELSE 0 END, p.start_date DESC;
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
-- 2) USP_MA_POPRICE_S - 행 하나 N/U/D.
--    종료일이 비어 있으면 '99991231'(무기한). 같은 품목 + 같은 거래처 범위(거래처 지정끼리, 공통끼리)
--    안에서 기간이 겹치면 저장을 거부하고 겹치는 기존 기간을 메시지로 알려준다.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_MA_POPRICE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_price_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_cust_id BIGINT = NULL,
    @p_start_date VARCHAR(8) = NULL,
    @p_end_date VARCHAR(8) = NULL,
    @p_cur_cd VARCHAR(10) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
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
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_end_date IS NULL OR @p_end_date = '' SET @p_end_date = '99991231';
            IF @p_cust_id = 0 SET @p_cust_id = NULL;   -- 빈 값이 0으로 넘어와도 "전체 거래처"로 본다

            IF @p_acc_id IS NULL OR @p_item_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사업장과 품목은 필수입니다.'; RETURN;
            END
            IF ISDATE(@p_start_date) = 0 OR LEN(@p_start_date) <> 8 OR ISDATE(@p_end_date) = 0 OR LEN(@p_end_date) <> 8
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'적용 시작일/종료일 형식이 올바르지 않습니다.'; RETURN;
            END
            IF @p_start_date > @p_end_date
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'적용 시작일이 종료일보다 늦습니다.'; RETURN;
            END
            IF @p_price IS NULL OR @p_price < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'단가는 0 이상의 값이어야 합니다.'; RETURN;
            END

            -- 기간 겹침 검사(자기 자신 제외). 겹치는 기존 단가 중 하나의 기간을 메시지에 보여준다.
            DECLARE @dup_start VARCHAR(8), @dup_end VARCHAR(8);
            SELECT TOP 1 @dup_start = start_date, @dup_end = end_date
            FROM TMAPOPRICE
            WHERE acc_id = @p_acc_id
              AND item_id = @p_item_id
              AND ISNULL(cust_id, 0) = ISNULL(@p_cust_id, 0)
              AND price_id <> ISNULL(@p_price_id, 0)
              AND start_date <= @p_end_date
              AND end_date >= @p_start_date
            ORDER BY start_date;

            IF @dup_start IS NOT NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'같은 품목/거래처에 기간이 겹치는 단가가 있습니다. (' + @dup_start + N' ~ '
                                 + CASE WHEN @dup_end = '99991231' THEN N'무기한' ELSE @dup_end END + N')';
                RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TMAPOPRICE (
                acc_id, item_id, cust_id, start_date, end_date, cur_cd, unit_cd, price, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @p_item_id, @p_cust_id, @p_start_date, @p_end_date, @p_cur_cd, @p_unit_cd, @p_price, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_price_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TMAPOPRICE SET
                item_id = @p_item_id,
                cust_id = @p_cust_id,
                start_date = @p_start_date,
                end_date = @p_end_date,
                cur_cd = @p_cur_cd,
                unit_cd = @p_unit_cd,
                price = @p_price,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE price_id = @p_price_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TMAPOPRICE WHERE price_id = @p_price_id;
        END

        SET @GeneratedCode = CAST(@p_price_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
