-- 품목 팝업(P_ITEM) 재고관리 조건 추가 + 조회조건 배치 정리 (2026-10-05, WYNLAB_DEV 전용)
--  1) 공통 콤보 L_YN(예/아니오) - 팝업 조회조건에서 Y/N 플래그(재고관리/구매/판매/생산 대상 등)를 거는 용도. 비우면 전체.
--  2) SSP_POP_ITEM_Q에 @p_stock_yn 추가(TBAITEM.stock_yn 일치, 비면 전체). 기존 호출은 그대로 동작.
--     *** 팝업 엔진은 sysPopUpS의 조건을 전부 프로시저 파라미터로 보내므로 3)보다 먼저(또는 같이) 적용해야 한다. ***
--  3) sysPopUpS: P_ITEM에 p_stock_yn(재고관리) 조건을 3번째 줄로 추가하고 줄/폭을 정리한다.
--     1줄 품번/품명 160 + 자산구분 110, 2줄 품목그룹1~4 각 110, 3줄 재고관리 80. 화면은 PopupConditions "p_stock_yn=Y" 로 기본값을 건다.
--  4) 팝업 창 크기 700x500 -> 920x560(조회조건이 줄 단위로 열이 맞춰져 가로가 늘고 줄이 하나 늘었다).
-- 구매/판매/생산 대상 조건은 TBAITEM에 대응 컬럼(po_yn/sale_yn/prod_yn)이 150번에서 삭제돼 있어 아직 없다(품목 마스터 플래그가 먼저 필요).
-- 여러 번 실행해도 안전하다.

IF NOT EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = 'L_YN')
    INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
    VALUES ('L_YN', NULL, N'예/아니오', 'yn_cd', 'yn_nm', 'Y', N'Y/N 플래그 조회조건용(비우면 전체)', 'admin', GETDATE(), 'Q',
            N'SELECT ''Y'' AS yn_cd, N''예'' AS yn_nm' + CHAR(13) + CHAR(10) + N'UNION ALL' + CHAR(13) + CHAR(10) + N'SELECT ''N'', N''아니오''');
GO

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_ITEM_Q]
    @p_keyword VARCHAR(100) = NULL,
    @p_asset_type VARCHAR(20) = NULL,
    @p_code VARCHAR(100) = NULL,
    @p_grp1_id VARCHAR(20) = NULL,
    @p_grp2_id VARCHAR(20) = NULL,
    @p_grp3_id VARCHAR(20) = NULL,
    @p_grp4_id VARCHAR(20) = NULL,
    @p_stock_yn VARCHAR(1) = NULL
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
      AND (@p_stock_yn IS NULL OR @p_stock_yn = '' OR ISNULL(a.stock_yn, 'N') = @p_stock_yn)
    ORDER BY a.item_id;
END
GO

UPDATE sysPopUpS SET caption = N'품번/품명', width = 160, row_no = 1, sort = 1 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_code';
UPDATE sysPopUpS SET width = 110, row_no = 1, sort = 2 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_asset_type';
UPDATE sysPopUpS SET width = 110, row_no = 2, sort = 3 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp1_id';
UPDATE sysPopUpS SET width = 110, row_no = 2, sort = 4 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp2_id';
UPDATE sysPopUpS SET width = 110, row_no = 2, sort = 5 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp3_id';
UPDATE sysPopUpS SET width = 110, row_no = 2, sort = 6 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp4_id';

IF NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_ITEM' AND param_nm = 'p_stock_yn')
    INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no)
    VALUES ('P_ITEM', 'p_stock_yn', N'재고관리', 'LOOKUP', 7, 80, 'L_YN', 3);
GO

UPDATE sysPopUpM SET popup_width = 920, popup_height = 560 WHERE popup_key = 'P_ITEM';
GO
