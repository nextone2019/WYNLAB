using System.ComponentModel;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress SpinEdit 기반 - 수량/금액 등 ERP 화면에서 반복되는 기본값(천단위 구분 표시,
/// 음수 금지)을 생성자에서 미리 켜둔다. 코드 컬럼처럼 예외적으로 음수가 필요한 화면만
/// AllowNegative를 켜면 된다.
/// </summary>
[ToolboxItem(true)]
public class SpinEditWyn : SpinEdit
{
    private bool _required;
    private bool _allowNegative;

    public SpinEditWyn()
    {
        Properties.DisplayFormat.FormatType = FormatType.Numeric;
        Properties.DisplayFormat.FormatString = "n0";
        Properties.EditFormat.FormatType = FormatType.Numeric;
        Properties.EditFormat.FormatString = "n0";
        Properties.MaxValue = 999_999_999_999;
        AllowNegative = false;
    }

    /// <summary>true로 켜면 필수입력 스타일(연노랑 배경)을 적용합니다.</summary>
    [Category("WYNLAB")]
    [Description("필수입력 스타일(연노랑 배경)을 적용합니다.")]
    [DefaultValue(false)]
    public bool Required
    {
        get => _required;
        set
        {
            _required = value;
            if (value)
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

    /// <summary>음수 입력 허용 여부 - 기본은 false(수량/금액은 보통 음수가 아니므로).</summary>
    [Category("WYNLAB")]
    [Description("음수 입력을 허용할지 여부. 기본은 false - 수량/금액 등 대부분의 ERP 숫자 필드는 음수가 없음.")]
    [DefaultValue(false)]
    public bool AllowNegative
    {
        get => _allowNegative;
        set
        {
            _allowNegative = value;
            Properties.MinValue = value ? decimal.MinValue : 0;
        }
    }
}
