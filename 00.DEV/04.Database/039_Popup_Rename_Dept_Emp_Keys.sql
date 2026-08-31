-- 팝업키 DEPT -> P_DEPT, EMP -> P_EMP로 변경(사장님 요청). popup_key는 sysPopUpM의 PK이자
-- sysPopUpD/sysPopUpS의 FK라서, 단순 UPDATE로 부모 키를 바꾸면 그 순간 기존 자식 행들이
-- 존재하지 않는 부모를 가리키게 되어 FK 위반이 난다. 그래서 "새 키로 부모 행을 먼저 복제 ->
-- 자식 행들을 새 키로 옮김 -> 옛 부모 행 삭제" 순서로 안전하게 옮긴다.

DECLARE @renames TABLE (old_key VARCHAR(30), new_key VARCHAR(30));
INSERT INTO @renames VALUES ('DEPT', 'P_DEPT'), ('EMP', 'P_EMP');

DECLARE @old_key VARCHAR(30), @new_key VARCHAR(30);
DECLARE rename_cursor CURSOR LOCAL FAST_FORWARD FOR SELECT old_key, new_key FROM @renames;
OPEN rename_cursor;
FETCH NEXT FROM rename_cursor INTO @old_key, @new_key;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = @old_key)
       AND NOT EXISTS (SELECT 1 FROM sysPopUpM WHERE popup_key = @new_key)
    BEGIN
        INSERT INTO sysPopUpM (popup_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
                                display_field, popup_width, popup_height, use_yn, remark, reg_user_id, reg_dt)
        SELECT @new_key, proc_nm, popup_nm, hierarchical_yn, key_field, parent_field,
               display_field, popup_width, popup_height, use_yn, remark, reg_user_id, reg_dt
        FROM sysPopUpM WHERE popup_key = @old_key;

        UPDATE sysPopUpD SET popup_key = @new_key WHERE popup_key = @old_key;
        UPDATE sysPopUpS SET popup_key = @new_key WHERE popup_key = @old_key;

        DELETE FROM sysPopUpM WHERE popup_key = @old_key;
    END

    FETCH NEXT FROM rename_cursor INTO @old_key, @new_key;
END

CLOSE rename_cursor;
DEALLOCATE rename_cursor;
GO
