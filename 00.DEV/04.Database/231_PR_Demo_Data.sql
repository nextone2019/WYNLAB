-- 생산관리(PR) 시연용 테스트 데이터 (2026-09-29): 시연 고객 + 시연 수주 1건 + Amkor 창고의 웨이퍼 재고(W2609-02). W2609-01은 웨이퍼입고 화면 시연용으로 재고 없음.
-- 224번 seed(품목/외주처/창고/라우팅) 위에 얹는다. 이미 있으면 건너뛴다(고객명, 수주 제목 '[시연]...', LOT 번호로 판단) - 다시 실행해도 무해하다.
--  * 웨이퍼 재고는 구매입고 문서 흐름(발주->납품->검사->입고)을 태우지 않고 기타입고(ETC_IN) 수불로 바로 넣는다. 시연에서 "웨이퍼가 Amkor에 도착해 있다"는
--    상태만 필요하기 때문이다. 실제 운영에서는 구매입고 화면으로 같은 창고/LOT 번호로 입고한다.
--  * 작업지시/공정실적/이전은 일부러 만들지 않는다 - 시연에서 화면으로 직접 만들어 보이게.
--  시연 시나리오: 작업지시(라우팅 DEMO-SWP, 시작 LOT W2609-01, 25장, 수주 연결) -> Bumping 실적 -> Amkor->ITEK 이전 -> EDS 실적(엑셀) -> ITEK->Amkor 이전
--  -> Packaging 실적(5,000개씩 분할) -> Amkor->ITEK 이전 -> Final Test 실적.

-- 시연 고객
INSERT INTO TBACUST (cust_nm, cur_cd, stat_cd, remark, reg_user_id, reg_dt)
SELECT N'시연고객(DEMO)', 'KRW', '0', N'생산 시연용', 'SYSTEM', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM TBACUST WHERE cust_nm = N'시연고객(DEMO)');
GO

-- 시연 수주: 완제품(PKG-FG) 20,000 EA
DECLARE @rc INT, @msg NVARCHAR(200), @gc VARCHAR(20), @ec INT, @em NVARCHAR(500);
DECLARE @cust BIGINT = (SELECT cust_id FROM TBACUST WHERE cust_nm = N'시연고객(DEMO)');
DECLARE @item BIGINT = (SELECT item_id FROM TBAITEM WHERE item_no = 'PKG-FG' AND acc_id = 1);
DECLARE @today VARCHAR(8) = CONVERT(VARCHAR(8), GETDATE(), 112);
DECLARE @delv VARCHAR(8) = CONVERT(VARCHAR(8), DATEADD(DAY, 30, GETDATE()), 112);

IF @cust IS NOT NULL AND @item IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSASOM WHERE so_title LIKE N'[[]시연]%')
BEGIN
    EXEC USP_SA_SO_S 'N', @p_acc_id = 1, @p_so_date = @today, @p_so_title = N'[시연] 반도체 완제품 주문', @p_cust_id = @cust,
        @p_cur_cd = 'KRW', @p_exc_rate = 1, @p_delv_date = @delv, @p_user_id = 'SYSTEM',
        @GeneratedCode = @gc OUTPUT, @ReturnCode = @rc OUTPUT, @ReturnMsg = @msg OUTPUT, @ErrorCode = @ec OUTPUT, @ErrorMsg = @em OUTPUT;
    IF @rc <> 0 THROW 50001, N'시연 수주 헤더 생성 실패', 1;

    DECLARE @so BIGINT = TRY_CAST(@gc AS BIGINT);
    EXEC USP_SA_SO_S_1 'N', @p_so_id = @so, @p_serl = 1, @p_item_id = @item, @p_unit_cd = 'EA', @p_qty = 20000, @p_next_qty = 0,
        @p_delv_date = @delv, @p_stock_yn = 'Y', @p_remark = N'시연용', @p_user_id = 'SYSTEM',
        @GeneratedCode = @gc OUTPUT, @ReturnCode = @rc OUTPUT, @ReturnMsg = @msg OUTPUT, @ErrorCode = @ec OUTPUT, @ErrorMsg = @em OUTPUT;
    IF @rc <> 0 THROW 50001, N'시연 수주 품목 생성 실패', 1;
END
GO

-- Amkor 창고 웨이퍼 재고: W2609-02(25장)만 미리 넣는다 - 빠른 시연/응용 시나리오용(기타입고 수불). W2609-01은 일부러 재고 없이 두어 웨이퍼입고(frmRcv) 화면으로 입고하는 것부터 시연한다.
DECLARE @acc BIGINT = 1;
DECLARE @wafer BIGINT = (SELECT item_id FROM TBAITEM WHERE item_no = 'WF-RAW' AND acc_id = @acc);
DECLARE @wh BIGINT = (SELECT wh_id FROM TBAWH WHERE wh_nm = N'Amkor 창고' AND acc_id = @acc);
DECLARE @today VARCHAR(8) = CONVERT(VARCHAR(8), GETDATE(), 112);
DECLARE @tid BIGINT;

IF @wafer IS NOT NULL AND @wh IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM TMASTOCK WHERE item_id = @wafer AND wh_id = @wh AND lot_no = N'W2609-02')
        EXEC USP_PR_TRANS_POST @acc, 'I', 'ETC_IN', @wafer, @wh, N'W2609-02', 25, 'DEMO', 0, 'DEMO', 2, NULL, N'시연용 웨이퍼 입고(TSMC)', 'SYSTEM', NULL, @today, @tid OUTPUT;

    -- 작업지시가 "입고 LOT 불러오기"로 고를 수 있게 미배정 LOT 등록(웨이퍼입고 확정이 만드는 것과 같은 행)
    IF NOT EXISTS (SELECT 1 FROM TPRLOT WHERE acc_id = @acc AND item_id = @wafer AND lot_no = N'W2609-02')
        INSERT INTO TPRLOT (acc_id, lot_no, item_id, unit_cd, init_qty, wo_id, wo_serl, remark, reg_user_id, reg_dt)
        VALUES (@acc, N'W2609-02', @wafer, 'SHT', 25, NULL, NULL, N'시연용 웨이퍼 LOT', 'SYSTEM', GETDATE());
END
GO
