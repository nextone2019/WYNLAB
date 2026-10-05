-- 재고실사(frmStockCount) 전자결재 연동 - 결재 문서유형 STKCNT (2026-10-04, WYNLAB_DEV 전용). 설계: Document\재고실사_설계서.md (P2)
--  1) 결재 문서유형 AP0002 'STKCNT'(재고실사서) - rel_cd1/rel_cd2를 채워 홈 '기안서 작성' 타일에도 뜬다.
--  2) USP_AP_APPR_S_STKCNT - 결재 이벤트 후처리. 결재 단위는 실사 문서 1건(doc_id = TMACNTM.cnt_id).
--     SUBMIT      입력완료(3) 상태의 실사만 상신할 수 있다(차이가 고정된 뒤에 결재를 받는다). 아니면 방금 만들어진 결재 행을 지우고 오류로 올린다
--                 (USP_AP_APPR_S가 트랜잭션 없이 결재 행을 먼저 만들기 때문). 통과하면 결재번호를 문서에 연결.
--     APPROVE_END 후처리 없음 - 승인은 "조정해도 좋다는 허가"이고 확정은 [확정] 버튼으로 따로 한다(승인 후 처리는 화면별로 사용자가 정한다는 합의).
--                 확정 때 서버가 결재 승인완료(E)인지 확인한다(USP_MA_CNT_CONFIRM_CORE).
--     UNDO_END    승인 취소 - 이미 확정된 실사면 확정취소(조정 수불 역거래)를 같이 한다(기타입고/출고와 같은 원칙).
--     RESET       기안자의 상신 취소 - 확정된 실사면 확정취소 후 결재 연결을 해제한다.
--     REJECT      문서 상태를 바꾸지 않는다(결재가 반려로 표시되고 기안자가 고쳐서 다시 상신). 상신 후 잠금은 반려(R)면 풀린다(FN_AP_IS_LOCKED).
--  3) frmStockCount 메뉴에 '화면 기능' 전자결재(APPROVAL, STKCNT)를 켠다 - 화면 상단 기능 바에 전자결재 버튼이 나타난다(메뉴등록에서 끌 수 있다).
-- 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE USP_AP_APPR_S_STKCNT
    @p_event VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
    @p_doc_id BIGINT,
    @p_app_id BIGINT = NULL,
    @p_user_id VARCHAR(50) = NULL,
    @p_client_pc NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @stat VARCHAR(10) = (SELECT stat_cd FROM TMACNTM WHERE cnt_id = @p_doc_id);

    IF @p_event = 'SUBMIT'
    BEGIN
        IF @stat IS NULL OR @stat <> '3'
        BEGIN
            -- 이 호출 직전에 USP_AP_APPR_S가 만든 결재 문서/결재라인을 되돌린다(트랜잭션이 없어서 직접 지운다).
            DELETE FROM TAPDOCPATH WHERE app_id = @p_app_id;
            DELETE FROM TAPDOC WHERE app_id = @p_app_id;
            THROW 50001, N'입력완료된 재고실사만 결재 상신할 수 있습니다. (모든 라인 입력 후 [입력완료]를 먼저 하세요)', 1;
        END

        UPDATE TMACNTM SET app_id = @p_app_id, app_no = (SELECT app_no FROM TAPDOC WHERE app_id = @p_app_id) WHERE cnt_id = @p_doc_id;
    END
    ELSE IF @p_event = 'UNDO_END'
    BEGIN
        IF @stat = 'C' EXEC USP_MA_CNT_CANCEL_CORE @p_doc_id, @p_user_id, @p_client_pc;
    END
    ELSE IF @p_event = 'RESET'
    BEGIN
        IF @stat = 'C' EXEC USP_MA_CNT_CANCEL_CORE @p_doc_id, @p_user_id, @p_client_pc;
        UPDATE TMACNTM SET app_id = NULL, app_no = NULL WHERE cnt_id = @p_doc_id;
    END
    -- 'APPROVE_END'(승인)/'REJECT'(반려): 문서 상태를 바꾸지 않는다.
END
GO

IF NOT EXISTS (SELECT 1 FROM TSMMINOR WHERE major_cd = 'AP0002' AND minor_cd = 'STKCNT')
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, rel_cd1, rel_cd2, reg_user_id, reg_dt)
VALUES ('AP0002', 'STKCNT', N'재고실사서', (SELECT ISNULL(MAX(sort), 0) + 1 FROM TSMMINOR WHERE major_cd = 'AP0002'), 'N', 'Y', 'MA.frmStockCount', N'자재', 'SYSTEM', GETDATE());
GO

-- frmStockCount 메뉴의 화면 기능: 전자결재 사용(STKCNT)
DECLARE @menu BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE SCREEN_CLASS_NM = 'frmStockCount');
IF @menu IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TSMMENUFEATURE WHERE menu_id = @menu AND feature_cd = 'APPROVAL')
    INSERT INTO TSMMENUFEATURE (menu_id, feature_cd, use_yn, option_val, reg_user_id, reg_dt)
    VALUES (@menu, 'APPROVAL', 'Y', 'STKCNT', 'SYSTEM', GETDATE());
GO
