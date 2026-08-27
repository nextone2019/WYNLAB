-- USP_SM_MINORCODE_S_1 수정 - 세 가지를 함께 고친다.
--
-- ① [데이터 유실 버그] OPENJSON이 읽는 속성명이 클라이언트가 보내는 것과 달랐다.
--    프로시저는 '$.MinorCd'(PascalCase)로 읽는데 클라이언트는 minor_cd(snake_case)로 보내서,
--    모든 값이 NULL로 읽히고 minor_cd가 NOT NULL이라 INSERT가 오류 515로 실패했다.
--    그런데 DELETE가 INSERT보다 먼저 실행되고 트랜잭션이 없어서, 삭제만 남고 삽입은 안 되는
--    상태가 커밋됐다 - 소분류가 있는 대분류에서 저장을 누르면 소분류가 전부 사라졌다.
--    (2026-08-26 로컬 개발DB에서 재현 확인: return_code -1 / error_code 515, 저장된 행 0건)
--
-- ② [트랜잭션] ①을 고쳐도 다른 이유로 INSERT가 실패하면 같은 유실이 반복된다.
--    DELETE + INSERT를 한 트랜잭션으로 묶어서, 실패하면 삭제까지 되돌린다.
--
-- ③ [파라미터 규약 통일] 범용 데이터 통로(api/data/save)는 파라미터가 p_로 시작하고 표준
--    출력이 PascalCase인 새 규약만 취급한다. 이 프로시저만 예전 규약(@major_cd, @return_code)을
--    쓰고 있어서 맞춘다. USP_SM_MINORCODE_Q / _S 는 이미 새 규약이다.
--    설계 배경은 저장소 루트의 GENERIC_DATA_API.md 참고.

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

        DELETE FROM TSMMINOR WHERE major_cd = @p_major_cd;

        INSERT INTO TSMMINOR (
            major_cd, minor_cd, minor_nm, sort, sys_yn, use_yn,
            rel_cd1, rel_cd2, rel_cd3, rel_cd4, rel_cd5,
            rel_cd6, rel_cd7, rel_cd8, rel_cd9, rel_cd10,
            remark, reg_user_id, reg_dt, reg_pc
        )
        SELECT
            @p_major_cd, J.minor_cd, J.minor_nm, ISNULL(J.sort, 0),
            ISNULL(J.sys_yn, 'N'), ISNULL(J.use_yn, 'Y'),
            J.rel_cd1, J.rel_cd2, J.rel_cd3, J.rel_cd4, J.rel_cd5,
            J.rel_cd6, J.rel_cd7, J.rel_cd8, J.rel_cd9, J.rel_cd10,
            J.remark, @p_user_id, GETDATE(), @p_client_pc
        FROM OPENJSON(@p_items_json)
        WITH (
            minor_cd  VARCHAR(100)   '$.minor_cd',
            minor_nm  NVARCHAR(200)  '$.minor_nm',
            sort      INT            '$.sort',
            sys_yn    VARCHAR(1)     '$.sys_yn',
            use_yn    VARCHAR(1)     '$.use_yn',
            rel_cd1   VARCHAR(50)    '$.rel_cd1',
            rel_cd2   VARCHAR(50)    '$.rel_cd2',
            rel_cd3   VARCHAR(50)    '$.rel_cd3',
            rel_cd4   VARCHAR(50)    '$.rel_cd4',
            rel_cd5   VARCHAR(50)    '$.rel_cd5',
            rel_cd6   VARCHAR(50)    '$.rel_cd6',
            rel_cd7   VARCHAR(50)    '$.rel_cd7',
            rel_cd8   VARCHAR(50)    '$.rel_cd8',
            rel_cd9   VARCHAR(50)    '$.rel_cd9',
            rel_cd10  VARCHAR(50)    '$.rel_cd10',
            remark    NVARCHAR(3000) '$.remark'
        ) J
        -- 그리드에서 행만 추가하고 코드를 안 적은 빈 행이 섞여 들어와도 저장이 통째로
        -- 실패하지 않도록 걸러낸다.
        WHERE NULLIF(LTRIM(RTRIM(J.minor_cd)), '') IS NOT NULL;

        COMMIT TRANSACTION;

        SET @GeneratedCode = @p_major_cd;
    END TRY
    BEGIN CATCH
        -- 트랜잭션이 살아있으면 되돌린다 - 이게 없으면 DELETE만 적용된 상태가 남는다(위 ② 참고).
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;

        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
