-- 영업관리 - 수주현황(frmSoList) 조회 프로시저. USP_MA_POLIST_Q(구매발주현황)와 완전히 같은 구조,
-- TSASOM/TSASOD 대상. TMAPOM에 있는 po_type에 대응하는 필드가 TSASOM엔 없어서 그 컬럼만 뺐다.
-- appr_stat_cd는 TAPDOC.app_stat_cd에서 온다(app_stat_cd가 맞는 컬럼명 - stat_cd 아님,
-- [[project_wynlab_sales_module_design]] 확인사항 참고).

CREATE OR ALTER PROCEDURE USP_SA_SOLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_so_id BIGINT = NULL,
    @p_so_no VARCHAR(20) = NULL,
    @p_so_title NVARCHAR(1000) = NULL,
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
                m.so_id, m.so_no, m.so_date, m.so_title,
                m.stat_cd,
                m.cust_id, c.cust_nm,
                m.dept_id, d.dept_nm,
                m.emp_id, e.emp_nm,
                m.app_id, m.app_no, t.app_stat_cd AS appr_stat_cd
            FROM TSASOM m
                LEFT JOIN TBACUST c ON c.cust_id = m.cust_id
                LEFT JOIN TBADEPT d ON d.dept_id = m.dept_id
                LEFT JOIN TBAEMP e ON e.emp_id = m.emp_id
                LEFT JOIN TAPDOC t ON t.app_id = m.app_id
            WHERE 1 = 1
              AND (@p_so_no IS NULL OR m.so_no LIKE '%' + @p_so_no + '%')
              AND (@p_so_title IS NULL OR m.so_title LIKE '%' + @p_so_title + '%')
            ORDER BY m.so_id DESC;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT
                dt.so_id, dt.serl,
                dt.item_id, i.item_no, i.item_nm, i.item_spec,
                dt.qty, dt.next_qty, (dt.qty - ISNULL(dt.next_qty, 0)) AS remain_qty, dt.unit_cd,
                dt.price, dt.total_amt,
                dt.delv_date, dt.wh_id, w.wh_nm
            FROM TSASOD dt
                LEFT JOIN TBAITEM i ON i.item_id = dt.item_id
                LEFT JOIN TBAWH w ON w.wh_id = dt.wh_id
            WHERE dt.so_id = @p_so_id
            ORDER BY dt.serl;
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

-- 메뉴 골격의 수주현황(MENU_ID=21) 자리를 채운다.
UPDATE TSMMENU
SET MODULE = 'SA', SCREEN_CLASS_NM = 'frmSoList', PROC_PREFIX = 'USP_SA_',
    upt_user_id = SUSER_SNAME(), upt_dt = GETDATE()
WHERE MENU_ID = 21 AND MENU_NM = N'수주현황';
GO
