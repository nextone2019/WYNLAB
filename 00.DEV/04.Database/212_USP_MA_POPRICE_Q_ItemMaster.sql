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

CREATE OR ALTER PROCEDURE USP_MA_POPRICE_Q
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
                CASE WHEN p.end_date < @v_today THEN N'만료'
                     WHEN p.start_date > @v_today THEN N'예정'
                     ELSE N'적용중' END AS stat_nm
            FROM TMAPOPRICE p
                LEFT JOIN TBAITEM i ON i.item_id = p.item_id
                LEFT JOIN TBACUST c ON c.cust_id = p.cust_id
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
