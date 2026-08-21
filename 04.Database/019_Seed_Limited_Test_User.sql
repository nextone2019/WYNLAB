/* =========================================================
   서버측 메뉴 권한 검증(RequireMenuPermissionAttribute) 테스트/회귀용 저권한 계정.
   그룹 소속도 없고 TSMMENUAUTH에 아무 권한행도 없어서, 로그인은 되지만 어떤 메뉴에도
   조회/등록/수정/삭제 권한이 없다 - "로그인만 되면 아무 API나 호출 가능하던" 문제가
   실제로 막히는지 확인할 때 이 계정으로 시도해보면 된다.
   비밀번호: 1234 (관리자 시드 계정과 동일한 해시 재사용)
   ========================================================= */

INSERT INTO TSMUSER (USER_ID, USER_NM, PASSWORD_HASH, IS_ADMIN_YN, USE_YN) VALUES
('test_noauth', N'권한없음테스트', '$2b$12$.nz4wt9UXNm7TKG8mtNJFO575P8rpqc4PPWZ.jsuLvROzcyuxCaXG', 'N', 'Y');
