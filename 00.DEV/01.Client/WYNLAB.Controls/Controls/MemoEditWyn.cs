using System.ComponentModel;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>
/// MemoEdit을 상속해서 "필수입력" 표시를 속성창(프로퍼티 그리드)에서 체크박스로 바로 켤 수 있게
/// 확장한 컨트롤 - TextEditWyn과 완전히 같은 목적/구조다(비고/설명처럼 여러 줄 입력이 필요한
/// 자리에서 TextEditWyn 대신 이걸 쓴다). RequiredFieldExtensions.MarkRequired&lt;T&gt;()가
/// BaseEdit 계열 전체(TextEdit/MemoEdit/DateEdit 등)를 공통으로 지원해서 로직은 그대로 재사용하고,
/// 여기서는 디자이너 체크박스 노출만 담당한다.
/// </summary>
[ToolboxItem(true)]
public class MemoEditWyn : MemoEdit
{
    private bool _required;

    [Category("WYNLAB")]
    [Description("필수 입력 항목이면 배경색으로 강조 표시합니다.")]
    [DefaultValue(false)]
    public bool Required
    {
        get => _required;
        set
        {
            _required = value;
            ApplyRequiredStyle();
        }
    }

    private void ApplyRequiredStyle()
    {
        if (_required)
        {
            this.MarkRequired();
        }
        else
        {
            this.ClearRequired();
        }
    }
}
