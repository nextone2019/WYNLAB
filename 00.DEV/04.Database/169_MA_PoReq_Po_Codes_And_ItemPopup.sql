-- 구매요청/구매발주 화면에 필요한 공통코드(발주구분/원천구분)와 품목 팝업(P_ITEM) 신규 등록
-- (2026-09-22). 통화(cur_cd)/부가세유형(vat_type)/단위(unit_cd)는 이미 있는 CM0003/CM0004/
-- CM0001을 그대로 쓰므로 여기서 새로 안 만든다.

-- ============================================================
-- 1) MA0003(발주구분) - TMAPOREQM.po_type/TMAPOM.po_type 공용.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0003')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('MA0003', N'발주구분', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0003')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES
    ('MA0003', 'NORMAL', N'일반발주', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('MA0003', 'URGENT', N'긴급발주', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 2) MA0004(원천구분) - TMAPOREQD/TMAPOD.src_type 공용. "직접입력"(사용자가 그리드에 바로
--    입력) / "요청전환"(구매요청 품목을 구매발주로 그대로 가져옴, TMAPOD.src_type='REQ' +
--    src_id=req_id로 역참조) 두 가지만 우선 둔다.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0004')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt)
VALUES ('MA0004', N'원천구분', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0004')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES
    ('MA0004', 'DIRECT', N'직접입력', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
    ('MA0004', 'REQ', N'구매요청전환', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

-- ============================================================
-- 3) SSP_POP_ITEM_Q - P_ITEM은 사실 이미 등록돼 있었다(2026-09-17, admin, 다른 세션 작업으로
--    보임 - 최초 조사에서는 못 찾았었다). sysPopUpD가 기대하는 컬럼(asset_type/po_unit_cd/
--    wh_nm/loc_nm 등)이 지금 만들려던 5컬럼짜리 단순 버전보다 훨씬 많아서, 그 컬럼 목록에
--    맞춰 다시 만든다 - po_yn/sale_yn/prod_yn 3개는 sysPopUpD엔 아직 남아있지만 TBAITEM에서
--    이미 삭제된 컬럼(150번 마이그레이션)이라 여기서 같이 정리한다.
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_ITEM_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.item_id, a.item_no, a.item_nm, a.item_spec, a.asset_type,
           a.unit_cd, a.po_unit_cd,
           a.wh_id, w.wh_nm, a.loc_id, l.loc_nm
    FROM TBAITEM a
        LEFT JOIN TBAWH w ON w.wh_id = a.wh_id
        LEFT JOIN TBALOC l ON l.loc_id = a.loc_id
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR a.item_no LIKE '%' + @p_keyword + '%'
           OR a.item_nm LIKE '%' + @p_keyword + '%')
    ORDER BY a.item_id;
END
GO

-- sysPopUpD의 죽은 컬럼(TBAITEM에서 이미 삭제됨) 정리.
DELETE FROM sysPopUpD WHERE popup_key = 'P_ITEM' AND column_nm IN ('po_yn', 'sale_yn', 'prod_yn');
GO

-- ============================================================
-- 4) 팝업 메타데이터 등록 (sysPopUpM/D/S) - 이미 있으면 손대지 않는다(위에서 이미 확인).
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_ITEM')
INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field, display_field, popup_width, popup_height, use_yn, reg_user_id, reg_dt)
VALUES ('P_ITEM', 'SSP_POP_ITEM_Q', N'품목 조회', 'N', 'item_id', NULL, 'item_nm', 700, 500, 'Y', SUSER_SNAME(), GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_ITEM')
BEGIN
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, sort, width, visible_yn) VALUES
    ('P_ITEM', 'item_id',    N'품목ID', 'TEXT', 1, 100, 'N'),
    ('P_ITEM', 'item_no',    N'품번',   'TEXT', 2, 100, 'Y'),
    ('P_ITEM', 'item_nm',    N'품명',   'TEXT', 3, 100, 'Y'),
    ('P_ITEM', 'item_spec',  N'규격',   'TEXT', 4, 100, 'Y'),
    ('P_ITEM', 'asset_type', N'자산유형', 'TEXT', 5, 100, 'Y'),
    ('P_ITEM', 'unit_cd',    N'기본단위', 'TEXT', 6, 100, 'Y'),
    ('P_ITEM', 'po_unit_cd', N'구매단위', 'TEXT', 7, 100, 'Y'),
    ('P_ITEM', 'wh_id',      N'창고ID', 'TEXT', 8, 0,   'Y'),
    ('P_ITEM', 'wh_nm',      N'창고',   'TEXT', 9, 100, 'Y'),
    ('P_ITEM', 'loc_id',     N'LocationID', 'TEXT', 10, 0, 'Y'),
    ('P_ITEM', 'loc_nm',     N'Location', 'TEXT', 11, 100, 'Y');
END
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_ITEM')
INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width) VALUES ('P_ITEM', 'p_keyword', N'품번/품명', 'TEXT', 1, 180);
GO
