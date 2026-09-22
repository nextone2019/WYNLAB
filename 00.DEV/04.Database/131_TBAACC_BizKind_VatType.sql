-- frmAcc.Designer.cs의 panData에 남은 4개 컬럼(biz_kind/biz_type/vat_type/vat_rate) 컨트롤을
-- 추가했다(사장님 작업, 128번 마이그레이션 코멘트에서 "컨트롤이 생기면 같이 추가"라고 남겨둔
-- 부분) - 이 파일이 그 DB 배관 마무리다. TBAACC 컬럼 자체는 이미 있었다(32개 컬럼에 포함).

CREATE OR ALTER PROCEDURE USP_BA_ACC_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_acc_nm NVARCHAR(100) = NULL,
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
            SELECT acc_id, acc_nm, biz_no, tel, cur_cd, owner_nm, owner_nm_eng,
                   zip_code, addr1, addr2, addr1_eng, addr2_eng,
                   homepage, email, fax, open_date, biz_kind, biz_type, vat_type, vat_rate,
                   logo, stamp
            FROM TBAACC
            WHERE (@p_acc_id IS NULL OR acc_id = @p_acc_id)
              AND (@p_acc_nm IS NULL OR acc_nm LIKE '%' + @p_acc_nm + '%')
            ORDER BY acc_id;
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

CREATE OR ALTER PROCEDURE USP_BA_ACC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_acc_nm NVARCHAR(100) = NULL,
    @p_biz_no VARCHAR(30) = NULL,
    @p_tel VARCHAR(30) = NULL,
    @p_cur_cd VARCHAR(3) = NULL,
    @p_owner_nm NVARCHAR(100) = NULL,
    @p_owner_nm_eng NVARCHAR(100) = NULL,
    @p_zip_code VARCHAR(20) = NULL,
    @p_addr1 NVARCHAR(1000) = NULL,
    @p_addr2 NVARCHAR(1000) = NULL,
    @p_addr1_eng NVARCHAR(1000) = NULL,
    @p_addr2_eng NVARCHAR(1000) = NULL,
    @p_homepage NVARCHAR(200) = NULL,
    @p_email NVARCHAR(100) = NULL,
    @p_fax VARCHAR(30) = NULL,
    @p_open_date VARCHAR(8) = NULL,
    @p_biz_kind NVARCHAR(200) = NULL,
    @p_biz_type NVARCHAR(200) = NULL,
    @p_vat_type VARCHAR(10) = NULL,
    @p_vat_rate NUMERIC(19, 2) = NULL,
    @p_logo NVARCHAR(MAX) = NULL,      -- Base64 인코딩된 이미지 - NULL/빈 문자열이면 기존 값을 그대로 둔다
    @p_stamp NVARCHAR(MAX) = NULL,     -- 위와 동일
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
        DECLARE @logo_bin VARBINARY(MAX) = NULL;
        DECLARE @stamp_bin VARBINARY(MAX) = NULL;
        IF @p_logo IS NOT NULL AND LEN(@p_logo) > 0
            SET @logo_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_logo"))', 'VARBINARY(MAX)');
        IF @p_stamp IS NOT NULL AND LEN(@p_stamp) > 0
            SET @stamp_bin = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@p_stamp"))', 'VARBINARY(MAX)');

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAACC (
                acc_nm, biz_no, tel, cur_cd, owner_nm, owner_nm_eng,
                zip_code, addr1, addr2, addr1_eng, addr2_eng,
                homepage, email, fax, open_date, biz_kind, biz_type, vat_type, vat_rate,
                logo, stamp,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_owner_nm_eng,
                @p_zip_code, @p_addr1, @p_addr2, @p_addr1_eng, @p_addr2_eng,
                @p_homepage, @p_email, @p_fax, @p_open_date, @p_biz_kind, @p_biz_type, @p_vat_type, @p_vat_rate,
                @logo_bin, @stamp_bin,
                @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_acc_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAACC SET
                acc_nm = @p_acc_nm, biz_no = @p_biz_no, tel = @p_tel, cur_cd = @p_cur_cd,
                owner_nm = @p_owner_nm, owner_nm_eng = @p_owner_nm_eng,
                zip_code = @p_zip_code, addr1 = @p_addr1, addr2 = @p_addr2,
                addr1_eng = @p_addr1_eng, addr2_eng = @p_addr2_eng,
                homepage = @p_homepage, email = @p_email, fax = @p_fax, open_date = @p_open_date,
                biz_kind = @p_biz_kind, biz_type = @p_biz_type, vat_type = @p_vat_type, vat_rate = @p_vat_rate,
                logo = ISNULL(@logo_bin, logo), stamp = ISNULL(@stamp_bin, stamp),
                upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_id = @p_acc_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAACC WHERE acc_id = @p_acc_id;
        END

        SET @GeneratedCode = CAST(@p_acc_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
