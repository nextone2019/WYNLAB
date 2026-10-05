-- 재고실사 수불 통제 방식 A(완전 동결) (2026-10-04, WYNLAB_DEV 전용). 설계: Document\재고실사_설계서.md (P3)
--  - 시스템설정 MA.CNT_FREEZE: B 변동 보정(기본, 동결 안 함 - 실사 중에도 입출고 가능하고 차이는 변동으로 보정) / A 완전 동결.
--  - 대상확정(SNAP) 때 그 시점의 설정값을 문서(TMACNTM.freeze_mode)에 고정한다. 이후 설정을 바꿔도 이미 진행 중인 실사는 영향이 없다.
--  - 동결(A) 범위: 대상확정(1)~입력완료(3)인 A방식 실사가 있는 창고에는 재고에 반영되는 모든 수불(TMATRANS 입력)을 거부한다.
--    확정(C)/취소(X)되면 풀린다. 이 실사 자신의 조정 수불(src_type='STKCNT')은 예외(확정/확정취소가 해야 하므로).
--  - 통제 지점은 TMATRANS 입력 트리거 하나다 - 구매입고/기타입출고/출하/생산/이동 등 수불을 만드는 모든 경로가 TMATRANS에 쓰므로
--    프로시저마다 검사를 심지 않고도 빠짐없이 막고, 앞으로 생길 경로도 자동으로 막힌다(설계서 초안의 FN 호출 방식 대신).
--    오류는 THROW 50001 문구로 올라가 확정/저장 화면의 오류 안내에 그대로 보인다.
-- 여러 번 실행해도 안전하다.

-- 1) 공통코드 MA0017(실사 수불 통제 방식) + 시스템설정
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'MA0017')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('MA0017', N'재고실사 수불 통제 방식', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'MA0017')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
VALUES ('MA0017', 'B', N'변동 보정(동결 안 함)', 1, 'Y', 'Y', 'SYSTEM', GETDATE()),
       ('MA0017', 'A', N'완전 동결(실사 중 해당 창고 수불 차단)', 2, 'Y', 'Y', 'SYSTEM', GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM TSMPROCCONFIG WHERE config_key = 'MA.CNT_FREEZE')
INSERT INTO TSMPROCCONFIG (
    config_key, module_cd, group_nm, config_nm, data_type, enum_major_cd,
    min_value, max_value, default_value, description, sort, use_yn,
    reg_user_id, reg_dt, upt_user_id, upt_dt
)
VALUES (
    'MA.CNT_FREEZE', 'MA', N'재고실사', N'실사 중 수불 통제 방식', 'ENUM', 'MA0017',
    NULL, NULL, 'B',
    N'B(변동 보정): 실사 중에도 입출고를 막지 않고, 대상확정 이후 확정된 수불을 차이 계산에서 보정합니다. A(완전 동결): 대상확정~입력완료 동안 그 창고의 입출고/이동/생산 등 모든 수불을 막습니다(실사 확정/취소 후 해제). 변경 이후 대상확정하는 실사부터 적용되고, 이미 진행 중인 실사는 시작할 때의 방식을 유지합니다.',
    10, 'Y', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE()
);
GO

-- 2) 동결 검사용 인덱스(트리거가 수불 입력마다 (사업장, 창고)로 진행 중 실사를 찾는다)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('TMACNTM') AND name = 'IX_TMACNTM_wh')
    CREATE NONCLUSTERED INDEX IX_TMACNTM_wh ON TMACNTM (acc_id, wh_id, stat_cd) INCLUDE (freeze_mode, cnt_no);
GO

-- 3) 대상확정 - freeze_mode를 시스템설정으로 고정 (274의 USP_MA_CNT_SNAP_CORE에서 freeze_mode만 바뀜)
CREATE OR ALTER PROCEDURE USP_MA_CNT_SNAP_CORE
    @p_cnt_id BIGINT,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    @o_skipped INT = 0 OUTPUT,                  /* 다른 진행 중 실사에 이미 있어 제외한 재고 행 수 */
    @o_skip_docs NVARCHAR(100) = NULL OUTPUT    /* 그 실사번호들(최대 3개) */
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @stat VARCHAR(10), @acc BIGINT, @no VARCHAR(20), @wh BIGINT;
    SELECT @stat = stat_cd, @acc = acc_id, @no = cnt_no, @wh = wh_id FROM TMACNTM WHERE cnt_id = @p_cnt_id;

    IF @stat IS NULL THROW 50001, N'재고실사를 찾을 수 없습니다.', 1;
    IF @stat <> '0' THROW 50001, N'작성 상태의 재고실사만 대상확정할 수 있습니다.', 1;
    IF @wh IS NULL THROW 50001, N'실사할 창고를 먼저 선택해 저장하세요.', 1;

    BEGIN TRAN;

    DECLARE @snap BIGINT = ISNULL((SELECT MAX(trans_id) FROM TMATRANS), 0);
    -- 수불 통제 방식(시스템설정 MA.CNT_FREEZE): A 완전 동결 / B 변동 보정(기본). 이 실사가 대상확정되는 시점의 값을 문서에 고정한다.
    DECLARE @freeze VARCHAR(1) = CASE WHEN dbo.FSM_PROCCONFIG('MA.CNT_FREEZE') = 'A' THEN 'A' ELSE 'B' END;

    -- 다른 진행 중 실사에 이미 있는 재고 행(제외 대상)
    DECLARE @skip TABLE (item_id BIGINT, lot_no NVARCHAR(50), loc_id BIGINT, cnt_no VARCHAR(20));
    INSERT INTO @skip (item_id, lot_no, loc_id, cnt_no)
    SELECT s.item_id, s.lot_no, s.loc_id, xm.cnt_no
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id AND i.stock_yn = 'Y'
        JOIN TMACNTD x ON x.item_id = s.item_id AND x.wh_id = s.wh_id AND x.loc_id = s.loc_id AND x.lot_no = s.lot_no
        JOIN TMACNTM xm ON xm.cnt_id = x.cnt_id AND xm.cnt_id <> @p_cnt_id AND xm.stat_cd IN ('1', '2', '3')
    WHERE s.acc_id = @acc AND s.wh_id = @wh;

    SELECT @o_skipped = COUNT(*) FROM (SELECT DISTINCT item_id, lot_no, loc_id FROM @skip) k;
    SELECT @o_skip_docs = STRING_AGG(d.cnt_no, ', ') FROM (SELECT DISTINCT TOP 3 cnt_no FROM @skip ORDER BY cnt_no) d;

    INSERT INTO TMACNTD (cnt_id, serl, acc_id, cnt_no, item_id, unit_cd, wh_id, loc_id, lot_no, book_qty, recnt_yn, add_yn, stock_yn,
                         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    SELECT @p_cnt_id, ROW_NUMBER() OVER (ORDER BY i.item_no, s.lot_no), @acc, @no, s.item_id, ISNULL(s.unit_cd, i.unit_cd), s.wh_id, s.loc_id, s.lot_no,
           s.stock_qty - dbo.FN_MA_CNT_MOVE(s.acc_id, s.item_id, s.wh_id, s.loc_id, s.lot_no, @snap, '99991231', @p_cnt_id),
           'N', 'N', 'Y', @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
    FROM TMASTOCK s
        JOIN TBAITEM i ON i.item_id = s.item_id
    WHERE s.acc_id = @acc AND s.wh_id = @wh AND i.stock_yn = 'Y'
      AND NOT EXISTS (SELECT 1 FROM @skip k WHERE k.item_id = s.item_id AND k.lot_no = s.lot_no AND k.loc_id = s.loc_id);

    -- 대상 재고(라인)가 0건이어도 대상확정은 통과한다 - 빈 창고/새 품목은 실사중 '라인추가'로 넣는다(입력완료는 라인이 1건 이상 있어야 한다).
    UPDATE TMACNTM SET stat_cd = '1', freeze_mode = @freeze, snap_trans_id = @snap, snap_dt = GETDATE(),
                       upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
    WHERE cnt_id = @p_cnt_id;

    COMMIT TRAN;
END
GO
GO

-- 4) 수불 입력 트리거 - 동결(A) 중인 창고의 수불을 거부한다(이 실사 자신의 조정 수불 STKCNT은 예외)
CREATE OR ALTER TRIGGER TR_TMATRANS_CNT_FREEZE ON TMATRANS
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @cnt_no VARCHAR(20), @msg NVARCHAR(400);
    SELECT TOP 1 @cnt_no = m.cnt_no
    FROM inserted i
        JOIN TMACNTM m ON m.acc_id = i.acc_id AND m.wh_id = i.wh_id AND m.stat_cd IN ('1', '2', '3') AND m.freeze_mode = 'A'
    WHERE ISNULL(i.src_type, '') <> 'STKCNT';

    IF @cnt_no IS NOT NULL
    BEGIN
        SET @msg = N'재고실사(' + @cnt_no + N') 진행 중이라 이 창고의 입출고/이동을 처리할 수 없습니다. 실사를 확정하거나 취소한 뒤 처리하세요.';
        THROW 50001, @msg, 1;
    END
END
GO