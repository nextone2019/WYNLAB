using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WYNLAB.Base.Controls;
using WYNLAB.Shared.Dtos;

namespace WYNLAB.Base;

/// <summary>
/// 모든 업무화면(MDI 자식폼)의 최상위 베이스.
/// XtraForm(DevExpress) 상속 - 스킨/테마가 자동 적용됨.
/// 여기에 공통 예외처리, 권한 반영, 화면종료 확인 등을 구현한다.
/// </summary>
public class BaseForm : XtraForm
{
    /// <summary>TSMMENU.MENU_ID - 로그인 시 내려받은 권한을 이 값으로 조회해서 버튼 활성화/
    /// 비활성화 처리. 화면 자신이 생성자에서 정하는 값이 아니라 ShellForm이 메뉴트리/바로가기/
    /// 화면검색 등으로 이 화면을 열 때 그 순간 채워준다(ShellForm.OpenMenuForm 참고) - MENU_ID는
    /// DB(로컬/서버)마다 IDENTITY로 다르게 채번되어 화면 스스로 상수로 못 박아두기 때문이다
    /// (예전 MenuCd 문자열은 화면이 직접 "SM_MENU"처럼 하드코딩했었다).</summary>
    public long MenuId { get; set; }

    public bool CanInsert { get; protected set; }
    public bool CanUpdate { get; protected set; }
    public bool CanDelete { get; protected set; }
    public bool CanPrint { get; protected set; } = true;
    public bool CanExcel { get; protected set; } = true;

    /// <summary>TSMMENUAUTH.AUTH01~10 - 조회/입력/저장/출력/엑셀 이외에 이 화면에 추가로
    /// 필요해진 권한이 있을 때 화면 개발자가 그대로 참조한다(사장님 지시, 2026-08-31).
    /// 인덱스 0=AUTH01 ... 9=AUTH10, 예: <c>if (!Auth[0]) btnSpecial.Enabled = false;</c>.
    /// 지금은 전부 의미가 정해지지 않아 기본 false(권한부여관리 화면에도 아직 컬럼이 없음).</summary>
    public bool[] Auth { get; protected set; } = new bool[10];

    /// <summary>
    /// 로그인 세션값 - 모든 업무화면(모듈 DLL 포함)에서 그대로 사용.
    /// 값의 실제 출처는 서버의 SSP_WYNLAB_GetSession 프로시저 결과(Session 클래스 참고).
    /// </summary>
    protected string CurrentUserId => Session.UserId;
    protected string CurrentUserNm => Session.UserNm;
    protected string CurrentEmpNo => Session.EmpNo;
    protected string CurrentDeptNm => Session.DeptNm;
    protected bool CurrentIsAdmin => Session.IsAdmin;
    protected bool CurrentIsDeveloper => Session.IsDeveloper;

    // ===== MDI 상단 공통 툴바(조회/입력/삭제/행추가/행삭제/저장/출력)가 호출하는 표준 액션 =====
    // Shell의 툴바 버튼은 현재 활성화된 MDI 자식폼(this)의 아래 메서드를 그대로 호출한다.
    // 화면마다 필요한 것만 override 하면 되고, 안 쓰는 기능은 기본값(아무 동작 안 함)으로 둔다.
    // 행추가/행삭제(NewRowClick/DeleteRowClick)는 그리드 안에서 바로 편집하는(인라인 편집) 화면용 -
    // 서버 저장은 별도로 SaveClick에서 한번에 하고, 이 둘은 그리드 위 행 자체만 늘리고/줄인다.
    public virtual Task QueryClick() => Task.CompletedTask;
    public virtual Task NewClick() => Task.CompletedTask;
    public virtual Task DeleteClick() => Task.CompletedTask;
    public virtual Task NewRowClick() => Task.CompletedTask;
    public virtual Task DeleteRowClick() => Task.CompletedTask;
    public virtual Task SaveClick() => Task.CompletedTask;
    public virtual Task PrintClick() => Task.CompletedTask;

    /// <summary>메뉴 클릭이 아니라 다른 화면(결재함 등)이 특정 건을 바로 보여주려고 이 화면을 연
    /// 경우에 호출된다 - key는 그 화면의 PK를 문자열로 넘긴 값(예: req_id). 기본은 아무 동작
    /// 안 함(메뉴로 여는 일반적인 화면은 신경 쓸 필요 없음) - 결재대상 문서처럼 "결재함에서
    /// 바로 포커스해서 열려야 하는" 화면만 override해서 그 키로 조회+상세 진입하면 된다.</summary>
    public virtual Task FocusRecordAsync(string key) => Task.CompletedTask;

    /// <summary>
    /// 화면 타이틀 바(BuildScreenHeader) 왼쪽에 그려지는 아이콘. 기본은 DevExpress 내장 SVG
    /// "datapanel"(Outlook Inspired 세트, 리소스명 "svgimages/outlook%20inspired/datapanel.svg" -
    /// PowerShell로 DevExpress.Images.v21.2.dll을 직접 로드해서 ImageResourceCache.GetSvgImage
    /// 호출까지 실제로 검증함, 2026-09-09) - 예전엔 손으로 그린 폴더 모양(MenuIconPainters.Folder)
    /// 이었는데, "폴더는 담는 그릇이라 개별 업무화면 자체를 가리키기엔 어색하다"는 지적과 함께
    /// SectionHeaderWyn의 SvgIcon 갤러리에서 사용자가 직접 고른 이 아이콘을 쓰라는 요청으로 바꿨다.
    /// 이 SVG는 단색 실루엣이 아니라 2색(테두리 회색 + 내용 파랑)짜리라 다른 SVG 아이콘들과 달리
    /// 색을 덧씌우지(tint) 않고 원본 그대로 쓴다. 리소스를 못 찾는 극히 드문 경우(다른 PC의
    /// DevExpress 버전 차이 등)엔 예전 폴더 아이콘으로 안전하게 되돌아간다.
    ///
    /// 화면 성격에 더 맞는 아이콘이 있으면(예: 메뉴관리의 트리 아이콘) 화면 클래스에서 override한다.
    /// </summary>
    protected virtual Image ScreenIcon =>
        DevExpress.Images.ImageResourceCache.Default.GetSvgImage(
            "svgimages/outlook%20inspired/datapanel.svg", null, new Size(16, 16))
        ?? MenuIconPainters.Render(MenuIconPainters.Folder, 16, Color.FromArgb(120, 124, 132));

    // ===== 저장프로시저 직접 호출(범용 데이터 통로) =====
    // 화면마다 서버에 Controller/Repository를 만들지 않고 프로시저를 바로 부른다. 덕분에
    // 검색조건을 하나 추가할 때 화면과 프로시저만 고치면 되고 서버는 배포하지 않아도 된다.
    // 설계 배경과 보안 모델은 저장소 루트의 GENERIC_DATA_API.md 참고.
    //
    // MenuId는 화면이 이미 갖고 있으므로(ShellForm이 열 때 채워줌) 여기서 자동으로 채운다 -
    // 서버가 이 값으로 권한과 실행 가능한 프로시저를 판단하기 때문에, 화면마다 손으로 넘기게
    // 두면 빠뜨리기 쉽다.

    /// <summary>조회 - 첫 번째 결과셋을 돌려준다. 그리드에 그대로 바인딩하면 된다.</summary>
    protected Task<System.Data.DataTable> QueryAsync(string procName, object? parameters = null) =>
        ProcData.QueryAsync(MenuId, procName, parameters);

    /// <summary>조회 - 결과셋을 여러 개 돌려주는 프로시저용.</summary>
    protected Task<List<System.Data.DataTable>> QueryMultiAsync(string procName, object? parameters = null) =>
        ProcData.QueryMultiAsync(MenuId, procName, parameters);

    /// <summary>저장/삭제 - 필요한 권한은 서버가 p_work_type(N/U/D)을 보고 판단한다.</summary>
    protected Task<WYNLAB.Shared.Dtos.ApiResult> SaveAsync(string procName, object? parameters = null) =>
        ProcData.SaveAsync(MenuId, procName, parameters);

    /// <summary>ProcData.ToDataTable(internal, WYNLAB.BaseForm 전용)을 다른 모듈 어셈블리의
    /// BaseForm 자식 화면들도 쓸 수 있게 다시 열어준다 - api/data/query가 아닌 별도 엔드포인트를
    /// 직접 호출해서 DataQueryResponse를 받은 경우(예: frmSysLookup의 실행결과 미리보기)에
    /// 그리드에 바인딩할 DataTable로 바꿀 때 쓴다.</summary>
    protected static System.Data.DataTable ToDataTable(WYNLAB.Shared.Dtos.DataTableResult result) =>
        ProcData.ToDataTable(result);

    public BaseForm()
    {
        // 폼의 글꼴을 지정해두면 자기 글꼴을 따로 정하지 않은 자식 컨트롤이 전부 이걸 물려받는다.
        // DevExpress 컨트롤은 Program.cs의 WindowsFormsSettings.DefaultFont가 맡지만, 순정
        // WinForms 컨트롤(Panel/Label 등)은 그 설정을 따르지 않아서 이쪽도 같이 잡아야
        // 한 화면 안에서 글꼴이 갈리지 않는다.
        this.Font = AppFonts.Body;

        this.MdiParent = null; // Shell에서 폼 생성 후 주입
        this.Load += BaseForm_Load;
        this.FormClosing += BaseForm_FormClosing;
    }

    // ===== 화면종료 확인(저장 안 된 변경사항) =====
    // 개별 탭의 X버튼과 ShellForm의 일괄닫기(다른 탭 모두 닫기/모두 닫기/탭 전체 닫기 버튼)가
    // 전부 이 한 자리를 공통으로 거친다 - 화면마다 "닫을 때 저장 확인" 로직을 따로 만들 필요가
    // 없다. 2026-08-28, 사장님 요청으로 추가.

    private bool _suppressDirtyTracking;
    private bool _isDirty;
    private bool _closeConfirmed;

    /// <summary>사용자가 값을 고쳤는지 여부. 화면 코드에서 직접 켜고 끄지 말고 TrackDirty/
    /// SuppressDirtyTracking을 통해서만 건드릴 것 - 그래야 "코드가 값을 채우는 것"과 "사용자가
    /// 고친 것"이 항상 정확히 구분된다.</summary>
    protected bool IsDirty
    {
        get => _isDirty;
        set => _isDirty = value;
    }

    /// <summary>화면을 닫을 때 저장 여부를 물어야 하는지. 기본은 IsDirty 그대로지만, panData
    /// 추적만으로 부족한 화면은 override해서 조건을 더할 수 있다.</summary>
    protected virtual bool HasUnsavedChanges => IsDirty;

    /// <summary>container 아래 모든 DevExpress 편집 컨트롤(TextEdit/LookUpEdit/CheckEdit/
    /// DateEdit/MemoEdit 등 - 전부 BaseEdit 하위 타입이라 이 한 자리에서 공통으로 잡힌다)에
    /// 변경 감지를 건다. 보통 생성자에서 panData 패널 하나만 통째로 넘기면 된다 - panData
    /// 안의 컨트롤 구성이나 타입이 나중에 바뀌어도(TextEdit -> LookUpEditWyn 등) 이 호출은
    /// 그대로 둬도 된다.</summary>
    protected void TrackDirty(Control container)
    {
        foreach (Control child in container.Controls)
        {
            if (child is BaseEdit edit)
            {
                edit.EditValueChanged += (_, _) => { if (!_suppressDirtyTracking) IsDirty = true; };
            }
            if (child.Controls.Count > 0) TrackDirty(child);
        }
    }

    /// <summary>편집 가능한 하위 그리드(grd2 등)의 DataTable에 변경 감지를 건다. 조회로 새
    /// DataTable을 받아 다시 바인딩할 때마다(재조회, EnterNewMode의 Clone() 등) 그 새 인스턴스에
    /// 대해 다시 호출해야 한다 - 이전 테이블에 걸어둔 구독은 그 테이블을 더 이상 안 쓰면서
    /// 자연히 무의미해진다. 방금 서버에서 막 채워 받은 테이블에 거는 것이라 최초 채움 자체는
    /// RowChanged를 발생시키지 않으므로(이미 채워진 뒤에 참조를 받음) SuppressDirtyTracking으로
    /// 감쌀 필요가 없다.</summary>
    protected void TrackDirty(DataTable table)
    {
        table.RowChanged += (_, _) => { if (!_suppressDirtyTracking) IsDirty = true; };
        table.RowDeleted += (_, _) => { if (!_suppressDirtyTracking) IsDirty = true; };
    }

    /// <summary>EnterEditMode/EnterNewMode처럼 "코드가 값을 채우는" 구간을 감싼다. 이 안에서
    /// 발생하는 변경은 사용자가 고친 게 아니므로 dirty로 잡히지 않고, 끝나면 IsDirty를 명시적으로
    /// false로 되돌린다 - 재조회/신규모드 진입은 항상 "변경 없음" 상태에서 시작해야 한다.</summary>
    protected void SuppressDirtyTracking(Action action)
    {
        _suppressDirtyTracking = true;
        try { action(); }
        finally
        {
            _suppressDirtyTracking = false;
            IsDirty = false;
        }
    }

    /// <summary>SuppressDirtyTracking의 비동기 버전 - 코드가 값을 채우는 구간에 await가 섞여
    /// 있을 때 쓴다(예: 서버에서 받아온 이미지를 PictureEdit.Image에 채우는 것처럼, 그 대입도
    /// BaseEdit 계열이라 EditValueChanged가 뜨는 컨트롤을 async 흐름 안에서 채우는 경우 -
    /// frmSiteConfig가 로그인배경/로고/파비콘을 QueryClick에서 이렇게 채운다). 동기 버전을 그대로
    /// await 하나만 감싸는 걸로는 안 된다 - _suppressDirtyTracking이 꺼진 뒤에 실행되는 await
    /// 이후 코드는 보호를 못 받는다(2026-09-09 실제 발견 - frmSiteConfig가 저장 직후 QueryClick을
    /// 다시 부르는데, 이미지 3장을 SuppressDirtyTracking 블록 "밖"에서 채우고 있어서 저장하자마자
    /// 다시 IsDirty=true가 되어 "변경 내역이 존재합니다" 확인창이 매번 다시 떴다).</summary>
    protected async Task SuppressDirtyTrackingAsync(Func<Task> action)
    {
        _suppressDirtyTracking = true;
        try { await action(); }
        finally
        {
            _suppressDirtyTracking = false;
            IsDirty = false;
        }
    }

    /// <summary>
    /// 변경사항이 있으면 "{화면명} 화면의 변경 내역이 존재 합니다. 저장 후 종료 하시겠습니까?"를
    /// 묻고, "예"면 저장까지 마친 뒤 닫아도 되는지 판단한다. 개별 탭의 X버튼(아래 FormClosing)과
    /// ShellForm의 일괄닫기 양쪽이 이 메서드 하나를 공통으로 쓴다.
    /// 반환값 true = 닫아도 된다(변경 없음 / "예"로 저장 성공 / "아니오"로 저장 없이 닫기),
    /// false = 저장에 실패해서 화면을 열어둔 채로 둬야 한다.
    /// </summary>
    public async Task<bool> ConfirmCloseAsync()
    {
        if (_closeConfirmed) return true;

        if (HasUnsavedChanges)
        {
            var confirm = AppMessageBox.Show(
                $"{Text} 화면의 변경 내역이 존재 합니다.\n저장 후 종료 하시겠습니까?",
                "변경 내역 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                await SafeExecuteAsync(SaveClick, "저장");
                if (HasUnsavedChanges) return false; // 저장 실패(또는 필수값 누락 등) - 열어둔 채로 둔다
            }
        }

        _closeConfirmed = true;
        return true;
    }

    // ===== 마스터 그리드 행 전환 시 저장 확인(2026-09-06, 사장님 지시 - "모든 화면에서 동일하게
    // 적용되어야 할 기능") =====
    // grd1(대분류/거래처 등 마스터 목록)에서 다른 행을 고르면 그 순간 panData/grd2(상세, 편집
    // 가능)가 새로 채워지는 화면들(frmMinorCode, frmCust 등)에서, 상세 쪽에 저장 안 된 변경이
    // 있어도 그냥 버리고 넘어가던 것을 "저장하시겠습니까?"로 먼저 물어보게 한다.

    /// <summary>ConfirmMasterRowSwitch가 그리드별로 기억해두는 "마지막으로 확정된(그대로 있어도 되는)
    /// 포커스 행 핸들" - 되돌릴 때 이 값을 쓴다.</summary>
    private readonly Dictionary<GridView, int> _confirmedRowHandles = new();
    private bool _suppressMasterRowSwitchConfirm;

    /// <summary>
    /// 마스터 그리드의 FocusedRowObjectChanged 핸들러 맨 앞에서 이 메서드를 부르고, 실제로 하던 일
    /// (EnterEditMode 등)은 onRowSelected 콜백으로 넘긴다:
    /// <code>gvw1.FocusedRowObjectChanged += (s, e) => ConfirmMasterRowSwitch(gvw1, e, row => EnterEditMode(row));</code>
    ///
    /// DevExpress WinForms GridView(21.2)에는 "행이 실제로 바뀌기 전" 취소할 수 있는 이벤트가 없다
    /// (직접 확인 - DevExpress.XtraGrid.Views.Base에 FocusedRowChangedEventArgs만 있고 Changing류는
    /// 없음). 그래서 이미 바뀐 뒤(FocusedRowObjectChanged)에 일단 이전 행으로 조용히 되돌리고
    /// 확인(예/아니오/취소)을 받은 뒤, 확정되면 다시 원래 옮기려던 행으로 이동하는 방식으로
    /// "바뀌기 전에 막는" 것처럼 흉내낸다. 그리드 자체(grd1)는 조회전용이라 여기서 다루는 "저장 안
    /// 된 변경"은 항상 grd1이 아니라 panData/grd2 등 상세 쪽 편집 내용이다(HasUnsavedChanges 그대로
    /// 재사용 - ConfirmCloseAsync와 같은 기준).
    ///
    /// 되돌리기/재이동으로 인한 재귀 호출을 이 메서드 스스로 처리하므로, 화면 쪽 EnterEditMode 등은
    /// "실제로 선택이 확정된 행"만 신경 쓰면 된다 - 취소/버림/저장실패로 원래 자리에 머무는 경우도
    /// onRowSelected가 그 행에 대해 다시 호출되므로 패널이 항상 grd1의 실제 포커스 행과 일치한다.
    /// </summary>
    protected void ConfirmMasterRowSwitch(GridView masterView, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e, Action<DataRowView> onRowSelected)
    {
        if (e.Row is DataRowView view) ConfirmMasterRowSwitchCore(masterView, view, onRowSelected);
    }

    /// <summary>List&lt;T&gt;(POCO DTO)에 바인딩된 마스터 그리드용 - frmUserAuth처럼 e.Row가
    /// DataRowView가 아니라 DTO 자신인 화면에서 쓴다. 타입 추론이 되도록 람다 매개변수에 타입을
    /// 명시해야 한다: <code>ConfirmMasterRowSwitch(gvw1, e, (UserListItemDto user) => ...);</code></summary>
    protected void ConfirmMasterRowSwitch<TRow>(GridView masterView, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e, Action<TRow> onRowSelected) where TRow : class
    {
        if (e.Row is TRow row) ConfirmMasterRowSwitchCore(masterView, row, onRowSelected);
    }

    private async void ConfirmMasterRowSwitchCore<TRow>(GridView masterView, TRow row, Action<TRow> onRowSelected) where TRow : class
    {
        var targetHandle = masterView.FocusedRowHandle;

        if (!_suppressMasterRowSwitchConfirm && HasUnsavedChanges
            && _confirmedRowHandles.TryGetValue(masterView, out var previousHandle)
            && previousHandle != targetHandle && previousHandle >= 0 && previousHandle < masterView.RowCount)
        {
            masterView.FocusedRowHandle = previousHandle; // 재귀 호출 - 이 handle에 대해 onRowSelected까지 알아서 다시 불림

            var confirm = AppMessageBox.Show(
                "변경 내역이 저장되지 않았습니다.\n저장하시겠습니까?",
                "변경 내역 확인", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (confirm == DialogResult.Cancel) return; // 되돌린 행에 그대로 머무른다

            if (confirm == DialogResult.Yes)
            {
                await SafeExecuteAsync(SaveClick, "저장");
                if (HasUnsavedChanges) return; // 저장 실패 - 되돌린 행에 그대로 머무른다
            }
            else
            {
                IsDirty = false; // "아니오" - 미저장 변경을 버리고 이동한다
            }

            if (targetHandle >= 0 && targetHandle < masterView.RowCount)
                masterView.FocusedRowHandle = targetHandle; // 재귀 호출 - 원래 옮기려던 행으로 확정
            return;
        }

        _confirmedRowHandles[masterView] = targetHandle;
        onRowSelected(row);
    }

    /// <summary>재조회/저장 직후처럼 화면이 스스로 grd1을 다시 그리거나 포커스를 옮기는 구간을
    /// 감싼다 - 이 구간에서 발생하는 FocusedRowObjectChanged는 사용자가 고른 게 아니므로
    /// ConfirmMasterRowSwitch의 확인을 타면 안 된다(특히 저장 직후엔 AcceptChanges가 RowChanged를
    /// 안 내서 IsDirty가 아직 true로 남아있는 채로 재조회가 걸려, 저장하자마자 또 확인창이
    /// 뜨는 오작동이 생긴다 - 실제로 겪음).</summary>
    protected void SuppressMasterRowSwitchConfirm(GridView masterView, Action action)
    {
        _suppressMasterRowSwitchConfirm = true;
        try { action(); }
        finally
        {
            _suppressMasterRowSwitchConfirm = false;
            _confirmedRowHandles[masterView] = masterView.FocusedRowHandle;
        }
    }

    // ===== 마스터가 트리(TreeListWyn)인 화면용 - GridView 버전과 완전히 같은 원리(2026-09-08,
    // TplTreeMasterSubGrid 템플릿 추가하며 같이 만듦). DevExpress TreeList도 GridView와 마찬가지로
    // "포커스 노드가 바뀌기 전" 취소 가능한 이벤트가 없어서(FocusedNodeChanged만 있음, Changing류
    // 없음 - 직접 확인) 같은 트릭(일단 이전 노드로 되돌리고 확인받은 뒤 원래 노드로 재이동)을 쓴다.
    // GridView 버전의 FocusedRowHandle(int)을 TreeListNode 참조로만 바꾼 것 - 로직은 동일하다.
    // TreeList.GetDataRecordByNode(node)가 GridView의 DataRowView와 똑같은 타입(DataTable에
    // 바인딩했다면 DataRowView)을 돌려주므로 onRowSelected 콜백 시그니처(Action<DataRowView>)도
    // GridView 버전과 그대로 재사용할 수 있다. =====

    private readonly Dictionary<TreeList, TreeListNode?> _confirmedTreeNodes = new();

    /// <summary>
    /// 마스터 트리의 FocusedNodeChanged 핸들러 맨 앞에서 이 메서드를 부르고, 실제로 하던 일은
    /// onRowSelected 콜백으로 넘긴다:
    /// <code>tree1.FocusedNodeChanged += (s, e) => ConfirmMasterRowSwitch(tree1, e, row => _ = OnMasterSelectedAsync(row.Row));</code>
    /// </summary>
    protected void ConfirmMasterRowSwitch(TreeList masterTree, FocusedNodeChangedEventArgs e, Action<DataRowView> onRowSelected)
    {
        if (e.Node != null && masterTree.GetDataRecordByNode(e.Node) is DataRowView view)
            ConfirmMasterRowSwitchCoreTree(masterTree, e.Node, view, onRowSelected);
    }

    private async void ConfirmMasterRowSwitchCoreTree<TRow>(TreeList masterTree, TreeListNode targetNode, TRow row, Action<TRow> onRowSelected) where TRow : class
    {
        if (!_suppressMasterRowSwitchConfirm && HasUnsavedChanges
            && _confirmedTreeNodes.TryGetValue(masterTree, out var previousNode)
            && previousNode != targetNode && previousNode != null)
        {
            masterTree.FocusedNode = previousNode; // 재귀 호출 - 이 노드에 대해 onRowSelected까지 알아서 다시 불림

            var confirm = AppMessageBox.Show(
                "변경 내역이 저장되지 않았습니다.\n저장하시겠습니까?",
                "변경 내역 확인", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (confirm == DialogResult.Cancel) return; // 되돌린 노드에 그대로 머무른다

            if (confirm == DialogResult.Yes)
            {
                await SafeExecuteAsync(SaveClick, "저장");
                if (HasUnsavedChanges) return; // 저장 실패 - 되돌린 노드에 그대로 머무른다
            }
            else
            {
                IsDirty = false; // "아니오" - 미저장 변경을 버리고 이동한다
            }

            masterTree.FocusedNode = targetNode; // 재귀 호출 - 원래 옮기려던 노드로 확정
            return;
        }

        _confirmedTreeNodes[masterTree] = targetNode;
        onRowSelected(row);
    }

    /// <summary>GridView 버전의 SuppressMasterRowSwitchConfirm과 같은 용도 - 재조회/저장 직후처럼
    /// 화면이 스스로 tree1을 다시 그리거나 포커스를 옮기는 구간을 감싼다.</summary>
    protected void SuppressMasterRowSwitchConfirm(TreeList masterTree, Action action)
    {
        _suppressMasterRowSwitchConfirm = true;
        try { action(); }
        finally
        {
            _suppressMasterRowSwitchConfirm = false;
            _confirmedTreeNodes[masterTree] = masterTree.FocusedNode;
        }
    }

    /// <summary>탭의 X버튼(DevExpress ClosePageButtonShowMode가 내부적으로 부르는 Close())을
    /// 포함해 이 폼이 닫히는 모든 경로가 여기를 거친다. ConfirmCloseAsync가 아직 확인 전이면
    /// 일단 닫기를 취소하고 물어본 뒤, 닫아도 된다는 결론이 나면 그때 다시 Close()를 부른다 -
    /// 그 재호출은 _closeConfirmed가 true라 이 핸들러를 다시 타도 곧바로 통과한다.
    ///
    /// [중요] CloseReason.MdiFormClosing(=MDI 부모인 ShellForm 자체가 닫히면서 그 여파로 이
    /// 자식이 같이 닫히는 경우)일 때는 여기서 절대 e.Cancel을 건드리지 않는다 - MDI 부모는
    /// 자식들을 닫아도 되는지 "동기적으로" 판단하는데, 여기서 한 번이라도 e.Cancel=true를
    /// 찍으면 그 뒤에 비동기로 확인을 마치고 다시 Close()를 불러 이 자식은 실제로 잘 닫혀도,
    /// 부모는 이미 "자식이 거부했다"고 보고 자기 자신의 종료 자체를 취소해버린다 - 그 결과
    /// 사용자가 "정말 종료하시겠습니까?"에 예를 눌러도 앱이 안 닫히고, 한 번 더 눌러야
    /// (그때는 자식이 이미 사라지고 없어서) 실제로 닫히는 증상으로 나타난다(실제로 겪음 - 화면을
    /// 하나도 안 띄워도, 홈 탭 하나만 있어도 재현됨). 이 경로에서는 ShellForm_FormClosing이
    /// Close()를 실제로 부르기 전에 모든 자식을 미리 ConfirmCloseAsync로 확인해두므로(그쪽 참고),
    /// 여기서는 안전하게 통과시키기만 하면 된다.</summary>
    private async void BaseForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_closeConfirmed) return;
        if (e.CloseReason == CloseReason.MdiFormClosing) return;

        e.Cancel = true;
        if (await ConfirmCloseAsync()) Close();
    }

    private async void BaseForm_Load(object? sender, EventArgs e)
    {
        ApplyMenuAuth();

        // 개발자 전용 - 컨트롤에 마우스오버하면 BindingField/팝업/룩업 정보를 툴팁으로 보여준다.
        // frmDept에서 화면마다 EnableBindingTooltips() 메서드를 손으로 만들던 방식(2026-08-31
        // 파일럿)을 여기 base 한 곳으로 옮겨서, 화면 코드에서는 컨트롤 선언 시 Tag에
        // BindingFieldTag 하나만 넣으면 자동 적용되게 했다(사장님 지시 - "모든 화면에 공통기능
        // 적용해줘"). 그리드/트리 컬럼은 FieldName이 이미 실제 DB 컬럼명이라 Tag 없이도
        // 자동으로 잡힌다.
        if (Session.IsDeveloper) ApplyBindingFieldTooltips(this);

        // 개인별 그리드 레이아웃(컬럼 순서/숨김/폭) 복원 + 우클릭 "레이아웃저장/초기화" 연결 -
        // 모든 사용자 대상(개발자 전용 아님). 화면 코드에서 그리드마다 따로 부를 필요 없이
        // BaseForm 한 곳에서 화면 안의 GridViewWyn을 전부 찾아 처리한다.
        await ApplyGridLayoutsAsync(this);

        // 화면 안의 탭(TabControlWyn/XtraTabControl)은 항상 맨 앞 탭이 선택된 채로 열려야 한다
        // (2026-09-08, 사장님 지시 - "탭을 사용하는 모든 화면은 최초 오픈시 제일 앞쪽에 있는 탭이
        // 선택되어 있도록"). VS 디자이너는 마지막으로 편집하던 탭을 SelectedTabPage로 그대로
        // 저장해버려서(예: 개발 중 두 번째 탭을 보다가 저장하면 그 탭이 초기 선택값이 됨) 화면마다
        // 실수로 뒷탭이 열린 채 배포되는 사고가 반복됐다 - Designer.cs를 손으로 고치는 대신 여기
        // 한 곳에서 강제로 되돌려 앞으로 디자이너가 다시 틀어놔도 항상 맞다.
        ResetTabsToFirstPage(this);
    }

    private static void ResetTabsToFirstPage(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (child is XtraTabControl tab && tab.TabPages.Count > 0) tab.SelectedTabPageIndex = 0;
            ResetTabsToFirstPage(child);
        }
    }

    /// <summary>
    /// container 아래 모든 GridViewWyn(순정 GridView는 대상 아님 - Role/RowAdd처럼 이것도
    /// GridViewWyn 전용 기능이다)에 대해: (1) 지금(디자이너 원본) 배치를 스냅샷으로 기억해두고
    /// (레이아웃초기화용), (2) 저장 요청/초기화 요청 이벤트를 구독하고, (3) 이 사용자가 예전에
    /// 저장해둔 레이아웃이 있으면 복원한다.
    ///
    /// 스냅샷을 먼저 찍고 나서 저장된 값을 복원하는 순서가 중요하다 - 반대로 하면 스냅샷 자체가
    /// "사용자가 저장해둔 배치"가 되어버려 초기화가 무의미해진다.
    /// </summary>
    private async Task ApplyGridLayoutsAsync(Control root)
    {
        var views = new List<GridViewWyn>();
        CollectGridViews(root, views);
        if (views.Count == 0) return;

        foreach (var view in views)
        {
            view.CapturePristineLayout();
            view.LayoutSaveRequested += async (s, e) => await SaveGridLayoutAsync(view);
            view.LayoutResetRequested += async (s, e) => await ResetGridLayoutAsync(view);
        }

        if (MenuId <= 0) return;

        // 전용 컨트롤러(api/grid-layout)를 쓴다 - 범용 데이터 통로(QueryAsync -> api/data/query)는
        // "그 메뉴에 등록된 PROC_PREFIX로 시작하는 프로시저만" 허용하는데, 이 기능은 특정 화면
        // 소유 데이터가 아니라 로그인한 사용자면 어느 화면에서든 써야 해서 그 제약과 안 맞는다
        // (실제로 기초코드등록에서 "이 메뉴에서 사용할 수 없는 프로시저입니다"로 막혔던 문제 -
        // GridLayoutController 클래스 설명 참고).
        //
        // 저장된 배치를 불러오는 건 있으면 좋고 없어도 그만인 부가 기능이라, 여기서 실패해도
        // 화면 열기 자체를 막으면 안 된다 - 이 화면 Load 흐름 안에서 처리 안 하면 Program.cs의
        // 전역 ThreadException 핸들러까지 올라가 "예상치 못한 오류" 팝업이 뜬다(실제로는 그냥
        // 컬럼 배치가 디자이너 기본값으로 남는 것뿐인데 사용자에게는 화면이 고장난 것처럼 보임).
        // 조용히 건너뛰고 디자이너 기본 배치(CapturePristineLayout으로 이미 잡아둔 상태)로 연다.
        try
        {
            var saved = await ApiClient.GetAsync<List<GridLayoutItemDto>>($"api/grid-layout?menuId={MenuId}") ?? new();
            foreach (var item in saved)
            {
                var view = views.FirstOrDefault(v => v.Name == item.GridKey);
                if (view != null && !string.IsNullOrEmpty(item.LayoutXml)) view.RestoreLayoutXml(item.LayoutXml);
            }
        }
        catch
        {
            // 무시 - 위 설명 참고.
        }
    }

    private static void CollectGridViews(Control root, List<GridViewWyn> result)
    {
        foreach (Control child in root.Controls)
        {
            if (child is GridControl grid && grid.MainView is GridViewWyn view) result.Add(view);
            CollectGridViews(child, result);
        }
    }

    private async Task SaveGridLayoutAsync(GridViewWyn view)
    {
        if (MenuId <= 0) return; // MenuId 없는 화면은 저장 위치를 특정할 수 없다

        var result = await ApiClient.PutAsync<SaveGridLayoutRequest, ApiResult>("api/grid-layout", new SaveGridLayoutRequest
        {
            MenuId = MenuId,
            GridKey = view.Name,
            LayoutXml = view.SaveLayoutXml()
        });

        if (result?.Success == true) Toast.Show("현재 컬럼 배치를 저장했습니다.");
        else AppMessageBox.Show(result?.Message ?? "레이아웃 저장에 실패했습니다.", "저장 실패");
    }

    private async Task ResetGridLayoutAsync(GridViewWyn view)
    {
        view.RestorePristineLayout();
        if (MenuId <= 0) return; // 저장된 적이 없으니 지울 것도 없다

        var result = await ApiClient.DeleteAsync<ApiResult>(
            $"api/grid-layout?menuId={MenuId}&gridKey={Uri.EscapeDataString(view.Name)}");

        if (result?.Success == true) Toast.Show("기본 배치로 초기화했습니다.");
        else AppMessageBox.Show(result?.Message ?? "레이아웃 초기화에 실패했습니다.", "초기화 실패");
    }

    /// <summary>
    /// 개발자(Session.IsDeveloper) 전용 - 화면의 모든 컨트롤에 마우스를 올리면 그 컨트롤이 어느
    /// DB 컬럼에 바인딩됐는지(BindingField) + 팝업/룩업이 걸려있으면 어떤 걸 쓰는지 툴팁으로
    /// 보여준다.
    ///
    /// - 그리드(GridControl.MainView)/트리(TreeList) 컬럼: FieldName이 이미 실제 DB 컬럼명이라
    ///   손댈 것 없이 전 화면에 자동 적용된다.
    /// - 개별 입력 컨트롤(txtDeptCd 등): 컨트롤 선언 시 Tag에 BindingFieldTag를 미리 넣어둔
    ///   것만 잡는다(예: new TextEditWyn { Tag = new BindingFieldTag("dept_cd") }). Tag를 안
    ///   채운 컨트롤은 그냥 건드리지 않는다(툴팁 없음).
    ///
    /// Control.Tag를 그냥 string으로 쓰지 않고 BindingFieldTag(전용 래퍼 클래스)로 감싸는
    /// 이유: frmShortcut.cs가 이미 Tag를 다른 용도(actionCd 저장)로 쓰고 있어서, 순수 string
    /// 체크만으로는 그 값까지 "BindingField"로 잘못 표시할 위험이 있다 - 타입으로 구분하면
    /// 이 화면들끼리 절대 안 섞인다.
    /// </summary>
    private static void ApplyBindingFieldTooltips(Control root)
    {
        foreach (Control child in root.Controls)
        {
            if (child is BaseEdit edit && edit.Tag is BindingFieldTag tag)
            {
                var tip = $"BindingField : {tag.Field}";
                if (edit is PopupLookupEditWyn pop && !string.IsNullOrWhiteSpace(pop.LookupKey))
                    tip += $"\nPopup : {pop.LookupKey}";
                else if (edit is LookUpEditWyn look)
                {
                    if (!string.IsNullOrWhiteSpace(look.LookupKey)) tip += $"\nLookUp : {look.LookupKey}";
                    else if (!string.IsNullOrWhiteSpace(look.ProcName)) tip += $"\nLookUp(Proc) : {look.ProcName}";
                }
                edit.ToolTip = tip;
            }
            else if (child is GridControl grid && grid.MainView is GridView gv)
            {
                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv.Columns)
                {
                    if (!string.IsNullOrEmpty(col.ToolTip)) continue;

                    // 보통은 FieldName이 이미 실제 DB 컬럼명이라(DataTable 바인딩) 그대로 쓴다.
                    // List<T> 바인딩 그리드(frmUserAuth 등)는 FieldName이 C# 프로퍼티명(PascalCase,
                    // 예: UserNm)이라 같은 필드를 가리키는 panData 쪽 BindingFieldTag(DB 컬럼명,
                    // 예: USER_NM)와 표기가 어긋난다 - 그런 컬럼은 col.Tag에 BindingFieldTag를
                    // 명시적으로 얹어두면(panData 컨트롤과 같은 방식) 그걸 우선한다(2026-09-03,
                    // 사용자권한관리에서 실제로 발견된 불일치).
                    if (col.Tag is BindingFieldTag colTag) col.ToolTip = $"BindingField : {colTag.Field}";
                    else if (!string.IsNullOrWhiteSpace(col.FieldName)) col.ToolTip = $"BindingField : {col.FieldName}";
                }
            }
            else if (child is TreeList tree)
            {
                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn col in tree.Columns)
                    if (string.IsNullOrEmpty(col.ToolTip) && !string.IsNullOrWhiteSpace(col.FieldName))
                        col.ToolTip = $"BindingField : {col.FieldName}";
            }

            ApplyBindingFieldTooltips(child); // 재귀 - 패널 안에 중첩된 컨트롤까지 전부 훑는다
        }
    }

    /// <summary>
    /// 로그인 세션에 캐싱된 메뉴권한(MenuDto)을 조회하여
    /// 등록/수정/삭제/엑셀 버튼의 Enabled를 일괄 반영한다.
    /// 실제 구현은 SessionManager(사용자 세션 캐시) 완성 후 채운다.
    /// </summary>
    protected virtual void ApplyMenuAuth()
    {
        if (MenuId <= 0) return;

        var auth = SessionManager.Current.GetMenuAuth(MenuId);
        CanInsert = auth?.InsertYn ?? false;
        CanUpdate = auth?.UpdateYn ?? false;
        CanDelete = auth?.DeleteYn ?? false;
        CanPrint = auth?.PrintYn ?? false;
        CanExcel = auth?.ExcelYn ?? false;
        Auth = auth?.Auth ?? new bool[10];
    }

    /// <summary>
    /// 모든 업무화면 공통 타이틀 바 - 폴더 아이콘 + 화면명(Text) + 화면 식별자(대괄호). 예전엔
    /// MenuCd 문자열("[SM_MENU]")을 그대로 보여줬는데, MENU_ID(정수)로 바뀌면서 그 자체는 사람이
    /// 읽고 알아볼 수 없어 SessionManager 세션 메뉴 목록에서 Module.ScreenClassNm("[SM.frmMenu]")을
    /// 찾아 대신 보여준다 - 개발/문의 시 "이 화면이 뭔지" 식별하는 용도는 그대로 유지된다.
    /// 화면마다 제목 영역을 제각각 만들지 않고 이 메서드 하나로 통일해서, 어떤 화면을 열어도
    /// 같은 위치/스타일로 "지금 보고 있는 화면이 뭔지" 바로 알 수 있게 한다.
    /// 반드시 다른 Dock=Top 패널(조회조건 등)보다 나중에 Controls.Add 해야 맨 위를 차지한다.
    /// </summary>
    protected Panel BuildScreenHeader()
    {
        // 흰 배경(회색 배경이면 아래 본문과 색이 끊겨 보인다는 피드백)으로 본문과 자연스럽게
        // 이어지게 하고, 아래쪽 얇은 선 하나로만 구분한다.
        var header = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.White };
        var bottomBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(228, 229, 232) };
        header.Controls.Add(bottomBorder);

        var icon = new PictureBox
        {
            Image = ScreenIcon,
            SizeMode = PictureBoxSizeMode.CenterImage,
            Location = new Point(14, 4),
            Size = new Size(20, 20),
            BackColor = Color.Transparent
        };

        // AutoSizeMode.Default로 실제 텍스트 길이만큼만 폭을 차지하게 해서, 화면명이 짧을 때
        // (예: "메뉴관리") 코드 라벨([SM_MENU])이 옆에 붙지 않고 멀리 떨어져 보이던 문제를 없앤다.
        var lblTitle = new LabelControl
        {
            Text = Text,
            Location = new Point(38, 6),
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblTitle.Appearance.Font = AppFonts.BodyBold;
        lblTitle.Appearance.ForeColor = Color.FromArgb(55, 55, 55);

        var screenKey = SessionManager.Current.GetMenuAuth(MenuId) is { } menu && !string.IsNullOrEmpty(menu.ScreenClassNm)
            ? $"{menu.Module}.{menu.ScreenClassNm}"
            : null;
        var lblCode = new LabelControl
        {
            Text = string.IsNullOrEmpty(screenKey) ? string.Empty : $"[{screenKey}]",
            AutoSizeMode = LabelAutoSizeMode.Default
        };
        lblCode.Appearance.Font = AppFonts.Caption;
        lblCode.Appearance.ForeColor = Color.FromArgb(150, 150, 150);

        header.Controls.Add(icon);
        header.Controls.Add(lblTitle);
        header.Controls.Add(lblCode);
        void PositionCode() => lblCode.Location = new Point(lblTitle.Right + 8, 8);
        header.Layout += (s, e) => PositionCode();
        PositionCode();

        return header;
    }

    /// <summary>
    /// 공통 예외처리 - 업무화면에서 try/catch 없이 이 메서드로 감싸서 호출.
    /// 로그 적재 + 사용자에게 표준화된 에러 메시지 표시.
    /// </summary>
    protected void SafeExecute(Action action, string actionNm = "")
    {
        try
        {
            ShowBusy();
            action();
        }
        catch (Exception ex)
        {
            // TODO: 공통 로거(Serilog 등) 연동
            AppMessageBox.Show($"[{actionNm}] 처리 중 오류가 발생했습니다.\n{ex.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            HideBusy();
        }
    }

    /// <summary>
    /// API 호출 등 비동기 작업용 SafeExecute. 조회/저장/삭제 버튼 클릭 핸들러에서 주로 사용.
    /// </summary>
    protected async Task SafeExecuteAsync(Func<Task> action, string actionNm = "")
    {
        try
        {
            ShowBusy();
            await action();
        }
        catch (Exception ex)
        {
            AppMessageBox.Show($"[{actionNm}] 처리 중 오류가 발생했습니다.\n{ex.Message}",
                "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            HideBusy();
        }
    }

    private SpinnerControl? _busySpinner;
    private System.Windows.Forms.Timer? _busyDelayTimer;
    private int _busyDepth;

    /// <summary>
    /// 오버레이를 띄우기 전에 기다리는 시간. 사내망에서 조회는 보통 100ms도 안 걸리는데,
    /// 그때마다 불투명 오버레이가 화면을 덮었다 걷히면 "로딩 표시"가 아니라 폼 전체가
    /// 번쩍이는 것으로 보인다(실제로 겪음 - 기초코드등록 조회). 이 시간 안에 끝나는 작업은
    /// 오버레이를 아예 띄우지 않아서 깜빡임이 없고, 그보다 오래 걸리는 작업에만 떠서
    /// 원래 의도(작업 중임을 확실히 알리기)를 그대로 살린다.
    /// </summary>
    private const int BusyDelayMs = 300;

    /// <summary>
    /// 화면 내용(그리드/입력영역) 위에 덮이는 오버레이 + 회전 스피너를 띄운다. 단, 곧바로
    /// 띄우지 않고 BusyDelayMs만큼 기다렸다가 띄운다(그 설명 참고).
    /// 예전엔 이 자리에서 전체 창 커서를 Cursors.WaitCursor로 바꿔서 "로딩 중"을 표시했는데,
    /// 커서 모양 변화만으로는 눈에 잘 안 띈다는 피드백에 따라 실제로 화면에 보이는 오버레이로
    /// 바꿨다. Shell 상단 툴바(ShellForm.AddIconBadgeButton)와 SafeExecute/SafeExecuteAsync
    /// 양쪽에서 공통으로 호출한다.
    ///
    /// 중첩 호출(예: 툴바가 ShowBusy를 부르고 그 안의 로직이 SafeExecuteAsync로 또 부르는 경우)에
    /// 대비해 깊이를 센다 - 안쪽 작업이 끝났다고 바깥 작업이 아직인데 오버레이가 걷히면 안 된다.
    /// </summary>
    public void ShowBusy()
    {
        _busyDepth++;
        if (_busyDepth > 1) return; // 이미 대기 중이거나 표시 중

        _busyForm = this;
        StartBusyTimer();
    }

    public void HideBusy()
    {
        if (_busyDepth > 0) _busyDepth--;
        if (_busyDepth > 0) return; // 바깥 작업이 아직 진행 중

        _busyDelayTimer?.Stop(); // 아직 안 떴으면 영영 안 뜨게 - 이게 깜빡임을 없애는 핵심
        if (_busySpinner != null) _busySpinner.Visible = false;
        if (ReferenceEquals(_busyForm, this)) _busyForm = null;
    }

    private void StartBusyTimer()
    {
        if (_busyDelayTimer == null)
        {
            _busyDelayTimer = new System.Windows.Forms.Timer { Interval = BusyDelayMs };
            _busyDelayTimer.Tick += (s, e) =>
            {
                _busyDelayTimer!.Stop();
                ShowBusyOverlayNow();
            };
        }

        _busyDelayTimer.Start();
    }

    /// <summary>지금 오버레이를 띄웠거나 띄우려고 대기 중인 폼. 모달 대화상자가 뜨는 동안
    /// 그 폼의 오버레이를 잠시 걷기 위해 정적으로 들고 있는다(SuspendBusyForModal 참고).</summary>
    private static BaseForm? _busyForm;

    /// <summary>
    /// 모달 대화상자(AppMessageBox 등)가 뜨는 동안 busy 오버레이를 잠시 걷는다.
    ///
    /// 툴바(ShellForm.AddIconBadgeButton)는 액션 전체를 ShowBusy/HideBusy로 감싸는데, 그 액션이
    /// 중간에 "삭제하시겠습니까?" 같은 확인창을 띄우면 사용자가 답할 때까지 계속 "작업 중"
    /// 상태다. 그러면 지연 시간이 지나 오버레이가 올라와서, 확인창 뒤 화면이 통째로 회색으로
    /// 덮여버린다(실제로 겪음 - 대분류 삭제 확인창). 사용자를 기다리는 시간은 작업 중이 아니므로
    /// 그동안은 걷어두고, 대화상자가 닫힌 뒤 실제 작업이 이어질 때 다시 지연 타이머를 건다.
    /// </summary>
    internal static void SuspendBusyForModal()
    {
        var form = _busyForm;
        if (form == null) return;

        form._busyDelayTimer?.Stop();
        if (form._busySpinner != null) form._busySpinner.Visible = false;
    }

    /// <summary>모달이 닫힌 뒤 호출 - 아직 작업이 끝나지 않았다면 지연 타이머를 처음부터 다시 건다.</summary>
    internal static void ResumeBusyAfterModal()
    {
        var form = _busyForm;
        if (form == null || form._busyDepth <= 0 || form.IsDisposed) return;

        form.StartBusyTimer();
    }

    /// <summary>예전엔 화면 전체를 옅은 회색 오버레이로 덮고 그 위에 스피너를 얹었는데, 화면이
    /// 통째로 안 보이는 게 불편하다는 지적(2026-09-15, "로딩이 걸리더라도 화면은 그대로 살아있었
    /// 으면 좋겠어" - 게다가 드물게 오버레이가 안 걷히고 그대로 남는 증상도 있었음, 전체화면을
    /// 덮는 오버레이라 그 증상이 특히 눈에 띄었다)으로, 화면을 덮지 않고 우측 하단에 작은
    /// 스피너만 띄우는 방식으로 바꿨다 - 로딩 중에도 화면 내용이 그대로 보이고 조작도 막지
    /// 않는다.</summary>
    private void ShowBusyOverlayNow()
    {
        if (_busySpinner == null)
        {
            _busySpinner = new SpinnerControl { SpinnerColor = Color.FromArgb(41, 121, 255) };
            Controls.Add(_busySpinner);
            Resize += (s, e) => PositionBusySpinner();
        }

        PositionBusySpinner();
        _busySpinner.Visible = true;
        _busySpinner.BringToFront();
    }

    private void PositionBusySpinner()
    {
        if (_busySpinner == null) return;
        const int margin = 16;
        _busySpinner.Location = new Point(ClientSize.Width - _busySpinner.Width - margin, ClientSize.Height - _busySpinner.Height - margin);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // 폼에 Controls로 붙지 않은 컴포넌트라 자동으로 정리되지 않는다 - 직접 끊어준다.
            _busyDelayTimer?.Stop();
            _busyDelayTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}
