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

        // SplitterWyn과 같은 방식(그 클래스 설명 참고) - 배경을 주변과 같은 색으로 둬서
        // 스킨 기본값(색 있는 굵은 띠)이 안 보이게 하고, 글리프(가운데 점)만 켜서 "여기
        // 드래그할 수 있다"는 최소한의 표시만 남긴다. WXI 스킨으로 바꾼 뒤 스플리터가
        // 두껍고 색이 도드라져 보인다는 피드백으로 조정.
        Appearance.BackColor = UiTheme.SplitterBackColor;
        Appearance.Options.UseBackColor = true;
        ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
    }
}
