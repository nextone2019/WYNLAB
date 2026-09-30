using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 모든 그리드 공통 "컬럼 헤더 클릭 정렬" 설정(2026-10-01 지시 - WYNLAB 전체 그리드에 동일하게, 앞으로 만들어질 그리드도 마찬가지).
/// GridViewWyn/BandedGridViewWyn은 GridViewWynBehavior가 자동으로 부르고, DevExpress 기본 GridView를 직접 쓰는 곳(팝업 결과 그리드 등)은
/// 만든 직후 이 메서드를 한 줄 불러주면 같은 규칙이 된다.
///  - 뷰/모든 컬럼의 정렬을 명시적으로 켠다(헤더를 클릭하면 오름 -> 내림, Ctrl+클릭으로 해제, Shift+클릭으로 다중 컬럼 정렬은 DevExpress 기본 동작).
///  - 룩업 컬럼(코드가 이름으로 보이는 컬럼)은 코드값이 아니라 "화면에 보이는 이름" 기준으로 정렬한다 - 안 그러면 이름 순서로 안 보여 정렬이 안 되는 것처럼 보인다.
///  - 코드로 컬럼을 만드는 그리드(팝업 결과 그리드 등)도 데이터가 바인딩되는 시점에 같은 규칙이 적용된다.
/// </summary>
public static class GridSortSupport
{
    public static void Enable(GridView view)
    {
        view.OptionsCustomization.AllowSort = true;
        foreach (GridColumn column in view.Columns) ApplyToColumn(column);

        // 컬럼의 ColumnEdit(룩업 여부)이 Designer/코드에서 나중에 정해지는 경우를 위해 데이터가 (다시) 바인딩될 때도 다시 적용한다.
        view.DataSourceChanged += (s, e) =>
        {
            foreach (GridColumn column in view.Columns) ApplyToColumn(column);
        };

    }

    private static void ApplyToColumn(GridColumn column)
    {
        column.OptionsColumn.AllowSort = DefaultBoolean.True;
        if (column.ColumnEdit is RepositoryItemLookUpEditBase) column.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
    }
}
