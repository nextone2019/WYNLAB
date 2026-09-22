CREATE OR ALTER PROCEDURE USP_BA_EMPLIST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
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
                emp_no, emp_nm, emp_nm_eng, ent_date, grp_ent_date, job_grade, job_type, ret_yn, ret_date, sex_cd, tel, hp_tel, email, nat_cd, zip_code, addr1, addr2, photo, photo_file_nm, photo_path, holi_yn, dilig_yn, pay_yn, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc, DEPT_ID, EMP_ID, acc_id
            FROM TBAEMP
            ORDER BY EMP_ID;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
