-- 납품/수입검사/구매진행현황 화면 메뉴 등록 - 자재관리(10) > 구매발주관리(12) 밑, 구매발주(70)/구매발주현황(71) 다음(2026-09-25).
-- 기본 권한은 부여하지 않는다(메뉴권한 화면에서 부여). 프로시저 접두사는 기존 구매 화면과 같은 USP_MA_.
DECLARE @screens TABLE (nm NVARCHAR(50), cls VARCHAR(50), sort INT);
INSERT INTO @screens VALUES
    (N'납품등록',       'frmDelv',     30),
    (N'납품현황',       'frmDelvList', 40),
    (N'수입검사등록',   'frmIqc',      50),
    (N'수입검사현황',   'frmIqcList',  60),
    (N'구매진행현황',   'frmPoStatus', 70);

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
SELECT s.nm, 12, 3, 'FORM', 'MA', s.cls, 'USP_MA_', s.sort, 'Y', SUSER_SNAME(), GETDATE()
FROM @screens s
WHERE NOT EXISTS (SELECT 1 FROM TSMMENU m WHERE m.MODULE = 'MA' AND m.SCREEN_CLASS_NM = s.cls);
GO
