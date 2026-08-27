using System.ComponentModel;
using DevExpress.XtraTreeList;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 메뉴관리(frmMenu.menuTree)/권한부여(frmUserManage.authTree 등) 화면이 각자 손으로 반복해서
/// 맞추던 트리 공통 스타일(그룹/leaf 행 배색, 줄 높이, 라인 옵션)을 뽑아낸 TreeList -
/// TabControlWyn을 ShellForm의 탭 그리기 로직에서 뽑아낸 것과 같은 방식이다.
///
/// 컬럼 구성(메뉴명 하나만 쓸지, 권한부여처럼 체크박스 컬럼 5개를 더 붙일지)은 화면마다 다르므로
/// 여기서 미리 정하지 않는다 - GridControlWyn이 컬럼을 미리 정하지 않는 것과 같은 이유. 이 컨트롤은
/// "그룹 행과 leaf 행을 시각적으로 구분한다"는 공통 규칙만 담당한다.
///
/// GROUP/leaf 구분은 데이터에 "MenuType" 필드가 있고 그 값이 "GROUP"인지로 판단한다(TSMMENU.MENU_TYPE
/// 규약과 동일) - 트리를 쓰는 화면은 전부 이 값을 이미 갖고 있어서(메뉴 자체를 다루거나, 메뉴 기준
/// 권한을 다루거나) 별도 매핑 없이 그대로 맞는다.
/// </summary>
[ToolboxItem(true)]
public class TreeListWyn : TreeList
{
    public TreeListWyn()
    {
        OptionsView.ShowIndicator = false;
        OptionsView.ShowHorzLines = false;
        OptionsView.ShowVertLines = false;
        RowHeight = 26; // 기본 행높이는 다소 빡빡해 보여서 살짝 여유를 줌(frmMenu.menuTree와 동일)
        Appearance.Row.Font = AppFonts.Body;

        NodeCellStyle += TreeListWyn_NodeCellStyle;
    }

    /// <summary>그룹(폴더, 클릭해도 화면이 안 열리거나 개별 권한이 없는 상위 분류) 행과 leaf 행을
    /// 배경/글자색으로 구분한다. 색은 UiTheme(appsettings.json Theme 섹션)에서 가져와서, 트리를
    /// 쓰는 화면이 늘어나도 전부 같은 톤을 재사용하게 한다(frmMenu.BuildLeftPanel의 원본 로직).</summary>
    private void TreeListWyn_NodeCellStyle(object? sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
    {
        var menuType = e.Node.GetValue("MenuType") as string;
        var isGroup = menuType == "GROUP";
        e.Appearance.BackColor = isGroup ? UiTheme.TreeGroupBackColor : UiTheme.TreeLeafBackColor;
        e.Appearance.ForeColor = isGroup ? UiTheme.TreeGroupForeColor : UiTheme.TreeLeafForeColor;
        e.Appearance.Font = isGroup ? AppFonts.BodyBold : AppFonts.Body;
        e.Appearance.Options.UseBackColor = true;
        e.Appearance.Options.UseForeColor = true;
        e.Appearance.Options.UseFont = true;
    }
}
