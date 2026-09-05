using System.ComponentModel;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace WYNLAB.Base.Controls;

/// <summary>
/// BandedGridView(컬럼 헤더를 여러 단으로 묶어서 보여주는 밴드형 그리드) 버전 - GridViewWyn과
/// 완전히 같은 기능 세트를 제공한다. 실제 동작은 전부 GridViewWynBehavior에 있고(BandedGridView도
/// GridView를 상속하므로 그대로 재사용 가능), 이 클래스는 속성/메서드를 그쪽으로 위임만 한다 -
/// 왜 GridViewWyn을 상속하지 않고 이렇게 별도로 만들었는지는 GridViewWyn.cs 클래스 설명 참고.
/// </summary>
public class BandedGridViewWyn : BandedGridView
{
    private readonly GridViewWynBehavior _behavior;

    public BandedGridViewWyn()
    {
        _behavior = new GridViewWynBehavior(this);
    }

    /// <summary>GridViewWyn.EndInit과 같은 이유 - 화면이 `.Role = ...`을 한 번도 명시적으로 안
    /// 건드려도 Query 잠금이 최소 한 번은 적용되게 한다.</summary>
    public override void EndInit()
    {
        base.EndInit();
        _behavior.EnsureRoleApplied();
    }

    [Category("WYNLAB")]
    [Description("왼쪽 인디케이터에 행 번호(1, 2, 3...)를 표시합니다.")]
    [DefaultValue(false)]
    public bool ShowRowNumbers
    {
        get => _behavior.ShowRowNumbers;
        set => _behavior.ShowRowNumbers = value;
    }

    [Category("WYNLAB")]
    [Description("데이터가 없을 때 그리드 가운데 표시할 안내 문구.")]
    [DefaultValue("조회된 데이터가 없습니다.")]
    public string EmptyText
    {
        get => _behavior.EmptyText;
        set => _behavior.EmptyText = value;
    }

    [Category("WYNLAB")]
    [Description("아직 저장하지 않고 수정만 한 셀을 배경색으로 강조합니다.")]
    [DefaultValue(false)]
    public bool HighlightUnsavedCells
    {
        get => _behavior.HighlightUnsavedCells;
        set => _behavior.HighlightUnsavedCells = value;
    }

    [Category("WYNLAB")]
    [Description("포커스된 행 전체를 배경색으로 강조합니다(CellSelect 모드에서도 동작).")]
    [DefaultValue(false)]
    public bool HighlightFocusedRow
    {
        get => _behavior.HighlightFocusedRow;
        set => _behavior.HighlightFocusedRow = value;
    }

    /// <summary>조회전용 그리드인지 입력/수정 가능한 그리드인지 - GridViewWyn.Role과 같다(GridRoleWyn 참고).</summary>
    [Category("WYNLAB")]
    [Description("조회전용(Query) 그리드인지 입력/수정 가능(Edit)한 그리드인지 지정합니다. Query면 편집이 막히고 EmbeddedNavigator의 추가/삭제/편집 버튼도 숨겨집니다.")]
    [DefaultValue(GridRoleWyn.Query)]
    public GridRoleWyn Role
    {
        get => _behavior.Role;
        set => _behavior.Role = value;
    }

    /// <summary>EmbeddedNavigator의 추가(Append) 버튼을 가로챈 이벤트 - GridViewWyn.RowAdd와 같다.</summary>
    public event EventHandler? RowAdd
    {
        add => _behavior.RowAdd += value;
        remove => _behavior.RowAdd -= value;
    }

    /// <summary>EmbeddedNavigator의 삭제(Remove) 버튼을 가로챈 이벤트 - GridViewWyn.RowDelete와 같다.</summary>
    public event EventHandler? RowDelete
    {
        add => _behavior.RowDelete += value;
        remove => _behavior.RowDelete -= value;
    }

    /// <summary>개인별 그리드 레이아웃 저장 - GridViewWyn과 같다.</summary>
    public event EventHandler? LayoutSaveRequested
    {
        add => _behavior.LayoutSaveRequested += value;
        remove => _behavior.LayoutSaveRequested -= value;
    }
    public event EventHandler? LayoutResetRequested
    {
        add => _behavior.LayoutResetRequested += value;
        remove => _behavior.LayoutResetRequested -= value;
    }

    public void CapturePristineLayout() => _behavior.CapturePristineLayout();
    public void RestorePristineLayout() => _behavior.RestorePristineLayout();
    public string SaveLayoutXml() => _behavior.SaveLayoutXml();
    public void RestoreLayoutXml(string xml) => _behavior.RestoreLayoutXml(xml);

    public void ClearDirtyMarks() => _behavior.ClearDirtyMarks();

    public void AddColumnSummary(GridColumn column, SummaryItemType type = SummaryItemType.Sum, string format = "{0:N0}") =>
        _behavior.AddColumnSummary(column, type, format);

    public void AddGroupSummary(GridColumn column, SummaryItemType type = SummaryItemType.Sum, string format = "{0:N0}") =>
        _behavior.AddGroupSummary(column, type, format);

    public void ExportToExcel(string? defaultFileName = null) => _behavior.ExportToExcel(defaultFileName);
}
