using System.ComponentModel;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace WYNLAB.Base.Controls;

/// <summary>
/// GridView를 상속해서 업무 그리드 화면마다 반복하던 표준 옵션을 기본값으로 미리 켜둔 컨트롤.
/// GridControl 자체는 그릇일 뿐이고 실제 동작(컬럼/옵션)은 GridView가 담당하므로, 커스텀
/// 대상은 GridControl이 아니라 이쪽이다 - GridControl은 툴박스에서 순정 그대로 끌어다 놓고
/// MainView만 이 클래스로 지정하면 된다.
///
/// 실제 동작(엑셀 붙여넣기/선택영역 합계/행번호/빈그리드 안내/미저장 강조/합계행 등)은 전부
/// GridViewWynBehavior에 있다 - BandedGridViewWyn과 로직을 공유하기 위함(클래스 설명 참고).
///
/// 컬럼 고정(Fixed Left/Right)은 GridColumn.Fixed 속성만으로 이미 순정 DevExpress에서
/// 바로 되기 때문에 여기서 따로 손댈 게 없다 - 화면별로 필요한 컬럼에 그냥 지정하면 된다.
/// </summary>
public class GridViewWyn : GridView
{
    private readonly GridViewWynBehavior _behavior;

    public GridViewWyn()
    {
        _behavior = new GridViewWynBehavior(this);
    }

    /// <summary>행번호 표시 - 기본은 꺼져있다(선택적). 켜면 왼쪽 인디케이터에 1, 2, 3... 순번이 표시된다.</summary>
    [Category("WYNLAB")]
    [Description("왼쪽 인디케이터에 행 번호(1, 2, 3...)를 표시합니다.")]
    [DefaultValue(false)]
    public bool ShowRowNumbers
    {
        get => _behavior.ShowRowNumbers;
        set => _behavior.ShowRowNumbers = value;
    }

    /// <summary>조회 결과가 0건일 때 그리드 가운데 보여줄 안내 문구.</summary>
    [Category("WYNLAB")]
    [Description("데이터가 없을 때 그리드 가운데 표시할 안내 문구.")]
    [DefaultValue("조회된 데이터가 없습니다.")]
    public string EmptyText
    {
        get => _behavior.EmptyText;
        set => _behavior.EmptyText = value;
    }

    /// <summary>아직 저장하지 않은 수정 셀을 배경색(필수입력과 동일 색)으로 강조할지 여부.</summary>
    [Category("WYNLAB")]
    [Description("아직 저장하지 않고 수정만 한 셀을 배경색으로 강조합니다.")]
    [DefaultValue(false)]
    public bool HighlightUnsavedCells
    {
        get => _behavior.HighlightUnsavedCells;
        set => _behavior.HighlightUnsavedCells = value;
    }

    /// <summary>지금 포커스된 행 전체를 배경색(UiTheme.GridFocusedRowBackColor)으로 강조할지 여부.
    /// CellSelect 모드에서도(셀 일부만 드래그 선택하는 것과 무관하게) 항상 적용된다.</summary>
    [Category("WYNLAB")]
    [Description("포커스된 행 전체를 배경색으로 강조합니다(CellSelect 모드에서도 동작).")]
    [DefaultValue(false)]
    public bool HighlightFocusedRow
    {
        get => _behavior.HighlightFocusedRow;
        set => _behavior.HighlightFocusedRow = value;
    }

    /// <summary>조회전용 그리드인지 입력/수정 가능한 그리드인지 - 셀 편집 가능 여부와
    /// EmbeddedNavigator의 추가/삭제/편집 버튼 노출을 한 번에 맞춘다(GridRoleWyn 참고).</summary>
    [Category("WYNLAB")]
    [Description("조회전용(Query) 그리드인지 입력/수정 가능(Edit)한 그리드인지 지정합니다. Query면 편집이 막히고 EmbeddedNavigator의 추가/삭제/편집 버튼도 숨겨집니다.")]
    [DefaultValue(GridRoleWyn.Query)]
    public GridRoleWyn Role
    {
        get => _behavior.Role;
        set => _behavior.Role = value;
    }

    /// <summary>EmbeddedNavigator의 추가(Append) 버튼을 가로챈 이벤트 - 구독해야만 버튼이
    /// 보이고, 클릭 시 여기 붙인 로직만 실행된다(DevExpress 기본 동작인 AddNewRow() 자동실행은
    /// 없다). Role=Query면 구독해도 버튼이 안 뜬다.</summary>
    public event EventHandler? RowAdd
    {
        add => _behavior.RowAdd += value;
        remove => _behavior.RowAdd -= value;
    }

    /// <summary>EmbeddedNavigator의 삭제(Remove) 버튼을 가로챈 이벤트 - RowAdd와 같은 규칙.</summary>
    public event EventHandler? RowDelete
    {
        add => _behavior.RowDelete += value;
        remove => _behavior.RowDelete -= value;
    }

    /// <summary>컬럼 헤더 우클릭 메뉴의 "레이아웃저장"/"레이아웃초기화"를 눌렀을 때 발생 -
    /// 실제 DB 저장/조회는 BaseForm이 담당한다(WYNLAB.Controls는 Session/ApiClient를 모른다).</summary>
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

    /// <summary>지금 배치를 "디자이너 원본"으로 기억해둔다 - BaseForm이 화면 Load 시 저장된
    /// 레이아웃을 복원하기 전에 반드시 먼저 호출해야 한다.</summary>
    public void CapturePristineLayout() => _behavior.CapturePristineLayout();
    public void RestorePristineLayout() => _behavior.RestorePristineLayout();
    public string SaveLayoutXml() => _behavior.SaveLayoutXml();
    public void RestoreLayoutXml(string xml) => _behavior.RestoreLayoutXml(xml);

    /// <summary>저장 성공 후 호출 - 수정 강조 표시를 전부 지운다.</summary>
    public void ClearDirtyMarks() => _behavior.ClearDirtyMarks();

    /// <summary>컬럼 하단에 항상 보이는 합계/평균 등을 추가한다(선택 영역 합계와 별개 - 이건 상시 표시).</summary>
    public void AddColumnSummary(GridColumn column, SummaryItemType type = SummaryItemType.Sum, string format = "{0:N0}") =>
        _behavior.AddColumnSummary(column, type, format);

    /// <summary>그룹핑된 그리드에서 그룹별 소계를 추가한다.</summary>
    public void AddGroupSummary(GridColumn column, SummaryItemType type = SummaryItemType.Sum, string format = "{0:N0}") =>
        _behavior.AddGroupSummary(column, type, format);

    /// <summary>엑셀로 내보내기 - 저장 위치를 사용자에게 물어보고 내보낸다.</summary>
    public void ExportToExcel(string? defaultFileName = null) => _behavior.ExportToExcel(defaultFileName);
}
