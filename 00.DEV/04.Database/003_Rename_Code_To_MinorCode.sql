/* =========================================================
   기초코드등록(CODE) 화면을 frmMinorCode로 리네임하면서, 서버쪽(프로시저/메뉴코드)도
   화면명에 맞춰 같이 정리한다 - 화면은 frmMinorCode인데 프로시저/메뉴코드는 여전히
   CODE/SM_CODE_BASE로 남아있으면 이름만 보고 서로 매칭이 안 된다는 문제 때문.

   프로시저는 sp_rename으로 이름만 바꾼다(본문은 그대로 - 재입력 실수 위험 없음).
   메뉴코드(TSMMENU.MENU_CD)는 PK라서 단순 UPDATE가 안 된다 - TSMMENUAUTH가
   FK(FK_TSMMENUAUTH_MENU, NO_ACTION)로 참조하고 있어서, 새 행을 만들고 자식을
   옮긴 뒤 옛 행을 지우는 순서로 처리한다.
   ========================================================= */

-- 1) 프로시저 이름 변경 (본문은 그대로)
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'USP_SM_CODE_Q')
    EXEC sp_rename 'USP_SM_CODE_Q', 'USP_SM_MINORCODE_Q';
GO
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'USP_SM_CODE_S')
    EXEC sp_rename 'USP_SM_CODE_S', 'USP_SM_MINORCODE_S';
GO
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'USP_SM_CODE_S_1')
    EXEC sp_rename 'USP_SM_CODE_S_1', 'USP_SM_MINORCODE_S_1';
GO

-- 2) 메뉴코드 SM_CODE_BASE -> SM_MINOR_CODE (PK 변경 - FK 자식(TSMMENUAUTH)도 같이 이동)
IF EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_CD = 'SM_CODE_BASE')
    AND NOT EXISTS (SELECT 1 FROM TSMMENU WHERE MENU_CD = 'SM_MINOR_CODE')
BEGIN
    INSERT INTO TSMMENU (MENU_CD, MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE, FORM_CLASS_NM, ICON_NM, SORT_ORDER, USE_YN, REG_DT, REG_USER_ID)
    SELECT 'SM_MINOR_CODE', MENU_NM, UPPER_MENU_CD, MENU_LEVEL, MENU_TYPE,
           'WYNLAB.SM.frmMinorCode.frmMinorCode, WYNLAB.SM.frmMinorCode',
           ICON_NM, SORT_ORDER, USE_YN, REG_DT, REG_USER_ID
    FROM TSMMENU
    WHERE MENU_CD = 'SM_CODE_BASE';

    UPDATE TSMMENUAUTH SET MENU_CD = 'SM_MINOR_CODE' WHERE MENU_CD = 'SM_CODE_BASE';

    DELETE FROM TSMMENU WHERE MENU_CD = 'SM_CODE_BASE';
END
GO
