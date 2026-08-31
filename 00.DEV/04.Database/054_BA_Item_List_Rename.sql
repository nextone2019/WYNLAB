-- BA_ITEM_LIST -> BA_ITEMLIST로 이름 정정. 기존 BA_ITEMGRP(->frmItemGrp)와 같은 관례
-- (합성어는 언더스코어 없이 붙여 씀)를 그대로 따른다. FORM_CLASS_NM(frmItemList)은 안 바뀐다 -
-- 폼 이름 자체는 원래도 목표한 그대로였고, menu_cd 표기만 정정하는 것.

UPDATE TSMMENU
SET MENU_CD = 'BA_ITEMLIST', PROC_PREFIX = 'USP_BA_ITEMLIST_'
WHERE MENU_CD = 'BA_ITEM_LIST';
GO
