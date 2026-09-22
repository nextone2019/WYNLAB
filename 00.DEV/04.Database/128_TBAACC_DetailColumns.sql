-- TBAACC에 사업장 상세정보 컬럼(사업자번호/통화/대표자/주소/연락처/로고/스탬프 등)을 DB에 직접
-- 추가하고 frmAcc.Designer.cs의 panData에도 이미 컨트롤을 올려둔 상태(사장님 작업) - 이 파일은
-- 그 뒤에 남은 USP_BA_ACC_Q/S 쪽 마무리다. 113번 마이그레이션 이후 acc_id/acc_nm 2개뿐이던
-- 프로시저를 실제 라이브 스키마(32개 컬럼, INFORMATION_SCHEMA로 직접 확인)에 맞춘다.
--
-- panData에 컨트롤이 없는 5개 컬럼(biz_kind/biz_type/open_date/vat_type/vat_rate)은 이번에
-- 손대지 않는다 - 화면에 입력할 곳이 없는데 프로시저에 파라미터만 만들어두면 항상 NULL로만
-- 저장되는 죽은 파라미터가 되므로, 화면에 컨트롤이 생길 때 같이 추가하는 게 맞다.
--
-- logo/stamp는 frmEMP.picEmpPhoto(USP_BA_EMP_S의 p_photo)와 완전히 같은 패턴 - PictureEditWyn.
-- ImageBytes를 Base64 문자열로 보내면 XML value() 트릭으로 VARBINARY(MAX)로 바꿔 image 컬럼에
-- 넣는다. logo_file_nm/logo_path(stamp도 동일)는 화면에 그 값을 채울 컨트롤이 없어서 이번엔
-- 같이 안 채운다(이미지 자체는 DB에 바로 저장되므로 나중에 파일명/경로가 필요해지면 그때
-- USP_SM_FILE_S 같은 별도 업로드 경로로 확장하면 된다).

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
                   homepage, email, fax, logo, stamp
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
        -- 문자열/NULL로 받은 Base64를 컬럼에 넣을 VARBINARY(MAX)로 바꾼다(USP_BA_EMP_S의 p_photo와
        -- 같은 방식). U에서는 이미지를 안 바꾼 저장(다른 필드만 수정)도 흔하므로, 비어있으면 새로
        -- NULL을 덮어쓰지 않고 기존 값을 그대로 유지한다.
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
                homepage, email, fax, logo, stamp,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_nm, @p_biz_no, @p_tel, @p_cur_cd, @p_owner_nm, @p_owner_nm_eng,
                @p_zip_code, @p_addr1, @p_addr2, @p_addr1_eng, @p_addr2_eng,
                @p_homepage, @p_email, @p_fax, @logo_bin, @stamp_bin,
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
                homepage = @p_homepage, email = @p_email, fax = @p_fax,
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
