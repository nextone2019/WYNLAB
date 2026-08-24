using System.Drawing;
using DevExpress.Data;
using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
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

    public GridViewWynBehavior(GridView view)
    {
        _view = view;

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

        _view.PopupMenuShowing += OnPopupMenuShowing;
        _view.CustomDrawRowIndicator += OnCustomDrawRowIndicator;
        _view.CustomDrawEmptyForeground += OnCustomDrawEmptyForeground;
        _view.CellValueChanged += OnCellValueChanged;
        _view.RowCellStyle += OnRowCellStyle;
    }

    private void OnPopupMenuShowing(object? sender, PopupMenuShowingEventArgs e)
    {
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
        if (!HighlightUnsavedCells) return;
        if (!_dirtyCells.Contains((e.RowHandle, e.Column.FieldName))) return;

        e.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
        e.Appearance.Options.UseBackColor = true;
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
