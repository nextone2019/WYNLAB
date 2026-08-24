using System.Drawing;

namespace WYNLAB.Base;

public enum ToastKind
{
    Success,
    Info,
    Warning
}

/// <summary>
/// 저장/삭제 성공처럼 "확인만 누르면 되는" 가벼운 알림을 위한 비모달 토스트.
/// 지금까지는 이런 성공 확인조차 AppMessageBox(클릭해서 닫아야 하는 모달)로 띄우거나
/// 아예 아무 표시 없이 조용히 끝났는데, 둘 다 좋은 UX가 아니라서 도입했다 - 에러/확인질문
/// (Yes/No)처럼 사용자가 반드시 인지해야 하는 것만 계속 AppMessageBox(모달)로 남기고,
/// "그냥 잘 됐다는 걸 알려주는" 성공 알림은 화면 우측 하단에 잠깐 떴다가 저절로 사라지는
/// 토스트로 대체한다.
///
/// BaseForm과 마찬가지로 WYNLAB.BaseForm에 두어 어느 업무모듈 DLL에서든 호출할 수 있게 했다.
/// </summary>
public static class Toast
{
    private static readonly List<ToastForm> _active = new();

    public static void Show(string message, ToastKind kind = ToastKind.Success)
    {
        var toast = new ToastForm(message, kind, _active.Count);
        toast.FormClosed += (s, e) => _active.Remove(toast);
        _active.Add(toast);
        toast.Show();
    }
}

internal class ToastForm : Form
{
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 2600 };

    // 클릭해도 포커스를 뺏지 않아야(WS_EX_NOACTIVATE) 지금 작업 중인 그리드/입력필드의
    // 포커스가 토스트 때문에 끊기지 않는다. ShowWithoutActivation과 세트로 필요.
    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            const int WS_EX_NOACTIVATE = 0x08000000;
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_NOACTIVATE;
            return cp;
        }
    }

    public ToastForm(string message, ToastKind kind, int stackIndex)
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = false;
        TopMost = true;
        Size = new Size(340, 52);

        BackColor = kind switch
        {
            ToastKind.Success => Color.FromArgb(46, 125, 79),
            ToastKind.Warning => Color.FromArgb(196, 130, 20),
            _ => Color.FromArgb(55, 71, 79)
        };

        var lbl = new Label
        {
            Text = message,
            ForeColor = Color.White,
            Font = AppFonts.Body,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(18, 0, 18, 0),
            Cursor = Cursors.Hand
        };
        Controls.Add(lbl);

        var workingArea = Screen.PrimaryScreen!.WorkingArea;
        var x = workingArea.Right - Width - 24;
        var y = workingArea.Bottom - Height - 24 - stackIndex * (Height + 10);
        Location = new Point(x, y);

        void DismissNow(object? s, EventArgs e)
        {
            _timer.Stop();
            Close();
        }

        _timer.Tick += DismissNow;
        Load += (s, e) => _timer.Start();
        lbl.Click += DismissNow; // 클릭하면 바로 닫기 - 굳이 다 사라질 때까지 기다릴 필요 없게
        Click += DismissNow;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
