using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM;

/// <summary>
/// 사이트(설치) 환경설정 - Developer Tool 메뉴 전용(TSMMENUAUTH로 개발자 계정에만 권한을
/// 부여하는 것을 전제). 회사당 딱 한 줄(TSMSITECONFIG)만 있는 설정이라, 다른 화면들의
/// grd1(목록)+panData(상세) 마스터-디테일 구조 대신 좌측 카테고리 목록 + 우측 폼 전환
/// 방식을 쓴다(frmMenu.cs와 같은 이유로 전부 코드로 직접 구성 - VS 디자이너 연동이 필요
/// 없는 화면).
///
/// 카테고리 5개: 브랜딩(로그인배경/로고/파비콘 이미지 포함) / 메일발신(SMTP, 비밀번호 제외) /
/// 첨부파일정책 / 비밀번호정책 / 색상값 정의(배포 시 개발자가 1회 지정하는 UiTheme 오버라이드).
/// 설계 배경은 project_wynlab_site_config_screen 메모리 참고.
///
/// 이번 1차 구현 범위: 화면 자체(조회/저장)만. 이 값들을 실제 소비하는 쪽(LoginForm 배경이미지
/// 적용, AuthService 비밀번호 정책 검증, EmailService SMTP 반영, FilesController 확장자/크기
/// 정책 반영)은 아직 안 걸었다 - 별도로 이어서 작업 필요.
/// </summary>
public class frmSiteConfig : BaseForm
{
    private const long MaxImagePickBytes = 5 * 1024 * 1024;

    private readonly Panel navPanel = new() { Dock = DockStyle.Left, Width = 160 };
    private readonly Panel contentPanel = new() { Dock = DockStyle.Fill };
    private readonly Dictionary<string, Panel> _categoryPanels = new();
    private readonly Dictionary<string, LabelControl> _navItems = new();
    private string _activeCategory = "brand";

    // 브랜딩
    private readonly TextEdit txtCompanyNm = new();
    private readonly PictureEdit picLoginBg = new();
    private readonly PictureEdit picLogo = new();
    private readonly PictureEdit picFavicon = new();
    private byte[]? _loginBgBytes, _logoBytes, _faviconBytes;
    private bool _loginBgChanged, _logoChanged, _faviconChanged;

    // 메일 발신
    private readonly TextEdit txtSmtpHost = new();
    private readonly SpinEdit spnSmtpPort = new() { Properties = { MinValue = 1, MaxValue = 65535 } };
    private readonly TextEdit txtSmtpUsername = new();
    private readonly TextEdit txtFromAddress = new();
    private readonly TextEdit txtFromDisplayNm = new();

    // 첨부파일 정책
    private readonly TextEdit txtBlockExtensions = new();
    private readonly SpinEdit spnMaxSizeMb = new() { Properties = { MinValue = 1, MaxValue = 10240 } };

    // 비밀번호 정책
    private readonly SpinEdit spnExpireDays = new() { Properties = { MinValue = 0, MaxValue = 3650 } };
    private readonly SpinEdit spnLockThreshold = new() { Properties = { MinValue = 1, MaxValue = 100 } };
    private readonly SpinEdit spnResetValidMin = new() { Properties = { MinValue = 1, MaxValue = 1440 } };
    private readonly SpinEdit spnMinLength = new() { Properties = { MinValue = 1, MaxValue = 100 } };
    private readonly CheckEdit chkRequireUpperLower = new() { Text = "영문 대소문자 포함" };
    private readonly CheckEdit chkRequireDigit = new() { Text = "숫자 포함" };
    private readonly CheckEdit chkRequireSpecial = new() { Text = "특수문자 포함" };
    private readonly RadioGroup rdoInitPwdPolicy = new();
    private readonly CheckEdit chkForceChangeOnFirstLogin = new() { Text = "최초 로그인 시 비밀번호 변경 강제" };

    // 세션(자리비움 잠금화면)
    private readonly SpinEdit spnIdleTimeoutMinutes = new() { Properties = { MinValue = 0, MaxValue = 1440 } };

    // 색상값 정의
    private readonly ColorEdit colRequiredField = new();
    private readonly ColorEdit colGridHeader = new();
    private readonly ColorEdit colGridFocusedRow = new();
    private readonly ColorEdit colBrand = new();
    private readonly ColorEdit colTreeGroup = new();
    private readonly ColorEdit colDivider = new();

    public frmSiteConfig()
    {
        Text = "사이트 환경설정";

        rdoInitPwdPolicy.Properties.Items.Add(new RadioGroupItem("USER_ID", "사번(USER_ID)와 동일"));
        rdoInitPwdPolicy.Properties.Items.Add(new RadioGroupItem("RANDOM", "임의 생성 후 안내"));

        BuildNav();
        BuildBrandPanel();
        BuildMailPanel();
        BuildFilePanel();
        BuildPasswordPanel();
        BuildScreensaverPanel();
        BuildColorPanel();

        Controls.Add(contentPanel);
        Controls.Add(navPanel);
        Controls.Add(BuildScreenHeader());

        // 개발자용 마우스오버 툴팁(BindingField) - 실제 적용은 BaseForm.ApplyBindingFieldTooltips가
        // 공통으로 처리한다(Session.IsDeveloper일 때만). 이 화면은 DB 컬럼을 직접 안 쓰고 API
        // DTO(SiteConfigDto) 프로퍼티로 주고받으므로, snake_case 컬럼명 대신 그 프로퍼티명을
        // 그대로 쓴다(2026-09-12 감사 - 원래 빠져있었음). 색상 선택 컨트롤(colXxx)은 BaseEdit이
        // 아니라서 Tag를 넣어도 ApplyBindingFieldTooltips가 안 잡는다 - 대상에서 제외.
        txtCompanyNm.Tag = new BindingFieldTag("CompanyNm");
        txtSmtpHost.Tag = new BindingFieldTag("SmtpHost");
        spnSmtpPort.Tag = new BindingFieldTag("SmtpPort");
        txtSmtpUsername.Tag = new BindingFieldTag("SmtpUsername");
        txtFromAddress.Tag = new BindingFieldTag("SmtpFromAddress");
        txtFromDisplayNm.Tag = new BindingFieldTag("SmtpFromDisplayNm");
        txtBlockExtensions.Tag = new BindingFieldTag("FileBlockExtensions");
        spnMaxSizeMb.Tag = new BindingFieldTag("FileMaxSizeMb");
        spnExpireDays.Tag = new BindingFieldTag("PwdExpireDays");
        spnLockThreshold.Tag = new BindingFieldTag("PwdLockThreshold");
        spnResetValidMin.Tag = new BindingFieldTag("PwdResetCodeValidMin");
        spnMinLength.Tag = new BindingFieldTag("PwdMinLength");
        chkRequireUpperLower.Tag = new BindingFieldTag("PwdRequireUpperLower");
        chkRequireDigit.Tag = new BindingFieldTag("PwdRequireDigit");
        chkRequireSpecial.Tag = new BindingFieldTag("PwdRequireSpecial");
        rdoInitPwdPolicy.Tag = new BindingFieldTag("InitPwdPolicy");
        chkForceChangeOnFirstLogin.Tag = new BindingFieldTag("ForceChangeOnFirstLogin");
        spnIdleTimeoutMinutes.Tag = new BindingFieldTag("IdleTimeoutMinutes");

        TrackDirty(contentPanel);

        ActivateCategory("brand");
        Load += async (s, e) => await QueryClick();
    }

    private void BuildNav()
    {
        navPanel.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.FromArgb(225, 225, 225) });

        var items = new (string Key, string Text)[]
        {
            ("brand", "브랜딩"),
            ("mail", "메일 발신"),
            ("file", "첨부파일 정책"),
            ("pwd", "비밀번호 정책"),
            ("screensaver", "화면보호기"),
            ("color", "색상값 정의"),
        };

        var y = 8;
        foreach (var (key, text) in items)
        {
            var item = new LabelControl
            {
                Text = text,
                Location = new Point(0, y),
                Size = new Size(160, 32),
                Padding = new Padding(14, 9, 0, 0),
                Cursor = Cursors.Hand,
            };
            item.Click += (s, e) => ActivateCategory(key);
            navPanel.Controls.Add(item);
            _navItems[key] = item;
            y += 32;
        }
    }

    private void ActivateCategory(string key)
    {
        _activeCategory = key;
        foreach (var kv in _navItems)
        {
            var active = kv.Key == key;
            kv.Value.Appearance.BackColor = active ? Color.FromArgb(230, 240, 255) : Color.Transparent;
            kv.Value.Appearance.ForeColor = active ? Color.FromArgb(24, 95, 220) : Color.FromArgb(70, 70, 70);
            kv.Value.Appearance.Options.UseBackColor = true;
            kv.Value.Appearance.Options.UseForeColor = true;
        }
        foreach (var kv in _categoryPanels)
            kv.Value.Visible = kv.Key == key;
    }

    private static LabelControl MakeLabel(string text, Point location) =>
        new() { Text = text, Location = location, AutoSize = true };

    private void BuildBrandPanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };

        panel.Controls.Add(MakeLabel("회사명", new Point(0, 4)));
        txtCompanyNm.Location = new Point(120, 0);
        txtCompanyNm.Size = new Size(220, 20);
        panel.Controls.Add(txtCompanyNm);

        AddImagePicker(panel, "로그인 배경이미지", new Point(0, 40), picLoginBg, new Size(180, 110),
            () => PickImage(bytes => { _loginBgBytes = bytes; _loginBgChanged = true; }, picLoginBg));
        AddImagePicker(panel, "로고 이미지", new Point(220, 40), picLogo, new Size(72, 72),
            () => PickImage(bytes => { _logoBytes = bytes; _logoChanged = true; }, picLogo));
        AddImagePicker(panel, "파비콘", new Point(340, 40), picFavicon, new Size(40, 40),
            () => PickImage(bytes => { _faviconBytes = bytes; _faviconChanged = true; }, picFavicon));

        contentPanel.Controls.Add(panel);
        _categoryPanels["brand"] = panel;
    }

    /// <summary>라벨 + 미리보기 썸네일 + "파일 선택" 버튼 한 묶음. onPick은 OpenFileDialog로 고른
    /// 파일을 읽어서 실제로 담을 필드(예: _logoBytes)에 넣는 콜백 - 저장은 폼 저장(SaveClick)
    /// 시점에 한꺼번에 한다(선택 즉시 서버로 안 올림 - "저장" 눌러야 반영).</summary>
    private static void AddImagePicker(Panel parent, string label, Point location, PictureEdit picture, Size pictureSize, Action onPick)
    {
        parent.Controls.Add(MakeLabel(label, new Point(location.X, location.Y)));

        picture.Location = new Point(location.X, location.Y + 20);
        picture.Size = pictureSize;
        picture.Properties.ShowMenu = false;
        picture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
        parent.Controls.Add(picture);

        var button = new ButtonWyn
        {
            Text = "파일 선택",
            Location = new Point(location.X, location.Y + 20 + pictureSize.Height + 6),
            Size = new Size(90, 26),
        };
        button.Click += (s, e) => onPick();
        parent.Controls.Add(button);
    }

    private void PickImage(Action<byte[]> onLoaded, PictureEdit preview)
    {
        using var dialog = new OpenFileDialog { Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.ico" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        var bytes = File.ReadAllBytes(dialog.FileName);
        if (bytes.Length > MaxImagePickBytes)
        {
            AppMessageBox.Show($"이미지 크기가 너무 큽니다(최대 {MaxImagePickBytes / 1024 / 1024}MB).", "확인");
            return;
        }

        onLoaded(bytes);
        using var stream = new MemoryStream(bytes);
        preview.Image = Image.FromStream(stream);
        IsDirty = true;
    }

    private void BuildMailPanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };

        panel.Controls.Add(MakeLabel("SMTP 계정 비밀번호는 서버 환경변수로만 관리합니다(이 화면에서 설정하지 않음).", new Point(0, 0)));

        var y = 30;
        AddRow(panel, "SMTP Host", txtSmtpHost, ref y, 220);
        AddRow(panel, "Port", spnSmtpPort, ref y, 100);
        AddRow(panel, "계정(UserName)", txtSmtpUsername, ref y, 220);
        AddRow(panel, "발신 이메일 주소", txtFromAddress, ref y, 220);
        AddRow(panel, "발신 표시이름", txtFromDisplayNm, ref y, 220);

        contentPanel.Controls.Add(panel);
        _categoryPanels["mail"] = panel;
    }

    private void BuildFilePanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };

        var y = 0;
        AddRow(panel, "차단 확장자(콤마 구분)", txtBlockExtensions, ref y, 280);
        AddRow(panel, "업로드 최대 크기(MB)", spnMaxSizeMb, ref y, 100);

        contentPanel.Controls.Add(panel);
        _categoryPanels["file"] = panel;
    }

    /// <summary>비밀번호 정책 - 예전엔 좌측 상단에 딱 붙어서 항목들이 죽 나열만 되어 있었는데,
    /// 여백이 너무 없고 어떤 항목끼리 관련된 건지 구분이 안 된다는 지적(2026-09-09)으로 세
    /// 그룹(GroupBoxWyn)으로 나눠 정리했다 - 만료/잠금 관련, 비밀번호 규칙 자체, 신규계정 정책.
    /// 자리비움 잠금(예전엔 이 화면 맨 아래 "세션" 항목 하나)은 별도 요청으로 아예 다른 챕터
    /// (화면보호기, BuildScreensaverPanel)로 분리했다.</summary>
    private void BuildPasswordPanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32), AutoScroll = true };
        const int groupWidth = 460;
        const int groupGap = 24;
        var y = 0;

        var grpExpiry = new GroupBoxWyn { Text = "만료 및 잠금", Location = new Point(0, y), Size = new Size(groupWidth, 148) };
        var gy1 = 34;
        AddRow(grpExpiry, "비밀번호 만료일수", spnExpireDays, ref gy1, 100, x: 16);
        AddRow(grpExpiry, "로그인 실패 잠금 임계값", spnLockThreshold, ref gy1, 100, x: 16);
        AddRow(grpExpiry, "재설정코드 유효시간(분)", spnResetValidMin, ref gy1, 100, x: 16);
        panel.Controls.Add(grpExpiry);
        y += grpExpiry.Height + groupGap;

        var grpRule = new GroupBoxWyn { Text = "비밀번호 규칙", Location = new Point(0, y), Size = new Size(groupWidth, 196) };
        var gy2 = 34;
        AddRow(grpRule, "최소 길이", spnMinLength, ref gy2, 100, x: 16);
        grpRule.Controls.Add(MakeLabel("문자조합 요구사항", new Point(16, gy2 + 2)));
        gy2 += 24;
        chkRequireUpperLower.Location = new Point(16, gy2); chkRequireUpperLower.AutoSize = true; grpRule.Controls.Add(chkRequireUpperLower); gy2 += 26;
        chkRequireDigit.Location = new Point(16, gy2); chkRequireDigit.AutoSize = true; grpRule.Controls.Add(chkRequireDigit); gy2 += 26;
        chkRequireSpecial.Location = new Point(16, gy2); chkRequireSpecial.AutoSize = true; grpRule.Controls.Add(chkRequireSpecial);
        panel.Controls.Add(grpRule);
        y += grpRule.Height + groupGap;

        var grpNew = new GroupBoxWyn { Text = "신규계정 정책", Location = new Point(0, y), Size = new Size(groupWidth, 160) };
        var gy3 = 34;
        grpNew.Controls.Add(MakeLabel("초기비밀번호", new Point(16, gy3)));
        gy3 += 22;
        rdoInitPwdPolicy.Location = new Point(16, gy3);
        rdoInitPwdPolicy.Size = new Size(240, 46);
        grpNew.Controls.Add(rdoInitPwdPolicy);
        gy3 += 54;
        chkForceChangeOnFirstLogin.Location = new Point(16, gy3); chkForceChangeOnFirstLogin.AutoSize = true;
        grpNew.Controls.Add(chkForceChangeOnFirstLogin);
        panel.Controls.Add(grpNew);

        contentPanel.Controls.Add(panel);
        _categoryPanels["pwd"] = panel;
    }

    /// <summary>화면보호기(자리비움 잠금) - 예전엔 비밀번호 정책 화면 맨 아래 "세션" 항목 하나로
    /// 끼어 있었는데, 별도 챕터로 분리해달라는 요청(2026-09-09)으로 새 카테고리를 만들었다.</summary>
    private void BuildScreensaverPanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32), AutoScroll = true };

        var grp = new GroupBoxWyn { Text = "자리비움 잠금", Location = new Point(0, 0), Size = new Size(460, 100) };
        var y = 34;
        AddRow(grp, "잠금까지의 시간(분, 0=사용 안 함)", spnIdleTimeoutMinutes, ref y, 100, x: 16);
        panel.Controls.Add(grp);

        contentPanel.Controls.Add(panel);
        _categoryPanels["screensaver"] = panel;
    }

    private void BuildColorPanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };

        panel.Controls.Add(MakeLabel("사이트 배포 시 개발자가 1회 지정하는 색상값입니다.", new Point(0, 0)));

        var y = 26;
        AddColorRow(panel, "필수입력 배경", colRequiredField, ref y);
        AddColorRow(panel, "그리드 헤더", colGridHeader, ref y);
        AddColorRow(panel, "포커스 행", colGridFocusedRow, ref y);
        AddColorRow(panel, "브랜드색", colBrand, ref y);
        AddColorRow(panel, "트리 그룹", colTreeGroup, ref y);
        AddColorRow(panel, "구분선", colDivider, ref y);

        contentPanel.Controls.Add(panel);
        _categoryPanels["color"] = panel;
    }

    /// <summary>x: 라벨 시작 x좌표(기본 0) - GroupBoxWyn 안에서 쓸 때는 테두리에 바로 안 붙게
    /// 16 정도를 준다. 입력칸은 항상 라벨칸(160px) 다음 자리(x+160)에 온다 - 기존 호출부(x
    /// 생략, 그룹 없이 패널에 바로 얹는 화면들)는 입력칸 위치가 그대로 유지된다.</summary>
    private static void AddRow(Control parent, string label, BaseEdit edit, ref int y, int width, int x = 0)
    {
        parent.Controls.Add(MakeLabel(label, new Point(x, y + 4)));
        edit.Location = new Point(x + 160, y);
        edit.Size = new Size(width, 20);
        parent.Controls.Add(edit);
        y += 30;
    }

    private static void AddColorRow(Panel parent, string label, ColorEdit edit, ref int y)
    {
        parent.Controls.Add(MakeLabel(label, new Point(0, y + 4)));
        edit.Location = new Point(160, y);
        edit.Size = new Size(140, 20);
        parent.Controls.Add(edit);
        y += 30;
    }

    public override async Task QueryClick()
    {
        try
        {
            var config = await ApiClient.GetAsync<SiteConfigDto>("api/site-config");
            if (config == null) return;

            // 이미지 3장(로그인배경/로고/파비콘)도 PictureEdit.Image 대입이라(BaseEdit 계열이라
            // TrackDirty(contentPanel)에 걸림) 반드시 이 블록 "안"에서 채워야 한다 - 예전엔 이
            // 블록이 끝난 뒤(_suppressDirtyTracking이 이미 꺼진 상태)에 따로 채웠다가, 저장
            // 직후 QueryClick을 다시 부를 때마다 이미지 로드가 IsDirty를 다시 true로 만들어서
            // "변경 내역이 존재합니다" 확인창이 저장을 했는데도 매번 다시 떴다(2026-09-09 실제
            // 발견 - SuppressDirtyTrackingAsync 추가 배경 참고).
            await SuppressDirtyTrackingAsync(async () =>
            {
                txtCompanyNm.Text = config.CompanyNm ?? string.Empty;

                txtSmtpHost.Text = config.SmtpHost ?? string.Empty;
                spnSmtpPort.Value = config.SmtpPort ?? 587;
                txtSmtpUsername.Text = config.SmtpUsername ?? string.Empty;
                txtFromAddress.Text = config.SmtpFromAddress ?? string.Empty;
                txtFromDisplayNm.Text = config.SmtpFromDisplayNm ?? string.Empty;

                txtBlockExtensions.Text = config.FileBlockExtensions ?? string.Empty;
                spnMaxSizeMb.Value = config.FileMaxSizeMb ?? 2048;

                spnExpireDays.Value = config.PwdExpireDays ?? 90;
                spnLockThreshold.Value = config.PwdLockThreshold ?? 5;
                spnResetValidMin.Value = config.PwdResetCodeValidMin ?? 20;
                spnMinLength.Value = config.PwdMinLength ?? 8;
                chkRequireUpperLower.Checked = config.PwdRequireUpperLower;
                chkRequireDigit.Checked = config.PwdRequireDigit;
                chkRequireSpecial.Checked = config.PwdRequireSpecial;
                rdoInitPwdPolicy.EditValue = config.InitPwdPolicy ?? "USER_ID";
                chkForceChangeOnFirstLogin.Checked = config.ForceChangeOnFirstLogin;
                spnIdleTimeoutMinutes.Value = config.IdleTimeoutMinutes ?? 30;

                colRequiredField.Color = ColorTranslator.FromHtml(config.RequiredFieldBackColor ?? "#FFF9DB");
                colGridHeader.Color = ColorTranslator.FromHtml(config.GridHeaderBackColor ?? "#F7F8FA");
                colGridFocusedRow.Color = ColorTranslator.FromHtml(config.GridFocusedRowBackColor ?? "#FDF3E1");
                colBrand.Color = ColorTranslator.FromHtml(config.BrandColor ?? "#1B2A3D");
                colTreeGroup.Color = ColorTranslator.FromHtml(config.TreeGroupBackColor ?? "#F2F3F5");
                colDivider.Color = ColorTranslator.FromHtml(config.DividerColor ?? "#E4E5E8");

                await LoadImageAsync("api/site-config/login-background", picLoginBg);
                await LoadImageAsync("api/site-config/logo", picLogo);
                await LoadImageAsync("api/site-config/favicon", picFavicon);
            });

            _loginBgChanged = _logoChanged = _faviconChanged = false;
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"환경설정 조회 중 오류가 발생했습니다.\n{ex.Message}", "오류");
        }
    }

    private static async Task LoadImageAsync(string url, PictureEdit target)
    {
        try
        {
            var (bytes, _) = await ApiClient.DownloadAsync(url);
            using var stream = new MemoryStream(bytes);
            target.Image = Image.FromStream(stream);
        }
        catch
        {
            target.Image = null; // 아직 등록된 이미지가 없으면(404) 빈 상태로 둔다
        }
    }

    public override async Task SaveClick()
    {
        var dto = new SiteConfigDto
        {
            CompanyNm = txtCompanyNm.Text,
            SmtpHost = txtSmtpHost.Text,
            SmtpPort = (int)spnSmtpPort.Value,
            SmtpUsername = txtSmtpUsername.Text,
            SmtpFromAddress = txtFromAddress.Text,
            SmtpFromDisplayNm = txtFromDisplayNm.Text,
            FileBlockExtensions = txtBlockExtensions.Text,
            FileMaxSizeMb = (int)spnMaxSizeMb.Value,
            PwdExpireDays = (int)spnExpireDays.Value,
            PwdLockThreshold = (int)spnLockThreshold.Value,
            PwdResetCodeValidMin = (int)spnResetValidMin.Value,
            PwdMinLength = (int)spnMinLength.Value,
            PwdRequireUpperLower = chkRequireUpperLower.Checked,
            PwdRequireDigit = chkRequireDigit.Checked,
            PwdRequireSpecial = chkRequireSpecial.Checked,
            InitPwdPolicy = rdoInitPwdPolicy.EditValue?.ToString() ?? "USER_ID",
            ForceChangeOnFirstLogin = chkForceChangeOnFirstLogin.Checked,
            IdleTimeoutMinutes = (int)spnIdleTimeoutMinutes.Value,
            RequiredFieldBackColor = ColorTranslator.ToHtml(colRequiredField.Color),
            GridHeaderBackColor = ColorTranslator.ToHtml(colGridHeader.Color),
            GridFocusedRowBackColor = ColorTranslator.ToHtml(colGridFocusedRow.Color),
            BrandColor = ColorTranslator.ToHtml(colBrand.Color),
            TreeGroupBackColor = ColorTranslator.ToHtml(colTreeGroup.Color),
            DividerColor = ColorTranslator.ToHtml(colDivider.Color),
        };

        var result = await ApiClient.PutAsync<SiteConfigDto, ApiResult>("api/site-config", dto);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        if (_loginBgChanged && _loginBgBytes != null) await UploadImageAsync("api/site-config/login-background", _loginBgBytes, "login-bg.png");
        if (_logoChanged && _logoBytes != null) await UploadImageAsync("api/site-config/logo", _logoBytes, "logo.png");
        if (_faviconChanged && _faviconBytes != null) await UploadImageAsync("api/site-config/favicon", _faviconBytes, "favicon.png");

        Toast.Show("저장되었습니다.");
        await QueryClick();
    }

    private static async Task UploadImageAsync(string url, byte[] bytes, string fileName)
    {
        // 이미지는 5MB 이하 작은 파일이라 청크 없이 한 번에 멀티파트로 보낸다.
        var result = await ApiClient.PutFileAsync<ApiResult>(url, bytes, fileName);
        if (result == null || !result.Success)
            AppMessageBox.Show(result?.Message ?? "이미지 저장에 실패했습니다.", "저장 실패");
    }

    public override Task NewClick() => Task.CompletedTask; // 회사당 1건뿐이라 "신규" 개념 없음
    public override Task DeleteClick() => Task.CompletedTask; // 삭제 불가(단일 설정 레코드)
}
