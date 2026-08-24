using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress LabelControl 기반 - 지금까지 화면마다 필드 라벨에 반복해서 손으로 맞추던 스타일
/// (AppFonts.Caption + 진회색)을 생성자에서 기본값으로 적용해둔다. GroupBoxWyn의 캡션과 같은
/// 톤앤매너다.
/// </summary>
[ToolboxItem(true)]
public class LabelWyn : LabelControl
{
    public LabelWyn()
    {
        Appearance.Font = AppFonts.Caption;
        Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        Appearance.Options.UseFont = true;
        Appearance.Options.UseForeColor = true;
    }
}
