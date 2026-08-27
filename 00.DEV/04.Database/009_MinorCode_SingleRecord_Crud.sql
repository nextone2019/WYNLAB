-- USP_SM_MINORCODE_S_1을 007(그리드 JSON 일괄 처리)에서 USP_SM_MINORCODE_S와 완전히 같은
-- 형태(단일 레코드, @p_work_type 분기, 파라미터 하나하나 나열)로 다시 바꾼다. 유사한 다른
-- 사이트의 P_FSMMINOR_S1 프로시저를 참고했다 - major_cd+minor_cd가 키인 것도 동일하다.
--
-- [007과 달라지는 점] 007은 그리드 전체를 JSON으로 한 번에 받아 프로시저 안에서 행 단위로
-- 나눠 처리했다. 이번엔 그 반대로, 프로시저는 레코드 하나만 다루고 화면(frmMinorCode.cs)이
-- 그리드에서 바뀐 행 수만큼 이 프로시저를 순서대로 호출한다 - USP_SM_MINORCODE_S(대분류)가
-- 항상 레코드 하나만 다루는 것과 완전히 같은 모양을 소분류에도 그대로 적용한 것.
--
-- [minor_cd(키)는 수정 대상이 아니다] major_cd가 한 번 등록되면 안 바뀌는 키로 다뤄지는 것과
-- 같은 이유로, 이 프로시저의 'U' 분기도 minor_cd를 WHERE에서만 쓰고 SET하지 않는다. 그리드에서
-- 코드 자체를 고친 경우는 화면이 "원래 코드 삭제 + 새 코드 등록"(D 호출 + N 호출)으로 표현한다.
--
-- [트랜잭션 범위가 좁아지는 점을 알아둘 것] 007은 그리드 전체가 한 트랜잭션이라 중간에 하나가
-- 실패해도 전부 롤백됐다. 이 방식은 호출마다 독립 트랜잭션이라, 그리드에서 여러 행이 바뀌었을
-- 때 일부는 반영되고 일부만 실패할 수 있다. 화면은 실패한 지점에서 멈추고 어떤 행인지 알려준다.

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_MINORCODE_S_1]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_major_cd VARCHAR(20),
    @p_minor_cd VARCHAR(100),
    @p_minor_nm NVARCHAR(200) = NULL,
    @p_sort INT = 0,
    @p_sys_yn VARCHAR(1) = 'N',
    @p_use_yn VARCHAR(1) = 'Y',
    @p_rel_cd1 VARCHAR(50) = NULL,
    @p_rel_cd2 VARCHAR(50) = NULL,
    @p_rel_cd3 VARCHAR(50) = NULL,
    @p_rel_cd4 VARCHAR(50) = NULL,
    @p_rel_cd5 VARCHAR(50) = NULL,
    @p_rel_cd6 VARCHAR(50) = NULL,
    @p_rel_cd7 VARCHAR(50) = NULL,
    @p_rel_cd8 VARCHAR(50) = NULL,
    @p_rel_cd9 VARCHAR(50) = NULL,
    @p_rel_cd10 VARCHAR(50) = NULL,
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
            INSERT INTO TSMMINOR (
                major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn,
                rel_cd1, rel_cd2, rel_cd3, rel_cd4, rel_cd5,
                rel_cd6, rel_cd7, rel_cd8, rel_cd9, rel_cd10,
                remark, reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_major_cd, @p_minor_cd, @p_minor_nm, @p_sort, @p_sys_yn, @p_use_yn,
                @p_rel_cd1, @p_rel_cd2, @p_rel_cd3, @p_rel_cd4, @p_rel_cd5,
                @p_rel_cd6, @p_rel_cd7, @p_rel_cd8, @p_rel_cd9, @p_rel_cd10,
                @p_remark, @p_user_id, GETDATE(), @p_client_pc
            );
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSMMINOR SET
                minor_nm = @p_minor_nm, sort = @p_sort, sys_yn = @p_sys_yn, use_yn = @p_use_yn,
                rel_cd1 = @p_rel_cd1, rel_cd2 = @p_rel_cd2, rel_cd3 = @p_rel_cd3,
                rel_cd4 = @p_rel_cd4, rel_cd5 = @p_rel_cd5, rel_cd6 = @p_rel_cd6,
                rel_cd7 = @p_rel_cd7, rel_cd8 = @p_rel_cd8, rel_cd9 = @p_rel_cd9, rel_cd10 = @p_rel_cd10,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE major_cd = @p_major_cd AND minor_cd = @p_minor_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSMMINOR WHERE major_cd = @p_major_cd AND minor_cd = @p_minor_cd;
        END

        SET @GeneratedCode = @p_minor_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
