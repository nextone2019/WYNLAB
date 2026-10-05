-- 품목 팝업(P_ITEM)/발주용 품목 팝업(P_ITEM_PO) 조회조건 정리 (2026-10-04, WYNLAB_DEV 전용)
--  - 품목그룹1~4: TEXT(그룹ID를 직접 입력해야 했음) -> LOOKUP. 품목그룹 콤보 L_ITEM_GRP는 레벨(p_grp_lvl)이 필수 파라미터인데 팝업 엔진은 LOOKUP 조건에
--    파라미터를 못 넘겨서(popPopUp: new LookUpEditWyn { LookupKey }) 그대로 쓰면 빈 콤보가 된다(P_ITEM_PO가 그랬다) -> 레벨 고정 전용 콤보
--    L_ITEM_GRP1~4(해당 레벨의 그룹 전체, 이름순)를 새로 만들고 팝업은 그걸 쓴다. 값은 grp_id(SSP_POP_ITEM_Q가 TBAITEM.grpN_id와 비교하는 값 그대로).
--  - 자산구분: TEXT -> LOOKUP(L_CM0002, 자산구분 공통코드). 값은 minor_cd(프로시저의 접두 일치 LIKE와 호환).
--  - 품목코드/명은 그대로 TEXT, 입력칸 폭 통일(품목코드/명 140, 나머지 120), 순서/줄: 1줄 품목코드/명 + 자산구분, 2줄 품목그룹1~4.
-- 상위 그룹을 고르면 하위 콤보가 좁혀지는 연쇄는 팝업 엔진이 LOOKUP 파라미터를 지원하지 않아 아직 없다(각 콤보는 해당 레벨의 전체 그룹).
-- 팝업 정의는 DB 데이터라 클라이언트 배포 없이 팝업을 다시 열면 바로 적용된다. 여러 번 실행해도 안전하다.

DECLARE @n INT = 1;
WHILE @n <= 4
BEGIN
    DECLARE @key VARCHAR(30) = 'L_ITEM_GRP' + CAST(@n AS VARCHAR(1));
    IF NOT EXISTS (SELECT 1 FROM sysLookupM WHERE lookup_key = @key)
    INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
    VALUES (@key, NULL, N'품목그룹' + CAST(@n AS NVARCHAR(1)) + N'(레벨 고정)', 'grp_id', 'grp_nm', 'Y', N'팝업 조회조건용 - 해당 레벨 그룹 전체', 'admin', GETDATE(), 'Q',
            N'SELECT   grp_id, ' + CHAR(13) + CHAR(10) + N'            grp_nm ' + CHAR(13) + CHAR(10)
            + N'FROM TBAITEMGRP' + CHAR(13) + CHAR(10) + N'WHERE grp_lvl = ''' + CAST(@n AS NVARCHAR(1)) + N'''' + CHAR(13) + CHAR(10)
            + N'Order by grp_nm');
    SET @n += 1;
END
GO

-- P_ITEM: 조회조건 재구성
UPDATE sysPopUpS SET control_type = 'LOOKUP', lookup_key = 'L_CM0002', width = 120, row_no = 1, sort = 2 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_asset_type';
UPDATE sysPopUpS SET width = 140, row_no = 1, sort = 1 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_code';
UPDATE sysPopUpS SET control_type = 'LOOKUP', lookup_key = 'L_ITEM_GRP1', width = 120, row_no = 2, sort = 3 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp1_id';
UPDATE sysPopUpS SET control_type = 'LOOKUP', lookup_key = 'L_ITEM_GRP2', width = 120, row_no = 2, sort = 4 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp2_id';
UPDATE sysPopUpS SET control_type = 'LOOKUP', lookup_key = 'L_ITEM_GRP3', width = 120, row_no = 2, sort = 5 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp3_id';
UPDATE sysPopUpS SET control_type = 'LOOKUP', lookup_key = 'L_ITEM_GRP4', width = 120, row_no = 2, sort = 6 WHERE popup_key = 'P_ITEM' AND param_nm = 'p_grp4_id';

-- P_ITEM_PO: 품목그룹 콤보가 레벨 파라미터 없이 빈 목록이던 문제도 같은 전용 콤보로 바로잡는다
UPDATE sysPopUpS SET lookup_key = 'L_ITEM_GRP1' WHERE popup_key = 'P_ITEM_PO' AND param_nm = 'p_grp1_id';
UPDATE sysPopUpS SET lookup_key = 'L_ITEM_GRP2' WHERE popup_key = 'P_ITEM_PO' AND param_nm = 'p_grp2_id';
UPDATE sysPopUpS SET lookup_key = 'L_ITEM_GRP3' WHERE popup_key = 'P_ITEM_PO' AND param_nm = 'p_grp3_id';
UPDATE sysPopUpS SET lookup_key = 'L_ITEM_GRP4' WHERE popup_key = 'P_ITEM_PO' AND param_nm = 'p_grp4_id';
GO
