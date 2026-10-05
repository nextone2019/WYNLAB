-- LOT계보조회 이력에 출하(영업 출하등록) 이벤트 추가 (2026-10-01) - 239의 프로시저에 UNION 한 블록 추가

CREATE OR ALTER PROCEDURE USP_PR_LOTTRACE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_lot_no NVARCHAR(50) = NULL,          /* L 전용: LOT번호 일부 */
    @p_wo_no VARCHAR(20) = NULL,            /* L 전용: 작업지시번호 일부 */
    @p_lot_id BIGINT = NULL,                /* H 전용 */
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
        IF @p_work_type = 'L'
        BEGIN
            IF ISNULL(@p_lot_no, '') = '' AND ISNULL(@p_wo_no, '') = ''
            BEGIN
                -- 조건이 없으면 전체 계보를 끌어오지 않고 빈 결과를 돌려준다(LOT번호나 작업지시번호를 넣어야 함).
                SELECT CAST(NULL AS BIGINT) AS lot_id WHERE 1 = 0;
                RETURN;
            END

            ;WITH hit AS (
                SELECT l.lot_id FROM TPRLOT l LEFT JOIN TPRWOM w ON w.wo_id = l.wo_id
                WHERE (ISNULL(@p_lot_no, '') = '' OR l.lot_no LIKE '%' + @p_lot_no + '%')
                  AND (ISNULL(@p_wo_no, '') = '' OR w.wo_no LIKE '%' + @p_wo_no + '%')
            ),
            up AS (
                SELECT lot_id FROM hit
                UNION ALL
                SELECT r.parent_lot_id FROM up JOIN TPRLOTREL r ON r.child_lot_id = up.lot_id
            ),
            roots AS (
                SELECT DISTINCT u.lot_id FROM up u
                WHERE NOT EXISTS (SELECT 1 FROM TPRLOTREL r WHERE r.child_lot_id = u.lot_id)
            ),
            tree AS (
                SELECT l.lot_id, 0 AS depth, CAST(NULL AS BIGINT) AS rslt_id,
                       CAST(RIGHT('0000000000' + CAST(l.lot_id AS VARCHAR(10)), 10) AS VARCHAR(900)) AS path
                FROM TPRLOT l JOIN roots ro ON ro.lot_id = l.lot_id
                UNION ALL
                SELECT c.lot_id, t.depth + 1, r.rslt_id,
                       CAST(t.path + '.' + RIGHT('0000000000' + CAST(c.lot_id AS VARCHAR(10)), 10) AS VARCHAR(900))
                FROM tree t
                    JOIN TPRLOTREL r ON r.parent_lot_id = t.lot_id
                    JOIN TPRLOT c ON c.lot_id = r.child_lot_id
            )
            SELECT
                t.lot_id, t.depth,
                REPLICATE(N'    ', t.depth) + CASE WHEN t.depth > 0 THEN N'└ ' ELSE N'' END + l.lot_no AS lot_disp,
                l.lot_no, i.item_no, i.item_nm, l.unit_cd, l.init_qty,
                w.wo_no,
                CASE WHEN l.wo_serl IS NULL OR l.wo_serl = 0 THEN N'입고(시작 LOT)' ELSE p.proc_nm END AS gen_proc_nm,
                rs.rslt_no,
                st.wh_nm, st.stock_qty,
                CASE WHEN EXISTS (SELECT 1 FROM hit h WHERE h.lot_id = t.lot_id) THEN 1 ELSE 0 END AS hit
            FROM tree t
                JOIN TPRLOT l ON l.lot_id = t.lot_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
                LEFT JOIN TPRWOM w ON w.wo_id = l.wo_id
                LEFT JOIN TPRWOD d ON d.wo_id = l.wo_id AND d.serl = l.wo_serl
                LEFT JOIN TBAPROC p ON p.acc_id = l.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TPRRSLTM rs ON rs.rslt_id = t.rslt_id
                OUTER APPLY (SELECT TOP 1 wh.wh_nm, s.stock_qty
                             FROM TMASTOCK s JOIN TBAWH wh ON wh.wh_id = s.wh_id
                             WHERE s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.stock_qty > 0
                             ORDER BY s.stock_qty DESC) st
            ORDER BY t.path
            OPTION (MAXRECURSION 200);
        END
        ELSE IF @p_work_type = 'H'
        BEGIN
            SELECT evt_kind, doc_type, doc_id, doc_no, evt_date, descr, qty, stat_nm FROM (
                -- 웨이퍼 입고(이 LOT 번호로 들어온 입고 문서)
                SELECT N'입고' AS evt_kind, 'RV' AS doc_type, v.rcv_id AS doc_id, v.rcv_no AS doc_no, v.rcv_date AS evt_date,
                       N'웨이퍼 입고 → ' + ISNULL(wh.wh_nm, N'') AS descr, v.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = v.stat_cd) AS stat_nm
                FROM TPRLOT l JOIN TPRRCV v ON v.acc_id = l.acc_id AND v.item_id = l.item_id AND v.lot_no = l.lot_no
                    LEFT JOIN TBAWH wh ON wh.wh_id = v.wh_id
                WHERE l.lot_id = @p_lot_id
                UNION ALL
                -- 공정 산출: 이 LOT를 만든 실적
                SELECT N'공정 산출', 'RS', m.rslt_id, m.rslt_no, m.rslt_date,
                       ISNULL(p.proc_nm, m.proc_cd) + N' 산출 (' + ISNULL(c.cust_nm, N'') + N', 투입 LOT ' + ISNULL(pl.lot_no, N'') + N')', r.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = m.stat_cd)
                FROM TPRLOTREL r JOIN TPRRSLTM m ON m.rslt_id = r.rslt_id
                    LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                    LEFT JOIN TPRLOT pl ON pl.lot_id = r.parent_lot_id
                WHERE r.child_lot_id = @p_lot_id
                UNION ALL
                -- 공정 투입: 이 LOT를 투입한 실적
                SELECT N'공정 투입', 'RS', m.rslt_id, m.rslt_no, m.rslt_date,
                       ISNULL(p.proc_nm, m.proc_cd) + N' 투입 (' + ISNULL(c.cust_nm, N'') + N', 양품 ' + CONVERT(NVARCHAR(30), CAST(m.good_qty AS DECIMAL(18,4)), 1) + N')', m.in_qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = m.stat_cd)
                FROM TPRRSLTM m
                    LEFT JOIN TBAPROC p ON p.acc_id = m.acc_id AND p.proc_cd = m.proc_cd
                    LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                WHERE m.in_lot_id = @p_lot_id
                UNION ALL
                -- 외주이전
                SELECT N'외주이전', 'XF', x.xfer_id, x.xfer_no, x.xfer_date,
                       ISNULL(fc.cust_nm, N'') + N' → ' + ISNULL(tc.cust_nm, N'') + N' (도착 ' + CONVERT(NVARCHAR(30), CAST(ISNULL(d.in_qty, 0) AS DECIMAL(18,4)), 1) + N')', d.out_qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0003' AND minor_cd = x.stat_cd)
                FROM TPRXFERD d JOIN TPRXFERM x ON x.xfer_id = d.xfer_id
                    LEFT JOIN TBACUST fc ON fc.cust_id = x.from_cust_id
                    LEFT JOIN TBACUST tc ON tc.cust_id = x.to_cust_id
                WHERE d.lot_id = @p_lot_id
                UNION ALL
                -- 출하(영업 출하등록 TSAGIM/D) - 이 LOT가 고객에게 나간 이력
                SELECT N'출하', 'GI', g.gi_id, g.gi_no, g.gi_date,
                       N'출하 → ' + ISNULL(c.cust_nm, N'') + N' (' + ISNULL(w.wh_nm, N'') + N', 수주 ' + ISNULL(g.so_no, N'') + N')', d.qty,
                       (SELECT minor_nm FROM TSMMINOR WHERE major_cd = 'PR0006' AND minor_cd = g.stat_cd)
                FROM TSAGID d JOIN TSAGIM g ON g.gi_id = d.gi_id
                    LEFT JOIN TBACUST c ON c.cust_id = g.cust_id
                    LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                WHERE d.lot_id = @p_lot_id
            ) e
            ORDER BY evt_date, doc_no;
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
