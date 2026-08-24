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

    public void ClearDirtyMarks() => _behavior.ClearDirtyMarks();

    public void AddColumnSummary(GridColumn column, SummaryItemType type = SummaryItemType.Sum, string format = "{0:N0}") =>
        _behavior.AddColumnSummary(column, type, format);

    public void AddGroupSummary(GridColumn column, SummaryItemType type = SummaryItemType.Sum, string format = "{0:N0}") =>
        _behavior.AddGroupSummary(column, type, format);

    public void ExportToExcel(string? defaultFileName = null) => _behavior.ExportToExcel(defaultFileName);
}
