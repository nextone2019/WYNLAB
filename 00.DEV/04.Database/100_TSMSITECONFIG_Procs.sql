/* ---------- TSMSITECONFIG 프로시저: 사이트 환경설정 화면(frmSiteConfig) ----------
   USP_SM_SITECONFIG_Q(Q) - 단일 행 조회(항상 config_id=1).
   USP_SM_SITECONFIG_S(U) - 이미지 3종을 제외한 모든 스칼라 값 갱신. 이미지는 컬럼명을
   파라미터화할 수 없어서(그리고 셋 다 단순 단일 컬럼 갱신이라) 프로시저 없이 Repository에서
   직접 파라미터화된 SQL 텍스트로 갱신한다(GenericDataRepository.QueryRawAsync와 같은 방식). */

CREATE OR ALTER PROCEDURE USP_SM_SITECONFIG_Q
    @p_work_type varchar(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF @p_work_type = 'Q'
    BEGIN
        SELECT
            company_nm AS CompanyNm,
            smtp_host AS SmtpHost,
            smtp_port AS SmtpPort,
            smtp_username AS SmtpUsername,
            smtp_from_address AS SmtpFromAddress,
            smtp_from_display_nm AS SmtpFromDisplayNm,
            file_block_extensions AS FileBlockExtensions,
            file_max_size_mb AS FileMaxSizeMb,
            pwd_expire_days AS PwdExpireDays,
            pwd_lock_threshold AS PwdLockThreshold,
            pwd_reset_code_valid_min AS PwdResetCodeValidMin,
            pwd_min_length AS PwdMinLength,
            CASE WHEN pwd_require_upper_lower = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PwdRequireUpperLower,
            CASE WHEN pwd_require_digit = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PwdRequireDigit,
            CASE WHEN pwd_require_special = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PwdRequireSpecial,
            init_pwd_policy AS InitPwdPolicy,
            CASE WHEN force_change_on_first_login = 'Y' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS ForceChangeOnFirstLogin,
            required_field_back_color AS RequiredFieldBackColor,
            grid_header_back_color AS GridHeaderBackColor,
            grid_focused_row_back_color AS GridFocusedRowBackColor,
            brand_color AS BrandColor,
            tree_group_back_color AS TreeGroupBackColor,
            divider_color AS DividerColor
        FROM TSMSITECONFIG
        WHERE config_id = 1;
    END
END
GO

CREATE OR ALTER PROCEDURE USP_SM_SITECONFIG_S
    @p_work_type varchar(10),
    @p_company_nm nvarchar(100) = NULL,
    @p_smtp_host varchar(200) = NULL,
    @p_smtp_port int = NULL,
    @p_smtp_username varchar(200) = NULL,
    @p_smtp_from_address varchar(200) = NULL,
    @p_smtp_from_display_nm nvarchar(100) = NULL,
    @p_file_block_extensions nvarchar(500) = NULL,
    @p_file_max_size_mb int = NULL,
    @p_pwd_expire_days int = NULL,
    @p_pwd_lock_threshold int = NULL,
    @p_pwd_reset_code_valid_min int = NULL,
    @p_pwd_min_length int = NULL,
    @p_pwd_require_upper_lower varchar(1) = NULL,
    @p_pwd_require_digit varchar(1) = NULL,
    @p_pwd_require_special varchar(1) = NULL,
    @p_init_pwd_policy varchar(20) = NULL,
    @p_force_change_on_first_login varchar(1) = NULL,
    @p_required_field_back_color varchar(10) = NULL,
    @p_grid_header_back_color varchar(10) = NULL,
    @p_grid_focused_row_back_color varchar(10) = NULL,
    @p_brand_color varchar(10) = NULL,
    @p_tree_group_back_color varchar(10) = NULL,
    @p_divider_color varchar(10) = NULL,
    @p_user_id varchar(30),
    @p_client_pc nvarchar(200) = NULL,
    @ReturnCode int OUTPUT,
    @ReturnMsg varchar(200) OUTPUT,
    @ErrorCode int OUTPUT,
    @ErrorMsg varchar(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = NULL;
    SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMSITECONFIG
            SET company_nm = @p_company_nm,
                smtp_host = @p_smtp_host,
                smtp_port = @p_smtp_port,
                smtp_username = @p_smtp_username,
                smtp_from_address = @p_smtp_from_address,
                smtp_from_display_nm = @p_smtp_from_display_nm,
                file_block_extensions = @p_file_block_extensions,
                file_max_size_mb = @p_file_max_size_mb,
                pwd_expire_days = @p_pwd_expire_days,
                pwd_lock_threshold = @p_pwd_lock_threshold,
                pwd_reset_code_valid_min = @p_pwd_reset_code_valid_min,
                pwd_min_length = @p_pwd_min_length,
                pwd_require_upper_lower = @p_pwd_require_upper_lower,
                pwd_require_digit = @p_pwd_require_digit,
                pwd_require_special = @p_pwd_require_special,
                init_pwd_policy = @p_init_pwd_policy,
                force_change_on_first_login = @p_force_change_on_first_login,
                required_field_back_color = @p_required_field_back_color,
                grid_header_back_color = @p_grid_header_back_color,
                grid_focused_row_back_color = @p_grid_focused_row_back_color,
                brand_color = @p_brand_color,
                tree_group_back_color = @p_tree_group_back_color,
                divider_color = @p_divider_color,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE config_id = 1;
        END
    END TRY
    BEGIN CATCH
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
