-- 범용 데이터 통로(api/data/query, api/data/save)를 위한 메뉴별 프로시저 접두사.
-- 설계 배경은 저장소 루트의 GENERIC_DATA_API.md 참고.
--
-- 왜 이 컬럼이 필요한가: 범용 통로는 화면이 프로시저 이름을 지정해서 호출하므로, 아무 프로시저나
-- 실행되지 않도록 화이트리스트가 있어야 한다. 그 목록을 서버 코드에 두면 "화면 추가 = 서버 배포"가
-- 그대로 남아 범용화의 의미가 없어지므로, 메뉴 정보와 같은 자리(DB)에 둔다.
-- 이미 FORM_CLASS_NM으로 "이 메뉴가 어떤 화면 클래스를 여는가"를 DB에 두고 있는 것과 같은 발상이다.
--
-- 접두사인 이유: 한 화면이 조회(_Q)/저장(_S)/그리드저장(_S_1)을 모두 쓰므로, 이름 하나가 아니라
-- 공통 접두사로 묶어야 지금 프로시저 명명규칙(USP_{모듈}_{화면}_{동작})이 그대로 통한다.
--
-- NULL이면 그 메뉴는 범용 통로를 못 쓴다(기존 화면별 API만 사용). 화면을 하나씩 옮기는 동안
-- 두 방식이 공존할 수 있어야 해서 NOT NULL로 두지 않는다.

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('TSMMENU') AND name = 'PROC_PREFIX')
BEGIN
    ALTER TABLE TSMMENU ADD PROC_PREFIX VARCHAR(100) NULL;
END
GO

-- 기초코드등록: 범용 통로 시범 전환 대상
UPDATE TSMMENU
   SET PROC_PREFIX = 'USP_SM_MINORCODE_'
 WHERE MENU_CD = 'SM_MINOR_CODE'
   AND (PROC_PREFIX IS NULL OR PROC_PREFIX <> 'USP_SM_MINORCODE_');
GO

SELECT MENU_CD, MENU_NM, PROC_PREFIX FROM TSMMENU WHERE PROC_PREFIX IS NOT NULL;
GO
