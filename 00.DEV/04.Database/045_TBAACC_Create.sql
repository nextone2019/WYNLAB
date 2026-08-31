-- TBAACC(사업장정보) 신규 - 사업장등록 화면(frmAcc, 범용 데이터 통로 사용) 지원용.
-- 지금은 acc_cd/acc_nm만 (자세한 컬럼정의는 나중에 추가 예정) + 다른 BA 테이블들과 공통으로
-- 쓰는 감사 컬럼(reg_user_id/reg_dt/reg_pc/upt_user_id/upt_dt/upt_pc, TBADEPT/TBAITEMGRP와
-- 동일한 타입/NULL 여부로 맞춤).
--
-- 다른 BA 테이블(TBADEPT 등)의 acc_cd는 "이 행이 어느 사업장 소속인지"를 가리키는 스코프
-- 컬럼이고, 이 테이블의 acc_cd는 그 값들이 실제로 참조하는 사업장 자체(PK)다 - 그래서 다른
-- 테이블처럼 acc_cd에 '0001' 기본값을 주는 대신, 이 테이블에서는 acc_cd를 직접 입력받는다.

CREATE TABLE TBAACC (
    ACC_CD       VARCHAR(20)   NOT NULL,
    ACC_NM       NVARCHAR(100) NULL,
    REG_USER_ID  VARCHAR(50)   NOT NULL,
    REG_DT       DATETIME      NULL,
    REG_PC       NVARCHAR(400) NULL,
    UPT_USER_ID  VARCHAR(50)   NULL,
    UPT_DT       DATETIME      NULL,
    UPT_PC       NVARCHAR(400) NULL,
    CONSTRAINT PK_TBAACC PRIMARY KEY CLUSTERED (ACC_CD)
);
GO

/* ---------- USP_BA_ACC_Q: 사업장 목록 조회 ---------- */
CREATE PROCEDURE USP_BA_ACC_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(20) = NULL,
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
            SELECT acc_cd, acc_nm
            FROM TBAACC
            WHERE (@p_acc_cd IS NULL OR acc_cd LIKE '%' + @p_acc_cd + '%')
              AND (@p_acc_nm IS NULL OR acc_nm LIKE '%' + @p_acc_nm + '%')
            ORDER BY acc_cd;
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

/* ---------- USP_BA_ACC_S: 사업장 등록/수정/삭제 ---------- */
CREATE PROCEDURE USP_BA_ACC_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_cd VARCHAR(20),
    @p_acc_nm NVARCHAR(100) = NULL,
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
            INSERT INTO TBAACC (acc_cd, acc_nm, reg_user_id, reg_dt, reg_pc)
            VALUES (@p_acc_cd, @p_acc_nm, @p_user_id, GETDATE(), @p_client_pc);
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TBAACC SET
                acc_nm = @p_acc_nm, upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
            WHERE acc_cd = @p_acc_cd;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TBAACC WHERE acc_cd = @p_acc_cd;
        END

        SET @GeneratedCode = @p_acc_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 메뉴등록: 기준정보관리(BA) 바로 아래, 거래처등록/품목그룹등록과 같은 레벨. 범용 데이터
-- 통로를 쓰므로 PROC_PREFIX만 맞춰두면 서버 코드 없이 바로 동작한다(GENERIC_DATA_API.md).
INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, SORT_ORDER, USE_YN, REG_DT, PROC_PREFIX)
VALUES ('BA_ACC', N'사업장등록', 'BA', 3, 'FORM', 'WYNLAB.BA.frmAcc, WYNLAB.BA', 5, 'Y', GETDATE(), 'USP_BA_ACC_');
GO
