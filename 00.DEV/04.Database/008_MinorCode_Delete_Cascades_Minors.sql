-- USP_SM_MINORCODE_S의 'D'(삭제) 분기가 TSMMAJOR만 지우고 TSMMINOR는 그대로 둬서, 소분류가
-- 있는 대분류를 삭제하면 소분류가 고아 레코드로 남는 문제가 있었다. 대분류 삭제 시 그 소속
-- 소분류를 먼저 지우도록 고친다(사용자가 운영 DB에 직접 적용한 것을 로컬 개발 DB와 저장소
-- 기록에도 반영).
--
-- FK 없이 소분류를 대분류코드로만 연결하고 있어서(TSMMINOR.major_cd, 참조 제약 없음) DB가
-- 강제하지 않으므로, 애플리케이션(이 프로시저)이 순서를 보장해야 한다 - 소분류 먼저, 대분류
-- 나중.

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_MINORCODE_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_major_cd VARCHAR(20),
    @p_major_nm NVARCHAR(200) = NULL,
    @p_sys_yn VARCHAR(1) = 'N',
    @p_rel_cd1 VARCHAR(50) = NULL,
    @p_rel_title1 VARCHAR(50) = NULL,
    @p_rel_cd_type1 VARCHAR(10) = NULL,
    @p_rel_cd2 VARCHAR(50) = NULL,
    @p_rel_title2 VARCHAR(50) = NULL,
    @p_rel_cd_type2 VARCHAR(10) = NULL,
    @p_rel_cd3 VARCHAR(50) = NULL,
    @p_rel_title3 VARCHAR(50) = NULL,
    @p_rel_cd_type3 VARCHAR(10) = NULL,
    @p_rel_cd4 VARCHAR(50) = NULL,
    @p_rel_title4 VARCHAR(50) = NULL,
    @p_rel_cd_type4 VARCHAR(10) = NULL,
    @p_rel_cd5 VARCHAR(50) = NULL,
    @p_rel_title5 VARCHAR(50) = NULL,
    @p_rel_cd_type5 VARCHAR(10) = NULL,
    @p_rel_cd6 VARCHAR(50) = NULL,
    @p_rel_title6 VARCHAR(50) = NULL,
    @p_rel_cd_type6 VARCHAR(10) = NULL,
    @p_rel_cd7 VARCHAR(50) = NULL,
    @p_rel_title7 VARCHAR(50) = NULL,
    @p_rel_cd_type7 VARCHAR(10) = NULL,
    @p_rel_cd8 VARCHAR(50) = NULL,
    @p_rel_title8 VARCHAR(50) = NULL,
    @p_rel_cd_type8 VARCHAR(10) = NULL,
    @p_rel_cd9 VARCHAR(50) = NULL,
    @p_rel_title9 VARCHAR(50) = NULL,
    @p_rel_cd_type9 VARCHAR(10) = NULL,
    @p_rel_cd10 VARCHAR(50) = NULL,
    @p_rel_title10 VARCHAR(50) = NULL,
    @p_rel_cd_type10 VARCHAR(10) = NULL,
    @p_remark NVARCHAR(3000) = NULL,
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
        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSMMAJOR (
                major_cd, major_nm, sys_yn,
                rel_cd1, rel_title1, rel_cd_type1, rel_cd2, rel_title2, rel_cd_type2,
                rel_cd3, rel_title3, rel_cd_type3, rel_cd4, rel_title4, rel_cd_type4,
                rel_cd5, rel_title5, rel_cd_type5, rel_cd6, rel_title6, rel_cd_type6,
                rel_cd7, rel_title7, rel_cd_type7, rel_cd8, rel_title8, rel_cd_type8,
                rel_cd9, rel_title9, rel_cd_type9, rel_cd10, rel_title10, rel_cd_type10,
                remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_major_cd, @p_major_nm, @p_sys_yn,
                @p_rel_cd1, @p_rel_title1, @p_rel_cd_type1, @p_rel_cd2, @p_rel_title2, @p_rel_cd_type2,
                @p_rel_cd3, @p_rel_title3, @p_rel_cd_type3, @p_rel_cd4, @p_rel_title4, @p_rel_cd_type4,
                @p_rel_cd5, @p_rel_title5, @p_rel_cd_type5, @p_rel_cd6, @p_rel_title6, @p_rel_cd_type6,
                @p_rel_cd7, @p_rel_title7, @p_rel_cd_type7, @p_rel_cd8, @p_rel_title8, @p_rel_cd_type8,
                @p_rel_cd9, @p_rel_title9, @p_rel_cd_type9, @p_rel_cd10, @p_rel_title10, @p_rel_cd_type10,
                @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMAJOR SET
                major_nm = @p_major_nm, sys_yn = @p_sys_yn,
                rel_cd1 = @p_rel_cd1, rel_title1 = @p_rel_title1, rel_cd_type1 = @p_rel_cd_type1,
                rel_cd2 = @p_rel_cd2, rel_title2 = @p_rel_title2, rel_cd_type2 = @p_rel_cd_type2,
                rel_cd3 = @p_rel_cd3, rel_title3 = @p_rel_title3, rel_cd_type3 = @p_rel_cd_type3,
                rel_cd4 = @p_rel_cd4, rel_title4 = @p_rel_title4, rel_cd_type4 = @p_rel_cd_type4,
                rel_cd5 = @p_rel_cd5, rel_title5 = @p_rel_title5, rel_cd_type5 = @p_rel_cd_type5,
                rel_cd6 = @p_rel_cd6, rel_title6 = @p_rel_title6, rel_cd_type6 = @p_rel_cd_type6,
                rel_cd7 = @p_rel_cd7, rel_title7 = @p_rel_title7, rel_cd_type7 = @p_rel_cd_type7,
                rel_cd8 = @p_rel_cd8, rel_title8 = @p_rel_title8, rel_cd_type8 = @p_rel_cd_type8,
                rel_cd9 = @p_rel_cd9, rel_title9 = @p_rel_title9, rel_cd_type9 = @p_rel_cd_type9,
                rel_cd10 = @p_rel_cd10, rel_title10 = @p_rel_title10, rel_cd_type10 = @p_rel_cd_type10,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE major_cd = @p_major_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            --MinorCode먼저 삭제
            DELETE FROM TSMMINOR WHERE major_cd = @p_major_cd;
            --MajorCode 삭제
            DELETE FROM TSMMAJOR WHERE major_cd = @p_major_cd;
        END

        SET @GeneratedCode = @p_major_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
