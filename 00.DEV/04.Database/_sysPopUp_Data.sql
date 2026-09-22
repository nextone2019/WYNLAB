-- 현재 개발 PC의 PopUp관리(sysPopUpM 3건 + sysPopUpD 13건 + sysPopUpS 4건) 데이터 -
-- SMO(Table.EnumScript, ScriptData=true)로 실제 값 그대로 뽑아냈다. IDENTITY 컬럼이 없어
-- (popup_key가 그 자체로 PK) IDENTITY_INSERT가 필요 없다.
--
-- _FullSchema_Snapshot.sql을 먼저 적용한 뒤 실행한다. 다시 뽑아야 하면 손으로 고치지 말고
-- 통째로 재생성한다. LookUp관리(sysLookupM/P/C)는 별도로 이미 채워져 있어 여기 포함하지 않는다.
INSERT [dbo].[sysPopUpM] ([popup_key], [proc_nm], [popup_nm], [hierarchical_yn], [key_field], [parent_field], [display_field], [popup_width], [popup_height], [use_yn], [remark], [reg_user_id], [reg_dt], [upt_user_id], [upt_dt]) VALUES (N'P_DEPT', N'SSP_POP_DEPT_Q', N'부서 조회', N'N', N'DEPT_ID', NULL, N'dept_nm', 700, 500, N'Y', NULL, NULL, CAST(N'2026-08-28T16:37:05.617' AS DateTime), NULL, NULL)
INSERT [dbo].[sysPopUpM] ([popup_key], [proc_nm], [popup_nm], [hierarchical_yn], [key_field], [parent_field], [display_field], [popup_width], [popup_height], [use_yn], [remark], [reg_user_id], [reg_dt], [upt_user_id], [upt_dt]) VALUES (N'P_EMP', N'SSP_POP_EMP_Q', N'사원정보조회', N'N', N'EMP_ID', NULL, N'emp_nm', 700, 500, N'Y', N'', N'admin', CAST(N'2026-08-31T14:24:13.100' AS DateTime), N'admin', CAST(N'2026-08-31T17:35:30.737' AS DateTime))
INSERT [dbo].[sysPopUpM] ([popup_key], [proc_nm], [popup_nm], [hierarchical_yn], [key_field], [parent_field], [display_field], [popup_width], [popup_height], [use_yn], [remark], [reg_user_id], [reg_dt], [upt_user_id], [upt_dt]) VALUES (N'P_MENU', N'SSP_POP_MENU_Q', N'메뉴 조회', N'Y', N'menu_id', N'upper_menu_id', N'menu_nm', 700, 500, N'Y', NULL, N'admin', CAST(N'2026-08-31T22:39:03.080' AS DateTime), NULL, NULL)

INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_DEPT', N'DEPT_ID', N'부서ID', N'TEXT', NULL, 1, 100, N'N')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_DEPT', N'dept_nm', N'부서명', N'TEXT', NULL, 2, 200, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_DEPT', N'dept_type', N'부서유형', N'TEXT', NULL, 3, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_DEPT', N'PAR_DEPT_ID', N'상위부서코드', N'TEXT', NULL, 4, 100, N'N')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'dept_nm', N'부서명', N'TEXT', N'', 5, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'EMP_ID', N'사원ID', N'TEXT', NULL, 0, 100, N'N')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'emp_nm', N'사원명', N'TEXT', N'', 2, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'emp_nm_eng', N'사원명(영문)', N'TEXT', N'', 3, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'emp_no', N'사원번호', N'TEXT', N'', 1, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'ent_date', N'입사일자', N'DATE', N'', 6, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_EMP', N'grp_ent_date', N'그룹입사일자', N'DATE', N'', 7, 100, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_MENU', N'menu_id', N'메뉴ID', N'TEXT', NULL, 1, 150, N'Y')
INSERT [dbo].[sysPopUpD] ([popup_key], [column_nm], [caption], [control_type], [lookup_proc_nm], [sort], [width], [visible_yn]) VALUES (N'P_MENU', N'menu_nm', N'메뉴명', N'TEXT', NULL, 2, 300, N'Y')

INSERT [dbo].[sysPopUpS] ([popup_key], [param_nm], [caption], [control_type], [sort], [width]) VALUES (N'P_DEPT', N'p_keyword', N'부서명', N'TEXT', 1, 180)
INSERT [dbo].[sysPopUpS] ([popup_key], [param_nm], [caption], [control_type], [sort], [width]) VALUES (N'P_EMP', N'p_code', N'사원번호/명', N'TEXT', 1, 120)
INSERT [dbo].[sysPopUpS] ([popup_key], [param_nm], [caption], [control_type], [sort], [width]) VALUES (N'P_EMP', N'p_dept_nm', N'부서명', N'TEXT', 3, 120)
INSERT [dbo].[sysPopUpS] ([popup_key], [param_nm], [caption], [control_type], [sort], [width]) VALUES (N'P_MENU', N'p_keyword', N'메뉴코드/명', N'TEXT', 1, 180)

