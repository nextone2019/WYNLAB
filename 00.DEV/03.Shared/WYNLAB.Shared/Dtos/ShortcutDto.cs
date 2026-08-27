namespace WYNLAB.Shared.Dtos;

/// <summary>
/// 상단 공통 툴바 액션(조회/입력/삭제/행추가/행삭제/저장/출력) 하나에 대한, 로그인 사용자
/// 기준 유효 단축키. 서버가 TSMSHORTCUTDEFAULT(기본값)와 TSMUSERSHORTCUT(사용자 재정의)를
/// 병합해서 내려준다 - 화면은 재정의 여부(CustomYn)를 몰라도 KeyCombo만 보면 그대로 쓸 수 있다.
/// </summary>
public class ShortcutDto
{
    /// <summary>QUERY/NEW/DELETE/ROWADD/ROWDELETE/SAVE/PRINT 중 하나 - BaseForm 표준 액션과 1:1</summary>
    public string ActionCd { get; set; } = string.Empty;
    public string ActionNm { get; set; } = string.Empty;

    /// <summary>"Ctrl+Q"처럼 ShortcutKeys.ToText(Keys)와 같은 형식의 문자열</summary>
    public string KeyCombo { get; set; } = string.Empty;

    /// <summary>사용자가 기본값을 재정의했는지 - 설정화면의 "초기화" 버튼 활성화 여부에 씀</summary>
    public bool CustomYn { get; set; }
    public int SortOrder { get; set; }
}
