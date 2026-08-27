-- SSP_CBO_CODE_Q의 파라미터 이름을 @p_major_code로 통일한다. 로컬 개발DB에는 @p_code로,
-- 운영DB에는 @p_where로 서로 다르게 올라가 있었다(둘 다 실제로 겪음 - 로컬은 @p_code로 확인,
-- 운영은 API가 @p_code를 보냈는데 "매개 변수 '@p_where'이(가) 필요하지만 제공되지 않았습니다"
-- SqlException으로 확인됨). 이 프로시저는 LookUpEditWyn.ProcName/Where를 통해 화면 어디서든
-- 공용으로 부르는 콤보 조회라(WYNLAB.BaseForm.ControlDataSources 참고), 파라미터 이름이
-- 환경마다 다르면 그때그때 API 쪽 파라미터 이름도 맞춰 바꿔야 해서 혼란스럽다 - 값의 실제
-- 의미(대분류코드)를 그대로 드러내는 이름 하나로 정리한다.

CREATE OR ALTER PROCEDURE [dbo].[SSP_CBO_CODE_Q]
    @p_major_code VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT minor_cd, minor_nm
    FROM TSMMINOR
    WHERE major_cd = @p_major_code
      AND use_yn = 'Y'
    ORDER BY sort;
END
GO
