-- 거래처 팝업(P_CUST) 개선 (2026-10-01)
--  1) 팝업 제목 '구매처 조회' -> '거래처 조회', 검색창/컬럼 캡션 '구매처명' -> '거래처명'
--  2) 조회조건에 거래처분류(L_BA0003: SA 매출/PO 매입/OS 외주) 추가 - 비우면 전체. 콤마로 여러 분류도 가능('PO,OS').
--     화면이 팝업을 열 때 이 조건의 기본값을 미리 넘겨 모듈별로 매출처/구매처/외주처만 보이게 하는 것도 같은 파라미터(p_cust_class)를 쓴다.
--  3) 그리드에 거래처분류(class_nm) 컬럼 추가
-- 주의: PopupLookupEditWyn의 "조용한 검색"(입력 후 포커스 이동)은 그 팝업의 모든 조회조건에 타이핑한 값을 넣는다 - p_cust_class에도
--       거래처명이 들어오므로, 분류코드가 아닌 값은 무시(전체)하도록 프로시저에서 걸러낸다.

CREATE OR ALTER PROCEDURE SSP_POP_CUST_Q
    @p_keyword VARCHAR(100) = NULL,
    @p_cust_class VARCHAR(100) = NULL       /* 거래처분류(BA0003) 코드, 콤마 구분 복수 가능. 비면 전체 */
AS
BEGIN
    SET NOCOUNT ON;

    -- 분류코드 목록이 아니면(타이핑한 거래처명이 들어온 경우 등) 전체로 취급
    IF ISNULL(@p_cust_class, '') <> ''
       AND EXISTS (SELECT 1 FROM STRING_SPLIT(@p_cust_class, ',') s
                   WHERE LTRIM(RTRIM(s.value)) NOT IN (SELECT minor_cd FROM TSMMINOR WHERE major_cd = 'BA0003'))
        SET @p_cust_class = NULL;

    SELECT a.CUST_ID, a.cust_nm, a.biz_no, a.owner_nm, a.tel, a.cur_cd, a.vat_type, b.minor_nm AS vat_type_nm, a.vat_rate,
           (SELECT STRING_AGG(m.minor_nm, ', ') WITHIN GROUP (ORDER BY m.sort)
            FROM TBACUSTCLASS cc JOIN TSMMINOR m ON m.major_cd = 'BA0003' AND m.minor_cd = cc.class_cd
            WHERE cc.CUST_ID = a.CUST_ID) AS class_nm
    FROM TBACUST AS a
        LEFT OUTER JOIN TSMMINOR AS b ON b.major_cd = 'CM0004' AND b.minor_cd = a.vat_type
    WHERE (@p_keyword IS NULL OR @p_keyword = '' OR a.cust_nm LIKE '%' + @p_keyword + '%')
      AND (ISNULL(@p_cust_class, '') = ''
           OR EXISTS (SELECT 1 FROM TBACUSTCLASS c
                      WHERE c.CUST_ID = a.CUST_ID
                        AND c.class_cd IN (SELECT LTRIM(RTRIM(s.value)) FROM STRING_SPLIT(@p_cust_class, ',') s)))
    ORDER BY a.cust_nm;
END
GO

UPDATE sysPopUpM SET popup_nm = N'거래처 조회', upt_user_id = 'admin', upt_dt = GETDATE() WHERE popup_key = 'P_CUST';
UPDATE sysPopUpS SET caption = N'거래처명' WHERE popup_key = 'P_CUST' AND param_nm = 'p_keyword';
UPDATE sysPopUpD SET caption = N'거래처명' WHERE popup_key = 'P_CUST' AND column_nm = 'cust_nm';

IF NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_CUST' AND param_nm = 'p_cust_class')
    INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width, lookup_key, row_no, control_nm)
    VALUES ('P_CUST', 'p_cust_class', N'거래처분류', 'LOOKUP', 2, 140, 'L_BA0003', 1, NULL);

IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_CUST' AND column_nm = 'class_nm')
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, lookup_proc_nm, sort, width, visible_yn)
    VALUES ('P_CUST', 'class_nm', N'거래처분류', 'TEXT', NULL, 10, 130, 'Y');
GO
