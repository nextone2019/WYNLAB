using System.Runtime.InteropServices;

namespace WYNLAB.Base;

/// <summary>
/// 그리드 재바인딩 + 트리 ExpandAll처럼 무거운 레이아웃 재계산이 연달아 일어나는 구간에서,
/// 그 사이 화면이 다시 그려지며 깜빡이는 걸 막는다. SuspendLayout은 레이아웃 "계산"만 미룰 뿐
/// 화면 "그리기" 자체는 막지 않아서(컨트롤이 하나씩 다시 그려지는 순간들이 그대로 눈에 보임)
/// 이 문제엔 소용이 없다 - WM_SETREDRAW로 그리기 자체를 잠깐 꺼뒀다가, 끝나면 다시 켜고 한
/// 번에 다시 그리는 것만이 확실하다(수십 년간 쓰인 표준 WinForms 관용구, .NET엔 기본 API로
/// 없어서 Win32를 직접 호출한다).
///
/// 반드시 Suspend/Resume 사이에서 예외가 나도 Resume이 호출되도록 try/finally로 감싸서 써야
/// 한다 - 안 그러면 그 폼이 영원히 안 그려지는 상태로 남는다.
/// </summary>
public static class DrawingSuspension
{
    private const int WM_SETREDRAW = 0x000B;

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, bool wParam, IntPtr lParam);

    public static void Suspend(Control control)
    {
        if (control.IsHandleCreated) SendMessage(control.Handle, WM_SETREDRAW, false, IntPtr.Zero);
    }

    public static void Resume(Control control)
    {
        if (!control.IsHandleCreated) return;
        SendMessage(control.Handle, WM_SETREDRAW, true, IntPtr.Zero);
        control.Invalidate();
        control.Refresh();
    }
}
