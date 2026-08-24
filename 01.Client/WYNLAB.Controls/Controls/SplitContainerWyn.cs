using System.ComponentModel;

namespace WYNLAB.Base.Controls;

/// <summary>
/// DevExpress SplitContainerControl 기반 - 좌/우(또는 상/하) 두 패널을 스플리터로 나눠서
/// 사용자가 드래그로 폭을 조절할 수 있게 하는 컨테이너. 화면 좌측 리스트 + 우측 상세 같은
/// WYNLAB 표준 레이아웃(메모리 "Screen layout convention" 참고)에서 반복적으로 쓰인다.
/// 다른 Wyn 컨트롤들과 같은 하우스 스타일(테두리 없이 깔끔하게)을 기본값으로 맞춰뒀다.
/// </summary>
[ToolboxItem(true)]
public class SplitContainerWyn : DevExpress.XtraEditors.SplitContainerControl
{
    public SplitContainerWyn()
    {
        BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
    }
}
