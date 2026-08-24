using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress GroupControl 기반 - 캡션 폰트/색상을 화면마다 따로 맞추지 않도록 생성자에서
/// 하우스 스타일(AppFonts.Caption, 진회색)을 기본으로 적용해둔다. 그 외 동작은 순정 그대로다.
/// </summary>
[ToolboxItem(true)]
public class GroupBoxWyn : GroupControl
{
    public GroupBoxWyn()
    {
        AppearanceCaption.Font = AppFonts.Caption;
        AppearanceCaption.ForeColor = Color.FromArgb(100, 100, 100);
        AppearanceCaption.Options.UseFont = true;
        AppearanceCaption.Options.UseForeColor = true;
    }
}
