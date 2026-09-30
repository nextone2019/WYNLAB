-- 라우팅관리(frmRoute)를 기초코드등록처럼 "좌측 목록 그리드 + 우측 입력/공정 그리드"로 바꾸면서 USP_PR_ROUTE_Q에 목록 조회(L)를 추가한다 (2026-09-29).
--   L: 라우팅 목록(좌측 그리드) - p_keyword가 라우팅코드/명에 포함되는 것, 비우면 전체. p_use_yn/p_acc_id는 선택 필터.
--   Q: 기존 그대로 - 한 건(헤더 + 공정행).
CREATE OR ALTER PROCEDURE USP_PR_ROUTE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_route_id BIGINT = NULL,
    @p_route_cd VARCHAR(20) = NULL,
    @p_keyword NVARCHAR(100) = NULL,
    @p_use_yn VARCHAR(1) = NULL,
    @p_acc_id BIGINT = NULL,
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
            SELECT m.route_id, m.acc_id, m.route_cd, m.route_nm, m.item_id, i.item_nm, m.use_yn,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRWOM w WHERE w.route_id = m.route_id) THEN 'Y' ELSE 'N' END AS used_yn
            FROM TPRROUTEM m LEFT JOIN TBAITEM i ON i.item_id = m.item_id
            WHERE (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_keyword IS NULL OR @p_keyword = N'' OR m.route_cd LIKE '%' + @p_keyword + '%' OR m.route_nm LIKE '%' + @p_keyword + '%')
              AND (@p_use_yn IS NULL OR @p_use_yn = '' OR m.use_yn = @p_use_yn)
            ORDER BY m.route_cd;
        END
        ELSE IF @p_work_type = 'Q'
        BEGIN
            DECLARE @match_id BIGINT;
            SELECT TOP 1 @match_id = route_id FROM TPRROUTEM
            WHERE (@p_route_id IS NULL OR route_id = @p_route_id)
              AND (@p_route_id IS NOT NULL OR @p_route_cd IS NULL OR route_cd LIKE '%' + @p_route_cd + '%')
            ORDER BY route_id DESC;

            SELECT m.route_id, m.acc_id, m.route_cd, m.route_nm, m.item_id, i.item_no, i.item_nm, m.use_yn, m.remark,
                   CASE WHEN EXISTS (SELECT 1 FROM TPRWOM w WHERE w.route_id = m.route_id) THEN 'Y' ELSE 'N' END AS used_yn
            FROM TPRROUTEM m LEFT JOIN TBAITEM i ON i.item_id = m.item_id
            WHERE m.route_id = @match_id;

            SELECT d.route_id, d.serl, d.acc_id, d.proc_cd, p.proc_nm, d.cust_id, c.cust_nm,
                   d.in_item_id, ii.item_no AS in_item_no, ii.item_nm AS in_item_nm, d.in_unit_cd,
                   d.out_item_id, oi.item_no AS out_item_no, oi.item_nm AS out_item_nm, d.out_unit_cd,
                   d.split_qty, d.price_unit_cd, d.price, d.remark
            FROM TPRROUTED d
                LEFT JOIN TBAPROC p ON p.acc_id = d.acc_id AND p.proc_cd = d.proc_cd
                LEFT JOIN TBACUST c ON c.cust_id = d.cust_id
                LEFT JOIN TBAITEM ii ON ii.item_id = d.in_item_id
                LEFT JOIN TBAITEM oi ON oi.item_id = d.out_item_id
            WHERE d.route_id = @match_id
            ORDER BY d.serl;
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
