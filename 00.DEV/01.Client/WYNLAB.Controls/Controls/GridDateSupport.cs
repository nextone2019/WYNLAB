using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 모든 그리드 공통 "일자 컬럼은 yyyy-MM-dd" 규칙(2026-10-01 지시). FieldName이 *_date인 컬럼을 일자 컬럼으로 본다
/// (시각이 필요한 *_dt는 제외). DateTime 값은 DisplayFormat/EditMask, "yyyyMMdd" 문자열은 표시 텍스트 변환으로 처리한다.
/// GridViewWynBehavior가 자동으로 부르므로 화면별 설정은 필요 없다.
/// </summary>
public static class GridDateSupport
{
    public const string Format = "yyyy-MM-dd";

    public static void Enable(GridView view)
    {
        Apply(view);
        view.DataSourceChanged += (s, e) => Apply(view);
        view.CustomColumnDisplayText += (s, e) =>
        {
            if (e.Column == null || !IsDateColumn(e.Column) || e.Column.ColumnEdit is RepositoryItemDateEdit) return;
            if (e.Value is string str && str.Length == 8 && DateTime.TryParseExact(str, "yyyyMMdd", null,
                    System.Globalization.DateTimeStyles.None, out var d))
                e.DisplayText = d.ToString(Format);
        };
    }

    private static bool IsDateColumn(GridColumn c)
        => c.FieldName != null && c.FieldName.EndsWith("_date", StringComparison.OrdinalIgnoreCase);

    private static void Apply(GridView view)
    {
        foreach (GridColumn c in view.Columns)
        {
            if (!IsDateColumn(c)) continue;
            if (c.ColumnEdit is RepositoryItemDateEdit de)
            {
                de.DisplayFormat.FormatType = FormatType.DateTime;
                de.DisplayFormat.FormatString = Format;
                de.EditFormat.FormatType = FormatType.DateTime;
                de.EditFormat.FormatString = Format;
                de.Mask.EditMask = Format;
            }
            else if (c.ColumnEdit == null)
            {
                c.DisplayFormat.FormatType = FormatType.DateTime;
                c.DisplayFormat.FormatString = Format;
            }
        }
    }
}
