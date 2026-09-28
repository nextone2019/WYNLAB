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
    private int _decimalPlaces;

    public SpinEditWyn()
    {
        // 위/아래 화살표(증감 버튼) - 클릭해서 값을 1씩 늘리고 줄이는 용도가 아니라 그냥 숫자
        // 입력칸으로 쓰는 화면이 대부분이라 기본은 숨긴다(2026-09-07 요청). SpinEdit엔
        // "ShowSpinButtons" 같은 단일 스위치가 없고(설치된 DevExpress.XtraEditors.v21.2.dll을
        // 직접 리플렉션해서 확인 - 추측 금지 컨벤션), 그 버튼도 결국 Buttons 컬렉션의 항목 중
        // 하나(SpinButtonIndex가 가리키는 자리)라서 그 항목만 안 보이게 끈다. 필요한 화면만
        // Properties.Buttons[Properties.SpinButtonIndex].Visible = true로 다시 켜면 된다.
        HideSpinButton();
        Properties.DisplayFormat.FormatType = FormatType.Numeric;
        Properties.EditFormat.FormatType = FormatType.Numeric;
        DecimalPlaces = 0;
        Properties.MaxValue = 999_999_999_999;
        AllowNegative = false;
    }

    /// <summary>LookUpEditWyn/PopupLookupEditWyn과 같은 이유(각 클래스 주석 참고) - Designer의
    /// BeginInit/EndInit 구간을 지나면서 생성자에서 손댄 Buttons 항목의 Visible이 되돌아갈 수
    /// 있어서, 실제로 그려지기 직전에 한 번 더 강제한다.</summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        HideSpinButton();
    }

    /// <summary>SpinButtonIndex가 가리키는 자리가 실제로 Buttons 안에 있을 때만 끈다 - VS
    /// 디자이너가 폼을 여는 도중 이 컨트롤을 BeginInit()까지만 해두고 아직 Buttons를 채우기 전에
    /// OnHandleCreated가 먼저 불리는 순간이 있어서(실제로 겪음, 2026-09-07 - "인덱스가 범위를
    /// 벗어났습니다" 예외로 디자이너에서 컨트롤 자체가 비활성화됨), 인덱스 검사 없이 바로 접근하면
    /// 디자이너가 그 컨트롤을 통째로 못 열게 된다.</summary>
    private void HideSpinButton()
    {
        var index = Properties.SpinButtonIndex;
        if (index >= 0 && index < Properties.Buttons.Count)
            Properties.Buttons[index].Visible = false;
    }

    /// <summary>소수점 이하 자릿수. 기본 0(정수) - 수량/금액처럼 소수가 필요한 컬럼은 이 값을
    /// 늘리면 표시/편집 형식(DisplayFormat/EditFormat, "n{자릿수}")이 같이 바뀐다.</summary>
    [Category("WYNLAB")]
    [Description("소수점 이하 자릿수. 기본 0(정수) - 수량/금액 등 소수가 필요한 컬럼에서 늘려서 씁니다.")]
    [DefaultValue(0)]
    public int DecimalPlaces
    {
        get => _decimalPlaces;
        set
        {
            _decimalPlaces = value;
            var format = "n" + value;
            Properties.DisplayFormat.FormatString = format;
            Properties.EditFormat.FormatString = format;
        }
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
                this.ClearRequired();
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
