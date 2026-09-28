-- P_ITEM_PO(구매 품목 팝업) 완성 - 구매단가(TMAPOPRICE)가 등록된 품목만 보여주고, 기준일에 적용되는 단가를 같이 보여준다
-- (2026-09-25). 구매요청등록(frmPoReq)의 품번 팝업이 이 팝업을 쓴다 - "단가가 등록된 품목만 구매요청/발주할 수 있다"는 규칙.
-- (구매단가등록 화면 자체는 단가 없는 품목도 골라 등록해야 하므로 일반 품목 팝업 P_ITEM을 쓴다.)
--
--  * 기준일(@p_base_date, 비우면 오늘)에 적용되는 단가(시작일 <= 기준일 <= 종료일)가 하나라도 있는 품목만 나온다.
--  * 품목당 한 행 - 대표단가: 전체 거래처 공통 단가가 있으면 그것, 없으면 거래처별 단가 중 시작일이 가장 최근인 것.
--    (팝업은 거래처를 모르므로 어느 거래처 단가인지는 cust_nm으로 보여준다. 공통 단가면 cust_id/cust_nm이 NULL)
--  * 기존 컬럼/파라미터는 그대로(라이브 정의 기준, 파라미터 7개 유지) - 결과에 cust_id, cust_nm, cur_cd, price, price_unit_cd,
--    price_id만 추가된다. 나머지 화면/팝업 정의는 영향 없음.

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
        JOIN pr ON pr.item_id = a.item_id AND pr.rn = 1
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

-- 팝업 결과 컬럼: 거래처/통화/구매단가를 보이게 추가(이미 있으면 갱신)
MERGE sysPopUpD AS t
USING (VALUES
    ('P_ITEM_PO', 'cust_nm',  N'단가거래처', 'TEXT',   CAST(NULL AS VARCHAR(50)), 12, 110, 'Y'),
    ('P_ITEM_PO', 'cur_cd',   N'통화',       'LOOKUP', 'L_CM0003',                13,  60, 'Y'),
    ('P_ITEM_PO', 'price',    N'구매단가',   'TEXT',   NULL,                      14,  90, 'Y')
) AS s (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
    ON t.popup_key = s.popup_key AND t.column_nm = s.column_nm
WHEN MATCHED THEN UPDATE SET caption = s.caption, control_type = s.control_type, lookup_proc_nm = s.lookup_proc_nm,
                             sort = s.sort, width = s.width, visible_yn = s.visible_yn
WHEN NOT MATCHED THEN INSERT (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
                      VALUES (s.popup_key, s.column_nm, s.caption, s.control_type, s.lookup_proc_nm, s.sort, s.width, s.visible_yn);
GO
