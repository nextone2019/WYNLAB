-- 생산관리(PR) 시연용 seed (2026-09-29). 반도체 후공정 외주 시연: 품목 5종 / 외주처 3곳 / 외주처 창고 + IN-TRANSIT / 라우팅 1건.
-- 모든 INSERT는 이미 있으면 건너뛴다(item_no, cust_nm, wh_nm, route_cd 기준) - 다시 실행해도 무해하다.
-- 단가(price)는 실제 값을 모르므로 비워 둔다(작업지시 화면에서 입력).
--
-- 품목(공정 산출물마다 다른 품목, 전부 LOT 관리/재고 관리):
--   WF-RAW    웨이퍼(원재료, 장)          -> Bumping    -> WF-BUMP  범핑 웨이퍼(장)
--   WF-BUMP                               -> EDS        -> DIE-GOOD Good Die(개)   (EDS 합/부 판정 후 Good만)
--   DIE-GOOD                              -> Packaging  -> PKG-RAW  패키지(개)     (5,000개 단위 LOT 분할)
--   PKG-RAW                               -> Final Test -> PKG-FG   완제품(개)
-- 외주처: TSMC(웨이퍼 공급), Amkor(Bumping/Packaging), ITEK(EDS/Final Test)
-- 창고: Amkor 창고 / ITEK 창고(외주처 재고, TBAWH.cust_id로 외주처 연결, wh_type='OS') + 이동중 재고(wh_type='TR')

-- 단위: 장(웨이퍼)
IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'CM0001' AND minor_cd = 'SHT')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('CM0001', 'SHT', N'장', 0, 'N', 'Y', 'SYSTEM', GETDATE());
GO

-- 외주처/공급처
INSERT INTO TBACUST (cust_nm, cur_cd, stat_cd, remark, reg_user_id, reg_dt)
SELECT v.nm, 'KRW', '0', N'생산 시연용', 'SYSTEM', GETDATE()
FROM (VALUES (N'TSMC'), (N'Amkor'), (N'ITEK')) v(nm)
WHERE NOT EXISTS (SELECT 1 FROM TBACUST c WHERE c.cust_nm = v.nm);
GO

-- 품목 (asset_type: M 원재료 / P 반제품 / G 제품)
INSERT INTO TBAITEM (item_no, item_nm, item_spec, unit_cd, asset_type, stock_yn, lot_yn, po_qc_yn, prod_qc_yn, stat_cd,
                     grp1_id, grp2_id, grp3_id, grp4_id, remark, reg_user_id, reg_dt, acc_id)
SELECT v.no, v.nm, v.spec, v.unit, v.asset, 'Y', 'Y', 'N', 'N', '0', '0', '0', 0, 0, N'생산 시연용', 'SYSTEM', GETDATE(), 1
FROM (VALUES
    ('WF-RAW',   N'웨이퍼',        N'25장 = 1 LOT',      'SHT', 'M'),
    ('WF-BUMP',  N'범핑 웨이퍼',   N'Bumping 완료',      'SHT', 'P'),
    ('DIE-GOOD', N'Good Die',      N'EDS 합격 Die',      'EA',  'P'),
    ('PKG-RAW',  N'패키지',        N'Final Test 대기',   'EA',  'P'),
    ('PKG-FG',   N'완제품 패키지', N'Final Test 합격',   'EA',  'G')
) v(no, nm, spec, unit, asset)
WHERE NOT EXISTS (SELECT 1 FROM TBAITEM i WHERE i.item_no = v.no AND i.acc_id = 1);
GO

-- 창고
INSERT INTO TBAWH (wh_nm, acc_id, wh_type, cust_id, reg_user_id, reg_dt)
SELECT v.nm, 1, v.typ, c.cust_id, 'SYSTEM', GETDATE()
FROM (VALUES (N'Amkor 창고', 'OS', N'Amkor'), (N'ITEK 창고', 'OS', N'ITEK'), (N'이동중 재고', 'TR', NULL)) v(nm, typ, cnm)
LEFT JOIN TBACUST c ON c.cust_nm = v.cnm
WHERE NOT EXISTS (SELECT 1 FROM TBAWH w WHERE w.wh_nm = v.nm AND w.acc_id = 1);
GO

-- 라우팅: 반도체 후공정 (시연)
IF NOT EXISTS (SELECT 1 FROM TPRROUTEM WHERE acc_id = 1 AND route_cd = 'DEMO-SWP')
BEGIN
    DECLARE @route BIGINT;
    INSERT INTO TPRROUTEM (acc_id, route_cd, route_nm, item_id, use_yn, remark, reg_user_id, reg_dt)
    SELECT 1, 'DEMO-SWP', N'반도체 후공정 (시연)', (SELECT item_id FROM TBAITEM WHERE item_no = 'PKG-FG' AND acc_id = 1), 'Y',
           N'Bumping -> EDS -> Packaging -> Final Test', 'SYSTEM', GETDATE();
    SET @route = SCOPE_IDENTITY();

    INSERT INTO TPRROUTED (route_id, serl, acc_id, proc_cd, cust_id, in_item_id, out_item_id, in_unit_cd, out_unit_cd,
                           split_qty, price_unit_cd, reg_user_id, reg_dt)
    SELECT @route, v.serl, 1, v.pcd,
           (SELECT cust_id FROM TBACUST WHERE cust_nm = v.cnm),
           (SELECT item_id FROM TBAITEM WHERE item_no = v.in_no AND acc_id = 1),
           (SELECT item_id FROM TBAITEM WHERE item_no = v.out_no AND acc_id = 1),
           v.in_unit, v.out_unit, v.split, v.price_unit, 'SYSTEM', GETDATE()
    FROM (VALUES
        (1, 'BUMP', N'Amkor', 'WF-RAW',   'WF-BUMP',  'SHT', 'SHT', NULL,   'SHT'),
        (2, 'EDS',  N'ITEK',  'WF-BUMP',  'DIE-GOOD', 'SHT', 'EA',  NULL,   'SHT'),
        (3, 'PKG',  N'Amkor', 'DIE-GOOD', 'PKG-RAW',  'EA',  'EA',  5000,   'EA'),
        (4, 'FT',   N'ITEK',  'PKG-RAW',  'PKG-FG',   'EA',  'EA',  NULL,   'EA')
    ) v(serl, pcd, cnm, in_no, out_no, in_unit, out_unit, split, price_unit);
END
GO
