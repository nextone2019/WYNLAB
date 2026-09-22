using DevExpress.XtraEditors;
using WYNLAB.Base;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 버튼을 누르는 순간 로그인창 위에 떠서, 셸(ShellForm)이 실제로 화면에 표시된 직후까지
/// 이어지는 스플래시. LoginForm과 생명주기가 독립적인 별개의 Form이어야 한다 - LoginForm은
/// ShowDialog()로 뜬 모달이라 도중에 Hide()하면 그 시점에 ShowDialog()가 바로 반환돼버리기
/// 때문(WinForms 동작).
///
/// 2026-09 리디자인(2차): 처음엔 TransparencyKey(특정 색만 뚫어 보이게 하는 방식)로 "투명창"을
/// 흉내냈는데, 그 방식은 로고/스피너의 안티에일리어싱된 가장자리가 키 색과 섞이면서 그 색
/// 기운(마젠타를 썼더니 붉은기)이 테두리에 비치고 - 특히 원형(스피너)처럼 곡선 전체가
/// 안티에일리어싱되는 도형에서는 가장자리 전체가 얼룩덜룩 깨져 보였다(실제로 겪음, "이미지도
/// 깨져"). 진짜 원인은 "이분법적 뚫림"이라 가장자리의 반투명 픽셀을 표현할 방법이 없었던 것 -
/// 그래서 이번엔 Windows의 레이어드 윈도우(WS_EX_LAYERED + UpdateLayeredWindow)를 직접 써서
/// 픽셀 단위로 진짜 알파값을 가진 비트맵을 그린다. 자식 컨트롤을 하나도 안 쓰고(레이어드
/// 윈도우는 일반적인 자식 컨트롤 WM_PAINT 합성과 안 맞물린다) 로고+스피너+텍스트 전부를
/// RenderFrame()에서 직접 그려서 통째로 밀어넣는다.
/// </summary>
public class SplashForm : XtraForm
{
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 25 };
    private readonly Color _accent = ColorHelper.FromHex(AppConfig.ToolbarColor);
    private float _angle;

    private const int SpinnerSize = 52;
    private const int LogoSize = 24;

    public SplashForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        ShowInTaskbar = false;
        Size = new Size(190, 116); // 문구가 두 줄("WYNLAB 서비스 접속 중입니다." / "잠시만 기다려 주십시오.")로 늘어나 폭/높이를 키웠다

        _timer.Tick += (s, e) => { _angle = (_angle + 9f) % 360f; RenderFrame(); };
    }

    // WS_EX_LAYERED를 켜야 UpdateLayeredWindow로 픽셀 단위 알파를 직접 밀어넣을 수 있다.
    // Form.Opacity/TransparencyKey 같은 WinForms 자체 레이어드 윈도우 경로와는 절대 같이
    // 쓰면 안 된다(내부적으로 서로 다른 방식으로 같은 WS_EX_LAYERED를 관리하려 들어 충돌한다) -
    // 그래서 이 클래스는 그 두 속성을 건드리지 않는다.
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x00080000; // WS_EX_LAYERED
            return cp;
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        RenderFrame();
        _timer.Start();
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        _timer.Stop();
        base.OnHandleDestroyed(e);
    }

    /// <summary>로고를 감싸고 도는 링 + 로고 + 캡션 한 프레임을 그려서 레이어드 윈도우로 밀어넣는다.
    /// 매 타이머 틱(25ms)마다 각도만 바꿔 다시 호출된다.</summary>
    private void RenderFrame()
    {
        var w = Width;
        var h = Height;

        var bmi = new NativeMethods.BITMAPINFO
        {
            // 반드시 BITMAPINFOHEADER 하나의 실제 크기(40)여야 한다 - 이 구조체엔 그 뒤에
            // colors 필드가 하나 더 있어서(32bpp는 팔레트가 없는데도 구조체 크기를 맞추려고
            // 넣어둔 필드) Marshal.SizeOf<BITMAPINFO>()를 그대로 쓰면 44가 나온다. Windows는
            // biSize로 헤더 버전을 판별하는데, 40도 44도 아닌 애매한 값이 아니라 "40이 아닌
            // 값"이면 표준 BITMAPINFOHEADER로 안 보고 다른 버전(BITMAPV4HEADER 등)의 헤더로
            // 오인해서 이후 필드를 전부 엉뚱하게 해석해버린다 - 스피너/로고/글자가 깨져 보이던
            // 진짜 원인이 이것이었다(2026-09-06 실제 발견).
            biSize = 40,
            biWidth = w,
            biHeight = -h, // 음수 = top-down(위에서 아래로) DIB - GDI+ Bitmap이 기대하는 순서와 맞춘다
            biPlanes = 1,
            biBitCount = 32,
            biCompression = 0 // BI_RGB
        };

        var screenDc = NativeMethods.GetDC(IntPtr.Zero);
        var memDc = NativeMethods.CreateCompatibleDC(screenDc);
        var hBitmap = NativeMethods.CreateDIBSection(memDc, ref bmi, 0, out var bits, IntPtr.Zero, 0);
        var oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);
        try
        {
            if (hBitmap == IntPtr.Zero || bits == IntPtr.Zero) return;

            // CreateDIBSection이 내준 메모리를 그대로 GDI+ Bitmap으로 감싸서 평소처럼 Graphics로
            // 그린다 - Format32bppPArgb로 감싸면 GDI+가 그리는 동안 알아서 "이미 알파를 곱한"
            // 값으로 저장해준다(UpdateLayeredWindow의 AC_SRC_ALPHA가 요구하는 형식과 정확히
            // 일치 - 이 부분을 안 맞추면 가장자리가 시커멓게 뜨거나 색이 왜곡된다).
            using var bmp = new Bitmap(w, h, w * 4, PixelFormat.Format32bppPArgb, bits);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                DrawContent(g, w, h);
            }

            var topPos = new NativeMethods.POINT { x = Left, y = Top };
            var size = new NativeMethods.SIZE { cx = w, cy = h };
            var srcPos = new NativeMethods.POINT { x = 0, y = 0 };
            var blend = new NativeMethods.BLENDFUNCTION
            {
                BlendOp = 0, // AC_SRC_OVER
                BlendFlags = 0,
                SourceConstantAlpha = 255,
                AlphaFormat = 1 // AC_SRC_ALPHA
            };
            NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref srcPos, 0, ref blend, 2 /* ULW_ALPHA */);
        }
        finally
        {
            NativeMethods.SelectObject(memDc, oldBitmap);
            NativeMethods.DeleteObject(hBitmap);
            NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
            NativeMethods.DeleteDC(memDc);
        }
    }

    private void DrawContent(Graphics g, int w, int h)
    {
        // 완전히 투명하기만 하면 뭐가 떠 있는지 잘 안 보인다는 지적(2026-09-06)으로, 카드
        // 형태의 반투명 짙은 배경을 하나 깐다 - 진짜 알파값을 쓰는 레이어드 윈도우라서(클래스
        // 설명 참고) 예전 TransparencyKey 방식과 달리 이런 "군데군데 비치는 카드"가 가장자리
        // 얼룩 없이 깔끔하게 나온다.
        using (var cardPath = RoundedRect(new Rectangle(0, 0, w, h), 16))
        using (var cardBrush = new SolidBrush(Color.FromArgb(195, 22, 27, 38)))
            g.FillPath(cardBrush, cardPath);

        // 카드 배경이 짙어졌으니, 그 위에 올라가는 로고/스피너/글자도 밝은 톤으로 맞춘다 -
        // 예전(옅은 배경 가정)엔 로고 그림자를 밝은 배경용(darkBackground:false)으로 그렸는데,
        // 이제 배경 자체가 짙으므로 로고도 짙은 배경용 그림자로 바꾼다.
        var spinnerRect = new Rectangle((w - SpinnerSize) / 2, 6, SpinnerSize, SpinnerSize);
        var trackRect = Rectangle.Inflate(spinnerRect, -2, -2);

        using (var trackPen = new Pen(Color.FromArgb(70, 255, 255, 255), 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            g.DrawEllipse(trackPen, trackRect);
        using (var pen = new Pen(Color.FromArgb(255, 91, 156, 255), 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            g.DrawArc(pen, trackRect, _angle, 100);

        var logoRect = new Rectangle(
            spinnerRect.X + (spinnerRect.Width - LogoSize) / 2,
            spinnerRect.Y + (spinnerRect.Height - LogoSize) / 2,
            LogoSize, LogoSize);
        LogoPainter.Draw(g, logoRect, darkBackground: true);

        var textRect = new RectangleF(4, spinnerRect.Bottom + 6, w - 8, 34);
        using var textBrush = new SolidBrush(Color.FromArgb(230, 232, 236));
        using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("WYNLAB 서비스 접속 중입니다.\n잠시만 기다려 주십시오.", AppFonts.Caption, textBrush, textRect, sf);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>레이어드 윈도우(WS_EX_LAYERED + UpdateLayeredWindow)에 필요한 최소한의 Win32 P/Invoke
    /// 묶음 - .NET Framework/WinForms엔 이 API가 감싸져 있지 않아 직접 선언해야 한다.</summary>
    private static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint usage, out IntPtr bits, IntPtr hSection, uint offset);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize,
            IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int x; public int y; }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE { public int cx; public int cy; }

        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
            public int colors; // 32bpp BI_RGB엔 팔레트가 없지만, 구조체 크기를 맞추려면 필드가 있어야 한다
        }
    }
}
