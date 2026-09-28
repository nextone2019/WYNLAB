-- P_ITEM_PO: 구매단가가 없는 품목도 목록에 나오게 한다(2026-09-25, 사장님 지시 - JOIN pr -> LEFT JOIN pr).
-- 203의 "단가가 있는 품목만" 규칙을 폐기한다: 단가가 있으면 대표단가(거래처/통화/단가/단가단위)가 채워지고, 없으면 그 칸들만 NULL이다.
-- (그 외 정의는 203과 같다.)

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_ITEM_PO_Q]
    @p_keyword          VARCHAR(100) = NULL,
    @p_asset_type       VARCHAR(20) = NULL,
    @p_base_date        VARCHAR(8) = NULL,
    @p_grp1_id          BIGINT = NULL,
    @p_grp2_id          BIGINT = NULL,
    @p_grp3_id          BIGINT = NULL,
    @p_grp4_id          BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @bd VARCHAR(8) = COALESCE(NULLIF(@p_base_date, ''), CONVERT(VARCHAR(8), GETDATE(), 112));

    ;WITH pr AS (
        SELECT p.price_id, p.item_id, p.cust_id, p.cur_cd, p.unit_cd AS price_unit_cd, p.price,
               ROW_NUMBER() OVER (PARTITION BY p.item_id
                                  ORDER BY CASE WHEN p.cust_id IS NULL THEN 0 ELSE 1 END, p.start_date DESC, p.price_id DESC) AS rn
        FROM TMAPOPRICE p
        WHERE p.start_date <= @bd AND p.end_date >= @bd
    )
    SELECT a.ITEM_ID, a.item_no, a.item_nm, a.item_spec, a.asset_type,
           a.unit_cd, a.po_unit_cd,
           pr.cust_id, c.cust_nm, pr.cur_cd, pr.price, pr.price_unit_cd, pr.price_id,
           a.wh_id, w.wh_nm, a.loc_id, l.loc_nm
    FROM TBAITEM a
        LEFT JOIN pr ON pr.item_id = a.item_id AND pr.rn = 1
        LEFT JOIN TBACUST c ON c.cust_id = pr.cust_id
        LEFT JOIN TBAWH w ON w.wh_id = a.wh_id
        LEFT JOIN TBALOC l ON l.loc_id = a.loc_id
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR a.item_no LIKE '%' + @p_keyword + '%'
           OR a.item_nm LIKE '%' + @p_keyword + '%')
      AND (@p_asset_type IS NULL OR @p_asset_type = '' OR a.asset_type = @p_asset_type)
      AND (@p_grp1_id IS NULL OR a.grp1_id = @p_grp1_id)
      AND (@p_grp2_id IS NULL OR a.grp2_id = @p_grp2_id)
      AND (@p_grp3_id IS NULL OR a.grp3_id = @p_grp3_id)
      AND (@p_grp4_id IS NULL OR a.grp4_id = @p_grp4_id)
    ORDER BY a.item_no;
END
GO
