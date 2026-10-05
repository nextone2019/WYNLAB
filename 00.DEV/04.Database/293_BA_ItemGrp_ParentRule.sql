-- 품목그룹 등록: 1레벨은 상위그룹 없이 저장, 2~4레벨은 바로 위 레벨의 상위그룹 필수 (2026-10-05, WYNLAB_DEV 전용)
--  문제: 1레벨을 상위그룹 없이 저장하면 화면이 빈 값을 보내고, 공통 저장 경로가 BIGINT 빈 값을 NULL로 바꿔 넘겨서 TBAITEMGRP.par_grp_id(NOT NULL)에 NULL이 들어가
--        INSERT가 실패했다("처리 중 오류가 발생했습니다", 오류 515). 기존 1레벨 데이터는 par_grp_id = 0(상위 없음)이고, 품목그룹 조회(SSP_CBO_ITEM_GRP_Q p_par_grp_id=0 등)도
--        그 규칙을 쓰므로 컬럼을 NULL 허용으로 바꾸지 않고 프로시저가 규칙을 책임진다.
--  규칙: 1레벨 - 상위그룹 비움/0만 허용(0으로 저장), 값이 있으면 거부. 2~4레벨 - 상위그룹 필수이며 실제로 존재하는 (레벨-1) 그룹이어야 한다. 그룹레벨은 1~4.
--  USP_BA_ITEMGRP_S(라이브 정의)에서 N/U의 par_grp_id 처리와 CATCH의 메시지(50001은 그대로 보여줌)만 바뀐다. 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMGRP_S]
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
        IF @p_work_type IN ('N', 'U')
        BEGIN
            -- 상위그룹 규칙 - 1레벨은 상위 없음(0), 2~4레벨은 바로 위 레벨의 실제 그룹
            DECLARE @lvl INT = TRY_CAST(@p_grp_lvl AS INT);
            IF @lvl IS NULL OR @lvl NOT BETWEEN 1 AND 4 THROW 50001, N'그룹 레벨은 1~4 중에서 선택하세요.', 1;

            IF @lvl = 1
            BEGIN
                IF ISNULL(@p_par_grp_id, 0) <> 0 THROW 50001, N'1레벨 그룹은 상위그룹을 지정할 수 없습니다.', 1;
                SET @p_par_grp_id = 0;
            END
            ELSE
            BEGIN
                IF ISNULL(@p_par_grp_id, 0) = 0 THROW 50001, N'상위그룹을 지정하세요. (1레벨만 상위그룹이 없습니다.)', 1;
                IF NOT EXISTS (SELECT 1 FROM TBAITEMGRP WHERE grp_id = @p_par_grp_id AND TRY_CAST(grp_lvl AS INT) = @lvl - 1)
                    THROW 50001, N'상위그룹은 바로 위 레벨(레벨-1)의 그룹이어야 합니다.', 1;
                IF @p_work_type = 'U' AND @p_par_grp_id = @p_grp_id THROW 50001, N'자기 자신을 상위그룹으로 지정할 수 없습니다.', 1;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TBAITEMGRP (
                acc_id, grp_nm, grp_lvl, par_grp_id, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @p_grp_nm, @p_grp_lvl, @p_par_grp_id, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );
            SET @GeneratedCode = CAST(SCOPE_IDENTITY() AS VARCHAR(20));
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAITEMGRP SET
                acc_id = @p_acc_id,
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
        SET @ReturnMsg = CASE WHEN ERROR_NUMBER() = 50001 THEN LEFT(ERROR_MESSAGE(), 200) ELSE N'처리 중 오류가 발생했습니다.' END;
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
