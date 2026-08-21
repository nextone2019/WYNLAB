using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout;

namespace NEXTFramework.UI.Common;

/// <summary>
/// 필수입력 항목을 화면에 표시하는 확장메서드 모음.
///
/// TextEdit, DateEdit, SpinEdit, ComboBoxEdit, MemoEdit 등은 전부 DevExpress의
/// BaseEdit를 공통 부모로 상속하기 때문에, 컨트롤 종류별로 따로 코드를 만들 필요 없이
/// 이 확장메서드 하나로 전부 커버된다.
///
/// 사용 예:
///   txtUserId.MarkRequired();                 // 입력창
///   groupAccount.AddItem("아이디", txtUserId).MarkRequired();  // LayoutControl 라벨까지 같이
///   gridColumnUserNm.MarkRequired();           // 그리드 헤더(인라인편집 그리드에서)
///
/// 색상은 전부 UiTheme(=appsettings.json의 Theme 섹션)에서 가져오므로, 회사별로
/// 배포판의 설정값만 바꾸면 전체 화면의 필수입력 스타일이 일괄로 바뀐다.
/// </summary>
public static class RequiredFieldExtensions
{
    /// <summary>입력 컨트롤(TextEdit/DateEdit/SpinEdit/ComboBoxEdit 등 BaseEdit 계열 전부)에 필수입력 스타일 적용</summary>
    public static T MarkRequired<T>(this T edit) where T : BaseEdit
    {
        edit.Properties.Appearance.BackColor = UiTheme.RequiredFieldBackColor;
        edit.Properties.Appearance.ForeColor = UiTheme.RequiredFieldForeColor;
        edit.Properties.Appearance.Options.UseBackColor = true;
        edit.Properties.Appearance.Options.UseForeColor = true;
        return edit;
    }

    /// <summary>그리드 컬럼 헤더에 필수입력 스타일 적용 (인라인 편집 그리드용)</summary>
    public static GridColumn MarkRequired(this GridColumn column)
    {
        column.AppearanceHeader.ForeColor = UiTheme.RequiredHeaderForeColor;
        column.AppearanceHeader.Options.UseForeColor = true;
        if (!column.Caption.EndsWith("*"))
        {
            column.Caption = $"{column.Caption} *";
        }
        return column;
    }

    /// <summary>LayoutControl 항목(라벨)에 필수입력 스타일 적용 - 등록/수정 팝업에서 사용</summary>
    public static LayoutControlItem MarkRequired(this LayoutControlItem item)
    {
        item.AppearanceItemCaption.ForeColor = UiTheme.RequiredHeaderForeColor;
        item.AppearanceItemCaption.Options.UseForeColor = true;
        if (!item.Text.EndsWith("*"))
        {
            item.Text = $"{item.Text} *";
        }
        return item;
    }
}
