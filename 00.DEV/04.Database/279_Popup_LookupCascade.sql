-- 팝업 조회조건 LOOKUP 연쇄(상위 선택값으로 하위 콤보 좁히기) + 품목그룹 연쇄 적용 (2026-10-04, WYNLAB_DEV 전용)
--  1) sysPopUpS.par_fields(신규): 이 LOOKUP 조건이 "부모"로 삼는 조건들의 param_nm(쉼표 구분). 팝업 엔진이 부모 값이 바뀔 때마다
--     그 값들을 (부모의 param_nm 그대로) 이 콤보의 룩업 파라미터로 넘겨 목록을 다시 불러오고, 이 콤보의 선택값은 비운다.
--     비어 있으면 연쇄 없음(기존 팝업은 전부 그대로).
--     *** 이 컬럼은 API(PopupLookupRepository)가 읽으므로 새 API를 배포하기 전에 DB에 먼저 적용해야 한다. ***
--  2) 품목그룹 콤보 L_ITEM_GRP1~4를 프로시저 기반(source_type='P')으로 바꾼다 - 레벨은 프로시저에 고정하고, 상위 선택값을 받으면 그 아래만 보여준다.
--     레벨2는 그룹1, 레벨3은 그룹2(없으면 그룹1 아래 전체), 레벨4는 그룹3(없으면 그룹2, 그룹1 순)으로 좁힌다. 파라미터는 전부 선택(NULL이면 그 단계는 필터 없음).
--  3) P_ITEM / P_ITEM_PO의 품목그룹2~4 조건에 par_fields 지정.
-- 여러 번 실행해도 안전하다.

IF COL_LENGTH('sysPopUpS', 'par_fields') IS NULL
    ALTER TABLE sysPopUpS ADD par_fields VARCHAR(200) NULL;
GO

CREATE OR ALTER PROCEDURE SSP_CBO_ITEM_GRP1_Q
AS
BEGIN
    SET NOCOUNT ON;
    SELECT grp_id, grp_nm FROM TBAITEMGRP WHERE grp_lvl = '1' ORDER BY grp_nm;
END
GO

CREATE OR ALTER PROCEDURE SSP_CBO_ITEM_GRP2_Q
    @p_grp1_id BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT grp_id, grp_nm FROM TBAITEMGRP
    WHERE grp_lvl = '2' AND (@p_grp1_id IS NULL OR par_grp_id = @p_grp1_id)
    ORDER BY grp_nm;
END
GO

CREATE OR ALTER PROCEDURE SSP_CBO_ITEM_GRP3_Q
    @p_grp1_id BIGINT = NULL,
    @p_grp2_id BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT g.grp_id, g.grp_nm FROM TBAITEMGRP g
    WHERE g.grp_lvl = '3'
      AND (   (@p_grp2_id IS NOT NULL AND g.par_grp_id = @p_grp2_id)
           OR (@p_grp2_id IS NULL AND @p_grp1_id IS NULL)
           OR (@p_grp2_id IS NULL AND @p_grp1_id IS NOT NULL
               AND g.par_grp_id IN (SELECT p.grp_id FROM TBAITEMGRP p WHERE p.grp_lvl = '2' AND p.par_grp_id = @p_grp1_id)))
    ORDER BY g.grp_nm;
END
GO

CREATE OR ALTER PROCEDURE SSP_CBO_ITEM_GRP4_Q
    @p_grp1_id BIGINT = NULL,
    @p_grp2_id BIGINT = NULL,
    @p_grp3_id BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT g.grp_id, g.grp_nm FROM TBAITEMGRP g
    WHERE g.grp_lvl = '4'
      AND (   (@p_grp3_id IS NOT NULL AND g.par_grp_id = @p_grp3_id)
           OR (@p_grp3_id IS NULL AND @p_grp2_id IS NULL AND @p_grp1_id IS NULL)
           OR (@p_grp3_id IS NULL AND @p_grp2_id IS NOT NULL
               AND g.par_grp_id IN (SELECT p.grp_id FROM TBAITEMGRP p WHERE p.grp_lvl = '3' AND p.par_grp_id = @p_grp2_id))
           OR (@p_grp3_id IS NULL AND @p_grp2_id IS NULL AND @p_grp1_id IS NOT NULL
               AND g.par_grp_id IN (SELECT p3.grp_id FROM TBAITEMGRP p3 JOIN TBAITEMGRP p2 ON p2.grp_id = p3.par_grp_id AND p2.grp_lvl = '2'
                                    WHERE p3.grp_lvl = '3' AND p2.par_grp_id = @p_grp1_id)))
    ORDER BY g.grp_nm;
END
GO

-- 278에서 만든 쿼리형(Q) 콤보를 프로시저형(P)으로 전환
UPDATE sysLookupM SET source_type = 'P', proc_nm = 'SSP_CBO_ITEM_GRP1_Q', query_txt = NULL WHERE lookup_key = 'L_ITEM_GRP1';
UPDATE sysLookupM SET source_type = 'P', proc_nm = 'SSP_CBO_ITEM_GRP2_Q', query_txt = NULL WHERE lookup_key = 'L_ITEM_GRP2';
UPDATE sysLookupM SET source_type = 'P', proc_nm = 'SSP_CBO_ITEM_GRP3_Q', query_txt = NULL WHERE lookup_key = 'L_ITEM_GRP3';
UPDATE sysLookupM SET source_type = 'P', proc_nm = 'SSP_CBO_ITEM_GRP4_Q', query_txt = NULL WHERE lookup_key = 'L_ITEM_GRP4';
GO

-- 룩업관리 화면에 보이는 파라미터 정의(상위 그룹 선택값)
INSERT INTO sysLookupP (lookup_key, param_nm, caption, sort)
SELECT v.k, v.p, v.c, v.s
FROM (VALUES ('L_ITEM_GRP2', 'p_grp1_id', N'품목그룹1', 1),
             ('L_ITEM_GRP3', 'p_grp1_id', N'품목그룹1', 1), ('L_ITEM_GRP3', 'p_grp2_id', N'품목그룹2', 2),
             ('L_ITEM_GRP4', 'p_grp1_id', N'품목그룹1', 1), ('L_ITEM_GRP4', 'p_grp2_id', N'품목그룹2', 2), ('L_ITEM_GRP4', 'p_grp3_id', N'품목그룹3', 3)) v(k, p, c, s)
WHERE NOT EXISTS (SELECT 1 FROM sysLookupP x WHERE x.lookup_key = v.k AND x.param_nm = v.p);
GO

-- 팝업 조건 연결: 하위 그룹은 상위 그룹 조건 값을 부모로 삼는다
UPDATE sysPopUpS SET par_fields = 'p_grp1_id' WHERE popup_key IN ('P_ITEM', 'P_ITEM_PO') AND param_nm = 'p_grp2_id';
UPDATE sysPopUpS SET par_fields = 'p_grp1_id,p_grp2_id' WHERE popup_key IN ('P_ITEM', 'P_ITEM_PO') AND param_nm = 'p_grp3_id';
UPDATE sysPopUpS SET par_fields = 'p_grp1_id,p_grp2_id,p_grp3_id' WHERE popup_key IN ('P_ITEM', 'P_ITEM_PO') AND param_nm = 'p_grp4_id';
GO
