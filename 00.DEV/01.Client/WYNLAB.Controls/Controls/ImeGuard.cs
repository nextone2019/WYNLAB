using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 전각 입력 방어(2026-10-05). Windows 11 한글 IME가 입력 모드를 자동으로 "전각"으로 바꾸면 영문/숫자/공백이 전각 문자로 입력돼 글자 사이가
/// 벌어진 데이터가 등록되곤 했다(DevExpress 편집기에서 반복). 세 겹으로 막는다:
///  1) <see cref="InstallGlobalFilter"/> - 앱 전체 메시지 필터가 키 입력(WM_CHAR)의 전각 영문/숫자/기호/공백을 그 자리에서 반각 문자로 바꾼다
///     (화면/컨트롤과 무관하게 모든 입력창, 팝업 포함).
///  2) <see cref="Attach"/> - 화면(BaseForm)이 열릴 때 아래 편집기들에 걸어서, 붙여넣기 등으로 전각이 들어온 값도 EditValueChanging에서 반각으로 바꾸고,
///     편집기에 들어갈 때(Enter) IME 전각 모드를 꺼 둔다.
///  3) 서버 - HalfWidthStringConverter가 요청의 모든 문자열을 다시 한 번 정규화한다(마지막 방어선).
/// 규칙은 서버와 같다(<see cref="TextNormalizer"/>) - 한글/한자 등 다른 문자는 건드리지 않는다.
/// </summary>
public static class ImeGuard
{
    private const int WM_CHAR = 0x0102;
    private const int IME_CMODE_FULLSHAPE = 0x0008;

    [DllImport("imm32.dll")] private static extern IntPtr ImmGetContext(IntPtr hWnd);
    [DllImport("imm32.dll")] private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
    [DllImport("imm32.dll")] private static extern bool ImmGetConversionStatus(IntPtr hIMC, out int conversion, out int sentence);
    [DllImport("imm32.dll")] private static extern bool ImmSetConversionStatus(IntPtr hIMC, int conversion, int sentence);
    [DllImport("user32.dll")] private static extern IntPtr GetFocus();
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern bool PostMessageW(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private static bool _filterInstalled;
    private static readonly ConditionalWeakTable<Control, object> Attached = new();
    private static readonly object Marker = new();

    // ------------------------------------------------------------ 1) 전역 키 입력 필터

    /// <summary>앱 시작 때 한 번 부른다(여러 번 불러도 한 번만 설치). UI 스레드의 메시지 루프에서 동작한다.</summary>
    public static void InstallGlobalFilter()
    {
        if (_filterInstalled) return;
        _filterInstalled = true;
        Application.AddMessageFilter(new FullWidthCharFilter());
    }

    private sealed class FullWidthCharFilter : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_CHAR) return false;

            var code = (int)m.WParam;
            if (code > 0xFFFF || !TextNormalizer.IsFullWidth((char)code)) return false;

            // WinForms는 메시지 필터가 메시지 내용을 고쳐도 반영하지 않는다(내부 전용 인터페이스만 허용) - 그래서 전각 키 입력은 삼키고,
            // 같은 창에 반각 문자 입력을 다시 보낸다(다시 들어온 반각 문자는 위 검사에서 그대로 통과한다).
            PostMessageW(m.HWnd, WM_CHAR, (IntPtr)TextNormalizer.ToHalfWidth((char)code), m.LParam);
            return true;
        }
    }

    // ------------------------------------------------------------ 2) 편집기 연결

    /// <summary>root 아래 모든 DevExpress 편집기(TextEdit/MemoEdit/ButtonEdit/LookUp...)에 전각 방어를 건다. 나중에 추가되는 컨트롤도 따라간다.</summary>
    public static void Attach(Control? root)
    {
        if (root == null || Attached.TryGetValue(root, out _)) return;
        Attached.Add(root, Marker);

        if (root is BaseEdit edit)
        {
            edit.EditValueChanging += Edit_EditValueChanging;
            edit.Enter += Edit_Enter;
        }

        root.ControlAdded += (s, e) => Attach(e.Control);
        foreach (Control child in root.Controls) Attach(child);
    }

    /// <summary>붙여넣기/프로그램이 넣은 값의 전각 문자를 반각으로 바꾼다(값이 문자열일 때만 - 날짜/숫자/체크는 그대로).</summary>
    public static void Edit_EditValueChanging(object? sender, ChangingEventArgs e)
    {
        if (e.NewValue is string text && TextNormalizer.NeedsFix(text))
            e.NewValue = TextNormalizer.ToHalfWidth(text);
    }

    private static void Edit_Enter(object? sender, EventArgs e)
    {
        if (sender is Control c && c.IsHandleCreated)
            c.BeginInvoke(new Action(ClearFullShapeOfFocusedWindow));   // 포커스가 안쪽 입력창으로 옮겨간 뒤에 처리한다
    }

    /// <summary>지금 포커스를 가진 창의 IME가 전각 모드면 반각으로 되돌린다(한/영 상태는 그대로).</summary>
    public static void ClearFullShapeOfFocusedWindow()
    {
        try
        {
            var hwnd = GetFocus();
            if (hwnd == IntPtr.Zero) return;

            var himc = ImmGetContext(hwnd);
            if (himc == IntPtr.Zero) return;
            try
            {
                if (ImmGetConversionStatus(himc, out var conversion, out var sentence) && (conversion & IME_CMODE_FULLSHAPE) != 0)
                    ImmSetConversionStatus(himc, conversion & ~IME_CMODE_FULLSHAPE, sentence);
            }
            finally { ImmReleaseContext(hwnd, himc); }
        }
        catch
        {
            // IME 조작 실패(IME 없음 등)는 입력 자체를 막을 이유가 아니다 - 필터/서버 정규화가 남아 있다.
        }
    }
}