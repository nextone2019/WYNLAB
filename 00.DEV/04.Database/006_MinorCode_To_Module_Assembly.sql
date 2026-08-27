-- frmMinorCode를 화면 단위 어셈블리(WYNLAB.SM.frmMinorCode.dll)에서 모듈 단위 어셈블리
-- (WYNLAB.SM.dll)로 옮긴 첫 시범 사례. 배경은 이 세션의 논의 참고 - 화면이 수백 개로 늘면
-- 화면당 어셈블리는 배포 스크립트가 화면 수만큼 빌드를 돌아야 하고 배포 파일도 그만큼 늘어나서,
-- 모듈(SM/SA/PR/BA/MA 등) 단위로 하나의 어셈블리에 묶고 서브모듈은 폴더로만 구분하기로 했다.
--
-- 네임스페이스도 WYNLAB.SM으로 평평해져서(서브모듈 폴더는 네임스페이스에 반영하지 않음),
-- FORM_CLASS_NM의 타입 전체 이름도 짧아진다.
--   이전: WYNLAB.SM.frmMinorCode.frmMinorCode, WYNLAB.SM.frmMinorCode
--   이후: WYNLAB.SM.frmMinorCode,               WYNLAB.SM

UPDATE TSMMENU
   SET FORM_CLASS_NM = 'WYNLAB.SM.frmMinorCode, WYNLAB.SM'
 WHERE MENU_CD = 'SM_MINOR_CODE'
   AND FORM_CLASS_NM <> 'WYNLAB.SM.frmMinorCode, WYNLAB.SM';
GO

SELECT MENU_CD, FORM_CLASS_NM FROM TSMMENU WHERE MENU_CD = 'SM_MINOR_CODE';
GO
