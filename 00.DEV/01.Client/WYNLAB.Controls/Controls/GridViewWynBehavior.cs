using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
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

        _view.OptionsClipboard.AllowCopy = DefaultBoolean.True;
        _view.OptionsClipboard.PasteMode = PasteMode.Append;

        _view.OptionsView.ShowAutoFilterRow = false; // 기본은 꺼둠 - 필요한 화면에서만
                                                       // view.OptionsView.ShowAutoFilterRow = true로 켠다.
        _view.OptionsView.ShowGroupPanel = false; // "Drag a column header here" 그룹 영역 - WYNLAB
                                                    // 화면 대부분은 그룹핑을 안 써서 기본은 숨김.
                                                    // 실제 그룹핑이 필요하면 AddGroupSummary()가
                                                    // 호출 시점에 다시 켜준다.
        _view.OptionsSelection.MultiSelect = true;
        _view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
        _view.OptionsNavigation.EnterMoveNextColumn = true;

        // 개인별 "레이아웃저장"(컬럼 순서/숨김/폭)이 정렬/그룹/필터/서식까지 같이 저장해버리면
        // "어제 걸어둔 조건 때문에 오늘 데이터가 안 보인다" 같은 혼란이 생긴다(사장님 지시로
        // 정렬/그룹/필터는 제외) - 저장 대상을 컬럼 배치 하나로만 좁혀둔다.
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
        _view.EndSorting += OnEndSorting;
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
        if (hitInfo.Column?.ColumnEdit is not RepositoryItemCheckEdit checkEdit) return;

        var current = _view.GetRowCellValue(hitInfo.RowHandle, hitInfo.Column);
        var isChecked = Equals(current, checkEdit.ValueChecked);
        _view.SetRowCellValue(hitInfo.RowHandle, hitInfo.Column, isChecked ? checkEdit.ValueUnchecked : checkEdit.ValueChecked);
        _view.FocusedRowHandle = hitInfo.RowHandle;
    }

    private void OnPopupMenuShowing(object? sender, PopupMenuShowingEventArgs e)
    {
        if (e.MenuType == GridMenuType.Column)
        {
            e.Menu.Items.Add(new DXMenuItem("레이아웃저장", (s, args) => LayoutSaveRequested?.Invoke(this, EventArgs.Empty)) { BeginGroup = true });
            e.Menu.Items.Add(new DXMenuItem("레이아웃초기화", (s, args) => LayoutResetRequested?.Invoke(this, EventArgs.Empty)));
            return;
        }

        if (e.MenuType != GridMenuType.Row) return;

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
        e.Menu.Items.Add(new DXMenuItem($"선택 영역 - 합계: {sum:N2}   평균: {avg:N2}   개수: {values.Count}", null!)
        {
            Enabled = false,
            BeginGroup = true
        });
    }

    private static bool IsNumericColumn(GridColumn column)
    {
        var t = column.ColumnType;
        return t == typeof(int) || t == typeof(long) || t == typeof(short) ||
               t == typeof(float) || t == typeof(double) || t == typeof(decimal);
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
        if (!HighlightUnsavedCells) return;
        _dirtyCells.Add((e.RowHandle, e.Column.FieldName));
    }

    private void OnRowCellStyle(object? sender, RowCellStyleEventArgs e)
    {
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

        var navigator = _view.GridControl?.EmbeddedNavigator;
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
        }
    }
}
