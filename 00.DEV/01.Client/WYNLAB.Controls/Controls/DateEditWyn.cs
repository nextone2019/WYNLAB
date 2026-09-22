using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 숫자만 입력해도 자릿수만으로 날짜를 자동 인식하는 파싱 규칙(2026-09-09 요청 - "20260901처럼
/// 순서대로 치면 날짜가 들어가게"). DateEditWyn(패널용 컨트롤)과 DateColumnEdit(그리드 컬럼용
/// RepositoryItem) 둘 다 이 로직을 그대로 재사용한다 - 파싱 규칙 자체는 어느 쪽에 붙어있든
/// 완전히 같아야 하므로 여기 한 곳에만 둔다.
///
/// 자릿수별로 의미를 다르게 해석한다: 2자리("22") -> 이번 달 22일 / 4자리("0822") -> 이번 해
/// 8월22일 / 8자리("20260901") -> 2026년9월1일 그대로. 구분자(-,/) 없이 숫자만 쭉 입력하는
/// 습관이 업무화면에서 압도적으로 많아서, 자릿수 = 몇 개의 날짜요소를 생략했는지를 뜻하는
/// 규칙으로 통일했다. 숫자 외 문자가 섞인 입력(사용자가 "2026-09-01"처럼 구분자를 직접 친
/// 경우)이나 이 세 자릿수에 안 맞는 입력은 손대지 않고 DevExpress 기본 파서에 그대로 맡긴다.
/// </summary>
internal static class SmartDateParser
{
    public static bool TryParse(string? text, out DateTime result)
    {
        result = default;
        if (string.IsNullOrEmpty(text)) return false;

        var now = DateTime.Now;

        switch (text!.Length)
        {
            case 1 or 2 when int.TryParse(text, out var day) && day is >= 1 and <= 31:
                return TryBuild(now.Year, now.Month, day, out result);

            case 4 when int.TryParse(text.Substring(0, 2), out var month)
                        && int.TryParse(text.Substring(2, 2), out var day4):
                return TryBuild(now.Year, month, day4, out result);

            case 8 when int.TryParse(text.Substring(0, 4), out var year)
                        && int.TryParse(text.Substring(4, 2), out var month8)
                        && int.TryParse(text.Substring(6, 2), out var day8):
                return TryBuild(year, month8, day8, out result);

            default:
                return false;
        }
    }

    /// <summary>월/일 값이 실존하는 날짜인지(2월 30일 같은 값 방지) 확인하고 나서만 반영한다 -
    /// DateTime 생성자에 바로 넘기면 잘못된 값에서 예외가 나서 입력 자체가 막혀버린다.</summary>
    private static bool TryBuild(int year, int month, int day, out DateTime result)
    {
        result = default;
        if (month is < 1 or > 12) return false;
        if (day < 1 || day > DateTime.DaysInMonth(year, month)) return false;

        result = new DateTime(year, month, day);
        return true;
    }

    /// <summary>DateEditWyn.Properties.ParseEditValue/DateColumnEdit.ParseEditValue 핸들러가
    /// 그대로 호출하는 공용 로직 - 자릿수 규칙 -> 사람이 구분자를 직접 친 일반 날짜문자열(예:
    /// "2026-09-01") -> 그 외(빈 값이거나 정말 해석 안 되는 값)는 순서로 시도한다.
    ///
    /// 마지막 단계가 필요한 이유(2026-09-10 실제로 겪음): MaskType.None으로 바꾼 뒤로 DevExpress
    /// 자체 파서가 빈 문자열/해석 실패한 텍스트를 null로 두지 않고 DateTime.MinValue(0001-01-01)로
    /// 떨어뜨려서, 필드를 다 지워도 "0001-01-01"같은 값이 남아있는 것처럼 보였다 - 날짜도 아닌
    /// 값을 억지로 저런 식으로 보여주는 것보단 빈 값으로 되돌리는 게 사용자에게 덜 혼란스럽다는
    /// 요청에 따라, 빈 값을 포함해 여기서 항상 직접 e.Value를 확정해서 DevExpress 기본 파서로
    /// 넘어갈 일이 없게 막는다(처음엔 빈 문자열만 예외로 두고 DevExpress 기본 처리에 맡겼는데,
    /// 그 기본 처리 자체가 0001-01-01로 떨어지는 원인이었다 - 실제로 겪음).</summary>
    public static void Handle(DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
    {
        var text = e.Value?.ToString()?.Trim();

        if (!string.IsNullOrEmpty(text))
        {
            if (TryParse(text, out var date))
            {
                e.Value = date;
                e.Handled = true;
                return;
            }

            if (DateTime.TryParse(text, out var parsed))
            {
                e.Value = parsed;
                e.Handled = true;
                return;
            }
        }

        e.Value = null;
        e.Handled = true;
    }
}

/// <summary>
/// DateEdit을 상속해서 SmartDateParser 규칙을 적용한 패널용 날짜 컨트롤. RepositoryItem.Parse
/// 이벤트로 원본 텍스트를 가로채 해석한다 - DevExpress가 이미 제공하는 훅이라 상속 없이 이벤트
/// 구독만으로도 되긴 하지만, 매번 화면마다 구독을 빼먹지 않으려면(RequiredFieldExtensions와
/// 같은 이유) 컨트롤 자체에 박아두는 게 안전하다.
/// </summary>
[ToolboxItem(true)]
public class DateEditWyn : DateEdit
{
    public DateEditWyn()
    {
        // DevExpress DateEdit은 기본값이 MaskType=DateTime(EditMask="d")인 대화형 마스크라, 위
        // SmartDateParser가 텍스트를 보기도 전에 이 마스크 엔진이 키 입력을 연/월/일 구간별로
        //먼저 가로채서 망가뜨린다(2026-09-10 실제로 겪음 - "20260910"을 순서대로 치면
        // "2605-09-10"처럼 엉뚱하게 들어감; 포커스 진입 시 전체선택되던 것도 타이핑 시작하는
        // 순간 "연" 구간만 남도록 좁아짐 - 전형적인 마스크 세그먼트 편집 동작). 마스크를 꺼서
        // 순수 텍스트 입력으로 바꾸면 키 입력이 그대로 쌓였다가 포커스아웃/Enter 시점에
        // ParseEditValue로 넘어가 SmartDateParser가 온전한 문자열을 받아 해석할 수 있다.
        Properties.Mask.MaskType = MaskType.None;
        Properties.ParseEditValue += Properties_ParseEditValue;
        KeyDown += DateEditWyn_KeyDown;
    }

    /// <summary>LookUpEditWyn.OnHandleCreated와 같은 이유(그 클래스 설명 참고) - DateEdit도
    /// ButtonEdit 계열이라 InitializeComponent의 BeginInit/EndInit을 지나면서 생성자의 기본
    /// 캘린더 버튼이 비워지는 DevExpress 버그를 겪는다(2026-09-09 실제로 겪음 - 사원등록 화면
    /// 날짜필드에 캘린더 아이콘이 아예 안 보였음). 실제로 그려지기 직전에 없으면 다시 채운다.
    /// 버튼 종류는 반드시 Combo여야 한다 - 처음엔 Glyph로 잘못 넣었는데(2026-09-09), Glyph는
    /// Button.Image를 직접 지정 안 하면 아무것도 안 그려져서 오히려 버튼이 통째로 안 보이는
    /// 결과가 됐다(실제로 겪음). DevExpress DateEdit 기본 생성자가 실제로 추가하는 버튼도
    /// Combo 타입이라는 걸 리플렉션으로 직접 확인했다.</summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (Properties.Buttons.Count == 0)
        {
            Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        }
    }

    /// <summary>편집 중 Enter를 누르면 다음 컨트롤로 포커스를 이동한다(2026-09-10 요청) - 캘린더
    /// 팝업이 떠 있을 때는 그 팝업의 기본 동작(선택한 날짜 확정+팝업 닫기)을 그대로 둬야 하므로
    /// IsPopupOpen일 때는 손대지 않는다.
    ///
    /// 두 번 잘못 시도한 끝에 이 방식으로 정착했다(2026-09-10 실제로 겪음, 자동화 테스트로 직접
    /// 검증):
    /// 1차: ProcessDialogKey 오버라이드 - 전혀 반응 없음. DevExpress BaseEdit이 IsInputKey를
    ///    오버라이드해서 Enter를 "일반 입력키"로 먼저 가져가버려 ProcessDialogKey 경로 자체를
    ///    안 탄다(리플렉션으로 확인).
    /// 2차: KeyDown 이벤트 + Parent.SelectNextControl() 직접 호출 - 포커스는 다음 컨트롤로
    ///    넘어가지만(SelectNextControl 자체는 성공), 타이핑한 텍스트("20260915")가 날짜로
    ///    변환되지 않고 그대로 남았다. SelectNextControl은 .NET 포커스만 옮길 뿐, DevExpress
    ///    에디터가 Text->EditValue로 값을 확정하는 내부 처리(Leave/Validate 경로에서 일어남)를
    ///    안 거치기 때문이다.
    /// 3차(최종): SendKeys.Send("{TAB}")로 실제 Tab 키 입력을 흉내낸다 - OS 메시지 큐를 통해
    ///    정상적인 Tab 포커스이동 경로를 그대로 타므로, 이미 잘 동작하던 Tab 키와 완전히 같은
    ///    절차(Leave/Validate 포함)를 거쳐 값 확정+포커스이동이 모두 정상적으로 일어난다.</summary>
    private void DateEditWyn_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || IsPopupOpen) return;

        e.Handled = true;
        e.SuppressKeyPress = true; // Enter가 그대로 전달돼 삑 소리/줄바꿈이 나는 걸 막는다.
        SendKeys.Send("{TAB}");
    }

    private void Properties_ParseEditValue(object? sender, ConvertEditValueEventArgs e) => SmartDateParser.Handle(e);

    /// <summary>
    /// DB의 VARCHAR(8) "yyyyMMdd" 감사컬럼(ent_date/open_date/ret_date 등, WYNLAB 전체의 날짜
    /// 저장 관례)과 직접 주고받는 속성 - 화면마다 .Text(문화권 표시형식이라 10자짜리
    /// "2026-09-11"이 나와서 8자 컬럼에 잘림)나 DateTime.TryParse(구분자 없는 8자리 문자열을
    /// 못 읽어서 항상 실패)를 직접 쓰다가 같은 버그(저장 시 잘림/조회 시 안 채워짐)를 화면마다
    /// 반복해서 겪었다(frmAcc/frmCust/frmEMP, 2026-09-11) - 이 속성 하나로 통일해서 화면
    /// 코드에서 DateTime 변환/포맷을 다시 손으로 만들 필요가 없게 한다.
    ///
    /// get은 EditValue가 있으면 항상 "yyyyMMdd"(8자) 문자열, 없으면 null. set은 "yyyyMMdd"
    /// 문자열이나 null을 받아 EditValue(DateTime?)로 바꿔 넣는다 - 형식이 안 맞으면 조용히
    /// null로 둔다(빈 값 취급, 화면이 깨지지 않게).
    /// </summary>
    [Browsable(false)]
    public string? YyyyMmDd
    {
        get => EditValue is DateTime date ? date.ToString("yyyyMMdd") : null;
        set => EditValue = DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : (DateTime?)null;
    }
}
