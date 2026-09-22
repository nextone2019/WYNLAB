-- frmItem(품목등록)의 담당부서/담당자/구매처 팝업 전체 점검 (2026-09-15, "전체 다 수정해줘").
-- 발견한 문제 3가지:
--   1) popDetailDeptNm/popDetailEmpNm이 MapField 연결이 없어서 팝업으로 선택해도 화면의
--      dept_id/emp_id가 안 바뀜(이름만 바뀜) - 저장해도 예전 값 그대로 남는다.
--   2) popDetailCustNm.LookupKey가 "P_EMP"로 잘못 지정되어 있었다(사원 팝업이 뜸) - 구매처
--      전용 팝업(P_CUST) 자체가 아예 없었다.
--   3) frmItem.cs의 SaveClick이 p_emp_id/p_cust_id를 처음부터 보내지 않고 있었다 - 화면에
--      뭘 골랐든 담당자/구매처는 항상 저장이 안 되고 있었다(USP_BA_ITEM_S는 이미 두 파라미터를
--      받는데 클라이언트가 안 보냈을 뿐).
-- 이 마이그레이션은 1)/2) 중 "P_CUST 팝업이 없다"는 DB쪽 문제만 해결한다(SSP_POP_CUST_Q +
-- 메타데이터). MapField 연결/LookupKey 수정/SaveClick 파라미터 추가는 frmItem.cs/.Designer.cs
-- 쪽 수정.

CREATE OR ALTER PROCEDURE [dbo].[SSP_POP_CUST_Q]
    @p_keyword VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CUST_ID, cust_nm, biz_no, owner_nm, tel
    FROM TBACUST
    WHERE (@p_keyword IS NULL OR @p_keyword = ''
           OR cust_nm LIKE '%' + @p_keyword + '%')
    ORDER BY CUST_ID;
END
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = 'P_CUST')
INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field, display_field, popup_width, popup_height, use_yn, reg_user_id, reg_dt)
VALUES ('P_CUST', 'SSP_POP_CUST_Q', N'구매처 조회', 'N', 'CUST_ID', NULL, 'cust_nm', 700, 500, 'Y', SUSER_SNAME(), GETDATE());
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpD WHERE popup_key = 'P_CUST')
BEGIN
    INSERT INTO sysPopUpD (popup_key, column_nm, caption, control_type, sort, width, visible_yn) VALUES
    ('P_CUST', 'CUST_ID',  N'구매처ID', 'TEXT', 1, 100, 'N'),
    ('P_CUST', 'cust_nm',  N'구매처명', 'TEXT', 2, 200, 'Y'),
    ('P_CUST', 'biz_no',   N'사업자번호', 'TEXT', 3, 120, 'Y'),
    ('P_CUST', 'owner_nm', N'대표자', 'TEXT', 4, 100, 'Y'),
    ('P_CUST', 'tel',      N'전화번호', 'TEXT', 5, 120, 'Y');
END
GO

IF NOT EXISTS (SELECT 1 FROM sysPopUpS WHERE popup_key = 'P_CUST')
INSERT INTO sysPopUpS (popup_key, param_nm, caption, control_type, sort, width) VALUES ('P_CUST', 'p_keyword', N'구매처명', 'TEXT', 1, 180);
GO
