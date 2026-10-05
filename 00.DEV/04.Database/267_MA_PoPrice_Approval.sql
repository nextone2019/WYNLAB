-- 267: 구매단가등록(frmPoPrice)에 전자결재 연결 - 결재 단위는 단가 한 줄(doc_id = TMAPOPRICE.price_id), 결재 문서유형 POPRICE(AP0002 기존).
--   TMAPOPRICE.app_id/app_no, 단가 목록 조회(Q2)에 app_id/app_no/appr_stat_cd, 저장(S)에 상신 후 잠금(종료일/비고 변경은 허용 - 단가개정용),
--   USP_AP_APPR_S_POPRICE(상신 시 결재번호 연결, 기안자 취소 시 해제, 승인/반려는 별도 후처리 없음 - 결재 이력만 남는다).
-- 여러 번 실행해도 안전하다. WYNLAB_DEV 전용.

IF COL_LENGTH('TMAPOPRICE', 'app_id') IS NULL ALTER TABLE TMAPOPRICE ADD app_id BIGINT NULL, app_no VARCHAR(20) NULL;
GO

CREATE OR ALTER PROCEDURE USP_AP_APPR_S_POPRICE
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_event = 'SUBMIT'
        UPDATE TMAPOPRICE SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id) WHERE price_id = @p_doc_id;
    ELSE IF @p_event = 'RESET'
        UPDATE TMAPOPRICE SET app_id = NULL, app_no = NULL WHERE price_id = @p_doc_id;
    -- 'APPROVE_END'/'UNDO_END'/'REJECT': 단가 자체는 바뀌지 않는다(승인 여부는 결재 상태로 표시).
END
GO
-- 구매단가등록(frmPoPrice) 구조 변경(2026-09-26) - 품목 마스터 중심 화면.
--   grd1 = 품목마스터(TBAITEM) 목록 + 품목별 "최종단가"(TMAPOPRICE)를 붙여서 보여준다.
--   panData = 선택한 품목의 정보, grd2 = 선택한 품목의 단가 목록(등록/수정/삭제는 USP_MA_POPRICE_S 그대로).
--
-- USP_MA_POPRICE_Q 작업구분:
--   'Q'  품목 목록 + 최종단가. 최종단가 = 오늘 이전(포함)에 시작한 단가 중 시작일이 가장 늦은 1건(거래처 무관).
--        오늘 이전에 시작한 단가가 없고 예정 단가만 있으면 가장 가까운 예정 단가를 보여준다. 단가가 하나도 없으면 최종단가 칸은 비어 있다.
--   'Q1' 기준일에 실제로 적용되는 단가 1건(발주/구매요청 화면이 사용) - 기존 그대로.
--   'Q2' 선택한 품목의 단가 목록(grd2) - 예전 'Q'(단가 행 조회)에서 품목 조건만 남긴 것.
-- 예전 'Q'의 p_cust_scope / p_valid_only 파라미터는 화면에서 안 쓰게 되어 뺐다(호출하는 곳은 frmPoPrice뿐).
-- 프로시저 형식: 지역 변수는 AS 아래(최상단 BEGIN 위)에 모으고 @v_ 접두사(2026-09-26 규칙).

CREATE OR ALTER PROCEDURE [dbo].[USP_MA_POPRICE_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_item_kw NVARCHAR(100) = NULL,       /* 'Q': 품번/품명 검색어(부분 일치) */
    @p_price_yn VARCHAR(1) = NULL,         /* 'Q': 'Y'=단가가 등록된 품목만, 'N'=단가가 없는 품목만, 그 외=전체 */
    @p_cust_id BIGINT = NULL,              /* 'Q1': 거래처 */
    @p_base_date VARCHAR(8) = NULL,        /* 'Q1': 단가를 찾을 기준일 */
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
    DECLARE @v_today VARCHAR(8);
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        SET @v_today = CONVERT(VARCHAR(8), GETDATE(), 112);

        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                i.acc_id, i.item_id, i.item_no, i.item_nm, i.item_spec,
                i.unit_cd, i.po_unit_cd, i.asset_type, i.stat_cd,
                i.cust_id, ic.cust_nm,
                i.remark,
                lp.price_id AS last_price_id,
                lp.price AS last_price,
                lp.cur_cd AS last_cur_cd,
                lp.unit_cd AS last_unit_cd,
                lp.cust_id AS last_cust_id,
                lc.cust_nm AS last_cust_nm,
                lp.start_date AS last_start_date,
                lp.end_date AS last_end_date,
                CASE WHEN lp.price_id IS NULL THEN NULL
                     WHEN lp.end_date < @v_today THEN N'만료'
                     WHEN lp.start_date > @v_today THEN N'예정'
                     ELSE N'적용중' END AS last_stat_nm
            FROM TBAITEM i
                LEFT JOIN TBACUST ic ON ic.cust_id = i.cust_id
                OUTER APPLY (
                    SELECT TOP 1 p.price_id, p.price, p.cur_cd, p.unit_cd, p.cust_id, p.start_date, p.end_date
                    FROM TMAPOPRICE p
                    WHERE p.item_id = i.item_id
                      AND (@p_acc_id IS NULL OR p.acc_id = @p_acc_id)
                    ORDER BY CASE WHEN p.start_date <= @v_today THEN 0 ELSE 1 END,
                             CASE WHEN p.start_date <= @v_today THEN p.start_date END DESC,
                             p.start_date ASC,
                             p.price_id DESC
                ) lp
                LEFT JOIN TBACUST lc ON lc.cust_id = lp.cust_id
            WHERE (@p_acc_id IS NULL OR i.acc_id = @p_acc_id)
              AND (@p_item_kw IS NULL OR @p_item_kw = N''
                   OR i.item_no LIKE N'%' + @p_item_kw + N'%'
                   OR i.item_nm LIKE N'%' + @p_item_kw + N'%')
              AND (ISNULL(@p_price_yn, '') NOT IN ('Y', 'N')
                   OR (@p_price_yn = 'Y' AND lp.price_id IS NOT NULL)
                   OR (@p_price_yn = 'N' AND lp.price_id IS NULL))
			  AND (i.asset_type IN (SELECT minor_cd FROM TSMMINOR WHERE major_cd = 'CM0002' AND rel_cd1 = 'Y'))	 --구매품에 해당하는 자산구분 건 만 
            ORDER BY i.item_no, i.item_id;
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
              AND p.start_date <= ISNULL(@p_base_date, @v_today)
              AND p.end_date >= ISNULL(@p_base_date, @v_today)
              AND (p.cust_id IS NULL OR p.cust_id = @p_cust_id)
            ORDER BY CASE WHEN p.cust_id IS NULL THEN 1 ELSE 0 END, p.start_date DESC;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            -- 선택한 품목의 단가 목록(전체 거래처 공통 단가가 맨 위, 그 다음 거래처별, 각각 시작일 최신순).
            SELECT
                p.price_id, p.acc_id,
                p.item_id, i.item_no, i.item_nm, i.item_spec,
                p.cust_id, c.cust_nm,
                p.start_date, p.end_date,
                p.cur_cd, p.unit_cd, p.price, p.remark,
                p.app_id, p.app_no, t.app_stat_cd AS appr_stat_cd,
                CASE WHEN p.end_date < @v_today THEN N'만료'
                     WHEN p.start_date > @v_today THEN N'예정'
                     ELSE N'적용중' END AS stat_nm
            FROM TMAPOPRICE p
                LEFT JOIN TBAITEM i ON i.item_id = p.item_id
                LEFT JOIN TBACUST c ON c.cust_id = p.cust_id
                LEFT JOIN TAPDOC t ON t.app_id = p.app_id
            WHERE p.item_id = @p_item_id
              AND (@p_acc_id IS NULL OR p.acc_id = @p_acc_id)
            ORDER BY CASE WHEN p.cust_id IS NULL THEN 0 ELSE 1 END, c.cust_nm, p.start_date DESC;
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
        -- 결재 상신된 단가(진행/승인완료)는 삭제할 수 없고, 단가/기간시작/거래처 등 내용도 못 바꾼다. 단 단가개정(이전 단가의 종료일을 줄임)과
        -- 비고 수정은 허용한다(개정해도 승인받은 단가 내용 자체는 그대로 남는다). 반려(R)된 단가는 다시 수정할 수 있다.
        IF @p_work_type IN ('U', 'D') AND dbo.FN_AP_IS_LOCKED((SELECT app_id FROM TMAPOPRICE WHERE price_id = @p_price_id)) = 1
        BEGIN
            IF @p_work_type = 'D' OR EXISTS (
                SELECT 1 FROM TMAPOPRICE
                WHERE price_id = @p_price_id
                  AND (item_id <> @p_item_id OR ISNULL(cust_id, 0) <> ISNULL(NULLIF(@p_cust_id, 0), 0)
                       OR start_date <> @p_start_date OR ISNULL(cur_cd, '') <> ISNULL(@p_cur_cd, '')
                       OR ISNULL(unit_cd, '') <> ISNULL(@p_unit_cd, '') OR ISNULL(price, -1) <> ISNULL(@p_price, -1)))
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'결재 상신된 단가는 수정하거나 삭제할 수 없습니다. (단가개정으로 종료일 정리/비고 수정만 가능)'; RETURN;
            END
        END
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
