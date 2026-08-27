using System.ComponentModel;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress CheckEdit 기반 - TextEditWyn/RadioBoxWyn과 같은 목적/구조로 "필수입력" 표시를
/// 속성창에서 체크박스 하나로 켤 수 있게 확장했다. "시스템관리자 권한 부여", "사용" 같은
/// 단일 예/아니오 항목에 쓴다(선택지가 여러 개면 RadioBoxWyn을 대신 쓴다).
/// </summary>
[ToolboxItem(true)]
public class CheckBoxWyn : CheckEdit
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
            Properties.Appearance.Options.UseBackColor = false;
            Properties.Appearance.Options.UseForeColor = false;
        }
    }
}
