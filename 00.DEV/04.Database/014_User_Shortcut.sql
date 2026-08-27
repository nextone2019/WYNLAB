-- 사용자별 단축키 설정 기능.
--
-- 대상은 화면(메뉴)이 아니라 상단 공통 툴바의 7개 고정 액션(조회/입력/삭제/행추가/행추가/저장/
-- 출력 - ShellForm.BuildToolbar가 f.QueryClick() 등으로 부르는 BaseForm 표준 액션)이다.
-- 즉 Ctrl+Q를 조회에 지정하면 어느 화면에서 조회하든 항상 Ctrl+Q로 동작한다 - 화면별로
-- 다른 키를 갖지 않는다.
--
-- 기본값(TSMSHORTCUTDEFAULT) + 사용자 재정의(TSMUSERSHORTCUT) 구조로 나눈 이유: 재정의 안 한
-- 사용자/액션은 항상 기본값을 그대로 쓰게 하기 위함이다. 이렇게 하면 나중에 기본 단축키
-- 자체를 바꿔도(TSMSHORTCUTDEFAULT UPDATE 한 번) 재정의 안 한 사용자 전체에 즉시 반영되고,
-- 신규 사용자를 위해 7행을 미리 깔아둘 필요도 없다.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMSHORTCUTDEFAULT')
BEGIN
    CREATE TABLE TSMSHORTCUTDEFAULT (
        action_cd  VARCHAR(20)   NOT NULL,
        action_nm  NVARCHAR(50)  NOT NULL,
        key_combo  VARCHAR(30)   NOT NULL,
        sort_order INT           NOT NULL,
        CONSTRAINT PK_TSMSHORTCUTDEFAULT PRIMARY KEY (action_cd)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TSMUSERSHORTCUT')
BEGIN
    -- 사용자가 기본값을 바꾼 경우에만 행이 생긴다(재정의만 저장) - USP_SM_SHORTCUT_Q가
    -- TSMSHORTCUTDEFAULT와 LEFT JOIN해서 없는 액션은 기본값으로 채운다.
    CREATE TABLE TSMUSERSHORTCUT (
        user_id     VARCHAR(20)  NOT NULL,
        action_cd   VARCHAR(20)  NOT NULL,
        key_combo   VARCHAR(30)  NOT NULL,
        reg_user_id VARCHAR(50)  NULL,
        reg_dt      DATETIME     NOT NULL,
        upt_user_id VARCHAR(50)  NULL,
        upt_dt      DATETIME     NULL,
        CONSTRAINT PK_TSMUSERSHORTCUT PRIMARY KEY (user_id, action_cd)
    );
END
GO

-- 기본 단축키 7종 시드. 예약키(ShellForm의 Ctrl+Tab - DevExpress 문서선택기가 네이티브로
-- 먼저 가로채므로 사용자가 지정해도 실제로는 동작 안 함)는 여기 포함하지 않는다.
-- Ctrl+K는 예전엔 화면검색 포커스 이동용으로 예약돼 있었으나, 사이드바 화면검색 콤보가
-- 항상 보이는 위치로 옮겨가면서 그 예약이 풀렸다(ShellForm.ProcessCmdKey 참고) - 그래서
-- 조회 기본값으로 자유롭게 재사용한다.
MERGE TSMSHORTCUTDEFAULT AS target
USING (VALUES
    ('QUERY',      N'조회',   'Ctrl+Q', 10),
    ('NEW',        N'입력',   'Ctrl+N', 20),
    ('DELETE',     N'삭제',   'Ctrl+D', 30),
    ('ROWADD',     N'행추가', 'Ctrl+I', 40),
    ('ROWDELETE',  N'행삭제', 'Ctrl+Shift+D', 50),
    ('SAVE',       N'저장',   'Ctrl+S', 60),
    ('PRINT',      N'출력',   'Ctrl+P', 70)
) AS src (action_cd, action_nm, key_combo, sort_order)
ON target.action_cd = src.action_cd
WHEN MATCHED THEN
    UPDATE SET action_nm = src.action_nm, key_combo = src.key_combo, sort_order = src.sort_order
WHEN NOT MATCHED THEN
    INSERT (action_cd, action_nm, key_combo, sort_order)
    VALUES (src.action_cd, src.action_nm, src.key_combo, src.sort_order);
GO

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_SHORTCUT_Q]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_user_id VARCHAR(20) = NULL,
    ---------------------------------------------------------------------------------------------------
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
        IF @p_work_type = 'Q'
        BEGIN
            -- 사용자 재정의가 있으면 그 값을, 없으면 기본값을 그대로 내려준다. custom_yn은
            -- 화면이 "초기화" 버튼을 그 행에만 켤지 판단하는 용도.
            SELECT
                d.action_cd,
                d.action_nm,
                COALESCE(u.key_combo, d.key_combo) AS key_combo,
                CASE WHEN u.key_combo IS NULL THEN 'N' ELSE 'Y' END AS custom_yn,
                d.sort_order
            FROM TSMSHORTCUTDEFAULT d
            LEFT JOIN TSMUSERSHORTCUT u
                ON u.action_cd = d.action_cd
               AND u.user_id = @p_user_id
            ORDER BY d.sort_order;
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

CREATE OR ALTER PROCEDURE [dbo].[USP_SM_SHORTCUT_S]
    @p_work_type VARCHAR(50),
    ---------------------------------------------------------------------------------------------------
    @p_action_cd VARCHAR(20),
    @p_key_combo VARCHAR(30) = NULL,
    @p_user_id VARCHAR(20),
    ---------------------------------------------------------------------------------------------------
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
        IF @p_work_type = 'U'
        BEGIN
            -- 이 사용자의 유효 단축키(기본값+재정의 병합) 중, 지금 바꾸려는 액션을 뺀 나머지가
            -- 이미 같은 키를 쓰고 있으면 막는다 - 클라이언트도 저장 전에 같은 검사를 하지만,
            -- 진짜 판단은 여기(서버)다.
            IF EXISTS (
                SELECT 1
                FROM TSMSHORTCUTDEFAULT d
                LEFT JOIN TSMUSERSHORTCUT u
                    ON u.action_cd = d.action_cd
                   AND u.user_id = @p_user_id
                WHERE d.action_cd <> @p_action_cd
                  AND COALESCE(u.key_combo, d.key_combo) = @p_key_combo
            )
            BEGIN
                SET @ReturnCode = -1;
                SET @ReturnMsg = N'이미 다른 동작에 사용 중인 단축키입니다.';
                RETURN;
            END

            MERGE TSMUSERSHORTCUT AS target
            USING (SELECT @p_user_id AS user_id, @p_action_cd AS action_cd) AS src
                ON target.user_id = src.user_id AND target.action_cd = src.action_cd
            WHEN MATCHED THEN
                UPDATE SET key_combo = @p_key_combo, upt_user_id = @p_user_id, upt_dt = GETDATE()
            WHEN NOT MATCHED THEN
                INSERT (user_id, action_cd, key_combo, reg_user_id, reg_dt)
                VALUES (@p_user_id, @p_action_cd, @p_key_combo, @p_user_id, GETDATE());
        END
        ELSE IF @p_work_type = 'D'
        BEGIN
            -- 재정의를 지운다 = 기본값으로 되돌린다(USP_SM_SHORTCUT_Q의 COALESCE가 자동으로 처리).
            DELETE FROM TSMUSERSHORTCUT WHERE user_id = @p_user_id AND action_cd = @p_action_cd;
        END

        SET @GeneratedCode = @p_action_cd;
    END TRY
    BEGIN CATCH
        SET @ReturnCode = -1;
        SET @ReturnMsg = N'처리 중 오류가 발생했습니다.';
        SET @ErrorCode = ERROR_NUMBER();
        SET @ErrorMsg = ERROR_MESSAGE();
    END CATCH
END
GO

-- 설정화면 메뉴 등록 - 사용자관리(SM_USRG) 그룹 밑에 사용자등록(SM_USER)과 나란히 둔다(단축키도
-- 개인별 사용자 설정이라는 점에서 같은 묶음이 자연스럽다). 범용 데이터 통로(api/data/*)를 쓰므로
-- PROC_PREFIX를 등록해야 화면에서 USP_SM_SHORTCUT_*를 호출할 수 있다.
IF NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_CD = 'SM_SHORTCUT')
BEGIN
    INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN, REG_DT, PROC_PREFIX)
    VALUES ('SM_SHORTCUT', N'단축키설정', 'SM_USRG', 3, 'FORM', 'WYNLAB.SM.SHORTCUT.frmShortcut, WYNLAB.SM', NULL, 30, 'Y', GETDATE(), 'USP_SM_SHORTCUT_');
END
GO

SELECT MENU_CD, MENU_NM, UPPER_MENU_CD, FORM_CLASS_NM, PROC_PREFIX FROM TSMMENU WHERE MENU_CD = 'SM_SHORTCUT';
GO
