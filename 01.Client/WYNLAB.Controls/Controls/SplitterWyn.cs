using System.ComponentModel;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress SplitterControl 기반 - SplitContainerWyn(자기 안에 Panel1/Panel2를 갖고 있는
/// 컨테이너)과 달리, 이건 독립적으로 배치한 PanelWyn 여러 개 사이에 끼워서 드래그로 폭/높이를
/// 조절하는 순수 스플리터 바다(순정 WinForms Splitter와 같은 방식 - Dock=Left인 패널 뒤에
/// 이걸 Dock=Left로 놓고, 그 다음에 Dock=Fill인 패널을 놓는 순서). 색상은 다른 구분선(PanelWyn
/// EdgeLine/TitleDivider)과 통일해서 UiTheme.DividerColor를 기본값으로 쓴다.
/// </summary>
[ToolboxItem(true)]
public class SplitterWyn : DevExpress.XtraEditors.SplitterControl
{
    public SplitterWyn()
    {
        Width = 4;
        Appearance.BackColor = UiTheme.DividerColor;
        Appearance.Options.UseBackColor = true;
    }
}
