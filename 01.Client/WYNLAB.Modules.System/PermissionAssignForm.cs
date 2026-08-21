using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using WYNLAB.Shared.Dtos;
using WYNLAB.UI.Common;
using System.Drawing;

namespace WYNLAB.Modules.System;

/// <summary>
/// 권한부여관리 화면. 좌측에서 사용자 또는 그룹(대상)을 고르면, 우측 메뉴트리에서
/// 그 대상에게 직접 걸린 조회/등록/수정/삭제/엑셀 권한을 체크박스로 편집한다.
///
/// 권한 병합 규칙(MenuPermissionMerger)이 "그룹 + 개인 전부 OR 합산"이므로:
/// - 그룹을 선택해서 저장하면 그 그룹 소속 전원에게 바로 적용됨
/// - 사용자를 선택해서 저장하면 그 사람의 소속그룹 권한 위에 "추가"되는 개인 권한이 됨
///   (그룹 권한보다 적게 줄 수는 없음 - 그런 예외가 필요하면 그룹을 나누는 게 맞음)
///
/// GROUP 타입 메뉴(상위 분류, 예: 영업관리)는 조회(View) 체크 하나만 의미가 있다 -
/// 이 체크가 꺼져있으면 로그인 시 ShellForm 좌측 메뉴에서 그 가지 전체가 안 보인다.
/// 등록/수정/삭제/엑셀은 실제 화면(FORM)에만 의미가 있어서 GROUP 행에서는 편집을 막는다.
/// </summary>
public class PermissionAssignForm : BaseForm
{
    private class TargetItem
    {
        public string Cd { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string SubLabel { get; set; } = string.Empty;
    }

    private string _targetType = "USER"; // USER 또는 GRP
    private string? _selectedCd;
    private string? _selectedLabel;
    private List<TargetItem> _targets = new();
    private List<MenuAuthItemDto> _authItems = new();

    private readonly SplitContainerControl splitContainer = new() { Dock = DockStyle.Fill };
    private readonly Panel leftPanel = new() { Dock = DockStyle.Fill };
    private readonly Panel rightPanel = new() { Dock = DockStyle.Fill };

    private readonly SimpleButton btnTabUser = new() { Text = "사용자" };
    private readonly SimpleButton btnTabGroup = new() { Text = "그룹" };
    private readonly TextEdit txtSearch = new();
    private readonly GridControl targetGrid = new();
    private readonly GridView targetGridView = new();
    private readonly ContextMenuStrip copyMenu = new();

    private readonly LabelControl lblTargetHeader = new();
    private readonly LabelControl lblTargetHint = new();
    private readonly TreeList authTree = new();
    private readonly RepositoryItemCheckEdit repoCheck = new();

    public PermissionAssignForm()
    {
        Text = "권한부여관리";
        MenuCd = "SM_AUTH";

        BuildLeftPanel();
        BuildRightPanel();
        splitContainer.Panel1.Controls.Add(leftPanel);
        splitContainer.Panel2.Controls.Add(rightPanel);
        splitContainer.Panel1.MinSize = 200;
        splitContainer.Panel2.MinSize = 400;
        splitContainer.SplitterPosition = 260;

        // Dock 추가 순서: Fill(splitContainer) 먼저, Top(타이틀바)은 나중에 추가해야 맨 위를 차지한다
        Controls.Add(splitContainer);
        Controls.Add(BuildScreenHeader());

        Load += async (s, e) => await QueryAsync();
    }

    /// <summary>
    /// Dock 추가 순서 중요: 이 프로젝트 전반의 검증된 패턴(BaseGridForm/UserGroupEditForm 등)과
    /// 동일하게 Fill을 먼저 추가하고, 그 다음에 Top 패널들을 추가한다 - 나중에 추가된 Top이
    /// 우선권을 가져 맨 위 가장자리를 차지하므로, 화면상 가장 위에 둘 것(tabPanel)을 가장
    /// 나중에 추가한다. BringToFront/SendToBack으로 순서를 억지로 맞추면 오히려 그리드
    /// 컬럼헤더가 다른 패널에 가려지는 문제가 생겨서(이전 버그) 순수 추가순서로만 해결한다.
    /// </summary>
    private void BuildLeftPanel()
    {
        var divider = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.FromArgb(225, 225, 225) };

        targetGrid.MainView = targetGridView;
        targetGrid.Dock = DockStyle.Fill;
        targetGridView.OptionsView.ShowGroupPanel = false;
        targetGridView.OptionsView.ShowIndicator = false;
        targetGridView.OptionsBehavior.Editable = false;
        targetGridView.OptionsSelection.EnableAppearanceFocusedRow = true;
        targetGridView.FocusedRowChanged += async (s, e) => await OnTargetSelectedAsync();
        targetGridView.MouseUp += TargetGridView_MouseUp;

        var searchPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(10, 6, 10, 6) };
        txtSearch.Font = AppFonts.Body;
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Properties.NullValuePrompt = "이름 검색";
        txtSearch.KeyDown += async (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            await LoadTargetsAsync();
        };
        searchPanel.Controls.Add(txtSearch);

        var tabPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(10, 8, 10, 0) };
        btnTabUser.Size = new Size(110, 26);
        btnTabUser.Location = new Point(10, 8);
        btnTabGroup.Size = new Size(110, 26);
        btnTabGroup.Location = new Point(124, 8);
        btnTabUser.Click += async (s, e) => await SwitchTabAsync("USER");
        btnTabGroup.Click += async (s, e) => await SwitchTabAsync("GRP");
        tabPanel.Controls.Add(btnTabUser);
        tabPanel.Controls.Add(btnTabGroup);

        leftPanel.Controls.Add(divider);
        leftPanel.Controls.Add(targetGrid);   // Fill 먼저
        leftPanel.Controls.Add(searchPanel);  // Top - tabPanel보다 먼저 추가되어 그 아래에 위치
        leftPanel.Controls.Add(tabPanel);     // Top - 가장 나중에 추가되어 맨 위 차지

        UpdateTabButtonStyle();
    }

    private void UpdateTabButtonStyle()
    {
        void Style(SimpleButton btn, bool active)
        {
            btn.Appearance.BackColor = active ? Color.FromArgb(37, 122, 201) : Color.FromArgb(241, 243, 245);
            btn.Appearance.ForeColor = active ? Color.White : Color.FromArgb(80, 80, 80);
            btn.Appearance.Options.UseBackColor = true;
            btn.Appearance.Options.UseForeColor = true;
        }
        Style(btnTabUser, _targetType == "USER");
        Style(btnTabGroup, _targetType == "GRP");
    }

    private async Task SwitchTabAsync(string targetType)
    {
        if (_targetType == targetType) return;
        _targetType = targetType;
        UpdateTabButtonStyle();
        ClearRightPanel();
        await LoadTargetsAsync();
    }

    private void BuildRightPanel()
    {
        var headerPanel = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(16, 10, 16, 0) };
        lblTargetHeader.Font = AppFonts.BodyBold;
        lblTargetHeader.Location = new Point(16, 8);
        lblTargetHeader.AutoSizeMode = LabelAutoSizeMode.None;
        lblTargetHeader.Size = new Size(500, 20);
        headerPanel.Controls.Add(lblTargetHeader);

        lblTargetHint.Font = AppFonts.Caption;
        lblTargetHint.Appearance.ForeColor = Color.FromArgb(140, 140, 140);
        lblTargetHint.Location = new Point(16, 28);
        lblTargetHint.AutoSizeMode = LabelAutoSizeMode.None;
        lblTargetHint.Size = new Size(600, 18);
        lblTargetHint.Text = "왼쪽에서 사용자 또는 그룹을 선택해주세요.";
        headerPanel.Controls.Add(lblTargetHint);

        authTree.Dock = DockStyle.Fill;
        authTree.KeyFieldName = "MenuCd";
        authTree.ParentFieldName = "UpperMenuCd";
        authTree.OptionsBehavior.Editable = true;
        authTree.OptionsView.ShowIndicator = false;
        authTree.OptionsView.AutoWidth = false;
        authTree.Enabled = false;

        repoCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
        authTree.RepositoryItems.Add(repoCheck);

        var colMenuNm = new TreeListColumn { FieldName = "MenuNm", Caption = "메뉴", VisibleIndex = 0, Width = 260 };
        var colView = new TreeListColumn { FieldName = "ViewYn", Caption = "조회", VisibleIndex = 1, Width = 60, ColumnEdit = repoCheck };
        var colInsert = new TreeListColumn { FieldName = "InsertYn", Caption = "등록", VisibleIndex = 2, Width = 60, ColumnEdit = repoCheck };
        var colUpdate = new TreeListColumn { FieldName = "UpdateYn", Caption = "수정", VisibleIndex = 3, Width = 60, ColumnEdit = repoCheck };
        var colDelete = new TreeListColumn { FieldName = "DeleteYn", Caption = "삭제", VisibleIndex = 4, Width = 60, ColumnEdit = repoCheck };
        var colExcel = new TreeListColumn { FieldName = "ExcelYn", Caption = "엑셀", VisibleIndex = 5, Width = 60, ColumnEdit = repoCheck };
        foreach (var col in new[] { colMenuNm, colView, colInsert, colUpdate, colDelete, colExcel })
        {
            col.Visible = true;
            col.OptionsColumn.AllowEdit = col != colMenuNm;
            authTree.Columns.Add(col);
        }

        // GROUP(상위분류) 행은 조회 체크만 의미가 있음(그 가지의 좌측메뉴 노출 여부) -
        // 등록/수정/삭제/엑셀은 실제 화면(FORM)에만 해당하므로 편집을 막는다.
        authTree.ShowingEditor += (s, e) =>
        {
            var node = authTree.FocusedNode;
            if (node == null || authTree.FocusedColumn == colMenuNm) return;
            var menuType = (string)node.GetValue("MenuType");
            if (menuType == "GROUP" && authTree.FocusedColumn != colView)
            {
                e.Cancel = true;
            }
        };

        rightPanel.Controls.Add(authTree);    // Fill 먼저
        rightPanel.Controls.Add(headerPanel);  // Top - 나중에 추가되어 맨 위 차지
    }

    private void ClearRightPanel()
    {
        _selectedCd = null;
        _selectedLabel = null;
        _authItems = new();
        authTree.DataSource = null;
        authTree.Enabled = false;
        lblTargetHeader.Text = string.Empty;
        lblTargetHint.Text = "왼쪽에서 사용자 또는 그룹을 선택해주세요.";
    }

    public override async Task QueryAsync()
    {
        await LoadTargetsAsync();
    }

    public override async Task SaveAsync()
    {
        await SaveCurrentAsync();
    }

    private async Task LoadTargetsAsync()
    {
        var keyword = txtSearch.Text.Trim();

        if (_targetType == "USER")
        {
            var query = $"api/users?userId=&userNm={Uri.EscapeDataString(keyword)}";
            var users = await ApiClient.GetAsync<List<UserListItemDto>>(query) ?? new();
            _targets = users.Select(u => new TargetItem { Cd = u.UserId, Label = u.UserNm, SubLabel = u.DeptNm ?? "" }).ToList();
        }
        else
        {
            var query = $"api/user-groups?userGrpNm={Uri.EscapeDataString(keyword)}";
            var groups = await ApiClient.GetAsync<List<UserGroupListItemDto>>(query) ?? new();
            _targets = groups.Select(g => new TargetItem { Cd = g.UserGrpCd, Label = g.UserGrpNm, SubLabel = $"{g.MemberCount}명" }).ToList();
        }

        targetGrid.DataSource = _targets;
        if (targetGridView.Columns["Cd"] != null) targetGridView.Columns["Cd"].Visible = false;
        if (targetGridView.Columns["Label"] != null) targetGridView.Columns["Label"].Caption = _targetType == "USER" ? "이름" : "그룹명";
        if (targetGridView.Columns["SubLabel"] != null) targetGridView.Columns["SubLabel"].Caption = _targetType == "USER" ? "부서" : "인원";
    }

    private async Task OnTargetSelectedAsync()
    {
        var row = targetGridView.GetFocusedRow() as TargetItem;
        if (row == null) return;

        _selectedCd = row.Cd;
        _selectedLabel = row.Label;
        await LoadAuthAsync(_targetType, row.Cd, row.Label);
    }

    private async Task LoadAuthAsync(string targetType, string targetCd, string label)
    {
        var query = $"api/menu-auth?targetType={targetType}&targetCd={Uri.EscapeDataString(targetCd)}";
        _authItems = await ApiClient.GetAsync<List<MenuAuthItemDto>>(query) ?? new();

        authTree.DataSource = _authItems;
        authTree.Enabled = true;
        authTree.ExpandAll();

        lblTargetHeader.Text = label;
        lblTargetHint.Text = targetType == "USER"
            ? "개인 추가 권한 - 소속그룹 권한에 더해서 부여됩니다 (그룹 권한보다 적게 줄 수는 없습니다)"
            : "그룹 권한 - 이 그룹 소속 전원에게 적용됩니다";
    }

    private async Task SaveCurrentAsync()
    {
        if (_selectedCd == null)
        {
            AppMessageBox.Show("먼저 왼쪽에서 대상을 선택해주세요.", "안내");
            return;
        }

        authTree.CloseEditor();
        authTree.PostEditor();

        var request = new SaveMenuAuthRequest
        {
            TargetType = _targetType,
            TargetCd = _selectedCd,
            Items = _authItems
        };

        var result = await ApiClient.PutAsync<SaveMenuAuthRequest, ApiResult>("api/menu-auth", request);
        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        AppMessageBox.Show($"'{_selectedLabel}'의 권한을 저장했습니다.", "저장 완료");
    }

    /// <summary>
    /// 좌측 목록에서 다른 대상을 우클릭하면 "이 대상의 권한을 복사"를 띄운다.
    /// 지금 우측에 편집 중인 대상(선택된 목적지)은 그대로 두고, 그 트리의 체크상태만
    /// 우클릭한 대상의 권한값으로 덮어써서 미리보기/조정 후 저장하도록 한다.
    /// </summary>
    private void TargetGridView_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        var hit = targetGridView.CalcHitInfo(e.Location);
        if (!hit.InRow) return;

        var row = targetGridView.GetRow(hit.RowHandle) as TargetItem;
        if (row == null) return;

        if (_selectedCd == null)
        {
            AppMessageBox.Show("복사해 넣을 대상을 먼저 왼쪽에서 선택해주세요.", "안내");
            return;
        }

        copyMenu.Items.Clear();
        copyMenu.Items.Add($"'{row.Label}'의 권한을 '{_selectedLabel}'에 복사", null, async (s, ev) => await CopyFromAsync(row));
        copyMenu.Show(targetGrid, e.Location);
    }

    private async Task CopyFromAsync(TargetItem source)
    {
        var query = $"api/menu-auth?targetType={_targetType}&targetCd={Uri.EscapeDataString(source.Cd)}";
        var sourceItems = await ApiClient.GetAsync<List<MenuAuthItemDto>>(query) ?? new();
        var sourceByMenu = sourceItems.ToDictionary(i => i.MenuCd);

        foreach (var item in _authItems)
        {
            if (!sourceByMenu.TryGetValue(item.MenuCd, out var src)) continue;
            item.ViewYn = src.ViewYn;
            item.InsertYn = src.InsertYn;
            item.UpdateYn = src.UpdateYn;
            item.DeleteYn = src.DeleteYn;
            item.ExcelYn = src.ExcelYn;
        }

        authTree.RefreshDataSource();
        AppMessageBox.Show($"'{source.Label}'의 권한을 복사했습니다. 확인 후 저장해주세요.", "복사 완료");
    }
}
