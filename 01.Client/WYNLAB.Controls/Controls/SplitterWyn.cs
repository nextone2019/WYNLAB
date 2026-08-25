using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.Utils;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress SplitterControl 기반 - SplitContainerWyn(자기 안에 Panel1/Panel2를 갖고 있는
/// 컨테이너)과 달리, 이건 독립적으로 배치한 PanelWyn 여러 개 사이에 끼워서 드래그로 폭/높이를
/// 조절하는 순수 스플리터 바다(순정 WinForms Splitter와 같은 방식 - Dock=Left인 패널 뒤에
/// 이걸 Dock=Left로 놓고, 그 다음에 Dock=Fill인 패널을 놓는 순서).
///
/// 기본 모습은 "보이지 않는" 스플리터다. 예전엔 회색(UiTheme.DividerColor) 바탕에 DevExpress가
/// 그리는 그립 점(ShowSplitGlyph)까지 얹혀 굵은 회색 띠처럼 보였는데, 좌우 패널이 이미 각자
/// 카드 테두리를 갖고 있어서 그 사이에 또 색 띠가 들어가면 경계선이 두 겹이 되어 두껍고
/// 부자연스러웠다(실제 피드백). 지금은 배경을 주변과 같은 색(UiTheme.SplitterBackColor)으로 두고
/// 그립과 테두리를 꺼서, 구분은 카드 테두리가 하고 이건 드래그 기능만 담당한다.
/// 굳이 구분선을 보이게 하려면 appsettings.json의 Theme.SplitterBackColor를 바꾸면 된다.
/// </summary>
[ToolboxItem(true)]
public class SplitterWyn : DevExpress.XtraEditors.SplitterControl
{
    public SplitterWyn()
    {
        Width = 6; // 보이지 않아도 마우스로 잡기엔 충분한 폭
        BorderStyle = BorderStyle.None;
        ShowSplitGlyph = DefaultBoolean.False;
        Appearance.BackColor = UiTheme.SplitterBackColor;
        Appearance.Options.UseBackColor = true;
    }
}
