namespace WYNLAB.Base;

/// <summary>
/// 사용자별 단축키 설정에서 쓰는 Keys <-> 문자열("Ctrl+Shift+D") 변환. 서버(TSMSHORTCUTDEFAULT/
/// TSMUSERSHORTCUT.KEY_COMBO)와 클라이언트가 같은 문자열 형식을 쓰는 게 중요하다 - ShellForm의
/// ProcessCmdKey는 눌린 키를 이 형식으로 바꿔 SessionManager.Shortcuts의 KeyCombo와 그대로
/// 문자열 비교하고, 설정화면(frmShortcut)도 사용자가 새로 누른 키를 이 형식으로 저장한다.
/// </summary>
public static class ShortcutKeys
{
    /// <summary>ShellForm이 여전히 직접 처리하는 키(DevExpress 문서선택기가 네이티브로 먼저
    /// 가로채는 Ctrl+Tab 등) - 사용자가 단축키 설정화면에서 이 값을 지정하지 못하게 막는다.</summary>
    public static readonly string[] ReservedCombos = { "Ctrl+Tab" };

    /// <summary>KeyDown/ProcessCmdKey의 Keys 값을 "Ctrl+Shift+D" 형식 문자열로 바꾼다.
    /// 순서는 항상 Ctrl -> Alt -> Shift -> 키 이름이다.</summary>
    public static string ToText(Keys keyData)
    {
        var parts = new List<string>();
        if ((keyData & Keys.Control) == Keys.Control) parts.Add("Ctrl");
        if ((keyData & Keys.Alt) == Keys.Alt) parts.Add("Alt");
        if ((keyData & Keys.Shift) == Keys.Shift) parts.Add("Shift");
        parts.Add((keyData & Keys.KeyCode).ToString());
        return string.Join("+", parts);
    }

    /// <summary>수정키(Ctrl/Alt/Shift)만 눌리고 아직 본 키가 없는 상태 - 설정화면 키캡처가
    /// 이 상태에서는 저장하지 않고 다음 키 입력을 계속 기다려야 한다.</summary>
    public static bool IsModifierOnly(Keys keyData) =>
        (keyData & Keys.KeyCode) is Keys.ControlKey or Keys.Menu or Keys.ShiftKey
            or Keys.LControlKey or Keys.RControlKey or Keys.LMenu or Keys.RMenu or Keys.LShiftKey or Keys.RShiftKey;
}
