-- P_MENU 팝업(상위메뉴 선택) 신규 - 메뉴등록(frmMenu)의 상위메뉴명(txtUpperMenuNm)에서 이름으로
-- 검색해 부모 메뉴를 고르면 상위메뉴코드(txtUpperMenuCd)가 같이 채워지도록 한다. TSMMENU
-- 자체가 계층 구조(UPPER_MENU_CD 자기참조)라 P_DEPT와 완전히 같은 패턴(hierarchical_yn='Y')
-- 으로 만든다 - 트리로 뜨고, 표시 컬럼은 메뉴코드/메뉴명 2개만.

CREATE PROCEDURE [dbo].[SSP_POP_MENU_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MENU_CD AS menu_cd, MENU_NM AS menu_nm, UPPER_MENU_CD AS upper_menu_cd, MENU_TYPE AS menu_type
    FROM TSMMENU
    WHERE USE_YN = 'Y'
      AND (@p_keyword IS NULL OR @p_keyword = ''
           OR MENU_CD LIKE '%' + @p_keyword + '%'
           OR MENU_NM LIKE '%' + @p_keyword + '%')
    ORDER BY MENU_CD;
END
GO

INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field, display_field, popup_width, popup_height, use_yn, reg_user_id, reg_dt)
VALUES ('P_MENU', 'SSP_POP_MENU_Q', N'메뉴 조회', 'Y', 'menu_cd', 'upper_menu_cd', 'menu_nm', 700, 500, 'Y', 'admin', GETDATE());
GO

INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, sort, width, visible_yn)
VALUES
    ('P_MENU', 'menu_cd', N'메뉴코드', 'TEXT', 1, 150, 'Y'),
    ('P_MENU', 'menu_nm', N'메뉴명', 'TEXT', 2, 300, 'Y');
GO

INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width)
VALUES ('P_MENU', 'p_keyword', N'메뉴코드/명', 'TEXT', 1, 180);
GO
