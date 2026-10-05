-- 영업관리 메뉴 개편 (2026-10-05): 거래명세서관리 / 출고관리(구 출하관리) / 매출관리
--  거래명세서관리(신규 그룹): 거래명세서등록(frmInvc), 거래명세서현황(frmInvcList)
--  출고관리(기존 출하관리 그룹 이름 변경): 출고등록(frmGi), 출고현황(frmGiList)
--  매출관리(신규 그룹): 매출등록(frmBill), 매출현황(frmBillList)
-- 신규 화면 권한은 출고등록(frmGi)과 같은 대상에게 똑같이 준다.
-- *** 화면(WYNLAB.SA.dll)이 배포된 뒤에 적용한다 - 먼저 적용하면 메뉴를 열 때 화면 클래스를 못 찾는다. ***
-- 여러 번 실행해도 안전하다. 적용 대상: WYNLAB_DEV, FADU 양쪽 DB.

SET NOCOUNT ON;

DECLARE @giGrp BIGINT = (SELECT UPPER_MENU_ID FROM TSMMENU WHERE SCREEN_CLASS_NM = 'frmGi');
DECLARE @sales BIGINT = (SELECT UPPER_MENU_ID FROM TSMMENU WHERE MENU_ID = @giGrp);
DECLARE @tpl BIGINT = (SELECT MENU_ID FROM TSMMENU WHERE SCREEN_CLASS_NM = 'frmGi');
DECLARE @invGrp BIGINT, @billGrp BIGINT;

-- 이름 변경 (출하 -> 출고)
UPDATE TSMMENU SET MENU_NM = N'출고관리', upt_user_id = 'wynlab', upt_dt = GETDATE() WHERE MENU_ID = @giGrp AND MENU_NM <> N'출고관리';
UPDATE TSMMENU SET MENU_NM = N'출고등록', upt_user_id = 'wynlab', upt_dt = GETDATE() WHERE SCREEN_CLASS_NM = 'frmGi' AND MENU_NM <> N'출고등록';
UPDATE TSMMENU SET MENU_NM = N'출고현황', upt_user_id = 'wynlab', upt_dt = GETDATE() WHERE SCREEN_CLASS_NM = 'frmGiList' AND MENU_NM <> N'출고현황';

-- 그룹: 수주관리(20) -> 거래명세서관리(25) -> 출고관리(30) -> 매출관리(40)
SELECT @invGrp = MENU_ID FROM TSMMENU WHERE UPPER_MENU_ID = @sales AND MENU_TYPE = 'GROUP' AND MENU_NM = N'거래명세서관리';
IF @invGrp IS NULL
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'거래명세서관리', @sales, 2, 'GROUP', NULL, 25, 'Y', 'wynlab', GETDATE());
    SET @invGrp = SCOPE_IDENTITY();
END
SELECT @billGrp = MENU_ID FROM TSMMENU WHERE UPPER_MENU_ID = @sales AND MENU_TYPE = 'GROUP' AND MENU_NM = N'매출관리';
IF @billGrp IS NULL
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'매출관리', @sales, 2, 'GROUP', NULL, 40, 'Y', 'wynlab', GETDATE());
    SET @billGrp = SCOPE_IDENTITY();
END

DECLARE @items TABLE (cls VARCHAR(100), nm NVARCHAR(100), grp BIGINT, so INT);
INSERT INTO @items VALUES ('frmInvc', N'거래명세서등록', @invGrp, 10), ('frmInvcList', N'거래명세서현황', @invGrp, 20),
                          ('frmBill', N'매출등록', @billGrp, 10), ('frmBillList', N'매출현황', @billGrp, 20);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT i.nm, i.grp, 3, 'FORM', 'SA', i.cls, 'USP_SA_', i.so, 'Y', 'wynlab', GETDATE()
FROM @items i
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.SCREEN_CLASS_NM = i.cls);

INSERT INTO TSMMENUAUTH (MENU_ID, AUTH_TARGET_TYPE, AUTH_TARGET_CD, VIEW_YN, INSERT_YN, DELETE_YN, SAVE_YN, PRINT_YN, EXCEL_YN,
                         AUTH01, AUTH02, AUTH03, AUTH04, AUTH05, AUTH06, AUTH07, AUTH08, AUTH09, AUTH10, REG_USER_ID, REG_DT)
SELECT m.MENU_ID, a.AUTH_TARGET_TYPE, a.AUTH_TARGET_CD, a.VIEW_YN, a.INSERT_YN, a.DELETE_YN, a.SAVE_YN, a.PRINT_YN, a.EXCEL_YN,
       a.AUTH01, a.AUTH02, a.AUTH03, a.AUTH04, a.AUTH05, a.AUTH06, a.AUTH07, a.AUTH08, a.AUTH09, a.AUTH10, 'wynlab', GETDATE()
FROM TSMMENU m
JOIN @items i ON i.cls = m.SCREEN_CLASS_NM
JOIN TSMMENUAUTH a ON a.MENU_ID = @tpl
WHERE NOT EXISTS (SELECT 1 FROM TSMMENUAUTH x WHERE x.MENU_ID = m.MENU_ID AND x.AUTH_TARGET_TYPE = a.AUTH_TARGET_TYPE AND x.AUTH_TARGET_CD = a.AUTH_TARGET_CD);

SELECT MENU_ID, MENU_NM, UPPER_MENU_ID, MENU_LEVEL, SORT_ORDER, SCREEN_CLASS_NM FROM TSMMENU WHERE UPPER_MENU_ID = @sales OR UPPER_MENU_ID IN (@invGrp, @billGrp, @giGrp, (SELECT MENU_ID FROM TSMMENU WHERE MENU_NM = N'수주관리' AND UPPER_MENU_ID = @sales)) ORDER BY MENU_LEVEL, UPPER_MENU_ID, SORT_ORDER;
GO
