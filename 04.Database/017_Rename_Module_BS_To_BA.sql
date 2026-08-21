/* =========================================================
   기준정보관리 모듈코드를 BS -> BA로 통일 (DLL 명명 규칙 SM/BA/SA/PR과 맞춤).
   'BS'/'BS_%' 로 시작하는 모든 MENU_CD와, 그걸 가리키는 UPPER_MENU_CD를 전부
   'BA'/'BA_%'로 바꾼다(예: BS -> BA, BS_ORG -> BA_ORG, BS_ORG_DEPT -> BA_ORG_DEPT).
   REPLACE는 문자열 내 모든 일치를 바꾸지만, 이 코드들은 전부 'BS'가 접두어로 한 번만
   나오는 형태라 안전하다. TSMMENUAUTH에는 BS 계열 권한행이 없는 것을 사전에 확인했다.
   ========================================================= */

UPDATE TSMMENU SET MENU_CD = REPLACE(MENU_CD, 'BS', 'BA')
    WHERE MENU_CD = 'BS' OR MENU_CD LIKE 'BS\_%' ESCAPE '\';

UPDATE TSMMENU SET UPPER_MENU_CD = REPLACE(UPPER_MENU_CD, 'BS', 'BA')
    WHERE UPPER_MENU_CD = 'BS' OR UPPER_MENU_CD LIKE 'BS\_%' ESCAPE '\';
