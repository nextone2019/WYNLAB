/* ---------- 사이트 환경설정 메뉴 재배치 ----------
   Developer Tool(39) 바로 아래 리프였던 걸 Configuration 서브그룹으로 옮긴다
   (Component관리/AI Builder와 같은 레벨의 그룹 하나 새로 만들고, 그 아래 리프로 이동).
   기존 행(SCREEN_CLASS_NM='frmSiteConfig')은 지우지 않고 UPPER_MENU_ID/MENU_LEVEL/MENU_NM만
   갱신한다 - 이미 살아있는 MENU_ID를 그대로 유지해야 나중에 권한(TSMMENUAUTH)을 다시
   부여할 필요가 없다. */

DECLARE @configGroupId bigint;

INSERT INTO TSMMENU (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, PROC_PREFIX, SORT_ORDER, USE_YN, reg_user_id, reg_dt)
VALUES (N'Configuration', 39, 2, 'GROUP', NULL, NULL, NULL, 40, 'Y', 'admin', GETDATE());

SET @configGroupId = SCOPE_IDENTITY();

UPDATE TSMMENU
SET MENU_NM = N'Configuration설정',
    UPPER_MENU_ID = @configGroupId,
    MENU_LEVEL = 3,
    SORT_ORDER = 10,
    upt_user_id = 'admin',
    upt_dt = GETDATE()
WHERE SCREEN_CLASS_NM = 'frmSiteConfig';
GO
