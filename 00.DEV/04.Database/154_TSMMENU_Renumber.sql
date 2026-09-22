-- TSMMENU MENU_ID 재정리 (2026-09-16, 사장님 지시).
--
-- 배경: 52번 다음 메뉴가 10052로 튀어있었다 - 원인은 _TSMMENU_Data.sql(예전 SSMS 데이터 스냅샷)
-- 안에 SET IDENTITY_INSERT ON 상태로 MENU_ID=10052를 직접 박아넣는 INSERT문이 있었고, 이게
-- 2026-09-08에 한 번 실행되면서 IDENTITY 채번 기준이 튀어버린 것(AI Builder의 메뉴 생성 코드
-- 자체는 IDENTITY_INSERT를 전혀 안 쓰는 정상 코드였음 - 확인 완료).
--
-- 이 마이그레이션은 MENU_ID/UPPER_MENU_ID만 정리한다: 지금 있는 61개 행을 현재 MENU_ID 순서
-- 그대로(구조/관계 그대로 유지, 빈 번호만 제거) 1~61로 다시 번호를 매긴다. TSMMENUAUTH(메뉴별
-- 권한부여, 37건)는 MENU_ID가 전부 바뀌므로 통째로 비운다(사장님 지시) - 재로그인 후 필요한
-- 메뉴 권한은 frmMenuAuth에서 다시 설정해야 한다.
--
-- MENU_ID는 IDENTITY라 UPDATE로 값을 못 바꾼다 - 그래서 1) 새 번호를 먼저 다 계산해두고,
-- 2) 기존 테이블을 비운 뒤, 3) SET IDENTITY_INSERT로 새 번호 그대로 다시 채워 넣는 방식을 쓴다.
-- 적용 전 원본을 TSMMENU_OLD_20260916/TSMMENUAUTH_OLD_20260916으로 그대로 백업해둔다(다른
-- *_OLD_* 백업 테이블들과 같은 관례).

BEGIN TRANSACTION;

IF OBJECT_ID('TSMMENU_OLD_20260916') IS NOT NULL DROP TABLE TSMMENU_OLD_20260916;
SELECT * INTO TSMMENU_OLD_20260916 FROM TSMMENU;

IF OBJECT_ID('TSMMENUAUTH_OLD_20260916') IS NOT NULL DROP TABLE TSMMENUAUTH_OLD_20260916;
SELECT * INTO TSMMENUAUTH_OLD_20260916 FROM TSMMENUAUTH;

-- 1) 새 번호 매핑 - 현재 MENU_ID 오름차순 그대로, 번호만 1부터 빈틈없이.
CREATE TABLE #map (OldId BIGINT PRIMARY KEY, NewId BIGINT);
INSERT INTO #map (OldId, NewId)
SELECT MENU_ID, ROW_NUMBER() OVER (ORDER BY MENU_ID)
FROM TSMMENU;

-- 2) 새 번호를 반영한 전체 데이터를 임시로 담아둔다(원본을 비우기 전에 미리 계산).
SELECT
    m.NewId AS MENU_ID,
    t.MENU_NM,
    pm.NewId AS UPPER_MENU_ID,
    t.MENU_LEVEL, t.MENU_TYPE, t.MODULE, t.SCREEN_CLASS_NM, t.ICON_NM, t.PROC_PREFIX,
    t.SORT_ORDER, t.USE_YN,
    t.AUTH01_NM, t.AUTH02_NM, t.AUTH03_NM, t.AUTH04_NM, t.AUTH05_NM,
    t.AUTH06_NM, t.AUTH07_NM, t.AUTH08_NM, t.AUTH09_NM, t.AUTH10_NM,
    t.reg_user_id, t.reg_dt, t.reg_pc, t.upt_user_id, t.upt_dt, t.upt_pc
INTO #newMenu
FROM TSMMENU t
JOIN #map m ON m.OldId = t.MENU_ID
LEFT JOIN #map pm ON pm.OldId = t.UPPER_MENU_ID;

-- 3) 메뉴별 권한부여(사용자/사용자그룹 대상 전부 - AUTH_TARGET_TYPE으로 구분) 삭제.
DELETE FROM TSMMENUAUTH;

-- 4) 기존 TSMMENU를 비우고 새 번호로 다시 채운다.
TRUNCATE TABLE TSMMENU;

SET IDENTITY_INSERT TSMMENU ON;
INSERT INTO TSMMENU (
    MENU_ID, MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, ICON_NM, PROC_PREFIX,
    SORT_ORDER, USE_YN,
    AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
)
SELECT
    MENU_ID, MENU_NM, UPPER_MENU_ID, MENU_LEVEL, MENU_TYPE, MODULE, SCREEN_CLASS_NM, ICON_NM, PROC_PREFIX,
    SORT_ORDER, USE_YN,
    AUTH01_NM, AUTH02_NM, AUTH03_NM, AUTH04_NM, AUTH05_NM, AUTH06_NM, AUTH07_NM, AUTH08_NM, AUTH09_NM, AUTH10_NM,
    reg_user_id, reg_dt, reg_pc, upt_user_id, upt_dt, upt_pc
FROM #newMenu
ORDER BY MENU_ID;
SET IDENTITY_INSERT TSMMENU OFF;

DROP TABLE #map;
DROP TABLE #newMenu;

COMMIT TRANSACTION;
