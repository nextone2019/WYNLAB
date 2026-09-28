-- 영업관리모듈 시작 - 판매단가(TSAPRICE) 테이블 + 프로시저(2026-09-28).
-- 구매단가(TMAPOPRICE, 185/186/212)와 완전히 같은 구조/규칙, 품목당 단위가 하나뿐이라(구매단위 같은
-- 별도 컬럼이 없음) po_unit_cd 대신 TBAITEM.unit_cd를 그대로 쓴다.
--
-- 적용 규칙(TMAPOPRICE와 동일):
--  * cust_id가 NULL이면 "전체 거래처 공통 단가", 값이 있으면 그 거래처에만 적용되는 단가.
--    수주 시 단가를 찾을 때는 (해당 거래처 단가) -> (공통 단가) 순으로 우선한다.
--  * 적용기간은 start_date ~ end_date(VARCHAR(8), yyyyMMdd, 양끝 포함). 종료일 미정이면 '99991231'.
--  * 같은 (사업장, 품목, 거래처(또는 공통), 시작일)은 한 건만 - 유니크 인덱스.
--  * 기간이 겹치는 단가는 인덱스로 막을 수 없어 저장 프로시저에서 검증한다.
--
-- USP_SA_PRICE_Q 작업구분(frmSaPrice, 품목 마스터 중심 화면 - frmPoPrice와 동일 UX):
--   'Q'  품목 목록 + 최종단가(오늘 이전 시작 중 최신 1건, 없으면 가장 가까운 예정, 거래처 무관).
--   'Q1' 기준일에 실제로 적용되는 단가 1건(수주 화면이 사용 - 거래처 지정 단가 우선, 없으면 공통).
--   'Q2' 선택한 품목의 단가 목록(grd2).

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSAPRICE')
BEGIN
    CREATE TABLE TSAPRICE (
        price_id      BIGINT IDENTITY(1,1) NOT NULL,
        acc_id        BIGINT         NOT NULL,                       -- 사업장
        item_id       BIGINT         NOT NULL,                       -- 품목(TBAITEM.item_id)
        cust_id       BIGINT         NULL,                           -- 거래처(TBACUST.cust_id). NULL = 전체 거래처 공통
        start_date    VARCHAR(8)     NOT NULL,                       -- 적용 시작일(yyyyMMdd)
        end_date      VARCHAR(8)     NOT NULL CONSTRAINT DF_TSAPRICE_end_date DEFAULT '99991231', -- 적용 종료일
        cur_cd        VARCHAR(10)    NULL,                           -- 통화(L_CM0003)
        unit_cd       VARCHAR(10)    NULL,                           -- 단가 기준 단위
        price         NUMERIC(18,4)  NOT NULL,                       -- 판매단가
        remark        NVARCHAR(3000) NULL,
        reg_user_id   VARCHAR(30)    NULL,
        reg_dt        DATETIME       NULL,
        reg_pc        NVARCHAR(200)  NULL,
        upt_user_id   VARCHAR(30)    NULL,
        upt_dt        DATETIME       NULL,
        upt_pc        NVARCHAR(200)  NULL,
        CONSTRAINT PK_TSAPRICE PRIMARY KEY CLUSTERED (price_id),
        CONSTRAINT CK_TSAPRICE_period CHECK (start_date <= end_date)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_TSAPRICE_key
        ON TSAPRICE (acc_id, item_id, cust_id, start_date);

    CREATE NONCLUSTERED INDEX IX_TSAPRICE_lookup
        ON TSAPRICE (acc_id, item_id, start_date, end_date)
        INCLUDE (cust_id, price, cur_cd, unit_cd);
END
GO

-- ============================================================
-- USP_SA_PRICE_Q
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_PRICE_Q
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_item_kw NVARCHAR(100) = NULL,       /* 'Q': 품번/품명 검색어(부분 일치) */
    @p_price_yn VARCHAR(1) = NULL,         /* 'Q': 'Y'=단가가 등록된 품목만, 'N'=단가가 없는 품목만, 그 외=전체 */
    @p_cust_id BIGINT = NULL,              /* 'Q1': 거래처 */
    @p_base_date VARCHAR(8) = NULL,        /* 'Q1': 단가를 찾을 기준일 */
    ---------------------------------------------------------------------------------------------------
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
    DECLARE @v_today VARCHAR(8);
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        SET @v_today = CONVERT(VARCHAR(8), GETDATE(), 112);

        IF @p_work_type = 'Q'
        BEGIN
            SELECT
                i.acc_id, i.item_id, i.item_no, i.item_nm, i.item_spec,
                i.unit_cd, i.stat_cd,
                i.cust_id, ic.cust_nm,
                lp.price_id AS last_price_id,
                lp.price AS last_price,
                lp.cur_cd AS last_cur_cd,
                lp.unit_cd AS last_unit_cd,
                lp.cust_id AS last_cust_id,
                lc.cust_nm AS last_cust_nm,
                lp.start_date AS last_start_date,
                lp.end_date AS last_end_date,
                CASE WHEN lp.price_id IS NULL THEN NULL
                     WHEN lp.end_date < @v_today THEN N'만료'
                     WHEN lp.start_date > @v_today THEN N'예정'
                     ELSE N'적용중' END AS last_stat_nm
            FROM TBAITEM i
                LEFT JOIN TBACUST ic ON ic.cust_id = i.cust_id
                OUTER APPLY (
                    SELECT TOP 1 p.price_id, p.price, p.cur_cd, p.unit_cd, p.cust_id, p.start_date, p.end_date
                    FROM TSAPRICE p
                    WHERE p.item_id = i.item_id
                      AND (@p_acc_id IS NULL OR p.acc_id = @p_acc_id)
                    ORDER BY CASE WHEN p.start_date <= @v_today THEN 0 ELSE 1 END,
                             CASE WHEN p.start_date <= @v_today THEN p.start_date END DESC,
                             p.start_date ASC,
                             p.price_id DESC
                ) lp
                LEFT JOIN TBACUST lc ON lc.cust_id = lp.cust_id
            WHERE (@p_acc_id IS NULL OR i.acc_id = @p_acc_id)
              AND (@p_item_kw IS NULL OR @p_item_kw = N''
                   OR i.item_no LIKE N'%' + @p_item_kw + N'%'
                   OR i.item_nm LIKE N'%' + @p_item_kw + N'%')
              AND (ISNULL(@p_price_yn, '') NOT IN ('Y', 'N')
                   OR (@p_price_yn = 'Y' AND lp.price_id IS NOT NULL)
                   OR (@p_price_yn = 'N' AND lp.price_id IS NULL))
            ORDER BY i.item_no, i.item_id;
        END
        ELSE IF @p_work_type = 'Q1'
        BEGIN
            SELECT TOP 1
                p.price_id, p.item_id, p.cust_id, p.start_date, p.end_date,
                p.cur_cd, p.unit_cd, p.price
            FROM TSAPRICE p
            WHERE p.acc_id = @p_acc_id
              AND p.item_id = @p_item_id
              AND p.start_date <= ISNULL(@p_base_date, @v_today)
              AND p.end_date >= ISNULL(@p_base_date, @v_today)
              AND (p.cust_id IS NULL OR p.cust_id = @p_cust_id)
            ORDER BY CASE WHEN p.cust_id IS NULL THEN 1 ELSE 0 END, p.start_date DESC;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            SELECT
                p.price_id, p.acc_id,
                p.item_id, i.item_no, i.item_nm, i.item_spec,
                p.cust_id, c.cust_nm,
                p.start_date, p.end_date,
                p.cur_cd, p.unit_cd, p.price, p.remark,
                CASE WHEN p.end_date < @v_today THEN N'만료'
                     WHEN p.start_date > @v_today THEN N'예정'
                     ELSE N'적용중' END AS stat_nm
            FROM TSAPRICE p
                LEFT JOIN TBAITEM i ON i.item_id = p.item_id
                LEFT JOIN TBACUST c ON c.cust_id = p.cust_id
            WHERE p.item_id = @p_item_id
              AND (@p_acc_id IS NULL OR p.acc_id = @p_acc_id)
            ORDER BY CASE WHEN p.cust_id IS NULL THEN 0 ELSE 1 END, c.cust_nm, p.start_date DESC;
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

-- ============================================================
-- USP_SA_PRICE_S - 행 하나 N/U/D.
-- ============================================================
CREATE OR ALTER PROCEDURE USP_SA_PRICE_S
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_price_id BIGINT = NULL,
    @p_acc_id BIGINT = NULL,
    @p_item_id BIGINT = NULL,
    @p_cust_id BIGINT = NULL,
    @p_start_date VARCHAR(8) = NULL,
    @p_end_date VARCHAR(8) = NULL,
    @p_cur_cd VARCHAR(10) = NULL,
    @p_unit_cd VARCHAR(10) = NULL,
    @p_price NUMERIC(18,4) = NULL,
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
        IF @p_work_type IN ('N', 'U')
        BEGIN
            IF @p_end_date IS NULL OR @p_end_date = '' SET @p_end_date = '99991231';
            IF @p_cust_id = 0 SET @p_cust_id = NULL;

            IF @p_acc_id IS NULL OR @p_item_id IS NULL
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'사업장과 품목은 필수입니다.'; RETURN;
            END
            IF ISDATE(@p_start_date) = 0 OR LEN(@p_start_date) <> 8 OR ISDATE(@p_end_date) = 0 OR LEN(@p_end_date) <> 8
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'적용 시작일/종료일 형식이 올바르지 않습니다.'; RETURN;
            END
            IF @p_start_date > @p_end_date
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'적용 시작일이 종료일보다 늦습니다.'; RETURN;
            END
            IF @p_price IS NULL OR @p_price < 0
            BEGIN
                SET @ReturnCode = -1; SET @ReturnMsg = N'단가는 0 이상의 값이어야 합니다.'; RETURN;
            END

            DECLARE @dup_start VARCHAR(8), @dup_end VARCHAR(8);
            SELECT TOP 1 @dup_start = start_date, @dup_end = end_date
            FROM TSAPRICE
            WHERE acc_id = @p_acc_id
              AND item_id = @p_item_id
              AND ISNULL(cust_id, 0) = ISNULL(@p_cust_id, 0)
              AND price_id <> ISNULL(@p_price_id, 0)
              AND start_date <= @p_end_date
              AND end_date >= @p_start_date
            ORDER BY start_date;

            IF @dup_start IS NOT NULL
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'같은 품목/거래처에 기간이 겹치는 단가가 있습니다. (' + @dup_start + N' ~ '
                                 + CASE WHEN @dup_end = '99991231' THEN N'무기한' ELSE @dup_end END + N')';
                RETURN;
            END
        END

        IF @p_work_type = 'N'
        BEGIN
            INSERT INTO TSAPRICE (
                acc_id, item_id, cust_id, start_date, end_date, cur_cd, unit_cd, price, remark,
                reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
            )
            VALUES (
                @p_acc_id, @p_item_id, @p_cust_id, @p_start_date, @p_end_date, @p_cur_cd, @p_unit_cd, @p_price, @p_remark,
                @p_user_id, GETDATE(), @p_client_pc, @p_user_id, GETDATE(), @p_client_pc
            );

            SET @p_price_id = SCOPE_IDENTITY();
        END
        ELSE IF @p_work_type = 'U'
        BEGIN
            UPDATE TSAPRICE SET
                item_id = @p_item_id,
                cust_id = @p_cust_id,
                start_date = @p_start_date,
                end_date = @p_end_date,
                cur_cd = @p_cur_cd,
                unit_cd = @p_unit_cd,
                price = @p_price,
                remark = @p_remark,
                upt_user_id = @p_user_id,
                upt_dt = GETDATE(),
                upt_pc = @p_client_pc
            WHERE price_id = @p_price_id;
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            DELETE FROM TSAPRICE WHERE price_id = @p_price_id;
        END

        SET @GeneratedCode = CAST(@p_price_id AS VARCHAR(20));
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 메뉴는 이미 만들어져 있던 판매단가등록(MENU_ID=18, 영업기준관리 하위) 자리를 채운다 - 새로 INSERT하지 않는다.
UPDATE TSMMENU
SET MODULE = 'SA', SCREEN_CLASS_NM = 'frmSaPrice', PROC_PREFIX = 'USP_SA_',
    upt_user_id = SUSER_SNAME(), upt_dt = GETDATE()
WHERE MENU_ID = 18 AND MENU_NM = N'판매단가등록';
GO
