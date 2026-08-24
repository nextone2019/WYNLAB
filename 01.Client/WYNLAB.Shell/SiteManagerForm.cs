using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// 개발자가 이 PC에서 접속할 서비스(서버)를 직접 추가/삭제하는 화면. appsettings.json에
/// 내장된 서비스(예: Development/Production)는 회사 전체가 같이 쓰는 고정 목록이라 여기서
/// 못 지우게 막고, AppConfig.AddSite/RemoveSite로 관리하는 "이 PC에만 저장된" 목록만 다룬다 -
/// 재배포 없이 개발자가 원하는 만큼 고객사 서버를 늘려가며 로그인 화면에서 바로 골라 쓸 수 있다.
/// </summary>
public class SiteManagerForm : XtraForm
{
    private const int HeaderHeight = 36;

    private readonly PanelControl headerPanel = new();
    private readonly LabelControl lblCaption = new() { Text = "서비스 관리" };
    private readonly LabelControl lblClose = new() { Text = "✕" };
    private readonly GridControlWyn grid = new();
    private readonly TextEditWyn txtName = new();
    private readonly TextEditWyn txtApiBaseUrl = new();
    private readonly SimpleButton btnAdd = new() { Text = "추가" };
    private readonly SimpleButton btnDelete = new() { Text = "선택 항목 삭제" };
    private readonly SimpleButton btnCloseFooter = new() { Text = "닫기" };

    private Point _dragStart;
    private bool _dragging;

    private class SiteRow
    {
        public string Name { get; set; } = string.Empty;
        public string ApiBaseUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public SiteManagerForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        Size = new Size(460, 420);
        ShowInTaskbar = false;

        BuildHeader();
        BuildBody();

        Controls.Add(BuildBodyPanel());
        Controls.Add(headerPanel);

        RefreshGrid();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(210, 212, 217));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    private void BuildHeader()
    {
        var brandColor = ColorHelper.FromHex(AppConfig.ToolbarColor);

        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = HeaderHeight;
        headerPanel.Appearance.BackColor = brandColor;
        headerPanel.Appearance.Options.UseBackColor = true;
        headerPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        lblCaption.Location = new Point(14, 9);
        lblCaption.AutoSizeMode = LabelAutoSizeMode.None;
        lblCaption.Size = new Size(Width - 48, 18);
        lblCaption.Appearance.ForeColor = Color.White;
        lblCaption.Appearance.Font = AppFonts.BodyBold;

        lblClose.Location = new Point(Width - 32, 8);
        lblClose.AutoSizeMode = LabelAutoSizeMode.None;
        lblClose.Size = new Size(20, 20);
        lblClose.Appearance.ForeColor = Color.FromArgb(230, 255, 255, 255);
        lblClose.Appearance.Font = AppFonts.Caption;
        lblClose.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblClose.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        lblClose.Cursor = Cursors.Hand;
        lblClose.Click += (s, e) => Close();

        headerPanel.Controls.Add(lblCaption);
        headerPanel.Controls.Add(lblClose);

        EnableDrag(headerPanel);
        EnableDrag(lblCaption);
    }

    private void BuildBody()
    {
        // GridControlWyn은 이제 View를 자동으로 안 만든다(디자이너 컬럼 저장 문제 때문에 뺐음 -
        // GridControlWyn.cs 주석 참고). 이 화면은 디자이너가 아니라 코드로 직접 만드니 여기서
        // 그냥 하나 만들어서 연결해준다.
        grid.MainView = new GridViewWyn();
        grid.View!.OptionsView.ShowGroupPanel = false;
        grid.View.OptionsBehavior.Editable = false;
        grid.View.EmptyText = "추가된 서비스가 없습니다.";
        grid.View.FocusedRowObjectChanged += (s, e) => UpdateDeleteButtonState();
    }

    private Control BuildBodyPanel()
    {
        var body = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16) };

        grid.Location = new Point(16, 16);
        grid.Size = new Size(428, 190);
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        var lblSectionTitle = new LabelControl
        {
            Text = "새 서비스 추가",
            Location = new Point(16, 220),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size = new Size(200, 16)
        };
        lblSectionTitle.Appearance.ForeColor = Color.FromArgb(140, 140, 140);
        lblSectionTitle.Appearance.Font = AppFonts.Caption;

        var lblName = FieldLabel("이름", 16, 244);
        txtName.Location = new Point(16, 264);
        txtName.Size = new Size(200, 24);
        txtName.Font = AppFonts.Body;

        var lblUrl = FieldLabel("API 주소", 228, 244);
        txtApiBaseUrl.Location = new Point(228, 264);
        txtApiBaseUrl.Size = new Size(216, 24);
        txtApiBaseUrl.Font = AppFonts.Body;
        txtApiBaseUrl.Properties.NullText = "http://host:port";

        btnAdd.Location = new Point(16, 298);
        btnAdd.Size = new Size(100, 28);
        btnAdd.Click += BtnAdd_Click;

        btnDelete.Location = new Point(124, 298);
        btnDelete.Size = new Size(120, 28);
        btnDelete.Click += BtnDelete_Click;

        btnCloseFooter.Location = new Point(344, 298);
        btnCloseFooter.Size = new Size(100, 28);
        btnCloseFooter.Click += (s, e) => Close();

        body.Controls.Add(grid);
        body.Controls.Add(lblSectionTitle);
        body.Controls.Add(lblName);
        body.Controls.Add(txtName);
        body.Controls.Add(lblUrl);
        body.Controls.Add(txtApiBaseUrl);
        body.Controls.Add(btnAdd);
        body.Controls.Add(btnDelete);
        body.Controls.Add(btnCloseFooter);

        return body;
    }

    private static LabelControl FieldLabel(string text, int x, int y)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, y), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(200, 16) };
        lbl.Appearance.ForeColor = Color.FromArgb(110, 110, 110);
        lbl.Appearance.Font = AppFonts.Caption;
        return lbl;
    }

    private void RefreshGrid()
    {
        var rows = AppConfig.AvailableEnvironments
            .Select(name => new SiteRow
            {
                Name = name,
                ApiBaseUrl = AppConfig.IsUserSite(name)
                    ? AppConfig.UserSites.First(s => s.Name == name).ApiBaseUrl
                    : "(회사 공통)",
                Type = AppConfig.IsUserSite(name) ? "내가 추가함" : "기본 제공"
            })
            .ToList();

        grid.DataSource = rows;
        UpdateDeleteButtonState();
    }

    private void UpdateDeleteButtonState()
    {
        var focused = grid.View!.GetFocusedRow() as SiteRow;
        btnDelete.Enabled = focused != null && AppConfig.IsUserSite(focused.Name);
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        var name = txtName.Text.Trim();
        var url = txtApiBaseUrl.Text.Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
        {
            AppMessageBox.Show("이름과 API 주소를 입력해주세요.", "확인");
            return;
        }

        try
        {
            AppConfig.AddSite(name, url);
        }
        catch (ArgumentException ex)
        {
            AppMessageBox.Show(ex.Message, "확인");
            return;
        }

        txtName.Text = string.Empty;
        txtApiBaseUrl.Text = string.Empty;
        RefreshGrid();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (grid.View!.GetFocusedRow() is not SiteRow row || !AppConfig.IsUserSite(row.Name)) return;

        if (AppMessageBox.Show($"'{row.Name}' 서비스를 삭제할까요?", "확인", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        AppConfig.RemoveSite(row.Name);
        RefreshGrid();
    }

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
}
