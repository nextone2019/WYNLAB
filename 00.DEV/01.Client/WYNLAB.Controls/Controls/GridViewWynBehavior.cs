using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.DPI;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace WYNLAB.Base.Controls;

/// <summary>
/// GridViewWyn/BandedGridViewWyn이 공유하는 실제 동작. DevExpress의 GridView와 BandedGridView는
/// 둘 다 GridView를 상속하는 "형제" 관계라서(BandedGridView : GridView), C#은 다중상속을
/// 지원하지 않으니 GridViewWyn을 BandedGridViewWyn이 그대로 물려받을 수 없다. 그래서 실제
/// 로직은 여기(GridView 타입 하나만 알면 되는 공용 클래스)에 한 번만 두고, 두 Wyn 클래스는
/// 각자 생성자에서 이 클래스를 인스턴스화해서 위임만 한다 - 로직을 두 곳에 복붙하면 한쪽만
/// 고치고 다른 쪽을 깜빡하는 사고가 나기 쉬워서 이렇게 분리했다.
/// </summary>
internal sealed class GridViewWynBehavior
{
    private readonly GridView _view;
    private readonly HashSet<(int RowHandle, string FieldName)> _dirtyCells = new();

    /// <summary>붙여넣기 한 번(Ctrl+V 한 번)에 대한 실행취소 정보 - 여러 단계 Undo 스택이 아니라
    /// "방금 그 한 번"만 되돌리는 단순한 스냅샷이다. 새 붙여넣기가 일어나면 덮어써지고, 저장
    /// 성공(ClearDirtyMarks) 후에도 더 이상 의미가 없어서 비운다.</summary>
    private sealed class PasteUndoState
    {
        public List<(int RowHandle, GridColumn Column, object? OriginalValue)> Overwrites { get; }
        public List<int> AddedRowHandles { get; }

        public PasteUndoState(List<(int RowHandle, GridColumn Column, object? OriginalValue)> overwrites, List<int> addedRowHandles)
        {
            Overwrites = overwrites;
            AddedRowHandles = addedRowHandles;
        }
    }

    private PasteUndoState? _lastPasteUndo;

    public bool ShowRowNumbers { get; set; }
    public string EmptyText { get; set; } = "조회된 데이터가 없습니다.";
    public bool HighlightUnsavedCells { get; set; }
    public bool HighlightFocusedRow { get; set; }

    private GridRoleWyn _role = GridRoleWyn.Query;
    public GridRoleWyn Role
    {
        get => _role;
        set { _role = value; ApplyRole(); }
    }

    // 개인별 그리드 레이아웃(컬럼 순서/숨김/폭) 저장 - 실제 DB 저장/조회는 BaseForm이 한다
    // (이 프로젝트(WYNLAB.Controls)는 WYNLAB.BaseForm보다 아래 계층이라 Session/ApiClient를
    // 몰라야 한다 - WYNLAB.BaseForm.csproj가 WYNLAB.Controls.csproj를 참조하는 방향이지 반대가
    // 아니다). 여기서는 순수 DevExpress 직렬화만 담당하고, "저장해줘"/"초기화해줘"는 이벤트로
    // 위로 올려보낸다 - BaseForm이 화면의 모든 GridViewWyn을 찾아 구독한다.
    public event EventHandler? LayoutSaveRequested;
    public event EventHandler? LayoutResetRequested;

    private byte[]? _pristineLayout;

    /// <summary>지금 상태를 "디자이너 원본"으로 기억해둔다 - BaseForm이 화면 Load 시, 저장된
    /// 레이아웃을 복원하기 전에 반드시 먼저 호출해야 한다. "레이아웃초기화"가 재시작 없이 이
    /// 스냅샷으로 즉시 되돌리는 데 쓰인다.</summary>
    public void CapturePristineLayout()
    {
        using var ms = new MemoryStream();
        _view.SaveLayoutToStream(ms);
        _pristineLayout = ms.ToArray();
    }

    public void RestorePristineLayout()
    {
        if (_pristineLayout == null) return;
        using var ms = new MemoryStream(_pristineLayout);
        _view.RestoreLayoutFromStream(ms);
    }

    public string SaveLayoutXml()
    {
        using var ms = new MemoryStream();
        _view.SaveLayoutToStream(ms);
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    /// <summary>배포 사이 컬럼 구성 자체가 바뀌었어도(컬럼 추가/삭제) 절대 예외로 화면을 못 열게
    /// 하면 안 된다 - OptionsLayout.Columns.RemoveOldColumns/AddNewColumns가 대부분 조용히
    /// 처리해주지만, 그것만 믿지 않고 여기서도 한 번 더 막는다(실패하면 그냥 지금 상태 유지 -
    /// CapturePristineLayout으로 이미 잡아둔 원본이 있으니 최악의 경우도 디자이너 기본 배치).</summary>
    public void RestoreLayoutXml(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml)) return;
        try
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            _view.RestoreLayoutFromStream(ms);
        }
        catch
        {
            // 저장된 레이아웃을 못 쓰게 됐다는 뜻 - 조용히 무시하고 지금(디자이너 원본) 배치를 유지한다.
        }
    }

    public GridViewWynBehavior(GridView view)
    {
        _view = view;

        // 그리드 컬럼헤더/행 글꼴을 명시하지 않으면 DevExpress 스킨이 정한 기본 글꼴(Tahoma
        // 계열)이 그대로 쓰이는데, 좌측 메뉴트리는 AppFonts(맑은 고딕)를 명시적으로
        // 쓰고 있어서 화면 안에서 메뉴와 그리드의 글꼴이 서로 다르게 보였다(실제로 겪음) -
        // 두 글꼴 다 9pt라 크기는 같지만 서체가 달라 눈에 띄었다. 메뉴와 통일하도록 맞춘다.
        _view.Appearance.HeaderPanel.Font = AppFonts.Body;
        _view.Appearance.Row.Font = AppFonts.Body;

        // 헤더행도 데이터행과 완전히 같은 흰 배경이면 구분이 안 돼서 밋밋해 보이고, 반대로
        // 스킨 기본 헤더색은 카드/그리드 톤과 안 어울렸다 - 아주 옅은 회색으로 살짝만 구분한다.
        _view.Appearance.HeaderPanel.BackColor = UiTheme.GridHeaderBackColor;
        _view.Appearance.HeaderPanel.Options.UseBackColor = true;
        _view.Appearance.HeaderPanel.ForeColor = UiTheme.GridHeaderForeColor;
        _view.Appearance.HeaderPanel.Options.UseForeColor = true;

        // 복사는 조회 화면 포함 항상 허용 - 붙여넣기 가능 여부는 ApplyRole()이 Role에 맞춰 정한다.
        _view.OptionsClipboard.AllowCopy = DefaultBoolean.True;

        _view.OptionsView.ShowAutoFilterRow = false; // 기본은 꺼둠 - 필요한 화면에서만
                                                       // view.OptionsView.ShowAutoFilterRow = true로 켠다.
        _view.OptionsView.ShowGroupPanel = false; // "Drag a column header here" 그룹 영역 - WYNLAB
                                                    // 화면 대부분은 그룹핑을 안 써서 기본은 숨김.
                                                    // 실제 그룹핑이 필요하면 AddGroupSummary()가
                                                    // 호출 시점에 다시 켜준다.
        _view.OptionsSelection.MultiSelect = true;
        _view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
        _view.OptionsNavigation.EnterMoveNextColumn = true;

        // 기본 EditorShowMode(Default = 사실상 MouseUpFocused)에서는 LookUp/Popup처럼 버튼이
        // 있는 편집기가 붙은 컬럼을 셀 하나 고치는 데 클릭이 3번 정도 필요하다 - 1) 행에 포커스,
        // 2) 셀 편집모드 진입, 3) 그제서야 드롭다운/팝업이 열림(2026-09-08 실제 지적 - AI Builder
        // Control컬럼의 LookUp에서 겪음). MouseDown으로 바꾸면 셀을 처음 클릭하는 그 순간 바로
        // 편집기가 열리고, 그 클릭 자체가 편집기(드롭다운 버튼 등)에도 그대로 전달돼서 대부분
        // 한 번의 클릭으로 값이 바뀐다. 화면 하나만의 문제가 아니라 이 프로젝트의 모든 그리드
        // 편집컬럼(COMBO/POP/CHECK/NUMBER)에 똑같이 해당하는 문제라 공용 기본값에서 고친다.
        _view.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;

        // 헤더 클릭 정렬 - 모든 그리드 공통(GridSortSupport 참고: 뷰/컬럼 정렬 명시, 룩업 컬럼은 표시 이름 기준, 나중에 추가되는 컬럼 포함).
        GridSortSupport.Enable(_view);
        GridDateSupport.Enable(_view);

        // 룩업 컬럼(ColumnEdit이 RepositoryItemLookUpEdit 계열 - 예: LookUpColumnEdit)은 기본
        // ShowButtonMode(Default, 사실상 포커스된 셀에서만 드롭다운 삼각형이 보임)라서, 그리드를
        // 훑어볼 때는 어떤 컬럼이 룩업인지 구분이 안 됐다(2026-09-16 요청 - "룩업들은 오른쪽에
        // 세모(룩업인지 표시나게) 보여줘"). ShowAlways로 바꾸면 포커스 여부와 상관없이 컬럼
        // 전체에 항상 삼각형이 보인다 - 화면마다 따로 처리할 필요 없이 공용 기본값에서 고친다.
        foreach (GridColumn column in _view.Columns)
        {
            if (column.ColumnEdit is RepositoryItemLookUpEdit)
                column.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
        }

        // 개인별 "레이아웃저장"(컬럼 순서/숨김/폭)이 정렬/그룹/필터/서식까지 같이 저장해버리면
        // "어제 걸어둔 조건 때문에 오늘 데이터가 안 보인다" 같은 혼란이 생긴다(사장님 지시로
        // 정렬/그룹/필터는 제외) - 저장 대상을 컬럼 배치 하나로만 좁혀둔다.
        // 그리드에서 여러 칸을 Ctrl+C 하면 DevExpress 기본값(CopyColumnHeaders=Default)이 컬럼 캡션을 첫 줄로 같이 복사한다 -
        // 그대로 붙여넣으면 "안전재고" 같은 글자가 숫자 칸 첫 줄로 들어가 문제가 된다(2026-10-05). 값만 복사한다.
        _view.OptionsClipboard.CopyColumnHeaders = DefaultBoolean.False;

        _view.OptionsLayout.StoreAppearance = false;
        _view.OptionsLayout.StoreFormatRules = false;
        _view.OptionsLayout.StoreDataSettings = false; // 정렬/그룹/필터/요약이 여기 묶여있다
        _view.OptionsLayout.Columns.StoreAppearance = false;
        // 배포 후 컬럼이 추가/삭제되어도(예: remark 컬럼을 나중에 뺌) 옛날에 저장된 레이아웃을
        // 복원할 때 없는 컬럼은 조용히 무시하고, 새로 생긴 컬럼은 저장된 적 없으니 그냥
        // 디자이너가 정한 자리에 나온다 - RestoreLayoutXml의 try/catch와 함께 이중 안전장치.
        _view.OptionsLayout.Columns.RemoveOldColumns = true;
        _view.OptionsLayout.Columns.AddNewColumns = true;

        _view.PopupMenuShowing += OnPopupMenuShowing;
        _view.CustomDrawRowIndicator += OnCustomDrawRowIndicator;
        _view.CustomDrawEmptyForeground += OnCustomDrawEmptyForeground;
        _view.CellValueChanged += OnCellValueChanged;
        _view.RowCellStyle += OnRowCellStyle;
        _view.MouseDown += OnMouseDown;
        _view.KeyDown += OnKeyDownToggleCheck;
        _view.ShowingEditor += OnShowingEditor;
        _view.ShownEditor += OnShownEditorImeGuard;
        _view.ValidatingEditor += OnValidatingEditorImeGuard;
        _view.EndSorting += OnEndSorting;
        _view.InitNewRow += OnInitNewRow;
    }

    /// <summary>새 행이 생기면(행추가/멀티행추가/그리드 자체 새 행 버튼 등 어떤 경로든 DevExpress가
    /// 공통으로 쏘는 InitNewRow) 체크박스 컬럼(ColumnEdit이 RepositoryItemCheckEdit)의 기본값은
    /// null이라 체크/해제 어느 쪽도 아닌 "네모점" 모양으로 보였다(2026-09-16 지적 - frmItemMulti
    /// 공정검사 등 새 행에서 실제로 겪음). ValueUnchecked로 미리 채워서 평범한 빈 체크박스로
    /// 보이게 한다 - OnMouseDown과 같은 이유로 "N"을 하드코딩하지 않고 그 리포지토리 아이템에
    /// 실제 설정된 값을 그대로 쓴다. GridViewWyn/BandedGridViewWyn을 쓰는 모든 화면의 체크박스
    /// 컬럼에 공통 적용된다(화면마다 따로 처리할 필요 없음).</summary>
    private void OnInitNewRow(object? sender, InitNewRowEventArgs e)
    {
        foreach (GridColumn column in _view.Columns)
        {
            if (column.ColumnEdit is RepositoryItemCheckEdit checkEdit)
                _view.SetRowCellValue(e.RowHandle, column, checkEdit.ValueUnchecked);
        }
    }

    /// <summary>컬럼 헤더 클릭 정렬은 DevExpress 기본 동작이라 따로 켤 게 없다(OptionsBehavior.AllowSort
    /// 기본값이 이미 true). 다만 정렬 후 포커스가 정렬 전 그 행을 계속 따라가는 게 기본 동작이라
    /// (예: 3번째로 보이던 행이 정렬 후 7번째로 밀려도 포커스는 그 행을 계속 따라감), 정렬을
    /// 새로 조회한 것처럼 항상 첫 행으로 되돌린다 - EndSorting은 헤더 클릭이든 코드로
    /// SortOrder를 바꾸든 정렬이 실제로 끝났을 때 공통으로 불린다. GridViewWyn/BandedGridViewWyn을
    /// 쓰는 모든 화면에 공통 적용된다.</summary>
    private void OnEndSorting(object? sender, EventArgs e)
    {
        if (_view.RowCount > 0) _view.FocusedRowHandle = _view.GetRowHandle(0);
    }

    /// <summary>
    /// 체크박스 컬럼(ColumnEdit이 RepositoryItemCheckEdit)은 DevExpress 기본 동작으로는 클릭이
    /// 두 번 필요하다 - 첫 클릭은 셀에 포커스/편집기를 여는 것뿐이고, 값 자체를 토글하는 건
    /// 편집기가 이미 열려있는 두 번째 클릭부터다(실제로 겪음 - 체크했다고 생각했는데 저장해보면
    /// 반영이 안 됨. 편집기가 아직 안 열려서 첫 클릭이 토글로 이어지지 않았던 것). 그래서 편집기
    /// 진입을 기다리지 않고 MouseDown에서 곧바로 셀 값을 읽어 반대값으로 써서 한 번 클릭에
    /// 토글되게 한다 - GridViewWyn/BandedGridViewWyn을 쓰는 모든 화면의 체크박스 컬럼에 공통
    /// 적용된다(화면마다 따로 처리할 필요 없음).
    ///
    /// ValueChecked/ValueUnchecked를 하드코딩하지 않고 그 리포지토리 아이템에 실제 설정된 값을
    /// 그대로 쓴다 - 이 코드베이스는 보통 "Y"/"N"을 쓰지만(예: frmMinorCode의 사용여부), 값
    /// 자체를 강제하면 다른 값 규약을 쓰는 화면이 생겼을 때 깨진다.
    /// </summary>
    private void OnMouseDown(object? sender, MouseEventArgs e)
    {
        var hitInfo = _view.CalcHitInfo(e.Location);
        if (!hitInfo.InRowCell) return;

        if (hitInfo.Column?.ColumnEdit is RepositoryItemCheckEdit checkEdit)
        {
            var current = _view.GetRowCellValue(hitInfo.RowHandle, hitInfo.Column);
            var isChecked = Equals(current, checkEdit.ValueChecked);
            _view.SetRowCellValue(hitInfo.RowHandle, hitInfo.Column, isChecked ? checkEdit.ValueUnchecked : checkEdit.ValueChecked);
            _view.FocusedRowHandle = hitInfo.RowHandle;
            return;
        }

        PopupDebugLog.Write($"MouseDown: Clicks={e.Clicks}, col={hitInfo.Column?.FieldName}, edit={hitInfo.Column?.ColumnEdit?.GetType().Name}");
    }

    /// <summary>체크박스 컬럼은 편집기를 열지 않아(OnShowingEditor) DevExpress 기본 스페이스바 토글도 같이
    /// 죽는다 - 포커스된 체크박스 셀에서 스페이스바를 누르면 OnMouseDown과 같은 방식으로 값을 뒤집는다
    /// (2026-10-05). 읽기전용 그리드/컬럼은 건드리지 않는다.</summary>
    private void OnKeyDownToggleCheck(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space || e.Modifiers != Keys.None) return;
        if (_view.IsEditing || _view.FocusedRowHandle < 0) return;
        if (_view.FocusedColumn is not { } column || column.ColumnEdit is not RepositoryItemCheckEdit checkEdit) return;
        if (!_view.OptionsBehavior.Editable || column.OptionsColumn.ReadOnly) return;

        var current = _view.GetRowCellValue(_view.FocusedRowHandle, column);
        var isChecked = Equals(current, checkEdit.ValueChecked);
        _view.SetRowCellValue(_view.FocusedRowHandle, column, isChecked ? checkEdit.ValueUnchecked : checkEdit.ValueChecked);
        e.Handled = true;
    }

    /// <summary>체크박스 컬럼은 실제 편집기(라이브 체크박스 컨트롤)를 아예 열지 않는다 - OnMouseDown이
    /// 이미 셀 값을 직접 뒤집어주는데, 그 뒤에 편집기까지 열리면 같은 클릭이 편집기 내부 체크박스에도
    /// 전달되어 한 번 더 토글돼버린다(우리 토글 + 편집기 자체 토글 = 짝수 번 = 눈에는 아무 변화가
    /// 없어 보임). 견적/구매요청 "불러오기" 팝업의 선택 체크박스에서 여러 번 클릭해야 선택되던
    /// 증상이 이 경쟁 때문이었다(2026-09-28 실제 지적). 편집기를 아예 안 띄우면 이 경쟁이 원천적으로
    /// 사라진다 - 체크박스 그림 자체는 편집기 없이도 셀 렌더링만으로 항상 그려진다(DevExpress 표준
    /// 동작, 포커스/편집 여부와 무관). GridViewWyn/BandedGridViewWyn을 쓰는 모든 화면에 공통 적용.</summary>
    private void OnShowingEditor(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_view.FocusedColumn?.ColumnEdit is RepositoryItemCheckEdit)
            e.Cancel = true;
    }

    /// <summary>DevExpress 자체 팝업은 여기서 완전히 끈다(e.Allow = false) - 데이터 영역
    /// 우클릭 메뉴는 전부 OnGridControlMouseUp이 직접 만들어서 띄우는 것 하나로 통일한다.
    /// 원래는 행이 있을 땐 DevExpress 기본 팝업을, 행이 0개일 땐 우리가 만든 팝업을 따로
    /// 썼는데(2026-09-16), 실제로 써보니 어느 쪽이 뜨는지 클릭 위치(행 위/인디케이터/빈
    /// 영역)에 따라 갈려서 "여기선 되는데 저기선 안 된다"는 혼란만 낳았다 - 아예 하나로
    /// 합쳐서 클릭 위치와 무관하게 항상 같은 메뉴/같은 동작이 되게 한다.</summary>
    private void OnPopupMenuShowing(object? sender, PopupMenuShowingEventArgs e)
    {
        if (e.MenuType != GridMenuType.Row) return;
        e.Allow = false;
    }

    /// <summary>데이터 영역 우클릭 메뉴(행추가/화면레이아웃/Excel연동/선택영역 합계)를 실제로
    /// 채워 넣는다 - ShowRowContextMenu가 직접 만든 팝업에 쓴다(클래스 상단 OnPopupMenuShowing
    /// 설명 참고 - DevExpress 자체 팝업은 완전히 꺼뒀다).</summary>
    private void BuildRowContextMenuItems(DXMenuItemCollection items)
    {
        AddRowManagementMenuItems(items);

        var layoutMenu = new DXSubMenuItem("화면레이아웃") { BeginGroup = true };
        layoutMenu.Items.Add(new DXMenuItem("레이아웃저장", (s, args) => LayoutSaveRequested?.Invoke(this, EventArgs.Empty)));
        layoutMenu.Items.Add(new DXMenuItem("레이아웃초기화", (s, args) => LayoutResetRequested?.Invoke(this, EventArgs.Empty)));
        items.Add(layoutMenu);

        var excelMenu = new DXSubMenuItem("Excel연동") { BeginGroup = true };
        excelMenu.Items.Add(new DXMenuItem("엑셀로 내보내기", (s, args) => ExportToExcel(null)));
        excelMenu.Items.Add(new DXMenuItem("선택 영역만 엑셀로 내보내기", (s, args) => ExportSelectionToExcel(null))
        {
            Enabled = _view.GetSelectedCells().Length > 0
        });
        excelMenu.Items.Add(new DXMenuItem("엑셀 붙여넣기", (s, args) => PasteFromClipboard())
        {
            Enabled = _role == GridRoleWyn.Edit
        });
        excelMenu.Items.Add(new DXMenuItem("붙여넣기 실행취소", (s, args) => UndoLastPaste())
        {
            Enabled = _role == GridRoleWyn.Edit && _lastPasteUndo != null
        });
        items.Add(excelMenu);

        var values = new List<double>();
        foreach (var cell in _view.GetSelectedCells())
        {
            if (!IsNumericColumn(cell.Column)) continue;

            var value = _view.GetRowCellValue(cell.RowHandle, cell.Column);
            if (value == null || value == DBNull.Value) continue;

            values.Add(Convert.ToDouble(value));
        }

        if (values.Count < 2) return; // 셀 하나만 선택했을 땐 굳이 안 보여줌 - 값 자체가 그대로 보이므로

        var sum = values.Sum();
        var avg = values.Average();
        items.Add(new DXMenuItem($"선택 영역 - 합계: {sum:N2}   평균: {avg:N2}   개수: {values.Count}", null!)
        {
            Enabled = false,
            BeginGroup = true
        });
    }

    /// <summary>데이터 영역 우클릭 메뉴는 클릭 위치(실제 행 위/행 인디케이터/빈 영역)와 무관하게
    /// 전부 여기서 직접 팝업을 만들어 띄운다 - OnPopupMenuShowing은 e.Allow = false로 DevExpress
    /// 자체 팝업을 완전히 꺼둔다(그 메서드 설명 참고). 같은 MouseUp 이벤트 처리 도중에
    /// ShowPopup을 바로 부르면(동기 호출) 그 우클릭의 마우스업 자체가 팝업에 "바깥 클릭"으로
    /// 잘못 전달되어 뜨자마자 닫히거나 첫 클릭이 씹히는 경우가 있었다(2026-09-16 실제 겪음 -
    /// 위치에 따라 되다 안 되다 함) - BeginInvoke로 한 박자 늦춰서 이번 마우스업 처리가 완전히
    /// 끝난 뒤에 띄운다.</summary>
    private void OnGridControlMouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        var gridControl = _view.GridControl;
        var location = e.Location;
        gridControl.BeginInvoke(new MethodInvoker(() => ShowRowContextMenu(gridControl, location)));
    }

    private void ShowRowContextMenu(GridControl gridControl, Point location)
    {
        // ShowPopup은 팝업을 띄우자마자 바로 리턴한다(항목 클릭은 나중에 비동기로 옴) - using으로
        // 즉시 Dispose하면 사용자가 뭘 누르기도 전에 메뉴/그 안의 클릭 델리게이트가 사라져서
        // "행추가/멀티행추가 눌러도 아무 반응 없음" 증상이 났다(2026-09-16 실제 겪음). 팝업이
        // 실제로 닫힐 때(CloseUp - 항목 선택이든 바깥 클릭이든) 그때 가서 Dispose한다.
        var menu = new DXPopupMenu(gridControl, ScaleHelper.CreateScaleHelperByDPI(gridControl));
        BuildRowContextMenuItems(menu.Items);
        if (menu.Items.Count == 0)
        {
            menu.Dispose();
            return;
        }

        menu.CloseUp += (s, args) => menu.Dispose();
        menu.ShowPopup(gridControl, location);
    }

    private static bool IsNumericColumn(GridColumn column)
    {
        var t = column.ColumnType;
        return t == typeof(int) || t == typeof(long) || t == typeof(short) ||
               t == typeof(float) || t == typeof(double) || t == typeof(decimal);
    }

    /// <summary>컨텍스트메뉴(데이터 영역 우클릭)에 행추가/멀티행추가/행삭제를 붙인다(2026-09-16
    /// 요청). 저장 가능한 그리드(Role=Edit)에서만 보인다 - 조회 전용 그리드는 애초에 행을 늘리고/
    /// 지울 수가 없으므로 메뉴 자체를 숨긴다(EmbeddedNavigator 5개 버튼이 Role 하나로 노출되는
    /// 규칙, ApplyRole 주석과 동일한 이유). 실제 추가/삭제는 화면의 RowAdd/RowDelete 델리게이트를
    /// 그대로 태운다 - 그리드가 직접 AddNewRow/DeleteRow를 부르면 화면별 검증(frmMinorCode의
    /// "대분류를 먼저 저장해야 함" 등)을 건너뛰는 구멍이 생기기 때문(OnNavigatorButtonClick와
    /// 같은 이유).</summary>
    private void AddRowManagementMenuItems(DXMenuItemCollection items)
    {
        if (_role != GridRoleWyn.Edit) return;

        // 선택된 행이 있으면(빈 그리드/새 입력행이 아닌 실제 행) 그 바로 다음에 끼워 넣는다
        // (2026-09-16 요청) - 없으면(빈 그리드 등) 원래대로 끝에 추가된다(AddRowAfterFocused 참고).
        var hasFocusedDataRow = _view.FocusedRowHandle != GridControl.NewItemRowHandle && _view.FocusedRowHandle >= 0;

        items.Add(new DXMenuItem("행추가", (s, args) => AddRowAfterFocused())
        {
            BeginGroup = true,
            Enabled = _rowAdd != null
        });
        items.Add(new DXMenuItem("행복사", (s, args) => CopyFocusedRow())
        {
            Enabled = _rowAdd != null && hasFocusedDataRow
        });
        items.Add(new DXMenuItem("멀티행추가...", (s, args) => AddMultipleRows())
        {
            Enabled = _rowAdd != null
        });

        var selectedRowHandles = GetSelectedRowHandlesForDelete();
        items.Add(new DXMenuItem(
            selectedRowHandles.Count > 1 ? $"행삭제 (선택 {selectedRowHandles.Count}개)" : "행삭제",
            (s, args) => DeleteRows(selectedRowHandles))
        {
            Enabled = _rowDelete != null && selectedRowHandles.Count > 0
        });
    }

    /// <summary>RowAdd 델리게이트는 화면마다 그냥 끝에 새 행을 추가하도록 짜여 있다(AddNewRow()) -
    /// 컨텍스트메뉴 "행추가"는 대신 지금 포커스된 행 바로 다음에 끼워 넣는다(2026-09-16 요청).
    /// AddMultipleRows와 같은 방식으로 커밋한다: RowAdd 호출 한 번은 새 입력행(NewItemRowHandle)
    /// 으로 포커스만 옮길 뿐이라, CloseEditor+UpdateCurrentRow로 직접 커밋을 확정한 다음에야
    /// 실제 데이터소스 순서를 옮길 수 있다. 포커스된 행이 없으면(빈 그리드 등) 그냥 원래대로
    /// 끝에 남겨둔다 - 옮길 기준 위치가 없으므로.</summary>
    private void AddRowAfterFocused()
    {
        if (_rowAdd == null) return;

        var sourceHandle = _view.FocusedRowHandle;
        var insertIndex = sourceHandle != GridControl.NewItemRowHandle && sourceHandle >= 0
            ? _view.GetDataSourceRowIndex(sourceHandle) + 1
            : -1;

        _rowAdd.Invoke(_view, EventArgs.Empty);
        if (_view.FocusedRowHandle != GridControl.NewItemRowHandle) return; // 화면 검증에 막혀 실제로 안 늘어남

        _view.CloseEditor();
        if (!_view.UpdateCurrentRow())
        {
            _view.CancelUpdateCurrentRow();
            return;
        }

        if (insertIndex < 0 || insertIndex >= _view.DataRowCount - 1) return; // 옮길 필요 없음(이미 그 자리이거나 기준이 없음)
        MoveDataRow(_view.DataRowCount - 1, insertIndex);
    }

    /// <summary>지금 포커스된 행의 값을 그대로 복사한 새 행을 바로 다음에 끼워 넣는다(2026-09-16
    /// 요청). AddRowAfterFocused로 빈 행을 먼저 원하는 위치에 만들고, 그 자리에 원본 값을
    /// 채운 뒤 다시 커밋한다 - 화면마다 다른 컬럼 구성을 몰라도 되게, 보이는 컬럼 값을 전부
    /// 그대로 복사한다(메뉴ID 등 서버가 채번하는 PK 컬럼은 화면이 애초에 읽기전용으로 막아두는
    /// 관례라 별도로 걸러낼 필요가 없다 - frmMenu.txtMenuId 등).</summary>
    private void CopyFocusedRow()
    {
        if (_rowAdd == null) return;

        var sourceHandle = _view.FocusedRowHandle;
        if (sourceHandle == GridControl.NewItemRowHandle || sourceHandle < 0) return;

        var values = _view.Columns.Cast<GridColumn>()
            .ToDictionary(col => col, col => _view.GetRowCellValue(sourceHandle, col));

        AddRowAfterFocused();

        var newHandle = _view.FocusedRowHandle;
        if (newHandle == GridControl.NewItemRowHandle || newHandle < 0 || newHandle == sourceHandle) return; // 검증 실패로 실제로 안 늘어남

        foreach (var kv in values)
            _view.SetRowCellValue(newHandle, kv.Key, kv.Value);

        _view.CloseEditor();
        _view.UpdateCurrentRow();
    }

    /// <summary>데이터소스에서 행 하나를 다른 위치로 옮긴다 - List(IList)/DataTable 둘 다 지원한다
    /// (이 코드베이스의 그리드가 사실상 이 둘뿐). 그 외 타입이면 조용히 아무 일도 안 한다(새
    /// 행은 원래대로 끝에 남는다) - 지원 안 하는 데이터소스 때문에 예외로 화면이 깨지면 안 된다.</summary>
    private void MoveDataRow(int fromIndex, int toIndex)
    {
        switch (_view.GridControl.DataSource)
        {
            case System.Data.DataTable table:
                var row = table.Rows[fromIndex];
                table.Rows.Remove(row);
                table.Rows.InsertAt(row, toIndex);
                break;
            case System.Collections.IList list:
                var item = list[fromIndex];
                list.RemoveAt(fromIndex);
                list.Insert(toIndex, item);
                break;
            default:
                return;
        }

        _view.RefreshData();
        _view.FocusedRowHandle = _view.GetRowHandle(toIndex);
    }

    /// <summary>우클릭 시점에 선택돼 있던 행들 - CellSelect 모드(이 그리드 공통 기본값)라서
    /// GetSelectedRows()가 아니라 GetSelectedCells()에서 행 핸들만 뽑아 중복 제거한다("선택 영역
    /// 합계" 기능이 같은 이유로 이미 GetSelectedCells()를 쓰고 있다). 아무 셀도 명시적으로
    /// 다중선택 안 했으면(그냥 클릭 한 번) 포커스행 하나만 대상으로 삼는다. 아직 커밋 안 된
    /// 새 입력행(NewItemRowHandle)은 "삭제"라는 개념 자체가 안 맞아서 제외한다.</summary>
    private List<int> GetSelectedRowHandlesForDelete()
    {
        var handles = _view.GetSelectedCells()
            .Select(c => c.RowHandle)
            .Where(h => h != GridControl.NewItemRowHandle)
            .Distinct()
            .ToList();

        if (handles.Count == 0 && _view.FocusedRowHandle != GridControl.NewItemRowHandle && _view.FocusedRowHandle >= 0)
            handles.Add(_view.FocusedRowHandle);

        return handles;
    }

    /// <summary>여러 행을 한 번에 지운다 - 앞에서부터 지우면 행이 리스트 기반 그리드일 때 뒤쪽
    /// 행들의 핸들이 밀려서 엉뚱한 행이 지워질 수 있다(UndoLastPaste가 추가된 행을 거꾸로 지우는
    /// 것과 같은 이유) - 핸들이 큰 것부터(=나중 행부터) 지운다. 화면의 RowDelete 핸들러가 도중에
    /// 확인창을 띄우거나 실제로는 안 지우기로 할 수도 있으므로(검증 실패 등), 매번 그 행이 아직
    /// 그리드에 있는지 다시 확인한다.</summary>
    private void DeleteRows(List<int> rowHandles)
    {
        if (_rowDelete == null) return;

        foreach (var handle in rowHandles.OrderByDescending(h => h))
        {
            if (_view.GetVisibleIndex(handle) < 0) continue; // 이미 없어진 행(다른 경로로 지워짐 등) - 건너뜀
            _view.FocusedRowHandle = handle;
            _rowDelete.Invoke(_view, EventArgs.Empty);
        }
    }

    /// <summary>몇 개를 추가할지 물어보고 그만큼 빈 행을 연달아 커밋한다. RowAdd 한 번은 "새 입력행
    /// (NewItemRowHandle)으로 포커스를 옮기는 것"까지만 하므로, 다음 행을 또 추가하려면 그 전에
    /// 이번 행을 먼저 실제 행으로 커밋해둬야 한다(PasteFromClipboard와 같은 커밋 방식). 화면
    /// 검증에 막혀 더 이상 안 늘어나면(NewItemRowHandle로 안 옮겨감) 그 자리에서 조용히 멈춘다 -
    /// 이미 추가된 행은 그대로 남는다.
    ///
    /// "행추가"와 같은 이유로 선택된 행 바로 다음부터 순서대로 끼워 넣는다(2026-09-16 요청) -
    /// 선택된 행이 없으면(빈 그리드 등) 원래대로 끝에 쌓인다. 기준 위치(insertIndex)는 반복
    /// 시작 전에 한 번만 계산해두고, 한 행을 옮길 때마다 1씩 증가시킨다 - 매번 다시 계산하면
    /// 방금 끼워 넣은 행 자신이 "선택된 행"의 새 위치에 영향을 줘서 순서가 뒤섞인다.</summary>
    private void AddMultipleRows()
    {
        if (_rowAdd == null) return;

        var input = XtraInputBox.Show("추가할 행 개수를 입력하세요.", "멀티행추가", "5");
        if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out var count) || count <= 0) return;

        var sourceHandle = _view.FocusedRowHandle;
        var insertIndex = sourceHandle != GridControl.NewItemRowHandle && sourceHandle >= 0
            ? _view.GetDataSourceRowIndex(sourceHandle) + 1
            : -1;

        for (var i = 0; i < count; i++)
        {
            _rowAdd.Invoke(_view, EventArgs.Empty);
            if (_view.FocusedRowHandle != GridControl.NewItemRowHandle) break; // 화면 검증에 막혀 실제로 안 늘어남

            _view.CloseEditor();
            if (!_view.UpdateCurrentRow())
            {
                _view.CancelUpdateCurrentRow();
                break;
            }

            if (insertIndex < 0 || insertIndex >= _view.DataRowCount - 1) continue; // 옮길 필요 없음(이미 그 자리이거나 기준이 없음)
            MoveDataRow(_view.DataRowCount - 1, insertIndex);
            insertIndex++; // 다음 행은 방금 옮긴 행 바로 다음 자리로 들어가야 원래 순서가 유지된다.
        }
    }

    private void OnCustomDrawRowIndicator(object? sender, RowIndicatorCustomDrawEventArgs e)
    {
        if (!ShowRowNumbers || e.RowHandle < 0) return;
        e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    /// <summary>데이터가 0건일 때 그리드 가운데에 안내 문구를 직접 그린다 - DevExpress
    /// 자체에는 이런 placeholder 기능이 없어서 빈 화면 영역에 텍스트를 수동으로 그려야 한다.</summary>
    private void OnCustomDrawEmptyForeground(object? sender, CustomDrawEventArgs e)
    {
        if (_view.RowCount > 0 || string.IsNullOrEmpty(EmptyText)) return;

        using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        using var brush = new SolidBrush(Color.FromArgb(160, 160, 160));
        e.Graphics.DrawString(EmptyText, _view.Appearance.Empty.Font, brush, e.Bounds, format);
        e.Handled = true;
    }

    private void OnCellValueChanged(object? sender, CellValueChangedEventArgs e)
    {
        if (HighlightUnsavedCells) _dirtyCells.Add((e.RowHandle, e.Column.FieldName));

        // 품번 등을 직접 타이핑해서 값이 바뀐 순간(커밋 시점) 그 값이 맞는지 고를 수 있게 팝업을
        // 띄운다. 팝업 선택 결과를 SetRowCellValue로 써넣는 것도 이 이벤트를 일으키므로,
        // PopupLookupColumnEdit.IsApplyingResult로 그 프로그램적 변경은 걸러낸다.
        if (e.Column.ColumnEdit is PopupLookupColumnEdit popupEdit)
        {
            PopupDebugLog.Write($"CellValueChanged: col={e.Column.FieldName}, value={e.Value}, applying={popupEdit.IsApplyingResult}");
            if (!_pasting && !popupEdit.IsApplyingResult && !string.IsNullOrEmpty(Convert.ToString(e.Value)))
                _ = popupEdit.OpenPopupForCellAsync(_view, e.RowHandle, e.Column);
        }
    }

    /// <summary>셀 편집기가 열릴 때 전각 방어를 건다 - 붙여넣은 전각 문자를 반각으로 바꾸고, IME가 전각 모드면 반각으로 되돌린다(ImeGuard 참고).</summary>
    private void OnShownEditorImeGuard(object? sender, EventArgs e)
    {
        if (_view.ActiveEditor is not { } editor) return;
        editor.EditValueChanging -= ImeGuard.Edit_EditValueChanging;
        editor.EditValueChanging += ImeGuard.Edit_EditValueChanging;
        editor.BeginInvoke(new Action(ImeGuard.ClearFullShapeOfFocusedWindow));
    }

    /// <summary>편집 확정 직전 마지막 확인 - 문자열 값에 전각 문자가 남아 있으면 반각으로 바꾼다.</summary>
    private static void OnValidatingEditorImeGuard(object? sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
    {
        if (e.Value is string text && WYNLAB.Shared.TextNormalizer.NeedsFix(text))
            e.Value = WYNLAB.Shared.TextNormalizer.ToHalfWidth(text);
    }

    /// <summary>숫자 값은 오른쪽 정렬이 표준(2026-09-25 사장님 지시 - "모든 그리드의 숫자 컬럼은 우측정렬"). 컬럼마다 정렬을 지정하지
    /// 않아도 되게 여기서 한꺼번에 처리한다: 셀 값이 숫자이거나 컬럼 서식(DisplayFormat)이 숫자이면 오른쪽. 예외 두 가지 -
    /// ① 컬럼에 정렬을 직접 지정해 둔 경우(AppearanceCell.TextOptions.HAlignment를 Default가 아닌 값으로) 그 값을 따른다.
    /// ② 값이 숫자여도 화면엔 이름/글자로 보이는 컬럼(룩업/팝업/체크/날짜 등 TextEdit 계열이 아닌 편집기)은 건드리지 않는다
    /// (예: 창고ID를 팝업으로 고르는 컬럼, 코드 대신 명칭을 보여주는 룩업).</summary>
    private static bool ShouldRightAlign(GridColumn column, object? cellValue)
    {
        if (column.AppearanceCell.Options.UseTextOptions && column.AppearanceCell.TextOptions.HAlignment != HorzAlignment.Default) return false;

        var edit = column.ColumnEdit;
        if (edit != null && edit.GetType() != typeof(RepositoryItemTextEdit) && edit is not RepositoryItemSpinEdit) return false;

        if (column.DisplayFormat.FormatType == FormatType.Numeric) return true;
        return cellValue is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal;
    }

    private void OnRowCellStyle(object? sender, RowCellStyleEventArgs e)
    {
        if (ShouldRightAlign(e.Column, e.CellValue))
        {
            e.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
            e.Appearance.Options.UseTextOptions = true;
        }

        // 필수 입력 컬럼(GridViewWyn.RequiredFields)의 빈 셀은 항상 필수 색으로 - 포커스 행/미저장 강조보다 먼저.
        if (_view is GridViewWyn wyn && wyn.IsRequiredColumn(e.Column)
            && (e.CellValue == null || e.CellValue == DBNull.Value || (e.CellValue is string s && string.IsNullOrWhiteSpace(s))))
        {
            e.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
            e.Appearance.Options.UseBackColor = true;
            return;
        }

        // 미저장 강조가 우선한다 - 포커스행이면서 동시에 수정된 셀이라면, "아직 저장 안 됨"이
        // "지금 포커스된 행"보다 사용자가 더 먼저 알아야 하는 정보라서.
        if (HighlightUnsavedCells && _dirtyCells.Contains((e.RowHandle, e.Column.FieldName)))
        {
            e.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
            e.Appearance.Options.UseBackColor = true;
            return;
        }

        // CellSelect 모드(기본값)에서는 DevExpress의 EnableAppearanceFocusedRow가 셀 하나에만
        // 적용되고 행 전체엔 안 먹는다(frmMinorCode에서 직접 확인된 문제 - 화면 캡처로 확인).
        // 그래서 선택 모드와 무관하게 확실히 먹는 RowCellStyle 방식으로 행 전체를 칠한다.
        if (HighlightFocusedRow && e.RowHandle == _view.FocusedRowHandle)
        {
            e.Appearance.BackColor = UiTheme.GridFocusedRowBackColor;
            e.Appearance.Options.UseBackColor = true;
        }
    }

    // RowAdd/RowDelete - 행추가/행삭제 클릭을 실제로 처리할지는 이 구독 여부로 결정하지만,
    // EmbeddedNavigator 버튼의 노출(Visible)은 2026-09-03부터 순수하게 Role만 따른다(사장님 지시 -
    // "조회 화면은 5개 버튼 전부 Visible:false, 수정/저장/삭제 화면은 5개 전부 Visible:true", Grid/
    // BandedGrid 공통). 예전엔 RowAdd/RowDelete 구독 여부로 Append/Remove만 따로 숨겼었는데
    // (frmUserAuth 같은 "편집은 되지만 행 자체는 안 늘어야 하는" 그리드를 위해), 지금은 Role
    // 하나로 5개 버튼이 전부 통일되게 바뀌었다 - 그런 그리드가 필요해지면 별도 논의.
    // 구독자가 없어도 버튼이 보일 수 있으므로, DevExpress 기본 동작(View.AddNewRow()/DeleteRow()를
    // 그냥 실행)이 새서 화면마다 있는 검증 로직(예: frmMinorCode.NewRowClick의 "대분류를 먼저
    // 저장해야 함")을 건너뛰지 않도록 OnNavigatorButtonClick에서 구독 여부와 무관하게 항상
    // e.Handled=true로 가로챈다(구독 없으면 조용히 무시).
    private EventHandler? _rowAdd;
    public event EventHandler? RowAdd
    {
        add => _rowAdd += value;
        remove => _rowAdd -= value;
    }

    private EventHandler? _rowDelete;
    public event EventHandler? RowDelete
    {
        add => _rowDelete += value;
        remove => _rowDelete -= value;
    }

    private bool _navigatorWired;

    /// <summary>Role이 바뀔 때마다 실제로 그리드를 그 역할에 맞게 잠근다/연다. OptionsBehavior는
    /// View 소속이라 바로 적용되지만, EmbeddedNavigator는 GridControl 소속이라 View가 아직 어느
    /// GridControl에도 안 붙은 시점(생성자 직후)엔 null이다 - 화면 생성자가 InitializeComponent()
    /// 뒤에 Role을 설정하는 게 표준 사용법이라 실제로는 항상 연결된 뒤에 호출되므로, 여기선 null이면
    /// 그냥 건너뛴다(재시도용 이벤트 구독까지는 필요 없음 - 실제로 그런 순서로 안 불림).</summary>
    /// <summary>화면이 `.Role = ...`을 한 번도 명시적으로 설정하지 않은 그리드를 위한 안전장치.
    /// `Role` 프로퍼티는 세터를 거쳐야만 ApplyRole()이 불리는데(필드 기본값 GridRoleWyn.Query는
    /// 그냥 초기값일 뿐 세터를 안 거치므로 아무 효과가 없다), Designer의 [DefaultValue(Query)]
    /// 때문에 "디자이너에서 굳이 안 건드린 그리드"는 InitializeComponent()에 `gvw1.Role = ...`
    /// 코드 자체가 생성되지 않는다 - 그 결과 EmbeddedNavigator가 DevExpress 순정 기본값(추가/
    /// 삭제/편집 버튼 전부 보임+활성) 그대로 남는 사고가 실제로 있었다(frmSysLookup grd1,
    /// Role=Query 의도였지만 한 번도 안 불림, 2026-09-02). GridViewWyn/BandedGridViewWyn이
    /// EndInit()에서 이 메서드를 호출해 명시적 설정 여부와 무관하게 항상 한 번은 잠금을
    /// 적용한다 - 그 뒤 화면 생성자가 실제로 `.Role = ...`을 부르면 그때 다시 ApplyRole()이
    /// 불려 최종 상태를 덮어쓰므로 순서 상관없이 항상 맞다.</summary>
    public void EnsureRoleApplied() => ApplyRole();

    private void ApplyRole()
    {
        var editable = _role == GridRoleWyn.Edit;

        _view.OptionsBehavior.Editable = editable;

        // DevExpress 내장 PasteMode는 하나로 두 요구사항(커서 위치에 덮어쓰기 + 행이 모자라면
        // 새 행 추가)을 동시에 못 만족한다 - PasteMode.Update는 이미 있는 행만 덮어쓸 뿐 새 행을
        // 못 만들고(빈 그리드에 여러 행을 붙여넣으면 내부적으로 "동일한 키를 사용하는 항목이 이미
        // 추가되었습니다" 크래시가 남, 실제로 겪음 2026-09-05), PasteMode.Append는 반대로 커서
        // 위치를 무시하고 항상 끝에 새 행만 추가한다. 그래서 내장 붙여넣기는 아예 끄고
        // (PasteMode.None - GridOptionsClipboard엔 AllowPaste 같은 별도 스위치가 없어서 끄는
        // 유일한 방법), Ctrl+V를 직접 가로채 PasteFromClipboard()에서 두 경우를 모두 처리한다.
        _view.OptionsClipboard.PasteMode = PasteMode.None;

        var gridControl = _view.GridControl;
        if (gridControl == null) return;

        WireProcessGridKey(gridControl);
        WireGridControlMouseUp(gridControl);

        var navigator = gridControl.EmbeddedNavigator;
        if (navigator == null) return;

        WireNavigatorButtonClick(navigator);

        // EmbeddedNavigator 5개 버튼(Append/Delete/Edit/EndEdit/CancelEdit) 전부 Role 하나로
        // 통일해서 노출한다 - 조회 화면(Query)은 전부 숨김, 수정/저장/삭제 화면(Edit)은 전부
        // 노출(사장님 지시, 2026-09-03).
        navigator.Buttons.Append.Visible = editable;
        navigator.Buttons.Remove.Visible = editable;
        navigator.Buttons.Edit.Visible = editable;
        navigator.Buttons.EndEdit.Visible = editable;
        navigator.Buttons.CancelEdit.Visible = editable;
    }

    private void WireNavigatorButtonClick(ControlNavigator navigator)
    {
        if (_navigatorWired) return;
        _navigatorWired = true;
        navigator.ButtonClick += OnNavigatorButtonClick;
    }

    private bool _processGridKeyWired;

    private void WireProcessGridKey(GridControl gridControl)
    {
        if (_processGridKeyWired) return;
        _processGridKeyWired = true;
        gridControl.ProcessGridKey += OnProcessGridKey;
    }

    private bool _gridMouseUpWired;

    private void WireGridControlMouseUp(GridControl gridControl)
    {
        if (_gridMouseUpWired) return;
        _gridMouseUpWired = true;
        gridControl.MouseUp += OnGridControlMouseUp;
    }

    /// <summary>GridControl.ProcessGridKey는 포커스된 View/활성 셀 편집기보다 먼저 키를 볼 수
    /// 있다 - Ctrl+V만 여기서 가로채 PasteFromClipboard()로 넘긴다(ApplyRole의 PasteMode.None
    /// 주석 참고, 내장 붙여넣기를 아예 안 쓰는 이유). 셀 편집기가 열려있는 중이면(더블클릭해서
    /// 글자를 고치는 중 등) 보통은 가로채지 않는다 - 그 경우의 Ctrl+V는 평범한 "텍스트 붙여넣기"라
    /// 편집기가 직접 처리해야 정상이고, 여기서 채가면 셀 안에 글자를 못 붙여넣게 된다.
    /// 단 클립보드가 여러 칸(탭/줄바꿈으로 나뉜 엑셀 범위)이면 편집기를 닫고 그리드 붙여넣기로 처리한다 -
    /// 셀을 클릭하면 곧바로 편집기가 열리는 그리드(EditorShowMode=MouseDown)에서 엑셀 범위를 붙여넣으면
    /// 한 셀 안에 전부 들어가 버리기 때문이다(2026-10-05). 엑셀의 한 칸 복사(끝에 줄바꿈 하나)는 여러 칸이 아니다.</summary>
    private DateTime _lastPasteScheduledAt = DateTime.MinValue;

    private static bool ClipboardIsMultiCell()
    {
        try
        {
            if (!Clipboard.ContainsText()) return false;
            var text = Clipboard.GetText().TrimEnd('\r', '\n');
            return text.Contains('\t') || text.Contains('\n');
        }
        catch { return false; } // 클립보드를 다른 프로그램이 잡고 있으면 평소처럼 편집기가 처리하게 둔다
    }

    private void OnProcessGridKey(object? sender, KeyEventArgs e)
    {
        if (!e.Control) return;
        if (e.KeyCode != Keys.V && e.KeyCode != Keys.Z) return;
        if (_view.ActiveEditor != null)
        {
            if (e.KeyCode != Keys.V || _role != GridRoleWyn.Edit || !ClipboardIsMultiCell()) return;
            _view.HideEditor(); // 편집 중이던 글자는 버리고(커밋 안 함) 포커스된 셀부터 붙여넣는다.
        }

        e.Handled = true;
        if (_role != GridRoleWyn.Edit) return;

        if (e.KeyCode == Keys.Z)
        {
            // 실행취소는 _lastPasteUndo를 시작하자마자 비우므로(UndoLastPaste 참고) 같은 키
            // 입력에 이 핸들러가 두 번 불려도(Ctrl+V와 같은 이유) 두 번째는 자연히 아무 일도
            // 안 한다 - Ctrl+V처럼 별도 디바운스가 필요 없다.
            _view.GridControl.BeginInvoke(new MethodInvoker(UndoLastPaste));
            return;
        }

        // ProcessGridKey 문서상 "포커스된 View와 활성 편집기" 두 단계에서 각각 불릴 수 있다고
        // 돼 있는데, 실제로 같은 Ctrl+V 한 번에 이 핸들러가 두 번 불려서 붙여넣기가 두 번
        // 실행되는 문제가 있었다(실제로 겪음, 2026-09-05/06). 두 호출이 같은 순간(동기)이
        // 아니라 살짝 시차를 두고 들어와서(직전 BeginInvoke가 이미 실행·완료된 뒤) 단순
        // "예약 중" 플래그로는 안 걸러졌다 - 그래서 마지막 예약 시각을 기억해두고, 아주 짧은
        // 시간(0.5초) 안에 또 들어오면 같은 입력의 중복으로 보고 무시한다.
        var now = DateTime.UtcNow;
        if ((now - _lastPasteScheduledAt).TotalMilliseconds < 500) return;
        _lastPasteScheduledAt = now;

        // ProcessGridKey는 DevExpress가 이 키 입력을 마저 처리하기 "전"에 불린다 - 그 안에서 바로
        // 행을 여러 개 추가/커밋하면 아직 안 끝난 키 입력 처리(포커스 이동 등)와 겹쳐서 마지막 행에
        // 값이 이상하게 들어가는 문제가 있었다(실제로 겪음, 2026-09-05 - 8행 붙여넣기 중 새로 추가된
        // 마지막 행만 값이 꼬임). 지금 키 입력 처리가 완전히 끝난 뒤(다음 메시지 루프에서) 실행되도록
        // 한 박자 늦춘다.
        _view.GridControl.BeginInvoke(new MethodInvoker(PasteFromClipboard));
    }

    /// <summary>포커스된 셀 위치부터 클립보드(엑셀 등에서 복사한 탭구분 텍스트)를 붙여넣는다.
    /// 이미 있는 행이면 그 자리에서 덮어쓰고, 행이 모자라면 화면의 RowAdd 델리게이트(예:
    /// frmMinorCode.NewRowClick의 "대분류를 먼저 저장해야 함" 검증)를 그대로 태워서 한 행씩
    /// 늘린다 - RowAdd 구독이 없는 그리드(행 개수가 고정된 그리드, frmUserAuth 등)는 그 지점에서
    /// 멈춘다(OnNavigatorButtonClick의 "구독 없으면 no-op" 규칙과 동일). 새로 늘린 행은 값을 채운
    /// 뒤 UpdateCurrentRow()로 한 행씩 확실히 커밋하고서야 다음 행으로 넘어간다 - DevExpress 내장
    /// PasteMode.Update가 신규 행 여러 개를 커밋 없이 한번에 처리하려다 크래시 나던 문제
    /// (ApplyRole 주석, 2026-09-05)를 이 순차 커밋 방식으로 피한다.</summary>
    // 붙여넣기/붙여넣기 취소가 셀에 값을 쓰는 동안 true - 팝업 컬럼(PopupLookupColumnEdit)이 붙여넣은 값마다 팝업을 띄우지 않게 한다(이름은 그대로 두고 화면의 검증이 처리).
    private bool _pasting;

    private void PasteFromClipboard()
    {
        _pasting = true;
        try { PasteCore(); }
        finally { _pasting = false; }
    }

    /// <summary>붙여넣은 글자를 그 컬럼이 실제로 저장하는 값으로 바꾼다. 룩업/콤보 컬럼의 셀 값은 코드(예: "A")인데
    /// 엑셀/그리드에서 복사한 글자는 화면에 보이던 이름("사용")이라 그대로 넣으면 목록에 없는 값이라 빈 칸으로 보였다
    /// (2026-10-05 품목일괄수정 품목상태). 이름 → 코드로 찾고, 이미 코드를 붙여넣었으면 그대로 둔다. 체크박스 컬럼은
    /// Y/N/True/False/1/0을 ValueChecked/ValueUnchecked로 바꾼다. 못 찾으면 원문 그대로(화면 검증이 처리).</summary>
    private static object? ConvertPastedText(GridColumn column, string text, out bool ok)
    {
        ok = true;
        // 엑셀/웹에서 온 전각 숫자·공백(NBSP)·보이지 않는 글자는 반각으로 - 이게 숫자 칸에 그대로 들어가면 "입력 문자열의 형식이 잘못되었습니다"가 난다(전각 방어, ImeGuard와 같은 규칙).
        text = (WYNLAB.Shared.TextNormalizer.ToHalfWidth(text) ?? string.Empty).Replace("​", "").Replace("﻿", "");
        var trimmed = text.Trim();

        // 숫자 컬럼: 쉼표/공백을 걷어내고 그 컬럼 타입으로 변환한다. 빈 칸은 비움(null), 숫자가 아니면 ok=false(그 셀은 건너뜀).
        var type = Nullable.GetUnderlyingType(column.ColumnType) ?? column.ColumnType;
        if (type == typeof(decimal) || type == typeof(double) || type == typeof(float)
            || type == typeof(int) || type == typeof(long) || type == typeof(short))
        {
            if (trimmed.Length == 0) return DBNull.Value;
            var number = trimmed.Replace(",", "").Replace(" ", "");
            if (decimal.TryParse(number, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                    System.Globalization.CultureInfo.InvariantCulture, out var d))
            {
                try { return Convert.ChangeType(d, type, System.Globalization.CultureInfo.InvariantCulture); }
                catch (OverflowException) { }
            }
            ok = false;
            return null;
        }

        if (column.ColumnEdit is RepositoryItemLookUpEdit lookUp && trimmed.Length > 0)
        {
            if (lookUp.GetKeyValueByDisplayText(trimmed) is { } key) return key;
            if (lookUp.GetDataSourceRowByKeyValue(trimmed) != null) return trimmed;
        }
        else if (column.ColumnEdit is RepositoryItemCheckEdit checkEdit)
        {
            if (trimmed.Equals(Convert.ToString(checkEdit.ValueChecked), StringComparison.OrdinalIgnoreCase)
                || trimmed is "1" or "Y" or "y" or "true" or "True" or "TRUE" or "예" or "사용") return checkEdit.ValueChecked;
            if (trimmed.Equals(Convert.ToString(checkEdit.ValueUnchecked), StringComparison.OrdinalIgnoreCase)
                || trimmed is "" or "0" or "N" or "n" or "false" or "False" or "FALSE" or "아니오" or "미사용") return checkEdit.ValueUnchecked;
        }
        return text;
    }

    private void PasteCore()
    {
        if (!Clipboard.ContainsText()) return;

        var focusedColumn = _view.FocusedColumn;
        if (focusedColumn == null) return;

        var pasteColumns = _view.Columns.Cast<GridColumn>()
            .Where(c => c.Visible && c.VisibleIndex >= focusedColumn.VisibleIndex)
            .OrderBy(c => c.VisibleIndex)
            .ToList();
        if (pasteColumns.Count == 0) return;

        var lines = Clipboard.GetText().Replace("\r\n", "\n").TrimEnd('\n').Split('\n');
        if (lines.Length == 0) return;

        // 새 행(맨 끝의 빈 입력행)에 포커스가 있으면 GetVisibleIndex가 -1을 돌려준다 - 그 경우
        // "지금 있는 행 다음"부터 이어붙이는 것으로 취급한다.
        var startVisibleIndex = _view.GetVisibleIndex(_view.FocusedRowHandle);
        if (startVisibleIndex < 0) startVisibleIndex = _view.RowCount;

        var originalRowCount = _view.RowCount; // 실행취소 시 "이 개수 이후로 새로 생긴 행"을 가려내는 기준
        var overwrites = new List<(int RowHandle, GridColumn Column, object? OriginalValue)>();
        var failedRows = 0;
        var badCells = 0; // 컬럼 형식(숫자 등)에 맞지 않아 건너뛴 셀 수

        for (var r = 0; r < lines.Length; r++)
        {
            var visibleIndex = startVisibleIndex + r;
            int rowHandle;
            var isNewRow = visibleIndex >= _view.RowCount;
            if (!isNewRow)
            {
                rowHandle = _view.GetVisibleRowHandle(visibleIndex);
                _view.FocusedRowHandle = rowHandle; // UpdateCurrentRow()가 이 행을 커밋하도록 포커스를 맞춘다.
            }
            else
            {
                if (_rowAdd == null) break; // 행 개수 고정 그리드 - 더 못 늘리니 여기서 중단.
                _rowAdd.Invoke(_view, EventArgs.Empty);
                if (_view.FocusedRowHandle != GridControl.NewItemRowHandle) break; // 화면 검증에 막혀 실제로 안 늘어남.
                rowHandle = GridControl.NewItemRowHandle;
            }

            var cells = lines[r].Split('\t');
            for (var c = 0; c < cells.Length && c < pasteColumns.Count; c++)
            {
                // 실행취소를 위해 덮어쓰기 전 원래 값을 먼저 기록한다(신규 행은 값 자체가 없었으니
                // 기록할 "원래 값"이 없다 - 행 자체를 지우는 걸로 되돌린다).
                if (!isNewRow)
                    overwrites.Add((rowHandle, pasteColumns[c], _view.GetRowCellValue(rowHandle, pasteColumns[c])));
                var pasted = ConvertPastedText(pasteColumns[c], cells[c], out var convertible);
                if (!convertible) { badCells++; continue; }
                try { _view.SetRowCellValue(rowHandle, pasteColumns[c], pasted); }
                catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException or ArgumentException) { badCells++; }
            }

            // UpdateCurrentRow() 전에 활성 편집기를 먼저 닫아 그 값이 확실히 반영된 뒤 커밋되게 한다
            // (DevExpress 공식 가이드 - RowUpdated는 편집기가 열려있는 동안은 안 불린다).
            _view.CloseEditor();
            if (!_view.UpdateCurrentRow())
            {
                _view.CancelUpdateCurrentRow();
                failedRows++;
            }
        }

        // 새로 생긴 행 핸들은 루프 도중이 아니라 다 끝난 뒤 최종 RowCount 기준으로 한 번에
        // 계산한다 - 커밋 실패(CancelUpdateCurrentRow)로 취소된 행은 애초에 RowCount에 안
        // 반영되므로 이렇게 해야 실제로 추가된 행만 정확히 걸러진다.
        var addedRowHandles = new List<int>();
        for (var i = originalRowCount; i < _view.RowCount; i++)
            addedRowHandles.Add(_view.GetVisibleRowHandle(i));

        _lastPasteUndo = overwrites.Count > 0 || addedRowHandles.Count > 0
            ? new PasteUndoState(overwrites, addedRowHandles)
            : null;

        if (badCells > 0)
        {
            XtraMessageBox.Show(
                $"{badCells}개 셀은 컬럼 형식(숫자 등)에 맞지 않아 붙여넣지 않았습니다.",
                "붙여넣기 결과", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        if (failedRows > 0)
        {
            XtraMessageBox.Show(
                $"{failedRows}개 행은 저장하지 못했습니다(중복된 값 등을 확인하세요).",
                "붙여넣기 결과", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>방금 한 번 실행한 붙여넣기만 되돌린다(여러 단계 Undo 스택이 아니라 딱 한 번 -
    /// 그래서 시작하자마자 _lastPasteUndo를 비워서 다시 못 부르게 한다). 덮어썼던 셀은 원래
    /// 값으로 복원하고, 붙여넣기가 새로 만든 행은 화면의 RowDelete 델리게이트를 그대로 태워서
    /// 지운다(OnNavigatorButtonClick과 같은 이유 - 그리드가 직접 DeleteRow를 부르면 화면별
    /// 삭제 검증을 건너뛰는 구멍이 생긴다). 나중에 추가된 행부터 거꾸로 지워야 앞쪽 행의 핸들이
    /// 안 밀린다. 그 사이 사용자가 직접 지웠거나 화면을 다시 조회해서 핸들이 더 이상 없는 행/셀은
    /// 조용히 건너뛴다.</summary>
    private void UndoLastPaste()
    {
        _pasting = true;
        try { UndoCore(); }
        finally { _pasting = false; }
    }

    private void UndoCore()
    {
        var undo = _lastPasteUndo;
        if (undo == null) return;
        _lastPasteUndo = null;

        for (var i = undo.AddedRowHandles.Count - 1; i >= 0; i--)
        {
            var handle = undo.AddedRowHandles[i];
            if (_rowDelete == null) continue;
            if (_view.GetVisibleIndex(handle) < 0) continue;
            _view.FocusedRowHandle = handle;
            _rowDelete.Invoke(_view, EventArgs.Empty);
        }

        foreach (var rowGroup in undo.Overwrites.GroupBy(o => o.RowHandle))
        {
            var rowHandle = rowGroup.Key;
            if (_view.GetVisibleIndex(rowHandle) < 0) continue;
            _view.FocusedRowHandle = rowHandle;
            foreach (var (_, column, originalValue) in rowGroup)
                _view.SetRowCellValue(rowHandle, column, originalValue);
            _view.CloseEditor();
            _view.UpdateCurrentRow();
        }
    }

    /// <summary>DevExpress 기본 동작(AddNewRow/DeleteRow를 그냥 실행)을 절대 그대로 두지 않는다 -
    /// RowAdd/RowDelete 구독 여부와 무관하게 항상 e.Handled=true로 가로채고, 구독이 있을 때만
    /// 실제로 그 델리게이트를 부른다. Append/Remove 버튼이 이제 Role만으로 노출되므로(구독 여부와
    /// 무관), 구독 없이 눌렸을 때 DevExpress 기본 동작이 새면 화면마다 있는 검증 로직(예:
    /// frmMinorCode.NewRowClick의 "대분류를 먼저 저장해야 함")을 건너뛰고 그리드가 직접 행을
    /// 만들어버리는 구멍이 생긴다 - 그래서 구독 없으면 조용히 무시(no-op)한다.</summary>
    private void OnNavigatorButtonClick(object? sender, NavigatorButtonClickEventArgs e)
    {
        if (e.Button.ButtonType == NavigatorButtonType.Append)
        {
            e.Handled = true;
            _rowAdd?.Invoke(_view, EventArgs.Empty);
        }
        else if (e.Button.ButtonType == NavigatorButtonType.Remove)
        {
            e.Handled = true;
            _rowDelete?.Invoke(_view, EventArgs.Empty);
        }
    }

    /// <summary>저장 성공 후 호출 - 수정 강조 표시를 전부 지운다.</summary>
    public void ClearDirtyMarks()
    {
        _dirtyCells.Clear();
        _view.Invalidate();

        // 저장이 끝나면 붙여넣기 실행취소는 더 이상 의미가 없다 - 스냅샷에 담긴 "원래 값"은
        // 이미 DB에 반영된 지금 상태보다 오래된 값이라, 그대로 되돌리면 화면과 DB가 어긋난다.
        _lastPasteUndo = null;
    }

    /// <summary>
    /// 컬럼 하단에 항상 보이는 합계/평균 등을 추가한다(선택 영역 합계와 별개 - 이건 상시 표시).
    /// 모든 숫자 컬럼에 자동으로 붙이지 않고 화면에서 필요한 컬럼만 명시적으로 호출하게 했다 -
    /// 코드/연도처럼 숫자형이어도 합계가 의미 없는 컬럼까지 자동으로 합산되는 걸 막기 위함.
    /// </summary>
    public void AddColumnSummary(GridColumn column, SummaryItemType type, string format)
    {
        _view.OptionsView.ShowFooter = true;
        column.Summary.Add(type, column.FieldName, format);
    }

    /// <summary>그룹핑된 그리드에서 그룹별 소계를 추가한다 - GroupSummary는 컬럼이 아니라
    /// View(그리드 전체) 소속이라 AddColumnSummary와 달리 이쪽에 붙인다.</summary>
    public void AddGroupSummary(GridColumn column, SummaryItemType type, string format)
    {
        _view.OptionsView.ShowGroupPanel = true;
        _view.GroupSummary.Add(type, column.FieldName, column, format);
    }

    /// <summary>엑셀로 내보내기 - 저장 위치를 사용자에게 물어보고 내보낸다.
    /// 지금까지 BaseGridForm.PrintClick에서 화면마다 반복하던 패턴을 그리드 쪽으로 옮겨서,
    /// BaseGridForm을 안 쓰는 화면(예: 기초코드등록처럼 그리드가 2개인 화면)에서도 그대로 쓸 수 있다.</summary>
    public void ExportToExcel(string? defaultFileName)
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx",
            FileName = defaultFileName ?? $"export_{DateTime.Now:yyyyMMdd}.xlsx"
        };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _view.ExportToXlsx(dlg.FileName);
            OpenExportedFile(dlg.FileName);
        }
    }

    /// <summary>선택된 셀이 걸쳐있는 행만 엑셀로 내보낸다(컬럼은 전체 - 행 단위로만 좁힌다).
    /// 화면에 안 띄운 임시 GridControl에 선택된 값만 옮겨 담아 내보내려 했으나, 창 핸들을 강제로
    /// 만들어도 내부 행 캐시가 안 채워져 매번 빈 파일이 나오는 문제가 반복됐다(실제로 겪음,
    /// 2026-09-06 - 두 번째 시도까지 실패). 그래서 새 그리드를 만드는 대신 이미 검증된 실제
    /// 화면 그리드(_view)를 그대로 재사용하되, CustomRowFilter로 선택되지 않은 행을 내보내는
    /// 순간만 잠깐 숨겼다가 곧바로 되돌린다.</summary>
    public void ExportSelectionToExcel(string? defaultFileName)
    {
        var cells = _view.GetSelectedCells();
        if (cells.Length == 0) return;

        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx",
            FileName = defaultFileName ?? $"export_selection_{DateTime.Now:yyyyMMdd}.xlsx"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var selectedDataSourceRows = new HashSet<int>(
            cells.Select(c => _view.GetDataSourceRowIndex(c.RowHandle)));

        void Filter(object? s, RowFilterEventArgs e)
        {
            e.Visible = selectedDataSourceRows.Contains(e.ListSourceRow);
            e.Handled = true;
        }

        _view.CustomRowFilter += Filter;
        try
        {
            _view.RefreshData();
            _view.ExportToXlsx(dlg.FileName);
        }
        finally
        {
            _view.CustomRowFilter -= Filter;
            _view.RefreshData(); // 숨겼던 행을 원상복구
        }

        OpenExportedFile(dlg.FileName);
    }

    /// <summary>내보내기 직후 결과를 바로 확인할 수 있게 엑셀(또는 .xlsx 기본 연결 프로그램)로
    /// 열어준다. 파일을 여는 것뿐이라 실행 파일을 직접 다운로드/실행하는 것과는 다르지만, 그래도
    /// 연결된 프로그램이 없거나(설치 안 된 PC) 경로에 문제가 있으면 예외가 날 수 있어 조용히
    /// 무시한다 - "엑셀 파일 자체는 이미 정상 저장됐다"는 사실과는 무관한 부가 기능이라, 여기서
    /// 실패해도 저장 자체가 실패한 것처럼 보이면 안 된다.</summary>
    private static void OpenExportedFile(string filePath)
    {
        try
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        catch
        {
        }
    }
}
