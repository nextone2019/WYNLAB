-- 품목일괄등록(frmItemMulti) 지원 - 2026-09-16.
-- 화면 자체는 새 저장 프로시저 없이 기존 USP_BA_ITEM_S(N)를 행마다 그대로 재사용한다(TplSingleGrid
-- 패턴과 동일 - "여러 줄 입력 후 한 번에 저장"일 뿐 저장 자체는 frmItem의 신규 등록과 완전히 같은
-- 동작이어야 하므로 프로시저를 분리하지 않는다). 창고/위치/담당부서/담당자/구매처는 팝업
-- 프레임워크(api/lookups/*), 품목그룹1~4는 콤보 프레임워크(api/combo-lookups/*, L_ITEM_GRP)를
-- 그대로 재사용해서 이름->ID를 화면(클라이언트)에서 직접 조회/검증한다 - 둘 다 메뉴의
-- PROC_PREFIX 화이트리스트에 안 걸리는(sysPopUpM/sysLookupM 메타데이터 기반) 별도 통로라 이
-- 화면이 USP_BA_WH_Q 등을 직접 호출할 필요가 없다.
--
-- 여기서 새로 필요한 것은 딱 하나: "붙여넣은/업로드한 품목코드가 이미 DB에 있는지" 한 번에
-- 확인하는 것 - 기존 USP_BA_ITEM_Q의 Q(단건 LIKE 검색)로는 여러 개를 정확히 비교할 수 없어서
-- work_type='Q2'를 추가한다(콤마로 구분된 품목코드 목록 -> STRING_SPLIT으로 정확히 일치하는
-- 것만 돌려줌).

CREATE OR ALTER PROCEDURE [dbo].[USP_BA_ITEM_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_item_id BIGINT = NULL,
    @p_item_no VARCHAR(100) = NULL,
    @p_item_nm NVARCHAR(100) = NULL,
    @p_item_no_list NVARCHAR(MAX) = NULL,
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
                a.item_id,
                a.item_no,
                a.item_nm,
                a.item_spec,
                a.unit_cd,
                a.po_unit_cd,
                a.wh_id,
                e.wh_nm,
                a.loc_id,
                f.loc_nm,
                a.safe_qty,
                a.dept_id,
                b.dept_nm,
                a.emp_id,
                c.emp_no,
                c.emp_nm,
                a.cust_id,
                d.cust_nm,
                a.asset_type,
                a.out_type,
                a.po_qc_yn,
                a.prod_qc_yn,
                a.lot_yn,
                a.stock_yn,
                a.stat_cd,
                a.grp1_id,
                a.grp2_id,
                a.grp3_id,
                a.grp4_id,
                a.remark
            FROM        TBAITEM as a
                        LEFT OUTER JOIN TBADEPT as b on a.dept_id = b.dept_id
                        LEFT OUTER JOIN TBAEMP as c on a.emp_id = c.emp_id
                        LEFT OUTER JOIN TBACUST as d on a.cust_id = d.cust_id
                        LEFT OUTER JOIN TBAWH as e on a.wh_id = e.wh_id
                        LEFT OUTER JOIN TBALOC as f on a.loc_id = f.loc_id
            WHERE       1 = 1
   --         AND         (@p_item_id IS NULL OR a.item_id = @p_item_id)
            AND         (@p_item_no IS NULL OR a.item_no LIKE '%' + @p_item_no + '%')
            AND         (@p_item_nm IS NULL OR a.item_nm LIKE '%' + @p_item_nm + '%')
            ORDER BY a.item_id;
        END
        ELSE IF @p_work_type = 'Q2'
        BEGIN
            -- frmItemMulti(품목일괄등록)의 붙여넣기/업로드 검증용 - 콤마로 구분된 품목코드 목록
            -- 중 이미 TBAITEM에 존재하는 것만(정확히 일치) 돌려준다.
            SELECT a.item_no, a.item_nm
            FROM TBAITEM a
            WHERE a.item_no IN (SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@p_item_no_list, ','));
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

IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MODULE = 'BA' AND SCREEN_CLASS_NM = 'frmItemMulti')
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'품목일괄등록', 9, 3, 'FORM', 'BA', 'frmItemMulti', 'USP_BA_ITEM_', 15, 'Y', SUSER_SNAME(), GETDATE());
END
GO
