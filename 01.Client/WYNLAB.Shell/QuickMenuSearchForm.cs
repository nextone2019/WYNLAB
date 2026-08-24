using DevExpress.XtraEditors;
using WYNLAB.Base;
using WYNLAB.Shared.Dtos;
using System.Drawing;

namespace WYNLAB.Shell;

/// <summary>
/// Ctrl+K로 여는 전역 메뉴 빠른 검색 팔레트(요즘 관리자 UI/에디터의 표준 UX). 좌측
/// 아코디언 메뉴트리를 계속 펼쳐가며 찾을 필요 없이, 화면 이름 일부만 입력하면 바로 열 수 있다.
/// ShellForm이 ProcessCmdKey로 Ctrl+K를 가로채 ShowDialog()로 띄우고, 결과(Result)가 있으면
/// OpenMenuByCode를 호출한다.
/// </summary>
public class QuickMenuSearchForm : XtraForm
{
    private readonly TextEdit txtSearch = new();
    private readonly ListBox listResults = new() { IntegralHeight = false, BorderStyle = BorderStyle.None };
    private readonly LabelControl lblEmpty = new()
    {
        Text = "일치하는 화면이 없습니다.",
        Dock = DockStyle.Fill,
        AutoSizeMode = LabelAutoSizeMode.None,
        Visible = false
    };
    private readonly List<MenuDto> _allMenus;
    private List<MenuDto> _filtered = new();

    public MenuDto? Result { get; private set; }

    public QuickMenuSearchForm(IReadOnlyList<MenuDto> menus)
    {
        _allMenus = menus.ToList();

        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(480, 380);
        BackColor = Color.FromArgb(210, 212, 216); // 1px 테두리처럼 보이게 바깥쪽만 이 색

        var inner = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(1) };
        var body = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

        txtSearch.Dock = DockStyle.Top;
        txtSearch.Height = 46;
        txtSearch.Properties.NullValuePrompt = "화면 이름으로 검색  (Esc: 닫기, Enter: 열기)";
        txtSearch.Properties.Appearance.Font = AppFonts.Heading;
        txtSearch.Properties.Appearance.Options.UseFont = true;
        txtSearch.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        txtSearch.TextChanged += (s, e) => RefreshResults();
        txtSearch.KeyDown += TxtSearch_KeyDown;

        var topBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 231, 235) };

        lblEmpty.Appearance.ForeColor = Color.FromArgb(150, 152, 158);
        lblEmpty.Appearance.Font = AppFonts.Body;
        lblEmpty.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        lblEmpty.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

        listResults.Dock = DockStyle.Fill;
        listResults.Font = AppFonts.Body;
        listResults.ItemHeight = 30;
        listResults.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        listResults.DrawItem += ListResults_DrawItem;
        listResults.DoubleClick += (s, e) => Commit();
        listResults.KeyDown += ListResults_KeyDown;

        body.Controls.Add(listResults);
        body.Controls.Add(lblEmpty);
        body.Controls.Add(topBorder);
        body.Controls.Add(txtSearch);
        inner.Controls.Add(body);
        Controls.Add(inner);

        Shown += (s, e) => { txtSearch.Focus(); RefreshResults(); };
        Deactivate += (s, e) => Close();
    }

    private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Escape:
                Close();
                e.Handled = true;
                break;
            case Keys.Enter:
                Commit();
                e.Handled = true;
                break;
            case Keys.Down:
                if (listResults.Items.Count > 0)
                {
                    listResults.SelectedIndex = Math.Min(listResults.SelectedIndex + 1, listResults.Items.Count - 1);
                }
                e.Handled = true;
                break;
            case Keys.Up:
                if (listResults.Items.Count > 0)
                {
                    listResults.SelectedIndex = Math.Max(listResults.SelectedIndex - 1, 0);
                }
                e.Handled = true;
                break;
        }
    }

    private void ListResults_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) { Close(); e.Handled = true; }
        else if (e.KeyCode == Keys.Enter) { Commit(); e.Handled = true; }
    }

    /// <summary>메뉴명에 검색어가 포함되는지로 필터 - 아직 화면 수가 적어 단순 포함검색으로 충분하다</summary>
    private void RefreshResults()
    {
        var keyword = txtSearch.Text?.Trim() ?? string.Empty;

        _filtered = string.IsNullOrEmpty(keyword)
            ? _allMenus.OrderBy(m => m.MenuNm).ToList()
            : _allMenus.Where(m => m.MenuNm.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                       .OrderBy(m => m.MenuNm).ToList();

        listResults.Items.Clear();
        listResults.Items.AddRange(_filtered.Select(m => (object)m.MenuNm).ToArray());
        if (listResults.Items.Count > 0) listResults.SelectedIndex = 0;

        lblEmpty.Visible = _filtered.Count == 0;
        listResults.Visible = _filtered.Count > 0;
    }

    private void ListResults_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= _filtered.Count) return;

        var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var back = selected ? Color.FromArgb(41, 121, 255) : Color.White;
        var fore = selected ? Color.White : Color.FromArgb(45, 45, 48);

        using (var brush = new SolidBrush(back))
            e.Graphics.FillRectangle(brush, e.Bounds);

        var menu = _filtered[e.Index];
        var rect = new Rectangle(e.Bounds.X + 14, e.Bounds.Y, e.Bounds.Width - 28, e.Bounds.Height);
        TextRenderer.DrawText(e.Graphics, menu.MenuNm, AppFonts.Body, rect, fore,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    private void Commit()
    {
        if (listResults.SelectedIndex < 0 || listResults.SelectedIndex >= _filtered.Count) return;

        Result = _filtered[listResults.SelectedIndex];
        DialogResult = DialogResult.OK;
        Close();
    }
}
