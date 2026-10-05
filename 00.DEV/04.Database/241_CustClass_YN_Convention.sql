-- is_member을 0/1(그러다 bool "True"/"False"로 새는 문제까지 겪은) 대신 이 코드베이스
-- 전역 관례인 Y/N으로 바꾼다(use_yn/end_yn/rtn_yn 등과 동일 - RepositoryItemCheckEdit도
-- ValueChecked="Y"/ValueUnchecked="N"이 표준, frmItem/frmSchedule/frmMinorCode 등 다수 화면
-- 선례). 문자열 컬럼이 되므로 ProcData.Str이 그대로 "Y"/"N"을 보내 이전의 bool 변환 문제 자체가
-- 없어진다.

CREATE OR ALTER PROCEDURE USP_BA_CUST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
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
                a.CUST_ID, a.cust_nm, a.biz_no, a.tel, a.cur_cd, a.owner_nm, a.zip_code, a.addr1, a.addr2,
                a.homepage, a.email, a.fax, a.biz_kind, a.biz_type, a.trans_open_date, a.vat_type, a.vat_rate,
                a.remark, a.stat_cd, a.EMP_ID, b.emp_nm
            FROM TBACUST a
            LEFT JOIN TBAEMP b ON a.EMP_ID = b.EMP_ID
            WHERE  1 =1
                --AND (@p_cust_id IS NULL OR a.CUST_ID = @p_cust_id)
              AND (@p_cust_nm IS NULL OR a.cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY a.CUST_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT CUST_ID, serl, prsn_nm, grade, tel1, tel2, fax, email
            FROM TBACUSTPRSN
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;

            SELECT CUST_ID, serl, bank_cd, acnt_no, remark
            FROM TBACUSTACNT
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;

            SELECT mn.minor_cd AS class_cd, mn.minor_nm AS class_nm,
                   CASE WHEN c.CUST_ID IS NULL THEN 'N' ELSE 'Y' END AS is_member
            FROM TSMMINOR mn
            LEFT JOIN TBACUSTCLASS c ON c.class_cd = mn.minor_cd AND c.CUST_ID = @p_cust_id
            WHERE mn.major_cd = 'BA0003' AND mn.use_yn = 'Y'
            ORDER BY mn.sort, mn.minor_cd;
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

CREATE OR ALTER PROCEDURE USP_BA_CUST_S_3
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_class_cd VARCHAR(20) = NULL,
    @p_is_member VARCHAR(1) = NULL,
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
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
        IF @p_work_type = 'U'
        BEGIN
            IF @p_is_member = 'Y'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM TBACUSTCLASS WHERE CUST_ID = @p_cust_id AND class_cd = @p_class_cd)
                    INSERT INTO TBACUSTCLASS (CUST_ID, class_cd, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                    VALUES (@p_cust_id, @p_class_cd, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            END
            ELSE
                DELETE FROM TBACUSTCLASS WHERE CUST_ID = @p_cust_id AND class_cd = @p_class_cd;
        END

        SET @GeneratedCode = @p_class_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
