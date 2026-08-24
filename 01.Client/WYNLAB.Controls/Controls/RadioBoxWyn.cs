using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress RadioGroup 기반 - 기본 레이아웃을 Flow(가로로 흐르는 배치)로 켜둔다. WYNLAB
/// 화면은 대부분 "사용여부: [사용] [미사용]" 처럼 좁은 공간에 가로로 몇 개 안 되는 선택지를
/// 넣는 경우가 많아서, DevExpress 기본값(Column, 세로 배치)보다 이쪽이 화면 밀도에 더 맞는다.
/// SetItems()로 값-라벨 쌍을 한 번에 채울 수 있다.
/// </summary>
[ToolboxItem(true)]
public class RadioBoxWyn : RadioGroup
{
    private bool _required;

    public RadioBoxWyn()
    {
        Properties.ItemsLayout = RadioGroupItemsLayout.Flow;
    }

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

    /// <summary>값-라벨 쌍으로 항목을 한 번에 채운다. 기존 항목은 모두 지워진다.</summary>
    public void SetItems(params (object Value, string Text)[] items)
    {
        Properties.Items.Clear();
        foreach (var (value, text) in items)
        {
            Properties.Items.Add(new RadioGroupItem(value, text));
        }
    }
}
