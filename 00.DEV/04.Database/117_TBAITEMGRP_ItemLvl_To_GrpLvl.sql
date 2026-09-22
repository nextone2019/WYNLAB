-- TBAITEMGRP.item_lvl -> grp_lvl 컬럼명 변경(요청사항). 이미 적용됨(이 파일을 재실행해도
-- sp_rename은 컬럼이 이미 grp_lvl이면 에러 없이 넘어가지 않으므로, 재실행 전엔 아래 IF로 존재
-- 여부를 확인한다.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('TBAITEMGRP') AND name = 'item_lvl')
    EXEC sp_rename 'TBAITEMGRP.item_lvl', 'grp_lvl', 'COLUMN';
GO

-- USP_BA_ITEMGRP_Q/_S 전면 재작성 - 이 두 프로시저는 이번 rename과 무관하게 이미 026 마이그레이션
-- 시절의 옛 컬럼명(item_class_cd/item_class_nm/par_item_class_cd)을 그대로 참조하고 있어서,
-- TBAITEMGRP가 grp_id/grp_nm/par_grp_id로 재설계된 뒤로 계속 깨져 있던 상태였다(CREATE OR ALTER
-- 자체가 "잘못된 열 이름"으로 실패 - 실제로 이번 마이그레이션 적용 중 확인함). 아직 frmItemGrp
-- 화면이 없어 호출하는 곳이 없으므로(BA 모듈 어디에도 참조 없음, 확인함) 동작을 바꿔도 위험이
-- 없다 - 구조가 거의 동일한 USP_BA_DEPT_Q/_S(자기참조 계층형 테이블, par_dept_id/dept_nm 패턴)를
-- 그대로 본떠 지금의 실제 TBAITEMGRP 스키마(grp_id/grp_nm/grp_lvl/par_grp_id/remark/acc_id)에
-- 맞춰 새로 짰다.
CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMGRP_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_grp_id BIGINT = NULL,
    @p_grp_nm NVARCHAR(30) = NULL,
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
                        a.acc_id,
                        a.grp_id,
                        a.grp_nm,
                        a.grp_lvl,
                        a.par_grp_id,
                        b.grp_nm as par_grp_nm,
                        a.remark
            FROM TBAITEMGRP as a
                        LEFT OUTER JOIN TBAITEMGRP as b on a.par_grp_id = b.grp_id
            WHERE (@p_grp_id IS NULL OR a.grp_id = @p_grp_id)
              AND (@p_grp_nm IS NULL OR a.grp_nm LIKE '%' + @p_grp_nm + '%')
            ORDER BY a.grp_id;
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

CREATE OR ALTER PROCEDURE USP_BA_ITEMGRP_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,	/* TODO: 로그인 세션에 사업장 생기면 서버가 채우도록 전환 */
    @p_grp_id BIGINT = NULL,		/* U/D일 때 필수 - N에서는 안 씀(신규 생성) */
    @p_grp_nm NVARCHAR(30) = NULL,
    @p_grp_lvl VARCHAR(10) = NULL,
    @p_par_grp_id BIGINT = NULL,
    @p_remark NVARCHAR(1000) = NULL,
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
            INSERT INTO TBAITEMGRP (
                acc_id, grp_nm, grp_lvl, par_grp_id, remark,
                reg_user_id, reg_dt, reg_pc
            )
            VALUES (
                @p_acc_id, @p_grp_nm, @p_grp_lvl, @p_par_grp_id, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMGRP SET
                grp_nm = @p_grp_nm, grp_lvl = @p_grp_lvl, par_grp_id = @p_par_grp_id,
                remark = @p_remark, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE grp_id = @p_grp_id;
            SET @GeneratedCode = CAST(@p_grp_id AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAITEMGRP WHERE grp_id = @p_grp_id;
            SET @GeneratedCode = CAST(@p_grp_id AS VARCHAR(20));
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
