-- P_ITEM(품목 팝업)에서 조회해도 아무 품목도 안 나오던 문제 수정 (2026-09-25).
--
-- 원인: 팝업 프레임워크는 sysPopUpS(조회조건 정의)에 있는 조건을 전부 프로시저 파라미터로 보낸다(값이 비어 있어도 NULL로).
-- P_ITEM의 조건 정의는 옛 품목 프로시저 기준(p_code, p_grp1~4_id, p_po_yn, p_sale_yn, p_prod_yn)인데, SSP_POP_ITEM_Q는
-- 169/182번에서 p_keyword/p_asset_type만 받도록 바뀌었다. 없는 파라미터(@p_code 등)를 넘기면 SQL Server가 프로시저 호출 자체를
-- 거부해서(오류 1FD1) 결과가 빈 목록으로 보였다 - 구매단가/구매요청/발주 등 P_ITEM을 쓰는 모든 화면의 품목 팝업이 해당된다.
--
-- 조치:
--  1) SSP_POP_ITEM_Q가 팝업이 보내는 조건을 모두 받게 확장한다(기존 호출은 그대로 동작: p_keyword/p_asset_type 유지).
--     - p_code: 품목코드/명(p_keyword와 같은 뜻 - 둘 중 값이 있는 쪽을 쓴다)
--     - p_grp1_id~p_grp4_id: 품목그룹 일치 조건(TBAITEM.grp1~4_id)
--     - p_asset_type은 라이브 정의 그대로 접두 일치(LIKE '값%')를 유지한다(라이브 프로시저가 182번 파일과 달라져 있었음).
--  2) 구매대상/판매대상/생산대상(p_po_yn/p_sale_yn/p_prod_yn) 조건은 P_ITEM 정의에서 뺀다 - 대응하는 컬럼(po_yn/sale_yn/prod_yn)이
--     150번에서 TBAITEM에서 삭제돼서 걸 수 있는 조건이 없다(넘겨도 아무 일도 안 하는 조건 칸만 남게 됨).

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_ITEM_Q]
    @p_keyword VARCHAR(100) = NULL,
    @p_asset_type VARCHAR(20) = NULL,
    @p_code VARCHAR(100) = NULL,
    @p_grp1_id VARCHAR(20) = NULL,
    @p_grp2_id VARCHAR(20) = NULL,
    @p_grp3_id VARCHAR(20) = NULL,
    @p_grp4_id VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 품번/품명 검색어: p_keyword가 비어 있으면 p_code를 쓴다(팝업관리 P_ITEM의 조건 이름이 p_code)
    DECLARE @kw VARCHAR(100) = COALESCE(NULLIF(@p_keyword, ''), NULLIF(@p_code, ''));

    SELECT a.item_id, a.item_no, a.item_nm, a.item_spec, a.asset_type,
           a.unit_cd, a.po_unit_cd,
           a.wh_id, w.wh_nm, a.loc_id, l.loc_nm
    FROM TBAITEM a
        LEFT JOIN TBAWH w ON w.wh_id = a.wh_id
        LEFT JOIN TBALOC l ON l.loc_id = a.loc_id
    WHERE 1 = 1
      AND (@kw IS NULL
           OR a.item_no LIKE '%' + @kw + '%'
           OR a.item_nm LIKE '%' + @kw + '%')
      AND (@p_asset_type IS NULL OR @p_asset_type = '' OR a.asset_type LIKE @p_asset_type + '%')
      AND (@p_grp1_id IS NULL OR @p_grp1_id = '' OR CAST(a.grp1_id AS VARCHAR(20)) = @p_grp1_id)
      AND (@p_grp2_id IS NULL OR @p_grp2_id = '' OR CAST(a.grp2_id AS VARCHAR(20)) = @p_grp2_id)
      AND (@p_grp3_id IS NULL OR @p_grp3_id = '' OR CAST(a.grp3_id AS VARCHAR(20)) = @p_grp3_id)
      AND (@p_grp4_id IS NULL OR @p_grp4_id = '' OR CAST(a.grp4_id AS VARCHAR(20)) = @p_grp4_id)
    ORDER BY a.item_id;
END
GO

DELETE FROM sysPopUpS WHERE popup_key = 'P_ITEM' AND param_nm IN ('p_po_yn', 'p_sale_yn', 'p_prod_yn');
GO
