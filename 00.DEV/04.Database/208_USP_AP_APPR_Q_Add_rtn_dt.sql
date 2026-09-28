-- 전자결재 창 오픈 오류 수정(2026-09-25): api/approvals/history가 "KeyNotFoundException: The given key 'rtn_dt'"로 500을 냈다.
-- ApprovalsController.MapHeader는 헤더 행에서 rtn_dt(반려일시)를 읽는데, 180번에서 다시 만든 USP_AP_APPR_Q의 'Q'(문서 1건의 전체이력) 헤더
-- SELECT에 a.rtn_dt가 빠져 있었다(a.rtn_yn만 있음). TAPDOC.rtn_dt 컬럼은 그대로 있다.
-- 라이브 정의를 통째로 다시 쓰지 않고, 그 정의에서 해당 SELECT 한 줄만 바꿔 다시 만든다(주석/나머지 로직 보존, 이미 고쳐졌으면 아무것도 안 함).

DECLARE @def NVARCHAR(MAX) = OBJECT_DEFINITION(OBJECT_ID('dbo.USP_AP_APPR_Q'));
DECLARE @old NVARCHAR(200) = N'a.rtn_yn, a.app_stat_cd AS stat_cd, a.emp_id, e.emp_nm';
DECLARE @new NVARCHAR(200) = N'a.rtn_yn, a.rtn_dt, a.app_stat_cd AS stat_cd, a.emp_id, e.emp_nm';

IF @def IS NOT NULL AND CHARINDEX(@old, @def) > 0
BEGIN
    SET @def = REPLACE(@def, @old, @new);
    -- CREATE로 시작하는 정의의 첫 CREATE를 ALTER로(공백 개수가 정의마다 달라서 패턴으로 찾는다)
    DECLARE @pos INT = PATINDEX('%CREATE%PROCEDURE%', @def);
    SET @def = STUFF(@def, @pos, LEN(N'CREATE'), N'ALTER');
    EXEC (@def);
    PRINT N'USP_AP_APPR_Q: rtn_dt 추가 완료';
END
ELSE
    PRINT N'USP_AP_APPR_Q: 변경할 부분이 없음(이미 반영됨)';
GO
