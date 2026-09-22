using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using WYNLAB.Shared.Dtos;
using WYNLAB.Base;
using WYNLAB.Base.Controls;
using System.Drawing;

namespace WYNLAB.SM.MENU;

/// <summary>
/// 메뉴관리 화면. 좌측에 등록된 메뉴 전체를 트리로 보여주고, 우측에서 바로 등록/수정한다
/// (별도 팝업 없음 - UserEditForm/UserGroupEditForm과 달리 이 화면은 계층 구조 자체가
/// 핵심이라 트리를 보면서 바로 옆에서 편집하는 게 훨씬 직관적).
///
/// 신규/삭제는 표준 툴바가 아니라 화면 자체의 버튼/체크박스로만 한다(2026-09-16 요청,
/// ApplyMenuAuth 참고):
/// - "최상위메뉴추가" -> 트리 선택과 무관하게 항상 최상위(모듈)로 신규 등록
/// - "하위메뉴추가" -> 좌측 트리에서 현재 선택된 메뉴가 있으면 그 메뉴의 하위로 신규 등록
///   (상위메뉴ID/메뉴레벨 자동 세팅, 직접 입력 불가)
/// - 삭제 = panData의 "사용" 체크박스를 해제하고 저장(소프트삭제, USE_YN='N') - 다시 체크하고
///   저장하면 복구된다. 사용중지된 메뉴는 좌측 트리에서 회색 글씨로 표시된다(TreeListWyn 참고).
///
/// MENU_ID는 BIGINT IDENTITY라 더는 사람이 입력하지 않는다(서버가 채번) - 메뉴코드 자기입력
/// 필드는 없어졌고, 대신 등록된 메뉴의 ID를 읽기전용으로만 보여준다.
/// </summary>
public partial class frmMenu : BaseForm
{
    private List<MenuListItemDto> _menus = new();

    private long? _editingMenuId;   // null이면 신규모드, 값이 있으면 그 메뉴 수정모드
    private long? _lastSelectedId;  // 트리에서 마지막으로 선택된 메뉴 (신규 시 상위메뉴 후보)

    // AUTH01~10은 화면에 10칸이 반복될 뿐이라 배열로 묶어서 반복문으로 다룬다 - 필드 자체는
    // Designer.cs에 개별로 선언돼 있어야(txtAuthNm1..10) VS 디자이너에서 하나씩 옮길 수 있다.
    private TextEditWyn[] TxtAuthNm => new[]
    {
        txtAuthNm1, txtAuthNm2, txtAuthNm3, txtAuthNm4, txtAuthNm5,
        txtAuthNm6, txtAuthNm7, txtAuthNm8, txtAuthNm9, txtAuthNm10
    };

    public frmMenu()
    {
        InitializeComponent();

        Text = "메뉴관리";

        Controls.Add(BuildScreenHeader());


        // svgImageCollection1(디자이너 갤러리에서 고른 datapanel/open 두 아이콘)은 DevExpress 전용
        // 컬렉션이라 TreeList.SelectImageList(순정 ImageList)에 바로 못 꽂는다 - 여기서 각 항목을
        // 실제 비트맵으로 한 번 렌더링해서 옮겨줘야 한다(안 하면 ImageIndex를 줘도 아무 목록이
        // 없어서 그려지지 않는다 - 2026-09-16 실제 겪음).
        var menuTypeIcons = new System.Windows.Forms.ImageList { ImageSize = new Size(16, 16) };
        menuTypeIcons.Images.Add(svgImageCollection1.GetImage("datapanel", new Size(16, 16), null));
        menuTypeIcons.Images.Add(svgImageCollection1.GetImage("open", new Size(16, 16), null));
        menuTree.SelectImageList = menuTypeIcons;

        menuTree.NodeCellStyle += (s, e) =>
        {
            var menuType = e.Node.GetValue("MenuType") as string;
            e.Node.ImageIndex = e.Node.SelectImageIndex = menuType switch
            {
                "GROUP" => 1,   // menuTypeIcons의 1번 = "open"
                "FORM" => 0,    // menuTypeIcons의 0번 = "datapanel"
                _ => -1         // 아이콘 없음
            };
        };

        //cboMenuType.Properties.Items.AddRange(new[] { "GROUP", "FORM" });

        // 아이콘명 콤보 - 손입력 대신 실제 아이콘을 미리보기로 보고 고른다(2026-09-16 요청).
        // 목록은 WYNLAB.Shared.MenuIconCatalog 하나뿐이라, 사이드바(ShellForm.TopMenuIcons)가
        // 실제로 그릴 수 있는 아이콘과 항상 일치한다 - 여기 없는 값을 억지로 넣을 방법이 없다.
        // 지금은 최상위(모듈) 메뉴의 사이드바 아이콘에만 쓰이지만(하위 메뉴는 이 값이 아직
        // 화면에 반영 안 됨), 콤보 자체는 모든 레벨에서 선택 가능하게 둔다.
        BuildIconNmCombo();

        // 입력 컨트롤 폰트를 앱 공통 스케일(AppFonts.Body)로 통일 - 지정하지 않으면
        // DevExpress 기본 폰트(Tahoma 8.25pt)로 표시되어 화면마다 크기가 들쭉날쭉해 보였다.
        foreach (var edit in new BaseEdit[]
                 {
                     txtMenuId, txtMenuNm, txtUpperMenuId, txtUpperMenuNm, spnMenuLevel, cboMenuType,
                     cboModule, txtScreenClassNm, cboIconNm, txtProcPrefix, spnSortOrder, chkUseYn
                 }.Concat(TxtAuthNm))
        {
            edit.Properties.Appearance.Font = AppFonts.Body;
            edit.Properties.Appearance.Options.UseFont = true;
        }
        txtMenuNm.MarkRequired();
        cboMenuType.MarkRequired();

        // 상위메뉴 팝업(멀티필드 모드) - MatchField가 이 컨트롤 자신이 대표하는 팝업 결과
        // 컬럼(menu_nm)을 가리키고, MapField가 나머지 컬럼(menu_id)을 txtUpperMenuId에 채운다.
        txtUpperMenuNm.LookupKey = "P_MENU";
        txtUpperMenuNm.MatchField = "menu_nm";
        txtUpperMenuNm.MapField("menu_id", txtUpperMenuId);

        // 상위메뉴ID가 어떤 경로로 바뀌든(트리 클릭 기반 신규모드 진입, 또는 방금 추가한
        // 팝업으로 직접 변경) 메뉴레벨을 항상 "그 부모의 레벨 + 1"로 다시 맞춘다 - 팝업으로
        // 부모를 바꿨는데 예전 레벨이 그대로 남으면 저장 시 실제 계층과 안 맞는 레벨이 들어간다.
        txtUpperMenuId.EditValueChanged += (s, e) =>
        {
            var parent = long.TryParse(txtUpperMenuId.Text, out var parentId)
                ? _menus.FirstOrDefault(m => m.MenuId == parentId)
                : null;
            spnMenuLevel.Value = (parent?.MenuLevel ?? 0) + 1;
        };

        // 개발자용 마우스오버 툴팁(BindingField)이 읽어갈 정보 - 실제 적용은
        // BaseForm.ApplyBindingFieldTooltips가 공통으로 처리한다. TSMMENU는 다른 BA 테이블과
        // 달리 컬럼명이 대문자라 그대로 맞춘다. txtUpperMenuNm은 PopupLookupEditWyn이라
        // "Popup : P_MENU"도 자동으로 같이 붙는다.
        txtMenuId.Tag = new BindingFieldTag("MENU_ID");
        txtMenuNm.Tag = new BindingFieldTag("MENU_NM");
        txtUpperMenuId.Tag = new BindingFieldTag("UPPER_MENU_ID");
        txtUpperMenuNm.Tag = new BindingFieldTag("UPPER_MENU_ID");
        spnMenuLevel.Tag = new BindingFieldTag("MENU_LEVEL");
        cboMenuType.Tag = new BindingFieldTag("MENU_TYPE");
        cboModule.Tag = new BindingFieldTag("MODULE");
        txtScreenClassNm.Tag = new BindingFieldTag("SCREEN_CLASS_NM");
        cboIconNm.Tag = new BindingFieldTag("ICON_NM");
        txtProcPrefix.Tag = new BindingFieldTag("PROC_PREFIX");
        spnSortOrder.Tag = new BindingFieldTag("SORT_ORDER");
        chkUseYn.Tag = new BindingFieldTag("USE_YN");
        for (var i = 0; i < TxtAuthNm.Length; i++)
            TxtAuthNm[i].Tag = new BindingFieldTag($"AUTH{(i + 1):00}_NM");

        // 버그 수정(원래 코드에도 있던 주석): Dock을 실제로 지정한 적이 없어서 트리가 기본
        // 크기(작은 박스)로만 떠 있었다 - Designer.cs에서 Dock=Fill로 이미 잡아뒀다.
        menuTree.KeyFieldName = "MenuId";
        menuTree.ParentFieldName = "UpperMenuId";
        menuTree.OptionsBehavior.Editable = false;
        // 컬럼 폭 합이 패널보다 좁아도(또는 스플리터로 더 늘려도) 남는/모자란 폭을 컬럼이
        // 채우게 해서 가로 스크롤바가 생기지 않게 한다.
        menuTree.OptionsView.AutoWidth = true;
        menuTree.FocusedNodeChanged += (s, e) => OnTreeSelectionChanged();

        btnNewTop.Click += (s, e) => EnterNewMode(null, forceTop: true);
        btnNewChild.Click += (s, e) => EnterNewMode(_lastSelectedId);
        btnSaveInline.Click += async (s, e) => await SaveClick();
        btnCancelEdit.Click += (s, e) => EnterNewMode(_lastSelectedId);
        // 복사(Save As) - 지금 선택된 메뉴가 있을 때만 의미가 있다(EnterEditMode/EnterNewMode에서
        // Enabled를 맞춘다). 저장은 하지 않고 신규입력 상태(메뉴ID만 비움)로만 만들어둔다 -
        // 나머지 값은 전부 그대로 복사돼 있으니 저장 누르면 새 ID로 채번된다.
        btnCopy.Enabled = false;
        btnCopy.Click += (s, e) => EnterCopyMode();

        // 화면종료 시 저장 확인(BaseForm.ConfirmCloseAsync) - 이 화면은 panData에 편집 컨트롤을
        // 직접 담으므로 그걸 넘긴다. menuTree는 편집 불가(OptionsBehavior.Editable = false)라
        // 별도로 걸 것이 없다.
        TrackDirty(panData);

        EnterNewMode(null);
        Load += async (s, e) => await QueryClick();
    }

    /// <summary>WYNLAB.Shared.MenuIconCatalog의 각 항목을 실제 DevExpress SVG 아이콘으로 렌더링해서
    /// cboIconNm의 드롭다운에 "그림 + 설명"으로 보여준다(2026-09-16 요청 - "아이콘명을 실제
    /// 아이콘을 룩업에서 선택"). Value는 그 항목의 Key(예: "settings") 그대로라 TSMMENU.ICON_NM에
    /// 저장되는 값과 ShellForm.TopMenuIcons가 찾는 값이 항상 같다. WYNLAB.SM은 WYNLAB.Shell을
    /// 참조하지 않는 방향이라(모듈이 호스트를 참조하면 안 됨) SvgIcons.cs를 그대로 못 가져다
    /// 쓰고, 같은 API(DevExpress.Images.ImageResourceCache)를 여기서 직접 호출한다.</summary>
    private void BuildIconNmCombo()
    {
        var images = new System.Windows.Forms.ImageList { ImageSize = new Size(16, 16), ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit };
        var iconColor = System.Drawing.Color.FromArgb(90, 94, 102);

        foreach (var entry in WYNLAB.Shared.MenuIconCatalog.Entries)
        {
            // GetSvgImage는 이름+크기별로 캐싱된 인스턴스를 그대로 돌려주므로(SvgIcons.cs와 같은
            // 이유) Dispose하지 않는다 - ImageList.Images.Add가 내부적으로 복사해서 담는다.
            var rendered = DevExpress.Images.ImageResourceCache.Default.GetSvgImage(entry.SvgResourceName, null, new Size(16, 16));
            images.Images.Add(rendered ?? new System.Drawing.Bitmap(16, 16));

            cboIconNm.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(
                entry.DisplayNm, entry.Key, images.Images.Count - 1));
        }

        cboIconNm.Properties.SmallImages = images;
    }

/// <summary>이 화면은 등록/삭제를 표준 툴바(신규입력/삭제, 그리고 같은 플래그를 공유하는
/// 행추가/행삭제)가 아니라 화면 자체의 버튼("최상위메뉴추가"/"하위메뉴추가")과 panData의
/// "사용" 체크박스(체크 해제 후 저장 = 삭제, 다시 체크 후 저장 = 복구)로만 처리한다
/// (2026-09-16 요청) - 툴바 쪽 4개는 항상 꺼둬서 두 가지 경로가 헷갈리지 않게 한다.
/// ShellForm.UpdateToolbarPermissions가 CanInsert/CanDelete를 그대로 읽어가므로, 서버가
/// 내려준 실제 권한(base.ApplyMenuAuth) 위에 이 화면만 덮어쓴다.</summary>
protected override void ApplyMenuAuth()
    {
        base.ApplyMenuAuth();
        CanInsert = false;
        CanDelete = false;
    }

    public override async Task QueryClick()
    {
        _menus = await ApiClient.GetAsync<List<MenuListItemDto>>("api/menus") ?? new();
        menuTree.DataSource = _menus;
        menuTree.ExpandAll();

        // 편집 중이던 메뉴가 여전히 존재하면 선택 유지, 없어졌으면(삭제됨) 신규모드로 복귀
        if (_editingMenuId != null && _menus.All(m => m.MenuId != _editingMenuId))
        {
            EnterNewMode(null);
        }
    }

    // 표준 툴바 신규입력/삭제는 이 화면에서 항상 꺼져있다(ApplyMenuAuth) - BaseForm 추상
    // 멤버라 구현만 비워둔다. 실제 등록/삭제는 btnNewTop/btnNewChild/chkUseYn으로 한다.
    public override Task NewClick() => Task.CompletedTask;
    public override Task DeleteClick() => Task.CompletedTask;
    public override Task NewRowClick() => Task.CompletedTask;
    public override Task DeleteRowClick() => Task.CompletedTask;

    private void OnTreeSelectionChanged()
    {
        var node = menuTree.FocusedNode;
        if (node == null) return;

        var menuId = Convert.ToInt64(node.GetValue("MenuId"));
        var menu = _menus.FirstOrDefault(m => m.MenuId == menuId);
        if (menu == null) return;

        _lastSelectedId = menuId;
        EnterEditMode(menu);
    }

    /// <summary>신규모드 진입. parentId가 있으면 그 메뉴의 하위로, forceTop이면(또는 parentId가
    /// null이면) 트리 선택과 무관하게 최상위(모듈)로 등록되도록 상위메뉴ID/메뉴레벨을 자동
    /// 세팅한다. 편집 컨트롤 값을 채우는 부분은 SuppressDirtyTracking으로 감싼다 - 안 그러면
    /// 코드가 값을 채우는 것뿐인데 TrackDirty(panData)가 "사용자가 고쳤다"로 오인해서, 트리
    /// 선택/신규모드 진입 직후부터 화면을 닫을 때 저장 확인이 뜨는 오작동이 생긴다.</summary>
    private void EnterNewMode(long? parentId, bool forceTop = false)
    {
        var newParentId = forceTop ? null : parentId;
        var parent = newParentId != null ? _menus.FirstOrDefault(m => m.MenuId == newParentId) : null;

        SuppressDirtyTracking(() =>
        {
            _editingMenuId = null;

            txtMenuId.Text = string.Empty;
            txtMenuNm.Text = string.Empty;
            txtUpperMenuId.Text = parent?.MenuId.ToString() ?? string.Empty;
            txtUpperMenuNm.Text = parent?.MenuNm ?? string.Empty;
            spnMenuLevel.Value = (parent?.MenuLevel ?? 0) + 1;
            cboMenuType.EditValue = "FORM";
            cboModule.EditValue = string.Empty;
            txtScreenClassNm.Text = string.Empty;
            cboIconNm.EditValue = null;
            txtProcPrefix.Text = string.Empty;
            spnSortOrder.Value = 0;
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)
            foreach (var t in TxtAuthNm) t.Text = string.Empty;
        });

        lblFormTitle.Text = parent != null ? "신규 메뉴 등록 (하위 메뉴)" : "신규 메뉴 등록 (최상위 모듈)";
        lblFormHint.Text = parent != null
            ? $"'{parent.MenuNm}' 메뉴의 하위로 등록됩니다."
            : "최상위 메뉴(모듈)로 등록됩니다. 좌측 트리 최상단에 새 가지가 생깁니다.";

        btnCopy.Enabled = false;
        txtMenuNm.Focus();
    }

    /// <summary>지금 우측 패널에 표시된 값(선택된 메뉴 한 건)을 그대로 둔 채 메뉴ID만 비우고
    /// 신규입력 상태로 전환한다 - 저장은 여기서 하지 않는다(사용자가 직접 저장 버튼을 눌러야
    /// 새 ID로 실제 등록됨). 비슷한 메뉴를 여러 개 등록할 때 매번 전체 필드를 다시 채울 필요
    /// 없이 몇 군데만 바꿔서 저장하면 되도록 하기 위함(사장님 요청).</summary>
    private void EnterCopyMode()
    {
        if (_editingMenuId == null) return; // btnCopy가 이 상태에선 비활성화라 보통 여기 안 옴

        var sourceId = _editingMenuId;
        var sourceNm = txtMenuNm.Text;

        SuppressDirtyTracking(() =>
        {
            _editingMenuId = null;
            txtMenuId.Text = string.Empty;
            // 메뉴명/상위메뉴/레벨/유형/모듈/화면클래스명/아이콘/정렬순서/AUTH01~10은 그대로 복사 -
            // 여기서 일부러 안 지운다.
            chkUseYn.Checked = true;
            chkUseYn.Enabled = false; // 신규는 항상 사용상태로 생성됨(서버에서 'Y' 고정)
        });

        lblFormTitle.Text = "메뉴 복사 등록";
        lblFormHint.Text = $"'{sourceNm}(ID:{sourceId})'의 값을 그대로 복사했습니다. 확인 후 저장하면 새 ID로 등록됩니다.";

        btnCopy.Enabled = false;
        txtMenuNm.Focus();
    }

    private void EnterEditMode(MenuListItemDto menu)
    {
        var parent = menu.UpperMenuId != null ? _menus.FirstOrDefault(m => m.MenuId == menu.UpperMenuId) : null;

        SuppressDirtyTracking(() =>
        {
            _editingMenuId = menu.MenuId;

            txtMenuId.Text = menu.MenuId.ToString();
            txtMenuNm.Text = menu.MenuNm;
            txtUpperMenuId.Text = menu.UpperMenuId?.ToString() ?? string.Empty;
            txtUpperMenuNm.Text = parent?.MenuNm ?? string.Empty;
            spnMenuLevel.Value = menu.MenuLevel;
            cboMenuType.EditValue = menu.MenuType;
            cboModule.EditValue = menu.Module ?? string.Empty;
            txtScreenClassNm.Text = menu.ScreenClassNm ?? string.Empty;
            cboIconNm.EditValue = menu.IconNm;
            txtProcPrefix.Text = menu.ProcPrefix ?? string.Empty;
            spnSortOrder.Value = menu.SortOrder;
            // "삭제 여부" 편집 지점 - 사용중지(USE_YN='N')된 메뉴를 선택하면 여기 체크가 풀린
            // 채로 뜬다. 다시 체크하고 저장하면 USP_SM_MENU_S(U)를 거쳐 복구된다(2026-09-16
            // 요청 - 툴바 삭제 버튼을 없앤 대신 이 체크박스가 삭제/복구를 모두 담당한다).
            chkUseYn.Checked = menu.UseYn;
            chkUseYn.Enabled = true;
            for (var i = 0; i < TxtAuthNm.Length; i++)
                TxtAuthNm[i].Text = i < menu.AuthNm.Length ? (menu.AuthNm[i] ?? string.Empty) : string.Empty;
        });

        lblFormTitle.Text = "메뉴 수정";
        lblFormHint.Text = parent != null ? $"상위 메뉴: {parent.MenuNm}" : "최상위 메뉴(모듈)입니다.";

        btnCopy.Enabled = true;
    }

    public override async Task SaveClick()
    {
        if (string.IsNullOrWhiteSpace(txtMenuNm.Text))
        {
            AppMessageBox.Show("메뉴명은 필수입니다.", "확인");
            return;
        }

        // 모듈/화면 클래스명은 "WYNLAB.{Module}.{ScreenClassNm}, WYNLAB.{Module}" 형태로 조합되는
        // 손입력 텍스트라 오타가 나도 저장 시점엔 아무 에러가 안 나고, 나중에 그 메뉴를 클릭했을
        // 때서야 화면을 못 찾는다는 에러로 드러난다 - 저장 전에 미리 Type.GetType으로 실제
        // 존재하는 클래스인지 확인해서 막는다(ShellForm.OpenMenuForm과 같은 조합 방식).
        // Shell.exe가 시작할 때 ModuleLoader.LoadAll이 Modules\ 폴더의 모든 화면 dll을 이미
        // 로드해뒀고 AssemblyResolve 훅도 걸어놔서(ModuleLoader.cs 참고), 여기서 Type.GetType을
        // 호출하면 ShellForm이 실제 메뉴 클릭 시 찾는 것과 완전히 같은 경로로 찾아본다.
        var module =cboModule.EditValue.ToString();
        var screenClassNm = txtScreenClassNm.Text.Trim();
        if ((string)cboMenuType.EditValue.ToString() == "FORM"
            && !string.IsNullOrWhiteSpace(module) && !string.IsNullOrWhiteSpace(screenClassNm)
            && Type.GetType($"WYNLAB.{module}.{screenClassNm}, WYNLAB.{module}") == null)
        {
            AppMessageBox.Show(
                $"화면 클래스를 찾을 수 없습니다: WYNLAB.{module}.{screenClassNm}, WYNLAB.{module}\n" +
                "모듈/화면 클래스명이 맞는지, 해당 화면 모듈(dll)이 배포되어 있는지 확인해주세요.",
                "확인");
            return;
        }

        btnSaveInline.Enabled = false;
        try
        {
            ApiResult? result;
            long savedMenuId;
            var wasNew = _editingMenuId == null;
            var authNm = TxtAuthNm.Select(t => string.IsNullOrWhiteSpace(t.Text) ? null : t.Text).ToArray();
            var upperMenuId = long.TryParse(txtUpperMenuId.Text, out var parsedUpperId) ? parsedUpperId : (long?)null;

            if (_editingMenuId != null)
            {
                savedMenuId = _editingMenuId.Value;
                var req = new MenuUpdateRequest
                {
                    MenuNm = txtMenuNm.Text,
                    UpperMenuId = upperMenuId,
                    MenuLevel = (int)spnMenuLevel.Value,
                    MenuType = (string)cboMenuType.EditValue,
                    Module = module,
                    ScreenClassNm = screenClassNm,
                    IconNm = cboIconNm.EditValue as string,
                    ProcPrefix = txtProcPrefix.Text.Trim(),
                    SortOrder = (int)spnSortOrder.Value,
                    UseYn = chkUseYn.Checked,
                    AuthNm = authNm
                };
                result = await ApiClient.PutAsync<MenuUpdateRequest, ApiResult>($"api/menus/{_editingMenuId}", req);
            }
            else
            {
                var req = new MenuCreateRequest
                {
                    MenuNm = txtMenuNm.Text,
                    UpperMenuId = upperMenuId,
                    MenuLevel = (int)spnMenuLevel.Value,
                    MenuType = (string)cboMenuType.EditValue,
                    Module = module,
                    ScreenClassNm = screenClassNm,
                    IconNm = cboIconNm.EditValue as string,
                    ProcPrefix = txtProcPrefix.Text.Trim(),
                    SortOrder = (int)spnSortOrder.Value,
                    AuthNm = authNm
                };
                result = await ApiClient.PostAsync<MenuCreateRequest, ApiResult>("api/menus", req);
                savedMenuId = result != null && result.Success && long.TryParse(result.GeneratedCode, out var newId) ? newId : 0;
            }

            if (result == null || !result.Success)
            {
                AppMessageBox.Show(result?.Message ?? "저장에 실패했습니다.", "저장 실패");
                return;
            }

            await QueryClick();

            var savedNode = menuTree.FindNodeByKeyID(savedMenuId);
            if (savedNode != null) menuTree.FocusedNode = savedNode;

            Toast.Show(wasNew ? "메뉴가 등록되었습니다." : "수정되었습니다.");
        }
        finally
        {
            btnSaveInline.Enabled = true;
        }
    }
}
