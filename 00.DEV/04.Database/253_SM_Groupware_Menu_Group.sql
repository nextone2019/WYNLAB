-- 시스템운영관리(25) 바로 아래에 평평하게 놓여 있던 공지사항등록/일정관리/쪽지함 3개 화면을
-- "그룹웨어" 서브그룹 하나로 묶는다(2026-10-03 요청). 화면(FORM) 행의 MENU_ID는 그대로 두고
-- 부모/레벨/정렬만 바꾸므로 TSMMENUAUTH(권한)/TSMUSERFAVORITEMENU(즐겨찾기)는 그대로 유지된다.
-- GROUP 행 자체는 권한행이 필요 없다 - 하위 FORM의 조회권한으로 자동 계산된다
-- (MenuPermissionMerger). 여러 번 실행해도 안전하다(그룹이 이미 있으면 재사용).
-- 적용 대상: WYNLAB_DEV, FADU 양쪽 DB.

SET NOCOUNT ON;

DECLARE @upperMenuId BIGINT = 25;   -- 시스템운영관리
DECLARE @groupId BIGINT;

SELECT @groupId = MENU_ID
  FROM TSMMENU
 WHERE UPPER_MENU_ID = @upperMenuId AND MENU_TYPE = 'GROUP' AND MENU_NM = N'그룹웨어';

IF @groupId IS NULL
BEGIN
    INSERT INTO TSMMENU
        (MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SORT_ORDER, USE_YN,
         reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc)
    VALUES
        (N'그룹웨어', @upperMenuId, 2, 'GROUP', 'SM', 40, 'Y',
         'system', GETDATE(), HOST_NAME(), 'system', GETDATE(), HOST_NAME());

    SET @groupId = SCOPE_IDENTITY();
END

UPDATE TSMMENU
   SET UPPER_MENU_ID = @groupId,
       MENU_LEVEL    = 3,
       SORT_ORDER    = CASE SCREEN_CLASS_NM WHEN 'frmBoard' THEN 10 WHEN 'frmSchedule' THEN 20 WHEN 'frmMessage' THEN 30 END,
       upt_user_id   = 'system',
       upt_dt        = GETDATE(),
       upt_pc        = HOST_NAME()
 WHERE MODULE = 'SM'
   AND MENU_TYPE = 'FORM'
   AND SCREEN_CLASS_NM IN ('frmBoard', 'frmSchedule', 'frmMessage');

SELECT MENU_ID, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, SCREEN_CLASS_NM, MENU_NM, SORT_ORDER
  FROM TSMMENU
 WHERE MENU_ID = @groupId OR UPPER_MENU_ID = @groupId
 ORDER BY MENU_LEVEL, SORT_ORDER;
