-- 구매입고/재고/수불 화면 메뉴 등록 + 자동 입고 옵션 개방 (2026-09-25).
--  * 구매입고등록(frmGr)/구매입고현황(frmGrList): 자재관리(10) > 구매발주관리(12) 밑, 수입검사현황(60)/구매진행현황(70) 다음.
--  * 재고관리 그룹을 자재관리(10) 밑에 새로 만들고 재고현황(frmStockList)/수불현황(frmTransList)을 둔다.
--  * MA0009(구매 입고방식)의 "자동(A)"을 사용으로 바꿔 프로세스 설정 화면 선택지에 나타나게 한다 - 입고 프로시저(199번)가 완성됐다.
-- 기본 권한은 부여하지 않는다(메뉴권한 화면에서 부여, 관리자 계정은 우회).

DECLARE @grp BIGINT;
SELECT @grp = MENU_ID FROM TSMMENU WHERE MENU_NM = N'재고관리' AND UPPER_MENU_ID = 10;
IF @grp IS NULL
BEGIN
    INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
    VALUES (N'재고관리', 10, 2, 'GROUP', 40, 'Y', SUSER_SNAME(), GETDATE());
    SET @grp = SCOPE_IDENTITY();
END

DECLARE @screens TABLE (nm NVARCHAR(50), cls VARCHAR(50), parent BIGINT, sort INT);
INSERT INTO @screens VALUES
    (N'구매입고등록', 'frmGr',        12,   80),
    (N'구매입고현황', 'frmGrList',    12,   90),
    (N'재고현황',     'frmStockList', @grp, 10),
    (N'수불현황',     'frmTransList', @grp, 20);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT s.nm, s.parent, 3, 'FORM', 'MA', s.cls, 'USP_MA_', s.sort, 'Y', SUSER_SNAME(), GETDATE()
FROM @screens s
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.MODULE = 'MA' AND m.SCREEN_CLASS_NM = s.cls);

UPDATE TSMMINOR SET use_yn = 'Y' WHERE major_cd = 'MA0009' AND minor_cd = 'A';
GO
