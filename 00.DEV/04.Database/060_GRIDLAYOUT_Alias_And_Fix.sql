-- 두 가지를 고친다.
--
-- 1) USP_SM_GRIDLAYOUT_Q가 GRID_KEY/LAYOUT_XML을 별칭 없이 그대로 반환하고 있었는데, 이 API를
--    059 설계 그대로 범용 데이터 통로(api/data/*)로 부르면 DataController가 "그 메뉴에 등록된
--    프로시저 접두사로 시작하는가"를 검사해서 매번 막힌다(모든 화면의 PROC_PREFIX가
--    USP_SM_GRIDLAYOUT_이 아니므로) - 실제로 기초코드등록에서 "이 메뉴에서 사용할 수 없는
--    프로시저입니다" 오류로 확인됨. 그리드 레이아웃은 애초에 특정 화면 소유 데이터가 아니라
--    로그인만 하면 누구나 쓸 수 있어야 하는 공통 기능이라, 전용 컨트롤러(api/grid-layout,
--    GridLayoutController)로 분리한다 - 그쪽은 Dapper로 이 프로시저를 직접 호출한다.
--
-- 2) 그 김에 Dapper가 DTO에 바로 매핑할 수 있도록 컬럼을 PascalCase로 별칭 처리한다 -
--    이 코드베이스의 SM 계열 프로시저(USP_SM_USERAUTH_Q 등)가 이미 쓰는 관례와 맞춘다.
CREATE OR ALTER PROCEDURE [dbo].[USP_SM_GRIDLAYOUT_Q]
    @p_work_type VARCHAR(50),
    @p_user_id VARCHAR(20) = NULL,
    @p_menu_cd VARCHAR(20) = NULL,
    @GeneratedCode VARCHAR(50) = NULL OUTPUT,
    @ReturnCode INT = 0 OUTPUT,
    @ReturnMsg NVARCHAR(200) = NULL OUTPUT,
    @ErrorCode INT = 0 OUTPUT,
    @ErrorMsg NVARCHAR(500) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ReturnCode = 0; SET @ReturnMsg = N'성공'; SET @ErrorCode = 0; SET @ErrorMsg = NULL;

    BEGIN TRY
        IF @p_work_type = 'Q'
        BEGIN
            SELECT GRID_KEY AS GridKey, LAYOUT_XML AS LayoutXml
            FROM TSMUSERGRIDLAYOUT
            WHERE USER_ID = @p_user_id AND MENU_CD = @p_menu_cd;
        END
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO
