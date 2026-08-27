using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WYNLAB.Base.Controls;

/// <summary>
/// 독립적으로 배치한 PanelWyn 여러 개 사이에 끼워서 드래그로 폭/높이를 조절하는 순수
/// 스플리터 바(SplitContainerWyn과 달리 자기 안에 Panel1/Panel2를 갖지 않는다 - Dock=Left인
/// 패널 뒤에 이걸 Dock=Left로 놓고, 그 다음에 Dock=Fill인 패널을 놓는 순서).
///
/// [2026-08-27] 원래 DevExpress SplitterControl 기반이었는데, 드래그하는 동안 색이 이상하게
/// 번쩍이는("홀로그램 같다") 버그가 있었다(실제로 겪음). 우리 코드를 완전히 손 안 댄
/// 상태(배경색만 지정)로 되돌려도, 스킨을 WXI에서 Office 2019 Colorful로 바꿔도 그대로
/// 재현됐다 - 즉 우리 커스텀 그리기나 특정 스킨 탓이 아니라, SplitterControl 자체의 드래그
/// 미리보기 렌더링이 이 앱 환경에서 근본적으로 깨져 있다는 뜻이다(공개 API가
/// Appearance/ShowSplitGlyph 정도뿐이라 그 렌더링 자체를 고칠 방법도 없다).
/// 그래서 DevExpress를 아예 걷어내고 순정 WinForms Splitter로 바꿨다 - 스킨 시스템을 안
/// 타므로 이런 종류의 문제 자체가 생길 수 없고, 수십 년간 검증된 동작이라 신뢰할 수 있다.
///
/// 배경은 주변과 같은 색(UiTheme.SplitterBackColor)으로 둬서 좌우 패널의 카드 테두리가 이미
/// 있는 자리에 또 색 띠가 겹쳐 두꺼워 보이지 않게 한다 - 다만 그것만으로는 "여기가 드래그
/// 가능한 스플리터"라는 게 전혀 안 보인다는 피드백이 있었다(테두리도 없고 배경도 안 튀니
/// 완전히 안 보임). 그래서 가운데에 옅은 점 세 개(그립 표시)만 찍는다 - 이 커스텀 그리기는
/// DevExpress SplitterControl이 아니라 순정 Splitter 위에서 하는 거라, 위에서 겪은 드래그
/// 색상오염 버그(그 컨트롤 내부 드래그 미리보기 렌더러 자체의 결함)와는 무관하다.
/// </summary>
[ToolboxItem(true)]
public class SplitterWyn : Splitter
{
    public SplitterWyn()
    {
        Width = 6; // 점 세 개를 찍기엔 충분하고, 마우스로 잡기에도 충분한 폭
        BackColor = UiTheme.SplitterBackColor;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using var brush = new SolidBrush(ColorHelper.Adjust(BackColor, -70));

        const int dotSize = 3;
        const int gap = 3;
        var cx = Width / 2f;
        var cy = Height / 2f;
        // Dock=Left/Right로 쓰이면 폭이 좁고 길어서 점을 세로로, Dock=Top/Bottom이면 가로로 찍는다.
        var isVertical = Width <= Height;

        for (var i = -1; i <= 1; i++)
        {
            var x = isVertical ? cx : cx + i * (dotSize + gap);
            var y = isVertical ? cy + i * (dotSize + gap) : cy;
            g.FillEllipse(brush, x - dotSize / 2f, y - dotSize / 2f, dotSize, dotSize);
        }
    }
}
