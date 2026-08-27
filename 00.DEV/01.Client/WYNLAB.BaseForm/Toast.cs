using System.Drawing;
using System.Drawing.Drawing2D;

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
/// "그냥 잘 됐다는 걸 알려주는" 성공 알림은 화면 가운데에 잠깐 떴다가 저절로 사라지는
/// 토스트로 대체한다.
///
/// 원래는 화면 우측 하단에 진한 색(성공=녹색 등) 배경으로 떴는데, 너무 튀어 보인다는 피드백으로
/// AppMessageBox와 같은 카드 스타일(흰 배경 + 옅은 테두리 + 작은 원형 배지)로 바꾸고 화면
/// 정중앙으로 옮겼다.
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
    // AppMessageBox와 같은 톤 - 이 앱 대부분의 화면이 흰 배경이라 토스트도 순백이면 뒤 화면과
    // 구분이 안 돼서 옅게 톤 다운했다.
    private static readonly Color SurfaceColor = Color.FromArgb(250, 250, 251);
    private static readonly Color BorderColor = Color.FromArgb(170, 174, 182);
    private static readonly Color TextColor = Color.FromArgb(55, 55, 55);

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

        // AppMessageBox와 같은 트릭 - 폼 배경을 테두리색으로 칠하고 안쪽 콘텐츠를 1px 밀어넣어
        // 그 사이 남는 띠가 곧 테두리가 되게 한다(따로 그릴 필요가 없어 갱신 자국도 안 남는다).
        BackColor = BorderColor;
        Padding = new Padding(1);

        var (badgeColor, glyphColor) = GetColors(kind);

        var body = new Panel { Dock = DockStyle.Fill, BackColor = SurfaceColor };
        Controls.Add(body);

        const int badgeSize = 22;
        var badge = new ToastBadge
        {
            Kind = kind,
            BadgeColor = badgeColor,
            GlyphColor = glyphColor,
            Size = new Size(badgeSize, badgeSize),
            Location = new Point(16, (body.Height - badgeSize) / 2)
        };
        body.Controls.Add(badge);

        var lbl = new Label
        {
            Text = message,
            ForeColor = TextColor,
            Font = AppFonts.Body,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(50, 0, 16, 0),
            Cursor = Cursors.Hand,
            BackColor = Color.Transparent
        };
        body.Controls.Add(lbl);

        // 화면 정중앙에 띄운다 - 여러 개가 겹치면 중앙에서 아래로 하나씩 쌓는다.
        var workingArea = Screen.PrimaryScreen!.WorkingArea;
        var x = workingArea.Left + (workingArea.Width - Width) / 2;
        var y = workingArea.Top + (workingArea.Height - Height) / 2 + stackIndex * (Height + 10);
        Location = new Point(x, y);

        void DismissNow(object? s, EventArgs e)
        {
            _timer.Stop();
            Close();
        }

        _timer.Tick += DismissNow;
        Load += (s, e) => _timer.Start();
        lbl.Click += DismissNow; // 클릭하면 바로 닫기 - 굳이 다 사라질 때까지 기다릴 필요 없게
        body.Click += DismissNow;
    }

    /// <summary>배지 배경은 옅게 톤 다운한 색, 글리프(체크/느낌표/i)는 그보다 짙은 같은 계열
    /// 색으로 - 예전처럼 진한 색으로 토스트 전체를 채우지 않아도 종류가 한눈에 구분된다.</summary>
    private static (Color badge, Color glyph) GetColors(ToastKind kind) => kind switch
    {
        ToastKind.Success => (Color.FromArgb(224, 240, 229), Color.FromArgb(58, 137, 96)),
        ToastKind.Warning => (Color.FromArgb(248, 234, 210), Color.FromArgb(184, 132, 42)),
        _ => (Color.FromArgb(222, 232, 247), Color.FromArgb(70, 110, 176))
    };

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}

/// <summary>토스트 왼쪽의 작은 원형 아이콘 배지 - 성공(체크)/경고(느낌표)/안내(i)를 폰트 글리프
/// 대신 선으로 직접 그린다. AppMessageBox.CloseGlyph와 같은 이유 - 맑은 고딕엔 체크(✓) 문자가
/// 없어서 윈도우가 다른 폰트로 대체해 그리면 크기·위치가 어긋난다(실제로 겪음).</summary>
internal class ToastBadge : Panel
{
    public ToastKind Kind { get; set; }
    public Color BadgeColor { get; set; }
    public Color GlyphColor { get; set; }

    public ToastBadge()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Color.White);

        using (var badgeBrush = new SolidBrush(BadgeColor))
            g.FillEllipse(badgeBrush, 0, 0, Width - 1, Height - 1);

        using var pen = new Pen(GlyphColor, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        using var dotBrush = new SolidBrush(GlyphColor);
        switch (Kind)
        {
            case ToastKind.Success:
                g.DrawLines(pen, new[] { new PointF(6f, 11.5f), new PointF(9.5f, 15f), new PointF(16f, 7f) });
                break;
            case ToastKind.Warning:
                g.DrawLine(pen, Width / 2f, 6f, Width / 2f, 13f);
                g.FillEllipse(dotBrush, Width / 2f - 1.2f, 15.5f, 2.4f, 2.4f);
                break;
            default:
                g.FillEllipse(dotBrush, Width / 2f - 1.2f, 5.5f, 2.4f, 2.4f);
                g.DrawLine(pen, Width / 2f, 9f, Width / 2f, 16f);
                break;
        }
        base.OnPaint(e);
    }
}
