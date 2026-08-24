using WYNLAB.Base;

namespace WYNLAB.SM.TEMPLATE;

/// <summary>
/// 새 화면 개발용 템플릿 - 자세한 사용법은 TemplateForm.Designer.cs 상단 주석 참고.
/// </summary>
public partial class TemplateForm : BaseForm
{
    public TemplateForm()
    {
        InitializeComponent();
        Text = "화면명";
        MenuCd = "MENU_CD_HERE";
    }
}
