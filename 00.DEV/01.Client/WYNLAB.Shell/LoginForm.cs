using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;

namespace WYNLAB.Shell;

/// <summary>
/// 로그인 화면.
/// - 타이틀바 없음(FormBorderStyle=None) - 시스템 X버튼이 아예 사라지고, 우측 상단의
///   커스텀 닫기 라벨(lblClose)로만 닫을 수 있음
/// - 좌측: 브랜드 영역(그라데이션 배경 + 로고 + 문구), 우측: 로그인 입력폼
/// - 서비스 선택(Development / Live) 콤보 - 선택 즉시 AppConfig.SwitchEnvironment 호출해서
///   로그인 요청이 실제로 어느 서버로 갈지 그 자리에서 바뀜
/// - 타이틀바가 없어서 창을 못 옮기니, 좌측 브랜드 영역을 드래그하면 창이 이동하도록 처리
/// </summary>
public partial class LoginForm : XtraForm
{
    private static readonly Color BrandColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

    // 로그인배경 이미지를 서버에서 받아오는 동안(LoadBrandingAsync) 화면은 이미 기본 파란
    // 브랜드컬러로 떠 있어서, 매번 로그인할 때마다 "파란 화면 -> 잠시 후 이미지로 교체"가
    // 눈에 띈다는 지적(2026-09-07)에 따라 마지막으로 받아온 이미지를 로컬에 캐시해둔다 -
    // 이 PC에서 이미지가 처음 설정되거나 바뀐 직후 한 번만 이 깜빡임이 보이고, 그 다음부터는
    // 캐시를 생성자에서 동기로 먼저 그린 뒤 화면을 띄우므로 즉시 올바른 이미지가 보인다.
    // %LocalAppData%\WYNLAB\...는 이 코드베이스의 로컬 캐시 표준 위치(AssetSyncer/ModuleLoader와 동일).
    private static readonly string BrandingCacheDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "Branding");
    private static readonly string LoginBackgroundCachePath = Path.Combine(BrandingCacheDir, "login-background.img");
    // 로고도 배경과 같은 증상(2026-09-07 지적) - 로고 없이 시작 -> 잠시 후 서버에서 받은 로고로
    // 교체되는 게 매번 눈에 띈다. 배경과 동일한 패턴(캐시 우선 동기 로드 + 다운로드 후 캐시 갱신)을
    // 그대로 적용한다.
    private static readonly string LoginLogoCachePath = Path.Combine(BrandingCacheDir, "login-logo.img");

    /// <summary>"아이디 저장" 체크 시 저장해두는 마지막 아이디 - 체크 해제하면 파일도 같이
    /// 지운다(2026-09-14 요청). last-environment.txt(AppConfig)와 같은 이유로 별도 파일로
    /// 둔다 - 비밀번호는 절대 여기 저장하지 않는다(SessionManager.RememberCredentials는 이미
    /// 메모리에서만 도는 재로그인용이고 디스크에 안 남는다).</summary>
    private static readonly string LastUserIdPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WYNLAB", "last-userid.txt");

    // 플레인 WinForms Panel을 씀 - RoundedPanel(배지)이 Parent.BackColor를 그대로 참조해서
    // 배경을 지우는데, DevExpress PanelControl은 실제 렌더링에 Appearance.BackColor를 쓰고
    // 베이스 .BackColor는 반영되지 않아 배지 모서리 색이 어긋나 보이는 문제가 있었다.
    private readonly Panel leftPanel = new();
    private readonly Panel badge = new();
    private readonly Panel companyBox = new();
    private readonly LabelControl titleLabel = new();

    /// <summary>frmSiteConfig(사이트환경설정 > 브랜딩)에서 개발자가 올려둔 로고 - 있으면 badge가
    /// LogoPainter의 기본 큐브 대신 이 이미지를 그린다.</summary>
    private Image? _customLogo;
    private readonly TextEdit txtUserId = new();
    private readonly TextEdit txtPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly CheckEdit chkRememberId = new() { Text = "아이디 저장" };
    private readonly ComboBoxEdit cboEnvironment = new();
    private readonly LabelControl lnkManageSites = new() { Text = "서비스 관리" };
    private readonly LabelControl lnkForgotPassword = new() { Text = "비밀번호를 잊으셨나요?" };
    private readonly SimpleButton btnLogin = new() { Text = "로그인" };
    private readonly LabelControl lblClose = new() { Text = "✕" };

    private readonly Dictionary<string, string> _envLabels = new()
    {
        ["Development"] = "Development",
        ["Production"] = "Live (Production)"
    };

    private Point _dragStart;
    private bool _dragging;

    /// <summary>로그인 성공 시 이 창을 닫기 직전에 띄운 스플래시 - Program.cs가 ShellForm을
    /// 다 띄운 뒤에 닫아준다. LoginForm 자신의 자식이 아니라 독립된 Form이어야 한다(클래스
    /// 상단 주석 참고 - 모달 대화상자 도중에 Hide()하면 ShowDialog()가 바로 반환돼버림).</summary>
    public SplashForm? Splash { get; private set; }

    public LoginForm()
    {
        Text = "WYN LAB 로그인";
        FormBorderStyle = FormBorderStyle.None;
        ControlBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        // 640x420 -> 560x380: 입력 필드 폭을 줄인 만큼(BuildRightPanel의 fieldWidth 참고) 창도
        // 같이 줄이고, 아래쪽에 남는 여백도 같이 정리했다(2026-09-06 요청).
        Size = new Size(560, 380);
        BackColor = Color.White;

        BuildLeftPanel();
        LoadCachedLoginBackground();
        LoadCachedLogo();
        BuildCloseButton();
        BuildRightPanel();
        LoadRememberedUserId();

        // 아이디가 이미 채워져 있으면 굳이 그 칸에 포커스를 줄 필요가 없다 - 바로 비밀번호를
        // 입력할 수 있게 한다(2026-09-14 요청). Focus()는 컨트롤 핸들이 만들어진 뒤에야 먹히므로
        // 생성자가 아니라 Shown에서 건다.
        if (chkRememberId.Checked)
            Shown += (s, e) => txtPassword.Focus();

        Controls.Add(leftPanel);

        // 로그인 "전"이라 토큰이 없으므로 익명 엔드포인트(api/site-config/public,
        // login-background/logo/favicon)만 부른다 - 로그인 성공 이후 값(색상 전체/첨부파일
        // 정책 등)은 SiteThemeSync가 별도로 처리한다(Program.cs 참고). 서버가 느리거나 접속
        // 안 돼도 로그인 자체는 막지 않아야 하므로 화면이 이미 뜬 뒤 비동기로 조용히 적용한다.
        Load += async (s, e) => await LoadBrandingAsync();
    }

    /// <summary>frmSiteConfig(사이트환경설정 > 브랜딩)에서 지정한 회사명/브랜드컬러/로그인배경/
    /// 로고/파비콘을 받아와 반영한다. 각 호출을 따로 try/catch하는 이유 - 이미지가 하나도
    /// 설정 안 되어 있으면 서버가 404를 주는데(SiteConfigController.GetImage), 그거 하나
    /// 때문에 나머지(회사명 등)까지 안 먹으면 안 된다.</summary>
    private async Task LoadBrandingAsync()
    {
        try
        {
            var branding = await ApiClient.GetAsync<PublicBrandingDto>("api/site-config/public");
            if (branding != null)
            {
                if (!string.IsNullOrWhiteSpace(branding.CompanyNm))
                {
                    titleLabel.Text = branding.CompanyNm;
                    Text = $"{branding.CompanyNm} 로그인";
                    companyBox.Invalidate(); // 화면 표시는 titleLabel이 아니라 companyBox.Paint가 titleLabel.Text를 읽어 그린다
                }
                // net48의 string.IsNullOrWhiteSpace엔 [NotNullWhen(false)]가 없어 위 가드로 이미
                // 확인했는데도 컴파일러가 못 알아채고 경고한다 - 안전하다.
                if (!string.IsNullOrWhiteSpace(branding.BrandColor))
                    ApplyBrandColor(ColorHelper.FromHex(branding.BrandColor!));
            }
        }
        catch { /* 서버 접속 불가 - appsettings.json 기본 브랜드로 계속 진행 */ }

        try
        {
            var (logoBytes, _) = await ApiClient.DownloadAsync("api/site-config/logo");
            if (logoBytes.Length > 0)
            {
                _customLogo = Image.FromStream(new MemoryStream(logoBytes));
                badge.Invalidate();
                SaveLogoCache(logoBytes);
            }
        }
        catch { /* 미설정(404) - LogoPainter 기본 큐브 유지 */ }

        try
        {
            var (bgBytes, _) = await ApiClient.DownloadAsync("api/site-config/login-background");
            if (bgBytes.Length > 0)
            {
                leftPanel.BackgroundImage = Image.FromStream(new MemoryStream(bgBytes));
                leftPanel.BackgroundImageLayout = ImageLayout.Stretch;
                SaveLoginBackgroundCache(bgBytes);
            }
        }
        catch { /* 미설정(404) - 단색 브랜드컬러 배경 유지 */ }

        try
        {
            var (faviconBytes, _) = await ApiClient.DownloadAsync("api/site-config/favicon");
            if (faviconBytes.Length > 0)
            {
                using var bitmap = new Bitmap(new MemoryStream(faviconBytes));
                Icon = Icon.FromHandle(bitmap.GetHicon()); // 업로드 원본이 .ico가 아니어도(png 등) 되도록 Bitmap.GetHicon() 경유
            }
        }
        catch { /* 미설정(404) - 기본 아이콘 유지 */ }
    }

    /// <summary>지난번 로그인 때 받아둔 로그인배경을 화면이 뜨기 전(생성자, 동기)에 미리 그린다 -
    /// LoadBrandingAsync의 서버 다운로드보다 항상 먼저 끝나므로, 캐시가 있으면 기본 파란
    /// 브랜드컬러가 아예 안 보이고 바로 올바른 이미지로 시작한다. 캐시가 없으면(이 PC에서
    /// 처음 로그인) 지금까지처럼 기본색으로 시작했다가 비동기 다운로드 후 교체된다.</summary>
    private void LoadCachedLoginBackground()
    {
        try
        {
            if (!File.Exists(LoginBackgroundCachePath)) return;
            var bytes = File.ReadAllBytes(LoginBackgroundCachePath);
            leftPanel.BackgroundImage = Image.FromStream(new MemoryStream(bytes));
            leftPanel.BackgroundImageLayout = ImageLayout.Stretch;
        }
        catch { /* 캐시가 손상됐어도 로그인 자체는 막으면 안 됨 - 기본 배경 유지 */ }
    }

    /// <summary>서버에서 새로 받은 로그인배경을 다음 로그인을 위해 로컬에 저장해둔다.
    /// Image.FromFile은 반환된 Image가 살아있는 동안 파일을 잠그므로(다음 갱신 시 덮어쓰기
    /// 실패 원인), 캐시는 항상 바이트 배열로 읽고 쓴다(LoadCachedLoginBackground와 동일).</summary>
    private static void SaveLoginBackgroundCache(byte[] bytes)
    {
        try
        {
            Directory.CreateDirectory(BrandingCacheDir);
            File.WriteAllBytes(LoginBackgroundCachePath, bytes);
        }
        catch { /* 캐시 저장 실패해도 로그인 화면 자체엔 영향 없음 - 조용히 무시 */ }
    }

    /// <summary>지난번에 받아둔 로고를 화면이 뜨기 전(생성자, 동기)에 미리 _customLogo에 채워둔다 -
    /// LoadCachedLoginBackground와 동일한 이유·패턴.</summary>
    private void LoadCachedLogo()
    {
        try
        {
            if (!File.Exists(LoginLogoCachePath)) return;
            var bytes = File.ReadAllBytes(LoginLogoCachePath);
            _customLogo = Image.FromStream(new MemoryStream(bytes));
        }
        catch { /* 캐시가 손상됐어도 로그인 자체는 막으면 안 됨 - 기본 큐브 로고 유지 */ }
    }

    /// <summary>서버에서 새로 받은 로고를 다음 로그인을 위해 로컬에 저장해둔다 -
    /// SaveLoginBackgroundCache와 동일한 이유·패턴(바이트 배열로만 다룸).</summary>
    private static void SaveLogoCache(byte[] bytes)
    {
        try
        {
            Directory.CreateDirectory(BrandingCacheDir);
            File.WriteAllBytes(LoginLogoCachePath, bytes);
        }
        catch { /* 캐시 저장 실패해도 로그인 화면 자체엔 영향 없음 - 조용히 무시 */ }
    }

    /// <summary>지난번에 "아이디 저장"을 체크하고 로그인했으면 그 아이디를 입력칸에 미리
    /// 채워주고 체크박스도 켜둔다 - 저장된 게 없으면(한 번도 체크 안 했거나 체크 해제로
    /// 지워짔음) 빈 칸/체크 해제 상태 그대로 둔다.</summary>
    private void LoadRememberedUserId()
    {
        try
        {
            if (!File.Exists(LastUserIdPath)) return;
            var saved = File.ReadAllText(LastUserIdPath).Trim();
            if (string.IsNullOrEmpty(saved)) return;

            txtUserId.Text = saved;
            chkRememberId.Checked = true;
        }
        catch { /* 캐시가 손상됐어도 로그인 자체는 막으면 안 됨 - 빈 칸으로 시작 */ }
    }

    /// <summary>로그인 성공 시점에 체크 상태를 반영한다 - 체크되어 있으면 방금 로그인한
    /// 아이디를 저장하고, 해제되어 있으면(이전에 저장해둔 게 있어도) 지운다.</summary>
    private static void SaveRememberedUserId(bool remember, string userId)
    {
        try
        {
            if (remember)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LastUserIdPath)!);
                File.WriteAllText(LastUserIdPath, userId);
            }
            else if (File.Exists(LastUserIdPath))
            {
                File.Delete(LastUserIdPath);
            }
        }
        catch { /* 저장/삭제 실패해도 로그인 자체엔 영향 없음 - 조용히 무시 */ }
    }

    /// <summary>서버 브랜드컬러로 바꿔야 하는 컨트롤들을 한 번에 갱신한다. 클래스 상단의
    /// static readonly BrandColor(appsettings.json 값)는 재대입이 안 되므로(그리고 서버가
    /// 없을 때의 기본값 계산에 여전히 필요하므로) 그대로 두고, 실제로 그 색을 쓰는 컨트롤들만
    /// 여기서 다시 칠한다.</summary>
    private void ApplyBrandColor(Color color)
    {
        leftPanel.BackColor = color;
        btnLogin.Appearance.BackColor = color;
        lnkForgotPassword.Appearance.ForeColor = color;
        lnkManageSites.Appearance.ForeColor = color;
    }

    /// <summary>
    /// 다이얼로그 전체 테두리를 얇게 그려서 배경과 경계를 분명하게 함
    /// (AppMessageBox/SplashForm과 같은 패턴 - 이 앱의 모든 커스텀 다이얼로그가 공유하는 규칙).
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(225, 226, 230));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    /// <summary>
    /// 좌측 브랜드 영역 - 단색 배경(예전엔 그라데이션이었는데, 플랫한 단색이 더 절제되고
    /// 고급스러워 보인다는 방향으로 정리) + 둥근 배지 로고 + 자간을 띄운 태그라인.
    /// 드래그로 창 이동 가능.
    /// </summary>
    private void BuildLeftPanel()
    {
        leftPanel.Dock = DockStyle.Left;
        leftPanel.Width = 240;
        leftPanel.BackColor = BrandColor;

        // 로고/회사명을 나란히 한 줄에 놓던 것(badge+titleLabel 인라인)을, 위아래로 쌓은
        // 두 자리로 바꿨다(2026-09-11 요청, 스크린샷으로 "로고 위치"/"회사명 위치" 지정).
        // 스크린샷의 파란 박스는 자리만 표시한 것이지 실제로 그 자리에 색을 칠해달라는
        // 뜻이 아니었다("배경색을 넣어 달라는게 아니고 위치를 보여주기위해서") - 카드 배경
        // 없이 로그인배경 이미지 위에 로고/텍스트만 바로 얹는다. 대신 어떤 배경 위에서도
        // 글자가 묻히지 않도록 옅은 그림자를 살짝 깐다.
        const int boxMargin = 24;
        var boxWidth = leftPanel.Width - boxMargin * 2;

        // BackColor=Transparent - 카드 배경 없이 로고/텍스트만 그리므로, 패널 전체 사각형이
        // leftPanel의 실제 배경(배경이미지)으로 비쳐 보여야 한다. 일반 Panel은 UserPaint
        // 없이도 BackColor=Transparent를 지원한다.
        badge.BackColor = Color.Transparent;
        badge.Size = new Size(boxWidth, 56);
        badge.Location = new Point(boxMargin, 48);
        badge.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 로고를 박스 크기에 맞춰 최대한 확대(zoom)해서 채운다 - 박스가 커지면 로고도
            // 같이 커지도록, 고정 크기(예전 30x30) 대신 가로세로비를 지키는 "맞춤" 계산을 쓴다.
            const int padding = 2;
            var availW = badge.Width - padding * 2;
            var availH = badge.Height - padding * 2;
            Rectangle logoRect;
            if (_customLogo != null)
            {
                var imgRatio = (float)_customLogo.Width / _customLogo.Height;
                var boxRatio = (float)availW / availH;
                var drawW = imgRatio > boxRatio ? availW : (int)(availH * imgRatio);
                var drawH = imgRatio > boxRatio ? (int)(availW / imgRatio) : availH;
                logoRect = new Rectangle(badge.Width / 2 - drawW / 2, badge.Height / 2 - drawH / 2, drawW, drawH);
                e.Graphics.DrawImage(_customLogo, logoRect);
            }
            else
            {
                // LogoPainter는 정사각형 rect를 전제로 하므로, 박스에 들어가는 가장 큰 정사각형을 쓴다.
                var size = Math.Min(availW, availH);
                logoRect = new Rectangle(badge.Width / 2 - size / 2, badge.Height / 2 - size / 2, size, size);
                LogoPainter.Draw(e.Graphics, logoRect, darkBackground: true);
            }
        };

        // 화면엔 그리지 않고 텍스트값만 들고 있는 용도로 남겨둔다 - companyBox.Paint가 이
        // Text를 읽어 직접 그린다(LoadBrandingAsync가 서버 회사명으로 갱신할 때도 이 컨트롤의
        // Text만 바꾸면 되도록, 기존 필드를 그대로 재사용).
        titleLabel.Text = "WYN LAB";

        companyBox.BackColor = Color.Transparent;
        companyBox.Size = new Size(boxWidth, 38);
        companyBox.Location = new Point(boxMargin, badge.Bottom + 6);
        companyBox.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            // 카드 배경이 없어졌으니, 배경이미지가 밝든 어둡든 글자가 묻히지 않도록 살짝
            // 아래로 어긋난 짙은 그림자를 먼저 그리고 그 위에 흰 글자를 덮는다.
            var shadowRect = companyBox.ClientRectangle;
            shadowRect.Offset(0, 1);
            using (var shadowBrush = new SolidBrush(Color.FromArgb(130, Color.Black)))
                e.Graphics.DrawString(titleLabel.Text, AppFonts.SubHeading, shadowBrush, shadowRect, sf);

            using var textBrush = new SolidBrush(Color.White);
            e.Graphics.DrawString(titleLabel.Text, AppFonts.SubHeading, textBrush, companyBox.ClientRectangle, sf);
        };

        leftPanel.Controls.Add(badge);
        leftPanel.Controls.Add(companyBox);

        EnableDrag(leftPanel);
        EnableDrag(companyBox);
    }

    /// <summary>우측 상단 커스텀 닫기 버튼 - 타이틀바가 없으므로 이게 유일한 닫기 수단</summary>
    private void BuildCloseButton()
    {
        lblClose.Location = new Point(Width - 44, 14);
        lblClose.AutoSizeMode = LabelAutoSizeMode.None;
        lblClose.Size = new Size(28, 28);
        lblClose.Appearance.ForeColor = Color.FromArgb(150, 150, 150);
        lblClose.Appearance.Font = AppFonts.SubHeading;
        lblClose.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblClose.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        lblClose.Cursor = Cursors.Hand;
        lblClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Controls.Add(lblClose);
        lblClose.BringToFront();
    }

    /// <summary>우측 로그인 입력 영역</summary>
    private void BuildRightPanel()
    {
        const int formX = 272;
        // 320 -> 260: 아이디/비밀번호 입력칸이 너무 길다는 지적(2026-09-06)으로 줄임. 폭이
        // 줄어든 만큼 창 전체 폭(Size)도 같이 줄였다 - 필드만 좁히고 창은 그대로 두면
        // 오른쪽에 어색한 빈 여백만 남는다.
        const int fieldWidth = 260;
        int y = 78;

        var welcomeLabel = new LabelControl
        {
            Text = "로그인",
            Location = new Point(formX, y),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(fieldWidth, 30)
        };
        welcomeLabel.Appearance.Font = AppFonts.Heading;
        Controls.Add(welcomeLabel);
        y += 40;

        AddFieldLabel("아이디", formX, ref y);
        txtUserId.Font = AppFonts.Body;
        txtUserId.Location = new Point(formX, y);
        txtUserId.Size = new Size(fieldWidth, 26);
        Controls.Add(txtUserId);
        y += 40;

        AddFieldLabel("비밀번호", formX, ref y);
        txtPassword.Font = AppFonts.Body;
        txtPassword.Location = new Point(formX, y);
        txtPassword.Size = new Size(fieldWidth, 26);
        Controls.Add(txtPassword);
        y += 30;

        // "비밀번호를 잊으셨나요?" 링크와 같은 줄, 왼쪽 자리에 배치(2026-09-14 요청).
        chkRememberId.Location = new Point(formX, y - 1);
        chkRememberId.Size = new Size(100, 18);
        chkRememberId.Font = AppFonts.Caption;
        Controls.Add(chkRememberId);

        lnkForgotPassword.Location = new Point(formX + fieldWidth - 130, y);
        lnkForgotPassword.AutoSizeMode = LabelAutoSizeMode.None;
        lnkForgotPassword.Size = new Size(130, 16);
        lnkForgotPassword.Appearance.ForeColor = BrandColor;
        lnkForgotPassword.Appearance.Font = AppFonts.Caption;
        lnkForgotPassword.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        lnkForgotPassword.Cursor = Cursors.Hand;
        lnkForgotPassword.Click += LnkForgotPassword_Click;
        Controls.Add(lnkForgotPassword);
        y += 24;

        AddFieldLabel("접속 서비스", formX, ref y);
        RefreshEnvironmentItems();
        cboEnvironment.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        cboEnvironment.Font = AppFonts.Body;
        cboEnvironment.Location = new Point(formX, y);
        cboEnvironment.Size = new Size(fieldWidth - 84, 26);
        cboEnvironment.SelectedIndexChanged += (s, e) =>
        {
            if (cboEnvironment.SelectedItem == null) return;
            var key = GetEnvKey((string)cboEnvironment.SelectedItem);
            AppConfig.SwitchEnvironment(key);
        };
        Controls.Add(cboEnvironment);

        // 개발자가 이 PC에서 접속할 서비스를 직접 추가/삭제하는 화면으로 - appsettings.json에
        // 내장 안 된 고객사 서버를 재배포 없이 늘려갈 때 씀(AppConfig.AddSite 참고).
        lnkManageSites.Location = new Point(formX + fieldWidth - 78, y + 5);
        lnkManageSites.AutoSizeMode = LabelAutoSizeMode.None;
        lnkManageSites.Size = new Size(78, 16);
        lnkManageSites.Appearance.ForeColor = BrandColor;
        lnkManageSites.Appearance.Font = AppFonts.Caption;
        lnkManageSites.Cursor = Cursors.Hand;
        lnkManageSites.Click += (s, e) =>
        {
            using var siteManager = new SiteManagerForm();
            var result = siteManager.ShowDialog(this);
            RefreshEnvironmentItems();
            // "선택"(또는 더블클릭)으로 고른 서비스가 있으면 그걸 로그인 드롭다운에 그대로
            // 반영한다 - RefreshEnvironmentItems가 먼저 목록을 다시 채운 뒤에 덮어써야
            // 방금 만든/이름바꾼 서비스도 목록에 있는 상태에서 선택된다(2026-09-14).
            if (result == DialogResult.OK && siteManager.SelectedSiteName != null)
                cboEnvironment.SelectedItem = GetEnvLabel(siteManager.SelectedSiteName);
        };
        Controls.Add(lnkManageSites);
        y += 46;

        btnLogin.Location = new Point(formX, y);
        btnLogin.Size = new Size(fieldWidth, 38);
        btnLogin.Appearance.BackColor = BrandColor;
        btnLogin.Appearance.ForeColor = Color.White;
        btnLogin.Appearance.Font = AppFonts.BodyBold;
        btnLogin.Appearance.Options.UseBackColor = true;
        btnLogin.Appearance.Options.UseForeColor = true;
        btnLogin.Click += BtnLogin_Click;
        Controls.Add(btnLogin);

        AcceptButton = btnLogin;
    }

    private void AddFieldLabel(string text, int x, ref int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 16) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        Controls.Add(lbl);
        y += 20;
    }

    private string GetEnvLabel(string env) => _envLabels.TryGetValue(env, out var label) ? label : env;
    private string GetEnvKey(string label) => _envLabels.FirstOrDefault(kv => kv.Value == label).Key ?? label;

    /// <summary>서비스 관리 화면에서 추가/삭제하고 돌아왔을 때 드롭다운 목록을 다시 채운다 -
    /// 지금 선택되어 있던 서비스가 삭제됐으면 AppConfig.CurrentEnvironment(기본값)로 되돌린다.</summary>
    private void RefreshEnvironmentItems()
    {
        var previouslySelectedKey = cboEnvironment.SelectedItem != null
            ? GetEnvKey((string)cboEnvironment.SelectedItem)
            : AppConfig.CurrentEnvironment;

        cboEnvironment.Properties.Items.Clear();
        cboEnvironment.Properties.Items.AddRange(AppConfig.AvailableEnvironments.Select(GetEnvLabel).ToArray());

        var selectedKey = AppConfig.AvailableEnvironments.Contains(previouslySelectedKey)
            ? previouslySelectedKey
            : AppConfig.CurrentEnvironment;
        cboEnvironment.SelectedItem = GetEnvLabel(selectedKey);
    }

    /// <summary>타이틀바가 없어서 창을 못 옮기므로, 지정한 컨트롤을 드래그하면 창이 같이 움직이도록 처리</summary>
    private void EnableDrag(Control control)
    {
        control.MouseDown += (s, e) => { _dragging = true; _dragStart = e.Location; };
        control.MouseMove += (s, e) =>
        {
            if (!_dragging) return;
            Location = new Point(Location.X + e.X - _dragStart.X, Location.Y + e.Y - _dragStart.Y);
        };
        control.MouseUp += (s, e) => { _dragging = false; };
    }

    private async void BtnLogin_Click(object? sender, EventArgs e)
    {
        var request = new LoginRequest
        {
            UserId = txtUserId.Text.Trim(),
            Password = txtPassword.Text,
            ClientVersion = Application.ProductVersion
        };

        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Password))
        {
            AppMessageBox.Show("아이디와 비밀번호를 입력해주세요.", "확인");
            return;
        }

        // 버튼 누르는 즉시(API 응답을 기다리기 전부터) 로그인창 위에 스플래시를 띄운다 -
        // 예전엔 로그인 성공 후 이 창을 닫기 직전에야 떴는데, 그러면 서버 응답을 기다리는
        // 동안(네트워크 왕복 구간)은 아무 피드백이 없었다(2026-09-05 요청).
        btnLogin.Enabled = false;
        ShowSplashOverThisForm();
        try
        {
            LoginResponse? response;
            try
            {
                response = await ApiClient.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
            }
            catch (HttpRequestException ex)
            {
                CloseSplash();
                AppMessageBox.Show($"서버에 연결할 수 없습니다. 네트워크 상태를 확인해주세요.\n{ex.Message}", "연결 오류");
                return;
            }

            if (response == null || !response.Success)
            {
                CloseSplash();
                AppMessageBox.Show(response?.Message ?? "로그인에 실패했습니다.", "로그인 실패");
                return;
            }

            if (response.RequirePasswordChange)
            {
                // 자격증명은 맞았지만(만료/관리자 초기화 등) 새 비밀번호를 먼저 설정해야 한다 -
                // 지금 발급된 토큰은 change-password 말고 다른 API를 못 부르는 제한된 토큰이다
                // (서버 MustChangePasswordFilter 참고). 비밀번호 변경창은 사용자가 직접 입력해야
                // 하는 대화상자라 스피너로 덮어두면 안 되므로, 그동안은 스플래시를 닫아둔다.
                ApiClient.SetAuthToken(response.AccessToken!);
                CloseSplash();

                using var changeForm = new ChangePasswordForm(request.Password);
                if (changeForm.ShowDialog(this) != DialogResult.OK) return; // 취소 - 로그인화면에 남는다

                // 비밀번호가 바뀌었으니 그 값으로 다시 로그인해서 정상 범위의 토큰을 받는다.
                // 방금 붙여둔 제한된(mustChangePwd=Y) 토큰을 먼저 떼어내야 한다 - 안 그러면
                // 로그인 자체가 [AllowAnonymous]인데도 그 토큰이 그대로 실려가서
                // MustChangePasswordFilter가 이 재로그인 요청까지 막아버린다(실제로 겪음).
                ApiClient.ClearAuthToken();
                ShowSplashOverThisForm();
                request.Password = changeForm.NewPassword;
                response = await ApiClient.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
                if (response == null || !response.Success)
                {
                    CloseSplash();
                    AppMessageBox.Show(response?.Message ?? "다시 로그인하는 중 오류가 발생했습니다.", "로그인 실패");
                    return;
                }
            }

            EnterAppAfterLogin(response, request.UserId, request.Password);
        }
        finally
        {
            btnLogin.Enabled = true;
        }
    }

    /// <summary>스플래시를 이 창(로그인창) 한가운데 겹쳐서 띄운다 - Show(this)로 소유 관계를
    /// 맺어서 로그인창과 같이 앞에 떠 있게 한다.</summary>
    private void ShowSplashOverThisForm()
    {
        Splash = new SplashForm { StartPosition = FormStartPosition.Manual };
        Splash.Location = new Point(
            Location.X + (Width - Splash.Width) / 2,
            Location.Y + (Height - Splash.Height) / 2);
        Splash.Show(this);
        Splash.Refresh();
    }

    private void CloseSplash()
    {
        Splash?.Close();
        Splash = null;
    }

    private void LnkForgotPassword_Click(object? sender, EventArgs e)
    {
        using var forgotForm = new ForgotPasswordForm();
        if (forgotForm.ShowDialog(this) != DialogResult.OK || forgotForm.Result == null) return;

        EnterAppAfterLogin(forgotForm.Result, forgotForm.ResultUserId!, forgotForm.ResultPassword!);
    }

    /// <summary>로그인 성공(일반 로그인 또는 비밀번호 찾기로 재로그인까지 끝난 경우) 공통 처리 -
    /// 세션 저장 + 이 창 닫기. 스플래시는 보통 BtnLogin_Click에서 버튼을 누른 시점에 이미
    /// 떠 있지만(ShowSplashOverThisForm), 비밀번호 찾기 흐름(LnkForgotPassword_Click)처럼 그
    /// 경로를 안 거치고 바로 여기로 오는 경우를 위해 없으면 여기서 만든다.
    /// userId/password도 같이 기억해둔다 - ShellForm의 서비스 전환 콤보가 로그인창을 다시
    /// 띄우지 않고 같은 자격증명으로 조용히 재로그인해보는 데 쓴다(2026-09-06 요청).</summary>
    private void EnterAppAfterLogin(LoginResponse response, string userId, string password)
    {
        SessionManager.Current.SignIn(response);
        SessionManager.Current.RememberCredentials(userId, password);
        ApiClient.SetAuthToken(response.AccessToken!);
        SaveRememberedUserId(chkRememberId.Checked, userId);

        if (Splash == null)
        {
            ShowSplashOverThisForm();
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
