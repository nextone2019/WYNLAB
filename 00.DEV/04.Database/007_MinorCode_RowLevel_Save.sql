-- USP_SM_MINORCODE_S_1을 "그리드 전체 삭제 후 재삽입"에서 행 단위 CRUD(N/U/D)로 바꾼다.
--
-- 이 프로시저는 USP_SM_MINORCODE_S(대분류)와 달리 한 번에 여러 행(그리드 전체)을 받는다.
-- 그래서 p_work_type을 프로시저 파라미터로 받는 대신, @p_items_json의 각 행 안에
-- work_type(N/U/D)을 담아서 온다 - 화면에서 그리드에 몇 개를 새로 추가하고 몇 개를 지우고
-- 몇 개를 고쳤어도 저장 버튼 한 번, 트랜잭션 하나로 다 처리해야 하기 때문이다. 그 외
-- 구조(TRY/CATCH, 트랜잭션, p_ 파라미터, 표준 출력)는 USP_SM_MINORCODE_S와 동일하다.
--
-- [예전 방식과 달라지는 점] 예전(005)엔 저장할 때마다 이 대분류의 소분류를 전부 지우고
-- 화면이 보낸 목록을 통째로 다시 넣었다. 그러면 안 바뀐 행도 매번 새로 INSERT되어
-- reg_dt/reg_user_id가 저장할 때마다 갱신되고(원래는 "최초 등록 시각"이어야 함), 그리드에
-- 노출 안 하는 컬럼(rel_cd1~10, sys_yn)은 매번 기본값으로 리셋됐다. 행 단위로 바뀌면
-- 실제로 손댄 행만 INSERT/UPDATE/DELETE되어 이 문제가 없어진다.
--
-- [처리 순서] 삭제 -> 수정 -> 신규 순으로 한다. 삭제를 먼저 해야, 방금 지운 코드를 다른
-- 행이 새 코드로 재사용해도(그리드에서 지우고 같은 코드로 새로 추가하는 경우) PK 충돌이
-- 안 난다.

CREATE OR ALTER PROCEDURE USP_SM_MINORCODE_S_1
    @p_major_cd VARCHAR(20),
    @p_items_json NVARCHAR(MAX),
    @p_user_id VARCHAR(50),
    @p_client_pc NVARCHAR(200) = NULL,
    @GeneratedCode VARCHAR(20) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1) 삭제(D) - work_type='D'인 행은 minor_cd(삭제 시점 원래 코드)만 갖고 온다.
        DELETE M
        FROM TSMMINOR M
        JOIN OPENJSON(@p_items_json) WITH (
            work_type VARCHAR(1)   '$.work_type',
            minor_cd  VARCHAR(100) '$.minor_cd'
        ) J ON M.major_cd = @p_major_cd AND M.minor_cd = J.minor_cd
        WHERE J.work_type = 'D';

        -- 2) 수정(U) - orig_minor_cd(수정 전 코드)로 대상 행을 찾고, minor_cd를 포함한 값을
        --    현재 화면 값으로 덮어쓴다. 소분류코드 자체를 그리드에서 고쳤어도(편집 가능한
        --    컬럼) orig_minor_cd로 WHERE를 매칭하고 minor_cd는 SET으로 새 값이 들어가서
        --    정상 처리된다. 그리드에 없는 컬럼(rel_cd1~10, sys_yn)은 건드리지 않는다 -
        --    화면이 모르는 값을 매번 기본값으로 되돌리지 않기 위함이다.
        UPDATE M SET
            minor_cd = J.minor_cd,
            minor_nm = J.minor_nm,
            sort     = ISNULL(J.sort, 0),
            use_yn   = ISNULL(J.use_yn, 'Y'),
            remark   = J.remark,
            upt_user_id = @p_user_id, upt_dt = GETDATE(), upt_pc = @p_client_pc
        FROM TSMMINOR M
        JOIN OPENJSON(@p_items_json) WITH (
            work_type     VARCHAR(1)     '$.work_type',
            orig_minor_cd VARCHAR(100)   '$.orig_minor_cd',
            minor_cd      VARCHAR(100)   '$.minor_cd',
            minor_nm      NVARCHAR(200)  '$.minor_nm',
            sort          INT            '$.sort',
            use_yn        VARCHAR(1)     '$.use_yn',
            remark        NVARCHAR(3000) '$.remark'
        ) J ON M.major_cd = @p_major_cd AND M.minor_cd = J.orig_minor_cd
        WHERE J.work_type = 'U'
          AND NULLIF(LTRIM(RTRIM(J.minor_cd)), '') IS NOT NULL;

        -- 3) 신규(N) - 그리드에서 행만 추가하고 코드를 안 채운 빈 행은 걸러낸다.
        INSERT INTO TSMMINOR (
            major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn,
            remark, reg_user_id, reg_dt, reg_pc
        )
        SELECT
            @p_major_cd, J.minor_cd, J.minor_nm, ISNULL(J.sort, 0), 'N', ISNULL(J.use_yn, 'Y'),
            J.remark, @p_user_id, GETDATE(), @p_client_pc
        FROM OPENJSON(@p_items_json) WITH (
            work_type VARCHAR(1)     '$.work_type',
            minor_cd  VARCHAR(100)   '$.minor_cd',
            minor_nm  NVARCHAR(200)  '$.minor_nm',
            sort      INT            '$.sort',
            use_yn    VARCHAR(1)     '$.use_yn',
            remark    NVARCHAR(3000) '$.remark'
        ) J
        WHERE J.work_type = 'N'
          AND NULLIF(LTRIM(RTRIM(J.minor_cd)), '') IS NOT NULL;

        COMMIT TRANSACTION;
        SET @GeneratedCode = @p_major_cd;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
