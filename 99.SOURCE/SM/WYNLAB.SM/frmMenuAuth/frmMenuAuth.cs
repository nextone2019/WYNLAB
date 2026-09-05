using DevExpress.XtraGrid.Columns;
using System.Drawing;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.SM;

/// <summary>
/// 메뉴기준 권한관리 화면 - frmUserAuth(사용자/사용자그룹을 고르면 메뉴트리에 권한을 매김)의
/// 반대 축이다: 왼쪽에서 메뉴를 고르면 오른쪽 탭(사용자/사용자그룹)에 그 메뉴에 대한 전체
/// 대상의 권한을 그리드로 보여주고 체크박스로 바로 부여한다.
///
/// frmUserAuth처럼 Designer.cs 기반(VS 디자이너로 마우스 조정 가능)으로 만들었다 - 컨트롤
/// 배치/컬럼 구성은 Designer.cs, 이벤트 연결과 실제 동작(조회/저장/캡션 동기화)은 여기.
///
/// 왼쪽 메뉴트리는 frmMenu.cs가 쓰는 것과 같은 GET api/menus(List&lt;MenuListItemDto&gt;)를
/// 그대로 재사용한다 - AUTH01~10 캡션(AuthNm)도 이 응답에 이미 들어있어서 별도 호출이 필요
/// 없다. 단, 그 엔드포인트는 SM_MENU 권한으로 보호되므로 이 화면(SM_MENU_AUTH) 사용자는
/// SM_MENU 조회 권한도 같이 있어야 한다(관리자용 화면이라 실무상 문제 없음).
/// </summary>
public partial class frmMenuAuth : BaseForm
{
    private List<MenuListItemDto> _menus = new();
    private MenuListItemDto? _selectedMenu;

    private List<MenuAuthByMenuItemDto> _userItems = new();
    private List<MenuAuthByMenuItemDto> _groupItems = new();
    private bool _userLoaded;
    private bool _groupLoaded;

    private GridColumn[] AuthColsUser => new[]
    {
        colUserAuth01, colUserAuth02, colUserAuth03, colUserAuth04, colUserAuth05,
        colUserAuth06, colUserAuth07, colUserAuth08, colUserAuth09, colUserAuth10
    };

    private GridColumn[] AuthColsGroup => new[]
    {
        colGroupAuth01, colGroupAuth02, colGroupAuth03, colGroupAuth04, colGroupAuth05,
        colGroupAuth06, colGroupAuth07, colGroupAuth08, colGroupAuth09, colGroupAuth10
    };

    public frmMenuAuth()
    {
        InitializeComponent();

        Text = "메뉴별권한관리";

        // Designer.cs 전환 때 빠뜨렸던 타이틀바 - panBase(Fill) 뒤에 추가해야 맨 위를 차지한다.
        Controls.Add(BuildScreenHeader());

        menuTree.KeyFieldName = "MenuId";
        menuTree.ParentFieldName = "UpperMenuId";
        menuTree.FocusedNodeChanged += async (s, e) => await OnMenuSelectedAsync();

        lblSelectedMenu.Appearance.Font = AppFonts.BodyBold;
        lblSelectedMenu.Appearance.Options.UseFont = true;
        lblMenuHeader.Appearance.Font = AppFonts.BodyBold;
        lblMenuHeader.Appearance.Options.UseFont = true;

        btnSave.Click += async (s, e) => await SaveClick();
        panelHeaderRight.Resize += (s, e) => btnSave.Location = new Point(panelHeaderRight.Width - btnSave.Width - 16, 6);

        ConfigureTargetGrid(gvwUser);
        ConfigureTargetGrid(gvwGroup);

        tabControl.SelectedPageChanged += async (s, e) => await LoadActiveTabAsync();

        Load += async (s, e) => await QueryClick();
    }

    /// <summary>사용자 탭/사용자그룹 탭 그리드 공통 동작 - 체크박스 편집은 되지만(Role=Edit),
    /// 행 자체는 전체 대상 고정목록이라 추가/삭제 대상이 아니므로 EmbeddedNavigator 5개
    /// 버튼은 다시 전부 숨긴다(frmUserAuth의 grd3/grd4와 같은 성격).</summary>
    private static void ConfigureTargetGrid(GridViewWyn view)
    {
        view.Role = GridRoleWyn.Edit;
        view.HighlightFocusedRow = true;

        var navigator = view.GridControl.EmbeddedNavigator;
        navigator.Buttons.Append.Visible = false;
        navigator.Buttons.Remove.Visible = false;
        navigator.Buttons.Edit.Visible = false;
        navigator.Buttons.EndEdit.Visible = false;
        navigator.Buttons.CancelEdit.Visible = false;
    }

    public override async Task QueryClick()
    {
        _menus = await ApiClient.GetAsync<List<MenuListItemDto>>("api/menus") ?? new();
        menuTree.DataSource = _menus;
        menuTree.ExpandAll();

        _selectedMenu = null;
        _userLoaded = false;
        _groupLoaded = false;
        grdUser.DataSource = null;
        grdGroup.DataSource = null;
        lblSelectedMenu.Text = "메뉴를 선택하세요";
    }

    private async Task OnMenuSelectedAsync()
    {
        var menuId = menuTree.FocusedNode?.GetValue("MenuId") is long id ? id : (long?)null;
        _selectedMenu = menuId == null ? null : _menus.FirstOrDefault(m => m.MenuId == menuId);

        lblSelectedMenu.Text = _selectedMenu != null ? $"[{_selectedMenu.MenuId}] {_selectedMenu.MenuNm}" : "메뉴를 선택하세요";

        SyncAuthCaptions(AuthColsUser, _selectedMenu?.AuthNm);
        SyncAuthCaptions(AuthColsGroup, _selectedMenu?.AuthNm);

        // 메뉴등록(frmMenu)에서 그룹(GROUP) 노드를 선택했을 때 - frmUserAuth.ConfigureAuthTree의
        // ShowingEditor 규칙(그룹 노드는 조회만 편집 가능)과 동일하게, 조회 컬럼만 편집 가능하게
        // 남기고 나머지는 잠근다(그룹 메뉴는 하위 전체 노출여부 판단용이라 세부 권한이 의미 없음).
        var isGroupMenu = _selectedMenu?.MenuType == "GROUP";
        ApplyGroupRestriction(gvwUser, isGroupMenu);
        ApplyGroupRestriction(gvwGroup, isGroupMenu);

        _userLoaded = false;
        _groupLoaded = false;
        await LoadActiveTabAsync();
    }

    private static void SyncAuthCaptions(GridColumn[] cols, string?[]? authNm)
    {
        for (var i = 0; i < cols.Length; i++)
            cols[i].Caption = authNm != null && !string.IsNullOrWhiteSpace(authNm[i]) ? authNm[i] : $"Auth{(i + 1):00}";
    }

    private static void ApplyGroupRestriction(GridViewWyn view, bool isGroupMenu)
    {
        foreach (GridColumn col in view.Columns)
        {
            if (col.FieldName is "TargetCd" or "TargetNm" or "SubNm" or "ViewYn") continue;
            col.OptionsColumn.AllowEdit = !isGroupMenu;
        }
    }

    /// <summary>현재 보이는 탭만 불러온다 - 메뉴를 바꾸면(OnMenuSelectedAsync) 캐시가 리셋되지만,
    /// 같은 메뉴 안에서 탭만 오가는 동안은 이미 불러온 그리드 값(저장 전 수정분 포함)을 그대로
    /// 유지한다.</summary>
    private async Task LoadActiveTabAsync()
    {
        if (_selectedMenu == null)
        {
            grdUser.DataSource = null;
            grdGroup.DataSource = null;
            return;
        }

        if (tabControl.SelectedTabPage == tabUser)
        {
            if (!_userLoaded)
            {
                _userItems = await ApiClient.GetAsync<List<MenuAuthByMenuItemDto>>(
                    $"api/menu-auth/by-menu?menuId={_selectedMenu.MenuId}&targetType=USER") ?? new();
                _userLoaded = true;
            }
            grdUser.DataSource = _userItems;
        }
        else
        {
            if (!_groupLoaded)
            {
                _groupItems = await ApiClient.GetAsync<List<MenuAuthByMenuItemDto>>(
                    $"api/menu-auth/by-menu?menuId={_selectedMenu.MenuId}&targetType=GRP") ?? new();
                _groupLoaded = true;
            }
            grdGroup.DataSource = _groupItems;
        }
    }

    public override async Task SaveClick()
    {
        if (_selectedMenu == null)
        {
            AppMessageBox.Show("메뉴를 먼저 선택하세요.", "저장 실패");
            return;
        }

        var isUserTab = tabControl.SelectedTabPage == tabUser;
        var view = isUserTab ? gvwUser : gvwGroup;
        view.CloseEditor();
        view.UpdateCurrentRow();

        var items = isUserTab ? _userItems : _groupItems;
        var result = await ApiClient.PutAsync<SaveMenuAuthByMenuRequest, ApiResult>("api/menu-auth/by-menu",
            new SaveMenuAuthByMenuRequest { MenuId = _selectedMenu.MenuId, TargetType = isUserTab ? "USER" : "GRP", Items = items });

        if (result == null || !result.Success)
        {
            AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
            return;
        }

        Toast.Show("저장되었습니다.");
        if (isUserTab) _userLoaded = false; else _groupLoaded = false;
        await LoadActiveTabAsync();
    }
}
