-- USP_PR_WO_Q 사업장(@p_acc_id) 조건 추가 (2026-10-03) - 254번과 같은 목적. 이 프로시저만 DEV/FADU 정의가 달라 DB별 파일로 둔다.
-- 적용 대상: WYNLAB_DEV 전용. Q(작업지시 단건 조회)와 L(목록) 두 분기 모두 메인 WHERE에 조건이 들어간다.
-- 생산관리(PR) 작업지시 프로시저 (2026-09-29). 226번(실적/이전)이 쓰는 작업지시(TPRWOM/TPRWOD)와 시작 LOT를 만든다.
--
--  작업지시 1건 = 웨이퍼 LOT 1개의 공정 체인. 저장(N)할 때
--    1) 라우팅(TPRROUTED)을 공정행(TPRWOD)으로 스냅샷 복사한다 - 이후 라우팅을 고쳐도 이미 낸 작업지시는 그대로.
--       공정행의 창고(wh_id)는 그 외주처의 외주창고(TBAWH.cust_id 연결, wh_type='OS')로 채운다.
--    2) 시작 LOT는 웨이퍼입고(TPRRCV, 234/235번)로 이미 입고된 미배정 웨이퍼 LOT를 골라 이 작업지시에 배정한다(TPRLOT.wo_id/wo_serl=0).
--       웨이퍼가 작업지시보다 먼저 도착하는 실제 순서를 따른다. 시작수량 = 그 LOT의 첫 공정 외주처 창고 재고 전량. 삭제하면 배정만 풀린다.
--  수정(U)은 일자/수주 연계/납기/담당/비고만. 시작 LOT/라우팅은 못 바꾼다(잘못 만들었으면 삭제 후 재작성).
--  삭제(D)는 실적/이전이 하나도 없을 때만.
--
--  USP_PR_WO_Q     Q: 헤더(0)+공정행(1)+LOT(2) / L: 목록(기간/상태/번호)
--  USP_PR_WO_S     헤더 N/U/D
--  USP_PR_WO_S_1   공정행 U(외주처/창고/단가/납기/분할수량/비고)
--  USP_PR_WO_C_S   상태: C 확정 / CC 확정취소 / X 중단 / XC 중단해제 / E 완료 / EC 완료취소  (계획 0 -> 확정 C -> 진행 1(첫 실적) -> 완료 E)

-- ============================================================
-- 1) USP_PR_WO_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_WO_Q
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),               /* Q 단건 / L 목록 */
    ---------------------------------------------------------------------------------------------------
    @p_wo_id BIGINT = NULL,
    @p_wo_no VARCHAR(20) = NULL,
    @p_fr_date VARCHAR(8) = NULL,           /* L 전용 */
    @p_to_date VARCHAR(8) = NULL,
    @p_stat_cd VARCHAR(10) = NULL,
    @p_lot_no NVARCHAR(50) = NULL,          /* 시작 LOT 검색 */
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = wo_id FROM TPRWOM
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_wo_id IS NULL OR wo_id = @p_wo_id)
              AND (@p_wo_id IS NOT NULL OR @p_wo_no IS NULL OR wo_no LIKE '%' + @p_wo_no + '%')
            ORDER BY wo_id DESC;

            SELECT
                m.wo_id, m.acc_id, m.wo_no, m.wo_date, m.route_id, r.route_cd, r.route_nm,
                m.item_id, i.item_no, i.item_nm, m.start_lot_no, m.start_qty,
                m.so_id, m.so_no, m.so_serl, m.delv_date, m.stat_cd,
                m.dept_id, d.dept_nm, m.emp_id, e.emp_nm, m.remark
            FROM TPRWOM m
                LEFT JOIN TPRROUTEM r ON r.route_id = m.route_id
                LEFT JOIN TBAITEM i ON i.item_id = m.item_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
            WHERE m.wo_id = @match_id;

            SELECT
                dt.wo_id, dt.serl, dt.acc_id, dt.wo_no, dt.proc_cd, p.proc_nm,
                dt.cust_id, c.cust_nm, dt.wh_id, w.wh_nm,
                dt.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, dt.in_unit_cd,
                dt.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, dt.out_unit_cd,
                dt.split_qty, dt.price_unit_cd, dt.price, dt.due_date, dt.stat_cd,
                dt.in_qty, dt.good_qty, dt.bad_qty,
                CASE WHEN dt.in_qty > 0 AND ISNULL(dt.in_unit_cd, '') = ISNULL(dt.out_unit_cd, '') THEN dt.good_qty * 100.0 / dt.in_qty
                     WHEN dt.good_qty + dt.bad_qty > 0 THEN dt.good_qty * 100.0 / (dt.good_qty + dt.bad_qty) ELSE NULL END AS yield_rate,
                dt.remark
            FROM TPRWOD dt
                LEFT JOIN TBAPROC p ON p.acc_id = dt.acc_id AND p.proc_cd = dt.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = dt.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
                LEFT JOIN TBAITEM ii ON ii.item_id = dt.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = dt.out_item_id
            WHERE dt.wo_id = @match_id
            ORDER BY dt.serl;

            -- 이 작업지시의 LOT와 현재 위치별 재고
            SELECT l.lot_id, l.lot_no, l.wo_serl, l.item_id, i.item_no, i.item_nm, l.unit_cd, l.init_qty,
                   s.wh_id, w.wh_nm, s.stock_qty
            FROM TPRLOT l
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.loc_id = 0 AND s.stock_qty <> 0
                LEFT JOIN TBAWH w ON w.wh_id = s.wh_id
            WHERE l.wo_id = @match_id
            ORDER BY l.wo_serl, l.lot_no;
        END
        ELSE IF @p_work_type = 'L'
        BEGIN
            SELECT
                m.wo_id, m.wo_no, m.wo_date, m.route_nm, m.item_nm, m.start_lot_no, m.start_qty, m.so_no, m.delv_date, m.stat_cd,
                -- 진행 공정: 실적이 있는 마지막 공정 순번/이름과 그 공정 양품 누계
                pr.serl AS cur_serl, pr.proc_nm AS cur_proc_nm, pr.good_qty AS cur_good_qty, pr.out_unit_cd AS cur_unit_cd
            FROM (
                SELECT wm.*, r.route_nm, i.item_nm
                FROM TPRWOM wm
                    LEFT JOIN TPRROUTEM r ON r.route_id = wm.route_id
                    LEFT JOIN TBAITEM i ON i.item_id = wm.item_id
            ) m
                OUTER APPLY (
                    SELECT TOP 1 d.serl, p.proc_nm, d.good_qty, d.out_unit_cd
                    FROM TPRWOD d LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                    WHERE d.wo_id = m.wo_id AND d.in_qty > 0 ORDER BY d.serl DESC
                ) pr
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_fr_date IS NULL OR m.wo_date >= @p_fr_date)
              AND (@p_to_date IS NULL OR m.wo_date <= @p_to_date)
              AND (@p_stat_cd IS NULL OR @p_stat_cd = '' OR m.stat_cd = @p_stat_cd)
              AND (@p_wo_no IS NULL OR m.wo_no LIKE '%' + @p_wo_no + '%')
              AND (@p_lot_no IS NULL OR m.start_lot_no LIKE '%' + @p_lot_no + '%')
            ORDER BY m.wo_id DESC;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
