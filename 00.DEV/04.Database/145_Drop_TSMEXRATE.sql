/* 144번에서 만든 TSMEXRATE는 API 연동 프레임워크를 처음 만들 때 쓴 예시용 테이블이었다 -
   실제 업무에서 쓸 환율 테이블은 BA 담당자가 TBAEXCRATE로 따로 설계/생성했으므로(2026-09-15),
   EximFxRateIntegration.cs가 이제 TBAEXCRATE에 저장하도록 바뀌었다. TSMEXRATE는 아무 데이터도
   없는 채로 안 쓰이게 됐으니 그대로 제거한다. */

IF OBJECT_ID('TSMEXRATE') IS NOT NULL
    DROP TABLE TSMEXRATE;
GO
