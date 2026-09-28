using System.ComponentModel;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>
/// TextEdit을 상속해서 "필수입력" 표시를 속성창(프로퍼티 그리드)에서 체크박스로 바로 켤 수 있게
/// 확장한 컨트롤. 기존 RequiredFieldExtensions.MarkRequired()는 코드에서 한 줄 호출해야 했는데
/// (호출을 빼먹으면 그 화면만 조용히 예전 스타일로 남는 문제가 있었다), 이 컨트롤을 툴박스에서
/// 끌어다 놓으면 코드 없이도 디자이너에서 Required 체크 한 번으로 100% 동일하게 적용된다.
/// 실제 스타일 정의는 여전히 UiTheme/MarkRequired() 하나에서만 관리 - 색만 여기서도 재사용한다.
/// </summary>
[ToolboxItem(true)]
public class TextEditWyn : TextEdit
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
