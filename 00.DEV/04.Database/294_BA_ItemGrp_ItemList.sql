-- 품목그룹등록: 선택한 그룹에 속한 품목 리스트 (2026-10-05, WYNLAB_DEV 전용)
--  USP_BA_ITEMGRP_Q에 work_type 'Q1'(+ @p_grp_id)을 추가한다 - 그 그룹의 레벨에 맞는 품목 컬럼(1레벨=grp1_id ... 4레벨=grp4_id)이 그 그룹 id인 품목.
--  상위 그룹을 고르면 그 아래 하위 그룹에 속한 품목도 함께 나온다(품목은 grp1~4를 모두 가지고 있으므로). 기존 'Q'(트리 목록)는 그대로.
--  화면 메뉴의 PROC_PREFIX(USP_BA_ITEMGRP_) 안에 들어오므로 새 프로시저 없이 이 프로시저에 붙였다. 여러 번 실행해도 안전하다.

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEMGRP_Q]
    @p_acc_id BIGINT = NULL,        /* 사업장 - 화면 조회조건 필수(표준), 비우면 전체 */
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_grp_id BIGINT = NULL,        /* Q1 - 품목을 볼 그룹 */
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
            WHERE (@p_acc_id IS NULL OR a.acc_id = @p_acc_id)
              AND 1 = 1
              AND (@p_grp_nm IS NULL OR a.grp_nm LIKE '%' + @p_grp_nm + '%')
            ORDER BY a.grp_id;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            DECLARE @gid VARCHAR(20) = CAST(@p_grp_id AS VARCHAR(20));

            SELECT
                        i.item_id,
                        i.item_no,
                        i.item_nm,
                        i.item_spec,
                        u.minor_nm AS unit_nm,
                        t.minor_nm AS asset_nm,
                        w.wh_nm,
                        i.stock_yn,
                        i.lot_yn,
                        s.minor_nm AS stat_nm
            FROM TBAITEMGRP g
                        JOIN TBAITEM i ON (@p_acc_id IS NULL OR i.acc_id = @p_acc_id)
                                      AND @gid = CASE g.grp_lvl WHEN '1' THEN CAST(i.grp1_id AS VARCHAR(20))
                                                                WHEN '2' THEN CAST(i.grp2_id AS VARCHAR(20))
                                                                WHEN '3' THEN CAST(i.grp3_id AS VARCHAR(20))
                                                                WHEN '4' THEN CAST(i.grp4_id AS VARCHAR(20)) END
                        LEFT OUTER JOIN TBAWH w ON w.wh_id = i.wh_id
                        LEFT OUTER JOIN TSMMINOR u ON u.major_cd = 'CM0001' AND u.minor_cd = i.unit_cd
                        LEFT OUTER JOIN TSMMINOR t ON t.major_cd = 'CM0002' AND t.minor_cd = i.asset_type
                        LEFT OUTER JOIN TSMMINOR s ON s.major_cd = 'BA0002' AND s.minor_cd = i.stat_cd
            WHERE g.grp_id = @p_grp_id
            ORDER BY i.item_no;
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
