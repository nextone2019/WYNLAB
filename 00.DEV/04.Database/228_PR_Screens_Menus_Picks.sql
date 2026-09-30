-- 생산관리(PR) 화면 지원 (2026-09-29): 불러오기 팝업 프로시저 2개, 콤보(라우팅/이전구분), 메뉴 등록.
--  USP_PR_RSLTREADYPICK_Q  frmRslt "실적 대기 LOT 불러오기" - 진행 중인 작업지시의 공정별로, 그 공정 외주처 창고에 투입 LOT 재고가 있는 행
--  USP_PR_XFERREADYPICK_Q  frmXfer "이전 대상 LOT 불러오기" - 다음 공정이 있는 공정의 외주처 창고에 산출 LOT 재고가 있는 행(이전 후보)
--  둘 다 popPick 공통 파라미터 규약(p_work_type='Q', p_acc_id, p_date_from/to, p_doc_no, p_cust_id, p_keyword)을 따른다.
--  메뉴: 생산관리(14) > 생산실행(신규 그룹) 밑에 4개 화면. 프로시저 접두사 USP_PR_. 기본 권한은 부여하지 않는다(메뉴권한 화면에서 부여).

-- ============================================================
-- 1) 불러오기 프로시저
-- ============================================================
CREATE OR ALTER PROCEDURE USP_PR_RSLTREADYPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 작업지시번호 */
    @p_cust_id BIGINT = NULL,               /* 외주처 */
    @p_keyword NVARCHAR(100) = NULL,        /* LOT/품번/품명 */
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
            SELECT
                m.wo_id, m.wo_no, m.wo_date, d.serl AS wo_serl, d.proc_cd, p.proc_nm,
                d.cust_id, c.cust_nm, d.wh_id, w.wh_nm,
                l.lot_id AS in_lot_id, l.lot_no AS in_lot_no,
                d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd, s.stock_qty AS in_qty,
                d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd, d.split_qty
            FROM TPRWOM m
                JOIN TPRWOD d ON d.wo_id = m.wo_id
                JOIN TPRLOT l ON l.wo_id = d.wo_id AND l.item_id = d.in_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = d.wh_id AND s.loc_id = 0 AND s.stock_qty > 0
                LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = d.cust_id
                LEFT JOIN TBAWH w ON w.wh_id = d.wh_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE m.stat_cd IN ('C', '1')
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.wo_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.wo_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.wo_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR d.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR l.lot_no LIKE '%' + @p_keyword + '%'
                   OR ii.item_no LIKE '%' + @p_keyword + '%'
                   OR ii.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.wo_no, d.serl, l.lot_no;
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

CREATE OR ALTER PROCEDURE USP_PR_XFERREADYPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 작업지시번호 */
    @p_cust_id BIGINT = NULL,               /* 출발 외주처 */
    @p_keyword NVARCHAR(100) = NULL,        /* LOT/품번/품명 */
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
            -- 도착 공정 = 출발 공정 다음 순번. 출발 공정 외주처 창고에 그 공정 산출품목 LOT 재고가 있는 행.
            SELECT
                m.wo_id, m.wo_no, m.wo_date,
                f.serl AS from_serl, fp.proc_nm AS from_proc_nm, f.cust_id AS from_cust_id, fc.cust_nm AS from_cust_nm, f.wh_id AS from_wh_id,
                t.serl AS to_serl, tp.proc_nm AS to_proc_nm, t.cust_id AS to_cust_id, tc.cust_nm AS to_cust_nm, t.wh_id AS to_wh_id,
                l.lot_id, l.lot_no, l.item_id, i.item_no, i.item_nm, l.unit_cd, s.stock_qty
            FROM TPRWOM m
                JOIN TPRWOD f ON f.wo_id = m.wo_id
                JOIN TPRWOD t ON t.wo_id = f.wo_id AND t.serl = (SELECT MIN(x.serl) FROM TPRWOD x WHERE x.wo_id = f.wo_id AND x.serl > f.serl)
                JOIN TPRLOT l ON l.wo_id = f.wo_id AND l.item_id = f.out_item_id
                JOIN TMASTOCK s ON s.acc_id = l.acc_id AND s.item_id = l.item_id AND s.lot_no = l.lot_no AND s.wh_id = f.wh_id AND s.loc_id = 0 AND s.stock_qty > 0
                LEFT JOIN TBAPROC fp ON fp.acc_id = f.acc_id AND fp.proc_cd = f.proc_cd
                LEFT JOIN TBAPROC tp ON tp.acc_id = t.acc_id AND tp.proc_cd = t.proc_cd
                LEFT JOIN TBACUST fc ON fc.cust_id = f.cust_id
                LEFT JOIN TBACUST tc ON tc.cust_id = t.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = l.item_id
            WHERE m.stat_cd IN ('C', '1')
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.wo_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.wo_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.wo_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR f.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR l.lot_no LIKE '%' + @p_keyword + '%'
                   OR i.item_no LIKE '%' + @p_keyword + '%'
                   OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.wo_no, f.serl, l.lot_no;
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

-- ============================================================
-- 2) 공통코드 PR0008(이전구분) + 콤보 L_PR0008 / L_PRROUTE(라우팅)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM TSMMAJOR WHERE major_cd = 'PR0008')
INSERT INTO TSMMAJOR (major_cd, major_nm, sys_yn, reg_user_id, reg_dt) VALUES ('PR0008', N'외주이전구분', 'Y', 'SYSTEM', GETDATE());
GO
INSERT INTO TSMMINOR (major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn, reg_user_id, reg_dt)
SELECT 'PR0008', v.cd, v.nm, v.sort, 'Y', 'Y', 'SYSTEM', GETDATE()
FROM (VALUES ('N', N'정방향', 1), ('R', N'역이전(반품/재작업)', 2)) v(cd, nm, sort)
WHERE NOT EXISTS (SELECT 1 FROM TSMMINOR n WHERE n.major_cd = 'PR0008' AND n.minor_cd = v.cd);
GO

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_PR0008', NULL, N'외주이전구분', 'minor_cd', 'minor_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   minor_cd, ' + CHAR(13) + CHAR(10) + N'            minor_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TSMMINOR' + CHAR(13) + CHAR(10) + N'WHERE major_cd= ''PR0008''' + CHAR(13) + CHAR(10)
       + N'AND     use_yn = ''Y''' + CHAR(13) + CHAR(10) + N'Order by sort, minor_nm'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_PR0008');

INSERT INTO sysLookupM (lookup_key, proc_nm, lookup_nm, value_field, display_field, use_yn, remark, reg_user_id, reg_dt, source_type, query_txt)
SELECT 'L_PRROUTE', NULL, N'라우팅', 'route_id', 'route_nm', 'Y', NULL, 'admin', GETDATE(), 'Q',
       N'SELECT   route_id, ' + CHAR(13) + CHAR(10) + N'            route_nm ' + CHAR(13) + CHAR(10)
       + N'FROM TPRROUTEM' + CHAR(13) + CHAR(10) + N'WHERE use_yn = ''Y''' + CHAR(13) + CHAR(10)
       + N'Order by route_cd'
WHERE NOT EXISTS (SELECT 1 FROM sysLookupM x WHERE x.lookup_key = 'L_PRROUTE');
GO

-- ============================================================
-- 3) 메뉴: 생산관리(14) > 생산실행 > 작업지시/공정실적/외주이전/작업지시현황
-- ============================================================
DECLARE @grp BIGINT = (SELECT TOP 1 MENU_ID FROM TSMMENU WHERE UPPER_MENU_ID = 14 AND MENU_LEVEL = 2 AND MENU_TYPE = 'GROUP' AND MENU_NM = N'생산실행');
IF @grp IS NULL
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'생산실행', 14, 2, 'GROUP', 20, 'Y', SUSER_SNAME(), GETDATE());
    SET @grp = SCOPE_IDENTITY();
END

DECLARE @screens TABLE (nm NVARCHAR(50), cls VARCHAR(50), sort INT);
INSERT INTO @screens VALUES
    (N'작업지시',       'frmWo',       10),
    (N'공정실적',       'frmRslt',     20),
    (N'외주이전',       'frmXfer',     30),
    (N'작업지시현황',   'frmWoStatus', 40);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT s.nm, @grp, 3, 'FORM', 'PR', s.cls, 'USP_PR_', s.sort, 'Y', SUSER_SNAME(), GETDATE()
FROM @screens s
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.MODULE = 'PR' AND m.SCREEN_CLASS_NM = s.cls);
GO
