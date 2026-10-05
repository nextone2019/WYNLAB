-- 거래처분류(BA0003, TSMMINOR에 이미 등록: OS=외주거래처/PO=매입거래처/SA=매출거래처) 다중선택
-- 지원 - 거래처 1개가 분류 여러 개에 동시에 속할 수 있어야 해서(예: 매입+매출 겸업) TBACUST에
-- 단일 컬럼으로 두지 않고 다대다 연결테이블을 새로 둔다. frmUserAuth의 TSMUSERGRPMAP(사용자-
-- 그룹) 체크박스 그리드와 같은 성격이지만, 감사컬럼은 TBACUSTPRSN/TBACUSTACNT와 같은
-- 최신 6컬럼 세트(reg_*/upt_*)를 따른다.

IF OBJECT_ID('TBACUSTCLASS') IS NOT NULL DROP TABLE TBACUSTCLASS;
GO
CREATE TABLE TBACUSTCLASS (
    CUST_ID      BIGINT        NOT NULL,
    class_cd     VARCHAR(20)   NOT NULL, -- TSMMINOR.minor_cd (major_cd='BA0003')
    reg_user_id  VARCHAR(30)   NULL,
    reg_dt       DATETIME      NULL,
    reg_pc       NVARCHAR(200) NULL,
    upt_user_id  VARCHAR(30)   NULL,
    upt_dt       DATETIME      NULL,
    upt_pc       NVARCHAR(200) NULL,
    CONSTRAINT PK_TBACUSTCLASS PRIMARY KEY CLUSTERED (CUST_ID, class_cd)
);
GO

-- USP_BA_CUST_Q의 Q1(상세 탭 다중 레코드셋)에 거래처분류 목록(체크박스 그리드용) 추가 -
-- BA0003 전체 코드를 항상 보여주고, 이 거래처가 이미 속한 분류만 is_member=1로 표시한다
-- (frmUserAuth.USP_SM_USERAUTH_Q_2의 "전체 목록 LEFT JOIN 소속여부"와 같은 방식).
CREATE OR ALTER PROCEDURE USP_BA_CUST_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT = NULL,
    @p_cust_nm NVARCHAR(100) = NULL,
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
                a.CUST_ID, a.cust_nm, a.biz_no, a.tel, a.cur_cd, a.owner_nm, a.zip_code, a.addr1, a.addr2,
                a.homepage, a.email, a.fax, a.biz_kind, a.biz_type, a.trans_open_date, a.vat_type, a.vat_rate,
                a.remark, a.stat_cd, a.EMP_ID, b.emp_nm
            FROM TBACUST a
            LEFT JOIN TBAEMP b ON a.EMP_ID = b.EMP_ID
            WHERE  1 =1
                --AND (@p_cust_id IS NULL OR a.CUST_ID = @p_cust_id)
              AND (@p_cust_nm IS NULL OR a.cust_nm LIKE '%' + @p_cust_nm + '%')
            ORDER BY a.CUST_ID;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT CUST_ID, serl, prsn_nm, grade, tel1, tel2, fax, email
            FROM TBACUSTPRSN
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;

            SELECT CUST_ID, serl, bank_cd, acnt_no, remark
            FROM TBACUSTACNT
            WHERE CUST_ID = @p_cust_id
            ORDER BY serl;

            SELECT mn.minor_cd AS class_cd, mn.minor_nm AS class_nm,
                   CASE WHEN c.CUST_ID IS NULL THEN 0 ELSE 1 END AS is_member
            FROM TSMMINOR mn
            LEFT JOIN TBACUSTCLASS c ON c.class_cd = mn.minor_cd AND c.CUST_ID = @p_cust_id
            WHERE mn.major_cd = 'BA0003' AND mn.use_yn = 'Y'
            ORDER BY mn.sort, mn.minor_cd;
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

-- USP_BA_CUST_S_1(담당자)/S_2(계좌)와 같은 자리의 세 번째 상세 저장 프로시저 - 체크박스
-- 토글이라 행이 새로 생기거나 지워지는 게 아니라 is_member 값만 바뀌므로(DataRowState는
-- 항상 Modified='U') N/D 없이 U 하나로 켜짐(추가)/꺼짐(삭제)을 함께 처리한다.
CREATE OR ALTER PROCEDURE USP_BA_CUST_S_3
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_cust_id BIGINT,
    @p_class_cd VARCHAR(20) = NULL,
    @p_is_member VARCHAR(1) = NULL,
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
        IF @p_work_type = 'U'
        BEGIN
            IF @p_is_member = '1'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM TBACUSTCLASS WHERE CUST_ID = @p_cust_id AND class_cd = @p_class_cd)
                    INSERT INTO TBACUSTCLASS (CUST_ID, class_cd, reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
                    VALUES (@p_cust_id, @p_class_cd, @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc);
            END
            ELSE
                DELETE FROM TBACUSTCLASS WHERE CUST_ID = @p_cust_id AND class_cd = @p_class_cd;
        END

        SET @GeneratedCode = @p_class_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
