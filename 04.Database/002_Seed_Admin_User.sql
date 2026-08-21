/* =========================================================
   테스트용 관리자 계정 시드
   비밀번호: 1234  (BCrypt 해시 - 반드시 최초 로그인 후 변경할 것)
   ========================================================= */

INSERT INTO TSMUSER (USER_ID, USER_NM, PASSWORD_HASH, IS_ADMIN_YN, USE_YN) VALUES
('admin', N'시스템관리자', '$2b$12$.nz4wt9UXNm7TKG8mtNJFO575P8rpqc4PPWZ.jsuLvROzcyuxCaXG', 'Y', 'Y');

/* 참고: BCrypt.Net-Next(.NET) 와 python bcrypt는 동일한 $2b$ 포맷을 사용하므로
   위 해시값 그대로 API의 BCrypt.Net.BCrypt.Verify() 검증을 통과한다.
   운영 계정을 추가로 만들 때는 아래처럼 회원가입/관리자화면에서
   BCrypt.Net.BCrypt.HashPassword(평문비밀번호) 로 생성해서 넣는다. */
