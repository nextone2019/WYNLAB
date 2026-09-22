using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Columns;
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
/// 그대로 재사용한다 - 서버는 전혀 안 건드렸고, 권한 체크도 전부 Module="SM"/ScreenClassNm="frmUserAuth"
/// 기준이라 그 값을 그대로 쓴다.
/// </summary>
public partial class frmUserAuth : BaseForm
{
    // ===== 탭1: 사용자별 권한관리 =====
    private List<UserListItemDto> _users = new();
    private string? _editingUserId; // null이면 신규모드

    private List<UserGroupAssignDto> _groups = new(); // grd3 - 전체 그룹 + 이 사용자의 소속여부(체크)
    private List<MenuAuthItemDto> _authItems = new(); // tree1 - 화면표시는 본인권한 OR 소속그룹권한(합산값)

    // tree1 저장 시 합산표시값을 그대로 보내면 그룹이 준 권한이 이 사용자 개인 권한으로 굳어버린다
    // (LoadAuthAsync/BuildAuthSaveItem 참고) - 그래서 로드 시점의 "본인 전용값"과 "표시된 합산값"을
    // MenuId 기준으로 따로 스냅샷해뒀다가, 저장 시 칸별로 지금 값이 로드시점 합산값과 같으면(=관리자가
    // 안 건드림) 본인 전용값을 그대로 돌려보내고, 다르면(=관리자가 실제로 체크/해제함) 지금 값을
    // 그대로 본인권한으로 저장한다.
    private Dictionary<long, MenuAuthItemDto> _authOwnSnapshot = new();
    private Dictionary<long, MenuAuthItemDto> _authUnionSnapshot = new();

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

    // AUTH01~10 - tree1/tree2에 조회/입력/저장/삭제와 같은 방식의 체크 컬럼으로 추가한다.
    // 컬럼 자체는 항상 10개 고정으로 떠 있고, 캡션 텍스트만 트리에서 포커스된 메뉴의
    // MenuAuthItemDto.AuthNm(메뉴등록/frmMenu에서 정의)으로 동적으로 갈아끼운다.
    private readonly TreeListColumn[] _authCols1 = new TreeListColumn[10];
    private readonly TreeListColumn[] _authCols2 = new TreeListColumn[10];

    public frmUserAuth()
    {
        InitializeComponent();

        Text = "사용자권한관리";

        gvw1.FocusedRowObjectChanged += Gvw1_FocusedRowObjectChanged;
        gvw2.FocusedRowObjectChanged += Gvw2_FocusedRowObjectChanged;

        // grd1(사용자 목록)/grd2(사용자그룹 목록) - 조회/선택 전용 그리드라 Role=Query(기본값이지만
        // 명시)로 편집을 막는다. GridViewWynBehavior가 OptionsClipboard.AllowCopy는 Role과
        // 무관하게 항상 켜두므로 편집만 막히고 셀 복사(Ctrl+C)는 그대로 된다(2026-09-04, "grd1은
        // 조회 전용이야. Enable을 막아줘. 대신에 컬럼별로 데이터를 복사는 할수 있도록 해줘" -
        // 모든 화면의 조회 그리드에 공통 적용되어야 하는 규칙이라 프로퍼티 하나로 해결되는
        // GridViewWyn으로 옮겼다). HighlightFocusedRow도 이제 그 프로퍼티로 대신한다.
        gvw1.Role = GridRoleWyn.Query;
        gvw1.HighlightFocusedRow = true;
        gvw2.Role = GridRoleWyn.Query;
        gvw2.HighlightFocusedRow = true;

        // grd3(소속그룹 체크)/grd4(소속사원 체크)는 실제로 편집(체크토글)이 되는 그리드라 순정
        // GridView로 남겨둔다 - 포커스행 강조만 기존 방식대로 직접 붙인다.
        gvw3.RowCellStyle += HighlightFocusedRow;
        gvw4.RowCellStyle += HighlightFocusedRow;

        ConfigureAuthTree(tree1, _authCols1);
        ConfigureAuthTree(tree2, _authCols2);
        tree1.FocusedNodeChanged += (s, e) => SyncAuthColumns(tree1, _authItems, _authCols1);
        tree2.FocusedNodeChanged += (s, e) => SyncAuthColumns(tree2, _groupAuthItems, _authCols2);

        ConfigureEmpPopup();

        // grd1(사용자 목록)의 "사용"(gridColumn11, FieldName=UseYn) 컬럼 - LookUpColumnEdit
        // (Designer.cs의 lookUpColumnEdit3, LookupKey="L_CM0100")로 Designer에서 직접 연결했다.
        // 코드에서 따로 셋업할 필요 없음(2026-09-04).

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. txtEmpNm은 PopupLookupEditWyn이라
        // "Popup : P_EMP"도 자동으로 같이 붙는다.
        //
        // grd1~4는 DataTable이 아니라 List<T>(UserListItemDto 등)에 바인딩돼서 GridColumn.FieldName이
        // 이미 C# 프로퍼티명(PascalCase, 예: UserNm)이다 - 다른 화면(DataTable 바인딩이라 FieldName이
        // 곧 DB 컬럼명)과 달리 여기는 PascalCase가 "진짜 바인딩 값"이다. 그래서 panData 쪽도 DB
        // 컬럼명(USER_NM) 대신 같은 PascalCase(UserNm)로 맞춘다(2026-09-03, 사장님 지시 - "UserNm이
        // 맞지 않아? 통일시켜줘"). 그리드 컬럼은 FieldName이 이미 PascalCase라 별도 Tag가 필요 없다.
        txtuser_id.Tag = new BindingFieldTag("UserId");
        txtuser_nm.Tag = new BindingFieldTag("UserNm");
        cboUseYn.Tag = new BindingFieldTag("UseYn");
        chkDeveloperYn.Tag = new BindingFieldTag("DeveloperYn");
        txtEmpNm.Tag = new BindingFieldTag("EmpNm");
        txtEmpNo.Tag = new BindingFieldTag("EmpId");
        txtDeptNm.Tag = new BindingFieldTag("DeptNm");
        cboAccCd.Tag = new BindingFieldTag("AccId");
        txtEmail.Tag = new BindingFieldTag("Email");
        txtuser_grp_cd.Tag = new BindingFieldTag("UserGrpCd");
        txtuser_grp_nm.Tag = new BindingFieldTag("UserGrpNm");
        memodescription.Tag = new BindingFieldTag("Description");

        // 탭이 바뀔 때마다 반대편 탭에서 방금 수정했을 수도 있는 데이터를 다시 불러온다 -
        // 그리드 포커스행이 그대로면 FocusedRowObjectChanged가 재발생하지 않아 저장 전 상태로
        // 남기 때문(BACK_frmUserManage.OnOuterTabChangedAsync와 같은 이유).
        tabControlWyn1.SelectedPageChanged += async (s, e) => await OnOuterTabChangedAsync();

        SearchOnEnter(txtUserGrpIdQ);
        SearchOnEnter(txtUserGrpCdQ);

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync) - 탭1 상세는 panelWyn16, 탭2 상세는
        // panData에 담겨 있다(TEMPLATE 복사본이 아니라 디자이너로 직접 배치한 화면이라 이름이
        // 다르다). grd3/grd4(소속그룹/소속사원 체크그리드)와 tree1/tree2(메뉴권한 트리)는
        // List<T> 바인딩이라 TrackDirty(DataTable)이 적용되지 않는다 - 이 두 그리드/트리에서만
        // 바꾼 뒤 닫으면 아직 저장 확인이 뜨지 않는다(알려진 범위 밖, 필요해지면 별도 처리).
        TrackDirty(panelWyn16);
        TrackDirty(panData);

        EnterNewMode();
        EnterNewGroupMode();
        // 오픈 시 자동 조회하지 않는다 - 검색창에 조건을 넣고 조회 버튼(또는 Ctrl+Q)을 눌러야 뜬다.
    }

    /// <summary>gvw1~4 공통 - 포커스된 행 전체를 배경색으로 강조한다(GridViewWynBehavior.
    /// OnRowCellStyle과 같은 로직, 그리드 4개가 전부 이 하나의 핸들러를 공유하므로 sender에서
    /// 실제 뷰를 가져온다).</summary>
    private static void HighlightFocusedRow(object? sender, RowCellStyleEventArgs e)
    {
        if (sender is not GridView view || e.RowHandle != view.FocusedRowHandle) return;
        e.Appearance.BackColor = UiTheme.GridFocusedRowBackColor;
        e.Appearance.Options.UseBackColor = true;
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

    /// <summary>컬럼 대부분(메뉴명/조회/입력/저장/삭제)은 디자이너(tree1/tree2 각각의 Columns)가
    /// 정의한다 - 여기서는 그 컬럼들을 그대로 쓰기 위한 최소한의 데이터 바인딩(트리 구조), GROUP
    /// (상위분류) 행은 조회 체크만 의미가 있어 나머지 컬럼은 편집을 막는 규칙(BACK_frmUserManage.
    /// BuildAuthTab과 같은 규칙), 그리고 AUTH01~10 체크 컬럼 10개를 준비한다 - 캡션이 메뉴마다
    /// 달라서(메뉴등록/frmMenu에서 정의) 디자이너에 고정 텍스트로 박아둘 수 없어 SyncAuthColumns가
    /// 매번 갈아끼운다.
    /// FieldName(Auth01~10)으로 디자이너에 이미 그 컬럼이 있으면(2026-09-06부터 tree1은 직접
    /// 그려서 관리) 그 컬럼을 그대로 쓰고, 없으면(아직 안 그린 tree2 등) 예전처럼 코드가 만들어
    /// 채운다 - 디자이너에 그려둔 걸 코드가 또 만들어 중복시키는 사고를 막는다(실제로 겪음).</summary>
    private void ConfigureAuthTree(TreeListWyn tree, TreeListColumn[] authCols)
    {
        tree.KeyFieldName = "MenuId";
        tree.ParentFieldName = "UpperMenuId";
        tree.OptionsBehavior.Editable = true;

        var colMenuNm = tree.Columns["MenuNm"];
        var colView = tree.Columns["ViewYn"];

        var nextVisibleIndex = tree.Columns.Cast<TreeListColumn>()
            .Where(c => c.Visible).Select(c => c.VisibleIndex).DefaultIfEmpty(-1).Max() + 1;
        for (var i = 0; i < 10; i++)
        {
            var fieldName = $"Auth{i + 1:00}";
            var col = tree.Columns[fieldName];
            if (col == null)
            {
                var editor = new RepositoryItemCheckEdit { AutoHeight = false };
                tree.RepositoryItems.Add(editor);
                col = new TreeListColumn
                {
                    FieldName = fieldName,
                    Caption = fieldName,
                    Name = $"colAuth{i + 1:00}_{tree.Name}",
                    ColumnEdit = editor,
                    Visible = true,
                    VisibleIndex = nextVisibleIndex + i,
                    Width = 55
                };
                tree.Columns.Add(col);
            }
            authCols[i] = col;
        }

        tree.ShowingEditor += (s, e) =>
        {
            var node = tree.FocusedNode;
            if (node == null || tree.FocusedColumn == colMenuNm) return;
            var menuType = (string)node.GetValue("MenuType");
            if (menuType == "GROUP" && tree.FocusedColumn != colView) e.Cancel = true;
        };
    }

    /// <summary>트리에서 포커스된 노드(메뉴)를 찾아 그 메뉴의 AuthNm 캡션으로 AUTH01~10 컬럼의
    /// Caption을 갈아끼운다. 포커스된 노드가 없거나(트리가 비어있음) 목록에서 못 찾으면 전부
    /// 기본 캡션("Auth01" 등)으로 되돌린다.</summary>
    private static void SyncAuthColumns(TreeListWyn tree, List<MenuAuthItemDto> items, TreeListColumn[] authCols)
    {
        var menuId = tree.FocusedNode?.GetValue("MenuId") is long id ? id : (long?)null;
        var item = menuId != null ? items.FirstOrDefault(i => i.MenuId == menuId) : null;
        for (var i = 0; i < 10; i++)
        {
            var caption = item != null && i < item.AuthNm.Length ? item.AuthNm[i] : null;
            authCols[i].Caption = string.IsNullOrWhiteSpace(caption) ? $"Auth{i + 1:00}" : caption;
        }
    }

    /// <summary>
    /// txtEmpNm(사원명 검색창, 팝업/룩업 개발가이드의 "멀티필드 모드" - 00.DEV/POPUP_FRAMEWORK_GUIDE.md
    /// 참고)이 팝업(P_EMP)에서 고른 사원 1건의 값을 4개 컨트롤(사원번호/사원명/부서코드/부서명)에
    /// 각각 나눠서 채워 넣도록 매핑을 등록한다.
    ///
    /// 동작 원리(PopupLookupEditWyn):
    ///  1) LookupKey = "P_EMP"  -> 어느 팝업(sysPopUpM.popup_key)을 열지 지정. 실제 조회는
    ///     이 팝업에 등록된 프로시저 SSP_POP_EMP_Q가 담당하며, 그 프로시저는
    ///     EMP_ID/emp_no/emp_nm/emp_nm_eng/dept_nm/... 컬럼을 그대로(별칭 없이) 반환한다.
    ///     popPopUp이 그 결과를 팝업 그리드에 그대로 보여주고, 사용자가 한 행을 고르면
    ///     그 행의 "모든" 컬럼값이 PopupLookupResult.Row(Dictionary&lt;string,string?&gt;)에 담겨
    ///     이 컨트롤로 넘어온다 - 키는 SQL이 실제로 반환한 컬럼명 그대로(EMP_ID, emp_no 등).
    ///  2) MatchField = "emp_nm" (Designer.cs에 이미 지정됨) -> txtEmpNm 자기 자신이 Row의
    ///     어느 컬럼을 대표하는지 지정. 이게 있어야 Leave(포커스 아웃) 시 방금 타이핑한 텍스트로
    ///     자동 검색(정확히 1건이면 조용히 채움, 아니면 그 값을 미리 채운 팝업을 띄움)이 동작한다.
    ///  3) MapField(resultColumn, targetControl) -> Row의 나머지 컬럼들을 어느 컨트롤에 채울지
    ///     등록. 아래 3줄이 실제 매핑이다 - "..." 버튼을 눌러 팝업에서 고르든, 텍스트를 직접 치고
    ///     포커스를 벗어나든(자동조회) 상관없이 ApplyResult() 한 곳에서 전부 이 매핑을 사용해
    ///     채워 넣으므로, 아래 등록만으로 두 경로 모두 자동 적용된다.
    ///  4) txtEmpNm을 지우면(EditValueChanged, 빈 값) 여기 매핑된 컨트롤도 전부 같이 지워진다
    ///     (ClearMappedFields) - 별도 처리 불필요.
    ///
    /// txtEmpNo는 화면에는 안 보이는 숨김 필드(Designer.cs에 Visible=false로 배치됨) - 이름은
    /// 예전 그대로지만 이제 EMP_ID를 담는다(TSMUSER.EMP_ID 저장용, 2026-09-06 emp_no -> EMP_ID
    /// 전환). 화면에 굳이 노출할 필요는 없다는 뜻으로 숨김은 그대로 둔다.
    /// </summary>
    private void ConfigureEmpPopup()
    {
        txtEmpNm.MapField("EMP_ID", txtEmpNo);
        txtEmpNm.MapField("dept_nm", txtDeptNm);
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

    /// <summary>사용자가 툴바에서 직접 누른 조회 - 새 검색이므로 이전 선택은 무시하고
    /// 결과 1행부터 보여준다. 저장/삭제 뒤의 재조회는 QueryUsersAsync/QueryGroupsAsync를
    /// preserveSelection: true로 직접 호출해서 방금 편집하던 행을 유지한다
    /// (feedback_query_refocus_after_save 메모리 참고).</summary>
    public override async Task QueryClick()
    {
        if (tabControlWyn1.SelectedTabPage == xtraTabPage2) await QueryGroupsAsync(preserveSelection: false);
        else await QueryUsersAsync(preserveSelection: false);
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

    /// <summary>다른 사용자를 고를 때 panData/panelWyn16에 저장 안 된 변경이 있으면 먼저 확인한다
    /// (BaseForm.ConfirmMasterRowSwitch 제네릭 버전 - 이 그리드는 DataTable이 아니라
    /// List&lt;UserListItemDto&gt;에 바인딩돼서 e.Row가 DataRowView가 아니라 DTO 자신이다.
    /// 2026-09-06 - 모든 화면 공통 적용).</summary>
    private void Gvw1_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw1, e, (UserListItemDto user) => _pendingLoadTask = EnterEditModeAsync(user));

    private async Task QueryUsersAsync(bool preserveSelection)
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

            // 저장 직후엔 아직 IsDirty가 true로 남아있을 수 있어서(AcceptChanges류가 RowChanged를
            // 안 냄) 이 재바인딩이 ConfirmMasterRowSwitch의 확인을 또 띄우면 안 된다 - 그래서
            // grd1 재바인딩/재포커스 구간 전체를 SuppressMasterRowSwitchConfirm으로 감싼다.
            SuppressMasterRowSwitchConfirm(gvw1, () =>
            {
                grd1.DataSource = _users;

                // 저장 직후 재조회(preserveSelection: true)에서만 방금 편집하던 사용자에게 포커스를
                // 되돌린다 - 안 그러면 DevExpress가 조용히 0번 행에 포커스를 줘서(FocusedRowObjectChanged
                // 없이) 방금 저장한 사용자와 다른 사람의 상세가 뜨거나 패널이 엉뚱한 값으로 남는다
                // (feedback_query_refocus_after_save 메모리 참고). 사용자가 직접 누른 조회
                // (preserveSelection: false)는 새 검색이므로 이전 선택을 무시하고 1행부터 보여준다.
                if (preserveSelection && _editingUserId != null)
                {
                    var handle = FindUserRowHandle(_editingUserId);
                    if (handle != null) gvw1.FocusedRowHandle = handle.Value;
                }
            });

            if (_pendingLoadTask != null) await _pendingLoadTask;
            HideBusy();
        }
        finally
        {
            DrawingSuspension.Resume(this);
        }

        // QueryGroupsAsync와 같은 이유(그 쪽 주석 참고, 2026-09-11) - DevExpress가 grd1.DataSource
        // 재할당 직후 위에서 맞춰둔 FocusedRowHandle을 자기 내부 "첫 행 자동포커스" 로직으로
        // 이벤트 없이 조용히 덮어쓸 수 있어서, BeginInvoke로 한 틱 미뤄 다시 한번 확정한다.
        if (preserveSelection && _editingUserId != null)
        {
            var targetUserId = _editingUserId;
            BeginInvoke(new Action(() =>
            {
                var handle = FindUserRowHandle(targetUserId);
                if (handle != null) gvw1.FocusedRowHandle = handle.Value;
            }));
        }
    }

    /// <summary>userId로 grd1의 행 핸들을 찾는다. 없으면 null.
    ///
    /// handle을 0부터 RowCount까지 돌며 GetRow(handle)로 값을 비교하는 방식(예전에 이 자리에
    /// 있었던 방식)은 List&lt;T&gt; 바인딩에서도 재조회 직후 handle과 실제 리스트 인덱스가
    /// 아직 안 맞아떨어지는 경우가 있어 못 찾을 때가 있었다(2026-09-11 실제 발견 - 사용자그룹
    /// 탭에서 저장 후 포커스가 늘 1번째 행으로 감). FindRowHandle 컨벤션(DataTable 화면들의
    /// _majors.Rows.IndexOf + GetRowHandle과 동일한 원리)대로, 데이터소스(List&lt;T&gt;) 안에서
    /// 먼저 인덱스를 찾고 그 인덱스를 GridView.GetRowHandle로 변환하면 항상 정확하다.</summary>
    private int? FindUserRowHandle(string userId)
    {
        var index = _users.FindIndex(u => string.Equals(u.UserId, userId, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return null;
        var handle = gvw1.GetRowHandle(index);
        return handle >= 0 ? handle : null;
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
        await QueryUsersAsync(preserveSelection: false);
        EnterNewMode();
        Toast.Show("사용중지 처리되었습니다.");
    }

    /// <summary>panelWyn16(탭1 상세)를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안
    /// 그러면 코드가 값을 채우는 것뿐인데 TrackDirty가 "사용자가 고쳤다"로 오인해서, 조회/행
    /// 선택 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingUserId = null;
            txtuser_id.Text = string.Empty;
            txtuser_id.ReadOnly = false;
            txtuser_nm.Text = string.Empty;
            cboUseYn.EditValue = "Y";
            cboUserType.EditValue = "U"; // 신규는 일반 사용자로 시작 - 관리자로 바꾸려면 저장 후 수정
            //checkBoxWyn1.Enabled = false; // 신규는 서버에서 항상 'Y'로 생성됨(BACK_frmUserManage와 같은 규칙)
            chkDeveloperYn.Checked = false; // 신규는 서버에서 항상 'N'으로 생성됨 - 이 화면으로는 못 바꿈
            txtEmpNm.Text = string.Empty;
            txtEmpNo.Text = string.Empty;
            txtDeptNm.Text = string.Empty;
            cboAccCd.EditValue = string.Empty;
            txtEmail.Text = string.Empty;
        });

        _groups = new();
        grd3.DataSource = null;
        _authItems = new();
        _authOwnSnapshot = new();
        _authUnionSnapshot = new();
        tree1.DataSource = null;
        SyncAuthColumns(tree1, _authItems, _authCols1);
        //tree1.Enabled = false;

        txtuser_id.Focus();
    }

    private async Task EnterEditModeAsync(UserListItemDto user)
    {
        SuppressDirtyTracking(() =>
        {
            _editingUserId = user.UserId;
            txtuser_id.Text = user.UserId;
            txtuser_id.ReadOnly = true; // 아이디는 PK라 수정 불가
            txtuser_nm.Text = user.UserNm;
            cboUseYn.EditValue = user.UseYn ? "Y" : "N"; // user.UseYn은 bool, cboUseYn(L_CM0100) 값필드는 "Y"/"N" 문자열
            // cboUserType은 예전엔 여기서 안 채워서(2026-09-10 실제 발견) 목록에서 다른 사용자를
            // 클릭해도 값이 안 바뀌고 마지막에 고른 값이 그대로 남아있었다 - "저장하면 전체
            // 사용자가 다 바뀌는 것 같다"는 착시의 원인(실제로는 SaveUserAsync가 이 필드를 저장
            // 요청에 담지도 않아서 진짜로 바뀐 적은 없었음, 그것도 같이 고침).
            cboUserType.EditValue = user.UserType ?? "U";
            //checkBoxWyn1.Enabled = true;
            chkDeveloperYn.Checked = user.DeveloperYn; // 조회 전용 표시 - Enabled=false라 여기서 못 바꿈
            txtEmpNm.Text = user.EmpNm ?? string.Empty;
            txtEmpNo.Text = user.EmpId?.ToString() ?? string.Empty;
            txtDeptNm.Text = user.DeptNm ?? string.Empty;
            cboAccCd.EditValue = user.AccId?.ToString() ?? string.Empty;
            txtEmail.Text = user.Email ?? string.Empty;
        });

        await LoadGroupsAsync(user.UserId);
        await LoadAuthAsync(user.UserId);
    }

    private async Task LoadGroupsAsync(string userId)
    {
        _groups = await ApiClient.GetAsync<List<UserGroupAssignDto>>($"api/users/{userId}/groups") ?? new();
        grd3.DataSource = _groups;
    }

    /// <summary>tree1에 본인권한만 보여주면, 소속그룹으로 받은 권한이 있어도 체크가 안 된 것처럼
    /// 보여 혼란을 준다(그룹 PROD_STF에 출하요청등록 권한을 줬는데, 그 그룹 소속 사용자를 여기서
    /// 보면 체크가 안 되어 있던 실제 버그 - 2026-09-02 보고). 그래서 본인권한(GetAuth)에 소속그룹
    /// 전체가 합산된 "실제 효과"(로그인시와 동일한 MenuPermissionMerger 로직, api/menu-auth/effective)
    /// 를 OR로 얹어서 표시한다. 저장 시 이 합산값을 그대로 보내면 안 되므로(위 필드 주석 참고)
    /// 로드 시점 스냅샷을 같이 남긴다.</summary>
    private async Task LoadAuthAsync(string userId)
    {
        var ownItems = FilterOutDeveloperTool(await ApiClient.GetAsync<List<MenuAuthItemDto>>(
            $"api/menu-auth?targetType=USER&targetCd={Uri.EscapeDataString(userId)}") ?? new());
        var effective = await ApiClient.GetAsync<List<MenuDto>>(
            $"api/menu-auth/effective?userId={Uri.EscapeDataString(userId)}") ?? new();
        var effectiveByMenuId = effective.ToDictionary(m => m.MenuId);

        _authOwnSnapshot = ownItems.ToDictionary(i => i.MenuId, CloneAuthValues);

        foreach (var item in ownItems)
        {
            if (!effectiveByMenuId.TryGetValue(item.MenuId, out var eff)) continue;
            item.ViewYn |= eff.ViewYn;
            item.InsertYn |= eff.InsertYn;
            item.UpdateYn |= eff.UpdateYn;
            item.DeleteYn |= eff.DeleteYn;
            item.PrintYn |= eff.PrintYn;
            item.ExcelYn |= eff.ExcelYn;
            item.Auth01 |= eff.Auth[0];
            item.Auth02 |= eff.Auth[1];
            item.Auth03 |= eff.Auth[2];
            item.Auth04 |= eff.Auth[3];
            item.Auth05 |= eff.Auth[4];
            item.Auth06 |= eff.Auth[5];
            item.Auth07 |= eff.Auth[6];
            item.Auth08 |= eff.Auth[7];
            item.Auth09 |= eff.Auth[8];
            item.Auth10 |= eff.Auth[9];
        }

        _authItems = ownItems;
        _authUnionSnapshot = ownItems.ToDictionary(i => i.MenuId, CloneAuthValues);

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
        SyncAuthColumns(tree1, _authItems, _authCols1);
    }

    /// <summary>"Developer Tool" 최상위 그룹과 그 하위 전체(AI Builder/Component관리 등)를
    /// tree1(사용자별 권한관리)/tree2(사용자그룹별 권한관리)에서 뺀다 - ShellForm의 사이드바
    /// 숨김(GetDeveloperOnlyMenuIds)과 같은 이유(2026-09-06 요청, "사용자별권한등록에서도
    /// 마찬가지로"). 이 화면을 쓰는 관리자 자신이 개발자 계정이면 그대로 다 보여준다.</summary>
    private static List<MenuAuthItemDto> FilterOutDeveloperTool(List<MenuAuthItemDto> items)
    {
        if (Session.IsDeveloper) return items;

        var root = items.FirstOrDefault(m => m.UpperMenuId == null && m.MenuNm == "Developer Tool");
        if (root == null) return items;

        var hidden = new HashSet<long> { root.MenuId };
        var queue = new Queue<long>();
        queue.Enqueue(root.MenuId);
        while (queue.Count > 0)
        {
            var id = queue.Dequeue();
            foreach (var child in items.Where(m => m.UpperMenuId == id))
            {
                if (hidden.Add(child.MenuId)) queue.Enqueue(child.MenuId);
            }
        }

        return items.Where(m => !hidden.Contains(m.MenuId)).ToList();
    }

    private static MenuAuthItemDto CloneAuthValues(MenuAuthItemDto src) => new()
    {
        MenuId = src.MenuId,
        ViewYn = src.ViewYn, InsertYn = src.InsertYn, UpdateYn = src.UpdateYn,
        DeleteYn = src.DeleteYn, PrintYn = src.PrintYn, ExcelYn = src.ExcelYn,
        Auth01 = src.Auth01, Auth02 = src.Auth02, Auth03 = src.Auth03, Auth04 = src.Auth04, Auth05 = src.Auth05,
        Auth06 = src.Auth06, Auth07 = src.Auth07, Auth08 = src.Auth08, Auth09 = src.Auth09, Auth10 = src.Auth10,
    };

    /// <summary>tree1이 바인딩하는 _authItems는 표시용 합산값(본인 OR 그룹)이라 그대로 저장하면
    /// 그룹이 준 권한까지 이 사용자 개인 권한으로 굳어버린다(나중에 그룹에서 빠지거나 그룹 권한이
    /// 바뀌어도 이 사용자만 그대로 남는 문제). 칸별로 로드 시점 합산값과 지금 값을 비교해서 안
    /// 바뀌었으면(관리자가 안 건드림) 본인 전용값을 그대로 돌려보내고, 바뀌었으면(관리자가 실제로
    /// 체크/해제함) 지금 값을 그대로 본인권한으로 저장한다.</summary>
    private MenuAuthItemDto BuildAuthSaveItem(MenuAuthItemDto current)
    {
        var own = _authOwnSnapshot.TryGetValue(current.MenuId, out var ownVal) ? ownVal : new MenuAuthItemDto { MenuId = current.MenuId };
        var loadUnion = _authUnionSnapshot.TryGetValue(current.MenuId, out var unionVal) ? unionVal : own;

        static bool Resolve(bool cur, bool load, bool ownVal) => cur == load ? ownVal : cur;

        return new MenuAuthItemDto
        {
            MenuId = current.MenuId,
            ViewYn = Resolve(current.ViewYn, loadUnion.ViewYn, own.ViewYn),
            InsertYn = Resolve(current.InsertYn, loadUnion.InsertYn, own.InsertYn),
            UpdateYn = Resolve(current.UpdateYn, loadUnion.UpdateYn, own.UpdateYn),
            DeleteYn = Resolve(current.DeleteYn, loadUnion.DeleteYn, own.DeleteYn),
            PrintYn = Resolve(current.PrintYn, loadUnion.PrintYn, own.PrintYn),
            ExcelYn = Resolve(current.ExcelYn, loadUnion.ExcelYn, own.ExcelYn),
            Auth01 = Resolve(current.Auth01, loadUnion.Auth01, own.Auth01),
            Auth02 = Resolve(current.Auth02, loadUnion.Auth02, own.Auth02),
            Auth03 = Resolve(current.Auth03, loadUnion.Auth03, own.Auth03),
            Auth04 = Resolve(current.Auth04, loadUnion.Auth04, own.Auth04),
            Auth05 = Resolve(current.Auth05, loadUnion.Auth05, own.Auth05),
            Auth06 = Resolve(current.Auth06, loadUnion.Auth06, own.Auth06),
            Auth07 = Resolve(current.Auth07, loadUnion.Auth07, own.Auth07),
            Auth08 = Resolve(current.Auth08, loadUnion.Auth08, own.Auth08),
            Auth09 = Resolve(current.Auth09, loadUnion.Auth09, own.Auth09),
            Auth10 = Resolve(current.Auth10, loadUnion.Auth10, own.Auth10),
        };
    }

    private async Task SaveUserAsync()
    {
        if (string.IsNullOrWhiteSpace(txtuser_id.Text) || string.IsNullOrWhiteSpace(txtuser_nm.Text))
        {
            AppMessageBox.Show("아이디와 이름은 필수입니다.", "확인");
            return;
        }

        var wasNew = _editingUserId == null;
        var empId = long.TryParse(txtEmpNo.Text, out var eid) ? eid : (long?)null;
        var accId = long.TryParse(cboAccCd.EditValue?.ToString(), out var aid) ? aid : (long?)null;
        var email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text;
        ApiResult? result;

        if (wasNew)
        {
            // 이 화면엔 비밀번호 입력 UI가 없다 - 초기 비밀번호는 서버가 TSMSITECONFIG.
            // init_pwd_policy(사이트환경설정 > 비밀번호정책)를 보고 정한다(아이디와 동일 또는
            // 랜덤). 랜덤 정책일 때는 저장 성공 후 결과 메시지로 알려줘야 관리자가 실제로
            // 그 계정을 전달할 수 있다(2026-09-06 연동).
            var req = new UserCreateRequest
            {
                UserId = txtuser_id.Text,
                UserNm = txtuser_nm.Text,
                EmpId = empId,
                AccId = accId,
                UserType = cboUserType.EditValue?.ToString(),
                Email = email
            };
            result = await ApiClient.PostAsync<UserCreateRequest, ApiResult>("api/users", req);

            if (result?.Success == true && !string.IsNullOrEmpty(result.InitialPassword))
            {
                AppMessageBox.Show($"계정이 생성되었습니다.\n초기 비밀번호: {result.InitialPassword}", "안내");
            }
        }
        else
        {
            var req = new UserUpdateRequest
            {
                UserNm = txtuser_nm.Text,
                UseYn = cboUseYn.EditValue?.ToString() == "Y", // cboUseYn(L_CM0100)은 값필드가 "Y"/"N" 문자열이고
                                                                // DTO.UseYn은 bool이라 여기서 변환한다.
                EmpId = empId,
                AccId = accId,
                UserType = cboUserType.EditValue?.ToString(),
                Email = email
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
            var authItemsToSave = _authItems.Select(BuildAuthSaveItem).ToList();
            var authResult = await ApiClient.PutAsync<SaveMenuAuthRequest, ApiResult>("api/menu-auth",
                new SaveMenuAuthRequest { TargetType = "USER", TargetCd = savedUserId, Items = authItemsToSave });
            if (authResult == null || !authResult.Success)
            {
                AppMessageBox.Show(authResult?.Message ?? "권한 저장에 실패했습니다.", "저장 실패");
                return;
            }
        }

        _editingUserId = savedUserId;
        await QueryUsersAsync(preserveSelection: true);
        Toast.Show(wasNew ? "사용자가 등록되었습니다." : "수정되었습니다.");
    }

    // ================================================================================
    // 탭2: 사용자그룹별 권한관리
    // ================================================================================

    /// <summary>다른 그룹을 고를 때 저장 안 된 변경이 있으면 먼저 확인한다(BaseForm.
    /// ConfirmMasterRowSwitch 제네릭 버전 - List&lt;UserGroupListItemDto&gt; 바인딩, 2026-09-06).</summary>
    private void Gvw2_FocusedRowObjectChanged(object? sender, FocusedRowObjectChangedEventArgs e) =>
        ConfirmMasterRowSwitch(gvw2, e, (UserGroupListItemDto group) => _pendingLoadTask = EnterEditGroupModeAsync(group));

    private async Task QueryGroupsAsync(bool preserveSelection)
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

            // 저장 직후엔 아직 IsDirty가 true로 남아있을 수 있어서 이 재바인딩이
            // ConfirmMasterRowSwitch의 확인을 또 띄우면 안 된다(gvw1/QueryUsersAsync와 같은 이유).
            SuppressMasterRowSwitchConfirm(gvw2, () =>
            {
                grd2.DataSource = _groupsList;

                if (_editingUserGrpCd != null && _groupsList.All(g => g.UserGrpCd != _editingUserGrpCd))
                    EnterNewGroupMode();

                // 저장 직후 재조회(preserveSelection: true)에서만 방금 편집하던 그룹에게 포커스를
                // 되돌린다 - grd1(QueryUsersAsync)과 같은 이유(feedback_query_refocus_after_save).
                if (preserveSelection && _editingUserGrpCd != null)
                {
                    var handle = FindGroupRowHandle(_editingUserGrpCd);
                    if (handle != null) gvw2.FocusedRowHandle = handle.Value;
                }
            });

            if (_pendingLoadTask != null) await _pendingLoadTask;
            HideBusy();
        }
        finally
        {
            DrawingSuspension.Resume(this);
        }

        // DevExpress가 grd2.DataSource 재할당 직후 위에서 맞춰둔 FocusedRowHandle을 자기
        // 내부의 "첫 행 자동포커스" 로직으로 이벤트 없이 조용히 덮어쓰는 경우가 있다(바로 위
        // 동기 코드에서 handle을 정확히 찾아 설정해도 2026-09-11 재현: 인덱스 기반
        // GetRowHandle로 바꿔도 여전히 저장 후 포커스가 1번째 행으로 감). 그 내부 리셋이 이
        // 메서드가 반환되고 메시지 루프로 한 번 돌아간 뒤에 일어나는 것으로 보여, BeginInvoke로
        // 한 틱 미뤄 그 리셋이 끝난 다음에 다시 한번 확정한다.
        if (preserveSelection && _editingUserGrpCd != null)
        {
            var targetGrpCd = _editingUserGrpCd;
            BeginInvoke(new Action(() =>
            {
                var handle = FindGroupRowHandle(targetGrpCd);
                if (handle != null) gvw2.FocusedRowHandle = handle.Value;
            }));
        }
    }

    /// <summary>userGrpCd로 grd2의 행 핸들을 찾는다. 없으면 null. FindUserRowHandle과 같은 이유로
    /// GetRow(handle) 비교 대신 리스트 인덱스 -> GetRowHandle 변환 방식을 쓴다.</summary>
    private int? FindGroupRowHandle(string userGrpCd)
    {
        var index = _groupsList.FindIndex(g => string.Equals(g.UserGrpCd, userGrpCd, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return null;
        var handle = gvw2.GetRowHandle(index);
        return handle >= 0 ? handle : null;
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
        await QueryGroupsAsync(preserveSelection: false);
        EnterNewGroupMode();
        Toast.Show("사용중지 처리되었습니다.");
    }

    /// <summary>panData(탭2 상세)를 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 이유는
    /// EnterNewMode/EnterEditModeAsync(탭1)와 같다.</summary>
    private void EnterNewGroupMode()
    {
        SuppressDirtyTracking(() =>
        {
            _editingUserGrpCd = null;
            txtuser_grp_cd.Text = string.Empty;
            txtuser_grp_cd.ReadOnly = false;
            txtuser_grp_nm.Text = string.Empty;
            memodescription.Text = string.Empty;
        });

        _members = new();
        grd4.DataSource = null;
        _groupAuthItems = new();
        tree2.DataSource = null;
        SyncAuthColumns(tree2, _groupAuthItems, _authCols2);
        //tree2.Enabled = false;

        txtuser_grp_cd.Focus();
    }

    private async Task EnterEditGroupModeAsync(UserGroupListItemDto group)
    {
        SuppressDirtyTracking(() =>
        {
            _editingUserGrpCd = group.UserGrpCd;
            txtuser_grp_cd.Text = group.UserGrpCd;
            txtuser_grp_cd.ReadOnly = true; // 그룹코드는 PK라 수정 불가
            txtuser_grp_nm.Text = group.UserGrpNm;
            memodescription.Text = group.Description ?? string.Empty;
        });

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
        _groupAuthItems = FilterOutDeveloperTool(await ApiClient.GetAsync<List<MenuAuthItemDto>>(
            $"api/menu-auth?targetType=GRP&targetCd={Uri.EscapeDataString(userGrpCd)}") ?? new());

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
        SyncAuthColumns(tree2, _groupAuthItems, _authCols2);
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
        await QueryGroupsAsync(preserveSelection: true);
        Toast.Show(wasNew ? "사용자그룹이 등록되었습니다." : "수정되었습니다.");
    }

    private void grd1_Click(object sender, EventArgs e)
    {

    }
}
