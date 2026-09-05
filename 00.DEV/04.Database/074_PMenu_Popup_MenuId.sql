/* =========================================================================
   073(TSMMENU MENU_CD->MENU_ID)에서 SSP_POP_MENU_Q의 반환 컬럼이
   menu_cd/upper_menu_cd -> menu_id/upper_menu_id로 바뀌었는데, P_MENU 팝업
   설정(sysPopUpM.key_field/parent_field, sysPopUpD 표시컬럼)은 그대로 남아있어서
   "이 팝업의 키 컬럼(menu_cd) 값을 찾을 수 없습니다" 오류가 났다(2026-09-04,
   frmMenu 상위메뉴 선택 팝업에서 실제 발견). 팝업 설정을 새 컬럼명으로 맞춘다.
   ========================================================================= */

UPDATE sysPopUpM
SET key_field = 'menu_id', parent_field = 'upper_menu_id'
WHERE popup_key = 'P_MENU';

UPDATE sysPopUpD
SET column_nm = 'menu_id', caption = N'메뉴ID'
WHERE popup_key = 'P_MENU' AND column_nm = 'menu_cd';
GO
