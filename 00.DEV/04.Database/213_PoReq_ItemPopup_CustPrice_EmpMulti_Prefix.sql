-- (1) 구매요청등록 품목 팝업(P_ITEM_PO)이 화면 헤더의 거래처/요청일자를 받아 그 기준의 단가를 보여준다(2026-09-26).
--   SSP_POP_ITEM_PO_Q에 @p_cust_id 추가. 단가 선택 규칙(기준일에 유효한 단가 중):
--     거래처가 넘어오면 -> 그 거래처에 등록된 단가가 있으면 그것, 없으면 거래처 없이(전체 거래처 공통) 등록된 단가. 다른 거래처의 단가는 안 보인다.
--     거래처가 없으면(NULL/0) -> 예전과 같음(공통 단가 우선, 없으면 가장 최근 시작한 단가).
--   기준일(@p_base_date)은 팝업 검색패널의 날짜 칸이 'yyyy-MM-dd'로 보내기도 해서(예전엔 VARCHAR(8)에 잘려 들어갔다) VARCHAR(10) + 하이픈 제거로 받는다.
--   프로시저 형식: 지역 변수는 AS 아래에 모으고 @v_ 접두사(2026-09-26 규칙). 반환 컬럼은 예전과 같다.
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_ITEM_PO_Q]
    @p_keyword          VARCHAR(100) = NULL,
    @p_asset_type       VARCHAR(20) = NULL,
    @p_base_date        VARCHAR(10) = NULL,
    @p_grp1_id          BIGINT = NULL,
    @p_grp2_id          BIGINT = NULL,
    @p_grp3_id          BIGINT = NULL,
    @p_grp4_id          BIGINT = NULL,
    @p_cust_id          BIGINT = NULL
AS
    DECLARE @v_bd VARCHAR(8);
BEGIN
    SET NOCOUNT ON;

    SET @v_bd = COALESCE(NULLIF(REPLACE(REPLACE(@p_base_date, '-', ''), '.', ''), ''), CONVERT(VARCHAR(8), GETDATE(), 112));

    ;WITH pr AS (
        SELECT p.price_id, p.item_id, p.cust_id, p.cur_cd, p.unit_cd AS price_unit_cd, p.price,
               ROW_NUMBER() OVER (PARTITION BY p.item_id
                                  ORDER BY CASE WHEN ISNULL(@p_cust_id, 0) > 0 AND p.cust_id = @p_cust_id THEN 0
                                                WHEN p.cust_id IS NULL THEN 1
                                                ELSE 2 END,
                                           p.start_date DESC, p.price_id DESC) AS rn
        FROM TMAPOPRICE p
        WHERE p.start_date <= @v_bd AND p.end_date >= @v_bd
          AND (ISNULL(@p_cust_id, 0) = 0 OR p.cust_id IS NULL OR p.cust_id = @p_cust_id)
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

-- (2) 사원정보일괄등록(frmEmpMulti) 메뉴의 프로시저 접두사 - USP_BA_EMPMULTI_로 돼 있어서 USP_BA_EMP_S/USP_BA_EMP_Q 호출이
--     "이 메뉴에서 사용할 수 없는 프로시저입니다"로 막혔다. frmEmp와 같은 USP_BA_EMP_로 맞춘다.
UPDATE TSMMENU SET PROC_PREFIX = 'USP_BA_EMP_'
WHERE MODULE = 'BA' AND SCREEN_CLASS_NM = 'frmEmpMulti' AND PROC_PREFIX <> 'USP_BA_EMP_';
GO
