using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;

namespace WYNLAB.Base;

/// <summary>
/// 필수입력 항목을 화면에 표시하는 확장메서드 모음.
///
/// 공통 규칙(전체 화면 통일):
/// - 라벨/그리드 헤더 텍스트로는 필수 여부를 표시하지 않는다 - 별표(*)나 빨간 글자 없이
///   평범한 기본 색 그대로 둔다.
/// - 필수 여부는 항상 "입력이 실제로 이루어지는 표면"의 배경색(연노랑, RequiredFieldBackColor)
///   하나로만 표시한다. TextEdit/DateEdit/SpinEdit/ComboBoxEdit/MemoEdit 등 BaseEdit 계열은
///   컨트롤 배경 전체가, 그리드는 값이 비어있는 셀만 같은 색으로 강조된다(항상 칠하면 필수
///   컬럼 전체가 노랗게 채워져 오히려 산만해지므로, "아직 안 채운 값"일 때만 강조).
///
/// TextEdit, DateEdit, SpinEdit, ComboBoxEdit, MemoEdit 등은 전부 DevExpress의
/// BaseEdit를 공통 부모로 상속하기 때문에, 컨트롤 종류별로 따로 코드를 만들 필요 없이
/// 이 확장메서드 하나로 전부 커버된다.
///
/// 사용 예:
///   txtUserId.MarkRequired();                                    // 입력창 자체
///   groupAccount.AddItem("아이디", txtUserId).MarkRequired();     // LayoutControl 라벨에 걸어도
///                                                                 // 내부적으로 txtUserId 쪽에 적용됨
///   gridView.Columns.Add(gridColumnUserNm);
///   gridColumnUserNm.MarkRequired();       // 컬럼을 View에 추가한 "다음"에 호출해야 함
///
/// 색상은 전부 UiTheme(=appsettings.json의 Theme 섹션)에서 가져오므로, 회사별로
/// 배포판의 설정값만 바꾸면 전체 화면의 필수입력 스타일이 일괄로 바뀐다.
/// </summary>
public static class RequiredFieldExtensions
{
    /// <summary>입력 컨트롤(TextEdit/DateEdit/SpinEdit/ComboBoxEdit 등 BaseEdit 계열 전부)에 필수입력 스타일 적용</summary>
    /// <summary>필수 표시를 되돌린다(Required=false) - 사용 플래그만 끄면 안 된다: 디자이너는 MarkRequired가 넣은 배경/글자색
    /// 값 자체를 .Designer.cs에 저장하고, 다시 열 때 그 값을 대입하면 사용 플래그가 자동으로 켜져서 노란색이 되살아난다
    /// (2026-09-25 - Required를 false로 바꿔도 색이 남던 원인). 색 값까지 비워서 저장할 것이 없게 한다.</summary>
    public static T ClearRequired<T>(this T edit) where T : BaseEdit
    {
        edit.Properties.Appearance.BackColor = System.Drawing.Color.Empty;
        edit.Properties.Appearance.ForeColor = System.Drawing.Color.Empty;
        edit.Properties.Appearance.Options.UseBackColor = false;
        edit.Properties.Appearance.Options.UseForeColor = false;
        return edit;
    }

    public static T MarkRequired<T>(this T edit) where T : BaseEdit
    {
        edit.Properties.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
        edit.Properties.Appearance.ForeColor = UiTheme.RequiredFieldForeColor;
        edit.Properties.Appearance.Options.UseBackColor = true;
        edit.Properties.Appearance.Options.UseForeColor = true;
        return edit;
    }

    /// <summary>
    /// 그리드 컬럼이 필수입력임을 표시 - 헤더는 그대로 두고, 해당 컬럼에서 값이 비어있는
    /// 셀만 입력 컨트롤과 같은 배경색으로 강조한다. 컬럼이 GridView.Columns에 추가된
    /// "다음"에 호출해야 한다(View가 연결되어 있어야 RowCellStyle을 걸 수 있음).
    /// </summary>
    public static GridColumn MarkRequired(this GridColumn column)
    {
        if (column.View is GridView view)
        {
            view.RowCellStyle += (s, e) =>
            {
                if (e.Column != column) return;
                var isEmpty = e.CellValue == null ||
                    (e.CellValue is string text && string.IsNullOrWhiteSpace(text));
                if (!isEmpty) return;

                e.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
                e.Appearance.Options.UseBackColor = true;
            };
        }
        return column;
    }

    /// <summary>
    /// LayoutControl 항목(라벨)에서 호출하는 편의 오버로드 - 라벨 자체는 스타일을 바꾸지 않고,
    /// 항목에 연결된 실제 입력 컨트롤(item.Control)에 위 MarkRequired&lt;T&gt;()를 그대로 적용한다.
    /// 등록/수정 화면에서 "groupX.AddItem(캡션, 컨트롤).MarkRequired()" 형태로 한 줄에 이어 쓰기 위함.
    /// </summary>
    public static LayoutControlItem MarkRequired(this LayoutControlItem item)
    {
        if (item.Control is BaseEdit edit)
        {
            edit.MarkRequired();
        }
        return item;
    }
}
