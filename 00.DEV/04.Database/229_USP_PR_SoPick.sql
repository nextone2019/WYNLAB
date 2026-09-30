-- 작업지시(frmWo) "수주 선택" 팝업 프로시저 (2026-09-29). popPick 공통 파라미터 규약을 따른다.
-- 마감(stop_yn='Y')되지 않은 수주 라인. 수주 확정(결재) 여부는 따지지 않는다 - 시연/초기 운영에서는 수주 등록 직후 생산을 걸 수 있게 둔다
-- (확정된 수주만 대상으로 바꾸려면 아래 WHERE에 m.cfm_yn = 'Y' 추가).
CREATE OR ALTER PROCEDURE USP_PR_SOPICK_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_date_from VARCHAR(8) = NULL,
    @p_date_to VARCHAR(8) = NULL,
    @p_doc_no VARCHAR(20) = NULL,           /* 수주번호 */
    @p_cust_id BIGINT = NULL,
    @p_keyword NVARCHAR(100) = NULL,        /* 품번/품명 */
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
                m.so_id, d.serl AS so_serl, m.so_no, m.so_date, m.cust_id, c.cust_nm,
                d.item_id, i.item_no, i.item_nm, d.unit_cd, d.qty, ISNULL(d.delv_date, m.delv_date) AS delv_date
            FROM TSASOM m
                JOIN TSASOD d ON d.so_id = m.so_id
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBAITEM i ON i.item_id = d.item_id
            WHERE ISNULL(m.stop_yn, 'N') <> 'Y' AND ISNULL(d.stop_yn, 'N') <> 'Y'
              AND (@p_acc_id IS NULL OR m.acc_id = @p_acc_id)
              AND (@p_date_from IS NULL OR @p_date_from = '' OR m.so_date >= @p_date_from)
              AND (@p_date_to IS NULL OR @p_date_to = '' OR m.so_date <= @p_date_to)
              AND (@p_doc_no IS NULL OR @p_doc_no = '' OR m.so_no LIKE '%' + @p_doc_no + '%')
              AND (@p_cust_id IS NULL OR m.cust_id = @p_cust_id)
              AND (@p_keyword IS NULL OR @p_keyword = ''
                   OR i.item_no LIKE '%' + @p_keyword + '%'
                   OR i.item_nm LIKE '%' + @p_keyword + '%')
            ORDER BY m.so_date DESC, m.so_no, d.serl;
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
