-- SSP_POP_ITEM_PO_Q 수정(2026-09-25): 품목그룹 조건이 비어 있으면(팝업이 ''를 보냄 -> BIGINT 파라미터에서 0이 된다) 조건 없음으로 본다.
-- 203/204는 NULL만 "조건 없음"으로 처리해서, 팝업/화면 캐시가 ''를 보내면 grp1~4_id = 0인 품목만 나왔다 -
-- 품목을 팝업에서 골라도 화면의 품목 목록(캐시)에 그 품목이 없어 그리드에 반영되지 않던 원인.
-- (원래 정의의 "@p_grp1_id = ''" 검사가 하던 일을 명시적으로 = 0으로 되살린다.)

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
      AND (@p_grp1_id IS NULL OR @p_grp1_id = 0 OR a.grp1_id = @p_grp1_id)
      AND (@p_grp2_id IS NULL OR @p_grp2_id = 0 OR a.grp2_id = @p_grp2_id)
      AND (@p_grp3_id IS NULL OR @p_grp3_id = 0 OR a.grp3_id = @p_grp3_id)
      AND (@p_grp4_id IS NULL OR @p_grp4_id = 0 OR a.grp4_id = @p_grp4_id)
    ORDER BY a.item_no;
END
GO
