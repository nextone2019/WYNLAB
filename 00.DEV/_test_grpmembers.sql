SET NOCOUNT ON;
DECLARE @rc INT, @rm NVARCHAR(200);
EXEC USP_SM_USERGRPAUTH_Q @p_work_type='Q2', @p_user_grp_cd='ADMIN';

EXEC USP_SM_USERGRPAUTH_S_2 @p_work_type='U', @p_user_grp_cd='ADMIN', @p_user_ids='test3,test4', @p_user_id='admin',
  @ReturnCode=@rc OUTPUT, @ReturnMsg=@rm OUTPUT;
SELECT ReturnCode=@rc, ReturnMsg=@rm;
EXEC USP_SM_USERGRPAUTH_Q @p_work_type='Q2', @p_user_grp_cd='ADMIN';

-- cleanup: ADMIN 그룹 원상복구(비워둠)
DELETE FROM TSMUSERGRPMAP WHERE USER_GRP_CD='ADMIN';
PRINT 'done';
