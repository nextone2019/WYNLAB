using DevExpress.XtraGrid.Views.Base;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM;

/// <summary>
/// 사용자권한관리 화면 - BACK_frmUserManage(기존 사용자관리 화면)의 후속작. 디자이너로 직접 배치한
/// 화면이라(frmMinorCode를 Save As해서 시작함) 컨트롤 이름이 그쪽 관례(txtuser_id 등)를 따른다.
///
/// 탭1 "사용자별 권한관리": grd1(사용자 LIST) 선택 -> 우측에 사용자정보(txtuser_id/txtuser_nm/
/// checkBoxWyn1=사용여부) + 소속그룹 체크그리드(grd3) + 메뉴권한트리(tree1).
/// 탭2 "사용자그룹별 권한관리": grd2(사용자그룹 LIST) 선택 -> 우측에 그룹정보(txtuser_grp_cd/
/// txtuser_grp_nm/txtdescription) + 소속 사용자 체크그리드(grd4) + 메뉴권한트리(tree2).
///
/// BACK_frmUserManage가 이미 쓰던 api/users, api/users/{id}/groups, api/user-groups, api/menu-auth를
/// 그대로 재사용한다 - 서버는 전혀 안 건드렸고, 권한 체크도 전부 MenuCd="SM_USER" 기준이라
/// 그 값을 그대로 쓴다.
/// </summary>
public partial class frmUserAuth : BaseForm
{
    // ===== 탭1: 사용자별 권한관리 =====
    private List<UserListItemDto> _users = new();
    private string? _editingUserId; // null이면 신규모드

    private List<UserGroupAssignDto> _groups = new(); // grd3 - 전체 그룹 + 이 사용자의 소속여부(체크)
    private List<MenuAuthItemDto> _authItems = new(); // tree1

    // FocusedRowObjectChanged 핸들러 안에서 시작하는 그룹/권한 로딩(비동기)이 끝날 때까지
    // DrawingSuspension을 유지하기 위한 대기용 - BACK_frmUserManage와 같은 이유
    // (첫 조회에서 그리드+트리가 연달아 다시 그려지며 화면 전체가 깜빡이는 문제,
    // feedback_busy_overlay_drawing_suspension_flicker 메모리 참고).
    private Task? _pendingLoadTask;

    // ===== 탭2: 사용자그룹별 권한관리 =====
    private List<UserGroupListItemDto> _groupsList = new();
    private string? _editingUserGrpCd; // null이면 신규모드

    private List<UserGroupMemberDto> _members = new(); // grd4 - 전체 사용자 + 이 그룹 소속여부(체크)
    private List<MenuAuthItemDto> _groupAuthItems = new(); // tree2

    public frmUserAuth()
    {
        InitializeComponent();

        Text = "사용자권한관리";
        MenuCd = "SM_USER";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw2.FocusedRowObjectChanged += Gvw2_FocusedRowObjectChanged;

        ConfigureAuthTree(tree1);
        ConfigureAuthTree(tree2);

        // 탭이 바뀔 때마다 반대편 탭에서 방금 수정했을 수도 있는 데이터를 다시 불러온다 -
        // 그리드 포커스행이 그대로면 FocusedRowObjectChanged가 재발생하지 않아 저장 전 상태로
        // 남기 때문(BACK_frmUserManage.OnOuterTabChangedAsync와 같은 이유).
        tabControlWyn1.SelectedPageChanged += async (s, e) => await OnOuterTabChangedAsync();

        SearchOnEnter(txtUserGrpIdQ);
        SearchOnEnter(txtUserGrpCdQ);

        EnterNewMode();
        EnterNewGroupMode();
        // 오픈 시 자동 조회하지 않는다 - 검색창에 조건을 넣고 조회 버튼(또는 Ctrl+Q)을 눌러야 뜬다.
    }

    private void SearchOnEnter(TextEditWyn box)
    {
        box.KeyDown += (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            _ = QueryClick();
        };
    }

    /// <summary>컬럼 자체는 이제 디자이너(tree1/tree2 각각의 Columns)가 정의한다 - 여기서는 그
    /// 컬럼들을 그대로 쓰기 위한 최소한의 데이터 바인딩(트리 구조)과, GROUP(상위분류) 행은
    /// 조회 체크만 의미가 있어 나머지 컬럼은 편집을 막는 규칙(BACK_frmUserManage.BuildAuthTab과
    /// 같은 규칙)만 붙인다. 컬럼 이름(FieldName)으로 찾으므로 디자이너에서 컬럼을 더 추가하거나
    /// 순서를 바꿔도 이 로직은 그대로 동작한다.</summary>
    private static void ConfigureAuthTree(TreeListWyn tree)
    {
        tree.KeyFieldName = "MenuCd";
        tree.ParentFieldName = "UpperMenuCd";
        tree.OptionsBehavior.Editable = true;

        var colMenuNm = tree.Columns["MenuNm"];
        var colView = tree.Columns["ViewYn"];

        tree.ShowingEditor += (s, e) =>
        {
            var node = tree.FocusedNode;
            if (node == null || tree.FocusedColumn == colMenuNm) return;
            var menuType = (string)node.GetValue("MenuType");
            if (menuType == "GROUP" && tree.FocusedColumn != colView) e.Cancel = true;
        };
    }

    private async Task OnOuterTabChangedAsync()
    {
        if (tabControlWyn1.SelectedTabPage == xtraTabPage2)
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

    public override async Task QueryClick()
    {
        if (tabControlWyn1.SelectedTabPage == xtraTabPage2) await QueryGroupsAsync();
        else await QueryUsersAsync();
    }

    public override Task NewClick()
    {
        if (tabControlWyn1.SelectedTabPage == xtraTabPage2) EnterNewGroupMode();
        else EnterNewMode();
        return Task.CompletedTask;
    }

    public override async Task DeleteClick()
    {
        if (tabControlWyn1.SelectedTabPage == xtraTabPage2) await DeleteGroupAsync();
        else await DeleteUserAsync();
    }

    public override async Task SaveClick()
    {
        if (tabControlWyn1.SelectedTabPage == xtraTabPage2) await SaveGroupAsync();
        else await SaveUserAsync();
    }

    // 디자이너가 두 탭의 행추가/행삭제 버튼(simpleButton1/2, btnAddRow2/btnDeletRow2)을 전부
    // 이 두 메서드에 연결해뒀다. 이 화면엔 행추가/삭제가 필요한 그리드가 없어서(grd3는 체크만
    // 하는 고정 목록, 트리는 메뉴 구조 그대로) 지금은 아무 동작도 없다 - frmMinorCode를 복사할 때
    // 같이 따라온 버튼이라 필요 없으면 디자이너에서 지워도 된다.
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;

    private async void btnAddRow2_Click(object sender, EventArgs e) => await NewRowClick();
    private async void btnDeletRow2_Click(object sender, EventArgs e) => await DeleteRowClick();

    // ================================================================================
    // 탭1: 사용자별 권한관리
    // ================================================================================

    private void Gvw1_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is UserListItemDto user) _pendingLoadTask = EnterEditModeAsync(user);
    }

    private async Task QueryUsersAsync()
    {
        var keyword = txtUserGrpIdQ.Text.Trim();
        _users = await ApiClient.GetAsync<List<UserListItemDto>>(
            $"api/users?userId={Uri.EscapeDataString(keyword)}&userNm={Uri.EscapeDataString(keyword)}") ?? new();

        // 첫 조회처럼 API가 느릴 때, 그리드 재바인딩(첫 행 자동포커스 -> 그룹/권한 로딩 -> 트리
        // ExpandAll)이 끝나기 전에 화면이 다시 그려지면 busy 오버레이가 뜬 채로 멈췄다 바로
        // 지워지는 두 단계로 보여 깜빡인다 - 로딩이 전부 끝날 때까지, 그리고 오버레이를 먼저
        // 지운 다음에야 화면 그리기를 다시 켠다(feedback_busy_overlay_drawing_suspension_flicker).
        DrawingSuspension.Suspend(this);
        try
        {
            _pendingLoadTask = null;
            grd1.DataSource = _users;
            if (_pendingLoadTask != null) await _pendingLoadTask;
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

        var confirm = AppMessageBox.Show($"'{txtuser_nm.Text}({_editingUserId})' 사용자를 사용중지 처리하시겠습니까?",
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
        txtuser_id.Text = string.Empty;
        txtuser_id.ReadOnly = false;
        txtuser_nm.Text = string.Empty;
        checkBoxWyn1.Checked = true;
        checkBoxWyn1.Enabled = false; // 신규는 서버에서 항상 'Y'로 생성됨(BACK_frmUserManage와 같은 규칙)

        _groups = new();
        grd3.DataSource = null;
        _authItems = new();
        tree1.DataSource = null;
        //tree1.Enabled = false;

        txtuser_id.Focus();
    }

    private async Task EnterEditModeAsync(UserListItemDto user)
    {
        _editingUserId = user.UserId;
        txtuser_id.Text = user.UserId;
        txtuser_id.ReadOnly = true; // 아이디는 PK라 수정 불가
        txtuser_nm.Text = user.UserNm;
        checkBoxWyn1.Checked = user.UseYn;
        checkBoxWyn1.Enabled = true;

        await LoadGroupsAsync(user.UserId);
        await LoadAuthAsync(user.UserId);
    }

    private async Task LoadGroupsAsync(string userId)
    {
        _groups = await ApiClient.GetAsync<List<UserGroupAssignDto>>($"api/users/{userId}/groups") ?? new();
        grd3.DataSource = _groups;
    }

    private async Task LoadAuthAsync(string userId)
    {
        _authItems = await ApiClient.GetAsync<List<MenuAuthItemDto>>(
            $"api/menu-auth?targetType=USER&targetCd={Uri.EscapeDataString(userId)}") ?? new();

        tree1.BeginUpdate();
        try
        {
            tree1.DataSource = _authItems;
            tree1.Enabled = true;
            tree1.ExpandAll();
        }
        finally
        {
            tree1.EndUpdate();
        }
    }

    private async Task SaveUserAsync()
    {
        if (string.IsNullOrWhiteSpace(txtuser_id.Text) || string.IsNullOrWhiteSpace(txtuser_nm.Text))
        {
            AppMessageBox.Show("아이디와 이름은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingUserId == null;
        ApiResult? result;

        if (wasNew)
        {
            // 이 화면엔 비밀번호 입력 UI가 없다(의도적으로 뺌) - 초기 비밀번호는 일단 아이디와
            // 같은 값으로 채워서 계정만 먼저 만들고, 실제 비밀번호 초기화는 별도 기능이 생기면
            // 그쪽으로 옮기면 된다. 지금은 임시 처리라는 점을 알아둘 것.
            var req = new UserCreateRequest
            {
                UserId = txtuser_id.Text,
                UserNm = txtuser_nm.Text,
                Password = txtuser_id.Text
            };
            result = await ApiClient.PostAsync<UserCreateRequest, ApiResult>("api/users", req);
        }
        else
        {
            var req = new UserUpdateRequest
            {
                UserNm = txtuser_nm.Text,
                UseYn = checkBoxWyn1.Checked
            };
            result = await ApiClient.PutAsync<UserUpdateRequest, ApiResult>($"api/users/{_editingUserId}", req);
        }

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var savedUserId = wasNew ? (result.GeneratedCode ?? txtuser_id.Text) : _editingUserId!;

        if (!wasNew)
        {
            gvw3.CloseEditor();
            gvw3.UpdateCurrentRow();
            var checkedGrpCds = _groups.Where(g => g.IsMember).Select(g => g.UserGrpCd).ToList();
            var groupResult = await ApiClient.PutAsync<UpdateUserGroupsRequest, ApiResult>(
                $"api/users/{savedUserId}/groups", new UpdateUserGroupsRequest { UserGrpCds = checkedGrpCds });
            if (groupResult == null || !groupResult.Success)
            {
                AppMessageBox.Show(groupResult?.Message ?? "소속 그룹 저장에 실패했습니다.", "저장 실패");
                return;
            }

            tree1.CloseEditor();
            tree1.PostEditor();
            var authResult = await ApiClient.PutAsync<SaveMenuAuthRequest, ApiResult>("api/menu-auth",
                new SaveMenuAuthRequest { TargetType = "USER", TargetCd = savedUserId, Items = _authItems });
            if (authResult == null || !authResult.Success)
            {
                AppMessageBox.Show(authResult?.Message ?? "권한 저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        _editingUserId = savedUserId;
        await QueryUsersAsync();
        Toast.Show(wasNew ? "사용자가 등록되었습니다." : "수정되었습니다.");
    }

    // ================================================================================
    // 탭2: 사용자그룹별 권한관리
    // ================================================================================

    private void Gvw2_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e)
    {
        if (e.Row is UserGroupListItemDto group) _pendingLoadTask = EnterEditGroupModeAsync(group);
    }

    private async Task QueryGroupsAsync()
    {
        // panHeader의 검색창(txtuser_id_q)은 이름이 "사용자ID/명"이지만, 이 탭이 조회하는 건
        // 사용자그룹 목록(grd2)이라 실제로는 그룹명 검색으로 쓴다(api/user-groups가 지원하는
        // 유일한 검색 조건).
        var keyword = txtUserGrpCdQ.Text.Trim();
        _groupsList = await ApiClient.GetAsync<List<UserGroupListItemDto>>(
            $"api/user-groups?userGrpNm={Uri.EscapeDataString(keyword)}") ?? new();

        DrawingSuspension.Suspend(this);
        try
        {
            _pendingLoadTask = null;
            grd2.DataSource = _groupsList;

            if (_editingUserGrpCd != null && _groupsList.All(g => g.UserGrpCd != _editingUserGrpCd))
                EnterNewGroupMode();

            if (_pendingLoadTask != null) await _pendingLoadTask;
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

        var confirm = AppMessageBox.Show($"'{txtuser_grp_nm.Text}({_editingUserGrpCd})' 그룹을 사용중지 처리하시겠습니까?",
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
        txtuser_grp_cd.Text = string.Empty;
        txtuser_grp_cd.ReadOnly = false;
        txtuser_grp_nm.Text = string.Empty;
        memodescription.Text = string.Empty;

        _members = new();
        grd4.DataSource = null;
        _groupAuthItems = new();
        tree2.DataSource = null;
        //tree2.Enabled = false;

        txtuser_grp_cd.Focus();
    }

    private async Task EnterEditGroupModeAsync(UserGroupListItemDto group)
    {
        _editingUserGrpCd = group.UserGrpCd;
        txtuser_grp_cd.Text = group.UserGrpCd;
        txtuser_grp_cd.ReadOnly = true; // 그룹코드는 PK라 수정 불가
        txtuser_grp_nm.Text = group.UserGrpNm;
        memodescription.Text = group.Description ?? string.Empty;

        await LoadMembersAsync(group.UserGrpCd);
        await LoadGroupAuthAsync(group.UserGrpCd);
    }

    private async Task LoadMembersAsync(string userGrpCd)
    {
        _members = await ApiClient.GetAsync<List<UserGroupMemberDto>>($"api/user-groups/{userGrpCd}/members") ?? new();
        grd4.DataSource = _members;
    }

    private async Task LoadGroupAuthAsync(string userGrpCd)
    {
        _groupAuthItems = await ApiClient.GetAsync<List<MenuAuthItemDto>>(
            $"api/menu-auth?targetType=GRP&targetCd={Uri.EscapeDataString(userGrpCd)}") ?? new();

        tree2.BeginUpdate();
        try
        {
            tree2.DataSource = _groupAuthItems;
            tree2.Enabled = true;
            tree2.ExpandAll();
        }
        finally
        {
            tree2.EndUpdate();
        }
    }

    private async Task SaveGroupAsync()
    {
        if (string.IsNullOrWhiteSpace(txtuser_grp_cd.Text) || string.IsNullOrWhiteSpace(txtuser_grp_nm.Text))
        {
            AppMessageBox.Show("그룹코드와 그룹명은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingUserGrpCd == null;
        ApiResult? result;

        if (wasNew)
        {
            var req = new UserGroupCreateRequest
            {
                UserGrpCd = txtuser_grp_cd.Text,
                UserGrpNm = txtuser_grp_nm.Text,
                Description = memodescription.Text
            };
            result = await ApiClient.PostAsync<UserGroupCreateRequest, ApiResult>("api/user-groups", req);
        }
        else
        {
            // 사용여부/정렬순서를 편집할 UI가 이 화면에 없다 - 사용여부는 항상 true로 보내고
            // (끄고 싶으면 삭제=사용중지 버튼을 쓴다), 정렬순서는 기존 값을 그대로 유지한다.
            var req = new UserGroupUpdateRequest
            {
                UserGrpNm = txtuser_grp_nm.Text,
                Description = memodescription.Text,
                SortOrder = _groupsList.FirstOrDefault(g => g.UserGrpCd == _editingUserGrpCd)?.SortOrder ?? 0,
                UseYn = true
            };
            result = await ApiClient.PutAsync<UserGroupUpdateRequest, ApiResult>($"api/user-groups/{_editingUserGrpCd}", req);
        }

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        var savedUserGrpCd = wasNew ? (result.GeneratedCode ?? txtuser_grp_cd.Text) : _editingUserGrpCd!;

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

            tree2.CloseEditor();
            tree2.PostEditor();
            var authResult = await ApiClient.PutAsync<SaveMenuAuthRequest, ApiResult>("api/menu-auth",
                new SaveMenuAuthRequest { TargetType = "GRP", TargetCd = savedUserGrpCd, Items = _groupAuthItems });
            if (authResult == null || !authResult.Success)
            {
                AppMessageBox.Show(authResult?.Message ?? "권한 저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        _editingUserGrpCd = savedUserGrpCd;
        await QueryGroupsAsync();
        Toast.Show(wasNew ? "사용자그룹이 등록되었습니다." : "수정되었습니다.");
    }
}
