using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM.USER;

/// <summary>
/// 사용자/사용자그룹 통합관리 화면 - 기초코드등록(frmMinorCode)과 같은 디자인 패턴(좌측 조회전용
/// 리스트 + 우측 상세패널 저장)을, 상단 탭 2개로 나눠 "사용자관리"와 "사용자그룹관리"를 한 화면에
/// 담았다.
///
/// - 사용자관리 탭: 사용자 등록 + 소속그룹 배정(체크그리드) + 사용자별 권한부여(트리)
/// - 사용자그룹관리 탭: 그룹 등록 + 소속 사용자 배정(체크그리드, 여러 명 동시 배정 가능) +
///   그룹별 권한부여(트리) - UserGroupListForm이 이미 쓰던 api/user-groups/{cd}/members
///   패턴을 그대로 재사용한다.
///
/// TSMUSERGRPMAP(사용자-그룹 매핑)을 양쪽 탭에서 각자 반대 방향으로 편집할 수 있다(사용자 탭은
/// "이 사용자가 속할 그룹들", 그룹 탭은 "이 그룹에 속할 사용자들"). 한쪽 탭에서 저장한 뒤 그
/// 편집 대상을 화면에 그대로 띄워둔 채 다른 탭으로 넘어갔다 돌아오면, 그리드 포커스행이 그대로라
/// FocusedRowObjectChanged가 재발생하지 않아 상세 데이터가 저장 전 상태로 남는 문제가 있다 -
/// 이를 막기 위해 outerTab.SelectedPageChanged에서 탭이 활성화될 때마다 현재 선택된 대상의
/// 상세 데이터(그룹배정/권한 또는 소속사용자/권한)를 무조건 재조회한다(OnOuterTabChangedAsync).
///
/// 기존 UserListForm(목록) + UserEditForm(수정 팝업, 그룹배정 포함) + UserGroupListForm(그룹
/// 관리) + PermissionAssignForm(사용자/그룹 권한부여) 화면들이 이미 쓰던 api/users,
/// api/users/{id}/groups, api/user-groups, api/user-groups/{cd}/members, api/menu-auth를
/// 그대로 재사용한다 - 서버는 전혀 안 건드림. 위 화면들은 검토 후 나중에 한꺼번에 정리하기로
/// 하고 지금은 그대로 남겨뒀다.
/// </summary>
public class frmUserManage : BaseForm
{
    private readonly TabControlWyn outerTab = new() { Dock = DockStyle.Fill };
    private readonly XtraTabPage tabUserPage = new() { Text = "사용자관리" };
    private readonly XtraTabPage tabGroupPage = new() { Text = "사용자그룹관리" };

    // ===================== 사용자관리 탭 =====================
    private readonly Panel panHeader = new() { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(250, 250, 251) };
    private readonly TextEditWyn txtSearchUserId = new();
    private readonly TextEditWyn txtSearchUserNm = new();

    private readonly Panel panBase = new() { Dock = DockStyle.Fill };
    private readonly Panel panLeft = new() { Dock = DockStyle.Left, Width = 380 };
    private readonly SplitterWyn splitter = new();
    private readonly Panel panRight = new() { Dock = DockStyle.Fill };

    private readonly SectionHeaderWyn sectionHeaderList = new() { Text = "사용자 LIST", Icon = SectionHeaderIcon.Grid, Dock = DockStyle.Top, Height = 25 };
    private readonly GridControlWyn grd1 = new() { Dock = DockStyle.Fill };
    private readonly GridViewWyn gvw1 = new() { HighlightFocusedRow = true };

    private readonly SectionHeaderWyn sectionHeaderDetail = new() { Text = "사용자 등록", Icon = SectionHeaderIcon.Document, Dock = DockStyle.Top, Height = 25 };
    private readonly Panel panDetailFields = new() { Dock = DockStyle.Top, Height = 190 };
    private readonly TextEditWyn txtUserId = new();
    private readonly TextEditWyn txtUserNm = new();
    private readonly TextEditWyn txtPassword = new() { Properties = { PasswordChar = '*' } };
    private readonly TextEditWyn txtEmpNo = new();
    private readonly TextEditWyn txtDeptCd = new();
    private readonly TextEditWyn txtPositionNm = new();
    private readonly TextEditWyn txtEmail = new();
    private readonly TextEditWyn txtMobileNo = new();
    private readonly CheckEdit chkIsAdmin = new() { Text = "시스템관리자 권한 부여" };
    private readonly CheckEdit chkUseYn = new() { Text = "사용" };

    private readonly TabControlWyn tabControl = new() { Dock = DockStyle.Fill };
    private readonly XtraTabPage tabGroup = new() { Text = "그룹배정" };
    private readonly XtraTabPage tabAuth = new() { Text = "권한부여" };

    private readonly GridControlWyn grdGroup = new() { Dock = DockStyle.Fill };
    private readonly GridViewWyn gvwGroup = new() { HighlightFocusedRow = true };
    private List<UserGroupAssignDto> _groups = new();

    private readonly TreeList authTree = new() { Dock = DockStyle.Fill };
    private readonly RepositoryItemCheckEdit repoCheck = new();
    private List<MenuAuthItemDto> _authItems = new();

    private List<UserListItemDto> _users = new();
    private string? _editingUserId; // null이면 신규모드

    // 그리드 재바인딩 시 DevExpress가 자동으로 첫 행에 포커스를 주면서 FocusedRowObjectChanged가
    // 발생하는데, 그 핸들러 안에서 시작하는 로딩(그룹/권한 등)은 비동기라 호출한 쪽(QueryUsersAsync/
    // QueryGroupsAsync)이 직접 기다릴 방법이 없다 - 여기 담아두면 그 로딩이 끝날 때까지
    // 기다렸다가 DrawingSuspension을 풀 수 있다(QueryUsersAsync/QueryGroupsAsync 참고).
    private Task? _pendingLoadTask;

    // ===================== 사용자그룹관리 탭 =====================
    private readonly Panel panHeader2 = new() { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(250, 250, 251) };
    private readonly TextEditWyn txtSearchGrpNm = new();

    private readonly Panel panBase2 = new() { Dock = DockStyle.Fill };
    private readonly Panel panLeft2 = new() { Dock = DockStyle.Left, Width = 380 };
    private readonly SplitterWyn splitter2 = new();
    private readonly Panel panRight2 = new() { Dock = DockStyle.Fill };

    private readonly SectionHeaderWyn sectionHeaderGroupList = new() { Text = "사용자그룹 LIST", Icon = SectionHeaderIcon.Grid, Dock = DockStyle.Top, Height = 25 };
    private readonly GridControlWyn grd3 = new() { Dock = DockStyle.Fill };
    private readonly GridViewWyn gvw3 = new() { HighlightFocusedRow = true };

    private readonly SectionHeaderWyn sectionHeaderGroupDetail = new() { Text = "사용자그룹 등록", Icon = SectionHeaderIcon.Document, Dock = DockStyle.Top, Height = 25 };
    private readonly Panel panGroupFields = new() { Dock = DockStyle.Top, Height = 110 };
    private readonly TextEditWyn txtUserGrpCd = new();
    private readonly TextEditWyn txtUserGrpNm = new();
    private readonly TextEditWyn txtGroupDescription = new();
    private readonly SpinEdit spnGroupSortOrder = new();
    private readonly CheckEdit chkGroupUseYn = new() { Text = "사용" };

    // 소속 사용자(왼쪽, 좁게) / 권한부여(오른쪽, Fill) - "여유가 있으면 한쪽에 소속 사용자
    // 리스트를 보여주자"는 요청을 좌우 분할로 구현.
    private readonly Panel panGroupBottom = new() { Dock = DockStyle.Fill };
    private readonly Panel panMembers = new() { Dock = DockStyle.Left, Width = 260 };
    private readonly SplitterWyn splitter3 = new();
    private readonly Panel panGroupAuth = new() { Dock = DockStyle.Fill };

    private readonly SectionHeaderWyn sectionHeaderMembers = new() { Text = "소속 사용자", Icon = SectionHeaderIcon.Grid, Dock = DockStyle.Top, Height = 25 };
    private readonly GridControlWyn grd4 = new() { Dock = DockStyle.Fill };
    private readonly GridViewWyn gvw4 = new() { HighlightFocusedRow = true };
    private List<UserGroupMemberDto> _members = new();

    private readonly SectionHeaderWyn sectionHeaderGroupAuth = new() { Text = "권한부여", Icon = SectionHeaderIcon.Document, Dock = DockStyle.Top, Height = 25 };
    private readonly TreeList authTree2 = new() { Dock = DockStyle.Fill };
    private readonly RepositoryItemCheckEdit repoCheck2 = new();
    private List<MenuAuthItemDto> _groupAuthItems = new();

    private List<UserGroupListItemDto> _groupsList = new();
    private string? _editingUserGrpCd; // null이면 신규모드

    public frmUserManage()
    {
        Text = "사용자관리";
        MenuCd = "SM_USER";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw3.FocusedRowObjectChanged += Gvw3_FocusedRowObjectChanged;

        BuildHeader();
        BuildLeft();
        BuildDetailFields();
        BuildGroupTab();
        BuildAuthTab();
        tabControl.TabPages.AddRange(new[] { tabGroup, tabAuth });

        // 대분류 화면과 같은 순서 규칙: Fill(필드+탭을 담을 컨테이너) 먼저 필요 없음 - panRight
        // 자체가 Dock=Fill이라 상세헤더/필드/탭을 Top->Top->Fill로 그냥 순서대로 쌓으면 된다.
        panRight.Controls.Add(tabControl);
        panRight.Controls.Add(panDetailFields);
        panRight.Controls.Add(sectionHeaderDetail);

        // Dock 추가 순서(이 프로젝트 전반의 규칙) - 나중에 추가한 쪽이 진짜 가장자리를 차지한다.
        // panRight(Fill) 먼저, splitter(Left)는 그 다음, panLeft(Left)를 맨 나중에 추가해야
        // panLeft가 진짜 왼쪽 끝을 차지하고 splitter가 그 오른쪽 경계에 온다.
        panBase.Controls.Add(panRight);
        panBase.Controls.Add(splitter);
        panBase.Controls.Add(panLeft);

        tabUserPage.Controls.Add(panBase);
        tabUserPage.Controls.Add(panHeader);

        BuildHeader2();
        BuildGroupList();
        BuildGroupFields();
        BuildMembersPanel();
        BuildGroupAuthPanel();

        panGroupBottom.Controls.Add(panGroupAuth);
        panGroupBottom.Controls.Add(splitter3);
        panGroupBottom.Controls.Add(panMembers);

        panRight2.Controls.Add(panGroupBottom);
        panRight2.Controls.Add(panGroupFields);
        panRight2.Controls.Add(sectionHeaderGroupDetail);

        panBase2.Controls.Add(panRight2);
        panBase2.Controls.Add(splitter2);
        panBase2.Controls.Add(panLeft2);

        tabGroupPage.Controls.Add(panBase2);
        tabGroupPage.Controls.Add(panHeader2);

        outerTab.TabPages.AddRange(new[] { tabUserPage, tabGroupPage });
        outerTab.SelectedPageChanged += async (s, e) => await OnOuterTabChangedAsync();

        Controls.Add(outerTab);
        Controls.Add(BuildScreenHeader());

        EnterNewMode();
        EnterNewGroupMode();
    }

    /// <summary>
    /// 탭 전환 시 반대편 탭에서 방금 수정했을 수도 있는 데이터를 다시 불러온다. 클래스
    /// 설명(위 doc comment)에 적은 이유 참고 - 그리드 포커스행이 그대로면 FocusedRowObjectChanged가
    /// 재발생하지 않아 상세 데이터가 저장 전 상태로 남기 때문에, 탭이 활성화될 때마다 현재
    /// 선택된 대상의 상세 데이터를 무조건 재조회한다.
    /// </summary>
    private async Task OnOuterTabChangedAsync()
    {
        if (outerTab.SelectedTabPage == tabGroupPage)
        {
            if (_editingUserGrpCd != null)
            {
                await LoadMembersAsync(_editingUserGrpCd);
                await LoadGroupAuthAsync(_editingUserGrpCd);
            }
        }
        else
        {
            if (_editingUserId != null)
            {
                await LoadGroupsAsync(_editingUserId);
                await LoadAuthAsync(_editingUserId);
            }
        }
    }

    private void BuildHeader()
    {
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        panHeader.Controls.Add(bottomBorder);

        AddHeaderLabel(panHeader, "아이디", 16);
        txtSearchUserId.Font = AppFonts.Body;
        txtSearchUserId.Location = new Point(58, 11);
        txtSearchUserId.Size = new Size(140, 24);
        panHeader.Controls.Add(txtSearchUserId);

        AddHeaderLabel(panHeader, "이름", 216);
        txtSearchUserNm.Font = AppFonts.Body;
        txtSearchUserNm.Location = new Point(250, 11);
        txtSearchUserNm.Size = new Size(140, 24);
        panHeader.Controls.Add(txtSearchUserNm);

        void SearchOnEnter(object? s, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            _ = QueryClick();
        }
        txtSearchUserId.KeyDown += SearchOnEnter;
        txtSearchUserNm.KeyDown += SearchOnEnter;
    }

    private static void AddHeaderLabel(Panel parent, string text, int x)
    {
        var lbl = new LabelControl { Text = text, Location = new Point(x, 15), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(40, 18) };
        lbl.Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        lbl.Appearance.Font = AppFonts.Caption;
        parent.Controls.Add(lbl);
    }

    private void BuildLeft()
    {
        grd1.MainView = gvw1;
        gvw1.OptionsBehavior.Editable = false; // 조회 전용 - 저장은 항상 우측 패널에서
        gvw1.OptionsView.ShowGroupPanel = false;
        gvw1.Columns.AddField(nameof(UserListItemDto.UserId)).Caption = "아이디";
        gvw1.Columns.AddField(nameof(UserListItemDto.UserNm)).Caption = "이름";
        gvw1.Columns.AddField(nameof(UserListItemDto.DeptNm)).Caption = "부서";
        gvw1.Columns.AddField(nameof(UserListItemDto.PositionNm)).Caption = "직급";
        gvw1.Columns.AddField(nameof(UserListItemDto.UseYn)).Caption = "사용";
        foreach (DevExpress.XtraGrid.Columns.GridColumn c in gvw1.Columns) c.Visible = true;

        // Dock 순서: Fill(그리드) 먼저, Top(헤더 스트립)은 나중에 추가해야 맨 위를 차지한다.
        panLeft.Controls.Add(grd1);
        panLeft.Controls.Add(sectionHeaderList);
    }

    /// <summary>사용자 상세 입력 필드 - 대분류 상세 패널과 같은 톤(라벨+입력창을 2열로).</summary>
    private void BuildDetailFields()
    {
        const int labelW = 76, col1X = 12, col2X = 200, rowH = 30;
        var y = 8;

        void Row(string label1, Control c1, string label2, Control c2)
        {
            AddField(panDetailFields, label1, c1, col1X, y, labelW, 110);
            if (c2 != null) AddField(panDetailFields, label2, c2, col2X, y, labelW, 160);
            y += rowH;
        }

        txtUserId.Font = AppFonts.Body;
        txtUserNm.Font = AppFonts.Body;
        txtPassword.Font = AppFonts.Body;
        txtEmpNo.Font = AppFonts.Body;
        txtDeptCd.Font = AppFonts.Body;
        txtPositionNm.Font = AppFonts.Body;
        txtEmail.Font = AppFonts.Body;
        txtMobileNo.Font = AppFonts.Body;

        Row("아이디", txtUserId, "이름", txtUserNm);
        Row("비밀번호", txtPassword, "사번", txtEmpNo);
        Row("부서코드", txtDeptCd, "직급", txtPositionNm);
        Row("이메일", txtEmail, "휴대폰", txtMobileNo);

        chkIsAdmin.Location = new Point(col1X + labelW + 6, y + 2);
        chkIsAdmin.Size = new Size(180, 20);
        panDetailFields.Controls.Add(chkIsAdmin);

        chkUseYn.Location = new Point(col2X + labelW + 6, y + 2);
        chkUseYn.Size = new Size(80, 20);
        panDetailFields.Controls.Add(chkUseYn);
    }

    private static void AddField(Control parent, string label, Control input, int x, int y, int labelW, int inputW)
    {
        var lbl = new LabelControl { Text = label, Location = new Point(x, y + 3), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(labelW, 18) };
        lbl.Appearance.ForeColor = Color.FromArgb(100, 100, 100);
        lbl.Appearance.Font = AppFonts.Caption;
        parent.Controls.Add(lbl);

        input.Location = new Point(x + labelW, y);
        input.Size = new Size(inputW, 24);
        parent.Controls.Add(input);
    }

    /// <summary>소속 그룹 배정 - 조회된 그룹 전체를 체크그리드로 보여주고 체크된 것만 저장.
    /// UserEditForm이 팝업에서 하던 것과 같은 데이터(api/users/{id}/groups)를 그대로 쓴다.</summary>
    private void BuildGroupTab()
    {
        grdGroup.MainView = gvwGroup;
        gvwGroup.OptionsView.ShowGroupPanel = false;
        gvwGroup.OptionsBehavior.Editable = true;

        // 컬럼을 로드 시점(LoadGroupsAsync)이 아니라 여기서 미리 정의해둔다 - DataSource가 아직
        // 없어도 AddField로 컬럼 자체는 만들 수 있고(grd1/grd3와 같은 패턴), 그래야 사용자를
        // 아직 선택하기 전에도 헤더("그룹코드/그룹명/소속")가 제대로 보인다. 로드 전까지 헤더가
        // 텅 비어 있으면 그리드 자체가 비활성화된 것처럼 보인다는 피드백이 있었다(실제로 겪음 -
        // 사용자그룹관리 탭의 소속 사용자 그리드에서 처음 발견됨, BuildMembersPanel도 같은 이유).
        gvwGroup.Columns.AddField(nameof(UserGroupAssignDto.UserGrpCd)).Caption = "그룹코드";
        gvwGroup.Columns.AddField(nameof(UserGroupAssignDto.UserGrpNm)).Caption = "그룹명";
        gvwGroup.Columns.AddField(nameof(UserGroupAssignDto.IsMember)).Caption = "소속";
        foreach (DevExpress.XtraGrid.Columns.GridColumn c in gvwGroup.Columns) c.Visible = true;
        if (gvwGroup.Columns[nameof(UserGroupAssignDto.UserGrpCd)] is { } colCd) colCd.OptionsColumn.AllowEdit = false;
        if (gvwGroup.Columns[nameof(UserGroupAssignDto.UserGrpNm)] is { } colNm) colNm.OptionsColumn.AllowEdit = false;
        if (gvwGroup.Columns[nameof(UserGroupAssignDto.IsMember)] is { } colMember) colMember.Width = 50;

        tabGroup.Controls.Add(grdGroup);
    }

    /// <summary>사용자 대상 권한부여 - PermissionAssignForm의 우측 트리와 같은 구성(메뉴트리 +
    /// 조회/등록/수정/삭제/엑셀 체크). 대상이 항상 "선택된 사용자"로 고정이라 좌측 대상선택
    /// UI(탭/검색)는 필요 없다.</summary>
    private void BuildAuthTab()
    {
        authTree.KeyFieldName = "MenuCd";
        authTree.ParentFieldName = "UpperMenuCd";
        authTree.OptionsBehavior.Editable = true;
        authTree.OptionsView.ShowIndicator = false;
        authTree.OptionsView.AutoWidth = false;

        repoCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
        authTree.RepositoryItems.Add(repoCheck);

        var colMenuNm = new TreeListColumn { FieldName = "MenuNm", Caption = "메뉴", VisibleIndex = 0, Width = 220 };
        var colView = new TreeListColumn { FieldName = "ViewYn", Caption = "조회", VisibleIndex = 1, Width = 55, ColumnEdit = repoCheck };
        var colInsert = new TreeListColumn { FieldName = "InsertYn", Caption = "등록", VisibleIndex = 2, Width = 55, ColumnEdit = repoCheck };
        var colUpdate = new TreeListColumn { FieldName = "UpdateYn", Caption = "수정", VisibleIndex = 3, Width = 55, ColumnEdit = repoCheck };
        var colDelete = new TreeListColumn { FieldName = "DeleteYn", Caption = "삭제", VisibleIndex = 4, Width = 55, ColumnEdit = repoCheck };
        var colExcel = new TreeListColumn { FieldName = "ExcelYn", Caption = "엑셀", VisibleIndex = 5, Width = 55, ColumnEdit = repoCheck };
        foreach (var col in new[] { colMenuNm, colView, colInsert, colUpdate, colDelete, colExcel })
        {
            col.Visible = true;
            col.OptionsColumn.AllowEdit = col != colMenuNm;
            authTree.Columns.Add(col);
        }

        // GROUP(상위분류) 행은 조회 체크만 의미가 있다(PermissionAssignForm과 같은 규칙 - 그
        // 클래스 설명 참고).
        authTree.ShowingEditor += (s, e) =>
        {
            var node = authTree.FocusedNode;
            if (node == null || authTree.FocusedColumn == colMenuNm) return;
            var menuType = (string)node.GetValue("MenuType");
            if (menuType == "GROUP" && authTree.FocusedColumn != colView) e.Cancel = true;
        };

        tabAuth.Controls.Add(authTree);
    }

    private void Gvw1_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is UserListItemDto user) _pendingLoadTask = EnterEditModeAsync(user);
    }

    public override async Task QueryClick()
    {
        if (outerTab.SelectedTabPage == tabGroupPage) await QueryGroupsAsync();
        else await QueryUsersAsync();
    }

    public override Task NewClick()
    {
        if (outerTab.SelectedTabPage == tabGroupPage) EnterNewGroupMode();
        else EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (outerTab.SelectedTabPage == tabGroupPage) await DeleteGroupAsync();
        else await DeleteUserAsync();
    }

    public override async Task SaveClick()
    {
        if (outerTab.SelectedTabPage == tabGroupPage) await SaveGroupAsync();
        else await SaveUserAsync();
    }

    private async Task QueryUsersAsync()
    {
        var query = $"api/users?userId={Uri.EscapeDataString(txtSearchUserId.Text.Trim())}&userNm={Uri.EscapeDataString(txtSearchUserNm.Text.Trim())}";
        _users = await ApiClient.GetAsync<List<UserListItemDto>>(query) ?? new();

        // grd1을 재바인딩하면 첫 행 자동포커스로 그룹/권한 로딩(+ 권한트리 ExpandAll)이
        // 뒤따라 일어나는데, 그 사이 화면이 다시 그려지며 깜빡이는 게 눈에 띈다는 피드백이
        // 있었다(실제로 겪음 - 권한트리만 BeginUpdate/EndUpdate로 감싸는 걸로는 부족했다,
        // 그리드 자체의 첫 바인딩 레이아웃 비용도 있었다). 그 로딩이 전부 끝날 때까지
        // 화면 전체 그리기를 잠깐 꺼둔다.
        DrawingSuspension.Suspend(this);
        try
        {
            _pendingLoadTask = null;
            grd1.DataSource = _users;
            if (_pendingLoadTask != null) await _pendingLoadTask;

            // API 호출이 느려서(첫 조회 콜드스타트) ShellForm의 busy 오버레이(BaseForm.ShowBusy)가
            // 이미 화면에 떠 있는 상태로 여기까지 왔을 수 있다. HideBusy를 Resume 이후로 미루면
            // "오버레이가 뜬 채로 그림이 멈췄다가(Resume의 강제 재도색) 바로 지워지는" 두 단계로
            // 보여서 화면 전체가 깜빡이는 것처럼 느껴진다(실제로 겪음 - 첫 조회에서만 재현,
            // 두 번째부터는 API가 빨라 오버레이 자체가 안 떠서 안 보였다). 그리기가 꺼져있는
            // 지금 오버레이까지 같이 지워두면, Resume이 최종 상태(오버레이 없는 완성된 화면)
            // 한 번만 보여준다.
            HideBusy();
        }
        finally
        {
            DrawingSuspension.Resume(this);
        }
    }

    private async Task DeleteUserAsync()
    {
        if (_editingUserId == null)
        {
            AppMessageBox.Show("사용중지 처리할 사용자를 먼저 선택해주세요.", "안내");
            return;
        }

        var confirm = AppMessageBox.Show($"'{txtUserNm.Text}({_editingUserId})' 사용자를 사용중지 처리하시겠습니까?",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/users/{_editingUserId}");
        _editingUserId = null;
        await QueryUsersAsync();
        EnterNewMode();
        Toast.Show("사용중지 처리되었습니다.");
    }

    private void EnterNewMode()
    {
        _editingUserId = null;
        txtUserId.Text = string.Empty;
        txtUserId.ReadOnly = false;
        txtUserNm.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtPassword.Enabled = true;
        txtEmpNo.Text = string.Empty;
        txtDeptCd.Text = string.Empty;
        txtPositionNm.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        chkIsAdmin.Checked = false;
        chkUseYn.Checked = true;
        chkUseYn.Enabled = false; // 신규는 서버에서 항상 'Y'로 생성됨(UserEditForm과 같은 규칙)

        _groups = new();
        grdGroup.DataSource = null;
        _authItems = new();
        authTree.DataSource = null;
        authTree.Enabled = false;
    }

    private async Task EnterEditModeAsync(UserListItemDto user)
    {
        _editingUserId = user.UserId;
        txtUserId.Text = user.UserId;
        txtUserId.ReadOnly = true; // 아이디는 PK라 수정 불가(UserEditForm과 같은 규칙)
        txtUserNm.Text = user.UserNm;
        txtPassword.Text = string.Empty;
        txtPassword.Enabled = false; // 비밀번호 변경은 별도 기능으로 분리(UserEditForm과 같은 이유)
        txtEmpNo.Text = user.EmpNo ?? string.Empty;
        txtDeptCd.Text = user.DeptCd ?? string.Empty;
        txtPositionNm.Text = user.PositionNm ?? string.Empty;
        txtEmail.Text = user.Email ?? string.Empty;
        txtMobileNo.Text = user.MobileNo ?? string.Empty;
        chkIsAdmin.Checked = user.IsAdminYn;
        chkUseYn.Checked = user.UseYn;
        chkUseYn.Enabled = true;

        await LoadGroupsAsync(user.UserId);
        await LoadAuthAsync(user.UserId);
    }

    private async Task LoadGroupsAsync(string userId)
    {
        _groups = await ApiClient.GetAsync<List<UserGroupAssignDto>>($"api/users/{userId}/groups") ?? new();
        grdGroup.DataSource = _groups;
    }

    private async Task LoadAuthAsync(string userId)
    {
        _authItems = await ApiClient.GetAsync<List<MenuAuthItemDto>>($"api/menu-auth?targetType=USER&targetCd={Uri.EscapeDataString(userId)}") ?? new();

        // 트리가 처음 데이터를 받아 전체 노드를 한꺼번에 펼칠 때(ExpandAll) 레이아웃 재계산
        // 비용이 커서, BeginUpdate/EndUpdate로 감싸지 않으면 그 사이 화면 전체가 잠깐 깜빡이며
        // 다시 그려지는 게 눈에 띈다(실제로 겪음 - 이 화면에서 첫 조회 때만 전체가 깜빡이고
        // 그다음부터는 안 그랬는데, 두 번째 선택부터는 트리가 이미 펼쳐져 있어 재계산이
        // 가벼워서였다).
        authTree.BeginUpdate();
        try
        {
            authTree.DataSource = _authItems;
            authTree.Enabled = true;
            authTree.ExpandAll();
        }
        finally
        {
            authTree.EndUpdate();
        }
    }

    private async Task SaveUserAsync()
    {
        if (string.IsNullOrWhiteSpace(txtUserId.Text) || string.IsNullOrWhiteSpace(txtUserNm.Text))
        {
            AppMessageBox.Show("아이디와 이름은 필수입니다.", "확인");
            return;
        }
        if (_editingUserId == null && string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            AppMessageBox.Show("비밀번호는 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingUserId == null;
        ApiResult? result;

        if (wasNew)
        {
            var req = new UserCreateRequest
            {
                UserId = txtUserId.Text,
                UserNm = txtUserNm.Text,
                Password = txtPassword.Text,
                EmpNo = txtEmpNo.Text,
                DeptCd = txtDeptCd.Text,
                PositionNm = txtPositionNm.Text,
                Email = txtEmail.Text,
                MobileNo = txtMobileNo.Text,
                IsAdminYn = chkIsAdmin.Checked
            };
            result = await ApiClient.PostAsync<UserCreateRequest, ApiResult>("api/users", req);
        }
        else
        {
            var req = new UserUpdateRequest
            {
                UserNm = txtUserNm.Text,
                EmpNo = txtEmpNo.Text,
                DeptCd = txtDeptCd.Text,
                PositionNm = txtPositionNm.Text,
                Email = txtEmail.Text,
                MobileNo = txtMobileNo.Text,
                UseYn = chkUseYn.Checked,
                IsAdminYn = chkIsAdmin.Checked
            };
            result = await ApiClient.PutAsync<UserUpdateRequest, ApiResult>($"api/users/{_editingUserId}", req);
        }

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var savedUserId = wasNew ? (result.GeneratedCode ?? txtUserId.Text) : _editingUserId!;

        // 신규는 방금 만들어진 사용자라 그룹/권한 탭이 아직 로드 전이다 - 저장 후 재조회에서
        // 아래 LocateByValue로 그 행에 포커스를 다시 옮기면 Gvw1_FocusedRowObjectChanged가
        // EnterEditModeAsync를 다시 태워서 그룹/권한 탭을 바로 채워준다(기존 UserEditForm도
        // 수정모드에서만 그룹을 저장했던 것과 같은 이유로 신규일 때는 그룹/권한 저장 자체는
        // 건너뛴다 - 신규 등록 직후 그룹부터 배정하고 싶으면 저장 버튼을 한 번 더 눌러서 진행).
        if (!wasNew)
        {
            gvwGroup.CloseEditor();
            gvwGroup.UpdateCurrentRow();
            var checkedGrpCds = _groups.Where(g => g.IsMember).Select(g => g.UserGrpCd).ToList();
            var groupResult = await ApiClient.PutAsync<UpdateUserGroupsRequest, ApiResult>(
                $"api/users/{savedUserId}/groups", new UpdateUserGroupsRequest { UserGrpCds = checkedGrpCds });
            if (groupResult == null || !groupResult.Success)
            {
                AppMessageBox.Show(groupResult?.Message ?? "소속 그룹 저장에 실패했습니다.", "저장 실패");
                return;
            }

            authTree.CloseEditor();
            authTree.PostEditor();
            var authResult = await ApiClient.PutAsync<SaveMenuAuthRequest, ApiResult>("api/menu-auth",
                new SaveMenuAuthRequest { TargetType = "USER", TargetCd = savedUserId, Items = _authItems });
            if (authResult == null || !authResult.Success)
            {
                AppMessageBox.Show(authResult?.Message ?? "권한 저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        await QueryUsersAsync();

        // 리스트 인덱스가 아니라 값으로 찾는다 - gvw1에 정렬/필터가 걸려있으면 인덱스 순서가
        // _users 리스트 순서와 어긋날 수 있어서(LocateByValue는 그 어떤 정렬 상태에서도 정확함,
        // 사용자그룹관리 탭의 SaveGroupAsync/gvw3와 같은 이유).
        var savedRowHandle = gvw1.LocateByValue("UserId", savedUserId);
        if (savedRowHandle >= 0) gvw1.FocusedRowHandle = savedRowHandle;

        Toast.Show(wasNew ? "사용자가 등록되었습니다." : "수정되었습니다.");
    }

    // ===================== 사용자그룹관리 탭 구현 =====================

    private void BuildHeader2()
    {
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        panHeader2.Controls.Add(bottomBorder);

        AddHeaderLabel(panHeader2, "그룹명", 16);
        txtSearchGrpNm.Font = AppFonts.Body;
        txtSearchGrpNm.Location = new Point(58, 11);
        txtSearchGrpNm.Size = new Size(180, 24);
        panHeader2.Controls.Add(txtSearchGrpNm);

        void SearchOnEnter(object? s, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            _ = QueryClick();
        }
        txtSearchGrpNm.KeyDown += SearchOnEnter;
    }

    private void BuildGroupList()
    {
        grd3.MainView = gvw3;
        gvw3.OptionsBehavior.Editable = false; // 조회 전용 - 저장은 항상 우측 패널에서
        gvw3.OptionsView.ShowGroupPanel = false;
        gvw3.Columns.AddField(nameof(UserGroupListItemDto.UserGrpCd)).Caption = "그룹코드";
        gvw3.Columns.AddField(nameof(UserGroupListItemDto.UserGrpNm)).Caption = "그룹명";
        gvw3.Columns.AddField(nameof(UserGroupListItemDto.Description)).Caption = "설명";
        gvw3.Columns.AddField(nameof(UserGroupListItemDto.MemberCount)).Caption = "인원";
        gvw3.Columns.AddField(nameof(UserGroupListItemDto.UseYn)).Caption = "사용";
        foreach (DevExpress.XtraGrid.Columns.GridColumn c in gvw3.Columns) c.Visible = true;

        panLeft2.Controls.Add(grd3);
        panLeft2.Controls.Add(sectionHeaderGroupList);
    }

    private void BuildGroupFields()
    {
        const int labelW = 76, col1X = 12, col2X = 200, rowH = 30;
        var y = 8;

        void Row(string label1, Control c1, string label2, Control c2)
        {
            AddField(panGroupFields, label1, c1, col1X, y, labelW, 110);
            if (c2 != null) AddField(panGroupFields, label2, c2, col2X, y, labelW, 160);
            y += rowH;
        }

        txtUserGrpCd.Font = AppFonts.Body;
        txtUserGrpNm.Font = AppFonts.Body;
        txtGroupDescription.Font = AppFonts.Body;
        spnGroupSortOrder.Font = AppFonts.Body;

        Row("그룹코드", txtUserGrpCd, "그룹명", txtUserGrpNm);
        Row("설명", txtGroupDescription, "정렬순서", spnGroupSortOrder);

        chkGroupUseYn.Location = new Point(col1X + labelW + 6, y + 2);
        chkGroupUseYn.Size = new Size(80, 20);
        panGroupFields.Controls.Add(chkGroupUseYn);
    }

    /// <summary>소속 사용자 배정 - 체크그리드에서 여러 명을 한 번에 체크해서 저장할 수 있다
    /// (그룹 관점에서 여러 사용자를 동시에 배정하는 게 더 효율적이라는 판단에 따라 편집 가능하게
    /// 구성). UserGroupListForm이 이미 쓰던 api/user-groups/{cd}/members를 그대로 쓴다.</summary>
    private void BuildMembersPanel()
    {
        grd4.MainView = gvw4;
        gvw4.OptionsView.ShowGroupPanel = false;
        gvw4.OptionsBehavior.Editable = true;

        // BuildGroupTab의 gvwGroup과 같은 이유로 컬럼을 로드 시점이 아니라 여기서 미리 정의한다 -
        // 그룹을 아직 선택하기 전(신규모드)에도 헤더가 비어있지 않게.
        gvw4.Columns.AddField(nameof(UserGroupMemberDto.UserId)).Caption = "아이디";
        gvw4.Columns.AddField(nameof(UserGroupMemberDto.UserNm)).Caption = "이름";
        gvw4.Columns.AddField(nameof(UserGroupMemberDto.DeptNm)).Caption = "부서";
        gvw4.Columns.AddField(nameof(UserGroupMemberDto.IsMember)).Caption = "소속";
        foreach (DevExpress.XtraGrid.Columns.GridColumn c in gvw4.Columns) c.Visible = true;
        if (gvw4.Columns[nameof(UserGroupMemberDto.UserId)] is { } colId) colId.OptionsColumn.AllowEdit = false;
        if (gvw4.Columns[nameof(UserGroupMemberDto.UserNm)] is { } colNm) colNm.OptionsColumn.AllowEdit = false;
        if (gvw4.Columns[nameof(UserGroupMemberDto.DeptNm)] is { } colDept) colDept.OptionsColumn.AllowEdit = false;
        if (gvw4.Columns[nameof(UserGroupMemberDto.IsMember)] is { } colMember) colMember.Width = 50;

        panMembers.Controls.Add(grd4);
        panMembers.Controls.Add(sectionHeaderMembers);
    }

    /// <summary>그룹 대상 권한부여 - 사용자관리 탭의 BuildAuthTab과 같은 구성이며, 대상만
    /// TargetType="GRP"로 고정된다.</summary>
    private void BuildGroupAuthPanel()
    {
        authTree2.KeyFieldName = "MenuCd";
        authTree2.ParentFieldName = "UpperMenuCd";
        authTree2.OptionsBehavior.Editable = true;
        authTree2.OptionsView.ShowIndicator = false;
        authTree2.OptionsView.AutoWidth = false;

        repoCheck2.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
        authTree2.RepositoryItems.Add(repoCheck2);

        var colMenuNm = new TreeListColumn { FieldName = "MenuNm", Caption = "메뉴", VisibleIndex = 0, Width = 220 };
        var colView = new TreeListColumn { FieldName = "ViewYn", Caption = "조회", VisibleIndex = 1, Width = 55, ColumnEdit = repoCheck2 };
        var colInsert = new TreeListColumn { FieldName = "InsertYn", Caption = "등록", VisibleIndex = 2, Width = 55, ColumnEdit = repoCheck2 };
        var colUpdate = new TreeListColumn { FieldName = "UpdateYn", Caption = "수정", VisibleIndex = 3, Width = 55, ColumnEdit = repoCheck2 };
        var colDelete = new TreeListColumn { FieldName = "DeleteYn", Caption = "삭제", VisibleIndex = 4, Width = 55, ColumnEdit = repoCheck2 };
        var colExcel = new TreeListColumn { FieldName = "ExcelYn", Caption = "엑셀", VisibleIndex = 5, Width = 55, ColumnEdit = repoCheck2 };
        foreach (var col in new[] { colMenuNm, colView, colInsert, colUpdate, colDelete, colExcel })
        {
            col.Visible = true;
            col.OptionsColumn.AllowEdit = col != colMenuNm;
            authTree2.Columns.Add(col);
        }

        authTree2.ShowingEditor += (s, e) =>
        {
            var node = authTree2.FocusedNode;
            if (node == null || authTree2.FocusedColumn == colMenuNm) return;
            var menuType = (string)node.GetValue("MenuType");
            if (menuType == "GROUP" && authTree2.FocusedColumn != colView) e.Cancel = true;
        };

        panGroupAuth.Controls.Add(authTree2);
        panGroupAuth.Controls.Add(sectionHeaderGroupAuth);
    }

    private void Gvw3_FocusedRowObjectChanged(object? sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is UserGroupListItemDto group) EnterEditGroupMode(group);
    }

    private async Task QueryGroupsAsync()
    {
        var query = $"api/user-groups?userGrpNm={Uri.EscapeDataString(txtSearchGrpNm.Text.Trim())}";
        _groupsList = await ApiClient.GetAsync<List<UserGroupListItemDto>>(query) ?? new();

        // QueryUsersAsync와 같은 이유(첫 행 자동포커스 -> 소속사용자/권한 로딩 -> 권한트리
        // ExpandAll이 뒤따르며 화면이 깜빡이는 문제)로 감싼다.
        DrawingSuspension.Suspend(this);
        try
        {
            _pendingLoadTask = null;
            grd3.DataSource = _groupsList;

            // 수정 중이던 그룹이 검색결과에서 빠졌으면(삭제됐거나 검색조건에 안 맞음) 신규모드로 복귀
            if (_editingUserGrpCd != null && _groupsList.All(g => g.UserGrpCd != _editingUserGrpCd))
            {
                EnterNewGroupMode();
            }

            if (_pendingLoadTask != null) await _pendingLoadTask;

            // QueryUsersAsync와 같은 이유(busy 오버레이가 Resume 이후에 지워지면 "떴다 사라지는"
            // 두 단계로 보여 깜빡임처럼 느껴짐) - 그리기가 꺼져있는 동안 같이 지운다.
            HideBusy();
        }
        finally
        {
            DrawingSuspension.Resume(this);
        }
    }

    private async Task DeleteGroupAsync()
    {
        if (_editingUserGrpCd == null)
        {
            AppMessageBox.Show("사용중지 처리할 그룹을 먼저 선택해주세요.", "안내");
            return;
        }

        var group = _groupsList.FirstOrDefault(g => g.UserGrpCd == _editingUserGrpCd);
        var confirm = AppMessageBox.Show(
            $"'{group?.UserGrpNm}({_editingUserGrpCd})' 그룹을 사용중지 처리하시겠습니까?\n소속된 사용자 {group?.MemberCount ?? 0}명의 그룹 권한 합산에서 이 그룹이 빠집니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        await ApiClient.DeleteAsync($"api/user-groups/{_editingUserGrpCd}");
        _editingUserGrpCd = null;
        await QueryGroupsAsync();
        EnterNewGroupMode();
        Toast.Show("사용중지 처리되었습니다.");
    }

    private void EnterNewGroupMode()
    {
        _editingUserGrpCd = null;
        txtUserGrpCd.Text = string.Empty;
        txtUserGrpCd.ReadOnly = false;
        txtUserGrpNm.Text = string.Empty;
        txtGroupDescription.Text = string.Empty;
        spnGroupSortOrder.Value = 0;
        chkGroupUseYn.Checked = true;
        chkGroupUseYn.Enabled = false; // 신규는 서버에서 항상 'Y'로 생성됨(UserGroupListForm과 같은 규칙)

        _members = new();
        grd4.DataSource = null;
        _groupAuthItems = new();
        authTree2.DataSource = null;
        authTree2.Enabled = false;
    }

    private void EnterEditGroupMode(UserGroupListItemDto group)
    {
        // grd3.DataSource를 다시 세팅할 때마다(QueryGroupsAsync 안) 포커스 행이 새로 잡히면서
        // FocusedRowObjectChanged가 또 발생한다 - 같은 그룹인데 소속사용자/권한을 또 불러오는
        // 불필요한 재조회를 막는다(UserGroupListForm의 EnterEditMode와 같은 이유).
        var isSameGroup = _editingUserGrpCd == group.UserGrpCd;
        _editingUserGrpCd = group.UserGrpCd;

        txtUserGrpCd.Text = group.UserGrpCd;
        txtUserGrpCd.ReadOnly = true; // 그룹코드는 PK라 수정 불가
        txtUserGrpNm.Text = group.UserGrpNm;
        txtGroupDescription.Text = group.Description ?? string.Empty;
        spnGroupSortOrder.Value = group.SortOrder;
        chkGroupUseYn.Checked = group.UseYn;
        chkGroupUseYn.Enabled = true;

        if (!isSameGroup)
        {
            _pendingLoadTask = Task.WhenAll(LoadMembersAsync(group.UserGrpCd), LoadGroupAuthAsync(group.UserGrpCd));
        }
    }

    private async Task LoadMembersAsync(string userGrpCd)
    {
        _members = await ApiClient.GetAsync<List<UserGroupMemberDto>>($"api/user-groups/{userGrpCd}/members") ?? new();
        grd4.DataSource = _members;
    }

    private async Task LoadGroupAuthAsync(string userGrpCd)
    {
        _groupAuthItems = await ApiClient.GetAsync<List<MenuAuthItemDto>>($"api/menu-auth?targetType=GRP&targetCd={Uri.EscapeDataString(userGrpCd)}") ?? new();

        // 사용자관리 탭의 LoadAuthAsync와 같은 이유(첫 ExpandAll 때만 전체 화면이 깜빡이는 문제) -
        // BeginUpdate/EndUpdate로 감싼다.
        authTree2.BeginUpdate();
        try
        {
            authTree2.DataSource = _groupAuthItems;
            authTree2.Enabled = true;
            authTree2.ExpandAll();
        }
        finally
        {
            authTree2.EndUpdate();
        }
    }

    private async Task SaveGroupAsync()
    {
        if (string.IsNullOrWhiteSpace(txtUserGrpCd.Text) || string.IsNullOrWhiteSpace(txtUserGrpNm.Text))
        {
            AppMessageBox.Show("그룹코드와 그룹명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingUserGrpCd == null;
        ApiResult? result;
        var savedUserGrpCd = wasNew ? txtUserGrpCd.Text : _editingUserGrpCd!;

        if (wasNew)
        {
            var req = new UserGroupCreateRequest
            {
                UserGrpCd = txtUserGrpCd.Text,
                UserGrpNm = txtUserGrpNm.Text,
                Description = txtGroupDescription.Text,
                SortOrder = (int)spnGroupSortOrder.Value
            };
            result = await ApiClient.PostAsync<UserGroupCreateRequest, ApiResult>("api/user-groups", req);
        }
        else
        {
            var req = new UserGroupUpdateRequest
            {
                UserGrpNm = txtUserGrpNm.Text,
                Description = txtGroupDescription.Text,
                SortOrder = (int)spnGroupSortOrder.Value,
                UseYn = chkGroupUseYn.Checked
            };
            result = await ApiClient.PutAsync<UserGroupUpdateRequest, ApiResult>($"api/user-groups/{_editingUserGrpCd}", req);
        }

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        // 소속 사용자 배정 + 권한은 수정모드(그룹이 이미 존재)일 때만 같이 저장한다 - 신규모드는
        // 두 그리드 다 비어있다(EnterNewGroupMode 참고).
        if (!wasNew)
        {
            gvw4.CloseEditor();
            gvw4.UpdateCurrentRow();
            var checkedUserIds = _members.Where(m => m.IsMember).Select(m => m.UserId).ToList();
            var memberResult = await ApiClient.PutAsync<UpdateGroupMembersRequest, ApiResult>(
                $"api/user-groups/{savedUserGrpCd}/members", new UpdateGroupMembersRequest { UserIds = checkedUserIds });
            if (memberResult == null || !memberResult.Success)
            {
                AppMessageBox.Show(memberResult?.Message ?? "소속 사용자 저장에 실패했습니다.", "저장 실패");
                return;
            }

            authTree2.CloseEditor();
            authTree2.PostEditor();
            var authResult = await ApiClient.PutAsync<SaveMenuAuthRequest, ApiResult>("api/menu-auth",
                new SaveMenuAuthRequest { TargetType = "GRP", TargetCd = savedUserGrpCd, Items = _groupAuthItems });
            if (authResult == null || !authResult.Success)
            {
                AppMessageBox.Show(authResult?.Message ?? "권한 저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        await QueryGroupsAsync();

        // 리스트 인덱스가 아니라 값으로 찾는다 - gvw3에 정렬/필터가 걸려있으면 인덱스 순서가
        // _groupsList 리스트 순서와 어긋날 수 있어서(LocateByValue는 그 어떤 정렬 상태에서도 정확함).
        var savedRowHandle = gvw3.LocateByValue("UserGrpCd", savedUserGrpCd);
        if (savedRowHandle >= 0) gvw3.FocusedRowHandle = savedRowHandle;

        Toast.Show(wasNew ? "그룹이 등록되었습니다." : "수정되었습니다.");
    }
}
