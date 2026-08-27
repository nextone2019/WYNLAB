using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DateEdit을 상속해서 "일(day) 숫자만 입력해도 이번 달 그 날짜로 자동 인식"하게 확장한 컨트롤.
/// 원래 DateEdit은 전체 날짜(예: 20260822)를 다 타이핑해야만 인식하는데, 업무 화면에서는
/// "이번 달 22일"처럼 당월 날짜를 입력하는 경우가 압도적으로 많아서 "22"만 쳐도 되게 하면
/// 입력 속도가 크게 줄어든다. RepositoryItem.Parse 이벤트로 원본 텍스트를 가로채 해석한다 -
/// DevExpress가 이미 제공하는 훅이라 상속 없이 이벤트 구독만으로도 되긴 하지만, 매번 화면마다
/// 구독을 빼먹지 않으려면(RequiredFieldExtensions와 같은 이유) 컨트롤 자체에 박아두는 게 안전하다.
/// </summary>
[ToolboxItem(true)]
public class DateEditWyn : DateEdit
{
    public DateEditWyn()
    {
        Properties.ParseEditValue += Properties_ParseEditValue;
    }

    private void Properties_ParseEditValue(object? sender, ConvertEditValueEventArgs e)
    {
        var text = e.Value?.ToString()?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        if (int.TryParse(text, out var day) && day is >= 1 and <= 31)
        {
            var now = DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);
            if (day <= lastDayOfMonth)
            {
                e.Value = new DateTime(now.Year, now.Month, day);
                e.Handled = true;
            }
        }
    }
}
